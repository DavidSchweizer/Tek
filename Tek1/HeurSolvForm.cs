using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tek1
{

    public partial class HeurSolvForm : Form
    {
        TekEdit View;
        bool _lastShowErrors = false;
        bool Canceled = false;
        bool Paused;

        public HeurSolvForm()
        {
            InitializeComponent();
            View = new TekEdit(split.Panel1, new Point(10,10),
                new Point(split.Panel1.ClientRectangle.Width - 10,
                          split.Panel1.ClientRectangle.Height - 10));
        }

        private void bLoad_Click(object sender, EventArgs e)
        {

        }

        private void Button_ToggleValue_Click(object sender, EventArgs e)
        {
            if (View.Board == null)
                return;
            int value = 0;
            if ((sender is Button) && Int32.TryParse((sender as Button).Text, out value))
            {
                View.ToggleSelectedValue(value);
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            View.HandleKeyDown(ref msg, keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void bSolveClick(object sender, EventArgs e)
        {

        }

        private void bReset_Click(object sender, EventArgs e)
        {
            View.ResetValues();
        }

        private void ToggleNoteButton_Click(object sender, EventArgs e)
        {
            int value = 0;
            if ((sender is Button) && Int32.TryParse((sender as Button).Text, out value))
                View.ToggleSelectedNoteValue(value);
        }

        private void bUnPlay_Click(object sender, EventArgs e)
        {
            View.UnPlay();

        }

        private void bTakeSnap_Click(object sender, EventArgs e)
        {
            View.TakeSnapshot(String.Format("snapshot {0}", 1 + View.SnapshotCount()));
        }

        private void bRestoreSnap_Click(object sender, EventArgs e)
        {
            View.RestoreSnapshot("snapshot 1");
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            if (View != null)
            {
                View.SetSize(split.Panel1.Width, split.Panel1.Height);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
        }

        private void cbShowError_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void bCheck_Click(object sender, EventArgs e)
        {
            _lastShowErrors = View.SetShowErrors(!_lastShowErrors);
        }

        private void bDefaultNotes_Click(object sender, EventArgs e)
        {
            View.ShowDefaultNotes();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            TekBoard board;
            board = new TekBoard(6, 6);
            View.Board = board;
            Refresh();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            TekArea area;
            TekFieldView field = View.Selector.CurrentFieldView;
            if (field != null)
            {
                area = View.SelectArea(field.Row, field.Col);
                View.DeleteArea(area);
                Refresh();
            }

        }

        private void bCreate_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void bLoad_Click_1(object sender, EventArgs e)
        {
        }

        private void bSave_Click_1(object sender, EventArgs e)
        {
        }

        private void bSolve_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            View.LoadFromFile("8x8-1.tx");

            using (StreamWriter sw = new StreamWriter("boarddump.dmp"))
            {
                foreach (TekField field in View.Board.values)
                    field.Dump(sw);
            }
            listBox1.Items.Clear();

            View.ShowDefaultNotes();
            TekHeuristics heuristics = new TekHeuristics();
            TekHeuristic heuristic = heuristics.FindHeuristic(View.Board);
            int heurFound = 1;
            Canceled = false;
            while (heuristic != null &&  !Canceled)
            {
                listBox1.Items.Add(String.Format("{0}: {1}", heurFound++, heuristic.AsString()));
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                View.SelectFields(heuristic.HeuristicFields.ToArray());
                if (checkBox1.Checked)
                {
                    Paused = true;
                    while (Paused)
                    {
                        Application.DoEvents(); // Delphi style, is supposed to be unsafe
                        if (Canceled)
                            break;
                    }
                }
                heuristic.ExecuteAction(View.Moves);
                View.Refresh();
                //if (heurFound == 19)
                //{ // this forces an hidden pair, just for test purposes
                //    View.Board.values[3, 4].PossibleValues.Remove(2);
                //}
               heuristic = heuristics.FindHeuristic(View.Board);
            }
            View.Selector.ClearMultiSelect();
            if (View.Board.IsSolved())
                MessageBox.Show("Solved!");
            else
                MessageBox.Show("Can not further be solved using these heuristics...");
                
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            using(StreamWriter sw = new StreamWriter("heurs.dmp"))
            {
                foreach (string s in listBox1.Items)
                        sw.WriteLine(s);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Canceled = true;
        }

        private void bNext_Click(object sender, EventArgs e)
        {
            Paused = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bNext.Enabled = checkBox1.Checked;
            bNext.Visible = checkBox1.Checked;
            bCancel.Enabled = checkBox1.Checked;
            bCancel.Visible = checkBox1.Checked;
        }
    }

   
    
}
