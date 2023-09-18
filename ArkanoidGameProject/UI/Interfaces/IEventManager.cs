using SFML.System;
using SFML.Window;

namespace ArkanoidGameProject.UI.Interfaces;

public interface IEventManager
{
    public void Register<TListener>(IInputListener listener) where TListener : IInputListener;
    public void Unregister<TListener>(IInputListener listener) where TListener : IInputListener;
    public void Update();
}