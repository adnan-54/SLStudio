using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SlrrLib.View
{
  public partial class FlagEditor : Window
  {
    public bool ClosedWithOK
    {
      get;
      private set;
    } = false;

    public FlagEditor()
    {
      InitializeComponent();
    }

    public void SetFlagInt(int flag)
    {
      ctrlCheckBox0x01.IsChecked = (flag & 0x01) != 0;
      ctrlCheckBox0x02.IsChecked = (flag & 0x02) != 0;
      ctrlCheckBox0x04.IsChecked = (flag & 0x04) != 0;
      ctrlCheckBox0x08.IsChecked = (flag & 0x08) != 0;
      ctrlCheckBox0x10.IsChecked = (flag & 0x10) != 0;
      ctrlCheckBox0x20.IsChecked = (flag & 0x20) != 0;
    }
    public int GetFlagInt()
    {
      int ret = 0;
      if (ctrlCheckBox0x01.IsChecked.Value)
        ret |= 0x01;
      if (ctrlCheckBox0x02.IsChecked.Value)
        ret |= 0x02;
      if (ctrlCheckBox0x04.IsChecked.Value)
        ret |= 0x04;
      if (ctrlCheckBox0x08.IsChecked.Value)
        ret |= 0x08;
      if (ctrlCheckBox0x10.IsChecked.Value)
        ret |= 0x10;
      if (ctrlCheckBox0x20.IsChecked.Value)
        ret |= 0x20;
      return ret;
    }

    private void ctrlButtonOK_Click(object sender, RoutedEventArgs e)
    {
      ClosedWithOK = true;
      Close();
    }

    private void ctrlButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      ClosedWithOK = false;
      Close();
    }
  }
}
