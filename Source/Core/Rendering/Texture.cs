using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CodeImp.DoomBuilder.Rendering
{
    public class BaseTexture : IDisposable
    {
        public BaseTexture()
        {
            Handle = Texture_New();
            if (Handle == IntPtr.Zero)
                throw new Exception("Texture_New failed");
        }

        ~BaseTexture()
        {
            Dispose();
        }

        public bool Disposed { get { return Handle == IntPtr.Zero; } }

        public void Dispose()
        {
            if (!Disposed)
            {
                Texture_Delete(Handle);
                Handle = IntPtr.Zero;
            }
        }

        internal IntPtr Handle;

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern IntPtr Texture_New();

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Texture_Delete(IntPtr handle);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Texture_Set2DImage(IntPtr handle, int width, int height);

        [DllImport("BuilderNative", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Texture_SetCubeImage(IntPtr handle, int size);
    }

    public class Texture : BaseTexture
    {
        public Texture(int width, int height)
        {
            Width = width;
            Height = height;
            Texture_Set2DImage(Handle, Width, Height);
        }

        public Texture(RenderDevice device, System.Drawing.Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            Texture_Set2DImage(Handle, Width, Height);
            device.SetPixels(this, bitmap);
        }

        public Texture(RenderDevice device, System.Drawing.Image image)
        {
            using (var bitmap = new System.Drawing.Bitmap(image))
            {
                Width = bitmap.Width;
                Height = bitmap.Height;
                Texture_Set2DImage(Handle, Width, Height);
                device.SetPixels(this, bitmap);
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public object Tag { get; set; }
    }

    public class CubeTexture : BaseTexture
    {
        public CubeTexture(RenderDevice device, int size)
        {
            Texture_SetCubeImage(Handle, size);
        }
    }

    public enum CubeMapFace : int { PositiveX, PositiveY, PositiveZ, NegativeX, NegativeY, NegativeZ }
}
