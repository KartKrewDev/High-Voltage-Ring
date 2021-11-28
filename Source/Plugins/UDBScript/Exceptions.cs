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

#region ================== Namespaces

using System;

#endregion

namespace CodeImp.DoomBuilder.UDBScript
{
	[Serializable]
	public class UserScriptAbortException : Exception
	{
		public UserScriptAbortException()
		{
		}

		public UserScriptAbortException(string message) : base(message)
		{
		}
	}

	[Serializable]
	public class CantConvertToVectorException : Exception
	{
		public CantConvertToVectorException()
		{
		}

		public CantConvertToVectorException(string message) : base(message)
		{
		}
	}

	[Serializable]
	public class ExitScriptException : Exception
	{
		public ExitScriptException()
		{
		}

		public ExitScriptException(string message) : base(message)
		{
		}
	}

	[Serializable]
	public class DieScriptException : Exception
	{
		public DieScriptException()
		{
		}

		public DieScriptException(string message) : base(message)
		{
		}
	}
}
