using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Resources;

using DataObjects;



namespace Desktop
{
  // Define Aliases
  using Tools = DataObjects.Tools;
  using PanelControls = DataObjects.PanelControls;
  using AnswerFormat = DataObjects.Constants.AnswerFormat;
  using PurgeDuration = DataObjects.Constants.PurgeDuration;
  //using GraphType = DataObjects.Constants.GraphType;


	/// <summary>
	/// Summary description for frmPoll
	/// </summary>
  public class frmPoll : System.Windows.Forms.Form
  {
    // This is the event handler that will be used by Drag & Drop events, such as Question Buttons.
    public delegate void DragDropEventHandler (int newPosition, object sender, DragEventArgs e);

    private System.ComponentModel.IContainer components;


    #region ControlDefinitions

    private System.Windows.Forms.TabControl tabNewPoll;
    private System.Windows.Forms.TabPage tabPageSummary;
    private System.Windows.Forms.TabPage tabPageQuestions;
    private System.Windows.Forms.TextBox textQuestion;
    private DataObjects.PanelGradient panelQuestion;
    private System.Windows.Forms.Label labelQuestion;
    private DataObjects.PanelGradient panelAfterAllPolls;
    private System.Windows.Forms.TextBox textBoxAfterAllPolls;
    private System.Windows.Forms.Label labelAfterAllPolls;
    private DataObjects.PanelGradient panelQuestionList;
    private DataObjects.PanelGradient panelAfterPoll;
    private System.Windows.Forms.CheckBox checkBoxRepeatAfterPoll;
    private System.Windows.Forms.TextBox textBoxAfterPoll;
    private System.Windows.Forms.Label labelAfterPoll;
    private DataObjects.PanelGradient panelEndMessage;
    private System.Windows.Forms.TextBox textBoxEndMessage;
    private System.Windows.Forms.Label labelEndMessage;
    private DataObjects.PanelGradient panelBeforePoll;
    private System.Windows.Forms.CheckBox checkBoxRepeatBeforePoll;
    private System.Windows.Forms.TextBox textBoxBeforePoll;
    private DataObjects.PanelGradient panelBeginMessage;
    private System.Windows.Forms.TextBox textBoxBeginMessage;
    private System.Windows.Forms.Label labelBeginMessage;
    private DataObjects.PanelGradient panelQuestionsTop;
    private System.Windows.Forms.Label labelQuestionsTop;
    private DataObjects.PanelGradient panelAnswerFormat;
    private System.Windows.Forms.Label labelBeforePoll;
    private DataObjects.PanelGradient panelCEI;
    private DataObjects.PanelGradient panelCEIright;
    private System.Windows.Forms.Label labelLastEdited;
    private System.Windows.Forms.Label labelLastEditDate;
    private DataObjects.PanelGradient panelCEIleft;
    private System.Windows.Forms.Label labelCreatedOn;
    private System.Windows.Forms.Label labelCreationDate;
    private System.Windows.Forms.Label labelCreatedBy;
    private DataObjects.PanelGradient panelQuestionList1;
    private DataObjects.PanelGradient panelQuestionList2;
    private System.Windows.Forms.Button buttonAddQuestion;
    private System.Windows.Forms.Button buttonRemoveQuestion;
    private DataObjects.PanelGradient panelChoices;
    private DataObjects.PanelGradient panelChoicesTitle;
    private System.Windows.Forms.Label labelChoicesTitle;
    private System.Windows.Forms.Button buttonAddChoice;
    private System.Windows.Forms.Button buttonRemoveChoice;
    private DataObjects.PanelGradient panelPreviewFrame;
    private DataObjects.PanelGradient panelPreviewTitle;
    private System.Windows.Forms.Label labelPreview;
    private DataObjects.PanelGradient panelPreview;
    private System.Windows.Forms.Timer DragDropTimer;
    private System.Windows.Forms.Label labelLastEditedByIntro;
    private System.Windows.Forms.Label labelLastEditedBy;
    private System.Windows.Forms.Label labelCreatorName;
    private System.Windows.Forms.TabPage tabPageSettings;
    private System.Windows.Forms.TabPage tabPageInstructions;
    private System.Windows.Forms.TabPage tabPageResponses;
    private DataObjects.PanelGradient panelPollSummary;
    private System.Windows.Forms.Label labelPollSummary;
    private System.Windows.Forms.TextBox textBoxPollSummary;
    private DataObjects.PanelGradient panelInstructionsTop;
    private DataObjects.PanelGradient panelSettingsTop;

    private DataObjects.PanelGradient panelResponsesTop;
    private System.Windows.Forms.Label labelInstructionsTop;
    private System.Windows.Forms.Label labelSettingsTop;
    private System.Windows.Forms.Label labelResponsesTop;
    private System.Windows.Forms.Label labelQuestionsHeader;
    private System.Windows.Forms.Label labelResponsesHeader;
    private DataObjects.PanelGradient panelResponsesFilter;
    private DataObjects.PanelGradient panelAnswers;
    private System.Windows.Forms.Button buttonRIdisplay;
    private DataObjects.PanelGradient panelRespondentInfo;

    private System.Windows.Forms.Timer timerAnimateRIpanel;
    private System.Windows.Forms.Label labelPanelRespondentInfo;
    private System.Windows.Forms.Label labelFilterHeader;
    private System.Windows.Forms.CheckedListBox listBoxResponsesDate;
    private System.Windows.Forms.CheckedListBox listBoxResponsesSequence;

    private DataObjects.PanelGradient panelResp_Responses;
    private DataObjects.PanelGradient panelResp_Questions;
    private DataObjects.LabelGradient labelNoResponses;
    private System.Windows.Forms.Panel panelResp_Spinner;
    private System.Windows.Forms.Label labelResp_MinMax;
    private System.Windows.Forms.Label labelResp_Answer;
    private System.Windows.Forms.TextBox textBoxResp_Answer;
    private System.Windows.Forms.ListView listViewResp_Answers;

    private System.Windows.Forms.Panel panelResp_ExtraInfo;
    private System.Windows.Forms.Label labelResp_ExtraInfoIntro;
    private System.Windows.Forms.TextBox textBoxResp_ExtraInfo;
    private System.Windows.Forms.Label labelResp_AnswersMsg;
    private System.Windows.Forms.TextBox textBoxResp_Question;
    private System.Windows.Forms.Label labelPanelResp;
    private DataObjects.PanelGradient panelResp_ActualQuestion;

    private System.Windows.Forms.Label labelResp_Address;
    private System.Windows.Forms.Label labelResp_AddressIntro;
    private System.Windows.Forms.Label labelResp_NameIntro;
    private System.Windows.Forms.Label labelResp_Name;
    private System.Windows.Forms.Label labelResp_DateIntro;
    private System.Windows.Forms.Label labelResp_Date;
    private System.Windows.Forms.Label labelResp_TimeIntro;
    private System.Windows.Forms.Label labelResp_Time;
    private System.Windows.Forms.Label labelResp_Pollster;
    private System.Windows.Forms.Label labelResp_PollsterIntro;
    private System.Windows.Forms.Label labelResp_SexIntro;
    private System.Windows.Forms.Label labelResp_Sex;
    private System.Windows.Forms.Label labelResp_Age;
    private System.Windows.Forms.Label labelResp_AgeIntro;
    private System.Windows.Forms.Label labelResp_CityIntro;
    private System.Windows.Forms.Label labelResp_City;
    private System.Windows.Forms.Label labelResp_StateProvIntro;
    private System.Windows.Forms.Label labelResp_Tel1Intro;
    private System.Windows.Forms.Label labelResp_Tel1;
    private System.Windows.Forms.Label labelResp_GPSIntro;
    private System.Windows.Forms.Panel panelResp_riMiddle;
    private System.Windows.Forms.Panel panelResp_riLeft;
    private System.Windows.Forms.Panel panelResp_riRight;
    private System.Windows.Forms.Label labelResp_StateProv;
    private System.Windows.Forms.Label labelResp_GPS;

    private System.Windows.Forms.Label labelResp_PostalCode;
    private System.Windows.Forms.Label labelResp_PostalCodeIntro;
    private System.Windows.Forms.Label labelResp_AnswerTime;
    private System.Windows.Forms.Label labelResp_AnswerDate;
    private System.Windows.Forms.Label labelResp_AnswerDateIntro;
    private System.Windows.Forms.Label labelResp_AnswerTimeIntro;
    private System.Windows.Forms.Label labelResp_ExtraInfoTitle;

    #endregion


    // To Do: Create alternate constructor so that pollName can be passed to frmPoll class

    // Form-level properties

    // This property is actually just a facade used to obtain the value of the 'CurrentChoice' property that
    // resides with every Question button.  It *generally* provides a simple way to obtain the current choice.
    // However it must be noted that it does not always contain the correct value during event handling.  An
    // example is if panel 0 is highlighted but then the user clicks a control on panel 2.  CurrentChoice will
    // still read '0' even though a 'Choices[2]' control object is firing an event.  This fact is accounted
    // for in the Controller by obtaining the 'ChoiceNum' property of the panel whose child fired the event.
    public int CurrentChoice
    {
      get
      {
        if (Tools.CountControls(panelQuestionList1, new QuestionButton()) == 0)     // (panelQuestionList1.Controls.Count == 0)
          return 0;
        else
        {
          // Retrieve value directly from the associated Question button
          int nonButtons = Tools.CountExtraneousControls(panelQuestionList1, new QuestionButton());
          QuestionButton qBut = panelQuestionList1.Controls[CurrentQuestion + nonButtons] as QuestionButton;
        
          return Convert.ToInt32(qBut.CurrentChoice);
        }
      }
      set
      {
        // Store value directly in associated Question button
        int nonButtons = Tools.CountExtraneousControls(panelQuestionList1, new QuestionButton());
        QuestionButton qBut = panelQuestionList1.Controls[CurrentQuestion + nonButtons] as QuestionButton;
        qBut.CurrentChoice = value;
      }
    }

    private int currentquestion;
    private int CurrentQuestion     // Note: This property is private, to signify that it is set and read from frmPoll only
    {
      get
      {
        return currentquestion;
      }
      set
      {
        currentquestion = value;
      }
    }


    // This property is set by code in this form and is monitored by the Controller.
    // It is set true (locked) during such actions that shouldn't change the state
    // of Controller.IsDirty.
    private bool lockIsDirty;
    public bool LockIsDirty
    {
      get
      {
        return lockIsDirty;
      }
      set
      {
        lockIsDirty = value;
      }
    }



    // Constants
    public const AnchorStyles anchorTL = AnchorStyles.Top | AnchorStyles.Left;
    public const AnchorStyles anchorTR = AnchorStyles.Top | AnchorStyles.Right;
    public const AnchorStyles anchorTLR = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    public const AnchorStyles anchorBL = AnchorStyles.Bottom | AnchorStyles.Left;
    public const AnchorStyles anchorBR = AnchorStyles.Bottom | AnchorStyles.Right;
    public const AnchorStyles anchorBLR = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    public const int ALT = 32;
    public const int CTRL = 8;
    public const int SHIFT = 4;


    // Public fields
    public string PollName;                             // Just a mirror of 'Controller.pollName'
    public DataObjects.PreviewInfo PreviewInfo;         // Keeps track of the current settings in the Preview pane
    public string DefaultQuestionText;

    
    // Internal fields
    internal DragDropData DragDropInfo;                 // Stores temporary information used during a drag & drop operation


    // Module-level private fields
    private bool listBoxResponsesSequenceChanging;      // If true then the event handler is doing its work; prevents recursive calls
    private bool listBoxResponsesDateChanging;          // If true then the event handler is doing its work; prevents recursive calls
    private RIpanelAnimationInfo RIpanelAnimation;      // Holds necessary data required to animated the opening/closing of the RI panel
    private bool HideQuestionNumbers;                   // Specially introduced so that "Question #" is handled correctly in the Preview pane
    private DateTime LastPanelSummaryResize;            // Used to hold off on displaying charts until all resizing is done


    // Since the Summary & Responses pages are not fully hooked up to ASM, we're going to maintain separate
    // data models for each of them; the contents of which will be "controlled" by the filter selection on each page.
    //private FilteredTabPageInfo summaryPage;
    private System.Windows.Forms.Panel panelExtraInfoHeader;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxSummaryPollSummary;
    private System.Windows.Forms.Label labelSummaryProductID;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label11;
    private DataObjects.PanelGradient paneSummaryTop;
    private System.Windows.Forms.Label labelSummaryTop;
    private System.Windows.Forms.Label labelQA_QuestionsHeader;
    private System.Windows.Forms.Label labelResp_EmailIntro;
    private System.Windows.Forms.Label labelResp_Email;
    private System.Windows.Forms.Label labelResp_AnswerFormat;
    private System.Windows.Forms.Label labelResp_AnswerFormatIntro;
    private System.Windows.Forms.Label labelRevisionIntro;
    private System.Windows.Forms.Label labelRevisionNumber;
    private System.Windows.Forms.Button buttonDuplicateQuestion;
    private System.Windows.Forms.Button buttonDuplicateChoice;
    private PanelGradient.PanelGradient panelSettingsBottom;
    private DataObjects.PanelGradient panelPollsterPrivileges;
    private System.Windows.Forms.Label labelPollsterPrivileges;
    private System.Windows.Forms.CheckBox checkBoxReviewData;
    private System.Windows.Forms.CheckBox checkBoxAbortRecord;
    private DataObjects.PanelGradient panelPurge;
    private System.Windows.Forms.ComboBox comboPurgeDuration;
    private System.Windows.Forms.Label labelPurge;
    private DataObjects.PanelGradient panelGeneralSettings;
    private System.Windows.Forms.Label labelGeneralSettings;
    private System.Windows.Forms.CheckBox checkBoxPersonalInfo;
    private System.Windows.Forms.CheckBox checkBoxHideQuestionNumbers;
    private DataObjects.PanelGradient panelPassword;
    private System.Windows.Forms.Label labelPasswordInstructions;
    private System.Windows.Forms.TextBox textBoxOpenPassword;
    private System.Windows.Forms.Label labelOpenPassword;
    private System.Windows.Forms.Label labelModifyPassword;
    private System.Windows.Forms.TextBox textBoxModifyPassword;
    private DataObjects.PanelGradient panelCompact;
    private System.Windows.Forms.Button buttonExportFilenameSelect;
    private System.Windows.Forms.Label labelExportInstructions;
    private System.Windows.Forms.TextBox textBoxExportFilename;
    private System.Windows.Forms.Label labelExportFilename;
    private System.Windows.Forms.CheckBox checkBoxExportEnabled;
    private System.Windows.Forms.Label labelSummaryQuestionsTally;
    private System.Windows.Forms.Label labelSummaryResponsesTally;
    private System.Windows.Forms.Panel panelMidUpper;
    private DataObjects.PanelGradient panelSummaryLower;
    private DataObjects.PanelGradient panelSummaryUpper;
    private System.Windows.Forms.Panel panelPollCreationInfo;
    private System.Windows.Forms.Panel panelSummaryCharts;
    private System.Windows.Forms.Timer timerShowCharts;
    private System.Windows.Forms.CheckBox checkBox1;
    private FilteredTabPageInfo responsesPageInfo;



    // frmPoll constructor used to open an existing poll
    public frmPoll(string pollname)
    {
      PreparePoll(pollname);
    }


    /// <summary>
    /// All (or some?) entry points to this form will be channeled through this method:
    ///   1. Initialize all controls on the form.
    ///   2. Set the caption on the title bar to:  "Poll_Name - Pocket Pollster"
    ///   3. Either:
    ///        a. New Poll:      Initialize the form & memory object with the basic, default data.
    ///        b. Existing Poll: Retrieve the poll data from its file and populate the form & memory object with it.
    /// </summary>
    /// <param name="pollname"></param>
    private void PreparePoll(string pollname)
    {
      // Required for Windows Form Designer support
      InitializeComponent();

      if (Tools.IsAppExpired())
        return;

      // This needs to be done here, before we open up an existing poll
      PopulateListControls();

      // Determine if we're creating a new poll or opening an existing one
      if (pollname == "")
      {
        // New Poll
        int pollnum = SysInfo.Data.Admin.GetNextBlankPollNumber();
        pollname = "Poll" + pollnum.ToString();
  
        // For brand new polls, we'll remove both the Summary and Responses pages
        // and move the Questions page before the Instructions page
        ArrayList tabPages = new ArrayList();
        tabPages.AddRange(new object[] {tabPageQuestions, tabPageInstructions, tabPageSettings});
        StoreTabPages(tabPages);
      }

      else
      { 
        // Existing Poll

        // To Do: Open file, retrieve data from it, and populate memory object with this data.
        //        Note: The event wiring (see above) will then automatically populate controls in frmPoll.
      
        // Remove path and file extension before storing in module-level "PollName" variable
        pollname = Tools.StripPathAndExtension(pollname);

        // For populated polls, we'll move the Responses page next to the Summary page
        ArrayList tabPages = new ArrayList();
        tabPages.AddRange(new object[] {tabPageSummary, tabPageResponses, tabPageInstructions, tabPageQuestions, tabPageSettings});
        StoreTabPages(tabPages);

        if (SysInfo.Data.Admin.OperatingMode == DataObjects.Constants.OpMode.Viewer)
        {
          tabNewPoll.TabPages.RemoveAt(4);
          tabNewPoll.TabPages.RemoveAt(3);
          tabNewPoll.TabPages.RemoveAt(2);
        }
      }

      // In both cases, make the first page the current one
      tabNewPoll.SelectedIndex = 0;

      // Store name in public variable (it'll later be obtained by the Controller)
      PollName = pollname;

      // Declare a Resource Manager instance
      ResourceManager resMgr = new ResourceManager("Desktop.Desktop", typeof(frmPoll).Assembly);

      //Debug: Still need to test that this next line works for other languages!!!
      DefaultQuestionText = resMgr.GetString("textQuestion.Text");    // ie. "Enter your question here . . ."
      
      // And set textQuestion to its default state
      SetDefaultQuestionPrompt();

      PreviewInfo = new DataObjects.PreviewInfo();

      // Store the current question index
      CurrentQuestion = 0;

      // Set caption in title bar. We won't display the path.
      this.Text = pollname;
    }


    /// <summary>
    /// Load Event - Executed after constructor.  Used to do an initialization of assorted controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void frmPoll_Load(object sender, System.EventArgs e)
    {
      int gap = 4;

      Readjust_frmPoll();            // Correctly resize the uppermost tab buttons (to accommodate their text)
      this.Width = this.Width + 2;   // This little trick, or some derivation of it, forces the Instruction panels to be readjusted properly

      // Initially center the NoResponses message on the Responses page
      Label label = labelNoResponses;
      label.Location = new Point((label.Parent.Width - label.Width) / 2, (label.Parent.Height - label.Height) / 2);

      // Prepare ImageList that contains an UpArrow & a DownArrow.  They're used as the
      // image for the animation button on the RespondentInfo panel on the Responses page.
      buttonRIdisplay.ImageList = new ImageList();
      Image image = Multimedia.Images.GetImage("ArrowUp.bmp");     // 0 - Up arrow (to collapse panel)
      buttonRIdisplay.ImageList.Images.Add(image);        
      image = Multimedia.Images.GetImage("ArrowDown.bmp");         // 1 - Down arrow (to expand panel)
      buttonRIdisplay.ImageList.Images.Add(image);
      buttonRIdisplay.ImageIndex = 0;  // Default setting

      listViewResp_Answers.Visible = false;
      textBoxResp_Answer.Visible = false;
      panelResp_Spinner.Visible = false;
      panelResp_ExtraInfo.Visible = false;
      labelResp_AnswersMsg.Visible = false;

      // These columns will only be used when in 'Details' mode
      listViewResp_Answers.HeaderStyle = ColumnHeaderStyle.None;
      listViewResp_Answers.Columns.Add("", 50, HorizontalAlignment.Left);
      listViewResp_Answers.Columns.Add("", listViewResp_Answers.Width - listViewResp_Answers.Columns[0].Width - gap, HorizontalAlignment.Left);
      listViewResp_Answers.CheckBoxes = false;

      // Prepare icons for ListView
      listViewResp_Answers.SmallImageList = new ImageList();
      Icon icon = Multimedia.Images.GetIcon("CheckMark1");
      listViewResp_Answers.SmallImageList.Images.Add(icon);
      icon = Multimedia.Images.GetIcon("CheckMark2");
      listViewResp_Answers.SmallImageList.Images.Add(icon);
    }


