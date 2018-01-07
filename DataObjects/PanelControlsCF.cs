using System;
using System.Drawing;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using OpenNETCF.Windows.Forms;


namespace DataObjects
{
	/// <summary>
	/// This file had to be introduced because there are too many discrepancies between the CF
	/// versions of the basic controls, even if we're using the enhanced OpenNETCF versions.
	/// </summary>


	public class PanelControlsCF
	{
		public PanelControlsCF()
		{
		}


    public static CheckBoxPP AddCheckBox(string text, Font font, int top, int left, int width, Panel panel, int selidx)
    {
      CheckBoxPP checkBox = new CheckBoxPP();

      checkBox.Font = font;
      checkBox.ForeColor = Color.Blue;
      checkBox.LabelForeColor = Color.Black;
      checkBox.SelectedIndex = selidx;

      checkBox.Location = new Point(left, top);
      checkBox.Text = text;

      int width2 = Tools.GetLabelWidth(text, font);

      int numlines = 1;
      if (width2 < width)
        numlines = width2 / width + 1;                                    // ex. 2.7 -> 3
      else
        numlines = (int) (((float) width2 / width) + 0.25) + 1;           // ex. 2.75 -> 4
      
      int height = 1 + (int) (font.Size * 2 * numlines);

      checkBox.LabelWidth = width;      // This actually sets the width of the LinkLabel
      checkBox.LabelHeight = height;    // Ditto re the height

      panel.Controls.Add(checkBox);
      checkBox.Click += new System.EventHandler(PanelChoices_Event);
      checkBox.LinkLabelClick += new EventHandler(PanelChoices_Event);

      return checkBox;
    }



    public static ComboBoxPP AddComboBox(_Choices choices, Font font, int top, int left, Panel panel)
    {
      ComboBoxPP cboBox = new ComboBoxPP();
      cboBox.Font = font;
      cboBox.ForeColor = Color.Blue;
      cboBox.DropDownStyle = ComboBoxStyle.DropDownList;

      string instruct = "Select an Item";
      cboBox.Items.Add(instruct);
      int maxWidth = Tools.GetLabelWidth(instruct, font);

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

      int prefWidth = maxWidth + 24 + (cboBox.Width - cboBox.ClientSize.Width); 
      if (prefWidth < panel.Width * 0.9)
      {  
        if (prefWidth < panel.Width * 0.4)
          prefWidth = (int) (panel.Width * 0.4);

        cboBox.Width = prefWidth;
      }
      else
        cboBox.Width = (int) (panel.Width * 0.9);
 
      cboBox.Top = top;
      cboBox.Left = left;

      panel.Controls.Add(cboBox);

      cboBox.SelectedIndexChanged += new System.EventHandler(PanelChoices_Event);

      return cboBox;
    }

    
    public static Label AddLabel(string text, Font font, Color color, int top, int xpos, int width, ContentAlignment align, Panel panel)
    {
      Label label = new Label();

      label.Font = font;
      label.Text = text;
      label.ForeColor = color;
      label.TextAlign = align;
      label.Top = top;
      
      if (width == -1)
      {
        label.Width = Tools.GetLabelWidth(text, font) + 2;   // The '2' is necessary to ensure the text doesn't wrap
        label.Height = (int) (font.Size * 2.25);
      }
      else      
      {
        int width2 = Tools.GetLabelWidth(text, font);
        int numLines = (int) (width2 / width) + 1;           // ex. 2.7 -> 3
        label.Width = width;
        int height = 1 + (int) (font.Size * 1.5 * numLines);
        label.Height = height;
      }

      switch (align)
      {
        case ContentAlignment.TopLeft:
          label.Left = xpos;
          break;

        case ContentAlignment.TopCenter:
          label.Left = xpos - (label.Width / 2);
          break;

        case ContentAlignment.TopRight:
          label.Left = xpos - label.Width;
          break;
      }

      panel.Controls.Add(label);

      return label;
    }

    public static Label AddLabel(string text, Font font, Color color, int top, int xpos, ContentAlignment align, Panel panel)
    {
      return AddLabel(text, font, color, top, xpos, -1, align, panel);
    }

    public static Label AddLabel(string text, Font font, int top, int xpos, int width, ContentAlignment align, Panel panel)
    {
      return AddLabel(text, font, Color.Black, top, xpos, width, align, panel);
    }

    public static Label AddLabel(string text, Font font, int top, int xpos, ContentAlignment align, Panel panel)
    {
      return AddLabel(text, font, Color.Black, top, xpos, -1, align, panel);
    }



