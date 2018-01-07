using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using DataObjects;


namespace Desktop
{
  // Define Aliases
  using Tools = DataObjects.Tools;


	/// <summary>
	/// Summary description for PollManager.
	/// </summary>
  public class PollManager
  {
    private static PollManager _current;
    private ArrayList _polls = new ArrayList();
    private static frmMain FormMain;
    private static string StartupFilename;         // Set to a filename (& path) if a filename is passed externally (such as double-clicking on a ".PP" file)
    private static CopyData copyData = null;       // The CopyData class used for message sending
    private static bool Uninstall;                 // Set in HandleStartupParameters - The Uninstall shortcut was activated

    // This is a temporary variable that we're going to use for early versions 
    // of the software to reduce the chances of it being pirated.
    //private static DateTime ExpiryDate;


    public static PollManager Current
    {
      get
      {
        return _current;
      }
    }

    public ArrayList Polls
    {
      get
      {
        return _polls;
      }
    }


    /// <summary>
    /// The main entry point for the desktop application.
    /// Optional arguments can be passed to the EXE.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      Cursor.Current = Cursors.WaitCursor;
      //Application.EnableVisualStyles();  // Debug: Need to investigate this more before using

      // Try to load the assorted SysInfo in from disk.  If there's no file (first time ever or it was deleted) then initialize with default info
      InitializeSysInfo();                 // Now we can set the rest of the reference data variables

      // Initialize Reference data such as geographical info, mobile devices, and languages
      //InitializeRefData();  // Debug: To Do!               

      Tools.HandleTempDirectory(SysInfo.Data.Paths.Temp, true);     // Ensures Temp folder exists and all possible files are removed

      // Check if any command line arguments were passed to the executable.  Among other things...
      // Find out if the "DataTransfer" parameter was passed to the EXE upon startup.
      // If so, then 'SysInfo.Data.Admin.DataTransferActive' will be set true.
      HandleStartupParameters(args);

      if (Uninstall)
        return;   // Quietly exit
      
      else if (!SysInfo.Data.Admin.DataTransferActive)
      {
        if (SysInfo.Data.Options.ShowSplash)
        {
          frmSplash.ShowSplashScreen();
          Application.DoEvents();
        }
      }

      // Now see whether a copy of the app is already running.
      Process aProcess = Process.GetCurrentProcess();
      string aProcName = aProcess.ProcessName;
			
      // Determine how we're going to proceed
      if (Process.GetProcessesByName(aProcName).Length == 1)   // If true, then this is the first copy of the executable to be started
      {
        // If this executable is running then we DON'T want another copy started by ActiveSync
        RegistryTools.DeleteAutoStartOnConnect();

        // This should always be set to true so that the partnership dialog box doesn't appear
        RegistryTools.SetGuestOnly(true);

        Application.ApplicationExit += new EventHandler(ShutDownApp);   // Method that'll be called when shutting down

        // Instantiate the poll manager
        _current = new PollManager();

        // Instantiate an instance of frmMain
        FormMain = new frmMain();
        FormMain.SetMenuItems(0);
        
        // This imports any residual data files that failed to get imported the last time the app was run.  Generally the only
        // time such files would exist would be if the app previously crashed.
        FormMain.ImportNewDataFiles();   

		InitializeCopyData();

        TestCode();  // Only used during debugging; normally empty

        bool okayToStartupApp = true;

        // Check whether the app is being run for the very first time ever.
        if (SysInfo.Data.Options.PrimaryUser == "")
        {
          FormMain.Show();  // Doing this simply to provide standard visual background (behind Options form)

          // We'll startup the Options form in a special way that will force (strongly request) a UserName be specified
          frmOptions optionsForm = new frmOptions(true);

          // If user really, really didn't want to specify a username then we MUST exit app
          if (SysInfo.Data.Options.PrimaryUser == "")
          {
            okayToStartupApp = false;
            FormMain.ExitApp();
          }
        }

        // Now check that the file association(s) are properly setup for this app
        CheckFileAssociations();

        if (okayToStartupApp)
        {
          if (Tools.IsAppExpired())
          {
            FormMain.Freeze();
          }
          else
          {
            // "StartupFilename" is set in HandleStartupParameters
            if (StartupFilename != null)
              FormMain.PreparePoll(StartupFilename);
          }

          Cursor.Current = Cursors.Default;
          Application.Run(FormMain);
        }
      }

