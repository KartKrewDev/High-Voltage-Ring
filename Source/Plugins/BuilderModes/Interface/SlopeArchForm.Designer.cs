namespace CodeImp.DoomBuilder.BuilderModes
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
			this.scale = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.label3 = new System.Windows.Forms.Label();
			this.quartercircleleft = new System.Windows.Forms.Button();
			this.quartercircleright = new System.Windows.Forms.Button();
			this.halfcircle = new System.Windows.Forms.Button();
			this.heightoffset = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.label4 = new System.Windows.Forms.Label();
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
			this.theta.WhenTextChanged += new System.EventHandler(this.theta_WhenTextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(61, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 13);
			this.label1.TabIndex = 19;
			this.label1.Text = "Angle:";
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
			this.offset.WhenTextChanged += new System.EventHandler(this.offset_WhenTextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(32, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 13);
			this.label2.TabIndex = 21;
			this.label2.Text = "Angle offset:";
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
			this.up.Location = new System.Drawing.Point(22, 131);
			this.up.Name = "up";
			this.up.Size = new System.Drawing.Size(39, 17);
			this.up.TabIndex = 24;
			this.up.TabStop = true;
			this.up.Text = "Up";
			this.up.UseVisualStyleBackColor = true;
			this.up.CheckedChanged += new System.EventHandler(this.up_CheckedChanged);
			// 
			// down
			// 
			this.down.AutoSize = true;
			this.down.Location = new System.Drawing.Point(22, 154);
			this.down.Name = "down";
			this.down.Size = new System.Drawing.Size(53, 17);
			this.down.TabIndex = 25;
			this.down.Text = "Down";
			this.down.UseVisualStyleBackColor = true;
			this.down.CheckedChanged += new System.EventHandler(this.down_CheckedChanged);
			// 
			// scale
			// 
			this.scale.AllowDecimal = true;
			this.scale.AllowExpressions = false;
			this.scale.AllowNegative = false;
			this.scale.AllowRelative = false;
			this.scale.ButtonStep = 5;
			this.scale.ButtonStepBig = 15F;
			this.scale.ButtonStepFloat = 5F;
			this.scale.ButtonStepSmall = 15F;
			this.scale.ButtonStepsUseModifierKeys = true;
			this.scale.ButtonStepsWrapAround = false;
			this.scale.Location = new System.Drawing.Point(104, 67);
			this.scale.Name = "scale";
			this.scale.Size = new System.Drawing.Size(63, 24);
			this.scale.StepValues = null;
			this.scale.TabIndex = 26;
			this.scale.WhenTextChanged += new System.EventHandler(this.scale_WhenTextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(50, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 13);
			this.label3.TabIndex = 27;
			this.label3.Text = "Scale %:";
			// 
			// quartercircleleft
			// 
			this.quartercircleleft.Location = new System.Drawing.Point(173, 12);
			this.quartercircleleft.Name = "quartercircleleft";
			this.quartercircleleft.Size = new System.Drawing.Size(113, 23);
			this.quartercircleleft.TabIndex = 28;
			this.quartercircleleft.Text = "Quarter circle left";
			this.quartercircleleft.UseVisualStyleBackColor = true;
			this.quartercircleleft.Click += new System.EventHandler(this.quartercircleleft_Click);
			// 
			// quartercircleright
			// 
			this.quartercircleright.Location = new System.Drawing.Point(173, 67);
			this.quartercircleright.Name = "quartercircleright";
			this.quartercircleright.Size = new System.Drawing.Size(113, 23);
			this.quartercircleright.TabIndex = 29;
			this.quartercircleright.Text = "Quarter circle right";
			this.quartercircleright.UseVisualStyleBackColor = true;
			this.quartercircleright.Click += new System.EventHandler(this.quartercircleright_Click);
			// 
			// halfcircle
			// 
			this.halfcircle.Location = new System.Drawing.Point(173, 38);
			this.halfcircle.Name = "halfcircle";
			this.halfcircle.Size = new System.Drawing.Size(113, 23);
			this.halfcircle.TabIndex = 30;
			this.halfcircle.Text = "Half circle";
			this.halfcircle.UseVisualStyleBackColor = true;
			this.halfcircle.Click += new System.EventHandler(this.halfcircle_Click);
			// 
			// heightoffset
			// 
			this.heightoffset.AllowDecimal = true;
			this.heightoffset.AllowExpressions = false;
			this.heightoffset.AllowNegative = true;
			this.heightoffset.AllowRelative = false;
			this.heightoffset.ButtonStep = 8;
			this.heightoffset.ButtonStepBig = 16F;
			this.heightoffset.ButtonStepFloat = 8F;
			this.heightoffset.ButtonStepSmall = 16F;
			this.heightoffset.ButtonStepsUseModifierKeys = true;
			this.heightoffset.ButtonStepsWrapAround = false;
			this.heightoffset.Location = new System.Drawing.Point(104, 97);
			this.heightoffset.Name = "heightoffset";
			this.heightoffset.Size = new System.Drawing.Size(63, 24);
			this.heightoffset.StepValues = null;
			this.heightoffset.TabIndex = 31;
			this.heightoffset.WhenTextChanged += new System.EventHandler(this.heightoffset_WhenTextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(29, 102);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 13);
			this.label4.TabIndex = 32;
			this.label4.Text = "Height offset:";
			// 
			// SlopeArchForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(298, 230);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.heightoffset);
			this.Controls.Add(this.halfcircle);
			this.Controls.Add(this.quartercircleright);
			this.Controls.Add(this.quartercircleleft);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.scale);
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
			this.Shown += new System.EventHandler(this.SlopeArchForm_Shown);
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
		private Controls.ButtonsNumericTextbox scale;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button quartercircleleft;
		private System.Windows.Forms.Button quartercircleright;
		private System.Windows.Forms.Button halfcircle;
		private Controls.ButtonsNumericTextbox heightoffset;
		private System.Windows.Forms.Label label4;
	}
}