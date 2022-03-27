using Microsoft.Glee.Drawing;
using SimulationTool.DataStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimulationTool.OpenDSSParser
{
    public class DSSFileParser
    {
        List<string> dssFiles = new List<string>();
        List<string> IgnoreEntries = new List<string>();
        List<string> KWEntries = new List<string>();
        List<string> ActionEntries = new List<string>();
        List<CircuitEntry> CEntries = new List<CircuitEntry>();
        List<string> nodes;
        List<string> edges;
        Dictionary<string, long> symbNodes = new Dictionary<string, long>();
        GraphDS graphDS;
        Graph symbGraph;
        Dictionary<string,string> excludedList;

        Dictionary<string, GraphNode> NodeData;
        Dictionary<string, GraphEdge> EdgeData;

        public Graph SymbGraph
        {
            get { return symbGraph; }
        }

        public DSSFileParser()
        {
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
            foreach(string res in rootNode.CEEntries)
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
            foreach(CircuitEntry ce in loads)
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
                foreach(string cStr in tr.CEEntries)
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
            catch(Exception ex)
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
                string bus1 = e.Split('-')[0].Trim().Split('.')[0].Trim()+".1.2.3";
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
            foreach(string s in nodes)
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
    }

}
