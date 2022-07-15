using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoB.ToolUtilities.OpenDSSParser;

namespace UoB.ToolUtilities.DataStructure
{
    public class DSSScriptWriter
    {
        public string DirPath;
        Dictionary<string, int> transformers;
        Dictionary<string, int> lines;
        List<string> monInstructions;
        List<string> monNames;
        public List<string> MonitorNames
        {
            get { return monNames; }
        }
        public DSSScriptWriter()
        {
            DirPath = string.Empty;
            monInstructions = new List<string>();
            monNames = new List<string>();
        }
        public DSSScriptWriter(string dPath) : this()
        {
            this.DirPath = dPath;
            this.transformers = null;
            this.lines = null;
        }
        public DSSScriptWriter(string dPath, Dictionary<string, int> tTransformers, Dictionary<string, int> tLines) : this()
        {
            this.DirPath = dPath;
            this.transformers = tTransformers;
            this.lines = tLines;
        }

        public string CreateAndPlaceMonitorScript(bool forPVMonitoring)
        {
            string currentDate = DateUtilities.GetCurrentDate();//DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            foreach (string tName in transformers.Keys)
            {
                string transName = string.Format("{0}_{1}", tName, currentDate);

                monInstructions.Add(string.Format("New Monitor.{0}_VI_t1 element=Transformer.{1} terminal=1 mode=0 enabled = yes", transName, tName));
                monNames.Add(string.Format("{0}_VI_t1", transName));
                monInstructions.Add(string.Format("New Monitor.{0}_PQ_t1 element=Transformer.{1} terminal=1 mode=1 enabled = yes", transName, tName));
                monNames.Add(string.Format("{0}_PQ_t1", transName));
                monInstructions.Add(string.Format("New Monitor.{0}_Tap_t1 element=Transformer.{1} terminal=1 mode=2 enabled = yes", transName, tName));
                monNames.Add(string.Format("{0}_Tap_t1", transName));
                monInstructions.Add(string.Format("New Monitor.{0}_VI_t2 element=Transformer.{1} terminal=2 mode=0 enabled = yes", transName, tName));
                monNames.Add(string.Format("{0}_VI_t2", transName));
                monInstructions.Add(string.Format("New Monitor.{0}_PQ_t2 element=Transformer.{1} terminal=2 mode=1 enabled = yes", transName, tName));
                monNames.Add(string.Format("{0}_PQ_t2", transName));
                monInstructions.Add(string.Format("New Monitor.{0}_Tap_t2 element=Transformer.{1} terminal=2 mode=2 enabled = yes", transName, tName));
                monNames.Add(string.Format("{0}_Tap_t2", transName));
                //THESE ARE V and I but not useful
                //monInstructions.Add(string.Format("New Monitor.{0}_VI2_t1 element=Transformer.{1} terminal=1 mode=112 enabled = yes", transName, tName));
                //monNames.Add(string.Format("{0}_VI2_t1", transName));
                //monInstructions.Add(string.Format("New Monitor.{0}_VI2_t2 element=Transformer.{1} terminal=2 mode=112 enabled = yes", transName, tName));
                //monNames.Add(string.Format("{0}_VI2_t2", transName));
            }
            foreach (string tLine in lines.Keys)
            {
                string lineName = string.Format("{0}_{1}", tLine, currentDate);
                monInstructions.Add(string.Format("New Monitor.{0}_VI_t1  element=line.{1} mode=0 Residual=Yes terminal=1", lineName, tLine));
                monNames.Add(string.Format("{0}_VI_t1", lineName));
                monInstructions.Add(string.Format("New Monitor.{0}_PQ_t1  element=line.{1} mode=1 Residual=Yes terminal=1", lineName, tLine));
                monNames.Add(string.Format("{0}_PQ_t1", lineName));
                monInstructions.Add(string.Format("New Monitor.{0}_VI_t2  element=line.{1} mode=0 Residual=Yes terminal=2", lineName, tLine));
                monNames.Add(string.Format("{0}_VI_t2", lineName));
                monInstructions.Add(string.Format("New Monitor.{0}_PQ_t2  element=line.{1} mode=1 Residual=Yes terminal=2", lineName, tLine));
                monNames.Add(string.Format("{0}_PQ_t2", lineName));
            }
            string destPath = string.Empty;
            if (!forPVMonitoring)
            {
                string monFileName = string.Format("{0}{1}.dss", "Monitors_", currentDate);
                destPath = string.Format("{0}\\{1}", DirPath, monFileName);
                if (File.Exists(destPath))
                    File.Delete(destPath);
                File.WriteAllLines(destPath, monInstructions);
                PlaceMonitorInMasterFile(monFileName, forPVMonitoring);
            }
            else
            {
                string monFileName = string.Format("{0}{1}_PV.dss", "Monitors_", currentDate);
                destPath = string.Format("{0}\\{1}", DirPath, monFileName);
                if (File.Exists(destPath))
                    File.Delete(destPath);
                File.WriteAllLines(destPath, monInstructions);
                PlaceMonitorInMasterFile(monFileName, forPVMonitoring);
            }
            return string.Format("{0} Monitors are created for {1} transformers and {2} lines. The {3} is saved. {4}. Master file updated", monInstructions.Count(), transformers.Keys.Count(),
                                    lines.Keys.Count, destPath, Environment.NewLine);
        }

