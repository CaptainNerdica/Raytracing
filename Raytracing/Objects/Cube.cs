using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Objects
{
	public class Cube : SceneObject
	{
		private readonly Prism _prism;
		private float _scale;

		public float Scale
		{
			get => _scale;
			set { _scale = value; _prism.Scale = (value, value, value); }
		}
		public Material Material { get => _prism.Material; set => _prism.Material = value; }

		public Cube(Point origin, Material material) : this(origin, 1, material) { }
		public Cube(Point origin, float scale, Material material) : base(origin)
		{
			_scale = scale;
			_prism = new Prism(origin, scale, scale, scale, material);
		}

		public override bool Intersects(Ray ray, float current, float max, out RayHit hit) => _prism.Intersects(ray, current, max, out hit);
		public override Material MaterialAt(Point point) => _prism.MaterialAt(point);
		public override Vector3 NormalAt(Point point) => _prism.NormalAt(point);
	}
}
