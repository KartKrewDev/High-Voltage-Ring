namespace CodeImp.DoomBuilder.BuilderModes.Interface
{
	partial class SlopeArchForm
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
			this.theta = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.label1 = new System.Windows.Forms.Label();
			this.offset = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.label2 = new System.Windows.Forms.Label();
			this.cancel = new System.Windows.Forms.Button();
			this.accept = new System.Windows.Forms.Button();
			this.up = new System.Windows.Forms.RadioButton();
			this.down = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// theta
			// 
			this.theta.AllowDecimal = true;
			this.theta.AllowExpressions = false;
			this.theta.AllowNegative = false;
			this.theta.AllowRelative = false;
			this.theta.ButtonStep = 5;
			this.theta.ButtonStepBig = 15F;
			this.theta.ButtonStepFloat = 5F;
			this.theta.ButtonStepSmall = 15F;
			this.theta.ButtonStepsUseModifierKeys = true;
			this.theta.ButtonStepsWrapAround = false;
			this.theta.Location = new System.Drawing.Point(104, 7);
			this.theta.Name = "theta";
			this.theta.Size = new System.Drawing.Size(63, 24);
			this.theta.StepValues = null;
			this.theta.TabIndex = 18;
			this.theta.WhenTextChanged += new System.EventHandler(this.UpdateArch);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(19, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(31, 13);
			this.label1.TabIndex = 19;
			this.label1.Text = "theta";
			// 
			// offset
			// 
			this.offset.AllowDecimal = true;
			this.offset.AllowExpressions = false;
			this.offset.AllowNegative = false;
			this.offset.AllowRelative = false;
			this.offset.ButtonStep = 5;
			this.offset.ButtonStepBig = 15F;
			this.offset.ButtonStepFloat = 5F;
			this.offset.ButtonStepSmall = 15F;
			this.offset.ButtonStepsUseModifierKeys = true;
			this.offset.ButtonStepsWrapAround = false;
			this.offset.Location = new System.Drawing.Point(104, 37);
			this.offset.Name = "offset";
			this.offset.Size = new System.Drawing.Size(63, 24);
			this.offset.StepValues = null;
			this.offset.TabIndex = 20;
			this.offset.WhenTextChanged += new System.EventHandler(this.UpdateArch);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(22, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 21;
			this.label2.Text = "offset";
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(104, 197);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(88, 24);
			this.cancel.TabIndex = 22;
			this.cancel.Text = "Cancel";
			this.cancel.UseVisualStyleBackColor = true;
			// 
			// accept
			// 
			this.accept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.accept.Location = new System.Drawing.Point(10, 197);
			this.accept.Name = "accept";
			this.accept.Size = new System.Drawing.Size(88, 24);
			this.accept.TabIndex = 23;
			this.accept.Text = "Apply";
			this.accept.UseVisualStyleBackColor = true;
			// 
			// up
			// 
			this.up.AutoSize = true;
			this.up.Checked = true;
			this.up.Location = new System.Drawing.Point(22, 96);
			this.up.Name = "up";
			this.up.Size = new System.Drawing.Size(39, 17);
			this.up.TabIndex = 24;
			this.up.TabStop = true;
			this.up.Text = "Up";
			this.up.UseVisualStyleBackColor = true;
			this.up.CheckedChanged += new System.EventHandler(this.UpdateArch);
			// 
			// down
			// 
			this.down.AutoSize = true;
			this.down.Location = new System.Drawing.Point(22, 119);
			this.down.Name = "down";
			this.down.Size = new System.Drawing.Size(53, 17);
			this.down.TabIndex = 25;
			this.down.Text = "Down";
			this.down.UseVisualStyleBackColor = true;
			this.down.CheckedChanged += new System.EventHandler(this.UpdateArch);
			// 
			// SlopeArchForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(205, 251);
			this.Controls.Add(this.down);
			this.Controls.Add(this.up);
			this.Controls.Add(this.accept);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.offset);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.theta);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SlopeArchForm";
			this.ShowIcon = false;
			this.Text = "SlopeArchForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Controls.ButtonsNumericTextbox theta;
		private System.Windows.Forms.Label label1;
		private Controls.ButtonsNumericTextbox offset;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button accept;
		private System.Windows.Forms.RadioButton up;
		private System.Windows.Forms.RadioButton down;
	}
}