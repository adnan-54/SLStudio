using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SlrrLib.View
{
  public class NotifyPropertyChangedBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string name)
    {
      if (PropertyChanged != null)
        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }
}
