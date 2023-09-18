using SFML.Window;

namespace ArkanoidGameProject.UI.Interfaces;

public interface IKeyboardListener : IInputListener
{
    public void OnKeyPressed(KeyEventArgs args);
    public void OnKeyReleased(KeyEventArgs args);
}