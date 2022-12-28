#region ================== Namespaces

using CodeImp.DoomBuilder.Map;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#endregion

namespace CodeImp.DoomBuilder.Windows
{
	public interface ILinedefEditForm
	{
		//Events
		event EventHandler OnValuesChanged;
		event EventHandler Closed;

		// Methods
		void Setup(ICollection<Linedef> lines, bool selectfront, bool selectback);
		DialogResult ShowDialog(IWin32Window owner);
		void Dispose();
	}
}
