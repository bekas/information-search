using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiTomito
{
    class Program
    {
        static void Main(string[] args)
        {
            var wikiName = "Cinemas.xml";
            //WikiLoader.CreateCinemaBase("WikiDump",wikiName);  
            TomitaWrapper.Tomita(Cinematographer.Load(wikiName));
        }
    }
}