    /// <summary>
    /// Adds a multiline label to a panel.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="font"></param>
    /// <param name="color"></param>
    /// <param name="top"></param>
    /// <param name="left"></param>
    /// <param name="height"></param>     // If -1 then 'width' will be used as "max width" and the height will be calculated
    /// <param name="width"></param>
    /// <param name="panel"></param>
    /// <returns></returns>
    public static Label AddMultilineLabel(string text, Font font, Color color, int top, int left, int height, int width, Panel panel)
    {
      Label label = new Label();

      label.Font = font;
      label.Text = Tools.FixPanelText(text);

      if (height == -1)
        height = Tools.GetLabelHeight(text, font, width);

      label.Height = height;
      label.Width = width;

      if (left == -1)
      {
        label.Left = (panel.Width - label.Width) / 2;
        //label.TextAlign = (top == -1) ? ContentAlignment.MiddleCenter : ContentAlignment.TopCenter;
      }
      else
      {
        label.Left = left;
        //label.TextAlign = (top == -1) ? ContentAlignment.MiddleLeft : ContentAlignment.TopLeft;
      }

      if (top == -1)
        label.Top = (panel.Height - label.Height) / 2;
      else
        label.Top = top;

      panel.Controls.Add(label);

      return label;
    }



    public static ListBoxPP AddListBox(_Choices choices, Font font, int top, int left, Panel panel, int maxHgt)
    {
      int gap = 4;
      int maxWidth = 0;
      
      ListBoxPP lstBox = new ListBoxPP();
      
      lstBox.Font = font;
      lstBox.ForeColor = Color.Blue;
      lstBox.BackColor = Color.AliceBlue;
      lstBox.EvenItemColor = Color.AliceBlue;
      lstBox.ShowLines = false;
      lstBox.ItemHeight = (int) (font.Size * 2.25);

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

      int prefWidth = maxWidth + gap * 2;
      lstBox.Height = lstBox.ItemHeight * lstBox.Items.Count;

      if (lstBox.Height > maxHgt * 3 / 4)
      {
        lstBox.Height = (int) Math.Round(Convert.ToDouble(maxHgt / 2 / lstBox.ItemHeight)) * lstBox.ItemHeight;
        lstBox.ShowScrollbar = true;
        prefWidth += 14;
      }

      if (prefWidth < panel.Width * 0.9)              // Make sure it's not too wide
      {  
        if (prefWidth < panel.Width * 0.4)            // Make sure it's not too narrow
          prefWidth = (int) (panel.Width * 0.4);

        lstBox.Width = prefWidth;
      }
      else
        lstBox.Width = (int) (panel.Width * 0.9);
 
      lstBox.Top = top;
      lstBox.Left = left;

      panel.Controls.Add(lstBox);

      lstBox.SelectedIndexChanged += new System.EventHandler(PanelChoices_Event);

      return lstBox;
    }


    public static RadioButtonPP AddRadioButton(string text, Font font, int top, int left, int width, Panel panel, int selidx)
    {
      RadioButtonPP radioBut = new RadioButtonPP();

      radioBut.Font = font;
      radioBut.ForeColor = Color.Blue;
      radioBut.LabelForeColor = Color.Black;
      radioBut.SelectedIndex = selidx;

      radioBut.Location = new Point(left, top);
      radioBut.Text = text;

      int width2 = Tools.GetLabelWidth(text, font);

      int numlines = 1;
      if (width2 < width)
        numlines = width2 / width + 1;                                    // ex. 2.7 -> 3
      else
        numlines = (int) (((float) width2 / width) + 0.25) + 1;           // ex. 2.75 -> 4
      
      int height = 1 + (int) (font.Size * 2 * numlines);   // Larger height factor necessary with radioButtons it seems

      radioBut.LabelWidth = width;      // This actually sets the width of the LinkLabel
      radioBut.LabelHeight = height;    // Ditto re the height

      panel.Controls.Add(radioBut);
      radioBut.Click += new System.EventHandler(PanelChoices_Event);
      radioBut.LinkLabelClick += new EventHandler(PanelChoices_Event);

      return radioBut;
    }



