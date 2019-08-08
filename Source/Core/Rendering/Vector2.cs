using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeImp.DoomBuilder.Rendering
{
    public struct Vector2
    {
        public Vector2(float v)
        {
            X = v;
            Y = v;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X;
        public float Y;

        public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount)
        {
            float squared = amount * amount;
            float cubed = amount * squared;
            float part1 = ((2.0f * cubed) - (3.0f * squared)) + 1.0f;
            float part2 = (-2.0f * cubed) + (3.0f * squared);
            float part3 = (cubed - (2.0f * squared)) + amount;
            float part4 = cubed - squared;
            return new Vector2(
                (((value1.X * part1) + (value2.X * part2)) + (tangent1.X * part3)) + (tangent2.X * part4),
                (((value1.Y * part1) + (value2.Y * part2)) + (tangent1.Y * part3)) + (tangent2.Y * part4));
        }

        public override bool Equals(object o)
        {
            if (o is Vector2)
            {
                Vector2 v = (Vector2)o;
                return this == v;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.X - right.X, left.Y - right.Y);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return left.X != right.X || left.Y != right.Y;
        }
    }
}
