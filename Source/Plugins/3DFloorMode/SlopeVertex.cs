#region ================== Namespaces

using System;
using System.Reflection;
using System.Collections.Generic;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Types;

#endregion

namespace CodeImp.DoomBuilder.ThreeDFloorMode
{
	public class SlopeVertex
	{
		#region ================== Variables

		// private Vector2D pos;
		private double x;
		private double y;
		private double z;
		private bool selected;

		#endregion

		#region ================== Constructors

		public SlopeVertex(Sector sector, int svgid, int vertexid)
		{
			string identifier;
			List<string> list = new List<string> { "x", "y", "z" };
			Type type = typeof(SlopeVertex);

			this.x = 0;
			this.y = 0;
			this.z = 0;
			this.selected = false;

			// Read the x, y and z values
			foreach (string str in list)
			{
				identifier = String.Format("user_svg{0}_v{1}_{2}", svgid, vertexid, str);

				// Use reflection to set the variable to the value
				type.GetField(str, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, sector.Fields.GetValue(identifier, 0.0));
			}
		}

		public SlopeVertex(Vector2D p, double z)
		{
			// this.pos = new Vector2D(p);
			this.x = p.x;
			this.y = p.y;
			this.z = z;
			this.selected = false;
		}

		#endregion

		#region ================== Properties

		public Vector2D Pos { get { return new Vector2D(x, y); } set { x = value.x; y = value.y; } }
		public double Z { get { return z; } set { z = value; } }
		public bool Selected { get { return selected; } set { selected = value; } }

		#endregion

		#region ================== Methods

		public void StoreInSector(Sector sector, int svgid, int vertexid)
		{
			string identifier;

			Dictionary<String, double> dict = new Dictionary<string, double>
			{
				{ "x", x },
				{ "y", y },
				{ "z", z }
			};

			// Make sure the field work with undo/redo
			sector.Fields.BeforeFieldsChange();

			// Process the x, y and z fields
			foreach (KeyValuePair<string, double> kvp in dict)
			{
				identifier = String.Format("user_svg{0}_v{1}_{2}", svgid, vertexid, kvp.Key);

				// Add or update the vertex field
				if (sector.Fields.ContainsKey(identifier))
					sector.Fields[identifier] = new UniValue(UniversalType.Float, kvp.Value);
				else
					sector.Fields.Add(identifier, new UniValue(UniversalType.Float, kvp.Value));
			}
		}

		#endregion
	}
}