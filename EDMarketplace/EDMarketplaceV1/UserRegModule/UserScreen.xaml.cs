using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserRegModule.Models;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for UserScreen.xaml
    /// </summary>
    public partial class UserScreen : Window
    {
        USModel usModel;
        public UserScreen()
        {
            usModel = new USModel();
            InitializeComponent();
            this.DataContext = usModel;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BthSelect_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            // Launch OpenFileDialog by calling ShowDialog method
            DialogResult result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                usModel.FPath = openFileDlg.FileName;
                lblDesc.Content += Environment.NewLine + string.Format("File at {0} is selected", usModel.FPath);
            }
        }

        private void BtnValidate_Click(object sender, RoutedEventArgs e)
        {
            //Call the ECF service with Path..
        }

        private void BthCreate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
