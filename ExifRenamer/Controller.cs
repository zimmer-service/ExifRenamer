using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExifRenamer
{
    public class Controller
    {
        private MainWindow mainWindow;

        public Controller(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        internal void SearchDirectory()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckPathExists = true;

            bool? res = ofd.ShowDialog();
            if (!res.HasValue || !res.Value)
                return;

            if (ofd.FileName.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                mainWindow.txtDir.Text = ofd.FileName;
                return;
            }

            mainWindow.txtDir.Text = Path.GetDirectoryName(ofd.FileName);
        }

        internal void RenameFiles()
        {
            if (!CheckUserInput())
                return;

            string[] files = Directory.GetFiles(mainWindow.txtDir.Text, "*.jpg");
            Bitmap bm;
            Dictionary<string, DateTime> fileDT = new Dictionary<string, DateTime>();
            string sdt;
            foreach (string file in files)
            {
                using (bm = new Bitmap(file))
                {
                    foreach (PropertyItem item in bm.PropertyItems)
                    {
                        if (item.Id == 0x9003)
                        {
                            sdt = Encoding.ASCII.GetString(item.Value);
                            fileDT.Add(file, ConvertFromExif(sdt));
                        }
                    }
                }
            }
        }

        private DateTime ConvertFromExif(string sdt)
        {

            return DateTime.Now;
        }

        private bool CheckUserInput()
        {
            if (string.IsNullOrEmpty(mainWindow.txtDir.Text))
            {
                MessageBox.Show("Es wurde kein Pfad angegeben!", "Es wurde kein Pfad angegeben!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!Directory.Exists(mainWindow.txtDir.Text))
            {
                MessageBox.Show("Der Pfad konnte nicht gefunden werden!", "Der Pfad konnte nicht gefunden werden!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(mainWindow.txtFormat.Text))
            {
                MessageBox.Show("Es wurde kein Format für den Dateinamen angegeben!", "Es wurde kein Format für den Dateinamen angegeben!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(mainWindow.txtIndex.Text))
            {
                MessageBox.Show("Die Indexlänge wurde nicht angegeben!", "Die Indexlänge wurde nicht angegeben!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
