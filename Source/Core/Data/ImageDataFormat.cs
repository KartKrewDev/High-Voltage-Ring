
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

using System.Drawing;
using System.IO;
using CodeImp.DoomBuilder.IO;

#endregion

namespace CodeImp.DoomBuilder.Data
{
	internal static class ImageDataFormat
	{
		// Input guess formats
		public const int UNKNOWN = 0;			// No clue.
		public const int DOOMPICTURE = 1;		// Could be Doom Picture format	(column list rendered data)
		public const int DOOMFLAT = 2;			// Could be Doom Flat format	(raw 8-bit pixel data)
		public const int DOOMCOLORMAP = 3;		// Could be Doom Colormap format (raw 8-bit pixel palette mapping)
		
		// File format signatures
		private static readonly int[] PNG_SIGNATURE = new[] { 137, 80, 78, 71, 13, 10, 26, 10 };
		private static readonly int[] GIF_SIGNATURE = new[] { 71, 73, 70 };
		private static readonly int[] BMP_SIGNATURE = new[] { 66, 77 }; 
		private static readonly int[] DDS_SIGNATURE = new[] { 68, 68, 83, 32 };
		private static readonly int[] JPG_SIGNATURE = new[] { 255, 216, 255 }; //mxd
		private static readonly int[] PCX_SIGNATURE = new[] { 10, 5, 1, 8 }; //mxd

        // Try load image data with the appropriate image reader. Returns null if the image could not be loaded
        public static Bitmap TryLoadImage(Stream data, int guessformat = UNKNOWN, Playpal palette = null)
        {
            int offsetx, offsety;
            return TryLoadImage(data, guessformat, palette, out offsetx, out offsety);
        }

        public static Bitmap TryLoadImage(Stream data, int guessformat, Playpal palette, out int offsetx, out int offsety)
        {
            offsetx = int.MinValue;
            offsety = int.MinValue;
            try
            {
                if (data == null) return null;

                // Data long enough to check for signatures?
                if (data.Length > 10)
                {
                    IImageReader loader = null;
                    if (CheckSignature(data, PNG_SIGNATURE))
                        loader = new FrameworkImageReader(true);
                    else if (CheckSignature(data, JPG_SIGNATURE))
                        loader = new FrameworkImageReader(false);
                    else if (CheckSignature(data, PCX_SIGNATURE))
                        loader = new PcxImageReader();
                    else if (CheckTgaSignature(data))
                        loader = new TgaImageReader();

                    if (loader != null)
                    {
                        data.Seek(0, SeekOrigin.Begin);
                        try
                        {
                            Bitmap image = loader.ReadAsBitmap(data, out offsetx, out offsety);
                            if (image != null) // The older loaders return null when they should throw an exception
                                return image;
                        }
                        catch
                        {
                        }
                    }
                }

                IImageReader doomloader = null;

                // Could it be a doom picture?
                switch (guessformat)
                {
                    case DOOMPICTURE:
                        // Check if data is valid for a doom picture
                        data.Seek(0, SeekOrigin.Begin);
                        DoomPictureReader picreader = new DoomPictureReader(palette);
                        if (picreader.Validate(data)) doomloader = picreader;
                        break;

                    case DOOMFLAT:
                        // Check if data is valid for a doom flat
                        data.Seek(0, SeekOrigin.Begin);
                        DoomFlatReader flatreader = new DoomFlatReader(palette);
                        if (flatreader.Validate(data)) doomloader = flatreader;
                        break;

                    case DOOMCOLORMAP:
                        // Check if data is valid for a doom colormap
                        data.Seek(0, SeekOrigin.Begin);
                        DoomColormapReader colormapreader = new DoomColormapReader(palette);
                        if (colormapreader.Validate(data)) doomloader = colormapreader;
                        break;
                }

                if (doomloader != null)
                {
                    data.Seek(0, SeekOrigin.Begin);
                    Bitmap image = doomloader.ReadAsBitmap(data, out offsetx, out offsety);
                    if (image != null)
                        return image;
                }

                return null;
            }
            catch
            {
                return null;
            }
		}

		// This checks a signature as byte array
		// NOTE: Expects the stream position to be at the start of the
		// signature, and expects the stream to be long enough.
		private static bool CheckSignature(Stream data, int[] sig)
		{
			//mxd. Rewind the data first
			data.Seek(0, SeekOrigin.Begin);
			
			// Go for all bytes
			foreach(int s in sig)
			{
				// When byte doesnt match the signature, leave
				if(data.ReadByte() != s) return false;
			}

			// Signature matches
			return true;
		}

		//mxd. This tries to guess if a given image is in TGA format...
		private static bool CheckTgaSignature(Stream data)
		{
			// TGA header is 18 bytes long
			if(data.Length < 18) return false;
			
			// Rewind the data first
			data.Seek(0, SeekOrigin.Begin);

			// Read TGA header
			int idlength = data.ReadByte(); // Can be 0 or the length of ID string, whatever that is
			int colormap = data.ReadByte(); // Can be 0 or 1
			if(colormap != 0 && colormap != 1) return false;

			int imagetype = data.ReadByte(); // Can be 0, 1, 2, 3, 9, 10, 11
			if((imagetype > 3 && imagetype < 9) || imagetype > 11) return false;

			data.Position += 9; // Skip some stuff...

			int width = data.ReadByte() + (data.ReadByte() << 8);
			if(width <= 0 || width > 8192) return false;

			int height = data.ReadByte() + (data.ReadByte() << 8);
			if(height <= 0 || height > 8192) return false;

			int bitsperpixel = data.ReadByte();  // Can be 8, 16, 24, 32
			return (bitsperpixel == 8 || bitsperpixel == 16 || bitsperpixel == 24 || bitsperpixel == 32);
		}
	}
}
