using ContractDataModels;
using DataSerailizer;
using Server.DSystem;
using Server.Models;
using Server.UploadIndividuals;
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

            IUploadIndividuals uService = new UploadIndividualsToRDFG();
            ServiceHost host1 = new ServiceHost(uService, new Uri("net.tcp://localhost:6565/UploadService"));
            var binding1 = new NetTcpBinding(SecurityMode.None);
            binding1.MaxReceivedMessageSize = 2147483647;
            host1.AddServiceEndpoint(typeof(IUploadIndividuals), binding1, "");
            host1.Opened += Host1_Opened;
            host1.Open();

            IObtainAllIndividuals oIndies = new ObtainAllIndies();
            ServiceHost host2 = new ServiceHost(oIndies, new Uri("net.tcp://localhost:6565/ObtainAllIndividuals"));
            var binding2 = new NetTcpBinding(SecurityMode.None);
            binding2.MaxReceivedMessageSize = 2147483647;
            host2.AddServiceEndpoint(typeof(IObtainAllIndividuals), binding2, "");
            host2.Opened += Host2_Opened;
            host2.Open();

            IObtainSSDetails oSSdet = new ObtainSSDetails();
            ServiceHost host3 = new ServiceHost(oSSdet, new Uri("net.tcp://localhost:6565/ObtainSSdetails"));
            var binding3 = new NetTcpBinding(SecurityMode.None);
            binding3.MaxReceivedMessageSize = 2147483647;
            host3.AddServiceEndpoint(typeof(IObtainSSDetails), binding3, "");
            host3.Opened += Host3_Opened;
            host3.Open();

            IObtainSSForInst ssInst = new ObtainSSForInstn();
            ServiceHost host4 = new ServiceHost(ssInst, new Uri("net.tcp://localhost:6565/ObtainSSForInstn"));
            var binding4 = new NetTcpBinding(SecurityMode.None);
            binding4.MaxReceivedMessageSize = 2147483647;
            host4.AddServiceEndpoint(typeof(IObtainSSForInst), binding4, "");
            host4.Opened += Host4_Opened;
            host4.Open();
        }

        private void Host4_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Getting data for instance service opened");
        }

        private void Host3_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("SSDetails request openened");
        }

        private void Host2_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Obtain all Individuals service started");
        }

        private void Host1_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Upload Individual Service started");
        }

        private void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Validation Service started");
        }
    }
}
