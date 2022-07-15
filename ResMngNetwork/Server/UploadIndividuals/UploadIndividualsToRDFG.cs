using ContractDataModels;
using DataSerailizer;
using Server.DSystem;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UoB.ToolUtilities.OpenDSSParser;

namespace Server.UploadIndividuals
{

    /// <summary>
    /// Class to hold data
    /// </summary>
    public class TempDataHolder
    {
        public static List<CircuitEntry> cEntities;

        public TempDataHolder()
        {
            cEntities = new List<CircuitEntry>();
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SubmitPV : ISubmitPVS
    {
                
        public SubmitPV()
        {
        
        }

        bool ISubmitPVS.SubmitPV(List<CircuitEntry> PVSystems)
        {
            TempDataHolder.cEntities = PVSystems;
            return true;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SendPVDetails : ISendPVInfo
    {
        public List<CircuitEntry> SendPVInfo()
        {
            if (TempDataHolder.cEntities == null)
                return null;
            if (TempDataHolder.cEntities.Count == 0)
                return null;
            else
                return TempDataHolder.cEntities;
        }
    }
    
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ObtainSSForInstn: IObtainSSForInst
    {
        public static DBData dbData;

        public ObtainSSForInstn() { }

        public SemanticStructure ObtainSSForInst(string InsName)
        {
            SemanticStructure ss = ObtainSSForInstn.dbData.OwlData.RDFG.GetNodeDetailsW(InsName);
            return ss;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ObtainSSDetails : IObtainSSDetails
    {
        public static DBData dbData;

        public ObtainSSDetails() { }

        public SemanticDetails ObtainSD(string SsName)
        {
            SemanticDetails sDet = new SemanticDetails();

            string actInstName = dbData.OwlData.RDFG.GetExactNodeName(SsName);
            SemanticStructure acss = dbData.OwlData.RDFG.NODetails[actInstName];
            List<string> acoutgoing = dbData.OwlData.RDFG.GetEdgesForNode(actInstName);
            List<string> acincoming = dbData.OwlData.RDFG.GetIncomingEdgesForNode(actInstName);

            if (acss != null)
            {
                sDet.ActialSS = acss;
            }
            if (acoutgoing.Count != 0)
            {
                sDet.Outgoing = acoutgoing; 
            }
            if (acincoming.Count != 0)
            {
                sDet.Incoming = acincoming;
            }

            return sDet;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ObtainAllIndies : IObtainAllIndividuals
    {
        public static DBData dbData = null;

        public ObtainAllIndies()
        {

        }

        /// <summary>
        /// KG API Part to get all individuals from Graph for clients.
        /// </summary>
        /// <returns></returns>
        public List<SemanticStructure> ObtainAllIndividuals()
        {
            List<SemanticStructure> sStrs = new List<SemanticStructure>();
            if (ObtainAllIndies.dbData != null)
            {
                foreach(SemanticStructure sst in ObtainAllIndies.dbData.OwlData.RDFG.NODetails.Values.ToList<SemanticStructure>())
                {
                    if (sst.SSType == SStrType.Instance)
                        sStrs.Add(sst);
                }
            }
            return sStrs;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ObtainAllLoadIndies : IObtainLoadIndividuals
    {
        public static DBData dbData = null;

        public ObtainAllLoadIndies()
        {

        }
        public List<SemanticStructure> ObtainLoadIndividuals()
        {
            List<SemanticStructure> sStrs = new List<SemanticStructure>();
            if (ObtainAllIndies.dbData != null)
            {
                foreach (SemanticStructure sst in ObtainAllIndies.dbData.OwlData.RDFG.NODetails.Values.ToList<SemanticStructure>())
                {
                    if (sst.SSType == SStrType.Instance)
                        sStrs.Add(sst);
                }
            }
            return sStrs;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UploadIndividualsToRDFG : IUploadIndividuals
    {
        public static DBData dbData = null;

        public UploadIndividualsToRDFG()
        {

        }

        /// <summary>
        /// Depricated method. dont bother changes here
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        public List<string> UploadIndividuals(List<CircuitEntry> entries)
        {
            List<string> results = new List<string>();
            foreach (CircuitEntry ce in entries)
            {
                try
                {
                    string nName = UploadIndividualsToRDFG.dbData.OwlData.RDFG.GetExactNodeName(ce.CEType);
                    if (!string.IsNullOrEmpty(nName))
                    {
                        //Step1: Add an Instance using ce.CEName 

                        SemanticStructure sc = new SemanticStructure() { SSName = ce.CEName, SSType = SStrType.Instance, XMLURI = string.Empty };

                        if (UploadIndividualsToRDFG.dbData.OwlData.RDFG.AddNode(sc.ToString()))
                            UploadIndividualsToRDFG.dbData.OwlData.RDFG.AddEntryToNODetail(sc.ToString(), sc);

                        SemanticStructure ss = UploadIndividualsToRDFG.dbData.OwlData.RDFG.NODetails[nName];

                        string edKey = string.Format("{0}-{1}", ss.SSName, sc.SSName);
                        if (!UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey))
                            UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData[edKey] = sc.SSType.ToString();

                        //Step2: Get outgoing and incoming edges
                        List<string> outgoing = UploadIndividualsToRDFG.dbData.OwlData.RDFG.GetEdgesForNode(nName);
                        List<string> incoming = UploadIndividualsToRDFG.dbData.OwlData.RDFG.GetIncomingEdgesForNode(nName);
                        List<string> classToBeUsed = new List<string>();
                        outgoing.Sort();
                        int counter = 0;
                        while (counter <= outgoing.Count - 1)
                        {
                            string ots = outgoing[counter];
                            string[] reqParts = ots.Split('-')[1].Split(':');
                            if (reqParts[1].ToLower().Equals("class"))
                            {
                                //Step3: Calculating data properties up in hierarchies 
                                //Seems like not required to get all properties
                                #region Good Recursive Code for getting up hierarchies
                                //string relation = string.Empty;
                                //if (UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData.ContainsKey(string.Format("{0}-{1}", nName.Split(':')[0], reqParts[0])))
                                //    relation = UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData[string.Format("{0}-{1}", nName.Split(':')[0], reqParts[0])];

                                ////if sub class I want to add all data properties gathered from all base classes till owl:Thing
                                //if (relation.ToLower().Equals("subclassof"))
                                //{
                                //    string source = ots.Split('-')[1];
                                //    while (!source.ToLower().Equals("owl:thing"))
                                //    {
                                //        List<string> og = UploadIndividualsToRDFG.dbData.OwlData.RDFG.GetEdgesForNode(source);
                                //        List<string> ic = UploadIndividualsToRDFG.dbData.OwlData.RDFG.GetIncomingEdgesForNode(source);
                                //        foreach (string ics in ic)
                                //        {
                                //            if (ics.Split(':')[1].Equals("DatatypeProperty"))
                                //                incoming.Add(ics);
                                //        }
                                //        foreach (string sst in og) //must be one only
                                //            source = sst.Split('-')[1];
                                //    }
                                //}
                                #endregion
                            }
                            else if (reqParts[1].ToLower().Equals("objectproperty"))
                            {
                                //Step4: Calculate restrictions
                                classToBeUsed.Add(ots.Split('-')[1]);
                                while (true)
                                {
                                    if ((counter + 1) > outgoing.Count - 1)
                                        break;
                                    string tots = outgoing[++counter];

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

                                    SemanticStructure ssN = UploadIndividualsToRDFG.dbData.OwlData.RDFG.NODetails[s];

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

                                        if (UploadIndividualsToRDFG.dbData.OwlData.RDFG.AddNode(scc.ToString()))
                                            UploadIndividualsToRDFG.dbData.OwlData.RDFG.AddEntryToNODetail(scc.ToString(), scc);

                                        string edKey1 = string.Format("{0}-{1}", ssN.SSName, scc.SSName);
                                        if (!UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                                            UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData[edKey1] = scc.SSType.ToString();

                                        string edKey2 = string.Format("{0}-{1}", sc.SSName, scc.SSName);
                                        if (!UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                                            UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData[edKey2] = scc.SSType.ToString();
                                    }
                                    else
                                    {
                                        int insCounter = 1;
                                        foreach (string vl in values)
                                        {
                                            SemanticStructure scc = new SemanticStructure() { SSName = string.Format("inst_{0}_{1}:{2}", s.Split(':')[0], insCounter, s.Split(':')[1]), SSType = SStrType.Instance, XMLURI = vl };

                                            if (UploadIndividualsToRDFG.dbData.OwlData.RDFG.AddNode(scc.ToString()))
                                                UploadIndividualsToRDFG.dbData.OwlData.RDFG.AddEntryToNODetail(scc.ToString(), scc);

                                            string edKey1 = string.Format("{0}-{1}", ssN.SSName, scc.SSName);
                                            if (!UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                                                UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData[edKey1] = scc.SSType.ToString();

                                            string edKey2 = string.Format("{0}-{1}", sc.SSName, scc.SSName);
                                            if (!UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                                                UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData[edKey2] = scc.SSType.ToString();
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
                        //Step5: Add nodes and edges for data properties using ce.CEEntries
                        foreach (string ins in incoming)
                        {
                            SemanticStructure ssN = UploadIndividualsToRDFG.dbData.OwlData.RDFG.NODetails[ins];

                            //Get value from CEEntries
                            string value = string.Empty;
                            foreach (string ces in ce.CEEntries)
                            {
                                if (ces.ToLower().Split('=')[0].Contains(ins.Split(':')[0].ToLower()))
                                {
                                    value = ces.Split('=')[1];
                                }
                            }

                            SemanticStructure scc = new SemanticStructure() { SSName = string.Format("inst_{0}", ins), SSType = SStrType.Instance, XMLURI = value };

                            if (UploadIndividualsToRDFG.dbData.OwlData.RDFG.AddNode(scc.ToString()))
                                UploadIndividualsToRDFG.dbData.OwlData.RDFG.AddEntryToNODetail(scc.ToString(), scc);

                            string edKey1 = string.Format("{0}-{1}", ssN.SSName, scc.SSName);
                            if (!UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey1))
                                UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData[edKey1] = scc.SSType.ToString();

                            string edKey2 = string.Format("{0}-{1}", sc.SSName, scc.SSName);
                            if (!UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData.ContainsKey(edKey2))
                                UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData[edKey2] = scc.SSType.ToString();
                        }
                    }
                }
                catch(Exception ex)
                {
                    string s = ex.Message + ce.CEName;
                }
            }

            //Creating Files for Neo4J
            List<string> cins = new List<string>();
            foreach (KeyValuePair<string, SemanticStructure> kvp in UploadIndividualsToRDFG.dbData.OwlData.RDFG.NODetails)
            {
                string s1 = kvp.Key.Split(':')[0].Replace("/", "_").Replace("#", "");
                if (char.IsDigit(s1[0]))
                    s1 = string.Format("A{0}", s1);

                cins.Add(string.Format("CREATE ({0}:SemanticStructure {{SSName:'{1}', SSType:'{2}'}})", s1, kvp.Value.SSName, kvp.Value.SSType));
            }

            foreach (KeyValuePair<string, string> ked in UploadIndividualsToRDFG.dbData.OwlData.RDFG.EdgeData)
            {
                string s1 = ked.Key.Split('-')[0].Replace("/", "_").Replace("#", "");
                if (char.IsDigit(s1[0]))
                    s1 = string.Format("A{0}", s1);

                string s2 = ked.Key.Split('-')[1].Replace("/", "_").Replace("#", "");
                if (char.IsDigit(s2[0]))
                    s2 = string.Format("A{0}", s2);
                if (s2.Contains(':'))
                    s2 = s2.Split(':')[0];

                cins.Add(string.Format("CREATE ({0})-[:{2}]->({1})",s1, s2, ked.Value));
            }
            System.IO.File.WriteAllLines(@"C:\WorkRelated-Offline\Dist_Prog_V2\Tools\neo4jexps\WriteNodesKG.txt", cins);

            return results;
        }

        public static string GetNodeNameForParam(string pName)
        {
            if (pName.Contains("R"))
                return "Resistance:Class";
            else if (pName.Contains("X"))
                return "Reatance.Class";

            return pName;

        }

        public static string GetParamNameForNode(string pName)
        {
            if (pName.Contains("Resistance"))
                return "R";
            else if (pName.Contains("Reatance"))
                return "X";
            else if (pName.ToLower().Contains("bus"))
                return "bus";
            return pName;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class AddNewUserRole : IAddNewUserRole, IProposalResult, ITransitionResult
    {
        public static DSNode dsn = null;
        bool pResult;
        public event ProposeResultEventHandler PRHandler;
        public event TransitResultEventHandler TRHandler;
        ISendAddNewUserResult callbackChannel;
        public AddNewUserRole()
        {
            pResult = false;
            this.PRHandler += AddNewUserRole_PRHandler;
            this.TRHandler += AddNewUserRole_TRHandler;
        }

        private bool AddNewUserRole_TRHandler(object sender, TransitResultEventArgs e)
        {
            bool tResult = e.TResult;
            callbackChannel.SendAddUserResult(tResult);
            return tResult;
        }

        private bool AddNewUserRole_PRHandler(object sender, ProposeResultEventArgs e)
        {
            this.pResult = e.PResult;
            if(!pResult)
             callbackChannel.SendAddUserResult(pResult); //Return when approval not obtained.
            return pResult;
        }
        string uN;
        public void AddNewUser(string UName)
        {
            this.uN = UName;
            callbackChannel = OperationContext.Current.GetCallbackChannel<ISendAddNewUserResult>();
            if (dsn != null)
            {               
                dsn.SplPower = true;

                NodeMesaage nMessage = new NodeMesaage();
                nMessage.ProposedUser = AddNewUserRole.dsn.UserName;
                nMessage.PCause = ProposalCause.NewOClass;
                nMessage.PTYpe = ProposalType.Voting;
                List<string> sItems = new List<string>();
                sItems.Add("Users");
                sItems.Add(UName);
                nMessage.DataItems = sItems;
                dsn.PANUserCommand.Propose(this, nMessage);
                
            }
        }
        public void ProcessProposalResult(VoteType overAllType)
        {
            if (overAllType == VoteType.Accepted)
            {
                //PRHandler?.Invoke(this, new ProposeResultEventArgs() { PResult = true });
                //Proposal Accepted and we are starting new transition
                NodeMesaage nMessage = new NodeMesaage();
                nMessage.ProposedUser = AddNewUserRole.dsn.UserName;
                nMessage.PCause = ProposalCause.NewOClass;
                nMessage.PTYpe = ProposalType.Transition;
                List<string> sItems = new List<string>();
                sItems.Add(this.uN);
                sItems.Add("Users");
                nMessage.DataItems = sItems;
                dsn.PANUserCommand.Transit(this, nMessage); //Call transition
            }
            else
            {
                PRHandler?.Invoke(this, new ProposeResultEventArgs() { PResult = false });
            }
        }

        public void ProcessTransitResult(TransitType tType)
        {
            if (tType == TransitType.Done)
            {
                TRHandler?.Invoke(this, new TransitResultEventArgs() { TResult = true });
            }
            else
            {
                TRHandler?.Invoke(this, new TransitResultEventArgs() { TResult = false });
            }
        }
    }

    public class ProposeANewUser  
    {
        public event RaiseProposeEventHandler RaiseProposal4;
        
        public ProposeANewUser()
        {
        }

        public void Propose(AddNewUserRole adRole, NodeMesaage nMsg)
        {
            RaiseProposal4?.Invoke(adRole, new ProposeEventArgs() { NMessage = nMsg });
        }

        public void Transit(AddNewUserRole adRole, NodeMesaage nMsg)
        {
            RaiseProposal4?.Invoke(adRole, new ProposeEventArgs() { NMessage = nMsg });
        }
            
    }
}
