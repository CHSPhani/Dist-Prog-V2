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
    /// Interaction logic for ADDet.xaml
    /// </summary>
    public partial class ADDet : Window
    {
        ADevDataModel adModel;

        public ADevDataModel ADModel
        {
            get
            {
                return this.adModel;
            }
        }
        public ADDet()
        {
            adModel = new ADevDataModel();
            InitializeComponent();
            this.DataContext = adModel;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked == true)
            {
                if (!adModel.SelDS.Contains(cb.Content.ToString()))
                    adModel.SelDS.Add(cb.Content.ToString());
            }
            else if (cb.IsChecked == false)
            {
                if (adModel.SelDS.Contains(cb.Content.ToString()))
                    adModel.SelDS.Remove(cb.Content.ToString());
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb.IsChecked == false)
            {
                if (adModel.SelDS.Contains(cb.Content.ToString()))
                    adModel.SelDS.Remove(cb.Content.ToString());
            }
        }
    }
}
