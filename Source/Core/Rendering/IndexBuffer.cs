using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CodeImp.DoomBuilder.Rendering
{
    public class IndexBuffer : IDisposable
    {
        public IndexBuffer(int sizeInBytes)
        {
            Handle = IndexBuffer_New(sizeInBytes);
            if (Handle == IntPtr.Zero)
                throw new Exception("IndexBuffer_New failed");
        }

        ~IndexBuffer()
        {
            Dispose();
        }

        public void SetBufferData(int[] data)
        {
            IndexBuffer_SetBufferData(Handle, data, data.Length * Marshal.SizeOf<int>());
        }

        public bool Disposed { get { return Handle == IntPtr.Zero; } }

        public void Dispose()
        {
            if (!Disposed)
            {
                IndexBuffer_Delete(Handle);
                Handle = IntPtr.Zero;
            }
        }

        internal IntPtr Handle;

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr IndexBuffer_New(int sizeInBytes);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void IndexBuffer_Delete(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void IndexBuffer_SetBufferData(IntPtr handle, int[] data, long size);
    }
}
