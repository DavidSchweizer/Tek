using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Tek1
{
    public partial class ConfigurationForm : Form
    {
        public TekHeuristics Heuristics;

        StreamWriter sw = new StreamWriter("options.dmp");

        public ConfigurationForm()
        {
            InitializeComponent();
            sw.AutoFlush = true;
        }

        public void DoSaveData()
        {
            for (int i = 0; i < clbHeuristics.Items.Count; i++)
                Heuristics.SetHeuristicEnabled(clbHeuristics.Items[i].ToString(), clbHeuristics.GetItemChecked(i));
            Heuristics.Dump(sw);
        }

        public void DoSetData(TekHeuristics heuristics)
        {
            clbHeuristics.Items.Clear();
            Heuristics = heuristics;
            if (Heuristics == null)
                return;
            Heuristics.Dump(sw);
            List<string> descriptions = Heuristics.GetHeuristicDescriptions();
            foreach (string description in descriptions)
                clbHeuristics.Items.Add(description, Heuristics.GetHeuristicEnabled(description));
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            DoSaveData();        
        }
    }
}
