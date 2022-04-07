using DataSerailizer;
using Server.DSystem;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
using UoB.ToolUtilities.OpenDSSParser;

namespace Server
{
    /// <summary>
    /// Interaction logic for UploadIndividual.xaml
    /// </summary>
    public partial class UploadIndividual : Window, IProposalResult, ITransitionResult
    {
        UploadInd upInd;

        public event RaiseProposeEventHandler RaiseProposal3;

        DSSFileParser dssFileParser;
        string dssFilePath;

        public UploadIndividual()
        {
            InitializeComponent();
        }

        public UploadIndividual(string uName, DBData dbData)
        {
            upInd = new UploadInd (uName, dbData);
            upInd.RaiseProposal2 += UpInd_RaiseProposal2;
            InitializeComponent();
            this.DataContext = upInd;
            dssFilePath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
        }
        private void UpInd_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dssFilePath = diag.SelectedPath;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dssFileParser = new DSSFileParser(dssFilePath);
            if (dssFileParser != null)
                this.upInd.CEntities = dssFileParser.CircuitEntities;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            upInd.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                upInd.ProposalState = true;
            else
                upInd.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
            {
                upInd.ProposalStatus = "Transition Done";
                WriteNeojScript();
            }
            else
                upInd.ProposalStatus = "Transition Failed";
        }

        void WriteNeojScript()
        {
            //Creating Files for Neo4J
            List<string> cins = new List<string>();
            foreach (KeyValuePair<string, SemanticStructure> kvp in this.upInd.CurrentDbInstance.OwlData.RDFG.NODetails)
            {
                string s1 = kvp.Key.Split(':')[0].Replace("/", "_").Replace("#", "");
                if (char.IsDigit(s1[0]))
                    s1 = string.Format("A{0}", s1);

                cins.Add(string.Format("CREATE ({0}:SemanticStructure {{SSName:'{1}', SSType:'{2}'}})", s1, kvp.Value.SSName, kvp.Value.SSType));
            }

            foreach (KeyValuePair<string, string> ked in this.upInd.CurrentDbInstance.OwlData.RDFG.EdgeData)
            {
                string s1 = ked.Key.Split('-')[0].Replace("/", "_").Replace("#", "");
                if (char.IsDigit(s1[0]))
                    s1 = string.Format("A{0}", s1);

                string s2 = ked.Key.Split('-')[1].Replace("/", "_").Replace("#", "");
                if (char.IsDigit(s2[0]))
                    s2 = string.Format("A{0}", s2);
                if (s2.Contains(':'))
                    s2 = s2.Split(':')[0];

                cins.Add(string.Format("CREATE ({0})-[:{2}]->({1})", s1, s2, ked.Value));
            }
            System.IO.File.WriteAllLines(@"C:\WorkRelated-Offline\Dist_Prog_V2\Tools\neo4jexps\WriteNodesKG.txt", cins);
        }
    }
}
