using System;
using System.Reflection;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;



namespace Multimedia
{
  public class Images
  {
    public static Image GetImage(string imageName)
    {
      int imageNum = -1;
      return GetImage(imageName, ref imageNum);
    }

    public static Image GetImage(string imageBaseName, ref int imageNum)
    {
      string fullName = "";
      Bitmap image = null;
      Stream stream;

      Assembly assembly = Assembly.GetExecutingAssembly();

      string assemblyName = assembly.GetName().ToString();
      assemblyName = assemblyName.Substring(0, assemblyName.IndexOf(","));  // ie. "Multimedia"

      // Is this just a single (ie. one-time) image?
      if (imageNum == -1)
      {
        fullName = assemblyName + ".";

        // See if a specific extension has been provided.  If not, then assume it's a "jpg"
        string[] nameParts = imageBaseName.Split(new char[] {'.'});
        if (nameParts.Length > 1)
          fullName += imageBaseName;
        else
          fullName += imageBaseName + ".jpg";

        stream = assembly.GetManifestResourceStream(fullName);
      }
      else  // Or is it one of many in an animation
      {
        // We'll assume that all animation components are JPEGs (at least for now)
        fullName = assemblyName + "." + imageBaseName + imageNum.ToString() + ".jpg";
        stream = assembly.GetManifestResourceStream(fullName);

        if (stream == null)
        {
          imageNum = 1;   // Reset sequence
          fullName = assemblyName + "." + imageBaseName + "1.jpg";
          stream = assembly.GetManifestResourceStream(fullName);
        }
      }   

      if (stream != null)
        image = new Bitmap(stream);

      stream.Close();

      return image;
    }


    public static Icon GetIcon(string iconName)
    {
      int iconNum = -1;
      return GetIcon(iconName, ref iconNum);
    }

    public static Icon GetIcon(string iconBaseName, ref int iconNum)
    {
      string fullName = "";
      Icon icon = null;
      Stream stream;

      Assembly assembly = Assembly.GetExecutingAssembly();

      string assemblyName = assembly.GetName().ToString();
      assemblyName = assemblyName.Substring(0, assemblyName.IndexOf(","));  // ie. "Multimedia"

      // Is this just a single (ie. one-time) image?
      if (iconNum == -1)
      {
        fullName = assemblyName + "." + iconBaseName + ".ico";
        stream = assembly.GetManifestResourceStream(fullName);
      }
      else  // Or is it one of many in an animation
      {
        fullName = assemblyName + "." + iconBaseName + iconNum.ToString() + ".ico";
        stream = assembly.GetManifestResourceStream(fullName);

        if (stream == null)
        {
          iconNum = 1;   // Reset sequence
          fullName = assemblyName + "." + iconBaseName + "1.ico";
          stream = assembly.GetManifestResourceStream(fullName);
        }
      }   

      if (stream != null)
        icon = new Icon(stream);

      stream.Close();

      return icon;
    }







//    public static string GetImage(string imageName)
//    {
//      int imageNum = -1;
//      return GetImage(imageName, ref imageNum);
//    }

//    public static string GetImage(string imageBaseName, ref int imageNum)
//    {
//      string fullName = "";
//      string imgFilename = "";
//
//      // Is this just a single (ie. one-time) image?
//      if (imageNum == -1)
//      {
//        fullName = imageBaseName + ".jpg";
//        imgFilename = ExtractResource(fullName);
//      }
//      else  // Or is it one of many in an animation
//      {
//        fullName = imageBaseName + imageNum.ToString() + ".jpg";
//        stream = assembly.GetManifestResourceStream(fullName);
//
//        if (stream == null)
//        {
//          imageNum = 1;   // Reset sequence
//          fullName = imageBaseName + "1.jpg";
//          stream = assembly.GetManifestResourceStream(fullName);
//        }
//      }   
//
//      return imgFilename;
//    }

//    public static string GetImage(string imageBaseName, ref int imageNum)
//    {
//      string fullName = "";
//      string imgFilename = "";
//
//      // Is this just a single (ie. one-time) image?
//      if (imageNum == -1)
//      {
//        fullName = imageBaseName + ".jpg";
//        imgFilename = ExtractResource(fullName);
//      }
//      else  // Or is it one of many in an animation
//      {
//        fullName = imageBaseName + imageNum.ToString() + ".jpg";
//        imgFilename = ExtractResource(fullName);
//
//        if (imgFilename == null)
//        {
//          imageNum = 1;   // Reset sequence
//          fullName = imageBaseName + "1.jpg";
//          imgFilename = ExtractResource(fullName);
//        }
//      }   
//
//      return imgFilename;
//    }
//
//
//
//
//
//    /// <summary>
//    /// This method is a replacement for GetImage above.  It actually extracts an embedded resource out of
//    /// the class library and stores it as a temporary file in the OS's designated temporary directory.
//    /// </summary>
//    /// <param name="resourceName"></param>
//    /// <returns></returns>
//    public static string ExtractResource(string resourceName)
//    {
//      // Look for the resource name
//      foreach(string currentResource in Assembly.GetExecutingAssembly().GetManifestResourceNames())
//      {
//        if (currentResource.LastIndexOf(resourceName) != -1)
//        {
//          string fqnTempFile = Path.GetTempFileName();
//          string path = Path.GetDirectoryName(fqnTempFile);
//          string rootName= Path.GetFileNameWithoutExtension(fqnTempFile);
//          string destFile = path + @"\" + rootName + Path.GetExtension(currentResource);
//
//          Stream fs = Assembly.GetExecutingAssembly().GetManifestResourceStream(currentResource);
//
//          byte[] buff = new byte[fs.Length];
//          fs.Read( buff, 0, (int) fs.Length);
//          fs.Close();
//
//          FileStream destStream = new FileStream (destFile, FileMode.Create);
//          destStream.Write(buff, 0, buff.Length);
//          destStream.Close();
//
//          return destFile;
//        }
//      }
//
//      //throw new Exception("Resource not found : " + resourceName);  // Because of the animation code accessing this, we don't want to see an error
//      return null;
//    } 



  }
}
