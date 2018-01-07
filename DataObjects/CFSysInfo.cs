using System;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;


namespace DataObjects
{
  /// <summary>
  /// The object instantiated from this class is only used on a mobile device.
  /// 
  /// The 'CFSysInfo' data structure consists of these major sections:
  /// 
  /// Name                  Type
  /// ---------------------------------
  /// Admin                 Class
  /// Device                Class
  /// Options               Class
  /// Paths                 Class
  ///
  /// </summary>



  public class CFSysInfo
  {
    // This instantiation makes the CFSysInfo data global [to every module that references DataObjects].
    public static CFSysInfo Data = new CFSysInfo();
  

    // Constructor for CFSysInfo
    public CFSysInfo()
    {
    }

    // This is the event handler that will be used by all of the properties in all of the nested classes.
    //public delegate void ASMEventHandler (string propName, object propValue, EventArgs e);


    // In this class we will have two types of objects at the next level below the root:
    //   - Classes
    //   - Collections

    #region NestedClassesAndCollections

    // Here we instantiate the nested classes.  We need to do it in this slightly more sophisticated way
    // (than we did originally) so that a true parent-child relationship is established with the
    // instantiated objects.

    private _MobileAdmin instanceAdmin = new _MobileAdmin();
    public _MobileAdmin MobileAdmin 
    {
      get 
      { 
        return instanceAdmin;
      }
    }

    private _MobileOptions instanceOptions = new _MobileOptions();
    public _MobileOptions MobileOptions 
    {
      get 
      { 
        return instanceOptions;
      }
    }

    private _DeviceSpecs instanceDeviceSpecs = new _DeviceSpecs();
    public _DeviceSpecs DeviceSpecs 
    {
      get 
      { 
        return instanceDeviceSpecs;
      }
    }

    private _MobilePaths instancePaths = new _MobilePaths();
    public _MobilePaths MobilePaths 
    {
      get 
      { 
        return instancePaths;
      }
    }

    private _Summaries instanceSummaries = new _Summaries();
    public _Summaries Summaries 
    {
      get 
      { 
        return instanceSummaries;
      }
    }

    #endregion


    /// <summary>
    /// Initializes a Brand New CFSysInfo object with as much default information as possible.
    /// Note: This method must exist separately from the class constructor(s) because it can ONLY be called after:
    ///          - An object is instantiated from this class
    /// </summary>
    public void Initialize()
    {
    }

  }  // end of class CFSysInfo



  #region Admin
  // This is the definition for the nested class '_MobileAdmin'
  public class _MobileAdmin
  {
    // Properties:

    private string _appName;
    [XmlIgnore]
    public string AppName
    {
      get
      {
        return _appName;
      }
      set
      {
        _appName = value;
      }
    }

    private string _appFilename;
    [XmlIgnore]
    public string AppFilename
    {
      get
      {
        return _appFilename;
      }
      set
      {
        _appFilename = value;
      }
    }

    private string _appExtension;
    [XmlIgnore]
    public string AppExtension
    {
      get
      {
        return _appExtension;
      }
      set
      {
        _appExtension = value;
      }
    }

    private Constants.ProductID _productID;
    [XmlIgnore]
    public Constants.ProductID ProductID
    {
      get
      {
        return _productID;
      }
      set
      {
        _productID = value;
      }
    }

    private string _versionNumber;
    [XmlIgnore]
    public string VersionNumber
    {
      get
      {
        return _versionNumber;
      }
      set
      {
        _versionNumber = value;
      }
    }

    private string _companyName;
    [XmlIgnore]
    public string CompanyName
    {
      get
      {
        return _companyName;
      }
      set
      {
        _companyName = value;
      }
    }

    private string _regKeyName;
    [XmlIgnore]
    public string RegKeyName
    {
      get
      {
        return _regKeyName;
      }
      set
      {
        _regKeyName = value;
      }
    }

    private DateTime _expiryDate;
    [XmlIgnore]
    public DateTime ExpiryDate
    {
      get
      {
        return _expiryDate;
      }
      set
      {
        _expiryDate = value;
      }
    }

    private string _osVersion;
    [XmlIgnore]
    public string OSVersion
    {
      get
      {
        return _osVersion;
      }
      set
      {
        _osVersion = value;
      }
    }

  }
  #endregion


  #region Device

  // This is the definition for the nested class '_DeviceSpecs'
  public class _DeviceSpecs
  {
    // Constructor for "_DeviceSpecs" class
    public _DeviceSpecs()
    {
    }


    // Properties:

    private int _screenWidth;
    [XmlIgnore]
    public int ScreenWidth
    {
      get
      {
        return _screenWidth;
      }
      set
      {
        _screenWidth = value;
      }
    }

    private int _screenHeight;
    [XmlIgnore]
    public int ScreenHeight
    {
      get
      {
        return _screenHeight;
      }
      set
      {
        _screenHeight = value;
      }
    }

