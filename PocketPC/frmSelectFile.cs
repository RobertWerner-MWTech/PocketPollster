using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using OpenNETCF.Windows.Forms;

using DataObjects;



namespace PocketPC
{
  // Define Aliases
  using CFSysInfo = DataObjects.CFSysInfo;
  using Platform = DataObjects.Constants.MobilePlatform;
  using SelectFileMode = DataObjects.Constants.SelectFileMode;
  using Orientation = DataObjects.Constants.DeviceOrientation;
  using _ActivePollSummary = _Summaries._ActivePollSummary;



	/// <summary>
	/// This module is used to select a PP data file to open.  It is used to both run polls and review polls.
	/// </summary>
	
  public class frmSelectFile : System.Windows.Forms.Form
  {
    private OpenNETCF.Windows.Forms.ListBoxEx listBoxFiles;
    private System.Windows.Forms.Label labelInstructions;
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.MenuItem menuItemBack;
    private System.Windows.Forms.MenuItem menuItemNext;
    private OpenNETCF.Windows.Forms.TextBoxEx textBoxSummary;
    private System.Windows.Forms.Label labelQuestions;
    private System.Windows.Forms.TextBox textBoxQuestions;
    private System.Windows.Forms.Label labelRespondents;
    private System.Windows.Forms.TextBox textBoxRespondents;        // Set by the 'newPoll' parameter, passed to the constructor


    // Public fields
    public string PollName;              // This field will be populated with the full pathname of a selected path or contain "" if no file was selected
    public bool OkayToShow;              // Set true if there are files to display in dialog box

    // Private fields
    private string FileFolder = "";      // Set by the 'folder' parameter, passed to the constructor
    private ArrayList FullPathInfo;      // Contains the full path of the items in the listbox
    private bool NewPoll = false;
    private System.Windows.Forms.TextBox textBoxRevision;
    private System.Windows.Forms.Label labelRevision;
    private SelectFileMode OpenMode;     // Set by the 'mode' parameter, passed to the constructor


    /// <summary>
    /// Constructors
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="mode"></param>   true - Open to Add to       false - Open for Review
    /// <param name="newPoll"></param>
    public frmSelectFile(string folder, SelectFileMode mode, bool newPoll)
    {
      SelectFile(folder, mode, newPoll);    
    }

    public frmSelectFile(string folder, SelectFileMode mode)
    {
      SelectFile(folder, mode, false);
    }


