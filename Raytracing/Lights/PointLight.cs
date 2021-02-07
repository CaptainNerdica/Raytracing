using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Lights
{
	public class PointLight : Light
	{
		public float DropOffPower { get; }
		public PointLight(Point center, float intensity, float dropOffPower, Color color) : base(center, intensity, color)
		{
			DropOffPower = dropOffPower;
		}

		public override float IntensityAt(Point position) => Intensity / MathF.Pow((position - Center).Length, DropOffPower);
		public override Vector3 DirectionAt(Point position) => (Center - position).Normalized;
		public override float DistanceAt(Point position) => (Center - position).Length;
	}
}
