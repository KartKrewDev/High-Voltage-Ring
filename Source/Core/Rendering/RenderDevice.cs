
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
    public class RenderDevice : IDisposable
    {
		public RenderDevice(RenderTargetControl rendertarget)
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

        public void SetShader(ShaderName shader)
        {
            RenderDevice_SetShader(Handle, shader);
        }

        public void SetUniform(UniformName uniform, bool value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value ? 1.0f : 0.0f }, 1);
        }

        public void SetUniform(UniformName uniform, float value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value }, 1);
        }

        public void SetUniform(UniformName uniform, Vector2 value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value.X, value.Y }, 2);
        }

        public void SetUniform(UniformName uniform, Vector3 value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value.X, value.Y, value.Z }, 3);
        }

        public void SetUniform(UniformName uniform, Vector4 value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value.X, value.Y, value.Z, value.W }, 4);
        }

        public void SetUniform(UniformName uniform, Color4 value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value.Red, value.Green, value.Blue, value.Alpha }, 4);
        }

        public void SetUniform(UniformName uniform, Matrix matrix)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] {
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            }, 16);
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
            // To do: move to shaders as an uniform
        }

        public void SetFogColor(int value)
        {
            // To do: move to shaders as an uniform
        }

        public void SetFogStart(float value)
        {
            // To do: move to shaders as an uniform
        }

        public void SetFogEnd(float value)
        {
            // To do: move to shaders as an uniform
        }

        public void SetMultisampleAntialias(bool value)
        {
            RenderDevice_SetMultisampleAntialias(Handle, value);
        }

        public void SetTextureFactor(int factor)
        {
            // To do: is this even applied to fragment shaders? if not, delete - if it is, convert to uniform
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
        }

        public void SetTexture(int unit, BaseTexture value)
        {
            RenderDevice_SetTexture(Handle, unit, value != null ? value.Handle : IntPtr.Zero);
        }

        public void SetSamplerFilter(int unit, TextureFilter filter)
        {
            SetSamplerFilter(unit, filter, filter, filter, 0.0f);
        }

        public void SetSamplerFilter(int unit, TextureFilter minfilter, TextureFilter magfilter, TextureFilter mipfilter, float maxanisotropy)
        {
            RenderDevice_SetSamplerFilter(Handle, unit, minfilter, magfilter, mipfilter, maxanisotropy);
        }

        public void SetSamplerState(int unit, TextureAddress address)
        {
            SetSamplerState(unit, address, address, address);
        }

        public void SetSamplerState(int unit, TextureAddress addressU, TextureAddress addressV, TextureAddress addressW)
        {
            RenderDevice_SetSamplerState(Handle, unit, addressU, addressV, addressW);
        }

        public void DrawIndexed(PrimitiveType type, int startIndex, int primitiveCount)
        {
            RenderDevice_DrawIndexed(Handle, type, startIndex, primitiveCount);
        }

        public void Draw(PrimitiveType type, int startIndex, int primitiveCount)
        {
            RenderDevice_Draw(Handle, type, startIndex, primitiveCount);
        }

        public void Draw(PrimitiveType type, int startIndex, int primitiveCount, FlatVertex[] data)
        {
            RenderDevice_DrawData(Handle, type, startIndex, primitiveCount, data, Marshal.SizeOf<FlatVertex>());
        }

        public void SetVertexDeclaration(VertexDeclaration decl)
        {
            RenderDevice_SetVertexDeclaration(Handle, decl != null ? decl.Handle : IntPtr.Zero);
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

        public void SetBufferData(IndexBuffer buffer, int[] data)
        {
            RenderDevice_SetIndexBufferData(Handle, buffer.Handle, data, data.Length * Marshal.SizeOf<int>());
        }

        public void SetBufferData(VertexBuffer buffer, int size)
        {
            RenderDevice_SetVertexBufferData(Handle, buffer.Handle, IntPtr.Zero, size);
        }

        public void SetBufferData(VertexBuffer buffer, FlatVertex[] data)
        {
            RenderDevice_SetVertexBufferData(Handle, buffer.Handle, data, data.Length * Marshal.SizeOf<FlatVertex>());
        }

        public void SetBufferData(VertexBuffer buffer, WorldVertex[] data)
        {
            RenderDevice_SetVertexBufferData(Handle, buffer.Handle, data, data.Length * Marshal.SizeOf<WorldVertex>());
        }

        public void SetBufferSubdata(VertexBuffer buffer, long destOffset, FlatVertex[] data)
        {
            RenderDevice_SetVertexBufferSubdata(Handle, buffer.Handle, destOffset, data, data.Length * Marshal.SizeOf<FlatVertex>());
        }

        public void SetBufferSubdata(VertexBuffer buffer, long destOffset, WorldVertex[] data)
        {
            RenderDevice_SetVertexBufferSubdata(Handle, buffer.Handle, destOffset, data, data.Length * Marshal.SizeOf<WorldVertex>());
        }

        public void SetBufferSubdata(VertexBuffer buffer, long destOffset, FlatVertex[] data, long offset, long size)
        {
            if (data.Length < size || size < 0) throw new ArgumentOutOfRangeException("size");
            RenderDevice_SetVertexBufferSubdata(Handle, buffer.Handle, destOffset, data, size * Marshal.SizeOf<FlatVertex>());
        }

        public void SetPixels(Texture texture, System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            RenderDevice_SetPixels(Handle, texture.Handle, bmpdata.Scan0);

            bitmap.UnlockBits(bmpdata);
        }

        public void SetPixels(CubeTexture texture, CubeMapFace face, System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            RenderDevice_SetCubePixels(Handle, texture.Handle, face, bmpdata.Scan0);

            bitmap.UnlockBits(bmpdata);
        }

        internal Plotter LockPlotter(Texture texture, int visibleWidth, int visibleHeight)
        {
            unsafe
            {
                IntPtr data = RenderDevice_LockTexture(Handle, texture.Handle);
                return new Plotter((PixelColor*)data.ToPointer(), texture.Width, texture.Height, Math.Min(texture.Width, visibleWidth), Math.Min(texture.Height, visibleHeight));
            }
        }

        public void UnlockPlotter(Texture texture)
        {
            RenderDevice_UnlockTexture(Handle, texture.Handle);
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
			Shaders.SetWorld3DConstants(General.Settings.VisualBilinear, General.Settings.FilterAnisotropy);
			
			// Initialize presentations
			Presentation.Initialize();
		}

        IntPtr Handle;

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RenderDevice_New(IntPtr hwnd);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_Delete(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RenderDevice_SetShader(IntPtr hwnd, ShaderName name);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RenderDevice_SetUniform(IntPtr hwnd, UniformName name, float[] data, int count);

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
        static extern void RenderDevice_SetMultisampleAntialias(IntPtr handle, bool value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetZEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetZWriteEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetTexture(IntPtr handle, int unit, IntPtr texture);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetSamplerFilter(IntPtr handle, int unit, TextureFilter minfilter, TextureFilter magfilter, TextureFilter mipfilter, float maxanisotropy);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetSamplerState(IntPtr handle, int unit, TextureAddress addressU, TextureAddress addressV, TextureAddress addressW);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_Draw(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_DrawIndexed(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_DrawData(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount, FlatVertex[] data, int stride);

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

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetIndexBufferData(IntPtr handle, IntPtr buffer, int[] data, long size);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferData(IntPtr handle, IntPtr buffer, IntPtr data, long size);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferData(IntPtr handle, IntPtr buffer, FlatVertex[] data, long size);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferData(IntPtr handle, IntPtr buffer, WorldVertex[] data, long size);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferSubdata(IntPtr handle, IntPtr buffer, long destOffset, FlatVertex[] data, long sizeInBytes);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferSubdata(IntPtr handle, IntPtr buffer, long destOffset, WorldVertex[] data, long sizeInBytes);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void RenderDevice_SetPixels(IntPtr handle, IntPtr texture, IntPtr data);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void RenderDevice_SetCubePixels(IntPtr handle, IntPtr texture, CubeMapFace face, IntPtr data);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern IntPtr RenderDevice_LockTexture(IntPtr handle, IntPtr texture);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void RenderDevice_UnlockTexture(IntPtr handle, IntPtr texture);

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

    public enum ShaderName : int
    {
        display2d_fsaa,
        display2d_normal,
        display2d_fullbright,
        things2d_thing,
        things2d_sprite,
        things2d_fill,
        world3d_main,
        world3d_fullbright,
        world3d_main_highlight,
        world3d_fullbright_highlight,
        world3d_main_vertexcolor,
        world3d_skybox,
        world3d_main_highlight_vertexcolor,
        world3d_p7,
        world3d_main_fog,
        world3d_p9,
        world3d_main_highlight_fog,
        world3d_p11,
        world3d_main_fog_vertexcolor,
        world3d_p13,
        world3d_main_highlight_fog_vertexcolor,
        world3d_vertex_color,
        world3d_constant_color,
        world3d_lightpass // AlphaBlendEnable = true
    }

    public enum UniformName : int
    {
        rendersettings,
        transformsettings,
        desaturation,
        highlightcolor,
        worldviewproj,
        world,
        modelnormal,
        FillColor,
        vertexColor,
        stencilColor,
        lightPosAndRadius,
        lightOrientation,
        light2Radius,
        lightColor,
        ignoreNormals,
        spotLight,
        campos
    }

    public enum Cull : int { None, Clockwise }
    public enum Blend : int { InverseSourceAlpha, SourceAlpha, One, BlendFactor }
    public enum BlendOperation : int { Add, ReverseSubtract }
    public enum FillMode : int { Solid, Wireframe }
    public enum TransformState : int { World, View, Projection }
    public enum TextureAddress : int { Wrap, Clamp }
    public enum PrimitiveType : int { LineList, TriangleList, TriangleStrip }
    public enum TextureFilter : int { None, Point, Linear, Anisotropic }
}
