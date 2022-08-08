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
    /// Interaction logic for DSDetailsScreen.xaml
    /// </summary>
    public partial class DSDetailsScreen : Window
    {
        List<DSLayoutModel> ldsm = new List<DSLayoutModel>();

        public List<DSLayoutModel> LDSMS
        {
            get
            {
                return this.ldsm;
            }
        }
        
        public DSDetailsScreen(List<DSLayoutModel> dl)
        {
            this.ldsm = dl;
            InitializeComponent();
            lstDS.ItemsSource = this.ldsm;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {

            foreach (DSLayoutModel dslm in this.ldsm)
            {
                if (string.IsNullOrEmpty(dslm.CFName))
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Please give name for {0}", dslm.CFName));
                    return;
                }
                if (string.IsNullOrEmpty(dslm.SCFType))
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Please select a data type for {0}", dslm.CFName));
                    return;
                }
            }
            this.Close();
        }
    }
}
