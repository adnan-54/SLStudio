using Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presenters
{
    public static class Solution
    {
        public static Models.Solution Create(string name, string title, string description, string author, string path)
        {
            try
            {
                using (Models.Solution sln = new Models.Solution(name, title, description, author))
                {
                    sln.Create(path);
                    return sln;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
                throw ex;
            }
        }
    }
}
