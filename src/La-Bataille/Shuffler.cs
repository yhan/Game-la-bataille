using System;
using System.Collections.Generic;

namespace LaBataille
{

    /// <summary>
    /// Shuffle N elements of whatever type 
    /// </summary>
    public static class Shuffler
    {
        private static readonly Random Randomizer = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Randomizer.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}