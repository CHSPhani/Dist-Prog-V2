using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoB.ToolUtilities.OpenDSSParser;

namespace SimulationEngine.SimulationHelper
{
    public class SimulationWorker
    {
        List<CircuitEntry> cEntries;
        Dictionary<string, int> Transformers;
        Dictionary<string, int> Lines;
        DSSScriptWriter dssScriptWriter;
        public string DPath;
        public string MSPath;
        public string MFName;
        public SimulationWorker()
        {
            MSPath = string.Empty;
            DPath = string.Empty;
            MFName = string.Empty;
            cEntries = null;
            Transformers = new Dictionary<string, int>();
            Lines = new Dictionary<string, int>();
        }

        public SimulationWorker(List<CircuitEntry> ces, string fPath) : this()
        {
            this.cEntries = ces;
            this.DPath = fPath;
        }


        /// <summary>
        /// Create Monitors for Circuit elements.
        /// Lines, Transformers.
        /// Buses can not be added with Monitors
        /// For Loads the terminal=2 gives the values of V, I, P and Q. 
        /// Also for Transformers we are adding the Monitors.
        /// </summary>
        public string CreateMonitors()
        {

            //Process Transformers and add Nodes and Edges
            List<CircuitEntry> transformers = this.cEntries.FindAll((ce) => { if (ce.CEType.Trim().ToLower().Equals("transformer")) { return true; } else { return false; } }).ToList<CircuitEntry>();
            foreach(CircuitEntry cT in transformers)
            {
                Transformers[cT.CEName] = 1;
            }

            //Process Lines and add Nodes and Edges
            List<CircuitEntry> lines = this.cEntries.FindAll((ce) => { if (ce.CEType.Trim().ToLower().Equals("line")) { return true; } else { return false; } }).ToList<CircuitEntry>();
            foreach(CircuitEntry cL in lines)
            {
                Lines[cL.CEName] = 1;
            }

            dssScriptWriter = new DSSScriptWriter(DPath, Transformers, Lines);
            return dssScriptWriter.CreateAndPlaceMonitorScript(false);
        }

        /// <summary>
        /// Function for creating Matlab Script
        /// </summary>
        /// <returns></returns>
        public string CreateMatLabScript()
        {
            string res = string.Empty;
            MatLabScriptWriter msWriter = new MatLabScriptWriter(DPath, dssScriptWriter.MonitorNames);
            res =  msWriter.WriteScript();
            MSPath = msWriter.SubDirPath;
            MFName = msWriter.MatFileName;
            return res;
        }
    }
}
