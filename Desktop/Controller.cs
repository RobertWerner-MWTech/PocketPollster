using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using DataObjects;


namespace Desktop
{
  // To Do: To get this code working we've moved Controller.cs into the Desktop project but ideally 
  //        we'd like to have all/part of this code in the common DataObjects project.



  // Define Aliases
  using ASMinfo = DataObjects.ASMinfo;
  using ASMEventHandler = DataObjects.Poll.ASMEventHandler;
  using ASMCollectionEventHandler = DataObjects.Poll.ASMCollectionEventHandler;
  using CurrRespondentEventHandler = frmPoll.CurrRespondentEventHandler;

  using DragDropEventHandler = frmPoll.DragDropEventHandler;

  using NodeInfo = DataObjects.Tools.NodeInfo;
  using AnswerFormat = DataObjects.Constants.AnswerFormat;
  using PurgeDuration = DataObjects.Constants.PurgeDuration;
  using PublishedLock = DataObjects.Constants.PublishedLock;


  /// <summary>
  /// Summary description for Controller.
  /// </summary>
  public class ControllerClass
  {
    public Poll pollModel;            // Define new Model (ie. Poll class)
    public frmPoll pollForm;          // Define View
    
    public bool newPoll;              // If 'true' then a new poll so user must be given opportunity to specify path & filename
    public bool PollPublished;        // Upon startup, check whether a template with same name exists or Respondents > 0; if so, then set this flag to True

    private SortedList ASMlist;       // A custom list that provides ASM linkage information
    private ArrayList UnlockedItems;  // A list of controls (or control groups) that have been unlocked



    #region PublicProperties

    // Define Public Properties

    private int currentquestion;
    public int CurrentQuestion
    {
      get
      {
        return currentquestion;
      }
      set
      {
        currentquestion = value;

        // To avoid a possible endless feedback loop, we're going to temporarily disable the event handler
        // in the form that would cause further event handling to be done here in the Controller.
        pollForm.QuestionButtonEvent -= new EventHandler(QuestionButtonHandler);

        // We need to temporarily turn off the 'PollPublished' flag so that the 
        // Published checking isn't done when we change from one question to another
        bool currPollPublished = PollPublished;
        PollPublished = false;

        // "Presses" the current question button and does everything else necessary
        // to ensure that the current question's associated information is displayed.
        pollForm.SetCurrentQuestion(pollModel, value, ASMlist);

        // Set this flag back to its earlier value
        PollPublished = currPollPublished;

        // Now wire back in the event trigger for the Question Buttons that reside in frmPoll
        pollForm.QuestionButtonEvent += new EventHandler(QuestionButtonHandler);
      }
    }


    private AnswerFormat currentanswerformat = AnswerFormat.Standard;
    public AnswerFormat CurrentAnswerFormat
    {
      get
      {
        return currentanswerformat;
      }
      set
      {
        currentanswerformat = value;
      }
    }

    // If (IsDirty == true) then changes have been made that require pollModel to be saved before the app exits
    private bool isDirty = false;
    public bool IsDirty
    {
      get
      {
        return isDirty;
      }
      set
      {
        if (pollForm.LockIsDirty == false)
          isDirty = value;
      }
    }


    // Note: This property is currently not utilized but we'll keep the code in place because the overhead is minimal.
    private int currentRespondent;
    public int CurrentRespondent
    {
      get
      {
        return currentRespondent;
      }
      set
      {
        currentRespondent = value;
      }
    }

    // Name of poll (set by frmPoll) - Note: This is the simple name, w/o a leading path or trailing extension
    private string _pollName;
    public string pollName
    {
      get
      {
        return _pollName;
      }
      set
      {
        _pollName = value;
        pollForm.PollName = value;  // Also store this name in the form
      }
    }

    // This is the full name of the poll, including path & extension
    private string _pollNameFull;
    public string pollNameFull
    {
      get
      {
        return _pollNameFull;
      }
      set
      {
        _pollNameFull = value;
      }
    }

    #endregion


    /// <summary>
    /// Constructor for Controller
    /// </summary>
    /// <param name="pollname"></param>
    /// <param name="newpoll"></param>    // true - this poll is brand new    false - this poll already has data in it
    public ControllerClass(string pollname, bool newpoll)
    {
      // Store the full pathname of this poll in a public variable
      pollNameFull = pollname;

      // Create a new Poll data model that will be tied to this instance of the Controller
      pollModel = new Poll();
      
      // Create a new Poll form that will be tied to this instance of the Controller
      pollForm = new frmPoll(pollname);

      pollForm.LockIsDirty = true;
      UnlockedItems = new ArrayList();

      // Hook into the form "closed" event
      pollForm.Closed += new EventHandler(PollFormClosed);
      pollForm.Closing += new CancelEventHandler(PollFormClosing);
      
      // Hook into the Click event of the 'AnswerFormat' radio buttons
      pollForm.AnswerFormatEvent += new EventHandler(AnswerFormatChanged);

      Type modelType = pollModel.GetType();

      // 'pollname', as specified above is a fully qualified filename, complete with path and extension, but the
      // constructor code in frmPoll strips away all but the core name.  This is what will be stored in 'pollName'.
      pollName = pollForm.PollName;   
      
      // Keeps track of whether this poll is a new one or one loaded from disk.  If the former then the first time
      // the user wants to save the data, he must specify a path and filename, though defaults will be provided.      
      newPoll = newpoll;

      // Prepare the available Answer radio buttons for this poll before we initialize ASM
      pollForm.PrepareAnswerButtons();

      // Prepares everything necessary to establish automated synchronicity between pollModel and
      // the "smart" controls in its associated pollForm.
      // Note: This could not be done in frmPoll's constructor because at that time there was no way
      //       to reference the Model from the form (which is necessary for Reflection).
      ASMlist = new SortedList();   // Initialize this collection class, which will be populated by the next line
      InitializeASM();              // ASM = Automated Synchronization Mechanism

      // Subscribe here to Property events for properties that reside in nested classes
      // Note: We can't subscribe here to those properties that reside in objects that are elements of collections.
      //       This is because each element needs to be subscribed to individually, as it's instantiated and added
      //       to the collection.
      // When an event is fired, the EventHandler specified here will be called.
      pollModel.CreationInfo.ModelEvent += new ASMEventHandler(PropertyEventHandler);
      pollModel.Instructions.ModelEvent += new ASMEventHandler(PropertyEventHandler);
      pollModel.PollingInfo.ModelEvent += new ASMEventHandler(PropertyEventHandler);
      pollModel.PollsterPrivileges.ModelEvent += new ASMEventHandler(PropertyEventHandler);
      
      // Subscribe here to Collection events for the various collections.  This will handle events for the Add, Insert,
      // & Remove operations of a collection.  When an event is fired, the EventHandler specified here will be called.
      
      // Debug: 2005-05-25 - After further thought, we're not going to subscribe to any collection events and
      //                     will handle the addition/deletion of collection elements differently.
      //pollModel.Questions.CollectionEvent += new ASMCollectionEventHandler(CollectionEventHandler);

      // In pollForm, because many of the events are handled by a solitary event handler, here we
      // just need one line to complete the wiring of those events that will be fired in the form.
      pollForm.FormControlEvent += new EventHandler(FormControlEventHandler);

      // This event handler monitors immediate changes (ie. even just one character) to an ASM marked textbox.
      pollForm.TextBoxChangedEvent += new EventHandler(TextBoxChangedEventHandler);

      // However we do have some specialized events that we'll handle separately
      pollForm.DragDropEvent += new DragDropEventHandler(DragDropHandler);
      pollForm.ChoiceEvent += new EventHandler(ChoiceHandler);
      PanelControls.PreviewEvent += new EventHandler(PreviewHandler);

      // Special Case Events that need to be handled individually
      pollForm.RespondentChangedEvent += new CurrRespondentEventHandler(CurrRespondentChangedEvent);
      pollForm.HideQuestionNumbersChangedEvent += new EventHandler(HideQuestionNumbersChangedEvent);
      pollForm.PurgeDurationChangedEvent += new EventHandler(PurgeDurationChangedEvent);

      // Monitor the special GraphInfo events that are fired by the ChartPanel controls on the Summary page.
      // We only need to monitor them so as to set "IsDirty" to true.
      pollForm.GraphInfoEvent += new EventHandler(GraphInfoChangedEvent);
      

      // If we're starting a new poll then initialize it
      if (newpoll)
      {
        InitNewPoll(pollModel);

        // Because the PurgeDuration combobox is not tied in with ASM, we need to manually set its index value
        pollForm.SetComboPurgeDuration(pollModel.CreationInfo.PurgeDuration);
      }

      else   // Load in existing poll
      {
        Tools.OpenData(pollname, pollModel, new ASMEventHandler(PropertyEventHandler));

        if (pollModel.Questions.Count == 0)  // We always must have at least one question present, even if it's blank
          AddDefaultQuestion(pollModel);

        // Prepare the necessary Question buttons: 1 button for each question in the model
        pollForm.UpdateQuestionButtons(pollModel);

        // Set current question in form.  For now, this will be the first question = Question #0.
        // In the future we could store the number of the last question worked on and go back to it immediately.
        CurrentQuestion = 0;

        // Because the PurgeDuration combobox is not tied in with ASM, we need to manually set its index value
        pollForm.SetComboPurgeDuration(pollModel.CreationInfo.PurgeDuration);

        // Debug: I don't think this is necessary because it's also called from within 'UpdateQuestionButtons' above
        // Prepare all controls on the Summary and Responses tab pages that involve Responses data
        //pollForm.UpdateCollectedData(pollModel);

        // Now that all the data has been loaded, set this value
        PollPublished = CheckIfPublished();
      }

      // Whether we loaded this poll in from disk or created it afresh, the preparation of the poll will
      // have inadvertently set 'IsDirty' to true, even though we haven't let the user touch it yet.  So
      // we'll explicitly set it to false right now.
      pollForm.LockIsDirty = false;

      // Register this Controller instance with PollManager
      PollManager.Current.RegisterPoll(this);
    }

    

