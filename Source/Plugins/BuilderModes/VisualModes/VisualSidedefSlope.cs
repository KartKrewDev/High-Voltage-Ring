using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CodeImp.DoomBuilder.BuilderModes;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Rendering;

namespace CodeImp.DoomBuilder.VisualModes
{
	internal class VisualSidedefSlope : VisualSlope, IVisualEventReceiver
	{
		#region ================== Variables

		private readonly BaseVisualMode mode;
		private readonly Sidedef sidedef;
		private readonly SectorLevel level;
		private readonly bool up;
		private Vector3D pickintersect;
		private double pickrayu;
		private Plane plane;

		#endregion

		#region ================== Constants

		private const int SIZE = 8;

		#endregion

		#region ================== Properties

		public Sidedef Sidedef { get { return sidedef; } }
		public SectorLevel Level { get { return level; } }
		public int NormalizedAngleDeg { get { return (sidedef.Line.AngleDeg >= 180) ? (sidedef.Line.AngleDeg - 180) : sidedef.Line.AngleDeg; } }

		#endregion

		#region ================== Constructor / Destructor

		public VisualSidedefSlope(BaseVisualMode mode, SectorLevel level, Sidedef sidedef, bool up) : base()
		{
			this.mode = mode;
			this.sidedef = sidedef;
			this.level = level;
			this.up = up;

			// length = sidedef.Line.Length;

			Update();

			// We have no destructor
			GC.SuppressFinalize(this);
		}

		#endregion

		#region ================== Methods

		public Vector3D GetCenterPoint()
		{
			Vector2D p = sidedef.Line.GetCenterPoint();
			return new Vector3D(p, level.plane.GetZ(p));
		}

		public override void Update()
		{
			plane = new Plane(level.plane.Normal, level.plane.Offset - 0.1f);

			if (!up)
				plane = plane.GetInverted();

			UpdatePosition();

			length = new Line3D(new Vector3D(sidedef.Line.Line.v1, plane.GetZ(sidedef.Line.Line.v1)), new Vector3D(sidedef.Line.Line.v2, plane.GetZ(sidedef.Line.Line.v2))).GetDelta().GetLength();
		}

		/// <summary>
		/// This is called when the thing must be tested for line intersection. This should reject
		/// as fast as possible to rule out all geometry that certainly does not touch the line.
		/// </summary>
		public override bool PickFastReject(Vector3D from, Vector3D to, Vector3D dir)
		{
			if (sidedef.IsDisposed || sidedef.Sector.IsDisposed)
				return false;

			RectangleF bbox = sidedef.Sector.BBox;

			if ((up && plane.Distance(from) > 0.0f) || (!up && plane.Distance(from) < 0.0f))
			{
				if (plane.GetIntersection(from, to, ref pickrayu))
				{
					if (pickrayu > 0.0f)
					{
						pickintersect = from + (to - from) * pickrayu;

						return ((pickintersect.x >= bbox.Left) && (pickintersect.x <= bbox.Right) &&
								(pickintersect.y >= bbox.Top) && (pickintersect.y <= bbox.Bottom));
					}
				}
			}

			return false;
		}

