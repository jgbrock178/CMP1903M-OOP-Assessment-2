namespace CMP1903M_Assessment_2
{
    public static class StringExtensions
    {
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