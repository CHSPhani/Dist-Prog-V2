using DataSerailizer;
using Microsoft.Glee.Drawing;
using Server.DSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.KnowledgeGraph
{
    public class KG
    {
        DBData systemData;
        Dictionary<string, List<NodeData>> nodeData;
        KGraphDS kgDS;
        
        public KG()
        {
            systemData = null;
            nodeData = new Dictionary<string, List<NodeData>>();
            kgDS = null;
        }

        public KG(List<DSNode> activeNodes) : this()
        {
            systemData = activeNodes[0].DataInstance;
            foreach (DSNode dsn in activeNodes)
            {
                nodeData[dsn.UserName] = dsn.DataInstance.NodeData;
            }
            CreateKG();
        }

        public Graph SymbolicGraph
        {
            get
            {
                return CreateSymbolicGraph();
            }
        }

        public KGraphDS KGraph
        {
            get
            {
                return this.kgDS;
            }
        }
        
        public Graph CreateSymbolicGraph()
        {
            Graph gph = new Graph("KnowledgeGraph");
            List<string> gNodes = new List<string>();
            Dictionary<string, List<string>> gEdges = new Dictionary<string, List<string>>();

            foreach (PGNode pgn in this.KGraph.GetAllNodes())
            {
                gNodes.Add(pgn.PGNName);
                LinkedList<PGNode> edges = this.KGraph.GetEdgesForNode(pgn);
                List<string> edgeNames = new List<string>();
                foreach(PGNode ed in edges)
                {
                    edgeNames.Add(ed.PGNName);
                }
                gEdges[pgn.PGNName] = edgeNames;
            }
            
            foreach(string s in gNodes)
            {
                gph.AddNode(s);
            }
            foreach (KeyValuePair<string,List<string>> kvP in gEdges)
            {
                string sourceStr = kvP.Key;
                foreach(String s in kvP.Value)
                {
                    gph.AddEdge(sourceStr, s);
                }
            }
            int count = 1;
            foreach (List<NodeData> nD in this.nodeData.Values)
            {
                foreach (NodeData nData in nD)
                {
                    string sNode = string.Format("DF-{0}", count++);
                    string tNode = gNodes.Find((g) => { if (g.Equals(nData.VerifiedDataSet)) { return true; } else { return false; } });
                    gph.AddEdge(sNode, tNode);
                }
                //count = 1;
            }
            return gph;
        }

        public void CreateKG()
        {
            kgDS = new KGraphDS();
            foreach (OClass oCls in systemData.OwlData.OWLClasses)
            {
                kgDS.AddNode(new PGNode(oCls.CName));
            }
            foreach (OObjectProperty ooP in systemData.OwlData.OWLObjProperties)
            {
                PGNode targetN = null;
                PGNode sourceN = null;
                foreach (OChildNode ocNode in ooP.OPChildNodes)
                {
                    if (ocNode.CNType.Equals("rdfs:range"))
                    {
                        targetN = kgDS.GetNodeByName(ocNode.CNName);
                    }
                    if (ocNode.CNType.Equals("rdfs:domain"))
                    {
                        sourceN = kgDS.GetNodeByName(ocNode.CNName);
                    }
                }
                kgDS.AddDirectedEdge(sourceN, targetN);
            }
        }
    }

    /// <summary>
    /// This is Knowledge Graph Data Structure in Adjacency List representation. 
    /// In Addition we maintain edgedata in a separate Dictionary so that edge data 
    /// can be obtained in constant time
    /// </summary>
    public class KGraphDS : ICloneable
    {
        Dictionary<PGNode, LinkedList<PGNode>> knowledgeGraph;
        Dictionary<string, KGEdgeData> edgeData;
        public KGraphDS()
        {
            knowledgeGraph = new Dictionary<PGNode, LinkedList<PGNode>>();
            edgeData = new Dictionary<string, KGEdgeData>();
        }

        public KGraphDS(Dictionary<PGNode, LinkedList<PGNode>> clGraph) : this()
        {
            this.knowledgeGraph = clGraph;
        }

        public bool AddNode(PGNode pgNode)
        {
            if (!knowledgeGraph.ContainsKey(pgNode))
            {
                knowledgeGraph.Add(pgNode, new LinkedList<PGNode>());
                return true;
            }
            return false;
        }
        public bool RemoveNode(PGNode pgNode)
        {
            if (knowledgeGraph.ContainsKey(pgNode))
            {
                knowledgeGraph.Remove(pgNode);
                return true;
            }
            return false;
        }
        public bool AddDirectedEdge(PGNode pgNode1, PGNode pgNode2)
        {
            if (pgNode1 == null || pgNode2 == null)
                return false;
            if ((GetEdgesForNode(pgNode1)).Contains(pgNode2))
                return false;
            if (!knowledgeGraph.ContainsKey(pgNode1))
                AddNode(pgNode1);
            knowledgeGraph[pgNode1].AddLast(pgNode2);
            AddEdgeData(pgNode1, pgNode2);
            return true;
        }

        public bool AddEdgeData(PGNode sNode, PGNode tNode)
        {
            string edKey = string.Format("{0}-{1}", sNode.PGNName, tNode.PGNName);
            edgeData[edKey] = null;
            return false;
        }
        public int GetAllNodesWithOutEdges()
        {
            List<PGNode> orphanNodes = new List<PGNode>();
            foreach (PGNode nodeName in knowledgeGraph.Keys)
            {
                if ((knowledgeGraph[nodeName]).Count == 0)
                    orphanNodes.Add(nodeName);
            }
            return orphanNodes.Count();
        }
        public List<PGNode> GetAllNodesWithEdges()
        {
            List<PGNode> nodes = new List<PGNode>();
            foreach (PGNode nodeName in knowledgeGraph.Keys)
            {
                if ((knowledgeGraph[nodeName]).Count != 0)
                    nodes.Add(nodeName);
            }
            return nodes;
        }
        //Return All Nodes
        public List<string> GetAllNodeNamess()
        {
            List<string> nNames = new List<string>();
            foreach (PGNode pgn in knowledgeGraph.Keys)
                nNames.Add(pgn.PGNName);
            return nNames;
        }
        public List<PGNode> GetAllNodes()
        {
            List<PGNode> nNames = new List<PGNode>();
            foreach (PGNode pgn in knowledgeGraph.Keys)
                nNames.Add(pgn);
            return nNames;
        }

        public PGNode GetNodeByName(string nName)
        {
            PGNode pgN = null;
            foreach (PGNode pgn in knowledgeGraph.Keys)
                if (pgn.PGNName == nName)
                {
                    pgN = pgn;
                    break;
                }
            return pgN;
        }

        //return all Edges
        public List<string> GetAllEdgesS()
        {
            List<string> Edges = new List<string>();
            foreach (PGNode key in knowledgeGraph.Keys)
            {
                LinkedList<PGNode> edges = knowledgeGraph[key];
                foreach (PGNode s in edges)
                {
                    Edges.Add(string.Format("{0},{1}", key.PGNName, s.PGNName));
                }
            }
            return Edges;
        }

        public List<PGNode> GetAllEdgesN()
        {
            List<PGNode> Edges = new List<PGNode>();
            foreach (PGNode key in knowledgeGraph.Keys)
            {
                LinkedList<PGNode> edges = knowledgeGraph[key];
                foreach (PGNode s in edges)
                {
                    Edges.Add(s);
                }
            }
            return Edges;
        }

        public long GetNodeCount()
        {
            return knowledgeGraph.Keys.Count();
        }
        public bool IsNodePresent(PGNode nodeName)
        {
            return knowledgeGraph.ContainsKey(nodeName);
        }
        public LinkedList<PGNode> GetEdgesForNode(PGNode nodeName)
        {
            if (knowledgeGraph.ContainsKey(nodeName))
                return knowledgeGraph[nodeName];
            return null;
        }
        //Prints the Adjacency list Graph
        public List<string> PrintGraph()
        {
            List<string> adjListEntries = new List<string>();
            StringBuilder listEntry = new StringBuilder();
            foreach (PGNode node in knowledgeGraph.Keys)
            {
                listEntry.Append(string.Format("{0}->", node));
                LinkedList<PGNode> edges = knowledgeGraph[node];
                foreach (PGNode eNode in edges)
                {
                    listEntry.Append(string.Format("{0}-> ", eNode.PGNName));
                }
                adjListEntries.Add(listEntry.ToString());
                listEntry.Clear();
            }
            return adjListEntries;
        }
        public object Clone()
        {
            return new KGraphDS(this.knowledgeGraph);
        }
    }

    /// <summary>
    /// This is Property Graph Node
    /// </summary>
    public class PGNode
    {
        public string PGNName { get; set; }

        public KGNodeData KgNodeData { get; set; }

        public PGNode(string nName)
        {
            PGNName = nName;
            KgNodeData = null;
        }

        public override string ToString()
        {
            return this.PGNName;
        }
    }
    public interface KGNodeData { }

    public interface KGEdgeData { }

}


