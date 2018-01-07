using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.PieChart;

using Multimedia;
using DataObjects;


namespace Desktop
{
  using AnswerFormat = DataObjects.Constants.AnswerFormat;
  using GraphType = DataObjects.Constants.GraphType;
  using GraphAvgMethod = DataObjects.Constants.GraphAvgMethod;


	/// <summary>
	/// Summary description for ChartPanel.
	/// </summary>
	public class ChartPanel : System.Windows.Forms.UserControl
	{
    public event EventHandler GraphInfoEvent;  // An event that is fired when one of the two graph-related properties is changed


    #region Public Properties

    private int questionNum;
    public int QuestionNum    // 0-based
    {
      get
      {
        return questionNum;
      }
      set
      {
        questionNum = value;
        labelQuestionNum.Text = "Question " + (questionNum + 1).ToString();
        QuestionAnswerFormat = pollModel.Questions[questionNum].AnswerFormat;
        SetCurrentGraphType();
      }
    }

    private AnswerFormat questionAnswerFormat;
    public AnswerFormat QuestionAnswerFormat
    {
      get
      {
        return questionAnswerFormat;
      }
      set
      {
        questionAnswerFormat = value;
      }
    }

    public GraphType CurrentGraphType
    {
      get
      {
        return pollModel.Questions[QuestionNum].GraphType;
      }
      set
      {
        if (pollModel.Questions[QuestionNum].GraphType != value)
        {
          pollModel.Questions[QuestionNum].GraphType = value;

          if (GraphInfoEvent != null)
            GraphInfoEvent(this, new EventArgs());
        }
      }
    }

    public GraphAvgMethod CurrentAvgMethod
    {
      get
      {
        return pollModel.Questions[QuestionNum].GraphAvgMethod;
      }
      set
      {
        if (pollModel.Questions[QuestionNum].GraphAvgMethod != value)
        {
          pollModel.Questions[QuestionNum].GraphAvgMethod = value;

          if (GraphInfoEvent != null)
            GraphInfoEvent(this, new EventArgs());
        }
      }
    }

    public string QuestionText
    {
      get
      {
        return textBoxQuestionText.Text;
      }
      set
      {
        textBoxQuestionText.Text = value;
      }
    }

    Poll pollModel;            // Used internally
    public Poll PollModel      // Set externally
    {
      set
      {
        pollModel = value;
        comboGraphAvgMethod.SelectedItem = pollModel.Questions[QuestionNum].GraphAvgMethod.ToString();   // Default value from object model
      }
    }

    bool populating = true;
    private bool Populating
    {
      get
      {
        return populating;
      }
      set
      {
        populating = value;
      }
    }

    // Just the public face of the 'Populating' property
    public bool Activated
    {
      get
      {
        return populating;
      }
    }    

    #endregion


    // Module-level variables
    private Control Chart;    // This provides module-level access to the Chart control, no matter which chart control is being used


    #region ControlDefinitions

    private PanelGradient.PanelGradient panelMain;
    private System.Windows.Forms.Panel panelTop;
    private System.Windows.Forms.Label labelQuestionNum;
    private System.Windows.Forms.Panel panelGraphTypes;
    private System.Windows.Forms.RadioButton radioButtonNone;
    private System.Windows.Forms.RadioButton radioButtonBar;
    private System.Windows.Forms.RadioButton radioButtonLine;
    private System.Windows.Forms.RadioButton radioButtonPie;
    private System.Windows.Forms.Panel panelMiddle;
    private System.Windows.Forms.Panel panelBottom;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Label labelAveragingIntro;
    private System.Windows.Forms.ComboBox comboGraphAvgMethod;
    private System.Windows.Forms.TextBox textBoxQuestionText;
    private System.Windows.Forms.RadioButton radioButtonText;
    private System.ComponentModel.IContainer components;

    #endregion


    // Constructor
		public ChartPanel()
		{
		  InitializeComponent();
          PrepareControls();
		}


    /// <summary>
    /// This method was introduced because too many events were inadvertently being fired while the user control's data was being prepared.
    /// This lets the developer decide when the user control is truly ready to go!
    /// </summary>
    public void Activate()
    {
      Populating = false;
      PrepareChart();
    }


