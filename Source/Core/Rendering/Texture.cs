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

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern IntPtr Texture_New();

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Texture_Delete(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Texture_Set2DImage(IntPtr handle, int width, int height);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Texture_SetPixels(IntPtr handle, IntPtr data);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern IntPtr Texture_Lock(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Texture_Unlock(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Texture_SetCubeImage(IntPtr handle, int size);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        protected static extern void Texture_SetCubePixels(IntPtr handle, CubeMapFace face, IntPtr data);
    }

    public class Texture : BaseTexture
    {
        public Texture(int width, int height)
        {
            Width = width;
            Height = height;
            Texture_Set2DImage(Handle, Width, Height);
        }

        public Texture(System.Drawing.Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            Texture_Set2DImage(Handle, Width, Height);
            SetPixels(bitmap);
        }

        public Texture(System.Drawing.Image image)
        {
            using (var bitmap = new System.Drawing.Bitmap(image))
            {
                Width = bitmap.Width;
                Height = bitmap.Height;
                Texture_Set2DImage(Handle, Width, Height);
                SetPixels(bitmap);
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public object Tag { get; set; }

        public void SetPixels(System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Texture_SetPixels(Handle, bmpdata.Scan0);

            bitmap.UnlockBits(bmpdata);
        }

        internal Plotter LockPlotter(int visibleWidth, int visibleHeight)
        {
            unsafe
            {
                IntPtr data = Texture_Lock(Handle);
                return new Plotter((PixelColor*)data.ToPointer(), Width, Height, Math.Min(Width, visibleWidth), Math.Min(Height, visibleHeight));
            }
        }

        public void UnlockPlotter()
        {
            Texture_Unlock(Handle);
        }
    }

    public class CubeTexture : BaseTexture
    {
        public CubeTexture(int size)
        {
            Texture_SetCubeImage(Handle, size);
        }

        public void SetPixels(CubeMapFace face, System.Drawing.Bitmap bitmap)
        {
            System.Drawing.Imaging.BitmapData bmpdata = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Texture_SetCubePixels(Handle, face, bmpdata.Scan0);

            bitmap.UnlockBits(bmpdata);
        }
    }

    public enum CubeMapFace : int { PositiveX, PositiveY, PositiveZ, NegativeX, NegativeY, NegativeZ }
}
