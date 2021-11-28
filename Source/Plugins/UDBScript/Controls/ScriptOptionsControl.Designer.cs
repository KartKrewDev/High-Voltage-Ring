namespace CodeImp.DoomBuilder.UDBScript
{
	partial class ScriptOptionsControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.parametersview = new System.Windows.Forms.DataGridView();
			this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.enumscombo = new System.Windows.Forms.ComboBox();
			this.browsebutton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.parametersview)).BeginInit();
			this.SuspendLayout();
			// 
			// parametersview
			// 
			this.parametersview.AllowUserToAddRows = false;
			this.parametersview.AllowUserToDeleteRows = false;
			this.parametersview.AllowUserToResizeRows = false;
			this.parametersview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.parametersview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Description,
            this.Value});
			this.parametersview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parametersview.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.parametersview.Location = new System.Drawing.Point(0, 0);
			this.parametersview.MultiSelect = false;
			this.parametersview.Name = "parametersview";
			this.parametersview.RowHeadersVisible = false;
			this.parametersview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.parametersview.Size = new System.Drawing.Size(657, 446);
			this.parametersview.TabIndex = 26;
			this.parametersview.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.parametersview_CellBeginEdit);
			this.parametersview.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.parametersview_CellClick);
			this.parametersview.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.parametersview_CellEndEdit);
			this.parametersview.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.parametersview_CellValueChanged);
			this.parametersview.SelectionChanged += new System.EventHandler(this.parametersview_SelectionChanged);
			this.parametersview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.parametersview_MouseUp);
			this.parametersview.Resize += new System.EventHandler(this.parametersview_Resize);
			// 
			// Description
			// 
			this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Description.FillWeight = 70F;
			this.Description.HeaderText = "Description";
			this.Description.Name = "Description";
			this.Description.ReadOnly = true;
			this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Value
			// 
			this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Value.FillWeight = 30F;
			this.Value.HeaderText = "Value";
			this.Value.Name = "Value";
			this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// enumscombo
			// 
			this.enumscombo.FormattingEnabled = true;
			this.enumscombo.Location = new System.Drawing.Point(177, 256);
			this.enumscombo.Name = "enumscombo";
			this.enumscombo.Size = new System.Drawing.Size(121, 21);
			this.enumscombo.TabIndex = 27;
			this.enumscombo.Validating += new System.ComponentModel.CancelEventHandler(this.enumscombo_Validating);
			// 
			// browsebutton
			// 
			this.browsebutton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.browsebutton.Location = new System.Drawing.Point(319, 168);
			this.browsebutton.Name = "browsebutton";
			this.browsebutton.Size = new System.Drawing.Size(28, 22);
			this.browsebutton.TabIndex = 28;
			this.browsebutton.TabStop = false;
			this.browsebutton.UseVisualStyleBackColor = true;
			this.browsebutton.Click += new System.EventHandler(this.browsebutton_Click);
			// 
			// ScriptOptionsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.browsebutton);
			this.Controls.Add(this.enumscombo);
			this.Controls.Add(this.parametersview);
			this.Name = "ScriptOptionsControl";
			this.Size = new System.Drawing.Size(657, 446);
			((System.ComponentModel.ISupportInitialize)(this.parametersview)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView parametersview;
		private System.Windows.Forms.ComboBox enumscombo;
		private System.Windows.Forms.Button browsebutton;
		private System.Windows.Forms.DataGridViewTextBoxColumn Description;
		private System.Windows.Forms.DataGridViewTextBoxColumn Value;
	}
}
