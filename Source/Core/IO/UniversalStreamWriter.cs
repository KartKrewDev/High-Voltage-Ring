
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
using System.Text;
using System.IO;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Config;

#endregion

namespace CodeImp.DoomBuilder.IO
{
	internal class UniversalStreamWriter
	{
		#region ================== Constants

		// Name of the UDMF configuration file
		private const string UDMF_CONFIG_NAME = "UDMF.cfg";

		#endregion

		#region ================== Variables

		private Configuration config;
		private bool remembercustomtypes;
		
		#endregion

		#region ================== Properties

		public bool RememberCustomTypes { get { return remembercustomtypes; } set { remembercustomtypes = value; } }

		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public UniversalStreamWriter()
		{
			// Make configurations
			config = new Configuration();
			
			// Find a resource named UDMF.cfg
			string[] resnames = General.ThisAssembly.GetManifestResourceNames();
			foreach(string rn in resnames)
			{
				// Found it?
				if(rn.EndsWith(UDMF_CONFIG_NAME, StringComparison.OrdinalIgnoreCase))
				{
					// Get a stream from the resource
					Stream udmfcfg = General.ThisAssembly.GetManifestResourceStream(rn);
					StreamReader udmfcfgreader = new StreamReader(udmfcfg, Encoding.ASCII);

					// Load configuration from stream
					config.InputConfiguration(udmfcfgreader.ReadToEnd());

					// Now we add the linedef flags, activations and thing flags
					// to this list, so that these don't show up in the custom
					// fields list either. We use true as dummy value (it has no meaning)

					// Add linedef flags
					foreach(KeyValuePair<string, string> flag in General.Map.Config.LinedefFlags)
						config.WriteSetting("managedfields.linedef." + flag.Key, true);

					// Add linedef activations
					foreach(LinedefActivateInfo activate in General.Map.Config.LinedefActivates)
						config.WriteSetting("managedfields.linedef." + activate.Key, true);

					//mxd. Add sidedef flags
					foreach(KeyValuePair<string, string> flag in General.Map.Config.SidedefFlags)
						config.WriteSetting("managedfields.sidedef." + flag.Key, true);

					//mxd. Add sector flags
					foreach(KeyValuePair<string, string> flag in General.Map.Config.SectorFlags)
						config.WriteSetting("managedfields.sector." + flag.Key, true);
					foreach(KeyValuePair<string, string> flag in General.Map.Config.CeilingPortalFlags)
						config.WriteSetting("managedfields.sector." + flag.Key, true);
					foreach(KeyValuePair<string, string> flag in General.Map.Config.FloorPortalFlags)
						config.WriteSetting("managedfields.sector." + flag.Key, true);

					// Add sector activations
					foreach(LinedefActivateInfo activate in General.Map.Config.SectorActivates)
						config.WriteSetting("managedfields.sector." + activate.Key, true);

					// Add thing flags
					foreach(KeyValuePair<string, string> flag in General.Map.Config.ThingFlags)
						config.WriteSetting("managedfields.thing." + flag.Key, true);

					// Done
					udmfcfgreader.Dispose();
					break;
				}
			}
		}

		#endregion

		#region ================== Writing

		// This writes the structures to a stream
		// writenamespace may be null to omit writing the namespace to the stream
		public void Write(MapSet map, Stream stream, string writenamespace, int writeversion)
		{
			Write(map.Vertices, map.Linedefs, map.Sidedefs, map.Sectors, map.Things, map.UnknownUDMFData, stream, writenamespace, writeversion);
		}

