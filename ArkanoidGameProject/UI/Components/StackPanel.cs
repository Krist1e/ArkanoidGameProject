using ArkanoidGameProject.GameObjects;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.UI.Components;


public enum Orientation
{
    Horizontal,
    Vertical
}

public class StackPanel : DisplayObject
{
    private readonly List<DisplayObject> _children = new();
    private readonly RectangleShape _background = new();
    
    private Orientation _orientation = Orientation.Horizontal;

    private int _spacing = 5;
    private int _padding = 5;
    
    public IEnumerable<DisplayObject> Children => _children;

    public Color BackgroundColor
    {
        get => _background.FillColor;
        set => _background.FillColor = value;
    }

    public int Padding
    {
        get => _padding;
        set
        {
            _padding = value;
            OnSizeChanged();
            PropertyChanged();
        }
    }

    public int Spacing
    {
        get => _spacing;
        set
        {
            _spacing = value;
            PropertyChanged();
        }
    }

    public Orientation Orientation
    {
        get => _orientation;
        set
        {
            _orientation = value;
            PropertyChanged();
        }
    }

    public void AddChild(DisplayObject child)
    {
        _children.Add(child);
        ModifyChild(child);
    }
    
    public void RemoveChild(DisplayObject child)
    {
        _children.Remove(child);
        PropertyChanged();
    }
    
    public void Clear()
    {
        _children.Clear();
    }

    public override void Draw(RenderWindow window)
    {
        window.Draw(_background);
        foreach (var child in _children) child.Draw(window);
    }

    public sealed override void OnWallCollision(CollisionArgs args) { }
    public sealed override void OnCollision(DisplayObject obj, CollisionArgs args) { }
    public sealed override void Move(float deltaTime) { }
    
    private void ModifyChild(DisplayObject child)
    {
        switch (Orientation)
        {
            case Orientation.Horizontal:
                ModifyHorizontal(child);
                break;
            case Orientation.Vertical:
                ModifyVertical(child);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void ModifyHorizontal(DisplayObject child)
    {
        child.LeftTop = new Vector2f(
            LeftTop.X + Padding + GetWidthOfChildren(_children.IndexOf(child)),
            LeftTop.Y + Padding
        );
    }
    
    private void ModifyVertical(DisplayObject child)
    {
        child.LeftTop = new Vector2f(
            LeftTop.X + Padding,
            LeftTop.Y + Padding + GetHeightOfChildren(_children.IndexOf(child))
        );
    }
    
    private float GetWidthOfChildren(int index)
    {
        var width = 0f;
        for (var i = 0; i < index; i++) width += _children[i].Size.X + Spacing;
        return width;
    }
    
    private float GetHeightOfChildren(int index)
    {
        var height = 0f;
        for (var i = 0; i < index; i++) height += _children[i].Size.Y + Spacing;
        return height;
    }
    
    private void PropertyChanged()
    {
        _children.ForEach(ModifyChild);
    }

    public override void Destroy()
    {
        base.Destroy();
        _children.ForEach(child => child.Destroy());
    }

    protected override void OnPositionChanged()
    {
        base.OnPositionChanged();
        _background.Position = LeftTop;
        PropertyChanged();
    }
    
    protected override void OnSizeChanged()
    {
        base.OnSizeChanged();
        _background.Size = Size + new Vector2f(Padding * 2, Padding * 2);
        _background.Position = LeftTop;
        PropertyChanged();
    }

    protected override void OnEnabledChanged()
    {
        base.OnEnabledChanged();
        _children.ForEach(child => child.IsEnabled = IsEnabled);
    }
}