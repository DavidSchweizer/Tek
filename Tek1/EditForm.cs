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

    public partial class EditForm : Form
    {
        TekEdit View;
        bool _lastShowErrors = false;
        public EditForm()
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
            int rows = (int) nudRows.Value, cols = (int) nudCols.Value;
            if (View.Board == null)
            {
                View.Board = new TekBoard(rows, cols);
            }
            else
            {
                View.ResizeBoard(rows, cols);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (View.Board == null)
                return;
            View.FillRandomAreas();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (View.Board == null)
                return;
            View.ResetBoard();
        }

        private void bLoad_Click_1(object sender, EventArgs e)
        {
            if (ofd1.ShowDialog() == DialogResult.OK)
            {
                View.LoadFromFile(ofd1.FileName);
                this.Text = ofd1.FileName;
            }
        }

        private void bSave_Click_1(object sender, EventArgs e)
        {
            if (View.Board == null)
                return;
            sfd1.FileName = ofd1.FileName;
            sfd1.InitialDirectory = ofd1.InitialDirectory;
            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                View.SaveToFile(sfd1.FileName);
            }
        }

        private void bSolve_Click(object sender, EventArgs e)
        {
            if (!View.Solve())
                MessageBox.Show("can not be solved");

        }
    }

   
    
}
