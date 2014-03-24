using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WikiTomito
{
    class TomitaWrapper
    {
        static public void Tomita(List<Cinematographer> wiki)
        {
            TomitaRun();
            ParseResult(wiki);    
        }

        static public void TomitaRun()
        {
            Process tomita = new Process();
            tomita.StartInfo.FileName = "tomita.exe";
            tomita.StartInfo.Arguments = "config.proto";
            tomita.StartInfo.UseShellExecute = false;
            tomita.StartInfo.RedirectStandardInput = true;
            tomita.StartInfo.RedirectStandardOutput = true;

            String outputText = " ";
            tomita.Start();
            StreamWriter mystemStreamWriter = tomita.StandardInput;
            StreamReader mystemStreamReader = tomita.StandardOutput;
            string bs = mystemStreamReader.ReadToEnd();
            mystemStreamWriter.Write(bs);
            mystemStreamWriter.Close();
            outputText += mystemStreamReader.ReadToEnd() + " ";
            tomita.WaitForExit();
            tomita.Close();

        }

        static public void ParseResult(List<Cinematographer> wiki)
        {
            var xml = XDocument.Load("facts.xml");
            foreach(var doc in xml.Descendants("document"))
            {
                var docName = doc.Attribute("url").Value;
                
                foreach (var fio in doc.Descendants("FIO"))
                {
                    string fname="", lname="", oname="";
                    if( fio.Element("FName") != null)
                        fname = fio.Element("FName").Attribute("val").Value;
                    if (fio.Element("LName") != null)
                        lname = fio.Element("LName").Attribute("val").Value;
                    if (fio.Element("OName") != null)
                        oname = fio.Element("OName").Attribute("val").Value;
                    AppendAnnotation("data" + docName, wiki, fname, lname, oname);
                }   
            }   
        }


        static void AppendAnnotation(string fileName, List<Cinematographer> wiki, string fname, string lname, string oname)
        {

            var c = Cinematographer.Search(wiki, fname, lname, oname);
            if (c != null)
            {
                File.AppendAllText(fileName, "\r\n\r\n" + c.ToString());
            }
        
        }

        

    }
}
