using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shingler
{
    class Program
    {
        static void Main(string[] args)
        {
            bool run = true;
            Shingler shingler = new Shingler();
            SuperShingler superShingler = new SuperShingler();
            MegaShingler megaShingler = new MegaShingler();
            while (run)
            {
                string text1 = LoadText("Файл №1> ");
                string text2 = LoadText("Файл №2> ");
                var res = shingler.Compare(text1, text2);
                Console.WriteLine("Совпадение шинглов (%): " + res * 100);
                res = superShingler.Compare(text1, text2);
                Console.WriteLine("Совпадение супер шинглов (кол-во): " + res);
                res = megaShingler.Compare(text1, text2);
                Console.WriteLine("Совпадение мега шинглов (кол-во): " + res);
            }

        }

        private static string LoadText(string p)
        {
            Console.Write(p);
            var file = Console.ReadLine();
            return File.ReadAllText(file);
        }




    }
}
