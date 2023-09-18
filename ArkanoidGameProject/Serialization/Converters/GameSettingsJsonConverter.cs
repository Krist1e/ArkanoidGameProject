using System.Text.Json;
using System.Text.Json.Serialization;
using SFML.System;

namespace ArkanoidGameProject.Serialization.Converters;

public class GameSettingsJsonConverter : JsonConverter<GameSettings>
{
    public override GameSettings? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var gameSettings = new GameSettings();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject) return gameSettings;

            if (reader.TokenType != JsonTokenType.PropertyName) continue;

            var propertyName = reader.GetString();
            reader.Read();
            switch (propertyName)
            {
                case nameof(GameSettings.PlayerName):
                    gameSettings.SetPlayerName(JsonSerializer.Deserialize<string>(ref reader, options)!);
                    break;
                case nameof(GameSettings.WindowSize):
                    gameSettings.SetWindowSize(JsonSerializer.Deserialize<Vector2f>(ref reader, options));
                    break;
                case nameof(GameSettings.Difficulty):
                    gameSettings.SetDifficulty(JsonSerializer.Deserialize<DifficultyLevel>(ref reader, options));
                    break;
            }
        }

        return gameSettings;
    }

    public override void Write(Utf8JsonWriter writer, GameSettings value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(nameof(GameSettings.PlayerName));
        JsonSerializer.Serialize(writer, value.PlayerName, options);
        writer.WritePropertyName(nameof(GameSettings.WindowSize));
        JsonSerializer.Serialize(writer, value.WindowSize, options);
        writer.WritePropertyName(nameof(GameSettings.Difficulty));
        JsonSerializer.Serialize(writer, value.Difficulty, options);
        writer.WriteEndObject();
    }
}