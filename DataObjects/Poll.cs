using System;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;      // Note: A specific reference to 'System.Drawing' had to be added to allow this line to be used!


namespace DataObjects
{
  /// <summary>
  /// The 'Poll' data structure consists of these major sections:
  /// 
  /// Name                  Type
  /// ---------------------------------
  /// CreationInfo          Class
  /// Instructions          Class
  /// Questions             Collection
  /// Respondents           Collection
  /// PollingInfo           Class
  /// PollsterPrivileges    Class
  /// 
  /// 
  /// Important Note: One inherent limitation of the Auto Synchronization Mechanism (ASM) employed
  ///                 is that no two properties can share the same name, no matter that they exist
  ///                 in separate nested classes.
  ///                 
  /// </summary>

  public class Poll
  {
    // This is the event handler that will be used by all of the properties in all of the nested classes.
    public delegate void ASMEventHandler (string propName, object propValue, EventArgs e);

    // This is the event handler that will be used by all collections in this class.
    // It'll be called whenever a collection element is added or removed.
    public delegate void ASMCollectionEventHandler (string collName, string Operation, int index, EventArgs e);


    // In this class we will have two types of objects at the next level below the root:
    //   - Nested Classes
    //   - Collections

    #region NestedClassesAndCollections

    // Here we instantiate the nested classes.  We need to do it in this slightly more sophisticated way
    // (than we did originally) so that a true parent-child relationship is established with the
    // instantiated objects.

    private _CreationInfo instanceCreationInfo = new _CreationInfo();
    public _CreationInfo CreationInfo 
    {
      get 
      { 
        return instanceCreationInfo;
      }
    }

    private _Instructions instanceInstructions = new _Instructions();
    public _Instructions Instructions 
    {
      get 
      { 
        return instanceInstructions;
      }
    }


    private _Questions instanceQuestions = new _Questions();
    public _Questions Questions 
    {
      get 
      { 
        return instanceQuestions;
      }
    }

    private _Respondents instanceRespondents = new _Respondents();
    public _Respondents Respondents 
    {
      get 
      { 
        return instanceRespondents;
      }
    }

    private _PollingInfo instancePollingInfo = new _PollingInfo();
    public _PollingInfo PollingInfo 
    {
      get 
      { 
        return instancePollingInfo;
      }
    }

    private _PollsterPrivileges instancePollsterPrivileges = new _PollsterPrivileges();
    public _PollsterPrivileges PollsterPrivileges 
    {
      get 
      { 
        return instancePollsterPrivileges;
      }
    }

    #endregion


    // Constructor for brand new [empty] polls
    public Poll()
    {
    }

    // Constructor for existing polls  (Necessary?)
    public Poll(string PollName)
    {
      // This constructor needs to be fleshed out if it's going to be used
    }


    // Declare the name of the solo event handler for this object.
    // This event will then be subscribed to by the Controller.

    // Important Note: I've later discovered an inherent flaw with having just one event for all the properties.
    //                 If a connected form object event necessitates a property value being changed then the
    //                 listener (event) needs to be temporarily disabled.  This would mean that if any property is
    //                 being altered almost at the same time then necessary event to reflect the change back in the
    //                 form(s) will not take effect.  So do need to move to an EventHandlerList or some other strategy.

    /// <summary>
    /// Initializes a Brand New Poll with as much default information as possible (ex. Creator ID, Creation Date, etc.)
    /// Note: This method must exist separately from the class constructor(s) because it can ONLY be called after:
    ///          - An object is instantiated from this class
    ///          - Event delegates are wired in
    /// </summary>
    
    #if (! CF)
      public void Initialize()
      {
        CreationInfo.PollGuid = System.Guid.NewGuid().ToString();                            // Create a unique ID for this poll
        CreationInfo.PollType = Constants.PollType.Standard;                                 // "Standard" polltype is the only type available [for now]
        CreationInfo.ProductID = SysInfo.Data.Admin.ProductID;
        CreationInfo.VersionNumber = SysInfo.Data.Admin.VersionNumber;
        CreationInfo.CreatorDeviceType = Constants.DeviceType.Windows;
        CreationInfo.CreatorDeviceGuid = SysInfo.Data.Admin.Guid;
        CreationInfo.CreatorName = Tools.UserNameFormat(SysInfo.Data.Options.PrimaryUser);
        CreationInfo.CreationDate = DateTime.Now;
        CreationInfo.GetPersonalInfo = SysInfo.Data.Options.Mobile.GetPersonalInfo;
        CreationInfo.HideQuestionNumbers = SysInfo.Data.Options.Mobile.HideQuestionNumbers;
        CreationInfo.LastEditedBy = CreationInfo.CreatorName;
        CreationInfo.LastEditDate = CreationInfo.CreationDate;
        CreationInfo.PurgeDuration = (int) SysInfo.Data.Options.Mobile.PurgeDuration;
        
        PollsterPrivileges.ReviewData = true;
        PollsterPrivileges.CanAbortRecord = true;
      }
    #endif

  }  // end of class Poll



  #region CreationInfo

  // This is the definition for the nested class '_CreationInfo'
  public class _CreationInfo
  {
    public event Poll.ASMEventHandler ModelEvent;


