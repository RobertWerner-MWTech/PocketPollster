using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using DataObjects;


namespace DataObjects
{
	/// <summary>
	/// Assorted methods used for accessing the Windows registry.
	/// </summary>
	public class RegistryTools
	{
		public RegistryTools()
		{
		}


    /// <summary>
    /// Sets the key that will automatically execute the PP executable when a Pocket PC connects via ActiveSync.
    /// </summary>
    /// <param name="appLocn"></param>
    /// <param name="parameter"></param>
    public static void SetAutoStartOnConnect(string appLocn, string parameter)
    {
      // Attempt to open the key
      RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows CE Services\\AutoStartOnConnect", true);

      // If the return value is null, the key doesn't exist
      if (key == null) 
      {
        // The key doesn't exist so create it & then open it
        key = Registry.LocalMachine.CreateSubKey("Software\\Microsoft\\Windows CE Services\\AutoStartOnConnect");
      }

      string appExt = Tools.GetAppExt(true, true);
      if (key.GetValue(appExt) == null)
        key.SetValue(appExt, '\u0022' + appLocn + '\u0022' + " " + '\u0022' + parameter + '\u0022');
    }


    /// <summary>
    /// Removes the key that automatically executes the PP executable when a Pocket PC connects via ActiveSync.
    /// </summary>
    /// <param name="appLocn"></param>
    public static void DeleteAutoStartOnConnect()
    {
      // Attempt to open the key
      RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows CE Services\\AutoStartOnConnect", true);

      // If the return value is null, the key doesn't exist
      if (key != null)
      {
        string appExt = Tools.GetAppExt(true, true);
        if (key.GetValue(appExt) != null)
          key.DeleteValue(appExt);
      }
    }



    /// <summary>
    /// Turns on or turns off the [undocumented] GuestOnly value.
    /// </summary>
    /// <param name="activate"></param>
    public static void SetGuestOnly(bool activate)
    {
      // Attempt to open the key
      RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows CE Services", true);

      // If the return value is null, the key doesn't exist
      if (key != null) 
      {
        int newval = activate ? 1 : 0;

        object oldval = key.GetValue("GuestOnly");

        if (oldval == null)
          key.SetValue("GuestOnly", newval);
        else
          if (oldval.ToString() != newval.ToString())
            key.SetValue("GuestOnly", newval);
      }
    }



    /// <summary>
    /// Ensures that the 'PocketPollster' registry key exists, and returns a reference to it.
    /// </summary>
    /// <returns></returns>
    private static RegistryKey GetDesktopAppKey()
    {
      string ppPath = "Software\\PocketPollster";
      RegistryKey regKey = Registry.LocalMachine.OpenSubKey(ppPath, true);

      if (regKey == null)
        regKey = Registry.LocalMachine.CreateSubKey(ppPath);

      return regKey;
    }


    // Only used during debugging - updates the version number of the mobile PP.exe stored on the development machine.
//    public static void DebugUpdateMobileVersionNumber(string version)
//    {
//      //Debug
//      version = "1.1.0.0";
//
//      RegistryKey regKey = GetDesktopAppKey();
//      regKey.SetValue("LatestMobileVersion", version);
//
//      regKey.Close();
//    }









	}
}
