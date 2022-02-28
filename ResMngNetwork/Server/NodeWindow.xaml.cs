using Server.DSystem;
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
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class NodeWindow : Window
    {
        DSNode CS = null;
        public NodeWindow()
        {
            CS = new DSNode();
            InitializeComponent();
        }

        public NodeWindow(DSNode cSystem)
        {
            this.CS = cSystem;
            InitializeComponent();
            this.DataContext = CS;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CS.SerializeDataForNode();
            this.Close();
        }
    }
}
