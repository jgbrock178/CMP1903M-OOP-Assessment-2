using System;

namespace CMP1903M_Assessment_2.Classes
{
    /// <summary>
    /// Abstract class for the Player. Contains default properties and methods for all players.
    /// </summary>
    public abstract class Player
    {
        /// <summary>
        /// The players name.
        /// </summary>
        public string Name { get; protected set; }
        
        /// <summary>
        /// The players score.
        /// </summary>
        public int Score { get; private set; }
        
        /// <summary>
        /// The players number.
        /// </summary>
        public int PlayerNumber { get; protected set; }
        
        /// <summary>
        /// The GameUI object for the game.
        /// </summary>
        protected GameUi Screen { get; set; }

        /// <summary>
        /// Virtual method to roll the dice.
        /// </summary>
        /// <param name="dice">A D6 array of the dice in play.</param>
        /// <param name="autoRoll">Whether to auto-roll the dice, or prompt the user to press a key to roll them</param>
        public virtual void RollDice(ref D6[] dice, bool autoRoll = false)
        {
            var random = new Random();
            int sleep = 8;
            
            while(sleep < 260)
            {
                foreach (D6 die in dice)
                {
                    die.Roll(random.Next(1, 7));
                }
                sleep = Convert.ToInt32(sleep * 1.2);
            }
        }

        /// <summary>
        /// Default constructor for the Player.
        /// </summary>
        protected Player() { }

        /// <summary>
        /// Constructor for the player when Game UI is passed.
        /// </summary>
        /// <param name="screen">The GameUI object for the game.</param>
        /// <param name="playerNumber">The number to associate with the player.</param>
        protected Player(GameUi screen, int playerNumber)
        {
            Screen = screen;
            PlayerNumber = playerNumber;
        }

        /// <summary>
        /// Updates the player score
        /// </summary>
        /// <param name="score">How much to increase the score by.</param>
        /// <returns>The players score after being updated.</returns>
        public int UpdateScore(int score)
        {
            Score += score;
            return Score;
        }

        /// <summary>
        /// Resets the players score back to 0.
        /// </summary>
        public void ResetScore()
        {
            Score = 0;
        }
    }
}