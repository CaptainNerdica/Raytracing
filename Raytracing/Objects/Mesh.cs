using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Objects
{
	public class Mesh : SceneObject, ICollection<SceneObject>
	{

		public ICollection<SceneObject> Objects { get; }
		public float Scale { get; set; }

		public int Count => Objects.Count;

		public bool IsReadOnly => Objects.IsReadOnly;

		public Mesh(Point origin) : this(origin, 1, Enumerable.Empty<SceneObject>()) { }
		public Mesh(Point origin, float scale) : this(origin, scale, Enumerable.Empty<SceneObject>()) { }
		public Mesh(Point origin, IEnumerable<SceneObject> objects) : this(origin, 1, objects) { }
		public Mesh(Point origin, float scale, IEnumerable<SceneObject> objects) : base(origin)
		{
			Scale = scale;
			Objects = new HashSet<SceneObject>(objects);
		}

		public void Add(SceneObject item) => Objects.Add(item);
		public void Clear() => Objects.Clear();
		public bool Contains(SceneObject item) => Objects.Contains(item);
		public void CopyTo(SceneObject[] array, int arrayIndex) => Objects.CopyTo(array, arrayIndex);
		public bool Remove(SceneObject item) => Objects.Remove(item);
		public IEnumerator<SceneObject> GetEnumerator() => Objects.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Objects.GetEnumerator();

		public override Material MaterialAt(Point point) => throw new NotImplementedException();
		public override Vector3 NormalAt(Point point) => throw new NotImplementedException();
		public override bool Intersects(Ray ray, float current, float max, out RayHit hit)
		{
			hit = default;
			bool hits = false;
			float minDist = float.PositiveInfinity;
			Ray newRay = new Ray((ray.Origin - Origin) / Scale, ray.Direction);
			foreach (SceneObject obj in Objects)
			{
				if (obj.Intersects(newRay, current, max, out RayHit outHit) && outHit.Length < minDist)
				{
					minDist = outHit.Length;
					hits = true;
					hit = outHit;
				}
			}
			if (hits)
				hit = new RayHit(Origin + hit.Point * Scale, -hit.Normal, hit.Length * Scale, hit.Material);
			return hits;
		}
	}
}
