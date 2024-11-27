using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEndDateChecker
{
    public partial class AutoProcForm_7634 : Form
    {
        List<string> LotsToAutoproc = new List<string>();
        List<string> LotsTotrack = new List<string>();
        string previouswebpage = "";
        string errorlog;
        string LotPassedDispoChecks;
        string LotToTriggerAutoProcOn;
        bool DidThePageLoadWhatIWanted = false;
        //bool AutoProcWasAlreadyTriggered = false;
        public AutoProcForm_7634()
        {
            InitializeComponent();
        }

        public string MesDatabase { get; set; }
        public string DispoSummaryUrl { get; set; }
        public List<string> GreenLotListFromTEDC { get; set; }
        private void ConfirmAutoProcButton_Click(object sender, EventArgs e)
        {
            AutoProcProgressBar.Visible = true;
            ControlsPanel.Enabled = false;
            Thread inputthelotids = new Thread(new ThreadStart(inputlotids));
            inputthelotids.IsBackground = true;
            inputthelotids.Start();
        }

        private void Exitbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AutoDispodataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AutoProcForm_7634_Load(object sender, EventArgs e)
        {
            try
            {
                AutoProcWebBrowser.Navigate(DispoSummaryUrl);
                UBER GetOperation = new UBER();
                AutoDispodataGridView.DataSource = GetOperation.GetOperationFromLots(GreenLotListFromTEDC, MesDatabase);

                DataGridViewColumn dtCol = new DataGridViewColumn();
                dtCol.Name = "Status";
                dtCol.DataPropertyName = "Status";
                dtCol.CellTemplate = new DataGridViewTextBoxCell();

                AutoDispodataGridView.Columns.Insert(2, dtCol);
                AutoDispodataGridView.Columns[0].Width = 60;
                AutoDispodataGridView.Columns[1].Width = 70;
                AutoDispodataGridView.Columns[2].Width = 200;
                AutoDispodataGridView.ClearSelection();
                AutoDispodataGridView.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                AutoDispodataGridView.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                AutoDispodataGridView.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
                foreach (DataGridViewRow datarow in AutoDispodataGridView.Rows)
                {
                    bool lotgoodtogo = true;

                    if (datarow.Cells[1].Value.ToString().Trim() == "7635") //operation check
                    {
                    }
                    else // not at right operation
                    {
                        datarow.Cells[2].Value = "Not at 7635.";
                        lotgoodtogo = false;
                    }

                    if (datarow.Cells[3].Value.ToString().Trim() == "N") //Hold Check
                    {
                    }
                    else //on hold
                    {
                        datarow.Cells[2].Value = datarow.Cells[2].Value + " Lot is on Hold.";
                        //lotgoodtogo = false;
                    }

                    try
                    {
                        if (datarow.Cells[4].Value.ToString().Trim() == "Y") //instruction flag check
                        {
                            datarow.Cells[2].Value = datarow.Cells[2].Value + " Lot has an INS flag.";
                            //lotgoodtogo = false;
                        }
                    }
                    catch { }
                    if (lotgoodtogo)
                    {
                        datarow.DefaultCellStyle.BackColor = Color.LightGreen;
                        LotsToAutoproc.Add(datarow.Cells[0].Value.ToString().Trim());
                        LotsTotrack.Add(datarow.Cells[0].Value.ToString().Trim());
                    }
                    else
                        datarow.DefaultCellStyle.BackColor = Color.LightPink;

                }
                Task W1 = Task.Factory.StartNew(() => Wait2seconds());
                W1.Wait();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the autoproc form, contact developer" + Environment.NewLine + ex.ToString());
            }
        }

        private void inputlotids()
        {
            foreach (string lotID in LotsToAutoproc)
            {
                UpdateStatusOnLot(lotID, "Starting", Color.LightYellow);
            }
            foreach (string lotID in LotsToAutoproc)
            {
                if (!(string.IsNullOrEmpty(lotID)))
                {
                    Task t1 = Task.Factory.StartNew(() => SendLotIDandCheckIfPageLoaded(lotID, "Lot-Level Disposition for " + lotID)); // set the lot id in the search box and click the set button
                    t1.Wait(); //wait to see if the lot info loaded
                }
                if (LotPassedDispoChecks == lotID) //passed the dispo check, go pass the lot level hold on it.
                {
                    Task t2 = Task.Factory.StartNew(() => SaveLotDispo(lotID, "RESULT=AUTO_HOLD")); // Make sure the lot level was saved and there are no more autoholds
                    t2.Wait(); //wait to see if the lot info loaded
                    UpdateStatusOnLot(lotID, "Passed Wafer level Dispo Checks", Color.LightGreen);
                }
                if (LotToTriggerAutoProcOn == lotID) //Go trigger autoproc
                {
                    Task t3 = Task.Factory.StartNew(() => GoTriggerAutoproc(lotID)); // Nav to auto proc and trigger it
                    t3.Wait();
                    //AutoProcWasAlreadyTriggered = false;
                }
                Task t4 = Task.Factory.StartNew(() => ResetToDispoScreen()); // set the lot id in the search box and click the set button
                t4.Wait();

            }
            LotsToAutoproc.Clear();

            CheckLotStatusForm(LotsTotrack);

            progressbarcomplete();
            //Order: 1.CheckifPageLoaded 2.SendLotIdAndClickSet 3.Checkifthisloadedonthewebpage 4. if it loaded the wait will end and go to next function.
        }
        private void Wait2seconds()
        {
            Thread.Sleep(2000);
        }
        private void SendLotIDandCheckIfPageLoaded(string lotid, string checkforthis)
        {
            var starttime = DateTime.UtcNow;
            SendLotIdAndClickSet(lotid);
            starttime = DateTime.UtcNow;
            while (DateTime.UtcNow - starttime < TimeSpan.FromSeconds(10))
            {
                Thread.Sleep(300);
                Checkifthisloadedonthewebpage(checkforthis);
                if (DidThePageLoadWhatIWanted)
                {
                    SeeWhatLoadedAndUpdateStatus();
                    break;
                }
            }
            if (DidThePageLoadWhatIWanted == false)
            {
                SendLotIdAndClickSet(lotid);
                starttime = DateTime.UtcNow;
                while (DateTime.UtcNow - starttime < TimeSpan.FromSeconds(10)) // wait again, maybe its super slow?
                {
                    Thread.Sleep(100);
                    Checkifthisloadedonthewebpage(checkforthis);
                    if (DidThePageLoadWhatIWanted)
                    {
                        SeeWhatLoadedAndUpdateStatus();
                        break;
                    }
                }
                if (DidThePageLoadWhatIWanted == false)
                    errorlog = errorlog + Environment.NewLine + "◙" + lotid + "didnt get selected on PW, 2nd try";
            }
            else
                DidThePageLoadWhatIWanted = false;
        }
        private void SendLotIdAndClickSet(string lotid)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { SendLotIdAndClickSet(lotid); });
                return;
            }
            try
            {
                HtmlElementCollection elc = AutoProcWebBrowser.Document.GetElementsByTagName("input");
                foreach (HtmlElement el in elc)
                {
                    if (el.Name.Equals("LotNumber"))
                    {
                        el.SetAttribute("value", lotid);
                        break;
                    }
                }
                foreach (HtmlElement el in elc)
                {
                    if (el.OuterHtml.Equals("<INPUT class=button type=submit value=SET>"))
                    {
                        el.InvokeMember("click");
                        break;
                    }
                }
                //Thread.Sleep(50);
                //errorlog = errorlog + Environment.NewLine +"ITEM SENT=" + item;
            }
            catch
            {
                MessageBox.Show("Unable to set your lot id and click the set button.");
            }
        }
        private void Checkifthisloadedonthewebpage(string item)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { Checkifthisloadedonthewebpage(item); });
                return;
            }
            try
            {
                string whatloaded = AutoProcWebBrowser.Document.Body.InnerText;
                string lotid = item.Substring(item.Length - 8, 8);
                if (whatloaded.Contains("Lot does not exist. Please enter another lot number on top left text box and click SET."))
                {
                    if (whatloaded.Contains("Lot: " + lotid))
                    {
                        previouswebpage = whatloaded;
                        DidThePageLoadWhatIWanted = true;
                        return;
                    }
                }
                if (whatloaded.Contains("No data found for lot " + lotid))
                {
                    if (whatloaded.Contains("Lot: " + lotid))
                    {
                        previouswebpage = whatloaded;
                        DidThePageLoadWhatIWanted = true;
                        return;
                    }
                }

                if ((whatloaded.Contains(item)) && (whatloaded != previouswebpage) && (whatloaded.Contains("This file last modified on:")))
                {
                    previouswebpage = whatloaded;
                    DidThePageLoadWhatIWanted = true;
                }
                else
                    DidThePageLoadWhatIWanted = false;
            }
            catch
            {
            }
        }

        private void SeeWhatLoadedAndUpdateStatus()
        {
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(SeeWhatLoadedAndUpdateStatus);
                Invoke(method);
                return;
            }
            string webinfo = AutoProcWebBrowser.Document.Body.InnerHtml;
            string lot_level_webinfo = webinfo.Substring(webinfo.IndexOf("<DIV id=LotLevelDispositionData>"), webinfo.IndexOf("<DIV id=WaferLevelDispositionData>") - webinfo.IndexOf("<DIV id=LotLevelDispositionData>"));
            string Wafer_level_webinfo = webinfo.Substring(webinfo.IndexOf("<DIV id=WaferLevelDispositionData>"));
            string lotid = webinfo.Substring(webinfo.IndexOf("</FORM>Lot: ") + 12, 8);
            if (webinfo.Contains("Lot does not exist. Please enter another lot number on top left text box and click SET."))
            {
                UpdateStatusOnLot(lotid, "Lot doesn't Exsist", Color.LightPink);
                return;
            }
            else if (webinfo.Contains("No data found for lot " + lotid))
            {
                UpdateStatusOnLot(lotid, "No data found", Color.LightPink);
                return;
            }
            if (Wafer_level_webinfo.Contains("No data returned.")) //good to go, no dispo is needed
            {
                UpdateStatusOnLot(lotid, "wafer dispo clean", Color.LightGreen);
                //manual pass the lot level hold and save the lot level dispo 
                if (lot_level_webinfo.Contains("|REASON=HOLD for DISPOSITION per XML Tag|RESULT=AUTO_HOLD|USERNAME=|OPERATION\">7634</DIV>")) //7634 lot level hold is there, clear it
                {
                    UpdateStatusOnLot(lotid, "7634 lot level hold found", Color.LightGreen);
                    LotPassedDispoChecks = lotid;
                }
                else if (!(lot_level_webinfo.Contains("<OPTION selected value=AUTO_HOLD>AUTO_HOLD</OPTION>")))
                {
                    UpdateStatusOnLot(lotid, "No AUTO_HOLD 7634 lot level hold found", Color.LightGreen);
                    LotPassedDispoChecks = lotid;
                }
                else
                {
                    if (lot_level_webinfo.Contains("<OPTION selected value=AUTO_HOLD>AUTO_HOLD</OPTION>"))
                        UpdateStatusOnLot(lotid, "Other Lot hold found", Color.LightPink);
                    return;
                }

            }
            else //There is some wafer level things that need taken care of, log it
            {
                //LotPassedDispoChecks = lotid; this ignores the dispo checks
                string[] webinfoarray = Wafer_level_webinfo.Split(new[] { "id=" }, StringSplitOptions.None);
                string dispoinfo = "";
                foreach (string line in webinfoarray)
                {
                    string temp;
                    if (line.Contains("USERNAME="))
                    {
                        temp = line.Substring(2, line.IndexOf("USERNAME=") - 2);
                        if (!(dispoinfo.Contains(temp)))
                        {
                            dispoinfo = dispoinfo + temp + Environment.NewLine;
                        }
                    }
                }
                UpdateStatusOnLot(lotid, "Needs Wafer Level dispo " + dispoinfo, Color.LightPink);
            }

        }
        private void SaveLotDispo(string lot, string MakeSureThisIsntThere)
        {
            var starttime = DateTime.UtcNow;
            UpdateStatusOnLot(lot, "Going to pass the lot hold now...", Color.Yellow);
            PassAndSaveLotLevelDispo();
            starttime = DateTime.UtcNow;
            while (DateTime.UtcNow - starttime < TimeSpan.FromSeconds(10))
            {
                Thread.Sleep(500);
                UpdateStatusOnLot(lot, "Checking it was passed...", Color.Yellow);
                CheckIfLotLevelDispoWasSaved(lot, MakeSureThisIsntThere);
                if (DidThePageLoadWhatIWanted)
                {
                    UpdateStatusOnLot(lot, "I assume it was passed...", Color.LightGreen);
                    LotToTriggerAutoProcOn = lot;
                    break;
                }
            }
            if (DidThePageLoadWhatIWanted == false)
            {
                UpdateStatusOnLot(lot, "lot level hold wasnt passed, double check it", Color.LightPink);
                errorlog = errorlog + Environment.NewLine + "◙" + lot + "didnt get lot level hold passed in PW, 2nd try";
            }
            else
                DidThePageLoadWhatIWanted = false;
        }
        private void CheckIfLotLevelDispoWasSaved(string lot, string MakeSureThisIsntThere)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { CheckIfLotLevelDispoWasSaved(lot, MakeSureThisIsntThere); });
                return;
            }
            try
            {
                string whatloaded = AutoProcWebBrowser.Document.Body.InnerText;
                if ((!(whatloaded.Contains(MakeSureThisIsntThere))) && (whatloaded != previouswebpage))
                {
                    if (!(whatloaded.Contains("Saving Lot-Level Disposition data. Please wait until this message goes away...")))
                    {
                        previouswebpage = whatloaded;
                        DidThePageLoadWhatIWanted = true;
                    }
                }
                else
                    DidThePageLoadWhatIWanted = false;
            }
            catch
            {
            }
        }
        private void PassAndSaveLotLevelDispo()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { PassAndSaveLotLevelDispo(); });
                return;
            }
            try
            {
                HtmlElementCollection elc = AutoProcWebBrowser.Document.GetElementsByTagName("select");
                foreach (HtmlElement el in elc)
                {
                    if (el.Name.Contains("Lot-Level"))
                    {
                        if (el.Id.Contains("OPERATION=7634"))
                        {
                            el.SetAttribute("value", "MANUAL_PASS");
                            break;
                        }
                        else //no manual pass there to select, user might not have privs.
                        {
                        }
                    }
                }
                HtmlElement el1 = AutoProcWebBrowser.Document.GetElementById("Save Lot-Level Disposition");
                el1.InvokeMember("click");
                //Thread.Sleep(50);
                //errorlog = errorlog + Environment.NewLine +"ITEM SENT=" + item;
            }
            catch
            {
                MessageBox.Show("Unable to select manual_pass and save");
            }
        }
        private void GoTriggerAutoproc(string lotid)
        {
            var starttime = DateTime.UtcNow;
            NavigateToAutoprocScreen();
            starttime = DateTime.UtcNow;
            while (DateTime.UtcNow - starttime < TimeSpan.FromSeconds(10))
            {
                MakeSureYouAreAtTheAutoProcScreen(lotid);
                Thread.Sleep(300);
                if (DidThePageLoadWhatIWanted)
                {
                    break;
                }
            }
            ManualTriggerAutoProc(lotid);
            starttime = DateTime.UtcNow;
            while (DateTime.UtcNow - starttime < TimeSpan.FromSeconds(10))
            {
                WaitForSave(lotid);
                Thread.Sleep(300);
                if (DidThePageLoadWhatIWanted)
                {
                    break;
                }
            }
            DidThePageLoadWhatIWanted = false;
        }
        private void NavigateToAutoprocScreen()
        {
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(NavigateToAutoprocScreen);
                Invoke(method);
                return;
            }
            AutoProcWebBrowser.Navigate(DispoSummaryUrl.Replace("Dispo Summary", "Auto Proc"));
        }
        private void NavigateToDispoScreen()
        {
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(NavigateToDispoScreen);
                Invoke(method);
                return;
            }
            AutoProcWebBrowser.Navigate(DispoSummaryUrl);
        }
        private void MakeSureYouAreAtTheAutoProcScreen(string lot)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { MakeSureYouAreAtTheAutoProcScreen(lot); });
                return;
            }
            try
            {
                string whatloaded = AutoProcWebBrowser.Document.Body.InnerText;
                if (whatloaded.Contains("Last time Manual Trigger was performed for " + lot))
                {
                    previouswebpage = whatloaded;
                    //AutoProcWasAlreadyTriggered = true;
                    DidThePageLoadWhatIWanted = true;
                    UpdateStatusOnLot(lot, "Autoproc was already triggered", Color.LightPink);
                }
                else if (whatloaded.Contains("button to trigger auto proc manually for lot " + lot))
                {
                    previouswebpage = whatloaded;
                    DidThePageLoadWhatIWanted = true;
                }
                else
                    DidThePageLoadWhatIWanted = false;
            }
            catch
            {
            }
        }
        private void WaitForSave(string lot)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { WaitForSave(lot); });
                return;
            }
            try
            {
                string whatloaded = AutoProcWebBrowser.Document.Body.InnerText;
                if (whatloaded.Contains("Success"))
                {
                    UpdateStatusOnLot(lot, "AutoProc was triggered", Color.LightGreen);
                    DidThePageLoadWhatIWanted = true;
                }
                else if (whatloaded.Contains("Last time Manual Trigger was performed for "))
                {
                    UpdateStatusOnLot(lot, "Autoproc was already triggered", Color.LightPink);
                    DidThePageLoadWhatIWanted = true;
                }
                else
                    DidThePageLoadWhatIWanted = false;
            }
            catch
            {
            }
        }
        private void ManualTriggerAutoProc(string Lotid)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { ManualTriggerAutoProc(Lotid); });
                return;
            }
            try
            {
                HtmlElement el1 = AutoProcWebBrowser.Document.GetElementById("submit6");
                el1.InvokeMember("click");
                //Thread.Sleep(50);
                //errorlog = errorlog + Environment.NewLine +"ITEM SENT=" + item;
            }
            catch
            {
                //MessageBox.Show("Unable to click the autoproc button");
                UpdateStatusOnLot(Lotid, "AutoProc was previously triggered", Color.LightPink);
            }
        }
        private void ResetToDispoScreen()
        {
            var starttime = DateTime.UtcNow;
            NavigateToDispoScreen();
            starttime = DateTime.UtcNow;
            while (DateTime.UtcNow - starttime < TimeSpan.FromSeconds(10))
            {
                Thread.Sleep(300);
                MakeSureYouAreAtTheDispoSummaryTab();
                if (DidThePageLoadWhatIWanted)
                {
                    break;
                }
            }
            if (DidThePageLoadWhatIWanted == false)
            {
                errorlog = errorlog + Environment.NewLine + "◙" + "did not navigate correctly to dispo screen";
            }
            else
                DidThePageLoadWhatIWanted = false;
        }
        private void MakeSureYouAreAtTheDispoSummaryTab()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { MakeSureYouAreAtTheDispoSummaryTab(); });
                return;
            }
            try
            {
                string whatloaded = AutoProcWebBrowser.Document.Body.InnerText;
                if (whatloaded.Contains("Lot-Level Disposition for "))
                {
                    previouswebpage = whatloaded;
                    DidThePageLoadWhatIWanted = true;
                }
                else
                    DidThePageLoadWhatIWanted = false;
            }
            catch
            {
            }
        }
        private void UpdateStatusOnLot(string Lot, string Status, Color color)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { UpdateStatusOnLot(Lot, Status, color); });
                return;
            }
            foreach (DataGridViewRow datarow in AutoDispodataGridView.Rows)
            {
                if (datarow.Cells[0].Value.ToString().Trim() == Lot)
                {
                    datarow.DefaultCellStyle.BackColor = color;
                    datarow.Cells[2].Value = Status;
                    if (Status.Contains("Needs Wafer Level dispo"))
                    {
                        //datarow.Cells[2].Value = Status.Substring(0,24);
                        datarow.Cells[2].ToolTipText = Status.Substring(24);
                    }
                }
            }
        }
        private void progressbarcomplete()
        {
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(progressbarcomplete);
                Invoke(method);
                return;
            }
            AutoProcProgressBar.Visible = false;
            ControlsPanel.Enabled = true;
            MessageBox.Show("Autoproc process complete!");
        }

        private void AutoDispodataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }
        private void CheckLotStatusForm(List<string> Lotlist)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { CheckLotStatusForm(Lotlist); });
                return;
            }
            List<string> lotsthatweretriggered = new List<string>();
            lotsthatweretriggered.AddRange(Lotlist);
            LotStatusForm lotstat = new LotStatusForm();
            lotstat.MesDatabase = MesDatabase;
            lotstat.LotsThatWereAutoproced = lotsthatweretriggered;
            lotstat.OperationToCheck = "7635";
            lotstat.Show();
        }
    }
}
