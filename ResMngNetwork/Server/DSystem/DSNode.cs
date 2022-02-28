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
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    OWLData oD = nMsg1.OwlData;
                    foreach(OClass oC in oD.OWLClasses)
                    {
                        this.initData.OwlData.OWLClasses.Add(oC);
                    }
                    foreach(ODataProperty oDP in oD.OWLDataProperties)
                    {
                        this.initData.OwlData.OWLDataProperties.Add(oDP);
                    }
                    foreach(OObjectProperty oOP in oD.OWLObjProperties)
                    {
                        this.initData.OwlData.OWLObjProperties.Add(oOP);
                    }
                    foreach(KeyValuePair<string,string> nSP in oD.OWLNameSpaces)
                    {
                        if (!oD.OWLNameSpaces.ContainsKey(nSP.Key))
                            this.initData.OwlData.OWLNameSpaces.Add(nSP.Key, nSP.Value);
                    }
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
                    string expr = nMsg1.DataItems[4];

                    //Start creating Data Property
                    ODataProperty oDp = new ODataProperty();
                    oDp.DProperty = propName;

                    OChildNode ocNode = new OChildNode(); //Domain
                    ocNode.CNBaseURI = "http://www.bristol.ac.uk/sles/v1/opendsst1";
                    ocNode.CNName = selectedCN;
                    ocNode.CNType = "rdfs:domain";
                    oDp.DPChildNodes.Add(ocNode);

                    if (functional) //Add functional
                    {
                        OChildNode ocNode1 = new OChildNode();
                        ocNode1.CNBaseURI = "http://www.w3.org/2002/07/owl";
                        ocNode1.CNName = "FunctionalProperty";
                        ocNode1.CNType = "rdf:type";
                        oDp.DPChildNodes.Add(ocNode1);
                    }

                    if (string.IsNullOrEmpty(expr))
                    {
                        OChildNode ocNode2 = new OChildNode(); //Range
                        ocNode2.CNBaseURI = "http://www.w3.org/2001/XMLSchema";
                        ocNode2.CNName = selectedDT;
                        ocNode2.CNType = "rdfs:range";
                        oDp.DPChildNodes.Add(ocNode2);
                    }
                    else
                    {
                        ValidateRules.CreateChildNodesForExpr(expr, selectedDT, ref oDp);
                        OChildNode ocNode3 = new OChildNode(); //Range
                        ocNode3.CNBaseURI = "http://www.w3.org/2001/XMLSchema";
                        ocNode3.CNName = selectedDT;
                        ocNode3.CNType = "owl:onDatatype";
                        oDp.DPChildNodes.Add(ocNode3);
                    }
                    //Save Data Property to OWLFile
                    this.initData.OwlData.OWLDataProperties.Add(oDp);
                }
                if (nMsg.PCause == ProposalCause.NewObjectProperty)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    bool isEquiv = false;
                    string npName = nMsg1.DataItems[0];
                    
                    string sName = nMsg1.DataItems[1];
                    OClass sC = this.initData.OwlData.OWLClasses.Find((c) => { if (c.CName.Equals(sName)) { return true; } else { return false; } });

                    string tName = nMsg1.DataItems[2];
                    OClass tC = this.initData.OwlData.OWLClasses.Find((c) => { if (c.CName.Equals(tName)) { return true; } else { return false; } });

                    string inverseProp = nMsg1.DataItems[4];

                    if (nMsg1.DataItems[3].ToString().ToLower().Equals("true"))
                        isEquiv = true;

                    OObjectProperty oProp = new OObjectProperty(npName);

                    //Create Child Nodes
                    OChildNode ocNode1 = new OChildNode(); //Domain
                    ocNode1.CNBaseURI = "http://www.bristol.ac.uk/sles/v1/opendsst1";
                    ocNode1.CNName = sC.CName;
                    ocNode1.CNType = "rdfs:domain";
                    oProp.OPChildNodes.Add(ocNode1);

                    OChildNode ocNode2 = new OChildNode(); //Range
                    ocNode2.CNBaseURI = "http://www.bristol.ac.uk/sles/v1/opendsst1";
                    ocNode2.CNName = tC.CName;
                    ocNode2.CNType = "rdfs:range";
                    oProp.OPChildNodes.Add(ocNode2);

                    OChildNode ocNode4 = new OChildNode(); //Functional Prop
                    ocNode4.CNBaseURI = "http://www.bristol.ac.uk/sles/v1/opendsst1";
                    ocNode4.CNName = "FunctionalProperty";
                    ocNode4.CNType = "rdf:type";
                    oProp.OPChildNodes.Add(ocNode4);

                    OChildNode ocNode5 = new OChildNode(); //Inverse Functional Prop
                    ocNode5.CNBaseURI = "http://www.bristol.ac.uk/sles/v1/opendsst1";
                    ocNode5.CNName = "InverseFunctionalProperty";
                    ocNode5.CNType = "rdf:type";
                    oProp.OPChildNodes.Add(ocNode5);

                    OChildNode ocNode6 = new OChildNode(); //ASymmetric
                    ocNode6.CNBaseURI = "http://www.bristol.ac.uk/sles/v1/opendsst1";
                    ocNode6.CNName = "AsymmetricProperty";
                    ocNode6.CNType = "rdf:type";
                    oProp.OPChildNodes.Add(ocNode6);

                    OChildNode ocNode7 = new OChildNode(); //Irreflexive Property
                    ocNode7.CNBaseURI = "http://www.bristol.ac.uk/sles/v1/opendsst1";
                    ocNode7.CNName = "IrreflexiveProperty";
                    ocNode7.CNType = "rdf:type";
                    oProp.OPChildNodes.Add(ocNode7);

                    if (isEquiv)
                    {
                        OChildNode ocNode3 = new OChildNode(); //Equivalent
                        ocNode3.CNBaseURI = "http://www.bristol.ac.uk/sles/v1/opendsst1";
                        ocNode3.CNName = sC.CName;
                        ocNode3.CNType = "owl:equivalentClass";
                        ocNode3.CNSpecialValue = tC.CName;
                        oProp.OPChildNodes.Add(ocNode3);
                    }

                    if(!string.IsNullOrEmpty(inverseProp))
                    {
                        OChildNode ocNode8 = new OChildNode(); //Irreflexive Property
                        ocNode8.CNBaseURI = "http://www.bristol.ac.uk/sles/v1/opendsst1";
                        ocNode8.CNName = inverseProp;
                        ocNode8.CNType = "owl:inverseOf";
                        oProp.OPChildNodes.Add(ocNode8);
                    }

                    //add this to  Object properties collection
                    this.initData.OwlData.OWLObjProperties.Add(oProp);
                    
                }
                if (nMsg.PCause ==  ProposalCause.ModifyDataProperty)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    string newRange = nMsg1.DataItems[0];
                    string newExpr = nMsg1.DataItems[1];
                    ODataProperty oldDP = nMsg1.OldDP;

                    ODataProperty oNDp = this.initData.OwlData.OWLDataProperties.Find((dp) => { if (dp.DProperty.Equals(oldDP.DProperty)) return true; else return false; });
                    ValidateRules.UpdateChildNodesForExpr(newExpr, newRange, ref oNDp);

                    foreach (OChildNode ocNode in oNDp.DPChildNodes)
                    {
                        if(ocNode.CNType.Equals("rdfs:range") || ocNode.CNType.Equals("owl:onDatatype"))
                        {
                            if (!ocNode.CNName.Equals(newRange))
                                ocNode.CNName = newRange;
                        }
                    }
                }
                if(nMsg.PCause == ProposalCause.NewOClass)
                {
                    NodeMesaage nMsg1 = tQueue.Dequeue();
                    string newCls = nMsg1.DataItems[0];

                    OClass oCl = new OClass(newCls);
                    var oc = this.initData.OwlData.OWLClasses.Find((o) => { if (o.CName.Equals(newCls)) { return true; } else { return false; } });

                    if (oc == null)
                    {
                        this.initData.OwlData.OWLClasses.Add(oCl);
                        this.initData.OwlData.DisjointClasses.Add(newCls);
                    }
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
                initData = dSerializer.DeSerialize(@"C:\WorkRelated-Offline\Dist Prog V2\InitialDB\InitialDB.dat");
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
