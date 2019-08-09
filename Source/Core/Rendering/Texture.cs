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

        IntPtr Handle;

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr Texture_New();

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void Texture_Delete(IntPtr handle);
    }

    public class Texture : BaseTexture
    {
        public Texture(int width, int height, int levels, Format format)
        {
        }

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

        public void UnlockPlotter()
        {
        }

        public static Texture FromStream(System.IO.Stream stream)
        {
            return null;
        }

        public static Texture FromStream(System.IO.Stream stream, int length, int width, int height, int levels, Format format)
        {
            return null;
        }
    }

    public class CubeTexture : BaseTexture
    {
        public CubeTexture(int size, int levels, Format format)
        {
        }

        public void SetPixels(CubeMapFace face, System.Drawing.Bitmap bitmap)
        {
        }
    }

    public enum CubeMapFace { PositiveX, PositiveY, PositiveZ, NegativeX, NegativeY, NegativeZ }
}
