
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
using CodeImp.DoomBuilder.Rendering;

#endregion

namespace CodeImp.DoomBuilder.Geometry
{
	public struct Vector3D
	{
		#region ================== Constants

		private const double TINY_VALUE = 0.0000000001f;
		
		#endregion

		#region ================== Variables

		// Coordinates
		public double x;
		public double y;
		public double z;

		#endregion

		#region ================== Constructors

		// Constructor
		public Vector3D(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Constructor
		public Vector3D(Vector2D v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = 0f;
		}

		// Constructor (mxd)
		public Vector3D(Vector2D v, double z) 
		{
			this.x = v.x;
			this.y = v.y;
			this.z = z;
		}
		
		#endregion

		#region ================== Statics

		// Conversion to Vector2D
		public static implicit operator Vector2D(Vector3D a)
		{
			return new Vector2D(a);
		}

		// This adds two vectors
		public static Vector3D operator +(Vector3D a, Vector3D b)
		{
			return new Vector3D(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// This adds to all dimensions
		public static Vector3D operator +(Vector3D a, double b)
		{
			return new Vector3D(a.x + b, a.y + b, a.z + b);
		}

		// This adds to all dimensions
		public static Vector3D operator +(double b, Vector3D a)
		{
			return new Vector3D(a.x + b, a.y + b, a.z + b);
		}

		// This subtracts two vectors
		public static Vector3D operator -(Vector3D a, Vector3D b)
		{
			return new Vector3D(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// This subtracts from all dimensions
		public static Vector3D operator -(Vector3D a, double b)
		{
			return new Vector3D(a.x - b, a.y - b, a.z - b);
		}

		// This subtracts from all dimensions
		public static Vector3D operator -(double a, Vector3D b)
		{
			return new Vector3D(a - b.x, a - b.y, a - b.z);
		}

		// This reverses a vector
		public static Vector3D operator -(Vector3D a)
		{
			return new Vector3D(-a.x, -a.y, -a.z);
		}

		// This scales a vector
		public static Vector3D operator *(double s, Vector3D a)
		{
			return new Vector3D(a.x * s, a.y * s, a.z * s);
		}

		// This scales a vector
		public static Vector3D operator *(Vector3D a, double s)
		{
			return new Vector3D(a.x * s, a.y * s, a.z * s);
		}

		// This scales a vector
		public static Vector3D operator *(Vector3D a, Vector3D b)
		{
			return new Vector3D(a.x * b.x, a.y * b.y, a.z * b.z);
		}
		
		// This scales a vector
		public static Vector3D operator /(double s, Vector3D a)
		{
			return new Vector3D(a.x / s, a.y / s, a.z / s);
		}

		// This scales a vector
		public static Vector3D operator /(Vector3D a, double s)
		{
			return new Vector3D(a.x / s, a.y / s, a.z / s);
		}

		// This scales a vector
		public static Vector3D operator /(Vector3D a, Vector3D b)
		{
			return new Vector3D(a.x / b.x, a.y / b.y, a.z / b.z);
		}

		// This compares a vector
		public static bool operator ==(Vector3D a, Vector3D b)
		{
			return (a.x == b.x) && (a.y == b.y) && (a.z == b.z);
		}

		// This compares a vector
		public static bool operator !=(Vector3D a, Vector3D b)
		{
			return (a.x != b.x) || (a.y != b.y) || (a.z != b.z);
		}

		// This calculates the cross product
		public static Vector3D CrossProduct(Vector3D a, Vector3D b)
		{
			Vector3D result = new Vector3D();

			// Calculate and return the dot product
			result.x = a.y * b.z - a.z * b.y;
			result.y = a.z * b.x - a.x * b.z;
			result.z = a.x * b.y - a.y * b.x;
			return result;
		}

		// This calculates the dot product
		public static double DotProduct(Vector3D a, Vector3D b)
		{
			// Calculate and return the dot product
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		// This reflects the vector v over mirror m
		// Note that mirror m must be normalized!
		public static Vector3D Reflect(Vector3D v, Vector3D m)
		{
			// Get the dot product of v and m
			double dp = Vector3D.DotProduct(v, m);

			// Make the reflected vector
			Vector3D mv = new Vector3D();
			mv.x = -v.x + 2f * m.x * dp;
			mv.y = -v.y + 2f * m.y * dp;
			mv.z = -v.z + 2f * m.z * dp;

			// Return the reflected vector
			return mv;
		}

		// This returns the reversed vector
		public static Vector3D Reversed(Vector3D v)
		{
			// Return reversed vector
			return new Vector3D(-v.x, -v.y, -v.z);
		}

		// This returns a vector from an angle
		public static Vector3D FromAngleXY(double angle)
		{
			// Return vector from angle
			return new Vector3D(Math.Sin(angle), -Math.Cos(angle), 0f);
		}
		
		// This returns a vector from an angle with a given legnth
		public static Vector3D FromAngleXY(double angle, double length)
		{
			// Return vector from angle
			return FromAngleXY(angle) * length;
		}

		// This returns a vector from an angle with a given legnth
		public static Vector3D FromAngleXYZ(double anglexy, double anglez)
		{
			// Calculate x y and z
			double ax = Math.Sin(anglexy) * Math.Cos(anglez);
			double ay = -Math.Cos(anglexy) * Math.Cos(anglez);
			double az = Math.Sin(anglez);

			// Return vector
			return new Vector3D(ax, ay, az);
		}

		//mxd
		public static Vector3D Transform(Vector3D v, Matrix m)
		{
			return new Vector3D
			{
				x = m.M11 * v.x + m.M21 * v.y + m.M31 * v.z + m.M41,
				y = m.M12 * v.x + m.M22 * v.y + m.M32 * v.z + m.M42,
				z = m.M13 * v.x + m.M23 * v.y + m.M33 * v.z + m.M43,
			};
		}

		//mxd
		public static Vector3D Transform(double x, double y, double z, Matrix m)
		{
			return new Vector3D
			{
				x = m.M11 * x + m.M21 * y + m.M31 * z + m.M41,
				y = m.M12 * x + m.M22 * y + m.M32 * z + m.M42,
				z = m.M13 * x + m.M23 * y + m.M33 * z + m.M43,
			};
		}
		
		#endregion
		
		#region ================== Methods

		// This calculates the angle
		public double GetAngleXY()
		{
			// Calculate and return the angle
			return -Math.Atan2(-y, x) + Angle2D.PIHALF;//mxd // (float)Math.PI * 0.5f;
		}

		// This calculates the angle
		public double GetAngleZ()
		{
			Vector2D xy = new Vector2D(x, y);

			// Calculate and return the angle
			return Math.Atan2(xy.GetLength(), z) + Angle2D.PIHALF;//mxd // (float)Math.PI * 0.5f;
		}

		// This calculates the length
		public double GetLength()
		{
			// Calculate and return the length
			return Math.Sqrt(x * x + y * y + z * z);
		}

		// This calculates the squared length
		public double GetLengthSq()
		{
			// Calculate and return the length
			return x * x + y * y + z * z;
		}

		// This calculates the length
		public double GetManhattanLength()
		{
			// Calculate and return the length
			return Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
		}

		// This normalizes the vector
		public Vector3D GetNormal()
		{
			double lensq = this.GetLengthSq();
			if(lensq > TINY_VALUE)
			{
				// Divide each element by the length
				double mul = 1f / Math.Sqrt(lensq);
				return new Vector3D(x * mul, y * mul, z * mul);
			}
			else
			{
				// Cannot normalize
				return new Vector3D(0f, 0f, 0f);
			}
		}

		// This scales the vector
		public Vector3D GetScaled(double s)
		{
			// Scale the vector
			return new Vector3D(x * s, y * s, z * s);
		}

		// This changes the vector length
		public Vector3D GetFixedLength(double l)
		{
			// Normalize, then scale
			return this.GetNormal().GetScaled(l);
		}
		
		// This checks if the vector is normalized
		public bool IsNormalized()
		{
			return (Math.Abs(GetLengthSq() - 1.0f) < 0.0001f);
		}
		
		// Output
		public override string ToString()
		{
			return x + ", " + y + ", " + z;
		}

		// Checks if the Vector has valid values for x, y and z
		public bool IsFinite()
		{
			return !double.IsNaN(x) && !double.IsNaN(y) && !double.IsNaN(z) && !double.IsInfinity(x) && !double.IsInfinity(y) && !double.IsInfinity(z);
		}

		//mxd. Addeed to make compiler a bit more happy...
		public override int GetHashCode() 
		{
			return base.GetHashCode();
		}

		//mxd. Addeed to make compiler a bit more happy...
		public override bool Equals(object obj) 
		{
			if(!(obj is Vector3D)) return false;

			Vector3D other = (Vector3D)obj;

			if(x != other.x) return false;
			if(y != other.y) return false;
			if(z != other.z) return false;
			return true;
		}

		
		#endregion
	}
}
