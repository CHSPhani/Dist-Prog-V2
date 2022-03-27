using Microsoft.Glee.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UoB.ToolUtilities.OpenDSSParser;

namespace SimulationTool
{
    public partial class SymbolicGraph : Form
    {
        ToolTip toolTip1 = new ToolTip();
        Graph LVDNGraph { get; set; }

        DSSFileParser dFileParser; 

        public SymbolicGraph()
        {
            LVDNGraph = null;
            dFileParser = null;
            InitializeComponent();
            this.toolTip1.Active = true;
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
        }

        public SymbolicGraph(DSSFileParser dssFileParser) :this()
        {
            this.dFileParser = dssFileParser;
        }
        private void SymbolicGraph_Load(object sender, EventArgs e)
        {
            this.LVDNGraph = dFileParser.SymbGraph;
            gViewer.Graph = LVDNGraph;
            this.propertyGrid1.SelectedObject = this.LVDNGraph;
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void recalculateLayoutButton_Click(object sender, EventArgs e)
        {
            this.gViewer.Graph = this.propertyGrid1.SelectedObject as Microsoft.Glee.Drawing.Graph;
        }

        object selectedObjectAttr;
        object selectedObject;
        private void gViewer_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (selectedObject != null)
                {
                    if (selectedObject is Edge)
                        (selectedObject as Edge).Attr = selectedObjectAttr as EdgeAttr;
                    else if (selectedObject is Node)
                        (selectedObject as Node).Attr = selectedObjectAttr as NodeAttr;

                    selectedObject = null;
                }

                if (gViewer.SelectedObject == null)
                {
                    label1.Text = "No object under the mouse";
                    this.gViewer.SetToolTip(toolTip1, "");

                }
                else
                {
                    selectedObject = gViewer.SelectedObject;

                    if (selectedObject is Edge)
                    {
                        Edge edg = selectedObject as Edge;
                        GraphEdge gEdge = (GraphEdge)edg.UserData;
                        if (gEdge != null)
                        {
                            selectedObjectAttr = (gViewer.SelectedObject as Edge).Attr.Clone();
                            (gViewer.SelectedObject as Edge).Attr.Color = Microsoft.Glee.Drawing.Color.Magenta;
                            (gViewer.SelectedObject as Edge).Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Magenta;
                            switch (gEdge.EType)
                            {
                                case EdgeType.Exec:
                                    {
                                        this.gViewer.SetToolTip(this.toolTip1, String.Format("edge from {0} {1}", gEdge.Head.Name, gEdge.Tail.Name));
                                        if (gEdge.UserData is GraphNode)
                                        {
                                            label1.Text = String.Format("edge: {0} ", (gEdge.UserData as GraphNode).ToString());
                                            this.propertyGrid1.SelectedObject = gEdge.UserData as GraphNode;
                                        }
                                        break;
                                    }
                                case EdgeType.NonExec:
                                    {
                                        this.gViewer.SetToolTip(this.toolTip1, String.Format("edge from {0} {1}", gEdge.Head.Name, gEdge.Tail.Name));
                                        label1.Text = String.Format("edge: {0}", gEdge.ToString());
                                        this.propertyGrid1.SelectedObject = gEdge;
                                        break;
                                    }
                                case EdgeType.NA:
                                default:
                                    {
                                        break;
                                    }
                            }
                            Edge edge = gViewer.SelectedObject as Edge;
                            //here you can use e.Attr.Id or e.UserData to get back to you data
                        }
                    }
                    else if (selectedObject is Node)
                    {
                        Node curNode = selectedObject as Node;
                        GraphNode gNode = (GraphNode)curNode.UserData;
                        if (gNode != null)
                        {
                            selectedObjectAttr = (gViewer.SelectedObject as Node).Attr.Clone();
                            (selectedObject as Node).Attr.Color = Microsoft.Glee.Drawing.Color.Magenta;
                            (selectedObject as Node).Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Magenta;
                            //here you can use e.Attr.Id to get back to your data
                            switch (gNode.NType)
                            {
                                case NodeType.Bus:
                                    {
                                        this.gViewer.SetToolTip(toolTip1, String.Format("node {0}", (gNode as GraphNode).ToString()));
                                        label1.Text = String.Format("node {0}", (gNode as GraphNode).ToString());
                                        break;
                                    }
                                case NodeType.Load:
                                    {
                                        this.gViewer.SetToolTip(toolTip1, String.Format("node {0}", (gNode as GraphNode).ToString()));
                                        label1.Text = String.Format("node {0}", (gNode as GraphNode).ToString());
                                        break;
                                    }
                                case NodeType.PVPanel:
                                    {
                                        this.gViewer.SetToolTip(toolTip1, String.Format("node {0}", (gNode as GraphNode).ToString()));
                                        label1.Text = String.Format("node {0}", (gNode as GraphNode).ToString());
                                        break;
                                    }
                                case NodeType.Substation:
                                    {
                                        this.gViewer.SetToolTip(toolTip1, String.Format("node {0}", (gNode as GraphNode).ToString()));
                                        label1.Text = String.Format("node {0}", (gNode as GraphNode).ToString());
                                        break;
                                    }
                                case NodeType.Transformer:
                                    {
                                        this.gViewer.SetToolTip(toolTip1, String.Format("node {0}", (gNode as GraphNode).ToString()));
                                        label1.Text = String.Format("node {0}", (gNode as GraphNode).ToString());
                                        break;
                                    }
                                case NodeType.NA:
                                    {
                                        this.gViewer.SetToolTip(toolTip1, String.Format("node {0}", gNode.ToString()));
                                        label1.Text = String.Format("node {0}", gNode.ToString());
                                        break;
                                    }
                                default:
                                    {
                                        this.gViewer.SetToolTip(toolTip1, String.Format("node {0}", gNode.ToString()));
                                        label1.Text = String.Format("node {0}", gNode.ToString());
                                        break;
                                    }
                            }
                            this.propertyGrid1.SelectedObject = (GraphNode)curNode.UserData;
                        }
                    }
                }
                gViewer.Invalidate();
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex);
            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1);
            }
        }
    }
}
