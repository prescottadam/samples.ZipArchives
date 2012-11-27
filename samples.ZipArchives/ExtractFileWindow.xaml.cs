using System;
using System.Collections.Generic;
using System.IO.Compression;
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

namespace samples.ZipArchives
{
    /// <summary>
    /// Interaction logic for ExtractFileWindow.xaml
    /// </summary>
    public partial class ExtractFileWindow : Window
    {
        public ZipArchiveEntry Result { get; set; }

        public ExtractFileWindow()
        {
            InitializeComponent();
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox.SelectedItem != null)
            {
                Result = listBox.SelectedItem as ZipArchiveEntry;
                Close();
            }
        }
    }
}
