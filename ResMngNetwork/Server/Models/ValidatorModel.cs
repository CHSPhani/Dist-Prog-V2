using DataSerailizer;
using Server.DataFileProcess;
using Server.DSystem;
using Server.ValidationService;
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
            string indVal = values[1] as string;
            IFileProcessResult fpResult = values[2] as IFileProcessResult;
            DBData dbData = values[3] as DBData;
            bool adValue = false;
            Boolean.TryParse(values[4].ToString(), out adValue);
            bool iCols = false;
            Boolean.TryParse(values[5].ToString(), out iCols);

            //if (string.IsNullOrEmpty(dataSet) && string.IsNullOrEmpty(indVal))
            //{
            //    EventHandler handler = EventCompleted;
            //    handler?.Invoke(this, new ValidateResultEventARgs() { EvntMsg = "PredefinedDS or Individual" });
            //    return;
            //}
            //else 
            if (fpResult == null)
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new ValidateResultEventARgs() { EvntMsg = "ResultNull" });
                return;
            }
            else if (fpResult.ProcesState.Contains("The process cannot access the file"))
            {
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new ValidateResultEventARgs() { EvntMsg = "Can not access file" });
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
                    ValidationResult vR = ValidateDataSets.ValidateCSVDataSet(dbData, indVal, fpResult, dataSet, adValue, iCols);
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

        bool integratedColNames;
        public bool IntegratedColNames
        {
            get
            {
                return this.integratedColNames;
            }
            set
            {
                this.integratedColNames = value;
                OnPropertyChanged("IntegratedColNames");
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

        List<string> indies;

        public List<string> Indies
        {
            get
            {
                return this.indies;
            }
            set
            {
                this.indies = value;
                OnPropertyChanged("Indies");
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
                        nData.AttachedInstance = res.InstName;
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
            foreach (SemanticStructure ss in this.curDbInstance.OwlData.RDFG.NODetails.Values)
            {
                if (ss.SSType == SStrType.Class)
                    clsNames.Add(ss.SSName);
            }
            this.DMClasses = clsNames;
        }

        public void FillProperties()
        {
            List<string> pNs = new List<string>();
            List<string> indS = new List<string>();

            //Step2: Get outgoing and incoming edges for selected class name
            string nName = this.curDbInstance.OwlData.RDFG.GetExactNodeName(this.SelectedCN);

            List<string> outgoing = this.curDbInstance.OwlData.RDFG.GetEdgesForNode(nName);
            List<string> incoming = this.curDbInstance.OwlData.RDFG.GetIncomingEdgesForNode(nName);
            List<string> classHierarchy = new List<string>();

            //sorting outgoing list because i added number whle adding outgoing properties.
            SortedList<int, string> nsList = new SortedList<int, string>();
            foreach (string s in outgoing)
            {
                nsList[Int32.Parse(s.Split('-')[0])] = s.Split('-')[1];
            }
            List<string> nOutgoing = new List<string>();
            foreach (KeyValuePair<int, string> kvp in nsList)
            {
                nOutgoing.Add(string.Format("{0}-{1}", kvp.Key, kvp.Value));
            }
            //sorting done

            int counter = 0;
            while (counter <= nOutgoing.Count - 1)
            {
                string ots = nOutgoing[counter];
                string[] reqParts = ots.Split('-')[1].Split(':');
                if (reqParts[1].ToLower().Equals("class"))
                {
                    //Step3: Calculating data properties up in hierarchies 
                    //Seems like not required to get all properties
                    #region Good Recursive Code for getting up hierarchies
                    string relation = string.Empty;
                    if (this.curDbInstance.OwlData.RDFG.EdgeData.ContainsKey(string.Format("{0}-{1}", nName.Split(':')[0], reqParts[0])))
                        relation = this.curDbInstance.OwlData.RDFG.EdgeData[string.Format("{0}-{1}", nName.Split(':')[0], reqParts[0])];

                    //if sub class I want to add all data properties gathered from all base classes till owl:Thing
                    if (relation.ToLower().Equals("subclassof"))
                    {
                        string source = ots.Split('-')[1];
                        //classHierarchy.Add(source);
                        while (!source.ToLower().Equals("owl:thing"))
                        {
                            classHierarchy.Add(source);
                            List<string> og = this.curDbInstance.OwlData.RDFG.GetEdgesForNode(source);
                            List<string> ic = this.curDbInstance.OwlData.RDFG.GetIncomingEdgesForNode(source);
                            foreach (string ics in ic)
                            {
                                if (ics.Split(':')[1].Equals("DatatypeProperty"))
                                    incoming.Add(ics);
                            }
                            foreach (string sst in og) //must be one only
                                source = sst.Split('-')[1];
                        }
                        classHierarchy.Add(source);
                    }
                    #endregion
                }
                counter++;
            }

            foreach (string s in incoming)
            {
                pNs.Add(s);
            }
            
            foreach(string s in nOutgoing)
            {
                if (s.Split(':')[1].ToLower().Contains("instance"))
                    indS.Add(s); //instance
                else
                    pNs.Add(s);
            }
            this.PNames = pNs;
            this.Indies = indS;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
