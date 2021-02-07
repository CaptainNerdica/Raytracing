using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raytracing.Import
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	internal class JsonSceneImport
	{
		public SceneImport Scene { get; set; }
		public Camera Camera { get; set; }
		public ICollection<SceneObjectImport> Objects { get; set; }
		public ICollection<LightImport> Lights { get; set; }
	}
	internal class SceneImport
	{
		public Color SkyBoxHighColor { get; set; }
		public Color SkyBoxLowColor { get; set; }
	}
	internal class SceneObjectImport
	{
		public string Type { get; set; }
		public JsonElement Object { get; set; }
	}
	internal class LightImport
	{
		public string Type { get; set; }
		public JsonElement Light { get; set; }
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
