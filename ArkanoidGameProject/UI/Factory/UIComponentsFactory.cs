using ArkanoidGameProject.GameObjects;
using ArkanoidGameProject.UI.Components;
using ArkanoidGameProject.UI.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace ArkanoidGameProject.UI.Factory;

public class UIComponentsFactory
{
    private readonly IEventManager _eventManager;
    private readonly TextInfo _textInfo;

    public UIComponentsFactory(IEventManager eventManager, TextInfo textInfo)
    {
        _eventManager = eventManager;
        _textInfo = textInfo;
    }

    public Button CreateButton(string content, Color color, Color hoverColor, Color textColor, Vector2f position, Vector2f size)
    {
        var button = new Button
        {
            TextInfo = _textInfo,
            Content = content,
            Color = color,
            HoverColor = hoverColor,
            TextColor = textColor,
            Position = position,
            Size = size,
            ZIndex = 2
        };
        _eventManager.Register<IMouseListener>(button);
        return button;
    }
    
    public RadioButton CreateRadioButton(string content, Color color, Color hoverColor, Color textColor, Vector2f position, Vector2f size)
    {
        var radioButton = new RadioButton
        {
            TextInfo = _textInfo,
            Content = content,
            Color = color,
            HoverColor = hoverColor,
            TextColor = textColor,
            Position = position,
            Size = size,
            ZIndex = 2
            
        };
        _eventManager.Register<IMouseListener>(radioButton);
        return radioButton;
    }

    public TextLabel CreateTextLabel(string content, Color color, Vector2f position = new(), Vector2f size = new())
    {
        var textLabel = new TextLabel
        {
            TextInfo = _textInfo,
            Content = content,
            Color = color,
            Position = position,
            Size = size,
            ZIndex = 2
        };
        return textLabel;
    }

    public TextBox CreateTextBox(string content, Color color, Color hoverColor, Color focusColor, Vector2f position = new(), Vector2f size = new())
    {
        var textBox = new TextBox
        {
            TextInfo = _textInfo,
            Content = content,
            Color = color,
            HoverColor = hoverColor,
            FocusColor = focusColor,
            Position = position,
            Size = size,
            ZIndex = 2
        };
        _eventManager.Register<IMouseListener>(textBox);
        _eventManager.Register<IKeyboardListener>(textBox);
        return textBox;
    }
    
    public StackPanel CreateStackPanel(Vector2f position = new(), Vector2f size = new())
    {
        var stackPanel = new StackPanel
        {
            Position = position,
            Size = size,
            ZIndex = 2
        };
        return stackPanel;
    }
}