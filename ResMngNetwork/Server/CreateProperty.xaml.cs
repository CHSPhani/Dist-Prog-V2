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
    /// Interaction logic for CreateProperty.xaml
    /// </summary>
    public partial class CreateProperty : Window, IProposalResult, ITransitionResult
    {
        InsertProperty insProp;

        public event RaiseProposeEventHandler RaiseProposal3;

        public CreateProperty()
        {
            insProp = new InsertProperty();
            InitializeComponent();
            this.DataContext = insProp;
        }

        public CreateProperty(string uName, DBData curDbData)
        {
            insProp = new InsertProperty(uName, curDbData);
            InitializeComponent();
            this.DataContext = insProp;
            insProp.RaiseProposal2 += InsProp_RaiseProposal2;
        }

        private void InsProp_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            insProp.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                insProp.ProposalState = true;
            else
                insProp.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                insProp.ProposalStatus = "Transition Done";
            else
                insProp.ProposalStatus = "Transition Failed";
            insProp.TransitDone();
        }
    }
}