		// This writes the structures to a stream
		// NOTE: writenamespace may be null to omit writing the namespace to the stream.
		// NOTE: The given structures must be complete, with the exception of the sidedefs.
		// If there are missing sidedefs, their reference will be removed from the linedefs.
		public void Write(ICollection<Vertex> vertices, ICollection<Linedef> linedefs,
						  ICollection<Sidedef> sidedefs, ICollection<Sector> sectors,
						  ICollection<Thing> things, ICollection<UniversalEntry> unknowndata, Stream stream, string writenamespace, int writeversion)
		{
			UniversalParser textmap = new UniversalParser();

			// Begin with fields that must be at the top
			if(writenamespace != null) textmap.Root.Add("namespace", writenamespace);
            if(writeversion != 0) textmap.Root.Add("version", writeversion);

            // Dump unknown fields at the top
            WriteUnknownData(unknowndata, textmap);

			Dictionary<Vertex, int> vertexids = new Dictionary<Vertex, int>(vertices.Count); //mxd
			Dictionary<Sidedef, int> sidedefids = new Dictionary<Sidedef, int>(sidedefs.Count); //mxd
			Dictionary<Sector, int> sectorids = new Dictionary<Sector, int>(sectors.Count); //mxd

			// Index the elements in the data structures
			int counter = 0; //mxd
			foreach(Vertex v in vertices) vertexids.Add(v, counter++);
			counter = 0; //mxd
			foreach(Sidedef sd in sidedefs) sidedefids.Add(sd, counter++);
			counter = 0; //mxd
			foreach(Sector s in sectors) sectorids.Add(s, counter++);

			// If we write the custom field types again, then forget
			// all previous field types (this gets rid of unused field types)
			if(remembercustomtypes) General.Map.Options.ForgetUniversalFieldTypes();

			// Write the data structures to textmap
			WriteVertices(vertices, textmap);
			WriteLinedefs(linedefs, textmap, sidedefids, vertexids);
			WriteSidedefs(sidedefs, textmap, sectorids);
			WriteSectors(sectors, textmap);
			WriteThings(things, textmap);

			// Get the textmap as string
			string textmapstr = textmap.OutputConfiguration();

			// Write to stream
			StreamWriter writer = new StreamWriter(stream, Encoding.ASCII);
			writer.Write(textmapstr);
			writer.Flush();
		}

		// This adds vertices
		private void WriteVertices(ICollection<Vertex> vertices, UniversalParser textmap)
		{
			// Go for all vertices
			foreach(Vertex v in vertices)
			{
				// Make collection
				UniversalCollection coll = new UniversalCollection();
				coll.Add("x", v.Position.x);
				coll.Add("y", v.Position.y);
				if(!double.IsNaN(v.ZCeiling)) coll.Add("zceiling", v.ZCeiling); //mxd
				if(!double.IsNaN(v.ZFloor)) coll.Add("zfloor", v.ZFloor); //mxd
				coll.Comment = v.Index.ToString();

				// Add custom fields
				AddCustomFields(v, "vertex", coll);

				// Store
				textmap.Root.Add("vertex", coll);
			}
		}

		// This adds linedefs
		private void WriteLinedefs(ICollection<Linedef> linedefs, UniversalParser textmap, IDictionary<Sidedef, int> sidedefids, IDictionary<Vertex, int> vertexids)
		{
			// Go for all linedefs
			foreach(Linedef l in linedefs)
			{
				// Make collection
				UniversalCollection coll = new UniversalCollection();
				if(l.Tag != 0) coll.Add("id", l.Tag);
				coll.Add("v1", vertexids[l.Start]);
				coll.Add("v2", vertexids[l.End]);
				coll.Comment = l.Index.ToString();

				//mxd. MoreIDs
				if(l.Tags.Count > 1) //first entry is saved as "id"
				{
					string[] moreidscol = new string[l.Tags.Count - 1];
					for(int i = 1; i < l.Tags.Count; i++)
					{
						moreidscol[i - 1] = l.Tags[i].ToString();
					}
					coll.Add("moreids", string.Join(" ", moreidscol));
				}
				
				// Sidedef references
				if((l.Front != null) && sidedefids.ContainsKey(l.Front))
					coll.Add("sidefront", sidedefids[l.Front]);
				else
					coll.Add("sidefront", -1);
				
				if((l.Back != null) && sidedefids.ContainsKey(l.Back))
					coll.Add("sideback", sidedefids[l.Back]);
				else
					coll.Add("sideback", -1);
				
				// Special
				if(l.Action != 0) coll.Add("special", l.Action);
				for (int i = 0; i < l.Args.Length; i++)
				{
					if (l.Args[i] != 0) coll.Add("arg" + i, l.Args[i]);
				}

				// Flags
				foreach(KeyValuePair<string, bool> flag in l.Flags)
					if(flag.Value) coll.Add(flag.Key, flag.Value);

				// Add custom fields
				AddCustomFields(l, "linedef", coll);

				// Store
				textmap.Root.Add("linedef", coll);
			}
		}

