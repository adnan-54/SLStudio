using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Models
{
    public class Solution : IDisposable
    {



        #region IDisposable
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~Solution()
        {
            Dispose();
        }
        #endregion
    }
}
