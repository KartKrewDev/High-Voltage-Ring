
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

using System.Collections.Generic;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.VisualModes
{
	public sealed class VisualBlockEntry
	{
        public List<Linedef> Lines { get; set; }
		public List<Thing> Things { get; set; }
		public List<Sector> Sectors { get; set; }
		
		internal VisualBlockEntry()
		{
			Lines = new List<Linedef>(2);
			Things = new List<Thing>(2);
			Sectors = new List<Sector>(2);
		}
	}
}
