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

    public class SaveOpCommand : ICommand
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

            string p0, p1, p2, p3,p4, p5 = string.Empty;


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

            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
            else if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "NewPropertyName" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "NewClassName" });
                return;
            }
            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "ExistingClassName" });
                return;
            }

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.NewObjectProperty;
            nMessage.PTYpe = ProposalType.Transition;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4); sItems.Add(p5);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class ProposeOpCommand : ICommand
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

            if (values[0] != null)
                p0 = values[0].ToString();
            else
                p0 = string.Empty;

            //People just accept this property and hence no data is required to pass them
            //just tell that it is a Equivalent Object Property

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.NewObjectProperty;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class AddNewOPModel : INotifyPropertyChanged
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

        string ncName;
        public string NCName
        {
            get
            {
                return this.ncName;
            }
            set
            {
                this.ncName = value;
                OnPropertyChanged("NCName");
            }
        }

        string opName;
        public string OPName
        {
            get
            {
                return this.opName;
            }
            set
            {
                this.opName = value;
                OnPropertyChanged("OPName");
            }
        }

        bool isEquiv;

        public bool IsEquiv
        {
            get
            {
                return this.isEquiv;
            }
            set
            {
                this.isEquiv = value;
                OnPropertyChanged("IsEquiv");
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

        List<string> iProps;
        public List<string> InProps
        {
            get
            {
                return this.iProps;
            }
            set
            {
                this.iProps = value;
                OnPropertyChanged("InProps");
            }
        }

        string inverseProp;

        public string InverseProp
        {
            get
            {
                return this.inverseProp;
            }
            set
            {
                this.inverseProp = value;
                OnPropertyChanged("InverseProp");
            }
        }

        ProposeOpCommand pOpCommand;

        public ProposeOpCommand POpCommand
        {
            get
            {
                if (pOpCommand == null)
                {
                    pOpCommand = new ProposeOpCommand();
                    pOpCommand.RaisePropose += POpCommand_RaisePropose;
                }
                return pOpCommand;
            }
        }

        SaveOpCommand sOpCommand;

        public SaveOpCommand SOpCommand
        {
            get
            {
                if (sOpCommand == null)
                {
                    sOpCommand = new SaveOpCommand();
                    sOpCommand.RaisePropose += POpCommand_RaisePropose;
                }
                return sOpCommand;
            }
        }

        private void POpCommand_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this, e);
        }

        public AddNewOPModel()
        { }

        public AddNewOPModel(string uName, DBData dbData)
        {
            this.OPName = string.Empty;
            this.InverseProp = string.Empty;
            this.ProposalStatus = "Changes not proposed";
            this.ProposalState = false;
            this.CurrentUserName = uName;
            this.curDbInstance = dbData;
            this.IsEquiv = false;
            List<string> clsNames = new List<string>();
            foreach (OClass oCls in this.curDbInstance.OwlData.OWLClasses)
            {
                clsNames.Add(oCls.CName);
            }
            this.DMClasses = clsNames;
            List<string> ips = new List<string>();
            foreach(OObjectProperty oP in this.curDbInstance.OwlData.OWLObjProperties)
            {
                ips.Add(oP.OProperty);
            }
            this.InProps = ips;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
