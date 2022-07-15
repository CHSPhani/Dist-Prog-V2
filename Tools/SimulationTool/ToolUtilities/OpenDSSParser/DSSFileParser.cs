using ContractDataModels;
using DataSerailizer;
using Microsoft.Glee.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UoB.ToolUtilities.DataStructure;

namespace UoB.ToolUtilities.OpenDSSParser
{
    public class DSSFileParser
    {
        List<string> dssFiles = new List<string>();
        List<string> IgnoreEntries = new List<string>();
        List<string> KWEntries = new List<string>();
        List<string> ActionEntries = new List<string>();
        List<string> nodes;
        List<string> edges;
        Dictionary<string, long> symbNodes = new Dictionary<string, long>();
        GraphDS graphDS;
        Dictionary<string, string> excludedList;
        Dictionary<string, GraphNode> NodeData;
        Dictionary<string, GraphEdge> EdgeData;
        List<string> furEdges = new List<string>();
        List<string> leafNodes = new List<string>();
        List<string> leafEdges = new List<string>();
        List<CousinDataHelper> CousinLoads = new List<CousinDataHelper>();
        List<string> impactTransformers = new List<string>();
        List<string> removableNodes = new List<string>();
        List<string> removableEdges = new List<string>();
        List<string> newEdges = new List<string>();
        Dictionary<string, GraphNode> NewNodeData = new Dictionary<string, GraphNode>();
        Dictionary<string, GraphEdge> NewEdgeData = new Dictionary<string, GraphEdge>();
        Dictionary<string, ImpactAnalysisData> pvNamesForProcessing = new Dictionary<string, ImpactAnalysisData>();
        string fPath;
        public string FilePath
        {
            get
            {
                return this.fPath;
            }
        }
        List<CircuitEntry> CEntries;
        public List<CircuitEntry> CircuitEntities
        {
            get
            {
                return this.CEntries;
            }
        }

        Graph symbGraph;
        public Graph SymbGraph
        {
            get { return symbGraph; }
        }

        public DSSFileParser()
        {
            CEntries = new List<CircuitEntry>();
            excludedList = new Dictionary<string, string>();
            symbGraph = new Graph("symGraph");
            nodes = new List<string>();
            edges = new List<string>();
            graphDS = new GraphDS();
            NodeData = new Dictionary<string, GraphNode>();
            EdgeData = new Dictionary<string, GraphEdge>();
            dssFiles = new List<string>();
            IgnoreEntries.Add("clear");
            IgnoreEntries.Add("!");
            KWEntries.Add("Circuit");
            ActionEntries.Add("new");
            ActionEntries.Add("redirect");
        }

