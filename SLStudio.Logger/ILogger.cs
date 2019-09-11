using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Logger
{
    public interface ILogger
    {
        void Error(Exception exception);
        void Error(string message, string title);
        void Warning(string message, string title);
        void Info(string message, string title);
        DataTable GetLog();
    }
}
