using Microsoft.Glee.Drawing;
using Microsoft.Win32;
using Server.DSystem;
using Server.KnowledgeGraph;
using Server.RDFGraph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;

namespace Server.Models
{
    public class SStartDataConverter : IMultiValueConverter
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

    public class StartSystemCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event SystemStartedEventHandler isuEventHandler;
        public event InitialSetupEventHandler isEventHandler;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;

            string p1, p2 = string.Empty;
            int p3 = 0;
            int p5 = 0;
            bool p4 = false;

            if (values[0] != null)
                p1 = values[0].ToString();
            else
                p1 = string.Empty;

            if (values[1] != null)
                p2 = values[1].ToString();
            else
                p2 = string.Empty;

            if (values[2] != null)
                p3 = Int32.Parse(values[2].ToString());

            if (values[3] != null)
                if (values[3].ToString().ToLower().Equals("false"))
                    p4 = false;
                else
                    p4 = true;
            if (values[4] != null)
                p5 = Int32.Parse(values[4].ToString());

            StartSystem sSystem = new StartSystem(p1, p2, p3, p5, p4);
            sSystem.SsEventHandler += SSystem_isEventHandler;
            sSystem.IsuEventHandler += SSystem_IsuEventHandler;
            sSystem.SystemStart();
        }

        private void SSystem_IsuEventHandler(object sender, InitSetupEventArgs e)
        {
            isEventHandler?.Invoke(sender, e);
        }

        private void SSystem_isEventHandler(object sender, SystemStartEventArgs e)
        {
            isuEventHandler?.Invoke(sender, e);
        }
    }

    public class KGCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            KGConsole kgConsole = new KGConsole();
            kgConsole.Show();
        }
    }

    public class RDFGCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var values = (object[])parameter;
            List<DSNode> ANodes = new List<DSNode>();

            if (values[0] != null)
                ANodes = values[0] as List<DSNode>;

            if (ANodes == null | ANodes.Count == 0)
            {

            }
            else
            {
                try
                {
                    RDFG rdfgCls = new RDFG(ANodes);

                    FormRDFGraph fRdfGr = new FormRDFGraph(rdfgCls.RDFSymbGraph);

                    fRdfGr.ShowDialog();
                }
                catch (Exception ex)
                {
                }
            }
        }

    }

    public class MWModel :INotifyPropertyChanged
    {
        public string WorkingDir { get; set; }

        public string DataDir { get; set; }
        
        public int NoOfUser { get; set; }

        public int NoOfAUser { get; set; }
        public bool InitSetupDone { get; set; }

        public string sysStatus;

        public string SysStatus
        {
            get
            {
                return this.sysStatus;
            }
            set
            {
                this.sysStatus = value;
                OnPropertyChanged("SysStatus");
            }
        }

        public List<DSNode> aNodes;

        public List<DSNode> ActiveNodes
        {
            get
            {
                return this.aNodes;
            }
            set
            {
                this.aNodes = value;
                OnPropertyChanged("ActiveNodes");
            }
        }

        StartSystemCommand ssCommand;

        public ICommand SSCommand
        {
            get
            {
                if(ssCommand == null)
                {
                    ssCommand = new StartSystemCommand();
                    ssCommand.isuEventHandler += SsCommand_isuEventHandler;
                    ssCommand.isEventHandler += SsCommand_isEventHandler;
                }
                return ssCommand;
            }
        }

        KGCommand kgrCommand;

        public ICommand KGrCommand
        {
            get
            {
                if(kgrCommand == null)
                {
                    kgrCommand = new KGCommand();
                }
                return kgrCommand;
            }
        }

        RDFGCommand rgrCommand;

        public ICommand RGrCommand
        {
            get
            {
                if(rgrCommand == null)
                {
                    rgrCommand = new RDFGCommand();
                }
                return rgrCommand;
            }
        }
        public object ValidateFile { get; private set; }

        private void SsCommand_isEventHandler(object sender, InitSetupEventArgs e)
        {
            string isVal = e.InitArgs as string; 
            if(isVal.Equals("true"))
            {
                this.InitSetupDone = true;
            }
            else
            {

            }
        }

        private void SsCommand_isuEventHandler(object sender, SystemStartEventArgs e)
        {
            this.ActiveNodes = e.ActiveNodes;
            string statusMsg = string.Format("System Status: {0} With Number of Nodes {1}", e.StatusMessage.ToString(),ActiveNodes.Count);
            this.SysStatus = statusMsg;
            DSNode dsn = this.ActiveNodes.Find((a) => { if (a.UserName.Equals("AUser")) { return true; } else { return false; } });
            if (dsn != null)
            {
                Server.ValidationService.ValidateFiles.dbData = dsn.DataInstance;
                Models.KGConsoleModel.dbData = dsn.DataInstance;
                Server.UploadIndividuals.UploadIndividualsToRDFG.dbData = dsn.DataInstance;
                Server.UploadIndividuals.ObtainAllIndies.dbData = dsn.DataInstance;
                Server.UploadIndividuals.ObtainSSDetails.dbData = dsn.DataInstance;
                Server.UploadIndividuals.ObtainSSForInstn.dbData = dsn.DataInstance;
                Server.UploadIndividuals.ObtainAllLoadIndies.dbData = dsn.DataInstance;
                Server.UploadIndividuals.AddNewUserRole.dsn= dsn;
            }
        }

        public MWModel()
        {
            this.SysStatus = "NotStarted";
            this.ActiveNodes = null;
            this.ActiveNodes = new List<DSNode>(); 
            LoadSMSettings();
        }
           
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void LoadSMSettings()
        {
            string configDataFolder = @"C:\WorkRelated-Offline\Dist_Prog_V2\ResMngNetwork\Server\ConfigData\ConfigData.txt";
            string[] lines = System.IO.File.ReadAllLines(configDataFolder);

            foreach (string line in lines)
            {
                string[] smData = line.Split('=');
                if (smData[0].ToLower().Equals("workingdir"))
                {
                    this.WorkingDir = smData[1];
                }
                if (smData[0].ToLower().Equals("datadir"))
                {
                    this.DataDir = smData[1];
                }
                else if (smData[0].ToLower().Equals("noofusers"))
                {
                    this.NoOfUser = Int32.Parse(smData[1]);
                }
                else if (smData[0].ToLower().Equals("noofauser"))
                {
                    this.NoOfAUser = Int32.Parse(smData[1]);
                }
                else if (smData[0].ToLower().Equals("initsetup"))
                {
                    if (Int32.Parse(smData[1]) == 0)
                    {
                        this.InitSetupDone = false;
                    }
                    else
                    {
                        this.InitSetupDone = true;
                    }
                }
            }            
        }
        public void SaveSMSettings()
        {
            string configDataFolder = @"C:\WorkRelated-Offline\Dist_Prog_V2\ResMngNetwork\Server\ConfigData\ConfigData.txt";

            if (File.Exists(configDataFolder))
                File.Delete(configDataFolder);

            using (TextWriter tw = new StreamWriter(configDataFolder))
            {
                tw.WriteLine(string.Format("WorkingDir={0}", this.WorkingDir));
                tw.WriteLine(string.Format("DataDir={0}", this.DataDir));
                tw.WriteLine(string.Format("NoOfUsers={0}", this.NoOfUser));
                tw.WriteLine(string.Format("NoOfAUser={0}", this.NoOfAUser));
                if (this.InitSetupDone)
                    tw.WriteLine(string.Format("InitSetup={0}", 1));
                else
                    tw.WriteLine(string.Format("InitSetup={0}", 0));
            }
        }
    }
}


