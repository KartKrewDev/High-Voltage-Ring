#region ================== Namespaces

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Controls;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Rendering;
using CodeImp.DoomBuilder.Types;

#endregion

namespace CodeImp.DoomBuilder.Windows
{
	internal partial class SectorEditFormSRB2 : DelayedForm, ISectorEditForm
	{
		#region ================== Events

		public event EventHandler OnValuesChanged; //mxd

		#endregion

		#region ================== Constants

		private const string NO_DAMAGETYPE = "None"; //mxd
		private const string TRIGGERER_DEFAULT = "Player";

		#endregion

		#region ================== Variables

		private ICollection<Sector> sectors;
		private Dictionary<Sector, SectorProperties> sectorprops; //mxd
		private bool preventchanges; //mxd
		private bool undocreated; //mxd
		private StepsList anglesteps; //mxd
	
		//mxd. Slope pivots
		private Vector2D globalslopepivot;
		private Dictionary<Sector, Vector2D> slopepivots;

		private bool oldmapischanged;

		#endregion

		#region ================== Structs

		private struct SectorProperties //mxd
		{
			public readonly int Brightness;
			public readonly int FloorHeight;
			public readonly int CeilHeight;
			public readonly string FloorTexture;
			public readonly string CeilTexture;

			//UDMF stuff
			public readonly int LightColor;
			public readonly int FadeColor;
			public readonly int LightAlpha;
			public readonly int FadeAlpha;
			public readonly int FadeStart;
			public readonly int FadeEnd;

			//UDMF Ceiling
			public readonly double CeilOffsetX;
			public readonly double CeilOffsetY;
			public readonly double CeilScaleX;
			public readonly double CeilScaleY;
			//public float CeilAlpha;
			public readonly double CeilRotation;
			public readonly int CeilBrightness;
			public readonly bool CeilLightAbsoulte;

			//UDMF Floor
			public readonly double FloorOffsetX;
			public readonly double FloorOffsetY;
			public readonly double FloorScaleX;
			public readonly double FloorScaleY;
			//public float FloorAlpha;
			public readonly double FloorRotation;
			public readonly int FloorBrightness;
			public readonly bool FloorLightAbsoulte;

			//UDMF slopes. Angles are in degrees
			public readonly Vector3D FloorSlope;
			public readonly Vector3D CeilSlope;
			public readonly double FloorSlopeAngleXY;
			public readonly double FloorSlopeAngleZ;
			public readonly double FloorSlopeOffset;
			public readonly double CeilSlopeAngleXY;
			public readonly double CeilSlopeAngleZ;
			public readonly double CeilSlopeOffset;

            public SectorProperties(Sector s) 
			{
				Brightness = s.Brightness;
				FloorHeight = s.FloorHeight;
				CeilHeight = s.CeilHeight;
				FloorTexture = s.FloorTexture;
				CeilTexture = s.CeilTexture;

				//UDMF stuff
				LightColor = UniFields.GetInteger(s.Fields, "lightcolor", PixelColor.INT_WHITE_NO_ALPHA);
				FadeColor = UniFields.GetInteger(s.Fields, "fadecolor", 0);
				LightAlpha = UniFields.GetInteger(s.Fields, "lightalpha", General.Map.Config.MaxColormapAlpha);
				FadeAlpha = UniFields.GetInteger(s.Fields, "fadealpha", General.Map.Config.MaxColormapAlpha);
				FadeStart = UniFields.GetInteger(s.Fields, "fadestart", 0);
				FadeEnd = UniFields.GetInteger(s.Fields, "fadeend", General.Map.Config.NumBrightnessLevels - 1);

				//UDMF Ceiling
				CeilOffsetX = UniFields.GetFloat(s.Fields, "xpanningceiling", 0.0);
				CeilOffsetY = UniFields.GetFloat(s.Fields, "ypanningceiling", 0.0);
				CeilScaleX = UniFields.GetFloat(s.Fields, "xscaleceiling", 1.0);
				CeilScaleY = UniFields.GetFloat(s.Fields, "yscaleceiling", 1.0);
				//CeilAlpha = UniFields.GetFloat(s.Fields, "alphaceiling", 1.0f);
				CeilRotation = s.Fields.GetValue("rotationceiling", 0.0);
				CeilBrightness = s.Fields.GetValue("lightceiling", 0);
				CeilLightAbsoulte = s.Fields.GetValue("lightceilingabsolute", false);

				//UDMF Floor
				FloorOffsetX = UniFields.GetFloat(s.Fields, "xpanningfloor", 0.0);
				FloorOffsetY = UniFields.GetFloat(s.Fields, "ypanningfloor", 0.0);
				FloorScaleX = UniFields.GetFloat(s.Fields, "xscalefloor", 1.0);
				FloorScaleY = UniFields.GetFloat(s.Fields, "yscalefloor", 1.0);
				//FloorAlpha = UniFields.GetFloat(s.Fields, "alphafloor", 1.0f);
				FloorRotation = s.Fields.GetValue("rotationfloor", 0.0);
				FloorBrightness = s.Fields.GetValue("lightfloor", 0);
				FloorLightAbsoulte = s.Fields.GetValue("lightfloorabsolute", false);

				//UDMF slopes
				if(s.FloorSlope.GetLengthSq() > 0)
				{
					FloorSlopeAngleXY = General.ClampAngle(Math.Round(Angle2D.RadToDeg(s.FloorSlope.GetAngleXY()) - 180, 1));
					FloorSlopeAngleZ = -Math.Round(Angle2D.RadToDeg(s.FloorSlope.GetAngleZ()) - 90, 1);
					FloorSlopeOffset = (double.IsNaN(s.FloorSlopeOffset) ? s.FloorHeight : s.FloorSlopeOffset);
				} 
				else 
				{
					FloorSlopeAngleXY = 0;
					FloorSlopeAngleZ = 0;
					FloorSlopeOffset = -s.FloorHeight;
				}
				FloorSlope = s.FloorSlope;

				if(s.CeilSlope.GetLengthSq() > 0)
				{
					CeilSlopeAngleXY = General.ClampAngle(Math.Round(Angle2D.RadToDeg(s.CeilSlope.GetAngleXY()) - 180, 1));
					CeilSlopeAngleZ = -Math.Round(270 - Angle2D.RadToDeg(s.CeilSlope.GetAngleZ()), 1);
					CeilSlopeOffset = (double.IsNaN(s.CeilSlopeOffset) ? s.CeilHeight : s.CeilSlopeOffset);
				} 
				else 
				{
					CeilSlopeAngleXY = 0;
					CeilSlopeAngleZ = 0;
					CeilSlopeOffset = s.CeilHeight;
				}

                CeilSlope = s.CeilSlope;
            }
		}

		#endregion

		#region ================== Constructor

