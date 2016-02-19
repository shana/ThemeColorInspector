using System;
using System.Windows.Media;

namespace SpoiledCat.Utils.Colors
{
    public class HSL
    {
        public HSL()
        {
            _h = 0;
            _s = 0;
            _l = 0;
        }

        double _h;
        double _s;
        double _l;

        public double H
        {
            get { return _h; }
            set
            {
                _h = value;
                _h = _h > 1 ? 1 : _h < 0 ? 0 : _h;
            }
        }

        public double S
        {
            get { return _s; }
            set
            {
                _s = value;
                _s = _s > 1 ? 1 : _s < 0 ? 0 : _s;
            }
        }

        public double L
        {
            get { return _l; }
            set
            {
                _l = value;
                _l = _l > 1 ? 1 : _l < 0 ? 0 : _l;
            }
        }
    }

    /// <summary>
    /// http://www.codeproject.com/Articles/19045/Manipulating-colors-in-NET-Part
    /// </summary>
    public struct CIEXYZ
    {
        /// <summary>
        /// Gets an empty CIEXYZ structure.
        /// </summary>
        public static readonly CIEXYZ Empty = new CIEXYZ();
        /// <summary>
        /// Gets the CIE D65 (white) structure.
        /// </summary>
        public static readonly CIEXYZ D65 = new CIEXYZ(0.9505, 1.0, 1.0890);

        double x;
        double y;
        double z;

        public static bool operator ==(CIEXYZ item1, CIEXYZ item2)
        {
            return (
                item1.X == item2.X
                && item1.Y == item2.Y
                && item1.Z == item2.Z
                );
        }

        public static bool operator !=(CIEXYZ item1, CIEXYZ item2)
        {
            return (
                item1.X != item2.X
                || item1.Y != item2.Y
                || item1.Z != item2.Z
                );
        }

