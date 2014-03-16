using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shingler
{
    class SuperShingler:Shingler
    {
        public double SuperShinRes { get; set; }
        public List<List<int>> Super1 { get; set; }
        public List<List<int>> Super2 { get; set; }

        override public double Compare(string text1, string text2, int hashCount = 84, int shingleSize = 10)
        {
            base.Compare(text1,text2,hashCount,shingleSize);

            Super1 = GetSuperShingles(Hashes1);
            Super2 = GetSuperShingles(Hashes2);
            
            return CalcSuperSim();
        }

        protected double CalcSuperSim()
        {
            double sim = 0;

            for (int i = 0; i < Super1.Count; i++)
            {
                int k = CompareHashes(Super1[i], Super2[i]);
                if (k == Super1[i].Count)
                {
                    sim += 1;
                }
            }

            SuperShinRes = sim;
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