/*
 * 
 * 
 * KG kg = new KG(ANodes);
                    KGraphDS gDs = kg.KGraph;//for answering question

                    KnowledgeGraph.KnowledgeGraph kGr = new KnowledgeGraph.KnowledgeGraph(kg.SymbolicGraph);  //kg.SymbolicGraph for presentation

                    kGr.ShowDialog();
 * 
    public class FolderSelectedEventArgs : EventArgs
    {
        public string FolderName { get; set; }

        public FolderSelectedEventArgs()
        {
            FolderName = string.Empty;
        }
    }
 *   public class OpenDialogCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public event EventHandler EventCompleted;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            FolderBrowserDialog diag = new FolderBrowserDialog();
            if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selFolderName = diag.SelectedPath;
                EventHandler handler = EventCompleted;
                handler?.Invoke(this, new FolderSelectedEventArgs() { FolderName = selFolderName });
                return;
            }
        }
    }
      OpenDialogCommand ofdCmd;
        public OpenDialogCommand OfdCmd
        {
            get
            {
                if(ofdCmd == null)
                {
                    ofdCmd = new OpenDialogCommand();
                    ofdCmd.EventCompleted += OfdCmd_EventCompleted;
                }
                return ofdCmd;
            }
        }
         private void OfdCmd_EventCompleted(object sender, EventArgs e)
        {
            FolderSelectedEventArgs fsArgs = e as FolderSelectedEventArgs;
            this.SelectedFolder = fsArgs.FolderName;
        }
 * */
