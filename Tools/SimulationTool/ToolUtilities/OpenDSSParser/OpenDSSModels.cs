using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoB.ToolUtilities.OpenDSSParser
{
    public class CircuitEntry
    {
        public string CEType { get; set; }
        public string CEName { get; set; }

        public List<string> CEEntries { get; set; }

        public CircuitEntry()
        {
            CEType = CEName = string.Empty;
            CEEntries = new List<string>();
        }
    }
    public enum NodeType
    {
        NA,
        Substation,
        Transformer,
        Bus,
        Load,
        PVPanel
    }

    public enum ECTypes
    {
        Transformer,
        Line,
        PVPanel,
        Load,
        PrimaryBus,
        SecondaryBus
    }

    public enum EdgeType
    {
        NA,
        NonExec,
        Exec
    }

    public enum GraphTypes
    {
        Detailed,
        Symbolic,
        NA
    }

    public enum LineUnit
    {
        m,
        ft,
        NA
    }

    /// <summary>
    /// Base class for all Nodes in the Graph
    /// The NodeType is NA for all Nodes that are extracted from File
    /// But while creting graph they will have a specific type.
    /// </summary>
    public class GraphNode
    {
        public string Name { get; set; }
        public NodeType NType { get; set; }
        public GraphNode()
        {
            Name = string.Empty;
            NType = NodeType.NA;
        }
        public override string ToString()
        {
            base.ToString();
            return string.Format("Graph Node Name={0}, Type={1}", this.Name, this.NType);

        }
    }
    
    /// <summary>
    /// Base class for all edges in the graph.
    /// </summary>
    public class GraphEdge
    {
        public GraphNode Tail { get; set; }
        public GraphNode Head { get; set; }
        public EdgeType EType { get; set; }

        public object UserData { get; set; }
        public GraphEdge()
        {
            Tail = new GraphNode();
            Head = new GraphNode();
            EType = EdgeType.NA;
            UserData = new object();
        }

        public GraphEdge(GraphNode t, GraphNode h, EdgeType eT)
        {
            Tail = t;
            Head = h;
            EType = eT;
        }
        public override string ToString()
        {
            return string.Format("Graph Edge Head={0}, Tail={1} EType={2}",
                                 this.Head.Name, this.Tail.Name, this.EType.ToString());
        }
    }

    public static class LoadDetails
    {
        public static Dictionary<string, string> LoadProfiles = new Dictionary<string, string>();

        static LoadDetails()
        {
            FillLoadProfiles();
        }
        static void FillLoadProfiles()
        {
            LoadProfiles.Add("208450100", "HIGH");
            LoadProfiles.Add("209450100", "HIGH");
            LoadProfiles.Add("210450100", "HIGH");
            LoadProfiles.Add("211450100", "HIGH");
            LoadProfiles.Add("212450100", "HIGH");
            LoadProfiles.Add("213450100", "HIGH");
            LoadProfiles.Add("216450100", "HIGH");
            LoadProfiles.Add("231019089", "HIGH");
            LoadProfiles.Add("208450101", "HIGH");
            LoadProfiles.Add("209450101", "HIGH");
            LoadProfiles.Add("210450112", "HIGH");
            LoadProfiles.Add("211450111", "HIGH");
            LoadProfiles.Add("212450101", "HIGH");
            LoadProfiles.Add("213450101", "HIGH");
            LoadProfiles.Add("216450101", "HIGH");
            LoadProfiles.Add("231019081", "HIGH");
            LoadProfiles.Add("424818078", "HIGH");
            LoadProfiles.Add("425407080", "HIGH");
            LoadProfiles.Add("541897444", "HIGH");
            LoadProfiles.Add("556796868", "HIGH");
            LoadProfiles.Add("269450100", "HIGH");
            LoadProfiles.Add("814484200", "HIGH");
            LoadProfiles.Add("640292047", "HIGH");
            LoadProfiles.Add("923350100", "HIGH");
            LoadProfiles.Add("109614539", "MEDIUM");
            LoadProfiles.Add("138468953", "MEDIUM");
            LoadProfiles.Add("313055646", "LOW");
            LoadProfiles.Add("318979782", "LOW");
            LoadProfiles.Add("438429982", "MEDIUM");
            LoadProfiles.Add("527508382", "LOW");
            LoadProfiles.Add("589583049", "MEDIUM");
            LoadProfiles.Add("665149786", "MEDIUM");
            LoadProfiles.Add("679416869", "MEDIUM");
            LoadProfiles.Add("113450100", "MEDIUM");
            LoadProfiles.Add("171450100", "LOW");
            LoadProfiles.Add("206350100", "LOW");
            LoadProfiles.Add("207350100", "MEDIUM");
            LoadProfiles.Add("208350100", "MEDIUM");
            LoadProfiles.Add("209350100", "MEDIUM");
            LoadProfiles.Add("189720633", "LOW");
            LoadProfiles.Add("183350100", "LOW");
            LoadProfiles.Add("184350100", "LOW");
            LoadProfiles.Add("10350100", "LOW");
            LoadProfiles.Add("160450100", "LOW");
            LoadProfiles.Add("190450100", "LOW");
            LoadProfiles.Add("255450100", "LOW");
            LoadProfiles.Add("637202100", "LOW");
            LoadProfiles.Add("954350100", "HIGH");
            LoadProfiles.Add("96450100", "HIGH");
            LoadProfiles.Add("135350100", "HIGH");
            LoadProfiles.Add("136350100", "MEDIUM");
            LoadProfiles.Add("137350100", "MEDIUM");
            LoadProfiles.Add("138350100", "MEDIUM");
            LoadProfiles.Add("139350100", "MEDIUM");
            LoadProfiles.Add("140350100", "MEDIUM");
            LoadProfiles.Add("141350100", "MEDIUM");
            LoadProfiles.Add("118350100", "MEDIUM");
            LoadProfiles.Add("133350100", "MEDIUM");
            LoadProfiles.Add("134350100", "MEDIUM");
            LoadProfiles.Add("142350100", "MEDIUM");
            LoadProfiles.Add("143350100", "MEDIUM");
            LoadProfiles.Add("144350100", "MEDIUM");
            LoadProfiles.Add("575350100", "MEDIUM");
            LoadProfiles.Add("576350100", "MEDIUM");
            LoadProfiles.Add("307998000", "MEDIUM");
            LoadProfiles.Add("324998000", "MEDIUM");
            LoadProfiles.Add("326998000", "MEDIUM");
            LoadProfiles.Add("329998000", "MEDIUM");
            LoadProfiles.Add("331998000", "MEDIUM");
            LoadProfiles.Add("374998000", "LOW");
            LoadProfiles.Add("577350100", "LOW");
            LoadProfiles.Add("578350100", "HIGH");
            LoadProfiles.Add("579350100", "MEDIUM");
            LoadProfiles.Add("578350110", "MEDIUM");
            LoadProfiles.Add("579350111", "MEDIUM");
            LoadProfiles.Add("234350100", "MEDIUM");
            LoadProfiles.Add("235350100", "MEDIUM");
            LoadProfiles.Add("246350100", "MEDIUM");
            LoadProfiles.Add("236350100", "MEDIUM");
            LoadProfiles.Add("237350100", "MEDIUM");
            LoadProfiles.Add("245350100", "MEDIUM");
            LoadProfiles.Add("781998000", "MEDIUM");
            LoadProfiles.Add("857998000", "MEDIUM");
            LoadProfiles.Add("642202100", "MEDIUM");
            LoadProfiles.Add("745202100", "MEDIUM");
            LoadProfiles.Add("240450100", "MEDIUM");
            LoadProfiles.Add("505202100", "MEDIUM");
            LoadProfiles.Add("643202100", "MEDIUM");
            LoadProfiles.Add("637202101", "MEDIUM");
            LoadProfiles.Add("459328650", "MEDIUM");
            LoadProfiles.Add("843998000", "MEDIUM");
            LoadProfiles.Add("844998000", "LOW");

        }
    }

    public static class PVPanelDetails
    {
        public static Dictionary<string, string> PVProfiles = new Dictionary<string, string>();

        static PVPanelDetails()
        {
            FillPVProfiles();
        }

        static void FillPVProfiles()
        {
            PVProfiles.Add("PV05410_g2100mk7800", "HIGH");
            PVProfiles.Add("PV05410_g2100ml7400", "MEDIUM");
            PVProfiles.Add("PV05410_g2100mn1400", "MEDIUM");
            PVProfiles.Add("PV05410_g2100nj7400", "MEDIUM");
            PVProfiles.Add("PV05410_g2100mn4700", "MEDIUM");
            PVProfiles.Add("PV05410_g2100nk6800", "LOW");
            PVProfiles.Add("PV05410_g2100oj3400", "LOW");
            PVProfiles.Add("PV05410_g2100oj3600", "MEDIUM");
            PVProfiles.Add("PV05410_g2100ml7401", "MEDIUM");
            PVProfiles.Add("PV05410_g2100qp9600", "MEDIUM");
            PVProfiles.Add("PV05410_g2100rl8500", "LOW");
            PVProfiles.Add("PV05410_g2100rn0300", "LOW");
            PVProfiles.Add("PV05410_g2100rn1800", "LOW");
            PVProfiles.Add("PV05410_g2100rm5200", "LOW");
            PVProfiles.Add("PV05410_g2100nl3900", "LOW");
            PVProfiles.Add("PV05410_g2100nl6900", "MEDIUM");
            PVProfiles.Add("PV05410_g2100pm3800", "LOW");
            PVProfiles.Add("PV05410_g2100pn0800", "MEDIUM");
        }

        public static bool IsProductionTime(string timeSlot)
        {
            string sTime = timeSlot.Split('-')[0];
            string eTime = timeSlot.Split('-')[1];
            DateTime asDateTime = DateTime.ParseExact("09:30", "HH:mm", CultureInfo.InvariantCulture);
            DateTime aeDateTime = DateTime.ParseExact("15:00", "HH:mm", CultureInfo.InvariantCulture);
            DateTime sDateTime = DateTime.ParseExact(sTime.Trim(), "HH:mm", CultureInfo.InvariantCulture);
            DateTime eDateTime = DateTime.ParseExact(eTime.Trim(), "HH:mm", CultureInfo.InvariantCulture);
            if (DateTime.Compare(sDateTime, asDateTime) >= 0 & DateTime.Compare(eDateTime, aeDateTime) <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
