using System;
using System.IO;
using System.Xml;
using System.Data;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;


namespace DataObjects
{
  // Define Aliases
  using ReflectionMode = Constants.ReflectionMode;
  using ExportFormat = Constants.ExportFormat;
  using XMLHeaderType = Constants.XMLHeaderType;
  using TextFormat = Constants.TextFormat;
  using ASMEventHandler = Poll.ASMEventHandler;
  using AnswerFormat = Constants.AnswerFormat;
  using _ActivePollSummary = _Summaries._ActivePollSummary;


	/// <summary>
	/// Tools contains a potpourri of methods & classes that are used by both the Desktop and Mobile apps.
	/// </summary>

  public class Tools
  {
    // These two variables are used by PopulateObject, PopulateCollection, and PopulateProperty.
    // Note: Because they are module-level variables, they need to be explicitly reset after being
    //       used for a given general operation.
    private static int dataRowIdx = -1;                 
    private static string currDataRelation = "";        


    public static string ObjectType(object obj)
    {
      string _objNamespace = obj.GetType().Namespace;
      string _objType = (obj.GetType().FullName).Substring(_objNamespace.Length + 1);

      return _objType;
    }



    /// <summary>
    /// Retrieves the actual Property within a model.  This property can exist anywhere in the model.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="propPath"></param>
    /// <returns></returns>
    public static PropertyInfo FindProperty(Poll model, string propPath)
    {
      return ModelReflection(ReflectionMode.FindProperty, model, propPath, null) as PropertyInfo;
    }


    /// <summary>
    /// Retrieves the value of a Property within a model.  The property can exist anywhere in the model.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="propPath"></param>
    /// <returns></returns>
    public static object GetPropertyValue(Poll model, string propPath)
    {
      return ModelReflection(ReflectionMode.GetValue, model, propPath, null);
    }
    

    /// <summary>
    /// Sets the value of a Property within a model.  The property can exist anywhere in the model.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="propPath"></param>
    /// <param name="newval"></param>
    public static void SetPropertyValue(Poll model, string propPath, object newval)
    {
      ModelReflection(ReflectionMode.SetValue, model, propPath, newval);
    }


    /// <summary>
    /// This method combines the functionality of what used to be three separate, though nearly identical methods.
    /// It is called by 'FindProperty', 'GetPropertyValue', and 'SetPropertyValue'.
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="model"></param>
    /// <param name="propPath"></param>
    /// <param name="newval"></param>
    /// <returns></returns>
    public static object ModelReflection(ReflectionMode mode, Poll model, string propPath, object newval)
    {
      object[] indexer = new object[0];
      PropertyInfo propInfo = null;
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

        if (currentNode.ParentIsCollection)
        {
          propInfo = (parentNode.Obj as PPbaseCollection)[parentNode.Idx].GetType().GetProperty(currentNode.PropName);

          if (propInfo != null)
          {
            if ((mode == ReflectionMode.SetValue) && (i == propPathList.Length - 1))
            {
              // Note: The 3rd parameter in ChangeType (ie. null) is necessary to get it to work with the CF too
              propInfo.SetValue((parentNode.Obj as PPbaseCollection)[parentNode.Idx], Convert.ChangeType(newval, propInfo.PropertyType, null), indexer);

              return null;   // Value now set so we can leave
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
            if ((mode == ReflectionMode.SetValue) && (i == propPathList.Length - 1))
            {
              // Note: The 3rd parameter in ChangeType (ie. null) is necessary to get it to work with the CF too
              propInfo.SetValue(currentNode.Obj, Convert.ChangeType(newval, propInfo.PropertyType, null), indexer);
              return null;   // Value now set so we can leave
            }
            else
              currentNode.Obj = propInfo.GetValue(currentNode.Obj, indexer);
          }
        }

        currentNode.ObjType = propInfo.PropertyType;   // Note: This is preferable to 'currentNode.Obj.GetType()' because it fails if a null string

        // Prepare for next iteration through loop
        parentNode.Clear();
        if (currentNode.IsCollection)
        {
          currentNode.CopyTo(parentNode);
          currentNode.Clear();
          currentNode.ParentIsCollection = true;
        }
      }

      // Finalization code.  Needed, for example, to ensure that a null string is represented as ""
      if (mode != ReflectionMode.SetValue)
      {
        if (currentNode.Obj == null)
        {
          if (currentNode.ObjType == null)
            Debug.Fail("Could not work through the path '" + propPath + "'", "Tools.ModelReflection - Mode: " + mode);

          else if (currentNode.ObjType.Name == "String")
            currentNode.Obj = "";
        }

        if (mode == ReflectionMode.FindProperty)
          return propInfo;

        if (mode == ReflectionMode.GetValue)
          return currentNode.Obj;
      }
    
      return null;  // Never actually executed but necessary so that code will compile
    
    }    // End of 'ModelReflection'



    // Used by 'ModelReflection', 'SaveData', and 'OpenData' to hold parent & child data while working its way down a property path.
    public class NodeInfo
    {
      public string PropName;                      // The portion of the property path that 'Obj' refers to
      public object Obj;
      public Type ObjType;
      public int Idx;                              // The index of a collection
      public bool IsCollection;
      public bool ParentIsCollection;

      // Default constructor
      public NodeInfo()
      {
        Idx = -1;
        IsCollection = false;
        ParentIsCollection = false;
      }

      public void CopyTo(NodeInfo node)
      {
        node.PropName = PropName;
        node.Obj = Obj;
        node.ObjType = ObjType;
        node.IsCollection = IsCollection;
        node.Idx = Idx;
        node.ParentIsCollection = ParentIsCollection;
      }
    
      public void Clear()
      {
        this.PropName = null;
        this.Obj = null;
        this.ObjType = null;
        this.Idx = -1;
        this.IsCollection = false;
        this.ParentIsCollection = false;
      }
    }


    // Used by 'GetRespondentInfoArray' to populate an ArrayList, which is used by the mobile app (and probably elsewhere in the future).
    public class RespondentInfo
    {
      public int Index;                // Seems redundant but is necessary as a reverse index when retrieved from control's tag
      public string PropName;
      public string LabelText;
      public string Value = "";        // This is the value passed to/from the form control; it's only necessary for non-textbox controls to represent no choice
      public string Presets;
      public TextFormat Formatting;
    }

    /// <summary>
    /// This method creates an ArrayList of Respondent fields that we'll use to populate forms.
    /// Note: It must be updated if the _Respondent class is updated.  Currently it is only used
    ///       by the Pocket PC app.
    /// </summary>
    /// <returns></returns>
    public static ArrayList GetRespondentInfoArray()
    {
      // These refer to the precise property names in Poll.Respondents.Respondent
      string fields = "FirstName, LastName, Address, City, StateProv, PostalCode, AreaCode, TelNum, Email, Sex, Age";

      // These refer to the intro label that will be displayed on the form
      string labels = "First Name, Last Name, Address, City, State/Prov, Postal/Zip, Area Code, Tel Num, E-mail, Sex, Age";
      
      ArrayList respondentArray = new ArrayList();

      string[] fieldList = fields.Split(new char[] {','});
      string[] labelList = labels.Split(new char[] {','});

      for (int i = 0; i < fieldList.Length; i++)
      {
        RespondentInfo respInfo = new RespondentInfo();
        respInfo.Index = i;
        respInfo.PropName = fieldList[i].Trim();
        respInfo.LabelText = labelList[i].Trim();

        respondentArray.Add(respInfo);
      }

      (respondentArray[0] as RespondentInfo).Formatting = TextFormat.ProperCase;      // FirstName
      (respondentArray[1] as RespondentInfo).Formatting = TextFormat.ProperCase;      // LastName
      (respondentArray[3] as RespondentInfo).Formatting = TextFormat.ProperCase;      // City
      (respondentArray[6] as RespondentInfo).Formatting = TextFormat.NumericOnly;     // Area Code
      (respondentArray[10] as RespondentInfo).Formatting = TextFormat.NumericOnly;    // Age

      (respondentArray[9] as RespondentInfo).Value = "";     // Sex
      (respondentArray[9] as RespondentInfo).Presets = Constants.Sex.Male.ToString() + "," + Constants.Sex.Female.ToString();

      return respondentArray;
    }


    /// <summary>
    /// "contents" is a CDF string that is used to populate the ComboBox.
    /// </summary>
    /// <param name="combo"></param>
    /// <param name="contents"></param>
    public static void FillCombo(ComboBox combo, string contents)
    {
      if ((contents != null) && (contents != ""))
      {
        string[] contentList = contents.Split(new char[] {','});
        foreach (string text in contentList)
        {
          combo.Items.Add(text);
        }
      }
    }




    #region Exporting

    /// <summary>
    /// Uses Reflection to iteratively cycle through a hierarchical data model and save its contents to a file.
    /// Note: Any properties that are tagged with the [XMLIgnore] attribute will not be written to the file.
    /// </summary>
    /// <param name="destination"></param>   // The filename we'll be saving the data to
    /// <param name="model"></param>         // Note: Original type was 'Poll' but changed to 'Object' to support other objects
    /// <param name="propPath"></param>      // Future idea to allow saving just a portion of the hierarchical tree
    /// <param name="format"></param>        // The export format to use
    public static void SaveData (string destination, object model, string propPath, ExportFormat format)
    {
      // Prepare node for the root of the model.  We'll use 'NodeInfo', though not all of its properties.
      NodeInfo rootNode = new NodeInfo();
      rootNode.Obj = model;
      rootNode.ObjType = model.GetType();

      switch (format)
      {
        case ExportFormat.XML:
          // Prepare for writing to external file 
          // Note: In the future, it may be possible to do this to an http:// destination!

          try
          {
            // Delete previous file, if it exists
            if (File.Exists(destination))
              File.Delete(destination);

            // Specify filename, file handling instructions, and access mode
            FileStream fileStream = new FileStream(destination, FileMode.Create, FileAccess.Write);
       
            // Create a new stream to write to the file
            StreamWriter stream = new StreamWriter(fileStream);

            // Write out start of XML file
            SendToStream("?xml version=\"1.0\"?", null, XMLHeaderType.Start, 0, stream, format, true);
            SendToStream(rootNode.ObjType.Name, null, XMLHeaderType.Start, 0, stream, format, true);    // ie. "<Poll>"

            // We will now initiate a recursive exploration through the hierarchical tree of the data model.
            // Because 'model' is itself a class, we shall start everything with 'ExportClasses'.
            ExportClasses(rootNode, 0, stream, format, propPath, "", true);

            // Note: Reaching here, the entire data model (or the specified portion) should have been written to the file.
            //       We just need to add the final closing construct.
            SendToStream(rootNode.ObjType.Name, null, XMLHeaderType.End, 0, stream, format, true);      // ie. "</Poll>"

            stream.Close();       // Close StreamWriter
            fileStream.Close();   // Close file
          }

          catch (Exception e)
          {
            ShowMessage("Could not save file: " + destination + "\n\nError: " + e.Message, "Error Saving File");
          }

          break;

        case ExportFormat.MDB:
          #if (!CF)

          try
          {
            // Create database, if it doesn't yet exist
            // Note: It may be necessary to always append and not overwrite, in case links exist from the MDB to other files
            if (!File.Exists(destination))
              DBTools.CreateDatabase(destination);

            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
            conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + destination;

            conn.Open();
            object[] objArrRestrict;
            // Select just TABLE in the Object array of restrictions (remove TABLE and insert Null to see tables, views, and other objects)
            // Restrictions: {TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE}
            objArrRestrict = new object[] {null, null, null, "TABLE"};
            DataTable schemaTable;
            schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, objArrRestrict);
            conn.Close();

            Poll pollModel = (Poll) model;

            // See if the Questions table exists yet
            if (!DBTools.IsItemInTable(schemaTable, "TABLE_NAME", "Questions"))
            {
              string cmdText = "CREATE TABLE Questions (QuestionNum Int Primary Key, QuestionText Memo, AnswerFormat Char(25))";
              DBTools.RunSQLNonQuery(conn, cmdText);
            }

            // See if the Choices table exists yet
            // Note: We're creating this one first so we can create the relationship with Questions during its creation
            if (!DBTools.IsItemInTable(schemaTable, "TABLE_NAME", "Choices"))
            {
              string cmdText = "CREATE TABLE Choices (refQuestionNum Int, ChoiceNum Int, ChoiceText Memo, ExtraInfo_Intro Memo, Primary Key (refQuestionNum, ChoiceNum), CONSTRAINT FK_Questions FOREIGN KEY(refQuestionNum) References Questions(QuestionNum))";
              DBTools.RunSQLNonQuery(conn, cmdText);
            }

            // See if the Respondents table exists yet
            if (!DBTools.IsItemInTable(schemaTable, "TABLE_NAME", "Respondents"))
            {
              string cmdText = "CREATE TABLE Respondents (RespNum Int Primary Key, PollsterName Char(25), [Guid] Char(36), " +
                               "TimeCaptured Date, FirstName Char(25), LastName Char(25), Address Char(75), City Char(30), " +
                               "StateProv Char(20), PostalCode Char(15), AreaCode Char(5), TelNum Char(20), [E-mail] Char(50), Sex Char(10), Age Char(3))";

              DBTools.RunSQLNonQuery(conn, cmdText);
            }

            // Populate the Questions & Choices tables

            // First clear both tables
            DBTools.RunSQLNonQuery(conn, "DELETE * FROM Choices");      // Need to clear this one first because of relationship with Questions
            DBTools.RunSQLNonQuery(conn, "DELETE * FROM Questions");

            // Now do the actual population
            string sqlText = "";
            int questNum = 1;
            foreach (_Question question in pollModel.Questions)
            {
              // QuestionNum   Text   AnswerFormat
              sqlText = CreateSQLInsertString("Questions", new object[3] {questNum, question.Text, question.AnswerFormat});
              if (DBTools.RunSQLNonQuery(conn, sqlText))   // Add record to Questions table
              {
                int choiceNum = 1;
                foreach (_Choice choice in question.Choices)
                {
                  // refQuestionNum   ChoiceNum   Text   ExtraInfo_Intro
                  if (choice.ExtraInfo == false || question.AnswerFormat == AnswerFormat.Spinner)
                    sqlText = CreateSQLInsertString("Choices", new object[4] {questNum, choiceNum, choice.Text, ""});
                  else
                    sqlText = CreateSQLInsertString("Choices", new object[4] {questNum, choiceNum, choice.Text, choice.MoreText});
                  DBTools.RunSQLNonQuery(conn, sqlText);
                  choiceNum++;
                }
              }

              questNum++;
            }


            // If they don't yet exist, create tables Question_1 ... Question_N, with each customized individually
            string questNumLookup = "";

            questNum = 1;
            foreach (_Question question in pollModel.Questions)
            {
              // Prepare this lookup mechanism, that will be utilized below, after this construct
              questNumLookup += question.ID.ToString() + "~" + questNum.ToString() + ",";     // Format: questID~questNum

              // See if the Question_# table exists yet; if not, then create it
              string tableName = "Question_" + questNum.ToString();

              if (!DBTools.IsItemInTable(schemaTable, "TABLE_NAME", tableName))
              {
                // Idx  refRespNum  Guid  TimeCaptured  Choice1  Choice2  Choice2Extra  Choice3 ... ChoiceN
                // Note: Though 'Idx' is essentially an autoincrement field, we can't declare it as such because when we
                //       erase all of the records in order to repopulate the table, we want the first value to start again at '1'.
                string cmdText = "CREATE TABLE " + tableName + " (Idx Int Primary Key, refRespNum Int, [Guid] Char(36), TimeCaptured Date, ";

                // Add Choice# and Choice#_ExtraInfo fields
                cmdText += DetermineChoiceFields(question.Choices, question.AnswerFormat);

                // If this poll involves the gathering of Respondent info then add a Constraint that establishes relationship with the Respondents table
                if (pollModel.CreationInfo.GetPersonalInfo)
                  cmdText += "CONSTRAINT FK_Respondents" + questNum.ToString() + " FOREIGN KEY(refRespNum) References Respondents(RespNum))";
                else
                  cmdText = cmdText.Substring(0, cmdText.Length - 2) + ")";
                
                DBTools.RunSQLNonQuery(conn, cmdText);
              }
              else
                DBTools.RunSQLNonQuery(conn, "DELETE * FROM " + tableName);   // Already exists so just remove old entries from this Question_# table
              
              questNum++;
            }

            // Populate the Respondents table
            int respondNum;   // Need to define here because used further below as well
            if (pollModel.CreationInfo.GetPersonalInfo)
            {
              DBTools.RunSQLNonQuery(conn, "DELETE * FROM Respondents");   // First remove old entries; Note: Must be done after Question_# tables are cleared!

              respondNum = 1;
              foreach (_Respondent resp in pollModel.Respondents)
              {
                // RespNum PollsterName Guid TimeCaptured FirstName LastName Address City StateProv PostalCode AreaCode TelNum Email Sex Age
              
                // First need to custom prepare some fields
                string sex = resp.Sex.ToString();
                sex = (sex.ToLower() == "unspecified") ? "" : sex;
                string age = resp.Age.ToString();
                age = (age == "0") ? "" : age;

                sqlText = CreateSQLInsertString("Respondents", new object[15] {respondNum, resp.PollsterName, resp.Guid, resp.TimeCaptured, resp.FirstName, resp.LastName, resp.Address, resp.City, resp.StateProv, resp.PostalCode, resp.AreaCode, resp.TelNum, resp.Email, sex, age});
                DBTools.RunSQLNonQuery(conn, sqlText);
                respondNum++;
              }
            }


            // Now populate each Question_# table

            // Create an index array that will keep track of the current Idx value for each Question_# table
            int[] idxValues = new int[pollModel.Questions.Count];
            for (int i = 0; i < pollModel.Questions.Count; i++)
            {
              idxValues[i] = 1;
            }

            respondNum = 1;
            foreach (_Respondent respondent in pollModel.Respondents)    // Iterate through the Respondents collection
            {
              // RespNum PollsterName Guid TimeCaptured FirstName LastName Address City StateProv PostalCode AreaCode TelNum Sex Age
              
              questNum = 1;   // This is the 1-based Question #.  In order to get the right object from the collection, 'questNum - 1' must be used.
              foreach (_Response response in respondent.Responses)       // And the Responses collection within each Respondent
              {
                string sQuestNum = GetEncodedValue(questNumLookup, response.QuestionID);   // Find out which Question_# table this response belongs to
                if (questNum.ToString() != sQuestNum)
                  Debug.Fail("Internal error determining correct Question_# table to add record to!");

                string tblName = "Question_" + questNum.ToString();
                // Idx refRespNum Guid TimeCaptured  Choice 1 Choice2 Choice2_Extra ... ChoiceN (example)
                int idx = idxValues[questNum - 1];
                idxValues[questNum - 1]++;

                // Populate the appropriate "Question_#" table
                _Question question = pollModel.Questions[questNum - 1];
                sqlText = CreateSQLInsertString(tblName, PrepareSelectedChoices(idx, respondNum, response.Guid, response.TimeCaptured, question.Choices, response, question.AnswerFormat));
                DBTools.RunSQLNonQuery(conn, sqlText);
                questNum++;
              }

              respondNum++;
            }

            // Compact Database - We're using a date comparison because we don't need to compact that often
            DateTime lastCompact = pollModel.CreationInfo.LastCompact;
            if (lastCompact == DateTime.MinValue)
              pollModel.CreationInfo.LastCompact = DateTime.Now;
            else
            {  
              TimeSpan duration = DateTime.Now - lastCompact;
              if (duration.Days > 30)
              {
                DBTools.CompactDatabase(conn.ConnectionString, destination);
                pollModel.CreationInfo.LastCompact = DateTime.Now;
              }
            }

            conn.Dispose();
          }

          catch (Exception e)
          {
            ShowMessage("Could not save file: " + destination + "\n\nError: " + e.Message, "Data Export Error");
          }

          #endif
          break;

        default:
          Debug.Fail("Unknown ExportFormat: " + format.ToString(), "Tools.SaveData");
          break;
      }
    }   // End of 'SaveData'
      


