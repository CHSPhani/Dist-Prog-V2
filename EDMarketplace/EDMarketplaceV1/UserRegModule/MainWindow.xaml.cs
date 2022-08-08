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
                if (this.UStats.SRole.ToLower().Equals("aldeveloper"))
                {
                    AlDevConfig adConfig = new AlDevConfig(this.UStats.CUName, conn);
                    adConfig.Closing += AdConfig_Closing;
                    adConfig.ShowDialog();
                }
                else
                {
                    //show Validate Screen
                    UserScreen uSc = new UserScreen(this.UStats.CUName);
                    uSc.ShowDialog();
                }
            }
            else
            {
                //Warn user
                this.UStats.CUName = string.Empty;
                this.UStats.PWD = string.Empty;
                txtPWd.Clear();
                this.UStats.SRole = string.Empty;
                this.UStats.ERoles.Clear();
                this.UStats.ERoles = new List<string>();
                MessageBox.Show("Please enter a valid uname and password");
            }
        }

        private void AdConfig_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AlDevConfig adc = sender as AlDevConfig;
            AlDevModel adm = adc.ADModel;
            //Step1 Get id for adev user
            int adID = SaveToDB.GetADId(this.UStats.CUName, conn);
            //Step-2 insert inst name with adID
            foreach(String si in adm.SelInst)
            {
                //Get next ID 
                int nextadsID = SaveToDB.GetMaxID("adSID", "aldevconfig1", conn);
                if (!SaveToDB.SaveADC1Details(nextadsID, adID, si, conn))
                    break;
            }
            foreach(string sds in adm.SelDS)
            {
                //Parse sds to get instance name.
                string instName = sds.Split('-')[0];
                int instID = SaveToDB.GetADSId(instName, conn);
                int adsds = SaveToDB.GetMaxID("adSDS", "aldevconfig2", conn);
                if (!SaveToDB.SaveADC2Details(instID, adsds, sds, conn))
                    break;
            }
            foreach(string sdp in adm.SelDP)
            {
                string instName = sdp.Split(':')[0];
                string dp = sdp.Substring(instName.Length + 1);
                int adsds = SaveToDB.GetADPId(instName, conn);
                int adsdp = SaveToDB.GetMaxID("adSDP", "aldevconfig3", conn);
                if (!SaveToDB.SaveADC3Details(adsds, adsdp, dp, conn))
                    break;
            }
        }

        private void TxtPWd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.UStats.PWD = ((PasswordBox)sender).Password;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            this.UStats.CUName = string.Empty;
            this.UStats.PWD = string.Empty;
            txtPWd.Clear();
            this.UStats.SRole = string.Empty;
            this.UStats.ERoles.Clear();
            this.UStats.ERoles = new List<string>();
        }
    }
}

#region OLD Code
/*
 * <Label Content="{Binding AdInstName}" Width="300" Margin="5,5,5,5"></Label>

<Label Content="{Binding AlDsName}" Width="300" Margin="5,5,5,5"></Label>

<StackPanel Orientation="Vertical">
                                                                                    <Label Content="{Binding CFName}"></Label>
                                                                                    <Label Content="{Binding SCFType}"></Label>
                                                                                    <Label Content="{Binding CFExp}"></Label>
                                                                                    <Label Content="{Binding CFDValue}"></Label>
                                                                                </StackPanel>
 * */
#endregion