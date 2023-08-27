
#region ================== Copyright (c) 2007 Pascal vd Heiden

/*
 * Copyright (c) 2007 Pascal vd Heiden, www.codeimp.com
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

#region ================== Namespaces

using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Data;
using CodeImp.DoomBuilder.GZBuilder.Data;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Types;
using CodeImp.DoomBuilder.GZBuilder; //mxd
using System.Collections.Generic;

#endregion

namespace CodeImp.DoomBuilder.Controls
{
	internal partial class ThingInfoPanelRR : UserControl
    {
        private Label[] arglabels;
        private Label[] args;
        private Label[] stringlabels;
        private Label[] stringargs;

        // Constructor
        public ThingInfoPanelRR()
		{
			// Initialize
			InitializeComponent();
			CodeImp.DoomBuilder.General.ApplyMonoListViewFix(flags);

			arglabels = new Label[] { arglbl1, arglbl2, arglbl3, arglbl4, arglbl5, arglbl6, arglbl7, arglbl8, arglbl9, arglbl10 };
			args = new Label[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10 };

            stringlabels = new Label[] { stringarg1label, stringarg2label };
            stringargs = new Label[] { stringarg1, stringarg2 };
        }

		// This shows the info
		public void ShowInfo(Thing t)
		{
			// Show/hide stuff depending on format
			bool hasArgs = General.Map.FormatInterface.HasThingArgs;
			for (int i = 0; i < args.Length; i++)
			{
				arglabels[i].Visible = hasArgs;
				args[i].Visible = hasArgs;
			}
            for (int i = 0; i < stringargs.Length; i++)
            {
                stringlabels[i].Visible = hasArgs;
                stringargs[i].Visible = hasArgs;
            }

            //mxd
            action.Visible = General.Map.FormatInterface.HasThingAction;
			labelaction.Visible = General.Map.FormatInterface.HasThingAction;

			// Move panel
			spritepanel.Left = infopanel.Left + infopanel.Width + infopanel.Margin.Right + spritepanel.Margin.Left;
			flagsPanel.Left = spritepanel.Left + spritepanel.Width + spritepanel.Margin.Right + flagsPanel.Margin.Left; //mxd
			
			// Lookup thing info
			ThingTypeInfo ti = General.Map.Data.GetThingInfo(t.Type);

			// Get thing action information
			LinedefActionInfo act;
			if(General.Map.Config.LinedefActions.ContainsKey(t.Action)) act = General.Map.Config.LinedefActions[t.Action];
			else if(t.Action == 0) act = new LinedefActionInfo(0, "None", true, false);
			else act = new LinedefActionInfo(t.Action, "Unknown", false, false);
			string actioninfo = act.ToString();
			
			// Determine z info to show
			t.DetermineSector();
			string zinfo;
			if(ti.AbsoluteZ || t.Sector == null)
			{
				zinfo = t.Position.z.ToString(CultureInfo.InvariantCulture) + " (abs.)"; //mxd
			}
			else
			{
				// Hangs from ceiling?
				if(ti.Hangs)
					zinfo = t.Position.z + " (" + Math.Round(Sector.GetCeilingPlane(t.Sector).GetZ(t.Position) - t.Position.z - ti.Height, General.Map.FormatInterface.VertexDecimals).ToString(CultureInfo.InvariantCulture) + ")"; //mxd
				else
					zinfo = t.Position.z + " (" + Math.Round(Sector.GetFloorPlane(t.Sector).GetZ(t.Position) + t.Position.z, General.Map.FormatInterface.VertexDecimals).ToString(CultureInfo.InvariantCulture) + ")"; //mxd
			}

			// Thing info
			infopanel.Text = " Thing " + t.Index + " ";
			type.Text = t.Type + " - " + ti.Title;
			if(ti.IsObsolete) type.Text += " - OBSOLETE"; //mxd
			action.Text = actioninfo;
			bool displayclassname = !string.IsNullOrEmpty(ti.ClassName) && !ti.ClassName.StartsWith("$"); //mxd
			labelclass.Enabled = displayclassname; //mxd
			classname.Enabled = displayclassname; //mxd
			classname.Text = (displayclassname ? ti.ClassName : "--"); //mxd
			position.Text = t.Position.x.ToString(CultureInfo.InvariantCulture) + ", " + t.Position.y.ToString(CultureInfo.InvariantCulture) + ", " + zinfo;
			tag.Text = t.Tag + (General.Map.Options.TagLabels.ContainsKey(t.Tag) ? " - " + General.Map.Options.TagLabels[t.Tag] : string.Empty);
			angle.Text = t.AngleDoom + "\u00B0";
			anglecontrol.Angle = t.AngleDoom;
			anglecontrol.Left = angle.Right + 1;
			
			// Sprite
			if(ti.Sprite.ToLowerInvariant().StartsWith(DataManager.INTERNAL_PREFIX) && (ti.Sprite.Length > DataManager.INTERNAL_PREFIX.Length))
			{
				spritename.Text = "";
				spritetex.Image = General.Map.Data.GetSpriteImage(ti.Sprite).GetSpritePreview();
			}
			else if((ti.Sprite.Length <= 8) && (ti.Sprite.Length > 0))
			{
				spritename.Text = ti.Sprite;
				spritetex.Image = General.Map.Data.GetSpriteImage(ti.Sprite).GetPreview();
			}
			else
			{
				spritename.Text = "";
				spritetex.Image = null;
			}

			// Arguments
			ArgumentInfo[] arginfo = ti.Args; //mxd
            for (int i = 0; i < args.Length; i++)
			{
                //mxd. Set default label colors
                args[i].ForeColor = SystemColors.ControlText;
                arglabels[i].ForeColor = SystemColors.ControlText;

				arglabels[i].Text = arginfo[i].Title + ":";
                arglabels[i].Enabled = arginfo[i].Used;
                args[i].Enabled = arginfo[i].Used;

				SetArgumentText(arginfo[i], args[i], t.ThingArgs[i]);
            }

            ArgumentInfo[] stringarginfo = ti.StringArgs; //mxd
            for (int i = 0; i < stringargs.Length; i++)
            {
                //mxd. Set default label colors
                stringargs[i].ForeColor = SystemColors.ControlText;
                stringlabels[i].ForeColor = SystemColors.ControlText;

                stringlabels[i].Text = stringarginfo[i].Title + ":";
                stringlabels[i].Enabled = stringarginfo[i].Used;
                stringargs[i].Enabled = stringarginfo[i].Used;

				string value = t.Fields.GetValue("thingstringarg" + i, string.Empty);
                SetArgumentText(stringarginfo[i], stringargs[i], value);
            }

            //mxd. Flags
            flags.Items.Clear();
			Dictionary<string, string> flagsrename = ti.FlagsRename;
			foreach(KeyValuePair<string, string> group in General.Map.Config.ThingFlags)
			{
				if(t.Flags.ContainsKey(group.Key) && t.Flags[group.Key])
				{
					ListViewItem lvi = (flagsrename != null && flagsrename.ContainsKey(group.Key)) 
						? new ListViewItem(flagsrename[group.Key]) { ForeColor = SystemColors.HotTrack } 
						: new ListViewItem(group.Value);
					lvi.Checked = true;
					flags.Items.Add(lvi);
				}
			}

			//mxd. Flags panel visibility and size
			flagsPanel.Visible = (flags.Items.Count > 0);
			if(flags.Items.Count > 0)
			{
				Rectangle rect = flags.GetItemRect(0);
				int itemspercolumn = 1;
				
				// Check how many items per column we have...
				for(int i = 1; i < flags.Items.Count; i++)
				{
					if(flags.GetItemRect(i).X != rect.X) break;
					itemspercolumn++;
				}

				flags.Width = rect.Width * (int)Math.Ceiling(flags.Items.Count / (float)itemspercolumn);
				flagsPanel.Width = flags.Width + flags.Left * 2;
			}

			// Show the whole thing
			this.Show();
			//this.Update(); // ano - don't think this is needed, and is slow
		}

		//mxd
		private static void SetArgumentText(ArgumentInfo info, Label label, int value) 
		{
			TypeHandler th = General.Types.GetArgumentHandler(info);
			th.SetValue(value);
			label.Text = th.GetStringValue();
		}

        private static void SetArgumentText(ArgumentInfo info, Label label, string value)
        {
            TypeHandler th = General.Types.GetArgumentHandler(info);
            th.SetValue(value);
            label.Text = th.GetStringValue();
        }

        // When visible changed
        protected override void OnVisibleChanged(EventArgs e)
		{
			// Hiding panels
			if(!this.Visible)
			{
				spritetex.Image = null;
			}

			// Call base
			base.OnVisibleChanged(e);
		}
    }
}
