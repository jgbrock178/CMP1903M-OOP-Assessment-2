using System;
using System.Collections.Generic;
using System.Linq;
using CMP1903M_Assessment_2.Extensions;

namespace CMP1903M_Assessment_2.Classes
{
    /// <summary>
    /// Contains all logic for the Game UI in the console.
    /// </summary>
    public class GameUi
    {
        // Protected to allow future 
        private string LeftMargin { get; } = new string(' ', 2);
        private Player[] _players;
        private Player _currentPlayer;
        
        /// <summary>
        /// Current round number.
        /// </summary>
        public int RoundNumber { set; get; }
        
        /// <summary>
        /// Sets the players in the UI.
        /// </summary>
        /// <param name="value">A Player array of game players.</param>
        public void SetPlayers(ref Player[] value)
        {
            _players = value;
        }

        private Player[] Players()
        {
            return _players;
        }
        
        /// <summary>
        /// Wrapper to move the cursor to the top left of the console. Used to write over previous content. Usually
        /// console.clear() would be sufficient, however this causes screen flashing on Windows terminal. Over-writing
        /// previous content fixes this.
        /// </summary>
        private void ResetCursorPosition()
        {
            ClearEndOfConsole();
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Writes 5 blank lines to the console. Used to ensure any UI that is fewer lines will not show the previous
        /// UI screen below it.
        /// </summary>
        private void ClearEndOfConsole()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(new string(' ', 80));   
            }
            Console.CursorTop -= 5;
        }

        /// <summary>
        /// Sets the current player in the UI.
        /// </summary>
        /// <param name="currentPlayer">Player object of the current player.</param>
        /// <returns>Returns the current player.</returns>
        public Player SetCurrentPlayer(ref Player currentPlayer)
        {
            _currentPlayer = currentPlayer;
            return _currentPlayer;
        }

        /// <summary>
        /// Current player object.
        /// </summary>
        /// <returns>Current player object.</returns>
        private Player CurrentPlayer()
        {
            return _currentPlayer;
        }

        /// <summary>
        /// Outputs the main menu to the console.
        /// </summary>
        /// <returns>An integer representing the selected option.</returns>
        public int MainMenu()
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
        
        /// <summary>
        /// Outputs the game over screen to the console.
        /// </summary>
        /// <returns>An integer representing the option selected.</returns>
        public int GameOverScreen()
        {
            int index = 0;
            
            PrintReplayMenu();

            while(true)
            {
                Console.CursorVisible = false;
                
                var keyPressed = Console.ReadKey();
                
                if (keyPressed.Key == ConsoleKey.RightArrow)
                {
                    if (index + 1 < 3)
                    {
                        index++;
                        PrintReplayMenu(index);
                    }
                }
                else if (keyPressed.Key == ConsoleKey.LeftArrow)
                {
                    if (index - 1 >= 0)
                    {
                        index--;
                        PrintReplayMenu(index);
                    }
                }
                else if (keyPressed.Key == ConsoleKey.Enter)
                {
                    Console.CursorVisible = true;
                    return index;
                }
                else
                {
                    PrintReplayMenu(index);
                }
            }
        }

