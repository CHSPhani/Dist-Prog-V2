using ContractDataModels;
using SimulationEngine.SimulationHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}
