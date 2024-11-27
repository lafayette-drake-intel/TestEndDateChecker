namespace TestEndDateChecker
{
    partial class LotStatusForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LotStatusForm));
            this.LotStatusDataGridView = new System.Windows.Forms.DataGridView();
            this.ManualRefreshButton = new System.Windows.Forms.Button();
            this.RefreshStatusProgressBar = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.LotStatusDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // LotStatusDataGridView
            // 
            this.LotStatusDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LotStatusDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LotStatusDataGridView.Location = new System.Drawing.Point(12, 12);
            this.LotStatusDataGridView.Name = "LotStatusDataGridView";
            this.LotStatusDataGridView.Size = new System.Drawing.Size(439, 256);
            this.LotStatusDataGridView.TabIndex = 0;
            // 
            // ManualRefreshButton
            // 
            this.ManualRefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ManualRefreshButton.Location = new System.Drawing.Point(12, 274);
            this.ManualRefreshButton.Name = "ManualRefreshButton";
            this.ManualRefreshButton.Size = new System.Drawing.Size(101, 23);
            this.ManualRefreshButton.TabIndex = 1;
            this.ManualRefreshButton.Text = "Manual Refresh";
            this.ManualRefreshButton.UseVisualStyleBackColor = true;
            this.ManualRefreshButton.Click += new System.EventHandler(this.ManualRefreshButton_Click);
            // 
            // RefreshStatusProgressBar
            // 
            this.RefreshStatusProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RefreshStatusProgressBar.Location = new System.Drawing.Point(119, 274);
            this.RefreshStatusProgressBar.Name = "RefreshStatusProgressBar";
            this.RefreshStatusProgressBar.Size = new System.Drawing.Size(320, 23);
            this.RefreshStatusProgressBar.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LotStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 305);
            this.Controls.Add(this.RefreshStatusProgressBar);
            this.Controls.Add(this.ManualRefreshButton);
            this.Controls.Add(this.LotStatusDataGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LotStatusForm";
            this.Text = "Lot Status";
            this.Load += new System.EventHandler(this.LotStatusForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LotStatusDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView LotStatusDataGridView;
        private System.Windows.Forms.Button ManualRefreshButton;
        private System.Windows.Forms.ProgressBar RefreshStatusProgressBar;
        private System.Windows.Forms.Timer timer1;
    }
}