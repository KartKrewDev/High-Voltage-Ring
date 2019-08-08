using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeImp.DoomBuilder.Rendering
{
    public struct Vector4
    {
        public Vector4(float v)
        {
            X = v;
            Y = v;
            Z = v;
            W = v;
        }

        public Vector4(Vector2 xy, float z, float w)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
            W = w;
        }

        public Vector4(Vector3 xyz, float w)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
        }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float X;
        public float Y;
        public float Z;
        public float W;

        public override bool Equals(object o)
        {
            if (o is Vector4)
            {
                Vector4 v = (Vector4)o;
                return this == v;
            }
            else
            {
                return false;
            }
        }

        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }

        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }

        public static Vector4 operator -(Vector4 v)
        {
            return new Vector4(-v.X, -v.Y, -v.Z, -v.W);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }

        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.W == right.W;
        }

        public static bool operator !=(Vector4 left, Vector4 right)
        {
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z || left.W != right.W;
        }
    }
}
