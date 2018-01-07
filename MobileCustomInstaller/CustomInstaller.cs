using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Configuration.Install;
using Microsoft.Win32;



namespace MobileInstaller
{
	/// <summary>
	/// Summary description for CustomInstaller.
	/// </summary>
	[RunInstaller(true)]
	public class CustomInstaller : System.Configuration.Install.Installer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CustomInstaller()
		{
			// This call is required by the Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
      this.AfterInstall += new InstallEventHandler(Installer_AfterInstall);
      this.AfterUninstall += new InstallEventHandler(Installer_AfterUninstall);
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
    #endregion


    private void Installer_AfterInstall(object sender, InstallEventArgs e)
    {
      // Get Fullpath to .ini file
      string arg = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PPsetup.ini");

      // Run WinCE App Manager to install .cab file on device
      RunAppManager(arg);
    }


    private void Installer_AfterUninstall(object sender, InstallEventArgs e)
    {
      // Run app manager in uninstall mode (without any arguments)
      RunAppManager(null);
    }


    private void RunAppManager(string arg)
    {
      // Get path to the app manager
      const string RegPath = @"Software\Microsoft\Windows\" + @"CurrentVersion\App Paths\CEAppMgr.exe";

      RegistryKey key = Registry.LocalMachine.OpenSubKey(RegPath);
      string appManager = key.GetValue("") as string;
   
      if (appManager != null)
      {
        // Launch the app
        Process.Start(string.Format("\"{0}\"", appManager), (arg == null) ? "" : string.Format("\"{0}\"", arg));
      }
      else
      {
        // Could not locate app manager
        MessageBox.Show("Could not launch the WinCE Application Manager.");
      }
    }




  }
}
