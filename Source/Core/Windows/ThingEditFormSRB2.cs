
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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Controls;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Types;

#endregion

namespace CodeImp.DoomBuilder.Windows
{
	/// <summary>
	/// Dialog window that allows viewing and editing of Thing properties.
	/// </summary>
	internal partial class ThingEditFormSRB2 : DelayedForm, IThingEditForm
	{
		#region ================== Events

		public event EventHandler OnValuesChanged; //mxd

		#endregion

		#region ================== Variables

		private ICollection<Thing> things;
		private ThingTypeInfo thinginfo;
		private bool preventchanges;
		private bool preventmapchange; //mxd
		private bool undocreated; //mxd
		private List<ThingProperties> thingprops; //mxd
		private Dictionary<string, string> flagsrename; //mxd
		private bool oldmapischanged;

		//mxd. Persistent settings
		private bool useabsoluteheight;

		private struct ThingProperties //mxd
		{
			//public readonly int Type;
			public readonly int AngleDoom;
			public readonly int Pitch;
			public readonly int Roll;
			public readonly double Scale;
			public readonly double Alpha;
			public readonly double X;
			public readonly double Y;
			public readonly double Z;
			public readonly Dictionary<string, bool> Flags;

			public ThingProperties(Thing t) 
			{
				X = t.Position.x;
				Y = t.Position.y;
				Z = t.Position.z;
				//Type = t.Type;
				AngleDoom = t.AngleDoom;
				Pitch = t.Pitch;
				Roll = t.Roll;
				Scale = UniFields.GetFloat(t.Fields, "scale", 1.0f);
				Alpha = UniFields.GetFloat(t.Fields, "alpha", 1.0f);
				Flags = t.GetFlags();
			}
		}

		#endregion

		#region ================== Constructor

		// Constructor
		public ThingEditFormSRB2() 
		{
			// Initialize
			InitializeComponent();

			//mxd. Load settings
			useabsoluteheight = General.Settings.ReadSetting("windows." + configname + ".useabsoluteheight", false);

			//mxd. Widow setup
			if(General.Settings.StoreSelectedEditTab)
			{
				int activetab = General.Settings.ReadSetting("windows." + configname + ".activetab", 0);
				tabs.SelectTab(activetab);
			}

			// Fill flags list
			foreach(KeyValuePair<string, string> tf in General.Map.Config.ThingFlags)
				flags.Add(tf.Value, tf.Key);
			flags.Enabled = (General.Map.Config.ThingFlags.Count > 0);

			// Initialize custom fields editor
			fieldslist.Setup("thing");

			// Fill universal fields list
			fieldslist.ListFixedFields(General.Map.Config.ThingFields);

			//mxd. Show fixed fields?
			hidefixedfields.Checked = !General.Settings.ReadSetting("windows." + configname + ".customfieldsshowfixed", true);

			// Thing height?
			posZ.Visible = General.Map.FormatInterface.HasThingHeight;
			zlabel.Visible = General.Map.FormatInterface.HasThingHeight;
			cbAbsoluteHeight.Visible = General.Map.FormatInterface.HasThingHeight; //mxd

			//mxd. Decimals allowed?
			if(General.Map.FormatInterface.VertexDecimals > 0) 
			{
				posX.AllowDecimal = true;
				posY.AllowDecimal = true;
				posZ.AllowDecimal = true;
			}

			//mxd. Use doom angle clamping?
			anglecontrol.DoomAngleClamping = General.Map.Config.DoomThingRotationAngles;

			// Setup types list
			thingtype.Setup();
		}

		#endregion

		#region ================== Methods

