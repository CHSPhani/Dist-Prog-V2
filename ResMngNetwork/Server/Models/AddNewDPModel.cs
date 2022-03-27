using DataSerailizer;
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
    public class SaveDpCommand : ICommand
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

            string p0, p1, p2, p3, p4 = string.Empty; //, p5 


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

            //if (values[5] != null)
            //    p5 = values[5].ToString();
            //else
            //    p5 = string.Empty;

            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
            else if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PropertyName" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "SelectedCN" });
                return;
            }
            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "FunctionalProperty" });
                return;
            }
            else if (string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "SelectedDT" });
                return;
            }
            //else if (string.IsNullOrEmpty(p5))
            //{
            //    EventHandler handler = EventCompleted;
            //    handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Expression" });
            //    return;
            //}
            

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.NewDataProperty;
            nMessage.PTYpe = ProposalType.Transition;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4); //sItems.Add(p5);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class ProposeDPCommand : ICommand
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

            string p0, p1, p2, p3, p4  = string.Empty; //,p5
            

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

            //if (values[5] != null)
            //    p5 = values[5].ToString();
            //else
            //    p5 = string.Empty;
            

            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
            else if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PropertyName" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "SelectedCN" });
                return;
            }
            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "FunctionalProperty" });
                return;
            }
            else if (string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "SelectedDT" });
                return;
            }
            //else if (string.IsNullOrEmpty(p5))
            //{
            //    EventHandler handler = EventCompleted;
            //    handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Expression" });
            //    return;
            //}

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.NewDataProperty;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4); //sItems.Add(p5);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class AddNewDPModel : INotifyPropertyChanged
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

        string pName;

        public string PName
        {
            get
            {
                return this.pName;
            }
            set
            {
                this.pName = value;
                OnPropertyChanged("PName");
            }
        }

        string selectedDT;

        public string SelectedDT
        {
            get
            {
                return this.selectedCN;
            }
            set
            {
                this.selectedCN = value;
                OnPropertyChanged("SelectedDT");
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

        string dtExpression;

        public string DtExpression
        {
            get
            {
                return this.dtExpression;
            }
            set
            {
                this.dtExpression = value;
                OnPropertyChanged("DtExpression");
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

        List<string> dTypes;

        public List<string> ODataTypes
        {
            get
            {
                return this.dTypes;
            }
            set
            {
                this.dTypes = value;
                OnPropertyChanged("ODataTypes");
            }
        }

        bool functionalProp;

        public bool FunctionalProp
        {
            get
            {
                return functionalProp;
            }

            set
            {
                functionalProp = value;
                OnPropertyChanged("FunctionalProp");
            }
        }

        ProposeDPCommand pDpCommand;

        public ProposeDPCommand PDpCommand
        {
            get
            {
                if(pDpCommand == null)
                {
                    pDpCommand = new ProposeDPCommand();
                    pDpCommand.RaisePropose += PDpCommand_RaisePropose;
                }
                return pDpCommand;
            }
        }

        SaveDpCommand sDpCommand;

        public SaveDpCommand SDpCommand
        {
            get
            {
                if(sDpCommand == null)
                {
                    sDpCommand = new SaveDpCommand();
                    sDpCommand.RaisePropose += PDpCommand_RaisePropose;
                }
                return sDpCommand;
            }
        }

        private void PDpCommand_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this, e);
        }

        public AddNewDPModel()
        {

        }

        public AddNewDPModel(string uName, DBData dbData)
        {
            this.ProposalStatus = "Changes not proposed";
            this.ProposalState = false;
            this.CurrentUserName = uName;
            this.curDbInstance = dbData;
            this.FunctionalProp = false;
            this.PName = string.Empty;
            this.SelectedDT = string.Empty;
            List<string> clsNames = new List<string>();
            foreach (KeyValuePair<string, SemanticStructure> kvp in this.curDbInstance.OwlData.RDFG.NODetails)
            {
                if (kvp.Value.SSType == SStrType.Class)
                    clsNames.Add(kvp.Value.SSName);
            }
            this.DMClasses = clsNames;
            FillDataTypes();
        }

        void FillDataTypes()
        {
            List<string> dT = new List<string>();
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
            this.ODataTypes = dT;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

