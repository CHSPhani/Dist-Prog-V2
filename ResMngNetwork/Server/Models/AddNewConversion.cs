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
    public class ProposeNewConvr : ICommand
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

            string p0, p1, p2, p3, p4 = string.Empty;


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

            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }
            else if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }
            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }
            else if (string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }


            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.NewConversion;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class SaveNewConvr : ICommand
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


            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }
            else if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }
            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }
            else if (string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "" });
                return;
            }


            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p0;
            nMessage.PCause = ProposalCause.NewConversion;
            nMessage.PTYpe = ProposalType.Transition;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class AddNewConversion : INotifyPropertyChanged
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

        ProposeNewConvr pnConv;

        public ProposeNewConvr PNConv
        {
            get
            {
                if (pnConv == null)
                {
                    pnConv = new ProposeNewConvr();
                    pnConv.RaisePropose += PnConv_RaisePropose;
                }
                return pnConv;
            }

        }

        SaveNewConvr snConv;

        public SaveNewConvr SNConv
        {
            get
            {
                if (snConv == null)
                {
                    snConv = new SaveNewConvr();
                    snConv.RaisePropose += PnConv_RaisePropose;
                }
                return snConv;
            }
        }

        private void PnConv_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this, e);
        }


        public AddNewConversion() { }

        public AddNewConversion(string uName, DBData dbData)
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
