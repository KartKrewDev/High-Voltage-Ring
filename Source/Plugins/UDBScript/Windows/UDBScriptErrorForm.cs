using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Windows;

namespace CodeImp.DoomBuilder.UDBScript
{
	public partial class UDBScriptErrorForm : DelayedForm
	{
		public UDBScriptErrorForm(string message, string stacktrace)
		{
			InitializeComponent();

			tbStackTrace.Text = message + "\r\n" + stacktrace;
			tbStackTrace.Select(0, 0);
		}
	}
}
