using System;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;

using DataObjects;


namespace PocketPC
{
  // Define Aliases
  using CFSysInfo = DataObjects.CFSysInfo;
  using Platform = DataObjects.Constants.MobilePlatform;
  using TextFormat = DataObjects.Constants.TextFormat;
  using RespondentInfo = Tools.RespondentInfo;



  /// <summary>
  /// Summary description for frmRespondent.
  /// </summary>
  public class frmRespondent : System.Windows.Forms.Form
  {
    private System.Windows.Forms.MenuItem menuItemBack;
    private System.Windows.Forms.MenuItem menuItemNext;
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.Label labelTopIntro;
    private DataObjects.AutoScrollPanel panelTopics;
    
    private bool IsDirty;                // Set true by code in this form if any text/combo box data is changed
    private bool OKonly;                 // A module level variable, set by a parameter passed to the constructor
    private int ScreenAdv;               // -1 : Move backward one screen   +1 : Move forward one screen
    private ArrayList mRespInfoArray;    // A module level variable, set by a parameter passed to the constructor
    

    
    /// <summary>
    /// This version of the constructor is called when in Wizard mode, where Back and Next are displayed.
    /// </summary>
    /// <param name="respInfoArray"></param>
    /// <param name="isDirty"></param>
    /// <param name="screenAdv"></param>
    /// <param name="backAvail"></param>
    public frmRespondent(ArrayList respInfoArray, ref bool isDirty, ref int screenAdv, ref bool backAvail)
    {
      Initialize(respInfoArray, ref isDirty, ref screenAdv, ref backAvail, false);
    }

    
    /// <summary>
    /// This version of the constructor is called when in dialog mode, where just OK is displayed.
    /// </summary>
    /// <param name="respInfoArray"></param>
    /// <param name="isDirty"></param>
    public frmRespondent(ArrayList respInfoArray, ref bool isDirty)
    {
      int dummyInt = 0;           // The values of these dummy variables are never used but
      bool dummyBool = false;     // the variables themselves are necessary to be passed.
      Initialize(respInfoArray, ref isDirty, ref dummyInt, ref dummyBool, true);
    }


