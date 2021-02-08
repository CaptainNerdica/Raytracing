using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Objects
{
	public class Plane : SceneObject
	{
		private Vector3 _normal;
		public Material Material { get; set; }
		public Vector3 Normal { get => _normal; set => _normal = Vector3.Normalize(value); }
		public Plane(Point origin, Vector3 normal, Material material) : base(origin)
		{
			Normal = normal;
			Material = material;
		}

		public override bool Intersects(Ray ray, float currentDist, float max, out RayHit hit)
		{
			float d = Vector3.Dot(Origin - ray.Origin, Normal) / Vector3.Dot(ray.Direction, Normal);
			Point p = ray.PointAtDistance(d);
			if (d > 0 && d + currentDist < max)
				hit = new RayHit(p, Normal, d, MaterialAt(p));
			else
				hit = default;
			return d > 0 && d + currentDist < max;
		}
		public override Vector3 NormalAt(Point point) => Normal;

		public override Material MaterialAt(Point point) => Material;
	}

	public class TiledPlane : Plane
	{
		private Vector3 _basis;
		public Vector3 Basis { get => _basis; set => _basis = VectorExtensions.RejectUnit(value, Normal).Normalized(); }
		public Material OtherMaterial { get; set; }
		public float TileLength { get; set; }
		public TiledPlane(Point origin, Vector3 normal, Vector3 basis, Material material, Material otherMaterial, float tileLength) : base(origin, normal, material)
		{
			Basis = basis;
			Material = material;
			OtherMaterial = otherMaterial;
			TileLength = tileLength;
		}

		public override Material MaterialAt(Point point)
		{
			Vector3 vec = point - Origin;
			Vector3 planePos = VectorExtensions.RejectUnit(vec, Normal);
			float x = Vector3.Dot(Basis, VectorExtensions.ProjectUnit(planePos, Basis));
			float y = Vector3.Dot(Vector3.Cross(Basis, Normal), VectorExtensions.RejectUnit(planePos, Basis));
			int tx = (int)Math.Floor(x / TileLength);
			int ty = (int)Math.Floor(y / TileLength);
			if (((tx + ty) & 1) == 0)
				return Material;
			else
				return OtherMaterial;
		}
	}
}
