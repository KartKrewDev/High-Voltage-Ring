
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

#endregion

namespace CodeImp.DoomBuilder.Geometry
{
	public struct Vector2D
	{
		#region ================== Constants

		private const double TINY_VALUE = 0.0000000001f;

		#endregion

		#region ================== Variables

		// Coordinates
		public double x;
		public double y;
		
		#endregion
		
		#region ================== Constructors

		// Constructor
		public Vector2D(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		// Constructor
		public Vector2D(Vector3D v)
		{
			this.x = v.x;
			this.y = v.y;
		}
		
		#endregion

		#region ================== Statics

		// Conversion to Vector3D
		public static implicit operator Vector3D(Vector2D a)
		{
			return new Vector3D(a);
		}

		// This adds two vectors
		public static Vector2D operator +(Vector2D a, Vector2D b)
		{
			return new Vector2D(a.x + b.x, a.y + b.y);
		}

		// This adds to a vector
		public static Vector2D operator +(double a, Vector2D b)
		{
			return new Vector2D(a + b.x, a + b.y);
		}

		// This adds to a vector
		public static Vector2D operator +(Vector2D a, double b)
		{
			return new Vector2D(a.x + b, a.y + b);
		}

		// This subtracts two vectors
		public static Vector2D operator -(Vector2D a, Vector2D b)
		{
			return new Vector2D(a.x - b.x, a.y - b.y);
		}

		// This subtracts from a vector
		public static Vector2D operator -(Vector2D a, double b)
		{
			return new Vector2D(a.x - b, a.y - b);
		}

		// This subtracts from a vector
		public static Vector2D operator -(double a, Vector2D b)
		{
			return new Vector2D(a - b.x, a - b.y);
		}

		// This reverses a vector
		public static Vector2D operator -(Vector2D a)
		{
			return new Vector2D(-a.x, -a.y);
		}

		// This scales a vector
		public static Vector2D operator *(double s, Vector2D a)
		{
			return new Vector2D(a.x * s, a.y * s);
		}

		// This scales a vector
		public static Vector2D operator *(Vector2D a, double s)
		{
			return new Vector2D(a.x * s, a.y * s);
		}

		// This scales a vector
		public static Vector2D operator *(Vector2D a, Vector2D b)
		{
			return new Vector2D(a.x * b.x, a.y * b.y);
		}

		// This scales a vector
		public static Vector2D operator /(double s, Vector2D a)
		{
			return new Vector2D(a.x / s, a.y / s);
		}

		// This scales a vector
		public static Vector2D operator /(Vector2D a, double s)
		{
			return new Vector2D(a.x / s, a.y / s);
		}

		// This scales a vector
		public static Vector2D operator /(Vector2D a, Vector2D b)
		{
			return new Vector2D(a.x / b.x, a.y / b.y);
		}

		// This calculates the dot product
		public static double DotProduct(Vector2D a, Vector2D b)
		{
			// Calculate and return the dot product
			return a.x * b.x + a.y * b.y;
		}

		// This calculates the cross product
		public static Vector2D CrossProduct(Vector2D a, Vector2D b)
		{
			Vector2D result = new Vector2D();

			// Calculate and return the dot product
			result.x = a.y * b.x;
			result.y = a.x * b.y;
			return result;
		}
		
		// This compares a vector
		public static bool operator ==(Vector2D a, Vector2D b)
		{
			return (a.x == b.x) && (a.y == b.y);
		}

		// This compares a vector
		public static bool operator !=(Vector2D a, Vector2D b)
		{
			return (a.x != b.x) || (a.y != b.y);
		}
		
		// This reflects the vector v over mirror m
		// Note that mirror m must be normalized!
		// R = V - 2 * M * (M dot V)
		public static Vector2D Reflect(Vector2D v, Vector2D m)
		{
			// Get the dot product of v and m
			double dp = Vector2D.DotProduct(m, v);

			// Make the reflected vector
			Vector2D mv = new Vector2D();
			mv.x = v.x - (2f * m.x * dp);
			mv.y = v.y - (2f * m.y * dp);

			// Return the reflected vector
			return mv;
		}

		// This returns the reversed vector
		public static Vector2D Reversed(Vector2D v)
		{
			// Return reversed vector
			return new Vector2D(-v.x, -v.y);
		}

		// This returns a vector from an angle
		public static Vector2D FromAngle(double angle)
		{
			// Return vector from angle
			return new Vector2D(Math.Sin(angle), -Math.Cos(angle));
		}

		// This returns a vector from an angle with a given legnth
		public static Vector2D FromAngle(double angle, double length)
		{
			// Return vector from angle
			return FromAngle(angle) * length;
		}

		// This calculates the angle
		public static double GetAngle(Vector2D a, Vector2D b)
		{
			// Calculate and return the angle
			return -Math.Atan2(-(a.y - b.y), (a.x - b.x)) + Angle2D.PIHALF;//mxd //(float)Math.PI * 0.5f;
		}

		// This returns the square distance between two points
		public static double DistanceSq(Vector2D a, Vector2D b)
		{
			Vector2D d = a - b;
			return d.GetLengthSq();
		}

		// This returns the distance between two points
		public static double Distance(Vector2D a, Vector2D b)
		{
			Vector2D d = a - b;
			return d.GetLength();
		}

		// This returns the manhattan distance between two points
		public static double ManhattanDistance(Vector2D a, Vector2D b)
		{
			Vector2D d = a - b;
			return Math.Abs(d.x) + Math.Abs(d.y);
		}
		
		#endregion

		#region ================== Methods

		// This returns the perpendicular vector by simply making a normal
		public Vector2D GetPerpendicular()
		{
			return new Vector2D(-y, x);
		}
		
		// This returns a vector with the sign of all components
		public Vector2D GetSign()
		{
			return new Vector2D(Math.Sign(x), Math.Sign(y));
		}
		
		// This calculates the angle
		public double GetAngle()
		{
			//mxd. Let's make sure the angle is in [0 .. PI2] range...
			double angle = -Math.Atan2(-y, x) + Angle2D.PIHALF;
			if(angle < 0f) angle += Angle2D.PI2;
			return angle;
		}

		// This calculates the length
		public double GetLength()
		{
			// Calculate and return the length
			return Math.Sqrt(x * x + y * y);
		}

		// This calculates the square length
		public double GetLengthSq()
		{
			// Calculate and return the square length
			return x * x + y * y;
		}

		// This calculates the length
		public double GetManhattanLength()
		{
			// Calculate and return the length
			return Math.Abs(x) + Math.Abs(y);
		}

		// This returns a normalized vector
		public Vector2D GetNormal()
		{
			double lensq = this.GetLengthSq();
			if(lensq > TINY_VALUE)
			{
				// Divide each element by the length
				double mul = 1f / Math.Sqrt(lensq);
				return new Vector2D(x * mul, y * mul);
			}
			else
			{
				// Cannot make normal
				return new Vector2D(0f, 0f);
			}
		}

		// This scales the vector
		public Vector2D GetScaled(double s)
		{
			// Scale the vector
			return new Vector2D(x * s, y * s);
		}

		// This changes the vector length
		public Vector2D GetFixedLength(double l)
		{
			// Normalize, then scale
			return this.GetNormal().GetScaled(l);
		}
		
		// Output
		public override string ToString()
		{
			return x + ", " + y;
		}

		// Transform
		public Vector2D GetTransformed(double offsetx, double offsety, double scalex, double scaley)
		{
			return new Vector2D((x + offsetx) * scalex, (y + offsety) * scaley);
		}

		// Inverse Transform
		public Vector2D GetInvTransformed(double invoffsetx, double invoffsety, double invscalex, double invscaley)
		{
			return new Vector2D((x * invscalex) + invoffsetx, (y * invscaley) + invoffsety);
		}
		
		// Rotate (Added by Anders Åstrand 2008-05-18)
		public Vector2D GetRotated(double theta)
		{
			double cos = Math.Cos(theta);
			double sin = Math.Sin(theta);
			double rx = cos * x - sin * y;
			double ry = sin * x + cos * y;
			return new Vector2D(rx, ry);
		}

		// Checks if the Vector has valid values for x and y
		public bool IsFinite()
		{
			return !double.IsNaN(x) && !double.IsNaN(y) && !double.IsInfinity(x) && !double.IsInfinity(y);
		}

		//mxd. Addeed to make compiler a bit more happy...
		public override int GetHashCode() 
		{
			return base.GetHashCode();
		}

		//mxd. Addeed to make compiler a bit more happy...
		public override bool Equals(object obj) 
		{
			if(!(obj is Vector2D)) return false;

			Vector2D other = (Vector2D)obj;

			if(x != other.x) return false;
			if(y != other.y) return false;
			return true;
		}

		#endregion
	}
}
