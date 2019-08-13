using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeImp.DoomBuilder.Rendering
{
    internal class Mesh : IDisposable
    {
        public Mesh(VertexElement[] vertexDecl, WorldVertex[] vertexData, int[] indexData)
        {
            VertexDecl = new VertexDeclaration(vertexDecl);
            unsafe { Vertices = new VertexBuffer(sizeof(WorldVertex)); }
            Vertices.SetBufferData(vertexData);
            Indices = new IndexBuffer(sizeof(int) * indexData.Length);
            Indices.SetBufferData(indexData);
            Count = indexData.Length;
        }

        ~Mesh()
        {
            Dispose();
        }

        internal void Draw(RenderDevice device)
        {
            /*
            device.SetVertexDeclaration(VertexDecl);
            device.SetVertexBuffer(0, Vertices, 0, WorldVertex.Stride);
            device.SetIndexBuffer(Indices);
            device.DrawElements(0, Count);
            device.SetIndexBuffer(null);
            device.SetVertexBuffer(0, null, 0, 0);
            device.SetVertexDeclaration(null);
            */
        }

        public void Dispose()
        {
            if (Vertices != null) Vertices.Dispose();
            if (Indices != null) Indices.Dispose();
        }

        VertexDeclaration VertexDecl;
        VertexBuffer Vertices;
        IndexBuffer Indices;
        int Count;
    }
}
