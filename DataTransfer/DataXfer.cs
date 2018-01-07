using System;
using System.IO;
using System.Data;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenNETCF.Desktop.Communication;

using DataObjects;


namespace DataTransfer
{
  // Define aliases
  using InstallMode = DataObjects.Constants.InstallationMode;    // Ref to enum that contains different installation modes
  using ExportFormat = DataObjects.Constants.ExportFormat;       // Ref to enum that contains available export formats
  using PurgeDuration = DataObjects.Constants.PurgeDuration;     // Ref to enum that contains assorted purge durations
  using DataXferSpeed = DataObjects.Constants.DataXferSpeed;



	/// <summary>
	/// Summary description for DataXfer.
	/// </summary>
	public class DataXfer
	{
    private static frmNotify statusForm;                         // Provides a modular-level reference to the Data Transfer status form
    private static RAPI rapi;                                    // Module-level variable allows easy sharing of RAPI object provided by calling form
    private static DateTime startTime;                           // Set to the time when the connection is first established
    private static DateTime lastBatteryCheck;                    // Used to check the battery less frequently than every second
    private static string Guid;                                  // The GUID value of the connected mobile device

    private static System.Timers.Timer dataXferTimer;            // Primary timer
    private static System.Timers.Timer copyFileTimer;            // Secondary timer solely used to update ProgressBar
 
    private static FileCopyInfo copyFileStatus;
    private static string MobileAppPath;                         // The location where the mobile app resides
    private static string MobileDataPath;                        // The location of the mobile data folders
    private static string MobileUserName;                        // The UserName of the [registered] primary user of the mobile device
    private static bool DisconnectMsgShown = false;
    private static bool ExitMsgShown = false;                    // Used to ensure that no more than one exit message is shown
    private static bool OneTimeDeviceInfoShown = false;          // Set to true once the mobile device id is displayed; to avoid calling the method unnecessarily
    //private static bool MobileInitStarted = false;               // Set to true once the mobile PP.exe is started in Initialization mode


    // Constructor
    public DataXfer()
    {      
    }


    // Contains data that is simply used to display in the Notification window.
    private class FileCopyInfo
    {
      public string FileName;
      public long FileSize;        // Bytes - Sometimes this is accurately provided by RAPI, other times it is a correlated estimate
      public long TotalSize;       // Bytes
      public int CopyTime;         // Seconds
      public int NextAdjTime;      // Seconds  - The next time we'll perform a RAPI check of the actual filesize
      public int NextAdjCounter;   // A simple counter: 0, 1, 2, ...
      public double CopyRate;      // Bytes per second
      public int LastPercent;      // % - Keeps track of last percentage displayed, so we don't have %'s jumping around when new actual data is available
    }

    // Used to hold info about files that are copied to/from the mobile device.
    public class BasicFileInfo
    {
      public string FileName;
      public long FileSize;
      public DateTime DateInfo;
    }


