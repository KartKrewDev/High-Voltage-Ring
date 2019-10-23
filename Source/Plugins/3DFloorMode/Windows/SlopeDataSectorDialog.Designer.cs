namespace CodeImp.DoomBuilder.ThreeDFloorMode
{
	partial class SlopeDataSectorDialog
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
			this.useselectedsector = new System.Windows.Forms.Button();
			this.createnewsector = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// useselectedsector
			// 
			this.useselectedsector.Location = new System.Drawing.Point(147, 238);
			this.useselectedsector.Name = "useselectedsector";
			this.useselectedsector.Size = new System.Drawing.Size(129, 23);
			this.useselectedsector.TabIndex = 2;
			this.useselectedsector.Text = "Use selected sector";
			this.useselectedsector.UseVisualStyleBackColor = true;
			this.useselectedsector.Click += new System.EventHandler(this.button1_Click);
			// 
			// createnewsector
			// 
			this.createnewsector.Location = new System.Drawing.Point(12, 238);
			this.createnewsector.Name = "createnewsector";
			this.createnewsector.Size = new System.Drawing.Size(129, 23);
			this.createnewsector.TabIndex = 1;
			this.createnewsector.Text = "Create sector in CSA";
			this.createnewsector.UseVisualStyleBackColor = true;
			this.createnewsector.Click += new System.EventHandler(this.createnewsector_Click);
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(282, 238);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(129, 23);
			this.cancel.TabIndex = 3;
			this.cancel.Text = "Cancel";
			this.cancel.UseVisualStyleBackColor = true;
			// 
			// webBrowser1
			// 
			this.webBrowser1.Location = new System.Drawing.Point(12, 12);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.ScrollBarsEnabled = false;
			this.webBrowser1.Size = new System.Drawing.Size(399, 220);
			this.webBrowser1.TabIndex = 4;
			// 
			// SlopeDataSectorDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(423, 273);
			this.Controls.Add(this.webBrowser1);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.createnewsector);
			this.Controls.Add(this.useselectedsector);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SlopeDataSectorDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Slope data sector";
			this.Load += new System.EventHandler(this.SlopeDataSectorDialog_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button useselectedsector;
		private System.Windows.Forms.Button createnewsector;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.WebBrowser webBrowser1;
	}
}