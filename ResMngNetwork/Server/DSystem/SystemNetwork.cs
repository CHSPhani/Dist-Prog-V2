using Server.ChangeRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using DataSerailizer;
using UoB.ToolUtilities.OpenDSSParser;
using Server.Models;

namespace Server.DSystem
{
    public interface ITransitionResult
    {
        void ProcessTransitResult(TransitType tType);
    }
   
    public interface IProposalResult
    {
        void ProcessProposalResult(VoteType overAllType);
    }
    public class NodeMesaage
    {
        public string ProposedUser { get; set; }

        public ProposalType PTYpe { get; set; }

        public ProposalCause PCause { get; set; }

        public List<string> DataItems { get; set; }

        public OWLDataG OwlDatag { get; set; }
        
        public ODataProperty OldDP { get; set; }

        public List<CircuitEntry> CEntities { get; set; }

        public OntoModificationModel OModel { get; set; }

        public UInstEntry UIEntry { get; set; }

        public VDSetModel VDSModel { get; set; }

        public bool DirectTransit { get; set; }

        public NodeMesaage()
        {
            DirectTransit = false;
            ProposedUser = string.Empty;
            PTYpe = ProposalType.None;
            PCause = ProposalCause.None;
            DataItems = new List<string>();
            OwlDatag = new OWLDataG();
            CEntities = new List<CircuitEntry>();
            OldDP = null;
            OModel = null;
            UIEntry = null;
        }
    }

    public enum TransitType
    {
        Done,
        Failed,
        NA
    }
    public enum VoteType
    {
        Accepted,
        Abort,
        NotAccepted,
        NoObjection,
        NA,
        NotInitiated
    }

    public class NodeVote
    {
        public VoteType Vote { get; set; }

        public NodeVote()
        {
            Vote =  VoteType.NA;
        }
    }

    public class CoreMessage
    {
        public DSNode Sender { get; set; }

        public MessageType MType { get; set; }

        public ProposalCause PCause { get; set; }

        public ProposalType PType { get; set; }

        public DSNode Receiver { get; set; }

        public CoreMessage() { }

        public override string ToString()
        {
            return string.Format("{0}.{1}({2},{3})",Sender.UserName,MType,PType,Receiver.UserName);
        }

    }
    /// <summary>
    /// Just to be used by OrderMessageByNode function only
    /// </summary>
    public class MessageSet
    {
        public List<DSNode> InNodes { get; set; }
        public List<DSNode> OutNodes { get; set; }

        public MessageSet()
        {
            InNodes = new List<DSNode>();
            OutNodes = new List<DSNode>();
        }

        public MessageSet(DSNode dNode, MessageType mType) : this()
        {
            switch (mType)
            {
                case MessageType.inwards:
                    {
                        InNodes.Add(dNode);
                        break;
                    }
                case MessageType.outwards:
                    {
                        OutNodes.Add(dNode);
                        break;
                    }
            }
        }
    }

    public class NetworkFunctions
    {
        /// <summary>
        /// This is a class where the  following are created in a hardcoded way, because this is a PoC
        /// 3. The BFS Graph, Broadcase and Convergecase are done
        /// </summary>
        public NetworkFunctions()
        { }

        static NetworkFunctions()
        {
            SpTree = null;
        }

