using Raytracing.Import;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raytracing
{
	[JsonConverter(typeof(Vector2Converter))]
	public readonly struct Vector2 : IEquatable<Vector2>, IFormattable
	{
		public readonly float X;
		public readonly float Y;

		public readonly Vector2 Normalized { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Normalize(this); }
		public readonly float Length { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Magnitude(this); }
		public readonly float LengthSquared { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => MagnitudeSquared(this); }

		public static Vector2 UnitX { get; } = new Vector2(1, 0);
		public static Vector2 UnitY { get; } = new Vector2(0, 1);
		public static Vector2 One { get; } = new Vector2(1, 1);
		public static Vector2 Zero { get; } = default;

		public Vector2(float value) : this(value, value) { }
		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Abs(Vector2 vector) => new Vector2(Math.Abs(vector.X), Math.Abs(vector.Y));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max) => Min(Max(value, min), max);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(Vector2 left, Vector2 right)
		{
			Vector2 diff = left - right;
			return MathF.Sqrt(Dot(diff, diff));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared(Vector2 left, Vector2 right)
		{
			Vector2 diff = left - right;
			return Dot(diff, diff);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(Vector2 left, Vector2 right) => left.X * right.X + left.Y * right.Y;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Lerp(Vector2 start, Vector2 end, float amount) => new Vector2(MathExt.Lerp(start.X, end.X, amount), MathExt.Lerp(start.Y, end.Y, amount));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Magnitude(Vector2 vector) => MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MagnitudeSquared(Vector2 vector) => Dot(vector, vector);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Max(Vector2 value1, Vector2 value2) => new Vector2(value1.X > value2.X ? value1.X : value2.X, value1.Y > value2.Y ? value1.Y : value2.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Min(Vector2 value1, Vector2 value2) => new Vector2(value1.X < value2.X ? value1.X : value2.X, value1.Y < value2.Y ? value1.Y : value2.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Normalize(Vector2 vector) => vector / MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Project(Vector2 vector1, Vector2 vector2) => (vector1.X * vector2.X + vector1.Y * vector2.Y) * vector2;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 RandomInUnit()
		{
			Vector2 v;
			Random r = new Random();
			while (true)
			{
				v = new Vector2(2 * (float)r.NextDouble() - 1, 2 * (float)r.NextDouble() - 1);
				if (v.LengthSquared < 1)
					return v;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Reflect(Vector2 vector, Vector2 normal) => vector - normal * (normal.X * vector.X + normal.Y * vector.Y) * 2;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Reject(Vector2 vector1, Vector2 vector2) => vector1 - (vector1.X * vector2.X + vector1.Y * vector2.Y) * vector2;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Rotate(Vector2 vector, float angle)
		{
			float r = angle * MathExt.DegreesToRadians;
			float cr = MathF.Cos(r);
			float sr = MathF.Sin(r);
			return new Vector2(vector.X * cr - vector.Y * sr, vector.X * sr + vector.Y * cr);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 SquareRoot(Vector2 vector) => new Vector2(MathF.Sqrt(vector.X), MathF.Sqrt(vector.Y));

		public override int GetHashCode() => HashCode.Combine(X, Y);
		public override bool Equals(object? obj) => obj is Vector2 v && Equals(v);
		public bool Equals(Vector2 other) => this == other;

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
			sb.Append('>');
			return sb.ToString();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator +(Vector2 value) => new Vector2(value.X, value.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 value) => new Vector2(-value.X, -value.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator +(Vector2 left, Vector2 right) => new Vector2(left.X + right.X, left.Y + right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator -(Vector2 left, Vector2 right) => new Vector2(left.X - right.X, left.Y - right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 left, Vector2 right) => new Vector2(left.X * right.X, left.Y * right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(float left, Vector2 right) => new Vector2(left * right.X, left * right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Vector2 left, float right) => new Vector2(left.X * right, left.Y * right);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 left, Vector2 right) => new Vector2(left.X / right.X, left.Y / right.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator /(Vector2 left, float right) => new Vector2(left.X / right, left.Y / right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector2 left, Vector2 right) => left.X == right.X && left.Y == right.Y;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2((float x, float y) tuple) => new Vector2(tuple.x, tuple.y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator (float x, float y)(Vector2 vector) => (vector.X, vector.Y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator System.Numerics.Vector2(Vector2 vector) => new System.Numerics.Vector2((float)vector.X, (float)vector.Y);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(System.Numerics.Vector2 vector) => new Vector2(vector.X, vector.Y);

	}
}
