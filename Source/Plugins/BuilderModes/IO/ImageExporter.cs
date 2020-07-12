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
	#region ================== Structs

	internal struct ImageExportSettings
	{
		public string Path;
		public string Name;
		public string Extension;
		public bool Floor;
		public bool Fullbright;
		public bool Brightmap;
		public bool Tiles;
		public PixelFormat PixelFormat;
		public ImageFormat ImageFormat;

		public ImageExportSettings(string path, string name, string extension, bool floor, bool fullbright, bool brightmap, bool tiles, PixelFormat pformat, ImageFormat iformat)
		{
			Path = path;
			Name = name;
			Extension = extension;
			Floor = floor;
			Brightmap = brightmap;
			Tiles = tiles;
			Fullbright = fullbright;
			PixelFormat = pformat;
			ImageFormat = iformat;
		}
	}

	#endregion

	internal class ImageExporter
	{
		#region ================== Variables

		private ICollection<Sector> sectors;
		private ImageExportSettings settings;

		#endregion

		#region ================== Constants

		private const int TILE_SIZE = 64;

		#endregion

		#region ================== Constructors

		public ImageExporter(ICollection<Sector> sectors, ImageExportSettings settings)
		{
			this.sectors = sectors;
			this.settings = settings;
		}

		#endregion

		#region ================== Methods

		/// <summary>
		/// Exports the sectors to images
		/// </summary>
		public void Export()
		{
			Bitmap texturebitmap = null;
			Bitmap brightmapbitmap = null;
			Graphics gbrightmap = null;
			Graphics gtexture = null;
			Vector2D offset;
			Vector2D size;

			GetSizeAndOffset(out size, out offset);

			// Normal texture
			texturebitmap = new Bitmap((int)size.x, (int)size.y, settings.PixelFormat);
			gtexture = Graphics.FromImage(texturebitmap);
			gtexture.Clear(Color.Black); // If we don't clear to black we'll see seams where the sectors touch, due to the AA
			gtexture.InterpolationMode = InterpolationMode.HighQualityBilinear;
			gtexture.CompositingQuality = CompositingQuality.HighQuality;
			gtexture.PixelOffsetMode = PixelOffsetMode.HighQuality;
			gtexture.SmoothingMode = SmoothingMode.AntiAlias; // Without AA the sector edges will be quite rough

			// Brightmap
			if (settings.Brightmap)
			{
				brightmapbitmap = new Bitmap((int)size.x, (int)size.y, settings.PixelFormat);
				gbrightmap = Graphics.FromImage(brightmapbitmap);
				gbrightmap.Clear(Color.Black); // If we don't clear to black we'll see seams where the sectors touch, due to the AA
				gbrightmap.InterpolationMode = InterpolationMode.HighQualityBilinear;
				gbrightmap.CompositingQuality = CompositingQuality.HighQuality;
				gbrightmap.PixelOffsetMode = PixelOffsetMode.HighQuality;
				gbrightmap.SmoothingMode = SmoothingMode.AntiAlias; // Without AA the sector edges will be quite rough
			}

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

				Bitmap brushtexture;
				Vector2D textureoffset = new Vector2D();
				Vector2D texturescale = new Vector2D();

				if (settings.Floor)
				{
					brushtexture = General.Map.Data.GetFlatImage(s.FloorTexture).ExportBitmap();

					textureoffset.x = s.Fields.GetValue("xpanningfloor", 0.0);
					textureoffset.y = s.Fields.GetValue("ypanningfloor", 0.0);

					// GZDoom uses bigger numbers for smaller scales (i.e. a scale of 2 will halve the size), so we need to change the scale
					texturescale.x = 1.0 / s.Fields.GetValue("xscalefloor", 1.0);
					texturescale.y = 1.0 / s.Fields.GetValue("yscalefloor", 1.0);
				}
				else
				{
					brushtexture = General.Map.Data.GetFlatImage(s.CeilTexture).ExportBitmap();

					textureoffset.x = s.Fields.GetValue("xpanningceiling", 0.0);
					textureoffset.y = s.Fields.GetValue("ypanningceiling", 0.0);

					// GZDoom uses bigger numbers for smaller scales (i.e. a scale of 2 will halve the size), so we need to change the scale
					texturescale.x = 1.0 / s.Fields.GetValue("xscaleceiling", 1.0);
					texturescale.y = 1.0 / s.Fields.GetValue("yscaleceiling", 1.0);
				}

				if (!settings.Fullbright)
					brushtexture = AdjustBrightness(brushtexture, s.Brightness > 0 ? s.Brightness / 255.0f : 0.0f);

				// Create the transformation matrix
				Matrix matrix = new Matrix();
				matrix.Rotate(rotation);
				matrix.Translate((float)(-offset.x * rotationvector.x), (float)(offset.x * rotationvector.y)); // Left/right offset from the map origin
				matrix.Translate((float)(offset.y * rotationvector.y), (float)(offset.y * rotationvector.x)); // Up/down offset from the map origin
				matrix.Translate(-(float)textureoffset.x, -(float)textureoffset.y); // Texture offset 
				matrix.Scale((float)texturescale.x, (float)texturescale.y);

				// Create the texture brush and apply the matrix
				TextureBrush tbrush = new TextureBrush(brushtexture);
				tbrush.Transform = matrix;

				// Draw the islands of the sector
				gtexture.FillPath(tbrush, p);

				// Create the brightmap based on the sector brightness
				if (settings.Brightmap)
				{
					SolidBrush sbrush = new SolidBrush(Color.FromArgb(255, s.Brightness, s.Brightness, s.Brightness));
					gbrightmap.FillPath(sbrush, p);
				}
			}

			// Finally save the image(s)
			if (settings.Tiles)
			{
				SaveImageAsTiles(texturebitmap);

				if (settings.Brightmap)
					SaveImageAsTiles(brightmapbitmap, "_brightmap");
			}
			else
			{
				texturebitmap.Save(Path.Combine(settings.Path, settings.Name) + settings.Extension, settings.ImageFormat);

				if (settings.Brightmap)
					brightmapbitmap.Save(Path.Combine(settings.Path, settings.Name) + "_brightmap" + settings.Extension, settings.ImageFormat);
			}
		}

		/// <summary>
		/// Saves the image in several uniform sized tiles. It will add numbers starting from 1, going from top left to bottom right, to the filename
		/// </summary>
		/// <param name="image">the image to split into tiles</param>
		/// <param name="suffix">additional suffix for filenames</param>
		private void SaveImageAsTiles(Bitmap image, string suffix="")
		{
			int xnum = (int)Math.Ceiling(image.Size.Width / (double)TILE_SIZE);
			int ynum = (int)Math.Ceiling(image.Size.Height / (double)TILE_SIZE);
			int imagenum = 1;

			for (int y = 0; y < ynum; y++)
			{
				for (int x = 0; x < xnum; x++)
				{
					int width = TILE_SIZE;
					int height = TILE_SIZE;

					// If the width and height are not divisible without remainder make sure only part of the source image is copied
					if (x * TILE_SIZE + TILE_SIZE > image.Size.Width)
						width = image.Size.Width - (x * TILE_SIZE);

					if(y * TILE_SIZE + TILE_SIZE > image.Size.Height)
						height = image.Size.Height - (y * TILE_SIZE);


					Bitmap bitmap = new Bitmap(TILE_SIZE, TILE_SIZE);
					Graphics g = Graphics.FromImage(bitmap);
					g.Clear(Color.Black);
					g.DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(x*TILE_SIZE, y*TILE_SIZE, width, height), GraphicsUnit.Pixel);

					bitmap.Save(string.Format("{0}{1}{2}{3}", Path.Combine(settings.Path, settings.Name), suffix, imagenum, settings.Extension));

					imagenum++;
				}
			}
		}

		/// <summary>
		/// Generates a list of images file names that will be creates
		/// </summary>
		/// <returns>List of image file names</returns>
		public List<string> GetImageNames()
		{
			List<string> imagenames = new List<string>();
			Vector2D offset;
			Vector2D size;

			GetSizeAndOffset(out size, out offset);

			if(settings.Tiles)
			{
				int x = (int)Math.Ceiling(size.x / TILE_SIZE);
				int y = (int)Math.Ceiling(size.y / TILE_SIZE);

				for (int i = 1; i <= x * y; i++)
					imagenames.Add(string.Format("{0}{1}{2}", Path.Combine(settings.Path, settings.Name), i, settings.Extension));

				if(settings.Brightmap)
					for (int i = 1; i <= x * y; i++)
						imagenames.Add(string.Format("{0}{1}_brightmap{2}", Path.Combine(settings.Path, settings.Name), i, settings.Extension));
			}
			else
			{
				imagenames.Add(string.Format("{0}{1}", Path.Combine(settings.Path, settings.Name), settings.Extension));

				if(settings.Brightmap)
					imagenames.Add(string.Format("{0}_brightmap{1}", Path.Combine(settings.Path, settings.Name), settings.Extension));
			}

			return imagenames;

		}

		/// <summary>
		/// Gets the size of the image, based on the sectors, and the offset from the map origin
		/// </summary>
		/// <param name="size">stores the size of the size of the image</param>
		/// <param name="offset">stores the offset from the map origin</param>
		private void GetSizeAndOffset(out Vector2D size, out Vector2D offset)
		{
			offset = new Vector2D(double.MaxValue, double.MinValue);
			size = new Vector2D(double.MinValue, double.MaxValue);

			// Find the top left and bottom right corners of the selection
			foreach (Sector s in sectors)
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
		}

		/// <summary>
		/// Adjusts the brightness of an image. Code by Rod Stephens http://csharphelper.com/blog/2014/10/use-an-imageattributes-object-to-adjust-an-images-brightness-in-c/
		/// </summary>
		/// <param name="image">Base image</param>
		/// <param name="brightness">Brightness between 0.0f and 1.0f</param>
		/// <returns>The new image with changed brightness</returns>
		private Bitmap AdjustBrightness(Image image, float brightness)
		{
			// Make the ColorMatrix.
			float b = brightness;
			ColorMatrix cm = new ColorMatrix(new float[][] {
				new float[] {b, 0, 0, 0, 0},
				new float[] {0, b, 0, 0, 0},
				new float[] {0, 0, b, 0, 0},
				new float[] {0, 0, 0, 1, 0},
				new float[] {0, 0, 0, 0, 1},
			});
			ImageAttributes attributes = new ImageAttributes();
			attributes.SetColorMatrix(cm);

			// Draw the image onto the new bitmap while applying the new ColorMatrix.
			Point[] points = {
				new Point(0, 0),
				new Point(image.Width, 0),
				new Point(0, image.Height),
			};
			Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

			// Make the result bitmap.
			Bitmap bm = new Bitmap(image.Width, image.Height);
			using (Graphics gr = Graphics.FromImage(bm))
			{
				gr.DrawImage(image, points, rect, GraphicsUnit.Pixel, attributes);
			}

			// Return the result.
			return bm;
		}

		#endregion
	}
}
