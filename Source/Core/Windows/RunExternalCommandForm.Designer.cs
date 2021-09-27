namespace CodeImp.DoomBuilder.Windows
{
	partial class RunExternalCommandForm
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
			this.btnContinue = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.rtbOutput = new System.Windows.Forms.RichTextBox();
			this.btnRetry = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// btnContinue
			// 
			this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnContinue.Enabled = false;
			this.btnContinue.Location = new System.Drawing.Point(596, 459);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(75, 23);
			this.btnContinue.TabIndex = 1;
			this.btnContinue.Text = "Continue";
			this.toolTip1.SetToolTip(this.btnContinue, "Continues loading the resources");
			this.btnContinue.UseVisualStyleBackColor = true;
			this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(677, 459);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.toolTip1.SetToolTip(this.btnCancel, "Cancels the running command, or cancels loading the resources");
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// rtbOutput
			// 
			this.rtbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbOutput.Location = new System.Drawing.Point(12, 12);
			this.rtbOutput.Name = "rtbOutput";
			this.rtbOutput.ReadOnly = true;
			this.rtbOutput.Size = new System.Drawing.Size(740, 441);
			this.rtbOutput.TabIndex = 2;
			this.rtbOutput.Text = "";
			// 
			// btnRetry
			// 
			this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRetry.Enabled = false;
			this.btnRetry.Location = new System.Drawing.Point(515, 459);
			this.btnRetry.Name = "btnRetry";
			this.btnRetry.Size = new System.Drawing.Size(75, 23);
			this.btnRetry.TabIndex = 1;
			this.btnRetry.Text = "Run again";
			this.toolTip1.SetToolTip(this.btnRetry, "Runs the command again");
			this.btnRetry.UseVisualStyleBackColor = true;
			this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
			// 
			// RunExternalCommandForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(766, 494);
			this.Controls.Add(this.rtbOutput);
			this.Controls.Add(this.btnRetry);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.btnCancel);
			this.Name = "RunExternalCommandForm";
			this.ShowIcon = false;
			this.Text = "Running external command";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RunExternalCommandForm_FormClosing);
			this.Shown += new System.EventHandler(this.RunExternalCommandForm_Shown);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.RichTextBox rtbOutput;
		private System.Windows.Forms.Button btnRetry;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}