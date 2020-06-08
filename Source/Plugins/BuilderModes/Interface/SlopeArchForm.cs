#region ================== Copyright (c) 2020 Boris Iwanski

/*
 * This program is free software: you can redistribute it and/or modify
 *
 * it under the terms of the GNU General Public License as published by
 * 
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 *    but WITHOUT ANY WARRANTY; without even the implied warranty of
 * 
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 * 
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.If not, see<http://www.gnu.org/licenses/>.
 */

#endregion

using System;
using CodeImp.DoomBuilder.Windows;
using CodeImp.DoomBuilder.Geometry;

namespace CodeImp.DoomBuilder.BuilderModes
{
	internal partial class SlopeArchForm : DelayedForm
	{
		#region ================== Variables

		private double originaltheta;
		private double originaloffset;
		private double originalscale;
		private double originalheightoffset;
		private double oldtheta;
		private double oldoffset;
		private double oldscale;
		private SlopeArcher slopearcher;
		public event EventHandler UpdateChangedObjects;

		#endregion

		#region ================== Constructor / Destructor

		internal SlopeArchForm(SlopeArcher slopearcher)
		{
			InitializeComponent();
			this.slopearcher = slopearcher;

			oldtheta = originaltheta = Math.Round(Angle2D.RadToDeg(this.slopearcher.Theta), 2);
			oldoffset = originaloffset = Math.Round(Angle2D.RadToDeg(this.slopearcher.OffsetAngle), 2);
			oldscale = originalscale = this.slopearcher.Scale;
			originalheightoffset = this.slopearcher.HeightOffset;

			theta.Text = originaltheta.ToString();
			offset.Text = originaloffset.ToString();
			scale.Text = (originalscale * 100.0).ToString();
			heightoffset.Text = originalheightoffset.ToString();
		}

		#endregion

		#region ================== Methods

		/// <summary>
		/// Updates the arch with the values currently entered in the dialog
		/// </summary>
		private void UpdateArch()
		{
			double t = theta.GetResultFloat(originaltheta);
			double o = offset.GetResultFloat(originaloffset);
			double s = scale.GetResultFloat(originalscale * 100.0) / 100.0;

			// Flip the scale if "down" is checked
			if(!up.Checked)
				s *= -1.0;

			slopearcher.Theta = Angle2D.DegToRad(t);
			slopearcher.OffsetAngle = Angle2D.DegToRad(o);
			slopearcher.Scale = s;
			slopearcher.HeightOffset = heightoffset.GetResultFloat(originalheightoffset);

			slopearcher.ApplySlope();

			// BaseVisualMode added a event handler to the dialog, so BaseVisualMode will update the geometry when we tell it to
			UpdateChangedObjects?.Invoke(this, EventArgs.Empty);
		}

		#endregion

		#region ================== Events

		/// <summary>
		/// Immediately apply the arch when the form is shown
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void SlopeArchForm_Shown(object sender, EventArgs e)
		{
			slopearcher.ApplySlope();
			UpdateChangedObjects?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Sets the values for theta and angle offset for a half circle
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void halfcircle_Click(object sender, EventArgs e)
		{
			theta.Text = "180";
			offset.Text = "0";

			oldtheta = 180.0;
			oldoffset = 0.0;

			UpdateArch();
		}

		/// <summary>
		/// Sets the values for theta and angle offset for a quarter circle (top left quadrant viewed from the first selected slope hande)
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void quartercircleleft_Click(object sender, EventArgs e)
		{
			theta.Text = "90";
			offset.Text = "90";

			oldtheta = 90.0;
			oldoffset = 90.0;

			UpdateArch();
		}

		/// <summary>
		/// Sets the values for theta and angle offset for a quarter circle (top right quadrant viewed from the first selected slope hande)
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void quartercircleright_Click(object sender, EventArgs e)
		{
			theta.Text = "90";
			offset.Text = "0";

			oldtheta = 90.0;

			UpdateArch();
		}

		/// <summary>
		/// Handles updates of the theta value. Also does sanity checks and modifies the values if necessary
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
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

			// Remember the old values
			oldtheta = theta.GetResultFloat(originaltheta);
			oldoffset = offset.GetResultFloat(originaloffset);

			UpdateArch();
		}

		/// <summary>
		/// Handles updates of the angle offset value. Also does sanity checks and modifies the values if necessary
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void offset_changed(object sender, EventArgs e)
		{
			double newtheta = theta.GetResultFloat(originaltheta);
			double newoffset = offset.GetResultFloat(originaloffset);

			// If the new result is larger than 180.0 reset to the previous values
			if (newtheta + newoffset > 180.0)
			{
				theta.Text = oldtheta.ToString();
				offset.Text = oldoffset.ToString();
			}

			// Remember the old values
			oldtheta = theta.GetResultFloat(originaltheta);
			oldoffset = offset.GetResultFloat(originaloffset);

			UpdateArch();
		}

		/// <summary>
		/// Handles updates of the scale value. Also does sanity checks and modifies the values if necessary
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void scale_changed(object sender, EventArgs e)
		{
			double newscale = scale.GetResultFloat(originalscale);

			if (newscale <= 0.0)
				scale.Text = oldscale.ToString();

			// Remember the old value
			oldscale = scale.GetResultFloat(originalscale);

			UpdateArch();
		}

		/// <summary>
		/// Handles updates of the height offset value.
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void heightoffset_changed(object sender, EventArgs e)
		{
			UpdateArch();
		}

		/// <summary>
		/// Handles updates of the "up" radio box.
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void up_CheckedChanged(object sender, EventArgs e)
		{
			UpdateArch();
		}

		/// <summary>
		/// Handles updates of the "down" radio box.
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void down_CheckedChanged(object sender, EventArgs e)
		{
			UpdateArch();
		}

		/// <summary>
		/// Toggles the "lock offset angle" checkbox.
		/// </summary>
		/// <param name="sender">sender</param>
		/// <param name="e">event arguments</param>
		private void lockoffset_CheckedChanged(object sender, EventArgs e)
		{
			offset.Enabled = !offset.Enabled;
		}

		/// <summary>
		/// Inverts the current slope
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void invert_Click(object sender, EventArgs e)
		{
			// Flip up/down direction
			if(up.Checked)
			{
				up.Checked = false;
				down.Checked = true;
			}
			else
			{
				up.Checked = true;
				down.Checked = false;
			}

			double t = theta.GetResultFloat(originaltheta);
			double o = offset.GetResultFloat(originaloffset);

			// Subtract theta from the offset, if the result is greater than 0, otherwise add theta
			if (o - t < 0.0)
				o = o + t;
			else
				o = o - t;

			offset.Text = o.ToString();

			UpdateArch();
		}

		#endregion
	}
}
