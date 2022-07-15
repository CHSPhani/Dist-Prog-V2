using ContractDataModels;
using DataSerailizer;
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
using System.Windows.Shapes;
using UoB.ToolUtilities.OpenDSSParser;
using UserRegModule.Models;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for ProsumerDet.xaml
    /// </summary>
    public partial class ProsumerDet : Window
    {
        ProsumerDataModel pdModel;
        DSSFileParser dssFileParser;
        public ProsumerDataModel PDModel
        {
            get
            {
                return this.pdModel;
            }
        }
        public ProsumerDet()
        {
            pdModel = new ProsumerDataModel();
            InitializeComponent();
            this.DataContext = pdModel;
        }

        public ProsumerDet(DSSFileParser tds):this()
        {
            this.dssFileParser = tds;
            List<SemanticStructure> sStrs = new List<SemanticStructure>();
            try
            {
                string uri = "net.tcp://localhost:6565/ObtainAllLoadIndies";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<IObtainLoadIndividuals>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                sStrs = proxy.ObtainLoadIndividuals(); //Obtain all Indies.
            }
            catch (Exception ex)
            {
                lblDesc.Content += Environment.NewLine + string.Format("Exception happend when calling Service for getting CE details from KG. Details {0}", ex.Message);
            }
            if (sStrs.Count == 0)
            {
                lblDesc.Content += Environment.NewLine + string.Format("Can not obtain CE details");
            }
            else
            {
                lblDesc.Content += Environment.NewLine + string.Format("Obtained CE details and Loaded.");
                List<CircuitEntry> cEs = dssFileParser.CircuitEntities;
                List<CircuitEntry> loads = cEs.FindAll(x => { if (x.CEType.ToLower().Equals("transformer")) { return true; } else { return false; } });
                List<string> lNames = new List<string>();
                foreach (CircuitEntry ce in loads)
                {
                    lNames.Add(ce.CEName);
                }
                List<string> sss = new List<string>();
                foreach (SemanticStructure ss in sStrs)
                {
                    if (lNames.Contains(ss.SSName))
                        sss.Add(ss.ToString());
                }
                pdModel.Trans = sss;
            }

            //PV Panels
            List<CircuitEntry> pvs = new List<CircuitEntry>();
            try
            {
                string uri = "net.tcp://localhost:6565/SendPVDetails";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<ISendPVInfo>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                pvs = proxy.SendPVInfo();
            }
            catch (Exception ex)
            {
                lblDesc.Content += Environment.NewLine;
                lblDesc.Content += "Not able to Obtain Individuals from Knowledge Graph. Exception is " + ex.Message;
            }
            if (pvs == null || pvs.Count == 0)
            {
                lblDesc.Content += Environment.NewLine;
                lblDesc.Content += "Not able to obtain PVS";
            }
            else
            {
                lblDesc.Content += Environment.NewLine;
                lblDesc.Content += "Obtained Details about PV Details.";
                List<string> pvnames = new List<string>();
                foreach(CircuitEntry ce in pvs)
                {
                    pvnames.Add(ce.CEName);
                }
                pdModel.PVPanels = pvnames;
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
