using System;
using CodeImp.DoomBuilder.Geometry;
using CodeImp.DoomBuilder.Map;
using CodeImp.DoomBuilder.Rendering;

namespace CodeImp.DoomBuilder.VisualModes
{
	public abstract class VisualSlope : IVisualPickable, IDisposable
	{
		#region ================== Variables

		// Disposing
		private bool isdisposed;

		// Selected?
		protected bool selected;

		// Pivot?
		protected bool pivot;

		// Smart Pivot?
		protected bool smartpivot;

		// Was changed?
		private bool changed;

		protected float length;

		private Matrix position;


		#endregion

		#region ================== Properties

		/// <summary>
		/// Selected or not? This is only used by the core to determine what color to draw it with.
		/// </summary>
		public bool Selected { get { return selected; } set { selected = value; } }

		/// <summary>
		/// Pivot or not? This is only used by the core to determine what color to draw it with.
		/// </summary>
		public bool Pivot { get { return pivot; } set { pivot = value; } }

		/// <summary>
		/// Disposed or not?
		/// </summary>
		public bool IsDisposed { get { return isdisposed; } }

		public bool SmartPivot { get { return smartpivot; } set { smartpivot = value; } }

		public bool Changed { get { return changed; } set { changed = value; } }

		public float Length { get { return length; } }

		public Matrix Position { get { return position; } }

		#endregion

		#region ================== Constructor / Destructor

		public VisualSlope()
		{
			// Register as resource
			// General.Map.Graphics.RegisterResource(this);

			pivot = false;
			smartpivot = false;
		}

		// Disposer
		public virtual void Dispose()
		{
			// Not already disposed?
		}

		#endregion

		#region ================== Methods

		// This is called before a device is reset (when resized or display adapter was changed)
		public void UnloadResource()
		{
		}

		// This is called resets when the device is reset
		// (when resized or display adapter was changed)
		public void ReloadResource()
		{
		}

		/// <summary>
		/// This is called when the thing must be tested for line intersection. This should reject
		/// as fast as possible to rule out all geometry that certainly does not touch the line.
		/// </summary>
		public virtual bool PickFastReject(Vector3D from, Vector3D to, Vector3D dir)
		{
			return true;
		}

		/// <summary>
		/// This is called when the thing must be tested for line intersection. This should perform
		/// accurate hit detection and set u_ray to the position on the ray where this hits the geometry.
		/// </summary>
		public virtual bool PickAccurate(Vector3D from, Vector3D to, Vector3D dir, ref float u_ray)
		{
			return true;
		}

		public virtual void Update() {}

		private Vector3D multi(Matrix m, Vector3D p)
		{
			return new Vector3D(
				m.M11 * p.x + m.M12 * p.y + m.M13 * p.z + m.M14,
				m.M21 * p.x + m.M22 * p.y + m.M23 * p.z + m.M24,
				m.M31 * p.x + m.M32 * p.y + m.M33 * p.z + m.M34
			);
		}

		public void SetPosition(Line2D line, Plane plane)
		{
			//Matrix translate = Matrix.Translation(pos.x, pos.y, pos.z);
			//Matrix rotate = Matrix.RotationZ(angle);
			//Matrix planerotate = Matrix.LookAt(RenderDevice.V3(v1), RenderDevice.V3(v2x), RenderDevice.V3(new Vector3D(0.0f, 0.0f, 1.0f)));
			/*
			float[] normal = new float[] { plane.Normal.x, plane.Normal.y, plane.Normal.z };
			float[] vec2 = new float[] { 0.0f, 0.0f, 0.0f };
			int imin = 0;

			for (int i = 0; i < 3; ++i)
				if (Math.Abs(normal[i]) < Math.Abs(normal[imin]))
					imin = i;
			
			float dt = normal[imin];

			vec2[imin] = 1;

			for(int i=0;i<3;i++)
				vec2[i] -= dt * normal[i];

			Vector3D v2 = new Vector3D(vec2[0], vec2[1], vec2[2]);
			*/
			Vector3D line_vector = Vector3D.CrossProduct(line.GetDelta().GetNormal(), plane.Normal);
			Vector3D new_vector = Vector3D.CrossProduct(plane.Normal, line_vector);

			Matrix m = Matrix.Null;

			m.M11 = new_vector.x;
			m.M12 = new_vector.y;
			m.M13 = new_vector.z;

			m.M21 = line_vector.x;
			m.M22 = line_vector.y;
			m.M23 = line_vector.z;

			m.M31 = plane.Normal.x;
			m.M32 = plane.Normal.y;
			m.M33 = plane.Normal.z;

			/*
			m.M11 = new_vector.x;
			m.M21 = new_vector.y;
			m.M31 = new_vector.z;

			m.M12 = line_vector.x;
			m.M22 = line_vector.y;
			m.M32 = line_vector.z;

			m.M13 = plane.Normal.x;
			m.M23 = plane.Normal.y;
			m.M33 = plane.Normal.z;
			*/

			m.M44 = 1.0f;

			Vector3D tp = new Vector3D(line.v1, plane.GetZ(line.v1));

			//Matrix xrotate = Matrix.RotationX(90.0f);
			//Vector3 v = new Vector3(plane.Normal.x, plane.Normal.y, plane.Normal.z);
			// Matrix rotate = Matrix.RotationAxis(v, angle);
			//position = Matrix.Multiply(translate, planerotate);
			// position = Matrix.Multiply(m, Matrix.Multiply(rotation, Matrix.Translation(RenderDevice.V3(v1))));
			//position = m;
			position = Matrix.Multiply(m, Matrix.Translation(RenderDevice.V3(tp)));

			/*
			if (ld.Index == 0)
			{
				DebugConsole.WriteLine("Linedef: " + ld.ToString() + " | Plane normal: " + plane.Normal.ToString() + " | line_vector: " + line_vector.ToString() + " | new_vector: " + new_vector.ToString());

				Vector3D mp = multi(position, new Vector3D(0.0f, 0.0f, 0.0f));
				DebugConsole.WriteLine(multi(position, mp).ToString());
				mp = multi(position, new Vector3D(16.0f, 0.0f, 0.0f));
				DebugConsole.WriteLine(multi(position, mp).ToString());
				mp = multi(position, new Vector3D(16.0f, 32.0f, 0.0f));
				DebugConsole.WriteLine(multi(position, mp).ToString());
				mp = multi(position, new Vector3D(0.0f, 32.0f, 0.0f));
				DebugConsole.WriteLine(multi(position, mp).ToString());
			}
			*/
		}

		#endregion
	}
}