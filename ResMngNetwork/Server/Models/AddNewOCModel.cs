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
    public class ProposeOCCommand : ICommand
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

            if (values[1] != null)
                p1 = values[1].ToString();
            else
                p1 = string.Empty;

            if (values[2] != null)
                p2 = values[2].ToString();
            else
                p2 = string.Empty;

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.PCause = ProposalCause.NewOClass;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            nMessage.ProposedUser = p0;
            sItems.Add(p1);
            sItems.Add(p2);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class SaveOCCommand : ICommand
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

            if (values[1] != null)
                p1 = values[1].ToString();
            else
                p1 = string.Empty;

            if (values[2] != null)
                p2 = values[2].ToString();
            else
                p2 = string.Empty;

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.PCause = ProposalCause.NewOClass;
            nMessage.PTYpe = ProposalType.Transition;
            List<string> sItems = new List<string>();
            nMessage.ProposedUser = p0;
            sItems.Add(p1);
            sItems.Add(p2);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class AddNewOCModel : INotifyPropertyChanged
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

        string cName;

        public string CName
        {
            get
            {
                return this.cName;
            }
            set
            {
                this.cName = value;
                OnPropertyChanged("CName");
            }
        }

        List<string> baseClasses;
        public List<string> BaseClasses
        {
            get
            {
                return this.baseClasses;
            }
            set
            {
                this.baseClasses = value;
            }
        }

        string sbClass;
        public string SBClass
        {
            get
            {
                return this.sbClass;
            }
            set
            {
                this.sbClass = value;
                OnPropertyChanged("SBClass");
            }
        }

        ProposeOCCommand pocCommand;
        public ProposeOCCommand POcCommand
        {
            get
            {
                if (pocCommand == null)
                {
                    pocCommand = new ProposeOCCommand();
                    pocCommand.RaisePropose += POcCommand_RaisePropose;
                }
                return pocCommand;
            }
        }

        SaveOCCommand socCommand;

        public SaveOCCommand SOcCommand
        {
            get
            {
                if (socCommand == null)
                {
                    socCommand = new SaveOCCommand();
                    socCommand.RaisePropose += POcCommand_RaisePropose;
                }
                return socCommand;
            }
        }

        private void POcCommand_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this, e);
        }

        public AddNewOCModel()
        {

        }
        public AddNewOCModel(string uName, DBData dbData)
        {
            this.ProposalStatus = "Changes not proposed";
            this.ProposalState = false;
            this.CurrentUserName = uName;
            this.curDbInstance = dbData;
            this.BaseClasses = GetBaseClasses();
        }



        List<string> GetBaseClasses()
        {
            List<string> bc = new List<string>();
            foreach(KeyValuePair<string, SemanticStructure> kvp in this.curDbInstance.OwlData.RDFG.NODetails)
            {
                if (kvp.Value.SSType == SStrType.Class)
                    bc.Add(kvp.Value.SSName);
            }
            return bc;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