        public static SpanningTree<DSNode> SpTree;
        /// <summary>
        /// A proposal is beign raised in the network
        /// </summary>
        /// <returns></returns>
        public static bool RaiseProposal(DSNode proposer, NodeMesaage nMsg)
        {
            try
            {
                List<CoreMessage> coreMsgs = NetworkFunctions.ConstructMessages(proposer, nMsg.PCause, nMsg.PTYpe);
                Dictionary<DSNode, MessageSet> tsMGs = NetworkFunctions.OrderMessageByNode(coreMsgs);
                NetworkFunctions.SpTree = NetworkFunctions.ConstructSpanTreeByBFS(proposer, tsMGs);

                //Just for Checking..
                string trString = string.Empty;
                List<SpanningTreeNode<DSNode>> sptList = new List<SpanningTreeNode<DSNode>>();
                sptList.Add(SpTree.GetRootNode());

                SpTree.Traverse(sptList, ref trString);
                string tData = trString;
                Console.WriteLine(tData);
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Represents Network of System 
        /// Connected Graph Topology and Network
        /// incidents are created for each DSNode. HArdcoded as this is a PoC Again.
        /// </summary>
        /// <param name="nodes"></param>
        public static void CreateIncidents(ref List<DSNode> nodes)
        {
            foreach (DSNode dsNode in nodes)
            {
                if (dsNode.UserName == "User1")
                {
                    dsNode.Incidents.Add(nodes.Find((n) => {
                        if (n.UserName == "User2")
                            return true;
                        else
                            return false;
                    }));
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "User3")
                            return true;
                        else
                            return false;
                    }));
                }
                if (dsNode.UserName == "User2")
                {
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "User1")
                            return true;
                        else
                            return false;
                    }));
                    dsNode.Incidents.Add(nodes.Find((n) => {
                        if (n.UserName == "User3")
                            return true;
                        else
                            return false;
                    }));
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "User4")
                            return true;
                        else
                            return false;
                    }));
                }
                if (dsNode.UserName == "User3")
                {
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "User1")
                            return true;
                        else
                            return false;
                    }));
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "User2")
                            return true;
                        else
                            return false;
                    }));
                    dsNode.Incidents.Add(nodes.Find((n) => {
                        if (n.UserName == "User5")
                            return true;
                        else
                            return false;
                    }));
                }
                if (dsNode.UserName == "User4")
                {
                    dsNode.Incidents.Add(nodes.Find((n) => {
                        if (n.UserName == "User5")
                            return true;
                        else
                            return false;
                    }));
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "User2")
                            return true;
                        else
                            return false;
                    }));
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "AUser")
                            return true;
                        else
                            return false;
                    }));
                }
                if (dsNode.UserName == "User5")
                {
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "User4")
                            return true;
                        else
                            return false;
                    }));
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "User3")
                            return true;
                        else
                            return false;
                    }));
                }
                if(dsNode.UserName == "AUser")
                {
                    dsNode.Incidents.Add(nodes.Find((n) =>
                    {
                        if (n.UserName == "User4")
                            return true;
                        else
                            return false;
                    }));
                }
            }
        }

        /// <summary>
        /// Depending the Incidents of a node messages are simualted. 
        /// </summary>
        /// <param name="proposer"></param>
        /// <param name="pCause"></param>
        /// <param name="pType"></param>
        /// <returns></returns>
        public static List<CoreMessage> ConstructMessages(DSNode proposer, ProposalCause pCause, ProposalType pType)
        {
            List<DSNode> nodesToBe = new List<DSNode>();
            List<DSNode> visitedNodes = new List<DSNode>();
            List<DSNode> tempNodes = new List<DSNode>();
            List<CoreMessage> cMessage = new List<CoreMessage>();
            nodesToBe.Add(proposer);
            int counter = 0;
            while(counter < nodesToBe.Count)
            {
                DSNode dNode = nodesToBe[counter];
                var alreadyVisited = visitedNodes.Find((n) => {
                    if (n.UserName == dNode.UserName)
                        return true;
                    else
                        return false;
                });
                if ((alreadyVisited as DSNode) != null)
                {
                    //DSNode already Visited;
                    ++counter;
                    continue;
                }
                else //Treat Node
                {
                    foreach (DSNode inNode in dNode.Incidents)
                    {
                        CoreMessage oMsg = new CoreMessage() { Sender = dNode, MType = MessageType.outwards, PCause = pCause, PType = pType, Receiver = inNode };
                        CoreMessage iMsg = new CoreMessage() { Sender = inNode, MType = MessageType.inwards, PCause = pCause, PType = pType, Receiver = dNode };
                        cMessage.Add(oMsg);
                        cMessage.Add(iMsg);
                        tempNodes.Add(inNode);
                    }
                    visitedNodes.Add(dNode);
                    //nodesToBe.Clear();
                    foreach (DSNode tN in tempNodes)
                        nodesToBe.Add(tN);
                    tempNodes.Clear();
                }
                ++counter;
            }
            return cMessage;
        }

        
        /// <summary>
        /// ORder message by DSNode so that creating span treecan be done by selecting messages
        /// </summary>
        /// <param name="proposer"></param>
        /// <param name="coreMsgs"></param>
        public static Dictionary<DSNode, MessageSet> OrderMessageByNode(List<CoreMessage> coreMsgs)
        {
            Dictionary<DSNode, MessageSet> tsMsgs = new Dictionary<DSNode, MessageSet>();
            foreach(CoreMessage cMessage in coreMsgs)
            {
                if (tsMsgs.ContainsKey(cMessage.Sender))
                {
                    switch (cMessage.MType)
                    {
                        case MessageType.inwards:
                            {
                                tsMsgs[cMessage.Sender].InNodes.Add(cMessage.Receiver);
                                break;
                            }
                        case MessageType.outwards:
                            {
                                tsMsgs[cMessage.Sender].OutNodes.Add(cMessage.Receiver);
                                break;
                            }
                    }
                }
                else
                {
                    switch (cMessage.MType)
                    {
                        case MessageType.inwards:
                            {
                                tsMsgs.Add(cMessage.Sender, new MessageSet(cMessage.Receiver,MessageType.inwards));
                                break;
                            }
                        case MessageType.outwards:
                            {
                                tsMsgs.Add(cMessage.Sender, new MessageSet(cMessage.Receiver, MessageType.outwards));
                                break;
                            }
                    }
                }
            }
            return tsMsgs;
        }

        /// <summary>
        /// Construct BFS based on messages. AS per Algorithm
        /// </summary>
        /// <param name="coreMsg"></param>
        public static SpanningTree<DSNode> ConstructSpanTreeByBFS(DSNode proposer, Dictionary<DSNode, MessageSet> tsMsgs)
        {
            SpanningTree<DSNode> sTree = null;
            List<DSNode> visitedNodes = new List<DSNode>();

            //Starting with Proposer node
            //Cant we get Proposer by the fact that no incoming messages? No 
            //Why? The message set generated like P2P setup where all nodes are sending message to all nodes based on Network.
            if(tsMsgs.ContainsKey(proposer))
            {
                sTree = new SpanningTree<DSNode>(proposer);
                visitedNodes.Add(proposer);
                MessageSet mSet = tsMsgs[proposer];
                
                //process outward message igonore inward because we are 
                foreach (DSNode outNode in mSet.OutNodes)
                {
                    sTree.GetRootNode().AddChild(outNode);
                    //sTree.AddChildNode(sTree.GetRootNode(), outNode); //parent message
                }
            }
            else
            {
                //Root node in in messages?? what to do??
                return null;
            }
            List<SpanningTreeNode<DSNode>> sptList = null;
            //Treat Other Nodes as we added root node
            foreach (DSNode node in tsMsgs.Keys)
            {
                if (visitedNodes.Contains(node))
                    continue;

                MessageSet mSet = tsMsgs[node];
                
                sptList = new List<SpanningTreeNode<DSNode>>();
                sptList.Add(sTree.GetRootNode());
                SpanningTreeNode<DSNode> posNode = null;
                sTree.FindInChildren(sptList, node, ref posNode);
                
                List<SpanningTreeNode<DSNode>> nodesTillHead = sTree.NodesTillHead(posNode);
                List<string> nodeNames = new List<string>();
                foreach(SpanningTreeNode<DSNode> ntHead in nodesTillHead)
                {
                    nodeNames.Add(ntHead.NodeData.UserName);
                }
                
                foreach (DSNode inNode in mSet.InNodes)
                {
                    if (nodeNames.Contains(inNode.UserName)) //aleady message
                        break;
                }

                foreach (DSNode outNode in mSet.OutNodes)
                {
                    if (nodeNames.Contains(outNode.UserName)) //aleady message
                        continue;
                    SpanningTreeNode<DSNode> teNode = null;
                    sTree.FindInChildren(sptList, outNode,ref teNode);
                    if (teNode == null)
                    {
                        SpanningTreeNode<DSNode> tpNode = null;
                        sTree.FindInChildren(sptList, node, ref tpNode);
                        tpNode.AddChild(outNode); //parent messagee
                    }
                }
                nodeNames.Clear();
                visitedNodes.Add(node);
            }
            return sTree;
        }

        public static List<SpanningTreeNode<DSNode>> GetRootNodeL()
        {
            List<SpanningTreeNode<DSNode>> sptList = new List<SpanningTreeNode<DSNode>>();
            sptList.Add(SpTree.GetRootNode());
            return sptList;
        }

        public static void Broadcast(List<SpanningTreeNode<DSNode>> sptList, string proposer, NodeMesaage nMsg)
        {
            foreach (SpanningTreeNode<DSNode> dNode in sptList)
            {
                dNode.NodeData.NetworkMessage(proposer, nMsg);
                //Boradcasting mesg
                Broadcast(dNode.Childeren, proposer, nMsg);
            }
        }

        public static void TransitState(List<SpanningTreeNode<DSNode>> sptList, string proposer, NodeMesaage nMsg)
        {
            foreach (SpanningTreeNode<DSNode> dNode in sptList)
            {
                dNode.NodeData.TransitState(proposer, nMsg);
                //Boradcasting mesg
                TransitState(dNode.Childeren, proposer, nMsg);
            }
        }
        


        static System.Timers.Timer timer1;
        static TCEventArgs TCEArgs = new TCEventArgs();
        public static void CollectTransition(SpanningTree<DSNode> sTree, DSNode proposer)
        {
            if (timer1 == null)
            {
                timer1 = new System.Timers.Timer(5000);
                timer1.Elapsed += Timer1_Elapsed; ;
                timer1.AutoReset = false;
                timer1.Enabled = true;
            }
            if (proposer.IsProposer)
            {
                NetworkFunctions.TCEArgs.proposer = proposer.UserName;
                proposer.Status = "Transition Done";
            }
            List<SpanningTreeNode<DSNode>> sptList = new List<SpanningTreeNode<DSNode>>();
            sptList.Add(sTree.GetRootNode());

            SpanningTreeNode<DSNode> vrNode = null;
            sTree.FindInChildren(sptList, proposer, ref vrNode);

            vrNode.NodeData.TransitResult = "TransitDone";
        }
        public static event StartCCEventHandler TCHandler;
        private static void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
            timer1 = null;
            //Raise Converge cast start event
            TCHandler(new object(), NetworkFunctions.TCEArgs);
        }

        static System.Timers.Timer timer;
        static CCEventArgs CCEArgs = new CCEventArgs();
        public static void CollectVoltes(SpanningTree<DSNode> sTree, DSNode proposer, NodeVote nVote)
        {
            if (timer == null)
            {
                timer = new System.Timers.Timer(5000);
                timer.Elapsed += Timer_Elapsed;
                timer.AutoReset = false;
                timer.Enabled = true;
            }
            if (proposer.IsProposer)
            {
                NetworkFunctions.CCEArgs.proposer = proposer.UserName;
                proposer.Status = "Collecting Votes from OTher Nodes. Waiting for 5 seconds for all nodes to contribute";
            }

            List<SpanningTreeNode<DSNode>> sptList = new List<SpanningTreeNode<DSNode>>();
            sptList.Add(sTree.GetRootNode());

            SpanningTreeNode<DSNode> vrNode = null;
            sTree.FindInChildren(sptList, proposer, ref vrNode);

            vrNode.NodeData.VoteResult = nVote.Vote.ToString();
        }

        public static event StartCCEventHandler CCEHandler;
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            timer.Stop();
            timer = null;
            //Raise Converge cast start event
            CCEHandler(new object(), NetworkFunctions.CCEArgs);

        }

        public static void GetLeafNodes(List<SpanningTreeNode<DSNode>> sptList, ref List<SpanningTreeNode<DSNode>> leafNodes)
        {
            foreach (SpanningTreeNode<DSNode> dNode in sptList)
            {
                if (dNode.Childeren.Count == 0)
                    leafNodes.Add(dNode);
                GetLeafNodes(dNode.Childeren, ref leafNodes);
            }
        }
        /// <summary>
        /// Converge case takes votes from leaf nodes and propagates to root node.
        /// </summary>
        /// <param name="sptList"></param>
        /// <returns></returns>
        public static string ConvergeCast(List<SpanningTreeNode<DSNode>> sptList)
        {
            List<SpanningTreeNode<DSNode>> lNodes = new List<SpanningTreeNode<DSNode>>();
            GetLeafNodes(sptList, ref lNodes);
            int counter = 0;
            while(counter < lNodes.Count)
            {
                SpanningTreeNode<DSNode> curNode = lNodes[counter];
                while(curNode.HeadNode != null)
                {
                    curNode.HeadNode.NodeData.VoteResult += string.Format(">{0}",curNode.NodeData.VoteResult);
                    curNode = curNode.HeadNode;
                }
                counter++;
            }
            string allVotes = SpTree.GetRootNode().NodeData.VoteResult;
            return allVotes;
        }

        /// <summary>
        /// PRocessing all votes at a place.
        /// </summary>
        /// <param name="allVotes"></param>
        /// <returns></returns>
        public static VoteType ProcessAllVotes(string allVotes)
        {
            Dictionary<string, int> dVotes = new Dictionary<string, int>();
            int totalVotes = 0;
            allVotes = allVotes.Replace(">>", ">");
            string[] votes = allVotes.Split('>');
            foreach(string v in votes)
            {
                if (dVotes.ContainsKey(v))
                    dVotes[v]++;
                else
                    dVotes[v] = 1;
                totalVotes++;
            }
            if (dVotes.ContainsKey(VoteType.Abort.ToString()))
                if (dVotes[VoteType.Abort.ToString()] > 0)
                    return VoteType.NotAccepted;
            VoteType finalVote;
            string fv=string.Empty;
            foreach(KeyValuePair<string,int> kv in dVotes)
            {
                if (kv.Value > (totalVotes / 2))
                    fv = kv.Key;
            }
            switch(fv.ToLower())
            {
                case "accepted":
                    {
                        finalVote = VoteType.Accepted;
                        break;
                    }
                case "notaccepted":
                    {
                        finalVote = VoteType.NotAccepted;
                        break;
                    }
                default:
                    {
                        finalVote = VoteType.NA;
                        break;
                    }
            }
            return finalVote;
        }
    }
    public class TCEventArgs : EventArgs
    {
        public string proposer;

        public TCEventArgs()
        {
            proposer = string.Empty;
        }
    }
    public class CCEventArgs:EventArgs
    {
        public string proposer;

        public CCEventArgs()
        {
            proposer = string.Empty;
        }
    }
    public delegate void StartCCEventHandler(object sender, EventArgs e);
    public class SpanningTreeNode<T>
    {
        private readonly T _data;
        private readonly List<SpanningTreeNode<T>> _children;
        private SpanningTreeNode<T> _parent;
        public SpanningTreeNode(T data)
        {
            _data = data;
            _children = new List<SpanningTreeNode<T>>();
            _parent = null;
        }

        public bool AddChild(T data)
        {
            if (data == null)
                return false;
            if (_children == null)
                return false;
            SpanningTreeNode<T> cNode = new SpanningTreeNode<T>(data);
            _children.Add(cNode);
            cNode._parent = this;
            return true;
        }
        public T NodeData
        {
            get
            {
                return _data;
            }
        }
        public SpanningTreeNode<T> HeadNode
        {
            get
            {
                return _parent;
            }
        }

        public List<SpanningTreeNode<T>> Childeren
        {
            get
            {
                return this._children;
            }
        }

    }

    public class SpanningTree<T>
    {
        SpanningTreeNode<T> rootNode = null;
        public SpanningTree(T data)
        {
            if (rootNode == null)
                rootNode = new SpanningTreeNode<T>(data);
        }

        public SpanningTreeNode<T> GetRootNode()
        {
            if (rootNode == null)
                return null;
            return rootNode;
        }

        public void FindInChildren(List<SpanningTreeNode<T>> dList, T data, ref SpanningTreeNode<T> foundNode)
        {
            foreach(SpanningTreeNode<T> dNode in dList)
            {
                if(dNode.NodeData.Equals(data))
                {
                    foundNode = dNode;
                }
                else
                {
                    FindInChildren(dNode.Childeren, data, ref foundNode);
                }
            }
        }
        
        public void Traverse(List<SpanningTreeNode<T>> dList, ref string trString)
        {
            foreach (SpanningTreeNode<T> dNode in dList)
            {
                trString += dNode.NodeData.ToString();
                Traverse(dNode.Childeren,ref trString);
            }
        }

        public List<SpanningTreeNode<T>> NodesTillHead(SpanningTreeNode<T> node)
        {
            List<SpanningTreeNode<T>> nodeList = new List<SpanningTreeNode<T>>();
            while (node.HeadNode != null)
            {
                nodeList.Add(node.HeadNode);
                node = node.HeadNode;
            }
            return nodeList;
        }

    }   
}

