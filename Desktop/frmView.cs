using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

using DataObjects;


namespace Desktop
{
  // Define Aliases
  using ViewMode = DataObjects.Constants.ViewMode;


	/// <summary>
	/// Summary description for frmView.
	/// </summary>
	public class frmView : System.Windows.Forms.Form
	{
    public ViewMode FormType;
    private int checkBoxCount = 0;   // The # of checkboxes that have been checked

		private System.ComponentModel.Container components = null;
    private DataObjects.PanelGradient panelMain;
    private System.Windows.Forms.Button buttonDelete;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Label labelInstructions;
    private System.Windows.Forms.ListView listViewMain;


		public frmView()
		{
			InitializeComponent();
		}


    /// <summary>
    /// This constructor allows us to populate this form with multiple kinds of info.
    /// </summary>
    /// <param name="viewMode"></param>
    public frmView(ViewMode viewMode)
    {
      InitializeComponent();
      PopulateForm(listViewMain, viewMode);
      FormType = viewMode;
    }


    /// <summary>
    /// This constructor allows us to set the initial starting location of the form.
    /// </summary>
    /// <param name="viewMode"></param>
    /// <param name="ulCorner"></param>
    public frmView(ViewMode viewMode, Point ulCorner)
    {
      InitializeComponent();
      PopulateForm(listViewMain, viewMode);
      FormType = viewMode;

      this.StartPosition = FormStartPosition.Manual;
      this.Location = ulCorner;
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmView));
      this.panelMain = new DataObjects.PanelGradient();
      this.buttonOK = new System.Windows.Forms.Button();
      this.listViewMain = new System.Windows.Forms.ListView();
      this.buttonDelete = new System.Windows.Forms.Button();
      this.labelInstructions = new System.Windows.Forms.Label();
      this.panelMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelMain
      // 
      this.panelMain.Controls.Add(this.labelInstructions);
      this.panelMain.Controls.Add(this.buttonOK);
      this.panelMain.Controls.Add(this.listViewMain);
      this.panelMain.Controls.Add(this.buttonDelete);
      this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelMain.GradientColorOne = System.Drawing.Color.Lavender;
      this.panelMain.GradientColorTwo = System.Drawing.Color.MediumSlateBlue;
      this.panelMain.Location = new System.Drawing.Point(0, 0);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new System.Drawing.Size(960, 614);
      this.panelMain.TabIndex = 2;
      // 
      // buttonOK
      // 
      this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOK.BackColor = System.Drawing.SystemColors.Control;
      this.buttonOK.Location = new System.Drawing.Point(888, 581);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.buttonOK.Size = new System.Drawing.Size(64, 24);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "OK";
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // listViewMain
      // 
      this.listViewMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewMain.CheckBoxes = true;
      this.listViewMain.Location = new System.Drawing.Point(8, 32);
      this.listViewMain.Name = "listViewMain";
      this.listViewMain.Size = new System.Drawing.Size(944, 536);
      this.listViewMain.TabIndex = 2;
      this.listViewMain.View = System.Windows.Forms.View.Details;
      this.listViewMain.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewMain_ColumnClick);
      this.listViewMain.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewMain_ItemCheck);
      // 
      // buttonDelete
      // 
      this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonDelete.BackColor = System.Drawing.SystemColors.Control;
      this.buttonDelete.Location = new System.Drawing.Point(9, 581);
      this.buttonDelete.Name = "buttonDelete";
      this.buttonDelete.Size = new System.Drawing.Size(139, 24);
      this.buttonDelete.TabIndex = 3;
      this.buttonDelete.Text = "Delete Selected";
      this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
      // 
      // labelInstructions
      // 
      this.labelInstructions.AutoSize = true;
      this.labelInstructions.Location = new System.Drawing.Point(8, 10);
      this.labelInstructions.Name = "labelInstructions";
      this.labelInstructions.Size = new System.Drawing.Size(62, 16);
      this.labelInstructions.TabIndex = 4;
      this.labelInstructions.Text = "Instructions";
      // 
      // frmView
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(960, 614);
      this.ControlBox = false;
      this.Controls.Add(this.panelMain);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "frmView";
      this.ShowInTaskbar = false;
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "frmView";
      this.TopMost = true;
      this.panelMain.ResumeLayout(false);
      this.ResumeLayout(false);

    }
		#endregion



    /// <summary>
    /// Populates the form with the appropriate data.
    /// </summary>
    /// <param name="listView"></param>
    /// <param name="viewMode"></param>
    private void PopulateForm(ListView listView, ViewMode viewMode)
    {
      listView.View = View.Details;
      listView.FullRowSelect = true;
      listView.AllowColumnReorder = true;
      listView.Sorting = SortOrder.None;
      listView.HeaderStyle = ColumnHeaderStyle.Clickable;

      labelInstructions.Text = "You can delete a " + viewMode.ToString().Substring(0, viewMode.ToString().Length - 1) + " by checking its box on the left and pressing the 'Delete' button.";

      switch (viewMode)
      {
        case ViewMode.Users:
          panelMain.GradientColorOne = Color.LightGreen;
          panelMain.GradientColorTwo = Color.SeaGreen;

          listView.Columns.Add("", 22, HorizontalAlignment.Center);                  // For the Delete checkbox
          listView.Columns.Add("Username ", 90, HorizontalAlignment.Left);
          listView.Columns.Add("First Name ", 90, HorizontalAlignment.Left);
          listView.Columns.Add("Last Name ", 90, HorizontalAlignment.Left);
          listView.Columns.Add("Creation Date ", 100, HorizontalAlignment.Left);
          listView.Columns.Add("Creation Time ", 100, HorizontalAlignment.Left);

          foreach (_User user in SysInfo.Data.Users)
          {
            ListViewItem item = new ListViewItem();
            
            if (user.Name == SysInfo.Data.Options.PrimaryUser)
              item.ForeColor = Color.Blue;

            item.Text = "";
            item.SubItems.AddRange(new string[] {user.Name, user.FirstName, user.LastName, user.CreationDate.ToShortDateString(), user.CreationDate.ToShortTimeString()});
            listView.Items.Add(item);
          }

          buttonDelete.Text = "Delete Selected Users";
          buttonDelete.Enabled = false;
          break;

        case ViewMode.Devices:
          panelMain.GradientColorOne = Color.Lavender;
          panelMain.GradientColorTwo = Color.MediumSlateBlue;
          
          listView.Columns.Add("", 22, HorizontalAlignment.Center);                  // For the Delete checkbox
          listView.Columns.Add("Primary User ", 75, HorizontalAlignment.Left);
          listView.Columns.Add("OS Version ", 75, HorizontalAlignment.Left);
          listView.Columns.Add("PP Version ", 100, HorizontalAlignment.Left);
          listView.Columns.Add("Last Update ", 120, HorizontalAlignment.Left);
          listView.Columns.Add("Last Sync ", 120, HorizontalAlignment.Left);

          foreach (_Device device in SysInfo.Data.Devices)
          {
            ListViewItem item = new ListViewItem();
            item.Text = "";
            item.SubItems.AddRange(new string[] {device.PrimaryUser, device.OSVersion, device.SoftwareVersion, device.LastUpdate.ToShortDateString() + " " + device.LastUpdate.ToShortTimeString(), device.LastSync.ToShortDateString() + " " + device.LastSync.ToShortTimeString()});
            listView.Items.Add(item);
          }

          buttonDelete.Text = "Delete Selected Devices";
          buttonDelete.Enabled = false;
          break;

        default:
          Debug.Fail("Unaccounted for ViewMode: " + viewMode.ToString(), "frmView.PopulateForm");
          break;
      }

      this.Text = "View " + viewMode.ToString();

      int totWidth = 0;
      for (int i = 0; i < listView.Columns.Count; i++)
      {
        totWidth += listView.Columns[i].Width + 2;
      }

      this.Width = listView.Left * 2 + totWidth + 2;
      this.Height = Screen.PrimaryScreen.WorkingArea.Height / 2;
      int minHgt = (int) (SysInfo.Data.Users.Count * listView.Font.Height * 1.3 + 150);
      minHgt = Math.Min(this.Height, minHgt);
      this.MinimumSize = new Size(this.Width, minHgt);

      this.Show();
    }


    private void listViewMain_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column == 0)
      {
        if (checkBoxCount < listViewMain.Items.Count)
          checkBoxCount = listViewMain.Items.Count;
        else
          checkBoxCount = 0;
        
        foreach (ListViewItem row in listViewMain.Items)
        {
          row.Checked = (checkBoxCount == 0) ? false : true;
        }
      }
      else
        // Set the ListViewItemSorter property to a new ListViewItemComparer object. 
        // Setting this property immediately sorts the ListView using the ListViewItemComparer object.
        listViewMain.ListViewItemSorter = new ListViewItemComparer(e.Column);
    }



    // Implements the manual sorting of items by columns.
    // RW: Not sure how to reverse sort with a right-click
    class ListViewItemComparer : IComparer
    {
      private int col;
      public ListViewItemComparer()
      {
        col = 0;
      }

      public ListViewItemComparer(int column)
      {
        col = column;
      }

      public int Compare(object x, object y)
      {
        return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
      }
    }


    public delegate void ButtonOKEventHandler (ViewMode viewMode, EventArgs e);
    public event ButtonOKEventHandler ButtonOKEvent;

    private void buttonOK_Click(object sender, System.EventArgs e)
    {
      if (ButtonOKEvent != null)
        ButtonOKEvent(FormType, new EventArgs());

      this.Close();
    }


    private void listViewMain_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
    {
      if (e.NewValue == CheckState.Checked)
      {
        if (checkBoxCount < listViewMain.Items.Count)
          checkBoxCount++;
      }
      else
      {
        if (checkBoxCount > 0)
          checkBoxCount--;
      }

      buttonDelete.Enabled = (checkBoxCount > 0) ? true : false;
    }


    private void buttonDelete_Click(object sender, System.EventArgs e)
    {
      string noun = (FormType == ViewMode.Users) ? "user" : "device";
      string suffix = ((checkBoxCount == 1) ? "this " + noun : "these " + checkBoxCount.ToString() + " " + noun + "s") + "?";
      string msg = "Are you absolutely sure that you want to delete " + suffix;

      if (Tools.ShowMessage(msg, "Deletion Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        for (int i = listViewMain.Items.Count - 1; i >= 0; i--)
        {
          if (listViewMain.Items[i].Checked)
          {
            switch (FormType)
            {
              case ViewMode.Users:
                if (listViewMain.Items[i].SubItems[1].Text == SysInfo.Data.Options.PrimaryUser)
                  SysInfo.Data.Options.PrimaryUser = "";                
                
                SysInfo.Data.Users.RemoveAt(i);
                listViewMain.Items.RemoveAt(i);
                break;

              case ViewMode.Devices:
                SysInfo.Data.Devices.RemoveAt(i);
                listViewMain.Items.RemoveAt(i);
                break;
            }
          }
        }
      }
    }


	}
}
