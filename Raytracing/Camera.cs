using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Raytracing
{
	public class Camera
	{
		public Point Position { get; set; } = Point.Zero;
		public Vector2 Rotation { get; set; } = Vector2.Zero;
		public float NearClipping { get; set; } = 0.05f;
		public float MaxDistance { get; set; } = 100;
		public float PlaneWidth { get; set; } = 0.1f;
		public int MaxReflections { get; set; } = 25;
		public float FieldOfView => 2 * MathF.Atan(PlaneWidth / 2 / NearClipping) * MathExt.RadiansToDegress;

		public Camera() { }

		public Camera(Point position, float nearClipping, float maxDistance, int maxReflections, Vector2 rotation)
		{
			Position = position;
			NearClipping = nearClipping;
			MaxDistance = maxDistance;
			MaxReflections = maxReflections;
			Rotation = rotation;
		}
		public Ray GetRay(int xPos, int yPos, int width, int height) => GetRay((float)xPos, yPos, width, height);

		public Ray GetRay(float xPos, float yPos, int width, int height)
		{
			float u = xPos / width - 0.5f;
			float v = (height / 2 - yPos) / width;
			float x = u * PlaneWidth;
			float y = v * PlaneWidth;
			float z = NearClipping;
			float ryr = Rotation.Y * MathExt.DegreesToRadians;
			float rxr = Rotation.X * MathExt.DegreesToRadians;
			float sx = MathF.Sin(ryr);
			float sy = MathF.Sin(-rxr);
			float cx = MathF.Cos(ryr);
			float cy = MathF.Cos(rxr);
			float x1 = cy * x + sy * (cx * z - sx * y);
			float y1 = cx * y + sx * z;
			float z1 = cy * (cx * z - sx * y) - sy * x;
			return new Ray(Position, new Vector3(x1, y1, z1));
		}
	}
}