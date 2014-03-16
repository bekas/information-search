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

            MegaShingler megaShingler = new MegaShingler();
            while (run)
            {
                string text1 = LoadText("Файл №1> ");
                string text2 = LoadText("Файл №2> ");
                megaShingler.Compare(text1, text2);
                
                Console.WriteLine("Совпадение шинглов (%): " + megaShingler.ShinRes * 100);
                Console.WriteLine("Совпадение супер шинглов (кол-во): " + megaShingler.SuperShinRes);
                Console.WriteLine("Совпадение мега шинглов (кол-во): " + megaShingler.MegaShinRes);
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
