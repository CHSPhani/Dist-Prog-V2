using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationTool.DataStructure
{
    /// <summary>
    /// This is Grah Data structure definition using Adj List representation
    /// </summary>
    /// 
    public static class GraphUtilites
    {
        public static GraphDS ReverseGraphEdges(GraphDS inputGraph)
        {
            GraphDS reverseGraph = new GraphDS();
            //Reverse Logic
            //1. go through all edges (LinkedList) of each main node (n1)
            //2. Make the node in edge as main node and enter that to new Graph.
            //3. Create edge from that node (n2) to n1.
            //4. Enter new edge into graphDS. 

            List<string> oldNodeNames = inputGraph.GetAllNodeNamess();
            LinkedList<string> oldEdges = null;
            foreach (string oldNodeName in oldNodeNames)
            {
                oldEdges = inputGraph.GetEdgesForNode(oldNodeName);
                foreach (string nodeInOldEdge in oldEdges)
                {
                    reverseGraph.AddNode(nodeInOldEdge);
                    reverseGraph.AddDirectedEdge(nodeInOldEdge, oldNodeName);
                }
            }
            return reverseGraph;
        }
        public static string DfsPath = string.Empty;
        // The function to do DFS traversal.  
        // It uses recursive DFSUtil()  
        public static void DFS(GraphDS inputGraph, string v)
        {
            // Mark all the vertices as not visited 
            // (set as false by default in c#)  
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            DfsPath = string.Empty;
            // Call the recursive helper function  
            // to print DFS traversal  
            DFSUtil(inputGraph, v, visited);
        }
        /// <summary>
        /// this function is used by DFS
        /// We use Graph adjacent edges to move around graph
        /// </summary>
        /// <param name="v"></param>
        /// <param name="visited"></param>
        static void DFSUtil(GraphDS inputGraph, string v, Dictionary<string, bool> visited)
        {
            // Mark the current node as visited 
            // and print it  
            visited[v] = true;
            DfsPath = DfsPath + v + "£";

            // Recur for all the vertices  
            // adjacent to this vertex  
            List<string> vList = new List<string>();
            LinkedList<string> result = inputGraph.GetEdgesForNode(v);
            if (result == null)
                return;

            foreach (string s in result)
            {
                string[] subStr = s.Split('-');
                if (!visited.ContainsKey(subStr[0]) || !visited[subStr[0]])
                {
                    vList.Add(subStr[0]);
                    visited[subStr[0]] = false;
                }
            }

            foreach (var n in vList)
            {
                if (!visited[n])
                    DFSUtil(inputGraph, n, visited);
            }
        }
        //These dictionaries tells In and Out degree of nodes.
        public static Dictionary<string, int> iN = new Dictionary<string, int>();
        public static Dictionary<string, int> ouT = new Dictionary<string, int>();
        public static void GetInOutDegree(GraphDS inputGraph)
        {
            iN.Clear();
            ouT.Clear();
            foreach (string sKey in inputGraph.GetAllNodeNamess())
            {
                findInOutDegree(inputGraph.GetEdgesForNode(sKey), sKey);
            }
        }
        static void findInOutDegree(LinkedList<string> adjList, string node)
        {
            // Out degree for ith vertex will be the count 
            // of direct paths from i to other vertices 
            ouT[node] = adjList.Count;

            foreach (string internalNode in adjList)
            {
                // Every vertex that has an incoming  
                // edge from i 
                if (iN.ContainsKey(internalNode))
                    iN[internalNode]++;
                else
                    iN[internalNode] = 1;
            }
        }
    }

    public class GraphDS : ICloneable
    {
        Dictionary<string, LinkedList<string>> lvdnGraph;
        public GraphDS()
        {
            lvdnGraph = new Dictionary<string, LinkedList<string>>(StringComparer.InvariantCultureIgnoreCase);
        }
        public GraphDS(Dictionary<string, LinkedList<string>> clGraph)
        {
            this.lvdnGraph = clGraph;
        }
        public bool AddNode(string strNode)
        {
            if (!lvdnGraph.ContainsKey(strNode))
            {
                lvdnGraph.Add(strNode, new LinkedList<string>());
                return true;
            }
            return false;
        }
        public bool RemoveNode(string rNode)
        {
            if (lvdnGraph.ContainsKey(rNode))
            {
                lvdnGraph.Remove(rNode);
                return true;
            }
            return false;
        }
        public bool AddDirectedEdge(string strNode, string edge)
        {
            if (string.IsNullOrEmpty(strNode) || string.IsNullOrEmpty(edge))
                return false;
            if ((GetEdgesForNode(strNode)).Contains(edge))
                return false;
            if (!lvdnGraph.ContainsKey(strNode))
                AddNode(strNode);
            lvdnGraph[strNode].AddLast(edge);
            return true;
        }
        public int GetAllNodesWithOutEdges()
        {
            List<string> orphanNodes = new List<string>();
            foreach (string nodeName in lvdnGraph.Keys)
            {
                if ((lvdnGraph[nodeName]).Count == 0)
                    orphanNodes.Add(nodeName);
            }
            return orphanNodes.Count();
        }
        public List<string> GetAllNodesWithEdges()
        {
            List<string> nodes = new List<string>();
            foreach (string nodeName in lvdnGraph.Keys)
            {
                if ((lvdnGraph[nodeName]).Count != 0)
                    nodes.Add(nodeName);
            }
            return nodes;
        }
        //Return All Nodes
        public List<string> GetAllNodeNamess()
        {
            return lvdnGraph.Keys.ToList<string>();
        }
        //return all Edges
        public List<string> GetAllEdges()
        {
            List<string> Edges = new List<string>();
            foreach (string key in lvdnGraph.Keys)
            {
                LinkedList<string> edges = lvdnGraph[key];
                foreach (string s in edges)
                {
                    Edges.Add(string.Format("{0}-{1}", key, s));
                }
            }
            return Edges;
        }
        public long GetNodeCount()
        {
            return lvdnGraph.Keys.Count();
        }
        public bool IsNodePresent(string nodeName)
        {
            return lvdnGraph.ContainsKey(nodeName);
        }
        public LinkedList<string> GetEdgesForNode(string nodeName)
        {
            if (lvdnGraph.ContainsKey(nodeName))
                return lvdnGraph[nodeName];
            return null;
        }
        //Prints the Adjacency list Graph
        public List<string> PrintGraph()
        {
            List<string> adjListEntries = new List<string>();
            StringBuilder listEntry = new StringBuilder();
            foreach (string node in lvdnGraph.Keys)
            {
                listEntry.Append(string.Format("{0}->", node));
                LinkedList<string> edges = lvdnGraph[node];
                foreach (string eNode in edges)
                {
                    listEntry.Append(string.Format("{0}-> ", eNode));
                }
                adjListEntries.Add(listEntry.ToString());
                listEntry.Clear();
            }
            return adjListEntries;
        }
        public object Clone()
        {
            return new GraphDS(this.lvdnGraph);
        }
    }
}
