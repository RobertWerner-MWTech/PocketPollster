using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using OpenNETCF.Desktop.Communication;

using DataTransfer;
using DataObjects;
using Multimedia;



namespace Desktop
{
  // Define Aliases
  using Tools = DataObjects.Tools;
  using ExportFormat = DataObjects.Constants.ExportFormat;
  using ViewMode = DataObjects.Constants.ViewMode;



	/// <summary>
	/// Summary description for frmMain.
	/// </summary>
  public class frmMain : System.Windows.Forms.Form
  {

    #region Variables
    private System.Windows.Forms.MainMenu menuMain;
    private System.Windows.Forms.MenuItem menuFile;
    private System.Windows.Forms.MenuItem menuFileNew;
    private System.Windows.Forms.MenuItem menuFileOpen;
    private System.Windows.Forms.MenuItem menuFileClose;
    private System.Windows.Forms.MenuItem menuFileExit;
    private System.Windows.Forms.MenuItem menuFileSeparator;
    private System.Windows.Forms.MenuItem menuWindow;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.MenuItem menuTools;
    private System.Windows.Forms.MenuItem menuHelp;
    private System.Windows.Forms.MenuItem menuHelpAbout;
    private System.Windows.Forms.MenuItem menuEdit;
    private System.Windows.Forms.MenuItem menuAdmin;
    private System.Windows.Forms.MenuItem menuAdminSpacer;
    private System.Windows.Forms.MenuItem menuFileSave;
    private System.Windows.Forms.MenuItem menuFileSaveAs;
    private System.Windows.Forms.StatusBar statusBar;
    private System.Windows.Forms.NotifyIcon notifyIcon;
    private System.Windows.Forms.Timer iconAnimationTimer;
    private System.Windows.Forms.ContextMenu notifyIconMenu;
    private System.Windows.Forms.MenuItem menuShowDesktop;
    private System.Windows.Forms.MenuItem menuFileSeparator2;
    private System.Windows.Forms.StatusBarPanel statusBarDataXferIcon;
    private System.Windows.Forms.StatusBarPanel statusBarDataXferStatus;
    private System.Windows.Forms.MenuItem menuFileCloseAll;
    private System.Windows.Forms.MenuItem menuWindowCascade;
    private System.Windows.Forms.MenuItem menuWindowTileHorizontal;
    private System.Windows.Forms.MenuItem menuWindowTileVertical;   // A special timer used to make the status bar message flash
    private System.Windows.Forms.StatusBarPanel statusBarMessage;
    private System.Windows.Forms.MenuItem menuFilePublish;
    private System.Windows.Forms.ToolBar toolBarMain;
    private System.Windows.Forms.ToolBarButton toolBarButtonOpen;
    private System.Windows.Forms.ImageList imageListToolBar;
    private System.Windows.Forms.ToolBarButton toolBarButtonClose;
    private System.Windows.Forms.ToolBarButton toolBarButtonSave;
    private System.Windows.Forms.ToolBarButton toolBarButtonNew;
    private System.Windows.Forms.ToolBarButton toolBarSeparator1;
    private System.Windows.Forms.ToolBarButton toolBarButtonPublish;
    private System.Windows.Forms.StatusBarPanel statusBarNewData;
    private System.Windows.Forms.Timer testCodeTimer;    
    private System.Windows.Forms.MenuItem menuToolsOptions;
    private System.Windows.Forms.MenuItem menuToolsUsers;
    private System.Windows.Forms.MenuItem menuToolsDevices;
    private System.Windows.Forms.MenuItem menuToolsSeparator;
    private System.Windows.Forms.Label labelExpired1;
    private System.Windows.Forms.Label labelExpired2;
    private System.Windows.Forms.LinkLabel linkExpiredWebsite;
    private System.Windows.Forms.Label labelScreenSize;
    private System.Windows.Forms.MenuItem menuToolsMonitor;
    private System.Windows.Forms.MenuItem menuFileExport;      // "Points to" the currently active poll (the MDIChildActivated code ensures that this is always up to date)
    private System.Windows.Forms.MenuItem menuFileUnpublish;   // Ditto
    private System.Windows.Forms.MenuItem menuItem1;
    private System.Windows.Forms.MenuItem menuToolsRespondents;                 // Code within ensures that this always refers to the current poll (or null if no polls open)

    private DataObjects.PanelGradient panelExpired;
    private DataObjects.PanelGradient panelScreenSize;

    private System.Timers.Timer statusMsgFlashTimer = new System.Timers.Timer(300);
    private System.Timers.Timer newDataMsgFlashTimer = new System.Timers.Timer(250);
    private System.Timers.Timer splashShutdownTimer = new System.Timers.Timer(1500);   // Duration that Splash Screen stays visible

    private string textStatusMsg;
    private int newDataMsgCumulFlashTime;
    private int flashMsgIteration = 0;    // Used to increase the msg display time and minimize the blank time with the flashing on/off of the msg
    private string newDataMsg = "New Data Available";
    private int currentIconNumber = 0;    // The number representing which icon (aka "frame") is being displayed in the Data Transfer animation

    private string ConnectedMsg = "Device connected";
    private string DisconnectedMsg = "No device connected";
    private string ConnectingMsg = "Connecting...";
    private frmNotify statusForm;                       // Provides a modular-level reference to the Data Transfer Notification form
    private RAPI rapi;                                  // This is THE solo Rapi object that is referenced by several Data Transfer modules
    private ControllerClass Controller;
    private ArrayList UtilityForms = new ArrayList();   // Keeps track of which special child forms are open (eg. Users, Devices, Options)

    #endregion


    public frmMain()
    {
      InitializeComponent();
      InitializeLinearGradients();

      CreateRapiObject();  // Debug: If CreateRapiObject returns 'false', what should we do?

      statusBarDataXferStatus.Text = DisconnectedMsg;

      DataXfer.StatusMessageEvent += new DataXfer.StatusMessageHandler(DisplayStatusMessage);
      statusMsgFlashTimer.AutoReset = true;
      statusMsgFlashTimer.Elapsed += new System.Timers.ElapsedEventHandler(FlashMessage);

      ControllerClass.SaveFileEvent += new ControllerClass.SaveFileHandler(SaveFile);
      DataXfer.IsFileOpenEvent += new DataXfer.IsFileOpenHandler(IsFileOpen);
      DataXfer.DisplayNewDataEvent += new DataXfer.DelayedImportHandler(DisplayNewDataMessage);

      // Activate contents of Edit Menu - Provides a full suite of Edit items via pre-built code
      EditMenuManager editMenuManager = new EditMenuManager();
      editMenuManager.ConnectMenus(this.menuEdit);

      // This next statement definitely belongs here and not in Form_Load.  For if the app was started by a
      // mobile device synching then the next construct will be entered and the window will be instantly minimized.
      this.WindowState = FormWindowState.Maximized;

      PrepareToEndSplashScreen();

      OnlyRunInDebug();     // Will only be called in Debug mode

      if (SysInfo.Data.Admin.DataTransferActive)
      {
        Tools.Beep(2000,100);
        Tools.Beep(4000,100);
    
        // If this app is started remotely by ActiveSync then we'll start it minimized & not shown
        // in the taskbar - essentially invisible.  But the user will be able to restore it by 
        // right-clicking on the animated icon in the System Tray.
        this.WindowState = FormWindowState.Minimized;
        this.ShowInTaskbar = false;

        InitiateDataTransfer();
      }


      // * Important Note! *
      // Nothing else can go here or else the app doesn't shut down properly when activated via a mobile device connecting.
    }



    // One of the MDI child windows has been activated.  This method updates several things accordingly.
    protected void MDIChildActivated(object sender, System.EventArgs e) 
    {
      if (this.ActiveMdiChild == null)  // No polls are open
      {
        Controller = null;
        menuFileUnpublish.Enabled = false;
      } 
      else 
      {
        SetControllerToActiveForm();
      }
    }


