using DataSerailizer;
using Server.DSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Server.Models
{
    public class ProposeClassCommand : ICommand
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

            string p1, p2, p3, p4, p5 = string.Empty;
            DBData dbData;

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
                p5 = values[4].ToString();
            else
                p5 = string.Empty;

            if (values[5] != null)
                dbData = values[5] as DBData;
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
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "ClassName" });
                return;
            }
            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "NameSpace" });
                return;
            }
            else if (string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "BaseClass" });
                return;
            }
            //else if (string.IsNullOrEmpty(p5))
            //{
            //    EventHandler handler = EventCompleted;
            //    handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "ClassNotes" });
            //    return;
            //}
            else if(dbData == null)
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "DBData" });
                return;
            }

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p1;
            nMessage.PCause = ProposalCause.NewClass;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4);
            if (string.IsNullOrEmpty(p5))
            {
                p5 = "ClassEmpty";
            }
            sItems.Add(p5);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }
    public class SaveClsCommand : ICommand
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

            string p1, p2, p3, p4, p5 = string.Empty;
            DBData dbData;

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
                p5 = values[4].ToString();
            else
                p5 = string.Empty;

            if (values[5] != null)
                dbData = values[5] as DBData;
            else
                dbData = null;

            if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "ClsName" });
                return;
            }
            else if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "NameSpace" });
                return;
            }

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p1;
            nMessage.PCause = ProposalCause.NewClass;
            nMessage.PTYpe = ProposalType.Transition;
            List<string> sItems = new List<string>();
            sItems.Add(p1);  sItems.Add(p2); sItems.Add(p3); sItems.Add(p4); sItems.Add(p5); 
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }
    public class InsertCls : INotifyPropertyChanged
    {
        public event RaiseProposeEventHandler RaiseProposal2;

        List<string> clsNames;
        public List<string> ClsNames
        {
            get
            {
                return this.clsNames;
            }
            set
            {
                this.clsNames = value;
                OnPropertyChanged("ClsNames");
            }
        }

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

        List<string> etURI;
        public List<string> ETURI
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

        string baseClsName;
        public string BaseClsName
        {
            get
            {
                return this.baseClsName;
            }

            set
            {
                this.baseClsName = value;
                OnPropertyChanged("BaseClsName");
            }
        }

        string nsName;
        public string NamespaceName
        {
            get
            {
                return this.nsName;
            }
            set
            {
                this.nsName = value;
                OnPropertyChanged("NamespaceName");
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

        string curUserName;
        public string CurUsrName
        {
            get
            {
                return this.curUserName;
            }
            set
            {
                this.curUserName = value;
                OnPropertyChanged("CurUsrName");
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

        DBData curDBData;
        public DBData CurDbData
        {
            get
            {
                return this.curDBData;
            }
            set
            {
                this.curDBData = value;
                OnPropertyChanged("CurDbData");
            }
        }

        ProposeClassCommand pcCommand;
        public ProposeClassCommand PCCommand
        {
            get
            {
                if(pcCommand == null)
                {
                    pcCommand = new ProposeClassCommand();
                    pcCommand.RaisePropose += PcCommand_RaisePropose;
                }
                return pcCommand;
            }
        }

        private void PcCommand_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this, e);
        }

        SaveClsCommand scCommand;

        public SaveClsCommand SCCommand
        {
            get
            {
                if (scCommand == null)
                {
                    scCommand = new SaveClsCommand();
                    scCommand.EventCompleted += ScCommand_EventCompleted;
                    scCommand.RaisePropose += PcCommand_RaisePropose;
                }
                return scCommand;
            }
        }

        public void TransitDone()
        {
            this.ETName = string.Empty;
            this.NamespaceName = string.Empty;
            this.ETNotes = string.Empty;
            this.BaseClsName = string.Empty;
        }

        private void ScCommand_EventCompleted(object sender, EventArgs e)
        {
            SaveCompleteEventArgs scEventArgs = e as SaveCompleteEventArgs;
            string msg = scEventArgs.EvntMsg;

            if (msg.Equals("ClsName"))
            {
                MessageBoxResult result = MessageBox.Show("Provide Class Name", "Error", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.NamespaceName = string.Empty;
                this.ETNotes = string.Empty;
                this.BaseClsName = string.Empty;
            }

            else if (msg.Equals("NameSpace"))
            {
                MessageBoxResult result = MessageBox.Show("Provide Namespace Value", "Error", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.NamespaceName = string.Empty;
                this.ETNotes = string.Empty;
                this.BaseClsName = string.Empty;
            }
            else if (msg.Equals("UserName"))
            {
                MessageBoxResult result = MessageBox.Show("Provide User Name", "Error", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.NamespaceName = string.Empty;
                this.ETNotes = string.Empty;
                this.BaseClsName = string.Empty;
            }
            else if (msg.Equals("Done"))
            {
                MessageBoxResult result = MessageBox.Show("Saved New Class Values", "Success", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.NamespaceName = string.Empty;
                this.ETNotes = string.Empty;
                this.BaseClsName = string.Empty;
            }
            else if (msg.Equals("Failed"))
            {
                MessageBoxResult result = MessageBox.Show("Insertion of class values Failed", "Error", MessageBoxButton.OK);
                this.ETName = string.Empty;
                this.NamespaceName = string.Empty;
                this.ETNotes = string.Empty;
                this.BaseClsName = string.Empty;
            }
        }
        
        public InsertCls()
        {
            clsNames = new List<string>();
            etURI = new List<string>();
            curUserName = string.Empty;
            curDBData = null;

        }
        public InsertCls(string curUN, DBData curDb)
        {
            this.ProposalStatus = "Changes not proposed";
            this.ProposalState = false;

            this.CurUsrName = curUN;
            this.CurDbData = curDb;
            this.ClsNames = GetAllClassDetails();
            this.ETURI = GetAllNameSpaces();
        }

        public List<string> GetAllClassDetails()
        {
            List<string> cDetails = new List<string>();
            try
            {
                foreach (EntityData eData in curDBData.ClassData)
                {
                    cDetails.Add(eData.FullEntityName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception while getting namespace details from entities and masterdetails. Details are {0}", ex.Message));
            }
            return cDetails;
        }
        public List<string> GetAllNameSpaces()
        {
            List<string> cDetails = new List<string>();
            try
            {
                foreach(PackageData pData in curDBData.PkgData)
                {
                    cDetails.Add(pData.PkgName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception while getting namespace details from entities and masterdetails. Details are {0}", ex.Message));
            }
            return cDetails;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
