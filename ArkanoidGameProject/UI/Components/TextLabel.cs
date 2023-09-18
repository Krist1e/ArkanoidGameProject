using ArkanoidGameProject.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.UI.Components;

public enum TextAlignment
{
    Left,
    Center,
    Right
}

public class TextLabel : DisplayObject
{
    private TextInfo _textInfo;
    protected readonly Text Text = new();
    protected readonly RectangleShape Background = new();

    private TextAlignment _textAlignment = TextAlignment.Left;
    private int _padding = 5;

    public TextInfo TextInfo
    {
        get => _textInfo;
        set
        {
            _textInfo = value;
            Text.DisplayedString = value.Content;
            Text.Font = value.Font;
            Text.CharacterSize = value.FontSize;
            Text.FillColor = value.Color;
            Background.FillColor = value.BackgroundColor;
            OnTextChange();
        }
    }
    
    public string Content 
    {
        get => _textInfo.Content;
        set
        {
            _textInfo.Content = value;
            Text.DisplayedString = value;
            OnTextChange();
        }
    }
    
    public Font Font
    {
        get => _textInfo.Font;
        set
        {
            _textInfo.Font = value;
            Text.Font = value;
            OnTextChange();
        }
    }
    
    public uint FontSize
    {
        get => _textInfo.FontSize;
        set
        {
            _textInfo.FontSize = value;
            Text.CharacterSize = value;
            OnTextChange();
        }
    }
    
    public Color Color
    {
        get => _textInfo.Color;
        set
        {
            _textInfo.Color = value;
            Text.FillColor = value;
        }
    }
    
    public Color BackgroundColor
    {
        get => _textInfo.BackgroundColor;
        set
        {
            _textInfo.BackgroundColor = value;
            Background.FillColor = value;
        }
    }

    public TextAlignment TextAlignment { get => _textAlignment; set { _textAlignment = value; OnTextChange(); } }
    public int Padding { get => _padding; set { _padding = value; OnTextChange(); } }
    
    private FloatRect TextSize => Text.GetLocalBounds();
    private Vector2f DrawPosition => TextAlignment switch
    {
        TextAlignment.Left => LeftTop + new Vector2f(Padding, Padding),
        TextAlignment.Center => LeftTop + new Vector2f((Size.X - TextSize.Width) / 2, Padding),
        TextAlignment.Right => LeftTop + new Vector2f(Size.X - TextSize.Width - Padding, Padding),
        _ => throw new ArgumentOutOfRangeException()
    }; 

    public override void Draw(RenderWindow window)
    {
        window.Draw(Background);
        window.Draw(Text);
    }
    
    public sealed override void OnWallCollision(CollisionArgs args) { }
    public sealed override void OnCollision(DisplayObject obj, CollisionArgs args) { }
    public sealed override void Move(float deltaTime) { }

    protected override void OnPositionChanged()
    {
        base.OnPositionChanged();
        Background.Position = LeftTop;
        Text.Position = DrawPosition;
    }
    
    protected override void OnSizeChanged()
    {
        base.OnSizeChanged();
        Background.Position = LeftTop;
        Background.Size = Size;
        Text.Position = DrawPosition;
    }

    private void OnTextChange()
    {
        Text.Position = DrawPosition;
    }
}