    /// <summary>
    /// Called by one of these events in pollForm:
    ///  - A Question Button is pressed, indicating that the current question should be changed
    ///  - The "Add" question button is pressed
    ///  - The "Duplicate" question button is pressed
    ///  - The "Remove" question button is pressed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void QuestionButtonHandler(object sender, EventArgs e)
    {
      // See if it's the Add/Duplicate/Remove buttons in the Question List console
      if (sender.GetType().Name == "Button")   
      {
        if (CheckPublishedLock(PublishedLock.QuestionStructure))
        {
          IsDirty = true;
          Button but = sender as Button;

          switch(but.Name)
          {
            case "buttonAddQuestion":
            {
              _Question quest = new _Question(pollModel.Questions);
              quest.ID = -1;    // -1 signifies that AutoNumber will be used to get the next incremental ID

              // Figure out what the default AnswerFormat for this new question should be
              if (CurrentQuestion != -1)
                quest.AnswerFormat = pollModel.Questions[CurrentQuestion].AnswerFormat;
              else
                quest.AnswerFormat = AnswerFormat.Standard;

              pollModel.Questions.Insert(CurrentQuestion + 1, quest);
              quest.ModelEvent += new ASMEventHandler(PropertyEventHandler);

              pollForm.UpdateQuestionButtons(pollModel);  // Add another Question button to the form
              AddDefaultChoice(pollModel, CurrentQuestion + 1);

              CurrentQuestion++;   // Do everything required to make this new question the current one
            }
            break;

            case "buttonDuplicateQuestion":
            {
              _Question oldQuest = pollModel.Questions[CurrentQuestion];
              _Question newQuest = new _Question(pollModel.Questions);                 // We need to make an identical copy of 'oldQuest'
              oldQuest.Clone(newQuest);                                                // using a custom-written deep cloning method
              pollModel.Questions.Insert(CurrentQuestion + 1, newQuest);               // and then insert it in the correct location
              newQuest.ModelEvent += new ASMEventHandler(PropertyEventHandler);
              pollForm.UpdateQuestionButtons(pollModel);
              CurrentQuestion++;
              pollForm.SetCurrentChoice(0);
            }
            break;

            case "buttonRemoveQuestion":
            {
              pollModel.Questions.RemoveAt(CurrentQuestion);
              int newCount = pollModel.Questions.Count;
          
              if (newCount == 0)
              {
                // No questions left so create a new default question
                Tools.ShowMessage("You've just removed your last question so we'll reset to the initial state", "Pocket Pollster");
                AddDefaultQuestion(pollModel);
              }
              else if (CurrentQuestion == newCount)   // Was this deleted question the last one?
              {
                pollForm.UpdateQuestionButtons(pollModel);
                CurrentQuestion--;
              }
              else
              {
                pollForm.UpdateQuestionButtons(pollModel);
                //pollForm.SetCurrentQuestion(pollModel, CurrentQuestion, ASMlist);   // Just deleted a question in the middle of the list so force redraw
                CurrentQuestion = CurrentQuestion;  // Force redraw
              }
            }
            break;
          }
        }
      }

      else   // It must be one of the numeric Question selector buttons
      {
        QuestionButton qBut = sender as QuestionButton;
        CurrentQuestion = qBut.QuestionNum;
      }
    }


    
    // When the user wants to change the order of the questions, he drags a question button and indicates a new location.
    // Such operations are initiated and visually handled in frmPoll but changes to the Model are handled here.
    private void DragDropHandler(int newPosition, object sender, DragEventArgs e)
    {
      IsDirty = true;      
      int oldPosition;

      switch(Tools.ObjectType(sender))
      {
        case "QuestionButton":
          if (CheckPublishedLock(PublishedLock.QuestionStructure))
          {
            QuestionButton qBut = sender as QuestionButton;
            oldPosition = qBut.QuestionNum;

            switch (e.Effect)
            {
              case DragDropEffects.Move:
              {
                //_Question quest = new _Question(pollModel.Questions);
                _Question quest = pollModel.Questions[oldPosition];      // Establish reference to question being moved
                pollModel.Questions.Insert(newPosition, quest);          // Insert in new location in the collection

                if (newPosition > oldPosition)
                {
                  pollModel.Questions.RemoveAt(oldPosition);        
                  CurrentQuestion = newPosition - 1;
                }
                else
                {
                  pollModel.Questions.RemoveAt(oldPosition + 1);        
                  CurrentQuestion = newPosition;
                }
              }
              break;

              case DragDropEffects.Copy:
              {
                _Question oldQuest = pollModel.Questions[oldPosition];
                _Question newQuest = new _Question(pollModel.Questions);                 // We need to make an identical copy of 'oldQuest'
                oldQuest.Clone(newQuest);                                                // using a custom-written deep cloning method
                pollModel.Questions.Insert(newPosition, newQuest);                       // and then insert it in the correct location
                newQuest.ModelEvent += new ASMEventHandler(PropertyEventHandler);
                pollForm.UpdateQuestionButtons(pollModel);
                CurrentQuestion = newPosition;
                pollForm.SetCurrentChoice(0);
              }
              break;

              default:
              {
                Debug.Fail("Unaccounted for drag & drop operation: " + e.Effect.ToString(), "Controller.DragDropHandler");
              }
              break;
            }
          }
          break;


        case "PanelChoice":
          if (CheckPublishedLock(PublishedLock.ChoiceStructure))
          {
            PanelChoice panel = sender as PanelChoice;
            oldPosition = panel.ChoiceNum;

            switch (e.Effect)
            {
              case DragDropEffects.Move:
              {
                _Choice choice = new _Choice(pollModel.Questions[CurrentQuestion].Choices);
                choice = pollModel.Questions[CurrentQuestion].Choices[oldPosition];            // Establish reference to choice being moved

                pollModel.Questions[CurrentQuestion].Choices.Insert(newPosition, choice);      // Insert in new location in the collection

                if (newPosition > oldPosition)
                {
                  pollModel.Questions[CurrentQuestion].Choices.RemoveAt(oldPosition);
                  choice = null;
                  pollForm.SetCurrentChoice(newPosition - 1);
                }
                else
                {
                  pollModel.Questions[CurrentQuestion].Choices.RemoveAt(oldPosition + 1);        
                  choice = null;
                  pollForm.SetCurrentChoice(newPosition);
                }

                // We need to update the entire panelChoices because the order of entries has changed.
                // But before we do so we need to turn off ASM or else UpdateChoices will muck up the 
                // order of the Choices collection.
                pollForm.FormControlEvent -= new System.EventHandler(FormControlEventHandler);

                // We can now safely update panelChoices
                pollForm.UpdateChoices(pollModel, ASMlist, CurrentQuestion, CurrentAnswerFormat);
          
                // Reactivate ASM
                pollForm.FormControlEvent += new System.EventHandler(FormControlEventHandler);

                // Need to highlight newly positioned current choice
                pollForm.SetCurrentChoice(pollForm.CurrentChoice);
              }
              break;

              case DragDropEffects.Copy:
              {
                _Choices choices = pollModel.Questions[CurrentQuestion].Choices;
                _Choice oldChoice = choices[oldPosition];     
                _Choice newChoice = new _Choice(choices);     // We need to make an identical copy of 'oldChoice'
                oldChoice.Clone(newChoice);                   // using a custom-written deep cloning method
                choices.Insert(newPosition, newChoice);       // and then insert it in the correct location

                // We need to update the entire panelChoices because the order of entries has changed.
                // But before we do so we need to turn off ASM or else UpdateChoices will muck up the 
                // order of the Choices collection.
                pollForm.FormControlEvent -= new System.EventHandler(FormControlEventHandler);

                // We can now safely update panelChoices
                pollForm.UpdateChoices(pollModel, ASMlist, CurrentQuestion, CurrentAnswerFormat);
          
                // Reactivate ASM
                pollForm.FormControlEvent += new System.EventHandler(FormControlEventHandler);

                // Need to highlight newly positioned current choice
                pollForm.SetCurrentChoice(pollForm.CurrentChoice);
              }
              break;

              default:
              {
                Debug.Fail("Unaccounted for drag & drop operation: " + e.Effect.ToString(), "Controller.DragDropHandler");
              }
              break;
            }
          }
          break;


        default:
          Debug.Fail("Unaccounted for object type: " + Tools.ObjectType(sender), "Controller.DragDropHandler");
          break;
      }
    }