        void PlaceMonitorInMasterFile(string monFileName, bool forPV)
        {
            List<string> lines = new List<string>();
            string masterFile = string.Format("{0}\\{1}", DirPath, Utilities.MasterFileName);
            string line = string.Empty;
            //Insert lines
            foreach (string line1 in File.ReadLines(masterFile))
            {
                if (!forPV)
                {
                    if (line1.Contains("Monitors"))
                    {
                        continue;
                    }
                    lines.Add(line1);
                    if (line1.Contains(Utilities.MonitorEntryPoint))
                    {
                        lines.Add("!Define the monitors");
                        lines.Add(string.Format("Redirect {0}", monFileName));
                    }
                }
                else
                {
                    if (line1.Contains("Redirect Monitors_"))
                    {
                        continue;
                    }
                    lines.Add(line1);
                    if (line1.Contains(Utilities.MonitorEntryPoint))
                    {
                        lines.Add("!Define the monitors");
                        lines.Add(string.Format("Redirect {0}", monFileName));
                    }
                }
            }
            //Saving new lines
            if (File.Exists(masterFile))
                File.Delete(masterFile);
            File.WriteAllLines(masterFile, lines);

        }

        public void CreatePVLoadShapeFile(string fileName, List<double> values)
        {
            string lsFile = string.Format("{0}\\{1}", DirPath, fileName);
            if (File.Exists(lsFile))
                File.Delete(lsFile);
            List<string> sVals = new List<string>();
            foreach (double dVal in values)
            {
                sVals.Add(dVal.ToString());
            }
            File.WriteAllLines(lsFile, sVals);
        }

        public void CreatePVLoadFile(List<GraphNode> pvGNode, string pvlFileName, string pvlsFileName, int lineCount)
        {
            string pvFile = string.Format("{0}\\{1}", DirPath, pvlFileName);
            List<string> pvFileLines = new List<string>();
            pvFileLines.Add(string.Format("new loadshape.PV_Loadshape npts={0} sinterval=2 csvfile={1} Pbase=7.50 action=normalize", lineCount, pvlsFileName));
            foreach (GraphNode gNode in pvGNode)
            {
                PVSystem pvNode = gNode as PVSystem;
                pvFileLines.Add(string.Format("new pvsystem.{0} bus1={1} irradiance={2} phases={3} kv={4} kVA={5} pf={6} pmpp={7} duty=PV_Loadshape",
                                                pvNode.Name, pvNode.Bus1, pvNode.Irradiance, pvNode.Phases, pvNode.kV, pvNode.kVA, pvNode.PF, pvNode.Pmpp));
            }

            if (File.Exists(pvFile))
                File.Delete(pvFile);
            File.WriteAllLines(pvFile, pvFileLines);
        }
    }

    public static class DateUtilities
    {
        public static string GetCurrentDate()
        {
            string currentDate = string.Empty;
            if (DateTime.Now.Month <= 9)
                if (DateTime.Now.Day <= 9)
                    currentDate = "0" + DateTime.Now.Day.ToString() + "-" + "0" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
                else
                    currentDate = DateTime.Now.Day.ToString() + "-" + "0" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
            else
                currentDate = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
            return currentDate;
        }
    }