    public static NumericUpDownPP AddSpinner(int currvalue, string min, string max, string increment, Font font, int top, int left, Panel panel, int selidx)
    {
      // All 3 data items must be present and valid or we can't proceed
      if ((min == null) | (min == "") | (max == null) | (max == "") | (increment == null) | (increment == ""))
        return null;

      if ((Tools.IsWholeNumber(min) == false) | (Tools.IsWholeNumber(max) == false) | (Tools.IsWholeNumber(increment) == false))
        return null;

      NumericUpDownPP spinner = new NumericUpDownPP();

      //spinner.Font = font;  // Not sure why, but this is crashing method
      spinner.ForeColor = Color.Blue;
      spinner.SelectedIndex = selidx;

      spinner.Width = Tools.GetLabelWidth(max.ToString(), font) + 40;
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

      spinner.ValueChanged += new System.EventHandler(PanelChoices_Event);

      // This second event handler is necessary for the situation where the
      // user types in a number and then hits 'Back' or 'Next'.
      spinner.LostFocus +=  new System.EventHandler(PanelChoices_Event);

      return spinner;
    }



    
    public static int AddTextBox(string text, Font font, int top, int left, int numlines, int width, string introText, Panel panel, int selidx)
    {
      int gap = 4;

      TextBoxPP txtBox = new TextBoxPP();
      
      txtBox.Font = font;
      txtBox.ForeColor = Color.Blue;
      txtBox.SelectedIndex = selidx;
    
      txtBox.Width = width;
      txtBox.Multiline = numlines == 1 ? false : true;
      txtBox.ScrollBars = (txtBox.Multiline) ? ScrollBars.Vertical : ScrollBars.None;
      txtBox.Height = (int) (font.Size * 2.25 * numlines);
      txtBox.Top = top;

      txtBox.Text = text;

      Label label = null;
      int totHgt = txtBox.Height;  // Default

      if (introText != null & introText != "")
      {
        if (numlines > 1)
        {
          label = AddLabel(introText, font, Color.Black, top + 1, left, -1, ContentAlignment.TopLeft, panel);
          txtBox.Location = new Point(left + 2, top + label.Height + gap);
          txtBox.Width = panel.Width - 2 * (left + 2);
          totHgt = txtBox.Bottom - top;
        }
        else if (left + Tools.GetLabelWidth(introText, font) + width > panel.Width * 0.9)
        {
          label = AddMultilineLabel(introText, font, Color.Black, top, left, -1, panel.Width - left * 2, panel);
          txtBox.Location = new Point(left, top + label.Height + gap);
          txtBox.Width = label.Width;
          totHgt = txtBox.Bottom - top;
        }
        else
        {
          label = AddLabel(introText, font, Color.Black, top + 1, left, -1, ContentAlignment.TopLeft, panel);   // Debug: Was 'MiddleLeft' in Desktop Preview
          txtBox.Location = new Point(left + label.Width + 4, top);
        }
      }
      else
        txtBox.Location = new Point(left, top);

      panel.Controls.Add(txtBox);
      txtBox.LostFocus += new System.EventHandler(PanelChoices_Event);

      return totHgt;
    }



