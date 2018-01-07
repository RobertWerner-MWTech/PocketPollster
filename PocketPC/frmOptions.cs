using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DataObjects;


namespace PocketPC
{
  // Define Aliases
  using CFSysInfo = DataObjects.CFSysInfo;
  using Platform = DataObjects.Constants.MobilePlatform;



	/// <summary>
	/// Summary description for frmOptions.
	/// </summary>
	public class frmOptions : System.Windows.Forms.Form
	{
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.MenuItem menuItemOK;
    private System.Windows.Forms.MenuItem menuItemCancel;
  
		public frmOptions()
		{
			InitializeComponent();
     
      this.Resize += new System.EventHandler(this.frmOptions_Resize);
      this.BringToFront();

      this.Resize += new System.EventHandler(this.frmOptions_Resize);
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmOptions));
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemOK = new System.Windows.Forms.MenuItem();
      this.menuItemCancel = new System.Windows.Forms.MenuItem();
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.Add(this.menuItemOK);
      this.mainMenu.MenuItems.Add(this.menuItemCancel);
      // 
      // menuItemOK
      // 
      this.menuItemOK.Text = "OK";
      // 
      // menuItemCancel
      // 
      this.menuItemCancel.Text = "Cancel";
      // 
      // frmOptions
      // 
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ControlBox = false;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";
    }
		#endregion


    private void frmOptions_Resize(object sender, System.EventArgs e)
    {
      RepositionControls();
    }


    private void RepositionControls()
    {
      int wid, hgt, aHgt;
      if (ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt))
      {
        //int gap = 4;   // A small value, used as the basis for providing horiz and vert spacing

        if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
        {
          // To Do: Still to need to flesh out SmartPhone positioning

        }

        else
        {
          if (wid < hgt)  // Is it in Portrait mode?
          {

          }

          else  // Landscape mode
          {

          }
        }
      }
    }





	}
}
