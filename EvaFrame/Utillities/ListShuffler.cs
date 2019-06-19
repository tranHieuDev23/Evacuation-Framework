using System;
using System.Collections.Generic;

namespace EvaFrame.Utilities
{
    class ListShuffler
    {
        public static List<T> Shuffle<T>(IEnumerable<T> list, Random rnd)
        {
            List<T> result = new List<T>(list);
            for (int i = 0; i < result.Count; i++)
            {
                int k = rnd.Next(0, i);
                T value = result[k];
                result[k] = result[i];
                result[i] = value;
            }
            return result;
        }
    }
}