    /// <summary>
    /// This method is called to initiate Data Transfer.  It contains the bulk of the data transfer code.
    /// 
    /// Note: Certain "steps" are referred to within this method.  They refer to those
    ///       outlined in the Pocket Pollster Design Specs document (MS Word).
    /// </summary>
    /// <param name="rapiObj"></param>           // A reference to the RAPI object in frmMain
    /// <param name="parentForm"></param>        // A reference to frmMain, from which we use just the Size & Location
    /// <param name="statusFormObj"></param>     // A reference to frmNotify
    public static void Start (ref RAPI rapiObj, Form parentForm, frmNotify statusFormObj)
    {
//      if (Tools.IsAppExpired())
//        return;

      #region Initialization

      Debug.WriteLine("Data Transfer Initiated!");
      SendMonitorMessage("- Data Transfer Started -");

      startTime = System.DateTime.Now;
      lastBatteryCheck = startTime - System.TimeSpan.FromSeconds(30);
      rapi = rapiObj;
      statusForm = statusFormObj;
      InstallMode installMode = InstallMode.NotSupported;  // Default value only (need to initialize so code will compile)

      CEOSVERSIONINFO osVersionInfo;
      string processorType;
      string PPpath = null;
      int numFiles;

      // Create timer
      dataXferTimer = new System.Timers.Timer(1000);       // 1 second interval
      dataXferTimer.AutoReset = true;
      dataXferTimer.Elapsed += new System.Timers.ElapsedEventHandler(MainTimerHandler);   // Add handler
      dataXferTimer.Start();

      // Create secondary timer, but only start when copying is started
      copyFileTimer = new System.Timers.Timer(1000);
      copyFileTimer.AutoReset = true;
      copyFileTimer.Elapsed += new System.Timers.ElapsedEventHandler(CopyFileTimerHandler);

      // Prepare to examine mobile device
      bool doInstall = false;
      bool beginXfer = false;
      bool delayedImport = false;

      #endregion

      // Wrap the entire data transfer process in a try-catch construct since
      // the disconnected nature of RAPI operations are prone to many errors.
      try
      {
        #region Preparation
        
        statusForm.ShowCurrentStatus("Examining mobile device . . .");
        SendMonitorMessage("Starting examination of mobile device.");
        
        // One or both of the following lines are failing with a Windows Mobile 5.0 device!!!
        osVersionInfo = RapiTools.GetOSInfo(rapi);
        processorType = Tools.DisallowNullString(GetProcessorType(osVersionInfo.dwMajorVersion));
        SendMonitorMessage("Mobile device's OS and processor type obtained.");

        if (processorType == null)               // First we need to see whether the PPC has a processor that we support
        {
          installMode = InstallMode.NotSupported;
          SendMonitorMessage("Processor not supported; displaying message box.");
          AskToInstall(installMode, parentForm, osVersionInfo);                                // Scenario 1 - Processor is not supported
        }
        else
        {
          SendMonitorMessage("Checking whether " + SysInfo.Data.Admin.AppName + " is installed on mobile device.");
          PPpath = RapiTools.GetAppPath(rapi);   // Checks whether PP is installed and if so, where

          if (PPpath == null)
          {
            installMode = InstallMode.New;                                                     // Scenario 2 - No footprint of PP on mobile device  -OR-
            SendMonitorMessage(SysInfo.Data.Admin.AppName + " is not installed on mobile device; displaying message box.");
            doInstall = AskToInstall(installMode, parentForm, osVersionInfo);                  // Scenario 3 - 'StopAskingToInstall' is set
          }
          else
          {
            SendMonitorMessage(SysInfo.Data.Admin.AppName + " appears to be installed, but confirming presence of " + SysInfo.Data.Admin.AppFilename);

            // Confirm presence of app's executable
            if (RapiTools.FileExists(rapi, PPpath + SysInfo.Data.Admin.AppFilename))
            {
              beginXfer = true;   // The EXE exists so we know a version of PP is installed.  Thus we can definitely begin the data transfer process.
              statusForm.ShowCurrentStatus(SysInfo.Data.Admin.AppName + " found on mobile device");
              SendMonitorMessage(SysInfo.Data.Admin.AppFilename + " was found on mobile device.");

              // See if we can retrieve the GUID value from the mobile device.  It's used when dealing with an existing User
              // but primarily used a bit further down below when creating a new Device.
              Guid = GetGuid();

              if (Guid == null)
                SendMonitorMessage("The app's GUID was not found.");
              else
                SendMonitorMessage("The app's GUID was found: " + Guid);

              // Try to retrieve the 'SysInfo.xml' file from the mobile device.  If it's available then we will:
              //   - Open up a CFSysInfo object and obtain from it the user's full name and user name
              //   - Pass this user info to the status form
              //   - See whether this user has yet been registered in SysInfo.Data.Users; if not then add new User object
              string cfSysInfoPath = RetrieveCFSysInfo(PPpath);   // This is the full path, incl. the filename
              if (cfSysInfoPath != null)
              {
                SendMonitorMessage("CFSysInfo was downloaded from mobile device.");
                OpenTempCFSysInfo(cfSysInfoPath);                 // Populate the CFSysInfo object with the data from the just copied file
                DisplayUserInfo(Tools.DisallowNullString(CFSysInfo.Data.MobileOptions.PrimaryUser), CFSysInfo.Data.MobileOptions.FirstName, CFSysInfo.Data.MobileOptions.LastName);
                SendMonitorMessage("Mobile device's user info obtained - Username: " + CFSysInfo.Data.MobileOptions.PrimaryUser + "   Fullname: " + CFSysInfo.Data.MobileOptions.FirstName + " " + CFSysInfo.Data.MobileOptions.LastName);

                // Examine the user info (if there is any) and handle it accordingly
                statusForm.ShowCurrentStatus(@"Examining user & device information . . .");
                string origPrimaryUser = CFSysInfo.Data.MobileOptions.PrimaryUser;
                SendMonitorMessage("Comparing username with known users in database.");
                string userName = UserMgr.ExamineUser(CFSysInfo.Data.MobileOptions.PrimaryUser, CFSysInfo.Data.MobileOptions.FirstName, CFSysInfo.Data.MobileOptions.LastName, Guid, false);

                // 2006-07-06 - Now that we're introducing the Summaries section, and dealing with it further below, we'll
                //              only save the CFSysInfo back to the mobile device once all of that work is done.
//                // Debug: Is this test sufficient to determine whether to copy back to device?
//                //        RW: 2005-10-13 - I think so but will leave these comments here in case
//                //        strange problems are encountered in the future.
//                if ((userName != null) && (userName != origPrimaryUser))
//                  SaveCFSysInfo(PPpath);   // Save the temporary CFSysInfo back to the mobile device [as SysInfo.xml]

                // We'll now retrieve the registered Primary User of this device, for use during the actual Data Transfer
                MobileUserName = Tools.DisallowNullString(CFSysInfo.Data.MobileOptions.PrimaryUser);
              }

              // We primarily need these two variables further on for comparison purposes but will need the first one if creating a new device object.
              string currVersion = CFRegistry.GetAppVersion();
              SendMonitorMessage("Retrieved current version number of installed mobile application: " + currVersion);
              string newVersion = GetAvailableMobileVersion();
              SendMonitorMessage("Retrieved version number of latest mobile application available: " + newVersion);

              if (Guid != null)
              {
                _Device device = SysInfo.Data.Devices.Find_Guid(Guid);
                if (device == null)                                
                {
                  // We would generally only call this method if this computer was seeing this mobile device for the very first time.
                  device = CreateNewDevice(Guid, osVersionInfo, currVersion, CFSysInfo.Data.MobileOptions.PrimaryUser);
                  SendMonitorMessage("Added new device to database.");
                }
                else   // The Device object was created previously but the User info was created subsequently
                {
                  string userName = Tools.DisallowNullString(CFSysInfo.Data.MobileOptions.PrimaryUser);
                  if ((userName != null) && (device.PrimaryUser != userName))
                    device.PrimaryUser = userName;
                }

                OneTimeDeviceInfoShown = DisplayOneTimeDeviceInfo(device);
              }


              // But is the EXE up to date?  We must compare version numbers to find out.
              // Note: An inherent assumption here is that we'll never provide an updated Compact Framework or
              //       OpenNET Compact Framework without also updating the version number of PP.exe.  So if we
              //       only need to update one of the framework packages then we'll just artificially increment
              //       the version number of PP.exe, as stored in MobileCabInfo.xml.
              statusForm.ShowCurrentStatus("Checking for available updates . . .");
              if (Tools.CompareVersionNumbers(currVersion, newVersion) == -1)
              {
                installMode = InstallMode.Update;
                SendMonitorMessage("New mobile version available; displaying message box.");
                doInstall = AskToInstall(installMode, parentForm, osVersionInfo, currVersion, newVersion);   // Scenario 5 - PP update available
              }
            }

            else   // PP.exe seems to be missing, though was previously installed
            {
              installMode = InstallMode.Reinstall;
              SendMonitorMessage("Mobile software is missing and needs to be reinstalled; displaying message box.");
              doInstall = AskToInstall(installMode, parentForm, osVersionInfo);                // Scenario 4 - PP was installed but reinstallation is required
            }
          }
        }

        #endregion

        // We're now ready to begin the mobile installation and/or begin data transfer.
        if (doInstall || beginXfer)
        {
          #region Installation

          if (doInstall)
          {
            //statusForm.ShowCurrentStatus("An update is available.  Starting installation . . .");
            if (PerformInstallation(installMode, processorType, osVersionInfo.dwMajorVersion))
            {
              statusForm.ShowCurrentStatus(SysInfo.Data.Admin.AppName + " installation was successful ! ! !");
              SendMonitorMessage(SysInfo.Data.Admin.AppName + " installation was successful.");

              beginXfer = true;

              // If the installation was successful then we can update the 'LastUpdate' and
              // 'SoftwareVersion' properties of the Device's object in 'SysInfo.Data.Devices'.

              // If this is the very first time that the app is ever being installed on the mobile device then
              // the variable 'Guid' will not yet be set.  So let's try to retrieve it now.
              if (Guid == null)
                Guid = GetGuid();   // Will create a new GUID, if one doesn't yet exist

              _Device device = SysInfo.Data.Devices.Find_Guid(Guid);
              if (device == null)
                device = CreateNewDevice(Guid, osVersionInfo, CFRegistry.GetAppVersion(), CFSysInfo.Data.MobileOptions.PrimaryUser);

              if (device != null)
              {
                device.LastUpdate = DateTime.Now;
                device.SoftwareVersion = CFRegistry.GetAppVersion();   // We actually have this info earlier but less overhead to just get it again now
              }

              if (! OneTimeDeviceInfoShown)
              {
                if (Guid == null)
                {
                  SendMonitorMessage("ERROR: Guid was not created.");
                  Debug.Fail("Incorrect assumption made about Guid being created!", "DataXfer.Start - Installation");
                }

                SendMonitorMessage("Retrieving device info from database via GUID.");
                device = SysInfo.Data.Devices.Find_Guid(Guid);
                OneTimeDeviceInfoShown = DisplayOneTimeDeviceInfo(device);
              }
            }
            else
            {
              SendMonitorMessage("ERROR: Installation did not complete successfully.");
              string msg = "Something went wrong during installation.  Please disconnect your";
              msg += "\nmobile device, restart it, and try installing again.";
              Tools.ShowMessage(msg, "Mobile Installation Failed");
            }
          }

          else   // No need to install so just set the module-level variables
          {
            SendMonitorMessage("Retrieving Data path from mobile device.");
            MobileAppPath = PPpath;
            statusForm.ShowCurrentStatus("Checking data folders on mobile device . . .");
            MobileDataPath = GetDataPath();
          }

          #endregion


          #region Transfer

          if (MobileUserName == null)
          {
            statusForm.ShowCurrentStatus("User info not entered yet!");
            SendMonitorMessage("User info on mobile device has not yet been entered.");
          }

          if (beginXfer)
          {
            Debug.WriteLine("Device GUID: " + Guid);
            statusForm.ShowCurrentStatus("Starting data transfer . . .");
            SendMonitorMessage("Installation operations are complete.  Starting data operations.");

            // Set some shortcut variables that we'll use during the transfer and processing of files
            string archivePath = SysInfo.Data.Paths.Archive;
            string dataPath = SysInfo.Data.Paths.Data;
            string incomingPath = SysInfo.Data.Paths.Incoming;
            string templatesPath = SysInfo.Data.Paths.Templates;
            string tempPath = SysInfo.Data.Paths.Temp;
            string appExt = Tools.GetAppExt();
            string appFilter = Tools.GetAppFilter();


            #region TransferFromDevice

            // We can only transfer files FROM a device if a username has been established on it.  This is pretty much a
            // double-check but provides a necessary one to prevent "anonymous" data from polluting the data repository.
            if (MobileUserName != null)
            {
              // Step #6: Copy files from mobile device to Incoming folder, performing some validation.
              try
              {
                // Get a list of the data files that are in the mobile "Data\Completed" folder
                FileList completedFiles = RapiTools.GetFolderContents(rapi, MobileDataPath + "Completed", appFilter);

                if (completedFiles == null)
                {
                  statusForm.ShowCurrentStatus("No new polls to download from mobile device.");
                  SendMonitorMessage("No new completed polls are available to be downloaded.");
                }
                else
                {
                  // Copy these files first to the Desktop's Temp folder.  Note: Not only will these files be used
                  // in this step but they'll also later be used in Steps #8 - 11
                  numFiles = completedFiles.Count;
                  SendMonitorMessage(numFiles.ToString() + " new completed files are available for download from mobile device.");

                  foreach (FileInformation fileInfo in completedFiles)
                  {
                    string mobileFile = MobileDataPath + @"Completed\" + fileInfo.FileName;
                    if (RapiTools.CopyFileFromDevice(rapi, mobileFile, tempPath + fileInfo.FileName))
                    {
                      SendMonitorMessage("  " + fileInfo.FileName + " was downloaded successfully.");
                    }
                    else
                    {
                      SendMonitorMessage("  ERROR: " + fileInfo.FileName + " could not be downloaded.");
                      string msg = "The following mobile file couldn't be copied:\n" + mobileFile;
                      msg = msg + "\n\nThis isn't necessarily a serious error as sometimes\n";
                      msg = msg + "files get locked.  After this process is complete\n";
                      msg = msg + "reset your mobile device & try downloading again.";
                      Tools.ShowMessage(msg, "Problem Copying File");
                      numFiles--;    // Decrement the file count
                    }
                  }

                  string statusMsg = Tools.PrepareCorrectTense(numFiles, "poll", "was", "were", true) + " downloaded.";
                  statusForm.ShowCurrentStatus(statusMsg);
                  SendMonitorMessage(statusMsg);

                  SendMonitorMessage("Copying downloaded files to 'Incoming' folder.");
                  // Now copy them to the "Incoming" folder, renaming them to this format:
                  //   "OriginalFilename_PollsterName_FinishPollingDate_#.pp" where "#" is an integer: 1, 2, 3, ...
                  foreach (FileInformation fileInfo in completedFiles)
                  {
                    if (File.Exists(tempPath + fileInfo.FileName))  // Extra check, in case file wasn't copied
                    {
                      string baseName = Tools.StripPathAndExtension(fileInfo.FileName);

                      // The mobile user may have collected multiple sets of the same poll data, placing them into separate files.
                      // These extras will be have a "~#" suffix, where # = 2, 3, ....  We must check for this and remove this [temporary] suffix.
                      int pos = baseName.IndexOf("~");
                      if (pos > -1)
                        baseName = baseName.Substring(0, pos);

                      string specificArchivePath = Tools.GetSpecificArchivePath(archivePath, baseName);

                      // When renaming, we were previously using the file's datetime stamp to get the last portion of the destination filename.
                      // But this isn't really correct because testing has shown that copying files does not generally leave the datetime the
                      // same as the source file.  So we must open up the file and retrieve the value of "PollingInfo.FinishPolling".
                      Poll tempPoll = new Poll();
                      Tools.OpenData(tempPath + fileInfo.FileName, tempPoll);
                      DateTime endDate = tempPoll.PollingInfo.FinishPolling;

                      // We'll do a quick check to ensure this date is correct.  If not then we'll just use today's date.
                      if (endDate.Year == 1)
                        endDate = DateTime.Now;

                      string longBaseName = baseName + "_" + MobileUserName + "_" + endDate.Year + "-" + Tools.Ensure2Digits(endDate.Month) + "-" + Tools.Ensure2Digits(endDate.Day) + "_";

                      bool nameTaken = false;
                      bool duplicateFile = false;
                      int i = 0;
                    
                      // We're now going to find a filename that has not yet been used, in order to successfully rename this file.
                      do
                      {
                        i++;

                        // Note: The last test in this next 'if' statement would only be true if a previous data transfer operation crashed.  
                        //       But the test's presence ensures that the file will not be lost or overwritten.
                        if (File.Exists(specificArchivePath + longBaseName + i.ToString() + appExt) || File.Exists(incomingPath + longBaseName + i.ToString() + appExt) || File.Exists(incomingPath + baseName + i.ToString() + appExt))
                        {
                          // Though there will be code later on to detect duplicate record entries, we're going to
                          // do a quick[er] file comparison check here now so as to remove any duplicate files.
                          string fileName = incomingPath + baseName + i.ToString() + appExt;
                          if (File.Exists(fileName))
                          {
                            duplicateFile = Tools.CompareFiles(tempPath + fileInfo.FileName, fileName);

                            if (! duplicateFile)
                            {
                              fileName = specificArchivePath + longBaseName + i.ToString() + appExt;
                              if (File.Exists(fileName))
                                duplicateFile = Tools.CompareFiles(tempPath + fileInfo.FileName, fileName);
                            }
                          }
                          nameTaken = true;
                        }
                        else
                        {
                          nameTaken = false;
                          longBaseName = longBaseName + i.ToString() + appExt;
                        }
                      } while (nameTaken && !duplicateFile);

                      if (! duplicateFile)
                      {
                        // Reaching here we now have a filename that does not exist in 'Data\Archive'.  So we can copy the file
                        // to 'Incoming', knowing full well that we can later move it to 'Archive' without any renaming issues.
                        // Debug: 2005-10-18 - RW - My intuition is telling me that, even with all the aggressive duplication
                        //                          checks above, there still are some scenarios when duplicate records might
                        //                          sneak past here.  I just can't think of them right now.
                        File.Copy(tempPath + fileInfo.FileName, incomingPath + longBaseName, true);
                        SendMonitorMessage("  '" + fileInfo.FileName + " was renamed to '" + longBaseName + " and successfully copied.");
                      }
                    }
                  }
                }
              }

              catch (Exception e)
              {
                Debug.Fail("Error Copying Files From Mobile Device\n\n" + e.Message, "DataXfer.Start - Transfer");
              }


              // Step #7: Import newly D/L data into master copies of data files
              // Note: We're going to gather the file information again, in case any unprocessed files are remaining
              //       from a previous session that was aborted or otherwise didn't finish successfully.
              try
              {
                // Get a list of the data files that are in the Data\Incoming folder
                string[] filesToProcess = Directory.GetFiles(incomingPath, appFilter);
                numFiles = filesToProcess.Length;

                if (numFiles > 0)
                {
                  statusForm.ShowCurrentStatus("Processing " + Tools.PrepareCorrectTense(numFiles, "poll") + " . . .");
                  SendMonitorMessage("Preparing to import " + Tools.PrepareCorrectTense(numFiles, "poll") + " downloaded files.");

                  foreach (string fullFilename in filesToProcess)
                  {
                    string fileName = Tools.GetOriginalFilename(fullFilename);
                    string specificArchivePath = Tools.GetSpecificArchivePath(archivePath, Tools.StripPathAndExtension(fileName));

                    if (!File.Exists(dataPath + fileName))
                    {
                      // No master copy of the poll yet exists so just copy into 'Data' (and archive too)
                      Tools.ExportBrandNewPoll(fullFilename);   // Check whether this file has its auto-export feature enabled; if so, then export its data too
                      File.Copy(fullFilename, dataPath + fileName);
                      File.Move(fullFilename, specificArchivePath + Tools.StripPath(fullFilename));
                      SendMonitorMessage("  No master copy of '" + fileName + "' exists so downloaded file becomes master copy.");
                    }

                    else
                    {
                      // Determine whether this file is currently being edited
                      if (! IsFileOpen(fileName))
                      {
                        // This file is NOT being edited so we can immediately populate it with the newly D/L data.
                        try
                        {
                          // But first we're going to backup the file in case something goes wrong with the Append operation
                          File.Copy(dataPath + fileName, dataPath + Tools.StripPathAndExtension(fileName) + ".bak", true);

                          bool appendOkay = Tools.AppendPoll(dataPath + fileName, fullFilename);

                          if (appendOkay)
                          {
                            // Now that we've successfully "imported" this new data, we can move the file from 'Incoming' to 'Archive'.
                            File.Move(fullFilename, specificArchivePath + Tools.StripPath(fullFilename));
                            SendMonitorMessage("  '" + fileName + "' was imported successfully.");
                          }

                          else   // Note: The append operation might not have happened for several reasons (Future: Provide notification of no import)
                          {
                            File.Delete(fullFilename);
                            SendMonitorMessage("  ERROR: Import of '" + fileName + "' did not succeed.");
                          }
                        }
                    
                        catch (Exception e)
                        {
                          Debug.Fail("Error appending new data into master poll: " + fileName + "\n\n" + e.Message, "DataXfer.Start - Transfer");
                          File.Copy(dataPath + Tools.StripPathAndExtension(fileName) + ".bak", dataPath + fileName, true);
                        }

                        finally  // Delete the backup file
                        {
                          File.Delete(dataPath + Tools.StripPathAndExtension(fileName) + ".bak");
                        }
                      }
                    
                      else  // This file IS being edited so we need to handle it in a different way.
                      {
                        delayedImport = true;  // Set flag, which we'll later check in Step #16
                        SendMonitorMessage("  '" + fileName + "' is being edited so import will be delayed.");
                      }
                    }
                  }
                }
              }

              catch (Exception e)
              {
                Debug.Fail("Error Importing Newly D/L Data\n\n" + e.Message, "DataXfer.Start - Transfer");
              }


      
              // Step #8: On the PPC, move the files in the 'Completed' folder into the 'Downloaded' folder.
              FileList prevDLFiles = null;    // For use in Step #9
              statusForm.ShowCurrentStatus("Examining completed polls . . .");

              try
              {
                string srcPath = MobileDataPath + @"Completed\";
                FileList completedFiles = RapiTools.GetFolderContents(rapi, srcPath, appFilter);
                string destPath = MobileDataPath + @"Downloaded\";
                prevDLFiles = RapiTools.GetFolderContents(rapi, destPath, appFilter);     
                string fileName = null;

                if (completedFiles != null)
                {
                  SendMonitorMessage("Completed files on mobile device being moved to 'Downloaded' folder.");

                  foreach (FileInformation fileInfo in completedFiles)
                  {
                    // First find a filename that does not yet exist in the "Downloaded" folder
                    fileName = fileInfo.FileName;
                    if (RapiTools.FileExists(rapi, destPath + fileName))
                    {
                      string baseName = Tools.StripExtension(fileName) + "~";
                      int i = 2;
                      do
                      {
                        fileName = baseName + i.ToString() + appExt;
                        if (RapiTools.FileExists(rapi, destPath + fileName))
                        {
                          fileName = null;
                          i++;
                        }
                      } while (fileName == null);
                    }

                    RapiTools.CopyFileOnDevice(rapi, srcPath + fileInfo.FileName, destPath + fileName);
                    RapiTools.DeleteFileOnDevice(rapi, srcPath, fileInfo.FileName);
                    SendMonitorMessage("  '" + fileInfo.FileName + "' was successfully moved.");

                    if (fileInfo.FileName != fileName)
                      if (File.Exists(tempPath + fileInfo.FileName))
                      {
                        if (File.Exists(tempPath + fileName))    // In the rare circumstance that somehow the resultant filename already
                          File.Delete(tempPath + fileName);      // exists then we'll delete it, b/c it's probably an old file.

                        File.Move(tempPath + fileInfo.FileName, tempPath + fileName);
                      }
                  }
                }
              }

              catch (Exception e)
              {
                Debug.Fail("Error Moving Files On Mobile Device From Completed To Downloaded Folder\n\n" + e.Message, "DataXfer.Start - Transfer");
              }



              // Step #9: Copy the older files in "Downloaded" (ie. pre Step #8) to the Desktop's "Temp" folder, thus providing
              //          a full mirror of what currently resides in the mobile device's "Downloaded" folder.
              try
              {
                if (prevDLFiles != null)
                {
                  SendMonitorMessage("Examining which files in 'Downloaded' folder of mobile device need to be purged.");
                  string srcPath = MobileDataPath + @"Downloaded\";
                  foreach (FileInformation fileInfo in prevDLFiles)
                  {
                    string fileName = fileInfo.FileName;
                    if (File.Exists(tempPath + fileName))    // In the rare circumstance that somehow the resultant filename already
                      File.Delete(tempPath + fileName);      // exists then we'll delete it, b/c it's probably an old file.

                    RapiTools.CopyFileFromDevice(rapi, srcPath + fileName, tempPath + fileName);
                  }
                }
              }

              catch (Exception e)
              {
                Debug.Fail("Error Copying Files From Downloaded Folder On Mobile Device To Desktop's Temp Folder\n\n" + e.Message, "DataXfer.Start - Transfer");
              }



              // Steps #10 & #11: Check the "PurgeDuration" property of each file that exists in the mobile device's "Downloaded" folder
              //                  to see if the file should be deleted.  We'll do this by examining the mirrored copy in the Desktop's
              //                  Temp folder.  Note: We technically have the list of files in two separate FileList variables above
              //                  but we'll just query the device again because it's such a quick process.  We'll also delete all of
              //                  the mirrored files in the Temp folder.
              try
              {
                string downloadedPath = MobileDataPath + @"Downloaded\";
                FileList fileList = RapiTools.GetFolderContents(rapi, downloadedPath, appFilter);

                if (fileList != null)
                {
                  foreach (FileInformation fileInfo in fileList)
                  {
                    if (File.Exists(tempPath + fileInfo.FileName))    // Double-check
                    {
                      Poll tempPoll = new Poll();
                      Tools.OpenData(tempPath + fileInfo.FileName, tempPoll);
                      //int numDays = tempPoll.CreationInfo.PurgeDuration;
                      int numDays = (int) tempPoll.CreationInfo.PurgeDuration;

                      bool okayToDelete = false;

                      switch (numDays)
                      {
                        case -1:   // Never delete this poll
                          break;

                        case 0:    // Delete immediately
                          okayToDelete = true;
                          break;

                        default:
                          System.TimeSpan duration = DateTime.Now - tempPoll.PollingInfo.FinishPolling;
                          if (duration.Days >= numDays)
                            okayToDelete = true;
                          break;
                      }

                      if (okayToDelete)
                      {
                        RapiTools.DeleteFileOnDevice(rapi, downloadedPath, fileInfo.FileName);
                        SendMonitorMessage("  '" + fileInfo.FileName + "' on mobile device was removed.");
                      }

                      // Delete mirrored file regardless of PurgeDuration settings
                      File.Delete(tempPath + fileInfo.FileName);
                    }
                  }
                }
              }

              catch (Exception e)
              {
                Debug.Fail("Error Purging No Longer Needed Files From The Downloaded Folder On The Mobile Device\n\n" + e.Message, "DataXfer.Start - Transfer");
              }
            }
            #endregion


            #region TransferToDevice

            // Step #12: Get a list of the data files that are in the mobile "Data\Templates" folder.
            //FileList mobileTemplates = null;
            statusForm.ShowCurrentStatus("Updating template files on mobile device . . .");
            SendMonitorMessage("Checking whether any template files need to be updated on mobile device.");

            // Note: We will be using the CFSysInfo object, which was populated earlier.
            try
            {
              // Before we enter the loop below, get a list of the existing Template files.
              // Note: This is the actual list of files present in the mobile 'Templates' folder.
              ArrayList existingTemplates = null;

              try
              {
                FileList mobileTemplates = RapiTools.GetFolderContents(rapi, MobileDataPath + "Templates", appFilter);
                existingTemplates = ConvertFileListToArrayList(mobileTemplates);
              }
              catch (Exception e)
              {
                Debug.Fail("Error Getting List of Template Files on Mobile Device\n\n" + e.Message, "DataXfer.Start - Transfer");
              }

              SendMonitorMessage(existingTemplates.Count.ToString() + " templates exist on mobile device.");

              // A bug in this section of code inadvertently allowed multiple copies of the same Template filename
              // to end up in the same CFSysInfo Summaries section.  This code ensures that only unique filenames appear.
              // Whether this double-check code can be removed in the future will have TBD.
              // Now examine the Templates.
              string mobileTemplateList = ",";   // Initialize list of template names
              for (int i = CFSysInfo.Data.Summaries.Templates.Count - 1; i >= 0; i--)
              {
                _TemplateSummary tempSummary = CFSysInfo.Data.Summaries.Templates[i];          // Get next template name
                if (Tools.ContainsSubstring(mobileTemplateList, tempSummary.Filename, true))   // See if this name already exists in the list
                  CFSysInfo.Data.Summaries.Templates.RemoveAt(i);                              // If so, then remove the item from Summaries
                else
                  mobileTemplateList += tempSummary.Filename + ",";                            // Otherwise add the yet unseen template name to the list
              }

              // Now we can go through the list of templates on the Desktop and compare them 
              // with what's on the mobile device, making adjustments where necessary.
              foreach(_TemplateSummary summary in SysInfo.Data.Summaries.Templates)
              {
                string filename = summary.Filename;  // w/o extension

                // Try to find the summary for the specified desktop-stored Template file
                _TemplateSummary summaryCF = CFSysInfo.Data.Summaries.Templates.Find_Template(filename);

                if (summaryCF != null)
                {
                  // The template has a Summary index record but it is possible that the file was inadvertently deleted by the user.
                  // So let's double-check that the physical file is actually present.
                  if (!existingTemplates.Contains(filename.ToLower() + appExt))
                  {
                    CFSysInfo.Data.Summaries.Templates.Remove(summaryCF);  // Remove old Template Summary record because the file is no longer present
                    summaryCF = null;
                  }
                }

                if (summaryCF == null)   // Template doesn't yet exist on mobile device so copy it to there
                {
                  RapiTools.CopyFileToDevice(rapi, templatesPath, filename + appExt, MobileDataPath + "Templates", ref ExitMsgShown);
                  SendMonitorMessage("  '" + filename + appExt + "' does not yet exist so was copied to device.");
                  CFSysInfo.Data.Summaries.Templates.Add(summary);   // Need to copy record from SysInfo to CFSysInfo
                }

                // Template does already exist on mobile device but the PollGuids are different, so what likely 
                // happened is that a poll of the same name was provided from a different desktop computer.
                else if (summary.PollGuid != summaryCF.PollGuid)
                {
                  // Need to copy template to mobile device but the filename already exists.
                  // So we will come up with a new, alternative filename - ie.  xxxxx~2 etc.
                  if (existingTemplates != null)   // Don't copy file if Rapi failed to return a file list
                  {  
                    string altFilename = Tools.GetAvailFilename(existingTemplates, filename);          // Will be returned with file extension
                    File.Copy(templatesPath + filename + appExt, templatesPath + altFilename, true);   // Make a copy of the file with the alternate filename
                    RapiTools.CopyFileToDevice(rapi, templatesPath, altFilename, MobileDataPath + "Templates", ref ExitMsgShown);   // Copy it to device
                    SendMonitorMessage("  '" + filename + appExt + "' does not yet exist but filename is taken so was renamed to '" + altFilename + "' and copied to device.");
                    File.Delete(templatesPath + altFilename);                                          // And then delete the temporary local copy
                    
                    _TemplateSummary templateSummary = new _TemplateSummary(summary);
                    summary.Filename = Tools.StripExtension(altFilename);
                    CFSysInfo.Data.Summaries.Templates.Add(summary); 
                  }
                }

                // The PollGuids are identical but the LastEditGuids are not.  So the version on the Desktop
                // is presumed to be newer and will this be used to overwrite the version currently on the PPC.
                else if (summary.LastEditGuid != summaryCF.LastEditGuid)
                {
                  RapiTools.CopyFileToDevice(rapi, templatesPath, filename + appExt, MobileDataPath + "Templates", ref ExitMsgShown);
                  SendMonitorMessage("  A newer version of '" + filename + appExt + "' was copied to device.");
                  summaryCF.LastEditGuid = summary.LastEditGuid;   // Just need to update LastEditGuid in Summaries index
                }
              }

              // All of the operations involving CFSysInfo are now complete, so we can
              // save the temporary CFSysInfo back to the mobile device [as SysInfo.xml].
              SaveCFSysInfo(PPpath);
              SendMonitorMessage("CFSysInfo was updated and copied back to mobile device.");
            }

            catch (Exception e)
            {
              Debug.Fail("Error Copying Template Files to Mobile Device\n\n" + e.Message, "DataXfer.Start - Transfer");
            }

            #endregion


            #region CheckTimeAndBattery

            // Step #14 - Check time on PPC with Desktop and correct PPC's time if required.

            // Note: With the approach we're using, there's guaranteed to be a few seconds lag time, but we can live with the minor error.
            statusForm.ShowCurrentStatus("Ensuring mobile device's time is correct . . .");
            SendMonitorMessage("Checking that mobile device's date & time are correct.");
            CFRegistry.SetNewTime();   // Store the Desktop's current date & time in the registry of the mobile device
            
            // This next line of code starts a special version of the Pocket PC.  If the time of the PPC
            // is less than 5 minutes difference from the Desktop then the time isn't changed.
            // Note: This also [currently] sets the GUID and Version number via the Mobile app, if they're not set/current.
            //       We may want to change this and make it exclusively set only the time!
            RapiTools.StartApp(rapi, MobileAppPath + SysInfo.Data.Admin.AppFilename, "SetTime");


            // Step #15: Examine final battery level and explicitly warn Desktop user if unit should be charged
            SendMonitorMessage("Making final battery check of mobile device in order to warn user if it's low.");
            SYSTEM_POWER_STATUS_EX powerStatus = RapiTools.GetPowerInfo(rapi);
            
            if (powerStatus.ACLineStatus == 0)
              if (powerStatus.BatteryLifePercent < SysInfo.Data.Options.Mobile.BatteryWarningLevel)
                Tools.ShowMessage("The battery level of this mobile device is only " + powerStatus.BackupBatteryLifePercent.ToString() + "%", "Recharge Mobile Device Immediately!");

            #endregion


            // Step #16: Inform user of newly D/L file(s) that belonged to a/some currently open file(s)
            if (delayedImport)
            {
              DisplayNewDataMessage(1);    // This initiates the "New Data Available" message in frmMain's statusbar
              SendMonitorMessage("Informed user that 1 or more new data files are available to be imported.");
            }

            // End of all Data Transfer steps.  Inform user and quietly leave.
            statusForm.ShowCurrentStatus("Data transfer successfully completed !");
            SendMonitorMessage("Data Transfer was completed successfully.");
          }

          #endregion
        }
      }

      catch (Exception e)  // This is the general 'Catch' clause for the entire Data Transfer code (except for Finalization)
      {
        if (! ExitMsgShown)
        {
          ExitMsgShown = true;
          Tools.ShowMessage(e.Message, "Data Transfer Error");
        }
      }

      #region Finalization

      // -------------------------------------------------------------------------------------------------------
      // The Data Transfer operation is now complete.  So finish up what we need to do and then exit the thread.

      try
      {
        // Stop timers and hide notification form before leaving
        string finalMsg = SysInfo.Data.Admin.AppName + " synchronization is now complete.";
        statusForm.ShowCurrentStatus(finalMsg);

        if (SysInfo.Data.Options.DataXfer.Sound)
        {
          Tools.Beep(5000,100);
          Tools.Beep(4000,100);
          Tools.Beep(3000,100);
        }

        dataXferTimer.Stop();
        dataXferTimer = null;
      
        copyFileTimer.Stop();
        copyFileTimer = null;

        rapi.Disconnect();    // This will inform code in frmMain.IconAnimationTimer_Tick to stop the animation

        // Just before ending this thread we will update the 'LastSync' property of the Device object.
        UpdateLastSyncProperty(Guid);


        // Determine whether to show message in Notification form or frmMain
        if (parentForm.WindowState != FormWindowState.Minimized)
        {
          DisplayStatusMessage((MobileUserName == null) ? finalMsg + "     UserName must still be defined on mobile device!" : finalMsg, true);
        }
        else
        {
          // Since we don't yet have the ability to make the Notify Form message flash, we'll just do it the "poor man's" way.
          for (int i = 0; i < 4; i++)
          {
            Thread.Sleep(1000);
            statusForm.ShowCurrentStatus("");
            Thread.Sleep(500);
            statusForm.ShowCurrentStatus(finalMsg);
          }
        }

        SendMonitorMessage("Hiding Data Transfer status window.");
        statusForm.Hide();
        statusForm.Close();
        SendMonitorMessage("- End of Data Transfer - ");
      }

      catch (Exception e)
      {
        ExitMsgShown = true;
        Tools.ShowMessage(e.Message, "Error Finalizing Data Transfer");
      }

      #endregion
    }

    
    
