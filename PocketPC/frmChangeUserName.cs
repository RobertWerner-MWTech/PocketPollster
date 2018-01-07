using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DataObjects;


namespace PocketPC
{
  // Define Aliases
  using Platform = DataObjects.Constants.MobilePlatform;



	/// <summary>
	/// Summary description for frmChangeUserName.
	/// </summary>
	public class frmChangeUserName : System.Windows.Forms.Form
	{
    private System.Windows.Forms.MenuItem menuItemBack;
    private System.Windows.Forms.MenuItem menuItemNext;
    private System.Windows.Forms.Panel panelUserName;
    private System.Windows.Forms.Label labelUserNameIntro;
    private System.Windows.Forms.Label labelUserName;
    private System.Windows.Forms.Label labelFirstName;
    private System.Windows.Forms.TextBox textBoxFirstName;
    private System.Windows.Forms.Label labelIntro;
    private System.Windows.Forms.TextBox textBoxLastName;
    private System.Windows.Forms.Label labelLastName;
    private System.Windows.Forms.MainMenu mainMenu1;
    private System.Windows.Forms.Label labelCurrentUser;
    private System.Windows.Forms.Label labelCurrentUserIntro;
  
    public int ScreenAdv;    // -1 : Move backward one screen   +1 : Move forward one screen



