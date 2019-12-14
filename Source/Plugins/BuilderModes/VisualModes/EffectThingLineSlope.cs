#region === Copyright (c) 2010 Pascal van der Heiden ===

using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	internal class EffectThingLineSlope : SectorEffect
	{
		// Thing used to create this effect
		// The thing is in the sector that must receive the slope and the
		// Thing's arg 0 indicates the linedef to start the slope at.
		private Thing thing;
		private Sidedef sidedef;
		
		// Constructor
		public EffectThingLineSlope(SectorData data, Thing sourcething, Sidedef sourcesidedef) : base(data)
		{
			thing = sourcething;
			sidedef = sourcesidedef;
			
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
			//mxd. Skip if arg0 is 0.
			if(thing.Args[0] == 0) return;
			
			ThingData td = data.Mode.GetThingData(thing);
			Thing t = thing;
			Linedef ld = sidedef.Line;

			if(ld != null)
			{
				if(t.Type == 9500)
				{
					SectorData sd = data.Mode.GetSectorData(sidedef.Sector);
					Vector3D v1 = new Vector3D(ld.Start.Position.x, ld.Start.Position.y, sd.Floor.plane.GetZ(ld.Start.Position));
					Vector3D v2 = new Vector3D(ld.End.Position.x, ld.End.Position.y, sd.Floor.plane.GetZ(ld.End.Position));
					Vector3D v3 = new Vector3D(t.Position.x, t.Position.y, t.Position.z + sd.Floor.plane.GetZ(t.Position));
					sd.AddUpdateSector(data.Sector, true);
					if (!sd.Updated) sd.Update();
					td.AddUpdateSector(sidedef.Sector, true);
					sd.Floor.plane = new Plane(v1, v2, v3, true);
				}
				else if(t.Type == 9501)
				{
					SectorData sd = data.Mode.GetSectorData(sidedef.Sector);
					Vector3D v1 = new Vector3D(ld.Start.Position.x, ld.Start.Position.y, sd.Ceiling.plane.GetZ(ld.Start.Position));
					Vector3D v2 = new Vector3D(ld.End.Position.x, ld.End.Position.y, sd.Ceiling.plane.GetZ(ld.End.Position));
					Vector3D v3 = new Vector3D(t.Position.x, t.Position.y, t.Position.z + sd.Ceiling.plane.GetZ(t.Position));
					sd.AddUpdateSector(data.Sector, true);
					if (!sd.Updated) sd.Update();
					td.AddUpdateSector(sidedef.Sector, true);
					sd.Ceiling.plane = new Plane(v1, v2, v3, false);
				}
			}
		}
	}
}
