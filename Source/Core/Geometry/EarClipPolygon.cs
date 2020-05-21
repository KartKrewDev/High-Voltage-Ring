
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

#endregion

namespace CodeImp.DoomBuilder.Geometry
{
	[Serializable]
	public sealed class EarClipPolygon : LinkedList<EarClipVertex>
	{
		#region ================== Variables

		// Tree variables
		private List<EarClipPolygon> children;
		private bool inner;

		#endregion

		#region ================== Properties

		public List<EarClipPolygon> Children { get { return children; } }
		public bool Inner { get { return inner; } set { inner = value; } }

		#endregion

		#region ================== Constructors

		// Constructor
		internal EarClipPolygon()
		{
			// Initialize
			children = new List<EarClipPolygon>();
		}

		// Constructor
		internal EarClipPolygon(EarClipPolygon p, EarClipVertex add) : base(p)
		{
			// Initialize
			base.AddLast(add);
			children = new List<EarClipPolygon>();
		}

		#endregion
		
		#region ================== Methods

		// This merges a polygon into this one
		public void Add(EarClipPolygon p)
		{
			// Initialize
			foreach(EarClipVertex v in p) base.AddLast(v);
		}

		// This calculates the area
		public double CalculateArea()
		{
			double area = 0;
            LinkedListNode<EarClipVertex> v = First;
            do
            {

                EarClipVertex v1 = v.Value;
                EarClipVertex v2 = (v.Next != null) ? v.Next.Value : First.Value;

                area += (v2.Position.x + v1.Position.x) * (v2.Position.y - v1.Position.y);

                v = v.Next;

            }
            while (v != null);
			return Math.Abs(area * 0.5f);
		}
		
		// This creates a bounding box from the outer polygon
		public RectangleF CreateBBox()
		{
			double left = float.MaxValue;
			double right = float.MinValue;
			double top = float.MaxValue;
			double bottom = float.MinValue;
			foreach(EarClipVertex v in this)
			{
				if(v.Position.x < left) left = v.Position.x;
				if(v.Position.x > right) right = v.Position.x;
				if(v.Position.y < top) top = v.Position.y;
				if(v.Position.y > bottom) bottom = v.Position.y;
			}
			return new RectangleF((float)left, (float)top, (float)(right - left), (float)(bottom - top));
		}
		
		// Point inside the polygon?
		// See: http://paulbourke.net/geometry/polygonmesh/index.html#insidepoly
		public bool Intersect(Vector2D p)
		{
			Vector2D v1 = base.Last.Value.Position;
			LinkedListNode<EarClipVertex> n = base.First;
			uint c = 0;
			Vector2D v2;
			
			// Go for all vertices
			while(n != null)
			{
				// Get next vertex
				v2 = n.Value.Position;

				// Check for intersection
				if(v1.y != v2.y //mxd. If line is not horizontal...
				  && p.y >  (v1.y < v2.y ? v1.y : v2.y) //mxd. ...And test point y intersects with the line y bounds...
				  && p.y <= (v1.y > v2.y ? v1.y : v2.y) //mxd
				  && (p.x < (v1.x < v2.x ? v1.x : v2.x) || (p.x <= (v1.x > v2.x ? v1.x : v2.x) //mxd. ...And test point x is to the left of the line, or is inside line x bounds and intersects it
						&& (v1.x == v2.x || p.x <= ((p.y - v1.y) * (v2.x - v1.x) / (v2.y - v1.y) + v1.x))))) 
					c++; //mxd. ...Count the line as crossed

				// Move to next
				v1 = v2;
				n = n.Next;
			}

			// Inside this polygon when we crossed odd number of polygon lines
			if(c % 2 != 0)
			{
				// Check if not inside the children
				foreach(EarClipPolygon child in children)
				{
					// Inside this child? Then it is not inside this polygon.
					if(child.Intersect(p)) return false;
				}

				// Inside polygon!
				return true;
			}

			// Not inside the polygon
			return false;
		}
		
		// This inserts a polygon if it is a child of this one
		public bool InsertChild(EarClipPolygon p)
		{
			// Polygon must have at least 1 vertex
			if(p.Count == 0) return false;
			
			// Check if it can be inserted at a lower level
			foreach(EarClipPolygon child in children)
			{
				if(child.InsertChild(p)) return true;
			}

			// Check if it can be inserted here
			if(this.Intersect(p.First.Value.Position))
			{
				// Make the polygon the inverse of this one
				p.Inner = !inner;
				children.Add(p);
				return true;
			}

			// Can't insert it as a child
			return false;
		}
		
		#endregion
	}
}
