using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeImp.DoomBuilder.Rendering
{
    internal class Mesh : IDisposable
    {
        unsafe public Mesh(RenderDevice graphics, WorldVertex[] vertexData, int[] indexData)
        {
            graphics.SetBufferData(Vertices, vertexData);
            graphics.SetBufferData(Indices, indexData);
            Count = indexData.Length;
        }

        ~Mesh()
        {
            Dispose();
        }

        internal void Draw(RenderDevice device)
        {
            device.SetVertexBuffer(0, Vertices, 0, WorldVertex.Stride);
            device.SetIndexBuffer(Indices);
            device.DrawIndexed(PrimitiveType.TriangleList, 0, Count / 3);
        }

        public void Dispose()
        {
            Vertices.Dispose();
            Indices.Dispose();
        }

        VertexBuffer Vertices = new VertexBuffer();
        IndexBuffer Indices = new IndexBuffer();
        int Count;
    }
}
