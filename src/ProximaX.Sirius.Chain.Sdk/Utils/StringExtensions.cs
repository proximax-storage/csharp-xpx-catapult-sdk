namespace ProximaX.Sirius.Chain.Sdk.Utils
{
    public static class StringExtensions
    {
        /// <summary>
        /// Substring using java style
        /// </summary>
        /// <param name="s">The input string</param>
        /// <param name="beginIndex">The begin index of the string</param>
        /// <param name="endIndex">The end index of the string</param>
        /// <returns></returns>
        public static string MySubstring(this string s, int beginIndex, int endIndex)
        {
            int len = endIndex - beginIndex;
            return s.Substring(beginIndex, len);
        }
    }
}
