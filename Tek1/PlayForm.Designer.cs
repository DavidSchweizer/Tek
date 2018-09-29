namespace Tek1
{
    partial class PlayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.ofd1 = new System.Windows.Forms.OpenFileDialog();
            this.sfd1 = new System.Windows.Forms.SaveFileDialog();
            this.ttSolve = new System.Windows.Forms.ToolTip(this.components);
            this.bCheck = new System.Windows.Forms.Button();
            this.bRestoreSnap = new System.Windows.Forms.Button();
            this.bDefaultNotes = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.bTakeSnap = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.bSolve = new System.Windows.Forms.Button();
            this.bBackspace = new System.Windows.Forms.Button();
            this.bReset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.split = new System.Windows.Forms.SplitContainer();
            this.cbMultipleSelect = new System.Windows.Forms.CheckBox();
            this.button11 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(10, 19);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(37, 35);
            this.button2.TabIndex = 1;
            this.button2.Tag = "1";
            this.button2.Text = "1";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button_ToggleValue_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(10, 58);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(37, 35);
            this.button3.TabIndex = 2;
            this.button3.Tag = "2";
            this.button3.Text = "2";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button_ToggleValue_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(10, 97);
            this.button4.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(37, 35);
            this.button4.TabIndex = 3;
            this.button4.Tag = "3";
            this.button4.Text = "3";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button_ToggleValue_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.Location = new System.Drawing.Point(10, 136);
            this.button5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(37, 35);
            this.button5.TabIndex = 4;
            this.button5.Tag = "4";
            this.button5.Text = "4";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button_ToggleValue_Click);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(10, 175);
            this.button6.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(37, 35);
            this.button6.TabIndex = 5;
            this.button6.Tag = "5";
            this.button6.Text = "5";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.Button_ToggleValue_Click);
            // 
            // ofd1
            // 
            this.ofd1.DefaultExt = "tx";
            this.ofd1.DereferenceLinks = false;
            this.ofd1.FileName = "9x7-1.tx";
            this.ofd1.Filter = "tx files (*.tx)|*.tx|All files (*.*)|*.*";
            this.ofd1.Title = "Open TEK file";
            // 
            // sfd1
            // 
            this.sfd1.CreatePrompt = true;
            this.sfd1.DefaultExt = "tx";
            // 
            // bCheck
            // 
            this.bCheck.BackgroundImage = global::Tek1.Properties.Resources.check;
            this.bCheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bCheck.Location = new System.Drawing.Point(11, 244);
            this.bCheck.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bCheck.Name = "bCheck";
            this.bCheck.Size = new System.Drawing.Size(36, 35);
            this.bCheck.TabIndex = 16;
            this.ttSolve.SetToolTip(this.bCheck, "(Un)Check errors");
            this.bCheck.UseVisualStyleBackColor = true;
            this.bCheck.Click += new System.EventHandler(this.bCheck_Click);
            // 
            // bRestoreSnap
            // 
            this.bRestoreSnap.BackgroundImage = global::Tek1.Properties.Resources.rstore23;
            this.bRestoreSnap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bRestoreSnap.Location = new System.Drawing.Point(145, 153);
            this.bRestoreSnap.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bRestoreSnap.Name = "bRestoreSnap";
            this.bRestoreSnap.Size = new System.Drawing.Size(36, 35);
            this.bRestoreSnap.TabIndex = 14;
            this.ttSolve.SetToolTip(this.bRestoreSnap, "Restore a snapshot");
            this.bRestoreSnap.UseVisualStyleBackColor = true;
            this.bRestoreSnap.Click += new System.EventHandler(this.bRestoreSnap_Click);
            // 
            // bDefaultNotes
            // 
            this.bDefaultNotes.BackgroundImage = global::Tek1.Properties.Resources.notes;
            this.bDefaultNotes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bDefaultNotes.Cursor = System.Windows.Forms.Cursors.Default;
            this.bDefaultNotes.Location = new System.Drawing.Point(83, 28);
            this.bDefaultNotes.Name = "bDefaultNotes";
            this.bDefaultNotes.Size = new System.Drawing.Size(36, 35);
            this.bDefaultNotes.TabIndex = 16;
            this.ttSolve.SetToolTip(this.bDefaultNotes, "Show all (default) notes");
            this.bDefaultNotes.UseCompatibleTextRendering = true;
            this.bDefaultNotes.UseVisualStyleBackColor = true;
            this.bDefaultNotes.Click += new System.EventHandler(this.bDefaultNotes_Click);
            // 
            // bSave
            // 
            this.bSave.BackgroundImage = global::Tek1.Properties.Resources.save_new;
            this.bSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bSave.Location = new System.Drawing.Point(105, 248);
            this.bSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(29, 28);
            this.bSave.TabIndex = 6;
            this.bSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ttSolve.SetToolTip(this.bSave, "Save the current state to a file");
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bTakeSnap
            // 
            this.bTakeSnap.BackgroundImage = global::Tek1.Properties.Resources.snappie2;
            this.bTakeSnap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bTakeSnap.Location = new System.Drawing.Point(105, 153);
            this.bTakeSnap.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bTakeSnap.Name = "bTakeSnap";
            this.bTakeSnap.Size = new System.Drawing.Size(36, 35);
            this.bTakeSnap.TabIndex = 13;
            this.ttSolve.SetToolTip(this.bTakeSnap, "Save a snapshot");
            this.bTakeSnap.UseVisualStyleBackColor = true;
            this.bTakeSnap.Click += new System.EventHandler(this.bTakeSnap_Click);
            // 
            // bLoad
            // 
            this.bLoad.BackgroundImage = global::Tek1.Properties.Resources.open_new;
            this.bLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bLoad.Location = new System.Drawing.Point(72, 248);
            this.bLoad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(29, 28);
            this.bLoad.TabIndex = 0;
            this.bLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ttSolve.SetToolTip(this.bLoad, "Load a new puzzle from a file");
            this.bLoad.UseVisualStyleBackColor = true;
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // bSolve
            // 
            this.bSolve.BackgroundImage = global::Tek1.Properties.Resources.solve;
            this.bSolve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bSolve.Location = new System.Drawing.Point(11, 283);
            this.bSolve.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bSolve.Name = "bSolve";
            this.bSolve.Size = new System.Drawing.Size(36, 35);
            this.bSolve.TabIndex = 7;
            this.ttSolve.SetToolTip(this.bSolve, "Solve the puzzle");
            this.bSolve.UseVisualStyleBackColor = true;
            this.bSolve.Click += new System.EventHandler(this.bSolveClick);
            // 
            // bBackspace
            // 
            this.bBackspace.BackgroundImage = global::Tek1.Properties.Resources.backspace;
            this.bBackspace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bBackspace.Location = new System.Drawing.Point(62, 106);
            this.bBackspace.Name = "bBackspace";
            this.bBackspace.Size = new System.Drawing.Size(36, 35);
            this.bBackspace.TabIndex = 12;
            this.ttSolve.SetToolTip(this.bBackspace, "Undo the last change");
            this.bBackspace.UseCompatibleTextRendering = true;
            this.bBackspace.UseVisualStyleBackColor = true;
            this.bBackspace.Click += new System.EventHandler(this.bUnPlay_Click);
            // 
            // bReset
            // 
            this.bReset.BackgroundImage = global::Tek1.Properties.Resources.reset;
            this.bReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bReset.Location = new System.Drawing.Point(73, 283);
            this.bReset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(36, 35);
            this.bReset.TabIndex = 8;
            this.ttSolve.SetToolTip(this.bReset, "Reset the puzzle");
            this.bReset.UseCompatibleTextRendering = true;
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bDefaultNotes);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(62, 19);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(131, 74);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Notes";
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.Location = new System.Drawing.Point(27, 31);
            this.button8.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(20, 19);
            this.button8.TabIndex = 13;
            this.button8.Tag = "1";
            this.button8.Text = "3";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.ToggleNoteButton_Click);
            // 
            // button10
            // 
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.Location = new System.Drawing.Point(43, 44);
            this.button10.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(20, 19);
            this.button10.TabIndex = 15;
            this.button10.Tag = "1";
            this.button10.Text = "5";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.ToggleNoteButton_Click);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.Location = new System.Drawing.Point(10, 45);
            this.button9.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(20, 19);
            this.button9.TabIndex = 14;
            this.button9.Tag = "1";
            this.button9.Text = "4";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.ToggleNoteButton_Click);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.Location = new System.Drawing.Point(43, 16);
            this.button7.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(20, 19);
            this.button7.TabIndex = 12;
            this.button7.Tag = "1";
            this.button7.Text = "2";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.ToggleNoteButton_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(10, 16);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 19);
            this.button1.TabIndex = 11;
            this.button1.Tag = "1";
            this.button1.Text = "1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ToggleNoteButton_Click);
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Location = new System.Drawing.Point(0, 0);
            this.split.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.split.Name = "split";
            // 
            // split.Panel1
            // 
            this.split.Panel1.SizeChanged += new System.EventHandler(this.panel1_Resize);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.button11);
            this.split.Panel2.Controls.Add(this.cbMultipleSelect);
            this.split.Panel2.Controls.Add(this.bCheck);
            this.split.Panel2.Controls.Add(this.bRestoreSnap);
            this.split.Panel2.Controls.Add(this.groupBox1);
            this.split.Panel2.Controls.Add(this.bSave);
            this.split.Panel2.Controls.Add(this.bTakeSnap);
            this.split.Panel2.Controls.Add(this.button6);
            this.split.Panel2.Controls.Add(this.bLoad);
            this.split.Panel2.Controls.Add(this.bSolve);
            this.split.Panel2.Controls.Add(this.bBackspace);
            this.split.Panel2.Controls.Add(this.button5);
            this.split.Panel2.Controls.Add(this.button2);
            this.split.Panel2.Controls.Add(this.bReset);
            this.split.Panel2.Controls.Add(this.button3);
            this.split.Panel2.Controls.Add(this.button4);
            this.split.Size = new System.Drawing.Size(631, 359);
            this.split.SplitterDistance = 410;
            this.split.SplitterWidth = 3;
            this.split.TabIndex = 10;
            // 
            // cbMultipleSelect
            // 
            this.cbMultipleSelect.AutoSize = true;
            this.cbMultipleSelect.Location = new System.Drawing.Point(103, 106);
            this.cbMultipleSelect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbMultipleSelect.Name = "cbMultipleSelect";
            this.cbMultipleSelect.Size = new System.Drawing.Size(95, 17);
            this.cbMultipleSelect.TabIndex = 17;
            this.cbMultipleSelect.Text = "&Multiple Select";
            this.cbMultipleSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbMultipleSelect.UseVisualStyleBackColor = true;
            this.cbMultipleSelect.CheckedChanged += new System.EventHandler(this.cbMultipleSelect_CheckedChanged);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(131, 324);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 18;
            this.button11.Text = "button11";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // PlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 359);
            this.Controls.Add(this.split);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "PlayForm";
            this.groupBox1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            this.split.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.SaveFileDialog sfd1;
        private System.Windows.Forms.Button bSolve;
        private System.Windows.Forms.Button bReset;
        private System.Windows.Forms.ToolTip ttSolve;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button bBackspace;
        private System.Windows.Forms.Button bTakeSnap;
        private System.Windows.Forms.Button bRestoreSnap;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.OpenFileDialog ofd1;
        private System.Windows.Forms.Button bCheck;
        private System.Windows.Forms.Button bDefaultNotes;
        private System.Windows.Forms.CheckBox cbMultipleSelect;
        private System.Windows.Forms.Button button11;
    }
}