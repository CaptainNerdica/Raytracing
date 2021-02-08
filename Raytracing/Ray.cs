using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing
{
	public readonly struct Ray : IFormattable
	{
		public Point Origin { get; }
		public Vector3 Direction { get; }

		public Ray(Point origin, Vector3 direction)
		{
			Origin = origin;
			Direction = direction.Normalized();
		}

		public Point PointAtDistance(float distance) => Origin + Direction * distance;

		public override readonly string ToString() => ToString("G", CultureInfo.CurrentCulture);
		public readonly string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);
		public readonly string ToString(string? format, IFormatProvider? formatProvider)
		{
			StringBuilder sb = new StringBuilder();
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			sb.Append('(');
			sb.Append(Origin.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(Direction.ToString(format, formatProvider));
			sb.Append(')');
			return sb.ToString();
		}
	}
}
