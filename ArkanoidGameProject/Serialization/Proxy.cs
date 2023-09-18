namespace ArkanoidGameProject.Serialization;

public class Proxy
{
    private readonly ISerializationService _serializationService;
    public Proxy(ISerializationService serializationService)
    {
        _serializationService = serializationService;
    }

    public string Path { get; set; } = "save";

    public void Save(Game.GameData gameData)
    {
        var data = _serializationService.Serialize(gameData);
        File.WriteAllText(Path + "." + _serializationService.FileExtension, data);
    }
    
    public Game.GameData? Load()
    {
        var data = File.ReadAllText(Path + "." + _serializationService.FileExtension);
        return _serializationService.Deserialize<Game.GameData>(data);
    }
}