		public SectorEditFormSRB2() 
		{
			InitializeComponent();

			//mxd. Load settings
			if(General.Settings.StoreSelectedEditTab)
			{
				int activetab = General.Settings.ReadSetting("windows." + configname + ".activetab", 0);
				tabs.SelectTab(activetab);
			}

			// Fill flags list
			foreach(KeyValuePair<string, string> lf in General.Map.Config.SectorFlags)
				flags.Add(lf.Value, lf.Key);
			flags.Enabled = General.Map.Config.SectorFlags.Count > 0;

			// Fill damagetype list
			damagetype.Items.Add(NO_DAMAGETYPE);
			damagetype.Items.AddRange(General.Map.Data.DamageTypes);

			//Fill triggerer list
			List<string> ttypes = new List<string>(General.Map.Config.TriggererTypes);
			triggerer.Items.AddRange(ttypes.ToArray());

			// Initialize custom fields editor
			fieldslist.Setup("sector");

			// Fill universal fields list
			fieldslist.ListFixedFields(General.Map.Config.SectorFields);

			// Initialize image selectors
			floortex.Initialize();
			ceilingtex.Initialize();

			// Set steps for brightness field
			brightness.StepValues = General.Map.Config.BrightnessLevels;

			// Apply settings
			ceilScale.LinkValues = General.Settings.ReadSetting("windows." + configname + ".linkceilingscale", false);
			floorScale.LinkValues = General.Settings.ReadSetting("windows." + configname + ".linkfloorscale", false);

			cbUseCeilLineAngles.Checked = General.Settings.ReadSetting("windows." + configname + ".useceillineangles", false);
			cbUseFloorLineAngles.Checked = General.Settings.ReadSetting("windows." + configname + ".usefloorlineangles", false);

			ceilingslopecontrol.UseLineAngles = General.Settings.ReadSetting("windows." + configname + ".useceilslopelineangles", false);
			floorslopecontrol.UseLineAngles = General.Settings.ReadSetting("windows." + configname + ".usefloorslopelineangles", false);

			ceilingslopecontrol.PivotMode = (SlopePivotMode)General.Settings.ReadSetting("windows." + configname + ".ceilpivotmode", (int)SlopePivotMode.LOCAL);
			floorslopecontrol.PivotMode = (SlopePivotMode)General.Settings.ReadSetting("windows." + configname + ".floorpivotmode", (int)SlopePivotMode.LOCAL);

			// Diable brightness controls?
			if(!General.Map.Config.DistinctFloorAndCeilingBrightness)
			{
				ceilBrightness.Enabled = false;
				ceilLightAbsolute.Enabled = false;
				resetceillight.Enabled = false;

				floorBrightness.Enabled = false;
				floorLightAbsolute.Enabled = false;
				resetfloorlight.Enabled = false;
			}

			ceilScale.Enabled = false;
			floorScale.Enabled = false;
		}

		#endregion

		#region ================== Methods

