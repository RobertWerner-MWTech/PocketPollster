using System;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;


namespace DataObjects
{
  /// <summary>
  /// The 'SysInfo' data structure consists of these major sections:
  /// 
  /// Name                  Type
  /// ---------------------------------
  /// Users                 Collection
  /// Devices               Collection
  /// Admin                 Class
  /// Options               Class
  /// Paths                 Class
  /// 
  /// Important Note: One inherent limitation of the Auto Synchronization Mechanism (ASM) employed
  ///                 is that no two properties can share the same name, no matter that they exist
  ///                 in separate nested classes.
  /// </summary>



  [Serializable]
  public class SysInfo
  {
    // Note: I first tried implementing this as a Singleton class, but it presented problems for 'Tools.SaveData'.
    //       So I'm going to implement it as a regular class.  If other developers take over this code one day
    //       then they need to understand not to instantiate it more than once, because that would serve no purpose.

    // This instantiation makes the SysInfo data global [to every module that references DataObjects].
    public static SysInfo Data = new SysInfo();

  
  
    // In this constructor we'll initialize some defaults
    public SysInfo()
    {
      Options.MDBformat = Constants.MDBformat.Newer;
      Options.ShowSplash = true;

      Options.DataXfer.UnattendedSync = true;
      Options.DataXfer.Sound = true;

      Options.Mobile.AutoUpdate = false;
      Options.Mobile.OverrideInstallBlock = false;
      Options.Mobile.PurgeDuration = 0;
      Options.Mobile.BatteryWarningLevel = 20;
      Options.Mobile.GetPersonalInfo = true;
      Options.Mobile.HideQuestionNumbers = false;
    }


    // **********
    // Not sure if we can use ASM with this SysInfo data but would be interesting to see if it eases workload with Options data
    // **********

    // This is the event handler that will be used by all of the properties in all of the nested classes.
    public delegate void ASMEventHandler (string propName, object propValue, EventArgs e);

    // This is the event handler that will be used by all collections in this class.
    // It'll be called whenever a collection element is added or removed.
    public delegate void ASMCollectionEventHandler (string collName, string Operation, int index, EventArgs e);


    // In this class we will have two types of objects at the next level below the root:
    //   - Classes
    //   - Collections

    #region NestedClassesAndCollections

    // Here we instantiate the nested classes.  We need to do it in this slightly more sophisticated way
    // (than we did originally) so that a true parent-child relationship is established with the
    // instantiated objects.

    private _Users instanceUsers = new _Users();
    public _Users Users 
    {
      get 
      { 
        return instanceUsers;
      }
    }

    private _Devices instanceDevices = new _Devices();
    public _Devices Devices 
    {
      get 
      { 
        return instanceDevices;
      }
    }

    private _Admin instanceAdmin = new _Admin();
    public _Admin Admin 
    {
      get 
      { 
        return instanceAdmin;
      }
    }

    private _Options instanceOptions = new _Options();
    public _Options Options 
    {
      get 
      { 
        return instanceOptions;
      }
    }

    private _Paths instancePaths = new _Paths();
    public _Paths Paths 
    {
      get 
      { 
        return instancePaths;
      }
    }

    private _PollSummaries instanceSummaries = new _PollSummaries();
    public _PollSummaries Summaries 
    {
      get 
      { 
        return instanceSummaries;
      }
    }

    #endregion


    /// <summary>
    /// Initializes a Brand New SysInfo object with as much default information as possible.
    /// Note: This method must exist separately from the class constructor(s) because it can ONLY be called after:
    ///          - An object is instantiated from this class
    ///          - Event delegates are wired in (a restraint of ASM)
    /// </summary>
    public void Initialize()
    {
    }

  }  // end of class SysInfo



  #region Users
  // This is the definition for the collection class '_Users', which will be instantiated into the collection 'Users'.
  // It is inherited from 'PPbaseCollection' for two reasons:
  //  1. Provides safety from null elements.
  //  2. Simplifies casting within the ASM code.

