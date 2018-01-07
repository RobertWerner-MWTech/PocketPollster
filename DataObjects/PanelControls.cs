using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;


namespace DataObjects
{
	/// <summary>
	/// This module provides control instantiation methods for The Preview pane of the Desktop
	/// 
	/// Note: The Pocket PC version exists separately in PanelControlsCF because the CF currently doesn't support all the same properties we use herein.
	/// </summary>

	public class PanelControls
	{
		public PanelControls()
		{
		}


    public static CheckBoxPP AddCheckBox(string text, Font font, int top, int left, int width, AnchorStyles anchors, Panel panel, int tabidx, int selidx)
    {
      CheckBoxPP checkBox = new CheckBoxPP();

      checkBox.Anchor = anchors;

      checkBox.Font = font;
      checkBox.TabIndex = tabidx;
      checkBox.SelectedIndex = selidx;
      checkBox.Name = "radioPreview" + tabidx.ToString();

      checkBox.Text = Tools.FixPanelText(text);

      int height = Tools.GetLabelHeight(text, font, width - 20);  // The '20' accounts for the checkbox and the gap afterwards
      checkBox.Height = height;
      checkBox.Width = width;
      checkBox.Left = left;
      checkBox.Top = top;

      checkBox.TextAlign = ContentAlignment.TopLeft;
      checkBox.CheckAlign = ContentAlignment.TopLeft;

      panel.Controls.Add(checkBox);

      checkBox.Click += new System.EventHandler(Preview_Event);

      return checkBox;
    }


    public static ComboBox AddComboBox(_Choices choices, Font font, int top, int left, AnchorStyles anchors, Panel panel, int tabidx)
    {
      int gap = 4;
      int maxWidth = 0;
      
      ComboBoxPP cboBox = new ComboBoxPP();
      cboBox.Anchor = anchors;
      cboBox.Font = font;
      cboBox.TabIndex = tabidx;
      cboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      cboBox.MaxDropDownItems = 10;
      cboBox.Name = "comboBox" + tabidx.ToString();

      cboBox.Items.Add("Select an Item");

      foreach (_Choice choice in choices)
      {
        string txt = choice.Text;
        if (txt != "")
        {
          cboBox.Items.Add(txt);
          int itemWidth = Tools.GetLabelWidth(txt, font);

          if (itemWidth > maxWidth)
            maxWidth = itemWidth;
        }  
      }

      if (cboBox.Items.Count == 1)
        return null;

      cboBox.SelectedIndex = 0;

      int prefWidth = maxWidth + gap + (cboBox.Width - cboBox.ClientSize.Width); 
      if (prefWidth < panel.Width * 0.8)
      {  
        if (prefWidth < panel.Width * 0.4)
          prefWidth = (int) (panel.Width * 0.4);
        cboBox.Width = prefWidth;
      }
      else
        cboBox.Width = (int) (panel.Width * 0.8);
 
      cboBox.Top = top;
      cboBox.Left = left;

      panel.Controls.Add(cboBox);

      cboBox.SelectedIndexChanged += new System.EventHandler(Preview_Event);

      return cboBox;
    }

    
    public static Label AddLabel(string text, Font font, Color color, int top, int xpos, ContentAlignment align, AnchorStyles anchors, Panel panel)
    {
      Label label = new Label();

      label.AutoSize = true;
      label.Anchor = anchors;
      label.TabIndex = 98;   // Outrageously large number

      label.Font = font;
      label.Text = Tools.FixPanelText(text);
      label.ForeColor = color;
      label.TextAlign = align;
      label.Top = top;

      switch (align)
      {
        case ContentAlignment.BottomLeft:
        case ContentAlignment.MiddleLeft:
        case ContentAlignment.TopLeft:
          label.Left = xpos;
          break;

        case ContentAlignment.BottomCenter:
        case ContentAlignment.MiddleCenter:
        case ContentAlignment.TopCenter:
          label.Left = xpos - (Tools.GetLabelWidth(text, font) / 2);
          break;

        case ContentAlignment.BottomRight:
        case ContentAlignment.MiddleRight:
        case ContentAlignment.TopRight:
          label.Left = xpos - Tools.GetLabelWidth(text, font);
          break;
      }

      panel.Controls.Add(label);

      return label;
    }

