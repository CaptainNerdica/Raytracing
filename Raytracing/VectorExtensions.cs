using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing
{
	internal static class VectorExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Normalized(this Vector3 vector) => vector / vector.Length();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Project(Vector3 vector1, Vector3 vector2) => vector2 * (Vector3.Dot(vector1, vector2) / vector2.Length());
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ProjectUnit(Vector3 vector, Vector3 unit) => unit * Vector3.Dot(vector, unit);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 Reject(Vector3 vector1, Vector3 vector2) => vector1 - vector2 * (Vector3.Dot(vector1, vector2) / vector2.Length());
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 RejectUnit(Vector3 vector, Vector3 unit) => vector - unit * Vector3.Dot(vector, unit);

		public static Vector3 RandomInUnit()
		{
			while (true)
			{
				Vector3 v = new Vector3(2 * (float)ThreadSafeRandom.NextDouble() - 0.5f, 2 * (float)ThreadSafeRandom.NextDouble() - 0.5f, 2 * (float)ThreadSafeRandom.NextDouble() - 0.5f);
				if (v.LengthSquared() <= 1)
					return v;
			}
		}
	}
}
