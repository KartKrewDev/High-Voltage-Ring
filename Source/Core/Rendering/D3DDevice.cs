
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
using System.Windows.Forms;
using CodeImp.DoomBuilder.Controls;
using CodeImp.DoomBuilder.Geometry;

#endregion

namespace CodeImp.DoomBuilder.Rendering
{
    #region High level mesh rendering
    public class Mesh
    {
        public Mesh(int indexCount, int vertexCount, MeshFlags flags, VertexElement[] elements) { }

        public VertexBuffer VertexBuffer { get; private set; }
        public IndexBuffer IndexBuffer { get; private set; }

        public void DrawSubset(int index) { }

        public void Dispose() { }
    }

    public class Effect
    {
        public static Effect FromStream(System.IO.Stream stream, ShaderFlags flags, out string errors) { errors = ""; return null; }

        public void SetTexture(EffectHandle handle, BaseTexture texture) { }
        public void SetValue<T>(EffectHandle handle, T value) where T : struct { }
        public EffectHandle GetParameter(EffectHandle parameter, string name) { return null; }
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
    #endregion

    #region Vertex buffer format / Input assembly
    public class VertexDeclaration
    {
        public VertexDeclaration(VertexElement[] elements) { }
        public void Dispose() { }
    }

    public struct VertexElement
    {
        public VertexElement(short stream, short offset, DeclarationType type, DeclarationUsage usage) { }
    }
    #endregion

    #region Buffer objects
    public class VertexBuffer
    {
        public VertexBuffer(int sizeInBytes) { }

        public DataStream Lock(LockFlags flags) { return null; }
        public DataStream Lock(int offset, int size, LockFlags flags) { return null; }
        public void Unlock() { }

        public object Tag { get; set; }

        public bool Disposed { get; private set; }
        public void Dispose() { Disposed = true; }
    }

    public class IndexBuffer
    {
        public DataStream Lock(LockFlags flags) { return null; }
        public void Unlock() { }

        public bool Disposed { get; private set; }
        public void Dispose() { Disposed = true; }
    }

    public class DataStream : IDisposable
    {
        public void Seek(long offset, System.IO.SeekOrigin origin) { }
        public void Write(ushort v) { }
        public void WriteRange(Array data) { }
        public void WriteRange(Array data, long offset, long size) { }
        public void Dispose() { }
    }
    #endregion

    #region Textures
    public class BaseTexture
    {
        public bool Disposed { get; private set; }
        public void Dispose() { Disposed = true; }
    }

    public class Texture : BaseTexture
    {
        public Texture(int width, int height, int levels, Format format) { }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public object Tag { get; set; }

        public void SetPixels(System.Drawing.Bitmap bitmap)
        {
            /*
            BitmapData bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            DataRectangle textureLock = texture.LockRectangle(0, LockFlags.None);
            textureLock.Data.WriteRange(bmlock.Scan0, bmlock.Height * bmlock.Stride);
            texture.UnlockRectangle(0);

            bitmap.UnlockBits(bmpdata);
            */
        }

        internal Plotter LockPlotter(int visibleWidth, int visibleHeight)
        {
            //return new Plotter((PixelColor*)plotlocked.Data.DataPointer.ToPointer(), plotlocked.Pitch / sizeof(PixelColor), Height, visibleWidth, visibleHeight);
            return null;
        }

        public void UnlockPlotter() { }

        public static Texture FromStream(System.IO.Stream stream) { return null; }
        public static Texture FromStream(System.IO.Stream stream, int length, int width, int height, int levels, Format format) { return null; }
    }

    public class CubeTexture : BaseTexture
    {
        public CubeTexture(int size, int levels, Format format) { }

        public void SetPixels(CubeMapFace face, System.Drawing.Bitmap bitmap) { }
    }
    #endregion

    #region Enumerations
    public enum RenderState
    {
        AlphaBlendEnable,
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

    public enum Cull { None, Counterclockwise }
    public enum Blend { InverseSourceAlpha, SourceAlpha, One, BlendFactor }
    public enum BlendOperation { Add, ReverseSubtract }
    public enum FillMode { Solid, Wireframe }
    public enum TransformState { World, View, Projection }
    public enum SamplerState { AddressU, AddressV, AddressW }
    public enum TextureAddress { Wrap, Clamp }
    public enum Format { Unknown, A8R8G8B8 }
    public enum LockFlags { None, Discard }
    public enum MeshFlags { Use32Bit, IndexBufferManaged, VertexBufferManaged, Managed }
    public enum ShaderFlags { None, Debug }
    public enum PrimitiveType { LineList, TriangleList, TriangleStrip }
    public enum CubeMapFace { PositiveX, PositiveY, PositiveZ, NegativeX, NegativeY, NegativeZ }
    public enum TextureFilter { None, Point, Linear, Anisotropic }
    public enum DeclarationType { Float2, Float3, Color }
    public enum DeclarationUsage { Position, Color, TextureCoordinate, Normal }
    #endregion

    #region Device context
    internal class D3DDevice : IDisposable
    {
		internal D3DDevice(RenderTargetControl rendertarget)
		{
			RenderTarget = rendertarget;
            Shaders = new ShaderManager(this);
            SetupSettings();
        }

