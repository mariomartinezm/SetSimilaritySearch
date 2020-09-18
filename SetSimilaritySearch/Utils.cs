using System;
using System.Collections.Generic;
using System.Linq;

namespace SetSimilaritySearch
{
    public static class Utils
    {
        public static (List<List<int>> sets, Dictionary<char, int> order) FrequencyOrderTransform(IEnumerable<string> sets)
        {
            var freq = from s in sets
                       from token in s
                       group token by token into y
                       select y;

            var counts = from pair in freq
                         orderby pair.Count()
                         select new { Key = pair.Key, Count = pair.Count() };

            var order = counts.Select((p, i) =>
                        new { p.Key, i }).ToDictionary(t => t.Key, t => t.i);

            var resultSets = new List<List<int>>();

            foreach(var s in sets)
            {
                resultSets.Add((from token in s
                                orderby order[token]
                                select order[token]).ToList());
            }
            
            return (resultSets, order);
        }

        public static int JaccardOverlapThreshold(int x, float t) => (int)(x * t);

        public static int CosineOverlapThreshold(int x, float t) => (int)(Math.Sqrt(x) * t);
    }
}