    public static string CreateSQLInsertString(string tableName, object[] paramValues)
    {
      string sqlText = "INSERT INTO " + tableName + " VALUES (";
      foreach (object obj in paramValues)
      {
        if (obj.ToString() == "")
          sqlText += "'', ";
        else
          sqlText += "'" + FixSQLString(obj.ToString()) + "', ";
      }

      sqlText = sqlText.Substring(0, sqlText.Length - 2) + ")";   // Remove trailing command and space and add a closing parenthesis
      return sqlText;
    }

    // An alternate version of the method.
    public static string CreateSQLInsertString(string tableName, ArrayList paramList)
    {
      string sqlText = "INSERT INTO " + tableName + " VALUES (";
      foreach (object obj in paramList)
      {
        if (obj.ToString() == "")
          sqlText += "'', ";
        else
          sqlText += "'" + FixSQLString(obj.ToString()) + "', ";
      }

      sqlText = sqlText.Substring(0, sqlText.Length - 2) + ")";   // Remove trailing command & space and add a closing parenthesis
      return sqlText;
    }


    public static string DetermineChoiceFields(_Choices choices, AnswerFormat answerFormat)
    {
      string fieldType = "";
      switch(answerFormat)
      {
        case AnswerFormat.Standard:
        case AnswerFormat.List:
        case AnswerFormat.DropList:
        case AnswerFormat.MultipleChoice:
          fieldType = "Char(1)";
          break;

        case AnswerFormat.Range:
        case AnswerFormat.Spinner:
          fieldType = "Char(5)";   // Generally should be 1,2,3... etc. but 5-characters should cover all cases
          break;

        case AnswerFormat.MultipleBoxes:
        case AnswerFormat.Freeform:
          fieldType = "Memo";
          break;

        default:
          Debug.Fail("Unknown AnswerFormat: " + answerFormat.ToString(), "Tools.PrepareSelectedChoices");
          break;
      }
      
      // This little loop creates the required "Choice#" and "Choice#_ExtraInfo" fields
      string cmdText = "";
      int choiceNum = 1;
      foreach (_Choice choice in choices)
      {
        cmdText += "Choice" + choiceNum.ToString() + " " + fieldType + ", ";
        if (choice.ExtraInfo)
          cmdText += "Choice" + choiceNum.ToString() + "_ExtraInfo Memo, ";

        choiceNum++;
      }

      return cmdText;
    }



    /// <summary>
    /// Determines what value(s) were selected/entered as choices and prepares an ArrayList consisting of them.
    /// This will then be used to create a SQL string that will insert a record into the appropriate "Question_#" table.
    /// The first 4 fields are fixed and are passed as the first 4 parameters here.  The remaining required fields vary in
    /// number depending on the particular circumstances of the way a Question was originally defined.
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="respondNum"></param>
    /// <param name="guid"></param>
    /// <param name="timeCaptured"></param>
    /// <param name="choices"></param>
    /// <param name="response"></param>
    /// <param name="answerFormat"></param>
    /// <returns></returns>
    public static ArrayList PrepareSelectedChoices(int idx, int respondNum, string guid, DateTime timeCaptured, _Choices choices, _Response response, AnswerFormat answerFormat)
    {
      ArrayList values = new ArrayList();
      string sAnswerIDs = "";

      try
      {
        // First add the known fields
        values.Add(idx);
        values.Add(respondNum);
        values.Add(guid);
        values.Add(timeCaptured);

        // Now populate the remaining field(s)
        switch(answerFormat)
        {
            // Note: Range is included with this first set because of the one minor difference in what value is stored.
            //       Even though Range currently can't have ExtraText values, that could change in the future.  So doing
            //       it this way means that this code won't have to be changed as a result.
          case AnswerFormat.Standard:
          case AnswerFormat.List:
          case AnswerFormat.DropList:
          case AnswerFormat.Range:
            foreach (_Choice choice in choices)
            {
              if (choice.ID.ToString() == response.AnswerID)
              {
                values.Add((answerFormat == AnswerFormat.Range) ? choice.MoreText.Trim() : "1");

                if (choice.ExtraInfo)
                  values.Add(response.ExtraText);
              }
              else
              {
                values.Add("");

                if (choice.ExtraInfo)
                  values.Add("");
              }
            }
            break;

          case AnswerFormat.MultipleChoice:
            sAnswerIDs = EnsurePrefix(EnsureSuffix(response.AnswerID, ","), ",");   // Add sentinels

            foreach (_Choice choice in choices)
            {
              string itemToFind = "," + choice.ID.ToString() + ",";
              if (sAnswerIDs.IndexOf(itemToFind) != -1)
              {
                values.Add("1");

                if (choice.ExtraInfo)
                  values.Add(response.ExtraText);
              }
              else
              {
                values.Add("");

                if (choice.ExtraInfo)
                  values.Add("");
              }
            }
            break;


          case AnswerFormat.MultipleBoxes:
            sAnswerIDs = EnsurePrefix(EnsureSuffix(response.AnswerID, ","), ",");   // Add sentinels

            foreach (_Choice choice in choices)
            {
              string itemToFind = "," + choice.ID.ToString() + ",";
              if (sAnswerIDs.IndexOf(itemToFind) != -1)
              {
                values.Add(GetExtraTextValue(response.ExtraText, choice.ID));

                if (choice.ExtraInfo)
                  values.Add(response.ExtraText);
              }
              else
              {
                values.Add("");

                if (choice.ExtraInfo)
                  values.Add("");
              }
            }
            break;

          case AnswerFormat.Freeform:
            // We can handle things this way because a Freeform Question table 
            // will always only have a single Choice field, called "Choice1".
            if (response.AnswerID == "0")
              values.Add(response.ExtraText);   // Note: Conststructing the if-else logic here is probably unnecessary but does respect the
            else                                //       way that the data model was defined.  In other words, if AnswerID != "0" then the
              values.Add("");                   //       the value of ExtraText is not be used whatsoever.
            break;

          case AnswerFormat.Spinner:
            values.Add(response.AnswerID);      // For the Spinner AnswerFormat type, the AnswerID field holds the actual selected value
            break;

          default:
            Debug.Fail("Unknown AnswerFormat: " + answerFormat.ToString(), "Tools.PrepareSelectedChoices");
            break;
        }
      }

      catch (Exception ex)
      {
        Debug.Fail("Error: " + ex.Message.ToString(), "Tools.PrepareSelectedChoices");
      }

      return values;
    }



    /// <summary>
    /// Recursive method that uses Reflection to iterate through the data model and write out current values to disk.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="level"></param>
    /// <param name="stream"></param>
    /// <param name="format"></param>
    /// <param name="propPath"></param>        // Future idea to allow saving just a portion of the hierarchical tree
    /// <param name="cumulPath"></param>       // Ditto
    /// <param name="writeToStream"></param>   // If true then data should be written to stream; if false then not
    public static void ExportClasses (NodeInfo node, int level, StreamWriter stream, ExportFormat format, string propPath, string cumulPath, bool writeToStream)
    {
      object[] indexer = new object[0];
      level++;   // We'll now be examining the children of 'node' so 'level' needs to be incremented

      if (node.IsCollection)
      {
        PPbaseCollection currColl = node.Obj as PPbaseCollection;
        foreach (object obj in currColl)
        {
          NodeInfo childNode = new NodeInfo();
          childNode.Obj = obj;
          childNode.ObjType = obj.GetType();

          // Note: The elements that populate a collection don't have names of their own.  They're just objects
          //       that are added to a collection but the object name is not retained.  So for purposes of export
          //       what we'll do is get the name of the object type and if it has a leading underscore we'll 
          //       remove it.  Thus "_Question" will become "Question" and "_Choice" will become "Choice", etc.
          SendToStream(childNode.ObjType.Name, null, XMLHeaderType.Start, level, stream, format, writeToStream);
          ExportClasses(childNode, level, stream, format, propPath, cumulPath, writeToStream);
          SendToStream(childNode.ObjType.Name, null, XMLHeaderType.End, level, stream, format, writeToStream);
        }
      }
      else
      {
        PropertyInfo[] propInfoArray = node.ObjType.GetProperties();
        foreach (PropertyInfo propInfo in propInfoArray)
        {
          // Each 'propInfo' could be:
          //   - An actual Property
          //   - A [nested] class
          //   - A collection  - Note: propInfoArray[2].PropertyType.BaseType.Name  The KEY!!!

          // With Reflection, there doesn't appear to be a "consistent" way to determine which type of data we're
          // dealing with, so we'll have to use individual tests.
          try
          {
            //if (propInfo.PropertyType.FullName.Substring(0,7) == "System.")                  // Property
            if (propInfo.PropertyType.FullName.Substring(0,7) == "System." || propInfo.PropertyType.BaseType.Name == "Enum")
            {
              ExportProperty(propInfo, node, level, stream, format, writeToStream);
            }
            
            else if (propInfo.PropertyType.BaseType.Name == "Object")                        // Instantiated Class aka Object
            {
              SendToStream(propInfo.Name, null, XMLHeaderType.Start, level, stream, format, writeToStream);

              NodeInfo childNode = new NodeInfo();
              childNode.Obj = propInfo.GetValue(node.Obj, indexer);
              childNode.ObjType = propInfo.PropertyType;
              ExportClasses(childNode, level, stream, format, propPath, cumulPath, writeToStream);

              SendToStream(propInfo.Name, null, XMLHeaderType.End, level, stream, format, writeToStream);
            }
            
            else if (propInfo.PropertyType.BaseType.BaseType.Name == "CollectionBase")       // Collection
            {
              SendToStream(propInfo.Name, null, XMLHeaderType.Start, level, stream, format, writeToStream);

              NodeInfo childNode = new NodeInfo();
              childNode.IsCollection = true;
              childNode.Obj = propInfo.GetValue(node.Obj, indexer);
              childNode.ObjType = propInfo.PropertyType;
              ExportClasses(childNode, level, stream, format, propPath, cumulPath, writeToStream);

              SendToStream(propInfo.Name, null, XMLHeaderType.End, level, stream, format, writeToStream);
            }
            
            else
              Debug.Fail("Unanticipated node type encountered", "Tools.ExportClasses");
          }
          catch
          {
            Debug.Fail("Error deciphering node type", "Tools.ExportClasses");
          }
        }
      }
    }     // End of "ExportClasses"



