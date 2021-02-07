using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Objects
{
	public class Triangle : SceneObject
	{
		public Vector3 Normal { get; set; }
		public Point Vertex0 { get; set; }
		public Point Vertex1 { get; set; }
		public Point Vertex2 { get; set; }
		public Material Material { get; set; }

		public Triangle(Vector3 normal, Point vert0, Point vert1, Point vert2, Material material) : base((Vector3)(vert0 + vert1 + vert2) / 3)
		{
			Normal = normal;
			Vertex0 = vert0;
			Vertex1 = vert1;
			Vertex2 = vert2;
			Material = material;
		}

		public Triangle(Point vert0, Point vert1, Point vert2, Material material) : this(Vector3.Cross(vert1 - vert0, vert2 - vert0).Normalized, vert0, vert1, vert2, material)
		{ }

		public override bool Intersects(Ray ray, float current, float max, out RayHit hit)
		{
			const float epsilon = 0.0000001f;
			hit = default;
			Vector3 edge1, edge2, h, s, q;
			float a, f, u, v;
			edge1 = Vertex1 - Vertex0;
			edge2 = Vertex2 - Vertex0;
			h = Vector3.Cross(ray.Direction, edge2);
			a = Vector3.Dot(edge1, h);
			if (a > -epsilon && a < epsilon)
				return false;
			f = 1.0f / a;
			s = ray.Origin - Vertex0;
			u = f * Vector3.Dot(s, h);
			if (u < 0 || u > 1)
				return false;
			q = Vector3.Cross(s, edge1);
			v = f * Vector3.Dot(ray.Direction, q);
			if (v < 0.0 || u + v > 1)
				return false;
			float t = f * Vector3.Dot(edge2, q);
			if (t > epsilon)
			{
				Point p = ray.PointAtDistance(t);
				hit = new RayHit(p, NormalAt(p), t, MaterialAt(p));
				return true;
			}
			return false;

		}
		public override Vector3 NormalAt(Point position) => Normal;
		public override Material MaterialAt(Point position) => Material;
	}
}