    public void Deactivate()
    {
      Populating = true;
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


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ChartPanel));
      this.panelMain = new PanelGradient.PanelGradient();
      this.panelBottom = new System.Windows.Forms.Panel();
      this.panelMiddle = new System.Windows.Forms.Panel();
      this.textBoxQuestionText = new System.Windows.Forms.TextBox();
      this.panelTop = new System.Windows.Forms.Panel();
      this.labelAveragingIntro = new System.Windows.Forms.Label();
      this.comboGraphAvgMethod = new System.Windows.Forms.ComboBox();
      this.panelGraphTypes = new System.Windows.Forms.Panel();
      this.radioButtonText = new System.Windows.Forms.RadioButton();
      this.radioButtonNone = new System.Windows.Forms.RadioButton();
      this.radioButtonBar = new System.Windows.Forms.RadioButton();
      this.radioButtonLine = new System.Windows.Forms.RadioButton();
      this.radioButtonPie = new System.Windows.Forms.RadioButton();
      this.labelQuestionNum = new System.Windows.Forms.Label();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.panelMain.SuspendLayout();
      this.panelMiddle.SuspendLayout();
      this.panelTop.SuspendLayout();
      this.panelGraphTypes.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelMain
      // 
      this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.panelMain.Controls.Add(this.panelBottom);
      this.panelMain.Controls.Add(this.panelMiddle);
      this.panelMain.Controls.Add(this.panelTop);
      this.panelMain.GradientColorOne = System.Drawing.Color.Honeydew;
      this.panelMain.GradientColorTwo = System.Drawing.Color.FromArgb(((System.Byte)(75)), ((System.Byte)(119)), ((System.Byte)(173)));
      this.panelMain.Location = new System.Drawing.Point(6, 6);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new System.Drawing.Size(514, 322);
      this.panelMain.TabIndex = 4;
      // 
      // panelBottom
      // 
      this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelBottom.DockPadding.All = 12;
      this.panelBottom.Location = new System.Drawing.Point(0, 112);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new System.Drawing.Size(514, 210);
      this.panelBottom.TabIndex = 13;
      // 
      // panelMiddle
      // 
      this.panelMiddle.Controls.Add(this.textBoxQuestionText);
      this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelMiddle.DockPadding.All = 12;
      this.panelMiddle.Location = new System.Drawing.Point(0, 40);
      this.panelMiddle.Name = "panelMiddle";
      this.panelMiddle.Size = new System.Drawing.Size(514, 72);
      this.panelMiddle.TabIndex = 12;
      // 
      // textBoxQuestionText
      // 
      this.textBoxQuestionText.BackColor = System.Drawing.Color.LightSteelBlue;
      this.textBoxQuestionText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBoxQuestionText.Location = new System.Drawing.Point(12, 12);
      this.textBoxQuestionText.Multiline = true;
      this.textBoxQuestionText.Name = "textBoxQuestionText";
      this.textBoxQuestionText.ReadOnly = true;
      this.textBoxQuestionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBoxQuestionText.Size = new System.Drawing.Size(490, 48);
      this.textBoxQuestionText.TabIndex = 0;
      this.textBoxQuestionText.Text = "Question Text";
      // 
      // panelTop
      // 
      this.panelTop.Controls.Add(this.labelAveragingIntro);
      this.panelTop.Controls.Add(this.comboGraphAvgMethod);
      this.panelTop.Controls.Add(this.panelGraphTypes);
      this.panelTop.Controls.Add(this.labelQuestionNum);
      this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelTop.Location = new System.Drawing.Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new System.Drawing.Size(514, 40);
      this.panelTop.TabIndex = 11;
      // 
      // labelAveragingIntro
      // 
      this.labelAveragingIntro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.labelAveragingIntro.AutoSize = true;
      this.labelAveragingIntro.Location = new System.Drawing.Point(197, 12);
      this.labelAveragingIntro.Name = "labelAveragingIntro";
      this.labelAveragingIntro.Size = new System.Drawing.Size(58, 16);
      this.labelAveragingIntro.TabIndex = 16;
      this.labelAveragingIntro.Text = "Averaging:";
      this.labelAveragingIntro.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.labelAveragingIntro.Visible = false;
      // 
      // comboGraphAvgMethod
      // 
      this.comboGraphAvgMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.comboGraphAvgMethod.BackColor = System.Drawing.Color.LightSteelBlue;
      this.comboGraphAvgMethod.Location = new System.Drawing.Point(254, 10);
      this.comboGraphAvgMethod.Name = "comboGraphAvgMethod";
      this.comboGraphAvgMethod.Size = new System.Drawing.Size(80, 21);
      this.comboGraphAvgMethod.TabIndex = 15;
      this.toolTip1.SetToolTip(this.comboGraphAvgMethod, "Choose the Averaging method most suitable for the given data");
      this.comboGraphAvgMethod.Visible = false;
      this.comboGraphAvgMethod.SelectedValueChanged += new System.EventHandler(this.comboGraphAvgMethod_SelectedValueChanged);
      // 
      // panelGraphTypes
      // 
      this.panelGraphTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.panelGraphTypes.Controls.Add(this.radioButtonText);
      this.panelGraphTypes.Controls.Add(this.radioButtonNone);
      this.panelGraphTypes.Controls.Add(this.radioButtonBar);
      this.panelGraphTypes.Controls.Add(this.radioButtonLine);
      this.panelGraphTypes.Controls.Add(this.radioButtonPie);
      this.panelGraphTypes.Location = new System.Drawing.Point(342, 9);
      this.panelGraphTypes.Name = "panelGraphTypes";
      this.panelGraphTypes.Size = new System.Drawing.Size(160, 24);
      this.panelGraphTypes.TabIndex = 0;
      this.panelGraphTypes.TabStop = true;
      // 
      // radioButtonText
      // 
      this.radioButtonText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.radioButtonText.Appearance = System.Windows.Forms.Appearance.Button;
      this.radioButtonText.BackColor = System.Drawing.Color.Silver;
      this.radioButtonText.Image = ((System.Drawing.Image)(resources.GetObject("radioButtonText.Image")));
      this.radioButtonText.Location = new System.Drawing.Point(96, 0);
      this.radioButtonText.Name = "radioButtonText";
      this.radioButtonText.Size = new System.Drawing.Size(32, 24);
      this.radioButtonText.TabIndex = 13;
      this.radioButtonText.Tag = "Text";
      this.toolTip1.SetToolTip(this.radioButtonText, "Textual Summary");
      this.radioButtonText.CheckedChanged += new System.EventHandler(this.ChartType_CheckedChanged);
      // 
      // radioButtonNone
      // 
      this.radioButtonNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.radioButtonNone.Appearance = System.Windows.Forms.Appearance.Button;
      this.radioButtonNone.BackColor = System.Drawing.Color.Silver;
      this.radioButtonNone.Image = ((System.Drawing.Image)(resources.GetObject("radioButtonNone.Image")));
      this.radioButtonNone.Location = new System.Drawing.Point(128, 0);
      this.radioButtonNone.Name = "radioButtonNone";
      this.radioButtonNone.Size = new System.Drawing.Size(32, 24);
      this.radioButtonNone.TabIndex = 12;
      this.radioButtonNone.Tag = "None";
      this.toolTip1.SetToolTip(this.radioButtonNone, "No Chart");
      this.radioButtonNone.CheckedChanged += new System.EventHandler(this.ChartType_CheckedChanged);
      // 
      // radioButtonBar
      // 
      this.radioButtonBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.radioButtonBar.Appearance = System.Windows.Forms.Appearance.Button;
      this.radioButtonBar.BackColor = System.Drawing.Color.Silver;
      this.radioButtonBar.Image = ((System.Drawing.Image)(resources.GetObject("radioButtonBar.Image")));
      this.radioButtonBar.Location = new System.Drawing.Point(0, 0);
      this.radioButtonBar.Name = "radioButtonBar";
      this.radioButtonBar.Size = new System.Drawing.Size(32, 24);
      this.radioButtonBar.TabIndex = 9;
      this.radioButtonBar.Tag = "Bar";
      this.toolTip1.SetToolTip(this.radioButtonBar, "Bar Chart");
      this.radioButtonBar.CheckedChanged += new System.EventHandler(this.ChartType_CheckedChanged);
      // 
      // radioButtonLine
      // 
      this.radioButtonLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.radioButtonLine.Appearance = System.Windows.Forms.Appearance.Button;
      this.radioButtonLine.BackColor = System.Drawing.Color.Silver;
      this.radioButtonLine.Image = ((System.Drawing.Image)(resources.GetObject("radioButtonLine.Image")));
      this.radioButtonLine.Location = new System.Drawing.Point(32, 0);
      this.radioButtonLine.Name = "radioButtonLine";
      this.radioButtonLine.Size = new System.Drawing.Size(32, 24);
      this.radioButtonLine.TabIndex = 10;
      this.radioButtonLine.Tag = "Line";
      this.toolTip1.SetToolTip(this.radioButtonLine, "Line Chart");
      this.radioButtonLine.CheckedChanged += new System.EventHandler(this.ChartType_CheckedChanged);
      // 
      // radioButtonPie
      // 
      this.radioButtonPie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.radioButtonPie.Appearance = System.Windows.Forms.Appearance.Button;
      this.radioButtonPie.BackColor = System.Drawing.Color.Silver;
      this.radioButtonPie.Image = ((System.Drawing.Image)(resources.GetObject("radioButtonPie.Image")));
      this.radioButtonPie.Location = new System.Drawing.Point(64, 0);
      this.radioButtonPie.Name = "radioButtonPie";
      this.radioButtonPie.Size = new System.Drawing.Size(32, 24);
      this.radioButtonPie.TabIndex = 11;
      this.radioButtonPie.Tag = "Pie";
      this.toolTip1.SetToolTip(this.radioButtonPie, "Pie Chart");
      this.radioButtonPie.CheckedChanged += new System.EventHandler(this.ChartType_CheckedChanged);
      // 
      // labelQuestionNum
      // 
      this.labelQuestionNum.AutoSize = true;
      this.labelQuestionNum.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelQuestionNum.Location = new System.Drawing.Point(9, 8);
      this.labelQuestionNum.Name = "labelQuestionNum";
      this.labelQuestionNum.Size = new System.Drawing.Size(85, 25);
      this.labelQuestionNum.TabIndex = 4;
      this.labelQuestionNum.Text = "Question";
      // 
      // ChartPanel
      // 
      this.BackColor = System.Drawing.Color.Navy;
      this.Controls.Add(this.panelMain);
      this.Name = "ChartPanel";
      this.Size = new System.Drawing.Size(520, 328);
      this.panelMain.ResumeLayout(false);
      this.panelMiddle.ResumeLayout(false);
      this.panelTop.ResumeLayout(false);
      this.panelGraphTypes.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		#endregion


