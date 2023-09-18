using System.Text.Json;
using ArkanoidGameProject.Serialization.Converters;

namespace ArkanoidGameProject.Serialization;

public class JsonSerialization : ISerializationService
{
    private readonly JsonSerializerOptions _options = new()
    {
        Converters =
        {
            new Vector2fJsonConverter(),
            new ColorJsonConverter(),
            new DisplayObjectJsonConverter(),
            new PaddleJsonConverter(),
            new BonusJsonConverter(),
            new GameSettingsJsonConverter()
        },
        WriteIndented = true
    };
    
    public string FileExtension => "json";
    public string Serialize<T>(T obj) => JsonSerializer.Serialize(obj, _options);

    public T? Deserialize<T>(string data) => JsonSerializer.Deserialize<T>(data, _options);
}