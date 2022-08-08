using DataSerailizer;
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
    /// Interaction logic for DSLayoutScreen.xaml
    /// </summary>
    public partial class DSLayoutScreen : Window
    {
        DSLayoutModel dslModel;
        public DSLayoutModel DslModel
        {
            get
            {
                return this.dslModel;
            }
        }
        public DSLayoutScreen(string luN)
        {
            dslModel = new DSLayoutModel(luN);
            InitializeComponent();
            this.DataContext = dslModel;
        }

        private void BthExit_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(this.dslModel.CFName))
            {
                MessageBox.Show("Please enter name");
                return;
            }

            if (string.IsNullOrEmpty(this.dslModel.SCFType))
            {
                MessageBox.Show("Please select a type fpr property");
                return;
            }

            this.Close();
        }
    }
}
