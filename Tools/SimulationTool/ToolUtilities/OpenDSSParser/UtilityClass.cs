using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoB.ToolUtilities.OpenDSSParser
{



    public static class Utilities
    {
        //hardcoded. We can design a UI later to get this from user. 
        public static string MasterFileName = @"master_ckt24.dss";
        public static string CapacitorFileName = @"capacitors_ckt24.dss";
        public static string BusFileName = @"buscoords_ckt24.dss";
        public static string AllocationFactorFileName = @"AllocationFactors_Base.txt";
        public static string PVLoadShapeFileName = @"PVloadshape_7_5MW_Distributed.txt";
        public static List<string> TransformerFileNames = new List<string>() { @"transformers_ckt24.dss" }; //, @"stepxfmrs_ckt24.dss" 
        public static List<string> LineFileName = new List<string>() { @"lines_ckt24.dss" }; //, @"sec_serv_ckt24.dss" 
        public static List<string> LoadFileName = new List<string>() { @"Allocated_Loads_ckt24.dss" };
        public static List<string> PVFileName = new List<string>() { @"Ckt24_PV_Distributed_7_5.dss" };
        public static string SubstationBus = "SUBXFMR_LSB";
        public static string MonitorEntryPoint = "Calcvoltagebases";
        public static int PVLoadStartHour = 6; //6 is the start reading of PV load shape
        public static int HalfHourReadings = 1800; //1800 is number of seconds for hour

        public static string RemoveComments(string value)
        {
            if (value.Contains("!"))
            {
                int index = value.IndexOf('!');
                string newValue = value.Substring(0, index - 1);
                return newValue.Trim();
            }
            else
            {
                return value;
            }
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

    public class DataUtilities
    {
        public static string MoveAllCSVFiles(string DPath, bool forPV, ref string SubDirPath)
        {
            //DBInterations dbInt = new DBInterations();
            string currentDate = DateUtilities.GetCurrentDate();//DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();

            //string SubDirPath = string.Empty;
            if (!forPV)
            {
                //if (dbInt.CheckMonitorExtractionDone(currentDate))
                //    return "Files are moved already";
                string SubDirName = string.Format("MatlabAnlaysis_{0}", currentDate);
                SubDirPath = DPath + "\\" + SubDirName;
            }
            else
            {
                string SubDirName = string.Format("MatlabPVAnlaysis_{0}", currentDate);
                SubDirPath = DPath + "\\" + SubDirName;
            }

            if (Directory.Exists(SubDirPath))
            {
                string[] files = System.IO.Directory.GetFiles(SubDirPath);
                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    FileInfo fi = new FileInfo(s);
                    if (fi.Extension.Equals(".csv"))
                        File.Delete(s);
                }
            }
            else
            {
                return string.Format("Source path {0} does not exist!", SubDirPath);
            }
            int moveCounter = 0;
            if (Directory.Exists(SubDirPath))
            {
                string[] files = System.IO.Directory.GetFiles(DPath);
                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    FileInfo fi = new FileInfo(s);
                    if (fi.Extension.Equals(".csv"))
                    {
                        string newPath = SubDirPath + "\\" + fi.Name;
                        File.Move(s, newPath);
                        moveCounter++;
                    }
                }
            }

            //dbInt.SaveMonitorDataExtractStatus(currentDate, 1);
            return string.Format("{0} File are moved for saving from {1} to \n {2}", moveCounter, DPath, SubDirPath);
        }
    }

    public static class DSSObjectNames
    {
        public static Dictionary<string, string> ObjectNames;
        static DSSObjectNames()
        {
            ObjectNames = new Dictionary<string, string>();
            ObjectNames.Add("Circuit", "Circuit");
            ObjectNames.Add("Loadshape", "Loadshape");
            ObjectNames.Add("energymeter", "energymeter");
        }

        //This function checks whether a function exists or not. 
        public static bool IsObjectNameExists(string objName)
        {
            return ObjectNames.ContainsKey(objName);
        }
    }

    public static class LineSegmentParsers
    {
        //Parse kind of Line segment: (0.26841171,0.26841171)
        public static List<double> ParsePerRS(string perRS)
        {
            List<double> values = new List<double>();
            //parse the string 
            string valStr = perRS.Substring(1, perRS.Length - 2);
            string[] vals = valStr.Split(',');
            foreach (string val in vals)
            {
                values.Add(double.Parse(val));
            }
            if (values.Count > 0)
                return values;
            return null;
        }

        //Parse kind of Line segment: mult = (file = LS_PhaseA.txt)
        public static string FileName(string mulVal)
        {
            return (mulVal.Split('=')[1]).Split('=')[1];
        }

        //Parse kind of Line segment: Loadshape.LS_PhaseA
        public static string NameOfObject(string objStr)
        {
            return objStr.Split('.')[1];
        }


        //Parse kind of Line segment: Loadshape.LS_PhaseA
        //Return LS_PhaseA
        //Checks and returns 
        public static string DSSObject(string objStr)
        {
            string objName = objStr.Split('.')[0];
            if (!string.IsNullOrEmpty(objName))
            {
                if (DSSObjectNames.IsObjectNameExists(objName))
                    return objName;
            }
            return string.Empty;
        }

        //Parse kind of Lines Redirect  Wiredata_ckt24.dss
        //returns file names
        public static string GetRedirectFileName(string line)
        {
            return (ShrinkSpacesToOne(line)).Split(' ')[1];
        }

        //This function removes all spaces in between a line and returns
        //Ex: "  Redirect    Wiredata_ckt24.dss  " ==> "Redirect Wiredata_ckt24.dss"
        public static string ShrinkSpacesToOne(string line)
        {
            string refLine = line.Trim();
            int spaceCount = 0;
            StringBuilder newLine = new StringBuilder();
            foreach (char c in refLine)
            {
                if (!Char.IsWhiteSpace(c))
                {
                    if (spaceCount != 0)
                    {
                        spaceCount = 0;
                    }
                    newLine.Append(c);
                }
                else
                {
                    if (spaceCount == 0)
                    {
                        spaceCount++;
                        newLine.Append(' ');
                    }
                }
            }
            return newLine.ToString();
        }
    }
}