		// This adds sidedefs
		private void WriteSidedefs(ICollection<Sidedef> sidedefs, UniversalParser textmap, IDictionary<Sector, int> sectorids)
		{
			// Go for all sidedefs
			foreach(Sidedef s in sidedefs)
			{
				// Make collection
				UniversalCollection coll = new UniversalCollection();
				if(s.OffsetX != 0) coll.Add("offsetx", s.OffsetX);
				if(s.OffsetY != 0) coll.Add("offsety", s.OffsetY);
				if(s.LongHighTexture != MapSet.EmptyLongName) coll.Add("texturetop", s.HighTexture);
				if(s.LongLowTexture != MapSet.EmptyLongName) coll.Add("texturebottom", s.LowTexture);
				if(s.LongMiddleTexture != MapSet.EmptyLongName) coll.Add("texturemiddle", s.MiddleTexture);
				coll.Add("sector", sectorids[s.Sector]);
				coll.Comment = s.Index.ToString();

				//mxd. Flags
				foreach(KeyValuePair<string, bool> flag in s.Flags)
					if(flag.Value) coll.Add(flag.Key, flag.Value);
				
				// Add custom fields
				AddCustomFields(s, "sidedef", coll);

				// Store
				textmap.Root.Add("sidedef", coll);
			}
		}

		// This adds sectors
		private void WriteSectors(ICollection<Sector> sectors, UniversalParser textmap)
		{
			// Go for all sectors
			foreach(Sector s in sectors)
			{
				// Make collection
				UniversalCollection coll = new UniversalCollection();
				coll.Add("heightfloor", s.FloorHeight);
				coll.Add("heightceiling", s.CeilHeight);
				coll.Add("texturefloor", s.FloorTexture);
				coll.Add("textureceiling", s.CeilTexture);
				coll.Add("lightlevel", s.Brightness);
				if(s.Effect != 0) coll.Add("special", s.Effect);
				if(s.Tag != 0) coll.Add("id", s.Tag);
				coll.Comment = s.Index.ToString();

				//mxd. MoreIDs
				if(s.Tags.Count > 1) //first entry is saved as "id"
				{
					string[] moreidscol = new string[s.Tags.Count - 1];
					for(int i = 1; i < s.Tags.Count; i++) 
					{
						moreidscol[i - 1] = s.Tags[i].ToString();
					}
					coll.Add("moreids", string.Join(" ", moreidscol));
				}

				//mxd. Slopes
				if(s.FloorSlope.GetLengthSq() > 0) 
				{
					coll.Add("floorplane_a", s.FloorSlope.x);
					coll.Add("floorplane_b", s.FloorSlope.y);
					coll.Add("floorplane_c", s.FloorSlope.z);
					coll.Add("floorplane_d", double.IsNaN(s.FloorSlopeOffset) ? 0.0 : s.FloorSlopeOffset);
				}

				if (s.CeilSlope.GetLengthSq() > 0) 
				{
					coll.Add("ceilingplane_a", s.CeilSlope.x);
					coll.Add("ceilingplane_b", s.CeilSlope.y);
					coll.Add("ceilingplane_c", s.CeilSlope.z);
					coll.Add("ceilingplane_d", double.IsNaN(s.CeilSlopeOffset) ? 0.0 : s.CeilSlopeOffset);
				}

				if(s.Action != 0) coll.Add("action", s.Action);
				for (int i = 0; i < s.Args.Length; i++)
				{
					if (s.Args[i] != 0) coll.Add("arg" + i, s.Args[i]);
				}

				//mxd. Flags
				foreach(KeyValuePair<string, bool> flag in s.Flags)
					if(flag.Value) coll.Add(flag.Key, flag.Value);

				// Add custom fields
				AddCustomFields(s, "sector", coll);

				// Store
				textmap.Root.Add("sector", coll);
			}
		}

