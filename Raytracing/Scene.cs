using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Raytracing
{
	public class Scene
	{
		public Camera Camera { get; }
		public ICollection<SceneObject> Objects { get; }
		public ICollection<Light> Lights { get; }
		public Color SkyBoxColorHigh { get; }
		public Color SkyBoxColorLow { get; }

		public Scene(Color skyboxHigh, Color skyboxLow, Camera camera) : this(skyboxHigh, skyboxLow, camera, new HashSet<SceneObject>(), new HashSet<Light>()) { }
		public Scene(Color skyboxHigh, Color skyboxLow, Camera camera, ICollection<SceneObject> objects, ICollection<Light> lights) : base()
		{
			Objects = objects;
			Lights = lights;
			Camera = camera;
			SkyBoxColorHigh = skyboxHigh;
			SkyBoxColorLow = skyboxLow;
		}

		public Color TraceRay(Ray ray, float currentLength, float maxDist, ref long traced, int depth = 0)
		{
			const float tolerance = 0.0001f;
			if (depth >= Camera.MaxReflections)
				return Color.Black;
			if (currentLength < maxDist)
			{
				++traced;
				SceneObject? hitObj = IntersectObject(ray, currentLength, maxDist, out RayHit hit);
				if (hitObj is not null)
				{
					Material material = hit.Material;
					Vector3 newNormal = (hit.Normal + VectorExtensions.RandomInUnit() * material.Diffuse / 4).Normalized();
					Vector3 reflectDir = Vector3.Reflect(ray.Direction, newNormal);
					Color reflectColor = default;
					int outSign = -Math.Sign(Vector3.Dot(hit.Normal, ray.Direction));
					if (material.Reflectivity > 0)
						reflectColor = TraceRay(new Ray(hit.Point + hit.Normal * tolerance * outSign, reflectDir), currentLength + hit.Length, maxDist, ref traced, depth + 1);
					Color light = Lighting(hit.Point + hit.Normal * tolerance * outSign, outSign * newNormal, -ray.Direction, material);
					//return light;
					//return Color.Lerp(material.Color, reflectColor, material.Reflectivity);
					return light * material.Color + reflectColor * material.Reflectivity;
				}
			}
			float t = 0.5f * (ray.Direction.Y + 1);
			return Color.Lerp(SkyBoxColorLow, SkyBoxColorHigh, t);
		}

		private SceneObject? IntersectObject(Ray ray, float currentDist, float maxDist, out RayHit hit)
		{
			hit = default;
			float minDist = float.MaxValue;
			SceneObject? hitObj = null;
			foreach (SceneObject obj in Objects)
			{
				if (obj.Intersects(ray, currentDist, maxDist, out RayHit rayHit) && rayHit.Length < minDist)
				{
					minDist = rayHit.Length;
					hitObj = obj;
					hit = rayHit;
				}
			}
			return hitObj;
		}

		private Color Lighting(Point point, Vector3 normal, Vector3 viewDir, Material material)
		{
			Color ambientColor = new Color(material.Ambient);
			Color lightColor = default;
			foreach (Light light in Lights)
			{
				Vector3 lightVec = light.DirectionAt(point);
				float distanceTo = light.DistanceAt(point);
				Ray shadowRay = new Ray(point, lightVec);
				SceneObject? obj = IntersectObject(shadowRay, 0, distanceTo, out _);
				if (obj == null)
				{
					float lambertian = Vector3.Dot(lightVec, normal);
					Vector3 h = (lightVec + viewDir).Normalized();
					float specular = MathF.Pow(Math.Max(Vector3.Dot(h, normal), 0), material.Shininess);
					Color c = light.Color * light.IntensityAt(point);
					lightColor += lambertian * c + specular * c;
				}
			}
			return ambientColor + lightColor;
		}
	}
}
