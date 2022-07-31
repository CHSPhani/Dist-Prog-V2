using MySql.Data.MySqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UoB.ToolUtilities.OpenDSSParser;
using UserRegModule.DAtaOps;
using UserRegModule.Models;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string sPath;
        DSSFileParser dssFileParser;
        UserStats UStats = new UserStats();
        MySqlConnection conn = null;
        public MainWindow()
        {
            sPath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
            dssFileParser = new DSSFileParser(sPath);
            conn = ConnectToDb1.Instance.conn;
            UStats = DAtaOps.SaveToDB.GetUserStats(conn);
            InitializeComponent();
            this.DataContext = UStats;
        }

        //Exit
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow wReg = new RegistrationWindow(dssFileParser);
            wReg.ShowDialog();
        }

        private void TxtCU_LostFocus(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtCU.Text))
            {
                List<string> eRoles = SaveToDB.GetUserRoles(txtCU.Text, conn);
                if (eRoles.Count == 0)
                {
                    MessageBox.Show("Please enter a valid User ID");
                    return;
                }
                this.UStats.ERoles = eRoles;
            }
            else
            {
                MessageBox.Show("Please enter a Valid User ID");
            }
        }

        private void TxtPWd_LostFocus(object sender, RoutedEventArgs e)
        {
            string pwd = ((PasswordBox)sender).Password;
            if (string.IsNullOrEmpty(pwd))
            {
                MessageBox.Show("Please Eneter a password");
            }
        }

        //Check PAssword
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (SaveToDB.CheckPassword(this.UStats.CUName, this.UStats.PWD, this.UStats.SRole, conn))
            {
                //show Validate Screen
                UserScreen uSc = new UserScreen();
                uSc.ShowDialog();
            }
            else
            {
                //Warn user
                this.UStats.CUName = string.Empty;
                this.UStats.PWD = string.Empty;
                this.UStats.SRole = string.Empty;
                this.UStats.ERoles.Clear();
                this.UStats.ERoles = new List<string>();
                MessageBox.Show("Please enter a valid uname and password");
            }
        }

        private void TxtPWd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.UStats.PWD = ((PasswordBox)sender).Password;
        }
    }
}
