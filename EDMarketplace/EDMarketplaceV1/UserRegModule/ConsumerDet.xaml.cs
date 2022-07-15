using DataSerailizer;
using ContractDataModels;
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
using UserRegModule.Models;
using UoB.ToolUtilities.OpenDSSParser;

namespace UserRegModule
{
    /// <summary>
    /// Interaction logic for ConsumerDet.xaml
    /// </summary>
    public partial class ConsumerDet : Window
    {
        ConsumerDataModel cdModel;
        DSSFileParser dssFileParser;
        public ConsumerDataModel CModel
        {
            get
            {
                return this.cdModel;
            }
        }
        public ConsumerDet()
        {
            cdModel = new ConsumerDataModel();
            InitializeComponent();
            this.DataContext = cdModel;
        }
        public ConsumerDet(DSSFileParser tds):this()
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
                lblDesc.Content += Environment.NewLine + string.Format("Exception happend when calling Service for getting Load details from KG. Details {0}", ex.Message);
            }
            if(sStrs.Count==0)
            {
                lblDesc.Content += Environment.NewLine + string.Format("Can not obtain Load details");
            }
            else
            {
                lblDesc.Content += Environment.NewLine + string.Format("Obtained Load details and Loaded.");
                List<CircuitEntry> cEs =  dssFileParser.CircuitEntities;
                List<CircuitEntry> loads = cEs.FindAll(x => { if (x.CEType.ToLower().Equals("load")) { return true; } else { return false; } });
                List<string> lNames = new List<string>();
                foreach(CircuitEntry ce in loads)
                {
                    lNames.Add(ce.CEName);
                }
                List<string> sss = new List<string>();
                foreach(SemanticStructure ss in sStrs)
                {
                    if (lNames.Contains(ss.SSName))
                        sss.Add(ss.ToString());
                }
                cdModel.ALoads = sss;
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
