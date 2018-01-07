namespace DataObjects
{
  using System;
  using System.IO;
  using System.Data;
  using System.Drawing;
  using System.Reflection;
  using System.Diagnostics;
  using System.Windows.Forms;
  using System.Runtime.InteropServices;
  using OpenNETCF;
  using OpenNETCF.Win32;



	/// <summary>
	/// ToolsCF contains methods that are only used by the Mobile app.
	/// </summary>

	public class ToolsCF
	{
		public ToolsCF()
		{
		}



    /// <summary>
    /// There is apparently no direct way from the desktop app to get the version number of the mobile
    /// 'PP.exe' file (which this code compiles into btw).  So this method, called upon startup, will 
    /// ensure that the CF Registry is correctly updated with the version number.
    /// 
    /// Note: If the user aborts an update then the version number will be incorrectly updated since
    ///       the new EXE won't actually be installed.  So this method is the key to resetting the
    ///       version number back to what is the real version number.
    /// </summary>
    /// <param name="updateRegistry"></param>   // Updates the registry, if required
    public static string GetVersionNumber(bool updateRegistry)
    {
      // Note: A bug was observed whereby the last digit of version retrieved from the assembly is not always correct
      string actualVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      string apparentVersion = RegistryCF.GetVersionNumber();

      if (updateRegistry)
        if (Tools.CompareVersionNumbers(actualVersion, apparentVersion) == 1)
          RegistryCF.SetVersionNumber(actualVersion);

      return actualVersion;
    }


    public static string GetOSVersion()
    {
      return EnvironmentEx.OSVersion.Version.Major + "." + 
             EnvironmentEx.OSVersion.Version.Minor + "." + 
             EnvironmentEx.OSVersion.Version.Build;
    }


    /// <summary>
    /// Checks whether a GUID value exists and if not then creates one.
    /// </summary>
    public static void CheckGuid()
    {
      string sGuid = RegistryCF.GetGuid();
      
      if (sGuid == null)
      {
        Guid guid = PocketGuid.NewGuid();
        RegistryCF.SetGuid(guid.ToString());
      }
    }


    // Shows or hides the on-screen keyboard
    // 1 - Show        0 - Hide
    [DllImport("coredll.dll")]
    public static extern bool SipShowIM(int dwFlag);

    // Used with pointers to CF controls
    [DllImport("coredll")]
    public static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("coredll.dll")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hwnd);





    /// <summary>
    /// This method works in conjunction with the Data Transfer app placing its time, which we assume to be the correct time,
    /// in a special registry location and then remotely starting this mobile app with the "SetTime" parameter.  It's important,
    /// though not critical, to keep the time on all the mobile devices as correct as possible.
    /// </summary>
    /// <returns>true - Time was changed   false - Time was not changed</returns>
    public static bool UpdateLocalTime()
    {
      DateTime currTime = DateTime.Now;
      string sNewTime = RegistryCF.GetNewTime();
      RegistryCF.ClearNewTime();                    // Good practice to clear this time value

      if (sNewTime != "")
      {
        //DateTime newTime = Convert.ToDateTime(sNewTime);
        DateTime newTime = Tools.GetDateFromString(sNewTime);

        // The 2nd test is to ensure that the date in the registry isn't completely bogus
        if ((newTime.ToString() != "") && (newTime.Year == currTime.Year))
        {
          TimeSpan timeSpan = currTime - newTime;

          if (Math.Abs(timeSpan.Minutes) > 5)  // Don't bother changing if time difference is less than 5 minutes
          {
            DateTimeEx.SetLocalTime(newTime);
            return true;
          }
        }
      }

      return false;
    }



    /// <summary>
    /// Performs a simple test to see if the screen dimensions have changed.
    /// If they have, then their values are updated in CFSysInfo.
    /// </summary>
    /// <returns>true - Screen dimensions changed   false - They didn't change</returns>
    public static bool UpdateScreenDimensions()
    {
      int wid, hgt, availHgt;
      return UpdateScreenDimensions(out wid, out hgt, out availHgt);
    }

    public static bool UpdateScreenDimensions(out int wid, out int hgt, out int availHgt)
    {
      // On a Pocket PC screen the Bounds represents the full extents of the screen, which is generally
      // 240W x 320H in Portrait mode.  Over this height there is a top titlebar, a large center section, 
      // and then a bottom menubar.
      //
      // Unfortunately there's a discrepancy in what is reported back for "Screen.PrimaryScreen.WorkingArea.Height".
      // Here's a brief summary of what has been discovered about this value:
      //
      //                           Portrait      Landscape
      // PPC 2002                    294            N/A?
      // PPC 2003                    268            N/A?
      // PPC 2003 2nd Edition        294            214
      //
      // An observation:  320 - 26 = 294          294 - 26 = 268

      // For the newer model, since the titlebar and menubar are the same height, here's
      // the calculation involved:
      //                            BH = Bounds Height      WAH = WorkingArea Height
      //   MiddleHeight = BH - (BH - WAH) - (BH - WAH) = 2WAH - BH

      bool scrnChanged = false;
      availHgt = 0;  // Initialize
      wid = Screen.PrimaryScreen.Bounds.Width;

      if (CFSysInfo.Data.DeviceSpecs.ScreenWidth != wid)
      {
        scrnChanged = true;
        CFSysInfo.Data.DeviceSpecs.ScreenWidth = wid;
        CFSysInfo.Data.DeviceSpecs.ScreenHeight = Screen.PrimaryScreen.Bounds.Height;
        int hgtWA = Screen.PrimaryScreen.WorkingArea.Height;

        if (wid == 240)
          if (hgtWA == 268)
            availHgt = 268;
          else
            availHgt = 2 * hgtWA - Screen.PrimaryScreen.Bounds.Height;
        
        else if (wid == 320)
          availHgt = 2 * hgtWA - Screen.PrimaryScreen.Bounds.Height;

        else
          Debug.Fail("Unaccounted for width: " + wid.ToString(), "ToolsCF.UpdateScreenDimensions");

        CFSysInfo.Data.DeviceSpecs.AvailHeight = availHgt;
      }

      hgt = CFSysInfo.Data.DeviceSpecs.ScreenHeight;
      availHgt = CFSysInfo.Data.DeviceSpecs.AvailHeight;

      return scrnChanged;
    }


    /// <summary>
    /// This method copies a template file to the Data folder, renaming it if a poll with that name already exists.
    /// </summary>
    /// <param name="srcPath"></param>
    /// <returns></returns>
    public static string CopyTemplateToDataFolder(string srcPath)
    {
      string fileName = Tools.StripPath(srcPath);
      string destPath = CFSysInfo.Data.MobilePaths.Data;

      fileName = Tools.GetAvailFilename(destPath, fileName);
      File.Copy(srcPath, destPath + fileName);

      return destPath + fileName;
    }


    /// <summary>
    /// Calculates (in a rough way) the height a paragraph will take up.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="text"></param>
    /// <param name="font"></param>
    /// <param name="maxWidth"></param>
    /// <returns></returns>
    public static int CalcTextHeight(string text, Font font, int maxWidth)
    {
      string tempString = text;
      string workString = "";
      int npos = 1;
      int sp_pos = 0;
      int line = 0;
      int nWidth = 0;

      Form form1 = new Form();                // Necessary to access 'CreateGraphics' below
      Graphics g = form1.CreateGraphics();    // Create a Graphics object for the Control

      //get original size
      SizeF size = g.MeasureString(text, font);

      if (size.Width > maxWidth)
      {
        while(tempString.Length > 0)
        {
          //Check for the last lane
          if (npos > tempString.Length)
          {
            line++;
            break;
          }
          workString = tempString.Substring(0, npos);
          //get the current width
          nWidth = (int) g.MeasureString(workString, font).Width;
          //check if we've got out of the destWidth
          if (nWidth > maxWidth)
          {
            //try to find a space
            sp_pos = workString.LastIndexOf(" ");
            if (sp_pos > 0)
            {
              //cut out the wrap lane we've found
              tempString = tempString.Substring((sp_pos + 1), tempString.Length - (sp_pos + 1));
              line++;
              npos = 0;
            }
            else //no space
            {
              tempString = tempString.Substring(npos, tempString.Length - npos);
              line++;
              npos = 0;
            }
          }

          npos++;
        }
      }
      else
        line = 1;

      return (int) (line * size.Height);
    } 


    // Since CF labels don't have the AutoSize property, this is the next best thing.
    public static void SetLabelWidth(Label label)
    {
      label.Width = Tools.GetLabelWidth(label.Text, label.Font);
    }

    // This is an alternative to the above method.
    public static void AutoSizeLabelText(Label label, string text)
    {
      label.Text = text;
      label.Width = Tools.GetLabelWidth(text, label.Font);
    }


    // Checks whether the application is expired (if an expiry date is embedded).  
    // If so, provides a warning message and returns 'true'.
    public static bool IsAppExpired()
    {
      if (CFSysInfo.Data.MobileAdmin.ExpiryDate.ToString() == "1/1/0001 12:00:00 AM")  // Check for null date
        return false;

      TimeSpan duration = CFSysInfo.Data.MobileAdmin.ExpiryDate - DateTime.Now.Date;

      if (duration.Days < 0)
        return true;
      
      else if (duration.Days < 8)
      {
        string msg = "This copy of the software will expire on " + CFSysInfo.Data.MobileAdmin.ExpiryDate.ToShortDateString() + ".\n\n";
        msg += "Please contact us to obtain a new copy of the software.";
        MessageBox.Show(msg, CFSysInfo.Data.MobileAdmin.AppName + " Is Expiring Soon");
      }

      return false;
    }










	}  // End of class ToolsCF
}