    /// <summary>
    /// This method will check for 2 kinds of possible disconnection:
    ///  1. SysInfo.Data.Admin.DataTransferActive == false
    ///  2. Rapi.Connected == false
    /// </summary>
    /// <returns>
    ///  true - Still connected
    ///  false - Disconnected at 1 or more levels (data transfer cannot continue)
    /// </returns>
    private static bool IsStillConnected()
    {
      if (SysInfo.Data.Admin.DataTransferActive && rapi.Connected)
        return true;

      else if (! DisconnectMsgShown)
      {
        DisconnectMsgShown = true;  // Prevent from entering this construct again (on this thread)
        
        if (SysInfo.Data.Options.DataXfer.Sound)
        {
          Tools.Beep(6000,100);
          Tools.Beep(5000,100);
          Tools.Beep(6000,100);
        }

        if (! ExitMsgShown)
        {
          ExitMsgShown = true;
          Tools.ShowMessage("The mobile device was prematurely disconnected.  Please reconnect and try again.", SysInfo.Data.Admin.AppName);
        }
      }

      return false;
    }


    #region Timers
    /// <summary>
    /// This is the primary timer that periodically monitors the data transfer operation and provides
    /// a status report back to the user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void MainTimerHandler (object sender, System.Timers.ElapsedEventArgs e)
    {
      DisplayDeviceInfo();
    }

   
    /// <summary>
    /// This is a secondary timer used to monitor the progress when a file is copied to a mobile device.
    /// It is fired every second.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void CopyFileTimerHandler (object sender, System.Timers.ElapsedEventArgs e)
    {
      if (IsStillConnected())
      {
        if (copyFileStatus == null)
          Debug.Fail("Incorrect assumption re creation of 'copyFileStatus' object!");

        int intervalTime = (int) copyFileTimer.Interval / 1000;
        copyFileStatus.CopyTime = copyFileStatus.CopyTime + intervalTime;
        long newCopyPercent = 0;    //Initialize

        if (copyFileStatus.CopyTime == copyFileStatus.NextAdjTime)
        {
          // Debug: Adding error-checking around this line, as WiFi connection with Dell Axim never gets to "double newCopyRate..." line!
          try
          {
            copyFileStatus.FileSize = RapiTools.GetFileSize(rapi, copyFileStatus.FileName);           // Get actual current filesize on mobile device

            double newCopyRate = ((double) copyFileStatus.FileSize) / copyFileStatus.CopyTime;
          
            //          // Update copy rate with latest data, but limit rate increases to 25%
            //          if (newCopyRate > (copyFileStatus.CopyRate * 1.25))
            //            copyFileStatus.CopyRate = copyFileStatus.CopyRate * 1.25;
            //          else
            //            copyFileStatus.CopyRate = newCopyRate;

            // The above construct doesn't work well with WiFi connected PPCs so we'll replace it with a simpler version
            copyFileStatus.CopyRate = newCopyRate;
            //Debug.WriteLine("New Copy Rate: " + newCopyRate.ToString());
            newCopyPercent = (100 * copyFileStatus.FileSize / copyFileStatus.TotalSize);
          }

          catch (Exception e2)
          {
            Debug.WriteLine(e2.Message);
          }

          switch(copyFileStatus.NextAdjCounter)
          {
            case 0:
              copyFileStatus.NextAdjTime = copyFileStatus.NextAdjTime + 3;
              break;
            case 1:
              copyFileStatus.NextAdjTime = copyFileStatus.NextAdjTime + 3;
              break;
            default:
              copyFileStatus.NextAdjTime = copyFileStatus.NextAdjTime + 30;
              break;
          }
          copyFileStatus.NextAdjCounter++;
        }

        else    // With no new filesize data, all we can do is estimate the filesize
        {
          long estFileSize = (long) (copyFileStatus.CopyRate * copyFileStatus.CopyTime);
          if (estFileSize > copyFileStatus.FileSize)
            copyFileStatus.FileSize = estFileSize;

          newCopyPercent = (100 * copyFileStatus.FileSize / copyFileStatus.TotalSize);
          if (newCopyPercent > 99)
            newCopyPercent = 99;
        }

        // Update percentage but don't let it every suddenly decrease just because we have new actual data available
        if (newCopyPercent > copyFileStatus.LastPercent)
          copyFileStatus.LastPercent = (int) newCopyPercent;
        else
          newCopyPercent = copyFileStatus.LastPercent;

        // And finally, display this percentage in the Notification form
        statusForm.ShowCopyProgress((int) newCopyPercent);
      }
    }

