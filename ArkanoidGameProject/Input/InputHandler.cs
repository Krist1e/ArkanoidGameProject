using System.Diagnostics;
using ArkanoidGameProject.Input.Interfaces;
using ArkanoidGameProject.UI;
using ArkanoidGameProject.UI.Interfaces;
using SFML.System;
using SFML.Window;

namespace ArkanoidGameProject.Input;

public class InputHandler : IKeyboardInput, IMouseInput, IPlayerInput
{
    private readonly Window _window;

    private MouseButtonEventArgs? _lastMouseButtonRelease;
    private MouseButtonEventArgs? _lastMouseButtonPress;
    private MouseMoveEventArgs? _lastMouseMove;

    private KeyEventArgs? _lastKeyboardPress;
    private KeyEventArgs? _lastKeyboardRelease;
    
    private readonly Vector2u _initialWindowSize;

    public InputHandler(Window window)
    {
        _window = window;
        _initialWindowSize = window.Size;
        EventManager = new UIManager(this, this);

        _window.KeyPressed += OnKeyPressed;
        _window.KeyReleased += OnKeyReleased;
        _window.MouseButtonPressed += OnMousePressed;
        _window.MouseButtonReleased += OnMouseReleased;
        _window.MouseMoved += OnMouseMoved;
    }
    
    private Vector2u CurrentWindowSize => _window.Size;

    public IEventManager EventManager { get; }

    public KeyEventArgs? KeyPressed => _lastKeyboardPress;
    public KeyEventArgs? KeyReleased => _lastKeyboardRelease;
    public MouseMoveEventArgs? MouseMoved => _lastMouseMove;
    public MouseButtonEventArgs? MousePressed => _lastMouseButtonPress;
    public MouseButtonEventArgs? MouseReleased => _lastMouseButtonRelease;

    public Vector2f WindowScale => new(CurrentWindowSize.X / (float)_initialWindowSize.X, CurrentWindowSize.Y / (float)_initialWindowSize.Y);

    public MouseEventType LastMouseEvent
    {
        get
        {
            if (_lastMouseButtonPress != null) return MouseEventType.Pressed;
            if (_lastMouseButtonRelease != null) return MouseEventType.Released;
            if (_lastMouseMove != null) return MouseEventType.Moved;
            return MouseEventType.None;
        }
    }

    public event Action<float>? MoveLeft;
    public event Action<float>? MoveRight;
    public event Action? SpacePressed;
    public event Action? EscapePressed;

    public void HandleInput()
    {
        _window.DispatchEvents();
        EventManager.Update();
        ResetInput();
        CheckKeyboardInput();
    }

    private void CheckKeyboardInput()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A))
            MoveLeft?.Invoke(1);
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D))
            MoveRight?.Invoke(1);
    }

    private void ResetInput()
    {
        _lastMouseButtonRelease = null;
        _lastMouseButtonPress = null;
        _lastMouseMove = null;
        _lastKeyboardPress = null;
        _lastKeyboardRelease = null;
    }

    private void OnKeyPressed(object? sender, KeyEventArgs e)
    {
        _lastKeyboardPress = e;
        
        switch (e.Code)
        {
            case Keyboard.Key.Space:
                SpacePressed?.Invoke();
                break;
            case Keyboard.Key.Escape:
                EscapePressed?.Invoke();
                break;
        }
    }

    private void OnKeyReleased(object? sender, KeyEventArgs e) => _lastKeyboardRelease = e;
    private void OnMousePressed(object? sender, MouseButtonEventArgs e) => _lastMouseButtonPress = e;
    private void OnMouseReleased(object? sender, MouseButtonEventArgs e) => _lastMouseButtonRelease = e;
    private void OnMouseMoved(object? sender, MouseMoveEventArgs e) => _lastMouseMove = e;
}