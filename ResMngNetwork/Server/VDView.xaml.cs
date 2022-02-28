using DataSerailizer;
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
    /// Interaction logic for VDView.xaml
    /// </summary>
    public partial class VDView : Window
    {
        VDViewModel vdvModel;
        public VDView()
        {
            InitializeComponent();
        }

        public VDView(string uName, DBData dData)
        {
            vdvModel = new VDViewModel(uName, dData);
            InitializeComponent();
            this.DataContext = vdvModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This is delete dataset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelDS_Click(object sender, RoutedEventArgs e)
        {
            vdvModel.DeletedDS();
        }
    }
}