		// This sets up the form to edit the given sectors
		public void Setup(ICollection<Sector> sectors) 
		{
			preventchanges = true; //mxd
			oldmapischanged = General.Map.IsChanged;
			undocreated = false;
            // Keep this list
            this.sectors = sectors;
			if(sectors.Count > 1) this.Text = "Edit Sectors (" + sectors.Count + ")";
			sectorprops = new Dictionary<Sector, SectorProperties>(sectors.Count); //mxd

			//mxd. Set default height offset
			heightoffset.Text = "0";

			CreateHelperProps(sectors); //mxd

			////////////////////////////////////////////////////////////////////////
			// Set all options to the first sector properties
			////////////////////////////////////////////////////////////////////////

			// Get first sector
			Sector sc = General.GetByIndex(sectors, 0);

			// Flags
			foreach(CheckBox c in flags.Checkboxes)
				if(sc.Flags.ContainsKey(c.Tag.ToString())) c.Checked = sc.Flags[c.Tag.ToString()];

			// Effects
			brightness.Text = sc.Brightness.ToString();

			// Floor/ceiling
			floorheight.Text = sc.FloorHeight.ToString();
			ceilingheight.Text = sc.CeilHeight.ToString();
			floortex.TextureName = sc.FloorTexture;
			ceilingtex.TextureName = sc.CeilTexture;

			// UDMF stuff
			// Texture offsets
			ceilOffsets.SetValuesFrom(sc.Fields, true);
			floorOffsets.SetValuesFrom(sc.Fields, true);

			// Texture scale
			ceilScale.SetValuesFrom(sc.Fields, true);
			floorScale.SetValuesFrom(sc.Fields, true);

			// Texture rotation
			double ceilAngle = sc.Fields.GetValue("rotationceiling", 0.0);
			double floorAngle = sc.Fields.GetValue("rotationfloor", 0.0);

			ceilRotation.Text = ceilAngle.ToString();
			floorRotation.Text = floorAngle.ToString();

			ceilAngleControl.Angle = General.ClampAngle(360 - (int)ceilAngle);
			floorAngleControl.Angle = General.ClampAngle(360 - (int)floorAngle);

			// Texture brightness
			ceilBrightness.Text = sc.Fields.GetValue("lightceiling", 0).ToString();
			floorBrightness.Text = sc.Fields.GetValue("lightfloor", 0).ToString();
			ceilLightAbsolute.Checked = sc.Fields.GetValue("lightceilingabsolute", false);
			floorLightAbsolute.Checked = sc.Fields.GetValue("lightfloorabsolute", false);

			// Damage
			damagetype.Text = sc.Fields.GetValue("damagetype", NO_DAMAGETYPE);

			// Misc
			gravity.Text = sc.Fields.GetValue("gravity", 1.0).ToString();
			friction.Text = sc.Fields.GetValue("friction", 0.90625).ToString();
			triggerTag.Text = sc.Fields.GetValue("triggertag", 0).ToString();
			triggerer.Text = sc.Fields.GetValue("triggerer", TRIGGERER_DEFAULT);

			// Sector colors
			fadeColor.SetValueFrom(sc.Fields, true);
			lightColor.SetValueFrom(sc.Fields, true);
			lightAlpha.Text = UniFields.GetInteger(sc.Fields, "lightalpha", General.Map.Config.MaxColormapAlpha).ToString();
			fadeAlpha.Text = UniFields.GetInteger(sc.Fields, "fadealpha", General.Map.Config.MaxColormapAlpha).ToString();
			fadeStart.Text = UniFields.GetInteger(sc.Fields, "fadestart", 0).ToString();
			fadeEnd.Text = UniFields.GetInteger(sc.Fields, "fadeend", General.Map.Config.NumBrightnessLevels - 1).ToString();

			// Slopes
			SetupFloorSlope(sc, true);
			SetupCeilingSlope(sc, true);

			// Custom fields
			fieldslist.SetValues(sc.Fields, true);

			// Comments
			commenteditor.SetValues(sc.Fields, true);

			anglesteps = new StepsList();

			////////////////////////////////////////////////////////////////////////
			// Now go for all sectors and change the options when a setting is different
			////////////////////////////////////////////////////////////////////////

			// Go for all sectors
			foreach(Sector s in sectors) 
			{
				// Flags
				SetupFlags(flags, s);

				// Effects
				if(s.Brightness.ToString() != brightness.Text) brightness.Text = "";

				// Floor/Ceiling
				if(s.FloorHeight.ToString() != floorheight.Text) floorheight.Text = "";
				if(s.CeilHeight.ToString() != ceilingheight.Text) ceilingheight.Text = "";
				if(s.FloorTexture != floortex.TextureName) 
				{
					floortex.MultipleTextures = true; //mxd
					floortex.TextureName = "";
				}
				if(s.CeilTexture != ceilingtex.TextureName) 
				{
					ceilingtex.MultipleTextures = true; //mxd
					ceilingtex.TextureName = "";
				}

				// UDMF stuff
				// Texture offsets
				ceilOffsets.SetValuesFrom(s.Fields, false);
				floorOffsets.SetValuesFrom(s.Fields, false);

				// Texture scale
				ceilScale.SetValuesFrom(s.Fields, false);
				floorScale.SetValuesFrom(s.Fields, false);

				// Texture rotation
				if(s.Fields.GetValue("rotationceiling", 0.0).ToString() != ceilRotation.Text) 
				{
					ceilRotation.Text = "";
					ceilAngleControl.Angle = AngleControlEx.NO_ANGLE;
				}
				if(s.Fields.GetValue("rotationfloor", 0.0).ToString() != floorRotation.Text)
				{
					floorRotation.Text = "";
					floorAngleControl.Angle = AngleControlEx.NO_ANGLE;
				}

				// Texture brightness
				if(s.Fields.GetValue("lightceiling", 0).ToString() != ceilBrightness.Text) ceilBrightness.Text = "";
				if(s.Fields.GetValue("lightfloor", 0).ToString() != floorBrightness.Text) floorBrightness.Text = "";

				if(s.Fields.GetValue("lightceilingabsolute", false) != ceilLightAbsolute.Checked) 
				{
					ceilLightAbsolute.ThreeState = true;
					ceilLightAbsolute.CheckState = CheckState.Indeterminate;
				}
				if(s.Fields.GetValue("lightfloorabsolute", false) != floorLightAbsolute.Checked) 
				{
					floorLightAbsolute.ThreeState = true;
					floorLightAbsolute.CheckState = CheckState.Indeterminate;
				}

				// Damage
				if(damagetype.SelectedIndex > -1 && s.Fields.GetValue("damagetype", NO_DAMAGETYPE) != damagetype.Text) 
					damagetype.SelectedIndex = -1;

				// Misc
				if (s.Fields.GetValue("gravity", 1.0).ToString() != gravity.Text) gravity.Text = "";
				if (s.Fields.GetValue("friction", 0.90625).ToString() != friction.Text) friction.Text = "";
				if (s.Fields.GetValue("triggertag", 0).ToString() != triggerTag.Text) triggerTag.Text = "";
				if (triggerer.SelectedIndex > -1 && s.Fields.GetValue("triggerer", TRIGGERER_DEFAULT) != triggerer.Text)
					triggerer.SelectedIndex = -1;

				// Sector colors
				fadeColor.SetValueFrom(s.Fields, false);
				lightColor.SetValueFrom(s.Fields, false);

				if (!string.IsNullOrEmpty(lightAlpha.Text))
				{
					int alpha = UniFields.GetInteger(s.Fields, "lightalpha", General.Map.Config.MaxColormapAlpha);
					if (alpha != lightAlpha.GetResult(alpha)) lightAlpha.Text = string.Empty;
				}

				if (!string.IsNullOrEmpty(fadeAlpha.Text))
				{
					int alpha = UniFields.GetInteger(s.Fields, "fadealpha", General.Map.Config.MaxColormapAlpha);
					if (alpha != fadeAlpha.GetResult(alpha)) fadeAlpha.Text = string.Empty;
				}

				if (!string.IsNullOrEmpty(fadeStart.Text))
				{
					int val = UniFields.GetInteger(s.Fields, "fadestart", 0);
					if (val != fadeStart.GetResult(val)) fadeStart.Text = string.Empty;
				}

				if (!string.IsNullOrEmpty(fadeEnd.Text))
				{
					int val = UniFields.GetInteger(s.Fields, "fadeend", General.Map.Config.NumBrightnessLevels - 1);
					if (val != fadeEnd.GetResult(val)) fadeEnd.Text = string.Empty;
				}

				// Slopes
				SetupFloorSlope(s, false);
				SetupCeilingSlope(s, false);

				// Custom fields
				fieldslist.SetValues(s.Fields, false);

				//mxd. Comments
				commenteditor.SetValues(s.Fields, false);

				//mxd. Angle steps
				foreach(Sidedef side in s.Sidedefs)
				{
					int angle;
					if(side.Line.Front != null && side.Index == side.Line.Front.Index)
						angle = General.ClampAngle(270 - side.Line.AngleDeg);
					else
						angle = General.ClampAngle(90 - side.Line.AngleDeg);

					if(!anglesteps.Contains(angle)) anglesteps.Add(angle);
				}
			}

			//mxd. Setup tags
			tagsselector.SetValues(sectors);

			//mxd. Update slope controls
			ceilingslopecontrol.UpdateControls();
			floorslopecontrol.UpdateControls();

			// Show sector height
			UpdateSectorHeight();

			//mxd. Update brightness reset buttons
			resetceillight.Visible = (ceilLightAbsolute.CheckState != CheckState.Unchecked || ceilBrightness.GetResult(0) != 0);
			resetfloorlight.Visible = (floorLightAbsolute.CheckState != CheckState.Unchecked || floorBrightness.GetResult(0) != 0);

			//mxd. Angle steps
			anglesteps.Sort();
			if(cbUseCeilLineAngles.Checked) ceilRotation.StepValues = anglesteps;
			if(cbUseFloorLineAngles.Checked) floorRotation.StepValues = anglesteps;
			if(ceilingslopecontrol.UseLineAngles) ceilingslopecontrol.StepValues = anglesteps;
			if(floorslopecontrol.UseLineAngles) floorslopecontrol.StepValues = anglesteps;

			//mxd. Comments
			commenteditor.FinishSetup();

			preventchanges = false; //mxd
		}

		//mxd
		private static void SetupFlags(CheckboxArrayControl control, Sector s)
		{
			foreach(CheckBox c in control.Checkboxes)
			{
				if(c.CheckState == CheckState.Indeterminate) continue; //mxd
				if(s.IsFlagSet(c.Tag.ToString()) != c.Checked)
				{
					c.ThreeState = true;
					c.CheckState = CheckState.Indeterminate;
				}
			}
		}

		//mxd
		private static void ApplyFlags(CheckboxArrayControl control, Sector s)
		{
			foreach(CheckBox c in control.Checkboxes)
			{
				switch(c.CheckState)
				{
					case CheckState.Checked: s.SetFlag(c.Tag.ToString(), true); break;
					case CheckState.Unchecked: s.SetFlag(c.Tag.ToString(), false); break;
				}
			}
		}

