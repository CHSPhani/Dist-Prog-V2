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
using UoB.ToolUtilities.OpenDSSParser;
using UserRegModule.Models;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for SmartUser.xaml
    /// </summary>
    public partial class SmartUser : Window
    {
        SmartUserDataModel suDataModel;
        DSSFileParser dssFileParser;

        public SmartUserDataModel SUModel
        {
            get
            {
                return this.suDataModel;
            }
        }

        public SmartUser()
        {
            suDataModel = new SmartUserDataModel();
            InitializeComponent();
            this.DataContext = suDataModel;
        }
        public SmartUser(DSSFileParser dsPArser):this()
        {
            this.dssFileParser = dsPArser;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
