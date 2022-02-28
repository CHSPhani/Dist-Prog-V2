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
    /// Interaction logic for CreateClass.xaml
    /// </summary>
    public partial class CreateClass : Window, IProposalResult, ITransitionResult
    {
        InsertCls inserCls;
        
        public event RaiseProposeEventHandler RaiseProposal3;

        public CreateClass()
        {
            InitializeComponent();
        }

        public CreateClass(string curUN, DBData curDb)
        {
            inserCls = new InsertCls(curUN, curDb);
            InitializeComponent();
            this.DataContext = inserCls;
            inserCls.RaiseProposal2 += InserCls_RaiseProposal2;
        }

        private void InserCls_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            inserCls.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                inserCls.ProposalState = true;
            else
                inserCls.ProposalState = false;
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                inserCls.ProposalStatus = "Transition Done";
            else
                inserCls.ProposalStatus = "Transition Failed";
            inserCls.TransitDone();
        }
    }
}
