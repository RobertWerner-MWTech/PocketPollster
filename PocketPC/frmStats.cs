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
  using AnswerFormat = DataObjects.Constants.AnswerFormat;
  using CFSysInfo = DataObjects.CFSysInfo;
  using Platform = DataObjects.Constants.MobilePlatform;
  using SmartListItem = OpenNETCF.Windows.Forms.SmartListItem;


	/// <summary>
	/// Summary description for frmStats.
	/// </summary>
	public class frmStats : System.Windows.Forms.Form
	{
    private System.Windows.Forms.MenuItem menuItemBack;
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.TabPage tabPageResponses;
    private System.Windows.Forms.TabPage tabPageSummary;
    private System.Windows.Forms.TabControl tabMain;
    private System.Windows.Forms.Panel panelResponses;
    private System.Windows.Forms.Panel panelSummary;
    private System.Windows.Forms.Label labelRespondent;
    private System.Windows.Forms.TrackBar trackBarRespondent;
    private System.Windows.Forms.Label labelQuestion;
    private System.Windows.Forms.TrackBar trackBarQuestion2;
    private System.Windows.Forms.Label labelQuestion2;
    private System.Windows.Forms.Label labelSummary;
    private System.Windows.Forms.TrackBar trackBarQuestion;
    private System.Windows.Forms.NumericUpDown spinnerRespondent;
    private System.Windows.Forms.NumericUpDown spinnerQuestion;                
    private System.Windows.Forms.NumericUpDown spinnerQuestion2;
    private System.Windows.Forms.Label labelNamePrefix;
    private System.Windows.Forms.Label labelName;
    private OpenNETCF.Windows.Forms.GroupBox panelResults;
    private System.Windows.Forms.Label labelQuestionText;
    private System.Windows.Forms.Label labelDateTimePrefix;
    private System.Windows.Forms.Label labelDateTime;
    private System.Windows.Forms.Panel panelSpinner;
    private System.Windows.Forms.Label labelMinMax;
    private System.Windows.Forms.Label labelAnswer;
    private System.Windows.Forms.ListView listViewAnswers;
    private System.Windows.Forms.TextBox textBoxAnswer;
    private System.Windows.Forms.MenuItem menuItemDone;
    private OpenNETCF.Windows.Forms.GroupBox panelResults2;
    private System.Windows.Forms.ListView listViewAnswers2;
    private System.Windows.Forms.Label labelQuestionText2;
    private DataObjects.AutoScrollPanel panelFreeForm;

    private string PollName;
    private Poll PollModel;
    private int SeqCriteria;
    private ArrayList DateCriteria;
    private _Respondents Respondents;        // The modular level variable that contains the records being displayed
    private int gap = 4;                     // A small value, used as the basis for providing horiz and vert spacing
    private int smallColWidth = 40;          // The width of the smaller columns on the Summary tab page
    private bool StopRepopulation = false;   // Prevents 'PopulateForm' from being executed twice inadvertently


    // Form-level properties

    // This keeps track of what Question we're currently displaying
    private int currQuestion = 0;
  
    private int CurrQuestion         // 0-based index
    {
      get
      {
        return currQuestion;
      }
      set
      {  
        currQuestion = value;                                        // Set the new question number and
        PopulateForm(Respondents, CurrRespondent, currQuestion);     // then populate the form accordingly
      }
    }

    // This keeps track of which Respondent we're currently displaying.
    private int currRespondent = 0;
    private int CurrRespondent       // 0-based index
    {
      get
      {
        return currRespondent;
      }
      set
      {
        currRespondent = value;                                      // Set the new respondent number and
        PopulateForm(Respondents, currRespondent, CurrQuestion);     // then populate the form accordingly
      }
    }


    /// <summary>
    /// This is the constructor for this form.
    /// </summary>
    /// <param name="pollName"></param>
    /// <param name="pollModel"></param>
    /// <param name="seqCriteria"></param>    // A value representing which set of responses to display; 0 = All
    /// <param name="dateCriteria"></param>   // The date(s) the user wishes to display; An empty collection = All
		public frmStats(string pollName, Poll pollModel, int seqCriteria, ArrayList dateCriteria)
		{
      Cursor.Current = Cursors.WaitCursor;
			InitializeComponent();

      PollName = pollName;
      PollModel = pollModel;          // The module-level variable is needed by the 'Back' button
      SeqCriteria = seqCriteria;
      DateCriteria = dateCriteria;
      
      Respondents = FilterRespondents(pollModel, seqCriteria, dateCriteria);   // Determine exactly which records will be displayed
      InitializeScreen();

      ToolsCF.SipShowIM(0);           // Hide SIP in case it's visible
      this.BringToFront();
      Cursor.Current = Cursors.Default;

      this.Resize += new System.EventHandler(this.frmStats_Resize);
      this.ShowDialog();
      this.Resize -= new System.EventHandler(this.frmStats_Resize);
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmStats));
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemBack = new System.Windows.Forms.MenuItem();
      this.menuItemDone = new System.Windows.Forms.MenuItem();
      this.tabMain = new System.Windows.Forms.TabControl();
      this.tabPageResponses = new System.Windows.Forms.TabPage();
      this.panelResponses = new System.Windows.Forms.Panel();
      this.panelResults = new OpenNETCF.Windows.Forms.GroupBox();
      this.panelSpinner = new System.Windows.Forms.Panel();
      this.labelMinMax = new System.Windows.Forms.Label();
      this.labelAnswer = new System.Windows.Forms.Label();
      this.textBoxAnswer = new System.Windows.Forms.TextBox();
      this.listViewAnswers = new System.Windows.Forms.ListView();
      this.labelQuestionText = new System.Windows.Forms.Label();
      this.labelDateTime = new System.Windows.Forms.Label();
      this.labelName = new System.Windows.Forms.Label();
      this.spinnerQuestion = new System.Windows.Forms.NumericUpDown();
      this.spinnerRespondent = new System.Windows.Forms.NumericUpDown();
      this.trackBarRespondent = new System.Windows.Forms.TrackBar();
      this.trackBarQuestion = new System.Windows.Forms.TrackBar();
      this.labelQuestion = new System.Windows.Forms.Label();
      this.labelNamePrefix = new System.Windows.Forms.Label();
      this.labelDateTimePrefix = new System.Windows.Forms.Label();
      this.labelRespondent = new System.Windows.Forms.Label();
      this.tabPageSummary = new System.Windows.Forms.TabPage();
      this.panelSummary = new System.Windows.Forms.Panel();
      this.panelResults2 = new OpenNETCF.Windows.Forms.GroupBox();
      this.listViewAnswers2 = new System.Windows.Forms.ListView();
      this.labelQuestionText2 = new System.Windows.Forms.Label();
      this.spinnerQuestion2 = new System.Windows.Forms.NumericUpDown();
      this.labelSummary = new System.Windows.Forms.Label();
      this.trackBarQuestion2 = new System.Windows.Forms.TrackBar();
      this.labelQuestion2 = new System.Windows.Forms.Label();
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.Add(this.menuItemBack);
      this.mainMenu.MenuItems.Add(this.menuItemDone);
      // 
      // menuItemBack
      // 
      this.menuItemBack.Text = "< Back";
      this.menuItemBack.Click += new System.EventHandler(this.menuItemBack_Click);
      // 
      // menuItemDone
      // 
      this.menuItemDone.Text = "Done";
      this.menuItemDone.Click += new System.EventHandler(this.menuItemDone_Click);
      // 
      // tabMain
      // 
      this.tabMain.Controls.Add(this.tabPageResponses);
      this.tabMain.Controls.Add(this.tabPageSummary);
      this.tabMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.tabMain.SelectedIndex = 0;
      this.tabMain.Size = new System.Drawing.Size(240, 268);
      this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
      // 
      // tabPageResponses
      // 
      this.tabPageResponses.Controls.Add(this.panelResponses);
      this.tabPageResponses.Location = new System.Drawing.Point(4, 4);
      this.tabPageResponses.Size = new System.Drawing.Size(232, 242);
      this.tabPageResponses.Text = "Responses";
      // 
      // panelResponses
      // 
      this.panelResponses.BackColor = System.Drawing.Color.LightSteelBlue;
      this.panelResponses.Controls.Add(this.panelResults);
      this.panelResponses.Controls.Add(this.labelDateTime);
      this.panelResponses.Controls.Add(this.labelName);
      this.panelResponses.Controls.Add(this.spinnerQuestion);
      this.panelResponses.Controls.Add(this.spinnerRespondent);
      this.panelResponses.Controls.Add(this.trackBarRespondent);
      this.panelResponses.Controls.Add(this.trackBarQuestion);
      this.panelResponses.Controls.Add(this.labelQuestion);
      this.panelResponses.Controls.Add(this.labelNamePrefix);
      this.panelResponses.Controls.Add(this.labelDateTimePrefix);
      this.panelResponses.Controls.Add(this.labelRespondent);
      this.panelResponses.Size = new System.Drawing.Size(248, 248);
      // 
      // panelResults
      // 
      this.panelResults.Controls.Add(this.panelSpinner);
      this.panelResults.Controls.Add(this.textBoxAnswer);
      this.panelResults.Controls.Add(this.listViewAnswers);
      this.panelResults.Controls.Add(this.labelQuestionText);
      this.panelResults.Location = new System.Drawing.Point(4, 80);
      this.panelResults.Size = new System.Drawing.Size(224, 156);
      // 
      // panelSpinner
      // 
      this.panelSpinner.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(216)), ((System.Byte)(228)), ((System.Byte)(248)));
      this.panelSpinner.Controls.Add(this.labelMinMax);
      this.panelSpinner.Controls.Add(this.labelAnswer);
      this.panelSpinner.Location = new System.Drawing.Point(48, 91);
      this.panelSpinner.Size = new System.Drawing.Size(128, 56);
      // 
      // labelMinMax
      // 
      this.labelMinMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelMinMax.Location = new System.Drawing.Point(0, 8);
      this.labelMinMax.Size = new System.Drawing.Size(120, 16);
      this.labelMinMax.Text = "Min: 1        Max: 10";
      this.labelMinMax.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // labelAnswer
      // 
      this.labelAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
      this.labelAnswer.ForeColor = System.Drawing.Color.Blue;
      this.labelAnswer.Location = new System.Drawing.Point(0, 24);
      this.labelAnswer.Size = new System.Drawing.Size(120, 24);
      this.labelAnswer.Text = "5";
      this.labelAnswer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // textBoxAnswer
      // 
      this.textBoxAnswer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.textBoxAnswer.Location = new System.Drawing.Point(24, 64);
      this.textBoxAnswer.Multiline = true;
      this.textBoxAnswer.Size = new System.Drawing.Size(176, 20);
      this.textBoxAnswer.Text = "";
      this.textBoxAnswer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxAnswer_KeyPress);
      // 
      // listViewAnswers
      // 
      this.listViewAnswers.FullRowSelect = true;
      this.listViewAnswers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewAnswers.Location = new System.Drawing.Point(24, 24);
      this.listViewAnswers.Size = new System.Drawing.Size(176, 32);
      this.listViewAnswers.View = System.Windows.Forms.View.Details;
      this.listViewAnswers.SelectedIndexChanged += new System.EventHandler(this.listViewAnswers_SelectedIndexChanged);
      // 
      // labelQuestionText
      // 
      this.labelQuestionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelQuestionText.ForeColor = System.Drawing.Color.Black;
      this.labelQuestionText.Location = new System.Drawing.Point(4, 4);
      this.labelQuestionText.Size = new System.Drawing.Size(208, 30);
      this.labelQuestionText.Text = "Question:";
      // 
      // labelDateTime
      // 
      this.labelDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelDateTime.ForeColor = System.Drawing.Color.Blue;
      this.labelDateTime.Location = new System.Drawing.Point(168, 6);
      this.labelDateTime.Size = new System.Drawing.Size(64, 16);
      // 
      // labelName
      // 
      this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelName.ForeColor = System.Drawing.Color.Blue;
      this.labelName.Location = new System.Drawing.Point(43, 6);
      this.labelName.Size = new System.Drawing.Size(80, 16);
      // 
      // spinnerQuestion
      // 
      this.spinnerQuestion.Location = new System.Drawing.Point(172, 29);
      this.spinnerQuestion.Minimum = new System.Decimal(new int[] {
                                                                    1,
                                                                    0,
                                                                    0,
                                                                    0});
      this.spinnerQuestion.Size = new System.Drawing.Size(52, 20);
      this.spinnerQuestion.Value = new System.Decimal(new int[] {
                                                                  1,
                                                                  0,
                                                                  0,
                                                                  0});
      this.spinnerQuestion.ValueChanged += new System.EventHandler(this.spinnerQuestion_ValueChanged);
      // 
      // spinnerRespondent
      // 
      this.spinnerRespondent.Location = new System.Drawing.Point(39, 29);
      this.spinnerRespondent.Minimum = new System.Decimal(new int[] {
                                                                      1,
                                                                      0,
                                                                      0,
                                                                      0});
      this.spinnerRespondent.Size = new System.Drawing.Size(52, 20);
      this.spinnerRespondent.Value = new System.Decimal(new int[] {
                                                                    1,
                                                                    0,
                                                                    0,
                                                                    0});
      this.spinnerRespondent.ValueChanged += new System.EventHandler(this.spinnerRespondent_ValueChanged);
      // 
      // trackBarRespondent
      // 
      this.trackBarRespondent.LargeChange = 10;
      this.trackBarRespondent.Location = new System.Drawing.Point(4, 51);
      this.trackBarRespondent.Maximum = 100;
      this.trackBarRespondent.Minimum = 1;
      this.trackBarRespondent.Size = new System.Drawing.Size(94, 45);
      this.trackBarRespondent.Value = 1;
      this.trackBarRespondent.ValueChanged += new System.EventHandler(this.trackBarRespondent_ValueChanged);
      // 
      // trackBarQuestion
      // 
      this.trackBarQuestion.LargeChange = 10;
      this.trackBarQuestion.Location = new System.Drawing.Point(135, 51);
      this.trackBarQuestion.Maximum = 100;
      this.trackBarQuestion.Minimum = 1;
      this.trackBarQuestion.Size = new System.Drawing.Size(97, 45);
      this.trackBarQuestion.Value = 1;
      this.trackBarQuestion.ValueChanged += new System.EventHandler(this.trackBarQuestion_ValueChanged);
      // 
      // labelQuestion
      // 
      this.labelQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelQuestion.Location = new System.Drawing.Point(134, 32);
      this.labelQuestion.Size = new System.Drawing.Size(38, 16);
      this.labelQuestion.Text = "Quest:";
      // 
      // labelNamePrefix
      // 
      this.labelNamePrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelNamePrefix.Location = new System.Drawing.Point(4, 6);
      this.labelNamePrefix.Size = new System.Drawing.Size(38, 16);
      this.labelNamePrefix.Text = "Name:";
      // 
      // labelDateTimePrefix
      // 
      this.labelDateTimePrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelDateTimePrefix.Location = new System.Drawing.Point(134, 6);
      this.labelDateTimePrefix.Size = new System.Drawing.Size(34, 16);
      this.labelDateTimePrefix.Text = "Time:";
      // 
      // labelRespondent
      // 
      this.labelRespondent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelRespondent.Location = new System.Drawing.Point(4, 32);
      this.labelRespondent.Size = new System.Drawing.Size(36, 16);
      this.labelRespondent.Text = "Resp:";
      // 
      // tabPageSummary
      // 
      this.tabPageSummary.Controls.Add(this.panelSummary);
      this.tabPageSummary.Location = new System.Drawing.Point(4, 4);
      this.tabPageSummary.Size = new System.Drawing.Size(232, 242);
      this.tabPageSummary.Text = "Summary";
      // 
      // panelSummary
      // 
      this.panelSummary.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(196)), ((System.Byte)(176)), ((System.Byte)(222)));
      this.panelSummary.Controls.Add(this.panelResults2);
      this.panelSummary.Controls.Add(this.spinnerQuestion2);
      this.panelSummary.Controls.Add(this.labelSummary);
      this.panelSummary.Controls.Add(this.trackBarQuestion2);
      this.panelSummary.Controls.Add(this.labelQuestion2);
      this.panelSummary.Size = new System.Drawing.Size(248, 248);
      // 
      // panelResults2
      // 
      this.panelResults2.Controls.Add(this.listViewAnswers2);
      this.panelResults2.Controls.Add(this.labelQuestionText2);
      this.panelResults2.Location = new System.Drawing.Point(4, 64);
      this.panelResults2.Size = new System.Drawing.Size(224, 168);
      // 
      // listViewAnswers2
      // 
      this.listViewAnswers2.FullRowSelect = true;
      this.listViewAnswers2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.listViewAnswers2.Location = new System.Drawing.Point(8, 24);
      this.listViewAnswers2.Size = new System.Drawing.Size(208, 136);
      this.listViewAnswers2.View = System.Windows.Forms.View.Details;
      // 
      // labelQuestionText2
      // 
      this.labelQuestionText2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelQuestionText2.ForeColor = System.Drawing.Color.Black;
      this.labelQuestionText2.Location = new System.Drawing.Point(4, 4);
      this.labelQuestionText2.Size = new System.Drawing.Size(208, 30);
      this.labelQuestionText2.Text = "Question:";
      // 
      // spinnerQuestion2
      // 
      this.spinnerQuestion2.Location = new System.Drawing.Point(71, 22);
      this.spinnerQuestion2.Minimum = new System.Decimal(new int[] {
                                                                     1,
                                                                     0,
                                                                     0,
                                                                     0});
      this.spinnerQuestion2.Size = new System.Drawing.Size(52, 20);
      this.spinnerQuestion2.Value = new System.Decimal(new int[] {
                                                                   1,
                                                                   0,
                                                                   0,
                                                                   0});
      this.spinnerQuestion2.ValueChanged += new System.EventHandler(this.spinnerQuestion_ValueChanged);
      // 
      // labelSummary
      // 
      this.labelSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular);
      this.labelSummary.ForeColor = System.Drawing.Color.Blue;
      this.labelSummary.Location = new System.Drawing.Point(7, 0);
      this.labelSummary.Size = new System.Drawing.Size(218, 20);
      this.labelSummary.Text = "Summary of N responses";
      this.labelSummary.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // trackBarQuestion2
      // 
      this.trackBarQuestion2.Location = new System.Drawing.Point(127, 24);
      this.trackBarQuestion2.Size = new System.Drawing.Size(81, 45);
      this.trackBarQuestion2.ValueChanged += new System.EventHandler(this.trackBarQuestion_ValueChanged);
      // 
      // labelQuestion2
      // 
      this.labelQuestion2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelQuestion2.Location = new System.Drawing.Point(15, 24);
      this.labelQuestion2.Size = new System.Drawing.Size(52, 16);
      this.labelQuestion2.Text = "Question:";
      // 
      // frmStats
      // 
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ControlBox = false;
      this.Controls.Add(this.tabMain);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";

    }
		#endregion


    private void InitializeScreen()
    {
      // Responses Page

      if (!PollModel.CreationInfo.GetPersonalInfo)
      {
        labelNamePrefix.Visible = false;
        labelName.Visible = false;
      }

      spinnerRespondent.Minimum = 1;
      spinnerRespondent.Increment = 1;
      spinnerRespondent.Maximum = Respondents.Count;

      trackBarRespondent.Height = 19;
      trackBarRespondent.Minimum = 1;
      trackBarRespondent.SmallChange = (Respondents.Count / 100 < 1) ? 1 : Respondents.Count / 100;
      trackBarRespondent.LargeChange = (Respondents.Count / 10 < 1) ? 1 : Respondents.Count / 10;
      trackBarRespondent.Maximum = Respondents.Count;

      spinnerQuestion.Minimum = 1;
      spinnerQuestion.Increment = 1;
      spinnerQuestion.Maximum = PollModel.Questions.Count;

      trackBarQuestion.Height = 19;
      trackBarQuestion.Minimum = 1;
      trackBarQuestion.SmallChange = (PollModel.Questions.Count / 100 < 1) ? 1 : PollModel.Questions.Count / 100;
      trackBarQuestion.LargeChange = (PollModel.Questions.Count / 10 < 1) ? 1 : PollModel.Questions.Count / 10;
      trackBarQuestion.Maximum = PollModel.Questions.Count;

      labelQuestionText.Location = new Point(gap * 2, gap);
      labelQuestionText.BackColor = labelQuestionText.Parent.BackColor;

      panelSpinner.Visible = false;
      listViewAnswers.Visible = false;
      textBoxAnswer.Visible = false;

      // These columns will only be used when in 'Details' mode
      listViewAnswers.HeaderStyle = ColumnHeaderStyle.None;
      listViewAnswers.Columns.Add("", 50, HorizontalAlignment.Left);
      listViewAnswers.Columns.Add("", listViewAnswers.Width - listViewAnswers.Columns[0].Width - gap, HorizontalAlignment.Left);
      listViewAnswers.CheckBoxes = false;

      // Prepare icons for ListView
      listViewAnswers.SmallImageList = new ImageList();

      Icon icon = Multimedia.Images.GetIcon("CheckMark1");
      listViewAnswers.SmallImageList.Images.Add(icon);
      
      icon = Multimedia.Images.GetIcon("CheckMark2");
      listViewAnswers.SmallImageList.Images.Add(icon);

      panelSpinner.Size = new Size(130, labelAnswer.Bottom + gap * 3);


      // Summary Page
      labelSummary.Location = new Point(0,0);
      labelSummary.Text = "Summary of " + Respondents.Count.ToString() + " Responses";

      spinnerQuestion2.Minimum = 1;
      spinnerQuestion2.Increment = 1;
      spinnerQuestion2.Maximum = PollModel.Questions.Count;

      trackBarQuestion2.Height = 20;
      trackBarQuestion2.Minimum = 1;
      trackBarQuestion2.SmallChange = (PollModel.Questions.Count / 100 < 1) ? 1 : PollModel.Questions.Count / 100;
      trackBarQuestion2.LargeChange = (PollModel.Questions.Count / 10 < 1) ? 1 : PollModel.Questions.Count / 10;
      trackBarQuestion2.Maximum = PollModel.Questions.Count;

      labelQuestionText2.BackColor = labelQuestionText2.Parent.BackColor;

      panelResults2.Location = new Point(gap * 2, labelQuestion2.Bottom + gap * 2);

      listViewAnswers2.CheckBoxes = false;
      listViewAnswers2.Columns.Add("Answer", 100, HorizontalAlignment.Left);   // Prelim width only
      listViewAnswers2.Columns.Add("Num", smallColWidth, HorizontalAlignment.Center);
      listViewAnswers2.Columns.Add("%", smallColWidth, HorizontalAlignment.Center);

      // Create the scrolling panel for the FreeForm Summary display
      panelFreeForm = new DataObjects.AutoScrollPanel(panelResults2.BackColor);
      panelFreeForm.Location = listViewAnswers2.Location;
      panelFreeForm.Size = listViewAnswers2.Size;
      panelResults2.Controls.Add(panelFreeForm);

      RepositionControls(true);
      PopulateForm(Respondents, CurrRespondent, CurrQuestion);
    }


    private _Respondents FilterRespondents(Poll pollModel, int seqCriteria, ArrayList dateCriteria)
    {
      // First filter by date
      _Respondents respondents = pollModel.Respondents.FilterByDate(dateCriteria);

      // Now filter by sequence (ie. First 10, Last 25, etc.)
      //respondents = pollModel.Respondents.FilterBySequence(seqCriteria);   // Debug: This seems incorrect, so I'm temporarily replacing with the next line
      respondents = respondents.FilterBySequence(seqCriteria);

      return respondents;
    }


    private void frmStats_Resize(object sender, System.EventArgs e)
    {
      RepositionControls(false);    
    }


    private void RepositionControls(bool forceRedraw)
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
          tabMain.Size = new Size(wid, aHgt);
          panelResponses.Size = new Size(wid, aHgt);
          panelSummary.Size = new Size(wid, aHgt);
          int availWidth = wid - gap * 2;

          // For both Portrait and Landscape
          labelDateTimePrefix.Left = wid / 2 + gap * 3;
          labelDateTime.Left = labelDateTimePrefix.Right + gap;
          labelQuestion.Location = new Point(labelDateTimePrefix.Left, labelRespondent.Top);
          spinnerQuestion.Left = labelQuestion.Right;

          if (wid < hgt)  // Is it in Portrait mode?
          {
            // Responses tab
            trackBarRespondent.Location = new Point(labelRespondent.Left + gap, labelRespondent.Bottom + 3);
            trackBarRespondent.Width = spinnerRespondent.Right - labelRespondent.Left + gap;
            trackBarQuestion.Location = new Point(labelQuestion.Left + gap, trackBarRespondent.Top);
          }

          else  // Landscape mode
          {
            // Responses tab
            trackBarRespondent.Location = new Point(spinnerRespondent.Right, spinnerRespondent.Top);
            trackBarRespondent.Width = spinnerRespondent.Width + gap;
            trackBarQuestion.Location = new Point(spinnerQuestion.Right, spinnerQuestion.Top);
          }

          // For both Portrait & Landscape mode
          
          // Responses tab
          trackBarQuestion.Width = trackBarRespondent.Width;
          panelResults.Location = new Point(labelRespondent.Left, trackBarRespondent.Bottom + 10);
          panelResults.Size = new Size(wid - panelResults.Left * 2, panelResponses.Height - panelResults.Top - gap * 7);
          labelQuestionText.Width = panelResults.Width - gap * 4;
          AdjustLabelHeight(labelQuestionText);

          // Now adjust the other controls within panelResults
          listViewAnswers.Location = new Point(labelQuestionText.Left, labelQuestionText.Bottom + gap * 2);
          listViewAnswers.Size = new Size(labelQuestionText.Width, panelResults.Height - listViewAnswers.Top - gap);
          textBoxAnswer.Location = new Point(labelQuestionText.Left, labelQuestionText.Bottom + gap * 2);
          textBoxAnswer.Size = new Size(availWidth - gap * 6, panelResults.Height - textBoxAnswer.Top - gap);

          panelSpinner.Location = new Point((panelResults.Width - panelSpinner.Width) / 2, labelQuestionText.Bottom + gap * 2);
          labelMinMax.Width = panelSpinner.Width;
          labelAnswer.Width = panelSpinner.Width;


          // Summary tab
          labelSummary.Width = wid;

          // Horizontally center the 3 controls that sit below 'labelSummary'
          int subWid = labelQuestion2.Width + gap + spinnerQuestion2.Width + gap + trackBarQuestion2.Width;
          labelQuestion2.Left = (wid - subWid) / 2;
          spinnerQuestion2.Left = labelQuestion2.Right + gap;
          trackBarQuestion2.Left = spinnerQuestion2.Right + gap;

          panelResults2.Size = new Size(wid - panelResults2.Left * 2, panelSummary.Height - panelResults2.Top - gap * 7);
          labelQuestionText2.Size = new Size(labelQuestionText.Width, labelQuestionText.Height);

          listViewAnswers2.Location = new Point(labelQuestionText2.Left, labelQuestionText2.Bottom + gap * 2);
          listViewAnswers2.Size = new Size(labelQuestionText2.Width, panelResults2.Height - listViewAnswers2.Top - gap);

          // The listview will either have 3 or 4 columns, with all but the first being narrow ones
          int col0Width = listViewAnswers2.Width - ((listViewAnswers2.Columns.Count - 1) * smallColWidth + 2);
          int visRows = (int) ((float) listViewAnswers2.Height / 17 + 0.5) - 1;
          int col0Adj = (wid < hgt) ? 1 : 13;
          listViewAnswers2.Columns[0].Width = (visRows > listViewAnswers2.Items.Count) ? col0Width : col0Width - col0Adj;

          panelFreeForm.Location = listViewAnswers2.Location;
          panelFreeForm.Size = listViewAnswers2.Size;
        }
      }
    }



    /// <summary>
    /// Pressing 'Back' returns to the Criteria form.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void menuItemBack_Click(object sender, System.EventArgs e)
    {
      this.Close();

      // We need to pass back all 4 parameters so that we give the illusion of returning back
      // to exactly the same Criteria form, with exactly the same previous settings.
      frmCriteria criteriaForm = new frmCriteria(PollName, PollModel, SeqCriteria, DateCriteria);
    }

    private void menuItemDone_Click(object sender, System.EventArgs e)
    {
      // Debug: More to do here?

      this.Close();
    }


    private void PopulateForm(_Respondents respondents, int currRespondent, int currQuestion)
    {
      if (StopRepopulation)
        return;
      else
        StopRepopulation = true;

      Cursor.Current = Cursors.WaitCursor;

      int wid, hgt, aHgt;
      ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt);

      int availWidth = wid - gap * 2;

      _Question quest = PollModel.Questions[currQuestion];   // Populate object variable with current question info

      // Note: currRespondent & currQuestion are 0-based but the spinners & trackbars are 1-based
      if (tabMain.SelectedIndex == 0)
      {
        spinnerRespondent.Value = (decimal) currRespondent + 1;
        trackBarRespondent.Value = currRespondent + 1;
        spinnerQuestion.Value = (decimal) currQuestion + 1;
        trackBarQuestion.Value = currQuestion + 1;

        _Respondent respondent = respondents[currRespondent];

        if (PollModel.CreationInfo.GetPersonalInfo)
        {
          string fullName = (respondent.FirstName + " " + respondent.LastName).Trim(); 
          labelName.Text = (fullName == "") ? "N/A" : fullName;
        }

        if (respondent.TimeCaptured.Date == DateTime.Now.Date)
        {
          labelDateTime.Text = Tools.DisallowNullTime(respondent.TimeCaptured, false);
          labelDateTimePrefix.Text = "Time:";
        }
        else
        {
          labelDateTime.Text = Tools.DisallowNullDate(respondent.TimeCaptured);
          labelDateTimePrefix.Text = "Date:";
        }

        labelQuestionText.Text = Tools.FixPanelText(quest.Text);
        AdjustLabelHeight(labelQuestionText);

        if (quest.Choices.Count == 0)
        {
          string msg = "This question has no available choices.  You should notify the poll's creator: " + PollModel.CreationInfo.CreatorName;
          Tools.ShowMessage(msg, "Question Has No Choices", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }

        _Response response = respondents[currRespondent].Responses.Find_ResponseByQuestionID(quest.ID);

        // Now add the available choices

        // Instantiate & prepare the control we need to display the data in
        switch (quest.AnswerFormat)
        {
          case AnswerFormat.Standard:
          case AnswerFormat.List:
          case AnswerFormat.DropList:
          case AnswerFormat.MultipleChoice:
          case AnswerFormat.Range:
          case AnswerFormat.MultipleBoxes:
            listViewAnswers.Top = labelQuestionText.Bottom + gap * 2;
            listViewAnswers.Items.Clear();
            listViewAnswers.View = View.List;
            listViewAnswers.Visible = true;
            textBoxAnswer.Visible = false;
            panelSpinner.Visible = false;

            switch (quest.AnswerFormat)
            {
              case AnswerFormat.Standard:
              case AnswerFormat.List:
              case AnswerFormat.DropList:
                listViewAnswers.Height = panelResults.Height - listViewAnswers.Top - gap;
                break;

              case AnswerFormat.MultipleChoice:
                listViewAnswers.Height = panelResults.Height - listViewAnswers.Top - gap;
                break;

              case AnswerFormat.Range:
                listViewAnswers.View = View.Details;
                listViewAnswers.Height = panelResults.Height - listViewAnswers.Top - gap;
                listViewAnswers.Columns[0].Width = 40;   // Default - may be increased below
                break;

              case AnswerFormat.MultipleBoxes:
                listViewAnswers.View = View.Details;
                listViewAnswers.Height = panelResults.Height - listViewAnswers.Top - gap;
                listViewAnswers.Columns[0].Width = 40;   // Default - may be increased below
                break;
            }
            break;

          case AnswerFormat.Freeform:
            textBoxAnswer.Location = new Point(gap * 3, labelQuestionText.Bottom + gap * 2);
            textBoxAnswer.Size = new Size(availWidth - gap * 6, panelResults.Height - textBoxAnswer.Top - gap);
            textBoxAnswer.Visible = true;
            panelSpinner.Visible = false;
            listViewAnswers.Visible = false;
            break;

          case AnswerFormat.Spinner:
            panelSpinner.Location = new Point((panelResults.Width - panelSpinner.Width) / 2, labelQuestionText.Bottom + gap * 2);
            panelSpinner.Visible = true;
            listViewAnswers.Visible = false;
            textBoxAnswer.Visible = false;
            break;

          default:
            Debug.Fail("Unknown AnswerFormat: " + quest.AnswerFormat.ToString(), "frmStats.PopulateForm");
            break;
        }


        // Now populate the control(s) with the required data
        foreach (_Choice choice in quest.Choices)
        {
          ListViewItem lvwItem = new ListViewItem();

          switch (quest.AnswerFormat)
          {
            case AnswerFormat.Standard:
            case AnswerFormat.List:
            case AnswerFormat.DropList:
              if (choice.Text != "")
              {
                lvwItem.Text = choice.Text;
                if (response.AnswerID == choice.ID.ToString())
                  if (response.ExtraText == "" || choice.ExtraInfo == false)
                    lvwItem.ImageIndex = 0;
                  else
                    lvwItem.ImageIndex = 1;

                listViewAnswers.Items.Add(lvwItem);
              }
              break;

            case AnswerFormat.MultipleChoice:
              if (choice.Text != "")
              {
                lvwItem.Text = choice.Text;

                if (response.AnswerID != "")
                {
                  // Retrieve all AnswerIDs and add sentinels
                  string allAnswerIDs = "," + response.AnswerID + ",";

                  if (allAnswerIDs.IndexOf("," + choice.ID.ToString() + ",") != -1)
                  {
                    // We've now determined that this item was chosen.  But we still need to find out whether it has an
                    // associated "extraText" value too.  If so, then it will be displayed below the ListView when the
                    // item is selected.
                    string extraText = Tools.GetExtraTextValue(response.ExtraText, choice.ID);
                    if (extraText == "" || choice.ExtraInfo == false)
                      lvwItem.ImageIndex = 0;
                    else
                      lvwItem.ImageIndex = 1;
                  }
                }
                listViewAnswers.Items.Add(lvwItem);
              }
              break;

            case AnswerFormat.Range:
              if (choice.Text != "")
              {
                // We're going to introduce some special code here to widen the range value
                // column in case we encounter any ranges larger than 1-digit wide.
                // Note: We could make this even more sophisticated in the future!
                if (choice.MoreText.Length > 1)
                  listViewAnswers.Columns[0].Width = 50;

                lvwItem.Text = "[" + choice.MoreText + "]";
                lvwItem.SubItems.Add(choice.Text);

                if (response.AnswerID == choice.ID.ToString())
                  lvwItem.ImageIndex = 0;

                listViewAnswers.Items.Add(lvwItem);
              }
              break;

            case AnswerFormat.MultipleBoxes:
              if (choice.Text != "")
              {
                lvwItem.SubItems.Add(choice.Text);

                if (response.AnswerID != "")
                {
                  // Retrieve all AnswerIDs and add sentinels
                  string allAnswerIDs = "," + response.AnswerID + ",";

                  if (allAnswerIDs.IndexOf("," + choice.ID.ToString() + ",") != -1)
                  {
                    // We've now determined that this item was chosen.  But we still need to find out whether it has an
                    // associated "extraText" value too.  If so, then it will be displayed below the ListView when the
                    // item is selected.
                    string extraText = Tools.GetExtraTextValue(response.ExtraText, choice.ID);
                    lvwItem.Text = extraText;

                    if (extraText.Length > 1)
                      listViewAnswers.Columns[0].Width = 50;
                  }
                }
                listViewAnswers.Items.Add(lvwItem);
              }
              break;

            case AnswerFormat.Freeform:
              textBoxAnswer.Text = response.ExtraText;
              break;

            case AnswerFormat.Spinner:
              if (response.AnswerID != "")
              {
                labelAnswer.Text = response.AnswerID;
                labelMinMax.Text = "Min: " + choice.Text + "    Max: " + choice.MoreText;
              }
              break;

            default:
              Debug.Fail("Unknown AnswerFormat: " + quest.AnswerFormat.ToString(), "frmStats.PopulateForm");
              break;
          }
        }

        ToolsCF.SipShowIM(0);    // Might as well ensure SIP is collapsed
      }

      else   // Must be Summary page
      {
        spinnerQuestion2.Value = (decimal) currQuestion + 1;
        trackBarQuestion2.Value = currQuestion + 1;

        labelQuestionText2.Text = Tools.FixPanelText(quest.Text);
        AdjustLabelHeight(labelQuestionText2);
        
        listViewAnswers2.Location = new Point(labelQuestionText2.Left, labelQuestionText2.Bottom + gap * 2);
        listViewAnswers2.Size = new Size(labelQuestionText2.Width, panelResults2.Height - listViewAnswers2.Top - gap);
        listViewAnswers2.Items.Clear();
        panelFreeForm.Location = listViewAnswers2.Location;
        panelFreeForm.Size = listViewAnswers2.Size;
        panelFreeForm.Contents.Controls.Clear();

        if (quest.AnswerFormat == AnswerFormat.Freeform)
        {
          panelFreeForm.Visible = true;
          listViewAnswers2.Visible = false;
        }
        else
        {
          listViewAnswers2.Visible = true;
          panelFreeForm.Visible = false;
        }

        ArrayList choiceSums = new ArrayList();
        ArrayList calcSums = new ArrayList();
        ArrayList choicePercents = new ArrayList();
        ArrayList choiceAverages = new ArrayList();
        ArrayList choiceValues = new ArrayList();
        ArrayList respAnswers = new ArrayList();
        ArrayList respNames = new ArrayList();
        ArrayList respDates = new ArrayList();

        // Retrieve summary info for poll question
        switch (quest.AnswerFormat)
        {
          case AnswerFormat.Standard:
          case AnswerFormat.List:
          case AnswerFormat.DropList:
          case AnswerFormat.MultipleChoice:
          case AnswerFormat.Range:
            listViewAnswers2.Columns[listViewAnswers2.Columns.Count - 1].Text = "%";
            Tools.SummarizeAnswers(quest.AnswerFormat, Respondents, quest, out choiceSums, out calcSums, out choicePercents);   // Gather sums and percents
            break;

          case AnswerFormat.MultipleBoxes:
            listViewAnswers2.Columns[listViewAnswers2.Columns.Count - 1].Text = "Avg";
            Tools.SummarizeAnswers(quest.AnswerFormat, Respondents, quest, out choiceSums, out calcSums, out choiceAverages);   // Gather sums and averages
            break;

          case AnswerFormat.Freeform:
            Tools.SummarizeFreeFormAnswers(Respondents, quest, out respAnswers, out respNames, out respDates);
            break;

          case AnswerFormat.Spinner:
            listViewAnswers2.Columns[listViewAnswers2.Columns.Count - 1].Text = "%";
            Tools.SummarizeSpinnerAnswers(Respondents, quest, out choiceValues, out choiceSums, out calcSums, out choicePercents);
            break;

          default:
            Debug.Fail("Unknown AnswerFormat: " + quest.AnswerFormat.ToString(), "frmStats.PopulateForm");
            break;
        }

        // Now populate the control(s) with the required data
        switch (quest.AnswerFormat)
        {
          case AnswerFormat.Spinner:
            for (int i = 0; i < choiceValues.Count; i++)
            {
              ListViewItem lvwItem = new ListViewItem();
              lvwItem.Text = choiceValues[i].ToString();
              lvwItem.SubItems.Add(choiceSums[i].ToString());
              double percent = (double) choicePercents[i];
              string sPercent = "";
              // The extra logic is necessary because 100% is a rare case and it's not
              // worth it to artificially widen the column just for this one single case.
              if (percent > 99.5)
                sPercent = "100";
              else
                sPercent = percent.ToString("0") + "%";
              
              lvwItem.SubItems.Add(sPercent);                
              listViewAnswers2.Items.Add(lvwItem);
            }
            break;

          case AnswerFormat.Freeform:
            int topMargin = gap;
            int availWid = panelFreeForm.Contents.Width - gap * 2;

            if (respAnswers.Count == 0)
            {
              Font font = new Font("Microsoft Sans Serif", 12F, FontStyle.Italic);
              Label label = PanelControlsCF.AddLabel("No responses recorded", font, Color.Red, topMargin, gap, availWid, ContentAlignment.TopLeft, panelFreeForm.Contents);
            }
            else
            {
              Font font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular);
              Font font2 = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
              for (int i = 0; i < respAnswers.Count; i++)
              {
                Label label = PanelControlsCF.AddLabel(respAnswers[i].ToString(), font, Color.Blue, topMargin, gap, availWid, ContentAlignment.TopLeft, panelFreeForm.Contents);
                //Label label = PanelControlsCF.AddMultilineLabel
                topMargin += label.Height + gap;

                label = PanelControlsCF.AddLabel(respNames[i].ToString(), font2, topMargin, gap, (int) (availWid * 0.4), ContentAlignment.TopLeft, panelFreeForm.Contents);
                label = PanelControlsCF.AddLabel(respDates[i].ToString(), font2, topMargin, availWid + gap, (int) (availWid * 0.6), ContentAlignment.TopRight, panelFreeForm.Contents);
                topMargin += label.Height + gap * 4;
              }

              panelFreeForm.SetScrollHeight(topMargin + gap);
            }
            break;

          default:   // All the other formats
            for (int i = 0; i < quest.Choices.Count; i++)
            {
              _Choice choice = quest.Choices[i];
              ListViewItem lvwItem = new ListViewItem();

              switch (quest.AnswerFormat)
              {
                case AnswerFormat.Standard:
                case AnswerFormat.List:
                case AnswerFormat.DropList:
                case AnswerFormat.MultipleChoice:
                case AnswerFormat.Range:
                  if (choice.Text != "")
                  {
                    if (quest.AnswerFormat == AnswerFormat.Range)
                      lvwItem.Text = "[" + choice.MoreText + "]  " + choice.Text;
                    else
                      lvwItem.Text = choice.Text;

                    lvwItem.SubItems.Add(choiceSums[i].ToString());

                    string sPercent = "";
                    try
                    {
                      double percent = (double) choicePercents[i];
                      // The extra logic is necessary because 100% is a rare case and it's not worth it to
                      // artificially widen the column just for this one single case.
                      if (percent > 99.5)
                        sPercent = "100";
                      else
                        sPercent = percent.ToString("0") + "%";
                    }
                    catch
                    {
                      // Do nothing, just catch error
                    }

                    lvwItem.SubItems.Add(sPercent);                
                    listViewAnswers2.Items.Add(lvwItem);
                  }
                  break;

                case AnswerFormat.MultipleBoxes:
                  if (choice.Text != "")
                  {
                    lvwItem.Text = choice.Text;
                    lvwItem.SubItems.Add(choiceSums[i].ToString());

                    string sAvg = "";
                    try
                    {
                      // Here 'choicePercents' is actually used to hold the averages of each choice's values
                      double avg = (double) choiceAverages[i];

                      if (avg < 9.5)
                        sAvg = avg.ToString("0.0");    // 2 digits, plus decimal point
                      else
                        sAvg = avg.ToString("0");      // 2+ digits
                    }
                    catch
                    {
                      // Do nothing, just catch error
                    }

                    lvwItem.SubItems.Add(sAvg);
                    listViewAnswers2.Items.Add(lvwItem);
                  }
                  break;

                default:
                  Debug.Fail("Unknown AnswerFormat: " + quest.AnswerFormat.ToString(), "frmStats.PopulateForm");
                  break;
              }
            }
            break;
        }

        // Some AnswerFormats require a summary tabulation
        string summaryText = "";
        switch (quest.AnswerFormat)
        {
          case AnswerFormat.Range:
          case AnswerFormat.Spinner:
            listViewAnswers2.Items.Add(new ListViewItem(""));
            double avgValue = (double) Tools.SumArrayValues(calcSums) / Tools.SumArrayValues(choiceSums);
            summaryText = "Avg Value:  " + avgValue.ToString("0.00");
            listViewAnswers2.Items.Add(new ListViewItem(summaryText));
            break;

          default:
            // Do nothing
            break;
        }


        //  For now, we'll not tally the none-responses
        //        // We've populated the listview with the actual items.  We must now add an entry that
        //        // encompasses those repondents who didn't provide an answer to this question.
        //        int extraIdx = quest.Choices.Count;
        //        if (choiceSums.Count > extraIdx)
        //        {
        //          ListViewItem lvwItem = new ListViewItem();
        //          lvwItem.Text = "< No Answer >";
        //          lvwItem.SubItems.Add(choiceSums[extraIdx].ToString());
        //          lvwItem.SubItems.Add(choicePercents[extraIdx].ToString() + "%");
        //          listViewAnswers2.Items.Add(lvwItem);
        //        }


        // Finally, we must check whether there are an excessive number of items that have forced the
        // vertical scroll bars to appear.  If so, then we must shrink the width of the first column
        // to compensate, otherwise the unwanted horiz scrollbar also appears.
        // Note: The formula used isn't exact but seems to provide a reasonable
        //       algorithm for both Portrait and Landscape mode.
        int col0Width = listViewAnswers2.Width - ((listViewAnswers2.Columns.Count - 1) * smallColWidth + 2);
        int visRows = (int) ((float) listViewAnswers2.Height / 17 + 0.5) - 1;
        int col0Adj = 13;   // Value based on testing
        listViewAnswers2.Columns[0].Width = (visRows > listViewAnswers2.Items.Count) ? col0Width : col0Width - col0Adj;

        ToolsCF.SipShowIM(0);    // Might as well ensure SIP is collapsed
      }

      StopRepopulation = false;
      Cursor.Current = Cursors.Default;
    }


    private void AdjustLabelHeight(Label label)
    {
      Size labelSize = Tools.GetLabelSize(label.Text, label.Font);
      int numLines = Math.Max(Convert.ToInt32(labelSize.Width / label.Width + 1), 1);
      label.Height = numLines * labelSize.Height;
    }


    private void spinnerRespondent_ValueChanged(object sender, System.EventArgs e)
    {
      int newResp = (int) spinnerRespondent.Value - 1;
      if (newResp != CurrRespondent)
        CurrRespondent = newResp;
      this.Focus();
    }

    private void trackBarRespondent_ValueChanged(object sender, System.EventArgs e)
    {
      CurrRespondent = (int) trackBarRespondent.Value - 1;
      this.Focus();
    }

    private void spinnerQuestion_ValueChanged(object sender, System.EventArgs e)
    {
      NumericUpDown spinner = sender as NumericUpDown;
      int newQuest = (int) spinner.Value - 1;
      if (newQuest != CurrQuestion)
        CurrQuestion = newQuest;
      this.Focus();
    }

    private void trackBarQuestion_ValueChanged(object sender, System.EventArgs e)
    {
      TrackBar trackBar = sender as TrackBar;
      //CurrQuestion = (int) trackBarQuestion.Value - 1;
      CurrQuestion = (int) trackBar.Value - 1;
      this.Focus();
    }

    private void tabMain_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      PopulateForm(Respondents, CurrRespondent, CurrQuestion);
    }

    // This event handler prevents any item from being selected in the ListView.
    // The exception to this is when AnswerFormat.MultipleChoice is in effect.
    private void listViewAnswers_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      ListView lstView = sender as ListView;

      if (lstView.SelectedIndices.Count != 0)  // Necessary, to ensure that an item is selected
      {
        ListViewItem lvi = lstView.Items[lstView.SelectedIndices[0]];   // This is based on the assumption that multiple selection will not occur

        if (lvi.ImageIndex > 0)     // -1 - No icon     0 - Selected item, but no extra info     1 - Selected item with extra info     2+ - Future
        {
          _Question quest = PollModel.Questions[currQuestion];   // Populate object variable with current question info
          _Choice choice = quest.Choices[lvi.Index];
          string msg = Tools.EnsureSuffix(choice.MoreText, ":") + "  ";
          _Response response = Respondents[currRespondent].Responses.Find_ResponseByQuestionID(quest.ID);

          if (quest.AnswerFormat == AnswerFormat.MultipleChoice)
            msg += Tools.GetExtraTextValue(response.ExtraText, choice.ID);
          else
            msg += response.ExtraText;
          
          Tools.ShowMessage(msg, "Extra Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
          lvi.Selected = false;
      }
    }


    private void textBoxAnswer_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
    {
      e.Handled = true;  // This will cancel all characters entered into the textbox, thus effectively making it read-only
    }


	}
}
