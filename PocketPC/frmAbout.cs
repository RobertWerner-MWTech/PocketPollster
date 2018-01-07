using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using DataObjects;



namespace PocketPC
{
  // Define Aliases
  using CFSysInfo = DataObjects.CFSysInfo;
  using ProductID = DataObjects.Constants.ProductID;
  using Platform = DataObjects.Constants.MobilePlatform;


	/// <summary>
	/// Summary description for frmAbout.
	/// </summary>
  public class frmAbout : System.Windows.Forms.Form
  {
    private System.Windows.Forms.Label labelVersion;
    private System.Windows.Forms.Label labelGuidIntro;
    private System.Windows.Forms.Label labelDataPathIntro;
    private System.Windows.Forms.Label labelGuid;
    private System.Windows.Forms.Label labelDataPath;
    private System.Windows.Forms.Label labelVersionIntro;
    private OpenNETCF.Windows.Forms.PictureBoxEx pictureAnimation;
    private System.Windows.Forms.Timer timerAnimation;
    private System.Windows.Forms.Panel panelInfo;
    private System.Windows.Forms.Label labelProdNameIntro;
    private System.Windows.Forms.Label labelProdName;
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.MenuItem menuItemOK;

  
    private int currentImageNumber = 0;   // The number representing which image (ie "frame") is being displayed in the animation


    public frmAbout()
    {
      InitializeComponent();
      PopulateInfoPanel();

      timerAnimation.Enabled = true;
      this.BringToFront();

      this.Resize += new System.EventHandler(this.frmAbout_Resize);
      this.ShowDialog();
      this.Resize -= new System.EventHandler(this.frmAbout_Resize);
    }


    /// <summary>
    /// Fills the central panel with various information about the application.
    /// </summary>
    private void PopulateInfoPanel()
    {
      string appName = CFSysInfo.Data.MobileAdmin.AppName + " ";
      string appProduct = Enum.ToObject(typeof(ProductID), CFSysInfo.Data.MobileAdmin.ProductID).ToString();
      if (appProduct == "")
        Debug.Fail("Unknown Product ID: " + CFSysInfo.Data.MobileAdmin.ProductID, "frmAbout.Constructor");
      
      labelProdName.Text = appName + appProduct;

      string appVersion = CFSysInfo.Data.MobileAdmin.VersionNumber;
      labelVersion.Text = appVersion;
      labelGuid.Text = RegistryCF.GetGuid();
      labelDataPath.Text = Tools.StripLastBackslash(CFSysInfo.Data.MobilePaths.Data);

      RepositionControls();
    }


