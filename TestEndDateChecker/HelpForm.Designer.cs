namespace TestEndDateChecker
{
    partial class HelpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpForm));
            this.ContactAppOwnerLink = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CurrentSiteComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.OlaRecipePathTextBox = new System.Windows.Forms.TextBox();
            this.StartingDirOfRecipesTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MarsDBTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.XuesDatabasetextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PWURLTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.PWDispoSummaryTabTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SaveSettingsButton = new System.Windows.Forms.Button();
            this.ParsedUserCurrentSitetextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // ContactAppOwnerLink
            // 
            this.ContactAppOwnerLink.AutoSize = true;
            this.ContactAppOwnerLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContactAppOwnerLink.ForeColor = System.Drawing.Color.Blue;
            this.ContactAppOwnerLink.Location = new System.Drawing.Point(12, 9);
            this.ContactAppOwnerLink.Name = "ContactAppOwnerLink";
            this.ContactAppOwnerLink.Size = new System.Drawing.Size(270, 20);
            this.ContactAppOwnerLink.TabIndex = 0;
            this.ContactAppOwnerLink.Text = "Click Here to contact App Owner";
            this.ContactAppOwnerLink.Click += new System.EventHandler(this.ContactAppOwnerLink_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Site:";
            // 
            // CurrentSiteComboBox
            // 
            this.CurrentSiteComboBox.FormattingEnabled = true;
            this.CurrentSiteComboBox.Items.AddRange(new object[] {
            "D1C",
            "AFO",
            "S24",
            "S11X",
            "S28"});
            this.CurrentSiteComboBox.Location = new System.Drawing.Point(288, 38);
            this.CurrentSiteComboBox.Name = "CurrentSiteComboBox";
            this.CurrentSiteComboBox.Size = new System.Drawing.Size(77, 21);
            this.CurrentSiteComboBox.TabIndex = 2;
            this.CurrentSiteComboBox.SelectedIndexChanged += new System.EventHandler(this.CurrentSiteComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "OLA Recipe Path:";
            // 
            // OlaRecipePathTextBox
            // 
            this.OlaRecipePathTextBox.Location = new System.Drawing.Point(112, 70);
            this.OlaRecipePathTextBox.Name = "OlaRecipePathTextBox";
            this.OlaRecipePathTextBox.Size = new System.Drawing.Size(331, 20);
            this.OlaRecipePathTextBox.TabIndex = 4;
            // 
            // StartingDirOfRecipesTextBox
            // 
            this.StartingDirOfRecipesTextBox.Location = new System.Drawing.Point(112, 96);
            this.StartingDirOfRecipesTextBox.Name = "StartingDirOfRecipesTextBox";
            this.StartingDirOfRecipesTextBox.Size = new System.Drawing.Size(331, 20);
            this.StartingDirOfRecipesTextBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Recipe Dir:";
            // 
            // MarsDBTextBox
            // 
            this.MarsDBTextBox.Location = new System.Drawing.Point(112, 122);
            this.MarsDBTextBox.Name = "MarsDBTextBox";
            this.MarsDBTextBox.Size = new System.Drawing.Size(331, 20);
            this.MarsDBTextBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Mars DB:";
            // 
            // XuesDatabasetextBox
            // 
            this.XuesDatabasetextBox.Location = new System.Drawing.Point(112, 148);
            this.XuesDatabasetextBox.Name = "XuesDatabasetextBox";
            this.XuesDatabasetextBox.Size = new System.Drawing.Size(331, 20);
            this.XuesDatabasetextBox.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Xues MES DB:";
            // 
            // PWURLTextBox
            // 
            this.PWURLTextBox.Location = new System.Drawing.Point(112, 174);
            this.PWURLTextBox.Name = "PWURLTextBox";
            this.PWURLTextBox.Size = new System.Drawing.Size(331, 20);
            this.PWURLTextBox.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Preon URL:";
            // 
            // PWDispoSummaryTabTextBox
            // 
            this.PWDispoSummaryTabTextBox.Location = new System.Drawing.Point(112, 200);
            this.PWDispoSummaryTabTextBox.Name = "PWDispoSummaryTabTextBox";
            this.PWDispoSummaryTabTextBox.Size = new System.Drawing.Size(331, 20);
            this.PWDispoSummaryTabTextBox.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Preon DS URL:";
            // 
            // SaveSettingsButton
            // 
            this.SaveSettingsButton.Location = new System.Drawing.Point(16, 226);
            this.SaveSettingsButton.Name = "SaveSettingsButton";
            this.SaveSettingsButton.Size = new System.Drawing.Size(427, 23);
            this.SaveSettingsButton.TabIndex = 15;
            this.SaveSettingsButton.Text = "Save your Settings";
            this.SaveSettingsButton.UseVisualStyleBackColor = true;
            this.SaveSettingsButton.Click += new System.EventHandler(this.SaveSettingsButton_Click);
            // 
            // ParsedUserCurrentSitetextBox
            // 
            this.ParsedUserCurrentSitetextBox.Location = new System.Drawing.Point(84, 38);
            this.ParsedUserCurrentSitetextBox.Name = "ParsedUserCurrentSitetextBox";
            this.ParsedUserCurrentSitetextBox.ReadOnly = true;
            this.ParsedUserCurrentSitetextBox.Size = new System.Drawing.Size(48, 20);
            this.ParsedUserCurrentSitetextBox.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(166, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Set Site default values:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(359, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Updates (tooltip)";
            this.toolTip1.SetToolTip(this.label9, resources.GetString("label9.ToolTip"));
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 100;
            this.toolTip1.AutoPopDelay = 15000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 20;
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 259);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ParsedUserCurrentSitetextBox);
            this.Controls.Add(this.SaveSettingsButton);
            this.Controls.Add(this.PWDispoSummaryTabTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.PWURLTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.XuesDatabasetextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.MarsDBTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.StartingDirOfRecipesTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OlaRecipePathTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CurrentSiteComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ContactAppOwnerLink);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HelpForm";
            this.Text = "Help and settings";
            this.Load += new System.EventHandler(this.HelpForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ContactAppOwnerLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CurrentSiteComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox OlaRecipePathTextBox;
        private System.Windows.Forms.TextBox StartingDirOfRecipesTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MarsDBTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox XuesDatabasetextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PWURLTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox PWDispoSummaryTabTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button SaveSettingsButton;
        private System.Windows.Forms.TextBox ParsedUserCurrentSitetextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}