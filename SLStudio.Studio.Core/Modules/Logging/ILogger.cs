using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Studio.Core.Modules.Logging
{
    public interface ILogger
    {
        void Debug(string title = "", string message = "");
        void Info(string title = "", string message = "");
        void Warning(string title = "", string message = "");
        void Error(string title = "", string message = "");
        void Error(Exception exception);
    }
}
