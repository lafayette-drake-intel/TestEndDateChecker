using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace TestEndDateChecker
{
    public partial class HelpForm : Form
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void ContactAppOwnerLink_Click(object sender, EventArgs e)
        {
            Outlook.Application oApp = new Outlook.Application();
            Outlook._MailItem oMailItem = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            oMailItem.To = "lafayette.drake@intel.com;spirit@intel.com";
            oMailItem.Subject = "Reporting Bug or Help needed on TestEndDateChecker";
            oMailItem.Display(true);
        }

        private void CurrentSiteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(CurrentSiteComboBox.SelectedItem.ToString() == "D1C")
            {
                OlaRecipePathTextBox.Text = @"\\ra2pznas01.rf2prod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                StartingDirOfRecipesTextBox.Text = @"\\rf2pzwfs002.rf2prod.mfg.intel.com\STTD\Sort\Recipe\CMT\Production\";
                MarsDBTextBox.Text = "D1D_PROD_MARS";
                XuesDatabasetextBox.Text = "D1D_PROD_XEUS";
                //http://d1-sortweb.rf3prod.mfg.intel.com/Preon/LotSummary/LotSummary/MENU_Lot_Sort%20Dispo 
                PWURLTextBox.Text = "http://d1-sortweb.rf3prod.mfg.intel.com/Preon/LotSummary/LotSummary/MENU_Lot_Sort%20Dispo";
                PWDispoSummaryTabTextBox.Text = "http://d1-sortweb.rf3prod.mfg.intel.com/Preon/DispoSummary/DispoSummary/MENU_Lot_Sort%20Dispo";
                //PWURLTextBox.Text = "http://rf3pvwb515n2.rf3prod.mfg.intel.com/PW/sortDispo.asp";
                //PWDispoSummaryTabTextBox.Text = "http://rf3pvwb515n2.rf3prod.mfg.intel.com/PW/sortDispo.asp?cs=Dispo Summary";
            }
            if (CurrentSiteComboBox.SelectedItem.ToString() == "S24")//F24
            {
                OlaRecipePathTextBox.Text = @"\\f24xcnas4.f24prod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                StartingDirOfRecipesTextBox.Text = @"\\f24p-nas-sortrms.f24prod.mfg.intel.com\sttd\Sort\Recipe\CMT\Production\";
                MarsDBTextBox.Text = "F24_PROD_MARS";
                XuesDatabasetextBox.Text = "F24_PROD_XEUS";
                PWURLTextBox.Text = "http://f24-sortweb.f24prod.mfg.intel.com/Preon/LotSummary/LotSummary/MENU_Lot_Sort%20Dispo";
                PWDispoSummaryTabTextBox.Text = "http://f24-sortweb.f24prod.mfg.intel.com/Preon/DispoSummary/DispoSummary/MENU_Lot_Sort%20Dispo";
                //PWURLTextBox.Text = "http://f24-sort.f24prod.mfg.intel.com/pw/sortDispo.asp";
                //PWDispoSummaryTabTextBox.Text = "http://f24-sort.f24prod.mfg.intel.com/PW/sortDispo.asp?cs=Dispo Summary";
            }
            if (CurrentSiteComboBox.SelectedItem.ToString() == "AFO")//AFO
            {
                OlaRecipePathTextBox.Text = @"\\afsp2xnfs1.afoprod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                StartingDirOfRecipesTextBox.Text = @"\\afsp8xnfs2.afoprod.mfg.intel.com\sort_recipe\Sort\Recipe\CMT\Production\";
                MarsDBTextBox.Text = "D1D_PROD_MARS";
                XuesDatabasetextBox.Text = "D1D_PROD_XEUS";
                PWURLTextBox.Text = "http://d1-sortweb.rf3prod.mfg.intel.com/Preon/LotSummary/LotSummary/MENU_Lot_Sort%20Dispo";
                PWDispoSummaryTabTextBox.Text = "http://d1-sortweb.rf3prod.mfg.intel.com/Preon/DispoSummary/DispoSummary/MENU_Lot_Sort%20Dispo";
                //PWURLTextBox.Text = "http://rf2-sort.rf2prod.mfg.intel.com/pw/sortDispo.asp";
                //PWDispoSummaryTabTextBox.Text = "http://rf2-sort.rf2prod.mfg.intel.com/pw/sortDispo.asp?cs=Dispo Summary";
            }
            if (CurrentSiteComboBox.SelectedItem.ToString() == "S11X")//F11X
            {
                OlaRecipePathTextBox.Text = @"\\s21netap2.f21prod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                StartingDirOfRecipesTextBox.Text = @"\\f21pucnasn2\sortrms\Sort\Recipe\CMT\Production\";
                MarsDBTextBox.Text = "F21_PROD_MARS";
                XuesDatabasetextBox.Text = "F21_PROD_XEUS";
                PWURLTextBox.Text = "http://f21-sort.f21prod.mfg.intel.com/pw/sortDispo.asp";
                PWDispoSummaryTabTextBox.Text = "http://f21-sort.f21prod.mfg.intel.com/PW/sortDispo.asp?cs=Dispo Summary";
            }
            if (CurrentSiteComboBox.SelectedItem.ToString() == "S28")//S28
            {
                OlaRecipePathTextBox.Text = @"\\f28pnas01n1.f28prod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                StartingDirOfRecipesTextBox.Text = @"\\f28pnas01n1.f28prod.mfg.intel.com\SortRMS\Sort\Recipe\CMT\Production\";
                MarsDBTextBox.Text = "F28_PROD_MARS";
                XuesDatabasetextBox.Text = "F28_PROD_XEUS";
                PWURLTextBox.Text = "http://f28-sortweb.f28prod.mfg.intel.com/LotSummary/LotSummary/MENU_Lot_Sort%20Dispo";
                PWDispoSummaryTabTextBox.Text = "http://f28-sortweb.f28prod.mfg.intel.com/Preon/DispoSummary/DispoSummary/MENU_Lot_Sort%20Dispo";
                //PWURLTextBox.Text = "http://f28-sort.f28prod.mfg.intel.com/pw/sortDispo.asp";
                //PWDispoSummaryTabTextBox.Text = "http://f28-sort.f28prod.mfg.intel.com/PW/sortDispo.asp?cs=Dispo Summary";
            }
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            ParsedUserCurrentSitetextBox.Text = Properties.Settings.Default["UserCurrentSite"].ToString();
            if (string.IsNullOrEmpty(ParsedUserCurrentSitetextBox.Text))
            {
                MessageBox.Show("Please manually setup your site info, was unable to find your current site from your mapped drives");
            }
            OlaRecipePathTextBox.Text = Properties.Settings.Default["OlaRecipePath"].ToString();
            StartingDirOfRecipesTextBox.Text = Properties.Settings.Default["StartingDirOfRecipes"].ToString();
            MarsDBTextBox.Text = Properties.Settings.Default["MarsDB"].ToString();
            XuesDatabasetextBox.Text = Properties.Settings.Default["DatabasetextBox"].ToString();
            PWURLTextBox.Text = Properties.Settings.Default["PWURL"].ToString();
            PWDispoSummaryTabTextBox.Text = Properties.Settings.Default["PWDispoSummaryTab"].ToString();
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            if (CurrentSiteComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select your site in the dropdown box first.");
                return;
            }
            Properties.Settings.Default["UserCurrentSite"] = CurrentSiteComboBox.SelectedItem.ToString();
            Properties.Settings.Default["OlaRecipePath"] = OlaRecipePathTextBox.Text;
            Properties.Settings.Default["StartingDirOfRecipes"] = StartingDirOfRecipesTextBox.Text;
            Properties.Settings.Default["MarsDB"] = MarsDBTextBox.Text;
            Properties.Settings.Default["DatabasetextBox"] = XuesDatabasetextBox.Text;
            Properties.Settings.Default["PWURL"] = PWURLTextBox.Text;
            Properties.Settings.Default["PWDispoSummaryTab"] = PWDispoSummaryTabTextBox.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Restarting Application for changes to take effect.");
            System.Windows.Forms.Application.Restart();
        }
    }
}
