using System;
using System.IO;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;


namespace DataObjects
{

  // Define Aliases
  using ReflectionMode = Constants.ReflectionMode;
  using ExportFormat = Constants.ExportFormat;
  using XMLHeaderType = Constants.XMLHeaderType;
  using ASMEventHandler = Poll.ASMEventHandler;
  using PublishedLock = Constants.PublishedLock;


  /// <summary>
  /// Forms the basis of an indexing scheme that allows model properties to quickly find which control(s) they're
  /// associated with.  When instantiated into an object, is added to the 'ASMlist' collection, which is indexed
  /// by property path strings such as "CreationInfo.AutoPurge" and "Questions[].Text".
  /// </summary>
  public class ASMinfo
  {
    public Control Control;
    public string PropertyPath;            // Full pathname of property location in the model - obtained from 'Property' in Tag
    public string Value;                   // Value that will be set in the model when control is selected (clicked, checked, etc.)
    public string Default;                 // The default value, in case 'Value == ""'
    public PublishedLock PubLock;          // A "published lock": If the poll has been published and the control's value is altered then provides warning
    public string Source;                  // Future Use: For initially populating control
    public ArrayList SiblingControls;      // We'll also populate this collection with ASMinfo elements, but these don't need to be indexed by a key

    public ASMinfo()
    {
      SiblingControls = new ArrayList();
    }
  }



	/// <summary>
	/// This class contains assorted methods and classes to facilitate ASM (Automated Synchronization Mechanism).
	/// </summary>

  public class ASM
	{
    private static string ChoiceTags = "";


		public ASM()
		{
		}


    // Searches through an ASMinfo object that contains multiple controls (such as with radioButtons)
    // and retrieves the 'Value' field associated with the specified control.
    public static object GetSiblingValue (Control control, ASMinfo asmInfo)
    {
      if (asmInfo.Control.Equals(control))    // Is it the first control, in the root of the asmInfo object?
        return asmInfo.Value;
      else                                    // No, so look for it in the sibling controls
      {
        foreach (ASMinfo siblingInfo in asmInfo.SiblingControls)
        {
          if (siblingInfo.Control.Equals(control))
            return siblingInfo.Value;
        }
      }

      // There may be some strange case where a control isn't found.
      Debug.Fail("Unable to find specified control [" + control.Name + "] in ASMinfo object", "Tools.GetSiblingValue");
      return null;
    }



    // Decodes data stored in 'Tag' property into constituent components.
    public static void DecodeTagData (Control ctrl, ASMinfo asmInfo)
    {
      string _tag = ctrl.Tag as String;
      string[] _tagList = _tag.Split(new char[] {','});

      foreach (string _txt in _tagList)
      {
        string[] _txtList = _txt.Split(new char[] {'='});

        if (_txtList.Length == 2)
        {
          string _field = _txtList[0].ToLower().Trim();
          string _value = _txtList[1].Trim();

          switch (_field)
          {
            case "property":
              asmInfo.PropertyPath = _value;
              break;

            case "value":
              asmInfo.Value = _value;
              break;

            case "default":
              asmInfo.Default = _value;
              break;

            // "Published Lock"
            case "publock":
              try
              {
                asmInfo.PubLock = (PublishedLock) Enum.Parse(typeof(PublishedLock), _value, true);
              }
              catch (Exception ex)
              {
                Debug.Fail("Error trying to decode tag: " + _value + "\n\n" + ex.Message, "DataObjects.ASM.DecodeTagData");
              }
              break;

            case "source":
              asmInfo.Source = _value;
              break;

            default:
              Debug.Fail("Unknown field value in Tag: " + _txtList[0].ToString() + "   Control: " + ctrl.Name, "Tools.DecodeTagData");
              break;
          }
        }
        else
        {
          Debug.Fail("Unrecognizable value in Tag: " + _txtList[0].ToString() + "   Control: " + ctrl.Name, "Tools.DecodeTagData");
        }
      }
    }


    /// <summary>
    /// Takes a control and correctly adds it and its information into ASMlist.
    /// </summary>
    /// <param name="ASMlist"></param>
    /// <param name="ctrl"></param>
    /// <returns>true - A new entry was added to ASMlist; false otherwise</returns>
    public static bool ActivateASM (SortedList ASMlist, Control ctrl)
    {
      ASMinfo asmInfo = new ASMinfo();
      string tag = ctrl.Tag as String;

      if ((tag != "") & (tag != null))
      {
        DecodeTagData(ctrl, asmInfo);   // Populate 'asmInfo' with data from the tag property of 'ctrl'
        asmInfo.Control = ctrl;         // Finish "basic" population of 'asmInfo' object

        // Development Notes:
        // Original Approach: Obtain the actual Property object in the model
        // Now:  We're no longer going to do this at this juncture because until collections
        //       are actually populated, the property paths are meaningless.  For example,
        //       there's no way to determine if "Questions[].Text" is a legitimate path until
        //       the 'Questions' collection is populated.
        // Future Idea: We could try populating each collection with a single element to test them.
        //              This population could even be hard-coded.

        string propertyPath = asmInfo.PropertyPath;

        // Build a list of  property paths that contain "Choices[]"
        if (propertyPath.IndexOf("Choices[]") != -1)
          if (ChoiceTags.IndexOf(propertyPath + ",") == -1) 
            ChoiceTags = ChoiceTags + propertyPath + ",";

        // See if this Property Path has already been added to the collection.  This would be the case for radio buttons.
        // If not then add a new entry to the collection; if so then add the new entry to the nested collection.
        if (ASMlist.ContainsKey(propertyPath))
        {
          // An object with the 'propertyPath' key already exists in the collection so this must be an associated control.
          //ASMinfo asmInfo1 = ASMlist[propertyPath] as ASMinfo;   // Get first entry for this property path from the collection
          Object objTmp = ASMlist[propertyPath];
          ASMinfo asmInfo1 = objTmp as ASMinfo;

          asmInfo1.SiblingControls.Add(asmInfo);
        }
        else  // No entry yet for this 'propertyPath' key so add directly at root level of collection
        {
          ASMlist.Add(propertyPath, asmInfo);
        }

        // Finally, we're going to replace the Tag value with just the 'PropertyPath'
        // string, since the 'Value' and 'Source' data is now stored in ASMlist.
        ctrl.Tag = propertyPath;

        return true;
      }
      else
        return false;
    }

  
    public static void ClearASMofChoiceTags (SortedList ASMlist)
    {
      string[] ChoiceTagList = ChoiceTags.Split(new char[] {','});
      foreach (string ChoiceTag in ChoiceTagList)
      {
        ASMlist.Remove(ChoiceTag);
      }
    }





	}
}
