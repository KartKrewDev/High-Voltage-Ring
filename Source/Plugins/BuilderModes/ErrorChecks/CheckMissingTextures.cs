
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

using CodeImp.DoomBuilder.Map;
using System.Collections.Generic;
using System.Threading;
using System;

#endregion

namespace CodeImp.DoomBuilder.BuilderModes
{
	[ErrorChecker("Check missing textures", true, 80)]
	public class CheckMissingTextures : ErrorChecker
	{
		#region ================== Constants

		private const int PROGRESS_STEP = 1000;

		#endregion

		#region ================== Constructor / Destructor

		// Constructor
		public CheckMissingTextures()
		{
			// Total progress is done when all lines are checked
			SetTotalProgress(General.Map.Map.Sidedefs.Count / PROGRESS_STEP);
		}

		#endregion

		#region ================== Enum

		[Flags]
		private enum Flags3DFloor
		{
			UseUpper = 1,
			UseLower = 2,
			RenderInside = 4
		}

		#endregion

		#region ================== Methods

		// This runs the check
		public override void Run()
		{
			int progress = 0;
			int stepprogress = 0;

			Dictionary<int, Flags3DFloor> sector3dfloors = new Dictionary<int, Flags3DFloor>();

			// Create a cache of sectors that have 3D floors, with their flags relevant to the error checker
			foreach (Linedef ld in General.Map.Map.Linedefs)
			{
				if (ld.Action == 160)
				{
					if ((ld.Args[1] & 4) == 4) // Type render inside
					{
						if (!sector3dfloors.ContainsKey(ld.Args[0]))
							sector3dfloors.Add(ld.Args[0], Flags3DFloor.RenderInside);
					}

					if ((ld.Args[2] & 16) == 16) // Flag use upper
					{
						if (!sector3dfloors.ContainsKey(ld.Args[0]))
							sector3dfloors.Add(ld.Args[0], Flags3DFloor.UseUpper);
						else
							sector3dfloors[ld.Args[0]] |= Flags3DFloor.UseUpper;
					}

					if ((ld.Args[2] & 32) == 32) // Flag use lower
					{
						if (!sector3dfloors.ContainsKey(ld.Args[0]))
							sector3dfloors.Add(ld.Args[0], Flags3DFloor.UseLower);
						else
							sector3dfloors[ld.Args[0]] |= Flags3DFloor.UseLower;
					}
				}
			}

			// Go for all the sidedefs
			foreach(Sidedef sd in General.Map.Map.Sidedefs)
			{
				// Check upper texture. Also make sure not to return a false
				// positive if the sector on the other side has the ceiling
				// set to be sky
				if (sd.HighTexture == "-") {
					if (sd.HighRequired())
					{
						if (sd.Line.Action == 181 && sd.Line.Args[1] > 0) continue; //mxd. Ceiling slopes doesn't require upper texture
						if (sd.Other != null && sd.Other.Sector.CeilTexture != General.Map.Config.SkyFlatName)
						{
							SubmitResult(new ResultMissingTexture(sd, SidedefPart.Upper));
						}
					}
					else if (sd.Other != null)
					{
						// Check if the sidedef's sector is a 3D floor. Since it points toward the 3D floor it only needs a texture if inside rendering is enabled
						if (sd.Sector.Tags.Count > 0)
						{
							foreach (int tag in sd.Sector.Tags)
							{
								if (sector3dfloors.ContainsKey(tag) && sector3dfloors[tag].HasFlag(Flags3DFloor.UseUpper) && sector3dfloors[tag].HasFlag(Flags3DFloor.RenderInside))
								{
									SubmitResult(new ResultMissingTexture(sd, SidedefPart.Upper));
									break;
								}
							}
						}

						// Check if the other sidedef's sector is a 3D floor, since we still might need a texture on this one depending on the flags
						if (sd.Other.Sector.Tags.Count > 0)
						{
							foreach (int tag in sd.Other.Sector.Tags)
							{
								if (sector3dfloors.ContainsKey(tag) && sector3dfloors[tag].HasFlag(Flags3DFloor.UseUpper))
								{
									SubmitResult(new ResultMissingTexture(sd, SidedefPart.Upper));
									break;
								}
							}
						}
					}
				}

				// Check middle texture
				if(sd.MiddleRequired() && sd.MiddleTexture == "-")
				{
					SubmitResult(new ResultMissingTexture(sd, SidedefPart.Middle));
				}

				// Check lower texture. Also make sure not to return a false
				// positive if the sector on the other side has the floor
				// set to be sky
				if(sd.LowTexture == "-")
				{
					if (sd.LowRequired())
					{
						if (sd.Line.Action == 181 && sd.Line.Args[0] > 0) continue; //mxd. Floor slopes doesn't require lower texture
						if (sd.Other != null && sd.Other.Sector.FloorTexture != General.Map.Config.SkyFlatName)
						{
							SubmitResult(new ResultMissingTexture(sd, SidedefPart.Lower));
						}
					}
					else if (sd.Other != null)
					{
						// Check if the sidedef's sector is a 3D floor. Since it points toward the 3D floor it only needs a texture if inside rendering is enabled
						if (sd.Sector.Tags.Count > 0)
						{
							foreach (int tag in sd.Sector.Tags)
							{
								if (sector3dfloors.ContainsKey(tag) && sector3dfloors[tag].HasFlag(Flags3DFloor.UseLower) && sector3dfloors[tag].HasFlag(Flags3DFloor.RenderInside))
								{
									SubmitResult(new ResultMissingTexture(sd, SidedefPart.Lower));
									break;
								}
							}
						}

						// Check if the other sidedef's sector is a 3D floor, since we still might need a texture on this one depending on the flags
						if (sd.Other.Sector.Tags.Count > 0)
						{
							foreach (int tag in sd.Other.Sector.Tags)
							{
								if (sector3dfloors.ContainsKey(tag) && sector3dfloors[tag].HasFlag(Flags3DFloor.UseLower))
								{
									SubmitResult(new ResultMissingTexture(sd, SidedefPart.Lower));
									break;
								}
							}
						}
					}

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