    public frmChangeUserName(ref int screenAdv, ref bool backAvail)
    {
      Cursor.Current = Cursors.WaitCursor;
      InitializeComponent();
      InitializeScreen();

      // Display the current user's name near the top of the screen
      labelCurrentUser.Text = CFSysInfo.Data.MobileOptions.FirstName + " " + CFSysInfo.Data.MobileOptions.LastName;

      ToolsCF.SipShowIM(0);
      this.BringToFront();
      Cursor.Current = Cursors.Default;
      this.Resize += new System.EventHandler(this.frmChangeUserName_Resize);

      this.ShowDialog();
      
      screenAdv = ScreenAdv;
      backAvail = true;    // Setting this to true will make this just displayed screen the "back one"

      this.Resize -= new System.EventHandler(this.frmChangeUserName_Resize);
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
      this.mainMenu1 = new System.Windows.Forms.MainMenu();
      this.menuItemBack = new System.Windows.Forms.MenuItem();
      this.menuItemNext = new System.Windows.Forms.MenuItem();
      this.panelUserName = new System.Windows.Forms.Panel();
      this.labelUserNameIntro = new System.Windows.Forms.Label();
      this.labelUserName = new System.Windows.Forms.Label();
      this.labelFirstName = new System.Windows.Forms.Label();
      this.textBoxFirstName = new System.Windows.Forms.TextBox();
      this.labelIntro = new System.Windows.Forms.Label();
      this.textBoxLastName = new System.Windows.Forms.TextBox();
      this.labelLastName = new System.Windows.Forms.Label();
      this.labelCurrentUser = new System.Windows.Forms.Label();
      this.labelCurrentUserIntro = new System.Windows.Forms.Label();
      // 
      // mainMenu1
      // 
      this.mainMenu1.MenuItems.Add(this.menuItemBack);
      this.mainMenu1.MenuItems.Add(this.menuItemNext);
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
      // panelUserName
      // 
      this.panelUserName.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(215)), ((System.Byte)(225)), ((System.Byte)(251)));
      this.panelUserName.Controls.Add(this.labelUserNameIntro);
      this.panelUserName.Controls.Add(this.labelUserName);
      this.panelUserName.Location = new System.Drawing.Point(35, 166);
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
      this.labelFirstName.ForeColor = System.Drawing.Color.Black;
      this.labelFirstName.Location = new System.Drawing.Point(44, 94);
      this.labelFirstName.Size = new System.Drawing.Size(63, 16);
      this.labelFirstName.Text = "First Name:";
      // 
      // textBoxFirstName
      // 
      this.textBoxFirstName.BackColor = System.Drawing.SystemColors.Control;
      this.textBoxFirstName.Location = new System.Drawing.Point(108, 91);
      this.textBoxFirstName.Size = new System.Drawing.Size(78, 22);
      this.textBoxFirstName.Text = "";
      this.textBoxFirstName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxName_KeyPress);
      this.textBoxFirstName.TextChanged += new System.EventHandler(this.textBoxFirstName_TextChanged);
      // 
      // labelIntro
      // 
      this.labelIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelIntro.ForeColor = System.Drawing.Color.Blue;
      this.labelIntro.Location = new System.Drawing.Point(8, 42);
      this.labelIntro.Size = new System.Drawing.Size(230, 32);
      this.labelIntro.Text = "If this isn\'t you then please enter your name and press \'Next\':";
      // 
      // textBoxLastName
      // 
      this.textBoxLastName.BackColor = System.Drawing.SystemColors.Control;
      this.textBoxLastName.Location = new System.Drawing.Point(108, 127);
      this.textBoxLastName.Size = new System.Drawing.Size(78, 22);
      this.textBoxLastName.Text = "";
      this.textBoxLastName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxName_KeyPress);
      this.textBoxLastName.TextChanged += new System.EventHandler(this.textBoxLastName_TextChanged);
      // 
      // labelLastName
      // 
      this.labelLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelLastName.ForeColor = System.Drawing.Color.Black;
      this.labelLastName.Location = new System.Drawing.Point(44, 130);
      this.labelLastName.Size = new System.Drawing.Size(63, 16);
      this.labelLastName.Text = "Last Name:";
      // 
      // labelCurrentUser
      // 
      this.labelCurrentUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
      this.labelCurrentUser.ForeColor = System.Drawing.Color.Red;
      this.labelCurrentUser.Location = new System.Drawing.Point(80, 8);
      this.labelCurrentUser.Size = new System.Drawing.Size(108, 18);
      // 
      // labelCurrentUserIntro
      // 
      this.labelCurrentUserIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelCurrentUserIntro.ForeColor = System.Drawing.Color.Black;
      this.labelCurrentUserIntro.Location = new System.Drawing.Point(8, 8);
      this.labelCurrentUserIntro.Size = new System.Drawing.Size(72, 18);
      this.labelCurrentUserIntro.Text = "Current User:";
      // 
      // frmChangeUserName
      // 
      this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(234)), ((System.Byte)(234)), ((System.Byte)(0)));
      this.ControlBox = false;
      this.Controls.Add(this.labelCurrentUser);
      this.Controls.Add(this.panelUserName);
      this.Controls.Add(this.labelFirstName);
      this.Controls.Add(this.textBoxFirstName);
      this.Controls.Add(this.labelIntro);
      this.Controls.Add(this.textBoxLastName);
      this.Controls.Add(this.labelLastName);
      this.Controls.Add(this.labelCurrentUserIntro);
      this.MaximizeBox = false;
      this.Menu = this.mainMenu1;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";
      this.Resize += new System.EventHandler(this.frmChangeUserName_Resize);

    }
		#endregion


    private void InitializeScreen()
    {
      RepositionControls(true);
    }


    private void frmChangeUserName_Resize(object sender, System.EventArgs e)
    {
      RepositionControls(false);
    }


    private void RepositionControls(bool forceRedraw)
    {
      int wid = 0, hgt = 0, aHgt = 0;

      if ((ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt)) | forceRedraw)
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

          labelCurrentUserIntro.Location = new Point(x, y);
          labelCurrentUser.Location = new Point(x * 10, y);

          if (hgt > wid)  // Is it in Portrait mode?
          {
            labelIntro.Location = new Point(x, labelCurrentUserIntro.Bottom + 2 * y);
            labelIntro.Size = new Size(wid - x * 2, labelIntro.Height);

            int subWid = textBoxFirstName.Right - labelFirstName.Left;
            labelFirstName.Location = new Point((wid - subWid) / 2, labelIntro.Bottom + gap * 5);
            textBoxFirstName.Location = new Point(labelFirstName.Right + gap, labelFirstName.Top - 3);

            labelLastName.Location = new Point(labelFirstName.Left, labelFirstName.Bottom + gap * 5);
            textBoxLastName.Location = new Point(textBoxFirstName.Left, labelLastName.Top - 3);

            panelUserName.Location = new Point((wid - panelUserName.Width) / 2, labelLastName.Bottom + gap * 5);
          }

          else  // Landscape mode
          {
            labelIntro.Location = new Point(x, labelCurrentUserIntro.Bottom + 2 * y);
            labelIntro.Size = new Size(wid - x * 2, labelIntro.Height);

            labelFirstName.Location = new Point(x, labelIntro.Bottom + gap * 4);
            textBoxFirstName.Location = new Point(labelFirstName.Right + 0, labelFirstName.Top - 3);

            textBoxLastName.Location = new Point(wid - x - textBoxLastName.Width, textBoxFirstName.Top);
            labelLastName.Location = new Point(textBoxLastName.Left - 0 - labelLastName.Width, labelFirstName.Top);

            panelUserName.Location = new Point((wid - panelUserName.Width) / 2, textBoxFirstName.Bottom + gap * 6);
          }
        }
      }
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


    private void menuItemBack_Click(object sender, System.EventArgs e)
    {
      ScreenAdv = -1;
      this.Close();
    }


    private void menuItemNext_Click(object sender, System.EventArgs e)
    {
      if (labelUserName.Text != "")
      {
        string msg = "\"" + Tools.ProperCase(textBoxFirstName.Text) + " " + Tools.ProperCase(textBoxLastName.Text) + "\"";
        msg = msg.PadLeft(16);
        msg = "Are you absolutely sure you've entered your name correctly?\n\n" + msg;
        DialogResult retval = Tools.ShowMessage(msg, "Review Name Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (retval == DialogResult.Yes)
        {
          // Reaching here, the user has successfully entered their first & last name, from which a username
          // has been created.  We will now store all of this information into the CFSysInfo object.
          SaveUserInfo(textBoxFirstName.Text, textBoxLastName.Text, labelUserName.Text);

          ScreenAdv = 1;
          this.Close();
        }
      }
      else if (textBoxFirstName.Text == "" && textBoxLastName.Text == "")
      {
        // The user is not changing his username so just move onto the next screen
        ScreenAdv = 1;
        this.Close();
      }

      else  // One or the other field has something in it, but not both
      {
        string msg = "Please enter valid first & last names or clear both fields.";
        Tools.ShowMessage(msg, "Incomplete UserName", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
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




	}
}
