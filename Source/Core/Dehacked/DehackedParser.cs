#region ================== Copyright (c) 2021 Boris Iwanski

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

#region ================== Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Data;

#endregion

namespace CodeImp.DoomBuilder.Dehacked
{
	internal sealed class DehackedParser
	{
		#region ================== Variables

		private StreamReader datareader;
		private List<DehackedThing> things;
		private string sourcename;
		private DataLocation datalocation;
		private int sourcelumpindex;
		private int linenumber;
		private DehackedData dehackeddata;
		private Dictionary<int, DehackedFrame> frames;
		private Dictionary<string, string> texts;
		private Dictionary<int, string> sprites;
		private Dictionary<int, string> renamedsprites;
		private Dictionary<int, string> newsprites;
		private string[] supportedpatchversions = { "19", "21", "2021" };

		#endregion

		#region ================== Properties

		public List<DehackedThing> Things { get { return things; } }
		public Dictionary<string, string> Texts { get { return texts; } }

		#endregion

		#region ================== Constructor

		public DehackedParser()
		{
			things = new List<DehackedThing>();
			frames = new Dictionary<int, DehackedFrame>();
			texts = new Dictionary<string, string>();
			sprites = new Dictionary<int, string>();
			renamedsprites = new Dictionary<int, string>();
			newsprites = new Dictionary<int, string>();
		}

		#endregion

		#region ================== Parsing

		/// <summary>
		/// Parses a dehacked patch.
		/// </summary>
		/// <param name="data">The Dehacked patch text</param>
		/// <param name="dehackeddata">Dehacked data from the game configuration</param>
		/// <param name="availablesprites">All sprite image names available in the resources</param>
		/// <returns></returns>
		public bool Parse(TextResourceData data, DehackedData dehackeddata, HashSet<string> availablesprites)
		{
			string line;
			string fieldkey = string.Empty;
			string fieldvalue = string.Empty;

			sourcename = data.Filename;
			datalocation = data.SourceLocation;
			sourcelumpindex = data.LumpIndex;
			this.dehackeddata = dehackeddata;

			using (datareader = new StreamReader(data.Stream, Encoding.ASCII))
			{
				//if (!ParseHeader())
				//	return false;

				while (!datareader.EndOfStream)
				{
					line = GetLine();
					string lowerline = line.ToLowerInvariant();

					// Skip blank lines and comments
					if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
						continue;

					if (lowerline.StartsWith("thing"))
					{
						if (!ParseThing(line))
							return false;
					}
					else if (lowerline.StartsWith("frame"))
					{
						if (!ParseFrame(line))
							return false;
					}
					else if (lowerline.StartsWith("[sprites]"))
					{
						ParseSprites();
					}
					else if (lowerline.StartsWith("text"))
					{
						if (!ParseText(line))
							return false;
					}
					else if(lowerline.StartsWith("doom version"))
					{
						if (!ParseDoomVersion(line))
							return false;
					}
					else if(lowerline.StartsWith("patch format"))
					{
						if (!ParsePatchFormat(line))
							return false;
					}
					else
					{
						// Just read over any block we don't know or care about
						ParseDummy();
					}
				}
			}

			// Process text replacements. This just renames sprites
			foreach(int key in dehackeddata.Sprites.Keys)
			{
				string sprite = dehackeddata.Sprites[key];
				if (texts.ContainsKey(sprite))
					sprites[key] = texts[sprite];
				else
					sprites[key] = sprite;
			}

			// Replace or add new sprites. Apparently sprites in the [SPRITES] block have precedence over text replacements
			foreach(int key in renamedsprites.Keys)
				sprites[key] = renamedsprites[key];

			foreach(int key in newsprites.Keys)
				// Should anything be done when a new sprite redefines a sprite number that already exists?
				sprites[key] = newsprites[key];
			
			// Assign all frames that have not been redefined in the Dehacked patch to our dictionary of frames
			foreach(int key in dehackeddata.Frames.Keys)
			{
				if (!frames.ContainsKey(key))
					frames[key] = dehackeddata.Frames[key];
			}

			// Process the frames. Pass the base frame to the Process method, since we need to copy properties
			// of the frames that are not defined in the Dehacked patch
			foreach(DehackedFrame f in frames.Values)
				f.Process(sprites, dehackeddata.Frames.ContainsKey(f.Number) ? dehackeddata.Frames[f.Number] : null);

			// Process things. Pass the base thing to the Process method, since we need to copy properties
			// of the thing that are not defined in the Dehacked patch
			foreach (DehackedThing t in things)
				t.Process(frames, dehackeddata.BitMnemonics, dehackeddata.Things.ContainsKey(t.Number) ? dehackeddata.Things[t.Number] : null, availablesprites);

			return true;
		}