    /// <summary>
    /// Run only once; called by the Constructor.
    /// </summary>
    private void PrepareControls()
    {
      comboGraphAvgMethod.DataSource = Enum.GetNames(typeof(GraphAvgMethod));
      Font font = new Font("Microsoft Sans Serif", 8.25F);
      labelAveragingIntro.Font = font;     // Not sure why, but these seem to
      comboGraphAvgMethod.Font = font;     // need to be set explicitly here
      
      // Debug: Need to find a way to prevent combobox from selecting text
    }



    /// <summary>
    /// This method is called whenever the QuestionNum property is set.  Thus it is only called once, soon after this ChartPanel is instantiated.
    /// Among other things, it "presses" the appropriate ChartType radio button.
    /// </summary>
    private void SetCurrentGraphType()
    {
      if (pollModel == null)
        radioButtonNone.Checked = true;

      else if (QuestionAnswerFormat == AnswerFormat.Freeform)
      {
        // If FreeForm then need to handle things differently because the standard graph types have no relevance
        radioButtonBar.Visible = false;
        radioButtonLine.Visible = false;
        radioButtonPie.Visible = false;
        radioButtonText.Visible = false;

        // For Freeform we're going to use the 'radioButtonNone' as an On/Off toggle
        radioButtonNone.ImageList = new ImageList();
        radioButtonNone.ImageList.Images.Add(Multimedia.Images.GetIcon("ShowGraph"));    // 0 - Green circle - When this is showing, then the Freeform info is hidden
        radioButtonNone.ImageList.Images.Add(Multimedia.Images.GetIcon("HideGraph"));    // 1 - Red circle with line across - When this is showing, then the Freeform info is visible
        radioButtonNone.ImageIndex = 0;    // The ".Checked" line below will toggle the radio button so setting it to '0' here is correct
        radioButtonNone.Tag = GraphType.Freeform.ToString();
        radioButtonNone.Checked = true;
      }

      else  // For all other AnswerFormat types, select the GraphType that was last selected
      {
        foreach (Control ctrl in panelGraphTypes.Controls)
        {
          RadioButton button = ctrl as RadioButton;
          if (button != null)
          {
            if (ctrl.Tag.ToString().ToLower() == CurrentGraphType.ToString().ToLower())
            {
              button.Checked = true;
              comboGraphAvgMethod.SelectedItem = CurrentAvgMethod.ToString();  // Only used by a few AnswerFormats but no harm in setting it regardless
              break;
            }
          }
        }
      }
    }