    private void StoreTabPages(ArrayList pagesToDisplay)
    {
      // First remove all tab pages
      for(int i = tabNewPoll.TabPages.Count - 1; i >= 0; i--)
      {
        tabNewPoll.TabPages.RemoveAt(i);
      }

      // Now add back required tab pages
      foreach(TabPage tabPage in pagesToDisplay)
      {
        tabNewPoll.TabPages.Add(tabPage);
      }
    }


    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing )
    {
      if (disposing)
      {
        if (components != null)
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
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmPoll));
      this.tabNewPoll = new System.Windows.Forms.TabControl();
      this.tabPageSummary = new System.Windows.Forms.TabPage();
      this.panelSummaryLower = new DataObjects.PanelGradient();
      this.panelSummaryCharts = new System.Windows.Forms.Panel();
      this.panelMidUpper = new System.Windows.Forms.Panel();
      this.labelSummaryQuestionsTally = new System.Windows.Forms.Label();
      this.labelSummaryResponsesTally = new System.Windows.Forms.Label();
      this.panelSummaryUpper = new DataObjects.PanelGradient();
      this.panelPollCreationInfo = new System.Windows.Forms.Panel();
      this.labelSummaryProductID = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.textBoxSummaryPollSummary = new System.Windows.Forms.TextBox();
      this.paneSummaryTop = new DataObjects.PanelGradient();
      this.labelSummaryTop = new System.Windows.Forms.Label();
      this.tabPageInstructions = new System.Windows.Forms.TabPage();
      this.panelBeginMessage = new DataObjects.PanelGradient();
      this.textBoxBeginMessage = new System.Windows.Forms.TextBox();
      this.labelBeginMessage = new System.Windows.Forms.Label();
      this.panelBeforePoll = new DataObjects.PanelGradient();
      this.checkBoxRepeatBeforePoll = new System.Windows.Forms.CheckBox();
      this.textBoxBeforePoll = new System.Windows.Forms.TextBox();
      this.labelBeforePoll = new System.Windows.Forms.Label();
      this.panelAfterPoll = new DataObjects.PanelGradient();
      this.checkBoxRepeatAfterPoll = new System.Windows.Forms.CheckBox();
      this.textBoxAfterPoll = new System.Windows.Forms.TextBox();
      this.labelAfterPoll = new System.Windows.Forms.Label();
      this.panelEndMessage = new DataObjects.PanelGradient();
      this.textBoxEndMessage = new System.Windows.Forms.TextBox();
      this.labelEndMessage = new System.Windows.Forms.Label();
      this.panelInstructionsTop = new DataObjects.PanelGradient();
      this.labelInstructionsTop = new System.Windows.Forms.Label();
      this.panelAfterAllPolls = new DataObjects.PanelGradient();
      this.textBoxAfterAllPolls = new System.Windows.Forms.TextBox();
      this.labelAfterAllPolls = new System.Windows.Forms.Label();
      this.tabPageQuestions = new System.Windows.Forms.TabPage();
      this.panelChoices = new DataObjects.PanelGradient();
      this.panelChoicesTitle = new DataObjects.PanelGradient();
      this.buttonDuplicateChoice = new System.Windows.Forms.Button();
      this.buttonRemoveChoice = new System.Windows.Forms.Button();
      this.buttonAddChoice = new System.Windows.Forms.Button();
      this.labelChoicesTitle = new System.Windows.Forms.Label();
      this.panelPreviewFrame = new DataObjects.PanelGradient();
      this.panelPreview = new DataObjects.PanelGradient();
      this.panelPreviewTitle = new DataObjects.PanelGradient();
      this.labelPreview = new System.Windows.Forms.Label();
      this.panelQuestion = new DataObjects.PanelGradient();
      this.textQuestion = new System.Windows.Forms.TextBox();
      this.labelQuestion = new System.Windows.Forms.Label();
      this.panelQuestionList = new DataObjects.PanelGradient();
      this.panelQuestionList1 = new DataObjects.PanelGradient();
      this.labelQA_QuestionsHeader = new System.Windows.Forms.Label();
      this.panelQuestionList2 = new DataObjects.PanelGradient();
      this.buttonAddQuestion = new System.Windows.Forms.Button();
      this.buttonRemoveQuestion = new System.Windows.Forms.Button();
      this.buttonDuplicateQuestion = new System.Windows.Forms.Button();
      this.panelAnswerFormat = new DataObjects.PanelGradient();
      this.panelQuestionsTop = new DataObjects.PanelGradient();
      this.labelQuestionsTop = new System.Windows.Forms.Label();
      this.tabPageResponses = new System.Windows.Forms.TabPage();
      this.labelNoResponses = new DataObjects.LabelGradient();
      this.panelAnswers = new DataObjects.PanelGradient();
      this.labelResp_AnswerTime = new System.Windows.Forms.Label();
      this.labelResp_AnswersMsg = new System.Windows.Forms.Label();
      this.panelResp_ExtraInfo = new System.Windows.Forms.Panel();
      this.panelExtraInfoHeader = new System.Windows.Forms.Panel();
      this.labelResp_ExtraInfoTitle = new System.Windows.Forms.Label();
      this.textBoxResp_ExtraInfo = new System.Windows.Forms.TextBox();
      this.labelResp_ExtraInfoIntro = new System.Windows.Forms.Label();
      this.panelResp_Spinner = new System.Windows.Forms.Panel();
      this.labelResp_MinMax = new System.Windows.Forms.Label();
      this.labelResp_Answer = new System.Windows.Forms.Label();
      this.textBoxResp_Answer = new System.Windows.Forms.TextBox();
      this.listViewResp_Answers = new System.Windows.Forms.ListView();
      this.labelResp_AnswerDate = new System.Windows.Forms.Label();
      this.labelResp_AnswerDateIntro = new System.Windows.Forms.Label();
      this.labelResp_AnswerTimeIntro = new System.Windows.Forms.Label();
      this.labelResp_AnswerFormat = new System.Windows.Forms.Label();
      this.labelResp_AnswerFormatIntro = new System.Windows.Forms.Label();
      this.panelResp_ActualQuestion = new DataObjects.PanelGradient();
      this.textBoxResp_Question = new System.Windows.Forms.TextBox();
      this.labelPanelResp = new System.Windows.Forms.Label();
      this.panelRespondentInfo = new DataObjects.PanelGradient();
      this.panelResp_riRight = new System.Windows.Forms.Panel();
      this.labelResp_Pollster = new System.Windows.Forms.Label();
      this.labelResp_PollsterIntro = new System.Windows.Forms.Label();
      this.labelResp_GPSIntro = new System.Windows.Forms.Label();
      this.labelResp_GPS = new System.Windows.Forms.Label();
      this.panelResp_riLeft = new System.Windows.Forms.Panel();
      this.labelResp_NameIntro = new System.Windows.Forms.Label();
      this.labelResp_Name = new System.Windows.Forms.Label();
      this.labelResp_DateIntro = new System.Windows.Forms.Label();
      this.labelResp_Date = new System.Windows.Forms.Label();
      this.labelResp_TimeIntro = new System.Windows.Forms.Label();
      this.labelResp_Time = new System.Windows.Forms.Label();
      this.labelResp_SexIntro = new System.Windows.Forms.Label();
      this.labelResp_Sex = new System.Windows.Forms.Label();
      this.labelResp_Age = new System.Windows.Forms.Label();
      this.labelResp_AgeIntro = new System.Windows.Forms.Label();
      this.labelResp_EmailIntro = new System.Windows.Forms.Label();
      this.labelResp_Email = new System.Windows.Forms.Label();
      this.panelResp_riMiddle = new System.Windows.Forms.Panel();
      this.labelResp_Address = new System.Windows.Forms.Label();
      this.labelResp_AddressIntro = new System.Windows.Forms.Label();
      this.labelResp_CityIntro = new System.Windows.Forms.Label();
      this.labelResp_City = new System.Windows.Forms.Label();
      this.labelResp_StateProvIntro = new System.Windows.Forms.Label();
      this.labelResp_StateProv = new System.Windows.Forms.Label();
      this.labelResp_PostalCode = new System.Windows.Forms.Label();
      this.labelResp_PostalCodeIntro = new System.Windows.Forms.Label();
      this.labelResp_Tel1Intro = new System.Windows.Forms.Label();
      this.labelResp_Tel1 = new System.Windows.Forms.Label();
      this.buttonRIdisplay = new System.Windows.Forms.Button();
      this.labelPanelRespondentInfo = new System.Windows.Forms.Label();
      this.panelResp_Responses = new DataObjects.PanelGradient();
      this.labelResponsesHeader = new System.Windows.Forms.Label();
      this.panelResp_Questions = new DataObjects.PanelGradient();
      this.labelQuestionsHeader = new System.Windows.Forms.Label();
      this.panelResponsesFilter = new DataObjects.PanelGradient();
      this.labelFilterHeader = new System.Windows.Forms.Label();
      this.listBoxResponsesDate = new System.Windows.Forms.CheckedListBox();
      this.listBoxResponsesSequence = new System.Windows.Forms.CheckedListBox();
      this.panelResponsesTop = new DataObjects.PanelGradient();
      this.labelResponsesTop = new System.Windows.Forms.Label();
      this.tabPageSettings = new System.Windows.Forms.TabPage();
      this.panelSettingsBottom = new PanelGradient.PanelGradient();
      this.panelCompact = new DataObjects.PanelGradient();
      this.buttonExportFilenameSelect = new System.Windows.Forms.Button();
      this.labelExportInstructions = new System.Windows.Forms.Label();
      this.textBoxExportFilename = new System.Windows.Forms.TextBox();
      this.labelExportFilename = new System.Windows.Forms.Label();
      this.checkBoxExportEnabled = new System.Windows.Forms.CheckBox();
      this.panelPassword = new DataObjects.PanelGradient();
      this.labelPasswordInstructions = new System.Windows.Forms.Label();
      this.textBoxOpenPassword = new System.Windows.Forms.TextBox();
      this.labelOpenPassword = new System.Windows.Forms.Label();
      this.labelModifyPassword = new System.Windows.Forms.Label();
      this.textBoxModifyPassword = new System.Windows.Forms.TextBox();
      this.panelPollsterPrivileges = new DataObjects.PanelGradient();
      this.labelPollsterPrivileges = new System.Windows.Forms.Label();
      this.checkBoxReviewData = new System.Windows.Forms.CheckBox();
      this.checkBoxAbortRecord = new System.Windows.Forms.CheckBox();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.panelPurge = new DataObjects.PanelGradient();
      this.comboPurgeDuration = new System.Windows.Forms.ComboBox();
      this.labelPurge = new System.Windows.Forms.Label();
      this.panelGeneralSettings = new DataObjects.PanelGradient();
      this.labelGeneralSettings = new System.Windows.Forms.Label();
      this.checkBoxPersonalInfo = new System.Windows.Forms.CheckBox();
      this.checkBoxHideQuestionNumbers = new System.Windows.Forms.CheckBox();
      this.panelPollSummary = new DataObjects.PanelGradient();
      this.textBoxPollSummary = new System.Windows.Forms.TextBox();
      this.labelPollSummary = new System.Windows.Forms.Label();
      this.panelSettingsTop = new DataObjects.PanelGradient();
      this.labelSettingsTop = new System.Windows.Forms.Label();
      this.panelCEI = new DataObjects.PanelGradient();
      this.panelCEIright = new DataObjects.PanelGradient();
      this.labelLastEdited = new System.Windows.Forms.Label();
      this.labelLastEditDate = new System.Windows.Forms.Label();
      this.labelLastEditedByIntro = new System.Windows.Forms.Label();
      this.labelLastEditedBy = new System.Windows.Forms.Label();
      this.panelCEIleft = new DataObjects.PanelGradient();
      this.labelCreatedOn = new System.Windows.Forms.Label();
      this.labelCreationDate = new System.Windows.Forms.Label();
      this.labelCreatedBy = new System.Windows.Forms.Label();
      this.labelCreatorName = new System.Windows.Forms.Label();
      this.labelRevisionIntro = new System.Windows.Forms.Label();
      this.labelRevisionNumber = new System.Windows.Forms.Label();
      this.DragDropTimer = new System.Windows.Forms.Timer(this.components);
      this.timerAnimateRIpanel = new System.Windows.Forms.Timer(this.components);
      this.timerShowCharts = new System.Windows.Forms.Timer(this.components);
      this.tabNewPoll.SuspendLayout();
      this.tabPageSummary.SuspendLayout();
      this.panelSummaryLower.SuspendLayout();
      this.panelMidUpper.SuspendLayout();
      this.panelSummaryUpper.SuspendLayout();
      this.panelPollCreationInfo.SuspendLayout();
      this.paneSummaryTop.SuspendLayout();
      this.tabPageInstructions.SuspendLayout();
      this.panelBeginMessage.SuspendLayout();
      this.panelBeforePoll.SuspendLayout();
      this.panelAfterPoll.SuspendLayout();
      this.panelEndMessage.SuspendLayout();
      this.panelInstructionsTop.SuspendLayout();
      this.panelAfterAllPolls.SuspendLayout();
      this.tabPageQuestions.SuspendLayout();
      this.panelChoicesTitle.SuspendLayout();
      this.panelPreviewFrame.SuspendLayout();
      this.panelPreviewTitle.SuspendLayout();
      this.panelQuestion.SuspendLayout();
      this.panelQuestionList.SuspendLayout();
      this.panelQuestionList1.SuspendLayout();
      this.panelQuestionList2.SuspendLayout();
      this.panelQuestionsTop.SuspendLayout();
      this.tabPageResponses.SuspendLayout();
      this.panelAnswers.SuspendLayout();
      this.panelResp_ExtraInfo.SuspendLayout();
      this.panelExtraInfoHeader.SuspendLayout();
      this.panelResp_Spinner.SuspendLayout();
      this.panelResp_ActualQuestion.SuspendLayout();
      this.panelRespondentInfo.SuspendLayout();
      this.panelResp_riRight.SuspendLayout();
      this.panelResp_riLeft.SuspendLayout();
      this.panelResp_riMiddle.SuspendLayout();
      this.panelResp_Responses.SuspendLayout();
      this.panelResp_Questions.SuspendLayout();
      this.panelResponsesFilter.SuspendLayout();
      this.panelResponsesTop.SuspendLayout();
      this.tabPageSettings.SuspendLayout();
      this.panelSettingsBottom.SuspendLayout();
      this.panelCompact.SuspendLayout();
      this.panelPassword.SuspendLayout();
      this.panelPollsterPrivileges.SuspendLayout();
      this.panelPurge.SuspendLayout();
      this.panelGeneralSettings.SuspendLayout();
      this.panelPollSummary.SuspendLayout();
      this.panelSettingsTop.SuspendLayout();
      this.panelCEI.SuspendLayout();
      this.panelCEIright.SuspendLayout();
      this.panelCEIleft.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabNewPoll
      // 
      this.tabNewPoll.Appearance = System.Windows.Forms.TabAppearance.Buttons;
      this.tabNewPoll.Controls.Add(this.tabPageSummary);
      this.tabNewPoll.Controls.Add(this.tabPageInstructions);
      this.tabNewPoll.Controls.Add(this.tabPageQuestions);
      this.tabNewPoll.Controls.Add(this.tabPageResponses);
      this.tabNewPoll.Controls.Add(this.tabPageSettings);
      this.tabNewPoll.Cursor = System.Windows.Forms.Cursors.Default;
      this.tabNewPoll.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabNewPoll.Font = new System.Drawing.Font("Arial", 12F);
      this.tabNewPoll.ItemSize = new System.Drawing.Size(170, 50);
      this.tabNewPoll.Location = new System.Drawing.Point(0, 0);
      this.tabNewPoll.Name = "tabNewPoll";
      this.tabNewPoll.Padding = new System.Drawing.Point(20, 3);
      this.tabNewPoll.SelectedIndex = 0;
      this.tabNewPoll.ShowToolTips = true;
      this.tabNewPoll.Size = new System.Drawing.Size(992, 646);
      this.tabNewPoll.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
      this.tabNewPoll.TabIndex = 10;
      this.tabNewPoll.TabStop = false;
      this.tabNewPoll.Click += new System.EventHandler(this.tabNewPoll_Click);
      // 
      // tabPageSummary
      // 
      this.tabPageSummary.BackColor = System.Drawing.Color.AliceBlue;
      this.tabPageSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.tabPageSummary.Controls.Add(this.panelSummaryLower);
      this.tabPageSummary.Controls.Add(this.panelSummaryUpper);
      this.tabPageSummary.Controls.Add(this.paneSummaryTop);
      this.tabPageSummary.Cursor = System.Windows.Forms.Cursors.Default;
      this.tabPageSummary.Location = new System.Drawing.Point(4, 54);
      this.tabPageSummary.Name = "tabPageSummary";
      this.tabPageSummary.Size = new System.Drawing.Size(984, 588);
      this.tabPageSummary.TabIndex = 0;
      this.tabPageSummary.Text = "Summary";
      this.tabPageSummary.ToolTipText = "Provides a general overview of the poll and the responses obtained.";
      // 
      // panelSummaryLower
      // 
      this.panelSummaryLower.Controls.Add(this.panelSummaryCharts);
      this.panelSummaryLower.Controls.Add(this.panelMidUpper);
      this.panelSummaryLower.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelSummaryLower.Location = new System.Drawing.Point(0, 176);
      this.panelSummaryLower.Name = "panelSummaryLower";
      this.panelSummaryLower.Size = new System.Drawing.Size(980, 408);
      this.panelSummaryLower.TabIndex = 12;
      // 
      // panelSummaryCharts
      // 
      this.panelSummaryCharts.AutoScrollMargin = new System.Drawing.Size(0, 20);
      this.panelSummaryCharts.BackColor = System.Drawing.Color.Navy;
      this.panelSummaryCharts.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelSummaryCharts.Location = new System.Drawing.Point(0, 56);
      this.panelSummaryCharts.Name = "panelSummaryCharts";
      this.panelSummaryCharts.Size = new System.Drawing.Size(980, 352);
      this.panelSummaryCharts.TabIndex = 2;
      this.panelSummaryCharts.Resize += new System.EventHandler(this.panelSummaryCharts_Resize);
      // 
      // panelMidUpper
      // 
      this.panelMidUpper.Controls.Add(this.labelSummaryQuestionsTally);
      this.panelMidUpper.Controls.Add(this.labelSummaryResponsesTally);
      this.panelMidUpper.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelMidUpper.Location = new System.Drawing.Point(0, 0);
      this.panelMidUpper.Name = "panelMidUpper";
      this.panelMidUpper.Size = new System.Drawing.Size(980, 56);
      this.panelMidUpper.TabIndex = 1;
      this.panelMidUpper.Resize += new System.EventHandler(this.panelMidUpper_Resize);
      // 
      // labelSummaryQuestionsTally
      // 
      this.labelSummaryQuestionsTally.AutoSize = true;
      this.labelSummaryQuestionsTally.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelSummaryQuestionsTally.ForeColor = System.Drawing.Color.DarkBlue;
      this.labelSummaryQuestionsTally.Location = new System.Drawing.Point(136, 12);
      this.labelSummaryQuestionsTally.Name = "labelSummaryQuestionsTally";
      this.labelSummaryQuestionsTally.Size = new System.Drawing.Size(140, 31);
      this.labelSummaryQuestionsTally.TabIndex = 2;
      this.labelSummaryQuestionsTally.Text = "# Questions";
      this.labelSummaryQuestionsTally.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // labelSummaryResponsesTally
      // 
      this.labelSummaryResponsesTally.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelSummaryResponsesTally.AutoSize = true;
      this.labelSummaryResponsesTally.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelSummaryResponsesTally.ForeColor = System.Drawing.Color.GhostWhite;
      this.labelSummaryResponsesTally.Location = new System.Drawing.Point(696, 12);
      this.labelSummaryResponsesTally.Name = "labelSummaryResponsesTally";
      this.labelSummaryResponsesTally.Size = new System.Drawing.Size(152, 31);
      this.labelSummaryResponsesTally.TabIndex = 1;
      this.labelSummaryResponsesTally.Text = "# Responses";
      this.labelSummaryResponsesTally.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // panelSummaryUpper
      // 
      this.panelSummaryUpper.Controls.Add(this.panelPollCreationInfo);
      this.panelSummaryUpper.Controls.Add(this.label1);
      this.panelSummaryUpper.Controls.Add(this.textBoxSummaryPollSummary);
      this.panelSummaryUpper.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelSummaryUpper.GradientColorTwo = System.Drawing.Color.SeaGreen;
      this.panelSummaryUpper.Location = new System.Drawing.Point(0, 40);
      this.panelSummaryUpper.Name = "panelSummaryUpper";
      this.panelSummaryUpper.Size = new System.Drawing.Size(980, 136);
      this.panelSummaryUpper.TabIndex = 11;
      // 
      // panelPollCreationInfo
      // 
      this.panelPollCreationInfo.Controls.Add(this.labelSummaryProductID);
      this.panelPollCreationInfo.Controls.Add(this.label2);
      this.panelPollCreationInfo.Controls.Add(this.label3);
      this.panelPollCreationInfo.Controls.Add(this.label4);
      this.panelPollCreationInfo.Controls.Add(this.label5);
      this.panelPollCreationInfo.Controls.Add(this.label6);
      this.panelPollCreationInfo.Controls.Add(this.label7);
      this.panelPollCreationInfo.Controls.Add(this.label8);
      this.panelPollCreationInfo.Controls.Add(this.label9);
      this.panelPollCreationInfo.Controls.Add(this.label10);
      this.panelPollCreationInfo.Controls.Add(this.label11);
      this.panelPollCreationInfo.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelPollCreationInfo.Location = new System.Drawing.Point(524, 0);
      this.panelPollCreationInfo.Name = "panelPollCreationInfo";
      this.panelPollCreationInfo.Size = new System.Drawing.Size(456, 136);
      this.panelPollCreationInfo.TabIndex = 16;
      // 
      // labelSummaryProductID
      // 
      this.labelSummaryProductID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelSummaryProductID.Location = new System.Drawing.Point(117, 72);
      this.labelSummaryProductID.Name = "labelSummaryProductID";
      this.labelSummaryProductID.Size = new System.Drawing.Size(72, 16);
      this.labelSummaryProductID.TabIndex = 13;
      this.labelSummaryProductID.Tag = "Property=CreationInfo.ProductID";
      this.labelSummaryProductID.Text = "ProductID";
      // 
      // label2
      // 
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label2.Location = new System.Drawing.Point(8, 8);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(146, 16);
      this.label2.TabIndex = 12;
      this.label2.Text = "Poll Creation Info";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label3.Location = new System.Drawing.Point(16, 40);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(87, 18);
      this.label3.TabIndex = 13;
      this.label3.Tag = "";
      this.label3.Text = "Created With:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label4.Location = new System.Drawing.Point(24, 72);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(94, 18);
      this.label4.TabIndex = 13;
      this.label4.Tag = "";
      this.label4.Text = "Pocket Pollster";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label5.Location = new System.Drawing.Point(24, 107);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(54, 18);
      this.label5.TabIndex = 13;
      this.label5.Tag = "";
      this.label5.Text = "Version:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label6.Location = new System.Drawing.Point(79, 107);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(46, 18);
      this.label6.TabIndex = 13;
      this.label6.Tag = "Property=CreationInfo.VersionNumber";
      this.label6.Text = "1.0.0.0";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label7.Location = new System.Drawing.Point(256, 40);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(75, 18);
      this.label7.TabIndex = 13;
      this.label7.Tag = "";
      this.label7.Text = "Created By:";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label8.ForeColor = System.Drawing.Color.Yellow;
      this.label8.Location = new System.Drawing.Point(339, 72);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(66, 18);
      this.label8.TabIndex = 13;
      this.label8.Tag = "Property=CreationInfo.CreatorName";
      this.label8.Text = "R. Werner";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label9.Location = new System.Drawing.Point(297, 107);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(37, 18);
      this.label9.TabIndex = 13;
      this.label9.Tag = "";
      this.label9.Text = "Date:";
      this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label10.ForeColor = System.Drawing.Color.Yellow;
      this.label10.Location = new System.Drawing.Point(339, 107);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(73, 18);
      this.label10.TabIndex = 13;
      this.label10.Tag = "Property=CreationInfo.CreationDate";
      this.label10.Text = "2006-05-07";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label11.Location = new System.Drawing.Point(264, 72);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(70, 18);
      this.label11.TabIndex = 13;
      this.label11.Tag = "";
      this.label11.Text = "Username:";
      this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label1.Location = new System.Drawing.Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(91, 18);
      this.label1.TabIndex = 15;
      this.label1.Text = "Poll Summary";
      // 
      // textBoxSummaryPollSummary
      // 
      this.textBoxSummaryPollSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxSummaryPollSummary.BackColor = System.Drawing.Color.LightYellow;
      this.textBoxSummaryPollSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.textBoxSummaryPollSummary.Location = new System.Drawing.Point(8, 32);
      this.textBoxSummaryPollSummary.Multiline = true;
      this.textBoxSummaryPollSummary.Name = "textBoxSummaryPollSummary";
      this.textBoxSummaryPollSummary.ReadOnly = true;
      this.textBoxSummaryPollSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxSummaryPollSummary.Size = new System.Drawing.Size(440, 92);
      this.textBoxSummaryPollSummary.TabIndex = 14;
      this.textBoxSummaryPollSummary.Tag = "Property=CreationInfo.PollSummary";
      this.textBoxSummaryPollSummary.Text = "";
      this.textBoxSummaryPollSummary.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSummaryPollSummary_KeyDown);
      this.textBoxSummaryPollSummary.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBoxReadOnly_MouseDown);
      this.textBoxSummaryPollSummary.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxReadOnly_KeyPress);
      // 
      // paneSummaryTop
      // 
      this.paneSummaryTop.Controls.Add(this.labelSummaryTop);
      this.paneSummaryTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.paneSummaryTop.GradientColorOne = System.Drawing.Color.AliceBlue;
      this.paneSummaryTop.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.paneSummaryTop.Location = new System.Drawing.Point(0, 0);
      this.paneSummaryTop.Name = "paneSummaryTop";
      this.paneSummaryTop.Size = new System.Drawing.Size(980, 40);
      this.paneSummaryTop.TabIndex = 13;
      // 
      // labelSummaryTop
      // 
      this.labelSummaryTop.AutoSize = true;
      this.labelSummaryTop.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
      this.labelSummaryTop.ForeColor = System.Drawing.Color.Blue;
      this.labelSummaryTop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelSummaryTop.Location = new System.Drawing.Point(8, 9);
      this.labelSummaryTop.Name = "labelSummaryTop";
      this.labelSummaryTop.Size = new System.Drawing.Size(746, 23);
      this.labelSummaryTop.TabIndex = 1;
      this.labelSummaryTop.Text = "This screen provides a general overview of the poll and the responses obtained.";
      // 
      // tabPageInstructions
      // 
      this.tabPageInstructions.BackColor = System.Drawing.Color.AliceBlue;
      this.tabPageInstructions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.tabPageInstructions.Controls.Add(this.panelBeginMessage);
      this.tabPageInstructions.Controls.Add(this.panelBeforePoll);
      this.tabPageInstructions.Controls.Add(this.panelAfterPoll);
      this.tabPageInstructions.Controls.Add(this.panelEndMessage);
      this.tabPageInstructions.Controls.Add(this.panelInstructionsTop);
      this.tabPageInstructions.Controls.Add(this.panelAfterAllPolls);
      this.tabPageInstructions.Location = new System.Drawing.Point(4, 54);
      this.tabPageInstructions.Name = "tabPageInstructions";
      this.tabPageInstructions.Size = new System.Drawing.Size(984, 588);
      this.tabPageInstructions.TabIndex = 4;
      this.tabPageInstructions.Text = "Instructions";
      this.tabPageInstructions.ToolTipText = "Lets you add specific instructions for each pollster.";
      this.tabPageInstructions.Resize += new System.EventHandler(this.tabPageInstructions_Resize);
      // 
      // panelBeginMessage
      // 
      this.panelBeginMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.panelBeginMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelBeginMessage.Controls.Add(this.textBoxBeginMessage);
      this.panelBeginMessage.Controls.Add(this.labelBeginMessage);
      this.panelBeginMessage.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(245)), ((System.Byte)(163)), ((System.Byte)(163)));
      this.panelBeginMessage.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(185)), ((System.Byte)(23)), ((System.Byte)(23)));
      this.panelBeginMessage.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelBeginMessage.Location = new System.Drawing.Point(0, 296);
      this.panelBeginMessage.Name = "panelBeginMessage";
      this.panelBeginMessage.Size = new System.Drawing.Size(458, 176);
      this.panelBeginMessage.TabIndex = 7;
      // 
      // textBoxBeginMessage
      // 
      this.textBoxBeginMessage.AcceptsReturn = true;
      this.textBoxBeginMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxBeginMessage.AutoSize = false;
      this.textBoxBeginMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.textBoxBeginMessage.Location = new System.Drawing.Point(7, 30);
      this.textBoxBeginMessage.Multiline = true;
      this.textBoxBeginMessage.Name = "textBoxBeginMessage";
      this.textBoxBeginMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxBeginMessage.Size = new System.Drawing.Size(434, 134);
      this.textBoxBeginMessage.TabIndex = 0;
      this.textBoxBeginMessage.Tag = "Property=Instructions.BeginMessage, PubLock=Instructions";
      this.textBoxBeginMessage.Text = "";
      // 
      // labelBeginMessage
      // 
      this.labelBeginMessage.AutoSize = true;
      this.labelBeginMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelBeginMessage.ForeColor = System.Drawing.Color.Black;
      this.labelBeginMessage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelBeginMessage.Location = new System.Drawing.Point(6, 5);
      this.labelBeginMessage.Name = "labelBeginMessage";
      this.labelBeginMessage.Size = new System.Drawing.Size(286, 18);
      this.labelBeginMessage.TabIndex = 1;
      this.labelBeginMessage.Text = "Opening instructions for the pollster to read out:";
      // 
      // panelBeforePoll
      // 
      this.panelBeforePoll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelBeforePoll.Controls.Add(this.checkBoxRepeatBeforePoll);
      this.panelBeforePoll.Controls.Add(this.textBoxBeforePoll);
      this.panelBeforePoll.Controls.Add(this.labelBeforePoll);
      this.panelBeforePoll.GradientColorOne = System.Drawing.Color.Lavender;
      this.panelBeforePoll.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(75)), ((System.Byte)(119)), ((System.Byte)(173)));
      this.panelBeforePoll.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelBeforePoll.Location = new System.Drawing.Point(0, 40);
      this.panelBeforePoll.Name = "panelBeforePoll";
      this.panelBeforePoll.Size = new System.Drawing.Size(458, 176);
      this.panelBeforePoll.TabIndex = 6;
      // 
      // checkBoxRepeatBeforePoll
      // 
      this.checkBoxRepeatBeforePoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkBoxRepeatBeforePoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.checkBoxRepeatBeforePoll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.checkBoxRepeatBeforePoll.Location = new System.Drawing.Point(12, 144);
      this.checkBoxRepeatBeforePoll.Name = "checkBoxRepeatBeforePoll";
      this.checkBoxRepeatBeforePoll.Size = new System.Drawing.Size(314, 24);
      this.checkBoxRepeatBeforePoll.TabIndex = 1;
      this.checkBoxRepeatBeforePoll.Tag = "Property=Instructions.RepeatBeforePoll, PubLock=Instructions";
      this.checkBoxRepeatBeforePoll.Text = "Repeat before every poll";
      // 
      // textBoxBeforePoll
      // 
      this.textBoxBeforePoll.AcceptsReturn = true;
      this.textBoxBeforePoll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxBeforePoll.AutoSize = false;
      this.textBoxBeforePoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.textBoxBeforePoll.Location = new System.Drawing.Point(7, 30);
      this.textBoxBeforePoll.Multiline = true;
      this.textBoxBeforePoll.Name = "textBoxBeforePoll";
      this.textBoxBeforePoll.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxBeforePoll.Size = new System.Drawing.Size(434, 106);
      this.textBoxBeforePoll.TabIndex = 0;
      this.textBoxBeforePoll.Tag = "Property=Instructions.BeforePoll, PubLock=Instructions";
      this.textBoxBeforePoll.Text = "";
      // 
      // labelBeforePoll
      // 
      this.labelBeforePoll.AutoSize = true;
      this.labelBeforePoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelBeforePoll.ForeColor = System.Drawing.Color.Black;
      this.labelBeforePoll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelBeforePoll.Location = new System.Drawing.Point(6, 5);
      this.labelBeforePoll.Name = "labelBeforePoll";
      this.labelBeforePoll.Size = new System.Drawing.Size(298, 18);
      this.labelBeforePoll.TabIndex = 1;
      this.labelBeforePoll.Text = "Private instructions for the pollster before the poll:";
      // 
      // panelAfterPoll
      // 
      this.panelAfterPoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.panelAfterPoll.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelAfterPoll.Controls.Add(this.checkBoxRepeatAfterPoll);
      this.panelAfterPoll.Controls.Add(this.textBoxAfterPoll);
      this.panelAfterPoll.Controls.Add(this.labelAfterPoll);
      this.panelAfterPoll.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(224)), ((System.Byte)(204)), ((System.Byte)(224)));
      this.panelAfterPoll.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(152)), ((System.Byte)(96)), ((System.Byte)(152)));
      this.panelAfterPoll.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelAfterPoll.Location = new System.Drawing.Point(520, 40);
      this.panelAfterPoll.Name = "panelAfterPoll";
      this.panelAfterPoll.Size = new System.Drawing.Size(458, 176);
      this.panelAfterPoll.TabIndex = 5;
      // 
      // checkBoxRepeatAfterPoll
      // 
      this.checkBoxRepeatAfterPoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkBoxRepeatAfterPoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.checkBoxRepeatAfterPoll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.checkBoxRepeatAfterPoll.Location = new System.Drawing.Point(12, 144);
      this.checkBoxRepeatAfterPoll.Name = "checkBoxRepeatAfterPoll";
      this.checkBoxRepeatAfterPoll.Size = new System.Drawing.Size(314, 24);
      this.checkBoxRepeatAfterPoll.TabIndex = 1;
      this.checkBoxRepeatAfterPoll.Tag = "Property=Instructions.RepeatAfterPoll, PubLock=Instructions";
      this.checkBoxRepeatAfterPoll.Text = "Repeat after every poll";
      // 
      // textBoxAfterPoll
      // 
      this.textBoxAfterPoll.AcceptsReturn = true;
      this.textBoxAfterPoll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxAfterPoll.AutoSize = false;
      this.textBoxAfterPoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.textBoxAfterPoll.Location = new System.Drawing.Point(7, 30);
      this.textBoxAfterPoll.Multiline = true;
      this.textBoxAfterPoll.Name = "textBoxAfterPoll";
      this.textBoxAfterPoll.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxAfterPoll.Size = new System.Drawing.Size(434, 106);
      this.textBoxAfterPoll.TabIndex = 0;
      this.textBoxAfterPoll.Tag = "Property=Instructions.AfterPoll, PubLock=Instructions";
      this.textBoxAfterPoll.Text = "";
      // 
      // labelAfterPoll
      // 
      this.labelAfterPoll.AutoSize = true;
      this.labelAfterPoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelAfterPoll.ForeColor = System.Drawing.Color.Black;
      this.labelAfterPoll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelAfterPoll.Location = new System.Drawing.Point(6, 5);
      this.labelAfterPoll.Name = "labelAfterPoll";
      this.labelAfterPoll.Size = new System.Drawing.Size(297, 18);
      this.labelAfterPoll.TabIndex = 1;
      this.labelAfterPoll.Text = "Private instructions for the pollster after each poll:";
      // 
      // panelEndMessage
      // 
      this.panelEndMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.panelEndMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelEndMessage.Controls.Add(this.textBoxEndMessage);
      this.panelEndMessage.Controls.Add(this.labelEndMessage);
      this.panelEndMessage.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(178)), ((System.Byte)(230)), ((System.Byte)(212)));
      this.panelEndMessage.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(47)), ((System.Byte)(141)), ((System.Byte)(109)));
      this.panelEndMessage.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelEndMessage.Location = new System.Drawing.Point(520, 296);
      this.panelEndMessage.Name = "panelEndMessage";
      this.panelEndMessage.Size = new System.Drawing.Size(458, 176);
      this.panelEndMessage.TabIndex = 4;
      // 
      // textBoxEndMessage
      // 
      this.textBoxEndMessage.AcceptsReturn = true;
      this.textBoxEndMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxEndMessage.AutoSize = false;
      this.textBoxEndMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.textBoxEndMessage.Location = new System.Drawing.Point(7, 30);
      this.textBoxEndMessage.Multiline = true;
      this.textBoxEndMessage.Name = "textBoxEndMessage";
      this.textBoxEndMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxEndMessage.Size = new System.Drawing.Size(434, 134);
      this.textBoxEndMessage.TabIndex = 0;
      this.textBoxEndMessage.Tag = "Property=Instructions.EndMessage, PubLock=Instructions";
      this.textBoxEndMessage.Text = "";
      // 
      // labelEndMessage
      // 
      this.labelEndMessage.AutoSize = true;
      this.labelEndMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelEndMessage.ForeColor = System.Drawing.Color.Black;
      this.labelEndMessage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelEndMessage.Location = new System.Drawing.Point(6, 5);
      this.labelEndMessage.Name = "labelEndMessage";
      this.labelEndMessage.Size = new System.Drawing.Size(280, 18);
      this.labelEndMessage.TabIndex = 1;
      this.labelEndMessage.Text = "Closing instructions for the pollster to read out:";
      // 
      // panelInstructionsTop
      // 
      this.panelInstructionsTop.Controls.Add(this.labelInstructionsTop);
      this.panelInstructionsTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelInstructionsTop.GradientColorOne = System.Drawing.Color.AliceBlue;
      this.panelInstructionsTop.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.panelInstructionsTop.Location = new System.Drawing.Point(0, 0);
      this.panelInstructionsTop.Name = "panelInstructionsTop";
      this.panelInstructionsTop.Size = new System.Drawing.Size(980, 40);
      this.panelInstructionsTop.TabIndex = 2;
      // 
      // labelInstructionsTop
      // 
      this.labelInstructionsTop.AutoSize = true;
      this.labelInstructionsTop.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
      this.labelInstructionsTop.ForeColor = System.Drawing.Color.Blue;
      this.labelInstructionsTop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelInstructionsTop.Location = new System.Drawing.Point(8, 9);
      this.labelInstructionsTop.Name = "labelInstructionsTop";
      this.labelInstructionsTop.Size = new System.Drawing.Size(584, 23);
      this.labelInstructionsTop.TabIndex = 1;
      this.labelInstructionsTop.Text = "This screen lets you add specific instructions for each pollster.";
      // 
      // panelAfterAllPolls
      // 
      this.panelAfterAllPolls.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelAfterAllPolls.Controls.Add(this.textBoxAfterAllPolls);
      this.panelAfterAllPolls.Controls.Add(this.labelAfterAllPolls);
      this.panelAfterAllPolls.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelAfterAllPolls.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(224)), ((System.Byte)(193)));
      this.panelAfterAllPolls.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(240)), ((System.Byte)(119)), ((System.Byte)(0)));
      this.panelAfterAllPolls.ImeMode = System.Windows.Forms.ImeMode.Disable;
      this.panelAfterAllPolls.Location = new System.Drawing.Point(0, 484);
      this.panelAfterAllPolls.Name = "panelAfterAllPolls";
      this.panelAfterAllPolls.Size = new System.Drawing.Size(980, 100);
      this.panelAfterAllPolls.TabIndex = 1;
      // 
      // textBoxAfterAllPolls
      // 
      this.textBoxAfterAllPolls.AcceptsReturn = true;
      this.textBoxAfterAllPolls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxAfterAllPolls.AutoSize = false;
      this.textBoxAfterAllPolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.textBoxAfterAllPolls.Location = new System.Drawing.Point(7, 30);
      this.textBoxAfterAllPolls.Multiline = true;
      this.textBoxAfterAllPolls.Name = "textBoxAfterAllPolls";
      this.textBoxAfterAllPolls.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxAfterAllPolls.Size = new System.Drawing.Size(956, 60);
      this.textBoxAfterAllPolls.TabIndex = 0;
      this.textBoxAfterAllPolls.Tag = "Property=Instructions.AfterAllPolls, PubLock=Instructions";
      this.textBoxAfterAllPolls.Text = "";
      // 
      // labelAfterAllPolls
      // 
      this.labelAfterAllPolls.AutoSize = true;
      this.labelAfterAllPolls.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelAfterAllPolls.ForeColor = System.Drawing.Color.Black;
      this.labelAfterAllPolls.Location = new System.Drawing.Point(6, 5);
      this.labelAfterAllPolls.Name = "labelAfterAllPolls";
      this.labelAfterAllPolls.Size = new System.Drawing.Size(375, 18);
      this.labelAfterAllPolls.TabIndex = 1;
      this.labelAfterAllPolls.Text = "Private final instructions for the pollster after all polling is done:";
      // 
      // tabPageQuestions
      // 
      this.tabPageQuestions.BackColor = System.Drawing.Color.AliceBlue;
      this.tabPageQuestions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.tabPageQuestions.Controls.Add(this.panelChoices);
      this.tabPageQuestions.Controls.Add(this.panelChoicesTitle);
      this.tabPageQuestions.Controls.Add(this.panelPreviewFrame);
      this.tabPageQuestions.Controls.Add(this.panelQuestion);
      this.tabPageQuestions.Controls.Add(this.panelQuestionList);
      this.tabPageQuestions.Controls.Add(this.panelAnswerFormat);
      this.tabPageQuestions.Controls.Add(this.panelQuestionsTop);
      this.tabPageQuestions.Location = new System.Drawing.Point(4, 54);
      this.tabPageQuestions.Name = "tabPageQuestions";
      this.tabPageQuestions.Size = new System.Drawing.Size(984, 588);
      this.tabPageQuestions.TabIndex = 2;
      this.tabPageQuestions.Text = "Questions & Answers";
      this.tabPageQuestions.ToolTipText = "Guides you through the creation of the questions of the poll.";
      // 
      // panelChoices
      // 
      this.panelChoices.AllowDrop = true;
      this.panelChoices.AutoScroll = true;
      this.panelChoices.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelChoices.GradientColorOne = System.Drawing.Color.Honeydew;
      this.panelChoices.GradientColorTwo = System.Drawing.Color.SeaGreen;
      this.panelChoices.Location = new System.Drawing.Point(0, 232);
      this.panelChoices.Name = "panelChoices";
      this.panelChoices.Size = new System.Drawing.Size(608, 352);
      this.panelChoices.TabIndex = 8;
      this.panelChoices.Tag = "";
      this.panelChoices.DragEnter += new System.Windows.Forms.DragEventHandler(this.GeneralPanel_DragEnter);
      this.panelChoices.DragDrop += new System.Windows.Forms.DragEventHandler(this.GeneralPanel_DragDrop);
      this.panelChoices.DragOver += new System.Windows.Forms.DragEventHandler(this.GeneralPanel_DragOver);
      // 
      // panelChoicesTitle
      // 
      this.panelChoicesTitle.Controls.Add(this.buttonDuplicateChoice);
      this.panelChoicesTitle.Controls.Add(this.buttonRemoveChoice);
      this.panelChoicesTitle.Controls.Add(this.buttonAddChoice);
      this.panelChoicesTitle.Controls.Add(this.labelChoicesTitle);
      this.panelChoicesTitle.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelChoicesTitle.GradientColorOne = System.Drawing.Color.Honeydew;
      this.panelChoicesTitle.GradientColorTwo = System.Drawing.Color.SeaGreen;
      this.panelChoicesTitle.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
      this.panelChoicesTitle.Location = new System.Drawing.Point(0, 184);
      this.panelChoicesTitle.Name = "panelChoicesTitle";
      this.panelChoicesTitle.Size = new System.Drawing.Size(608, 48);
      this.panelChoicesTitle.TabIndex = 13;
      // 
      // buttonDuplicateChoice
      // 
      this.buttonDuplicateChoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDuplicateChoice.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(185)), ((System.Byte)(215)), ((System.Byte)(185)));
      this.buttonDuplicateChoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.buttonDuplicateChoice.Location = new System.Drawing.Point(450, 12);
      this.buttonDuplicateChoice.Name = "buttonDuplicateChoice";
      this.buttonDuplicateChoice.Size = new System.Drawing.Size(72, 24);
      this.buttonDuplicateChoice.TabIndex = 5;
      this.buttonDuplicateChoice.Text = "Duplicate";
      this.buttonDuplicateChoice.Click += new System.EventHandler(this.Choice_Event);
      // 
      // buttonRemoveChoice
      // 
      this.buttonRemoveChoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonRemoveChoice.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(185)), ((System.Byte)(215)), ((System.Byte)(185)));
      this.buttonRemoveChoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.buttonRemoveChoice.Location = new System.Drawing.Point(528, 12);
      this.buttonRemoveChoice.Name = "buttonRemoveChoice";
      this.buttonRemoveChoice.Size = new System.Drawing.Size(72, 24);
      this.buttonRemoveChoice.TabIndex = 4;
      this.buttonRemoveChoice.Text = "Remove";
      this.buttonRemoveChoice.Click += new System.EventHandler(this.Choice_Event);
      // 
      // buttonAddChoice
      // 
      this.buttonAddChoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonAddChoice.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(185)), ((System.Byte)(215)), ((System.Byte)(185)));
      this.buttonAddChoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.buttonAddChoice.Location = new System.Drawing.Point(372, 12);
      this.buttonAddChoice.Name = "buttonAddChoice";
      this.buttonAddChoice.Size = new System.Drawing.Size(72, 24);
      this.buttonAddChoice.TabIndex = 3;
      this.buttonAddChoice.Text = "Add";
      this.buttonAddChoice.Click += new System.EventHandler(this.Choice_Event);
      // 
      // labelChoicesTitle
      // 
      this.labelChoicesTitle.AutoSize = true;
      this.labelChoicesTitle.Font = new System.Drawing.Font("Arial Black", 12F);
      this.labelChoicesTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelChoicesTitle.Location = new System.Drawing.Point(8, 6);
      this.labelChoicesTitle.Name = "labelChoicesTitle";
      this.labelChoicesTitle.Size = new System.Drawing.Size(84, 26);
      this.labelChoicesTitle.TabIndex = 2;
      this.labelChoicesTitle.Text = "Answers";
      // 
      // panelPreviewFrame
      // 
      this.panelPreviewFrame.AutoScroll = true;
      this.panelPreviewFrame.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelPreviewFrame.Controls.Add(this.panelPreview);
      this.panelPreviewFrame.Controls.Add(this.panelPreviewTitle);
      this.panelPreviewFrame.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelPreviewFrame.Location = new System.Drawing.Point(608, 184);
      this.panelPreviewFrame.Name = "panelPreviewFrame";
      this.panelPreviewFrame.Size = new System.Drawing.Size(272, 400);
      this.panelPreviewFrame.TabIndex = 12;
      // 
      // panelPreview
      // 
      this.panelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelPreview.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(233)));
      this.panelPreview.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(247)), ((System.Byte)(33)));
      this.panelPreview.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
      this.panelPreview.Location = new System.Drawing.Point(0, 44);
      this.panelPreview.Name = "panelPreview";
      this.panelPreview.Size = new System.Drawing.Size(268, 352);
      this.panelPreview.TabIndex = 4;
      // 
      // panelPreviewTitle
      // 
      this.panelPreviewTitle.Controls.Add(this.labelPreview);
      this.panelPreviewTitle.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelPreviewTitle.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(233)));
      this.panelPreviewTitle.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(249)), ((System.Byte)(73)));
      this.panelPreviewTitle.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
      this.panelPreviewTitle.Location = new System.Drawing.Point(0, 0);
      this.panelPreviewTitle.Name = "panelPreviewTitle";
      this.panelPreviewTitle.Size = new System.Drawing.Size(268, 44);
      this.panelPreviewTitle.TabIndex = 3;
      // 
      // labelPreview
      // 
      this.labelPreview.AutoSize = true;
      this.labelPreview.Font = new System.Drawing.Font("Arial Black", 12F);
      this.labelPreview.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelPreview.Location = new System.Drawing.Point(8, 6);
      this.labelPreview.Name = "labelPreview";
      this.labelPreview.Size = new System.Drawing.Size(78, 26);
      this.labelPreview.TabIndex = 3;
      this.labelPreview.Text = "Preview";
      // 
      // panelQuestion
      // 
      this.panelQuestion.Controls.Add(this.textQuestion);
      this.panelQuestion.Controls.Add(this.labelQuestion);
      this.panelQuestion.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelQuestion.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(233)), ((System.Byte)(233)));
      this.panelQuestion.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(168)), ((System.Byte)(16)), ((System.Byte)(0)));
      this.panelQuestion.Location = new System.Drawing.Point(0, 88);
      this.panelQuestion.Name = "panelQuestion";
      this.panelQuestion.Size = new System.Drawing.Size(880, 96);
      this.panelQuestion.TabIndex = 7;
      // 
      // textQuestion
      // 
      this.textQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textQuestion.ForeColor = System.Drawing.SystemColors.WindowText;
      this.textQuestion.Location = new System.Drawing.Point(104, 24);
      this.textQuestion.Multiline = true;
      this.textQuestion.Name = "textQuestion";
      this.textQuestion.Size = new System.Drawing.Size(752, 56);
      this.textQuestion.TabIndex = 4;
      this.textQuestion.Tag = "Property=Questions[].Text, PubLock=QA_Basic";
      this.textQuestion.Text = "Enter your question here . . .";
      this.textQuestion.DoubleClick += new System.EventHandler(this.textQuestion_DoubleClick);
      this.textQuestion.Enter += new System.EventHandler(this.textQuestion_Enter);
      // 
      // labelQuestion
      // 
      this.labelQuestion.AutoSize = true;
      this.labelQuestion.Font = new System.Drawing.Font("Arial Black", 12F);
      this.labelQuestion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelQuestion.Location = new System.Drawing.Point(6, 6);
      this.labelQuestion.Name = "labelQuestion";
      this.labelQuestion.Size = new System.Drawing.Size(86, 26);
      this.labelQuestion.TabIndex = 0;
      this.labelQuestion.Text = "Question";
      // 
      // panelQuestionList
      // 
      this.panelQuestionList.Controls.Add(this.panelQuestionList1);
      this.panelQuestionList.Controls.Add(this.panelQuestionList2);
      this.panelQuestionList.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelQuestionList.Location = new System.Drawing.Point(880, 88);
      this.panelQuestionList.Name = "panelQuestionList";
      this.panelQuestionList.Size = new System.Drawing.Size(100, 496);
      this.panelQuestionList.TabIndex = 5;
      this.panelQuestionList.TabStop = true;
      // 
      // panelQuestionList1
      // 
      this.panelQuestionList1.AllowDrop = true;
      this.panelQuestionList1.AutoScroll = true;
      this.panelQuestionList1.Controls.Add(this.labelQA_QuestionsHeader);
      this.panelQuestionList1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelQuestionList1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelQuestionList1.Location = new System.Drawing.Point(0, 0);
      this.panelQuestionList1.Name = "panelQuestionList1";
      this.panelQuestionList1.Size = new System.Drawing.Size(100, 400);
      this.panelQuestionList1.TabIndex = 12;
      this.panelQuestionList1.Resize += new System.EventHandler(this.panelQuestionList1_Resize);
      this.panelQuestionList1.DragEnter += new System.Windows.Forms.DragEventHandler(this.GeneralPanel_DragEnter);
      this.panelQuestionList1.DragLeave += new System.EventHandler(this.GeneralPanel_DragLeave);
      this.panelQuestionList1.DragDrop += new System.Windows.Forms.DragEventHandler(this.GeneralPanel_DragDrop);
      this.panelQuestionList1.DragOver += new System.Windows.Forms.DragEventHandler(this.GeneralPanel_DragOver);
      // 
      // labelQA_QuestionsHeader
      // 
      this.labelQA_QuestionsHeader.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelQA_QuestionsHeader.ForeColor = System.Drawing.Color.Red;
      this.labelQA_QuestionsHeader.Location = new System.Drawing.Point(10, 8);
      this.labelQA_QuestionsHeader.Name = "labelQA_QuestionsHeader";
      this.labelQA_QuestionsHeader.Size = new System.Drawing.Size(80, 28);
      this.labelQA_QuestionsHeader.TabIndex = 1;
      this.labelQA_QuestionsHeader.Text = "Questions";
      this.labelQA_QuestionsHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // panelQuestionList2
      // 
      this.panelQuestionList2.Controls.Add(this.buttonAddQuestion);
      this.panelQuestionList2.Controls.Add(this.buttonRemoveQuestion);
      this.panelQuestionList2.Controls.Add(this.buttonDuplicateQuestion);
      this.panelQuestionList2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelQuestionList2.GradientColorOne = System.Drawing.Color.Blue;
      this.panelQuestionList2.Location = new System.Drawing.Point(0, 400);
      this.panelQuestionList2.Name = "panelQuestionList2";
      this.panelQuestionList2.Size = new System.Drawing.Size(100, 96);
      this.panelQuestionList2.TabIndex = 13;
      // 
      // buttonAddQuestion
      // 
      this.buttonAddQuestion.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(170)), ((System.Byte)(190)), ((System.Byte)(255)));
      this.buttonAddQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.buttonAddQuestion.Location = new System.Drawing.Point(15, 9);
      this.buttonAddQuestion.Name = "buttonAddQuestion";
      this.buttonAddQuestion.Size = new System.Drawing.Size(72, 24);
      this.buttonAddQuestion.TabIndex = 0;
      this.buttonAddQuestion.Text = "Add";
      this.buttonAddQuestion.Click += new System.EventHandler(this.QuestionButton_Event);
      // 
      // buttonRemoveQuestion
      // 
      this.buttonRemoveQuestion.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(170)), ((System.Byte)(190)), ((System.Byte)(255)));
      this.buttonRemoveQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.buttonRemoveQuestion.Location = new System.Drawing.Point(15, 67);
      this.buttonRemoveQuestion.Name = "buttonRemoveQuestion";
      this.buttonRemoveQuestion.Size = new System.Drawing.Size(72, 24);
      this.buttonRemoveQuestion.TabIndex = 0;
      this.buttonRemoveQuestion.Text = "Remove";
      this.buttonRemoveQuestion.Click += new System.EventHandler(this.QuestionButton_Event);
      // 
      // buttonDuplicateQuestion
      // 
      this.buttonDuplicateQuestion.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(170)), ((System.Byte)(190)), ((System.Byte)(255)));
      this.buttonDuplicateQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.buttonDuplicateQuestion.Location = new System.Drawing.Point(15, 38);
      this.buttonDuplicateQuestion.Name = "buttonDuplicateQuestion";
      this.buttonDuplicateQuestion.Size = new System.Drawing.Size(72, 24);
      this.buttonDuplicateQuestion.TabIndex = 0;
      this.buttonDuplicateQuestion.Text = "Duplicate";
      this.buttonDuplicateQuestion.Click += new System.EventHandler(this.QuestionButton_Event);
      // 
      // panelAnswerFormat
      // 
      this.panelAnswerFormat.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelAnswerFormat.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(225)), ((System.Byte)(233)), ((System.Byte)(243)));
      this.panelAnswerFormat.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(75)), ((System.Byte)(119)), ((System.Byte)(173)));
      this.panelAnswerFormat.Location = new System.Drawing.Point(0, 40);
      this.panelAnswerFormat.Name = "panelAnswerFormat";
      this.panelAnswerFormat.Size = new System.Drawing.Size(980, 48);
      this.panelAnswerFormat.TabIndex = 6;
      // 
      // panelQuestionsTop
      // 
      this.panelQuestionsTop.Controls.Add(this.labelQuestionsTop);
      this.panelQuestionsTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelQuestionsTop.GradientColorOne = System.Drawing.Color.AliceBlue;
      this.panelQuestionsTop.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.panelQuestionsTop.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelQuestionsTop.Location = new System.Drawing.Point(0, 0);
      this.panelQuestionsTop.Name = "panelQuestionsTop";
      this.panelQuestionsTop.Size = new System.Drawing.Size(980, 40);
      this.panelQuestionsTop.TabIndex = 10;
      // 
      // labelQuestionsTop
      // 
      this.labelQuestionsTop.AutoSize = true;
      this.labelQuestionsTop.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
      this.labelQuestionsTop.ForeColor = System.Drawing.Color.Blue;
      this.labelQuestionsTop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelQuestionsTop.Location = new System.Drawing.Point(8, 9);
      this.labelQuestionsTop.Name = "labelQuestionsTop";
      this.labelQuestionsTop.Size = new System.Drawing.Size(675, 23);
      this.labelQuestionsTop.TabIndex = 1;
      this.labelQuestionsTop.Text = "This screen guides you through the creation of the questions of the poll.";
      // 
      // tabPageResponses
      // 
      this.tabPageResponses.BackColor = System.Drawing.Color.Navy;
      this.tabPageResponses.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.tabPageResponses.Controls.Add(this.labelNoResponses);
      this.tabPageResponses.Controls.Add(this.panelAnswers);
      this.tabPageResponses.Controls.Add(this.panelResp_ActualQuestion);
      this.tabPageResponses.Controls.Add(this.panelRespondentInfo);
      this.tabPageResponses.Controls.Add(this.panelResp_Responses);
      this.tabPageResponses.Controls.Add(this.panelResp_Questions);
      this.tabPageResponses.Controls.Add(this.panelResponsesFilter);
      this.tabPageResponses.Controls.Add(this.panelResponsesTop);
      this.tabPageResponses.Location = new System.Drawing.Point(4, 54);
      this.tabPageResponses.Name = "tabPageResponses";
      this.tabPageResponses.Size = new System.Drawing.Size(984, 588);
      this.tabPageResponses.TabIndex = 3;
      this.tabPageResponses.Text = "Responses";
      this.tabPageResponses.ToolTipText = "Lets you review the responses obtained by the pollsters.";
      this.tabPageResponses.Resize += new System.EventHandler(this.tabPageResponses_Resize);
      // 
      // labelNoResponses
      // 
      this.labelNoResponses.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelNoResponses.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
      this.labelNoResponses.Font = new System.Drawing.Font("Arial", 28F);
      this.labelNoResponses.ForeColor = System.Drawing.Color.AliceBlue;
      this.labelNoResponses.GradientColorOne = System.Drawing.Color.Navy;
      this.labelNoResponses.GradientColorTwo = System.Drawing.Color.LightBlue;
      this.labelNoResponses.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.labelNoResponses.Location = new System.Drawing.Point(256, 96);
      this.labelNoResponses.Name = "labelNoResponses";
      this.labelNoResponses.Size = new System.Drawing.Size(496, 248);
      this.labelNoResponses.TabIndex = 18;
      this.labelNoResponses.Text = "No responses have yet been obtained for this poll.";
      this.labelNoResponses.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.labelNoResponses.Visible = false;
      // 
      // panelAnswers
      // 
      this.panelAnswers.AutoScroll = true;
      this.panelAnswers.Controls.Add(this.labelResp_AnswerTime);
      this.panelAnswers.Controls.Add(this.labelResp_AnswersMsg);
      this.panelAnswers.Controls.Add(this.panelResp_ExtraInfo);
      this.panelAnswers.Controls.Add(this.panelResp_Spinner);
      this.panelAnswers.Controls.Add(this.textBoxResp_Answer);
      this.panelAnswers.Controls.Add(this.listViewResp_Answers);
      this.panelAnswers.Controls.Add(this.labelResp_AnswerDate);
      this.panelAnswers.Controls.Add(this.labelResp_AnswerDateIntro);
      this.panelAnswers.Controls.Add(this.labelResp_AnswerTimeIntro);
      this.panelAnswers.Controls.Add(this.labelResp_AnswerFormat);
      this.panelAnswers.Controls.Add(this.labelResp_AnswerFormatIntro);
      this.panelAnswers.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelAnswers.GradientColorOne = System.Drawing.Color.Honeydew;
      this.panelAnswers.GradientColorTwo = System.Drawing.Color.SeaGreen;
      this.panelAnswers.Location = new System.Drawing.Point(100, 384);
      this.panelAnswers.Name = "panelAnswers";
      this.panelAnswers.Size = new System.Drawing.Size(780, 200);
      this.panelAnswers.TabIndex = 17;
      this.panelAnswers.Resize += new System.EventHandler(this.panelAnswers_Resize);
      // 
      // labelResp_AnswerTime
      // 
      this.labelResp_AnswerTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_AnswerTime.AutoSize = true;
      this.labelResp_AnswerTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_AnswerTime.Location = new System.Drawing.Point(698, 32);
      this.labelResp_AnswerTime.Name = "labelResp_AnswerTime";
      this.labelResp_AnswerTime.Size = new System.Drawing.Size(79, 18);
      this.labelResp_AnswerTime.TabIndex = 8;
      this.labelResp_AnswerTime.Text = "AnswerTime";
      // 
      // labelResp_AnswersMsg
      // 
      this.labelResp_AnswersMsg.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelResp_AnswersMsg.AutoSize = true;
      this.labelResp_AnswersMsg.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(128)));
      this.labelResp_AnswersMsg.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_AnswersMsg.ForeColor = System.Drawing.Color.Blue;
      this.labelResp_AnswersMsg.Location = new System.Drawing.Point(536, 128);
      this.labelResp_AnswersMsg.Name = "labelResp_AnswersMsg";
      this.labelResp_AnswersMsg.Size = new System.Drawing.Size(201, 22);
      this.labelResp_AnswersMsg.TabIndex = 7;
      this.labelResp_AnswersMsg.Text = "No response was obtained";
      this.labelResp_AnswersMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // panelResp_ExtraInfo
      // 
      this.panelResp_ExtraInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.panelResp_ExtraInfo.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(234)), ((System.Byte)(255)), ((System.Byte)(234)));
      this.panelResp_ExtraInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelResp_ExtraInfo.Controls.Add(this.panelExtraInfoHeader);
      this.panelResp_ExtraInfo.Controls.Add(this.textBoxResp_ExtraInfo);
      this.panelResp_ExtraInfo.Controls.Add(this.labelResp_ExtraInfoIntro);
      this.panelResp_ExtraInfo.Location = new System.Drawing.Point(112, 72);
      this.panelResp_ExtraInfo.Name = "panelResp_ExtraInfo";
      this.panelResp_ExtraInfo.Size = new System.Drawing.Size(190, 114);
      this.panelResp_ExtraInfo.TabIndex = 6;
      this.panelResp_ExtraInfo.Visible = false;
      // 
      // panelExtraInfoHeader
      // 
      this.panelExtraInfoHeader.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(213)), ((System.Byte)(255)), ((System.Byte)(213)));
      this.panelExtraInfoHeader.Controls.Add(this.labelResp_ExtraInfoTitle);
      this.panelExtraInfoHeader.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelExtraInfoHeader.ForeColor = System.Drawing.SystemColors.ControlText;
      this.panelExtraInfoHeader.Location = new System.Drawing.Point(0, 0);
      this.panelExtraInfoHeader.Name = "panelExtraInfoHeader";
      this.panelExtraInfoHeader.Size = new System.Drawing.Size(186, 30);
      this.panelExtraInfoHeader.TabIndex = 3;
      // 
      // labelResp_ExtraInfoTitle
      // 
      this.labelResp_ExtraInfoTitle.AutoSize = true;
      this.labelResp_ExtraInfoTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_ExtraInfoTitle.ForeColor = System.Drawing.SystemColors.ControlText;
      this.labelResp_ExtraInfoTitle.Location = new System.Drawing.Point(5, 6);
      this.labelResp_ExtraInfoTitle.Name = "labelResp_ExtraInfoTitle";
      this.labelResp_ExtraInfoTitle.Size = new System.Drawing.Size(125, 18);
      this.labelResp_ExtraInfoTitle.TabIndex = 1;
      this.labelResp_ExtraInfoTitle.Text = "Extra Info Obtained";
      // 
      // textBoxResp_ExtraInfo
      // 
      this.textBoxResp_ExtraInfo.BackColor = System.Drawing.Color.Honeydew;
      this.textBoxResp_ExtraInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.textBoxResp_ExtraInfo.Location = new System.Drawing.Point(12, 64);
      this.textBoxResp_ExtraInfo.Multiline = true;
      this.textBoxResp_ExtraInfo.Name = "textBoxResp_ExtraInfo";
      this.textBoxResp_ExtraInfo.ReadOnly = true;
      this.textBoxResp_ExtraInfo.Size = new System.Drawing.Size(170, 43);
      this.textBoxResp_ExtraInfo.TabIndex = 2;
      this.textBoxResp_ExtraInfo.Text = "textBoxExtraInfo";
      this.textBoxResp_ExtraInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBoxReadOnly_MouseDown);
      this.textBoxResp_ExtraInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxReadOnly_KeyPress);
      // 
      // labelResp_ExtraInfoIntro
      // 
      this.labelResp_ExtraInfoIntro.AutoSize = true;
      this.labelResp_ExtraInfoIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_ExtraInfoIntro.Location = new System.Drawing.Point(8, 40);
      this.labelResp_ExtraInfoIntro.Name = "labelResp_ExtraInfoIntro";
      this.labelResp_ExtraInfoIntro.Size = new System.Drawing.Size(92, 18);
      this.labelResp_ExtraInfoIntro.TabIndex = 1;
      this.labelResp_ExtraInfoIntro.Text = "ExtraInfo Intro:";
      // 
      // panelResp_Spinner
      // 
      this.panelResp_Spinner.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.panelResp_Spinner.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(216)), ((System.Byte)(228)), ((System.Byte)(248)));
      this.panelResp_Spinner.Controls.Add(this.labelResp_MinMax);
      this.panelResp_Spinner.Controls.Add(this.labelResp_Answer);
      this.panelResp_Spinner.Location = new System.Drawing.Point(352, 80);
      this.panelResp_Spinner.Name = "panelResp_Spinner";
      this.panelResp_Spinner.Size = new System.Drawing.Size(170, 80);
      this.panelResp_Spinner.TabIndex = 5;
      // 
      // labelResp_MinMax
      // 
      this.labelResp_MinMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_MinMax.Location = new System.Drawing.Point(7, 8);
      this.labelResp_MinMax.Name = "labelResp_MinMax";
      this.labelResp_MinMax.Size = new System.Drawing.Size(156, 16);
      this.labelResp_MinMax.TabIndex = 0;
      this.labelResp_MinMax.Text = "Min: 1        Max: 10";
      this.labelResp_MinMax.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // labelResp_Answer
      // 
      this.labelResp_Answer.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Answer.ForeColor = System.Drawing.Color.Blue;
      this.labelResp_Answer.Location = new System.Drawing.Point(7, 40);
      this.labelResp_Answer.Name = "labelResp_Answer";
      this.labelResp_Answer.Size = new System.Drawing.Size(156, 28);
      this.labelResp_Answer.TabIndex = 1;
      this.labelResp_Answer.Text = "5";
      this.labelResp_Answer.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // textBoxResp_Answer
      // 
      this.textBoxResp_Answer.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.textBoxResp_Answer.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
      this.textBoxResp_Answer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.textBoxResp_Answer.Location = new System.Drawing.Point(528, 80);
      this.textBoxResp_Answer.Multiline = true;
      this.textBoxResp_Answer.Name = "textBoxResp_Answer";
      this.textBoxResp_Answer.ReadOnly = true;
      this.textBoxResp_Answer.Size = new System.Drawing.Size(208, 32);
      this.textBoxResp_Answer.TabIndex = 3;
      this.textBoxResp_Answer.Text = "";
      this.textBoxResp_Answer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBoxReadOnly_MouseDown);
      this.textBoxResp_Answer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxReadOnly_KeyPress);
      // 
      // listViewResp_Answers
      // 
      this.listViewResp_Answers.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.listViewResp_Answers.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(234)), ((System.Byte)(255)), ((System.Byte)(234)));
      this.listViewResp_Answers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.listViewResp_Answers.FullRowSelect = true;
      this.listViewResp_Answers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewResp_Answers.Location = new System.Drawing.Point(360, 16);
      this.listViewResp_Answers.MultiSelect = false;
      this.listViewResp_Answers.Name = "listViewResp_Answers";
      this.listViewResp_Answers.Size = new System.Drawing.Size(208, 56);
      this.listViewResp_Answers.TabIndex = 1;
      this.listViewResp_Answers.View = System.Windows.Forms.View.Details;
      this.listViewResp_Answers.SelectedIndexChanged += new System.EventHandler(this.listViewResp_Answers_SelectedIndexChanged);
      // 
      // labelResp_AnswerDate
      // 
      this.labelResp_AnswerDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_AnswerDate.AutoSize = true;
      this.labelResp_AnswerDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_AnswerDate.Location = new System.Drawing.Point(698, 8);
      this.labelResp_AnswerDate.Name = "labelResp_AnswerDate";
      this.labelResp_AnswerDate.Size = new System.Drawing.Size(78, 18);
      this.labelResp_AnswerDate.TabIndex = 8;
      this.labelResp_AnswerDate.Text = "AnswerDate";
      // 
      // labelResp_AnswerDateIntro
      // 
      this.labelResp_AnswerDateIntro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_AnswerDateIntro.AutoSize = true;
      this.labelResp_AnswerDateIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_AnswerDateIntro.Location = new System.Drawing.Point(657, 8);
      this.labelResp_AnswerDateIntro.Name = "labelResp_AnswerDateIntro";
      this.labelResp_AnswerDateIntro.Size = new System.Drawing.Size(37, 18);
      this.labelResp_AnswerDateIntro.TabIndex = 8;
      this.labelResp_AnswerDateIntro.Text = "Date:";
      // 
      // labelResp_AnswerTimeIntro
      // 
      this.labelResp_AnswerTimeIntro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_AnswerTimeIntro.AutoSize = true;
      this.labelResp_AnswerTimeIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_AnswerTimeIntro.Location = new System.Drawing.Point(657, 32);
      this.labelResp_AnswerTimeIntro.Name = "labelResp_AnswerTimeIntro";
      this.labelResp_AnswerTimeIntro.Size = new System.Drawing.Size(38, 18);
      this.labelResp_AnswerTimeIntro.TabIndex = 8;
      this.labelResp_AnswerTimeIntro.Text = "Time:";
      // 
      // labelResp_AnswerFormat
      // 
      this.labelResp_AnswerFormat.AutoSize = true;
      this.labelResp_AnswerFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_AnswerFormat.ForeColor = System.Drawing.Color.MediumBlue;
      this.labelResp_AnswerFormat.Location = new System.Drawing.Point(108, 8);
      this.labelResp_AnswerFormat.Name = "labelResp_AnswerFormat";
      this.labelResp_AnswerFormat.Size = new System.Drawing.Size(92, 18);
      this.labelResp_AnswerFormat.TabIndex = 8;
      this.labelResp_AnswerFormat.Text = "AnswerFormat";
      // 
      // labelResp_AnswerFormatIntro
      // 
      this.labelResp_AnswerFormatIntro.AutoSize = true;
      this.labelResp_AnswerFormatIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_AnswerFormatIntro.Location = new System.Drawing.Point(7, 8);
      this.labelResp_AnswerFormatIntro.Name = "labelResp_AnswerFormatIntro";
      this.labelResp_AnswerFormatIntro.Size = new System.Drawing.Size(99, 18);
      this.labelResp_AnswerFormatIntro.TabIndex = 8;
      this.labelResp_AnswerFormatIntro.Text = "Answer Format:";
      // 
      // panelResp_ActualQuestion
      // 
      this.panelResp_ActualQuestion.Controls.Add(this.textBoxResp_Question);
      this.panelResp_ActualQuestion.Controls.Add(this.labelPanelResp);
      this.panelResp_ActualQuestion.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelResp_ActualQuestion.GradientColorOne = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(233)), ((System.Byte)(233)));
      this.panelResp_ActualQuestion.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(168)), ((System.Byte)(16)), ((System.Byte)(0)));
      this.panelResp_ActualQuestion.Location = new System.Drawing.Point(100, 318);
      this.panelResp_ActualQuestion.Name = "panelResp_ActualQuestion";
      this.panelResp_ActualQuestion.Size = new System.Drawing.Size(780, 66);
      this.panelResp_ActualQuestion.TabIndex = 20;
      // 
      // textBoxResp_Question
      // 
      this.textBoxResp_Question.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxResp_Question.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(234)), ((System.Byte)(234)));
      this.textBoxResp_Question.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.textBoxResp_Question.ForeColor = System.Drawing.SystemColors.WindowText;
      this.textBoxResp_Question.Location = new System.Drawing.Point(80, 8);
      this.textBoxResp_Question.Multiline = true;
      this.textBoxResp_Question.Name = "textBoxResp_Question";
      this.textBoxResp_Question.ReadOnly = true;
      this.textBoxResp_Question.Size = new System.Drawing.Size(688, 48);
      this.textBoxResp_Question.TabIndex = 4;
      this.textBoxResp_Question.Tag = "";
      this.textBoxResp_Question.Text = "";
      // 
      // labelPanelResp
      // 
      this.labelPanelResp.AutoSize = true;
      this.labelPanelResp.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelPanelResp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelPanelResp.Location = new System.Drawing.Point(6, 6);
      this.labelPanelResp.Name = "labelPanelResp";
      this.labelPanelResp.Size = new System.Drawing.Size(63, 18);
      this.labelPanelResp.TabIndex = 0;
      this.labelPanelResp.Text = "Question";
      // 
      // panelRespondentInfo
      // 
      this.panelRespondentInfo.Controls.Add(this.panelResp_riRight);
      this.panelRespondentInfo.Controls.Add(this.panelResp_riLeft);
      this.panelRespondentInfo.Controls.Add(this.panelResp_riMiddle);
      this.panelRespondentInfo.Controls.Add(this.buttonRIdisplay);
      this.panelRespondentInfo.Controls.Add(this.labelPanelRespondentInfo);
      this.panelRespondentInfo.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelRespondentInfo.GradientColorOne = System.Drawing.Color.Lavender;
      this.panelRespondentInfo.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(75)), ((System.Byte)(119)), ((System.Byte)(173)));
      this.panelRespondentInfo.Location = new System.Drawing.Point(100, 118);
      this.panelRespondentInfo.Name = "panelRespondentInfo";
      this.panelRespondentInfo.Size = new System.Drawing.Size(780, 200);
      this.panelRespondentInfo.TabIndex = 16;
      this.panelRespondentInfo.Resize += new System.EventHandler(this.panelRespondentInfo_Resize);
      // 
      // panelResp_riRight
      // 
      this.panelResp_riRight.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.panelResp_riRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelResp_riRight.Controls.Add(this.labelResp_Pollster);
      this.panelResp_riRight.Controls.Add(this.labelResp_PollsterIntro);
      this.panelResp_riRight.Controls.Add(this.labelResp_GPSIntro);
      this.panelResp_riRight.Controls.Add(this.labelResp_GPS);
      this.panelResp_riRight.Location = new System.Drawing.Point(522, 32);
      this.panelResp_riRight.Name = "panelResp_riRight";
      this.panelResp_riRight.Size = new System.Drawing.Size(192, 152);
      this.panelResp_riRight.TabIndex = 7;
      // 
      // labelResp_Pollster
      // 
      this.labelResp_Pollster.AutoSize = true;
      this.labelResp_Pollster.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Pollster.Location = new System.Drawing.Point(68, 8);
      this.labelResp_Pollster.Name = "labelResp_Pollster";
      this.labelResp_Pollster.Size = new System.Drawing.Size(85, 18);
      this.labelResp_Pollster.TabIndex = 6;
      this.labelResp_Pollster.Text = "PollsterName";
      // 
      // labelResp_PollsterIntro
      // 
      this.labelResp_PollsterIntro.AutoSize = true;
      this.labelResp_PollsterIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_PollsterIntro.Location = new System.Drawing.Point(8, 8);
      this.labelResp_PollsterIntro.Name = "labelResp_PollsterIntro";
      this.labelResp_PollsterIntro.Size = new System.Drawing.Size(53, 18);
      this.labelResp_PollsterIntro.TabIndex = 5;
      this.labelResp_PollsterIntro.Text = "Pollster:";
      // 
      // labelResp_GPSIntro
      // 
      this.labelResp_GPSIntro.AutoSize = true;
      this.labelResp_GPSIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_GPSIntro.Location = new System.Drawing.Point(8, 38);
      this.labelResp_GPSIntro.Name = "labelResp_GPSIntro";
      this.labelResp_GPSIntro.Size = new System.Drawing.Size(37, 18);
      this.labelResp_GPSIntro.TabIndex = 5;
      this.labelResp_GPSIntro.Text = "GPS:";
      // 
      // labelResp_GPS
      // 
      this.labelResp_GPS.AutoSize = true;
      this.labelResp_GPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_GPS.Location = new System.Drawing.Point(68, 38);
      this.labelResp_GPS.Name = "labelResp_GPS";
      this.labelResp_GPS.Size = new System.Drawing.Size(63, 18);
      this.labelResp_GPS.TabIndex = 6;
      this.labelResp_GPS.Text = "GPS_Info";
      // 
      // panelResp_riLeft
      // 
      this.panelResp_riLeft.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.panelResp_riLeft.Controls.Add(this.labelResp_NameIntro);
      this.panelResp_riLeft.Controls.Add(this.labelResp_Name);
      this.panelResp_riLeft.Controls.Add(this.labelResp_DateIntro);
      this.panelResp_riLeft.Controls.Add(this.labelResp_Date);
      this.panelResp_riLeft.Controls.Add(this.labelResp_TimeIntro);
      this.panelResp_riLeft.Controls.Add(this.labelResp_Time);
      this.panelResp_riLeft.Controls.Add(this.labelResp_SexIntro);
      this.panelResp_riLeft.Controls.Add(this.labelResp_Sex);
      this.panelResp_riLeft.Controls.Add(this.labelResp_Age);
      this.panelResp_riLeft.Controls.Add(this.labelResp_AgeIntro);
      this.panelResp_riLeft.Controls.Add(this.labelResp_EmailIntro);
      this.panelResp_riLeft.Controls.Add(this.labelResp_Email);
      this.panelResp_riLeft.Location = new System.Drawing.Point(40, 32);
      this.panelResp_riLeft.Name = "panelResp_riLeft";
      this.panelResp_riLeft.Size = new System.Drawing.Size(208, 152);
      this.panelResp_riLeft.TabIndex = 6;
      // 
      // labelResp_NameIntro
      // 
      this.labelResp_NameIntro.AutoSize = true;
      this.labelResp_NameIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_NameIntro.Location = new System.Drawing.Point(8, 56);
      this.labelResp_NameIntro.Name = "labelResp_NameIntro";
      this.labelResp_NameIntro.Size = new System.Drawing.Size(44, 18);
      this.labelResp_NameIntro.TabIndex = 8;
      this.labelResp_NameIntro.Text = "Name:";
      // 
      // labelResp_Name
      // 
      this.labelResp_Name.AutoSize = true;
      this.labelResp_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Name.Location = new System.Drawing.Point(63, 56);
      this.labelResp_Name.Name = "labelResp_Name";
      this.labelResp_Name.Size = new System.Drawing.Size(108, 18);
      this.labelResp_Name.TabIndex = 9;
      this.labelResp_Name.Text = "labelResp_Name";
      // 
      // labelResp_DateIntro
      // 
      this.labelResp_DateIntro.AutoSize = true;
      this.labelResp_DateIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_DateIntro.Location = new System.Drawing.Point(8, 8);
      this.labelResp_DateIntro.Name = "labelResp_DateIntro";
      this.labelResp_DateIntro.Size = new System.Drawing.Size(37, 18);
      this.labelResp_DateIntro.TabIndex = 10;
      this.labelResp_DateIntro.Text = "Date:";
      // 
      // labelResp_Date
      // 
      this.labelResp_Date.AutoSize = true;
      this.labelResp_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Date.Location = new System.Drawing.Point(63, 8);
      this.labelResp_Date.Name = "labelResp_Date";
      this.labelResp_Date.TabIndex = 5;
      this.labelResp_Date.Text = "labelResp_Date";
      // 
      // labelResp_TimeIntro
      // 
      this.labelResp_TimeIntro.AutoSize = true;
      this.labelResp_TimeIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_TimeIntro.Location = new System.Drawing.Point(8, 32);
      this.labelResp_TimeIntro.Name = "labelResp_TimeIntro";
      this.labelResp_TimeIntro.Size = new System.Drawing.Size(38, 18);
      this.labelResp_TimeIntro.TabIndex = 6;
      this.labelResp_TimeIntro.Text = "Time:";
      // 
      // labelResp_Time
      // 
      this.labelResp_Time.AutoSize = true;
      this.labelResp_Time.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Time.Location = new System.Drawing.Point(63, 32);
      this.labelResp_Time.Name = "labelResp_Time";
      this.labelResp_Time.Size = new System.Drawing.Size(102, 18);
      this.labelResp_Time.TabIndex = 7;
      this.labelResp_Time.Text = "labelResp_Time";
      // 
      // labelResp_SexIntro
      // 
      this.labelResp_SexIntro.AutoSize = true;
      this.labelResp_SexIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_SexIntro.Location = new System.Drawing.Point(8, 128);
      this.labelResp_SexIntro.Name = "labelResp_SexIntro";
      this.labelResp_SexIntro.Size = new System.Drawing.Size(32, 18);
      this.labelResp_SexIntro.TabIndex = 8;
      this.labelResp_SexIntro.Text = "Sex:";
      // 
      // labelResp_Sex
      // 
      this.labelResp_Sex.AutoSize = true;
      this.labelResp_Sex.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Sex.Location = new System.Drawing.Point(64, 128);
      this.labelResp_Sex.Name = "labelResp_Sex";
      this.labelResp_Sex.Size = new System.Drawing.Size(53, 18);
      this.labelResp_Sex.TabIndex = 9;
      this.labelResp_Sex.Text = "Fe/male";
      // 
      // labelResp_Age
      // 
      this.labelResp_Age.AutoSize = true;
      this.labelResp_Age.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Age.Location = new System.Drawing.Point(63, 104);
      this.labelResp_Age.Name = "labelResp_Age";
      this.labelResp_Age.Size = new System.Drawing.Size(20, 18);
      this.labelResp_Age.TabIndex = 9;
      this.labelResp_Age.Text = "##";
      // 
      // labelResp_AgeIntro
      // 
      this.labelResp_AgeIntro.AutoSize = true;
      this.labelResp_AgeIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_AgeIntro.Location = new System.Drawing.Point(8, 104);
      this.labelResp_AgeIntro.Name = "labelResp_AgeIntro";
      this.labelResp_AgeIntro.Size = new System.Drawing.Size(32, 18);
      this.labelResp_AgeIntro.TabIndex = 8;
      this.labelResp_AgeIntro.Text = "Age:";
      // 
      // labelResp_EmailIntro
      // 
      this.labelResp_EmailIntro.AutoSize = true;
      this.labelResp_EmailIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_EmailIntro.Location = new System.Drawing.Point(8, 80);
      this.labelResp_EmailIntro.Name = "labelResp_EmailIntro";
      this.labelResp_EmailIntro.Size = new System.Drawing.Size(47, 18);
      this.labelResp_EmailIntro.TabIndex = 8;
      this.labelResp_EmailIntro.Text = "E-mail:";
      // 
      // labelResp_Email
      // 
      this.labelResp_Email.AutoSize = true;
      this.labelResp_Email.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Email.Location = new System.Drawing.Point(64, 80);
      this.labelResp_Email.Name = "labelResp_Email";
      this.labelResp_Email.Size = new System.Drawing.Size(106, 18);
      this.labelResp_Email.TabIndex = 9;
      this.labelResp_Email.Text = "labelResp_Email";
      // 
      // panelResp_riMiddle
      // 
      this.panelResp_riMiddle.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.panelResp_riMiddle.Controls.Add(this.labelResp_Address);
      this.panelResp_riMiddle.Controls.Add(this.labelResp_AddressIntro);
      this.panelResp_riMiddle.Controls.Add(this.labelResp_CityIntro);
      this.panelResp_riMiddle.Controls.Add(this.labelResp_City);
      this.panelResp_riMiddle.Controls.Add(this.labelResp_StateProvIntro);
      this.panelResp_riMiddle.Controls.Add(this.labelResp_StateProv);
      this.panelResp_riMiddle.Controls.Add(this.labelResp_PostalCode);
      this.panelResp_riMiddle.Controls.Add(this.labelResp_PostalCodeIntro);
      this.panelResp_riMiddle.Controls.Add(this.labelResp_Tel1Intro);
      this.panelResp_riMiddle.Controls.Add(this.labelResp_Tel1);
      this.panelResp_riMiddle.Location = new System.Drawing.Point(264, 32);
      this.panelResp_riMiddle.Name = "panelResp_riMiddle";
      this.panelResp_riMiddle.Size = new System.Drawing.Size(248, 152);
      this.panelResp_riMiddle.TabIndex = 5;
      // 
      // labelResp_Address
      // 
      this.labelResp_Address.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_Address.AutoSize = true;
      this.labelResp_Address.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Address.Location = new System.Drawing.Point(88, 8);
      this.labelResp_Address.Name = "labelResp_Address";
      this.labelResp_Address.Size = new System.Drawing.Size(121, 18);
      this.labelResp_Address.TabIndex = 6;
      this.labelResp_Address.Text = "labelResp_Address";
      // 
      // labelResp_AddressIntro
      // 
      this.labelResp_AddressIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_AddressIntro.AutoSize = true;
      this.labelResp_AddressIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_AddressIntro.Location = new System.Drawing.Point(8, 8);
      this.labelResp_AddressIntro.Name = "labelResp_AddressIntro";
      this.labelResp_AddressIntro.Size = new System.Drawing.Size(58, 18);
      this.labelResp_AddressIntro.TabIndex = 5;
      this.labelResp_AddressIntro.Text = "Address:";
      // 
      // labelResp_CityIntro
      // 
      this.labelResp_CityIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_CityIntro.AutoSize = true;
      this.labelResp_CityIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_CityIntro.Location = new System.Drawing.Point(8, 38);
      this.labelResp_CityIntro.Name = "labelResp_CityIntro";
      this.labelResp_CityIntro.Size = new System.Drawing.Size(32, 18);
      this.labelResp_CityIntro.TabIndex = 5;
      this.labelResp_CityIntro.Text = "City:";
      // 
      // labelResp_City
      // 
      this.labelResp_City.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_City.AutoSize = true;
      this.labelResp_City.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_City.Location = new System.Drawing.Point(88, 38);
      this.labelResp_City.Name = "labelResp_City";
      this.labelResp_City.Size = new System.Drawing.Size(95, 18);
      this.labelResp_City.TabIndex = 6;
      this.labelResp_City.Text = "labelResp_City";
      // 
      // labelResp_StateProvIntro
      // 
      this.labelResp_StateProvIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_StateProvIntro.AutoSize = true;
      this.labelResp_StateProvIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_StateProvIntro.Location = new System.Drawing.Point(8, 68);
      this.labelResp_StateProvIntro.Name = "labelResp_StateProvIntro";
      this.labelResp_StateProvIntro.Size = new System.Drawing.Size(71, 18);
      this.labelResp_StateProvIntro.TabIndex = 5;
      this.labelResp_StateProvIntro.Text = "State/Prov:";
      // 
      // labelResp_StateProv
      // 
      this.labelResp_StateProv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_StateProv.AutoSize = true;
      this.labelResp_StateProv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_StateProv.Location = new System.Drawing.Point(88, 68);
      this.labelResp_StateProv.Name = "labelResp_StateProv";
      this.labelResp_StateProv.Size = new System.Drawing.Size(67, 18);
      this.labelResp_StateProv.TabIndex = 6;
      this.labelResp_StateProv.Text = "State/Prov";
      // 
      // labelResp_PostalCode
      // 
      this.labelResp_PostalCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_PostalCode.AutoSize = true;
      this.labelResp_PostalCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_PostalCode.Location = new System.Drawing.Point(88, 98);
      this.labelResp_PostalCode.Name = "labelResp_PostalCode";
      this.labelResp_PostalCode.Size = new System.Drawing.Size(65, 18);
      this.labelResp_PostalCode.TabIndex = 6;
      this.labelResp_PostalCode.Text = "Postal/Zip";
      // 
      // labelResp_PostalCodeIntro
      // 
      this.labelResp_PostalCodeIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_PostalCodeIntro.AutoSize = true;
      this.labelResp_PostalCodeIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_PostalCodeIntro.Location = new System.Drawing.Point(8, 98);
      this.labelResp_PostalCodeIntro.Name = "labelResp_PostalCodeIntro";
      this.labelResp_PostalCodeIntro.Size = new System.Drawing.Size(68, 18);
      this.labelResp_PostalCodeIntro.TabIndex = 5;
      this.labelResp_PostalCodeIntro.Text = "Postal/Zip:";
      // 
      // labelResp_Tel1Intro
      // 
      this.labelResp_Tel1Intro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_Tel1Intro.AutoSize = true;
      this.labelResp_Tel1Intro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Tel1Intro.Location = new System.Drawing.Point(8, 128);
      this.labelResp_Tel1Intro.Name = "labelResp_Tel1Intro";
      this.labelResp_Tel1Intro.Size = new System.Drawing.Size(27, 18);
      this.labelResp_Tel1Intro.TabIndex = 5;
      this.labelResp_Tel1Intro.Text = "Tel:";
      // 
      // labelResp_Tel1
      // 
      this.labelResp_Tel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.labelResp_Tel1.AutoSize = true;
      this.labelResp_Tel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResp_Tel1.Location = new System.Drawing.Point(88, 128);
      this.labelResp_Tel1.Name = "labelResp_Tel1";
      this.labelResp_Tel1.Size = new System.Drawing.Size(98, 18);
      this.labelResp_Tel1.TabIndex = 6;
      this.labelResp_Tel1.Text = "labelResp_Tel1";
      // 
      // buttonRIdisplay
      // 
      this.buttonRIdisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonRIdisplay.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(234)), ((System.Byte)(234)), ((System.Byte)(234)));
      this.buttonRIdisplay.Font = new System.Drawing.Font("Symbol", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(2)));
      this.buttonRIdisplay.Location = new System.Drawing.Point(750, 4);
      this.buttonRIdisplay.Name = "buttonRIdisplay";
      this.buttonRIdisplay.Size = new System.Drawing.Size(26, 22);
      this.buttonRIdisplay.TabIndex = 3;
      this.buttonRIdisplay.Click += new System.EventHandler(this.buttonRIdisplay_Click);
      // 
      // labelPanelRespondentInfo
      // 
      this.labelPanelRespondentInfo.AutoSize = true;
      this.labelPanelRespondentInfo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelPanelRespondentInfo.ForeColor = System.Drawing.Color.Black;
      this.labelPanelRespondentInfo.Location = new System.Drawing.Point(8, 8);
      this.labelPanelRespondentInfo.Name = "labelPanelRespondentInfo";
      this.labelPanelRespondentInfo.Size = new System.Drawing.Size(110, 18);
      this.labelPanelRespondentInfo.TabIndex = 2;
      this.labelPanelRespondentInfo.Text = "Respondent Info";
      // 
      // panelResp_Responses
      // 
      this.panelResp_Responses.AllowDrop = true;
      this.panelResp_Responses.AutoScroll = true;
      this.panelResp_Responses.Controls.Add(this.labelResponsesHeader);
      this.panelResp_Responses.Dock = System.Windows.Forms.DockStyle.Left;
      this.panelResp_Responses.GradientColorOne = System.Drawing.Color.WhiteSmoke;
      this.panelResp_Responses.GradientColorTwo = System.Drawing.Color.LightSeaGreen;
      this.panelResp_Responses.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelResp_Responses.Location = new System.Drawing.Point(0, 118);
      this.panelResp_Responses.Name = "panelResp_Responses";
      this.panelResp_Responses.Size = new System.Drawing.Size(100, 466);
      this.panelResp_Responses.TabIndex = 14;
      this.panelResp_Responses.Resize += new System.EventHandler(this.panelResp_Responses_Resize);
      // 
      // labelResponsesHeader
      // 
      this.labelResponsesHeader.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelResponsesHeader.ForeColor = System.Drawing.Color.Red;
      this.labelResponsesHeader.Location = new System.Drawing.Point(10, 8);
      this.labelResponsesHeader.Name = "labelResponsesHeader";
      this.labelResponsesHeader.Size = new System.Drawing.Size(80, 26);
      this.labelResponsesHeader.TabIndex = 1;
      this.labelResponsesHeader.Text = "Responses";
      this.labelResponsesHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // panelResp_Questions
      // 
      this.panelResp_Questions.AllowDrop = true;
      this.panelResp_Questions.AutoScroll = true;
      this.panelResp_Questions.Controls.Add(this.labelQuestionsHeader);
      this.panelResp_Questions.Dock = System.Windows.Forms.DockStyle.Right;
      this.panelResp_Questions.GradientColorOne = System.Drawing.Color.WhiteSmoke;
      this.panelResp_Questions.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
      this.panelResp_Questions.Location = new System.Drawing.Point(880, 118);
      this.panelResp_Questions.Name = "panelResp_Questions";
      this.panelResp_Questions.Size = new System.Drawing.Size(100, 466);
      this.panelResp_Questions.TabIndex = 13;
      this.panelResp_Questions.Resize += new System.EventHandler(this.panelResp_Questions_Resize);
      // 
      // labelQuestionsHeader
      // 
      this.labelQuestionsHeader.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelQuestionsHeader.ForeColor = System.Drawing.Color.Red;
      this.labelQuestionsHeader.Location = new System.Drawing.Point(10, 8);
      this.labelQuestionsHeader.Name = "labelQuestionsHeader";
      this.labelQuestionsHeader.Size = new System.Drawing.Size(80, 28);
      this.labelQuestionsHeader.TabIndex = 0;
      this.labelQuestionsHeader.Text = "Questions";
      this.labelQuestionsHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // panelResponsesFilter
      // 
      this.panelResponsesFilter.Controls.Add(this.labelFilterHeader);
      this.panelResponsesFilter.Controls.Add(this.listBoxResponsesDate);
      this.panelResponsesFilter.Controls.Add(this.listBoxResponsesSequence);
      this.panelResponsesFilter.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelResponsesFilter.GradientColorOne = System.Drawing.Color.LightSteelBlue;
      this.panelResponsesFilter.GradientColorTwo = System.Drawing.Color.Teal;
      this.panelResponsesFilter.Location = new System.Drawing.Point(0, 40);
      this.panelResponsesFilter.Name = "panelResponsesFilter";
      this.panelResponsesFilter.Size = new System.Drawing.Size(980, 78);
      this.panelResponsesFilter.TabIndex = 15;
      this.panelResponsesFilter.Resize += new System.EventHandler(this.panelResponsesFilter_Resize);
      // 
      // labelFilterHeader
      // 
      this.labelFilterHeader.AutoSize = true;
      this.labelFilterHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelFilterHeader.ForeColor = System.Drawing.Color.Yellow;
      this.labelFilterHeader.Location = new System.Drawing.Point(8, 4);
      this.labelFilterHeader.Name = "labelFilterHeader";
      this.labelFilterHeader.Size = new System.Drawing.Size(378, 18);
      this.labelFilterHeader.TabIndex = 20;
      this.labelFilterHeader.Text = "You can use the following criteria to filter the responses shown:";
      // 
      // listBoxResponsesDate
      // 
      this.listBoxResponsesDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxResponsesDate.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(100)), ((System.Byte)(116)), ((System.Byte)(132)));
      this.listBoxResponsesDate.CheckOnClick = true;
      this.listBoxResponsesDate.Font = new System.Drawing.Font("Arial", 10F);
      this.listBoxResponsesDate.ForeColor = System.Drawing.Color.White;
      this.listBoxResponsesDate.HorizontalScrollbar = true;
      this.listBoxResponsesDate.Location = new System.Drawing.Point(8, 27);
      this.listBoxResponsesDate.MultiColumn = true;
      this.listBoxResponsesDate.Name = "listBoxResponsesDate";
      this.listBoxResponsesDate.Size = new System.Drawing.Size(512, 40);
      this.listBoxResponsesDate.TabIndex = 19;
      this.listBoxResponsesDate.ThreeDCheckBoxes = true;
      this.listBoxResponsesDate.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listBoxResponsesDate_ItemCheck);
      // 
      // listBoxResponsesSequence
      // 
      this.listBoxResponsesSequence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxResponsesSequence.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(142)), ((System.Byte)(156)), ((System.Byte)(170)));
      this.listBoxResponsesSequence.CheckOnClick = true;
      this.listBoxResponsesSequence.Font = new System.Drawing.Font("Arial", 10F);
      this.listBoxResponsesSequence.ForeColor = System.Drawing.Color.White;
      this.listBoxResponsesSequence.HorizontalScrollbar = true;
      this.listBoxResponsesSequence.Location = new System.Drawing.Point(459, 27);
      this.listBoxResponsesSequence.MultiColumn = true;
      this.listBoxResponsesSequence.Name = "listBoxResponsesSequence";
      this.listBoxResponsesSequence.Size = new System.Drawing.Size(512, 40);
      this.listBoxResponsesSequence.TabIndex = 18;
      this.listBoxResponsesSequence.ThreeDCheckBoxes = true;
      this.listBoxResponsesSequence.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listBoxResponsesSequence_ItemCheck);
      // 
      // panelResponsesTop
      // 
      this.panelResponsesTop.Controls.Add(this.labelResponsesTop);
      this.panelResponsesTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelResponsesTop.GradientColorOne = System.Drawing.Color.AliceBlue;
      this.panelResponsesTop.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.panelResponsesTop.Location = new System.Drawing.Point(0, 0);
      this.panelResponsesTop.Name = "panelResponsesTop";
      this.panelResponsesTop.Size = new System.Drawing.Size(980, 40);
      this.panelResponsesTop.TabIndex = 11;
      // 
      // labelResponsesTop
      // 
      this.labelResponsesTop.AutoSize = true;
      this.labelResponsesTop.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
      this.labelResponsesTop.ForeColor = System.Drawing.Color.Blue;
      this.labelResponsesTop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelResponsesTop.Location = new System.Drawing.Point(8, 9);
      this.labelResponsesTop.Name = "labelResponsesTop";
      this.labelResponsesTop.Size = new System.Drawing.Size(636, 23);
      this.labelResponsesTop.TabIndex = 1;
      this.labelResponsesTop.Text = "This screen lets you review the responses obtained by the pollsters.";
      // 
      // tabPageSettings
      // 
      this.tabPageSettings.BackColor = System.Drawing.Color.AliceBlue;
      this.tabPageSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.tabPageSettings.Controls.Add(this.panelSettingsBottom);
      this.tabPageSettings.Controls.Add(this.panelPollSummary);
      this.tabPageSettings.Controls.Add(this.panelSettingsTop);
      this.tabPageSettings.Font = new System.Drawing.Font("Arial", 12F);
      this.tabPageSettings.Location = new System.Drawing.Point(4, 54);
      this.tabPageSettings.Name = "tabPageSettings";
      this.tabPageSettings.Size = new System.Drawing.Size(984, 588);
      this.tabPageSettings.TabIndex = 1;
      this.tabPageSettings.Text = "Settings";
      this.tabPageSettings.ToolTipText = "Lets you configure the basic and administrative settings of the poll.";
      // 
      // panelSettingsBottom
      // 
      this.panelSettingsBottom.Controls.Add(this.panelCompact);
      this.panelSettingsBottom.Controls.Add(this.panelPassword);
      this.panelSettingsBottom.Controls.Add(this.panelPollsterPrivileges);
      this.panelSettingsBottom.Controls.Add(this.panelPurge);
      this.panelSettingsBottom.Controls.Add(this.panelGeneralSettings);
      this.panelSettingsBottom.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelSettingsBottom.GradientColorOne = System.Drawing.Color.RoyalBlue;
      this.panelSettingsBottom.GradientColorTwo = System.Drawing.Color.Navy;
      this.panelSettingsBottom.Location = new System.Drawing.Point(0, 128);
      this.panelSettingsBottom.Name = "panelSettingsBottom";
      this.panelSettingsBottom.Size = new System.Drawing.Size(980, 456);
      this.panelSettingsBottom.TabIndex = 11;
      // 
      // panelCompact
      // 
      this.panelCompact.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.panelCompact.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCompact.Controls.Add(this.buttonExportFilenameSelect);
      this.panelCompact.Controls.Add(this.labelExportInstructions);
      this.panelCompact.Controls.Add(this.textBoxExportFilename);
      this.panelCompact.Controls.Add(this.labelExportFilename);
      this.panelCompact.Controls.Add(this.checkBoxExportEnabled);
      this.panelCompact.GradientColorTwo = System.Drawing.Color.LightGreen;
      this.panelCompact.Location = new System.Drawing.Point(557, 8);
      this.panelCompact.Name = "panelCompact";
      this.panelCompact.Size = new System.Drawing.Size(414, 124);
      this.panelCompact.TabIndex = 13;
      // 
      // buttonExportFilenameSelect
      // 
      this.buttonExportFilenameSelect.BackColor = System.Drawing.Color.LightGray;
      this.buttonExportFilenameSelect.Enabled = false;
      this.buttonExportFilenameSelect.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.buttonExportFilenameSelect.Location = new System.Drawing.Point(372, 84);
      this.buttonExportFilenameSelect.Name = "buttonExportFilenameSelect";
      this.buttonExportFilenameSelect.Size = new System.Drawing.Size(28, 22);
      this.buttonExportFilenameSelect.TabIndex = 8;
      this.buttonExportFilenameSelect.Text = "...";
      this.buttonExportFilenameSelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.buttonExportFilenameSelect.Click += new System.EventHandler(this.buttonExportFilenameSelect_Click);
      // 
      // labelExportInstructions
      // 
      this.labelExportInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelExportInstructions.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelExportInstructions.Location = new System.Drawing.Point(6, 8);
      this.labelExportInstructions.Name = "labelExportInstructions";
      this.labelExportInstructions.Size = new System.Drawing.Size(394, 32);
      this.labelExportInstructions.TabIndex = 7;
      this.labelExportInstructions.Text = "The contents of this poll can be automatically exported to an MS Access Database:" +
        "";
      // 
      // textBoxExportFilename
      // 
      this.textBoxExportFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxExportFilename.Enabled = false;
      this.textBoxExportFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.textBoxExportFilename.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.textBoxExportFilename.Location = new System.Drawing.Point(10, 84);
      this.textBoxExportFilename.Name = "textBoxExportFilename";
      this.textBoxExportFilename.Size = new System.Drawing.Size(352, 22);
      this.textBoxExportFilename.TabIndex = 6;
      this.textBoxExportFilename.Tag = "Property=CreationInfo.ExportFilename";
      this.textBoxExportFilename.Text = "";
      // 
      // labelExportFilename
      // 
      this.labelExportFilename.AutoSize = true;
      this.labelExportFilename.Enabled = false;
      this.labelExportFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelExportFilename.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelExportFilename.Location = new System.Drawing.Point(6, 61);
      this.labelExportFilename.Name = "labelExportFilename";
      this.labelExportFilename.Size = new System.Drawing.Size(106, 18);
      this.labelExportFilename.TabIndex = 7;
      this.labelExportFilename.Text = "Export Filename:";
      // 
      // checkBoxExportEnabled
      // 
      this.checkBoxExportEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.checkBoxExportEnabled.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.checkBoxExportEnabled.Location = new System.Drawing.Point(329, 41);
      this.checkBoxExportEnabled.Name = "checkBoxExportEnabled";
      this.checkBoxExportEnabled.Size = new System.Drawing.Size(80, 20);
      this.checkBoxExportEnabled.TabIndex = 7;
      this.checkBoxExportEnabled.Tag = "Property=CreationInfo.ExportEnabled";
      this.checkBoxExportEnabled.Text = "Enabled";
      this.checkBoxExportEnabled.CheckedChanged += new System.EventHandler(this.checkBoxExportEnabled_CheckedChanged);
      // 
      // panelPassword
      // 
      this.panelPassword.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelPassword.Controls.Add(this.labelPasswordInstructions);
      this.panelPassword.Controls.Add(this.textBoxOpenPassword);
      this.panelPassword.Controls.Add(this.labelOpenPassword);
      this.panelPassword.Controls.Add(this.labelModifyPassword);
      this.panelPassword.Controls.Add(this.textBoxModifyPassword);
      this.panelPassword.GradientColorTwo = System.Drawing.Color.DarkKhaki;
      this.panelPassword.Location = new System.Drawing.Point(296, 8);
      this.panelPassword.Name = "panelPassword";
      this.panelPassword.Size = new System.Drawing.Size(240, 152);
      this.panelPassword.TabIndex = 12;
      this.panelPassword.Visible = false;
      // 
      // labelPasswordInstructions
      // 
      this.labelPasswordInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelPasswordInstructions.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelPasswordInstructions.Location = new System.Drawing.Point(6, 8);
      this.labelPasswordInstructions.Name = "labelPasswordInstructions";
      this.labelPasswordInstructions.Size = new System.Drawing.Size(210, 50);
      this.labelPasswordInstructions.TabIndex = 7;
      this.labelPasswordInstructions.Text = "For extra security you can add passwords to restrict administrator access to this" +
        " poll:";
      // 
      // textBoxOpenPassword
      // 
      this.textBoxOpenPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxOpenPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.textBoxOpenPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.textBoxOpenPassword.Location = new System.Drawing.Point(139, 72);
      this.textBoxOpenPassword.Name = "textBoxOpenPassword";
      this.textBoxOpenPassword.PasswordChar = '*';
      this.textBoxOpenPassword.Size = new System.Drawing.Size(88, 22);
      this.textBoxOpenPassword.TabIndex = 6;
      this.textBoxOpenPassword.Text = "";
      // 
      // labelOpenPassword
      // 
      this.labelOpenPassword.AutoSize = true;
      this.labelOpenPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelOpenPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelOpenPassword.Location = new System.Drawing.Point(8, 74);
      this.labelOpenPassword.Name = "labelOpenPassword";
      this.labelOpenPassword.Size = new System.Drawing.Size(118, 18);
      this.labelOpenPassword.TabIndex = 7;
      this.labelOpenPassword.Text = "Password to Open:";
      // 
      // labelModifyPassword
      // 
      this.labelModifyPassword.AutoSize = true;
      this.labelModifyPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelModifyPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelModifyPassword.Location = new System.Drawing.Point(8, 114);
      this.labelModifyPassword.Name = "labelModifyPassword";
      this.labelModifyPassword.Size = new System.Drawing.Size(125, 18);
      this.labelModifyPassword.TabIndex = 7;
      this.labelModifyPassword.Text = "Password to Modify:";
      // 
      // textBoxModifyPassword
      // 
      this.textBoxModifyPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxModifyPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.textBoxModifyPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.textBoxModifyPassword.Location = new System.Drawing.Point(139, 112);
      this.textBoxModifyPassword.Name = "textBoxModifyPassword";
      this.textBoxModifyPassword.PasswordChar = '*';
      this.textBoxModifyPassword.Size = new System.Drawing.Size(88, 22);
      this.textBoxModifyPassword.TabIndex = 6;
      this.textBoxModifyPassword.Text = "";
      // 
      // panelPollsterPrivileges
      // 
      this.panelPollsterPrivileges.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelPollsterPrivileges.Controls.Add(this.labelPollsterPrivileges);
      this.panelPollsterPrivileges.Controls.Add(this.checkBoxReviewData);
      this.panelPollsterPrivileges.Controls.Add(this.checkBoxAbortRecord);
      this.panelPollsterPrivileges.Controls.Add(this.checkBox1);
      this.panelPollsterPrivileges.GradientColorTwo = System.Drawing.Color.DarkOrchid;
      this.panelPollsterPrivileges.Location = new System.Drawing.Point(8, 208);
      this.panelPollsterPrivileges.Name = "panelPollsterPrivileges";
      this.panelPollsterPrivileges.Size = new System.Drawing.Size(264, 124);
      this.panelPollsterPrivileges.TabIndex = 11;
      // 
      // labelPollsterPrivileges
      // 
      this.labelPollsterPrivileges.AutoSize = true;
      this.labelPollsterPrivileges.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelPollsterPrivileges.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelPollsterPrivileges.Location = new System.Drawing.Point(6, 8);
      this.labelPollsterPrivileges.Name = "labelPollsterPrivileges";
      this.labelPollsterPrivileges.Size = new System.Drawing.Size(115, 18);
      this.labelPollsterPrivileges.TabIndex = 6;
      this.labelPollsterPrivileges.Text = "Pollster Privileges:";
      // 
      // checkBoxReviewData
      // 
      this.checkBoxReviewData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.checkBoxReviewData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.checkBoxReviewData.Location = new System.Drawing.Point(16, 60);
      this.checkBoxReviewData.Name = "checkBoxReviewData";
      this.checkBoxReviewData.Size = new System.Drawing.Size(200, 20);
      this.checkBoxReviewData.TabIndex = 7;
      this.checkBoxReviewData.Tag = "Property=PollsterPrivileges.ReviewData";
      this.checkBoxReviewData.Text = "Review Data";
      // 
      // checkBoxAbortRecord
      // 
      this.checkBoxAbortRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.checkBoxAbortRecord.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.checkBoxAbortRecord.Location = new System.Drawing.Point(16, 88);
      this.checkBoxAbortRecord.Name = "checkBoxAbortRecord";
      this.checkBoxAbortRecord.Size = new System.Drawing.Size(200, 20);
      this.checkBoxAbortRecord.TabIndex = 7;
      this.checkBoxAbortRecord.Tag = "Property=PollsterPrivileges.CanAbortRecord";
      this.checkBoxAbortRecord.Text = "Can Abort Records";
      // 
      // checkBox1
      // 
      this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.checkBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.checkBox1.Location = new System.Drawing.Point(16, 32);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(200, 20);
      this.checkBox1.TabIndex = 7;
      this.checkBox1.Tag = "Property=PollsterPrivileges.ChangeUserName";
      this.checkBox1.Text = "Change UserName";
      // 
      // panelPurge
      // 
      this.panelPurge.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelPurge.Controls.Add(this.comboPurgeDuration);
      this.panelPurge.Controls.Add(this.labelPurge);
      this.panelPurge.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(75)), ((System.Byte)(119)), ((System.Byte)(173)));
      this.panelPurge.Location = new System.Drawing.Point(8, 104);
      this.panelPurge.Name = "panelPurge";
      this.panelPurge.Size = new System.Drawing.Size(264, 104);
      this.panelPurge.TabIndex = 9;
      // 
      // comboPurgeDuration
      // 
      this.comboPurgeDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.comboPurgeDuration.Location = new System.Drawing.Point(48, 62);
      this.comboPurgeDuration.MaxDropDownItems = 10;
      this.comboPurgeDuration.Name = "comboPurgeDuration";
      this.comboPurgeDuration.Size = new System.Drawing.Size(136, 24);
      this.comboPurgeDuration.TabIndex = 6;
      this.comboPurgeDuration.Tag = "";
      // 
      // labelPurge
      // 
      this.labelPurge.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.labelPurge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelPurge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelPurge.Location = new System.Drawing.Point(20, 5);
      this.labelPurge.Name = "labelPurge";
      this.labelPurge.Size = new System.Drawing.Size(216, 51);
      this.labelPurge.TabIndex = 5;
      this.labelPurge.Text = "How long after a pollster completes a poll should the data file on their mobile d" +
        "evice be removed?";
      // 
      // panelGeneralSettings
      // 
      this.panelGeneralSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelGeneralSettings.Controls.Add(this.labelGeneralSettings);
      this.panelGeneralSettings.Controls.Add(this.checkBoxPersonalInfo);
      this.panelGeneralSettings.Controls.Add(this.checkBoxHideQuestionNumbers);
      this.panelGeneralSettings.GradientColorTwo = System.Drawing.Color.IndianRed;
      this.panelGeneralSettings.Location = new System.Drawing.Point(8, 8);
      this.panelGeneralSettings.Name = "panelGeneralSettings";
      this.panelGeneralSettings.Size = new System.Drawing.Size(264, 96);
      this.panelGeneralSettings.TabIndex = 10;
      // 
      // labelGeneralSettings
      // 
      this.labelGeneralSettings.AutoSize = true;
      this.labelGeneralSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelGeneralSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelGeneralSettings.Location = new System.Drawing.Point(6, 8);
      this.labelGeneralSettings.Name = "labelGeneralSettings";
      this.labelGeneralSettings.Size = new System.Drawing.Size(108, 18);
      this.labelGeneralSettings.TabIndex = 6;
      this.labelGeneralSettings.Text = "General Settings:";
      // 
      // checkBoxPersonalInfo
      // 
      this.checkBoxPersonalInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.checkBoxPersonalInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.checkBoxPersonalInfo.Location = new System.Drawing.Point(13, 32);
      this.checkBoxPersonalInfo.Name = "checkBoxPersonalInfo";
      this.checkBoxPersonalInfo.Size = new System.Drawing.Size(238, 20);
      this.checkBoxPersonalInfo.TabIndex = 7;
      this.checkBoxPersonalInfo.Tag = "Property=CreationInfo.GetPersonalInfo";
      this.checkBoxPersonalInfo.Text = "Gather Respondents\' Personal Info";
      // 
      // checkBoxHideQuestionNumbers
      // 
      this.checkBoxHideQuestionNumbers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.checkBoxHideQuestionNumbers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.checkBoxHideQuestionNumbers.Location = new System.Drawing.Point(13, 60);
      this.checkBoxHideQuestionNumbers.Name = "checkBoxHideQuestionNumbers";
      this.checkBoxHideQuestionNumbers.Size = new System.Drawing.Size(238, 20);
      this.checkBoxHideQuestionNumbers.TabIndex = 7;
      this.checkBoxHideQuestionNumbers.Tag = "Property=CreationInfo.HideQuestionNumbers";
      this.checkBoxHideQuestionNumbers.Text = "Hide Question Numbers";
      // 
      // panelPollSummary
      // 
      this.panelPollSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelPollSummary.Controls.Add(this.textBoxPollSummary);
      this.panelPollSummary.Controls.Add(this.labelPollSummary);
      this.panelPollSummary.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelPollSummary.GradientColorTwo = System.Drawing.Color.SeaGreen;
      this.panelPollSummary.Location = new System.Drawing.Point(0, 40);
      this.panelPollSummary.Name = "panelPollSummary";
      this.panelPollSummary.Size = new System.Drawing.Size(980, 88);
      this.panelPollSummary.TabIndex = 10;
      // 
      // textBoxPollSummary
      // 
      this.textBoxPollSummary.AcceptsReturn = true;
      this.textBoxPollSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxPollSummary.AutoSize = false;
      this.textBoxPollSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.textBoxPollSummary.Location = new System.Drawing.Point(7, 28);
      this.textBoxPollSummary.Multiline = true;
      this.textBoxPollSummary.Name = "textBoxPollSummary";
      this.textBoxPollSummary.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxPollSummary.Size = new System.Drawing.Size(961, 48);
      this.textBoxPollSummary.TabIndex = 0;
      this.textBoxPollSummary.Tag = "Property=CreationInfo.PollSummary";
      this.textBoxPollSummary.Text = "";
      // 
      // labelPollSummary
      // 
      this.labelPollSummary.AutoSize = true;
      this.labelPollSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelPollSummary.ForeColor = System.Drawing.Color.Black;
      this.labelPollSummary.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelPollSummary.Location = new System.Drawing.Point(6, 5);
      this.labelPollSummary.Name = "labelPollSummary";
      this.labelPollSummary.Size = new System.Drawing.Size(214, 18);
      this.labelPollSummary.TabIndex = 1;
      this.labelPollSummary.Text = "Descriptive Summary For This Poll:";
      // 
      // panelSettingsTop
      // 
      this.panelSettingsTop.Controls.Add(this.labelSettingsTop);
      this.panelSettingsTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelSettingsTop.GradientColorOne = System.Drawing.Color.AliceBlue;
      this.panelSettingsTop.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.panelSettingsTop.Location = new System.Drawing.Point(0, 0);
      this.panelSettingsTop.Name = "panelSettingsTop";
      this.panelSettingsTop.Size = new System.Drawing.Size(980, 40);
      this.panelSettingsTop.TabIndex = 9;
      // 
      // labelSettingsTop
      // 
      this.labelSettingsTop.AutoSize = true;
      this.labelSettingsTop.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
      this.labelSettingsTop.ForeColor = System.Drawing.Color.Blue;
      this.labelSettingsTop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelSettingsTop.Location = new System.Drawing.Point(8, 9);
      this.labelSettingsTop.Name = "labelSettingsTop";
      this.labelSettingsTop.Size = new System.Drawing.Size(738, 23);
      this.labelSettingsTop.TabIndex = 1;
      this.labelSettingsTop.Text = "This screen lets you configure the basic and administrative settings of the poll." +
        "";
      // 
      // panelCEI
      // 
      this.panelCEI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCEI.Controls.Add(this.panelCEIright);
      this.panelCEI.Controls.Add(this.panelCEIleft);
      this.panelCEI.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panelCEI.GradientColorOne = System.Drawing.Color.RoyalBlue;
      this.panelCEI.GradientColorTwo = System.Drawing.Color.RoyalBlue;
      this.panelCEI.Location = new System.Drawing.Point(0, 646);
      this.panelCEI.Name = "panelCEI";
      this.panelCEI.Size = new System.Drawing.Size(992, 32);
      this.panelCEI.TabIndex = 11;
      this.panelCEI.Resize += new System.EventHandler(this.panelCEI_Resize);
      // 
      // panelCEIright
      // 
      this.panelCEIright.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.panelCEIright.Controls.Add(this.labelLastEdited);
      this.panelCEIright.Controls.Add(this.labelLastEditDate);
      this.panelCEIright.Controls.Add(this.labelLastEditedByIntro);
      this.panelCEIright.Controls.Add(this.labelLastEditedBy);
      this.panelCEIright.GradientColorOne = System.Drawing.Color.RoyalBlue;
      this.panelCEIright.GradientColorTwo = System.Drawing.Color.RoyalBlue;
      this.panelCEIright.Location = new System.Drawing.Point(564, 6);
      this.panelCEIright.Name = "panelCEIright";
      this.panelCEIright.Size = new System.Drawing.Size(420, 20);
      this.panelCEIright.TabIndex = 6;
      // 
      // labelLastEdited
      // 
      this.labelLastEdited.AutoSize = true;
      this.labelLastEdited.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelLastEdited.ForeColor = System.Drawing.Color.White;
      this.labelLastEdited.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelLastEdited.Location = new System.Drawing.Point(265, 0);
      this.labelLastEdited.Name = "labelLastEdited";
      this.labelLastEdited.Size = new System.Drawing.Size(75, 18);
      this.labelLastEdited.TabIndex = 7;
      this.labelLastEdited.Text = "Last Edited:";
      this.labelLastEdited.SizeChanged += new System.EventHandler(this.panelCEI_Resize);
      // 
      // labelLastEditDate
      // 
      this.labelLastEditDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
      this.labelLastEditDate.AutoSize = true;
      this.labelLastEditDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelLastEditDate.ForeColor = System.Drawing.Color.White;
      this.labelLastEditDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelLastEditDate.Location = new System.Drawing.Point(339, 0);
      this.labelLastEditDate.Name = "labelLastEditDate";
      this.labelLastEditDate.Size = new System.Drawing.Size(82, 18);
      this.labelLastEditDate.TabIndex = 6;
      this.labelLastEditDate.Tag = "Property=CreationInfo.LastEditDate";
      this.labelLastEditDate.Text = "LastEditDate";
      this.labelLastEditDate.SizeChanged += new System.EventHandler(this.panelCEI_Resize);
      // 
      // labelLastEditedByIntro
      // 
      this.labelLastEditedByIntro.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelLastEditedByIntro.AutoSize = true;
      this.labelLastEditedByIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelLastEditedByIntro.ForeColor = System.Drawing.Color.White;
      this.labelLastEditedByIntro.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelLastEditedByIntro.Location = new System.Drawing.Point(3, 0);
      this.labelLastEditedByIntro.Name = "labelLastEditedByIntro";
      this.labelLastEditedByIntro.Size = new System.Drawing.Size(95, 18);
      this.labelLastEditedByIntro.TabIndex = 5;
      this.labelLastEditedByIntro.Text = "Last Edited By:";
      this.labelLastEditedByIntro.SizeChanged += new System.EventHandler(this.panelCEI_Resize);
      // 
      // labelLastEditedBy
      // 
      this.labelLastEditedBy.AutoSize = true;
      this.labelLastEditedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelLastEditedBy.ForeColor = System.Drawing.Color.White;
      this.labelLastEditedBy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelLastEditedBy.Location = new System.Drawing.Point(103, 0);
      this.labelLastEditedBy.Name = "labelLastEditedBy";
      this.labelLastEditedBy.Size = new System.Drawing.Size(69, 18);
      this.labelLastEditedBy.TabIndex = 4;
      this.labelLastEditedBy.Tag = "Property=CreationInfo.LastEditedBy";
      this.labelLastEditedBy.Text = "UserName";
      this.labelLastEditedBy.SizeChanged += new System.EventHandler(this.panelCEI_Resize);
      // 
      // panelCEIleft
      // 
      this.panelCEIleft.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.panelCEIleft.Controls.Add(this.labelCreatedOn);
      this.panelCEIleft.Controls.Add(this.labelCreationDate);
      this.panelCEIleft.Controls.Add(this.labelCreatedBy);
      this.panelCEIleft.Controls.Add(this.labelCreatorName);
      this.panelCEIleft.Controls.Add(this.labelRevisionIntro);
      this.panelCEIleft.Controls.Add(this.labelRevisionNumber);
      this.panelCEIleft.GradientColorOne = System.Drawing.Color.RoyalBlue;
      this.panelCEIleft.GradientColorTwo = System.Drawing.Color.RoyalBlue;
      this.panelCEIleft.Location = new System.Drawing.Point(1, 6);
      this.panelCEIleft.Name = "panelCEIleft";
      this.panelCEIleft.Size = new System.Drawing.Size(487, 20);
      this.panelCEIleft.TabIndex = 5;
      // 
      // labelCreatedOn
      // 
      this.labelCreatedOn.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelCreatedOn.AutoSize = true;
      this.labelCreatedOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelCreatedOn.ForeColor = System.Drawing.Color.White;
      this.labelCreatedOn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelCreatedOn.Location = new System.Drawing.Point(184, 0);
      this.labelCreatedOn.Name = "labelCreatedOn";
      this.labelCreatedOn.Size = new System.Drawing.Size(91, 18);
      this.labelCreatedOn.TabIndex = 4;
      this.labelCreatedOn.Text = "Creation Date:";
      this.labelCreatedOn.SizeChanged += new System.EventHandler(this.panelCEI_Resize);
      // 
      // labelCreationDate
      // 
      this.labelCreationDate.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelCreationDate.AutoSize = true;
      this.labelCreationDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelCreationDate.ForeColor = System.Drawing.Color.White;
      this.labelCreationDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelCreationDate.Location = new System.Drawing.Point(280, 0);
      this.labelCreationDate.Name = "labelCreationDate";
      this.labelCreationDate.Size = new System.Drawing.Size(84, 18);
      this.labelCreationDate.TabIndex = 3;
      this.labelCreationDate.Tag = "Property=CreationInfo.CreationDate, Default=Unknown";
      this.labelCreationDate.Text = "CreationDate";
      this.labelCreationDate.SizeChanged += new System.EventHandler(this.panelCEI_Resize);
      // 
      // labelCreatedBy
      // 
      this.labelCreatedBy.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelCreatedBy.AutoSize = true;
      this.labelCreatedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelCreatedBy.ForeColor = System.Drawing.Color.White;
      this.labelCreatedBy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelCreatedBy.Location = new System.Drawing.Point(3, 0);
      this.labelCreatedBy.Name = "labelCreatedBy";
      this.labelCreatedBy.Size = new System.Drawing.Size(75, 18);
      this.labelCreatedBy.TabIndex = 2;
      this.labelCreatedBy.Text = "Created By:";
      this.labelCreatedBy.SizeChanged += new System.EventHandler(this.panelCEI_Resize);
      // 
      // labelCreatorName
      // 
      this.labelCreatorName.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.labelCreatorName.AutoSize = true;
      this.labelCreatorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelCreatorName.ForeColor = System.Drawing.Color.White;
      this.labelCreatorName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelCreatorName.Location = new System.Drawing.Point(83, 0);
      this.labelCreatorName.Name = "labelCreatorName";
      this.labelCreatorName.Size = new System.Drawing.Size(85, 18);
      this.labelCreatorName.TabIndex = 1;
      this.labelCreatorName.Tag = "Property=CreationInfo.CreatorName, Default=Unknown";
      this.labelCreatorName.Text = "CreatorName";
      this.labelCreatorName.SizeChanged += new System.EventHandler(this.panelCEI_Resize);
      // 
      // labelRevisionIntro
      // 
      this.labelRevisionIntro.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelRevisionIntro.AutoSize = true;
      this.labelRevisionIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelRevisionIntro.ForeColor = System.Drawing.Color.White;
      this.labelRevisionIntro.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelRevisionIntro.Location = new System.Drawing.Point(406, 0);
      this.labelRevisionIntro.Name = "labelRevisionIntro";
      this.labelRevisionIntro.Size = new System.Drawing.Size(60, 18);
      this.labelRevisionIntro.TabIndex = 4;
      this.labelRevisionIntro.Text = "Revision:";
      // 
      // labelRevisionNumber
      // 
      this.labelRevisionNumber.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.labelRevisionNumber.AutoSize = true;
      this.labelRevisionNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.labelRevisionNumber.ForeColor = System.Drawing.Color.White;
      this.labelRevisionNumber.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelRevisionNumber.Location = new System.Drawing.Point(470, 0);
      this.labelRevisionNumber.Name = "labelRevisionNumber";
      this.labelRevisionNumber.Size = new System.Drawing.Size(12, 18);
      this.labelRevisionNumber.TabIndex = 4;
      this.labelRevisionNumber.Tag = "Property=CreationInfo.Revision, Default=0";
      this.labelRevisionNumber.Text = "1";
      // 
      // DragDropTimer
      // 
      this.DragDropTimer.Interval = 1;
      this.DragDropTimer.Tick += new System.EventHandler(this.DragDropTimer_Tick);
      // 
      // timerAnimateRIpanel
      // 
      this.timerAnimateRIpanel.Interval = 20;
      this.timerAnimateRIpanel.Tick += new System.EventHandler(this.timerAnimateRIpanel_Tick);
      // 
      // timerShowCharts
      // 
      this.timerShowCharts.Interval = 500;
      this.timerShowCharts.Tick += new System.EventHandler(this.timerShowCharts_Tick);
      // 
      // frmPoll
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(255)), ((System.Byte)(255)));
      this.ClientSize = new System.Drawing.Size(992, 678);
      this.Controls.Add(this.tabNewPoll);
      this.Controls.Add(this.panelCEI);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(840, 580);
      this.Name = "frmPoll";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "New Poll";
      this.Load += new System.EventHandler(this.frmPoll_Load);
      this.tabNewPoll.ResumeLayout(false);
      this.tabPageSummary.ResumeLayout(false);
      this.panelSummaryLower.ResumeLayout(false);
      this.panelMidUpper.ResumeLayout(false);
      this.panelSummaryUpper.ResumeLayout(false);
      this.panelPollCreationInfo.ResumeLayout(false);
      this.paneSummaryTop.ResumeLayout(false);
      this.tabPageInstructions.ResumeLayout(false);
      this.panelBeginMessage.ResumeLayout(false);
      this.panelBeforePoll.ResumeLayout(false);
      this.panelAfterPoll.ResumeLayout(false);
      this.panelEndMessage.ResumeLayout(false);
      this.panelInstructionsTop.ResumeLayout(false);
      this.panelAfterAllPolls.ResumeLayout(false);
      this.tabPageQuestions.ResumeLayout(false);
      this.panelChoicesTitle.ResumeLayout(false);
      this.panelPreviewFrame.ResumeLayout(false);
      this.panelPreviewTitle.ResumeLayout(false);
      this.panelQuestion.ResumeLayout(false);
      this.panelQuestionList.ResumeLayout(false);
      this.panelQuestionList1.ResumeLayout(false);
      this.panelQuestionList2.ResumeLayout(false);
      this.panelQuestionsTop.ResumeLayout(false);
      this.tabPageResponses.ResumeLayout(false);
      this.panelAnswers.ResumeLayout(false);
      this.panelResp_ExtraInfo.ResumeLayout(false);
      this.panelExtraInfoHeader.ResumeLayout(false);
      this.panelResp_Spinner.ResumeLayout(false);
      this.panelResp_ActualQuestion.ResumeLayout(false);
      this.panelRespondentInfo.ResumeLayout(false);
      this.panelResp_riRight.ResumeLayout(false);
      this.panelResp_riLeft.ResumeLayout(false);
      this.panelResp_riMiddle.ResumeLayout(false);
      this.panelResp_Responses.ResumeLayout(false);
      this.panelResp_Questions.ResumeLayout(false);
      this.panelResponsesFilter.ResumeLayout(false);
      this.panelResponsesTop.ResumeLayout(false);
      this.tabPageSettings.ResumeLayout(false);
      this.panelSettingsBottom.ResumeLayout(false);
      this.panelCompact.ResumeLayout(false);
      this.panelPassword.ResumeLayout(false);
      this.panelPollsterPrivileges.ResumeLayout(false);
      this.panelPurge.ResumeLayout(false);
      this.panelGeneralSettings.ResumeLayout(false);
      this.panelPollSummary.ResumeLayout(false);
      this.panelSettingsTop.ResumeLayout(false);
      this.panelCEI.ResumeLayout(false);
      this.panelCEIright.ResumeLayout(false);
      this.panelCEIleft.ResumeLayout(false);
      this.ResumeLayout(false);

    }
    #endregion



    #region AllTabPages

    #region TabPagesCommon
    /// <remarks>
    ///
    /// All Tab Pages
    ///
    /// </remarks>

    // For some unknown reason, a tabPage's "Click", "GotFocus", and "Enter" events 
    // don't fire when their page gains focus so we have to handle the event this way.
    private void tabNewPoll_Click(object sender, System.EventArgs e)
    {
      try
      {
        TabControl tab = tabNewPoll;

        switch (tab.SelectedTab.Name)
        { 
          case "tabPageSummary":
            //          textBoxSummaryPollSummary.Focus();
            //          textBoxSummaryPollSummary.SelectedText = "";
            break;

          case "tabPageQuestions":
            textQuestion.Focus();
            break;

          case "tabPageResponses":
            if (responsesPageInfo != null)
            {
              if (responsesPageInfo.Respondents.Count > 0)
              {
                if (responsesPageInfo.CurrRespondent == -1)
                  responsesPageInfo.CurrRespondent = 0;

                if (responsesPageInfo.CurrQuestion == -1)
                  responsesPageInfo.CurrQuestion = 0;
              }
            }
            break;

          default:
            // No special handling of other pages right now so just ignore this event for them
            break;
        }
      }

      catch (Exception ex)
      {
        Debug.WriteLine("Unhandled exception: " + ex.Message, "tabNewPoll_Click");
      }
    }


    /// <summary>
    /// Adjusts the position of the bottommost panel and all its children whenever the form is resized. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void panelCEI_Resize(object sender, System.EventArgs e)
    {
      int _fullwidth = this.panelCEI.Width;
      int _gap = 30;   // The horizontal gap between the two sets of label pairs
      Panel _leftpanel = this.panelCEIleft;
      Panel _rightpanel = this.panelCEIright;

      _leftpanel.Width = (int) (_fullwidth * 0.5);
      _rightpanel.Width = _leftpanel.Width;
      _rightpanel.Left = _fullwidth - _rightpanel.Width;

      labelCreatorName.Left = labelCreatedBy.Right + 1;
      labelCreatedOn.Left = labelCreatorName.Right + _gap;
      labelCreationDate.Left = labelCreatedOn.Right + 1;
      labelRevisionIntro.Left = labelCreationDate.Right + _gap;
      labelRevisionNumber.Left = labelRevisionIntro.Right + 1;

      labelLastEditDate.Left = panelCEIright.Width - _gap / 3 - labelLastEditDate.Width;
      labelLastEdited.Left = labelLastEditDate.Left - 1 - labelLastEdited.Width;
      labelLastEditedBy.Left = labelLastEdited.Left - _gap - labelLastEditedBy.Width;
      labelLastEditedByIntro.Left = labelLastEditedBy.Left - 1 - labelLastEditedByIntro.Width;
    }


    /// <summary>
    /// This method ensures three things:
    ///  1. That the top tab buttons are sufficiently wide to hold their descriptions (where possible).
    ///  2. That frmPoll cannot be resized so small that it covers up the tab buttons.
    ///  3. Ditto re frmMain.
    /// </summary>
    private void Readjust_frmPoll()
    {
      int _desiredwidth;
      int _margin = 50;
      int _maxwidth = 0;
      int _minwidth;
      int _width;
      
      // Need to create 'g' to gain access to the MeasureString function
      Graphics g = tabNewPoll.CreateGraphics();  

      foreach(TabPage tab in tabNewPoll.Controls)
      {
        _width = (int) (g.MeasureString(tab.Text, tab.Font)).Width;
        if (_width > _maxwidth)
          _maxwidth = _width;
      }

      g.Dispose();  // Need to get rid of the graphics object we created
      _desiredwidth = _maxwidth + tabNewPoll.Padding.X;

      // This extra check only added after Viewer Mode (just 2 tabs) was introduced
      if (tabNewPoll.TabPages.Count < 4 && _desiredwidth < 170)
        _desiredwidth = 170;

      if (_desiredwidth != tabNewPoll.ItemSize.Width)
      {
        if (_desiredwidth * tabNewPoll.TabPages.Count > tabNewPoll.Width)
          _desiredwidth = (int) (tabNewPoll.Width - _margin) / tabNewPoll.TabPages.Count;

        if (_desiredwidth != tabNewPoll.ItemSize.Width)
          tabNewPoll.ItemSize = new Size(_desiredwidth, tabNewPoll.ItemSize.Height);
      }

      // Also need to ensure that the form won't be shrunk so small that any of the tabs might get hidden
      _minwidth = tabNewPoll.ItemSize.Width * tabNewPoll.TabPages.Count + _margin;
      if (_minwidth > this.MinimumSize.Width)
      {
        this.MinimumSize = new Size(_minwidth, this.MinimumSize.Height);
        this.MdiParent.MinimumSize = new Size(_minwidth, this.MinimumSize.Height + 20);
      }

      //To Do: Still need to add code to prevent vertical resizing too small!
    }


    /// <summary>
    /// There are several conditions under which we must update the Summary and Responses pages:
    ///   - When a poll is first opened
    ///   - When new responses are downloaded that update a poll
    ///   - When questions are added or removed on the Q&A page
    /// </summary>
    public void UpdateCollectedData(Poll model)
    {
      // Responses page
      // Note: This first construct is purely for initialization purposes.  It's only run once per session.
      if (responsesPageInfo == null)
      {
        responsesPageInfo = new FilteredTabPageInfo();

        responsesPageInfo.tabPage = tabPageResponses;
        responsesPageInfo.respButPanel = panelResp_Responses;
        responsesPageInfo.questButPanel = panelResp_Questions;

        responsesPageInfo.Model = model;
        responsesPageInfo.Respondents = model.Respondents;    //Debug: Need to make sure that filtering doesn't affect original set of Respondents!
        
        responsesPageInfo.QuestionChangedEvent += new EventHandler(PopulateResponseData);
        responsesPageInfo.QuestionChangedEvent += new EventHandler(PressResp_QuestionButton);
        responsesPageInfo.RespondentChangedEvent += new EventHandler(PopulateResponseData);
        responsesPageInfo.RespondentChangedEvent += new EventHandler(PressResp_RespondentButton);
        responsesPageInfo.RespondentChangedEvent += new EventHandler(CurrRespondentChanged);
      }

      if (model.Respondents.Count > 0)
      {
        PrepareResponseFilters(responsesPageInfo, listBoxResponsesSequence, listBoxResponsesDate);
        
        // This bit of code resets some things that were done when No Responses were displayed
        panelResponsesFilter.Visible = true;
        responsesPageInfo.respButPanel.Visible = true;
        responsesPageInfo.questButPanel.Visible = true;
        panelRespondentInfo.Visible = responsesPageInfo.Model.CreationInfo.GetPersonalInfo;   // If true then show panel, otherwise hide
        panelResp_ActualQuestion.Visible = true;
        panelAnswers.Visible = true;
        labelNoResponses.Visible = false;
        
        PopulateResp_QuestionButtons(responsesPageInfo);
        PopulateResp_ResponseButtons(responsesPageInfo);
      }
      
      else   // No responses yet retrieved
      {
        // This poll doesn't yet have any responses so hide a bunch of things.
        // Note: A message will be displayed when the user clicks on the "Responses" tab.
        panelResponsesFilter.Visible = false;
        responsesPageInfo.respButPanel.Visible = false;
        responsesPageInfo.questButPanel.Visible = false;
        panelRespondentInfo.Visible = false;
        panelResp_ActualQuestion.Visible = false;
        panelAnswers.Visible = false;
        labelNoResponses.Visible = true;
      }

      // Summary page
      labelSummaryQuestionsTally.Text = Tools.PrepareCorrectTense(model.Questions.Count, "Question");
      labelSummaryResponsesTally.Text = Tools.PrepareCorrectTense(model.Respondents.Count, "Response");

      if (model.Respondents.Count > 0)
        DrawSummaryCharts(model);   // Draw the charts on the Summary page - 1 chart per question
    }



    /// <summary>
    /// Fills those combo & list boxes that display unchanging "reference" data.
    /// </summary>
    private void PopulateListControls()
    {
      // comboPurgeDuration - in Settings
      foreach (int val in Enum.GetValues(typeof(PurgeDuration)))
      {      
        string pdText = Enum.GetName(typeof(PurgeDuration), val);
        pdText = pdText.Replace("_", " ");
        comboPurgeDuration.Items.Add(pdText);
      }

      comboPurgeDuration.SelectedIndex = 0;    // Default, though may be overwritten later on during initialization

      // Note: If we didn't have to remove the underscores, we could have used this single line to do the population
      // comboPurgeDuration.DataSource = Enum.GetNames(typeof(PurgeDuration));
    }


    /// <summary>
    /// Updates the set of Question buttons that reside on the right side of the Q&A and Responses pages.
    /// Since questions are always sequential, it should suffice to just update them wholesale, as opposed to figuring out
    /// where to insert each one.  This is because a given button isn't really directly linked with a given Question.
    /// Rather, it just provides the index number of the Question object in the data model.
    /// Note: Internally, these buttons have a 0-based index, but externally appear as "1", "2", "3", etc.
    /// </summary>
    /// <param name="numButtons"></param>   // The number of buttons that should be present
    /// <param name="panel"></param>        // The panel in which to add/remove the button(s)
    /// <param name="tabPage"></param>      // The tab page on which the buttons are located
    private void UpdateQuestionButtons(int numButtons, Panel panel, TabPage tabPage)
    {
      int currNum = Tools.CountControls(panel, new QuestionButton());   // The number of buttons that currently exist
      int diff = numButtons - currNum;

      for (int i = 0; i < Math.Abs(diff); i++)
      {
        // See if we need to add or remove a button
        if (diff > 0)
        {
          int qIdx = currNum;
          AddQuestionButton(qIdx, panel, tabPage);
          currNum++;
        }
        else
        {
          int qIdx = currNum - 1;
          RemoveQuestionButton(qIdx, panel);
          currNum--;
        }
      }

      if (panel.Controls.Count > 0)
        panel.AutoScrollMinSize = new Size(panel.AutoScrollMinSize.Width, panel.Controls[panel.Controls.Count - 1].Bottom);
    }

    // This is called from the Controller and refers specifically to the question buttons on the Q&A page.
    public void UpdateQuestionButtons(Poll pollModel)
    {
      int numQuestions = pollModel.Questions.Count;
      UpdateQuestionButtons(numQuestions, panelQuestionList1, tabPageQuestions);
      UpdateCollectedData(pollModel);  // We must also update the # of Questions on the Summary and Responses pages
    }


    /// <summary>
    /// Add an additional Question button to specified panel.
    /// </summary>
    /// <param name="butIdx"></param>
    /// <param name="panel"></param>
    /// <param name="tabPage"></param>
    private void AddQuestionButton(int butIdx, Panel panel, TabPage tabPage)
    {
      int gap = 4;

      QuestionButton qBut = new QuestionButton();
      int butNum = butIdx + 1;
      qBut.Name = qBut.NamePrefix + butNum.ToString();   // ie. "radioQuestion1", "radioQuestion2", etc.
      qBut.Text = butNum.ToString();                     // ie. "1, "2", etc.
      qBut.QuestionNum = butIdx;

      if (Tools.CountControls(panel, new QuestionButton()) == 0)
        qBut.Top = Tools.LocateChildrenBottomY(panel) + gap;
      else
        qBut.Top = Tools.LocateChildTopY(panel, new QuestionButton()) + (butIdx * (qBut.Height + gap));

      qBut.Left = (panel.ClientSize.Width - qBut.Width) / 2;

      switch (tabPage.Name)
      {
        case "tabPageQuestions":
          // Wire in an event handler for when any of the buttons are pressed
          qBut.CheckedChanged += new EventHandler(QuestionButton_Event);

          // Wire in event handlers to handle drag & drop of the question button (so its order can be changed)
          qBut.MouseDown += new MouseEventHandler(GeneralControl_MouseDown);
          qBut.MouseUp += new MouseEventHandler(GeneralControl_MouseUp);
          break;

        case "tabPageResponses":
          // Wire in an event handler for when any of the buttons are pressed
          qBut.CheckedChanged += new EventHandler(Responses_QuestionButton_Event);
          break;

        default:
          Debug.WriteLine("Unknown TabPage encountered: " + tabPage.Name, "frmPoll.AddQuestionButton");
          break;
      }

      // Then add this new radio button to its parent panel
      panel.Controls.Add(qBut);

      if (tabPage.Name == "tabPageQuestions")
      {
        if ((qBut.Top + qBut.Height + gap) > panel.Height)
          Tools.CenterPanelControls(panel);

        if (panel.Controls.Count > 0)
          buttonRemoveQuestion.Enabled = true;
      }
    }


    /// <summary>
    /// Remove the specified question button from the panel - This is only done on the Q&A page.
    /// Note: The actual removal of the question from Poll is done in the Controller.
    ///       We thus have ability to just remove the last button, no matter
    ///       which actual Question is being removed.  The decremental loop in UpdateQuestionButtons
    ///       takes care of this for us - ie. last to first.
    /// </summary>
    /// <param name="butIdx"></param>
    /// <param name="panel"></param>
    private void RemoveQuestionButton(int butIdx, Panel panel)
    {
      if (butIdx < 0)  // Invalid button index?
        return;

      // These next set of lines make the assumption that all non-QuestionButtons will
      // be added before any QuestionButtons are.  This should be a safe assumption.
      int butCnt = Tools.CountControls(panel, new QuestionButton());
      int nonButtons = panel.Controls.Count - butCnt;
      QuestionButton qBut = panel.Controls[butIdx + nonButtons] as QuestionButton;

      // Remove this radio button from its parent panel
      panel.Controls.Remove(qBut);
      butCnt--;

      int gap = 4;
      if (butCnt > 0)
      {
        qBut = panel.Controls[butCnt + nonButtons - 1] as QuestionButton;

        if ((qBut.Bottom + gap) < panel.Height)
          Tools.CenterPanelControls(panel);
      }

      else if (butCnt == 0)
        buttonRemoveQuestion.Enabled = false;
    }


    /// <summary>
    /// Updates the set of Response buttons that reside on the left side of the Summary and Responses pages.
    /// Note: Internally, these buttons have a 0-based index, but externally appear as "1", "2", "3", etc.
    /// </summary>
    /// <param name="pageInfo"></param>   // The Page Info object for the Responses tab page
    private void UpdateResponseButtons(FilteredTabPageInfo pageInfo)
    {
      int numButtons = pageInfo.Respondents.Count;   // The number of buttons that should be present
      Panel panel = pageInfo.respButPanel;

      int currNum = Tools.CountControls(panel, new ResponseButton());   // The number of buttons that currently exist
      int diff = numButtons - currNum;

      for (int i = 0; i < Math.Abs(diff); i++)
      {
        // See if we need to add or remove a button
        if (diff > 0)
        {
          int rIdx = currNum;
          AddResponseButton(rIdx, panel, pageInfo.tabPage);
          currNum++;
        }
        else
        {
          int rIdx = currNum - 1;
          RemoveResponseButton(rIdx, panel, pageInfo.tabPage);
          currNum--;
        }
      }

      if (panel.Controls.Count > 0)
        panel.AutoScrollMinSize = new Size(panel.AutoScrollMinSize.Width, panel.Controls[panel.Controls.Count - 1].Bottom);
    }


    /// <summary>
    /// Add an additional Response button to the specified panel.
    /// </summary>
    /// <param name="butIdx"></param>
    /// <param name="panel"></param>
    /// <param name="tabPage"></param>
    private void AddResponseButton(int butIdx, Panel panel, TabPage tabPage)
    {
      int gap = 4;

      ResponseButton rBut = new ResponseButton();
      int butNum = butIdx + 1;
      rBut.Name = rBut.NamePrefix + butNum.ToString();   // ie. "radioResponse1", "radioResponse2", etc.
      rBut.Text = butNum.ToString();                     // ie. "1, "2", etc.
      rBut.ResponseNum = butIdx;

      if (Tools.CountControls(panel, new ResponseButton()) == 0)
        rBut.Top = Tools.LocateChildrenBottomY(panel) + gap;
      else
        rBut.Top = Tools.LocateChildTopY(panel, new ResponseButton()) + (butIdx * (rBut.Height + gap));

      rBut.Left = (panel.ClientSize.Width - rBut.Width) / 2;

      // Wire in an event handler for when the button is pressed
      switch (tabPage.Name)
      {
        case "tabPageSummary":
          // To Do
          break;

        case "tabPageResponses":
          rBut.CheckedChanged += new EventHandler(Responses_ResponseButton_Event);
          break;

        default:
          Debug.WriteLine("Unknown TabPage encountered: " + tabPage.Name, "frmPoll.AddResponseButton");
          break;
      }

      // Then add this new radio button to its parent panel
      panel.Controls.Add(rBut);
    }


    /// <summary>
    /// Remove the specified question button from the panel.
    /// </summary>
    /// <param name="butIdx"></param>
    /// <param name="panel"></param>
    /// <param name="tabPage"></param>
    private void RemoveResponseButton(int butIdx, Panel panel, TabPage tabPage)
    {
      if (butIdx < 0)  // Invalid button index?
        return;

      // These next set of lines make the assumption that all controls that are not ResponseButtons
      // will be added before any ResponseButtons are.  This should be a safe assumption.
      int butCnt = Tools.CountControls(panel, new ResponseButton());
      int nonButtons = panel.Controls.Count - butCnt;
      ResponseButton rBut = panel.Controls[butIdx + nonButtons] as ResponseButton;

      // Remove this radio button from its parent panel
      panel.Controls.Remove(rBut);
      butCnt--;

      int gap = 4;
      if (butCnt > 0)
      {
        rBut = panel.Controls[butCnt + nonButtons - 1] as ResponseButton;

        if ((rBut.Bottom + gap) < panel.Height)
          Tools.CenterPanelControls(panel);
      }
    }


    private void panelResp_Responses_Resize(object sender, System.EventArgs e)
    {
      Tools.CenterPanelControls(sender as Panel);
    }

    private void panelResp_Questions_Resize(object sender, System.EventArgs e)
    {
      Tools.CenterPanelControls(sender as Panel);
    }

    #endregion


    #region Summary_TabPage
    /// <remarks>
    /// Summary page
    /// </remarks>

    #endregion
    

    #region Responses_TabPage
    /// <remarks>
    /// 
    /// Responses page
    /// 
    /// </remarks>

    /// <summary>
    /// Initializes the two response filters - Sequence & Date - with selectable items appropriate to the full set of
    /// available Responses in the model.  If either filter is just going to display "ALL" then the filter will be hidden.  
    /// If both are just "ALL" then the entire filter panel will be hidden.
    /// </summary>
    /// <param name="pageInfo"></param>
    /// <param name="listBoxSequence"></param>
    /// <param name="listBoxDate"></param>
    /// <param name="skipDate"></param>        // If true then the Date portion of this method will be skipped
    private void PrepareResponseFilters(FilteredTabPageInfo pageInfo, CheckedListBox listBoxSequence, CheckedListBox listBoxDate, bool skipDate)
    {
      listBoxSequence.Items.Clear();
      pageInfo.SeqCriteria.Clear();

      listBoxSequence.Items.Add("ALL");
      pageInfo.SeqCriteria.Add(0);

      //int numResp = pageInfo.Model.Respondents.Count;   // Originally we were always drawing from the full, unfiltered set of Respondents
      int numResp = pageInfo.Respondents.Count;

      ArrayList sigValues = new ArrayList();  // Significant values
      sigValues.AddRange(new int[] {10, 25, 50, 100, 250, 500, 1000});
      int maxIdx = sigValues.Count - 1;

      for (int i = 0; i < sigValues.Count; i++)
      {
        int sigVal = (int) sigValues[i];
        if (numResp > sigVal)
        {
          listBoxSequence.Items.Add("First " + sigVal.ToString());
          pageInfo.SeqCriteria.Add(sigVal);
        }
        else
        {
          maxIdx = i - 1;
          break;
        }
      }

      for (int i = maxIdx; i >= 0; i--)
      {
        int sigVal = (int) sigValues[i];
        if (numResp > sigVal)
        {
          listBoxSequence.Items.Add("Last " + sigVal.ToString());
          pageInfo.SeqCriteria.Add(- sigVal);
        }
      }
      listBoxSequence.SetItemChecked(0, true);


      // Now handle the Date filter portion - This is ONLY called when the filters are first setup, not afterwards.
      if (skipDate)
        return;
      
      listBoxDate.Items.Clear();
      pageInfo.DateCriteria.Clear();

      listBoxDate.Items.Add("ALL");
      pageInfo.DateCriteria.Add(0);

      // Need to examine date range of collected responses
      _Respondent resp = pageInfo.Model.Respondents[0];   // Note that here we're looking at the full set of respondents
      DateTime date1 = resp.TimeCaptured;
      int maxCnt = pageInfo.Model.Respondents.Count - 1;
      resp = pageInfo.Model.Respondents[maxCnt];
      DateTime date2 = resp.TimeCaptured;

      if (date1.Date != date2.Date)
      {
        DateTime lastDate = date2.AddDays(1);   // Set an artificial sentinel
        for (int i = maxCnt; i > -1; i--)
        {
          DateTime currDate = pageInfo.Model.Respondents[i].TimeCaptured;
          if (currDate.Date != lastDate.Date)
          {
            TimeSpan duration = DateTime.Now.Date - currDate.Date;

            switch (duration.Days)
            {
              case 0:
                listBoxDate.Items.Add("Today");
                break;

              case 1:
                listBoxDate.Items.Add("Yesterday");
                break;

              default:
                listBoxDate.Items.Add(currDate.Date.ToShortDateString());
                break;
            }

            pageInfo.DateCriteria.Add(currDate.Date);
            lastDate = currDate;
          }
        }
      }

      listBoxDate.SetItemChecked(0, true);
    }


    private void PrepareResponseFilters(FilteredTabPageInfo pageInfo, CheckedListBox listBoxSequence, CheckedListBox listBoxDate)
    {
      PrepareResponseFilters(pageInfo, listBoxSequence, listBoxDate, false);
    }


    /// <summary>
    /// Populates panelResp_Questions with buttons labelled 1,2,3,...
    /// </summary>
    /// <param name="pageInfo"></param>
    private void PopulateResp_QuestionButtons(FilteredTabPageInfo pageInfo)
    {
      UpdateQuestionButtons(pageInfo.Model.Questions.Count, pageInfo.questButPanel, pageInfo.tabPage);
    }


    /// <summary>
    /// Populates panelResp_Responses with buttons labelled 1,2,3,...
    /// </summary>
    /// <param name="pageInfo"></param>
    private void PopulateResp_ResponseButtons(FilteredTabPageInfo pageInfo)
    {
      UpdateResponseButtons(pageInfo);
    }


    #region ResponsesPanelAnimation

    private void buttonRIdisplay_Click(object sender, System.EventArgs e)
    {
      RIpanelAnimation = new RIpanelAnimationInfo();
      RIpanelAnimation.MaxHeight = 200;
      RIpanelAnimation.Increment = (panelRespondentInfo.Height == RIpanelAnimation.MaxHeight) ? -10 : 10;

      timerAnimateRIpanel.Interval = 2000 / (RIpanelAnimation.MaxHeight / Math.Abs(RIpanelAnimation.Increment));
      timerAnimateRIpanel.Start();
    }


    private void timerAnimateRIpanel_Tick(object sender, System.EventArgs e)
    {
      panelRespondentInfo.Height += RIpanelAnimation.Increment;

      if (RIpanelAnimation.Increment < 0)
      {
        if (panelRespondentInfo.Height <= (buttonRIdisplay.Height + 2 * buttonRIdisplay.Top))
        {
          timerAnimateRIpanel.Stop();
          //buttonRIdisplay.Text =  "\xAF";
          buttonRIdisplay.ImageIndex = 1;
        }
      }
      else
      {
        if (panelRespondentInfo.Height >= RIpanelAnimation.MaxHeight)
        {
          timerAnimateRIpanel.Stop();
          //buttonRIdisplay.Text =  "\xAD";
          buttonRIdisplay.ImageIndex = 0;
        }
      }
    }

    #endregion


    #region Resizing

    private void tabPageResponses_Resize(object sender, System.EventArgs e)
    {
      if (labelNoResponses.Visible)   // If labelNoResponses is visible then ensure that it's centered.
      {
        TabPage tabPage = responsesPageInfo.tabPage;
        Label label = labelNoResponses;
        label.Location = new Point((tabPage.Width - label.Width) / 2, (tabPage.Height - label.Height) / 2);
      }
    }


    /// <summary>
    /// This event handler resizes the two filter panels located at the top of the form.
    /// Notes:
    ///   - If a panel contains just "ALL" then it will be hidden
    ///   - If both panels are hidden then so will be their parent
    /// </summary>
    private void panelResponsesFilter_Resize(object sender, System.EventArgs e)
    {
      if (listBoxResponsesSequence.Items.Count == 1 && listBoxResponsesDate.Items.Count == 1)
        panelResponsesFilter.Visible = false;
      
      else
      {
        int margin = listBoxResponsesDate.Left;

        if (listBoxResponsesDate.Items.Count == 1)
        {
          listBoxResponsesDate.Visible = false;
          listBoxResponsesSequence.Left = margin;
          listBoxResponsesSequence.Width = panelResponsesFilter.Width - margin * 2;
        }
        else if (listBoxResponsesSequence.Items.Count == 1)
        {
          listBoxResponsesSequence.Visible = false;
          listBoxResponsesDate.Width = panelResponsesFilter.Width - margin * 2;
        }
        else
        {
          listBoxResponsesDate.Visible = true;
          listBoxResponsesSequence.Visible = true;
          listBoxResponsesDate.Width = panelResponsesFilter.Width / 2 - margin;
          listBoxResponsesSequence.Left = listBoxResponsesDate.Right;
          listBoxResponsesSequence.Width = panelResponsesFilter.Width / 2 - margin;
        }
      }
    }


    /// <summary>
    /// Keeps the left, center, and right panels proportionally positioned.
    /// </summary>
    private void panelRespondentInfo_Resize(object sender, System.EventArgs e)
    {
      int gap = 4;

      Panel panel1 = panelResp_riLeft;       // 27%
      Panel panel2 = panelResp_riMiddle;     // Remainder
      Panel panel3 = panelResp_riRight;      // 15% (min 200 pixels)

      int xMin = gap * 2;
      int xMax = buttonRIdisplay.Left - gap * 2;
      int xWid = (xMax - xMin) - gap * 4 - gap * 2;    // ie. 3 panels => 2 gaps between them (we're making the left one wider)

      panel1.Left = xMin;
      panel1.Width = (int) (xWid * 0.27);
      panel3.Width = Math.Max((int) (xWid * 0.15), 200);
      panel3.Left = xMax - panel3.Width;

      panel2.Left = panel1.Right + gap * 4;
      panel2.Width = panel3.Left - gap * 2 - panel2.Left;
    }


    /// <summary>
    /// Determines what is the current AnswerFormat and then resizes the appropriate controls.
    /// </summary>
    private void panelAnswers_Resize(object sender, System.EventArgs e)
    {
      if (responsesPageInfo != null)
        if (responsesPageInfo.CurrQuestion != -1)
          PreparePanelAnswerControls(responsesPageInfo.Model.Questions[responsesPageInfo.CurrQuestion].AnswerFormat);
    }

    #endregion



    /// <summary>
    /// The code in this event handler does two things:
    ///   - Lets only one item be checked at a time
    ///   - Alters what data is shown in Responses
    /// </summary>
    private void listBoxResponsesSequence_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (listBoxResponsesSequenceChanging)
        return;

      listBoxResponsesSequenceChanging = true;   // Prevents recursive calls from occurring while this event handler is doing its work

      CheckedListBox listBox = sender as CheckedListBox;
      int idx = e.Index;

      // We must always have at least one checkbox checked.  Here we prevent
      // the user from trying to explicitly uncheck one of the checkboxes.
      if (e.CurrentValue == CheckState.Checked)
        e.NewValue = CheckState.Checked;
      else
      {
        for (int i = 0; i < listBox.Items.Count; i++)
        {
          if (i != idx)
            listBox.SetItemChecked(i, false);
        }

        // A new checkbox was checked, thus the filtering has changed, so update the displayed Responses.
        responsesPageInfo.SeqSelection = (int) responsesPageInfo.SeqCriteria[idx];

        if (responsesPageInfo.CurrQuestion != -1 && responsesPageInfo.CurrRespondent != -1)
        {
          FilterRespondents(responsesPageInfo);

          // Note: We'll never change the Date filter but will change the Sequence filter depending on how many
          //       responses are now available based on the date criteria set.
          
          // Important Note:
          // The contents of the Date filter remain unchanged for a given poll.  The contents of the Sequence
          // filter change depending on the number of respondents available as a result of the Date filtering.
          // BUT we will not change the contents of the Sequence filter based on changes made within IT.       
          //PrepareResponseFilters(responsesPageInfo, listBoxResponsesSequence, listBoxResponsesDate, true);
          PopulateResp_ResponseButtons(responsesPageInfo);

          responsesPageInfo.CurrRespondent = 0;
          responsesPageInfo.CurrQuestion = 0;
        }
      }

      listBox.ClearSelected();
      listBoxResponsesSequenceChanging = false;   // Permit future changes to occur
    }
    

    /// <summary>
    /// The code in this event handler does two things:
    ///   - Keeps the first item, "ALL", unique from all the others
    ///   - Alters what data is shown in Responses
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void listBoxResponsesDate_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      if (listBoxResponsesDateChanging)
        return;

      listBoxResponsesDateChanging = true;   // Prevents recursive calls from occurring while this event handler is doing its work

      CheckedListBox listBox = sender as CheckedListBox;
      int idx = e.Index;

      if (idx == 0)
      {
        // We must always have at least one checkbox checked.  Here we prevent the user
        // from trying to explicitly change the 'ALL' checkbox from checked to unchecked.
        if (e.CurrentValue == CheckState.Checked)
          e.NewValue = CheckState.Checked;
        
        for (int i = 1; i < listBox.Items.Count; i++)
        {
          listBox.SetItemChecked(i, false);
        }
      }

      else if (listBox.GetItemChecked(0))              // This is any item other than 'ALL'.  If 'ALL' was previously checked
        listBox.SetItemChecked(0, false);              // then uncheck it now

      else if (e.CurrentValue == CheckState.Checked)   // If the user is unchecking a checkbox other than 'ALL'
      {                                                // then we must check whether all the other non-ALL checkboxes
        // are also unchecked.  If so, then we must set 'ALL' to the checked state.
        bool noCheckBoxesSet = true;
        for (int i = 1; i < listBox.Items.Count; i++)
        {
          if (i != idx && listBox.GetItemChecked(i))
          {
            noCheckBoxesSet = false;
            break;
          }
        }
      
        if (noCheckBoxesSet)
          listBox.SetItemChecked(0, true);
      }

      listBox.ClearSelected();

      // Update the date filter criteria
      if (e.NewValue == CheckState.Checked)
      {
        if (idx == 0)
          responsesPageInfo.DateSelections.Clear();   // An empty collection signifies 'ALL'
        else
          responsesPageInfo.DateSelections.Add(responsesPageInfo.DateCriteria[idx]);
      }
      else  // Must be unchecked
      {
        int chkIdx = responsesPageInfo.DateSelections.IndexOf(responsesPageInfo.DateCriteria[idx]);
        responsesPageInfo.DateSelections.RemoveAt(chkIdx);
      }

      if (responsesPageInfo.CurrQuestion != -1 && responsesPageInfo.CurrRespondent != -1)
      {
        // When filtering by date, we'll always start with the full set of respondents from the model
        responsesPageInfo.Respondents = responsesPageInfo.Model.Respondents;

        // We'll also reset the Sequence criteria because it may no longer be relevant with the new date criteria
        responsesPageInfo.SeqSelection = 0;

        // Now change what is shown on the rest of the page
        FilterRespondents(responsesPageInfo);

        PrepareResponseFilters(responsesPageInfo, listBoxResponsesSequence, listBoxResponsesDate, true);
        PopulateResp_ResponseButtons(responsesPageInfo);

        responsesPageInfo.CurrRespondent = 0;
        responsesPageInfo.CurrQuestion = 0;
      }

      listBoxResponsesDateChanging = false;   // Permit future changes to occur
    }


    private void Responses_QuestionButton_Event(object sender, EventArgs e)
    {
      QuestionButton qBut = sender as QuestionButton;
      
      if (qBut.Checked)
      {
        qBut.SetBackColor("Down");
        responsesPageInfo.CurrQuestion = qBut.QuestionNum;
      }
      else
        qBut.SetBackColor("Up");
    }


    private void Responses_ResponseButton_Event(object sender, EventArgs e)
    {
      ResponseButton rBut = sender as ResponseButton;

      if (rBut.Checked)
      {
        rBut.SetBackColor("Down");
        responsesPageInfo.CurrRespondent = rBut.ResponseNum;
      }
      else
        rBut.SetBackColor("Up");
    }
   

    /// <summary>
    /// On the Responses page, populates the [horizontally] center portion depending
    /// on the current values of the Responses and Question buttons on the same page.
    /// </summary>
    private void PopulateResponseData(object sender, EventArgs e)
    {
      try
      {
        int itemHgt = 0;   // The item height of the ListView (to be populated later)

        ListView listView = listViewResp_Answers;
        Panel panelExtra = panelResp_ExtraInfo;
        TextBox textBox = textBoxResp_Answer;
        Panel panelSpin = panelResp_Spinner;
        Label labelMinMax = labelResp_MinMax;
        Label labelAnswer = labelResp_Answer;
        Label labelMsg = labelResp_AnswersMsg;     // Used to display an additional message to the user

        FilteredTabPageInfo pageInfo = sender as FilteredTabPageInfo;
        int currResp = pageInfo.CurrRespondent;
        int currQuest = pageInfo.CurrQuestion;

        if (currResp == -1 || currQuest == -1)  // Don't proceed if everything's not fully initialized
          return;

        Cursor.Current = Cursors.WaitCursor;

        _Question quest = pageInfo.Model.Questions[currQuest];     // Populate object variable with current question info
        _Respondent respondent = pageInfo.Respondents[currResp];


        // Upper panel: Respondent info
        labelResp_Date.Text = Tools.DisallowNullDate(respondent.TimeCaptured);
        labelResp_Time.Text = Tools.DisallowNullTime(respondent.TimeCaptured, false);

        labelResp_Name.Text = Tools.CheckForNA(respondent.FirstName + " " + respondent.LastName);
        labelResp_Email.Text = respondent.Email;
        labelResp_Sex.Text = respondent.Sex.ToString();
        labelResp_Age.Text = Tools.CheckForNA((respondent.Age == "0") ? "" : respondent.Age);

        labelResp_Address.Text = Tools.CheckForNA(respondent.Address);
        labelResp_City.Text = Tools.CheckForNA(respondent.City);
        labelResp_StateProv.Text = Tools.CheckForNA(respondent.StateProv);
        labelResp_PostalCode.Text = Tools.CheckForNA(respondent.PostalCode);

        if (respondent.AreaCode.Trim() == "")
          labelResp_Tel1.Text = "N/A";
        else
        {
          string telNum = respondent.TelNum.Trim().Replace("-", ".").Replace(" ", ".");
          labelResp_Tel1.Text = respondent.AreaCode.Trim() + "." + telNum;
        }

        labelResp_Pollster.Text = Tools.CheckForNA(respondent.PollsterName);
        labelResp_GPS.Text = Tools.CheckForNA(respondent.GPS);


        // Middle panel: Question
        textBoxResp_Question.Text = quest.Text;


        // Lower panel: Answers
        if (quest.Choices.Count == 0)
        {
          string msg = "This question has no available choices.  You should notify the poll's creator: " + pageInfo.Model.CreationInfo.CreatorName;
          Tools.ShowMessage(msg, "Question Has No Choices", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }

        // Retrieve the Response for this Question and this Respondent
        int maxTextWid = 0;
        bool noResponses = true;
        _Response response = pageInfo.Respondents[currResp].Responses.Find_ResponseByQuestionID(quest.ID);

        // The only time a response would not be found is when a Question was added AFTER respondent data
        // was collected.  For this special case we will display a special message further on below.
        if (response != null)
        {
          // Display the AnswerFormat for the current question
          labelResp_AnswerFormat.Text = GetAnswerFormatLabel(quest.AnswerFormat);

          // Display the date & time the response was captured
          labelResp_AnswerDate.Text = Tools.DisallowNullDate(response.TimeCaptured);
          labelResp_AnswerTime.Text = Tools.DisallowNullTime(response.TimeCaptured, true);


          // Now add the available Choices

          // Instantiate & prepare the control we need to display the data in
          switch (quest.AnswerFormat)
          {
            case AnswerFormat.Standard:
            case AnswerFormat.List:
            case AnswerFormat.DropList:
            case AnswerFormat.MultipleChoice:
            case AnswerFormat.Range:
            case AnswerFormat.MultipleBoxes:
              if (listView.Items.Count == 0)
                listView.Items.Add(new ListViewItem(" "));

              itemHgt = listView.GetItemRect(0).Height;

              listView.Items.Clear();
              listView.Visible = true;
              panelExtra.Visible = false;
              labelMsg.Visible = false;
              textBox.Visible = false;
              panelSpin.Visible = false;

              switch (quest.AnswerFormat)
              {
                case AnswerFormat.Standard:
                case AnswerFormat.List:
                case AnswerFormat.DropList:
                case AnswerFormat.MultipleChoice:
                  listView.View = View.List;
                  break;

                case AnswerFormat.Range:
                case AnswerFormat.MultipleBoxes:
                  listView.View = View.Details;
                  break;
              }

              // Determine optimum size for ListView based on the items it's going to display
              //Tools.SetOptimumListViewSize(listView, itemHgt, new Size((int) (panelAnswers.Width * 0.6), panelAnswers.Height - (panelExtra.Height + 30)), quest.Choices);
              Tools.SetOptimumListViewSize(listView, itemHgt, new Size((int) (panelAnswers.Width * 0.6), panelAnswers.Height - 10), quest.Choices);
              break;

            case AnswerFormat.Freeform:
              textBox.Visible = true;

              panelSpin.Visible = false;
              listView.Visible = false;
              panelExtra.Visible = false;
              labelMsg.Visible = false;

              textBox.Size = new Size(240, 120);
              break;

            case AnswerFormat.Spinner:
              panelSpin.Visible = true;
              listView.Visible = false;
              panelExtra.Visible = false;
              textBox.Visible = false;
              labelMsg.Visible = false;
              break;

            default:
              Debug.Fail("Unknown AnswerFormat: " + quest.AnswerFormat.ToString(), "frmPoll.PopulateResponseData");
              break;
          }

          PreparePanelAnswerControls(quest.AnswerFormat);

          // Now populate the control(s) with the required data
          maxTextWid = listView.Columns[0].Width;  // Initial value

          // Cycle through the choices and populate the listview

          // ImageIndex Legend:
          //   -1 - No icon (default)
          //    0 - Blue dot = Choice selected
          //    1 - Blue dot with smaller red dot - Choice selected and "ExtraText" added as well
          //    2+ - Currently not used; reserved for future use
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
                  {
                    noResponses = false;

                    if (response.ExtraText == "" || choice.ExtraInfo == false)
                      lvwItem.ImageIndex = 0;
                    else
                    {
                      lvwItem.ImageIndex = 1;
                      lvwItem.Selected = true;
                    }
                  }

                  listView.Items.Add(lvwItem);
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

                      noResponses = false;
                    }
                  }

                  listView.Items.Add(lvwItem);
                }
                break;

              case AnswerFormat.Range:
                if (choice.Text != "")
                {
                  lvwItem.Text = "[" + choice.MoreText + "]";
                  lvwItem.SubItems.Add(choice.Text);

                  if (response.AnswerID == choice.ID.ToString())
                  {
                    lvwItem.ImageIndex = 0;
                    noResponses = false;
                  }

                  listView.Items.Add(lvwItem);
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
                      // The ExtraText in this case holds the values that were entered into each textbox, so the 
                      // resultant data is displayed differently.  Thus, no need for the ExtraInfo panel.
                      string extraText = Tools.GetExtraTextValue(response.ExtraText, choice.ID);
                      maxTextWid = Math.Max(maxTextWid, Tools.GetLabelWidth(extraText, listView.Font));
                      lvwItem.Text = extraText;   // This is generally a number - ex. "30", as in "Conservatives - 30%"
                      noResponses = false;
                    }
                  }

                  listView.Items.Add(lvwItem);
                }
                break;

              case AnswerFormat.Freeform:
                if (response.ExtraText != "")
                {
                  textBox.Text = response.ExtraText;
                  noResponses = false;
                }
                else
                  textBox.Text = "";
                break;

              case AnswerFormat.Spinner:
                if (response.AnswerID != "")
                {
                  labelAnswer.Text = response.AnswerID;
                  labelMinMax.Text = "Min: " + choice.Text + "    Max: " + choice.MoreText;
                  noResponses = false;
                }
                break;

              default:
                Debug.Fail("Unknown AnswerFormat: " + quest.AnswerFormat.ToString(), "frmStats.PopulateForm");
                break;
            }
          }

          if (noResponses)
            labelMsg.Text = "No response was obtained";
        }
        else
        {
          labelMsg.Text = "This question was added after responses were obtained";
        }

        if (noResponses)
        {
          textBox.Visible = false;
          panelSpin.Visible = false;
          listView.Visible = false;
          panelExtra.Visible = false;

          labelMsg.Left = (labelMsg.Parent.Width - labelMsg.Width) / 2;
          labelMsg.Visible = true;
        }
        else if (quest.AnswerFormat == AnswerFormat.MultipleBoxes)
        {
          if (maxTextWid > listView.Columns[0].Width)
          {
            int dif = listView.Width - (listView.Columns[0].Width + listView.Columns[1].Width);
            listView.Columns[0].Width = maxTextWid + 20;
            listView.Width = listView.Columns[0].Width + listView.Columns[1].Width + dif;
          }
        }
      }

      catch(Exception ex)
      {
        Debug.Fail("Error populating Response Data: " + ex.Message, "frmPoll.PopulateResponseData");
      }

      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }


    private string GetAnswerFormatLabel(AnswerFormat answerFormat)
    {
      // Declare a Resource Manager instance
      ResourceManager resMgr = new ResourceManager("Desktop.Desktop", typeof(frmPoll).Assembly);
      return resMgr.GetString("AnswerFormat." + answerFormat.ToString());    // ex: "Multiple Choice"
    }



    /// <summary>
    /// This event handler is called whenever the value of 'responsesPageInfo.CurrQuestion' is changed.
    /// It ensures that the corresponding Question button is pressed.  If the value of this property
    /// is -1 then it ensures that all buttons are unchecked.
    /// </summary>
    private void PressResp_QuestionButton(object sender, EventArgs e)
    {
      int qIdx = responsesPageInfo.CurrQuestion;
      Panel panel = responsesPageInfo.questButPanel;

      if (qIdx == -1)  // Ensure that all buttons are unchecked
      {
        foreach(Control ctrl in panel.Controls)
        {
          if (ctrl.GetType() == typeof(QuestionButton))
          {
            QuestionButton qBut = (QuestionButton) ctrl;
            qBut.Checked = false;
          }                                     
        }
      }
      else
      {
        int numCtrl = panel.Controls.Count;
        int numBut = Tools.CountControls(panel, new QuestionButton());   // The number of buttons that currently exist
        int nonButtons = numCtrl - numBut;

        QuestionButton qBut = panel.Controls[qIdx + nonButtons] as QuestionButton;
        qBut.Checked = true;
      }
    }


    /// <summary>
    /// This event handler is called whenever the value of 'responsesPageInfo.CurrRespondent' is changed.
    /// It ensures that the corresponding Respondent button is pressed.  If the value of this property
    /// is -1 then it ensures that all buttons are unchecked.
    /// </summary>
    private void PressResp_RespondentButton(object sender, EventArgs e)
    {
      int rIdx = responsesPageInfo.CurrRespondent;
      Panel panel = responsesPageInfo.respButPanel;
      
      if (rIdx == -1)  // Ensure that all buttons are unchecked
      {
        foreach(Control ctrl in panel.Controls)
        {
          if (ctrl.GetType() == typeof(ResponseButton))
          {
            ResponseButton rBut = (ResponseButton) ctrl;
            rBut.Checked = false;
          }                                     
        }
      }
      else
      {
        int numCtrl = panel.Controls.Count;
        int numBut = Tools.CountControls(panel, new ResponseButton());   // The number of buttons that currently exist
        int nonButtons = numCtrl - numBut;

        ResponseButton rBut = panel.Controls[rIdx + nonButtons] as ResponseButton;
        rBut.Checked = true;
      }
    }


    private void FilterRespondents(FilteredTabPageInfo filteredPageInfo)
    {
      // First filter by date
      _Respondents respondents = filteredPageInfo.Model.Respondents.FilterByDate(filteredPageInfo.DateSelections);

      // Now filter by sequence (ie. First 10, Last 25, etc.)
      respondents = respondents.FilterBySequence(filteredPageInfo.SeqSelection);

      filteredPageInfo.Respondents = respondents;
    }


    // This event handler prevents any item from being selected in the ListView.
    // The exception to this is when AnswerFormat.MultipleChoice is in effect.
    private void listViewResp_Answers_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      ListView lstView = sender as ListView;

      if (lstView.SelectedIndices.Count != 0)  // Necessary, to ensure that an item is selected
      {
        ListViewItem lvi = lstView.Items[lstView.SelectedIndices[0]];   // This is based on the assumption that multiple selection will not occur

        FilteredTabPageInfo pageInfo = responsesPageInfo;
        _Question quest = pageInfo.Model.Questions[pageInfo.CurrQuestion];   // Populate object variable with current question info

        if (lvi.ImageIndex > 0)   // -1 - No icon     0 - Selected item, but no extra info     1 - Selected item with extra info     2+ - Future
        {
          _Choice choice = quest.Choices[lvi.Index];

          if (choice.ExtraInfo)
          {
            _Response response = pageInfo.Respondents[pageInfo.CurrRespondent].Responses.Find_ResponseByQuestionID(quest.ID);

            string extraInfo = "";
            if (quest.AnswerFormat == AnswerFormat.MultipleChoice)
              extraInfo = Tools.GetExtraTextValue(response.ExtraText, choice.ID);
            else
              extraInfo = response.ExtraText;

            DisplayExtraInfo(choice.MoreText, extraInfo);
          }
        }

        else
        {
          if (quest.AnswerFormat == AnswerFormat.MultipleChoice)   // Allow no unselecting if not multiple choice
          {
            DisplayExtraInfo("", "");  // Hide ExtraInfo panel
          }
          
          lvi.Selected = false;
        }
      }
    }


    /// <summary>
    /// This method is called from 'listViewResp_Answers_SelectedIndexChanged' and takes
    /// care of populating all of the pertinent controls within 'panelResp_ExtraInfo'.
    /// </summary>
    /// <param name="introText"></param>
    /// <param name="extraInfo"></param>
    private void DisplayExtraInfo(string introText, string extraInfo)
    {
      int gap = 4;
      TextBox textBox = textBoxResp_ExtraInfo;
      Panel panel = panelResp_ExtraInfo;

      if (introText == "" && extraInfo == "")
        panel.Visible = false;
      else
      {
        panel.Width = listViewResp_Answers.Width;

        if (introText == "")
          labelResp_ExtraInfoIntro.Text = "";
        else
          labelResp_ExtraInfoIntro.Text = Tools.EnsureSuffix(introText, ":");        

        textBox.Text = extraInfo;
        textBox.Left = gap * 3;

        int reqdWidth = 50;  // A reasonable minimum width
        if (textBox.Lines.Length == 1)
          reqdWidth = Tools.GetLabelWidth(extraInfo, textBox.Font);
        else
          for (int i = 0; i < textBox.Lines.Length; i++)
          {
            reqdWidth = Math.Max(reqdWidth, Tools.GetLabelWidth(textBox.Lines[i], textBox.Font));
          }

        int availWidth = panel.Width - gap * 6;   // gap * 3 | content | gap * 3
        textBox.Width = Math.Min((int) (reqdWidth * 1.1), availWidth);

        if ((reqdWidth < availWidth) && (textBox.Lines.Length == 1))
        {
          textBox.Multiline = false;
        }
        else
        {
          textBox.Multiline = true;
          int hgt = (int) (Tools.GetLabelHeight(extraInfo, textBox.Font, textBox.Width) * 1.1);
          if (hgt > 120)
          {
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.Width += 30;
            textBox.Height = 120;
          }
          else
            textBox.Height = hgt;
        }

        panel.Height = textBox.Bottom + gap * 3;

        if (listViewResp_Answers.Bottom + 20 + panel.Height > panel.Parent.Height)
          panel.Location = new Point(listViewResp_Answers.Right + 10, listViewResp_Answers.Top + listViewResp_Answers.Height / 2);
        else
          panel.Location = new Point(listViewResp_Answers.Left, listViewResp_Answers.Bottom + 20);

        panel.Visible = true;
      }
    }



    /// <summary>
    /// This method repositions the children of 'panelAnswers'.
    /// Note: It depends upon 'panelResp_ActualQuestion' being above 'panelAnswers' and not a child of it.
    /// </summary>
    /// <param name="answerFormat"></param>
    private void PreparePanelAnswerControls(AnswerFormat answerFormat)
    {
      int gap = 4;

      ListView listView = listViewResp_Answers;
      Panel panelExtra = panelResp_ExtraInfo;
      TextBox textBox = textBoxResp_Answer;
      Panel panelSpin = panelResp_Spinner;
      Label labelMsg = labelResp_AnswersMsg;

      labelMsg.Location = new Point((panelAnswers.Width - labelMsg.Width) / 2, gap * 3);

      switch (answerFormat)
      {
        case AnswerFormat.Standard:
        case AnswerFormat.List:
        case AnswerFormat.DropList:
        case AnswerFormat.MultipleChoice:
        case AnswerFormat.Range:
        case AnswerFormat.MultipleBoxes:
          listView.Location = new Point((panelAnswers.Width - listView.Width) / 2, gap * 3);
          panelExtra.Location = new Point(listView.Left, listView.Bottom + gap * 4);
          //labelMsg.Location = new Point(listView.Left + (listView.Width - labelMsg.Width) / 2, listView.Bottom + gap * 2);
          panelAnswers.AutoScrollMinSize = new Size (panelAnswers.AutoScrollMinSize.Width, panelExtra.Bottom + gap);
          break;

        case AnswerFormat.Freeform:
          textBox.Location = new Point((textBox.Parent.Width - textBox.Width) / 2, gap * 3);
          //labelMsg.Location = new Point(textBox.Left + (textBox.Width - labelMsg.Width) / 2, textBox.Bottom + gap * 2);
          panelAnswers.AutoScrollMinSize = new Size (panelAnswers.AutoScrollMinSize.Width, labelMsg.Bottom + gap);
          break;

        case AnswerFormat.Spinner:
          panelSpin.Top = gap * 3;
          //labelMsg.Location = new Point(panelSpin.Left + (panelSpin.Width - labelMsg.Width) / 2, panelSpin.Bottom + gap * 2);
          panelAnswers.AutoScrollMinSize = new Size (panelAnswers.AutoScrollMinSize.Width, labelMsg.Bottom + gap);
          break;

        default:
          Debug.Fail("Unknown AnswerFormat: " + answerFormat.ToString(), "frmPoll.PreparePanelAnswerControls");
          break;
      }

      // Shouldn't be necessary to explicitly have to set these but for some reason it takes on mysterious values!
      // I think the problem may have something to do with the OnPaint method of PanelGradient.
      listView.Top = 12;  
      textBox.Top = listView.Top;
      panelSpin.Top = listView.Top;
      labelMsg.Top = listView.Top;
    }

    #endregion


    #region Q&A_TabPage

    private void panelQuestionList1_Resize(object sender, System.EventArgs e)
    {
      Tools.CenterPanelControls(sender as Panel);
    }


    /// <summary>
    /// Prepares the toggle buttons that let the user change the format of the possible answers (choices).
    /// </summary>
    public void PrepareAnswerButtons()
    {
      int numButtons = Enum.GetNames(typeof(AnswerFormat)).Length;
    
      // Declare a Resource Manager instance
      ResourceManager resMgr = new ResourceManager("Desktop.Desktop", typeof(frmPoll).Assembly);

      // Create all of the buttons
      for (byte i=0; i < numButtons; ++i)
      {
        // Instantiate a new radio button
        AnswerButton rad = new AnswerButton();
        
        string prefix = "AnswerFormat";                          // ie. "radioAnswerFormat"
        string suffix = Enum.GetName(typeof(AnswerFormat), i);   // This is the enum value, converted to string format
        rad.Name = prefix + suffix;                              // ie. "radioAnswerMultipleChoice"
        rad.AnswerFormat = (AnswerFormat) i;

        // Prepare the Tag property so that these buttons will work with ASM
        rad.Tag = "Property=Questions[].AnswerFormat, Value=" + i.ToString() + ", PubLock=AnswerFormat";

        //Debug: Still need to test that this next line works for other languages!!!
        rad.Text = resMgr.GetString(prefix + "." + suffix);    // ex: "Multiple Choice"

        // Let's add a double-check to ensure buttons are being labelled correctly
        if (rad.Text == "")
          Debug.Fail("No value retrieved from resource file for this key: " + prefix + "." + suffix, "frmPoll.PrepareAnswerButtons");

        int newleft = 8 + (int) (i * rad.Width * 1.05);
        rad.Location = new System.Drawing.Point(newleft, 5);

        // Wire in an event handler for when any of the buttons are pressed
        rad.CheckedChanged += new EventHandler(AnswerFormat_Event);

        // And finally, add this new radio button to its parent panel
        panelAnswerFormat.Controls.Add(rad);

        // Now that the radioButtons have been installed, press the first one ("Standard")
        // Note: 'PerformClick' doesn't work, I think because the form's instantiation isn't complete at this point
        if (i == 0)
          rad.Checked = true;
      }
    }


    // Reset textQuestion back to its default appearance and set focus to textQuestion control
    public void SetDefaultQuestionPrompt()
    {
      textQuestion.Text = DefaultQuestionText;
      textQuestion.Focus();
    }



    
    
    /// <summary>
    /// "Presses" the specified Question button, if it isn't already pressed.
    /// Also does the following:
    ///   - Clears CurrentPreviewInfo, since the setting of another question has no bearing on the current one
    ///   - Resets CurrentPreviewExtraInfo, since ...
    ///   - Sets textQuestion.Text to that of new current question, with special code to handle an empty (or default) question
    ///   - Sets the correct AnswerFormat radioButton, which in turn:
    ///         i. Clears panelChoices
    ///        ii. Populates panelChoices according to contents in Questions[].Choices[]
    ///              - If Choices is empty then provide one default choice
    /// </summary>
    /// <param name="model"></param>
    /// <param name="index"></param>    // 0-based index of Question
    /// <param name="ASMlist"></param>
    public void SetCurrentQuestion(Poll model, int index, SortedList ASMlist)
    {
      // Since we're moving to a different question, the old Preview settings no longer have any relevance
      PreviewInfo = new DataObjects.PreviewInfo();

      CurrentQuestion = index;       // Module-level variable that keeps track of the current question index

      int nonButtons = Tools.CountExtraneousControls(panelQuestionList1, new QuestionButton());
      QuestionButton qBut = panelQuestionList1.Controls[index + nonButtons] as QuestionButton;

      //QuestionButton qBut = panelQuestionList1.Controls[index] as QuestionButton;

      if (qBut.Checked  == false)
        qBut.Checked = true;
    
      // Not the perfect solution, but an acceptable way to keep the new button in view
      panelQuestionList1.ScrollControlIntoView(qBut);       

      // Now do everything else necessary to display all the related data for this question
      _Question quest = model.Questions[index];

      // Press the correct AnswerFormat radio button
      byte afIndex = (byte) quest.AnswerFormat;

      // Reset the previously selected Choice for this question and update panelChoices
      panelChoices.Tag = "";  // We must reset this because we're moving to a new question
      AnswerButton answerButton = panelAnswerFormat.Controls[afIndex] as AnswerButton;
      if (answerButton.Checked)
        UpdateChoices(model, ASMlist, index, quest.AnswerFormat);
      else
      {
        LockIsDirty = true;
        answerButton.Checked = true;  // This will also call 'UpdateChoices'
        LockIsDirty = false;
      }

      SetCurrentChoice(CurrentChoice, 0);

      // Prepare all the default information for this new question and set focus to it as well
      string newText = quest.Text;

      if (newText == "" | newText == DefaultQuestionText)  // | newText == null)
      {
        SetDefaultQuestionPrompt();
      
        // If there's just the initial default question then no need for "Remove"
        if (model.Questions.Count == 1)
          buttonRemoveQuestion.Enabled = false;
      }
      else
        textQuestion.Text = newText;
    }


    /// <summary>
    /// Highlight the specified panel, giving the impression that it is "pressed", like a button.
    /// And optionally, give focus to the control with the specified TabIndex.
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="tabindex"></param>
    private void SetCurrentChoice(PanelChoice panel, int tabindex)
    {
      if (CurrentQuestion + 1 > Tools.CountControls(panelQuestionList1, new QuestionButton()))
        return;

      // Since panels don't "gain focus" per say, we're going to use the tag
      // property of each Question button to hold the index of the current panel.

      // First unhighlight previously selected panel (if there was one)
      int nonButtons = Tools.CountExtraneousControls(panelQuestionList1, new QuestionButton());
      QuestionButton qBut = panelQuestionList1.Controls[CurrentQuestion + nonButtons] as QuestionButton;
      
      int prevChoice = CurrentChoice;

      // Use some special logic here to help eliminate cases where we don't need to unhighlight anything
      Panel parent = panel.Parent as Panel;
      if (prevChoice + 1 <= parent.Controls.Count)
      {
        try
        {
          (parent.Controls[prevChoice] as PanelChoice).SetBackColor("Up");
        }
        catch
        {
          // Handle exceptions
          Debug.Fail("Error referencing previously selected Choice panel", "frmPoll.SetCurrentChoice.panel");
        }
      }
      
      // Now we can highlight the new current one
      panel.SetBackColor("Down");            // Highlight the new current panel
      CurrentChoice = panel.ChoiceNum;       // and record its index number at the module level (this will also store it in its question button's Tag)
      parent.ScrollControlIntoView(panel);   // Not the perfect solution, but an acceptable way to keep the highlighted panel in view
    
      // And finally, set the current focus to the primary control, if so indicated
      if (tabindex != -1)
      {
        // And finally, ensure that the control with the specified TabIndex gains focus
        foreach (Control control in panel.Controls)
        {
          if (control.TabIndex == tabindex)
          {
            control.Focus();
            break;
          }
        }
      }
    }

    public void SetCurrentChoice(int choiceIndex, int tabindex)
    {
      int numpanels = panelChoices.Controls.Count;

      if (numpanels > 0)
      {
        if (choiceIndex <= (numpanels - 1))
          SetCurrentChoice(panelChoices.Controls[choiceIndex] as PanelChoice, tabindex);
        else
          SetCurrentChoice(0, tabindex);   // Circumstances have removed the previously current panel so set the first panel instead
      }
    }

    public void SetCurrentChoice(int choiceIndex)
    {
      SetCurrentChoice(choiceIndex, -1);
    }

    public void SetCurrentChoice(string strChoiceIndex, int tabindex)
    {
      int numpanels = panelChoices.Controls.Count;
      
      if (numpanels > 0)
      {
        if (strChoiceIndex == "")
          strChoiceIndex = "0";

        int choiceIndex = Convert.ToInt32(strChoiceIndex);
 
        if (choiceIndex <= (numpanels - 1))
          SetCurrentChoice(panelChoices.Controls[choiceIndex] as PanelChoice, tabindex);
        else
          Debug.Fail("Illegal panel number encountered: " + strChoiceIndex, "frmPoll.SetCurrentChoice.string");
      }
    }


    /// <summary>
    /// This method, which is called from 'AnswerFormatChanged' in the Controller, does the following:
    ///   - Clears panelChoices
    ///   - Populates panelChoices according to the contents of Questions[#].Choices[], where # = CurrentQuestion
    /// Note: If Choices is empty then we'll provide one default choice.
    /// 
    /// Future Improvement: Don't blanket erase everything and redraw.  Instead intelligently add/insert/remove new item.
    ///                     Note: Carefully think out if this will actually improve or worsen redraw speed!
    /// </summary>
    /// <param name="model"></param>           // Note: Passed by reference from the Controller
    /// <param name="asmList"></param>         // Note: Passed by reference from the Controller
    /// <param name="qIdx"></param>            // Question Index
    /// <param name="answerFormat"></param>    // Current Answer Format
    public void UpdateChoices(Poll model, SortedList asmList, int qIdx, AnswerFormat answerFormat)
    {
      // Clear panelChoices
      this.SuspendLayout();
      panelChoices.Controls.Clear();
      buttonAddChoice.Enabled = true;
      buttonRemoveChoice.Enabled = true;

      // If no questions yet then just exit
      if (model.Questions.Count == 0)
      {
        this.ResumeLayout();
        return;
      }

      // Before adding any new panels and controls therein we need to cleanup the ASMlist.  Otherwise
      // we'll have entries with ASMlist that containing a growing number of redundant sibling entries.
      ASM.ClearASMofChoiceTags(asmList);

      // Update panelChoices based on this criteria:
      //  - What is the current Question?
      //  - What are the 'Choices' of this Question?
      //  - What is the current Answer Format?
      
      // Prepare various variables before entering loop
      int gap = 4;   // Used both vertically & horizontally
      int topMargin = gap;
      bool exitLoop = false;
      PanelChoice newpanel = new PanelChoice();
      AnswerFormat aFormat = answerFormat;

      // Add one or more choices within loop
      foreach (_Choice choice in model.Questions[qIdx].Choices)
      {
        switch (aFormat)
        {
          case AnswerFormat.Standard:
          case AnswerFormat.List:
          case AnswerFormat.DropList:
          case AnswerFormat.MultipleChoice:
          case AnswerFormat.Range:
          case AnswerFormat.MultipleBoxes:
            newpanel = AddChoice(choice, aFormat, panelChoices, topMargin, asmList);
            break;

          case AnswerFormat.Freeform:
            newpanel = AddChoice(choice, aFormat, panelChoices, topMargin, asmList);
            buttonAddChoice.Enabled = false;
            buttonRemoveChoice.Enabled = false;
            exitLoop = true;   // We only want one Choice, not multiple ones
            break;

          case AnswerFormat.Spinner:
            newpanel = AddChoice(choice, aFormat, panelChoices, topMargin, asmList);
            buttonAddChoice.Enabled = false;
            buttonRemoveChoice.Enabled = false;
            exitLoop = true;   // We only need one Choice panel to define a Spinner
            break;

          default:
            Debug.WriteLine("Unknown AnswerFormat encountered: " + aFormat.ToString(), "frmPoll.UpdateChoices");
            break;
        }

        if (newpanel != null)
          topMargin += newpanel.Height + gap;

        if (exitLoop == true)
          break;
      }


      // Debug: Not sure why, but this code is not setting the TabIndexes properly - RW - 2005-06-02
      //        Have to investigate how panels affect overall TabIndex sequencing!
      //      // Correctly set TabIndexes of textboxes within the Choice panels
      //      int tabIdx = buttonRemoveChoice.TabIndex;
      //
      //      for (int i = 0; i < panelChoices.Controls.Count; i++)
      //      {
      //        tabIdx++;
      //        PanelChoice panel = panelChoices.Controls[i] as PanelChoice;
      //        TextBox textbox = panel.Controls[0] as TextBox;   // Note: Currently only designed to work with one object in each panelChoice
      //        textbox.TabIndex = tabIdx;
      //      }

      // Now update the preview panel
      UpdatePreview(model, qIdx, answerFormat);

      this.ResumeLayout();
    }

    
    
    /// <summary>
    /// This method is only called by 'UpdateChoices'.  It is called successively for each sub-panel:
    /// First for the first one, then the 2nd one, and so on.  Some of the logic inside this method
    /// takes advantage of this fact.
    /// </summary>
    /// <param name="choice"></param>            // The Choice object, whose data we'll display in the Choice panel
    /// <param name="aFormat"></param>           // AnswerFormat
    /// <param name="parentPanel"></param>       // The panel to add the new Choice to - currently panelChoices
    /// <param name="topMargin"></param>         // Provides the precise Y value that will become the 'Top' property of the new choice
    /// <param name="asmList"></param>
    /// <returns>The newly added panel</returns>
    private PanelChoice AddChoice(_Choice choice, AnswerFormat aFormat, Panel parentPanel, int topMargin, SortedList asmList)
    {
      int gap = 4;

      // First figure out where in the panel this Choice should go
      int index = parentPanel.Controls.Count;
      int width = parentPanel.ClientSize.Width - 24;     // Max available width for sub-panels

      // Now instantiate a new Choice panel to hold the contents of this choice's data
      PanelChoice panel = new PanelChoice();
      panel.Name = panel.NamePrefix + index.ToString();  // ie. "panelChoice0", "panelChoice1", etc.
      panel.ChoiceNum = index;   // Zero-based index

      panel.Width = width;
      panel.Top = topMargin;    //panel.Top = gap + index * (panel.Height + gap);
      panel.Left = 12;

      // Wire in an event handler for when any of the panels gains focus.  For now we're just going to use this
      // for creating the illusion that the panel is actually selected (when in facts its children are selected).
      panel.Click += new EventHandler(Choice_Event);

      // Wire in event handlers to handle drag & drop of the panel (so its order can be changed)
      panel.MouseDown += new MouseEventHandler(GeneralControl_MouseDown);
      panel.MouseUp += new MouseEventHandler(GeneralControl_MouseUp);


      // Now add onto this sub-panel the control(s) that will display the Choice data
      int chkWidth;
      int chkLeft;
      int txtLeft;
      int txtWidth;
      string introText;
      TextBox textbox;
      Font font = new Font("Arial", 10, FontStyle.Regular);   // Default font throughout this method

      // Add other controls, where applicable
      switch (aFormat)
      {
        case AnswerFormat.Standard:
        case AnswerFormat.List:
        case AnswerFormat.DropList:
        case AnswerFormat.MultipleChoice:
          // First add checkbox on right side, as that will determine avail width of main textbox
          chkWidth = Tools.GetLabelWidth("Allow More Info:", font) + 20;
          chkLeft = width - gap - chkWidth - 6;
          AddCheckBox("chkExtraInfo", "ExtraInfo", index, -1, chkLeft, chkWidth, anchorTR, choice.ExtraInfo, "Allow More Info:", font, false, panel, 1, asmList);
     
          // Now add primary Answer textbox, along with introductory label
          introText = "Answer " + (index + 1).ToString() + ":";
          txtWidth = chkLeft - gap - Tools.GetLabelWidth(introText, font) - 50;
          textbox = AddTextBox("txtAnswer", "Text", index, -1, gap, txtWidth, anchorTLR, choice.Text, font, null, null, introText, panel, 0, asmList);

          // See if we need to display more info in the same panel
          if (choice.ExtraInfo)
          {
            // 2006-09-11 - Here we're adding a new feature to allow the creator to specify a multiline text box for MoreInfo
            chkWidth = Tools.GetLabelWidth("Multiline Textbox:", font) + 20;
            chkLeft = textbox.Right - chkWidth;
            AddCheckBox("chkExtraInfoMultiline", "ExtraInfoMultiline", index, 50, chkLeft, chkWidth, anchorTR, choice.ExtraInfoMultiline, "Multiline Textbox:", font, false, panel, 3, asmList);
            
            
            panel.Height = (int) (1.75 * panel.BaseHeight);   // Expand height of panel to accommodate more controls
            txtWidth = chkLeft - (gap * 8 + Tools.GetLabelWidth("More Info Introduction:", font));
            AddTextBox("txtMoreInfo", "MoreText", index, 50, gap, txtWidth, anchorTLR, choice.MoreText, font, null, null, "More Info Introduction:", panel, 2, asmList);
          }
          break;


        case AnswerFormat.Range:
          // First add primary Answer textbox, along with introductory label
          introText = "Answer " + (index + 1).ToString() + ":";
          textbox = AddTextBox("txtAnswer", "Text", index, -1, gap, (int) (width / 2), anchorTLR, choice.Text, font, null, null, introText, panel, 0, asmList);
          
          // Then add a corresponding numeric value textbox
          // Debug: Won't this cause a problem when flipping between AnswerFormats 0 - 3 and this one, #4 ???
          introText = "Numeric Value:";
          txtLeft = width - (gap + 10 + Tools.GetLabelWidth(introText, font) + 30);
          textbox = AddTextBox("txtNumericScale", "MoreText", index, -1, txtLeft, 30, anchorTR, Tools.NumericOnly(choice.MoreText), font, "0", "R", introText, panel, 1, asmList);
          
          // Only allow numeric entry into this textbox
          textbox.KeyDown += new KeyEventHandler(textBox_KeyDown);
          textbox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
          break;


        case AnswerFormat.MultipleBoxes:
          // Add primary Answer textbox, along with introductory label
          introText = "Answer " + (index + 1).ToString() + ":";
          txtWidth = width - (gap + 10 + Tools.GetLabelWidth(introText, font) + 30);
          textbox = AddTextBox("txtAnswer", "Text", index, -1, gap, txtWidth, anchorTLR, choice.Text, font, null, null, introText, panel, 0, asmList);
          break;


        case AnswerFormat.Freeform:
          font = new Font("Arial", 10, FontStyle.Italic);
          AddLabel("Nothing else is required to define this Freeform option.", font, -1, -1, anchorTLR, panel, true);
          break;


        case AnswerFormat.Spinner:
          // Add Minimum, Maximum, and Increment textboxes
          textbox = AddTextBox("txtSpinnerMin", "Text", index, -1, gap, 30, anchorTL, Tools.NumericOnly(choice.Text), font, "0", "R", "Minimum:", panel, 0, asmList);
          // Only allow numeric entry into this textbox
          textbox.KeyDown += new KeyEventHandler(textBox_KeyDown);
          textbox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);

          textbox = AddTextBox("txtSpinnerMax", "MoreText", index, -1, textbox.Right + 50, 30, anchorTL, Tools.NumericOnly(choice.MoreText), font, "0", "R", "Maximum:", panel, 1, asmList);
          // Only allow numeric entry into this textbox
          textbox.KeyDown += new KeyEventHandler(textBox_KeyDown);
          textbox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
          
          textbox = AddTextBox("txtSpinnerStep", "MoreText2", index, -1, textbox.Right + 50, 30, anchorTL, Tools.NumericOnly(choice.MoreText2), font, "0", "R", "Increment:", panel, 2, asmList);
          // Only allow numeric entry into this textbox
          textbox.KeyDown += new KeyEventHandler(textBox_KeyDown);
          textbox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
          
          break;


        default:
          Debug.WriteLine("Unknown AnswerFormat encountered: Code = " + aFormat.ToString(), "frmPoll.AddChoice");
          break;
      }

      // Add this new sub-panel to its parent panel
      parentPanel.Controls.Add(panel);

      if (parentPanel.Controls.Count > 0)
        buttonRemoveChoice.Enabled = true;

      return panel;  // Returns a reference to the newly added panel
    }



    private void SetPanelRightAnchors(bool setting)
    {
      foreach(PanelChoice panel in panelChoices.Controls)
      {
        panel.SetAnchorRight(setting);
      }
    }


    #region Choice_AddControls

    /// <summary>
    /// This method adds a textbox to the specified panel
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="tagsuffix"></param>
    /// <param name="index"></param>
    /// <param name="top"></param>             // Use -1 to have textbox vertically centered in panel
    /// <param name="left"></param>            // Use -1 to have textbox horizontally centered in panel
    /// <param name="width"></param>
    /// <param name="anchors"></param>
    /// <param name="text"></param>
    /// <param name="font"></param>
    /// <param name="textformat"></param>      // Used to force 'text' into a specific format
    /// <param name="introLabel"></param>      // Use null or "" if no introductory label
    /// <param name="panel"></param>
    /// <param name="tabindex"></param>
    /// <param name="asmList"></param>
    /// <returns>A reference to the newly added textbox</returns>
    private TextBox AddTextBox(string prefix, string tagsuffix, int index, int top, int left, int width, AnchorStyles anchors, string text, Font font, string textformat, string textalign, string introLabel, Panel panel, int tabindex, SortedList asmList)
    {
      TextBox textbox = new TextBox();
      textbox.Name = prefix + index.ToString();
      textbox.Tag = "Property=Questions[].Choices[]." + tagsuffix+ ", Value=" + index.ToString() + ", PubLock=QA_Basic";
      textbox.TabIndex = tabindex;

      textbox.Font = font;
      
      // Determine which text alignment should be used
      if (textalign == "" | textalign == null)
        textalign = "L";

      switch (textalign.Substring(0,1).ToUpper())
      {
        case "R":
          textbox.TextAlign = HorizontalAlignment.Right;
          break;

        case "C":
          textbox.TextAlign = HorizontalAlignment.Center;
          break;

        default:
          textbox.TextAlign = HorizontalAlignment.Left;
          break;
      }

      textbox.Anchor = anchors;
      textbox.Width = width;
      Label label = new Label();   // Need to initialize because of code below

      if (left == -1)
      {
        if (introLabel != null & introLabel != "")
        {
          int totalWidth = Tools.GetLabelWidth(introLabel, font) + 2 + textbox.Width;
          label = AddLabel(introLabel, font, -1, (panel.Width - totalWidth) / 2, anchors, panel, true);
          textbox.Left = label.Left + label.Width + 2;
        }
      }
      else
      {
        if (introLabel != null & introLabel != "")
        {
          label = AddLabel(introLabel, font, -1, left, anchors, panel, true);
          textbox.Left = left + 2 + label.Width;
        }
      }

      if (top == -1)
        textbox.Top = (panel.Height - textbox.Height) / 2;
      else
        textbox.Top = top;

      textbox.Text = Tools.FormatString(text, textformat);

      if (introLabel != null & introLabel != "")
        label.Top = textbox.Top + (textbox.Height - label.Height) / 2;

      panel.Controls.Add(textbox);

      // We need this check because calls to this method regarding panelPreview don't involve ASM
      if (asmList != null)
      {
        // This adds this new textbox to ASMlist and if that goes okay then adds event handlers to it
        if (ASM.ActivateASM(asmList, textbox))
        {
          // Establish the general event handler for this new textbox
          EstablishASMEventHandlers(textbox);

          // Add a Choice specific handler that will ensure the textbox's parent panel is highlighted
          //textbox.Click += new System.EventHandler(this.Choice_Event);
          textbox.GotFocus += new System.EventHandler(this.Choice_Event);
        }
      }

      return textbox;
    }

 
    
    /// <summary>
    /// Adds a label to the specified panel.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="font"></param>
    /// <param name="top"></param>             // Use -1 to have label vertically centered in panel
    /// <param name="left"></param>            // Use -1 to have label horizontally centered in panel
    /// <param name="anchors"></param>
    /// <param name="panel"></param>
    /// <param name="addEvent"></param>        // If true then click event will be wired in
    /// <returns>A reference to the newly added label</returns>
    private Label AddLabel(string text, Font font, int top, int left, AnchorStyles anchors, Panel panel, bool addEvent)
    {
      Label label = new Label();

      label.AutoSize = true;
      label.Anchor = anchors;
      label.TabIndex = 98;   // Outrageously large number

      label.Font = font;
      label.Text = text;

      if (left == -1)
      {
        label.Left = (panel.Width - label.Width) / 2;
        label.TextAlign = ContentAlignment.MiddleCenter;
      }
      else
      {
        label.Left = left;
        label.TextAlign = ContentAlignment.MiddleLeft;
      }

      if (top == -1)
        label.Top = (panel.Height - label.Height) / 2;
      else
        label.Top = top;

      panel.Controls.Add(label);

      if (addEvent)
        label.Click += new System.EventHandler(this.Choice_Event);

      return label;
    }



    /// <summary>
    /// Adds a multiline label to a panel.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="font"></param>
    /// <param name="top"></param>
    /// <param name="left"></param>
    /// <param name="height"></param>     // If -1 then 'width' will be used as "max width" and the height will be calculated
    /// <param name="width"></param>
    /// <param name="anchors"></param>
    /// <param name="panel"></param>
    /// <param name="addEvent"></param>
    /// <returns></returns>
    private Label AddMultilineLabel(string text, Font font, int top, int left, int height, int width, AnchorStyles anchors, Panel panel, bool addEvent)
    {
      Label label = new Label();

      label.AutoSize = false;
      label.Anchor = anchors;
      label.TabIndex = 99;   // Outrageously large number

      label.Font = font;
      label.Text = Tools.FixPanelText(text);

      if (height == -1)
      {
        //        int width2 = Tools.GetLabelWidth(text, font);
        //        int numlines = (width2 / width) + 1;    // ex. 2.7 -> 3
        //        height = 1 + (int) (font.Size * 1.625 * numlines);
        height = Tools.GetLabelHeight(text, font, width);
      }
 
      label.Height = height;
      label.Width = width;

      if (left == -1)
      {
        label.Left = (panel.Width - label.Width) / 2;
        label.TextAlign = ContentAlignment.MiddleCenter;
      }
      else
      {
        label.Left = left;
        label.TextAlign = ContentAlignment.MiddleLeft;
      }

      if (top == -1)
        label.Top = (panel.Height - label.Height) / 2;
      else
        label.Top = top;

      panel.Controls.Add(label);

      if (addEvent)
        label.Click += new System.EventHandler(this.Choice_Event);

      return label;
    }



    /// <summary>
    /// Adds a checkbox to the specified panel.
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="tagsuffix"></param>
    /// <param name="index"></param>
    /// <param name="top"></param>             // Use -1 to have checkbox vertically centered in panel
    /// <param name="left"></param>            // Use -1 to have checkbox horizontally centered in panel
    /// <param name="width"></param>
    /// <param name="anchors"></param>
    /// <param name="checkstate"></param>
    /// <param name="text"></param>
    /// <param name="font"></param>
    /// <param name="checkonleft"></param>
    /// <param name="panel"></param>
    /// <param name="tabindex"></param>
    /// <param name="asmList"></param>
    /// <returns>A reference to the newly added checkbox</returns>
    private CheckBox AddCheckBox(string prefix, string tagsuffix, int index, int top, int left, int width, AnchorStyles anchors, bool checkstate, string text, Font font, bool checkonleft, Panel panel, int tabindex, SortedList asmList)
    {
      CheckBox checkbox = new CheckBox();

      checkbox.Name = prefix + index.ToString();
      checkbox.Tag = "Property=Questions[].Choices[]." + tagsuffix + ", Value=" + index.ToString() + ", PubLock=QA_Basic";
      checkbox.TabIndex = tabindex;

      checkbox.Font = font;
      checkbox.Anchor = anchors;

      checkbox.TextAlign = ContentAlignment.MiddleLeft;
      if (checkonleft)
        checkbox.CheckAlign = ContentAlignment.MiddleLeft;
      else
        checkbox.CheckAlign = ContentAlignment.MiddleRight;

      checkbox.Width = width;
      
      if (left == -1)
        checkbox.Left = (panel.Width - checkbox.Width) / 2;
      else
        checkbox.Left = left;

      if (top == -1)
        checkbox.Top = (panel.Height - checkbox.Height) / 2;
      else
        checkbox.Top = top;

      checkbox.Text = text;
      checkbox.Checked = checkstate;

      panel.Controls.Add(checkbox);

      // We need this check because calls to this method regarding panelPreview doesn't involve ASM
      if (asmList != null)
      {
        // This adds this new checkbox to ASMlist and if that goes okay then adds event handlers to it
        if (ASM.ActivateASM(asmList, checkbox))
        {
          // Establish the general event handler for this new checkbox
          EstablishASMEventHandlers(checkbox);

          // Add a Choice specific handler that will ensure the checkbox's parent panel is highlighted
          checkbox.Click += new System.EventHandler(this.Choice_Event);
          checkbox.GotFocus += new System.EventHandler(checkBox_GotFocus);
        }
      }

      return checkbox;
    }

    #endregion



    // Called from above; simply sets the correct choice based on which checkbox gains focus.
    private void checkBox_GotFocus(object sender, EventArgs e)
    {
      CheckBox checkBox = sender as CheckBox;
      PanelChoice panel = checkBox.Parent as PanelChoice;
      SetCurrentChoice(panel, checkBox.TabIndex);
    }



    // If text in textQuestion is the default text then we'll specially handle it.
    private void textQuestion_Enter(object sender, EventArgs e)
    {
      if (textQuestion.Text == DefaultQuestionText)
        textQuestion.SelectAll();
    }


    // Not sure why the textbox doesn't do this automatically, but this ensures it does.
    private void textQuestion_DoubleClick(object sender, EventArgs e)
    {
      textQuestion.SelectAll();
    }



    public event System.EventHandler AnswerFormatEvent;

    /// <summary>
    /// This event handler is called by the AnswerFormat radio buttons (near the top of the Questions & Answers page).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AnswerFormat_Event(object sender, EventArgs e)
    {
      try
      {
        AnswerButton aBut = sender as AnswerButton;
        
        if (aBut.Checked == true)  
        {
          aBut.SetBackColor("Down");
          
          if (AnswerFormatEvent != null)
          {
            AnswerFormatEvent(sender, new EventArgs());
          }
        }
        else
          aBut.SetBackColor("Up");   // radioButton returning upward so restore original color
      }

      catch (Exception ex)
      {
        Debug.Fail("Error: " + e.GetType().ToString() + "\n\nMessage: " + ex.Message, "frmPoll.AnswerFormat_Event");
      }
    }



    /// <summary>
    /// This event handler is called by the Question radio buttons or Add/Duplicate/Remove buttons
    /// that reside on the right side of the Questions & Answers page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public event System.EventHandler QuestionButtonEvent;
    private void QuestionButton_Event(object sender, EventArgs e)
    {
      try
      {
        if (sender.GetType().Name == "Button")   // See if it's the Add/Duplicate/Remove button in the Question List console
        {
          if (QuestionButtonEvent != null)
            QuestionButtonEvent(sender, new EventArgs());
        }
        
        else   // It must be one of the numeric Question selector buttons
        {
          QuestionButton qBut = sender as QuestionButton;
        
          if (qBut.Checked == true)  
          {
            qBut.SetBackColor("Down");
          
            if (QuestionButtonEvent != null)
              QuestionButtonEvent(sender, new EventArgs());
          }
          else
            qBut.SetBackColor("Up");
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("Error: " + e.GetType().ToString(), "frmPoll.QuestionButtion_Event");
      }
    }



    /// <summary>
    /// This event handler is called by:
    ///  - The Add Choice button
    ///  - The Duplicate button
    ///  - The Remove Choice button
    ///  - One of the Choice panels
    ///  - One of the children of the Choice panels
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public event System.EventHandler ChoiceEvent;
    private void Choice_Event(object sender, EventArgs e)
    {
      try
      {
        if (ChoiceEvent != null)
          ChoiceEvent(sender, new EventArgs());
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("Error: " + e.GetType().ToString(), "frmPoll.Choice_Event");
      }
    }



    // Normal entrance into this method.  Simplifies calling externally.
    public void UpdatePreview(Poll model, int qIdx, AnswerFormat aFmt)
    {
      UpdatePreview(model, qIdx, aFmt, panelPreview, 0, labelPreview.Left + 2);
    }

    /// <summary>
    /// Updates the Preview Pane.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="qIdx"></param>
    /// <param name="answerFormat"></param>
    /// <param name="panel"></param>
    /// <param name="topMargin"></param>
    /// <param name="leftMargin"></param>
    private void UpdatePreview(Poll model, int qIdx, AnswerFormat answerFormat, Panel panel, int topMargin, int leftMargin)
    {
      // This initial check had to be added for the unique case when 'HideQuestionNumbers' is set to True
      // and a new poll is started.  This method inadvertently gets called and without any questions present
      // an exception is raised.
      if (model.Questions.Count == 0)
        return;

      try
      {
        int gap = 15;
        int width = panel.Width;
        int availWidth = width - leftMargin * 2;

        int tabIndex = -1;
        int selIndex = -1;
      
        Font font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);   // Default font throughout this method

        panel.Controls.Clear();
        panel.AutoScroll = false;
        bool PreviewAutoScrollActivated = false;

        _Question quest = model.Questions[qIdx];


        // Note: This special module-level variable had to be introduced because the ASM checkbox on the Settings page
        //       doesn't change the property until AFTER this method is called and thus is out of sync.  Introducing 
        //       this variable resolved that problem.
        if (! HideQuestionNumbers)
        {
          // Display question number
          Font fontLarge = new Font("Arial", 12, FontStyle.Bold);
          topMargin += gap + PanelControls.AddLabel("Question #" + (CurrentQuestion + 1).ToString(), fontLarge, Color.DarkBlue, topMargin, leftMargin, ContentAlignment.MiddleLeft, anchorTL, panel).Height;
        }

        // Display question at top of panel
        string txt = quest.Text;

        if ((txt == "") | (txt == DefaultQuestionText))
          topMargin += gap;
        else
          topMargin += gap + PanelControls.AddMultilineLabel(txt, font, topMargin, leftMargin, -1, (int) (width * 0.9), anchorTL, panel).Height;
      
        RadioButton radBut;

        bool exitLoop = false;
        AnswerFormat aFormat = answerFormat;


        // Add one or more choices within loop
        foreach (_Choice choice in quest.Choices)
        {
          switch (aFormat)
          {
            case AnswerFormat.Standard:    // Standard vertical radio buttons
              if (choice.Text != "")
              {           
                tabIndex++;
                selIndex++;
                radBut = PanelControls.AddRadioButton(choice.Text, font, topMargin, leftMargin + gap, availWidth - (leftMargin + gap), anchorTL, panel, tabIndex, selIndex);
                topMargin += radBut.Height;

                // See if this radio button was previously selected
                if (selIndex == PreviewInfo.SelectedIndex)
                {
                  radBut.Checked = true;   // If so, then set it like it was previously

                  // And if its ExtraInfo flag is set true then provide a textbox for "Other" type input
                  if (choice.ExtraInfo)
                  {
                    topMargin += (int) (gap / 2);
                    tabIndex++;
                    string introText = Tools.EnsureSuffix(choice.MoreText, ":");
                    string otherText = Tools.GetExtraTextValue(PreviewInfo.OtherText, PreviewInfo.SelectedIndex);
                    int numlines = choice.ExtraInfoMultiline ? 5 : 1;
                    int txtBoxHgt = PanelControls.AddTextBox(otherText, font, topMargin, leftMargin + 40, numlines, (int) (panel.Width * 0.35), anchorTL, introText, panel, tabIndex, selIndex);
                    topMargin += txtBoxHgt;
                  }
                }

                topMargin += gap;
              }
              else
                // If this Choice.Text item is present but blank then provide the equivalent spacing to one line
                topMargin += (int) (font.Size * 1.625) + gap;   
            
              break;


            case AnswerFormat.List:
              tabIndex++;

              // We're going to pass all of the choices at once to the listbox creation routine
              ListBox lstBox = PanelControls.AddListBox(quest.Choices, font, topMargin, leftMargin + gap, anchorTLR, panel, tabIndex);
            
              if (lstBox != null)
              {
                int idx = PreviewInfo.SelectedIndex;

                if (idx != -1)
                {
                  lstBox.SelectedIndex = idx;

                  if (quest.Choices[idx].ExtraInfo)
                  {
                    topMargin += lstBox.Height + gap;
                    tabIndex++;
                    string introText = Tools.EnsureSuffix(quest.Choices[idx].MoreText, ":");
                    string otherText = Tools.GetExtraTextValue(PreviewInfo.OtherText, PreviewInfo.SelectedIndex);
                    int numlines = choice.ExtraInfoMultiline ? 5 : 1;
                    int txtBoxHgt = PanelControls.AddTextBox(otherText, font, topMargin, leftMargin + gap, numlines, (int) (panel.Width * 0.35), anchorTL, introText , panel, tabIndex, idx);
                    topMargin += txtBoxHgt;  // Is this line necessary???
                  }
                }
              }

              exitLoop = true;   // We only want one dropdown listbox, not multiple ones
              break;


            case AnswerFormat.DropList:
              tabIndex++;

              // We're going to pass all of the choices at once to the combobox creation routine
              ComboBox cboBox = PanelControls.AddComboBox(quest.Choices, font, topMargin, leftMargin + gap, anchorTLR, panel, tabIndex);
            
              if (cboBox != null)
              {
                int idx = PreviewInfo.SelectedIndex;

                if (idx != -1)
                {
                  cboBox.SelectedIndex = idx + 1;

                  if (quest.Choices[idx].ExtraInfo)
                  {
                    topMargin += cboBox.Height + gap;
                    tabIndex++;
                    string otherText = Tools.GetExtraTextValue(PreviewInfo.OtherText, PreviewInfo.SelectedIndex);
                    int numlines = choice.ExtraInfoMultiline ? 5 : 1;
                    int txtBoxHgt = PanelControls.AddTextBox(otherText, font, topMargin, leftMargin + gap, numlines, (int) (panel.Width * 0.35), anchorTL, Tools.EnsureSuffix(quest.Choices[idx].MoreText, ":"), panel, tabIndex, idx);
                    topMargin += txtBoxHgt;
                  }
                }
              }

              exitLoop = true;   // We only want one dropdown listbox, not multiple ones
              break;
          
          
            case AnswerFormat.MultipleChoice:
              if (choice.Text != "")
              {
                tabIndex++;
                selIndex++;
                CheckBoxPP chkBox = PanelControls.AddCheckBox(choice.Text, font, topMargin, leftMargin + gap, availWidth - (leftMargin + gap), anchorTL, panel, tabIndex, selIndex);
                topMargin += chkBox.Height;

                // See if this checkbox was previously selected
                if (PreviewInfo.CheckBoxItems.ContainsKey(selIndex))
                {
                  chkBox.Checked = true;  // If so, then set it like it was previously

                  // And if its ExtraInfo flag is set true then provide a textbox for "Other" type input
                  if (choice.ExtraInfo)
                  {
                    topMargin += (int) (gap / 2);
                    tabIndex++;
                    //string prevText = (PreviewInfo.CheckBoxItems[selIndex] as PreviewInfo).OtherText;
                    string prevText = Tools.GetExtraTextValue(PreviewInfo.OtherText, selIndex);


                    string introText = Tools.EnsureSuffix(choice.MoreText, ":");
                    int numlines = choice.ExtraInfoMultiline ? 5 : 1;
                    int txtBoxHgt = PanelControls.AddTextBox(prevText, font, topMargin, leftMargin + 40, numlines, (int) (panel.Width * 0.35), anchorTL, introText, panel, tabIndex, selIndex);
                    topMargin += txtBoxHgt;
                  }
                }

                topMargin += gap;
              }
              else
                // If this Choice.Text item is present but blank then provide the equivalent spacing to one line
                topMargin += (int) (font.Size * 1.625) + gap;   
            
              break;


            case AnswerFormat.Range:
              if (choice.Text != "")
              {
                tabIndex++;
                selIndex++;

                int radLeft = leftMargin + gap * 2;
                radBut = PanelControls.AddRadioButton(choice.Text, font, topMargin, radLeft, availWidth - radLeft, anchorTL, panel, tabIndex, selIndex);

                // See if this radio button was previously selected
                if (selIndex == PreviewInfo.SelectedIndex)
                  radBut.Checked = true;  // If so, then set it like it was previously

                PanelControls.AddLabel(choice.MoreText, font, topMargin + 1, radLeft - 5, ContentAlignment.MiddleRight, anchorTL, panel);
                topMargin += radBut.Height + gap;
              }
              else
                topMargin += (int) (font.Size * 1.625) + gap;   // Equivalent spacing to one line
            
              break;


            case AnswerFormat.MultipleBoxes:
              if (choice.Text != "")
              {           
                tabIndex++;
                selIndex++;

                string prevtxt2 = "";
                
                if (PreviewInfo.CheckBoxItems.Count > 0 && selIndex < PreviewInfo.CheckBoxItems.Count)
                  prevtxt2 = (PreviewInfo.CheckBoxItems[selIndex] as PreviewInfo).OtherText;
                
                int txtBoxWid = 40;
                int labelLeft = leftMargin + gap + txtBoxWid + 5;
                Label label = PanelControls.AddMultilineLabel(choice.Text, font, topMargin + 1, labelLeft, -1, availWidth - labelLeft, anchorTL, panel);
                int txtBoxTop = topMargin + (label.Height - font.Height * 2) / 2;
                int txtBoxHgt = PanelControls.AddTextBox(prevtxt2, font, txtBoxTop, leftMargin + gap, 1, txtBoxWid, anchorTL, "", panel, tabIndex, selIndex);

                topMargin += Math.Max(txtBoxHgt, label.Height) + gap;
              }
              else
                // If this Choice.Text item is present but blank then provide the equivalent spacing to one line
                topMargin += (int) (font.Size * 1.625) + gap;   
            
              break;


            case AnswerFormat.Freeform:
              tabIndex++;
              selIndex++;
              PanelControls.AddTextBox(PreviewInfo.Freeform, font, topMargin, leftMargin + gap, 5, (int) (panel.Width * 0.6), anchorTLR, "", panel, tabIndex, selIndex);

              exitLoop = true;   // We only want one textbox, not multiple ones
              break;


            case AnswerFormat.Spinner:
              tabIndex++;
              selIndex++;  // Note: Currently not used in practice but including it will allow for future expansion when more than 1 spinner sits on a panel
              PanelControls.AddSpinner(PreviewInfo.Spinner, choice.Text, choice.MoreText, choice.MoreText2, font, topMargin, leftMargin + 3 * gap, anchorTL, panel, tabIndex, selIndex);

              exitLoop = true;   // We [currently] only need one spinner, not multiple ones
              break;


            default:
              Debug.WriteLine("Unknown AnswerFormat encountered: " + aFormat.ToString(), "frmPoll.UpdatePreview");
              break;
          }

          if (exitLoop == true)
            break;
        }

        // Add an extra space to provide a bit of a bottom margin
        topMargin += gap;
        panel.AutoScrollMinSize = new Size(AutoScrollMinSize.Width, topMargin);

        // Let's check if we need to turn on AutoScrolling
        if (topMargin > panel.Height)
        {
          if (PreviewAutoScrollActivated == false)
          {
            panel.AutoScroll = true;
            PreviewAutoScrollActivated = true;
          }
        }
      }

      catch (Exception ex)
      {
        Tools.ShowMessage(ex.Message, "Error in frmPoll.UpdatePreview");
      }
    }

    #endregion


    #region Instructions_TabPage

    /// <remarks>
    /// 
    /// Instructions page
    /// 
    /// </remarks>
    private void tabPageInstructions_Resize(object sender, System.EventArgs e)
    {
      ResizeInstructionsPanels();
    }

    private void ResizeInstructionsPanels()
    {
      Size availSize = tabPageInstructions.ClientSize;
      availSize = new Size(availSize.Width, availSize.Height - panelInstructionsTop.Height);

      if (checkBoxPersonalInfo.Checked)
      {
        panelAfterAllPolls.Height = availSize.Height / 5;

        availSize = new Size(availSize.Width, availSize.Height - panelAfterAllPolls.Height);

        panelBeforePoll.Height = availSize.Height / 2;
        panelBeforePoll.Width  = availSize.Width / 2;

        panelAfterPoll.Left = panelBeforePoll.Right;
        panelAfterPoll.Height = availSize.Height / 2;
        panelAfterPoll.Width  = availSize.Width / 2;

        panelBeginMessage.Visible = true;
        panelBeginMessage.Top = panelBeforePoll.Bottom;
        panelBeginMessage.Height = availSize.Height / 2;
        panelBeginMessage.Width  = availSize.Width / 2;

        panelEndMessage.Visible = true;
        panelEndMessage.Location = panelBeforePoll.Location + panelBeforePoll.Size;
        panelEndMessage.Height = availSize.Height / 2;
        panelEndMessage.Width  = availSize.Width / 2;
      }
      else
      {
        panelBeginMessage.Visible = false;
        panelEndMessage.Visible = false;

        panelAfterAllPolls.Height = availSize.Height / 2;

        panelBeforePoll.Height = availSize.Height / 2;
        panelBeforePoll.Width  = availSize.Width / 2;

        panelAfterPoll.Left = panelBeforePoll.Right;
        panelAfterPoll.Height = availSize.Height / 2;
        panelAfterPoll.Width  = availSize.Width / 2;
      }
    }

    #endregion


    #region Settings_TabPage
    /// <remarks>
    /// 
    /// Settings page
    /// 
    /// </remarks>


    /// <summary>
    /// The setting of this value controls the visibility of panelRespondentInfo on the Responses page.
    /// It also controls the visibility of 'panelBeginMessage' and 'panelEndMessage' on the Instructions page.
    /// We're implementing this special event handler to provide the instantaneous change required.
    /// </summary>
    private void checkBoxPersonalInfo_CheckedChanged(object sender, System.EventArgs e)
    {
      CheckBox checkBox = sender as CheckBox;
      panelRespondentInfo.Visible = checkBox.Checked;
      ResizeInstructionsPanels();
    }

    #endregion

    #endregion



    public event System.EventHandler FormControlEvent;
    /// <summary>
    /// This event handler is called by all controls activated with ASM compliant tags.
    /// </summary>
    private void GeneralControl_Event(object sender, EventArgs e)
    {
      try
      {
        if (FormControlEvent != null)
          FormControlEvent(sender, new EventArgs());
      }
      catch (Exception ex)
      {
        // Handle exceptions
        Debug.Fail("Error: " + ex.Message, "frmPoll.GeneralControl_Event");
      }
    }


    public event EventHandler TextBoxChangedEvent;
    /// <summary>
    /// Once a poll has been published, we need to immediately inform the user if the
    /// text in a textbox is altered, as it could have mild to major repercussions.
    /// </summary>
    private void TextBoxChanged_Event(object sender, EventArgs e)
    {
      try
      {
        if (TextBoxChangedEvent != null)
          TextBoxChangedEvent(sender, new EventArgs());
      }
      catch (Exception ex)
      {
        // Handle exceptions
        Debug.Fail("Error: " + ex.Message, "frmPoll.TextBoxChanged_Event");
      }
    }




    /// <summary>
    /// Usually ASM works automatically.  But if the user is doing something like editing a textbox and then
    /// clicks on the upper-right 'X' to close the form, the ASM event will not immediately be fired.  This
    /// causes two problems
    ///  1. If the user opts to save the poll then this latest change won't be included.
    ///  2. pollModel is set to null in the Controller's PollFormClosed method.  This causes
    ///     an error when the ASM event is eventually fired.
    /// 
    /// This method, which is called from the form's 'Closing' event, looks on the current tab for another
    /// control to temporarily receive focus.  Forcing this to happen causes the ASM GeneralControl_Event above
    /// to be immediately fired.  And if there is no ASM event to be fired, it doesn't do any harm.
    /// </summary>
    public void ForceASMEvent()
    {
      foreach (Control control in tabNewPoll.SelectedTab.Controls)
      {
        if (control.CanFocus && ! control.ContainsFocus)
        {
          control.Focus();   // This is just an arbitrary control that isn't the control with the current focus
          break;
        }
      }

      this.Focus();          // Set focus more generally to form itself (probably not necessary but doesn't hurt)
    }

    
    /// <summary>
    /// Setup the event handlers for a "smart" control - ie. One that has valid data in its Tag property
    /// that establishes a link with the corresponding Property in the connected Model.
    /// 
    /// Example: The 'checkCanPrint' control may have this Tag value: "Property=PollsterPrivileges.CanPrint".  The presence
    ///          of this Tag value set sets up automated synchronization with the 'CanPrint' property in the Model.
    /// </summary>
    /// <param name="ctrl"></param>
    public void EstablishASMEventHandlers(Control ctrl)
    {
      // Note: The events we've chosen to provide delegates for in the switch construct below are the
      //       default events for the controls we're examining.  Time will tell if they are sufficient.

      string ctrlType = Tools.ObjectType(ctrl);

      switch (ctrlType)
      {
        case "CheckBox":
          (ctrl as CheckBox).CheckedChanged += new System.EventHandler(this.GeneralControl_Event);
          break;

        case "Label":
          // Note: I don't think this event will ever occur but good to have here for completeness
          (ctrl as Label).TextChanged += new System.EventHandler(this.GeneralControl_Event);
          break;

        case "ComboBox":
          (ctrl as ComboBox).SelectedIndexChanged += new System.EventHandler(this.GeneralControl_Event);
          break;

        case "ListBox":
          (ctrl as ListBox).SelectedIndexChanged += new System.EventHandler(this.GeneralControl_Event);
          break;

        case "RadioButton":
        case "QuestionButton":
        case "AnswerButton":
          (ctrl as RadioButton).CheckedChanged += new System.EventHandler(this.GeneralControl_Event);
          break;
        
        case "TextBox":
          (ctrl as TextBox).LostFocus += new System.EventHandler(this.GeneralControl_Event);
          (ctrl as TextBox).TextChanged += new System.EventHandler(this.TextBoxChanged_Event);
          break;

        default:
          Debug.Fail("Unknown object type: " + ctrlType, "frmPoll.EstablishASMEventHandlers");
          break;
      }        
    }


    #region NumericEntryChecking
    // The code in this region is implemented when textbox entry should only be numeric.

    // Boolean flag used to determine when a character other than a number is entered.
    private bool nonNumberEntered = false;

    // Handle the KeyDown event to determine the type of character entered into the control.
    private void textBox_KeyDown(object sender, KeyEventArgs e)
    {
      // Initialize the flag to false.
      nonNumberEntered = false;

      // Determine whether the keystroke is a number from the top of the keyboard.
      if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
      {
        // Determine whether the keystroke is a number from the keypad.
        if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
        {
          // Determine whether the keystroke is a backspace.
          if(e.KeyCode != Keys.Back)
          {
            // A non-numerical keystroke was pressed.
            // Set the flag to true and evaluate in KeyPress event.
            nonNumberEntered = true;
          }
        }
      }
    }

    // This event occurs after the KeyDown event and can be used to prevent
    // non-numeric characters from entering the control.
    private void textBox_KeyPress(object sender, KeyPressEventArgs e)
    {
      // Check for the flag being set in the KeyDown event
      if (nonNumberEntered == true)
      {
        // Stop the character from being entered into the control since it is non-numeric
        e.Handled = true;

        // And give an audible cue that it is invalid
        Tools.Beep(150, 200);
      }
    }

    #endregion


    #region ReadOnlyTextBoxes
    // To make a textbox fully readonly, 4 things need to be done:
    //   1. Set its ReadOnly property to true
    //   2. Change its BackColor to something more visually pleasing than the effect provided by 'Window'
    //   3. Set its KeyPress event to point to the event handler below
    //   4. Set its MouseDown event to point to the event handler further below

    /// <summary>
    /// A generic KeyPress event handler that simply serves to make the textbox read-only.
    /// </summary>
    private void textBoxReadOnly_KeyPress(object sender, KeyPressEventArgs e)
    {
      e.Handled = true;       // This will cancel all characters entered into the textbox, thus effectively making it read-only
      Tools.Beep(150, 200);   // And give an audible cue that it is invalid
    }

    private void textBoxReadOnly_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        TextBox textBox = sender as TextBox;
        if (textBox.ReadOnly)
        {
          ContextMenu menu = new ContextMenu();
          menu.MenuItems.Add("Copy", new EventHandler(textBoxReadOnly_MenuCopy));
          menu.MenuItems.Add("Select All", new EventHandler(textBoxReadOnly_SelectAll));
          textBox.ContextMenu = menu;
        }
      }
    }

    private void textBoxReadOnly_MenuCopy(object sender, EventArgs e)
    {
      MenuItem menuItem = sender as MenuItem;
      ContextMenu menu = menuItem.Parent as ContextMenu;
      TextBox textBox = menu.SourceControl as TextBox;
      textBox.Copy();
    }

    private void textBoxReadOnly_SelectAll(object sender, EventArgs e)
    {
      MenuItem menuItem = sender as MenuItem;
      ContextMenu menu = menuItem.Parent as ContextMenu;
      TextBox textBox = menu.SourceControl as TextBox;
      textBox.SelectAll();
    }

    #endregion


    #region General_DragDrop
    
    // Initiate a dedicated timer.  If the user holds down the mouse long enough then we'll assume he wants to drag & drop.
    private void GeneralControl_MouseDown(object sender, MouseEventArgs e)
    {
      DragDropInfo = new DragDropData();         // Instantiate module-level object
      DragDropInfo.dragObject = sender;                             
      
      // Determine what 'offsetY' should be (varies depending on location and layout of parent panel)
      switch(Tools.ObjectType(sender))
      {
        case "QuestionButton":
          //DragDropInfo.offsetY = (this.Top + this.Height - this.ClientRectangle.Height) + this.tabPageQuestions.Top + this.tabNewPoll.ItemSize.Height + this.panelQuestionList.Top - 4;
          DragDropInfo.offsetY = panelQuestionList1.PointToScreen(panelQuestionList1.Location).Y;
          break;

        case "PanelChoice":
          //DragDropInfo.offsetY = (this.Top + this.Height - this.ClientRectangle.Height) + this.tabPageQuestions.Top + this.tabNewPoll.ItemSize.Height + this.panelChoices.Top - 4;
          
          // Note: I don't know why PanelChoices itself doesn't work for this calculation but the Y-offset if completely off
          DragDropInfo.offsetY = panelChoices.Controls[0].PointToScreen(panelChoices.Controls[0].Location).Y - 8;  // Not sure why the -8 is necessary
          break;

        default:
          Debug.Fail("Unaccounted for object type: " + Tools.ObjectType(sender), "frmPoll.DragDropInfo.Initialize");
          break;
      }
      
      DragDropTimer.Interval = DragDropInfo.TimerInterval;
      DragDropTimer.Enabled = true;
    }

    // ... Otherwise if he lets go quickly then we'll assume he just wants to click the question button instead.
    private void GeneralControl_MouseUp(object sender, MouseEventArgs e)
    {
      try
      {
        DragDropTimer.Enabled = false;
        DragDropInfo.dragObject = null;    // Just cleaning up
      }
      catch
      {
        // Do nothing - just catches an error that was observed during testing
      }
    }

    // Initiates drag & drop visual effects
    private void DragDropTimer_Tick(object sender, EventArgs e)
    {
      DragDropTimer.Enabled = false;
      DragDropInfo.BuildGapList();
      Control ctrl = DragDropInfo.dragObject as Control;
      ctrl.DoDragDrop(ctrl, DragDropEffects.All);
    }

    private void GeneralPanel_DragEnter(object sender, DragEventArgs e)
    {
      // Note: Typically we would get info from 'sender' but we already have it stored so we don't need to

      // Old Code
//      if (e.Data.GetDataPresent(DragDropInfo.dragObject.GetType()))
//        e.Effect = DragDropEffects.Move;

    }

    private void GeneralPanel_DragOver(object sender, DragEventArgs e)
    {
      // New Code


      // Determine whether file data exists in the drop data. If not, then the drop effect reflects that the drop cannot occur.
      if (!e.Data.GetDataPresent(DragDropInfo.dragObject.GetType())) 
      {
        e.Effect = DragDropEffects.None;
        return;
      }

      // Set the effect based upon the KeyState
      if ((e.KeyState & SHIFT) == SHIFT && (e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
      {
        e.Effect = DragDropEffects.Move;
      } 
      else if ((e.KeyState & CTRL) == CTRL && (e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
      {
        e.Effect = DragDropEffects.Copy;
      } 
      else if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)  
      {
        e.Effect = DragDropEffects.Move;
      } 
      else
      {
        e.Effect = DragDropEffects.None;
      }

      
      Panel panel = sender as Panel;

      int sepY = DragDropInfo.FindClosestGap(e.Y);    // Find the gap closest to 'e.Y' and return the gap's Y-value

      // See if we have to erase the previous separator line
      int lastY = DragDropInfo.GetMostRecentLastY();

      if (sepY != lastY)
      {
        if (lastY != -1)
          EraseSeparatorLine(panel, lastY);                  // Erase previous separator line

        DragDropInfo.RemoveLastY(lastY);
        DragDropInfo.AddLastY(sepY);                         // Put new Y-value onto stack
        DrawSeparatorLine(panel, sepY, Color.Black);         // And draw new separator line
      }
    }


    private void GeneralPanel_DragLeave(object sender, System.EventArgs e)
    {
      EraseSeparatorLine((sender as Panel), DragDropInfo.GetMostRecentLastY());      // Erase final separator line
    }


    public event DragDropEventHandler DragDropEvent;

    private void GeneralPanel_DragDrop(object sender, DragEventArgs e)
    {
      EraseSeparatorLine((sender as Panel), DragDropInfo.GetMostRecentLastY());      // Erase final separator line
      
      // QuestionButton droppedObj = (QuestionButton) e.Data.GetData(typeof(QuestionButton));
      // We already know what the dropped object will be, because it is stored here: 'DragDropInfo.obj'

      int N = DragDropInfo.DetermineOrdinalPosition();

      if (N != -1)  // Note: Can be -1 if dragging occurs too quickly and then 'lastYlist' is never populated
      {
        try
        {
          if (DragDropEvent != null)
            DragDropEvent(N, DragDropInfo.dragObject, e);   // Fire event
        }
        catch
        {
          // Handle exceptions
          Debug.Fail("Error: " + e.GetType().ToString(), "frmPoll.GeneralPanel_DragDrop");
        }
      }
    }


    /// <summary>
    /// Draws a line of the specified color at the specified y-coord on the panel.
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    private void DrawSeparatorLine(Panel panel, int y, Color color)
    {
      Graphics g = panel.CreateGraphics();
      Pen myPen = new Pen(color, 1);
      g.DrawLine(myPen, 2, y, panel.ClientSize.Width - 2 , y);               // Leave 2-pixel gap on left & right
      g.Dispose();
    }


    /// <summary>
    /// Erases a previously drawn line at the specified y-coord on the panel.
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="y"></param>
    private void EraseSeparatorLine(Panel panel, int y)
    {
      Rectangle rect = new Rectangle(2, y, panel.ClientSize.Width - 2, 1);
      panel.Invalidate(rect, false);
    }


    internal class DragDropData
    {
      internal object dragObject;
      internal int TimerInterval;
      //      internal int lastX;      // Possible future use
      //      internal int offsetX;    // Possible future use
      internal ArrayList lastYlist;
      internal int offsetY;
      internal string gapList;

      internal DragDropData()
      {
        TimerInterval = 300;             // May need to change this delay time (different for different users?)
        lastYlist = new ArrayList();
      }
      
      // Constructs a CDF string of the Y-values of where the gaps between the objects on the parent panel are located.
      internal void BuildGapList()
      {
        Panel panel = (dragObject as Control).Parent as Panel;

        gapList = "";
        //        int bottomY = 0;
        //
        //        foreach (Control ctrl in panel.Controls)
        //        {
        //          // The introduction of this check is necessary to allow other types of controls, such as labels,
        //          // to exist within the same panel.
        //          if (ctrl.GetType() == dragObject.GetType())  
        //          {
        //            gapList = gapList + ((ctrl.Top + bottomY) / 2 - 1).ToString() + ",";
        //            bottomY = ctrl.Bottom;
        //          }
        //        }
        //
        //        // Still need to add the Y-value for just below the lowermost control.  We'll locate this value exactly
        //        // the same distance beyond the last control that the first location is before the first control.
        //        int gap = Convert.ToInt32(gapList.Substring(0, gapList.IndexOf(",")));
        //        gapList = gapList + (bottomY + gap).ToString();

        int halfGap = 0;  // Initialize
        Control prevCtrl = null;

        foreach (Control ctrl in panel.Controls)
        {
          // The introduction of this check is necessary to allow other types of controls, such as labels,
          // to exist within the same panel.
          if (ctrl.GetType() == dragObject.GetType())  
          {
            if (prevCtrl == null)
              prevCtrl = ctrl;
            else
            {
              if (gapList == "")
              {
                halfGap = (ctrl.Top - prevCtrl.Bottom) / 2;
                gapList = (prevCtrl.Top - halfGap).ToString() + ",";   // Estimate the location above the first control
              }

              gapList += ((prevCtrl.Bottom + ctrl.Top) / 2).ToString() + ",";
              prevCtrl = ctrl;
            }
          }
        }

        // Still need to add the Y-value for just below the lowermost control.  We'll locate this value exactly
        // the same distance beyond the last control that the first location is before the first control.
        gapList += (prevCtrl.Bottom + halfGap).ToString();
      }


      // Finds the gap closest to the mouse's current Y location and return the gap's Y-value.
      internal int FindClosestGap(int mouseY)
      {
        int currY = mouseY - offsetY;
        
        // Cycle through the list of gap locations and find the one closes to 'currY'
        string[] gapListArray = gapList.Split(new char[] {','});
        int closestDist = 9999;   // Ridiculously large number
        int closestGap = -1;

        foreach (string sGap in gapListArray)
        {
          int gap = Convert.ToInt32(sGap);
          int dist = Math.Abs(currY - gap);
          if (dist < closestDist)
          {
            closestDist = dist;
            closestGap = gap;
          }
          else
          {
            break;  // If the distance grows then we've found the gap location closest to the mouse cursor
          }
        }

        if (closestGap == -1)
          Debug.Fail("Error in finding closest gap", "frmPoll.DragDropData.FindClosestGap");

        return closestGap;
      }


      // Calculates the new array position (ie. 0, 1, 2, 3, ...) based on the value of the 'Y' parameter
      internal int DetermineOrdinalPosition()
      {
        int N = -1;
        string sLastY = GetMostRecentLastY().ToString();

        if (sLastY != "-1")
        {
          string[] gapListArray = gapList.Split(new char[] {','});

          for (int i = 0; i < gapListArray.Length; i++)
          {
            if (gapListArray[i] == sLastY)
            {
              N = i;
              break;
            }
          }
        }

        return N;
      }


      internal int GetMostRecentLastY()
      {
        if (lastYlist.Count > 0)
          return Convert.ToInt32(lastYlist[lastYlist.Count - 1]);
        else
          return -1;
      }


      internal void AddLastY(int Y)
      {
        lastYlist.Add(Y);
      }


      internal void RemoveLastY(int Y)
      {
        if (lastYlist.Count > 0)
        {
          int index = lastYlist.IndexOf(Y);

          if (index != -1)
            lastYlist.RemoveAt(index);
        }
      }

    }

    #endregion


    #region InternalClasses

    /// <summary>
    /// Contains assorted values associated with animating the "opening" and "closing" of the RespondentInfo panel.
    /// </summary>
    internal class RIpanelAnimationInfo
    {
      internal int MaxHeight;
      internal int Increment;
    }


    /// <summary>
    /// This class is used to hold information about separate models used by the Summary and Responses tab pages.
    /// For historical reasons, it is not used by the Q&A tab page.
    /// Because the user can apply separate filters to each, we need to maintain separate data models.
    /// </summary>
    internal class FilteredTabPageInfo
    {
      internal TabPage tabPage;                                // The Tab Page referred to by the object
      internal DataObjects.PanelGradient respButPanel;         // The Responses Button panel, if present
      internal DataObjects.PanelGradient questButPanel;        // The Questions Button panel, if present

      internal Poll Model;                                     // Simple reference to original Poll
      internal ArrayList SeqCriteria = new ArrayList();
      internal int SeqSelection = 0;                           // 0 = ALL
      internal ArrayList DateCriteria = new ArrayList();
      internal ArrayList DateSelections = new ArrayList();

      // Current set of Respondents being shown
      private _Respondents respondents;
      internal _Respondents Respondents
      {
        get
        {
          return respondents;
        }
        set
        {
          respondents = value;

          if (currRespondent >= respondents.Count)   // Reset when necessary
            currRespondent = 0;
        }
      }


      internal event EventHandler QuestionChangedEvent;

      // The current question on the Responses page (currently not used on the Summary page)
      private int currQuestion = -1;
      internal int CurrQuestion
      {
        get
        {
          return currQuestion;
        }
        set
        {
          currQuestion = value;

          if (QuestionChangedEvent != null)
            QuestionChangedEvent(this, new EventArgs());
        }
      }


      internal event EventHandler RespondentChangedEvent;

      // The current respondent on the Summary or Responses page
      private int currRespondent = -1;
      internal int CurrRespondent
      {
        get
        {
          return currRespondent;
        }
        set
        {
          currRespondent = value;

          if (RespondentChangedEvent != null)
            RespondentChangedEvent(this, new EventArgs());
        }
      }
    }

    #endregion


   
    public delegate void CurrRespondentEventHandler (int currRespondent, EventArgs e);
    public event CurrRespondentEventHandler RespondentChangedEvent;

    /// <summary>
    /// Simply provides a notification mechanism for the 'CurrentRespondent' property in the Controller.
    /// </summary>
    private void CurrRespondentChanged(object sender, EventArgs e)
    {
      FilteredTabPageInfo pageInfo = (FilteredTabPageInfo) sender;
      
      if (RespondentChangedEvent != null)
        RespondentChangedEvent(pageInfo.CurrRespondent, new EventArgs());
    }



    public event EventHandler HideQuestionNumbersChangedEvent;

    /// <summary>
    /// Simply provides a notification mechanism for the Controller that the 'HideQuestionNumbers' checkbox has changed.
    /// </summary>
    private void checkBoxHideQuestionNumbers_CheckedChanged(object sender, EventArgs e)
    {
      HideQuestionNumbers = (sender as CheckBox).Checked;

      if (HideQuestionNumbersChangedEvent != null)
        HideQuestionNumbersChangedEvent(sender, new EventArgs());
    }


    public event EventHandler PurgeDurationChangedEvent;

    /// <summary>
    /// PurgeDuration is a special case that ASM can't currently handle.  The reason is because the values of each
    /// Enum item are not consecutive like a regular Enum.  Thus the Enum values don't correspond to the ComboBox
    /// index values and so everything gets messed up.  Thus we need to handle the event in a custom way.
    /// </summary>
    private void comboPurgeDuration_SelectedValueChanged(object sender, System.EventArgs e)
    {
      ComboBox comboBox = sender as ComboBox;
    
      if (PurgeDurationChangedEvent != null)
        PurgeDurationChangedEvent(sender, new EventArgs());
    }


    /// <summary>
    /// Called by the Controller and presets 'comboPurgeDuration' according to the stored value.
    /// </summary>
    /// <param name="prevValue"></param>
    internal void SetComboPurgeDuration(int prevValue)
    {
      int idx = 0;
      foreach (int val in Enum.GetValues(typeof(PurgeDuration)))
      {      
        if (val == prevValue)
        {
          comboPurgeDuration.SelectedIndex = idx;
          break;
        }
        else
          idx++;
      }
    }

    private void textBoxSummaryPollSummary_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      string msg = "This textbox is for display purposes only. If you wish to alter\nthe contents of the Poll Summary, please go to the Settings page.";
      Tools.ShowMessage(msg, SysInfo.Data.Admin.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }


    private void checkBoxExportEnabled_CheckedChanged(object sender, System.EventArgs e)
    {
      bool checkState = (sender as CheckBox).Checked;

      labelExportFilename.Enabled = checkState;
      textBoxExportFilename.Enabled = checkState;
      buttonExportFilenameSelect.Enabled = checkState;

      if (checkState && textBoxExportFilename.Text == "")
      {
        textBoxExportFilename.Focus();
        textBoxExportFilename.Text = SysInfo.Data.Paths.Data + Tools.StripPathAndExtension(PollName) + ".mdb";
        this.Focus();     // Forces ASM to be updated
      }
    }


    private void buttonExportFilenameSelect_Click(object sender, System.EventArgs e)
    {
      bool confirm = false;
      string filename = Tools.SelectExportFilename(textBoxExportFilename.Text, out confirm);
      if (confirm)
      {
        textBoxExportFilename.Focus();
        textBoxExportFilename.Text = filename;
        this.Focus();     // Forces ASM to be updated
      }
    }


    /// <summary>
    /// Draws the charts on the Summary page; 1 chart per question
    /// </summary>
    /// <param name="model"></param>
    private void DrawSummaryCharts(Poll model)
    {
      panelSummaryCharts.Controls.Clear();
      
      int width = panelSummaryCharts.Width / 2 - 4;   // The last value is to account for the width of the scrollbar
      int height = width * 3 / 4;
      int left = 0;
      int top = 0;
      int qNum = 0;

      foreach (_Question question in model.Questions)
      {
        ChartPanel chartPanel = new ChartPanel();
        chartPanel.PollModel = model;
        chartPanel.QuestionNum = qNum;
        chartPanel.QuestionText = question.Text;
        chartPanel.GraphInfoEvent += new EventHandler(chartPanel_GraphInfoEvent);
        chartPanel.Size = new Size(width, height);
        //chartPanel.Activate();

        int remain;
        Math.DivRem(qNum, 2, out remain);
        
        left = (remain == 0) ? 0 : width;
        chartPanel.Location = new Point(left, top);

        panelSummaryCharts.Controls.Add(chartPanel);
        qNum++;

        if (remain == 1)
        {
          top += height;

          if (top > panelSummaryCharts.Height)
            panelSummaryCharts.AutoScrollMinSize = new Size(panelSummaryCharts.Width, top + chartPanel.Height);
        }
      }

    }


    public event EventHandler GraphInfoEvent;
    public void chartPanel_GraphInfoEvent(object sender, System.EventArgs e)
    {
      if (GraphInfoEvent != null)
        GraphInfoEvent(this, new EventArgs());
    }


    private void panelSummaryCharts_Resize(object sender, System.EventArgs e)
    {
      // Before resizing, first ensure that all of the ChartPanels are deactivated.
      // They'll later be reactivated in the timer tick event.
      foreach(Control ctrl in panelSummaryCharts.Controls)
      {
        ChartPanel chartPanel = (ChartPanel) ctrl;

        if (chartPanel != null)
        {
          if (chartPanel.Activated)
            chartPanel.Deactivate();
        }
      }

      Panel parentPanel = sender as Panel;
      int width = parentPanel.ClientSize.Width / 2 - 4;   // The last value is to account for the width of the scrollbar

      foreach (Control ctrl in parentPanel.Controls)
      {
        ChartPanel chart = ctrl as ChartPanel;

        if (chart != null)
        {
          if (chart.Left < 20)
            chart.Width = width;
          else
          {
            chart.Left = width;
            chart.Width = width;
          }
        }
      }

      panelSummaryCharts.AutoScrollMinSize = new Size(width * 2, panelSummaryCharts.AutoScrollMinSize.Height);

      LastPanelSummaryResize = DateTime.Now;

      // Ensure the timer is started.  Once there is no recent resizing then its child controls (charts) will be activated.
      if (! timerShowCharts.Enabled)
      {
        timerShowCharts.Interval = 300;
        timerShowCharts.Start();
      }
    }


    // Adjust the horizontal position of "# Questions" and "# Responses"
    private void panelMidUpper_Resize(object sender, System.EventArgs e)
    {
      int width = (sender as Panel).Width / 2;

      Label label = labelSummaryQuestionsTally;
      label.Left = (width - label.Width) / 2;

      label = labelSummaryResponsesTally;
      label.Left = width + (width - label.Width) / 2;
    }


    private void timerShowCharts_Tick(object sender, System.EventArgs e)
    {
      TimeSpan duration = DateTime.Now - LastPanelSummaryResize;

      // Was it more than 2 timer cycles since the Resize event was last fired?
      if (duration.Seconds * 1000 + duration.Milliseconds > timerShowCharts.Interval * 2)
      {
        timerShowCharts.Stop();
        
        foreach(Control ctrl in panelSummaryCharts.Controls)
        {
          ChartPanel chartPanel = (ChartPanel) ctrl;

          if (chartPanel != null)
            chartPanel.Activate();
        }
      }
    }




  }  // end of class frmPoll
}