    private void RepositionControls()
    {
      int wid, hgt, aHgt;
      if (ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt))
      {
        int gap = 4;   // A small value, used as the basis for providing horiz and vert spacing

        if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
        {
          // To Do: Still to need to flesh out SmartPhone positioning

        }

        else
        {
          int x = gap * 2;
          int y = x;

          pictureAnimation.Left = (wid - pictureAnimation.Width) / 2;

          if (hgt > wid)  // Is it in Portrait mode?
          {
            labelProdNameIntro.Location = new Point(x, y);
            labelProdName.Location = new Point(labelProdNameIntro.Right + 4, y);
          
            labelVersionIntro.Location = new Point(x, y + gap * 5);
            labelVersion.Location = new Point(labelProdName.Left, labelVersionIntro.Top);
          
            labelGuidIntro.Visible = true;
            labelGuid.Visible = true;

            labelGuidIntro.Location = new Point(x, labelVersion.Top + gap * 5);
            labelGuid.Location = new Point(x, labelGuidIntro.Top + gap * 4);
          
            labelDataPathIntro.Location = new Point(x, labelGuid.Top + gap * 5);
            labelDataPath.Location = new Point(x, labelDataPathIntro.Top + gap * 4);
          
            panelInfo.Size = new Size(Math.Max(labelGuid.Right + 4, labelDataPath.Right + 4), labelDataPath.Bottom + 4);
            panelInfo.Left = (wid - panelInfo.Width) / 2;
            panelInfo.Top = aHgt - gap * 4 - panelInfo.Height;
          }

          else  // Landscape mode
          {
            labelProdNameIntro.Location = new Point(x, y);
            labelProdName.Location = new Point(labelProdNameIntro.Right + 6, y);
          
            labelVersionIntro.Location = new Point(x, y + gap * 5);
            labelVersion.Location = new Point(labelProdName.Left, labelVersionIntro.Top);
          
            // No room for GUID
            labelGuidIntro.Visible = false;
            labelGuid.Visible = false;
         
            labelDataPathIntro.Location = new Point(x, labelVersion.Top + gap * 5);
            labelDataPath.Location = new Point(labelProdName.Left, labelDataPathIntro.Top);
       
            panelInfo.Size = new Size(labelDataPath.Right + 4, labelDataPath.Bottom + 4);
            panelInfo.Left = (wid - panelInfo.Width) / 2;
            panelInfo.Top = aHgt - gap - panelInfo.Height;
          }
        }
      }
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmAbout));
      this.panelInfo = new System.Windows.Forms.Panel();
      this.labelVersion = new System.Windows.Forms.Label();
      this.labelProdNameIntro = new System.Windows.Forms.Label();
      this.labelGuidIntro = new System.Windows.Forms.Label();
      this.labelDataPathIntro = new System.Windows.Forms.Label();
      this.labelGuid = new System.Windows.Forms.Label();
      this.labelDataPath = new System.Windows.Forms.Label();
      this.labelVersionIntro = new System.Windows.Forms.Label();
      this.labelProdName = new System.Windows.Forms.Label();
      this.pictureAnimation = new OpenNETCF.Windows.Forms.PictureBoxEx();
      this.timerAnimation = new System.Windows.Forms.Timer();
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemOK = new System.Windows.Forms.MenuItem();
      // 
      // panelInfo
      // 
      this.panelInfo.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(216)), ((System.Byte)(224)), ((System.Byte)(238)));
      this.panelInfo.Controls.Add(this.labelVersion);
      this.panelInfo.Controls.Add(this.labelProdNameIntro);
      this.panelInfo.Controls.Add(this.labelGuidIntro);
      this.panelInfo.Controls.Add(this.labelDataPathIntro);
      this.panelInfo.Controls.Add(this.labelGuid);
      this.panelInfo.Controls.Add(this.labelDataPath);
      this.panelInfo.Controls.Add(this.labelVersionIntro);
      this.panelInfo.Controls.Add(this.labelProdName);
      this.panelInfo.Location = new System.Drawing.Point(8, 112);
      this.panelInfo.Size = new System.Drawing.Size(224, 136);
      // 
      // labelVersion
      // 
      this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelVersion.ForeColor = System.Drawing.Color.Blue;
      this.labelVersion.Location = new System.Drawing.Point(60, 32);
      this.labelVersion.Size = new System.Drawing.Size(120, 16);
      this.labelVersion.Text = "..";
      // 
      // labelProdNameIntro
      // 
      this.labelProdNameIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelProdNameIntro.Location = new System.Drawing.Point(8, 8);
      this.labelProdNameIntro.Size = new System.Drawing.Size(50, 16);
      this.labelProdNameIntro.Text = "Product:";
      // 
      // labelGuidIntro
      // 
      this.labelGuidIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelGuidIntro.Location = new System.Drawing.Point(8, 56);
      this.labelGuidIntro.Size = new System.Drawing.Size(36, 16);
      this.labelGuidIntro.Text = "GUID:";
      // 
      // labelDataPathIntro
      // 
      this.labelDataPathIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelDataPathIntro.Location = new System.Drawing.Point(8, 96);
      this.labelDataPathIntro.Size = new System.Drawing.Size(58, 16);
      this.labelDataPathIntro.Text = "Data Files:";
      // 
      // labelGuid
      // 
      this.labelGuid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelGuid.ForeColor = System.Drawing.Color.Blue;
      this.labelGuid.Location = new System.Drawing.Point(8, 72);
      this.labelGuid.Size = new System.Drawing.Size(208, 16);
      this.labelGuid.Text = "...";
      // 
      // labelDataPath
      // 
      this.labelDataPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelDataPath.ForeColor = System.Drawing.Color.Blue;
      this.labelDataPath.Location = new System.Drawing.Point(8, 112);
      this.labelDataPath.Size = new System.Drawing.Size(208, 16);
      this.labelDataPath.Text = "...";
      // 
      // labelVersionIntro
      // 
      this.labelVersionIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelVersionIntro.Location = new System.Drawing.Point(8, 32);
      this.labelVersionIntro.Size = new System.Drawing.Size(50, 16);
      this.labelVersionIntro.Text = "Version:";
      // 
      // labelProdName
      // 
      this.labelProdName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelProdName.ForeColor = System.Drawing.Color.Blue;
      this.labelProdName.Location = new System.Drawing.Point(60, 8);
      this.labelProdName.Size = new System.Drawing.Size(120, 16);
      this.labelProdName.Text = "..";
      // 
      // pictureAnimation
      // 
      this.pictureAnimation.Location = new System.Drawing.Point(70, 8);
      this.pictureAnimation.Size = new System.Drawing.Size(100, 100);
      this.pictureAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.pictureAnimation.TransparentColor = System.Drawing.Color.White;
      // 
      // timerAnimation
      // 
      this.timerAnimation.Tick += new System.EventHandler(this.timerAnimation_Tick);
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.Add(this.menuItemOK);
      // 
      // menuItemOK
      // 
      this.menuItemOK.Text = "OK";
      this.menuItemOK.Click += new System.EventHandler(this.menuItemOK_Click);
      // 
      // frmAbout
      // 
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ControlBox = false;
      this.Controls.Add(this.pictureAnimation);
      this.Controls.Add(this.panelInfo);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";
      this.Load += new System.EventHandler(this.frmAbout_Load);
      this.Closed += new System.EventHandler(this.frmAbout_Closed);

    }
    #endregion



    private void frmAbout_Closed(object sender, System.EventArgs e)
    {
      timerAnimation.Enabled = false;
    }

    private void timerAnimation_Tick(object sender, System.EventArgs e)
    {
      currentImageNumber++;
      pictureAnimation.Image = Multimedia.Images.GetImage("Anim", ref currentImageNumber);
    }

    private void frmAbout_Load(object sender, System.EventArgs e)
    {
      pictureAnimation.Left = (pictureAnimation.Parent.Width - pictureAnimation.Width) / 2;
      pictureAnimation.Top = 8;
    }

    private void frmAbout_Resize(object sender, System.EventArgs e)
    {
      RepositionControls();
    }

    private void menuItemOK_Click(object sender, System.EventArgs e)
    {
      this.Close();
    }





  }
}
