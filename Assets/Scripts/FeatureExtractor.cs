using System.Collections.Generic;

namespace UniBudouX
{
    public class FeatureExtractor
    {
        private class UnicodeBlockCompare : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x - y;
            }
        }
        
        private static string UnicodeBlockIndex(string w)
        {
            if (w == null || w == Utils.Invalid) { return Utils.Invalid; }

            var v = w[0];
            var index = UnicodeBlocks.Values.BinarySearch(v, new UnicodeBlockCompare());
            if (index < 0)
            {
                index = ~index;
            }
            else
            {
                ++index;
            }
            return index.ToString("D3");
        }

        public static List<string> GetFeature(
            string w1,
            string w2,
            string w3,
            string w4,
            string w5,
            string w6,
            string p1,
            string p2,
            string p3)
        {
            var b1 = UnicodeBlockIndex(w1);
            var b2 = UnicodeBlockIndex(w2);
            var b3 = UnicodeBlockIndex(w3);
            var b4 = UnicodeBlockIndex(w4);
            var b5 = UnicodeBlockIndex(w5);
            var b6 = UnicodeBlockIndex(w6);

            Dictionary<string, string> rawFeature = new()
            {
                {"UP1", p1},
                {"UP2", p2},
                {"UP3", p3},
                {"BP1", p1 + p2},
                {"BP2", p2 + p3},
                {"UW1", w1},
                {"UW2", w2},
                {"UW3", w3},
                {"UW4", w4},
                {"UW5", w5},
                {"UW6", w6},
                {"BW1", w2 + w3},
                {"BW2", w3 + w4},
                {"BW3", w4 + w5},
                {"TW1", w1 + w2 + w3},
                {"TW2", w2 + w3 + w4},
                {"TW3", w3 + w4 + w5},
                {"TW4", w4 + w5 + w6},
                {"UB1", b1},
                {"UB2", b2},
                {"UB3", b3},
                {"UB4", b4},
                {"UB5", b5},
                {"UB6", b6},
                {"BB1", b2 + b3},
                {"BB2", b3 + b4},
                {"BB3", b4 + b5},
                {"TB1", b1 + b2 + b3},
                {"TB2", b2 + b3 + b4},
                {"TB3", b3 + b4 + b5},
                {"TB4", b4 + b5 + b6},
                {"UQ1", p1 + b1},
                {"UQ2", p2 + b2},
                {"UQ3", p3 + b3},
                {"BQ1", p2 + b2 + b3},
                {"BQ2", p2 + b3 + b4},
                {"BQ3", p3 + b2 + b3},
                {"BQ4", p3 + b3 + b4},
                {"TQ1", p2 + b1 + b2 + b3},
                {"TQ2", p2 + b2 + b3 + b4},
                {"TQ3", p3 + b1 + b2 + b3},
                {"TQ4", p3 + b2 + b3 + b4}
            };

            List<string> cache = new();
            foreach (var keyValuePair in rawFeature)
            {
                if (keyValuePair.Value == Utils.Invalid)
                {
                    cache.Add(keyValuePair.Key);
                }
            }
            
            foreach (var key in cache)
            {
                rawFeature.Remove(key);
            }

            List<string> ret = new();
            
            foreach (var keyValuePair in rawFeature)
            {
                ret.Add(keyValuePair.Key + ":" + keyValuePair.Value);
            }

            return ret;
        }
    }
}