		/// <summary>
		/// Returns a new line and increments the line number
		/// </summary>
		/// <returns>The read line</returns>
		private string GetLine()
		{
			linenumber++;
			string line = datareader.ReadLine();

			if (line != null)
				return line.Trim();
			else
				return null;
		}

		/// <summary>
		/// Logs a warning with the given message.
		/// </summary>
		/// <param name="message">The warning message</param>
		private void LogWarning(string message)
		{
			string errsource = Path.Combine(datalocation.GetDisplayName(), sourcename);
			if (sourcelumpindex != -1) errsource += ":" + sourcelumpindex;

			message = "Dehacked warning in \"" + errsource + "\" line " + linenumber + ". " + message + ".";

			TextResourceErrorItem error = new TextResourceErrorItem(ErrorType.Warning, ScriptType.UNKNOWN, datalocation, sourcename, sourcelumpindex, linenumber, message);

			General.ErrorLogger.Add(error);
		}

		/// <summary>
		/// Logs an error with the given message.
		/// </summary>
		/// <param name="message">The error message</param>
		private void LogError(string message)
		{
			string errsource = Path.Combine(datalocation.GetDisplayName(), sourcename);
			if (sourcelumpindex != -1) errsource += ":" + sourcelumpindex;

			message = "Dehacked error in \"" + errsource + "\" line " + linenumber + ". " + message + ".";

			TextResourceErrorItem error = new TextResourceErrorItem(ErrorType.Error, ScriptType.UNKNOWN, datalocation, sourcename, sourcelumpindex, linenumber, message);

			General.ErrorLogger.Add(error);
		}

		/// <summary>
		/// Get a key and value from a line in the format "key = value".
		/// </summary>
		/// <param name="line">The line to get the key and value from</param>
		/// <param name="key">The key is written into this variable</param>
		/// <param name="value">The value is writtin into this variable</param>
		/// <returns>true if a key and value were retrieved, otherwise false</returns>
		private bool GetKeyValueFromLine(string line, out string key, out string value)
		{
			key = string.Empty;
			value = string.Empty;

			if (!line.Contains('='))
			{
				LogError("Expected '=' in line, but it didn't contain one.");
				return false;
			}

			string[] parts = line.Split('=');
			key = parts[0].Trim().ToLowerInvariant();
			value = parts[1].Trim();

			return true;
		}

		/// <summary>
		/// This just keeps reading lines until a blank like is encountered.
		/// </summary>
		private void ParseDummy()
		{
			string line;

			while(true)
			{
				line = GetLine();

				if (string.IsNullOrWhiteSpace(line)) break;
				if (line.StartsWith("#")) continue;
			}
		}

		private bool ParseDoomVersion(string line)
		{
			string fieldkey = string.Empty;
			string fieldvalue = string.Empty;

			// We expect the "Doom version = xxx" string
			if (!GetKeyValueFromLine(line, out fieldkey, out fieldvalue))
				return false;

			if (fieldkey != "doom version")
			{
				LogError("Expected 'Doom version', but got '" + fieldkey + "'.");
				return false;
			}
			else if (!supportedpatchversions.Contains(fieldvalue))
				LogWarning("Unexpected Doom version. Expected one of " + string.Join(", ", supportedpatchversions) + ", got " + fieldvalue + ". Parsing might not work correctly");

			return true;
		}

		private bool ParsePatchFormat(string line)
		{
			string fieldkey = string.Empty;
			string fieldvalue = string.Empty;

			// We expect the "Patch format = xxx" string
			if (!GetKeyValueFromLine(line, out fieldkey, out fieldvalue))
				return false;

			if (fieldkey != "patch format")
			{
				LogError("Expected 'Patch format', but got '" + fieldkey + "'.");
				return false;
			}
			else if (fieldvalue != "6")
				LogWarning("Unexpected patch format. Expected 6, got " + fieldvalue + ". Parsing might not work correctly");

			return true;
		}

