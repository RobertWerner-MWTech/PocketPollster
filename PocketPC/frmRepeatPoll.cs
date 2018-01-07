using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;
using DataObjects;


namespace PocketPC
{
  // Define Aliases
  using CFSysInfo = DataObjects.CFSysInfo;
  using Platform = DataObjects.Constants.MobilePlatform;



	/// <summary>
	/// Summary description for frmRepeatPoll.
	/// </summary>
	public class frmRepeatPoll : System.Windows.Forms.Form
	{
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.Label labelInstructions;
    private OpenNETCF.Windows.Forms.ButtonEx buttonPollAgain;
    private OpenNETCF.Windows.Forms.ButtonEx buttonClose;
    private OpenNETCF.Windows.Forms.ButtonEx buttonComplete;
    private OpenNETCF.Windows.Forms.ButtonEx buttonPersonalInfo;
  
    public int UserChoice = 0;
    private int numButtons = 4;


		public frmRepeatPoll(out int userChoice, bool askingRespondents, bool recordAborted)
		{
      Cursor.Current = Cursors.WaitCursor;
			InitializeComponent();
      this.BringToFront();

      if (!askingRespondents)
      {
        buttonPersonalInfo.Visible = false;
        numButtons = 3;
        buttonPollAgain.Text = "Poll Again";  // The prompt for when respondents are not being queried
      }
      else if (recordAborted)
      {
        buttonPersonalInfo.Visible = false;
        numButtons = 3;
      }

      InitializeScreen();

      this.Resize += new System.EventHandler(this.frmRepeatPoll_Resize);
      Cursor.Current = Cursors.Default;
      this.ShowDialog();
      
      this.Resize -= new System.EventHandler(this.frmRepeatPoll_Resize);      
      userChoice = UserChoice;
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmRepeatPoll));
      this.labelInstructions = new System.Windows.Forms.Label();
      this.buttonPollAgain = new OpenNETCF.Windows.Forms.ButtonEx();
      this.buttonClose = new OpenNETCF.Windows.Forms.ButtonEx();
      this.buttonComplete = new OpenNETCF.Windows.Forms.ButtonEx();
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.buttonPersonalInfo = new OpenNETCF.Windows.Forms.ButtonEx();
      // 
      // labelInstructions
      // 
      this.labelInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelInstructions.ForeColor = System.Drawing.Color.Blue;
      this.labelInstructions.Location = new System.Drawing.Point(50, 12);
      this.labelInstructions.Size = new System.Drawing.Size(156, 46);
      this.labelInstructions.Text = "The poll for this person is now complete.  Please choose one of the following opt" +
        "ions:";
      // 
      // buttonPollAgain
      // 
      this.buttonPollAgain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.buttonPollAgain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.buttonPollAgain.Location = new System.Drawing.Point(56, 111);
      this.buttonPollAgain.Size = new System.Drawing.Size(136, 30);
      this.buttonPollAgain.Tag = "2";
      this.buttonPollAgain.Text = "Poll Another Person";
      this.buttonPollAgain.TextAlign = OpenNETCF.Drawing.ContentAlignment.MiddleCenter;
      this.buttonPollAgain.Click += new System.EventHandler(this.ButtonSelection);
      // 
      // buttonClose
      // 
      this.buttonClose.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.buttonClose.Location = new System.Drawing.Point(56, 155);
      this.buttonClose.Size = new System.Drawing.Size(136, 31);
      this.buttonClose.Tag = "3";
      this.buttonClose.Text = "Close & Reuse Later";
      this.buttonClose.TextAlign = OpenNETCF.Drawing.ContentAlignment.MiddleCenter;
      this.buttonClose.Click += new System.EventHandler(this.ButtonSelection);
      // 
      // buttonComplete
      // 
      this.buttonComplete.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.buttonComplete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.buttonComplete.Location = new System.Drawing.Point(56, 200);
      this.buttonComplete.Size = new System.Drawing.Size(136, 32);
      this.buttonComplete.Tag = "4";
      this.buttonComplete.Text = "Complete Poll";
      this.buttonComplete.TextAlign = OpenNETCF.Drawing.ContentAlignment.MiddleCenter;
      this.buttonComplete.Click += new System.EventHandler(this.ButtonSelection);
      // 
      // buttonPersonalInfo
      // 
      this.buttonPersonalInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.buttonPersonalInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.buttonPersonalInfo.Location = new System.Drawing.Point(56, 64);
      this.buttonPersonalInfo.Size = new System.Drawing.Size(136, 30);
      this.buttonPersonalInfo.Tag = "1";
      this.buttonPersonalInfo.Text = "Edit Personal Info";
      this.buttonPersonalInfo.TextAlign = OpenNETCF.Drawing.ContentAlignment.MiddleCenter;
      this.buttonPersonalInfo.Click += new System.EventHandler(this.ButtonSelection);
      // 
      // frmRepeatPoll
      // 
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ControlBox = false;
      this.Controls.Add(this.labelInstructions);
      this.Controls.Add(this.buttonClose);
      this.Controls.Add(this.buttonPollAgain);
      this.Controls.Add(this.buttonComplete);
      this.Controls.Add(this.buttonPersonalInfo);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";

    }
		#endregion


    private void InitializeScreen()
    {
      ToolsCF.SipShowIM(0);       // Ensure that SIP is collapsed
      RepositionControls(true);
    }


    private void frmRepeatPoll_Resize(object sender, System.EventArgs e)
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
          if (hgt > wid)  // Is it in Portrait mode?
          {
            labelInstructions.Size = new Size(156, 46);
            labelInstructions.Left = (wid - labelInstructions.Width) / 2 + 5;
            labelInstructions.Top = 12;

            // Now size and stack buttons with available space
            // Topmost button will be gap*4 pixels below the Instructions text
            // Bottommost button will be gap*3 pixels above bottom edge of screen
            int topEdge = labelInstructions.Bottom + gap * 3;

            int availSpace = aHgt - topEdge - (gap * 4);

            // How we calculate 'unitHgt':
            //   4 Buttons:   4 buttons + 3 spaces (ie. 4*2 + 3*1 = 11)
            //   3 Buttons:   3 buttons + 2 spaces (ie. 3*2 + 2*1 = 8)
            int unitHgt = (numButtons == 4) ? availSpace / 11 : availSpace / 8;

            if (numButtons == 4)
            {
              buttonPersonalInfo.Top = topEdge;
              buttonPersonalInfo.Height = unitHgt * 2;
              buttonPersonalInfo.Left = (wid - buttonPersonalInfo.Width) / 2;
              buttonPollAgain.Top = buttonPersonalInfo.Bottom + unitHgt;
            }
            else
              buttonPollAgain.Top = topEdge;
            
            buttonPollAgain.Height = unitHgt * 2;
            buttonPollAgain.Left = (wid - buttonPollAgain.Width) / 2;

            buttonClose.Top = buttonPollAgain.Bottom + unitHgt;
            buttonClose.Height = buttonPollAgain.Height;
            buttonClose.Left = buttonPollAgain.Left;

            buttonComplete.Top = buttonClose.Bottom + unitHgt;
            buttonComplete.Height = buttonClose.Height;
            buttonComplete.Left = buttonClose.Left;
          }

          else  // Landscape mode
          {
            labelInstructions.Size = new Size(200, 46);
            labelInstructions.Left = (wid - labelInstructions.Width) / 2 + 5;
            labelInstructions.Top = 12;

            int topEdge = labelInstructions.Bottom + gap * 3;
            int availSpace = aHgt - topEdge - (gap * 3);

            if (numButtons == 4)
            {
              // Now size and stack buttons with available space, in 2 x 2 pattern
              // Topmost button will be gap*3 pixels below the Instructions text
              // Bottommost button will be gap*3 pixels above bottom edge of screen
              int unitHgt = availSpace / 5;  // 2 buttons + 1 space (ie. 2*2 + 1*1 = 5)

              buttonPersonalInfo.Top = topEdge;
              buttonPersonalInfo.Height = unitHgt * 2;
              buttonPersonalInfo.Left = wid / 2 - 10 - buttonPersonalInfo.Width;

              buttonPollAgain.Top = buttonPersonalInfo.Top;
              buttonPollAgain.Height = unitHgt * 2;
              buttonPollAgain.Left = wid / 2 + 10;

              buttonClose.Top = buttonPersonalInfo.Bottom + unitHgt;
              buttonClose.Height = buttonPersonalInfo.Height;
              buttonClose.Left = buttonPersonalInfo.Left;

              buttonComplete.Top = buttonClose.Top;
              buttonComplete.Height = buttonClose.Height;
              buttonComplete.Left = buttonPollAgain.Left;
            }
            else
            {
              // Stack the buttons 3 high, like in Portrait mode
              int unitHgt = availSpace / 8;  // 3 buttons + 2 spaces (ie. 3*2 + 2*1 = 8)

              buttonPollAgain.Top = topEdge;
              buttonPollAgain.Height = unitHgt * 2;
              buttonPollAgain.Left = (wid - buttonPollAgain.Width) / 2;

              buttonClose.Top = buttonPollAgain.Bottom + unitHgt;
              buttonClose.Height = buttonPollAgain.Height;
              buttonClose.Left = buttonPollAgain.Left;

              buttonComplete.Top = buttonClose.Bottom + unitHgt;
              buttonComplete.Height = buttonClose.Height;
              buttonComplete.Left = buttonClose.Left;
            }
          }
        }
      }
    }


    private void ButtonSelection(object sender, System.EventArgs e)
    {
      ButtonEx button = sender as ButtonEx;
      UserChoice = Convert.ToInt32(button.Tag);
      this.Close();
    }
	}
}
