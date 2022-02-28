using DataSerailizer;
using Server.ChangeRules;
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
    /// Interaction logic for ModifyOP.xaml
    /// </summary>
    public partial class ModifyOP : Window, IProposalResult, ITransitionResult
    {
        MOPModel mopModel;

        public event RaiseProposeEventHandler RaiseProposal3;
        public ModifyOP()
        {
            InitializeComponent();
        }

        public ModifyOP(string uName, DBData dbData)
        {
            mopModel = new MOPModel(uName, dbData);
            mopModel.RaiseProposal2 += MopModel_RaiseProposal2;
            InitializeComponent();
            this.DataContext = mopModel;
        }

        private void MopModel_RaiseProposal2(object sender, ProposeEventArgs e)
        {
            RaiseProposal3?.Invoke(this, e);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CmbPT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mopModel.FillProperties();
        }

        private void CmbPT_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            mopModel.GetPropertyDetails();
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
                mopModel.ProposalStatus = "Transition Done";
            else
                mopModel.ProposalStatus = "Transition Failed";
            
        }

        public void ProcessProposalResult(VoteType overAllType)
        {
            mopModel.ProposalStatus = overAllType.ToString();
            if (overAllType == VoteType.Accepted)
                mopModel.ProposalState = true;
            else
                mopModel.ProposalState = false;
        }
        
        private void TxtRange_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
