using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CodeImp.DoomBuilder
{
	public static class MD5Hash
	{
		private static MD5 hasher = MD5.Create();
		
		public static string Get(Stream stream)
		{
			// Rewind the stream
			stream.Position = 0;
			
			// Check hash
			byte[] data = hasher.ComputeHash(stream);

			// Rewind the stream again...
			stream.Position = 0;

			// Create a new Stringbuilder to collect the bytes and create a string.
			StringBuilder hash = new StringBuilder();

			// Loop through each byte of the hashed data and format each one as a hexadecimal string.
			for(int i = 0; i < data.Length; i++) hash.Append(data[i].ToString("x2"));

			return hash.ToString();
		}

		/// <summary>
		/// Computes the MD5 hash of a string.
		/// </summary>
		/// <param name="input">The string to compute the MD5 hash of</param>
		/// <returns>The MD5 hash as a string</returns>
		public static string Get(string input)
		{
			byte[] bytes = hasher.ComputeHash(Encoding.ASCII.GetBytes(input));
			return string.Concat(Array.ConvertAll(bytes, x => x.ToString("x2")));
		}
	}
}
