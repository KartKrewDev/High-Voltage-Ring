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

namespace CodeImp.DoomBuilder.BuilderModes.Interface
{
	public partial class SlopeArchForm : DelayedForm
	{
		private EditMode mode;
		private double originaltheta;
		private double originaloffset;
		private Vector2D p1;
		private Vector2D p2;

		public SlopeArchForm(EditMode mode, Vector2D p1, Vector2D p2)
		{
			InitializeComponent();

			this.mode = mode;
			this.p1 = p1;
			this.p2 = p2;

			originaltheta = 90.0;
			originaloffset = 45.0;

			theta.Text = originaltheta.ToString();
			offset.Text = originaloffset.ToString();
		}

		private void UpdateArch(object sender, EventArgs e)
		{
			double t = theta.GetResultFloat(originaltheta);
			double o = offset.GetResultFloat(originaloffset);

			if (t > 180.0)
				theta.Text = "180";

			if (t + o > 180.0 || t + o < 0.0)
				return;

			double s = up.Checked ? 1.0 : -1.0;

			((BaseVisualMode)mode).DoArchBetweenHandles(p1, p2, t, o, s);
		}
	}
}
