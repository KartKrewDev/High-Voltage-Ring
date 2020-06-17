#region ================== Copyright (c) 2020 Boris Iwanski

/*
 * This program is free software: you can redistribute it and/or modify
 *
 * it under the terms of the GNU General Public License as published by
 * 
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 * 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * 
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.If not, see<http://www.gnu.org/licenses/>.
 */

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;
using System.Diagnostics;

namespace CodeImp.DoomBuilder.BuilderModes.IO
{
	internal struct ImageExportSettings
	{
		public string Name;
		public string Path;
		public bool Floor;
		public PixelFormat PixelFormat;
		public ImageFormat ImageFormat;

		public ImageExportSettings(string name, string path, bool floor, PixelFormat pformat, ImageFormat iformat)
		{
			Name = name;
			Path = path;
			Floor = floor;
			PixelFormat = pformat;
			ImageFormat = iformat;
		}
	}

	internal class ImageExporter
	{
		public void Export(ICollection<Sector> sectors, ImageExportSettings settings)
		{
			Bitmap bitmap;
			Vector2D offset = new Vector2D(double.MaxValue, double.MinValue);
			Vector2D size = new Vector2D(double.MinValue, double.MaxValue);

			HashSet<Vertex> vertices = new HashSet<Vertex>();

			// Find the top left and bottom right corners of the selection
			foreach(Sector s in sectors)
			{
				foreach (Sidedef sd in s.Sidedefs)
				{
					foreach (Vertex v in new Vertex[] { sd.Line.Start, sd.Line.End })
					{
						if (v.Position.x < offset.x)
							offset.x = v.Position.x;

						if (v.Position.x > size.x)
							size.x = v.Position.x;

						if (v.Position.y > offset.y)
							offset.y = v.Position.y;

						if (v.Position.y < size.y)
							size.y = v.Position.y;
					}
				}
			}

			// Right now "size" is the bottom right corener of the selection, so subtract the offset
			// (top left corner of the selection). y will always be negative, so make it positive
			size -= offset;
			size.y *= -1.0;

			bitmap = new Bitmap((int)size.x, (int)size.y, settings.PixelFormat);

			Graphics g = Graphics.FromImage(bitmap);
			g.Clear(Color.Black); // If we don't clear to black we'll see seams where the sectors touch, due to the AA
			g.InterpolationMode = InterpolationMode.HighQualityBilinear;
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.SmoothingMode = SmoothingMode.AntiAlias; // Without AA the sector edges will be quite rough

			foreach (Sector s in sectors)
			{
				GraphicsPath p = new GraphicsPath();
				float rotation = (float)s.Fields.GetValue("rotationfloor", 0.0);

				// If a sector is rotated any offset is on the rotated axes. But we need to offset by
				// map coordinates. We'll use this vector to compute that offset
				Vector2D rotationvector = Vector2D.FromAngle(Angle2D.DegToRad(rotation) + Angle2D.PIHALF);

				// Sectors are triangulated, so draw every triangle
				for (int i = 0; i < s.Triangles.Vertices.Count / 3; i++)
				{
					// The GDI image has the 0/0 coordinate in the top left, so invert the y component
					Vector2D v1 = s.Triangles.Vertices[i * 3] - offset; v1.y *= -1.0;
					Vector2D v2 = s.Triangles.Vertices[i * 3 + 1] - offset; v2.y *= -1.0;
					Vector2D v3 = s.Triangles.Vertices[i * 3 + 2] - offset; v3.y *= -1.0;

					p.AddLine((float)v1.x, (float)v1.y, (float)v2.x, (float)v2.y);
					p.AddLine((float)v2.x, (float)v2.y, (float)v3.x, (float)v3.y);
					p.CloseFigure();
				}

				Bitmap texture;
				
				if(settings.Floor)
					texture = General.Map.Data.GetFlatImage(s.FloorTexture).ExportBitmap();
				else
					texture = General.Map.Data.GetFlatImage(s.CeilTexture).ExportBitmap();

				Vector2D textureoffset = new Vector2D();
				textureoffset.x = s.Fields.GetValue("xpanningfloor", 0.0);
				textureoffset.y = s.Fields.GetValue("ypanningfloor", 0.0);

				// Create the transformation matrix
				Matrix matrix = new Matrix();
				matrix.Rotate(rotation);
				matrix.Translate((float)(-offset.x * rotationvector.x), (float)(offset.x * rotationvector.y)); // Left/right offset from the map origin
				matrix.Translate((float)(offset.y * rotationvector.y), (float)(offset.y * rotationvector.x)); // Up/down offset from the map origin
				matrix.Translate(-(float)textureoffset.x, -(float)textureoffset.y); // Texture offset 

				// Create the texture brush and apply the matrix
				TextureBrush t = new TextureBrush(texture);
				t.Transform = matrix;

				// Draw the islands of the sector
				g.FillPath(t, p);

			}

			// Finally save the image
			bitmap.Save(Path.Combine(settings.Path, settings.Name), settings.ImageFormat);
		}
	}
}
