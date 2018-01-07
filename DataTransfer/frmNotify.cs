using System;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using OpenNETCF.Desktop.Communication;

using DataObjects;


namespace DataTransfer
{
	/// <summary>
	/// This is the Notification form that appears in the lower right corner of the screen.
	/// It provides pertinent information to the user during the data transfer process.
	/// </summary>

	public class frmNotify : TransDialog
	{
    const string timeLabelPrefix = "Connect Time:";
    const string copyLabelMiddle = "of";
    const string copyLabelSuffix = "bytes copied";
    public int bottomMargin = 0;
    public int rightMargin = 0;

    private System.Windows.Forms.Label labelBattery;
    private System.Windows.Forms.Label labelTime;
    private System.Windows.Forms.Label labelStatus;
    private ProgressBarEx progressBar;
    private System.Windows.Forms.Label labelDeviceID;
    private System.Windows.Forms.Label labelLastSync;
    private System.Windows.Forms.Label labelUser;
    private System.Windows.Forms.Label labelUserName;


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		public frmNotify()
		{
      InitializeForm();
		}


    // Alternate constructor
    public frmNotify(int botMargin, int rgtMargin)
    {
      if (botMargin >= 0)
        bottomMargin = botMargin;

      if (rgtMargin >= 0)
        rightMargin = rgtMargin;

      InitializeForm();
    }

    private void InitializeForm()
    {
      InitializeComponent();
      InitializeLinearGradients();
    }


