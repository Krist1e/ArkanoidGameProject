using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using ArkanoidGameProject.Bonuses;
using ArkanoidGameProject.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.Serialization;

public class TxtSerialization : ISerializationService
{
    private readonly Dictionary<Type, TxtConverter> _converters = new();
    
    public TxtSerialization()
    {
        _converters.Add(typeof(Vector2f), new Vector2FTxtConverter(this));
        _converters.Add(typeof(Color), new ColorTxtConverter(this));
        _converters.Add(typeof(bool), new BooleanTxtConverter(this));
        _converters.Add(typeof(float), new SingleTxtConverter(this));
        _converters.Add(typeof(int), new Int32TxtConverter(this));
        _converters.Add(typeof(string), new StringTxtConverter(this));
        _converters.Add(typeof(TimeSpan), new TimeSpanTxtConverter(this));
        _converters.Add(typeof(IEnumerable<>), new EnumerableTxtConverter(this));
        _converters.Add(typeof(DisplayObject), new DisplayObjectTxtConverter(this));
        _converters.Add(typeof(Bonus), new BonusTxtConverter(this));
        _converters.Add(typeof(Paddle), new PaddleTxtConverter(this));
        _converters.Add(typeof(GameSettings), new GameSettingsTxtConverter(this));
        _converters.Add(typeof(Game.GameData), new GameDataTxtConverter(this));
    }
    
    public string FileExtension => "txt";

    public string Serialize<T>(T obj)
    {
        return Serialize(obj, typeof(T));
    }

    public T? Deserialize<T>(string data)
    {
        return (T?)Deserialize(data, typeof(T));
    }

    private string Serialize(object obj, Type type)
    {
        if (type.IsGenericType)
            type = type.GetGenericTypeDefinition();
        if (_converters.TryGetValue(type, out var converter))
        {
            return converter.Serialize(obj);
        }

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var builder = new StringBuilder();
        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            builder.AppendLine($"{property.Name}={Serialize(value, property.PropertyType)}");
        }

