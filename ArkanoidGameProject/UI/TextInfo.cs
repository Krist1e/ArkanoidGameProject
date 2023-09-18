using SFML.Graphics;

namespace ArkanoidGameProject.UI;

public struct TextInfo
{
    public string Content = string.Empty;
    public Font Font;
    public uint FontSize = 30;
    public Color Color = Color.Black;
    public Color BackgroundColor = Color.Transparent;

    public TextInfo(Font font)
    {
        Font = font;
    }
}