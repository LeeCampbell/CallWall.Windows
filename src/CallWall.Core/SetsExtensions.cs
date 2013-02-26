using System.Collections.Generic;

namespace CallWall
{
    public static class SetsExtensions
    {
        public static void AddRange<T>(this ISet<T> target, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                target.Add(item);
            }
        }
    }
}