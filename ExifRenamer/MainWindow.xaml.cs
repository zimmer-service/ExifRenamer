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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExifRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller ctrl;

        public TextBox TxtDir { get { return this.txtDir; } }
        public TextBox TxtFormat { get { return this.txtFormat; } }
        public TextBox TxtIndex { get { return this.txtIndex; } }

        public MainWindow()
        {
            InitializeComponent();
            ctrl = new Controller(this);
        }

        private void BtnSearchDir_Click(object sender, RoutedEventArgs e)
        {
            ctrl.SearchDirectory();
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            ctrl.RenameFiles();
        }
    }
}
