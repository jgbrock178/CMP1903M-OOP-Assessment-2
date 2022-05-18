using System;
using System.Threading;

namespace CMP1903M_Assessment_2
{
    /// <summary>
    /// Human player class.
    /// </summary>
    public class HumanPlayer : Player
    {
        /// <summary>
        /// Default initializer for the Human Player.
        /// </summary>
        /// <param name="screen">The GameUI object for the game.</param>
        /// <param name="playerNumber">The number of the player.</param>
        public HumanPlayer(GameUi screen, int playerNumber)
        {
            Screen = screen;
            PlayerNumber = playerNumber;
        }
        
        /// <summary>
        /// Prompts the user to enter their name. Will set to "Player X" if none entered.
        /// </summary>
        /// <returns>A string containing the players name.</returns>
        public string GetPlayerName()
        {
            string name = Screen.GetPlayerName($"Player {PlayerNumber}, enter your name");

            if (name == "")
            {
                name = $"Player {PlayerNumber}";
            }

            Name = name;
            return Name;
        }

        /// <summary>
        /// Rolls all die and outputs animation for the rolling.
        /// </summary>
        /// <param name="dice">Array containing all dice to be rolled.</param>
        /// <param name="autoRoll">Whether to prompt the user to enter a key to roll, or just roll the dice.</param>
        public override void RollDice(ref D6[] dice, bool autoRoll = false)
        {
            int sleep = 8;

            if (!autoRoll)
            {
                Screen.PrintBoard(dice, $"{Name}'s turn. Press any key to roll.");
                Console.ReadKey(true);
            }

            // Rolling dice animation.
            while(sleep < 260)
            {
                foreach (D6 die in dice)
                {
                    die.Roll();
                }
                Screen.PrintBoard(dice, "Rolling....");
                Thread.Sleep(sleep);
                sleep = Convert.ToInt32(sleep * 1.2);
            }
        }
    }
}