		//mxd
		private void MakeUndo() 
		{
			if(undocreated) return;
			undocreated = true;

			//mxd. Make undo
			General.Map.UndoRedo.CreateUndo("Edit " + (sectors.Count > 1 ? sectors.Count + " sectors" : "sector"));
			foreach(Sector s in sectors) s.Fields.BeforeFieldsChange();
		}

		// mxd
		private void CreateHelperProps(ICollection<Sector> sectors) 
		{
			slopepivots = new Dictionary<Sector, Vector2D>(sectors.Count);
			
			foreach(Sector s in sectors)
			{
				if(slopepivots.ContainsKey(s)) continue;
				Vector2D pivot = new Vector2D(s.BBox.X + s.BBox.Width / 2, s.BBox.Y + s.BBox.Height / 2);
				globalslopepivot += pivot;
				slopepivots.Add(s, pivot);

				// Store initial properties
				sectorprops.Add(s, new SectorProperties(s));
			}

			globalslopepivot /= sectors.Count;
		}

		// This updates the sector height field
		private void UpdateSectorHeight() 
		{
			int delta = int.MinValue;

			// Check all selected sectors
			foreach(Sector s in sectors) 
			{
				if(delta == int.MinValue) 
				{
					// First sector in list
					delta = s.CeilHeight - s.FloorHeight;
				} 
				else if(delta != (s.CeilHeight - s.FloorHeight)) 
				{
					// We can't show heights because the delta
					// heights for the sectors is different
					delta = int.MinValue;
					break;
				}
			}

			if(delta != int.MinValue) 
			{
				sectorheight.Text = delta.ToString();
				sectorheight.Visible = true;
				sectorheightlabel.Visible = true;
			} 
			else 
			{
				sectorheight.Visible = false;
				sectorheightlabel.Visible = false;
			}
		}

		//mxd
		private void UpdateCeilingHeight() 
		{
			int offset;

			if(heightoffset.Text == "++" || heightoffset.Text == "--") // Raise or lower by sector height
			{
				int sign = (heightoffset.Text == "++" ? 1 : -1);
				foreach(Sector s in sectors)
				{
					offset = sectorprops[s].CeilHeight - sectorprops[s].FloorHeight;
					s.CeilHeight += offset * sign;
				}
			}
			else
			{
				//restore values
				if(string.IsNullOrEmpty(ceilingheight.Text))
				{
					// Reset increment steps, otherwise it's just keep counting and counting
					heightoffset.ResetIncrementStep();

					foreach (Sector s in sectors)
					{
						// To get the steps for ---/+++ into effect the offset has to be retrieved again for each sector
						offset = heightoffset.GetResult(0);
						s.CeilHeight = sectorprops[s].CeilHeight + offset;
					}
				}
				else //update values
				{
					// Reset increment steps, otherwise it's just keep counting and counting
					heightoffset.ResetIncrementStep();

					foreach (Sector s in sectors)
					{
						// To get the steps for ---/+++ into effect the offset has to be retrieved again for each sector
						offset = heightoffset.GetResult(0);
						s.CeilHeight = ceilingheight.GetResult(sectorprops[s].CeilHeight) + offset;
					}
				}
			}
		}

		//mxd
		private void UpdateFloorHeight() 
		{
			int offset;

			if(heightoffset.Text == "++" || heightoffset.Text == "--")
			{
				// Raise or lower by sector height
				int sign = (heightoffset.Text == "++" ? 1 : -1);
				foreach(Sector s in sectors)
				{
					offset = sectorprops[s].CeilHeight - sectorprops[s].FloorHeight;
					s.FloorHeight += offset * sign;
				}
			}
			else
			{
				// Reset increment steps, otherwise it's just keep counting and counting
				heightoffset.ResetIncrementStep();
				
				//restore values
				if(string.IsNullOrEmpty(floorheight.Text))
				{
					foreach(Sector s in sectors)
					{
						// To get the steps for ---/+++ into effect the offset has to be retrieved again for each sector
						offset = heightoffset.GetResult(0);
						s.FloorHeight = sectorprops[s].FloorHeight + offset;
					}
				}
				else //update values
				{
					foreach(Sector s in sectors)
					{
						// To get the steps for ---/+++ into effect the offset has to be retrieved again for each sector
						offset = heightoffset.GetResult(0);
						s.FloorHeight = floorheight.GetResult(sectorprops[s].FloorHeight) + offset;
					}
				}
			}
		}

		#endregion

		#region ================== Events