    /// <summary>
    /// Handles events fired by:
    ///  - The Add choice button
    ///  - The Duplicate choice button
    ///  - The Remove choice button
    ///  - Selecting one of the Choice panels (or its children)
    /// </summary>
    private void ChoiceHandler(object sender, EventArgs e)
    {
      switch (sender.GetType().Name)
      {
        case "Button":       // See if it's the Add/Duplicate/Remove buttons in the Choices title bar
          IsDirty = true;
          Button but = sender as Button;

          if (CheckPublishedLock(PublishedLock.ChoiceStructure))
          {
            switch(but.Name)
            {
              case "buttonAddChoice":
              {
                _Choice choice = new _Choice(pollModel.Questions[CurrentQuestion].Choices);
                choice.ID = -1;    // -1 signifies that AutoNumber will be used to get the next incremental ID

                pollModel.Questions[CurrentQuestion].Choices.Insert(pollForm.CurrentChoice + 1, choice);   // Add the blank new choice to the model
                pollForm.UpdateChoices(pollModel, ASMlist, CurrentQuestion, CurrentAnswerFormat);          // Update panelChoices accordingly

                // Do everything required to make this new choice the current one
                pollForm.CurrentChoice++;   
                pollForm.SetCurrentChoice(pollForm.CurrentChoice);
              }
              break;

              case "buttonDuplicateChoice":
              {
                _Choices choices = pollModel.Questions[CurrentQuestion].Choices;
                _Choice oldChoice = choices[pollForm.CurrentChoice];     
                _Choice newChoice = new _Choice(choices);                 // We need to make an identical copy of 'oldChoice'
                oldChoice.Clone(newChoice);                               // using a custom-written deep cloning method
                choices.Insert(pollForm.CurrentChoice + 1, newChoice);    // and then insert it in the correct location

                // We need to update the entire panelChoices because the order of entries has changed.
                // But before we do so we need to turn off ASM or else UpdateChoices will muck up the 
                // order of the Choices collection.
                pollForm.FormControlEvent -= new System.EventHandler(FormControlEventHandler);

                // We can now safely update panelChoices
                pollForm.UpdateChoices(pollModel, ASMlist, CurrentQuestion, CurrentAnswerFormat);
          
                // Reactivate ASM
                pollForm.FormControlEvent += new System.EventHandler(FormControlEventHandler);

                // Need to highlight newly positioned current choice
                pollForm.SetCurrentChoice(pollForm.CurrentChoice + 1);
              }
              break;

              case "buttonRemoveChoice":
              {
                pollModel.Questions[CurrentQuestion].Choices.RemoveAt(pollForm.CurrentChoice);             // Remove currently selected choice

                int newCount = pollModel.Questions[CurrentQuestion].Choices.Count;
            
                if (newCount == 0)
                {
                  // No choices left so create a new default choice
                  Tools.ShowMessage("You've just removed your last answer so we'll reset to the initial state", "Pocket Pollster");
                  AddDefaultChoice(pollModel, CurrentQuestion);
                  pollForm.UpdateChoices(pollModel, ASMlist, CurrentQuestion, CurrentAnswerFormat);
                  pollForm.SetCurrentChoice(0);
                }
                else if (pollForm.CurrentChoice == newCount)   // Was this deleted choice the last one in the list?
                {
                  pollForm.UpdateChoices(pollModel, ASMlist, CurrentQuestion, CurrentAnswerFormat);
                  pollForm.CurrentChoice--;
                  pollForm.SetCurrentChoice(pollForm.CurrentChoice);
                }
                else   // The previous current question was earlier on in the list
                {
                  pollForm.UpdateChoices(pollModel, ASMlist, CurrentQuestion, CurrentAnswerFormat);
                  pollForm.SetCurrentChoice(pollForm.CurrentChoice);
                }
              }
              break;
            }
          }
          break;


        case "PanelChoice":
          PanelChoice panel = sender as PanelChoice;
          pollForm.SetCurrentChoice(panel.ChoiceNum);
          break;


        default:   // One of the children of the Choice panels must have fired the event
          // Changes to some of the controls force us to "redraw" panelChoices, whereas others do not.
          // We'll discern which control fired this event and then act accordingly.
          Control ctrl = sender as Control;

          if (ctrl.Name.IndexOf("chkExtraInfoMultiline") != -1)   // Fired by a Multiline checkbox
          {
            pollForm.UpdatePreview(pollModel, CurrentQuestion, CurrentAnswerFormat);
          }
          else if (ctrl.Name.IndexOf("chkExtraInfo") != -1)   // Fired by an ExtraInfo checkbox
          {
            pollForm.UpdateChoices(pollModel, ASMlist, CurrentQuestion, CurrentAnswerFormat);
            int tabindex = (ctrl as CheckBox).Checked ? 2 : 0;
            pollForm.SetCurrentChoice((ctrl.Parent as PanelChoice).ChoiceNum, tabindex);
          }
          else    // Fired by a textbox
            // Need to set parent panel of child control to be current
            pollForm.SetCurrentChoice((ctrl.Parent as PanelChoice).ChoiceNum, -1);

          break;
      }
    }


