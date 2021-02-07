using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raytracing
{
	public readonly struct Material
	{
		public float Ambient { get; }
		public float Diffuse { get; }
		public float Reflectivity { get; }
		public float Shininess { get; }
		public Color Color { get; }

		public static readonly Material Mirror = new Material(0.05f, 0, 1, 75, Color.White);
		public static readonly Material Black = new Material(0.05f, 0.1f, 0, 3, Color.Black);
		public static readonly Material White = new Material(0.05f, 0.1f, 0, 3, Color.White);

		[JsonConstructor]
		public Material(float ambient, float diffuse, float reflectivity, float shininess, Color color)
		{
			Ambient = Math.Clamp(ambient, 0, 1);
			Reflectivity = Math.Clamp(reflectivity, 0, 1);
			Diffuse = Math.Clamp(diffuse, 0, 1);
			Shininess = shininess;
			Color = color;
		}
	}
}
