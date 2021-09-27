#region ================== Copyright (c) 2021 Boris Iwanski

/*
 * This program is free software: you can redistribute it and/or modify
 *
 * it under the terms of the GNU General Public License as published by
 * 
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 * 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * 
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.If not, see<http://www.gnu.org/licenses/>.
 */

#endregion

#region ================== Namespaces

using System;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Threading;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Config;

#endregion

namespace CodeImp.DoomBuilder.Windows
{
	public partial class RunExternalCommandForm : DelayedForm
	{
		#region ================== Variables

		private Process process;
		private ProcessStartInfo startinfo;
		private Thread runthread;
		private object lockobj;
		private bool haserrors;
		private ExternalCommandSettings settings;

		#endregion

		#region ================== Delegates

		private delegate void CallStringBoolMethodDeletage(string s, bool iserror);
		private delegate void CallVoidMethodDeletage();

		#endregion

		#region ================== Constructors

		public RunExternalCommandForm(ProcessStartInfo startinfo, ExternalCommandSettings settings)
		{
			InitializeComponent();

			lockobj = new object();
			haserrors = false;

			rtbOutput.Font = new Font(FontFamily.GenericMonospace, rtbOutput.Font.Size);

			this.startinfo = startinfo;
			this.settings = settings;
		}

		#endregion

		#region ================== Methods

		/// <summary>
		/// Starts execution of the external command
		/// </summary>
		private void Start()
		{
			rtbOutput.Clear();
			haserrors = false;
			btnContinue.Enabled = false;
			btnRetry.Enabled = false;

			runthread = new Thread(() => Run());
			runthread.Name = "Run external command";
			runthread.Priority = ThreadPriority.Normal;
			runthread.Start();
		}

		/// <summary>
		/// Stops execution of the external command
		/// </summary>
		private void Stop()
		{
			haserrors = true;
			process.CancelOutputRead();
			process.CancelErrorRead();
			KillProcessAndChildren(process.Id);
			FinishRun();
		}

		/// <summary>
		/// Runs the external command
		/// </summary>
		private void Run()
		{
			process = new Process();
			process.StartInfo = startinfo;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.CreateNoWindow = true;

			process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
			process.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();
			process.WaitForExit();

			if (process.ExitCode != 0 && settings.ExitCodeIsError)
				haserrors = true;

			FinishRun();
		}

		/// <summary>
		/// Finishes after running the external command. Closes the window if there were no errors
		/// </summary>
		private void FinishRun()
		{
			if(InvokeRequired)
			{
				CallVoidMethodDeletage d = FinishRun;
				Invoke(d);
			}
			else
			{
				btnContinue.Enabled = true;
				btnRetry.Enabled = true;

				if(haserrors)
				{
					AppendText(Environment.NewLine + Environment.NewLine + "There were errors during the execution of the external commands.", true);
					AppendText(Environment.NewLine + "Exit code: " + process.ExitCode, true);
				}

				if(!haserrors && settings.AutoCloseOnSuccess)
				{
					DialogResult = DialogResult.OK;
					Close();
				}
			}
		}

		/// <summary>
		/// Handles lines going to stdout
		/// </summary>
		/// <param name="sendingprocess">Process the line is coming from</param>
		/// <param name="outline">The received line event</param>
		private void OutputHandler(object sendingprocess, DataReceivedEventArgs outline)
		{
			if(outline.Data != null)
				AppendText(outline.Data + Environment.NewLine, false);
		}

		/// <summary>
		/// handles lines going to stderr
		/// </summary>
		/// <param name="sendingprocess">Process the line is coming from</param>
		/// <param name="outline">The received line event</param>
		private void ErrorHandler(object sendingprocess, DataReceivedEventArgs outline)
		{
			if (outline.Data != null)
			{
				AppendText(outline.Data + Environment.NewLine, true);

				if(settings.StdErrIsError)
					haserrors = true;
			}
		}

		/// <summary>
		/// Adds a line to the output text box and scrolls to the bottom. Colors the text red if it's an error
		/// </summary>
		/// <param name="text">Text to append</param>
		/// <param name="iserror">If it's an error or not</param>
		private void AppendText(string text, bool iserror)
		{
			if(InvokeRequired)
			{
				CallStringBoolMethodDeletage d = AppendText;
				Invoke(d, text, iserror);
			}
			else
			{
				lock (lockobj)
				{
					rtbOutput.AppendText(text);

					if(iserror)
					{
						rtbOutput.Select(rtbOutput.Text.Length - text.Length, text.Length);
						rtbOutput.SelectionColor = Color.Red;
						//haserrors = true;
					}

					rtbOutput.Select(rtbOutput.Text.Length, 0);
					rtbOutput.ScrollToCaret();
				}
			}
		}

		/// <summary>
		/// Recursively kill a process and its childern. Taken from https://stackoverflow.com/a/40265540
		/// </summary>
		/// <param name="pid">Process id</param>
		private static void KillProcessAndChildren(int pid)
		{
			ManagementObjectSearcher processSearcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
			ManagementObjectCollection processCollection = processSearcher.Get();

			// We must kill child processes first!
			if (processCollection != null)
			{
				foreach (ManagementObject mo in processCollection)
				{
					KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
				}
			}

			// Then kill parents.
			try
			{
				Process proc = Process.GetProcessById(pid);
				if (!proc.HasExited) proc.Kill();
			}
			catch (ArgumentException)
			{
				// Process already exited.
			}
		}

		#endregion

		#region ================== Events

		private void RunExternalCommandForm_Shown(object sender, EventArgs e)
		{
			Start();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (process.HasExited)
			{
				DialogResult = DialogResult.Cancel;
				Close();
			}
			else
			{
				Stop();
			}
		}

		private void btnRetry_Click(object sender, EventArgs e)
		{
			Start();
		}

		private void btnContinue_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void RunExternalCommandForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!process.HasExited)
				e.Cancel = true;
		}

		#endregion
	}
}
