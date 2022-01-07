#region ================== Copyright (c) 2022 Boris Iwanski

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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Windows;

#endregion

namespace CodeImp.DoomBuilder.UDBScript
{
	public partial class ScriptRunnerForm : DelayedForm
	{
		#region ================== Constants

		/// <summary>
		/// How long a script is allowed to run until the form is made visible.
		/// </summary>
		const int RUNTIME_THRESHOLD = 1000;

		#endregion

		#region ================== Variables

		/// <summary>
		/// Cancellation token for stopping the script.
		/// </summary>
		CancellationTokenSource cancellationtokensource;

		/// <summary>
		/// If script is currently executed or not.
		/// </summary>
		bool running;

		/// <summary>
		/// How many milliseconds the script has been running
		/// </summary>
		double runningseconds;

		/// <summary>
		/// Determines if the form should be automatically closed when the script is finished.
		/// </summary>
		bool autoclose;

		/// <summary>
		/// Stopwatch used to determine how long the script is already running.
		/// </summary>
		Stopwatch stopwatch;

		/// <summary>
		/// Timer for making the form visible when the script is running for too long.
		/// </summary>
		System.Windows.Forms.Timer timer;


		#endregion

		#region ================== Methods

		public ScriptRunnerForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Invokes a method, but stops the timer before running the code, and starting the timer after the code ran.
		/// </summary>
		/// <param name="method">Method to invoke</param>
		/// <returns>Return value of the method</returns>
		public object InvokePaused(Delegate method)
		{
			if (InvokeRequired)
			{
				return Invoke(new Func<object>(() => InvokePaused(method)));
			}
			else
			{
				stopwatch.Stop();
				object result = Invoke(method);
				stopwatch.Start();
				return result;
			}
		}

		/// <summary>
		/// Invokes a method.
		/// </summary>
		/// <param name="action">Method to invoke</param>
		public void RunAction(Action action)
		{
			if (InvokeRequired)
				Invoke(action);
			else
				action();
		}

		/// <summary>
		/// Sets the value of the progress bar (in the range from 0 to 100).
		/// </summary>
		/// <param name="value">Value of the progress bar (in the range from 0 to 100)</param>
		private void SetProgress(int value)
		{
			if (progressbar.Style != ProgressBarStyle.Continuous)
				progressbar.Style = ProgressBarStyle.Continuous;

			// Do some trickery to remove the movement of the progress bar, since it can
			// otherwise screw with how much the progress bar is filled
			if (progressbar.Value != value)
			{
				if (value > progressbar.Maximum)
					value = progressbar.Maximum;
				else if (value < progressbar.Minimum)
					value = progressbar.Minimum;

				if (progressbar.Maximum == value)
				{
					progressbar.Value = value;
					progressbar.Value = value - 1;
				}
				else
				{
					progressbar.Value = value + 1;
				}
				progressbar.Value = value;
			}

			// Make the form visible so that the user can actually see the progress bar
			MakeVisible();
		}

		private void SetProgressStatus(string status)
		{
			lbStatus.Text = status;
		}

		private void Log(string text)
		{
			// If there's something in the log we don't want to automatically
			// close the form, otherwise the user could not read the contents
			autoclose = false;

			// Since we don't want to have a useless line at the end of the textbox
			// we add a new line before adding the new text (unless there's no text
			// at all yet, then we don't add a new line
			if (!string.IsNullOrEmpty(tbLog.Text))
				tbLog.AppendText(Environment.NewLine);

			// Add the new text
			tbLog.AppendText(text);

			// Make the form visible so that the user can actually see the status bar
			MakeVisible();
		}

		private async void RunScript(CancellationToken cancellationtoken)
		{
			// Callbacks for setting the progress bar, status text, and adding log lines from the script
			Progress<int> progress = new Progress<int>(SetProgress);
			Progress<string> status = new Progress<string>(SetProgressStatus);
			Progress<string> log = new Progress<string>(Log);

			running = true;

			// Prepare running the script
			BuilderPlug.Me.ScriptRunner.PreRun(cancellationtoken, progress, status, log);

			try
			{
				await Task.Run(() => BuilderPlug.Me.ScriptRunner.Run());
				stopwatch.Stop();
			}
			catch (Exception ex)
			{
				stopwatch.Stop();
				BuilderPlug.Me.ScriptRunner.HandleExceptions(ex);
			}

			// Clean up and update after running the script
			BuilderPlug.Me.ScriptRunner.PostRun();

			running = false;

			btnAction.Text = "Close";
			btnAction.Enabled = true;

			// Stop the progress bar from animating when the script finished
			if (progressbar.Style == ProgressBarStyle.Marquee)
				progressbar.Style = ProgressBarStyle.Continuous;

			if (autoclose)
			{
				MakeInvisible();
				//Hide();
				Close();
			}
		}

		/// <summary>
		/// Makes the form visible.
		/// </summary>
		private void MakeVisible()
		{
			Opacity = 1.0;
			btnAction.Enabled = true;
		}

		/// <summary>
		/// Makes the form invisible.
		/// </summary>
		private void MakeInvisible()
		{
			Opacity = 0.0;
		}

		#endregion

		#region ================== Events

		/// <summary>
		/// Cancels the currently running script, or closes the form if no script is running.
		/// </summary>
		/// <param name="sender">The sender</param>
		/// <param name="e">Event arguments</param>
		private void btnAction_Click(object sender, EventArgs e)
		{
			if (running)
			{
				btnAction.Enabled = false;
				cancellationtokensource.Cancel();
			}
			else
			{
				MakeInvisible();
				//Hide();
				Close();
			}
		}

		/// <summary>
		/// Sets everything up for running the script, and then immediately runs the script.
		/// </summary>
		/// <param name="sender">The sender</param>
		/// <param name="e">Event arguments</param>
		private void ScriptRunnerForm_Shown(object sender, EventArgs e)
		{
			cancellationtokensource = new CancellationTokenSource();
			autoclose = true;
			runningseconds = 0;

			progressbar.Value = 0;
			progressbar.Style = ProgressBarStyle.Marquee;

			Text = "Running script";
			lbStatus.Text = "Running script...";

			btnAction.Text = "Cancel";
			// Disable the button because it could otherwise be pressed while the form is invisible.
			// It'll be enabled as soon as the form is made visible
			btnAction.Enabled = false;

			tbLog.Clear();

			// The timer ticks ever 100ms. The method it runs checks how long the script is running
			// and makes the form visible if the runtime threshold has been reached
			timer = new System.Windows.Forms.Timer();
			timer.Interval = 100;
			timer.Tick += timerShow_Tick;
			timer.Start();

			// This stopwatch is used to measure how long the script has been running
			stopwatch = new Stopwatch();
			stopwatch.Start();

			// Start running the script
			RunScript(cancellationtokensource.Token);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			MakeInvisible();
		}

		/// <summary>
		/// Makes the form visible if the runtime threshold has been reached. Shows the elapsed time the script is running.
		/// </summary>
		/// <param name="sender">The sender</param>
		/// <param name="e">Event arguments</param>
		private void timerShow_Tick(object sender, EventArgs e)
		{
			if (Opacity == 0.0 && stopwatch.ElapsedMilliseconds > 1000)
			{
				MakeVisible();
			}

			double newrunningsecods = Math.Floor(stopwatch.Elapsed.TotalSeconds);

			if(newrunningsecods > runningseconds)
			{
				runningseconds = newrunningsecods;
				Text = "Running script (" + string.Format("{0:D2}:{1:D2}:{2:D2}", stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds) + ")";
			}
		}

		private void ScriptRunnerForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			timer.Stop();
		}

		#endregion
	}
}