
#region ================== Copyright (c) 2007 Pascal vd Heiden

/*
 * Copyright (c) 2007 Pascal vd Heiden, www.codeimp.com
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

#region ================== Namespaces

using System;
using System.Windows.Forms;
using CodeImp.DoomBuilder.Geometry;
using System.Runtime.InteropServices;

#endregion

namespace CodeImp.DoomBuilder.Actions
{
	internal class MouseInput : IDisposable
	{
		#region ================== Variables

		// Mouse input
		private RawMouse mouse;
		
		#endregion

		#region ================== Constructor / Disposer

		// Constructor
		public MouseInput(Control source)
		{
			// Start mouse input
			mouse = new RawMouse(source);

			// We have no destructor
			GC.SuppressFinalize(this);
		}

		// Disposer
		public void Dispose()
		{
			if(mouse != null)
			{
				mouse.Dispose();
				mouse = null;
			}
		}

		#endregion

		#region ================== Methods

		#endregion

		#region ================== Processing

		// This processes the input
		public Vector2D Process()
		{
			MouseState ms = mouse.Poll();

			// Calculate changes depending on sensitivity
			float changex = ms.X * General.Settings.VisualMouseSensX * General.Settings.MouseSpeed * 0.01f;
			float changey = ms.Y * General.Settings.VisualMouseSensY * General.Settings.MouseSpeed * 0.01f;

			return new Vector2D(changex, changey);
		}

		#endregion
	}

    public struct MouseState
    {
        public MouseState(float x, float y) { X = x; Y = y; }
        public float X { get; }
        public float Y { get; }
    }

    public class RawMouse
    {
        public RawMouse(System.Windows.Forms.Control control)
        {
            Handle = RawMouse_New(control.Handle);
            if (Handle == IntPtr.Zero)
                throw new Exception("VertexDeclaration_New failed");
        }

        ~RawMouse()
        {
            Dispose();
        }

        public MouseState Poll()
        {
            return new MouseState(RawMouse_GetX(Handle), RawMouse_GetY(Handle));
        }

        public bool Disposed { get { return Handle == IntPtr.Zero; } }

        public void Dispose()
        {
            if (!Disposed)
            {
                RawMouse_Delete(Handle);
                Handle = IntPtr.Zero;
            }
        }

        internal IntPtr Handle;

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr RawMouse_New(IntPtr windowHandle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern void RawMouse_Delete(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern float RawMouse_GetX(IntPtr handle);

        [DllImport("BuilderNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern float RawMouse_GetY(IntPtr handle);
    }
}
