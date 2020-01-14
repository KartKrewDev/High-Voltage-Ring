
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
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using CodeImp.DoomBuilder.Rendering;
using System.Drawing.Imaging;
using CodeImp.DoomBuilder.Data;

#endregion

namespace CodeImp.DoomBuilder.IO
{
    class PcxImageReader : IImageReader
    {
        public Bitmap ReadAsBitmap(Stream stream, out int offsetx, out int offsety)
        {
            offsetx = int.MinValue;
            offsety = int.MinValue;

            using (BinaryReader reader = new BinaryReader(stream))
            {
                int manufacturer = reader.ReadByte(); // 10=ZSoft
                int version = reader.ReadByte();
                // 0=PC Paintbrush v2.5
                // 2=PC Paintbrush v2.8 w palette information
                // 3=PC Paintbrush v2.8 w/o palette information
                // 4=PC Paintbrush/Windows
                // 5=PC Paintbrush v3.0+
                int encoding = reader.ReadByte(); // 1 = RLE, none other known
                int bitsPerComponent = reader.ReadByte();
                int leftMargin = reader.ReadUInt16();
                int topMargin = reader.ReadUInt16();
                int rightMargin = reader.ReadUInt16();
                int bottomMargin = reader.ReadUInt16();
                int dpiX = reader.ReadUInt16();
                int dpiY = reader.ReadUInt16();
                byte[] egaPalette = reader.ReadBytes(48); // 16 RGB triplets
                reader.ReadByte(); // reserved
                int numColorPlanes = reader.ReadByte();
                int planePitch = reader.ReadUInt16(); // always even
                int paletteInfo = reader.ReadUInt16(); // 1=color/bw palette, 2=grayscale image
                int width = reader.ReadUInt16();
                int height = reader.ReadUInt16();
                reader.ReadBytes(54); // reserved

                int vgaPaletteID = 0;
                byte[] vgaPalette = null;

                int srcpitch = numColorPlanes * planePitch;
                byte[] scanlines = new byte[srcpitch * height];

                int pos = 0;
                while (pos < scanlines.Length)
                {
                    byte value = reader.ReadByte();
                    if ((value & 0xc0) == 0xc0) // two last bits
                    {
                        byte length = (byte)(value & 0x3f);
                        value = reader.ReadByte();
                        while (length > 0)
                        {
                            scanlines[pos++] = value;
                            length--;
                        }
                    }
                    else
                    {
                        scanlines[pos++] = value;
                    }
                }

                byte[] imageData = new byte[width * height * 4];
                int destpitch = width * 4;

                if (bitsPerComponent == 4 && numColorPlanes == 1 && paletteInfo == 1) // 16 colors from a palette
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int srcshift = ((x & 1) << 2);
                            int srcoffset = (x >> 1) + y * srcpitch;
                            int palentry = (scanlines[srcoffset] >> srcshift) & 15;
                            int offset = x * 4 + y * destpitch;
                            imageData[offset + 2] = egaPalette[palentry * 3];
                            imageData[offset + 1] = egaPalette[palentry * 3 + 1];
                            imageData[offset + 0] = egaPalette[palentry * 3 + 2];
                            imageData[offset + 3] = 255;
                        }
                    }
                }
                else if (bitsPerComponent == 4 && numColorPlanes == 4 && paletteInfo == 2) // 4096 colors with 16 levels of transparency
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int srcshift = ((x & 1) << 2);
                            int srcoffset = (x >> 1) + y * srcpitch;
                            int red = (scanlines[srcoffset] >> srcshift) & 15;
                            int green = (scanlines[srcoffset + planePitch] >> srcshift) & 15;
                            int blue = (scanlines[srcoffset + planePitch * 2] >> srcshift) & 15;
                            int alpha = (scanlines[srcoffset + planePitch * 3] >> srcshift) & 15;

