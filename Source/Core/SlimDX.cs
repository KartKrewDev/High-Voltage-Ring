using System;

namespace SlimDX
{
    #region High level mesh rendering
    namespace Direct3D9
    {
        public class Mesh
        {
            public Mesh(int indexCount, int vertexCount, MeshFlags flags, VertexElement[] elements) { }

            public VertexBuffer VertexBuffer { get; private set; }
            public IndexBuffer IndexBuffer { get; private set; }

            public DataStream LockVertexBuffer(LockFlags flags) { return null; }
            public DataStream LockIndexBuffer(LockFlags flags) { return null; }
            public void UnlockVertexBuffer() { }
            public void UnlockIndexBuffer() { }

            public void DrawSubset(int index) { }

            public void Dispose() { }
        }

        public class Effect
        {
            public static Effect FromStream(System.IO.Stream stream, ShaderFlags flags, out string errors) { errors = ""; return null; }

            public void SetTexture(EffectHandle handle, BaseTexture texture) { }
            public void SetValue<T>(EffectHandle handle, T value) where T : struct { }
            public EffectHandle GetParameter(EffectHandle parameter, string name) { return null; }
            public string Technique { set; private get; }
            public void CommitChanges() { }

            public void Begin() { }
            public void BeginPass(int index) { }
            public void EndPass() { }
            public void End() { }

            public void Dispose() { }
        }

        public class EffectHandle
        {
            public void Dispose() { }
        }
    }
    #endregion

    #region Vertex buffer format / Input assembly
    namespace Direct3D9
    {
        public class VertexDeclaration
        {
            public VertexDeclaration(VertexElement[] elements) { }
            public void Dispose() { }
        }

        public struct VertexElement
        {
            public VertexElement(short stream, short offset, DeclarationType type, DeclarationMethod method, DeclarationUsage usage, byte usageIndex) { }
            public static readonly VertexElement VertexDeclarationEnd;
        }
    }
    #endregion

    #region Buffer objects
    namespace Direct3D9
    {
        public class VertexBuffer
        {
            public VertexBuffer(int sizeInBytes, Usage usage, VertexFormat format, Pool pool) { }

            public DataStream Lock(int offset, int size, LockFlags flags) { return null; }
            public void Unlock() { }

            public object Tag { get; set; }

            public bool Disposed { get; private set; }
            public void Dispose() { Disposed = true; }
        }

        public class IndexBuffer
        {
            public DataStream Lock(int offset, int size, LockFlags flags) { return null; }
            public void Unlock() { }

            public object Tag { get; set; }

            public bool Disposed { get; private set; }
            public void Dispose() { Disposed = true; }
        }
    }
    #endregion

    #region Images (textures and surfaces)
    namespace Direct3D9
    {
        public class BaseTexture
        {
            public bool Disposed { get; }
            public void Dispose() { }
        }

        public class Texture : BaseTexture
        {
            public Texture(int width, int height, int levels, Usage usage, Format format, Pool pool) { }

            public int Width { get; private set; }
            public int Height { get; private set; }

            public object Tag { get; set; }

            public DataRectangle LockRectangle(int level, LockFlags flags) { return null; }
            public void UnlockRectangle(int level) { }

            public static Texture FromStream(System.IO.Stream stream) { return null; }
            public static Texture FromStream(System.IO.Stream stream, int length, int width, int height, int levels, Usage usage, Format format, Pool pool) { return null; }
        }

        public class CubeTexture : BaseTexture
        {
            public CubeTexture(int size, int levels, Usage usage, Format format, Pool pool) { }

            public DataRectangle LockRectangle(CubeMapFace face, int level, LockFlags flags) { return null; }
            public void UnlockRectangle(CubeMapFace face, int level) { }
        }
    }
    #endregion

    #region Locked buffer writing and reading
    public class DataRectangle
    {
        public DataRectangle(int pitch, DataStream s) { Data = s; Pitch = pitch; }
        public DataStream Data { get; private set; }
        public int Pitch { get; private set; }
    }

    public class DataStream : IDisposable
    {
        public void Seek(long offset, System.IO.SeekOrigin origin) { }
        public void Write(ushort v) { }
        public void Write(Array data, long offset, long size) { }
        public void WriteRange(Array data) { }
        public void WriteRange(Array data, long offset, long size) { }
        public void WriteRange(IntPtr data, long size) { }
        public void Dispose() { }

        public void ReadRange(Array data, long offset, long size) { }

        public bool CanRead { get; private set; }
        public bool CanWrite { get; private set; }
        public long Length { get; private set; }
        public IntPtr DataPointer { get; private set; }
    }
    #endregion

