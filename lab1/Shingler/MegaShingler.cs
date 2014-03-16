using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shingler
{
    class MegaShingler:SuperShingler
    {
        override public double Compare(string text1, string text2, int hashCount = 84, int shingleSize = 10)
        {
            var mega1 = GetMegaShingles(GetSuperShingles(CalcMinHashesFromText(text1, hashCount, shingleSize)));
            var mega2 = GetMegaShingles(GetSuperShingles(CalcMinHashesFromText(text2, hashCount, shingleSize)));
            double sim = 0;

            for (int i = 0; i < mega1.Count; i++)
            {
                int k = CompareHashes(mega1[i], mega2[i]);
                if (k == mega1[i].Count)
                {
                    sim += 1;
                }
            }

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