    private int _availHeight;
    [XmlIgnore]
    public int AvailHeight
    {
      get
      {
        return _availHeight;
      }
      set
      {
        _availHeight = value;
      }
    }

    private Constants.MobilePlatform _platform;
    [XmlIgnore]
    public Constants.MobilePlatform Platform
    {
      get
      {
        return _platform;
      }
      set
      {
        _platform = value;
      }
    }
  }

  #endregion


  #region Options

  // This is the definition for the nested class '_MobileOptions'
  public class _MobileOptions
  {
    // Constructor for "_MobileOptions" class
    public _MobileOptions()
    {
    }


    // Properties:
    private ushort _defaultLanguageID;
    public ushort DefaultLanguageID
    {
      get
      {
        return _defaultLanguageID;
      }
      set
      {
        _defaultLanguageID = value;
      }
    }

    private bool _startWithMenu = true; //true = default
    public bool StartWithMenu
    {
      get
      {
        return _startWithMenu;
      }
      set
      {
        _startWithMenu = value;
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
      }
    }

    private string _primaryUser = "";
    public string PrimaryUser
    {
      get
      {
        return _primaryUser;
      }
      set
      {
        _primaryUser = value;
      }
    }

//    // Can only be set True by the Desktop app, though can be set False by the mobile app.
//    // If True then it indicates that the user info above was examined, possibly altered,
//    // and essentially verified by the Desktop.  These checks eliminate incorrect duplication
//    // of info - ex. "Robert Werner" vs. "Rachel Werner" or two "John Smith" users that are
//    // actually different people.
//    private bool _userInfoConfirmed;
//    public bool UserInfoConfirmed
//    {
//      get
//      {
//        return _userInfoConfirmed;
//      }
//      set
//      {
//        _userInfoConfirmed = value;
//      }
//    }

    // If true then the FirstName, LastName, and PrimaryUser fields on the device can be changed.
    // The default is false.
    private bool _allowNameEditing;
    public bool AllowNameEditing
    {
      get
      {
        return _allowNameEditing;
      }
      set
      {
        _allowNameEditing = value;
      }
    }

    private string _defaultCountry = "";
    public string DefaultCountry
    {
      get
      {
        return _defaultCountry;
      }
      set
      {
        _defaultCountry = value;
      }
    }

    private string _defaultStateProv = "";
    public string DefaultStateProv
    {
      get
      {
        return _defaultStateProv;
      }
      set
      {
        _defaultStateProv = value;
      }
    }

    private string _defaultCommunity = "";
    public string DefaultCommunity
    {
      get
      {
        return _defaultCommunity;
      }
      set
      {
        _defaultCommunity = value;
      }
    }
  }

  #endregion


  #region Paths

  // This is the definition for the nested class '_MobilePaths'
  public class _MobilePaths
  {
    // Constructor for the "_MobilePaths" class
    public _MobilePaths()
    {
    }


    // Properties:

    private string _app = "";
    [XmlIgnore]
    public string App
    {
      get
      {
        return _app;
      }
      set
      {
        _app = value;
      }
    }

    private string _data = "";
    public string Data
    {
      get
      {
        return _data;
      }
      set
      {
        _data = value;
      }
    }

    private string _completed = "";
    [XmlIgnore]
    public string Completed
    {
      get
      {
        return _completed;
      }
      set
      {
        _completed = value;
      }
    }

    private string _downloaded = "";
    [XmlIgnore]
    public string Downloaded
    {
      get
      {
        return _downloaded;
      }
      set
      {
        _downloaded = value;
      }
    }

    private string _templates = "";
    [XmlIgnore]
    public string Templates
    {
      get
      {
        return _templates;
      }
      set
      {
        _templates = value;
      }
    }

    private string _help = "";
    [XmlIgnore]
    public string Help
    {
      get
      {
        return _help;
      }
      set
      {
        _help = value;
      }
    }
  }

  #endregion


  #region Summaries

  public class _Summaries
  {
    private _ActivePolls instanceActivePolls = new _ActivePolls();
    public _ActivePolls ActivePolls
    {
      get
      {
        return instanceActivePolls;
      }
    }

    private _Templates instanceTemplates = new _Templates();
    public _Templates Templates
    {
      get
      {
        return instanceTemplates;
      }
    }


    #region ActivePolls
  
    // This is the definition for the collection class '_ActivePolls', which will be instantiated into the collection 'ActivePolls'.

    public class _ActivePolls : PPbaseCollection
    {
      // Indexer
      public new _ActivePollSummary this[int index]
      {
        get
        {
          return (_ActivePollSummary) base[index];
        }
        set
        {
          base.Insert(index, value);
        }
      }

      public int Add(_ActivePollSummary value)
      {
        int index = base.Add(value);
        return index;
      }

      public void Insert(int index, _ActivePollSummary value)
      {
        base.Insert(index, value);
      }

      public void Remove(_ActivePollSummary value)
      {
        base.Remove(value);
      }

      public bool Contains(_ActivePollSummary value)
      {
        return base.Contains(value);
      }

