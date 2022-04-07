using ContractDataModels;
using DataSerailizer;
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

namespace SimulationTool
{
    public partial class Form1 : Form
    {
        string sPath;
        DSSFileParser dssFileParser;
        public Form1()
        {
            sPath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
            dssFileParser = null;
            InitializeComponent();
            label1.Text = sPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void button3_Click(object sender, EventArgs e)
        {
            if(dssFileParser == null)
            {
                MessageBox.Show("Please Parse File.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SymbolicGraph sForm1 = new SymbolicGraph(dssFileParser);
                sForm1.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dssFileParser = new DSSFileParser(sPath);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Operational Branching.
            List<SemanticStructure> sStrs = new List<SemanticStructure>();
            try
            {
                string uri = "net.tcp://localhost:6565/ObtainAllIndividuals";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<IObtainAllIndividuals>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                sStrs = proxy.ObtainAllIndividuals(); //Obtain all Indies.
            }
            catch(Exception ex)
            {
                label6.Text += Environment.NewLine;
                label6.Text += "Not able to Obtain Individuals from Knowledge Graph. Exception is " + ex.Message;
            }
            if (sStrs.Count==0)
            {
                label6.Text += Environment.NewLine;
                label6.Text += "Not able to obtain circuit entities";
            }
            else
            {
                label6.Text += Environment.NewLine;
                label6.Text += "Obtained Details about Circuit Entities.";

                dssFileParser.GetOperationalState(sStrs);
            }
        }
    }
}
