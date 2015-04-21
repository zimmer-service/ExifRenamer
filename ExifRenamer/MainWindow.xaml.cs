using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExifRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ExifRenamerController exifRenamer;

        public MainWindow()
        {
            InitializeComponent();
            exifRenamer = new ExifRenamerController();
        }

        private void BtnSearchDir_Click(object sender, RoutedEventArgs e)
        {
            txtDir.Text = exifRenamer.SearchDirectory();
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckUserInput())
                return;

            Cursor curCursor = this.Cursor;
            this.Cursor = Cursors.Wait;
            try
            {
                exifRenamer.RenameFiles(txtDir.Text, txtFileExt.Text, txtFormat.Text, txtIndex.Text);
            }
            finally
            {
                this.Cursor = curCursor;
            }
        }

        private bool CheckUserInput()
        {
            if (string.IsNullOrEmpty(txtDir.Text))
            {
                MessageBox.Show("Es wurde kein Pfad angegeben!", "Es wurde kein Pfad angegeben!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!Directory.Exists(txtDir.Text))
            {
                MessageBox.Show("Der Pfad konnte nicht gefunden werden!", "Der Pfad konnte nicht gefunden werden!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtFormat.Text))
            {
                MessageBox.Show("Es wurde kein Format für den Dateinamen angegeben!", "Es wurde kein Format für den Dateinamen angegeben!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtIndex.Text))
            {
                MessageBox.Show("Die Indexlänge wurde nicht angegeben!", "Die Indexlänge wurde nicht angegeben!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            int foo;
            if (!int.TryParse(txtIndex.Text, out foo))
            {
                MessageBox.Show("Die Indexlänge muss als Ganzzahl angegeben werden!", "Die Indexlänge muss als Ganzzahl angegeben werden!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(txtFileExt.Text))
            {
                MessageBox.Show("Es wurde kein Dateityp angegeben!", "Es wurde kein Dateityp angegeben!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
