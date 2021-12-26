using System;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Config;

namespace CodeImp.DoomBuilder.Controls.Scripting
{
	internal class ScriptIconsManager
	{
		private int scripttypeiconsoffset;
		private int scriptgroupiconsoffset;
		private int scriptgroupopeniconsoffset;
		private ImageList icons;

		internal int ScriptTypeIconsOffset { get { return scripttypeiconsoffset; } }
		internal int ScriptGroupIconsOffset { get { return scriptgroupiconsoffset; } }
		internal int ScriptGroupOpenIconsOffset { get { return scriptgroupopeniconsoffset; } }
		public ImageList Icons { get { return icons; } }

		public ScriptIconsManager(ImageList icons)
		{
			this.icons = icons;

			int numicons = Enum.GetNames(typeof(ScriptType)).Length;
			scriptgroupopeniconsoffset = icons.Images.Count - numicons;
			scriptgroupiconsoffset = scriptgroupopeniconsoffset - numicons;
			scripttypeiconsoffset = scriptgroupiconsoffset - numicons;

			System.Drawing.Bitmap i = new System.Drawing.Bitmap(icons.Images[28]);
			i.Save("c:\\users\\boris\\desktop\\group1.bmp");
			i.Dispose();

			i = new System.Drawing.Bitmap(icons.Images[51]);
			i.Save("c:\\users\\boris\\desktop\\group2.bmp");
			i.Dispose();
		}

		public int GetResourceIcon(int datalocationtype)
		{
			return datalocationtype;
		}

		public int GetScriptIcon(ScriptType type)
		{
			int scripttype = (int)type + scripttypeiconsoffset;
			if(scripttype >= scriptgroupiconsoffset) scripttype = scripttypeiconsoffset;
			return scripttype;
		}

		public int GetScriptFolderIcon(ScriptType type, bool opened)
		{
			int scripttype = (int)type;
			if(scripttype >= scriptgroupiconsoffset - scripttypeiconsoffset)
				scripttype = scripttypeiconsoffset;

			if(opened) return scriptgroupopeniconsoffset + scripttype;
			return scriptgroupiconsoffset + scripttype;
		}
	}
}