    /// <summary>
    /// Writes a property and its current value out to the XML file.
    /// Note: If a property is tagged with the [XmlIgnore] attribute then the property will NOT be written out.
    /// </summary>
    /// <param name="propInfo"></param>
    /// <param name="node"></param>
    /// <param name="level"></param>
    /// <param name="stream"></param>
    /// <param name="format"></param>
    /// <param name="writeToStream"></param>
    public static void ExportProperty (PropertyInfo propInfo, NodeInfo node, int level, StreamWriter stream, ExportFormat format, bool writeToStream)
    {
      object[] indexer = new object[0];

      // Check whether the property is tagged with the [XMLIgnore] attribute
      object[] attributes = propInfo.GetCustomAttributes(typeof(XmlIgnoreAttribute), true);
      if (attributes.Length == 0)
      {
        if (propInfo.PropertyType.FullName == "System.Drawing.Image")
          SendToStream(propInfo.PropertyType.Name, "", XMLHeaderType.Both, level, stream, format, writeToStream);
        else
          SendToStream(propInfo.Name, propInfo.GetValue(node.Obj, indexer), XMLHeaderType.Both, level, stream, format, writeToStream);
      }
    }


    public static void SendToStream (string name, object val, XMLHeaderType xmlHeaderType, int level, StreamWriter stream, ExportFormat format, bool writeToStream)
    {
      // This test is being added to deinflate the size of the .PP file.  
      // It will prevent blank/empty properties from being needlessly saved.
      if (val != null && (val.GetType().ToString() == "System.String") && (val.ToString() == ""))
        return;

      if (writeToStream)
      {
        if (name.Substring(0,1) == "_")      // Remove any leading
          name = name.Substring(1);          // underscore

        switch (format)
        {
          case ExportFormat.XML:
            string text = "";
            string indent = "";
            indent = indent.PadLeft(level * 4, ' ');
            
            switch (xmlHeaderType)
            {
              case XMLHeaderType.Start:
                text = "<" + name + ">";
                break;

              case XMLHeaderType.Both:
                text = "<" + name + ">";
            
                if (val != null)
                {
                  string sValue = "";

                  switch (val.GetType().ToString())
                  {
                    case "System.DateTime":
                      sValue = ((DateTime) val).ToString("s");    // Store date in ISO 8601 format
                      break;

                    case "System.String":
                      sValue = val.ToString();
                      sValue = sValue.Replace("&", "&amp;");      // Necessary because ampersands aren't allowed as standalone XML data
                      sValue = sValue.Replace("<", "&lt;");       // Necessary because less-than signs aren't allowed as standalone XML data
                      break;

                    default:
                      sValue = val.ToString();
                      break;
                  }

                  text = text + sValue;
                }

                text = text + "</" + name + ">";
                break;

              case XMLHeaderType.End:
                text = "</" + name + ">";
                break;

              default:
                Debug.Fail("Unknown xmlHeaderType: " + xmlHeaderType.ToString(), "Tools.SendToStream");
                break;
            }

            if (writeToStream)
              stream.WriteLine(indent + text);

            break;


          case ExportFormat.MDB:
            // Do nothing because this isn't a serialization type file???
            break;


          default:
            Debug.Fail("Unknown ExportFormat: " + format.ToString(), "Tools.SendToStream");
            break;
        }
      }
    }


#if(!CF)
    /// <summary>
    /// Displays a SaveFileDialog box, allowing the user to specify a path & filename.
    /// </summary>
    /// <param name="initFilename"></param>
    /// <param name="confirm"></param>
    /// <returns></returns>
    public static string SelectExportFilename(string initFilename, out bool confirm)
    {
      // Display a SaveFileDialog so the user can specify an Export filename
      SaveFileDialog fileDialog = new SaveFileDialog();
      fileDialog.DefaultExt = "*.mdb";
      fileDialog.FileName = initFilename;
      fileDialog.Filter = "MS Access Files (*.mdb)|*.mdb";
      fileDialog.InitialDirectory = GetPath(initFilename);
      fileDialog.ShowHelp = true;
      fileDialog.Title = "Specify Export Filename";

      if (fileDialog.ShowDialog() == DialogResult.OK)
        confirm = true;
      else
        confirm = false;

      return fileDialog.FileName;
    }
#endif


    /// <summary>
    /// Checks whether the auto-export feature in a poll is activated and if so, exports the data to the designated MDB file.
    /// </summary>
    /// <param name="poll"></param>
    public static void ExportIfRequired(Poll poll)
    {
      if (poll.CreationInfo.ExportEnabled)
      {
        if (poll.CreationInfo.ExportFilename == "")
          poll.CreationInfo.ExportEnabled = false;   // No point in export being enabled if no filename is specified
        else
          SaveData(poll.CreationInfo.ExportFilename, poll, "", ExportFormat.MDB);
      }
    }


    /// <summary>
    /// This is a necessary but rarely used method that is called in those circumstances when a poll is downloaded but no master copy yet
    /// exists.  This would happen if a poll is created on one Windows computer but then downloaded on another that doesn't yet have it.
    /// </summary>
    /// <param name="filename"></param>
    public static void ExportBrandNewPoll(string filename)
    {
      Poll poll = new Poll();
      OpenData(filename, poll);
      ExportIfRequired(poll);
    }

    #endregion


    #region Importing

    /// <summary>
    /// Currently to be used by SysInfo as we don't have any immediate plans to wire it up with ASM.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="model"></param>
    public static void OpenData (string filename, object model)
    {
      OpenData(filename, model, null);
    }

    /// <summary>
    /// Opens an XML file and uses the data therein to populate the data model.
    /// If 'parentEventHandler' is non-null then uses it to wire up certain events.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="model"></param>
    /// <param name="parentEventHandler"></param>
    public static void OpenData (string filename, object model, ASMEventHandler parentEventHandler)
    {
      try
      {
        // Check if the file exists
        if (! File.Exists(filename))
          return;

        // Create a DataSet object
        DataSet dataSet = new DataSet();

        // Use the ReadXml method to read in the file
        dataSet.ReadXml(filename);

        // Prepare node for the root of the data model.  We'll use 'NodeInfo', though not all of its properties.
        NodeInfo rootNode = new NodeInfo();
        rootNode.Obj = model;
        rootNode.ObjType = model.GetType();
        rootNode.PropName = model.GetType().Name;

        // *** Important Note ***
        // While it would be best if we could keep this code all generic, unfortunately one can't [generically] add items to
        // PPbaseCollection so we're going to get very specific here, building the collections structure before we call 'PopulateObject'.
        // Think of it like the necessity of constructing a building before any tenants can move in and populate the building.
        // Hopefully one day I'll figure out how to creat this structure on-the-fly in PopulateObject, as required.
        switch(rootNode.PropName)
        {
          case "Poll":
            PreparePollObject(ref model, dataSet, parentEventHandler);
            break;

          case "SysInfo":
            #if (! CF)
            PrepareSysInfoObject(ref model, dataSet);
            #endif
            break;

          case "CFSysInfo":
            PrepareCFSysInfoObject(ref model, dataSet);  // Used by both Desktop and Pocket PC apps
            break;

          default:
            Debug.Fail("Unaccounted for data type to open", "Tools.OpenData");
            break;
        }

        // We will now initiate a recursive exploration through the hierarchical tree of the data model.
        // It will load all available data in the DataSet into the appropriate properties.
        PopulateObject(rootNode, 0, dataSet);

        // Reset module level variables in anticipation of next time we use this method
        dataRowIdx = -1;                 
        currDataRelation = "";    
      }

      catch (Exception ex)
      {
        ShowMessage("Couldn't open poll: " + filename + "\n\nAdditional info: " + ex.Message, "Error Opening File");
      }
    }



    /// <summary>
    /// A Poll specific method that builds the required data structure that'll later be populated by 'PopulateObject'.
    /// *** This will need to be expanded once we're utilizing any other collections, such as 'UserPrivileges'. ***
    /// Note: Hopefully one day this will be replaced by a more generic method or maybe code within 'PopulateObject' that'll do the same thing.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="dataSet"></param>
    /// <param name="parentEventHandler"></param>
    private static void PreparePollObject(ref object pollObj, DataSet dataSet, ASMEventHandler parentEventHandler)
    {
      Poll model = (Poll) pollObj;

      // Create an empty shell of 'Questions' & 'Choices' into which "PopulateObject" can place the data it finds in the dataset.
      if (dataSet.Tables.Contains("Question"))  // This test should technically never fail but it adds an extra precaution
      {
        for (int i = 0; i < dataSet.Tables["Question"].Rows.Count; i++)
        {
          // Add a question to the model
          _Question quest = new _Question(model.Questions);
          model.Questions.Add(quest);

          // Wire up event in Question object with handler passed down from Controller
          quest.ModelEvent += parentEventHandler;

          if (dataSet.Tables.Contains("Choices"))  // This test should technically never fail but it adds an extra precaution
          {
            // Now see how many choices need to be added to this question
            int questID = (int) dataSet.Tables["Question"].Rows[i]["Question_Id"];

            DataRow[] choicesRows = dataSet.Tables["Choices"].Select("Question_Id = " + questID.ToString());

            if (choicesRows.Length > 1)
              Debug.Fail(choicesRows.Length.ToString() + "rows found in 'choicesRows', but there should only be one!", "Controller.OpenData");

            int choicesID = (int) choicesRows[0]["Choices_Id"];

            if (dataSet.Tables.Contains("Choice"))  // This test should technically never fail but it adds an extra precaution
            {
              DataRow[] choiceRows = dataSet.Tables["Choice"].Select("Choices_Id = " + choicesID.ToString());

              // We now know how many choices this question has, so add them.
              for (int j = 0; j < choiceRows.Length; j++)
              {
                _Choice choice = new _Choice(quest.Choices);
                quest.Choices.Add(choice);
              }
            }
          }
        }
      }


      // Create an empty shell of 'Respondents' & 'Responses' into which "PopulateObject" can place the data it finds in the dataset.
      if (dataSet.Tables.Contains("Respondent"))  // This test should technically never fail but it adds an extra precaution
      {
        for (int i = 0; i < dataSet.Tables["Respondent"].Rows.Count; i++)
        {
          // Add a respondent to the model
          _Respondent respondent = new _Respondent(model.Respondents);
          model.Respondents.Add(respondent);

          // Wire up event in Respondent object with handler passed down from Controller
          respondent.ModelEvent += parentEventHandler;

          if (dataSet.Tables.Contains("Responses"))  // This test should technically never fail but it adds an extra precaution
          {
            // Now see how many responses need to be added to this respondent
            int respondentID = (int) dataSet.Tables["Respondent"].Rows[i]["Respondent_Id"];

            DataRow[] responsesRows = dataSet.Tables["Responses"].Select("Respondent_Id = " + respondentID.ToString());

            if (responsesRows.Length > 1)
              Debug.Fail(responsesRows.Length.ToString() + "rows found in 'responsesRows', but there should only be one!", "Controller.OpenData");

            int responsesID = (int) responsesRows[0]["Responses_Id"];

            if (dataSet.Tables.Contains("Response"))  // This test should technically never fail but it adds an extra precaution
            {
              DataRow[] responseRows = dataSet.Tables["Response"].Select("Responses_Id = " + responsesID.ToString());

              // We now know how many responses this respondention has, so add them.
              for (int j = 0; j < responseRows.Length; j++)
              {
                _Response response = new _Response(respondent.Responses);
                respondent.Responses.Add(response);
              }
            }
          }
        }
      }

      // Debug Note: This commented out code was what was used before when Responses did not exist as a child of Respondents.
      //      // Create an empty shell of 'Respondents' into which "PopulateObject" can place the data it finds in the dataset.
      //      // Note: This will have to be expanded in the future when the sub-collections of Respondents are added.
      //      if (dataSet.Tables.Contains("Respondent"))  // This test should technically never fail but it adds an extra precaution
      //      {
      //        for (int i = 0; i < dataSet.Tables["Respondent"].Rows.Count; i++)
      //        {
      //          // Add a Respondent to the model
      //          _Respondent respondent = new _Respondent(model.Respondents);
      //          model.Respondents.Add(respondent);
      //
      //          // Wire up event in Respondent object with handler passed down from Controller
      //          respondent.ModelEvent += parentEventHandler;
      //        }
      //      }
      //
      //      // Create an empty shell of 'Responses' into which "PopulateObject" can place the data it finds in the dataset.
      //      // Note: This will have to be expanded in the future when the sub-collections of Responses are added.
      //      if (dataSet.Tables.Contains("Response"))  // This test should technically never fail but it adds an extra precaution
      //      {
      //        for (int i = 0; i < dataSet.Tables["Response"].Rows.Count; i++)
      //        {
      //          // Add a Response to the model
      //          _Response response = new _Response(model.Responses);
      //          model.Responses.Add(response);
      //
      //          // Wire up event in Response object with handler passed down from Controller
      //          response.ModelEvent += parentEventHandler;
      //        }
      //      }


      pollObj = model;  // Cast Poll model back to object, which will then be returned by reference
    }



#if (! CF)
    /// <summary>
    /// A SysInfo specific method that builds the required data structure that'll later be populated by 'PopulateObject'.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="dataSet"></param>
    private static void PrepareSysInfoObject(ref object sysInfoObj, DataSet dataSet)
    {
      SysInfo model = (SysInfo) sysInfoObj;

      // Create empty shells for 'Users', 'Devices', and 'TemplateSummary',
      // into which "PopulateObject" can place the data it finds in the dataset.
      
      if (dataSet.Tables.Contains("User"))
      {
        for (int i = 0; i < dataSet.Tables["User"].Rows.Count; i++)
        {
          // Add a user to the model
          _User user = new _User(model.Users);
          model.Users.Add(user);
        }
      }

      if (dataSet.Tables.Contains("Device"))
      {
        for (int i = 0; i < dataSet.Tables["Device"].Rows.Count; i++)
        {
          // Add a device to the model
          _Device device = new _Device(model.Devices);
          model.Devices.Add(device);
        }
      }

      if (dataSet.Tables.Contains("TemplateSummary"))  // Debug: Still need to test!
      {
        for (int i = 0; i < dataSet.Tables["TemplateSummary"].Rows.Count; i++)
        {
          // Add a template summary to the model
          //_TemplateSummary templateSummary = new _TemplateSummary(model.Summaries.Templates);
          _TemplateSummary templateSummary = new _TemplateSummary();
          model.Summaries.Templates.Add(templateSummary);
        }
      }

      sysInfoObj = model;
    }
#endif




    /// <summary>
    /// A CFSysInfo specific method that builds the required data structure that'll later be populated by 'PopulateObject'.
    /// Used by both the Desktop and Pocket PC apps.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="dataSet"></param>
    private static void PrepareCFSysInfoObject(ref object cfSysInfoObj, DataSet dataSet)
    {
      CFSysInfo model = (CFSysInfo) cfSysInfoObj;

      // Create an empty shell for 'PollSummary' and 'TemplateSummary', which "PopulateObject" can place the data it finds in the dataset.

      if (dataSet.Tables.Contains("ActivePollSummary"))
      {
        for (int i = 0; i < dataSet.Tables["ActivePollSummary"].Rows.Count; i++)
        {
          // Add a template summary to the model
          _ActivePollSummary activePollSummary = new _ActivePollSummary();
          model.Summaries.ActivePolls.Add(activePollSummary);
        }
      }

      if (dataSet.Tables.Contains("TemplateSummary"))
      {
        for (int i = 0; i < dataSet.Tables["TemplateSummary"].Rows.Count; i++)
        {
          // Add a template summary to the model
          _TemplateSummary templateSummary = new _TemplateSummary();
          model.Summaries.Templates.Add(templateSummary);
        }
      }

      cfSysInfoObj = model;
    }



