namespace HI3_ReplaceAce
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl = new HI3_ReplaceAce.TabControlEx();
            this.introTab = new System.Windows.Forms.TabPage();
            this.safeModeCheckBox = new System.Windows.Forms.CheckBox();
            this.beginButton = new System.Windows.Forms.Button();
            this.introHeader = new System.Windows.Forms.Label();
            this.workTab = new System.Windows.Forms.TabPage();
            this.workDoneButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.doneTab = new System.Windows.Forms.TabPage();
            this.completedLabel = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.introTab.SuspendLayout();
            this.workTab.SuspendLayout();
            this.doneTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.CheckDesignMode = false;
            this.tabControl.Controls.Add(this.introTab);
            this.tabControl.Controls.Add(this.workTab);
            this.tabControl.Controls.Add(this.doneTab);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowTabHeaders = false;
            this.tabControl.Size = new System.Drawing.Size(296, 119);
            this.tabControl.TabIndex = 0;
            // 
            // introTab
            // 
            this.introTab.BackColor = System.Drawing.SystemColors.Control;
            this.introTab.Controls.Add(this.safeModeCheckBox);
            this.introTab.Controls.Add(this.beginButton);
            this.introTab.Controls.Add(this.introHeader);
            this.introTab.Location = new System.Drawing.Point(0, 0);
            this.introTab.Name = "introTab";
            this.introTab.Padding = new System.Windows.Forms.Padding(3);
            this.introTab.Size = new System.Drawing.Size(296, 119);
            this.introTab.TabIndex = 0;
            this.introTab.Text = "introTab";
            // 
            // safeModeCheckBox
            // 
            this.safeModeCheckBox.AutoSize = true;
            this.safeModeCheckBox.Checked = true;
            this.safeModeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.safeModeCheckBox.Location = new System.Drawing.Point(215, 94);
            this.safeModeCheckBox.Name = "safeModeCheckBox";
            this.safeModeCheckBox.Size = new System.Drawing.Size(78, 17);
            this.safeModeCheckBox.TabIndex = 3;
            this.safeModeCheckBox.Text = "Safe Mode";
            this.safeModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // beginButton
            // 
            this.beginButton.Location = new System.Drawing.Point(111, 90);
            this.beginButton.Name = "beginButton";
            this.beginButton.Size = new System.Drawing.Size(75, 23);
            this.beginButton.TabIndex = 1;
            this.beginButton.Text = "Begin";
            this.beginButton.UseVisualStyleBackColor = true;
            this.beginButton.Click += new System.EventHandler(this.beginButton_Click);
            // 
            // introHeader
            // 
            this.introHeader.Location = new System.Drawing.Point(6, 9);
            this.introHeader.Name = "introHeader";
            this.introHeader.Size = new System.Drawing.Size(284, 69);
            this.introHeader.TabIndex = 0;
            this.introHeader.Text = "Welcome to HI3 Replace Ace\r\n\r\nThis app will attempt to replace the ACE BGM theme " +
    "with the GION OST.\r\nTo begin, press the Begin button";
            this.introHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // workTab
            // 
            this.workTab.BackColor = System.Drawing.SystemColors.Control;
            this.workTab.Controls.Add(this.workDoneButton);
            this.workTab.Controls.Add(this.logTextBox);
            this.workTab.Location = new System.Drawing.Point(0, 0);
            this.workTab.Name = "workTab";
            this.workTab.Padding = new System.Windows.Forms.Padding(3);
            this.workTab.Size = new System.Drawing.Size(296, 119);
            this.workTab.TabIndex = 1;
            this.workTab.Text = "workTab";
            // 
            // workDoneButton
            // 
            this.workDoneButton.Enabled = false;
            this.workDoneButton.Location = new System.Drawing.Point(111, 90);
            this.workDoneButton.Name = "workDoneButton";
            this.workDoneButton.Size = new System.Drawing.Size(75, 23);
            this.workDoneButton.TabIndex = 1;
            this.workDoneButton.Text = "Working...";
            this.workDoneButton.UseVisualStyleBackColor = true;
            this.workDoneButton.Click += new System.EventHandler(this.workDoneButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(296, 84);
            this.logTextBox.TabIndex = 0;
            this.logTextBox.Text = "";
            // 
            // doneTab
            // 
            this.doneTab.BackColor = System.Drawing.SystemColors.Control;
            this.doneTab.Controls.Add(this.completedLabel);
            this.doneTab.Location = new System.Drawing.Point(0, 0);
            this.doneTab.Name = "doneTab";
            this.doneTab.Size = new System.Drawing.Size(296, 119);
            this.doneTab.TabIndex = 2;
            this.doneTab.Text = "doneTab";
            // 
            // completedLabel
            // 
            this.completedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.completedLabel.Location = new System.Drawing.Point(0, 0);
            this.completedLabel.Name = "completedLabel";
            this.completedLabel.Size = new System.Drawing.Size(296, 119);
            this.completedLabel.TabIndex = 0;
            this.completedLabel.Text = resources.GetString("completedLabel.Text");
            this.completedLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 119);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "HI3 Replace Ace v1.0";
            this.tabControl.ResumeLayout(false);
            this.introTab.ResumeLayout(false);
            this.introTab.PerformLayout();
            this.workTab.ResumeLayout(false);
            this.doneTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TabControlEx tabControl;
        private System.Windows.Forms.TabPage introTab;
        private System.Windows.Forms.TabPage workTab;
        private System.Windows.Forms.TabPage doneTab;
        private System.Windows.Forms.Button beginButton;
        private System.Windows.Forms.Label introHeader;
        private System.Windows.Forms.Button workDoneButton;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.CheckBox safeModeCheckBox;
        private System.Windows.Forms.Label completedLabel;
    }
}

