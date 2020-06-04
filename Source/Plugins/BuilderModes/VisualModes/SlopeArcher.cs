using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeImp.DoomBuilder.Editing;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.VisualModes;
using CodeImp.DoomBuilder.Geometry;

namespace CodeImp.DoomBuilder.BuilderModes
{
	internal class SlopeArcher
	{
		BaseVisualMode mode;
		private List<IVisualEventReceiver> sectors;
		private VisualSidedefSlope handle1;
		private VisualSidedefSlope handle2;
		private double theta;
		private double offsetangle;
		private double scale;
		private int baseheight;
		private double baseheightoffset;
		private double heightoffset;
		private Line2D handleline;
		private double length;

		public double Theta { get { return theta; } set { theta = value; CalculateBaseHeightOffset(); } }
		public double OffsetAngle { get { return offsetangle; } set { offsetangle = value; CalculateBaseHeightOffset(); } }
		public double Scale { get { return scale; } set { scale = value; } }
		public int Baseheight { get { return baseheight; } }
		public double HeightOffset { get { return heightoffset; } set { heightoffset = value; } }

		public SlopeArcher(BaseVisualMode mode, List<IVisualEventReceiver> sectors, VisualSidedefSlope handle1, VisualSidedefSlope handle2, double theta, double offsetangle, double scale)
		{
			this.mode = mode;
			this.sectors = sectors;
			this.handle1 = handle1;
			this.handle2 = handle2;
			this.theta = theta;
			this.offsetangle = offsetangle;
			this.scale = scale;
			heightoffset = 0.0;

			handleline = new Line2D(handle1.GetCenterPoint(), handle2.GetCenterPoint());
			length = handleline.GetLength();

			baseheight = handle1.Level.type == SectorLevelType.Ceiling ? handle1.Level.sector.CeilHeight : handle1.Level.sector.FloorHeight;
			baseheightoffset = 0.0;
		}

		private void CalculateBaseHeightOffset()
		{
			double right = Math.Cos(0.0);
			double left = Math.Cos(theta + offsetangle);
			double middle = Math.Cos(offsetangle);

			double radius = length / (middle - left);
			double leftdelimiter = Math.Cos(offsetangle + theta);
			double rightdelimiter = Math.Cos(offsetangle);

			double sectionstart = Math.Cos(offsetangle + theta) * radius;

			baseheightoffset = Math.Sqrt(radius * radius - sectionstart * sectionstart) * scale;
		}

		public void ApplySlope()
		{
			double right = Math.Cos(0.0);
			double left = Math.Cos(theta + offsetangle);
			double middle = Math.Cos(offsetangle);

			double radius = length / (middle - left);
			double leftdelimiter = Math.Cos(offsetangle + theta);
			double rightdelimiter = Math.Cos(offsetangle);

			double sectionstart = Math.Cos(offsetangle + theta) * radius;

			foreach (BaseVisualGeometrySector bvgs in sectors)
			{
				double u1 = 1.0;
				double u2 = 0.0;

				foreach (Sidedef sd in bvgs.Sector.Sides.Keys)
				{
					double intersection;
					double bla;

					if (!Line2D.GetIntersection(handleline.v1, handleline.v2, sd.Line.Line.v1.x, sd.Line.Line.v1.y, sd.Line.Line.v2.x, sd.Line.Line.v2.y, out bla, out intersection, false))
						continue;

					if (intersection < u1)
						u1 = intersection;
					if (intersection > u2)
						u2 = intersection;
				}

				if (u1 == u2)
				{
					if (u1 >= 0.5)
					{
						u1 = 1.0;
					}
					else
					{
						u2 = 0.0;
					}

				}

				double xpos1 = sectionstart + (u1 * length);
				double xpos2 = sectionstart + (u2 * length);
				double height1 = Math.Sqrt(radius * radius - xpos1 * xpos1) * scale;
				double height2 = Math.Sqrt(radius * radius - xpos2 * xpos2) * scale;

				if (double.IsNaN(height1))
					height1 = 0.0;

				if (double.IsNaN(height2))
					height2 = 0.0;

				height1 = height1 - baseheightoffset + baseheight;
				height2 = height2 - baseheightoffset + baseheight;

				height1 += heightoffset;
				height2 += heightoffset;

				double slopeangle = Vector2D.GetAngle(new Vector2D(xpos1, height1), new Vector2D(xpos2, height2));

				Plane plane = new Plane(new Vector3D(handleline.GetCoordinatesAt(u1), height1), handleline.GetAngle() + Angle2D.PIHALF, slopeangle, true);

				VisualSidedefSlope.ApplySlope(bvgs.Level, plane, mode);
				bvgs.Sector.UpdateSectorGeometry(true);

				//Debug.WriteLine(string.Format("sector: {0} | xpos1: {1}, height1: {2} | xpos2: {3}, height2: {4} | slope angle: {5}", bvgs.Sector.Sector.Index, xpos1, xpos2, height1, height2, slopeangle));
			}
		}
	}
}
