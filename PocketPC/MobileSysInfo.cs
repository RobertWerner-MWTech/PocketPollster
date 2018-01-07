using System;
using System.Collections;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Serialization;


namespace DataObjects
{
  /// <summary>
  /// This is the mobile device version of 'SysInfo'
  /// It consists of these major sections:
  /// 
  /// Name                  Type
  /// ---------------------------------
  /// Admin                 Class
  /// Options               Class
  /// Paths                 Class
  /// 
  /// </summary>



  public class MobileSysInfo
  {
    // Note: I first tried implementing this as a Singleton class, but it presented problems for 'Tools.SaveData'.
    //       So I'm going to implement it as a regular class.  If other developers take over this code one day
    //       then they need to understand not to instantiate it more than once, because that would serve no purpose.

    // This instantiation makes the MobileSysInfo data global.
    public static MobileSysInfo Data = new MobileSysInfo();

    // Constructor for MobileSysInfo
    public MobileSysInfo()
    {
    }

    public delegate void ASMEventHandler (string propName, object propValue, EventArgs e);


    // In this class we will have two types of objects at the next level below the root:
    //   - Classes
    //   - Collections (in mobile ?)
    #region NestedClassesAndCollections

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

    #endregion


    /// <summary>
    /// Initializes a Brand New MobileSysInfo object with as much default information as possible.
    /// </summary>
    public void Initialize()
    {
    }

  }  // end of class MobileSysInfo


  #region Admin
  // This is the definition for the nested class '_Admin'
  public class _Admin
  {
    // Properties:

    private string _appname = "Pocket Pollster";
    public string AppName
    {
      get
      {
        return _appname;
      }
      set
      {
        _appname = value;
      }
    }

    private string _appFilename = "PP.exe";
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
  }
  #endregion


  #region Options

  // This is the definition for the nested class '_Options'
  public class _Options
  {
    // Constructor for "_Options" class
    public _Options()
    {
    }


    // Properties:
  
    private ushort _defaultLanguageID = 0;  // Default = English
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
      }
    }

    // This is the username, like "rwerner"
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
      }
    }

    private bool _allowNameEditing = false;
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
  }

  #endregion


  #region Paths

  // This is the definition for the nested class '_Paths'.
  // Note: Because App is discerned directly from the EXE and Data is obtained from the Registry,
  //       none of the paths found here are retrieved from SysInfo.xml.  This is somewhat different
  //       than on the desktop.
  
  public class _Paths
  {
    // Constructor for the "_Paths" class
    public _Paths()
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
    [XmlIgnore]
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

}