    #region Enumerations
    namespace Direct3D9
    {
        public enum RenderState
        {
            AlphaBlendEnable,
            AlphaFunc,
            AlphaRef,
            AlphaTestEnable,
            CullMode,
            BlendOperation,
            SourceBlend,
            DestinationBlend,
            FillMode,
            FogEnable,
            FogColor,
            FogStart,
            FogEnd,
            MultisampleAntialias,
            TextureFactor,
            ZEnable,
            ZWriteEnable
        }

        public enum Compare { GreaterEqual }
        public enum Cull { None, Counterclockwise }
        public enum Blend { InverseSourceAlpha, SourceAlpha, One, BlendFactor }
        public enum BlendOperation { Add, ReverseSubtract }
        public enum FillMode { Solid, Wireframe }
        public enum TransformState { World, View, Projection }
        public enum SamplerState { AddressU, AddressV, AddressW }
        public enum TextureAddress { Wrap, Clamp }
        public enum ClearFlags { Target, ZBuffer }
        public enum Format { Unknown, A8R8G8B8 }
        public enum Usage { None, WriteOnly, Dynamic, RenderTarget }
        public enum VertexFormat { None }
        public enum Pool { Default, Managed, SystemMemory }
        public enum LockFlags { None, Discard }
        public enum MeshFlags { Use32Bit, IndexBufferManaged, VertexBufferManaged, Managed }
        public enum ShaderFlags { None, Debug }
        public enum PrimitiveType { LineList, TriangleList, TriangleStrip }
        public enum CubeMapFace { PositiveX, PositiveY, PositiveZ, NegativeX, NegativeY, NegativeZ }
        public enum TextureFilter { None, Point, Linear, Anisotropic }
        public enum DeclarationType { Float2, Float3, Color }
        public enum DeclarationMethod { Default }
        public enum DeclarationUsage { Position, Color, TextureCoordinate, Normal }
    }
    #endregion

    #region Matrix, vector, color
    public struct Matrix
    {
        public float M11, M12, M13, M14;
        public float M21, M22, M23, M24;
        public float M31, M32, M33, M34;
        public float M41, M42, M43, M44;

        public static Matrix Null { get; }
        public static Matrix Identity { get { Matrix m = Null; m.M11 = 1.0f; m.M22 = 1.0f; m.M33 = 1.0f; m.M44 = 1.0f; return m; } }
        public static Matrix Translation(Vector3 v) { Matrix m = Null; m.M11 = v.X; m.M22 = v.Y; m.M33 = v.Z; m.M44 = 1.0f; return m; }
        public static Matrix Translation(float x, float y, float z) { Matrix m = Null; m.M11 = x; m.M22 = y; m.M33 = z; m.M44 = 1.0f; return m; }
        public static Matrix RotationX(float angle) { throw new NotImplementedException(); }
        public static Matrix RotationY(float angle) { throw new NotImplementedException(); }
        public static Matrix RotationZ(float angle) { throw new NotImplementedException(); }
        public static Matrix Scaling(float x, float y, float z) { throw new NotImplementedException(); }
        public static Matrix Scaling(Vector3 v) { throw new NotImplementedException(); }
        public static Matrix LookAtLH(Vector3 eye, Vector3 target, Vector3 up) { throw new NotImplementedException(); }
        public static Matrix LookAtRH(Vector3 eye, Vector3 target, Vector3 up) { throw new NotImplementedException(); }
        public static Matrix PerspectiveFovLH(float fov, float aspect, float znear, float zfar) { throw new NotImplementedException(); }
        public static Matrix PerspectiveFovRH(float fov, float aspect, float znear, float zfar) { throw new NotImplementedException(); }
        public static Matrix Multiply(Matrix a, Matrix b) { throw new NotImplementedException(); }

        public static Matrix operator *(Matrix a, Matrix b) { throw new NotImplementedException(); }

        public override bool Equals(object o)
        {
            if (o is Matrix)
            {
                Matrix v = (Matrix)o;
                return this == v;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return
                M11.GetHashCode() + M12.GetHashCode() + M13.GetHashCode() + M14.GetHashCode() +
                M21.GetHashCode() + M22.GetHashCode() + M23.GetHashCode() + M24.GetHashCode() +
                M31.GetHashCode() + M32.GetHashCode() + M33.GetHashCode() + M34.GetHashCode() +
                M41.GetHashCode() + M42.GetHashCode() + M43.GetHashCode() + M44.GetHashCode();
        }

        public static bool operator ==(Matrix left, Matrix right)
        {
            return
                left.M11 == right.M11 && left.M12 == right.M12 && left.M13 == right.M13 && left.M14 == right.M14 &&
                left.M21 == right.M21 && left.M22 == right.M22 && left.M23 == right.M23 && left.M24 == right.M24 &&
                left.M31 == right.M31 && left.M32 == right.M32 && left.M33 == right.M33 && left.M34 == right.M34 &&
                left.M41 == right.M41 && left.M42 == right.M42 && left.M43 == right.M43 && left.M44 == right.M44;
        }

        public static bool operator !=(Matrix left, Matrix right)
        {
            return
                left.M11 != right.M11 || left.M12 != right.M12 || left.M13 != right.M13 || left.M14 != right.M14 ||
                left.M21 != right.M21 || left.M22 != right.M22 || left.M23 != right.M23 || left.M24 != right.M24 ||
                left.M31 != right.M31 || left.M32 != right.M32 || left.M33 != right.M33 || left.M34 != right.M34 ||
                left.M41 != right.M41 || left.M42 != right.M42 || left.M43 != right.M43 || left.M44 != right.M44;
        }
    }