  public class _Users : PPbaseCollection
  {
    public event SysInfo.ASMCollectionEventHandler CollectionEvent;

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
        Debug.Fail("FireASMc Exception in SysInfo model", "Users");
      }
    }


    // Indexer
    public new _User this[int index]
    {
      get
      {
        //throws ArgumentOutOfRangeException
        return (_User) base[index];
      }
      set
      {
        //throws ArgumentOutOfRangeException
        base.Insert(index, value);
      }
    }

    public int Add(_User value)
    {
      int index = base.Add(value);

      // ASM code needs to go here!
      FireASMc("Users", "Add", index);

      return index;
    }

    public void Insert(int index, _User value)
    {
      base.Insert(index, value);
      // ASM code needs to go here!
    }

    public void Remove(_User value)
    {
      base.Remove(value);
      // ASM code needs to go here!
    }

    public bool Contains(_User value)
    {
      return base.Contains(value);
    }

    // Returns the index position that the 'value' occupies in the collection.
    public int IndexOf(_User value)
    {
      return base.IndexOf(value);
    }

    // Searches the users in the collection for a user with the specified user name.
    // Returns 'true' if found, 'false' otherwise.
    public bool ContainsName(string text)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _User user = (_User) List[i];
        if (user.Name == text)
          return true;
      }

      return false;  // If reaches here then a user with the specified text was not found
    }

    // Searches the users in the collection for a user with the specified text.
    // Returns the user if it finds it, otherwise 'null'.
    public _User Find_User(string userName)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _User user = (_User) List[i];
        if (user.Name == userName)
          return user;
      }

      return null;  // If reaches here then a user with the specified text was not found
    }


    // Add other type-safe methods here
    // ...
    // ...
  }

  #endregion

  #region User
  // This is the definition for the class '_User', which will populate the collection 'Users'.
  public class _User
  {
    private _Users users;


    /// <summary>
    /// This is the constructor for the '_User' class.  We must pass it the parent collection
    /// it resides in so that it knows which Index number it has within this collection.
    /// </summary>
    /// <param name="users"></param>
    public _User (_Users users)
    {
      this.users = users;
    }

    // This read-only property maintains the Index number of the instantiated object in the parent collection.
    public int Index
    {
      get
      {
        return this.users.IndexOf(this);
      }
    } 

    public event SysInfo.ASMEventHandler ModelEvent;


    // Note: Not sure if we'll ever use ASM for SysInfo data.  And if we do, remember that it [currently does NOT
    //       work with the nested properties.  We may or may not be able to overcome this apparent limitation.


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
        propertyName = "Users[" + Index.ToString() + "]." + propertyName;

        if (ModelEvent != null)
        {
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in SysInfo model", "User");
      }
    }

    private void FireASM (string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }


    // Properties:

    private static long maxID = 0;  // The largest ID value used so far by any user
    
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

    private string _Name;
    public string Name
    {
      get
      {
        return _Name;
      }
      set
      {
        _Name = value;
        FireASM("Name", _Name);
      }
    }

    private string _password = "";
    public string Password
    {
      get
      {
        return _password;
      }
      set
      {
        _password = value;
        FireASM("Password", _password);
      }
    }

    private string _firstName;
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

    private string _lastName;
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

    private string _employeeID;
    public string EmployeeID
    {
      get
      {
        return _employeeID;
      }
      set
      {
        _employeeID = value;
        FireASM("EmployeeID", _employeeID);
      }
    }

    private string _email;
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

    private string _celPhone;
    public string CelPhone
    {
      get
      {
        return _celPhone;
      }
      set
      {
        _celPhone = value;
        FireASM("CelPhone", _celPhone);
      }
    }

    private string _skypeID;
    public string SkypeID
    {
      get
      {
        return _skypeID;
      }
      set
      {
        _skypeID = value;
        FireASM("SkypeID", _skypeID);
      }
    }

    private string _groupID;
    public string GroupID
    {
      get
      {
        return _groupID;
      }
      set
      {
        _groupID = value;
        FireASM("GroupID", _groupID);
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

    private DateTime _creationDate = DateTime.Now;
    public DateTime CreationDate
    {
      get
      {
        return _creationDate;
      }
      set
      {
        _creationDate = value;
        //FireASM("CreationDate", _creationDate.ToShortDateString());
        FireASM("CreationDate", _creationDate);
      }
    }


    private _Privileges instancePrivileges = new _Privileges();
    public _Privileges Privileges 
    {
      get 
      { 
        return instancePrivileges;
      }
    }

    public class _Privileges
    {
      // Properties

      private bool _desktop;
      public bool Desktop
      {
        get
        {
          return _desktop;
        }
        set
        {
          _desktop = value;
        }
      }

      private bool _ppc;
      public bool PPC
      {
        get
        {
          return _ppc;
        }
        set
        {
          _ppc = value;
        }
      }

      private bool _celPhone;
      public bool CelPhone
      {
        get
        {
          return _celPhone;
        }
        set
        {
          _celPhone = value;
        }
      }

      private bool _print;
      public bool Print
      {
        get
        {
          return _print;
        }
        set
        {
          _print = value;
        }
      }

      private bool _export;
      public bool Export
      {
        get
        {
          return _export;
        }
        set
        {
          _export = value;
        }
      }

      private bool _advancedUser;
      public bool AdvancedUser
      {
        get
        {
          return _advancedUser;
        }
        set
        {
          _advancedUser = value;
        }
      }

      private bool _administrator;
      public bool Administrator
      {
        get
        {
          return _administrator;
        }
        set
        {
          _administrator = value;
        }
      }
    }

  }
  #endregion  // End of class _User


  #region Devices
  // This is the definition for the collection class '_Devices', which will be instantiated into the collection 'Devices'.
  // It is inherited from 'PPbaseCollection' for two reasons:
  //  1. Provides safety from null elements.
  //  2. Simplifies casting within the ASM code.

  public class _Devices : PPbaseCollection
  {
    public event SysInfo.ASMCollectionEventHandler CollectionEvent;

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
        Debug.Fail("FireASMc Exception in SysInfo model", "Devices");
      }
    }


    // Indexer
    public new _Device this[int index]
    {
      get
      {
        //throws ArgumentOutOfRangeException
        return (_Device) base[index];
      }
      set
      {
        //throws ArgumentOutOfRangeException
        base.Insert(index, value);
      }
    }

    public int Add(_Device value)
    {
      int index = base.Add(value);

      // ASM code needs to go here!
      FireASMc("Devices", "Add", index);

      return index;
    }

    public void Insert(int index, _Device value)
    {
      base.Insert(index, value);
      // ASM code needs to go here!
    }

    public void Remove(_Device value)
    {
      base.Remove(value);
      // ASM code needs to go here!
    }

    public bool Contains(_Device value)
    {
      return base.Contains(value);
    }

    // Returns the index position that the 'value' occupies in the collection.
    public int IndexOf(_Device value)
    {
      return base.IndexOf(value);
    }

    // Searches the Devices in the collection for a Device with the specified ID.
    // Returns 'true' if found, 'false' otherwise.
    public bool ContainsID(long id)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Device Device = (_Device) List[i];
        if (Device.ID == id)
          return true;
      }

      return false;  // If reaches here then a Device with the specified ID was not found
    }

    // Searches the Devices in the collection for a Device with the specified ID.
    // Returns the Device if it finds it, otherwise 'null'.
    public _Device Find_ID(long id)
    {
      for (int i = 0; i < List.Count; i++)
      {
        _Device Device = (_Device) List[i];
        if (Device.ID == id)
          return Device;
      }

      return null;  // If reaches here then a Device with the specified ID was not found
    }

    // Searches the Devices in the collection for a Device with the specified GUID.
    // Returns the Device if it finds it, otherwise 'null'.
    public _Device Find_Guid(string guid)
    {
      if (guid == null)
        return null;

      for (int i = 0; i < List.Count; i++)
      {
        _Device Device = (_Device) List[i];
        if (Device.Guid == guid)
          return Device;
      }

      return null;  // If reaches here then a Device with the specified GUID was not found
    }


    // Add other type-safe methods here
    // ...
    // ...
  }

  #endregion

  #region Device
  // This is the definition for the class '_Device', which will populate the collection 'Devices'.
  public class _Device
  {
    private _Devices Devices;


    /// <summary>
    /// This is the constructor for the '_Device' class.  We must pass it the parent collection
    /// it resides in so that it knows which Index number it has within this collection.
    /// </summary>
    /// <param name="Devices"></param>
    public _Device(_Devices Devices)
    {
      this.Devices = Devices;
    }

    // This read-only property maintains the Index number of the instantiated object in the parent collection.
    public int Index
    {
      get
      {
        return this.Devices.IndexOf(this);
      }
    } 

    public event SysInfo.ASMEventHandler ModelEvent;


    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to these
    // events and correctly sets the corresponding linked form control.
    public void FireASM(string propertyName, object propertyValue, EventArgs e)   // Think about later changing "public" to "protected" or "internal"
    {
      try
      {
        // Because this method resides in a class that itself resides in a collection class,
        // we need to tack on information that specifies which element in the collection the
        // event is being fired from.
        propertyName = "Devices[" + Index.ToString() + "]." + propertyName;

        if (ModelEvent != null)
        {
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in SysInfo model", "Device");
      }
    }

    private void FireASM (string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
    }


    // This method returns a properly formatted Device ID, with enough leading 
    // zeros to ensure that all Device ID strings will be the same length.
    // Ex. If maxID = 100 and id = 5, then "005" will be returned.
    public string FormatDeviceID(long id)
    {
      string sId = id.ToString();
      string sMaxId = maxID.ToString();

      if (sId.Length < sMaxId.Length)
        sId = sId.PadLeft(sMaxId.Length, '0');

      return sId;
    }


    public string FormatDate(DateTime date)
    {
      // Debug: Could get fancier with things like:
      //          - Earlier time today
      //          - "Yesterday"

      string sDate = "";

      if (date == DateTime.MinValue)  // DateTime.Parse("1/1/0001"))
        sDate = "";
      else
        sDate = date.ToShortDateString();

      return sDate;
    }


    // Properties:

    private static long maxID = 0;  // The largest ID value used so far by any Device
    
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

    private string _osVersion = "";
    public string OSVersion
    {
      get
      {
        return _osVersion;
      }
      set
      {
        _osVersion = value;
        FireASM("OSVersion", _osVersion);
      }
    }

    private string _softwareVersion;
    public string SoftwareVersion
    {
      get
      {
        return _softwareVersion;
      }
      set
      {
        _softwareVersion = value;
        FireASM("SoftwareVersion", _softwareVersion);
      }
    }

    private DateTime _lastUpdate;
    public DateTime LastUpdate
    {
      get
      {
        return _lastUpdate;
      }
      set
      {
        _lastUpdate = value;
        //FireASM("LastUpdate", _lastUpdate.ToShortDateString());
        FireASM("LastUpdate", _lastUpdate);
      }
    }

    private DateTime _lastSync;
    public DateTime LastSync
    {
      get
      {
        return _lastSync;
      }
      set
      {
        _lastSync = value;
        //FireASM("LastSync", _lastSync.ToShortDateString());
        FireASM("LastSync", _lastSync);
      }
    }

    private string _primaryUser;
    public string PrimaryUser
    {
      get
      {
        return _primaryUser;
      }
      set
      {
        _primaryUser = value;
        FireASM("PrimaryUser", _primaryUser);
      }
    }

    private bool _multipleUsers;
    public bool MultipleUsers
    {
      get
      {
        return _multipleUsers;
      }
      set
      {
        _multipleUsers = value;
        FireASM("MultipleUsers", _multipleUsers);
      }
    }

    private bool _active;
    public bool Active
    {
      get
      {
        return _active;
      }
      set
      {
        _active = value;
        FireASM("Active", _active);
      }
    }
  }
  #endregion  // End of class _Device


  #region Admin
  // This is the definition for the nested class '_Admin'
  public class _Admin
  {
    // Methods:

    public int GetNextBlankPollNumber()
    {
      BlankPollNumber += 1;
      return BlankPollNumber;
    }

    public int GetCurrentBlankPollNumber()
    {
      return BlankPollNumber;
    }


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

    // Note: This is populated with Constants.ProductID
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

    // 0 - Viewer Mode      1 - Standard Mode
    private Constants.OpMode _operatingMode;
    [XmlIgnore]
    public Constants.OpMode OperatingMode
    {
      get
      {
        return _operatingMode;
      }
      set
      {
        _operatingMode = value;
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
      }
    }


    // Note: This more complex structure replaces an earlier simpler implementation of 'DataTransferActive'.
    //       It is being used to avoid the synchronization problems inherent with multiple threads accessing
    //       the same variable at the same time.  Think of it like a sophisticated Property.
    private static bool dataTransferActive;
    private object _syncRoot = new object();

    [XmlIgnore]
    public bool DataTransferActive
    {
      get
      {
        lock(_syncRoot)
        {
          return dataTransferActive;
        }
      }
      set
      {
        lock(_syncRoot)
        {
          dataTransferActive = value;
        }
      }
    }

    // A counter for setting the number of a new blank poll
    private int _blankPollNumber = 0;
    
    [XmlIgnore]
    public int BlankPollNumber
    {
      get
      {
        return _blankPollNumber;
      }
      set
      {
        _blankPollNumber = value;
      }
    }
  }
  #endregion


  #region Options

  // This is the definition for the nested class '_Options'
  public class _Options
  {
    //public delegate void ASMEventHandler (string propname, object propvalue, Type proptype, System.EventArgs e);
    public event SysInfo.ASMEventHandler ModelEvent;


    // Constructor for "_Options" class
    public _Options()
    {
      AreaCodes = new ArrayList();
      PostalCodes = new ArrayList();
    }


    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to these
    // events and correctly sets the corresponding linked form control.
    private void FireASM (string propertyName, object propertyValue, EventArgs e)
    {
      try
      {
        if (ModelEvent != null)
        {
          // Because this method resides in a nested class we need to tack on information
          // that specifies which nested class the event is being fired from.
          propertyName = "Options." + propertyName;
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in SysInfo model", "Options");
      }
    }

    private void FireASM (string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
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
        FireASM("DefaultLanguageID", _defaultLanguageID);
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

    private Constants.MDBformat _mdbFormat;
    public Constants.MDBformat MDBformat
    {
      get
      {
        return _mdbFormat;
      }
      set
      {
        _mdbFormat = value;
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
        FireASM("DefaultCountry", _defaultCountry);
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
        FireASM("DefaultStateProv", _defaultStateProv);
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
        FireASM("DefaultCommunity", _defaultCommunity);
      }
    }

    // ToDo: May have to expand complexity of this next line:
    public ArrayList AreaCodes;

    // ToDo: May have to expand complexity of this next line:
    public ArrayList PostalCodes;


    private bool _showSplash;
    public bool ShowSplash
    {
      get
      {
        return _showSplash;
      }
      set
      {
        _showSplash = value;
      }
    }


    private _DataXfer instanceDataXfer = new _DataXfer();
    public _DataXfer DataXfer 
    {
      get 
      { 
        return instanceDataXfer;
      }
    }

    public class _DataXfer
    {
      // Properties

      private bool _unattendedSync;
      public bool UnattendedSync
      {
        get
        {
          return _unattendedSync;
        }
        set
        {
          _unattendedSync = value;
        }
      }

      private bool _sound;
      public bool Sound
      {
        get
        {
          return _sound;
        }
        set
        {
          _sound = value;
        }
      }
    }

  
    private _Mobile instanceMobile = new _Mobile();
    public _Mobile Mobile 
    {
      get 
      { 
        return instanceMobile;
      }
    }

    public class _Mobile
    {
      // Properties

      private ushort _defaultDeviceTypeID;
      public ushort DefaultDeviceTypeID
      {
        get
        {
          return _defaultDeviceTypeID;
        }
        set
        {
          _defaultDeviceTypeID = value;
        }
      }

      // If set 'true' then frmInstallPrompt dialog box will be bypassed, allowing automatic installations and updates.
      private bool _autoUpdate;
      public bool AutoUpdate
      {
        get
        {
          return _autoUpdate;
        }
        set
        {
          _autoUpdate = value;
        }
      }

      // If set 'true' then will always ask PPCs w/o PP whether to install, even if they have StopAskingToInstall on the PPC set true.
      private bool _overrideInstallBlock;
      public bool OverrideInstallBlock
      {
        get
        {
          return _overrideInstallBlock;
        }
        set
        {
          _overrideInstallBlock = value;
        }
      }

      private bool _dataAutoTransfer;
      public bool DataAutoTransfer
      {
        get
        {
          return _dataAutoTransfer;
        }
        set
        {
          _dataAutoTransfer = value;
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
        }
      }

      private bool _allowUnattendedDownloads;
      public bool AllowUnattendedDownloads
      {
        get
        {
          return _allowUnattendedDownloads;
        }
        set
        {
          _allowUnattendedDownloads = value;
        }
      }

      private byte _batteryWarningLevel;
      public byte BatteryWarningLevel
      {
        get
        {
          return _batteryWarningLevel;
        }
        set
        {
          _batteryWarningLevel = value;
        }
      }

      // The location (not the path) the app was last installed to.
      private string _appInstallLocation = "";
      [XmlIgnore]
      public string AppInstallLocation
      {
        get
        {
          return _appInstallLocation;
        }
        set
        {
          _appInstallLocation = value;
        }
      }

      // The location (not the path) the data folders were last installed to.
      private string _dataInstallLocation = "";
      [XmlIgnore]
      public string DataInstallLocation
      {
        get
        {
          return _dataInstallLocation;
        }
        set
        {
          _dataInstallLocation = value;
        }
      }

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

      // If true then ask for Respondent Personal Info
      private bool _getPersonalInfo;
      public bool GetPersonalInfo
      {
        get
        {
          return _getPersonalInfo;
        }
        set
        {
          _getPersonalInfo = value;
        }
      }

      // If true then hide the Question Numbers
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
        }
      }
    }
  }


  // Minor classes that belong inside the "_Options" class

  // Structure of the elements that will populate the AreaCodes collection
  // To Do: This code to be expanded when we're focusing on this class.
  public class AreaCode
  {
    public ushort Number;
    //public bool Enabled;  // Any use to having this field?
  }

  // Structure of the elements that will populate the PostalCodes collection
  // To Do: This code to be expanded when we're focusing on this class.
  public class PostalCode
  {
    public string Code;
    public bool TreatAsZip;      // If true then refer to as a "Zip" code
    //public bool Enabled;  // Any use to having this field?
  }
  #endregion


  #region Paths

  // This is the definition for the nested class '_Paths'
  [Serializable]
  public class _Paths
  {
    //public delegate void ASMEventHandler (string propname, object propvalue, Type proptype, System.EventArgs e);
    public event SysInfo.ASMEventHandler ModelEvent;


    // Constructor for the "_Paths" class
    public _Paths()
    {
    }


    // This method is called from every property in this class that wants to utilize the
    // Automatic Synchronization Mechanism (ASM).  The Controller subscribes to these
    // events and correctly sets the corresponding linked form control.
    private void FireASM (string propertyName, object propertyValue, EventArgs e)
    {
      try
      {
        if (ModelEvent != null)
        {
          // Because this method resides in a nested class we need to tack on information
          // that specifies which nested class the event is being fired from.
          propertyName = "Paths." + propertyName;
          ModelEvent(propertyName, propertyValue, new EventArgs());
        }
      }
      catch
      {
        // Handle exceptions
        Debug.Fail("FireASM Exception in SysInfo model", "Paths");
      }
    }

    private void FireASM (string propertyName, object propertyValue)
    {
      FireASM(propertyName, propertyValue, new EventArgs());
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
        FireASM("App", _app);
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
        FireASM("Data", _data);
      }
    }

    private string _incoming = "";
    [XmlIgnore]
    public string Incoming
    {
      get
      {
        return _incoming;
      }
      set
      {
        _incoming = value;
        FireASM("Incoming", _incoming);
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
        FireASM("Templates", _templates);
      }
    }

    private string _archive = "";
    [XmlIgnore]
    public string Archive
    {
      get
      {
        return _archive;
      }
      set
      {
        _archive = value;
        FireASM("Archive", _archive);
      }
    }

    private string _currentData = "";
    [XmlIgnore]
    public string CurrentData
    {
      get
      {
        return _currentData;
      }
      set
      {
        _currentData = value;
        FireASM("CurrentData", _currentData);
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
        FireASM("Help", _help);
      }
    }

    private string _mobileCabs = "";
    [XmlIgnore]
    public string MobileCabs
    {
      get
      {
        return _mobileCabs;
      }
      set
      {
        _mobileCabs = value;
        FireASM("MobileCabs", _mobileCabs);
      }
    }

    private string _temp = "";
    [XmlIgnore]
    public string Temp
    {
      get
      {
        return _temp;
      }
      set
      {
        _temp = value;
        FireASM("Temp", _temp);
      }
    }
  }

  #endregion


  #region PollSummaries

  // Note: This class is identical to "_Summaries" in CFSysInfo except for the absence of the Active collection.
  //       A different name had to be used because the two share the same namespace.
  public class _PollSummaries
  {
    // Constructor for the "_PollSummaries" class
    public _PollSummaries()
    {
      instanceTemplates = new _Templates(this);    // Instantiate a new, albeit empty, Templates collection, passing this instance to "_PollSummaries"
    }

    private _Templates instanceTemplates;
    public _Templates Templates
    {
      get
      {
        return instanceTemplates;
      }
    }
  }


  #region Templates
  // This is the definition for the collection class '_Templates', which will be instantiated into the collection 'Templates'.

  public class _Templates : PPbaseCollection
  {
    private _PollSummaries pollSummaries;

    public _Templates(_PollSummaries pollSummaries)
    {
      this.pollSummaries = pollSummaries;
    }


    // Indexer
    public new _TemplateSummary this[int index]
    {
      get
      {
        return (_TemplateSummary) base[index];
      }
      set
      {
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
        if (summary.Filename == filename)
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
        if (summary.Filename == filename)
          return summary;
      }

      return null;  // If reaches here then a Summary object with the specified filename was not found
    }


    // Add other type-safe methods here
    // ...
    // ...
  }

  #endregion

  // Note: "_TemplateSummary" is now located in BaseClasses.cs because it is shared by both SysInfo & CFSysInfo.



  #endregion





}
