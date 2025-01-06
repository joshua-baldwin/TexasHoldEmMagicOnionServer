using System;
using System.Collections.Generic;
using System.Linq;
using THE.MagicOnion.Shared.Entities;

namespace THE.MagicOnion.Shared.Utilities
{
    public static class ExtensionMethods
    {
        public static T GetRandomElement<T>(this List<T> source)
        {
            var rand = new Random();
            var index = rand.Next(0, source.Count);
            return source[index];
        }

        public static List<CardEntity> Shuffle(this List<CardEntity> source)
        {
            var rand = new Random();
            var sourceArray = source.ToArray();
            rand.Shuffle(sourceArray);
            return sourceArray.ToList();
        }
        
        private static void Shuffle(this Random rng, CardEntity[] array)
        {
            var n = array.Length;
            while (n > 1) 
            {
                var k = rng.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
    }
}