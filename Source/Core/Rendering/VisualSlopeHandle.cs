#region ================== Namespaces

using System;

#endregion

namespace CodeImp.DoomBuilder.Rendering
{
	internal sealed class VisualSlopeHandle : IDisposable, IRenderResource
	{
		#region ================== Variables

		private VertexBuffer geometry;
		private bool isdisposed;

		#endregion

		#region ================== Properties

		public VertexBuffer Geometry { get { return geometry; } }

		#endregion

		#region ================== Constructor / Disposer

		public VisualSlopeHandle()
		{
			// Create geometry
			ReloadResource();

			// Register as source
			General.Map.Graphics.RegisterResource(this);
		}

		public void Dispose()
		{
			// Not already disposed?
			if (!isdisposed)
			{
				if (geometry != null)
					geometry.Dispose();

				// Unregister resource
				General.Map.Graphics.UnregisterResource(this);

				// Done
				isdisposed = true;
			}
		}

		#endregion

		#region ================== Methods

		// This is called resets when the device is reset
		// (when resized or display adapter was changed)
		public void ReloadResource()
		{
			WorldVertex v0 = new WorldVertex(0.0f, -8.0f, 0.1f);
			WorldVertex v1 = new WorldVertex(0.0f, 0.0f, 0.1f);
			WorldVertex v2 = new WorldVertex(1.0f, 0.0f, 0.1f);
			WorldVertex v3 = new WorldVertex(1.0f, -8.0f, 0.1f);

			v1.c = v2.c = PixelColor.INT_WHITE;
			v0.c = v3.c = PixelColor.INT_WHITE_NO_ALPHA;

			WorldVertex[] vertices = new[]
			{
				v0, v1, v2,
				v0, v2, v3
			};

			geometry = new VertexBuffer();
			General.Map.Graphics.SetBufferData(geometry, vertices);
		}

		// This is called before a device is reset
		// (when resized or display adapter was changed)
		public void UnloadResource()
		{
			if (geometry != null)
				geometry.Dispose();

			geometry = null;
		}

		#endregion
	}
}