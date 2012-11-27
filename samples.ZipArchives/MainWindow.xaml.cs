using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace samples.ZipArchives
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static string PromptForDirectory()
        {
            var dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dlg.SelectedPath;
            }
            return string.Empty;
        }

        private static string PromptForSaveFile(string file, string ext, string filter)
        {
            var dlg = new SaveFileDialog();
            dlg.FileName = file;
            dlg.DefaultExt = ext;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dlg.FileName;
            }
            return string.Empty;
        }

        private static string PromptForOpenFile(string file, string ext, string filter)
        {
            var dlg = new OpenFileDialog();
            dlg.FileName = file;
            dlg.DefaultExt = ext;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dlg.FileName;
            }
            return string.Empty;
        }

        private static ZipArchiveEntry PromptForArchiveEntry(ZipArchive zipArchive)
        {
            var dlg = new ExtractFileWindow();
            dlg.DataContext = zipArchive.Entries;
            dlg.ShowDialog();
            return dlg.Result;
        }

        private void OnExtractArchive(object sender, RoutedEventArgs e)
        {
            var archive = PromptForOpenFile(
                string.Empty, ".zip", "Zip archives (.zip)|*.zip");
            if (string.IsNullOrEmpty(archive))
                return;

            var destination = PromptForDirectory();

            ZipFile.ExtractToDirectory(archive, destination);
        }

        private void OnExtractFile(object sender, RoutedEventArgs e)
        {
            var archive = PromptForOpenFile(
                string.Empty, ".zip", "Zip archives (.zip)|*.zip");
            if (string.IsNullOrEmpty(archive))
                return;

            using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Read))
            {
                var itemToExtract = PromptForArchiveEntry(zipArchive);
                if (itemToExtract == null)
                    return;

                var target = PromptForSaveFile(
                    itemToExtract.FullName, string.Empty, "All files (.*)|*.*");

                using (var fs = new FileStream(target, FileMode.Create))
                {
                    using (var contents = itemToExtract.Open())
                    {
                        contents.CopyToAsync(fs);
                    }
                }
            }
        }

        private void OnCreateArchive(object sender, RoutedEventArgs e)
        {
            var dir = PromptForDirectory();
            var target = PromptForSaveFile(
                "Archive.zip", ".zip", "Zip archives (.zip)|*.zip");
            ZipFile.CreateFromDirectory(dir, target);
        }

        private void OnAddFileToArchive(object sender, RoutedEventArgs e)
        {
            var archive = PromptForOpenFile(
                string.Empty, ".zip", "Zip archives (.zip)|*.zip");
            if (string.IsNullOrEmpty(archive))
                return;

            var file = PromptForOpenFile(
                string.Empty, ".*", "All files (.*)|*.*");
            if (string.IsNullOrEmpty(archive))
                return;

            using (ZipArchive zipArchive = ZipFile.Open(archive, ZipArchiveMode.Update))
            {
                var name = Path.GetFileName(file);
                zipArchive.CreateEntryFromFile(file, name);
            }
        }
    }
}