    #endregion


    /// <summary>
    /// Retrieves the location of where the data files are stored.
    /// Does some checking to ensure that the required directories exist.
    /// </summary>
    /// <returns>The path to the Data files</returns>
    private static string GetDataPath()
    {
      string dataPath = CFRegistry.GetDataPath();
      if (dataPath == null)
      {
        dataPath = MobileAppPath + @"Data\";
        CFRegistry.SetDataPath(dataPath);               // Ensure that the registry setting exists and/or is correct
      }
//      else
//        dataPath = Tools.EnsureFullPath(dataPath);
         
      RapiTools.CreateDataFolders(rapi, dataPath);      // Ensure that the data folders actually exist

      return dataPath;
    }



    /// <summary>
    /// Examines the contents of the embedded MobileCabInfo.xml to find out the current 
    /// version number of the PP.exe contained in the CAB setup package.
    /// Debug: At this time, this version number has to be manually updated whenever
    ///        a new PocketPC version is compiled.
    /// </summary>
    /// <returns></returns>
    private static string GetAvailableMobileVersion()
    {
      DataSet dataSet = GetMobileCabInfo();
      string version = null;

      DataTable table = dataSet.Tables["App"];
      if (table != null)
        version = table.Rows[0]["Version"].ToString();

      return version;
    }