    /// <summary>
    /// Instantiates a new RAPI object.
    /// </summary>
    private bool CreateRapiObject()
    {
      bool retval = true;

      try
      {
        rapi = new RAPI();
        rapi.ActiveSync.Active += new ActiveHandler(ActiveSync_Active);
        rapi.ActiveSync.Disconnect += new DisconnectHandler(ActiveSync_Disconnect);
        rapi.ActiveSync.Listen += new ListenHandler(ActiveSync_Listen);
        rapi.ActiveSync.Answer += new AnswerHandler(ActiveSync_Answer);
      }

      catch
      {
        //PrepareToEndSplashScreen();
        string msg = SysInfo.Data.Admin.AppName + " can not communicate with your mobile device(s)";
        msg += "\nwithout ActiveSync running.  Please ensure it is properly installed.";
        msg += "\n\nYou can get a free copy here:\nhttp://www.microsoft.com/windowsmobile/downloads/activesync41.mspx\n ";
        Tools.ShowMessage(msg, "ActiveSync Not Present", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        retval = false;
      }

      return retval;
    }


    // Ensures that the LR sizing grip doesn't appear when the window is maximized, or else it acts strangely.
    private void frmMain_Resize(object sender, System.EventArgs e)
    {
      switch(this.WindowState)
      {
        case FormWindowState.Maximized:
          statusBar.SizingGrip = false;
          notifyIcon.Visible = false;     // Hide System Tray icon
          break;

        case FormWindowState.Minimized:
          if (iconAnimationTimer.Enabled)
            notifyIcon.Visible = true;    // Show System Tray icon
          break;

        default:   // 'Normal' state
          statusBar.SizingGrip = true; 
          notifyIcon.Visible = false;     // Hide System Tray icon
          break;
      }

      if (panelExpired.Visible)
        panelExpired.Location = new Point((this.ClientRectangle.Width - panelExpired.Width) / 2, (this.ClientRectangle.Height - panelExpired.Height) / 2);

      if (panelScreenSize.Visible)
        panelScreenSize.Location = new Point((this.ClientRectangle.Width - panelScreenSize.Width) / 2, (this.ClientRectangle.Height - panelScreenSize.Height) / 2);
    }


    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
        if (components != null) 
          components.Dispose();

      base.Dispose (disposing);
      components = null;  // RW: This is test code to avoid warning I was getting!
    }


    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
      this.menuMain = new System.Windows.Forms.MainMenu();
      this.menuFile = new System.Windows.Forms.MenuItem();
      this.menuFileNew = new System.Windows.Forms.MenuItem();
      this.menuFileOpen = new System.Windows.Forms.MenuItem();
      this.menuFileClose = new System.Windows.Forms.MenuItem();
      this.menuFileCloseAll = new System.Windows.Forms.MenuItem();
      this.menuFileSeparator = new System.Windows.Forms.MenuItem();
      this.menuFileSave = new System.Windows.Forms.MenuItem();
      this.menuFileSaveAs = new System.Windows.Forms.MenuItem();
      this.menuFilePublish = new System.Windows.Forms.MenuItem();
      this.menuFileUnpublish = new System.Windows.Forms.MenuItem();
      this.menuItem1 = new System.Windows.Forms.MenuItem();
      this.menuFileExport = new System.Windows.Forms.MenuItem();
      this.menuFileSeparator2 = new System.Windows.Forms.MenuItem();
      this.menuFileExit = new System.Windows.Forms.MenuItem();
      this.menuEdit = new System.Windows.Forms.MenuItem();
      this.menuTools = new System.Windows.Forms.MenuItem();
      this.menuToolsUsers = new System.Windows.Forms.MenuItem();
      this.menuToolsDevices = new System.Windows.Forms.MenuItem();
      this.menuToolsMonitor = new System.Windows.Forms.MenuItem();
      this.menuToolsSeparator = new System.Windows.Forms.MenuItem();
      this.menuToolsRespondents = new System.Windows.Forms.MenuItem();
      this.menuToolsOptions = new System.Windows.Forms.MenuItem();
      this.menuWindow = new System.Windows.Forms.MenuItem();
      this.menuWindowCascade = new System.Windows.Forms.MenuItem();
      this.menuWindowTileHorizontal = new System.Windows.Forms.MenuItem();
      this.menuWindowTileVertical = new System.Windows.Forms.MenuItem();
      this.menuHelp = new System.Windows.Forms.MenuItem();
      this.menuHelpAbout = new System.Windows.Forms.MenuItem();
      this.menuAdminSpacer = new System.Windows.Forms.MenuItem();
      this.menuAdmin = new System.Windows.Forms.MenuItem();
      this.statusBar = new System.Windows.Forms.StatusBar();
      this.statusBarNewData = new System.Windows.Forms.StatusBarPanel();
      this.statusBarMessage = new System.Windows.Forms.StatusBarPanel();
      this.statusBarDataXferStatus = new System.Windows.Forms.StatusBarPanel();
      this.statusBarDataXferIcon = new System.Windows.Forms.StatusBarPanel();
      this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.notifyIconMenu = new System.Windows.Forms.ContextMenu();
      this.menuShowDesktop = new System.Windows.Forms.MenuItem();
      this.iconAnimationTimer = new System.Windows.Forms.Timer(this.components);
      this.toolBarMain = new System.Windows.Forms.ToolBar();
      this.toolBarButtonNew = new System.Windows.Forms.ToolBarButton();
      this.toolBarButtonOpen = new System.Windows.Forms.ToolBarButton();
      this.toolBarButtonClose = new System.Windows.Forms.ToolBarButton();
      this.toolBarButtonSave = new System.Windows.Forms.ToolBarButton();
      this.toolBarSeparator1 = new System.Windows.Forms.ToolBarButton();
      this.toolBarButtonPublish = new System.Windows.Forms.ToolBarButton();
      this.imageListToolBar = new System.Windows.Forms.ImageList(this.components);
      this.testCodeTimer = new System.Windows.Forms.Timer(this.components);
      this.panelExpired = new DataObjects.PanelGradient();
      this.linkExpiredWebsite = new System.Windows.Forms.LinkLabel();
      this.labelExpired2 = new System.Windows.Forms.Label();
      this.labelExpired1 = new System.Windows.Forms.Label();
      this.panelScreenSize = new DataObjects.PanelGradient();
      this.labelScreenSize = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.statusBarNewData)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.statusBarMessage)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.statusBarDataXferStatus)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.statusBarDataXferIcon)).BeginInit();
      this.panelExpired.SuspendLayout();
      this.panelScreenSize.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuMain
      // 
      this.menuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this.menuFile,
                                                                             this.menuEdit,
                                                                             this.menuTools,
                                                                             this.menuWindow,
                                                                             this.menuHelp,
                                                                             this.menuAdminSpacer,
                                                                             this.menuAdmin});
      // 
      // menuFile
      // 
      this.menuFile.Index = 0;
      this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this.menuFileNew,
                                                                             this.menuFileOpen,
                                                                             this.menuFileClose,
                                                                             this.menuFileCloseAll,
                                                                             this.menuFileSeparator,
                                                                             this.menuFileSave,
                                                                             this.menuFileSaveAs,
                                                                             this.menuFilePublish,
                                                                             this.menuFileUnpublish,
                                                                             this.menuItem1,
                                                                             this.menuFileExport,
                                                                             this.menuFileSeparator2,
                                                                             this.menuFileExit});
      this.menuFile.Text = "&File";
      // 
      // menuFileNew
      // 
      this.menuFileNew.Index = 0;
      this.menuFileNew.Text = "&New";
      this.menuFileNew.Click += new System.EventHandler(this.menuFileNew_Click);
      // 
      // menuFileOpen
      // 
      this.menuFileOpen.Index = 1;
      this.menuFileOpen.RadioCheck = true;
      this.menuFileOpen.Text = "&Open";
      this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
      // 
      // menuFileClose
      // 
      this.menuFileClose.Index = 2;
      this.menuFileClose.RadioCheck = true;
      this.menuFileClose.Text = "&Close";
      this.menuFileClose.Click += new System.EventHandler(this.menuFileClose_Click);
      // 
      // menuFileCloseAll
      // 
      this.menuFileCloseAll.Index = 3;
      this.menuFileCloseAll.Text = "Close All";
      this.menuFileCloseAll.Click += new System.EventHandler(this.menuFileCloseAll_Click);
      // 
      // menuFileSeparator
      // 
      this.menuFileSeparator.Index = 4;
      this.menuFileSeparator.Text = "-";
      // 
      // menuFileSave
      // 
      this.menuFileSave.Index = 5;
      this.menuFileSave.Text = "&Save";
      this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
      // 
      // menuFileSaveAs
      // 
      this.menuFileSaveAs.Index = 6;
      this.menuFileSaveAs.Text = "Save &As";
      this.menuFileSaveAs.Click += new System.EventHandler(this.menuFileSaveAs_Click);
      // 
      // menuFilePublish
      // 
      this.menuFilePublish.Index = 7;
      this.menuFilePublish.Text = "&Publish";
      this.menuFilePublish.Click += new System.EventHandler(this.menuFilePublish_Click);
      // 
      // menuFileUnpublish
      // 
      this.menuFileUnpublish.Enabled = false;
      this.menuFileUnpublish.Index = 8;
      this.menuFileUnpublish.Text = "&Unpublish";
      this.menuFileUnpublish.Click += new System.EventHandler(this.menuFileUnpublish_Click);
      // 
      // menuItem1
      // 
      this.menuItem1.Index = 9;
      this.menuItem1.Text = "-";
      // 
      // menuFileExport
      // 
      this.menuFileExport.Index = 10;
      this.menuFileExport.Text = "&Export";
      this.menuFileExport.Click += new System.EventHandler(this.menuFileExport_Click);
      // 
      // menuFileSeparator2
      // 
      this.menuFileSeparator2.Index = 11;
      this.menuFileSeparator2.Text = "-";
      // 
      // menuFileExit
      // 
      this.menuFileExit.Index = 12;
      this.menuFileExit.Text = "E&xit";
      this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
      // 
      // menuEdit
      // 
      this.menuEdit.Index = 1;
      this.menuEdit.Text = "&Edit";
      // 
      // menuTools
      // 
      this.menuTools.Index = 2;
      this.menuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                              this.menuToolsUsers,
                                                                              this.menuToolsDevices,
                                                                              this.menuToolsMonitor,
                                                                              this.menuToolsSeparator,
                                                                              this.menuToolsRespondents,
                                                                              this.menuToolsOptions});
      this.menuTools.Text = "&Tools";
      // 
      // menuToolsUsers
      // 
      this.menuToolsUsers.Index = 0;
      this.menuToolsUsers.Text = "Users";
      this.menuToolsUsers.Click += new System.EventHandler(this.menuToolsUsers_Click);
      // 
      // menuToolsDevices
      // 
      this.menuToolsDevices.Index = 1;
      this.menuToolsDevices.Text = "Devices";
      this.menuToolsDevices.Click += new System.EventHandler(this.menuToolsDevices_Click);
      // 
      // menuToolsMonitor
      // 
      this.menuToolsMonitor.Index = 2;
      this.menuToolsMonitor.Text = "Monitor Data Xfer";
      this.menuToolsMonitor.Click += new System.EventHandler(this.menuToolsMonitor_Click);
      // 
      // menuToolsSeparator
      // 
      this.menuToolsSeparator.Index = 3;
      this.menuToolsSeparator.Text = "-";
      // 
      // menuToolsRespondents
      // 
      this.menuToolsRespondents.Index = 4;
      this.menuToolsRespondents.Text = "Respondents";
      this.menuToolsRespondents.Visible = false;
      this.menuToolsRespondents.Click += new System.EventHandler(this.menuToolsRespondents_Click);
      // 
      // menuToolsOptions
      // 
      this.menuToolsOptions.Index = 5;
      this.menuToolsOptions.Text = "&Options";
      this.menuToolsOptions.Click += new System.EventHandler(this.menuToolsOptions_Click);
      // 
      // menuWindow
      // 
      this.menuWindow.Index = 3;
      this.menuWindow.MdiList = true;
      this.menuWindow.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                               this.menuWindowCascade,
                                                                               this.menuWindowTileHorizontal,
                                                                               this.menuWindowTileVertical});
      this.menuWindow.MergeOrder = 10;
      this.menuWindow.Text = "&Window";
      // 
      // menuWindowCascade
      // 
      this.menuWindowCascade.Index = 0;
      this.menuWindowCascade.Text = "&Cascade";
      this.menuWindowCascade.Click += new System.EventHandler(this.WindowCascade_Click);
      // 
      // menuWindowTileHorizontal
      // 
      this.menuWindowTileHorizontal.Index = 1;
      this.menuWindowTileHorizontal.Text = "Tile &Horizontal";
      this.menuWindowTileHorizontal.Click += new System.EventHandler(this.WindowTileH_Click);
      // 
      // menuWindowTileVertical
      // 
      this.menuWindowTileVertical.Index = 2;
      this.menuWindowTileVertical.Text = "Tile &Vertical";
      this.menuWindowTileVertical.Click += new System.EventHandler(this.WindowTileV_Click);
      // 
      // menuHelp
      // 
      this.menuHelp.Index = 4;
      this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                             this.menuHelpAbout});
      this.menuHelp.Text = "Help";
      // 
      // menuHelpAbout
      // 
      this.menuHelpAbout.Index = 0;
      this.menuHelpAbout.Text = "About";
      this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
      // 
      // menuAdminSpacer
      // 
      this.menuAdminSpacer.Enabled = false;
      this.menuAdminSpacer.Index = 5;
      this.menuAdminSpacer.Text = "";
      this.menuAdminSpacer.Visible = false;
      // 
      // menuAdmin
      // 
      this.menuAdmin.Index = 6;
      this.menuAdmin.Text = "- Admin -";
      this.menuAdmin.Visible = false;
      // 
      // statusBar
      // 
      this.statusBar.Location = new System.Drawing.Point(0, 579);
      this.statusBar.Name = "statusBar";
      this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
                                                                                 this.statusBarNewData,
                                                                                 this.statusBarMessage,
                                                                                 this.statusBarDataXferStatus,
                                                                                 this.statusBarDataXferIcon});
      this.statusBar.ShowPanels = true;
      this.statusBar.Size = new System.Drawing.Size(912, 22);
      this.statusBar.TabIndex = 1;
      this.statusBar.Text = "statusBar1";
      this.statusBar.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.statusBar_PanelClick);
      // 
      // statusBarNewData
      // 
      this.statusBarNewData.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
      this.statusBarNewData.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
      this.statusBarNewData.MinWidth = 150;
      this.statusBarNewData.Width = 150;
      // 
      // statusBarMessage
      // 
      this.statusBarMessage.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
      this.statusBarMessage.Width = 636;
      // 
      // statusBarDataXferStatus
      // 
      this.statusBarDataXferStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
      this.statusBarDataXferStatus.MinWidth = 70;
      this.statusBarDataXferStatus.Width = 70;
      // 
      // statusBarDataXferIcon
      // 
      this.statusBarDataXferIcon.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
      this.statusBarDataXferIcon.MinWidth = 40;
      this.statusBarDataXferIcon.Width = 40;
      // 
      // notifyIcon
      // 
      this.notifyIcon.ContextMenu = this.notifyIconMenu;
      this.notifyIcon.Text = "Transferring Data...";
      // 
      // notifyIconMenu
      // 
      this.notifyIconMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                   this.menuShowDesktop});
      // 
      // menuShowDesktop
      // 
      this.menuShowDesktop.Index = 0;
      this.menuShowDesktop.Text = "Show PP Desktop";
      this.menuShowDesktop.Click += new System.EventHandler(this.menuShowDesktop_Click);
      // 
      // iconAnimationTimer
      // 
      this.iconAnimationTimer.Tick += new System.EventHandler(this.IconAnimationTimer_Tick);
      // 
      // toolBarMain
      // 
      this.toolBarMain.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
                                                                                   this.toolBarButtonNew,
                                                                                   this.toolBarButtonOpen,
                                                                                   this.toolBarButtonClose,
                                                                                   this.toolBarButtonSave,
                                                                                   this.toolBarSeparator1,
                                                                                   this.toolBarButtonPublish});
      this.toolBarMain.Divider = false;
      this.toolBarMain.DropDownArrows = true;
      this.toolBarMain.ImageList = this.imageListToolBar;
      this.toolBarMain.Location = new System.Drawing.Point(0, 0);
      this.toolBarMain.Name = "toolBarMain";
      this.toolBarMain.ShowToolTips = true;
      this.toolBarMain.Size = new System.Drawing.Size(912, 26);
      this.toolBarMain.TabIndex = 3;
      this.toolBarMain.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
      this.toolBarMain.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarMain_ButtonClick);
      // 
      // toolBarButtonNew
      // 
      this.toolBarButtonNew.ImageIndex = 0;
      this.toolBarButtonNew.Tag = "New";
      // 
      // toolBarButtonOpen
      // 
      this.toolBarButtonOpen.ImageIndex = 1;
      this.toolBarButtonOpen.Tag = "Open";
      // 
      // toolBarButtonClose
      // 
      this.toolBarButtonClose.ImageIndex = 2;
      this.toolBarButtonClose.Tag = "Close";
      // 
      // toolBarButtonSave
      // 
      this.toolBarButtonSave.ImageIndex = 3;
      this.toolBarButtonSave.Tag = "Save";
      // 
      // toolBarSeparator1
      // 
      this.toolBarSeparator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
      // 
      // toolBarButtonPublish
      // 
      this.toolBarButtonPublish.ImageIndex = 5;
      this.toolBarButtonPublish.Tag = "Publish";
      this.toolBarButtonPublish.Text = "Publish";
      // 
      // imageListToolBar
      // 
      this.imageListToolBar.ImageSize = new System.Drawing.Size(16, 16);
      this.imageListToolBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListToolBar.ImageStream")));
      this.imageListToolBar.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // testCodeTimer
      // 
      this.testCodeTimer.Tick += new System.EventHandler(this.testCodeTimer_Tick);
      //this.testCodeTimer.Start();
      // 
      // panelExpired
      // 
      this.panelExpired.Controls.Add(this.linkExpiredWebsite);
      this.panelExpired.Controls.Add(this.labelExpired2);
      this.panelExpired.Controls.Add(this.labelExpired1);
      this.panelExpired.GradientColorOne = System.Drawing.Color.SteelBlue;
      this.panelExpired.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.panelExpired.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelExpired.Location = new System.Drawing.Point(24, 152);
      this.panelExpired.Name = "panelExpired";
      this.panelExpired.Size = new System.Drawing.Size(424, 240);
      this.panelExpired.TabIndex = 21;
      this.panelExpired.Visible = false;
      // 
      // linkExpiredWebsite
      // 
      this.linkExpiredWebsite.ActiveLinkColor = System.Drawing.Color.Blue;
      this.linkExpiredWebsite.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.linkExpiredWebsite.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.linkExpiredWebsite.Location = new System.Drawing.Point(0, 192);
      this.linkExpiredWebsite.Name = "linkExpiredWebsite";
      this.linkExpiredWebsite.Size = new System.Drawing.Size(424, 48);
      this.linkExpiredWebsite.TabIndex = 4;
      this.linkExpiredWebsite.TabStop = true;
      this.linkExpiredWebsite.Text = "www.PocketPollster.com";
      this.linkExpiredWebsite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.linkExpiredWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkExpiredWebsite_LinkClicked);
      // 
      // labelExpired2
      // 
      this.labelExpired2.Dock = System.Windows.Forms.DockStyle.Top;
      this.labelExpired2.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelExpired2.ForeColor = System.Drawing.Color.Firebrick;
      this.labelExpired2.Location = new System.Drawing.Point(0, 88);
      this.labelExpired2.Name = "labelExpired2";
      this.labelExpired2.Size = new System.Drawing.Size(424, 88);
      this.labelExpired2.TabIndex = 3;
      this.labelExpired2.Text = "Please visit our website to obtain another copy.";
      this.labelExpired2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // labelExpired1
      // 
      this.labelExpired1.Dock = System.Windows.Forms.DockStyle.Top;
      this.labelExpired1.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelExpired1.ForeColor = System.Drawing.Color.Firebrick;
      this.labelExpired1.Location = new System.Drawing.Point(0, 0);
      this.labelExpired1.Name = "labelExpired1";
      this.labelExpired1.Size = new System.Drawing.Size(424, 88);
      this.labelExpired1.TabIndex = 1;
      this.labelExpired1.Text = "We\'re sorry, but this version of Pocket Pollster has expired";
      this.labelExpired1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // panelScreenSize
      // 
      this.panelScreenSize.Controls.Add(this.labelScreenSize);
      this.panelScreenSize.GradientColorOne = System.Drawing.Color.SteelBlue;
      this.panelScreenSize.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.panelScreenSize.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelScreenSize.Location = new System.Drawing.Point(464, 184);
      this.panelScreenSize.Name = "panelScreenSize";
      this.panelScreenSize.Size = new System.Drawing.Size(424, 192);
      this.panelScreenSize.TabIndex = 21;
      this.panelScreenSize.Visible = false;
      // 
      // labelScreenSize
      // 
      this.labelScreenSize.Dock = System.Windows.Forms.DockStyle.Top;
      this.labelScreenSize.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelScreenSize.ForeColor = System.Drawing.Color.Blue;
      this.labelScreenSize.Location = new System.Drawing.Point(0, 0);
      this.labelScreenSize.Name = "labelScreenSize";
      this.labelScreenSize.Size = new System.Drawing.Size(424, 184);
      this.labelScreenSize.TabIndex = 1;
      this.labelScreenSize.Text = "Pocket Pollster works best if the screen resolution is set to 1024x768 or larger";
      this.labelScreenSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // frmMain
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(912, 601);
      this.Controls.Add(this.panelExpired);
      this.Controls.Add(this.toolBarMain);
      this.Controls.Add(this.statusBar);
      this.Controls.Add(this.panelScreenSize);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.IsMdiContainer = true;
      this.Menu = this.menuMain;
      this.MinimumSize = new System.Drawing.Size(620, 500);
      this.Name = "frmMain";
      this.Text = "App Filename";
      this.Resize += new System.EventHandler(this.frmMain_Resize);
      this.MdiChildActivate += new System.EventHandler(this.MDIChildActivated);
      this.Load += new System.EventHandler(this.frmMain_Load);
      this.Closed += new System.EventHandler(this.frmMain_Closed);
      ((System.ComponentModel.ISupportInitialize)(this.statusBarNewData)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.statusBarMessage)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.statusBarDataXferStatus)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.statusBarDataXferIcon)).EndInit();
      this.panelExpired.ResumeLayout(false);
      this.panelScreenSize.ResumeLayout(false);
      this.ResumeLayout(false);
    }
    #endregion


    #region MenuAndToolBarEvents

    private void menuFileNew_Click(object sender, System.EventArgs e)
    {
      PreparePoll();
    }

    private void menuFileOpen_Click(object sender, System.EventArgs e)
    {
      OpenFile();
    }

    private void menuFileClose_Click(object sender, System.EventArgs e)
    {
      ClosePoll();
    }

    private void menuFileCloseAll_Click(object sender, System.EventArgs e)
    {
      CloseAllPolls();
    }

    private void menuFileSave_Click(object sender, System.EventArgs e)
    {
      SaveFile();
    }

    private void menuFileSaveAs_Click(object sender, System.EventArgs e)
    {
      SaveFileAs();
    } 

    private void menuFilePublish_Click(object sender, System.EventArgs e)
    {
      PublishPoll();
    }

    private void menuFileUnpublish_Click(object sender, System.EventArgs e)
    {
      UnpublishPoll();
    }

    private void menuFileExport_Click(object sender, System.EventArgs e)
    {
      ExportPoll();
    }

    private void menuFileExit_Click(object sender, System.EventArgs e)
    {
      ExitApp();
    }

    private void menuToolsUsers_Click(object sender, System.EventArgs e)
    {
      ShowUserManager();
    }

    private void menuToolsDevices_Click(object sender, System.EventArgs e)
    {
      ShowDevices();
    }

    private void menuToolsMonitor_Click(object sender, System.EventArgs e)
    {
      frmMonitor monitorForm = new frmMonitor();
      monitorForm.Show();
    }

    private void menuToolsRespondents_Click(object sender, System.EventArgs e)
    {
      ShowRespondents();
    }

    private void menuToolsOptions_Click(object sender, System.EventArgs e)
    {
      ShowOptions();
    }

    //Window->Cascade Menu item handler
    private void WindowCascade_Click(object sender, System.EventArgs e) 
    {
      this.LayoutMdi(MdiLayout.Cascade);
    }

    //Window->Tile Horizontally Menu item handler
    private void WindowTileH_Click(object sender, System.EventArgs e) 
    {
      this.LayoutMdi(MdiLayout.TileHorizontal);
    }

    //Window->Tile Vertically Menu item handler
    private void WindowTileV_Click(object sender, System.EventArgs e) 
    {
      this.LayoutMdi(MdiLayout.TileVertical);
    }

    private void menuHelpAbout_Click(object sender, System.EventArgs e)
    {
      frmAbout aboutForm = new frmAbout(true);
      aboutForm.ShowDialog();
    }

    // This is a menu item from the NotifyIcon menu in the system tray
    private void menuShowDesktop_Click(object sender, System.EventArgs e)
    {
      this.ShowInTaskbar = true;
      this.Visible = true;
      this.WindowState = FormWindowState.Normal;
      this.Refresh();
    }


    private void toolBarMain_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
    {
      switch (e.Button.Tag.ToString().ToLower())
      {
        case "new":
          PreparePoll();
          break;

        case "open":
          OpenFile();
          break;

        case "close":
          ClosePoll();
          break;

        case "save":
          SaveFile();
          break;

        case "publish":
          PublishPoll();
          break;

        default:
          Debug.Fail("Unknown toolbar button pressed!  Tag: " + e.Button.Tag.ToString(), "frmMain.toolBarMain_ButtonClick");
          break;
      }

      Debug.WriteLine("");
    }

    #endregion


    #region PreparePollObject

    /// <summary>
    /// Used to prepare a brand new poll
    /// </summary>
    public void PreparePoll()
    {
      PreparePoll("");
    }
    
    /// <summary>
    /// Used to open an existing poll, or to create a new poll if called with a null string (ie. "") parameter.
    /// </summary>
    /// <param name="pollname">If not a null string then specifies the full pathname</param>
    /// <returns>true - poll was opened successfully</returns>
    public bool PreparePoll(string pollname)
    {
      // Need to ensure that we don't open the same poll twice
      if (pollname != "" && IsPollOpen(pollname))
      {
        Tools.ShowMessage("'" + pollname + "' is already open!", "File Already Open", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }

      // Create new instance of Controller, which in turn will create a Poll model and a frmPoll.
      // It will also register a reference to this Controller in PollManager's ArrayList of Polls.
      Controller = new ControllerClass(pollname, (pollname == ""));
      Controller.pollForm.MdiParent = this;
      Controller.pollForm.Show();

      if (PollManager.Current.Polls.Count == 1)
        Controller.pollForm.WindowState = FormWindowState.Maximized;

      return true;
    }

    #endregion


    #region OpenSaveClosePolls

    /// <summary>
    /// Used to open 1 or more existing polls.
    /// </summary>
    public void OpenFile()
    {
      // Display an OpenFileDialog so the user can select a PP file
      OpenFileDialog fileDialog = new OpenFileDialog();
      fileDialog.Filter = SysInfo.Data.Admin.AppName + " Files (" + Tools.GetAppFilter() + ")|" + Tools.GetAppFilter();
      fileDialog.InitialDirectory = SysInfo.Data.Paths.CurrentData;
      fileDialog.ShowHelp = true;
      fileDialog.Title = "Open " + SysInfo.Data.Admin.AppName + " File(s)";
      fileDialog.Multiselect = true;

      // Show the Dialog.  If the user clicks OK in the dialog and 1 or more .PP files were selected then open it/them.
      if (fileDialog.ShowDialog() == DialogResult.OK)
      {
        Cursor.Current = Cursors.WaitCursor;

        foreach (string fileName in fileDialog.FileNames)
        {
          string filename = Tools.StripPathAndExtension(fileName);

          if (PreparePoll(fileName))  // Specify filename with full path so method can find it
            if (Directory.GetCurrentDirectory() != fileDialog.InitialDirectory)
              SysInfo.Data.Paths.CurrentData = Tools.EnsureFullPath(Directory.GetCurrentDirectory());
        }

        if (PollManager.Current.Polls.Count == 1)
        {
          ControllerClass controller = (ControllerClass) PollManager.Current.Polls[0];
          controller.pollForm.WindowState = FormWindowState.Maximized;
        }
        else if (fileDialog.FileNames.Length > 1)
        {
          this.LayoutMdi(MdiLayout.Cascade);
        }

        Cursor.Current = Cursors.Default;
      }
    }

    
    
    /// <summary>
    /// Initiates the process to save a poll.
    /// </summary>
    /// <param name="exportAlso">true - flag to pass along that it's okay to export as well as save   false - don't export at any point in process</param>
    /// <returns>true - file was saved    false - file was not saved</returns>
    public bool SaveFile(bool exportAlso)
    {
      this.Focus();   // Forces any pending events, like textbox entry, to occur before saving is done

      if (Controller.newPoll)
        return SaveFileAs(exportAlso);

      SaveData(Controller.pollNameFull, null, exportAlso);
      return true;
    }

    public bool SaveFile()
    {
      return SaveFile(true);
    }


    
    /// <summary>
    /// Used to save a poll, explicitly allowing the user to specify a name.
    /// </summary>
    /// <param name="initDir"></param>
    /// <param name="exportAlso"></param>
    /// <returns>true - file was saved    false - file was not saved</returns>
    public bool SaveFileAs(string initDir, bool exportAlso)
    {
      this.Focus();   // Forces any pending events, like textbox entry, to occur before saving is done

      // Display an OpenFileDialog so the user can select a PP file
      SaveFileDialog fileDialog = new SaveFileDialog();
      fileDialog.DefaultExt = Tools.GetAppFilter().Substring(1).ToLower();  // ie. "pp"
      fileDialog.FileName = Controller.pollName;
      fileDialog.Filter = SysInfo.Data.Admin.AppName + " Files (" + Tools.GetAppFilter() + ")|" + Tools.GetAppFilter();
      fileDialog.InitialDirectory = initDir;
      fileDialog.ShowHelp = true;
      fileDialog.Title = "Save " + SysInfo.Data.Admin.AppName + " File";

      string newFilename;
      bool fileNameIsOkay = false;

      do
      {
        // Show the Dialog.  If the user clicks OK in the dialog and a .PP file was selected then save object to this filename.
        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
          //newFilename = Tools.ProperCase(Tools.StripPathAndExtension(fileDialog.FileName));
          newFilename = Tools.StripPathAndExtension(fileDialog.FileName);
        
          if (newFilename.IndexOf("~") > -1)
            Tools.ShowMessage("Sorry, but you can't use a tilde character ( '~' ) in your filename!", "Illegal Character Entered");
          
          else
          {
            // Also need to ensure that we're not saving to a file that is currently open
            if (newFilename == Controller.pollName)
              fileNameIsOkay = true;   // Saving to existing filename so okay
            else if (IsPollOpen(newFilename))
              Tools.ShowMessage("You can't use this name because this file is currently open.", "Filename Belongs To Currently Open File");
            else
              fileNameIsOkay = true;
          }
        }
        else
          return false;   // User pressed Cancel so leave quietly

      } while (!fileNameIsOkay);

      newFilename = Directory.GetCurrentDirectory() + "\\" + newFilename + Tools.GetAppExt();
      SaveData(newFilename, null, exportAlso);

      if (Directory.GetCurrentDirectory() != fileDialog.InitialDirectory)
        SysInfo.Data.Paths.CurrentData = Tools.EnsureFullPath(Directory.GetCurrentDirectory());

      if (Controller.pollNameFull != newFilename)
        Controller.pollNameFull = newFilename;

      newFilename = Tools.StripPathAndExtension(newFilename);
      if (newFilename != Controller.pollName)
      {
        Controller.pollName = newFilename;           // Store filename for future saves
        Controller.pollForm.Text = newFilename;      // Change filename in titlebar of form
      }

      if (Controller.newPoll)
        Controller.newPoll = false;

      return true;
    }

    public bool SaveFileAs(bool exportAlso)
    {
      return SaveFileAs(SysInfo.Data.Paths.CurrentData, exportAlso);
    }

    public bool SaveFileAs()
    {
      return SaveFileAs(SysInfo.Data.Paths.CurrentData, true);
    }



    /// <summary>
    /// This intermediate method has to be used so that we can set some properties before saving the Poll object model.
    /// </summary>
    /// <param name="destFilename"></param>
    /// <param name="propPath"></param>
    /// <param name="exportAlso">true - export to MDB is allowed   false - avoid checking whether auto-export is enabled</param>
    private void SaveData (string destFilename, string propPath, bool exportAlso)
    {
      Cursor.Current = Cursors.WaitCursor;

      Poll poll = Controller.pollModel;
      poll.CreationInfo.LastEditedBy = Tools.UserNameFormat(SysInfo.Data.Options.PrimaryUser);
      poll.CreationInfo.LastEditDate = DateTime.Now;

      if (Controller.IsDirty)   // Only reset the GUID if the structure has changed
        poll.CreationInfo.LastEditGuid = System.Guid.NewGuid().ToString();

      if (exportAlso)
      {
        // Must first save to the MDB because if it is compacted then it writes a value to CreationInfo.LastCompact
        // and thus the PP file must be saved second.
        Tools.ExportIfRequired(poll);
        DisplayStatusMessage("Data was successfully exported to: " + Tools.StripPath(destFilename), true);
        SetMsgTimeLimit(5);
      }

      // Now save in native format to .PP file
      Tools.SaveData(destFilename, poll, propPath, ExportFormat.XML);
      Controller.IsDirty = false;  // Now that we've saved the file, this property must be set to false

      Cursor.Current = Cursors.Default;
    }

    private void SaveData (string destFilename, string propPath)
    {
      SaveData(destFilename, propPath, true);
    }



    /// <summary>
    /// Checks whether the specified poll is currently open (ie. displayed in one of the MDI windows).
    /// </summary>
    /// <param name="pollName"></param>
    /// <returns></returns>
    private bool IsPollOpen(string pollName)
    {
      foreach (ControllerClass controller in PollManager.Current.Polls)
      {
        if (pollName.ToLower() == Tools.StripPathAndExtension(controller.pollNameFull).ToLower())
          return true;
      }

      return false;
    }


    private void ClosePoll()
    {
      if (this.ActiveMdiChild != null)
        this.ActiveMdiChild.Close();
    }

    private void CloseAllPolls()
    {
      do
      {
        ClosePoll();
      } while (this.ActiveMdiChild != null);
    }

    #endregion


    #region PublishPoll

    /// <summary>
    /// Copies the current poll to the 'Templates' folder, ensuring that the Respondents, Responses, 
    /// and PollingInfo sections are cleaned of all captured data, if there is any present.
    /// 
    /// Note: If the file being edited is in the 'Data' folder then besides creating the new version in 'Templates'
    ///       we must also update the original in 'Data'.  If we don't, then the "LastEditGuid" properties will be
    ///       out of sync and when a populated poll comes back from a mobile user, the system will think that it
    ///       and the master copy in 'Data' are actually different versions (with the same filename), when in fact
    ///       they are exactly the same version.
    /// </summary>
    public void PublishPoll()
    {
      bool needToSaveMasterCopy = false;
      bool needToSetIsDirtyFlag = false;    // Since SaveData clears 'IsDirty', we need to set it late

      this.Focus();   // Forces any pending events, like textbox entry, to occur before saving is done

      // If the file we're editing is in the 'Data' folder then we must also save the master copy.  
      // This ensures that the version in 'Data' and the version in 'Templates' will remain in sync.
      // Note: If this is a new poll that hasn't yet been saved then this is a moot point.
      if (Controller.IsDirty && Tools.EnsureFullPath(Tools.GetPath(Controller.pollNameFull)) == SysInfo.Data.Paths.Data)
      {
        string msg = "In order to make this poll available as a data collection template\n" +
          "the master copy must be saved so that the two remain in sync.\n\n" +
          "Is it okay to save all changes made to the master copy?";
        DialogResult retval = Tools.ShowMessage(msg, "Save Master Copy of Poll?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        
        if (retval == DialogResult.Yes)
          needToSaveMasterCopy = true;   // Sets a flag that will later instruct code to save master copy into the Data folder
        else
          return;
      }

      Cursor.Current = Cursors.WaitCursor;
      string fullFilename;
      if (Controller.newPoll)
      {
        Controller.pollModel.CreationInfo.Revision = 1;     // This is the first template of this type to be published
        SaveFileAs(SysInfo.Data.Paths.Templates, false);    // Saves a copy of the poll into the Templates folder
        fullFilename = SysInfo.Data.Paths.Templates + Controller.pollName + Tools.GetAppExt();
      }
      else
      {
        // Note: For now, at least, we're going to overwrite w/o asking, just like Blogger does.
        fullFilename = SysInfo.Data.Paths.Templates + Controller.pollName + Tools.GetAppExt();

        // If this template is being saved afresh, then we'll set the Revision number to 1
        if (!File.Exists(fullFilename))
        {
          Controller.pollModel.CreationInfo.Revision = 1;   // This is the first template of this type to be published

//          // If this poll has a master copy then set IsDirty flag so that it will be saved later with the same revision number too
//          if (Tools.EnsureFullPath(Tools.GetPath(Controller.pollNameFull)) == SysInfo.Data.Paths.Data)
//            needToSetIsDirtyFlag = true;

          // If IsDirty = false and the master copy resides in the 'Data' folder then save the changes made below to the master copy too
          if (!Controller.IsDirty && Tools.EnsureFullPath(Tools.GetPath(Controller.pollNameFull)) == SysInfo.Data.Paths.Data)
            needToSaveMasterCopy = true;
        }
        
        else   // Overwriting, so check whether the LastEditGuid values are different or changes were made that haven't yet been saved
        {
          string prevGuid = Tools.GetLastEditGuid(fullFilename);
          if (prevGuid != Controller.pollModel.CreationInfo.LastEditGuid || Controller.IsDirty)
          {
            Controller.pollModel.CreationInfo.Revision++;

            // If this poll has a master copy then set IsDirty flag so that it will be saved later with the same revision number too
            if (Tools.EnsureFullPath(Tools.GetPath(Controller.pollNameFull)) == SysInfo.Data.Paths.Data)
            {
              needToSetIsDirtyFlag = true;

              if (!Controller.IsDirty)
                needToSaveMasterCopy = true;
            }
          }
        }
        
        // We need to save a duplicate copy of the poll so that we can open it up in a separate object model.
        SaveData(fullFilename, null, false);                // Saves a copy of the poll into the Templates folder
      }

      // Now that we have a saved copy of this file we can open up the copy and "clean" the sections listed in the summary above
      Poll templatesPoll = new Poll();
      Tools.OpenData(fullFilename, templatesPoll);   // Open up the just saved Templates copy

      // Clear the sections of the poll into which captured data is placed, thus leaving us with a template version of the file
      templatesPoll.Respondents.Clear();
      templatesPoll.PollingInfo.Clear();

      // Now we can resave the blank template back to disk
      Tools.SaveData(fullFilename, templatesPoll, null, ExportFormat.XML);   // And resaves the Templates copy

      if (needToSaveMasterCopy)
        SaveFile(false);   // Saves master copy in Data folder

      else if (needToSetIsDirtyFlag)
        Controller.IsDirty = true;

      Controller.PollPublished = true;

      // Finally, we need to Update (or Add) record to Summaries.Templates collection in SysInfo
      if (SysInfo.Data.Summaries.Templates.ContainsFilename(Controller.pollName))
      {      // Update existing record
        _TemplateSummary summary = SysInfo.Data.Summaries.Templates.Find_Template(Controller.pollName);
        summary.Revision = Controller.pollModel.CreationInfo.Revision;
        summary.PollSummary = Controller.pollModel.CreationInfo.PollSummary;
        summary.NumQuestions = Controller.pollModel.Questions.Count;
        // Note: PollGuid doesn't need to be updated since it never changes
        summary.LastEditGuid = Controller.pollModel.CreationInfo.LastEditGuid;
      }

      else   // Add new record
      {
        //_TemplateSummary summary = new _TemplateSummary(SysInfo.Data.Summaries.Templates);
        _TemplateSummary summary = new _TemplateSummary();
        summary.Filename = Controller.pollName;        
        summary.Revision = Controller.pollModel.CreationInfo.Revision;
        summary.PollSummary = Controller.pollModel.CreationInfo.PollSummary;
        summary.NumQuestions = Controller.pollModel.Questions.Count;
        summary.PollGuid = Controller.pollModel.CreationInfo.PollGuid;
        summary.LastEditGuid = Controller.pollModel.CreationInfo.LastEditGuid;
        SysInfo.Data.Summaries.Templates.Add(summary);
      }

      Cursor.Current = Cursors.Default;
      menuFileUnpublish.Enabled = true;
      DisplayStatusMessage(Controller.pollName + " was successfully published!", true);
      SetMsgTimeLimit(5);
    }

    #endregion


    #region UnpublishPoll

    // Deletes the template associated with the current poll.
    public void UnpublishPoll()
    {
      string msg = @"Are you sure you want to delete Data\Templates\" + Controller.pollName + Tools.GetAppExt() + " ?";
      if (Tools.ShowMessage(msg, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        string fullPath = SysInfo.Data.Paths.Templates + Controller.pollName + Tools.GetAppExt();
        if (File.Exists(fullPath))   // Extra check, but good practice
          File.Delete(fullPath);

        DisplayStatusMessage(Controller.pollName + " template was deleted", true);
        SetMsgTimeLimit(5);

        menuFileUnpublish.Enabled = false;
      }
    }

    #endregion


    #region ExportPoll

    public void ExportPoll()
    {
      // This first check isn't really necessary but it might save the user the problem of accidentally
      // not having the PP and MDB filenames match, when he actually wanted them to.
      if (Controller.newPoll)
        Tools.ShowMessage("Please first save this poll before trying to export it.", "Save Required First");
      
      else
      {
        this.Focus();   // Forces any pending events, like textbox entry, to occur before saving is done
        bool confirm = false;
        string filename = Tools.SelectExportFilename(Tools.StripExtension(Controller.pollNameFull) + ".mdb", out confirm);
        if (confirm)
        {
          this.Refresh();
          Cursor.Current = Cursors.WaitCursor;
          Tools.SaveData(filename, Controller.pollModel, "", ExportFormat.MDB);
          Cursor.Current = Cursors.Default;
          DisplayStatusMessage("Data was successfully exported to: " + Tools.StripPath(filename), true);
          SetMsgTimeLimit(5);
        }
      }
    }

    #endregion


    /// <summary>
    /// Ensures that the module level variable 'Controller' is set to the currently active form.
    /// </summary>
    private void SetControllerToActiveForm()
    {
      frmPoll activeForm = this.ActiveMdiChild as frmPoll;   // Note: This is a more reliable approach than simply grabbing 'ActiveMdiChild.Text'
      string pollName = activeForm.PollName;                 //       because this title bar text may change in the future & thus break this method.

      // Now that we know the name of the active form, let's find it in the PollManager and then set the 'Controller' variable accordingly
      foreach (ControllerClass controller in PollManager.Current.Polls)
      {
        if (pollName == controller.pollName)
        {
          Controller = controller;

          if (File.Exists(SysInfo.Data.Paths.Templates + controller.pollName + Tools.GetAppExt()))
            menuFileUnpublish.Enabled = true;
          else
            menuFileUnpublish.Enabled = false;

          break;
        }
      }
    }


    #region BackgroundFill

    /// <summary>
    /// If you'd like to have linear gradients for the background of your MDI application then do the following:
    ///   1. Copy the following 3 methods into your parent form.
    ///   2. Call 'InitializeLinearGradients()' from your parent form's constructor.
    /// </summary>
    public void InitializeLinearGradients()
    {
      foreach (Control c in this.Controls)
      {
        if (c.GetType().Name == "MdiClient")
        {
          c.Paint += new System.Windows.Forms.PaintEventHandler(PaintClient);
          c.SizeChanged += new System.EventHandler(SizeClient);
        }
      }      
    }

    protected void SizeClient(Object sender, EventArgs e)
    {
      sender = sender as MdiClient;
      this.Invalidate();
    }
    
    protected void PaintClient(Object sender, PaintEventArgs e)
    {
      MdiClient mc = sender as MdiClient;
      e.Graphics.Clip = new Region(mc.ClientRectangle);

      LinearGradientBrush lgb = new LinearGradientBrush(mc.ClientRectangle, Color.LightBlue, Color.Navy, 90F, false);
      //LinearGradientBrush lgb = new LinearGradientBrush(mc.ClientRectangle, Color.FromArgb(255,255,255), Color.FromArgb(65,65,255), 90F, false);

      e.Graphics.FillRectangle(lgb, mc.ClientRectangle);
      lgb.Dispose();
      e.Dispose();
    }

    #endregion


    [Conditional("DEBUG")]
    public void OnlyRunInDebug()
    {
      // Temp Debug Only
      //this.testCodeTimer.Enabled = true;

      //Tools.ZipTest();

      //      menuAdmin.Visible = true;          // Turn on a special menu that contains special Admin functions
      //      menuAdminSpacer.Visible = true;
    }


    public void AnimateDataTransferIcon(bool Show)
    {
      if (Show)
      {
        iconAnimationTimer.Interval = 75;
        iconAnimationTimer.Enabled = true;
        notifyIcon.Visible = true;
      }
      else
      {
        iconAnimationTimer.Enabled = false;
        notifyIcon.Visible = false;
        statusBarDataXferIcon.Icon = null;
      }
    }


    // Animates an icon that either resides in the status bar of frmMain or in the System Tray.
    private void IconAnimationTimer_Tick(object sender, EventArgs e)
    {
      // This test is necessary for the regular situation where the Data Transfer thread ends
      // of its own accord.  This will stop the animation of the icon, though the "Device Connected"
      // message will still be displayed (as it should be) until ActiveSync is disconnected.
      if (!rapi.Connected)
        AnimateDataTransferIcon(false);
      else
      {
        currentIconNumber++;
        if (this.WindowState == FormWindowState.Minimized)
          notifyIcon.Icon = Multimedia.Images.GetIcon("Anim", ref currentIconNumber);
        else
          statusBarDataXferIcon.Icon = Multimedia.Images.GetIcon("Anim", ref currentIconNumber);
      }
    }


    #region ActiveSync Events
    private void ActiveSync_Active()
    {
      Hourglass(false);
      this.statusBarDataXferStatus.Text = ConnectedMsg;
      SysInfo.Data.Admin.DataTransferActive = true;

      InitiateDataTransfer();
    }


    private void ActiveSync_Disconnect()
    {
      this.statusBarDataXferStatus.Text = DisconnectedMsg;
      DisplayStatusMessage("", false);   // The prev msg was probably saying to disconnect device.  So stop showing it once device is removed.
      AnimateDataTransferIcon(false);

      // Under normal circumstances, data transfer should end on its own, after everything is done.
      // But it's possible that the ActiveSync connection may be prematurely disconnected.  If this
      // is the case then we must take steps to inform the thread that it needs to abort.
      if (SysInfo.Data.Admin.DataTransferActive)
        AbortDataTransfer();

      // Here we need to exit PP Desktop if it was started by ActiveSync AND never opened up
      if (this.ShowInTaskbar == false)
      {
        ExitApp();
      }
    }


    private void ActiveSync_Listen()
    {
      this.statusBarDataXferStatus.Text =  DisconnectedMsg;
    }


    private void ActiveSync_Answer()
    {
      Hourglass(true);
      this.statusBarDataXferStatus.Text = ConnectingMsg;
    }
    #endregion


    public static void Hourglass(bool Show)
    {
      if (Show)
        Cursor.Current = Cursors.WaitCursor;
      else
        Cursor.Current = Cursors.Default; 

      return;
    }


    private bool Connect()
    {
      try
      {
        if (rapi.Connected)
          return true;
 
        if (!rapi.DevicePresent) 
          return false;

        rapi.Connect(true, 60);
       
        //this.statusBarDataXferStatus.Text = ConnectedMsg;
      }

      catch (Exception) 
      { 
        throw; 
      }
      
      return true;
    }


    //    // Debug: I don't think this is used!
    //    private void Disconnect()
    //    {
    //      try
    //      {
    //        if (!rapi.Connected)
    //         return;
    //				
    //        if (!rapi.DevicePresent) 
    //          return;
    //				
    //        rapi.Disconnect();
    //      }
    //      catch (Exception) 
    //      { 
    //        throw; 
    //      }
    //    }



    /// <summary>
    /// Creates a new thread and initiates data transfer on it.
    /// </summary>
    private void InitiateDataTransfer()
    {
      if (this.Connect())
      {
        // First instantiate a notification form so that we can display Data Transfer stats to the user.
        if (this.WindowState == FormWindowState.Maximized)
          statusForm = new frmNotify(statusBar.Height + 2, 2);
        else
          statusForm = new frmNotify();
        
        statusForm.Show();        // Display Notification Window asynchronously
        Application.DoEvents();   // This is necessary or else the Notification Window doesn't always get time to be fully formed

        // Create new data transfer thread and execute it
        ThreadStart dataXfer = new ThreadStart(DataXferStub);
        Thread dataXferThread = new Thread(dataXfer);
        dataXferThread.Start();

        AnimateDataTransferIcon(true);
      }
    }


    private void DataXferStub()
    {
      DataXfer.Start(ref rapi, this, statusForm);
    }


    private void AbortDataTransfer()
    {
      // This flag is periodically checked by the Data Xfer thread, so by explicitly
      // setting it to false will inform the thread that it needs to prematurely abort.
      SysInfo.Data.Admin.DataTransferActive = false;
    }


    #region PrimaryStatusBarMessage

    /// <summary>
    /// Displays a message in the center left of the status bar.  Messages may be coming from
    /// the Data Transfer thread so we have to ensure they're placed on the correct thread.
    /// </summary>
    private delegate void DisplayStatusMessageDelegate(string text, bool flash);
    private void DisplayStatusMessage(string text, bool flash)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new DisplayStatusMessageDelegate(DisplayStatusMessage), new object[] {text, flash});
        return;
      }

      // Only reaches here when on UI thread
      if (text == "")
        statusMsgFlashTimer.Stop();

      //if (this.WindowState != FormWindowState.Minimized)
      if (this.ShowInTaskbar)
      {
        statusBarMessage.Text = text;

        if (flash)
        {
          textStatusMsg = text;   // Necessary to store 'text' value because StatusBar panel doesn't have visible property
          statusMsgFlashTimer.Start();
        }
      }
    }

    // A simple timer that determines how long the status msg is displayed.
    private void SetMsgTimeLimit(int numOfSeconds)
    {
      System.Timers.Timer msgTimer = new System.Timers.Timer(numOfSeconds * 1000);
      msgTimer.AutoReset = false;   // Run timer for only one iteration
      msgTimer.Elapsed += new System.Timers.ElapsedEventHandler(StopMsgDisplay);   // Add handler
      msgTimer.Start();
    }

    private void StopMsgDisplay(object sender, System.Timers.ElapsedEventArgs e)
    {
      DisplayStatusMessage("", false);
    }

    private void FlashMessage(object sender, System.Timers.ElapsedEventArgs e)
    {
      flashMsgIteration++;

      if (flashMsgIteration < 3)
        statusBarMessage.Text = textStatusMsg;
      else
      {
        statusBarMessage.Text = "";
        flashMsgIteration = 0;
      }
    }

    #endregion


    #region NewDataAvailable

    /// <summary>
    /// Displays a "New Data Available" message in the left of the status bar.  Messages may be coming
    /// from the Data Transfer thread so we have to ensure it's placed on the correct thread.
    ///  mode:
    ///        0 - Clear statusbar panel and set back to default style
    ///        1 - Flash statusbar panel message for 3 seconds, changing style to "Raised"
    ///        2 - Display unflashing statusbar panel message
    ///        
    /// </summary>
    private delegate void DisplayNewDataMessageDelegate(byte mode);
    private void DisplayNewDataMessage(byte mode)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new DisplayNewDataMessageDelegate(DisplayNewDataMessage), new object[] {mode});
        return;
      }

      // Only reaches here when on UI thread

      switch (mode)
      {
        case 0:  // Reset panel back to original, blank state
          statusBarNewData.Text = "";
          statusBarNewData.BorderStyle = StatusBarPanelBorderStyle.Sunken;
          newDataMsgFlashTimer.Stop();
          break;

        case 1:  // Display msg and make flash (& gently beep) for 5 seconds
          statusBarNewData.Text = newDataMsg;
          statusBarNewData.BorderStyle = StatusBarPanelBorderStyle.Raised;
          newDataMsgCumulFlashTime = 5000;
          newDataMsgFlashTimer.AutoReset = true;
          newDataMsgFlashTimer.Elapsed += new System.Timers.ElapsedEventHandler(FlashNewDataMsg);
          newDataMsgFlashTimer.Start();
          break;

        case 2:  // Display unflashing msg
          statusBarNewData.Text = newDataMsg;
          statusBarNewData.BorderStyle = StatusBarPanelBorderStyle.Raised;
          newDataMsgFlashTimer.Stop();
          break;
      }
    }

    // A simple timer that determines how long the status msg is displayed.
    private void FlashNewDataMsg(object sender, System.Timers.ElapsedEventArgs e)
    {
      if (statusBarMessage.Text == "")
      {
        statusBarNewData.Text = newDataMsg;
        Tools.Beep(2500, 50);
      }
      else
        statusBarNewData.Text = "";
     
      newDataMsgCumulFlashTime = newDataMsgCumulFlashTime - (int) newDataMsgFlashTimer.Interval;

      if (newDataMsgCumulFlashTime <= 0)
        DisplayNewDataMessage(2);

    }



    /// <summary>
    /// Handles the clicking of the statusbar panels.
    /// Note: I don't yet know how to identify which specific panel was clicked so while the first 'if' statement isn't
    ///       technically correct, it'll suffice until we have to worry about another statusbar panel being clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void statusBar_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
    {
      if (e.StatusBarPanel.Text == newDataMsg)
      {
        string msg = "New data is available to be imported into the file(s) you currently have open.\n";
        msg = msg + "You can import this data immediately or it'll be done automatically when you exit.\n\n";
        msg = msg + "Would you like to import this data now?";
        if (Tools.ShowMessage(msg, SysInfo.Data.Admin.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          // Retrieves data from any residual Incoming data files.
          // Note: The Polls arraylist is passed to the method so that data is brought into any open files.
          ImportNewDataFiles(PollManager.Current.Polls);

          // The data has been imported so we can clear this message now
          DisplayNewDataMessage(0);                               
        }
      }
    }

    #endregion


    public void UpdateNotifyIconToolTip(string text)
    {
      this.notifyIcon.Text = text;
    }

    
    // Initiates shutdown of Pocket Pollster
    internal void ExitApp()
    {
      this.Close();
    }


    /// <summary>
    /// Enables/disables various menu items according to different criteria.
    /// </summary>
    /// <param name="pollCount"></param>
    public void SetMenuItems(int pollCount)
    {
      bool setting = (pollCount == 0) ? false : true;

      menuFileClose.Enabled = setting;
      menuFileCloseAll.Enabled = setting;
      menuFileSave.Enabled = setting;
      menuFileSaveAs.Enabled = setting;
      menuFilePublish.Enabled = setting;
      menuFileExport.Enabled = setting;

      menuWindowCascade.Enabled = setting;
      menuWindowTileHorizontal.Enabled = setting;
      menuWindowTileVertical.Enabled = setting;

      // If there's only one open file then some menu items don't really have any relevance (they're redundant or N/A)
      if (pollCount == 1)
      {
        menuFileCloseAll.Enabled = false;
        menuWindowCascade.Enabled = false;
        menuWindowTileHorizontal.Enabled = false;
        menuWindowTileVertical.Enabled = false;
      }

      SetToolBarButtons(pollCount);
    }


    private void SetToolBarButtons(int pollCount)
    {
      bool setting = (pollCount == 0) ? false : true;

      toolBarButtonClose.Enabled = setting;
      toolBarButtonSave.Enabled = setting;
      toolBarButtonPublish.Enabled = setting;
    }


    private void frmMain_Closed(object sender, System.EventArgs e)
    {
      this.Dispose();
    }

 
    private delegate bool IsFileOpenDelegate(string filename);

    /// <summary>
    /// Checks whether the specified file is currenly open in the desktop editor.
    /// Is threadsafe.
    /// </summary>
    /// <param name="filename"></param>  // The filename to check
    /// <returns></returns>
    private bool IsFileOpen(string filename)
    {
      if (this.InvokeRequired)
      {
        return (bool) this.Invoke(new IsFileOpenDelegate(IsFileOpen), new object[] {filename});
      }

      // Only reaches here when on UI thread
      return IsPollOpen(Tools.StripPathAndExtension(filename));
    }


    /// <summary>
    /// Examines the Incoming data folder for the presence of any files.  These files most likely haven't been
    /// processed yet because the user was working with their master copies at the time they were D/L.
    /// Note: For any poll that is open, we must import the data both into the stored file AND into the active data object.
    ///       The reason being that if the user chooses to not save his changes then this newly imported data would be lost too!
    /// </summary>
    /// <param name="openPolls"></param>   // If non-null, then is a collection of the polls (Controller objects) that are currently open
    public void ImportNewDataFiles(ArrayList openPolls)
    {
      try
      {
        string dataPath = SysInfo.Data.Paths.Data;
        string incomingPath = SysInfo.Data.Paths.Incoming;
        string archivePath = SysInfo.Data.Paths.Archive;

        // Get a list of the data files that are in the Data\Incoming folder
        string[] filesToProcess = Directory.GetFiles(incomingPath, Tools.GetAppFilter());
        int numFiles = filesToProcess.Length;

        if (numFiles > 0)
        {
          foreach (string fullFilename in filesToProcess)
          {
            string fileName = Tools.GetOriginalFilename(fullFilename);
            string baseName = Tools.StripPathAndExtension(fileName);
            string specificArchivePath = Tools.GetSpecificArchivePath(archivePath, baseName);

            DisplayStatusMessage("Importing: " + fileName, false);

            if (!File.Exists(dataPath + fileName))
            {
              // The master copy of the file doesn't exist yet so just put a copy into 'Data' (and archive it too)
              File.Copy(fullFilename, dataPath + fileName);
              File.Move(fullFilename, specificArchivePath + Tools.StripPath(fullFilename));
            }

            else
            {
              try
              {
                // First backup the file in case something goes wrong with the Append operation
                File.Copy(dataPath + fileName, dataPath + Tools.StripPathAndExtension(fileName) + ".bak", true);
                
                Poll activePoll = null;
                frmPoll activePollForm = null;   // Debug

                // Check if this poll is currently open
                foreach (ControllerClass controller in openPolls)
                {
                  if (baseName.ToLower() == controller.pollName.ToLower())
                  {
                    activePoll = controller.pollModel;
                    activePollForm = controller.pollForm;   // Debug: This is only here to provide the reference a few lines below
                    break;
                  }
                }

                bool appendOkay = Tools.AppendPoll(dataPath + fileName, fullFilename, activePoll);

                if (appendOkay)
                { 
                  // Now that we've successfully "imported" this new data, we can move the file from 'Incoming' to 'Archive'.
                  File.Move(fullFilename, specificArchivePath + Tools.StripPath(fullFilename));
                  
                  // Debug: If the this poll is currently open then we likely need to update the Responses tab page
                  activePollForm.UpdateCollectedData(activePoll);   // This still needs to be THOROUGHLY tested!!!
                }

                else   // Note: The append operation might not have happened for several reasons
                  File.Delete(fullFilename);
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
          }
        }
      }

      catch (Exception e)
      {
        Debug.Fail("Error Importing Newly D/L Data\n\n" + e.Message, "DataXfer.Start - Transfer");
      }

      finally
      {
        DisplayStatusMessage("", false);
      }
    }

    public void ImportNewDataFiles()
    {
      ImportNewDataFiles(new ArrayList());
    }


    #region UtilityForms

    private void ShowUserManager()
    {
      if (!IsUtilityFormDisplayed(ViewMode.Users))
      {
        frmView viewForm = null;
        Point pt = new Point(0,0);
        if (IsUtilityFormDisplayed(ViewMode.Devices, ref pt))
        {
          pt = new Point(pt.X + 200, pt.Y + 25);
          viewForm = new frmView(ViewMode.Users, pt);
        }
        else
          viewForm = new frmView(ViewMode.Users);
        
        UtilityForms.Add(viewForm);
        this.menuToolsUsers.Enabled = false;
        viewForm.ButtonOKEvent += new Desktop.frmView.ButtonOKEventHandler(ClosingUtilityForm);
      }
    }


    private void ShowDevices()
    {
      if (!IsUtilityFormDisplayed(ViewMode.Devices))
      {
        frmView viewForm = null;
        Point pt = new Point(0,0);
        if (IsUtilityFormDisplayed(ViewMode.Users, ref pt))
        {
          pt = new Point(pt.X + 200, pt.Y + 25);
          viewForm = new frmView(ViewMode.Devices, pt);
        }
        else
          viewForm = new frmView(ViewMode.Devices);

        UtilityForms.Add(viewForm);
        this.menuToolsDevices.Enabled = false;
        viewForm.ButtonOKEvent += new Desktop.frmView.ButtonOKEventHandler(ClosingUtilityForm);
      }
    }



    private bool IsUtilityFormDisplayed(ViewMode formType)
    {
      Point pt = new Point(0, 0);  // Dummy
      return IsUtilityFormDisplayed(formType, ref pt);
    }


    /// <summary>
    /// Checks to see whether a utility form of the specified type is already open.
    /// If so, then it doesn't open a second one.
    /// </summary>
    /// <param name="formType"></param>
    /// <returns></returns>
    private bool IsUtilityFormDisplayed(ViewMode formType, ref Point location)
    {
      bool isVisible = false;

      foreach(object obj in UtilityForms)
      {
        frmView viewForm = obj as frmView;
        if (viewForm.FormType == formType)
        {
          isVisible = true;
          location = viewForm.Location;
          break;
        }
      }

      return isVisible;
    }


    private void ClosingUtilityForm(ViewMode formType, EventArgs e)
    {
      foreach(object obj in UtilityForms)
      {
        frmView viewForm = obj as frmView;
        if (viewForm.FormType == formType)
        {
          UtilityForms.Remove(obj);

          switch (formType)
          {
            case ViewMode.Users:
              this.menuToolsUsers.Enabled = true;

              // It's possible that the Primary User was deleted and no new one was added.  Give the user a chance to
              // define a new one, just as if we were starting the app for the first time.
              if (SysInfo.Data.Options.PrimaryUser == "")
              {
                viewForm.Visible = false;
                Tools.ShowMessage("There must always be a primary user, so now you need to redefine one.", "Primary User Deleted");

                // We'll startup the Options form in a special way that will force (strongly request) a UserName be specified
                frmOptions optionsForm = new frmOptions(true);

                // If user really, really didn't want to specify a username then we MUST exit app
                if (SysInfo.Data.Options.PrimaryUser == "")
                  this.ExitApp();
              }
              
              break;

            case ViewMode.Devices:
              this.menuToolsDevices.Enabled = true;
              break;

            default:
              Debug.Fail("Unaccounted for ViewMode: " + formType.ToString(), "frmMain.ClosingUtilityForm");
              break;
          }

          break;
        }
      }
    }


    private void ShowRespondents()
    {
      frmRespondents respondentsForm = new frmRespondents();
    }


    private void ShowOptions()
    {
      frmOptions optionsForm = new frmOptions();
    }


    #endregion



    // Simply used to initiate test code, some of which must only be run once form is fully instantiated.
    // Note: Testing revealed that 200ms does not seem to provide sufficient time for everything to get properly
    //       setup for all code to work properly.  Increasing to 1000ms seems to have resolved this.
    private void testCodeTimer_Tick(object sender, System.EventArgs e)
    {
      testCodeTimer.Enabled = false;
      TestCode();
    }


    private void frmMain_Load(object sender, System.EventArgs e)
    {
      this.Text = SysInfo.Data.Admin.AppName;

      try
      {
        this.Connect();   // Prelim check to see if we're already connected

        if (rapi.Connected)
          this.statusBarDataXferStatus.Text = ConnectedMsg; 
      }
      catch
      {
        // This exception will generally only occur if ActiveSync is not installed.  But an error message
        // is provided elsewhere so just trap the error and continue.
      }

      panelExpired.Location = new Point((this.ClientRectangle.Width - panelExpired.Width) / 2, (this.ClientRectangle.Height - panelExpired.Height) / 2);
      linkExpiredWebsite.Links.Add(0, linkExpiredWebsite.Text.Length, "http://pocketpollster.com");

      
      if (Screen.PrimaryScreen.Bounds.Width < 1000)
      {
        panelScreenSize.Location = new Point((this.ClientRectangle.Width - panelScreenSize.Width) / 2, (this.ClientRectangle.Height - panelScreenSize.Height) / 2);
        panelScreenSize.Visible = true;
      }

      Application.DoEvents(); 
    }


    private void PrepareToEndSplashScreen()
    {
      splashShutdownTimer.AutoReset = false;
      splashShutdownTimer.Elapsed += new System.Timers.ElapsedEventHandler(EndSplashScreen);
      splashShutdownTimer.Start();
    }

    private void EndSplashScreen(object sender, System.Timers.ElapsedEventArgs e)
    {
      frmSplash.CloseForm();
    }


    // This method freezes all of the features on the form.  It is generally called
    // if the software is being used illegally.
    internal void Freeze()
    {
      toolBarButtonNew.Enabled = false;
      toolBarButtonOpen.Enabled = false;
      menuFileNew.Enabled = false;
      menuFileOpen.Enabled = false;
      menuFileClose.Enabled = false;
      menuFileSave.Enabled = false;
      menuFileSaveAs.Enabled = false;
      menuFilePublish.Enabled = false;
      menuFileExport.Enabled = false;

      panelExpired.Visible = true;
    }

    private void linkExpiredWebsite_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
    {
       System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
    }




    private void TestCode()
    {
      // Note: Opening polls like this works for basic testing but not all functionality works properly when we do it this way.
      //PreparePoll(SysInfo.Data.Paths.Templates + "Kevin Falcon's Talk.pp");
      PreparePoll(SysInfo.Data.Paths.Data + "2006 Canadian Political Election.pp");
    }

    private void panelInner2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
    {
    
    }




  }
}