    private void frmNotify_Load(object sender, System.EventArgs e)
    {
      int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
      int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
      this.Left = screenWidth - this.Width - rightMargin;
      this.Top = screenHeight - this.Height - bottomMargin;
      this.Text = " " + SysInfo.Data.Admin.AppName + " Data Transfer";

      labelTime.Text = timeLabelPrefix + " 00:00";
      labelTime.Left = this.ClientRectangle.Width - labelBattery.Left - labelTime.Width;

      labelDeviceID.Location = new Point(labelDeviceID.Location.X, this.ClientRectangle.Height - (labelDeviceID.Height + 22));
      labelLastSync.Location = new Point(labelLastSync.Location.X, this.ClientRectangle.Height - (labelLastSync.Height + 22));

      // Ensure that this label is correctly positioned at the bottom of the form
      labelUser.Location = new Point(labelUser.Location.X, this.ClientRectangle.Height - (labelUser.Height + 4));
      labelUserName.Location = new Point(labelUserName.Location.X, this.ClientRectangle.Height - (labelUserName.Height + 4));
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
      this.labelBattery = new System.Windows.Forms.Label();
      this.labelTime = new System.Windows.Forms.Label();
      this.labelStatus = new System.Windows.Forms.Label();
      this.progressBar = new DataObjects.ProgressBarEx();
      this.labelDeviceID = new System.Windows.Forms.Label();
      this.labelLastSync = new System.Windows.Forms.Label();
      this.labelUser = new System.Windows.Forms.Label();
      this.labelUserName = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // labelBattery
      // 
      this.labelBattery.AutoSize = true;
      this.labelBattery.BackColor = System.Drawing.Color.Transparent;
      this.labelBattery.Location = new System.Drawing.Point(7, 13);
      this.labelBattery.Name = "labelBattery";
      this.labelBattery.Size = new System.Drawing.Size(0, 16);
      this.labelBattery.TabIndex = 0;
      // 
      // labelTime
      // 
      this.labelTime.AutoSize = true;
      this.labelTime.BackColor = System.Drawing.Color.Transparent;
      this.labelTime.Location = new System.Drawing.Point(256, 13);
      this.labelTime.Name = "labelTime";
      this.labelTime.Size = new System.Drawing.Size(0, 16);
      this.labelTime.TabIndex = 1;
      this.labelTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // labelStatus
      // 
      this.labelStatus.BackColor = System.Drawing.Color.Transparent;
      this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelStatus.ForeColor = System.Drawing.Color.Blue;
      this.labelStatus.Location = new System.Drawing.Point(16, 48);
      this.labelStatus.Name = "labelStatus";
      this.labelStatus.Size = new System.Drawing.Size(260, 20);
      this.labelStatus.TabIndex = 0;
      this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // progressBar
      // 
      this.progressBar.BackColor = System.Drawing.Color.Transparent;
      this.progressBar.DrawingColor = System.Drawing.Color.Blue;
      this.progressBar.Location = new System.Drawing.Point(26, 80);
      this.progressBar.Maximum = 100;
      this.progressBar.Minimum = 0;
      this.progressBar.Name = "progressBar";
      this.progressBar.PercentageMode = DataObjects.ProgressBarEx.PercentageDrawingMode.Center;
      this.progressBar.Size = new System.Drawing.Size(238, 20);
      this.progressBar.Step = 1;
      this.progressBar.TabIndex = 2;
      this.progressBar.Value = 0;
      // 
      // labelDeviceID
      // 
      this.labelDeviceID.AutoSize = true;
      this.labelDeviceID.BackColor = System.Drawing.Color.Transparent;
      this.labelDeviceID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelDeviceID.ForeColor = System.Drawing.Color.White;
      this.labelDeviceID.Location = new System.Drawing.Point(7, 128);
      this.labelDeviceID.Name = "labelDeviceID";
      this.labelDeviceID.Size = new System.Drawing.Size(81, 16);
      this.labelDeviceID.TabIndex = 6;
      this.labelDeviceID.Text = "Device ID #057";
      this.labelDeviceID.Visible = false;
      // 
      // labelLastSync
      // 
      this.labelLastSync.AutoSize = true;
      this.labelLastSync.BackColor = System.Drawing.Color.Transparent;
      this.labelLastSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelLastSync.ForeColor = System.Drawing.Color.White;
      this.labelLastSync.Location = new System.Drawing.Point(144, 128);
      this.labelLastSync.Name = "labelLastSync";
      this.labelLastSync.Size = new System.Drawing.Size(76, 16);
      this.labelLastSync.TabIndex = 6;
      this.labelLastSync.Text = "Last Synched:";
      this.labelLastSync.TextAlign = System.Drawing.ContentAlignment.TopRight;
      this.labelLastSync.Visible = false;
      // 
      // labelUser
      // 
      this.labelUser.AutoSize = true;
      this.labelUser.BackColor = System.Drawing.Color.Transparent;
      this.labelUser.ForeColor = System.Drawing.Color.LightBlue;
      this.labelUser.Location = new System.Drawing.Point(8, 152);
      this.labelUser.Name = "labelUser";
      this.labelUser.Size = new System.Drawing.Size(108, 16);
      this.labelUser.TabIndex = 7;
      this.labelUser.Text = "User: Robert Werner";
      this.labelUser.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      this.labelUser.Visible = false;
      // 
      // labelUserName
      // 
      this.labelUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.labelUserName.AutoSize = true;
      this.labelUserName.BackColor = System.Drawing.Color.Transparent;
      this.labelUserName.ForeColor = System.Drawing.Color.LightBlue;
      this.labelUserName.Location = new System.Drawing.Point(212, 152);
      this.labelUserName.Name = "labelUserName";
      this.labelUserName.Size = new System.Drawing.Size(43, 16);
      this.labelUserName.TabIndex = 7;
      this.labelUserName.Text = "UserID:";
      this.labelUserName.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      this.labelUserName.Visible = false;
      // 
      // frmNotify
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(294, 176);
      this.Controls.Add(this.labelUser);
      this.Controls.Add(this.labelDeviceID);
      this.Controls.Add(this.labelLastSync);
      this.Controls.Add(this.labelUserName);
      this.Controls.Add(this.labelTime);
      this.Controls.Add(this.labelBattery);
      this.Controls.Add(this.labelStatus);
      this.Controls.Add(this.progressBar);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimumSize = new System.Drawing.Size(200, 100);
      this.Name = "frmNotify";
      this.ShowInTaskbar = false;
      this.Text = "frmNotify";
      this.TopMost = true;
      this.Load += new System.EventHandler(this.frmNotify_Load);
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
      sender = sender as TransDialog;
      this.Invalidate();
    }
    
    protected void PaintClient(Object sender, PaintEventArgs e)
    {
      TransDialog td = sender as TransDialog;

      e.Graphics.Clip = new Region(td.ClientRectangle);
      //LinearGradientBrush lgb = new LinearGradientBrush(td.ClientRectangle, Color.LightBlue, Color.Navy, 90F, false);
      LinearGradientBrush lgb = new LinearGradientBrush(td.ClientRectangle, Color.LightBlue, Color.FromArgb(0,0,160), 90F, false);

      e.Graphics.FillRectangle(lgb, td.ClientRectangle);
      lgb.Dispose();
    }

    #endregion