    /// <summary>
    /// Opens the embedded 'MobileCabInfo.xml' file and copies the data therein into a DataSet.
    /// </summary>
    /// <returns></returns>
    private static DataSet GetMobileCabInfo()
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      Stream stream = assembly.GetManifestResourceStream("DataTransfer.MobileCabInfo.xml");
      DataSet dataSet = new DataSet();
      dataSet.ReadXml(stream);
      stream.Close();
  
      return dataSet;
    }


    private static ArrayList ConvertFileListToArrayList(FileList fileList)
    {
      ArrayList files = new ArrayList();

      if (fileList != null)
        foreach (FileInformation fileInfo in fileList)
        {
          files.Add(fileInfo.FileName.ToLower());    // Basic filename with extension but no path (need to test this)
        }

      return files;
    }


    #region AskToInstall

    /// <summary>
    /// Displays a dialog box, asking the user whether he wants to install Pocket Pollster onto the connected mobile device.
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="parentForm"></param>
    /// <param name="osVersionInfo"></param>
    /// <param name="currAppVersion"></param>
    /// <param name="newAppVersion"></param>
    /// <returns>true - Okay to proceed with installation</returns>
    private static bool AskToInstall (InstallMode mode, Form parentForm, CEOSVERSIONINFO osVersionInfo, string currAppVersion, string newAppVersion)
    {
      // We have a connected mobile device, to which one of the following scenarios applies:
      //  1. This device has a processor that is not supported by PP.
      //  2. This device is being examined by this app and has no PP "footmark" in its registry.
      //  3. This device has been examined by this app before but the user chose to exclude it from
      //      being asked for PP installations in the future.
      //  4. PP was previously installed but something has gone wrong with the application.
      //  5. A PP update is available for the mobile device.

      bool proceed = false;
      bool okayToInstall = false;
      string suffix = "";
      frmInstallPrompt frmInstall = new frmInstallPrompt();

      switch (mode)
      {
        case InstallMode.New:
          if (CFRegistry.InstallPromptOkay() || SysInfo.Data.Options.Mobile.OverrideInstallBlock)
          {
            frmInstall.SetMode(InstallMode.New, RapiTools.GetStorageLocations(rapi, true));
            suffix = "installation";
            proceed = true;
          }
          else
            DisplayStatusMessage("This mobile device has been previously set to forego installation.", false);
          break;

        case InstallMode.Update:
          frmInstall.SetMode(InstallMode.Update, currAppVersion, newAppVersion);
          suffix = "update";
          proceed = true;
          break;

        case InstallMode.Reinstall:
          frmInstall.SetMode(InstallMode.Reinstall, RapiTools.GetStorageLocations(rapi, true));
          suffix = "reinstallation";
          proceed = true;
          break;

        case InstallMode.NotSupported:
          if (CFRegistry.InstallPromptOkay())
          {
            frmInstall.SetMode(InstallMode.NotSupported);
            proceed = true;
          }
          break;
      }

      if (proceed)
      {
        if (SysInfo.Data.Options.Mobile.AutoUpdate && mode != InstallMode.NotSupported)
        {
          DisplayStatusMessage("AutoUpdate activated - Proceeding with mobile " + suffix + ".", false);
          okayToInstall = true;   // Proceed directly to install, bypassing dialog box
        }
        else
        {
          frmInstall.DisplayDeviceInfo(rapi, osVersionInfo);
          frmInstall.CenterForm(parentForm, parentForm.WindowState);
          statusForm.ShowCurrentStatus("");   // Clear msg because user needs to focus on dialog box that's about to appear
          DialogResult retval = frmInstall.ShowDialog();

          switch (retval)
          {
            case DialogResult.Yes:
              if (! frmInstall.AskNoMore)
                if (! CFRegistry.InstallPromptOkay())
                  CFRegistry.SetStopAskingToInstall(false);

              okayToInstall = true;
              break;

            case DialogResult.No:
              if (frmInstall.AskNoMore)
                CFRegistry.SetStopAskingToInstall(true);
              okayToInstall = false;
              break;

            case DialogResult.Cancel:
              okayToInstall = false;
              break;

            case DialogResult.OK:
              if (frmInstall.AskNoMore)
                CFRegistry.SetStopAskingToInstall(true);
              okayToInstall = false;
              break;
          }
        }
      }

      return okayToInstall;
    }

    private static bool AskToInstall (InstallMode mode, Form parentForm, CEOSVERSIONINFO osVersionInfo)
    {
      return AskToInstall(mode, parentForm, osVersionInfo, null, null);
    }

    #endregion


