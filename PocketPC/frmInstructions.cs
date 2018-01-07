using System;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using DataObjects;



namespace PocketPC
{
  // Define Aliases
  using InstructType = DataObjects.Constants.InstructionsType;



	/// <summary>
	/// Summary description for frmInstructions.
	/// </summary>
	public class frmInstructions : System.Windows.Forms.Form
	{
    private System.Windows.Forms.Label labelIntro;
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.MenuItem menuItemBack;
    private System.Windows.Forms.MenuItem menuItemNext;
    private System.Windows.Forms.TextBox textBoxMain;

    public int ScreenAdv;    // -1 : Move backward one screen   +1 : Move forward one screen

  

		public frmInstructions(string text, InstructType mode, ref int screenAdv, ref bool backAvail)
		{
      Cursor.Current = Cursors.WaitCursor;
			InitializeComponent();
      InitializeScreen();

      SetIntroText(mode, backAvail);
      textBoxMain.Text = text;

      ToolsCF.SipShowIM(0);
      this.BringToFront();
      Cursor.Current = Cursors.Default;
      this.Resize += new System.EventHandler(this.frmInstructions_Resize);

      this.ShowDialog();
      
      screenAdv = ScreenAdv;
      backAvail = true;    // Setting this to true will make this just displayed screen the "back one"

      this.Resize -= new System.EventHandler(this.frmInstructions_Resize);
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmInstructions));
      this.textBoxMain = new System.Windows.Forms.TextBox();
      this.labelIntro = new System.Windows.Forms.Label();
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemBack = new System.Windows.Forms.MenuItem();
      this.menuItemNext = new System.Windows.Forms.MenuItem();
      // 
      // textBoxMain
      // 
      this.textBoxMain.BackColor = System.Drawing.Color.White;
      this.textBoxMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.textBoxMain.Location = new System.Drawing.Point(8, 32);
      this.textBoxMain.Multiline = true;
      this.textBoxMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxMain.Size = new System.Drawing.Size(224, 232);
      this.textBoxMain.Text = "";
      this.textBoxMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMain_KeyPress);
      // 
      // labelIntro
      // 
      this.labelIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelIntro.ForeColor = System.Drawing.Color.Blue;
      this.labelIntro.Location = new System.Drawing.Point(4, 8);
      this.labelIntro.Size = new System.Drawing.Size(236, 16);
      this.labelIntro.Text = "Intro";
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.Add(this.menuItemBack);
      this.mainMenu.MenuItems.Add(this.menuItemNext);
      // 
      // menuItemBack
      // 
      this.menuItemBack.Text = "< Back";
      this.menuItemBack.Click += new System.EventHandler(this.menuItemBack_Click);
      // 
      // menuItemNext
      // 
      this.menuItemNext.Text = "Next >";
      this.menuItemNext.Click += new System.EventHandler(this.menuItemNext_Click);
      // 
      // frmInstructions
      // 
      this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(200)), ((System.Byte)(200)));
      this.ControlBox = false;
      this.Controls.Add(this.labelIntro);
      this.Controls.Add(this.textBoxMain);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";
    }
		#endregion


    private void SetIntroText(InstructType mode, bool backAvail)
    {
      string msg = "";
      Color color = Color.White;

      switch (mode)
      {
        case InstructType.BeforePoll:
          msg = "Private instructions before poll:";
          color = Color.LightSteelBlue;
          menuItemBack.Enabled = false;    // This first screen always has the Back button disabled
          break;

        case InstructType.BeginMessage:
          msg = "Please read the following to the respondent:";
          color = Color.FromArgb(255, 200, 200);
          menuItemBack.Enabled = backAvail;
          break;

        case InstructType.AfterPoll:
          msg = "Private instructions after poll:";
          color = Color.Thistle;
          menuItemBack.Enabled = backAvail;
          break;

        case InstructType.EndMessage:
          msg = "Please read the following to the respondent:";
          color = Color.FromArgb(142, 218, 192);
          menuItemBack.Enabled = backAvail;
          break;

        case InstructType.AfterAllPolls:
          msg = "Private instructions after all respondents:";
          color = Color.FromArgb(255, 189, 123);
          menuItemBack.Enabled = false;    // This last screen always has the Back button disabled (this is redundant because backAvail is always false for this screen)
          break;

        default:
          Debug.Fail("Unknown Instructions Type: " + mode.ToString(), "frmInstructions.SetIntroText");
          break;
      }

      labelIntro.Text = msg;
      this.BackColor = color;
      labelIntro.BackColor = color;
    }


    private void textBoxMain_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
    {
      e.Handled = true;  // This will cancel all characters entered into the textbox, thus effectively making it read-only
    }


    private void InitializeScreen()
    {
      RepositionControls(true);
    }


    private void frmInstructions_Resize(object sender, System.EventArgs e)
    {
      RepositionControls(false);
    }


    private void RepositionControls(bool forceRedraw)
    {
      int wid = 0, hgt = 0, aHgt = 0;

      if ((ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt)) | forceRedraw)
      {
        int gap = 4;   // A small value, used as the basis for providing horiz and vert spacing

        labelIntro.Location = new Point(gap * 2, gap * 2);
        labelIntro.Size = new Size(wid - gap * 4, labelIntro.Height);

        textBoxMain.Location = new Point(gap * 2, gap * 8);
        textBoxMain.Size = new Size(wid - gap * 4, aHgt - gap * 12);
      }
    }


    private void menuItemBack_Click(object sender, System.EventArgs e)
    {
      ScreenAdv = -1;
      this.Close();
    }


    private void menuItemNext_Click(object sender, System.EventArgs e)
    {
      ScreenAdv = 1;
      this.Close();
    }
	}
}
