using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shingler
{
    class SuperShingler:Shingler
    {
        override public double Compare(string text1, string text2, int hashCount = 84, int shingleSize = 10)
        {
            var super1 = GetSuperShingles(CalcMinHashesFromText(text1, hashCount, shingleSize));
            var super2 = GetSuperShingles(CalcMinHashesFromText(text2, hashCount, shingleSize));
            double sim = 0;

            for (int i = 0; i < super1.Count; i++)
            {
                int k = CompareHashes(super1[i], super2[i]);
                if (k == super1[i].Count)
                {
                    sim += 1;
                }
            }

            return sim;
        }

        protected List<List<int>> GetSuperShingles(List<int> hashes, int superCount = 6)
        {
            List<List<int>> superShingles = new List<List<int>>();
            int count = hashes.Count / superCount;
            for (int i = 0; i < superCount; i++)
            {
                superShingles.Add(new List<int>());
                for (int j = 0; j < count; j++)
                {
                    superShingles[i].Add(hashes[i + superCount * j]);
                
                }

                //superShingles.Add(hashes.Skip(count * i).Take(count).ToList());     
            }
            return superShingles;
        }
    }
}
