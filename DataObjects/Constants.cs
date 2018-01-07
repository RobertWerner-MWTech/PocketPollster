using System;
using System.Diagnostics;


namespace DataObjects
{
  namespace Constants
  {
    public enum PollType : byte
    {
      Standard,
      Branching,
      Quiz
    }

    public enum OpMode : byte
    {
      Viewer = 0,
      Standard = 1
    }

    public enum DeviceType : byte
    {
      Windows,
      Web,
      Pocket_PC,
      Cel_Phone
    }

    public enum DeviceOrientation : byte
    {
      Portrait,
      Landscape
    }

    // This enum defines the different kinds of AnswerFormats available.  It's also used to instantiate the radio buttons
    // that sit near the top of frmPoll.  They are used to decide what type of answer format a given question will have.
    // Notes:
    //   - Changing the order of these enum fields will change the order of the radioAnswer buttons in frmPoll
    //   - If an item is added here then its corresponding full-text version must be added to Desktop.resx
    public enum AnswerFormat : byte
    {
      Standard,
      List,
      DropList,
      MultipleChoice,
      Range,
      MultipleBoxes,
      Freeform,
      Spinner
    }
  
    public enum ReflectionMode : byte
    {
      FindProperty,
      GetValue,
      SetValue
    }

    public enum ExportFormat : byte
    {
      XML,
      XMLSchema,   // Not sure we need to do this one because of automated conversion from 'XML'
      MDB,
    }

    public enum XMLHeaderType : byte
    {
      Start,
      End,
      Both
    }

    public enum InstallationMode : byte
    {
      New,
      Update,
      Reinstall,
      NotSupported
    }

    // Specialized versions could be added into the 11 - 55 / 111 - 155 / 211 - 255 ranges
    public enum ProductID : byte
    {
      Lite = 0,
      Plus = 1,
      Pro = 100,
      Enterprise = 200
    }


    public enum PurgeDuration : int
    {
      After_Download = 0,
      One_Day = 1,
      Two_Days = 2,
      Three_Days = 3,
      Four_Days = 4,
      Five_Days = 5,
      One_Week = 7,
      One_Month = 30,
      Never = -1
    }

    public enum Sex : byte
    {
      Unspecified,     // This is necessary so that the sex isn't defaulted to Male in case the user never enters it
      Male,
      Female
    }

    public enum MobilePlatform : byte
    {
      // Note: The first value is actually "Palm PC2" but spaces aren't allowed in enums.  
      //       All code currently only tests for the "SmartPhone" value.
      Palm_PC2,         // Used by the Pocket PC 2000 devices like the HP Jornada 540 series
      PocketPC,
      SmartPhone        // Need to confirm this one!
    }
   
    public enum InstructionsType : byte
    {
      BeforePoll,
      BeginMessage,
      AfterPoll,
      EndMessage,
      AfterAllPolls
    }

    public enum TextFormat : byte
    {
      None,
      NumericOnly,
      LowerCase,
      UpperCase,
      ProperCase
    }

    public enum SelectFileMode : byte
    {
      RunPoll,
      ReviewData
    }

    public enum ViewMode : byte
    {
      Users,
      Devices
    }

    // The relative speed at which data is transferred to a mobile device.
    // Currently only used to adjust velocity rate of Data Xfer progress bar.
    public enum DataXferSpeed : byte      
    {
      VerySlow,      // Pocket PC 2002 (wce300/arm)
      Slow,          // Not yet used; reserved for a future device
      Normal,        // Currently used for all wce400/armv4 devices
      Fast,          // Not yet used; reserved for a future device
      VeryFast       // Not yet used; reserved for a future device
    }

    // Used with ASM to mark certain controls as being "locked" once a poll is published.  The severity of the lock
    // determines what warning message is displayed to the user if the control's value is changed.
    public enum PublishedLock : byte
    {
      None,
      Instructions,      // Likely few catastrophic consequences if value is changed
      QA_Basic,          // Ditto, unless text is dramatically altered
      AnswerFormat,      // More serious problems could result in poll results if value is changed
      ChoiceStructure,   // Severe problems will likely result if value is structure is altered
      QuestionStructure  // Ditto
    }

    public enum MDBformat : byte
    {
      Older = 4,
      Newer = 5
    }

    public enum GraphType : byte
    {
      None,      // This is not a chart but simply indicates the lack of any data displayed
      Bar,
      Line,
      Pie,
      Text,
      Freeform   // This isn't really a type of chart, but lets code distinguish it from the other types
    }

    public enum GraphAvgMethod : byte
    {
      Simple,
      Weighted
    }


  }
}
