using System;
using CodeImp.DoomBuilder.Rendering;

namespace CodeImp.DoomBuilder.Geometry
{
	public static class InterpolationTools
	{
		public enum Mode
		{
			LINEAR,
			EASE_IN_OUT_SINE,
			EASE_IN_SINE,
			EASE_OUT_SINE,
		}

		public static double Interpolate(double val1, double val2, double delta, Mode mode)
		{
			switch(mode)
			{
				case Mode.LINEAR: return Linear(val1, val2, delta);
				case Mode.EASE_IN_SINE: return EaseInSine(val1, val2, delta);
				case Mode.EASE_OUT_SINE: return EaseOutSine(val1, val2, delta);
				case Mode.EASE_IN_OUT_SINE: return EaseInOutSine(val1, val2, delta);
				default: throw new NotImplementedException("InterpolationTools.Interpolate: \"" + mode + "\" mode is not supported!");
			}
		}
		
		//Based on Robert Penner's original easing equations (http://www.robertpenner.com/easing/)
		public static float Linear(float val1, float val2, float delta)
		{
			return delta * val2 + (1.0f - delta) * val1;
		}

		//Based on Robert Penner's original easing equations (http://www.robertpenner.com/easing/)
		public static double Linear(double val1, double val2, double delta)
		{
			return delta * val2 + (1.0f - delta) * val1;
		}

		/**
		 * Easing equation function for a sinusoidal (sin(t)) easing in: accelerating from zero velocity.
		 */
		public static double EaseInSine(double val1, double val2, double delta) 
		{
			double f_val1 = val1;
			double f_val2 = val2 - f_val1;
			return -f_val2 * Math.Cos(delta * Angle2D.PIHALF) + f_val2 + f_val1;
		}

		/**
		 * Easing equation function for a sinusoidal (sin(t)) easing out: decelerating from zero velocity.
		 */
		public static double EaseOutSine(double val1, double val2, double delta) 
		{
			return (val2 - val1) * Math.Sin(delta * Angle2D.PIHALF) + val1;
		}

		/**
		 * Easing equation function for a sinusoidal (sin(t)) easing in/out: acceleration until halfway, then deceleration.
		 */
		public static double EaseInOutSine(double val1, double val2, double delta)
		{
			return -(val2 - val1) / 2 * (Math.Cos(Angle2D.PI * delta) - 1) + val1;
		}

		//mxd
		public static PixelColor InterpolateColor(PixelColor c1, PixelColor c2, double delta)
		{
			double invdelta = 1.0f - delta;
			byte a = (byte)(c1.a * invdelta + c2.a * delta);
			byte r = (byte)(c1.r * invdelta + c2.r * delta);
			byte g = (byte)(c1.g * invdelta + c2.g * delta);
			byte b = (byte)(c1.b * invdelta + c2.b * delta);
			return new PixelColor(a, r, g, b);
		}

		//mxd
		public static PixelColor InterpolateColor(PixelColor c1, PixelColor c2, double delta, Mode mode)
		{
			byte a = (byte)Math.Round(Interpolate(c1.a, c2.a, delta, mode));
			byte r = (byte)Math.Round(Interpolate(c1.r, c2.r, delta, mode));
			byte g = (byte)Math.Round(Interpolate(c1.g, c2.g, delta, mode));
			byte b = (byte)Math.Round(Interpolate(c1.b, c2.b, delta, mode));
			return new PixelColor(a, r, g, b);
		}
	}
}