		/// <summary>
		/// This is called when the thing must be tested for line intersection. This should perform
		/// accurate hit detection and set u_ray to the position on the ray where this hits the geometry.
		/// </summary>
		public override bool PickAccurate(Vector3D from, Vector3D to, Vector3D dir, ref double u_ray)
		{
			u_ray = pickrayu;

			Sidedef sd = MapSet.NearestSidedef(sidedef.Sector.Sidedefs, pickintersect);
			if (sd == sidedef) {
				double side = sd.Line.SideOfLine(pickintersect);

				if ((side <= 0.0f && sd.IsFront) || (side > 0.0f && !sd.IsFront))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Updates the position. Depending on 3D floors and which side of the linedef the slope handle is on the
		/// direction of the line used as a base has to be inverted
		/// </summary>
		public void UpdatePosition()
		{
			bool invertline = false;

			if (up)
			{
				if (level.extrafloor && level.type == SectorLevelType.Ceiling)
				{
					if (sidedef.IsFront)
						invertline = true;
				}
				else
				{
					if (!sidedef.IsFront)
						invertline = true;
				}
			}
			else
			{
				if (level.extrafloor && level.type == SectorLevelType.Floor)
				{
					if (!sidedef.IsFront)
						invertline = true;
				}
				else
				{
					if (sidedef.IsFront)
						invertline = true;
				}
			}

			if (invertline)
				SetPosition(new Line2D(sidedef.Line.End.Position, sidedef.Line.Start.Position), level.plane);
			else
				SetPosition(sidedef.Line.Line, level.plane);
		}

		/// <summary>
		/// Tries to find a slope handle to pivot around. If possible if finds the handle belonging to a line that has the
		/// same angle as the start handle, and is the furthest away. If such a handle does not exist it finds one that's
		/// closest to those specs
		/// </summary>
		/// <param name="starthandle">The slope handle to start from (the one we need to find a pivot handle for)</param>
		/// <returns></returns>
		public static VisualSidedefSlope GetSmartPivotHandle(VisualSidedefSlope starthandle, BaseVisualMode mode)
		{
			VisualSidedefSlope handle = starthandle;
			List<VisualSidedefSlope> potentialhandles = new List<VisualSidedefSlope>();
			List<IVisualEventReceiver> selectedsectors = mode.GetSelectedObjects(true, false, false, false, false);

			if (selectedsectors.Count == 0)
			{
				// No sectors selected, so find all handles that belong to the same level
				foreach (VisualSidedefSlope checkhandle in mode.AllSlopeHandles[starthandle.Sidedef.Sector])
					if (checkhandle != starthandle && checkhandle.Level == starthandle.Level)
						potentialhandles.Add(checkhandle);
			}
			else
			{
				// Sectors are selected, get all handles from those sectors that have the same level
				HashSet<Sector> sectors = new HashSet<Sector>();

				foreach (BaseVisualGeometrySector bvgs in selectedsectors)
					sectors.Add(bvgs.Sector.Sector);

				foreach (Sector s in sectors)
					foreach (VisualSidedefSlope checkhandle in mode.AllSlopeHandles[s])
						if(checkhandle != starthandle)
							foreach (BaseVisualGeometrySector bvgs in selectedsectors)
								if (bvgs.Level == checkhandle.Level)
									potentialhandles.Add(checkhandle);
			}

			foreach (KeyValuePair<Sector, List<VisualSlope>> kvp in mode.AllSlopeHandles)
				foreach (VisualSidedefSlope checkhandle in kvp.Value)
					checkhandle.SmartPivot = false;

			// Sort potential handles by their angle difference to the start handle. That means that handles with less angle difference will be at the beginning of the list
			List<VisualSidedefSlope> anglediffsortedhandles = potentialhandles.OrderBy(h => Math.Abs(starthandle.NormalizedAngleDeg - h.NormalizedAngleDeg)).ToList();

			// Get all potential handles that have to same angle as the one that's closest to the start handle, then sort them by distance, and take the one that's furthest away
			if (anglediffsortedhandles.Count > 0)
				handle = anglediffsortedhandles.Where(h => h.NormalizedAngleDeg == anglediffsortedhandles[0].NormalizedAngleDeg).OrderByDescending(h => Math.Abs(starthandle.Sidedef.Line.Line.GetDistanceToLine(h.sidedef.Line.GetCenterPoint(), false))).First();

			if (handle == starthandle)
				return null;

			return handle;
		}

		public static void ApplySlope(SectorLevel level, Plane plane, BaseVisualMode mode)
		{
			bool applytoceiling = false;

			Vector2D center = new Vector2D(level.sector.BBox.X + level.sector.BBox.Width / 2,
											   level.sector.BBox.Y + level.sector.BBox.Height / 2);

			if (level.extrafloor)
			{
				// The top side of 3D floors is the ceiling of the sector, but it's a "floor" in UDB, so the
				// ceiling of the control sector has to be modified
				if (level.type == SectorLevelType.Floor)
					applytoceiling = true;
			}
			else
			{
				if (level.type == SectorLevelType.Ceiling)
					applytoceiling = true;
			}

			if (applytoceiling)
			{
				Plane downplane = plane.GetInverted();
				level.sector.CeilSlope = downplane.Normal;
				level.sector.CeilSlopeOffset = downplane.Offset;
			}
			else
			{
				level.sector.FloorSlope = plane.Normal;
				level.sector.FloorSlopeOffset = plane.Offset;
			}

			// Rebuild sector
			BaseVisualSector vs;
			if (mode.VisualSectorExists(level.sector))
			{
				vs = (BaseVisualSector)mode.GetVisualSector(level.sector);
			}
			else
			{
				vs = mode.CreateBaseVisualSector(level.sector);
			}

			if (vs != null) vs.UpdateSectorGeometry(true);
		}

		#endregion

		#region ================== Events

		public void OnChangeTargetHeight(int amount)
		{
			VisualSlope pivothandle = null;
			List<IVisualEventReceiver> selectedsectors = mode.GetSelectedObjects(true, false, false, false, false);
			List<SectorLevel> levels = new List<SectorLevel>();

			if (selectedsectors.Count == 0)
				levels.Add(level);
			else
			{
				foreach (BaseVisualGeometrySector bvgs in selectedsectors)
					levels.Add(bvgs.Level);

				if (!levels.Contains(level))
					levels.Add(level);
			}

			// Try to find a slope handle the user set to be the pivot handle
			// TODO: doing this every time is kind of stupid. Maybe store the pivot handle in the mode?
			foreach (KeyValuePair<Sector, List<VisualSlope>> kvp in mode.AllSlopeHandles)
			{
				foreach (VisualSidedefSlope handle in kvp.Value)
				{
					if (handle.Pivot)
					{
						pivothandle = handle;
						break;
					}
				}
			}

			// User didn't set a pivot handle, try to find the smart pivot handle
			if(pivothandle == null)
				pivothandle = GetSmartPivotHandle(this, mode);

			// Still no pivot handle, cancle
			if (pivothandle == null)
				return;

			pivothandle.SmartPivot = true;

			mode.CreateUndo("Change slope");

			Plane originalplane = level.plane;
			Plane pivotplane = ((VisualSidedefSlope)pivothandle).Level.plane;

			// Build a new plane. p1 and p2 are the points of the slope handle that is modified, p3 is on the line of the pivot handle
			Vector3D p1 = new Vector3D(sidedef.Line.Start.Position, Math.Round(originalplane.GetZ(sidedef.Line.Start.Position)));
			Vector3D p2 = new Vector3D(sidedef.Line.End.Position, Math.Round(originalplane.GetZ(sidedef.Line.End.Position)));
			Vector3D p3 = new Vector3D(((VisualSidedefSlope)pivothandle).Sidedef.Line.Line.GetCoordinatesAt(0.5f), Math.Round(pivotplane.GetZ(((VisualSidedefSlope)pivothandle).Sidedef.Line.Line.GetCoordinatesAt(0.5f))));

			// Move the points of the handle up/down
			p1 += new Vector3D(0f, 0f, amount);
			p2 += new Vector3D(0f, 0f, amount);

			Plane plane = new Plane(p1, p2, p3, true);

			// Apply slope to surfaces
			foreach (SectorLevel l in levels)
				ApplySlope(l, plane, mode);

			mode.SetActionResult("Changed slope.");
		}

		// Select or deselect
		public void OnSelectEnd()
		{
			if (this.selected)
			{
				this.selected = false;
				mode.RemoveSelectedObject(this);
			}
			else
			{
				if(this.pivot)
				{
					General.Interface.DisplayStatus(Windows.StatusType.Warning, "It is not allowed to mark pivot slope handles as selected.");
					return;
				}

				this.selected = true;
				mode.AddSelectedObject(this);
			}
		}

		public void OnEditEnd()
		{
			// We can only have one pivot handle, so remove it from all first
			foreach (KeyValuePair<Sector, List<VisualSlope>> kvp in mode.AllSlopeHandles)
			{
				foreach (VisualSlope handle in kvp.Value)
				{
					if (handle == mode.HighlightedTarget)
					{
						if (handle.Selected)
							General.Interface.DisplayStatus(Windows.StatusType.Warning, "It is not allowed to mark selected slope handles as pivot slope handles.");
						else
							handle.Pivot = !handle.Pivot;
					}
					else
						handle.Pivot = false;
				}
			}
		}

		// Return texture name
		public string GetTextureName() { return ""; }

		// Unused
		public void OnSelectBegin() { }
		public void OnEditBegin() { }
		public void OnChangeTargetBrightness(bool up) { }
		public void OnChangeTextureOffset(int horizontal, int vertical, bool doSurfaceAngleCorrection) { }
		public void OnSelectTexture() { }
		public void OnCopyTexture() { }
		public void OnPasteTexture() { }
		public void OnCopyTextureOffsets() { }
		public void OnPasteTextureOffsets() { }
		public void OnTextureAlign(bool alignx, bool aligny) { }
		public void OnToggleUpperUnpegged() { }
		public void OnToggleLowerUnpegged() { }
		public void OnProcess(long deltatime) { }
		public void OnTextureFloodfill() { }
		public void OnInsert() { }
		public void OnTextureFit(FitTextureOptions options) { } //mxd
		public void ApplyTexture(string texture) { }
		public void ApplyUpperUnpegged(bool set) { }
		public void ApplyLowerUnpegged(bool set) { }
		public void SelectNeighbours(bool select, bool withSameTexture, bool withSameHeight) { } //mxd
		public virtual void OnPaintSelectEnd() { } // biwa
		public void OnChangeScale(int x, int y) { }
		public void OnResetTextureOffset() { }
		public void OnResetLocalTextureOffset() { }
		public void OnCopyProperties() { }
		public void OnPasteProperties(bool usecopysetting) { }
		public void OnDelete() { }
		public void OnPaintSelectBegin() { }
		public void OnMouseMove(MouseEventArgs e) { }

		#endregion
	}
}