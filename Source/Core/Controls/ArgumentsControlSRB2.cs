#region ================== Namespaces

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.GZBuilder.Data;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Types;
using CodeImp.DoomBuilder.GZBuilder;

#endregion

namespace CodeImp.DoomBuilder.Controls
{
	public partial class ArgumentsControlSRB2 : UserControl
	{
		#region ================== Native stuff

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

		private const int WM_SETREDRAW = 11;

		#endregion

		#region ================== Variables

		private int action;
		private ArgumentInfo[] arginfo;
		private ArgumentInfo[] stringarginfo;
		private Label[] labels;
		private ArgumentBox[] args;
		private Label[] stringlabels;
		private TextBox[] stringargs;

		#endregion

		#region ================== Constructor

		public ArgumentsControlSRB2()
		{
			InitializeComponent();

			Reset();

			labels = new Label[] { arg0label, arg1label, arg2label, arg3label, arg4label, arg5label, arg6label, arg7label, arg8label, arg9label };
			args = new ArgumentBox[] { arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 };
			stringlabels = new Label[] { stringarg0label, stringarg1label };
			stringargs = new System.Windows.Forms.TextBox[] { stringarg0, stringarg1 };
		}

		#endregion

		#region ================== Setup

		public void Reset()
		{
			// Only when running (this.DesignMode won't do when not this, but one of parent controls is in design mode)
			if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
			{
				// do nothing.
			}
		}

		public void SetValue(Linedef l, bool first)
		{
			SetValue(l.Fields, l.Args, first);
		}

		public void SetValue(Thing t, bool first)
		{
			SetValue(t.Fields, t.Args, first);
		}

		private void SetValue(UniFields fields, int[] newargs, bool first)
		{
			// Update arguments
			for (int i = 0; i < args.Length; i++)
			{
				if (first)
					args[i].SetValue(newargs[i]);
				else
					if (!string.IsNullOrEmpty(args[i].Text) && newargs[i] != args[i].GetResult(int.MinValue)) args[i].ClearValue();
			}

			for (int i = 0; i < stringargs.Length; i++)
			{
				if (first)
					stringargs[i].Text = fields.GetValue("stringarg" + i, string.Empty);
				else
					if (fields.GetValue("stringarg" + i, string.Empty) != stringargs[i].Text) stringargs[i].Text = string.Empty;
			}
		}

		#endregion

		#region ================== Apply

		public void Apply(Linedef l, int step)
		{
			for (int i = 0; i < args.Length; i++)
				l.Args[i] = args[i].GetResult(l.Args[i], step);

			for (int i = 0; i < stringargs.Length; i++)
				if (!string.IsNullOrEmpty(stringargs[i].Text))
					UniFields.SetString(l.Fields, "stringarg" + i, stringargs[i].Text, string.Empty);
		}

		public void Apply(Thing t, int step)
		{
			for (int i = 0; i < args.Length; i++)
				t.Args[i] = args[i].GetResult(t.Args[i], step);

			for (int i = 0; i < stringargs.Length; i++)
				if (!string.IsNullOrEmpty(stringargs[i].Text))
					UniFields.SetString(t.Fields, "stringarg" + i, stringargs[i].Text, string.Empty);
		}

		#endregion

		#region ================== Update

		//TODO: Info for string args
		public void UpdateAction(int action, bool setuponly)
		{
			// Update arguments
			int showaction = 0;
			ArgumentInfo[] oldarginfo = (arginfo != null ? (ArgumentInfo[])arginfo.Clone() : null); //mxd
			ArgumentInfo[] oldstringarginfo = (stringarginfo != null ? (ArgumentInfo[])stringarginfo.Clone() : null);

			// Only when action type is known
			if (General.Map.Config.LinedefActions.ContainsKey(action)) showaction = action;

			// Update argument infos
			arginfo = General.Map.Config.LinedefActions[showaction].Args;
			stringarginfo = General.Map.Config.LinedefActions[showaction].StringArgs;

			//mxd. Don't update action args when old and new argument infos match
			if (arginfo != null && oldarginfo != null && stringarginfo != null && oldstringarginfo != null && ArgumentInfosMatch(arginfo, oldarginfo) && ArgumentInfosMatch(stringarginfo, oldstringarginfo)) return;

			// Change the argument descriptions
			this.BeginUpdate();

			for (int i = 0; i < args.Length; i++)
				UpdateArgument(args[i], labels[i], arginfo[i]);

			for (int i = 0; i < stringargs.Length; i++)
				UpdateStringArgument(stringargs[i], stringlabels[i], stringarginfo[i]);

			if (!setuponly)
			{
				// Apply action's default arguments
				if (showaction != 0)
				{
					for (int i = 0; i < args.Length; i++)
						args[i].SetDefaultValue();
				}
				else //or set them to 0
				{
					for (int i = 0; i < args.Length; i++)
						args[i].SetValue(0);
				}

				for (int i = 0; i < stringargs.Length; i++)
					stringargs[i].Text = string.Empty;
			}

			// Store current action
			this.action = showaction;

			this.EndUpdate();
		}

