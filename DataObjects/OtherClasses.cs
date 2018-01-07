using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;


namespace DataObjects
{
  // This file contains minor classes that are used throughout the Desktop software.

  public class AnswerButton : RadioButton
  {
    public AnswerButton()
    { 
      Width = 88;
      Height = 38;
      
      Font = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point);
      TextAlign = ContentAlignment.MiddleCenter;
  
      AutoCheck = true;
      SetBackColor("Up");
      Appearance = Appearance.Button;
      FlatStyle = FlatStyle.Standard;
  
      Tag = "Questions[].AnswerFormat";
    }

    // Read-only property
    public string NamePrefix
    {
      get
      {
        return "radioAnswerFormat";
      }
    }

    private Constants.AnswerFormat answerformat;
    public Constants.AnswerFormat AnswerFormat
    {
      get
      {
        return answerformat;
      }
      set
      {
        answerformat = value;
      }
    }

    public void SetBackColor(string position)
    {
      if (position.ToLower() == "down")
        BackColor = Color.FromArgb(255,226,234,244);
      else
        BackColor = Color.LightSteelBlue;
    }
  }


  public class QuestionButton : RadioButton
  {
    public QuestionButton()
    { 
      Width = 70;
      Height = 60;

      bool prefFontFound = false;
      foreach (FontFamily fontFamily in FontFamily.Families)
      {
        if (fontFamily.Name.ToLower() == "book antiqua")
        {
          prefFontFound = true;
          break;
        }
      }

      if (prefFontFound)
        Font = new Font("Book Antiqua", 24, FontStyle.Bold);
      else
        Font = new Font("Arial", 24, FontStyle.Bold);
  
      TextAlign = ContentAlignment.MiddleCenter;
  
      AutoCheck = true;
      SetBackColor("Up");
      Appearance = Appearance.Button;
      FlatStyle = FlatStyle.Standard;
  
      Tag = "";  // No need for tag, because this is the Questions indexer?  Or can it be used like: "this.CurrentQuestionNumber" ?
    }

    // Read-only property
    public string NamePrefix
    {
      get
      {
        return "radioQuestion";
      }
    }

    private int questionNum;
    public int QuestionNum
    {
      get
      {
        return questionNum;
      }
      set
      {
        questionNum = value;
      }
    }

    private int currentChoice;
    public int CurrentChoice
    {
      get
      {
        return currentChoice;
      }
      set
      {
        currentChoice = value;
      }
    }

    public void SetBackColor(string position)
    {
      if (position.ToLower() == "down")
        BackColor = Color.FromArgb(245,245,255);
      else
        BackColor = Color.FromArgb(192,192,255);
    }
  }



  public class ResponseButton : RadioButton
  {
    public ResponseButton()
    { 
      Width = 70;
      Height = 30;  // Half the height of Question buttons

      bool prefFontFound = false;
      foreach (FontFamily fontFamily in FontFamily.Families)
      {
        if (fontFamily.Name.ToLower() == "book antiqua")
        {
          prefFontFound = true;
          break;
        }
      }

      if (prefFontFound)
        Font = new Font("Book Antiqua", 12, FontStyle.Bold);
      else
        Font = new Font("Arial", 12, FontStyle.Bold);
  
      TextAlign = ContentAlignment.MiddleCenter;
  
      AutoCheck = true;
      SetBackColor("Up");
      Appearance = Appearance.Button;
      FlatStyle = FlatStyle.Standard;
  
      Tag = "";
    }

    // Read-only property
    public string NamePrefix
    {
      get
      {
        return "radioResponse";
      }
    }

    private int responseNum;
    public int ResponseNum
    {
      get
      {
        return responseNum;
      }
      set
      {
        responseNum = value;
      }
    }

    public void SetBackColor(string position)
    {
      if (position.ToLower() == "down")
        BackColor = Color.FromArgb(214,254,254);
      else   // "up"
        BackColor = Color.FromArgb(100,200,200);
    }
  }



  // Defines a special control that will serve as a sub-panel to panelChoices.
  // Each of these panels will hold the contents of one choice.
  public class PanelChoice : Panel
  {
    public PanelChoice()
    { 
      Height = BaseHeight;
      BorderStyle = BorderStyle.Fixed3D;
      Font = new Font("Arial", 10, FontStyle.Regular, GraphicsUnit.Point);
      Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      
      SetBackColor("Up");
    }

    // Read-only property
    public string NamePrefix
    {
      get
      {
        return "panelChoice";
      }
    }

    public int BaseHeight
    {
      get
      {
        return 50;
      }
    }

    private int choicenum;
    public int ChoiceNum
    {
      get
      {
        return choicenum;
      }
      set
      {
        choicenum = value;
      }
    }

    public void SetBackColor(string position)
    {
      if (position.ToLower() == "down")
        BackColor = Color.FromArgb(255,170,255,170);
      else
        BackColor = Color.FromArgb(255,175,225,175);
    }

    public void SetAnchorRight(bool activate)
    {
      if (activate)
        Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      else
        Anchor = AnchorStyles.Top | AnchorStyles.Left;
    }
  }



  /// <summary>
  /// This class contains a data structure that is used by the Preview panel in Desktop.frmPoll.
  /// </summary>
  public class PreviewInfo
  {
    public int SelectedIndex;              // SelectedIndex* of item that is current selected
    public string OtherText;               // Holds the ExtraInfo text the user might add into the provided textbox
    public string Freeform;                // Only used for Answer Format #5
    public int Spinner;                    // Only used for Answer Format #6
    public SortedList CheckBoxItems;       // Holds 'SelectedIndex' and 'OtherText' data for checkboxes only (keyed by 'SelectedIndex')

    // * Note: The 'SelectedIndex' for listboxes and comboboxes is directly equivalent.  But for radio buttons and checkboxes
    //         we can not rely on the 'TabIndex' property being contiguous because we will be interspersing "Other" type textboxes
    //         in between them.

    public PreviewInfo()
    {
      SelectedIndex = -1;
      OtherText = "";
      Freeform = "";
      Spinner = -1;
      CheckBoxItems = new SortedList();
    }

    // Adds the item whose Selected Index = index.
    public void Add(int index)
    {
      Add(index, "");
    }

    // Adds the item whose SelectedIndex = index and whose OtherText = text.
    public void Add(int index, string text)
    {
      if (this.CheckBoxItems.Count == 0)                 // If the CheckBoxItems array is empty
        if (SelectedIndex != -1)                         // and we have valid values in the root
        {                                                // then copy them down into a new first element
          PreviewInfo pInfo = new PreviewInfo();
          pInfo.SelectedIndex = this.SelectedIndex;
          pInfo.OtherText = this.OtherText;
          CheckBoxItems.Add(this.SelectedIndex, pInfo);
        }

      // After that preliminary check, we can now add (or update) the new parameters
      if (CheckBoxItems.ContainsKey(index))
        (CheckBoxItems[index] as PreviewInfo).OtherText = text;
      else
      {
        PreviewInfo pInfo = new PreviewInfo();
        pInfo.SelectedIndex = index;
        pInfo.OtherText = text;
        CheckBoxItems.Add(index, pInfo);
      }
    }

    // Removes the item whose Selected Index = index.
    public void Remove(int index)
    {
      if (this.CheckBoxItems.ContainsKey(index))
        this.CheckBoxItems.Remove(index);
    }

    // Provides a bit-encoded sum of the items in the CheckBoxInfo array.
    public int CalcBitSum()
    {
      int extrainfoSum = 0;

      foreach (PreviewInfo pInfo in this.CheckBoxItems.Values)
      {
        extrainfoSum = extrainfoSum + (int) Math.Pow(2, pInfo.SelectedIndex);
      }

      return extrainfoSum;    
    }
  }



}
