namespace CMP1903M_Assessment_2
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
        /// Abstract method to roll the dice.
        /// </summary>
        /// <param name="dice">A D6 array of the dice in play.</param>
        /// <param name="autoRoll">Whether to auto-roll the dice, or prompt the user to press a key to roll them</param>
        public abstract void RollDice(ref D6[] dice, bool autoRoll = false);
        
        /// <summary>
        /// Default initializer for the Player.
        /// </summary>
        protected Player(){}

        /// <summary>
        /// Initializer for the player when Game UI is passed.
        /// </summary>
        /// <param name="screen">The GameUI object for the game.</param>
        protected Player(GameUi screen)
        {
            Screen = screen;
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