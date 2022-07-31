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
using UserRegModule.Models;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for DPDet.xaml
    /// </summary>
    public partial class DPDet : Window
    {
        NUDataProperty nudProp;

        public NUDataProperty NUDPP
        {
            get
            {
                return this.nudProp;
            }
        }

        public DPDet()
        {
            nudProp = new NUDataProperty();
            InitializeComponent();
            this.DataContext = nudProp;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
