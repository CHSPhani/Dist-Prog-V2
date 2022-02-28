using DataSerailizer;
using Server.DataFileProcess;
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
    public class ProcessResultEventArgs : EventArgs
    {
        public string EventMsg;

        public IFileProcessResult PResult;

        public ProcessResultEventArgs()
        {
            EventMsg = string.Empty;
            PResult = null;
        }
    }

    public class ValidateResultEventARgs : EventArgs
    {
        public string EvntMsg { get; set; }

        public ValidationResult VResult { get; set; }

        public ValidateResultEventARgs()
        {
            EvntMsg = string.Empty;
            VResult = new ValidationResult();
        }
    }

    public class ProcessFileCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event EventHandler EventCompleted;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            string p0 = string.Empty;

            if (values[0] != null)
                p0 = values[0].ToString();
            else
                p0 = string.Empty;

            if (string.IsNullOrEmpty(p0))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new ProcessResultEventArgs() { EventMsg = "SelFileName", PResult = null });
                return;
            }
            else
            {
                //Process file
                DataFilesProcess dfProcess = new DataFilesProcess(p0);
                IFileProcessResult pRes = dfProcess.ProcessFile();
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new ProcessResultEventArgs() { EventMsg = "Result", PResult = pRes });
            }
        }
    }

    public class ValidateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event EventHandler EventCompleted;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string dataSet = values[0] as string;
            IFileProcessResult fpResult = values[1] as IFileProcessResult;
            DBData dbData = values[2] as DBData;
            bool adValue = false;
            Boolean.TryParse(values[3].ToString(), out adValue);
            if (string.IsNullOrEmpty(dataSet))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new ValidateResultEventARgs() { EvntMsg = "PredefinedDS" });
                return;
            }
            else if (fpResult == null)
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new ValidateResultEventARgs() { EvntMsg = "ResultNull" });
                return;
            }
            else if (dbData == null)
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new ValidateResultEventARgs() { EvntMsg = "DBData" });
                return;
            }
            else
            {
                //Process and Return Validation result
                if (fpResult.FileType == AllowedFileTypes.csv)
                {
                    ValidationResult vR = ValidateDataSets.ValidateCSVDataSet(dbData, fpResult, dataSet, adValue);
                    EventHandler handler = EventCompleted;
                    handler?.Invoke(this, new ValidateResultEventARgs() { EvntMsg = "Processed", VResult = vR });
                    return;
                }
            }
        }
    }

    public class ValidatorModel : INotifyPropertyChanged
    {
        bool igColNames;
        public bool IgColName
        {
            get
            {
                return igColNames;
            }
            set
            {
                igColNames = value;
                OnPropertyChanged("IgColName");
            }
        }

        bool perDetails;
        public bool PerDetails
        {
            get
            {
                return this.perDetails;
            }
            set
            {
                this.perDetails = value;
                OnPropertyChanged("PerDetails");
            }
        }

        bool acceptDValues;
        public bool AcceptDefValues
        {
            get
            {
                return this.acceptDValues;
            }
            set
            {
                this.acceptDValues = value;
                OnPropertyChanged("AcceptDefValues");
            }
        }

        bool considerSS;
        public bool ConsiderSubSet
        {
            get
            {
                return this.considerSS;
            }
            set
            {
                this.considerSS = value;
                OnPropertyChanged("ConsiderSubSet");
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

        string selFileName;
        public string SelFileName
        {
            get
            {
                return this.selFileName;
            }
            set
            {
                this.selFileName = value;
                OnPropertyChanged("SelFileName");
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

        IFileProcessResult pResult;
        public IFileProcessResult PResult
        {
            get
            {
                return this.pResult;
            }
            set
            {
                this.pResult = value;
                OnPropertyChanged("PResult");
            }
        }

        string vResult;
        public string ValdiationResult
        {
            get
            {
                return this.vResult;
            }
            set
            {
                this.vResult = value;
                OnPropertyChanged("ValdiationResult");
            }
        }

        ProcessFileCommand pfCommand;
        public ICommand PFCommand
        {
            get
            {
                if (pfCommand == null)
                {
                    pfCommand = new ProcessFileCommand();
                    pfCommand.EventCompleted += PfCommand_EventCompleted;
                }
                return pfCommand;
            }
        }

        private void PfCommand_EventCompleted(object sender, EventArgs e)
        {
            ProcessResultEventArgs pArgs = e as ProcessResultEventArgs;
            if (pArgs.EventMsg == "SelFileName")
            {
                MessageBoxResult result = MessageBox.Show("Select  File", "Error", MessageBoxButton.OK);
            }
            else if (pArgs.EventMsg == "Result")
            {
                //MessageBoxResult result = MessageBox.Show("File Processed", "Success", MessageBoxButton.OK);
                //How to get the which file is processed???
                //Hard coded as of now
                PResult = pArgs.PResult;  
            }
        }

        ValidateCommand vCommand;

        public ICommand VCommand
        {
            get
            {
                if (vCommand == null)
                {
                    vCommand = new ValidateCommand();
                    vCommand.EventCompleted += VCommand_EventCompleted;
                }
                return vCommand;
            }
        }

        private void VCommand_EventCompleted(object sender, EventArgs e)
        {
            ValidateResultEventARgs sArgs = e as ValidateResultEventARgs;
            if (sArgs.EvntMsg == "ResultNull")
            {
                MessageBoxResult result = MessageBox.Show("Process Result is null", "Error", MessageBoxButton.OK);
            }
            else if (sArgs.EvntMsg == "PredefinedDS")
            {
                MessageBoxResult result = MessageBox.Show("Select Predefined data set", "Error", MessageBoxButton.OK);
            }
            else if (sArgs.EvntMsg == "DMClass")
            {
                MessageBoxResult result = MessageBox.Show("Data Set values from Ontology is not available.", "Error", MessageBoxButton.OK);
            }
            else
            {
                ValidationResult res = sArgs.VResult;
                this.ValdiationResult = res.Reason;
                if(res.Validated)
                {
                    if(this.PerDetails)
                    {
                        NodeData nData = new NodeData();
                        string[] prts = this.SelFileName.Split('\\');
                        nData.FileName = prts[prts.Length-1];
                        nData.AbsLocation = this.SelFileName;
                        nData.FileType = ((CSVFileProcessResult)PResult).FileType.ToString();
                        nData.ColNames = ((CSVFileProcessResult)PResult).ColNames;
                        nData.NoOfCols = ((CSVFileProcessResult)PResult).NoOfCols;
                        nData.NoOfRows = ((CSVFileProcessResult)PResult).NoOfRows;
                        nData.ProcessResult = ((CSVFileProcessResult)PResult).ProcessResult;
                        nData.VerifiedDataSet = res.OClassName;
                        if (curDbInstance.NodeData == null)
                        {
                            curDbInstance.NodeData = new List<NodeData>();
                            this.CurrentDbInstance.NodeData.Add(nData);
                        }
                        else
                        {
                            this.CurrentDbInstance.NodeData.Add(nData);
                        }
                    }
                }
            }
        }

        public ValidatorModel()
        { }

        public ValidatorModel(string uName, DBData dbData)
        {
            this.CurrentUserName = uName;
            this.curDbInstance = dbData;
            vResult = string.Empty;
            selFileName = string.Empty;
            selectedCN = string.Empty;
            igColNames = false;
            perDetails = false;
            acceptDValues = false;
            this.ConsiderSubSet = true;
            PResult = null;
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
            foreach (ODataProperty odpProp in this.curDbInstance.OwlData.OWLDataProperties)
            {
                var slProps = odpProp.DPChildNodes.FindAll((p) => { if (p.CNType.Equals("rdfs:domain")) return true; else return false; });
                foreach (OChildNode ocNode in slProps)
                {
                    if (ocNode.CNName.Equals(this.SelectedCN))
                        pNs.Add(odpProp.DProperty);
                }
            }
            this.PNames = pNs;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


//public IEnumerable<string> Datasets
//{
//    get
//    {
//        foreach (DMClass dmC in this.dmClasses)
//        {
//            if (!string.IsNullOrEmpty(dmC.BaseClassName))
//                yield return dmC.CName;
//        }
//    }
//}