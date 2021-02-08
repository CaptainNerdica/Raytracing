using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Objects
{
	public class Sphere : SceneObject
	{
		public virtual Material Material { get; set; }
		public virtual float Radius { get; set; }
		public Sphere(Point origin, Material material, float radius) : base(origin)
		{
			Radius = radius;
			Material = material;
		}

		public override bool Intersects(Ray ray, float current, float max, out RayHit rayHit)
		{
			Vector3 dir = ray.Origin - Origin;
			float b = Vector3.Dot(dir, ray.Direction);
			float c = dir.LengthSquared() - Radius * Radius;
			float discriminant = b * b - c;
			rayHit = default;
			if (discriminant < 0)
				return false;
			float sqrtDiscrim = MathF.Sqrt(discriminant);
			float root = (-b - sqrtDiscrim);
			if (root < 0 || max < root)
			{
				root = -b + sqrtDiscrim;
				if (root < 0 || max < root)
					return false;
			}
			Point point = ray.PointAtDistance(root);
			rayHit = new RayHit(point, NormalAt(point), root, Material);
			return true;
		}
		public override Vector3 NormalAt(Point position) => (position - Origin).Normalized();
		public override Material MaterialAt(Point position) => Material;
	}
}