    public static Label AddLabel(string text, Font font, int top, int xpos, ContentAlignment align, AnchorStyles anchors, Panel panel)
    {
      return AddLabel(text, font, Color.Black, top, xpos, align, anchors, panel);
    }



    /// <summary>
    /// Adds a multiline label to a panel.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="font"></param>
    /// <param name="color"></param>
    /// <param name="top"></param>
    /// <param name="left"></param>
    /// <param name="height">If -1 then 'width' will be used as "max width" and the height will be calculated</param>     
    /// <param name="width"></param>
    /// <param name="anchors"></param>
    /// <param name="panel"></param>
    /// <returns></returns>
    public static Label AddMultilineLabel(string text, Font font, Color color, int top, int left, int height, int width, AnchorStyles anchors, Panel panel)
    {
      Label label = new Label();

      label.AutoSize = false;
      label.Anchor = anchors;
      label.TabIndex = 99;   // Outrageously large number

      label.Font = font;
      label.ForeColor = color;
      label.Text = Tools.FixPanelText(text);

      if (height == -1)
        height = Tools.GetLabelHeight(text, font, width);
 
      label.Height = height;
      label.Width = width;

      if (left == -1)
      {
        label.Left = (panel.Width - label.Width) / 2;
        label.TextAlign = (top == -1) ? ContentAlignment.MiddleCenter : ContentAlignment.TopCenter;
      }
      else
      {
        label.Left = left;
        label.TextAlign = (top == -1) ? ContentAlignment.MiddleLeft : ContentAlignment.TopLeft;
      }

      if (top == -1)
        label.Top = (panel.Height - label.Height) / 2;
      else
        label.Top = top;

      panel.Controls.Add(label);

      return label;
    }

    public static Label AddMultilineLabel(string text, Font font, int top, int left, int height, int width, AnchorStyles anchors, Panel panel)
    {
      return AddMultilineLabel(text, font, Color.Black, top, left, height, width, anchors, panel);
    }


    public static ListBox AddListBox(_Choices choices, Font font, int top, int left, AnchorStyles anchors, Panel panel, int tabidx)
    {
      int gap = 4;
      int maxWidth = 0;
      
      ListBoxPP lstBox = new ListBoxPP();
      lstBox.Anchor = anchors;
      lstBox.Font = font;
      lstBox.TabIndex = tabidx;
      lstBox.BorderStyle = BorderStyle.FixedSingle;
      lstBox.Name = "listBox" + tabidx.ToString();

      foreach (_Choice choice in choices)
      {
        string txt = choice.Text;
        if (txt != "")
        {
          lstBox.Items.Add(txt);
          int itemWidth = Tools.GetLabelWidth(txt, font);
          if (itemWidth > maxWidth)
            maxWidth = itemWidth;
        }  
      }

      if (lstBox.Items.Count == 0)
        return null;

      lstBox.Height = lstBox.PreferredHeight + gap;

      int prefWidth = maxWidth + gap + (lstBox.Width - lstBox.ClientSize.Width); 
      if (prefWidth < panel.Width * 0.8)
      {  
        if (prefWidth < panel.Width * 0.4)
          prefWidth = (int) (panel.Width * 0.4);
        lstBox.Width = prefWidth;
      }
      else
        lstBox.Width = (int) (panel.Width * 0.8);
 
      lstBox.Top = top;
      lstBox.Left = left;

      panel.Controls.Add(lstBox);

      lstBox.SelectedIndexChanged += new System.EventHandler(Preview_Event);

      return lstBox;
    }



