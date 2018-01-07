using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using DataObjects;



namespace Desktop
{
  // Define Aliases
  using PurgeDuration = DataObjects.Constants.PurgeDuration;
  using MDBformat = DataObjects.Constants.MDBformat;


	/// <summary>
	/// Summary description for frmOptions.
	/// </summary>
	public class frmOptions : System.Windows.Forms.Form
	{
    #region Control Definitions
    private System.Windows.Forms.ToolTip toolTip1;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.TabControl tabOptions;
    private System.Windows.Forms.TabPage tabPageGeneral;
    private System.Windows.Forms.TabPage tabPageDataXfer;
    private DataObjects.PanelGradient panelGeneral;
    private System.Windows.Forms.Panel panelOtherOptions;
    private System.Windows.Forms.ComboBox comboMDBformat;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Panel panelUserName;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox textBoxFirstName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox textBoxLastName;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label labelUserName;
    private DataObjects.PanelGradient panelInstructions;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.TabPage tabPageMobile;
    private System.Windows.Forms.NumericUpDown spinnerBatteryWarningLevel;
    private System.Windows.Forms.ComboBox comboPurgeDuration;
    private System.Windows.Forms.CheckBox checkBoxAutoUpdate;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox checkBoxOverrideInstallBlock;
    private System.Windows.Forms.CheckBox checkBoxGetPersonalInfo;
    private System.Windows.Forms.CheckBox checkBoxHideQuestionNumbers;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label labelInstructions;
    private System.Windows.Forms.Label label10;
    private PanelGradient.PanelGradient panelBottom;
    private PanelGradient.PanelGradient panelMobile;
    private PanelGradient.PanelGradient panelDataXfer;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.CheckBox checkBoxUnattendedSync;
    private System.Windows.Forms.CheckBox checkBoxSyncAudio;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.CheckBox checkBoxShowSplash;
    #endregion


    // Modular level variables
    private bool StartupMode = false;


		public frmOptions()
		{
			InitializeComponent();
      PopulateForm();
      this.ShowDialog();
		}


    public frmOptions(bool startupMode)
    {
      InitializeComponent();

      StartupMode = startupMode;

      // This is a special mode that is ONLY used when the program is first installed
      // and a UserName must be assigned to the computer the app is running on.
      if (StartupMode)
      {
        this.Text = "Inititialize Pocket Pollster";
        panelInstructions.Visible = true;
        buttonOK.Enabled = false;
      }
      else
      {
        panelInstructions.Visible = false;
      }

      PopulateForm();
      this.ShowDialog();
    }


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}

