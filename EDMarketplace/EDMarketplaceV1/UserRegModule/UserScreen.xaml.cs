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
    public partial class UserScreen : Window, ISendDSAddUpdate
    {
        USModel usModel;
        public UserScreen()
        {
            usModel = new USModel();
            InitializeComponent();
            this.DataContext = usModel;
        }

        public UserScreen(string uName)
        {
            usModel = new USModel();
            usModel.LUName = uName;
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
            //Create Data Set.
            VDSetModel vdsetModel = new VDSetModel();
            if (string.IsNullOrEmpty(this.usModel.LUName))
            {
                System.Windows.Forms.MessageBox.Show("Please enter username!! Wierd");
                return;
            }
            else
                vdsetModel.LUName = this.usModel.LUName;
            if (string.IsNullOrEmpty(this.usModel.FPath))
            {
                System.Windows.Forms.MessageBox.Show("Please Select a File that suits the Layout Created");
                return;
            }
            else
                vdsetModel.FPath = this.usModel.FPath;
            
            vdsetModel.DslModels = this.usModel.DslModels;

            //All good and now can Call the WCF service with Path..

            try
            {
                string uri = "net.tcp://localhost:6565/AdNewDSToUI";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                DuplexChannelFactory<IAddDSToUI> channel = new DuplexChannelFactory<IAddDSToUI>(new InstanceContext(this), binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(new InstanceContext(this), endPoint);
                proxy.AddDsToUI(vdsetModel);
            }
            catch (Exception ex)
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Content += Environment.NewLine + string.Format("Exception happend when adding new user to KG.\n Details {0}", ex.Message); }));
            }
        }

        //Create layout
        private void BthCreate_Click(object sender, RoutedEventArgs e)
        {
            DSLayoutScreen dslScreen = new DSLayoutScreen(this.usModel.LUName);
            dslScreen.Closing += DslScreen_Closing;
            dslScreen.ShowDialog();
        }

        private void DslScreen_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DSLayoutScreen dss = sender as DSLayoutScreen;
            this.usModel.DslModels.Clear();
            if (dss != null)
            {
                this.usModel.DslModels.Add(dss.DslModel);
            }
        }

        private void BthProcess_Click(object sender, RoutedEventArgs e)
        {
            //Create DSLayoutModel from File
            CSVFileProcessResult csvPFResult = new CSVFileProcessResult();
            csvPFResult.FileType = AllowedFileTypes.csv;
            try
            {
                int counter = 1;
                string[] lines = System.IO.File.ReadAllLines(usModel.FPath);

                foreach (string line in lines)
                {
                    csvPFResult.FileContent.Add(line);
                    if (counter == 1)
                    {
                        List<string> temoCols = new List<string>();
                        foreach (string s in line.Split(',').ToList<string>())
                        {
                            if (!string.IsNullOrEmpty(s))
                                temoCols.Add(s);
                        }
                        csvPFResult.ColNames = temoCols;
                        csvPFResult.NoOfCols = csvPFResult.ColNames.Count;
                    }
                    counter++;
                }
                csvPFResult.NoOfRows = counter;
            }
            catch (Exception ex)
            {
                csvPFResult.ProcesState = ex.Message;
                csvPFResult.ProcessResult = false;
            }
            csvPFResult.ProcesState = "process successful";
            csvPFResult.ProcessResult = true;
            lblDesc.Content += Environment.NewLine + string.Format("File of type {0} is process sucessfully.",csvPFResult.FileType);
            //Add
            this.usModel.DslModels.Clear();
            List <DSLayoutModel> dsml = new List<DSLayoutModel>();
            foreach(string s in csvPFResult.ColNames)
            {
                DSLayoutModel dm = new DSLayoutModel(this.usModel.LUName);
                dm.CFName = s;
                dsml.Add(dm);
            }
            DSDetailsScreen dsds = new DSDetailsScreen(dsml);
            dsds.Closing += Dsds_Closing;
            dsds.ShowDialog();
        }

        private void Dsds_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DSDetailsScreen nn = sender as DSDetailsScreen;
            foreach(DSLayoutModel dsm in nn.LDSMS)
            {
                this.usModel.DslModels.Add(dsm);
            }
        }

        private void BtnSelectF_Click(object sender, RoutedEventArgs e)
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

        public void SendDSAddResult(bool res)
        {
            if (res)
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Content += Environment.NewLine + string.Format("Added instance to user in KG."); }));
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() => { lblDesc.Content += Environment.NewLine + string.Format("Failed to add instance to user in KG."); }));
            }
        }
    }

    public interface IFileProcessResult
    {
        bool ProcessResult { get; set; }

        string ProcesState { get; set; }

        AllowedFileTypes FileType { get; set; }
    }

    public class CSVFileProcessResult : IFileProcessResult
    {
        public List<string> FileContent;

        public List<string> ColNames;
        public int NoOfCols { get; set; }

        public int NoOfRows { get; set; }

        bool processRes;
        public bool ProcessResult
        {
            get
            {
                return processRes;
            }
            set
            {
                processRes = value;
            }
        }

        string pState;

        public string ProcesState
        {
            get
            {
                return this.pState;
            }
            set
            {
                pState = value;
            }
        }

        AllowedFileTypes fType;

        public AllowedFileTypes FileType
        {
            get
            {
                return this.fType;
            }
            set
            {
                fType = value;
            }
        }

        public CSVFileProcessResult()
        {
            FileType = AllowedFileTypes.invalid;
            FileContent = new List<string>();
            ColNames = new List<string>();
            NoOfCols = 0;
            NoOfRows = 0;
            processRes = false;
        }
    }
    public enum AllowedFileTypes
    {
        txt,
        csv,
        xlsx,
        xml,
        json,
        invalid
    }
}
