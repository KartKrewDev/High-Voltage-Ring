
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
using System.Runtime.InteropServices;

#endregion

namespace CodeImp.DoomBuilder.Rendering
{
    internal class RenderDevice : IDisposable
    {
		internal RenderDevice(RenderTargetControl rendertarget)
		{
            Handle = RenderDevice_New(rendertarget.Handle);
            if (Handle == IntPtr.Zero)
                throw new Exception("RenderDevice_New failed");

            RenderTarget = rendertarget;
            Shaders = new ShaderManager(this);
            SetupSettings();
        }

        ~RenderDevice()
        {
            Dispose();
        }

        public bool Disposed { get { return Handle == IntPtr.Zero; } }

        public void Dispose()
        {
            if (!Disposed)
            {
                RenderDevice_Delete(Handle);
                Handle = IntPtr.Zero;
            }
        }

        public void SetVertexBuffer(int index, VertexBuffer buffer, long offset, long stride)
        {
            RenderDevice_SetVertexBuffer(Handle, index, buffer != null ? buffer.Handle : IntPtr.Zero, offset, stride);
        }

        public void SetIndexBuffer(IndexBuffer buffer)
        {
            RenderDevice_SetIndexBuffer(Handle, buffer != null ? buffer.Handle : IntPtr.Zero);
        }

        public void SetAlphaBlendEnable(bool value)
        {
            RenderDevice_SetAlphaBlendEnable(Handle, value);
        }

        public void SetAlphaTestEnable(bool value)
        {
            RenderDevice_SetAlphaTestEnable(Handle, value);
        }

        public void SetCullMode(Cull mode)
        {
            RenderDevice_SetCullMode(Handle, mode);
        }

        public void SetBlendOperation(BlendOperation op)
        {
            RenderDevice_SetBlendOperation(Handle, op);
        }

        public void SetSourceBlend(Blend blend)
        {
            RenderDevice_SetSourceBlend(Handle, blend);
        }

        public void SetDestinationBlend(Blend blend)
        {
            RenderDevice_SetDestinationBlend(Handle, blend);
        }

        public void SetFillMode(FillMode mode)
        {
            RenderDevice_SetFillMode(Handle, mode);
        }

        public void SetFogEnable(bool value)
        {
            RenderDevice_SetFogEnable(Handle, value);
        }

        public void SetFogColor(int value)
        {
            RenderDevice_SetFogColor(Handle, value);
        }

        public void SetFogStart(float value)
        {
            RenderDevice_SetFogStart(Handle, value);
        }

        public void SetFogEnd(float value)
        {
            RenderDevice_SetFogEnd(Handle, value);
        }

        public void SetMultisampleAntialias(bool value)
        {
            RenderDevice_SetMultisampleAntialias(Handle, value);
        }

        public void SetTextureFactor(int factor)
        {
            RenderDevice_SetTextureFactor(Handle, factor);
        }

        public void SetZEnable(bool value)
        {
            RenderDevice_SetZEnable(Handle, value);
        }

        public void SetZWriteEnable(bool value)
        {
            RenderDevice_SetZWriteEnable(Handle, value);
        }

        Matrix[] Transforms = new Matrix[] { Matrix.Identity, Matrix.Identity, Matrix.Identity };

        public Matrix GetTransform(TransformState state)
        {
            return Transforms[(int)state];
        }

        public void SetTransform(TransformState state, Matrix matrix)
        {
            Transforms[(int)state] = matrix;
            RenderDevice_SetTransform(Handle, state, new float[] {
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            });
        }

        public void SetSamplerState(int unit, TextureAddress address)
        {
            SetSamplerState(unit, address, address, address);
        }

        public void SetSamplerState(int unit, TextureAddress addressU, TextureAddress addressV, TextureAddress addressW)
        {
            RenderDevice_SetSamplerState(Handle, unit, addressU, addressV, addressW);
        }

        public void DrawPrimitives(PrimitiveType type, int startIndex, int primitiveCount)
        {
            RenderDevice_DrawPrimitives(Handle, type, startIndex, primitiveCount);
        }

        public void DrawUserPrimitives(PrimitiveType type, int startIndex, int primitiveCount, FlatVertex[] data)
        {
            RenderDevice_DrawUserPrimitives(Handle, type, startIndex, primitiveCount, data);
        }

        public void SetVertexDeclaration(VertexDeclaration decl)
        {
            RenderDevice_SetVertexDeclaration(Handle, decl.Handle);
        }

        public void StartRendering(bool clear, Color4 backcolor)
        {
            RenderDevice_StartRendering(Handle, clear, backcolor.ToArgb(), IntPtr.Zero, true);
        }

