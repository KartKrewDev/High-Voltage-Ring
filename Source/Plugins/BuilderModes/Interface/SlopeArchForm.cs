using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Windows;
using CodeImp.DoomBuilder.Controls;
using CodeImp.DoomBuilder.Editing;
using CodeImp.DoomBuilder.Geometry;

namespace CodeImp.DoomBuilder.BuilderModes
{
	internal partial class SlopeArchForm : DelayedForm
	{
		private EditMode mode;
		private double originaltheta;
		private double originaloffset;
		private double originalscale;
		private double originalheightoffset;
		private SlopeArcher slopearcher;
		public event EventHandler UpdateChangedObjects;
		bool updating;

		internal SlopeArchForm(EditMode mode, SlopeArcher slopearcher)
		{
			InitializeComponent();

			this.mode = mode;
			this.slopearcher = slopearcher;

			originaltheta = Angle2D.RadToDeg(this.slopearcher.Theta);
			originaloffset = Angle2D.RadToDeg(this.slopearcher.OffsetAngle);
			originalscale = this.slopearcher.Scale;
			originalheightoffset = this.slopearcher.HeightOffset;

			theta.Text = originaltheta.ToString();
			offset.Text = originaloffset.ToString();
			scale.Text = (originalscale * 100.0).ToString();
			heightoffset.Text = originalheightoffset.ToString();

			updating = false;
		}

		private void UpdateArch()
		{
			double t = theta.GetResultFloat(originaltheta);
			double o = offset.GetResultFloat(originaloffset);
			double s = scale.GetResultFloat(originalscale * 100.0) / 100.0;

			if (t > 180.0)
				theta.Text = "180";

			if (t + o > 180.0 || t + o < 0.0)
				return;

			if(!up.Checked)
				s *= -1.0;

			slopearcher.Theta = Angle2D.DegToRad(t);
			slopearcher.OffsetAngle = Angle2D.DegToRad(o);
			slopearcher.Scale = s;
			slopearcher.HeightOffset = heightoffset.GetResultFloat(originalheightoffset);

			slopearcher.ApplySlope();

			UpdateChangedObjects?.Invoke(this, EventArgs.Empty);
		}

		private void SlopeArchForm_Shown(object sender, EventArgs e)
		{
			slopearcher.ApplySlope();
			UpdateChangedObjects?.Invoke(this, EventArgs.Empty);
		}

		private void halfcircle_Click(object sender, EventArgs e)
		{
			updating = true;

			theta.Text = "180";
			offset.Text = "0";

			UpdateArch();

			updating = false;
		}

		private void quartercircleleft_Click(object sender, EventArgs e)
		{
			updating = true;

			theta.Text = "90";
			offset.Text = "90";

			UpdateArch();

			updating = false;
		}

		private void quartercircleright_Click(object sender, EventArgs e)
		{
			updating = true;

			theta.Text = "90";
			offset.Text = "0";

			UpdateArch();

			updating = false;
		}

		private void theta_WhenTextChanged(object sender, EventArgs e)
		{
			if (updating)
				return;

			updating = true;

			double t = theta.GetResultFloat(originaltheta);

			if (t <= 0.0)
			{
				t = 1.0;
				theta.Text = "1";
			}

			if (t > 180.0)
			{
				t = 180.0;
				theta.Text = "180";
			}

			double o = (180 - t) / 2.0;

			offset.Text = o.ToString();

			UpdateArch();

			updating = false;
		}

		private void offset_WhenTextChanged(object sender, EventArgs e)
		{
			if (updating)
				return;

			UpdateArch();
		}

		private void scale_WhenTextChanged(object sender, EventArgs e)
		{
			if (updating)
				return;

			UpdateArch();
		}

		private void heightoffset_WhenTextChanged(object sender, EventArgs e)
		{
			if (updating)
				return;

			UpdateArch();
		}

		private void up_CheckedChanged(object sender, EventArgs e)
		{
			if (updating)
				return;

			UpdateArch();
		}

		private void down_CheckedChanged(object sender, EventArgs e)
		{
			if (updating)
				return;

			UpdateArch();
		}
	}
}
