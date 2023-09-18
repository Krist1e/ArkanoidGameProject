namespace ArkanoidGameProject.Input;

public interface IPlayerInput
{
    public event Action<float>? MoveLeft;
    public event Action<float>? MoveRight;
    public event Action? SpacePressed;
}