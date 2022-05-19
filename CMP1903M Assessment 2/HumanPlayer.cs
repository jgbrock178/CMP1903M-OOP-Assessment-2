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
        /// Default constructor for Human Player with no attributes. Immediately raises an exception.
        /// </summary>
        /// <exception cref="InvalidPlayerInstantiationException">
        /// Raises this as the required attributes aren't included.
        /// </exception>
        public HumanPlayer()
        {
            throw new InvalidPlayerInstantiationException();
        }
        
        /// <summary>
        /// Constructor for the HumanPlayer class when the GameUi and player number is included.
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
            var random = new Random();

            if (!autoRoll)
            {
                Screen.PrintBoard(dice, $"{Name}'s turn. Press any key to roll.");
                Console.ReadKey(true);
            }

            // Rolling dice animation.
            // Many thanks to Alex Elwell for recommending changing the dice value in place for the animation.
            while(sleep < 260)
            {
                foreach (D6 die in dice)
                {
                    die.Roll(random.Next(1, 7));
                }
                Screen.PrintBoard(dice, "Rolling....");
                Thread.Sleep(sleep);
                sleep = Convert.ToInt32(sleep * 1.2);
            }
        }
    }
}