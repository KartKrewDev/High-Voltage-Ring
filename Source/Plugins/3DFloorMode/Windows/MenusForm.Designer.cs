namespace CodeImp.DoomBuilder.ThreeDFloorMode
{
	partial class MenusForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenusForm));
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.floorslope = new System.Windows.Forms.ToolStripButton();
			this.ceilingslope = new System.Windows.Forms.ToolStripButton();
			this.floorandceilingslope = new System.Windows.Forms.ToolStripButton();
			this.updateslopes = new System.Windows.Forms.ToolStripButton();
			this.addsectorscontextmenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addslopeceiling = new System.Windows.Forms.ToolStripMenuItem();
			this.addslopefloor = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.removeslopeceiling = new System.Windows.Forms.ToolStripMenuItem();
			this.removeslopefloor = new System.Windows.Forms.ToolStripMenuItem();
			this.relocatecontrolsectors = new System.Windows.Forms.ToolStripButton();
			this.toolStrip1.SuspendLayout();
			this.addsectorscontextmenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.floorslope,
            this.ceilingslope,
            this.floorandceilingslope,
            this.updateslopes,
            this.relocatecontrolsectors});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(442, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// floorslope
			// 
			this.floorslope.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.floorslope.Image = global::ThreeDFloorMode.Properties.Resources.Floor;
			this.floorslope.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.floorslope.Name = "floorslope";
			this.floorslope.Size = new System.Drawing.Size(23, 22);
			this.floorslope.Tag = "drawfloorslope";
			this.floorslope.Text = "Apply drawn slope to floor";
			this.floorslope.Click += new System.EventHandler(this.floorslope_Click);
			// 
			// ceilingslope
			// 
			this.ceilingslope.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ceilingslope.Image = global::ThreeDFloorMode.Properties.Resources.Ceiling;
			this.ceilingslope.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ceilingslope.Name = "ceilingslope";
			this.ceilingslope.Size = new System.Drawing.Size(23, 22);
			this.ceilingslope.Tag = "drawceilingslope";
			this.ceilingslope.Text = "Apply drawn slope to ceiling";
			this.ceilingslope.Click += new System.EventHandler(this.ceilingslope_Click);
			// 
			// floorandceilingslope
			// 
			this.floorandceilingslope.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.floorandceilingslope.Image = global::ThreeDFloorMode.Properties.Resources.FloorAndCeiling;
			this.floorandceilingslope.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.floorandceilingslope.Name = "floorandceilingslope";
			this.floorandceilingslope.Size = new System.Drawing.Size(23, 22);
			this.floorandceilingslope.Tag = "drawfloorandceilingslope";
			this.floorandceilingslope.Text = "Apply drawn slope to floor and ceiling";
			this.floorandceilingslope.Click += new System.EventHandler(this.floorandceilingslope_Click);
			// 
			// updateslopes
			// 
			this.updateslopes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.updateslopes.Image = ((System.Drawing.Image)(resources.GetObject("updateslopes.Image")));
			this.updateslopes.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.updateslopes.Name = "updateslopes";
			this.updateslopes.Size = new System.Drawing.Size(85, 22);
			this.updateslopes.Text = "Update slopes";
			this.updateslopes.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// addsectorscontextmenu
			// 
			this.addsectorscontextmenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addslopeceiling,
            this.addslopefloor,
            this.toolStripSeparator1,
            this.removeslopeceiling,
            this.removeslopefloor});
			this.addsectorscontextmenu.Name = "addsectorscontextmenu";
			this.addsectorscontextmenu.Size = new System.Drawing.Size(216, 98);
			this.addsectorscontextmenu.Opening += new System.ComponentModel.CancelEventHandler(this.addsectorscontextmenu_Opening);
			this.addsectorscontextmenu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.addsectorscontextmenu_Closing);
			// 
			// addslopeceiling
			// 
			this.addslopeceiling.Name = "addslopeceiling";
			this.addslopeceiling.Size = new System.Drawing.Size(215, 22);
			this.addslopeceiling.Text = "Add slope to ceiling";
			this.addslopeceiling.Click += new System.EventHandler(this.ceilingToolStripMenuItem_Click);
			// 
			// addslopefloor
			// 
			this.addslopefloor.Name = "addslopefloor";
			this.addslopefloor.Size = new System.Drawing.Size(215, 22);
			this.addslopefloor.Text = "Add slope to floor";
			this.addslopefloor.Click += new System.EventHandler(this.floorToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(212, 6);
			// 
			// removeslopeceiling
			// 
			this.removeslopeceiling.Name = "removeslopeceiling";
			this.removeslopeceiling.Size = new System.Drawing.Size(215, 22);
			this.removeslopeceiling.Text = "Remove slope from ceiling";
			this.removeslopeceiling.Click += new System.EventHandler(this.removeSlopeFromCeilingToolStripMenuItem_Click);
			// 
			// removeslopefloor
			// 
			this.removeslopefloor.Name = "removeslopefloor";
			this.removeslopefloor.Size = new System.Drawing.Size(215, 22);
			this.removeslopefloor.Text = "Remove slope from floor";
			this.removeslopefloor.Click += new System.EventHandler(this.removeSlopeFromFloorToolStripMenuItem_Click);
			// 
			// relocatecontrolsectors
			// 
			this.relocatecontrolsectors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.relocatecontrolsectors.Image = ((System.Drawing.Image)(resources.GetObject("relocatecontrolsectors.Image")));
			this.relocatecontrolsectors.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.relocatecontrolsectors.Name = "relocatecontrolsectors";
			this.relocatecontrolsectors.Size = new System.Drawing.Size(137, 22);
			this.relocatecontrolsectors.Tag = "relocate3dfloorcontrolsectors";
			this.relocatecontrolsectors.Text = "Relocate control sectors";
			this.relocatecontrolsectors.Click += new System.EventHandler(this.relocatecontrolsectors_Click);
			// 
			// MenusForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(442, 262);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MenusForm";
			this.Text = "MenusForm";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.addsectorscontextmenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton floorslope;
		private System.Windows.Forms.ToolStripButton ceilingslope;
		private System.Windows.Forms.ToolStripButton floorandceilingslope;
		private System.Windows.Forms.ToolStripButton updateslopes;
		private System.Windows.Forms.ContextMenuStrip addsectorscontextmenu;
		private System.Windows.Forms.ToolStripMenuItem addslopefloor;
		private System.Windows.Forms.ToolStripMenuItem addslopeceiling;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem removeslopeceiling;
		private System.Windows.Forms.ToolStripMenuItem removeslopefloor;
		private System.Windows.Forms.ToolStripButton relocatecontrolsectors;
	}
}