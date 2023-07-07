using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace .Application.Extensions{

    public static class ListExtensions
    {
        private static readonly Random ThreadSafeRandom = new Random();

        public static List<T> Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list.ToList();
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var array = source.ToArray();
            return ShuffleInternal(array, array.Length);
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count)
        {
            var array = source.ToArray();
            return ShuffleInternal(array, Math.Min(count, array.Length)).Take(count);
        }

        private static IEnumerable<T> ShuffleInternal<T>(T[] array, int count)
        {
            for (var n = 0; n < count; n++)
            {
                var k = ThreadSafeRandom.Next(n, array.Length);
                var temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            return array;
        }
    }
}