    /// <summary>
    /// Handles events fired by controls in the Preview pane.  We are simulating what happens on a mobile device.
    /// For example, if the user chooses an option that has "ExtraInfo" marked 'true' then we will add a little
    /// text box to the lower right of this option.
    /// 
    /// AnswerFormats 0 - 3 allow the user to add an extra comment for any item that is so marked with this
    /// capability (ie. ExtraInfo = true).  We will redraw the Preview pane when the presence or location of
    /// the "Other" type textbox changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PreviewHandler(object sender, EventArgs e)
    {
      if (pollModel == null)  // I don't know why but with certain polls, sometimes this method is entered and 'pollModel' doesn't exist
        return;

      try
      {
        int idx;

        // Initialize, though for AnswerFormats 0 - 3 only
        int oldExtraInfo = 0;
        int newExtraInfo = 0;
        string ctrlType = sender.GetType().Name;

        // Create reference to PreviewInfo data
        DataObjects.PreviewInfo previewInfo = pollForm.PreviewInfo;   

        AnswerFormat aFormat = CurrentAnswerFormat;

        // Record previous state of choices (for AnswerFormats 0 - 3 only)
        if (aFormat == AnswerFormat.Standard || aFormat == AnswerFormat.List || aFormat == AnswerFormat.DropList)
        {
          if (previewInfo.SelectedIndex != -1)  // Possible with combobox
            oldExtraInfo = pollModel.Questions[CurrentQuestion].Choices[previewInfo.SelectedIndex].ExtraInfo ? (int) Math.Pow(2, previewInfo.SelectedIndex) : 0;
          else
            oldExtraInfo = 0;
        }
        else if (aFormat == AnswerFormat.MultipleChoice)
          oldExtraInfo = previewInfo.CalcBitSum();

        switch (aFormat)
        {
          case AnswerFormat.Standard:
            switch (ctrlType)
            {
              case "RadioButtonPP":
                try
                {
                  previewInfo.SelectedIndex = (sender as RadioButtonPP).SelectedIndex;
                  idx = previewInfo.SelectedIndex;
                  
                  newExtraInfo = pollModel.Questions[CurrentQuestion].Choices[idx].ExtraInfo ? (int) Math.Pow(2, idx) : 0;

                  if (oldExtraInfo != newExtraInfo)
                    pollForm.UpdatePreview(pollModel, CurrentQuestion, CurrentAnswerFormat);
                }
                catch (Exception ex)
                {
                  Debug.Fail("Error updating preview pane: " + ex, "Controller.PreviewHandler.(case RadioButtonPP)");
                }

                break;

              case "TextBoxPP":
                previewInfo.SelectedIndex = (sender as TextBoxPP).SelectedIndex;
                Tools.EncodeExtraTextValue((sender as TextBoxPP).Text, previewInfo.SelectedIndex, ref previewInfo.OtherText);
                break;

              default:
                Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "Controller.PreviewHandler - Standard");
                break;
            }

            break;


          case AnswerFormat.List:
            switch (ctrlType)
            {
              case "ListBoxPP":
                previewInfo.SelectedIndex = (sender as ListBox).SelectedIndex;
                idx = previewInfo.SelectedIndex;
                newExtraInfo = pollModel.Questions[CurrentQuestion].Choices[idx].ExtraInfo ? (int) Math.Pow(2, idx) : 0;

                if (oldExtraInfo != newExtraInfo)
                  pollForm.UpdatePreview(pollModel, CurrentQuestion, CurrentAnswerFormat);

                break;

              case "TextBoxPP":
                previewInfo.SelectedIndex = (sender as TextBoxPP).SelectedIndex;
                Tools.EncodeExtraTextValue((sender as TextBoxPP).Text, previewInfo.SelectedIndex, ref previewInfo.OtherText);
                break;

              default:
                Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "Controller.PreviewHandler - List");
                break;
            }

            break;


          case AnswerFormat.DropList:
            switch (ctrlType)
            {
              case "ComboBoxPP":
                previewInfo.SelectedIndex = (sender as ComboBox).SelectedIndex - 1;
                idx = previewInfo.SelectedIndex;
           
                if (idx != -1)
                  newExtraInfo = pollModel.Questions[CurrentQuestion].Choices[idx].ExtraInfo ? (int) Math.Pow(2, idx) : 0;
                else
                  newExtraInfo = 0;

                if (oldExtraInfo != newExtraInfo)
                  pollForm.UpdatePreview(pollModel, CurrentQuestion, CurrentAnswerFormat);

                break;

              case "TextBoxPP":
                previewInfo.SelectedIndex = (sender as TextBoxPP).SelectedIndex;
                Tools.EncodeExtraTextValue((sender as TextBoxPP).Text, previewInfo.SelectedIndex, ref previewInfo.OtherText);
                break;

              default:
                Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "Controller.PreviewHandler - DropList");
                break;
            }

            break;


          case AnswerFormat.MultipleChoice:
            switch (ctrlType)
            {
              case "CheckBoxPP":
                CheckBoxPP chkBox = sender as CheckBoxPP;
                idx = chkBox.SelectedIndex;
                
                if (chkBox.Checked)
                  previewInfo.Add(idx);
                else
                {
                  previewInfo.Remove(idx);
                  // Because of code in PreviewInfo.Add we need to also clear the root if CheckBoxItems.Count == 0
                  if (previewInfo.CheckBoxItems.Count == 0)
                    previewInfo.SelectedIndex = -1;
                }

                newExtraInfo = previewInfo.CalcBitSum();

                if (oldExtraInfo != newExtraInfo)
                  pollForm.UpdatePreview(pollModel, CurrentQuestion, CurrentAnswerFormat);
                break;

              case "TextBoxPP":
                // If we enter here then, by definition, the associated checkbox MUST be checked
                //previewInfo.Add((sender as TextBoxPP).SelectedIndex, (sender as TextBoxPP).Text);

                // 2006-09-12 - Note: This code replaces the old line.  We're now handling the ExtraInfo text in a new & better way
                Tools.EncodeExtraTextValue((sender as TextBoxPP).Text, (sender as TextBoxPP).SelectedIndex, ref previewInfo.OtherText);
                break;

              default:
                Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "Controller.PreviewHandler - MultipleChoice");
                break;
            }

            break;


