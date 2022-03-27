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
    /// Interaction logic for AddNewRestri.xaml
    /// </summary>
    public partial class AddNewRestri : Window, IProposalResult, ITransitionResult
    {
        AddNewRstr adNR;

        public event RaiseProposeEventHandler RaiseProposal3;

        public AddNewRestri()
        {
            InitializeComponent();
        }

        public AddNewRestri(string uName, DBData dbData)
        {
            adNR = new AddNewRstr(uName, dbData);
            adNR.RaiseProposal2 += AdNR_RaiseProposal2;
            InitializeComponent();
            this.DataContext = adNR;
        }

        private void AdNR_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            adNR.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                adNR.ProposalState = true;
            else
                adNR.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                adNR.ProposalStatus = "Transition Done";
            else
                adNR.ProposalStatus = "Transition Failed";
        }

    }
}
