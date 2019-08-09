using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CodeImp.DoomBuilder.Rendering
{
    public class VertexBuffer : IDisposable
    {
        public VertexBuffer(int sizeInBytes)
        {
            Handle = VertexBuffer_New(sizeInBytes);
            if (Handle == IntPtr.Zero)
                throw new Exception("VertexBuffer_New failed");
        }

        ~VertexBuffer()
        {
            Dispose();
        }

        public void SetBufferData(FlatVertex[] data)
        {
            VertexBuffer_SetBufferData(Handle, data, data.Length * Marshal.SizeOf<FlatVertex>());
        }

        public void SetBufferData(WorldVertex[] data)
        {
            VertexBuffer_SetBufferData(Handle, data, data.Length * Marshal.SizeOf<WorldVertex>());
        }

        public void SetBufferSubdata(long destOffset, FlatVertex[] data)
        {
            VertexBuffer_SetBufferSubdata(Handle, destOffset, data, data.Length * Marshal.SizeOf<FlatVertex>());
        }

        public void SetBufferSubdata(long destOffset, WorldVertex[] data)
        {
            VertexBuffer_SetBufferSubdata(Handle, destOffset, data, data.Length * Marshal.SizeOf<WorldVertex>());
        }

        public void SetBufferSubdata(long destOffset, FlatVertex[] data, long offset, long size)
        {
            if (data.Length < size || size < 0) throw new ArgumentOutOfRangeException("size");
            VertexBuffer_SetBufferSubdata(Handle, destOffset, data, size * Marshal.SizeOf<FlatVertex>());
        }

        public bool Disposed { get { return Handle == IntPtr.Zero; } }

        public void Dispose()
        {
            if (!Disposed)
            {
                VertexBuffer_Delete(Handle);
                Handle = IntPtr.Zero;
            }
        }

        IntPtr Handle;

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr VertexBuffer_New(int sizeInBytes);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void VertexBuffer_Delete(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void VertexBuffer_SetBufferData(IntPtr handle, FlatVertex[] data, long size);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void VertexBuffer_SetBufferData(IntPtr handle, WorldVertex[] data, long size);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void VertexBuffer_SetBufferSubdata(IntPtr handle, long destOffset, FlatVertex[] data, long sizeInBytes);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void VertexBuffer_SetBufferSubdata(IntPtr handle, long destOffset, WorldVertex[] data, long sizeInBytes);
    }
}
