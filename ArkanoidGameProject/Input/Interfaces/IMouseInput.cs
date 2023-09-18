using SFML.System;
using SFML.Window;

namespace ArkanoidGameProject.Input.Interfaces;

public enum MouseEventType
{
    Moved,
    Pressed,
    Released,
    None
}

public interface IMouseInput
{
    public Vector2f WindowScale { get; }
    public MouseEventType LastMouseEvent { get; }
    public MouseMoveEventArgs? MouseMoved { get; }
    public MouseButtonEventArgs? MousePressed { get; }
    public MouseButtonEventArgs? MouseReleased { get; }
}