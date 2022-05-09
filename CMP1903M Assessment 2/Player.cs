namespace CMP1903M_Assessment_2
{
    public abstract class Player
    {
        public string Name { get; set; }
        public int Score { get; private set; }
        
        public bool CurrentTurn { get; set; }

        public Player(){}
        
        public Player(string name)
        {
            Name = name;
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