using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using OpenNETCF.Desktop.Communication;
using DataObjects;

namespace DataTransfer
{
  // Define aliases
  using InstallMode = DataObjects.Constants.InstallationMode;


	/// <summary>
	/// Summary description for frmInstallPrompt.
	/// </summary>
	public class frmInstallPrompt : System.Windows.Forms.Form
	{
    public bool AskNoMore = false;
    public string AppName;

    private bool oneTimeComboChange = false;
    
    
    
    private System.Windows.Forms.Button buttonYes;
    private System.Windows.Forms.Button buttonNo;
    private System.Windows.Forms.CheckBox checkBoxStopAsking;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label labelPlatform;
    private System.Windows.Forms.Label labelProcessor;
    private System.Windows.Forms.Label labelOSVersion;
    private System.Windows.Forms.CheckBox checkBoxAutoUpdate;
    private System.Windows.Forms.Label labelInstallPrompt;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.ComboBox comboAppLocation;
    private System.Windows.Forms.ComboBox comboDataLocation;
    private System.Windows.Forms.Label labelApp;
    private System.Windows.Forms.Label labelData;
    private System.Windows.Forms.Label labelPlatformTxt;
    private System.Windows.Forms.Label labelProcessorTxt;
    private System.Windows.Forms.Label labelOSVersionTxt;
    private System.Windows.Forms.Label labelCurrVersTxt;
    private System.Windows.Forms.Label labelNewVersTxt;
    private System.Windows.Forms.Label labelCurrentVersion;
    private System.Windows.Forms.Label labelNewVersion;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;



