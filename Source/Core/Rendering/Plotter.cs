
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
	internal unsafe sealed class Plotter : IDisposable
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
            pixels = (PixelColor*)graphics.LockTexture(Texture).ToPointer();

            if (clear)
                General.ZeroMemory(new IntPtr(pixels), Width * Height * sizeof(PixelColor));

            foreach (var command in commands)
            {
                command();
            }

            commands.Clear();
            clear = false;

            graphics.UnlockTexture(Texture);
        }

		// This clears all pixels black
		public void Clear()
		{
            clear = true;
            commands.Clear();
		}
		
		// This draws a pixel normally
		public void DrawVertexSolid(int x, int y, int size, PixelColor c, PixelColor l, PixelColor d)
		{
            commands.Add(() =>
            {
                int width = Width;
                int height = Height;
                int x1 = x - size;
                int x2 = x + size;
                int y1 = y - size;
                int y2 = y + size;

                // Do unchecked?
                if ((x1 >= 0) && (x2 < width) && (y1 >= 0) && (y2 < height))
                {
                    // Filled square
                    for (int yp = y1; yp <= y2; yp++)
                        for (int xp = x1; xp <= x2; xp++)
                            pixels[yp * width + xp] = c;

                    // Vertical edges
                    for (int yp = y1 + 1; yp <= y2 - 1; yp++)
                    {
                        pixels[yp * width + x1] = l;
                        pixels[yp * width + x2] = d;
                    }

                    // Horizontal edges
                    for (int xp = x1 + 1; xp <= x2 - 1; xp++)
                    {
                        pixels[y1 * width + xp] = l;
                        pixels[y2 * width + xp] = d;
                    }

                    // Corners
                    pixels[y2 * width + x2] = d;
                    pixels[y1 * width + x1] = l;
                }
            });
		}

		// This draws a dotted grid line horizontally
		public void DrawGridLineH(int y, int x1, int x2, PixelColor c)
		{
            commands.Add(() =>
            {
                int width = Width;
                int height = Height;
                int numpixels = width >> 1;
                int offset = y & 0x01;
                int ywidth = y * width;
                x1 = General.Clamp(x1 >> 1, 0, numpixels - 1);
                x2 = General.Clamp(x2 >> 1, 0, numpixels - 1);

                if ((y >= 0) && (y < height))
                {
                    // Draw all pixels on this line
                    for (int i = x1; i < x2; i++) pixels[ywidth + ((i << 1) | offset)] = c;
                }
            });
		}

		// This draws a dotted grid line vertically
		public void DrawGridLineV(int x, int y1, int y2, PixelColor c)
		{
            commands.Add(() =>
            {
                int width = Width;
                int height = Height;
                int numpixels = height >> 1;
                int offset = x & 0x01;
                y1 = General.Clamp(y1 >> 1, 0, numpixels - 1);
                y2 = General.Clamp(y2 >> 1, 0, numpixels - 1);

                if ((x >= 0) && (x < width))
                {
                    // Draw all pixels on this line
                    for (int i = y1; i < y2; i++) pixels[((i << 1) | offset) * width + x] = c;
                }
            });
		}

		// This draws a line normally
		// See: http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
		public void DrawLineSolid(int x1, int y1, int x2, int y2, PixelColor c, uint mask = 0xffffffff)
		{
            commands.Add(() =>
            {

                int width = Width;
                int height = Height;

                // Check if the line is outside the screen for sure.
                // This is quickly done by checking in which area both points are. When this
                // is above, below, right or left of the screen, then skip drawing the line.
                if (((x1 < 0) && (x2 < 0)) ||
                   ((x1 > width) && (x2 > width)) ||
                   ((y1 < 0) && (y2 < 0)) ||
                   ((y1 > height) && (y2 > height))) return;

                // Distance of the line
                int dx = x2 - x1;
                int dy = y2 - y1;

                // Positive (absolute) distance
                int dxabs = Math.Abs(dx);
                int dyabs = Math.Abs(dy);

                // Half distance
                int x = dyabs >> 1;
                int y = dxabs >> 1;

                // Direction
                int sdx = Math.Sign(dx);
                int sdy = Math.Sign(dy);

                // Start position
                int px = x1;
                int py = y1;

                // When the line is completely inside screen,
                // then do an unchecked draw, because all of its pixels are
                // guaranteed to be within the memory range
                if ((x1 >= 0) && (x2 >= 0) && (x1 < width) && (x2 < width) &&
                   (y1 >= 0) && (y2 >= 0) && (y1 < height) && (y2 < height))
                {
                    // Draw first pixel
                    pixels[py * width + px] = c;

                    // Check if the line is more horizontal than vertical
                    if (dxabs >= dyabs)
                    {
                        for (int i = 0; i < dxabs; i++)
                        {
                            y += dyabs;
                            if (y >= dxabs)
                            {
                                y -= dxabs;
                                py += sdy;
                            }
                            px += sdx;

                            // Draw pixel
                            if ((mask & (1 << (i & 0x7))) != 0)
                            {
                                pixels[py * width + px] = c;
                            }
                        }
                    }
                    // Else the line is more vertical than horizontal
                    else
                    {
                        for (int i = 0; i < dyabs; i++)
                        {
                            x += dxabs;
                            if (x >= dyabs)
                            {
                                x -= dyabs;
                                px += sdx;
                            }
                            py += sdy;

                            // Draw pixel
                            if ((mask & (1 << (i & 0x7))) != 0)
                            {
                                pixels[py * width + px] = c;
                            }
                        }
                    }
                }
                else
                {
                    // Draw first pixel
                    if ((px >= 0) && (px < width) && (py >= 0) && (py < height))
                        pixels[py * width + px] = c;

                    // Check if the line is more horizontal than vertical
                    if (dxabs >= dyabs)
                    {
                        for (int i = 0; i < dxabs; i++)
                        {
                            y += dyabs;
                            if (y >= dxabs)
                            {
                                y -= dxabs;
                                py += sdy;
                            }
                            px += sdx;

                            // Draw pixel
                            if ((mask & (1 << (i & 0x7))) != 0)
                            {
                                if ((px >= 0) && (px < width) && (py >= 0) && (py < height))
                                    pixels[py * width + px] = c;
                            }
                        }
                    }
                    // Else the line is more vertical than horizontal
                    else
                    {
                        for (int i = 0; i < dyabs; i++)
                        {
                            x += dxabs;
                            if (x >= dyabs)
                            {
                                x -= dyabs;
                                px += sdx;
                            }
                            py += sdy;

                            // Draw pixel
                            if ((mask & (1 << (i & 0x7))) != 0)
                            {
                                if ((px >= 0) && (px < width) && (py >= 0) && (py < height))
                                    pixels[py * width + px] = c;
                            }
                        }
                    }
                }
            });
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

        public void Dispose()
        {
            if (Texture != null) Texture.Dispose();
        }

        PixelColor* pixels;
        bool clear = true;
        List<Action> commands = new List<Action>();

        const int DASH_INTERVAL = 16;
    }
}
