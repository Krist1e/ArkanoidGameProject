using ArkanoidGameProject.GameObjects;
using ArkanoidGameProject.UI.Interfaces;
using SFML.Graphics;
using SFML.Window;

namespace ArkanoidGameProject.UI.Components;

public class Button : DisplayObject, IMouseListener
{
    private readonly TextLabel _text = new();
    private Color _color;

    public TextInfo TextInfo
    {
        get => _text.TextInfo;
        set => _text.TextInfo = value;
    }

    public string Content 
    {
        get => _text.Content;
        set => _text.Content = value;
    }

    public Color TextColor
    {
        get => _text.Color;
        set => _text.Color = value;
    }

    public Font TextFont
    {
        get => _text.Font;
        set => _text.Font = value;
    }
    
    public uint FontSize
    {
        get => _text.FontSize;
        set => _text.FontSize = value;
    }

    public TextAlignment TextAlignment
    {
        get => _text.TextAlignment;
        set => _text.TextAlignment = value;
    }
    
    public int Padding
    {
        get => _text.Padding;
        set => _text.Padding = value;
    }

    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            _text.BackgroundColor = _color;
        }
    }
    public Color HoverColor { get; set; }
    
    public event EventHandler? Click;
    
    public event Action<IInputListener>? Enabled;
    public event Action<IInputListener>? Disabled;

    public override void Draw(RenderWindow window)
    {
        _text.Draw(window);
    }

    public virtual void OnMouseEnter(MouseMoveEventArgs mouseMoveEvent)
    {
        _text.BackgroundColor = HoverColor;
    }
    
    public virtual void OnMouseLeave(MouseMoveEventArgs mouseMoveEvent)
    {
        _text.BackgroundColor = _color;
    }

    public virtual void OnMousePress(MouseButtonEventArgs args)
    {
        if (args.Button == Mouse.Button.Left)
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
    public virtual void OnMouseRelease(MouseButtonEventArgs args) { }
    public override sealed void OnWallCollision(CollisionArgs args) { }
    public override sealed void OnCollision(DisplayObject obj, CollisionArgs args) { }
    public override sealed void Move(float deltaTime) { }

    public override void Destroy()
    {
        base.Destroy();
        _text.Destroy();
        Click = null;
    }

    protected override void OnPositionChanged()
    {
        base.OnPositionChanged();
        _text.Position = Position;
    }

    protected override void OnSizeChanged()
    {
        base.OnSizeChanged();
        _text.Size = Size;
    }

    protected override void OnEnabledChanged()
    {
        base.OnEnabledChanged();
        _text.IsEnabled = IsEnabled;
        if (IsEnabled)
            Enabled?.Invoke(this);
        else
            Disabled?.Invoke(this);
    }
}