    // Constructor for "CreationInfo" class
    public _CreationInfo()
    {
      UserPrivileges = new ArrayList();
    }


    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to
    // these events and correctly sets the corresponding linked form control.
    private void FireASM(string propertyName, object propertyValue, EventArgs e)
    {
      try
      {
        if (ModelEvent != null)
        {
          // Because this method resides in a nested class we need to tack on information
          // that specifies which nested class the event is being fired from.
          propertyName = "CreationInfo." + propertyName;
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in Poll model", "CreationInfo");
      }
    }


    // This is the "shortcut" 2-parameter version of the FireASM method.  It's designed
    // to be called from properties at the root of the class, whereas the 3-parameter
    // version is called from properties of hierarchical classes, if they exist.
    private void FireASM(string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }


    // Properties:

    private string _pollGuid = "";
    public string PollGuid
    {
      get
      {
        return _pollGuid;
      }
      set
      {
        _pollGuid = value;
        FireASM("PollGuid", _pollGuid);
      }
    }

    private int _revision = 0;
    public int Revision
    {
      get
      {
        return _revision;
      }
      set
      {
        _revision = value;
        FireASM("Revision", _revision);
      }
    }

    private Constants.PollType _polltype;
    public Constants.PollType PollType
    {
      get
      {
        return _polltype;
      }
      set
      {
        _polltype = value;
        FireASM("PollType", _polltype);
      }
    }

    private Constants.ProductID _productID;
    public Constants.ProductID ProductID
    {
      get
      {
        return _productID;
      }
      set
      {
        _productID = value;
        FireASM("ProductID", _productID);
      }
    }

    private string _versionnumber = "";
    public string VersionNumber
    {
      get
      {
        return _versionnumber;
      }
      set
      {
        _versionnumber = value;
        FireASM("VersionNumber", _versionnumber);
      }
    }

    private string _pollSummary;
    public string PollSummary
    {
      get
      {
        return _pollSummary;
      }
      set
      {
        _pollSummary = value;
        FireASM("PollSummary", _pollSummary);
      }
    }

    private Constants.DeviceType _creatordevicetype;
    public Constants.DeviceType CreatorDeviceType
    {
      get
      {
        return _creatordevicetype;
      }
      set
      {
        _creatordevicetype = value;
        FireASM("CreatorDeviceType", _creatordevicetype);
      }
    }

    private string _creatorDeviceGuid = "";
    public string CreatorDeviceGuid
    {
      get
      {
        return _creatorDeviceGuid;
      }
      set
      {
        _creatorDeviceGuid = value;
        FireASM("CreatorDeviceGuid", _creatorDeviceGuid);
      }
    }

    private string _creatorName;
    public string CreatorName
    {
      get
      {
        return _creatorName;
      }
      set
      {
        _creatorName = value;
        FireASM("CreatorName", _creatorName);
      }
    }

    private DateTime _creationdate;
    public DateTime CreationDate
    {
      get
      {
        return _creationdate;
      }
      set
      {
        _creationdate = value;
        FireASM("CreationDate", _creationdate);
      }
    }

    private string _openPassword = "";
    public string OpenPassword
    {
      get
      {
        return _openPassword;
      }
      set
      {
        _openPassword = value;
        FireASM("OpenPassword", _openPassword);
      }
    }

    private string _modifyPassword = "";
    public string ModifyPassword
    {
      get
      {
        return _modifyPassword;
      }
      set
      {
        _modifyPassword = value;
        FireASM("ModifyPassword", _modifyPassword);
      }
    }

    private bool _getPersonalInfo = true;
    public bool GetPersonalInfo
    {
      get
      {
        return _getPersonalInfo;
      }
      set
      {
        _getPersonalInfo = value;
        FireASM("GetPersonalInfo", _getPersonalInfo);
      }
    }

    private bool _hideQuestionNumbers;
    public bool HideQuestionNumbers
    {
      get
      {
        return _hideQuestionNumbers;
      }
      set
      {
        _hideQuestionNumbers = value;
        FireASM("HideQuestionNumbers", _hideQuestionNumbers);
      }
    }

    private string _lastEditedBy;
    public string LastEditedBy
    {
      get
      {
        return _lastEditedBy;
      }
      set
      {
        _lastEditedBy = value;
        FireASM("LastEditedBy", _lastEditedBy);
      }
    }

    private DateTime _lasteditdate;
    public DateTime LastEditDate
    {
      get
      {
        return _lasteditdate;
      }
      set
      {
        _lasteditdate = value;
        //FireASM("LastEditDate", _lasteditdate.ToShortDateString());
        FireASM("LastEditDate", _lasteditdate);
      }
    }

    private string _lastEditGuid = "";
    public string LastEditGuid
    {
      get
      {
        return _lastEditGuid;
      }
      set
      {
        _lastEditGuid = value;
        FireASM("LastEditGuid", _lastEditGuid);
      }
    }

    private int _purgeDuration;
    public int PurgeDuration
    {
      get
      {
        return _purgeDuration;
      }
      set
      {
        _purgeDuration = value;
        
        // Turned off because too complicated to get this property working with ASM (integral values don't correspond with combobox index values)
        //FireASM("PurgeDuration", _purgeDuration);
      }
    }

    private bool _exportEnabled;
    public bool ExportEnabled
    {
      get
      {
        return _exportEnabled;
      }
      set
      {
        _exportEnabled = value;
        FireASM("ExportEnabled", _exportEnabled);
      }
    }

    private string _exportFilename = "";
    public string ExportFilename
    {
      get
      {
        return _exportFilename;
      }
      set
      {
        _exportFilename = value;
        FireASM("ExportFilename", _exportFilename);
      }
    }

    private DateTime _lastCompact;
    public DateTime LastCompact
    {
      get
      {
        return _lastCompact;
      }
      set
      {
        _lastCompact = value;
      }
    }

    private DateTime _doNotUseBeforeDate;
    public DateTime DoNotUseBeforeDate
    {
      get
      {
        return _doNotUseBeforeDate;
      }
      set
      {
        _doNotUseBeforeDate = value;
        FireASM("DoNotUseBeforeDate", _doNotUseBeforeDate);
      }
    }

    private DateTime _doNotUseAfterDate;
    public DateTime DoNotUseAfterDate
    {
      get
      {
        return _doNotUseAfterDate;
      }
      set
      {
        _doNotUseAfterDate = value;
        FireASM("DoNotUseAfterDate", _doNotUseAfterDate);
      }
    }


    // ToDo: May have to expand complexity of this next line:
    public ArrayList UserPrivileges;
  }


  // Minor classes that belong inside the "CreationInfo" class

  // Structure of the elements that will populate the UserPrivileges collection
  // To Do: This code to be expanded when we're focusing on this class.
  public class UserPrivilege
  {
    public uint UserID;
    public bool CanOpen;
    public bool CanEdit;
  }

  #endregion


  #region Instructions

  // This is the definition for the nested class '_Instructions'