    public class MatLabScriptWriter2
    {
        string dPath;
        List<string> monNames;
        string currentDate;
        List<string> mlInst;
        public MatLabScriptWriter2()
        {
            currentDate = DateUtilities.GetCurrentDate();//DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            dPath = string.Empty;
            monNames = new List<string>();
            mlInst = new List<string>();
        }
        public MatLabScriptWriter2(string dP, List<string> mNames) : this()
        {
            this.dPath = dP;
            this.monNames = mNames;
        }
        public string WriteScript()
        {
            string subDirName = string.Format("MatlabAnlaysis_{0}", currentDate);
            string subDirPath = dPath + "\\" + subDirName;
            string masterFilePath = dPath + "\\" + Utilities.MasterFileName;
            //CreateDir for Matlab
            if (!Directory.Exists(subDirPath))
                Directory.CreateDirectory(subDirPath);
            //start writing instructions
            //Standard
            mlInst.Add("function TimeseriesAnalyses");
            mlInst.Add("[DSSCircObj, DSSText, gridpvPath] = DSSStartup;");
            mlInst.Add("DSSCircuit = DSSCircObj.ActiveCircuit;");
            mlInst.Add(string.Format(@"DSSText.command = 'Compile {0}';", masterFilePath));
            mlInst.Add("DSSText.command = 'Set mode=duty number=1440 hour=0 h=60 sec=0';");
            mlInst.Add("DSSText.Command = 'Set Controlmode=TIME';");
            mlInst.Add("DSSText.command = 'solve';");
            //All Monitors
            foreach (string mName in monNames)
            {
                mlInst.Add(string.Format("DSSText.Command = 'export mon {0}';", mName));
                mlInst.Add("monitorFile = DSSText.Result; ");
                mlInst.Add("MyCSV = importdata(monitorFile);");
            }
            //End
            mlInst.Add("end");
            //Special date is required as maptlab dont accept names with - in them.
            string currentDate1 = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            string mlFileName = string.Format("{0}{1}.m", "TimeSimulation_", currentDate1);
            string destPath = string.Format("{0}\\{1}", subDirPath, mlFileName);
            if (File.Exists(destPath))
                File.Delete(destPath);
            File.WriteAllLines(destPath, mlInst);
            return string.Format("Mat Lab Script is created in {0} with Time series Analysis for Circuit and exporting data {1} Monitors", subDirPath, monNames.Count);
        }
        public string WritePVScript(int maxLNum, string pvLFileName)
        {
            string subDirName = string.Format("MatlabPVAnlaysis_{0}", currentDate);
            string subDirPath = dPath + "\\" + subDirName;
            string masterFilePath = dPath + "\\" + Utilities.MasterFileName;
            string pvFileName = dPath + "\\" + pvLFileName;
            //CreateDir for Matlab
            if (!Directory.Exists(subDirPath))
                Directory.CreateDirectory(subDirPath);
            //start writing instructions
            //Standard
            mlInst.Add("function PeakTimeAnalysis()");
            mlInst.Add(string.Format("maxTimeIndex = {0};", maxLNum));
            mlInst.Add("[DSSCircObj, DSSText, gridpvPath] = DSSStartup;");
            mlInst.Add("DSSCircuit = DSSCircObj.ActiveCircuit;");
            mlInst.Add(string.Format(@"caseNames = ['{0}',{1}'{2}'{3}]; ", masterFilePath, "{", pvFileName, "}"));
            mlInst.Add("for jj=1:length(caseNames)");
            mlInst.Add("%% Call OpenDSS compile");
            mlInst.Add("DSSfilename = caseNames{jj};");
            mlInst.Add("location = cd;");
            mlInst.Add("DSSText.command = sprintf('Compile (%s)',caseNames{1}); %run basecase first");
            mlInst.Add("DSSText.command = 'solve';");
            mlInst.Add("cd(location);");
            mlInst.Add("DSSText.command = sprintf('Compile (%s)',DSSfilename); %add solar scenario");
            mlInst.Add("DSSText.command = 'solve';");
            mlInst.Add("cd(location);");
            mlInst.Add("%% Run the simulation in static mode for the peak time");
            mlInst.Add("DSSText.command = sprintf('Set mode=duty number=1  hour=%i  h=1.0 sec=%i',floor((maxTimeIndex)/3600),round(mod(maxTimeIndex,3600)));");
            mlInst.Add("DSSText.Command = 'Set Controlmode=Static'; %take control actions immediately without delays");
            mlInst.Add("DSSText.command = 'solve';");
            //End for loop
            mlInst.Add("end");
            mlInst.Add("");
            //All Monitors
            foreach (string mName in monNames)
            {
                mlInst.Add(string.Format("DSSText.Command = 'export mon {0}';", mName));
                mlInst.Add("monitorFile = DSSText.Result; ");
                mlInst.Add("MyCSV = importdata(monitorFile);");
            }
            //End function
            mlInst.Add("end");
            //Special date is required as maptlab dont accept names with - in them.
            string currentDate1 = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            string mlFileName = string.Format("{0}{1}.m", "PeakAnalysis_", currentDate1);
            string destPath = string.Format("{0}\\{1}", subDirPath, mlFileName);
            if (File.Exists(destPath))
                File.Delete(destPath);
            File.WriteAllLines(destPath, mlInst);
            return subDirPath;// string.Format("Mat Lab Script is created in {0} with Time series Analysis for Circuit and exporting data {1} Monitors", subDirPath, monNames.Count);
        }
    }
}
