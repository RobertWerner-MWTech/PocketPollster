using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using DataTransfer;
using DataObjects;



namespace Desktop
{
	/// <summary>
	/// Summary description for frmMonitor.
	/// </summary>
	public class frmMonitor : System.Windows.Forms.Form
	{
    private System.Windows.Forms.TextBox textBoxMonitor;
		private System.ComponentModel.Container components = null;

    private int ItemCount = 1;
    private DataObjects.PanelGradient panelInstructions;
    private System.Windows.Forms.Label labelInstructions;
    private PanelGradient.PanelGradient panelMain;
    private PanelGradient.PanelGradient panelInitialInstructions;
    private System.Windows.Forms.Label labelInitialInstructions;
    private System.Windows.Forms.Button buttonClipboardCopy;
    private System.Windows.Forms.Button buttonClose;
    const string newLine = "\r\n";


		public frmMonitor()
		{
			InitializeComponent();
      DataXfer.MonitoringMessageEvent += new DataXfer.MonitoringMessageHandler(DisplayMonitoringMessage);   // May only want to activate when user is ready!
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



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMonitor));
      this.panelMain = new PanelGradient.PanelGradient();
      this.panelInitialInstructions = new PanelGradient.PanelGradient();
      this.labelInitialInstructions = new System.Windows.Forms.Label();
      this.panelInstructions = new DataObjects.PanelGradient();
      this.labelInstructions = new System.Windows.Forms.Label();
      this.textBoxMonitor = new System.Windows.Forms.TextBox();
      this.buttonClose = new System.Windows.Forms.Button();
      this.buttonClipboardCopy = new System.Windows.Forms.Button();
      this.panelMain.SuspendLayout();
      this.panelInitialInstructions.SuspendLayout();
      this.panelInstructions.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelMain
      // 
      this.panelMain.Controls.Add(this.panelInitialInstructions);
      this.panelMain.Controls.Add(this.panelInstructions);
      this.panelMain.Controls.Add(this.textBoxMonitor);
      this.panelMain.Controls.Add(this.buttonClose);
      this.panelMain.Controls.Add(this.buttonClipboardCopy);
      this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMain.GradientColorOne = System.Drawing.Color.PaleTurquoise;
      this.panelMain.GradientColorTwo = System.Drawing.Color.Teal;
      this.panelMain.Location = new System.Drawing.Point(0, 0);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new System.Drawing.Size(800, 534);
      this.panelMain.TabIndex = 7;
      // 
      // panelInitialInstructions
      // 
      this.panelInitialInstructions.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelInitialInstructions.Controls.Add(this.labelInitialInstructions);
      this.panelInitialInstructions.GradientColorOne = System.Drawing.Color.LightGoldenrodYellow;
      this.panelInitialInstructions.GradientColorTwo = System.Drawing.Color.Yellow;
      this.panelInitialInstructions.Location = new System.Drawing.Point(304, 192);
      this.panelInitialInstructions.Name = "panelInitialInstructions";
      this.panelInitialInstructions.Size = new System.Drawing.Size(232, 96);
      this.panelInitialInstructions.TabIndex = 11;
      // 
      // labelInitialInstructions
      // 
      this.labelInitialInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelInitialInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelInitialInstructions.Location = new System.Drawing.Point(0, 0);
      this.labelInitialInstructions.Name = "labelInitialInstructions";
      this.labelInitialInstructions.Size = new System.Drawing.Size(228, 92);
      this.labelInitialInstructions.TabIndex = 0;
      this.labelInitialInstructions.Text = "When you\'re ready, please synchronize your mobile device.";
      this.labelInitialInstructions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // panelInstructions
      // 
      this.panelInstructions.Controls.Add(this.labelInstructions);
      this.panelInstructions.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelInstructions.GradientColorOne = System.Drawing.Color.AliceBlue;
      this.panelInstructions.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.panelInstructions.Location = new System.Drawing.Point(0, 0);
      this.panelInstructions.Name = "panelInstructions";
      this.panelInstructions.Size = new System.Drawing.Size(800, 34);
      this.panelInstructions.TabIndex = 10;
      // 
      // labelInstructions
      // 
      this.labelInstructions.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.labelInstructions.ForeColor = System.Drawing.Color.Blue;
      this.labelInstructions.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.labelInstructions.Location = new System.Drawing.Point(8, 8);
      this.labelInstructions.Name = "labelInstructions";
      this.labelInstructions.Size = new System.Drawing.Size(736, 16);
      this.labelInstructions.TabIndex = 1;
      this.labelInstructions.Text = "This screen allows you to monitor the entire Data Transfer process.";
      // 
      // textBoxMonitor
      // 
      this.textBoxMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxMonitor.BackColor = System.Drawing.Color.LightGray;
      this.textBoxMonitor.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.textBoxMonitor.Location = new System.Drawing.Point(8, 40);
      this.textBoxMonitor.Multiline = true;
      this.textBoxMonitor.Name = "textBoxMonitor";
      this.textBoxMonitor.Size = new System.Drawing.Size(784, 440);
      this.textBoxMonitor.TabIndex = 9;
      this.textBoxMonitor.Text = "";
      // 
      // buttonClose
      // 
      this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonClose.BackColor = System.Drawing.SystemColors.Control;
      this.buttonClose.Location = new System.Drawing.Point(736, 504);
      this.buttonClose.Name = "buttonClose";
      this.buttonClose.Size = new System.Drawing.Size(56, 24);
      this.buttonClose.TabIndex = 8;
      this.buttonClose.Text = "Close";
      this.buttonClose.Click += new System.EventHandler(this.frmMonitor_Closed);
      // 
      // buttonClipboardCopy
      // 
      this.buttonClipboardCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonClipboardCopy.BackColor = System.Drawing.SystemColors.Control;
      this.buttonClipboardCopy.Location = new System.Drawing.Point(10, 504);
      this.buttonClipboardCopy.Name = "buttonClipboardCopy";
      this.buttonClipboardCopy.Size = new System.Drawing.Size(112, 24);
      this.buttonClipboardCopy.TabIndex = 8;
      this.buttonClipboardCopy.Text = "Copy To Clipboard";
      this.buttonClipboardCopy.Click += new System.EventHandler(this.buttonClipboardCopy_Click);
      // 
      // frmMonitor
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(800, 534);
      this.Controls.Add(this.panelMain);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "frmMonitor";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Data Transfer Monitoring";
      this.Resize += new System.EventHandler(this.frmMonitor_Resize);
      this.Load += new System.EventHandler(this.frmMonitor_Load);
      this.panelMain.ResumeLayout(false);
      this.panelInitialInstructions.ResumeLayout(false);
      this.panelInstructions.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		#endregion