        /// <summary>
        /// Gets or sets X component.
        /// </summary>
        public double X
        {
            get { return x; }
            set
            {
                x = (value > 0.9505) ? 0.9505 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets Y component.
        /// </summary>
        public double Y
        {
            get { return y; }
            set
            {
                y = (value > 1.0) ? 1.0 : ((value < 0) ? 0 : value);
            }
        }

        /// <summary>
        /// Gets or sets Z component.
        /// </summary>
        public double Z
        {
            get { return z; }
            set
            {
                z = (value > 1.089) ? 1.089 : ((value < 0) ? 0 : value);
            }
        }

        public CIEXYZ(double x, double y, double z)
        {
            this.x = (x > 0.9505) ? 0.9505 : ((x < 0) ? 0 : x);
            this.y = (y > 1.0) ? 1.0 : ((y < 0) ? 0 : y);
            this.z = (z > 1.089) ? 1.089 : ((z < 0) ? 0 : z);
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return (this == (CIEXYZ)obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }

    public struct CIELab
    {
        /// <summary>
        /// Gets an empty CIELab structure.
        /// </summary>
        public static readonly CIELab Empty = new CIELab();

        double l;
        double a;
        double b;

        /// <summary>
        /// Gets or sets L component.
        /// </summary>
        public double L
        {
            get { return l; }
            set { l = value; }
        }

        /// <summary>
        /// Gets or sets a component.
        /// </summary>
        public double A
        {
            get { return a; }
            set { a = value; }
        }

        /// <summary>
        /// Gets or sets a component.
        /// </summary>
        public double B
        {
            get { return b; }
            set { b = value; }
        }

        public CIELab(double l, double a, double b)
        {
            this.l = l;
            this.a = a;
            this.b = b;
        }

        public static bool operator ==(CIELab item1, CIELab item2)
        {
            return (
                item1.L == item2.L
                && item1.A == item2.A
                && item1.B == item2.B
                );
        }

        public static bool operator !=(CIELab item1, CIELab item2)
        {
            return (
                item1.L != item2.L
                || item1.A != item2.A
                || item1.B != item2.B
                );
        }


        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return (this == (CIELab)obj);
        }

        public override int GetHashCode()
        {
            return L.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
        }
    }

    public static class ColorExtensions
	{
        /// <summary>
        /// Sets the absolute brightness of a colour
        /// </summary>
        /// <param name="brightness">The luminance level to impose</param>
        /// <returns>an adjusted colour</returns>
        public static Color SetBrightness(this Color c, double brightness)
		{
			HSL hsl = c.ToHsl();
			hsl.L = brightness;
			return hsl.ToColor();
		}

		/// <summary>
		/// Modifies an existing brightness level
		/// </summary>
		/// <remarks>
		/// To reduce brightness use a number smaller than 1. To increase brightness use a number larger tnan 1
		/// </remarks>
		/// <param name="brightness">The luminance delta</param>
		/// <returns>An adjusted colour</returns>
		public static Color ModifyBrightness(this Color c, double brightness)
		{
			HSL hsl = c.ToHsl();
			hsl.L *= brightness;
			return hsl.ToColor();
		}

		/// <summary>
		/// Sets the absolute saturation level
		/// </summary>
		/// <remarks>Accepted values 0-1</remarks>
		/// <param name="Saturation">The saturation value to impose</param>
		/// <returns>An adjusted colour</returns>
		public static Color SetSaturation(this Color c, double Saturation)
		{
			HSL hsl = c.ToHsl();
			hsl.S = Saturation;
			return hsl.ToColor();
		}

		/// <summary>
		/// Modifies an existing Saturation level
		/// </summary>
		/// <remarks>
		/// To reduce Saturation use a number smaller than 1. To increase Saturation use a number larger tnan 1
		/// </remarks>
		/// <param name="Saturation">The saturation delta</param>
		/// <returns>An adjusted colour</returns>
		public static Color ModifySaturation(this Color c, double Saturation)
		{
			HSL hsl = c.ToHsl();
			hsl.S *= Saturation;
			return hsl.ToColor();
		}

		/// <summary>
		/// Sets the absolute Hue level
		/// </summary>
		/// <remarks>Accepted values 0-1</remarks>
		/// <param name="Hue">The Hue value to impose</param>
		/// <returns>An adjusted colour</returns>
		public static Color SetHue(this Color c, double Hue)
		{
			HSL hsl = c.ToHsl();
			hsl.H = Hue;
			return hsl.ToColor();
		}

		/// <summary>
		/// Modifies an existing Hue level
		/// </summary>
		/// <remarks>
		/// To reduce Hue use a number smaller than 1. To increase Hue use a number larger tnan 1
		/// </remarks>
		/// <param name="c">The original colour</param>
		/// <param name="Hue">The Hue delta</param>
		/// <returns>An adjusted colour</returns>
		public static Color ModifyHue(this Color c, double Hue)
		{
			HSL hsl = c.ToHsl();
			hsl.H *= Hue;
			return hsl.ToColor();
		}

		/// <summary>
		/// Converts a colour from HSL to RGB
		/// </summary>
		/// <remarks>Adapted from the algoritm in Foley and Van-Dam</remarks>
		/// <returns>A Color structure containing the equivalent RGB values</returns>
		public static Color ToColor(this HSL hsl)
		{
			double r = 0, g = 0, b = 0;
			double temp1, temp2;

			if (hsl.L == 0) {
				r = g = b = 0;
			} else {
				if (hsl.S == 0) {
					r = g = b = hsl.L;
				} else {
					temp2 = ((hsl.L <= 0.5) ? hsl.L * (1.0 + hsl.S) : hsl.L + hsl.S - (hsl.L * hsl.S));
					temp1 = 2.0 * hsl.L - temp2;

					double[] t3 = new double[] { hsl.H + 1.0 / 3.0, hsl.H, hsl.H - 1.0 / 3.0 };
					double[] clr = new double[] { 0, 0, 0 };
					for (int i = 0; i < 3; i++) {
						if (t3[i] < 0)
							t3[i] += 1.0;
						if (t3[i] > 1)
							t3[i] -= 1.0;

						if (6.0 * t3[i] < 1.0)
							clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
						else if (2.0 * t3[i] < 1.0)
							clr[i] = temp2;
						else if (3.0 * t3[i] < 2.0)
							clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
						else
							clr[i] = temp1;
					}
					r = clr[0];
					g = clr[1];
					b = clr[2];
				}
			}

			return Color.FromRgb((byte)(255 * r), (byte)(255 * g), (byte)(255 * b));

		}


		//
		/// <summary>
		/// Converts RGB to HSL
		/// </summary>
		/// <returns>An HSL value</returns>
		public static HSL ToHsl(this Color c)
		{
			HSL hsl = new HSL();

			hsl.H = c.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360
			hsl.L = c.GetBrightness();
			hsl.S = c.GetSaturation();

			return hsl;
		}

		public static Color ToComplement(this Color c)
		{
			HSL hsl = c.ToHsl();
			hsl.H += 0.5f;
			if (hsl.H > 1)
				hsl.H -= 1;
			Color ret = hsl.ToColor();
			if (ret == c)
				hsl.L = 1 - hsl.L;
			return hsl.ToColor();
		}

		public static CIEXYZ ToXYZ(this Color c)
		{
			// converts
			return new CIEXYZ(
				(c.ScR * 0.4124 + c.ScG * 0.3576 + c.ScB * 0.1805),
				(c.ScR * 0.2126 + c.ScG * 0.7152 + c.ScB * 0.0722),
				(c.ScR * 0.0193 + c.ScG * 0.1192 + c.ScB * 0.9505)
				);
		}

		/// <summary>
		/// XYZ to L*a*b* transformation function.
		/// </summary>
		static double XYZToLab(double t)
		{
			return ((t > 0.008856) ? Math.Pow(t, (1.0 / 3.0)) : (7.787 * t + 16.0 / 116.0));
		}

		/// <summary>
		/// Converts CIEXYZ to CIELab.
		/// </summary>
		public static CIELab ToLab(this Color c)
		{
            CIEXYZ xyz = c.ToXYZ();
            return new CIELab(
                116.0 * XYZToLab(xyz.Y / CIEXYZ.D65.Y) - 16,
                500.0 * (XYZToLab(xyz.X / CIEXYZ.D65.X) - XYZToLab(xyz.Y / CIEXYZ.D65.Y)),
                200.0 * (XYZToLab(xyz.Y / CIEXYZ.D65.Y) - XYZToLab(xyz.Z / CIEXYZ.D65.Z))
            );
		}

		public static float GetHue(this Color c)
		{
			var r = c.R;
			var g = c.G;
			var b = c.B;
			byte minval = (byte)Math.Min(r, Math.Min(g, b));
			byte maxval = (byte)Math.Max(r, Math.Max(g, b));

			if (maxval == minval)
				return 0.0f;

			float diff = (float)(maxval - minval);
			float rnorm = (maxval - r) / diff;
			float gnorm = (maxval - g) / diff;
			float bnorm = (maxval - b) / diff;

			float hue = 0.0f;
			if (r == maxval)
				hue = 60.0f * (6.0f + bnorm - gnorm);
			if (g == maxval)
				hue = 60.0f * (2.0f + rnorm - bnorm);
			if (b == maxval)
				hue = 60.0f * (4.0f + gnorm - rnorm);
			if (hue > 360.0f)
				hue = hue - 360.0f;

			return hue;

		}

		public static float GetBrightness(this Color c)
		{
			byte minval = Math.Min(c.R, Math.Min(c.G, c.B));
			byte maxval = Math.Max(c.R, Math.Max(c.G, c.B));

			return (float)(maxval + minval) / 510;
		}

		public static float GetSaturation(this Color c)
		{
			byte minval = (byte)Math.Min(c.R, Math.Min(c.G, c.B));
			byte maxval = (byte)Math.Max(c.R, Math.Max(c.G, c.B));

			if (maxval == minval)
				return 0.0f;

			int sum = maxval + minval;
			if (sum > 255)
				sum = 510 - sum;

			return (float)(maxval - minval) / sum;
		}

        public static double Distance(this Color lhs, Color rhs)
        {
            var lhslab = lhs.ToLab();
            var rhslab = rhs.ToLab();
            return Math.Sqrt(
                Math.Pow(lhslab.L - rhslab.L, 2) +
                Math.Pow(lhslab.A - rhslab.A, 2) +
                Math.Pow(lhslab.B - rhslab.B, 2)
                );
        }
	}
}

