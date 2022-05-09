using System;
using System.Collections.Generic;
using System.Linq;

namespace CMP1903M_Assessment_2
{
    public class GameUi
    {
        private int LeftMargin { set; get; } = 2;

        public static int MainMenu()
        {
            int index = 0;
            
            PrintMainMenu();

            while(true)
            {
                Console.CursorVisible = false;
                
                var keyPressed = Console.ReadKey();
                
                if (keyPressed.Key == ConsoleKey.RightArrow)
                {
                    if (index + 1 < 3)
                    {
                        index++;
                        PrintMainMenu(index);
                    }
                }
                else if (keyPressed.Key == ConsoleKey.LeftArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        PrintMainMenu(index);
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    return index;
                }
                else
                {
                    PrintMainMenu(index);
                }
            }
        }
        
        public static int GameOverScreen(Player playerOne, Player playerTwo)
        {
            int index = 0;
            
            PrintReplayMenu(playerOne, playerTwo);

            while(true)
            {
                Console.CursorVisible = false;
                
                var keyPressed = Console.ReadKey();
                
                if (keyPressed.Key == ConsoleKey.RightArrow)
                {
                    if (index + 1 < 3)
                    {
                        index++;
                        PrintReplayMenu(playerOne, playerTwo, index);
                    }
                }
                else if (keyPressed.Key == ConsoleKey.LeftArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        PrintReplayMenu(playerOne, playerTwo, index);
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    return index;
                }
                else
                {
                    PrintReplayMenu(playerOne, playerTwo, index);
                }
            }
        }

        public static string ReadKey(int limit = 0)
        {
            string output = "";
            ConsoleKeyInfo keyPressed;
            do
            {
                keyPressed = Console.ReadKey(true);

                if (keyPressed.Key == ConsoleKey.Backspace)
                {
                    if (output.Length > 0)
                    {
                        output = output.Remove(output.Length - 1, 1);
                        Console.CursorLeft -= 1;
                        Console.Write(" ");
                        Console.CursorLeft -= 1;
                    }
                }
                else if ((char.IsLetter(keyPressed.KeyChar) || keyPressed.Key == ConsoleKey.Spacebar) 
                         && (output.Length < limit || limit == 0))
                {
                    output += keyPressed.KeyChar.ToString();
                    Console.Write(keyPressed.KeyChar.ToString());
                }
            } while (keyPressed.Key != ConsoleKey.Enter);

            return output;
        }

        public static string GetPlayerName(string prompt)
        {
            Console.Clear();
            PrintMenuHeader();
            return PrintPrompt(prompt);
        }

