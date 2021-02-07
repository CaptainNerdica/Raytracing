using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing.Lights
{
	public class DirectionalLight : Light
	{
		private Vector3 _direction;
		public Vector3 Direction { get => _direction; set => _direction = value.Normalized; }
		
		public DirectionalLight(Point center, Vector3 direction, float intensity, Color color) : base(center, intensity, color)
		{
			Direction = direction;
		}

		public override float IntensityAt(Point position) => Intensity;
		public override Vector3 DirectionAt(Point position) => -Direction;
		public override float DistanceAt(Point position) => (Center - position).Length;
	}
}
