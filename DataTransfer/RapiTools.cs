using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using OpenNETCF.Desktop.Communication;
using System.Runtime.InteropServices;

using DataObjects;



namespace DataTransfer
{
  // Define aliases
  using BasicFileInfo = DataXfer.BasicFileInfo;
  using RAPIFileTime = OpenNETCF.Desktop.Communication.RAPI.RAPIFileTime;


  // Required to correct a bug with OSVERSIONINO when connecting to a WM 5.0 device
  [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
  public struct CEOSVERSIONINFO
  {
    internal int dwOSVersionInfoSize;
    public int dwMajorVersion;
    public int dwMinorVersion;
    public int dwBuildNumber;
    public PlatformType dwPlatformId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst=128)]
    public string szCSDVersion;
  }


  /// <summary>
  /// Summary description for RapiTools.
  /// </summary>
  public class RapiTools
  {
    // Required to correct a bug with OSVERSIONINO when connecting to a WM 5.0 device
    [DllImport("rapi.dll", CharSet=CharSet.Unicode, SetLastError=true)]
    internal static extern int CeGetVersionEx(ref CEOSVERSIONINFO lpVersionInformation);
    

    // This new version, using the revised data structure, fixed the problem with the WM5.0 devices.
    public static CEOSVERSIONINFO GetOSInfo(RAPI rapi)
    {
      CEOSVERSIONINFO verInfo = new CEOSVERSIONINFO();
      CeGetVersionEx(ref verInfo);
      return verInfo;
    }


    public static SYSTEM_INFO GetSystemInfo(RAPI rapi)
    {
      SYSTEM_INFO sysInfo = new SYSTEM_INFO();
      rapi.GetDeviceSystemInfo(out sysInfo);

      return sysInfo;
    }


    public static SYSTEM_POWER_STATUS_EX GetPowerInfo(RAPI rapi)
    {
      SYSTEM_POWER_STATUS_EX status = new SYSTEM_POWER_STATUS_EX();
      rapi.GetDeviceSystemPowerStatus(out status);

      return status;
    }


    public static long GetFileSize(RAPI rapi, string filename)
    {
      return rapi.GetDeviceFileSize(filename);
    }


    public static bool FileExists(RAPI rapi, string filename)
    {
      return rapi.DeviceFileExists(filename);
    }


    public static void StartApp(RAPI rapi, string filename, string parameters)
    {
      rapi.CreateProcess(filename, parameters);
    }


    
    /// <summary>
    /// The primary purpose of this method is to check whether Pocket Pollster is installed on the mobile device.
    /// But it's also sophisticated enough to correct a bad AppPath registry entry, looking in alternate locations
    /// for the presence of the executable.
    /// Note: It has so far been tested on a PPC with 1 storage card and 1 area of built-in storage.  Some assumptions
    ///       have been made about what other storage areas might be named by the device. (2005-09-08)
    /// </summary>
    /// <param name="rapi"></param>
    /// <returns>The path where the app's EXE is located.</returns>
    public static string GetAppPath(RAPI rapi)
    {
      string path = CFRegistry.GetAppPath();   // Checks the registry for the [apparent] existence of Pocket Pollster

      if (path != null)
      {
        // Though there may be a registry setting, this doesn't guarantee that the software is actually present.  So check this.
        path = Tools.EnsureFullPath(path);
        if (! rapi.DeviceFileExists(path + SysInfo.Data.Admin.AppFilename))
        {
          path = null;

          // The EXE is not where it's supposed to be.  See if we can find it.
          foreach (string folder in GetStorageLocations(rapi, false))
          {
            string searchPath = "";
            if (folder == @"\")                      // If main memory then look in \Program Files; otherwise directly in root of the storage medium
              searchPath = @"\Program Files\";
            else
              searchPath = @"\" + folder + @"\";

            searchPath = searchPath + SysInfo.Data.Admin.AppName + @"\";

            if (rapi.DeviceFileExists(searchPath + SysInfo.Data.Admin.AppFilename))
            {
              path = searchPath;
              CFRegistry.SetAppPath(path);
              break;
            }
          }

          // If the app wasn't found then clear these critical registry values.
          if (path == null)
          {
            CFRegistry.SetAppVersion("");
            CFRegistry.SetAppPath("");
            CFRegistry.SetDataPath("");
          }
        }
      }

      return path;
    }

    
    
    /// <summary>
    /// Every mobile device has 1 or more places where data & programs can be stored.  At the very least this includes
    /// main memory, but it can potentially have 1 or more removable storage cards and 1 or more built-in storage areas.
    /// This method examines the device to see what's currently available.
    /// </summary>
    /// <param name="rapi"></param>
    /// <param name="forDisplay"></param>  // This parameter simply determines how Main Memory will be stored in the return arraylist
    /// <returns>An array of the storage locations available.</returns>
    public static ArrayList GetStorageLocations(RAPI rapi, bool forDisplay)
    {
      // This procedure utilizes the fact that the RAPIFileAttributes enum contains these values:
      //   16 - Directory
      //  256 - Temporary
      const int tempDir = 272;

      ArrayList locationList = new ArrayList();    // Initialize the array we're going to pass back
      
      if (forDisplay)
        locationList.Add("Main Memory");
      else
        locationList.Add("\\");
      
      // Now from the root directory, see what else is present.
      FileList folderList = rapi.EnumFiles("\\*.*");

      foreach (FileInformation folderInfo in folderList)
      {
        string folder = folderInfo.FileName;
        if ((folderInfo.FileAttributes & tempDir) == tempDir)  // Only add those items that are both Directories
          locationList.Add(folder);                            // and "Temporary" - ie. storage card & built-in storage.
      }

      return locationList;
    }


    #region CopyFileToDevice
    /// <summary>
    /// Testing of rapi.CopyFileToDevice revealed that if the connection is broken that a subsequent attempt, without first shutting
    /// down this application, will cause rapi.CopyFileToDevice to fail because of a locking problem with the source file.
    /// This method resolves this problem.  It's also possible that the destination file already exists and is locked.
    /// 
    /// Note: Code has been introduced to explicitly set the time of the copied file to be identical to that on the desktop
    ///       but it doesn't seem to work correctly, with the time equivalent to GMT-8.  This must be a bug within RAPI.
    /// </summary>
    /// <param name="rapi"></param>
    /// <param name="srcPath"></param>         // The path on the desktop
    /// <param name="fileName"></param>
    /// <param name="destPath"></param>        // The path on the mobile device
    /// <param name="ExitMsgShown"></param>    // A flag in DataXfer that prevents multiple message boxes from being shown
    /// <returns></returns>
    public static bool CopyFileToDevice(RAPI rapi, string srcPath, string fileName, string destPath, ref bool ExitMsgShown)
    {
      srcPath = Tools.EnsureFullPath(srcPath);
      destPath = Tools.EnsureFullPath(destPath);
      string tmpPath = SysInfo.Data.Paths.Temp;   // {Temp Directory} + AppFilename

      try
      {
        DateTime dateTimeInfo = File.GetLastWriteTime(srcPath + fileName);

        if (CheckSourceFileAccess(srcPath + fileName))
        {
          // There appear to be no problems copying file directly
          rapi.CopyFileToDevice(srcPath + fileName, destPath + fileName, true);      // Copy file to device, overwriting if necessary
        }
        else   // File is definitely locked (to RAPI copy function) so use more complex approach
        {
          // First see if we can just copy the locked file directly to Temp directory
          if (! File.Exists(tmpPath + fileName))
          {
            File.Copy(srcPath + fileName, tmpPath + fileName);
            rapi.CopyFileToDevice(tmpPath + fileName, destPath + fileName, true);    // Copy file to device, overwriting if necessary
          }
          else   // This file is locked too so find a free filename we can use
          {
            int sepCharPos = fileName.LastIndexOf(".");
            string origName = fileName.Substring(0, sepCharPos);
            string origExt = fileName.Substring(sepCharPos);
            string newName = "";

            int i = 1;
            do
            {
              if (!File.Exists(tmpPath + origName + i.ToString() + origExt))
                newName = origName + i.ToString() + origExt;
              else
                i++;
            } while (newName == "");

            File.Copy(srcPath + fileName, tmpPath + newName);

            if (rapi.DeviceFileExists(destPath + fileName))
              rapi.DeleteDeviceFile(destPath + fileName);

            rapi.CopyFileToDevice(tmpPath + newName, destPath + fileName, true);    // Copy file to device, overwriting if necessary
          }
        }

        // If there's an error setting the device filetime then handle separately
        try
        {
          // Note: One would think that these next 2 statements would set the correct time of the file on the mobile device but they don't appear to. :-(
          //       The datestamp on the device file seems to be in a different time zone.
          rapi.SetDeviceFileTime(destPath + fileName, RAPI.RAPIFileTime.CreateTime, dateTimeInfo);
          rapi.SetDeviceFileTime(destPath + fileName, RAPI.RAPIFileTime.LastModifiedTime, dateTimeInfo);
        }
        catch (Exception e)
        {
          Debug.WriteLine("Error setting device filetime.  Message: " + e.Message);
        }
      }

      catch (Exception e)
      {
        if (! ExitMsgShown)
        {
          ExitMsgShown = true;
          string msg = "Error copying file to mobile device" + "\n" + e.Message;
          msg += "\n\nSource Path: " + srcPath;
          msg += "\nDest Path: " + destPath;
          msg += "\nFilename: " + fileName;
          msg += "\n\nPlease reconnect and try again!";

          Debug.WriteLine(msg);
          Tools.ShowMessage(msg, SysInfo.Data.Admin.AppName);
        }
        return false;
      }

      return true;
    }

    public static bool CopyFileToDevice(RAPI rapi, string srcPath, string fileName, string destPath)
    {
      bool tmp = false;
      return CopyFileToDevice(rapi, srcPath, fileName, destPath, ref tmp);
    }

    
    /// <summary>
    /// Checks whether the specified file is locked.  Note: This method doesn't
    /// use RAPI but very much belongs in the 'CopyFileToDevice' set of methods.
    /// </summary>
    /// <param name="srcFullPath"></param>
    /// <returns>true - access okay      false - file is locked</returns>
    private static bool CheckSourceFileAccess(string srcFullPath)
    {
      if (! File.Exists(srcFullPath))
        return false;

      try
      {
        FileStream stream = new FileStream(srcFullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        stream.Close();
        stream = null;
      }

      catch (IOException e)   // File is locked.  This error will be accounted for in 'CopyFileToDevice'
      {
        Debug.WriteLine(e.Message);
        return false;
      }

      catch (Exception e)
      {
        Debug.Fail(e.Message, "DataXfer.CheckSourceFileAccess");
        return false;
      }

      return true;
    }


    /// <summary>
    /// Sets a file's attributes to 'Hidden'.
    /// </summary>
    /// <param name="rapi"></param>
    /// <param name="filePath"></param>
    public static void HideFile(RAPI rapi, string filePath)
    {
      rapi.SetDeviceFileAttributes(filePath, RAPI.RAPIFileAttributes.Hidden);
    }

    #endregion


    #region CopyFilesFromDevice

    /// <summary>
    /// Copies a file from the connected mobile device to the desktop and ensures its DateTime stamp is identical to that on the mobile device.
    /// 
    /// Debug: I'm not sure why, but the 3 types of DateTime properties are not working correctly.  They always refer to the current day.
    /// </summary>
    /// <param name="rapi"></param>
    /// <param name="srcFile"></param>
    /// <param name="destFile"></param>
    /// <returns></returns>
    public static bool CopyFileFromDevice(RAPI rapi, string srcFile, string destFile)
    {
      try
      {
        DateTime dateTimeInfo = rapi.GetDeviceFileTime(srcFile, RAPI.RAPIFileTime.LastAccessTime);
//        DateTime dateTimeInfo2 = rapi.GetDeviceFileTime(srcFile, RAPI.RAPIFileTime.LastModifiedTime);
//        DateTime dateTimeInfo3 = rapi.GetDeviceFileTime(srcFile, RAPI.RAPIFileTime.CreateTime);
        rapi.CopyFileFromDevice(destFile, srcFile, true);
        File.SetLastWriteTime(destFile, dateTimeInfo);
      }

      catch (Exception e)
      {
        Debug.WriteLine("Couldn't copy mobile file: " + srcFile);
        Debug.WriteLine("Error Message: " + e.Message);
        
        return false;
      }

      return true;
    }


    /// <summary>
    /// Copies 1 or more files from the connected mobile device to a folder on the desktop.
    /// </summary>
    /// <param name="rapi"></param>
    /// <param name="srcFile"></param>
    /// <param name="destFile"></param>
    /// <returns></returns>
    public static bool CopyFilesFromDevice(RAPI rapi, string srcPath, string fileFilter, string destPath)
    {
      string srcFile = "";

      try
      {
        srcPath = Tools.EnsureFullPath(srcPath);
        destPath = Tools.EnsureFullPath(destPath);
        FileList filesToCopy = GetFolderContents(rapi, srcPath, fileFilter);

        if (filesToCopy != null)
          foreach (FileInformation fileInfo in filesToCopy)
          {
            srcFile = fileInfo.FileName;
            string destFile = destPath + srcFile;
            srcFile = srcPath + srcFile;
            CopyFileFromDevice(rapi, srcFile, destFile);
          }
      }

      catch (Exception e)
      {
        Debug.WriteLine("Couldn't copy mobile file: " + srcFile);
        Debug.WriteLine("Error Message: " + e.Message);
        
        return false;
      }

      return true;
    }

    #endregion


    #region CopyFileOnDevice

    public static bool CopyFileOnDevice(RAPI rapi, string srcFile, string destFile)
    {
      if (srcFile == destFile)
        return false;

      try
      {
        rapi.CopyFileOnDevice(srcFile, destFile, true);
      }

      catch (Exception e)
      {
        Debug.WriteLine("Couldn't copy file on mobile device: " + srcFile);
        Debug.WriteLine("Error Message: " + e.Message);
        
        return false;
      }

      return true;
    }

    #endregion
    


    public static bool DeleteFileOnDevice(RAPI rapi, string path, string filename)
    {
      path = Tools.EnsureFullPath(path);

      try
      {
        if (rapi.DeviceFileExists(path + filename))
          rapi.DeleteDeviceFile(path + filename);
      }

      catch (Exception e)
      {
        Debug.WriteLine("Couldn't delete file on mobile device: " + path + filename);
        Debug.WriteLine("Error Message: " + e.Message);
        
        return false;
      }

      return true;
    }


    /// <summary>
    /// Simply makes sure that the "\Temp" folder in main memory exists on the connected mobile device.  This
    /// folder is where the CAB setup files are temporarily copied to before they are executed and then erased.
    /// </summary>
    /// <param name="rapi"></param>
    /// <param name="ExitMsgShown"></param>
    /// <returns></returns>
    public static bool TempFolderExists(RAPI rapi, ref bool ExitMsgShown)
    {
      try
      {
        if (! rapi.DeviceFileExists(@"\Temp"))
          rapi.CreateDeviceDirectory(@"\Temp");
      }
      catch
      {
        if (! ExitMsgShown)
        {
          ExitMsgShown = true;
          Tools.ShowMessage(@"Can't create '\Temp' directory on the mobile device.  Please reset device and try again.", "Error with Mobile Device");
        }
        return false;
      }

      return true;
    }


    
    /// <summary>
    /// Ensures that the necessary data folders on the mobile device exist, creating them if necessary.
    /// </summary>
    /// <param name="rapi"></param>
    /// <param name="rootPath"></param>
    public static void CreateDataFolders(RAPI rapi, string rootPath)
    {
      // Note: Cannot have a trailing backslash when using 'CreateDeviceDirectory'

      try
      {
        // 'rootPath' may have been passed with or without a "Data\" suffix or may just have a "\" suffix
        // Either way, we need to have just the bare root folder name before we can move forward
        if (rootPath.EndsWith(@"\"))
          rootPath = rootPath.Substring(0, rootPath.Length - 1);
        if (rootPath.ToLower().EndsWith(@"data"))
          rootPath = rootPath.Substring(0, rootPath.Length - 4);
        if (rootPath.EndsWith(@"\"))
          rootPath = rootPath.Substring(0, rootPath.Length - 1);

        if (! rapi.DeviceFileExists(rootPath))
          rapi.CreateDeviceDirectory(rootPath);
      
        string dataPath = rootPath + @"\Data";

        if (! rapi.DeviceFileExists(dataPath))
          rapi.CreateDeviceDirectory(dataPath);
        
        if (! rapi.DeviceFileExists(dataPath + @"\Completed"))
          rapi.CreateDeviceDirectory(dataPath + @"\Completed");
        
        if (! rapi.DeviceFileExists(dataPath + @"\Templates"))
          rapi.CreateDeviceDirectory(dataPath + @"\Templates");
      }

      catch (Exception e)
      {
        if (e.Message.Trim().ToLower() == "could not create directory")
        {
          string msg = "Could not create data folders for some unknown reason.\n\nPlease reset your mobile device and try again.";
            Tools.ShowMessage(msg, "Error Creating Data Folders");
        }
        else
          Tools.ShowMessage(e.Message, "Error Creating Data Folders");
      }
    }



    /// <summary>
    /// Retrieves the list of files in a specified folder.  Returns this information:
    ///   - Filename
    ///   - File size
    ///   - File date & time (various types)
    /// </summary>
    /// <param name="rapi"></param>
    /// <param name="path"></param>
    /// <param name="fileFilter"></param>
    /// <returns></returns>
    public static FileList GetFolderContents(RAPI rapi, string path, string fileFilter)
    {
      path = Tools.EnsureFullPath(path);
      
      if ((fileFilter == null) || (fileFilter == ""))
        fileFilter = "*.*";

      return rapi.EnumFiles(path + fileFilter);
    }


    public static SortedList GetFolderContentsSortedList(RAPI rapi, string path, string fileFilter)
    {
      SortedList fileList = new SortedList();
      FileList folderContents = GetFolderContents(rapi, path, fileFilter);

      if (folderContents == null || folderContents.Count == 0)
        return null;

      else
      {
        foreach (FileInformation fileInfo in folderContents)
        {
          fileList.Add(fileInfo.FileName, fileInfo);
        }
      }

      return fileList;
    }




  }
}
