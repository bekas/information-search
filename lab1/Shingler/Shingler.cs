using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shingler
{
    class Shingler
    {
        public double ShinRes { get; set; }
        public List<int> Hashes1 { get; set; }
        public List<int> Hashes2 { get; set; }

        virtual public double Compare(string text1, string text2, int hashCount = 84, int shingleSize = 5)
        {
            Hashes1 = CalcMinHashesFromText(text1, hashCount, shingleSize);
            Hashes2 = CalcMinHashesFromText(text2, hashCount, shingleSize);
            return CalcSim(hashCount);
        }

        protected double CalcSim(int hashCount) 
        {
            double sim = CompareHashes(Hashes1, Hashes2);
            sim /= (double)hashCount;
            ShinRes = sim;
            return sim;
        }

        protected int CompareHashes(List<int> hashes1, List<int> hashes2)
        {
            int sim = 0;
            for (int i = 0; i < hashes1.Count; i++)
            {
                if (hashes1[i] == hashes2[i])
                {
                    sim += 1;
                }
            }
            return sim;
        
        }

        protected List<int> CalcMinHashesFromText(string text,int hashCount = 36, int shingleSize = 10)
        {
            return GetMinHashes(CalcHashes(GetShingles(Canonisation(text),shingleSize),hashCount));
        }

        protected List<string> Canonisation(string text)
        {
            var resText = new List<string>();
            foreach(var word in text.ToLower().Split(stopSymbols))
            {
                if(!stopWords.Contains(word))
                {
                    resText.Add(word);
                }
            }
            return resText;
        }

        protected List<string> GetShingles(List<string> words, int count = 10)
        {
            List<string> shingles = new List<string>();

            for (int i = 0; i < words.Count - count + 1; i++) 
            {
                string shingle = "";
                foreach(var w in words.Skip(i).Take(count))
                {
                    shingle += w;
                }
                shingles.Add(shingle);
            }
            return shingles;
        }

        protected List<List<int>> CalcHashes(List<string> shingles, int countHashes = 36)
        {
            List<List<int>> hashes = new List<List<int>>();
            for (int i = 0; i < countHashes; i++) 
            { 
                hashes.Add(new List<int>());
                foreach(var shingle in shingles)
                {
                    hashes[i].Add(HashFunc(shingle, i));
                }
            }
            return hashes;
        }

        protected List<int> GetMinHashes(List<List<int>> hashes)
        {
            List<int> minHashes = new List<int>();
            int hashNum = 0;
            foreach (var hash in hashes) 
            {
                var min = hash.First();
                int k = 0;
                int shinMin = 0;

                foreach (var sh in hash)
                {
                    if (min > sh)
                    {
                        min = sh;
                        shinMin = k;
                    }
                    k++;
                }
                Console.WriteLine("Хеш#" + hashNum + " - шингл#" + shinMin);
                hashNum++;
                minHashes.Add(min);
            }
            return minHashes;
        }


        protected int HashFunc(string shingle, int num) 
        {
            num += 84;
            return Hash(shingle, 2, simpleNumbers[num]);
        }

        protected int Hash(string shingle, int p, int mod)
        {
            int hash = (int)shingle[0];
            int m = 1;
            for (int i = 1; i < shingle.Length; i++, m*=p)
            {
                hash = (hash * p) % mod + (int)shingle[i];
            }
            return hash % mod;
        }

        protected int[] simpleNumbers = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53,
                                59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113,
                                127, 131, 137, 139, 149, 151,157,163,167,173,179,181,191,193,
                                197,199,211,223,227,229,233,239,241,251,257,263,269,271,277,281,
                                283,293,307,311,313,317,331,337,347,349,353,359,367,373,379,383,389,397,
                                401,409,419,421,431,433,439 ,
                                2861,2879,2887,2897,2903,2909,2917,2927,2939,2953,2957,2963,2969,2971,2999,3001,3011,3019,3023,3037,3041,3049,3061,3067,3079,
                                3083,3089,3109,3119,3121,3137,3163,3167,3169,3181,3187,3191,3203,3209,3217,3221,3229,3251,3253,3257,
                                3259,3271,3299,3301,3307,3313,3319,3323,3329,3331,3343,3347,3359,3361,3371,3373,3389,3391,3407,3413,
                                3433,3449,3457,3461,3463,3467,3469,3491,3499,3511,3517,3527,3529,3533,3539,3541,3547,3557,3559,3571,
};
        protected string[] stopWords = { "они", "себе", "сам", "ее", "его", "им", "был", "были", "быть", "есть", "", "его", "себя", "я", "ты", "все", "мы", "это", "как", "так", "и", "в", "над", "к", "до", "не", "на", "но", "за", "то", "с", "ли", "а", "во", "от", "со", "для", "о", "же", "ну", "вы", "бы", "что", "кто", "он", "она" };
        protected char[] stopSymbols = { '.', ',', '!', '?', ':', ';', '-', '\n', '\r', '(', ')', '{', '}', '[', ']', '"', ' ' };
    }
}
