using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace ArkanoidGameProject.UI.Components;

public class RadioButton : Button
{
    private static readonly Dictionary<string, RadioButtonGroup> Groups = new();
    
    private string _group = "default";
    
    public RadioButton()
    {
        Groups.TryGetValue(_group, out var group);
        group ??= new RadioButtonGroup();
        group.Add(this);
        Groups[_group] = group;
    }
    
    public string Group
    {
        get => _group;
        set
        {
            if (_group == value)
                return;
            
            Groups[_group].Remove(this);
            _group = value;
            Groups.TryGetValue(_group, out var group);
            group ??= new RadioButtonGroup();
            group.Add(this);
            Groups[_group] = group;
            
            if (IsChecked)
                group.Check(this);
        }
    }

    public bool IsChecked { get; set; }

    public override void Draw(RenderWindow window)
    {
        base.Draw(window);
        if (!IsChecked) return;
        var circle = new CircleShape(10)
        {
            FillColor = Color.Black,
            Origin = new Vector2f(10, 10),
            Position = new Vector2f(RightBottom.X - 20, Position.Y)
        };
        window.Draw(circle);
    }

    public override void OnMousePress(MouseButtonEventArgs args)
    {
        base.OnMousePress(args);
        Groups[_group].Check(this);
    }

    public override void Destroy()
    {
        base.Destroy();
        Groups[_group].Remove(this);
    }

    protected override void OnEnabledChanged()
    {
        base.OnEnabledChanged();
        if (IsEnabled)
            Groups[_group].Add(this);
        else
            Groups[_group].Remove(this);
    }

    private class RadioButtonGroup
    {
        private readonly List<RadioButton> _radioButtons = new();

        public void Add(RadioButton radioButton)
        {
            _radioButtons.Add(radioButton);
        }
        
        public void Remove(RadioButton radioButton)
        {
            _radioButtons.Remove(radioButton);
        }
        
        public void Check(RadioButton radioButton)
        {
            _radioButtons.ForEach(button => button.IsChecked = radioButton == button);
        }
    }
}