        public void StartRendering(bool clear, Color4 backcolor, Texture target, bool usedepthbuffer)
        {
            RenderDevice_StartRendering(Handle, clear, backcolor.ToArgb(), target.Handle, true);
        }

        public void FinishRendering()
        {
            RenderDevice_FinishRendering(Handle);
        }

        public void Present()
        {
            RenderDevice_Present(Handle);
        }

        public void ClearTexture(Color4 backcolor, Texture texture)
        {
            RenderDevice_ClearTexture(Handle, backcolor.ToArgb(), texture.Handle);
        }

        public void CopyTexture(Texture src, CubeTexture dst, CubeMapFace face)
        {
            RenderDevice_CopyTexture(Handle, src.Handle, dst.Handle, face);
        }

        internal void RegisterResource(IRenderResource res)
        {
        }

        internal void UnregisterResource(IRenderResource res)
        {
        }

        public void SetupSettings()
		{
			// Setup renderstates
			SetAlphaBlendEnable(false);
			SetAlphaTestEnable(false);
			SetCullMode(Cull.None);
			SetDestinationBlend(Blend.InverseSourceAlpha);
			SetFillMode(FillMode.Solid);
			SetFogEnable(false);
			SetMultisampleAntialias((General.Settings.AntiAliasingSamples > 0));
			SetSourceBlend(Blend.SourceAlpha);
			SetTextureFactor(-1);
			SetZEnable(false);
			SetZWriteEnable(false);

			// Matrices
			SetTransform(TransformState.World, Matrix.Identity);
			SetTransform(TransformState.View, Matrix.Identity);
			SetTransform(TransformState.Projection, Matrix.Identity);
			
			// Texture addressing
			SetSamplerState(0, TextureAddress.Wrap);
			
			// Shader settings
			Shaders.World3D.SetConstants(General.Settings.VisualBilinear, General.Settings.FilterAnisotropy);
			
			// Initialize presentations
			Presentation.Initialize();
		}

        IntPtr Handle;

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RenderDevice_New(IntPtr hwnd);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_Delete(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBuffer(IntPtr handle, int index, IntPtr buffer, long offset, long stride);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetIndexBuffer(IntPtr handle, IntPtr buffer);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetAlphaBlendEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetAlphaTestEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetCullMode(IntPtr handle, Cull mode);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetBlendOperation(IntPtr handle, BlendOperation op);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetSourceBlend(IntPtr handle, Blend blend);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetDestinationBlend(IntPtr handle, Blend blend);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetFillMode(IntPtr handle, FillMode mode);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetFogEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetFogColor(IntPtr handle, int value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetFogStart(IntPtr handle, float value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetFogEnd(IntPtr handle, float value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetMultisampleAntialias(IntPtr handle, bool value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetTextureFactor(IntPtr handle, int factor);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetZEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetZWriteEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetTransform(IntPtr handle, TransformState state, float[] matrix);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetSamplerState(IntPtr handle, int unit, TextureAddress addressU, TextureAddress addressV, TextureAddress addressW);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_DrawPrimitives(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_DrawUserPrimitives(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount, FlatVertex[] data);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexDeclaration(IntPtr handle, IntPtr decl);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_StartRendering(IntPtr handle, bool clear, int backcolor);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_StartRendering(IntPtr handle, bool clear, int backcolor, IntPtr target, bool usedepthbuffer);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_FinishRendering(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_Present(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_ClearTexture(IntPtr handle, int backcolor, IntPtr texture);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_CopyTexture(IntPtr handle, IntPtr src, IntPtr dst, CubeMapFace face);

        //mxd. Anisotropic filtering steps
        public static readonly List<float> AF_STEPS = new List<float> { 1.0f, 2.0f, 4.0f, 8.0f, 16.0f };

        //mxd. Antialiasing steps
        public static readonly List<int> AA_STEPS = new List<int> { 0, 2, 4, 8 };

        internal RenderTargetControl RenderTarget { get; private set; }
        internal ShaderManager Shaders { get; private set; }

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

    public enum Cull : int { None, Counterclockwise }
    public enum Blend : int { InverseSourceAlpha, SourceAlpha, One, BlendFactor }
    public enum BlendOperation : int { Add, ReverseSubtract }
    public enum FillMode : int { Solid, Wireframe }
    public enum TransformState : int { World, View, Projection }
    public enum TextureAddress : int { Wrap, Clamp }
    public enum PrimitiveType : int { LineList, TriangleList, TriangleStrip }
    public enum TextureFilter : int { None, Point, Linear, Anisotropic }
}