          case AnswerFormat.Range:
            if (ctrlType == "RadioButtonPP")
              previewInfo.SelectedIndex = (sender as RadioButtonPP).SelectedIndex;
            else
              Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "Controller.PreviewHandler - Range");

            break;


          case AnswerFormat.MultipleBoxes:
            if (ctrlType == "TextBoxPP")
            {
              previewInfo.SelectedIndex = (sender as TextBoxPP).SelectedIndex;
              previewInfo.Add((sender as TextBoxPP).SelectedIndex, (sender as TextBoxPP).Text);
            }
            else
              Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "Controller.PreviewHandler - MultipleBoxes");

            break;


          case AnswerFormat.Freeform:
            if (ctrlType == "TextBoxPP")
              previewInfo.Freeform = (sender as TextBoxPP).Text;
            else
              Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "Controller.PreviewHandler - Freeform");

            break;


          case AnswerFormat.Spinner:
            if (ctrlType == "NumericUpDownPP")
              previewInfo.Spinner = (int) (sender as NumericUpDown).Value;
            else
              Debug.Fail("Unaccounted for control: " + sender.GetType().Name, "Controller.PreviewHandler - Spinner");

            break;


          default:
            break;
        }
      }

      catch (Exception ex)
      {
        Debug.Fail("Error preparing preview pane: " + ex.Message, "Controller.PreviewHandler");
      }
    }



    /// <summary>
    /// This is called when the frmPoll.Closing event is fired.  That event can happen in one of several ways:
    ///   - The upper-right 'X' on frmPoll is clicked
    ///   - File-Close is clicked in frmMain
    ///   - File-CloseAll is clicked in frmMain
    ///   - Some future code may cause 1 or more polls to be closed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PollFormClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      this.pollForm.ForceASMEvent();  // Simply forces any waiting ASM event to occur, such as text in a textbox

      if (IsDirty)
      {
        DialogResult choice = Tools.ShowMessage("Do you want to save the changes to " + pollForm.PollName + "?", SysInfo.Data.Admin.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

        switch (choice)
        {
          case DialogResult.Yes:    // Call Save in frmMain
            SaveFile();                 // This will cause an event to be fire below; an event that is being monitored in frmMain  
            break;                      // It also resets the IsDirty flag

          case DialogResult.No:
            // Do nothing, just close w/o saving
            break;

          case DialogResult.Cancel:
            e.Cancel = true;
            break;
        }
      }
    }


    /// <summary>
    /// This is called when the frmPoll.Closed event is fired.  It occurs immediately after frmPoll.Closing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PollFormClosed(object sender, EventArgs e)
    {
      // The frmPoll object is disposed of already but we need to explicitly destroy the companion data model too.
      pollModel = null;  

      // UnRegister (ie. remove) this Controller instance with PollManager
      PollManager.Current.UnRegisterPoll(this);
    }


    // Note: The bool return value shown here must be so because of restructuring we had to do in the corresponding method in frmMain.
    public delegate bool SaveFileHandler();
    public static event SaveFileHandler SaveFileEvent;

    private static bool SaveFile()
    {
      return SaveFileEvent();
    }



    /// <summary>
    /// Handles events fired by one of the frmPoll.radioAnswerFormatxxxx buttons near the top of the "Questions & Answers" page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AnswerFormatChanged(object sender, EventArgs e)
    {
      IsDirty = true;
      AnswerButton aBut = sender as AnswerButton;
      CurrentAnswerFormat = aBut.AnswerFormat;

      // Note: We need to pass the CurrentAnswerFormat parameter because we can't be guaranteed that the model is updated with it at this point
      pollForm.UpdateChoices(pollModel, ASMlist, CurrentQuestion, CurrentAnswerFormat);

      // It's possible that the new AnswerFormat may have caused a previously selected choice (panel)
      // to no longer appear.  So we need to check for this and compensate for it.
//      if (pollForm.CurrentChoice + 1 < pollForm.panelChoices.Controls.Count)
//        pollForm.CurrentChoice = 0;
//      else

      pollForm.SetCurrentChoice(pollForm.CurrentChoice, -1);
    }



    public delegate void PropertyEventHandlerDelegate(string propPath, object propValue, EventArgs e);

    /// <summary>
    /// Handles events fired by a property in the data model.  We will generally change
    /// the display properties of different controls, if it is appropriate to do so.
    /// </summary>
    /// <param name="propPath"></param>
    /// <param name="propValue"></param>
    /// <param name="e"></param>
    private void PropertyEventHandler(string propPath, object propValue, EventArgs e)
    {
      try
      {
        int CtrlIdx = -1;   // Initialize

        // If propPath specifies one or more collections then we have to retrieve the index(es) and use them appropriately.
        // Note: How we use them is different depending on which collection is involved.
        if (propPath.IndexOf("[") != -1)                         // Does this property path contain any collections?
        {
          if (propPath.IndexOf("Questions[") != -1)              // Does it refer to the 'Questions' collection?
          {
            if (Tools.GetIndex(propPath) != CurrentQuestion)     // Does it refer to the current collection being displayed?
              return;                                            // No, not the current question so do nothing
            else
              Tools.DecodePropertyPath(propPath, ref CtrlIdx);   // Yes, so obtain index of 'Choices[#]'
          }
          else
          {
            // Debug: For now we'll leave as-is, but we may have to expand this clause for Respondents, Responses,
            //        and/or another collection.
            Tools.DecodePropertyPath(propPath, ref CtrlIdx);     // A collection other than Questions, but do something similar
          }

          // If the property is an Enum then we need to replace 'propValue' with the Enum's index number
          PropertyInfo propInfo = Tools.FindProperty(pollModel, propPath);
          if (propInfo.PropertyType.IsEnum)
            propValue = (byte) propValue;

          Tools.RemoveIndices(ref propPath);                     // Remove all indices - ie. Questions[5].Choices[7].Text -> Questions[].Choices[].Text
        }

        Object objTmp = ASMlist[propPath];
        ASMinfo asmInfo = objTmp as ASMinfo;

        if (asmInfo == null)
          return;

        // We now have enough information to change the value of the Control based on the value of the Property.
        ChangeControlValue(asmInfo, propValue, CtrlIdx);
      }

      catch (Exception ex)
      {
        Debug.Fail("Unhandled exception: " + ex.Message + "\n\npropPath: " + propPath + "   propValue: " + propValue.ToString(), "Controller.PropertyEventHandler");
      }
    }

    
    
    /// <summary>
    /// Sets the value of a form control based on a change in a property in the model.
    /// Note: We're making an assumption that the Type of propValue will be appropriate for the type
    ///       of control it is associated with.  This is probably safe but we need to test to be sure.
    /// </summary>
    /// <param name="asmInfo"></param>     // The ASMinfo object containing pertinent information about the control we need to set
    /// <param name="propValue"></param>   // The value we need to set in the form control
    /// <param name="CtrlIdx"></param>     // An optional index value that is only used in special cases, such as multiple sibling textboxes (-1 if not used)
    private void ChangeControlValue(ASMinfo asmInfo, object propValue, int CtrlIdx)
    {
      int idx;
      Control control = asmInfo.Control;
      string controlType = Tools.ObjectType(control);

      // Turn off listener
      pollForm.FormControlEvent -= new EventHandler(FormControlEventHandler);

      switch (controlType)
      {
        case "CheckBox":
          if (CtrlIdx == -1)
            (control as CheckBox).Checked = (bool) propValue;
          else
          {
            // Do we need to add code to handle multiple checkboxes such as 'Choices[].ExtraInfo' ?
            //Debug.Fail("Reminder: Need to add code to handle sibling checkboxes", "Controller.ChangeControlValue");

            // Now we need to find out exactly which CheckBox should be set to the value of 'propValue'.  It is either
            // the "parent" control within 'asmInfo' or one of the controls referenced in its child collection.
            if (CtrlIdx.ToString() == asmInfo.Value)
              (control as CheckBox).Checked = (bool) propValue;
            else
            {
              foreach (object obj in asmInfo.SiblingControls)
              {
                if (CtrlIdx.ToString() == (obj as ASMinfo).Value)
                {
                  ((obj as ASMinfo).Control as CheckBox).Checked = (bool) propValue;
                  break;
                }
              }  
            }



          }
          break;

        case "ComboBox":
          idx = (int) propValue;
          if (idx < (control as ComboBox).Items.Count)
            (control as ComboBox).SelectedIndex = (Int32) propValue;
          else
            Debug.Fail("Unavailable ComboBox item trying to be set.\n# of items: " + 
                       (control as ComboBox).Items.Count.ToString() + 
                       "\nIndex: " + propValue.ToString() +
                       "\nComboBox Name: " + control.Name, "Controller.ChangeControlValue");
          break;

        case "Label":
          if (asmInfo.Default != null && propValue.ToString() == "")
            (control as Label).Text = asmInfo.Default;
          
          else if (propValue.GetType().ToString() == "System.DateTime")
            (control as Label).Text = Tools.DisallowNullDate((DateTime) propValue);    // Debug: Improve in the future by allowing Format parameter in ASM
          
          else
            (control as Label).Text = propValue.ToString();

          // 2006-05-26 - This next bit of code was added to handle those circumstances where more than one label
          //              is displaying the same data.  Such is the case with CreatorName and CreatorDate.  This
          //              allows ASM to display it in more than one location in frmPoll.
          if (asmInfo.SiblingControls.Count > 0)
          {
            foreach (object obj in asmInfo.SiblingControls)
            {
              // ChangeControlValue(obj as ASMinfo, propValue, CtrlIdx);  // Fails because FormControlEvent seems to get out of sync

              // So we'll implement this interim solution
              Label label = (obj as ASMinfo).Control as Label;

              if (propValue.GetType().ToString() == "System.DateTime")
                label.Text = Tools.DisallowNullDate((DateTime) propValue);
              else
                label.Text = propValue.ToString();
            }
          }
          break;

        case "ListBox":
          idx = (Int32) propValue;
          if (idx < (control as ListBox).Items.Count)
            (control as ListBox).SelectedIndex = (Int32) propValue;
          else
            Debug.Fail("Unavailable ListBox item trying to be set.\n# of items: " + 
              (control as ListBox).Items.Count.ToString() + 
              "\nIndex: " + propValue.ToString() +
              "\nListBox Name: " + control.Name, "Controller.ChangeControlValue");
          break;

        case "RadioButton":
        case "AnswerButton":
          string propval = propValue.ToString().ToLower();

          // Now we need to find out exactly which RadioButton should be set.  It is either the "parent"
          // control within 'asmInfo' or one of the controls referenced in its child collection.
          if (asmInfo.Value.ToLower() == propval)
            (asmInfo.Control as RadioButton).Checked = true;
          else
          {
            foreach (ASMinfo siblingInfo in asmInfo.SiblingControls)
            {
              if (siblingInfo.Value.ToLower() == propval)
              {
                (siblingInfo.Control as RadioButton).Checked = true;
                break;
              }
            }
          }
          break;

        case "TextBox":
          if (CtrlIdx == -1)
          {
            if (asmInfo.Default != null && propValue.ToString() == "")
              (control as TextBox).Text = asmInfo.Default;
            else
              (control as TextBox).Text = propValue.ToString();

            // 2006-05-26 - This next bit of code was added to handle those circumstances where more than
            //              one textbox is displaying the same data.  Such is the case with PollSummary.
            //              This allows ASM to display it in more than one location in frmPoll.
            //              This does not handle instant changes if any textbox allows input though.
            //              That is handled by FormControlEventHandler.
            if (asmInfo.SiblingControls.Count > 0)
            {
              foreach (object obj in asmInfo.SiblingControls)
              {
                TextBox textBox = (obj as ASMinfo).Control as TextBox;
                textBox.Text = propValue.ToString();
              }
            }
          }
          else
          {
            // Do we need to add code to handle multiple textboxes such as 'Choices[].Text' and 'Choices[].MoreText' ?
            //Debug.Fail("Reminder: Need to add code to handle sibling textboxes", "Controller.ChangeControlValue");

            // Test Code
            if (CtrlIdx.ToString() == asmInfo.Value)
              (control as TextBox).Text = propValue.ToString();
            else
            {
              foreach (object obj in asmInfo.SiblingControls)
              {
                if (CtrlIdx.ToString() == (obj as ASMinfo).Value)
                {
                  ((obj as ASMinfo).Control as TextBox).Text = propValue.ToString();
                  break;
                }
              }  
            }
          }
          break;

        default:
          Debug.Fail("Unexpected control fired event: " + control.Name, "Controller.ChangeControlValue");
          break;
      }

      // Turn listener back on
      pollForm.FormControlEvent += new EventHandler(FormControlEventHandler);
    }



    /// <summary>
    /// Handles events fired by those form controls that have had their Tag properties specially populated
    /// so as to establish an automated synchronization with the associated Properties in the Model.
    /// 
    /// This method also checks whether the 'PublishedLock' is in effect.  If so, then it takes the appropriate
    /// action to handle it.  The one exception to this is that the handling of textboxes in this regard is 
    /// handled by 'TextBoxChangedEventHandler' below because that method examines characters on a character-by-character
    /// basis whereas this one is only called when a textbox loses its focus.
    /// </summary>
    private void FormControlEventHandler(object sender, EventArgs e)
    {
      string senderType = Tools.ObjectType(sender);
      string name = (sender as Control).Name;
      string propPath = (sender as Control).Tag.ToString();
      ASMinfo asmInfo = (ASMinfo) ASMlist[propPath];

      PopulateIndices(ref propPath, sender);

      if (asmInfo == null)
        senderType = "";      // The ASMlist collection doesn't have any info on this particular control (but should) so force error

      object oldval;
      int chgCode = -1;   // Default
      switch (senderType)
      {
        case "CheckBox":
          CheckBox _chk = sender as CheckBox;

          if (CheckPublishedLock(pollModel, propPath, out oldval, _chk.Checked, asmInfo.PubLock))
            chgCode = ChangePropertyValue(pollModel, propPath, _chk.Checked);
          else
            //_chk.Checked = (bool) oldval;
            Tools.SetPropertyValue(pollModel, propPath, oldval);
          break;

        case "ComboBox":
          ComboBox _cbo = sender as ComboBox;

          if (CheckPublishedLock(pollModel, propPath, out oldval, _cbo.SelectedIndex, asmInfo.PubLock))
            chgCode = ChangePropertyValue(pollModel, propPath, _cbo.SelectedIndex);  // Debug: Passing it '_cbo.SelectedIndex' is incorrect! (RW: ???)
          else
            //_cbo.SelectedIndex = (int) oldval;
            Tools.SetPropertyValue(pollModel, propPath, oldval);
          break;

        case "Label":
          Label _lbl = sender as Label;
          chgCode = ChangePropertyValue(pollModel, propPath, _lbl.Text);
          break;

        case "ListBox":
          ListBox _lst = sender as ListBox;

          if (CheckPublishedLock(pollModel, propPath, out oldval, _lst.SelectedIndex, asmInfo.PubLock))
            chgCode = ChangePropertyValue(pollModel, propPath, _lst.SelectedIndex);  // Debug: Still need to test!
          else
            //_lst.SelectedIndex = (int) oldval;
            Tools.SetPropertyValue(pollModel, propPath, oldval);
          break;

        case "RadioButton":
        case "QuestionButton":
        case "AnswerButton":
          RadioButton _rad = sender as RadioButton;
          
          // We're only interested in a radio button gaining focus (becoming 'checked').
          // Unless all radio buttons in a group are unselected, the sequence of events is as follows:
          //   - The previously selected radio button fires a "CheckedChanged" event, indicating that its "Checked" property is now 'false'
          //   - The newly selected radio button fires a "CheckedChanged" event, indicating that its "Checked" property is now 'true'
          if (_rad.Checked == true)
          {
            object radValue = ASM.GetSiblingValue(_rad as Control, asmInfo);

            if (CheckPublishedLock(pollModel, propPath, out oldval, radValue, asmInfo.PubLock))
              chgCode = ChangePropertyValue(pollModel, propPath, radValue);  // Debug: Need to test!!!
            else
            {
              // To Do:  Need to set the correct radio button in the group back to whichever one was set before
              Tools.SetPropertyValue(pollModel, propPath, oldval);
            }
          }
          break;

        case "TextBox":
          TextBox _txt = sender as TextBox;

          // Note: Checking of the PublishedLock is handled by the 'TextBoxChangedEventHandler' method
          chgCode = ChangePropertyValue(pollModel, propPath, _txt.Text);   // Note for reuse: This line of code stays whereas that above/below will go

          if (asmInfo.SiblingControls.Count > 0)
          {
            // We don't know whether the control that fired the event is the primary one (from the perspective of ASM)
            // or a sibling.  So we need to discern this and then set the other(s) to the same new value.
            UpdateCompanionTextBoxes(_txt, _txt.Text, asmInfo);
          }
          break;

        default:
          Debug.Fail("Unexpected control fired event:   Control name: " + ((Control) sender).Name + "   Tag: " + ((Control) sender).Tag, "Controller.FormControlEventHandler");
          break;
      }

      if (chgCode == 2)
        IsDirty = true;
    }


    private void TextBoxChangedEventHandler(object sender, EventArgs e)
    {
      if (PollPublished)   // Actually redundant because this test also appears in 'CheckPublishedLock' but will speed up execution of app
      {
        TextBox textBox = sender as TextBox;
        string name = textBox.Name;
        string propPath = textBox.Tag.ToString();
        ASMinfo asmInfo = (ASMinfo) ASMlist[propPath];

        PopulateIndices(ref propPath, sender);

        object newval = textBox.Text;
        object oldval;

        if (!CheckPublishedLock(pollModel, propPath, out oldval, newval, asmInfo.PubLock))
          // If the 'CheckPublishedLock' test fails then set the control back to the old value
          Tools.SetPropertyValue(pollModel, propPath, oldval);  // Using this method so that form control is updated too
      }
    }



    /// <summary>
    /// Once a poll has been published or has any Respondent data in it, it is not advisable for the user to make
    /// changes to assorted items in the poll.  However there may be some circumstances when they want to do this, 
    /// such as to correct a minor spelling mistake.  This method checks:
    ///   - Whether the poll has been published
    ///   - Whether the value on the form is different than the value in the object model (often not the case if a LostFocus occurred)
    ///   - The type of 'PublishedLock' in place
    ///   
    /// Where required, it provides a warning as to why the change should not be made, but gives the user the ultimate power to decide what to do.
    /// </summary>
    /// <param name="pollModel"></param>
    /// <param name="propPath"></param>
    /// <param name="oldval"></param>
    /// <param name="newval">The value in the form control</param>
    /// <param name="pubLock">The type of PublishedLock in place</param>
    /// <returns>true - Accept change    false - Reject change</returns>
    private bool CheckPublishedLock(Poll pollModel, string propPath, out object oldval, object newval, PublishedLock pubLock)
    {
      bool allowChange = false;  // Default assumption
      oldval = null;  // Need to assign here, as it's an 'out' parameter

      if (!PollPublished || CheckIfUnlocked(propPath, pubLock))
        allowChange = true;
      
      else
      {
        if (pollModel != null && propPath != null)
          oldval = Tools.GetPropertyValue(pollModel, propPath);
        else
          pollModel = null;  // So that we'll enter the next construct

        if (pollModel == null || !oldval.Equals(newval))
        {
          string unlockSelection = "";    // Default value; the null string implies cancel the change
          frmUnlock unlockForm;

          switch(pubLock)
          {
            case PublishedLock.None:
              allowChange = true;  // No lock, so no need to check anything
              break;

            case PublishedLock.Instructions:
              unlockForm = new frmUnlock(propPath, pubLock, out unlockSelection);
              break;

            case PublishedLock.QA_Basic:
              unlockForm = new frmUnlock(propPath, pubLock, out unlockSelection);
              break;

            case PublishedLock.AnswerFormat:
              unlockForm = new frmUnlock(propPath, pubLock, out unlockSelection);
              break;

            case PublishedLock.ChoiceStructure:
              unlockForm = new frmUnlock(propPath, pubLock, out unlockSelection);
              break;

            case PublishedLock.QuestionStructure:
              unlockForm = new frmUnlock(propPath, pubLock, out unlockSelection);
              break;

            default:
              Debug.Fail("Unknown PublishedLock", "Controller.CheckPublishedLock");
              break;
          }

          if (unlockSelection != "")
          {
            allowChange = true;

            if (unlockSelection.ToUpper() == "ALL")
              this.PollPublished = false;   // This removes all locking for the duration of the session

            else if (!UnlockedItems.Contains(unlockSelection))
              UnlockedItems.Add(unlockSelection);
          }
        }
      }

      return allowChange;
    }



    //  This method is called by the following:
    //    - Add & Remove Question buttons
    //    - Add & Remove Choice buttons
    private bool CheckPublishedLock(PublishedLock pubLock)
    {
      object dummyVal;
      return CheckPublishedLock(pollModel, null, out dummyVal, null, pubLock);
    }



    /// <summary>
    /// Examines the module-level variable 'UnlockedItems' and checks whether the specific item
    /// or its general category has been unlocked.  This is in reference to locking that is done
    /// once a poll is published.
    /// </summary>
    /// <param name="propPath"></param>
    /// <param name="pubLock"></param>
    /// <returns>true - item is unlocked      false - item is locked</returns>
    private bool CheckIfUnlocked(string propPath, PublishedLock pubLock)
    {
      bool unlocked = false;

      // First see if the specific control being examined has previously been unlocked
      if (UnlockedItems.Contains(propPath))
        unlocked = true;

      // Now check the general settings for the category that this control belongs in (e.g. all controls on 'Instructions')
      else if (UnlockedItems.Contains(pubLock.ToString()))
        unlocked = true;

      return unlocked;
    }



    /// <summary>
    /// If propPath specifies one or more collections then we have to add in the appropriate index (or indices).
    /// For now we're only going to deal with Questions[] and Choices[], but in the future we'll have to account for more types.
    /// </summary>
    /// <param name="propPath"></param>
    /// <param name="sender"></param>
    private void PopulateIndices(ref string propPath, object sender)
    {
      if (propPath.IndexOf("[") != -1)
      {
        if (propPath.IndexOf("Questions[]") != -1)
        {
          if (propPath.IndexOf("Choices[]") == -1)
            Tools.EncodePropertyPath(ref propPath, CurrentQuestion, -1);
          else
          {
            // Note: We have to do it this way, rather than obtaining CurrentChoice from pollForm
            //       because 'GeneralControl_Event' is fired before 'Choice_Event', so pollForm.CurrentChoice
            //       is not up to date if the user picks a control in the non-current Choice panel.
            int currentChoice = ((sender as Control).Parent as PanelChoice).ChoiceNum;
            Tools.EncodePropertyPath(ref propPath, CurrentQuestion, currentChoice);
          }
        }
        else
        {
          // This will signify to us when we need to add more code in this construct in the future.
          Debug.Fail("Unaccounted for property path: " + propPath, "Controller.PopulateIndices");
        }
      }
    }



    // Eventually enhance this method to be 'UpdateCompanionControls'.
    private void UpdateCompanionTextBoxes(TextBox textBox, string newval, ASMinfo asmInfo)
    {
      // Note: If these controls are essentially part of a control array (like Choices) then we must realize
      //       that the "Value" property associated with them in ASMinfo uniquely distinguishes them.  Thus
      //       2 controls with different "Value" properties are NOT connected controls.

      // First determine the Value property of the control that fired the event
      string currValue = "";

      if (textBox == (asmInfo.Control as TextBox))
        currValue = asmInfo.Value;
      else
        foreach (object obj in asmInfo.SiblingControls)
        {
          if (textBox == ((obj as ASMinfo).Control as TextBox))
          {
            currValue = (obj as ASMinfo).Value;
            break;
          }
        }

      // Now go through the controls again and see which ones need to be set with the newly changed text
      if (textBox != (asmInfo.Control as TextBox) && (currValue == asmInfo.Value))
        (asmInfo.Control as TextBox).Text = newval;

      foreach (object obj in asmInfo.SiblingControls)
      {
        TextBox textBox2 = (obj as ASMinfo).Control as TextBox;

        if (textBox != textBox2 && (currValue == (obj as ASMinfo).Value))
          textBox2.Text = newval;
      }
    }


    
    /// <summary>
    /// Sets a Property value in the Model based on a change with a form control.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="propPath"></param>
    /// <param name="newval"></param>
    /// <returns>0 - An error occurred     1 - Property value not changed     2 - Property value changed</returns>
    private int ChangePropertyValue(Poll model, string propPath, object newval)
    {
      Type modelType = model.GetType();
      PropertyInfo property = Tools.FindProperty(model, propPath);

      // Debug: We should be able to have 1 method retrieve both Property and PropertyValue!
      string propertyType = property.PropertyType.Name;
      object oldval = Tools.GetPropertyValue(model, propPath);

      // Ensure that 'oldval' is not null, or else an error will occur below when we try to compare it with a non-null 'newval'
      if (oldval == null)
      {
        switch (propertyType)
        {
          case "Boolean":
            oldval = false;
            break;

          case "String":
            oldval = "";
            break;

          default:
            Debug.Fail("Unaccounted for data type: " + propertyType, "Controller.ChangePropertyValue (1)");
            return 0;
        }
      }

      // Ensure that the Type of 'newval' matches with the Property Type (and thus with the Type of 'oldval')
      if (Tools.ObjectType(newval) != propertyType)
      {
        switch (propertyType)
        {
          case "Boolean":
            newval = Convert.ToBoolean(newval);      
            break;

          case "Byte":
            newval = Convert.ToByte(newval);      
            break;

          // [Currently] These are all the Enums we might encounter
          case "PollType":         
          case "ProductID":
          case "DeviceType":
          case "AnswerFormat":
          case "Sex":
            newval = Enum.Parse(property.PropertyType, newval.ToString(), true);
            break;

          case "String":
            Debug.Fail("Unaccounted for data conversion required:   From  " + Tools.ObjectType(newval) + "  to String", "Controller.ChangePropertyValue");
            return 0;

          default:
            Debug.Fail("Unaccounted for data type: " + propertyType, "Controller.ChangePropertyValue (2)");
            break;
        }
      }

      // Note: "==" doesn't work because we're comparing objects.
      if (oldval.Equals(newval) || newval.ToString() == pollForm.DefaultQuestionText)
        return 1;   // Values are identical so no need to change them

      // Turn off listener
      string[] propPathList = propPath.Split(new char[] {'.'});  // Used by some of the paths below

      // If this event occurs within the Choices collection then we need to handle it differently.  This is because an event
      // fired there does not call 'PropertyEventHandler', but instead calls an internal event handling method, which in turn
      // calls the event handler within the Question object.  So what we'll do is fool it by disabling (and then reenabling)
      // the event handler in the Question object.
      if (propPath.IndexOf(".Choices[") != -1)
        ActivateEventHandlers(model, propPathList[0] + ".Text", false);   // Fool it into thinking that we're deactivating this property's handler
      else
        ActivateEventHandlers(model, propPath, false);

      // Set property value
      try
      {
        Tools.SetPropertyValue(model, propPath, newval);
      
        if (NeedToUpdatePreview(propPath))
          pollForm.UpdatePreview(pollModel, CurrentQuestion, CurrentAnswerFormat);
      }
      catch
      {
        Debug.Fail("Error trying to set '" + propPath + "' with this value: " + newval.ToString(), "Controller.ChangePropertyValue");
        return 0;
      }

      // Turn listener back on
      if (propPath.IndexOf(".Choices[") != -1)
        ActivateEventHandlers(model, propPathList[0] + ".Text", true);   // Reactivate this property's handler
      else
        ActivateEventHandlers(model, propPath, true);
      
      return 2;   // Successfully modified Property value
    }



    /// <summary>
    /// Lets one quickly turn on/off the property event handlers in the data model.  It first locates
    /// where a given property is situated in the model and then looks within this property's containing 
    /// class to see if it can turn on or off (depending on 'setting') the event handler(s).
    /// Note: In PocketPollster there will just be one property event handler in every nested class
    ///       but we're building this method to be more generic.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="propname"></param>
    /// <param name="setting"></param>     // true - Turn On      false - Turn Off
    public void ActivateEventHandlers(Poll model, string propPath, bool setting)
    {
      object[] indexer = new object[0];      // Used by property GetValue statements
      string[] propPathList = propPath.Split(new char[] {'.'});

      NodeInfo parentNode = new NodeInfo();
      NodeInfo currentNode = new NodeInfo();

      currentNode.Obj = model;      
      currentNode.ObjType = model.GetType();

      for (int i = 0; i < propPathList.Length; i++)
      {
        string node = propPathList[i];

        // See if this node is a collection
        if (node.EndsWith("]"))
        {
          currentNode.IsCollection = true;
          string sIdx = node.Substring(node.IndexOf('['));
          sIdx = sIdx.TrimStart(new char[] {'['});
          sIdx = sIdx.TrimEnd(new char[] {']'});
          currentNode.Idx= Convert.ToInt32(sIdx);
          currentNode.PropName = node.Substring(0, node.IndexOf("["));   // Strip away index info, leaving just collection's name
        }
        else
        {
          currentNode.IsCollection = false;
          currentNode.PropName = node;
        }

        PropertyInfo propInfo;
        
        if (currentNode.ParentIsCollection)
        {
          propInfo = (parentNode.Obj as PPbaseCollection)[parentNode.Idx].GetType().GetProperty(currentNode.PropName);

          if (propInfo != null)
          {
            if (i == propPathList.Length - 1)
            {
              // Locate all of the events for this class in the model
              EventInfo[] eventList = (parentNode.Obj as PPbaseCollection)[parentNode.Idx].GetType().GetEvents();

              if (eventList != null)
              {
                // And turn all of these events on or off, depending on what was requested
                foreach (EventInfo eventInfo in eventList)
                {
                  if (setting == true)
                    eventInfo.AddEventHandler((parentNode.Obj as PPbaseCollection)[parentNode.Idx], new ASMEventHandler(PropertyEventHandler));
                  else
                    eventInfo.RemoveEventHandler((parentNode.Obj as PPbaseCollection)[parentNode.Idx], new ASMEventHandler(PropertyEventHandler));
                }
              }
            }
            else
              currentNode.Obj = propInfo.GetValue((parentNode.Obj as PPbaseCollection)[parentNode.Idx], indexer);
          }
        }
        else
        {
          propInfo = currentNode.ObjType.GetProperty(currentNode.PropName);

          if (propInfo != null)
          {
            if (i == propPathList.Length - 1)
            {
              // Locate all of the events for this class in the model
              EventInfo[] eventList = currentNode.Obj.GetType().GetEvents();

              if (eventList != null)
              {
                // And turn all of these events on or off, depending on what was requested
                foreach (EventInfo eventInfo in eventList)
                {
                  if (setting == true)
                    eventInfo.AddEventHandler(currentNode.Obj, new ASMEventHandler(PropertyEventHandler));
                  else
                    eventInfo.RemoveEventHandler(currentNode.Obj, new ASMEventHandler(PropertyEventHandler));
                }
              }
            }
            else
              currentNode.Obj = propInfo.GetValue(currentNode.Obj, indexer);
          }
        }

        currentNode.ObjType = propInfo.PropertyType;   // Note: This is preferable to 'currentNode.Obj.GetType()' because this fails if a null string

        // Prepare for next iteration through loop
        parentNode.Clear();
        if (currentNode.IsCollection)
        {
          currentNode.CopyTo(parentNode);
          currentNode.Clear();
          currentNode.ParentIsCollection = true;
        }
      }
    }    // End of 'ActivateEventHandlers'



    /// <summary>
    /// This method does everything necessary to establish automated synchronicity between
    /// the "smart" controls of pollForm and their associated Properties in pollModel.
    /// </summary>
    private void InitializeASM()
    {
      BuildASMlist(pollForm.Controls, pollModel, ASMlist);
    }



    /// <summary>
    /// Retrieves the specially marked 'Tag' property strings and decodes them into their constitutent components:
    ///   Property=  The name of property incl. any nested classes it sits within; a collection is represented with "[]"
    ///                 examples:   Questions[].Text       CreationInfo.PurgeDuration
    ///   Value=     The value that a given control represents; commonly used with a series of radio buttons
    ///   Default=   The default value to display (currently only coded for labels and textboxes)
    ///   Source=    A future property - The idea is to think about a simple mechanism to have the control populated from
    ///              several sources, like an Enum, a RecordSet, etc. 
    ///       
    /// Notes:
    ///  - These keywords are case insensitive but the actual Property paths ARE case-sensitive!
    ///  - If there is more than one keyword then it (and its value) should be CDF
    ///  - No quotation marks should be used unless they're actual characters to be displayed
    ///  - Use the Default property carefully because it will be used if the control's value if ever set to "".
    ///    One may not want the control suddenly reverting back to the original text just because the user
    ///    temporarily erased it.
    /// 
    /// The property paths are used as keys for building a collection consisting of this form control data.
    /// </summary>
    /// <param name="allcontrols"></param>
    /// <param name="model"></param>
    /// <param name="ASMlist"></param>
    private void BuildASMlist(Control.ControlCollection allcontrols, Poll model, SortedList ASMlist)
    {
      foreach(Control ctrl in allcontrols) 
      {
        if(ctrl.HasChildren) 
        {
          BuildASMlist(ctrl.Controls, model, ASMlist);   // Recursively call method to get child controls as well
        }

        // This next line will add this control into the ASMlist collection, and if successful,
        // will setup an ASM Event Handler for the control
        if (ASM.ActivateASM(ASMlist, ctrl))
        {
          // While we're here, let's establish an event handler for all controls whose associated properties
          // don't exist within a collection.  The ones within a collection need to be added dynamically later.
//          if (propertyPath.IndexOf("[") == -1)
//            pollForm.EstablishASMEventHandlers(ctrl);

          // For now we'll establish event handlers for all the controls we encounter
          pollForm.EstablishASMEventHandlers(ctrl);
        }
      }
    }



    /// <summary>
    /// Prepares a brand new poll with the following:
    ///   - 1 blank question
    ///   - 1 blank answer (choice)
    /// This is done as a visual cue to help the user get started.
    /// </summary>
    /// <param name="model"></param>
    private void InitNewPoll(Poll model)
    {
      // Populate memory object with as much default information as possible.  As mentioned elsewhere, 
      // this population must occur ONLY AFTER the object has been instantiated and the Event Delegates
      // are wired in.  Note: This method can ONLY be used for a brand new poll!
      model.Initialize();
 
      // Create default question in poll; this also creates a default choice
      AddDefaultQuestion(model);

      // Prepare all controls on the Summary and Responses tab pages that involve Responses data
      pollForm.UpdateCollectedData(pollModel);
    }



    private void AddDefaultQuestion(Poll model)
    {
      IsDirty = true;

      _Question quest = new _Question(model.Questions);
      quest.ID = -1;                                       // -1 signifies that AutoNumber will be used to get the next incremental ID
      quest.AnswerFormat = AnswerFormat.Standard;          // May want to grab current setting from AnswerButtons

      model.Questions.Add(quest);
      quest.ModelEvent += new ASMEventHandler(PropertyEventHandler);

      // Prepare the necessary Question buttons: 1 button for each question in the model
      pollForm.UpdateQuestionButtons(model);

      // Prepare the initial [blank] Choice
      AddDefaultChoice(model, 0);
      
      // Debug: Try doing without the next line
      //pollForm.UpdateChoices(model, ASMlist, CurrentQuestion, CurrentAnswerFormat);

      // Set current question in form.  For now, this will be the first question = Question #0.
      // In the future we could store the number of the last question worked on and go back to it immediately.
      CurrentQuestion = 0;
      pollForm.CurrentChoice = 0;     // Also set the current choice
    }



    private void AddDefaultChoice(Poll model, int qIdx)
    {
      _Question quest = model.Questions[qIdx];

      // Add a default Choice
      _Choice choice = new _Choice(quest.Choices);
      choice.ID = -1;  // -1 signifies that AutoNumber will be used to get the next incremental ID
      quest.Choices.Add(choice);
    }



    private bool NeedToUpdatePreview(string propPath)
    {
      if (propPath.IndexOf("Questions[") != -1)
      {
        // We'll assume that we're always working with the current question but let's add a check anyhow
        int qIdx = Tools.GetIndex(propPath);
        if (qIdx != CurrentQuestion)
          Debug.Fail("Non current question being processed: propPath = " + propPath, "Controller.NeedToUpdatePreview");

        Tools.RemoveIndices(ref propPath);

        // Note: In the next line, the string MUST end with a comma!
        string propPathList = "Questions[].Text, Questions[].Choices[].Text, Questions[].Choices[].MoreText, Questions[].Choices[].MoreText2,";

        if (propPathList.IndexOf(propPath + ",") != -1)
          return true;
        else
          return false;
      }
      else
        return false;
    }



    /// <summary>
    /// This event handler is fired whenever a Response button on the Responses page of frmPoll is pressed.
    /// The value of the 'CurrentRespondent' property is currently not utilized but may be in the future.
    /// </summary>
    private void CurrRespondentChangedEvent(int currRespondent, EventArgs e)
    {
      CurrentRespondent = currRespondent;
    }


    /// <summary>
    /// This event handler is fired whenever the "Hide Question Numbers" checkbox is changed in frmPoll.
    /// </summary>
    private void HideQuestionNumbersChangedEvent(object sender, EventArgs e)
    {
      pollForm.UpdatePreview(pollModel, CurrentQuestion, CurrentAnswerFormat);
    }


    private void PurgeDurationChangedEvent(object sender, EventArgs e)
    {
      ComboBox comboBox = sender as ComboBox;
      int idx = comboBox.SelectedIndex;

      pollModel.CreationInfo.PurgeDuration = (int) Enum.GetValues(typeof(PurgeDuration)).GetValue(idx);  
    }


    private void GraphInfoChangedEvent(object sender, EventArgs e)
    {
      IsDirty = true;
    }


    /// <summary>
    /// Determines whether the poll has been published.  Two tests are done to ascertain this:
    ///    1. Whether Respondents > 0
    ///    2. Whether a Template file with the same name exists
    /// </summary>
    /// <returns></returns>
    private bool CheckIfPublished()
    {
      if (pollModel.Respondents.Count > 0)
        return true;

      string templateName = pollName + Tools.GetAppExt();
      if (File.Exists(SysInfo.Data.Paths.Templates + templateName))
        return true;

      return false;
    }



  }
}
