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
    public class StoreNewOntologyCommand:ICommand
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

            string p1 = string.Empty;
            OWLDataG oDet;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                oDet = values[1] as OWLDataG;
            else
                oDet = null;

            if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
            else if (oDet == null)
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "ODetails" });
                return;
            }


            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p1;
            nMessage.PCause = ProposalCause.NewOntology;
            nMessage.PTYpe = ProposalType.Transition;
            nMessage.OwlDatag = oDet;
            List<string> sItems = new List<string>();
            sItems.Add(p1);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }
    public class ProposeNewOntologyCommand : ICommand
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

            string p1 = string.Empty;
            

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            OWLDataG oDet;
            if (values[1] != null)
               oDet = values[1] as OWLDataG;
            else
                oDet = null;

            if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
            else if (oDet == null)
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "ODetails" });
                return;
            }

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p1;
            nMessage.PCause = ProposalCause.NewOntology;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            sItems.Add(p1); 
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class UploadOntologyFile : INotifyPropertyChanged
    {
        public event RaiseProposeEventHandler RaiseProposal2;

        public UploadOntologyFile()
        {

        }

        ProposeNewOntologyCommand pnoCommand;
        public ICommand PnoCommand
        {
            get
            {
                if(pnoCommand == null)
                {
                    pnoCommand = new ProposeNewOntologyCommand();
                    pnoCommand.RaisePropose += PnoCommand_RaisePropose;
                }
                return pnoCommand;
            }
        }

        private void PnoCommand_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this, e);
        }

        StoreNewOntologyCommand snoCommand;

        public ICommand SnoCommand
        {
            get
            {
                if(snoCommand == null)
                {
                    snoCommand = new StoreNewOntologyCommand();
                    snoCommand.RaisePropose += PnoCommand_RaisePropose;
                }
                return snoCommand;
            }
        }

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

        OWLDataG oDetails;
        public OWLDataG ODetails
        {
            get
            {
                return this.oDetails;
            }
            set
            {
                this.oDetails = value;
                OnPropertyChanged("ODetails");
            }
        }

        string oFilePath;
        public string OFilePath
        {
            get
            {
                return this.oFilePath;
            }
            set
            {
                this.oFilePath = value;
                OnPropertyChanged("OFilePath");
            }
        }

        string oUploadStatus;

        public string OUploadStatus
        {
            get
            {
                return this.oUploadStatus;
            }
            set
            {
                this.oUploadStatus = value;
                OnPropertyChanged("OUploadStatus");
            }
        }

        string cuUser;
        public string CSUser
        {
            get
            {
                return this.cuUser;
            }
            set
            {
                this.cuUser = value;
                OnPropertyChanged("CSUser");
            }
        }
        public void TransitDone()
        {

        }
        public UploadOntologyFile(string cUser, DBData dbData)
        {
            this.ProposalStatus = "Changes not proposed";
            this.ProposalState = false;
            this.OUploadStatus = "Upload Not Done";
            this.CSUser = cUser;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
