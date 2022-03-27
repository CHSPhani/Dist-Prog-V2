using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationTool.OpenDSSParser
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
    }

}
