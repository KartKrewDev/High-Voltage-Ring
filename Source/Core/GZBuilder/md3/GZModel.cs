using System.Collections.Generic;
using CodeImp.DoomBuilder.Rendering;
using CodeImp.DoomBuilder.GZBuilder.Data;

namespace CodeImp.DoomBuilder.GZBuilder.MD3
{
	internal class GZModel 
	{
		internal readonly List<Mesh> Meshes;
		internal readonly List<Texture> Textures;
		internal float Radius;
        internal BoundingBoxSizes BBox;

        internal GZModel() 
		{
			Meshes = new List<Mesh>();
			Textures = new List<Texture>();
		}
	}
}