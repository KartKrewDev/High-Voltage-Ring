	
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
	public struct Angle2D
	{
		#region ================== Constants

		public const double PI = Math.PI;
		public const double PIHALF = Math.PI * 0.5;
		public const double PI2 = Math.PI * 2;
		public const double PIDEG = 57.295779513082320876798154814105;
		public const double SQRT2 = 1.4142135623730950488016887242097;
		
		#endregion

		#region ================== Methods

		// This converts doom angle to real angle
		public static double DoomToReal(int doomangle)
		{
			return Math.Round(Normalized(DegToRad((doomangle + 90))), 4);
		}

		// This converts real angle to doom angle
		public static int RealToDoom(double realangle)
		{
			return (int)Math.Round(RadToDeg(Normalized(realangle - PIHALF)));
		}

		// This converts degrees to radians
		public static double DegToRad(double deg)
		{
			return deg / PIDEG;
		}

		// This converts radians to degrees
		public static double RadToDeg(double rad)
		{
			return rad * PIDEG;
		}

		// This normalizes an angle
		public static double Normalized(double a)
		{
			while(a < 0f) a += PI2;
			while(a >= PI2) a -= PI2;
			return a;
		}

		// This returns the difference between two angles
		public static double Difference(double a, double b)
		{
			// Calculate delta angle
			double d = Normalized(a) - Normalized(b);

			// Make corrections for zero barrier
			if(d < 0f) d += PI2;
			if(d > PI) d = PI2 - d;

			// Return result
			return d;
		}

		//mxd. Slade 3 MathStuff::angle2DRad ripoff...
		//Returns the angle between the 2d points [p1], [p2] and [p3]
		public static double GetAngle(Vector2D p1, Vector2D p2, Vector2D p3)
		{
			// From: http://stackoverflow.com/questions/3486172/angle-between-3-points
			// modified not to bother converting to degrees
			Vector2D ab = new Vector2D(p2.x - p1.x, p2.y - p1.y);
			Vector2D cb = new Vector2D(p2.x - p3.x, p2.y - p3.y);

			// dot product
			double dot = (ab.x * cb.x + ab.y * cb.y);

			// length square of both vectors
			double abSqr = ab.x * ab.x + ab.y * ab.y;
			double cbSqr = cb.x * cb.x + cb.y * cb.y;

			// square of cosine of the needed angle
			double cosSqr = dot * dot / abSqr / cbSqr;

			// this is a known trigonometric equality:
			// cos(alpha * 2) = [ cos(alpha) ]^2 * 2 - 1
			double cos2 = 2.0f * cosSqr - 1.0f;

			// Here's the only invocation of the heavy function.
			// It's a good idea to check explicitly if cos2 is within [-1 .. 1] range
			double alpha2 =
				(cos2 <= -1) ? PI :
				(cos2 >= 1) ? 0.0 :
				Math.Acos(cos2);

			double rs = alpha2 * 0.5;

			// Now revolve the ambiguities.
			// 1. If dot product of two vectors is negative - the angle is definitely
			// above 90 degrees. Still we have no information regarding the sign of the angle.

			// NOTE: This ambiguity is the consequence of our method: calculating the cosine
			// of the double angle. This allows us to get rid of calling sqrt.
			if(dot < 0) rs = PI - rs;

			// 2. Determine the sign. For this we'll use the Determinant of two vectors.
			double det = (ab.x * cb.y - ab.y * cb.x);
			if(det < 0) rs = (2.0 * PI) - rs;

			return rs;
		}
		
		#endregion
	}
}