  public class _Instructions
  {
    //public delegate void ASMEventHandler (string propname, object propvalue, Type proptype, System.EventArgs e);
    public event Poll.ASMEventHandler ModelEvent;

    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to these
    // events and correctly sets the corresponding linked form control.
    private void FireASM(string propertyName, object propertyValue, EventArgs e)
    {
      try
      {
        if (ModelEvent != null)
        {
          // Because this method resides in a nested class we need to tack on information
          // that specifies which nested class the event is being fired from.
          propertyName = "Instructions." + propertyName;
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in Poll model", "Instructions");
      }
    }

    private void FireASM(string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }


    // Properties:

    private string _beforepoll = "";
    public string BeforePoll
    {
      get
      {
        return _beforepoll;
      }
      set
      {
        _beforepoll = value;
        FireASM("BeforePoll", _beforepoll);
      }
    }

    private bool _repeatbeforepoll;
    public bool RepeatBeforePoll
    {
      get
      {
        return _repeatbeforepoll;
      }
      set
      {
        _repeatbeforepoll = value;
        FireASM("RepeatBeforePoll", _repeatbeforepoll);
      }
    }

    private string _beginmessage = "";
    public string BeginMessage
    {
      get
      {
        return _beginmessage;
      }
      set
      {
        _beginmessage = value;
        FireASM("BeginMessage", _beginmessage);
      }
    }

    private string _afterpoll = "";
    public string AfterPoll
    {
      get
      {
        return _afterpoll;
      }
      set
      {
        _afterpoll = value;
        FireASM("AfterPoll", _afterpoll);
      }
    }

    private bool _repeatafterpoll;
    public bool RepeatAfterPoll
    {
      get
      {
        return _repeatafterpoll;
      }
      set
      {
        _repeatafterpoll = value;
        FireASM("RepeatAfterPoll", _repeatafterpoll);
      }
    }

    private string _endmessage = "";
    public string EndMessage
    {
      get
      {
        return _endmessage;
      }
      set
      {
        _endmessage = value;
        FireASM("EndMessage", _endmessage);
      }
    }

    private string _afterallpolls = "";
    public string AfterAllPolls
    {
      get
      {
        return _afterallpolls;
      }
      set
      {
        _afterallpolls = value;
        FireASM("AfterAllPolls", _afterallpolls);
      }
    }
  }

  #endregion


  #region Questions

  #region _Questions
  // This is the definition for the collection class '_Questions', which will be instantiated into the collection 'Questions'.
  // It is inherited from 'PPbaseCollection' for two reasons:
  //  1. Provides safety from null elements.
  //  2. Simplifies casting within the ASM code.

  public class _Questions : PPbaseCollection
  {
    public event Poll.ASMCollectionEventHandler CollectionEvent;

    // This method is called from the following methods of this collection class
    //   - Add
    //   - Insert
    //   - Remove
    // It invokes the Automatic Synchronization Mechanism (ASM).  The Controller 
    // subscribes to these events and then responds to them accordingly.
    private void FireASMc(string collName, string Operation, int index)
    {
      try
      {
        if (CollectionEvent != null)
        {
          CollectionEvent(collName, Operation, index, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASMc Exception in Poll model", "Questions");
      }
    }


    // Indexer
    public new _Question this[int index]
    {
      get
      {
        //throws ArgumentOutOfRangeException
        return (_Question) base[index];
      }
      set
      {
        //throws ArgumentOutOfRangeException
        base.Insert(index, value);
      }
    }

    public int Add(_Question value)
    {
      int index = base.Add(value);

      // ASM code needs to go here!
      FireASMc("Questions", "Add", index);

      return index;
    }

    public void Insert(int index, _Question value)
    {
      base.Insert(index, value);
      // ASM code needs to go here!
    }

    public void Remove(_Question value)
    {
      base.Remove(value);
      // ASM code needs to go here!
    }

    public bool Contains(_Question value)
    {
      return base.Contains(value);
    }

    // Returns the index position that the 'value' occupies in the collection.
    public int IndexOf(_Question value)
    {
      return base.IndexOf(value);
    }

    // Searches the questions in the collection for a question with the specified text.
    // Returns 'true' if found, 'false' otherwise.
    public bool ContainsText(string text)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Question question = (_Question) List[i];
        if (question.Text == text)
          return true;
      }

      return false;  // If reaches here then a question with the specified text was not found
    }

    // Searches the questions in the collection for a question with the specified text.
    // Returns the question if it finds it, otherwise 'null'.
    public _Question Find_Question(string text)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Question question = (_Question) List[i];
        if (question.Text == text)
          return question;
      }

      return null;  // If reaches here then a question with the specified text was not found
    }

    // Add other type-safe methods here
    // ...
    // ...
  }

  #endregion


  #region _Question
  // This is the definition for the class '_Question', which will populate the collection 'Questions'.
  public class _Question
  {
    private _Questions questions;


    /// <summary>
    /// This is the default constructor for the '_Question' class.  We must pass it the parent
    /// collection it resides in so that it knows which Index number it has within this collection.
    /// </summary>
    /// <param name="questions"></param>
    public _Question(_Questions questions)
    {
      this.questions = questions;
      instanceChoices = new _Choices(this);  // Instantiate a new, albeit empty, Choices collection, passing this instance to "_Choices"
    }


    // This read-only property maintains the Index number of the instantiated object in the parent collection.
    public int Index
    {
      get
      {
        return this.questions.IndexOf(this);
      }
    } 

