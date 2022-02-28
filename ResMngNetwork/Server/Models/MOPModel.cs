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
            
            string p0, p1, p2 = string.Empty;
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
                oldDP = values[3] as ODataProperty;
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
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Range" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Expression" });
                return;
            }

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.ModifyDataProperty;
            nMessage.PTYpe = ProposalType.Transition;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2);
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

            string p0, p1, p2, p3,p4 = string.Empty;
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
                oldDP = values[5] as ODataProperty;
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
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Range" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Property Expression" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Old Domain Name" });
                return;
            }

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.ModifyDataProperty;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4);
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
            this.ProposalStatus = "Changes not proposed";
            this.ProposalState = false;
            this.IgnoreCAbsense = true;
            this.CurrentUserName = uName;
            this.curDbInstance = dbData;
            List<string> clsNames = new List<string>();
            foreach (OClass oCls in this.curDbInstance.OwlData.OWLClasses)
            {
                clsNames.Add(oCls.CName);
            }
            this.DMClasses = clsNames;
        }

        public void FillProperties()
        {
            List<string> pNs = new List<string>();
            foreach(ODataProperty odpProp in this.curDbInstance.OwlData.OWLDataProperties)
            {
                var slProp = odpProp.DPChildNodes.Find((p) => { if (p.CNType.Equals("rdfs:domain") && (p.CNName.Equals(this.SelectedCN))) return true; else return false; });
                if (slProp != null)
                {
                    pNs.Add(odpProp.DProperty);
                }
                
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
            ODataProperty pDetail = this.curDbInstance.OwlData.OWLDataProperties.Find((dp) => { if (dp.DProperty.Equals(this.SelectedPN)) return true; else return false; });
            this.OldDP = pDetail;

            StringBuilder sBuilder = new StringBuilder();
            sBuilder.Append("[");
            foreach (OChildNode ocNode in pDetail.DPChildNodes)
            {
                if(ocNode.CNType.Equals("rdfs:domain"))
                {
                    if (ocNode.CNName.Equals(this.SelectedCN))
                        this.DPDomain = ocNode.CNName;
                }
                if(ocNode.CNType.Equals("rdfs:range") || ocNode.CNType.Equals("owl:onDatatype"))
                {
                    this.DPRange = ocNode.CNName;
                }
                if (ValidateRules.IsOperator(ocNode.CNType))
                {
                    //now we need to construct exprssion
                    sBuilder.Append(ValidateRules.GetOperator(ocNode.CNType));
                    sBuilder.Append(" ");
                    sBuilder.Append(ocNode.CNSpecialValue);
                    sBuilder.Append(" , ");
                }
            }
            string newStr = sBuilder.ToString();
            string newStr1 = string.Empty;
            if (!string.IsNullOrEmpty(newStr) & newStr.Length > 1) //if there is no expression then only [ will be there and hence I am checking > 1
            {
                newStr1 = newStr.Substring(0, (newStr.Length - (" , ").Length));
                this.RExpr = newStr1 + "]";
            }
            else
            {
                this.RExpr = newStr1;
            }
            
            //ConstructExpression using ocNode and then assign expression. Currently I support 
            //'length' | 'minLength' | 'maxLength' | 'pattern' | 'langRange' | '<=' | '<' | '>=' | '>'
            //length minLength and maxLength -> !Empty
            // <= minInclusive
            // < minExclusive
            // >=maxInclusive
            // > maxExclusive
            // this.RExpr = dP.PRangeExpr;
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
    }
}
