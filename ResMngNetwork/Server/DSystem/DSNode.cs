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
            vds.Show();
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

            AddNewDtoDP addnewd = new AddNewDtoDP();
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
                }
                return vdSet;
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

        //Event for Propertu Class and PAckage raise proposal
        private void SpWindow_RaiseProposal4(object sender, ProposeEventArgs e)
        {
            NodeMesaage nMsg = e.NMessage;

            if (nMsg.PTYpe == ProposalType.Voting)
            {
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
            else
            {
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
                if(nMsg.PCause == ProposalCause.NewClass)
                {
                    DBInsert.InsertClassEntry(tQueue.Dequeue(), this.initData);
                    this.Status = string.Format("Class details Updated.");
                }
                if(nMsg.PCause == ProposalCause.NewProperty)
                {
                    DBInsert.InsertPropertyEntry(tQueue.Dequeue(), this.initData);
                    this.Status = string.Format("Property details Updated.");
                }
                if(nMsg.PCause == ProposalCause.NewOntology)
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
                if(nMsg.PCause == ProposalCause.NewDataProperty)
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

                    //Start creating Data Property
                    //http://www.bristol.ac.uk/sles/v1/opendsst2 is XML URI
                    SemanticStructure ss = new SemanticStructure() { SSName = propName, SSType = SStrType.DatatypeProperty, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                    if (initData.OwlData.RDFG.AddNode(ss.ToString()))
                        initData.OwlData.RDFG.AddEntryToNODetail(ss.ToString(), ss);
                    //1.
                    SemanticStructure ss2 = new SemanticStructure() { SSName = "FunctionalProperty", SSType = SStrType.Type, XMLURI = "http://www.w3.org/2002/07/owl" };
                    string edKey = string.Format("{0}-{1}", ss.SSName, ss2.SSName);
                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey))
                        initData.OwlData.RDFG.EdgeData[edKey] = ss2.SSType.ToString();
                    //add edge
                    initData.OwlData.RDFG.AddEdge(ss.ToString(), ss2.ToString());

                    //2.
                    SemanticStructure ss3 = new SemanticStructure() { SSName = propName, SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                    string edKey1 = string.Format("{0}-{1}", ss.SSName, selectedCN);
                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                        initData.OwlData.RDFG.EdgeData[edKey1] = ss3.SSType.ToString();
                    //add edge
                    initData.OwlData.RDFG.AddEdge(ss.ToString(), ss3.ToString());

                    //3.
                    SemanticStructure ss4 = new SemanticStructure() { SSName = selectedDT, SSType = SStrType.Range, XMLURI = "http://www.w3.org/2001/XMLSchema" };
                    string edKey2 = string.Format("{0}-{1}", ss.SSName, ss4.SSName);
                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                        initData.OwlData.RDFG.EdgeData[edKey2] = ss3.SSType.ToString();
                    //add edge
                    initData.OwlData.RDFG.AddEdge(ss.ToString(), ss4.ToString());
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
                    if (initData.OwlData.RDFG.AddNode(ss.ToString()))
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
                        string edKey1 = string.Format("{0}-{1}", ss.SSName, ss2.SSName);
                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                            initData.OwlData.RDFG.EdgeData[edKey1] = ss3.SSType.ToString();
                        //add edge
                        initData.OwlData.RDFG.AddEdge(ss.ToString(), ss3.ToString());
                    }
                    //3. Set Domain
                    if (!string.IsNullOrEmpty(sName))
                    {
                        SemanticStructure ss4 = new SemanticStructure() { SSName = sName, SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                        string edKey4 = string.Format("{0}-{1}", ss.SSName, sName);
                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey4))
                            initData.OwlData.RDFG.EdgeData[edKey4] = ss4.SSType.ToString();
                        //add edge
                        initData.OwlData.RDFG.AddEdge(ss.ToString(), ss4.ToString());
                    }

                    //3. Set Range
                    if (!string.IsNullOrEmpty(tName))
                    {
                        SemanticStructure ss5 = new SemanticStructure() { SSName = tName, SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                        string edKey5 = string.Format("{0}-{1}", ss.SSName, tName);
                        if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey5))
                            initData.OwlData.RDFG.EdgeData[edKey5] = ss5.SSType.ToString();
                        //add edge
                        initData.OwlData.RDFG.AddEdge(ss.ToString(), ss5.ToString());
                    }
                }
                if (nMsg.PCause ==  ProposalCause.ModifyDataProperty)
                {
                    //NodeMesaage nMsg1 = tQueue.Dequeue();
                    //string newRange = nMsg1.DataItems[0];
                    //string newExpr = nMsg1.DataItems[1];
                    //ODataProperty oldDP = nMsg1.OldDP;

                    //ODataProperty oNDp = this.initData.OwlData.OWLDataProperties.Find((dp) => { if (dp.DProperty.Equals(oldDP.DProperty)) return true; else return false; });
                    //ValidateRules.UpdateChildNodesForExpr(newExpr, newRange, ref oNDp);

                    //foreach (OChildNode ocNode in oNDp.DPChildNodes)
                    //{
                    //    if(ocNode.CNType.Equals("rdfs:range") || ocNode.CNType.Equals("owl:onDatatype"))
                    //    {
                    //        if (!ocNode.CNName.Equals(newRange))
                    //            ocNode.CNName = newRange;
                    //    }
                    //}
                }
                if(nMsg.PCause == ProposalCause.NewOClass)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    string newCls = nMsg1.DataItems[0];
                    string newBaseCls = nMsg1.DataItems[1];

                    //http://www.bristol.ac.uk/sles/v1/opendsst2 is XML URI
                    SemanticStructure ss = new SemanticStructure() { SSName = newCls, SSType = SStrType.Class, XMLURI = "http://www.bristol.ac.uk/sles/v1/opendsst2" };
                    if (initData.OwlData.RDFG.AddNode(ss.ToString()))
                        initData.OwlData.RDFG.AddEntryToNODetail(ss.ToString(), ss);

                    string edKey = string.Format("{0}-{1}", ss.SSName, newBaseCls);
                    if (!initData.OwlData.RDFG.EdgeData.ContainsKey(edKey))
                        initData.OwlData.RDFG.EdgeData[edKey] = SStrType.SubClassOf.ToString();

                    this.initData.OwlData.RDFG.AddEdge(string.Format("{0}:{1}",newBaseCls,SStrType.Class), newCls);   
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
