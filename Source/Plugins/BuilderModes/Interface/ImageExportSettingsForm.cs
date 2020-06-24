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

using System;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeImp.DoomBuilder.BuilderModes.Interface
{
	public partial class ImageExportSettingsForm : Form
	{
		#region ================== Properties

		public string FilePath { get { return tbExportPath.Text.Trim(); } }
		public bool Floor { get { return rbFloor.Checked; } }
		public bool Fullbright { get { return cbFullbright.Checked; } }
		public bool Brightmap { get { return cbBrightmap.Checked; } }
		public bool Tiles { get { return cbTiles.Checked; } }

		#endregion


		#region ================== Constructor

		public ImageExportSettingsForm()
		{
			InitializeComponent();

			cbImageFormat.SelectedIndex = 0;
			cbPixelFormat.SelectedIndex = 0;

			string name = Path.GetFileNameWithoutExtension(General.Map.FileTitle) + "_" + General.Map.Options.LevelName + "_" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

			if (string.IsNullOrEmpty(General.Map.FilePathName))
			{
				saveFileDialog.FileName = name;
			}
			else
			{
				saveFileDialog.InitialDirectory = Path.GetDirectoryName(General.Map.FilePathName);
				saveFileDialog.FileName = Path.GetDirectoryName(General.Map.FilePathName) + Path.DirectorySeparatorChar + name + ".png";
				tbExportPath.Text = saveFileDialog.FileName;
			}

			cbFullbright.Checked = General.Settings.ReadPluginSetting("imageexportfullbright", true);
			cbBrightmap.Checked = General.Settings.ReadPluginSetting("imageexportbrightmap", false);
			cbTiles.Checked = General.Settings.ReadPluginSetting("imageexporttiles", false);
		}

		#endregion

		#region ================== Methods

		public ImageFormat GetImageFormat()
		{
			switch(cbImageFormat.SelectedIndex)
			{
				case 1: // JPG
					return ImageFormat.Jpeg;
				default: // PNG
					return ImageFormat.Png;
			}
		}

		public PixelFormat GetPixelFormat()
		{
			switch(cbPixelFormat.SelectedIndex)
			{
				case 1: // 24 bit
					return PixelFormat.Format24bppRgb;
				case 2: // 16 bit
					return PixelFormat.Format16bppRgb555;
				default: // 32 bit
					return PixelFormat.Format32bppArgb;
			}
		}

		private void browse_Click(object sender, EventArgs e)
		{
			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				tbExportPath.Text = saveFileDialog.FileName;

				string extension = Path.GetExtension(saveFileDialog.FileName);
				
				switch(extension)
				{
					case ".jpg":
						cbImageFormat.SelectedIndex = 1;
						break;
					default:
						cbImageFormat.SelectedIndex = 0;
						break;
				}
			}
		}

		#endregion

		private void cancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void export_Click(object sender, EventArgs e)
		{
			 General.Settings.WritePluginSetting("imageexportfullbright", cbFullbright.Checked);
			 General.Settings.WritePluginSetting("imageexportbrightmap", cbBrightmap.Checked);
			 General.Settings.WritePluginSetting("imageexporttiles", cbTiles.Checked);

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cbImageFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			string newextension = "";

			switch (cbImageFormat.SelectedIndex)
			{
				case 1: // JPG
					newextension = ".jpg";
					break;
				default: // PNG
					newextension = ".png";
					break;
			}

			tbExportPath.Text = Path.ChangeExtension(tbExportPath.Text, newextension);
		}
	}
}
