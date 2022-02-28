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
    public class ProposePropertyCommand : ICommand
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

            string p1, p2, p3, p4, p5,p6 = string.Empty;
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
                p6 = values[5].ToString();
            else
                p6 = string.Empty;

            if (values[6] != null)
                dbData = values[6] as DBData;
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
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PropertyName" });
                return;
            }
            else if (string.IsNullOrEmpty(p3))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PropertyType" });
                return;
            }
            //else if (string.IsNullOrEmpty(p4)) //Class somtimes can be empty
            //{
            //    EventHandler handler = EventCompleted;
            //    handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "ContainerClass" });
            //    return;
            //}
            else if (string.IsNullOrEmpty(p5))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "ContainerNamespace" });
                return;
            }
            else if (string.IsNullOrEmpty(p6))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PropertyNotes" });
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
            nMessage.PCause = ProposalCause.NewProperty;
            nMessage.PTYpe = ProposalType.Voting;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p5); sItems.Add(p6);
            if (string.IsNullOrEmpty(p4))
            {
                p4 ="ClassEmpty";
            }
            sItems.Add(p4);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class StorePropertyCommand : ICommand
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

            string p1, p2, p3, p4, p5, p6 = string.Empty;
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
                p6 = values[5].ToString();
            else
                p6 = string.Empty;

            if (values[6] != null)
                dbData = values[6] as DBData;
            else
                dbData = null;

            if (string.IsNullOrEmpty(p1))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PropertyName" });
                return;
            }
            if (string.IsNullOrEmpty(p2))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "PropertyType" });
                return;
            }
            if (string.IsNullOrEmpty(p3) && string.IsNullOrEmpty(p4))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new SaveCompleteEventArgs() { EvntMsg = "ContainerEntity" });
                return;
            }

            NodeMesaage nMessage = new NodeMesaage();
            nMessage.ProposedUser = p1;
            nMessage.PCause = ProposalCause.NewProperty;
            nMessage.PTYpe = ProposalType.Transition;
            List<string> sItems = new List<string>();
            sItems.Add(p1); sItems.Add(p2); sItems.Add(p3); sItems.Add(p4);  sItems.Add(p5); sItems.Add(p6);
            nMessage.DataItems = sItems;
            RaisePropose?.Invoke(this, new ProposeEventArgs() { NMessage = nMessage });
        }
    }

    public class InsertProperty : INotifyPropertyChanged
    {
        public event RaiseProposeEventHandler RaiseProposal2;

        string pName;
        
        public string PropName
        {
            get
            {
                return this.pName;
            }
            set
            {
                this.pName = value;
                OnPropertyChanged("PropName");
            }
        }

        StorePropertyCommand spCommand;
        public StorePropertyCommand SPCommand
        {
            get
            {
                if (spCommand == null)
                {
                    spCommand = new StorePropertyCommand();
                    spCommand.EventCompleted += SpCommand_EventCompleted;
                    spCommand.RaisePropose += PpCommand_RaisePropose;
                }
                return spCommand;
            }
        }

        ProposePropertyCommand ppCommand;

        public ProposePropertyCommand PPCommand
        {
            get
            {
                if(ppCommand == null)
                {
                    ppCommand = new ProposePropertyCommand();
                    ppCommand.RaisePropose += PpCommand_RaisePropose;
                }
                return ppCommand;
            }
        }

       

        List<string> typesForProperties;

        public List<string> TypesForProperties
        {
            get
            {
                return this.typesForProperties;
            }
            set
            {
                this.typesForProperties = value;
                OnPropertyChanged("TypesForProperties");
            }
        }

        string prType;
        public string PropertyType
        {
            get
            {
                return this.prType;
            }
            set
            {
                this.prType = value;
                OnPropertyChanged("PropertyType");
            }
        }


        List<string> cntClasses;
        public List<string> ContainerClasses
        {
            get
            {
                return this.cntClasses;
            }
            set
            {
                this.cntClasses = value;
                OnPropertyChanged("ContainerClass");
            }
        }

        string ctClass;
        public string ContainerClass
        {
            get
            {
                return this.ctClass;
            }
            set
            {
                this.ctClass = value;
                OnPropertyChanged("ContainerClass");
            }

        }

        List<string> cntETUris;
        public List<string> ETURIs
        {
            get
            {
                return this.cntETUris;
            }
            set
            {
                this.cntETUris = value;
                OnPropertyChanged("ETURIs");
            }
        }

        string etURI;
        public string EtURI
        {
            get
            {
                return this.etURI;
            }
            set
            {
                this.etURI = value;
                OnPropertyChanged("EtURI");
            }

        }


        string prNotes;
        public string PrNotes
        {
            get
            {
                return this.prNotes;
            }

            set
            {
                this.prNotes = value;
                OnPropertyChanged("ETNotes");
            }
        }

        string curUsrName;
        public string CurUsrName
        {
            get
            {
                return this.curUsrName;
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
        public DBData CurDBData
        {
            get
            {
                return this.curDBData;
            }
        }

        public InsertProperty()
        {

        }
        public InsertProperty(string uName, DBData cData)
        {
            this.ProposalStatus = "Changes not proposed";
            this.ProposalState = false;
            this.curUsrName = uName;
            this.curDBData = cData;
            typesForProperties = new List<string>();
            cntClasses = new List<string>();
            this.TypesForProperties = GetAllTypesForProperties();
            this.ContainerClasses = GetAllClassDetails();
            this.ETURIs = GetAllNameSpaces();
        }

        public List<string> GetAllTypesForProperties()
        {
            List<string> cDetails = new List<string>();
            try
            {
                foreach (EntityData eData in curDBData.ClassData)
                {
                    cDetails.Add(string.Format("{0}/{1}", eData.ETURIName, eData.ETName));
                }
                foreach (EntityData eData in curDBData.PropertyData)
                {
                    cDetails.Add(string.Format("{0}/{1}", eData.ETURIName, eData.ETName));
                }
                foreach (EntityData eData in curDBData.EnumData)
                {
                    cDetails.Add(string.Format("{0}/{1}", eData.ETURIName, eData.ETName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Exception while getting namespace details from entities and masterdetails. Details are {0}", ex.Message));
            }
            return cDetails;
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
                foreach (PackageData pData in curDBData.PkgData)
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

        private void PpCommand_RaisePropose(object sender, ProposeEventArgs e)
        {
            this.ProposalStatus = "Proposal Started";
            RaiseProposal2?.Invoke(this, e);
        }
        
        public void TransitDone()
        {
            this.PropName = string.Empty;
            this.PropertyType = string.Empty;
            this.ContainerClass = string.Empty;
            this.PrNotes = string.Empty;
        }

        private void SpCommand_EventCompleted(object sender, EventArgs e)
        {
            SaveCompleteEventArgs scEventArgs = e as SaveCompleteEventArgs;
            if (scEventArgs.EvntMsg.Equals("PropertyName"))
            {
                MessageBoxResult result = MessageBox.Show("Provide Property Name", "Error", MessageBoxButton.OK);
                this.PropName = string.Empty;
                this.PropertyType = string.Empty;
                this.ContainerClass = string.Empty;
                this.PrNotes = string.Empty;
            }
            else if (scEventArgs.EvntMsg.Equals("PropertyType"))
            {
                MessageBoxResult result = MessageBox.Show("Provide Property Type", "Error", MessageBoxButton.OK);
                this.PropName = string.Empty;
                this.PropertyType = string.Empty;
                this.ContainerClass = string.Empty;
                this.PrNotes = string.Empty;
            }
            else if (scEventArgs.EvntMsg.Equals("ContainerEntity"))
            {
                MessageBoxResult result = MessageBox.Show("Provide Container Class or Namespace One is must", "Error", MessageBoxButton.OK);
                this.PropName = string.Empty;
                this.PropertyType = string.Empty;
                this.ContainerClass = string.Empty;
                this.PrNotes = string.Empty;
            }
            else if (scEventArgs.Equals("UserName"))
            {
                MessageBoxResult result = MessageBox.Show("Provide User Name", "Error", MessageBoxButton.OK);
                this.PropName = string.Empty;
                this.PropertyType = string.Empty;
                this.ContainerClass = string.Empty;
                this.PrNotes = string.Empty;
            }
            else if (scEventArgs.EvntMsg.Equals("Done"))
            {
                MessageBoxResult result = MessageBox.Show("Saved New Property Values", "Success", MessageBoxButton.OK);
                this.PropName = string.Empty;
                this.PropertyType = string.Empty;
                this.ContainerClass = string.Empty;
                this.PrNotes = string.Empty;
            }
            else if (scEventArgs.EvntMsg.Equals("Failed"))
            {
                MessageBoxResult result = MessageBox.Show("Insertion of Property values Failed", "Error", MessageBoxButton.OK);
                this.PropName = string.Empty;
                this.PropertyType = string.Empty;
                this.ContainerClass = string.Empty;
                this.PrNotes = string.Empty;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
