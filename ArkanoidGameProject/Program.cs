namespace ArkanoidGameProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(800, 600, 60);
            game.Start();
        }
    }
}