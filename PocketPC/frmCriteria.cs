using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using DataObjects;



namespace PocketPC
{
  // Define Aliases
  using CFSysInfo = DataObjects.CFSysInfo;
  using Platform = DataObjects.Constants.MobilePlatform;



	/// <summary>
	/// Summary description for frmCriteria.
	/// </summary>
	public class frmCriteria : System.Windows.Forms.Form
	{
    private System.Windows.Forms.MenuItem menuItemBack;
    private System.Windows.Forms.MenuItem menuItemNext;
    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.Label labelInstructions;
    private System.Windows.Forms.ListView listViewSelectDate;            
    private System.Windows.Forms.ListView listViewSelectRecent;
    private System.Windows.Forms.Label labelSequence;
    private System.Windows.Forms.Label labelDate;

    private string PollName;
    private Poll PollModel;
    private int gap = 4;       // A small value, used as the basis for providing horiz and vert spacing
    private int leftMargin;
    private int ScreenAdv;     // -1 : Move backward one screen   +1 : Move forward one screen
    private ArrayList recentCriteria = new ArrayList();
    private ArrayList dateCriteria = new ArrayList();




    public frmCriteria(string pollName, Poll pollModel)
    {
      Initialize(pollName, pollModel, 0, new ArrayList());
    }

    public frmCriteria(string pollName, Poll pollModel, int sequenceVal, ArrayList dateValues)
    {
      Initialize(pollName, pollModel, sequenceVal, dateValues);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pollName"></param>
    /// <param name="pollModel"></param>
    /// <param name="sequenceVal"></param>  // The previously selected Sequence value, if any
    /// <param name="dateValues"></param>   // The previously selected Data values, if any
		private void Initialize(string pollName, Poll pollModel, int sequenceVal, ArrayList dateValues)
		{
      Cursor.Current = Cursors.WaitCursor;
			InitializeComponent();

      PollName = pollName;
      PollModel = pollModel;
      InitializeScreen();

      PopulateForm(pollModel, sequenceVal, dateValues);

      ToolsCF.SipShowIM(0);   // Hide SIP, in case it's visible
      this.BringToFront();
      Cursor.Current = Cursors.Default;

      this.Resize += new System.EventHandler(this.frmCriteria_Resize);
      this.ShowDialog();
      this.Resize -= new System.EventHandler(this.frmCriteria_Resize);

      if (ScreenAdv == 1)
      { 
        // Determine which [single] Sequence value was selected
        foreach(ListViewItem item in listViewSelectRecent.Items)
        {
          if (item.Checked)
          {
            sequenceVal = (int) recentCriteria[item.Index];
            break;
          }
        }


        // Determine which Date(s) were selected
        dateValues = new ArrayList();
        if (! listViewSelectDate.Items[0].Checked)   // If "ALL" was selected then we'll just pass an empty array
        {
          foreach(ListViewItem item in listViewSelectDate.Items)
          {
            if (item.Checked)
              dateValues.Add(dateCriteria[item.Index]);
          }
        }

        // Now that we know what data the user wants to view, we can display it
        frmStats statsForm = new frmStats(PollName, PollModel, sequenceVal, dateValues);
      }
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmCriteria));
      this.mainMenu = new System.Windows.Forms.MainMenu();
      this.menuItemBack = new System.Windows.Forms.MenuItem();
      this.menuItemNext = new System.Windows.Forms.MenuItem();
      this.listViewSelectDate = new System.Windows.Forms.ListView();
      this.labelInstructions = new System.Windows.Forms.Label();
      this.listViewSelectRecent = new System.Windows.Forms.ListView();
      this.labelSequence = new System.Windows.Forms.Label();
      this.labelDate = new System.Windows.Forms.Label();
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
      // listViewSelectDate
      // 
      this.listViewSelectDate.CheckBoxes = true;
      this.listViewSelectDate.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewSelectDate.Location = new System.Drawing.Point(124, 55);
      this.listViewSelectDate.Size = new System.Drawing.Size(102, 152);
      this.listViewSelectDate.View = System.Windows.Forms.View.List;
      this.listViewSelectDate.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewSelectDate_ItemCheck);
      // 
      // labelInstructions
      // 
      this.labelInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelInstructions.ForeColor = System.Drawing.Color.Blue;
      this.labelInstructions.Location = new System.Drawing.Point(14, 8);
      this.labelInstructions.Size = new System.Drawing.Size(200, 16);
      this.labelInstructions.Text = "Specify which data to review:";
      // 
      // listViewSelectRecent
      // 
      this.listViewSelectRecent.CheckBoxes = true;
      this.listViewSelectRecent.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewSelectRecent.Location = new System.Drawing.Point(16, 55);
      this.listViewSelectRecent.Size = new System.Drawing.Size(102, 152);
      this.listViewSelectRecent.View = System.Windows.Forms.View.List;
      this.listViewSelectRecent.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listViewSelectRecent_ItemCheck);
      // 
      // labelSequence
      // 
      this.labelSequence.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelSequence.ForeColor = System.Drawing.Color.Black;
      this.labelSequence.Location = new System.Drawing.Point(13, 37);
      this.labelSequence.Size = new System.Drawing.Size(72, 16);
      this.labelSequence.Text = "Sequence:";
      // 
      // labelDate
      // 
      this.labelDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular);
      this.labelDate.ForeColor = System.Drawing.Color.Black;
      this.labelDate.Location = new System.Drawing.Point(123, 37);
      this.labelDate.Size = new System.Drawing.Size(72, 16);
      this.labelDate.Text = "Date:";
      // 
      // frmCriteria
      // 
      this.BackColor = System.Drawing.Color.LightSteelBlue;
      this.ControlBox = false;
      this.Controls.Add(this.labelDate);
      this.Controls.Add(this.listViewSelectRecent);
      this.Controls.Add(this.labelInstructions);
      this.Controls.Add(this.listViewSelectDate);
      this.Controls.Add(this.labelSequence);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Menu = this.mainMenu;
      this.MinimizeBox = false;
      this.Text = "Pocket Pollster";

    }
		#endregion


    private void InitializeScreen()
    {
      leftMargin = gap * 4;
      RepositionControls(true);
    }


    private void frmCriteria_Resize(object sender, System.EventArgs e)
    {
      RepositionControls(false);    
    }


    private void RepositionControls(bool forceRedraw)
    {
      ToolsCF.SipShowIM(0);       // Ensure that SIP is collapsed
      int wid = 0, hgt = 0, aHgt = 0;

      if ((ToolsCF.UpdateScreenDimensions(out wid, out hgt, out aHgt)) | forceRedraw)
      {
        if (CFSysInfo.Data.DeviceSpecs.Platform == Platform.SmartPhone)
        {
          // To Do: Still to need to flesh out SmartPhone positioning

        }

        else
        {
          //int horizMargin = leftMargin + gap * 2;

          if (wid < hgt)  // Is it in Portrait mode?
          {
            listViewSelectRecent.Left = wid / 2 - gap - listViewSelectRecent.Width;
            listViewSelectRecent.Height = hgt / 2;
            labelSequence.Left = listViewSelectRecent.Left - 1;

            listViewSelectDate.Left = wid / 2 + gap;
            listViewSelectDate.Height = listViewSelectRecent.Height;
            labelDate.Left = listViewSelectDate.Left - 1;
          }

          else  // Landscape mode
          {
            listViewSelectRecent.Left = wid / 2 - gap * 4 - listViewSelectRecent.Width;
            listViewSelectRecent.Height = aHgt - gap * 4 - listViewSelectRecent.Top;
            labelSequence.Left = listViewSelectRecent.Left;

            listViewSelectDate.Left = wid / 2 + gap * 4;
            listViewSelectDate.Height = listViewSelectRecent.Height;
            labelDate.Left = listViewSelectDate.Left;
          }
        }
      }
    }


    private void menuItemBack_Click(object sender, System.EventArgs e)
    {
      ScreenAdv = -1;
      this.Close();
    }


    private void menuItemNext_Click(object sender, System.EventArgs e)
    {
      ScreenAdv = 1;
      this.Close();
    }


    // Populates the 2 listviews with the data appropriate for the current Poll.
    // Also restores the previous state of the two listboxes.
    private void PopulateForm(Poll pollModel, int seqVal, ArrayList dateSelect)
    {
      listViewSelectRecent.Items.Add(new ListViewItem("ALL"));
      recentCriteria.Add(0);

      int numResp = pollModel.Respondents.Count;

      if (numResp > 10)
      {
        listViewSelectRecent.Items.Add(new ListViewItem("First 10"));
        recentCriteria.Add(10);

        if (numResp > 25)
        {
          listViewSelectRecent.Items.Add(new ListViewItem("First 25"));
          recentCriteria.Add(25);

          if (numResp > 50)
          {
            listViewSelectRecent.Items.Add(new ListViewItem("First 50"));
            recentCriteria.Add(50);

            listViewSelectRecent.Items.Add(new ListViewItem("Last 50"));
            recentCriteria.Add(-50);
          }

          listViewSelectRecent.Items.Add(new ListViewItem("Last 25"));
          recentCriteria.Add(-25);
        }

        listViewSelectRecent.Items.Add(new ListViewItem("Last 10"));
        recentCriteria.Add(-10);
      }


      AddListViewItem(listViewSelectDate, "ALL", dateCriteria, 0);
      
      // Need to examine date range of collected responses
      _Respondent resp = pollModel.Respondents[0];
      DateTime date1 = resp.TimeCaptured;
      int maxCnt = pollModel.Respondents.Count - 1;
      resp = pollModel.Respondents[maxCnt];
      DateTime date2 = resp.TimeCaptured;

      if (date1.Date != date2.Date)
      {
        DateTime lastDate = date2.AddDays(1);         // Set an artificial sentinel
        for (int i = maxCnt; i > -1; i--)
        {
          DateTime currDate = pollModel.Respondents[i].TimeCaptured;
          if (currDate.Date != lastDate.Date)
          {
            TimeSpan duration = DateTime.Now.Date - currDate.Date;

            switch (duration.Days)
            {
              case 0:
                AddListViewItem(listViewSelectDate, "Today", dateCriteria, currDate.Date);
                break;

              case 1:
                AddListViewItem(listViewSelectDate, "Yesterday", dateCriteria, currDate.Date);
                break;

              default:
                AddListViewItem(listViewSelectDate, currDate.Date.ToShortDateString(), dateCriteria, currDate.Date);
                break;
            }

            lastDate = currDate;
          }
        }
      }


      // Now preset the item(s) of each listview
      for (int i = 0; i < recentCriteria.Count; i++)
      {
        if (seqVal == (int) recentCriteria[i])
        {
          listViewSelectRecent.Items[i].Checked = true;
          break;
        }
      }

      if (dateSelect.Count == 0)
        listViewSelectDate.Items[0].Checked = true;
      else
      {
        for (int i = 0; i < dateCriteria.Count; i++)
        {
          if (dateSelect.Contains(dateCriteria[i]))
          {
            listViewSelectDate.Items[i].Checked = true;
          }
        }
      }

      listViewSelectRecent.Focus();
    }


    private void AddListViewItem(ListView listView, string text, ArrayList array, int assocVal)
    {
      listView.Items.Add(new ListViewItem(text));
      array.Add(assocVal);
    }

    private void AddListViewItem(ListView listView, string text, ArrayList array, DateTime assocVal)
    {
      listView.Items.Add(new ListViewItem(text));
      array.Add(assocVal);
    }


    /// <summary>
    /// The code in this event handler keeps the first item, "ALL" unique from all the others.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void listViewSelectDate_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
    {
      ListView listView = sender as ListView;
      int idx = e.Index;
    
      if (idx == 0)
      {
        for (int i = 1; i < listView.Items.Count; i++)
        {
          listView.Items[i].Checked = false;
        }

        // We must always have at least one checkbox checked.  Here we prevent the user
        // from trying to explicitly change the 'ALL' checkbox from checked to unchecked.
        if (e.CurrentValue == CheckState.Checked)
          listView.Items[0].Checked = true;
      }
      else if (listView.Items[0].Checked)
        listView.Items[0].Checked = false;

      else if (e.CurrentValue == CheckState.Checked)   // If the user is unchecking a checkbox other than 'ALL'
      {                                                // then we must check whether all the other non-ALL checkboxes
                                                       // are also unchecked.  If so, then 'ALL' must be checked.
        bool noCheckBoxesSet = true;
        for (int i = 1; i < listView.Items.Count; i++)
        {
          if (i != idx && listView.Items[i].Checked)
          {
            noCheckBoxesSet = false;
            break;
          }
        }
      
        if (noCheckBoxesSet)
          listView.Items[0].Checked = true;
      }

      listView.Items[idx].Selected = false;
    }


    /// <summary>
    /// The code in this event handler lets just one item be checked at a time.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void listViewSelectRecent_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
    {
      ListView listView = sender as ListView;
      int idx = e.Index;

      // We must always have at least one checkbox checked.  Here we prevent
      // the user from trying to explicitly uncheck one of the checkboxes.
      if (e.CurrentValue == CheckState.Checked)
        listView.Items[idx].Checked = true;

      for (int i = 0; i < listView.Items.Count; i++)
      {
        if (i != idx)
          listView.Items[i].Checked = false;
      }

      listView.Items[idx].Selected = false;
    }






	}
}
