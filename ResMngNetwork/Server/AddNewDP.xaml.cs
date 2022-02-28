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
    /// Interaction logic for AddNewDP.xaml
    /// </summary>
    public partial class AddNewDP : Window, IProposalResult, ITransitionResult
    {
        AddNewDPModel anDPModel;

        public event RaiseProposeEventHandler RaiseProposal3;


        public AddNewDP()
        {
            InitializeComponent();
        }

        public AddNewDP(string uName, DBData dbData)
        {
            anDPModel = new AddNewDPModel(uName, dbData);
            anDPModel.RaiseProposal2 += AnDPModel_RaiseProposal2;
            InitializeComponent();
            this.DataContext = anDPModel;
        }

        private void AnDPModel_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            anDPModel.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                anDPModel.ProposalState = true;
            else
                anDPModel.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                anDPModel.ProposalStatus = "Transition Done";
            else
                anDPModel.ProposalStatus = "Transition Failed";
        }
    }
}
