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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeImp.DoomBuilder;
using Jint;

#endregion

namespace CodeImp.DoomBuilder.UDBScript
{
	class RuntimeConstraint : IConstraint
	{
		#region ================== Constants

		private const long CHECK_MILLISECONDS = 5000;

		#endregion

		#region ================== Variables

		private Stopwatch stopwatch;

		#endregion

		#region ================== Constructor

		public RuntimeConstraint(Stopwatch stopwatch)
		{
			this.stopwatch = stopwatch;
		}

		#endregion

		#region ================== Methods

		public void Reset()
		{
		}

		/// <summary>
		/// Checks how long the script has been running and asks the user if it should abort or keep running
		/// </summary>
		public void Check()
		{
			if(stopwatch.ElapsedMilliseconds > CHECK_MILLISECONDS)
			{
				DialogResult result = MessageBox.Show("The script has been running for some time, want to stop it?", "Script", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (result == DialogResult.Yes)
					throw new UserScriptAbortException();
				else
				{
					stopwatch.Restart();
				}
			}
		}

		#endregion
	}
}
