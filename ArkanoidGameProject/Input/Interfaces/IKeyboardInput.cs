using SFML.Window;

namespace ArkanoidGameProject.Input;

public interface IKeyboardInput
{
    public KeyEventArgs? KeyPressed { get; }
    public KeyEventArgs? KeyReleased { get; }
    
}