    /// <summary>
    /// Called from all of the constructors of this class.
    /// </summary>
    /// <param name="respInfoArray"></param>
    /// <param name="isDirty"></param>
    /// <param name="screenAdv"></param>
    /// <param name="backAvail"></param>
    /// <param name="okOnly"></param>
    private void Initialize(ArrayList respInfoArray, ref bool isDirty, ref int screenAdv, ref bool backAvail, bool okOnly)
    {
      Cursor.Current = Cursors.WaitCursor;
      InitializeComponent();
      //InitializeScreen();

      this.BringToFront();

      if (okOnly)
      {
        menuItemBack.Text = "OK";
        menuItemNext.Text = "";
        menuItemNext.Enabled = false;
      }
      else
      {
        menuItemBack.Text = "< Back";
        menuItemBack.Enabled = backAvail;
        menuItemNext.Text = "Next >";
      }

      OKonly = okOnly;
      IsDirty = isDirty;

      PopulateForm(respInfoArray);
      InitializeScreen();
      mRespInfoArray = respInfoArray;   // Provide a module-level reference to the passed parameter

      this.Resize += new System.EventHandler(this.frmRespondent_Resize);
      Cursor.Current = Cursors.Default;

      this.ShowDialog();

      this.Resize -= new System.EventHandler(this.frmRespondent_Resize);
      isDirty = IsDirty;
      screenAdv = ScreenAdv;
      backAvail = true;   // This is set correctly but we may never allow one to back up into frmRespondent
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmRespondent));
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemBack = new System.Windows.Forms.MenuItem();
      this.menuItemNext = new System.Windows.Forms.MenuItem();
      this.labelTopIntro = new System.Windows.Forms.Label();
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
      // labelTopIntro
      // 
      this.labelTopIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelTopIntro.ForeColor = System.Drawing.Color.Blue;
      this.labelTopIntro.Location = new System.Drawing.Point(4, 8);
      this.labelTopIntro.Size = new System.Drawing.Size(228, 30);
      this.labelTopIntro.Text = "Please ask the person you\'re interviewing for the following information (optional" +
        ")";
      // 
      // frmRespondent
      // 
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ControlBox = false;
      this.Controls.Add(this.labelTopIntro);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";

    }
    #endregion


    private void InitializeScreen()
    {
      RepositionControls(true);
    }


    private void frmRespondent_Resize(object sender, System.EventArgs e)
    {
      RepositionControls(false);
    }


    private void RepositionControls(bool forceRedraw)
    {
      int wid = 0, hgt = 0, aHgt = 0;

      if ((ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt)) || forceRedraw)
      {
        int gap = 4;   // A small value, used as the basis for providing horiz and vert spacing

        if (panelTopics == null)
          return;

        if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
        {
          // To Do: Still to need to flesh out SmartPhone positioning

        }

        else
        {
          // Both orientations
          panelTopics.Width = wid - gap * 4;

          if (wid < hgt)  // Is it in Portrait mode?
          {
            labelTopIntro.Left = gap * 3;
            labelTopIntro.Width = wid - gap * 6;
            panelTopics.Height = aHgt - gap * 8;
          }
          else  // Landscape mode
          {
            labelTopIntro.Left = gap * 3;
            labelTopIntro.Width = (int) (wid * 0.8);
            panelTopics.Height = aHgt - gap * 10;
          }
        }
      }
    }


    // Note: This will serve as "OK" when being called from frmRepeat_Respondent
    private void menuItemBack_Click(object sender, System.EventArgs e)
    {
      this.Focus();
      SaveUserInput();
      ScreenAdv = (this.OKonly) ? 0 : -1;
      this.Close();
    }


    // Note: This will be disabled when being called from frmRepeat_Respondent
    private void menuItemNext_Click(object sender, System.EventArgs e)
    {
      this.Focus();
      SaveUserInput();
      ScreenAdv = 1;
      this.Close();
    }


    private void PopulateForm(ArrayList respInfoArray)
    {
      int gap = 4;   // A small value, used as the basis for providing horiz and vert spacing
      int rowHeight = 18;

      int wid, hgt, aHgt;
      ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt);

      // Create the scrolling panel for the topic controls
      panelTopics = new DataObjects.AutoScrollPanel();

      panelTopics.Location = new Point(gap * 4, gap * 10);
      panelTopics.Size = new Size(wid - gap * 4, aHgt - gap * 8);
      this.Controls.Add(panelTopics);

      int top = gap * 2;
      int colWidth = panelTopics.Contents.Width / 2 - gap * 2;

      foreach (RespondentInfo respInfo in respInfoArray)
      {
        string propName = respInfo.PropName;
        string labelText = respInfo.LabelText;

        // Intro Label
        Label labelIntro = new Label();
        labelIntro.Text = labelText + " :";
        labelIntro.Location = new Point(0, top + 1);
        labelIntro.Size = new Size(gap * 18, rowHeight);
        labelIntro.TextAlign = ContentAlignment.TopLeft;
        labelIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
        panelTopics.Contents.Controls.Add(labelIntro);

        // We've now added the label to the scrolling panel.  So now see what type of control should be to its right
        if (respInfo.Presets != null)
        {
          ComboBoxEx comboBox = new ComboBoxEx();
          comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
          comboBox.Location = new Point(gap * 22, top);
          comboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
          comboBox.Size = new Size(gap * 24, rowHeight);
          comboBox.Enabled = true;
          comboBox.Tag = respInfo;
          Tools.FillCombo(comboBox, respInfo.Presets);
          comboBox.Text = respInfo.Value;

          panelTopics.Contents.Controls.Add(comboBox);
        }
        else
        {
          TextBoxEx textBox = new TextBoxEx();
          textBox.Tag = respInfo;    // propName;
          textBox.Location = new Point(gap * 22, top);
          textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
          textBox.Size = new Size(gap * 24, rowHeight);
          textBox.Enabled = true;
          textBox.Tag = respInfo;
          textBox.AcceptsTab = true;
          textBox.BackColor = Color.White;
          textBox.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);    // For now, we've only figured out how to check Numeric-Only on the fly
          textBox.LostFocus += new EventHandler(TextBox_LostFocus);
          textBox.Text = respInfo.Value;

          panelTopics.Contents.Controls.Add(textBox);
        }

        top += rowHeight + gap * 3;
      }

      panelTopics.SetScrollHeight(top + gap + 80);   // The numeric value wad added to account for the SIP
    }


