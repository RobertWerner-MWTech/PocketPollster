using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;



namespace DataObjects
{
	/// <summary>
	/// Summary description for DBTools.
	/// </summary>
	public class DBTools
	{

    public static bool CreateDatabase(string fullFilename)
    {
      bool succeeded = false;

      try
      {
        string newDB = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fullFilename; 
        Type objClassType = Type.GetTypeFromProgID("ADOX.Catalog"); 

        if (objClassType != null) 
        { 
          object obj = Activator.CreateInstance(objClassType); 

          // Create MDB file 
          string engineType = ((byte) SysInfo.Data.Options.MDBformat).ToString();
          obj.GetType().InvokeMember("Create", System.Reflection.BindingFlags.InvokeMethod, null, obj, 
            new object[]{"Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:Engine Type=" + engineType + ";Data Source=" + newDB + ";" });

          succeeded = true;

          // Clean up
          System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
          obj = null;
        }
      }

      catch (Exception ex)
      {
        Tools.ShowMessage("Could not create database file: " + fullFilename + "\n\n" + ex.Message, "Database Creation Error");
      }

      return succeeded;
    }


    public static bool RunSQLNonQuery(OleDbConnection conn, string cmdText)
    {
      bool succeeded = false;

      try
      {
        conn.Open();
        OleDbCommand cmd = new OleDbCommand(cmdText, conn);
        cmd.ExecuteNonQuery();
        conn.Close();
        succeeded = true;
      }

      catch (Exception ex)
      {
        conn.Close();
        Tools.ShowMessage("SQL statement did not succeed.\n\n" + cmdText + "\n\n" + ex.Message, "SQL NonQuery Error");
      }

      return succeeded;
    }


    /// <summary>
    /// Created from here: http://www.codeproject.com/cs/database/mdbcompact_latebind.asp
    /// </summary>
    /// <param name="connectString"></param>
    /// <param name="fullFilename"></param>
    public static void CompactDatabase(string connectString, string fullFilename)
    {
      try
      {
        object[] oParams;
        
        // Create an instance of a Jet Replication Object
        object objJRO = Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));

        string tempDBPath = SysInfo.Data.Paths.Temp + "TempDB.mdb";

        if (File.Exists(tempDBPath))
          File.Delete(tempDBPath);

        // Fill Parameters array
        // Note: change "Jet OLEDB:Engine Type=5" to an appropriate value
        //       or leave it as is if your DB is JET4X format (Access 2000,2002)
        //       (yes, jetengine5 is for JET4X, no misprint here)
        string engineType = ((byte) SysInfo.Data.Options.MDBformat).ToString();
        oParams = new object[] { connectString,
                                 "Provider=Microsoft.Jet.OLEDB.4.0;Data" + 
                                 " Source=" + tempDBPath + ";Jet OLEDB:Engine Type=" + engineType};
      
        // Invoke the CompactDatabase method of a JRO object
        // Pass Parameters array
        objJRO.GetType().InvokeMember("CompactDatabase",
          System.Reflection.BindingFlags.InvokeMethod,
          null,
          objJRO,
          oParams);

        // Database is compacted now to a new file
        // Let's copy it over an old one and delete it
        System.IO.File.Delete(fullFilename);
        System.IO.File.Move(tempDBPath, fullFilename);

        // Clean up (just in case)
        System.Runtime.InteropServices.Marshal.ReleaseComObject(objJRO);
        objJRO = null;
      }

      catch (Exception ex)
      {
        // For now, let's now show a msg because as long as program doesn't fail then it's not critically important
        //Tools.ShowMessage("Compacting MDB database failed.\n\nError message: " + ex.Message, "DBTools.CompactDatabase");
        Debug.WriteLine("Compacting MDB database failed.\n\nError message: " + ex.Message, "DBTools.CompactDatabase");
      }
    }



//    public static DataSet OpenRecordSet(OleDbConnection conn, string sqlString)
//    {
//      try
//      {
//        OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlString, conn);
//
//
//        //    Function OpenRecordSet(ByVal SQLstr As String)
//        //    Try
//        //      dadapter = New OleDb.OleDbDataAdapter(SQLstr, conn)
//        //    dadapter.MissingSchemaAction = MissingSchemaAction.AddWithKey
//        //                                     dadapter.MissingMappingAction = MissingMappingAction.Passthrough
//        //                                                                       dadapter.Fill(dset)
//        //                                                                       fields = dset.Tables(0).Columns
//        //                                                                                                 If dset.Tables(0).Rows.Count() = 0 Then
//        //                                                                                                                                      index = -1
//        //                                                                                                                                      Else
//        //                                                                                                                                        index = 0
//        //                                                                                                                                        End If
//        //                                                                                                                                          newRow = False
//        //                                                                                                                                                     OpenRecordSet = True
//        //                                                                                                                                                     Catch ex As Exception
//        //                                                                                                                                                                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical + _
//        //                                                                                                                                                                                                                        MsgBoxStyle.ApplicationModal + MsgBoxStyle.OkOnly, _
//        //                                                                                                                                                                                                                                                                             "ADO.NET RecordSet Library Error")
//        //    OpenRecordSet = False
//        //                      End Try
//        //                            End Function
//
//
//
//
//      }
//
//      catch (Exception ex)
//      {
//
//
//
//      }
//
//
//
//    }











    // Note: This method currently only takes a cursory look at the data table.  It doesn't carefully look to see if each field exists and is correct.
    public static bool IsItemInTable(DataTable table, string colName, string searchString)
    {
      bool itemFound = false;

      foreach (DataRow row in table.Rows)
      {
        if (row[colName].ToString().ToLower() == searchString.ToLower())
        {
          itemFound = true;
          break;
        }
      }

      return itemFound;
    }




	}
}
