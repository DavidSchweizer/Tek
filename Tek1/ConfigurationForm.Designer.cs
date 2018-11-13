namespace Tek1
{
    partial class ConfigurationForm
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
            this.clbHeuristics = new System.Windows.Forms.CheckedListBox();
            this.bSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clbHeuristics
            // 
            this.clbHeuristics.FormattingEnabled = true;
            this.clbHeuristics.Location = new System.Drawing.Point(12, 7);
            this.clbHeuristics.Name = "clbHeuristics";
            this.clbHeuristics.Size = new System.Drawing.Size(434, 550);
            this.clbHeuristics.TabIndex = 0;
            // 
            // bSave
            // 
            this.bSave.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.bSave.Location = new System.Drawing.Point(484, 515);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 42);
            this.bSave.TabIndex = 12;
            this.bSave.Text = "Save";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 595);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.clbHeuristics);
            this.Name = "ConfigurationForm";
            this.Text = "ConfigurationForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox clbHeuristics;
        private System.Windows.Forms.Button bSave;
    }
}