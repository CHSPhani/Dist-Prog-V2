using DataSerailizer;
using Server.ChangeRules;
using Server.DSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Server.Models
{
    public class SaveOPCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event EventHandler EventCompleted;

        public event RaiseProposeEventHandler RaisePropose;
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p0, p1, p2, p3, p4, p5, p6 = string.Empty;
            ODataProperty oldDP = null;


            if (values[0] != null)
                p0 = values[0].ToString();
            else
                p0 = string.Empty;

            if (values[1] != null)
                p1 = values[1].ToString();
            else
                p1 = string.Empty;

            if (values[2] != null)
                p2 = values[2].ToString();
            else
                p2 = string.Empty;

            if (values[3] != null)
                p3 = values[3].ToString();
            else
                p3 = string.Empty;

            if (values[4] != null)
                p4 = values[4].ToString();
            else
                p4 = string.Empty;

            if (values[5] != null)
                p5 = values[5].ToString();
            else
                p5 = string.Empty;

            if (values[4] != null)
                oldDP = values[4] as ODataProperty;
            else
                oldDP = null;

            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
            else if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Class Name" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Name" });
                return;
            }

            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Range" });
                return;
            }

            else if (string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Expression" });
                return;
            }

            //else if (string.IsNullOrEmpty(p4))
            //{
            //    EventHandler handler = EventCompleted;
            //    handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Old Domain Name" });
            //    return;
            //}

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.ModifyDataProperty;
            nMessage.PTYpe = ProposalType.Transition;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4); sItems.Add(p5);
            nMessage.OldDP = oldDP;
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }
    public class ProposeOPCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event EventHandler EventCompleted;

        public event RaiseProposeEventHandler RaisePropose;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p0, p1, p2, p3, p4, p5, p6 = string.Empty;
            ODataProperty oldDP = null;


            if (values[0] != null)
                p0 = values[0].ToString();
            else
                p0 = string.Empty;

            if (values[1] != null)
                p1 = values[1].ToString();
            else
                p1 = string.Empty;

            if (values[2] != null)
                p2 = values[2].ToString();
            else
                p2 = string.Empty;

            if (values[3] != null)
                p3 = values[3].ToString();
            else
                p3 = string.Empty;

            if (values[4] != null)
                p4 = values[4].ToString();
            else
                p4 = string.Empty;

            if (values[5] != null)
                p5 = values[5].ToString();
            else
                p5 = string.Empty;
   
            if (values[4] != null)
                oldDP = values[4] as ODataProperty;
            else
                oldDP = null; 
            
            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
            else if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Class Name" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Name" });
                return;
            }

            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Range" });
                return;
            }

            else if (string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Expression" });
                return;
            }

            //else if (string.IsNullOrEmpty(p4))
            //{
            //    EventHandler handler = EventCompleted;
            //    handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Old Domain Name" });
            //    return;
            //}

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.ModifyDataProperty;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4); sItems.Add(p5);
            nMessage.OldDP = oldDP;
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }
    public class MOPModel : INotifyPropertyChanged
    {
        public event RaiseProposeEventHandler RaiseProposal2;

        string pStatus;
        public string ProposalStatus
        {
            get
            {
                return this.pStatus;
            }
            set
            {
                this.pStatus = value;
                OnPropertyChanged("ProposalStatus");
            }
        }

        bool pState;
        public bool ProposalState
        {
            get
            {
                return this.pState;
            }
            set
            {
                this.pState = value;
                if (this.pState) { this.ProposalState1 = false; } else { this.ProposalState1 = true; }
                OnPropertyChanged("ProposalState");
            }
        }

        bool pState1;
        public bool ProposalState1
        {
            get
            {
                return this.pState1;
            }
            set
            {
                this.pState1 = value;
                OnPropertyChanged("ProposalState1");
            }
        }

        string curUserName;
        public string CurrentUserName
        {
            get
            {
                return this.curUserName;
            }
            set
            {
                this.curUserName = value;
                OnPropertyChanged("CurrentUserName");
            }
        }

        DBData curDbInstance;

        public DBData CurrentDbInstance
        {
            get
            {
                return curDbInstance;
            }
        }

        string selectedCN;
        public string SelectedCN
        {
            get
            {
                return selectedCN;
            }
            set
            {
                this.selectedCN = value;
                OnPropertyChanged("SelectedCN");
            }
        }

        string selectedPN;
        public string SelectedPN
        {
            get
            {
                return selectedPN;
            }
            set
            {
                this.selectedPN = value;
                OnPropertyChanged("SelectedPN");
            }
        }

        List<string> pNames;

        public List<string> PNames
        {
            get
            {
                return this.pNames;
            }
            set
            {
                this.pNames = value;
                OnPropertyChanged("PNames");
            }
        }

        List<string> dmClasses;

        public List<string> DMClasses
        {
            get
            {
                return this.dmClasses;
            }
            private set
            {
                this.dmClasses = value;
                OnPropertyChanged("DMClasses");
            }
        }

        bool ignoreCAbsense;

        public bool IgnoreCAbsense
        {
            get
            {
                return this.ignoreCAbsense;
            }
            set
            {
                this.ignoreCAbsense = value;
                OnPropertyChanged("IgnoreCAbsense");
            }
        }

        ProposeOPCommand popCommand;

        public ICommand PopCommand
        {
            get
            {
                if (popCommand == null)
                {
                    popCommand = new ProposeOPCommand();
                    popCommand.RaisePropose += PopCommand_RaisePropose;
                }
                return popCommand;
            }
        }

        SaveOPCommand sopCommand;

        public ICommand SaveOPCommand
        {
            get
            {
                if (sopCommand == null)
                {
                    sopCommand = new SaveOPCommand();
                    sopCommand.RaisePropose += PopCommand_RaisePropose;
                }
                return sopCommand;
            }
        }
        private void PopCommand_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this, e);
        }

        public MOPModel() { }

        public MOPModel(string uName, DBData dbData)
        {
            FillDT();
            this.ProposalStatus = "Changes not proposed";
            this.ProposalState = false;
            this.IgnoreCAbsense = true;
            this.CurrentUserName = uName;
            this.curDbInstance = dbData;
            List<string> clsNames = new List<string>();
            foreach (SemanticStructure ss in this.curDbInstance.OwlData.RDFG.NODetails.Values)
            {
                if (ss.SSType == SStrType.Class)
                    clsNames.Add(ss.SSName);
            }
            this.DMClasses = clsNames;
        }

        public void FillProperties()
        {
            List<string> pNs = new List<string>();
            
            //Step2: Get outgoing and incoming edges for selected class name
            string nName = this.curDbInstance.OwlData.RDFG.GetExactNodeName(this.SelectedCN);

            List<string> outgoing = this.curDbInstance.OwlData.RDFG.GetEdgesForNode(nName);
            List<string> incoming = this.curDbInstance.OwlData.RDFG.GetIncomingEdgesForNode(nName);
            List<string> classHierarchy = new List<string>();

            //sorting outgoing list because i added number whle adding outgoing properties.
            SortedList<int, string> nsList = new SortedList<int, string>();
            foreach (string s in outgoing)
            {
                int indOfSep = s.IndexOf('-');
                string strKey = s.Substring(0, indOfSep);
                string strVal = s.Substring(indOfSep + 1);

                nsList[Int32.Parse(strKey)] = strVal; //s.Split('-')[1];
            }
            List<string> nOutgoing = new List<string>();
            foreach (KeyValuePair<int, string> kvp in nsList)
            {
                nOutgoing.Add(string.Format("{0}-{1}", kvp.Key, kvp.Value));
            }
            //sorting done

            #region No need to go up in hierarchy to get something
            //int counter = 0;
            //while (counter <= nOutgoing.Count - 1)
            //{
            //    string ots = nOutgoing[counter];
            //    string[] reqParts = ots.Split('-')[1].Split(':');
            //    if (reqParts[1].ToLower().Equals("class"))
            //    {
            //        //Step3: Calculating data properties up in hierarchies 
            //        //Seems like not required to get all properties
            //        #region Good Recursive Code for getting up hierarchies
            //        string relation = string.Empty;
            //        if (this.curDbInstance.OwlData.RDFG.EdgeData.ContainsKey(string.Format("{0}-{1}", nName.Split(':')[0], reqParts[0])))
            //            relation = this.curDbInstance.OwlData.RDFG.EdgeData[string.Format("{0}-{1}", nName.Split(':')[0], reqParts[0])];

            //        //if sub class I want to add all data properties gathered from all base classes till owl:Thing
            //        if (relation.ToLower().Equals("subclassof"))
            //        {
            //            string source = ots.Split('-')[1];
            //            //classHierarchy.Add(source);
            //            while (!source.ToLower().Equals("owl:thing"))
            //            {
            //                classHierarchy.Add(source);
            //                List<string> og = this.curDbInstance.OwlData.RDFG.GetEdgesForNode(source);
            //                List<string> ic = this.curDbInstance.OwlData.RDFG.GetIncomingEdgesForNode(source);
            //                foreach (string ics in ic)
            //                {
            //                    if (ics.Split(':')[1].Equals("DatatypeProperty"))
            //                        incoming.Add(ics);
            //                }
            //                foreach (string sst in og) //must be one only
            //                    source = sst.Split('-')[1];
            //            }
            //            classHierarchy.Add(source);
            //        }
            //        #endregion
            //    }
            //    counter++;
            //}
            #endregion
            //No need of data properties to add
            //foreach (string s in incoming)
            //{
            //    pNs.Add(s);
            //}

            foreach (string s in nOutgoing)
            {
                if (s.Contains(':'))
                    if (!s.Split(':')[1].ToLower().Contains("instance"))
                        pNs.Add(s);
            }
            foreach(string s in incoming)
            {
                if (s.Contains(':'))
                    if (!s.Split(':')[1].ToLower().Contains("instance"))
                        pNs.Add(s);
            }
            this.PNames = pNs;
        }

        ODataProperty oldProp;

        public ODataProperty OldDP
        {
            get
            {
                return this.oldProp;
            }
            set
            {
                this.oldProp = value;
                OnPropertyChanged("OldDP");
            }
        }
        public void GetPropertyDetails()
        {
            if (string.IsNullOrEmpty(this.SelectedPN))
                return;
            string nName = this.curDbInstance.OwlData.RDFG.GetExactNodeName(this.SelectedCN);

            List<string> routgoing = this.curDbInstance.OwlData.RDFG.GetEdgesForNode(nName);
            List<string> rincoming = this.curDbInstance.OwlData.RDFG.GetIncomingEdgesForNode(nName);

            string strKey2 = string.Empty;
            string strVal2 = string.Empty;
            if (this.SelectedPN.Contains('-'))
            {
                int indOfSep2 = this.SelectedPN.IndexOf('-');
                strKey2 = this.SelectedPN.Substring(0, indOfSep2);
                strVal2 = this.SelectedPN.Substring(indOfSep2 + 1);
                
                foreach (string s in routgoing)
                {
                    int indOfSep = s.IndexOf('-');
                    string strKey = s.Substring(0, indOfSep);
                    string strVal = s.Substring(indOfSep + 1);

                    if (strVal.Split(':')[1].ToLower().Equals("rangeexpression")) //if (s.Split('-')[1].Split(':')[1].ToLower().Equals("rangeexpression"))
                    {
                        string cStr = strVal.Split(':')[0]; //.Split('-')[1]
                        if (cStr.Contains(strVal2.Split(':')[0]))//.Contains(cStr))//.Contains(strVal2))//SelectedPN.Split('-')[1].Split(':')[0]))
                        {
                            string rnName = this.curDbInstance.OwlData.RDFG.GetExactNodeName(cStr);
                            SemanticStructure rss = this.curDbInstance.OwlData.RDFG.NODetails[rnName];
                            this.RExpr = rss.XMLURI;
                            break;
                        }
                    }
                }

                SemanticStructure ss = this.curDbInstance.OwlData.RDFG.NODetails[strVal2];// this.SelectedPN.Split('-')[1]];

                List<string> outgoing = this.curDbInstance.OwlData.RDFG.GetEdgesForNode(strVal2);// this.SelectedPN.Split('-')[1]);
                List<string> incoming = this.curDbInstance.OwlData.RDFG.GetIncomingEdgesForNode(strVal2);// this.SelectedPN.Split('-')[1]);

                var firstRResult = incoming.FindAll((p) => { if (p.Split(':')[1].ToLower().Equals("datatypeproperty") || p.Split(':')[1].ToLower().Equals("datatype")) { return true; } else { return false; } });
                if (firstRResult.Count == 1)
                {
                    var secResult = this.curDbInstance.OwlData.RDFG.GetEdgesForNode(firstRResult[0]).FindAll((p) => { if (dT.Contains(p.Split('-')[1].Split(':')[0])) { return true; } else { return false; } });
                    if (secResult.Count == 1)
                    {
                        this.DPRange = secResult[0].Split('-')[1].Split(':')[0];
                    }
                    else
                    { }
                }
                else
                { }
            }
            else
            {
                foreach (string s in routgoing)
                {
                    int indOfSep = s.IndexOf('-');
                    string strKey = s.Substring(0, indOfSep);
                    string strVal = s.Substring(indOfSep + 1);

                    if (strVal.Split(':')[1].ToLower().Equals("rangeexpression")) //if (s.Split('-')[1].Split(':')[1].ToLower().Equals("rangeexpression"))
                    {
                        string cStr = strVal.Split(':')[0]; //.Split('-')[1]
                        if (cStr.Contains(strVal2.Split(':')[0]))//.Contains(cStr))//.Contains(strVal2))//SelectedPN.Split('-')[1].Split(':')[0]))
                        {
                            string rnName = this.curDbInstance.OwlData.RDFG.GetExactNodeName(cStr);
                            SemanticStructure rss = this.curDbInstance.OwlData.RDFG.NODetails[rnName];
                            this.RExpr = rss.XMLURI;
                            break;
                        }
                    }
                }

                //No - in the property hence data property
                string exactdpn = this.curDbInstance.OwlData.RDFG.GetExactNodeName(this.SelectedPN);
                SemanticStructure ss = this.curDbInstance.OwlData.RDFG.NODetails[this.SelectedPN];// exactdpn];//.Split('-')[1]];

                List<string> outgoing = this.curDbInstance.OwlData.RDFG.GetEdgesForNode(this.SelectedPN);// exactdpn);// this.SelectedPN.Split('-')[1]);
                List<string> incoming = this.curDbInstance.OwlData.RDFG.GetIncomingEdgesForNode(this.SelectedPN);// exactdpn);// this.SelectedPN.Split('-')[1]);

                var secResult = outgoing.FindAll((p) => { if (dT.Contains(p.Split('-')[1].Split(':')[0])) { return true; } else { return false; } });
                if (secResult.Count == 1)
                {
                    this.DPRange = secResult[0].Split('-')[1].Split(':')[0];
                }
                else
                { }
            }
        }


        string rExpr;
        public string RExpr
        {
            get
            {
                return this.rExpr;
            }
            set
            {
                this.rExpr = value;
                OnPropertyChanged("RExpr");
            }
        }

        string baseURI;
        public string BaseURI
        {
            get
            {
                return baseURI;
            }
            set
            {
                this.baseURI = value;
                OnPropertyChanged("BaseURI");
            }
        }
        string dpRange;
        public string DPRange
        {
            get
            {
                return dpRange;
            }
            set
            {
                this.dpRange = value;
                OnPropertyChanged("DPRange");
            }
        }

        string dpDomain;
        public string DPDomain
        {
            get
            {
                return this.dpDomain;
            }
            set
            {
                this.dpDomain = value;
                OnPropertyChanged("DPDomain");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        List<string> dT = new List<string>();
        void FillDT() //xsd:
        {
            dT.Add("byte");
            dT.Add("boolean");
            dT.Add("int");
            dT.Add("integer");
            dT.Add("long");
            dT.Add("short");
            dT.Add("string");
            dT.Add("double");
            dT.Add("decimal");
            dT.Add("float");
            dT.Add("unsignedByte");
            dT.Add("unsignedInt");
            dT.Add("unsignedLong");
            dT.Add("unsignedShort");
        }
    }
}
