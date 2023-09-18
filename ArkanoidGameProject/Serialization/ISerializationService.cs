namespace ArkanoidGameProject.Serialization;

public interface ISerializationService
{
    public string FileExtension { get; }
    public string Serialize<T>(T obj);
    public T? Deserialize<T>(string data);
}