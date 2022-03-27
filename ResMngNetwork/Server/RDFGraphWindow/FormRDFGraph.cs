using DataSerailizer;
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

namespace Server.RDFGraph
{
    public partial class FormRDFGraph : Form
    {
        ToolTip toolTip1 = new ToolTip();
        Graph LVDNGraph { get; set; }

        public FormRDFGraph()
        {
            LVDNGraph = null;
            InitializeComponent();
            this.toolTip1.Active = true;
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
        }
        public FormRDFGraph(Graph symbGraph) : this()
        {
            this.LVDNGraph = symbGraph;
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
                        string gEdge = (string)edg.UserData;
                        if (gEdge != null)
                        {
                            selectedObjectAttr = (gViewer.SelectedObject as Edge).Attr.Clone();
                            (gViewer.SelectedObject as Edge).Attr.Color = Microsoft.Glee.Drawing.Color.Magenta;
                            (gViewer.SelectedObject as Edge).Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Magenta;

                            Edge edge = gViewer.SelectedObject as Edge;
                            this.propertyGrid1.SelectedObject = (string)edge.UserData;
                            //here you can use e.Attr.Id or e.UserData to get back to you data
                        }
                    }
                    else if (selectedObject is Node)
                    {
                        Node curNode = selectedObject as Node;
                        SemanticStructure gNode = (SemanticStructure)curNode.UserData;
                        if (gNode != null)
                        {
                            selectedObjectAttr = (gViewer.SelectedObject as Node).Attr.Clone();
                            (selectedObject as Node).Attr.Color = Microsoft.Glee.Drawing.Color.Magenta;
                            (selectedObject as Node).Attr.Fontcolor = Microsoft.Glee.Drawing.Color.Magenta;
                            this.propertyGrid1.SelectedObject = (SemanticStructure)curNode.UserData;
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

        private void SymbolicGraph_Load(object sender, EventArgs e)
        {
            gViewer.Graph = LVDNGraph;
            this.propertyGrid1.SelectedObject = this.LVDNGraph;
        }
    }
}
