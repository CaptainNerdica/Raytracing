using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Objects
{
	public class Prism : SceneObject
	{
		private readonly Mesh _mesh;
		private float _scaleX;
		private float _scaleY;
		private float _scaleZ;
		private Material _material;

		public float ScaleX { get => _scaleX; set { _scaleX = value; RebuildMesh(_scaleX, _scaleY, _scaleZ, _material); } }
		public float ScaleY { get => _scaleY; set { _scaleY = value; RebuildMesh(_scaleX, _scaleY, _scaleZ, _material); } }
		public float ScaleZ { get => _scaleZ; set { _scaleZ = value; RebuildMesh(_scaleX, _scaleY, _scaleZ, _material); } }
		public (float x, float y, float z) Scale
		{
			get => (_scaleX, _scaleY, _scaleZ);
			set
			{
				_scaleX = value.x;
				_scaleY = value.y;
				_scaleZ = value.z;
				RebuildMesh(_scaleX, _scaleY, _scaleZ, _material);
			}
		}
		public Material Material { get => _material; set { _material = value; RebuildMesh(_scaleX, _scaleY, _scaleZ, Material); } }

		public Prism(Point origin, Material material) : this(origin, 1, 1, 1, material) { }
		public Prism(Point origin, float scaleX, float scaleY, float scaleZ, Material material) : base(origin)
		{
			_material = material;
			_scaleX = scaleX;
			_scaleY = scaleY;
			_scaleZ = scaleZ;
			_mesh = new Mesh(origin);
			RebuildMesh(scaleX, scaleY, scaleZ, material);
		}

		private void RebuildMesh(float x, float y, float z, Material material)
		{
			_mesh.Clear();
			_mesh.Add(new Quad((0, 0, 0), (x, 0, 0), (x, y, 0), (0, y, 0), material));
			_mesh.Add(new Quad((0, 0, z), (x, 0, z), (x, y, z), (0, y, z), material));
			_mesh.Add(new Quad((0, 0, 0), (x, 0, 0), (x, 0, z), (0, 0, z), material));
			_mesh.Add(new Quad((0, y, 0), (x, y, 0), (x, y, z), (0, y, z), material));
			_mesh.Add(new Quad((0, 0, 0), (0, y, 0), (0, y, z), (0, 0, z), material));
			_mesh.Add(new Quad((x, 0, 0), (x, y, 0), (x, y, z), (x, 0, z), material));
		}

		public override bool Intersects(Ray ray, float current, float max, out RayHit hit) => _mesh.Intersects(ray, current, max, out hit);
		public override Vector3 NormalAt(Point point) => _mesh.NormalAt(point);
		public override Material MaterialAt(Point point) => _mesh.MaterialAt(point);
	}
}