		public frmInstallPrompt()
		{
			InitializeComponent();
      InitializeLinearGradients();
      AppName = SysInfo.Data.Admin.AppName;
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmInstallPrompt));
      this.labelInstallPrompt = new System.Windows.Forms.Label();
      this.buttonYes = new System.Windows.Forms.Button();
      this.buttonNo = new System.Windows.Forms.Button();
      this.checkBoxStopAsking = new System.Windows.Forms.CheckBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.labelPlatformTxt = new System.Windows.Forms.Label();
      this.labelProcessorTxt = new System.Windows.Forms.Label();
      this.labelPlatform = new System.Windows.Forms.Label();
      this.labelProcessor = new System.Windows.Forms.Label();
      this.labelOSVersion = new System.Windows.Forms.Label();
      this.labelOSVersionTxt = new System.Windows.Forms.Label();
      this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
      this.buttonOK = new System.Windows.Forms.Button();
      this.comboAppLocation = new System.Windows.Forms.ComboBox();
      this.comboDataLocation = new System.Windows.Forms.ComboBox();
      this.labelApp = new System.Windows.Forms.Label();
      this.labelData = new System.Windows.Forms.Label();
      this.labelCurrVersTxt = new System.Windows.Forms.Label();
      this.labelNewVersTxt = new System.Windows.Forms.Label();
      this.labelCurrentVersion = new System.Windows.Forms.Label();
      this.labelNewVersion = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // labelInstallPrompt
      // 
      this.labelInstallPrompt.BackColor = System.Drawing.Color.Transparent;
      this.labelInstallPrompt.Location = new System.Drawing.Point(8, 10);
      this.labelInstallPrompt.Name = "labelInstallPrompt";
      this.labelInstallPrompt.Size = new System.Drawing.Size(368, 40);
      this.labelInstallPrompt.TabIndex = 0;
      this.labelInstallPrompt.Text = "msg";
      // 
      // buttonYes
      // 
      this.buttonYes.BackColor = System.Drawing.Color.Transparent;
      this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
      this.buttonYes.Location = new System.Drawing.Point(104, 67);
      this.buttonYes.Name = "buttonYes";
      this.buttonYes.Size = new System.Drawing.Size(70, 30);
      this.buttonYes.TabIndex = 1;
      this.buttonYes.Text = "Yes";
      // 
      // buttonNo
      // 
      this.buttonNo.BackColor = System.Drawing.Color.Transparent;
      this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
      this.buttonNo.Location = new System.Drawing.Point(216, 67);
      this.buttonNo.Name = "buttonNo";
      this.buttonNo.Size = new System.Drawing.Size(70, 30);
      this.buttonNo.TabIndex = 2;
      this.buttonNo.Text = "No";
      // 
      // checkBoxStopAsking
      // 
      this.checkBoxStopAsking.BackColor = System.Drawing.Color.Transparent;
      this.checkBoxStopAsking.Location = new System.Drawing.Point(9, 131);
      this.checkBoxStopAsking.Name = "checkBoxStopAsking";
      this.checkBoxStopAsking.Size = new System.Drawing.Size(367, 24);
      this.checkBoxStopAsking.TabIndex = 4;
      this.checkBoxStopAsking.Text = "Don\'t ask about installing on THIS mobile device again.";
      this.checkBoxStopAsking.CheckedChanged += new System.EventHandler(this.checkBoxStopAsking_CheckedChanged);
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.Transparent;
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Location = new System.Drawing.Point(387, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(1, 136);
      this.panel1.TabIndex = 4;
      // 
      // labelPlatformTxt
      // 
      this.labelPlatformTxt.AutoSize = true;
      this.labelPlatformTxt.BackColor = System.Drawing.Color.Transparent;
      this.labelPlatformTxt.Location = new System.Drawing.Point(400, 74);
      this.labelPlatformTxt.Name = "labelPlatformTxt";
      this.labelPlatformTxt.Size = new System.Drawing.Size(50, 16);
      this.labelPlatformTxt.TabIndex = 5;
      this.labelPlatformTxt.Text = "Platform:";
      // 
      // labelProcessorTxt
      // 
      this.labelProcessorTxt.AutoSize = true;
      this.labelProcessorTxt.BackColor = System.Drawing.Color.Transparent;
      this.labelProcessorTxt.Location = new System.Drawing.Point(400, 132);
      this.labelProcessorTxt.Name = "labelProcessorTxt";
      this.labelProcessorTxt.Size = new System.Drawing.Size(58, 16);
      this.labelProcessorTxt.TabIndex = 5;
      this.labelProcessorTxt.Text = "Processor:";
      // 
      // labelPlatform
      // 
      this.labelPlatform.AutoSize = true;
      this.labelPlatform.BackColor = System.Drawing.Color.Transparent;
      this.labelPlatform.Location = new System.Drawing.Point(484, 74);
      this.labelPlatform.Name = "labelPlatform";
      this.labelPlatform.Size = new System.Drawing.Size(14, 16);
      this.labelPlatform.TabIndex = 5;
      this.labelPlatform.Text = "...";
      // 
      // labelProcessor
      // 
      this.labelProcessor.AutoSize = true;
      this.labelProcessor.BackColor = System.Drawing.Color.Transparent;
      this.labelProcessor.Location = new System.Drawing.Point(484, 132);
      this.labelProcessor.Name = "labelProcessor";
      this.labelProcessor.Size = new System.Drawing.Size(14, 16);
      this.labelProcessor.TabIndex = 5;
      this.labelProcessor.Text = "...";
      // 
      // labelOSVersion
      // 
      this.labelOSVersion.AutoSize = true;
      this.labelOSVersion.BackColor = System.Drawing.Color.Transparent;
      this.labelOSVersion.Location = new System.Drawing.Point(484, 103);
      this.labelOSVersion.Name = "labelOSVersion";
      this.labelOSVersion.Size = new System.Drawing.Size(14, 16);
      this.labelOSVersion.TabIndex = 5;
      this.labelOSVersion.Text = "...";
      // 
      // labelOSVersionTxt
      // 
      this.labelOSVersionTxt.AutoSize = true;
      this.labelOSVersionTxt.BackColor = System.Drawing.Color.Transparent;
      this.labelOSVersionTxt.Location = new System.Drawing.Point(400, 103);
      this.labelOSVersionTxt.Name = "labelOSVersionTxt";
      this.labelOSVersionTxt.Size = new System.Drawing.Size(65, 16);
      this.labelOSVersionTxt.TabIndex = 5;
      this.labelOSVersionTxt.Text = "OS Version:";
      // 
      // checkBoxAutoUpdate
      // 
      this.checkBoxAutoUpdate.BackColor = System.Drawing.Color.Transparent;
      this.checkBoxAutoUpdate.Location = new System.Drawing.Point(9, 112);
      this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
      this.checkBoxAutoUpdate.Size = new System.Drawing.Size(367, 24);
      this.checkBoxAutoUpdate.TabIndex = 3;
      this.checkBoxAutoUpdate.Text = "Perform installations without prompting from now on.";
      this.checkBoxAutoUpdate.CheckedChanged += new System.EventHandler(this.checkBoxAutoUpdate_CheckedChanged);
      // 
      // buttonOK
      // 
      this.buttonOK.BackColor = System.Drawing.Color.Transparent;
      this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.buttonOK.Location = new System.Drawing.Point(136, 64);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(112, 32);
      this.buttonOK.TabIndex = 1;
      this.buttonOK.Text = "OK";
      this.buttonOK.Visible = false;
      // 
      // comboAppLocation
      // 
      this.comboAppLocation.BackColor = System.Drawing.Color.AliceBlue;
      this.comboAppLocation.Location = new System.Drawing.Point(438, 13);
      this.comboAppLocation.Name = "comboAppLocation";
      this.comboAppLocation.Size = new System.Drawing.Size(120, 21);
      this.comboAppLocation.TabIndex = 6;
      this.comboAppLocation.SelectedValueChanged += new System.EventHandler(this.comboAppLocation_SelectedValueChanged);
      // 
      // comboDataLocation
      // 
      this.comboDataLocation.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
      this.comboDataLocation.Location = new System.Drawing.Point(438, 41);
      this.comboDataLocation.Name = "comboDataLocation";
      this.comboDataLocation.Size = new System.Drawing.Size(120, 21);
      this.comboDataLocation.TabIndex = 6;
      this.comboDataLocation.SelectedValueChanged += new System.EventHandler(this.comboDataLocation_SelectedValueChanged);
      // 
      // labelApp
      // 
      this.labelApp.AutoSize = true;
      this.labelApp.BackColor = System.Drawing.Color.Transparent;
      this.labelApp.Location = new System.Drawing.Point(400, 16);
      this.labelApp.Name = "labelApp";
      this.labelApp.Size = new System.Drawing.Size(27, 16);
      this.labelApp.TabIndex = 5;
      this.labelApp.Text = "App:";
      // 
      // labelData
      // 
      this.labelData.AutoSize = true;
      this.labelData.BackColor = System.Drawing.Color.Transparent;
      this.labelData.Location = new System.Drawing.Point(400, 44);
      this.labelData.Name = "labelData";
      this.labelData.Size = new System.Drawing.Size(31, 16);
      this.labelData.TabIndex = 5;
      this.labelData.Text = "Data:";
      // 
      // labelCurrVersTxt
      // 
      this.labelCurrVersTxt.AutoSize = true;
      this.labelCurrVersTxt.BackColor = System.Drawing.Color.Transparent;
      this.labelCurrVersTxt.Location = new System.Drawing.Point(400, 16);
      this.labelCurrVersTxt.Name = "labelCurrVersTxt";
      this.labelCurrVersTxt.Size = new System.Drawing.Size(87, 16);
      this.labelCurrVersTxt.TabIndex = 7;
      this.labelCurrVersTxt.Text = "Current Version:";
      this.labelCurrVersTxt.Visible = false;
      // 
      // labelNewVersTxt
      // 
      this.labelNewVersTxt.AutoSize = true;
      this.labelNewVersTxt.BackColor = System.Drawing.Color.Transparent;
      this.labelNewVersTxt.Location = new System.Drawing.Point(400, 45);
      this.labelNewVersTxt.Name = "labelNewVersTxt";
      this.labelNewVersTxt.Size = new System.Drawing.Size(71, 16);
      this.labelNewVersTxt.TabIndex = 7;
      this.labelNewVersTxt.Text = "New Version:";
      this.labelNewVersTxt.Visible = false;
      // 
      // labelCurrentVersion
      // 
      this.labelCurrentVersion.AutoSize = true;
      this.labelCurrentVersion.BackColor = System.Drawing.Color.Transparent;
      this.labelCurrentVersion.Location = new System.Drawing.Point(484, 16);
      this.labelCurrentVersion.Name = "labelCurrentVersion";
      this.labelCurrentVersion.Size = new System.Drawing.Size(39, 16);
      this.labelCurrentVersion.TabIndex = 7;
      this.labelCurrentVersion.Text = "1.0.0.0";
      this.labelCurrentVersion.Visible = false;
      // 
      // labelNewVersion
      // 
      this.labelNewVersion.AutoSize = true;
      this.labelNewVersion.BackColor = System.Drawing.Color.Transparent;
      this.labelNewVersion.Location = new System.Drawing.Point(484, 45);
      this.labelNewVersion.Name = "labelNewVersion";
      this.labelNewVersion.Size = new System.Drawing.Size(39, 16);
      this.labelNewVersion.TabIndex = 7;
      this.labelNewVersion.Text = "1.0.0.1";
      this.labelNewVersion.Visible = false;
      // 
      // frmInstallPrompt
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(574, 156);
      this.Controls.Add(this.comboAppLocation);
      this.Controls.Add(this.labelPlatformTxt);
      this.Controls.Add(this.labelProcessorTxt);
      this.Controls.Add(this.labelPlatform);
      this.Controls.Add(this.labelProcessor);
      this.Controls.Add(this.labelOSVersion);
      this.Controls.Add(this.labelOSVersionTxt);
      this.Controls.Add(this.labelApp);
      this.Controls.Add(this.labelData);
      this.Controls.Add(this.labelCurrVersTxt);
      this.Controls.Add(this.labelNewVersTxt);
      this.Controls.Add(this.labelCurrentVersion);
      this.Controls.Add(this.labelNewVersion);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.checkBoxStopAsking);
      this.Controls.Add(this.buttonYes);
      this.Controls.Add(this.labelInstallPrompt);
      this.Controls.Add(this.buttonNo);
      this.Controls.Add(this.checkBoxAutoUpdate);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.comboDataLocation);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmInstallPrompt";
      this.Text = "Mobile Device Installation";
      this.TopMost = true;
      this.ResumeLayout(false);

    }
		#endregion



    #region BackgroundFill

    /// <summary>
    /// If you'd like to have linear gradients for the background of your MDI application then do the following:
    ///   1. Copy the following 3 methods into your parent form.
    ///   2. Call 'InitializeLinearGradients()' from your parent form's constructor.
    /// </summary>
    public void InitializeLinearGradients()
    {
      //      foreach (Control c in this.Controls)
      //      {
      //        if (c.GetType().Name == "MdiClient")
      //        {
      //          c.Paint += new System.Windows.Forms.PaintEventHandler(PaintClient);
      //          c.SizeChanged += new System.EventHandler(SizeClient);
      //        }
      //      }      

      this.Paint += new System.Windows.Forms.PaintEventHandler(PaintClient);
      this.SizeChanged += new System.EventHandler(SizeClient);
    }

    protected void SizeClient(Object sender, EventArgs e)
    {
      sender = sender as Form;
      this.Invalidate();
    }
    
    protected void PaintClient(Object sender, PaintEventArgs e)
    {
      Form frm = sender as Form;

      e.Graphics.Clip = new Region(frm.ClientRectangle);
      //LinearGradientBrush lgb = new LinearGradientBrush(frm.ClientRectangle, Color.FromArgb(255,255,255), Color.FromArgb(65,65,255), 90F, false);
      LinearGradientBrush lgb = new LinearGradientBrush(frm.ClientRectangle, Color.FromArgb(255,255,255), Color.LightBlue, 90F, false);

      e.Graphics.FillRectangle(lgb, frm.ClientRectangle);
      lgb.Dispose();
    }

    #endregion



    // Note: This is called from 'DataXfer.AskToInstall'
    public void SetMode(InstallMode mode)
    {
      SetMode(mode, null, null, null);
    }

    // Note: This is called from 'DataXfer.AskToInstall'
    public void SetMode(InstallMode mode, ArrayList locations)
    {
      SetMode(mode, locations, null, null);
    }

    // Note: This is called from 'DataXfer.AskToInstall'
    public void SetMode(InstallMode mode, string currVersion, string newVersion)
    {
      SetMode(mode, null, currVersion, newVersion);
    }

    private void SetMode(InstallMode mode, ArrayList locations, string currVersion, string newVersion)
    {
      switch (mode)
      {
        case InstallMode.New:
          labelInstallPrompt.Text = "The connected mobile device does not have " + AppName + " installed.";
          labelInstallPrompt.Text = labelInstallPrompt.Text + "\n\nWould you like to install it now?";
          break;

        case InstallMode.Update:
          labelInstallPrompt.Text = "A newer version of " + AppName + " is available for the connected mobile device.";
          labelInstallPrompt.Text = labelInstallPrompt.Text + "  Would you like to install it now?";

          labelCurrVersTxt.Visible = true;
          labelCurrentVersion.Visible = true;
          labelCurrentVersion.Text = (currVersion == null) ? "" : currVersion;
          
          labelNewVersTxt.Visible = true;
          labelNewVersion.Visible = true;
          labelNewVersion.Text = (newVersion == null) ? "" : newVersion;
          break;

        case InstallMode.Reinstall:
          labelInstallPrompt.Text = "An error has been detected with the current installation of " + AppName + " on the connected mobile device.";
          labelInstallPrompt.Text = labelInstallPrompt.Text + "  Would you like to reinstall it now?";
          break;

        case InstallMode.NotSupported:
          labelInstallPrompt.Text = "Sorry, but the processor of the connected mobile device is not supported by " + AppName + ".";
          buttonYes.Visible = false;
          buttonNo.Visible = false;
          buttonOK.Visible = true;
          checkBoxAutoUpdate.Visible = false;
          checkBoxStopAsking.Text = "Ignore this device in the future.";
          labelApp.Enabled = false;
          labelData.Enabled = false;
          comboAppLocation.Enabled = false;
          comboDataLocation.Enabled = false;
          break;
      }

      if (locations == null)
      {
        labelApp.Visible = false;
        comboAppLocation.Visible = false;
        labelData.Visible = false;
        comboDataLocation.Visible = false;
      }
      else
      {
        foreach(string folder in locations)
        {
          comboAppLocation.Items.Add(folder);
          if (folder == SysInfo.Data.Options.Mobile.AppInstallLocation)
            comboAppLocation.SelectedItem = folder;

          comboDataLocation.Items.Add(folder);
          if (folder == SysInfo.Data.Options.Mobile.DataInstallLocation)
            comboDataLocation.SelectedItem = folder;
        }
        
        if (comboAppLocation.SelectedIndex == -1)
          comboAppLocation.SelectedIndex = 0;

        if (comboDataLocation.SelectedIndex == -1)
          comboDataLocation.SelectedIndex = 0;
      }
    }



    // Note: This is called from 'DataXfer.AskToInstall'
    public void DisplayDeviceInfo(RAPI rapi, CEOSVERSIONINFO verInfo)
    {
      string txt;

      txt = verInfo.dwPlatformId.ToString();
      if (txt.IndexOf("PLATFORM") != -1)
      {
        txt = txt.Substring(txt.IndexOf("PLATFORM") + 8);
        if (txt.IndexOf("_") != -1)
          txt = txt.Substring(txt.IndexOf("_") + 1);
      }
      labelPlatform.Text = txt;
      
      txt = verInfo.dwMajorVersion + "." + verInfo.dwMinorVersion + "." + verInfo.dwBuildNumber;
      labelOSVersion.Text = txt;
      
      SYSTEM_INFO sysInfo = new SYSTEM_INFO();
      rapi.GetDeviceSystemInfo(out sysInfo);
      txt = sysInfo.dwProcessorType.ToString();

      if (txt.IndexOf("PROCESSOR") != -1)
      {
        txt = txt.Substring(txt.IndexOf("PROCESSOR") + 9);
        if (txt.IndexOf("_") != -1)
          txt = txt.Substring(txt.IndexOf("_") + 1);
      }
      labelProcessor.Text = txt;
    }


    // Called from DataXfer
    public void CenterForm(Form parent, FormWindowState windowState)
    {
      if (windowState == FormWindowState.Minimized || windowState == FormWindowState.Maximized)
      {
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
        this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
      }
      else  // Normal state
      {
        this.StartPosition = FormStartPosition.Manual;
        this.Left = parent.Left + (parent.Width - this.Width) / 2;
        this.Top = parent.Top + (parent.Height - this.Height) / 2;
      }
    }


    private void checkBoxAutoUpdate_CheckedChanged(object sender, System.EventArgs e)
    {
      SysInfo.Data.Options.Mobile.AutoUpdate = checkBoxAutoUpdate.Checked;
    }

    private void checkBoxStopAsking_CheckedChanged(object sender, System.EventArgs e)
    {
      AskNoMore = checkBoxStopAsking.Checked;
    }

    private void comboAppLocation_SelectedValueChanged(object sender, System.EventArgs e)
    {
      SysInfo.Data.Options.Mobile.AppInstallLocation = comboAppLocation.SelectedItem.ToString();

      if (this.Visible && (! oneTimeComboChange))
      {
        oneTimeComboChange = true;
        comboDataLocation.SelectedItem = comboAppLocation.SelectedItem;
      }
    }

    private void comboDataLocation_SelectedValueChanged(object sender, System.EventArgs e)
    {
      SysInfo.Data.Options.Mobile.DataInstallLocation = comboDataLocation.SelectedItem.ToString();

      if (this.Visible && (! oneTimeComboChange))
        oneTimeComboChange = true;
    }



	}
}
