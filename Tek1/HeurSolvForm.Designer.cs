﻿namespace Tek1
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
            this.bPause = new System.Windows.Forms.Button();
            this.bStart = new System.Windows.Forms.Button();
            this.split = new System.Windows.Forms.SplitContainer();
            this.tc1 = new System.Windows.Forms.TabControl();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.bLoad = new System.Windows.Forms.Button();
            this.bReset = new System.Windows.Forms.Button();
            this.bNext = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tbPlay = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.playPanel1 = new Tek1.PlayPanel();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.tc1.SuspendLayout();
            this.tpLog.SuspendLayout();
            this.tbPlay.SuspendLayout();
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
            // bPause
            // 
            this.bPause.BackgroundImage = global::Tek1.Properties.Resources.pause;
            this.bPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bPause.Enabled = false;
            this.bPause.Location = new System.Drawing.Point(126, 349);
            this.bPause.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bPause.Name = "bPause";
            this.bPause.Size = new System.Drawing.Size(36, 35);
            this.bPause.TabIndex = 15;
            this.ttSolve.SetToolTip(this.bPause, "Pause Processing");
            this.bPause.UseVisualStyleBackColor = true;
            this.bPause.Click += new System.EventHandler(this.bPauseClick);
            // 
            // bStart
            // 
            this.bStart.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.bStart.BackgroundImage = global::Tek1.Properties.Resources.start;
            this.bStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bStart.Location = new System.Drawing.Point(10, 349);
            this.bStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(36, 35);
            this.bStart.TabIndex = 11;
            this.ttSolve.SetToolTip(this.bStart, "Start or restart heuristic solve");
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStartClick);
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
            this.split.Size = new System.Drawing.Size(981, 413);
            this.split.SplitterDistance = 311;
            this.split.SplitterWidth = 3;
            this.split.TabIndex = 10;
            // 
            // tc1
            // 
            this.tc1.Controls.Add(this.tpLog);
            this.tc1.Controls.Add(this.tbPlay);
            this.tc1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc1.Location = new System.Drawing.Point(0, 0);
            this.tc1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tc1.Name = "tc1";
            this.tc1.SelectedIndex = 0;
            this.tc1.Size = new System.Drawing.Size(667, 413);
            this.tc1.TabIndex = 0;
            // 
            // tpLog
            // 
            this.tpLog.Controls.Add(this.button2);
            this.tpLog.Controls.Add(this.bLoad);
            this.tpLog.Controls.Add(this.bReset);
            this.tpLog.Controls.Add(this.bPause);
            this.tpLog.Controls.Add(this.bNext);
            this.tpLog.Controls.Add(this.checkBox1);
            this.tpLog.Controls.Add(this.listBox1);
            this.tpLog.Controls.Add(this.bStart);
            this.tpLog.Location = new System.Drawing.Point(4, 22);
            this.tpLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tpLog.Name = "tpLog";
            this.tpLog.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tpLog.Size = new System.Drawing.Size(659, 387);
            this.tpLog.TabIndex = 0;
            this.tpLog.Text = "Log";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // bLoad
            // 
            this.bLoad.Location = new System.Drawing.Point(325, 352);
            this.bLoad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bLoad.Name = "bLoad";
            this.bLoad.Size = new System.Drawing.Size(50, 27);
            this.bLoad.TabIndex = 17;
            this.bLoad.Text = "Load";
            this.bLoad.UseVisualStyleBackColor = true;
            this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
            // 
            // bReset
            // 
            this.bReset.BackgroundImage = global::Tek1.Properties.Resources.reset;
            this.bReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.bReset.Location = new System.Drawing.Point(66, 349);
            this.bReset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(36, 35);
            this.bReset.TabIndex = 16;
            this.bReset.TabStop = false;
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click_1);
            // 
            // bNext
            // 
            this.bNext.BackgroundImage = global::Tek1.Properties.Resources.next;
            this.bNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bNext.Enabled = false;
            this.bNext.Location = new System.Drawing.Point(221, 349);
            this.bNext.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bNext.Name = "bNext";
            this.bNext.Size = new System.Drawing.Size(36, 35);
            this.bNext.TabIndex = 14;
            this.bNext.UseVisualStyleBackColor = true;
            this.bNext.Visible = false;
            this.bNext.Click += new System.EventHandler(this.bNext_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(262, 335);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(77, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Step mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
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
            this.listBox1.Size = new System.Drawing.Size(655, 329);
            this.listBox1.TabIndex = 12;
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // tbPlay
            // 
            this.tbPlay.Controls.Add(this.button1);
            this.tbPlay.Controls.Add(this.playPanel1);
            this.tbPlay.Location = new System.Drawing.Point(4, 22);
            this.tbPlay.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbPlay.Name = "tbPlay";
            this.tbPlay.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbPlay.Size = new System.Drawing.Size(659, 387);
            this.tbPlay.TabIndex = 1;
            this.tbPlay.Text = "Play";
            this.tbPlay.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(72, 258);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 88);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // playPanel1
            // 
            this.playPanel1.Location = new System.Drawing.Point(2, 0);
            this.playPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.playPanel1.Name = "playPanel1";
            this.playPanel1.Size = new System.Drawing.Size(209, 209);
            this.playPanel1.TabIndex = 0;
            this.playPanel1.View = null;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(570, 349);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(60, 27);
            this.button2.TabIndex = 18;
            this.button2.Text = "Options";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // HeurSolvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 413);
            this.Controls.Add(this.split);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "HeurSolvForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HeurSolvForm_FormClosed);
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.tc1.ResumeLayout(false);
            this.tpLog.ResumeLayout(false);
            this.tpLog.PerformLayout();
            this.tbPlay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SaveFileDialog sfd1;
        private System.Windows.Forms.ToolTip ttSolve;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.OpenFileDialog ofd1;
        private System.Windows.Forms.TabControl tc1;
        private System.Windows.Forms.TabPage tpLog;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button bPause;
        private System.Windows.Forms.Button bNext;
        private System.Windows.Forms.Button bReset;
        private System.Windows.Forms.Button bLoad;
        private System.Windows.Forms.TabPage tbPlay;
        private PlayPanel playPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}