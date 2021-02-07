using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing
{
	public interface IIntersectable
	{
		bool Intersects(Ray ray, float current, float max, out RayHit hit);
		Vector3 NormalAt(Point point);
		Material MaterialAt(Point point);
	}
}
