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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Interaction logic for ValidateDataSet.xaml
    /// </summary>
    public partial class ValidateDataSet : Window
    {
        ValidatorModel vModel;
        public ValidateDataSet()
        {
            vModel = new ValidatorModel();
            InitializeComponent();
            this.DataContext = vModel;
        }

        public ValidateDataSet(string uName, DBData dbData)
        {
            vModel = new ValidatorModel(uName, dbData);
            InitializeComponent();
            this.DataContext = vModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            // Launch OpenFileDialog by calling ShowDialog method
            DialogResult result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result ==  System.Windows.Forms.DialogResult.OK)
            {
                vModel.SelFileName = openFileDlg.FileName;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CmbDS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vModel.FillProperties();
        }
        
    }
}
