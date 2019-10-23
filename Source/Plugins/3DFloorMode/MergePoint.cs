#region ================== Copyright (c) 2014 Boris Iwanski

/*
 * Copyright (c) 2014 Boris Iwanski
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

using System.Collections.Generic;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;

namespace CodeImp.DoomBuilder.ThreeDFloorMode
{
	public class MergePoint
	{
		private Vector2D position;
		private int floorheight;
		private int ceilingheight;
		private string floortexture;
		private string ceilingtexture;
		private List<ThreeDFloor> threedfloors;

		public Vector2D Position { get { return position; } set { position = value; } }
		public int FloorHeight { get { return floorheight; } set { floorheight = value; } }
		public int CeilingHeight { get { return ceilingheight; } set { ceilingheight = value; } }
		public string FloorTexture { get { return floortexture; } set { floortexture = value; } }
		public string CeilingTexture { get { return ceilingtexture; } set { ceilingtexture = value; } }
		public List<ThreeDFloor> ThreeDFloors { get { return threedfloors; } }

		public MergePoint(Vector2D position)
		{
			this.position = position;
			threedfloors = new List<ThreeDFloor>();
		}

		public void Update()
		{
			List<Sector> sectors = new List<Sector>();

			foreach (Sector s in General.Map.Map.Sectors)
			{
				if (s.Intersect(position))
					sectors.Add(s);
			}

			if (sectors.Count == 0)
				return;

			floorheight = sectors[0].FloorHeight;
			ceilingheight = sectors[0].CeilHeight;

			floortexture = sectors[0].FloorTexture;
			ceilingtexture = sectors[0].CeilTexture;

			foreach (Sector s in sectors)
			{
				if (s.FloorHeight < floorheight)
				{
					floorheight = s.FloorHeight;
					floortexture = s.FloorTexture;
				}

				if (s.CeilHeight > ceilingheight)
				{
					ceilingheight = s.CeilHeight;
					ceilingtexture = s.CeilTexture;
				}
			}
		}
	}
}