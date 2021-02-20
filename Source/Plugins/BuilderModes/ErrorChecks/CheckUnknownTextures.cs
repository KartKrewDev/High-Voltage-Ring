
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

using CodeImp.DoomBuilder.Config;
using CodeImp.DoomBuilder.Map;
using System.Threading;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	[ErrorChecker("Check unknown textures", true, 60)]
	public class CheckUnknownTextures : ErrorChecker
	{
		#region ================== Constants

		private const int PROGRESS_STEP = 1000;

		#endregion

		#region ================== Constructor / Destructor

		// Constructor
		public CheckUnknownTextures()
		{
			// Total progress is done when all lines are checked
			SetTotalProgress(General.Map.Map.Sidedefs.Count / PROGRESS_STEP);
		}

		#endregion

		#region ================== Methods

		// This runs the check
		public override void Run()
		{
			int progress = 0;
			int stepprogress = 0;

			// Go for all the sidedefs
			foreach(Sidedef sd in General.Map.Map.Sidedefs)
			{
				bool ignoreuppertexture = false;
				bool ignoremiddletexture = false;
				bool ignorelowertexture = false;

				// Some actions, like transfer heights, use special non-existing texture names for effects. Allow those to be ignored
				if(sd.Line.Action != 0)
				{
					LinedefActionInfo info = General.Map.Config.GetLinedefActionInfo(sd.Line.Action);
					ignoreuppertexture = info.ErrorCheckerExemptions.IgnoreUpperTexture;
					ignoremiddletexture = info.ErrorCheckerExemptions.IgnoreMiddleTexture;
					ignorelowertexture = info.ErrorCheckerExemptions.IgnoreLowerTexture;
				}

				// Check upper texture
				if(!ignoreuppertexture && sd.LongHighTexture != MapSet.EmptyLongName && !General.Map.Data.GetTextureExists(sd.LongHighTexture))
				{
					SubmitResult(new ResultUnknownTexture(sd, SidedefPart.Upper));
				}

				// Check middle texture
				if(!ignoremiddletexture && sd.LongMiddleTexture != MapSet.EmptyLongName && !General.Map.Data.GetTextureExists(sd.LongMiddleTexture))
				{
					SubmitResult(new ResultUnknownTexture(sd, SidedefPart.Middle));
				}

				// Check lower texture
				if(!ignorelowertexture && sd.LongLowTexture != MapSet.EmptyLongName && !General.Map.Data.GetTextureExists(sd.LongLowTexture))
				{
					SubmitResult(new ResultUnknownTexture(sd, SidedefPart.Lower));
				}
				
				// Handle thread interruption
				try { Thread.Sleep(0); }
				catch(ThreadInterruptedException) { return; }

				// We are making progress!
				if((++progress / PROGRESS_STEP) > stepprogress)
				{
					stepprogress = (progress / PROGRESS_STEP);
					AddProgress(1);
				}
			}
		}

		#endregion
	}
}
