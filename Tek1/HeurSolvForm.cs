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
            ofd1.FileName = "test.tx";
            DoLoad();
//            List<TekRegion> list = TekRegion.GetCompactRegions(View.Board.values[0, 0]);
  //          using (StreamWriter sw = new StreamWriter("regions.dump"))
    //            TekRegion.DumpList(list, sw);
        }

        void DoLoad()
        {
            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                View.LoadFromFile(ofd1.FileName);
                this.Text = ofd1.FileName;
                initializeHeuristicLog(this.Text);
                DoReset(true);
            }
        }

        private void bLoad_Click(object sender, EventArgs e)
        {
            DoLoad();
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

        void DoReset(bool initial = false)
        {
            if (!initial)
                CloseHeuristicLog();
            listBox1.Items.Clear();
            if (!initial)
                View.ResetValues();
            bStart.Enabled = true;

        }

        private void bReset_Click(object sender, EventArgs e)
        {
            DoReset();
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

        StreamWriter HeuristicLog = null;
        void initializeHeuristicLog(string PuzzleFilename)
        {
            if (HeuristicLog != null)
                CloseHeuristicLog();
            HeuristicLog = new StreamWriter(Path.ChangeExtension(PuzzleFilename, "log"));
            LogHeuristic("start Log {0} at {1}\n", PuzzleFilename, DateTime.Now.ToString("dd MMMM yyyy   H:mm:ss"));            
        }

        void LogHeuristic(string message, params object[] fields)
        {
            if (HeuristicLog == null)
                initializeHeuristicLog(this.Text);
            if (HeuristicLog == null)
                return;
            HeuristicLog.WriteLine(String.Format(message, fields));
            HeuristicLog.Flush();
        }

        void CloseHeuristicLog()
        {
            if (HeuristicLog != null)
            { 
                LogHeuristic("close Log at {0}", DateTime.Now.ToString("dd MMMM yyyy   H:mm:ss"));
                HeuristicLog.Close();
            }
            HeuristicLog = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bStart.Enabled = false;
           
            listBox1.Items.Clear();
            View.ShowDefaultNotes();
            TekHeuristics heuristics = new TekHeuristics();
            TekHeuristic heuristic = heuristics.FindHeuristic(View.Board);
            int heurFound = 1;
            Canceled = false;
            while (heuristic != null &&  !Canceled)
            {
                listBox1.Items.Add(String.Format("{0}: {1}", heurFound, heuristic.AsString()));
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.Refresh();
                LogHeuristic("{0}: {1}", heurFound++, heuristic.AsString());
                View.SelectFields(heuristic.HeuristicFields.ToArray());
                View.HighlightFields(true, heuristic.AffectedFields.ToArray());
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
                View.HighlightFields(false, heuristic.AffectedFields.ToArray());
                View.Refresh();
                //if (heurFound == 19)
                //{ // this forces an hidden pair, just for test purposes
                //    View.Board.values[3, 4].PossibleValues.Remove(2);
                //}
                heuristic = heuristics.FindHeuristic(View.Board);
            }
            View.Selector.ClearMultiSelect();
            if (View.Board.IsSolved())
            {
                MessageBox.Show("Solved!");
                LogHeuristic("Solved!\n");
            }
            else
            {
                MessageBox.Show("Can not further be solved using these heuristics...");
                LogHeuristic("\nCan not further be solved using these heuristics...");
                if (MessageBox.Show("Save state?", "Verify", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    TekBoardParser parser = new TekBoardParser();
                    parser.Export(View.Board, "test.tx");
                }
            }
                
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

        private void bReset_Click_1(object sender, EventArgs e)
        {
            DoReset();
        }

        private void HeurSolvForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseHeuristicLog();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (View.Selector.CurrentFieldView != null)
            {

                List<TekRegion> list = TekRegion.GetCompactRegions(View.Selector.CurrentFieldView.Field);
                using (StreamWriter sw = new StreamWriter("regions.dump"))
                    TekRegion.DumpList(list, sw);
            }

        }
    }

   
    
}
