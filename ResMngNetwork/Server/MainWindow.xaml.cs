using ContractDataModels;
using DataSerailizer;
using Server.DSystem;
using Server.Models;
using Server.ValidationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MWModel mwModel;
        
        public MainWindow()
        {
            mwModel = new MWModel();
            InitializeComponent();
            this.DataContext = mwModel;
            InvokingServices();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mwModel.SaveSMSettings();
            if (mwModel.ActiveNodes.Count != 0)
            {
                DSNode dsn = mwModel.ActiveNodes.Find((a) => { if (a.UserName.Equals("AUser")) { return true; } else { return false; } });
                if (dsn != null)
                    try
                    {
                        DataSerializer dSerializer = new DataSerializer();
                        dSerializer.SerializeNodeDB(dsn.DataInstance, dsn.DataDir, true);
                    }
                    catch (Exception ex)
                    {

                    }
            }
            this.Close();
        }

        void InvokingServices()
        {
            IValidateService vService = new ValidateFiles();
            ServiceHost host = new ServiceHost(vService, new Uri("net.tcp://localhost:6565/MessageService"));
            var binding = new NetTcpBinding(SecurityMode.None);
            host.AddServiceEndpoint(typeof(IValidateService), binding, "");
            host.Opened += Host_Opened;
            host.Open();
        }

        private void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Validation Service started");
        }
    }
}
