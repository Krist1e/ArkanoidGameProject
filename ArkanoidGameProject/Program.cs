namespace ArkanoidGameProject;

internal static class Program
{
    private static void Main(string[] args)
    {
        Game game = new Game(1280, 720, 60);
        game.Start();
    }
}