    public static event System.EventHandler PanelChoicesEvent;
    private static void PanelChoices_Event(object sender, EventArgs e)
    {
      try
      {
        switch (sender.GetType().Name)
        {
          case "CheckBoxPP":
            if (PanelChoicesEvent != null)
              PanelChoicesEvent(sender, new EventArgs());
            break;

          case "ComboBoxPP":
            if (PanelChoicesEvent != null)
              PanelChoicesEvent(sender, new EventArgs());
            break;

          case "LinkLabelPP":
            if (PanelChoicesEvent != null)
              PanelChoicesEvent(sender, new EventArgs());
            break;

          case "ListBoxPP":
            if (PanelChoicesEvent != null)
              PanelChoicesEvent(sender, new EventArgs());
            break;

          case "NumericUpDownPP":
            if (PanelChoicesEvent != null)
              PanelChoicesEvent(sender, new EventArgs());
            break;

          case "RadioButtonPP":
            if (PanelChoicesEvent != null)
              PanelChoicesEvent(sender, new EventArgs());
            break;

          case "TextBoxPP":
            if (PanelChoicesEvent != null)
              PanelChoicesEvent(sender, new EventArgs());
            break;

          default:
            Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "PanelControlsCF.PanelChoices_Event");
            break;
        }
      }
      catch (Exception ex)
      {
        // Handle exceptions
        Debug.Fail("Error: " + ex.Message, "PanelControlsCF.PanelChoices_Event");
      }
    }
	}



  #region Enhanced Panel Controls

  // This section contains special controls that are to be added to 'frmPoll.panelChoices.Contents'.
  // The original reason they were introduced was to introduce the "SelectedIndex" property to each.
  // This property is equivalent to the Choice Index value.  So, for example, if a question has 4
  // available choices then the first radiobutton will have SelectedIndex = 0, the next one to 1, 
  // and so on.


  public class CheckBoxPP : CheckBoxEx
  {
    public event EventHandler LinkLabelClick;
    LinkLabelPP linkLabel = new LinkLabelPP();


    public CheckBoxPP()
    {
      base.Width = 17;
      base.Height = 17;
      linkLabel.AssocControl = this;
      this.ParentChanged += new EventHandler(CheckBoxPP_Added);
      linkLabel.Click += new EventHandler(LinkLabel_Click);
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
        linkLabel.SelectedIndex = value;
      }
    }


    private string text;
    public override string Text
    {
      get
      {
        return text;
      }
      set
      {
        text = value;
        linkLabel.Text = value;
      }
    }


    private int width;
    public int LabelWidth
    {
      get
      {
        return width;
      }
      set
      {
        width = value;
        linkLabel.Width = value;
      }
    }


    private int height;
    public int LabelHeight
    {
      get
      {
        return height;
      }
      set
      {
        height = value;
        linkLabel.Height = value;
      }
    }


    public override System.Drawing.Font Font
    {
      get
      {
        return base.Font;
      }
      set
      {
        base.Font = value;
        linkLabel.Font = value;
      }
    }


    public Color LabelForeColor
    {
      get
      {
        return linkLabel.ForeColor;
      }
      set
      {
        linkLabel.ForeColor = value;
        linkLabel.ActiveLinkColor = value;
        linkLabel.DisabledLinkColor = value;
        linkLabel.LinkColor = value;
        linkLabel.VisitedLinkColor = value;
        linkLabel.LinkBehavior = LinkBehavior.NeverUnderline;
      }
    }


    protected void LinkLabel_Click(object sender, EventArgs e)
    {
      this.Checked = !this.Checked;   // If the label is clicked then toggle the associated checkbox
      
      if (LinkLabelClick != null)
        LinkLabelClick(sender, new EventArgs());
    }


    // This little event handler does something really neat.  When the CheckBoxPP is
    // added to something (e.g. a Panel or a Form) then it also adds the associated
    // LinkLabel to the same parent.
    private void CheckBoxPP_Added(object sender, EventArgs e)
    {
      if (this.Parent != null)
      {
        this.Parent.Controls.Add(linkLabel);
        linkLabel.Location = new Point(this.Right + 6, this.Top + 2);
      }
    }
  }



  // Just a wrapper right now so name is consistent with the others.
  public class ComboBoxPP : ComboBoxEx
  {
    public ComboBoxPP()
    { 
    }
  }


  // Defines a special LinkLabel control for use in Pocket Pollster.
  public class LinkLabelPP : LinkLabel
  {
    public LinkLabelPP()
    {
      this.ForeColor = Color.Black;
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

    private Control assocControl;
    public Control AssocControl
    {
      get
      {
        return assocControl;
      }
      set
      {
        assocControl = value;
      }
    }
  }



  // Just a wrapper right now so name is consistent with the others.
  public class ListBoxPP : ListBoxEx
  {
    public ListBoxPP()
    { 
    }
  }


  // Defines a special spinner control for use in Pocket Pollster.
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
    public event EventHandler LinkLabelClick;
    LinkLabelPP linkLabel = new LinkLabelPP();


    public RadioButtonPP()
    {
      base.Width = 15;
      base.Height = 15;
      linkLabel.AssocControl = this;
      this.ParentChanged += new EventHandler(RadioButtonPP_Added);
      linkLabel.Click += new EventHandler(LinkLabel_Click);
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
        linkLabel.SelectedIndex = value;
      }
    }


    private string text;
    public override string Text
    {
      get
      {
        return text;
      }
      set
      {
        text = value;
        linkLabel.Text = value;
      }
    }


    private int width;
    public int LabelWidth
    {
      get
      {
        return width;
      }
      set
      {
        width = value;
        linkLabel.Width = value;
      }
    }


    private int height;
    public int LabelHeight
    {
      get
      {
        return height;
      }
      set
      {
        height = value;
        linkLabel.Height = value;
      }
    }


    public override System.Drawing.Font Font
    {
      get
      {
        return base.Font;
      }
      set
      {
        base.Font = value;
        linkLabel.Font = value;
      }
    }


    public Color LabelForeColor
    {
      get
      {
        return linkLabel.ForeColor;
      }
      set
      {
        linkLabel.ForeColor = value;
        linkLabel.ActiveLinkColor = value;
        linkLabel.DisabledLinkColor = value;
        linkLabel.LinkColor = value;
        linkLabel.VisitedLinkColor = value;
        linkLabel.LinkBehavior = LinkBehavior.NeverUnderline;
      }
    }


    protected void LinkLabel_Click(object sender, EventArgs e)
    {
      this.Checked = true;   // If the label is clicked then check the corresponding radio button too
      
      if (LinkLabelClick != null)
        LinkLabelClick(sender, new EventArgs());
    }


    // This little event handler does something really neat.  When the RadioButtonPP is
    // added to something (e.g. a Panel or a Form) then it also adds the associated
    // LinkLabel to the same parent.
    private void RadioButtonPP_Added(object sender, EventArgs e)
    {
      if (this.Parent != null)
      {
        this.Parent.Controls.Add(linkLabel);
        linkLabel.Location = new Point(this.Right + 6, this.Top);
      }
    }
  }



  // Defines a special textbox for use in Pocket Pollster.
  public class TextBoxPP : TextBoxEx
  {
    public TextBoxPP()
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

  #endregion



}
