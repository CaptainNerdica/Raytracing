using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Raytracing.Import
{
	internal class DoubleConverter : JsonConverter<(float x, float y)>
	{
		public override (float x, float y) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartArray)
				throw new JsonException("Start of array not found");
			reader.Read();
			float x = reader.GetSingle();
			reader.Read();
			float y = reader.GetSingle();
			if (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				throw new JsonException("End of array not found");
			return (x, y);
		}
		public override void Write(Utf8JsonWriter writer, (float x, float y) value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(value.x);
			writer.WriteNumberValue(value.y);
			writer.WriteEndArray();
		}
	}

	internal class Vector2Converter : JsonConverter<Vector2>
	{
		public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartArray)
				throw new JsonException("Start of array not found");
			reader.Read();
			float x = reader.GetSingle();
			reader.Read();
			float y = reader.GetSingle();
			if (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				throw new JsonException("End of array not found");
			return new Vector2(x, y);
		}
		public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(value.X);
			writer.WriteNumberValue(value.Y);
			writer.WriteEndArray();
		}
	}
}
