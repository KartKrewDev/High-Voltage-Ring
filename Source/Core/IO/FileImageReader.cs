
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
            throw new NotImplementedException();
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
