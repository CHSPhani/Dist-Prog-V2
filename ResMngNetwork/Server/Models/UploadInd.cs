using DataSerailizer;
using Server.DSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UoB.ToolUtilities.OpenDSSParser;

namespace Server.Models
{
    public class ProposeUploadInd : ICommand
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

            string p0  = string.Empty;
           
            if (values[0] != null)
                p0 = values[0].ToString();
            else
                p0 = string.Empty;

            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
           
            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;

            nMessage.PCause = ProposalCause.UploadInd;
            nMessage.PTYpe = ProposalType.Voting;

            List<string> sItems = new List<string>();
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class SaveUploadInd : ICommand
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

            string p0  = string.Empty;
            DBData curDb = null;
            List<CircuitEntry> cEntites = null;

            if (values[0] != null)
                p0 = values[0].ToString();
            else
                p0 = string.Empty;

            if (values[1] != null)
                curDb = values[1] as DBData;
            else
                curDb = null;

            if (values[2] != null)
                cEntites = values[2] as List<CircuitEntry>;
            else
                cEntites = null;

            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
            
            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.UploadInd;
            nMessage.PTYpe = ProposalType.Transition;
            nMessage.CEntities = cEntites;
            List<string> sItems = new List<string>();
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class UploadInd : INotifyPropertyChanged
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

        List<CircuitEntry> cEntities;
        public List<CircuitEntry> CEntities
        {
            get
            {
                return this.cEntities;
            }
            set
            {
                this.cEntities = value;
                OnPropertyChanged("CEntities");
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

        ProposeUploadInd pUInd;

        public ProposeUploadInd PUInd
        {
            get
            {
                if (pUInd == null)
                {
                    pUInd = new ProposeUploadInd();
                    pUInd.RaisePropose += PUInd_RaisePropose;
                }
                return pUInd;
            }

        }

        private void PUInd_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this, e);
        }

        SaveUploadInd sUInd;

        public SaveUploadInd SUInd
        {
            get
            {
                if (sUInd == null)
                {
                    sUInd = new SaveUploadInd();
                    sUInd.RaisePropose += PUInd_RaisePropose;
                }
                return sUInd;
            }
        }

        
        public UploadInd() { }

        public UploadInd(string uName, DBData dbData)
        {
            this.ProposalStatus = "Changes not proposed";
            this.ProposalState = false;
            this.CurrentUserName = uName;
            this.curDbInstance = dbData;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
