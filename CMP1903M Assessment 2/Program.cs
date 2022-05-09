using System;
using System.Threading;

namespace CMP1903M_Assessment_2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Game game = new Game();
            game.StartGame();

            Environment.Exit(0);


/*

            Console.WriteLine();
            Console.WriteLine(" ╔══════════════╤═════════════════════════════════╤══════════════╗");
            Console.WriteLine(" ║  Player One  │ ← Current Player →              │  Player Two  ║");
            Console.WriteLine(" ║ ╭──╮╭──╮╭──╮ │                                 │ ╭──╮ ╶┐ ╭──╮ ║");
            Console.WriteLine(" ║ │  ││  ││  │ │                                 │ │  │  │ │  │ ║");
            Console.WriteLine(" ║ ╰──╯╰──╯╰──╯ │                                 │ ╰──╯ ╶┴╴╰──╯ ║");
            Console.WriteLine(" ╟──────────────╯                                 ╰──────────────╢");
            Console.WriteLine(" ║                                                               ║");
            Console.WriteLine(" ║    ╭─────────╮╭─────────╮╭─────────╮╭─────────╮╭─────────╮    ║");
            Console.WriteLine(" ║    │         ││  ⊙      ││  ⊙      ││  ⊙   ⊙  ││  ⊙   ⊙  │    ║");
            Console.WriteLine(" ║    │    ⊙    ││         ││    ⊙    ││         ││    ⊙    │    ║");
            Console.WriteLine(" ║    │         ││      ⊙  ││      ⊙  ││  ⊙   ⊙  ││  ⊙   ⊙  │    ║");
            Console.WriteLine(" ║    ╰─────────╯╰─────────╯╰─────────╯╰─────────╯╰─────────╯    ║");
            Console.WriteLine(" ║                                                               ║");
            Console.WriteLine(" ╟───────────────────────────────────────────────────────────────╢");
            Console.WriteLine(" ║  Press any key to roll...                                     ║");
            Console.WriteLine(" ╚═══════════════════════════════════════════════════════════════╝");
            Console.ReadKey();
            DieAnimation();
            Console.ReadKey();
         */

        }

        public static string[] PrintRandomDice()
        {
            string[,] dieFaces = new string[,]
            {
                {
                    "│         │",
                    "│    ⊙    │",
                    "│         │"
                },
                {
                    "│ ⊙       │",
                    "│         │",
                    "│       ⊙ │"
                },
                {
                    "│ ⊙       │",
                    "│    ⊙    │",
                    "│       ⊙ │"
                },
                {
                    "│ ⊙     ⊙ │",
                    "│         │",
                    "│ ⊙     ⊙ │"
                },
                {
                    "│ ⊙     ⊙ │",
                    "│    ⊙    │",
                    "│ ⊙     ⊙ │"
                },
                {
                    "│ ⊙     ⊙ │",
                    "│ ⊙     ⊙ │",
                    "│ ⊙     ⊙ │"
                }
            };

            var rand = new Random();
            int index = rand.Next(6);
            
            string[] randomDice = new string[]
            {
                "╭─────────╮",
                dieFaces[index, 0],
                dieFaces[index, 1],
                dieFaces[index, 2],
                "╰─────────╯"
            };

            return randomDice;
        }
        
        public static void DieAnimation()
        {
            int sleep = 8;

            while(sleep < 300)
            {
                string[] dice1 = PrintRandomDice();
                string[] dice2 = PrintRandomDice();
                string[] dice3 = PrintRandomDice();
                string[] dice4 = PrintRandomDice();
                string[] dice5 = PrintRandomDice();
                
                Console.WriteLine(dice1[0] + dice2[0] + dice3[0] + dice4[0] + dice5[0]);
                Console.WriteLine(dice1[1] + dice2[1] + dice3[1] + dice4[1] + dice5[1]);
                Console.WriteLine(dice1[2] + dice2[2] + dice3[2] + dice4[2] + dice5[2]);
                Console.WriteLine(dice1[3] + dice2[3] + dice3[3] + dice4[3] + dice5[3]);
                Console.WriteLine(dice1[4] + dice2[4] + dice3[4] + dice4[4] + dice5[4]);
                
                Console.CursorTop -= 5;
                
                Thread.Sleep(sleep);
                
                sleep = Convert.ToInt32(sleep * 1.1);
            }
        }
    }
}