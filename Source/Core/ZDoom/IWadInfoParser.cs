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

using System.Collections.Generic;
using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Data;

#endregion

namespace CodeImp.DoomBuilder.ZDoom
{
	class IWadInfoParser : ZDTextParser
	{
		#region ================== Variables

		private List<IWadInfo> iwads;

		#endregion

		#region ================== Properties

		internal override ScriptType ScriptType { get { return ScriptType.UNKNOWN; } }
		public List<IWadInfo> IWads { get { return iwads; } }

		#endregion

		#region ================== Constructors

		public IWadInfoParser()
		{
			iwads = new List<IWadInfo>();

			whitespace = "\n \t\r\u00A0";
			specialtokens = ",{}=\n";
		}

		#endregion

		#region ================== Methods

		/// <summary>
		/// Parses DECALDEF data
		/// </summary>
		/// <param name="data">The data to parse</param>
		/// <param name="clearerrors">If errors should be cleared</param>
		/// <returns>true if paring worked, otherwise false</returns>
		public override bool Parse(TextResourceData data, bool clearerrors)
		{
			if (!AddTextResource(data))
			{
				if (clearerrors) ClearError();
				return true;
			}

			// Cannot process?
			if (!base.Parse(data, clearerrors)) return false;

			while (SkipWhitespace(true))
			{
				string token = ReadToken().ToLowerInvariant();
				if (string.IsNullOrEmpty(token)) continue;

				switch (token)
				{
					case "iwad":
						ParseIWad();
						break;
					default:
						SkipStructure();
						break;
				}
			}

			return true;
		}

		/// <summary>
		/// Gets a pair of a key and multiple values.
		/// The key value pair looks like this:
		/// key = value1 [, value2 [, value3 [...] ] ]
		/// </summary>
		/// <param name="key">The key</param>
		/// <param name="values">The list of values</param>
		/// <returns>True if a pair could be parsed, false otherwise</returns>
		private bool GetKeyValuesPair(out string key, out List<string> values)
		{

			string token;

			values = new List<string>();

			SkipWhitespace(true);
			
			key = ReadToken().ToLowerInvariant();

			SkipWhitespace(true);

			token = ReadToken().ToLowerInvariant();

			if(token != "=")
			{
				ReportError("Expected \"=\", but got \"" + token + "\"");
				return false; 
			}

			// Get all values
			do
			{
				SkipWhitespace(true);
				token = ReadToken();
				values.Add(token);
			} while (NextTokenIs(",", false));

			return true;
		}

		/// <summary>
		/// Parses a Iwad block.
		/// </summary>
		/// <returns>True if parsing succeeded, false if it didn't</returns>
		private bool ParseIWad()
		{
			if(!NextTokenIs("{", false))
			{
				ReportError("Expected opening brace");
				return false;
			}

			IWadInfo iwad = new IWadInfo();

			while(SkipWhitespace(true))
			{
				string key;
				List<string> values;

				// If we encounter a closing swirly bracke the end of the block is reached
				if(NextTokenIs("}", false))
				{
					iwads.Add(iwad);
					return true;
				}

				if (!GetKeyValuesPair(out key, out values))
					return false;

				switch(key)
				{
					case "autoname":
						iwad.AutoName = values[0];
						break;
				}
			}

			return false;
		}

		#endregion
	}
}