    public struct Color3
    {
        public Color3(float r, float g, float b) { Red = r; Green = g; Blue = b; }
        public Color3(Vector3 c) { Red = c.X; Green = c.Y; Blue = c.Z; }
        public Color3(System.Drawing.Color c) { Red = c.R / 255.0f; Green = c.G / 255.0f; Blue = c.B / 255.0f; }
        public float Red, Green, Blue;

        public override bool Equals(object o)
        {
            if (o is Color3)
            {
                Color3 v = (Color3)o;
                return this == v;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode() { return Red.GetHashCode() + Green.GetHashCode() + Blue.GetHashCode(); }
        public static bool operator ==(Color3 left, Color3 right) { return left.Red == right.Red && left.Green == right.Green && left.Blue == right.Blue; }
        public static bool operator !=(Color3 left, Color3 right) { return left.Red != right.Red || left.Green != right.Green || left.Blue != right.Blue; }
    }

    public struct Color4
    {
        public Color4(int argb)
        {
            uint v = (uint)argb;
            Alpha = ((v >> 24) & 0xff) / 255.0f;
            Red = ((v >> 16) & 0xff) / 255.0f;
            Green = ((v >> 8) & 0xff) / 255.0f;
            Blue = (v & 0xff) / 255.0f;
        }

        public Color4(float r, float g, float b, float a) { Red = r; Green = g; Blue = b; Alpha = a; }
        public Color4(Vector4 c) { Red = c.X; Green = c.Y; Blue = c.Z; Alpha = c.W; }
        public Color4(System.Drawing.Color c) { Red = c.R / 255.0f; Green = c.G / 255.0f; Blue = c.B / 255.0f; Alpha = c.A / 255.0f; }
        public float Red, Green, Blue, Alpha;

        public int ToArgb()
        {
            uint r = (uint)Math.Max(Math.Min(Red * 255.0f, 255.0f), 0.0f);
            uint g = (uint)Math.Max(Math.Min(Green * 255.0f, 255.0f), 0.0f);
            uint b = (uint)Math.Max(Math.Min(Blue * 255.0f, 255.0f), 0.0f);
            uint a = (uint)Math.Max(Math.Min(Alpha * 255.0f, 255.0f), 0.0f);
            return (int)((a << 24) | (r << 16) | (g << 8) | b);
        }

        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb(ToArgb());
        }

        public override bool Equals(object o)
        {
            if (o is Color4)
            {
                Color4 v = (Color4)o;
                return this == v;
            }
            else
            {
                return false;
            }
        }

        public static Color4 operator +(Color4 left, Color4 right) { return new Color4(left.Red + right.Red, left.Green + right.Green, left.Blue + right.Blue, left.Alpha + right.Alpha); }
        public static Color4 operator -(Color4 left, Color4 right) { return new Color4(left.Red - right.Red, left.Green - right.Green, left.Blue - right.Blue, left.Alpha - right.Alpha); }
        public static Color4 operator -(Color4 v) { return new Color4(-v.Red, -v.Green, -v.Blue, -v.Alpha); }

        public override int GetHashCode() { return Red.GetHashCode() + Green.GetHashCode() + Blue.GetHashCode() + Alpha.GetHashCode(); }
        public static bool operator ==(Color4 left, Color4 right) { return left.Red == right.Red && left.Green == right.Green && left.Blue == right.Blue && left.Alpha == right.Alpha; }
        public static bool operator !=(Color4 left, Color4 right) { return left.Red != right.Red || left.Green != right.Green || left.Blue != right.Blue || left.Alpha != right.Alpha; }
    }

    public struct Vector2
    {
        public Vector2(float v) { X = v; Y = v; }
        public Vector2(float x, float y) { X = x; Y = y; }
        public float X;
        public float Y;

        public static Vector2 Hermite(Vector2 value1, Vector2 tangent1, Vector2 value2, Vector2 tangent2, float amount) { throw new NotImplementedException(); }

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

        public override int GetHashCode() { return X.GetHashCode() + Y.GetHashCode(); }

