using System.Collections;

namespace THE.Utilities
{
    public static class ExtensionMethods
    {
        public static T GetRandomElement<T>(this List<T> source)
        {
            var rand = new Random();
            var index = rand.Next(0, source.Count);
            return source[index];
        }

        public static List<T> Shuffle<T>(this List<T> source)
        {
            var rand = new Random();
            var sourceArray = source.ToArray();
            rand.Shuffle(sourceArray);
            return sourceArray.ToList();
        }
    }
}