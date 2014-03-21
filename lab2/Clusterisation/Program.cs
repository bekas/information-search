using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clusterisation
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = true;

            Clusterisation clusterisation = new Clusterisation();
            while (run)
            {
                var texts = LoadDir("Директория с файлами (напр. 1) > ");
                Console.Write("Кол-во кластеров(напр. 2) > ");
                var count = 2;
                try
                {
                    count = Convert.ToInt32(Console.ReadLine());
                }
                catch(Exception e){}
                clusterisation.Clusterisate(texts, count);
                int i = 0;
                foreach (var c in clusterisation.Clusters)
                {
                    Console.WriteLine("Кластер " + i);
                    Console.Write("Ключ: [");
                    foreach (var w in c.Center.OrderByDescending(x=>x.Value).Take(10))
                    {
                        Console.Write(w.Key + " ");
                    }
                    Console.WriteLine("] ");
                    Console.WriteLine("Файлы: ");
                    foreach (var t in c.Texts)
                    {
                        Console.WriteLine(t.File + " ");
                    }
                    i++;
                }

              }

        }


        private static List<Text> LoadDir(string p)
        {
            Console.Write(p);
            var dir = Console.ReadLine();
            List<Text> texts = new List<Text>();
            foreach (var file in Directory.GetFiles(dir))
            {
                Clusterisation.MyStem(file);
                try
                {
                    texts.Add(new Text() { Block = File.ReadAllText(file + ".norm"), File = file });
                    File.Delete(file + ".norm");
                }
                catch 
                {
                    texts.Add(new Text() { Block = File.ReadAllText(file), File = file });
                }
                
            }
            return texts;
        }

        private static string LoadText(string p)
        {
            Console.Write(p);
            var file = Console.ReadLine();
            return File.ReadAllText(file);
        }

    }
}
