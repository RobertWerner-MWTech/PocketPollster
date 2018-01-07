using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;


namespace DataObjects
{
  /// <summary>
  /// This is a special null safe collection that we'll use as the base collection class throughout this project.
  /// This class is meant to be extended by a type safe class so that the setter methods are protected.
  /// It also simplifies casting within the ASM code - See usage in 'Tools.cs' -> Now 'ASM.cs'.
  /// </summary>
  public class PPbaseCollection : System.Collections.CollectionBase
  {
    // Class is not meant to be instantiated, only inherited
    protected PPbaseCollection()
    {
    }

    // Provide an indexer
    // Note: Normally this would be declared 'protected' but I've made it 'public' in order to
    //       be able to use 'PPbaseCollection' as a generic collection for casting purposes.
    //       I'm not sure if this is improper for more public uses - ie. use by other developers.
    public object this[int index]
    {
      get
      {
        //throws ArgumentOutOfRangeException
        return List[index];
      }
      set
      {
        //throws ArgumentOutOfRangeException
        //Insert(index, value);
        List[index] = value; 
      }
    }


    // Custom implementations of the protected members of IList
    // These methods are for internal use by a type safe subclass.

    protected int Add(object value)
    {
      if (value != null)
      {
        // throws NotSupportedException
        return List.Add(value);
      }
      else
      {
        return -1;
      }
    }

    protected void Insert(int index, object value)
    {
      if (value != null)
      {
        //throws ArgumentOutOfRangeException
        List.Insert(index, value);
      }
      // else do nothing
    }

    protected void Remove(object value)
    {
      List.Remove(value);
    }

    protected bool Contains(object value)
    {
      return List.Contains(value);
    }

    // Returns the index position that the 'object' occupies in the collection.
    protected int IndexOf(object value)
    {
      return List.IndexOf(value);
    }

       
    // RW Note: This next one came from the original code on the Internet but not sure whether to keep it.
    public void CopyTo(Array array, int start)
    {
      //throws ArgumentOutOfRangeException
      List.CopyTo(array, start);
    }


    // RW: I think that more methods could be added


  }


  #if (CF)

    public class SortedList : PPbaseCollection
    {
      // Indexer
      public new SortedList this[int index]
      {
        get
        {
          //throws ArgumentOutOfRangeException
          return (SortedList) base[index];
        }
        set
        {
          //throws ArgumentOutOfRangeException
          base.Insert(index, value);
        }
      }

      public int Add(SortedList value)
      {
        int index = base.Add(value);
        return index;
      }

      public void Insert(int index, SortedList value)
      {
        base.Insert(index, value);
      }

      public void Remove(SortedList value)
      {
        base.Remove(value);
      }

      public bool Contains(SortedList value)
      {
        return base.Contains(value);
      }

      // Returns the index position that the 'value' occupies in the collection.
      public int IndexOf(SortedList value)
      {
        return base.IndexOf(value);
      }

      // Searches the questions in the collection for a question with the specified text.
      // Returns 'true' if found, 'false' otherwise.
//      public bool ContainsText(string text)
//      {
//        for (int i = 0; i < List.Count; i++)
//        {
//          SortedList question = (SortedList) List[i];
//          if (question.Text == text)
//            return true;
//        }
//
//        return false;  // If reaches here then a question with the specified text was not found
//      }
//
//      // Searches the questions in the collection for a question with the specified text.
//      // Returns the question if it finds it, otherwise 'null'.
//      public SortedList FindSortedList(string text)
//      {
//        for (int i = 0; i < List.Count; i++)
//        {
//          SortedList question = (SortedList) List[i];
//          if (question.Text == text)
//            return question;
//        }
//
//        return null;  // If reaches here then a question with the specified text was not found
//      }

      // Add other type-safe methods here
      // ...
      // ...
    }

  #endif  



  // This is the definition for the class '_TemplateSummary', which will populate the collection 'Templates'.
  // It is used by both SysInfo and CFSysInfo.
  public class _TemplateSummary
  {
    public _TemplateSummary()
    {
    }

    // Sometimes most/all of the property values are already known.  This constructor allows them to be quickly set.
    public _TemplateSummary(_TemplateSummary summary)
    {
      Filename = summary.Filename;
      Revision = summary.Revision;
      PollSummary = summary.PollSummary;
      NumQuestions = summary.NumQuestions;
      PollGuid = summary.PollGuid;
      LastEditGuid = summary.LastEditGuid;
    }

    // Here an alternative constructor for when all the values are already known.
    public _TemplateSummary(string filename, int revision, string pollSummary, int numQuestions, string pollGuid, string lastEditGuid)
    {
      Filename = filename;
      Revision = revision;
      PollSummary = pollSummary;
      NumQuestions = numQuestions;
      PollGuid = pollGuid;
      LastEditGuid = lastEditGuid;
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
  
    private string _pollGuid;
    public string PollGuid
    {
      get
      {
        return _pollGuid;
      }
      set
      {
        _pollGuid = value;
      }
    }
  
    private string _lastEditGuid;
    public string LastEditGuid
    {
      get
      {
        return _lastEditGuid;
      }
      set
      {
        _lastEditGuid = value;
      }
    }
  }














}
