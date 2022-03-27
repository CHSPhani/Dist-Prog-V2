using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoB.ToolUtilities.OpenDSSParser;

namespace SimulationEngine.SimulationHelper
{
    public class MatLabScriptWriter
    {
        string dPath;
        List<string> monNames;
        string currentDate;
        List<string> mlInst;
        string subDirPath;
        string mlFileName;

        public string MatFileName
        {
            get
            {
                return this.mlFileName;
            }
        }
        public string SubDirPath
        {
            get
            {
                return this.subDirPath;
            }
        }
        public MatLabScriptWriter()
        {
            subDirPath = string.Empty;
            currentDate = DateUtilities.GetCurrentDate();//DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            dPath = string.Empty;
            monNames = new List<string>();
            mlInst = new List<string>();
        }
        public MatLabScriptWriter(string dP, List<string> mNames) : this()
        {
            this.dPath = dP;
            this.monNames = mNames;
        }
        public string WriteScript()
        {
            string subDirName = string.Format("MatlabAnlaysis_{0}", currentDate);
            subDirPath = dPath + "\\" + subDirName;
            string masterFilePath = dPath + "\\" + Utilities.MasterFileName;
            //CreateDir for Matlab
            if (!Directory.Exists(subDirPath))
                Directory.CreateDirectory(subDirPath);
            //start writing instructions
            //Standard
            mlInst.Add("function[] = TimeseriesAnalyses()");//function TimeseriesAnalyses");
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
            mlFileName = string.Format("{0}{1}.m", "TimeSimulation_", currentDate1);
            string destPath = string.Format("{0}\\{1}", subDirPath, mlFileName);
            if (File.Exists(destPath))
                File.Delete(destPath);
            File.WriteAllLines(destPath, mlInst);
            return string.Format("Mat Lab Script is created in {0} with Time series Analysis \n for Circuit and exporting data {1} Monitors", subDirPath, monNames.Count);
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
            mlFileName = string.Format("{0}{1}.m", "PeakAnalysis_", currentDate1);
            string destPath = string.Format("{0}\\{1}", subDirPath, mlFileName);
            if (File.Exists(destPath))
                File.Delete(destPath);
            File.WriteAllLines(destPath, mlInst);
            return subDirPath;// string.Format("Mat Lab Script is created in {0} with Time series Analysis for Circuit and exporting data {1} Monitors", subDirPath, monNames.Count);
        }
    }
}
