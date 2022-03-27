using DataSerailizer;
using Microsoft.Glee.Drawing;
using Server.DSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.RDFGraph
{
    public class RDFG
    {
        DBData systemData;
        Dictionary<string, List<NodeData>> nodeData;
        Graph symbGraph;

        public Graph RDFSymbGraph
        {
            get
            {
                return this.symbGraph;
            }
        }

        public RDFG() {
            symbGraph = null;
        }

        public RDFG(List<DSNode> activeNodes) : this()
        {
            systemData = activeNodes[0].DataInstance;
            CreateRDFG();
        }
        
        void CreateRDFG()
        {
            //Just for testing..Not useful code.
            //showing graph
            DataSerailizer.RDFGraph rr = systemData.OwlData.RDFG;
            Dictionary<string, SemanticStructure> noDetails = rr.NODetails;

            symbGraph = new Graph("symGraph");
            symbGraph.CleanNodes();
            symbGraph.Directed = true;
            List<string> nodes = rr.GetAllNodeNamess();
            Dictionary<string, int> codes = new Dictionary<string, int>();
            int counter = 1;
            foreach (string s in nodes)
            {
                codes[s] = counter;
                Node n = (Node)symbGraph.AddNode(counter.ToString());
                n.Attr.Shape = Shape.Box;
                if (rr.NODetails.ContainsKey(s))
                {
                    if (rr.NODetails[s].SSType == SStrType.Class)
                        n.Attr.Fillcolor = Color.Green;
                    n.UserData = rr.NODetails[s];
                }
                n.Attr.Fillcolor = Color.Blue;
                counter++;
            }
            //Write Edges
            symbGraph.Edges.Clear();
            foreach (string s in nodes)
            {
                List<string> edges = rr.GetEdgesForNode(s);
                foreach (string e in edges)
                {
                    Edge ed = symbGraph.AddEdge(codes[s].ToString(), codes[e.Split('-')[1]].ToString());
                    if (rr.NODetails.ContainsKey(s) && rr.NODetails.ContainsKey(e.Split('-')[1]))
                        ed.UserData = string.Format("{0}\n{1}", rr.NODetails[s], rr.NODetails[e.Split('-')[1]]);
                }
            }
            
        }
    }
}