    private void frmMonitor_Load(object sender, System.EventArgs e)
    {
      textBoxMonitor.Visible = false;
      panelInitialInstructions.Visible = true;
      Tools.CenterControl(panelInitialInstructions);
    }


    private void frmMonitor_Resize(object sender, System.EventArgs e)
    {
      if (panelInitialInstructions.Visible)
        Tools.CenterControl(panelInitialInstructions);
    }


    /// <summary>
    /// Displays a message in the center left of the status bar.  Messages may be coming from
    /// the Data Transfer thread so we have to ensure they're placed on the correct thread.
    /// </summary>
    private delegate void DisplayMonitoringMessageDelegate(string text);
    private void DisplayMonitoringMessage(string text)
    {
      if (this.InvokeRequired)
      {
        this.BeginInvoke(new DisplayMonitoringMessageDelegate(DisplayMonitoringMessage), new object[] {text});
        return;
      }

      // Only reaches here when on UI thread
      if (panelInitialInstructions.Visible)
      {
        panelInitialInstructions.Visible = false;
        textBoxMonitor.Visible = true;
      }

      string fullText = (ItemCount < 10) ? " " + ItemCount.ToString() : ItemCount.ToString();
      fullText += ".  " + DateTime.Now.ToLongTimeString();      
      fullText += "   " + text + newLine;
      textBoxMonitor.Text += fullText;
      ItemCount++;
    }



    private void frmMonitor_Closed(object sender, System.EventArgs e)
    {
      this.Close();
    }


    private void buttonClipboardCopy_Click(object sender, System.EventArgs e)
    {
      Color oldColor = textBoxMonitor.BackColor;
      textBoxMonitor.BackColor = Color.Navy;
      textBoxMonitor.Refresh();
      Clipboard.SetDataObject(textBoxMonitor.Text, true);
      System.Threading.Thread.Sleep(500);
      textBoxMonitor.BackColor = oldColor;
    }






	}
}