        public void SetStreamSource(int index, VertexBuffer buffer, long offset, long stride) { }
        public void SetRenderState(RenderState state, float v) { }
        public void SetRenderState(RenderState state, bool v) { }
        public void SetRenderState(RenderState state, int v) { }
        public void SetRenderState(RenderState state, Cull v) { }
        public void SetRenderState(RenderState state, Blend v) { }
        public void SetRenderState(RenderState state, BlendOperation v) { }
        public void SetRenderState(RenderState state, FillMode v) { }
        public Matrix GetTransform(TransformState state) { return Matrix.Identity; }
        public void SetTransform(TransformState state, Matrix matrix) { }
        public void SetSamplerState(int unit, SamplerState state, TextureAddress address) { }
        public void DrawPrimitives(PrimitiveType type, int startIndex, int primitiveCount) { }
        public void DrawUserPrimitives<T>(PrimitiveType type, int startIndex, int primitiveCount, T[] data) where T : struct { }
        public void SetVertexDeclaration(VertexDeclaration decl) { }

        public void Dispose() { }

		internal void RegisterResource(ID3DResource res) { }
		internal void UnregisterResource(ID3DResource res) { }

        public void StartRendering(bool clear, Color4 backcolor)
        {
            //if (clear)
            //    Clear(ClearFlags.Target | ClearFlags.ZBuffer, backcolor, 1f, 0);
        }
        public void StartRendering(bool clear, Color4 backcolor, Texture target, bool usedepthbuffer)
        {
            //if (clear)
            //    Clear(ClearFlags.Target, backcolor, 1f, 0);
        }
        public void FinishRendering() { }
        public void Present() { }
        public void ClearTexture(Color4 backcolor, Texture texture) { }
        public void CopyTexture(Texture src, CubeTexture dst, CubeMapFace face) { }

        //mxd. Anisotropic filtering steps
        public static readonly List<float> AF_STEPS = new List<float> { 1.0f, 2.0f, 4.0f, 8.0f, 16.0f }; 
		
		//mxd. Antialiasing steps
		public static readonly List<int> AA_STEPS = new List<int> { 0, 2, 4, 8 };

		internal RenderTargetControl RenderTarget { get; private set; }
		internal ShaderManager Shaders { get; private set; }

		public void SetupSettings()
		{
			// Setup renderstates
			SetRenderState(RenderState.AlphaBlendEnable, false);
			SetRenderState(RenderState.AlphaRef, 0x0000007E);
			SetRenderState(RenderState.AlphaTestEnable, false);
			SetRenderState(RenderState.CullMode, Cull.None);
			SetRenderState(RenderState.DestinationBlend, Blend.InverseSourceAlpha);
			SetRenderState(RenderState.FillMode, FillMode.Solid);
			SetRenderState(RenderState.FogEnable, false);
			SetRenderState(RenderState.MultisampleAntialias, (General.Settings.AntiAliasingSamples > 0));
			SetRenderState(RenderState.SourceBlend, Blend.SourceAlpha);
			SetRenderState(RenderState.TextureFactor, -1);
			SetRenderState(RenderState.ZEnable, false);
			SetRenderState(RenderState.ZWriteEnable, false);

			// Matrices
			SetTransform(TransformState.World, Matrix.Identity);
			SetTransform(TransformState.View, Matrix.Identity);
			SetTransform(TransformState.Projection, Matrix.Identity);
			
			// Texture addressing
			SetSamplerState(0, SamplerState.AddressU, TextureAddress.Wrap);
			SetSamplerState(0, SamplerState.AddressV, TextureAddress.Wrap);
			SetSamplerState(0, SamplerState.AddressW, TextureAddress.Wrap);
			
			// Shader settings
			Shaders.World3D.SetConstants(General.Settings.VisualBilinear, General.Settings.FilterAnisotropy);
			
			// Initialize presentations
			Presentation.Initialize();
		}

		// Make a color from ARGB
		public static int ARGB(float a, float r, float g, float b)
		{
			return Color.FromArgb((int)(a * 255f), (int)(r * 255f), (int)(g * 255f), (int)(b * 255f)).ToArgb();
		}

		// Make a color from RGB
		public static int RGB(int r, int g, int b)
		{
			return Color.FromArgb(255, r, g, b).ToArgb();
		}

		// This makes a Vector3 from Vector3D
		public static Vector3 V3(Vector3D v3d)
		{
			return new Vector3(v3d.x, v3d.y, v3d.z);
		}

		// This makes a Vector3D from Vector3
		public static Vector3D V3D(Vector3 v3)
		{
			return new Vector3D(v3.X, v3.Y, v3.Z);
		}

		// This makes a Vector2 from Vector2D
		public static Vector2 V2(Vector2D v2d)
		{
			return new Vector2(v2d.x, v2d.y);
		}

		// This makes a Vector2D from Vector2
		public static Vector2D V2D(Vector2 v2)
		{
			return new Vector2D(v2.X, v2.Y);
		}
    }
    #endregion
}
