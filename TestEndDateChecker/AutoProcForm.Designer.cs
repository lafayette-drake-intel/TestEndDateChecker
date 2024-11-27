namespace TestEndDateChecker
{
    partial class AutoProcForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoProcForm));
            this.label1 = new System.Windows.Forms.Label();
            this.AutoDispodataGridView = new System.Windows.Forms.DataGridView();
            this.ConfirmAutoProcButton = new System.Windows.Forms.Button();
            this.Exitbutton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AutoProcWebBrowser = new System.Windows.Forms.WebBrowser();
            this.AutoProcProgressBar = new System.Windows.Forms.ProgressBar();
            this.ControlsPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.AutoDispodataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.ControlsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(318, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Do you want to pass the lot level hold (6751) and trigger autoproc \r\non these lot" +
    "s? (lots in green are at 6753 and ready to dispo)";
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
            this.AutoDispodataGridView.TabIndex = 1;
            this.AutoDispodataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AutoDispodataGridView_CellContentClick);
            this.AutoDispodataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.AutoDispodataGridView_CellFormatting);
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
            // AutoProcProgressBar
            // 
            this.AutoProcProgressBar.Location = new System.Drawing.Point(12, 38);
            this.AutoProcProgressBar.Name = "AutoProcProgressBar";
            this.AutoProcProgressBar.Size = new System.Drawing.Size(299, 23);
            this.AutoProcProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.AutoProcProgressBar.TabIndex = 184;
            this.AutoProcProgressBar.Visible = false;
            // 
            // ControlsPanel
            // 
            this.ControlsPanel.Controls.Add(this.panel1);
            this.ControlsPanel.Controls.Add(this.Exitbutton);
            this.ControlsPanel.Controls.Add(this.ConfirmAutoProcButton);
            this.ControlsPanel.Location = new System.Drawing.Point(316, 52);
            this.ControlsPanel.Name = "ControlsPanel";
            this.ControlsPanel.Size = new System.Drawing.Size(83, 220);
            this.ControlsPanel.TabIndex = 185;
            // 
            // AutoProcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 276);
            this.Controls.Add(this.ControlsPanel);
            this.Controls.Add(this.AutoProcProgressBar);
            this.Controls.Add(this.AutoDispodataGridView);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutoProcForm";
            this.Text = "Auto Dispo";
            this.Load += new System.EventHandler(this.AutoProcForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AutoDispodataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ControlsPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView AutoDispodataGridView;
        private System.Windows.Forms.Button ConfirmAutoProcButton;
        private System.Windows.Forms.Button Exitbutton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser AutoProcWebBrowser;
        private System.Windows.Forms.ProgressBar AutoProcProgressBar;
        private System.Windows.Forms.Panel ControlsPanel;
    }
}