		private void apply_Click(object sender, EventArgs e) 
		{
			MakeUndo(); //mxd

			// Go for all sectors
			foreach(Sector s in sectors) 
			{
				// Apply all flags
				ApplyFlags(flags, s);

				// Fields
				fieldslist.Apply(s.Fields);

				//mxd. Comments
				commenteditor.Apply(s.Fields);

				//Damage
				if(!string.IsNullOrEmpty(damagetype.Text))
					UniFields.SetString(s.Fields, "damagetype", damagetype.Text, NO_DAMAGETYPE);

				// Misc
				if(!string.IsNullOrEmpty(gravity.Text)) 
					UniFields.SetFloat(s.Fields, "gravity", gravity.GetResultFloat(s.Fields.GetValue("gravity", 1.0)), 1.0);

				if (!string.IsNullOrEmpty(friction.Text))
					UniFields.SetFloat(s.Fields, "friction", friction.GetResultFloat(s.Fields.GetValue("friction", 0.90625)), 0.90625);

				if (!string.IsNullOrEmpty(triggerTag.Text))
					UniFields.SetInteger(s.Fields, "triggertag", triggerTag.GetResult(s.Fields.GetValue("triggertag", 0)), 0);

				if (!string.IsNullOrEmpty(triggerer.Text))
					UniFields.SetString(s.Fields, "triggerer", triggerer.Text, TRIGGERER_DEFAULT);

				// Clear horizontal slopes
				double diff = Math.Abs(Math.Round(s.FloorSlopeOffset) - s.FloorSlopeOffset);
				if (Math.Abs(s.FloorSlope.z) == 1.0 && diff < 0.000000001)
				{
					s.FloorHeight = -Convert.ToInt32(s.FloorSlopeOffset);
					s.FloorSlope = new Vector3D();
					s.FloorSlopeOffset = double.NaN;
				}

				diff = Math.Abs(Math.Round(s.CeilSlopeOffset) - s.CeilSlopeOffset);
				if (Math.Abs(s.CeilSlope.z) == 1.0 && diff < 0.000000001)
				{
					s.CeilHeight = Convert.ToInt32(s.CeilSlopeOffset);
					s.CeilSlope = new Vector3D();
					s.CeilSlopeOffset = double.NaN;
				}
			}

			//mxd. Apply tags
			tagsselector.ApplyTo(sectors);

			// Update the used textures
			General.Map.Data.UpdateUsedTextures();

			// Done
			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty); //mxd
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancel_Click(object sender, EventArgs e) 
		{
			//mxd. Let's pretend nothing of this really happened...
			if (undocreated)
			{
				General.Map.UndoRedo.WithdrawUndo();

				// Changing certain properties of the sector, like floor/ceiling textures will set General.Map.IsChanged to true.
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
		private void SectorEditFormUDMF_FormClosing(object sender, FormClosingEventArgs e) 
		{
			// Save settings
			General.Settings.WriteSetting("windows." + configname + ".activetab", tabs.SelectedIndex);

			General.Settings.WriteSetting("windows." + configname + ".linkceilingscale", ceilScale.LinkValues);
			General.Settings.WriteSetting("windows." + configname + ".linkfloorscale", floorScale.LinkValues);

			General.Settings.WriteSetting("windows." + configname + ".useceillineangles", cbUseCeilLineAngles.Checked);
			General.Settings.WriteSetting("windows." + configname + ".usefloorlineangles", cbUseFloorLineAngles.Checked);

			General.Settings.WriteSetting("windows." + configname + ".useceilslopelineangles", ceilingslopecontrol.UseLineAngles);
			General.Settings.WriteSetting("windows." + configname + ".usefloorslopelineangles", floorslopecontrol.UseLineAngles);

			General.Settings.WriteSetting("windows." + configname + ".ceilpivotmode", (int)ceilingslopecontrol.PivotMode);
			General.Settings.WriteSetting("windows." + configname + ".floorpivotmode", (int)floorslopecontrol.PivotMode);
		}

		private void SectorEditFormUDMF_HelpRequested(object sender, HelpEventArgs hlpevent) 
		{
			General.ShowHelp("w_sectoredit.html");
			hlpevent.Handled = true;
		}

		private void tabcustom_MouseEnter(object sender, EventArgs e) 
		{
			fieldslist.Focus();
		}

		private void ceilAngleControl_AngleChanged(object sender, EventArgs e) 
		{
			ceilRotation.Text = (General.ClampAngle(360 - ceilAngleControl.Angle)).ToString();
		}

		private void floorAngleControl_AngleChanged(object sender, EventArgs e) 
		{
			floorRotation.Text = (General.ClampAngle(360 - floorAngleControl.Angle)).ToString();
		}

		private void cbUseCeilLineAngles_CheckedChanged(object sender, EventArgs e) 
		{
			ceilRotation.ButtonStepsWrapAround = cbUseCeilLineAngles.Checked;
			ceilRotation.StepValues = (cbUseCeilLineAngles.Checked ? anglesteps : null);
		}

		private void cbUseFloorLineAngles_CheckedChanged(object sender, EventArgs e) 
		{
			floorRotation.ButtonStepsWrapAround = cbUseFloorLineAngles.Checked;
			floorRotation.StepValues = (cbUseFloorLineAngles.Checked ? anglesteps : null);
		}

		private void resetdamagetype_Click(object sender, EventArgs e)
		{
			damagetype.Focus();
			damagetype.Text = NO_DAMAGETYPE;
		}

		private void damagetype_TextChanged(object sender, EventArgs e)
		{
			resetdamagetype.Visible = (damagetype.Text != NO_DAMAGETYPE);
		}

		private void damagetype_MouseDown(object sender, MouseEventArgs e)
		{
			if(damagetype.Text == NO_DAMAGETYPE) damagetype.SelectAll();
		}

		#endregion

		#region ================== Sector Realtime events (mxd)

		private void ceilingheight_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges)	return;
			MakeUndo(); //mxd

			UpdateCeilingHeight();
			UpdateSectorHeight();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorheight_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges)	return;
			MakeUndo(); //mxd

