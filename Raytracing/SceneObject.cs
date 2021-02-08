using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing
{
	public abstract class SceneObject : IIntersectable
	{
		public virtual Point Origin { get; set; }
		
		public SceneObject(Point origin)
		{
			Origin = origin;
		}

		public abstract bool Intersects(Ray ray, float current, float max, out RayHit hit);
		public abstract Vector3 NormalAt(Point point);
		public abstract Material MaterialAt(Point point);
	}
}