        return builder.ToString();
    }
    
    private object Deserialize(string data, Type type)
    {
        if (type.IsGenericType)
            type = type.GetGenericTypeDefinition();
        if (_converters.TryGetValue(type, out var converter))
        {
            return converter.Deserialize(data);
        }
        
        var obj = Activator.CreateInstance(type);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        var lines = data.Split(Environment.NewLine);
        for (var index = 0; index < lines.Length; index++)
        {
            var line = lines[index];
            var parts = line.Split('=');
            var propertyName = parts[0];
            var value = line.Contains('=') && !line.EndsWith('=') ? parts[1] : GetMultilineValue(lines, ref index);
            var property = properties.FirstOrDefault(p => p.Name == propertyName);
            if (property is not null)
            {
                property.SetValue(obj, Deserialize(value, property.PropertyType));
            }
            else
            {
                var field = fields.FirstOrDefault(f => f.Name == propertyName);
                if (field is null) continue;
                field.SetValue(obj, Deserialize(value, field.FieldType));
            }
        }

        return obj;
    }
    
    private string GetMultilineValue(IReadOnlyList<string> lines, ref int index)
    {
        var builder = new StringBuilder();
        index++;
        while (index < lines.Count)
        {
            var line = lines[index];
            if (!line.StartsWith('\t') && !string.IsNullOrWhiteSpace(line))
            {
                index--;
                break;
            }

            builder.AppendLine(line);
            index++;
        }
        
        return builder.ToString();
    }
    
    private abstract class TxtConverter
    {
        protected readonly TxtSerialization Serialization;
        
        protected TxtConverter(TxtSerialization serialization)
        {
            Serialization = serialization;
        }

        public abstract string Serialize(object obj);
        public abstract object Deserialize(string data);
    }
    
    
    private class Vector2FTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var vector = (Vector2f) obj;
            return $"{vector.X} {vector.Y}";
        }

        public override object Deserialize(string data)
        {
            var values = data.Split(' ');
            return new Vector2f(float.Parse(values[0]), float.Parse(values[1]));
        }

        public Vector2FTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }
    
    private class ColorTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var color = (Color) obj;
            return $"{color.R} {color.G} {color.B} {color.A}";
        }

        public override object Deserialize(string data)
        {
            var values = data.Split(' ');
            return new Color(byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]), byte.Parse(values[3]));
        }

        public ColorTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }

    private class BooleanTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var boolean = (bool) obj;
            return boolean.ToString();
        }

        public override object Deserialize(string data)
        {
            return bool.Parse(data);
        }

        public BooleanTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }

    private class Int32TxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var integer = (int) obj;
            return integer.ToString();
        }

        public override object Deserialize(string data)
        {
            return int.Parse(data);
        }

        public Int32TxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }

    private class SingleTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var single = (float) obj;
            return single.ToString(CultureInfo.InvariantCulture);
        }

        public override object Deserialize(string data)
        {
            return float.Parse(data);
        }

        public SingleTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }

    private class StringTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var str = (string) obj;
            return str;
        }

        public override object Deserialize(string data)
        {
            return data;
        }

        public StringTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }

    private class TimeSpanTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var timeSpan = (TimeSpan) obj;
            return timeSpan.ToString();
        }

        public override object Deserialize(string data)
        {
            return TimeSpan.Parse(data);
        }

        public TimeSpanTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }

    private class EnumerableTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var enumerable = (IEnumerable) obj;
            var builder = new StringBuilder();
            foreach (var item in enumerable)
            {
                builder.Append("\r\n\t" + Serialization.Serialize(item, item.GetType().BaseType).Replace(Environment.NewLine, Environment.NewLine + "\t"));
            }

            return builder.ToString();
        }

        public override object Deserialize(string data)
        {
            var lines = data.Split(Environment.NewLine);
            var type = Type.GetType(lines[0].Split('=')[1]).BaseType;
            var listType = typeof(List<>).MakeGenericType(type);
            var list = (IList)Activator.CreateInstance(listType);
            var builder = new StringBuilder();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    builder.AppendLine(line.Trim());
                }
                else
                {
                    if (builder.Length <= 0) continue;
                    list.Add(Serialization.Deserialize(builder.ToString(), type));
                    builder.Clear();
                }
            }

            return list;
        }

        public EnumerableTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }

    private class DisplayObjectTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var displayObject = (DisplayObject) obj;
            var type = obj.GetType();
            var builder = new StringBuilder();
            builder.AppendLine("Type=" + type.FullName);
            builder.AppendLine(Serialization.Serialize(displayObject, type));
            return builder.ToString();
        }

        public override object Deserialize(string data)
        {
            var lines = data.Split(Environment.NewLine);
            var type = Type.GetType(lines[0].Split('=')[1]);
            var builder = new StringBuilder();
            for (var i = 1; i < lines.Length; i++)
            {
                builder.AppendLine(lines[i].Trim());
            }
            return Serialization.Deserialize(builder.ToString(), type);
        }

        public DisplayObjectTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }
    
    private class BonusTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var bonus = (Bonus) obj;
            var type = obj.GetType();
            var builder = new StringBuilder();
            builder.AppendLine("Type=" + type.FullName);
            builder.AppendLine(Serialization.Serialize(bonus, type));
            return builder.ToString();
        }
        
        public override object Deserialize(string data)
        {
            var lines = data.Split(Environment.NewLine);
            var type = Type.GetType(lines[0].Split('=')[1]);
            var builder = new StringBuilder();
            for (var i = 1; i < lines.Length; i++)
            {
                builder.AppendLine(lines[i].Trim());
            }
            return Serialization.Deserialize(builder.ToString(), type);
        }
        
        public BonusTxtConverter(TxtSerialization serialization) : base(serialization)
        {}
    }

    private class PaddleTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var paddle = (Paddle) obj;
            var builder = new StringBuilder();
            builder.AppendLine(nameof(paddle.Color) + "=" + Serialization.Serialize(paddle.Color, typeof(Color)));
            builder.AppendLine(nameof(paddle.Speed) + "=" + Serialization.Serialize(paddle.Speed, typeof(float)));
            builder.AppendLine(nameof(paddle.Position) + "=" + Serialization.Serialize(paddle.Position, typeof(Vector2f)));
            builder.AppendLine(nameof(paddle.Size) + "=" + Serialization.Serialize(paddle.Size, typeof(Vector2f)));
            builder.AppendLine(nameof(paddle.LeftTop) + "=" + Serialization.Serialize(paddle.LeftTop, typeof(Vector2f)));
            builder.AppendLine(nameof(paddle.RightBottom) + "=" + Serialization.Serialize(paddle.RightBottom, typeof(Vector2f)));
            builder.AppendLine(nameof(paddle.IsEnabled) + "=" + Serialization.Serialize(paddle.IsEnabled, typeof(bool)));
            builder.AppendLine(nameof(paddle.IsVisible) + "=" + Serialization.Serialize(paddle.IsVisible, typeof(bool)));
            builder.AppendLine(nameof(paddle.IsStatic) + "=" + Serialization.Serialize(paddle.IsStatic, typeof(bool)));
            builder.AppendLine(nameof(paddle.ZIndex) + "=" + Serialization.Serialize(paddle.ZIndex, typeof(int)));
            return builder.ToString();
        }

        public override object Deserialize(string data)
        {
            var paddle = new Paddle();
            var lines = data.Split(Environment.NewLine);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split('=');
                var propertyName = parts[0];
                var value = parts[1];
                switch (propertyName)
                {
                    case nameof(paddle.Color):
                        paddle.Color = (Color) Serialization.Deserialize(value, typeof(Color));
                        break;
                    case nameof(paddle.Speed):
                        paddle.Speed = (float) Serialization.Deserialize(value, typeof(float));
                        break;
                    case nameof(paddle.Position):
                        paddle.Position = (Vector2f) Serialization.Deserialize(value, typeof(Vector2f));
                        break;
                    case nameof(paddle.Size):
                        paddle.Size = (Vector2f) Serialization.Deserialize(value, typeof(Vector2f));
                        break;
                    case nameof(paddle.LeftTop):
                        paddle.LeftTop = (Vector2f) Serialization.Deserialize(value, typeof(Vector2f));
                        break;
                    case nameof(paddle.RightBottom):
                        paddle.RightBottom = (Vector2f) Serialization.Deserialize(value, typeof(Vector2f));
                        break;
                    case nameof(paddle.IsEnabled):
                        paddle.IsEnabled = (bool) Serialization.Deserialize(value, typeof(bool));
                        break;
                    case nameof(paddle.IsVisible):
                        paddle.IsVisible = (bool) Serialization.Deserialize(value, typeof(bool));
                        break;
                    case nameof(paddle.IsStatic):
                        paddle.IsStatic = (bool) Serialization.Deserialize(value, typeof(bool));
                        break;
                    case nameof(paddle.ZIndex):
                        paddle.ZIndex = (int) Serialization.Deserialize(value, typeof(int));
                        break;
                }
            }

            return paddle;
        }

        public PaddleTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }

    private class GameSettingsTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var gameSettings = (GameSettings) obj;
            var builder = new StringBuilder();
            builder.AppendLine(nameof(gameSettings.PlayerName) + "=" + Serialization.Serialize(gameSettings.PlayerName, typeof(string)));
            builder.AppendLine(nameof(gameSettings.WindowSize) + "=" + Serialization.Serialize(gameSettings.WindowSize, typeof(Vector2f)));
            builder.AppendLine(nameof(gameSettings.Difficulty) + "=" + gameSettings.Difficulty);
            return builder.ToString();
        }

        public override object Deserialize(string data)
        {
            var lines = data.Split(Environment.NewLine);
            var gameSettings = new GameSettings();
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var keyValuePair = line.Split('=');
                var key = keyValuePair[0];
                var value = keyValuePair[1];
                switch (key)
                {
                    case nameof(gameSettings.PlayerName):
                        gameSettings.SetPlayerName((string) Serialization.Deserialize(value, typeof(string)));
                        break;
                    case nameof(gameSettings.WindowSize):
                        gameSettings.SetWindowSize((Vector2f) Serialization.Deserialize(value, typeof(Vector2f)));
                        break;
                    case nameof(gameSettings.Difficulty):
                        var difficultyLevel = Enum.TryParse<DifficultyLevel>(value, out var result) ? result : DifficultyLevel.Easy;
                        gameSettings.SetDifficulty(difficultyLevel);
                        break;
                }
            }

            return gameSettings;
        }

        public GameSettingsTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }

    private class GameDataTxtConverter : TxtConverter
    {
        public override string Serialize(object obj)
        {
            var gameData = (Game.GameData) obj;
            var builder = new StringBuilder();
            builder.AppendLine(nameof(gameData.GameField) + "=\r\n\t" + Serialization.Serialize(gameData.GameField, typeof(GameField)).Replace(Environment.NewLine, Environment.NewLine + "\t"));
            builder.AppendLine(nameof(gameData.GameStats) + "=\r\n\t" + Serialization.Serialize(gameData.GameStats, typeof(GameStats)).Replace(Environment.NewLine, Environment.NewLine + "\t"));
            builder.AppendLine(nameof(gameData.GameSettings) + "=\r\n\t" + Serialization.Serialize(gameData.GameSettings, typeof(GameSettings)).Replace(Environment.NewLine, Environment.NewLine + "\t"));
            builder.AppendLine(nameof(gameData.Bonuses) + "=" + Serialization.Serialize(gameData.Bonuses, typeof(IEnumerable<Bonus>)));
            return builder.ToString();
        }

        public override object Deserialize(string data)
        {
            var gameData = new Game.GameData();
            var lines = data.Split(Environment.NewLine);
            
            return gameData;
        }

        public GameDataTxtConverter(TxtSerialization serialization) : base(serialization)
        {
        }
    }
}