#region === Copyright (c) 2010 Pascal van der Heiden ===

using System.Collections.Generic;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	internal class EffectSRB2ThingVertexSlope : SectorEffect
	{
		// Thing used to create this effect
		private List<Thing> things;

		// Floor or ceiling?
		private bool slopefloor;

		// Constructor
		public EffectSRB2ThingVertexSlope(SectorData data, List<Thing> sourcethings, bool floor) : base(data)
		{
			things = sourcethings;
			slopefloor = floor;

			// New effect added: This sector needs an update!
			if(data.Mode.VisualSectorExists(data.Sector))
			{
				BaseVisualSector vs = (BaseVisualSector)data.Mode.GetVisualSector(data.Sector);
				vs.UpdateSectorGeometry(true);
			}
		}

		// This makes sure we are updated with the source linedef information
		public override void Update()
		{
			Vector3D[] verts = new Vector3D[3];
			int index = 0;
			foreach (Thing t in things)
			{
				ThingData td = data.Mode.GetThingData(t);
				td.AddUpdateSector(data.Sector, true);
				verts[index] = t.Position;
				if (t.Args[0] == 0)
				{
					t.DetermineSector(data.Mode.BlockMap);
					if (t.Sector != null) verts[index].z += t.Sector.FloorHeight;
				}
				index++;
				if (index > 2) break; //Only the first three vertices are used
			}

			// Make new plane
			if (slopefloor)
				data.Floor.plane = new Plane(verts[0], verts[1], verts[2], true);
			else
				data.Ceiling.plane = new Plane(verts[0], verts[2], verts[1], false);
		}
	}
}
