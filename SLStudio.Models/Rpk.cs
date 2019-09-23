using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models
{
    public class Rpk
    {
        public string Name { get; set; }
        public string Directory { get; set; }

        public List<Rpk> ExternalLinks { get; set; }

        public Rpk()
        {

        }
    }
}
