#region ================== Copyright (c) 2020 Boris Iwanski

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
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Windows;

#endregion

namespace CodeImp.DoomBuilder.UDBScript
{
	public partial class PreferencesForm : Form
	{
		#region ================== Constructor

		public PreferencesForm()
		{
			InitializeComponent();

			exepath.Text = BuilderPlug.Me.EditorExePath;
		}

		#endregion

		#region ================== Methods

		// When OK is pressed on the preferences dialog
		// Prevent inlining, otherwise there are unexpected interactions with Assembly.GetCallingAssembly
		// See https://docs.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getcallingassembly?view=netframework-4.6.1#remarks
		[MethodImplAttribute(MethodImplOptions.NoInlining)]
		public void OnAccept(PreferencesController controller)
		{
			BuilderPlug.Me.SetEditor(exepath.Text);
		}

		// This sets up the form with the preferences controller
		public void Setup(PreferencesController controller)
		{
			// Add tab pages
			foreach (TabPage p in tabs.TabPages)
			{
				controller.AddTab(p);

				// The parent tab control has its font set to bold, which inherits to the tab pages,
				// so we need to change it back to regular
				p.Font = new Font(p.Font, FontStyle.Regular);
			}

			// Bind events
			controller.OnAccept += OnAccept;
		}

		#endregion

		#region ================== Events

		private void btnSelectExe_Click(object sender, EventArgs e)
		{
			if(filedialog.ShowDialog() == DialogResult.OK)
			{
				exepath.Text = filedialog.FileName;
			}
		}

		#endregion
	}
}
