using System;
using System.Diagnostics;
using OpenNETCF.Desktop.Communication;
using DataObjects;



namespace DataTransfer
{
  // Define Aliases
  using Tools = DataObjects.Tools;


	/// <summary>
	/// Methods to examine/manipulate the registry on a Compact Framework device.
	/// </summary>
	public class CFRegistry
	{
		public CFRegistry()
		{
			//
			// TODO: Add constructor logic here
			//
		}



    /// <summary>
    /// Ensures that the app's registry key exists on the mobile device, and returns a reference to it.
    /// </summary>
    /// <returns></returns>
    private static CERegistryKey GetMobileAppKey()
    {
      string path = @"Software\" + SysInfo.Data.Admin.RegKeyName;
      
      CERegistryKey regKey = null;

      // It's possible that the PPC may be disconnected just before calling this method
      try
      {
        regKey = CERegistry.LocalMachine.OpenSubKey(path, true);

        if (regKey == null)
          regKey = CERegistry.LocalMachine.CreateSubKey(path);
      }

      catch (Exception e)
      {
        // This is just for developer information purposes only.  In Release mode we
        // really don't need to do anything more than catch the error and continue.
        Debug.WriteLine("Error accessing registry: " + e.Message, "CFRegistry.GetMobileAppKey");  
      }

      return regKey;
    }


    // Retrieves the "AppPath" value, if it exists.
    public static string GetAppPath()
    {
      string path = null;
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("AppPath") != null)
        {
          path = regKey.GetValue("AppPath").ToString();
          path = Tools.EnsureFullPath(path);
        }

        regKey.Close();
      }

