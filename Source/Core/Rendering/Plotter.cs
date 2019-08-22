
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
using CodeImp.DoomBuilder.Geometry;

#endregion

namespace CodeImp.DoomBuilder.Rendering
{
	internal sealed class Plotter : IDisposable
	{
		public Plotter(int width, int height)
		{
            this.Texture = new Texture(width, height);
        }

        ~Plotter()
        {
            Dispose();
        }

        public int Width { get { return Texture.Width; } }
        public int Height { get { return Texture.Height; } }
        public Texture Texture { get; set; }

        public void DrawContents(RenderDevice graphics)
        {
            if (clear == false && vertices.Count == 0)
                return;

            var projmat = Matrix.Scaling(2.0f / this.Texture.Width, 2.0f / this.Texture.Height, 1.0f) * Matrix.Translation(-1.0f, -1.0f, 0.0f);

            graphics.StartRendering(clear, new Color4(0), this.Texture, false);
            graphics.SetShader(ShaderName.plotter);
            graphics.SetUniform(UniformName.projection, projmat);
            graphics.SetAlphaBlendEnable(true);
            graphics.SetBlendOperation(BlendOperation.Add);
            graphics.SetSourceBlend(Blend.SourceAlpha);
            graphics.SetDestinationBlend(Blend.InverseSourceAlpha);
            graphics.Draw(PrimitiveType.TriangleList, 0, vertices.Count / 3, vertices.ToArray());
            graphics.SetAlphaBlendEnable(false);
            graphics.FinishRendering();

            clear = false;
            vertices.Clear();
        }

        void DrawLine(int x0, int y0, int x1, int y1, int c, bool dotted = false)
        {
            var v = new FlatVertex();
            v.c = c;

            float nx, ny, len;
            if (x0 == x1)
            {
                nx = 1.0f;
                ny = 0.0f;
                len = y1 - y0;
            }
            else if (y0 == y1)
            {
                nx = 0.0f;
                ny = 1.0f;
                len = x1 - x0;
            }
            else
            {
                nx = (float)(y1 - y0);
                ny = (float)-(x1 - x0);
                len = (float)Math.Sqrt(nx * nx + ny * ny);
                nx /= len;
                ny /= len;
            }

            float dx = -ny * 0.5f;
            float dy = nx * 0.5f;

            float xx0 = x0 + 0.5f - dx;
            float yy0 = y0 + 0.5f - dy;
            float xx1 = x1 + 0.5f + dx;
            float yy1 = y1 + 0.5f + dy;

            float start, end;
            if (!dotted)
            {
                start = 0.5f;
                end = 0.5f;
            }
            else
            {
                start = 0.0f;
                end = len;
            }

            float lineextent = 3.0f; // line width in shader + 1
            nx *= lineextent;
            ny *= lineextent;

            v.u = start; v.v = -lineextent; v.x = xx0 - nx; v.y = yy0 - ny; vertices.Add(v);
            v.u = start; v.v = lineextent; v.x = xx0 + nx; v.y = yy0 + ny; vertices.Add(v);
            v.u = end; v.v = lineextent; v.x = xx1 + nx; v.y = yy1 + ny; vertices.Add(v);
            vertices.Add(v);
            v.u = end; v.v = -lineextent; v.x = xx1 - nx; v.y = yy1 - ny; vertices.Add(v);
            v.u = start; v.v = -lineextent; v.x = xx0 - nx; v.y = yy0 - ny; vertices.Add(v);
        }

        void FillBox(int x0, int y0, int x1, int y1, int c)
        {
            var v = new FlatVertex();
            v.c = c;
            v.u = 0.5f;
            v.v = 0.0f;

            v.x = x0; v.y = y0; vertices.Add(v);
            v.x = x1; v.y = y0; vertices.Add(v);
            v.x = x1; v.y = y1; vertices.Add(v);
            vertices.Add(v);
            v.x = x0; v.y = y1; vertices.Add(v);
            v.x = x0; v.y = y0; vertices.Add(v);
        }

        public void DrawVertexSolid(int x, int y, int size, PixelColor c, PixelColor l, PixelColor d)
        {
            int x0 = x - size;
            int x1 = x + size;
            int y0 = y - size;
            int y1 = y + size;

            int lightcolor = l.ToInt();
            int darkcolor = d.ToInt();
            int centercolor = c.ToInt();
            DrawLine(x1, y1, x0, y1, darkcolor);
            DrawLine(x1, y1, x1, y0, darkcolor);
            DrawLine(x0, y0, x1, y0, lightcolor);
            DrawLine(x0, y0, x0, y1, lightcolor);
            FillBox(x0 + 1, y0 + 1, x1, y1, centercolor);
        }

        public void DrawGridLineH(int y, int x1, int x2, PixelColor c)
        {
            DrawLine(x1, y, x2, y, c.ToInt());
        }

        public void DrawGridLineV(int x, int y1, int y2, PixelColor c)
        {
            DrawLine(x, y1, x, y2, c.ToInt());
        }

        public void DrawLineSolid(int x1, int y1, int x2, int y2, PixelColor c, bool dotted = false)
        {
            DrawLine(x1, y1, x2, y2, c.ToInt(), dotted);
        }

        public void DrawLine3DFloor(Vector2D start, Vector2D end, PixelColor c, PixelColor c2) 
		{
			Vector2D delta = end - start;
			float length = delta.GetLength();

			if(length < DASH_INTERVAL * 2) 
			{
				DrawLineSolid((int)start.x, (int)start.y, (int)end.x, (int)end.y, c2);
			} 
			else 
			{
				float d1 = DASH_INTERVAL / length;
				float d2 = 1.0f - d1;

				Vector2D p1 = CurveTools.GetPointOnLine(start, end, d1);
				Vector2D p2 = CurveTools.GetPointOnLine(start, end, d2);

				DrawLineSolid((int)start.x, (int)start.y, (int)p1.x, (int)p1.y, c2);
				DrawLineSolid((int)p1.x, (int)p1.y, (int)p2.x, (int)p2.y, c);
				DrawLineSolid((int)p2.x, (int)p2.y, (int)end.x, (int)end.y, c2);
			}
		}

        // This clears all pixels black
        public void Clear()
        {
            clear = true;
        }

        public void Dispose()
        {
            if (Texture != null) Texture.Dispose();
        }

        bool clear = true;
        List<FlatVertex> vertices = new List<FlatVertex>();

        const int DASH_INTERVAL = 16;
    }
}