    /// <summary>
    /// Is called every second and displays the data transfer connection time.  Also indirectly serves to
    /// inform the user that the data transfer process is still occuring.
    /// </summary>
    private delegate void ShowTimeInfoDelegate(System.DateTime startTime, System.DateTime currTime);
    public void ShowTimeInfo(System.DateTime startTime, System.DateTime currTime)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ShowTimeInfoDelegate(ShowTimeInfo), new object[] {startTime, currTime});
        return;
      }

      // Only reaches here when on UI thread
      System.TimeSpan timeDiff = currTime - startTime;

      string minutes = timeDiff.Minutes.ToString();
      if (minutes.Length == 1)
        minutes = "0" + minutes;

      string seconds = timeDiff.Seconds.ToString();
      if (seconds.Length == 1)
        seconds = "0" + seconds;

      labelTime.Text = timeLabelPrefix + " " + minutes + ":" + seconds;
    }


    /// <summary>
    /// Is called only every 30 seconds and displays the % Remaining of the primary battery.
    /// </summary>
    private delegate void UpdateBatteryLevelDelegate(byte acPower, byte batLife);
    public void UpdateBatteryLevel(byte acPower, byte batLife)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new UpdateBatteryLevelDelegate(UpdateBatteryLevel), new object[] {acPower, batLife});
        return;
      }

      // Only reaches here when on UI thread
      if (acPower == 1)
        labelBattery.Text = "AC Power";
      else
        labelBattery.Text = "Battery: " + batLife.ToString() + "%";
    }



    /// <summary>
    /// Is executed every second and updates the progress bar while a [large] file is being copied.
    /// </summary>
    private delegate void ShowCopyProgressDelegate(int percentCopied);
    public void ShowCopyProgress(int percentCopied)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ShowCopyProgressDelegate(ShowCopyProgress), new object[] {percentCopied});
        return;
      }

      // Only reaches here when on UI thread
      if (percentCopied == -1)                 // Note: -1 is passed when copying is complete or was interrupted
        progressBar.Visible = false;
      else
      {
        progressBar.Visible = true;
        progressBar.Value = percentCopied;

        if (progressBar.Value < 43)
        {
          progressBar.PercentageMode = ProgressBarEx.PercentageDrawingMode.Movable;
          progressBar.ForeColor = Color.Blue;
        }
        else if (progressBar.Value < 46)
        {
          progressBar.PercentageMode = ProgressBarEx.PercentageDrawingMode.Center;
          progressBar.ForeColor = Color.Blue;
        }
        else if (progressBar.Value < 55)
        {
          progressBar.PercentageMode = ProgressBarEx.PercentageDrawingMode.Center;
          progressBar.ForeColor = Color.LightSteelBlue;
        }
        else
        {
          progressBar.PercentageMode = ProgressBarEx.PercentageDrawingMode.Center;
          progressBar.ForeColor = Color.White;
        }
      }
    }



    /// <summary>
    /// Used to display a status message to the user about what's currently happening in the data transfer process.
    /// </summary>
    private delegate void ShowCurrentStatusDelegate(string text);
    public void ShowCurrentStatus(string text)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ShowCurrentStatusDelegate(ShowCurrentStatus), new object[] {text});
        return;
      }

      // Only reaches here when on UI thread
      labelStatus.Text = text;
    }



    /// <summary>
    /// Displays the Device.ID of the currently connected mobile device.
    /// </summary>
    private delegate void ShowDeviceIDDelegate(string id);
    public void ShowDeviceID(string id)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ShowDeviceIDDelegate(ShowDeviceID), new object[] {id});
        return;
      }

      // Only reaches here when on UI thread
      if (! labelDeviceID.Visible)
        if (id != "")
        {
          labelDeviceID.Text = "Device ID #" + id;
          labelDeviceID.Visible = true;
        }
    }



    /// <summary>
    /// Displays information about the last time the connected mobile device was synched.
    /// </summary>
    private delegate void ShowLastSyncDelegate(string lastSync);
    public void ShowLastSync(string lastSync)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ShowLastSyncDelegate(ShowLastSync), new object[] {lastSync});
        return;
      }

      // Only reaches here when on UI thread
      if (! labelLastSync.Visible)
        if (lastSync != "")
        {
          labelLastSync.Text = "Last Sync: " + lastSync;
          labelLastSync.Left = this.ClientRectangle.Width - labelDeviceID.Left - labelLastSync.Width;
          labelLastSync.Visible = true;
        }
    }



    /// <summary>
    /// Displays assorted User Info of the user of the currently connected mobile device.
    /// </summary>
    private delegate void ShowUserInfoDelegate(string userName, string firstName, string lastName);
    public void ShowUserInfo(string userName, string firstName, string lastName)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new ShowUserInfoDelegate(ShowUserInfo), new object[] {userName, firstName, lastName});
        return;
      }

      // Only reaches here when on UI thread
      if (! labelUser.Visible)
        if (userName != "")
        {
          labelUser.Text = "User: " + firstName + " " + lastName;
          labelUser.Visible = true;

          labelUserName.Text = "UserID: " + userName;
          labelUserName.Left = this.ClientRectangle.Width - labelUser.Left - labelUserName.Width;
          labelUserName.Visible = true;
        }
    }

	}
}
