using Raytracing.Import;
using Raytracing.Objects;
using StereoLithographyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Raytracing
{
	public static class SceneImport
	{
		public static ICollection<SceneObject> FromSTL(string filePath) => FromSTL(filePath, new Material(0.05f, 0, 0, 50, Color.Gray));
		public static ICollection<SceneObject> FromSTL(string filePath, Material material)
		{
			STLDocument stl = STLDocument.OpenRead(filePath);
			ICollection<SceneObject> objects = new HashSet<SceneObject>();
			foreach (Facet facet in stl.Facets)
			{
				Vector3 normal = (facet.Normal.X, facet.Normal.Z, facet.Normal.Y);
				Point vert0 = (facet.Vertex1.X, facet.Vertex1.Z, facet.Vertex1.Y);
				Point vert1 = (facet.Vertex2.X, facet.Vertex2.Z, facet.Vertex2.Y);
				Point vert2 = (facet.Vertex3.X, facet.Vertex3.Z, facet.Vertex3.Y);
				Triangle tri = new Triangle(normal, vert0, vert1, vert2, material);
				objects.Add(tri);
			}
			return objects;
		}
		public static ICollection<SceneObject> FromSTLRandomMaterial(string filePath)
		{
			STLDocument stl = STLDocument.OpenRead(filePath);
			ICollection<SceneObject> objects = new HashSet<SceneObject>();
			Random rand = new Random();
			foreach (Facet facet in stl.Facets)
			{
				Material mat = new Material(0.05f, (float)rand.NextDouble(), (float)rand.NextDouble(), rand.Next(0, 1000), new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));
				Vector3 normal = (facet.Normal.X, facet.Normal.Z, facet.Normal.Y);
				Point vert0 = (facet.Vertex1.X, facet.Vertex1.Z, facet.Vertex1.Y);
				Point vert1 = (facet.Vertex2.X, facet.Vertex2.Z, facet.Vertex2.Y);
				Point vert2 = (facet.Vertex3.X, facet.Vertex3.Z, facet.Vertex3.Y);
				Triangle tri = new Triangle(normal, vert0, vert1, vert2, mat);
				objects.Add(tri);
			}
			return objects;
		}

		public static Scene FromJsonFile(string path)
		{
			using Stream fileStream = File.OpenRead(path);
			using StreamReader sr = new StreamReader(fileStream);
			return FromJson(sr.ReadToEnd());
		}
		public static Scene FromJson(string json)
		{
			JsonSceneImport? import = JsonSerializer.Deserialize<JsonSceneImport>(json);
			if (import is null)
				throw new JsonException("Could not deserialize scene");
			ICollection<SceneObject> sceneObjects = new HashSet<SceneObject>();
			foreach (SceneObjectImport objImport in import.Objects)
			{
				Type? objType = Type.GetType(objImport.Type);
				if (objType is null)
					throw new Exception($"Could not find type {objImport.Type}");
				if (!objType.IsAssignableTo(typeof(SceneObject)))
					throw new Exception($"Type does not inherit from ${nameof(SceneObject)}.");
				if (JsonSerializer.Deserialize(objImport.Object.GetRawText(), objType) is not SceneObject obj)
					throw new Exception("Could not deserialize scene object.");
				sceneObjects.Add(obj);
			}
			ICollection<Light> lights = new HashSet<Light>();
			foreach (LightImport lightImport in import.Lights)
			{
				Type? lightType = Type.GetType(lightImport.Type);
				if (lightType is null)
					throw new Exception("Could not find type.");
				if (!lightType.IsAssignableTo(typeof(Light)))
					throw new Exception($"Type does not inherit from ${nameof(Light)}.");
				if (JsonSerializer.Deserialize(lightImport.Light.GetRawText(), lightType) is not Light light)
					throw new Exception("Could not deserialize light object.");
				lights.Add(light);
			}
			return new Scene(import.Scene.SkyBoxHighColor, import.Scene.SkyBoxLowColor, import.Camera, sceneObjects, lights);
		}
	}
}
