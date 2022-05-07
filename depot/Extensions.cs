namespace Depot
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public static class Extensions
    {
        public static void RemoveAll<T>(this ICollection<T> a, IEnumerable<T> b)
        {
            foreach (var item in b)
            {
                a.Remove(item);
            }
        }

        public static void RemoveAll<T>(this DbSet<T> a, IEnumerable<T> b) where T : class
        {
            foreach (var item in b)
            {
                a.Remove(item);
            }
        }

        public static void SplitString(this string str, int maxlen, Action<string> action)
        {
            int position = 0;
            while (position < str.Length)
            {
                int len = position + maxlen > str.Length ? str.Length - position : maxlen;
                action(str.Substring(position, len));
                position += len;
            }
        }
    }
}