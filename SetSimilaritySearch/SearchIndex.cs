using System.Collections.Generic;

namespace SetSimilaritySearch
{
    class SearchIndex
    {
        private List<List<int>> mSets;
        private Dictionary<char, int> mOrder;
        private Dictionary<int, List<(int, int)>> mIndex;
        private float mSimilarityThreshold;

        public SearchIndex(IEnumerable<string> sets,
                           string similarityFuncName = "Jaccard",
                           float similarityThreshold = 0.5f)
        {
            (mSets, mOrder) = Utils.FrequencyOrderTransform(sets);
            mSimilarityThreshold = similarityThreshold;

            mIndex = new Dictionary<int, List<(int, int)>>();

            int i = 0;
            foreach(var s in mSets)
            {
                var prefix = GetPrefixIndex(s);
                int j = 0;
                foreach(var token in prefix)
                {
                    if(mIndex.ContainsKey(token))
                    {
                        mIndex[token].Add((i, j));
                    }
                    else
                    {
                        mIndex[token] = new List<(int, int)>();
                        mIndex[token].Add((i, j));
                    }
                    j++;
                }
                i++;
            }
        }

        private List<int> GetPrefixIndex(List<int> s)
        {
            int t = Utils.JaccardOverlapThreshold(s.Count, mSimilarityThreshold);
            int prefixSize = s.Count - t + 1;

            return s.GetRange(0, prefixSize);
        }
    }
}