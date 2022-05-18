using System;
using System.Text;

namespace CMP1903M_Assessment_2
{
    /// <summary>
    /// Main entry way for the program.
    /// </summary>
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            Game game = new Game();
            game.StartGame();

            Environment.Exit(0);
        }
    }
}