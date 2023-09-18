using System.Text.Json;
using System.Text.Json.Serialization;
using ArkanoidGameProject.GameObjects;

namespace ArkanoidGameProject;

public class DisplayObjectJsonConverter : JsonConverter<DisplayObject>
{
    public override DisplayObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
        
        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();
        
        var propertyName = reader.GetString();
        reader.Read();
        if (propertyName != "Type")
            throw new JsonException();
        
        var typeName = reader.GetString();
        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException();
        
        propertyName = reader.GetString();
        reader.Read();
        if (propertyName != "Data")
            throw new JsonException();
        
        var type = Type.GetType($"ArkanoidGameProject.GameObjects.{typeName}");
        if (type == null)
            throw new JsonException();
        
        var obj = JsonSerializer.Deserialize(ref reader, type, options);
        if (obj == null)
            throw new JsonException();
        
        reader.Read();
        if (reader.TokenType != JsonTokenType.EndObject)
            throw new JsonException();
        
        return (DisplayObject) obj;
    }

    public override void Write(Utf8JsonWriter writer, DisplayObject value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        var type = value.GetType();
        writer.WriteString("Type", type.Name);
        writer.WritePropertyName("Data");
        JsonSerializer.Serialize(writer, value, type, options);
        writer.WriteEndObject();
    }
}