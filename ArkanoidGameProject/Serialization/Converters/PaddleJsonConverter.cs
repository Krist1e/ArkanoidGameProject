using System.Text.Json;
using System.Text.Json.Serialization;
using ArkanoidGameProject.GameObjects;
using SFML.Graphics;
using SFML.System;
namespace ArkanoidGameProject.Serialization.Converters;

public class PaddleJsonConverter : JsonConverter<Paddle>
{
    public override Paddle? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
        
        var color = new Color();
        var speed = 0f;
        var position = new Vector2f();
        var size = new Vector2f();
        var isEnabled = false;
        var isVisible = false;
        var isStatic = false;
        var zIndex = 0;
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return new Paddle(position, size.X, size.Y, color, speed)
                {
                    IsEnabled = isEnabled,
                    IsVisible = isVisible,
                    IsStatic = isStatic,
                    ZIndex = zIndex
                };
            
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            
            var propertyName = reader.GetString();
            reader.Read();
            switch (propertyName)
            {
                case nameof(Paddle.Color):
                    color = JsonSerializer.Deserialize<Color>(ref reader, options);
                    break;
                case nameof(Paddle.Speed):
                    speed = JsonSerializer.Deserialize<float>(ref reader, options);
                    break;
                case nameof(Paddle.Position):
                    position = JsonSerializer.Deserialize<Vector2f>(ref reader, options);
                    break;
                case nameof(Paddle.Size):
                    size = JsonSerializer.Deserialize<Vector2f>(ref reader, options);
                    break;
                case nameof(Paddle.LeftTop):
                    JsonSerializer.Deserialize<Vector2f>(ref reader, options);
                    break;
                case nameof(Paddle.RightBottom):
                    JsonSerializer.Deserialize<Vector2f>(ref reader, options);
                    break;
                case nameof(Paddle.IsEnabled):
                    isEnabled = reader.GetBoolean();
                    break;
                case nameof(Paddle.IsVisible):
                    isVisible = reader.GetBoolean();
                    break;
                case nameof(Paddle.IsStatic):
                    isStatic = reader.GetBoolean();
                    break;
                case nameof(Paddle.ZIndex):
                    zIndex = reader.GetInt32();
                    break;
                default:
                    throw new JsonException();
            }
        }
        
        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Paddle value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(value.Color));
        JsonSerializer.Serialize(writer, value.Color, options);
        writer.WritePropertyName(nameof(value.Speed));
        JsonSerializer.Serialize(writer, value.Speed, options);
        writer.WritePropertyName(nameof(value.Position));
        JsonSerializer.Serialize(writer, value.Position, options);
        writer.WritePropertyName(nameof(value.Size));
        JsonSerializer.Serialize(writer, value.Size, options);
        writer.WritePropertyName(nameof(value.LeftTop));
        JsonSerializer.Serialize(writer, value.LeftTop, options);
        writer.WritePropertyName(nameof(value.RightBottom));
        JsonSerializer.Serialize(writer, value.RightBottom, options);
        writer.WriteBoolean(nameof(value.IsEnabled), value.IsEnabled);
        writer.WriteBoolean(nameof(value.IsVisible), value.IsVisible);
        writer.WriteBoolean(nameof(value.IsStatic), value.IsStatic);
        writer.WriteNumber(nameof(value.ZIndex), value.ZIndex);
        writer.WriteEndObject();
    }
}