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


namespace Server.KnowledgeGraph
{
    public partial class KnowledgeGraph : Form
    {
        ToolTip toolTip1 = new ToolTip();
        
        public KnowledgeGraph()
        {
            InitializeComponent();
            this.toolTip1.Active = true;
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 1000;
            toolTip1.ReshowDelay = 500;
        }

        public KnowledgeGraph(Graph kg) : this()
        {
            gViewer.Graph = kg;
        }
        
        object selectedObjectAttr;
        object selectedObject;
        private void gViewer_SelectionChanged(object sender, EventArgs e)
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
            }
            gViewer.Invalidate();
        }
        private void recalculateLayoutButton_Click(object sender, EventArgs e)
        {
            this.gViewer.Graph = this.propertyGrid1.SelectedObject as Microsoft.Glee.Drawing.Graph;

        }
        private void OperationalBranching_Load(object sender, EventArgs e)
        {
            
        } 
    }
}
