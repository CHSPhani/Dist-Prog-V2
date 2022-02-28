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
    /// Interaction logic for ShowResources.xaml
    /// </summary>
    public partial class ShowResources : Window
    {
        ShowResourcesModel srModel;
        public ShowResources()
        {
            srModel = new ShowResourcesModel();
            InitializeComponent();
            this.DataContext = srModel;
        }

        public ShowResources(string uName, DBData dbData)
        {
            srModel = new ShowResourcesModel(uName, dbData);
            InitializeComponent();
            this.DataContext = srModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSD_Click(object sender, RoutedEventArgs e)
        {
            //Show Details
        }

        private void TvEnt_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView tvi = e.OriginalSource as TreeView;
            ResourceItem rItem = tvi.SelectedItem as ResourceItem;
            string name = rItem.Name;
            srModel.FillClassDetails(name);
            srModel.FillPropertyDetails(name);
        }

        private void LvCls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PvCls_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
