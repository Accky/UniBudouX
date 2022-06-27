using System.Collections.Generic;

namespace UniBudouX
{
    public class Parser
    {
        public enum KnbcMode
        {
            Japanese,
            ZnHans,
        }

        private static KnbcMode mode = KnbcMode.Japanese;

        public static KnbcMode Mode
        {
            get => mode;
            set
            {
                mode = value;
                model = mode == KnbcMode.Japanese ? KnbcJapan.Pairs : KnbcZnHans.Pairs;
            }
        }
        
        private static Dictionary<string, int> model = KnbcJapan.Pairs;
        
        public static List<string> Parse(string sentence, int threshold = 1000)
        {
            if (sentence.Length == 0) { return null; }

            string p1 = Utils.Unknown;
            string p2 = Utils.Unknown;
            string p3 = Utils.Unknown;

            List<string> chunks = new() {sentence[0].ToString()};

            for (int i = 1; i < sentence.Length; i++)
            {
                var feature = FeatureExtractor.GetFeature(
                    i > 2 ? sentence[i-3].ToString() : Utils.Invalid,
                    i > 1 ? sentence[i-2].ToString() : Utils.Invalid,
                    sentence[i-1].ToString(),
                    sentence[i].ToString(),
                    i + 1 < sentence.Length ? sentence[i+1].ToString() : Utils.Invalid,
                    i + 2 < sentence.Length ? sentence[i+2].ToString() : Utils.Invalid,
                    p1,
                    p2,
                    p3);
                
                int score = 0;
                
                foreach (var f in feature)
                {
                    if (!model.ContainsKey(f)) { continue; }
                    score += model[f];
                }

                if (score > threshold)
                {
                    chunks.Add(sentence[i].ToString());
                }
                else
                {
                    chunks[^1] += sentence[i].ToString();
                }

                var p = score > 0 ? Utils.Positive : Utils.Negative;
                p1 = p2;
                p2 = p3;
                p3 = p;
            }

            return chunks;
        }
    }
}