		/// <summary>
		/// Parses the header of the Dehacked file.
		/// </summary>
		/// <returns>true if parsing the header was successful, otherwise false</returns>
		private bool ParseHeader()
		{
			string fieldkey = string.Empty;
			string fieldvalue = string.Empty;

			// Read starting header
			string line = GetLine();
			if (line != "Patch File for DeHackEd v3.0")
			{
				LogError("Did not find expected Dehacked file header.");
				return false;
			}

			// Skip all empty lines or comments
			do
			{
				line = GetLine();
				if (line == null)
				{
					LogError("File ended before header could be read.");
					return false;
				}
			} while (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"));

			// Now we expect the "Doom version = xxx" string
			if (!GetKeyValueFromLine(line, out fieldkey, out fieldvalue))
				return false;

			if(fieldkey != "doom version")
			{
				LogError("Expected 'Doom version', but got '" + fieldkey + "'.");
				return false;
			}
			else if (!supportedpatchversions.Contains(fieldvalue))
				LogWarning("Unexpected Doom version. Expected one of " + string.Join(", ", supportedpatchversions) + ", got " + fieldvalue + ". Parsing might not work correctly");

			// Skip all empty lines or comments
			do
			{
				line = GetLine();
				if (line == null)
				{
					LogError("File ended before header could be read.");
					return false;
				}
			} while (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"));

			// Now we expect the "Patch format = xxx" string
			if (!GetKeyValueFromLine(line, out fieldkey, out fieldvalue))
				return false;

			if (fieldkey != "patch format")
			{
				LogError("Expected 'Patch format', but got '" + fieldkey + "'.");
				return false;
			}
			else if (fieldvalue != "6")
				LogWarning("Unexpected patch format. Expected 6, got " + fieldvalue + ". Parsing might not work correctly");

			return true;
		}

		/// <summary>
		/// Parses a Dehacked thing
		/// </summary>
		/// <param name="line">The header of a thing definition block</param>
		/// <returns>true if paring was successful, otherwise false</returns>
		private bool ParseThing(string line)
		{
			// Thing headers have the format "Thing <thingnumber> (<thingname>)". Note that "thingnumber" is not the
			// DoomEdNum, but the Dehacked thing number
			Regex re = new Regex(@"thing\s+(\d+)\s+\((.+)\)", RegexOptions.IgnoreCase);
			Match m = re.Match(line);

			if (!m.Success)
			{
				LogError("Found thing definition, but thing header seems to be wrong.");
				return false;
			}

			int dehthingnumber = int.Parse(m.Groups[1].Value);
			string dehthingname = m.Groups[2].Value;
			string fieldkey = string.Empty;
			string fieldvalue = string.Empty;

			DehackedThing thing = new DehackedThing(dehthingnumber, dehthingname);
			things.Add(thing);

			while(true)
			{
				line = GetLine();

				if (string.IsNullOrWhiteSpace(line))
					break;
				else if (line.StartsWith("#$"))
					line = line.Substring(1);
				else if (line.StartsWith("#")) continue;

				if (!GetKeyValueFromLine(line, out fieldkey, out fieldvalue))
					return false;

				thing.Props[fieldkey] = fieldvalue;
			}

			return true;
		}

		/// <summary>
		/// Parses a Dehacked frame.
		/// </summary>
		/// <param name="line">The header of a frame definition block</param>
		/// <returns>true if paring was successful, otherwise false</returns>
		private bool ParseFrame(string line)
		{
			// Frame headers have the format "Frame <framenumber"
			Regex re = new Regex(@"frame\s+(\d+)", RegexOptions.IgnoreCase);
			Match m = re.Match(line);

			if (!m.Success)
			{
				LogError("Found frame definition, but frame header seems to be wrong.");
				return false;
			}

			int framenumber = int.Parse(m.Groups[1].Value);
			string fieldkey = string.Empty;
			string fieldvalue = string.Empty;

			DehackedFrame frame = new DehackedFrame(framenumber);
			frames[framenumber] = frame;

			while (true)
			{
				line = GetLine();

				if (string.IsNullOrWhiteSpace(line)) break;
				if (line.StartsWith("#")) continue;

				if (!GetKeyValueFromLine(line, out fieldkey, out fieldvalue))
					return false;

				frame.Props[fieldkey] = fieldvalue;
			}

			return true;
		}

		/// <summary>
		/// Parses a Dehacked text replacement
		/// </summary>
		/// <param name="line">The header of a text replacement block</param>
		/// <returns>true if paring was successful, otherwise false</returns>
		private bool ParseText(string line)
		{
			// Text replacement headers have the format "Text <originallength> <newlength>"
			Regex re = new Regex(@"text\s+(\d+)\s+(\d+)", RegexOptions.IgnoreCase);
			Match m = re.Match(line);

			if (!m.Success)
			{
				LogError("Found text replacement definition, but text replacement header seems to be wrong.");
				return false;
			}

			int textreplaceoldcount = int.Parse(m.Groups[1].Value);
			int textreplacenewcount = int.Parse(m.Groups[2].Value);

			// Read the old text character by character
			StringBuilder oldtext = new StringBuilder(textreplaceoldcount);
			while (textreplaceoldcount > 0)
			{
				// Sanity check for malformed patches, for example in dbimpact.wad (see https://github.com/jewalky/UltimateDoomBuilder/issues/673)
				if (datareader.EndOfStream)
				{
					LogError("Reached enexpected end of file when " + textreplaceoldcount + (textreplaceoldcount == 1 ? " more character was" : " more characters were") + " expected");
					return false;
				}

				int c = datareader.Read();

				// Dehacked patches use Windows style CRLF line endings, but text replacements
				// actually only use LF, so we have to ignore the CR
				if (c == '\r') continue;

				// Since we're not reading line by line we have to increment the line number ourselves
				if (c == '\n') linenumber++;

				oldtext.Append(Convert.ToChar(c));
				textreplaceoldcount--;
			}

			StringBuilder newtext = new StringBuilder();
			while (textreplacenewcount > 0)
			{
				// Sanity check for malformed patches, for example in dbimpact.wad (see https://github.com/jewalky/UltimateDoomBuilder/issues/673)
				if (datareader.EndOfStream)
				{
					LogWarning("Reached unexpected end of file when " + textreplacenewcount + (textreplacenewcount == 1 ? " more character was" : " more characters were") + " expected");
					break;
				}

				int c = datareader.Read();

				// Dehacked patches use Windows style CRLF line endings, but text replacements
				// actually only use LF, so we have to ignore the CR
				if (c == '\r') continue;

				// Since we're not reading line by line we have to increment the line number ourselves
				if (c == '\n') linenumber++;

				newtext.Append(Convert.ToChar(c));
				textreplacenewcount--;
			}

			// Sanity check. After reading old and new text there should be a CRLF
			if (!datareader.EndOfStream && datareader.Read() != '\r' && datareader.Read() != '\n')
			{
				LogError("Expected CRLF after text replacement, got something else.");
				return false;
			}

			linenumber++;

			texts[oldtext.ToString()] = newtext.ToString();

			return true;
		}

		/// <summary>
		/// Parses a [SPRITES] block
		/// </summary>
		/// <returns>true if paring was successful, otherwise false</returns>
		private bool ParseSprites()
		{
			string line;
			string fieldkey = string.Empty;
			string fieldvalue = string.Empty;

			while (true)
			{
				line = GetLine();

				if (string.IsNullOrWhiteSpace(line)) break;
				if (line.StartsWith("#")) continue;

				if (!GetKeyValueFromLine(line, out fieldkey, out fieldvalue))
					return false;

				if (fieldvalue.Length != 4)
				{
					LogWarning("New sprite name has to be 4 characters long, but is " + fieldvalue.Length + " characters long. Skipping");
					continue;
				}

				int newspriteindex;

				if(int.TryParse(fieldkey, out newspriteindex))
				{
					// The key is a number, so it's a DSDhacked new sprite
					newsprites[newspriteindex] = fieldvalue;
				}
				else // Regular sprite replacement
				{
					if (fieldkey.Length != 4)
					{
						LogWarning("Old sprite name has to be 4 characters long, but is " + fieldkey.Length + " characters long. Skipping");
						continue;
					}

					// Find the sprite number of the original sprite and remember that we have to rename that
					foreach (int key in dehackeddata.Sprites.Keys)
					{
						if (dehackeddata.Sprites[key].ToLowerInvariant() == fieldkey)
						{
							renamedsprites[key] = fieldvalue;
							break;
						}
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Gets a dictionary of sprite replacements, with the key being the old sprite name, and the value being the new sprite name
		/// </summary>
		/// <returns>Dictionary of sprite replacements</returns>
		public Dictionary<string, string> GetSpriteReplacements()
		{
			Dictionary<string, string> replace = new Dictionary<string, string>();

			// Go through all text replacements
			foreach(string key in texts.Keys)
			{
				if (key.Length != 4 || texts[key].Length != 4) continue; // Sprites must be 4 characters long

				replace[key] = texts[key];
			}

			// Go through all sprite and see if they have an replacement. Apparently they have higher precedence than text replacements
			foreach(int key in dehackeddata.Sprites.Keys)
			{
				if (renamedsprites.ContainsKey(key))
					replace[dehackeddata.Sprites[key]] = renamedsprites[key];
			}

			return replace;
		}

		#endregion


	}
}
