using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CMP1903M_Assessment_2
{
    public class Game
    {
        public enum Status
        {
            NotStarted,
            InProgress,
            Finished
        }

        private int WinCondition { get; set; } = 3;
        private Player PlayerOne { get; set; }
        private Player PlayerTwo { get; set; }

        private Player _currentPlayer;
        private Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                _currentPlayer.CurrentTurn = true;
                if (value == PlayerOne)
                {
                    PlayerTwo.CurrentTurn = false;
                }
                else
                {
                    PlayerOne.CurrentTurn = false;
                }
            }
        }
        
        private D6[] dice = new[]
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
            return PlayerOne.Score >= WinCondition || PlayerTwo.Score >= WinCondition;
        }
        
        private void RollDice()
        {
            for (int i = 1; i < 7; i++)
            {
                _diceValueFrequency[i] = 0;
            }
            
            foreach (D6 die in dice)
            {
                die.Roll();
                _diceValueFrequency[die.CurrentValue]++;
            }
        }

        private void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == PlayerOne ? PlayerTwo : PlayerOne;
        }

        private void Round()
        {
            Console.CursorVisible = false;
            bool reRoll = false;
            List<int> currentHighlights = new List<int>();
            
            while (true)
            {
                int sleep = 8;
                
                if (!reRoll)
                {
                    GameUi.PrintBoard(PlayerOne, PlayerTwo, dice, $"{CurrentPlayer.Name}'s turn. Press any key to roll.");
                    Console.ReadKey(true);
                }
                
                // Rolling dice animation.
                while(sleep < 260)
                {
                    RollDice();
                    GameUi.PrintBoard(PlayerOne, PlayerTwo, dice, "Rolling....", true, currentHighlights);
                    Thread.Sleep(sleep);
                    sleep = Convert.ToInt32(sleep * 1.2);
                }

                int maxFrequency = _diceValueFrequency.Max(d => d.Value);

                if (maxFrequency > 1 && currentHighlights.Count == 0)
                {
                    currentHighlights = _diceValueFrequency.Where(d => d.Value == maxFrequency)
                        .Select(d => d.Key)
                        .ToList();
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
                            CurrentPlayer.UpdateScore(3);
                            break;
                        case 4:
                            message = "You've got 4 of a kind. That's + 6 points!";
                            CurrentPlayer.UpdateScore(6);
                            break;
                        case 5:
                            message = "You've got 5 of a kind. That's + 12 points!";
                            CurrentPlayer.UpdateScore(12);
                            break;
                    }
                    GameUi.PrintBoard(PlayerOne, PlayerTwo, dice, message, false, currentHighlights);
                    Console.ReadKey(true);
                    foreach (D6 die in dice)
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
                        CurrentPlayer.UpdateScore(3);
                        break;
                    case 4:
                        message = "You've got 4 of a kind. That's + 6 points!";
                        CurrentPlayer.UpdateScore(6);
                        break;
                    case 5:
                        message = "You've got 5 of a kind. That's + 12 points!";
                        CurrentPlayer.UpdateScore(12);
                        break;
                }
                GameUi.PrintBoard(PlayerOne, PlayerTwo, dice, message, false, currentHighlights);
                Console.ReadKey(true);

                if (maxFrequency == 2)
                {
                    reRoll = true;
                    foreach (D6 die in dice)
                    {
                        if (currentHighlights.Contains(die.CurrentValue))
                        {
                            die.Locked = true;
                        }
                    }
                }
                else
                {
                    Console.CursorVisible = true;  
                    SwitchPlayer();
                    return;
                }
            }
        }
        
        public int StartGame()
        {
            bool skipMenu = false;
            bool quitConfirmed = false;
            int option = 0;
            while (true)
            {
                if (!skipMenu)
                {
                    option = GameUi.MainMenu();   
                }
                
                switch (option)
                {
                    case 0:
                        if (!skipMenu) {
                            PlayerOne = new HumanPlayer();
                            PlayerTwo = new HumanPlayer();
                            string playerName = GameUi.GetPlayerName("Player 1, enter your name");
                            PlayerOne.Name = playerName == "" ? "Player One" : playerName;
                            playerName = GameUi.GetPlayerName("Player 2, enter your name");
                            PlayerTwo.Name = playerName == "" ? "Player Two" : playerName;
                            CurrentPlayer = PlayerOne;
                        }
                        else
                        {
                            PlayerOne.ResetScore();
                            PlayerTwo.ResetScore();
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
                            option = GameUi.GameOverScreen(PlayerOne, PlayerTwo);

                            if (option == 0)
                            {
                                skipMenu = true;
                                break;
                            }
                            else if (option == 1)
                            {
                                skipMenu = false;
                                break;
                            }
                            
                            quitConfirmed = GameUi.QuitConfirmation();

                            if (quitConfirmed)
                            {
                                return 0;
                            }
                        }

                        break;
                    case 1:
                        GameUi.PrintBoard(PlayerOne, PlayerTwo, dice, "Boo");
                        break;
                    case 2:
                        quitConfirmed = GameUi.QuitConfirmation();

                        if (quitConfirmed)
                        {
                            return 0;
                        }
                        break;
                }
            }
        }
    }
}