    public event Poll.ASMEventHandler ModelEvent;

    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to these
    // events and correctly sets the corresponding linked form control.
    public void FireASM (string propertyName, object propertyValue, EventArgs e)   // Think about later changing "public" to "protected" or "internal"
    {
      try
      {
        // Because this method resides in a class that itself resides in a collection class,
        // we need to tack on information that specifies which element in the collection the
        // event is being fired from.
        propertyName = "Questions[" + Index.ToString() + "]." + propertyName;

        if (ModelEvent != null)
        {
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in Poll model", "Question");
      }
    }

    private void FireASM(string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }
    


    /// <summary>
    /// This method clones (makes a deep copy of) the object and places the data in 'destQuest', which is thus passed back by reference.
    /// Note: I'm not sure if this is the most effective way to deep clone a Question object, but it's the only way I figured out how to do it!
    /// </summary>
    /// <param name="destQuest">The Question object that we'll copy the data to</param>
    /// <returns></returns>
    public void Clone(_Question destQuest)
    {
      destQuest.AnswerFormat = this.AnswerFormat;
      destQuest.GraphType = this.GraphType;
      destQuest.ID = -1;
      destQuest.Mandatory = this.Mandatory;
      destQuest.Notes = this.Notes;
      destQuest.Picture = this.Picture;
      destQuest.Text = this.Text;

      foreach(_Choice srcChoice in this.Choices)
      {
        _Choice destChoice = new _Choice(destQuest.Choices);
        srcChoice.Clone(destChoice);
        destQuest.Choices.Add(destChoice);
      }
    }


    // Properties:

    // Note: The use of this static int variable doesn't quite work as we'd like.  Ideally we'd like to see it reset to -1
    //       with each new poll.  But that doesn't seem to be happening.

    private static int maxID = -1;  // The largest ID value used so far by any question
    
    // Note: As noted just above, maxID isn't working exactly the way we'd like - both here and every other place that an "ID" is used.
    //       What would be ideal would be if the ID parameter were reset to '0' at the start of every collection (ie. equal to 'Index').
    //       But the ultimate purpose of ID is to act as a permanent identifier to the record in question.  So, in fact, no matter what
    //       its value, as long as it is unique (& unchanging) then it still serves its primary purpose perfectly fine.

    private int _id;
    public int ID
    {
      get
      {
        return _id;
      }
      set
      {
        _id = value;
        if (_id == -1)
        {
          maxID++;
          _id = maxID;
        }
        else if (_id > maxID)
          maxID = _id;

        // We'll turn off for now, since there's no control associated with this property
        //FireASM("ID", _id);
      }
    }

    private bool _mandatory;
    public bool Mandatory
    {
      get
      {
        return _mandatory;
      }
      set
      {
        _mandatory = value;
        FireASM("Mandatory", _mandatory);
      }
    }

    private string _text = "";
    public string Text
    {
      get
      {
        return _text;
      }
      set
      {
        _text = value;
        FireASM("Text", _text);
      }
    }

    private Image _picture;
    public Image Picture
    {
      get
      {
        return _picture;
      }
      set
      {
        _picture = value;
        FireASM("Picture", _picture);
      }
    }

    private string _notes = "";
    public string Notes
    {
      get
      {
        return _notes;
      }
      set
      {
        _notes = value;
        FireASM("Notes", _notes);
      }
    }

    private Constants.AnswerFormat _answerformat;
    public Constants.AnswerFormat AnswerFormat
    {
      get
      {
        return _answerformat;
      }
      set
      {
        _answerformat = value;
        FireASM("AnswerFormat", _answerformat);
      }
    }
  
    private Constants.GraphType _graphType = Constants.GraphType.Bar;   // We'll leave this default value for now
    public Constants.GraphType GraphType
    {
      get
      {
        return _graphType;
      }
      set
      {
        _graphType = value;
        FireASM("GraphType", _graphType);
      }
    }

    private Constants.GraphAvgMethod _graphAvgMethod = Constants.GraphAvgMethod.Simple;   // We'll leave this default value for now
    public Constants.GraphAvgMethod GraphAvgMethod
    {
      get
      {
        return _graphAvgMethod;
      }
      set
      {
        _graphAvgMethod = value;
        FireASM("GraphAvgMethod", _graphAvgMethod);
      }
    }

    private _Choices instanceChoices;
    public _Choices Choices 
    {
      get 
      { 
        return instanceChoices;
      }
    }
  }
  #endregion  // End of class _Question


  #region _Choices
  // This is the definition for the collection class '_Choices', which will be instantiated into the collection 'Choices'.
    public class _Choices : PPbaseCollection
  {
    private _Question question;  // Internal reference to parent class

    /// <summary>
    /// This is the constructor for the '_Choices' class.  We must pass it the parent class it resides in.
    /// </summary>
    /// <param name="choices"></param>
    public _Choices(_Question question)
    {
      this.question = question;
    }

    public new _Choice this[int index]
    {
      get
      { 
        return (_Choice) base[index];
      }
      set 
      { 
        base.Insert(index, value);
      }
    }

    public int Add(_Choice choice)
    {
      choice.ModelEvent += new Poll.ASMEventHandler(question.FireASM);
      return base.Add(choice);
    }

    public void Insert(int index, _Choice choice)
    {
      choice.ModelEvent += new Poll.ASMEventHandler(question.FireASM);
      base.Insert(index, choice);
    }

    public void Remove(_Choice choice)
    {
      choice.ModelEvent -= new Poll.ASMEventHandler(question.FireASM);
      base.Remove(choice);
    }

    public bool Contains(_Choice choice)
    {
      return base.Contains(choice);
    }

    // Returns the index position that the 'choice' occupies in the collection.
    public int IndexOf(_Choice choice)
    {
      return base.IndexOf(choice);
    }


    // *** Need to test these remaining methods! ***

    // Searches the choices in the collection for a choice with the specified text.
    // Returns 'true' if found, 'false' otherwise.
    public bool ContainsText(string text)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Choice choice = (_Choice) List[i];
        if (choice.Text == text)
          return true;
      }

      return false;  // If reaches here then a choice with the specified text was not found
    }

    // Searches the choices in the collection for a choice with the specified text.
    // Returns the choice if it finds it, otherwise 'null'.
    public _Choice Find_Choice(string text)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Choice choice = (_Choice) List[i];
        if (choice.Text == text)
          return choice;
      }

      return null;  // If reaches here then a choice with the specified text was not found
    }

    // Searches the choices in the collection for a choice with the specified ID.
    // Returns the choice if it finds it, otherwise 'null'.
    public _Choice Find_Choice(int ID)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Choice choice = (_Choice) List[i];
        if (choice.ID == ID)
          return choice;
      }

      return null;  // If reaches here then a choice with the specified ID was not found
    }

    // Searches the choices in the collection for a choice with the specified ID.
    // If it finds it, returns the index position that this choice occupies; otherwise -1.
    public int IndexOfByID(int ID)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Choice choice = (_Choice) List[i];
        if (choice.ID == ID)
          return i;
      }

      return -1;  // If reaches here then a choice with the specified ID was not found
    }
  
    // Retrieves the text value from each Choice and returns them in an ArrayList.
    public ArrayList GetAllTextValues()
    {
      ArrayList arrayList = new ArrayList();

      for (int i = 0; i < List.Count; i++)
      {
        _Choice choice = (_Choice) List[i];
        arrayList.Add(choice.Text);
      }

      return arrayList;
    }