    private static void PopulateCollection (NodeInfo node, int level, DataSet dataSet, DataRow[] dataRows)
    {
      level++;  

      try
      {
        int iColl = (node.Obj as PPbaseCollection).Count;

        for (int i = 0; i < iColl; i++)
        {
          NodeInfo childNode = new NodeInfo();
          childNode.Obj = (node.Obj as PPbaseCollection)[i];
          childNode.ObjType = childNode.Obj.GetType();
          childNode.Idx = i;
          childNode.PropName = childNode.ObjType.Name.Substring(1);
          childNode.ParentIsCollection = true;

          PopulateObject(childNode, level, dataSet, dataRows);
        }
      }

      catch (Exception e)
      {
        Debug.Fail("Logic error: " + e.Message, "Tools.PopulateCollection");
      }
    }



    /// <summary>
    /// Recursive method that uses Reflection to iterate through the data model and populate it with data from a dataset.
    /// </summary>
    /// <param name="node"></param>              // The current node we're examining
    /// <param name="level"></param>             // The hierarchical level of the node we're examining
    /// <param name="dataSet"></param>           // The dataset we're obtaining data to populate the model (specified as a node property)
    private static void PopulateObject (NodeInfo node, int level, DataSet dataSet, DataRow[] dataRows)
    {
      try
      {
        object[] indexer = new object[0];
        level++;        // Below we'll be examining the children of 'node' so 'level' needs to be incremented

        PropertyInfo[] propInfoArray = node.ObjType.GetProperties();
        int iCount = propInfoArray.Length;
      
        for (int i = 0; i < iCount; i++)
        {
          // Each 'propInfo' could be from:
          //   - An actual Property
          //   - A [nested] class
          //   - A collection (???)
          PropertyInfo propInfo = propInfoArray[i];

          // With Reflection, there doesn't appear to be a "consistent" way to determine
          // which type of data we're dealing with, so we'll have to use individual tests.

          // Note: I tried using the commented out line but couldn't make proper conversion within 'PopulateProperty'
          //       w/o explicitly hard-coding each Enum type.  That's too much trouble so for now we'll just keep
          //       using only the basic data types.
          if (propInfo.PropertyType.FullName.Substring(0,7) == "System." || propInfo.PropertyType.BaseType.Name == "Enum")
            //if (propInfo.PropertyType.FullName.Substring(0,7) == "System.")
          {
            if (propInfo.CanWrite)
            {
              PopulateProperty(propInfo, dataSet.Tables[node.PropName], node, dataRows);
            }
          }

          else if (propInfo.PropertyType.BaseType.Name == "Object")                    // Object
          {
            NodeInfo childNode = new NodeInfo();
            childNode.Obj = propInfo.GetValue(node.Obj, indexer);
            childNode.ObjType = propInfo.PropertyType;
            childNode.Idx = 0;
            childNode.PropName = propInfo.Name;

            PopulateObject(childNode, level, dataSet, dataRows);
            // Debug: Can I replace the above line with the shortened version???
          }

          else if (propInfo.PropertyType.BaseType.BaseType.Name == "CollectionBase")   // Collection
          {
            NodeInfo childNode = new NodeInfo();
            childNode.IsCollection = true;
            childNode.Obj = propInfo.GetValue(node.Obj, indexer);
            childNode.ObjType = propInfo.PropertyType;
            childNode.Idx = 0;
            childNode.PropName = propInfo.Name;

            if (dataSet.Tables[propInfo.Name] != null)
              if (dataSet.Tables[propInfo.Name].Rows.Count > 1)
              {
                string grandchildTableName = dataSet.Tables[propInfo.Name].ChildRelations[0].ChildTable.TableName;
                string dataRelation = propInfo.Name + "_" + grandchildTableName;

                if (currDataRelation == "")
                  currDataRelation = dataRelation;

                if (currDataRelation == dataRelation)
                  dataRowIdx++;
                else
                {
                  currDataRelation = dataRelation;
                  dataRowIdx = 0;
                }

                dataRows = dataSet.Tables[propInfo.Name].Rows[dataRowIdx].GetChildRows(dataRelation);
              }

            PopulateCollection(childNode, level, dataSet, dataRows);
          }

          else
            Debug.Fail("Unanticipated node type encountered: " + node.Obj.ToString(), "Tools.PopulateObject");
        }
      }

      catch (Exception e)
      {
        Debug.Fail("Error: " + e.Message, "Tools.PopulateObject");
      }
    }     // End of "PopulateObject"

    private static void PopulateObject (NodeInfo node, int level, DataSet dataSet)
    {
      PopulateObject(node, level, dataSet, null);
    }



    /// <summary>
    /// Note: This method currently only supports the basic data types and (in the future) the Image data type.  It would be great to
    ///       also support every type of Enum that is being stored in SysInfo, CFSysInfo, and Poll, but there doesn't seem to be a
    ///       "generic" way to make the conversion from the stored string to a given Enum type.
    /// </summary>
    /// <param name="propInfo"></param>
    /// <param name="dataTable"></param>
    /// <param name="node"></param>
    /// <param name="dataRows"></param>
    private static void PopulateProperty (PropertyInfo propInfo, DataTable dataTable, NodeInfo node, DataRow[] dataRows)
    {
      object newval = null;
      object[] indexer = new object[0];

      // This check is needed because sometimes certain properties are not available in the DataSet.
      // For example, properties marked with [XmlIgnore] will not be written to disk.  Also, we don't
      // want the program failing because someone altered the XML file.
      if (dataTable != null && dataTable.Columns[propInfo.Name] != null)
      {
        try
        {
          // Set property in data model
          if (propInfo.PropertyType.FullName == "System.Drawing.Image")
          {
            //Need to handle images differently - Future To Do
          }
          else
          {
            if (dataRows == null)
              newval = dataTable.Rows[node.Idx][propInfo.Name];
            else
              newval = dataRows[node.Idx][propInfo.Name];

            // If newval = "" and we're not converting to a string then we won't try setting a value because it'll just result in an exception.
            if (newval.ToString() == "" && propInfo.PropertyType.ToString() != "System.String")
              return;

            if (propInfo.PropertyType.IsEnum)
            {
              #if(CF)
                object val = OpenNETCF.EnumEx.Parse(propInfo.PropertyType, newval.ToString(), true);
              #else
                object val = Enum.Parse(propInfo.PropertyType, newval.ToString(), true);
              #endif

              propInfo.SetValue(node.Obj, val, indexer);
            } 
            else if (propInfo.PropertyType == typeof(System.DateTime))
            {
              propInfo.SetValue(node.Obj, GetDateFromString(newval.ToString()), indexer);
            }
            else
            {
              // Note: The 3rd parameter in ChangeType (ie. null) is necessary to get it to work with the CF too
              propInfo.SetValue(node.Obj, Convert.ChangeType(newval, propInfo.PropertyType, null), indexer);
            }
          }
        }

        catch (Exception e)
        {
          string errMsg = "Problem trying to write property in data model: " + e.Message;
          errMsg += "\nProperty Name: " + propInfo.Name;
          if (newval != null)
            errMsg += "\nValue: " + newval.ToString();
          Debug.Fail(errMsg, "Tools.PopulateProperty");
        }
      }
    }


    /// <summary>
    /// Converts a string representation of a date into an actual DateTime object.  Generally works with strings
    /// formatted with the ISO 8601 format but is also backwards compatible to support datetimes originally stored
    /// with the en-US culture format.
    /// </summary>
    /// <param name="sDate"></param>
    /// <returns></returns>
    public static DateTime GetDateFromString(string sDate)
    {
      DateTime date;

      try
      {
        // First do a basic check whether the date is ISO 6801 compliant
        if ((sDate.Length == 19) && (sDate.Substring(10,1) == "T"))
        {
          // Looks good, try converting
          date = DateTime.Parse(sDate, CultureInfo.InvariantCulture);
        }
        else
        {
          // Assume (hope!) it's in en-US format
          CultureInfo cultureEnUS = CultureInfo.CreateSpecificCulture("en-US");
          date = DateTime.Parse(sDate, cultureEnUS);
        }
      }
      catch
      {
        string msg = "Dates must be stored in either the universal ISO 8601 format or the English U.S. format.";
        msg += "\nThe date encountered was neither and simply could not be understood.\n\n";
        Debug.Fail("Error trying to convert string to date object: " + sDate, msg);
        date = DateTime.MinValue;  // Return default date instead
      }

      return date;
    }



    public static DataSet GetProductInfo()
    {
      #if(CF)
        string resourceName = "DataObjectsCF.ProductInfo.xml";
      #else
        string resourceName = "DataObjects.ProductInfo.xml";
      #endif

      Assembly assembly = Assembly.GetExecutingAssembly();
      Stream stream = assembly.GetManifestResourceStream(resourceName);
      XmlReader reader = new XmlTextReader(stream);
      
      DataSet dataSet = new DataSet();
      dataSet.ReadXml(reader, XmlReadMode.Auto);

      reader.Close();
      stream.Close();
  
      return dataSet;
    }
   
    
    /// <summary>
    /// This is a specialized version of 'Tools.OpenData' that simply retrieves the value
    /// of one property in a simple nested class - ex. "Poll.CreationInfo.PollSummary".
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="parentClass"></param>    // ex. "CreationInfo"
    /// <param name="propertyName"></param>   // ex. "PollSummary"
    /// <returns>The property's value, in string form</returns>
    public static string GetPropertyData(string filename, string parentClass, string propertyName)
    {
      try
      {
        // Check if the file exists
        if (! File.Exists(filename))
          return null;

        // Create a DataSet object
        DataSet dataSet = new DataSet();

        // Use the ReadXml method to read in the file
        dataSet.ReadXml(filename);

        // Retrieve the table
        DataTable table = dataSet.Tables[parentClass];

        // Each simple nested class should have exactly one row of data.  Let's double-check this.
        string propValue = "";
        if (table.Rows.Count == 1)
        {
          int idx = table.Columns[propertyName].Ordinal;
          propValue = table.Rows[0][idx].ToString();
        }

        return propValue;
      }
      catch (Exception ex)
      {
        Debug.Fail("Error retrieving property value - Exception Message: " + ex.Message, "Tools.GetPropertyData");
        return null;
      }
    }
    


    /// <summary>
    /// This is a specialized version of 'Tools.OpenData' that retrieves a variety of overview info from the specified poll.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="revision"></param>
    /// <param name="pollSummary"></param>
    /// <param name="reviewData"></param>
    /// <param name="questionCount"></param>
    /// <param name="respondentCount"></param>
    /// <param name="pollGuid"></param>
    /// <param name="lastEditGuid"></param>
    public static void GetPollOverview(string filename, out int revision, out string pollSummary, out bool reviewData, out int questionCount, out int respondentCount, out string pollGuid, out string lastEditGuid)
    {
      // Need to set these default values to satisfy requirements of the 'out' parameter.
      revision = 0;
      pollSummary = "";
      reviewData = false;
      questionCount = 0;
      respondentCount = 0;
      pollGuid = "";
      lastEditGuid = "";

      try
      {
        // Check if the file exists
        if (! File.Exists(filename))
          return;

        // Create a DataSet object
        DataSet dataSet = new DataSet();

        // Use the ReadXml method to read in the file
        dataSet.ReadXml(filename);

        // Retrieve the CreationInfo table
        DataTable table = dataSet.Tables["CreationInfo"];

        // Each simple nested class should have exactly one row of data.  Let's double-check this.
        if (table.Rows.Count == 1)
        {
          // Get Revision Number
          DataColumn column = table.Columns["Revision"];
          if (column != null)
          {
            int idx = column.Ordinal;
            revision = Convert.ToInt32(table.Rows[0][idx]);
          }

          // Get the Poll Summary
          column = table.Columns["PollSummary"];
          if (column != null)
          {
            int idx = column.Ordinal;
            pollSummary = table.Rows[0][idx].ToString();
          }

          column = table.Columns["PollGuid"];
          if (column != null)
          {
            int idx = column.Ordinal;
            pollGuid = table.Rows[0][idx].ToString();
          }

          column = table.Columns["LastEditGuid"];
          if (column != null)
          {
            int idx = column.Ordinal;
            lastEditGuid = table.Rows[0][idx].ToString();
          }
        }


        // Retrieve the PollsterPrivileges table
        table = dataSet.Tables["PollsterPrivileges"];

        // Each simple nested class should have exactly one row of data.  Let's double-check this.
        if (table.Rows.Count == 1)
        {
          // Get ReviewData property
          DataColumn column = table.Columns["ReviewData"];
          if (column != null)
          {
            int idx = column.Ordinal;
            reviewData = Convert.ToBoolean(table.Rows[0][idx]);
          }
        }


        // Retrieve the Question table
        table = dataSet.Tables["Question"];
        if (table != null)
          questionCount = table.Rows.Count;

        // Retrieve the Respondent table
        table = dataSet.Tables["Respondent"];
        if (table != null)  
          respondentCount = table.Rows.Count;   // Debug: Need to check if this is correct!
      }

      catch (Exception ex)
      {
        Debug.Fail("Error retrieving overview data from poll - Exception Message: " + ex.Message, "Tools.GetPollOverview");
        return;
      }
    }


    /// <summary>
    /// This version of the method is for Templates and doesn't return a value for Respondent Count.
    /// </summary>
    public static void GetPollOverview(string filename, out int revision, out string pollSummary, out int questionCount, out string pollGuid, out string lastEditGuid)
    {
      bool dummy1 = false;
      int dummy2 = 0;
      GetPollOverview(filename, out revision, out pollSummary, out dummy1, out questionCount, out dummy2, out pollGuid, out lastEditGuid);
    }


    /// <summary>
    /// This version of the method is for the Pocket PC and retrieves just the minimal info required.
    /// </summary>
    public static void GetPollOverview(string filename, out int revision, out string pollSummary, out bool reviewData, out int questionCount, out int respondentCount)
    {
      string dummy1 = "";
      string dummy2 = "";
      GetPollOverview(filename, out revision, out pollSummary, out reviewData, out questionCount, out respondentCount, out dummy1, out dummy2);
    }

    #endregion



    // Is passed a property path like "Questions[5].Choices[7].Text" then returns "Questions[].Choices[].Text".
    public static void RemoveIndices (ref string propPath)
    {
      string tmpPath = propPath;
      int i, j = 0;

      do
      {
        i = tmpPath.IndexOf("[", j);
        if (i != -1)
        {
          j = tmpPath.IndexOf("]", i + 1);

          if (j != -1)
            tmpPath = tmpPath.Substring(0, i + 1) + tmpPath.Substring(j);
        }
      } while (i != -1 && j < tmpPath.Length);

      propPath = tmpPath;
    }

    
    // Adds one or two indices into a property path.
    // Note: Ignores collections that are already populated.
    // Note: If only one index to be changed then pass '-1' to index2.
    public static void AddIndices (ref string propPath, int index1, int index2)
    {
      string tmpPath = propPath;
      int i = tmpPath.IndexOf("[]");

      if (i != -1)
      {
        if (index1 != -1)
        {
          tmpPath = tmpPath.Substring(0,i+1) + index1.ToString() + tmpPath.Substring(i+1);      

          i = tmpPath.IndexOf("[]");

          if (i != -1)
            if (index2 != -1)
              tmpPath = tmpPath.Substring(0,i+1) + index2.ToString() + tmpPath.Substring(i+1);
        }
      }

      propPath = tmpPath;
    }

 
    public static void AddIndices (ref string propPath, int index)
    {
      AddIndices(ref propPath, index, -1);
    }


