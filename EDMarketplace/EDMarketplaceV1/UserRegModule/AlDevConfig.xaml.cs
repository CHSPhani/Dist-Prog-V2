using DataSerailizer;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using UserRegModule.DAtaOps;
using UserRegModule.Models;
using UserRegModule.Utilities;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for AlDevConfig.xaml
    /// </summary>
    public partial class AlDevConfig : Window
    {
        string uN = string.Empty;
        string uRole = string.Empty;
        Dictionary<string, List<string>> sResults = new Dictionary<string, List<string>>();
        List<string> roles = new List<string>();
        AlDevModel adModel;

        public AlDevModel ADModel
        {
            get
            {
                return this.adModel;
            }
        }

        public AlDevConfig(string uName, MySqlConnection conn)
        {
            adModel = new AlDevModel();
            adModel.LuName = uName;
            uN = uName;
            uRole = SaveToDB.GetADURole(uName, conn);
            roles = uRole.Split(':').ToList<string>();
            foreach(string r in roles)
            {
                AlDev ad = new AlDev();
                ad.ADRole = r;

                sResults  = ServiceUtils.GetSearchResults(r);
                List<string> instances = sResults[ParseUtils.Inst];
                foreach(string inst in instances)
                {
                    AlDevInst adi = new AlDevInst();
                    adi.AdInstName = inst;
                    //Get DataSets
                    List<string> dsForUI = ServiceUtils.GetDSForUI(adi.AdInstName);
                    foreach (string s in dsForUI)
                    {
                        AlDevDS adds = new AlDevDS();
                        adds.AlDsName = s;
                        //Get DS details 
                        List<DSLayoutModel> dsml = ServiceUtils.GetDSDet(s);
                        foreach(DSLayoutModel dsm in dsml)
                        {
                            adds.AlDsLY.Add(dsm);
                        }
                        adi.AlDsDs.Add(adds);
                    }
                    ad.AdInst.Add(adi);
                }
                adModel.AlDev.Add(ad);
            }
            InitializeComponent();
            this.DataContext = adModel;
        }


        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void ChkInstName_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ciName = sender as CheckBox;
            adModel.SelInst.Add(ciName.Content as string);
        }

        private void ChkInstName_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox ciName = sender as CheckBox;
            string nm = ciName.Content as string;
            if (adModel.SelInst.Contains(nm))
                adModel.SelInst.Remove(nm);
        }

        private void ChkDSName_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox dsName = sender as CheckBox;
            adModel.SelDS.Add(dsName.Content as string);
        }

        private void ChkDSName_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox dsName = sender as CheckBox;
            string nm = dsName.Content as string;
            if (adModel.SelDS.Contains(nm))
                adModel.SelDS.Remove(nm);
        }
        
        private void ChkDP_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox dpName = sender as CheckBox;
            adModel.SelDP.Add(((TextBlock)dpName.Content).Text as string);
        }

        private void ChkDP_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox dpName = sender as CheckBox;
            string nm = (((TextBlock)dpName.Content).Text as string);
            if (adModel.SelDP.Contains(nm))
                adModel.SelDP.Remove(nm);
        }
    }
   
}
