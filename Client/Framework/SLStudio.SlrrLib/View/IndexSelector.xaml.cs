using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SlrrLib.View
{
    public partial class IndexSelector : Window
    {
        public IndexSelector()
        {
            InitializeComponent();
            if (ctrListbox.Items.Count != 0)
                ctrListbox.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private List<int> indexToKey = new List<int>();
        private List<string> values = new List<string>();

        public void SetSelectionList(Dictionary<int, string> dict)
        {
            ctrListbox.Items.Clear();
            ctrListbox.ItemsSource = null;
            indexToKey = new List<int>();
            values = new List<string>();
            foreach (var dictElement in dict)
            {
                indexToKey.Add(dictElement.Key);
                values.Add(dictElement.Value);
            }
            var items = values.ToList();
            for (int it_i = 0; it_i != items.Count; it_i++)
            {
                items[it_i] += "(0x" + indexToKey[it_i].ToString("X8") + ")";
            }
            ctrListbox.ItemsSource = items;
            SelectedInt = indexToKey[0];
            SelectedString = values[0];
        }

        public int SelectedInt = -1;
        public string SelectedString = "";

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int selectedINd = ctrListbox.SelectedIndex;
            if (selectedINd == -1 && ctrListbox.Items.Count == 0)
                return;
            if (selectedINd == -1)
                selectedINd = 0;

            SelectedInt = indexToKey[selectedINd];
            SelectedString = values[selectedINd];
        }
    }
}