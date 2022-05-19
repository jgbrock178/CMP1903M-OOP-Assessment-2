namespace CMP1903M_Assessment_2
{
    /// <summary>
    /// Interface for implementing Dice.
    /// </summary>
    public interface IDie
    {
        /// <summary>
        /// Rolls the die.
        /// </summary>
        /// <returns>An integer representing the face value.</returns>
        int Roll(int number);
        
        /// <summary>
        /// Prints out the current face value of the die to the console.
        /// </summary>
        void PrintDie();
        
        /// <summary>
        /// Draws the face value of the die.
        /// </summary>
        /// <returns>A string array containing each line of the die.</returns>
        string[] DrawCurrentDie();
    }
}