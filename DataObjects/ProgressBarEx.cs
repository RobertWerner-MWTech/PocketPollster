using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace DataObjects
{
	/// <summary>
	/// Represents an extended Windows progress bar control.
	/// </summary>
	[DefaultProperty("PercentageMode")]
	public class ProgressBarEx : System.Windows.Forms.UserControl
	{
		private int maxValue;								                		// Maximum value
		private int minValue;						                				// Minimum value
		private float _value;					            	    				// Value property value
		private int stepValue;				           		    				// Step value
		private float percentageValue;	      				    			// Percent value
		private int drawingWidth;			                        	// Drawing width according to the logical Value property
		private Color drawingColor;				        		    			// Color used for drawing activities
		private ColorBlend gradientBlender;	    				    		// Color mixer object
		private PercentageDrawingMode percentageDrawingMode;		// Percent Drawing type
		private SolidBrush writingBrush;						          	// Percent writing brush
		private Font writingFont;						              			// Font to write Percent with
		private LinearGradientBrush _Drawer;


		/// <summary>
		///  Specifies how the percentage value should be drawn
		/// </summary>
		public enum PercentageDrawingMode : int
		{
			None = 0,		// No Percentage shown
			Center,			// Percentage alwayes centered
			Movable		  // Percentage moved with the progress activities
		}
		
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
			
		/// <summary>
		/// Initialize new instance of the ProgressBarEx control
		/// </summary>
		public ProgressBarEx()
		{
			InitializeComponent();		// Designer stuff

			maxValue = this.Width;
			minValue = 0;
			stepValue = 1;
			percentageDrawingMode = PercentageDrawingMode.Center;

			// Color Mixer contain 3 colors for (top, center, bottom)
			gradientBlender = new ColorBlend(3);

			// Position of mixing pints is (top, middle, bottom)
			gradientBlender.Positions = new float[]{0.0F, 0.5F, 1.0F};

			DrawingColor = Color.Blue;
			writingBrush = new SolidBrush(Color.Black);
			writingFont = new Font("Arial", 10, FontStyle.Bold);

			_Drawer = new LinearGradientBrush(this.ClientRectangle, Color.Black, Color.White, LinearGradientMode.Vertical);
			
      // Cancel Reflection while drawing
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
			
      // Allow Transparent backcolor
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true); 

			this.BackColor = Color.Transparent;
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
				
			writingBrush.Dispose();			// Release Percentage writer brush
			writingFont.Dispose();			// Release Percentage font
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// ProgressBarEx
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Name = "ProgressBarEx";
			this.Size = new System.Drawing.Size(256, 24);
			this.Resize += new System.EventHandler(this.ProgressBarEx_Resize);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ProgressBarEx_Paint);
		}
		#endregion

		
		private void ProgressBarEx_Resize(object sender, System.EventArgs e)
		{
			if(this.Height > 40)
				this.Height = 40;
			if(this.Height < 15)
				this.Height = 15;	  	// Prevent a Progress shorter than its own percentage
			if(this.Width < 50)
				this.Width = 50;	  	// Prevent a Progress smaller than its own percentage
			this.Refresh();
		}

		private void ProgressBarEx_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// Why to draw outside the control !! ?
			if(_value != 0 & _value <= maxValue)
			{
				// Calculate the right side of drawing edge
				drawingWidth = (int) (this.Width * _value) / (maxValue - minValue);

				// Calculate the Percentage according to the logical value reached 
				percentageValue = (_value / maxValue) * 100;
					
				// Tie our color mixer with the just created brush
				_Drawer.InterpolationColors = gradientBlender;

				// Now we ready to draw, so do the actual drawing
				e.Graphics.FillRectangle(_Drawer, 0, 0, drawingWidth, this.Height);

				// Prepare for Percentage writing only when required
				if (percentageDrawingMode != PercentageDrawingMode.None)
				{
					string st = ((int) percentageValue).ToString() + "%";

					// Calculate Percentage rectangle size
					SizeF s = e.Graphics.MeasureString(st, writingFont);

					if (percentageDrawingMode == PercentageDrawingMode.Movable)
					{
						//e.Graphics.DrawString(st, writingFont, writingBrush, drawingWidth, (e.ClipRectangle.Height / 2 - s.Height / 2));
            e.Graphics.DrawString(st, writingFont, writingBrush, drawingWidth + 1, (e.ClipRectangle.Height / 2 - s.Height / 2));  // RW: Adds a little gap
					}
					else if (percentageDrawingMode == PercentageDrawingMode.Center)
					{
						e.Graphics.DrawString(st, writingFont, writingBrush,
						                    	new PointF((e.ClipRectangle.Width / 2 - s.Width / 2),
					                    		(e.ClipRectangle.Height / 2 - s.Height / 2)));
					}
				}
			}
		}


		/// <summary>
		/// Increment the progress one step
		/// </summary>
		public void StepForward()
		{
			if ((_value + stepValue) < maxValue)		// If valid, increment the value by step size
			{
				_value += stepValue;
				this.Refresh();
			}
			else									// If not, don't exceed the maximum allowed
			{
				_value = maxValue;					
				this.Refresh();
			}
		}


		/// <summary>
		/// Decrement the progress one step
		/// </summary>
		public void StepBackward()
		{
			if((_value - stepValue) > minValue)		// If valid, decrement the value by step size
			{
				_value -= stepValue;
				this.Refresh();
			}
			else							 		// If not, don't exceed the minimum allowed
			{		
				_value = minValue;					
				this.Refresh();
			}
		}



		/// <summary>
		/// Gets or Sets a value determine how to display Percentage value
		/// </summary>
		[Category("Behavior"),Description("Specify how to display the Percentage value")]
		public PercentageDrawingMode PercentageMode
		{
			get
			{
				return percentageDrawingMode;
			}
			set
			{
				percentageDrawingMode = value;
				this.Refresh();
			}
		}



		/// <summary>
		/// Gets or Sets the color used to draw the Progress activities
		/// </summary>
		[Category("Appearance"),Description("Specify the color used to draw the progress activities")]
		public Color DrawingColor
		{
			get
			{
				return drawingColor;
			}
			set
			{
				drawingColor = value;

				// If assigned then remix the colors used for gradient display
				gradientBlender.Colors[0] = ControlPaint.Dark(value);
				gradientBlender.Colors[1] = ControlPaint.Light(value);
				gradientBlender.Colors[2] = ControlPaint.Dark(value);
				this.Refresh();
			}
		}



		/// <summary>
		///  Gets or sets the maximum value of the range of the control. 
		/// </summary>
		[Category("Layout"),Description("Specify the maximum value the progress can increased to")]
		public int Maximum
		{
			get
			{
				return maxValue;
			}
			set
			{
				maxValue = value;
				this.Refresh();
			}
		}



		/// <summary>
		/// Gets or sets the minimum value of the range of the control.
		/// </summary>
		[Category("Layout")]
		public int Minimum
		{
			get
			{
				return minValue;
			}
			set
			{
				minValue = value;
				this.Refresh();
			}
		}



		/// <summary>
		///  Gets or sets the amount by which a call to the System.Windows.Forms.ProgressBar.
		///  StepForward method increases the current position of the progress bar.
		/// </summary>
		[Category("Layout")]
		public int Step
		{
			get
			{
				return stepValue;
			}
			set
			{
				stepValue = value;
				this.Refresh();
			}
		}



		/// <summary>
		/// Gets or sets the current position of the progress bar. 
		/// </summary>
		/// <exception cref="System.ArgumentException">The value specified is greater than the value of
		/// the System.Windows.Forms.ProgressBar.Maximum property.  -or- The value specified is less
		/// than the value of the System.Windows.Forms.ProgressBar.Minimum property</exception>
		[Category("Layout")]
		public int Value
		{
			get
			{
				return (int)_value;
			}
			set
			{
				// Protect the value and refuse any invalid values
				// 
				// Here we may just handle invalid values and dont bother the client by exceptions
				if(value > maxValue | value < minValue)
				{
					throw new ArgumentException("Invalid value used");
				}
				_value = value;
				this.Refresh();
			}
		}



		/// <summary>
		/// Gets the Percent value the Progress activities reached
		/// </summary>
		public int Percent
		{
			get
			{
				// Its float value, so to be accurate round it then return
				return (int)Math.Round(percentageValue);
			}
		}

		
	
		// This property exist in the parent, ovveride it for our own good
		/// <summary>
		/// Gets or Sets the color used to draw the Precentage value
		/// </summary>
		[Category("Appearance"),Description("Specify the color used to draw the Percentage value")]
		public override Color ForeColor
		{
			get
			{
				return writingBrush.Color;
			}
			set
			{
				writingBrush.Color = value;
				this.Invalidate(false);
			}
		}



	}
}