      return Tools.DisallowNullString(path);
    }


    // Sets the value of "AppPath".
    public static void SetAppPath(string path)
    {
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        regKey.SetValue("AppPath", path);
        regKey.Close();
      }
    }


    // Retrieves the "DataPath" value, if it exists.
    public static string GetDataPath()
    {
      string path = null;
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("DataPath") != null)
        {
          path = regKey.GetValue("DataPath").ToString();
          path = Tools.EnsureFullPath(path);
        }

        regKey.Close();
      }

      return Tools.DisallowNullString(path);
    }


    // Sets the value of "DataPath".
    public static void SetDataPath(string path)
    {
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        regKey.SetValue("DataPath", path);
        regKey.Close();
      }
    }


    // Checks whether the "StopAskingToInstall" flag exists and is set true.
    public static bool InstallPromptOkay()
    {
      bool okayToProceed = true;
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("StopAskingToInstall") != null)
        {
          string regStr = regKey.GetValue("StopAskingToInstall").ToString();
          okayToProceed = (regStr == "1") ? false : true;
        }
        regKey.Close();
      }

      return okayToProceed;
    }


    // Creates the "StopAskingToInstall" flag and sets it to the specified value.
    public static void SetStopAskingToInstall(bool newval)
    {
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        regKey.SetValue("StopAskingToInstall", newval ? 1 : 0);
        regKey.Close();
      }
    }


    // Retrieves the version number of the dotNet Compact Framework installed on the mobile device.
    public static string GetCFVersion(string keyPath)
    {
      string version = null;
      CERegistryKey regKey = CERegistry.LocalMachine.OpenSubKey(keyPath);

      if (regKey != null)
      {
        version = "1.0.2268.0";  // Some older versions of CF don't have any values so we can assume this version number
        string[] valNames = regKey.GetValueNames();
        if (valNames.Length != 0)
        {
          version = valNames[0].TrimEnd(new char[] {'\0'});  // Store first possible version number

          for (int i = 1; i < valNames.Length; i++)
          {
            string txt = valNames[i];
            txt = txt.TrimEnd(new char[] {'\0'});
            if (Tools.CompareVersionNumbers(txt, version) == 1)
              version = txt;
          }
        }
        regKey.Close();
      }

      return Tools.DisallowNullString(version);
    }



    // Retrieves the version number of the OpenNET Framework installed on the mobile device.
    public static string GetOpenNETCFVersion(string keyPath, string valPrefix)
    {
      string version = null;
      valPrefix = valPrefix.ToLower();
      CERegistryKey regKey = CERegistry.LocalMachine.OpenSubKey(keyPath);

      if (regKey != null)
      {
        string[] valNames = regKey.GetValueNames();

        for (int i = 0; i < valNames.Length; i++)
        {
          string txt = valNames[i].ToLower();
          txt = txt.TrimEnd(new char[] {'\0'});
          if (txt.StartsWith(valPrefix))
          {
            int pos = txt.IndexOf("version=");
            if (pos != -1)
            {
              txt = txt.Substring(pos + 8);
              pos = txt.IndexOf(",");
              if (pos == -1)
                version = txt;   // Must be at the end of the string
              else
                version = txt.Substring(0, pos);
            }
            break;
          }
        }
        regKey.Close();
      }

      return Tools.DisallowNullString(version);
    }


    // Retrieves the value of the current version of Pocket Pollster installed on the mobile device.
    public static string GetAppVersion()
    {
      string version = null;
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("Version") != null)
        {
          version = Tools.DisallowNullString(regKey.GetValue("Version").ToString());
        }

        regKey.Close();
      }

      return version;
    }



    // Sets the value of the current version of Pocket Pollster into the mobile device's registry.
    public static void SetAppVersion(string version)
    {
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        regKey.SetValue("Version", version);
        regKey.Close();
      }
    }


    // Sets a special key to inform WCELOAD where to install Pocket Pollster.
    public static void SetInstallLocation(string cabLocation, string destPath)
    {
      string installPath = @"SOFTWARE\Apps\Microsoft Application Installer\Install";

      CERegistryKey regKey = CERegistry.LocalMachine.OpenSubKey(installPath);

      if (regKey == null)
        regKey = CERegistry.LocalMachine.CreateSubKey(installPath);

      if (regKey != null)
      {
        regKey.SetValue(cabLocation, destPath);
        regKey.Close();
      }
    }


    // If Pocket Pollster has been previously installed then this method changes the "Instl"
    // key from 1 to 0 so that the confirmation dialog doesn't appear on the mobile device.
    public static void ResetInstallFlag()
    {
      string flagPath = @"SOFTWARE\Apps\IMS Pocket Pollster";

      CERegistryKey regKey = CERegistry.LocalMachine.OpenSubKey(flagPath);

      // Only continue if the app was previously installed
      if (regKey != null)
      {
        regKey.SetValue("Instl", 0);
        regKey.Close();
      }
    }


    // Retrieves the "Guid" value, if it exists.
    public static string GetGuid()
    {
      string guid = null;
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("Guid") != null)
        {
          guid = Tools.DisallowNullString(regKey.GetValue("Guid").ToString());
        }

        regKey.Close();
      }

      return guid;
    }



    // Creates a new "Guid" value here and then sets it in the registry on the mobile device.
    public static string SetNewGuid()
    {
      string guid = System.Guid.NewGuid().ToString();

      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        regKey.SetValue("Guid", guid);
        regKey.Close();
      }

      return guid;
    }



    // This method should work but there is some but with DeleteSubKey that's preventing it from doing so.
    // Hopefully a future version of OpenNETCF will correct this problem.
    public static void RemoveSoftwareAppsEntry(string companyName, string appName)
    {
      string path = @"SOFTWARE\Apps\" + companyName + " " + appName;

      CERegistryKey regKey = CERegistry.LocalMachine.OpenSubKey(path);

      if (regKey != null)
      {
        regKey.Close();
        CERegistry.LocalMachine.DeleteSubKey(path);
      }
    }


    // Stores the Desktop's date & time onto the mobile computer.
    public static void SetNewTime()
    {
      CERegistryKey regKey = GetMobileAppKey();

      if (regKey != null)
      {
        //regKey.SetValue("NewTime", DateTime.Now.ToString());
        regKey.SetValue("NewTime", DateTime.Now.ToString("s"));
        regKey.Close();
      }
    }






	}
}
