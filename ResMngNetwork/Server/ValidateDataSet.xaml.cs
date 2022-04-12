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
    /// Interaction logic for ValidateDataSet.xaml
    /// </summary>
    public partial class ValidateDataSet : Window, IProposalResult, ITransitionResult
    {
        ValidatorModel vModel;

        public event RaiseProposeEventHandler RaiseProposal3;

        public ValidateDataSet()
        {
            vModel = new ValidatorModel();
            InitializeComponent();
            this.DataContext = vModel;
        }

        public ValidateDataSet(string uName, DBData dbData)
        {
            vModel = new ValidatorModel(uName, dbData);
            vModel.RaiseProposal2 += VModel_RaiseProposal2;
            InitializeComponent();
            this.DataContext = vModel;
        }

        private void VModel_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            // Launch OpenFileDialog by calling ShowDialog method
            DialogResult result = openFileDlg.ShowDialog();
            // Get the selected file name and display in a TextBox.
            // Load content of file in a TextBlock
            if (result ==  System.Windows.Forms.DialogResult.OK)
            {
                vModel.SelFileName = openFileDlg.FileName;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CmbDS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vModel.FillProperties();
        }

        private void CmbIND_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            vModel.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                vModel.ProposalState = true;
            else
                vModel.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                vModel.ProposalStatus = "Transition Done";
            else
                vModel.ProposalStatus = "Transition Failed";
        }
    }
}
