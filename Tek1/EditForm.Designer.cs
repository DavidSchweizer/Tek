namespace Tek1
{
    partial class EditForm
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
            this.ofd1 = new System.Windows.Forms.OpenFileDialog();
            this.sfd1 = new System.Windows.Forms.SaveFileDialog();
            this.ttSolve = new System.Windows.Forms.ToolTip(this.components);
            this.bCreate = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.bLoad = new System.Windows.Forms.Button();
            this.bSolve = new System.Windows.Forms.Button();
            this.split = new System.Windows.Forms.SplitContainer();
            this.tc1 = new System.Windows.Forms.TabControl();
            this.tpCreate = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudCols = new System.Windows.Forms.NumericUpDown();
            this.nudRows = new System.Windows.Forms.NumericUpDown();
            this.tpDefine = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.tc1.SuspendLayout();
            this.tpCreate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).BeginInit();
            this.SuspendLayout();
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
            // bCreate
            // 
            this.bCreate.BackgroundImage = global::Tek1.Properties.Resources.CreateBoard;
            this.bCreate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bCreate.Location = new System.Drawing.Point(152, 252);
            this.bCreate.Name = "bCreate";
            this.bCreate.Size = new System.Drawing.Size(44, 43);
            this.bCreate.TabIndex = 4;
            this.bCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ttSolve.SetToolTip(this.bCreate, "Create or resize the board");
            this.bCreate.UseVisualStyleBackColor = true;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            // 
            // bSave
            // 
            this.bSave.BackgroundImage = global::Tek1.Properties.Resources.save_new;
            this.bSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bSave.Location = new System.Drawing.Point(182, 389);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(44, 43);
            this.bSave.TabIndex = 9;
            this.bSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ttSolve.SetToolTip(this.bSave, "Save the current state to a file");
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click_1);
            // 
            // bLoad
            // 
            this.bLoad.BackgroundImage = global::Tek1.Properties.Resources.open_new;
            this.bLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bLoad.Location = new System.Drawing.Point(132, 389);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(44, 43);
            this.bLoad.TabIndex = 8;
            this.bLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ttSolve.SetToolTip(this.bLoad, "Load a new puzzle from a file");
            this.bLoad.UseVisualStyleBackColor = true;
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click_1);
            // 
            // bSolve
            // 
            this.bSolve.BackgroundImage = global::Tek1.Properties.Resources.solve;
            this.bSolve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bSolve.Location = new System.Drawing.Point(40, 443);
            this.bSolve.Name = "bSolve";
            this.bSolve.Size = new System.Drawing.Size(54, 54);
            this.bSolve.TabIndex = 10;
            this.ttSolve.SetToolTip(this.bSolve, "Solve the puzzle");
            this.bSolve.UseVisualStyleBackColor = true;
            this.bSolve.Click += new System.EventHandler(this.bSolve_Click);
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Location = new System.Drawing.Point(0, 0);
            this.split.Name = "split";
            // 
            // split.Panel1
            // 
            this.split.Panel1.SizeChanged += new System.EventHandler(this.panel1_Resize);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.tc1);
            this.split.Size = new System.Drawing.Size(946, 605);
            this.split.SplitterDistance = 613;
            this.split.TabIndex = 10;
            // 
            // tc1
            // 
            this.tc1.Controls.Add(this.tpCreate);
            this.tc1.Controls.Add(this.tpDefine);
            this.tc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc1.Location = new System.Drawing.Point(0, 0);
            this.tc1.Name = "tc1";
            this.tc1.SelectedIndex = 0;
            this.tc1.Size = new System.Drawing.Size(329, 605);
            this.tc1.TabIndex = 0;
            // 
            // tpCreate
            // 
            this.tpCreate.Controls.Add(this.button3);
            this.tpCreate.Controls.Add(this.bSave);
            this.tpCreate.Controls.Add(this.bLoad);
            this.tpCreate.Controls.Add(this.bSolve);
            this.tpCreate.Controls.Add(this.button2);
            this.tpCreate.Controls.Add(this.button1);
            this.tpCreate.Controls.Add(this.bCreate);
            this.tpCreate.Controls.Add(this.label2);
            this.tpCreate.Controls.Add(this.label1);
            this.tpCreate.Controls.Add(this.nudCols);
            this.tpCreate.Controls.Add(this.nudRows);
            this.tpCreate.Location = new System.Drawing.Point(4, 29);
            this.tpCreate.Name = "tpCreate";
            this.tpCreate.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpCreate.Size = new System.Drawing.Size(321, 572);
            this.tpCreate.TabIndex = 0;
            this.tpCreate.Text = "Create Board";
            this.tpCreate.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(20, 242);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 66);
            this.button2.TabIndex = 6;
            this.button2.Text = "reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(201, 229);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 66);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Number of &cols:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of &rows:";
            // 
            // nudCols
            // 
            this.nudCols.Location = new System.Drawing.Point(75, 171);
            this.nudCols.Name = "nudCols";
            this.nudCols.Size = new System.Drawing.Size(120, 26);
            this.nudCols.TabIndex = 1;
            this.nudCols.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // nudRows
            // 
            this.nudRows.Location = new System.Drawing.Point(75, 69);
            this.nudRows.Name = "nudRows";
            this.nudRows.Size = new System.Drawing.Size(120, 26);
            this.nudRows.TabIndex = 0;
            this.nudRows.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // tpDefine
            // 
            this.tpDefine.Location = new System.Drawing.Point(4, 29);
            this.tpDefine.Name = "tpDefine";
            this.tpDefine.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tpDefine.Size = new System.Drawing.Size(320, 572);
            this.tpDefine.TabIndex = 1;
            this.tpDefine.Text = "Define Areas";
            this.tpDefine.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(211, 457);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 66);
            this.button3.TabIndex = 11;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // EditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 605);
            this.Controls.Add(this.split);
            this.KeyPreview = true;
            this.Name = "EditForm";
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.tc1.ResumeLayout(false);
            this.tpCreate.ResumeLayout(false);
            this.tpCreate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRows)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog sfd1;
        private System.Windows.Forms.ToolTip ttSolve;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.OpenFileDialog ofd1;
        private System.Windows.Forms.TabControl tc1;
        private System.Windows.Forms.TabPage tpCreate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudCols;
        private System.Windows.Forms.NumericUpDown nudRows;
        private System.Windows.Forms.TabPage tpDefine;
        private System.Windows.Forms.Button bCreate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.Button bSolve;
        private System.Windows.Forms.Button button3;
    }
}