    /// <summary>
    /// Will take a property path, either a full one or a partial one, and retrieve the index of the first collection specified therein.
    /// </summary>
    /// <param name="propPath"></param>
    /// <returns></returns>
    public static int GetIndex (string propPath)
    {
      string[] propPathList = propPath.Split(new char[] {'.'});    // ie. "Questions[3].Text" -> "Questions[3]" + "Text"
      string sIdx = propPathList[0].Substring(propPathList[0].IndexOf('['));
      sIdx = sIdx.TrimStart(new char[] {'['});
      sIdx = sIdx.TrimEnd(new char[] {']'});
      return Convert.ToInt32(sIdx);
    }


    /// <summary>
    /// Takes a fully populated property path string, removes the indices it has and uses them accordingly.
    /// 
    /// Notes
    /// -----
    /// Questions[#] - Nothing to do here because either the current question is set and we make a change on the form, or it isn't and we don't
    /// Choices[#]   - Set 'CtrlIdx' equal to the Choices' index
    /// </summary>
    /// <param name="propPath"></param>
    /// <param name="CtrlIdx"></param>
    public static void DecodePropertyPath (string propPath, ref int CtrlIdx)
    {
      // For now we're going to use a hardcoded approach involving a switch statement.  But conceptually it may be 
      // possible to use a more automated approach in the future, by using methods such as 'FindProperty' and
      // 'GetValue' on the "Currentxxxx" properties in the Controller.

      string[] propPathList = propPath.Split(new char[] {'.'});

      try
      {
        for (int i = 0; i < propPathList.Length; i++)
        {
          string txt = propPathList[i];
          int j = txt.IndexOf("[");

          if (j != -1)   // This text segment refers to a collection so we need to retrieve its index and deal with it accordingly
          {
            int idx = GetIndex(txt);
            RemoveIndices(ref txt);
          
            switch (txt)
            {
              case "Questions[]":
                // Do nothing in this method
                break;

              case "Choices[]":
                CtrlIdx = idx;
                break;

              case "Respondents[]":
                //CtrlIdx = idx;   // Experimental line: 2006-10-12

                // Do nothing in this method
                break;

              case "Responses[]":
                // Do nothing in this method
                break;

              default:
                Debug.Fail("Unknown collection '" + txt + "' encountered in property path: " + propPath, "Tools.DecodePropertyPath");
                break;
            }
          }
        }
      }

      catch (Exception ex)
      {
        Debug.Fail("Error trying to decode ASM data - Message: " + ex.Message, "Tools.DecodePropertyPath");
      }
    }



    /// <summary>
    /// Populates a property path string with the necessary indices (if it needs them).
    /// </summary>
    /// <param name="propPath"></param>
    /// <param name="currentQuestion"></param>
    /// <param name="currentChoice"></param>
    public static void EncodePropertyPath (ref string propPath, int currentQuestion, int currentChoice)
    {
      // For now we're going to use a hardcoded approach involving a switch statement.  But conceptually it may be 
      // possible to use a more automated approach in the future, by using methods such as 'FindProperty' and
      // 'GetValue' on the "Currentxxxx" properties in the Controller.
      
      // Perhaps the best approach is to introduce a method that will use a switch statement to look up which "Currentxxxx" variable
      // is associated with which collection.  So for example, "Questions[]" -> Questions ... which will look up and retrieve the
      // CurrentQuestion index value.

      string newPath = "";
      string[] propPathList = propPath.Split(new char[] {'.'});

      for (int i = 0; i < propPathList.Length; i++)
      {
        string txt = propPathList[i];
        int j = txt.IndexOf("[");

        if (j != -1)   // This text segment refers to a collection so we need to populate its index
        {
          switch (txt)
          {
            case "Questions[]":
              AddIndices(ref txt, currentQuestion);
              break;

            case "Choices[]":
              AddIndices(ref txt, currentChoice);  // Debug: Not yet sure if this is how we'll use it, but will suffice for now
              break;

            default:
              Debug.Fail("Unknown collection '" + txt + "' encountered in property path: " + propPath, "Tools.EncodePropertyPath");
              break;
          }
        }

        newPath = newPath + txt + ".";
      }

      newPath = newPath.Substring(0, newPath.Length - 1);  // Remove very last period

      if (newPath != propPath)
        propPath = newPath;
    }



    // Formats a text string according to the given formatCode.
    public static string FormatString (string txt, string formatCode)
    {
      switch (formatCode)
      {
        case "A":   // Convert string to uppercase
          txt = txt.ToUpper();
          break;

        case "a":   // Convert string to lowercase
          txt = txt.ToLower();
          break;
        
        case "0":   // Ensure that the string is a numeric representation, or return null
          if (IsWholeNumber(txt) != true)
            txt = null;
          break;

        default:    // Do nothing; leave string as is
          break;
      }

      return txt;
    }


    public static string Ensure2Digits(int val)
    {
      return string.Format("{0:0#}", val);
    }


    // Tests whether the input string is a whole number - ie. 0, 1, 2, ...
    public static bool IsWholeNumber (string txt)
    {
      if (txt == null)
        txt = "";
      
      Regex objNotWholePattern=new Regex("[^0-9]");
      return !objNotWholePattern.IsMatch(txt);
    }


    // Will return 'txt' as is if it's a number; otherwise will return a null string    
    public static string NumericOnly (string txt)
    {
      if (IsWholeNumber(txt))
        return txt;
      else
        return "";
    }