//    public string[] GetAllTextValues()
//    {
//      string[] stringArray = new string[List.Count];
//
//      for (int i = 0; i < List.Count; i++)
//      {
//        _Choice choice = (_Choice) List[i];
//        stringArray[i] = choice.Text;
//      }
//
//      return stringArray;
//    }






    // Add other type-safe methods here
    // ...
    // ...
  }

  #endregion


  #region _Choice
  // This is the definition for the class '_Choice', which will populate the collection 'Choices'.
  public class _Choice
  {
    private _Choices choices;

    /// <summary>
    /// This is the constructor for the '_Choice' class.  We must pass it the parent collection
    /// it resides in so that it knows which Index number it has within this collection.
    /// </summary>
    /// <param name="Choices"></param>
    public _Choice(_Choices choices)
    {
      this.choices = choices;
    }

    // This read-only property maintains the Index number of the instantiated object in the parent collection.
    public int Index
    {
      get
      {
        return this.choices.IndexOf(this);
      }
    } 
    
    public event Poll.ASMEventHandler ModelEvent;

    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to these
    // events and correctly sets the corresponding linked form control.
    private void FireASM(string propertyName, object propertyValue, EventArgs e)
    {
      try
      {
        if (ModelEvent != null)
        {
          // Because this method resides in a class that itself resides in a collection class,
          // we need to tack on information that specifies which element in the collection the
          // event is being fired from.
          propertyName = "Choices[" + Index.ToString() + "]." + propertyName;
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in Poll model", "Choice");
      }
    }

    private void FireASM(string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }

    public void Clone(_Choice destChoice)
    {
      destChoice.CorrectAnswer = this.CorrectAnswer;
      destChoice.ExtraInfo = this.ExtraInfo;
      destChoice.ExtraInfoMultiline = this.ExtraInfoMultiline;
      destChoice.GotoQuestionID = this.GotoQuestionID;
      destChoice.ID = -1;   // IDs must be unique so makes sense not to copy the original ID
      destChoice.MoreText = this.MoreText;
      destChoice.MoreText2 = this.MoreText2;
      destChoice.Multimedia = this.Multimedia;
      destChoice.Picture = this.Picture;
      destChoice.ShowMultimediaNow = this.ShowMultimediaNow;
      destChoice.Text = this.Text;
      destChoice.URL = this.URL;
    }


    // Properties:

    private static int maxID = -1;  // The largest ID value used so far by any choice
    
    private int _id;
    public int ID
    {
      get
      {
        return _id;
      }
      set
      {
        _id = value;
        if (_id == -1)
        {
          maxID++;
          _id = maxID;
        }
        else if (_id > maxID)
          maxID = _id;

        // We'll turn off for now, since there's no control associated with this property
        //FireASM("ID", _id);
      }
    }

    private string _text = "";
    public string Text
    {
      get
      {
        return _text;
      }
      set
      {
        _text = value;
        FireASM("Text", _text);
      }
    }

    private string _moretext = "";
    public string MoreText
    {
      get
      {
        return _moretext;
      }
      set
      {
        _moretext = value;
        FireASM("MoreText", _moretext);
      }
    }

    private string _moretext2 = "";
    public string MoreText2
    {
      get
      {
        return _moretext2;
      }
      set
      {
        _moretext2 = value;
        FireASM("MoreText2", _moretext2);
      }
    }

    private bool _extrainfo;
    public bool ExtraInfo
    {
      get
      {
        return _extrainfo;
      }
      set
      {
        _extrainfo = value;
        FireASM("ExtraInfo", _extrainfo);
      }
    }

    private bool _extrainfoMultiline;
    public bool ExtraInfoMultiline
    {
      get
      {
        return _extrainfoMultiline;
      }
      set
      {
        _extrainfoMultiline = value;
        FireASM("ExtraInfoMultiline", _extrainfoMultiline);
      }
    }

    private Image _picture;
    public Image Picture
    {
      get
      {
        return _picture;
      }
      set
      {
        _picture = value;
        FireASM("Picture", _picture);
      }
    }

    private string _url = "";
    public string URL
    {
      get
      {
        return _url;
      }
      set
      {
        _url = value;
        FireASM("URL", _url);
      }
    }

    private bool _correctanswer;
    public bool CorrectAnswer
    {
      get
      {
        return _correctanswer;
      }
      set
      {
        _correctanswer = value;
        FireASM("CorrectAnswer", _correctanswer);
      }
    }

    // Note: When this property is actually used, we may want to switch its type back to 'int'.
    //       I've only made it a string so that it doesn't [uselessly] get written out to each file.
    private string _gotoquestionid = "";
    public string GotoQuestionID
    {
      get
      {
        return _gotoquestionid;
      }
      set
      {
        _gotoquestionid = value;
        FireASM("GotoQuestionID", _gotoquestionid);
      }
    }

    private string _multimedia = "";
    public string Multimedia
    {
      get
      {
        return _multimedia;
      }
      set
      {
        _multimedia = value;
        FireASM("Multimedia", _multimedia);
      }
    }

    private bool _showmultimedianow;
    public bool ShowMultimediaNow
    {
      get
      {
        return _showmultimedianow;
      }
      set
      {
        _showmultimedianow = value;
        FireASM("ShowMultimediaNow", _showmultimedianow);
      }
    }
  }
  #endregion


  #endregion


  #region Respondents

  #region _Respondents
  // This is the definition for the collection class '_Respondents', which will be instantiated into the collection 'Respondents'.
  // It is inherited from 'PPbaseCollection' for two reasons:
  //  1. Provides safety from null elements.
  //  2. Simplifies casting within the ASM code.

  public class _Respondents : PPbaseCollection
  {
    public event Poll.ASMCollectionEventHandler CollectionEvent;

    // This method is called from the following methods of this collection class
    //   - Add
    //   - Insert
    //   - Remove
    // It invokes the Automatic Synchronization Mechanism (ASM).  The Controller 
    // subscribes to these events and then responds to them accordingly.
    private void FireASMc(string collName, string Operation, int index)
    {
      try
      {
        if (CollectionEvent != null)
        {
          CollectionEvent(collName, Operation, index, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASMc Exception in Poll model", "Respondents");
      }
    }


    // Indexer
    public new _Respondent this[int index]
    {
      get
      {
        //throws ArgumentOutOfRangeException
        return (_Respondent) base[index];
      }
      set
      {
        //throws ArgumentOutOfRangeException
        base.Insert(index, value);
      }
    }

    public int Add(_Respondent value)
    {
      int index = base.Add(value);

      // ASM code needs to go here!
      FireASMc("Respondents", "Add", index);

      return index;
    }

    public void Insert(int index, _Respondent value)
    {
      base.Insert(index, value);
      // ASM code needs to go here!
    }

    public void Remove(_Respondent value)
    {
      base.Remove(value);
      // ASM code needs to go here!
    }

    public bool Contains(_Respondent value)
    {
      return base.Contains(value);
    }

    // Returns the index position that the 'value' occupies in the collection.
    public int IndexOf(_Respondent value)
    {
      return base.IndexOf(value);
    }

    // Searches the Respondents in the collection for a Respondent with the specified name.
    // 'name' is defined as:  First Name + Last Name, with a space in between, and is being
    // compared in a case insensitive manner.
    // Returns 'true' if found, 'false' otherwise.
    public bool ContainsName(string text)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Respondent Respondent = (_Respondent) List[i];
        if ((Respondent.FirstName.ToLower() + " " + Respondent.LastName.ToLower()) == text.ToLower())
          return true;
      }

      return false;  // If reaches here then a Respondent with the specified text was not found
    }

    // Searches the Respondents in the collection for a Respondent with the specified name.
    // Returns the Respondent if it finds it, otherwise 'null'.
    public _Respondent FindName(string text)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Respondent Respondent = (_Respondent) List[i];
        if (Respondent.FirstName + " " + Respondent.LastName == text)
          return Respondent;
      }

      return null;  // If reaches here then a Respondent with the specified name was not found
    }

    // Searches the Respondents in the collection for a Respondent with the specified GUID.
    // Returns 'true' if found, 'false' otherwise.
    public bool ContainsGuid(string guid)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Respondent Respondent = (_Respondent) List[i];
        if (Respondent.Guid == guid)
          return true;
      }

      return false;  // If reaches here then a Respondent with the specified GUID was not found
    }

    // Searches the Respondents in the collection for a Respondent with the specified GUID.
    // Returns the Respondent if it finds it, otherwise 'null'.
    public _Respondent FindGuid(string guid)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Respondent Respondent = (_Respondent) List[i];
        if (Respondent.Guid == guid)
          return Respondent;
      }

      return null;  // If reaches here then a Respondent with the specified GUID was not found
    }


    // Returns the subset of Respondents that have dates contained in the date array parameter.
    public _Respondents FilterByDate(ArrayList dateList)
    {
      _Respondents respondents;

      // If the Date array is empty then "ALL" was chosen so return the entire list of respondents
      if (dateList.Count == 0)
        return (_Respondents) List;
      else
      {
        respondents = new _Respondents();

        for (int i = 0; i < List.Count; i++)
        {
          _Respondent respondent = (_Respondent) List[i];

          if (dateList.Contains(respondent.TimeCaptured.Date))
            respondents.Add(respondent);
        }
      }

      return respondents;
    }


    // Returns the subset of Respondents, based on the passed sequence value.
    // Encoding:  0 = ALL , 10 = First 10 , -10 = Last 10
    public _Respondents FilterBySequence(int seqVal)
    {
      _Respondents respondents = new _Respondents();

      // If "ALL" is specified then return the entire list of respondents
      if ((seqVal == 0) || (List.Count < Math.Abs(seqVal)))
        respondents = (_Respondents) List;
      else
      {
        int diff;
        if (seqVal > 0)
          diff = 0;
        else
          diff = List.Count + seqVal;     // ex. 100 entries, last 10:  diff = 100 + -10 = 90

        for (int i = 0; i < Math.Abs(seqVal); i++)
        {
          _Respondent respondent = (_Respondent) List[i + diff];
          respondents.Add(respondent);
        }
      }

      return respondents;
    }




    // Add other type-safe methods here
    // ...
    // ...
  }

  #endregion


  #region _Respondent
  // This is the definition for the class '_Respondent', which will populate the collection 'Respondents'.

  // Note: In Tools there is a small class (RespondentInfo) and a method (GetRespondentInfoArray)
  //       that may have to be modified if anything in this class is changed.


  public class _Respondent
  {
    private _Respondents Respondents;


    /// <summary>
    /// This is the constructor for the '_Respondent' class.  We must pass it the parent collection
    /// it resides in so that it knows which Index number it has within this collection.
    /// </summary>
    /// <param name="Respondents"></param>
    public _Respondent(_Respondents Respondents)
    {
      this.Respondents = Respondents;
      instanceResponses = new _Responses(this);   // Instantiate a new, albeit empty, Responses collection, passing this instance to "_Responses"
    }

    // This read-only property maintains the Index number of the instantiated object in the parent collection.
    public int Index
    {
      get
      {
        return this.Respondents.IndexOf(this);
      }
    } 

    public event Poll.ASMEventHandler ModelEvent;


    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to these
    // events and correctly sets the corresponding linked form control.
    public void FireASM (string propertyName, object propertyValue, EventArgs e)   // Think about later changing "public" to "protected" or "internal"
    {
      try
      {
        // Because this method resides in a class that itself resides in a collection class,
        // we need to tack on information that specifies which element in the collection the
        // event is being fired from.
        propertyName = "Respondents[" + Index.ToString() + "]." + propertyName;

        if (ModelEvent != null)
        {
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch (Exception ex)
      {
        // Handle exceptions
        Debug.Fail("Exception in Poll mode\nError message: " + ex.Message, "FireASM.Respondent");
      }
    }

    private void FireASM(string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }


    // Properties:
    
    private long maxID = -1;  // The largest ID value used so far by any Respondent
    
    private long _id;
    public long ID
    {
      get
      {
        return _id;
      }
      set
      {
        _id = value;
        if (_id == -1)
        {
          maxID++;
          _id = maxID;
        }
        else if (_id > maxID)
          maxID = _id;

        // We'll turn off for now, since there's no control associated with this property
        //FireASM("ID", _id);
      }
    }

    private string _pollsterName;
    public string PollsterName
    {
      get
      {
        return _pollsterName;
      }
      set
      {
        _pollsterName = value;
        FireASM("PollsterName", _pollsterName);
      }
    }

    private string _guid;
    public string Guid
    {
      get
      {
        return _guid;
      }
      set
      {
        _guid = value;
        FireASM("Guid", _guid);
      }
    }

    private DateTime _timeCaptured;
    public DateTime TimeCaptured
    {
      get
      {
        return _timeCaptured;
      }
      set
      {
        _timeCaptured = value;
        FireASM("TimeCaptured", _timeCaptured);
      }
    }

    private string _gps = "";
    public string GPS
    {
      get
      {
        return _gps;
      }
      set
      {
        _gps = value;
        FireASM("GPS", _gps);
      }
    }

    private string _firstName = "";
    public string FirstName
    {
      get
      {
        return _firstName;
      }
      set
      {
        _firstName = value;
        FireASM("FirstName", _firstName);
      }
    }

    private string _lastName = "";
    public string LastName
    {
      get
      {
        return _lastName;
      }
      set
      {
        _lastName = value;
        FireASM("LastName", _lastName);
      }
    }

    private string _address = "";
    public string Address
    {
      get
      {
        return _address;
      }
      set
      {
        _address = value;
        FireASM("Address", _address);
      }
    }

    private string _city = "";
    public string City
    {
      get
      {
        return _city;
      }
      set
      {
        _city = value;
        FireASM("City", _city);
      }
    }

    private string _stateProv = "";
    public string StateProv
    {
      get
      {
        return _stateProv;
      }
      set
      {
        _stateProv = value;
        FireASM("StateProv", _stateProv);
      }
    }

    private string _postalCode = "";
    public string PostalCode
    {
      get
      {
        return _postalCode;
      }
      set
      {
        _postalCode = value;
        FireASM("PostalCode", _postalCode);
      }
    }

    private string _areaCode = "";
    public string AreaCode
    {
      get
      {
        return _areaCode;
      }
      set
      {
        _areaCode = value;
        FireASM("AreaCode", _areaCode);
      }
    }

    private string _telNum = "";
    public string TelNum
    {
      get
      {
        return _telNum;
      }
      set
      {
        _telNum = value;
        FireASM("TelNum", _telNum);
      }
    }

    private string _telNum2 = "";
    public string TelNum2
    {
      get
      {
        return _telNum2;
      }
      set
      {
        _telNum2 = value;
        FireASM("TelNum2", _telNum2);
      }
    }

    private string _email = "";
    public string Email
    {
      get
      {
        return _email;
      }
      set
      {
        _email = value;
        FireASM("Email", _email);
      }
    }

    private Constants.Sex _sex;
    public Constants.Sex Sex
    {
      get
      {
        return _sex;
      }
      set
      {
        _sex = value;
        FireASM("Sex", _sex);
      }
    }

    private string _age = "";
    public string Age
    {
      get
      {
        return _age;
      }
      set
      {
        _age = value;
        FireASM("Age", _age);
      }
    }

    private Image _photo;
    public Image Photo
    {
      get
      {
        return _photo;
      }
      set
      {
        _photo = value;
        FireASM("Photo", _photo);
      }
    }

    // Debug: 'FieldsToUse' and 'MoreQuestions' collections to be added here in the future


    private _Responses instanceResponses;
    public _Responses Responses 
    {
      get 
      { 
        return instanceResponses;
      }
    }
  }

  #endregion  // End of class _Respondent


  #region Responses

  #region _Responses
  // This is the definition for the collection class '_Responses', which will be instantiated into the collection 'Responses'.

  public class _Responses : PPbaseCollection
  {
    private _Respondent respondent;   // Internal reference to parent class

    /// <summary>
    /// This is the constructor for the '_Responses' class.  We must pass it the parent class it resides in.
    /// </summary>
    /// <param name="responses"></param>
    public _Responses(_Respondent respondent)
    {
      this.respondent = respondent;
    }

    public new _Response this[int index]
    {
      get
      { 
        return (_Response) base[index];
      }
      set 
      { 
        base.Insert(index, value);
      }
    }

    public int Add(_Response response)
    {
      response.ModelEvent += new Poll.ASMEventHandler(respondent.FireASM);
      return base.Add(response);
    }

    public void Insert(int index, _Response response)
    {
      response.ModelEvent += new Poll.ASMEventHandler(respondent.FireASM);
      base.Insert(index, response);
    }

    public void Remove(_Response response)
    {
      response.ModelEvent -= new Poll.ASMEventHandler(respondent.FireASM);
      base.Remove(response);
    }

    public bool Contains(_Response response)
    {
      return base.Contains(response);
    }

    // Returns the index position that the 'response' occupies in the collection.
    public int IndexOf(_Response response)
    {
      return base.IndexOf(response);
    }

    // *** Need to test these remaining methods! ***

//    // Searches the responses in the collection for a response with the specified text.
//    // Returns 'true' if found, 'false' otherwise.
//    public bool ContainsText(string text)
//    {
//      for (int i = 0; i < List.Count; i++)
//      {
//        _Response response = (_Response) List[i];
//        if (response.Text == text)
//          return true;
//      }
//
//      return false;  // If reaches here then a response with the specified text was not found
//    }

    // Searches the responses in the collection for a response with the specified QuestionID.
    // Returns the response if it finds it, otherwise 'null'.
    public _Response Find_ResponseByQuestionID(int questionID)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Response response = (_Response) List[i];
        if (response.QuestionID == questionID)
          return response;
      }

      return null;  // If reaches here then a response with the specified text was not found
    }

    // Add other type-safe methods here
    // ...
    // ...


  }

  #endregion


  #region _Response
  // This is the definition for the class '_Response', which will populate the collection 'Responses'.
  public class _Response
  {
    private _Responses responses;

    /// <summary>
    /// This is the constructor for the '_Choice' class.  We must pass it the parent collection
    /// it resides in so that it knows which Index number it has within this collection.
    /// </summary>
    /// <param name="Choices"></param>
    public _Response(_Responses responses)
    {
      this.responses = responses;
    }

    // This read-only property maintains the Index number of the instantiated object in the parent collection.
    public int Index
    {
      get
      {
        return this.responses.IndexOf(this);
      }
    } 
    
    public event Poll.ASMEventHandler ModelEvent;

    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to these
    // events and correctly sets the corresponding linked form control.
    private void FireASM(string propertyName, object propertyValue, EventArgs e)
    {
      try
      {
        if (ModelEvent != null)
        {
          // Because this method resides in a class that itself resides in a collection class,
          // we need to tack on information that specifies which element in the collection the
          // event is being fired from.
          propertyName = "Responses[" + Index.ToString() + "]." + propertyName;
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in Poll model", "Response");
      }
    }

    private void FireASM(string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }


    // Properties:

    private long maxID = -1;  // The largest ID value used so far by any Response
    private long _id;
    public long ID
    {
      get
      {
        return _id;
      }
      set
      {
        _id = value;
        if (_id == -1)
        {
          maxID++;
          _id = maxID;
        }
        else if (_id > maxID)
          maxID = _id;

        // We'll turn off for now, since there's no control associated with this property
        //FireASM("ID", _id);
      }
    }

    private string _guid;
    public string Guid
    {
      get
      {
        return _guid;
      }
      set
      {
        _guid = value;
        FireASM("Guid", _guid);
      }
    }

    private DateTime _timeCaptured;
    public DateTime TimeCaptured
    {
      get
      {
        return _timeCaptured;
      }
      set
      {
        _timeCaptured = value;
        FireASM("TimeCaptured", _timeCaptured);
      }
    }

    private int _questionID;
    public int QuestionID
    {
      get
      {
        return _questionID;
      }
      set
      {
        _questionID = value;
        FireASM("QuestionID", _questionID);
      }
    }

    private string _answerID = "";  // This must be initialized here or else some code in frmPoll (CF) will crash!
    public string AnswerID
    {
      get
      {
        return _answerID;
      }
      set
      {
        _answerID = value;
        FireASM("AnswerID", _answerID);
      }
    }

    private DateTime _lastModified;
    public DateTime LastModified
    {
      get
      {
        return _lastModified;
      }
      set
      {
        _lastModified = value;
        FireASM("LastModified", _lastModified);
      }
    }

    private string _extraText = "";
    public string ExtraText
    {
      get
      {
        return _extraText;
      }
      set
      {
        _extraText = value;
        FireASM("ExtraText", _extraText);
      }
    }

    private string _authentication = "";
    public string Authentication
    {
      get
      {
        return _authentication;
      }
      set
      {
        _authentication = value;
        FireASM("Authentication", _authentication);
      }
    }


    // Debug: 'MultpleChanges' collection to be added here in the future


  }
  #endregion  // End of class _Response

  #endregion

  #endregion


  #region PollingInfo

  // This is the definition for the nested class '_PollingInfo'
  public class _PollingInfo
  {
    public event Poll.ASMEventHandler ModelEvent;

    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to
    // these events and correctly sets the corresponding linked form control.
    private void FireASM(string propertyName, object propertyValue, EventArgs e)
    {
      try
      {
        if (ModelEvent != null)
        {
          // Because this method resides in a nested class we need to tack on information
          // that specifies which nested class the event is being fired from.
          propertyName = "PollingInfo." + propertyName;
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in Poll model", "PollingInfo");
      }
    }

    private void FireASM(string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }

    // Sets all the properties of this class back to their default state
    public void Clear()
    {
      PollsterName = "";
      ProductID = Constants.ProductID.Lite;
      VersionNumber = null;
      StartPolling = DateTime.MinValue;    // System.DateTime.Parse("1/1/0001");
      FinishPolling = DateTime.MinValue;  // System.DateTime.Parse("1/1/0001");
      Guid = null;
    }


    // Properties:

    private string _pollsterName;
    public string PollsterName
    {
      get
      {
        return _pollsterName;
      }
      set
      {
        _pollsterName = value;
        FireASM("PollsterName", _pollsterName);
      }
    }

    private Constants.ProductID _productID;
    public Constants.ProductID ProductID
    {
      get
      {
        return _productID;
      }
      set
      {
        _productID = value;
        FireASM("ProductID", _productID);
      }
    }

    private string _versionNumber;
    public string VersionNumber
    {
      get
      {
        return _versionNumber;
      }
      set
      {
        _versionNumber = value;
        FireASM("VersionNumber", _versionNumber);
      }
    }

    private DateTime _startPolling;
    public DateTime StartPolling
    {
      get
      {
        return _startPolling;
      }
      set
      {
        _startPolling = value;
        FireASM("StartPolling", _startPolling);
      }
    }

    private DateTime _finishPolling;
    public DateTime FinishPolling
    {
      get
      {
        return _finishPolling;
      }
      set
      {
        _finishPolling = value;
        FireASM("FinishPolling", _finishPolling);
      }
    }

    private string _guid;
    public string Guid
    {
      get
      {
        return _guid;
      }
      set
      {
        _guid = value;
        FireASM("Guid", _guid);
      }
    }
  }

  #endregion


  #region PollsterPrivileges

  // This is the definition for the nested class '_PollsterPrivileges'
  public class _PollsterPrivileges
  {
    public event Poll.ASMEventHandler ModelEvent;

    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to
    // these events and correctly sets the corresponding linked form control.
    private void FireASM(string propertyName, object propertyValue, EventArgs e)
    {
      try
      {
        if (ModelEvent != null)
        {
          // Because this method resides in a nested class we need to tack on information
          // that specifies which nested class the event is being fired from.
          propertyName = "PollsterPrivileges." + propertyName;
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in Poll model", "PollsterPrivileges");
      }
    }

    private void FireASM(string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }


    // Properties:

    private bool _modifypoll;
    public bool ModifyPoll
    {
      get
      {
        return _modifypoll;
      }
      set
      {
        _modifypoll = value;
        FireASM("ModifyPoll", _modifypoll);
      }
    }

    private bool _changeUserName;
    public bool ChangeUserName
    {
      get
      {
        return _changeUserName;
      }
      set
      {
        _changeUserName = value;
        FireASM("ChangeUserName", _changeUserName);
      }
    }

    private bool _reviewData;
    public bool ReviewData
    {
      get
      {
        return _reviewData;
      }
      set
      {
        _reviewData = value;
        FireASM("ReviewData", _reviewData);
      }
    }

    private bool _canAbortRecord;
    public bool CanAbortRecord
    {
      get
      {
        return _canAbortRecord;
      }
      set
      {
        _canAbortRecord = value;
        FireASM("CanAbortRecord", _canAbortRecord);
      }
    }

    private bool _canprint;
    public bool CanPrint
    {
      get
      {
        return _canprint;
      }
      set
      {
        _canprint = value;
        FireASM("CanPrint", _canprint);
      }
    }

    private bool _authenticateRespondentInfo;
    public bool AuthenticateRespondentInfo
    {
      get
      {
        return _authenticateRespondentInfo;
      }
      set
      {
        _authenticateRespondentInfo = value;
        FireASM("AuthenticateRespondentInfo", _authenticateRespondentInfo);
      }
    }

    private bool _authenticateResponses;
    public bool AuthenticateResponses
    {
      get
      {
        return _authenticateResponses;
      }
      set
      {
        _authenticateResponses = value;
        FireASM("AuthenticateResponses", _authenticateResponses);
      }
    }
  }

  #endregion

}
