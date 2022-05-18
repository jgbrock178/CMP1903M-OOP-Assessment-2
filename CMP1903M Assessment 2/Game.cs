using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CMP1903M_Assessment_2
{
    /// <summary>
    /// Class for the main game logic.
    /// </summary>
    /// TODO: Allow user to go back on menu.
    /// TODO: Add settings to allow user to change the win scenario.
    /// TODO: Swap Computer Play and PVP in menu.
    public class Game
    {
        private int _currentPlayerIndex;
        private Player[] _players;
        private Player _currentPlayer;
        
        private GameUi Screen { get; set; }
        private int RoundNumber { get; set; }
        private int WinCondition { get; set; } = 10;
        private D6[] _dice = new[]
        {
            new D6(1),
            new D6(2),
            new D6(3),
            new D6(4),
            new D6(5)
        };
        
        private Dictionary<int, int> _diceValueFrequency = new Dictionary<int, int>()
        {
            {1, 0},
            {2, 0},
            {3, 0},
            {4, 0},
            {5, 0},
            {6, 0}
        };

        private bool IsGameWon()
        {
            return _players.Count(p => p.Score >= WinCondition) > 0;
        }

        /// <summary>
        /// Switches the current player.
        /// </summary>
        private void SwitchPlayer()
        {
            _currentPlayerIndex = _currentPlayerIndex + 1 < _players.Length ? _currentPlayerIndex + 1 : 0;
            _currentPlayer = _players[_currentPlayerIndex];
            RoundNumber += 1;
            Screen.SetCurrentPlayer(ref _currentPlayer);
            Screen.RoundNumber = RoundNumber;
        }

        
        private void Round()
        {
            Console.CursorVisible = false;
            bool reRoll = false;
            List<int> currentHighlights = new List<int>();
            
            while (true)
            {
                // Reset _discValueFrequency to 0 for all values.
                for (int i = 1; i < 7; i++)
                {
                    _diceValueFrequency[i] = 0;
                }
                
                if (!reRoll)
                {
                    _currentPlayer.RollDice(ref _dice);
                }
                else
                {
                    _currentPlayer.RollDice(ref _dice, true);
                }

                foreach (D6 die in _dice)
                {
                    _diceValueFrequency[die.CurrentValue]++;
                }
                
                int maxFrequency = _diceValueFrequency.Max(d => d.Value);

                if (maxFrequency > 1)
                {
                    currentHighlights = _diceValueFrequency.Where(d => d.Value == maxFrequency)
                        .Select(d => d.Key)
                        .ToList();
                    
                    foreach (D6 die in _dice)
                    {
                        if (currentHighlights.Contains(die.CurrentValue) && (!reRoll || maxFrequency > 2))
                        {
                            die.Locked = true;
                        }
                        else
                        {
                            die.Locked = false;
                        }
                    }
                }

                string message = "";
                
                if (reRoll)
                {
                    switch (maxFrequency)
                    {
                        case 1:
                        case 2:
                            message = "Bad luck! No points this round.";
                            break;
                        case 3:
                            message = "You've got 3 of a kind. That's + 3 points!";
                            _currentPlayer.UpdateScore(3);
                            break;
                        case 4:
                            message = "You've got 4 of a kind. That's + 6 points!";
                            _currentPlayer.UpdateScore(6);
                            break;
                        case 5:
                            message = "You've got 5 of a kind. That's + 12 points!";
                            _currentPlayer.UpdateScore(12);
                            break;
                    }
                    Screen.PrintBoard(_dice, message);
                    Thread.Sleep(1000);

                    foreach (D6 die in _dice)
                    {
                        die.Locked = false;
                    }

                    SwitchPlayer();
                    return;
                }
                
                switch (maxFrequency)
                {
                    case 1:
                        message = "Oh no! No points this turn.";
                        break;
                    case 2:
                        message = "You've got 2 of a kind. Click to roll again.";
                        break;
                    case 3:
                        message = "You've got 3 of a kind. That's + 3 points!";
                        _currentPlayer.UpdateScore(3);
                        break;
                    case 4:
                        message = "You've got 4 of a kind. That's + 6 points!";
                        _currentPlayer.UpdateScore(6);
                        break;
                    case 5:
                        message = "You've got 5 of a kind. That's + 12 points!";
                        _currentPlayer.UpdateScore(12);
                        break;
                }
                Screen.PrintBoard(_dice, message);
                Thread.Sleep(1000);

                if (maxFrequency == 2)
                {
                    reRoll = true;
                    if (_currentPlayer is HumanPlayer)
                    {
                        Console.ReadKey(true);
                    }
                }
                else
                {
                    Console.CursorVisible = true;  
                    foreach (D6 die in _dice)
                    {
                        if (currentHighlights.Contains(die.CurrentValue))
                        {
                            die.Locked = false;
                        }
                    }
                    SwitchPlayer();
                    return;
                }
            }
        }
        
        
        /// <summary>
        /// Choose a random starting player and sets default variables.
        /// </summary>
        private void ChooseStartingPlayer()
        {
            _currentPlayerIndex = new Random().Next(0, _players.Length);
            _currentPlayer = _players[_currentPlayerIndex];
            Screen.SetPlayers(ref _players);
            Screen.SetCurrentPlayer(ref _currentPlayer);
            Screen.RoundNumber = RoundNumber;
        }
        
        /// <summary>
        /// Main entry way into game. 
        /// </summary>
        /// <returns></returns>
        public int StartGame()
        {
            Screen = new GameUi();
            RoundNumber = 1;
            bool skipMenu = false;
            bool quitConfirmed = false;
            int option = 0;
            while (true)
            {
                if (!skipMenu)
                {
                    option = Screen.MainMenu();
                }

                switch (option)
                {
                    case 0: // Game Mode: Player VS Computer
                        if (!skipMenu)
                        {
                            // TODO: Change numOfPlayers to be an interactive menu instead of input.
                            string numOfPlayers = Screen.GetValidatedUserInput("How many computer players (1-5)",
                                new[] {"1", "2", "3", "4", "5"});
                            
                            _players = new Player[Int32.Parse(numOfPlayers) + 1];    
                            
                            for (int i = 1; i <= Int32.Parse(numOfPlayers); i++)
                            {
                                ComputerPlayer compPlayer = new ComputerPlayer(Screen, i + 1);
                                compPlayer.GetPlayerName(_players.Where(p => p != null)
                                    .Select(p => p.Name).ToArray());
                                _players[i] = compPlayer;
                            }

                            HumanPlayer player = new HumanPlayer(Screen, 1);
                            player.GetPlayerName();
                            _players[0] = player;
                            
                            ChooseStartingPlayer();
                        }
                        else
                        {
                            foreach (Player player in _players)
                            {
                                player.ResetScore();
                            }
                        }

                        break;
                    case 1: // Game Mode: Player VS Player
                        if (!skipMenu)
                        {
                            // TODO: Change numOfPlayers to be an interactive menu instead of input.
                            string numOfPlayers = Screen.GetValidatedUserInput("How many players (2-6)",
                                new[] {"2", "3", "4", "5", "6"});
                            
                            _players = new Player[Int32.Parse(numOfPlayers)];
                            
                            for (int i = 0; i < Int32.Parse(numOfPlayers); i++)
                            {
                                HumanPlayer player = new HumanPlayer(Screen, i + 1);
                                player.GetPlayerName();
                                _players[i] = player;
                            }
                            
                            ChooseStartingPlayer();
                        }
                        else
                        {
                            foreach (Player player in _players)
                            {
                                player.ResetScore();
                            }
                        }
                        break;
                    case 2: // Quit
                        quitConfirmed = Screen.QuitConfirmation();
                        if (quitConfirmed)
                        {
                            return 0;
                        }
                        continue;
                }

                // Play the game
                while (true)
                {
                    Round();
                    if (IsGameWon())
                    {
                        break;
                    }
                }

                while (true)
                {
                    option = Screen.GameOverScreen();

                    if (option == 0)
                    {
                        skipMenu = true;
                        break;
                    }
                    
                    if (option == 1)
                    {
                        skipMenu = false;
                        break;
                    }
                            
                    quitConfirmed = Screen.QuitConfirmation();

                    if (quitConfirmed)
                    {
                        return 0;
                    }
                }
            }
        }
    }
}