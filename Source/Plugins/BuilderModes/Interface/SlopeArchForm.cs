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
		private double originaltheta;
		private double originaloffset;
		private double originalscale;
		private double originalheightoffset;
		private double oldtheta;
		private double oldoffset;
		private double oldscale;
		private SlopeArcher slopearcher;
		public event EventHandler UpdateChangedObjects;

		internal SlopeArchForm(SlopeArcher slopearcher)
		{
			InitializeComponent();
			this.slopearcher = slopearcher;

			oldtheta = originaltheta = Angle2D.RadToDeg(this.slopearcher.Theta);
			oldoffset = originaloffset = Angle2D.RadToDeg(this.slopearcher.OffsetAngle);
			oldscale = originalscale = this.slopearcher.Scale;
			originalheightoffset = this.slopearcher.HeightOffset;

			theta.Text = originaltheta.ToString();
			offset.Text = originaloffset.ToString();
			scale.Text = (originalscale * 100.0).ToString();
			heightoffset.Text = originalheightoffset.ToString();
		}

		private void UpdateArch()
		{
			double t = theta.GetResultFloat(originaltheta);
			double o = offset.GetResultFloat(originaloffset);
			double s = scale.GetResultFloat(originalscale * 100.0) / 100.0;

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
			theta.Text = "180";
			offset.Text = "0";

			oldtheta = 180.0;
			oldoffset = 0.0;

			UpdateArch();
		}

		private void quartercircleleft_Click(object sender, EventArgs e)
		{
			theta.Text = "90";
			offset.Text = "90";

			oldtheta = 90.0;
			oldoffset = 90.0;

			UpdateArch();
		}

		private void quartercircleright_Click(object sender, EventArgs e)
		{
			theta.Text = "90";
			offset.Text = "0";

			oldtheta = 90.0;

			UpdateArch();
		}

		private void theta_changed(object sender, EventArgs e)
		{
			double newtheta = theta.GetResultFloat(originaltheta);
			double newoffset = offset.GetResultFloat(originaloffset);

			// Make sure that the new theta is between 0.0 and 180.0
			if (newtheta > 180.0)
			{
				newtheta = 180.0;
				theta.Text = "180";

				General.Interface.DisplayStatus(StatusType.Warning, "The angle can not be greater than 180.");
			}
			else if (newtheta <= 0.0)
			{
				newtheta = oldtheta;
				theta.Text = oldtheta.ToString();
			}

			// If the angle offset it locked automatically change its value to reflect the changed theta value
			if (lockoffset.Checked)
			{
				double diff = oldtheta - newtheta;
				newoffset = offset.GetResultFloat(originaloffset) + diff / 2.0;

				// Make sure that the result isn't invalid
				if(newoffset < 0.0)
				{
					theta.Text = oldtheta.ToString();
					offset.Text = oldoffset.ToString();
				}
				else
					offset.Text = (offset.GetResultFloat(originaloffset) + diff / 2.0).ToString();
			}

			// If the new result is larger than 180.0 reset to the previous values
			if(newtheta + newoffset > 180.0)
			{
				theta.Text = oldtheta.ToString();
				offset.Text = oldoffset.ToString();

				General.Interface.DisplayStatus(StatusType.Warning, "The sum of the angle and offset angle can not be greater than 180.");
			}

			oldtheta = theta.GetResultFloat(originaltheta);
			oldoffset = offset.GetResultFloat(originaloffset);

			UpdateArch();
		}

		private void offset_changed(object sender, EventArgs e)
		{
			double newtheta = theta.GetResultFloat(originaltheta);
			double newoffset = offset.GetResultFloat(originaloffset);

			if(newtheta + newoffset > 180.0)
			{
				theta.Text = oldtheta.ToString();
				offset.Text = oldoffset.ToString();
			}

			oldoffset = offset.GetResultFloat(originaloffset);

			UpdateArch();
		}

		private void scale_changed(object sender, EventArgs e)
		{
			double newscale = scale.GetResultFloat(originalscale);

			if (newscale <= 0.0)
				scale.Text = oldscale.ToString();

			oldscale = scale.GetResultFloat(originalscale);

			UpdateArch();
		}

		private void heightoffset_changed(object sender, EventArgs e)
		{
			UpdateArch();
		}

		private void up_CheckedChanged(object sender, EventArgs e)
		{
			UpdateArch();
		}

		private void down_CheckedChanged(object sender, EventArgs e)
		{
			UpdateArch();
		}

		private void lockoffset_CheckedChanged(object sender, EventArgs e)
		{
			offset.Enabled = !offset.Enabled;
		}
	}
}
