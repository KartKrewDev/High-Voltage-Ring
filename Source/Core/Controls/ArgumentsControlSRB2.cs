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
using System.Diagnostics.Eventing.Reader;

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

        #region ================== Enums

        private enum ArgMode
        {
            INT,
            STRING,
            SCRIPT,
        }

        #endregion

        #region ================== Variables

        private int action;
		private ArgumentInfo[] arginfo;
		private Label[] labels;
		private ArgumentBox[] args;
        private TextBox[] stringargs;
        private ColoredComboBox[] scriptargs;
        private CheckBox[] stringargcb;
        private ArgMode[] argmodes;
        private string[] argstrval;
        private bool[] haveargstr;

        #endregion

        #region ================== Constructor

        public ArgumentsControlSRB2()
		{
			InitializeComponent();

			Reset();

			labels = new Label[] { arg0label, arg1label, arg2label, arg3label, arg4label, arg5label, arg6label, arg7label, arg8label, arg9label };
			args = new ArgumentBox[] { arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9 };
			stringargs = new System.Windows.Forms.TextBox[] { stringarg0, stringarg1 };
            scriptargs = new ColoredComboBox[] { scriptarg0 };
            stringargcb = new System.Windows.Forms.CheckBox[] { stringargcb0, stringargcb1 };

			argmodes = new ArgMode[] { ArgMode.INT, ArgMode.INT };
			argstrval = new string[] { string.Empty, string.Empty };
            haveargstr = new bool[] { false, false };
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

		public void SetValue(Sector s, bool first)
		{
			SetValue(s.Fields, s.Args, first);
		}

		private void SetValue(UniFields fields, int[] newargs, bool first)
		{
			if (first)
			{
				// Update arguments
				for (int i = 0; i < args.Length; i++)
				{
					if (i < stringargs.Length)
					{
                        argstrval[i] = fields.GetValue("stringarg" + i, string.Empty);
						haveargstr[i] = !string.IsNullOrEmpty(argstrval[i]);
					}

					args[i].SetValue(newargs[i]);
				}
			}
			else
			{
                // Update arguments
                for (int i = 0; i < args.Length; i++)
                {
                    if (i < stringargs.Length)
                    {
                        if (argstrval[i] != fields.GetValue("stringarg" + i, string.Empty))
						{
                            haveargstr[i] = true;
                            argstrval[i] = string.Empty;
                        }
                    }

                    if (!string.IsNullOrEmpty(args[i].Text) && newargs[i] != args[i].GetResult(int.MinValue))
						args[i].ClearValue();
                }
			}
		}

		#endregion

		#region ================== Apply

		public void Apply(Linedef l, int step)
		{
            Apply(l.Fields, l.Args, step);
        }

		public void Apply(Thing t, int step)
		{
            Apply(t.Fields, t.Args, step);
        }

		public void Apply(Sector s, int step)
		{
			Apply(s.Fields, s.Args, step);
		}

        private void Apply(UniFields fields, int[] newargs, int step)
        {
            for (int i = 0; i < args.Length; i++)
			{
                ArgMode mode = ArgMode.INT;
				if (i < argmodes.Length)
				{
					mode = argmodes[i];
				}

                switch (mode)
                {
                    case ArgMode.SCRIPT:
                        if (i >= scriptargs.Length)
                        {
                            goto case ArgMode.STRING;
                        }

                        if (!string.IsNullOrEmpty(scriptargs[i].Text))
                            UniFields.SetString(fields, "stringarg" + i, scriptargs[i].Text, string.Empty);
                        break;

                    case ArgMode.STRING:
                        if (i >= stringargs.Length)
                        {
							goto case ArgMode.INT;
                        }

						if (!string.IsNullOrEmpty(stringargs[i].Text))
							UniFields.SetString(fields, "stringarg" + i, stringargs[i].Text, string.Empty);
                        break;

                    case ArgMode.INT:
						if (i < stringargs.Length)
						{
                            if (fields.ContainsKey("stringarg" + i))
								fields.Remove("stringarg" + i);
                        }
						newargs[i] = args[i].GetResult(newargs[i], step);
                        break;

                    default: throw new NotImplementedException("Unknown ArgMode");
                }
            }
        }

        #endregion

        #region ================== Update

		public void UpdateAction(int action, bool setuponly)
		{
			// Update arguments
			int showaction = 0;
			ArgumentInfo[] oldarginfo = (arginfo != null ? (ArgumentInfo[])arginfo.Clone() : null); //mxd

			// Only when action type is known
			if (General.Map.Config.LinedefActions.ContainsKey(action)) showaction = action;

			// Update argument infos
			arginfo = General.Map.Config.LinedefActions[showaction].Args;

			//mxd. Don't update action args when old and new argument infos match
			if (arginfo != null && oldarginfo != null
				&& ArgumentInfosMatch(arginfo, oldarginfo))
			{
				return;
			}

			// Change the argument descriptions
			this.BeginUpdate();

			for (int i = 0; i < args.Length; i++)
				UpdateArgument(args[i], labels[i], arginfo[i]);

			if (!setuponly)
			{
				// Apply action's or thing's default arguments
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
				{
                    stringargs[i].Text = argstrval[i] = " ";

                    if (i < scriptargs.Length)
					{
                        scriptargs[i].Text = " ";
                    }
                }
            }

			// Store current action
			this.action = showaction;

			this.EndUpdate();
		}

		public void UpdateScriptControls()
		{
			for (int i = 0; i < stringargs.Length; i++)
			{
				if (arginfo[i].Script != 0 && i < scriptargs.Length)
				{
					// It's a SCRIPT!
					scriptargs[i].Items.Clear();
					scriptargs[i].Location = new Point(args[i].Location.X, args[i].Location.Y + 2);

                    foreach (ScriptItem nsi in General.Map.NamedScripts.Values)
						scriptargs[i].Items.Add(new ColoredComboBoxItem(nsi, nsi.IsInclude ? SystemColors.HotTrack : SystemColors.WindowText));
					scriptargs[i].DropDownWidth = Tools.GetDropDownWidth(scriptargs[i]);

					stringargcb[i].Visible = false;
					stringargcb[i].Checked = true;

					scriptargs[i].Visible = true;
					stringargs[i].Visible = false;
					args[i].Visible = false;

					argmodes[i] = ArgMode.SCRIPT;
					stringargs[i].Text = scriptargs[i].Text = argstrval[i];

					if (General.Map.NamedScripts.ContainsKey(argstrval[i]))
						UpdateScriptArguments(General.Map.NamedScripts[argstrval[i]]);
				}
				else if (arginfo[i].Str)
				{
                    // It's a string
                    stringargs[i].Clear();
                    stringargs[i].Location = new Point(args[i].Location.X, args[i].Location.Y + 2);

                    stringargcb[i].Visible = false;
					stringargcb[i].Checked = true;

					if (i < scriptargs.Length)
					{
                        scriptargs[i].Visible = false;
                    }
					stringargs[i].Visible = true;
					args[i].Visible = false;

					argmodes[i] = ArgMode.STRING;
					stringargs[i].Text = argstrval[i];
				}
				else if (arginfo[i].Used)
				{
					// It's an integer
					stringargcb[i].Visible = false;
					stringargcb[i].Checked = false;

                    if (i < scriptargs.Length)
                    {
                        scriptargs[i].Visible = false;
                    }
                    stringargs[i].Visible = false;
					args[i].Visible = true;

					argmodes[i] = ArgMode.INT;
				}
				else
				{
                    // YOU DECIDE!
                    stringargs[i].Clear();
                    stringargs[i].Location = new Point(args[i].Location.X, args[i].Location.Y + 2);

                    stringargcb[i].Visible = true;
					stringargcb[i].Checked = haveargstr[i];

                    argmodes[i] = (haveargstr[i] ? ArgMode.STRING : ArgMode.INT);

                    if (i < scriptargs.Length)
                    {
                        scriptargs[i].Visible = false;
                    }
                    stringargs[i].Visible = (argmodes[i] == ArgMode.STRING);
					args[i].Visible = (argmodes[i] == ArgMode.INT);

                    stringargs[i].Text = argstrval[i];
                }
			}
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

        private void UpdateScriptArguments(ScriptItem item)
        {
            if (item != null)
            {
                string[] argnames = item.GetArgumentsDescriptionsSRB2((arginfo[0].Script == 1));
                for (int i = 1; i < labels.Length; i++)
                {
                    if (!string.IsNullOrEmpty(argnames[i]))
                    {
                        labels[i].Text = argnames[i] + ":";
                        labels[i].Enabled = true;
                        labels[i].Font = new Font(labels[i].Font, FontStyle.Regular);
                        labels[i].ForeColor = SystemColors.WindowText;
                    }
                    else
                    {
                        labels[i].Text = arginfo[i].Title + ":";
                        labels[i].Enabled = arginfo[i].Used;
                        UpdateToolTip(labels[i], arginfo[i]);
                    }

                    args[i].ForeColor = (labels[i].Enabled ? SystemColors.WindowText : SystemColors.GrayText);
                }
            }
            else
            {
                for (int i = 1; i < labels.Length; i++)
                {
                    labels[i].Text = arginfo[i].Title + ":";
                    labels[i].Enabled = arginfo[i].Used;
                    UpdateToolTip(labels[i], arginfo[i]);
                    args[i].ForeColor = (labels[i].Enabled ? SystemColors.WindowText : SystemColors.GrayText);
                }
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

        #region ================== Events

        private void stringargcb_CheckedChanged(int i)
		{
			if (!stringargcb[i].Visible)
			{
				return;
			}

            argmodes[i] = (stringargcb[i].Checked ? ArgMode.STRING : ArgMode.INT);

            stringargs[i].Visible = (argmodes[i] == ArgMode.STRING);
            args[i].Visible = (argmodes[i] == ArgMode.INT);
        }

        private void stringargcb0_CheckedChanged(object sender, EventArgs e)
        {
            stringargcb_CheckedChanged(0);
        }

        private void stringargcb1_CheckedChanged(object sender, EventArgs e)
        {
            stringargcb_CheckedChanged(1);
        }

        private void scriptarg0_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(scriptarg0.Text)) return;
            ScriptItem item = null;
            if (scriptarg0.SelectedIndex != -1)
            {
                item = ((ScriptItem)((ColoredComboBoxItem)scriptarg0.SelectedItem).Value);
            }
            else
            {
                string scriptname = scriptarg0.Text.Trim().ToLowerInvariant();
                if (General.Map.NamedScripts.ContainsKey(scriptname))
                    item = General.Map.NamedScripts[scriptname];
            }

            UpdateScriptArguments(item);
        }

        #endregion
    }
}
