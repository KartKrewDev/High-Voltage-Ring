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
	internal struct Vector3DWrapper
	{
		#region ================== Variables

		public double _x;
		public double _y;
		public double _z;
		private MapElement parent;

		#endregion

		#region ================== Constructors

		internal Vector3DWrapper(Vector3D v, MapElement parent = null)
		{
			_x = v.x;
			_y = v.y;
			_z = v.z;
			this.parent = parent;
		}

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
					((Thing)parent).Move(new Vector3D(_x, _y, _z));
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
					((Thing)parent).Move(new Vector3D(_x, _y, _z));
			}
		}

		/// <summary>
		/// The `z` value of the vector.
		/// </summary>
		public double z
		{
			get
			{
				return _z;
			}
			set
			{
				_z = value;

				if (parent is Vertex)
					((Vertex)parent).Move(new Vector2D(_x, _y));
				else if (parent is Thing)
					((Thing)parent).Move(new Vector3D(_x, _y, _z));
			}
		}

		#endregion

		/// <summary>
		/// Creates a new `Vector3D` from x and y coordinates
		/// ```
		/// let v = new UDB.Vector3D(32, 64, 128);
		/// ```
		/// </summary>
		/// <param name="x">The x coordinate</param>
		/// <param name="y">The y coordinate</param>
		/// <param name="z">The z coordinate</param>
		public Vector3DWrapper(double x, double y, double z)
		{
			this._x = x;
			this._y = y;
			this._z = z;
			parent = null;
		}

		/// <summary>
		/// Creates a new `Vector3D` from a point.
		/// ```
		/// let v = new UDB.Vector3D([ 32, 64, 128 ]);
		/// ```
		/// </summary>
		/// <param name="v">The vector to create the `Vector3D` from</param>
		public Vector3DWrapper(object v)
		{
			try
			{
				Vector3D v1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(v, true);

				_x = v1.x;
				_y = v1.y;
				_z = v1.z;
				this.parent = null;
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		#endregion

		#region ================== Internal

		internal Vector3D AsVector3D()
		{
			return new Vector3D(_x, _y, _z);
		}

		#endregion

		#region ================== Operators

		public static implicit operator Vector2DWrapper(Vector3DWrapper a)
		{
			return new Vector2DWrapper(a._x, a._y);
		}

		#region ================== Addition

		public static object operator +(Vector3DWrapper lhs, object rhs)
		{
			if(rhs is double)
			{
				return new Vector3DWrapper(lhs._x + (double)rhs, lhs._y + (double)rhs, lhs._z + (double)rhs);
			}
			else if(rhs.GetType().IsArray || rhs is ExpandoObject || rhs is Vector2DWrapper || rhs is Vector3DWrapper)
			{
				try
				{
					object v = BuilderPlug.Me.GetVectorFromObject(rhs, true);

					if(v is Vector2D)
						return new Vector3DWrapper(lhs._x + ((Vector2D)v).x, lhs._y + ((Vector2D)v).y, lhs._z);
					else
						return new Vector3DWrapper(lhs._x + ((Vector3D)v).x, lhs._y + ((Vector3D)v).y, lhs._z + ((Vector3D)v).z);
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

		public static object operator +(object lhs, Vector3DWrapper rhs)
		{
			if(lhs is double)
			{
				return new Vector3DWrapper((double)lhs + rhs._x, (double)lhs + rhs._y, (double)lhs + rhs._z);
			}
			else if (lhs.GetType().IsArray || lhs is ExpandoObject || lhs is Vector2DWrapper || lhs is Vector3DWrapper)
			{
				try
				{
					object v = BuilderPlug.Me.GetVectorFromObject(lhs, true);

					if (v is Vector2D)
						return new Vector3DWrapper(((Vector2D)v).x + rhs._x, ((Vector2D)v).y + rhs._y, rhs._z);
					else
						return new Vector3DWrapper(((Vector3D)v).x + rhs._x, ((Vector3D)v).y + rhs._y, ((Vector3D)v).z + rhs._z);
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

		public static object operator -(Vector3DWrapper lhs, object rhs)
		{
			if (rhs is double)
				return new Vector3DWrapper(lhs._x - (double)rhs, lhs._y - (double)rhs, lhs._z -(double)rhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(rhs, true);

				if (v is Vector2D)
					return new Vector3DWrapper(lhs._x - ((Vector2D)v).x, lhs._y - ((Vector2D)v).y, lhs._z);
				else
					return new Vector3DWrapper(lhs._x - ((Vector3D)v).x, lhs._y - ((Vector3D)v).y, lhs._z - ((Vector3D)v).z);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static object operator -(object lhs, Vector3DWrapper rhs)
		{
			if (lhs is double)
				return new Vector3DWrapper(rhs._x- (double)lhs, rhs._y - (double)lhs, rhs._z - (double)lhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(lhs, true);

				if (v is Vector2D)
					return new Vector3DWrapper(((Vector2D)v).x - rhs._x, ((Vector2D)v).y - rhs._y, -rhs._z);
				else
					return new Vector3DWrapper(((Vector3D)v).x - rhs._x, ((Vector3D)v).y - rhs._y, ((Vector3D)v).z - rhs._z);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		#endregion

		#region ================== Multiplication

		public static object operator *(Vector3DWrapper lhs, object rhs)
		{
			if (rhs is double)
				return new Vector3DWrapper(lhs._x * (double)rhs, lhs._y * (double)rhs, lhs._z * (double)rhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(rhs, true);

				if (v is Vector2D)
					return new Vector3DWrapper(lhs._x * ((Vector2D)v).x, lhs._y * ((Vector2D)v).y, 0);
				else
					return new Vector3DWrapper(lhs._x * ((Vector3D)v).x, lhs._y * ((Vector3D)v).y, lhs._z * ((Vector3D)v).z);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static object operator *(object lhs, Vector3DWrapper rhs)
		{
			if (lhs is double)
				return new Vector3DWrapper(rhs._x * (double)lhs, rhs._y * (double)lhs, rhs._z * (double)lhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(lhs, true);

				if (v is Vector2D)
					return new Vector3DWrapper(((Vector2D)v).x * rhs._x, ((Vector2D)v).y * rhs._y, 0);
				else
					return new Vector3DWrapper(((Vector3D)v).x * rhs._x, ((Vector3D)v).y * rhs._y, ((Vector3D)v).z * rhs._z);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		#endregion

		#region ================== Division

		public static object operator /(Vector3DWrapper lhs, object rhs)
		{
			if (rhs is double)
				return new Vector3DWrapper(lhs._x / (double)rhs, lhs._y / (double)rhs, lhs._z / (double)rhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(rhs, true);

				if (v is Vector2D)
					return new Vector3DWrapper(lhs._x / ((Vector2D)v).x, lhs._y / ((Vector2D)v).y, lhs._z / 0);
				else
					return new Vector3DWrapper(lhs._x / ((Vector3D)v).x, lhs._y / ((Vector3D)v).y, lhs._z / ((Vector3D)v).z);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static object operator /(object lhs, Vector3DWrapper rhs)
		{
			if (lhs is double)
				return new Vector3DWrapper(rhs._x / (double)lhs, rhs._y / (double)lhs, rhs._z / (double)lhs);

			try
			{
				object v = BuilderPlug.Me.GetVectorFromObject(lhs, true);

				if (v is Vector2D)
					return new Vector3DWrapper(((Vector2D)v).x / rhs._x, ((Vector2D)v).y / rhs._y, 0 / rhs._z);
				else
					return new Vector3DWrapper(((Vector3D)v).x / rhs._x, ((Vector3D)v).y / rhs._y, ((Vector3D)v).z / rhs._z);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		#endregion

		#region ================== Equality

		public static bool operator ==(Vector3DWrapper lhs, object rhs)
		{
			try
			{
				Vector3D v1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(rhs, true);
				return (lhs._x == v1.x) && (lhs._y == v1.y) && (lhs._z == v1.z);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static bool operator ==(object lhs, Vector3DWrapper rhs)
		{
			try
			{
				Vector3D v1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(lhs, true);
				return (v1.x == rhs._x) && (v1.y == rhs._y) && (v1.z == rhs._z);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static bool operator !=(Vector3DWrapper lhs, object rhs)
		{
			try
			{
				Vector3D v1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(rhs, true);
				return (lhs._x != v1.x) || (lhs._y != v1.y) || (lhs._z != v1.z);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		public static bool operator !=(object lhs, Vector3DWrapper rhs)
		{
			try
			{
				Vector3D v1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(lhs, true);
				return (v1.x != rhs._x) || (v1.y != rhs._y) || (v1.z != rhs._z);
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
		/// Returns the dot product of two `Vector3D`s.
		/// </summary>
		/// <param name="a">First `Vector3D`</param>
		/// <param name="b">Second `Vector3D`</param>
		/// <returns>The dot product of the two vectors</returns>
		public static double dotProduct(Vector3DWrapper a, Vector3DWrapper b)
		{
			// Calculate and return the dot product
			return a._x * b._x + a._y * b._y + a._z * b._z;
		}

		/// <summary>
		/// Returns the cross product of two `Vector3D`s.
		/// </summary>
		/// <param name="a">First `Vector3D`</param>
		/// <param name="b">Second `Vector3D`</param>
		/// <returns>Cross product of the two vectors as `Vector3D`</returns>
		public static Vector3DWrapper crossProduct(object a, object b)
		{
			try
			{
				Vector3D a1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(a, true);
				Vector3D b1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(b, true);

				return new Vector3DWrapper(a1.y * b1.x - a1.z * b1.y, a1.z * b1.x - a1.x * b1.z, a1.x * b1.y - a1.y * b1.x);
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		/// <summary>
		/// Reflects a `Vector3D` over a mirror `Vector3D`.
		/// </summary>
		/// <param name="v">`Vector3D` to reflect</param>
		/// <param name="m">Mirror `Vector3D`</param>
		/// <returns>The reflected vector as `Vector3D`</returns>
		public static Vector3DWrapper reflect(object v, object m)
		{
			try
			{
				Vector3D v1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(v, true);
				Vector3D m1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(m, true);

				return new Vector3DWrapper(Vector3D.Reflect(v1, m1));
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		/// <summary>
		/// Returns a reversed `Vector3D`.
		/// </summary>
		/// <param name="v">`Vector3D` to reverse</param>
		/// <returns>The reversed vector as `Vector3D`</returns>
		public static Vector3DWrapper reversed(object v)
		{
			try
			{
				Vector3D v1 = (Vector3D)BuilderPlug.Me.GetVectorFromObject(v, true);

				return new Vector3DWrapper(Vector3D.Reversed(v1));
			}
			catch (CantConvertToVectorException e)
			{
				throw BuilderPlug.Me.ScriptRunner.CreateRuntimeException(e.Message);
			}
		}

		/// <summary>
		/// Creates a `Vector3D` from an angle in radians
		/// </summary>
		/// <param name="angle">Angle on the x/y axes in radians</param>
		/// <returns>Vector as `Vector3D`</returns>
		public static Vector3DWrapper fromAngleXYRad(double angle)
		{
			return new Vector3DWrapper(Vector3D.FromAngleXY(angle));
		}

		/// <summary>
		/// Creates a `Vector3D` from an angle in radians,
		/// </summary>
		/// <param name="angle">Angle on the x/y axes in degrees</param>
		/// <returns>Vector as `Vector3D`</returns>
		public static Vector3DWrapper fromAngleXY(double angle)
		{
			return new Vector3DWrapper(Vector3D.FromAngleXY(Angle2D.DegToRad(angle)));
		}

		/// <summary>
		/// Creates a `Vector3D` from two angles in radians
		/// </summary>
		/// <param name="anglexy">Angle on the x/y axes in radians</param>
		/// <param name="anglez">Angle on the z axis in radians</param>
		/// <returns>Vector as `Vector3D`</returns>
		public static Vector3DWrapper fromAngleXYZRad(double anglexy, double anglez)
		{
			return new Vector3DWrapper(Vector3D.FromAngleXYZ(anglexy, anglez));
		}

		/// <summary>
		/// Creates a `Vector3D` from two angles in degrees
		/// </summary>
		/// <param name="anglexy">Angle on the x/y axes in radians</param>
		/// <param name="anglez">Angle on the z axis in radians</param>
		/// <returns>Vector as `Vector3D`</returns>
		public static Vector3DWrapper fromAngleXYZ(double anglexy, double anglez)
		{
			return new Vector3DWrapper(Vector3D.FromAngleXYZ(Angle2D.DegToRad(anglexy), Angle2D.DegToRad(anglez)));
		}

		#endregion

		#region ================== Methods

		/// <summary>
		/// Returns the x/y angle of the `Vector3D` in radians.
		/// </summary>
		/// <returns>The x/y angle of the `Vector3D` in radians</returns>
		public double getAngleXYRad()
		{
			return new Vector3D(_x, _y, _z).GetAngleXY();
		}

		/// <summary>
		/// Returns the angle of the `Vector3D` in degrees.
		/// </summary>
		/// <returns>The angle of the `Vector3D` in degrees</returns>
		public double getAngleXY()
		{
			return Angle2D.RadToDeg(new Vector3D(_x, _y, _z).GetAngleXY());
		}

		/// <summary>
		/// Returns the z angle of the `Vector3D` in radians.
		/// </summary>
		/// <returns>The z angle of the `Vector3D` in radians</returns>
		public double getAngleZRad()
		{
			return new Vector3D(_x, _y, _z).GetAngleZ();
		}

		/// <summary>
		/// Returns the z angle of the `Vector3D` in degrees.
		/// </summary>
		/// <returns>The z angle of the `Vector3D` in degrees</returns>
		public double getAngleZ()
		{
			return Angle2D.RadToDeg(new Vector3D(_x, _y, _z).GetAngleZ());
		}

		/// <summary>
		/// Returns the length of the `Vector3D`.
		/// </summary>
		/// <returns>The length of the `Vector3D`</returns>
		public double getLength()
		{
			return new Vector3D(_x, _y, _z).GetLength();
		}

		/// <summary>
		/// Returns the square length of the `Vector3D`.
		/// </summary>
		/// <returns>The square length of the `Vector3D`</returns>
		public double getLengthSq()
		{
			return new Vector3D(_x, _y, _z).GetLengthSq();
		}

		/// <summary>
		/// Returns the normal of the `Vector3D`.
		/// </summary>
		/// <returns>The normal as `Vector3D`</returns>
		public Vector3DWrapper getNormal()
		{
			return new Vector3DWrapper(new Vector3D(_x, _y, _z).GetNormal());
		}

		/// <summary>
		/// Return the scaled `Vector3D`.
		/// </summary>
		/// <param name="scale">Scale, where 1.0 is unscaled</param>
		/// <returns>The scaled `Vector3D`</returns>
		public Vector3DWrapper getScaled(double scale)
		{
			return new Vector3DWrapper(new Vector3D(_x, _y, _z).GetScaled(scale));
		}

		/// <summary>
		/// Checks if the `Vector3D` is normalized or not.
		/// </summary>
		/// <returns>`true` if `Vector3D` is normalized, otherwise `false`</returns>
		public bool isNormalized()
		{
			return new Vector3D(_x, _y, _z).IsNormalized();
		}

		/// <summary>
		/// Checks if the `Vector3D` is finite or not.
		/// </summary>
		/// <returns>`true` if `Vector3D` is finite, otherwise `false`</returns>
		public bool isFinite()
		{
			return new Vector3D(_x, _y, _z).IsFinite();
		}

		public override string ToString()
		{
			return new Vector3D(_x, _y, _z).ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Vector3DWrapper)) return false;

			Vector3DWrapper other = (Vector3DWrapper)obj;

			if (_x != other._x) return false;
			if (_y != other._y) return false;
			if (_z != other._z) return false;
			return true;
		}

		#endregion
	}
}