        /// <summary>
        /// Allows the input of text in the console, limited by a number of digits. Will only allow integers and numbers
        /// to be entered - all other characters are ignored. When limit is hit, no further characters will be entered
        /// (user will see no further characters output in terminal).
        /// </summary>
        /// <param name="limit">The limit of the input text. If non given, will allow unlimited text length.</param>
        /// <returns>A string of the entered text.</returns>
        private string ReadKey(int limit = 0)
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
                else if ((char.IsLetter(keyPressed.KeyChar) || char.IsNumber(keyPressed.KeyChar) || keyPressed.Key == ConsoleKey.Spacebar) 
                         && (output.Length < limit || limit == 0))
                {
                    output += keyPressed.KeyChar.ToString();
                    Console.Write(keyPressed.KeyChar.ToString());
                }
            } while (keyPressed.Key != ConsoleKey.Enter);

            return output;
        }

        /// <summary>
        /// Prompts the user to enter their name via the console.
        /// </summary>
        /// <param name="prompt">The prompt text to print.</param>
        /// <returns>A string containing the players name.</returns>
        public string GetPlayerName(string prompt)
        {
            ResetCursorPosition();
            PrintMenuHeader();
            prompt += ": ";
            Console.CursorTop -= 1;
            Console.WriteLine(LeftMargin + @"╠═══════════════════════════════════════════════════════════════╣");
            Console.WriteLine(LeftMargin + $"║ {prompt,-61} ║");
            Console.WriteLine(LeftMargin + @"╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(LeftMargin + @"Maximum 10 characters. Press enter to confirm.".PadCenter(66));
            Console.ResetColor();
            Console.CursorTop -= 3;
            Console.CursorLeft += prompt.Length + LeftMargin.Length + 2;
            
            return ReadKey(10);
        }

        /// <summary>
        /// Prompts the user for input via the console and only allows validated input. It invalid input entered, shows
        /// an error to the user and re-prompts.
        /// </summary>
        /// <param name="prompt">The prompt to output to the console.</param>
        /// <param name="validInputs">A string array of the valid inputs allowed.</param>
        /// <returns>A string representing the entered text.</returns>
        public string GetValidatedUserInput(string prompt, string[] validInputs)
        {
            prompt += ": ";
            ResetCursorPosition();
            PrintMenuHeader();
            Console.CursorTop -= 1;
            Console.WriteLine(LeftMargin + @"╠═══════════════════════════════════════════════════════════════╣");
            Console.WriteLine(LeftMargin + $"║ {prompt,-61} ║");
            Console.WriteLine(LeftMargin + @"╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(LeftMargin + @"Press enter to confirm.".PadCenter(66));
            Console.ResetColor();
            ClearEndOfConsole();
            Console.CursorTop -= 3;
            Console.CursorLeft += prompt.Length + LeftMargin.Length + 2;
            
            string option = ReadKey();
            Console.CursorTop += 1;
            
            while (!validInputs.Contains(option))
            {
                Console.WriteLine();
                Console.CursorTop -= 3;
                Console.WriteLine(LeftMargin + @"╠═══════════════════════════════════════════════════════════════╣");
                Console.Write(LeftMargin + "║ ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid input, please try again.".PadRight(61));
                Console.ResetColor();
                Console.WriteLine(" ║");
                Console.WriteLine(LeftMargin + $"║ {prompt,-61} ║");
                Console.WriteLine(LeftMargin + @"╚═══════════════════════════════════════════════════════════════╝");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(LeftMargin + @"Press enter to confirm.".PadCenter(66));
                Console.ResetColor();
                Console.CursorTop -= 3;
                Console.CursorLeft += prompt.Length + LeftMargin.Length + 2;
                option = ReadKey();
            }

            return option;
        }

        /// <summary>
        /// Shows a confirmation menu to the user asking whether they really want to quit and handles logic for their
        /// input.
        /// </summary>
        /// <returns>A boolean representing the users chosen option.</returns>
        public bool QuitConfirmation()
        {
            int index = 0;

            while(true)
            {
                PrintQuitConfirmation(index);
                Console.CursorVisible = false;
                
                var keyPressed = Console.ReadKey(true);
                
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

        /// <summary>
        /// Prints the confirmation for quitting based on what option is currently selected.
        /// </summary>
        /// <param name="selectedOption">The index of the currently selected option.</param>
        private void PrintQuitConfirmation(int selectedOption = 0)
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
            
            ResetCursorPosition();
            PrintMenuHeader();
            int yesStateIndex = 0;
            int noStateIndex = 0;

            for (int i = 0; i < 3; i++)
            {
                Console.Write(LeftMargin + "║");
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
            
            Console.WriteLine(LeftMargin + @"╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(LeftMargin + @"Use arrow keys to select option and press enter to confirm.".PadCenter(66));
            ClearEndOfConsole();
            Console.ResetColor();
        }
        
        /// <summary>
        /// Prints the game board showing the current state.
        /// </summary>
        /// <param name="dice">A D6 array representing the game dice.</param>
        /// <param name="message">The message to output at the bottom of the UI.</param>
        public void PrintBoard(D6[] dice, object message) {
            string[] dice1 = dice[0].DrawCurrentDie();
            string[] dice2 = dice[1].DrawCurrentDie();
            string[] dice3 = dice[2].DrawCurrentDie();
            string[] dice4 = dice[3].DrawCurrentDie();
            string[] dice5 = dice[4].DrawCurrentDie();

            // Print the game header
            if (_players.Length == 2)
            {
                PrintTwoPlayerBoardHeader();
            }
            else
            {
                PrintMultiplayerBoardHeader();
            }
            
            Console.WriteLine(LeftMargin + "║                                                               ║");
            for (int i = 0; i < 5; i++)
            {
                Console.Write(LeftMargin + "║    ");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                if (dice[0].Locked)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(dice1[i]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.Write(dice1[i]);
                }
                
                if (dice[1].Locked)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(dice2[i]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.Write(dice2[i]);
                }
                
                if (dice[2].Locked)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(dice3[i]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.Write(dice3[i]);
                }
                
                if (dice[3].Locked)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(dice4[i]);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    Console.Write(dice4[i]);
                }
                
                if (dice[4].Locked)
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
            Console.WriteLine(LeftMargin + "║                                                               ║");
            Console.WriteLine(LeftMargin + "╟───────────────────────────────────────────────────────────────╢");

            if (message is string)
            {
                Console.WriteLine(LeftMargin + $"║ {message.ToString().PadCenter(61)} ║");
            }
            else if (message is IEnumerable<object> lines)
            {
                foreach (var line in lines)
                {
                    Console.WriteLine(LeftMargin + $"║ {line.ToString().PadCenter(61)} ║");
                }
            }
            Console.WriteLine(LeftMargin + "╚═══════════════════════════════════════════════════════════════╝");
            
        }

        /// <summary>
        /// Prints the header for the game board when only 2 players are playing.
        /// </summary>
        /// <example>
        /// Example of the board output:
        /// <code>
        ///  ╔══════════════╤═════════════════════════════════╤══════════════╗
        ///  ║      Max     │ ← ← ←                           │     Jack     ║
        ///  ║ ╭──╮╭──╮╭──╮ │                                 │ ╭──╮╭──╮╭──╮ ║
        ///  ║ │  ││  ││  │ │                                 │ │  ││  ││  │ ║
        ///  ║ ╰──╯╰──╯╰──╯ │                                 │ ╰──╯╰──╯╰──╯ ║
        ///  ╟──────────────╯                                 ╰──────────────╢
        /// </code>
        /// </example>
        private void PrintTwoPlayerBoardHeader()
        {
            Player playerOne = Players()[0];
            Player playerTwo = Players()[1];

            string[] player1Score = DrawScore(playerOne.Score, 3);
            string[] player2Score = DrawScore(playerTwo.Score, 3);
            
            ResetCursorPosition();
            Console.WriteLine("");
            Console.WriteLine(LeftMargin + "╔══════════════╤═════════════════════════════════╤══════════════╗");
            Console.Write(LeftMargin + "║  ");
            if (playerTwo == CurrentPlayer())
            {
                Console.Write(playerOne.Name.PadCenter(10));
            }
            else if (playerOne == CurrentPlayer())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(playerOne.Name.PadCenter(10));
                Console.ResetColor();
            }
            Console.Write("  │");
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(playerOne == _currentPlayer ? " ← ← ←" : "      ");
            
            Console.Write(new string(' ', 21));
            
            Console.Write(playerTwo == _currentPlayer ? "→ → → " : "      ");
            Console.ResetColor();
            
            Console.Write("│  ");
            if (playerOne == CurrentPlayer())
            {
                Console.Write(playerTwo.Name.PadCenter(10));
            }
            else if (playerTwo == CurrentPlayer())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(playerTwo.Name.PadCenter(10));
                Console.ResetColor();
            }
            Console.WriteLine("  ║");

            for (int i = 0; i < 3; i++)
            {
                Console.Write(LeftMargin + $"║ {player1Score[i]} │");
                Console.Write("                                 ");
                Console.WriteLine($"│ {player2Score[i]} ║");
            }
            Console.WriteLine(LeftMargin + "╟──────────────╯                                 ╰──────────────╢");
        }

        /// <summary>
        /// Prints the game board header when more than 2 players are playing.
        /// </summary>
        /// <example>
        /// Example of the board output:
        /// <code>
        ///   ╔══════════════╤══════════════╤═════════════════════════════════╗
        ///   ║      Max     │    Jenny     │                                 ║
        ///   ║ ╭──╮╭──╮╭──╮ │ ╭──╮╭──╮╭──╮ │   Max  Jen  Mar  Lis  Sco  Bob  ║
        ///   ║ │  ││  ││  │ │ │  ││  ││  │ │   000  000  000  000  010  000  ║
        ///   ║ ╰──╯╰──╯╰──╯ │ ╰──╯╰──╯╰──╯ │         ▲                       ║
        ///   ╟─╴LAST ROUND╶─┴─╴THIS ROUND╶─┴─────────────╴SCORES╶────────────╢
        /// </code>
        /// </example>
        private void PrintMultiplayerBoardHeader()
        {
            int lastPlayerIndex = CurrentPlayer().PlayerNumber - 2 >= 0 ? CurrentPlayer().PlayerNumber - 2 : Players().Length - 1;
            Player lastPlayer = _players[lastPlayerIndex];
            string[] lastRoundScore = DrawScore(lastPlayer.Score, 3);
            string[] currentScore = DrawScore(CurrentPlayer().Score, 3);
            string[][] scoreMap = PrepareScoreMap();
            
            ResetCursorPosition();
            Console.WriteLine("");
            Console.WriteLine(LeftMargin + "╔══════════════╤══════════════╤═════════════════════════════════╗");
            Console.Write(LeftMargin + "║ ");
            if (RoundNumber == 1)
            {
                Console.Write("---".PadCenter(12) + " │ ");
            }
            else
            {
                Console.Write(lastPlayer.Name.PadCenter(12) + " │ ");
            }
            Console.WriteLine(CurrentPlayer().Name.PadCenter(12) + " │                                 ║");

            int mapWhitespace = 31 - scoreMap[0].Length * 3;
            int mapInterSpace = scoreMap[0].Length - 1;
            int mapLeftWhitespace = (mapWhitespace - mapInterSpace) / 2;
            int mapRightWhitespace = mapWhitespace - mapInterSpace - mapLeftWhitespace;
            
            for (int i = 0; i < 3; i++)
            {
                Console.Write(LeftMargin + "║ " + lastRoundScore[i] + " │ ");
                Console.Write(currentScore[i] + " │ ");

                Console.Write(new string(' ', mapLeftWhitespace));
                for (int j = 0; j < scoreMap[i].Length; j++)
                {
                    if (CurrentPlayer().PlayerNumber == j + 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                        
                    Console.Write(scoreMap[i][j]);
                    Console.ResetColor();
                    if (j < scoreMap[i].Length - 1)
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine(new string(' ', mapRightWhitespace) + " ║");
            }

            Console.Write(LeftMargin + "╟─╴");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Last Round");
            Console.ResetColor();
            Console.Write("╶─┴─╴");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("This Round");
            Console.ResetColor();
            Console.Write("╶─┴─────────────╴");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Scores");
            Console.ResetColor();
            Console.WriteLine("╶────────────╢");
        }

        /// <summary>
        /// Prepares the score map for for the game board when more than 2 players are playing.
        /// </summary>
        /// <returns>
        /// A string[][] multi-dimensional array containing:
        /// <list type="bullet">
        /// <item>First 3 digits of name.</item>
        /// <item>Current score.</item>
        /// <item>Space or an indicator for the current player.</item>
        /// </list>
        /// <example>
        /// <code>
        ///   Max  Jen  Mar  Lis  Sco  Bob  (First 3 letters of name)
        ///   000  000  000  000  010  000  (Score)
        ///         ▲                       (Current player indicator)
        /// </code>
        /// </example>
        /// </returns>
        private string[][] PrepareScoreMap()
        {
            List<List<string>> outputData = new List<List<string>>()
            {
                new List<string>(), new List<string>(), new List<string>()
            };
            
            foreach (var player in _players)
            {
                outputData[0].Add(player.Name.Length < 4 ? player.Name.PadCenter(3) : player.Name.Substring(0, 3));
                outputData[1].Add(player.Score.ToString("D3"));
                outputData[2].Add(player == CurrentPlayer() ? " ▲ " : "   ");
            }

            string[][] output = new[]
            {
                outputData[0].ToArray(),
                outputData[1].ToArray(),
                outputData[2].ToArray()
            };
            return output;
        }

        /// <summary>
        /// Prints the menu header to the console.
        /// </summary>
        private void PrintMenuHeader()
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
            
            Console.WriteLine("");
            Console.WriteLine(LeftMargin + @"╔═══════════════════════════════════════════════════════════════╗");
            foreach (string line in titleLogo)
            {
                Console.Write(LeftMargin + "║ ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(line);
                Console.ResetColor();
                Console.WriteLine(" ║");
            }
            Console.WriteLine(LeftMargin + @"║                                                               ║");
            Console.WriteLine(LeftMargin + @"╠═══════════════════════════════════════════════════════════════╣");
        }
        
        /// <summary>
        /// Prints the main menu to the console highlighting the current selection.
        /// </summary>
        /// <param name="selectedOption">The index of the selected option.</param>
        private void PrintMainMenu(int selectedOption = 0)
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

            string[,] multiPlayerStates = new string[,]
            {
                {
                    "                 ",
                    "   Multiplayer   ",
                    "                 "
                },
                {
                    "╭╴             ╶╮",
                    "│  Multiplayer  │",
                    "╰╴             ╶╯"
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
            
            int playComputerStateIndex = 0;
            int multiPlayerStateIndex = 0;
            int quitStateIndex = 0;
            
            ResetCursorPosition();
            PrintMenuHeader();

            // Output three rows to draw the buttons. Colours the currently selected button.
            for (int i = 0; i < 3; i++)
            {
                Console.Write(LeftMargin + "║  ");
                if (selectedOption == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    playComputerStateIndex = 1;
                }
                Console.Write(playComputerStates[playComputerStateIndex, i]);
                Console.ResetColor();
                Console.Write("/".PadLeft(5-i) + new string(' ', i + 1));
                if (selectedOption == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    multiPlayerStateIndex = 1;
                }
                Console.Write(multiPlayerStates[multiPlayerStateIndex, i]);
                Console.ResetColor();
                Console.Write("/".PadLeft(5-i) + new string(' ', i + 2));
                if (selectedOption == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    quitStateIndex = 1;
                }
                Console.Write(quitStates[quitStateIndex, i]);
                Console.ResetColor();
                Console.WriteLine("  ║");
            }

            Console.WriteLine(LeftMargin + @"╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(LeftMargin + @"Use arrow keys to select option and press enter to confirm.".PadCenter(66));
            ClearEndOfConsole();
            Console.ResetColor();
        }
        
        /// <summary>
        /// Prints the replay menu at the end of each game, highlighting the currently selected option.
        /// </summary>
        /// <param name="selectedOption">Index of the current selected option.</param>
        private void PrintReplayMenu(int selectedOption = 0)
        {
            string[,] playAgainStates = new string[,]
            {
                {
                    "                  ",
                    "   Same Players   ",
                    "                  "
                },
                {
                    "╭╴              ╶╮",
                    "│  Same Players  │",
                    "╰╴              ╶╯"
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

            PrintGameWon();
            
            // Output three rows to draw the buttons. Colours the currently selected button.
            for (int i = 0; i < 3; i++)
            {
                Console.Write(LeftMargin + "║   ");
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
                Console.WriteLine("   ║");
            }

            Console.WriteLine(LeftMargin + @"╚═══════════════════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(LeftMargin + @"Use arrow keys to select option and press enter to confirm.".PadCenter(66));
            Console.ResetColor();
        }

        /// <summary>
        /// Prints the game over message.
        /// </summary>
        private void PrintGameWon()
        {
            Player winner = _players.OrderByDescending(p => p.Score).First();

            string[] gameOverMessage = new[]
            {
                @"  _____                         ____                 ",
                @" / ____|                       / __ \                ",
                @"| |  __  __ _ _ __ ___   ___  | |  | |_   _____ _ __ ",
                @"| | |_ |/ _` | '_ ` _ \ / _ \ | |  | \ \ / / _ \ '__|",
                @"| |__| | (_| | | | | | |  __/ | |__| |\ V /  __/ |   ",
                @" \_____|\__,_|_| |_| |_|\___|  \____/  \_/ \___|_|   "
            };
            
            ResetCursorPosition();
            Console.WriteLine("");
            Console.WriteLine(LeftMargin + @"╔═══════════════════════════════════════════════════════════════╗");
            foreach (string line in gameOverMessage)
            {
                Console.Write(LeftMargin + "║ ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(line.PadCenter(61));
                Console.ResetColor();
                Console.WriteLine(" ║");
            }
            Console.WriteLine(LeftMargin + $"║ {" ".PadCenter(61)} ║");
            Console.WriteLine(LeftMargin + $"║ {(winner.Name + " won the game with a score of "+ winner.Score).PadCenter(61)} ║");
            Console.WriteLine(LeftMargin + $"║ {" ".PadCenter(61)} ║");
            Console.WriteLine(LeftMargin + @"╠═══════════════════════════════════════════════════════════════╣");
        }
        
        /// <summary>
        /// Converts the given score into a larger version for printing to the game board.
        /// </summary>
        /// <param name="score">The score to be converted.</param>
        /// <param name="numberOfDigits">The number of digits to display (pads the excess with 0).</param>
        /// <returns>A string array containing the lines to be printed.</returns>
        private string[] DrawScore(int score, int numberOfDigits = 0)
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