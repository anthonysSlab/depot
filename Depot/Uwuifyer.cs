namespace Depot
{
    using System.Text;
    using System.Text.RegularExpressions;

    public static class Uwuifyer
    {
        private static readonly Regex regex = new("(?=[aeiou])", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string UwuifyText(string? text)
        {
            if (text == null)
                return string.Empty;
            StringBuilder sb = new(text.Length);

            char last = (char)0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = char.ToLower(text[i]);
                bool isLower = char.IsLower(text[i]);

                if (c == 'l' || c == 'r')
                {
                    sb.Append(Capitalize('w', isLower));
                }
                else if (last == 't' && c == 'h')
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(Capitalize("ff", isLower));
                }
                else if (regex.IsMatch(c.ToString()) && last == 'n')
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(Capitalize("ny" + c, isLower));
                }
                else
                {
                    sb.Append(c);
                }
                last = c;
            }

            return sb.ToString();
        }

        public static char Capitalize(char a, bool isLower)
        {
            return isLower ? char.ToLower(a) : char.ToUpper(a);
        }

        public static string Capitalize(string a, bool isLower)
        {
            return isLower ? a.ToLower() : a.ToUpper();
        }
    }
}