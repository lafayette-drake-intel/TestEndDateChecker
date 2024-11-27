namespace TestEndDateChecker
{
    partial class AutoProcForm_6051
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoProcForm_6051));
            this.ControlsPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AutoProcWebBrowser = new System.Windows.Forms.WebBrowser();
            this.Exitbutton = new System.Windows.Forms.Button();
            this.ConfirmAutoProcButton = new System.Windows.Forms.Button();
            this.AutoProcProgressBar = new System.Windows.Forms.ProgressBar();
            this.AutoDispodataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.TestButton = new System.Windows.Forms.Button();
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
            this.ControlsPanel.Location = new System.Drawing.Point(315, 37);
            this.ControlsPanel.Name = "ControlsPanel";
            this.ControlsPanel.Size = new System.Drawing.Size(77, 233);
            this.ControlsPanel.TabIndex = 189;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AutoProcWebBrowser);
            this.panel1.Location = new System.Drawing.Point(7, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(53, 52);
            this.panel1.TabIndex = 4;
            // 
            // AutoProcWebBrowser
            // 
            this.AutoProcWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AutoProcWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.AutoProcWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.AutoProcWebBrowser.Name = "AutoProcWebBrowser";
            this.AutoProcWebBrowser.Size = new System.Drawing.Size(53, 52);
            this.AutoProcWebBrowser.TabIndex = 0;
            this.AutoProcWebBrowser.Visible = false;
            // 
            // Exitbutton
            // 
            this.Exitbutton.Location = new System.Drawing.Point(1, 181);
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
            this.AutoProcProgressBar.Location = new System.Drawing.Point(10, 37);
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
            this.AutoDispodataGridView.Location = new System.Drawing.Point(10, 59);
            this.AutoDispodataGridView.Name = "AutoDispodataGridView";
            this.AutoDispodataGridView.ReadOnly = true;
            this.AutoDispodataGridView.RowHeadersVisible = false;
            this.AutoDispodataGridView.Size = new System.Drawing.Size(299, 203);
            this.AutoDispodataGridView.TabIndex = 187;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(318, 26);
            this.label1.TabIndex = 186;
            this.label1.Text = "Do you want to pass the lot level hold (6051) and trigger autoproc \r\non these lot" +
    "s? (lots in green are at 6053 and ready to dispo)";
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(93, 268);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(75, 23);
            this.TestButton.TabIndex = 190;
            this.TestButton.Text = "TestButton";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Visible = false;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // AutoProcForm_6051
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 313);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.ControlsPanel);
            this.Controls.Add(this.AutoProcProgressBar);
            this.Controls.Add(this.AutoDispodataGridView);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AutoProcForm_6051";
            this.Text = "Auto Dispo 6051";
            this.Load += new System.EventHandler(this.AutoProcForm_6051_Load);
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
        private System.Windows.Forms.Button TestButton;
    }
}