                            int offset = x * 4 + y * destpitch;
                            imageData[offset + 2] = (byte)((red * 255 + 7) / 15);
                            imageData[offset + 1] = (byte)((green * 255 + 7) / 15);
                            imageData[offset + 0] = (byte)((blue * 255 + 7) / 15);
                            imageData[offset + 3] = (byte)((alpha * 255 + 7) / 15);
                        }
                    }
                }
                else if (bitsPerComponent == 8 && numColorPlanes == 1 && paletteInfo == 1) // 256 colors from a palette
                {
                    if (version == 5)
                    {
                        vgaPaletteID = reader.ReadByte(); // 0x0c
                        vgaPalette = reader.ReadBytes(768); // 256 RGB triplets
                    }
                    else
                    {
                        throw new InvalidDataException("No vga palette in pcx");
                    }

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int palentry = scanlines[x + y * srcpitch];
                            int offset = x * 4 + y * destpitch;
                            imageData[offset + 2] = vgaPalette[palentry * 3];
                            imageData[offset + 1] = vgaPalette[palentry * 3 + 1];
                            imageData[offset + 0] = vgaPalette[palentry * 3 + 2];
                            imageData[offset + 3] = 255;
                        }
                    }
                }
                else if (bitsPerComponent == 8 && numColorPlanes == 1 && paletteInfo == 2) // 256 shades of gray
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            byte gray = scanlines[x + y * srcpitch];
                            int offset = x * 4 + y * destpitch;
                            imageData[offset + 2] = gray;
                            imageData[offset + 1] = gray;
                            imageData[offset + 0] = gray;
                            imageData[offset + 3] = 255;
                        }
                    }
                }
                else if (bitsPerComponent == 8 && numColorPlanes == 3) // 24 true color
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int srcoffset = x + y * srcpitch;
                            int offset = x * 4 + y * destpitch;
                            imageData[offset + 2] = scanlines[srcoffset];
                            imageData[offset + 1] = scanlines[srcoffset + planePitch];
                            imageData[offset + 0] = scanlines[srcoffset + planePitch * 2];
                            imageData[offset + 3] = 255;
                        }
                    }
                }
                else if (bitsPerComponent == 8 && numColorPlanes == 4) // 24 true color with alpha channel
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            int srcoffset = x + y * srcpitch;
                            int offset = x * 4 + y * destpitch;
                            imageData[offset + 2] = scanlines[srcoffset];
                            imageData[offset + 1] = scanlines[srcoffset + planePitch];
                            imageData[offset + 0] = scanlines[srcoffset + planePitch * 2];
                            imageData[offset + 3] = scanlines[srcoffset + planePitch * 3];
                        }
                    }
                }
                else
                {
                    throw new InvalidDataException(string.Format("Unsupported pcx subformat (bits={0}, planes={1})", bitsPerComponent, numColorPlanes));
                }

                return new Bitmap(width, height, destpitch, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(imageData, 0));
            }
        }
    }

    class TgaImageReader : IImageReader
    {
        public Bitmap ReadAsBitmap(Stream stream, out int offsetx, out int offsety)
        {
            offsetx = int.MinValue;
            offsety = int.MinValue;
            throw new NotImplementedException();
        }
    }

    class FrameworkImageReader : IImageReader
    {
        bool isPng;

        public FrameworkImageReader(bool isPng)
        {
            this.isPng = isPng;
        }

        public Bitmap ReadAsBitmap(Stream stream, out int offsetx, out int offsety)
        {
            using (var image = Image.FromStream(new NoCloseStream(stream)))
            {
                ReadPngOffsets(stream, out offsetx, out offsety);
                return new Bitmap(image);
            }
        }

        void ReadPngOffsets(Stream stream, out int offsetx, out int offsety)
        {
            offsetx = int.MinValue;
            offsety = int.MinValue;

            if (isPng)
            {
                stream.Position = 8;
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    // Read chunks untill we encounter either "grAb" or "IDAT"
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        // Big Endian!
                        int chunklength = ToInt32BigEndian(reader.ReadBytes(4));
                        string chunkname = new string(reader.ReadChars(4));

                        if (chunkname == "grAb")
                        {
                            offsetx = ToInt32BigEndian(reader.ReadBytes(4));
                            offsety = ToInt32BigEndian(reader.ReadBytes(4));
                            break;
                        }
                        else if (chunkname == "IDAT")
                        {
                            break;
                        }
                        else
                        {
                            // Skip the rest of the chunk
                            reader.BaseStream.Position += chunklength + 4;
                        }
                    }
                }
            }
        }

        static int ToInt32BigEndian(byte[] buffer)
        {
            return (buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3];
        }

        // This wraps a stream but makes Close/Dispose do nothing. That prevents.net's Image.FromStream from closing it as we want to fall back to other image loaders.
        class NoCloseStream : Stream
        {
            Stream stream;
            public NoCloseStream(Stream s) { stream = s; }
            public override bool CanRead { get { return stream.CanRead; } }
            public override bool CanSeek { get { return stream.CanSeek; } }
            public override bool CanWrite { get { return stream.CanWrite; } }
            public override bool CanTimeout { get { return stream.CanTimeout; } }
            public override int ReadTimeout { get { return stream.ReadTimeout; } }
            public override int WriteTimeout { get { return stream.WriteTimeout; } }
            public override int EndRead(IAsyncResult asyncResult) { return stream.EndRead(asyncResult); }
            public override void EndWrite(IAsyncResult asyncResult) { stream.EndWrite(asyncResult); }
            public override int ReadByte() { return stream.ReadByte(); }
            public override long Length { get { return stream.Length; } }
            public override void WriteByte(byte value) { stream.WriteByte(value); }
            public override long Position { get { return stream.Position; } set { stream.Position = value; } }
            public override void Flush() { stream.Flush(); }
            public override int Read(byte[] buffer, int offset, int count) { return stream.Read(buffer, offset, count); }
            public override long Seek(long offset, SeekOrigin origin) { return stream.Seek(offset, origin); }
            public override void SetLength(long value) { stream.SetLength(value); }
            public override void Write(byte[] buffer, int offset, int count) { stream.Write(buffer, offset, count); }
        }
    }
}