    /// <summary>
    /// Initiation method
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="mode"></param>
    /// <param name="newPoll"></param>
    private void SelectFile(string folder, SelectFileMode mode, bool newPoll)
    {
      Cursor.Current = Cursors.WaitCursor;
      InitializeComponent();

      OpenMode = mode;   // Make available throughout module
      PollName = "";     // Initialize public variable
      FileFolder = Tools.EnsureFullPath(folder);
      FullPathInfo = new ArrayList();

      if (mode == SelectFileMode.RunPoll)
      {
        labelInstructions.Text = (newPoll) ? "Which poll would you like to run:" : "Which poll would you like to resume:";
        NewPoll = newPoll;   // Make available throughout module

        // Set visibility of Respondent info depending on setting of 'newPoll'
        labelRespondents.Visible = !newPoll;
        textBoxRespondents.Visible = !newPoll;

        string[] availFiles = Directory.GetFiles(folder, "*" + Tools.GetAppExt());

        if (availFiles.Length > 0)
        {
          OkayToShow = true;
          Array.Sort(availFiles);
          PopulateFileList(availFiles);
          Cursor.Current = Cursors.Default;
          this.BringToFront();
        }
        else
        {
          string caption = "";
          string msg = "";

          if (newPoll)
          {
            caption = "No Available Templates";
            msg = "There are no poll templates available.  You first need to sync your mobile device to obtain the current set of templates.";
          }
          else
          {
            caption = "No Polls Available";
            msg = "There are no previous polls available to open.  You must start a new poll.";
          }

          OkayToShow = false;
          PollName = "";   // Need to reset before leaving
          Cursor.Current = Cursors.Default;
          Tools.ShowMessage(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }

      else   // Must be 'ReviewData'
      {
        labelInstructions.Text = "Which poll would you like to review:";

        // Prepare ImageList of icons to differentiate different kinds of files
        listBoxFiles.ImageList = new ImageList();
        Icon icon = Multimedia.Images.GetIcon("ReviewPoll0q");     // 0 - Blank icon (for no questions)
        listBoxFiles.ImageList.Images.Add(icon);        

        icon = Multimedia.Images.GetIcon("ReviewPoll0r");          // 1 - Blank icon (for no respondents)
        listBoxFiles.ImageList.Images.Add(icon);        

        icon = Multimedia.Images.GetIcon("ReviewPoll1");           // 2 - Green triangle (like a "spare" in bowling)
        listBoxFiles.ImageList.Images.Add(icon);        

        icon = Multimedia.Images.GetIcon("ReviewPoll2");           // 3 - Blue square (like a "strike" in bowling)
        listBoxFiles.ImageList.Images.Add(icon);

        icon = Multimedia.Images.GetIcon("ReviewPoll3");           // 4 - Red circle with line through it
        listBoxFiles.ImageList.Images.Add(icon);

        // There may be files to review in either the 'Data' or 'Completed' folders,
        // so we need to examine both.  In the listbox, we'll represent each type
        // with a different kind of symbol.
        string filter = "*" + Tools.GetAppExt();
        string[] availFiles = Directory.GetFiles(folder, filter);                       // Data folder
        string[] availFiles2 = Directory.GetFiles(FileFolder + "Completed", filter);    // Completed sub-folder

        if (availFiles.Length + availFiles2.Length > 0)
        {
          OkayToShow = true;
          
          if (availFiles.Length > 0)
          {
            Array.Sort(availFiles);
            PopulateFileList(availFiles, 2);   // Display with green triangle
          }

          if (availFiles2.Length > 0)
          {
            Array.Sort(availFiles2);
            PopulateFileList(availFiles2, 3);  // Display with blue square
          }

          Cursor.Current = Cursors.Default;
          this.BringToFront();
        }
        else
        {
          string caption = "No Polls Available";
          string msg = "There are no previous polls available to review.";
  
          OkayToShow = false;
          PollName = "";   // Need to reset before leaving
          Cursor.Current = Cursors.Default;
          Tools.ShowMessage(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }

      InitializeScreen();   // This must be at the end or for some reason the listBoxFiles height doesn't "stick"
      this.Resize += new System.EventHandler(this.frmSelectFile_Resize);
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmSelectFile));
      this.labelInstructions = new System.Windows.Forms.Label();
      this.listBoxFiles = new OpenNETCF.Windows.Forms.ListBoxEx();
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemBack = new System.Windows.Forms.MenuItem();
      this.menuItemNext = new System.Windows.Forms.MenuItem();
      this.textBoxSummary = new OpenNETCF.Windows.Forms.TextBoxEx();
      this.labelQuestions = new System.Windows.Forms.Label();
      this.textBoxQuestions = new System.Windows.Forms.TextBox();
      this.labelRespondents = new System.Windows.Forms.Label();
      this.textBoxRespondents = new System.Windows.Forms.TextBox();
      this.textBoxRevision = new System.Windows.Forms.TextBox();
      this.labelRevision = new System.Windows.Forms.Label();
      // 
      // labelInstructions
      // 
      this.labelInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelInstructions.ForeColor = System.Drawing.Color.Blue;
      this.labelInstructions.Location = new System.Drawing.Point(15, 8);
      this.labelInstructions.Size = new System.Drawing.Size(200, 16);
      this.labelInstructions.Text = "Select A File To Open:";
      // 
      // listBoxFiles
      // 
      this.listBoxFiles.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(237)), ((System.Byte)(240)), ((System.Byte)(245)));
      this.listBoxFiles.DataSource = null;
      this.listBoxFiles.DisplayMember = null;
      this.listBoxFiles.EvenItemColor = System.Drawing.Color.FromArgb(((System.Byte)(237)), ((System.Byte)(240)), ((System.Byte)(245)));
      this.listBoxFiles.ForeColor = System.Drawing.SystemColors.ControlText;
      this.listBoxFiles.ImageList = null;
      this.listBoxFiles.ItemHeight = 16;
      this.listBoxFiles.LineColor = System.Drawing.Color.Black;
      this.listBoxFiles.Location = new System.Drawing.Point(16, 29);
      this.listBoxFiles.SelectedIndex = -1;
      this.listBoxFiles.ShowLines = false;
      this.listBoxFiles.ShowScrollbar = true;
      this.listBoxFiles.Size = new System.Drawing.Size(208, 88);
      this.listBoxFiles.TopIndex = 0;
      this.listBoxFiles.WrapText = false;
      this.listBoxFiles.SelectedIndexChanged += new System.EventHandler(this.listBoxFiles_SelectedIndexChanged);
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
      this.menuItemNext.Enabled = false;
      this.menuItemNext.Text = "Next >";
      this.menuItemNext.Click += new System.EventHandler(this.menuItemNext_Click);
      // 
      // textBoxSummary
      // 
      this.textBoxSummary.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(198)), ((System.Byte)(212)), ((System.Byte)(232)));
      this.textBoxSummary.Location = new System.Drawing.Point(16, 154);
      this.textBoxSummary.Multiline = true;
      this.textBoxSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxSummary.Size = new System.Drawing.Size(208, 110);
      this.textBoxSummary.Style = OpenNETCF.Windows.Forms.TextBoxStyle.Default;
      this.textBoxSummary.Text = "";
      this.textBoxSummary.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
      // 
      // labelQuestions
      // 
      this.labelQuestions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelQuestions.ForeColor = System.Drawing.Color.Black;
      this.labelQuestions.Location = new System.Drawing.Point(86, 128);
      this.labelQuestions.Size = new System.Drawing.Size(41, 16);
      this.labelQuestions.Text = "Quest:";
      // 
      // textBoxQuestions
      // 
      this.textBoxQuestions.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(198)), ((System.Byte)(212)), ((System.Byte)(232)));
      this.textBoxQuestions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.textBoxQuestions.Location = new System.Drawing.Point(126, 126);
      this.textBoxQuestions.Size = new System.Drawing.Size(25, 20);
      this.textBoxQuestions.Text = "";
      this.textBoxQuestions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
      // 
      // labelRespondents
      // 
      this.labelRespondents.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelRespondents.ForeColor = System.Drawing.Color.Black;
      this.labelRespondents.Location = new System.Drawing.Point(15, 128);
      this.labelRespondents.Size = new System.Drawing.Size(35, 16);
      this.labelRespondents.Text = "Resp:";
      // 
      // textBoxRespondents
      // 
      this.textBoxRespondents.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(198)), ((System.Byte)(212)), ((System.Byte)(232)));
      this.textBoxRespondents.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.textBoxRespondents.Location = new System.Drawing.Point(49, 126);
      this.textBoxRespondents.Size = new System.Drawing.Size(25, 20);
      this.textBoxRespondents.Text = "";
      this.textBoxRespondents.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_KeyPress);
      // 
      // textBoxRevision
      // 
      this.textBoxRevision.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(198)), ((System.Byte)(212)), ((System.Byte)(232)));
      this.textBoxRevision.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.textBoxRevision.Location = new System.Drawing.Point(199, 126);
      this.textBoxRevision.Size = new System.Drawing.Size(25, 20);
      this.textBoxRevision.Text = "";
      // 
      // labelRevision
      // 
      this.labelRevision.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelRevision.ForeColor = System.Drawing.Color.Black;
      this.labelRevision.Location = new System.Drawing.Point(168, 128);
      this.labelRevision.Size = new System.Drawing.Size(31, 16);
      this.labelRevision.Text = "Rev:";
      // 
      // frmSelectFile
      // 
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ControlBox = false;
      this.Controls.Add(this.textBoxRevision);
      this.Controls.Add(this.labelRevision);
      this.Controls.Add(this.textBoxRespondents);
      this.Controls.Add(this.labelRespondents);
      this.Controls.Add(this.textBoxQuestions);
      this.Controls.Add(this.labelQuestions);
      this.Controls.Add(this.textBoxSummary);
      this.Controls.Add(this.listBoxFiles);
      this.Controls.Add(this.labelInstructions);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";
      this.Closed += new System.EventHandler(this.frmSelectFile_Closed);

    }
		#endregion


    private void PopulateFileList(string[] fileList)
    {
      PopulateFileList(fileList, -1);  // No icon
    }

    private void PopulateFileList(string[] fileList, int baseIndex)
    {
      foreach(string filename in fileList)
      {
        if (baseIndex == -1)
          listBoxFiles.Items.Add(Tools.StripPathAndExtension(filename));
        else
        {
          string basename = Tools.StripPathAndExtension(filename);
          int index = CheckListImageIndex(basename, baseIndex);   // Check whether the assumed ListImage Index needs to be changed
          ListItem listItem = new ListItem(basename, index);
          listBoxFiles.Items.Add(listItem);
        }
        FullPathInfo.Add(filename);
      }
    }


    /// <summary>
    /// When files are presented for ReviewData, different icons are placed beside each filename.
    /// The basic symbol for 'In Progress' files is a green triangle.  The basic symbol for 'Completed'
    /// files is a blue square.  Special circumstances dictate that different symbols than these are used:
    ///   - 
    /// </summary>
    /// <param name="basename"></param>
    /// <param name="baseIndex"></param>
    /// <returns></returns>
    private int CheckListImageIndex(string basename, int baseIndex)
    {
      int index = baseIndex;   // Default value

      _ActivePollSummary activePollSummary = CFSysInfo.Data.Summaries.ActivePolls.Find_PollSummary(basename);

      if (activePollSummary != null)
      {
        if (activePollSummary.NumQuestions == 0)
          index = 0;

        else if (activePollSummary.NumResponses == 0)
          index = 1;

        else if (!activePollSummary.ReviewData)
          index = 4;
      }

      return index;
    }


    private void InitializeScreen()
    {
      RepositionControls(true);

      if (listBoxFiles.Items.Count == 1)
        listBoxFiles.SelectedIndex = 0;
    }


    private void frmSelectFile_Resize(object sender, System.EventArgs e)
    {
      RepositionControls(false);
    }


    private void RepositionControls(bool forceRedraw)
    {
      ToolsCF.SipShowIM(0);       // Ensure that SIP is collapsed
      int wid = 0, hgt = 0, aHgt = 0;

      if ((ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt)) | forceRedraw)
      {
        int gap = 4;   // A small value, used as the basis for providing horiz and vert spacing
        double daHgt = Convert.ToDouble(aHgt);

        // First position top label
        labelInstructions.Location = new Point(gap * 4, gap * 2);
        Font font = labelRespondents.Font;   // Base font for all labels

        // Logic to handle display of the two listboxes
        if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
        {
          // To Do: Future code to go here

        }
        else
        {
          if (hgt > wid)   // Portrait
          {
            listBoxFiles.Location = new Point(gap * 4, gap * 7);
            listBoxFiles.Size = new Size(wid - gap * 8, Convert.ToInt32(daHgt * 0.45));
            AdjustSummaryInfoLocations(Orientation.Portrait);

            textBoxSummary.Location = new Point(listBoxFiles.Left, textBoxQuestions.Bottom + gap * 2);
            textBoxSummary.Size = new Size(wid - gap * 8, aHgt - gap * 2 - textBoxSummary.Top);
          }

          else             // Landscape
          {
            listBoxFiles.Location = new Point(gap * 4, Convert.ToInt32(daHgt * 0.15));
            listBoxFiles.Size = new Size(wid / 2 - gap * 5, Convert.ToInt32(daHgt * 0.7));

            textBoxSummary.Location = new Point(listBoxFiles.Right + gap * 2, listBoxFiles.Top);
            textBoxSummary.Size = listBoxFiles.Size;
            AdjustSummaryInfoLocations(Orientation.Landscape);
          }
        }
      }
    }


    /// <summary>
    /// Adjusts the position of the controls holding the # of Respondents, # of Questions, and Revision #
    /// </summary>
    /// <param name="orientation"></param>
    private void AdjustSummaryInfoLocations(Orientation orientation)
    {
      int gap = 4;

      if (orientation == Orientation.Portrait)
      {
        if (!NewPoll)
        {
          ToolsCF.AutoSizeLabelText(labelRespondents, "Resp:");
          labelRespondents.Location = new Point(listBoxFiles.Left, listBoxFiles.Bottom + gap * 2 + 2);
          textBoxRespondents.Location = new Point(labelRespondents.Right + gap, labelRespondents.Top - 3);

          ToolsCF.AutoSizeLabelText(labelQuestions, "Quest:");
          labelQuestions.Location = new Point(textBoxRespondents.Right + gap * 4, labelRespondents.Top);
          textBoxQuestions.Location = new Point(labelQuestions.Right + gap, textBoxRespondents.Top);

          textBoxRevision.Location = new Point(listBoxFiles.Right - gap - textBoxRevision.Width, textBoxQuestions.Top);
          ToolsCF.AutoSizeLabelText(labelRevision, "Rev:");
          labelRevision.Location = new Point(textBoxRevision.Left - gap - labelRevision.Width, labelQuestions.Top);
        }

        else
        {
          ToolsCF.AutoSizeLabelText(labelQuestions, "Questions:");
          labelQuestions.Location = new Point(listBoxFiles.Left, listBoxFiles.Bottom + gap * 2 + 2);
          textBoxQuestions.Location = new Point(labelQuestions.Right + gap, labelQuestions.Top - 3);

          textBoxRevision.Location = new Point(listBoxFiles.Right - gap - textBoxRevision.Width, textBoxQuestions.Top);
          ToolsCF.AutoSizeLabelText(labelRevision, "Revision:");
          labelRevision.Location = new Point(textBoxRevision.Left - gap - labelRevision.Width, labelQuestions.Top);
        }
      }

      else   // Landscape mode
      {
        if (!NewPoll)
        {
          ToolsCF.AutoSizeLabelText(labelRespondents, "Responses:");
          labelRespondents.Location = new Point(listBoxFiles.Left, listBoxFiles.Bottom + gap * 2);
          textBoxRespondents.Location = new Point(labelRespondents.Right + gap, labelRespondents.Top - 3);

          ToolsCF.AutoSizeLabelText(labelQuestions, "Questions:");
          labelQuestions.Location = new Point(textBoxRespondents.Right + gap * 5, labelRespondents.Top);
          textBoxQuestions.Location = new Point(labelQuestions.Right + gap, textBoxRespondents.Top);

          textBoxRevision.Location = new Point(textBoxSummary.Right - gap - textBoxRevision.Width, textBoxQuestions.Top);
          ToolsCF.AutoSizeLabelText(labelRevision, "Revision:");
          labelRevision.Location = new Point(textBoxRevision.Left - gap - labelRevision.Width, labelQuestions.Top);
        }
        else
        {
          ToolsCF.AutoSizeLabelText(labelQuestions, "Questions:");
          labelQuestions.Location = new Point(listBoxFiles.Left + gap * 4, listBoxFiles.Bottom + gap * 2);
          textBoxQuestions.Location = new Point(labelQuestions.Right + gap, labelQuestions.Top - 3);

          textBoxRevision.Location = new Point(textBoxSummary.Right - gap * 8 - textBoxRevision.Width, textBoxQuestions.Top);
          ToolsCF.AutoSizeLabelText(labelRevision, "Revision:");
          labelRevision.Location = new Point(textBoxRevision.Left - gap - labelRevision.Width, labelQuestions.Top);
        }
      }
    }


    private void listBoxFiles_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      //Cursor.Current = Cursors.WaitCursor;   // Now, we'll only show the wait cursor if we have to interrogate the file

      // Dynamically display info about Poll
      ListBoxEx listBox = sender as ListBoxEx;

      if (listBox.SelectedIndex < listBox.Items.Count)  // We need to check because for some reason, non-existent indices are sometimes returned!
      {
        string filename = FullPathInfo[listBox.SelectedIndex].ToString();

        string summary = "";
        int respondentCount = 0;
        int questionCount = 0;
        int revision = 0;
        bool reviewData = true;

        // Retrieve pertinent info about this poll
        GetPollDetails(filename, out revision, out summary, out reviewData, out questionCount, out respondentCount);

        // If no questions or no respondents then change icon image.  We'll still allow user 
        // to hit "Next" but then inform them why they can't proceed.
        if (questionCount == 0)
        {
          listBox.Items[listBox.SelectedIndex].ImageIndex = 0;
          textBoxSummary.Text = summary;
          menuItemNext.Enabled = true;
        }
        
        else if (respondentCount == 0)
        {
          listBox.Items[listBox.SelectedIndex].ImageIndex = 1;
          textBoxSummary.Text = summary;
          menuItemNext.Enabled = true;
        }

        // If we're reviewing data and ReviewData isn't allowed then display red circle with line through it.  Also disable Next.
        else if (OpenMode == SelectFileMode.ReviewData && !reviewData)
        {
          listBox.Items[listBox.SelectedIndex].ImageIndex = 4;
          textBoxSummary.Text = "Sorry, but the review of this poll's data has been disabled.";
          menuItemNext.Enabled = false;
        }

        else
        {
          textBoxSummary.Text = summary;
          menuItemNext.Enabled = true;
        }

        textBoxRespondents.Text = " " + respondentCount.ToString();
        textBoxQuestions.Text = " " + questionCount.ToString();        // Nothing but LeftAlign allowed so the space will compensate some
        textBoxRevision.Text = " " + revision.ToString();
      }

      else
      {
        listBox.SelectedIndex = -1;
        textBoxSummary.Text = "";
        textBoxQuestions.Text = "";
        textBoxRespondents.Text = "";
        textBoxRevision.Text = "";

        menuItemNext.Enabled = false;
      }

      Cursor.Current = Cursors.Default;
    }



    /// <summary>
    /// Retrieves the top-level information about the specified poll.  First it tries to get it from CFSysInfo, but if it's not
    /// present then it queries the file itself for the data, and populates CFSysInfo to save this extra step the next time.
    /// </summary>
    /// <param name="filename"></param>          // This is the full pathname
    /// <param name="revision"></param>
    /// <param name="pollSummary"></param>
    /// <param name="reviewData"></param>
    /// <param name="questionCount"></param>
    /// <param name="respondentCount"></param>
    public void GetPollDetails(string filename, out int revision, out string pollSummary, out bool reviewData, out int questionCount, out int respondentCount)
    {
      string basename = Tools.StripPathAndExtension(filename);

      // Assign default values to out parameters
      pollSummary = "";
      revision = 0;
      reviewData = true;
      questionCount = 0;
      respondentCount = 0;   // Note: Not used by Templates, only Active Polls

      // See if we're starting a new poll, and thus looking at templates
      if (OpenMode == SelectFileMode.RunPoll && NewPoll)
      {
        _TemplateSummary templateSummary = CFSysInfo.Data.Summaries.Templates.Find_Template(basename);
        if (templateSummary != null)
        {
          revision = templateSummary.Revision;
          pollSummary = templateSummary.PollSummary;
          questionCount = templateSummary.NumQuestions;
        }
        else  // Not in Summaries yet so get directly from the poll itself and then store this info in Summaries
        {
          Cursor.Current = Cursors.WaitCursor;
          string pollGuid = "";
          string lastEditGuid = "";
          Tools.GetPollOverview(filename, out revision, out pollSummary, out questionCount, out pollGuid, out lastEditGuid);
          templateSummary = new _TemplateSummary(basename, revision, pollSummary, questionCount, pollGuid, lastEditGuid);
          CFSysInfo.Data.Summaries.Templates.Add(templateSummary);
        }
      }

      else  // Either Resuming an existing poll (in Data) or Reviewing a poll (in Data)
      {
        _ActivePollSummary activePollSummary = CFSysInfo.Data.Summaries.ActivePolls.Find_PollSummary(basename);

        if (activePollSummary != null)
        {
          pollSummary = activePollSummary.PollSummary;
          revision = activePollSummary.Revision;
          reviewData = activePollSummary.ReviewData;
          questionCount = activePollSummary.NumQuestions;
          respondentCount = activePollSummary.NumResponses;
        }
        else  // Not in Summaries yet so get directly from the poll itself and then store this info in Summaries
        {
          Cursor.Current = Cursors.WaitCursor;
          Tools.GetPollOverview(filename, out revision, out pollSummary, out reviewData, out questionCount, out respondentCount);
          activePollSummary = new _ActivePollSummary(basename, revision, pollSummary, reviewData, questionCount, respondentCount);
          CFSysInfo.Data.Summaries.ActivePolls.Add(activePollSummary);
        }
      }
    }



    /// <summary>
    /// This event handler cancels all characters entered into any textbox, thus effectively making it read-only.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void textBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      e.Handled = true;  // This will cancel all characters entered into the textbox, thus effectively making it read-only
    }


    // Note: This is equivalent to 'Cancel' in a traditional dialog box.
    private void menuItemBack_Click(object sender, System.EventArgs e)
    {
      this.Close();
    }


    // Note: This is equivalent to 'OK' in a traditional dialog box.
    private void menuItemNext_Click(object sender, System.EventArgs e)
    {
      bool cancelClose = false;    // If set true then closing of form will be aborted

      // A file was selected to be opened.  But what mode are we using?
      if (OpenMode == SelectFileMode.RunPoll)
      {
        PollName = listBoxFiles.Items[listBoxFiles.SelectedIndex].ToString() + Tools.GetAppExt();

        if (NewPoll)
        {
          // First see if there's an existing file in 'Data' with the same filename.  If so, then give
          // the user the opportunity to open it instead - something they may have been intending anyhow.
          if (File.Exists(CFSysInfo.Data.MobilePaths.Data + PollName))
          {
            string msg = "An existing poll with this name exists.  Would you like to continue using the existing poll instead?";
            DialogResult dialogResult = Tools.ShowMessage(msg, "Use Existing Poll?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            this.Refresh();   // This makes the MessageBox disappear immediately

            switch (dialogResult)
            {
              case DialogResult.Yes:
                PollName = CFSysInfo.Data.MobilePaths.Data + PollName;
                break;

              case DialogResult.No:
                PollName = FileFolder + PollName;
                break;

              default:   // Cancel
                PollName = "";
                cancelClose = true;
                break;
            }
          }
          else
            PollName = FileFolder + PollName;
        }

        else   // Opening existing poll
        {
          PollName = FileFolder + PollName;
        }
      }

      else   // We must be in Review mode
      {
        if (listBoxFiles.Items[listBoxFiles.SelectedIndex].ImageIndex < 2)
        {
          string msg;
          string title;

          if (listBoxFiles.Items[listBoxFiles.SelectedIndex].ImageIndex == 0)
          {
            msg = "This poll has no questions.  Please notify your system administrator!";
            title = "No Questions";
          }
          else
          {
            msg = "This poll has no responses yet, so there's nothing to review.";
            title = "No Responses";
          }

          Tools.ShowMessage(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          cancelClose = true;
        }

        else
          PollName = FullPathInfo[listBoxFiles.SelectedIndex].ToString();
      }

      if (! cancelClose)
        this.Close();
    }


    private void frmSelectFile_Closed(object sender, System.EventArgs e)
    {
      this.Resize -= new System.EventHandler(this.frmSelectFile_Resize);
    }




	}
}
