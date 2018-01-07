using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using DataObjects;



namespace Desktop
{
  // Define Aliases
  using PublishedLock = DataObjects.Constants.PublishedLock;



	/// <summary>
	/// Presents a dialog box that allows the user to selectively unlock a control, a group of controls, or all controls.
	/// Locking occurs once a poll is published.
	/// </summary>
	public class frmUnlock : System.Windows.Forms.Form
	{
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label labelIntro;
    private System.Windows.Forms.Label labelDetails;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Panel panelChoices;
    private System.Windows.Forms.RadioButton radioButtonJustOne;
    private System.Windows.Forms.RadioButton radioButtonSameType;
    private System.Windows.Forms.RadioButton radioButtonAll;
    private System.Windows.Forms.Label labelInstructions;

    private string UnlockSelection = "";


		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		public frmUnlock(string propPath, PublishedLock lockType, out string unlockSelection)
		{
			InitializeComponent();
      PrepareForm(propPath, lockType);
      ShowDialog();
      unlockSelection = UnlockSelection;
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmUnlock));
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.labelIntro = new System.Windows.Forms.Label();
      this.labelDetails = new System.Windows.Forms.Label();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.panelChoices = new System.Windows.Forms.Panel();
      this.radioButtonJustOne = new System.Windows.Forms.RadioButton();
      this.radioButtonSameType = new System.Windows.Forms.RadioButton();
      this.radioButtonAll = new System.Windows.Forms.RadioButton();
      this.labelInstructions = new System.Windows.Forms.Label();
      this.panelChoices.SuspendLayout();
      this.SuspendLayout();
      // 
      // pictureBox1
      // 
      this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(408, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(192, 112);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      // 
      // labelIntro
      // 
      this.labelIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelIntro.ForeColor = System.Drawing.Color.White;
      this.labelIntro.Location = new System.Drawing.Point(11, 10);
      this.labelIntro.Name = "labelIntro";
      this.labelIntro.Size = new System.Drawing.Size(384, 110);
      this.labelIntro.TabIndex = 2;
      this.labelIntro.Text = @"When this poll was published it was immediately locked because subsequent changes to its structure or contents could cause data integrity problems, namely trying to take data from a newer version and make it 'fit' into an older version.  If the poll template has not yet been uploaded to any mobile devices then you may first wish to unpublish the poll, make your changes, and then publish it again.";
      // 
      // labelDetails
      // 
      this.labelDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelDetails.ForeColor = System.Drawing.Color.Yellow;
      this.labelDetails.Location = new System.Drawing.Point(11, 136);
      this.labelDetails.Name = "labelDetails";
      this.labelDetails.Size = new System.Drawing.Size(586, 64);
      this.labelDetails.TabIndex = 3;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.BackColor = System.Drawing.Color.LightGray;
      this.buttonOK.Location = new System.Drawing.Point(425, 485);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(80, 32);
      this.buttonOK.TabIndex = 0;
      this.buttonOK.Text = "OK";
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCancel.BackColor = System.Drawing.Color.LightGray;
      this.buttonCancel.Location = new System.Drawing.Point(520, 485);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(80, 32);
      this.buttonCancel.TabIndex = 1;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.Click += new System.EventHandler(this.CloseForm);
      // 
      // panelChoices
      // 
      this.panelChoices.BackColor = System.Drawing.Color.Maroon;
      this.panelChoices.Controls.Add(this.radioButtonJustOne);
      this.panelChoices.Controls.Add(this.radioButtonSameType);
      this.panelChoices.Controls.Add(this.radioButtonAll);
      this.panelChoices.Location = new System.Drawing.Point(136, 280);
      this.panelChoices.Name = "panelChoices";
      this.panelChoices.Size = new System.Drawing.Size(296, 160);
      this.panelChoices.TabIndex = 5;
      // 
      // radioButtonJustOne
      // 
      this.radioButtonJustOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.radioButtonJustOne.ForeColor = System.Drawing.Color.White;
      this.radioButtonJustOne.Location = new System.Drawing.Point(16, 16);
      this.radioButtonJustOne.Name = "radioButtonJustOne";
      this.radioButtonJustOne.Size = new System.Drawing.Size(275, 24);
      this.radioButtonJustOne.TabIndex = 7;
      this.radioButtonJustOne.Text = "Allow just this item to be changed";
      this.radioButtonJustOne.CheckedChanged += new System.EventHandler(this.UserChoice_CheckedChanged);
      // 
      // radioButtonSameType
      // 
      this.radioButtonSameType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.radioButtonSameType.ForeColor = System.Drawing.Color.White;
      this.radioButtonSameType.Location = new System.Drawing.Point(16, 68);
      this.radioButtonSameType.Name = "radioButtonSameType";
      this.radioButtonSameType.Size = new System.Drawing.Size(275, 24);
      this.radioButtonSameType.TabIndex = 6;
      this.radioButtonSameType.Text = "Allow all items of this type to be changed";
      this.radioButtonSameType.CheckedChanged += new System.EventHandler(this.UserChoice_CheckedChanged);
      // 
      // radioButtonAll
      // 
      this.radioButtonAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.radioButtonAll.ForeColor = System.Drawing.Color.White;
      this.radioButtonAll.Location = new System.Drawing.Point(16, 120);
      this.radioButtonAll.Name = "radioButtonAll";
      this.radioButtonAll.Size = new System.Drawing.Size(275, 24);
      this.radioButtonAll.TabIndex = 5;
      this.radioButtonAll.Tag = "ALL";
      this.radioButtonAll.Text = "Remove all locks";
      this.radioButtonAll.CheckedChanged += new System.EventHandler(this.UserChoice_CheckedChanged);
      // 
      // labelInstructions
      // 
      this.labelInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelInstructions.ForeColor = System.Drawing.Color.White;
      this.labelInstructions.Location = new System.Drawing.Point(11, 224);
      this.labelInstructions.Name = "labelInstructions";
      this.labelInstructions.Size = new System.Drawing.Size(586, 40);
      this.labelInstructions.TabIndex = 3;
      this.labelInstructions.Text = "Please choose one of the following options and then press OK.  (Note: Such unlock" +
        "ing only applies to the current session.)";
      // 
      // frmUnlock
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.BackColor = System.Drawing.Color.Navy;
      this.ClientSize = new System.Drawing.Size(606, 524);
      this.Controls.Add(this.panelChoices);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.labelIntro);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.labelDetails);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.labelInstructions);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmUnlock";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Unlock Poll";
      this.Closed += new System.EventHandler(this.CloseForm);
      this.panelChoices.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		#endregion


    private void PrepareForm(string propPath, PublishedLock pubLock)
    {
      string details = "";

      switch(pubLock)
      {
        case PublishedLock.Instructions:
          details = "Changing an item on the Instructions page will cause no problems for " +                    "Pocket Pollster but you'll have to be the judge of whether the change(s) you make will adversely " +                    "affect the integrity of the data.  For example, the precise wording of your instructions can affect the " +                    "poll results.";
          break;

        case PublishedLock.QA_Basic:
          details = "It is for you to judge the overall effect of changing the wording of the Questions or Answers. " +
                    "If you're just correcting a spelling mistake or making the wording more clear, then that's no " +
                    "problem but if you're making major changes then you really should publish an entirely new poll.";
          break;

        case PublishedLock.AnswerFormat:
          details = "Some changes with the Answer Format pose no problem - eg. from Standard to List.  But " +
                    "changing from one format type to a radically different one can cause serious problems.";
          break;

        case PublishedLock.ChoiceStructure:
          details = "Changing the structure of the Answers is very risky.  You are strongly advised NOT to do this.";
          break;

        case PublishedLock.QuestionStructure:
          details = "Changing the structure of the Questions is very risky.  You are strongly advised NOT to do this.";
          break;

        default:
          Debug.Fail("Unknown PublishedLock", "frmUnlock.PrepareForm");
          break;
      }

      labelDetails.Text = details;
      labelDetails.Height = Tools.GetLabelHeight(details, labelDetails.Font, labelDetails.Width);

      labelInstructions.Top = labelDetails.Bottom + 12;

      panelChoices.Top = labelInstructions.Bottom + 18;
      panelChoices.Left = (this.Width - panelChoices.Width) / 2;

      if (propPath == null)   // Only happens when Add/Remove buttons of Choices/Questions is pressed
      {
        int dist = panelChoices.Height - radioButtonAll.Bottom;
        radioButtonAll.Top = radioButtonSameType.Top - 12;
        radioButtonSameType.Top = radioButtonJustOne.Top;
        radioButtonJustOne.Visible = false;
        panelChoices.Height = radioButtonAll.Bottom + dist;
      }
      else    
      {
        radioButtonJustOne.Tag = propPath;              // Set tag of upper radio button (the 3rd one is already hard-coded to "ALL")
      }

      radioButtonSameType.Tag = pubLock.ToString();     // Set tag of [what is generally the] center radio button
      this.Height = panelChoices.Bottom + 36 + buttonOK.Height + (this.Height - buttonOK.Bottom);

      buttonOK.Enabled = false;
    }



    private void UserChoice_CheckedChanged(object sender, System.EventArgs e)
    {
      if ((sender as RadioButton).Checked == true)
        panelChoices.Tag = (sender as RadioButton).Tag;

      buttonOK.Enabled = true;
    }



    // Used by Cancel and the upper-right 'X'
    private void CloseForm(object sender, System.EventArgs e)
    {
      this.Close();
    }



    private void buttonOK_Click(object sender, System.EventArgs e)
    {
      UnlockSelection = panelChoices.Tag.ToString();
      this.Close();
    }














	}
}
