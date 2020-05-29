using SLStudio.Core.Utilities.Logger;
using System;
using System.Data;

namespace SLStudio.Core.Modules.Logger.ViewModels
{
    class LogDetailsViewModel : ViewModel
    {
        public LogDetailsViewModel(DataRowView row)
        {
            Log = new Log(row.Row.ItemArray[0].ToString(), 
                               row.Row.ItemArray[1].ToString(), 
                               row.Row.ItemArray[2].ToString(), 
                               row.Row.ItemArray[3].ToString(), 
                               row.Row.ItemArray[4].ToString(), 
                               DateTime.Parse(row.Row.ItemArray[5].ToString()));

            DisplayName = $"Log {Log.Id}";
        }

        public Log Log { get; }
    }
}
