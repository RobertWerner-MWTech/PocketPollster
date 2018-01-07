using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using OpenNETCF.Win32;
using DataObjects;



namespace PocketPC
{

  // Define Aliases
  using CFSysInfo = DataObjects.CFSysInfo;
  using Platform = DataObjects.Constants.MobilePlatform;




	/// <summary>
	/// Summary description for frmGetNameInfo.
	/// </summary>
	public class frmGetNameInfo : System.Windows.Forms.Form
	{
    private System.Windows.Forms.Panel panelOuter;
    private System.Windows.Forms.Panel panelInner;
    private System.Windows.Forms.Label labelUserNameIntro;
    private System.Windows.Forms.Label labelUserName;
    private System.Windows.Forms.Label labelFirstName;
    private System.Windows.Forms.TextBox textBoxFirstName;
    private System.Windows.Forms.Label labelIntro;
    private System.Windows.Forms.TextBox textBoxLastName;
    private System.Windows.Forms.Panel panelUserName;
    private System.Windows.Forms.Label labelLastName;
    private System.Windows.Forms.MainMenu mainMenu1;
    private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
    private OpenNETCF.Windows.Forms.ButtonEx buttonOK;
 
    private bool SIPvisible;   // Set true when the SIP is visible



		public frmGetNameInfo()
		{
      Cursor.Current = Cursors.WaitCursor;
			InitializeComponent();
      InitializeScreen();

      if (CFSysInfo.Data.MobileOptions.FirstName != "")
        textBoxFirstName.Text = CFSysInfo.Data.MobileOptions.FirstName;

      if (CFSysInfo.Data.MobileOptions.LastName != "")
        textBoxLastName.Text = CFSysInfo.Data.MobileOptions.LastName;

      if (CFSysInfo.Data.MobileOptions.PrimaryUser != "")
        labelUserName.Text = CFSysInfo.Data.MobileOptions.PrimaryUser;

      if (CFSysInfo.Data.MobileOptions.AllowNameEditing)
      {
        textBoxFirstName.Focus();
        //SIPvisible = true;
        //ShowSIP(SIPvisible);  // Show keyboard before displaying dialog box
      }
      else
      {
//        Tools.ShowMessage("Sorry, but you are not currently allowed to change this information", "User Info Locked");
//        CloseForm();
      }

      this.Resize += new System.EventHandler(this.frmGetNameInfo_Resize);
      Cursor.Current = Cursors.Default;
      this.BringToFront();
      this.Resize -= new System.EventHandler(this.frmGetNameInfo_Resize);
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
      this.panelOuter = new System.Windows.Forms.Panel();
      this.panelInner = new System.Windows.Forms.Panel();
      this.buttonOK = new OpenNETCF.Windows.Forms.ButtonEx();
      this.panelUserName = new System.Windows.Forms.Panel();
      this.labelUserNameIntro = new System.Windows.Forms.Label();
      this.labelUserName = new System.Windows.Forms.Label();
      this.labelFirstName = new System.Windows.Forms.Label();
      this.textBoxFirstName = new System.Windows.Forms.TextBox();
      this.labelIntro = new System.Windows.Forms.Label();
      this.textBoxLastName = new System.Windows.Forms.TextBox();
      this.labelLastName = new System.Windows.Forms.Label();
      this.mainMenu1 = new System.Windows.Forms.MainMenu();
      this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
      // 
      // panelOuter
      // 
      this.panelOuter.BackColor = System.Drawing.Color.Black;
      this.panelOuter.Controls.Add(this.panelInner);
      this.panelOuter.Size = new System.Drawing.Size(176, 176);
      // 
      // panelInner
      // 
      this.panelInner.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(218)), ((System.Byte)(228)), ((System.Byte)(248)));
      this.panelInner.Controls.Add(this.buttonOK);
      this.panelInner.Controls.Add(this.panelUserName);
      this.panelInner.Controls.Add(this.labelFirstName);
      this.panelInner.Controls.Add(this.textBoxFirstName);
      this.panelInner.Controls.Add(this.labelIntro);
      this.panelInner.Controls.Add(this.textBoxLastName);
      this.panelInner.Controls.Add(this.labelLastName);
      this.panelInner.Location = new System.Drawing.Point(2, 2);
      this.panelInner.Size = new System.Drawing.Size(174, 174);
      // 
      // buttonOK
      // 
      this.buttonOK.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.buttonOK.Location = new System.Drawing.Point(112, 144);
      this.buttonOK.Size = new System.Drawing.Size(56, 24);
      this.buttonOK.Text = "OK";
      this.buttonOK.TextAlign = OpenNETCF.Drawing.ContentAlignment.MiddleCenter;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // panelUserName
      // 
      this.panelUserName.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(215)), ((System.Byte)(225)), ((System.Byte)(251)));
      this.panelUserName.Controls.Add(this.labelUserNameIntro);
      this.panelUserName.Controls.Add(this.labelUserName);
      this.panelUserName.Location = new System.Drawing.Point(6, 107);
      this.panelUserName.Size = new System.Drawing.Size(161, 28);
      // 
      // labelUserNameIntro
      // 
      this.labelUserNameIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelUserNameIntro.ForeColor = System.Drawing.Color.Black;
      this.labelUserNameIntro.Location = new System.Drawing.Point(3, 6);
      this.labelUserNameIntro.Size = new System.Drawing.Size(70, 16);
      this.labelUserNameIntro.Text = "User Name:";
      // 
      // labelUserName
      // 
      this.labelUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelUserName.ForeColor = System.Drawing.Color.Firebrick;
      this.labelUserName.Location = new System.Drawing.Point(79, 6);
      this.labelUserName.Size = new System.Drawing.Size(72, 16);
      this.labelUserName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // labelFirstName
      // 
      this.labelFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelFirstName.ForeColor = System.Drawing.Color.Blue;
      this.labelFirstName.Location = new System.Drawing.Point(6, 43);
      this.labelFirstName.Size = new System.Drawing.Size(63, 16);
      this.labelFirstName.Text = "First Name:";
      // 
      // textBoxFirstName
      // 
      this.textBoxFirstName.BackColor = System.Drawing.SystemColors.Control;
      this.textBoxFirstName.Location = new System.Drawing.Point(76, 40);
      this.textBoxFirstName.Size = new System.Drawing.Size(90, 22);
      this.textBoxFirstName.Text = "";
      this.textBoxFirstName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxName_KeyPress);
      this.textBoxFirstName.TextChanged += new System.EventHandler(this.textBoxFirstName_TextChanged);
      // 
      // labelIntro
      // 
      this.labelIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelIntro.ForeColor = System.Drawing.Color.Blue;
      this.labelIntro.Location = new System.Drawing.Point(6, 5);
      this.labelIntro.Size = new System.Drawing.Size(164, 28);
      this.labelIntro.Text = "Please identify the primary user of this device:";
      // 
      // textBoxLastName
      // 
      this.textBoxLastName.BackColor = System.Drawing.SystemColors.Control;
      this.textBoxLastName.Location = new System.Drawing.Point(76, 72);
      this.textBoxLastName.Size = new System.Drawing.Size(90, 22);
      this.textBoxLastName.Text = "";
      this.textBoxLastName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxName_KeyPress);
      this.textBoxLastName.TextChanged += new System.EventHandler(this.textBoxLastName_TextChanged);
      // 
      // labelLastName
      // 
      this.labelLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelLastName.ForeColor = System.Drawing.Color.Blue;
      this.labelLastName.Location = new System.Drawing.Point(6, 75);
      this.labelLastName.Size = new System.Drawing.Size(63, 16);
      this.labelLastName.Text = "Last Name:";
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      // 
      // frmGetNameInfo
      // 
      this.ClientSize = new System.Drawing.Size(178, 178);
      this.ControlBox = false;
      this.Controls.Add(this.panelOuter);
      this.MaximizeBox = false;
      this.Menu = this.mainMenu1;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";
    }
		#endregion


    private void buttonOK_Click(object sender, System.EventArgs e)
    {
      string msg = "";

      if (labelUserName.Text == "")
      {
        msg = "Are you sure you don't want to enter your name right now?\n\n(Note: You'll be able to enter it later but you can't transfer files until you've done so)";
        if (Tools.ShowMessage(msg, "No Name Entered", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button2) == DialogResult.No)
          return;
      }
      else
      {
        msg = "\"" + Tools.ProperCase(textBoxFirstName.Text) + " " + Tools.ProperCase(textBoxLastName.Text) + "\"";
        msg = msg.PadLeft(16);
        msg = "Are you absolutely sure you've entered your name correctly?\n\n" + msg;
        DialogResult retval = Tools.ShowMessage(msg, "Review Name Entry", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

        if (retval == DialogResult.No)  
          return;

        else if (retval == DialogResult.Yes)
          // Reaching here, the user has successfully entered their first & last name, from which a username
          // has been created.  We will now store all of this information into the CFSysInfo object.
          SaveUserInfo(textBoxFirstName.Text, textBoxLastName.Text, labelUserName.Text);
      }

      CloseForm();
    }


    /// <summary>
    /// Stores the captured (and calculated) info into the CFSysInfo object.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="userName"></param>
    private void SaveUserInfo(string firstName, string lastName, string userName)
    {
      CFSysInfo.Data.MobileOptions.FirstName = Tools.ProperCase(firstName);
      CFSysInfo.Data.MobileOptions.LastName = Tools.ProperCase(lastName);
      CFSysInfo.Data.MobileOptions.PrimaryUser = userName.ToLower();


      // Debug: Everything seems to operate fine without this next property.
      
      // This next line is necessary for those circumstances when the user changes their
      // user info after it has been confirmed by the Desktop as being fine.  For example,
      // perhaps a user will change their name from "Peter Smith" to "Pete Smith" or perhaps
      // they'll see a spelling mistake with their last name as they originally entered it.
      //CFSysInfo.Data.MobileOptions.UserInfoConfirmed = false;
    }


    private void CloseForm()
    {
      ShowSIP(false);   // Ensure kybd is hidden before leaving
      this.Close();
    }




    private void textBoxFirstName_TextChanged(object sender, System.EventArgs e)
    {
      textBoxFirstName.Text = textBoxFirstName.Text;
      BuildUserName();
    }

    private void textBoxLastName_TextChanged(object sender, System.EventArgs e)
    {
      textBoxLastName.Text = textBoxLastName.Text;
      BuildUserName();
    }

    private void BuildUserName()
    {
      string firstName = textBoxFirstName.Text.ToLower();
      string lastName = textBoxLastName.Text.ToLower();

      if (firstName != "" && lastName != "")
        labelUserName.Text = firstName.Substring(0,1) + lastName;  // ex. "rwerner"
      else
        labelUserName.Text = "";
    }

    private void textBoxName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
    {
      char c = e.KeyChar;
      int i = (int) c;

      // Only allow: Backspace, UpperCase letters, and LowerCase letters
      if (i == 8 || (i >= 65 & i <= 90) || (i >= 97 & i <= 122))
      {
        // Do nothing because the character is valid
      }
      else
      {
        e.Handled = true;  // This will cancel the character input
      }
    }


    // Note: This button is currently hidden but we'll leave the code in for now
    private void buttonSIP_Click(object sender, System.EventArgs e)
    {
      SIPvisible = ! SIPvisible;
      ShowSIP(SIPvisible);
    }


    public void ShowSIP(bool show)
    {
      ToolsCF.SipShowIM((show == true) ? 1 : 0);

    }


    private void InitializeScreen()
    {
      RepositionControls();
    }


    private void frmGetNameInfo_Resize(object sender, System.EventArgs e)
    {
      RepositionControls();
    }


    private void RepositionControls()
    {
      int wid, hgt, aHgt;
      ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt);
    
      int gap = 4;   // A small value, used as the basis for providing horiz and vert spacing

      panelOuter.Location = new Point(0, 0);
      panelOuter.Size = this.Size;
      
      panelInner.Width = panelOuter.Width - gap;
      panelInner.Left = gap / 2;
      panelInner.Height = panelOuter.Height - gap;
      panelInner.Top = gap / 2;

      this.Left = (wid - this.Width) / 2;

      if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
      {   // Debug: SmartPhone code still to be fleshed out
        this.Top = (aHgt - this.Height) / 2;
      }
      else
      {
        if (wid < hgt)  // Is it in Portrait mode?
          this.Top = (aHgt - this.Height) / 2 + 18;  // Designed to cover up the large PP icon in the background
        else   // Landscape mode
          this.Top = 30;
      }

    }








	}
}
