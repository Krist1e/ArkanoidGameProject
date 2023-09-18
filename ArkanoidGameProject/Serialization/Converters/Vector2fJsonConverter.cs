using System.Text.Json;
using System.Text.Json.Serialization;
using SFML.System;

namespace ArkanoidGameProject;

public class Vector2fJsonConverter : JsonConverter<Vector2f>
{
    public override Vector2f Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var vector = new Vector2f();
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject:
                    return vector;
                case JsonTokenType.PropertyName:
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "X":
                            vector.X = reader.GetSingle();
                            break;
                        case "Y":
                            vector.Y = reader.GetSingle();
                            break;
                    }

                    break;
                }
            }
        }
        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Vector2f value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("X", value.X);
        writer.WriteNumber("Y", value.Y);
        writer.WriteEndObject();
    }
}