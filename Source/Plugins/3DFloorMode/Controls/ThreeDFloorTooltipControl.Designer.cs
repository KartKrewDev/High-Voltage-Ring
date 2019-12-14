namespace CodeImp.DoomBuilder.ThreeDFloorMode
{
	partial class ThreeDFloorHelperTooltipElementControl
	{
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.sectorTopFlat = new System.Windows.Forms.Panel();
			this.sectorBorderTexture = new System.Windows.Forms.Panel();
			this.sectorBottomFlat = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.bottomHeight = new System.Windows.Forms.Label();
			this.topHeight = new System.Windows.Forms.Label();
			this.borderHeight = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// sectorTopFlat
			// 
			this.sectorTopFlat.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.sectorTopFlat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.sectorTopFlat.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.sectorTopFlat.Location = new System.Drawing.Point(12, 16);
			this.sectorTopFlat.Name = "sectorTopFlat";
			this.sectorTopFlat.Size = new System.Drawing.Size(65, 65);
			this.sectorTopFlat.TabIndex = 22;
			// 
			// sectorBorderTexture
			// 
			this.sectorBorderTexture.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.sectorBorderTexture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.sectorBorderTexture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.sectorBorderTexture.Location = new System.Drawing.Point(83, 16);
			this.sectorBorderTexture.Name = "sectorBorderTexture";
			this.sectorBorderTexture.Size = new System.Drawing.Size(65, 65);
			this.sectorBorderTexture.TabIndex = 21;
			// 
			// sectorBottomFlat
			// 
			this.sectorBottomFlat.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.sectorBottomFlat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.sectorBottomFlat.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.sectorBottomFlat.Location = new System.Drawing.Point(154, 16);
			this.sectorBottomFlat.Name = "sectorBottomFlat";
			this.sectorBottomFlat.Size = new System.Drawing.Size(65, 65);
			this.sectorBottomFlat.TabIndex = 20;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(80, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 19;
			this.label3.Text = "Border";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(151, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 18;
			this.label2.Text = "Bottom";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(9, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(26, 13);
			this.label1.TabIndex = 17;
			this.label1.Text = "Top";
			// 
			// bottomHeight
			// 
			this.bottomHeight.BackColor = System.Drawing.Color.Transparent;
			this.bottomHeight.Location = new System.Drawing.Point(189, 0);
			this.bottomHeight.Name = "bottomHeight";
			this.bottomHeight.Size = new System.Drawing.Size(30, 13);
			this.bottomHeight.TabIndex = 23;
			this.bottomHeight.Text = "X";
			this.bottomHeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// topHeight
			// 
			this.topHeight.BackColor = System.Drawing.Color.Transparent;
			this.topHeight.Location = new System.Drawing.Point(47, 0);
			this.topHeight.Name = "topHeight";
			this.topHeight.Size = new System.Drawing.Size(30, 13);
			this.topHeight.TabIndex = 24;
			this.topHeight.Text = "X";
			this.topHeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// borderHeight
			// 
			this.borderHeight.BackColor = System.Drawing.Color.Transparent;
			this.borderHeight.Location = new System.Drawing.Point(114, 0);
			this.borderHeight.Name = "borderHeight";
			this.borderHeight.Size = new System.Drawing.Size(30, 13);
			this.borderHeight.TabIndex = 25;
			this.borderHeight.Text = "X";
			this.borderHeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ThreeDFloorHelperTooltipElementControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.borderHeight);
			this.Controls.Add(this.topHeight);
			this.Controls.Add(this.bottomHeight);
			this.Controls.Add(this.sectorTopFlat);
			this.Controls.Add(this.sectorBorderTexture);
			this.Controls.Add(this.sectorBottomFlat);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "ThreeDFloorHelperTooltipElementControl";
			this.Size = new System.Drawing.Size(222, 82);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ThreeDFloorHelperTooltipElementControl_Paint);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.Panel sectorBorderTexture;
		public System.Windows.Forms.Panel sectorTopFlat;
		public System.Windows.Forms.Panel sectorBottomFlat;
		public System.Windows.Forms.Label bottomHeight;
		public System.Windows.Forms.Label topHeight;
		public System.Windows.Forms.Label borderHeight;
	}
}
