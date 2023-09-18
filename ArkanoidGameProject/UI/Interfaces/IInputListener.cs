namespace ArkanoidGameProject.UI.Interfaces;

public interface IInputListener
{
    public bool IsEnabled { get; }
    
    public event Action<IInputListener>? Enabled;
    public event Action<IInputListener>? Disabled;
}