        public static Vector2 operator +(Vector2 left, Vector2 right) { return new Vector2(left.X + right.X, left.Y + right.Y); }
        public static Vector2 operator -(Vector2 left, Vector2 right) { return new Vector2(left.X - right.X, left.Y - right.Y); }
        public static Vector2 operator -(Vector2 v) { return new Vector2(-v.X, -v.Y); }

        public static bool operator ==(Vector2 left, Vector2 right) { return left.X == right.X && left.Y == right.Y; }
        public static bool operator !=(Vector2 left, Vector2 right) { return left.X != right.X || left.Y != right.Y; }
    }

    public struct Vector3
    {
        public Vector3(float v) { X = v; Y = v; Z = v; }
        public Vector3(Vector2 xy, float z) { X = xy.X; Y = xy.Y; Z = z; }
        public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }
        public float X;
        public float Y;
        public float Z;

        public static Vector4 Transform(Vector3 v, Matrix m) { throw new NotImplementedException(); }
        public static Vector3 Hermite(Vector3 value1, Vector3 tangent1, Vector3 value2, Vector3 tangent2, float amount) { throw new NotImplementedException(); }

        public static float DistanceSquared(Vector3 a, Vector3 b) { Vector3 c = b - a; return Vector3.Dot(c, c); }
        public static float Dot(Vector3 a, Vector3 b) { return a.X * b.X + a.Y * b.Y + a.Z * b.Z; }

        public float Length() { return (float)Math.Sqrt(Vector3.Dot(this, this)); }

        public void Normalize()
        {
            float len = Length();
            if (len > 0.0f)
            {
                X /= len;
                Y /= len;
                Z /= len;
            }
        }

        public override bool Equals(object o)
        {
            if (o is Vector3)
            {
                Vector3 v = (Vector3)o;
                return this == v;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode() { return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode(); }

        public static Vector3 operator +(Vector3 left, Vector3 right) { return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z); }
        public static Vector3 operator -(Vector3 left, Vector3 right) { return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z); }
        public static Vector3 operator -(Vector3 v) { return new Vector3(-v.X, -v.Y, -v.Z); }

        public static bool operator ==(Vector3 left, Vector3 right) { return left.X == right.X && left.Y == right.Y && left.Z == right.Z; }
        public static bool operator !=(Vector3 left, Vector3 right) { return left.X != right.X || left.Y != right.Y || left.Z != right.Z; }
    }

    public struct Vector4
    {
        public Vector4(float v) { X = v; Y = v; Z = v; W = v; }
        public Vector4(Vector2 xy, float z, float w) { X = xy.X; Y = xy.Y; Z = z; W = w; }
        public Vector4(Vector3 xyz, float w) { X = xyz.X; Y = xyz.Y; Z = xyz.Z; W = w; }
        public Vector4(float x, float y, float z, float w) { X = x; Y = y; Z = z; W = w; }
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

        public static Vector4 operator +(Vector4 left, Vector4 right) { return new Vector4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W); }
        public static Vector4 operator -(Vector4 left, Vector4 right) { return new Vector4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W); }
        public static Vector4 operator -(Vector4 v) { return new Vector4(-v.X, -v.Y, -v.Z, -v.W); }

        public override int GetHashCode() { return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode(); }
        public static bool operator ==(Vector4 left, Vector4 right) { return left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.W == right.W; }
        public static bool operator !=(Vector4 left, Vector4 right) { return left.X != right.X || left.Y != right.Y || left.Z != right.Z || left.W != right.W; }
    }
    #endregion

    #region DirectInput mouse handling
    namespace DirectInput
    {
        public enum DeviceAxisMode { Relative }
        public enum CooperativeLevel { Nonexclusive, Foreground }

        public class DirectInput
        {
            public void Dispose() { }
        }

        public class MouseProperties
        {
            public DeviceAxisMode AxisMode { private get; set; }
        }

        public class MouseState
        {
            public float X { get; }
            public float Y { get; }
        }

        public class Result
        {
            public bool IsSuccess { get; }
        }

        public class Mouse
        {
            public Mouse(DirectInput dinput) { }
            public MouseProperties Properties { get; }
            public void SetCooperativeLevel(System.Windows.Forms.Control control, CooperativeLevel level) { }
            public void Acquire() { }
            public void Unacquire() { }
            public Result Poll() { return null; }
            public MouseState GetCurrentState() { return null; }
            public void Dispose() { }
        }
    }
    #endregion

    #region Stop watch timer
    // Actually System.Diagnostics (System.Runtime.Extensions.dll, System.dll, netstandard.dll)
    public class StopWatch
    {
        public long ElapsedMilliseconds { get; }
        public void Reset() { }
        public void Start() { }
    }

    public class Configuration
    {
        public static StopWatch Timer { get; set; }
    }
    #endregion

    #region Exceptions
    namespace DirectInput
    {
        public class DirectInputException : ApplicationException
        {
        }
    }
    #endregion
}
