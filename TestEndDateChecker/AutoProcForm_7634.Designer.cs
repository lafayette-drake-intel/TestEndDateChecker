namespace TestEndDateChecker
{
    partial class AutoProcForm_7634
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoProcForm_7634));
            this.ControlsPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AutoProcWebBrowser = new System.Windows.Forms.WebBrowser();
            this.Exitbutton = new System.Windows.Forms.Button();
            this.ConfirmAutoProcButton = new System.Windows.Forms.Button();
            this.AutoProcProgressBar = new System.Windows.Forms.ProgressBar();
            this.AutoDispodataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.ControlsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AutoDispodataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ControlsPanel
            // 
            this.ControlsPanel.Controls.Add(this.panel1);
            this.ControlsPanel.Controls.Add(this.Exitbutton);
            this.ControlsPanel.Controls.Add(this.ConfirmAutoProcButton);
            this.ControlsPanel.Location = new System.Drawing.Point(316, 52);
            this.ControlsPanel.Name = "ControlsPanel";
            this.ControlsPanel.Size = new System.Drawing.Size(83, 220);
            this.ControlsPanel.TabIndex = 189;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AutoProcWebBrowser);
            this.panel1.Location = new System.Drawing.Point(1, 59);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(68, 57);
            this.panel1.TabIndex = 4;
            // 
            // AutoProcWebBrowser
            // 
            this.AutoProcWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoProcWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.AutoProcWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.AutoProcWebBrowser.Name = "AutoProcWebBrowser";
            this.AutoProcWebBrowser.Size = new System.Drawing.Size(68, 57);
            this.AutoProcWebBrowser.TabIndex = 0;
            this.AutoProcWebBrowser.Visible = false;
            // 
            // Exitbutton
            // 
            this.Exitbutton.Location = new System.Drawing.Point(1, 168);
            this.Exitbutton.Name = "Exitbutton";
            this.Exitbutton.Size = new System.Drawing.Size(74, 44);
            this.Exitbutton.TabIndex = 3;
            this.Exitbutton.Text = "Go Back, no autoproc";
            this.Exitbutton.UseVisualStyleBackColor = true;
            this.Exitbutton.Click += new System.EventHandler(this.Exitbutton_Click);
            // 
            // ConfirmAutoProcButton
            // 
            this.ConfirmAutoProcButton.Location = new System.Drawing.Point(1, 9);
            this.ConfirmAutoProcButton.Name = "ConfirmAutoProcButton";
            this.ConfirmAutoProcButton.Size = new System.Drawing.Size(74, 44);
            this.ConfirmAutoProcButton.TabIndex = 2;
            this.ConfirmAutoProcButton.Text = "Autoproc the list";
            this.ConfirmAutoProcButton.UseVisualStyleBackColor = true;
            this.ConfirmAutoProcButton.Click += new System.EventHandler(this.ConfirmAutoProcButton_Click);
            // 
            // AutoProcProgressBar
            // 
            this.AutoProcProgressBar.Location = new System.Drawing.Point(12, 38);
            this.AutoProcProgressBar.Name = "AutoProcProgressBar";
            this.AutoProcProgressBar.Size = new System.Drawing.Size(299, 23);
            this.AutoProcProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.AutoProcProgressBar.TabIndex = 188;
            this.AutoProcProgressBar.Visible = false;
            // 
            // AutoDispodataGridView
            // 
            this.AutoDispodataGridView.AllowUserToAddRows = false;
            this.AutoDispodataGridView.AllowUserToDeleteRows = false;
            this.AutoDispodataGridView.AllowUserToResizeColumns = false;
            this.AutoDispodataGridView.AllowUserToResizeRows = false;
            this.AutoDispodataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AutoDispodataGridView.Location = new System.Drawing.Point(12, 61);
            this.AutoDispodataGridView.Name = "AutoDispodataGridView";
            this.AutoDispodataGridView.ReadOnly = true;
            this.AutoDispodataGridView.RowHeadersVisible = false;
            this.AutoDispodataGridView.Size = new System.Drawing.Size(299, 203);
            this.AutoDispodataGridView.TabIndex = 187;
            this.AutoDispodataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AutoDispodataGridView_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(318, 26);
            this.label1.TabIndex = 186;
            this.label1.Text = "Do you want to pass the lot level hold (7634) and trigger autoproc \r\non these lot" +
    "s? (lots in green are at 7635 and ready to dispo)";
            // 
            // AutoProcForm_7634
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 294);
            this.Controls.Add(this.ControlsPanel);
            this.Controls.Add(this.AutoProcProgressBar);
            this.Controls.Add(this.AutoDispodataGridView);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutoProcForm_7634";
            this.Text = "Auto Dispo";
            this.Load += new System.EventHandler(this.AutoProcForm_7634_Load);
            this.ControlsPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AutoDispodataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel ControlsPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser AutoProcWebBrowser;
        private System.Windows.Forms.Button Exitbutton;
        private System.Windows.Forms.Button ConfirmAutoProcButton;
        private System.Windows.Forms.ProgressBar AutoProcProgressBar;
        private System.Windows.Forms.DataGridView AutoDispodataGridView;
        private System.Windows.Forms.Label label1;
    }
}