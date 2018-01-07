using System;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
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
  using AnswerFormat = DataObjects.Constants.AnswerFormat;
  using AutoScrollPanel = DataObjects.AutoScrollPanel;



	/// <summary>
	/// Summary description for frmPoll.
	/// </summary>
	public class frmPoll : System.Windows.Forms.Form
	{
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.MenuItem menuItemBack;
    private System.Windows.Forms.MenuItem menuItemFinish;
    private System.Windows.Forms.MenuItem menuItemNext;    
    private System.Windows.Forms.Label labelcurrQuest;
    private System.Windows.Forms.Label labelQuestion;
    private AutoScrollPanel panelChoices;

    private Poll PollModel;
    private bool IsDirty;                // Set true by code in this form if even one choice is selected/changed
    private bool AbortRecord = false;    // Set true if the pollster wishes to not store the responses for the current respondent
    private int gap = 4;                 // A small value, used as the basis for providing horiz and vert spacing
    private int leftMargin;
    public int ScreenAdv;                // -1 : Move backward one screen   +1 : Move forward one screen


    // Form-level properties

    // This keeps track of what Question we're currently displaying
    private int currQuestion;
  
    private int CurrQuestion     // 0-based index
    {
      get
      {
        return currQuestion;
      }
      set
      {  
        currQuestion = value;                                                  // Set the new question number
        PopulateForm(PollModel, CurrRespondent, currQuestion, false, true);    // and then populate the form accordingly
      }
    }


    // This keeps track of which Respondent we're currently dealing with.
    // It is only set in the constructor and then just read by various methods.
    private int currRespondent;
    private int CurrRespondent   // 0-based index
    {
      get
      {
        return currRespondent;
      }
      set
      {
        currRespondent = value;
      }
    }


    // Constructor
		public frmPoll(Poll pollModel, int respID, ref bool isDirty, ref int screenAdv, ref bool backAvail, out bool abortRecord)
		{
      Cursor.Current = Cursors.WaitCursor;
			InitializeComponent();
      InitializeBaseControls(pollModel.CreationInfo.HideQuestionNumbers);
      InitializeScreen();

      PollModel = pollModel;
      IsDirty = isDirty;
      CurrRespondent = respID;
      CurrQuestion = 0;             // This also populates the form for Question #1 and its Choices

      this.BringToFront();

      // Note: Upon initial entry into this form, I don't think that one should be allowed to back up into frmRespondent.
      //       It just seems awkward, because the user will start cycling through several questions all using the same form.
      //       And remember that the user will be able to edit the RespondentInfo before saving anyhow.
      // Later Thinking: Not sure I agree with this anymore.

      // We can only activate this once panelChoices has been instantiated
      this.Resize += new System.EventHandler(this.frmPoll_Resize);

      // And we also need to wire up the handling of all events fired in panelChoices           
      PanelControlsCF.PanelChoicesEvent += new EventHandler(ChoiceHandler);      

      Cursor.Current = Cursors.Default;
      this.ShowDialog();

      // This next line is absolutely necessary or else a subtle bug is introduced such that
      // the new iteration of this form will have its ChoiceHandler referring to an older iteration
      // and as such, CurrQuestion and CurrRespondent will be completely wrong!
      PanelControlsCF.PanelChoicesEvent -= new EventHandler(ChoiceHandler);      

      this.Resize -= new System.EventHandler(this.frmPoll_Resize);

      isDirty = IsDirty;
      abortRecord = AbortRecord;
      screenAdv = ScreenAdv;

      // Debug: Question: Do we allow the user to back up into frmPoll from frmInstructions?
      backAvail = false;   // For now, we'll say 'No'
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmPoll));
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemBack = new System.Windows.Forms.MenuItem();
      this.menuItemNext = new System.Windows.Forms.MenuItem();
      this.menuItemFinish = new System.Windows.Forms.MenuItem();
      this.labelcurrQuest = new System.Windows.Forms.Label();
      this.labelQuestion = new System.Windows.Forms.Label();
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.Add(this.menuItemBack);
      this.mainMenu.MenuItems.Add(this.menuItemNext);
      this.mainMenu.MenuItems.Add(this.menuItemFinish);
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
      // menuItemFinish
      // 
      this.menuItemFinish.Text = "  Finish";
      this.menuItemFinish.Click += new System.EventHandler(this.menuItemFinish_Click);
      // 
      // labelcurrQuest
      // 
      this.labelcurrQuest.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
      this.labelcurrQuest.ForeColor = System.Drawing.Color.Yellow;
      this.labelcurrQuest.Location = new System.Drawing.Point(8, 8);
      this.labelcurrQuest.Size = new System.Drawing.Size(224, 20);
      this.labelcurrQuest.Text = "Question #1";
      // 
      // labelQuestion
      // 
      this.labelQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelQuestion.ForeColor = System.Drawing.Color.Blue;
      this.labelQuestion.Location = new System.Drawing.Point(16, 34);
      this.labelQuestion.Size = new System.Drawing.Size(208, 30);
      // 
      // frmPoll
      // 
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ControlBox = false;
      this.Controls.Add(this.labelQuestion);
      this.Controls.Add(this.labelcurrQuest);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";

    }
		#endregion


    private void InitializeBaseControls(bool hideQuestNum)
    {
      int wid, hgt, aHgt;
      ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt);

      leftMargin = gap * 4;
      
      if (hideQuestNum)
      {
        labelcurrQuest.Visible = false;
        labelQuestion.Location = new Point(leftMargin, 12);
      }
      else
      {
        labelQuestion.Location = new Point(leftMargin, 34);   // Hardcode '34' for now - It looks like a good location below labelcurrQuest
      }

      // Create the scrolling panel for the topic controls
      panelChoices = new DataObjects.AutoScrollPanel();
      this.Controls.Add(panelChoices);
    }


    private void InitializeScreen()
    {
      RepositionControls(true, true);
    }


    private void frmPoll_Resize(object sender, System.EventArgs e)
    {
      RepositionControls(false, false);
    }


    private void RepositionControls(bool forceRedraw, bool skipPopulation)
    {
      int wid = 0, hgt = 0, aHgt = 0;

      if ((ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt)) | forceRedraw)
      {
        if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
        {
          // To Do: Still to need to flesh out SmartPhone positioning

        }

        else
        {
          int horizMargin = leftMargin + gap * 2;

          labelQuestion.Size = new Size(wid - leftMargin * 2, labelQuestion.Height);
          panelChoices.Location = new Point(horizMargin, labelQuestion.Bottom + gap * 2);
          panelChoices.Size = new Size(wid - horizMargin, aHgt - panelChoices.Top - gap * 2);
        }

        if (!skipPopulation)
          PopulateForm(PollModel, CurrRespondent, CurrQuestion, false);
      }
    }


    #region MenuButtons

    private void menuItemBack_Click(object sender, System.EventArgs e)
    {
      this.Focus();  // Forces all events (such as TextBox.LostFocus) to fire

      if (CurrQuestion == 0)
      {
        ScreenAdv = -1;
        this.Close();
      }
      else
        CurrQuestion--;  // Decrement question number and repopulate screen
    }


    private void menuItemNext_Click(object sender, System.EventArgs e)
    {
      this.Focus();

      if (CurrQuestion == PollModel.Questions.Count - 1)  // Are we at the last Question?
      {
        ScreenAdv = 1;
        this.Close();
      }
      else
        CurrQuestion++;  // Increment question number and repopulate screen
    }


    private void menuItemFinish_Click(object sender, System.EventArgs e)
    {
      bool proceed = false;

      if (CurrQuestion == PollModel.Questions.Count - 1)  // Are we on the last question?
        proceed = true;
      else
      {
        // Example Scenario: Maybe the respondent wanted to leave and the poll had to be cut short
        string msg = "Are you sure you want to skip the rest of the questions?";
        if (Tools.ShowMessage(msg, "Skip Remaining Questions?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
          proceed = true;

          if (PollModel.PollsterPrivileges.CanAbortRecord)
          {
            msg = "Do you want to throw away the responses for the CURRENT set of questions?";
            if (Tools.ShowMessage(msg, "Abort This Record?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
              AbortRecord = true;
          }
        }
      }
      
      if (proceed)
      {
        ScreenAdv = 1;
        this.Close();
      }
    }

    #endregion


    #region PopulateForm

    private void PopulateForm(Poll pollModel, int currResp, int currQuest, bool resetTopMargin)
    {
      PopulateForm(pollModel, currResp, currQuest, false, resetTopMargin);
    }

    /// <summary>
    /// Populates the form with the current information.
    /// </summary>
    /// <param name="pollModel"></param>
    /// <param name="currResp"></param>
    /// <param name="currQuest"></param>
    /// <param name="skipQuestionSection"></param>    // If true then don't worry about upper question portion of form
    /// <param name="resetTopMargin"></param>         // If true then we're populating a new question so set TopMargin back to 0
    private void PopulateForm(Poll pollModel, int currResp, int currQuest, bool skipQuestionSection, bool resetTopMargin)
    {
      Cursor.Current = Cursors.WaitCursor;

      CheckBoxPP chkBox;
      Label label;
      NumericUpDownPP spinner;
      RadioButtonPP radBut;

      int wid, hgt, aHgt;
      ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt);

      //int availWidth = wid - gap * 2;
      int availWidth = panelChoices.Width;

      int selIndex = -1;                                                        // Used keep track of which control we're placing on the form
      Font font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);   // Default font throughout this method

      if (currQuest >= pollModel.Questions.Count)   // Double-check (though should never happen)
        return;

      // Populate object variable with current question info
      _Question quest = pollModel.Questions[currQuest];

      if (!skipQuestionSection)
      {
        if (! pollModel.CreationInfo.HideQuestionNumbers)
        {
          // Display question number
          labelcurrQuest.Text = "Question #" + (currQuest + 1).ToString();
        }

        // Decide whether to disable Back button
        // Note: This was primarily added to prevent a strange bug that occurred when the user was allowed
        //       to back up into the Respondent form.  After this happened, the 'CurrQuestion' value went
        //       out of sync, though I'm not sure why.
        menuItemBack.Enabled = (currQuest == 0) ? false : true;

        // Decide whether to disable Next button
        menuItemNext.Enabled = (currQuest < pollModel.Questions.Count - 1) ? true : false;

        // labelQuestion size must be adjusted to fit the size of the Question text
        string questText = pollModel.Questions[currQuest].Text;
        Size labelSize = Tools.GetLabelSize(questText, labelQuestion.Font);

        labelQuestion.Text = Tools.FixPanelText(questText);
        int numLines = Math.Max(Convert.ToInt32(labelSize.Width / labelQuestion.Width + 1), 1);
        labelQuestion.Height = numLines * labelSize.Height;
      }

      panelChoices.Top = labelQuestion.Bottom + gap * 2;
      
      int oldTop = 0;
      if (!resetTopMargin)
        oldTop = panelChoices.GetTopMargin();

      panelChoices.Contents.Controls.Clear();   // Clear previous Choice controls before creating a new set

      if (quest.Choices.Count == 0)
      {
        string msg = "This question has no available choices.  You should notify the poll's creator: " + pollModel.CreationInfo.CreatorName;
        Tools.ShowMessage(msg, "Question Has No Choices", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }

      // Now add the available choices
      int topMargin = gap * 2;           // Note that this value is relative to the top of panelChoices
      int rowGap = gap * 3;
      
      int leftIndent = 0;                // Used to set indentation of several AnswerFormat types
      bool longLabelsExamined = false;   // Set true by certain answerformat types after pre-scanning of Choice labels is done

      bool needSIP = false;              // Set true if even one textbox is placed on form
      bool exitLoop = false;
      string extraText = "";             // Populated with the previous ExtraText value
      string prevText;
      int numlines;
      int textWid;
      _Response response;

      // Set true once the one (and only) choice is set again (ex. with radiobuttons).
      // This saves us wasting time checking 'WasChoicePrevSelected' again once we've
      // already set the one item with the ExtraInfo textbox.
      bool prevChoiceSel = false;    

      foreach (_Choice choice in quest.Choices)
      {
        switch (quest.AnswerFormat)
        {
          case AnswerFormat.Standard:         // Standard vertical radio buttons
            if (choice.Text != "")
            {
              if (!longLabelsExamined)
              {
                // 15 is width of the circular radio button, 6 is the gap between it and its label, and 20 provides space for the possible scrollbar
                leftIndent = Tools.CheckForLongLabels(quest.Choices, font, availWidth - leftMargin - 15 - 6 - 20, leftMargin, 0);
                longLabelsExamined = true;
              }

              selIndex++;
              radBut = PanelControlsCF.AddRadioButton(choice.Text, font, topMargin, leftIndent, availWidth - leftIndent - 15 - 6 - 20, panelChoices.Contents, selIndex);

              if (radBut != null)
              {
                topMargin += radBut.LabelHeight;

                if (!prevChoiceSel && (WasChoicePrevSelected(pollModel, currResp, currQuest, choice.ID, out extraText)))
                {
                  radBut.Checked = true;
                  prevChoiceSel = true;

                  // If its ExtraInfo flag is set true and this choice has been selected then provide a textbox for "Other" type input
                  if (choice.ExtraInfo)
                  {
                    needSIP = true;
                    prevText = Tools.EnsureSuffix(choice.MoreText, ":");
                    textWid = Tools.GetLabelWidth(prevText, font);
                    numlines = choice.ExtraInfoMultiline ? 3 : 1;

                    topMargin += gap * 2;
                    int txtBoxHgt = PanelControlsCF.AddTextBox(extraText, font, topMargin, leftMargin + gap * 2, numlines, (int) (panelChoices.Width * 0.35), prevText, panelChoices.Contents, selIndex);
                    topMargin += txtBoxHgt;
                  }
                }

                topMargin += rowGap;
              }
            }
            else
              // If this Choice.Text item is present but blank then provide the equivalent spacing to one line
              topMargin += (int) (font.Size * 2.25) + rowGap;   
            
            break;


          case AnswerFormat.List:             // Listbox
            // We're going to pass all of the choices at once to the listbox creation method
            ListBoxPP lstBox = PanelControlsCF.AddListBox(quest.Choices, font, topMargin, leftMargin, panelChoices.Contents, aHgt - panelChoices.Top);

            if (lstBox != null)
            {
              topMargin += lstBox.Height;
              selIndex = GetPrevSelIndex(pollModel, currResp, currQuest, out extraText);

              if (selIndex != -1)
              {
                lstBox.SelectedIndex = selIndex;
                _Choice choice2 = quest.Choices[selIndex];

                // If its ExtraInfo flag is set true and this choice has been selected then provide a textbox for "Other" type input
                if (choice2.ExtraInfo)
                {
                  needSIP = true;
                  topMargin += gap * 3;
                  prevText = Tools.EnsureSuffix(choice2.MoreText, ":");
                  numlines = choice.ExtraInfoMultiline ? 3 : 1;

                  int txtBoxHgt = PanelControlsCF.AddTextBox(extraText, font, topMargin, leftMargin + gap, numlines, (int) (panelChoices.Width * 0.35), prevText, panelChoices.Contents, selIndex);
                  topMargin += txtBoxHgt;
                }
              }

              topMargin += rowGap;
            }

            exitLoop = true;   // We only want one listbox, not multiple ones
            break;
          
          
          case AnswerFormat.DropList:         // Dropdown list
            // We're going to pass all of the choices at once to the dropdown listbox creation method
            ComboBoxPP cboBox = PanelControlsCF.AddComboBox(quest.Choices, font, topMargin, leftMargin, panelChoices.Contents);

            if (cboBox != null)
            {
              topMargin += cboBox.Height;
              selIndex = GetPrevSelIndex(pollModel, currResp, currQuest, out extraText);

              if (selIndex != -1)
              {
                cboBox.SelectedIndex = selIndex + 1;   // The +1 is necessary to account for the introductory item
                _Choice choice2 = quest.Choices[selIndex];

                // If its ExtraInfo flag is set true and this choice has been selected then provide a textbox for "Other" type input
                if (choice2.ExtraInfo)
                {
                  needSIP = true;
                  topMargin += gap * 3;
                  prevText = Tools.EnsureSuffix(choice2.MoreText, ":");
                  numlines = choice.ExtraInfoMultiline ? 3 : 1;

                  int txtBoxHgt = PanelControlsCF.AddTextBox(extraText, font, topMargin, leftMargin + gap, numlines, (int) (panelChoices.Width * 0.35), prevText, panelChoices.Contents, selIndex);
                  topMargin += txtBoxHgt;
                }
              }

              topMargin += rowGap;
            }

            exitLoop = true;   // We only want one dropdown listbox, not multiple ones
            break;


          case AnswerFormat.MultipleChoice:   // Checkboxes
            if (choice.Text != "")
            {
              if (!longLabelsExamined)
              {
                // 17 is width of the square checkbox, 6 is the gap between it and its label, and 20 provides space for the possible scrollbar
                leftIndent = Tools.CheckForLongLabels(quest.Choices, font, availWidth - leftMargin - 17 - 6 - 20, leftMargin, 0);
                longLabelsExamined = true;
              }

              selIndex++;
              chkBox = PanelControlsCF.AddCheckBox(choice.Text, font, topMargin, leftIndent, availWidth - leftIndent - 17 - 6 - 20, panelChoices.Contents, selIndex);

              if (chkBox != null)
              {
                topMargin += chkBox.LabelHeight;

                // Need to add extra check in here to see if the checkbox was previously checked!
                // Note: For details on how 'extraText' values are encoded, please look inside
                //       the comments of 'WasChoicePrevSelected'.
                if (WasChoicePrevSelected(pollModel, currResp, currQuest, choice.ID, out extraText))
                {      
                  chkBox.Checked = true;  // If so, then set it like it was previously

                  // If its ExtraInfo flag is set true and this choice has been selected then provide a textbox for "Other" type input
                  if (choice.ExtraInfo)
                  {
                    needSIP = true;
                    topMargin += gap * 2;
                    prevText = Tools.EnsureSuffix(choice.MoreText, ":");
                    numlines = choice.ExtraInfoMultiline ? 3 : 1;

                    int txtBoxHgt = PanelControlsCF.AddTextBox(extraText, font, topMargin, leftMargin + gap * 3, numlines, (int) (panelChoices.Width * 0.35), prevText, panelChoices.Contents, selIndex);
                    topMargin += txtBoxHgt;
                  }
                }

                topMargin += rowGap;
              }
            }
            else
              // If this Choice.Text item is present but blank then provide the equivalent spacing to one line
              topMargin += (int) (font.Size * 2.25) + rowGap;   
            
            break;


          case AnswerFormat.Range:
            if (choice.Text != "")
            {
              if (!longLabelsExamined)
              {
                // 15 is width of the circular radio button, 6 is the gap between it and its label, and 20 provides space for the possible scrollbar
                leftIndent = Tools.CheckForLongLabels(quest.Choices, font, availWidth - leftMargin - 15 - 6 - 20, leftMargin + gap * 3, leftMargin);
                longLabelsExamined = true;
              }

              selIndex++;
              radBut = PanelControlsCF.AddRadioButton(choice.Text, font, topMargin, leftIndent, availWidth - leftIndent - 15 - 6 - 20, panelChoices.Contents, selIndex);
              PanelControlsCF.AddLabel(choice.MoreText, font, Color.Blue, topMargin + 1, leftIndent - 5, ContentAlignment.TopRight, panelChoices.Contents);

              if (radBut != null)
              {
                topMargin += radBut.LabelHeight + rowGap;

                if (!prevChoiceSel && (WasChoicePrevSelected(pollModel, currResp, currQuest, choice.ID, out extraText)))
                {
                  radBut.Checked = true;
                  prevChoiceSel = true;
                }
              }
            }
            else
              // If this Choice.Text item is present but blank then provide the equivalent spacing to one line
              topMargin += (int) (font.Size * 2.25) + rowGap;   
            
            break;


          case AnswerFormat.MultipleBoxes:
            if (choice.Text != "")
            {
              selIndex++;
              response = pollModel.Respondents[currResp].Responses.Find_ResponseByQuestionID(quest.ID);
              extraText = Tools.GetExtraTextValue(response.ExtraText, choice.ID);

              int txtBoxWid = 40;
              int labelLeft = leftMargin + gap + txtBoxWid + 5;
              label = PanelControlsCF.AddMultilineLabel(choice.Text, font, Color.Black, topMargin + 1, labelLeft, -1, availWidth - labelLeft, panelChoices.Contents);
              int txtBoxTop = topMargin + (label.Height - (int) font.Size * 2) / 2;   //Debug: Confirm that font.Size is similar to font.Height
              int txtBoxHgt = PanelControlsCF.AddTextBox(extraText, font, txtBoxTop, leftMargin + gap, 1, txtBoxWid, "", panelChoices.Contents, selIndex);

              topMargin += Math.Max(txtBoxHgt, label.Height) + rowGap;
              needSIP = true;
            }
            else
              // If this Choice.Text item is present but blank then provide the equivalent spacing to one line
              topMargin += (int) (font.Size * 2.25) + rowGap;   

            break;

          
          case AnswerFormat.Freeform:
          {
            // We're going to handle everything for this option with this first pass
            response = pollModel.Respondents[currResp].Responses.Find_ResponseByQuestionID(quest.ID);

            selIndex++;
            int txtBoxHgt = PanelControlsCF.AddTextBox(response.ExtraText, font, topMargin, gap, 5, (int) (panelChoices.Width * 0.7), "", panelChoices.Contents, selIndex);
            needSIP = true;
            
            topMargin += txtBoxHgt + rowGap;

            exitLoop = true;   // We only want one textbox, not multiple ones
          }
          break;


          case AnswerFormat.Spinner:
            // We're going to handle everything for this option with this first pass
            selIndex++;  // Note: Currently not used in practice but including it will allow for future expansion when more than 1 spinner sits on a panel
            response = pollModel.Respondents[currResp].Responses.Find_ResponseByQuestionID(quest.ID);
            int answerID = (response.AnswerID == "") ? 0 : Convert.ToInt32(response.AnswerID);

            spinner = PanelControlsCF.AddSpinner(answerID, choice.Text, choice.MoreText, choice.MoreText2, font, topMargin, leftMargin + 3 * gap, panelChoices.Contents, selIndex);
            
            if (spinner != null)
            {
              topMargin += spinner.Height + rowGap;

              if (response.AnswerID == "")                       // This is necessary so that 'response.AnswerID' is set to an initial value
                response.AnswerID = spinner.Value.ToString();    // in case 'Next' is pressed immediately, without ever touching the spinner
            }

            exitLoop = true;   // For now, we only want one spinner, not multiple ones ... though this may change shortly!
            break;


          default:
            Debug.WriteLine("Unknown AnswerFormat encountered: " + quest.AnswerFormat.ToString(), "frmPoll.PopulateForm (1)");
            break;
        }

        if (exitLoop == true)
          break;
      }


      // We must now ensure there's enough room below the last item.  This varies depending on whether text entry is required.
      if (needSIP)
      {
        panelChoices.SetScrollHeight(topMargin + gap + 70);   // The numeric value wad added to account for the SIP            
      }
      else
      {
        panelChoices.SetScrollHeight(topMargin + gap);        // No SIP required
        ToolsCF.SipShowIM(0);                                 // Might as well ensure SIP is collapsed
      }

//      if (resetTopMargin)
//        panelChoices.SetTopMargin(0);
//      else if (panelChoices.IsScrollBarsVisible())
//        panelChoices.SetTopMarginViaBottom(oldBottom);

      panelChoices.SetTopMargin(resetTopMargin ? 0 : oldTop);
      Cursor.Current = Cursors.Default;
    }




    /// <summary>
    /// Determines if the specified choice was previously selected.  Also returns the 'extraText' value by reference.
    /// </summary>
    /// <param name="pollModel"></param>
    /// <param name="currResp"></param>
    /// <param name="currQuest"></param>
    /// <param name="choiceID"></param>
    /// <param name="extraText"></param>
    /// <returns></returns>
    private bool WasChoicePrevSelected(Poll pollModel, int currResp, int currQuest, int choiceID, out string extraText)
    {
      bool retval = false;
      extraText = "";
      
      try
      {
        _Question quest = pollModel.Questions[currQuest];

        int questID = quest.ID;
        _Response response = pollModel.Respondents[currResp].Responses.Find_ResponseByQuestionID(questID);
        response.AnswerID = response.AnswerID.Trim(new char[] {','});   // Extra check in case the value was improperly altered

        // For MultipleChoice ExtraText values, we'll either have a null string ("") or have each entry
        // prefixed with the associated ChoiceID value and a tilde (a character that isn't allowed entry
        // by the user).  An example:  if response.AnswerID = "7,15"
        //                             then we may have response.ExtraText = "15~apple"
        if ((quest.AnswerFormat == AnswerFormat.MultipleChoice) || (quest.AnswerFormat == AnswerFormat.MultipleBoxes))
        {
          if (response.AnswerID != "")   // Debug: Need to test all this code with MultipleBoxes!!!
          {
            // Retrieve all AnswerIDs and add sentinels
            string allAnswerIDs = "," + response.AnswerID + ",";

            if (allAnswerIDs.IndexOf("," + choiceID.ToString() + ",") != -1)
            {
              retval = true;

              // Note: We've now satisfied the primary purpose of this method, which is to determine whether the value
              //       specified by "choiceID" was previously selected.  But we're now going to see whether there's an
              //       associated "extraText" value, which will be displayed in the associated textbox.
              extraText = Tools.GetExtraTextValue(response.ExtraText, choiceID);
            }
          }
        }

        else  // Single ExtraText value only
        {
          extraText = response.ExtraText;
          if (response.AnswerID == choiceID.ToString())
            retval = true;
        }
      }

      catch (Exception ex)
      {
        Debug.Fail("Error determining if choice was previously selected - Message: " + ex.Message, "frmPoll.WasChoicePrevSelected");
      }

      return retval;
    }

    

    /// <summary>
    /// Retrieves the index of the previously selected item (or -1 if there was no selection).
    /// Note: Currently only works with single choice questions.
    /// Also returns the 'extraText' value by reference.
    /// </summary>
    /// <param name="pollModel"></param>
    /// <param name="currResp"></param>
    /// <param name="currQuest"></param>
    /// <param name="extraText"></param>
    /// <returns></returns>
    private int GetPrevSelIndex(Poll pollModel, int currResp, int currQuest, out string extraText)
    {
      int choiceIdx = -1;  // Initialize

      _Question quest = pollModel.Questions[currQuest];
      _Response response = pollModel.Respondents[currResp].Responses.Find_ResponseByQuestionID(quest.ID);
      extraText = response.ExtraText;

      if (response.AnswerID != "")
      {
        int answerID = Convert.ToInt32(response.AnswerID);

        for (int i = 0; i < quest.Choices.Count; i++)
        {
          if (quest.Choices[i].ID == answerID)
          {
            choiceIdx = i;
            break;
          }
        }
      }

      return choiceIdx;
    }

    #endregion 


    #region HandleChoices

    /// <summary>
    /// Handles events fired when the user selects a choice (or enters text into a Choice textbox)
    /// 
    /// AnswerFormats 0 - 3 allow the user to add an extra comment for any item that is so marked with this
    /// capability (ie. ExtraInfo = true).  We will call PopulateForm again when the presence of or location
    /// of the "Other" type textbox changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChoiceHandler(object sender, EventArgs e)
    {
      // Note: We're going to set this property right now.  Though it's possible that something might
      //       go wrong later in the method, that would be rare and the error will be caught anyhow.
      IsDirty = true;

      try
      {
        _Choice choice;
        int choiceID;
        int choiceIdx;
        string ctrlType = sender.GetType().Name;

        // Initialize - FYI these variables manage those items that have an ExtraInfo field and were or are currently selected
        string oldExtraInfo = "";   // For single-choice items, is simply the ChoiceID prev selected; for multiple-choice items is a CDF representation of the previously selected ChoiceIDs
        string newExtraInfo = "";

        _Question question = PollModel.Questions[CurrQuestion];
        AnswerFormat aFormat = question.AnswerFormat;
        int questID = question.ID;        
        _Response response = PollModel.Respondents[CurrRespondent].Responses.Find_ResponseByQuestionID(questID);

        // Add unique GUID for this response
        response.Guid = PocketGuid.NewGuid().ToString();

        // Add appropriate date stamping
        DateTime currTime = DateTime.Now;
        if (response.TimeCaptured.Date == DateTime.MinValue)  // new DateTime(1, 1, 1))
          response.TimeCaptured = currTime;

        response.LastModified = currTime;

        // Record previous state of choices (for AnswerFormats 0 - 3 only)
        if (aFormat == AnswerFormat.Standard || aFormat == AnswerFormat.List || aFormat == AnswerFormat.DropList)
        {
          if (response.AnswerID != "")
          {
            choice = question.Choices.Find_Choice(Convert.ToInt32(response.AnswerID));   // See which choice, if any, was previously selected
            if (choice != null)                           // If there was a previous choice then
              if (choice.ExtraInfo)                       // Did this choice have an ExtraInfo textbox?
                oldExtraInfo = choice.ID.ToString();      // If so then store its ChoiceID
          }
        }
        else if (aFormat == AnswerFormat.MultipleChoice)   
          // Since each checkbox can have an ExtraInfo textbox we need to use a more complex approach 
          // to see which ones that do have such a textbox, were previously selected.
          oldExtraInfo = GetExtraInfoSelections(response, question.Choices); 


        switch (aFormat)
        {
          case AnswerFormat.Standard:
            switch (ctrlType)
            {
              case "RadioButtonPP":
              case "LinkLabelPP":
                if (ctrlType == "RadioButtonPP")
                  choiceIdx = (sender as RadioButtonPP).SelectedIndex;
                else
                  choiceIdx = (sender as LinkLabelPP).SelectedIndex;

                choice = question.Choices[choiceIdx];
                choiceID = choice.ID;
                response.AnswerID = choiceID.ToString();   // Store newly selected choice

                // Now see if we need to repopulate the form
                if (choice.ExtraInfo)
                  newExtraInfo = response.AnswerID;

                if (newExtraInfo != oldExtraInfo)
                  PopulateForm(PollModel, CurrRespondent, CurrQuestion, true, false);

                break;

              case "TextBoxPP":
                choiceIdx = (sender as TextBoxPP).SelectedIndex;
                choiceID = PollModel.Questions[CurrQuestion].Choices[choiceIdx].ID;
                response.ExtraText = (sender as TextBoxPP).Text.Trim();
                break;

              default:
                Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "frmPoll.ChoiceHandler - Standard");
                break;
            }
            break;


          case AnswerFormat.List:
            switch (ctrlType)
            {
              case "ListBoxPP":
                choiceIdx = (sender as ListBoxPP).SelectedIndex;
                choice = question.Choices[choiceIdx];
                choiceID = choice.ID;
                response.AnswerID = choiceID.ToString();   // Store newly selected choice

                // Now see if we need to repopulate the form
                if (choice.ExtraInfo)
                  newExtraInfo = response.AnswerID;

                if (newExtraInfo != oldExtraInfo)
                  PopulateForm(PollModel, CurrRespondent, CurrQuestion, true, false);

                break;

              case "TextBoxPP":
                choiceIdx = (sender as TextBoxPP).SelectedIndex;
                choiceID = PollModel.Questions[CurrQuestion].Choices[choiceIdx].ID;
                response.ExtraText = (sender as TextBoxPP).Text.Trim();
                break;

              default:
                Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "frmPoll.ChoiceHandler - List");
                break;
            }
            break;


          case AnswerFormat.DropList:
            switch (ctrlType)
            {
              case "ComboBoxPP":
                choiceIdx = (sender as ComboBoxPP).SelectedIndex - 1;  // Need to subtract 1 to account for intro instructions
                if (choiceIdx == -1)
                {
                  newExtraInfo = "";
                  response.AnswerID = "";
                }
                else
                {
                  choice = question.Choices[choiceIdx];
                  choiceID = choice.ID;
                  response.AnswerID = choiceID.ToString();   // Store newly selected choice

                  // Now see if we need to repopulate the form
                  if (choice.ExtraInfo)
                    newExtraInfo = response.AnswerID;
                }

                if (newExtraInfo != oldExtraInfo)
                  PopulateForm(PollModel, CurrRespondent, CurrQuestion, true, false);

                break;

              case "TextBoxPP":
                choiceIdx = (sender as TextBoxPP).SelectedIndex;
                choiceID = PollModel.Questions[CurrQuestion].Choices[choiceIdx].ID;
                response.ExtraText = (sender as TextBoxPP).Text.Trim();
                break;

              default:
                Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "frmPoll.ChoiceHandler - DropList");
                break;
            }
            break;


          case AnswerFormat.MultipleChoice:
            switch (ctrlType)
            {
              case "CheckBoxPP":
              case "LinkLabelPP":
                bool checkSet = false;
                if (ctrlType == "CheckBoxPP")
                {
                  choiceIdx = (sender as CheckBoxPP).SelectedIndex;
                  checkSet = (sender as CheckBoxPP).Checked;
                }
                else
                {
                  choiceIdx = (sender as LinkLabelPP).SelectedIndex;
                  checkSet = ((sender as LinkLabelPP).AssocControl as CheckBoxPP).Checked;
                }
                
                choice = question.Choices[choiceIdx];
                choiceID = choice.ID;

                // If the checkbox is being checked then record this new item in response.AnswerID; if being unchecked then remove item from response.AnswerID
                StoreMultipleChoice(response, choiceID.ToString(), checkSet, question.Choices);   // Update response.AnswerID by reference

                // Determine which checkboxes, that have an ExtraInfo textbox attached, are now currently checked
                newExtraInfo = GetExtraInfoSelections(response, question.Choices); 

                if (newExtraInfo != oldExtraInfo)
                  PopulateForm(PollModel, CurrRespondent, CurrQuestion, true, false);

                break;

              case "TextBoxPP":
                // If we enter here then, by definition, the associated checkbox will be checked
                TextBoxPP textBox = sender as TextBoxPP;
                choiceIdx = textBox.SelectedIndex;
                choiceID = PollModel.Questions[CurrQuestion].Choices[choiceIdx].ID;
                
                if (response.ExtraText != "")
                  response.ExtraText += ",";

                //response.ExtraText += choiceID.ToString() + "~" + (sender as TextBoxPP).Text.Trim();
                StoreExtraTextData(response, choiceID, textBox.Text.Trim());

                break;

              default:
                Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "frmPoll.ChoiceHandler - MultipleChoice");
                break;
            }
            break;


          case AnswerFormat.Range:
          switch (ctrlType)
          {
            case "RadioButtonPP":
            case "LinkLabelPP":
              if (ctrlType == "RadioButtonPP")
                choiceIdx = (sender as RadioButtonPP).SelectedIndex;
              else
                choiceIdx = (sender as LinkLabelPP).SelectedIndex;

              choice = question.Choices[choiceIdx];
              choiceID = choice.ID;
              response.AnswerID = choiceID.ToString();   // Store newly selected choice

              // Note: Since there are no optional text boxes allowed with this format,
              //       we don't need to text for anything else, like with the other formats.
              break;

            default:
              Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "frmPoll.ChoiceHandler - Range");
              break;
          }
          break;


          case AnswerFormat.MultipleBoxes:
            if (ctrlType == "TextBoxPP")
            {
              TextBoxPP textBox = sender as TextBoxPP;
              string text = textBox.Text.Trim();

              choiceIdx = textBox.SelectedIndex;
              choiceID = PollModel.Questions[CurrQuestion].Choices[choiceIdx].ID;

              // If a textbox is filled (or emptied) then we'll record (or remove) its Choice.ID.
              StoreMultipleChoice(response, choiceID.ToString(), (text != ""), question.Choices);   // Update response.AnswerID by reference

              // And also store the actual text value
              StoreExtraTextData(response, choiceID, text);
            }
            else
              Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "frmPoll.ChoiceHandler - MultipleBoxes");

            break;


          case AnswerFormat.Freeform:
            if (ctrlType == "TextBoxPP")
            {
              if ((sender as TextBoxPP).Text == "")
              {
                response.AnswerID = "";
                response.ExtraText = "";
              }
              else
              {
                response.AnswerID = "0";
                response.ExtraText = (sender as TextBoxPP).Text;
              }
            }
            else
              Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "frmPoll.ChoiceHandler - Freeform");

            break;


          case AnswerFormat.Spinner:
            if (ctrlType == "NumericUpDownPP")
            {
              response.AnswerID = (sender as NumericUpDownPP).Value.ToString();
              this.Focus();   // This keeps the cursor out of the spinner's textbox (more visually appealing)
            }
            else
              Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "frmPoll.ChoiceHandler - Spinner");

            break;


          default:
            break;
        }
      }

      catch (Exception ex)
      {
        Debug.Fail("Error preparing available choices: " + ex.Message, "frmPoll.ChoiceHandler");
      }
    }


    
    /// <summary>
    /// This method is currently only used by AnswerFormat.MultipleChoice.  It examines Response.AnswerID
    /// and then creates a new CDF string based on which of those have an ExtraInfo textbox assocated with
    /// them.  In other words, it creates a CDF string of those checkboxes that were selected and have an
    /// ExtraInfo text box.  Back in the calling routine, this info is compared with the current selections
    /// to see if the screen needs to be redrawn.
    /// </summary>
    /// <param name="answerIDs"></param>
    /// <param name="choices"></param>
    /// <returns></returns>
    private string GetExtraInfoSelections(_Response response, _Choices choices)
    {
      if (response.AnswerID == "")  // Prelim check
        return "";

      string answerIDs = "," + response.AnswerID + ",";   // Retrieve current AnswerIDs and add sentinels
      string retval = "";

      foreach (_Choice choice in choices)
      {
        if (choice.ExtraInfo)
        {
          string choiceID = choice.ID.ToString();
          if (answerIDs.IndexOf("," + choiceID + ",") != -1)
            retval += choiceID + ",";
        }
      }

      return retval.TrimEnd(new char[] {','});   // Remove trailing comma, if there is one
    }


    /// <summary>
    /// This method is used by AnswerFormat.MultipleChoice and MultipleBoxes.  It stores
    /// the AnswerID of each choice in a CDF manner in the response.AnswerID field.
    /// </summary>
    /// <param name="response"></param>
    /// <param name="sChoiceID"></param>
    /// <param name="checkSet"></param>
    /// <param name="choices"></param>
    private void StoreMultipleChoice(_Response response, string sChoiceID, bool checkSet, _Choices choices)
    {
      string answerIDs = "";

      if (checkSet)   // Selecting checkbox anew
        if (response.AnswerID == "")
          answerIDs = "," + sChoiceID + ",";
        else
          answerIDs = "," + response.AnswerID + "," + sChoiceID + ",";
      
      else            // Deselecting checkbox
      {
        answerIDs = "," + response.AnswerID + ",";                   // Add sentinels
        answerIDs = answerIDs.Replace("," + sChoiceID + ",", ",");   // Remove deselected ChoiceID
      }

      // The list now contains the right items but we need to ensure that the ChoiceIDs are in the same order as the checkboxes are arranged
      string newAnswerIDs = "";
      foreach (_Choice choice in choices)
      {
        if (answerIDs.IndexOf("," + choice.ID.ToString() + ",") != -1)
          newAnswerIDs += choice.ID.ToString() + ",";
      }

      response.AnswerID = newAnswerIDs.TrimEnd(new char[] {','});
    }



    private void StoreExtraTextData(_Response response, int choiceID, string newText)
    {
      string extraText = response.ExtraText;                                   // ex. "15~abcd,2~defg"
      string sID = choiceID.ToString();                                        // ex. "2"

      if (extraText == "")
        extraText += sID + "~" + newText;                                    
      
      else
      {
        extraText = Tools.EnsurePrefix(Tools.EnsureSuffix(extraText, ","), ",");     // ex. ",15~abcd,2~defg,"

        int pos = extraText.IndexOf("," + sID + "~");                          // ex. 8
        if (pos == -1)
        {
          // There was no ExtraText associated with this ChoiceID so we can just append it
          if (newText != "")   // But first we need to ensure that it isn't a null string
            extraText += sID + "~" + newText;
        }
        else
        {
          int pos2 = extraText.IndexOf(",", pos + 1);   // Find next comma     // ex. 15
          string oldEncoded = extraText.Substring(pos + 1, pos2 - pos - 1);    // ex. "2~defg"

          if (newText == "")
            extraText = extraText.Replace(oldEncoded + ",", "");               // ex. ",15~abcd,"
          else
            extraText = extraText.Replace(oldEncoded, sID + "~" + newText);    // ex. ",15~abcd,2~xyz,"
        }
      }

      extraText = extraText.Trim(new char[] {','});                          // Remove sentinels

      if (response.ExtraText != extraText)  // Since events can be fired, we don't want to set the same value back to the data model
        response.ExtraText = extraText;
    }

    #endregion



    private void TestCode()
    {

    }








	}
}
