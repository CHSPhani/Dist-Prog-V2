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
    /// Interaction logic for AddNewOP.xaml
    /// </summary>
    public partial class AddNewOP : Window, IProposalResult, ITransitionResult
    {
        AddNewOPModel aOPModel;

        public event RaiseProposeEventHandler RaiseProposal3;


        public AddNewOP()
        {
            InitializeComponent();
        }

        public AddNewOP(string uName, DBData dbData)
        {
            aOPModel = new AddNewOPModel(uName, dbData);
            aOPModel.RaiseProposal2 += AOPModel_RaiseProposal2;
            InitializeComponent();
            this.DataContext = aOPModel;
        }

        private void AOPModel_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            aOPModel.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                aOPModel.ProposalState = true;
            else
                aOPModel.ProposalState = false;
        }
        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                aOPModel.ProposalStatus = "Transition Done";
            else
                aOPModel.ProposalStatus = "Transition Failed";
        }
    }
}