    /// <summary>
    /// This is the event handler for all of the GraphType radio buttons.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChartType_CheckedChanged(object sender, System.EventArgs e)
    {
      RadioButton button = sender as RadioButton;

      if (button.Checked)  // Only run the enclosed code when a button is being activated, not deactivated
      {
        // Determine whether Averaging label & combobox should be shown
        if (button.Tag.ToString().ToLower() == "none" || button.Tag.ToString().ToLower() == "freeform")
        {
          labelAveragingIntro.Visible = false;
          comboGraphAvgMethod.Visible = false;
        }
        else
        {
          switch (QuestionAnswerFormat)
          {
            case AnswerFormat.Standard:
            case AnswerFormat.List:
            case AnswerFormat.DropList:
            case AnswerFormat.Range:
            case AnswerFormat.MultipleBoxes:   // Not sure about this one
            case AnswerFormat.Spinner:
            case AnswerFormat.Freeform:
              labelAveragingIntro.Visible = false;
              comboGraphAvgMethod.Visible = false;
              break;

            case AnswerFormat.MultipleChoice:
              labelAveragingIntro.Visible = true;
              comboGraphAvgMethod.Visible = true;
              break;

            default:
              Debug.Fail("Unknown AnswerFormat: " + QuestionAnswerFormat.ToString(), "ChartPanel.ChartType_CheckedChanged");
              break;
          }
        }

        // If Freeform then need to do special handling of the single radio button
        if (button.Tag.ToString().ToLower() == GraphType.Freeform.ToString().ToLower())
        {
          radioButtonNone.ImageIndex = Math.Abs(radioButtonNone.ImageIndex - 1);
          radioButtonNone.CheckedChanged -= new System.EventHandler(this.ChartType_CheckedChanged);
          radioButtonNone.Checked = false;
          radioButtonNone.CheckedChanged += new System.EventHandler(this.ChartType_CheckedChanged);

          if (radioButtonNone.ImageIndex == 1)  // If showing red circle w line then need to show panel
          {
            CurrentGraphType = GraphType.Freeform;   // Save this setting for future use
            PrepareChart();
          }
          else   // The green circle is displayed so hide the "chart"
          {
            CurrentGraphType = GraphType.None;       // Save this setting for future use
            panelBottom.Controls[0].Visible = false;
          }
        }
        else if (button.Checked)
        {
          SaveGraphType(button.Tag.ToString());      // Save this setting for future use
          PrepareChart();
        }
      }

      SetBackColor(button);
    }


    private void SetBackColor(RadioButton button)
    {
      if (QuestionAnswerFormat == AnswerFormat.Freeform)
      {
        if (button.ImageIndex == 0)
          button.BackColor = Color.Honeydew;
        else
          button.BackColor = Color.Snow;
      }
      else
      {
        if (button.Checked)
          button.BackColor = Color.AliceBlue;
        else
          button.BackColor = Color.Silver;
      }
    }



    /// <summary>
    /// This method is called when a GraphType radio button is pressed.
    /// It ensures that the current Question's GraphType property is set correctly.
    /// </summary>
    private void SaveGraphType(string buttonTag)
    {
      if (pollModel == null)
        Debug.Fail("Poll Model not yet defined", "ChartPanel.SaveGraphType");   // Should never reach here
      else
      {
        if (buttonTag.ToLower() != CurrentGraphType.ToString().ToLower())
          CurrentGraphType = (GraphType) Enum.Parse(typeof(GraphType), buttonTag, true);
      }
    }


    private void PrepareChart()
    {
      if (Populating)
        return;

      this.SuspendLayout();
      Cursor.Current = Cursors.WaitCursor;

      if (Chart != null)
      {
        Chart.Dispose();
        Chart = null;
      }

      switch (CurrentGraphType)
      {
        case GraphType.Bar:
        case GraphType.Line:  
          BarLineChart barlineChart = new BarLineChart();

          if (CurrentGraphType == GraphType.Bar)
          {
            barlineChart.RenderingMode = ChartRenderingMode.BarWith3d;
            barlineChart.CumulativeMode = ChartCumulativeMode.StartFromLastValue;
          }
          else   // Line
          {
            barlineChart.RenderingMode = ChartRenderingMode.Linear3d;
            barlineChart.CumulativeMode = ChartCumulativeMode.StartFrom0;
          }

          barlineChart.BackColor = Color.Honeydew;
          barlineChart.BackColor2 = Color.FromArgb(136, 166, 204);
          
          barlineChart.BottomMargin	= 50;
          barlineChart.MainTitle = "";
          barlineChart.VerticalAxisMinValue = 0;

          barlineChart.ColumnFont = new Font("Arial", 8);        // Text above bars
          barlineChart.ColumnTitleFont = new Font("Arial", 8);   // Text below bars
          //barlineChart.Font = new Font("Arial", 14);
          barlineChart.LegendFont = new Font("Arial", 8);        // Appears to affect the Y-axis text
          barlineChart.MainTitleFont = new Font("Arial", 14);    // Text at bottom of chart
          barlineChart.DisplayTextOnColumns = true;

          Chart = (Control) barlineChart;
          break;

        case GraphType.Pie:
          PieChartControl pieChart = new PieChartControl();

          pieChart.BackColor = Color.Transparent;
          pieChart.LeftMargin = 10;
          pieChart.RightMargin = 10;
          pieChart.TopMargin = 10;
          pieChart.BottomMargin = 10;

          pieChart.FitChart= true;
          pieChart.SliceRelativeHeight = 0.25F;
          pieChart.InitialAngle = -1;
          pieChart.EdgeLineWidth = 1.0F;
          pieChart.EdgeColorType = EdgeColorType.DarkerThanSurface;
          pieChart.Font = new Font("Arial", 8);
          
          Chart = (Control) pieChart;
          break;

        case GraphType.Text:
          ListView listViewText = new ListView();
          listViewText.View = View.Details;
          listViewText.BackColor = Color.FromArgb(136, 166, 204);

          listViewText.MultiSelect = false;
          listViewText.FullRowSelect = true;
          listViewText.CheckBoxes = false;
          listViewText.Columns.Add("Answer", 200, HorizontalAlignment.Left);   // Prelim width only
          listViewText.Columns.Add("Num", 50, HorizontalAlignment.Center);
          listViewText.Columns.Add("%", 70, HorizontalAlignment.Center);
          listViewText.Resize += new EventHandler(listViewText_Resize);

          Chart = (Control) listViewText;
          break;

        case GraphType.Freeform:
          Panel panelFreeform = new Panel();
          panelFreeform.BackColor = Color.FromArgb(136, 166, 204);
          Chart = (Control) panelFreeform;
          break;

        case GraphType.None:
          // Do nothing because the old Chart was removed above
          break;

        default:
          Debug.Fail("Unknown Graph Type: " + CurrentGraphType.ToString(), "ChartPanel.PrepareChart");
          break;
      }

      if (Chart!= null)
      {
        Chart.Dock = DockStyle.Fill;
        panelBottom.Controls.Add(Chart);

        PopulateChart();   // We can now populate the chart with data from pollModel
      }

      Cursor.Current = Cursors.Default;
      this.ResumeLayout(true);
    }