      // Returns the index position that the 'value' occupies in the collection.
      public int IndexOf(_ActivePollSummary value)
      {
        return base.IndexOf(value);
      }

      // Searches the Active Poll Summaries in the collection for one with the specified filename.
      // Returns 'true' if found, 'false' otherwise.
      public bool ContainsFilename(string filename)
      {
        for (int i = 0; i < List.Count; i++)
        {
          _ActivePollSummary summary = (_ActivePollSummary) List[i];
          if (summary.Filename.ToLower() == filename.ToLower())
            return true;
        }

        return false;  // If reaches here then a entry with the specified filename was not found
      }

      // Searches the Active Poll Summaries in the collection for an object with the specified filename.
      // Returns the Summary object if it finds it, otherwise 'null'.
      public _ActivePollSummary Find_PollSummary(string filename)
      {
        for (int i = 0; i < List.Count; i++)
        {
          _ActivePollSummary summary = (_ActivePollSummary) List[i];
          if (summary.Filename.ToLower() == filename.ToLower())
            return summary;
        }

        return null;  // If reaches here then a Summary object with the specified filename was not found
      }

      // Add other type-safe methods here
      // ...
      // ...
    }

    #endregion

    #region ActivePollSummary
    // This is the definition for the class '_ActivePollSummary', which will populate the collection 'ActivePoll'.
    public class _ActivePollSummary
    {
      public _ActivePollSummary()
      {
      }

      // Sometimes most/all of the property values are already known.  This constructor allows them to be quickly set.
      public _ActivePollSummary(_ActivePollSummary summary)
      {
        Filename = summary.Filename;
        Revision = summary.Revision;
        PollSummary = summary.PollSummary;
        ReviewData = summary.ReviewData;
        NumQuestions = summary.NumQuestions;
        NumResponses = summary.NumResponses;
      }

      // Here an alternative constructor for when all the values are already known.
      public _ActivePollSummary(string filename, int revision, string pollSummary, bool reviewData, int numQuestions, int numResponses)
      {
        Filename = filename;
        Revision = revision;
        PollSummary = pollSummary;
        ReviewData = reviewData;
        NumQuestions = numQuestions;
        NumResponses = numResponses;
      }


      // Properties:

      private string _filename;
      public string Filename
      {
        get
        {
          return _filename;
        }
        set
        {
          _filename = value;
        }
      }

      private int _revision;
      public int Revision
      {
        get
        {
          return _revision;
        }
        set
        {
          _revision = value;
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
        }
      }

      private int _numQuestions;
      public int NumQuestions
      {
        get
        {
          return _numQuestions;
        }
        set
        {
          _numQuestions = value;
        }
      }

      private int _numResponses;
      public int NumResponses
      {
        get
        {
          return _numResponses;
        }
        set
        {
          _numResponses = value;
        }
      }
    }

    #endregion  // End of class _ActivePollSummary


    #region Templates
    // This is the definition for the collection class '_Templates', which will be instantiated into the collection 'Templates'.

    public class _Templates : PPbaseCollection
    {

      // Indexer
      public new _TemplateSummary this[int index]
      {
        get
        {
          //throws ArgumentOutOfRangeException
          return (_TemplateSummary) base[index];
        }
        set
        {
          //throws ArgumentOutOfRangeException
          base.Insert(index, value);
        }
      }

      public int Add(_TemplateSummary value)
      {
        int index = base.Add(value);
        return index;
      }

      public void Insert(int index, _TemplateSummary value)
      {
        base.Insert(index, value);
      }

      public void Remove(_TemplateSummary value)
      {
        base.Remove(value);
      }

      public bool Contains(_TemplateSummary value)
      {
        return base.Contains(value);
      }

      // Returns the index position that the 'value' occupies in the collection.
      public int IndexOf(_TemplateSummary value)
      {
        return base.IndexOf(value);
      }

      // Searches the Template Summaries in the collection for one with the specified filename.
      // Returns 'true' if found, 'false' otherwise.
      public bool ContainsFilename(string filename)
      {
        for (int i = 0; i < List.Count; i++)
        {
          _TemplateSummary summary = (_TemplateSummary) List[i];
          if (summary.Filename.ToLower() == filename.ToLower())
            return true;
        }

        return false;  // If reaches here then a entry with the specified filename was not found
      }

      // Searches the Template Summaries in the collection for an object with the specified filename.
      // Returns the Summary object if it finds it, otherwise 'null'.
      public _TemplateSummary Find_Template(string filename)
      {
        for (int i = 0; i < List.Count; i++)
        {
          _TemplateSummary summary = (_TemplateSummary) List[i];
          if (summary.Filename.ToLower() == filename.ToLower())
            return summary;
        }

        return null;  // If reaches here then a Summary object with the specified filename was not found
      }

      // Add other type-safe methods here
      // ...
      // ...
    }

    #endregion

    // Note: "_TemplateSummary" is now located in BaseClasses.cs because it is shared by both SysInfo & CFSysInfo



  }

  #endregion


}
