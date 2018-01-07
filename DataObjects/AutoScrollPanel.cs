using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;



namespace DataObjects
{
	/// <summary>
	/// Summary description for AutoScrollPanel.
	/// </summary>
  public class AutoScrollPanel : Panel
  {
    public Panel Contents
    {
      get 
      {
        return contents; 
      }
    }

    Panel contents;
    VScrollBar vScroll;


    public AutoScrollPanel()
    {
      Initialize(Color.LightSteelBlue);
    }

    public AutoScrollPanel(Color backColor)
    {
      Initialize(backColor);
    }

    public void Initialize(Color backColor)
    {
      // Create a scroll bar
      this.vScroll = new VScrollBar();
      this.vScroll.Parent = this;
      this.vScroll.Visible = true;
      this.vScroll.Minimum = 0;
      this.vScroll.SmallChange = 20;
      this.vScroll.ValueChanged += new EventHandler (this.scrollbar_ValueChanged);
      this.vScroll.BackColor = backColor;
      this.BackColor = backColor;  // Removes a white line at the bottom of the panel

      // Create the contents panel that holds the controls to be scrolled
      this.contents = new Panel();
      this.contents.Parent = this;
      this.contents.Width = this.ClientSize.Width - this.vScroll.Size.Width;
      this.contents.BackColor = backColor;
    }


    private void scrollbar_ValueChanged (object obj, EventArgs e)
    {
      if (obj == this.vScroll)
      {
        //By decreasing the top y coordinate
        //the contents panel appears to scroll
        this.contents.Top = -this.vScroll.Value;
        this.Update();
      }
    }


    private void CheckScrollBars()
    {
      bool oldVisibility = this.vScroll.Visible;
      this.vScroll.Visible = this.contents.Size.Height > this.ClientSize.Height;

      if (oldVisibility && !this.vScroll.Visible)  // If the scrollbars were visible but aren't any longer
        this.contents.Top = 0;                     // Then need to reset location of controls or some may be hidden
    }


    protected override void OnResize (EventArgs e)
    {
      this.contents.Width = this.ClientSize.Width - this.vScroll.Size.Width;

      this.vScroll.Bounds = new Rectangle (this.ClientSize.Width - this.vScroll.Size.Width,
          0,
          this.vScroll.Size.Width,
          this.ClientSize.Height);

      if (this.ClientSize.Height >= 0)
        this.vScroll.LargeChange = this.ClientSize.Height;

      CheckScrollBars();
    }


    public void SetScrollHeight(int height)
    {
      this.contents.Top = 0;   // Necessary addition or panel may remain scrolled out of position
      this.contents.Height = height;
      this.vScroll.Maximum = this.contents.Size.Height;
      CheckScrollBars();
    }

    
    // Debug: Experimental code
    
    public int GetTopMargin()
    {
      return this.contents.Top;
    }

    public void SetTopMargin(int topMargin)
    {
      this.contents.Top = topMargin;
      this.vScroll.Value = - topMargin;   // Need to set this one too!
      //CheckScrollBars();      
      this.Update();
    }

    public bool IsScrollBarsVisible()
    {
      CheckScrollBars();
      return this.vScroll.Visible;
    }







  }
}