    #region PerformInstallation
    /// <summary>
    /// Copies the required CAB files to the connected mobile device and executes them.
    /// Version checking is done to see which CAB files, if any, need to be installed.
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="procType"></param>
    /// <param name="osMajorVersion"></param>
    /// <returns></returns>
    private static bool PerformInstallation(InstallMode mode, string procType, int osMajorVersion)
    {
      int cumulCABcount = 0;   // Keeps track of which CABs are installed or already installed.
      string cabPath = null;   // Prepare the precise path to the folder containing the CAB files we can install for the connected mobile device
      DataXferSpeed copySpeed = DataXferSpeed.Slow;  // Default
      SendMonitorMessage("Checking software available for mobile device.");

      switch (osMajorVersion)
      {
        case 3:
          cabPath = "wce300";
          copySpeed = DataXferSpeed.VerySlow;
          break;

        case 4:
        case 5:  // For now the WM5.0 devices will just use the "old" version of the app
          cabPath = "wce400";
          copySpeed = DataXferSpeed.Normal;
          break;

        default:
          Debug.Fail("Unknown OS Version on connected mobile device: " + osMajorVersion.ToString(), "DataXfer.PerformInstallation");
          break;
      }

      if (cabPath == null)    // Exit if failed just above
        return false;

      cabPath = SysInfo.Data.Paths.MobileCabs + cabPath + @"\" + procType + @"\";
      DataSet dataSet = GetMobileCabInfo();

      for (int i = 0; i < dataSet.Tables.Count; i++)
      {
        string filename = "";
        string availVer = "";
        string cfRegKey = "";
        string valPrefix = "";
      
        DataTable table = dataSet.Tables[i];
        DataRow row;

        if (table.Rows.Count == 1)
        {
          row = table.Rows[0];   // Each table should have only one row
      
          for (int j = 0; j < table.Columns.Count; j++)
          {
            try
            {
              DataColumn col = table.Columns[j];
              string celData = row[col.ColumnName].ToString();

              switch (col.ColumnName.ToLower())
              {
                case "filename":
                  filename = celData;
                  if (filename.IndexOf("#") != -1)
                    filename = filename.Replace("#", procType);
                  break;

                case "filename_v3":
                  if (osMajorVersion == 3)
                  {
                    filename = celData;
                    if (filename.IndexOf("#") != -1)
                      filename = filename.Replace("#", procType);
                  }
                  break;

                case "filename_v4":
                  if (osMajorVersion == 4 || osMajorVersion == 5)
                  {
                    filename = celData;
                    if (filename.IndexOf("#") != -1)
                      filename = filename.Replace("#", procType);
                  }
                  break;
                
                case "version":
                  availVer = celData;
                  break;

                case "regkey":
                  cfRegKey = celData;
                  break;

                case "valprefix":
                  valPrefix = celData;
                  break;

                default:
                  Debug.WriteLine("Unknown value type encountered in MobileCabInfo.xml : " + col.ColumnName);
                  break;
              }
            }
            catch (Exception e)
            {
              Debug.WriteLine("Error while reading XML data: " + e.Message);
            }
          }
        }
        else
        {
          Debug.Fail("Unaccounted for # of rows (" + table.Rows.Count.ToString() + ") in " + table.TableName + " table.", "DataTransfer.MobileCabInfo.xml");
        }

        if (filename == "" || availVer == "" || cfRegKey == "")
          Debug.Fail("Unable to check whether " + table.TableName + " needs to be upgraded.  Please fix MobileCabInfo.xml!");
        
        else
        {
          string currVer;

          switch (table.TableName.ToLower())
          {
            case "netcf":
              currVer = CFRegistry.GetCFVersion(cfRegKey);
              if (currVer == null || (Tools.CompareVersionNumbers(availVer, currVer) == 1))
                if (InstallMobileCAB(cabPath, filename, "Compact Framework", copySpeed))
                {
                  // Some older Pocket PCs take a loooooong time to extract the contents of a CAB file.
                  // So we're going to give it lots of time to do its work.
                  bool netCFInitialized = false;
                  for (int j = 0; j < 15; j++)
                  {
                    Debug.WriteLine("Checking if NetCF was installed correctly - Iteration " + j.ToString() + " : " + DateTime.Now.ToLongTimeString());

                    // See if the extraction from the CAB file is complete.
                    if (Tools.CompareVersionNumbers(availVer, CFRegistry.GetCFVersion(cfRegKey)) == 0)
                    {
                      netCFInitialized = true;
                      Debug.WriteLine("Iterations required before NETCF was installed: " + j.ToString());
                      break;
                    }
                    else
                      Thread.Sleep(2000);  // Sleep for 2 seconds to give the CAB file more time to do its work
                  }

                  if (netCFInitialized)
                    cumulCABcount = cumulCABcount + 1;
                  else
                    return false;
                }
                else
                  return false;
              else
                cumulCABcount = cumulCABcount + 1;
              break;


//            case "opennetcf":
//              currVer = CFRegistry.GetOpenNETCFVersion(cfRegKey, valPrefix);
//              if (currVer == null || (Tools.CompareVersionNumbers(availVer, currVer) == 1))
//                if (InstallMobileCAB(cabPath, filename, "OpenNET Compact Framework", copySpeed))
//                {
//                  bool openNetCFInitialized = false;
//                  for (int j = 0; j < 10; j++)
//                  {
//                    Debug.WriteLine("Checking if OpenNETCF was installed correctly - Iteration " + j.ToString() + " : " + DateTime.Now.ToLongTimeString());
//
//                    // See if the extraction from the CAB file is complete.
//                    if (Tools.CompareVersionNumbers(availVer, CFRegistry.GetOpenNETCFVersion(cfRegKey, valPrefix)) == 0)
//                    {
//                      openNetCFInitialized = true;
//                      Debug.WriteLine("Iterations required before OpenNETCF was installed: " + j.ToString());
//                      break;
//                    }
//                    else
//                      Thread.Sleep(2000);  // Sleep for 2 seconds to give the CAB file more time to do its work
//                  }
//
//                  if (openNetCFInitialized)
//                    cumulCABcount = cumulCABcount + 2;
//                  else
//                    return false;
//                }
//                else
//                  return false;
//              else
//                cumulCABcount = cumulCABcount + 2;
//              break;


            case "app":
              currVer = CFRegistry.GetAppVersion();
              if (currVer == null || (Tools.CompareVersionNumbers(availVer, currVer) == 1))
              {
                string destPath = null;
                if (mode == InstallMode.Update)
                {
                  destPath = CFRegistry.GetAppPath();
                  if (destPath == null)
                    destPath = @"\Program Files\" + SysInfo.Data.Admin.AppName;
                }
                else
                {
                  // The user may have specified that PP be installed in a location
                  // other than main memory.  If so, then we need to handle this here.
                  string aiLocn = Tools.DisallowNullString(SysInfo.Data.Options.Mobile.AppInstallLocation);
                
                  if (aiLocn != null && aiLocn.ToLower() != "main memory")
                    destPath = @"\" + aiLocn + @"\" + SysInfo.Data.Admin.AppName;
                  else
                    destPath = @"\Program Files\" + SysInfo.Data.Admin.AppName;
                }

                // Debug: Just before installing app, try removing its registry entry, to avoid "Do you want to reinstall?" prompt
                //        Commented out until DeleteSubKey bug is corrected by OpenNETCF
                //CFRegistry.RemoveSoftwareAppsEntry(SysInfo.Data.Admin.CompanyName, SysInfo.Data.Admin.AppName);

                // Note: 'destPath' is passed deliberately without a trailing backslash
                if (InstallMobileCAB(cabPath, filename, SysInfo.Data.Admin.AppName, copySpeed, destPath))
                {
                  // Set AppPath value in the registry
                  destPath = Tools.EnsureFullPath(destPath);
                  CFRegistry.SetAppPath(destPath);
                  MobileAppPath = destPath;

                  // Set DataPath value in the registry
                  string dataPath = null;

                  if (mode == InstallMode.Update)
                  {
                    dataPath = CFRegistry.GetDataPath();
                    if (dataPath == null)
                      dataPath = destPath + "Data";
                  }
                  else
                  {
                    string diLocn = Tools.DisallowNullString(SysInfo.Data.Options.Mobile.DataInstallLocation);

                    if (diLocn != null && diLocn.ToLower() != "main memory")
                    {
                      dataPath = @"\" + diLocn + @"\" + SysInfo.Data.Admin.AppName;
                      RapiTools.CreateDataFolders(rapi, dataPath);
                    }
                    else
                    {
                      dataPath = @"\Program Files\" + SysInfo.Data.Admin.AppName;
                      RapiTools.CreateDataFolders(rapi, dataPath);
                    }
                    dataPath = dataPath + @"\Data";
                  }

                  CFRegistry.SetDataPath(dataPath);
                  MobileDataPath = Tools.EnsureFullPath(dataPath);

                  // Testing has revealed that in some cases there's a lag time between when the app's CAB file is executed
                  // on the PPC and when the extracted files are actually available.  So we'll introduce some code here to 
                  // handle that possibility.
                  bool mobileAppInitialized = false;
                  for (int j = 0; j < 15; j++)
                  {
                    Debug.WriteLine("Checking if the App was installed correctly - Iteration " + j.ToString() + " : " + DateTime.Now.ToLongTimeString());

                    // Now that AppPath and DataPath are established in the mobile registry, see if the app's EXE
                    // has yet been extracted from the CAB file.  If so, then execute it with a special parameter.
                    if (RapiTools.FileExists(rapi, destPath + SysInfo.Data.Admin.AppFilename))
                    {
                      // If the device doesn't already have a GUID then create a new one and store it in the mobile device's registry
                      if (CFRegistry.GetGuid() == null)
                        CFRegistry.SetNewGuid();

                      // Now we'll store the mobile app's version number into the mobile device registry.
                      // This, by the way, prevents an unnecessary reinstallation if the mobile device is resynched before the mobile app is ever run.
                      CFRegistry.SetAppVersion(availVer);

//                      // Since the current version of OpenNETCF provides no way to get the version number of the just
//                      // installed mobile PP.exe, we'll startup the app in Initialize mode in order to get the version
//                      // number stored in the registry.  This prevents an unnecessary reinstallation if the mobile device
//                      // is resynched before the mobile app is run.
//                      RapiTools.StartApp(rapi, destPath + SysInfo.Data.Admin.AppFilename, "Initialize");

                      mobileAppInitialized = true;
                      Debug.WriteLine("Iterations required before PP.exe was installed: " + j.ToString());
                      break;
                    }
                    else
                      Thread.Sleep(2000);  // Sleep for 2 seconds to give the CAB file more time to extract its contents
                  }

                  if (mobileAppInitialized)
                    cumulCABcount = cumulCABcount + 2;
                  else
                    Debug.Fail("Error: " + SysInfo.Data.Admin.AppFilename + " was installed but can't be found [yet]", "DataXfer.PerformInstallation");
                }
                
                else
                {
                  // Arriving here, generally it means that something has gone wrong with the installation of the app.
                  // But there's a special circumstance whereby the required CAB file is missing on the desktop.  We
                  // will do a special check to see if the app's EXE is available on the mobile device from a previous
                  // installation.  If so, then the subsequent data transfer can actually proceed. Note: Though we will
                  // assume, in this special case, that the data folders exist and are correct, we will set the app path
                  // just like we've done in the above construct.

                  // Set AppPath value in the registry
                  destPath = Tools.EnsureFullPath(destPath);
                  CFRegistry.SetAppPath(destPath);
                  MobileAppPath = destPath;

                  // Check if app's EXE exists on mobile device.
                  if (RapiTools.FileExists(rapi, destPath + SysInfo.Data.Admin.AppFilename))
                    cumulCABcount = cumulCABcount + 2;
                  else
                    return false;
                }
              }
              else
                cumulCABcount = cumulCABcount + 2;
              break;

            default:
              Debug.WriteLine("Unknown CAB type encountered in MobileCabInfo.xml : " + table.TableName);
              break;
          }
        }
      }

      // The value of 3 means that both of the components (NetCF and App) are correctly in place
      return (cumulCABcount == 3) ? true : false;
    }