      else  // A copy of the app is already running
      {
        if (SysInfo.Data.Admin.DataTransferActive)
        {
          // Since the data transfer thread will already have been started by the RAPI.ActiveSync.Active event
          // we don't need to start a 2nd copy of this application.  So just exit quietly.
          // Note: Because we now remove the AutoStartOnConnect string, we'll actually never get to here
          //       but it doesn't hurt to keep this extra code regardless.          
          Application.ExitThread();
        }
        else
        {
          // if another process exists, send file name as message and quit
          if (StartupFilename != null)
          {
            // Create a new instance of the class
            CopyData copyData = new CopyData();

            // Create the named channels to send and receive on.  The name is arbitrary; it can be anything you want.
            copyData.Channels.Add(SysInfo.Data.Admin.AppName);

            // Send filename
            copyData.Channels[SysInfo.Data.Admin.AppName].Send(StartupFilename);
          }
          else   // 2nd copy started without any specific file so don't allow it
          { 
            // Prevent 2nd copy of executable from running
            Tools.ShowMessage("The " + SysInfo.Data.Admin.AppName + " Desktop application is already running!", SysInfo.Data.Admin.AppName);
            Application.ExitThread();
          }
        }
      }
    }    // End of "Main"




    private static void InitializeSysInfo()
    {
      // First find out where this EXE is located
      SysInfo.Data.Paths.App = Tools.EnsureFullPath(Application.StartupPath);
      
      // FYI: The following line is an alternative to "Application.StartupPath"
      //SysInfo.Data.Paths.App = Tools.EnsureFullPath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase));

      // Necessary so that the various support folders appear in a more easily accessible location for the developer
      OnlyRunInDebug();                    // Special code, only run while in Debug mode

      string appPath = SysInfo.Data.Paths.App;

      // See if we can retrieve the rest of the SysInfo data from disk
      Tools.OpenData(appPath + "SysInfo.xml", SysInfo.Data);

      // The master copy of much of the 'Admin' info resides in the embedded 'ProductInfo.xml'.
      // We will load this data and then copy it into the 'SysInfo' object.
      DataSet dataSet = Tools.GetProductInfo();

      DataTable table = dataSet.Tables["Admin"];
      if (table != null)
      {
        SysInfo.Data.Admin.AppName = table.Rows[0]["AppName"].ToString();
        SysInfo.Data.Admin.AppFilename = table.Rows[0]["AppFilename"].ToString();
        SysInfo.Data.Admin.AppExtension = Tools.EnsurePrefix(table.Rows[0]["AppExtension"].ToString(), ".").ToLower();
        SysInfo.Data.Admin.ProductID = (DataObjects.Constants.ProductID) Enum.Parse(typeof(DataObjects.Constants.ProductID), table.Rows[0]["ProductID"].ToString(), true);
        SysInfo.Data.Admin.CompanyName = table.Rows[0]["CompanyName"].ToString();
        SysInfo.Data.Admin.RegKeyName = table.Rows[0]["RegKeyName"].ToString();
        SysInfo.Data.Admin.OperatingMode = (DataObjects.Constants.OpMode) Enum.Parse(typeof(DataObjects.Constants.OpMode), table.Rows[0]["OperatingMode"].ToString(), true);     // 1 - Standard     0 - Viewer Mode   

        // "ExpiryDate" may or may not be present
        if (table.Columns["ExpiryDate"] != null)
        {
          string expiryDate = table.Rows[0]["ExpiryDate"].ToString();
          if (expiryDate != "")
            SysInfo.Data.Admin.ExpiryDate = Tools.GetDateFromString(expiryDate);
        }
      }


      // Now that we have the AppName, we can find the location of the Data Files.  Note: At first, the Data folder existed underneath
      // of the app's folder in Program Files.  But this is old school, so we're now going to default it to the correct place in My Documents.
      if (SysInfo.Data.Paths.Data == "")
      {
        //SysInfo.Data.Paths.Data = Tools.EnsureDirectoryExists(SysInfo.Data.Paths.App + @"Data\");   // Old location
        string dataAppPath = Tools.EnsureFullPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData).ToString());
        dataAppPath += SysInfo.Data.Admin.AppName;
        Tools.EnsureDirectoryExists(dataAppPath);
        dataAppPath += @"\Data";
        Tools.EnsureDirectoryExists(dataAppPath);
        SysInfo.Data.Paths.Data = dataAppPath + @"\";
      }

      // Always need to set the following values, all of which are not written out to disk
      string dataPath = SysInfo.Data.Paths.Data;
      
      SysInfo.Data.Paths.Incoming = Tools.EnsureDirectoryExists(dataPath + @"Incoming\", true);
      SysInfo.Data.Paths.Templates = Tools.EnsureDirectoryExists(dataPath + @"Templates\", true);
      SysInfo.Data.Paths.Archive = Tools.EnsureDirectoryExists(dataPath + @"Archive\", true);

      SysInfo.Data.Paths.Help = Tools.EnsureDirectoryExists(appPath + @"Help\");
      SysInfo.Data.Paths.MobileCabs = Tools.EnsureDirectoryExists(appPath + @"MobileCabs\");
      
      SysInfo.Data.Paths.CurrentData = SysInfo.Data.Paths.Data;
      SysInfo.Data.Admin.DataTransferActive = false;    // Default value
      SysInfo.Data.Paths.Temp = Tools.EnsureDirectoryExists(System.IO.Path.GetTempPath() + SysInfo.Data.Admin.AppName + @"\");

      // Populate the Product Version Number too
      SysInfo.Data.Admin.VersionNumber = Assembly.GetExecutingAssembly().GetName().Version.ToString();

      // Check if the GUID needs to be created.  Generally this should only happen the VERY first time the app is run.
      if (SysInfo.Data.Admin.Guid == null || SysInfo.Data.Admin.Guid == "")
      {
        System.Guid guid = System.Guid.NewGuid();
        SysInfo.Data.Admin.Guid = guid.ToString();
      }


      // Ensure that the [Poll] Summaries section of SysInfo is up to date

      // First remove any summary records that no longer have a corresponding template file
      if (SysInfo.Data.Summaries.Templates.Count > 0)
      {
        for (int i = SysInfo.Data.Summaries.Templates.Count - 1; i >= 0; i--) 
        {
          _TemplateSummary summary = SysInfo.Data.Summaries.Templates[i];
          if (!File.Exists(SysInfo.Data.Paths.Templates + summary.Filename + Tools.GetAppExt()))
            SysInfo.Data.Summaries.Templates.Remove(summary);
        }
      }

      // Now add any new summary records that haven't yet been created for existing template files
      string[] templateFiles = Directory.GetFiles(SysInfo.Data.Paths.Templates, Tools.GetAppFilter());
      foreach (string fullFilename in templateFiles)
      {
        string baseName = Tools.StripPathAndExtension(fullFilename);

        if (!SysInfo.Data.Summaries.Templates.ContainsFilename(baseName))
        {
          string pollSummary = "";
          int revision = 0;
          int questionCount = 0;
          string pollGuid = "";
          string lastEditGuid = "";

          Tools.GetPollOverview(fullFilename, out revision, out pollSummary, out questionCount, out pollGuid, out lastEditGuid);

          //_TemplateSummary summary = new _TemplateSummary(SysInfo.Data.Summaries.Templates);
          _TemplateSummary summary = new _TemplateSummary();
          summary.Filename = baseName;
          summary.PollSummary = pollSummary;
          summary.Revision = revision;
          summary.NumQuestions = questionCount;
          summary.PollGuid = pollGuid;
          summary.LastEditGuid = lastEditGuid;

          SysInfo.Data.Summaries.Templates.Add(summary);
        }
      }
    }



    private static void InitializeRefData()
    {
      // Load mobile device data (Note: Not tested yet!)
      //      MobileDevice device = new MobileDevice();
      //      
      //      device.ID = 0;
      //      device.Name = "Casio E-125 PPC";
      //      device.Filename = "Casio E-125.jpg";
      //      Reference.Data.MobileDevices.Add(device);
      //
      //      device.ID = 1;
      //      device.Name = "Casio EG-800";
      //      device.Filename = "Casio EG-800.jpg";
      //      Reference.Data.MobileDevices.Add(device);
      //
      //      device.ID = 2;
      //      device.Name = "Ipaq";
      //      device.Filename = "HP Ipaq.jpg";
      //      Reference.Data.MobileDevices.Add(device);
    }



    /// <summary>
    /// Check if any parameters were passed to the executable.  
    /// If so, then handle them accordingly.
    /// </summary>
    /// <param name="args">The command line arguments</param>
    private static void HandleStartupParameters(string[] args)
    {
      if (args.Length > 0)
      {
        for (int i = 0; i < args.Length; i++)
        {
          string param = args[i].ToLower();

          if (param.StartsWith(@"\") || param.StartsWith(@"/") || param.StartsWith("-"))
          {
            param = param.Substring(1);

            if (param == "datatransfer")
              SysInfo.Data.Admin.DataTransferActive = true;    // This global flag will be checked throughout the application

            else if (param.Substring(0,9).ToLower() == "uninstall")      // Format:  "/uninstall={...GUID...}"
            {
              int pos = param.IndexOf("=");
              if (pos != -1)
              {
                string guid = param.Substring(pos + 1);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.System);
                Uninstall = true;   // Set a module level flag to suppress code in the main routine
                ProcessStartInfo processInfo = new ProcessStartInfo(path + "\\msiexec.exe", "/i " + guid);
                Process.Start(processInfo);
              }
            }
          }
          else
          {
            if (param.EndsWith(SysInfo.Data.Admin.AppExtension.ToLower()))   // If the app was activated via an external data file
              StartupFilename = Tools.GetLongName(param);                    // then prepare to open the file.
          }
        }
      }
    }


    private static void InitializeCopyData()
    {
      // Create a new instance of the class
      copyData = new CopyData();
			
      // Assign the handle
      //copyData.AssignHandle(this.Handle);
      copyData.AssignHandle(FormMain.Handle);

      // Create the named channels to send and receive on.  The name is arbitary.
      copyData.Channels.Add(SysInfo.Data.Admin.AppName);

      // Hook up event notifications whenever a message is received:
      copyData.DataReceived += new DataReceivedEventHandler(copyData_DataReceived);
    }


    /// <summary>
    /// Fired whenever message is received from another instance of the application.
    /// </summary>
    /// <param name="sender">The CopyData class which receieved the data</param>
    /// <param name="e">Data that was received from the other instance</param>
    private static void copyData_DataReceived(object sender, DataReceivedEventArgs e)
    {			
      if (e.ChannelName.Equals(SysInfo.Data.Admin.AppName))
      {
        string fileName = (string) e.Data;
        FormMain.PreparePoll(fileName);
      }
    }


    public void RegisterPoll(ControllerClass controller)
    {
      // Store the controller in the collection of controller objects
      Polls.Add(controller);
      FormMain.SetMenuItems(Polls.Count);
    }

    public void UnRegisterPoll(ControllerClass controller)
    {
      // Remove the controller in the collection of controller objects
      Polls.Remove(controller);
      FormMain.SetMenuItems(Polls.Count);
    }


    [Conditional("DEBUG")]
    private static void OnlyRunInDebug()
    {
      // Necessary because of the difference where the EXE sits during development and upon installation.
      // This initial code allows external files to be properly loaded.
      string path = Application.StartupPath; 

      if (path.LastIndexOf("bin\\Debug") != -1)
      {
        path = path.Substring(0, path.LastIndexOf("bin\\Debug"));
        SysInfo.Data.Paths.App = path;
      }
    } 


    public static void ShutDownApp(object sender, EventArgs e)
    {
      Debug.WriteLine("Ending application...");

      // This imports any residual data files.  These files would exist if:
      //  - New data files were imported for which the user was editing the master copy at the time
      //  - AND the user chose not to import these new data files during the session.
      FormMain.ImportNewDataFiles();   

      // Note: This new boolean setting was introduced in Jan 2007.
      if (SysInfo.Data.Options.DataXfer.UnattendedSync)
      {
        // Note: The introduction of additional parameter handling code means that all startup switches,
        //       like "DataTransfer", must now be prefixed with a "/" or a "\" or a "-".
        #if (DEBUG)
        //Note: This next if-else clause had to be introduced because early deployments of the app were done with Debug compilations
        if (Application.StartupPath.LastIndexOf("bin\\Debug") != -1)
          RegistryTools.SetAutoStartOnConnect(SysInfo.Data.Paths.App + "bin\\Debug\\" + SysInfo.Data.Admin.AppFilename, "/DataTransfer");
        else
          RegistryTools.SetAutoStartOnConnect(SysInfo.Data.Paths.App + SysInfo.Data.Admin.AppFilename, "/DataTransfer");
        #else        
        RegistryTools.SetAutoStartOnConnect(SysInfo.Data.Paths.App + SysInfo.Data.Admin.AppFilename, "/DataTransfer");
        #endif
      }

      // Testing has revealed that we can leave 'GuestOnly' set to 1 all the time since ActiveSync
      // seems able to distinguish the primary Pocket PC from other ones.
      // 2006-06-18 - RW - I ran into a situation where this prevented me from establishing a partnership, so we're now going to reset it as originally planned!
      RegistryTools.SetGuestOnly(false);

      // Save the SysInfo object to disk
      string sysInfoFile = SysInfo.Data.Paths.App + "SysInfo.xml";
      Tools.SaveData(sysInfoFile, SysInfo.Data, null, DataObjects.Constants.ExportFormat.XML);
      File.SetAttributes(sysInfoFile, FileAttributes.Hidden);      // This is separate because it doesn't work with the CF

      Tools.HandleTempDirectory(SysInfo.Data.Paths.Temp, false);   // Tries to erase Temp folder and all files within
      Application.ExitThread();
    }



    /// <summary>
    /// This method checks whether the Registry is correctly setup to associate PP data files with the PP.exe app.
    /// If it isn't then it makes it so.
    /// Note: In the future we could make the Check more sophisticated to determine whether the Registry is already
    ///       setup but simply pointing toward another application (like a future competitor of Pocket Pollster).
    ///       Then we should ask whether to change the file association settings.
    /// </summary>
    private static void CheckFileAssociations()
    {
      FileAssociation FA = new FileAssociation();
      FA.Extension = "pp";
      FA.ContentType = "data/PocketPollster";
      FA.FullName = "PocketPollster Data File";
      FA.ProperName = "PocketPollster";

      string path = Tools.EnsureFullPath(Application.StartupPath);  // Points to the location of the executable
      FA.AddCommand("open", path + SysInfo.Data.Admin.AppFilename + " %1");
      FA.IconPath = path + SysInfo.Data.Admin.AppFilename;
      FA.IconIndex = 0;

      if (FA.Check(path + SysInfo.Data.Admin.AppFilename + " %1") == false)
        FA.Create();
    }



//    /// <summary>
//    /// Opens the embedded 'ProductInfo.xml' file and copies the data therein into a DataSet.
//    /// This file contains high-level information about the product and can be readily changed before a compilation.
//    /// An identical version of this XML file exists in PocketPC, so when one is updated, so should the other.
//    /// </summary>
//    /// <returns></returns>
//    public static DataSet GetProductInfo()
//    {
//      Assembly assembly = Assembly.GetCallingAssembly();
//      Stream stream = assembly.GetManifestResourceStream("Desktop.ProductInfo.xml");
//
//      //Create an XMLTextReader object used for reading from the stream
//      XmlTextReader reader = new XmlTextReader(stream);
//
//      DataSet dataSet = new DataSet();
//      dataSet.ReadXml(reader);
//      
//      reader.Close();
//      stream.Close();
//
//      return dataSet;
//    }






    // This method is simply used to test various new code.  Once tested & working, the code is removed, leaving the method blank.
    // Note: Keep this method at the very bottom of the code, so it can always be easily accessed.
    public static void TestCode()
    {
      //CultureInfo culture = CultureInfo.CurrentCulture;
//      foreach(_TemplateSummary summary in SysInfo.Data.Summaries.Templates)
//      {
//        Debug.WriteLine(summary.Filename + "  " + summary.Revision + "  " + summary.PollGuid);
//      }
//
//      Debug.WriteLine(SysInfo.Data.Summaries.Templates[3]);




    }




	}
}
