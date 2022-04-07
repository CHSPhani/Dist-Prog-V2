using Server.Models;
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

namespace Server
{
    /// <summary>
    /// Interaction logic for KGConsole.xaml
    /// </summary>
    public partial class KGConsole : Window
    {
        KGConsoleModel kgcModel;
        public KGConsole()
        {
            kgcModel = new KGConsoleModel();
            InitializeComponent();
            this.DataContext = kgcModel;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            kgcModel.SearchRes =  kgcModel.GetSearchResults();
        }
        
    }
}
