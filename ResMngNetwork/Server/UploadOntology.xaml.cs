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

namespace Server
{
    /// <summary>
    /// Interaction logic for UploadOntology.xaml
    /// </summary>
    public partial class UploadOntology : Window, IProposalResult, ITransitionResult
    {
        UploadOntologyFile uoFile;

        public event RaiseProposeEventHandler RaiseProposal3;
        public UploadOntology()
        {
            uoFile = new UploadOntologyFile();
            InitializeComponent();
            this.DataContext = uoFile;
        }
        public UploadOntology(string cuUsr, DBData curDbData)
        {
            uoFile = new UploadOntologyFile(cuUsr,curDbData);
            InitializeComponent();
            this.DataContext = uoFile;
            uoFile.RaiseProposal2 += UoFile_RaiseProposal2;
        }

        private void UoFile_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            // Launch OpenFileDialog by calling ShowDialog method
            DialogResult result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                uoFile.OFilePath = openFileDlg.FileName;
            }
        }

        //Read File and set the DMCLAss in Model
        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OntologyReaderG oReader = new OntologyReaderG();
                OWLDataG oData = oReader.ReadAndCreateOWLData(uoFile.OFilePath);
                uoFile.ODetails = oData;
                uoFile.OUploadStatus = "Upload Done";
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception occured in Creating Ontologies. The Reason in {0}", ex.Message));
            }
            finally
            {

            }
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            uoFile.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                uoFile.ProposalState = true;
            else
                uoFile.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                uoFile.ProposalStatus = "Transition Done";
            else
                uoFile.ProposalStatus = "Transition Failed";
            uoFile.TransitDone();
        }
    }
}
