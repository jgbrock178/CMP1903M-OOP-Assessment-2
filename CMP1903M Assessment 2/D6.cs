using System;
using System.Linq;

namespace CMP1903M_Assessment_2
{
    /// <summary>
    /// A representation of a D6 die (standard die with 6 sides).
    /// </summary>
    public class D6 : IDie
    {
        /// <summary>
        /// Current value of the die.
        /// </summary>
        public int CurrentValue { get; private set; }
        
        /// <summary>
        /// Whether the dice is locked and so can't be rolled.
        /// </summary>
        public bool Locked { get; set; }
        
        /// <summary>
        /// Default initializer for the D6 - rolls the die to generate a random starting face.
        /// </summary>
        public D6()
        {
            Roll();
        }

        /// <summary>
        /// Initializer for the D6 when a starting value is specified.
        /// </summary>
        /// <param name="face">The default value for the die.</param>
        /// <exception cref="DieValueNotValidException">If an invalid number is passed.</exception>
        public D6(int face)
        {
            int[] allowedValues = Enumerable.Range(1, 6).ToArray();
            if (!allowedValues.Contains(face))
            {
                throw new DieValueNotValidException(face);
            }

            CurrentValue = face;
        }

        /// <summary>
        /// "Rolls the die" generating a random number between 1 and 6.
        /// </summary>
        /// <returns>An integer representing the number rolled.</returns>
        public int Roll()
        {
            if (Locked) return CurrentValue;
            var rand = new Random();
            CurrentValue = rand.Next(6) + 1;
            return CurrentValue;
        }
        
        /// <summary>
        /// Prints a graphical representation of the dice to the console.
        /// </summary>
        public void PrintDie()
        {
            string[] dieFace = DrawDie();

            foreach (string line in dieFace)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Draws die face value.
        /// </summary>
        /// <param name="number">The number to draw. If no value is set will print the current value.</param>
        /// <returns>A string array containing each line of the die face to be printed.</returns>
        public static string[] DrawDie(int number = 0)
        {
            string[,] dieFaces = new string[,]
            {
                {
                    "│         │",
                    "│    ⊙    │",
                    "│         │"
                },
                {
                    "│ ⊙       │",
                    "│         │",
                    "│       ⊙ │"
                },
                {
                    "│ ⊙       │",
                    "│    ⊙    │",
                    "│       ⊙ │"
                },
                {
                    "│ ⊙     ⊙ │",
                    "│         │",
                    "│ ⊙     ⊙ │"
                },
                {
                    "│ ⊙     ⊙ │",
                    "│    ⊙    │",
                    "│ ⊙     ⊙ │"
                },
                {
                    "│ ⊙     ⊙ │",
                    "│ ⊙     ⊙ │",
                    "│ ⊙     ⊙ │"
                }
            };

            int dieValue = number == 0 ? new Random().Next(1, 7) : number;

            string[] diceFace = new string[]
            {
                "╭─────────╮",
                dieFaces[dieValue - 1, 0],
                dieFaces[dieValue - 1, 1],
                dieFaces[dieValue - 1, 2],
                "╰─────────╯"
            };

            return diceFace;
        }
        
        /// <summary>
        /// Generates the graphical interface representing the current value of the die.
        /// </summary>
        /// <returns>A string array where each element is a separate line. Allows the die to be printed elsewhere.</returns>
        public string[] DrawCurrentDie()
        {
            return D6.DrawDie(CurrentValue);
        }
    }
}