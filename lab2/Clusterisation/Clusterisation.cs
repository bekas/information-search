using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterisation
{
    class Clusterisation
    {

        public List<Text> Texts { get; set; }
        public List<Cluster> Clusters { get; set; }

        public void Clusterisate(List<Text> texts, int clusterCount = 5) 
        {
            Texts = texts;
            foreach (var text in texts)
            {
               text.Dict = CalcDictionary(Canonisation(text.Block));
            }
            CanonisateTexts(Texts);
            Clusters = new List<Cluster>();
            for (int i = 0; i < clusterCount; i++)
            { 
                Clusters.Add(new Cluster(Clusters.Select(c=>c.Center).ToList(),texts.Select(t=>t.Dict).ToList()));
            }
            KMeans(Texts,Clusters);           
         }

        private void KMeans(List<Text> texts, List<Cluster> clusters)
        {
            bool run = true;
            int iteration = 0;
            int maxIteration = 100;

            while (iteration < maxIteration && clusters.Any(c=>c.NeedIteration))
            {
                clusters.ForEach(c => c.Texts = new List<Text>());
                foreach (var text in texts)
                {
                    Cluster ownerCluster = clusters[0];
                    double dist = text.Dict.Count;
                    foreach (var cluster in clusters)
                    {
                        var d = cluster.DistanceCos(text.Dict);
                        if (d < dist)
                        {
                            dist = d;
                            ownerCluster = cluster;
                        }
                    }
                    ownerCluster.Texts.Add(text);
                }
                clusters.ForEach(c=>c.CalcCenter());
                iteration++;
            }
        }

        private int CanonisateTexts(List<Text> texts) 
        {
            foreach (var text1 in texts) 
            {
                foreach (var key in text1.Dict.Keys)
                {
                    foreach (var text2 in texts)
                    {
                        if (!text2.Dict.ContainsKey(key)) 
                        {
                            text2.Dict.Add(key, 0);
                        }
                    }
                
                }
            }
            int res = 0;
            if(texts.Count > 0)
            {
                res = texts[0].Dict.Count;
            }
            var keys = texts[0].Dict.Keys.ToList();
            foreach (var text in texts)
            {
                foreach (var key in keys)
                {
                    text.Dict[key] /= text.Dict.Keys.Count;
                }
            }
            return res;
        }

        private Dictionary<string,double> CalcDictionary(List<string> text)
        {
            Dictionary<string, double> dict = new Dictionary<string,double>();
            foreach(var w in text)
            {
                if (dict.ContainsKey(w))
                {
                    dict[w]++;
                }
                else 
                {
                    dict.Add(w, 1);
                }
            }
            var minVal = (double)dict.Values.Max() / 2;
            return dict.OrderByDescending(x=>x.Value).Take(40).ToDictionary(x=>x.Key,x=>x.Value);  
        }

        private List<string> Canonisation(string text)
        {
            var resText = new List<string>();
            foreach (var word in text.ToLower().Split(stopSymbols))
            {
                if (!stopWords.Contains(word))
                {
                    resText.Add(word);
                }
            }
            return resText;
        }

        static public void MyStem(string file)
        {
            Process mystem = new Process();
            mystem.StartInfo.FileName = "mystem.exe";
            mystem.StartInfo.Arguments = "-e utf-8 -c -l " + file + " " + file + ".norm";
            mystem.StartInfo.UseShellExecute = false;
            mystem.StartInfo.RedirectStandardInput = true;
            mystem.StartInfo.RedirectStandardOutput = true;

            String outputText = " ";
            mystem.Start();
            StreamWriter mystemStreamWriter = mystem.StandardInput;
            StreamReader mystemStreamReader = mystem.StandardOutput;
            string bs = mystemStreamReader.ReadToEnd();
            mystemStreamWriter.Write(bs);
            mystemStreamWriter.Close();
            outputText += mystemStreamReader.ReadToEnd() + " ";
            mystem.WaitForExit();
            mystem.Close();

        }

        private string[] stopWords = {"иза","пря","один","между","например","более","либо","какой","просто","хорошо","нет","да","также","другой","мой","перед","ни","не","эта","тех","очень","разный","их","когда","хороший","если","можно","самый","большой","вот","чтобы","еще","делать","об","простой","данный","ранее","или","много","даже","будет","давать","может","такой","мочь","который","свой","уже","уж","наш","весь","этот","оно","из","тот","при","по","того","том","у", "они", "себе", "сам", "ее", "его", "им", "был", "были", "быть", "есть", "", "его", "себя", "я", "ты", "все", "мы", "это", "как", "так", "и", "в", "над", "к", "до", "не", "на", "но", "за", "то", "с", "ли", "а", "во", "от", "со", "для", "о", "же", "ну", "вы", "бы", "что", "кто", "он", "она" };
        private char[] stopSymbols = {'©','’','°','_','@','1','2','3','4','5','6','7','8','9','0','q','w','e','r','t','y','u','i','o','p','a','s','d','f','g','h','j','k','l','z','x','c','v','b','n','m','–','№','«','»','—','\'','.', ',', '!', '?', ':', ';', '-', '\n', '\r', '(', ')', '{', '}', '[', ']', '"', ' ', '|', '/', '\\', '+', '=', '*','%','$','#','^','&','`','~','<','>' };
    }
}
