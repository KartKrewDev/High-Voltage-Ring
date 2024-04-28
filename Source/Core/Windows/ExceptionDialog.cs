#region ================== Namespaces

using System;
using System.IO;
using System.Management;
using System.Windows.Forms;
using System.Threading;

#endregion

namespace CodeImp.DoomBuilder.Windows
{
	public partial class ExceptionDialog : Form
	{
		private const string CRASH_DUMP_FILE = "UDBCrash.txt";

		private readonly bool isterminating;
		private readonly string logpath;
		
		public ExceptionDialog(UnhandledExceptionEventArgs e) 
		{
			InitializeComponent();

			logpath = Path.Combine(General.SettingsPath, CRASH_DUMP_FILE);
			Exception ex = (Exception)e.ExceptionObject;
			errorDescription.Text = "Error in " + ex.Source + ":";
			string sysinfo = GetSystemInfo();
			using(StreamWriter sw = File.CreateText(logpath)) 
			{
				sw.Write(sysinfo + GetExceptionDescription(ex));
			}

			errorMessage.Text = ex.Message + Environment.NewLine + ex.StackTrace;
			isterminating = e.IsTerminating;  // Recoverable?
		}

		public ExceptionDialog(ThreadExceptionEventArgs e) 
		{
			InitializeComponent();

			logpath = Path.Combine(General.SettingsPath, CRASH_DUMP_FILE);
			errorDescription.Text = "Error in " + e.Exception.Source + ":";
			string sysinfo = GetSystemInfo();
			using(StreamWriter sw = File.CreateText(logpath)) 
			{
				sw.Write(sysinfo + GetExceptionDescription(e.Exception));
			}

			errorMessage.Text = sysinfo + "********EXCEPTION DETAILS********" + Environment.NewLine 
				+ e.Exception.Message + Environment.NewLine + e.Exception.StackTrace;
		}

		public void Setup() 
		{
			string[] titles =
			{
				"High Voltage Ring has encountered an error."
			};

			this.Text = titles[new Random().Next(0, titles.Length - 1)];
			bContinue.Enabled = !isterminating;
		}

		private static string GetSystemInfo()
		{
			string result = "***********SYSTEM INFO***********" + Environment.NewLine;
			
			// Get OS name
			ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem");
			foreach(ManagementBaseObject mo in searcher.Get())
			{
				result += "OS: " + mo["Caption"] + Environment.NewLine;
				break;
			}

			// Get GPU name
			searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
			foreach(ManagementBaseObject mo in searcher.Get())
			{
				PropertyData bpp = mo.Properties["CurrentBitsPerPixel"];
				PropertyData description = mo.Properties["Description"];
				if(bpp != null && description != null && bpp.Value != null)
				{
					result += "GPU: " + description.Value + Environment.NewLine;
					break;
				}
			}

            // Get GZDB version
            result += "HVR: R" + General.ThisAssembly.GetName().Version.Revision + Environment.NewLine;
            result += "Platform: " + (Environment.Is64BitProcess ? "x64" : "x86") + Environment.NewLine + Environment.NewLine;

			return result;
		}

		private static string GetExceptionDescription(Exception ex) 
		{
			// Add to error logger
			General.WriteLogLine("***********************************************************");
			General.ErrorLogger.Add(ErrorType.Error, ex.Source + ": " + ex.Message);
			General.WriteLogLine("***********************************************************");

			string message = "********EXCEPTION DETAILS********"
							 + Environment.NewLine + ex.Source + ": " + ex.Message + Environment.NewLine + ex.StackTrace;

			if(File.Exists(General.LogFile)) 
			{
				try 
				{
					string[] lines = File.ReadAllLines(General.LogFile);
					message += Environment.NewLine + Environment.NewLine + "***********ACTIONS LOG***********";
					for(int i = lines.Length - 1; i > -1; i--) 
						message += Environment.NewLine + lines[i];
				} catch(Exception) { }
			}

			return message;
		}

		private void reportlink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) 
		{
			if(!File.Exists(logpath)) return;
			try { System.Diagnostics.Process.Start("explorer.exe", @"/select, " + logpath); }
			catch { MessageBox.Show("Unable to show the error report location..."); }
			reportlink.LinkVisited = true;
		}

		private void newissue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) 
		{
			try { System.Diagnostics.Process.Start("https://git.do.srb2.org/KartKrew/high-voltage-ring/-/issues"); } 
			catch { MessageBox.Show("Unable to open URL..."); }
			newissue.LinkVisited = true;
		}

		private void bContinue_Click(object sender, EventArgs e) 
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void bQuit_Click(object sender, EventArgs e)
		{
			if(General.Map != null) General.Map.SaveMapBackup();
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void bToClipboard_Click(object sender, EventArgs e) 
		{
			errorMessage.SelectAll();
			errorMessage.Copy();
			errorMessage.DeselectAll();
		}
	}
}
