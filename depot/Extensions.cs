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
    }
}