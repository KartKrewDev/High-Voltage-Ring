using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CodeImp.DoomBuilder.Rendering
{
    public class VertexDeclaration : IDisposable
    {
        public VertexDeclaration(VertexElement[] elements)
        {
            Handle = VertexDeclaration_New(elements, elements.Length);
            if (Handle == IntPtr.Zero)
                throw new Exception("VertexDeclaration_New failed");
        }

        ~VertexDeclaration()
        {
            Dispose();
        }

        public bool Disposed { get { return Handle == IntPtr.Zero; } }

        public void Dispose()
        {
            if (!Disposed)
            {
                VertexDeclaration_Delete(Handle);
                Handle = IntPtr.Zero;
            }
        }

        internal IntPtr Handle;

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr VertexDeclaration_New(VertexElement[] elements, int count);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void VertexDeclaration_Delete(IntPtr handle);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VertexElement
    {
        public VertexElement(short stream, short offset, DeclarationType type, DeclarationUsage usage)
        {
            Stream = stream;
            Offset = offset;
            Type = type;
            Usage = usage;
        }

        public short Stream;
        public short Offset;
        public DeclarationType Type;
        public DeclarationUsage Usage;
    }

    public enum DeclarationType : int { Float2, Float3, Color }
    public enum DeclarationUsage : int { Position, Color, TextureCoordinate, Normal }
}
