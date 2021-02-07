using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raytracing.Import
{
	internal class TripleConverter : JsonConverter<(float x, float y, float z)>
	{
		public override (float x, float y, float z) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartArray)
				throw new JsonException("Start of array not found");
			reader.Read();
			float x = reader.GetSingle();
			reader.Read();
			float y = reader.GetSingle();
			reader.Read();
			float z = reader.GetSingle();
			if (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				throw new JsonException("End of array not found");
			return (x, y, z);
		}
		public override void Write(Utf8JsonWriter writer, (float x, float y, float z) value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(value.x);
			writer.WriteNumberValue(value.y);
			writer.WriteNumberValue(value.z);
			writer.WriteEndArray();
		}
	}

	internal class ColorConverter : JsonConverter<Color>
	{
		public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartArray)
				throw new JsonException("Start of array not found");
			reader.Read();
			float r = reader.GetSingle();
			reader.Read();
			float g = reader.GetSingle();
			reader.Read();
			float b = reader.GetSingle();
			if (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				throw new JsonException("End of array not found");
			return new Color(r, g, b);
		}
		public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(value.R);
			writer.WriteNumberValue(value.G);
			writer.WriteNumberValue(value.B);
			writer.WriteEndArray();
		}
	}

	internal class Vector3Converter : JsonConverter<Vector3>
	{
		public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartArray)
				throw new JsonException("Start of array not found");
			reader.Read();
			float x = reader.GetSingle();
			reader.Read();
			float y = reader.GetSingle();
			reader.Read();
			float z = reader.GetSingle();
			if (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				throw new JsonException("End of array not found");
			return new Vector3(x, y, z);
		}
		public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(value.X);
			writer.WriteNumberValue(value.Y);
			writer.WriteNumberValue(value.Z);
			writer.WriteEndArray();
		}
	}

	internal class PointConverter : JsonConverter<Point>
	{
		public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartArray)
				throw new JsonException("Start of array not found");
			reader.Read();
			float x = reader.GetSingle();
			reader.Read();
			float y = reader.GetSingle();
			reader.Read();
			float z = reader.GetSingle();
			if (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				throw new JsonException("End of array not found");
			return new Point(x, y, z);
		}
		public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(value.X);
			writer.WriteNumberValue(value.Y);
			writer.WriteNumberValue(value.Z);
			writer.WriteEndArray();
		}
	}
}
