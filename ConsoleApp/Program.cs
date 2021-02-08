using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using Raytracing;
using Raytracing.Objects;
using Color = Raytracing.Color;

namespace RayTracing.ConsoleApp
{
	class Program
	{
		//{
		//	"Type": "Raytracing.Objects.TiledPlane",
		//	"Object": {
		//		"Origin": [ 0, 0, 0 ],
		//		"Basis": [ 1, 0, 0 ],
		//		"Normal": [ 0, 1, 0 ],
		//		"TileLength": 1,
		//		"Material": {
		//			"Color": [ 1, 1, 1 ],
		//			"Diffuse": 0.025,
		//			"Reflectivity": 0.2,
		//			"Shininess": 25,
		//			"Ambient": 0.05
		//		},
		//		"OtherMaterial": {
		//			"Color": [ 0, 0, 0 ],
		//			"Diffuse": 0.1,
		//			"Reflectivity": 0.05,
		//			"Shininess": 25,
		//			"Ambient": 0.05
		//		}
		//	}
		//}
		static void Main()
		{
			Scene scene = SceneImport.FromJsonFile("scene.json");
			float ambient = 0.05f;
			float diffuse = 0f;
			float reflectivity = 0.99f;
			float shininess = 500;
			Color matColor = new Color(0.0f);//(0, 0.025f, 0.0125f);
			Material mat = new Material(ambient, diffuse, reflectivity, shininess, matColor);
			float s = 6;
			Mesh mesh = new Mesh((-0.05f, -0.05f, -0.3f), 0.1f)
			{
				new Quad((0, 0, 0), (1, 0, 0), (1, 1, 0), (0, 1, 0), new Material(ambient, 0, 0, 25, Color.Red)),
				new Quad((0, 0, 1), (1, 0, 1), (1, 1, 1), (0, 1, 1), new Material(ambient, 0, 0, 25, -Color.Red)),
				new Quad((0, 0, 0), (0, 1, 0), (0, 1, 1), (0, 0, 1), new Material(ambient, 0, 0, 25, Color.Blue)),
				new Quad((1, 0, 0), (1, 1, 0), (1, 1, 1), (1, 0, 1), new Material(ambient, 0, 0, 25, -Color.Blue)),
				new Quad((0, 0, 0), (1, 0, 0), (1, 0, 1), (0, 0, 1), new Material(ambient, 0, 0, 25, Color.Green)),
				new Quad((0, 1, 0), (1, 1, 0), (1, 1, 1), (0, 1, 1), new Material(ambient, 0, 0, 25, -Color.Green)),
			};
			scene.Objects.Add(mesh);
			//scene.Objects.Add(new Cube((-s / 2, 0, -s / 2), s, mat));
			scene.Objects.Add(new Sphere((0, 0, 0), mat, s));
			//scene.Objects.Add(new Cube((-0.025f, -0.05f, -0.3f), 0.1f, new Material(ambient, 0, 0, 25, Color.Red)));
			//scene.Objects.Add(new Sphere((0, -0.1f, 0), new Material(ambient, 0, 0, 25, Color.Blue), 0.05f));
			using Image image = Engine.Render(scene, 1, 1, 1024, 1024);
			image.Save("out/scene.png");
		}
	}
}