        public DSSFileParser(string dirPath) : this()
        {
            this.fPath = dirPath;
            dssFiles = Directory.GetFiles(dirPath, "*.dss", SearchOption.AllDirectories).ToList<string>();
            if (ParseDSSConfig())
            {
                CreateGraphDS();
                CreateSymbolicGraph();
            }
        }
        void CreateGraphDS()
        {
            var rootNode = this.CEntries.Find((ce) => { if (ce.CEType.Equals("Circuit")) { return true; } else { return false; } });

            string rNode = string.Empty;
            rNode = rootNode.CEName;
            //Add root node Curcuit Name
            if (graphDS.AddNode(rNode))
            {
                AddToNodeData(rNode, NodeType.Substation);
            }

            //get root bus from root node
            string firstBN = string.Empty;
            foreach (string res in rootNode.CEEntries)
            {
                if (res.Contains("bus1"))
                {
                    firstBN = res.Split('=')[1];
                    if (graphDS.AddNode(firstBN))
                    {
                        AddToNodeData(firstBN, NodeType.Bus);
                    }
                    //Adding Edge to EdgeData strcture
                    string eKey1 = string.Format("{0}-{1}", rNode, firstBN);
                    graphDS.AddDirectedEdge(rNode, firstBN);
                    if (!EdgeData.ContainsKey(eKey1))
                    {
                        EdgeData.Add(eKey1, new GraphEdge() { Head = NodeData[rNode], Tail = NodeData[firstBN], EType = EdgeType.Exec, UserData = rootNode });
                    }
                }
            }

            //Process Lines and add Nodes and Edges
            List<CircuitEntry> lines = this.CEntries.FindAll((ce) => { if (ce.CEType.Trim().ToLower().Equals("line")) { return true; } else { return false; } }).ToList<CircuitEntry>();
            ProcessLines(lines);

            //Process Transformers and add Nodes and Edges
            List<CircuitEntry> transformers = this.CEntries.FindAll((ce) => { if (ce.CEType.Trim().ToLower().Equals("transformer")) { return true; } else { return false; } }).ToList<CircuitEntry>();
            ProcessTransformers(transformers);

            //Process Loads and add then to graph
            List<CircuitEntry> loads = this.CEntries.FindAll((ce) => { if (ce.CEType.Trim().ToLower().Equals("load")) { return true; } else { return false; } }).ToList<CircuitEntry>();
            ProcessLoads(loads);
        }
        void ProcessLoads(List<CircuitEntry> loads)
        {
            foreach (CircuitEntry ce in loads)
            {
                var bus1Val = ce.CEEntries.Find((c) => { if (c.Split('=')[0].Trim().ToLower().Equals("bus1")) { return true; } else { return false; } });
                string bus1 = bus1Val.Split('=')[1];
                string lName = ce.CEName;
                if (graphDS.AddNode(lName))
                {
                    AddToNodeData(lName, NodeType.Load);
                }

                //Now add the edge for actual line
                string eKey = string.Format("{0}-{1}", bus1, lName);
                graphDS.AddDirectedEdge(bus1, lName);
                //This is Exec Edge
                if (!EdgeData.ContainsKey(eKey))
                {
                    EdgeData.Add(eKey, new GraphEdge() { Head = NodeData[bus1], Tail = NodeData[lName], EType = EdgeType.Exec, UserData = ce });
                }
            }
        }
        void ProcessTransformers(List<CircuitEntry> transformers)
        {
            List<string> busNames = new List<string>();
            bool feDone = false;
            foreach (CircuitEntry tr in transformers)
            {
                foreach (string cStr in tr.CEEntries)
                {
                    if (cStr.Split('=')[0].Trim().ToLower().Equals("bus"))
                    {
                        string bus = cStr.Split('=')[1];
                        busNames.Add(bus);

                        if (!feDone)
                        {
                            if (graphDS.AddNode(tr.CEName))
                            {
                                AddToNodeData(tr.CEName, NodeType.Transformer);
                            }
                            if (graphDS.AddNode(bus))
                            {
                                AddToNodeData(bus, NodeType.Bus);
                            }
                            //Now add the edge for actual line
                            string eKey = string.Format("{0}-{1}", bus, tr.CEName);
                            graphDS.AddDirectedEdge(bus, tr.CEName);
                            //This is Exec Edge
                            if (!EdgeData.ContainsKey(eKey))
                            {
                                EdgeData.Add(eKey, new GraphEdge() { Head = NodeData[bus], Tail = NodeData[tr.CEName], EType = EdgeType.Exec, UserData = tr });
                            }
                            feDone = true;
                        }
                        else
                        {
                            if (graphDS.AddNode(tr.CEName))
                            {
                                AddToNodeData(tr.CEName, NodeType.Transformer);
                            }
                            if (graphDS.AddNode(bus))
                            {
                                AddToNodeData(bus, NodeType.Bus);
                            }
                            //Now add the edge for actual line
                            string eKey = string.Format("{0}-{1}", tr.CEName, bus);
                            graphDS.AddDirectedEdge(tr.CEName, bus);
                            //This is Exec Edge
                            if (!EdgeData.ContainsKey(eKey))
                            {
                                EdgeData.Add(eKey, new GraphEdge() { Head = NodeData[tr.CEName], Tail = NodeData[bus], EType = EdgeType.Exec, UserData = tr });
                            }
                            feDone = false;
                        }
                    }
                }
                int firstCounter = 0;
                int secondCounter = 1;
                while (secondCounter < busNames.Count)
                {
                    string bus1 = busNames[firstCounter];
                    string bus2 = busNames[secondCounter];


                    //Now add the edge for actual line
                    string eKey = string.Format("{0}-{1}", bus1, bus2);
                    graphDS.AddDirectedEdge(bus1, bus2);
                    //This is Exec Edge
                    if (EdgeData.ContainsKey(eKey))
                    {
                        EdgeData.Remove(eKey);
                    }

                    firstCounter++;
                    secondCounter++;
                }
            }
        }
        void ProcessLines(List<CircuitEntry> lines)
        {
            foreach (CircuitEntry ce in lines)
            {
                var bus1Val = ce.CEEntries.Find((c) => { if (c.Split('=')[0].Trim().ToLower().Equals("bus1")) { return true; } else { return false; } });
                var bus2Val = ce.CEEntries.Find((c) => { if (c.Split('=')[0].Trim().ToLower().Equals("bus2")) { return true; } else { return false; } });

                string bus1 = bus1Val.Split('=')[1];
                string bus2 = bus2Val.Split('=')[1];

                if (graphDS.AddNode(bus1))
                {
                    AddToNodeData(bus1, NodeType.Bus);
                }
                if (graphDS.AddNode(bus2))
                {
                    AddToNodeData(bus2, NodeType.Bus);
                }
                //Now add the edge for actual line
                string eKey = string.Format("{0}-{1}", bus1, bus2);
                graphDS.AddDirectedEdge(bus1, bus2);
                //This is Exec Edge
                if (!EdgeData.ContainsKey(eKey))
                {
                    EdgeData.Add(eKey, new GraphEdge() { Head = NodeData[bus1], Tail = NodeData[bus2], EType = EdgeType.Exec, UserData = ce });
                }
            }
        }
        bool ParseDSSConfig()
        {
            //1.Read the master_ckt24.dss file. This is the starting file. 
            //Ideally there should be one master file. 
            var masterFilePath = dssFiles.Find((s) => { if (s.Contains(Utilities.MasterFileName)) { return true; } else { return false; } });
            if (!string.IsNullOrEmpty(masterFilePath))
                return ParseDSSFile(masterFilePath);
            return false;
        }
        bool ParseDSSFile(string mFilePath)
        {
            try
            {
                foreach (string line in File.ReadLines(mFilePath))
                {
                    if (string.IsNullOrEmpty(line))
                        continue;
                    if (IgnoreEntries.Contains(line.Trim().ToLower()))
                        continue;
                    if (line.StartsWith("!")) //Comment
                        continue;

                    string processLine = Regex.Replace(line.Trim(), @"\s+", " ").Replace(" =", "=").Replace("= ", "=");

                    //we only process lines with new and redirect
                    if (processLine.Split(' ')[0].Trim().ToLower().Equals("new"))
                    {
                        ProcessorNEWEntry(processLine, false);
                    }
                    if (processLine.Split(' ')[0].Trim().Equals("~"))
                    {
                        ProcessorNEWEntry(processLine, true);
                    }
                    else if (processLine.Split(' ')[0].Trim().ToLower().Equals("redirect"))
                    {
                        foreach (string f in dssFiles)
                        {
                            string[] fParts = f.Split('\\');
                            string f2 = fParts[fParts.Length - 1];
                            if (f2.Trim().ToLower().Equals(processLine.Split(' ')[1].Trim().ToLower()))
                            {
                                ParseDSSFile(f);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        void ProcessorNEWEntry(string pLine, bool append)
        {
            CircuitEntry cEntry = null;
            if (append)
            {
                cEntry = CEntries[CEntries.Count - 1];
            }
            else
            {
                cEntry = new CircuitEntry();
            }
            List<string> entries = pLine.Split(' ').ToList<string>();
            foreach (string s in entries)
            {
                if (s.Equals("!"))
                    break;
                if (s.Contains('='))
                {
                    cEntry.CEEntries.Add(s);
                }
                else if (s.Contains("."))
                {
                    cEntry.CEType = s.Split('.')[0];
                    cEntry.CEName = s.Split('.')[1];
                }
            }
            if (!append)
                CEntries.Add(cEntry);
        }
        void AddToNodeData(string gNodeName, NodeType nType = NodeType.NA)
        {
            if (!NodeData.ContainsKey(gNodeName))
            {
                NodeData.Add(gNodeName, new GraphNode() { Name = gNodeName, NType = nType }); //Adding a new Bus Object
            }
        }
        void CreateSymbolicGraph()
        {
            symbGraph.CleanNodes();
            symbGraph.Directed = true;
            long counter = 0;
            //Add Nodes
            nodes = graphDS.GetAllNodeNamess();
            foreach (string s in nodes)
            {
                if (symbNodes.ContainsKey(s))
                {
                    counter = symbNodes[s];
                }
                else
                {
                    symbNodes[s] = ++counter;
                }
            }
            //Write Edges
            symbGraph.Edges.Clear();

            foreach (string e in EdgeData.Keys)
            {
                string bus1 = e.Split('-')[0].Trim().Split('.')[0].Trim() + ".1.2.3";
                string bus2 = e.Split('-')[1].Trim().Split('.')[0].Trim() + ".1.2.3";

                string finalBus1 = string.Empty;
                string finalBus2 = string.Empty;


                if (symbNodes.ContainsKey(bus1))
                {
                    if (!string.IsNullOrEmpty(symbNodes[bus1].ToString()))
                    {
                        if (!bus1.Equals(e.Split('-')[0]))
                        {
                            finalBus1 = bus1;
                            if (NodeData.ContainsKey(e.Split('-')[0]))
                                NodeData.Remove(e.Split('-')[0]);
                            if (!excludedList.ContainsKey(e.Split('-')[0]))
                                excludedList[e.Split('-')[0]] = finalBus1;
                        }
                        else
                            finalBus1 = e.Split('-')[0];
                    }
                }
                else
                {
                    finalBus1 = e.Split('-')[0];
                }

                if (symbNodes.ContainsKey(bus2))
                {
                    if (!string.IsNullOrEmpty(symbNodes[bus2].ToString()))
                    {
                        if (!bus2.Equals(e.Split('-')[1]))
                        {
                            finalBus2 = bus2;
                            if (NodeData.ContainsKey(e.Split('-')[1]))
                                NodeData.Remove(e.Split('-')[1]);
                            if (!excludedList.ContainsKey(e.Split('-')[1]))
                                excludedList[e.Split('-')[1]] = finalBus2;
                        }
                        else
                            finalBus2 = e.Split('-')[1];
                    }
                }
                else
                {
                    finalBus2 = e.Split('-')[1];
                }

                Edge ce = (Edge)symbGraph.AddEdge(symbNodes[finalBus1].ToString(), symbNodes[finalBus2].ToString());
                string edgKey = string.Format("{0}-{1}", e.Split('-')[0], e.Split('-')[1]);
                if (EdgeData.ContainsKey(edgKey))
                {
                    if ((EdgeData[edgKey]).EType == EdgeType.Exec)
                    {
                        ce.Attr.Color = Microsoft.Glee.Drawing.Color.Green;
                    }
                    else
                    {
                        ce.Attr.Color = Microsoft.Glee.Drawing.Color.Black;
                    }
                    ce.UserData = EdgeData[edgKey];
                }
            }

            //Write Nodes
            foreach (string s in nodes)
            {
                if (excludedList.Keys.Contains(s))
                    continue;
                string id = symbNodes[s].ToString();
                if (NodeData[s].NType == NodeType.Substation)
                {
                    Node n = (Node)symbGraph.AddNode(id);
                    n.Attr.Shape = Shape.Diamond;
                    n.Attr.Fillcolor = Color.Green;
                    n.UserData = NodeData[s];
                }
                else if (NodeData[s].NType == NodeType.Bus)
                {
                    Node n = (Node)symbGraph.AddNode(id);
                    n.Attr.Shape = Shape.Box;
                    n.Attr.Fillcolor = Color.DarkBlue;
                    n.UserData = NodeData[s];
                }
                else if (NodeData[s].NType == NodeType.Load)
                {
                    Node n = (Node)symbGraph.AddNode(id);
                    n.Attr.Shape = Shape.House;
                    if (LoadDetails.LoadProfiles[s] == "HIGH")
                    {
                        n.Attr.Fillcolor = Color.Gray;
                    }
                    else if (LoadDetails.LoadProfiles[s] == "MEDIUM")
                    {
                        n.Attr.Fillcolor = Color.LightBlue;
                    }
                    else if (LoadDetails.LoadProfiles[s] == "LOW")
                    {
                        n.Attr.Fillcolor = Color.LightGray;
                    }

                    n.UserData = NodeData[s];
                }
                else if (NodeData[s].NType == NodeType.PVPanel)
                {
                    Node n = (Node)symbGraph.AddNode(id);
                    n.Attr.Shape = Shape.House;
                    if (PVPanelDetails.PVProfiles[s] == "HIGH")
                    {
                        n.Attr.Fillcolor = Color.Green;
                    }
                    else if (PVPanelDetails.PVProfiles[s] == "MEDIUM")
                    {
                        n.Attr.Fillcolor = Color.LawnGreen;
                    }
                    else if (PVPanelDetails.PVProfiles[s] == "LOW")
                    {
                        n.Attr.Fillcolor = Color.LightGreen;
                    }
                    n.Attr.Fillcolor = Color.Yellow;
                    n.UserData = NodeData[s];
                }
                else if (NodeData[s].NType == NodeType.Transformer)
                {
                    Node n = (Node)symbGraph.AddNode(id);
                    n.Attr.Shape = Shape.DoubleCircle;
                    n.Attr.Fillcolor = Color.Brown;
                    n.UserData = NodeData[s];
                }
                else
                {
                    Node n = (Node)symbGraph.AddNode(id);
                    n.UserData = NodeData[s];
                }
            }
        }
        
        Dictionary<SemanticStructure, SemanticDetails> founds = new Dictionary<SemanticStructure, SemanticDetails>();
        List<string> notFounds = new List<string>();
       
        public SemanticDetails GetSSDetails(string ssName)
        {
            //Operational Branching.
            SemanticDetails sDets = new SemanticDetails();
            try
            {
                string uri = "net.tcp://localhost:6565/ObtainSSdetails";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<IObtainSSDetails>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                sDets = proxy.ObtainSD(ssName);
            }
            catch (Exception ex)
            {
                string eDet = "Not able to Obtain Individuals from Knowledge Graph. Exception is " + ex.Message;
            }
            if (sDets == null)
            {
                string eDet = "Not able to obtain circuit entities";
                return null;
            }
            return sDets;
        }

        public SemanticStructure GetSSDataForInst(string ssName)
        {
            SemanticStructure sStr = new SemanticStructure();
            try
            {
                string uri = "net.tcp://localhost:6565/ObtainSSForInstn";
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                binding.OpenTimeout = TimeSpan.FromMinutes(120);
                var channel = new ChannelFactory<IObtainSSForInst>(binding);
                var endPoint = new EndpointAddress(uri);
                var proxy = channel.CreateChannel(endPoint);
                sStr = proxy.ObtainSSForInst(ssName);
            }
            catch (Exception ex)
            {
                string sDet = "Not able to obtain semantic str" + ex.Message;
            }
            if (sStr == null)
            {
                return null;
            }
            return sStr;
        }

        public void AddPVs(List<CircuitEntry> pvs)
        {
            List<string> pvNames = new List<string>();
            Dictionary<string, string> pairs = new Dictionary<string, string>();
            foreach (CircuitEntry ce in pvs)
            {
                string pvName = string.Empty;
                foreach (string s in symbNodes.Keys)
                {
                    if (s.Contains(ce.CEEntries[0]))
                    {
                        pvName = s;
                        pvNames.Add(s);
                        pairs.Add(s, ce.CEName);
                        break;
                    }
                }
                if (graphDS.AddNode(ce.CEName))
                {
                    AddToNodeData(ce.CEName, NodeType.PVPanel);
                }
                //Adding Edge to EdgeData strcture
                string eKey1 = string.Format("{0}-{1}", pvName, ce.CEName);

                graphDS.AddDirectedEdge(pvName, ce.CEName);

                if (!EdgeData.ContainsKey(eKey1))
                {
                    EdgeData.Add(eKey1, new GraphEdge() { Head = NodeData[pvName], Tail = NodeData[ce.CEName], EType = EdgeType.Exec, UserData = pvName });
                }
            }
            CreateSymbolicGraph();
            CreateCycles(pvNames, pairs);
            CreateSymbolicGraph();
        }
        List<string> pathToNearTranf = new List<string>();
        bool tNodereached = false;
        void CreateCycles(List<string> pvNames, Dictionary<string, string> pairs)
        {
            foreach (string pvName in pvNames)
            {
                string ceName = pairs[pvName];// ce.CEEntries[0];
                pathToNearTranf.Clear();
                tNodereached = false;
                FindPathToNearTransformer(pvName);
                if (tNodereached)
                {
                    Edge ce1 = (Edge)symbGraph.AddEdge(symbNodes[ceName].ToString(), symbNodes[pathToNearTranf[pathToNearTranf.Count - 1]].ToString());
                    ce1.Attr.Color = Microsoft.Glee.Drawing.Color.IndianRed;

                    // Then add the load bus as a edge to the original bus 
                    graphDS.AddDirectedEdge(ceName, pathToNearTranf[pathToNearTranf.Count - 1]);
                    EdgeData.Add(string.Format("{0}-{1}", ceName, pathToNearTranf[pathToNearTranf.Count - 1]),
                            new GraphEdge { Head = NodeData[ceName], Tail = NodeData[pathToNearTranf[pathToNearTranf.Count - 1]], EType = EdgeType.NonExec });
                }
            }
        }
        /// <summary>
        /// We need to fix a bug here. 
        /// If one node has 3 edges and transformer connects to second edge we will not be able to get that 
        /// because we assume that there is only one edge possible. Thats why we break the loop : foreach (string s in result)
        /// with first iteration.
        /// </summary>
        /// <param name="pvName"></param>
        List<string> visitedEdges = new List<string>();
        void FindPathToNearTransformer(string pvName)
        {
            visitedEdges.Clear();
            while (true)
            {
                if (!pathToNearTranf.Contains(pvName))
                    pathToNearTranf.Add(pvName);
                else
                    break;
                if (tNodereached)
                    break;

                IEnumerable<string> results = (EdgeData.Keys).ToList<string>().FindAll(x => { if (x.ToLower().Contains(pvName.ToLower())) { return true; } else { return false; } });
                if (results.Count() == 0)
                {
                    break;
                }

                foreach (string s in results)
                {
                    if (visitedEdges.Contains(s))
                        continue;
                    else

                        visitedEdges.Add(s);

                    string[] subStr = s.Split('-');
                    if (string.Equals(subStr[0].ToLower(), pvName.ToLower()))
                    {
                        pvName = subStr[1];
                        if (!(NodeData[subStr[1]].NType == NodeType.Transformer))//is Transformer))//TransNames.Contains(subStr[1]))
                        {
                            break;
                        }
                        else
                        {
                            tNodereached = true;
                            break;
                        }
                    }
                    if (string.Equals(subStr[1].ToLower(), pvName.ToLower()))
                    {
                        pvName = subStr[0];
                        if (!(NodeData[subStr[0]].NType == NodeType.Transformer)) // is Transformer))//!TransNames.Contains(subStr[0]))
                        {
                            break;
                        }
                        else
                        {
                            tNodereached = true;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Not so good logic for Operational branching.
        /// </summary>
        /// <param name="sStrs"></param>
        List<Edge> delEdges = new List<Edge>();
        public void GetOperationalState(List<SemanticStructure> sStrs)
        {
           
            //Step1: Processing Nodes first
            foreach (string s in nodes)
            {
                string id = symbNodes[s].ToString();
                Node n = symbGraph.FindNode(id);
                if (n != null)
                {
                    GraphNode uD = n.UserData as GraphNode;
                    if (uD != null)
                    {
                        var reSS = sStrs.Find((p) => { if (p.SSName.ToLower().Equals(uD.Name.ToLower()) || p.XMLURI.ToLower().Equals(uD.Name.ToLower())) { return true; } else { return false; } });
                        if (reSS != null)
                        {
                            SemanticDetails sd = null;
                            if (reSS.SSName.Contains(':'))
                                sd = GetSSDetails(reSS.SSName.Split(':')[0]);
                            else
                                sd = GetSSDetails(reSS.SSName);
                            founds[reSS] = sd;
                        }
                        else
                        {
                            notFounds.Add(uD.Name);
                        }
                    }
                }
            }
            //Step2: Try to get information from semantic details to see can we find notFounds?
            Dictionary<string, SemanticStructure> lineNames = new Dictionary<string, SemanticStructure>();
            //Step3: Process Edge Data.
            foreach (KeyValuePair<string, GraphEdge> ge in EdgeData)
            {
                CircuitEntry ce = ge.Value.UserData as CircuitEntry;
                if (ce != null)
                {
                    string lineName = ce.CEName;
                    SemanticDetails sd = GetSSDetails(lineName);
                    founds[sd.ActialSS] = sd;
                    lineNames[lineName] = sd.ActialSS;
                    //get from linename
                    foreach (string s in ce.CEEntries)
                    {
                        if (s.Contains("bus"))
                            if (notFounds.Contains(s.Split('=')[1]))
                            {
                                notFounds.Remove(s.Split('=')[1]);
                            }
                    }
                }
            }
            //Step4: Getting Line status
            Dictionary<string, string> lineStatus = new Dictionary<string, string>();
            foreach (string lName in lineNames.Keys)
            {
                SemanticDetails sd = founds[lineNames[lName]];
                foreach (string s in sd.Incoming)
                {

                }
                foreach (string s2 in sd.Outgoing)
                {
                    if (s2.Split('-')[1].Split(':')[0].ToLower().Contains("switch"))
                    {
                        string tbSearch = s2.Split('-')[1];
                        SemanticStructure ss = GetSSDataForInst(tbSearch);
                        lineStatus[lName] = ss.XMLURI;
                    }
                }
            }

            //Step5: Now go through graph and check edge status.
            //Update status Green for connected Red for dis-connected
            
            foreach (Edge e in symbGraph.Edges)
            {
                //EdgeAttr.Color== Color.Red
                if ((e.UserData as GraphEdge).UserData as CircuitEntry != null)
                {
                    string lName = (((e.UserData as GraphEdge).UserData as CircuitEntry).CEName);
                    if (lineStatus.ContainsKey(lName))
                        if (lineStatus[lName].Equals("y"))
                        {
                            e.EdgeAttr.Color = Color.Red;
                            delEdges.Add(e);
                        }
                        else
                            e.EdgeAttr.Color = Color.Green;
                }
            }
            //Step6: Delete Red Edges from graph
            foreach(Edge e in delEdges)
            {
                symbGraph.Edges.Remove(e);
            }
        }
        public void GoLiveV2(string timeSlot)
        {
            //Step0: Check deleted edges and set proper ones
            List<string> pDelEdges = new List<string>();
            foreach(Edge e in delEdges)
            {
                //Get the GraphNode from symgraph. 
                Node sNode = symbGraph.FindNode(e.Source);
                GraphNode uD = sNode.UserData as GraphNode;
                Node tNode = symbGraph.FindNode(e.Target);
                GraphNode tD = tNode.UserData as GraphNode;
                if (uD!=null & tD != null)
                {
                    pDelEdges.Add(string.Format("{0}-{1}", uD.Name, tD.Name));
                }
            }
            List<GraphNode> pvNodes = new List<GraphNode>();
            //Step-1: Get all PVNode from NodeData
            foreach(GraphNode gN in NodeData.Values)
            {
                if (gN.NType == NodeType.PVPanel)
                    pvNodes.Add(gN);
            }

            //Step2:Get Siblings for PVNodes.
            foreach (GraphNode pgN in pvNodes)
            {
                string sourceBus = string.Empty;
                IEnumerable<string> results = (EdgeData.Keys).ToList<string>().FindAll(x => { if (x.ToLower().Contains(pgN.Name.ToLower())) { return true; } else { return false; } });
                if (results.Count() != 0)
                {
                    string ogEdge = string.Empty;
                    string igEdge = string.Empty;
                    foreach (string s in results)
                    {
                        if(s.Split('-')[1].ToLower().Equals(pgN.Name.ToLower()))
                        {
                            sourceBus = s.Split('-')[0];
                            igEdge = s;
                        }
                        else if (s.Split('-')[0].ToLower().Equals(pgN.Name.ToLower()))
                        {
                            ogEdge = s;
                        }
                    }
                    //Step3: Now we have a Node from which an edge is coming to PVPanel. We need to use this node to get siblings for PVPanel
                    List<string> nodesToBeVisited = new List<string>();
                    IEnumerable<string> results2 = (EdgeData.Keys).ToList<string>().FindAll(x => { if (x.ToLower().Contains(sourceBus.ToLower())) { return true; } else { return false; } });
                    foreach( string s1 in results2)
                    {
                        if (pDelEdges.Contains(s1))
                            continue;
                        if (s1.Split('-')[0].ToLower().Equals(sourceBus.ToLower()))
                        {
                            if (!s1.Split('-')[1].ToLower().Equals(pgN.Name.ToLower()))
                            {
                                nodesToBeVisited.Add(s1.Split('-')[1]);
                            }
                        }
                    }
                    //Step4: Now we have nodesToBeVisited. Use them to get loads..
                    List<string> loads = new List<string>();
                    foreach(string ntVisited in nodesToBeVisited)
                    {
                        GetLoadsForBus(ntVisited, ref loads);
                    }
                    //Step5: Pair Loads with PV Panel.
                    //Step5.1 Select a load
                    string selectedLoad = string.Empty;
                    //I am hard coding a load now
                    if (loads.Count == 1)
                        selectedLoad = loads[0];
                    else if (loads.Count > 1 & loads.Count < 2)
                        selectedLoad = loads[1];              
                    else
                        if (loads.Count > 2)
                        selectedLoad = loads[2];
                    //Ideally I need to check Loads details..for later...
                    foreach (string load in loads)
                    {
                        Node lN = symbGraph.FindNode(load);
                    }
                    //Process only if we have a selected load.
                    if (!string.IsNullOrEmpty(selectedLoad))
                    {
                        //Step5.2 Remove incoming and outgoing edges for PVNode from Graph DS.
                        EdgeData.Remove(ogEdge);
                        EdgeData.Remove(igEdge);
                        //Step5.3 Remove incoming Edges for selected load from GraphDS.
                        IEnumerable<string> ieLoads = (EdgeData.Keys).ToList<string>().FindAll(x => { if (x.ToLower().Contains(selectedLoad.ToLower())) { return true; } else { return false; } });
                        foreach (string s1 in ieLoads)
                        {
                            if (s1.Split('-')[1].ToLower().Equals(selectedLoad.ToLower()))
                            {
                                EdgeData.Remove(s1);
                            }
                        }
                        //Step5.4 Pair Load with PV Panel
                        Edge ce1 = (Edge)symbGraph.AddEdge(symbNodes[selectedLoad].ToString(), symbNodes[pgN.Name].ToString());
                        ce1.Attr.Color = Microsoft.Glee.Drawing.Color.IndianRed;

                        // Then add the load bus as a edge to the original bus 
                        graphDS.AddDirectedEdge(selectedLoad, pgN.Name);
                        EdgeData.Add(string.Format("{0}-{1}", selectedLoad, pgN.Name),
                                new GraphEdge { Head = NodeData[selectedLoad], Tail = NodeData[pgN.Name], EType = EdgeType.NonExec });

                    }
                    //Can we JUST isolate PV Panel if we dont have Loads? Yes I am doing this. 
                    else
                    {
                        //Remove incoming and outgoing edges for PVNode from Graph DS.
                        EdgeData.Remove(ogEdge);
                        EdgeData.Remove(igEdge);
                    }
                }
            }
            //Finally create symbolic graph
            CreateSymbolicGraph();
        }
        void GetLoadsForBus(string nVisisted, ref List<string> loads)
        {
            IEnumerable<string> results2 = (EdgeData.Keys).ToList<string>().FindAll(x => { if (x.ToLower().Contains(nVisisted.ToLower())) { return true; } else { return false; } });
            foreach (string s1 in results2)
            {
                if (s1.Split('-')[0].ToLower().Equals(nVisisted.ToLower()))
                {
                    if (NodeData[s1.Split('-')[1]].NType == NodeType.Bus)
                        GetLoadsForBus(s1.Split('-')[1], ref loads);
                    else if (NodeData[s1.Split('-')[1]].NType == NodeType.Load)
                        loads.Add(s1.Split('-')[1]);
                }
            }
        }

        #region Code from OpBr
        /// <summary>
        /// This function implements Edmonds Algorithm to remove cycles and create operational branching.
        /// </summary>
        /// <param name="timeSlot"></param>
        public void GoLiveV21(string timeSlot)
        {
            //for vertices more than one incoming degree
            List<string> verticesWithMoreInDegree = new List<string>();
            //we only use GraphDS here.
            //Step -1: find In and Out degree of all vertices in graph
            GraphUtilites.GetInOutDegree(graphDS);

            //Step-2: find out what vertices has more than two incoming edges. 
            foreach (KeyValuePair<string, int> kPair in GraphUtilites.iN)
            {
                if (kPair.Value >= 2)
                    verticesWithMoreInDegree.Add(kPair.Key);
            }

            //Step-2.1 Check from where the incoming vertices are coming. IF they are coming from Load or Bus. 
            Dictionary<string, List<string>> cycleEdges = new Dictionary<string, List<string>>();
            foreach (string sVertex in verticesWithMoreInDegree)
            {
                IEnumerable<string> result = (EdgeData.Keys).Where(x => { if (x.ToLower().Contains(sVertex.ToLower())) { return true; } else { return false; } });
                if (result.Count() == 0)
                {
                    break;
                }
                //Special case for Transformers. we have cycles here only.
                if (NodeData[sVertex].NType == NodeType.Transformer)
                {
                    List<string> edges = new List<string>();
                    foreach (string s in result)
                    {
                        //This condition ensures that the sVertex is at sending end. We are interested only in those edges.
                        //We are not interested in those where sVertex is at receiving end
                        if (string.Equals(sVertex.ToLower(), s.Split('-')[0].ToLower()))
                        {
                            edges.Add(s);
                        }
                    }
                    impactTransformers.Add(sVertex);
                    cycleEdges.Add(sVertex, edges);
                }
                else
                {
                    //sVertex is the one which has multiple edges
                    //Get all Edges where the sVertex participates. there may be many
                    foreach (string s in result)
                    {
                        //This condition ensures that the sVertex is at receiving end. We are interested
                        //only in those edges not in those where sVertex is at sending end
                        if (string.Equals(sVertex.ToLower(), s.Split('-')[1].ToLower()))
                        {
                            string erNode = s.Split('-')[0];
                            //Now see how the sending ends that are Master buses represented with erNode, for sVertex are 
                            //case 1: Typically they dont have incoming but one outgoing
                            if ((!GraphUtilites.iN.ContainsKey(erNode)) & (GraphUtilites.ouT.ContainsKey(erNode) && GraphUtilites.ouT[erNode] == 1))
                            {
                                IEnumerable<string> inResult1 = (EdgeData.Keys).Where(x => { if (x.ToLower().Contains(erNode.ToLower())) { return true; } else { return false; } });
                                if (inResult1.Count() == 0)
                                {
                                    Console.WriteLine("ERROR. We are thiking that node has outgoing edge but it is not.");
                                }
                                foreach (string intS in inResult1)
                                {
                                    if (string.Equals(erNode, intS.Split('-')[0]) & string.Equals(sVertex, intS.Split('-')[1]))
                                    {
                                        //We are removing master bus that has no incoming but only outgoing. 
                                        removableEdges.Add(intS);
                                        removableNodes.Add(erNode);
                                    }
                                }
                            }
                            //Case 2: Sometimes erNode dont have incoming but multiple outgoing
                            else if ((!GraphUtilites.iN.ContainsKey(erNode)) & (GraphUtilites.ouT.ContainsKey(erNode) && GraphUtilites.ouT[erNode] > 1))
                            {
                                IEnumerable<string> inResult1 = (EdgeData.Keys).Where(x => { if (x.ToLower().Contains(erNode.ToLower())) { return true; } else { return false; } });
                                if (inResult1.Count() == 0)
                                {
                                    Console.WriteLine("ERROR. We are thiking that node has multiple outgoing edges but it is not.");
                                }
                                foreach (string intS in inResult1)
                                {
                                    //Remove that edge from erNode -> sVertex. We dont want it 
                                    if (string.Equals(erNode, intS.Split('-')[0]) & string.Equals(sVertex, intS.Split('-')[1]))
                                    {
                                        //We are removing master bus that has no incoming but only outgoing. 
                                        removableEdges.Add(intS);
                                        removableNodes.Add(erNode);
                                    }
                                    //Other edges where receiving end to erNode is NOT sVertex
                                    if (string.Equals(erNode, intS.Split('-')[0]) & !string.Equals(sVertex, intS.Split('-')[1]))
                                    {
                                        //Still remove this edge and erNode
                                        removableEdges.Add(intS);
                                        removableNodes.Add(erNode);
                                        //But need to create new edge
                                        newEdges.Add(string.Format("{0}-{1}", sVertex, intS.Split('-')[1]));
                                    }
                                }
                            }
                            //Both incoming and outgoing...
                            else
                            {

                            }
                        }
                    }
                }
            }

            //Now Create new graph.
            GraphDS newGraph = graphDS.Clone() as GraphDS;
            NewNodeData = new Dictionary<string, GraphNode>(NodeData);
            NewEdgeData = new Dictionary<string, GraphEdge>(EdgeData);
            //Let us remove some nodes which can lead in wrong way 
            RemoveFromGraph(newGraph, NewNodeData, NewEdgeData);

            //Now resolve Cycles
            if (cycleEdges.Count > 0)
            {
                if (!PVPanelDetails.IsProductionTime(timeSlot))
                {
                    //Get PVs
                    IEnumerable<KeyValuePair<string, GraphNode>> pvNames = NewNodeData.Where(x => { if (((GraphNode)x.Value).NType == NodeType.PVPanel) { return true; } else { return false; } });
                    foreach (KeyValuePair<string, GraphNode> kvPair in pvNames)
                    {
                        string pvn = kvPair.Value.Name;
                        removableNodes.Add(pvn);
                        IEnumerable<string> result = (EdgeData.Keys).Where(x => { if (x.ToLower().Contains(pvn.ToLower())) { return true; } else { return false; } });
                        if (result.Count() == 0)
                        {
                            break;
                        }
                        foreach (string s in result)
                        {
                            removableEdges.Add(s);
                        }
                    }
                }
                else
                {
                    pvNamesForProcessing = new Dictionary<string, ImpactAnalysisData>();
                    foreach (KeyValuePair<string, List<string>> pair in cycleEdges)
                    {
                        List<string> impactEdges = new List<string>();
                        List<string> impactNodes = new List<string>();
                        List<ImpactAnalysisData> iaData = new List<ImpactAnalysisData>();
                        ProcessCycleEdges(pair.Value);
                        ProcessLeafNodes(pair.Key, leafNodes, leafEdges);
                        //Get the PVName
                        string pvName = GetPVNodeNameFromNodes(leafNodes);
                        //FindImpact
                        string iNotes = FindImpact(pvName, NewNodeData, NewEdgeData, ref impactNodes, ref impactEdges);
                        pvNamesForProcessing.Add(pvName, new ImpactAnalysisData() { PvName = pvName, IEdges = impactEdges, INodes = impactNodes, INotes = iNotes });
                        //Process Cousins..
                        if (CousinLoads.Count() > 0)
                        {

                        }
                        leafNodes.Clear();
                    }
                    //Process Impact on edges
                    ProcessImpactOnEdges(pvNamesForProcessing, timeSlot, NewNodeData, NewEdgeData);
                }
            }
        }
        string GetPVNodeNameFromNodes(List<string> lNodes)
        {
            string pvPanel = string.Empty;
            IEnumerable<string> n = lNodes.FindAll(x => { if ((graphDS.GetEdgesForNode(x)).Count() > 0) { return true; } else { return false; } });
            foreach (string s in n)
            {
                if ((NodeData[s].NType == NodeType.PVPanel))
                    pvPanel = s;
            }
            return pvPanel;
        }
        string FindImpact(string pvNode, Dictionary<string, GraphNode> nnData, Dictionary<string, GraphEdge> neData, ref List<string> impactNodes, ref List<string> impactEdges)
        {
            Dictionary<String, Capacitor> Capacitors = new Dictionary<string, Capacitor>();
            ReadCapacitorDetails(Capacitors);
            string startNode = pvNode;
            impactNodes.Add(startNode);
            StringBuilder iNotes = new StringBuilder();
            while (true)
            {
                if (string.IsNullOrEmpty(startNode) || string.Equals(startNode, "ckt24"))
                {
                    iNotes.Append(string.Format("Reached Substation Node {0}", startNode));
                    break;
                }
                if (nnData[startNode] is Bus)
                {
                    if (Capacitors.ContainsKey(startNode))
                    {
                        iNotes.Append(string.Format("Bus {0} has Capacitor Attached", startNode));
                        iNotes.Append(Environment.NewLine);
                    }
                }
                if (nnData[startNode] is Transformer)
                {
                    Transformer t = nnData[startNode] as Transformer;
                    if (t.RegControl)
                    {
                        iNotes.Append(string.Format("Transformer {0} has Voltage Regulator Attached", startNode));
                        iNotes.Append(Environment.NewLine);
                    }
                }
                IEnumerable<string> result = (neData.Keys).Where(x => { if (x.ToLower().Contains(startNode.ToLower())) { return true; } else { return false; } });
                if (result.Count() == 0)
                {
                    break;
                }
                foreach (string s in result)
                {
                    string[] subStr = s.Split('-');
                    if (string.Equals(startNode.ToLower(), subStr[1].ToLower()))
                    {
                        if (string.Equals(subStr[0].ToLower(), pvNode.ToLower()))
                        {
                            impactEdges.Add(s);
                            continue;
                        }
                        impactEdges.Add(s);
                        iNotes.Append(string.Format("Node {0} is connected to {1}", startNode, subStr[0]));
                        iNotes.Append(Environment.NewLine);
                        startNode = subStr[0];
                        impactNodes.Add(startNode);
                    }
                }
            }
            return iNotes.ToString();
        }
        List<Line> exeLines = new List<Line>();
        void ProcessImpactOnEdges(Dictionary<string, ImpactAnalysisData> pvNames, string timeSlot, Dictionary<string, GraphNode> nnData, Dictionary<string, GraphEdge> neData)
        {
            Dictionary<string, int> transformers = new Dictionary<string, int>();
            Dictionary<string, int> lines = new Dictionary<string, int>();
            List<GraphNode> pvgNodes = new List<GraphNode>();
            string DPath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
            foreach (KeyValuePair<string, ImpactAnalysisData> pviePair in pvNames)
            {
                List<string> impEdges = pviePair.Value.IEdges;
                pvgNodes.Add(NodeData[pviePair.Key]);

                //Get executiveEdges
                foreach (string ed in impEdges)
                {
                    GraphEdge gEdge = neData[ed];

                    if (gEdge.EType == EdgeType.Exec)
                    {
                        if (gEdge.UserData is Line)
                        {
                            Line exLine = gEdge.UserData as Line;
                            exeLines.Add(exLine);
                            lines[exLine.Id] = 0;
                        }
                    }
                    else
                    {
                        //Here we will have the first edge connecting from PV Panel to Transformer.
                        if (gEdge.Head is Transformer)
                            if (transformers.ContainsKey(((Transformer)gEdge.Head).Name))
                                transformers[((Transformer)gEdge.Head).Name]++;
                            else
                                transformers[((Transformer)gEdge.Head).Name] = 1;
                        if (gEdge.Tail is Transformer)
                            if (transformers.ContainsKey(((Transformer)gEdge.Tail).Name))
                                transformers[((Transformer)gEdge.Tail).Name]++;
                            else
                                transformers[((Transformer)gEdge.Tail).Name] = 1;
                    }
                }
                foreach (Line exeLine in exeLines)
                {
                    lines[exeLine.Id]++;
                    if (exeLine.Head is Transformer)
                        transformers[((Transformer)exeLine.Head).Name]++;
                    if (exeLine.Tail is Transformer)
                        transformers[((Transformer)exeLine.Tail).Name]++;
                }
            }

            List<double> tspvDetails = GetPvDetailsForTimeInvterval(timeSlot);
            DSSScriptWriter dssWriter = new DSSScriptWriter(DPath, transformers, lines);
            dssWriter.CreatePVLoadShapeFile(CreatePVLoadShapeFileName(), tspvDetails);
            dssWriter.CreateAndPlaceMonitorScript(true);
            //Create PV DSS file with new load shape and selected PV Panels.
            //But we cant do this for one-by one...we need to do for all selected PV panels.
            dssWriter.CreatePVLoadFile(pvgNodes, CreatePVLoadFileName(), CreatePVLoadShapeFileName(), tspvDetails.Count());
            //Create Matlab
            MatLabScriptWriter2 msWriter = new MatLabScriptWriter2(DPath, dssWriter.MonitorNames);
            int maxlNum = tspvDetails.IndexOf(tspvDetails.Max());
            string resFilePath = msWriter.WritePVScript(maxlNum, CreatePVLoadFileName());
            //Ask user to execute
            //NOT a GOOD Code: I am using a message box here to see that the Matlab script is executed.
            //in ideal conditions i need to execute the matlab script from here and get the result. 
           
        }
        //Hardcoded name PV load shape
        string CreatePVLoadShapeFileName()
        {
            return string.Format("{0}_{1}.txt", "PVloadshape_7_5MW_Distributed", DateUtilities.GetCurrentDate());
        }
        string CreatePVLoadFileName()
        {
            return string.Format("{0}_{1}.dss", "Ckt24_PV_Distributed_7_5", DateUtilities.GetCurrentDate());
        }
        List<double> pvlDetails = new List<double>();
        public List<double> GetPvDetailsForTimeInvterval(string timeSlot)
        {
            List<double> pvDetails = new List<double>();
            string dirPath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
            string filePath = dirPath + @"\" + Utilities.PVLoadShapeFileName;
            foreach (string line in File.ReadLines(filePath))
            {
                pvlDetails.Add(Double.Parse(line));
            }
            string[] tSlots = timeSlot.Split('-');

            int startHour = (Int32.Parse(tSlots[0].Split(':')[0].ToString()) - Utilities.PVLoadStartHour);
            int startMin = Int32.Parse(tSlots[0].Split(':')[1].ToString());
            int endHour = (Int32.Parse(tSlots[1].Split(':')[0].ToString()) - Utilities.PVLoadStartHour);
            int endMin = Int32.Parse(tSlots[1].Split(':')[1].ToString());

            int startReading = 0;
            int endReading = 0;

            if (startMin == 0)
                startReading = (startHour * 3600);
            else
                startReading = (startHour * 3600) + Utilities.HalfHourReadings;
            if (endMin == 0)
                endReading = (endHour * 3600);
            else
                endReading = (endHour * 3600) + Utilities.HalfHourReadings;

            pvDetails = pvlDetails.GetRange(startReading - 1, ((endReading - startReading) - 1));

            return pvDetails;
        }
        void RemoveFromGraph(GraphDS newGraph, Dictionary<string, GraphNode> NewNodeData, Dictionary<string, GraphEdge> NewEdgeData)
        {
            foreach (string s in removableNodes)
            {
                newGraph.RemoveNode(s);
                NewNodeData.Remove(s);
            }
            foreach (string s in removableEdges)
            {
                NewEdgeData.Remove(s);
            }

            foreach (string s in newEdges)
            {
                newGraph.AddDirectedEdge(s.Split('-')[0], s.Split('-')[1]);
                NewEdgeData.Add(s, new GraphEdge() { Head = NodeData[s.Split('-')[0]], Tail = NodeData[s.Split('-')[1]], EType = EdgeType.NonExec });
            }
            removableNodes.Clear();
            removableEdges.Clear();
            newEdges.Clear();
        }
        void ProcessCycleEdges(List<string> cEdges)
        {
            furEdges.Clear();
            foreach (string ce in cEdges)
            {
                string curVertex = ce.Split('-')[1];//We are sure that the first part is sVertex. 
                IEnumerable<string> result = (EdgeData.Keys).Where(x => { if (x.ToLower().Contains(curVertex.ToLower())) { return true; } else { return false; } });
                if (result.Count() == 0)
                {
                    break;
                }
                foreach (string s in result)
                {
                    if (string.Equals(s.Split('-')[0].ToLower(), curVertex.ToLower()))
                    {
                        if ((NodeData[s.Split('-')[1]].NType == NodeType.Load) || (NodeData[s.Split('-')[1]].NType == NodeType.PVPanel))
                        {
                            leafNodes.Add(s.Split('-')[1]);
                            leafEdges.Add(s);
                        }
                        else
                        {
                            furEdges.Add(s);
                        }
                    }
                }
            }
            if (furEdges.Count > 0)
            {
                ProcessCycleEdges(new List<string>(furEdges));
                furEdges.Clear();
            }
            else
                return;
        }
        void ProcessLeafNodes(string rootNode, List<string> lNodes, List<string> lEdges)
        {
            string pvPanel = string.Empty;
            IEnumerable<string> n = lNodes.FindAll(x => { if ((graphDS.GetEdgesForNode(x)).Count() > 0) { return true; } else { return false; } });
            foreach (string s in n)
            {
                if ((NodeData[s].NType == NodeType.PVPanel))
                    pvPanel = s;
            }

            //This condition deals with specific case where we have only PV attached to Bus no other loads
            if (lNodes.Count() == 1 && string.Equals(lNodes[0].ToLower(), pvPanel.ToLower()))
            {
                //Remove Edge causing the cycle, but the PV Panel is not removed from Bus. 
                //We can give this some other Loads.
                IEnumerable<string> result = (EdgeData.Keys).Where(x => { if (x.ToLower().Contains(pvPanel.ToLower())) { return true; } else { return false; } });
                if (result.Count() == 0)
                {
                    return;
                }
                foreach (string s in result)
                {
                    if (string.Equals(pvPanel.ToLower(), s.Split('-')[0].ToLower()) &
                        string.Equals(rootNode.ToLower(), s.Split('-')[1].ToLower())) //for outging.
                    {
                        removableEdges.Add(s);//removing edge causing cycle. 
                    }
                }
                CousinLoads.Add(new CousinDataHelper() { PVPanel = pvPanel, RootTransf = rootNode });
            }
            else
            {
                //Step 0: Remove Nodes
                IEnumerable<string> result = (EdgeData.Keys).Where(x => { if (x.ToLower().Contains(pvPanel.ToLower())) { return true; } else { return false; } });
                if (result.Count() == 0)
                {
                    return;
                }
                foreach (string s in result)
                {
                    if (string.Equals(pvPanel.ToLower(), s.Split('-')[0].ToLower()) &
                        string.Equals(rootNode.ToLower(), s.Split('-')[1].ToLower())) //for outging.
                    {
                        removableEdges.Add(s);//removing edge causing cycle. 
                    }
                    //Disconnect PV from Graph//Assume there is only one edge ofcourse yes..
                    if (string.Equals(pvPanel.ToLower(), s.Split('-')[1].ToLower()))
                    {
                        removableEdges.Add(s);//removing edge that is connecting / bus to PV panel.
                    }
                }
                //Step 1: Filter nodes for pairing from nodeForPairing collection
                //1.1 Check each node in nodesForPairing in the lEdges for the parent bus
                //1.2 check lEdges for parent bus of PV panel (using pvPanel)
                //1.3 select only those nodes who has the same parent as pvPanel. 
                //result is new shortlisted pair of nodes
                List<GraphNode> shortListedNodes = new List<GraphNode>();
                GraphNode pvNode = null;
                string pvParentEdge = lEdges.Find(x => { if (x.Split('-')[1].Equals(pvPanel)) { return true; } else { return false; } });
                string pvParentBus = pvParentEdge.Split('-')[0];
                pvNode = NodeData[pvParentEdge.Split('-')[1]];
                IEnumerable<string> siblingLoads = lEdges.FindAll(x => { if (x.Split('-')[0].Equals(pvParentBus) && !x.Split('-')[1].Equals(pvNode.Name)) { return true; } else { return false; } });
                foreach (String s in siblingLoads)
                {
                    shortListedNodes.Add(NodeData[s.Split('-')[1]]);
                }
                //Step2: find the pairings using DS layer.
                //2.1 we calculate kVA for each load and see that with kVA of PV panel.
                //2.2 Check power factor for 0.2 variance. 
                IPairMatching pairMatching = new PairMaker();
                if (shortListedNodes.Count > 0)
                {
                    List<GraphNode> pairs = new List<GraphNode>();
                    pairs = pairMatching.PartnersForPVLoad(pvNode, shortListedNodes);
                    foreach (GraphNode pNode in pairs)
                    {
                        newEdges.Add(string.Format("{0}-{1}", pvPanel, pNode.Name));
                        IEnumerable<string> result2 = (EdgeData.Keys).Where(x => { if (x.ToLower().Contains(pNode.Name.ToLower())) { return true; } else { return false; } });
                        foreach (string s in result2)
                        {
                            removableEdges.Add(s);
                        }
                    }
                    if (pairMatching.CanStillServe)
                        CousinLoads.Add(new CousinDataHelper() { PVPanel = pvPanel, RootTransf = rootNode });
                }
                else
                {
                    //reaise exception??
                }
            }
        }
        public void ReadCapacitorDetails(Dictionary<String, Capacitor> Capacitors)
        {
            string dirPath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
            if (string.IsNullOrEmpty(dirPath))
            {
                return;
            }
            //Add Buses
            string filePath = dirPath + @"\" + Utilities.CapacitorFileName;
            foreach (string line in File.ReadLines(filePath))
            {
                if (line.StartsWith("!") || string.IsNullOrEmpty(line) || line.StartsWith("\t"))
                    continue;
                string lLine = line.Replace(' ', '\t');
                string[] capDetails = lLine.Split('\t');
                Capacitor cap = new Capacitor();
                foreach (string token in capDetails)
                {
                    if (string.Equals(token, "New"))
                        continue;
                    if (string.Equals((token.Split('.')[0]).ToLower(), "Capacitor") || (token.Split('.')[0]).Contains("Capacitor"))
                        cap.Name = LineSegmentParsers.NameOfObject(token);
                    if (string.Equals((token.Split('=')[0]).ToLower(), "phases"))
                        cap.Phases = Int32.Parse(Utilities.RemoveComments(token.Split('=')[1]));
                    if (string.Equals((token.Split('=')[0]).ToLower(), "kvar"))
                        cap.kvar = Double.Parse(Utilities.RemoveComments(token.Split('=')[1]));
                    if (string.Equals((token.Split('=')[0]).ToLower(), "bus1"))
                        cap.Bus = Utilities.RemoveComments(token.Split('=')[1]);
                    if (string.Equals((token.Split('=')[0]).ToLower(), "kv"))
                        cap.kV = Double.Parse(Utilities.RemoveComments(token.Split('=')[1]));
                    if (string.Equals((token.Split('=')[0]).ToLower(), "conn"))
                    {
                        string tStr = Utilities.RemoveComments(token.Split('=')[1]);
                        if (tStr.ToLower().Equals("wye"))
                            cap.CapacitorConnType = ConnectionType.Wye;
                        else if (tStr.ToLower().Equals("delta"))
                            cap.CapacitorConnType = ConnectionType.Delta;
                        else
                            cap.CapacitorConnType = ConnectionType.NotAssigned;
                    }
                    if (string.Equals((token.Split('=')[0]).ToLower(), "enabled"))
                    {
                        string tStr = Utilities.RemoveComments(token.Split('=')[1]);
                        cap.Enabled = string.Equals(tStr.ToLower(), "enabled");
                    }
                }
                Capacitors.Add(cap.Bus, cap);
            }
        }
        #endregion
    }

    public class ImpactResultData
    {
        public string PVName { get; set; }
        public GraphDS ImpactGraphDS { get; set; }
        public Graph VisualGraph { get; set; }
        public string ImpactNotes { get; set; }
    }
    public class CousinDataHelper
    {
        public string PVPanel { get; set; }
        public string RootTransf { get; set; }

        public CousinDataHelper()
        {
            PVPanel = string.Empty;
            RootTransf = string.Empty;
        }
    }
    public class ImpactNotes
    {
        public string PVPanel { get; set; }

        public string Impact { get; set; }

        public ImpactNotes() { }

        public override string ToString()
        {
            return string.Format("{0} has Impact on Network {1}:{2}", PVPanel, Environment.NewLine, Impact);
        }
    }
    public interface IPairMatching
    {
        List<GraphNode> PartnersForPVLoad(GraphNode pvLoad, List<GraphNode> normalLoads);
        bool CanStillServe { get; }
    }
    public class PairMaker : IPairMatching
    {
        Dictionary<string, Double> AllocationFactors = new Dictionary<string, double>();
        string dirPath = @"C:\WorkRelated-Offline\Dist_Prog_V2\OpenDSSCircuit\Ckt24_V3";
        bool canStillServe;

        public PairMaker()
        {
            canStillServe = true;
            ReadAllocationFactors();
        }

        public void ReadAllocationFactors()
        {
            if (string.IsNullOrEmpty(dirPath))
            {
                return;
            }
            //Add Buses
            string filePath = dirPath + @"\" + Utilities.AllocationFactorFileName;
            foreach (string line in File.ReadLines(filePath))
            {
                AllocationFactors.Add((line.Split('=')[0]).Split('.')[1], Double.Parse(line.Split('=')[1]));
            }
        }

        public bool CanStillServe
        {
            get { return canStillServe; }
        }

        /// <summary>
        /// We want to compare the kVA and Power Factor for PV Panel and Loads
        /// KVA - Kilo Volt Ampere is the unit of Apparent power. An apparent power is the one for which an equipment is rated with.
        /// So we compare kVA of load and PV Panel, 
        /// For Load we have xfKVA and AllocationFactor. Multiplying both gives kVA
        /// For PVPanel we have already kVA. 
        /// IF kVA of a Load is within the limits of PV panel AND the Diff(PF of load - PF of PV Panel) less than or equal to 0.2 then we make a pair. 
        /// </summary>
        /// <param name="pvLoad"></param>
        /// <param name="normalLoads"></param>
        /// <returns></returns>
        public List<GraphNode> PartnersForPVLoad(GraphNode pvLoad, List<GraphNode> normalLoads)
        {
            List<GraphNode> pairedNodes = new List<GraphNode>();
            PVSystem pvSys = pvLoad as PVSystem;
            double consumedKVA = 0.0;
            foreach(GraphNode g in normalLoads)
            {
                pairedNodes.Add(g);
            }
            //foreach (Load l in normalLoads)
            //{
            //    double loadKVA = l.xfkVA * AllocationFactors[l.Name];

            //    if ((consumedKVA >= pvSys.kVA) | (pvSys.kVA - consumedKVA) < loadKVA)
            //        break;

            //    if (loadKVA <= pvSys.kVA)
            //    {
            //        if ((pvSys.PF - l.PF) <= 0.2)
            //        {
            //            pairedNodes.Add(l);
            //            consumedKVA += loadKVA;
            //        }
            //    }
            //}
            //this.canStillServe = ((pvSys.kVA - consumedKVA) > (pvSys.kVA * 0.2)) ? true : false;
            return pairedNodes;
        }
    }
    public static class DSUtilityClass
    {
        //This Percentage is the one that determines whether a PVPanel can still serve. 
        public static int SSPercentage = 20;
    }
}
