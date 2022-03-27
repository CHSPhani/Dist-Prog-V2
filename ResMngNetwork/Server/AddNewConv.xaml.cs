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
    /// Interaction logic for AddNewConv.xaml
    /// </summary>
    public partial class AddNewConv : Window, IProposalResult, ITransitionResult
    {
        AddNewConversion adNC;

        public event RaiseProposeEventHandler RaiseProposal3;


        public AddNewConv()
        {
            InitializeComponent();
        }

        public AddNewConv(string uName, DBData dbData)
        {
            adNC = new AddNewConversion(uName, dbData);
            adNC.RaiseProposal2 += AdNC_RaiseProposal2;
            InitializeComponent();
            this.DataContext = adNC;
        }

        private void AdNC_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            adNC.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                adNC.ProposalState = true;
            else
                adNC.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                adNC.ProposalStatus = "Transition Done";
            else
                adNC.ProposalStatus = "Transition Failed";
        }
    }
}
