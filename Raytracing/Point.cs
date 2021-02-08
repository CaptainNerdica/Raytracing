using Raytracing.Import;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raytracing
{
	[JsonConverter(typeof(PointConverter))]
	public readonly struct Point : IEquatable<Point>, IFormattable
	{
		public readonly float X;
		public readonly float Y;
		public readonly float Z;

		public static Point Zero { get; } = default;

		public Point(float value) : this(value, value, value) { }
		public Point(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Coplanar(Point point0, Point point1, Point point2, Point point3, float tolerance = 0.0000001f) => Vector3.Dot(point3 - point0, Vector3.Cross(point1 - point0, point2 - point0)) + tolerance < 2 * tolerance;

		public override int GetHashCode() => HashCode.Combine(X, Y, Z);
		public override bool Equals(object? obj) => obj is Point p && Equals(p);
		public bool Equals(Point other) => this == other;

		public override readonly string ToString() => ToString("G", CultureInfo.CurrentCulture);
		public readonly string ToString(string? format) => ToString(format, CultureInfo.CurrentCulture);
		public readonly string ToString(string? format, IFormatProvider? formatProvider)
		{
			StringBuilder sb = new StringBuilder();
			string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;
			sb.Append('<');
			sb.Append(X.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(' ');
			sb.Append(Y.ToString(format, formatProvider));
			sb.Append(separator);
			sb.Append(' ');
			sb.Append(Z.ToString(format, formatProvider));
			sb.Append('>');
			return sb.ToString();
		}

		public static Point operator +(Point point, Vector3 vector) => new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
		public static Point operator -(Point point, Vector3 vector) => new Point(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);
		public static Vector3 operator -(Point point1, Point point2) => new Vector3(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
		public static Point operator *(Point point, float value) => new Vector3(point.X * value, point.Y * value, point.Z * value);
		public static Point operator /(Point point, float value) => new Vector3(point.X / value, point.Y / value, point.Z / value);
		public static bool operator ==(Point left, Point right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z;
		public static bool operator !=(Point left, Point right) => !(left == right);

		public static implicit operator Point((float x, float y, float z) tuple) => new Point(tuple.x, tuple.y, tuple.z);
		public static implicit operator Point(Vector3 vector) => new Point(vector.X, vector.Y, vector.Z);
		public static implicit operator Vector3(Point point) => new Vector3(point.X, point.Y, point.Z);
	}
}