//    private string GetRespondentValue(_Respondent respondent, string propName)
//    {
//      object val = null;
//      object[] indexer = new object[0];
//
//      PropertyInfo propInfo = respondent.GetType().GetProperty(propName);
//
//      if (propInfo != null)
//        val = propInfo.GetValue(respondent, indexer);
//
//      if (val != null)
//        return val.ToString();
//
//      return "";
//    }



    // Note: So far we've only figured out how to handle NumericOnly on-the-fly.
    private void TextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
    {
      TextBoxEx textBox = sender as TextBoxEx;
      TextFormat format = (textBox.Tag as RespondentInfo).Formatting;

      if (format == TextFormat.NumericOnly)
      {
        char c = e.KeyChar;
        int i = (int) c;

        if ((i == 8) || (i == 46) || char.IsNumber(c))
        {
          // Do nothing because the character is valid
        }
        else
        {
          e.Handled = true;  // This will cancel the character input
        }
      }
    }


    // Debug: Currently not used
//    private void TextBox_TextChanged(object sender, System.EventArgs e)
//    {
//      TextFormat format;
//      TextBoxEx textBox;
//
//      if (sender.GetType().Name == "TextBoxEx")
//      {
//        textBox = sender as TextBoxEx;
//        format = (textBox.Tag as RespondentInfo).Formatting;
//
//        int selStart = textBox.SelectionStart;
//        textBox.Text = textBox.Text.ToUpper();
//        textBox.SelectionStart = selStart;
//      }
//    }


    /// <summary>
    /// This event is fired when the user leaves a textbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TextBox_LostFocus(object sender, System.EventArgs e)
    {
      TextBoxEx textBox = sender as TextBoxEx;
      RespondentInfo respInfo = textBox.Tag as RespondentInfo;
      TextFormat format = respInfo.Formatting;

      switch(format)
      {
        case TextFormat.NumericOnly:
          // Do nothing, because this was handled in the KeyPress event
          break;

        case TextFormat.LowerCase:
          textBox.Text = textBox.Text.ToLower().Trim();
          break;

        case TextFormat.UpperCase:
          textBox.Text = textBox.Text.ToUpper().Trim();
          break;

        case TextFormat.ProperCase:
          textBox.Text = Tools.ProperCase(textBox.Text).Trim();
          break;

        case TextFormat.None:
          textBox.Text = textBox.Text.Trim();
          break;

        default:
          Debug.Fail("Unknown formatting code: " + format.ToString(), "frmRespondent.TextBox_LostFocus");
          break;
      }
    }



    /// <summary>
    /// Examine every TextBox & ComboBox that the user could have entered data 
    /// into and store this information in the module-level mRespInfoArray object.
    /// </summary>
    private void SaveUserInput()
    {
      foreach(Control ctrl in panelTopics.Contents.Controls)
      {
        bool possValToSave = true;
        string propName;
        string newval = null;
        int idx = -1;

        if (ctrl.GetType().Name == "TextBoxEx")
        {
          TextBoxEx textBox = ctrl as TextBoxEx;
          propName = (textBox.Tag as RespondentInfo).PropName;
          idx = (textBox.Tag as RespondentInfo).Index;
          newval = textBox.Text;
        }
        else if (ctrl.GetType().Name == "ComboBoxEx")
        {
          ComboBoxEx comboBox = ctrl as ComboBoxEx;
          propName = (comboBox.Tag as RespondentInfo).PropName;
          idx = (comboBox.Tag as RespondentInfo).Index;
          newval = comboBox.Text;
        }
        else
        {
          // A label, so just ignore
          possValToSave = false;
        }

        if (possValToSave)
        {
          // See if we have a new value to store
          string oldval = (mRespInfoArray[idx] as RespondentInfo).Value;
          if (oldval != newval)
          {
            (mRespInfoArray[idx] as RespondentInfo).Value = newval;
            IsDirty = true;
          }
        }
      }
    }


  }
}
