
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
using System.Reflection;

#endregion

namespace CodeImp.DoomBuilder.Rendering
{
    public class RenderDeviceException : Exception
    {
        public RenderDeviceException(string message) : base(message) { }
    }

    public class RenderDevice : IDisposable
    {
		public RenderDevice(RenderTargetControl rendertarget)
		{
		
		// Grab the X11 Display handle by abusing reflection to access internal classes in the mono implementation.
		// That's par for the course for everything in Linux, so yeah..
		IntPtr display = IntPtr.Zero;
		Type xplatui = Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms");
		if (xplatui != null)
		{
		    display = (IntPtr)xplatui.GetField("DisplayHandle", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		}
		
            Handle = RenderDevice_New(display, rendertarget.Handle);
            if (Handle == IntPtr.Zero)
                throw new Exception("RenderDevice_New failed");

            RenderTarget = rendertarget;

            SetupSettings();
        }

        ~RenderDevice()
        {
            Dispose();
        }

        public bool Disposed { get { return Handle == IntPtr.Zero; } }

        private void CheckAndThrow()
        {
            string err = Marshal.PtrToStringAnsi(RenderDevice_GetError(Handle));
            if (err != "")
                throw new RenderDeviceException(err);
        }

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
            CheckAndThrow();
        }

        public void SetUniform(UniformName uniform, bool value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value ? 1.0f : 0.0f }, 1);
            CheckAndThrow();
        }

        public void SetUniform(UniformName uniform, float value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value }, 1);
            CheckAndThrow();
        }

        public void SetUniform(UniformName uniform, Vector2 value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value.X, value.Y }, 2);
            CheckAndThrow();
        }

        public void SetUniform(UniformName uniform, Vector3 value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value.X, value.Y, value.Z }, 3);
            CheckAndThrow();
        }

        public void SetUniform(UniformName uniform, Vector4 value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value.X, value.Y, value.Z, value.W }, 4);
            CheckAndThrow();
        }

        public void SetUniform(UniformName uniform, Color4 value)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] { value.Red, value.Green, value.Blue, value.Alpha }, 4);
            CheckAndThrow();
        }

        public void SetUniform(UniformName uniform, Matrix matrix)
        {
            RenderDevice_SetUniform(Handle, uniform, new float[] {
                matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                matrix.M41, matrix.M42, matrix.M43, matrix.M44
            }, 16);
            CheckAndThrow();
        }

        public void SetVertexBuffer(VertexBuffer buffer)
        {
            RenderDevice_SetVertexBuffer(Handle, buffer != null ? buffer.Handle : IntPtr.Zero);
            CheckAndThrow();
        }

        public void SetIndexBuffer(IndexBuffer buffer)
        {
            RenderDevice_SetIndexBuffer(Handle, buffer != null ? buffer.Handle : IntPtr.Zero);
            CheckAndThrow();
        }

        public void SetAlphaBlendEnable(bool value)
        {
            RenderDevice_SetAlphaBlendEnable(Handle, value);
            CheckAndThrow();
        }

        public void SetAlphaTestEnable(bool value)
        {
            RenderDevice_SetAlphaTestEnable(Handle, value);
            CheckAndThrow();
        }

        public void SetCullMode(Cull mode)
        {
            RenderDevice_SetCullMode(Handle, mode);
            CheckAndThrow();
        }

        public void SetBlendOperation(BlendOperation op)
        {
            RenderDevice_SetBlendOperation(Handle, op);
            CheckAndThrow();
        }

        public void SetSourceBlend(Blend blend)
        {
            RenderDevice_SetSourceBlend(Handle, blend);
            CheckAndThrow();
        }

        public void SetDestinationBlend(Blend blend)
        {
            RenderDevice_SetDestinationBlend(Handle, blend);
            CheckAndThrow();
        }

        public void SetFillMode(FillMode mode)
        {
            RenderDevice_SetFillMode(Handle, mode);
            CheckAndThrow();
        }

        public void SetMultisampleAntialias(bool value)
        {
            RenderDevice_SetMultisampleAntialias(Handle, value);
            CheckAndThrow();
        }

        public void SetZEnable(bool value)
        {
            RenderDevice_SetZEnable(Handle, value);
            CheckAndThrow();
        }

        public void SetZWriteEnable(bool value)
        {
            RenderDevice_SetZWriteEnable(Handle, value);
            CheckAndThrow();
        }

        public void SetTexture(BaseTexture value)
        {
            RenderDevice_SetTexture(Handle, value != null ? value.Handle : IntPtr.Zero);
            CheckAndThrow();
        }

        public void SetSamplerFilter(TextureFilter filter)
        {
            SetSamplerFilter(filter, filter, TextureFilter.None, 0.0f);
            CheckAndThrow();
        }

        public void SetSamplerFilter(TextureFilter minfilter, TextureFilter magfilter, TextureFilter mipfilter, float maxanisotropy)
        {
            RenderDevice_SetSamplerFilter(Handle, minfilter, magfilter, mipfilter, maxanisotropy);
            CheckAndThrow();
        }

        public void SetSamplerState(TextureAddress address)
        {
            SetSamplerState(address, address, address);
            CheckAndThrow();
        }

        public void SetSamplerState(TextureAddress addressU, TextureAddress addressV, TextureAddress addressW)
        {
            RenderDevice_SetSamplerState(Handle, addressU, addressV, addressW);
            CheckAndThrow();
        }

        public void DrawIndexed(PrimitiveType type, int startIndex, int primitiveCount)
        {
            RenderDevice_DrawIndexed(Handle, type, startIndex, primitiveCount);
            CheckAndThrow();
        }

        public void Draw(PrimitiveType type, int startIndex, int primitiveCount)
        {
            RenderDevice_Draw(Handle, type, startIndex, primitiveCount);
            CheckAndThrow();
        }

        public void Draw(PrimitiveType type, int startIndex, int primitiveCount, FlatVertex[] data)
        {
            RenderDevice_DrawData(Handle, type, startIndex, primitiveCount, data);
            CheckAndThrow();
        }

        public void StartRendering(bool clear, Color4 backcolor)
        {
            RenderDevice_StartRendering(Handle, clear, backcolor.ToArgb(), IntPtr.Zero, true);
            CheckAndThrow();
        }

        public void StartRendering(bool clear, Color4 backcolor, Texture target, bool usedepthbuffer)
        {
            RenderDevice_StartRendering(Handle, clear, backcolor.ToArgb(), target.Handle, usedepthbuffer);
            CheckAndThrow();
        }

        public void FinishRendering()
        {
            RenderDevice_FinishRendering(Handle);
            CheckAndThrow();
        }

        public void Present()
        {
            RenderDevice_Present(Handle);
            CheckAndThrow();
        }

        public void ClearTexture(Color4 backcolor, Texture texture)
        {
            RenderDevice_ClearTexture(Handle, backcolor.ToArgb(), texture.Handle);
            CheckAndThrow();
        }

        public void CopyTexture(CubeTexture dst, CubeMapFace face)
        {
            RenderDevice_CopyTexture(Handle, dst.Handle, face);
            CheckAndThrow();
        }

        public void SetBufferData(IndexBuffer buffer, int[] data)
        {
            RenderDevice_SetIndexBufferData(Handle, buffer.Handle, data, data.Length * Marshal.SizeOf<int>());
            CheckAndThrow();
        }

        public void SetBufferData(VertexBuffer buffer, int length, VertexFormat format)
        {
            int stride = (format == VertexFormat.Flat) ? FlatVertex.Stride : WorldVertex.Stride;
            RenderDevice_SetVertexBufferData(Handle, buffer.Handle, IntPtr.Zero, length * stride, format);
            CheckAndThrow();
        }

        public void SetBufferData(VertexBuffer buffer, FlatVertex[] data)
        {
            RenderDevice_SetVertexBufferData(Handle, buffer.Handle, data, data.Length * Marshal.SizeOf<FlatVertex>(), VertexFormat.Flat);
            CheckAndThrow();
        }

        public void SetBufferData(VertexBuffer buffer, WorldVertex[] data)
        {
            RenderDevice_SetVertexBufferData(Handle, buffer.Handle, data, data.Length * Marshal.SizeOf<WorldVertex>(), VertexFormat.World);
            CheckAndThrow();
        }

        public void SetBufferSubdata(VertexBuffer buffer, long destOffset, FlatVertex[] data)
        {
            RenderDevice_SetVertexBufferSubdata(Handle, buffer.Handle, destOffset * FlatVertex.Stride, data, data.Length * FlatVertex.Stride);
            CheckAndThrow();
        }

        public void SetBufferSubdata(VertexBuffer buffer, long destOffset, WorldVertex[] data)
        {
            RenderDevice_SetVertexBufferSubdata(Handle, buffer.Handle, destOffset * WorldVertex.Stride, data, data.Length * WorldVertex.Stride);
            CheckAndThrow();
        }

        public void SetBufferSubdata(VertexBuffer buffer, FlatVertex[] data, long size)
        {
            if (size < 0 || size > data.Length) throw new ArgumentOutOfRangeException("size");
            RenderDevice_SetVertexBufferSubdata(Handle, buffer.Handle, 0, data, size * FlatVertex.Stride);
            CheckAndThrow();
        }

        public void SetPixels(Texture texture, System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            RenderDevice_SetPixels(Handle, texture.Handle, bmpdata.Scan0);

            bitmap.UnlockBits(bmpdata);
            CheckAndThrow();
        }

        public void SetPixels(CubeTexture texture, CubeMapFace face, System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            RenderDevice_SetCubePixels(Handle, texture.Handle, face, bmpdata.Scan0);

            bitmap.UnlockBits(bmpdata);
            CheckAndThrow();
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
			SetMultisampleAntialias((General.Settings.AntiAliasingSamples > 0));
			SetSourceBlend(Blend.SourceAlpha);
			SetUniform(UniformName.texturefactor, new Color4(1f, 1f, 1f, 1f));
			SetZEnable(false);
			SetZWriteEnable(false);
			
			// Texture addressing
			SetSamplerState(TextureAddress.Wrap);
			
            //mxd. It's still nice to have anisotropic filtering when texture filtering is disabled
            TextureFilter magminfilter = (General.Settings.VisualBilinear ? TextureFilter.Linear : TextureFilter.Point);
            SetSamplerFilter(
                General.Settings.FilterAnisotropy > 1.0f ? TextureFilter.Anisotropic : magminfilter,
                magminfilter,
                General.Settings.VisualBilinear ? TextureFilter.Linear : TextureFilter.None, // [SB] use None, otherwise textures are still filtered
                General.Settings.FilterAnisotropy);

            // Initialize presentations
            Presentation.Initialize();
		}

        IntPtr Handle;

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RenderDevice_New(IntPtr display, IntPtr window);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_Delete(IntPtr handle);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RenderDevice_GetError(IntPtr handle);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RenderDevice_SetShader(IntPtr handle, ShaderName name);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RenderDevice_SetUniform(IntPtr handle, UniformName name, float[] data, int count);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBuffer(IntPtr handle, IntPtr buffer);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetIndexBuffer(IntPtr handle, IntPtr buffer);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetAlphaBlendEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetAlphaTestEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetCullMode(IntPtr handle, Cull mode);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetBlendOperation(IntPtr handle, BlendOperation op);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetSourceBlend(IntPtr handle, Blend blend);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetDestinationBlend(IntPtr handle, Blend blend);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetFillMode(IntPtr handle, FillMode mode);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetMultisampleAntialias(IntPtr handle, bool value);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetZEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetZWriteEnable(IntPtr handle, bool value);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetTexture(IntPtr handle, IntPtr texture);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetSamplerFilter(IntPtr handle, TextureFilter minfilter, TextureFilter magfilter, TextureFilter mipfilter, float maxanisotropy);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetSamplerState(IntPtr handle, TextureAddress addressU, TextureAddress addressV, TextureAddress addressW);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_Draw(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_DrawIndexed(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_DrawData(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount, FlatVertex[] data);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_StartRendering(IntPtr handle, bool clear, int backcolor);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_StartRendering(IntPtr handle, bool clear, int backcolor, IntPtr target, bool usedepthbuffer);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_FinishRendering(IntPtr handle);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_Present(IntPtr handle);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_ClearTexture(IntPtr handle, int backcolor, IntPtr texture);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_CopyTexture(IntPtr handle, IntPtr dst, CubeMapFace face);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetIndexBufferData(IntPtr handle, IntPtr buffer, int[] data, long size);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferData(IntPtr handle, IntPtr buffer, IntPtr data, long size, VertexFormat format);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferData(IntPtr handle, IntPtr buffer, FlatVertex[] data, long size, VertexFormat format);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferData(IntPtr handle, IntPtr buffer, WorldVertex[] data, long size, VertexFormat format);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferSubdata(IntPtr handle, IntPtr buffer, long destOffset, FlatVertex[] data, long sizeInBytes);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetVertexBufferSubdata(IntPtr handle, IntPtr buffer, long destOffset, WorldVertex[] data, long sizeInBytes);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void RenderDevice_SetPixels(IntPtr handle, IntPtr texture, IntPtr data);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void RenderDevice_SetCubePixels(IntPtr handle, IntPtr texture, CubeMapFace face, IntPtr data);

        //mxd. Anisotropic filtering steps
        public static readonly List<float> AF_STEPS = new List<float> { 1.0f, 2.0f, 4.0f, 8.0f, 16.0f };

        //mxd. Antialiasing steps
        public static readonly List<int> AA_STEPS = new List<int> { 0, 2, 4, 8 };

        internal RenderTargetControl RenderTarget { get; private set; }

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
        plotter,
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
        world3d_lightpass
    }

    public enum UniformName : int
    {
        rendersettings,
        projection,
        desaturation,
        highlightcolor,
        view,
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
        campos,
        texturefactor,
        fogsettings,
        fogcolor
    }

    public enum VertexFormat : int { Flat, World }
    public enum Cull : int { None, Clockwise }
    public enum Blend : int { InverseSourceAlpha, SourceAlpha, One }
    public enum BlendOperation : int { Add, ReverseSubtract }
    public enum FillMode : int { Solid, Wireframe }
    public enum TextureAddress : int { Wrap, Clamp }
    public enum PrimitiveType : int { LineList, TriangleList, TriangleStrip }
    public enum TextureFilter : int { None, Point, Linear, Anisotropic }
}
