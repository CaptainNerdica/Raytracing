using System;
using System.Text;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text.Json.Serialization;
using Raytracing.Import;
using System.Runtime.InteropServices;

namespace Raytracing
{
	[JsonConverter(typeof(Vector3Converter))]
	[StructLayout(LayoutKind.Sequential, Size = 16)]
	public readonly struct Vector3 : IEquatable<Vector3>, IFormattable
	{
		public readonly float X;
		public readonly float Y;
		public readonly float Z;

		public readonly Vector3 Normalized { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Normalize(this); }
		public readonly float Length { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => Magnitude(this); }
		public readonly float LengthSquared { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => MagnitudeSquared(this); }

		public static Vector3 UnitX { get; } = new Vector3(1, 0, 0);
		public static Vector3 UnitY { get; } = new Vector3(0, 1, 0);
		public static Vector3 UnitZ { get; } = new Vector3(0, 0, 1);
		public static Vector3 One { get; } = new Vector3(1, 1, 1);
		public static Vector3 Zero { get; } = default;

		public Vector3(float value) : this(value, value, value) { }
		public Vector3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Abs(Vector3 vector) => new Vector3(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z));
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max) => Min(Max(value, min), max);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Cross(Vector3 left, Vector3 right) => new Vector3(left.Y * right.Z - left.Z * right.Y, left.Z * right.X - left.X * right.Z, left.X * right.Y - left.Y * right.X);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(Vector3 left, Vector3 right)
		{
			Vector3 diff = left - right;
			return MathF.Sqrt(Dot(diff, diff));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared(Vector3 left, Vector3 right)
		{
			Vector3 diff = left - right;
			return Dot(diff, diff);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe float Dot(Vector3 left, Vector3 right) => left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SkipLocalsInit]
		public static unsafe Vector3 Lerp(Vector3 start, Vector3 end, float amount)
		{
			Vector3 f;
			Vector128<float> startVec = Sse.LoadVector128((float*)&start);
			Vector128<float> endVec = Sse.LoadVector128((float*)&end);
			Vector128<float> amountVec = Vector128.Create(amount);
			Vector128<float> mAmountVec = Vector128.Create(1 - amount);
			Vector128<float> ev = Sse.Multiply(endVec, amountVec);
			Vector128<float> sv = Sse.Multiply(startVec, mAmountVec);
			Vector128<float> v = Sse.Add(sv, ev);
			Sse.StoreAligned((float*)&f, v);
			return f;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Magnitude(Vector3 vector) => MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MagnitudeSquared(Vector3 vector) => Dot(vector, vector);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Max(Vector3 value1, Vector3 value2) => new Vector3(value1.X > value2.X ? value1.X : value2.X, value1.Y > value2.Y ? value1.Y : value2.Y, value1.Z > value2.Z ? value1.Z : value2.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Min(Vector3 value1, Vector3 value2) => new Vector3(value1.X < value2.X ? value1.X : value2.X, value1.Y < value2.Y ? value1.Y : value2.Y, value1.Z < value2.Z ? value1.Z : value2.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Normalize(Vector3 vector) => vector / MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Project(Vector3 vector1, Vector3 vector2) => (vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z) * vector2;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 RandomInUnit()
		{
			Vector3 v;
			while (true)
			{
				v = new Vector3(2 * (float)ThreadSafeRandom.NextDouble() - 1, 2 * (float)ThreadSafeRandom.NextDouble() - 1, 2 * (float)ThreadSafeRandom.NextDouble() - 1);
				if (v.LengthSquared < 1)
					return v;
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Reflect(Vector3 vector, Vector3 normal) => vector - normal * (normal.X * vector.X + normal.Y * vector.Y + normal.Z * vector.Z) * 2;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Reject(Vector3 vector1, Vector3 vector2) => vector1 - (vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z) * vector2;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Rotate(Vector3 vector, Vector3 about, float angle)
		{
			float r = angle * MathExt.DegreesToRadians;
			Vector3 normal = about.Normalized;
			float cr = MathF.Cos(r);
			float sr = MathF.Sin(r);
			float dot = normal.X * vector.X + normal.Y * vector.Y + normal.Z * vector.Z;
			float mcr = 1 - cr;
			return
				new Vector3(
					vector.X * cr + sr * (normal.Y * vector.Z - normal.Z * vector.Y) + normal.X * dot * mcr,
					vector.Y * cr + sr * (normal.Z * vector.X - normal.X * vector.Z) + normal.Y * dot * mcr,
					vector.Z * cr + sr * (normal.X * vector.Y - normal.Y * vector.X) + normal.Z * dot * mcr);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 SquareRoot(Vector3 vector) => new Vector3(MathF.Sqrt(vector.X), MathF.Sqrt(vector.Y), MathF.Sqrt(vector.Z));

		public override bool Equals(object? other) => other is Vector3 v && Equals(v);
		public bool Equals(Vector3 other) => this == other;
		public override int GetHashCode() => HashCode.Combine(X, Y, Z);
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 operator +(Vector3 vector) => vector;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SkipLocalsInit]
		public static unsafe Vector3 operator -(Vector3 value)
		{
			Vector3 v;
			Vector128<float> l = Sse.LoadVector128((float*)&value);
			Sse.Store((float*)&v, Sse.Subtract(Vector128<float>.Zero, l));
			return v;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SkipLocalsInit]
		public static unsafe Vector3 operator +(Vector3 left, Vector3 right)
		{
			Vector3 v;
			Vector128<float> l = Sse.LoadVector128((float*)&left);
			Vector128<float> r = Sse.LoadVector128((float*)&right);
			Sse.Store((float*)&v, Sse.Add(l, r));
			return v;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SkipLocalsInit]
		public static unsafe Vector3 operator -(Vector3 left, Vector3 right)
		{
			Vector3 v;
			Vector128<float> l = Sse.LoadVector128((float*)&left);
			Vector128<float> r = Sse.LoadVector128((float*)&right);
			Sse.Store((float*)&v, Sse.Subtract(l, r));
			return v;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SkipLocalsInit]
		public static unsafe Vector3 operator *(Vector3 left, Vector3 right)
		{
			Vector3 v;
			Vector128<float> l = Sse.LoadVector128((float*)&left);
			Vector128<float> r = Sse.LoadVector128((float*)&right);
			Sse.Store((float*)&v, Sse.Multiply(l, r));
			return v;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SkipLocalsInit]
		public static unsafe Vector3 operator *(float left, Vector3 right)
		{
			Vector3 v;
			Vector128<float> l = Vector128.Create(left);
			Vector128<float> r = Sse.LoadVector128((float*)&right);
			Sse.Store((float*)&v, Sse.Multiply(l, r));
			return v;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SkipLocalsInit]
		public static unsafe Vector3 operator *(Vector3 left, float right)
		{
			Vector3 v;
			Vector128<float> l = Sse.LoadVector128((float*)&left);
			Vector128<float> r = Vector128.Create(right);
			Sse.Store((float*)&v, Sse.Multiply(l, r));
			return v;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SkipLocalsInit]
		public static unsafe Vector3 operator /(Vector3 left, Vector3 right)
		{
			Vector3 v;
			Vector128<float> l = Sse.LoadVector128((float*)&left);
			Vector128<float> r = Sse.LoadVector128((float*)&right);
			Sse.Store((float*)&v, Sse.Divide(l, r));
			return v;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SkipLocalsInit]
		public static unsafe Vector3 operator /(Vector3 left, float right)
		{
			Vector3 v;
			Vector128<float> l = Sse.LoadVector128((float*)&left);
			Vector128<float> r = Vector128.Create(right);
			Sse.Store((float*)&v, Sse.Divide(l, r));
			return v;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3 left, Vector3 right) => left.X == right.X && left.Y == right.Y && left.Z == right.Z;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3 left, Vector3 right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3((float x, float y, float z) tuple) => new Vector3(tuple.x, tuple.y, tuple.z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator (float x, float y, float z)(Vector3 vector) => (vector.X, vector.Y, vector.Z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator System.Numerics.Vector3(Vector3 vector) => new System.Numerics.Vector3(vector.X, vector.Y, vector.Z);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(System.Numerics.Vector3 vector) => new Vector3(vector.X, vector.Y, vector.Z);
	}
}