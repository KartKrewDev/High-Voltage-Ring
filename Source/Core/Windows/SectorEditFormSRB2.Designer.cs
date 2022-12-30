namespace CodeImp.DoomBuilder.Windows
{
	partial class SectorEditFormSRB2
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
			if(disposing && (components != null)) 
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
			System.Windows.Forms.GroupBox groupaction;
			System.Windows.Forms.GroupBox groupeffect;
			System.Windows.Forms.Label labelTriggerer;
			System.Windows.Forms.Label labelTriggerTag;
			System.Windows.Forms.Label label16;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SectorEditFormSRB2));
			System.Windows.Forms.Label label9;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.GroupBox groupfloorceiling;
			System.Windows.Forms.Label label15;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label labelLightAlpha;
			System.Windows.Forms.Label labelFadeAlpha;
			System.Windows.Forms.Label labelFadeStart;
			System.Windows.Forms.Label labelFadeEnd;
			System.Windows.Forms.Label labelFriction;
			this.tagsselector = new CodeImp.DoomBuilder.Controls.TagsSelector();
			this.triggerer = new System.Windows.Forms.ComboBox();
			this.triggerTag = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.resetdamagetype = new System.Windows.Forms.Button();
			this.brightness = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.damagetype = new System.Windows.Forms.ComboBox();
			this.gravity = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.heightoffset = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.ceilingheight = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.sectorheightlabel = new System.Windows.Forms.Label();
			this.sectorheight = new System.Windows.Forms.Label();
			this.floorheight = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tabproperties = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.flags = new CodeImp.DoomBuilder.Controls.CheckboxArrayControl();
			this.tabColors = new System.Windows.Forms.TabPage();
			this.groupBox8 = new System.Windows.Forms.GroupBox();
			this.fadeEnd = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.fadeStart = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.fadeAlpha = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.lightAlpha = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.lightColor = new CodeImp.DoomBuilder.Controls.ColorFieldsControl();
			this.fadeColor = new CodeImp.DoomBuilder.Controls.ColorFieldsControl();
			this.tabSurfaces = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.resetfloorlight = new System.Windows.Forms.Button();
			this.labelFloorOffsets = new System.Windows.Forms.Label();
			this.labelFloorScale = new System.Windows.Forms.Label();
			this.cbUseFloorLineAngles = new System.Windows.Forms.CheckBox();
			this.floorAngleControl = new CodeImp.DoomBuilder.Controls.AngleControlEx();
			this.label11 = new System.Windows.Forms.Label();
			this.floorRotation = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.floorLightAbsolute = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.floorBrightness = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.floorScale = new CodeImp.DoomBuilder.Controls.PairedFieldsControl();
			this.floorOffsets = new CodeImp.DoomBuilder.Controls.PairedFieldsControl();
			this.floortex = new CodeImp.DoomBuilder.Controls.FlatSelectorControl();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.resetceillight = new System.Windows.Forms.Button();
			this.labelCeilOffsets = new System.Windows.Forms.Label();
			this.labelCeilScale = new System.Windows.Forms.Label();
			this.cbUseCeilLineAngles = new System.Windows.Forms.CheckBox();
			this.ceilAngleControl = new CodeImp.DoomBuilder.Controls.AngleControlEx();
			this.label1 = new System.Windows.Forms.Label();
			this.ceilRotation = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.ceilLightAbsolute = new System.Windows.Forms.CheckBox();
			this.labelLightFront = new System.Windows.Forms.Label();
			this.ceilBrightness = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			this.ceilScale = new CodeImp.DoomBuilder.Controls.PairedFieldsControl();
			this.ceilOffsets = new CodeImp.DoomBuilder.Controls.PairedFieldsControl();
			this.ceilingtex = new CodeImp.DoomBuilder.Controls.FlatSelectorControl();
			this.tabslopes = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.floorslopecontrol = new CodeImp.DoomBuilder.Controls.SectorSlopeControl();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.ceilingslopecontrol = new CodeImp.DoomBuilder.Controls.SectorSlopeControl();
			this.tabcomment = new System.Windows.Forms.TabPage();
			this.commenteditor = new CodeImp.DoomBuilder.Controls.CommentEditor();
			this.tabcustom = new System.Windows.Forms.TabPage();
			this.fieldslist = new CodeImp.DoomBuilder.Controls.FieldsEditorControl();
			this.cancel = new System.Windows.Forms.Button();
			this.apply = new System.Windows.Forms.Button();
			this.tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.friction = new CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox();
			groupaction = new System.Windows.Forms.GroupBox();
			groupeffect = new System.Windows.Forms.GroupBox();
			labelTriggerer = new System.Windows.Forms.Label();
			labelTriggerTag = new System.Windows.Forms.Label();
			label16 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			groupfloorceiling = new System.Windows.Forms.GroupBox();
			label15 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			labelLightAlpha = new System.Windows.Forms.Label();
			labelFadeAlpha = new System.Windows.Forms.Label();
			labelFadeStart = new System.Windows.Forms.Label();
			labelFadeEnd = new System.Windows.Forms.Label();
			labelFriction = new System.Windows.Forms.Label();
			groupaction.SuspendLayout();
			groupeffect.SuspendLayout();
			groupfloorceiling.SuspendLayout();
			this.tabs.SuspendLayout();
			this.tabproperties.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabColors.SuspendLayout();
			this.groupBox8.SuspendLayout();
			this.tabSurfaces.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabslopes.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabcomment.SuspendLayout();
			this.tabcustom.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupaction
			// 
			groupaction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			groupaction.Controls.Add(this.tagsselector);
			groupaction.Location = new System.Drawing.Point(7, 445);
			groupaction.Name = "groupaction";
			groupaction.Size = new System.Drawing.Size(557, 80);
			groupaction.TabIndex = 2;
			groupaction.TabStop = false;
			groupaction.Text = " Identification ";
			// 
			// tagsselector
			// 
			this.tagsselector.Location = new System.Drawing.Point(6, 15);
			this.tagsselector.Name = "tagsselector";
			this.tagsselector.Size = new System.Drawing.Size(545, 60);
			this.tagsselector.TabIndex = 0;
			// 
			// groupeffect
			// 
			groupeffect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			groupeffect.Controls.Add(this.friction);
			groupeffect.Controls.Add(labelFriction);
			groupeffect.Controls.Add(labelTriggerer);
			groupeffect.Controls.Add(this.triggerer);
			groupeffect.Controls.Add(labelTriggerTag);
			groupeffect.Controls.Add(this.triggerTag);
			groupeffect.Controls.Add(label16);
			groupeffect.Controls.Add(this.resetdamagetype);
			groupeffect.Controls.Add(this.brightness);
			groupeffect.Controls.Add(this.damagetype);
			groupeffect.Controls.Add(label9);
			groupeffect.Controls.Add(this.gravity);
			groupeffect.Controls.Add(label2);
			groupeffect.Location = new System.Drawing.Point(7, 330);
			groupeffect.Name = "groupeffect";
			groupeffect.Size = new System.Drawing.Size(557, 109);
			groupeffect.TabIndex = 1;
			groupeffect.TabStop = false;
			groupeffect.Text = " Effects ";
			// 
			// labelTriggerer
			// 
			labelTriggerer.Location = new System.Drawing.Point(218, 16);
			labelTriggerer.Name = "labelTriggerer";
			labelTriggerer.Size = new System.Drawing.Size(74, 14);
			labelTriggerer.TabIndex = 7;
			labelTriggerer.Text = "Triggerer:";
			labelTriggerer.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// triggerer
			// 
			this.triggerer.FormattingEnabled = true;
			this.triggerer.Location = new System.Drawing.Point(298, 12);
			this.triggerer.Name = "triggerer";
			this.triggerer.Size = new System.Drawing.Size(167, 21);
			this.triggerer.TabIndex = 8;
			// 
			// labelTriggerTag
			// 
			labelTriggerTag.Location = new System.Drawing.Point(9, 16);
			labelTriggerTag.Name = "labelTriggerTag";
			labelTriggerTag.Size = new System.Drawing.Size(74, 14);
			labelTriggerTag.TabIndex = 21;
			labelTriggerTag.Text = "Trigger tag:";
			labelTriggerTag.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// triggerTag
			// 
			this.triggerTag.AllowDecimal = false;
			this.triggerTag.AllowExpressions = false;
			this.triggerTag.AllowNegative = false;
			this.triggerTag.AllowRelative = false;
			this.triggerTag.ButtonStep = 1;
			this.triggerTag.ButtonStepBig = 1F;
			this.triggerTag.ButtonStepFloat = 1F;
			this.triggerTag.ButtonStepSmall = 1F;
			this.triggerTag.ButtonStepsUseModifierKeys = true;
			this.triggerTag.ButtonStepsWrapAround = false;
			this.triggerTag.Location = new System.Drawing.Point(89, 11);
			this.triggerTag.Name = "triggerTag";
			this.triggerTag.Size = new System.Drawing.Size(81, 24);
			this.triggerTag.StepValues = null;
			this.triggerTag.TabIndex = 20;
			// 
			// label16
			// 
			label16.Location = new System.Drawing.Point(218, 50);
			label16.Name = "label16";
			label16.Size = new System.Drawing.Size(74, 14);
			label16.TabIndex = 0;
			label16.Text = "Damage:";
			label16.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// resetdamagetype
			// 
			this.resetdamagetype.Image = ((System.Drawing.Image)(resources.GetObject("resetdamagetype.Image")));
			this.resetdamagetype.Location = new System.Drawing.Point(471, 44);
			this.resetdamagetype.Name = "resetdamagetype";
			this.resetdamagetype.Size = new System.Drawing.Size(28, 25);
			this.resetdamagetype.TabIndex = 2;
			this.resetdamagetype.Text = " ";
			this.tooltip.SetToolTip(this.resetdamagetype, "Reset");
			this.resetdamagetype.UseVisualStyleBackColor = true;
			this.resetdamagetype.Click += new System.EventHandler(this.resetdamagetype_Click);
			// 
			// brightness
			// 
			this.brightness.AllowDecimal = false;
			this.brightness.AllowExpressions = false;
			this.brightness.AllowNegative = false;
			this.brightness.AllowRelative = true;
			this.brightness.ButtonStep = 8;
			this.brightness.ButtonStepBig = 16F;
			this.brightness.ButtonStepFloat = 1F;
			this.brightness.ButtonStepSmall = 1F;
			this.brightness.ButtonStepsUseModifierKeys = true;
			this.brightness.ButtonStepsWrapAround = false;
			this.brightness.Location = new System.Drawing.Point(89, 46);
			this.brightness.Name = "brightness";
			this.brightness.Size = new System.Drawing.Size(81, 24);
			this.brightness.StepValues = null;
			this.brightness.TabIndex = 4;
			this.brightness.WhenTextChanged += new System.EventHandler(this.brightness_WhenTextChanged);
			// 
			// damagetype
			// 
			this.damagetype.FormattingEnabled = true;
			this.damagetype.Location = new System.Drawing.Point(298, 46);
			this.damagetype.Name = "damagetype";
			this.damagetype.Size = new System.Drawing.Size(167, 21);
			this.damagetype.TabIndex = 1;
			this.damagetype.TextChanged += new System.EventHandler(this.damagetype_TextChanged);
			this.damagetype.MouseDown += new System.Windows.Forms.MouseEventHandler(this.damagetype_MouseDown);
			// 
			// label9
			// 
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(21, 51);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(59, 13);
			label9.TabIndex = 3;
			label9.Text = "Brightness:";
			// 
			// gravity
			// 
			this.gravity.AllowDecimal = true;
			this.gravity.AllowExpressions = false;
			this.gravity.AllowNegative = true;
			this.gravity.AllowRelative = true;
			this.gravity.ButtonStep = 1;
			this.gravity.ButtonStepBig = 1F;
			this.gravity.ButtonStepFloat = 0.1F;
			this.gravity.ButtonStepSmall = 0.01F;
			this.gravity.ButtonStepsUseModifierKeys = true;
			this.gravity.ButtonStepsWrapAround = false;
			this.gravity.Location = new System.Drawing.Point(89, 74);
			this.gravity.Name = "gravity";
			this.gravity.Size = new System.Drawing.Size(81, 24);
			this.gravity.StepValues = null;
			this.gravity.TabIndex = 6;
			// 
			// label2
			// 
			label2.Location = new System.Drawing.Point(9, 79);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(74, 14);
			label2.TabIndex = 5;
			label2.Text = "Gravity:";
			label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// groupfloorceiling
			// 
			groupfloorceiling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			groupfloorceiling.Controls.Add(label15);
			groupfloorceiling.Controls.Add(label6);
			groupfloorceiling.Controls.Add(label5);
			groupfloorceiling.Controls.Add(this.heightoffset);
			groupfloorceiling.Controls.Add(this.ceilingheight);
			groupfloorceiling.Controls.Add(this.sectorheightlabel);
			groupfloorceiling.Controls.Add(this.sectorheight);
			groupfloorceiling.Controls.Add(this.floorheight);
			groupfloorceiling.Location = new System.Drawing.Point(7, 186);
			groupfloorceiling.Name = "groupfloorceiling";
			groupfloorceiling.Size = new System.Drawing.Size(254, 138);
			groupfloorceiling.TabIndex = 0;
			groupfloorceiling.TabStop = false;
			groupfloorceiling.Text = " Heights ";
			// 
			// label15
			// 
			label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label15.ForeColor = System.Drawing.SystemColors.HotTrack;
			label15.Location = new System.Drawing.Point(9, 83);
			label15.Name = "label15";
			label15.Size = new System.Drawing.Size(74, 14);
			label15.TabIndex = 4;
			label15.Text = "Height offset:";
			label15.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.tooltip.SetToolTip(label15, "Changes floor and ceiling height by given value.\r\nUse \"++\" to raise by sector hei" +
        "ght.\r\nUse \"--\" to lower by sector height.");
			// 
			// label6
			// 
			label6.Location = new System.Drawing.Point(9, 23);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(74, 14);
			label6.TabIndex = 0;
			label6.Text = "Ceiling height:";
			label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label5
			// 
			label5.Location = new System.Drawing.Point(9, 53);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(74, 14);
			label5.TabIndex = 2;
			label5.Text = "Floor height:";
			label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// heightoffset
			// 
			this.heightoffset.AllowDecimal = false;
			this.heightoffset.AllowExpressions = true;
			this.heightoffset.AllowNegative = true;
			this.heightoffset.AllowRelative = true;
			this.heightoffset.ButtonStep = 8;
			this.heightoffset.ButtonStepBig = 16F;
			this.heightoffset.ButtonStepFloat = 1F;
			this.heightoffset.ButtonStepSmall = 1F;
			this.heightoffset.ButtonStepsUseModifierKeys = true;
			this.heightoffset.ButtonStepsWrapAround = false;
			this.heightoffset.Location = new System.Drawing.Point(89, 78);
			this.heightoffset.Name = "heightoffset";
			this.heightoffset.Size = new System.Drawing.Size(81, 24);
			this.heightoffset.StepValues = null;
			this.heightoffset.TabIndex = 5;
			this.heightoffset.WhenTextChanged += new System.EventHandler(this.heightoffset_WhenTextChanged);
			// 
			// ceilingheight
			// 
			this.ceilingheight.AllowDecimal = false;
			this.ceilingheight.AllowExpressions = true;
			this.ceilingheight.AllowNegative = true;
			this.ceilingheight.AllowRelative = true;
			this.ceilingheight.ButtonStep = 8;
			this.ceilingheight.ButtonStepBig = 16F;
			this.ceilingheight.ButtonStepFloat = 1F;
			this.ceilingheight.ButtonStepSmall = 1F;
			this.ceilingheight.ButtonStepsUseModifierKeys = true;
			this.ceilingheight.ButtonStepsWrapAround = false;
			this.ceilingheight.Location = new System.Drawing.Point(89, 18);
			this.ceilingheight.Name = "ceilingheight";
			this.ceilingheight.Size = new System.Drawing.Size(81, 24);
			this.ceilingheight.StepValues = null;
			this.ceilingheight.TabIndex = 1;
			this.ceilingheight.WhenTextChanged += new System.EventHandler(this.ceilingheight_WhenTextChanged);
			// 
			// sectorheightlabel
			// 
			this.sectorheightlabel.Location = new System.Drawing.Point(9, 113);
			this.sectorheightlabel.Name = "sectorheightlabel";
			this.sectorheightlabel.Size = new System.Drawing.Size(74, 14);
			this.sectorheightlabel.TabIndex = 6;
			this.sectorheightlabel.Text = "Sector height:";
			this.sectorheightlabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// sectorheight
			// 
			this.sectorheight.AutoSize = true;
			this.sectorheight.Location = new System.Drawing.Point(89, 114);
			this.sectorheight.Name = "sectorheight";
			this.sectorheight.Size = new System.Drawing.Size(13, 13);
			this.sectorheight.TabIndex = 7;
			this.sectorheight.Text = "0";
			// 
			// floorheight
			// 
			this.floorheight.AllowDecimal = false;
			this.floorheight.AllowExpressions = true;
			this.floorheight.AllowNegative = true;
			this.floorheight.AllowRelative = true;
			this.floorheight.ButtonStep = 8;
			this.floorheight.ButtonStepBig = 16F;
			this.floorheight.ButtonStepFloat = 1F;
			this.floorheight.ButtonStepSmall = 1F;
			this.floorheight.ButtonStepsUseModifierKeys = true;
			this.floorheight.ButtonStepsWrapAround = false;
			this.floorheight.Location = new System.Drawing.Point(89, 48);
			this.floorheight.Name = "floorheight";
			this.floorheight.Size = new System.Drawing.Size(81, 24);
			this.floorheight.StepValues = null;
			this.floorheight.TabIndex = 3;
			this.floorheight.WhenTextChanged += new System.EventHandler(this.floorheight_WhenTextChanged);
			// 
			// labelLightAlpha
			// 
			labelLightAlpha.AutoSize = true;
			labelLightAlpha.Location = new System.Drawing.Point(6, 92);
			labelLightAlpha.Name = "labelLightAlpha";
			labelLightAlpha.Size = new System.Drawing.Size(62, 13);
			labelLightAlpha.TabIndex = 18;
			labelLightAlpha.Text = "Light alpha:";
			labelLightAlpha.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelFadeAlpha
			// 
			labelFadeAlpha.AutoSize = true;
			labelFadeAlpha.Location = new System.Drawing.Point(6, 118);
			labelFadeAlpha.Name = "labelFadeAlpha";
			labelFadeAlpha.Size = new System.Drawing.Size(63, 13);
			labelFadeAlpha.TabIndex = 20;
			labelFadeAlpha.Text = "Fade alpha:";
			labelFadeAlpha.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelFadeStart
			// 
			labelFadeStart.AutoSize = true;
			labelFadeStart.Location = new System.Drawing.Point(11, 149);
			labelFadeStart.Name = "labelFadeStart";
			labelFadeStart.Size = new System.Drawing.Size(57, 13);
			labelFadeStart.TabIndex = 22;
			labelFadeStart.Text = "Fade start:";
			labelFadeStart.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelFadeEnd
			// 
			labelFadeEnd.AutoSize = true;
			labelFadeEnd.Location = new System.Drawing.Point(12, 177);
			labelFadeEnd.Name = "labelFadeEnd";
			labelFadeEnd.Size = new System.Drawing.Size(55, 13);
			labelFadeEnd.TabIndex = 24;
			labelFadeEnd.Text = "Fade end:";
			labelFadeEnd.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// tabs
			// 
			this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabs.Controls.Add(this.tabproperties);
			this.tabs.Controls.Add(this.tabColors);
			this.tabs.Controls.Add(this.tabSurfaces);
			this.tabs.Controls.Add(this.tabslopes);
			this.tabs.Controls.Add(this.tabcomment);
			this.tabs.Controls.Add(this.tabcustom);
			this.tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tabs.Location = new System.Drawing.Point(10, 10);
			this.tabs.Margin = new System.Windows.Forms.Padding(1);
			this.tabs.Name = "tabs";
			this.tabs.Padding = new System.Drawing.Point(20, 3);
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(578, 552);
			this.tabs.TabIndex = 1;
			// 
			// tabproperties
			// 
			this.tabproperties.Controls.Add(this.groupBox3);
			this.tabproperties.Controls.Add(groupaction);
			this.tabproperties.Controls.Add(groupeffect);
			this.tabproperties.Controls.Add(groupfloorceiling);
			this.tabproperties.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.tabproperties.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabproperties.Location = new System.Drawing.Point(4, 22);
			this.tabproperties.Name = "tabproperties";
			this.tabproperties.Padding = new System.Windows.Forms.Padding(3);
			this.tabproperties.Size = new System.Drawing.Size(570, 526);
			this.tabproperties.TabIndex = 0;
			this.tabproperties.Text = "Properties";
			this.tabproperties.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.flags);
			this.groupBox3.Location = new System.Drawing.Point(7, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(557, 174);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = " Flags ";
			// 
			// flags
			// 
			this.flags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.flags.AutoScroll = true;
			this.flags.Columns = 2;
			this.flags.Location = new System.Drawing.Point(15, 21);
			this.flags.Name = "flags";
			this.flags.Size = new System.Drawing.Size(536, 147);
			this.flags.TabIndex = 0;
			this.flags.VerticalSpacing = 1;
			// 
			// tabColors
			// 
			this.tabColors.Controls.Add(this.groupBox8);
			this.tabColors.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tabColors.Location = new System.Drawing.Point(4, 22);
			this.tabColors.Name = "tabColors";
			this.tabColors.Size = new System.Drawing.Size(570, 526);
			this.tabColors.TabIndex = 5;
			this.tabColors.Text = "Colors";
			this.tabColors.UseVisualStyleBackColor = true;
			// 
			// groupBox8
			// 
			this.groupBox8.Controls.Add(this.fadeEnd);
			this.groupBox8.Controls.Add(labelFadeEnd);
			this.groupBox8.Controls.Add(this.fadeStart);
			this.groupBox8.Controls.Add(labelFadeStart);
			this.groupBox8.Controls.Add(this.fadeAlpha);
			this.groupBox8.Controls.Add(labelFadeAlpha);
			this.groupBox8.Controls.Add(this.lightAlpha);
			this.groupBox8.Controls.Add(labelLightAlpha);
			this.groupBox8.Controls.Add(this.lightColor);
			this.groupBox8.Controls.Add(this.fadeColor);
			this.groupBox8.Location = new System.Drawing.Point(3, 3);
			this.groupBox8.Name = "groupBox8";
			this.groupBox8.Size = new System.Drawing.Size(277, 229);
			this.groupBox8.TabIndex = 18;
			this.groupBox8.TabStop = false;
			this.groupBox8.Text = "Global sector colors";
			// 
			// fadeEnd
			// 
			this.fadeEnd.AllowDecimal = false;
			this.fadeEnd.AllowExpressions = false;
			this.fadeEnd.AllowNegative = false;
			this.fadeEnd.AllowRelative = false;
			this.fadeEnd.ButtonStep = 1;
			this.fadeEnd.ButtonStepBig = 1F;
			this.fadeEnd.ButtonStepFloat = 1F;
			this.fadeEnd.ButtonStepSmall = 1F;
			this.fadeEnd.ButtonStepsUseModifierKeys = true;
			this.fadeEnd.ButtonStepsWrapAround = false;
			this.fadeEnd.Location = new System.Drawing.Point(74, 172);
			this.fadeEnd.Name = "fadeEnd";
			this.fadeEnd.Size = new System.Drawing.Size(81, 24);
			this.fadeEnd.StepValues = null;
			this.fadeEnd.TabIndex = 25;
			this.fadeEnd.WhenTextChanged += new System.EventHandler(this.fadeEnd_WhenTextChanged);
			// 
			// fadeStart
			// 
			this.fadeStart.AllowDecimal = false;
			this.fadeStart.AllowExpressions = false;
			this.fadeStart.AllowNegative = false;
			this.fadeStart.AllowRelative = false;
			this.fadeStart.ButtonStep = 1;
			this.fadeStart.ButtonStepBig = 1F;
			this.fadeStart.ButtonStepFloat = 1F;
			this.fadeStart.ButtonStepSmall = 1F;
			this.fadeStart.ButtonStepsUseModifierKeys = true;
			this.fadeStart.ButtonStepsWrapAround = false;
			this.fadeStart.Location = new System.Drawing.Point(74, 144);
			this.fadeStart.Name = "fadeStart";
			this.fadeStart.Size = new System.Drawing.Size(81, 24);
			this.fadeStart.StepValues = null;
			this.fadeStart.TabIndex = 23;
			this.fadeStart.WhenTextChanged += new System.EventHandler(this.fadeStart_WhenTextChanged);
			// 
			// fadeAlpha
			// 
			this.fadeAlpha.AllowDecimal = false;
			this.fadeAlpha.AllowExpressions = false;
			this.fadeAlpha.AllowNegative = false;
			this.fadeAlpha.AllowRelative = false;
			this.fadeAlpha.ButtonStep = 1;
			this.fadeAlpha.ButtonStepBig = 1F;
			this.fadeAlpha.ButtonStepFloat = 1F;
			this.fadeAlpha.ButtonStepSmall = 1F;
			this.fadeAlpha.ButtonStepsUseModifierKeys = true;
			this.fadeAlpha.ButtonStepsWrapAround = false;
			this.fadeAlpha.Location = new System.Drawing.Point(74, 113);
			this.fadeAlpha.Name = "fadeAlpha";
			this.fadeAlpha.Size = new System.Drawing.Size(81, 24);
			this.fadeAlpha.StepValues = null;
			this.fadeAlpha.TabIndex = 21;
			this.fadeAlpha.WhenTextChanged += new System.EventHandler(this.fadeAlpha_WhenTextChanged);
			// 
			// lightAlpha
			// 
			this.lightAlpha.AllowDecimal = false;
			this.lightAlpha.AllowExpressions = false;
			this.lightAlpha.AllowNegative = false;
			this.lightAlpha.AllowRelative = false;
			this.lightAlpha.ButtonStep = 1;
			this.lightAlpha.ButtonStepBig = 1F;
			this.lightAlpha.ButtonStepFloat = 1F;
			this.lightAlpha.ButtonStepSmall = 1F;
			this.lightAlpha.ButtonStepsUseModifierKeys = true;
			this.lightAlpha.ButtonStepsWrapAround = false;
			this.lightAlpha.Location = new System.Drawing.Point(74, 87);
			this.lightAlpha.Name = "lightAlpha";
			this.lightAlpha.Size = new System.Drawing.Size(81, 24);
			this.lightAlpha.StepValues = null;
			this.lightAlpha.TabIndex = 19;
			this.lightAlpha.WhenTextChanged += new System.EventHandler(this.lightAlpha_WhenTextChanged);
			// 
			// lightColor
			// 
			this.lightColor.DefaultValue = 16777215;
			this.lightColor.Field = "lightcolor";
			this.lightColor.Label = "Light:";
			this.lightColor.Location = new System.Drawing.Point(6, 19);
			this.lightColor.Name = "lightColor";
			this.lightColor.Size = new System.Drawing.Size(207, 29);
			this.lightColor.TabIndex = 16;
			this.lightColor.OnValueChanged += new System.EventHandler(this.lightColor_OnValueChanged);
			// 
			// fadeColor
			// 
			this.fadeColor.DefaultValue = 0;
			this.fadeColor.Field = "fadecolor";
			this.fadeColor.Label = "Fade:";
			this.fadeColor.Location = new System.Drawing.Point(6, 47);
			this.fadeColor.Name = "fadeColor";
			this.fadeColor.Size = new System.Drawing.Size(207, 31);
			this.fadeColor.TabIndex = 17;
			this.fadeColor.OnValueChanged += new System.EventHandler(this.fadeColor_OnValueChanged);
			// 
			// tabSurfaces
			// 
			this.tabSurfaces.Controls.Add(this.groupBox2);
			this.tabSurfaces.Controls.Add(this.groupBox1);
			this.tabSurfaces.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabSurfaces.Location = new System.Drawing.Point(4, 22);
			this.tabSurfaces.Name = "tabSurfaces";
			this.tabSurfaces.Size = new System.Drawing.Size(570, 526);
			this.tabSurfaces.TabIndex = 2;
			this.tabSurfaces.Text = "Surfaces";
			this.tabSurfaces.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.resetfloorlight);
			this.groupBox2.Controls.Add(this.labelFloorOffsets);
			this.groupBox2.Controls.Add(this.labelFloorScale);
			this.groupBox2.Controls.Add(this.cbUseFloorLineAngles);
			this.groupBox2.Controls.Add(this.floorAngleControl);
			this.groupBox2.Controls.Add(this.label11);
			this.groupBox2.Controls.Add(this.floorRotation);
			this.groupBox2.Controls.Add(this.floorLightAbsolute);
			this.groupBox2.Controls.Add(this.label12);
			this.groupBox2.Controls.Add(this.floorBrightness);
			this.groupBox2.Controls.Add(this.floorScale);
			this.groupBox2.Controls.Add(this.floorOffsets);
			this.groupBox2.Controls.Add(this.floortex);
			this.groupBox2.Location = new System.Drawing.Point(3, 244);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(564, 235);
			this.groupBox2.TabIndex = 55;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = " Floor ";
			// 
			// resetfloorlight
			// 
			this.resetfloorlight.Image = ((System.Drawing.Image)(resources.GetObject("resetfloorlight.Image")));
			this.resetfloorlight.Location = new System.Drawing.Point(246, 138);
			this.resetfloorlight.Name = "resetfloorlight";
			this.resetfloorlight.Size = new System.Drawing.Size(23, 23);
			this.resetfloorlight.TabIndex = 12;
			this.tooltip.SetToolTip(this.resetfloorlight, "Reset");
			this.resetfloorlight.UseVisualStyleBackColor = true;
			this.resetfloorlight.Click += new System.EventHandler(this.resetfloorlight_Click);
			// 
			// labelFloorOffsets
			// 
			this.labelFloorOffsets.Location = new System.Drawing.Point(8, 27);
			this.labelFloorOffsets.Name = "labelFloorOffsets";
			this.labelFloorOffsets.Size = new System.Drawing.Size(98, 14);
			this.labelFloorOffsets.TabIndex = 0;
			this.labelFloorOffsets.Tag = "";
			this.labelFloorOffsets.Text = "Texture offsets:";
			this.labelFloorOffsets.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelFloorScale
			// 
			this.labelFloorScale.Location = new System.Drawing.Point(8, 59);
			this.labelFloorScale.Name = "labelFloorScale";
			this.labelFloorScale.Size = new System.Drawing.Size(98, 14);
			this.labelFloorScale.TabIndex = 2;
			this.labelFloorScale.Tag = "";
			this.labelFloorScale.Text = "Texture scale:";
			this.labelFloorScale.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// cbUseFloorLineAngles
			// 
			this.cbUseFloorLineAngles.AutoSize = true;
			this.cbUseFloorLineAngles.Location = new System.Drawing.Point(181, 172);
			this.cbUseFloorLineAngles.Name = "cbUseFloorLineAngles";
			this.cbUseFloorLineAngles.Size = new System.Drawing.Size(113, 17);
			this.cbUseFloorLineAngles.TabIndex = 16;
			this.cbUseFloorLineAngles.Tag = "";
			this.cbUseFloorLineAngles.Text = "Use linedef angles";
			this.cbUseFloorLineAngles.UseVisualStyleBackColor = true;
			this.cbUseFloorLineAngles.CheckedChanged += new System.EventHandler(this.cbUseFloorLineAngles_CheckedChanged);
			// 
			// floorAngleControl
			// 
			this.floorAngleControl.Angle = -2430;
			this.floorAngleControl.AngleOffset = 90;
			this.floorAngleControl.DoomAngleClamping = false;
			this.floorAngleControl.Location = new System.Drawing.Point(6, 156);
			this.floorAngleControl.Name = "floorAngleControl";
			this.floorAngleControl.Size = new System.Drawing.Size(44, 44);
			this.floorAngleControl.TabIndex = 13;
			this.floorAngleControl.AngleChanged += new System.EventHandler(this.floorAngleControl_AngleChanged);
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(26, 172);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(80, 14);
			this.label11.TabIndex = 14;
			this.label11.Tag = "";
			this.label11.Text = "Rotation:";
			this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// floorRotation
			// 
			this.floorRotation.AllowDecimal = true;
			this.floorRotation.AllowExpressions = false;
			this.floorRotation.AllowNegative = true;
			this.floorRotation.AllowRelative = true;
			this.floorRotation.ButtonStep = 5;
			this.floorRotation.ButtonStepBig = 15F;
			this.floorRotation.ButtonStepFloat = 1F;
			this.floorRotation.ButtonStepSmall = 0.1F;
			this.floorRotation.ButtonStepsUseModifierKeys = true;
			this.floorRotation.ButtonStepsWrapAround = false;
			this.floorRotation.Location = new System.Drawing.Point(113, 167);
			this.floorRotation.Name = "floorRotation";
			this.floorRotation.Size = new System.Drawing.Size(62, 24);
			this.floorRotation.StepValues = null;
			this.floorRotation.TabIndex = 15;
			this.floorRotation.Tag = "";
			this.floorRotation.WhenTextChanged += new System.EventHandler(this.floorRotation_WhenTextChanged);
			// 
			// floorLightAbsolute
			// 
			this.floorLightAbsolute.AutoSize = true;
			this.floorLightAbsolute.Location = new System.Drawing.Point(181, 142);
			this.floorLightAbsolute.Name = "floorLightAbsolute";
			this.floorLightAbsolute.Size = new System.Drawing.Size(67, 17);
			this.floorLightAbsolute.TabIndex = 11;
			this.floorLightAbsolute.Text = "Absolute";
			this.floorLightAbsolute.UseVisualStyleBackColor = true;
			this.floorLightAbsolute.CheckedChanged += new System.EventHandler(this.floorLightAbsolute_CheckedChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(26, 142);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(80, 14);
			this.label12.TabIndex = 9;
			this.label12.Tag = "";
			this.label12.Text = "Brightness:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// floorBrightness
			// 
			this.floorBrightness.AllowDecimal = false;
			this.floorBrightness.AllowExpressions = false;
			this.floorBrightness.AllowNegative = true;
			this.floorBrightness.AllowRelative = true;
			this.floorBrightness.ButtonStep = 16;
			this.floorBrightness.ButtonStepBig = 32F;
			this.floorBrightness.ButtonStepFloat = 1F;
			this.floorBrightness.ButtonStepSmall = 1F;
			this.floorBrightness.ButtonStepsUseModifierKeys = true;
			this.floorBrightness.ButtonStepsWrapAround = false;
			this.floorBrightness.Location = new System.Drawing.Point(113, 137);
			this.floorBrightness.Name = "floorBrightness";
			this.floorBrightness.Size = new System.Drawing.Size(62, 24);
			this.floorBrightness.StepValues = null;
			this.floorBrightness.TabIndex = 10;
			this.floorBrightness.Tag = "lightfloor";
			this.floorBrightness.WhenTextChanged += new System.EventHandler(this.floorBrightness_WhenTextChanged);
			// 
			// floorScale
			// 
			this.floorScale.AllowDecimal = true;
			this.floorScale.AllowValueLinking = true;
			this.floorScale.ButtonStep = 1;
			this.floorScale.ButtonStepBig = 1F;
			this.floorScale.ButtonStepFloat = 0.1F;
			this.floorScale.ButtonStepSmall = 0.01F;
			this.floorScale.ButtonStepsUseModifierKeys = true;
			this.floorScale.DefaultValue = 1F;
			this.floorScale.Field1 = "xscalefloor";
			this.floorScale.Field2 = "yscalefloor";
			this.floorScale.LinkValues = false;
			this.floorScale.Location = new System.Drawing.Point(110, 53);
			this.floorScale.Name = "floorScale";
			this.floorScale.Size = new System.Drawing.Size(186, 26);
			this.floorScale.TabIndex = 3;
			this.floorScale.OnValuesChanged += new System.EventHandler(this.floorScale_OnValuesChanged);
			// 
			// floorOffsets
			// 
			this.floorOffsets.AllowDecimal = true;
			this.floorOffsets.AllowValueLinking = false;
			this.floorOffsets.ButtonStep = 1;
			this.floorOffsets.ButtonStepBig = 32F;
			this.floorOffsets.ButtonStepFloat = 16F;
			this.floorOffsets.ButtonStepSmall = 1F;
			this.floorOffsets.ButtonStepsUseModifierKeys = true;
			this.floorOffsets.DefaultValue = 0F;
			this.floorOffsets.Field1 = "xpanningfloor";
			this.floorOffsets.Field2 = "ypanningfloor";
			this.floorOffsets.LinkValues = false;
			this.floorOffsets.Location = new System.Drawing.Point(110, 21);
			this.floorOffsets.Name = "floorOffsets";
			this.floorOffsets.Size = new System.Drawing.Size(186, 26);
			this.floorOffsets.TabIndex = 1;
			this.floorOffsets.OnValuesChanged += new System.EventHandler(this.floorOffsets_OnValuesChanged);
			// 
			// floortex
			// 
			this.floortex.Location = new System.Drawing.Point(356, 21);
			this.floortex.MultipleTextures = false;
			this.floortex.Name = "floortex";
			this.floortex.Size = new System.Drawing.Size(176, 200);
			this.floortex.TabIndex = 25;
			this.floortex.TextureName = "";
			this.floortex.OnValueChanged += new System.EventHandler(this.floortex_OnValueChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.resetceillight);
			this.groupBox1.Controls.Add(this.labelCeilOffsets);
			this.groupBox1.Controls.Add(this.labelCeilScale);
			this.groupBox1.Controls.Add(this.cbUseCeilLineAngles);
			this.groupBox1.Controls.Add(this.ceilAngleControl);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.ceilRotation);
			this.groupBox1.Controls.Add(this.ceilLightAbsolute);
			this.groupBox1.Controls.Add(this.labelLightFront);
			this.groupBox1.Controls.Add(this.ceilBrightness);
			this.groupBox1.Controls.Add(this.ceilScale);
			this.groupBox1.Controls.Add(this.ceilOffsets);
			this.groupBox1.Controls.Add(this.ceilingtex);
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(564, 235);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = " Ceiling ";
			// 
			// resetceillight
			// 
			this.resetceillight.Image = ((System.Drawing.Image)(resources.GetObject("resetceillight.Image")));
			this.resetceillight.Location = new System.Drawing.Point(246, 138);
			this.resetceillight.Name = "resetceillight";
			this.resetceillight.Size = new System.Drawing.Size(23, 23);
			this.resetceillight.TabIndex = 12;
			this.tooltip.SetToolTip(this.resetceillight, "Reset");
			this.resetceillight.UseVisualStyleBackColor = true;
			this.resetceillight.Click += new System.EventHandler(this.resetceillight_Click);
			// 
			// labelCeilOffsets
			// 
			this.labelCeilOffsets.Location = new System.Drawing.Point(8, 27);
			this.labelCeilOffsets.Name = "labelCeilOffsets";
			this.labelCeilOffsets.Size = new System.Drawing.Size(98, 14);
			this.labelCeilOffsets.TabIndex = 0;
			this.labelCeilOffsets.Tag = "";
			this.labelCeilOffsets.Text = "Texture offsets:";
			this.labelCeilOffsets.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// labelCeilScale
			// 
			this.labelCeilScale.Location = new System.Drawing.Point(8, 59);
			this.labelCeilScale.Name = "labelCeilScale";
			this.labelCeilScale.Size = new System.Drawing.Size(98, 14);
			this.labelCeilScale.TabIndex = 2;
			this.labelCeilScale.Tag = "";
			this.labelCeilScale.Text = "Texture scale:";
			this.labelCeilScale.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// cbUseCeilLineAngles
			// 
			this.cbUseCeilLineAngles.AutoSize = true;
			this.cbUseCeilLineAngles.Location = new System.Drawing.Point(181, 172);
			this.cbUseCeilLineAngles.Name = "cbUseCeilLineAngles";
			this.cbUseCeilLineAngles.Size = new System.Drawing.Size(113, 17);
			this.cbUseCeilLineAngles.TabIndex = 16;
			this.cbUseCeilLineAngles.Tag = "";
			this.cbUseCeilLineAngles.Text = "Use linedef angles";
			this.cbUseCeilLineAngles.UseVisualStyleBackColor = true;
			this.cbUseCeilLineAngles.CheckedChanged += new System.EventHandler(this.cbUseCeilLineAngles_CheckedChanged);
			// 
			// ceilAngleControl
			// 
			this.ceilAngleControl.Angle = -2430;
			this.ceilAngleControl.AngleOffset = 90;
			this.ceilAngleControl.DoomAngleClamping = false;
			this.ceilAngleControl.Location = new System.Drawing.Point(6, 156);
			this.ceilAngleControl.Name = "ceilAngleControl";
			this.ceilAngleControl.Size = new System.Drawing.Size(44, 44);
			this.ceilAngleControl.TabIndex = 13;
			this.ceilAngleControl.AngleChanged += new System.EventHandler(this.ceilAngleControl_AngleChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(26, 172);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 14);
			this.label1.TabIndex = 14;
			this.label1.Tag = "";
			this.label1.Text = "Rotation:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ceilRotation
			// 
			this.ceilRotation.AllowDecimal = true;
			this.ceilRotation.AllowExpressions = false;
			this.ceilRotation.AllowNegative = true;
			this.ceilRotation.AllowRelative = true;
			this.ceilRotation.ButtonStep = 5;
			this.ceilRotation.ButtonStepBig = 15F;
			this.ceilRotation.ButtonStepFloat = 1F;
			this.ceilRotation.ButtonStepSmall = 0.1F;
			this.ceilRotation.ButtonStepsUseModifierKeys = true;
			this.ceilRotation.ButtonStepsWrapAround = true;
			this.ceilRotation.Location = new System.Drawing.Point(113, 167);
			this.ceilRotation.Name = "ceilRotation";
			this.ceilRotation.Size = new System.Drawing.Size(62, 24);
			this.ceilRotation.StepValues = null;
			this.ceilRotation.TabIndex = 15;
			this.ceilRotation.Tag = "";
			this.ceilRotation.WhenTextChanged += new System.EventHandler(this.ceilRotation_WhenTextChanged);
			// 
			// ceilLightAbsolute
			// 
			this.ceilLightAbsolute.AutoSize = true;
			this.ceilLightAbsolute.Location = new System.Drawing.Point(181, 142);
			this.ceilLightAbsolute.Name = "ceilLightAbsolute";
			this.ceilLightAbsolute.Size = new System.Drawing.Size(67, 17);
			this.ceilLightAbsolute.TabIndex = 11;
			this.ceilLightAbsolute.Tag = "";
			this.ceilLightAbsolute.Text = "Absolute";
			this.ceilLightAbsolute.UseVisualStyleBackColor = true;
			this.ceilLightAbsolute.CheckedChanged += new System.EventHandler(this.ceilLightAbsolute_CheckedChanged);
			// 
			// labelLightFront
			// 
			this.labelLightFront.Location = new System.Drawing.Point(26, 142);
			this.labelLightFront.Name = "labelLightFront";
			this.labelLightFront.Size = new System.Drawing.Size(80, 14);
			this.labelLightFront.TabIndex = 9;
			this.labelLightFront.Tag = "";
			this.labelLightFront.Text = "Brightness:";
			this.labelLightFront.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// ceilBrightness
			// 
			this.ceilBrightness.AllowDecimal = false;
			this.ceilBrightness.AllowExpressions = false;
			this.ceilBrightness.AllowNegative = true;
			this.ceilBrightness.AllowRelative = true;
			this.ceilBrightness.ButtonStep = 16;
			this.ceilBrightness.ButtonStepBig = 32F;
			this.ceilBrightness.ButtonStepFloat = 1F;
			this.ceilBrightness.ButtonStepSmall = 1F;
			this.ceilBrightness.ButtonStepsUseModifierKeys = true;
			this.ceilBrightness.ButtonStepsWrapAround = false;
			this.ceilBrightness.Location = new System.Drawing.Point(113, 137);
			this.ceilBrightness.Name = "ceilBrightness";
			this.ceilBrightness.Size = new System.Drawing.Size(62, 24);
			this.ceilBrightness.StepValues = null;
			this.ceilBrightness.TabIndex = 10;
			this.ceilBrightness.Tag = "lightceiling";
			this.ceilBrightness.WhenTextChanged += new System.EventHandler(this.ceilBrightness_WhenTextChanged);
			// 
			// ceilScale
			// 
			this.ceilScale.AllowDecimal = true;
			this.ceilScale.AllowValueLinking = true;
			this.ceilScale.ButtonStep = 1;
			this.ceilScale.ButtonStepBig = 1F;
			this.ceilScale.ButtonStepFloat = 0.1F;
			this.ceilScale.ButtonStepSmall = 0.01F;
			this.ceilScale.ButtonStepsUseModifierKeys = true;
			this.ceilScale.DefaultValue = 1F;
			this.ceilScale.Field1 = "xscaleceiling";
			this.ceilScale.Field2 = "yscaleceiling";
			this.ceilScale.LinkValues = false;
			this.ceilScale.Location = new System.Drawing.Point(110, 53);
			this.ceilScale.Name = "ceilScale";
			this.ceilScale.Size = new System.Drawing.Size(186, 26);
			this.ceilScale.TabIndex = 3;
			this.ceilScale.OnValuesChanged += new System.EventHandler(this.ceilScale_OnValuesChanged);
			// 
			// ceilOffsets
			// 
			this.ceilOffsets.AllowDecimal = true;
			this.ceilOffsets.AllowValueLinking = false;
			this.ceilOffsets.ButtonStep = 1;
			this.ceilOffsets.ButtonStepBig = 32F;
			this.ceilOffsets.ButtonStepFloat = 16F;
			this.ceilOffsets.ButtonStepSmall = 1F;
			this.ceilOffsets.ButtonStepsUseModifierKeys = true;
			this.ceilOffsets.DefaultValue = 0F;
			this.ceilOffsets.Field1 = "xpanningceiling";
			this.ceilOffsets.Field2 = "ypanningceiling";
			this.ceilOffsets.LinkValues = false;
			this.ceilOffsets.Location = new System.Drawing.Point(110, 21);
			this.ceilOffsets.Name = "ceilOffsets";
			this.ceilOffsets.Size = new System.Drawing.Size(186, 26);
			this.ceilOffsets.TabIndex = 1;
			this.ceilOffsets.OnValuesChanged += new System.EventHandler(this.ceilOffsets_OnValuesChanged);
			// 
			// ceilingtex
			// 
			this.ceilingtex.Location = new System.Drawing.Point(356, 21);
			this.ceilingtex.MultipleTextures = false;
			this.ceilingtex.Name = "ceilingtex";
			this.ceilingtex.Size = new System.Drawing.Size(176, 200);
			this.ceilingtex.TabIndex = 25;
			this.ceilingtex.TextureName = "";
			this.ceilingtex.OnValueChanged += new System.EventHandler(this.ceilingtex_OnValueChanged);
			// 
			// tabslopes
			// 
			this.tabslopes.Controls.Add(this.groupBox5);
			this.tabslopes.Controls.Add(this.groupBox4);
			this.tabslopes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabslopes.Location = new System.Drawing.Point(4, 22);
			this.tabslopes.Name = "tabslopes";
			this.tabslopes.Size = new System.Drawing.Size(570, 526);
			this.tabslopes.TabIndex = 3;
			this.tabslopes.Text = "Slopes";
			this.tabslopes.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.floorslopecontrol);
			this.groupBox5.Location = new System.Drawing.Point(3, 261);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(298, 266);
			this.groupBox5.TabIndex = 1;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = " Floor slope ";
			// 
			// floorslopecontrol
			// 
			this.floorslopecontrol.Location = new System.Drawing.Point(4, 19);
			this.floorslopecontrol.Name = "floorslopecontrol";
			this.floorslopecontrol.Size = new System.Drawing.Size(290, 178);
			this.floorslopecontrol.TabIndex = 0;
			this.floorslopecontrol.UseLineAngles = false;
			this.floorslopecontrol.OnAnglesChanged += new System.EventHandler(this.floorslopecontrol_OnAnglesChanged);
			this.floorslopecontrol.OnUseLineAnglesChanged += new System.EventHandler(this.floorslopecontrol_OnUseLineAnglesChanged);
			this.floorslopecontrol.OnPivotModeChanged += new System.EventHandler(this.floorslopecontrol_OnPivotModeChanged);
			this.floorslopecontrol.OnResetClicked += new System.EventHandler(this.floorslopecontrol_OnResetClicked);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.ceilingslopecontrol);
			this.groupBox4.Location = new System.Drawing.Point(3, 3);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(298, 252);
			this.groupBox4.TabIndex = 0;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = " Ceiling slope ";
			// 
			// ceilingslopecontrol
			// 
			this.ceilingslopecontrol.Location = new System.Drawing.Point(4, 19);
			this.ceilingslopecontrol.Name = "ceilingslopecontrol";
			this.ceilingslopecontrol.Size = new System.Drawing.Size(290, 178);
			this.ceilingslopecontrol.TabIndex = 1;
			this.ceilingslopecontrol.UseLineAngles = false;
			this.ceilingslopecontrol.OnAnglesChanged += new System.EventHandler(this.ceilingslopecontrol_OnAnglesChanged);
			this.ceilingslopecontrol.OnUseLineAnglesChanged += new System.EventHandler(this.ceilingslopecontrol_OnUseLineAnglesChanged);
			this.ceilingslopecontrol.OnPivotModeChanged += new System.EventHandler(this.ceilingslopecontrol_OnPivotModeChanged);
			this.ceilingslopecontrol.OnResetClicked += new System.EventHandler(this.ceilingslopecontrol_OnResetClicked);
			// 
			// tabcomment
			// 
			this.tabcomment.Controls.Add(this.commenteditor);
			this.tabcomment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.tabcomment.Location = new System.Drawing.Point(4, 22);
			this.tabcomment.Name = "tabcomment";
			this.tabcomment.Size = new System.Drawing.Size(570, 526);
			this.tabcomment.TabIndex = 4;
			this.tabcomment.Text = "Comment";
			this.tabcomment.UseVisualStyleBackColor = true;
			// 
			// commenteditor
			// 
			this.commenteditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.commenteditor.Location = new System.Drawing.Point(0, 0);
			this.commenteditor.Name = "commenteditor";
			this.commenteditor.Size = new System.Drawing.Size(570, 526);
			this.commenteditor.TabIndex = 0;
			// 
			// tabcustom
			// 
			this.tabcustom.Controls.Add(this.fieldslist);
			this.tabcustom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabcustom.Location = new System.Drawing.Point(4, 22);
			this.tabcustom.Name = "tabcustom";
			this.tabcustom.Padding = new System.Windows.Forms.Padding(3);
			this.tabcustom.Size = new System.Drawing.Size(570, 526);
			this.tabcustom.TabIndex = 1;
			this.tabcustom.Text = "Custom";
			this.tabcustom.UseVisualStyleBackColor = true;
			this.tabcustom.MouseEnter += new System.EventHandler(this.tabcustom_MouseEnter);
			// 
			// fieldslist
			// 
			this.fieldslist.AllowInsert = true;
			this.fieldslist.AutoInsertUserPrefix = true;
			this.fieldslist.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.fieldslist.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fieldslist.Location = new System.Drawing.Point(3, 3);
			this.fieldslist.Margin = new System.Windows.Forms.Padding(8);
			this.fieldslist.Name = "fieldslist";
			this.fieldslist.PropertyColumnVisible = true;
			this.fieldslist.PropertyColumnWidth = 150;
			this.fieldslist.ShowFixedFields = true;
			this.fieldslist.Size = new System.Drawing.Size(564, 520);
			this.fieldslist.TabIndex = 1;
			this.fieldslist.TypeColumnVisible = true;
			this.fieldslist.TypeColumnWidth = 100;
			this.fieldslist.ValueColumnVisible = true;
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.Location = new System.Drawing.Point(474, 566);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(112, 25);
			this.cancel.TabIndex = 4;
			this.cancel.Text = "Cancel";
			this.cancel.UseVisualStyleBackColor = true;
			this.cancel.Click += new System.EventHandler(this.cancel_Click);
			// 
			// apply
			// 
			this.apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.apply.Location = new System.Drawing.Point(356, 566);
			this.apply.Name = "apply";
			this.apply.Size = new System.Drawing.Size(112, 25);
			this.apply.TabIndex = 3;
			this.apply.Text = "OK";
			this.apply.UseVisualStyleBackColor = true;
			this.apply.Click += new System.EventHandler(this.apply_Click);
			// 
			// tooltip
			// 
			this.tooltip.AutomaticDelay = 10;
			this.tooltip.AutoPopDelay = 10000;
			this.tooltip.InitialDelay = 10;
			this.tooltip.ReshowDelay = 100;
			// 
			// friction
			// 
			this.friction.AllowDecimal = true;
			this.friction.AllowExpressions = false;
			this.friction.AllowNegative = true;
			this.friction.AllowRelative = true;
			this.friction.ButtonStep = 1;
			this.friction.ButtonStepBig = 0.125F;
			this.friction.ButtonStepFloat = 0.03125F;
			this.friction.ButtonStepSmall = 0.03125F;
			this.friction.ButtonStepsUseModifierKeys = true;
			this.friction.ButtonStepsWrapAround = false;
			this.friction.Location = new System.Drawing.Point(298, 74);
			this.friction.Name = "friction";
			this.friction.Size = new System.Drawing.Size(81, 24);
			this.friction.StepValues = null;
			this.friction.TabIndex = 23;
			// 
			// labelFriction
			// 
			labelFriction.Location = new System.Drawing.Point(218, 79);
			labelFriction.Name = "labelFriction";
			labelFriction.Size = new System.Drawing.Size(74, 14);
			labelFriction.TabIndex = 22;
			labelFriction.Text = "Friction:";
			labelFriction.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// SectorEditFormSRB2
			// 
			this.AcceptButton = this.apply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(598, 592);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.apply);
			this.Controls.Add(this.tabs);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SectorEditFormSRB2";
			this.Opacity = 0D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Sector";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SectorEditFormUDMF_FormClosing);
			this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.SectorEditFormUDMF_HelpRequested);
			groupaction.ResumeLayout(false);
			groupeffect.ResumeLayout(false);
			groupeffect.PerformLayout();
			groupfloorceiling.ResumeLayout(false);
			groupfloorceiling.PerformLayout();
			this.tabs.ResumeLayout(false);
			this.tabproperties.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.tabColors.ResumeLayout(false);
			this.groupBox8.ResumeLayout(false);
			this.groupBox8.PerformLayout();
			this.tabSurfaces.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabslopes.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.tabcomment.ResumeLayout(false);
			this.tabcustom.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tabproperties;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox brightness;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox ceilingheight;
		private System.Windows.Forms.Label sectorheightlabel;
		private System.Windows.Forms.Label sectorheight;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox floorheight;
		private System.Windows.Forms.TabPage tabcustom;
		private CodeImp.DoomBuilder.Controls.FieldsEditorControl fieldslist;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.Button apply;
		private System.Windows.Forms.TabPage tabSurfaces;
		private System.Windows.Forms.GroupBox groupBox1;
		private CodeImp.DoomBuilder.Controls.PairedFieldsControl ceilOffsets;
		private CodeImp.DoomBuilder.Controls.FlatSelectorControl ceilingtex;
		private CodeImp.DoomBuilder.Controls.PairedFieldsControl ceilScale;
		private System.Windows.Forms.Label label1;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox ceilRotation;
		private System.Windows.Forms.CheckBox ceilLightAbsolute;
		private System.Windows.Forms.Label labelLightFront;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox ceilBrightness;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label11;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox floorRotation;
		private System.Windows.Forms.CheckBox floorLightAbsolute;
		private System.Windows.Forms.Label label12;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox floorBrightness;
		private CodeImp.DoomBuilder.Controls.PairedFieldsControl floorScale;
		private CodeImp.DoomBuilder.Controls.PairedFieldsControl floorOffsets;
		private CodeImp.DoomBuilder.Controls.FlatSelectorControl floortex;
		private CodeImp.DoomBuilder.Controls.AngleControlEx floorAngleControl;
		private CodeImp.DoomBuilder.Controls.AngleControlEx ceilAngleControl;
		private System.Windows.Forms.GroupBox groupBox3;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox gravity;
		private CodeImp.DoomBuilder.Controls.CheckboxArrayControl flags;
		private System.Windows.Forms.CheckBox cbUseFloorLineAngles;
		private System.Windows.Forms.CheckBox cbUseCeilLineAngles;
		private System.Windows.Forms.TabPage tabslopes;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox4;
		private CodeImp.DoomBuilder.Controls.SectorSlopeControl floorslopecontrol;
		private CodeImp.DoomBuilder.Controls.SectorSlopeControl ceilingslopecontrol;
		private CodeImp.DoomBuilder.Controls.ButtonsNumericTextbox heightoffset;
		private System.Windows.Forms.ToolTip tooltip;
		private System.Windows.Forms.Label labelFloorOffsets;
		private System.Windows.Forms.Label labelFloorScale;
		private System.Windows.Forms.Label labelCeilOffsets;
		private System.Windows.Forms.Label labelCeilScale;
		private System.Windows.Forms.TabPage tabcomment;
		private CodeImp.DoomBuilder.Controls.CommentEditor commenteditor;
		private CodeImp.DoomBuilder.Controls.TagsSelector tagsselector;
		private System.Windows.Forms.Button resetfloorlight;
		private System.Windows.Forms.Button resetceillight;
		private System.Windows.Forms.Button resetdamagetype;
		private System.Windows.Forms.ComboBox damagetype;
        private System.Windows.Forms.TabPage tabColors;
        private Controls.ColorFieldsControl fadeColor;
        private Controls.ColorFieldsControl lightColor;
        private System.Windows.Forms.GroupBox groupBox8;
		private Controls.ButtonsNumericTextbox lightAlpha;
		private Controls.ButtonsNumericTextbox fadeAlpha;
		private System.Windows.Forms.ComboBox triggerer;
		private Controls.ButtonsNumericTextbox triggerTag;
		private Controls.ButtonsNumericTextbox fadeEnd;
		private Controls.ButtonsNumericTextbox fadeStart;
		private Controls.ButtonsNumericTextbox friction;
	}
}