using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models
{
    public class ExternalLinks
    {
        private List<string> links = new List<string>();

        public ExternalLinks() { }

        public void AddLink(string link)
        {
            //todo: resource para link ja existente
            if (links.IndexOf(link) != -1)
                throw new Exception();

            links.Add(link);
        }

        public void RemoveLink(string link)
        {
            //todo: resource para link não existente
            if (links.IndexOf(link) == -1)
                throw new Exception();

            links.Remove(link);
        }

        public Dictionary<string, int> GetLinks()
        {
            //todo: resource para quando não tem links
            if (links.Count <= 0)
                throw new Exception();

            Dictionary<string, int> linkPairs = new Dictionary<string, int>();

            int index = 1;

            foreach(string link in links)
            {
                linkPairs.Add(link, index++);
            }

            return linkPairs;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            //todo: verificar syntax isso
            sb.AppendLine("<External_Links>");

            foreach (string link in links)
            {
                sb.AppendLine($"\t{link}");
            }

            sb.AppendLine("</External_Links>");

            return sb.ToString();
        }
    }
}
