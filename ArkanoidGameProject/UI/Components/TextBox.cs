using ArkanoidGameProject.UI.Interfaces;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArkanoidGameProject.UI.Components;

public class TextBox : TextLabel, IMouseListener, IKeyboardListener
{
    private static readonly List<TextBox> TextBoxes = new();
    
    private Color _cachedBackgroundColor;
    private bool _isFocused;
    private bool _isHovered;
    
    public TextBox()
    {
        TextBoxes.Add(this);
    }
    
    public event Action? Focused;
    public event Action? Unfocused;
    
    public event Action<IInputListener>? Enabled;
    public event Action<IInputListener>? Disabled;
    
    public bool IsFocused
    {
        get => _isFocused;
        set
        {
            if (_isFocused == value)
                return;
            
            _isFocused = value;
            if (_isFocused)
            {
                TextBoxes.ForEach(textBox => textBox.IsFocused = textBox == this);
                BackgroundColor = FocusColor;
                Focused?.Invoke();
            }
            else
            {
                BackgroundColor = _cachedBackgroundColor;
                Unfocused?.Invoke();
            }
        }
    }
    
    public Color HoverColor { get; set; }
    public Color FocusColor { get; set; }

    public override void Draw(RenderWindow window)
    {
        base.Draw(window);

        if (!IsFocused) return;
        var cursor = new RectangleShape(new Vector2f(1, Text.CharacterSize));
        cursor.FillColor = Text.FillColor;
        cursor.Position = new Vector2f(Text.Position.X + Text.GetLocalBounds().Width + 1, Text.Position.Y);
        window.Draw(cursor);
    }

    public void OnMouseEnter(MouseMoveEventArgs args)
    {
        if (IsFocused || _isHovered) return;
        
        _isHovered = true;
        _cachedBackgroundColor = BackgroundColor;
        BackgroundColor = HoverColor;
    }

    public void OnMouseLeave(MouseMoveEventArgs args)
    {
        if (IsFocused || !_isHovered) return;
        
        _isHovered = false;
        BackgroundColor = _cachedBackgroundColor;
    }

    public void OnMousePress(MouseButtonEventArgs args)
    {
        if (IsFocused) return;
        IsFocused = true;
        _isHovered = false;
    }

    public void OnKeyPressed(KeyEventArgs args)
    {
        if (!IsFocused) return;
        switch (args.Code)
        {
            case Keyboard.Key.Backspace:
            {
                if (Content.Length > 0)
                    Content = Content[..^1];
                break;
            }
            case Keyboard.Key.Space:
                Content += " ";
                break;
            case Keyboard.Key.Enter:
                IsFocused = false;
                break;
            default:
                Content += args.Code switch
                {
                    >= Keyboard.Key.A and <= Keyboard.Key.Z => args.Shift ? args.Code.ToString() : args.Code.ToString().ToLower(),
                    >= Keyboard.Key.Num0 and <= Keyboard.Key.Num9 => args.Code.ToString().Last(),
                    >= Keyboard.Key.Numpad0 and <= Keyboard.Key.Numpad9 => args.Code.ToString().Last(),
                    _ => ""
                };
                break;
        }
    }

    public void OnMouseRelease(MouseButtonEventArgs args) { }
    public void OnKeyReleased(KeyEventArgs args) { }
    
    public override void Destroy()
    {
        base.Destroy();
        TextBoxes.Remove(this);
    }

    protected override void OnEnabledChanged()
    {
        base.OnEnabledChanged();
        if (IsEnabled)
            Enabled?.Invoke(this);
        else
            Disabled?.Invoke(this);
    }
}