			base.Dispose(disposing);
		}



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.components = new System.ComponentModel.Container();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.comboMDBformat = new System.Windows.Forms.ComboBox();
      this.label7 = new System.Windows.Forms.Label();
      this.textBoxFirstName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.textBoxLastName = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.spinnerBatteryWarningLevel = new System.Windows.Forms.NumericUpDown();
      this.comboPurgeDuration = new System.Windows.Forms.ComboBox();
      this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
      this.checkBoxOverrideInstallBlock = new System.Windows.Forms.CheckBox();
      this.checkBoxGetPersonalInfo = new System.Windows.Forms.CheckBox();
      this.checkBoxHideQuestionNumbers = new System.Windows.Forms.CheckBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.checkBoxUnattendedSync = new System.Windows.Forms.CheckBox();
      this.checkBoxSyncAudio = new System.Windows.Forms.CheckBox();
      this.checkBoxShowSplash = new System.Windows.Forms.CheckBox();
      this.label10 = new System.Windows.Forms.Label();
      this.tabOptions = new System.Windows.Forms.TabControl();
      this.tabPageGeneral = new System.Windows.Forms.TabPage();
      this.panelGeneral = new DataObjects.PanelGradient();
      this.panelOtherOptions = new System.Windows.Forms.Panel();
      this.label12 = new System.Windows.Forms.Label();
      this.panelUserName = new System.Windows.Forms.Panel();
      this.label3 = new System.Windows.Forms.Label();
      this.labelUserName = new System.Windows.Forms.Label();
      this.tabPageDataXfer = new System.Windows.Forms.TabPage();
      this.panelDataXfer = new PanelGradient.PanelGradient();
      this.label11 = new System.Windows.Forms.Label();
      this.tabPageMobile = new System.Windows.Forms.TabPage();
      this.panelMobile = new PanelGradient.PanelGradient();
      this.label1 = new System.Windows.Forms.Label();
      this.panelInstructions = new DataObjects.PanelGradient();
      this.labelInstructions = new System.Windows.Forms.Label();
      this.panelBottom = new PanelGradient.PanelGradient();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      ((System.ComponentModel.ISupportInitialize)(this.spinnerBatteryWarningLevel)).BeginInit();
      this.tabOptions.SuspendLayout();
      this.tabPageGeneral.SuspendLayout();
      this.panelGeneral.SuspendLayout();
      this.panelOtherOptions.SuspendLayout();
      this.panelUserName.SuspendLayout();
      this.tabPageDataXfer.SuspendLayout();
      this.panelDataXfer.SuspendLayout();
      this.tabPageMobile.SuspendLayout();
      this.panelMobile.SuspendLayout();
      this.panelInstructions.SuspendLayout();
      this.panelBottom.SuspendLayout();
      this.SuspendLayout();
      // 
      // comboMDBformat
      // 
      this.comboMDBformat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboMDBformat.Location = new System.Drawing.Point(158, 37);
      this.comboMDBformat.MaxDropDownItems = 10;
      this.comboMDBformat.Name = "comboMDBformat";
      this.comboMDBformat.Size = new System.Drawing.Size(80, 21);
      this.comboMDBformat.TabIndex = 3;
      this.toolTip1.SetToolTip(this.comboMDBformat, "Choose the MDB format you prefer");
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label7.Location = new System.Drawing.Point(24, 40);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(138, 16);
      this.label7.TabIndex = 1;
      this.label7.Text = "MS Access Export Format:";
      this.toolTip1.SetToolTip(this.label7, "Choose the MDB format you prefer");
      // 
      // textBoxFirstName
      // 
      this.textBoxFirstName.Location = new System.Drawing.Point(86, 38);
      this.textBoxFirstName.Name = "textBoxFirstName";
      this.textBoxFirstName.Size = new System.Drawing.Size(76, 20);
      this.textBoxFirstName.TabIndex = 1;
      this.textBoxFirstName.Text = "";
      this.toolTip1.SetToolTip(this.textBoxFirstName, "Enter the first name of the main user of this desktop application");
      this.textBoxFirstName.TextChanged += new System.EventHandler(this.UserNameChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label2.Location = new System.Drawing.Point(8, 8);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(119, 16);
      this.label2.TabIndex = 0;
      this.label2.Text = "Primary User\'s Name:";
      this.toolTip1.SetToolTip(this.label2, "The name of the person using this computer");
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(24, 40);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(63, 16);
      this.label4.TabIndex = 0;
      this.label4.Text = "First Name:";
      this.toolTip1.SetToolTip(this.label4, "Enter the first name of the main user of this desktop application");
      // 
      // textBoxLastName
      // 
      this.textBoxLastName.Location = new System.Drawing.Point(263, 38);
      this.textBoxLastName.Name = "textBoxLastName";
      this.textBoxLastName.Size = new System.Drawing.Size(76, 20);
      this.textBoxLastName.TabIndex = 1;
      this.textBoxLastName.Text = "";
      this.toolTip1.SetToolTip(this.textBoxLastName, "Enter the last name of the main user of this desktop application");
      this.textBoxLastName.TextChanged += new System.EventHandler(this.UserNameChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(201, 40);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(62, 16);
      this.label5.TabIndex = 0;
      this.label5.Text = "Last Name:";
      this.toolTip1.SetToolTip(this.label5, "Enter the last name of the main user of this desktop application");
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(408, 40);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(61, 16);
      this.label6.TabIndex = 0;
      this.label6.Text = "UserName:";
      this.toolTip1.SetToolTip(this.label6, "This UserName is automatically generated from the First && Last Names");
      // 
      // spinnerBatteryWarningLevel
      // 
      this.spinnerBatteryWarningLevel.Increment = new System.Decimal(new int[] {
                                                                                 5,
                                                                                 0,
                                                                                 0,
                                                                                 0});
      this.spinnerBatteryWarningLevel.Location = new System.Drawing.Point(424, 125);
      this.spinnerBatteryWarningLevel.Maximum = new System.Decimal(new int[] {
                                                                               75,
                                                                               0,
                                                                               0,
                                                                               0});
      this.spinnerBatteryWarningLevel.Minimum = new System.Decimal(new int[] {
                                                                               5,
                                                                               0,
                                                                               0,
                                                                               0});
      this.spinnerBatteryWarningLevel.Name = "spinnerBatteryWarningLevel";
      this.spinnerBatteryWarningLevel.Size = new System.Drawing.Size(48, 20);
      this.spinnerBatteryWarningLevel.TabIndex = 12;
      this.toolTip1.SetToolTip(this.spinnerBatteryWarningLevel, "How low should the battery level on a mobile device be allowed to drop before a r" +
        "echarge reminder is shown");
      this.spinnerBatteryWarningLevel.Value = new System.Decimal(new int[] {
                                                                             5,
                                                                             0,
                                                                             0,
                                                                             0});
      this.spinnerBatteryWarningLevel.Leave += new System.EventHandler(this.spinnerBatteryWarningLevel_Leave);
      // 
      // comboPurgeDuration
      // 
      this.comboPurgeDuration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboPurgeDuration.Location = new System.Drawing.Point(105, 125);
      this.comboPurgeDuration.MaxDropDownItems = 10;
      this.comboPurgeDuration.Name = "comboPurgeDuration";
      this.comboPurgeDuration.Size = new System.Drawing.Size(130, 21);
      this.comboPurgeDuration.TabIndex = 11;
      this.toolTip1.SetToolTip(this.comboPurgeDuration, "How long after a poll is completed should its data file be removed on the mobile " +
        "device");
      // 
      // checkBoxAutoUpdate
      // 
      this.checkBoxAutoUpdate.Location = new System.Drawing.Point(24, 40);
      this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
      this.checkBoxAutoUpdate.Size = new System.Drawing.Size(136, 16);
      this.checkBoxAutoUpdate.TabIndex = 9;
      this.checkBoxAutoUpdate.Tag = "";
      this.checkBoxAutoUpdate.Text = "Auto Update";
      this.toolTip1.SetToolTip(this.checkBoxAutoUpdate, "Automatically installs mobile app onto mobile device without displaying Installat" +
        "ion dialog box");
      // 
      // checkBoxOverrideInstallBlock
      // 
      this.checkBoxOverrideInstallBlock.Location = new System.Drawing.Point(24, 72);
      this.checkBoxOverrideInstallBlock.Name = "checkBoxOverrideInstallBlock";
      this.checkBoxOverrideInstallBlock.Size = new System.Drawing.Size(160, 16);
      this.checkBoxOverrideInstallBlock.TabIndex = 10;
      this.checkBoxOverrideInstallBlock.Tag = "";
      this.checkBoxOverrideInstallBlock.Text = "Override Installation Block";
      this.toolTip1.SetToolTip(this.checkBoxOverrideInstallBlock, "A user can restrict installation of the app onto a mobile device; this overrides " +
        "that restriction");
      // 
      // checkBoxGetPersonalInfo
      // 
      this.checkBoxGetPersonalInfo.Location = new System.Drawing.Point(240, 40);
      this.checkBoxGetPersonalInfo.Name = "checkBoxGetPersonalInfo";
      this.checkBoxGetPersonalInfo.Size = new System.Drawing.Size(200, 16);
      this.checkBoxGetPersonalInfo.TabIndex = 7;
      this.checkBoxGetPersonalInfo.Tag = "";
      this.checkBoxGetPersonalInfo.Text = "Gather Respondents\' Personal Info";
      this.toolTip1.SetToolTip(this.checkBoxGetPersonalInfo, "Request personal info from each respondent");
      // 
      // checkBoxHideQuestionNumbers
      // 
      this.checkBoxHideQuestionNumbers.Location = new System.Drawing.Point(240, 72);
      this.checkBoxHideQuestionNumbers.Name = "checkBoxHideQuestionNumbers";
      this.checkBoxHideQuestionNumbers.Size = new System.Drawing.Size(160, 16);
      this.checkBoxHideQuestionNumbers.TabIndex = 8;
      this.checkBoxHideQuestionNumbers.Tag = "";
      this.checkBoxHideQuestionNumbers.Text = "Hide Question Numbers";
      this.toolTip1.SetToolTip(this.checkBoxHideQuestionNumbers, "Hide question numbers from mobile users");
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(24, 128);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(83, 16);
      this.label8.TabIndex = 5;
      this.label8.Text = "Purge Duration:";
      this.toolTip1.SetToolTip(this.label8, "How long after a poll is completed should its data file be removed on the mobile " +
        "device");
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(304, 128);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(119, 16);
      this.label9.TabIndex = 6;
      this.label9.Text = "Battery Warning Level:";
      this.toolTip1.SetToolTip(this.label9, "How low should the battery level on a mobile device be allowed to drop before a r" +
        "echarge reminder is shown");
      // 
      // checkBoxUnattendedSync
      // 
      this.checkBoxUnattendedSync.Location = new System.Drawing.Point(24, 40);
      this.checkBoxUnattendedSync.Name = "checkBoxUnattendedSync";
      this.checkBoxUnattendedSync.Size = new System.Drawing.Size(120, 16);
      this.checkBoxUnattendedSync.TabIndex = 6;
      this.checkBoxUnattendedSync.Text = "Unattended Sync";
      this.toolTip1.SetToolTip(this.checkBoxUnattendedSync, "Allows synching to occur even when Pocket Pollster is not running");
      // 
      // checkBoxSyncAudio
      // 
      this.checkBoxSyncAudio.Location = new System.Drawing.Point(24, 72);
      this.checkBoxSyncAudio.Name = "checkBoxSyncAudio";
      this.checkBoxSyncAudio.Size = new System.Drawing.Size(120, 16);
      this.checkBoxSyncAudio.TabIndex = 7;
      this.checkBoxSyncAudio.Text = "Audible Feedback";
      this.toolTip1.SetToolTip(this.checkBoxSyncAudio, "Provides the mobile user with audible feedback");
      // 
      // checkBoxShowSplash
      // 
      this.checkBoxShowSplash.Location = new System.Drawing.Point(24, 120);
      this.checkBoxShowSplash.Name = "checkBoxShowSplash";
      this.checkBoxShowSplash.Size = new System.Drawing.Size(128, 16);
      this.checkBoxShowSplash.TabIndex = 7;
      this.checkBoxShowSplash.Text = "Show Splash Screen";
      this.toolTip1.SetToolTip(this.checkBoxShowSplash, "Determines whether the Splash Screen is shown");
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label10.Location = new System.Drawing.Point(8, 8);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(42, 16);
      this.label10.TabIndex = 4;
      this.label10.Text = "Export:";
      // 
      // tabOptions
      // 
      this.tabOptions.Controls.Add(this.tabPageGeneral);
      this.tabOptions.Controls.Add(this.tabPageDataXfer);
      this.tabOptions.Controls.Add(this.tabPageMobile);
      this.tabOptions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabOptions.ItemSize = new System.Drawing.Size(60, 18);
      this.tabOptions.Location = new System.Drawing.Point(0, 56);
      this.tabOptions.Name = "tabOptions";
      this.tabOptions.Padding = new System.Drawing.Point(12, 3);
      this.tabOptions.SelectedIndex = 0;
      this.tabOptions.Size = new System.Drawing.Size(654, 316);
      this.tabOptions.TabIndex = 1;
      // 
      // tabPageGeneral
      // 
      this.tabPageGeneral.BackColor = System.Drawing.Color.Transparent;
      this.tabPageGeneral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tabPageGeneral.Controls.Add(this.panelGeneral);
      this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
      this.tabPageGeneral.Name = "tabPageGeneral";
      this.tabPageGeneral.Size = new System.Drawing.Size(646, 290);
      this.tabPageGeneral.TabIndex = 0;
      this.tabPageGeneral.Text = "General";
      // 
      // panelGeneral
      // 
      this.panelGeneral.Controls.Add(this.panelOtherOptions);
      this.panelGeneral.Controls.Add(this.panelUserName);
      this.panelGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelGeneral.GradientColorOne = System.Drawing.Color.Ivory;
      this.panelGeneral.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(128)));
      this.panelGeneral.Location = new System.Drawing.Point(0, 0);
      this.panelGeneral.Name = "panelGeneral";
      this.panelGeneral.Size = new System.Drawing.Size(644, 288);
      this.panelGeneral.TabIndex = 1;
      // 
      // panelOtherOptions
      // 
      this.panelOtherOptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelOtherOptions.Controls.Add(this.checkBoxShowSplash);
      this.panelOtherOptions.Controls.Add(this.label12);
      this.panelOtherOptions.Controls.Add(this.label10);
      this.panelOtherOptions.Controls.Add(this.comboMDBformat);
      this.panelOtherOptions.Controls.Add(this.label7);
      this.panelOtherOptions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelOtherOptions.DockPadding.All = 8;
      this.panelOtherOptions.Location = new System.Drawing.Point(0, 80);
      this.panelOtherOptions.Name = "panelOtherOptions";
      this.panelOtherOptions.Size = new System.Drawing.Size(644, 208);
      this.panelOtherOptions.TabIndex = 9;
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label12.Location = new System.Drawing.Point(8, 88);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(82, 16);
      this.label12.TabIndex = 5;
      this.label12.Text = "Miscellaneous:";
      // 
      // panelUserName
      // 
      this.panelUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelUserName.Controls.Add(this.label3);
      this.panelUserName.Controls.Add(this.textBoxFirstName);
      this.panelUserName.Controls.Add(this.label2);
      this.panelUserName.Controls.Add(this.label4);
      this.panelUserName.Controls.Add(this.textBoxLastName);
      this.panelUserName.Controls.Add(this.label5);
      this.panelUserName.Controls.Add(this.label6);
      this.panelUserName.Controls.Add(this.labelUserName);
      this.panelUserName.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelUserName.DockPadding.Bottom = 16;
      this.panelUserName.DockPadding.Left = 8;
      this.panelUserName.DockPadding.Right = 8;
      this.panelUserName.DockPadding.Top = 8;
      this.panelUserName.Location = new System.Drawing.Point(0, 0);
      this.panelUserName.Name = "panelUserName";
      this.panelUserName.Size = new System.Drawing.Size(644, 80);
      this.panelUserName.TabIndex = 5;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(127, 6);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(0, 16);
      this.label3.TabIndex = 0;
      // 
      // labelUserName
      // 
      this.labelUserName.AutoSize = true;
      this.labelUserName.ForeColor = System.Drawing.Color.Blue;
      this.labelUserName.Location = new System.Drawing.Point(468, 40);
      this.labelUserName.Name = "labelUserName";
      this.labelUserName.Size = new System.Drawing.Size(0, 16);
      this.labelUserName.TabIndex = 0;
      // 
      // tabPageDataXfer
      // 
      this.tabPageDataXfer.BackColor = System.Drawing.Color.Transparent;
      this.tabPageDataXfer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.tabPageDataXfer.Controls.Add(this.panelDataXfer);
      this.tabPageDataXfer.Location = new System.Drawing.Point(4, 22);
      this.tabPageDataXfer.Name = "tabPageDataXfer";
      this.tabPageDataXfer.Size = new System.Drawing.Size(646, 290);
      this.tabPageDataXfer.TabIndex = 1;
      this.tabPageDataXfer.Text = "Data Transfer";
      // 
      // panelDataXfer
      // 
      this.panelDataXfer.Controls.Add(this.checkBoxSyncAudio);
      this.panelDataXfer.Controls.Add(this.checkBoxUnattendedSync);
      this.panelDataXfer.Controls.Add(this.label11);
      this.panelDataXfer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelDataXfer.GradientColorOne = System.Drawing.Color.AliceBlue;
      this.panelDataXfer.GradientColorTwo = System.Drawing.Color.RoyalBlue;
      this.panelDataXfer.Location = new System.Drawing.Point(0, 0);
      this.panelDataXfer.Name = "panelDataXfer";
      this.panelDataXfer.Size = new System.Drawing.Size(644, 288);
      this.panelDataXfer.TabIndex = 0;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label11.Location = new System.Drawing.Point(8, 8);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(56, 16);
      this.label11.TabIndex = 5;
      this.label11.Text = "Synching:";
      // 
      // tabPageMobile
      // 
      this.tabPageMobile.BackColor = System.Drawing.Color.Transparent;
      this.tabPageMobile.Controls.Add(this.panelMobile);
      this.tabPageMobile.Location = new System.Drawing.Point(4, 22);
      this.tabPageMobile.Name = "tabPageMobile";
      this.tabPageMobile.Size = new System.Drawing.Size(646, 290);
      this.tabPageMobile.TabIndex = 2;
      this.tabPageMobile.Text = "Mobile";
      // 
      // panelMobile
      // 
      this.panelMobile.Controls.Add(this.spinnerBatteryWarningLevel);
      this.panelMobile.Controls.Add(this.comboPurgeDuration);
      this.panelMobile.Controls.Add(this.checkBoxAutoUpdate);
      this.panelMobile.Controls.Add(this.label1);
      this.panelMobile.Controls.Add(this.checkBoxOverrideInstallBlock);
      this.panelMobile.Controls.Add(this.checkBoxGetPersonalInfo);
      this.panelMobile.Controls.Add(this.checkBoxHideQuestionNumbers);
      this.panelMobile.Controls.Add(this.label8);
      this.panelMobile.Controls.Add(this.label9);
      this.panelMobile.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMobile.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(215)), ((System.Byte)(255)), ((System.Byte)(215)));
      this.panelMobile.GradientColorTwo = System.Drawing.Color.MediumSeaGreen;
      this.panelMobile.Location = new System.Drawing.Point(0, 0);
      this.panelMobile.Name = "panelMobile";
      this.panelMobile.Size = new System.Drawing.Size(646, 290);
      this.panelMobile.TabIndex = 7;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label1.Location = new System.Drawing.Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(89, 16);
      this.label1.TabIndex = 4;
      this.label1.Text = "Mobile Defaults:";
      // 
      // panelInstructions
      // 
      this.panelInstructions.Controls.Add(this.labelInstructions);
      this.panelInstructions.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelInstructions.DockPadding.Bottom = 10;
      this.panelInstructions.DockPadding.Left = 8;
      this.panelInstructions.DockPadding.Right = 8;
      this.panelInstructions.DockPadding.Top = 12;
      this.panelInstructions.GradientColorOne = System.Drawing.Color.AliceBlue;
      this.panelInstructions.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.panelInstructions.Location = new System.Drawing.Point(0, 0);
      this.panelInstructions.Name = "panelInstructions";
      this.panelInstructions.Size = new System.Drawing.Size(654, 56);
      this.panelInstructions.TabIndex = 7;
      // 
      // labelInstructions
      // 
      this.labelInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelInstructions.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelInstructions.ForeColor = System.Drawing.Color.Blue;
      this.labelInstructions.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelInstructions.Location = new System.Drawing.Point(8, 12);
      this.labelInstructions.Name = "labelInstructions";
      this.labelInstructions.Size = new System.Drawing.Size(638, 34);
      this.labelInstructions.TabIndex = 14;
      this.labelInstructions.Text = "Before you can use Pocket Pollster, you need to enter your first && last name.  Y" +
        "ou can also set any of the other defaults too.  All of this information can be c" +
        "hanged later.";
      // 
      // panelBottom
      // 
      this.panelBottom.Controls.Add(this.buttonOK);
      this.panelBottom.Controls.Add(this.buttonCancel);
      this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelBottom.GradientColorOne = System.Drawing.Color.LightBlue;
      this.panelBottom.GradientColorTwo = System.Drawing.Color.WhiteSmoke;
      this.panelBottom.Location = new System.Drawing.Point(0, 332);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new System.Drawing.Size(654, 40);
      this.panelBottom.TabIndex = 8;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.BackColor = System.Drawing.SystemColors.Control;
      this.buttonOK.Location = new System.Drawing.Point(504, 8);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 6;
      this.buttonOK.Text = "OK";
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.BackColor = System.Drawing.SystemColors.Control;
      this.buttonCancel.Location = new System.Drawing.Point(577, 8);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(64, 24);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // panel1
      // 
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 312);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(654, 20);
      this.panel1.TabIndex = 9;
      // 
      // frmOptions
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(654, 372);
      this.ControlBox = false;
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.panelBottom);
      this.Controls.Add(this.tabOptions);
      this.Controls.Add(this.panelInstructions);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmOptions";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Options";
      this.TopMost = true;
      ((System.ComponentModel.ISupportInitialize)(this.spinnerBatteryWarningLevel)).EndInit();
      this.tabOptions.ResumeLayout(false);
      this.tabPageGeneral.ResumeLayout(false);
      this.panelGeneral.ResumeLayout(false);
      this.panelOtherOptions.ResumeLayout(false);
      this.panelUserName.ResumeLayout(false);
      this.tabPageDataXfer.ResumeLayout(false);
      this.panelDataXfer.ResumeLayout(false);
      this.tabPageMobile.ResumeLayout(false);
      this.panelMobile.ResumeLayout(false);
      this.panelInstructions.ResumeLayout(false);
      this.panelBottom.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		#endregion


    // Populate controls with current values
    private void PopulateForm()
    {
      // GENERAL Tab

      _User user = SysInfo.Data.Users.Find_User(SysInfo.Data.Options.PrimaryUser);

      if (user != null)
      {
        textBoxFirstName.Text = user.FirstName;
        textBoxLastName.Text = user.LastName;
        labelUserName.Text = user.Name;  // Same as PrimaryUser
      }

      // Populate the MS Access Export Format combobox
      comboMDBformat.DataSource = Enum.GetNames(typeof(MDBformat));
      comboMDBformat.SelectedItem = SysInfo.Data.Options.MDBformat.ToString();

      checkBoxShowSplash.Checked = SysInfo.Data.Options.ShowSplash;


      // DATA TRANSFER Tab
      checkBoxUnattendedSync.Checked = SysInfo.Data.Options.DataXfer.UnattendedSync;
      checkBoxSyncAudio.Checked = SysInfo.Data.Options.DataXfer.Sound;


      // MOBILE Tab
      checkBoxAutoUpdate.Checked = SysInfo.Data.Options.Mobile.AutoUpdate;
      checkBoxOverrideInstallBlock.Checked = SysInfo.Data.Options.Mobile.OverrideInstallBlock;
      checkBoxGetPersonalInfo.Checked = SysInfo.Data.Options.Mobile.GetPersonalInfo;
      checkBoxHideQuestionNumbers.Checked = SysInfo.Data.Options.Mobile.HideQuestionNumbers;

      // Get the current Purge Duration value
      string currText = Enum.GetName(typeof(PurgeDuration), SysInfo.Data.Options.Mobile.PurgeDuration).Replace("_", " ");

      // Populate the PurgeDuration ComboBox
      int idx = 0;
      foreach (int val in Enum.GetValues(typeof(PurgeDuration)))
      {      
        string pdText = Enum.GetName(typeof(PurgeDuration), val);
        pdText = pdText.Replace("_", " ");
        comboPurgeDuration.Items.Add(pdText);
        
        if (pdText == currText)
          comboPurgeDuration.SelectedIndex = idx;

        idx++;
      }

      // Set the current Battery Warning Level
      spinnerBatteryWarningLevel.Value = SysInfo.Data.Options.Mobile.BatteryWarningLevel;
    }




    /// <summary>
    /// Save any changed data back to SysInfo.
    /// </summary>
    private void SaveData()
    {
      // Retrieve original user again
      _User user = SysInfo.Data.Users.Find_User(SysInfo.Data.Options.PrimaryUser);

      // GENERAL Tab

      // If the User name info is changed then we need to handle it specially
      string firstName = textBoxFirstName.Text.Trim();
      string lastName = textBoxLastName.Text.Trim();
      string userName = labelUserName.Text;

      if (user == null || firstName != user.FirstName || lastName != user.LastName)
      {
        // The method being called takes care of validating the username.  Sometimes it returns a slightly different version of the username.
        string newUserName = DataObjects.UserMgr.ExamineUser(userName, firstName, lastName, SysInfo.Data.Admin.Guid, true);

        if (newUserName != SysInfo.Data.Options.PrimaryUser)
          SysInfo.Data.Options.PrimaryUser = newUserName;
      }

      // Now save the MDB Access Format selection
      SysInfo.Data.Options.MDBformat = (MDBformat) Enum.Parse(typeof(MDBformat), comboMDBformat.SelectedItem.ToString());

      SysInfo.Data.Options.ShowSplash = checkBoxShowSplash.Checked;


      // DATA TRANSFER Tab
      SysInfo.Data.Options.DataXfer.UnattendedSync = checkBoxUnattendedSync.Checked;
      SysInfo.Data.Options.DataXfer.Sound = checkBoxSyncAudio.Checked;


      // MOBILE Tab

      // Checkboxes
      SysInfo.Data.Options.Mobile.AutoUpdate = checkBoxAutoUpdate.Checked;
      SysInfo.Data.Options.Mobile.OverrideInstallBlock = checkBoxOverrideInstallBlock.Checked;
      SysInfo.Data.Options.Mobile.GetPersonalInfo = checkBoxGetPersonalInfo.Checked;
      SysInfo.Data.Options.Mobile.HideQuestionNumbers = checkBoxHideQuestionNumbers.Checked;

      // Get the Purge Duration value from the combobox
      SysInfo.Data.Options.Mobile.PurgeDuration = (int) Enum.GetValues(typeof(PurgeDuration)).GetValue(comboPurgeDuration.SelectedIndex);  

      // Retrieve the value from the Battery Warning Level spinner
      SysInfo.Data.Options.Mobile.BatteryWarningLevel = (byte) spinnerBatteryWarningLevel.Value;
    }


    #region EventHandlers

    /// <summary>
    /// Save data back to SysInfo property model and then close form.
    /// </summary>
    private void buttonOK_Click(object sender, System.EventArgs e)
    {
      SaveData();

      if (StartupMode)
      {
        string msg = "Thank you for registering, " + textBoxFirstName.Text.Trim() + ".\n\nWelcome to " + SysInfo.Data.Admin.AppName + " ! ! !";
        Tools.ShowMessage(msg, SysInfo.Data.Admin.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }

      this.Close();
    }


    /// <summary>
    /// Close form without saving data.
    /// </summary>
    private void buttonCancel_Click(object sender, System.EventArgs e)
    {
      if (StartupMode && labelUserName.Text == "")
      {
        string msg = SysInfo.Data.Admin.AppName + " can not continue if the primary username is not specified.  Are you sure you want to quit?";
        if (Tools.ShowMessage(msg, "Cancellation Request", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
          return;
      }

      this.Close();
    }


    // I'm not sure why, but one can enter a value smaller than Min or greater than Max and it accepts it.
    // So we'll add code here to prevent these erroneous values from being entered.
    private void spinnerBatteryWarningLevel_Leave(object sender, System.EventArgs e)
    {
      NumericUpDown spinner = sender as NumericUpDown;

      if (spinner.Value < spinner.Minimum)
        spinner.Value = spinner.Minimum;

      else if (spinner.Value > spinner.Maximum)
        spinner.Value = spinner.Maximum;
    }


    private void UserNameChanged(object sender, System.EventArgs e)
    {
      string text1 = textBoxFirstName.Text.ToLower();
      string text2 = textBoxLastName.Text.ToLower();

      if (text1 == "" || text2 == "")
      {
        labelUserName.Text = "";
        if (StartupMode)
          buttonOK.Enabled = false;
      }
      else
      {
        labelUserName.Text = text1.Substring(0, 1) + text2;
        if (StartupMode)
          buttonOK.Enabled = true;
      }
    }

    #endregion

	}
}