        public static string PrintPrompt(string prompt, bool confirm = false)
        {
            prompt += ": ";
            Console.CursorTop -= 1;
            Console.WriteLine(@" ╠═══════════════════════════════════════════════════════════════╣");
            Console.WriteLine($" ║ {prompt.ToString(),-61} ║");
            Console.WriteLine(@" ╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@"Press enter to confirm.".PadCenter(66));
            Console.ResetColor();
            Console.CursorTop -= 3;
            Console.CursorLeft += prompt.Length + 3;
            
            return ReadKey(10);
        }

        public static bool QuitConfirmation()
        {
            int index = 0;
            //PrintQuitConfirmation();

            while(true)
            {
                PrintQuitConfirmation(index);
                Console.CursorVisible = false;
                
                var keyPressed = Console.ReadKey();
                
                if (keyPressed.Key == ConsoleKey.RightArrow)
                {
                    if (index + 1 < 2)
                    {
                        index++;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.LeftArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    if (index == 0)
                    {
                        return true;
                    }

                    return false;
                }
            }
        }

        private static void PrintQuitConfirmation(int selectedOption = 0)
        {
            string[,] yesStates = new string[,]
            {
                {
                    "         ",
                    "   Yes   ",
                    "         "
                },
                {
                    "╭╴     ╶╮",
                    "│  Yes  │",
                    "╰╴     ╶╯"
                }
            };
            
            string[,] noStates = new string[,]
            {
                {
                    "        ",
                    "   No   ",
                    "        "
                },
                {
                    "╭╴    ╶╮",
                    "│  No  │",
                    "╰╴    ╶╯"
                }
            };
            Console.Clear();
            PrintMenuHeader();
            int yesStateIndex = 0;
            int noStateIndex = 0;

            for (int i = 0; i < 3; i++)
            {
                Console.Write(" ║");
                Console.Write(i == 1 ? "  Are you sure you want to quit?      " : new string(' ', 38));

                if (selectedOption == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    yesStateIndex = 1;
                }

                Console.Write(yesStates[yesStateIndex, i]);
                Console.ResetColor();
                Console.Write("    ");

                if (selectedOption == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    noStateIndex = 1;
                }

                Console.Write(noStates[noStateIndex, i]);
                Console.ResetColor();
                Console.WriteLine("    ║");
            }
            
            Console.WriteLine(@" ╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@"Use arrow keys to select option.".PadCenter(66));
            Console.ResetColor();
        }

        /// <summary>
        /// Prints the board to the console.
        /// </summary>
        /// <param name="playerOne">Player one object</param>
        /// <param name="playerTwo">Player two object</param>
        /// <param name="dice">An array of D6 die.</param>
        /// <param name="message">Either a string or any enumerable type with strings (for multiple lines).</param>
        public static void PrintBoard(Player playerOne, Player playerTwo, D6[] dice, object message, 
            bool rollingDie = false, List<int> highlights = null)
        {
            string[] player1Score = DrawScore(playerOne.Score, 3);
            string[] player2Score = DrawScore(playerTwo.Score, 3);
            string[] dice1 = dice[0].DrawDie();
            string[] dice2 = dice[1].DrawDie();
            string[] dice3 = dice[2].DrawDie();
            string[] dice4 = dice[3].DrawDie();
            string[] dice5 = dice[4].DrawDie();

            if (highlights == null)
            {
                highlights = new List<int>();
            }

            Console.Clear();
            Console.WriteLine(" ╔══════════════╤═════════════════════════════════╤══════════════╗");
            Console.Write(" ║  ");
            if (playerTwo.CurrentTurn)
            {
                Console.Write(playerOne.Name.PadCenter(10));
            }
            else if (playerOne.CurrentTurn)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(playerOne.Name.PadCenter(10));
                Console.ResetColor();
            }
            Console.Write("  │");
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(playerOne.CurrentTurn ? " ← ← ←" : "      ");
            
            Console.Write(new string(' ', 21));
            
            Console.Write(playerTwo.CurrentTurn ? "→ → → " : "      ");
            Console.ResetColor();
            
            Console.Write("│  ");
            if (playerOne.CurrentTurn)
            {
                Console.Write(playerTwo.Name.PadCenter(10));
            }
            else if (playerTwo.CurrentTurn)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(playerTwo.Name.PadCenter(10));
                Console.ResetColor();
            }
            Console.WriteLine("  ║");

            for (int i = 0; i < 3; i++)
            {
                Console.Write($" ║ {player1Score[i]} │");
                Console.Write("                                 ");
                Console.WriteLine($"│ {player2Score[i]} ║");
            }
            Console.WriteLine(" ╟──────────────╯                                 ╰──────────────╢");
            Console.WriteLine(" ║                                                               ║");
            for (int i = 0; i < 5; i++)
            {
                Console.Write(" ║    ");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                if (highlights.Contains(dice[0].CurrentValue) && (!rollingDie || dice[0].Locked))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(dice1[i]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.Write(dice1[i]);
                }
                
                if (highlights.Contains(dice[1].CurrentValue) && (!rollingDie || dice[1].Locked))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(dice2[i]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.Write(dice2[i]);
                }
                
                if (highlights.Contains(dice[2].CurrentValue) && (!rollingDie || dice[2].Locked))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(dice3[i]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.Write(dice3[i]);
                }
                
                if (highlights.Contains(dice[3].CurrentValue) && (!rollingDie || dice[3].Locked))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(dice4[i]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.Write(dice4[i]);
                }
                
                if (highlights.Contains(dice[4].CurrentValue) && (!rollingDie || dice[4].Locked))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(dice5[i]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.Write(dice5[i]);
                }

                Console.ResetColor();
                Console.WriteLine("    ║");
            }
            Console.WriteLine(" ║                                                               ║");
            Console.WriteLine(" ╟───────────────────────────────────────────────────────────────╢");

            if (message is string)
            {
                Console.WriteLine($" ║ {message.ToString().PadCenter(61)} ║");
            }
            else if (message is IEnumerable<object> lines)
            {
                foreach (var line in lines)
                {
                    Console.WriteLine($" ║ {line.ToString().PadCenter(61)} ║");
                }
            }
            Console.WriteLine(" ╚═══════════════════════════════════════════════════════════════╝");
                
                
            //Console.WriteLine(" ║  Player One  │ ← Current Player                │  Player Two  ║");
            //Console.WriteLine(" ║ ╭──╮╭──╮╭──╮ │                                 │ ╭──╮ ╶┐ ╭──╮ ║");
            //Console.WriteLine(" ║ │  ││  ││  │ │                                 │ │  │  │ │  │ ║");
            //Console.WriteLine(" ║ ╰──╯╰──╯╰──╯ │                                 │ ╰──╯ ╶┴╴╰──╯ ║");
            //Console.WriteLine(" ╟──────────────╯                                 ╰──────────────╢");
            //Console.WriteLine(" ║                                                               ║");
            /*Console.WriteLine(" ║    ╭─────────╮╭─────────╮╭─────────╮╭─────────╮╭─────────╮    ║");
            Console.WriteLine(" ║    │         ││  ⊙      ││  ⊙      ││  ⊙   ⊙  ││  ⊙   ⊙  │    ║");
            Console.WriteLine(" ║    │    ⊙    ││         ││    ⊙    ││         ││    ⊙    │    ║");
            Console.WriteLine(" ║    │         ││      ⊙  ││      ⊙  ││  ⊙   ⊙  ││  ⊙   ⊙  │    ║");
            Console.WriteLine(" ║    ╰─────────╯╰─────────╯╰─────────╯╰─────────╯╰─────────╯    ║");
            Console.WriteLine(" ║                                                               ║");
            Console.WriteLine(" ╟───────────────────────────────────────────────────────────────╢");
            Console.WriteLine(" ║  Press any key to roll...                                     ║");
            Console.WriteLine(" ╚═══════════════════════════════════════════════════════════════╝");*/
        }

        private static void PrintMenuHeader()
        {
            string[] titleLogo = new[]
            {
                @"       ____                     __  __                       ",
                @"      |___ \                   |  \/  |                      ",
                @"        __) |     ___  _ __    | \  / | ___  _ __ ___        ",
                @"       |__ <     / _ \| '__|   | |\/| |/ _ \| '__/ _ \       ",
                @"       ___) |   | (_) | |      | |  | | (_) | | |  __/       ",
                @"      |____/     \___/|_|      |_|  |_|\___/|_|  \___|       "
            };
            
            Console.WriteLine(@" ╔═══════════════════════════════════════════════════════════════╗");
            foreach (string line in titleLogo)
            {
                Console.Write(" ║ ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(line);
                Console.ResetColor();
                Console.WriteLine(" ║");
            }
            Console.WriteLine(@" ║                                                               ║");
            Console.WriteLine(@" ╠═══════════════════════════════════════════════════════════════╣");
        }
        
        private static void PrintMainMenu(int selectedOption = 0)
        {
            string[,] playComputerStates = new string[,]
            {
                {
                    "                   ",
                    "   Play Computer   ",
                    "                   "
                },
                {
                    "╭╴               ╶╮",
                    "│  Play Computer  │",
                    "╰╴               ╶╯"
                }
            };

            string[,] twoPlayerStates = new string[,]
            {
                {
                    "                ",
                    "   Two Player   ",
                    "                "
                },
                {
                    "╭╴            ╶╮",
                    "│  Two Player  │",
                    "╰╴            ╶╯"
                }
            };

            string[,] quitStates = new string[,]
            {
                {
                    "          ",
                    "   Quit   ",
                    "          "
                },
                {
                    "╭╴      ╶╮",
                    "│  Quit  │",
                    "╰╴      ╶╯"
                }
            };

            Console.Clear();
            PrintMenuHeader();
            
            int playComputerStateIndex = 0;
            int twoPlayerStateIndex = 0;
            int quitStateIndex = 0;

            // Output three rows to draw the buttons. Colours the currently selected button.
            for (int i = 0; i < 3; i++)
            {
                Console.Write(" ║  ");
                if (selectedOption == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    playComputerStateIndex = 1;
                }
                Console.Write(playComputerStates[playComputerStateIndex, i]);
                Console.ResetColor();
                Console.Write("/".PadLeft(6-i) + new string(' ', i + 1));
                if (selectedOption == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    twoPlayerStateIndex = 1;
                }
                Console.Write(twoPlayerStates[twoPlayerStateIndex, i]);
                Console.ResetColor();
                Console.Write("/".PadLeft(6-i) + new string(' ', i + 1));
                if (selectedOption == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    quitStateIndex = 1;
                }
                Console.Write(quitStates[quitStateIndex, i]);
                Console.ResetColor();
                Console.WriteLine("  ║");
            }

            Console.WriteLine(@" ╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@"Use arrow keys to select option.".PadCenter(66));
            Console.ResetColor();
        }
        
        private static void PrintReplayMenu(Player playerOne, Player playerTwo, int selectedOption = 0)
        {
            string[,] playAgainStates = new string[,]
            {
                {
                    "                ",
                    "   Play Again   ",
                    "                "
                },
                {
                    "╭╴            ╶╮",
                    "│  Play Again  │",
                    "╰╴            ╶╯"
                }
            };

            string[,] newGameStates = new string[,]
            {
                {
                    "              ",
                    "   New Game   ",
                    "              "
                },
                {
                    "╭╴          ╶╮",
                    "│  New Game  │",
                    "╰╴          ╶╯"
                }
            };

            string[,] quitStates = new string[,]
            {
                {
                    "          ",
                    "   Quit   ",
                    "          "
                },
                {
                    "╭╴      ╶╮",
                    "│  Quit  │",
                    "╰╴      ╶╯"
                }
            };

            int playAgainStateIndex = 0;
            int newGameStateIndex = 0;
            int quitStateIndex = 0;

            PrintGameWon(playerOne, playerTwo);
            
            // Output three rows to draw the buttons. Colours the currently selected button.
            for (int i = 0; i < 3; i++)
            {
                Console.Write(" ║    ");
                if (selectedOption == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    playAgainStateIndex = 1;
                }
                Console.Write(playAgainStates[playAgainStateIndex, i]);
                Console.ResetColor();
                Console.Write("/".PadLeft(8-i) + new string(' ', i + 1));
                if (selectedOption == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    newGameStateIndex = 1;
                }
                Console.Write(newGameStates[newGameStateIndex, i]);
                Console.ResetColor();
                Console.Write("/".PadLeft(5-i) + new string(' ', i + 1));
                if (selectedOption == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    quitStateIndex = 1;
                }
                Console.Write(quitStates[quitStateIndex, i]);
                Console.ResetColor();
                Console.WriteLine("    ║");
            }

            Console.WriteLine(@" ╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(@"Use arrow keys to select option.".PadCenter(66));
            Console.ResetColor();
        }

        public static void PrintGameWon(Player playerOne, Player playerTwo)
        {
            Player winner;
            
            winner = playerOne.Score > playerTwo.Score ? playerOne : playerTwo;
            
            string[] gameOverMessage = new[]
            {
                @"  _____                         ____                 ",
                @" / ____|                       / __ \                ",
                @"| |  __  __ _ _ __ ___   ___  | |  | |_   _____ _ __ ",
                @"| | |_ |/ _` | '_ ` _ \ / _ \ | |  | \ \ / / _ \ '__|",
                @"| |__| | (_| | | | | | |  __/ | |__| |\ V /  __/ |   ",
                @" \_____|\__,_|_| |_| |_|\___|  \____/  \_/ \___|_|   "
            };

            Console.Clear();
            Console.WriteLine(@" ╔═══════════════════════════════════════════════════════════════╗");
            foreach (string line in gameOverMessage)
            {
                Console.Write(" ║ ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(line.ToString().PadCenter(61));
                Console.ResetColor();
                Console.WriteLine(" ║");
            }
            Console.WriteLine($" ║ {" ".PadCenter(61)} ║");
            Console.WriteLine($" ║ {(winner.Name + " won the game with a score of "+ winner.Score).PadCenter(61)} ║");
            Console.WriteLine($" ║ {" ".PadCenter(61)} ║");
            Console.WriteLine(@" ╠═══════════════════════════════════════════════════════════════╣");
        }

        // ---- -------- ---- -------- ---- -------- ---- -------- ---- -------- ---- -------- ---- -------- ---- -------- ---- -------- ---- --------    
        // ╭──╮ ╭──────╮  ╶┐     ╶─┐   ╭──╮ ╭──────╮                                                                                                             
        // │  │ │      │   │       │   ╭──┘        │                                                                                                         
        // ╰──╯ │      │  ╶┴╴      │   └──╴ ╭──────┘                                                                                                              
        //      │      │           │        │                                                                                                             
        //      ╰──────╯         ╶─┴─╴      └──────╴                                                                                                            
        
        private static string[] DrawScore(int score, int numberOfDigits = 0)
        {
            string[,] numbers = new string[,]
            {
                {
                    "╭──╮",
                    "│  │",
                    "╰──╯"
                },
                {
                    " ╶┐ ",
                    "  │ ",
                    " ╶┴╴"
                },
                {
                    "╭──╮",
                    "╭──┘",
                    "└──╴"
                },
                {
                    "╭──╮",
                    "  ─┤",
                    "╰──╯"
                },
                {
                    "╷  ╷",
                    "└──┤",
                    "   ╵"
                },
                {
                    "┌──╴",
                    "└──╮",
                    "╰──╯"
                },
                {
                    "╭──╴",
                    "├──╮",
                    "╰──╯"
                },
                {
                    "╶──╮",
                    "   │",
                    "   ╵"
                },
                {
                    "╭──╮",
                    "├──┤",
                    "╰──╯"
                },
                {
                    "╭──╮",
                    "╰──┤",
                    "   ╵"
                }
            };
            
            string numberAsString = score.ToString(new string('0', numberOfDigits));
            string[] output = new[] {"", "", ""};
            
            foreach (var number in numberAsString)
            {
                output[0] += numbers[Int32.Parse(number.ToString()), 0];
                output[1] += numbers[Int32.Parse(number.ToString()), 1];
                output[2] += numbers[Int32.Parse(number.ToString()), 2];
            }

            return output;
        } 
    }
}