		// This sets up the form to edit the given things
		public void Setup(ICollection<Thing> things) 
		{
			preventchanges = true;
			oldmapischanged = General.Map.IsChanged;
            undocreated = false;
            argscontrol.Reset();

            // Keep this list
            this.things = things;
			if(things.Count > 1) this.Text = "Edit Things (" + things.Count + ")";
			hint.Visible = things.Count > 1; //mxd
			hintlabel.Visible = things.Count > 1; //mxd
			thingtype.UseMultiSelection = things.Count > 1; //mxd

			////////////////////////////////////////////////////////////////////////
			// Set all options to the first thing properties
			////////////////////////////////////////////////////////////////////////

			Thing ft = General.GetByIndex(things, 0);

			// Set type
			thingtype.SelectType(ft.Type);

			// Flags
			foreach(CheckBox c in flags.Checkboxes) 
			{
				if(ft.Flags.ContainsKey(c.Tag.ToString())) c.Checked = ft.Flags[c.Tag.ToString()];
			}

			// Coordination
			angle.Text = ft.AngleDoom.ToString();
			cbAbsoluteHeight.Checked = useabsoluteheight; //mxd

			//mxd
			ft.DetermineSector();
			double floorheight = (ft.Sector != null ? Sector.GetFloorPlane(ft.Sector).GetZ(ft.Position) : 0);
			posX.Text = (ft.Position.x).ToString();
			posY.Text = (ft.Position.y).ToString();
			posZ.Text = (useabsoluteheight ? (Math.Round(ft.Position.z + floorheight, General.Map.FormatInterface.VertexDecimals)).ToString() : (ft.Position.z).ToString());
			posX.ButtonStep = General.Map.Grid.GridSize;
			posY.ButtonStep = General.Map.Grid.GridSize;
			posZ.ButtonStep = General.Map.Grid.GridSize;

			//mxd. User vars. Should be done before adding regular fields
			ThingTypeInfo fti = General.Map.Data.GetThingInfoEx(ft.Type);
			if (fti != null && fti.Actor != null)
			{
				Dictionary<string, UniversalType> uservars = fti.Actor.GetAllUserVars();
				Dictionary<string, object> uservardefaults = fti.Actor.GetAllUserVarDefaults();

				if (uservars.Count > 0)
					fieldslist.SetUserVars(uservars, uservardefaults, ft.Fields, true);
			}
			thinginfo = fti; //mxd

			// Custom fields
			fieldslist.SetValues(ft.Fields, true);
			commenteditor.SetValues(ft.Fields, true);
			scale.Text = ft.Fields.GetValue("scale", 1.0).ToString();
			pitch.Text = ft.Pitch.ToString();
			roll.Text = ft.Roll.ToString();

			// Action/tags
			tagSelector.Setup(UniversalType.ThingTag);
			tagSelector.SetTag(ft.Tag);

			//mxd. Args
			argscontrol.UpdateThingType(thinginfo);
			argscontrol.SetValue(ft, true);

			////////////////////////////////////////////////////////////////////////
			// Now go for all lines and change the options when a setting is different
			////////////////////////////////////////////////////////////////////////

			thingprops = new List<ThingProperties>();

			// Go for all things
			foreach(Thing t in things) 
			{
				//mxd. Update sector info
				t.DetermineSector();

				// Type does not match?
				ThingTypeInfo info = thingtype.GetSelectedInfo(); //mxd
				if(info != null && info.Index != t.Type)
				{
					thingtype.ClearSelectedType();
					thinginfo = null; //mxd
				}

				// Flags
				foreach(CheckBox c in flags.Checkboxes) 
				{
					if(c.CheckState == CheckState.Indeterminate) continue; //mxd
					if(t.IsFlagSet(c.Tag.ToString()) != c.Checked) 
					{
						c.ThreeState = true;
						c.CheckState = CheckState.Indeterminate;
					}
				}

				// Coordination
				if(t.AngleDoom.ToString() != angle.Text) angle.Text = "";

				//mxd. Position
				if((t.Position.x).ToString() != posX.Text) posX.Text = "";
				if((t.Position.y).ToString() != posY.Text) posY.Text = "";
				if(useabsoluteheight && t.Sector != null) 
				{
					if((Math.Round(Sector.GetFloorPlane(t.Sector).GetZ(t.Position) + t.Position.z, General.Map.FormatInterface.VertexDecimals)).ToString() != posZ.Text)
						posZ.Text = "";
				} 
				else if((t.Position.z).ToString() != posZ.Text) 
				{
					posZ.Text = "";
				}

				// Tags
				if(t.Tag != ft.Tag) tagSelector.ClearTag(); //mxd

				//mxd. Arguments
				argscontrol.SetValue(t, false);

				//mxd. User vars. Should be done before adding regular fields
				ThingTypeInfo ti = General.Map.Data.GetThingInfoEx(t.Type);
				if (ti != null && ti.Actor != null)
				{
					Dictionary<string, UniversalType> uservars = ti.Actor.GetAllUserVars();
					Dictionary<string, object> uservardefaults = ti.Actor.GetAllUserVarDefaults();

					if (uservars.Count > 0)
						fieldslist.SetUserVars(uservars, uservardefaults, t.Fields, false);
				}

				//mxd. Custom fields
				fieldslist.SetValues(t.Fields, false);
				commenteditor.SetValues(t.Fields, false); //mxd. Comments
				if (!string.IsNullOrEmpty(scale.Text))
					UniFields.SetFloat(t.Fields, "scale", scale.GetResultFloat(t.Fields.GetValue("scale", 1.0)), 1.0);

				if(t.Pitch.ToString() != pitch.Text) pitch.Text = "";
				if(t.Roll.ToString() != roll.Text) roll.Text = "";

				//mxd. Store initial properties
				thingprops.Add(new ThingProperties(t));
			}

			preventchanges = false;

			//mxd. Trigger updates manually...
			preventmapchange = true;
			angle_WhenTextChanged(angle, EventArgs.Empty);
			pitch_WhenTextChanged(pitch, EventArgs.Empty);
			roll_WhenTextChanged(roll, EventArgs.Empty);
			flags_OnValueChanged(flags, EventArgs.Empty);
			preventmapchange = false;

			argscontrol.UpdateScriptControls(); //mxd
			commenteditor.FinishSetup(); //mxd
			UpdateFlagNames(); //mxd
		}

