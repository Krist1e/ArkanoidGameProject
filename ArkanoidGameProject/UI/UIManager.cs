using ArkanoidGameProject.Input;
using ArkanoidGameProject.Input.Interfaces;
using ArkanoidGameProject.UI.Interfaces;
using SFML.System;

namespace ArkanoidGameProject.UI;

public class UIManager : IEventManager
{
    private readonly IMouseInput _mouseInput;
    private readonly IKeyboardInput _keyboardInput;
    
    private readonly List<IMouseListener> _mouseListeners = new();
    private readonly List<IKeyboardListener> _keyboardListeners = new();

    private bool _isMouseListenersStateChanged;
    private bool _isKeyboardListenersStateChanged;

    private readonly List<IMouseListener> _mouseListenersToAdd = new();
    private readonly List<IKeyboardListener> _keyboardListenersToAdd = new();

    private readonly List<IMouseListener> _mouseListenersToRemove = new();
    private readonly List<IKeyboardListener> _keyboardListenersToRemove = new();

    public UIManager(IMouseInput mouseInput, IKeyboardInput keyboardInput)
    {
        _mouseInput = mouseInput;
        _keyboardInput = keyboardInput;
    }
    
    public Vector2f WindowScale => _mouseInput.WindowScale;

    public void Register<TListener>(IInputListener listener) where TListener : IInputListener
    {
        if (typeof(TListener) == typeof(IMouseListener) && listener is IMouseListener mouseListener)
        {
            mouseListener.Enabled += OnMouseListenerEnabled;
            mouseListener.Disabled += OnMouseListenerDisabled;
            _isMouseListenersStateChanged = true;
        }
        else if (typeof(TListener) == typeof(IKeyboardListener) && listener is IKeyboardListener keyboardListener)
        {
            keyboardListener.Enabled += OnKeyboardListenerEnabled;
            keyboardListener.Disabled += OnKeyboardListenerDisabled;
            _isKeyboardListenersStateChanged = true;
        }
        else
        {
            throw new ArgumentException(
                $"Listener type {typeof(TListener)} is not supported by {nameof(UIManager)}");
        }
    }

    public void Unregister<TListener>(IInputListener listener) where TListener : IInputListener
    {
        if (typeof(TListener) == typeof(IMouseListener) && listener is IMouseListener mouseListener)
        {
            _mouseListenersToRemove.Add(mouseListener);
            mouseListener.Enabled -= OnMouseListenerEnabled;
            mouseListener.Disabled -= OnMouseListenerDisabled;
            _isMouseListenersStateChanged = true;
        }
        else if (typeof(TListener) == typeof(IKeyboardListener) && listener is IKeyboardListener keyboardListener)
        {
            _keyboardListenersToRemove.Add(keyboardListener);
            keyboardListener.Enabled -= OnKeyboardListenerEnabled;
            keyboardListener.Disabled -= OnKeyboardListenerDisabled;
            _isKeyboardListenersStateChanged = true;
        }
        else
        {
            throw new ArgumentException(
                $"Listener type {typeof(TListener)} is not supported by {nameof(UIManager)}");
        }
    }

    public void Update()
    {
        UpdateMouseListeners();
        UpdateKeyboardListeners();
    }

    private void UpdateMouseListeners()
    {
        UpdateMousePendings();

        float mousePositionX;
        float mousePositionY;

        switch (_mouseInput.LastMouseEvent)
        {
            case MouseEventType.Moved:
                mousePositionX = _mouseInput.MouseMoved!.X;
                mousePositionY = _mouseInput.MouseMoved!.Y;
                break;
            case MouseEventType.Pressed:
                mousePositionX = _mouseInput.MousePressed!.X;
                mousePositionY = _mouseInput.MousePressed!.Y;
                break;
            case MouseEventType.Released:
                mousePositionX = _mouseInput.MouseReleased!.X;
                mousePositionY = _mouseInput.MouseReleased!.Y;
                break;
            case MouseEventType.None:
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        foreach (var obj in _mouseListeners)
        {
            var scaledLeftTop = obj.LeftTop.Scale(WindowScale);
            var scaledRightBottom = obj.RightBottom.Scale(WindowScale);
            if (mousePositionX >= scaledLeftTop.X && mousePositionX <= scaledRightBottom.X &&
                mousePositionY >= scaledLeftTop.Y && mousePositionY <= scaledRightBottom.Y)
            {
                if (_mouseInput.MouseMoved is not null) 
                    obj.OnMouseEnter(_mouseInput.MouseMoved);

                if (_mouseInput.MousePressed is not null)
                    obj.OnMousePress(_mouseInput.MousePressed);
                else if (_mouseInput.MouseReleased is not null) 
                    obj.OnMouseRelease(_mouseInput.MouseReleased);
            }
            else
            {
                if (_mouseInput.MouseMoved is not null) 
                    obj.OnMouseLeave(_mouseInput.MouseMoved);
            }
        }
    }

    private void UpdateMousePendings()
    {
        if (!_isMouseListenersStateChanged) return;

        _mouseListeners.RemoveAll(listener => _mouseListenersToRemove.Contains(listener));
        _mouseListenersToRemove.Clear();
        
        _mouseListeners.AddRange(_mouseListenersToAdd);
        _mouseListenersToAdd.Clear();

        _isMouseListenersStateChanged = false;
    }


    private void UpdateKeyboardListeners()
    {
        UpdateKeyboardPendings();
        
        foreach (var obj in _keyboardListeners)
        {
            if (_keyboardInput.KeyPressed is not null)
            {
                obj.OnKeyPressed(_keyboardInput.KeyPressed);
            }
            else if (_keyboardInput.KeyReleased is not null)
            {
                obj.OnKeyReleased(_keyboardInput.KeyReleased);
            }
        }
    }
    
    private void UpdateKeyboardPendings()
    {
        if (!_isKeyboardListenersStateChanged) return;
        
        _keyboardListeners.RemoveAll(listener => _keyboardListenersToRemove.Contains(listener));
        _keyboardListenersToRemove.Clear();
        
        _keyboardListeners.AddRange(_keyboardListenersToAdd);
        _keyboardListenersToAdd.Clear();

        _isKeyboardListenersStateChanged = false;
    }
    
    private void OnMouseListenerEnabled(IInputListener listener)
    {
        _mouseListenersToAdd.Add(listener as IMouseListener);
        _isMouseListenersStateChanged = true;
    }

    private void OnMouseListenerDisabled(IInputListener listener)
    {
        _mouseListenersToRemove.Add(listener as IMouseListener);
        _isMouseListenersStateChanged = true;
    }

    private void OnKeyboardListenerEnabled(IInputListener listener)
    {
        _keyboardListenersToAdd.Add(listener as IKeyboardListener);
        _isKeyboardListenersStateChanged = true;
    }

    private void OnKeyboardListenerDisabled(IInputListener listener)
    {
        _keyboardListenersToRemove.Add(listener as IKeyboardListener);
        _isKeyboardListenersStateChanged = true;
    }
}