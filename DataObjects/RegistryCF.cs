using System;
using OpenNETCF;
using OpenNETCF.Win32;


namespace DataObjects
{
	/// <summary>
	/// Summary description for Registry.
	/// </summary>
	public class RegistryCF
	{
		public RegistryCF()
		{
			//
			// TODO: Add constructor logic here
			//
		}



    /// <summary>
    /// Ensures that the 'PocketPollster' registry key exists, and returns a reference to it.
    /// </summary>
    /// <returns></returns>
    private static RegistryKey GetAppKey()
    {
      string ppPath = "Software\\PocketPollster";
      RegistryKey regKey = Registry.LocalMachine.OpenSubKey(ppPath, true);

      if (regKey == null)
        regKey = Registry.LocalMachine.CreateSubKey(ppPath);

      return regKey;
    }


    /// <summary>
    /// Retrieves the version number from the registry, if it exists.
    /// </summary>
    /// <returns></returns>
    public static string GetVersionNumber()
    {
      string version = null;
      RegistryKey regKey = GetAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("Version") != null)
          version = regKey.GetValue("Version").ToString();

        regKey.Close();
      }

      return version;
    }


    /// <summary>
    /// Ensures that the 'Version' value, of the 'PocketPollster' registry entry, exists and is up to date.
    /// </summary>
    /// <param name="fileversion"></param>  // The version number of 'PP.exe', as determined by Reflection
    public static void SetVersionNumber(string fileversion)
    {
      RegistryKey regKey = GetAppKey();

      object regVerObj = regKey.GetValue("Version");

      if (regVerObj == null)
        regKey.SetValue("Version", fileversion);
      else
      {
        string regversion = regVerObj.ToString();
        if (fileversion != regversion)
          regKey.SetValue("Version", fileversion);
      }

      regKey.Close();
    }



    // Retrieves the "AppPath" value, if it exists.
    public static string GetAppPath()
    {
      string path = null;
      RegistryKey regKey = GetAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("AppPath") != null)
        {
          path = regKey.GetValue("AppPath").ToString();
          path = Tools.EnsureFullPath(path);
        }

        regKey.Close();
      }

      return path;
    }


    // Sets the value of "AppPath".
    public static void SetAppPath(string path)
    {
      RegistryKey regKey = GetAppKey();

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
      RegistryKey regKey = GetAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("DataPath") != null)
        {
          path = regKey.GetValue("DataPath").ToString();
          path = Tools.EnsureFullPath(path);
        }

        regKey.Close();
      }

      return path;
    }


    // Sets the value of "DataPath".
    public static void SetDataPath(string path)
    {
      RegistryKey regKey = GetAppKey();

      if (regKey != null)
      {
        regKey.SetValue("DataPath", path);
        regKey.Close();
      }
    }


    // Retrieves the "GUID" value, if it exists.
    public static string GetGuid()
    {
      string guid = null;
      RegistryKey regKey = GetAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("Guid") != null)
          guid = regKey.GetValue("Guid").ToString();

        regKey.Close();
      }

      return guid;
    }


    // Sets the value of "Guid".
    public static void SetGuid(string guid)
    {
      RegistryKey regKey = GetAppKey();

      if (regKey != null)
      {
        regKey.SetValue("Guid", guid);
        regKey.Close();
      }
    }



    // Retrieves the "NewTime" value, if it exists.
    public static string GetNewTime()
    {
      string newtime = null;
      RegistryKey regKey = GetAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("NewTime") != null)
          newtime = regKey.GetValue("NewTime").ToString();

        regKey.Close();
      }

      return newtime;
    }


    // Since a bug with OpenNETCF prevents us from deleting a registry key, we'll just clear it.
    public static void ClearNewTime()
    {
      RegistryKey regKey = GetAppKey();

      if (regKey != null)
      {
        if (regKey.GetValue("NewTime") != null)
          regKey.SetValue("NewTime", "");

        regKey.Close();
      }
    }





	}
}
