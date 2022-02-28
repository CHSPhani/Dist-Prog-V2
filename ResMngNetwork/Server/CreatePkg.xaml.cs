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
    /// Interaction logic for CreatePkg.xaml
    /// </summary>
    public partial class CreatePkg : Window, IProposalResult, ITransitionResult
    {
        InsertPkg iPkg;
        
        public event RaiseProposeEventHandler RaiseProposal3;
        public CreatePkg()
        {
            iPkg = new InsertPkg();
            InitializeComponent();
            this.DataContext = iPkg;
        }

        public CreatePkg(string uName, DBData dbData)
        {       
            iPkg = new InsertPkg(uName, dbData);
            InitializeComponent();
            this.DataContext = iPkg;
            iPkg.RaiseProposal2 += IPkg_RaiseProposal;
        }
      
        private void IPkg_RaiseProposal(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            iPkg.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                iPkg.ProposalState = true;
            else
                iPkg.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                iPkg.ProposalStatus = "Transition Done";
            else
                iPkg.ProposalStatus = "Transition Failed";
            iPkg.TransitDone();
        }
    }
}
