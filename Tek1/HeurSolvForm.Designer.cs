namespace Tek1
{
    partial class HeurSolvForm
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
            this.split = new System.Windows.Forms.SplitContainer();
            this.tc1 = new System.Windows.Forms.TabControl();
            this.tpCreate = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.tc1.SuspendLayout();
            this.tpCreate.SuspendLayout();
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
            this.split.Panel2.Controls.Add(this.tc1);
            this.split.Size = new System.Drawing.Size(763, 413);
            this.split.SplitterDistance = 393;
            this.split.SplitterWidth = 3;
            this.split.TabIndex = 10;
            // 
            // tc1
            // 
            this.tc1.Controls.Add(this.tpCreate);
            this.tc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc1.Location = new System.Drawing.Point(0, 0);
            this.tc1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tc1.Name = "tc1";
            this.tc1.SelectedIndex = 0;
            this.tc1.Size = new System.Drawing.Size(367, 413);
            this.tc1.TabIndex = 0;
            // 
            // tpCreate
            // 
            this.tpCreate.Controls.Add(this.checkBox1);
            this.tpCreate.Controls.Add(this.listBox1);
            this.tpCreate.Controls.Add(this.button3);
            this.tpCreate.Location = new System.Drawing.Point(4, 22);
            this.tpCreate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tpCreate.Name = "tpCreate";
            this.tpCreate.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tpCreate.Size = new System.Drawing.Size(359, 387);
            this.tpCreate.TabIndex = 0;
            this.tpCreate.Text = "Heuristics";
            this.tpCreate.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBox1.ForeColor = System.Drawing.Color.Black;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.Location = new System.Drawing.Point(2, 2);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(355, 342);
            this.listBox1.TabIndex = 12;
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(4, 352);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(50, 27);
            this.button3.TabIndex = 11;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(231, 362);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(77, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Step mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // HeurSolvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 413);
            this.Controls.Add(this.split);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "HeurSolvForm";
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.tc1.ResumeLayout(false);
            this.tpCreate.ResumeLayout(false);
            this.tpCreate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog sfd1;
        private System.Windows.Forms.ToolTip ttSolve;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.OpenFileDialog ofd1;
        private System.Windows.Forms.TabControl tc1;
        private System.Windows.Forms.TabPage tpCreate;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}