    /// <summary>
    /// Ensures that a path ends with a final backslash ("\").
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string EnsureFullPath (string path)
    {
      return EnsureSuffix(path, @"\");
    }


    public static string EnsureSuffix(string text, string suffix)
    {
      if (! text.EndsWith(suffix))
        text = text + suffix;

      return text;
    }


    public static string EnsurePrefix(string text, string prefix)
    {
      if (! text.StartsWith(prefix))
        text = prefix + text;

      return text;
    }

    public static string RepeatedChars(char oneChar, int num)
    {
      string result = "";
      return result.PadLeft(num, oneChar);
    }

    /// <summary>
    /// Remove the file extension from 'filename', which may or may not be fully qualified.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>The core filename only</returns>
    public static string StripExtension (string filename)
    {
      int p = filename.IndexOf(".");
      if (p != -1)
        filename = filename.Substring(0, p);
      
      return filename;
    }

    /// <summary>
    /// Remove the path and file extension from filename, which may or may not be fully qualified.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>The core filename only</returns>
    public static string StripPathAndExtension (string path)
    {
      path = StripPath(path);

      int p = path.IndexOf(".");
      if (p != -1)
        path = path.Substring(0, p);
      
      return path;
    }

    /// <summary>
    /// Remove the path from filename, which may or may not be fully qualified.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>The core filename (& extension) only</returns>
    public static string StripPath (string path)
    {
      if (path.IndexOf(@"\") != -1)
      {
        // Decompile path into constituent parts
        string[] components = path.Split(new char[] {'\\'});
        path = components[components.Length - 1];
      }

      return path;
    }


    /// <summary>
    /// Ensures that trailing backslash, if there is one, is removed.
    /// Mainly used for display purposes.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>The path without the last backslash</returns>
    public static string StripLastBackslash (string path)
    {
      if (path.Substring(path.Length - 1, 1) == @"\")
        path = path.Substring(0, path.Length - 1);

      return path;
    }


    /// <summary>
    /// Retrieve the path from the filename.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>The core filename (& extension) only</returns>
    public static string GetPath (string path)
    {
      if (path.IndexOf(@"\") == -1)
        return @"\";
      else
      {
        // Decompile path into constituent parts
        string[] components = path.Split(new char[] {'\\'});
        path = "";

        // Now recompile all but the last item back together
        for (int i = 0; i < components.Length - 1; i++)
        {
          path = path + components[i] + @"\";
        }
      }

      return path;
    }


    /// <summary>
    /// Used to determine an available filename when we don't want to overwrite the original file.
    /// The "new" name is arrived at by adding a tilde and a number - ex. "TestFile~2.ext"
    /// </summary>
    /// <param name="path"></param>
    /// <param name="basename"></param>  // This includes the name and an extension
    /// <returns></returns>
    public static string GetAvailFilename(string path, string basename)
    {
      string filename = basename;

      if (File.Exists(path + basename))
      {
        string newBasename = StripExtension(basename) + "~";
        string appExt = GetAppExt();
        int i = 2;
        do
        {
          filename = newBasename + i.ToString() + appExt;
          if (File.Exists(path + filename))
          {
            filename = null;  // This forces the loop to continue
            i++;
          }
        } while (filename == null);
      }

      return filename;
    }

    /// <summary>
    /// This is an alternate to the method above.  This one holds the existing files in an ArrayList.
    /// </summary>
    /// <param name="existingFiles"></param>
    /// <param name="basename"></param>         // May or may not have an extension
    /// <returns></returns>                     // Will return filename with an extension
    public static string GetAvailFilename(ArrayList existingFiles, string basename)
    {
      string appExt = GetAppExt();

      string filename = StripExtension(basename) + appExt;   // Ensure that extension is present

      if (existingFiles.Contains(filename.ToLower()))
      {
        string newBasename = StripExtension(filename) + "~";

        int i = 2;
        do
        {
          filename = newBasename + i.ToString() + appExt;
          if (existingFiles.Contains(filename.ToLower()))
          {
            filename = null;  // This forces the loop to continue
            i++;
          }
        } while (filename == null);
      }

      return filename;
    }

    
    // Returns a value of {null} if the string is actually = ""
    public static string DisallowNullString (string text)
    {
      if (text == "")
        text = null;

      return text;
    }

    
    public static string DisallowNullDate (DateTime date)
    {
      string sDate = "";

      if (date == DateTime.MinValue)
        sDate = "N/A";
      else
        sDate = date.ToShortDateString();

      return sDate;
    }


    public static string DisallowNullTime (DateTime date, bool showSeconds)
    {
      string sDate = "";

      if (date == DateTime.MinValue)
        sDate = "N/A";
      else
        if (showSeconds)
          sDate = date.ToLongTimeString();
        else
          sDate = date.ToShortTimeString();        

      return sDate;
    }


    public static string EnsureDirectoryExists (string path, bool setHidden)
    {
      try 
      {
        // Determine whether the directory exists
        if (!Directory.Exists(path)) 
        {
          // Try to create the directory
          Directory.CreateDirectory(path);
        }

        if (setHidden)
        {
          System.IO.DirectoryInfo dirInfo = new DirectoryInfo(path);
          dirInfo.Attributes = System.IO.FileAttributes.Hidden;
        }

        return path;
      } 

      catch (Exception e) 
      {
        Debug.Fail("Unable to create required directory: " + path + "Error Message: " + e.Message, "Tools.EnsureDirectoryExists");
        return "";
      } 
    }

    public static string EnsureDirectoryExists (string path)
    {
      return EnsureDirectoryExists(path, false);
    }





    public static string ProperCase (string text)
    {
      // This next line was originally used but only works in WinForms
      // return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(text);

      string[] words = text.Split(new char[] {' '});
      string newText = "";

      // Now recompile all the words back together
      for (int i = 0; i < words.Length; i++)
      {
        string word = words[i];
        word = word.Substring(0,1).ToUpper() + word.Substring(1).ToLower();
        newText = newText + word + " ";
      }

      if (! text.EndsWith(" "))
        newText = newText.Trim();

      return newText;
    }


    /// <summary>
    /// Searches 'fullString' for 'subString'
    /// </summary>
    /// <param name="fullString"></param>
    /// <param name="subString"></param>
    /// <param name="ignoreCase"></param>
    /// <returns></returns>
    public static bool ContainsSubstring(string fullString, string subString, bool ignoreCase)
    {
      bool retval = false;

      if (!fullString.StartsWith(","))
        fullString = "," + fullString;

      if (!fullString.EndsWith(","))
        fullString = fullString + ",";

      if (!subString.StartsWith(","))
        subString = "," + subString;

      if (!subString.EndsWith(","))
        subString = subString + ",";

      if (ignoreCase)
      {
        fullString = fullString.ToLower();
        subString = subString.ToLower();
      }

      if (fullString.IndexOf(subString) != -1)
        retval = true;

      return retval;
    }


    // Returns a user name like "rwerner" as "RWerner"
    public static string UserNameFormat (string text)
    {
      if (text.Length > 2)
        text = text.Substring(0,1).ToUpper() + text.Substring(1,1).ToUpper() + text.Substring(2).ToLower();

      return text;
    }


    public static string FixPanelText (string text)
    {
      string text2 = text;

      // A single ampersand is a control character so we need two in a row
      if (text.IndexOf("&") != -1)
        text2 = text.Replace("&", "&&");
 
      return text2;
    }


    public static string FixSQLString (string text)
    {
      string text2 = text;

      // A single apostrophe is not allowed in a SQL string so we need two in a row
      if (text.IndexOf("'") != -1)
        text2 = text.Replace("'", "''");
 
      return text2;
    }


    /// <summary>
    /// If 'text' is "" or just spaces then returns "N/A"; otherwise returns the original text.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string CheckForNA (string text)
    {
      if (text.Trim() == "")
        return "N/A";

      return text;
    }



#if (CF)
    //Do nothing for now

#else
      // Enables traditional Beep function
      // freq = Hertz      duration = milliseconds
      [DllImport("kernel32.dll")]
      public static extern bool Beep (int freq, int duration);
#endif


#if (CF)
    //Do nothing for now

#else
      [DllImport("user32")] 
      public static extern int BringWindowToTop(int hwnd);

      [DllImport("coredll.dll")]
      public static extern bool SetForegroundWindow(IntPtr hWnd );

      [DllImport("coredll")]
      public static extern IntPtr FindWindow(string className, string wndName);
#endif


    /// <summary>
    /// Retrieves the foreground window (of Windows).
    /// </summary>
#if (CF)
    // Do nothing for now

#else
      public class ForegroundWindow : IWin32Window
      {
        private static ForegroundWindow _window = new ForegroundWindow();
        private ForegroundWindow(){}

        public static IWin32Window Instance 
        { 
          get { return _window; } 
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        IntPtr IWin32Window.Handle
        {
          get 
          { 
            return GetForegroundWindow();
          }
        }
      }
#endif


    #region ConvertShortToLongPathName

    #if (!CF)
    // This code comes from:
    // http://www.c-sharpcorner.com/UploadFile/crajesh1981/RajeshPage103142006044841AM/RajeshPage1.aspx?ArticleID=63e02c1f-761f-44ab-90dd-8d2348b8c6d2

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int GetLongPathName
    (
      [MarshalAs(UnmanagedType.LPTStr)]
      string path,

      [MarshalAs(UnmanagedType.LPTStr)]
      StringBuilder longPath,

      int longPathLength
    );


    /// <summary>
    /// Retrieves the full, non-abbreviated filename, also known as the LongPathName.
    /// Note: The file must actually exist for this method to work.
    /// </summary>
    /// <param name="sShortFileName"></param>
    /// <returns></returns>
    public static string GetLongName(string sShortFileName)
    {
      StringBuilder longPath = new StringBuilder(255);
      GetLongPathName(sShortFileName, longPath, longPath.Capacity);

      return longPath.ToString();
    }

    #endif

    #endregion



    public static DialogResult ShowMessage (string msg, string caption)
    {
#if (CF)
      DialogResult result = MessageBox.Show(msg, caption);
#else
      DialogResult result = MessageBox.Show(ForegroundWindow.Instance, msg, caption);
#endif
      return result;
    }

    public static DialogResult ShowMessage (string msg, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
    {
#if (CF)
      DialogResult result = MessageBox.Show(msg, caption, buttons, icon, defaultButton);
#else
      DialogResult result = MessageBox.Show(ForegroundWindow.Instance, msg, caption, buttons, icon, defaultButton);
#endif
      return result;
    }

    public static DialogResult ShowMessage (string msg, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
      DialogResult result = ShowMessage(msg, caption, buttons, icon, MessageBoxDefaultButton.Button1);
      return result;
    }




    /// <summary>
    /// Compares two 4-part numeric strings that represent version numbers - Ex. "1.15.2057.1874"
    /// </summary>
    /// <param name="ver1"></param>
    /// <param name="ver2"></param>
    /// <returns></returns> 1: ver1 > ver2       0: ver1 = ver2       -1: ver1 < ver2
    public static int CompareVersionNumbers (string ver1, string ver2)
    {
      try
      {
        if (ver1 == null)
          ver1 = "";

        if (ver2 == null)
          ver2 = "";

        if (ver1 == ver2)
          return 0;

        // First check whether either ver1 or ver2 are blank
        if (ver1 == "" && ver2 == "")
          return 0;

        else if (ver1 == "")
          return -1;

        else if (ver2 == "")
          return 1;

        // None of the cases above so proceed with comparison
        string[] ver1List = ver1.Split(new char[] {'.'});
        string[] ver2List = ver2.Split(new char[] {'.'});

        for (int i = 0; i < ver1List.Length; i++)
        {
          int item1 = Convert.ToInt32(ver1List[i]);
          int item2 = Convert.ToInt32(ver2List[i]);

          if (item1 < item2)
            return -1;
          else if (item1 > item2)
            return 1;

          // If the two items are identical then iterate to next pair in the sequence
        }
      }

      catch (Exception e)
      {
        string msg = "Error comparing two numbers: " + ver1 + " , " + ver2;
        msg += "\n\n" + e.Message;
        Debug.Fail(msg, "Tools.CompareVersionNumbers");
      }

      return 0;
    }


    public static bool HandleTempDirectory (string tmpPath, bool initialize)
    {
      // Note: 'tmpPath' is essentially = {Temp Directory} + AppFilename

      if (initialize)
      {
        if (! Directory.Exists(tmpPath))
        {
          try
          {
            Directory.CreateDirectory(tmpPath);
          }
          catch
          {
            Tools.ShowMessage("Can't create temporary directory: " + tmpPath, "Directory Creation Error");
            return false;
          }
        }
        else  // Directory already exists so try cleaning up any lingering files
        {
          string[] fileList = Directory.GetFiles(tmpPath);
          if (fileList.Length != 0)            
          {
            for (int i = 0; i < fileList.Length; i++)
            {
              try
              {
                File.Delete(fileList[i]);
              }
              catch (Exception e)
              {
                // Not being able to delete a temporary file is not necessarily an error.  Sometimes files are just locked until the computer is reset.
                Debug.WriteLine("Warning message: " + e.Message, "Can't delete '" + fileList[i] + "'");
                return false;
              }
            }
          }
        }
      }
      
      else   // Shutting down so try to get rid of Temp directory and its contents
      {
        if (Directory.Exists(tmpPath))
        {
          // Try to remove all files
          string[] fileList = Directory.GetFiles(tmpPath);
          if (fileList.Length != 0)            
          {
            for (int i = 0; i < fileList.Length; i++)
            {
              try
              {
                File.Delete(fileList[i]);
              }
              catch (Exception e)
              {
                Debug.WriteLine("Error message: " + e.Message, "Error deleting '" + fileList[i] + "'");
                return false;
              }
            }
          }

          // And remove footprint of PP Temp directory too
          if (Directory.GetFiles(tmpPath).Length == 0)     
          {
            try
            {
              Directory.Delete(tmpPath);
            }
            catch
            {
              Tools.ShowMessage("Can't remove temporary directory: " + tmpPath, "Directory Deletion Error");
              return false;
            }
          }
        }
      }
    
      return true;
    }


    /// <summary>
    /// Tries to delete the specified file.
    /// </summary>
    /// <param name="fullPath"></param>
    /// <returns></returns>  true - file was deleted    false - file couldn't be deleted
    public static bool DeleteFile(string fullPath)
    {
      if (File.Exists(fullPath))
      {
        try
        {
          File.Delete(fullPath);
        }
        catch
        {
          Debug.WriteLine("Couldn't delete: " + fullPath);
          return false;
        }
      }

      return true;
    }

    public static bool DeleteFile(string fileDir, string fileName)
    {
      return DeleteFile(fileDir + fileName);
    }



    public static void DebugPause()
    {
#if (DEBUG)
#if (CF)
      MessageBox.Show("Temporarily paused for debugging purposes", CFSysInfo.Data.MobileAdmin.AppName);
#else
          MessageBox.Show("Temporarily paused for debugging purposes", SysInfo.Data.Admin.AppName);
#endif
#endif
    }



    /// <summary>
    /// Compares two files to see if they're identical.
    /// Note: This has been tested only with text files so far but should work with binary files too.
    /// </summary>
    /// <param name="file1"></param>
    /// <param name="file2"></param>
    /// <returns>true - files are identical; false - they're not identical</returns>
    public static bool CompareFiles(string file1, string file2)
    {
      bool noMatch = false;
      
      // Check filesizes first
      FileInfo fileInfo1 = new FileInfo(file1);
      FileInfo fileInfo2 = new FileInfo(file2);

      if (fileInfo1.Length == fileInfo2.Length)
      {
        // Check actual file contents, character by character.
        FileStream fileStream1 = fileInfo1.OpenRead();
        FileStream fileStream2 = fileInfo2.OpenRead();
        bool eofReached = false;

        do
        {
          int val1 = fileStream1.ReadByte();
          int val2 = fileStream2.ReadByte();
          
          if (val1 == -1 || val2 == -1)
            eofReached = true;
          else if (val1 != val2)
            noMatch = true;

        } while (!noMatch && !eofReached);

        fileStream1.Close();
        fileStream2.Close();
      }
      else
        return false;

      if (noMatch)
        return false;

      return true;
    }



    /// <summary>
    /// Retrieves the original filename from the full Archive Filename Format
    ///   ex. "OriginalFilename_PollsterName_FinishPollingDate_#.pp" >>> "OriginalFilename.pp"
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetOriginalFilename(string fileName)
    {
      // We need to back out the name by stripping away from right to left
      // because the OriginalFilename could have one or more underscores in it.
      fileName = StripPathAndExtension(fileName);
      string appExt = GetAppExt();

      for (int i = 0; i < 3; i++)
      {
        int pos = fileName.LastIndexOf("_");
        fileName = fileName.Substring(0, pos) + appExt;
      }

      return fileName;
    }


    /// <summary>
    /// Retrieves the value of AppExtension (in SysInfo or CFSysInfo) - ex. ".pp"
    /// </summary>
    /// <returns></returns>
    public static string GetAppExt()
    {
      string appExt = "";

#if (CF)
      appExt = CFSysInfo.Data.MobileAdmin.AppExtension;
#else
        appExt = SysInfo.Data.Admin.AppExtension;
#endif

      return appExt;
    }

    /// <summary>
    /// Retrieves the AppExtension with two available formatting options:
    ///  - Keep or remove the leading dot
    ///  - Capitalize or make lower-case
    /// Examples:
    ///    noDot = true  : "pp"
    ///    noDot = false : ".pp"
    /// </summary>
    /// <param name="noDot"></param>
    /// <param name="capitalize"></param>
    /// <returns></returns>
    public static string GetAppExt(bool noDot, bool capitalize)
    {
      string appExt = GetAppExt();

      appExt = (noDot) ? appExt.Substring(1) : appExt;
      appExt = (capitalize) ? appExt.ToUpper() : appExt.ToLower();

      return appExt;
    }


    /// <summary>
    /// Retrieves a string that serves as a file filter - ex. "*.pp"
    /// </summary>
    /// <returns></returns>
    public static string GetAppFilter()
    {
      return "*" + GetAppExt();
    }


    public static string PrepareCorrectTense(int quantity, string noun, string verbSingular, string verbPlural, bool noFlag)
    {
      string phrase;

      verbSingular = (verbSingular == null) ? "" : verbSingular;
      verbPlural = (verbPlural == null) ? "" : verbPlural;

      if (quantity == 0)
        phrase = ((noFlag == true) ? "No" : "0") + " " + noun + "s " + verbPlural;
      else if (quantity == 1)
        phrase = "1 " + noun + " " + verbSingular;
      else
        phrase = quantity.ToString() + " " + noun + "s " + verbPlural;

      return phrase.Trim();
    }

    public static string PrepareCorrectTense(int quantity, string noun, string verbSingular, string verbPlural)
    {
      return PrepareCorrectTense(quantity, noun, verbSingular, verbPlural, false);
    }

    public static string PrepareCorrectTense(int quantity, string noun)
    {
      return PrepareCorrectTense(quantity, noun, "", "", false);
    }



#if (!CF)
    /// <summary>
    /// Appends the Respondents (and Responses) data of the newPoll into the masterPoll.
    /// </summary>
    /// <param name="masterFilename"></param>
    /// <param name="newdataFilename"></param>
    /// <param name="activePoll"></param>
    /// <returns>true - New data was imported      false - Nothing was imported; generally a duplicate or incompatible new data file</returns>
    public static bool AppendPoll(string masterFilename, string newdataFilename, Poll activePoll)
    {
      bool isDirty = false;    // If set true then file will be resaved with new data

      Poll newPoll = new Poll();
      OpenData(newdataFilename, newPoll);

      Poll masterPoll = new Poll();
      OpenData(masterFilename, masterPoll);

      // We'll do some initial validation checks to try to ensure that newPoll's data belongs in masterPoll.
      if (newPoll.CreationInfo.PollGuid != masterPoll.CreationInfo.PollGuid)
      {
        string msg = "The newly imported data file - '" + StripPathAndExtension(newdataFilename) + 
          "' - does not appear to have\nthe same structure " +
          "as the master version with this same name.\n\nSo the captured data will not be imported.";
        ShowMessage(msg, "Incompatible Downloaded Poll");
        return false;
      }

      if (newPoll.CreationInfo.LastEditGuid != masterPoll.CreationInfo.LastEditGuid)
      {
        // Note: It's correct to use 'masterFilename' here because 'newdataFilename' contains the more cryptic Archive syntax.
        string msg = "The structure of the newly imported data file - '" + StripPathAndExtension(masterFilename) + 
          "' - is not\nthe same version as the master copy with this same name.\n\n" +
          "It is generally recommended that the captured data is not\n" +
          "imported but you can override this if you want.\n\n" +
          "Would you like to import the data from this new file?";
      
        DialogResult retval = MessageBox.Show(msg, "Downloaded Poll Is Different Version", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        if (retval == DialogResult.No)
          return false;
      }

      foreach (_Respondent respondent in newPoll.Respondents)
      {
        // Check for duplicate Guids; if found then check for duplicate TimeCaptured; if duplicate too then quietly reject record
        bool okayToAdd = false;
        
        string guid = respondent.Guid;
        _Respondent existingRespondent = masterPoll.Respondents.FindGuid(guid);

        // 99.9999999999999% of the time (or thereabouts!) no respondent will be found, unless a file is somehow imported twice.
        if (existingRespondent == null)
          okayToAdd = true;
        else
        {
          // Whichever the case, we will now also check TimeCaptured to establish uniqueness or duplication.
          if (existingRespondent.TimeCaptured != respondent.TimeCaptured)
            okayToAdd = true;
          else
            Debug.WriteLine("Warning: Duplicate record found - Not necessarily an error", "Tools.AppendPoll");
        }   

        if (okayToAdd)
        {
          isDirty = true;

          // Debug: The problem with this line and similar one below is that ID and Index are not getting updated to
          //        reflect their presence in the new poll.
          respondent.ID = -1;   // Experimental line to resolve problem above
          masterPoll.Respondents.Add(respondent);

          // If this same poll is currently open then import data into its object model too
          if (activePoll != null)
            activePoll.Respondents.Add(respondent);
        }
      }


      // Now examine properties in PollingInfo section and adjust where required
      if (masterPoll.PollingInfo.PollsterName != SysInfo.Data.Options.PrimaryUser)
        masterPoll.PollingInfo.PollsterName = SysInfo.Data.Options.PrimaryUser;

      if (masterPoll.PollingInfo.ProductID != SysInfo.Data.Admin.ProductID)
        masterPoll.PollingInfo.ProductID = SysInfo.Data.Admin.ProductID;

      if (masterPoll.PollingInfo.VersionNumber != SysInfo.Data.Admin.VersionNumber)
        masterPoll.PollingInfo.VersionNumber = SysInfo.Data.Admin.VersionNumber;

      if (masterPoll.PollingInfo.Guid != SysInfo.Data.Admin.Guid)
        masterPoll.PollingInfo.Guid = SysInfo.Data.Admin.Guid;

      if (masterPoll.PollingInfo.StartPolling == DateTime.MinValue || masterPoll.PollingInfo.StartPolling > newPoll.PollingInfo.StartPolling)
        masterPoll.PollingInfo.StartPolling = newPoll.PollingInfo.StartPolling;

      if (masterPoll.PollingInfo.FinishPolling == DateTime.MinValue || masterPoll.PollingInfo.FinishPolling < newPoll.PollingInfo.FinishPolling)
        masterPoll.PollingInfo.FinishPolling = newPoll.PollingInfo.FinishPolling;


      // If any changes were made then save the masterPoll back to disk
      if (isDirty)
      {
        // The following construct handles the export of the pollModel to an MDB file, if one is defined and exporting is enabled.
        
        // Note: Must first save to the MDB because if it is compacted then it writes a
        // value to CreationInfo.LastCompact and thus the PP file must be saved second.
        ExportIfRequired(masterPoll);

        // Now save data to the master file (*.pp file)
        SaveData(masterFilename, masterPoll, null, ExportFormat.XML);
        return true;
      }

      return false;
    }

    public static bool AppendPoll(string masterFilename, string newdataFilename)
    {
      return AppendPoll(masterFilename, newdataFilename, null);
    }
#endif    



    public static string GetLastEditGuid(string fullPath)
    {
      Poll poll = new Poll();
      OpenData(fullPath, poll);

      return poll.CreationInfo.LastEditGuid;
    }


    /// <summary>
    /// Ensures that the specified Archive subfolder exists
    /// </summary>
    /// <param name="archivePath"></param>  // The location of 'Data\Archive\' (incl. trailing backslash)
    /// <param name="baseName"></param>     // The name of the subfolder within archive; essentially the filename w/o the extension
    /// <returns></returns>
    public static string GetSpecificArchivePath(string archivePath, string baseName)
    {
      string specificArchivePath = archivePath + baseName;
      
      if (! Directory.Exists(specificArchivePath))
        Directory.CreateDirectory(specificArchivePath);
      
      return specificArchivePath + @"\";
    }



    #region TextSize Calculations

    // Used for getting width of text, as it would appear in a label.
    public static int GetLabelWidth(string text, Font font)
    {
      return GetLabelSize(text, font).Width;
    }


    public static int GetMaxLabelWidth(string[] textList, Font font)
    {
      int maxLen = 0;

      for (int i = 0; i < textList.Length; i++)
      {
        maxLen = Math.Max(maxLen, GetLabelWidth(textList[i], font));
      }

      return maxLen;
    }
 

    // Gets size of text, as it would appear in a label
    public static Size GetLabelSize(string text, Font font)
    {
      // Necessary to access 'CreateGraphics' below
      Form form1 = new Form();  

      // Create a Graphics object for the Control
      Graphics g = form1.CreateGraphics();

      // Get the Size needed to accommodate the formatted Text
      Size size = g.MeasureString(text, font).ToSize();

      return size;
    }


    public static int GetLabelHeight(string text, Font font, int maxWid)
    {
      if (text == "")
        return 0;

#if (! CF)
      // This is a more sophisticated text bounds calculation method, that takes into account
      // strings that wrap prematurely because of longer than normal width words.
      Form form1 = new Form();                // Necessary to access 'CreateGraphics' below
      Graphics g = form1.CreateGraphics();    // Create a Graphics object for the Control

      System.Drawing.StringFormat format = new System.Drawing.StringFormat ();
      System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0, 0, maxWid, 10000);
      System.Drawing.CharacterRange[] ranges = {new System.Drawing.CharacterRange(0, text.Length)};
      System.Drawing.Region[] regions = new System.Drawing.Region[1];

      format.SetMeasurableCharacterRanges(ranges);

      regions = g.MeasureCharacterRanges (text, font, rect, format);
      rect = regions[0].GetBounds(g);
      g.Dispose();
      form1.Dispose();

      return (int)(rect.Bottom + 5.0F);  // The 5.0 seems necessary so descenders don't get cut off
      
#else
      //      int width2 = Tools.GetLabelWidth(text, font);
      //
      //      // Note: This algorithm is flawed but unfortunately there appears to be no precise way to calculate the number of
      //      //       required lines.  It varies depending on the length and positioning of the words involved.  As soon as a 
      //      //       better method is available then it'll be adopted here.
      //      int numlines = (int) (width2 / maxWid) + 1;          // ex. 2.7 -> 3  
      //      int height = 1 + (int) (font.Size * 2 * numlines) + 8;   // Debug: The added factor at the end is to compensate for the rough algorithm

      int height = ToolsCF.CalcTextHeight(text, font, maxWid);

      return height;
#endif
    }

    #endregion
    

    /// <summary>
    /// Takes a single-line string and formats it into a multiline string so that it'll fit into tight spaces.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="maxLines"></param>
    /// <param name="maxWidth"></param>
    /// <param name="font"></param>
    /// <returns></returns>
    public static string CreateMultilineString(string text, int maxLines, int maxWidth, Font font)
    {
      int len = text.Length;
      string newString = "";
      int lineNum = 1;
      int p1 = 0;
      int p2 = 0;

      try
      {
        do
        {
          string subText = text.Substring(p1, p2 - p1 + 1);
          int wid = GetLabelWidth(subText, font);

          if (wid > maxWidth)
          {
            if (text.Substring(p2, 1) == " ")            // Is the current character a space?
            {
              newString += text.Substring(p1, p2 - p1 + 1);
              p1 = p2 + 1;
            }

            else if (p2 + 1 < len && text.Substring(p2 + 1, 1) == " ")   // Is the next character a space?
            {
              newString += text.Substring(p1, p2 - p1 + 1);
              p1 = p2 + 2;
            }

            else if (text.Substring(p2 - 1, 1) == " ")   // Is the previous character a space?
            {
              newString += text.Substring(p1, p2 - p1 - 1);
              p1 = p2;
            }

            else if (p2 + 2 < len && text.Substring(p2 + 2, 1) == " ")   // Is the character that is two ahead a space?
            {
              newString += text.Substring(p1, p2 - p1 + 2);
              p1 = p2 + 3;
            }

            else if (text.Substring(p2 - 2, 1) == " ")   // What about the one that is two characters before?
            {
              newString += text.Substring(p1, p2 - p1 - 2);
              p1 = p2 - 1;
            }

            else
            {
              newString += text.Substring(p1, p2 - p1 + 1);
              if (p2 < len - 1)
                newString += "-";
              p1 = p2 + 1;
            }

            // Whichever clause we ended up in before, there are some commonalities to proceed forward
            newString += "\n";
            lineNum++;
            p2 = p1;
          }
          else
            p2++;

        } while (p2 < len && lineNum <= maxLines);

        if (p2 == len)
        {
          newString += text.Substring(p1, p2 - p1);
        }
        else
        {
          newString = newString.TrimEnd(new char[] {'\n'});    // Remove trailing newline character
          newString += "...";
        }
      }

      catch (Exception ex)
      {
        Debug.Fail("Error building multiline string: " + ex.Message.ToString(), "Tools.CreateMultilineString");
      }

      return newString;
    }


    public static string AbbreviateString(string text, int maxChars)
    {
      if (text.Length <= maxChars)
        return text;

      int len = 0;

      for (int i = maxChars - 1; i >= 0; i--)
      {
        if (text.Substring(i, 1) == " ")
        {
          len = i;
          break;
        }
      }

      if (len == 0)
        len = maxChars - 2;

      return text.Substring(0, len) + "...";
    }



    /// <summary>
    /// This method does several things:
    ///  - Checks whether there's an existing entry in 'existingValues' associated with 'index'
    ///  - If so then it checks whether the associated string is the same as 'newValue'
    ///       - If so then it does nothing
    ///       - If not then it encodes 'newValue' with 'index' and replaces the old encoded pair in 'existingValues'
    ///  - If not then it:
    ///       - Encodes 'newValue' with 'index' and stores this in 'existingValues'
    /// </summary>
    /// <param name="newValue"></param>
    /// <param name="index"></param>
    /// <param name="existingValues"></param>
    /// <returns></returns>
    public static void EncodeExtraTextValue(string newValue, int index, ref string existingValues)
    {
      if (existingValues == null)   // Prelim check
        existingValues = "";

      string oldValue = GetExtraTextValue(existingValues, index);

      if (oldValue == "")
      {
        existingValues = EnsureSuffix(existingValues, ",");
        existingValues += index.ToString() + "~" + newValue;
      }
      else if (oldValue != newValue)
        existingValues = existingValues.Replace(index.ToString() + "~" + oldValue, index.ToString() + "~" + newValue);
    }


    /// <summary>
    /// Decodes the ExtraText string, retrieving the value associated with "choiceID".
    /// </summary>
    /// <param name="allExtraText"></param>
    /// <param name="choiceID"></param>
    /// <returns>The 'extraText' value</returns>
    public static string GetExtraTextValue(string allExtraText, int choiceID)
    {
      string extraText = "";

      if (allExtraText != "")
      {
        string extraText2 = "," + allExtraText;        // ex. "15~abcd,2~defg" -> ",15~abcd,2~defg"  (Now searchable!)

        int pos = extraText2.IndexOf("," + choiceID.ToString() + "~");
        if (pos != -1)                                                   // ex. 0                        8
        {
          extraText2 = extraText2.Substring(pos + 1);                    // ex. "15~abcd,2~defg"         "2~defg"
          pos = extraText2.IndexOf(",");                                 // ex. 7                        -1
          if (pos != -1)
            extraText2 = extraText2.Substring(0, pos);                   // ex. "15~abcd"

          pos = extraText2.IndexOf("~");                                 // ex. 2                        1
          extraText = extraText2.Substring(pos + 1);                     // "abcd"                       "defg"
        }
      }

      return extraText;
    }

    // This is just a more generic entry point into the functionality provided by 'GetExtraTextValue'.
    // The values are encoded in a string using this format:    "index1~value1,index2~value2,...,indexN~valueN"
    public static string GetEncodedValue(string encodedValues, int index)
    {
      return GetExtraTextValue(encodedValues, index);
    }



    /// <summary>
    /// Calculates the sums and percentages of the responses of a given question for a specified set of respondents.
    /// Note: Spinner and Freeform answer formats have their own methods for tabulating the results.
    /// </summary>
    /// <param name="answerFormat"></param>
    /// <param name="respondents"></param>
    /// <param name="quest"></param>
    /// <param name="choiceSums">Simply a tally of each item chosen or entered</param>
    /// <param name="calcSums">Essentially a weighted average - Currently used by Range, Mult Choice, & Mult Textboxes</param>
    /// <param name="choicePercents">This either stores a Percentage or an Average, depending on the AnswerFormat involved</param>
    /// <param name="tallyNoneResponses">If 'true' then non-responses will also be included in the summary</param>
    public static void SummarizeAnswers(AnswerFormat answerFormat, _Respondents respondents, _Question quest, out ArrayList choiceSums, out ArrayList calcSums, out ArrayList choicePercents, bool tallyNoneResponses)
    {
      choiceSums = new ArrayList();
      calcSums = new ArrayList();
      choicePercents = new ArrayList();
      ArrayList rangeValues = new ArrayList();

      try
      {
        bool tallyCalcSums = false;

        if (answerFormat == AnswerFormat.MultipleChoice || answerFormat == AnswerFormat.Range || answerFormat == AnswerFormat.MultipleBoxes)
          tallyCalcSums = true;    // Note: This is really only used by 'Range' because it's implied for the other two

        int questionID = quest.ID;
        int choiceCount = quest.Choices.Count;

        // We need to have a way to cross-reference the AnswerID value of a response with its order within a question.
        // Note: The AnswerID in Response is identical to Choice.ID in Question.Choices.
        string indexMap = "";
        for (int i = 0; i < choiceCount; i++)
        {
          _Choice choice = quest.Choices[i];

          // Note: We may want to replace 'indexMap' with a string array in the future
          indexMap +=  choice.ID.ToString() + "~" + i.ToString() + ",";
          choiceSums.Add(0);

          if (answerFormat == AnswerFormat.Range)
          {
            calcSums.Add(0.0);

            int rangeVal = 0;
            try
            {
              rangeVal = Convert.ToInt32(choice.MoreText);
            }
            catch
            {
              // Just catch conversion error, no need to do anything else
            }
            finally
            {
              rangeValues.Add(rangeVal);
            }
          }
          else if (answerFormat == AnswerFormat.MultipleBoxes || answerFormat == AnswerFormat.MultipleChoice)
          {
            calcSums.Add(0.0);
          }
        }

        if (tallyNoneResponses)
          choiceSums.Add(0);   // We'll add an extra 'choice' that will be used to keep track of "No opinions"

        // Note: To keep track of the sums we're first going to try use an ArrayList.  
        //       If it seems too slow then we will try an encoded string like: "0~17,1~5,2~0,3~7"

        // Calculate how many answers of each type were chosen and keep track of these in 'choiceSums'.
        foreach(_Respondent respondent in respondents)
        {
          _Response response = respondent.Responses.Find_ResponseByQuestionID(questionID);

          string sAnswerID = "";

          if (response != null)
            sAnswerID = response.AnswerID;

          if (sAnswerID == "")
          {
            if (tallyNoneResponses)
              choiceSums[choiceCount] = ((int) choiceSums[choiceCount]) + 1;
          }

          else if (answerFormat == AnswerFormat.Standard || answerFormat == AnswerFormat.List || answerFormat == AnswerFormat.DropList || answerFormat == AnswerFormat.Range)
          {
            int answerID = Convert.ToInt32(sAnswerID);
            int choiceIdx = Convert.ToInt32(GetEncodedValue(indexMap, answerID));
            choiceSums[choiceIdx] = ((int) choiceSums[choiceIdx]) + 1;

            if (tallyCalcSums)
              calcSums[choiceIdx] = (double) calcSums[choiceIdx] + Convert.ToDouble(rangeValues[choiceIdx]);
          }

          else  // More than one AnswerID might exist (MultipleChoice/MultipleBoxes)
          {
            sAnswerID = EnsurePrefix(EnsureSuffix(sAnswerID, ","), ",");   // Add sentinels

            // Prior to entering the Do loop below, we need to know how many answers there are.
            // This will serve as the divisor to establish a weighting for CalcSums.
            int numAnswers = sAnswerID.Split(new char[] {','}).Length - 2;    // ie. For every set of answers, there's a null string prefix & suffix

            int p1 = 1;
            int p2;

            do
            {
              p2 = sAnswerID.IndexOf(",", p1);
              if (p2 != -1)
              {
                int answerID = Convert.ToInt32(sAnswerID.Substring(p1, p2 - p1));
                int choiceIdx = Convert.ToInt32(GetEncodedValue(indexMap, answerID));

                if (answerFormat == AnswerFormat.MultipleBoxes)
                {
                  string sUserVal = GetEncodedValue(response.ExtraText, answerID);

                  int userVal = 0;
                  try
                  {
                    userVal = Convert.ToInt32(sUserVal);  // For now, we're only going to allow the tabulation of Int values, not Float values
                  }
                  catch
                  {
                    // Just catch conversion error, no need to do anything else
                  }
                  finally
                  {
                    if (userVal != 0)
                    {
                      calcSums[choiceIdx] = ((double) calcSums[choiceIdx]) + userVal;
                      choiceSums[choiceIdx] = ((int) choiceSums[choiceIdx]) + 1;    // Only increment if there's a legitimate value
                    }
                  }
                }
                else   // Must be MultipleChoice
                {
                  calcSums[choiceIdx] = (double) calcSums[choiceIdx] + (1.0 / numAnswers);
                  choiceSums[choiceIdx] = ((int) choiceSums[choiceIdx]) + 1;
                }

                p1 = p2 + 1;
              }
            } while (p1 < sAnswerID.Length);
          }
        }

        // We'll handle the calculations differently depending on which AnswerFormat is active
        if (answerFormat == AnswerFormat.MultipleBoxes)
        {
          for (int i = 0; i < choiceSums.Count; i++)
          {
            double calcVal = Convert.ToDouble(calcSums[i]) / (int) choiceSums[i];   // This is the average, though it's stored in 'choicePercents'
  
            if (double.IsNaN(calcVal))
              choicePercents.Add("");
            else
              choicePercents.Add(calcVal);
          }
        }
        else
        {
          // Now that we have the sums, calculate the relative percentages
          int sumTotal = 0;
          for (int i = 0; i < choiceSums.Count; i++)
          {
            sumTotal += (int) choiceSums[i];
          }

          foreach (object aSum in choiceSums)
          {
            double calcVal = (Convert.ToDouble(aSum) / sumTotal * 100);        

            if (double.IsNaN(calcVal))
              choicePercents.Add("");
            else
              choicePercents.Add(calcVal);
          }
        }
      }
      catch (Exception ex)
      {
        Debug.Fail("Exception tallying answers: " + ex.Message.ToString(), "Tools.SummarizeAnswers");
      }
    }

    public static void SummarizeAnswers(AnswerFormat answerFormat, _Respondents respondents, _Question quest, out ArrayList choiceSums, out ArrayList choicePercents)
    {
      ArrayList calcSums;    // Dummy only; not passed back to the calling code
      bool tallyNoneRespondents = false;
      SummarizeAnswers(answerFormat, respondents, quest, out choiceSums, out calcSums, out choicePercents, tallyNoneRespondents);
    }

    public static void SummarizeAnswers(AnswerFormat answerFormat, _Respondents respondents, _Question quest, out ArrayList choiceSums, out ArrayList calcSums, out ArrayList choicePercents)
    {
      bool tallyNoneRespondents = false;
      SummarizeAnswers(answerFormat, respondents, quest, out choiceSums, out calcSums, out choicePercents, tallyNoneRespondents);
    }



    /// <summary>
    /// This method was created because the acquisition of spinner (aka "UpDown") data is quite different than the other controls.
    /// </summary>
    /// <param name="respondents"></param>
    /// <param name="quest"></param>
    /// <param name="choiceValues"></param>
    /// <param name="choiceSums"></param>
    /// <param name="calcSums"></param>
    /// <param name="choicePercents"></param>
    public static void SummarizeSpinnerAnswers(_Respondents respondents, _Question quest, out ArrayList choiceValues, out ArrayList choiceSums, out ArrayList calcSums, out ArrayList choicePercents)
    {
      choiceValues = new ArrayList();
      choiceSums = new ArrayList();
      calcSums = new ArrayList();
      choicePercents = new ArrayList();

      _Choice choice = quest.Choices[0];    // For a spinner, there is always only one choice
      int min = 0;
      int max = 0;

      try
      {
        min = Convert.ToInt32(choice.Text);
        max = Convert.ToInt32(choice.MoreText);
      }
      catch
      {
        return;   // Can't proceed if Min & Max aren't legit
      }

      string[] valueSums = new string[max - min + 1];

      // Calculate how many answers of each type were chosen and keep track of these in 'valueSums'.
      foreach(_Respondent respondent in respondents)
      {
        _Response response = respondent.Responses.Find_ResponseByQuestionID(quest.ID);

        string sValue = response.AnswerID;   // This is the recorded spinner value

        try
        {
          int idx = Convert.ToInt32(sValue);
          string sOldValue = valueSums[idx - min];
          int spinValue = (sOldValue == null) ? 0 : Convert.ToInt32(sOldValue);
          spinValue++;
          valueSums[idx - min] = spinValue.ToString();
        }
        catch
        {
          // Do nothing, just trap error
        }
      }

      // Now cycle through string array and add non-null items to ArrayList 'choiceSums'
      // Also populate 'choiceValues' with the indexes (+ min) of 'valueSums'; these will populate the wide column
      int sumTotal = 0;
      for (int i = 0; i < valueSums.Length; i++)
      {
        if (valueSums[i] != null)
        {
          choiceValues.Add(i + min);                         // ex. 15 - The spinner value that was picked
          int iValueSum = Convert.ToInt32(valueSums[i]);     // ex. 2  - The value above (ie. 15) was picked twice
          choiceSums.Add(iValueSum);
          calcSums.Add((i + min) * iValueSum);               // ex. 15 * 2 - The relative weighting
          sumTotal += iValueSum;
        }
      }

      foreach (object aSum in choiceSums)
      {
        double calcVal = (Convert.ToDouble(aSum) / sumTotal * 100);        
        choicePercents.Add(calcVal);
      }
    }


    /// <summary>
    /// This method was created because the acquisition of FreeForm data is quite different than the other controls.
    /// </summary>
    /// <param name="respondents"></param>
    /// <param name="quest"></param>
    /// <param name="respAnswers"></param>
    /// <param name="respFooters"></param>
    public static void SummarizeFreeFormAnswers(_Respondents respondents, _Question quest, out ArrayList respAnswers, out ArrayList respNames, out ArrayList respDates)
    {
      respAnswers = new ArrayList();
      respNames = new ArrayList();
      respDates = new ArrayList();

      foreach(_Respondent respondent in respondents)
      {
        _Response response = respondent.Responses.Find_ResponseByQuestionID(quest.ID);

        // Technically the presence of a FreeForm answer is indicated by a "0" present in this field, but this
        // logic test leaves open other possibilities in the future - ie. Different numbers would indicate 
        // different types of answers.
        if (response.AnswerID != "")
        {
          respAnswers.Add(response.ExtraText);
          respNames.Add(respondent.FirstName + " " + respondent.LastName);
          respDates.Add(response.LastModified.ToShortDateString() + " " + response.LastModified.ToShortTimeString());
        }
      }
    }


    /// <summary>
    /// Calculates the percent of each value as compared to the sum of all the values and places the percentages in 'percents'.
    /// Note: This is very similar to what 'ConvertArrayListToPercentages' does.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="percents"></param>
    public static void CalcPercents(ArrayList values, ref ArrayList percents)
    {
      double sum = 0.0;
      for (int i = 0; i < values.Count; i++)
      {
        double val = Convert.ToDouble(values[i]);
        sum += val;
      }

      for (int i = 0; i < values.Count; i++)
      {
        percents[i] = Convert.ToDouble(values[i]) / sum * 100;
      }
    }


    /// <summary>
    /// Calculates the sum of the values contained in an ArrayList.  These values are assumed to be integers.
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static int SumArrayValues(ArrayList array)
    {
      int sum = 0;

      foreach(Object obj in array)
      {
        sum += Convert.ToInt32(obj);
      }

      return sum;
    }


    /// <summary>
    /// Calculates how many of the specified kind of control exist in the specified panel.
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="ctrl"></param>
    /// <returns></returns>
    public static int CountControls(Panel panel, Control uniqueControl)
    {
      int i = 0;

      foreach (Control ctrl in panel.Controls)
      {
        if (ctrl.GetType() == uniqueControl.GetType())
          i++;
      }

      return i;
    }


    public static int CountExtraneousControls(Panel panel, Control uniqueControl)
    {
      int numCtrl = panel.Controls.Count;                     // Total number of controls in panel
      int numUnique = CountControls(panel, uniqueControl);    // The number of controls of the specified type
      return numCtrl - numUnique;
    }


    /// <summary>
    /// Examines all of the child controls in the panel and obtains the bottom edge of the lowest one.
    /// </summary>
    /// <param name="panel"></param>
    /// <returns></returns>
    public static int LocateChildrenBottomY(Panel panel)
    {
      int maxY = 0;

      foreach (Control ctrl in panel.Controls)
      {
        maxY = Math.Max(maxY, ctrl.Bottom);
      }

      return maxY;
    }


    /// <summary>
    /// Retrieves the top edge of the uppermost control of the specified type.
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="uniqueControl"></param>
    /// <returns></returns>
    public static int LocateChildTopY(Panel panel, Control uniqueControl)
    {
      int minY = 32000;  // Ridiculously large number

      foreach (Control ctrl in panel.Controls)
      {
        if (ctrl.GetType() == uniqueControl.GetType())
          minY = Math.Min(minY, ctrl.Top);
      }

      return minY;
    }


    /// <summary>
    /// Retrieves the first child control of the specified type from the panel.
    /// Note: This is simply the first control in the control collection, not necessarily the uppermost one.
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="uniqueControl"></param>
    /// <returns></returns>
    public static Control GetFirstChildControl(Panel panel, Control uniqueControl)
    {
      Control retCtrl = new Control();

      foreach (Control ctrl in panel.Controls)
      {
        if (ctrl.GetType() == uniqueControl.GetType())
        {
          retCtrl = ctrl;
          break;
        }
      }

      return retCtrl;
    }


    /// <summary>
    /// Horizontally centers all of the controls inside of a panel.
    /// </summary>
    /// <param name="panel"></param>
    public static void CenterPanelControls(Panel panel)
    {
      if (panel.Controls.Count > 0)
      {
        int availWid = panel.ClientSize.Width;
       
        Control ctrl0 = panel.Controls[0];

        if (ctrl0.Left != (availWid - ctrl0.Width) / 2)   // Check position of the first control in the panel
        {
          // The controls aren't centered so make them so
          foreach (Control ctrl in panel.Controls)
          {
            ctrl.Left = (availWid - ctrl.Width) / 2;
          }
        }
      }
    }


    /// <summary>
    /// Centers the specified control inside of its parent container.
    /// </summary>
    /// <param name="ctrl"></param>
    public static void CenterControl(Control ctrl)
    {
      ctrl.Location = new Point((ctrl.Parent.Width - ctrl.Width) / 2, (ctrl.Parent.Height - ctrl.Height) / 2);
    }



    /// <summary>
    /// Determines the optimum height & width of the ListView control based
    /// on the data it's going to display and then sets it accordingly.
    /// </summary>
    /// <param name="listView"></param>
    /// <param name="itemHgt"></param>
    /// <param name="maxSize"></param>
    /// <param name="choices"></param>
    /// <returns>The optimum size of the ListView</returns>
    public static void SetOptimumListViewSize (ListView listView, int itemHgt, Size maxSize, _Choices choices)
    {
      int gap = 4;

      if (listView.Columns.Count == 0)   // May not be fully initialized yet
        return;

      string[] textList = new string[choices.Count];
      for (int i = 0; i < choices.Count; i++)
      {
        textList[i] = choices[i].Text;
      }

      int maxTextWid = GetMaxLabelWidth(textList, listView.Font);  // This is the width of the text that will appear in the 2nd column
      
      // First determine required width of the ListView
      int diffWid = 35;  // I don't know to calculate this number precisely so just chose this value after a little testing
      listView.Columns[1].Width = maxTextWid + diffWid + gap;
      listView.Width = Math.Min(listView.Columns[0].Width + listView.Columns[1].Width + diffWid / 7, maxSize.Width);

      // Now determine the required height of the ListView
      int diffHgt = 20;
      listView.Height = Math.Min((itemHgt + 1) * choices.Count + diffHgt, maxSize.Height);
    }


    //leftIndent = CheckForLongLabels(quest.Choices, availWidth, leftIndent);
    public static int CheckForLongLabels (_Choices choices, Font font, int availWidth, int initLeft, int altLeft)
    {
      int maxWidth = 0;

      foreach (_Choice choice in choices)
      {
        maxWidth = Math.Max(maxWidth, GetLabelWidth(choice.Text, font));
      }

      return (maxWidth > availWidth) ? altLeft : initLeft;
    }


    /// <summary>
    /// Converts an ArrayList containing a bunch of numbers to percentages.
    /// Ex. If the elements are:  1  2  3  4    then the percentages will be: 10% 20% 30% 40%
    /// </summary>
    /// <param name="srcArray"></param>
    /// <returns></returns>
    public static ArrayList ConvertArrayListToPercentages(ArrayList arrayList)
    {
      double sum = 0.0;
      foreach(object item in arrayList)
      {
        sum += (double) item;
      }

      for (int i = 0; i < arrayList.Count; i++)
      {
        arrayList[i] = (double) arrayList[i] / sum * 100;
      }

      return arrayList;
    }


    /// <summary>
    /// VS2005 has a slicker way of converting an ArrayList of integers to an array of decimals but we'll have to use this for now.
    /// </summary>
    /// <param name="srcArray"></param>
    /// <returns></returns>
    public static decimal[] ConvertArrayListToDecimalArray(ArrayList srcArray)
    {
      decimal[] decArray = new decimal[srcArray.Count];

      int i = 0;
      foreach(object obj in srcArray)
      {
        decArray[i] = Convert.ToDecimal(obj);
        i++;
      }

      return decArray;
    }


    /// <summary>
    /// This method appends each element of 'array1' with its corresponding element in 'array2'.
    /// </summary>
    /// <param name="array1"></param>
    /// <param name="array2"></param>
    /// <returns></returns>
    public static string[] AppendStringArray (string[] array1, string[] array2)
    {
      string[] retArray = new string[array1.Length];

      for (int i = 0; i < array1.Length; i++)
      {
        retArray[i] = array1[i] + array2[i];
      }

      return retArray;
    }


    /// <summary>
    /// This is an alternate version of the method above.  With this one, an ArrayList, rather a string array, is passed as the second parameter.
    /// </summary>
    /// <param name="array1"></param>
    /// <param name="array2"></param>
    /// <returns></returns>
    public static string[] AppendStringArray (string[] array1, ArrayList array2)
    {
      string[] retArray = new string[array1.Length];

      for (int i = 0; i < array1.Length; i++)
      {
        object obj = array2[i];
        string item = "";
        switch(obj.GetType().Name)
        {
          case "Decimal":
          case "Float":
          case "Single":
          case "Double":
            item = String.Format("{0:0.0}", Math.Round(Convert.ToDouble(obj), 1));
            break;

          default:
            item = obj.ToString();
            break;
        }

        retArray[i] = array1[i] + item;
      }

      return retArray;
    }


    /// <summary>
    /// This version is a bit different in that the first parameter is an ArrayList.
    /// </summary>
    /// <param name="array1"></param>
    /// <param name="suffix"></param>
    /// <param name="adjustTense"></param>
    /// <returns></returns>
    public static string[] AppendStringArray (ArrayList array1, string suffix, bool adjustTense)
    {
      string[] retArray = new string[array1.Count];

      for (int i = 0; i < array1.Count; i++)
      {
        object obj = array1[i];
        string item = "";
        switch(obj.GetType().Name)
        {
          case "Int":
          case "Int16":
          case "Int32":
            item = obj.ToString();
            break;

          case "Decimal":
          case "Float":
          case "Single":
          case "Double":
            item = String.Format("{0:0.0}", Math.Round(Convert.ToDouble(obj), 1));
            break;

          default:
            item = obj.ToString();
            break;
        }

        if (adjustTense)
          retArray[i] = PrepareCorrectTense(Convert.ToInt32(array1[i]), suffix);
        else
          retArray[i] = array1[i] + suffix;
      }

      return retArray;
    }

    public static string[] AppendStringArray (ArrayList array1, string suffix)
    {
      return AppendStringArray(array1, suffix, false);
    }


    /// <summary>
    /// This is an alternate version of the methods above.  With this one, a solitary string value is appended to each element.
    /// </summary>
    /// <param name="array1"></param>
    /// <param name="suffix"></param>
    /// <param name="adjustTense"></param>
    /// <returns></returns>
    public static string[] AppendStringArray (string[] array1, string suffix, bool adjustTense)
    {
      string[] retArray = new string[array1.Length];

      for (int i = 0; i < array1.Length; i++)
      {
        if (adjustTense)
          retArray[i] = PrepareCorrectTense(Convert.ToInt32(array1[i]), suffix);
        else
          retArray[i] = array1[i] + suffix;
      }

      return retArray;
    }

    public static string[] AppendStringArray (string[] array1, string suffix)
    {
      return AppendStringArray(array1, suffix, false);
    }




    #if(!CF)

    // Checks whether the application is expired (if an expiry date is embedded).  
    // If so, provides a warning message and returns 'true'.
    public static bool IsAppExpired()
    {
      if (SysInfo.Data.Admin.ExpiryDate == DateTime.MinValue)  // Check for null date
        return false;

      TimeSpan duration = SysInfo.Data.Admin.ExpiryDate - DateTime.Now.Date;

      if (duration.Days < 0)
        return true;
      
      else if (duration.Days < 8)
      {
        string msg = "This copy of the software will expire on " + SysInfo.Data.Admin.ExpiryDate.ToShortDateString() + ".\n\n";
        msg += "Please contact us to obtain a new copy of the software.";
        MessageBox.Show(msg, SysInfo.Data.Admin.AppName + " Is Expiring Soon", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        System.Diagnostics.Process.Start("http://PocketPollster.com");
      }

      return false;
    }

    #endif


  
  }  // End of class Tools
}
