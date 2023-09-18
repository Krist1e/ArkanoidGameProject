using System.Text.Json;
using System.Text.Json.Serialization;
using SFML.Graphics;

namespace ArkanoidGameProject;

public class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
        
        var color = new Color();
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndObject:
                    return color;
                case JsonTokenType.PropertyName:
                {
                    var propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "R":
                            color.R = reader.GetByte();
                            break;
                        case "G":
                            color.G = reader.GetByte();
                            break;
                        case "B":
                            color.B = reader.GetByte();
                            break;
                        case "A":
                            color.A = reader.GetByte();
                            break;
                    }

                    break;
                }
            }
        }
        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("R", value.R);
        writer.WriteNumber("G", value.G);
        writer.WriteNumber("B", value.B);
        writer.WriteNumber("A", value.A);
        writer.WriteEndObject();
    }
}