    #endregion


    #region DeviceInfo
    /// <summary>
    /// Tries to retrieve the GUID value from the mobile device's registry.  If it
    /// doesn't exist then executes the mobile PP.exe with the special "Initialize"
    /// parameter so that the GUID value is created on the device.
    /// </summary>
    private static string GetGuid()
    {
      string guid = CFRegistry.GetGuid();
      if (guid == null)
      {
//        // Note: Much/All of the rest of this code won't be run if PP hasn't yet been installed on the mobile device
//        if (! MobileInitStarted)
//        {
//          // The GUID hasn't yet been created (or was erased) so force a new one to be created
//          string appPath = CFRegistry.GetAppPath();
//          if (appPath != null)
//          {
//            if (RapiTools.FileExists(rapi, appPath + SysInfo.Data.Admin.AppFilename))
//            {
//              // This will ensure that the Guid (and Version) registry key(s) are created.
//              // Note: It'll likely take a few seconds before the Guid is created.
//              RapiTools.StartApp(rapi, appPath + SysInfo.Data.Admin.AppFilename, "Initialize");
//              MobileInitStarted = true;
//            }
//          }
//        }

        // New Idea: Create the GUID on the desktop and then just store it in the mobile device registry.
        // RW Note: Can't remember why I did it the other way, but now seems like very complicated approach.
        guid = CFRegistry.SetNewGuid();
      }

      return guid;
    }



    /// <summary>
    /// Creates a new device object, populating it with the passed GUID value and
    /// retrieving as much other data as possible to fill the other properties.
    /// </summary>
    /// <param name="guid"></param>
    /// <param name="verInfo"></param>
    /// <param name="appVersion"></param>   // Note: Will be "" if new mobile device and app isn't installed yet
    /// <param name="userName"></param>
    /// <returns></returns>
    private static _Device CreateNewDevice(string guid, CEOSVERSIONINFO verInfo, string appVersion, string userName)
    {
      if (guid == null)   // No purpose to having a device record if there's no GUID
        return null;

      _Device device = new _Device(SysInfo.Data.Devices);
      device.ID = -1;        // Forces next available ID # to be set
      device.Guid = guid;
      device.OSVersion = verInfo.dwMajorVersion + "." + verInfo.dwMinorVersion + "." + verInfo.dwBuildNumber;
      device.SoftwareVersion = appVersion;
      // LastUpdate: This is updated when the software on the device is updated
      // LastSync: We won't set now but only after the DataXfer process has concluded
      device.PrimaryUser = userName;
      // MultipleUsers: Future To Do
      device.Active = true;        // Note: Setting to false will be a Future To Do

      SysInfo.Data.Devices.Add(device);

      return device;
    }

    private static void UpdateLastSyncProperty(string guid)
    {
      _Device device = SysInfo.Data.Devices.Find_Guid(guid);
      if (device != null)
        device.LastSync = DateTime.Now;
    }

    /// <summary>
    /// Pocket Pollster [currently] only supports the ARM, SH3, and X86 processors.  This method discerns which
    /// processor the connected PPC has and informs the user if it's one of the unsupported models.
    /// </summary>
    /// <returns>The name of the processor if a supported one and null otherwise.</returns>
    private static string GetProcessorType (int osVersion)
    {
      SYSTEM_INFO sysInfo = RapiTools.GetSystemInfo(rapi);
      string procType = sysInfo.wProcessorArchitecture.ToString();

      switch (procType.ToLower())
      {
        case "arm":
          if (osVersion == 4 || osVersion == 5)  // Note: For now, we'll just install the old version onto WM5 devices too!
            procType += "v4";
          break;

        case "shx":
          procType = "SH3";
          break;

        case "intel":
          procType = "X86";
          break;

        default:
          DisplayStatusMessage("The connected mobile device has an unsupported processor - Type: " + procType, false);
          procType = null;
          break;
      }

      return procType;
    }

    #endregion


    #region StatusFormUpdates

    /// <summary>
    /// This method updates some of the information on the Notification form.  It is fired every second.
    /// </summary>
    private static void DisplayDeviceInfo()
    {
      if (IsStillConnected())
      {
        DateTime currTime = System.DateTime.Now;

        // Display Connection Time Info
        statusForm.ShowTimeInfo(startTime, currTime);

        // Only check battery every 30 seconds
        System.TimeSpan battTime = currTime - lastBatteryCheck;

        if (battTime.Seconds >= 30)
        {
          lastBatteryCheck = currTime;

          // Display Power Info
          SYSTEM_POWER_STATUS_EX status = RapiTools.GetPowerInfo(rapi);

          // I'm not sure why, but for some reason the Battery status information gets "disconnected" before RAPI is actually disconnected.
          // So without this prelim check, it looks funny just before ending.
          if (status.BatteryFlag != 0)
            statusForm.UpdateBatteryLevel(status.ACLineStatus, status.BatteryLifePercent);
        }
      }
    }


    /// <summary>
    /// This method displays info on the Notification form that doesn't change.
    /// </summary>
    /// <param name="device"></param>
    /// <returns></returns>
    private static bool DisplayOneTimeDeviceInfo(_Device device)
    {
      if ((device == null) || (device.ID < 1))
        return false;
      
      statusForm.ShowDeviceID(device.FormatDeviceID(device.ID));
      statusForm.ShowLastSync(device.FormatDate(device.LastSync));
      return true;
    }


    /// <summary>
    /// Displays various information about the user.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="userName"></param>
    private static void DisplayUserInfo(string userName, string firstName, string lastName)
    {
      if (userName == null)
        return;

      statusForm.ShowUserInfo(userName, firstName, lastName);
    }

    #endregion


    #region CABInstallation
    /// <summary>
    /// Instantiates a special object and populates it with data that is used by the progress bar 
    /// in frmNotify to keep the user informed about the status of the installation process.
    /// </summary>
    /// <param name="startNew"></param>
    /// <param name="commonName"></param>
    /// <param name="fileName"></param>
    /// <param name="totalSize"></param>
    /// <param name="copySpeed"></param>
    public static void InitiateCopyInfo(bool startNew, string commonName, string fileName, long totalSize, DataXferSpeed copySpeed)
    {
      if (startNew)
      {
        if (copyFileStatus != null)                   // Starting a new copy process so reinitialize
          copyFileStatus = null;

        copyFileStatus = new FileCopyInfo();
        copyFileStatus.FileName = fileName;
        copyFileStatus.FileSize = 0;
        copyFileStatus.TotalSize = totalSize;
        copyFileStatus.CopyTime = 0;                  // When first instantiated, we're at 0 seconds
        
        //copyFileStatus.NextAdjTime = 5;               // First adjustment time will occur at 5 seconds
        copyFileStatus.NextAdjTime = 2;               // First adjustment time will occur at 2 seconds
        copyFileStatus.NextAdjCounter = 0;

        // Set reasonable minimum rate [to start]
        switch (copySpeed)
        {
          case DataXferSpeed.VerySlow:
            copyFileStatus.CopyRate = 15000;
            break;

          case DataXferSpeed.Slow:
            copyFileStatus.CopyRate = 25000;
            break;

          case DataXferSpeed.Normal:
            copyFileStatus.CopyRate = 60000;
            break;

          case DataXferSpeed.Fast:
            copyFileStatus.CopyRate = 75000;
            break;

          case DataXferSpeed.VeryFast:
            copyFileStatus.CopyRate = 100000;
            break;
        
          default:
            copyFileStatus.CopyRate = 25000;
            break;
        }

        copyFileStatus.LastPercent = 0;
        copyFileTimer.Start();
      }

      else  // Passing 'false' to {startNew} effectively stops the copy progress timer
      {
        Debug.WriteLine("FileCopy has concluded: Size = " + copyFileStatus.TotalSize.ToString() + "   Time = " + copyFileStatus.CopyTime.ToString());
        statusForm.ShowCopyProgress(-1);              // Hide the progress bar
        copyFileTimer.Stop();                         // Debug: Need to confirm that this absolutely stops the timer
        copyFileStatus = null;
      }
    }

    public static void InitiateCopyInfo(bool startNew)
    {
      InitiateCopyInfo(startNew, null, null, 0, DataXferSpeed.Normal);
    }



