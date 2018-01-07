using System;
using Microsoft.Win32;
using System.Diagnostics;
using DataObjects;


namespace DataTransfer
{
	/// <summary>
	/// Summary description for Registry.
	/// </summary>
	public class RegistryTools
	{
		public RegistryTools()
		{
			//
			// TODO: Add constructor logic here
			//
		}



    /// <summary>
    /// Ensures that the 'PocketPollster' registry key exists, and returns a reference to it.
    /// </summary>
    /// <returns></returns>
    private static RegistryKey GetDesktopAppKey()
    {
      string path = @"Software\" + SysInfo.Data.Admin.RegKeyName;
      RegistryKey regKey = Registry.LocalMachine.OpenSubKey(path, true);

      if (regKey == null)
        regKey = Registry.LocalMachine.CreateSubKey(path);

      return regKey;
    }


//    /// <summary>
//    /// Gets the version number of the latest mobile version available.  This is, in fact, the version number of
//    /// the mobile setup package that resides in a special Pocket Pollster sub-folder on the desktop computer.
//    /// </summary>
//    public static string GetLatestMobileVersion()
//    {
//      string version = "";
//
//      RegistryKey regKey = GetDesktopAppKey();
//
//      if (regKey != null)
//        if (regKey.GetValue("LatestMobileVersion") != null)
//          version = regKey.GetValue("LatestMobileVersion").ToString();
//
//      return version;
//    }


















	}
}
