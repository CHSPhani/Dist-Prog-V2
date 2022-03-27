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
    /// Interaction logic for AddNewDtoDP.xaml
    /// </summary>
    public partial class AddNewDtoDP : Window, IProposalResult, ITransitionResult
    {
        AddNewDToDP addnD;

        public event RaiseProposeEventHandler RaiseProposal3;

        public AddNewDtoDP()
        {
            InitializeComponent();
        }

        public AddNewDtoDP(string uName, DBData dbData)
        {
            addnD = new AddNewDToDP(uName, dbData);
            addnD.RaiseProposal2 += AddnD_RaiseProposal2;
            InitializeComponent();
            this.DataContext = addnD;
        }

        private void AddnD_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            addnD.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                addnD.ProposalState = true;
            else
                addnD.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                addnD.ProposalStatus = "Transition Done";
            else
                addnD.ProposalStatus = "Transition Failed";
        }
    }
}
