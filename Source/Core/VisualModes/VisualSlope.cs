using System;
using CodeImp.DoomBuilder.Geometry;
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

		public void SetPosition(Vector3D pos, Geometry.Plane plane, float angle)
		{

			Matrix translate = Matrix.Translation(pos.x, pos.y, pos.z);
			Matrix rotate = Matrix.RotationZ(angle);
			Vector3 v = new Vector3(plane.Normal.x, plane.Normal.y, plane.Normal.z);
			// Matrix rotate = Matrix.RotationAxis(v, angle);
			//position = Matrix.Multiply(rotate, translate);
			position = Matrix.Multiply(rotate, translate);
		}

		#endregion
	}
}