using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WikiTomito
{
    class WikiLoader
    {
        static public List<Cinematographer> LoadFile(string file)
        {
            var doc = XDocument.Load(file);
            var texts = doc
                        .Elements()
                        .First()
                        .Elements("{http://www.mediawiki.org/xml/export-0.8/}page")
                        .Select(x => x.Element("{http://www.mediawiki.org/xml/export-0.8/}revision")
                                      .Element("{http://www.mediawiki.org/xml/export-0.8/}text")
                                      .Value)
                        .Where(x =>x.Contains("{{Кинематографист"));

            return texts.Select(x=>new Cinematographer(x)).ToList();
        }

        static public List<Cinematographer> LoadDir(string dir)
        {
            List<Cinematographer> cinemas = new List<Cinematographer>();
            foreach (var file in Directory.GetFiles(dir))
            {
                var newCinemas = LoadFile(file);
                cinemas.AddRange(newCinemas);
               // break;
  
            }
            return cinemas;
        }

        static public void CreateCinemaBase(string wikiDir, string cinema)
        {
            var cinemas = LoadDir(wikiDir);
            Cinematographer.Save(cinemas, cinema);
        }

    }
}
