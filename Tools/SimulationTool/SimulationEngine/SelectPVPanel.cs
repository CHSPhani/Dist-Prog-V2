using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimulationEngine
{
    public partial class SelectPVPanel : Form
    {
        List<string> PVsystems = new List<string>();
        public List<string> SelectedPVSystems = new List<string>();

        public SelectPVPanel()
        {
            InitializeComponent();
            SelectedPVSystems = new List<string>();
        }
        public SelectPVPanel(List<string> pvs) : this()
        {
            this.PVsystems = pvs;
        }


        private void SelectPVPanel_Load(object sender, EventArgs e)
        {
            checkedListBox1.Items.AddRange(PVsystems.ToArray());
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string curStr = checkedListBox1.SelectedItem.ToString();
            if (e.NewValue == CheckState.Checked)
            {
                SelectedPVSystems.Add(curStr);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                if (SelectedPVSystems.Contains(curStr))
                {
                    SelectedPVSystems.Remove(curStr);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (checkedListBox1.CheckedIndices.Count > 0)
                checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[0], false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }
    }
}
