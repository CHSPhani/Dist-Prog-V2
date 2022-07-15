using DataSerailizer;
using Server.DSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Server.Models
{
    public class PSaveDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SaveCompleteEventArgs : EventArgs
    {
        public string EvntMsg { get; set; }

        public SaveCompleteEventArgs()
        {
            EvntMsg = string.Empty;
        }
    }

    public class ProposeEventArgs : EventArgs
    {
        public NodeMesaage NMessage { get; set; }

        public ProposeEventArgs()
        {
            NMessage = new NodeMesaage();
        }
    }

    public class ProposeResultEventArgs : EventArgs
    {
        public bool PResult { get; set; }

        public ProposeResultEventArgs()
        {
            PResult = false;
        }
    }

    public class TransitResultEventArgs :EventArgs
    {
        public bool TResult { get; set; }

        public TransitResultEventArgs()
        {
            TResult = false;
        }
    }

    public delegate void RaiseProposeEventHandler(object sender, ProposeEventArgs e);
    public delegate bool ProposeResultEventHandler(object sender, ProposeResultEventArgs e);
    public delegate bool TransitResultEventHandler(object sender, TransitResultEventArgs e);

    public class ProposePackageCommand : ICommand
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

            string p1, p2, p3, p4 = string.Empty;
            

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                p2 = values[1].ToString();
            else
                p2 = string.Empty;

            if (values[2] != null)
                p3 = values[2].ToString();
            else
                p3 = string.Empty;

            if (values[3] != null)
                p4 = values[3].ToString();
            else
                p4 = string.Empty;

            if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PkgName" });
                return;
            }
            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PkgNameSpace" });
                return;
            }
            else if (string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Notes" });
                return;
            }
            else if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "User Name" });
                return;
            }

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p1;
            nMessage.PCause = ProposalCause.NewPackage;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
            
            bool res = false; //DBInsert.InsertPackage(TypeNames.Package, p1, p2, p3, p4);

            if (res)
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Done" });
            }
            else
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Failed" });
            }
        }
    }

    public class SavePkgCommand : ICommand
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

            string p1, p2, p3, p4 = string.Empty;
            DBData dbData = null;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                p2 = values[1].ToString();
            else
                p2 = string.Empty;

            if (values[2] != null)
                p3 = values[2].ToString();
            else
                p3 = string.Empty;

            if (values[3] != null)
                p4 = values[3].ToString();
            else
                p4 = string.Empty;

            if (values[4] != null)
                dbData = values[4] as DBData;
            else
                dbData = null;

            if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "UserName" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PkgName" });
                return;
            }
            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PkgNameSpace" });
                return;
            }

            else if (string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "Notes" });
                return;
            }
            else if (dbData == null)
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "DBData" });
                return;
            }

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p1;
            nMessage.PCause = ProposalCause.NewPackage;
            nMessage.PTYpe = ProposalType.Transition;
            List<string> sItems = new List<string>();
            sItems.Add(p2); sItems.Add(p3); sItems.Add(p4);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class InsertPkg : INotifyPropertyChanged
    {
        public event RaiseProposeEventHandler RaiseProposal2;

        string etName;
        public string ETName
        {
            get
            {
                return this.etName;
            }
            set
            {
                this.etName = value; ;
                OnPropertyChanged("ETName");
            }
        }

        string etURI;
        public string ETURI
        {
            get
            {
                return this.etURI;
            }
            set
            {
                this.etURI = value; ;
                OnPropertyChanged("ETURI");
            }
        }

        string etNotes;
        public string ETNotes
        {
            get
            {
                return this.etNotes;
            }

            set
            {
                this.etNotes = value;
                OnPropertyChanged("ETNotes");
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

        SavePkgCommand spCommand;
        public SavePkgCommand SPCommand
        {
            get
            {
                if (spCommand == null)
                {
                    spCommand = new SavePkgCommand();
                    spCommand.EventCompleted += SpCommand_EventCompleted;
                    spCommand.RaisePropose += PpCommand_RaisePropose;// SpCommand_EventCompleted;
                }
                return spCommand;
            }
        }

        ProposePackageCommand ppCommand;

        public ICommand PPCommand
        {
            get
            {
                if(ppCommand == null)
                {
                    ppCommand = new ProposePackageCommand();
                    ppCommand.RaisePropose += PpCommand_RaisePropose;
                }
                return ppCommand;
            }
        }

        private void PpCommand_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this,e);
        }

        public void TransitDone()
        {
            this.ETName = string.Empty;
            this.ETURI = string.Empty;
            this.ETNotes = string.Empty;
        }

        private void SpCommand_EventCompleted(object sender, EventArgs e)
        {
            SaveCompleteEventArgs scEventArgs = e as SaveCompleteEventArgs;
            string msg = scEventArgs.EvntMsg;

            if (msg.Equals("PkgName"))
            {
                MessageBoxResult result = MessageBox.Show("Provide Class Name", "Error", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.ETURI = string.Empty;
                this.ETNotes = string.Empty;
            }
            else if (msg.Equals("PkgNameSpace"))
            {
                MessageBoxResult result = MessageBox.Show("Provide Namespace Value", "Error", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.ETURI = string.Empty;
                this.ETNotes = string.Empty;
            }
            else if (msg.Equals("Notes"))
            {
                MessageBoxResult result = MessageBox.Show("Enter Notes for Package", "Error", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.ETURI = string.Empty;
                this.ETNotes = string.Empty;
            }
            else if (msg.Equals("Notes"))
            {
                MessageBoxResult result = MessageBox.Show("DBData is not there Empty. some thing wrong", "Error", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.ETURI = string.Empty;
                this.ETNotes = string.Empty;
            }
            else if (msg.Equals("Done"))
            {
                MessageBoxResult result = MessageBox.Show("Saved New Class Values", "Success", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.ETURI = string.Empty;
                this.ETNotes = string.Empty;
            }
            else if (msg.Equals("Failed"))
            {
                MessageBoxResult result = MessageBox.Show("Insertion of class values Failed", "Error", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.ETURI = string.Empty;
                this.ETNotes = string.Empty;
            }
        }

        public InsertPkg()
        {
           
        }

        public InsertPkg(string uName, DBData dbData)
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