    /// <summary>
    /// Installs a CAB file onto the mobile device.
    /// Note: This method will likely conclude before the CAB file has had a chance to complete its installation work.
    ///       This fact must be taken into account elsewhere, lest we make assumptions that certain files emanating
    ///       from the installation are, in fact, still in the process of being extracted from the CAB file!
    /// </summary>
    /// <param name="srcPath"></param>
    /// <param name="cabFile"></param>
    /// <param name="cabName"></param>
    /// <param name="copySpeed"></param>
    /// <param name="destPath"></param>
    /// <returns>true - File installation was successful</returns>
    public static bool InstallMobileCAB(string srcPath, string cabFile, string cabName, DataXferSpeed copySpeed, string destPath)
    {
      Debug.WriteLine("Installing mobile cab file: " + cabFile);
      SendMonitorMessage("Starting installation of " + cabName + " onto mobile device.");

      FileInfo fileInfo = null;

      // It sometimes happens that the necessary CAB file is not available to be copied.  During development this happens when
      // the solution is restored from backup and the developer forgets to restore the required CAB files.  On an end-user machine
      // they might inadvertently delete one of the CAB files.  So we'll do this preliminary check to ensure the file is present.
      if (! File.Exists(srcPath + cabFile))
      {
        string msg = "The mobile setup file '" + cabFile + "' is missing.  It must be restored in order to prepare/update mobile devices in the future.";
        msg = msg + "\n\nThe easiest way to restore this file is to reinstall " + SysInfo.Data.Admin.AppName + ".";
        Tools.ShowMessage(msg, "Missing Setup File");
        return false;
      }

      try
      {
        if (! RapiTools.TempFolderExists(rapi, ref ExitMsgShown))  // Ensure \Temp folder exists on mobile device
          return false;

        statusForm.ShowCurrentStatus("Installing " + cabName + " . . .");
        fileInfo = new FileInfo(srcPath + cabFile);

        InitiateCopyInfo(true, cabName, @"\Temp\" + cabFile, fileInfo.Length, copySpeed);
        RapiTools.CopyFileToDevice(rapi, srcPath, cabFile, @"\Temp\", ref ExitMsgShown);
        SendMonitorMessage("  '" + cabFile + "' copied to mobile device.");

        if (destPath != null)
          CFRegistry.SetInstallLocation(@"\Temp\" + cabFile, destPath);
      }

      catch (Exception e)
      {
        Debug.WriteLine(e.Message);
        Debug.WriteLine(rapi.Connected.ToString());

        if (! ExitMsgShown)
        {
          ExitMsgShown = true;
          Tools.ShowMessage("Error installing " + cabName + ".  Please disconnect the mobile device and try again.", SysInfo.Data.Admin.AppName);
        }
      }

      finally
      {
        InitiateCopyInfo(false);   // Show progress bar
      }

      // Need to ensure that entire file was copied
      if ((! IsStillConnected()) || (fileInfo.Length != RapiTools.GetFileSize(rapi, @"\Temp\" + cabFile)))
      {
        if (! ExitMsgShown)
        {
          ExitMsgShown = true;
          Tools.ShowMessage("Error installing " + cabName + ".  Please disconnect the mobile device and try again.", SysInfo.Data.Admin.AppName);
        }
        SendMonitorMessage("ERROR: Not all of '" + cabFile + "' was copied to mobile device.");

        return false;
      }
      else
      {
        try
        {
          Debug.WriteLine(cabFile + " was copied okay.  About to execute on device.");
          SendMonitorMessage("'" + cabFile + "' was copied okay.  About to execute on device.");

          // Execute CAB file on mobile device
          if (RapiTools.FileExists(rapi, @"\Windows\wceload.exe"))
          {
            CFRegistry.ResetInstallFlag();

            if (destPath == null)
              RapiTools.StartApp(rapi, @"\Windows\wceload.exe", @"\Temp\" + cabFile);
            else
              RapiTools.StartApp(rapi, @"\Windows\wceload.exe", @" /noui /noaskdest");    // The '/noui' flag is not working as hoped but I'll leave it in for now

            SendMonitorMessage("  Extraction of '" + cabFile + "' was started okay.");
          }
          else
          {
            if (!ExitMsgShown)
            {
              ExitMsgShown = true;
              Tools.ShowMessage("The connected mobile device is missing 'wceload.exe'.  Without it setup cannot occur.", "Critical Error with Mobile Device");
              SendMonitorMessage("ERROR: Mobile device is missing 'wceload.exe'");
            }

            return false;
          }
        }

        catch (Exception e)
        {
          Debug.WriteLine(e.Message);

          if (! IsStillConnected())
            return false;
        }
      }

      Debug.WriteLine("CAB File Extraction has been initiated.  Exiting 'InstallMobileCAB'");

      return true;
    }

    public static bool InstallMobileCAB(string srcPath, string cabFile, string cabName, DataXferSpeed copySpeed)
    {
      return InstallMobileCAB(srcPath, cabFile, cabName, copySpeed, null);
    }

    #endregion


    #region StatusMessageDisplay
    public delegate void StatusMessageHandler(string msg, bool flash);
    public static event StatusMessageHandler StatusMessageEvent;

    // Display the specified message in one of the StatusBar panels of frmMain and then remove it after 10 seconds.
    private static void DisplayStatusMessage(string msg, bool flash)
    {
      try
      {
        if (StatusMessageEvent != null)
          StatusMessageEvent(msg, flash);

        // We'll only start the timer after the first message, not when we're removing it.
        if (msg != "")
        {
          System.Timers.Timer msgTimer = new System.Timers.Timer(10000);
          msgTimer.AutoReset = false;   // Run timer for only one iteration
          msgTimer.Elapsed += new System.Timers.ElapsedEventHandler(StopMsgDisplay);   // Add handler
          msgTimer.Start();
        }
      }
      catch (Exception e)
      {
        // Handle exceptions
        Debug.Fail("Exception generating status message: " + e.Message, "DataXfer.DisplayStatusMessage");
      }
    }

    private static void StopMsgDisplay(object sender, System.Timers.ElapsedEventArgs e)
    {
      DisplayStatusMessage("", false);
    }

    #endregion


    #region MonitoringMessages
    public delegate void MonitoringMessageHandler(string msg);
    public static event MonitoringMessageHandler MonitoringMessageEvent;

    // Fires an event that, if monitored, passes the specified message to the monitoring form.
    private static void SendMonitorMessage(string msg)
    {
      try
      {
        if (MonitoringMessageEvent != null)
          MonitoringMessageEvent(msg);
      }

      catch (Exception ex)
      {
        // Handle exceptions
        Debug.Fail("Exception generating monitoring message: " + ex.Message, "DataXfer.SendMonitorMessage");
      }
    }

    #endregion


    #region CFSysInfo
    /// <summary>
    /// Looks for the [CF] "SysInfo.xml" file in the app folder on the mobile device and copies it
    /// to the Temp folder on the Desktop, renaming it to CFSysInfo.xml.
    /// </summary>
    /// <param name="path"></param>   // The path where the mobile app is located
    /// <returns>The full path of the copied file</returns>
    private static string RetrieveCFSysInfo(string path)
    {
      string srcFile = path + "SysInfo.xml";
      if (RapiTools.FileExists(rapi, srcFile))
      {
        // The file is available to be copied.  First make sure that we're free to copy it.
        string destFile = SysInfo.Data.Paths.Temp + "CFSysInfo.xml";
        if (! Tools.DeleteFile(destFile))
          return null;
         
        // Reaching here, we *should* be able to copy the file from the mobile device, so let's try.
        try
        {
          if (RapiTools.CopyFileFromDevice(rapi, srcFile, destFile))
            return destFile;
        }
        catch
        {
          Debug.WriteLine("Couldn't copy mobile file: " + srcFile);
          return null;
        }
      }

      // If we reach here then the file is not available
      return null;
    }


    // Retrieve CFSysInfo data from the file we just copied from the mobile device
    // and use its data to populate the 'CFSysInfo.Data' object.
    private static void OpenTempCFSysInfo(string fullPath)
    {
      // For reasons I'm not quite sure of, this data object needs to be re-instantiated for it's the case that if it
      // contained data from a previous population during the same session that it causes an error during 'OpenData'.
      CFSysInfo.Data = new CFSysInfo();

      Tools.OpenData(fullPath, CFSysInfo.Data);
    }


    /// <summary>
    /// Saves the CFSysInfo object to "SysInfo.xml" in the Temp folder.
    /// Then copies this file to the app folder on the mobile device.
    /// Also erases all temporary files that we've used in this process.
    /// </summary>
    /// <param name="mobileAppPath"></param>   // The path [on the mobile device] where the mobile app is located.
    private static void SaveCFSysInfo(string mobileAppPath)
    {
      string xmlName = "SysInfo.xml";
      string tempDir = SysInfo.Data.Paths.Temp;

      Tools.DeleteFile(tempDir, "CFSysInfo.xml");   // Remove old file
      Tools.SaveData(tempDir + "SysInfo.xml", CFSysInfo.Data, null, ExportFormat.XML);

      // Reaching here, we *should* be able to copy the file to the mobile device, so let's try.
      try
      {
        RapiTools.CopyFileToDevice(rapi, tempDir, xmlName, mobileAppPath);
        RapiTools.HideFile(rapi, mobileAppPath + xmlName);
      }
      catch
      {
        Debug.WriteLine("Couldn't copy mobile file: " + xmlName);
      }
      finally
      {
        Tools.DeleteFile(tempDir, xmlName);   // Remove file because it's no longer needed
      }
    }

    #endregion


    #region IsFileOpen

    // Debug: In frmMain, "IsFileOpen" has not yet correctly implemented 'BeginInvoke' so this *may* cause strange problems.

    public delegate bool IsFileOpenHandler(string filename);
    public static event IsFileOpenHandler IsFileOpenEvent;

    public static bool IsFileOpen(string filename)
    {
      return IsFileOpenEvent(filename);
    }

    #endregion


    #region DelayedImport

    public delegate void DelayedImportHandler(byte mode);
    public static event DelayedImportHandler DisplayNewDataEvent;

    // Display the "New Data Available" message in the leftmost StatusBar panels of frmMain.
    private static void DisplayNewDataMessage(byte mode)
    {
      try
      {
        if (DisplayNewDataEvent != null)
          DisplayNewDataEvent(mode);
      }

      catch (Exception e)
      {
        Debug.Fail("Error invoking status message: " + e.Message, "DataXfer.DisplayNewDataMessage");
      }
    }

    #endregion


  }
}
