using DataSerailizer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoB.ToolUtilities.OpenDSSParser
{
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

    public class PVSystem : GraphNode
    {
        public string Bus1 { get; set; }
        public int Irradiance { get; set; }
        public double kV { get; set; }
        public double kVA { get; set; }
        public double PF { get; set; }
        public double Pmpp { get; set; }
        public LoadShape Duty { get; set; }
        public int Phases { get; set; }

        public PVSystem()
        {
            kV = kVA = PF = Pmpp = 0.0;
            Bus1 = string.Empty;
            Irradiance = Phases = 0;
            Duty = null;
        }
        public override string ToString()
        {
            return string.Format("PVSystem Name={0}, Type={1}, Bus={2}, Irradiance={3}, kV={4}, kVA={5}, PF={6}, Pmpp={7}, Phases={8}",
                                 this.Name, this.NType, this.Bus1, this.Irradiance, this.kV, this.kVA, this.PF, this.Pmpp, this.Phases);
        }
    }

    public class LoadShape : GraphNode
    {
        public long NPTS { get; set; }

        public int MInterval { get; set; }

        public string Mult { get; set; }

        public string Action { get; set; }

        public LoadShape()
        {
            NPTS = MInterval = 0;
            Mult = Action = string.Empty;
        }
    }

    public class ImpactAnalysisData
    {
        public string PvName { get; set; }
        public List<string> INodes { get; set; }
        public List<string> IEdges { get; set; }
        public string INotes { get; set; }

        public ImpactAnalysisData()
        {
            PvName = string.Empty;
            INodes = new List<string>();
            IEdges = new List<string>();
            INotes = string.Empty;
        }
    }
   
    public class PrepareImpactResult
    {
        List<ImpactResultData> listIRData = new List<ImpactResultData>();
        public PrepareImpactResult()
        {

        }

        public PrepareImpactResult(ImpactResultData irData) : this()
        {
            listIRData.Add(irData);
        }

        void CreateVisualGraph()
        {
            foreach (ImpactResultData irData in listIRData)
            {
                //irData.VisualGraph
            }
        }
    }

    /// <summary>
    /// New	load.278071200	phases=1	
    /// kV=0.24	bus1=G2000RN2800_N274196_sec_1.3	xfkVA=3.25119253	pf=0.98	status=variable	model=4 CVRwatts=0.8	
    /// CVRvars=3	conn=wye	Vminpu=0.7	duty=LS_PhaseC		!	Phase	3.
    /// This class describes a Load
    /// </summary>
    public class Load : GraphNode
    {
        public int Phases { get; set; }
        public double kV { get; set; }
        public double kVA { get; set; }
        public double kvar { get; set; }
        public double kw { get; set; }
        public double kwH { get; set; }
        public double xfkVA { get; set; }
        public double PF { get; set; }
        public string Bus1 { get; set; }
        public string status { get; set; }
        public int Model { get; set; }
        public double CVRWatts { get; set; }
        public double CVRvars { get; set; }
        public ConnectionType LoadConnType { get; set; }
        public double Vminpu { get; set; }
        public double Vmaxpu { get; set; }
        public LoadShape Duty { get; set; }

        public Load()
        {
            Phases = Model = 0;
            kV = xfkVA = PF = CVRvars = CVRWatts = Vminpu = Vmaxpu = 0.0;
            kVA = kvar = kw = kwH = 0.0;
            Bus1 = string.Empty;
            LoadConnType = ConnectionType.NotAssigned;
            Duty = null;
        }
        public override string ToString()
        {
            return string.Format("Load Name={0}, Type={1}, Phases={2}, kV={3}, kVA={4}, kvar={5}, kw={6}, kwH={7}, xfakVA={8}, PF={9}, Bus1={10}, status={11}, Model={12}",
                                 this.Name, this.NType, this.Phases, this.kV, this.kVA, this.kvar, this.kw, this.kwH, this.xfkVA, this.PF, this.Bus1, this.status, this.Model);
        }
    }
    public enum ConnectionType
    {
        NotAssigned,
        Wye,
        Delta
    }

    /// <summary>
    /// New Capacitor.Cap_G2100PL6500 phases=3 kvar=900 bus1=N300521 kV=34.5 conn=wye enabled=true
    /// This class describes Capacitor.
    /// </summary>
    public class Capacitor : GraphNode
    {
        public int Phases { get; set; }
        public bool Enabled { get; set; }
        public ConnectionType CapacitorConnType { get; set; }
        public double kvar { get; set; }
        public double kV { get; set; }
        public string Bus { get; set; }

        public Capacitor()
        {
            Phases = 0;
            kvar = kV = 0.0;
            Enabled = false;
            Bus = string.Empty;
        }
    }

    public class Bus : GraphNode
    {
        //public string BusName { get; set; }
        public long BusSlNo { get; set; }
        public double BusLongitude { get; set; }
        public double BusLatitude { get; set; }

        public Bus()
        {
            //BusName = string.Empty;
            BusSlNo = long.MinValue;
            BusLongitude = double.NaN;
            BusLatitude = double.NaN;
            NType = NodeType.Bus;
        }
        public override string ToString()
        {
            return string.Format("Bus Name={0}, Type={1}, BusSlNo={2}, Longitude={3}, Latitude={4}", this.Name, this.NType, this.BusSlNo, this.BusLongitude, this.BusLatitude);
        }
    }
    public class Location
    {
        public double Longitude;
        public double Latitude;
        public override string ToString()
        {
            return string.Format("({0}:{1})", this.Longitude, this.Latitude);
        }
        public Location()
        {
            Longitude = Latitude = 0.0;
        }
    }

    public enum TransformerType
    {
        NotAssigned,
        Substation,
        Distribution
    }

    public class Winding
    {
        public int No { get; set; }

        public string Bus { get; set; }

        public ConnectionType WindingConnType { get; set; }

        public double kv { get; set; }

        public double kvA { get; set; }

        public int PercentageR { get; set; }

        public Winding()
        {
            No = -1;
            Bus = string.Empty;
            WindingConnType = ConnectionType.NotAssigned;
            kv = 0.0d;
            kvA = 0.0d;
            PercentageR = 0;
        }
        public override string ToString()
        {
            return string.Format("Winding No={0}, Bus={1}, WindingConnType={2}, kv={3}, kvA ={4}, PercentageR={5}",
                                    this.No, this.Bus, this.WindingConnType, this.kv, this.kvA, this.PercentageR);
        }
    }
    public class Transformer : GraphNode
    {
        public Location TrLocation { get; set; }
        public TransformerType TrType { get; set; }

        public int Phases { get; set; }

        public int NoOfWindings { get; set; }

        public List<Winding> Windings { get; set; }

        public float PerImag { get; set; }

        public float PerLoadLoss { get; set; }

        public float PerNoLoadLoss { get; set; }

        public float Xhl { get; set; }

        public string sub { get; set; }

        public List<Double> PerRS { get; set; }

        public float MinTap { get; set; }

        public float MaxTap { get; set; }

        public long EmergHkva { get; set; }

        public bool RegControl { get; set; }
        public Transformer()
        {
            TrLocation = new Location();
            TrType = TransformerType.NotAssigned;
            NoOfWindings = -1;
            Windings = new List<Winding>();
            PerImag = PerLoadLoss = PerNoLoadLoss = Xhl = 0.0f;
            sub = string.Empty;
            MinTap = MaxTap = float.NaN;
            EmergHkva = long.MinValue;
            PerRS = new List<double>();
        }
        public override string ToString()
        {
            List<string> windingInfo = new List<string>();
            foreach (Winding w in this.Windings)
            {
                windingInfo.Add(w.ToString());
            }
            return string.Format("Transformer Name={0}, Type ={1}, Phases={2}, NoofWindings={3}, Winding1 ={4}, Winding2={5}, PerImag={6}, PerLoadLoss={7}, Xhl={8}, sub={9}, RegControlAttached={10}",
                                  this.Name, this.TrType, this.Phases, this.NoOfWindings, windingInfo[0], windingInfo[1], this.PerImag, this.PerLoadLoss, this.Xhl, this.sub, this.RegControl);
        }

    }

    public class Line : GraphEdge
    {
        public string Id { get; set; }
        public string Bus1 { get; set; }
        public string Bus2 { get; set; }
        public int Phase { get; set; }
        public double Length { get; set; }
        public LineUnit Units { get; set; }
        public string LineCode { get; set; }

        public Line()
        {
            Id = Bus1 = Bus2 = LineCode = string.Empty;
            Phase = 0;
            Length = 0;
        }

        public override string ToString()
        {
            return string.Format("Line Id = {1}, Bus1={2}, Bus2={3}, Phase={4}, Length={5}",
                                  base.ToString(), this.Id, this.Bus1, this.Bus2, this.Phase, this.Length);
        }
    }
}
