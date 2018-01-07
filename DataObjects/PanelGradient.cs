using System;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;


namespace DataObjects
{
	/// <summary>
	/// Provides a custom panel control that displays a gradient effect using two colours.
	/// </summary>
	public class PanelGradient : Panel
	{
		public PanelGradient()
		{
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);   // Not sure if this is necessary

      // Activates double buffering - necessary to avoid flicker
      SetStyle(ControlStyles.UserPaint, true);
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      SetStyle(ControlStyles.DoubleBuffer, true); 
		}

 
    #region Implementation Member Fields
    protected Color gradientColorOne = Color.White;
    protected Color gradientColorTwo = Color.Blue;
    protected LinearGradientMode lgm = LinearGradientMode.ForwardDiagonal;
    //protected Border3DStyle b3dstyle = Border3DStyle.Bump;
    #endregion

    #region GradientColorOne Properties
    [
    DefaultValue(typeof(Color),"White"),
    Description("The first gradient color."),
    Category("Appearance"),
    ]

      //GradientColorOne Properties
    public Color GradientColorOne
    {
      get 
      {
        return gradientColorOne;
      }
      set
      {
        gradientColorOne = value;
        Invalidate();
      }
    }
    #endregion
		
    #region GradientColorTwo Properties
    [
    DefaultValue(typeof(Color),"Blue"),
    Description("The second gradient color."),
    Category("Appearance"),
    ]

      //GradientColorTwo Properties
    public Color GradientColorTwo
    {
      get 
      {
        return gradientColorTwo;
      }
      set
      {
        gradientColorTwo = value;
        Invalidate();
      }
    }

    #endregion

    #region LinearGradientMode Properties
    //LinearGradientMode Properties
    [
    DefaultValue(typeof(LinearGradientMode),"ForwardDiagonal"),
    Description("Gradient Mode"),
    Category("Appearance"),
    ]

    public LinearGradientMode GradientMode
    {
      get 
      {
        return lgm;
      }
			
      set
      {
        lgm = value;
        Invalidate();
      }
    }
    #endregion

    #region Removed Properties
		
    // Remove BackColor Property
    [
    Browsable(false),
    EditorBrowsable(EditorBrowsableState.Never)
    ]
    public override System.Drawing.Color BackColor
    {
      get	
      {
        return new System.Drawing.Color();
      }
      set	{;}
    }
		
    #endregion


    protected override void OnPaintBackground(PaintEventArgs e)
    {
      if (e.ClipRectangle.IsEmpty)
        return;

      using(LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, this.GradientColorOne, this.GradientColorTwo, this.GradientMode))
      {
        e.Graphics.FillRectangle(lgb, this.ClientRectangle);
      }
    }

    protected override void OnSizeChanged(EventArgs e)
    {
      base.OnSizeChanged (e);
      this.Invalidate();
    }

	}
}
