using System;
using System.Linq;
using System.Threading;
using CMP1903M_Assessment_2.Exceptions;

namespace CMP1903M_Assessment_2.Classes
{
    /// <summary>
    /// Computer Player.
    /// </summary>
    public class ComputerPlayer : Player
    {
        /// <summary>
        /// Default constructor for ComputerPlayer with no attributes. Immediately raises an exception.
        /// </summary>
        /// <exception cref="InvalidPlayerInstantiationException">
        /// Raises this as the required attributes aren't included.
        /// </exception>
        public ComputerPlayer()
        {
            throw new InvalidPlayerInstantiationException();
        }
        
        /// <summary>
        /// Constructor for the ComputerPlayer class when GameUi and a player number specified.
        /// </summary>
        /// <param name="screen">The UI object of the game.</param>
        /// <param name="playerNumber">The number of the player.</param>
        public ComputerPlayer(GameUi screen, int playerNumber)
        {
            Screen = screen;
            PlayerNumber = playerNumber;
        }
        
        /// <summary>
        /// Generates a random name for the player.
        /// </summary>
        /// <param name="existingNames">If a string array given, will ensure random name isn't a duplicate.</param>
        /// <returns>A string representing the players name.</returns>
        public string GetPlayerName(string[] existingNames = null)
        {
            string[] names = new string[]
            {
                "Wall-e",
                "R2D2",
                "Hal 9000",
                "Data",
                "Optimus P",
                "RAM",
                "Chappie"
            };
            
            string[] nonTakenNames = existingNames == null ? names : names.Except(existingNames).ToArray();

            Name = nonTakenNames[new Random().Next(0, nonTakenNames.Length)];

            return Name;
        }

        /// <summary>
        /// Rolls all die and outputs animation for the rolling.
        /// </summary>
        /// <param name="dice">Array containing all dice to be rolled.</param>
        /// <param name="autoRoll">Whether to mimic an auto-roll.</param>
        public override void RollDice(ref D6[] dice, bool autoRoll = false)
        {
            var random = new Random();
            int sleep = 8;

            if (!autoRoll)
            {
                Screen.PrintBoard(dice, $"{Name}'s turn. They're thinking about it...");
                Thread.Sleep(1000);
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