using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shingler
{
    class MegaShingler:SuperShingler
    {
        public double MegaShinRes { get; set; }
        public List<List<int>> Mega1 { get; set; }
        public List<List<int>> Mega2 { get; set; }

        override public double Compare(string text1, string text2, int hashCount = 84, int shingleSize = 10)
        {
            base.Compare(text1, text2, hashCount, shingleSize);
            Mega1 = GetMegaShingles(Super1);
            Mega2 = GetMegaShingles(Super2);

            return CalcMegaSim();
        }


        protected double CalcMegaSim()
        {
            double sim = 0;

            for (int i = 0; i < Mega1.Count; i++)
            {
                int k = CompareHashes(Mega1[i], Mega2[i]);
                if (k == Mega1[i].Count)
                {
                    sim += 1;
                }
            }

            MegaShinRes = sim;
            return sim;
        }

        protected List<List<int>> GetMegaShingles(List<List<int>> super)
        {
            List<List<int>> megaShingles = new List<List<int>>();
            for (int i = 0; i < super.Count; i++)
            {
                for (int j = i + 1; j < super.Count; j++)
                {
                    megaShingles.Add(new List<int>());
                    megaShingles.Last().AddRange(super[i]);
                    megaShingles.Last().AddRange(super[j]);
                }    
            }
            return megaShingles;
        }
    }
}
