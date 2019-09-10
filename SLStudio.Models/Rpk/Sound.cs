using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models
{
    public class Sound
    {
        public int soundSuperId;
        public int soundTypeId;

        public string soundDirectory;
        public string soundName;
        public string soundDescription;
        public string soundAlias;

        public int soundInstances;

        public float soundMinDist;
        public float soundMaxDist;

        public float soundvolume;
    }
}
