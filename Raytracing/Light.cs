using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing
{
	public abstract class Light
	{
		public virtual Point Center { get; set; }
		public float Intensity { get; }
		public Color Color { get; }

		public Light(Point center, float intensity, Color color)
		{
			Center = center;
			Intensity = intensity;
			Color = color;
		}

		public abstract float IntensityAt(Point position);
		public abstract Vector3 DirectionAt(Point position);
		public abstract float DistanceAt(Point position);
	}
}
