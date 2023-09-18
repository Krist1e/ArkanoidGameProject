using SFML.System;
using SFML.Window;

namespace ArkanoidGameProject.UI.Interfaces;

public interface IMouseListener : IInputListener
{
    public Vector2f LeftTop { get; }
    public Vector2f RightBottom { get; }
    public void OnMouseEnter(MouseMoveEventArgs args);
    public void OnMouseLeave(MouseMoveEventArgs args);
    public void OnMousePress(MouseButtonEventArgs args);
    public void OnMouseRelease(MouseButtonEventArgs args);
}