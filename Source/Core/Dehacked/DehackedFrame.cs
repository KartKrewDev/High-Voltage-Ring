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

#endregion

namespace CodeImp.DoomBuilder.Dehacked
{
	public class DehackedFrame
	{
		#region ================== Variables

		private int number;
		private int spritenumber;
		private long spritesubnumber;
		private Dictionary<string, string> props;
		private string sprite;
		private bool bright;

		#endregion

		#region ================== Properties

		public int Number { get { return number; } internal set { number = value; } }
		public int SpriteNumber { get { return spritenumber; } internal set { spritenumber = value; } }
		public long SpriteSubNumber { get { return spritesubnumber; } internal set { spritesubnumber = value; } }
		public Dictionary<string, string> Props { get { return props; } }
		public string Sprite { get { return sprite; } internal set { sprite = value; } }
		public bool Bright { get { return bright; } }

		#endregion

		#region ================== Constructor

		internal DehackedFrame(int number)
		{
			this.number = number;
			sprite = string.Empty;
			props = new Dictionary<string, string>();
		}

		internal DehackedFrame(int number, Dictionary<string, string> props) : this(number)
		{
			foreach (string key in props.Keys)
				this.props[key.ToLowerInvariant()] = props[key];
		}

		#endregion

		#region ================== Methods

		/// <summary>
		/// Processes the frame, setting it up so it can be used by things
		/// </summary>
		/// <param name="definedsprites">All available Dehacked sprites</param>
		/// <param name="baseframe">The base Dehacked frame</param>
		internal void Process(Dictionary<int, string> definedsprites, DehackedFrame baseframe)
		{
			// Copy all missing properties of the base frame
			if(baseframe != null)
			{
				foreach (string key in baseframe.Props.Keys)
					if (!props.ContainsKey(key))
						props[key] = baseframe.props[key];
			}

			foreach (KeyValuePair<string, string> kvp in props)
			{
				string prop = kvp.Key.ToLowerInvariant();
				string value = kvp.Value;

				switch (prop)
				{
					case "sprite number":
						spritenumber = int.Parse(value);
						if (definedsprites.ContainsKey(spritenumber))
							sprite = definedsprites[spritenumber];
						else
							General.ErrorLogger.Add(ErrorType.Error, "Dehacked frame " + number + " is referencing sprite " + spritenumber + " that is not defined.");
						break;
					case "sprite subnumber":
						spritesubnumber = long.Parse(value);
						if (spritesubnumber >= 32768)
						{
							spritesubnumber -= 32768;
							bright = true;
						}
						break;
				}
			}
		}

		#endregion
	}
}
