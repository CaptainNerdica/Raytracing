using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Raytracing;
using Raytracing.Objects;
using Color = Raytracing.Color;

namespace RayTracing.ConsoleApp
{
	class Program
	{
		static void Main()
		{
			Scene scene = SceneImport.FromJsonFile("scene.json");
			float ambient = 0.05f;
			float diffuse = 0f;
			float reflectivity = 1f;
			float shininess = 25;
			Material mat = new Material(ambient, diffuse, reflectivity, shininess, Color.White);
			float s = 3;
			Mesh mesh = new Mesh((-s / 2, 0, -s / 2), s)
			{
				new Quad((0, 0, 0), (1, 0, 0), (1, 1, 0), (0, 1, 0), mat),
				new Quad((0, 0, 1), (1, 0, 1), (1, 1, 1), (0, 1, 1), mat),
				new Quad((0, 0, 0), (0, 1, 0), (0, 1, 1), (0, 0, 1), mat),
				new Quad((1, 0, 0), (1, 1, 0), (1, 1, 1), (1, 0, 1), mat),
			};
			//scene.Objects.Add(mesh);
			scene.Objects.Add(new Cube((2, 1, 3), 3, new Material(0.05f, 0, 1, 500, Color.Black)));
			scene.Objects.Add(new Sphere((0, 1, 2), new Material(ambient, 0.1f, 0.05f, 25, Color.Red), 1));
			using Image image = Engine.Render(scene, 3, 1, 1024, 1024);
			image.Save("out/scene.png");
		}
	}
}
