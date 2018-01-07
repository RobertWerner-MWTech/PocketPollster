using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using DataObjects;
using Multimedia;


namespace Desktop
{
	/// <summary>
	/// Summary description for frmAbout.
	/// </summary>
	public class frmAbout : System.Windows.Forms.Form
	{
    private System.Windows.Forms.Label labelProductName;
    private System.Windows.Forms.Label labelProductID;
    private System.Windows.Forms.PictureBox pictureLogo;
    private System.Windows.Forms.Label labelVersionNumber;
    private System.Windows.Forms.Label labelCopyright;
    private System.Windows.Forms.Label labelGuid;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.Timer timer1;
    private System.ComponentModel.IContainer components;

    private double opacityIncrement = 0.05;



		public frmAbout(bool aboutMode)
		{
			InitializeComponent();

      int gap = 4;
      labelGuid.Text = "Computer ID: " + SysInfo.Data.Admin.Guid;

      labelProductName.Text = SysInfo.Data.Admin.AppName;
      labelProductName.Left = pictureLogo.Right + 20;
      labelProductID.Left = labelProductName.Right - 5;
      labelProductID.Text = SysInfo.Data.Admin.ProductID.ToString();

      labelCopyright.Text = "Copyright © " + DateTime.Now.Year.ToString() + " " + SysInfo.Data.Admin.CompanyName;      
      labelCopyright.Location = new Point(gap * 2, this.Height - gap * 2 - labelCopyright.Height);

      labelVersionNumber.Text = "Version " + SysInfo.Data.Admin.VersionNumber;
      labelVersionNumber.Location = new Point(this.Width - labelVersionNumber.Width - 10, labelCopyright.Top);

      if (aboutMode)
      {
        this.FormBorderStyle = FormBorderStyle.Fixed3D;
        this.Text = "About " + SysInfo.Data.Admin.AppName;
      }

      else  // Splash Screen Mode (currently not used)
      {
        this.FormBorderStyle = FormBorderStyle.Fixed3D;
        labelVersionNumber.Left = labelVersionNumber.Left + 10;
      }

      InitializeBackground();

      this.Opacity = .00;
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmAbout));
      this.labelProductName = new System.Windows.Forms.Label();
      this.labelProductID = new System.Windows.Forms.Label();
      this.pictureLogo = new System.Windows.Forms.PictureBox();
      this.labelVersionNumber = new System.Windows.Forms.Label();
      this.labelCopyright = new System.Windows.Forms.Label();
      this.labelGuid = new System.Windows.Forms.Label();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.SuspendLayout();
      // 
      // labelProductName
      // 
      this.labelProductName.AutoSize = true;
      this.labelProductName.BackColor = System.Drawing.Color.Transparent;
      this.labelProductName.Font = new System.Drawing.Font("Arial Black", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelProductName.ForeColor = System.Drawing.Color.RoyalBlue;
      this.labelProductName.Location = new System.Drawing.Point(163, 288);
      this.labelProductName.Name = "labelProductName";
      this.labelProductName.Size = new System.Drawing.Size(318, 56);
      this.labelProductName.TabIndex = 0;
      this.labelProductName.Tag = "";
      this.labelProductName.Text = "Product Name";
      this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelProductID
      // 
      this.labelProductID.AutoSize = true;
      this.labelProductID.BackColor = System.Drawing.Color.Transparent;
      this.labelProductID.Font = new System.Drawing.Font("Arial", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelProductID.ForeColor = System.Drawing.Color.Red;
      this.labelProductID.Location = new System.Drawing.Point(464, 288);
      this.labelProductID.Name = "labelProductID";
      this.labelProductID.Size = new System.Drawing.Size(111, 28);
      this.labelProductID.TabIndex = 1;
      this.labelProductID.Tag = "";
      this.labelProductID.Text = "ProductID";
      this.labelProductID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
      // 
      // pictureLogo
      // 
      this.pictureLogo.BackColor = System.Drawing.Color.Transparent;
      this.pictureLogo.Image = ((System.Drawing.Image)(resources.GetObject("pictureLogo.Image")));
      this.pictureLogo.Location = new System.Drawing.Point(48, 256);
      this.pictureLogo.Name = "pictureLogo";
      this.pictureLogo.Size = new System.Drawing.Size(100, 111);
      this.pictureLogo.TabIndex = 2;
      this.pictureLogo.TabStop = false;
      this.pictureLogo.Visible = false;
      // 
      // labelVersionNumber
      // 
      this.labelVersionNumber.AutoSize = true;
      this.labelVersionNumber.BackColor = System.Drawing.Color.Transparent;
      this.labelVersionNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelVersionNumber.ForeColor = System.Drawing.Color.White;
      this.labelVersionNumber.Location = new System.Drawing.Point(472, 390);
      this.labelVersionNumber.Name = "labelVersionNumber";
      this.labelVersionNumber.Size = new System.Drawing.Size(110, 18);
      this.labelVersionNumber.TabIndex = 3;
      this.labelVersionNumber.Text = "Version 1.0.0.157";
      this.labelVersionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // labelCopyright
      // 
      this.labelCopyright.AutoSize = true;
      this.labelCopyright.BackColor = System.Drawing.Color.Transparent;
      this.labelCopyright.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelCopyright.ForeColor = System.Drawing.Color.White;
      this.labelCopyright.Location = new System.Drawing.Point(176, 390);
      this.labelCopyright.Name = "labelCopyright";
      this.labelCopyright.Size = new System.Drawing.Size(62, 18);
      this.labelCopyright.TabIndex = 3;
      this.labelCopyright.Text = "Copyright";
      this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelGuid
      // 
      this.labelGuid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelGuid.BackColor = System.Drawing.Color.Transparent;
      this.labelGuid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelGuid.ForeColor = System.Drawing.Color.Yellow;
      this.labelGuid.Location = new System.Drawing.Point(207, 6);
      this.labelGuid.Name = "labelGuid";
      this.labelGuid.Size = new System.Drawing.Size(376, 24);
      this.labelGuid.TabIndex = 4;
      this.labelGuid.Text = "Computer ID:";
      this.labelGuid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // timer1
      // 
      this.timer1.Interval = 50;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // frmAbout
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = new System.Drawing.Size(590, 414);
      this.Controls.Add(this.labelGuid);
      this.Controls.Add(this.labelVersionNumber);
      this.Controls.Add(this.labelProductID);
      this.Controls.Add(this.labelProductName);
      this.Controls.Add(this.labelCopyright);
      this.Controls.Add(this.pictureLogo);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmAbout";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "About";
      this.TopMost = true;
      this.Click += new System.EventHandler(this.frmAbout_Click);
      this.Closing += new System.ComponentModel.CancelEventHandler(this.frmAbout_Closing);
      this.ResumeLayout(false);

    }
		#endregion



    #region BackgroundFill

    /// <summary>
    /// If you'd like to have linear gradients for the background of your MDI application then do the following:
    ///   1. Copy the following 3 methods into your parent form.
    ///   2. Call 'InitializeLinearGradients()' from your parent form's constructor.
    /// </summary>
    public void InitializeBackground()
    {
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
      try
      {
        Form frm = sender as Form;
        e.Graphics.Clip = new Region(frm.ClientRectangle);
        //
        //        LinearGradientBrush lgb = new LinearGradientBrush(frm.ClientRectangle, Color.LightBlue, Color.FromArgb(0,0,160), 90F, false);
        //        e.Graphics.FillRectangle(lgb, frm.ClientRectangle);
        //        lgb.Dispose();

        PictureBox pic = pictureLogo;
  
        if (pic.Image != null)
        {
          Rectangle rect = pic.ClientRectangle;
          Rectangle imgRect = new Rectangle(pic.Left, pic.Top, pic.Width, pic.Height);
  
          ImageAttributes imageAttributes = new ImageAttributes();
          imageAttributes.SetColorKey(Color.FromArgb(220,220,220), Color.White, ColorAdjustType.Bitmap);
  
          e.Graphics.DrawImage(pic.Image, imgRect, 0, 0, rect.Width, rect.Height, GraphicsUnit.Pixel, imageAttributes);
        }
      }

      catch (Exception ex)
      {
        Debug.Fail(ex.Message, "frmAbout.PaintClient");
      }
    }

    #endregion



    private void timer1_Tick(object sender, System.EventArgs e)
    {
      if (opacityIncrement > 0)
      {
        if(this.Opacity < 1)
          this.Opacity += opacityIncrement;
        else
          timer1.Stop();
      }
      else  // closing form
      {
        if (this.Opacity > 0)
          this.Opacity += opacityIncrement;
        else
        {
          timer1.Stop();
          this.Close();
          this.Dispose();
        }
      }
    }

    private void frmAbout_Click(object sender, System.EventArgs e)
    {
      opacityIncrement = - 0.08;
      timer1.Start();
    }

    private void frmAbout_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel = true;
      opacityIncrement = - 0.08;
      timer1.Start();
    }



	}
}
