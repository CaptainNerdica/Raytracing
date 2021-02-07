using Raytracing.Import;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raytracing
{
	[JsonConverter(typeof(ColorConverter))]
	public readonly struct Color : IEquatable<Color>, IFormattable
	{
		public readonly float R;
		public readonly float G;
		public readonly float B;

		public static Color Black => default;
		public static Color Red => new Color(1.0f, 0.0f, 0.0f);
		public static Color Yellow => new Color(1.0f, 1.0f, 0.0f);
		public static Color Green => new Color(0.0f, 1.0f, 0.0f);
		public static Color Cyan => new Color(0.0f, 1.0f, 1.0f);
		public static Color Blue => new Color(0.0f, 0.0f, 1.0f);
		public static Color Magenta => new Color(1.0f, 0.0f, 1.0f);
		public static Color White => new Color(1.0f, 1.0f, 1.0f);
		public static Color Gray => new Color(0.5f, 0.5f, 0.5f);


		public Color(float value) : this(value, value, value) { }
		public Color(float red, float green, float blue)
		{
			R = Math.Clamp(red, 0, float.MaxValue);
			G = Math.Clamp(green, 0, float.MaxValue);
			B = Math.Clamp(blue, 0, float.MaxValue);
		}
		public Color(byte red, byte green, byte blue, float gamma)
		{
			R = MathF.Pow((float)red / byte.MaxValue, gamma);
			G = MathF.Pow((float)green / byte.MaxValue, gamma);
			B = MathF.Pow((float)blue / byte.MaxValue, gamma);
		}
		public Color(System.Drawing.Color color, float gamma) : this(color.R, color.G, color.B, gamma)
		{ }


		public static Color Blend(ReadOnlySpan<Color> colors)
		{
			Color blend = Black;
			for (int i = 0; i < colors.Length; ++i)
				blend += colors[i] / colors.Length;
			return blend;
		}
		public static Color Blend(Color[] colors) => Blend(colors, 0, colors.Length);
		public static Color Blend(Color[] colors, int startIndex, int count)
		{
			Color blend = Black;
			for (int i = 0; i < count; ++i)
				blend += colors[i + startIndex] / count;
			return blend;
		}
		public static Color Clamp(Color color, Color min, Color max) => new Color(Math.Clamp(color.R, min.R, max.R), Math.Clamp(color.G, min.G, max.G), Math.Clamp(color.B, min.B, max.B));
		public static Color InverseGamma(Color color, float gamma) => new Color(MathF.Pow(color.R, gamma), MathF.Pow(color.G, gamma), MathF.Pow(color.B, gamma));
		public static Color Gamma(Color color, float gamma) => new Color(MathF.Pow(color.R, 1 / gamma), MathF.Pow(color.G, 1 / gamma), MathF.Pow(color.B, 1 / gamma));
		public static Color Lerp(Color start, Color end, float amount) => new Color(MathExt.Lerp(start.R, end.R, amount), MathExt.Lerp(start.G, end.G, amount), MathExt.Lerp(start.B, end.B, amount));
		public static Color Max(Color color1, Color color2) => new Color(Math.Max(color1.R, color2.R), Math.Max(color1.G, color2.G), Math.Max(color1.B, color2.B));
		public static Color Min(Color color1, Color color2) => new Color(Math.Min(color1.R, color2.R), Math.Min(color1.G, color2.G), Math.Min(color1.B, color2.B));

		public override bool Equals(object? obj) => obj is Color c && Equals(c);
		public bool Equals(Color other) => this == other;
		public override int GetHashCode() => HashCode.Combine(R, G, B);
		public override readonly string ToString() => ToString("G", CultureInfo.CurrentCulture);
		public readonly string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);
		public readonly string ToString(string? format, IFormatProvider? formatProvider)
		{
			StringBuilder sb = new StringBuilder();
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			sb.Append('(');
			sb.Append(R.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(' ');
			sb.Append(G.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(' ');
			sb.Append(B.ToString(format, formatProvider));
			sb.Append(')');
			return sb.ToString();
		}

		public static bool operator ==(Color left, Color right) => left.R == right.R && left.G == right.G && left.B == right.B;
		public static bool operator !=(Color left, Color right) => !(left == right);

		public static Color operator -(Color color) => new Color(1 - color.R, 1 - color.G, 1 - color.B);
		public static Color operator +(Color left, Color right) => new Color(left.R + right.R, left.G + right.G, left.B + right.B);
		public static Color operator -(Color left, Color right) => new Color(left.R - right.R, left.G - right.G, left.B - right.B);
		public static Color operator *(Color color, float value) => new Color(color.R * value, color.G * value, color.B * value);
		public static Color operator *(float value, Color color) => new Color(color.R * value, color.G * value, color.B * value);
		public static Color operator *(Color color1, Color color2) => new Color(color2.R * color1.R, color2.G * color1.G, color2.B * color1.B);
		public static Color operator /(Color color, float value) => new Color(color.R / value, color.G / value, color.B / value);
		public static Color operator /(Color color1, Color color2) => new Color(color1.R / color2.R, color1.G / color2.G, color1.B / color2.B);

		public static explicit operator System.Drawing.Color(Color color) => System.Drawing.Color.FromArgb(Math.Clamp((byte)(color.R * byte.MaxValue), (byte)0, (byte)255), Math.Clamp((byte)(color.G * byte.MaxValue), (byte)0, (byte)255), Math.Clamp((byte)(color.B * byte.MaxValue), (byte)0, (byte)255));
		public static implicit operator Color(System.Drawing.Color color) => new Color(color, 2.2f);

		public static implicit operator Color((float r, float g, float b) tuple) => new Color(tuple.r, tuple.g, tuple.b);
		public static implicit operator (float r, float g, float b)(Color color) => (color.R, color.G, color.B);

		public static implicit operator Color((byte r, byte g, byte b) tuple) => new Color(tuple.r, tuple.g, tuple.b);

		public static implicit operator Vector3(Color color) => new Vector3(color.R, color.G, color.B);
		public static implicit operator Color(Vector3 vector) => new Color(vector.X, vector.Y, vector.Z);
	}
}
