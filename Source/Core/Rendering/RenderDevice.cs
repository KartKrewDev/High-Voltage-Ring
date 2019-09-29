
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
using System.IO;
using System.Text;
using System.Linq;

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

            DeclareShader(ShaderName.display2d_fsaa, "display2d.vp", "display2d_fsaa.fp");
            DeclareShader(ShaderName.display2d_normal, "display2d.vp", "display2d_normal.fp");
            DeclareShader(ShaderName.display2d_fullbright, "display2d.vp", "display2d_fullbright.fp");
            DeclareShader(ShaderName.things2d_thing, "things2d.vp", "things2d_thing.fp");
            DeclareShader(ShaderName.things2d_sprite, "things2d.vp", "things2d_sprite.fp");
            DeclareShader(ShaderName.things2d_fill, "things2d.vp", "things2d_fill.fp");
            DeclareShader(ShaderName.plotter, "plotter.vp", "plotter.fp");
            DeclareShader(ShaderName.world3d_main, "world3d_main.vp", "world3d_main.fp");
            DeclareShader(ShaderName.world3d_fullbright, "world3d_main.vp", "world3d_fullbright.fp");
            DeclareShader(ShaderName.world3d_main_highlight, "world3d_main.vp", "world3d_main_highlight.fp");
            DeclareShader(ShaderName.world3d_fullbright_highlight, "world3d_main.vp", "world3d_fullbright_highlight.fp");
            DeclareShader(ShaderName.world3d_main_vertexcolor, "world3d_customvertexcolor.vp", "world3d_main.fp");
            DeclareShader(ShaderName.world3d_skybox, "world3d_skybox.vp", "world3d_skybox.fp");
            DeclareShader(ShaderName.world3d_main_highlight_vertexcolor, "world3d_customvertexcolor.vp", "world3d_main_highlight.fp");
            DeclareShader(ShaderName.world3d_main_fog, "world3d_lightpass.vp", "world3d_main_fog.fp");
            DeclareShader(ShaderName.world3d_main_highlight_fog, "world3d_lightpass.vp", "world3d_main_highlight_fog.fp");
            DeclareShader(ShaderName.world3d_main_fog_vertexcolor, "world3d_customvertexcolor_fog.vp", "world3d_main_fog.fp");
            DeclareShader(ShaderName.world3d_main_highlight_fog_vertexcolor, "world3d_customvertexcolor_fog.vp", "world3d_main_highlight_fog.fp");
            DeclareShader(ShaderName.world3d_vertex_color, "world3d_main.vp", "world3d_vertex_color.fp");
            DeclareShader(ShaderName.world3d_constant_color, "world3d_customvertexcolor.vp", "world3d_constant_color.fp");
            DeclareShader(ShaderName.world3d_lightpass, "world3d_lightpass.vp", "world3d_lightpass.fp");

            RenderTarget = rendertarget;

            SetupSettings();
        }

        ~RenderDevice()
        {
            Dispose();
        }

        public bool Disposed { get { return Handle == IntPtr.Zero; } }

        void ThrowIfFailed(bool result)
        {
            if (!result)
                throw new RenderDeviceException(Marshal.PtrToStringAnsi(RenderDevice_GetError(Handle)));
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                RenderDevice_Delete(Handle);
                Handle = IntPtr.Zero;
            }
        }

        public void DeclareShader(ShaderName name, string vertResourceName, string fragResourceName)
        {
            RenderDevice_DeclareShader(Handle, name, GetResourceText(vertResourceName), GetResourceText(fragResourceName));
        }

        static string GetResourceText(string name)
        {
            string fullname = string.Format("CodeImp.DoomBuilder.Resources.{0}", name);
            using (Stream stream = General.ThisAssembly.GetManifestResourceStream(fullname))
            {
                if (stream == null)
                    throw new Exception(string.Format("Resource {0} not found!", fullname));
                byte[] data = new byte[stream.Length];
                if (stream.Read(data, 0, data.Length) != data.Length)
                    throw new Exception("Could not read resource stream");
                return Encoding.UTF8.GetString(data);
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
            RenderDevice_SetUniform(Handle, uniform, ref matrix, 16);
        }

        public void SetUniform(UniformName uniform, ref Matrix matrix)
        {
            RenderDevice_SetUniform(Handle, uniform, ref matrix, 16);
        }

        public void SetVertexBuffer(VertexBuffer buffer)
        {
            RenderDevice_SetVertexBuffer(Handle, buffer != null ? buffer.Handle : IntPtr.Zero);
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

        public void SetMultisampleAntialias(bool value)
        {
            RenderDevice_SetMultisampleAntialias(Handle, value);
        }

        public void SetZEnable(bool value)
        {
            RenderDevice_SetZEnable(Handle, value);
        }

        public void SetZWriteEnable(bool value)
        {
            RenderDevice_SetZWriteEnable(Handle, value);
        }

        public void SetTexture(BaseTexture value)
        {
            RenderDevice_SetTexture(Handle, value != null ? value.Handle : IntPtr.Zero);
        }

        public void SetSamplerFilter(TextureFilter filter)
        {
            SetSamplerFilter(filter, filter, TextureFilter.None, 0.0f);
        }

        public void SetSamplerFilter(TextureFilter minfilter, TextureFilter magfilter, TextureFilter mipfilter, float maxanisotropy)
        {
            RenderDevice_SetSamplerFilter(Handle, minfilter, magfilter, mipfilter, maxanisotropy);
        }

        public void SetSamplerState(TextureAddress address)
        {
            RenderDevice_SetSamplerState(Handle, address);
        }

        public void DrawIndexed(PrimitiveType type, int startIndex, int primitiveCount)
        {
            ThrowIfFailed(RenderDevice_DrawIndexed(Handle, type, startIndex, primitiveCount));
        }

        public void Draw(PrimitiveType type, int startIndex, int primitiveCount)
        {
            ThrowIfFailed(RenderDevice_Draw(Handle, type, startIndex, primitiveCount));
        }

        public void Draw(PrimitiveType type, int startIndex, int primitiveCount, FlatVertex[] data)
        {
            ThrowIfFailed(RenderDevice_DrawData(Handle, type, startIndex, primitiveCount, data));
        }

        public void StartRendering(bool clear, Color4 backcolor)
        {
            ThrowIfFailed(RenderDevice_StartRendering(Handle, clear, backcolor.ToArgb(), IntPtr.Zero, true));
        }

        public void StartRendering(bool clear, Color4 backcolor, Texture target, bool usedepthbuffer)
        {
            ThrowIfFailed(RenderDevice_StartRendering(Handle, clear, backcolor.ToArgb(), target.Handle, usedepthbuffer));
        }

        public void FinishRendering()
        {
            ThrowIfFailed(RenderDevice_FinishRendering(Handle));
        }

        public void Present()
        {
            ThrowIfFailed(RenderDevice_Present(Handle));
        }

        public void ClearTexture(Color4 backcolor, Texture texture)
        {
            ThrowIfFailed(RenderDevice_ClearTexture(Handle, backcolor.ToArgb(), texture.Handle));
        }

        public void CopyTexture(CubeTexture dst, CubeMapFace face)
        {
            ThrowIfFailed(RenderDevice_CopyTexture(Handle, dst.Handle, face));
        }

        public void SetBufferData(IndexBuffer buffer, int[] data)
        {
            ThrowIfFailed(RenderDevice_SetIndexBufferData(Handle, buffer.Handle, data, data.Length * Marshal.SizeOf<int>()));
        }

        public void SetBufferData(VertexBuffer buffer, int length, VertexFormat format)
        {
            int stride = (format == VertexFormat.Flat) ? FlatVertex.Stride : WorldVertex.Stride;
            ThrowIfFailed(RenderDevice_SetVertexBufferData(Handle, buffer.Handle, IntPtr.Zero, length * stride, format));
        }

        public void SetBufferData(VertexBuffer buffer, FlatVertex[] data)
        {
            ThrowIfFailed(RenderDevice_SetVertexBufferData(Handle, buffer.Handle, data, data.Length * Marshal.SizeOf<FlatVertex>(), VertexFormat.Flat));
        }

        public void SetBufferData(VertexBuffer buffer, WorldVertex[] data)
        {
            ThrowIfFailed(RenderDevice_SetVertexBufferData(Handle, buffer.Handle, data, data.Length * Marshal.SizeOf<WorldVertex>(), VertexFormat.World));
        }

        public void SetBufferSubdata(VertexBuffer buffer, long destOffset, FlatVertex[] data)
        {
            ThrowIfFailed(RenderDevice_SetVertexBufferSubdata(Handle, buffer.Handle, destOffset * FlatVertex.Stride, data, data.Length * FlatVertex.Stride));
        }

        public void SetBufferSubdata(VertexBuffer buffer, long destOffset, WorldVertex[] data)
        {
            ThrowIfFailed(RenderDevice_SetVertexBufferSubdata(Handle, buffer.Handle, destOffset * WorldVertex.Stride, data, data.Length * WorldVertex.Stride));
        }

        public void SetBufferSubdata(VertexBuffer buffer, FlatVertex[] data, long size)
        {
            if (size < 0 || size > data.Length) throw new ArgumentOutOfRangeException("size");
            ThrowIfFailed(RenderDevice_SetVertexBufferSubdata(Handle, buffer.Handle, 0, data, size * FlatVertex.Stride));
        }

        public void SetPixels(Texture texture, System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                ThrowIfFailed(RenderDevice_SetPixels(Handle, texture.Handle, bmpdata.Scan0));
            }
            finally
            {
                bitmap.UnlockBits(bmpdata);
            }
        }

        public void SetPixels(CubeTexture texture, CubeMapFace face, System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                ThrowIfFailed(RenderDevice_SetCubePixels(Handle, texture.Handle, face, bmpdata.Scan0));
            }
            finally
            {
                bitmap.UnlockBits(bmpdata);
            }
        }

        public unsafe void SetPixels(Texture texture, uint* pixeldata)
        {
            ThrowIfFailed(RenderDevice_SetPixels(Handle, texture.Handle, new IntPtr(pixeldata)));
        }

        public unsafe void* MapPBO(Texture texture)
        {
            void* ptr = RenderDevice_MapPBO(Handle, texture.Handle).ToPointer();
            ThrowIfFailed(ptr != null);
            return ptr;
        }

        public void UnmapPBO(Texture texture)
        {
            ThrowIfFailed(RenderDevice_UnmapPBO(Handle, texture.Handle));
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

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        static extern void RenderDevice_DeclareShader(IntPtr handle, ShaderName name, string vertexShader, string fragShader);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RenderDevice_GetError(IntPtr handle);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_SetShader(IntPtr handle, ShaderName name);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetUniform(IntPtr handle, UniformName name, float[] data, int count);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern void RenderDevice_SetUniform(IntPtr handle, UniformName name, ref Matrix data, int count);

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
        static extern void RenderDevice_SetSamplerState(IntPtr handle, TextureAddress address);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_Draw(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_DrawIndexed(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_DrawData(IntPtr handle, PrimitiveType type, int startIndex, int primitiveCount, FlatVertex[] data);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_StartRendering(IntPtr handle, bool clear, int backcolor, IntPtr target, bool usedepthbuffer);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_FinishRendering(IntPtr handle);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_Present(IntPtr handle);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_ClearTexture(IntPtr handle, int backcolor, IntPtr texture);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_CopyTexture(IntPtr handle, IntPtr dst, CubeMapFace face);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_SetIndexBufferData(IntPtr handle, IntPtr buffer, int[] data, long size);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_SetVertexBufferData(IntPtr handle, IntPtr buffer, IntPtr data, long size, VertexFormat format);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_SetVertexBufferData(IntPtr handle, IntPtr buffer, FlatVertex[] data, long size, VertexFormat format);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_SetVertexBufferData(IntPtr handle, IntPtr buffer, WorldVertex[] data, long size, VertexFormat format);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_SetVertexBufferSubdata(IntPtr handle, IntPtr buffer, long destOffset, FlatVertex[] data, long sizeInBytes);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        static extern bool RenderDevice_SetVertexBufferSubdata(IntPtr handle, IntPtr buffer, long destOffset, WorldVertex[] data, long sizeInBytes);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern bool RenderDevice_SetPixels(IntPtr handle, IntPtr texture, IntPtr data);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern IntPtr RenderDevice_MapPBO(IntPtr handle, IntPtr texture);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern bool RenderDevice_UnmapPBO(IntPtr handle, IntPtr texture);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern bool RenderDevice_SetCubePixels(IntPtr handle, IntPtr texture, CubeMapFace face, IntPtr data);

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
