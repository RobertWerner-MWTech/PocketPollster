using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;

using DataObjects;


namespace Desktop
{
	/// <summary>
	/// Summary description for SplashScreen
	/// </summary>
	public class frmSplash : System.Windows.Forms.Form
	{

    #region Variables

		// Threading
		public static frmSplash ms_frmSplash = null;
		static Thread ms_oThread = null;

		// Fade in and out.
		private double m_dblOpacityIncrement = .05;
		private double m_dblOpacityDecrement = .08;
		private const int TIMER_INTERVAL = 50;

		// Status and progress bar
		static string ms_sStatus;
		private double m_dblCompletionFraction = 0;
		private Rectangle m_rProgress;

		// Progress smoothing
		private double m_dblLastCompletionFraction = 0.0;
		private double m_dblPBIncrementPerTimerInterval = .015;

		// Self-calibration support
		private bool m_bFirstLaunch = false;
		private DateTime m_dtStart;
		private bool m_bDTSet = false;
		private int m_iIndex = 1;
		private int m_iActualTicks = 0;
		private ArrayList m_alPreviousCompletionFraction;
		private ArrayList m_alActualTimes = new ArrayList();
		private const string REG_KEY_INITIALIZATION = "Initialization";
		private const string REGVALUE_PB_MILISECOND_INCREMENT = "Increment";
		private const string REGVALUE_PB_PERCENTS = "Percents";

		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.Label lblTimeRemaining;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Panel pnlStatus;
    private System.Windows.Forms.Panel panelOuter;
    private PanelGradient.PanelGradient panelInner;
    private System.Windows.Forms.Label labelVersionNumber;
    private System.Windows.Forms.Label labelCopyright;
    private System.Windows.Forms.PictureBox pictureLogo;
    private System.Windows.Forms.Label labelOneLiner;
    private System.Windows.Forms.Label labelProductID;
    private System.Windows.Forms.Label labelProductName;
    private System.Windows.Forms.PictureBox pictureBox1;
		private System.ComponentModel.IContainer components;

    #endregion


