using DataSerailizer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Server.Models;
using System.ComponentModel;
using Server.ChangeRules;
using UoB.ToolUtilities.OpenDSSParser;
using Server.UploadIndividuals;
using Server.DataFileProcess;

namespace Server.DSystem
{
    public class ShowResourcesWindow : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            ShowResources sResources = new ShowResources(p1, dbData);
            sResources.ShowDialog();
        }
    }
    public class ShowCreatePkgWindow : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            CreatePkg cPkg = new CreatePkg(p1, dbData);
            cPkg.RaiseProposal3 += CPkg_RaiseProposal3;
            cPkg.Show();
        }

        private void CPkg_RaiseProposal3(object sender, Models.ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class ShowCreateClassWindow : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            CreateClass cClass = new CreateClass(p1, dbData);
            cClass.RaiseProposal3 += CClass_RaiseProposal3;
            cClass.Show();
        }

        private void CClass_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class ShowCreatePropertyWindow : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            CreateProperty cProp = new CreateProperty(p1, dbData);
            cProp.RaiseProposal3 += CProp_RaiseProposal3;
            cProp.Show();
        }

        private void CProp_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class UploadOntologyCmd : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;
            UploadOntology uOntology = new UploadOntology(p1, dbData);
            uOntology.RaiseProposal3 += UOntology_RaiseProposal3;
            uOntology.Show();
        }

        private void UOntology_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class ValidateScreenCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            ValidateDataSet vds = new ValidateDataSet(p1, dbData);
            vds.RaiseProposal3 += Vds_RaiseProposal3;
            vds.Show();
        }

        private void Vds_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class ValidatedDVViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            VDView vdv = new VDView(p1, dbData);
            vdv.Show();
        }
    }
    public class ModifyOPCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            ModifyOP mOp = new ModifyOP(p1, dbData);
            mOp.RaiseProposal3 += MOp_RaiseProposal3;
            mOp.Show();

        }

        private void MOp_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class AddNewDPCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            AddNewDP aDp = new AddNewDP(p1, dbData);
            aDp.RaiseProposal3 += ADp_RaiseProposal3;
            aDp.Show();
        }

        private void ADp_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    
    public class AddNewOPCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            AddNewOP aOp = new AddNewOP(p1, dbData);
            aOp.RaiseProposal3 += AOp_RaiseProposal3;
            aOp.Show();
        }

        private void AOp_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class NOCCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            AddNewOC aNoc = new AddNewOC(p1, dbData);
            aNoc.RaiseProposal3 += ANoc_RaiseProposal3;
            aNoc.Show();
        }

        private void ANoc_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class AddNewDcommand:ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            AddNewDtoDP addnewd = new AddNewDtoDP(p1, dbData);
            addnewd.RaiseProposal3 += Addnewd_RaiseProposal3;
            addnewd.Show();
        }

        private void Addnewd_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class AddNewRestr:ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            AddNewRestri addnewr = new AddNewRestri();
            addnewr.RaiseProposal3 += Addnewr_RaiseProposal3;
            addnewr.Show();
        }

        private void Addnewr_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class AddNewCnvr : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            AddNewConv addnewc = new AddNewConv();
            addnewc.RaiseProposal3 += Addnewr_RaiseProposal3;
            addnewc.Show();
        }

        private void Addnewr_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }
    public class UpdateIndCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event RaiseProposeEventHandler RaiseProposal4;


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                dbData = values[1] as DBData;
            else
                dbData = null;

            UploadIndividual upInd = new UploadIndividual(p1, dbData);
            upInd.RaiseProposal3 += UpInd_RaiseProposal3;
            upInd.Show();
        }

        private void UpInd_RaiseProposal3(object sender, ProposeEventArgs e)
        {
            RaiseProposal4?.Invoke(sender, e);
        }
    }

    public class DSNode : IEquatable<DSNode>, INotifyPropertyChanged
    {
        public string UserName { get; set; }
        public MemberRole UserRole { get; set; }
        public string WorkingDir { get; set; }
        public string DataDir { get; set; }

        public bool isProposer;

        public bool IsProposer
        {
            get
            {
                return this.isProposer;
            }
            set
            {
                this.isProposer = value;
                if (this.isProposer) { this.ProposerState = "Proposer"; } else { this.ProposerState = "Non-Proposer"; }
                OnPropertyChanged("IsProposer");
            }
        }

        string proposerState;

        public string ProposerState
        {
            get
            {
                return this.proposerState;
            }
            set
            {
                this.proposerState = value;
                OnPropertyChanged("ProposerState");
            }
        }
        
        string status;
        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
                OnPropertyChanged("Status");
            }
        }

        bool splPower;
        public bool SplPower
        {
            get
            {
                return this.splPower;
            }
            set
            {
                this.splPower = value;
            }
        }

        public List<DSNode> Incidents { get; set; } 

        DBData initData = null;

        public DBData DataInstance
        {
            get
            {
                return initData;
            }
        }

        NodeData nData;

        public NodeData NodeSpecificData
        {
            get
            {
                return this.nData;
            }
            set
            {
                this.nData = value;
                OnPropertyChanged("NodeSpecificData");
            }
        }

        ShowCreatePkgWindow scPkg;
        public ICommand SCPkg
        {
            get
            {
                if (scPkg == null)
                {
                    scPkg = new ShowCreatePkgWindow();
                    scPkg.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return scPkg;
            }
        }
        
        ShowResourcesWindow srWindow;
        public ICommand SRWindow
        {
            get
            {
                if(srWindow == null)
                {
                    srWindow = new ShowResourcesWindow();
                }
                return srWindow;
            }
        }

        ShowCreateClassWindow scWindow;
        public ICommand SCWindow
        {
            get
            {
                if(scWindow == null)
                {
                    scWindow = new ShowCreateClassWindow();
                    scWindow.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return scWindow;
            }
        }
        
        ShowCreatePropertyWindow spWindow;
        public ICommand SPWindow
        {
            get
            {
                if(spWindow == null)
                {
                    spWindow = new ShowCreatePropertyWindow();
                    spWindow.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return spWindow;
            }
        }

        ValidateScreenCommand vdSet;
        public ICommand ValidateDatasetCommand
        {
            get
            {
                if (vdSet == null)
                {
                    vdSet = new ValidateScreenCommand();
                    vdSet.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return vdSet;
            }
        }

        UpdateIndCommand uiCmd;

        public ICommand UploadIndividualsCommand
        {
            get
            {
                if(uiCmd == null)
                {
                    uiCmd = new UpdateIndCommand();
                    uiCmd.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return uiCmd;
            }
        }

        ValidatedDVViewCommand vdView;
        public ICommand DVViewCommand
        {
            get
            {
                if (vdView == null)
                {
                    vdView = new ValidatedDVViewCommand();
                }
                return vdView;
            }
        }

        UploadOntologyCmd uOntology;
        public ICommand UOntology
        {
            get
            {
                if(uOntology == null)
                {
                    uOntology = new UploadOntologyCmd();
                    uOntology.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return uOntology;
            }
        }

        string voteResult;

        ModifyOPCommand mpCommand;

        public ICommand MOPCommand
        {
            get
            {
                if(mpCommand == null)
                {
                    mpCommand = new ModifyOPCommand();
                    mpCommand.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return mpCommand;
            }
        }

        AddNewDPCommand nDPCommand;

        public ICommand NewDPCommand
        {
            get
            {
                if (nDPCommand == null)
                {
                    nDPCommand = new AddNewDPCommand();
                    nDPCommand.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return nDPCommand;
            }
        }

        ProposeANewUser paNURCommand;

        public ProposeANewUser PANUserCommand
        {
            get
            {
                if(paNURCommand != null)
                {
                    paNURCommand = new ProposeANewUser();
                    paNURCommand.RaiseProposal4 += SpWindow_RaiseProposal4;            
                }
                return paNURCommand;
            }
        }
        
        AddNewOPCommand nOPCommand;
        public ICommand NewOPCommand
        {
            get
            {
                if(nOPCommand == null)
                {
                    nOPCommand = new AddNewOPCommand();
                    nOPCommand.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return nOPCommand;
            }
        }

        NOCCommand aNocCommand;

        public ICommand NewOCCommand
        {
            get
            {
                if (aNocCommand == null)
                {
                    aNocCommand = new NOCCommand();
                    aNocCommand.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return aNocCommand;
            }
        }
        
        AddNewDcommand addNewDCmd;

        public ICommand NewDCmd
        {
            get
            {
                if(addNewDCmd == null)
                {
                    addNewDCmd = new AddNewDcommand();
                    addNewDCmd.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return addNewDCmd;
            }
        }

        AddNewRestr addNewRt;

        public ICommand AddNewRt
        {
            get
            {
                if(addNewRt == null)
                {
                    addNewRt = new AddNewRestr();
                    addNewRt.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return addNewRt;
            }
        }

        AddNewCnvr anCnv;
               
        public ICommand AddNewCnv
        {
            get
            {
                if(anCnv == null)
                {
                    anCnv = new AddNewCnvr();
                    anCnv.RaiseProposal4 += SpWindow_RaiseProposal4;
                }
                return anCnv;
            }
        }

        public string VoteResult
        {
            get
            {
                return voteResult;
            }
            set
            {
                voteResult = value;
                OnPropertyChanged("VoteResult");
            }
        }

        string transitResult;

        public string TransitResult
        {
            get
            {
                return this.transitResult;
            }
            set
            {
                this.transitResult = value;
                OnPropertyChanged("TransitResult");
            }
        }

        VoteType overallVote;
        
        public VoteType OverallVoteType
        {
            get
            {
                return this.overallVote;
            }
            set
            {
                this.overallVote = value;
                OnPropertyChanged("OverallVoteType");
            }
        }

        object eventOriginator;

        public object EOrig
        {
            get
            {
                return this.eventOriginator;
            }
            set
            {
                this.eventOriginator = value;
            }
        }


        //Event for Property Class and PAckage raise proposal
        private void SpWindow_RaiseProposal4(object sender, ProposeEventArgs e)
        {
            NodeMesaage nMsg = e.NMessage;

            if (nMsg.PTYpe == ProposalType.Voting)
            {
                if (!this.SplPower & (this.UserRole == MemberRole.NonVotingMember | this.UserRole == MemberRole.MuteMember))
                {
                    this.Status = string.Format("Can not initiate voting process.");
                    this.EOrig = sender;
                    if (this.EOrig != null)
                    {
                        (EOrig as IProposalResult).ProcessProposalResult(VoteType.NotInitiated);
                    }
                    return;
                }
                if (nMsg.ProposedUser.Equals(this.UserName)) //Setting Proposer
                {
                    this.IsProposer = true;
                    this.EOrig = sender;
                }
                if (!NetworkFunctions.RaiseProposal(this, nMsg))
                {
                    MessageBox.Show("Not able to Raise proposal", "Failure");
                    return;
                }
                NetworkFunctions.Broadcast(NetworkFunctions.GetRootNodeL(), this.UserName, nMsg);
                
            }
            else
            {
                if (nMsg.DirectTransit)
                {
                    if (!NetworkFunctions.RaiseProposal(this, nMsg))
                    {
                        MessageBox.Show("Not able to Raise proposal", "Failure");
                        return;
                    }
                }
                if (nMsg.ProposedUser.Equals(this.UserName)) //Setting Proposer
                {
                    this.IsProposer = true;
                    this.EOrig = sender;
                }
                NetworkFunctions.TransitState(NetworkFunctions.GetRootNodeL(), this.UserName, nMsg);
            }
        }
        private void NetworkFunctions_TCHandler(object sender, EventArgs e)
        {
            if (this.isProposer)
            {
                TCEventArgs tcArgs = e as TCEventArgs;
                if(tcArgs != null)
                {
                    if(tcArgs.proposer == this.UserName)
                    {

                    }
                }
                if (this.EOrig != null)
                {
                    (EOrig as ITransitionResult).ProcessTransitResult(TransitType.Done);
                }
            }
            this.status = "Transition Done";
        }
        private void NetworkFunctions_CCEHandler(object sender, EventArgs e)
        {
            //Consume
            if (this.isProposer)
            {
                CCEventArgs ccEArgs = e as CCEventArgs;
                if (ccEArgs != null)
                {
                    if (ccEArgs.proposer == this.UserName)
                    {
                        this.Status = "ConvergeCast started now";
                        string allvotes = NetworkFunctions.ConvergeCast(NetworkFunctions.GetRootNodeL());
                        this.Status = "ConvergeCast is completed now. Processing all votes";
                        this.OverallVoteType = NetworkFunctions.ProcessAllVotes(allvotes);
                        this.Status = string.Format("All Votes are processed and Overall Result:{0}", OverallVoteType.ToString());
                    }
                }
                if (this.EOrig != null)
                {
                    (EOrig as IProposalResult).ProcessProposalResult(this.OverallVoteType);
                }
            }
            this.Status = "Voting process done.";
        }

        private readonly object msgLock;
        Queue<NodeMesaage> mQueue;
        public void NetworkMessage(string proposer, NodeMesaage nMsg)
        {
            if (this.UserRole == MemberRole.NonVotingMember | this.UserRole == MemberRole.MuteMember)
            {
                this.Status = string.Format("Can not participate in voting {0} Request proposed by {1}", nMsg.PCause, proposer, nMsg.PTYpe);
                NetworkFunctions.CollectVoltes(NetworkFunctions.SpTree, this, new NodeVote() { Vote = VoteType.NA });
                return;
            }
            if (this.UserName == proposer)
                this.Status = string.Format("{0} Request proposed to all Nodes through {2} process", nMsg.PCause, proposer, nMsg.PTYpe);
            else
                this.Status = string.Format("{0} Request Recevied by {1} through {2} process", nMsg.PCause, proposer, nMsg.PTYpe);

            //Primitive way of enqueuing messaged multiple clients sending message to a node
            lock (msgLock)
            {
                mQueue.Enqueue(nMsg);
            }
            while (mQueue.Count != 0)
            {
                NodeVote nVote = new NodeVote();
                VoteType vote = ValidateRules.Validate(initData, mQueue.Dequeue());
                this.Status = string.Format("Vote of {0} casted by {1} through {2} process", vote.ToString(), proposer, nMsg.PTYpe);
                if(vote == VoteType.Abort)
                {
                    nVote.Vote = VoteType.Abort;
                }
                else if (vote == VoteType.Accepted || vote == VoteType.NoObjection)
                {
                    nVote.Vote = VoteType.Accepted;
                }
                else
                {
                    nVote.Vote = VoteType.NotAccepted;
                }
                NetworkFunctions.CollectVoltes(NetworkFunctions.SpTree, this, nVote);
            }
        }

        private readonly object msgLock1;
        Queue<NodeMesaage> tQueue;
        public void TransitState(string proposer, NodeMesaage nMsg)
        {
            this.Status = string.Format("{0} Request Recevied by {1} through {2} process", nMsg.PCause, proposer, nMsg.PTYpe);

            lock (msgLock1)
            {
                tQueue.Enqueue(nMsg);
            }
            while (tQueue.Count != 0)
            {
                if (nMsg.PCause == ProposalCause.NewPackage)
                {
                    DBInsert.InsertNewPackage(tQueue.Dequeue(), this.initData);
                    this.Status = string.Format("Package details Updated.");
                }
                if (nMsg.PCause == ProposalCause.NewClass)
                {
                    DBInsert.InsertClassEntry(tQueue.Dequeue(), this.initData);
                    this.Status = string.Format("Class details Updated.");
                }
                if (nMsg.PCause == ProposalCause.NewProperty)
                {
                    DBInsert.InsertPropertyEntry(tQueue.Dequeue(), this.initData);
                    this.Status = string.Format("Property details Updated.");
                }
                if (nMsg.PCause == ProposalCause.NewOntology)
                {
                    //NodeMesaage nMsg1 = tQueue.Dequeue();
                    //OWLData oD = nMsg1.OwlData;
                    //foreach(OClass oC in oD.OWLClasses)
                    //{
                    //    this.initData.OwlData.OWLClasses.Add(oC);
                    //}
                    //foreach(ODataProperty oDP in oD.OWLDataProperties)
                    //{
                    //    this.initData.OwlData.OWLDataProperties.Add(oDP);
                    //}
                    //foreach(OObjectProperty oOP in oD.OWLObjProperties)
                    //{
                    //    this.initData.OwlData.OWLObjProperties.Add(oOP);
                    //}
                    //foreach(KeyValuePair<string,string> nSP in oD.OWLNameSpaces)
                    //{
                    //    if (!oD.OWLNameSpaces.ContainsKey(nSP.Key))
                    //        this.initData.OwlData.OWLNameSpaces.Add(nSP.Key, nSP.Value);
                    //}
                }
                if (nMsg.PCause == ProposalCause.NewDataProperty)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    string propName = nMsg1.DataItems[0];
                    string selectedCN = nMsg1.DataItems[1];
                    bool functional = false;
                    if (nMsg1.DataItems[2].ToLower().Equals("true"))
                        functional = true;
                    else
                        functional = false;
                    string selectedDT = nMsg1.DataItems[3];
                    //string expr = nMsg1.DataItems[4];

                    //First see whether the class exists or not
                    string cls1 = initData.OwlData.RDFG.GetExactNodeName(selectedCN);
                    SemanticStructure sc = null;
                    sc = initData.OwlData.RDFG.GetNodeDetails(selectedCN); //cls1
                    if (sc == null)
                    {
                        //Adding New Class
                        sc = new SemanticStructure() { SSName = selectedCN, SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                        //Add to Graph
                        initData.OwlData.RDFG.AddNode(sc.ToString());
                        initData.OwlData.RDFG.AddEntryToNODetail(sc.ToString(), sc);
                    }

                    //Start creating Data Property
                    //http://www.bristol.ac.uk/sles/v1/opendsst2 is XML URI
                    SemanticStructure ss = new SemanticStructure() { SSName = propName, SSType = SStrType.DatatypeProperty, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                    initData.OwlData.RDFG.AddNode(ss.ToString());
                    initData.OwlData.RDFG.AddEntryToNODetail(ss.ToString(), ss);

                    //Adding to class
                    string edKey1 = string.Format("{0}-{1}", ss.SSName, sc.SSName);
                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                        initData.OwlData.RDFG.EdgeData[edKey1] = SStrType.Class.ToString();
                    //add edge
                    initData.OwlData.RDFG.AddEdge(ss.ToString(), sc.ToString());

                    //Id functional Add functional property to Data Property
                    if (functional)
                    {
                        string fCls1 = initData.OwlData.RDFG.GetExactNodeName("FunctionalProperty");
                        SemanticStructure ss2 = null;
                        ss2 = initData.OwlData.RDFG.GetNodeDetails("FunctionalProperty");
                        if (ss2 == null)
                        {
                            ss2 = new SemanticStructure() { SSName = "FunctionalProperty", SSType = SStrType.Type, XMLURI = "http://www.w3.org/2002/07/owl" };
                            initData.OwlData.RDFG.AddNode(ss2.ToString());
                            initData.OwlData.RDFG.AddEntryToNODetail(ss2.ToString(), ss2);
                        }
                        string edKey = string.Format("{0}-{1}", ss.SSName, ss2.SSName);
                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey))
                            initData.OwlData.RDFG.EdgeData[edKey] = SStrType.Type.ToString();
                        //add edge
                        initData.OwlData.RDFG.AddEdge(ss.ToString(), ss2.ToString());
                    }

                 
                    //3. Range
                    string dCls1 = initData.OwlData.RDFG.GetExactNodeName(selectedDT);
                    SemanticStructure ss4 = null;
                    ss4 = initData.OwlData.RDFG.GetNodeDetails(selectedDT);
                    if (ss4 == null)
                    {
                        ss4 = new SemanticStructure() { SSName = selectedDT, SSType = SStrType.Range, XMLURI = "http://www.w3.org/2001/XMLSchema" };
                        initData.OwlData.RDFG.AddNode(ss4.ToString());
                        initData.OwlData.RDFG.AddEntryToNODetail(ss4.ToString(), ss4);
                    }
                    string edKey2 = string.Format("{0}-{1}", ss.SSName, ss4.SSName);
                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                        initData.OwlData.RDFG.EdgeData[edKey2] = SStrType.Range.ToString();
                    //add edge
                    initData.OwlData.RDFG.AddEdge(ss.ToString(), ss4.ToString());
                    
                    this.Status = string.Format("New Data Property details Updated.");
                }
                if (nMsg.PCause == ProposalCause.NewObjectProperty)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    
                    string npName = nMsg1.DataItems[0];
                    string sName = nMsg1.DataItems[1];
                    string tName = nMsg1.DataItems[2];
                    string inverseProp = nMsg1.DataItems[3];

                    //Start creating Object Property
                    //http://www.bristol.ac.uk/sles/v1/opendsst2 is XML URI
                    SemanticStructure ss = new SemanticStructure() { SSName = npName, SSType = SStrType.ObjectProperty, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                    initData.OwlData.RDFG.AddNode(ss.ToString());
                    initData.OwlData.RDFG.AddEntryToNODetail(ss.ToString(), ss);

                    //1.
                    SemanticStructure ss2 = new SemanticStructure() { SSName = "topObjectProperty", SSType = SStrType.subPropertyOf, XMLURI = "http://www.w3.org/2002/07/owl" };
                    string edKey = string.Format("{0}-{1}", ss.SSName, ss2.SSName);
                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey))
                        initData.OwlData.RDFG.EdgeData[edKey] = ss2.SSType.ToString();
                    //add edge
                    initData.OwlData.RDFG.AddEdge(ss.ToString(), ss2.ToString());

                    //2.Inverse Prop
                    if (!string.IsNullOrEmpty(inverseProp))
                    {
                        SemanticStructure ss3 = new SemanticStructure() { SSName = inverseProp, SSType = SStrType.inverseOf, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                        string edKey1 = string.Format("{0}-{1}", ss.SSName, ss3.SSName);
                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                            initData.OwlData.RDFG.EdgeData[edKey1] = ss3.SSType.ToString();
                        //add edge
                        initData.OwlData.RDFG.AddEdge(ss.ToString(), ss3.ToString());
                    }
                    //3. Set Domain
                    if (!string.IsNullOrEmpty(sName))
                    {
                        SemanticStructure ss4 = new SemanticStructure() { SSName = sName, SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                        string edKey4 = string.Format("{0}-{1}", ss.SSName, ss4.SSName);
                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey4))
                            initData.OwlData.RDFG.EdgeData[edKey4] = ss4.SSType.ToString();
                        //add edge
                        initData.OwlData.RDFG.AddEdge(ss.ToString(), ss4.ToString());
                    }

                    //3. Set Range
                    if (!string.IsNullOrEmpty(tName))
                    {
                        SemanticStructure ss5 = new SemanticStructure() { SSName = tName, SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                        string edKey5 = string.Format("{0}-{1}", ss.SSName, ss5.SSName);
                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey5))
                            initData.OwlData.RDFG.EdgeData[edKey5] = ss5.SSType.ToString();
                        //add edge
                        initData.OwlData.RDFG.AddEdge(ss.ToString(), ss5.ToString());
                    }
                    this.Status = string.Format("New Object Property details Updated.");
                }
                if (nMsg.PCause == ProposalCause.ModifyDataProperty)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    string clsName = nMsg.DataItems[0];
                    string newRange = nMsg.DataItems[1];
                    string pDT = nMsg.DataItems[2];
                    string newExpr = nMsg.DataItems[3];

                    //string oldClsName = nMsg.DataItems[2];
                    bool igCols = true;
                    if (nMsg.DataItems[4].ToLower().Equals("true"))
                        igCols = true;
                    else
                        igCols = false;

                    ODataProperty oldDP = nMsg.OldDP;

                    SemanticStructure sc = null;
                    if (newRange.Contains('-'))
                        sc = new SemanticStructure() { SSName = string.Format("Expr>{0}", newRange.Split('-')[1].Split(':')[0]), SSType = SStrType.RangeExpression, XMLURI = newExpr };
                    else
                        sc = new SemanticStructure() { SSName = string.Format("Expr>{0}", newRange.Split(':')[0]), SSType = SStrType.RangeExpression, XMLURI = newExpr };

                    SemanticStructure sms = null;

                    if (initData.OwlData.RDFG.AddNode(sc.ToString()))
                    { }
                    else
                    {
                        //Modifying content
                        sms = initData.OwlData.RDFG.NODetails[sc.ToString()];
                        sms.SSName = sc.SSName;
                        sms.SSType = sc.SSType;
                        sms.XMLURI = sc.XMLURI;
                    }
                    initData.OwlData.RDFG.AddEntryToNODetail(sc.ToString(), sc); //add anyway

                    if (sms == null) 
                    { //here we are adding new expression...
                        string aClsName = initData.OwlData.RDFG.GetExactNodeName(clsName);

                        SemanticStructure ss = initData.OwlData.RDFG.NODetails[aClsName];

                        string edKey = string.Format("{0}-{1}", ss.SSName, sc.SSName);
                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey))
                            initData.OwlData.RDFG.EdgeData[edKey] = sc.SSType.ToString();
                        //add edge
                        initData.OwlData.RDFG.AddEdge(ss.ToString(), sc.ToString()); //di I forgot this?
                    }
                    
                }
                if (nMsg.PCause == ProposalCause.NewOClass)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    string newCls = nMsg1.DataItems[0];
                    string newBaseCls = nMsg1.DataItems[1];

                    string nBName = initData.OwlData.RDFG.GetExactNodeName(newBaseCls);
                    SemanticStructure ssb = initData.OwlData.RDFG.NODetails[nBName];

                    //http://www.bristol.ac.uk/sles/v1/opendsst2 is XML URI
                    SemanticStructure ss = new SemanticStructure() { SSName = newCls, SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                    if (initData.OwlData.RDFG.AddNode(ss.ToString()))
                        initData.OwlData.RDFG.AddEntryToNODetail(ss.ToString(), ss);

                    string edKey = string.Format("{0}-{1}", ssb.SSName, ss.SSName);//newBaseCls
                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey))
                        initData.OwlData.RDFG.EdgeData[edKey] = SStrType.SubClassOf.ToString();

                    this.initData.OwlData.RDFG.AddEdge(string.Format("{0}:{1}", ssb.SSName, SStrType.Class), string.Format("{0}:{1}",ss.SSName,SStrType.Class));
                }
                if (nMsg.PCause == ProposalCause.UploadInd)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    List<CircuitEntry> entries = nMsg1.CEntities;
                    foreach (CircuitEntry ce in entries)
                    {
                        try
                        {
                            string nName = initData.OwlData.RDFG.GetExactNodeName(ce.CEType);
                            if (!string.IsNullOrEmpty(nName))
                            {
                                //Step1: Add an Instance using ce.CEName 

                                SemanticStructure sc = new SemanticStructure() { SSName = ce.CEName, SSType = SStrType.Instance, XMLURI = string.Empty };

                                initData.OwlData.RDFG.AddNode(sc.ToString());
                                initData.OwlData.RDFG.AddEntryToNODetail(sc.ToString(), sc);

                                SemanticStructure ss = initData.OwlData.RDFG.NODetails[nName];

                                string edKey = string.Format("{0}-{1}", ss.SSName, sc.SSName);
                                if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey))
                                    initData.OwlData.RDFG.EdgeData[edKey] = sc.SSType.ToString();
                                //add edge
                                initData.OwlData.RDFG.AddEdge(ss.ToString(), sc.ToString()); //di I forgot this?

                                //Step2: Get outgoing and incoming edges
                                List<string> outgoing = initData.OwlData.RDFG.GetEdgesForNode(nName);
                                List<string> incoming = initData.OwlData.RDFG.GetIncomingEdgesForNode(nName);
                                List<string> classToBeUsed = new List<string>();
                                List<string> classHierarchy = new List<string>();
                                //outgoing.Sort();
                                //sorting outgoing list because i added number whle adding outgoing properties.
                                SortedList<int, string> nsList = new SortedList<int, string>();
                                foreach (string s in outgoing)
                                {
                                    nsList[Int32.Parse(s.Split('-')[0])] = s.Split('-')[1];
                                }
                                List<string> nOutgoing = new List<string>();
                                foreach (KeyValuePair<int, string> kvp in nsList)
                                {
                                    nOutgoing.Add(string.Format("{0}-{1}", kvp.Key, kvp.Value));
                                }
                                //sorting done
                                int counter = 0;
                                while (counter <= nOutgoing.Count - 1)
                                {
                                    string ots = nOutgoing[counter];
                                    string[] reqParts = ots.Split('-')[1].Split(':');
                                    if (reqParts[1].ToLower().Equals("class"))
                                    {
                                        //Step3: Calculating data properties up in hierarchies 
                                        //Seems like not required to get all properties
                                        #region Good Recursive Code for getting up hierarchies
                                        string relation = string.Empty;
                                        if (initData.OwlData.RDFG.EdgeData.ContainsKey(string.Format("{0}-{1}", nName.Split(':')[0], reqParts[0])))
                                            relation = initData.OwlData.RDFG.EdgeData[string.Format("{0}-{1}", nName.Split(':')[0], reqParts[0])];

                                        //if sub class I want to add all data properties gathered from all base classes till owl:Thing
                                        if (relation.ToLower().Equals("subclassof"))
                                        {
                                            string source = ots.Split('-')[1];
                                            //classHierarchy.Add(source);
                                            while (!source.ToLower().Equals("owl:thing"))
                                            {
                                                classHierarchy.Add(source);
                                                List<string> og = initData.OwlData.RDFG.GetEdgesForNode(source);
                                                List<string> ic = initData.OwlData.RDFG.GetIncomingEdgesForNode(source);
                                                foreach (string ics in ic)
                                                {
                                                    if (ics.Split(':')[1].Equals("DatatypeProperty"))
                                                        incoming.Add(ics);
                                                }
                                                foreach (string sst in og) //must be one only
                                                    source = sst.Split('-')[1];
                                            }
                                            classHierarchy.Add(source);
                                        }
                                        #endregion
                                    }
                                    else if (reqParts[1].ToLower().Equals("objectproperty"))
                                    {
                                        //Step4: Calculate restrictions
                                        classToBeUsed.Add(ots.Split('-')[1]);
                                        while (true)
                                        {
                                            if ((counter + 1) > nOutgoing.Count - 1)
                                                break;
                                            string tots = nOutgoing[++counter];

                                            string[] treqParts = tots.Split('-')[1].Split(':');
                                            if (treqParts[1].ToLower().Equals("objectproperty") || treqParts[1].ToLower().Equals("instance"))
                                            {
                                                --counter;
                                                break;
                                            }
                                            else
                                                classToBeUsed.Add(tots.Split('-')[1]);
                                        }
                                        //Need to add nodes and edges using ce.CEEntries
                                        foreach (string s in classToBeUsed)
                                        {
                                            //Step4.1: Add an Instance using ce.CEName 
                                            string pN = UploadIndividualsToRDFG.GetParamNameForNode(s);

                                            SemanticStructure ssN = initData.OwlData.RDFG.NODetails[s];

                                            //Get value from CEEntries
                                            List<string> values = new List<string>();
                                            foreach (string ces in ce.CEEntries)
                                            {
                                                if (ces.ToLower().Split('=')[0].Contains(pN.ToLower()))
                                                {
                                                    values.Add(ces.Split('=')[1]);
                                                }
                                            }
                                            if (values.Count == 0)
                                            {
                                                SemanticStructure scc = new SemanticStructure() { SSName = string.Format("inst_{0}", s), SSType = SStrType.Instance, XMLURI = string.Empty };

                                                if (initData.OwlData.RDFG.AddNode(scc.ToString()))
                                                    initData.OwlData.RDFG.AddEntryToNODetail(scc.ToString(), scc);

                                                string edKey1 = string.Format("{0}-{1}", ssN.SSName, scc.SSName);
                                                if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                                                    initData.OwlData.RDFG.EdgeData[edKey1] = scc.SSType.ToString();
                                                //add edge
                                                initData.OwlData.RDFG.AddEdge(ssN.ToString(), scc.ToString()); //di I forgot this?

                                                string edKey2 = string.Format("{0}-{1}", sc.SSName, scc.SSName);
                                                if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                                                    initData.OwlData.RDFG.EdgeData[edKey2] = scc.SSType.ToString();
                                                //add edge
                                                initData.OwlData.RDFG.AddEdge(sc.ToString(), scc.ToString()); //di I forgot this?
                                            }
                                            else
                                            {
                                                int insCounter = 1;
                                                foreach (string vl in values)
                                                {
                                                    SemanticStructure scc = new SemanticStructure() { SSName = string.Format("inst_{0}_{1}:{2}", s.Split(':')[0], insCounter, s.Split(':')[1]), SSType = SStrType.Instance, XMLURI = vl };

                                                    if (initData.OwlData.RDFG.AddNode(scc.ToString()))
                                                        initData.OwlData.RDFG.AddEntryToNODetail(scc.ToString(), scc);

                                                    string edKey1 = string.Format("{0}-{1}", ssN.SSName, scc.SSName);
                                                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                                                        initData.OwlData.RDFG.EdgeData[edKey1] = scc.SSType.ToString();
                                                    //add edge
                                                    initData.OwlData.RDFG.AddEdge(ssN.ToString(), scc.ToString()); //di I forgot this?

                                                    string edKey2 = string.Format("{0}-{1}", sc.SSName, scc.SSName);
                                                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                                                        initData.OwlData.RDFG.EdgeData[edKey2] = scc.SSType.ToString();
                                                    //add edge
                                                    initData.OwlData.RDFG.AddEdge(sc.ToString(), scc.ToString()); //di I forgot this?

                                                    insCounter++;
                                                }
                                            }
                                        }
                                        classToBeUsed.Clear();
                                    }
                                    else
                                    {
                                        //what is this? 
                                    }
                                    counter++;
                                }
                                //Step5: Adding object properties from up in the hierarchy
                                foreach (string cName in classHierarchy)
                                {
                                    if (!string.IsNullOrEmpty(cName))
                                    {
                                        if (cName.Equals("owl:thing"))
                                            continue;
                                        //Step5.1: Get outgoing and incoming edges
                                        List<string> og = initData.OwlData.RDFG.GetEdgesForNode(cName);
                                        List<string> inc = initData.OwlData.RDFG.GetIncomingEdgesForNode(cName);
                                        List<string> cToBeUsed = new List<string>();

                                        //sorting outgoing list because i added number whle adding outgoing properties.
                                        SortedList<int, string> nList = new SortedList<int, string>();
                                        foreach (string s in og)
                                        {
                                            nList[Int32.Parse(s.Split('-')[0])] = s.Split('-')[1];
                                        }
                                        List<string> nOG = new List<string>();
                                        foreach (KeyValuePair<int, string> kvp in nList)
                                        {
                                            nOG.Add(string.Format("{0}-{1}", kvp.Key, kvp.Value));
                                        }

                                        int inCntr = 0;
                                        while (inCntr <= nOG.Count - 1)
                                        {
                                            string ots = nOG[inCntr];
                                            string[] reqParts = ots.Split('-')[1].Split(':');
                                            if (reqParts[1].ToLower().Equals("objectproperty"))
                                            {
                                                //Step4: Calculate restrictions
                                                classToBeUsed.Add(ots.Split('-')[1]);
                                                while (true)
                                                {
                                                    if ((counter + 1) > nOG.Count - 1)
                                                        break;
                                                    string tots = nOG[++counter];

                                                    string[] treqParts = tots.Split('-')[1].Split(':');
                                                    if (treqParts[1].ToLower().Equals("objectproperty"))
                                                    {
                                                        --counter;
                                                        break;
                                                    }
                                                    else
                                                        classToBeUsed.Add(tots.Split('-')[1]);
                                                }
                                                //Need to add nodes and edges using ce.CEEntries
                                                foreach (string s in classToBeUsed)
                                                {
                                                    //Step4.1: Add an Instance using ce.CEName 
                                                    string pN = UploadIndividualsToRDFG.GetParamNameForNode(s);

                                                    SemanticStructure ssN = initData.OwlData.RDFG.NODetails[s];

                                                    //Get value from CEEntries
                                                    List<string> values = new List<string>();
                                                    foreach (string ces in ce.CEEntries)
                                                    {
                                                        if (ces.ToLower().Split('=')[0].Contains(pN.ToLower()))
                                                        {
                                                            values.Add(ces.Split('=')[1]);
                                                        }
                                                    }
                                                    if (values.Count == 0)
                                                    {
                                                        SemanticStructure scc = new SemanticStructure() { SSName = string.Format("inst_{0}_{1}", ce.CEName, s), SSType = SStrType.Instance, XMLURI = string.Empty };

                                                        initData.OwlData.RDFG.AddNode(scc.ToString());
                                                        initData.OwlData.RDFG.AddEntryToNODetail(scc.ToString(), scc);

                                                        string edKey1 = string.Format("{0}-{1}", ssN.SSName, scc.SSName);
                                                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                                                            initData.OwlData.RDFG.EdgeData[edKey1] = scc.SSType.ToString();
                                                        //add edge
                                                        initData.OwlData.RDFG.AddEdge(ssN.ToString(), scc.ToString()); //di I forgot this?

                                                        string edKey2 = string.Format("{0}-{1}", sc.SSName, scc.SSName);
                                                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                                                            initData.OwlData.RDFG.EdgeData[edKey2] = scc.SSType.ToString();
                                                        //add edge
                                                        initData.OwlData.RDFG.AddEdge(sc.ToString(), scc.ToString()); //di I forgot this?
                                                    }
                                                    else
                                                    {
                                                        int insCounter = 1;
                                                        foreach (string vl in values)
                                                        {
                                                            SemanticStructure scc = new SemanticStructure() { SSName = string.Format("inst_{0}_{1}_{2}:{3}", ce.CEName, s.Split(':')[0], insCounter, s.Split(':')[1]), SSType = SStrType.Instance, XMLURI = vl };

                                                            initData.OwlData.RDFG.AddNode(scc.ToString());
                                                            initData.OwlData.RDFG.AddEntryToNODetail(scc.ToString(), scc);

                                                            string edKey1 = string.Format("{0}-{1}", ssN.SSName, scc.SSName);
                                                            if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                                                                initData.OwlData.RDFG.EdgeData[edKey1] = scc.SSType.ToString();
                                                            //add edge
                                                            initData.OwlData.RDFG.AddEdge(ssN.ToString(), scc.ToString()); //di I forgot this?

                                                            string edKey2 = string.Format("{0}-{1}", sc.SSName, scc.SSName);
                                                            if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                                                                initData.OwlData.RDFG.EdgeData[edKey2] = scc.SSType.ToString();
                                                            //add edge
                                                            initData.OwlData.RDFG.AddEdge(sc.ToString(), scc.ToString()); //di I forgot this?

                                                            insCounter++;
                                                        }
                                                    }
                                                }
                                                classToBeUsed.Clear();
                                            }
                                            inCntr++;
                                        }
                                    }
                                }
                                //Step6: Add nodes and edges for data properties using ce.CEEntries
                                foreach (string ins in incoming)
                                {
                                    SemanticStructure ssN = initData.OwlData.RDFG.NODetails[ins];

                                    //Get value from CEEntries
                                    string value = string.Empty;
                                    if (ins.Split(':')[0].ToLower().Equals("name"))
                                        value = ce.CEName;
                                    else
                                    {
                                        foreach (string ces in ce.CEEntries)
                                        {
                                            if (ces.ToLower().Split('=')[0].Contains(ins.Split(':')[0].ToLower()))
                                            {
                                                value = ces.Split('=')[1];
                                            }
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        SemanticStructure scc = new SemanticStructure() { SSName = string.Format("inst_{0}_{1}", ce.CEName, ins), SSType = SStrType.Instance, XMLURI = value };

                                        //if (initData.OwlData.RDFG.AddNode(scc.ToString()))
                                        //    initData.OwlData.RDFG.AddEntryToNODetail(scc.ToString(), scc);

                                        initData.OwlData.RDFG.AddNode(scc.ToString());
                                        initData.OwlData.RDFG.AddEntryToNODetail(scc.ToString(), scc);

                                        string edKey1 = string.Format("{0}-{1}", ssN.SSName, scc.SSName);
                                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                                            initData.OwlData.RDFG.EdgeData[edKey1] = scc.SSType.ToString();
                                        //add edge
                                        initData.OwlData.RDFG.AddEdge(ssN.ToString(), scc.ToString()); //di I forgot this?


                                        string edKey2 = string.Format("{0}-{1}", sc.SSName, scc.SSName);
                                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                                            initData.OwlData.RDFG.EdgeData[edKey2] = scc.SSType.ToString();
                                        //add edge
                                        initData.OwlData.RDFG.AddEdge(sc.ToString(), scc.ToString()); //di I forgot this?
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string s = ex.Message + ce.CEName;
                        }
                    }
                    this.Status = string.Format("New Upload details Updated.");
                }
                if (nMsg.PCause == ProposalCause.OntoChanges)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    string selectedCls = nMsg1.DataItems[0];
                    bool connected = false;
                    string boolConn = nMsg1.DataItems[1];
                    bool.TryParse(boolConn, out connected);
                    //Change Ontology
                    ValidateDataSets.CreateOntEgClassV1(initData, selectedCls, nMsg1.OModel, connected);
                    this.Status = string.Format("Ontology Changed according to new additions.");
                }
                NetworkFunctions.CollectTransition(NetworkFunctions.SpTree, this);
            }
        }
        
        public DSNode()
        {
            UserName = string.Empty;
            UserRole = MemberRole.None;
            WorkingDir = string.Empty;
            DataDir = string.Empty;
            Status = string.Empty;
            Incidents = new List<DSNode>();
            msgLock = new object();
            msgLock1 = new object();
            mQueue = new Queue<NodeMesaage>();
            tQueue = new Queue<NodeMesaage>();
            this.IsProposer = false;
            this.SplPower = false; 

            paNURCommand = new ProposeANewUser();
            paNURCommand.RaiseProposal4 += SpWindow_RaiseProposal4;

            NetworkFunctions.CCEHandler += NetworkFunctions_CCEHandler;
            NetworkFunctions.TCHandler += NetworkFunctions_TCHandler;
        }
        
        public bool SerializeDataForNode()
        {
            try
            {
                DataSerializer dSerializer = new DataSerializer();
                dSerializer.SerializeNodeDB(this.initData, this.DataDir, true);
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeSerializeInitData()
        {
            DataSerializer dSerializer = new DataSerializer();
            
            try
            {
                initData = dSerializer.DeSerialize(@"C:\WorkRelated-Offline\Dist_Prog_V2\InitialDB\InitialDB.dat");
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                return false;
            }
            finally
            {
                
            }
            return true;
        }
        public bool DeSerializeInitData(string dbFilePath)
        {
            DataSerializer dSerializer = new DataSerializer();

            try
            {
                initData = dSerializer.DeSerialize(dbFilePath);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                return false;
            }
            finally
            {

            }
            return true;
        }

        public bool Equals(DSNode other)
        {
            if (other == null)
                return false;
            DSNode otherNode = other as DSNode;
            if (otherNode == null)
                return false;
            else
            {
                if (this.UserName == otherNode.UserName)
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            return this.UserName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum TypeNames
    {
        Class,
        Property,
        Package,
        Enum
    }
    public class DBInsert
    {
        public static bool InsertNewPackage(NodeMesaage nMsg, DBData dbData)
        {
            try
            {
                PackageData pData = new PackageData();
                int maxEtid = dbData.PkgData.Max(p => p.ETID);
                pData.ETID = ++maxEtid;
                pData.MasterPkgName = "UoB.CIM.Resources.";
                pData.PName = nMsg.DataItems[0];
                pData.PkgName = nMsg.DataItems[1];
                pData.PkgNotes = nMsg.DataItems[2];
                pData.TypeName = TypeNames.Package.ToString();
                dbData.PkgData.Add(pData);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool InsertClassEntry(NodeMesaage nMSg, DBData dbData)
        {
            try
            {
                EntityData eData = new EntityData();
                int maxEtid = dbData.ClassData.Max(c => c.ETID);
                eData.ETID = ++maxEtid;
                eData.ETName = nMSg.DataItems[1];
                eData.ETNotes = nMSg.DataItems[3];
                string mUri = DBInsert.ReplaceDotWithSlash(nMSg.DataItems[2]);
                eData.ETURIName = mUri;
                string feName = String.Format("{0}v1.1/{1}/{2}", "http://bristol.ac.uk/sles/cim/", mUri, nMSg.DataItems[1]);
                eData.FullEntityName = feName;
                eData.MasterURIName = "http://bristol.ac.uk/sles/cim/";
                eData.TypeName = TypeNames.Class.ToString();
                dbData.ClassData.Add(eData);

                if (!string.IsNullOrEmpty(nMSg.DataItems[4])) //!(nMSg.DataItems[4].Equals("ClassEmpty")) 
                {
                    RelationsData rData = new RelationsData();
                    rData.SourceETID = maxEtid;
                    rData.TargetETID = DBInsert.GetBaseClassID(dbData, nMSg.DataItems[4]);
                    rData.RelName = "isa";
                    rData.RelNotes = string.Format("{0}-{1}-isa", feName, nMSg.DataItems[4]);
                    dbData.RelationData.Add(rData);
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        static string ReplaceDotWithSlash(string iStr)
        {
            return iStr.Replace(".", "/");
        }
        static int GetBaseClassID(DBData curDBData, string cnName)
        {
            int cId = -1;
            try
            {
                foreach (EntityData eData in curDBData.ClassData)
                {
                    if (eData.FullEntityName.Equals(cnName))
                    {
                        cId = eData.ETID;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception while getting base class Id. Details are {0}", ex.Message));
            }
            return cId;
        }

        public static bool InsertPropertyEntry(NodeMesaage nMSg, DBData dbData)
        {
            try
            {
                EntityData pData = new EntityData();
                int maxEtid = dbData.PropertyData.Max(p => p.ETID);
                pData.ETID = ++maxEtid;
                pData.ETName = nMSg.DataItems[1];
                pData.ETNotes = nMSg.DataItems[5];
                string mUri = DBInsert.ReplaceDotWithSlash(nMSg.DataItems[4]);
                pData.ETURIName = mUri;
                string feName = String.Format("{0}v1.1/{1}/{2}", "http://bristol.ac.uk/sles/cim/", mUri, nMSg.DataItems[1]);
                pData.FullEntityName = feName;
                pData.MasterURIName = "http://bristol.ac.uk/sles/cim/";
                pData.TypeName = TypeNames.Property.ToString();
                dbData.PropertyData.Add(pData);

                //Property Type
                if (!string.IsNullOrEmpty(nMSg.DataItems[2]))
                {
                    RelationsData rData = new RelationsData();
                    rData.SourceETID = maxEtid;
                    rData.TargetETID = DBInsert.GetTypesID(dbData, nMSg.DataItems[2]);
                    rData.RelName = "typeOf";
                    rData.RelNotes = string.Format("{0}-{1}-typeOf", feName, nMSg.DataItems[2]);
                    dbData.RelationData.Add(rData);
                }

                //Container class for Property
                if (!string.IsNullOrEmpty(nMSg.DataItems[3]))
                {
                    RelationsData rData = new RelationsData();
                    rData.SourceETID = maxEtid;
                    rData.TargetETID = DBInsert.GetClassID(dbData, nMSg.DataItems[3]);
                    rData.RelName = "propertyof";
                    rData.RelNotes = string.Format("{0}-{1}-propertyof", feName, nMSg.DataItems[3]);
                    dbData.RelationData.Add(rData);
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        static int GetTypesID(DBData curDBData, string typeName)
        {
            int pID = -1;
            try
            {
                foreach (EntityData eData in curDBData.ClassData)
                {
                    string cName = string.Format("{0}/{1}", eData.ETURIName, eData.ETName);
                    if (cName.Equals(typeName))
                        pID = eData.ETID;
                }
                foreach (EntityData eData in curDBData.PropertyData)
                {
                    string pName = string.Format("{0}/{1}", eData.ETURIName, eData.ETName);
                    if (pName.Equals(typeName))
                        pID = eData.ETID;
                }
                foreach (EntityData eData in curDBData.EnumData)
                {
                    string enData = string.Format("{0}/{1}", eData.ETURIName, eData.ETName);
                    if (enData.Equals(typeName))
                        pID = eData.ETID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception while getting Property ID details. Details are {0}", ex.Message));
            }
            return pID;
        }

        static int GetClassID(DBData curDBData, string clsName)
        {
            int cID = -1;
            try
            {
                foreach (EntityData eData in curDBData.ClassData)
                {
                    if (eData.FullEntityName.Equals(clsName))
                        cID = eData.ETID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception while getting namespace details from entities and masterdetails. Details are {0}", ex.Message));
            }
            return cID;
        }
    }

}

/*
   #region Data Classes


            //Class Event
        private void ScWindow_RaiseProposal4(object sender, ProposeEventArgs e)
        {
            NodeMesaage nMsg = e.NMessage;

            if (this.UserRole == MemberRole.NonVotingMember | this.UserRole == MemberRole.MuteMember)
            {
                this.Status = string.Format("Can not initiate voting process.");
                this.EOrig = sender;
                if (this.EOrig != null)
                {
                    (EOrig as IProposalResult).ProcessProposalResult(VoteType.NotInitiated);
                }
                return;
            }
            if (nMsg.ProposedUser.Equals(this.UserName)) //Setting Proposer
            {
                this.IsProposer = true;
                this.EOrig = sender;
            }
            if (!NetworkFunctions.RaiseProposal(this, nMsg))
            {
                MessageBox.Show("Not able to Raise proposal", "Failure");
                return;
            }
            NetworkFunctions.Broadcast(NetworkFunctions.GetRootNodeL(), this.UserName, nMsg);

        }

        //Pkg Event
        private void ScPkg_RaiseProposal4(object sender, ProposeEventArgs e)
        {

            NodeMesaage nMsg = e.NMessage;

            if (this.UserRole == MemberRole.NonVotingMember | this.UserRole == MemberRole.MuteMember)
            {
                this.Status = string.Format("Can not initiate voting process.");
                this.EOrig = sender;
                if (this.EOrig != null)
                {
                    (EOrig as IProposalResult).ProcessProposalResult(VoteType.NotInitiated);
                }
                return;
            }

            if (nMsg.ProposedUser.Equals(this.UserName)) //Setting Proposer
            {
                this.IsProposer = true;
                this.EOrig = sender;
            }

            if (!NetworkFunctions.RaiseProposal(this, nMsg))
            {
                MessageBox.Show("Not able to Raise proposal", "Failure");
                return;
            }
            NetworkFunctions.Broadcast(NetworkFunctions.GetRootNodeL(), this.UserName, nMsg);
        }



   [Serializable]
   public class PackageData
   {
       public int ETID { get; set; }

       public string TypeName { get; set; }

       public string PName { get; set; }

       public string MasterPkgName { get; set; }

       public string PkgName { get; set; }

       public string PkgNotes { get; set; }

       public PackageData()
       {
           ETID = 0;
           TypeName = PName = MasterPkgName = PkgName = PkgName = string.Empty;
       }

   }

   [Serializable]
   public class EntityData
   {
       public int ETID { get; set; }

       public string TypeName { get; set; }

       public string FullEntityName { get; set; }

       public string ETName { get; set; }

       public string MasterURIName { get; set; }

       public string ETURIName { get; set; }

       public string ETNotes { get; set; }

       public EntityData()
       {
           ETID = 0;
           FullEntityName = TypeName = ETName = MasterURIName = ETURIName = ETNotes = string.Empty;
       }

   }

   [Serializable]
   public class RelationsData
   {
       public int SourceETID { get; set; }
       public string RelName { get; set; }
       public int TargetETID { get; set; }
       public string RelNotes { get; set; }

       public RelationsData()
       {
           SourceETID = TargetETID = 0;
           RelName = RelNotes = string.Empty;
       }
   }

   [Serializable]
   public class DBData
   {
       public List<PackageData> PkgData { get; set; }

       public List<EntityData> ClassData { get; set; }

       public List<EntityData> PropertyData { get; set; }

       public List<EntityData> EnumData { get; set; }

       public List<RelationsData> RelationData { get; set; }

       public List<DMClass> DmClasses { get; set; }

       public DBData()
       {
           PkgData = new List<PackageData>();
           ClassData = PropertyData = EnumData = new List<EntityData>();
           RelationData = new List<RelationsData>();
           DmClasses = new List<DMClass>();
       }

   }

   [Serializable]
   public class DMDataProperty
   {
       public string BaseURI { get; set; }
       public string PName { get; set; }

       public string PDomain { get; set; }

       public string PRange { get; set; }

       public bool PFunctional { get; set; }

       public string PRangeExpr { get; set; }

       public DMDataProperty()
       {
           BaseURI = PName = PDomain = PRange = PRangeExpr = string.Empty;
           PFunctional = false;
       }
   }

   [Serializable]
   public class DMClass
   {
       public string BaseURI { get; set; }

       public string CName { get; set; }

       public string BaseClassName { get; set; }

       public List<DMDataProperty> DataProperties { get; set; }

       public DMClass()
       {
           BaseURI = string.Empty;
           CName = string.Empty;
           BaseClassName = string.Empty;
           DataProperties = new List<DMDataProperty>();
       }

   }
   #endregion
   */
