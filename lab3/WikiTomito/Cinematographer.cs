using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WikiTomito
{
    [Serializable]
    public class Cinematographer
    {
        private string NameBlock = "имя";
        private string BirthDayBlock = "дата рождения";
        private string BirthPlaceBlock = "место рождения";
        private string DeathDayBlock = "дата смерти";
        private string DeathPlaceBlock = "место смерти";
        private string ProfessionBlock = "профессия";
        private string NationalityBlock = "гражданство";
        private string ActiveYearsBlock = "годы активности";
        private string DirectionBlock = "направление ";
        private string StudioBlock = "киностуди";
        private string HonorsBlock = "награды";
        private string Del = "|";
        private string Par = "=";
        private string Fin = "}}";

        public Cinematographer()
        { }

        public Cinematographer(string text)
        {
            Name = GetBlock(text, NameBlock, Del);
            BirthDay = GetBlock(text, BirthDayBlock, BirthPlaceBlock );
            BirthPlace = GetBlock(text, BirthPlaceBlock, DeathDayBlock);
            DeathDay = GetBlock(text, DeathDayBlock, DeathPlaceBlock);
            DeathPlace = GetBlock(text, DeathPlaceBlock, ProfessionBlock);
            Profession = GetBlock(text, ProfessionBlock, NationalityBlock);
            Nationality = GetBlock(text, NationalityBlock, ActiveYearsBlock);
            ActiveYears = GetBlock(text, ActiveYearsBlock, DirectionBlock);
            Direction = GetBlock(text, DirectionBlock, StudioBlock);
            Studio = GetBlock(text, StudioBlock, HonorsBlock);
            Honors = GetBlock(text, HonorsBlock,Fin);

        }


        private string GetBlock(string text, string block, string nextBlock)
        {
            string s = "";
            if (text.IndexOf(block) >= 0 && (text.IndexOf(nextBlock) >= 0 || text.IndexOf(Del) >= 0))
            {
                s = text.Substring(text.IndexOf(block));
                if (s.IndexOf(Par) > 0)
                {
                    s = s.Substring(s.IndexOf(Par));
                }
                if (s.IndexOf(nextBlock) < 0)
                {
                    nextBlock = Del;
                }
                var length = s.IndexOf(nextBlock);

                s = s.Substring(0, length>0?length:0);
                s = s.Trim(' ', '|');
                s = s.Replace("|", " ");
                s = s.Replace("<br />", ";");
                s = s.Replace("{{", " "); 
                foreach (var stop in stops)
                {
                    s=s.Replace(stop, ""); 
                }
            }
            return s.Trim();
        }

        public string Name { get; set; }
        public string BirthDay { get; set; }
        public string DeathDay { get; set; }
        public string BirthPlace { get; set; }
        public string DeathPlace { get; set; }
        public string Profession { get; set; }
        public string Nationality { get; set; }
        public string ActiveYears { get; set; }
        public string Direction { get; set; }
        public string Studio { get; set; }
        public string Honors { get; set; }

        private string[] stops = { "флагификация","флаг","местоРождения", "местоСмерти","место рождения","место смерти", ",;", ", ;", ",  ;", ",   ;", ",    ;", "МР", "МС", "Место Смерти", "Место Рождения", "МестоРождения", "МестоСмерти", "Флагификация", "Флаг", "{", "}", "[", "]", "=", "style\"background: transparent\"", "&lt;", "&gt;", "br ", "/", "small", "( lang-fr", "!", ">", "<" };
        static public  void Save(List<Cinematographer> list,string file)
        {
            var objects = list.ToArray();
            XmlSerializer serializer = new XmlSerializer(typeof(Cinematographer[]));
            Stream writer = new FileStream(file, FileMode.Create);
            serializer.Serialize(writer, objects);
            writer.Close();
        
        }

        static public List<Cinematographer> Load(string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Cinematographer[]));         
            Stream reader = new FileStream(file, FileMode.Open);
            Cinematographer[] objects;
            objects = (Cinematographer[])serializer.Deserialize(reader);
            return objects.ToList();
        }
        static char[] split = { ' ', ',' };
        static public Cinematographer Search(List<Cinematographer> wiki,string fname, string lname, string oname)
        {
            List<string> searchName = new List<string>();
            if (!String.IsNullOrWhiteSpace(fname))
                searchName.Add(fname);
            if (!String.IsNullOrWhiteSpace(lname))
                searchName.Add(lname);


            var a1 = wiki.Where(c => 
            {
                bool res = !String.IsNullOrWhiteSpace(c.Name);
                foreach (var name in searchName)
                {
                    bool res2 = false;
                    
                    foreach (var n in c.Name.ToLower().Split(split))
                    {
                        res2 |= n == name.ToLower();
                    }
                    res &= res2;
                }

                return res;
            }
            ).ToList();

            if (!String.IsNullOrWhiteSpace(oname))
                searchName.Add(oname); 

            var a2 = wiki.Where(c =>
            {
                bool res = !String.IsNullOrWhiteSpace(c.Name);
                foreach (var name in searchName)
                {
                    bool res2 = false;

                    foreach (var n in c.Name.ToLower().Split(split))
                    {
                        res2 |= n == name.ToLower();
                    }
                    res &= res2;
                }

                return res;
            }
            ).ToList();
            if (a2.Count() > 0)
                return a2.First();
            else
                return a1.FirstOrDefault();

        }
        

        public override string ToString()
        {
            var str = Name;
            if (!String.IsNullOrWhiteSpace(BirthDay) && String.IsNullOrWhiteSpace(DeathDay))
            {
                str += " родился(-ась) " + BirthDay;
            }
            else if(!String.IsNullOrWhiteSpace(BirthDay) && !String.IsNullOrWhiteSpace(DeathDay))
            {
                str += ".Годы жизни: " + BirthDay + "-" + DeathDay;
            
            }

            if (!String.IsNullOrWhiteSpace(BirthPlace) && String.IsNullOrWhiteSpace(DeathPlace) )
            {
                str += ", " + BirthPlace + ".";
            }
            else if(!String.IsNullOrWhiteSpace(BirthPlace) && !String.IsNullOrWhiteSpace(DeathPlace))
            {
                str += ", " + BirthPlace + "-" + DeathPlace + ".";
            }

            if (!String.IsNullOrWhiteSpace(Profession))
            {
                str += "По профессии - " + Profession + ".";
            }

            if (!String.IsNullOrWhiteSpace(Nationality))
            {
                str += "Гражданство: " + Nationality + ".";
            }

           if (!String.IsNullOrWhiteSpace(ActiveYears))
            {
                str += "Период активности - " + ActiveYears + ".";
            }

            if (!String.IsNullOrWhiteSpace(Direction))
            {
                str += "Направление - " + Direction + ".";
            }

           if (!String.IsNullOrWhiteSpace(Studio))
            {
                str += "Работал(-а) на " + Studio + ".";
            }

            if (!String.IsNullOrWhiteSpace(Honors))
            {
                str += "Имеет следующие награды: " + Honors + ".";
            }

            return str;
        }
    }

  
}