    private void PopulateChart()
    {
      if (Populating)
        return;

      ArrayList allLabels;
      
      if (Chart != null)
      {
        ArrayList choiceSums = new ArrayList();
        ArrayList calcSums = new ArrayList();
        ArrayList choicePercents = new ArrayList();    // Note: These values are calculated with the 'Simple' method in Tools.SummarizeAnswers
        ArrayList choiceAverages = new ArrayList();
        ArrayList choiceValues = new ArrayList();
        ArrayList respAnswers = new ArrayList();
        ArrayList respNames = new ArrayList();
        ArrayList respDates = new ArrayList();

        _Question quest = pollModel.Questions[QuestionNum];
        _Respondents respondents = pollModel.Respondents;

        // First retrieve the correct summary info appropriate for the particular Answer Format
        switch (QuestionAnswerFormat)
        {
          case AnswerFormat.Standard:
          case AnswerFormat.List:
          case AnswerFormat.DropList:
          case AnswerFormat.MultipleChoice:
          case AnswerFormat.Range:
            Tools.SummarizeAnswers(QuestionAnswerFormat, respondents, quest, out choiceSums, out calcSums, out choicePercents);   // Gather sums and percents
            if (choiceSums.Count == 0)
            {
              DisplayNoResponses();
              return;
            }
            break;

          case AnswerFormat.MultipleBoxes:
            Tools.SummarizeAnswers(QuestionAnswerFormat, respondents, quest, out choiceSums, out calcSums, out choiceAverages);   // Gather sums and averages
            if (choiceSums.Count == 0)
            {
              DisplayNoResponses();
              return;
            }
            break;

          case AnswerFormat.Spinner:
            Tools.SummarizeSpinnerAnswers(respondents, quest, out choiceValues, out choiceSums, out calcSums, out choicePercents);
            if (choiceValues.Count == 0)
            {
              DisplayNoResponses();
              return;
            }
            break;

          case AnswerFormat.Freeform:
            Tools.SummarizeFreeFormAnswers(respondents, quest, out respAnswers, out respNames, out respDates);
            break;

          default:
            Debug.Fail("Unknown AnswerFormat: " + QuestionAnswerFormat.ToString(), "ChartPanel.PopulateChart(1)");
            break;
        }

        switch(CurrentGraphType)
        {
          case GraphType.Bar:
          {
            BarLineChart barChart = (BarLineChart) Chart;
            barChart.Columns.Clear();
            allLabels = GetAllTextValues(quest);
            
            if (QuestionAnswerFormat == AnswerFormat.Spinner)
              ClearMissingSpinnerResponses(choiceValues, ref allLabels);
            
            int val = 0;
            int maxVal = 0;
            int numColumns = (QuestionAnswerFormat == AnswerFormat.Spinner) ? choiceValues.Count : quest.Choices.Count;

            for (int i = 0; i < numColumns; i++)
            {
              ChartColumn column = null;

              switch (QuestionAnswerFormat)
              {
                case AnswerFormat.Standard:
                case AnswerFormat.List:
                case AnswerFormat.DropList:
                case AnswerFormat.Range:
                  val = Convert.ToInt32(choiceSums[i]);
                  maxVal = Math.Max(maxVal, val);
                  column = barChart.AddColumn(val);
                  break;

                case AnswerFormat.MultipleChoice:
                  if (CurrentAvgMethod == GraphAvgMethod.Simple)
                  {
                    val = Convert.ToInt32(choiceSums[i]);
                    maxVal = Math.Max(maxVal, val);
                    column = barChart.AddColumn(val);
                  }
                  else
                  {
                    val = Convert.ToInt32(calcSums[i]);
                    maxVal = Math.Max(maxVal, val);
                    column = barChart.AddColumn(val);
                  }
                  break;

                case AnswerFormat.MultipleBoxes:
                  val = 0;
                  try
                  {
                    val = Convert.ToInt32(choiceAverages[i]);
                  }
                  catch
                  {
                    // Do nothing but catch conversion error
                  }
                  maxVal = Math.Max(maxVal, val);
                  column = barChart.AddColumn(val);
                  break;

                case AnswerFormat.Spinner:
                  val = Convert.ToInt32(choiceSums[i]);
                  maxVal = Math.Max(maxVal, val);
                  column = barChart.AddColumn(val);
                  break;
              }

              if (column != null)
              {
                column.Title = Tools.AbbreviateString(allLabels[i].ToString(), 4 + 48 / quest.Choices.Count);
              }
            }

            int newMax;
            int newStep;
            CalcMaxAndStepValues(maxVal, out newMax, out newStep);
            barChart.VerticalAxisMaxValue = (newMax < 5) ? 5 : newMax;
            barChart.VerticalAxisStep = newStep;
            barChart.Refresh();
          }
          break;
          

          case GraphType.Line:  
          {
            BarLineChart lineChart = (BarLineChart) Chart;
            lineChart.Columns.Clear();
            allLabels = GetAllTextValues(quest);

            if (QuestionAnswerFormat == AnswerFormat.Spinner)
              ClearMissingSpinnerResponses(choiceValues, ref allLabels);

            int val = 0;
            int maxVal = 0;
            int numColumns = (QuestionAnswerFormat == AnswerFormat.Spinner) ? choiceValues.Count : quest.Choices.Count;

            for (int i = 0; i < numColumns; i++)
            {
              ChartColumn column = null;

              switch (QuestionAnswerFormat)
              {
                case AnswerFormat.Standard:
                case AnswerFormat.List:
                case AnswerFormat.DropList:
                case AnswerFormat.Range:
                  val = Convert.ToInt32(choiceSums[i]);
                  maxVal = Math.Max(maxVal, val);
                  column = lineChart.AddColumn(val);
                  break;

                case AnswerFormat.MultipleChoice:
                  if (CurrentAvgMethod == GraphAvgMethod.Simple)
                  {
                    val = Convert.ToInt32(choiceSums[i]);
                    maxVal = Math.Max(maxVal, val);
                    column = lineChart.AddColumn(val);
                  }
                  else
                  {
                    val = Convert.ToInt32(calcSums[i]);
                    maxVal = Math.Max(maxVal, val);
                    column = lineChart.AddColumn(val);
                  }
                  break;

                case AnswerFormat.MultipleBoxes:
                  val = Convert.ToInt32(choiceAverages[i]);
                  maxVal = Math.Max(maxVal, val);
                  column = lineChart.AddColumn(val);
                  break;

                case AnswerFormat.Spinner:
                  val = Convert.ToInt32(choiceSums[i]);
                  maxVal = Math.Max(maxVal, val);
                  column = lineChart.AddColumn(val);
                  break;
              }

              if (column != null)
              {
                int maxWidth = (lineChart.Width - lineChart.LeftMargin - lineChart.RightMargin) / quest.Choices.Count - lineChart.MarginBetweenColumn;
                column.Title = Tools.AbbreviateString(allLabels[i].ToString(), 4 + 48 / quest.Choices.Count);
              }
            }

            int newMax;
            int newStep;
            CalcMaxAndStepValues(maxVal, out newMax, out newStep);
            lineChart.VerticalAxisMaxValue = (newMax < 5) ? 5 : newMax;
            lineChart.VerticalAxisStep = newStep;
            lineChart.Refresh();
          }
          break;


          case GraphType.Pie:
          {
            // Note: This next line doesn't create a new PieChart but simply casts the 'Chart' control back to the 'PieChartControl' type.
            PieChartControl pieChart = (PieChartControl) Chart;

            allLabels = GetAllTextValues(quest);
            decimal[] pieValues = null;

            switch (QuestionAnswerFormat)
            {
              case AnswerFormat.Standard:
              case AnswerFormat.List:
              case AnswerFormat.DropList:
              case AnswerFormat.Range:
                ClearEmptyResponses(ref choiceSums, ref allLabels, ref choicePercents);
                pieValues = Tools.ConvertArrayListToDecimalArray(choiceSums);
                break;

              case AnswerFormat.MultipleChoice:
                if (CurrentAvgMethod == GraphAvgMethod.Simple)
                {
                  ClearEmptyResponses(ref choiceSums, ref allLabels, ref choicePercents);
                  pieValues = Tools.ConvertArrayListToDecimalArray(choicePercents);
                }
                else
                {
                  ClearEmptyResponses(ref choiceSums, ref allLabels, ref calcSums);
                  // Note: Because of weighting factors, the elements in 'calcSums' don't really mean anything individually, but rather
                  //       only in relation to each other - much like a percentage.  Thus, I think it's safe to use them as is.
                  pieValues = Tools.ConvertArrayListToDecimalArray(calcSums);
                }
                break;

              case AnswerFormat.MultipleBoxes:
                ClearEmptyResponses(ref choiceAverages, ref allLabels, ref choiceSums);
                pieValues = Tools.ConvertArrayListToDecimalArray(choiceAverages);
                break;

              case AnswerFormat.Spinner:
                ClearMissingSpinnerResponses(choiceValues, ref allLabels);
                pieValues = Tools.ConvertArrayListToDecimalArray(choiceSums);
                break;
            }

            pieChart.Values = pieValues;
            string[] pieTexts = (string[]) allLabels.ToArray(typeof(string));
            for (int i = 0; i < pieTexts.Length - 1; i++)            
            {
              pieTexts[i] = Tools.AbbreviateString(pieTexts[i], 20);
            }

            switch (QuestionAnswerFormat)
            {
              case AnswerFormat.Standard:
              case AnswerFormat.List:
              case AnswerFormat.DropList:
                pieTexts = Tools.AppendStringArray(Tools.AppendStringArray(Tools.AppendStringArray(pieTexts, "\n("), choicePercents), "%)");
                break;

              case AnswerFormat.Range:
                pieTexts = Tools.AppendStringArray(Tools.AppendStringArray(Tools.AppendStringArray(pieTexts, "\n("), choicePercents), "%)");

                // More To Add Here?
                break;

              case AnswerFormat.MultipleChoice:
                if (CurrentAvgMethod == GraphAvgMethod.Simple)
                  pieTexts = Tools.AppendStringArray(Tools.AppendStringArray(Tools.AppendStringArray(pieTexts, "\n("), choicePercents), "%)");
                else
                  pieTexts = Tools.AppendStringArray(Tools.AppendStringArray(Tools.AppendStringArray(pieTexts, "\n("), Tools.ConvertArrayListToPercentages(calcSums)), "%)");

                break;

              case AnswerFormat.MultipleBoxes:
                pieTexts = Tools.AppendStringArray(Tools.AppendStringArray(Tools.AppendStringArray(pieTexts, "\n(Avg: "), choiceAverages), ")");
                break;

              case AnswerFormat.Spinner:
                pieTexts = Tools.AppendStringArray(Tools.AppendStringArray(Tools.AppendStringArray(pieTexts, "  ("), choicePercents), "%)");

                // More To Add Here?
                break;
            }

            pieChart.Texts = pieTexts;
            pieChart.ToolTips = Tools.AppendStringArray(choiceSums, " response", true);
          }
          break;


          case GraphType.Text:
          {
            ListView listViewText = (ListView) Chart;
            listViewText.Items.Clear();

            listViewText.Columns[listViewText.Columns.Count - 1].Text = (QuestionAnswerFormat == AnswerFormat.MultipleBoxes) ? "Avg" : "%";

            switch (QuestionAnswerFormat)
            {
              case AnswerFormat.Spinner:
                for (int i = 0; i < choiceValues.Count; i++)
                {
                  ListViewItem lvwItem = new ListViewItem();
                  lvwItem.Text = choiceValues[i].ToString();
                  lvwItem.SubItems.Add(choiceSums[i].ToString() + " ");
                  double percent = (double) choicePercents[i];
                  lvwItem.SubItems.Add(percent.ToString("0.0") + "% ");
                  listViewText.Items.Add(lvwItem);
                }
                break;

              default:   // All the other formats
                if (QuestionAnswerFormat == AnswerFormat.MultipleChoice && CurrentAvgMethod == GraphAvgMethod.Weighted)
                  Tools.CalcPercents(calcSums, ref choicePercents);   // This will recalculate the percentages based on the values in 'calcSums'

                for (int i = 0; i < quest.Choices.Count; i++)
                {
                  _Choice choice = quest.Choices[i];
                  ListViewItem lvwItem = new ListViewItem();

                  switch (QuestionAnswerFormat)
                  {
                    case AnswerFormat.Standard:
                    case AnswerFormat.List:
                    case AnswerFormat.DropList:
                    case AnswerFormat.Range:
                      if (choice.Text != "")
                      {
                        if (QuestionAnswerFormat == AnswerFormat.Range)
                          lvwItem.Text = "[" + choice.MoreText + "]  " + choice.Text;
                        else
                          lvwItem.Text = choice.Text;

                        lvwItem.SubItems.Add(choiceSums[i].ToString() + " ");
                        double percent = (double) choicePercents[i];
                        lvwItem.SubItems.Add(percent.ToString("0.0") + "% ");      

                        listViewText.Items.Add(lvwItem);
                      }
                      break;

                    case AnswerFormat.MultipleChoice:
                      if (choice.Text != "")
                      {
                        lvwItem.Text = choice.Text;

                        if (CurrentAvgMethod == GraphAvgMethod.Simple)
                          lvwItem.SubItems.Add(choiceSums[i].ToString() + " ");
                        else
                        {
                          double calcVal = (double) calcSums[i];
                          lvwItem.SubItems.Add(calcVal.ToString("0.0") + " ");
                        }

                        double percent = (double) choicePercents[i];
                        lvwItem.SubItems.Add(percent.ToString("0.0") + "% ");
     
                        listViewText.Items.Add(lvwItem);
                      }
                      break;

                    case AnswerFormat.MultipleBoxes:
                      if (choice.Text != "")
                      {
                        lvwItem.Text = choice.Text;
                        lvwItem.SubItems.Add(choiceSums[i].ToString() + " ");
                        double avg = (double) choiceAverages[i];
                        lvwItem.SubItems.Add(avg.ToString("0.0") + " ");
                        listViewText.Items.Add(lvwItem);
                      }
                      break;
                  }
                }
                break;
            }

            // Some AnswerFormats require a summary tabulation
            string summaryText = "";
            switch (QuestionAnswerFormat)
            {
              case AnswerFormat.Range:
              case AnswerFormat.Spinner:
                listViewText.Items.Add(new ListViewItem(""));
                double avgValue = (double) Tools.SumArrayValues(calcSums) / Tools.SumArrayValues(choiceSums);
                summaryText = "Average Value:  " + avgValue.ToString("0.00");
                listViewText.Items.Add(new ListViewItem(summaryText));
                break;

              default:
                // Do nothing
                break;
            }

//            // Finally, we must check whether there are an excessive number of items that have forced the
//            // vertical scroll bars to appear.  If so, then we must shrink the width of the first column
//            // to compensate, otherwise the unwanted horiz scrollbar also appears.
//            // Note: The formula used isn't exact but seems to provide a reasonable
//            //       algorithm for both Portrait and Landscape mode.
//            int col0Width = listViewText.Width - ((listViewText.Columns.Count - 1) * smallColWidth + 2);
//            int visRows = (int) ((double) listViewText.Height / 17 + 0.5) - 1;
//            int col0Adj = (wid < hgt) ? 1 : 13;
//            listViewText.Columns[0].Width = (visRows > listViewText.Items.Count) ? col0Width : col0Width - col0Adj;
          }
          break;


          case GraphType.Freeform:
          {
            int gap = 4;

            Panel panelFreeform = (Panel) Chart;
            panelFreeform.Controls.Clear();
            int topMargin = gap;
            int availWid = panelFreeform.Width - gap * 8;

            if (respAnswers.Count == 0)
            {
              DisplayNoResponses();
            }
            else
            {
              Font font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
              Label label = PanelControls.AddLabel("Freeform Summary", font, Color.Black, topMargin, gap, ContentAlignment.TopLeft, AnchorStyles.Top | AnchorStyles.Left, panelFreeform);
              topMargin += label.Height + gap * 2;

              font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular);
              Font font2 = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
              for (int i = 0; i < respAnswers.Count; i++)
              {
                label = PanelControls.AddMultilineLabel(respAnswers[i].ToString(), font, Color.Blue, topMargin, gap * 3, -1, availWid, AnchorStyles.Top | AnchorStyles.Left, panelFreeform);
                topMargin += label.Height + gap;

                label = PanelControls.AddLabel(respNames[i].ToString(), font2, topMargin, gap * 3, ContentAlignment.TopLeft, AnchorStyles.Top | AnchorStyles.Left, panelFreeform);
                label = PanelControls.AddLabel(respDates[i].ToString(), font2, topMargin, availWid + gap * 3, ContentAlignment.TopRight, AnchorStyles.Top | AnchorStyles.Right, panelFreeform);
                topMargin += label.Height + gap * 8;
              }

              panelFreeform.AutoScrollMinSize = new Size(availWid, topMargin + 20);
            }
          }
          break;


          default:
            Debug.Fail("Unknown Graph Type: " + CurrentGraphType.ToString(), "ChartPanel.PopulateChart(2)");
            break;
        }
      }
    }


    /// <summary>
    /// Most AnswerFormats have their possible choices readily available.  But the spinner encodes them differently.
    /// This method correctly retrieves the text values for a given question.
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    private ArrayList GetAllTextValues(_Question quest)
    {
      ArrayList allLabels = new ArrayList();

      if (quest.AnswerFormat == AnswerFormat.Spinner)
      {
        // Every question with a Spinner has only one Choice object.  Within this object, the following values are encoded:
        //   Text      - Min value
        //   MoreText  - Max value
        //   MoreText2 - Step value
        _Choice choice = quest.Choices[0];

        int min = Convert.ToInt32(choice.Text);
        int max = Convert.ToInt32(choice.MoreText);
        int step = Convert.ToInt32(choice.MoreText2);

        for (int i = min; i <= max; i += step)
        {
          allLabels.Add(i.ToString());
        }
      }
      else
        allLabels = quest.Choices.GetAllTextValues();

      return allLabels;
    }





    /// <summary>
    /// Any element in 'values' that is zero will be removed, as will be the elements in the same position in 'labels' and 'values2'.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="labels"></param>
    /// <param name="values2"></param>
    private void ClearEmptyResponses(ref ArrayList values, ref ArrayList labels, ref ArrayList values2)
    {
      // For any answer that has a value of zero, remove both the value and its corresponding label from their respective ArrayLists.
      for(int i = labels.Count - 1; i >= 0; i--)
      {
        double item = Convert.ToDouble(values[i]);
        if (item == 0.0)
        {
          labels.RemoveAt(i);
          values.RemoveAt(i);
          values2.RemoveAt(i);
        }
      }
    }

    private void ClearEmptyResponses(ref ArrayList values, ref ArrayList labels)
    {
      ArrayList dummy = new ArrayList();
      ClearEmptyResponses(ref values, ref labels, ref dummy);
    }



    /// <summary>
    /// The only choiceValues present are those which were selected at least once.  All others will be absent.
    /// So the key is to remove all labels for which there is no corresponding item in 'choiceValues'.
    /// </summary>
    /// <param name="choiceValues"></param>
    /// <param name="labels"></param>
    private void ClearMissingSpinnerResponses(ArrayList choiceValues, ref ArrayList labels)
    {
      for(int i = labels.Count - 1; i >= 0; i--)
      {
        int val = Convert.ToInt32(labels[i]);       // ex. 1,2,3,...
        if (!choiceValues.Contains((object) val))
        {
          labels.RemoveAt(i);
        }
      }
    }



    // Note: If this event handler is activated too early then duplicate bars will be drawn
    private void comboGraphAvgMethod_SelectedValueChanged(object sender, System.EventArgs e)
    {
      ComboBox comboBox = sender as ComboBox;
      CurrentAvgMethod = (GraphAvgMethod) Enum.Parse(typeof(GraphAvgMethod), comboBox.SelectedItem.ToString(), true);
      PopulateChart();
      panelGraphTypes.Focus();
    }

