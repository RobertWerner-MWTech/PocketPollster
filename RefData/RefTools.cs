using System;
using System.IO;
using System.Reflection;


namespace RefData
{
	/// <summary>
	/// Summary description for RefTools.
	/// </summary>
	public class RefTools
	{
		public RefTools()
		{
		}


    public static void ExtractResource(string path, string resourceName)
    {
      Stream stream;

      Assembly assembly = Assembly.GetExecutingAssembly();

      string assemblyName = assembly.GetName().ToString();
      assemblyName = assemblyName.Substring(0, assemblyName.IndexOf(","));  // ie. "RefData"

      string fullName = assemblyName + "." + resourceName;
      stream = assembly.GetManifestResourceStream(fullName);

      if (stream != null)
      {
        string destFile = path + resourceName;

        byte[] buff = new byte[stream.Length];
        stream.Read(buff, 0, (int) stream.Length);
        stream.Close();

        FileStream destStream = new FileStream (destFile, FileMode.Create);
        destStream.Write(buff, 0, buff.Length);
        destStream.Close();
      }

      stream.Close();
    } 




	}
}
