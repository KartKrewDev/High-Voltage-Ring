using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeImp.DoomBuilder.Rendering
{
    public class Mesh : IDisposable
    {
        public Mesh(VertexElement[] vertexDecl, WorldVertex[] vertexData, int[] indexData)
        {
            VertexDecl = new VertexDeclaration(vertexDecl);
            unsafe { Vertices = new VertexBuffer(sizeof(WorldVertex)); }
            Vertices.SetBufferData(vertexData);
            Indices = new IndexBuffer(sizeof(int) * indexData.Length);
            Indices.SetBufferData(indexData);
        }

        ~Mesh()
        {
            Dispose();
        }

        public void DrawSubset(int index)
        {
        }

        public void Dispose()
        {
            if (Vertices != null) Vertices.Dispose();
            if (Indices != null) Indices.Dispose();
        }

        VertexDeclaration VertexDecl;
        VertexBuffer Vertices;
        IndexBuffer Indices;
    }
}