		public void UpdateThingType(ThingTypeInfo info)
		{
			// Update arguments
			ArgumentInfo[] oldarginfo = (arginfo != null ? (ArgumentInfo[])arginfo.Clone() : null); //mxd
			ArgumentInfo[] oldstringarginfo = (stringarginfo != null ? (ArgumentInfo[])stringarginfo.Clone() : null);

			// Update argument infos
			if (info != null)
			{
				arginfo = info.Args;
				stringarginfo = info.StringArgs;
			}
			else
			{
				arginfo = General.Map.Config.LinedefActions[0].Args;
				stringarginfo = General.Map.Config.LinedefActions[0].Args;
			}

			//mxd. Don't update args when old and new argument infos match
			if (arginfo != null && oldarginfo != null && stringarginfo != null && oldstringarginfo != null && ArgumentInfosMatch(arginfo, oldarginfo) && ArgumentInfosMatch(stringarginfo, oldstringarginfo)) return;

			// Change the argument descriptions
			this.BeginUpdate();

			for (int i = 0; i < args.Length; i++)
				UpdateArgument(args[i], labels[i], arginfo[i]);

			for (int i = 0; i < stringargs.Length; i++)
				UpdateStringArgument(stringargs[i], stringlabels[i], stringarginfo[i]);

			// Apply thing's default arguments
			if (info != null)
			{
				for (int i = 0; i < args.Length; i++)
					args[i].SetDefaultValue();
			}
			else //or set them to 0
			{
				for (int i = 0; i < args.Length; i++)
					args[i].SetValue(0);
			}

			for (int i = 0; i < stringargs.Length; i++)
				stringargs[i].Text = string.Empty;

			this.EndUpdate();
		}

		public void UpdateScriptControls()
		{
		}

		private void UpdateArgument(ArgumentBox arg, Label label, ArgumentInfo info)
		{
			// Update labels
			label.Text = info.Title + ":";
			label.Enabled = info.Used;
			arg.ForeColor = (label.Enabled ? SystemColors.WindowText : SystemColors.GrayText);
			arg.Setup(info);

			// Update tooltip
			UpdateToolTip(label, info);
		}
		private void UpdateStringArgument(TextBox arg, Label label, ArgumentInfo info)
		{
			// Update labels
			label.Text = info.Title + ":";
			label.Enabled = info.Used;
			arg.ForeColor = (label.Enabled ? SystemColors.WindowText : SystemColors.GrayText);

			// Update tooltip
			UpdateToolTip(label, info);
		}

		private void UpdateToolTip(Label label, ArgumentInfo info)
		{
			if (info.Used && !string.IsNullOrEmpty(info.ToolTip))
			{
				tooltip.SetToolTip(label, info.ToolTip);
				label.Font = new Font(label.Font, FontStyle.Underline);
				label.ForeColor = SystemColors.HotTrack;
			}
			else
			{
				tooltip.SetToolTip(label, null);
				label.Font = new Font(label.Font, FontStyle.Regular);
				label.ForeColor = SystemColors.WindowText;
			}
		}

		//mxd
		private static bool ArgumentInfosMatch(ArgumentInfo[] info1, ArgumentInfo[] info2)
		{
			if (info1.Length != info2.Length) return false;
			bool haveusedargs = false; // Arguments should still be reset if all arguments are unused

			for (int i = 0; i < info1.Length; i++)
			{
				if (info1[i].Used != info2[i].Used || info1[i].Type != info2[i].Type
					|| info1[i].Title.ToUpperInvariant() != info2[i].Title.ToUpperInvariant())
					return false;

				haveusedargs |= info1[i].Used;
			}

			return haveusedargs;
		}

		#endregion

		#region ================== Redraw control

		private void BeginUpdate()
		{
			SendMessage(this.Parent.Handle, WM_SETREDRAW, false, 0);
		}

		private void EndUpdate()
		{
			SendMessage(this.Parent.Handle, WM_SETREDRAW, true, 0);
			this.Parent.Refresh();
		}

		#endregion
	}
}