		//mxd
		private void MakeUndo() 
		{
			if(undocreated) return;
			undocreated = true;

			//mxd. Make undo
			General.Map.UndoRedo.CreateUndo("Edit " + (things.Count > 1 ? things.Count + " things" : "thing"));
			foreach(Thing t in things) t.Fields.BeforeFieldsChange();
		}

		//mxd
		private void UpdateFlagNames()
		{
			Dictionary<string, string> newflagsrename = (thinginfo != null ? thinginfo.FlagsRename : null);
			
			// Update flag names?
			if(flagsrename != null || newflagsrename != null)
			{
				flags.SuspendLayout();

				// Restore default flags?
				if(flagsrename != null)
				{
					foreach(CheckBox cb in flags.Checkboxes)
					{
						string flag = cb.Tag.ToString();
						if(flagsrename.ContainsKey(flag))
						{
							cb.Text = General.Map.Config.ThingFlags[flag];
							cb.ForeColor = SystemColors.WindowText;
						}
					}
				}

				// Apply new renaming?
				if(newflagsrename != null)
				{
					foreach(CheckBox cb in flags.Checkboxes)
					{
						string flag = cb.Tag.ToString();
						if(newflagsrename.ContainsKey(flag))
						{
							cb.Text = newflagsrename[flag];
							cb.ForeColor = SystemColors.HotTrack;
						}
					}
				}

				flags.ResumeLayout();
			}

			// Store current flag names
			flagsrename = newflagsrename;
		}

		#endregion

		#region ================== Events

		//mxd
		private void thingtype_OnTypeDoubleClicked() 
		{
			apply_Click(this, EventArgs.Empty);
		}

		// Angle text changes
		private void angle_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			preventchanges = true;
			anglecontrol.Angle = angle.GetResult(AngleControlEx.NO_ANGLE);
			preventchanges = false;
			if(!preventmapchange) ApplyAngleChange(); //mxd
		}

