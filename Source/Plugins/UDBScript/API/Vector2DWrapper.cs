#region ================== Copyright (c) 2021 Boris Iwanski

/*
 * This program is free software: you can redistribute it and/or modify
 *
 * it under the terms of the GNU General Public License as published by
 * 
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 * 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * 
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.If not, see<http://www.gnu.org/licenses/>.
 */

#endregion

#region ================== Namespaces

using System.Dynamic;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;

#endregion

namespace CodeImp.DoomBuilder.UDBScript.Wrapper
{
	internal struct Vector2DWrapper
	{
		#region ================== Variables

		public double _x;
		public double _y;
		private readonly MapElement parent;

		#endregion

		#region ================== Properties

		/// <summary>
		/// The `x` value of the vector.
		/// </summary>
		public double x
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;

				if (parent is Vertex)
					((Vertex)parent).Move(new Vector2D(_x, _y));
				else if (parent is Thing)
					((Thing)parent).Move(new Vector2D(_x, _y));
			}
		}

		/// <summary>
		/// The `y` value of the vector.
		/// </summary>
		public double y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;

				if (parent is Vertex)
					((Vertex)parent).Move(new Vector2D(_x, _y));
				else if (parent is Thing)
					((Thing)parent).Move(new Vector2D(_x, _y));
			}
		}

		#endregion

		#region ================== Constructors

		internal Vector2DWrapper(Vector2D v, MapElement parent = null)
		{
			_x = v.x;
			_y = v.y;
			this.parent = parent;
		}

		internal Vector2DWrapper(double x, double y, MapElement parent)
		{
			_x = x;
			_y = y;
			this.parent = parent;
		}

		/// <summary>
		/// Creates a new `Vector2D` from x and y coordinates
		/// ```
		/// let v = new UDB.Vector2D(32, 64);
		/// ```
		/// </summary>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		public Vector2DWrapper(double x, double y)
		{
			this._x = x;
			this._y = y;
			parent = null;
		}

		/// <summary>
		/// Creates a new `Vector2D` from a point.
		/// ```
		/// let v = new UDB.Vector2D([ 32, 64 ]);
		/// ```
		/// </summary>
		/// <param name="v">The vector to create the `Vector2D` from</param>
		public Vector2DWrapper(object v)
		{
			try
			{
				Vector2D v1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(v, false);

				_x = v1.x;
				_y = v1.y;
				parent = null;
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		#endregion

		#region ================== Internal

		internal Vector2D AsVector2D()
		{
			return new Vector2D(_x, _y);
		}

		#endregion

		#region ================== Operators

		public static implicit operator Vector3DWrapper(Vector2DWrapper a)
		{
			return new Vector3DWrapper(a._x, a._y, 0.0);
		}

		#region ================== Addition

		public static object operator +(Vector2DWrapper lhs, object rhs)
		{
			if (rhs is double)
			{
				return new Vector2DWrapper(lhs._x + (double)rhs, lhs._y + (double)rhs);
			}
			else if (rhs.GetType().IsArray || rhs is ExpandoObject || rhs is Vector2DWrapper || rhs is Vector3DWrapper)
			{
				try
				{
					object v = BuilderPlug.Me.GetVectorFromObject(rhs, true);

					if (v is Vector2D)
						return new Vector2DWrapper(lhs._x + ((Vector2D)v).x, lhs._y + ((Vector2D)v).y);
					else
						return new Vector2DWrapper(lhs._x + ((Vector3D)v).x, lhs._y + ((Vector3D)v).y);
				}
				catch (CantConvertToVectorException e)
				{
					throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
				}
			}
			else
			{
				return lhs.ToString() + rhs.ToString();
			}
		}

		public static object operator +(object lhs, Vector2DWrapper rhs)
		{
			if (lhs is double)
			{
				return new Vector2DWrapper((double)lhs + rhs._x, (double)lhs + rhs._y);
			}
			else if (lhs.GetType().IsArray || lhs is ExpandoObject || lhs is Vector2DWrapper || lhs is Vector3DWrapper)
			{
				try
				{
					object v = BuilderPlug.Me.GetVectorFromObject(lhs, true);

					if (v is Vector2D)
						return new Vector2DWrapper(((Vector2D)v).x + rhs._x, ((Vector2D)v).y + rhs._y);
					else
						return new Vector2DWrapper(((Vector3D)v).x + rhs._x, ((Vector3D)v).y + rhs._y);
				}
				catch (CantConvertToVectorException e)
				{
					throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
				}
			}
			else
			{
				return lhs.ToString() + rhs.ToString();
			}
		}

		#endregion

		#region ================== Subtraction

		public static object operator -(Vector2DWrapper lhs, object rhs)
		{
			if (rhs is double)
				return new Vector2DWrapper(lhs._x - (double)rhs, lhs._y - (double)rhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(rhs, true);

				if (v is Vector2D)
					return new Vector2DWrapper(lhs._x - ((Vector2D)v).x, lhs._y - ((Vector2D)v).y);
				else
					return new Vector2DWrapper(lhs._x - ((Vector3D)v).x, lhs._y - ((Vector3D)v).y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static object operator -(object lhs, Vector2DWrapper rhs)
		{
			if (lhs is double)
				return new Vector2DWrapper((double)lhs - rhs._x, (double)lhs - rhs._y);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(lhs, true);

				if (v is Vector2D)
					return new Vector2DWrapper(((Vector2D)v).x - rhs._x, ((Vector2D)v).y - rhs._y);
				else
					return new Vector2DWrapper(((Vector3D)v).x - rhs._x, ((Vector3D)v).y - rhs._y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static Vector2DWrapper operator -(Vector2DWrapper a)
		{
			return new Vector2DWrapper(-a._x, -a._y);
		}

		#endregion

		#region ================== Multiply

		public static object operator *(Vector2DWrapper lhs, object rhs)
		{
			if (rhs is double)
				return new Vector2DWrapper(lhs._x * (double)rhs, lhs._y * (double)rhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(rhs, true);

				if (v is Vector2D)
					return new Vector2DWrapper(lhs._x * ((Vector2D)v).x, lhs._y * ((Vector2D)v).y);
				else
					return new Vector2DWrapper(lhs._x * ((Vector3D)v).x, lhs._y * ((Vector3D)v).y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static object operator *(object lhs, Vector2DWrapper rhs)
		{
			if (lhs is double)
				return new Vector2DWrapper((double)lhs * rhs._x, (double)lhs * rhs._y);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(lhs, true);

				if (v is Vector2D)
					return new Vector2DWrapper(((Vector2D)v).x * rhs._x, ((Vector2D)v).y * rhs._y);
				else
					return new Vector2DWrapper(((Vector3D)v).x * rhs._x, ((Vector3D)v).y * rhs._y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		#endregion

		#region ================== Divide

		public static object operator /(Vector2DWrapper lhs, object rhs)
		{
			if (rhs is double)
				return new Vector2DWrapper(lhs._x / (double)rhs, lhs._y / (double)rhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(rhs, true);

				if (v is Vector2D)
					return new Vector2DWrapper(lhs._x / ((Vector2D)v).x, lhs._y / ((Vector2D)v).y);
				else
					return new Vector2DWrapper(lhs._x / ((Vector3D)v).x, lhs._y / ((Vector3D)v).y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static object operator /(object lhs, Vector2DWrapper rhs)
		{
			if (lhs is double)
				return new Vector2DWrapper(rhs._x / (double)lhs, rhs._y / (double)lhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(lhs, true);

				if (v is Vector2D)
					return new Vector2DWrapper(((Vector2D)v).x / rhs._x, ((Vector2D)v).y / rhs._y);
				else
					return new Vector2DWrapper(((Vector3D)v).x / rhs._x, ((Vector3D)v).y / rhs._y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		#endregion

		#region ================== Equality

		public static bool operator ==(Vector2DWrapper lhs, object rhs)
		{
			try
			{
				Vector2D v1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(rhs, false);
				return (lhs._x == v1.x) && (lhs._y == v1.y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static bool operator ==(object lhs, Vector2DWrapper rhs)
		{
			try
			{
				Vector2D v1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(lhs, false);
				return (v1.x == rhs._x) && (v1.y == rhs._y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static bool operator !=(Vector2DWrapper lhs, object rhs)
		{
			try
			{
				Vector2D v1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(rhs, false);
				return (lhs._x != v1.x) || (lhs._y != v1.y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static bool operator !=(object lhs, Vector2DWrapper rhs)
		{
			try
			{
				Vector2D v1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(lhs, false);
				return (v1.x != rhs._x) || (v1.y != rhs._y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		#endregion

		#endregion

		#region ================== Statics

		/// <summary>
		/// Returns the dot product of two `Vector2D`s.
		/// </summary>
		/// <param name="a">First `Vector2D`</param>
		/// <param name="b">Second `Vector2D`</param>
		/// <returns>The dot product of the two vectors</returns>
		public static double dotProduct(Vector2DWrapper a, Vector2DWrapper b)
		{
			// Calculate and return the dot product
			return a._x * b._x + a._y * b._y;
		}

		/// <summary>
		/// Returns the cross product of two `Vector2D`s.
		/// </summary>
		/// <param name="a">First `Vector2D`</param>
		/// <param name="b">Second `Vector2D`</param>
		/// <returns>Cross product of the two vectors as `Vector2D`</returns>
		public static Vector2DWrapper crossProduct(object a, object b)
		{
			try
			{
				Vector2D a1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(a, false);
				Vector2D b1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(b, false);

				return new Vector2DWrapper(a1.y * b1.x, a1.x * b1.y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		/// <summary>
		/// Reflects a `Vector2D` over a mirror `Vector2D`.
		/// </summary>
		/// <param name="v">`Vector2D` to reflect</param>
		/// <param name="m">Mirror `Vector2D`</param>
		/// <returns>The reflected vector as `Vector2D`</returns>
		public static Vector2DWrapper reflect(object v, object m)
		{
			try
			{
				Vector2D v1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(v, false);
				Vector2D m1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(m, false);

				Vector2D mv = Vector2D.Reflect(v1, m1);

				return new Vector2DWrapper(mv.x, mv.y);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		/// <summary>
		/// Returns a reversed `Vector2D`.
		/// </summary>
		/// <param name="v">`Vector2D` to reverse</param>
		/// <returns>The reversed vector as `Vector2D`</returns>
		public static Vector2DWrapper reversed(object v)
		{
			try
			{
				Vector2D v1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(v, false);

				return new Vector2DWrapper(Vector2D.Reversed(v1));
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		/// <summary>
		/// Creates a `Vector2D` from an angle in radians,
		/// </summary>
		/// <param name="angle">Angle in radians</param>
		/// <returns>Vector as `Vector2D`</returns>
		public static Vector2DWrapper fromAngleRad(double angle)
		{
			return new Vector2DWrapper(Vector2D.FromAngle(angle));
		}

		/// <summary>
		/// Creates a `Vector2D` from an angle in degrees,
		/// </summary>
		/// <param name="angle">Angle in degrees</param>
		/// <returns>Vector as `Vector2D`</returns>
		public static Vector2DWrapper fromAngle(double angle)
		{
			return new Vector2DWrapper(Vector2D.FromAngle(Angle2D.DegToRad(angle)));
		}

		/// <summary>
		/// Returns the angle between two `Vector2D`s in radians
		/// </summary>
		/// <param name="a">First `Vector2D`</param>
		/// <param name="b">Second `Vector2D`</param>
		/// <returns>Angle in radians</returns>
		public static double getAngleRad(object a, object b)
		{
			try
			{
				Vector2D a1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(a, false);
				Vector2D b1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(b, false);

				return Vector2D.GetAngle(a1, b1);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		/// <summary>
		/// Returns the angle between two `Vector2D`s in degrees.
		/// </summary>
		/// <param name="a">First `Vector2D`</param>
		/// <param name="b">Second `Vector2D`</param>
		/// <returns>Angle in degrees</returns>
		public static double getAngle(object a, object b)
		{
			try
			{
				Vector2D a1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(a, false);
				Vector2D b1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(b, false);

				return Angle2D.RadToDeg(Vector2D.GetAngle(a1, b1));
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		/// <summary>
		/// Returns the square distance between two `Vector2D`s.
		/// </summary>
		/// <param name="a">First `Vector2D`</param>
		/// <param name="b">Second `Vector2D`</param>
		/// <returns>The squared distance</returns>
		public static double getDistanceSq(object a, object b)
		{
			try
			{
				Vector2D a1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(a, false);
				Vector2D b1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(b, false);

				return Vector2D.DistanceSq(a1, b1);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		/// <summary>
		/// Returns the distance between two `Vector2D`s.
		/// </summary>
		/// <param name="a">First `Vector2D`</param>
		/// <param name="b">Second `Vector2D`</param>
		/// <returns>The distance</returns>
		public static double getDistance(object a, object b)
		{
			try
			{
				Vector2D a1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(a, false);
				Vector2D b1 = (Vector2D)BuilderPlug.Me.GetVectorFromObject(b, false);

				return Vector2D.Distance(a1, b1);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		#endregion

		#region ================== Methods

		/// <summary>
		/// Returns the perpendicular to the `Vector2D`.
		/// </summary>
		/// <returns>The perpendicular as `Vector2D`</returns>
		public Vector2DWrapper getPerpendicular()
		{
			return new Vector2DWrapper(-_y, _x);
		}

		/// <summary>
		/// Returns a `Vector2D` with the sign of all components.
		/// </summary>
		/// <returns>A `Vector2D` with the sign of all components</returns>
		public Vector2DWrapper getSign()
		{
			return new Vector2DWrapper(new Vector2D(_x, _y).GetSign());
		}

		/// <summary>
		/// Returns the angle of the `Vector2D` in radians.
		/// </summary>
		/// <returns>The angle of the `Vector2D` in radians</returns>
		public double getAngleRad()
		{
			return new Vector2D(_x, _y).GetAngle();
		}

		/// <summary>
		/// Returns the angle of the `Vector2D` in degree.
		/// </summary>
		/// <returns>The angle of the `Vector2D` in degree</returns>
		public double getAngle()
		{
			return Angle2D.RadToDeg(new Vector2D(_x, _y).GetAngle());
		}

		/// <summary>
		/// Returns the length of the `Vector2D`.
		/// </summary>
		/// <returns>The length of the `Vector2D`</returns>
		public double getLength()
		{
			return new Vector2D(_x, _y).GetLength();
		}

		/// <summary>
		/// Returns the square length of the `Vector2D`.
		/// </summary>
		/// <returns>The square length of the `Vector2D`</returns>
		public double getLengthSq()
		{
			return new Vector2D(_x, _y).GetLengthSq();
		}

		/// <summary>
		/// Returns the normal of the `Vector2D`.
		/// </summary>
		/// <returns>The normal as `Vector2D`</returns>
		public Vector2DWrapper getNormal()
		{
			return new Vector2DWrapper(new Vector2D(_x, _y).GetNormal());
		}

		/// <summary>
		/// Returns the transformed vector as `Vector2D`.
		/// </summary>
		/// <param name="offsetx">X offset</param>
		/// <param name="offsety">Y offset</param>
		/// <param name="scalex">X scale</param>
		/// <param name="scaley">Y scale</param>
		/// <returns>The transformed vector as `Vector2D`</returns>
		public Vector2DWrapper getTransformed(double offsetx, double offsety, double scalex, double scaley)
		{
			return new Vector2DWrapper(new Vector2D(_x, _y).GetTransformed(offsetx, offsety, scalex, scaley));
		}

		/// <summary>
		/// Returns the inverse transformed vector as `Vector2D`.
		/// </summary>
		/// <param name="invoffsetx">X offset</param>
		/// <param name="invoffsety">Y offset</param>
		/// <param name="invscalex">X scale</param>
		/// <param name="invscaley">Y scale</param>
		/// <returns>The inverse transformed vector as `Vector2D`</returns>
		public Vector2DWrapper getInverseTransformed(double invoffsetx, double invoffsety, double invscalex, double invscaley)
		{
			return new Vector2DWrapper(new Vector2D(_x, _y).GetInvTransformed(invoffsetx, invoffsety, invscalex, invscaley));
		}

		/// <summary>
		/// Returns the rotated vector as `Vector2D`.
		/// </summary>
		/// <param name="theta">Angle in degree to rotate by</param>
		/// <returns>The rotated `Vector2D`</returns>
		public Vector2DWrapper getRotated(double theta)
		{
			return new Vector2DWrapper(new Vector2D(_x, _y).GetRotated(Angle2D.DegToRad(theta)));
		}

		/// <summary>
		/// Returns the rotated vector as `Vector2D`.
		/// </summary>
		/// <param name="theta">Angle in radians to rotate by</param>
		/// <returns>The rotated `Vector2D`</returns>
		public Vector2DWrapper getRotatedRad(double theta)
		{
			return new Vector2DWrapper(new Vector2D(_x, _y).GetRotated(theta));
		}

		/// <summary>
		/// Checks if the `Vector2D` is finite or not.
		/// </summary>
		/// <returns>`true` if `Vector2D` is finite, otherwise `false`</returns>
		public bool isFinite()
		{
			return new Vector2D(_x, _y).IsFinite();
		}

		public override string ToString()
		{
			return new Vector2D(_x, _y).ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Vector2DWrapper)) return false;

			Vector2DWrapper other = (Vector2DWrapper)obj;

			if (_x != other._x) return false;
			if (_y != other._y) return false;
			return true;
		}

		#endregion
	}
}
