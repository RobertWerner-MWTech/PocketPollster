using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenNETCF;
using OpenNETCF.IO;
using OpenNETCF.Win32;
using OpenNETCF.Windows.Forms;
using System.Runtime.InteropServices;

using DataObjects;
using Multimedia;
using RefData;



namespace PocketPC
{
  // Define Aliases
  using CFSysInfo = DataObjects.CFSysInfo;
  using ProductID = DataObjects.Constants.ProductID;
  using Platform = DataObjects.Constants.MobilePlatform;
  using InstructType = DataObjects.Constants.InstructionsType;
  using ExportFormat = DataObjects.Constants.ExportFormat;
  using SelectFileMode = DataObjects.Constants.SelectFileMode;
  using RespondentInfo = DataObjects.Tools.RespondentInfo;  
  using _ActivePollSummary = _Summaries._ActivePollSummary;


	/// <summary>
	/// The startup form of the mobile app.
	/// </summary>
	public class frmMobile : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.Timer batteryTimer;
    private System.Windows.Forms.MenuItem menuFile;
    private System.Windows.Forms.MenuItem menuFileNew;
    private System.Windows.Forms.MenuItem menuFileOpen;
    private System.Windows.Forms.MenuItem menuHelp;
    private System.Windows.Forms.MenuItem menuTools;
    private System.Windows.Forms.MenuItem menuToolsStartMenu;
    private System.Windows.Forms.MenuItem menuItem1;
    private System.Windows.Forms.MenuItem menuFileExit;
    private System.Windows.Forms.MenuItem menuHelpAbout;
    private System.Windows.Forms.Label labelBattery;
    private System.Windows.Forms.Label labelUser;
    private System.Windows.Forms.Panel panelStartMenu;
    private System.Windows.Forms.Panel panelStartMenuInner;
    private OpenNETCF.Windows.Forms.PictureBoxEx pictureLogo;
    private OpenNETCF.Windows.Forms.ButtonEx buttonCloseStartMenu;
    private OpenNETCF.Windows.Forms.ButtonEx buttonFileNew;
    private OpenNETCF.Windows.Forms.ButtonEx buttonFileOpen;
    private OpenNETCF.Windows.Forms.ButtonEx buttonFileReview;
    private System.Windows.Forms.MenuItem menuFileReview;
    private System.Windows.Forms.Panel panelExpired;
    private System.Windows.Forms.Label labelExpired1;
    private System.Windows.Forms.Label labelExpired2;
    private System.Windows.Forms.Label labelExpiredWebsite;


    #region Module Variables

    //public Poll PollModel;            // Define new Model (ie. Poll class)
    public Timer checkNameTimer;
    private BatteryLife batLife;
    private int currLogoMode = 0;
    bool Repositioning = false;         // Set true when the repositioning is occurring
    
    #endregion



    /// <summary>
    /// This is the constructor for this form.
    /// </summary>
		public frmMobile()
		{
      Cursor.Current = Cursors.WaitCursor;
      this.BringToFront();
      InitializeComponent();
      InitializeMenus();

      // Instantiate BatteryLife control (bug in OpenNETCF requires it done this way)
      this.batLife.Bounds = new Rectangle(10, 10, 100, 20);
      this.batLife.Visible = false;

      TestCode();  // Only used during debugging; normally empty

      ShowUser();
      ShowBatteryLevel();

      this.batteryTimer.Interval = 5000;
      this.batteryTimer.Tick += new System.EventHandler(this.BatteryTimer_Tick);
      this.batteryTimer.Enabled = true;

      ApplicationEx.ThreadExit += new EventHandler(ShutDownApp);

      // I'm not sure why, but this method is not working correctly on some Pocket PCs
//      if (ToolsCF.IsAppExpired())
//        Freeze();
//      else
//      {
//      }

      // Checks whether the FirstName and LastName info has yet been entered.
      // In order to display as a dialog box we need to display it a little
      // after frmMobile has been displayed.
      checkNameTimer = new Timer();      // Create timer
      checkNameTimer.Interval = 100;     // Timer interval
      checkNameTimer.Tick += new EventHandler(CheckNameEventHandler);
      checkNameTimer.Enabled = true;

      this.Resize += new System.EventHandler(this.frmMobile_Resize);    
      Cursor.Current = Cursors.Default;
    }