		// This adds things
		private void WriteThings(ICollection<Thing> things, UniversalParser textmap)
		{
			// Go for all things
			foreach(Thing t in things)
			{
				// Make collection
				UniversalCollection coll = new UniversalCollection();
				if(t.Tag != 0) coll.Add("id", t.Tag);
				coll.Add("x", t.Position.x);
				coll.Add("y", t.Position.y);
				if(t.Position.z != 0.0f) coll.Add("height", t.Position.z);
				coll.Add("angle", t.AngleDoom);
				if(t.Pitch != 0) coll.Add("pitch", t.Pitch); //mxd
				if(t.Roll != 0) coll.Add("roll", t.Roll); //mxd
				if(t.ScaleX != 0 && t.ScaleX != 1.0f) coll.Add("scalex", t.ScaleX); //mxd
				if(t.ScaleY != 0 && t.ScaleY != 1.0f) coll.Add("scaley", t.ScaleY); //mxd
                if (t.MobjScale != 0 && t.MobjScale != 1.0f) coll.Add("mobjscale", t.MobjScale);
                coll.Add("type", t.Type);
				if(t.Action != 0) coll.Add("special", t.Action);
				for (int i = 0; i < t.Args.Length; i++)
				{
					if (t.Args[i] != 0) coll.Add("arg" + i, t.Args[i]);
				}
                for (int i = 0; i < t.ThingArgs.Length; i++)
                {
                    if (t.ThingArgs[i] != 0) coll.Add("thingarg" + i, t.ThingArgs[i]);
                }
                coll.Comment = t.Index.ToString();

				// Flags
				foreach(KeyValuePair<string, bool> flag in t.Flags)
					if(flag.Value) coll.Add(flag.Key, flag.Value);

				// Add custom fields
				AddCustomFields(t, "thing", coll);

				// Store
				textmap.Root.Add("thing", coll);
			}
		}

		/// <summary>
		/// This writes UDMF data UDB doesn't know about to the map.
		/// </summary>
		/// <param name="data">Collection of universal entries.</param>
		/// <param name="textmap">The map</param>
		private void WriteUnknownData(ICollection<UniversalEntry> data, UniversalParser textmap)
		{
			foreach (UniversalEntry e in data)
			{
				if (e.Value is UniversalCollection)
				{
					UniversalCollection coll = new UniversalCollection();

					foreach (UniversalEntry ie in (UniversalCollection)e.Value)
						coll.Add(ie.Key, ie.Value);

					textmap.Root.Add(e.Key, coll);
				}
				else
				{
					textmap.Root.Add(e);
				}
			}
		}

		// This adds custom fields from a map element to a collection
		private void AddCustomFields(MapElement element, string elementname, UniversalCollection collection)
		{
			// Add custom fields
			foreach(KeyValuePair<string, UniValue> f in element.Fields)
			{
				// Not a managed field?
				if(!config.SettingExists("managedfields." + elementname + "." + f.Key))
				{
					// Add type information to DBS file for map
					if(remembercustomtypes)
						General.Map.Options.SetUniversalFieldType(elementname, f.Key, f.Value.Type);

					// Store field
					collection.Add(f.Key, f.Value.Value);
				}
			}
		}

		#endregion
	}
}

