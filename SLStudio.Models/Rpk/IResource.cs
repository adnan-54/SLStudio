using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models
{
    public interface IResource
    {
        int SuperId{ get; set; }
        int TypeId{ get; set; }
        int TypeOf{ get; set; }
        string Alias { get; set; }
    }
}