#region OldCode
/*
 * 
        public string Tranverse()
        {
            StringBuilder sbuilder = new StringBuilder();
            SpanningTreeNode<T> tempNode = rootNode;
            sbuilder.Append(tempNode.NodeData.ToString()+";");
            List<SpanningTreeNode<T>> childNodes = rootNode.Childeren;
            int counter = 0;
            while (counter < childNodes.Count)
            {
                sbuilder.Append(childNodes[counter].NodeData.ToString() + ";");
                foreach (SpanningTreeNode<T> cN in childNodes[counter].Childeren)
                    childNodes.Add(cN);
                counter++;
            }
            return sbuilder.ToString();
        }

        public SpanningTreeNode<T> FindInChildren(T data)
        {
            SpanningTreeNode<T> tempNode = rootNode;
            if (rootNode.Equals(data))
                return rootNode;
            List<SpanningTreeNode<T>> childNodes = rootNode.Childeren;
            int counter = 0;
            while (counter < childNodes.Count)
            {
                if (childNodes[counter].NodeData.Equals(data))
                    return childNodes[counter];
                else
                    foreach(SpanningTreeNode<T> cN in childNodes[counter].Childeren)
                        childNodes.Add(cN);
                counter++;
            }
            return null;
        }

     
        public void AddChildNode(SpanningTreeNode<T> parentNode, T data)
        {
            if (rootNode.NodeData.Equals(parentNode.NodeData))
            {
                rootNode.AddChild(data);
                return;
            }
            if (rootNode.Childeren.Count == 0)
                return;
            else
                foreach (SpanningTreeNode<T> child in rootNode.Childeren)
                    AddChildNode(child, data);
        }

 * 
 * */
#endregion
