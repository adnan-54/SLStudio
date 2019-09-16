using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Logging
{
    public interface ILog
    {
        void Info(string message = "", string title = "");
        void Info(Exception ex);
        void Debug(string message = "", string title = "");
        void Debug(Exception ex);
        void Warning(string message = "", string title = "");
        void Warning(Exception ex);
        void Error(string message = "", string title = "");
        void Error(Exception ex);
    }
}