    /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMobile));
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuFile = new System.Windows.Forms.MenuItem();
      this.menuFileNew = new System.Windows.Forms.MenuItem();
      this.menuFileOpen = new System.Windows.Forms.MenuItem();
      this.menuFileReview = new System.Windows.Forms.MenuItem();
      this.menuItem1 = new System.Windows.Forms.MenuItem();
      this.menuFileExit = new System.Windows.Forms.MenuItem();
      this.menuTools = new System.Windows.Forms.MenuItem();
      this.menuToolsStartMenu = new System.Windows.Forms.MenuItem();
      this.menuHelp = new System.Windows.Forms.MenuItem();
      this.menuHelpAbout = new System.Windows.Forms.MenuItem();
      this.batteryTimer = new System.Windows.Forms.Timer();
      this.batLife = new OpenNETCF.Windows.Forms.BatteryLife();
      this.pictureLogo = new OpenNETCF.Windows.Forms.PictureBoxEx();
      this.labelBattery = new System.Windows.Forms.Label();
      this.labelUser = new System.Windows.Forms.Label();
      this.panelStartMenu = new System.Windows.Forms.Panel();
      this.panelStartMenuInner = new System.Windows.Forms.Panel();
      this.buttonCloseStartMenu = new OpenNETCF.Windows.Forms.ButtonEx();
      this.buttonFileNew = new OpenNETCF.Windows.Forms.ButtonEx();
      this.buttonFileOpen = new OpenNETCF.Windows.Forms.ButtonEx();
      this.buttonFileReview = new OpenNETCF.Windows.Forms.ButtonEx();
      this.panelExpired = new System.Windows.Forms.Panel();
      this.labelExpiredWebsite = new System.Windows.Forms.Label();
      this.labelExpired2 = new System.Windows.Forms.Label();
      this.labelExpired1 = new System.Windows.Forms.Label();
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.Add(this.menuFile);
      this.mainMenu.MenuItems.Add(this.menuTools);
      this.mainMenu.MenuItems.Add(this.menuHelp);
      // 
      // menuFile
      // 
      this.menuFile.MenuItems.Add(this.menuFileNew);
      this.menuFile.MenuItems.Add(this.menuFileOpen);
      this.menuFile.MenuItems.Add(this.menuFileReview);
      this.menuFile.MenuItems.Add(this.menuItem1);
      this.menuFile.MenuItems.Add(this.menuFileExit);
      this.menuFile.Text = "File";
      // 
      // menuFileNew
      // 
      this.menuFileNew.Text = "New";
      this.menuFileNew.Click += new System.EventHandler(this.menuFileNew_Click);
      // 
      // menuFileOpen
      // 
      this.menuFileOpen.Text = "Open";
      this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
      // 
      // menuFileReview
      // 
      this.menuFileReview.Text = "&Review";
      this.menuFileReview.Click += new System.EventHandler(this.menuFileReview_Click);
      // 
      // menuItem1
      // 
      this.menuItem1.Text = "-";
      // 
      // menuFileExit
      // 
      this.menuFileExit.Text = "Exit";
      this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
      // 
      // menuTools
      // 
      this.menuTools.MenuItems.Add(this.menuToolsStartMenu);
      this.menuTools.Text = "Tools";
      // 
      // menuToolsStartMenu
      // 
      this.menuToolsStartMenu.Text = "Show Start Menu";
      this.menuToolsStartMenu.Click += new System.EventHandler(this.menuToolsStartMenu_Click);
      // 
      // menuHelp
      // 
      this.menuHelp.MenuItems.Add(this.menuHelpAbout);
      this.menuHelp.Text = "Help";
      // 
      // menuHelpAbout
      // 
      this.menuHelpAbout.Text = "About";
      this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
      // 
      // batteryTimer
      // 
      this.batteryTimer.Interval = 4000;
      // 
      // pictureLogo
      // 
      this.pictureLogo.Location = new System.Drawing.Point(80, 32);
      this.pictureLogo.Size = new System.Drawing.Size(72, 56);
      this.pictureLogo.TransparentColor = System.Drawing.Color.FromArgb(((System.Byte)(176)), ((System.Byte)(196)), ((System.Byte)(221)));
      // 
      // labelBattery
      // 
      this.labelBattery.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelBattery.ForeColor = System.Drawing.Color.SteelBlue;
      this.labelBattery.Location = new System.Drawing.Point(153, 247);
      this.labelBattery.Size = new System.Drawing.Size(80, 16);
      this.labelBattery.Text = "...";
      this.labelBattery.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // labelUser
      // 
      this.labelUser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelUser.ForeColor = System.Drawing.Color.SteelBlue;
      this.labelUser.Location = new System.Drawing.Point(7, 247);
      this.labelUser.Size = new System.Drawing.Size(80, 16);
      this.labelUser.Text = "...";
      // 
      // panelStartMenu
      // 
      this.panelStartMenu.BackColor = System.Drawing.Color.Black;
      this.panelStartMenu.Controls.Add(this.panelStartMenuInner);
      this.panelStartMenu.Location = new System.Drawing.Point(40, 112);
      this.panelStartMenu.Size = new System.Drawing.Size(160, 122);
      this.panelStartMenu.Visible = false;
      // 
      // panelStartMenuInner
      // 
      this.panelStartMenuInner.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(218)), ((System.Byte)(228)), ((System.Byte)(248)));
      this.panelStartMenuInner.Controls.Add(this.buttonCloseStartMenu);
      this.panelStartMenuInner.Controls.Add(this.buttonFileNew);
      this.panelStartMenuInner.Controls.Add(this.buttonFileOpen);
      this.panelStartMenuInner.Controls.Add(this.buttonFileReview);
      this.panelStartMenuInner.Location = new System.Drawing.Point(2, 2);
      this.panelStartMenuInner.Size = new System.Drawing.Size(156, 118);
      // 
      // buttonCloseStartMenu
      // 
      this.buttonCloseStartMenu.AutoSize = true;
      this.buttonCloseStartMenu.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(218)), ((System.Byte)(228)), ((System.Byte)(248)));
      this.buttonCloseStartMenu.BorderColor = System.Drawing.Color.Silver;
      this.buttonCloseStartMenu.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular);
      this.buttonCloseStartMenu.Location = new System.Drawing.Point(140, 0);
      this.buttonCloseStartMenu.Size = new System.Drawing.Size(16, 20);
      this.buttonCloseStartMenu.Text = "X";
      this.buttonCloseStartMenu.TextAlign = OpenNETCF.Drawing.ContentAlignment.MiddleCenter;
      this.buttonCloseStartMenu.Click += new System.EventHandler(this.buttonCloseStartMenu_Click);
      // 
      // buttonFileNew
      // 
      this.buttonFileNew.BackColor = System.Drawing.Color.Lavender;
      this.buttonFileNew.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
      this.buttonFileNew.Location = new System.Drawing.Point(19, 18);
      this.buttonFileNew.Size = new System.Drawing.Size(116, 24);
      this.buttonFileNew.Text = "Start New Poll";
      this.buttonFileNew.TextAlign = OpenNETCF.Drawing.ContentAlignment.MiddleCenter;
      this.buttonFileNew.Click += new System.EventHandler(this.buttonFileNew_Click);
      // 
      // buttonFileOpen
      // 
      this.buttonFileOpen.BackColor = System.Drawing.Color.Lavender;
      this.buttonFileOpen.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
      this.buttonFileOpen.Location = new System.Drawing.Point(19, 51);
      this.buttonFileOpen.Size = new System.Drawing.Size(116, 24);
      this.buttonFileOpen.Text = "Resume A Poll";
      this.buttonFileOpen.TextAlign = OpenNETCF.Drawing.ContentAlignment.MiddleCenter;
      this.buttonFileOpen.Click += new System.EventHandler(this.buttonFileOpen_Click);
      // 
      // buttonFileReview
      // 
      this.buttonFileReview.BackColor = System.Drawing.Color.Lavender;
      this.buttonFileReview.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
      this.buttonFileReview.Location = new System.Drawing.Point(19, 84);
      this.buttonFileReview.Size = new System.Drawing.Size(116, 24);
      this.buttonFileReview.Text = "Review Data";
      this.buttonFileReview.TextAlign = OpenNETCF.Drawing.ContentAlignment.MiddleCenter;
      this.buttonFileReview.Click += new System.EventHandler(this.buttonFileReview_Click);
      // 
      // panelExpired
      // 
      this.panelExpired.BackColor = System.Drawing.Color.AliceBlue;
      this.panelExpired.Controls.Add(this.labelExpiredWebsite);
      this.panelExpired.Controls.Add(this.labelExpired2);
      this.panelExpired.Controls.Add(this.labelExpired1);
      this.panelExpired.Location = new System.Drawing.Point(32, 64);
      this.panelExpired.Size = new System.Drawing.Size(176, 128);
      this.panelExpired.Visible = false;
      // 
      // labelExpiredWebsite
      // 
      this.labelExpiredWebsite.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular);
      this.labelExpiredWebsite.ForeColor = System.Drawing.Color.Blue;
      this.labelExpiredWebsite.Location = new System.Drawing.Point(8, 104);
      this.labelExpiredWebsite.Size = new System.Drawing.Size(160, 16);
      this.labelExpiredWebsite.Text = "www.PocketPollster.com";
      this.labelExpiredWebsite.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // labelExpired2
      // 
      this.labelExpired2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular);
      this.labelExpired2.ForeColor = System.Drawing.Color.Firebrick;
      this.labelExpired2.Location = new System.Drawing.Point(8, 60);
      this.labelExpired2.Size = new System.Drawing.Size(160, 32);
      this.labelExpired2.Text = "Please visit our website to obtain another copy.";
      this.labelExpired2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // labelExpired1
      // 
      this.labelExpired1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular);
      this.labelExpired1.ForeColor = System.Drawing.Color.Firebrick;
      this.labelExpired1.Location = new System.Drawing.Point(8, 6);
      this.labelExpired1.Size = new System.Drawing.Size(160, 48);
      this.labelExpired1.Text = "We\'re sorry, but this version of Pocket Pollster has expired";
      this.labelExpired1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // frmMobile
      // 
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ControlBox = false;
      this.Controls.Add(this.labelBattery);
      this.Controls.Add(this.labelUser);
      this.Controls.Add(this.panelStartMenu);
      this.Controls.Add(this.pictureLogo);
      this.Controls.Add(this.batLife);
      this.Controls.Add(this.panelExpired);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.Closed += new System.EventHandler(this.frmMobile_Closed);

    }

		#endregion


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args) 
		{
      bool initOnly;

      InitializeCFSysInfo();
      HandleStartupParameters(args, out initOnly);

      // Get the PP Device GUID from the registry.  If one doesn't exist then create a new one and store it.
      ToolsCF.CheckGuid();

      // Get the actual version number from the executable and ensure the registry has this correct version number in it.
      // Note: The registry storage is required because there's currently no way for the Windows DataXfer app to directly
      //       obtain it from the executable itself.
      ToolsCF.GetVersionNumber(true);
      
      OnlyRunInDebug();

      // If we're running this only in Initialization mode then we will
      // not actually display the form but instead just quietly exit.
      if (! initOnly)
      {
        ToolsCF.SipShowIM(0);   // Might as well ensure SIP is collapsed
        ApplicationEx.Run(new frmMobile());
      }
    }


    #region Initialization

    private void RepositionControls()
    {
      if (!Repositioning)
      {
        Repositioning = true;

        int wid, hgt, aHgt;
        ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt);

        labelUser.Location = new Point(7, aHgt - labelUser.Height);
        labelBattery.Location = new Point(wid - 7 - labelBattery.Width, labelUser.Top);

        // Logic to handle display of start menu and logo
        if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
        {
          // Such a small screen so no start menu
          panelStartMenu.Visible = false;

          // Show a logo; it'll be shrunk if necessary
          ShowLogo(2);
        }
        else
        {
          if (hgt > wid)
          {
            // Position start menu just below small logo image
            panelStartMenu.Location = new Point((wid - panelStartMenu.Width) / 2, 8 + 100 + 8);

            ShowLogo(panelStartMenu.Visible == true ? 1 : 3);
          }
          else
          {
            // Not enough space to display both logo and start menu (and side-by-side doesn't look good) so we'll just center menu
            panelStartMenu.Location = new Point((wid - panelStartMenu.Width) / 2, (aHgt - panelStartMenu.Height) / 2);

            ShowLogo(panelStartMenu.Visible == true ? 0 : 3);
          }
        }

        Repositioning = false;  // Signal that we're done repositioning
      }
    }


    #region CFSysInfo

    public static void InitializeCFSysInfo()
    {
      // Note: We could also technically get the AppPath from the registry but this way it's fullproof.
      string appPath = Tools.EnsureFullPath(ApplicationEx.StartupPath);

      // Testing revealed that datasets in the CF have a different structure than datasets
      // in Windows.  So, for now, we've essentially duplicated all the Importing functionality
      // in ToolsCF.  Hopefully in the future we can avoid using datasets and have just one copy
      // of the code in DataObjects.Tools.
      Tools.OpenData(appPath + "SysInfo.xml", CFSysInfo.Data, null);

      // Unlike on the desktop, NONE of the paths are stored in SysInfo.xml.  We simply use
      // the 'SysInfo.Data.MobilePaths' object as a convenient central place for retrieving paths
      // during program operation.  There are some other properties though that we'd like to
      // retrieve from this file.
      CFSysInfo.Data.MobilePaths.App = appPath;

      // If the actual location of the executable is different from where
      // the registry says it is then correct the entry in the registry.
      if (appPath != RegistryCF.GetAppPath())
        RegistryCF.SetAppPath(appPath);
      
      // During installation the user may have chosen to store the data files in a 
      // different location such as built-in storage or a removable storage card.
      // This test is also necessary when running the emulator, since it'll never be preset there.
      string dataPath = RegistryCF.GetDataPath();
      if ((dataPath == null) || (dataPath == ""))
      {
        dataPath = appPath + @"Data\";
        RegistryCF.SetDataPath(dataPath);
      }
      else
      {
        // Deconstruct and then rebuild dataPath, creating required folders along the way
        dataPath = Tools.EnsureFullPath(dataPath);
        String[] pathPieces = dataPath.Split(new char[] {'\\'});
        string path = "";

        for (int i = 0; i < pathPieces.Length; i++)
        {
          string pathPortion = pathPieces[i];
          if (pathPortion != "")
          {
            if (path == "")
              path = @"\" + pathPortion;
            else
            {
              path = path + @"\" + pathPortion;
              Tools.EnsureDirectoryExists(path);
            }
          }
        }

        if (! dataPath.ToLower().EndsWith(@"\data\"))
          dataPath = dataPath + @"Data\";
      }

      CFSysInfo.Data.MobilePaths.Data = Tools.EnsureDirectoryExists(dataPath);
      CFSysInfo.Data.MobilePaths.Completed = Tools.EnsureDirectoryExists(dataPath + @"Completed\");
      CFSysInfo.Data.MobilePaths.Downloaded = Tools.EnsureDirectoryExists(dataPath + @"Downloaded\");
      CFSysInfo.Data.MobilePaths.Templates = Tools.EnsureDirectoryExists(dataPath + @"Templates\");
      CFSysInfo.Data.MobilePaths.Help = Tools.EnsureDirectoryExists(appPath + @"Help\");

      // The master copy of much of the 'Admin' info resides in the embedded 'ProductInfo.xml'.
      // We will load this data and then copy it into the 'CFSysInfo' object.
      DataSet dataSet = Tools.GetProductInfo();

      DataTable table = dataSet.Tables["Admin"];
      if (table != null)
      {
        CFSysInfo.Data.MobileAdmin.AppName = table.Rows[0]["AppName"].ToString();
        CFSysInfo.Data.MobileAdmin.AppFilename = table.Rows[0]["AppFilename"].ToString();
        CFSysInfo.Data.MobileAdmin.AppExtension = Tools.EnsurePrefix(table.Rows[0]["AppExtension"].ToString(), ".").ToLower();
        CFSysInfo.Data.MobileAdmin.ProductID = (ProductID) EnumEx.Parse(typeof(ProductID), table.Rows[0]["ProductID"].ToString(), true);
        CFSysInfo.Data.MobileAdmin.CompanyName = table.Rows[0]["CompanyName"].ToString();
        CFSysInfo.Data.MobileAdmin.RegKeyName = table.Rows[0]["RegKeyName"].ToString();
      }

      // It may seem redundant to use this method again here, since it was executed in Main() but remember
      // that there's a special noUI mode executed from the desktop that would never reach here.  Thus we
      // can't depend on this next line being executed every time.
      CFSysInfo.Data.MobileAdmin.VersionNumber = ToolsCF.GetVersionNumber(false);
      
      // The OS Version is required for certain things like whether to display a special gradient background.
      CFSysInfo.Data.MobileAdmin.OSVersion = ToolsCF.GetOSVersion();

      // Note: Screen Dimension values to be populated elsewhere

      // Need to carry out the replacement because older Pocket PCs appear as "Palm PC2" but spaces aren't allowed in Enums.
      string platform = OpenNETCF.EnvironmentEx.PlatformName.Replace(" ", "_");
      CFSysInfo.Data.DeviceSpecs.Platform = (Platform) EnumEx.Parse(typeof(DataObjects.Constants.MobilePlatform), platform, true);


      // Summaries section - We're initially going to check whether there's a one-to-one relationship between ActivePoll & Template
      //                     records and their corresponding files.  If there are any extra records then we'll delete them here.  If
      //                     the reverse is true then we'll wait until the poll summaries are actually needed before creating them.
      
      // First look at the Active Polls
      string appExt = Tools.GetAppExt();
      for (int i = CFSysInfo.Data.Summaries.ActivePolls.Count - 1; i >= 0; i--)
      {
        _ActivePollSummary apSummary = CFSysInfo.Data.Summaries.ActivePolls[i];
        if (! File.Exists(CFSysInfo.Data.MobilePaths.Data + apSummary.Filename + appExt))
          CFSysInfo.Data.Summaries.ActivePolls.RemoveAt(i);
      }

      // Now examine the Templates
      for (int i = CFSysInfo.Data.Summaries.Templates.Count - 1; i >= 0; i--)
      {
        _TemplateSummary tempSummary = CFSysInfo.Data.Summaries.Templates[i];
        if (! File.Exists(CFSysInfo.Data.MobilePaths.Templates + tempSummary.Filename + appExt))
          CFSysInfo.Data.Summaries.Templates.RemoveAt(i);
      }
    }

    #endregion


    /// <summary>
    /// Checks if any parameters were passed to the executable.
    /// </summary>
    /// <param name="args"></param>       // 1 or more arguments passed to the EXE
    /// <param name="initOnly"></param>   // true - App is being run for initialization purposes only
    private static void HandleStartupParameters(string[] args, out bool initOnly)
    {
      initOnly = false;
      
      if (args.Length > 0)
      {
        for (int i = 0; i < args.Length; i++)
        {
          string param = args[i].ToLower();
          if (param.StartsWith(@"\") || param.StartsWith(@"/") || param.StartsWith("-"))
            param = param.Substring(1);

          // 2006-06-07 - Alpha Testing has revealed numerous DataXfer installation issues on some computers.  So I've removed
          //              all calls with this parameter and am handling the initial creation of the GUID and Version in the
          //              CF Registry differently.
          if (param.Substring(0,4) == "init")
            initOnly = true;

          else if (param.Substring(0,7) == "settime")
          {
            ToolsCF.UpdateLocalTime();
            initOnly = true;
          }


          // Can add logic to handle other kinds of
          // parameters by converting 'if' to switch/case'.

        }
      }
    }


    [Conditional("DEBUG")]
    private static void OnlyRunInDebug()
    {
      // Currently no code here

      Debug.WriteLine("");
    } 

    #endregion


    #region CloseApplication

    private void frmMobile_Closed(object sender, System.EventArgs e)
    {
      ApplicationEx.Exit();
    }

    public void ShutDownApp(object sender, EventArgs e)
    {
      batteryTimer.Enabled = false;

      this.Close();
      this.Dispose();

      // Save the CFSysInfo object to disk
      Tools.SaveData(CFSysInfo.Data.MobilePaths.App + "SysInfo.xml", CFSysInfo.Data, null, DataObjects.Constants.ExportFormat.XML);
      // I want to keep this next line separate from the generic SaveData routine so that the latter
      // can be more easily updated should changes be required to it in the future.
      FileEx.SetAttributes(CFSysInfo.Data.MobilePaths.App + "SysInfo.xml", FileAttributes.Hidden);

      Application.Exit();
    }

    #endregion


    #region ScreenDisplay

    /// <summary>
    /// Displays the username, if it exists
    /// </summary>
    private void ShowUser()
    {
      string userName = CFSysInfo.Data.MobileOptions.PrimaryUser;

      if (userName == "")
        labelUser.Visible = false;
      else
      {
        labelUser.Text = "User: " + userName;
        labelUser.Visible = true;
      }
    }


    private void BatteryTimer_Tick(object sender, EventArgs e)
    {
      ShowBatteryLevel();
    }

    private void ShowBatteryLevel()
    {
      if (batLife.ACPowerStatus.ToString() == "Online")
        labelBattery.Text = "AC Power";
      else
      {
        byte batLevel = batLife.BatteryLifePercent;
        if (batLevel > 100)     // Sometimes the battery level is initially reported
          batLevel = 99;        // inaccurately, such as 255%

        labelBattery.Text = "Battery: " + batLevel.ToString() + "%";
      }
    }


    /// <summary>
    /// Displays or hides the start menu, adjusting the size of the logo accordingly.
    /// </summary>
    /// <param name="visibility"></param>
    private void DisplayStartMenu (bool visibility)
    {
      menuToolsStartMenu.Checked = visibility;
      panelStartMenu.Visible = visibility;
      RepositionControls();
    }


    /// <summary>
    /// Displays the Pocket Pollster logo in one of several ways.
    /// </summary>
    /// <param name="mode"></param>  0 - Hide   1 - Small & at top   2 - Small & centered   3 - Large & centered
    private void ShowLogo(int mode)
    {
      currLogoMode = mode;

      int scrnWid = CFSysInfo.Data.DeviceSpecs.ScreenWidth;
      int scrnHgt = CFSysInfo.Data.DeviceSpecs.ScreenHeight;
      int availHgt = CFSysInfo.Data.DeviceSpecs.AvailHeight;

      switch (mode)
      {
        case 0:   // No logo
          pictureLogo.Visible = false;
          break;

        case 1:   // Small logo, positioned near top of screen
          pictureLogo.Image = Multimedia.Images.GetImage("Logo-small");
          pictureLogo.Width = pictureLogo.Image.Width;
          pictureLogo.Height = pictureLogo.Image.Height;
          pictureLogo.Left = (scrnWid - pictureLogo.Width) / 2;
          pictureLogo.Top = 8;

          pictureLogo.Visible = true;
          break;

        case 2:   // Small logo, centered in both directions
          pictureLogo.Image = Multimedia.Images.GetImage("Logo-small");

          if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
          {
            int minDim = Math.Min(scrnWid, availHgt);
            pictureLogo.Width = minDim / 2;
            pictureLogo.Height = minDim / 2;
          }          
          else  // PocketPC
          {
            pictureLogo.Width = pictureLogo.Image.Width;
            pictureLogo.Height = pictureLogo.Image.Height;
          }

          pictureLogo.Left = (scrnWid - pictureLogo.Width) / 2;
          pictureLogo.Top = (labelUser.Top - pictureLogo.Height) / 2;
          pictureLogo.Visible = true;
          break;

        case 3:   // Large logo, centered in both directions
          pictureLogo.Image = Multimedia.Images.GetImage("Logo-large");

          // Determine how large the 240x240 logo should be.
          // Note: At this time, this test will always be true, but perhaps it will not
          //       with a future device.
          if (scrnWid < 280 || availHgt < 280)
          {
            double minDim = Math.Min(scrnWid, availHgt);
            pictureLogo.Width = Convert.ToInt32(minDim * 0.75);
            pictureLogo.Height = Convert.ToInt32(minDim * 0.75);
          }          
          else
          {
            pictureLogo.Width = pictureLogo.Image.Width;
            pictureLogo.Height = pictureLogo.Image.Height;
          }

          pictureLogo.Left = (scrnWid - pictureLogo.Width) / 2;
          
          if (scrnWid < scrnHgt)   // Portrait
            pictureLogo.Top = (2 * availHgt - scrnHgt - pictureLogo.Height);
          else
            pictureLogo.Top = (availHgt - pictureLogo.Height) / 2;
          
          pictureLogo.Visible = true;
          break;

        default:
          Debug.Fail("Unaccounted for mode: " + mode.ToString(), "frmMobile.ShowLogo");
          break;
      }
    }

    #endregion


    #region Menus

    private void buttonFileNew_Click(object sender, System.EventArgs e)
    {
      OpenFile(true);
    }

    private void buttonFileOpen_Click(object sender, System.EventArgs e)
    {
      OpenFile(false);
    }

    private void buttonFileReview_Click(object sender, System.EventArgs e)
    {
      ReviewData();
    }

    private void menuFileNew_Click(object sender, System.EventArgs e)
    {
      OpenFile(true);
    }

    private void menuFileOpen_Click(object sender, System.EventArgs e)
    {
      OpenFile(false);
    }

    private void menuFileReview_Click(object sender, System.EventArgs e)
    {
      ReviewData();
    }

    private void menuFileExit_Click(object sender, System.EventArgs e)
    {
      ApplicationEx.Exit();
    }


    private void menuToolsStartMenu_Click(object sender, System.EventArgs e)
    {
      menuToolsStartMenu.Checked = !menuToolsStartMenu.Checked;                    // Toggle the setting
      panelStartMenu.Visible = menuToolsStartMenu.Checked;
      CFSysInfo.Data.MobileOptions.StartWithMenu = menuToolsStartMenu.Checked;
      RepositionControls();
    }


    private void menuHelpAbout_Click(object sender, System.EventArgs e)
    {
      frmAbout aboutForm = new frmAbout();
    }


    /// <summary>
    /// Setup different menus for the SmartPhone vs. the PocketPC.
    /// </summary>
    private void InitializeMenus()
    {
      if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
      {
        menuTools.MenuItems.Remove(menuToolsStartMenu);
        CFSysInfo.Data.MobileOptions.StartWithMenu = false;
      }
    }

    #endregion



    private void frmMobile_Resize(object sender, System.EventArgs e)
    {
      RepositionControls();
    }


    private void buttonCloseStartMenu_Click(object sender, System.EventArgs e)
    {
      DisplayStartMenu(false);
    }


    /// <summary>
    /// This is the event called by the initial startup timer.  It is only called once and then the timer is shut off.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    private void CheckNameEventHandler(object obj, EventArgs e)
    {
      CheckNameInfo();
    }


    /// <summary>
    /// Lets the user enter their first & last name, which is used to defined the PrimaryUser.
    /// </summary>
    private void CheckNameInfo()
    {
      checkNameTimer.Enabled = false;  // Shut off since we're only using this timer once

      // If the PrimaryUser property hasn't yet been set then display dialog box to obtain it.
      if (CFSysInfo.Data.MobileOptions.PrimaryUser == "")
      {
        frmGetNameInfo frmGetNames = new frmGetNameInfo();
        frmGetNames.ShowDialog();

        if (CFSysInfo.Data.MobileOptions.PrimaryUser != "")  // Save data immediately in case app crashes before shutdown
        {
          Tools.SaveData(CFSysInfo.Data.MobilePaths.App + "SysInfo.xml", CFSysInfo.Data, null, DataObjects.Constants.ExportFormat.XML);
          // I want to keep this next line separate from the generic SaveData routine so that the latter
          // can be more easily updated should changes be required to it in the future.
          FileEx.SetAttributes(CFSysInfo.Data.MobilePaths.App + "SysInfo.xml", FileAttributes.Hidden);
        }
      }

      // Before leaving, determine whether we need to display the initial startup menu
      DisplayStartMenu(CFSysInfo.Data.MobileOptions.StartWithMenu);

      // And display username, in case it was entered
      ShowUser();
    }



    /// <summary>
    /// Opens a file afresh (New) or Opens an existing file (Open)
    /// </summary>
    /// <param name="newPoll"></param>   // true - A new poll is being started       false - An existing file is being opened
    private void OpenFile(bool newPoll)
    {
      if (CFSysInfo.Data.MobileOptions.PrimaryUser == "")
      {
        string msg = "Sorry, but you cannot do any polling until you've entered the username for this device.";
        Tools.ShowMessage(msg, CFSysInfo.Data.MobileAdmin.AppName);
        CheckNameInfo();
        return;
      }

      Cursor.Current = Cursors.WaitCursor;
      string pollName = "";
      frmSelectFile selectFileForm;

      if (newPoll)   // Present list of Templates to user
      {
        selectFileForm = new frmSelectFile(CFSysInfo.Data.MobilePaths.Templates, SelectFileMode.RunPoll, true);
        if (!selectFileForm.OkayToShow)
          return;

        selectFileForm.ShowDialog();
        ToolsCF.SipShowIM(0);       // Ensure that SIP is collapsed

        // To Do: Could add something fancier than just hourglass cursor; for example, small "Loading Poll" modeless message
        Cursor.Current = Cursors.WaitCursor;

        // If the user selected a template with the same name as a previous poll then they may have chosen
        // to open up the existing one instead.  If so, then we need to change the setting of 'newPoll'.
        if (Tools.GetPath(selectFileForm.PollName) == CFSysInfo.Data.MobilePaths.Data)
          newPoll = false;
      }

      else           // Present list of existing polls to user
      {
        selectFileForm = new frmSelectFile(CFSysInfo.Data.MobilePaths.Data, SelectFileMode.RunPoll, false);
        if (!selectFileForm.OkayToShow)
          return;

        selectFileForm.ShowDialog();
        ToolsCF.SipShowIM(0);       // Ensure that SIP is collapsed
      }

      pollName = selectFileForm.PollName;
      if (pollName == "")
      {
        Cursor.Current = Cursors.Default;
        return;
      }

      // Reaching here, a file has been selected

      // But if it's a template, then we need to copy it into the Data folder and work with that copy.
      // Note: The method being called will also rename the file if one with the same name already exists.
      if (newPoll)
        pollName = ToolsCF.CopyTemplateToDataFolder(pollName);

      RunPoll(pollName);
    }



    /// <summary>
    /// Provides statistics to the user about previously gathered poll data.
    /// </summary>
    private void ReviewData()
    {
      Cursor.Current = Cursors.WaitCursor;
      string pollName = "";
      frmSelectFile selectFileForm;
      selectFileForm = new frmSelectFile(CFSysInfo.Data.MobilePaths.Data, SelectFileMode.ReviewData);

      if (!selectFileForm.OkayToShow)
        return;

      Cursor.Current = Cursors.Default;
      selectFileForm.ShowDialog();
      ToolsCF.SipShowIM(0);       // Ensure that SIP is collapsed

      Cursor.Current = Cursors.WaitCursor;
      pollName = selectFileForm.PollName;

      if (pollName == "")
      {
        Cursor.Current = Cursors.Default;
        return;
      }

      // Reaching here, a file has been selected, so display the statistics for it.
      ShowStats(pollName);
    }



    /// <summary>
    /// This is the method that controls the execution of every poll.
    /// </summary>
    /// <param name="pollName"></param>  // This is the full pathname
    private void RunPoll(string pollName)
    {
      bool isDirty = false;            // Set true once some data is captured
      bool keepPolling = true;         // Set false once user wants to close poll
      bool pollComplete = false;       // Set true once user has signalled that entire poll is complete (no more respondents for this file)
      bool firstTime = true;           // Set false once through the polling sequence loop
      bool backAvail = false;          // If true then enable Back button; if false then disable it
      bool abortRecord = false;        // If set true (within frmPoll) then the current record will not be saved
      int screenID = 1;                // This is a number that identifies a particular screen below
      int screenDir = 0;               // Set to either -1 or +1 depending on if Back or Next is pressed on a given screen
      int numQuestions;                // Number of questions in this poll
      int respondentID = -1;           // Keeps track of which respondent we're dealing with (Note: The -1 is just a dummy default value)


      // Open the poll we're going to use
      Poll pollModel = new Poll();
      Tools.OpenData(pollName, pollModel);

      // Preliminary check
      numQuestions = pollModel.Questions.Count;
      if (numQuestions == 0)
      {
        string msg = "This poll has no questions.  You should notify the poll's creator: " + pollModel.CreationInfo.CreatorName;
        Tools.ShowMessage(msg, "Poll Has No Questions", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      // Instantiate the arraylist that contains all the information about the current respondent
      ArrayList respInfoArray = Tools.GetRespondentInfoArray();

      // Ensure that assorted "PollingInfo" data is stored
      StoreBasicPollingInfo(pollModel);

      // Initialize a new Respondent object in pollModel.
      CreateNewRespondent(pollModel, out respondentID);

      this.Resize -= new System.EventHandler(this.frmMobile_Resize);    
      Cursor.Current = Cursors.Default;  // Now that everything is loaded, we can turn off the hourglass cursor
      screenDir = 1;                     // This must be defaulted to 1 in order to step forward through the screens

      do
      {
        // This is the start of the polling sequence for a respondent
        switch (screenID)
        {
          case 0:
            keepPolling = false;
            isDirty = false;
            break;

          case 1:
            backAvail = (firstTime) ? true : false;

            // See if the user should be given the opportunity to change the username
            if (firstTime && pollModel.PollsterPrivileges.ChangeUserName)
            {
              new frmChangeUserName(ref screenDir, ref backAvail);
              
              // Necessary, in case the username was changed
              if (pollModel.PollingInfo.PollsterName != CFSysInfo.Data.MobileOptions.PrimaryUser)
              {
                pollModel.PollingInfo.PollsterName = CFSysInfo.Data.MobileOptions.PrimaryUser;                 // "Master" pollster name
                pollModel.Respondents[respondentID].PollsterName = CFSysInfo.Data.MobileOptions.PrimaryUser;   // Pollster name for this record
                ShowUser();
              }
            }

            screenID = screenID + screenDir;
            break;

          case 2:
            backAvail = (firstTime) ? true : false;

            // See if 'BeforePoll' instructions should be displayed; these are private instructions for the pollster
            if (firstTime || pollModel.Instructions.RepeatBeforePoll)
              if (pollModel.Instructions.BeforePoll != "")
                ShowInstructions(pollModel.Instructions.BeforePoll, InstructType.BeforePoll, ref screenDir, ref backAvail);
            
            screenID = screenID + screenDir;
            break;

          case 3:
            backAvail = (firstTime) ? true : false;

            // See if there are 'BeginMessage' instructions to be displayed; these are public instructions for the person being polled
            if (pollModel.Instructions.BeginMessage != "")
              ShowInstructions(pollModel.Instructions.BeginMessage, InstructType.BeginMessage, ref screenDir, ref backAvail);

            screenID = screenID + screenDir;
            break;

          case 4:
            if (pollModel.CreationInfo.GetPersonalInfo)
              new frmRespondent(respInfoArray, ref isDirty, ref screenDir, ref backAvail);

            screenID = screenID + screenDir;
            break;

          case 5:
            if (numQuestions > 0)
            {
              frmPoll poll = new frmPoll(pollModel, respondentID, ref isDirty, ref screenDir, ref backAvail, out abortRecord);
            }

            screenID = screenID + screenDir;
            break;

          case 6:
            // See if 'AfterPoll' instructions should be displayed; these are private instructions for the pollster
            if (firstTime || pollModel.Instructions.RepeatAfterPoll)
              if (pollModel.Instructions.AfterPoll != "")
                ShowInstructions(pollModel.Instructions.AfterPoll, InstructType.AfterPoll, ref screenDir, ref backAvail);

            screenID = screenID + screenDir;
            break;

          case 7:
            // See if there are 'EndMessage' instructions to be displayed; these are public instructions for the person being polled
            if (pollModel.Instructions.EndMessage != "")
              ShowInstructions(pollModel.Instructions.EndMessage, InstructType.EndMessage, ref screenDir, ref backAvail);

            screenID = screenID + screenDir;
            break;

          case 8:
            // Now that the poll is done for the current respondent (or current occurrence), ask the user what they want to do
            int userChoice;
            frmRepeatPoll repeatPollForm = new frmRepeatPoll(out userChoice, pollModel.CreationInfo.GetPersonalInfo, abortRecord);

            switch (userChoice)
            {
              case 1:   // Edit Respondent's Personal Info
                new frmRespondent(respInfoArray, ref isDirty);
                break;

              case 2:   // Poll another person
                if (abortRecord)
                  pollModel.Respondents.RemoveAt(respondentID);
                else
                  SaveRespondentInfo(pollModel, respondentID, respInfoArray);

                ClearRespondentValues(respInfoArray);
                CreateNewRespondent(pollModel, out respondentID);
                firstTime = false;
                screenID = 1;
                break;

              case 3:   // Close, but keep available for future use
                pollModel.PollingInfo.FinishPolling = DateTime.Now;  // May not be finished but no harm in doing this
                
                if (abortRecord)
                  pollModel.Respondents.RemoveAt(respondentID);
                else
                  SaveRespondentInfo(pollModel, respondentID, respInfoArray);
                
                UpdateActiveSummaryRecord(pollName, pollModel);
                screenID++;    // Moving the ScreenID to 8 will force us out of the loop
                break;

              case 4:   // Close & Complete (move file to 'Completed' folder)
                pollModel.PollingInfo.FinishPolling = DateTime.Now;

                if (abortRecord)
                  pollModel.Respondents.RemoveAt(respondentID);
                else
                {
                  SaveRespondentInfo(pollModel, respondentID, respInfoArray);
                  // Most often this is already True but if the user hit 'Finish' and then
                  // chose to NOT abort the record, then the empty record must be saved
                  isDirty = true;     
                }

                // Poll now not active so remove record from CFSysInfo ActivePoll summaries
                _ActivePollSummary activeSummary = CFSysInfo.Data.Summaries.ActivePolls.Find_PollSummary(Tools.StripPathAndExtension(pollName));
                if (activeSummary != null)
                  CFSysInfo.Data.Summaries.ActivePolls.Remove(activeSummary);

                screenID++;    // Moving the ScreenID to 8 will force us out of the loop
                pollComplete = true;
                break;

              default:
                Debug.Fail("Unknown user choice: " + userChoice.ToString(), "frmMobile.RunPoll");
                break;
            }
            break;

          case 9:  // Note: If we reach here then code within will force us out of the loop
            // See if there are 'AfterAllPolls' instructions to be displayed; these are private instructions for the pollster
            if (pollModel.Instructions.AfterAllPolls != "")
            {
              backAvail = false;   // Don't allow user to step back from this screen
              ShowInstructions(pollModel.Instructions.AfterAllPolls, InstructType.AfterAllPolls, ref screenDir, ref backAvail);
            }

            keepPolling = false;
            break;

          default:
            Debug.Fail("Unknown ScreenID (" + screenID.ToString() + ")", "frmMobile.RunPoll");
            break;
        }
      } while (keepPolling);

      if (isDirty)
      {
        // We're now going to save the recorded data back to the file but where this file goes still needs to be determined.
        Tools.SaveData(pollName, pollModel, null, ExportFormat.XML);  // Save back to 'Data' folder

        if (pollComplete)  // If complete, then we need to move the file to 'Completed' folder
        {
          string completedPath = CFSysInfo.Data.MobilePaths.Completed;
          string destName = Tools.GetAvailFilename(completedPath, Tools.StripPath(pollName));
          File.Move(pollName, completedPath + destName);
        }
      }

      this.Resize += new System.EventHandler(this.frmMobile_Resize);

      // It's possible that the display mode of the mobile device has changed between Portrait and Landscape mode
      RepositionControls();

      // Test Code: Trying to eliminate periodic bug where the PP screen disappears altogether.  It's still running but somehow
      //            this main screen did not appear upon return from frmRepeatPoll.
      this.BringToFront();
    }



    private void ShowStats(string pollName)
    {
      // Temporarily turn off the Resizing event for this current form
      this.Resize -= new System.EventHandler(this.frmMobile_Resize);

      // Open the poll we're going to use
      Poll pollModel = new Poll();
      Tools.OpenData(pollName, pollModel);

      frmCriteria criteriaForm = new frmCriteria(pollName, pollModel);
      this.Resize += new System.EventHandler(this.frmMobile_Resize);

      // It's possible that the display mode of the mobile device has changed between Portrait and Landscape mode
      RepositionControls();
      Cursor.Current = Cursors.Default;
    }



    /// <summary>
    /// Instantiates & populates the Respondent object with the basic info.
    /// Then adds this object into the 'Respondents' collection.
    /// </summary>
    /// <param name="pollModel"></param>
    /// <param name="respondentID"></param>
    private void CreateNewRespondent(Poll pollModel, out int respondentID)
    {
      respondentID = pollModel.Respondents.Count;   // This allows us to continue on with previous polls
      _Respondent respondent = new _Respondent(pollModel.Respondents);

      // Populate all fields that don't require user input
      respondent.ID = respondentID;
      respondent.PollsterName = CFSysInfo.Data.MobileOptions.PrimaryUser;  // Note: This field may change midstream via the user
      respondent.Guid = PocketGuid.NewGuid().ToString();
      respondent.TimeCaptured = DateTime.Now;

      pollModel.Respondents.Add(respondent);

      // While we're here, let's initialize the Responses collection for this respondent
      int idx = 0;
      foreach (_Question question in pollModel.Questions)
      {
        _Response response = new _Response(respondent.Responses);
        response.ID = idx;                    // 0-based index
        response.QuestionID = question.ID;    // Note: question.ID values are unique but not necessarily consecutive!

        // Note: AnswerID and ExtraText are populated by frmPoll

        respondent.Responses.Add(response);   // Add to collection
        idx++;
      }
    }



    private void ClearRespondentValues(ArrayList respInfoArray)
    {
      for (int i = 0; i < respInfoArray.Count; i++)
      {
        (respInfoArray[i] as RespondentInfo).Value = "";
      }
    }



    /// <summary>
    /// Copies the data [temporarily] stored in 'respInfoArray' into the correct location in PollModel.Respondents
    /// </summary>
    /// <param name="pollModel"></param>
    /// <param name="respID"></param>
    /// <param name="respInfoArray"></param>
    private void SaveRespondentInfo(Poll pollModel, int respID, ArrayList respInfoArray)
    {
      _Respondent respondent = null;

      try
      {
        respondent = pollModel.Respondents[respID];   // RW: I believe this assumes that respID always equals the Index value???
      }
      catch
      {
        Debug.Fail("Invalid respondentID: " + respID.ToString(), "frmMobile.SaveRespondentInfo");
      }

      // Populate it with data that was stored in RespondentInfo
      foreach (RespondentInfo respInfo in respInfoArray)
      {
        string propName = respInfo.PropName;
        string newval = respInfo.Value;

        object[] indexer = new object[0];
        PropertyInfo propInfo = respondent.GetType().GetProperty(propName);
  
        if (propInfo != null)
        {
          if (propInfo.PropertyType.IsEnum)
          {
            if (newval != "")   // An error will result if we try to set a Enum type property to a blank value
            {
              object objVal = OpenNETCF.EnumEx.Parse(propInfo.PropertyType, newval.ToString(), true);
              propInfo.SetValue(respondent, objVal, indexer);
            }
          } 
          else
          {
            // This next 'SetValue' is receiving input directly from the user.  If the field is a non-string
            // then it's highly possible that the entered value is invalid for the field type we have available.
            // So we'll add a little error checking to ensure the app doesn't crash.  This can't happen with a
            // ComboBox since there are discrete values within.
            try
            {
              if (newval != "")
                propInfo.SetValue(respondent, Convert.ChangeType(newval, propInfo.PropertyType, null), indexer);
            }
  
            catch (Exception ex)
            {
              Debug.WriteLine("Illegal value entered: " + newval + "     Exception message: " + ex.Message);
            }
          }
        }
      }
    }


    private void ShowInstructions(string instructions, InstructType mode, ref int screenAdv, ref bool backAvail)
    {
      frmInstructions instructForm = new frmInstructions(instructions, mode, ref screenAdv, ref backAvail);
    }



    private void StoreBasicPollingInfo(Poll pollModel)
    {
      if (pollModel.PollingInfo.StartPolling.Date == DateTime.MinValue)
      {
        pollModel.PollingInfo.PollsterName = CFSysInfo.Data.MobileOptions.PrimaryUser;
        pollModel.PollingInfo.ProductID = CFSysInfo.Data.MobileAdmin.ProductID;
        pollModel.PollingInfo.VersionNumber = CFSysInfo.Data.MobileAdmin.VersionNumber;
        pollModel.PollingInfo.StartPolling = DateTime.Now;
        pollModel.PollingInfo.Guid = DataObjects.RegistryCF.GetGuid();
      }
    }


    private void UpdateActiveSummaryRecord(string pollName, Poll pollModel)
    {
      string basename = Tools.StripPathAndExtension(pollName);
      _ActivePollSummary activePollSummary = CFSysInfo.Data.Summaries.ActivePolls.Find_PollSummary(basename);

      if (activePollSummary == null)
      {
        activePollSummary = new _ActivePollSummary(basename, pollModel.CreationInfo.Revision, pollModel.CreationInfo.PollSummary, pollModel.PollsterPrivileges.ReviewData, pollModel.Questions.Count, pollModel.Respondents.Count);
        CFSysInfo.Data.Summaries.ActivePolls.Add(activePollSummary);
      }
      else
        activePollSummary.NumResponses = pollModel.Respondents.Count;
    }



    // This method freezes all of the features on the form.  It is generally called
    // if the software is being used illegally.
    private void Freeze()
    {
      panelStartMenu.Visible = false;
      menuFileNew.Enabled = false;
      menuFileOpen.Enabled = false;
      menuFileReview.Enabled = false;
      menuToolsStartMenu.Enabled = false;
      ShowLogo(0);

      panelExpired.Visible = true;
    }




    // This method is simply used to test various new code.  Once tested & working, the code is removed, leaving the method blank.
    // Note: Keep this method at the very bottom of the code, so it can always be easily accessed.
    private void TestCode()
    {

//      // The following construct is only required for the Emulator
//      Core.SystemInfo sysInfo = Core.GetSystemInfo();
//      if (sysInfo.ProcessorArchitecture.ToString().ToLower() == "intel")
//      {
//        //Debug Only: To speed up initial development setup (each time) we'll just copy some template files to the emulator.
//        string templatesFolder = CFSysInfo.Data.MobilePaths.Templates;
//
////        if (! File.Exists(tempFldr + "Sample7.pp"))
////          RefData.RefTools.ExtractResource(tempFldr, "Sample7.pp");
////
////        if (! File.Exists(tempFldr + "Sample8.pp"))
////          RefData.RefTools.ExtractResource(tempFldr, "Sample8.pp");
//
//        string dataFolder = CFSysInfo.Data.MobilePaths.Data;
//
////        if (! File.Exists(tempFldr + "ROFrothAudit.pp"))
////          RefData.RefTools.ExtractResource(tempFldr, "ROFrothAudit.pp");
//
//        if (! File.Exists(dataFolder + "Sample1.pp"))
//          RefData.RefTools.ExtractResource(dataFolder, "Sample1.pp");
//
//        if (! File.Exists(dataFolder + "Sample2.pp"))
//          RefData.RefTools.ExtractResource(dataFolder, "Sample2.pp");
//
//        if (! File.Exists(dataFolder + "Sample3.pp"))
//          RefData.RefTools.ExtractResource(dataFolder, "Sample3.pp");
//
//        if (! File.Exists(dataFolder + "Sample4.pp"))
//          RefData.RefTools.ExtractResource(dataFolder, "Sample4.pp");


//        if (! File.Exists(tempFldr + "MacleanAudit2.pp"))
//          RefData.RefTools.ExtractResource(tempFldr, "MacleanAudit2.pp");
//      }
//
//
//
//      // The following construct is only required for the Emulator
//      sysInfo = Core.GetSystemInfo();
//      if (sysInfo.ProcessorArchitecture.ToString().ToLower() == "intel")
//      {
//        //Debug Only: To speed up initial development setup (each time) we'll just copy some template files to the emulator.
//        string tempFldr = CFSysInfo.Data.MobilePaths.Templates;
//
//        if (! File.Exists(tempFldr + "MacleanAudit.pp"))
//          RefData.RefTools.ExtractResource(tempFldr, "MacleanAudit.pp");
//
//        if (! File.Exists(tempFldr + "MacleanAudit2.pp"))
//          RefData.RefTools.ExtractResource(tempFldr, "MacleanAudit2.pp");
//
//        if (! File.Exists(tempFldr + "Sample1.pp"))
//          RefData.RefTools.ExtractResource(tempFldr, "Sample1.pp");
//
//        if (! File.Exists(tempFldr + "Sample2.pp"))
//          RefData.RefTools.ExtractResource(tempFldr, "Sample2.pp");
//
////        if (! File.Exists(tempFldr + "Test1.pp"))
////          RefData.RefTools.ExtractResource(tempFldr, "Test1.pp");
////
////        tempFldr = CFSysInfo.Data.MobilePaths.Data;
////
////        if (! File.Exists(tempFldr + "Sample5.pp"))
////          RefData.RefTools.ExtractResource(tempFldr, "Sample5.pp");
////
////        if (! File.Exists(tempFldr + "Sample6.pp"))
////          RefData.RefTools.ExtractResource(tempFldr, "Sample6.pp");
////
////        if (! File.Exists(tempFldr + "Test2.pp"))
////          RefData.RefTools.ExtractResource(tempFldr, "Test2.pp");
//      }
//
//
//
//      //_PollSummary pollSummary = new _PollSummary();
////      _Summaries._PollSummary pollSummary = new DataObjects._Summaries._PollSummary(CFSysInfo.Data.Summaries.ActivePolls);
////      pollSummary.Filename = "abcd";
////      pollSummary.NumQuestions = 5;
////      pollSummary.NumResponses = 7;
////
////      CFSysInfo.Data.Summaries.ActivePolls.Add(pollSummary);
//
//
//
//
    }


	}
}