//    private void textBoxQuestionText_Resize(object sender, System.EventArgs e)
//    {
//      if (TextBoxResizing)
//        return;
//
//      TextBoxResizing = true;
//
//      Debug.WriteLine(QuestionNum.ToString());
//      TextBox textBox = sender as TextBox;
//      int wid = Tools.GetLabelWidth(textBox.Text, textBox.Font);
//      if (wid > textBox.ClientSize.Width)
//        textBox.ScrollBars = ScrollBars.Vertical;
//      else
//        textBox.ScrollBars = ScrollBars.None;
//
//      TextBoxResizing = false;
//    }



    private void CalcMaxAndStepValues(int maxVal, out int newMax, out int newStep)
    {
      // Calc Max value
      double logMax = Math.Log10((double) maxVal);
      double exp = Math.Floor(logMax);
      double tmpMax = Math.Pow(10, (logMax - exp)) + 1;
      tmpMax *= Math.Pow(10, exp);
      newMax = (int) tmpMax;

      // Calc Step value    Note: (Max - Min) / Step = # of horiz grid lines
      double stepVal = tmpMax / 10F;
      double logStep = Math.Log10((double) stepVal);
      exp = Math.Floor(logStep);
      double tmpStep = Math.Pow(10, (logStep - exp));
      tmpStep *= Math.Pow(10, exp);
      newStep = Math.Max(1, (int) tmpStep);
    }


    private void listViewText_Resize(object sender, EventArgs e)
    {
      ListView listView = (ListView) sender;

      if (listView.Columns.Count > 0)
      {
        // Sum up widths of all but the first column
        int sum = 0;
        for (int i = 1; i < listView.Columns.Count; i++)
        {
          sum += listView.Columns[i].Width;
        }

        listView.Columns[0].Width = listView.ClientSize.Width - sum - 1;
      }
    }


    private void DisplayNoResponses()
    {
      panelBottom.Controls.Clear();   // Remove the chart control
      Font font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
      Label label = PanelControls.AddLabel("No responses recorded", font, Color.Red, -1, -1, ContentAlignment.MiddleCenter, AnchorStyles.Top | AnchorStyles.Left, panelBottom);
      Tools.CenterControl(label);
      panelGraphTypes.Visible = false;
      labelAveragingIntro.Visible = false;
      comboGraphAvgMethod.Visible = false;
    }



	}
}
