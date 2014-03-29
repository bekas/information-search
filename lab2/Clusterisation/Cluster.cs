using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterisation
{
    class Cluster
    {
        public static Random Rand = new Random();
        public Dictionary<string, double> Center { get; set; }
        public List<Text> Texts { get; set; }

        public Cluster(Dictionary<string,double> dict)
        {
            Init(dict);
        }

        public Cluster(List<Dictionary<string,double>> exist,List<Dictionary<string,double>> all)
        {
            Dictionary<string, double> dict = all.First();
            double dist = 0;
            foreach (var d1 in all)
            {
                if (exist.Any(x => DistanceCos(x, d1) < 1.01 && DistanceCos(x, d1) > 0.99))
                {
                    continue;
                }
                double d = 0;
                foreach (var d2 in exist)
                {
                    d += DistanceCos(d1, d2);
                }
                if (d > dist)
                {
                    dict = d1;
                    dist = d;
                }
            }
            
            Init(dict);
        }

        private void Init(Dictionary<string,double> dict)
        {
            Center = new Dictionary<string, double>();
            foreach (var key in dict.Keys)
            {
                Center.Add(key, (dict[key])); 
            }
            NeedIteration = true;
        }

        public void CalcCenter()
        {
            if (Texts.Count > 0)
            {
                Dictionary<string, double> center = new Dictionary<string, double>();
                foreach (var key in Center.Keys)
                {
                    center.Add(key, 0);
                }
                foreach (var t in Texts)
                {
                    foreach (var key in t.Dict.Keys)
                    {
                        center[key] += t.Dict[key];
                    }
                }
                var keys = center.Keys.ToList();
                foreach (var key in keys)
                {
                    center[key] /= Texts.Count;
                }
                double dist = DistanceCos(center);
                NeedIteration = dist > Math.Sqrt(center.Values.Sum()) / 10;
                Center = center;
            }
        }
        public double DistanceE(Dictionary<string,double> dict) 
        {
            double dist = 0;
            foreach (var key in dict.Keys)
            {
                dist += Math.Pow((Center[key] - dict[key]), 2);
            }
            dist = Math.Sqrt(dist);
            return dist;
        }

        public double DistanceCos(Dictionary<string, double> dict, Dictionary<string, double> center = null)
        {
            if (center == null)
            {
                center = Center;
            }
            double d1=0,d2=0,d3 = 0;
            foreach (var key in dict.Keys)
            {
                d1 += center[key] * dict[key];
                d2 += center[key] * center[key];
                d3 += dict[key] * dict[key];
            }

            var dist = d1 / (Math.Sqrt(d2) * Math.Sqrt(d3));
            if (dist > 0.0001)
            {
                return 1 / dist;
            }
            else
            {
                return 100000;
            }
            //return dist;
        }

        public bool NeedIteration { get; set; }
    }
}