    public static RadioButtonPP AddRadioButton(string text, Font font, int top, int left, int width, AnchorStyles anchors, Panel panel, int tabidx, int selidx)
    {
      RadioButtonPP radioBut = new RadioButtonPP();

      radioBut.Anchor = anchors;

      radioBut.Font = font;
      radioBut.TabIndex = tabidx;
      radioBut.SelectedIndex = selidx;
      radioBut.Name = "radioPreview" + tabidx.ToString();

      radioBut.Text = Tools.FixPanelText(text);

      int height = Tools.GetLabelHeight(text, font, width - 10);
      radioBut.Height = height;
      radioBut.Width = width;

      radioBut.Left = left;
      radioBut.Top = top;

      radioBut.TextAlign = ContentAlignment.MiddleLeft;
      radioBut.CheckAlign = ContentAlignment.TopLeft;

      panel.Controls.Add(radioBut);

      radioBut.Click += new System.EventHandler(Preview_Event);

      return radioBut;
    }


    public static NumericUpDownPP AddSpinner(int currvalue, string min, string max, string increment, Font font, int top, int left, AnchorStyles anchors, Panel panel, int tabidx, int selidx)
    {
      // All 3 data items must be present and valid or we can't proceed
      if ((min == null) | (min == "") | (max == null) | (max == "") | (increment == null) | (increment == ""))
        return null;

      if ((Tools.IsWholeNumber(min) == false) | (Tools.IsWholeNumber(max) == false) | (Tools.IsWholeNumber(increment) == false))
        return null;

      NumericUpDownPP spinner = new NumericUpDownPP();

      spinner.Anchor = anchors;
      spinner.Font = font;
      spinner.TabIndex = tabidx;
      spinner.BorderStyle = BorderStyle.FixedSingle;
      spinner.Name = "spinner" + tabidx.ToString();
      spinner.SelectedIndex = selidx;

      spinner.Width = Tools.GetLabelWidth(max.ToString(), font) + 20;
      spinner.Height = spinner.PreferredHeight;
      spinner.Top = top;
      spinner.Left = left;

      spinner.Minimum = Convert.ToDecimal(min);
      spinner.Maximum = Convert.ToDecimal(max);
      spinner.Increment = Convert.ToDecimal(increment);

      if (currvalue >= spinner.Minimum & currvalue <= spinner.Maximum)
        spinner.Value = currvalue;
      else
        spinner.Value = spinner.Minimum;

      panel.Controls.Add(spinner);

      spinner.ValueChanged += new System.EventHandler(Preview_Event);

      return spinner;
    }

    
    // Adds a textbox and accompanying intro text.  Returns the height of vertical space occupied by these new object(s).
    public static int AddTextBox(string text, Font font, int top, int left, int numlines, int width, AnchorStyles anchors, string introText, Panel panel, int tabidx, int selidx)
    {
      //int gap = 4;

      TextBoxPP txtBox = new TextBoxPP();
      
      txtBox.Anchor = anchors;
      txtBox.Font = font;
      txtBox.TabIndex = tabidx;
      txtBox.SelectedIndex = selidx;
      txtBox.Name = "textBox" + tabidx.ToString();

      txtBox.Width = width;
      txtBox.Multiline = (numlines == 1) ? false : true;
      txtBox.Text = text;
      txtBox.Height = (int) (font.Size * 1.625 * numlines);
    
      Label label = null;
      int totHgt = txtBox.Height;  // Default
      if (introText != null & introText != "")
      {
        if (numlines > 1)
        {
          //label = AddLabel(introText, font, top + 1, left, ContentAlignment.MiddleLeft, anchors, panel);
          label = AddMultilineLabel(introText, font, top, left, -1, panel.Width - left * 2, anchors, panel);
          txtBox.Location = new Point(left + 2, top + label.Height + 2);
          txtBox.Width = panel.Width - 2 * (left + 2);
          totHgt = txtBox.Bottom - top;
        }
        else if (left + Tools.GetLabelWidth(introText, font) + width > panel.Width * 0.9)
        {
          label = AddMultilineLabel(introText, font, top, left, -1, panel.Width - left * 2, anchors, panel);
          txtBox.Location = new Point(left + 2, top + label.Height + 1);
          txtBox.Width = label.Width;
          totHgt = txtBox.Bottom - top;
        }
        else
        {
          label = AddLabel(introText, font, top + 2, left, ContentAlignment.MiddleLeft, anchors, panel);
          txtBox.Location = new Point(left + label.Width + 2, top);
        }
      }
      else
        txtBox.Location = new Point(left, top);

      panel.Controls.Add(txtBox);
      txtBox.LostFocus += new System.EventHandler(Preview_Event);

      return totHgt;
    }