		//mxd. Angle control clicked
		private void anglecontrol_AngleChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			angle.Text = anglecontrol.Angle.ToString();
			if(!preventmapchange) ApplyAngleChange();
		}

		private void pitch_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			int p = pitch.GetResult(AngleControlEx.NO_ANGLE);
			preventchanges = true;
			pitchControl.Angle = (p == AngleControlEx.NO_ANGLE ? p : p + 90);
			preventchanges = false;
			if(!preventmapchange) ApplyPitchChange();
		}

		private void pitchControl_AngleChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			pitch.Text = (General.ClampAngle(pitchControl.Angle - 90)).ToString();
			if(!preventmapchange) ApplyPitchChange();
		}

		private void roll_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			int r = roll.GetResult(AngleControlEx.NO_ANGLE);
			preventchanges = true;
			rollControl.Angle = (r == AngleControlEx.NO_ANGLE ? r : r + 90);
			preventchanges = false;
			if(!preventmapchange) ApplyRollChange();
		}

		private void rollControl_AngleChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			roll.Text = (General.ClampAngle(rollControl.Angle - 90)).ToString();
			if(!preventmapchange) ApplyRollChange();
		}

		// Apply clicked
		private void apply_Click(object sender, EventArgs e) 
		{
			//mxd. Make Undo
			MakeUndo();
			
			List<string> defaultflags = new List<string>();

			// Verify the tag
			if(General.Map.FormatInterface.HasThingTag) //mxd
			{
				tagSelector.ValidateTag();//mxd
				if(((tagSelector.GetTag(0) < General.Map.FormatInterface.MinTag) || (tagSelector.GetTag(0) > General.Map.FormatInterface.MaxTag))) 
				{
					General.ShowWarningMessage("Thing tag must be between " + General.Map.FormatInterface.MinTag + " and " + General.Map.FormatInterface.MaxTag + ".", MessageBoxButtons.OK);
					return;
				}
			}

			// Verify the type
			if(!string.IsNullOrEmpty(thingtype.TypeStringValue) && ((thingtype.GetResult(0) < General.Map.FormatInterface.MinThingType) || (thingtype.GetResult(0) > General.Map.FormatInterface.MaxThingType))) 
			{
				General.ShowWarningMessage("Thing type must be between " + General.Map.FormatInterface.MinThingType + " and " + General.Map.FormatInterface.MaxThingType + ".", MessageBoxButtons.OK);
				return;
			}

			// Go for all the things
			int offset = 0; //mxd
			foreach(Thing t in things) 
			{
				// Coordination
				//mxd. Randomize rotations?
				if(cbrandomangle.Checked)
				{
					int newangle = General.Random(0, 359);
					if(General.Map.Config.DoomThingRotationAngles) newangle = newangle / 45 * 45;
					t.Rotate(newangle);
				}
				if(cbrandompitch.Checked) t.SetPitch(General.Random(0, 359));
				if(cbrandomroll.Checked) t.SetRoll(General.Random(0, 359));

				//mxd. Check position
				double px = General.Clamp(t.Position.x, General.Map.Config.LeftBoundary, General.Map.Config.RightBoundary);
				double py = General.Clamp(t.Position.y, General.Map.Config.BottomBoundary, General.Map.Config.TopBoundary);
				if(t.Position.x != px || t.Position.y != py) t.Move(new Vector2D(px, py));

				// Tags
				t.Tag = General.Clamp(tagSelector.GetSmartTag(t.Tag, offset), General.Map.FormatInterface.MinTag, General.Map.FormatInterface.MaxTag); //mxd

				//mxd. Apply args
				argscontrol.Apply(t, offset);

				//mxd. Custom fields
				fieldslist.Apply(t.Fields);

				//mxd. User vars. Should be called after fieldslist.Apply()
				ThingTypeInfo ti = General.Map.Data.GetThingInfoEx(t.Type);
				if (ti != null && ti.Actor != null)
				{
					Dictionary<string, UniversalType> uservars = ti.Actor.GetAllUserVars();

					if(uservars.Count > 0)
						fieldslist.ApplyUserVars(uservars, t.Fields);
				}

				//mxd. Comments
				commenteditor.Apply(t.Fields);

				// Update settings
				t.UpdateConfiguration();

				//mxd. Increase offset...
				offset++;
			}

			// Set as defaults
			foreach(CheckBox c in flags.Checkboxes) 
			{
				if(c.CheckState == CheckState.Checked) defaultflags.Add(c.Tag.ToString());
			}
			General.Settings.DefaultThingType = thingtype.GetResult(General.Settings.DefaultThingType);
			General.Settings.DefaultThingAngle = Angle2D.DegToRad((float)angle.GetResult((int)Angle2D.RadToDeg(General.Settings.DefaultThingAngle) - 90) + 90);
			General.Settings.SetDefaultThingFlags(defaultflags);

			// Done
			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty); //mxd
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		// Cancel clicked
		private void cancel_Click(object sender, EventArgs e) 
		{
			//mxd. Perform undo?
			if (undocreated)
			{
				General.Map.UndoRedo.WithdrawUndo();

				// Changing certain properties of the sector, like its type, will set General.Map.IsChanged to true.
				// But if cancel is pressed and the changes are discarded, and the map was not changed before, we have to force
				// General.Map.IsChanged back to false
				if (General.Map.IsChanged && oldmapischanged == false)
					General.Map.ForceMapIsChangedFalse();
			}

			// Be gone
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		//mxd
		private void cbAbsoluteHeight_CheckedChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo();

			useabsoluteheight = cbAbsoluteHeight.Checked;

			preventchanges = true;

			//update label text
			Thing ft = General.GetByIndex(things, 0);
			double z = ft.Position.z;
			if(useabsoluteheight && ft.Sector != null) z += Sector.GetFloorPlane(ft.Sector).GetZ(ft.Position);
			posZ.Text = Math.Round(z, General.Map.FormatInterface.VertexDecimals).ToString();

			foreach(Thing t in things) 
			{
				z = t.Position.z;
				if(useabsoluteheight && t.Sector != null) z += Sector.GetFloorPlane(t.Sector).GetZ(t.Position);
				string ztext = Math.Round(z, General.Map.FormatInterface.VertexDecimals).ToString();
				if(posZ.Text != ztext) 
				{
					posZ.Text = "";
					break;
				}
			}

			preventchanges = false;
		}

		//mxd
		private void tabcustom_MouseEnter(object sender, EventArgs e) 
		{
			fieldslist.Focus();
		}

		//mxd
		private void ThingEditFormUDMF_Shown(object sender, EventArgs e)
		{
			if(tabs.SelectedIndex == 0)
			{
				thingtype.Focus();
				thingtype.FocusTextbox();
			}
		}

		//mxd
		private void ThingEditForm_FormClosing(object sender, FormClosingEventArgs e) 
		{
			// Save settings
			General.Settings.WriteSetting("windows." + configname + ".activetab", tabs.SelectedIndex);
			General.Settings.WriteSetting("windows." + configname + ".useabsoluteheight", useabsoluteheight);
			General.Settings.WriteSetting("windows." + configname + ".customfieldsshowfixed", !hidefixedfields.Checked);
		}

		// Help
		private void ThingEditForm_HelpRequested(object sender, HelpEventArgs hlpevent) 
		{
			General.ShowHelp("w_thingedit.html");
			hlpevent.Handled = true;
		}

		#endregion

		#region ================== mxd. Realtime events

		private void posX_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd
			int i = 0;

			// Reset increment steps, otherwise it's just keep counting and counting
			posX.ResetIncrementStep();

			// Update values
			foreach (Thing t in things)
				t.Move(new Vector2D(posX.GetResultFloat(thingprops[i++].X), t.Position.y));

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void posY_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd
			int i = 0;

			// Reset increment steps, otherwise it's just keep counting and counting
			posY.ResetIncrementStep();

			// Update values
			foreach (Thing t in things)
				t.Move(new Vector2D(t.Position.x, posY.GetResultFloat(thingprops[i++].Y)));

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void posZ_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd
			int i = 0;

			// Reset increment steps, otherwise it's just keep counting and counting
			posZ.ResetIncrementStep();

			if (string.IsNullOrEmpty(posZ.Text)) 
			{
				// Restore values
				foreach(Thing t in things)
					t.Move(new Vector3D(t.Position.x, t.Position.y, thingprops[i++].Z));
			} 
			else 
			{
				// Update values
				foreach(Thing t in things) 
				{
					double z = posZ.GetResultFloat(thingprops[i++].Z);
					if(useabsoluteheight && !posZ.CheckIsRelative() && t.Sector != null)
						z -= Math.Round(Sector.GetFloorPlane(t.Sector).GetZ(t.Position.x, t.Position.y), General.Map.FormatInterface.VertexDecimals);
					t.Move(new Vector3D(t.Position.x, t.Position.y, z));
				}
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void scale_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd
			int i = 0;

			foreach (Thing t in things)
			{
				double s = scale.GetResultFloat(thingprops[i].Scale);
				if (s == 0) s = 1.0f;
				UniFields.SetFloat(t.Fields, "scale", s);
				t.SetScale(s, s);
				i++;
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		// Selected type changes
		private void thingtype_OnTypeChanged(ThingTypeInfo value) 
		{
			thinginfo = value;

			//mxd. Update things
			if(preventchanges ||
					(!string.IsNullOrEmpty(thingtype.TypeStringValue) && 
					thingtype.GetResult(0) < General.Map.FormatInterface.MinThingType 
					|| thingtype.GetResult(0) > General.Map.FormatInterface.MaxThingType))
				return;

			MakeUndo(); //mxd

			foreach(Thing t in things) 
			{
				//Set type
				t.Type = thingtype.GetResult(t.Type);

				// Update settings
				t.UpdateConfiguration();
			}

			UpdateFlagNames(); //mxd
			argscontrol.UpdateThingType(thinginfo);

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		//mxd
		private void ApplyAngleChange() 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd
			int i = 0;

			// Reset increment steps, otherwise it's just keep counting and counting
			angle.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(angle.Text)) 
			{
				foreach(Thing t in things) t.Rotate(thingprops[i++].AngleDoom);
			} 
			else //update values
			{
				foreach(Thing t in things)
					t.Rotate(angle.GetResult(thingprops[i++].AngleDoom));
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void ApplyPitchChange() 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd
			int i = 0;

			// Reset increment steps, otherwise it's just keep counting and counting
			pitch.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(pitch.Text)) 
			{
				foreach(Thing t in things) t.SetPitch(thingprops[i++].Pitch);
			} 
			else //update values
			{ 
				foreach(Thing t in things)
					t.SetPitch(pitch.GetResult(thingprops[i++].Pitch));
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		//mxd
		private void ApplyRollChange() 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd
			int i = 0;

			// Reset increment steps, otherwise it's just keep counting and counting
			roll.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(roll.Text)) 
			{
				foreach(Thing t in things) t.SetRoll(thingprops[i++].Roll);
			} 
			else //update values
			{ 
				foreach(Thing t in things)
					t.SetRoll(roll.GetResult(thingprops[i++].Roll));
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void flags_OnValueChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			if(!preventmapchange) //mxd
			{
				MakeUndo();
				int i = 0;

				// Apply flags
				foreach(Thing t in things)
				{
					// Apply all flags
					foreach(CheckBox c in flags.Checkboxes)
					{
						if(c.CheckState == CheckState.Checked)
							t.SetFlag(c.Tag.ToString(), true);
						else if(c.CheckState == CheckState.Unchecked)
							t.SetFlag(c.Tag.ToString(), false);
						else if(thingprops[i].Flags.ContainsKey(c.Tag.ToString()))
							t.SetFlag(c.Tag.ToString(), thingprops[i].Flags[c.Tag.ToString()]);
						else //things created in the editor have empty Flags by default
							t.SetFlag(c.Tag.ToString(), false);
					}

					i++;
				}

				// Dispatch event
				General.Map.IsChanged = true;
				if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
			}

			// Gather enabled flags
			HashSet<string> activeflags = new HashSet<string>();
			foreach(CheckBox cb in flags.Checkboxes)
			{
				if(cb.CheckState != CheckState.Unchecked) activeflags.Add(cb.Tag.ToString());
			}

			// Check em
			List<string> warnings = ThingFlagsCompare.CheckFlags(activeflags);
			if(warnings.Count > 0) 
			{
				// Got missing flags
				tooltip.SetToolTip(missingflags, string.Join(Environment.NewLine, warnings.ToArray()));
				missingflags.Visible = true;
				settingsgroup.ForeColor = Color.DarkRed;
				return;
			}

			// Everything is OK
			missingflags.Visible = false;
			settingsgroup.ForeColor = SystemColors.ControlText;
		}

		private void cbrandomangle_CheckedChanged(object sender, EventArgs e)
		{
			angle.Enabled = !cbrandomangle.Checked;
			groupangle.Enabled = !cbrandomangle.Checked;
		}

		private void cbrandompitch_CheckedChanged(object sender, EventArgs e)
		{
			pitch.Enabled = !cbrandompitch.Checked;
			grouppitch.Enabled = !cbrandompitch.Checked;
		}

		private void cbrandomroll_CheckedChanged(object sender, EventArgs e)
		{
			roll.Enabled = !cbrandomroll.Checked;
			grouproll.Enabled = !cbrandomroll.Checked;
		}

		private void hidefixedfields_CheckedChanged(object sender, EventArgs e)
		{
			fieldslist.ShowFixedFields = !hidefixedfields.Checked;
		}

		#endregion

		private void argscontrol_Load(object sender, EventArgs e)
		{

		}

	}
}