		public frmSplash()
		{
			InitializeComponent();

      int gap = 4;
      panelInner.Location = new Point(gap, gap);
      panelInner.Size = new Size(this.Width - (int) (2.5 * gap), this.Height - (int) (2.5 * gap));

      labelProductName.Text = SysInfo.Data.Admin.AppName;
      labelProductName.Left = pictureLogo.Right + 35;
      labelProductID.Left = labelProductName.Right - 5;
      labelProductID.Text = SysInfo.Data.Admin.ProductID.ToString();
      labelOneLiner.Left = labelProductName.Left + 2;

      labelCopyright.Text = "Copyright © " + DateTime.Now.Year.ToString() + " " + SysInfo.Data.Admin.CompanyName;
      labelCopyright.Location = new Point(gap * 2, panelInner.Height - gap * 2 - labelCopyright.Height);

      labelVersionNumber.Text = "Version " + SysInfo.Data.Admin.VersionNumber;
      labelVersionNumber.Location = new Point(panelInner.Width - labelVersionNumber.Width - 10, labelCopyright.Top);

			this.Opacity = .00;
			timer1.Interval = TIMER_INTERVAL;
			timer1.Start();
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
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmSplash));
      this.lblStatus = new System.Windows.Forms.Label();
      this.pnlStatus = new System.Windows.Forms.Panel();
      this.lblTimeRemaining = new System.Windows.Forms.Label();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.panelOuter = new System.Windows.Forms.Panel();
      this.panelInner = new PanelGradient.PanelGradient();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.labelOneLiner = new System.Windows.Forms.Label();
      this.labelProductID = new System.Windows.Forms.Label();
      this.labelProductName = new System.Windows.Forms.Label();
      this.pictureLogo = new System.Windows.Forms.PictureBox();
      this.labelVersionNumber = new System.Windows.Forms.Label();
      this.labelCopyright = new System.Windows.Forms.Label();
      this.panelOuter.SuspendLayout();
      this.panelInner.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblStatus
      // 
      this.lblStatus.BackColor = System.Drawing.Color.Transparent;
      this.lblStatus.Location = new System.Drawing.Point(36, 538);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new System.Drawing.Size(279, 14);
      this.lblStatus.TabIndex = 0;
      // 
      // pnlStatus
      // 
      this.pnlStatus.BackColor = System.Drawing.Color.Transparent;
      this.pnlStatus.Location = new System.Drawing.Point(37, 523);
      this.pnlStatus.Name = "pnlStatus";
      this.pnlStatus.Size = new System.Drawing.Size(279, 24);
      this.pnlStatus.TabIndex = 1;
      this.pnlStatus.Visible = false;
      this.pnlStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlStatus_Paint);
      // 
      // lblTimeRemaining
      // 
      this.lblTimeRemaining.BackColor = System.Drawing.Color.Transparent;
      this.lblTimeRemaining.Location = new System.Drawing.Point(39, 574);
      this.lblTimeRemaining.Name = "lblTimeRemaining";
      this.lblTimeRemaining.Size = new System.Drawing.Size(279, 16);
      this.lblTimeRemaining.TabIndex = 2;
      this.lblTimeRemaining.Text = "Time remaining";
      this.lblTimeRemaining.Visible = false;
      // 
      // timer1
      // 
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // panelOuter
      // 
      this.panelOuter.BackColor = System.Drawing.Color.Black;
      this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panelOuter.Controls.Add(this.panelInner);
      this.panelOuter.Controls.Add(this.lblTimeRemaining);
      this.panelOuter.Controls.Add(this.lblStatus);
      this.panelOuter.Controls.Add(this.pnlStatus);
      this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelOuter.Location = new System.Drawing.Point(0, 0);
      this.panelOuter.Name = "panelOuter";
      this.panelOuter.Size = new System.Drawing.Size(740, 400);
      this.panelOuter.TabIndex = 10;
      // 
      // panelInner
      // 
      this.panelInner.Controls.Add(this.pictureBox1);
      this.panelInner.Controls.Add(this.labelOneLiner);
      this.panelInner.Controls.Add(this.labelProductID);
      this.panelInner.Controls.Add(this.labelProductName);
      this.panelInner.Controls.Add(this.pictureLogo);
      this.panelInner.Controls.Add(this.labelVersionNumber);
      this.panelInner.Controls.Add(this.labelCopyright);
      this.panelInner.GradientColorTwo = System.Drawing.Color.MediumBlue;
      this.panelInner.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelInner.Location = new System.Drawing.Point(5, 5);
      this.panelInner.Name = "panelInner";
      this.panelInner.Size = new System.Drawing.Size(730, 390);
      this.panelInner.TabIndex = 15;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(383, 112);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(160, 252);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 27;
      this.pictureBox1.TabStop = false;
      // 
      // labelOneLiner
      // 
      this.labelOneLiner.AutoSize = true;
      this.labelOneLiner.Font = new System.Drawing.Font("Tahoma", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelOneLiner.ForeColor = System.Drawing.Color.Yellow;
      this.labelOneLiner.Location = new System.Drawing.Point(293, 67);
      this.labelOneLiner.Name = "labelOneLiner";
      this.labelOneLiner.Size = new System.Drawing.Size(409, 28);
      this.labelOneLiner.TabIndex = 26;
      this.labelOneLiner.Text = "Powerful Data Collection Made Simple!";
      // 
      // labelProductID
      // 
      this.labelProductID.AutoSize = true;
      this.labelProductID.BackColor = System.Drawing.Color.Transparent;
      this.labelProductID.Font = new System.Drawing.Font("Arial", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelProductID.ForeColor = System.Drawing.Color.Red;
      this.labelProductID.Location = new System.Drawing.Point(598, 9);
      this.labelProductID.Name = "labelProductID";
      this.labelProductID.Size = new System.Drawing.Size(111, 28);
      this.labelProductID.TabIndex = 25;
      this.labelProductID.Tag = "";
      this.labelProductID.Text = "ProductID";
      this.labelProductID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // labelProductName
      // 
      this.labelProductName.AutoSize = true;
      this.labelProductName.Font = new System.Drawing.Font("Arial Black", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelProductName.ForeColor = System.Drawing.Color.RoyalBlue;
      this.labelProductName.Location = new System.Drawing.Point(288, 8);
      this.labelProductName.Name = "labelProductName";
      this.labelProductName.Size = new System.Drawing.Size(318, 56);
      this.labelProductName.TabIndex = 24;
      this.labelProductName.Tag = "";
      this.labelProductName.Text = "Product Name";
      this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // pictureLogo
      // 
      this.pictureLogo.BackColor = System.Drawing.Color.Transparent;
      this.pictureLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureLogo.Image")));
      this.pictureLogo.Location = new System.Drawing.Point(16, 9);
      this.pictureLogo.Name = "pictureLogo";
      this.pictureLogo.Size = new System.Drawing.Size(240, 266);
      this.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureLogo.TabIndex = 23;
      this.pictureLogo.TabStop = false;
      // 
      // labelVersionNumber
      // 
      this.labelVersionNumber.AutoSize = true;
      this.labelVersionNumber.BackColor = System.Drawing.Color.Transparent;
      this.labelVersionNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelVersionNumber.ForeColor = System.Drawing.Color.WhiteSmoke;
      this.labelVersionNumber.Location = new System.Drawing.Point(608, 365);
      this.labelVersionNumber.Name = "labelVersionNumber";
      this.labelVersionNumber.Size = new System.Drawing.Size(110, 18);
      this.labelVersionNumber.TabIndex = 22;
      this.labelVersionNumber.Text = "Version 1.0.0.157";
      this.labelVersionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // labelCopyright
      // 
      this.labelCopyright.AutoSize = true;
      this.labelCopyright.BackColor = System.Drawing.Color.Transparent;
      this.labelCopyright.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelCopyright.ForeColor = System.Drawing.Color.WhiteSmoke;
      this.labelCopyright.Location = new System.Drawing.Point(10, 362);
      this.labelCopyright.Name = "labelCopyright";
      this.labelCopyright.Size = new System.Drawing.Size(62, 18);
      this.labelCopyright.TabIndex = 21;
      this.labelCopyright.Text = "Copyright";
      this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // frmSplash
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(740, 400);
      this.Controls.Add(this.panelOuter);
      this.ForeColor = System.Drawing.Color.Black;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "frmSplash";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "SplashScreen";
      this.TopMost = true;
      this.Click += new System.EventHandler(this.frmSplash_Click);
      this.panelOuter.ResumeLayout(false);
      this.panelInner.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		#endregion



		// ************* Static Methods *************** //

		// A static method to create the thread and launch the SplashScreen.
		static public void ShowSplashScreen()
		{
			// Make sure it's only launched once.
			if( ms_frmSplash != null )
				return;

			ms_oThread = new Thread(new ThreadStart(frmSplash.ShowForm));
			ms_oThread.IsBackground = true;
			ms_oThread.ApartmentState = ApartmentState.STA;
			ms_oThread.Start();
		}


		// A property returning the splash screen instance
		static public frmSplash SplashForm 
		{
			get
			{
				return ms_frmSplash;
			} 
		}

		// A private entry point for the thread.
		static private void ShowForm()
		{
			ms_frmSplash = new frmSplash();
			Application.Run(ms_frmSplash);
		}

		// A static method to close the SplashScreen
		static public void CloseForm()
		{
			if( ms_frmSplash != null && ms_frmSplash.IsDisposed == false )
			{
				// Make it start going away.
				ms_frmSplash.m_dblOpacityIncrement = - ms_frmSplash.m_dblOpacityDecrement;
			}

  		ms_oThread = null;	// we don't need these any more.
			ms_frmSplash = null;
		}

		// A static method to set the status and update the reference.
		static public void SetStatus(string newStatus)
		{
			SetStatus(newStatus, true);
		}

		// A static method to set the status and optionally update the reference.
		// This is useful if you are in a section of code that has a variable
		// set of status string updates.  In that case, don't set the reference.
		static public void SetStatus(string newStatus, bool setReference)
		{
			ms_sStatus = newStatus;
			if( ms_frmSplash == null )
				return;
			if( setReference )
				ms_frmSplash.SetReferenceInternal();
		}

		// Static method called from the initializing application to 
		// give the splash screen reference points.  Not needed if
		// you are using a lot of status strings.
		static public void SetReferencePoint()
		{
			if( ms_frmSplash == null )
				return;
			ms_frmSplash.SetReferenceInternal();

		}

		// ************ Private methods ************

		// Internal method for setting reference points.
		private void SetReferenceInternal()
		{
			if( m_bDTSet == false )
			{
				m_bDTSet = true;
				m_dtStart = DateTime.Now;
				ReadIncrements();
			}
			double dblMilliseconds = ElapsedMilliSeconds();
			m_alActualTimes.Add(dblMilliseconds);
			m_dblLastCompletionFraction = m_dblCompletionFraction;
			if( m_alPreviousCompletionFraction != null && m_iIndex < m_alPreviousCompletionFraction.Count )
				m_dblCompletionFraction = (double)m_alPreviousCompletionFraction[m_iIndex++];
			else
				m_dblCompletionFraction = ( m_iIndex > 0 )? 1: 0;
		}


		// Utility function to return elapsed Milliseconds since the SplashScreen was launched.
		private double ElapsedMilliSeconds()
		{
			TimeSpan ts = DateTime.Now - m_dtStart;
			return ts.TotalMilliseconds;
		}


		// Function to read the checkpoint intervals from the previous invocation of the SplashScreen from the registry.
		private void ReadIncrements()
		{
			string sPBIncrementPerTimerInterval = RegistryAccess.GetStringRegistryValue( REGVALUE_PB_MILISECOND_INCREMENT, "0.0015");
			double dblResult;

			if( Double.TryParse(sPBIncrementPerTimerInterval, System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out dblResult) == true )
				m_dblPBIncrementPerTimerInterval = dblResult;
			else
				m_dblPBIncrementPerTimerInterval = .0015;

			string sPBPreviousPctComplete = RegistryAccess.GetStringRegistryValue( REGVALUE_PB_PERCENTS, "" );

			if( sPBPreviousPctComplete != "" )
			{
				string [] aTimes = sPBPreviousPctComplete.Split(null);
				m_alPreviousCompletionFraction = new ArrayList();

				for(int i = 0; i < aTimes.Length; i++ )
				{
					double dblVal;
					if( Double.TryParse(aTimes[i], System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.InvariantInfo, out dblVal) )
						m_alPreviousCompletionFraction.Add(dblVal);
					else
						m_alPreviousCompletionFraction.Add(1.0);
				}
			}
			else
			{
				m_bFirstLaunch = true;
				lblTimeRemaining.Text = "";
			}
		}

		// Method to store the intervals (in percent complete) from the current invocation of
		// the splash screen to the registry.
		private void StoreIncrements()
		{
			string sPercent = "";
			double dblElapsedMilliseconds = ElapsedMilliSeconds();
			for( int i = 0; i < m_alActualTimes.Count; i++ )
				sPercent += ((double)m_alActualTimes[i]/dblElapsedMilliseconds).ToString("0.####", System.Globalization.NumberFormatInfo.InvariantInfo) + " ";

			RegistryAccess.SetStringRegistryValue( REGVALUE_PB_PERCENTS, sPercent );

			m_dblPBIncrementPerTimerInterval = 1.0/(double)m_iActualTicks;
			RegistryAccess.SetStringRegistryValue( REGVALUE_PB_MILISECOND_INCREMENT, m_dblPBIncrementPerTimerInterval.ToString("#.000000", System.Globalization.NumberFormatInfo.InvariantInfo));
		}

		//********* Event Handlers ************

		// Tick Event handler for the Timer control.  Handles fade in and fade out.  Also handles the smoothed progress bar.
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			lblStatus.Text = ms_sStatus;

			if( m_dblOpacityIncrement > 0 )
			{
				m_iActualTicks++;
				if( this.Opacity < 1 )
					this.Opacity += m_dblOpacityIncrement;
			}
			else
			{
				if( this.Opacity > 0 )
					this.Opacity += m_dblOpacityIncrement;
				else
				{
					StoreIncrements();
					this.Close();
				}
			}
			if( m_bFirstLaunch == false && m_dblLastCompletionFraction < m_dblCompletionFraction )
			{
				m_dblLastCompletionFraction += m_dblPBIncrementPerTimerInterval;
				int width = (int)Math.Floor(pnlStatus.ClientRectangle.Width * m_dblLastCompletionFraction);
				int height = pnlStatus.ClientRectangle.Height;
				int x = pnlStatus.ClientRectangle.X;
				int y = pnlStatus.ClientRectangle.Y;
				if( width > 0 && height > 0 )
				{
					m_rProgress = new Rectangle( x, y, width, height);
					pnlStatus.Invalidate(m_rProgress);
					int iSecondsLeft = 1 + (int)(TIMER_INTERVAL * ((1.0 - m_dblLastCompletionFraction)/m_dblPBIncrementPerTimerInterval)) / 1000;
					if( iSecondsLeft == 1 )
						lblTimeRemaining.Text = string.Format( "1 second remaining");
					else
						lblTimeRemaining.Text = string.Format( "{0} seconds remaining", iSecondsLeft);

				}
			}
		}

		// Paint the portion of the panel invalidated during the tick event.
		private void pnlStatus_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if( m_bFirstLaunch == false && e.ClipRectangle.Width > 0 && m_iActualTicks > 1 )
			{
				LinearGradientBrush brBackground = new LinearGradientBrush(m_rProgress, Color.FromArgb(58, 96, 151), Color.FromArgb(181, 237, 254), LinearGradientMode.Horizontal);
				e.Graphics.FillRectangle(brBackground, m_rProgress);
			}
		}

		// Close the form if they double click on it.
		private void frmSplash_Click(object sender, System.EventArgs e)
		{
			CloseForm();
		}
	}



	/// <summary>
	/// A class for managing registry access.
	/// </summary>
	public class RegistryAccess
	{
		private const string SOFTWARE_KEY = "Software";
		private const string COMPANY_NAME = "MyCompany";
		private const string APPLICATION_NAME = "MyApplication";

		// Method for retrieving a Registry Value.
		static public string GetStringRegistryValue(string key, string defaultValue)
		{
			RegistryKey rkCompany;
			RegistryKey rkApplication;

			rkCompany = Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY, false).OpenSubKey(COMPANY_NAME, false);
			if( rkCompany != null )
			{
				rkApplication = rkCompany.OpenSubKey(APPLICATION_NAME, true);
				if( rkApplication != null )
				{
					foreach(string sKey in rkApplication.GetValueNames())
					{
						if( sKey == key )
						{
							return (string)rkApplication.GetValue(sKey);
						}
					}
				}
			}
			return defaultValue;
		}

		// Method for storing a Registry Value.
		static public void SetStringRegistryValue(string key, string stringValue)
		{
			RegistryKey rkSoftware;
			RegistryKey rkCompany;
			RegistryKey rkApplication;

			rkSoftware = Registry.CurrentUser.OpenSubKey(SOFTWARE_KEY, true);
			rkCompany = rkSoftware.CreateSubKey(COMPANY_NAME);
			if( rkCompany != null )
			{
				rkApplication = rkCompany.CreateSubKey(APPLICATION_NAME);
				if( rkApplication != null )
				{
					rkApplication.SetValue(key, stringValue);
				}
			}
		}
	}
}
