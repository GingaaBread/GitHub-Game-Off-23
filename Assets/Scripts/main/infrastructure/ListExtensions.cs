using System;
using System.Collections.Generic;

namespace main.infrastructure
{
    public static class ListExtensions
    {
        private static readonly Random random = new();

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            for (var i = 0; i < n - 1; i++)
            {
                var r = i + random.Next(n - i);
                (list[r], list[i]) = (list[i], list[r]);
            }
        }
    }
}