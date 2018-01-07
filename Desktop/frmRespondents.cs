using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Desktop
{
	/// <summary>
	/// Summary description for frmRespondents.
	/// </summary>
	public class frmRespondents : System.Windows.Forms.Form
	{
    private System.Windows.Forms.Label labelInstructions;
    private PanelGradient.PanelGradient panelGradient1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboBoxGroups;
    private DataObjects.PanelGradient panelInstructions;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.ListView listViewMain;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		public frmRespondents()
		{
			InitializeComponent();
      PopulateForm();
      this.ShowDialog();
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmRespondents));
      this.labelInstructions = new System.Windows.Forms.Label();
      this.panelGradient1 = new PanelGradient.PanelGradient();
      this.listViewMain = new System.Windows.Forms.ListView();
      this.button1 = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.comboBoxGroups = new System.Windows.Forms.ComboBox();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.button4 = new System.Windows.Forms.Button();
      this.panelInstructions = new DataObjects.PanelGradient();
      this.label2 = new System.Windows.Forms.Label();
      this.panelGradient1.SuspendLayout();
      this.panelInstructions.SuspendLayout();
      this.SuspendLayout();
      // 
      // labelInstructions
      // 
      this.labelInstructions.Location = new System.Drawing.Point(0, 0);
      this.labelInstructions.Name = "labelInstructions";
      this.labelInstructions.TabIndex = 0;
      this.labelInstructions.Text = "dsfasd";
      // 
      // panelGradient1
      // 
      this.panelGradient1.Controls.Add(this.listViewMain);
      this.panelGradient1.Controls.Add(this.button1);
      this.panelGradient1.Controls.Add(this.label1);
      this.panelGradient1.Controls.Add(this.comboBoxGroups);
      this.panelGradient1.Controls.Add(this.button2);
      this.panelGradient1.Controls.Add(this.button3);
      this.panelGradient1.Controls.Add(this.button4);
      this.panelGradient1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelGradient1.GradientColorOne = System.Drawing.Color.WhiteSmoke;
      this.panelGradient1.GradientColorTwo = System.Drawing.Color.PaleGreen;
      this.panelGradient1.Location = new System.Drawing.Point(0, 48);
      this.panelGradient1.Name = "panelGradient1";
      this.panelGradient1.Size = new System.Drawing.Size(928, 518);
      this.panelGradient1.TabIndex = 8;
      // 
      // listViewMain
      // 
      this.listViewMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewMain.CheckBoxes = true;
      this.listViewMain.LabelEdit = true;
      this.listViewMain.Location = new System.Drawing.Point(10, 56);
      this.listViewMain.Name = "listViewMain";
      this.listViewMain.Size = new System.Drawing.Size(816, 392);
      this.listViewMain.TabIndex = 7;
      this.listViewMain.View = System.Windows.Forms.View.Details;
      this.listViewMain.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewMain_ItemCheck);
      // 
      // button1
      // 
      this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button1.BackColor = System.Drawing.Color.PaleGreen;
      this.button1.Location = new System.Drawing.Point(839, 104);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(72, 32);
      this.button1.TabIndex = 6;
      this.button1.Text = "Add";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(8, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(89, 16);
      this.label1.TabIndex = 5;
      this.label1.Text = "Respondent List:";
      // 
      // comboBoxGroups
      // 
      this.comboBoxGroups.Location = new System.Drawing.Point(103, 13);
      this.comboBoxGroups.Name = "comboBoxGroups";
      this.comboBoxGroups.Size = new System.Drawing.Size(257, 21);
      this.comboBoxGroups.TabIndex = 3;
      // 
      // button2
      // 
      this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button2.BackColor = System.Drawing.Color.PaleGreen;
      this.button2.Location = new System.Drawing.Point(839, 160);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(72, 32);
      this.button2.TabIndex = 6;
      this.button2.Text = "Remove";
      // 
      // button3
      // 
      this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button3.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(221)), ((System.Byte)(253)), ((System.Byte)(174)));
      this.button3.Location = new System.Drawing.Point(370, 13);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(72, 20);
      this.button3.TabIndex = 6;
      this.button3.Text = "Add";
      // 
      // button4
      // 
      this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.button4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(221)), ((System.Byte)(253)), ((System.Byte)(174)));
      this.button4.Location = new System.Drawing.Point(450, 13);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(72, 20);
      this.button4.TabIndex = 6;
      this.button4.Text = "Remove";
      // 
      // panelInstructions
      // 
      this.panelInstructions.Controls.Add(this.label2);
      this.panelInstructions.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelInstructions.GradientColorOne = System.Drawing.Color.AliceBlue;
      this.panelInstructions.GradientColorTwo = System.Drawing.Color.AliceBlue;
      this.panelInstructions.Location = new System.Drawing.Point(0, 0);
      this.panelInstructions.Name = "panelInstructions";
      this.panelInstructions.Size = new System.Drawing.Size(928, 48);
      this.panelInstructions.TabIndex = 9;
      // 
      // label2
      // 
      this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label2.ForeColor = System.Drawing.Color.Blue;
      this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.label2.Location = new System.Drawing.Point(8, 8);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(912, 36);
      this.label2.TabIndex = 1;
      this.label2.Text = "Before you can use Pocket Pollster, you need to enter your first && last name.  Y" +
        "ou can also set any of the defaults too.  All of this information can be changed" +
        " later.";
      // 
      // frmRespondents
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.BackColor = System.Drawing.Color.MediumSeaGreen;
      this.ClientSize = new System.Drawing.Size(928, 566);
      this.Controls.Add(this.panelGradient1);
      this.Controls.Add(this.panelInstructions);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmRespondents";
      this.ShowInTaskbar = false;
      this.Text = "Respondents";
      this.panelGradient1.ResumeLayout(false);
      this.panelInstructions.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		#endregion


    private void PopulateForm()
    { 
      listViewMain.HeaderStyle = ColumnHeaderStyle.Clickable;
      listViewMain.Columns.Add("", 20, HorizontalAlignment.Center);
      listViewMain.Columns.Add("Last Name", 100, HorizontalAlignment.Left);
      listViewMain.Columns.Add("First Name", 100, HorizontalAlignment.Left);


      ListViewItem lvwItem = new ListViewItem();
      lvwItem.Text = "abcd";
      lvwItem.Checked = true;

      listViewMain.Items.Add(lvwItem);
      listViewMain.Items[0].BackColor = Color.LightBlue;

      lvwItem = new ListViewItem();
      lvwItem.Text = "efgh";
      lvwItem.Checked = false;
      listViewMain.Items.Add(lvwItem);

      lvwItem = new ListViewItem();
      lvwItem.Text = "ijkl";
      lvwItem.Checked = true;
      listViewMain.Items.Add(lvwItem);




    }

    private void listViewMain_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
    {
      int index = e.Index;

      ListViewItem lvwItem = listViewMain.Items[index];

      if (lvwItem.Checked)
        lvwItem.BackColor = Color.Transparent;
      else
        lvwItem.BackColor = Color.LightBlue;
    }




	}
}
