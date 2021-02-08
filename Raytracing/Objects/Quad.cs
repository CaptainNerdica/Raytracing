using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Objects
{
	public class Quad : SceneObject
	{
		private readonly Triangle _triangle1;
		private readonly Triangle _triangle2;
		public Material Material { get; set; }

		public Quad(Point vertex0, Point vertex1, Point vertex2, Point vertex3, Material material) : base((Vector3)(vertex0 + vertex1 + vertex2 + vertex3) / 4)
		{
			if (!Point.Coplanar(vertex0, vertex1, vertex2, vertex3))
				throw new ArgumentException("Points are not coplanar.");
			Material = material;
			_triangle1 = new Triangle(vertex0, vertex1, vertex2, material);
			_triangle2 = new Triangle(vertex0, vertex2, vertex3, material);
		}

		public override bool Intersects(Ray ray, float current, float max, out RayHit hit) => _triangle1.Intersects(ray, current, max, out hit) || _triangle2.Intersects(ray, current, max, out hit);
		public override Vector3 NormalAt(Point position) => _triangle1.NormalAt(position);
		public override Material MaterialAt(Point point) => Material;
	}
}