    public static event System.EventHandler PreviewEvent;
    private static void Preview_Event(object sender, EventArgs e)
    {
      try
      {
        switch (sender.GetType().Name)
        {
          case "CheckBoxPP":
            if (PreviewEvent != null)
              PreviewEvent(sender, new EventArgs());
            break;

          case "ComboBoxPP":
            if (PreviewEvent != null)
              PreviewEvent(sender, new EventArgs());
            break;

          case "ListBoxPP":
            if (PreviewEvent != null)
              PreviewEvent(sender, new EventArgs());
            break;

          case "NumericUpDownPP":
            if (PreviewEvent != null)
              PreviewEvent(sender, new EventArgs());
            break;

          case "RadioButtonPP":
            if (PreviewEvent != null)
              PreviewEvent(sender, new EventArgs());
            break;

          case "TextBoxPP":
            if (PreviewEvent != null)
              PreviewEvent(sender, new EventArgs());
            break;

          default:
            Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "PanelControls.Preview_Event");
            break;
        }
      }
      catch (Exception ex)
      {
        // Handle exceptions
        Debug.Fail("Error: " + ex.Message, "PanelControls.Preview_Event");
      }
    }
	}


  #region Enhanced Panel Controls

  // Defines a special checkbox for use in Pocket Pollster.
  public class CheckBoxPP : CheckBox
  {
    public CheckBoxPP()
    { 
      AutoCheck = true;
    }

    // Provides the equivalent property to that which exists in Listboxes and Comboboxes.
    private int selectedindex;
    public int SelectedIndex
    {
      get
      {
        return selectedindex;
      }
      set
      {
        selectedindex = value;
      }
    }
  }


  // Defines a special spinner for use in Pocket Pollster.
  // Note: This enhancement isn't currently needed because we only allow one spinner on the whole panel.
  //       But future plans will allow multiple spinners so 'SelectedIndex' will eventually be required.
  public class NumericUpDownPP : NumericUpDown
  {
    public NumericUpDownPP()
    { 
    }

    // Provides the equivalent property to that which exists in Listboxes and Comboboxes.
    private int selectedindex;
    public int SelectedIndex
    {
      get
      {
        return selectedindex;
      }
      set
      {
        selectedindex = value;
      }
    }
  }


  // Defines a special radiobutton for use in Pocket Pollster.
  public class RadioButtonPP : RadioButton
  {
    public RadioButtonPP()
    { 
      AutoCheck = true;
    }

    // Provides the equivalent property to that which exists in Listboxes and Comboboxes.
    private int selectedindex;
    public int SelectedIndex
    {
      get
      {
        return selectedindex;
      }
      set
      {
        selectedindex = value;
      }
    }
  }


  // Defines a special textbox for use in Pocket Pollster.
  public class TextBoxPP : TextBox
  {
    public TextBoxPP()
    { 
      BorderStyle = BorderStyle.FixedSingle;
    }

    // Provides the equivalent property to that which exists in Listboxes and Comboboxes.
    private int selectedindex;
    public int SelectedIndex
    {
      get
      {
        return selectedindex;
      }
      set
      {
        selectedindex = value;
      }
    }
  }


  // Just a wrapper right now so name is consistent with the others.
  public class ComboBoxPP : ComboBox
  {
    public ComboBoxPP()
    { 
    }
  }


  // Just a wrapper right now so name is consistent with the others.
  public class ListBoxPP : ListBox
  {
    public ListBoxPP()
    { 
    }
  }





  #endregion


}
