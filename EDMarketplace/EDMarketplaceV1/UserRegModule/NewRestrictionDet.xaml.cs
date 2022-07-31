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
    /// Interaction logic for NewRestrictionDet.xaml
    /// </summary>
    public partial class NewRestrictionDet : Window
    {
        NURestriction nuRes;
        public NURestriction NuRes
        {
            get
            {
                return this.nuRes;
            }
        }
        public NewRestrictionDet()
        {
            nuRes = new NURestriction();
            InitializeComponent();
            this.DataContext = nuRes;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
