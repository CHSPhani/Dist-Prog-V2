using ContractDataModels;
using DataSerailizer;
using SimulationEngine.SimulationHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UoB.ToolUtilities.OpenDSSParser;

namespace SimulationEngine
{
    public partial class Form1 : Form
    {
        string sPath;
        DSSFileParser dssFileParser;
        string res;
        SimulationWorker sWorker;
        string validationPath;
        public Form1()
        {
            sWorker = null;
            validationPath = string.Empty;
            sPath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
            res = string.Empty;
            dssFileParser = null;
            InitializeComponent();
            label1.Text = sPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Present the folder browser dialog and get the dir path.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                sPath = folderBrowserDialog1.SelectedPath;
                label1.Text += folderBrowserDialog1.SelectedPath;
            }
            if (string.IsNullOrEmpty(sPath))
                MessageBox.Show("Please select a Folder", "Error", MessageBoxButtons.OK);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dssFileParser = new DSSFileParser(sPath);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dssFileParser == null)
            {
                MessageBox.Show("Please Read OpenDSS File Set", "Error", MessageBoxButtons.OK);
            }
            else if (dssFileParser.CircuitEntities.Count == 0)
            {
                MessageBox.Show("No Circuit Entries Found", "Error", MessageBoxButtons.OK);
            }
            else
            {
                sWorker = new SimulationWorker(dssFileParser.CircuitEntities, dssFileParser.FilePath);
                res = sWorker.CreateMonitors();
                label6.Text += Environment.NewLine;
                label6.Text += res;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (dssFileParser == null)
            {
                MessageBox.Show("Please Read OpenDSS File Set", "Error", MessageBoxButtons.OK);
            }
            else if (dssFileParser.CircuitEntities.Count == 0)
            {
                MessageBox.Show("No Circuit Entries Found", "Error", MessageBoxButtons.OK);
            }
            else
            {
                //Generating Maptlab Script
                res = sWorker.CreateMatLabScript();
                label6.Text += Environment.NewLine;
                label6.Text += res;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Create the MATLAB instance 
                MLApp.MLApp matlab = new MLApp.MLApp();

                // Change to the directory where the function is located 
                matlab.Execute(sWorker.MSPath);// @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3\MatlabAnlaysis_11-03-2022");

                // Define the output 
                object result = null;

                // Call the MATLAB function myfunc
                matlab.Feval(sWorker.MFName.Split('.')[0], 0, out result); //"TimeSimulation_1132022"

                // Display result 
                object[] res = result as object[];
            }
            catch (Exception ex)
            {
                label6.Text += Environment.NewLine;
                label6.Text += string.Format("Exception happend when executing MAtlab Script. \n Exception details: {0}", ex.Message);
            }
            label6.Text += Environment.NewLine;
            label6.Text += "MatLab execution Done";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dssFileParser == null)
            {
                MessageBox.Show("Please Read OpenDSS File Set", "Error", MessageBoxButtons.OK);
            }
            else if (dssFileParser.CircuitEntities.Count == 0)
            {
                MessageBox.Show("No Circuit Entries Found", "Error", MessageBoxButtons.OK);
            }
            else
            {
                validationPath = string.Empty;
                string res = DataUtilities.MoveAllCSVFiles(sWorker.DPath,false, ref validationPath);
                label6.Text += Environment.NewLine;
                label6.Text += res;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<string> result = null;
            try
            {
                string uri = "net.tcp://localhost:6565/MessageService";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<IValidateService>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                result = proxy.ValidateFiles(this.validationPath); //validationPath is the path where files are there. 
            }
            catch(Exception ex)
            {
                label6.Text += Environment.NewLine;
                label6.Text += "Not able to Process Validation Files. Exception is "+ex.Message;
            }

            if(result == null)
            {
                label6.Text += Environment.NewLine;
                label6.Text += "Not able tp Process Validation Files";
            }
            else
            {
                List<string> results = result as List<string>;
                if (results == null)
                {
                    label6.Text += Environment.NewLine;
                    label6.Text += "Can not Cast results to List<String>";
                }
                if (results.Count == 0)
                {
                    label6.Text += Environment.NewLine;
                    label6.Text += "Validation results are zero";
                }
                else
                {
                    //Process results and show in a separate Screen.
                    int counter = 0;
                    List<string> unsucc = new List<string>();
                    foreach (string s in results)
                    {
                        if (s.EndsWith("Successfully"))
                        {
                            counter++;
                        }
                        else
                        {
                            string[] mParts = s.Split(' ');
                            unsucc.Add(mParts[2]);
                        }
                    }
                    label6.Text += Environment.NewLine;
                    label6.Text += string.Format("{0} files are validated successfully", counter);
                    label6.Text += Environment.NewLine;
                    if (unsucc.Count > 0)
                    {
                        label6.Text += Environment.NewLine;
                        StringBuilder sb = new StringBuilder();
                        foreach (string s in unsucc)
                            sb.Append(s + ",");
                        label6.Text += string.Format("File failed to validated successfully are: ", sb.ToString());
                    }
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            List<string> result = null;
            if (this.dssFileParser == null)
            {
                label6.Text += Environment.NewLine;
                label6.Text += "List of config files are exists";
            }
            try
            {
                string uri = "net.tcp://localhost:6565/UploadService";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<IUploadIndividuals>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                result = proxy.UploadIndividuals(this.dssFileParser.CircuitEntities); //Circuit entries
            }
            catch(Exception ex)
            {
                label6.Text += Environment.NewLine;
                label6.Text += "Not able to Process Upload individuals Files. Exception is " + ex.Message;
            }

            if(result == null)
            {
                label6.Text += Environment.NewLine;
                label6.Text += "Not able tp Update Files";
            }
            else
            {
                List<string> results = result as List<string>;
                if (results == null)
                {
                    label6.Text += Environment.NewLine;
                    label6.Text += "Can not Cast results to List<String>";
                }
                if (results.Count == 0)
                {
                    label6.Text += Environment.NewLine;
                    label6.Text += "Upload Individuals results are zero";
                }
                else
                {
                    //Process results and show in a separate Screen.
                }
            }
        }

        
        //Need to add PV panels 
        private void button10_Click(object sender, EventArgs e)
        {
            //1. ReadPV from File mentioned in Utilites
            string dirPath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
            ReadPVDetails(dirPath);
            SelectPVPanel svp = new SelectPVPanel(PVS.Keys.ToList<string>());
            svp.FormClosed += Svp_FormClosed;
            svp.ShowDialog();
            //2. Create separate list and then
            //3. AddPV

        }
        private void Svp_FormClosed(object sender, FormClosedEventArgs e)
        {
            List<string> selectedItems = ((SelectPVPanel)sender).SelectedPVSystems;
            List<PVSystem> pvS = new List<PVSystem>();
            foreach (string s in selectedItems)
            {
                pvS.Add(PVS[s][0]);
            }
            //Convert pvS to CircuitEntry
            List<CircuitEntry> ces = new List<CircuitEntry>();
            foreach(PVSystem pv in pvS)
            {
                CircuitEntry ce = new CircuitEntry();
                ce.CEType = "PVPanel";
                ce.CEName = pv.Name;
                ce.CEEntries.Add(pv.Bus1);
                ce.CEEntries.Add(pv.Irradiance.ToString());
                ce.CEEntries.Add(pv.PF.ToString());
                ces.Add(ce);
            }
            bool result = false;
            //Here comes a WCF service that sends CE to Network.
            try
            {
                string uri = "net.tcp://localhost:6565/SubmitPV";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<ISubmitPVS>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                result = proxy.SubmitPV(ces);
            }
            catch (Exception ex)
            {
                label6.Text += Environment.NewLine;
                label6.Text += "Not able to Process Upload PV details. Exception is " + ex.Message;
            }
            if(result)
            {
                label6.Text += Environment.NewLine;
                label6.Text += "PV Details are uploaded";
            }
            else
            {
                label6.Text += Environment.NewLine;
                label6.Text += "Not able to Upload PV details";
            }
        }

        public Dictionary<String, List<PVSystem>> PVS; 
        /// <summary>
        /// This function reads all PV panel details that are connected to Buses
        /// new pvsystem.PVstep_05410_a1     bus1=n312290_lo.1.2.3          irradiance=1 phases=3 kv=13.20 kVA=1206.74 pf=1.00 pmpp=1097.04 duty=PV_Loadshape
        /// new pvsystem.PV05410_g2100mk7800 bus1=g2100mk7800_n300492_sec.3 irradiance=1 phases=1 kv=0.24 kVA=4.83 pf=1.00 pmpp=4.39 duty=PV_Loadshape
        /// </summary>
        public void ReadPVDetails(string dirPath)
        {
            PVS = new Dictionary<string, List<PVSystem>>();
            //if not supplied 
            if (string.IsNullOrEmpty(dirPath))
            {
                return;
            }
            string pKey = string.Empty;
            foreach (string lPath in Utilities.PVFileName)
            {
                string pvFilePath = dirPath + @"\" + lPath;
                PVSystem pvSystem = null;
                foreach (string lLine1 in File.ReadLines(pvFilePath))
                {
                    if (lLine1.StartsWith("!") || string.IsNullOrEmpty(lLine1) || lLine1.StartsWith("\t"))
                        continue;
                    string lLine = lLine1.Replace(' ', '\t');
                    string[] liDetails = lLine.Split('\t');
                    if (liDetails.Count() <= 2)
                    {
                        liDetails = lLine.Split(' ');
                    }
                    pvSystem = new PVSystem();
                    foreach (string token in liDetails)
                    {
                        if (string.Equals(token.ToLower(), "new"))
                            continue;
                        if (string.Equals((token.Split('.')[0]).ToLower(), "loadshape"))
                            break;
                        if (string.Equals((token.Split('.')[0]).ToLower(), "pvsystem") || (token.Split('.')[0]).Contains("pvsystem"))
                            pvSystem.Name = LineSegmentParsers.NameOfObject(token);
                        if (string.Equals((token.Split('=')[0]).ToLower(), "bus1"))
                            pvSystem.Bus1 = Utilities.RemoveComments(token.Split('=')[1]);
                        if (string.Equals((token.Split('=')[0]).ToLower(), "irradiance"))
                            pvSystem.Irradiance = Int32.Parse(Utilities.RemoveComments(token.Split('=')[1]));
                        if (string.Equals((token.Split('=')[0]).ToLower(), "phases"))
                            pvSystem.Phases = Int32.Parse(Utilities.RemoveComments(token.Split('=')[1]));
                        if (string.Equals((token.Split('=')[0]).ToLower(), "kv"))
                            pvSystem.kV = Double.Parse(Utilities.RemoveComments(token.Split('=')[1]));
                        if (string.Equals((token.Split('=')[0]).ToLower(), "kva"))
                            pvSystem.kVA = Double.Parse(Utilities.RemoveComments(token.Split('=')[1]));
                        if (string.Equals((token.Split('=')[0]).ToLower(), "pf"))
                            pvSystem.PF = Double.Parse(Utilities.RemoveComments(token.Split('=')[1]));
                        if (string.Equals((token.Split('=')[0]).ToLower(), "pmpp"))
                            pvSystem.Pmpp = Double.Parse(Utilities.RemoveComments(token.Split('=')[1]));
                    }
                    if (!string.IsNullOrEmpty(pvSystem.Bus1))
                    {
                        pvSystem.NType = NodeType.PVPanel;
                        pKey = pvSystem.Bus1;
                        if (PVS.ContainsKey(pKey))
                        {
                            PVS[pKey].Add(pvSystem);
                        }
                        else
                        {
                            PVS.Add(pKey, new List<PVSystem>() { pvSystem });
                        }
                    }
                }
            }
        }

       
        /// <summary>
        /// Here I am simulating the Matlab with PV details induced in them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void button11_Click(object sender, EventArgs e)
        {
            List<double> pvlDetails = new List<double>();
            string dirPath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
            string filePath = dirPath + @"\" + Utilities.PVLoadShapeFileName;
            foreach (string line in File.ReadLines(filePath))
            {
                pvlDetails.Add(Double.Parse(line));
            }
            
            List<double> tspvDetails = dssFileParser.GetPvDetailsForTimeInvterval("11:00-11:30");
            
           // DSSScriptWriter dssWriter = new DSSScriptWriter(dirPath, transformers, lines);
        }
        
        /// <summary>
        /// Validate simuated PV files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {

        }
    }


   
}
