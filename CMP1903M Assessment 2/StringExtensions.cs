namespace CMP1903M_Assessment_2
{
    /// <summary>
    /// Extensions to the String type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Pads the given string in equal amounts to center the string. If uneven, will lean to the left by
        /// one character.
        /// </summary>
        /// <param name="content">The string to be padded.</param>
        /// <param name="limit">The length of the returned string.</param>
        /// <returns>A string the length of limit, unless the content is already over this limit.</returns>
        public static string PadCenter(this string content, int limit)
        {
            int whitespace = limit - content.Length;
            if (whitespace <= 0)
            {
                return content;
            }
            int leftWhitespace = whitespace / 2;
            return content.PadLeft(leftWhitespace + content.Length).PadRight(limit);
        }
    }
}