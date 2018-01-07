using System;
using System.Diagnostics;
using System.Windows.Forms;



namespace DataObjects
{
	/// <summary>
	/// This User Manager will eventually be much more extensive but right now will just contain the rudimentary methods required to handle users.
	/// </summary>

	public class UserMgr
	{
		public UserMgr()
		{
		}



    /// <summary>
    /// Checks the specified UserName with those stored in the Users table of SysInfo 
    /// and takes the appropriate action depending on what is found therein.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="guid"></param>
    /// <param name="desktopUser"></param>  // True if username is being checked by the Desktop app for the desktop user
    /// <returns>UserName - Either the original passed as 'userName' or a modified version</returns>
    public static string ExamineUser (string userName, string firstName, string lastName, string guid, bool desktopUser)
    {
      // We can't let any blank UserNames be added
      if (userName == "")
        return null;

      string newUserName = null;
      _User user = SysInfo.Data.Users.Find_User(userName);

      if (user == null)   // We've never seen this UserName before so create a new user with it
      {   
        user = CreateNewUser(userName, firstName, lastName, desktopUser);
        newUserName = user.Name;
        
        if (!desktopUser)
          UpdateCFSysInfo(userName, firstName, lastName);
      }

      else
      {
        if (user.FirstName == firstName && user.LastName == lastName)   // The user already exists and first & last names match
        {
//          _Device device = SysInfo.Data.Devices.Find_Guid(guid);
//        
//          if ((device == null) || (userName != device.PrimaryUser))
//          {
//            newUserName = FindOutIfSamePerson(userName, firstName, lastName, user, desktopUser);
//          }
//          else
//          {
//            // Nothing to do because we've seen this user and this device before and this device
//            // is still being used by the same person as recorded in the PrimaryUser field.
//            newUserName = userName;   // Except set the return value
//          }

          // Jan 2007 Note: Because we now allow a device's username to be changed on-the-fly if so allowed by a special 
          //                setting in a poll, the previous tests no longer have the same relevance as they did before.
          // So just set the return value becaus this is a known user
          newUserName = userName;
        }
        else   // Either the first or last name is different (probably the first name) so ask if it's the same person (ex. "Rob Werner" vs. "Robert Werner" vs. "Richard Werner"
          newUserName = FindOutIfSamePerson(userName, firstName, lastName, user, desktopUser);
      }

      return newUserName;
    }


    /// <summary>
    /// The username matches with an existing one so we need to ask the desktop user whether this is
    /// the same person or a different one.  There are many possibilities of how this could happen:
    ///   - John Smith (jsmith) vs. Jane Smith (jsmith)         [Different person]
    ///   - Robert Werner (rwerner) vs. Rob Werner (rwerner)    [Same person, but name entered differently]
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="user"></param>
    /// <param name="desktopUser"></param>
    /// <returns></returns>
    private static string FindOutIfSamePerson(string userName, string firstName, string lastName, _User user, bool desktopUser)
    {
      string newUserName = null;
      string msg = "";
      
      if (desktopUser)
        msg = "The default username for you was determined to be '" + userName + "'.";
      else
        msg = "This mobile device was registered with the username '" + userName + "'.";

      msg += "\nBut this username is already being used by " + user.FirstName + " " + user.LastName + ".";
      
      if (desktopUser)
        msg += "\n\nMight you be the same person?";
      else
        msg += "\n\nIs the user of this device, " + firstName + " " + lastName + ", the same person?";
      
      if (DialogResult.Yes == MessageBox.Show(Tools.ForegroundWindow.Instance, msg, "UserName Already Exists", MessageBoxButtons.YesNo))
      {
        user.FirstName = firstName;                         // Update "new" first name - ex. "Rob" vs. "Robert", but the same person in both cases
        user.LastName = lastName;                           // Technically not required but won't hurt
        newUserName = userName;                             // No change in username, but need to set this return value variable

        if (!desktopUser)
          UpdateCFSysInfo(userName, firstName, lastName);     // Update permanent copy in mobile SysInfo        
      }

      else  // Not the same person so assign a new UserName (Debug: In the future we could display a dialog box to request a new name)
      {     // Debug: Remember that we must change CFSysInfo and send back to device if username is changed!!!
        newUserName = GetNewUserName(userName, user);
        MessageBox.Show(Tools.ForegroundWindow.Instance, firstName + " " + lastName + "'s username was changed from '" + userName + "' to '" + newUserName + "'.", "UserNames Must Be Unique", MessageBoxButtons.OK, MessageBoxIcon.Information);
        user = CreateNewUser(newUserName, firstName, lastName, desktopUser);

        if (!desktopUser)
          UpdateCFSysInfo(newUserName, firstName, lastName);              // Update temporary copy of CFSysInfo.xml
      }

      return newUserName;
    }



    /// <summary>
    /// Creates a new username, that isn't already being used, based on the original one provided.
    /// It is generally called when the user has requested (read "previously created") a username
    /// that already exists - ex. "Robert Werner" = rwerner  but "Rachel Werner" = rwerner too.
    /// We can't have two different users with the same username.
    /// </summary>
    /// <param name="origName"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    private static string GetNewUserName(string origName, _User user)
    {
      string newUserName = origName;
      int numSuffix = 2;  // This is the default suffix we'll attach to the username to change it to something different

      do
      {
        newUserName = origName + numSuffix.ToString();
        user = SysInfo.Data.Users.Find_User(newUserName);

        if (user == null)
          return newUserName;
        else
          numSuffix++;
      } while (true);     // We'll break out of the loop via the return statement
    }



    private static _User CreateNewUser(string userName, string firstName, string lastName, bool desktopUser)
    {
      _User user = new _User(SysInfo.Data.Users);

      user.ID = -1;                // Forces next available ID # to be set
      user.Name = userName;
      user.FirstName = firstName;
      user.LastName = lastName;
      user.Privileges.PPC = true;
      user.Privileges.Desktop = desktopUser;

      SysInfo.Data.Users.Add(user);

      return user;
    }

    // To be call from the mobile app.
    private static _User CreateNewUser(string userName, string firstName, string lastName)
    {
      return CreateNewUser(userName, firstName, lastName, false);
    }


    // Note: The various methods contained here in the User Manager are used by both the Desktop app,
    //       on behalf of the Desktop user, and by the Data Transfer thread, on behalf of the Mobile
    //       user.  In the former case, this particular method isn't really necessary but calling it
    //       from the various methods shown above doesn't cause any harm.
    private static void UpdateCFSysInfo(string userName, string firstName, string lastName)
    {
      CFSysInfo.Data.MobileOptions.PrimaryUser = userName;
      CFSysInfo.Data.MobileOptions.FirstName = firstName;
      CFSysInfo.Data.MobileOptions.LastName = lastName;
      CFSysInfo.Data.MobileOptions.AllowNameEditing = false;   // Debug: How can we provide a simple way to give the Desktop user the ability to make this true when desired?
      //CFSysInfo.Data.MobileOptions.UserInfoConfirmed = true;
    }

	}
}