			UpdateFloorHeight();
			UpdateSectorHeight();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void heightoffset_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			UpdateFloorHeight();
			UpdateCeilingHeight();
			UpdateSectorHeight();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void brightness_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges)	return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			brightness.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(brightness.Text)) 
			{
				foreach(Sector s in sectors)
					s.Brightness = sectorprops[s].Brightness;
			
			} 
			else //update values
			{
				foreach(Sector s in sectors)
					s.Brightness = General.Clamp(brightness.GetResult(sectorprops[s].Brightness), General.Map.FormatInterface.MinBrightness, General.Map.FormatInterface.MaxBrightness);
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilingtex_OnValueChanged(object sender, EventArgs e) 
		{
			if(preventchanges)	return;
			MakeUndo(); //mxd

			//restore values
			if(string.IsNullOrEmpty(ceilingtex.TextureName)) 
			{
				foreach(Sector s in sectors)
					s.SetCeilTexture(sectorprops[s].CeilTexture);
			
			} 
			else //update values
			{
				foreach(Sector s in sectors)
					s.SetCeilTexture(ceilingtex.GetResult(s.CeilTexture));
			}

			// Update the used textures
			General.Map.Data.UpdateUsedTextures();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void floortex_OnValueChanged(object sender, EventArgs e) 
		{
			if(preventchanges)	return;
			MakeUndo(); //mxd

			//restore values
			if(string.IsNullOrEmpty(floortex.TextureName)) 
			{
				foreach(Sector s in sectors)
					s.SetFloorTexture(sectorprops[s].FloorTexture);

			} 
			else //update values
			{
				foreach(Sector s in sectors)
					s.SetFloorTexture(floortex.GetResult(s.FloorTexture));
			}

			// Update the used textures
			General.Map.Data.UpdateUsedTextures();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorRotation_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges)	return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			floorRotation.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(floorRotation.Text))
			{
				floorAngleControl.Angle = AngleControlEx.NO_ANGLE;
				
				foreach(Sector s in sectors) 
				{
					UniFields.SetFloat(s.Fields, "rotationfloor", sectorprops[s].FloorRotation, 0.0);
					s.UpdateNeeded = true;
				}
			} 
			else //update values
			{
				floorAngleControl.Angle = (int)General.ClampAngle(360 - floorRotation.GetResultFloat(0));
				
				foreach(Sector s in sectors) 
				{
					UniFields.SetFloat(s.Fields, "rotationfloor", floorRotation.GetResultFloat(sectorprops[s].FloorRotation), 0.0);
					s.UpdateNeeded = true;
				}
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilRotation_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			ceilRotation.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(ceilRotation.Text))
			{
				ceilAngleControl.Angle = AngleControlEx.NO_ANGLE;
				
				foreach(Sector s in sectors) 
				{
					UniFields.SetFloat(s.Fields, "rotationceiling", sectorprops[s].CeilRotation, 0.0);
					s.UpdateNeeded = true;
				}
			} 
			else //update values
			{
				ceilAngleControl.Angle = (int)General.ClampAngle(360 - ceilRotation.GetResultFloat(0));
				
				foreach(Sector s in sectors) 
				{
					UniFields.SetFloat(s.Fields, "rotationceiling", ceilRotation.GetResultFloat(sectorprops[s].CeilRotation), 0.0);
					s.UpdateNeeded = true;
				}
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void lightColor_OnValueChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			foreach(Sector s in sectors) 
			{
				lightColor.ApplyTo(s.Fields, sectorprops[s].LightColor);
				s.UpdateNeeded = true;
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void fadeColor_OnValueChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			foreach(Sector s in sectors) 
			{
				fadeColor.ApplyTo(s.Fields, sectorprops[s].FadeColor);
				s.UpdateNeeded = true;
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void lightAlpha_WhenTextChanged(object sender, EventArgs e)
		{
			if (preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			lightAlpha.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(lightAlpha.Text))
			{
				foreach (Sector s in sectors)
					UniFields.SetInteger(s.Fields, "lightalpha", sectorprops[s].LightAlpha, General.Map.Config.MaxColormapAlpha);
			}
			else //update values
			{
				foreach (Sector s in sectors)
				{
					int alpha = General.Clamp(lightAlpha.GetResult(sectorprops[s].LightAlpha), 0, General.Map.Config.MaxColormapAlpha);
					UniFields.SetInteger(s.Fields, "lightalpha", alpha, General.Map.Config.MaxColormapAlpha);
				}
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void fadeAlpha_WhenTextChanged(object sender, EventArgs e)
		{
			if (preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			fadeAlpha.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(fadeAlpha.Text))
			{
				foreach (Sector s in sectors)
					UniFields.SetInteger(s.Fields, "fadealpha", sectorprops[s].FadeAlpha, General.Map.Config.MaxColormapAlpha);
			}
			else //update values
			{
				foreach (Sector s in sectors)
				{
					int alpha = General.Clamp(fadeAlpha.GetResult(sectorprops[s].FadeAlpha), 0, General.Map.Config.MaxColormapAlpha);
					UniFields.SetInteger(s.Fields, "fadealpha", alpha, General.Map.Config.MaxColormapAlpha);
				}
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void fadeStart_WhenTextChanged(object sender, EventArgs e)
		{
			if (preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			fadeStart.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(fadeStart.Text))
			{
				foreach (Sector s in sectors)
					UniFields.SetInteger(s.Fields, "fadestart", sectorprops[s].FadeStart, 0);
			}
			else //update values
			{
				foreach (Sector s in sectors)
				{
					int val = General.Clamp(fadeStart.GetResult(sectorprops[s].FadeStart), 0, General.Map.Config.NumBrightnessLevels - 2);
					UniFields.SetInteger(s.Fields, "fadestart", val, 0);
				}
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void fadeEnd_WhenTextChanged(object sender, EventArgs e)
		{
			if (preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			fadeEnd.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(fadeEnd.Text))
			{
				foreach (Sector s in sectors)
					UniFields.SetInteger(s.Fields, "fadeend", sectorprops[s].FadeEnd, General.Map.Config.NumBrightnessLevels - 1);
			}
			else //update values
			{
				foreach (Sector s in sectors)
				{
					int val = General.Clamp(fadeEnd.GetResult(sectorprops[s].FadeEnd), 1, General.Map.Config.NumBrightnessLevels - 1);
					UniFields.SetInteger(s.Fields, "fadeend", val, General.Map.Config.NumBrightnessLevels - 1);
				}
			}

			General.Map.IsChanged = true;
			if (OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		#endregion

		#region ================== Ceiling/Floor realtime events (mxd)

		private void ceilOffsets_OnValuesChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			ceilOffsets.ResetIncrementStep();

			foreach (Sector s in sectors) 
			{
				ceilOffsets.ApplyTo(s.Fields, General.Map.FormatInterface.MinTextureOffset, General.Map.FormatInterface.MaxTextureOffset, sectorprops[s].CeilOffsetX, sectorprops[s].CeilOffsetY);
				s.UpdateNeeded = true;
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorOffsets_OnValuesChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			floorOffsets.ResetIncrementStep();

			foreach (Sector s in sectors) 
			{
				floorOffsets.ApplyTo(s.Fields, General.Map.FormatInterface.MinTextureOffset, General.Map.FormatInterface.MaxTextureOffset, sectorprops[s].FloorOffsetX, sectorprops[s].FloorOffsetY);
				s.UpdateNeeded = true;
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilScale_OnValuesChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			ceilScale.ResetIncrementStep();

			foreach (Sector s in sectors) 
			{
				ceilScale.ApplyTo(s.Fields, General.Map.FormatInterface.MinTextureOffset, General.Map.FormatInterface.MaxTextureOffset, sectorprops[s].CeilScaleX, sectorprops[s].CeilScaleY);
				s.UpdateNeeded = true;
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorScale_OnValuesChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			floorScale.ResetIncrementStep();

			foreach (Sector s in sectors) 
			{
				floorScale.ApplyTo(s.Fields, General.Map.FormatInterface.MinTextureOffset, General.Map.FormatInterface.MaxTextureOffset, sectorprops[s].FloorScaleX, sectorprops[s].FloorScaleY);
				s.UpdateNeeded = true;
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilBrightness_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			ceilBrightness.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(ceilBrightness.Text)) 
			{
				foreach(Sector s in sectors) 
				{
					UniFields.SetInteger(s.Fields, "lightceiling", sectorprops[s].CeilBrightness, 0);
					s.UpdateNeeded = true;
				}
			} 
			else //update values
			{
				foreach(Sector s in sectors) 
				{
					bool absolute = false;
					switch(ceilLightAbsolute.CheckState)
					{
						case CheckState.Indeterminate:
							absolute = s.Fields.GetValue("lightceilingabsolute", false);
							break;
						case CheckState.Checked:
							absolute = true;
							break;
					}

					int value = General.Clamp(ceilBrightness.GetResult(sectorprops[s].CeilBrightness), (absolute ? 0 : -255), 255);
					UniFields.SetInteger(s.Fields, "lightceiling", value, 0);
					s.UpdateNeeded = true;
				}
			}

			resetceillight.Visible = (ceilLightAbsolute.CheckState != CheckState.Unchecked || ceilBrightness.Text != "0");
			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorBrightness_WhenTextChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			// Reset increment steps, otherwise it's just keep counting and counting
			floorBrightness.ResetIncrementStep();

			//restore values
			if (string.IsNullOrEmpty(floorBrightness.Text)) 
			{
				foreach(Sector s in sectors) 
				{
					UniFields.SetInteger(s.Fields, "lightfloor", sectorprops[s].FloorBrightness, 0);
					s.UpdateNeeded = true;
				}
			} 
			else //update values
			{
				foreach(Sector s in sectors) 
				{
					bool absolute = false;
					switch(floorLightAbsolute.CheckState)
					{
						case CheckState.Indeterminate:
							absolute = s.Fields.GetValue("lightfloorabsolute", false);
							break;
						case CheckState.Checked:
							absolute = true;
							break;
					}

					int value = General.Clamp(floorBrightness.GetResult(sectorprops[s].FloorBrightness), (absolute ? 0 : -255), 255);
					UniFields.SetInteger(s.Fields, "lightfloor", value, 0);
					s.UpdateNeeded = true;
				}
			}

			resetfloorlight.Visible = (floorLightAbsolute.CheckState != CheckState.Unchecked || floorBrightness.Text != "0");
			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilLightAbsolute_CheckedChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			if(ceilLightAbsolute.Checked) 
			{
				foreach(Sector s in sectors) 
				{
					s.Fields["lightceilingabsolute"] = new UniValue(UniversalType.Boolean, true);
					s.UpdateNeeded = true;
				}
			} 
			else if(ceilLightAbsolute.CheckState == CheckState.Indeterminate) 
			{
				foreach(Sector s in sectors) 
				{
					if(sectorprops[s].CeilLightAbsoulte) 
					{
						s.Fields["lightceilingabsolute"] = new UniValue(UniversalType.Boolean, true);
						s.UpdateNeeded = true;
					}
					else if(s.Fields.ContainsKey("lightceilingabsolute")) 
					{
						s.Fields.Remove("lightceilingabsolute");
						s.UpdateNeeded = true;
					}
				}
			} 
			else 
			{
				foreach(Sector s in sectors) 
				{
					if(s.Fields.ContainsKey("lightceilingabsolute")) 
					{
						s.Fields.Remove("lightceilingabsolute");
						s.UpdateNeeded = true;
					}
				}
			}

			resetceillight.Visible = (ceilLightAbsolute.CheckState != CheckState.Unchecked || ceilBrightness.Text != "0");
			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorLightAbsolute_CheckedChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			if(floorLightAbsolute.Checked)
			{
				foreach(Sector s in sectors) 
				{
					s.Fields["lightfloorabsolute"] = new UniValue(UniversalType.Boolean, true);
					s.UpdateNeeded = true;
				}
			} 
			else if(floorLightAbsolute.CheckState == CheckState.Indeterminate) 
			{
				foreach(Sector s in sectors) 
				{
					if(sectorprops[s].FloorLightAbsoulte) 
					{
						s.Fields["lightfloorabsolute"] = new UniValue(UniversalType.Boolean, true);
						s.UpdateNeeded = true;
					} 
					else if(s.Fields.ContainsKey("lightfloorabsolute")) 
					{
						s.Fields.Remove("lightfloorabsolute");
						s.UpdateNeeded = true;
					}
				}
			} 
			else 
			{
				foreach(Sector s in sectors) 
				{
					if(s.Fields.ContainsKey("lightfloorabsolute")) 
					{
						s.Fields.Remove("lightfloorabsolute");
						s.UpdateNeeded = true;
					}
				}
			}

			resetfloorlight.Visible = (floorLightAbsolute.CheckState != CheckState.Unchecked || floorBrightness.Text != "0");
			General.Map.IsChanged = true;
			if(OnValuesChanged != null)	OnValuesChanged(this, EventArgs.Empty);
		}

		private void resetceillight_Click(object sender, EventArgs e)
		{
			MakeUndo(); //mxd

			preventchanges = true;

			ceilLightAbsolute.Checked = false;
			ceilBrightness.Text = "0";

			foreach(Sector s in sectors)
			{
				if(s.Fields.ContainsKey("lightceilingabsolute")) s.Fields.Remove("lightceilingabsolute");
				if(s.Fields.ContainsKey("lightceiling")) s.Fields.Remove("lightceiling");
			}

			preventchanges = false;

			resetceillight.Visible = false;
			ceilBrightness.Focus();
			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void resetfloorlight_Click(object sender, EventArgs e)
		{
			MakeUndo(); //mxd

			preventchanges = true;

			floorLightAbsolute.Checked = false;
			floorBrightness.Text = "0";

			foreach(Sector s in sectors)
			{
				if(s.Fields.ContainsKey("lightfloorabsolute")) s.Fields.Remove("lightfloorabsolute");
				if(s.Fields.ContainsKey("lightfloor")) s.Fields.Remove("lightfloor");
			}

			preventchanges = false;

			resetfloorlight.Visible = false;
			floorBrightness.Focus();
			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		#endregion

		#region ================== Slope Utility (mxd)

		private void SetupFloorSlope(Sector s, bool first) 
		{
			if(s.FloorSlope.GetLengthSq() > 0) 
			{
				double anglexy = General.ClampAngle(Math.Round(Angle2D.RadToDeg(s.FloorSlope.GetAngleXY()) - 180, 1));
				double anglez = -Math.Round(Angle2D.RadToDeg(s.FloorSlope.GetAngleZ()) - 90, 1);
				double offset = Math.Round(GetVirtualSlopeOffset(s, floorslopecontrol.PivotMode, true), 1);

				if(anglexy >= 180 && anglez < 0) 
				{
					anglexy -= 180;
					anglez = -anglez;
				}

				floorslopecontrol.SetValues(anglexy, anglez, offset, first);
			} 
			else 
			{
				floorslopecontrol.SetValues(0.0, 0.0, s.FloorHeight, first);
			}
		}

		private void SetupCeilingSlope(Sector s, bool first) 
		{
			if(s.CeilSlope.GetLengthSq() > 0) 
			{
				double anglexy = General.ClampAngle(Math.Round(Angle2D.RadToDeg(s.CeilSlope.GetAngleXY()) - 180, 1));
				double anglez = -(270 - Math.Round(Angle2D.RadToDeg(s.CeilSlope.GetAngleZ()), 1));
				double offset = Math.Round(GetVirtualSlopeOffset(s, ceilingslopecontrol.PivotMode, false), 1);

				if(anglexy >= 180 && anglez < 0) 
				{
					anglexy -= 180;
					anglez = -anglez;
				}

				ceilingslopecontrol.SetValues(anglexy, anglez, offset, first);
			} 
			else 
			{
				ceilingslopecontrol.SetValues(0.0, 0.0, s.CeilHeight, first);
			}
		}

		// Gets the offset to be displayed in a SectorSlopeControl
		private double GetVirtualSlopeOffset(Sector s, SlopePivotMode mode, bool floor) 
		{
			double offset = (floor ? s.FloorSlopeOffset : s.CeilSlopeOffset);
			if(double.IsNaN(offset))
			{
				offset = (floor ? s.FloorHeight : s.CeilHeight);
			}

			Vector3D normal = (floor ? s.FloorSlope : s.CeilSlope);
			
			if(normal.GetLengthSq() > 0)
			{
				Vector3D center = GetSectorCenter(s, 0, mode);
				Plane p = new Plane(normal, offset);
				return p.GetZ(center);
			}

			return offset;
		}

		// Gets the offset to be displayed in a SectorSlopeControl
		private double GetInitialVirtualSlopeOffset(Sector s, SlopePivotMode mode, bool floor) 
		{
			double offset = (floor ? sectorprops[s].FloorSlopeOffset : sectorprops[s].CeilSlopeOffset);
			Vector3D normal = (floor ? sectorprops[s].FloorSlope : sectorprops[s].CeilSlope);

			if(normal.GetLengthSq() > 0) 
			{
				Vector3D center = GetSectorCenter(s, 0, mode);
				Plane p = new Plane(normal, offset);
				return p.GetZ(center);
			}

			return offset;
		}

		private Vector3D GetSectorCenter(Sector s, double offset, SlopePivotMode mode)
		{
			switch(mode) 
			{
				case SlopePivotMode.GLOBAL: //translate from the center of selection 
					return new Vector3D(globalslopepivot, offset);

				case SlopePivotMode.LOCAL: //translate from sector's bounding box center
					return new Vector3D(slopepivots[s], offset);

				case SlopePivotMode.ORIGIN: //don't translate
					return new Vector3D(0, 0, offset);

				default:
					throw new NotImplementedException("Unknown SlopePivotMode: " + (int)mode);
			}
		}

		#endregion

		#region ================== Slopes realtime events (mxd)

		private void ceilingslopecontrol_OnAnglesChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			//Set or restore values
			foreach(Sector s in sectors) 
			{
				double anglexy = General.ClampAngle(ceilingslopecontrol.GetAngleXY(sectorprops[s].CeilSlopeAngleXY) + 270);
				double anglez = -(ceilingslopecontrol.GetAngleZ(sectorprops[s].CeilSlopeAngleZ) + 90);

				double virtualoffset = GetInitialVirtualSlopeOffset(s, ceilingslopecontrol.PivotMode, false);
				Vector3D center = GetSectorCenter(s, ceilingslopecontrol.GetOffset(virtualoffset), ceilingslopecontrol.PivotMode);
				Plane p = new Plane(center, Angle2D.DegToRad(anglexy), Angle2D.DegToRad(anglez), false);
				s.CeilSlope = p.Normal;
				s.CeilSlopeOffset = p.Offset;

				s.UpdateNeeded = true;
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorslopecontrol_OnAnglesChanged(object sender, EventArgs e) 
		{
			if(preventchanges) return;
			MakeUndo(); //mxd

			//Set or restore values
			foreach(Sector s in sectors)
			{
				double anglexy = General.ClampAngle(floorslopecontrol.GetAngleXY(sectorprops[s].FloorSlopeAngleXY) + 90);
				double anglez = -(floorslopecontrol.GetAngleZ(sectorprops[s].FloorSlopeAngleZ) + 90);

				double virtualoffset = GetInitialVirtualSlopeOffset(s, floorslopecontrol.PivotMode, true);
				Vector3D center = GetSectorCenter(s, floorslopecontrol.GetOffset(virtualoffset), floorslopecontrol.PivotMode);
				Plane p = new Plane(center, Angle2D.DegToRad(anglexy), Angle2D.DegToRad(anglez), true);
				s.FloorSlope = p.Normal;
				s.FloorSlopeOffset = p.Offset;

				s.UpdateNeeded = true;
			}

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		// Update displayed ceiling offset value
		private void ceilingslopecontrol_OnPivotModeChanged(object sender, EventArgs e) 
		{
			MakeUndo(); //mxd
			bool first = true;
			foreach(Sector s in sectors)
			{
				SetupCeilingSlope(s, first);
				first = false;
			}
		}

		// Update displayed floor offset value
		private void floorslopecontrol_OnPivotModeChanged(object sender, EventArgs e) 
		{
			MakeUndo(); //mxd
			bool first = true;
			foreach(Sector s in sectors)
			{
				SetupFloorSlope(s, first);
				first = false;
			}
		}

		private void ceilingslopecontrol_OnResetClicked(object sender, EventArgs e) 
		{
			MakeUndo(); //mxd
			Sector fs = General.GetByIndex(sectors, 0);

			// biwa. Do not reset to the z position of the plane of the center of the sector anymore, since 
			// that will result in pretty crazy values of 3D floor control sectors
			/*
			if (fs.CeilSlope.IsNormalized())
			{
				fs.UpdateBBox();
				Plane p = new Plane(fs.CeilSlope, fs.CeilSlopeOffset);
				ceilingslopecontrol.SetOffset((int)Math.Round(p.GetZ(fs.BBox.X + fs.BBox.Width / 2, fs.BBox.Y + fs.BBox.Height / 2)), true);
			}
			else
				ceilingslopecontrol.SetOffset(fs.CeilHeight, true);
			*/
			ceilingslopecontrol.SetOffset(fs.CeilHeight, true);

			foreach (Sector s in sectors) 
			{
				// biwa. Do not reset to the z position of the plane of the center of the sector anymore, since 
				// that will result in pretty crazy values of 3D floor control sectors
				/*
				if (s.CeilSlope.IsNormalized())
				{
					s.UpdateBBox();
					Plane p = new Plane(s.CeilSlope, s.CeilSlopeOffset);
					s.CeilHeight = (int)Math.Round(p.GetZ(s.BBox.X + s.BBox.Width / 2, s.BBox.Y + s.BBox.Height / 2));
				}
				*/

				s.CeilSlope = new Vector3D();
				s.CeilSlopeOffset = double.NaN;
				s.UpdateNeeded = true;
				ceilingslopecontrol.SetOffset(s.CeilHeight, false);
			}

			ceilingslopecontrol.UpdateOffset();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void floorslopecontrol_OnResetClicked(object sender, EventArgs e) 
		{
			MakeUndo(); //mxd
			Sector fs = General.GetByIndex(sectors, 0);

			// biwa. Do not reset to the z position of the plane of the center of the sector anymore, since 
			// that will result in pretty crazy values of 3D floor control sectors
			/*
			if (fs.FloorSlope.IsNormalized())
			{
				fs.UpdateBBox();
				Plane p = new Plane(fs.FloorSlope, fs.FloorSlopeOffset);
				floorslopecontrol.SetOffset((int)Math.Round(p.GetZ(fs.BBox.X + fs.BBox.Width / 2, fs.BBox.Y + fs.BBox.Height / 2)), true);
			}
			else
				floorslopecontrol.SetOffset(fs.FloorHeight, true);
			*/

			floorslopecontrol.SetOffset(fs.FloorHeight, true);

			foreach (Sector s in sectors) 
			{
				// biwa. Do not reset to the z position of the plane of the center of the sector anymore, since 
				// that will result in pretty crazy values of 3D floor control sectors
				/*
				if (s.FloorSlope.IsNormalized())
				{
					s.UpdateBBox();
					Plane p = new Plane(s.FloorSlope, s.FloorSlopeOffset);
					s.FloorHeight = (int)Math.Round(p.GetZ(s.BBox.X + s.BBox.Width / 2, s.BBox.Y + s.BBox.Height / 2));
				}
				*/

				s.FloorSlope = new Vector3D();
				s.FloorSlopeOffset = double.NaN;
				s.UpdateNeeded = true;
				floorslopecontrol.SetOffset(s.FloorHeight, false);
			}

			floorslopecontrol.UpdateOffset();

			General.Map.IsChanged = true;
			if(OnValuesChanged != null) OnValuesChanged(this, EventArgs.Empty);
		}

		private void ceilingslopecontrol_OnUseLineAnglesChanged(object sender, EventArgs e) 
		{
			ceilingslopecontrol.StepValues = (ceilingslopecontrol.UseLineAngles ? anglesteps : null);
		}

		private void floorslopecontrol_OnUseLineAnglesChanged(object sender, EventArgs e) 
		{
			floorslopecontrol.StepValues = (floorslopecontrol.UseLineAngles ? anglesteps : null);
		}

		#endregion

    }
}
