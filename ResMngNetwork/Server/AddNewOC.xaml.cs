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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Interaction logic for AddNewOC.xaml
    /// </summary>
    public partial class AddNewOC : Window, IProposalResult, ITransitionResult
    {
        AddNewOCModel aNoCModel;

        public event RaiseProposeEventHandler RaiseProposal3;

        public AddNewOC()
        {
            InitializeComponent();
        }

        public AddNewOC(string uName, DBData dbData)
        {
            aNoCModel = new AddNewOCModel(uName, dbData);
            aNoCModel.RaiseProposal2 += ANoCModel_RaiseProposal2;
            InitializeComponent();
            this.DataContext = aNoCModel;
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            aNoCModel.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                aNoCModel.ProposalState = true;
            else
                aNoCModel.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                aNoCModel.ProposalStatus = "Transition Done";
            else
                aNoCModel.ProposalStatus = "Transition Failed";
        }

        private void ANoCModel_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
