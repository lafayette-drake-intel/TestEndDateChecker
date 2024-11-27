using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
//using myJMP = JMP;
using System.Diagnostics;
using System.Management;
using System.Globalization;

namespace TestEndDateChecker
{
    // The purpose of this program is to compare the test end dates in CB vs SOD and make sure they match
    // #1 make a GUI
    // #2 make some logic to let us input a lot list
    // #3 Run an SQL query and get our test end dates for all wafers in each lot in our lotlist textbox by TP name, operation
    // #4 list the TEDs in a column in our datagrid view
    // #5 Get SOD TEDS and list them right next to the corisponding TED for CB
    // #6 Compare the TEDs and color them green if they match and red if they dont
    // #7 Trigger a messagebox for the runs that dont match
    public partial class Form1 : Form
    {
        string UserCurrentSite = Properties.Settings.Default["UserCurrentSIte"].ToString();
        List<KeyValuePair<string, string>> Users_mapped_drives = new List<KeyValuePair<string, string>>();  // to  check the map drive
        string UserIdrive = "";
        bool DidThePageLoadWhatIWanted = false;
        bool lotId7Digit = false;
        bool showCbResortData = false;
        string errorlog;
        string previouswebpage = "";
        string NoSodDataFound = "";
        string operationselected = "'6051'";
        string OlaRecipePath = "";
        string StartingDirOfRecipes = "";
        string PWURL = "";
        string PWDispoSummaryTab = "";
        string MarsDB = "";
        List<string> productsList = new List<string>();
        List<string> GreenLotList = new List<string>();
        List<string> RedLotList = new List<string>();
        string tempLotId;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(dgv1_Scroll); //trigger event for scrollbar sync
            dataGridView2.Scroll += new System.Windows.Forms.ScrollEventHandler(dgv2_Scroll); //trigger event for scrollbar sync
        }

        private void dgv1_Scroll(object sender, EventArgs e) //sync the scrollbars
        {
            try
            {
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
            }
            catch { }
        }
        private void dgv2_Scroll(object sender, EventArgs e) //sync the scrollbars
        {
            try
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView2.FirstDisplayedScrollingRowIndex;
            }
            catch { }
        }
        private void cleanthelotlist()
        {
            int initialcount = LOTlistBox.Items.Count;
            //remove any trash thats not a lot id
            string clipboardinfo = "";
            try
            {
                clipboardinfo = Clipboard.GetText();
            }
            catch
            {
                MessageBox.Show("Unable to get clipboard text, try again");
            }
            string[] lotIDs = clipboardinfo.Split(new char[] { ' ', '\t', '\n', '\r', '>', '<' });
            foreach (string line in lotIDs)
            {
                Match match = Regex.Match(line, @"[a-zA-Z]\d{6}\.*");
                if (match.Success)
                {
                    string lotid = line.Trim();
                    if (lotid.Length == 8) // make sure lot id is only 8 char
                    {
                        if(!(LOTlistBox.Items.Contains(lotid.ToUpper())))
                        LOTlistBox.Items.Add(lotid.ToUpper());
                    }
                }
            }
            if(LOTlistBox.Items.Count == initialcount) //nothing added, go get the englots
            {
                foreach (string line in lotIDs)
                {
                    Match match = Regex.Match(line, @"[a-zA-Z]\d{3}\.*");
                    if (match.Success)
                    {
                        string lotid = line.Trim();
                        if (lotid.Length == 8) // make sure lot id is only 8 char
                        {
                            if (!(LOTlistBox.Items.Contains(lotid.ToUpper())))
                                LOTlistBox.Items.Add(lotid.ToUpper());
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cleanthelotlist();
            //go get our test end dates
        }

        private void GetTEDbutton_Click(object sender, EventArgs e)
        {
            if (LOTlistBox.Items.Count > 0)
            {
                DisableControls();
                RedLotList.Clear();
                GreenLotList.Clear();
                getCBTEDs();
            }
            else
            {
                MessageBox.Show("You dont have any lots in your list");
            }
        }
        private void getCBTEDs ()
        {
            CBprogressBar.Visible = true;
            dataGridView1.DataSource = null;
            Thread gettheCBTED = new Thread(new ThreadStart(gogettheCBTED));
            gettheCBTED.IsBackground = true;
            gettheCBTED.Start();
        }
        private string FindTheDatabaseForYourLot (string LotIdToLookUp)
        {
            string database = "";
            LotIdToLookUp = LotIdToLookUp.Substring(0, 1);
            if ((LotIdToLookUp == "D") || (LotIdToLookUp == "Z"))
                database = "D1D_PROD_XEUS";
            if (LotIdToLookUp == "H")
                database = "F24_PROD_XEUS";
            if (LotIdToLookUp == "Q")
                database = "F24_PROD_XEUS";
            if (LotIdToLookUp == "N")
                database = "F28_PROD_XEUS";
            if (LotIdToLookUp == "L")
                database = "F32_PROD_XEUS";
            if (LotIdToLookUp == "W")
                database = "F21_PROD_XEUS";
            if (LotIdToLookUp == "C")
                database = "F24_PROD_XEUS";
            if (string.IsNullOrEmpty(database))
            {
                MessageBox.Show("Couldnt parse the database from your Lot number, defaulting to your MES database.");
                database = DatabasetextBox.Text.Trim();
            }
            return database;
        }
        private void gogettheCBTED()
        {
            string[] operations = operationselected.Split(' ');
            DataTable DtM = new DataTable();
            foreach (string lot in LOTlistBox.Items)
            {
                UBER getSQL = new UBER();
                DataTable DT = new DataTable();
                //DT = getSQL.ReturnLastTEDFromLot(firstlot, DatabasetextBox.Text);
                string database = FindTheDatabaseForYourLot(lot);
                string lotIDtogetDataFor = lot;
                if (lotId7Digit)
                    lotIDtogetDataFor = lotIDtogetDataFor.Substring(0, 7) + "%";
                DT = getSQL.ReturnLastTEDFromLot(lotIDtogetDataFor, database, operations, showCbResortData);
                string[] operationreplacement = operationselected.Replace("'", "").Split(' ');
                string operationsjoined = string.Join("", operationreplacement);
                foreach (DataRow DTrow in DT.Rows)
                {
                    string OPfound = DTrow[1].ToString();
                    foreach (string op in operationreplacement)
                    {
                        if (OPfound.Contains(op))
                        {
                            operationsjoined = operationsjoined.Replace(op, ""); // didnt find it
                        }
                    }
                }
                if (!(string.IsNullOrEmpty(operationsjoined)))
                {
                    string[] operationsjoinedsplit = Splitintofours(operationsjoined, 4).ToArray();
                    foreach (string op in operationsjoinedsplit)
                    {
                        try
                        {
                            DataRow DTrowadder = DT.NewRow();
                            DTrowadder[0] = lot;
                            DTrowadder[1] = op;
                            DTrowadder[2] = "NONE";
                            DTrowadder[3] = "NONE";
                            DTrowadder[4] = System.DateTime.Now;
                            DT.Rows.Add(DTrowadder);
                        }
                        catch
                        { }
                    }
                }
                try
                {
                    string SQLinfofromUber = getSQL.returnProductIDfromLOT(lot, DatabasetextBox.Text);
                    productsList.Add(SQLinfofromUber.Split(',')[0]);
                    string WaferQtyfromSQL = SQLinfofromUber.Split(',')[0];
                    if (!(SQLinfofromUber.Contains("Error")))
                        WaferQtyfromSQL = SQLinfofromUber.Split(',')[4];
                    int wafercountfromCBTED = 0;
                    foreach (DataRow DTrow in DT.Rows)
                    {
                        if (DTrow[1].ToString() == "6051")
                            if (DTrow[0].ToString() == lot)
                                if (!(DTrow[3].ToString() == "NONE"))
                                    wafercountfromCBTED++;
                    }
                    DataRow addtheqty = DT.NewRow();
                    addtheqty[1] = "6051";
                    addtheqty[2] = "Qty:" + wafercountfromCBTED + "/" + WaferQtyfromSQL;
                    DT.Rows.Add(addtheqty);
                    string operationsjoinedforqty = string.Join("", operationreplacement);

                    if (operationsjoinedforqty.Contains("7087"))
                    {
                        int wafercountfromCBTED7087 = 0;
                        foreach (DataRow DTrow in DT.Rows)
                        {
                            if (DTrow[1].ToString() == "7087")
                                if (DTrow[0].ToString() == lot)
                                    if (!(DTrow[3].ToString() == "NONE"))
                                        wafercountfromCBTED7087++;
                        }
                        DataRow addtheqty7087 = DT.NewRow();
                        addtheqty7087[1] = "7087";
                        addtheqty7087[2] = "Qty:" + wafercountfromCBTED7087 + "/" + WaferQtyfromSQL;
                        DT.Rows.Add(addtheqty7087);
                    }
                    if (operationsjoinedforqty.Contains("6751"))
                    {
                        int wafercountfromCBTED7087 = 0;
                        foreach (DataRow DTrow in DT.Rows)
                        {
                            if (DTrow[1].ToString() == "6751")
                                if (DTrow[0].ToString() == lot)
                                    if (!(DTrow[3].ToString() == "NONE"))
                                        wafercountfromCBTED7087++;
                        }
                        DataRow addtheqty7087 = DT.NewRow();
                        addtheqty7087[1] = "6751";
                        addtheqty7087[2] = "Qty:" + wafercountfromCBTED7087 + "/" + WaferQtyfromSQL;
                        DT.Rows.Add(addtheqty7087);
                    }
                    if (operationsjoinedforqty.Contains("7286"))
                    {
                        int wafercountfromCBTED7087 = 0;
                        foreach (DataRow DTrow in DT.Rows)
                        {
                            if (DTrow[1].ToString() == "7286")
                                if (DTrow[0].ToString() == lot)
                                    if (!(DTrow[3].ToString() == "NONE"))
                                        wafercountfromCBTED7087++;
                        }
                        DataRow addtheqty7087 = DT.NewRow();
                        addtheqty7087[1] = "7286";
                        addtheqty7087[2] = "Qty:" + wafercountfromCBTED7087 + "/" + WaferQtyfromSQL;
                        DT.Rows.Add(addtheqty7087);
                    }

                    if (operationsjoinedforqty.Contains("7634"))
                    {
                        int wafercountfromCBTED7087 = 0;
                        foreach (DataRow DTrow in DT.Rows)
                        {
                            if (DTrow[1].ToString() == "7634")
                                if (DTrow[0].ToString() == lot)
                                    if (!(DTrow[3].ToString() == "NONE"))
                                        wafercountfromCBTED7087++;
                        }
                        DataRow addtheqty7087 = DT.NewRow();
                        addtheqty7087[1] = "7634";
                        addtheqty7087[2] = "Qty:" + wafercountfromCBTED7087 + "/" + WaferQtyfromSQL;
                        DT.Rows.Add(addtheqty7087);
                    }
                    if (operationsjoinedforqty.Contains("7466"))
                    {
                        int wafercountfromCBTED7087 = 0;
                        foreach (DataRow DTrow in DT.Rows)
                        {
                            if (DTrow[1].ToString() == "7466")
                                if (DTrow[0].ToString() == lot)
                                    if (!(DTrow[3].ToString() == "NONE"))
                                        wafercountfromCBTED7087++;
                        }
                        DataRow addtheqty7087 = DT.NewRow();
                        addtheqty7087[1] = "7466";
                        addtheqty7087[2] = "Qty:" + wafercountfromCBTED7087 + "/" + WaferQtyfromSQL;
                        DT.Rows.Add(addtheqty7087);
                    }
                    if (operationsjoinedforqty.Contains("7180"))
                    {
                        int wafercountfromCBTED7087 = 0;
                        foreach (DataRow DTrow in DT.Rows)
                        {
                            if (DTrow[1].ToString() == "7180")
                                if (DTrow[0].ToString() == lot)
                                    if (!(DTrow[3].ToString() == "NONE"))
                                        wafercountfromCBTED7087++;
                        }
                        DataRow addtheqty7087 = DT.NewRow();
                        addtheqty7087[1] = "7180";
                        addtheqty7087[2] = "Qty:" + wafercountfromCBTED7087 + "/" + WaferQtyfromSQL;
                        DT.Rows.Add(addtheqty7087);
                    }
                    if (operationsjoinedforqty.Contains("8361"))
                    {
                        int wafercountfromCBTED7087 = 0;
                        foreach (DataRow DTrow in DT.Rows)
                        {
                            if (DTrow[1].ToString() == "8361")
                                if (DTrow[0].ToString() == lot)
                                    if (!(DTrow[3].ToString() == "NONE"))
                                        wafercountfromCBTED7087++;
                        }
                        DataRow addtheqty7087 = DT.NewRow();
                        addtheqty7087[1] = "8361";
                        addtheqty7087[2] = "Qty:" + wafercountfromCBTED7087 + "/" + WaferQtyfromSQL;
                        DT.Rows.Add(addtheqty7087);
                    }
                    if (operationsjoinedforqty.Contains("6617"))
                    {
                        int wafercountfromCBTED7087 = 0;
                        foreach (DataRow DTrow in DT.Rows)
                        {
                            if (DTrow[1].ToString() == "6617")
                                if (DTrow[0].ToString() == lot)
                                    if (!(DTrow[3].ToString() == "NONE"))
                                        wafercountfromCBTED7087++;
                        }
                        DataRow addtheqty7087 = DT.NewRow();
                        addtheqty7087[1] = "6617";
                        addtheqty7087[2] = "Qty:" + wafercountfromCBTED7087 + "/" + WaferQtyfromSQL;
                        DT.Rows.Add(addtheqty7087);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Could not parse your wafer quantity from xues" + e.ToString());
                    updatedatetheCBTEDdatagridview(DtM);
                }
                
                DT.Rows.Add();
                DtM.Merge(DT);
            }
            updatedatetheCBTEDdatagridview(DtM);
        }
        
        static IEnumerable<string> Splitintofours(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }
        private void updatedatetheCBTEDdatagridview (DataTable DTM)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { updatedatetheCBTEDdatagridview(DTM); });
                return;
            }
            dataGridView1.DataSource = DTM;
            //dataGridView1.Columns[4].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm:ss";
            
            foreach (DataGridViewColumn dc in dataGridView1.Columns)
            {
                if (dc.ValueType == typeof(System.DateTime))
                {
                    dc.DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("en-us");
                    dc.DefaultCellStyle.Format = "MM/dd/yyyy hh:mm:ss tt";
                }
            }
            if (operationselected.Contains("7180")) //remove the 6051 row
            {
                foreach (DataGridViewRow datarow in dataGridView1.Rows)
                {
                    try
                    {
                        if (datarow.Cells[1].Value.ToString().Equals("6051"))
                            dataGridView1.Rows.Remove(datarow);
                    }
                    catch
                    { }
                }
            }
            if (operationselected.Contains("6617")) //remove the 6051 row
            {
                foreach (DataGridViewRow datarow in dataGridView1.Rows)
                {
                    try
                    {
                        if (datarow.Cells[1].Value.ToString().Equals("6051"))
                            dataGridView1.Rows.Remove(datarow);
                    }
                    catch
                    { }
                }
            }

            CBprogressBar.Visible = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (LOTlistBox.Items.Count > 0)
            {
                DisableControls();
                RedLotList.Clear();
                GreenLotList.Clear();
                getSODteds();
            }
            else
            {
                MessageBox.Show("You dont have any lots in your list");
            }
            
        }
        private void getSODteds ()
        {
            SODprogressBar.Visible = true;
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            Thread inputthelotids = new Thread(new ThreadStart(inputlotids));
            inputthelotids.IsBackground = true;
            inputthelotids.Start();
        }
        private void inputlotids()
        {
            //string[] lotIDs = LotIdtextBox.Text.Split(new char[] { ' ', '\t', '\n', '\r', '>', '<' });
            //string lotID = lotIDs[0];
            foreach (string lotID in LOTlistBox.Items)
            {
                if (!(string.IsNullOrEmpty(lotID)))
                {
                    Task t1 = Task.Factory.StartNew(() => SendLotIDandCheckIfPageLoaded(lotID, "INTERFACE_BIN Bin Lot Information for " + lotID)); // set the lot id in the search box and click the set button
                    t1.Wait(); //wait to see if the lot info loaded
                    if (checkBox7087.Checked)
                    {
                        Task t2 = Task.Factory.StartNew(() => Pickoperationandgetinfo(7087.ToString(), "INTERFACE_BIN Bin Lot Information for " + lotID + " at Operation 7087")); // go look at the 7087 data //INTERFACE_BIN Bin Lot Information for D952932B at Operation 7087
                        t2.Wait(); //wait to see if the lot info loaded
                    }
                    if (checkBox6751.Checked)
                    {
                        Task t3 = Task.Factory.StartNew(() => Pickoperationandgetinfo(6751.ToString(), "INTERFACE_BIN Bin Lot Information for " + lotID + " at Operation 6751")); // go look at the 6751 data
                        t3.Wait(); //wait to see if the lot info loaded
                    }
                    if (checkBox7286.Checked)
                    {
                        Task t4 = Task.Factory.StartNew(() => Pickoperationandgetinfo(7286.ToString(), "INTERFACE_BIN Bin Lot Information for " + lotID + " at Operation 7268")); // go look at the 7286 data
                        t4.Wait(); //wait to see if the lot info loaded
                    }

                    if (checkBox7634.Checked)
                    {
                        Task t4 = Task.Factory.StartNew(() => Pickoperationandgetinfo(7634.ToString(), "INTERFACE_BIN Bin Lot Information for " + lotID + " at Operation 7634")); // go look at the 7634 data
                        t4.Wait(); //wait to see if the lot info loaded
                    }
                    if (checkBox7466.Checked)
                    {
                        Task t4 = Task.Factory.StartNew(() => Pickoperationandgetinfo(7466.ToString(), "INTERFACE_BIN Bin Lot Information for " + lotID + " at Operation 7466")); // go look at the 7466 data
                        t4.Wait(); //wait to see if the lot info loaded
                    }
                    if (checkBox6617.Checked)
                    {
                        Task t4 = Task.Factory.StartNew(() => Pickoperationandgetinfo(6617.ToString(), "INTERFACE_BIN Bin Lot Information for " + lotID + " at Operation 6617")); // go look at the 6617 data
                        t4.Wait(); //wait to see if the lot info loaded
                    }
                    if (checkBox7180.Checked)
                    {
                        try
                        {
                            string lastrow = dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[1].Value.ToString();
                            if (lastrow != "7180")
                            {
                                Task t4 = Task.Factory.StartNew(() => Pickoperationandgetinfo(7180.ToString(), "INTERFACE_BIN Bin Lot Information for " + lotID + " at Operation 7180")); // go look at the 6617 data
                                t4.Wait(); //wait to see if the lot info loaded
                            }
                            if (checkBox8361.Checked)
                            {
                                Task t4 = Task.Factory.StartNew(() => Pickoperationandgetinfo(8361.ToString(), "INTERFACE_BIN Bin Lot Information for " + lotID + " at Operation 8361")); // go look at the 6617 data
                                t4.Wait(); //wait to see if the lot info loaded
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                addarow(lotID);
            }
            if (LOTlistBox.Items.Count < 2)
            {
                SendLotIdAndClickSet("");
            }
            progressbarcomplete();
            //Order: 1.CheckifPageLoaded 2.SendLotIdAndClickSet 3.Checkifthisloadedonthewebpage 4. if it loaded the wait will end and go to next function.
        }
        private void addarow( string lot)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { addarow(lot); });
                return;
            }
            try
            {
                UBER getSQL = new UBER();
                string SQLinfofromUber = getSQL.returnProductIDfromLOT(lot, DatabasetextBox.Text);
                string WaferQtyfromSQL = SQLinfofromUber.Split(',')[0];
                if (!(SQLinfofromUber.Contains("Error")))
                    WaferQtyfromSQL = SQLinfofromUber.Split(',')[4];
                addSODQtyforOperation("6051", lot, WaferQtyfromSQL);
                if(checkBox7087.Checked)
                {
                    addSODQtyforOperation("7087", lot, WaferQtyfromSQL);
                }
                if (checkBox6751.Checked)
                {
                    addSODQtyforOperation("6751", lot, WaferQtyfromSQL);
                }
                if (checkBox7286.Checked)
                {
                    addSODQtyforOperation("7286", lot, WaferQtyfromSQL);
                }

                if (checkBox7634.Checked)
                {
                    addSODQtyforOperation("7634", lot, WaferQtyfromSQL);
                }
                if (checkBox7466.Checked)
                {
                    addSODQtyforOperation("7466", lot, WaferQtyfromSQL);
                }
                if (checkBox7180.Checked)
                {
                    addSODQtyforOperation("7180", lot, WaferQtyfromSQL);
                }
                if (checkBox8361.Checked)
                {
                    addSODQtyforOperation("8361", lot, WaferQtyfromSQL);
                }
                if (checkBox6617.Checked)
                {
                    addSODQtyforOperation("6617", lot, WaferQtyfromSQL);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not parse your wafer quantity from xues" + e.ToString());
            }
            dataGridView2.Rows.Add(1);
        }
        private void addSODQtyforOperation(string operation, string lot, string MESWaferQty)
        {
            int wafercountfromCBTED = 0;
            foreach (DataGridViewRow datarow in dataGridView2.Rows)
            {
                try
                {
                    if (datarow.Cells[1].Value.ToString().Equals(operation))
                        if (datarow.Cells[0].Value.ToString().Equals(lot))
                            if (!(datarow.Cells[3].Value.ToString().Equals("NONE")))
                                wafercountfromCBTED++;
                }
                catch
                { }
            }
            DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
            row.Cells[0].Value = "";
            row.Cells[1].Value = operation;
            row.Cells[2].Value = "Qty:" + wafercountfromCBTED + "/" + MESWaferQty;
            row.Cells[3].Value = "";
            row.Cells[4].Value = "";
            dataGridView2.Rows.Add(row);
            //remove the 6051 row for 7180
            if(operation =="7180")
            {
                for (int i = dataGridView2.Rows.Count - 1; i >= 0; i--)
                {
                    var datarow = dataGridView2.Rows[i];
                    try
                    {
                        if ((datarow.Cells[1].Value.ToString().Equals("1427")) || (datarow.Cells[1].Value.ToString().Equals("6051")))
                            dataGridView2.Rows.Remove(datarow);
                    }
                    catch
                    { }
                }
            }
            if (operation == "6617")
            {
                for (int i = dataGridView2.Rows.Count - 1; i >= 0; i--)
                {
                    var datarow = dataGridView2.Rows[i];
                    try
                    {
                        if ((datarow.Cells[1].Value.ToString().Equals("6142")) || (datarow.Cells[1].Value.ToString().Equals("6051")))
                            dataGridView2.Rows.Remove(datarow);
                    }
                    catch
                    { }
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
            SODprogressBar.Visible = false;
        }
        private void SendLotIDandCheckIfPageLoaded(string lotid, string checkforthis)
        {
            var starttime = DateTime.UtcNow;
            SendLotIdAndClickSet(lotid);
            starttime = DateTime.UtcNow;
            while (DateTime.UtcNow - starttime < TimeSpan.FromSeconds(25))
            {
                Thread.Sleep(300);
                Checkifthisloadedonthewebpage(checkforthis);
                if (DidThePageLoadWhatIWanted)
                {
                    GetTheInfoFromThePageAndStoreIt();
                    break;
                }
            }
            if (DidThePageLoadWhatIWanted == false)
            {
                SendLotIdAndClickSet(lotid);
                starttime = DateTime.UtcNow;
                while (DateTime.UtcNow - starttime < TimeSpan.FromSeconds(25)) // wait again, maybe its super slow?
                {
                    Thread.Sleep(100);
                    Checkifthisloadedonthewebpage(checkforthis);
                    if (DidThePageLoadWhatIWanted)
                    {
                        GetTheInfoFromThePageAndStoreIt();
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
                HtmlElementCollection elc = webBrowser1.Document.GetElementsByTagName("input");
                foreach (HtmlElement el in elc)
                {
                    if (el.Name.Equals("SetLot"))
                    {
                        el.SetAttribute("value", lotid);
                        break;
                    }
                }
                foreach (HtmlElement el in elc)
                {
                    if (el.Name.Equals("btnSetLot"))
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
                string whatloaded = webBrowser1.Document.Body.InnerText;
                string lotid = item.Substring(item.Length - 8, 8);
                if (whatloaded.Contains("Lot does not exist."))
                {
                    if (whatloaded.Contains("Lot: "+lotid))
                    {
                        previouswebpage = whatloaded;
                        DidThePageLoadWhatIWanted = true;
                        return;
                    }
                }
                if (whatloaded.Contains("No Sort data found for this lot"))
                {
                    if (whatloaded.Contains("Lot: " + lotid))
                    {
                        previouswebpage = whatloaded;
                        DidThePageLoadWhatIWanted = true;
                        return;
                    }
                }
                
                if ((whatloaded.Contains(item)) && (whatloaded != previouswebpage) &&(whatloaded.Contains("Lot  - Sort Dispo")|| whatloaded.Contains("Lot - Sort Dispo"))) //INTERFACE_BIN Bin Lot Information for D952932B at Operation 6051
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
        private void GetTheInfoFromThePageAndStoreIt ()
        {
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(GetTheInfoFromThePageAndStoreIt);
                Invoke(method);
                return;
            }
            string webinfo = webBrowser1.Document.Body.InnerHtml;
            string lotid = webinfo.Substring(webinfo.IndexOf("strong>Lot: ") + 12, 8);
            if (webinfo.Contains("Lot does not exist."))
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                row.Cells[0].Value = lotid;
                row.Cells[1].Value = "NA";
                row.Cells[2].Value = "";
                row.Cells[3].Value = "No Data, lot might have";
                row.Cells[4].Value = "been merged";
                dataGridView2.Rows.Add(row);
                return;
            }
            if (webinfo.Contains("No Sort data found for this lot"))
            {
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                row.Cells[0].Value = lotid;
                row.Cells[1].Value = "NA";
                row.Cells[2].Value = "";
                row.Cells[3].Value = "No Data, lot might still";
                row.Cells[4].Value = "be running";
                dataGridView2.Rows.Add(row);
                return;
            }
            //<input name="operationCombo$DDD$L" id="operationCombo_DDD_L_VI" type="hidden" value="6051"
            // old string tempoperation = webinfo.Substring(webinfo.IndexOf("Operation: <SELECT class=textbox name=selectOperation>"));
            // old 2 string tempoperation = webinfo.Substring(webinfo.IndexOf("<input name=\"operationCombo$DDD$L\" id=\"operationCombo_DDD_L_VI\" type=\"hidden\" value=\"") + 85, 4);
            //tempoperation = tempoperation.Substring(tempoperation.IndexOf("<input name=\"operationCombo$DDD$L\" id=\"operationCombo_DDD_L_VI\" type=\"hidden\" value=\"") + 85, 4);
            // old tempoperation = tempoperation.Substring(tempoperation.IndexOf("<OPTION selected value=")+23,4);
            // old webinfo = webinfo.Substring(webinfo.IndexOf("id=INTERFACE_BINSortInfofor") + 27, (webinfo.IndexOf("title=WAFER=AVG&amp")) - (webinfo.IndexOf("id=INTERFACE_BINSortInfofor")) - 27);
            webinfo = webinfo.Substring(webinfo.IndexOf("INTERFACE_BIN Bin Lot Information for ") + 38, (webinfo.IndexOf("TOTAL:<br>AVG:")) - (webinfo.IndexOf("INTERFACE_BIN Bin Lot Information for ")) -38);
            string TempLotID = webinfo.Substring(0, 8);
            string tempoperation = webinfo.Substring(webinfo.IndexOf("at Operation ") + 13, 4);
            string[] waferinfo = webinfo.Split(new[] { "dxgvDataRow_Glass gvRows" }, StringSplitOptions.None);
            foreach (string line in waferinfo)
            {
                if (!(line.Contains(TempLotID))) // skip the first item in the array, 
                {
                    string[] PerWaferInfo = line.Split(new[] { "</td>" }, StringSplitOptions.None);
                    PerWaferInfo = PerWaferInfo.Select(x => x.Replace("<td class=\"gvCells dx-wrap dxgv dx-ac\">", string.Empty)).ToArray();

                    //<td class=\"gvCells dx-wrap dxgv dx-ac\">
                    string tempwafer = PerWaferInfo[1].Substring(0, 3);
                    // old string tempwafer = line.Substring(0, 3);
                    string tempTPname = "";
                    string tempTED = "";
                    DateTime myDate = DateTime.Now;
                    
                    // old string[] PerWaferInfo = line.Split(new[] { "<TD>" }, StringSplitOptions.None);
                    foreach(string item in PerWaferInfo)
                    {
                        if ((item.Length == 16) || (item.Length >= 16 && item.Length <= 20 && operationselected.Contains("7180")))//Find the TP name old was 23
                        {
                            tempTPname = item;//.Substring(0, 16);
                            int pos = Array.IndexOf(PerWaferInfo, item) +3;
                            tempTED = PerWaferInfo[pos].Replace("</TD>", "").Trim();
                            myDate = DateTime.Parse(tempTED);
                            //tempTED = myDate.ToString(CultureInfo.GetCultureInfo("en-us"));
                            //tempTED = myDate.ToString("MM/dd/yyyy hh:mm:ss");
                            //tempTED = myDate.ToString("M/dd/yyyy hh:mm:ss tt");
                            tempTED = myDate.ToString("MM/dd/yyyy hh:mm:ss tt");
                            break;
                        }
                    }
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = TempLotID;
                    row.Cells[1].Value = tempoperation;
                    row.Cells[2].Value = tempwafer;
                    row.Cells[3].Value = tempTPname;
                    row.Cells[4].Value = tempTED;
                    dataGridView2.Rows.Add(row);
                }
                //else
                //{
                //    DataGridViewRow Lotrow = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                //    Lotrow.Cells[0].Value = TempLotID;
                //    dataGridView2.Rows.Add(Lotrow);
                //}
            }
            //dataGridView2.Rows.Add(1);
        }
        private void addemptyvaluesfornodatafound ( string operation, string lotID)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { addemptyvaluesfornodatafound(operation, lotID); });
                return;
            }
            DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[dataGridView2.Rows.Count -2].Clone();
            row.Cells[0].Value = lotID;
            row.Cells[1].Value = operation;
            row.Cells[2].Value = "NONE";
            row.Cells[3].Value = "NONE";
            row.Cells[4].Value = "NONE";
            dataGridView2.Rows.Add(row);
        }
        private void Pickoperationandgetinfo(string operation, string checkforthis)
        {
            var starttime = DateTime.UtcNow;
            string lotid = checkforthis.Substring((checkforthis.Length) - 26, 8);
            SendOperation(operation, lotid);
            if (NoSodDataFound == operation + " " + lotid)
                addemptyvaluesfornodatafound(operation, lotid);
            else
            {
                starttime = DateTime.UtcNow;
                while (DateTime.UtcNow - starttime < TimeSpan.FromSeconds(30))
                {
                    Thread.Sleep(300);
                    Checkifthisloadedonthewebpage(checkforthis);
                    if (DidThePageLoadWhatIWanted)
                    {
                        GetTheInfoFromThePageAndStoreIt();
                        break;
                    }
                }
                if (DidThePageLoadWhatIWanted == false)
                {
                    errorlog = errorlog + Environment.NewLine + "◙" + operation + "didnt get selected on PW, 2nd try";
                }
                else
                    DidThePageLoadWhatIWanted = false;
            }
        }
       
        private void SendOperation(string operation, string lotid)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { SendOperation(operation, lotid); });
                return;
            }
            try
            {
                
                HtmlElement el1 = webBrowser1.Document.GetElementById("operationCombo_DDD_PW-1");
                if (el1.InnerText.Contains(operation))
                {
                    //operationCombo_I
                    //SetComboItem("operationCombo_I", operation);
                    HtmlElementCollection elc = webBrowser1.Document.GetElementsByTagName("input");
                    foreach (HtmlElement el in elc)
                    {
                        if (el.Name.Equals("operationCombo")) // old selectOperation
                        {
                            el.InvokeMember("onfocus");
                            el.SetAttribute("value", operation);
                            //el.InnerText = operation;
                            //el.SetAttribute("selected", "selected");
                            el.InvokeMember("onChange");
                            el.InvokeMember("click");

                            NoSodDataFound = "";
                            break;
                        }
                    }
                }
                else //operation not selectable, no data
                {
                    NoSodDataFound = operation + " " + lotid;
                }
                
                HtmlElementCollection elc2 = webBrowser1.Document.GetElementsByTagName("input");
                foreach (HtmlElement el in elc2)
                {
                    //<input name=\"applyFilterButton\" class=\"dxb-hb\" id=\"applyFilterButton_I\" type=\"button\" value=\"Go\">
                    if (el.OuterHtml.Equals("<input name=\"applyFilterButton\" class=\"dxb-hb\" id=\"applyFilterButton_I\" type=\"button\" value=\"Go\">")) //old if (el.OuterHtml.Equals("<INPUT class=button type=submit value=GO name=submitButton>"))
                    {
                        el.InvokeMember("click");
                        break;
                    }
                }
                //Thread.Sleep(50);
                //errorlog = errorlog + Environment.NewLine +"ITEM SENT=" + item;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Unable to set your lot id and click the set button." + Environment.NewLine + exc.ToString());
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (LOTlistBox.Items.Count > 0)
            {
                DisableControls();
                RedLotList.Clear();
                GreenLotList.Clear();
                getSODteds();
                getCBTEDs();
            }
            else
            {
                MessageBox.Show("You dont have any lots in your list");
            }
        }
        private void EnableControls()
        {
            ControlsPanel.Enabled = true;
        }
        private void DisableControls()
        {
            productsList.Clear();
            ControlsPanel.Enabled = false;
        }
        public static string Format(DataGridViewRow row, string separator)
        {
            string[] values = new string[row.Cells.Count];
            for (int i = 0; i < row.Cells.Count; i++)
                values[i] = row.Cells[i].Value + "";
            return string.Join(separator, values).Trim();
        }

        private void CompareTheTEDs()
        {
            foreach (DataGridViewRow SODrow in dataGridView2.Rows)
            {
                bool RowPresent = false;
                string SODrowtoCheck = Format(SODrow, " ");
                if (SODrowtoCheck.Contains("Qty:"))
                {
                    string[] wfrarray = SODrowtoCheck.Substring(SODrowtoCheck.IndexOf("Qty:") + 4).Split('/');
                    if (wfrarray[0] == wfrarray[1])
                        SODrow.Cells[2].Style.BackColor = Color.LightGreen;
                    else
                    {
                        SODrow.Cells[2].Style.BackColor = Color.Yellow;
                        AddLotToRedList(tempLotId);
                    }
                }
                else
                {
                    try { tempLotId = SODrow.Cells[0].Value.ToString().Trim(); } catch { tempLotId = ""; }
                    if (!(string.IsNullOrEmpty(SODrowtoCheck)))
                    {
                        foreach (DataGridViewRow CBrow in dataGridView1.Rows)
                        {
                            string SODrowtoCheckCell4 = "";
                            try { SODrowtoCheckCell4 = SODrow.Cells[4].Value.ToString().Trim(); } catch { }
                            string CBrowtoCheckCell4 = "";
                            try { CBrowtoCheckCell4 = CBrow.Cells[4].Value.ToString().Trim(); } catch { }
                            try
                            {
                                DateTime converteddate = DateTime.Parse(CBrowtoCheckCell4);
                                CBrowtoCheckCell4 = converteddate.ToString("MM/dd/yyyy hh:mm:ss tt");
                            }
                            catch { }
                            if (!(string.IsNullOrEmpty(CBrowtoCheckCell4)))
                            {
                                if (SODrowtoCheckCell4 == CBrowtoCheckCell4)
                                {
                                    RowPresent = true;
                                    SODrow.Cells[4].Style.BackColor = Color.LightGreen;
                                    AddLotToGreenList(tempLotId);
                                    break;
                                }
                            }
                        }
                        if (!(SODrowtoCheck.Contains("NONE")))
                            if (!RowPresent)
                            {
                                SODrow.DefaultCellStyle.BackColor = Color.Red;
                                AddLotToRedList(tempLotId);
                            }
                    }
                }
            }
            foreach (DataGridViewRow CBrow in dataGridView1.Rows)
            {
                bool RowPresent = false;
                string CBrowtoCheck = Format(CBrow, " ");
                if (CBrowtoCheck.Contains("Qty:"))
                {
                    string[] wfrarray = CBrowtoCheck.Substring(CBrowtoCheck.IndexOf("Qty:") + 4).Split('/');
                    if (wfrarray[0] == wfrarray[1])
                        CBrow.Cells[2].Style.BackColor = Color.LightGreen;
                    else
                        CBrow.Cells[2].Style.BackColor = Color.Yellow;
                }
                else
                {
                    if (!(string.IsNullOrEmpty(CBrowtoCheck)))
                    {
                        foreach (DataGridViewRow SODrow in dataGridView2.Rows)
                        {
                            string SODrowtoCheckCell4 = "";
                            try { SODrowtoCheckCell4 = SODrow.Cells[4].Value.ToString().Trim(); } catch { }
                            string CBrowtoCheckCell4 = "";
                            try { CBrowtoCheckCell4 = CBrow.Cells[4].Value.ToString().Trim(); } catch { }
                            try
                            {
                                DateTime converteddate = DateTime.Parse(CBrowtoCheckCell4);
                                CBrowtoCheckCell4 = converteddate.ToString("MM/dd/yyyy hh:mm:ss tt");
                            }
                            catch
                            { }
                            if (!(string.IsNullOrEmpty(SODrowtoCheckCell4)))
                            {
                                if (SODrowtoCheckCell4 == CBrowtoCheckCell4)
                                {
                                    RowPresent = true;
                                    CBrow.Cells[4].Style.BackColor = Color.LightGreen;
                                    break;
                                }
                            }
                        }
                        if (!(CBrowtoCheck.Contains("NONE")))
                            if (!RowPresent)
                                CBrow.DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
            //Make buttons for wafers
            foreach (DataGridViewRow CBROW in dataGridView1.Rows)
            {
                if (CBROW.Cells[2].Value == null) //check for nulls
                    return;
                string Wafer = (CBROW.Cells[2].Value.ToString());
                if (Wafer.Length == 3)
                {
                    DataGridViewButtonCell buttonCell = new DataGridViewButtonCell();
                    CBROW.Cells[2] = buttonCell;
                }
            }
            //FM6 6751 data checks
            // 2nd socket Binning dispo requirement.
            //  1.B53 - limit 3(flag)
            //  2.B8 / 10 / 15 / 30 - limit 3(flag)
            //  3.Expected bins: B7 / B19
            List<string> Alarms = new List<string>();
            List<string> MonitoredBinlist = new List<string>(new string[] { "IB8", "IB10", "IB15", "IB30", "IB53" });
            foreach (DataGridViewRow CBROW in dataGridView1.Rows)
            {
                if ((CBROW.Cells[1].Value.ToString() == "6751") && (CBROW.Cells[8].Value.ToString().Contains("8PFK")))
                {
                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        if (MonitoredBinlist.Contains(column.Name.ToUpper()))
                        {
                            string CV = CBROW.Cells[column.Name.ToUpper()].Value.ToString();
                            int howmany = Int32.Parse(CV);
                            if (howmany > 1)
                            {
                                Alarms.Add("FM6 Alarm! " + CBROW.Cells[0].Value.ToString() + " @" + CBROW.Cells[1].Value.ToString() + " W" + CBROW.Cells[2].Value.ToString() + " " + column.Name.ToUpper()+": " + howmany.ToString()+@">=2");
                            }
                        }
                    }
                }
            }
            if (Alarms.Any()) //list is not empty
            {
                MessageBox.Show(string.Join(Environment.NewLine, Alarms));
            }

            //check for POR tape
            if (PORTapeCheckBox.Checked)
            {
                if (UserCurrentSite == "F11X")
                    MessageBox.Show("Unable to check POR tape for F11X at this time");
                else
                {
                    UBER getSQL = new UBER();
                    List<string> PORTPList = new List<string>();
                    PORTPList = getSQL.returnAMTableFromDatabase(productsList, UserCurrentSite, StartingDirOfRecipes, OlaRecipePath);
                    toolTip1.SetToolTip(PORTapeCheckBox, string.Join(Environment.NewLine, PORTPList.ToArray()));
                    foreach (DataGridViewRow SODrow in dataGridView2.Rows)
                    {
                        string cellcontents = (SODrow.Cells[3].FormattedValue.ToString());
                        if ((!(string.IsNullOrEmpty(cellcontents))) && (cellcontents != "NONE"))
                        {
                            if (PORTPList.Contains(cellcontents))
                                SODrow.Cells[3].Style.BackColor = Color.LightGreen;
                            else
                                SODrow.Cells[3].Style.BackColor = Color.Red;
                        }
                    }
                    foreach (DataGridViewRow CBRow in dataGridView1.Rows)
                    {
                        string cellcontents = (CBRow.Cells[3].FormattedValue.ToString());
                        if ((!(string.IsNullOrEmpty(cellcontents))) && (cellcontents != "NONE"))
                        {
                            if (PORTPList.Contains(cellcontents))
                                CBRow.Cells[3].Style.BackColor = Color.LightGreen;
                            else
                                CBRow.Cells[3].Style.BackColor = Color.Red;
                        }
                    }
                }
            }

            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].Width = 60;
            dataGridView1.Columns[2].Width = 60;
            dataGridView1.Columns[3].Width = 135;
            dataGridView1.Columns[4].Width = 150;
            dataGridView1.Columns[0].Frozen = true;
            dataGridView1.Columns[1].Frozen = true;
            dataGridView1.Columns[2].Frozen = true;
            EnableControls();
        }

        private void AddLotToGreenList (string lot)
        {
            if (!(GreenLotList.Contains(lot)))
                if (!(RedLotList.Contains(lot)))
                    GreenLotList.Add(lot);
        }
        private void AddLotToRedList (string lot)
        {
            if (GreenLotList.Contains(lot))
                GreenLotList.Remove(lot);
            if (!(RedLotList.Contains(lot)))
                RedLotList.Add(lot);
        }
        private void pasteLotListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cleanthelotlist();
        }

        private void copyLotListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(string.Join(Environment.NewLine, LOTlistBox.Items.Cast<String>()));
        }

        private void LOTlistBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 22) //paste
                    cleanthelotlist();
                if (e.KeyChar == 3) //copy
                {
                    Clipboard.SetText(string.Join(Environment.NewLine, LOTlistBox.SelectedItems.Cast<String>()));
                }
            }
            catch
            {
            }
            //LotdetailtextBox.Text = e.KeyChar.ToString();
        }

        private void ClearListbutton_Click(object sender, EventArgs e)
        {
            LOTlistBox.Items.Clear();
        }

        private void LOTlistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lotid = "";
            if(LOTlistBox.SelectedItems.Count == 1 )
            {
                lotid = LOTlistBox.SelectedItem.ToString();
                LotdetailtextBox.Text = lotid;
                Task t1 = Task.Factory.StartNew(() => GetLotDetail(lotid)); // set the lot id in the search box and click the set button
            }
        }
        private void GetLotDetail (string LotID)
        {
            updatelotdetailinfo("Looking...");
            UBER productCheck = new UBER();
            string SQLinfofromUber = productCheck.returnProductIDfromLOT(LotID,DatabasetextBox.Text);
            updatelotdetailinfo(SQLinfofromUber);
        }
        private void updatelotdetailinfo(string SQLinfofromUber)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { updatelotdetailinfo(SQLinfofromUber); });
                return;
            }
            if (SQLinfofromUber == "Looking...")
            {
                ProductTextBox.Text = "Looking...";
                OperationTextbox.Text = "";
                QtyTextBox.Text = "";
                WaferdataGridView.DataSource = null;
            }
            else
            {
                if ((SQLinfofromUber.Contains("Error")) | (string.IsNullOrEmpty(SQLinfofromUber)))
                {
                    ProductTextBox.Text = "error in search";
                    return;
                }
                else
                {
                    ProductTextBox.Text = SQLinfofromUber.Split(',')[0];
                    OperationTextbox.Text = SQLinfofromUber.Split(',')[3];
                    QtyTextBox.Text = SQLinfofromUber.Split(',')[4];
                    string[] WafersInLot = SQLinfofromUber.Split(',')[5].Split(';');
                    DataTable wafers = new DataTable();
                    int count = WafersInLot.Count();
                    wafers.Columns.Add();
                    for (var f = 0; f < count; f++)
                    {
                        wafers.Rows.Add(WafersInLot[f]);
                    }
                    WaferdataGridView.DataSource = wafers;
                }
            }
        }

        private void checkBox7087_CheckedChanged(object sender, EventArgs e)
        {
            getoperationsselected();
        }

        private void checkBox6751_CheckedChanged(object sender, EventArgs e)
        {
            getoperationsselected();
        }
        private void checkBox7634_CheckedChanged(object sender, EventArgs e)
        {
            getoperationsselected();
        }

        private void checkBox7466_CheckedChanged(object sender, EventArgs e)
        {
            getoperationsselected();
        }

        private void checkBox7286_CheckedChanged(object sender, EventArgs e)
        {
            getoperationsselected();
        }
        private void checkBox7180_CheckedChanged(object sender, EventArgs e)
        {
            getoperationsselected();
        }
        private void checkBox6617_CheckedChanged(object sender, EventArgs e)
        {
            getoperationsselected();
        }
        private void checkBox8361_CheckedChanged(object sender, EventArgs e)
        {
            getoperationsselected();
        }
        private void getoperationsselected()
        {
            if (checkBox7087.Checked)
            {
                if (!(operationselected.Contains("'7087'")))
                    operationselected = operationselected + " '7087'";
            }
            else
            {
                try { operationselected = operationselected.Replace(" '7087'", ""); }
                catch { }
            }
            if (checkBox6751.Checked)
            {
                if (!(operationselected.Contains("'6751'")))
                    operationselected = operationselected + " '6751'";
            }
            else
            {
                try { operationselected = operationselected.Replace(" '6751'", ""); }
                catch { }
            }
            if (checkBox7286.Checked)
            {
                if (!(operationselected.Contains("'7286'")))
                    operationselected = operationselected + " '7286'";
            }
            else
            {
                try { operationselected = operationselected.Replace(" '7286'", ""); }
                catch { }
            }

            if (checkBox7634.Checked)
            {
                if (!(operationselected.Contains("'7634'")))
                    operationselected = operationselected + " '7634'";
            }
            else
            {
                try { operationselected = operationselected.Replace(" '7634'", ""); }
                catch { }
            }

            if (checkBox7466.Checked)
            {
                if (!(operationselected.Contains("'7466'")))
                    operationselected = operationselected + " '7466'";
            }
            else
            {
                try { operationselected = operationselected.Replace(" '7466'", ""); }
                catch { }
            }
            if ((operationselected == "'6051'") || (operationselected == "'7180'") || (operationselected == "'6617'")) // they did not pick an alternate op
            {
                if (checkBox7180.Checked)
                {
                    if (!(operationselected.Contains("'7180'")))
                        operationselected = "'7180'";
                    if (checkBox8361.Checked)
                    {
                        if (!(operationselected.Contains("'8361'")))
                            operationselected = operationselected + " '8361'";
                    }
                    else
                    {
                        try { operationselected = operationselected.Replace(" '8361'", ""); }
                        catch { }
                    }
                }
                else if (checkBox6617.Checked)
                {
                    if (!(operationselected.Contains("'6617'")))
                        operationselected = "'6617'";
                }
                else
                {
                    try { operationselected = "'6051'"; }
                    catch { }
                }
            }
            //MessageBox.Show(operationselected);
        }

        private void CBprogressBar_VisibleChanged(object sender, EventArgs e)
        {
            if (CBprogressBar.Visible == false)
            {
                if (SODprogressBar.Visible == false)
                {
                    //both are done
                    previouswebpage = "";
                    CompareTheTEDs();
                }
            }
        }

        private void SODprogressBar_VisibleChanged(object sender, EventArgs e)
        {
            if (CBprogressBar.Visible == false)
            {
                if (SODprogressBar.Visible == false)
                {
                    //both are done
                    previouswebpage = "";
                    CompareTheTEDs();
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                string lotid = Convert.ToString(selectedRow.Cells[0].Value);
                if (lotid.Length == 8)
                {
                    LotdetailtextBox.Text = lotid;
                    Task t1 = Task.Factory.StartNew(() => GetLotDetail(lotid));
                }
            }
        }

        private void getLotInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView2.Rows[selectedrowindex];
                string lotid = Convert.ToString(selectedRow.Cells[0].Value);
                if (lotid.Length == 8)
                {
                    LotdetailtextBox.Text = lotid;
                    Task t1 = Task.Factory.StartNew(() => GetLotDetail(lotid));
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            toolStripMenuItem1.PerformClick();
        }

        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            getLotInfoToolStripMenuItem.PerformClick();
        }
        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 1)
            {
                if (dataGridView1.SelectedCells[0].GetType() == typeof(DataGridViewButtonCell))
                {
                    try
                    {
                        string WaferID = dataGridView1.SelectedCells[0].Value.ToString().Trim();
                        string lotnumber = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString().Trim();
                        string installpath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location.ToString());
                        if (!(File.Exists(installpath + @"\Wafer Plot.exe")))
                        {
                            File.Copy(@"\\pheoweb\RecipeEditor\Wafer Plot.exe", installpath + @"\Wafer Plot.exe");
                        }
                        string arguments = lotnumber + " " + WaferID;
                        Process.Start(installpath + @"\Wafer Plot.exe", arguments);
                    }
                    catch //maybe nothing in the cell?
                    { }
                }
            }
        }

        private void deleteLotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = LOTlistBox.SelectedIndices.Count - 1; i >= 0; i--)
            {
                LOTlistBox.Items.RemoveAt(LOTlistBox.SelectedIndices[i]);
            }
        }

        private void LOTlistBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                List<string> selectedlots = new List<string>();
                for (int i = 0; i < LOTlistBox.SelectedIndices.Count; i++)
                {
                    selectedlots.Add(LOTlistBox.Items[LOTlistBox.SelectedIndices[i]].ToString());
                }
                foreach (string item in selectedlots)
                {
                    GreenLotList.Remove(item);
                }
                for (int i = LOTlistBox.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    LOTlistBox.Items.RemoveAt(LOTlistBox.SelectedIndices[i]);
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                GetTEDButton.PerformClick();
            }
        }

        private void LotId7DigitCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (LotId7DigitCheckbox.Checked == true)
            {
                lotId7Digit = true;
            }
            else
                lotId7Digit = false;
        }

        private void ShowResortDataCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowResortDataCheckbox.Checked == true)
            {
                showCbResortData = true;
            }
            else
                showCbResortData = false;
        }

        private void pasteEngLotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string clipboardinfo = "";
            try
            {
                clipboardinfo = Clipboard.GetText();
            }
            catch
            {
                MessageBox.Show("Unable to get clipboard text, try again");
            }
            string[] lotIDs = clipboardinfo.Split(new char[] { ' ', '\t', '\n', '\r', '>', '<' });
            foreach (string line in lotIDs)
            {
                if (line.Length == 8) // make sure lot id is only 8 char
                {
                    if (!(LOTlistBox.Items.Contains(line.ToUpper())))
                        LOTlistBox.Items.Add(line.ToUpper());
                }
            }
        }
        //global brushes with ordinary/selected colors
        private SolidBrush ForegroundBrushSelected = new SolidBrush(Color.White);
        private SolidBrush ForegroundBrush = new SolidBrush(Color.Black);
        private SolidBrush BackgroundBrushSelected = new SolidBrush(Color.FromKnownColor(KnownColor.Highlight));
        private SolidBrush LightGreenBrush = new SolidBrush(Color.LightGreen);
        private SolidBrush LightPinkBrush = new SolidBrush(Color.LightPink);
        private SolidBrush BackgroundBrush2 = new SolidBrush(Color.LightGray);

        //custom method to draw the item in the lot list, DrawMode of the ListBox set to OwnerDrawFixed
        private void LOTlistBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;
            if (index >= 0 && index < LOTlistBox.Items.Count)
            {
                string text = LOTlistBox.Items[index].ToString();
                Graphics g = e.Graphics;

                //background:
                SolidBrush backgroundBrush;
                if (selected)
                    backgroundBrush = BackgroundBrushSelected;
                else if (GreenLotList.Contains(LOTlistBox.Items[index].ToString()))
                    backgroundBrush = LightGreenBrush;
                else if (RedLotList.Contains(LOTlistBox.Items[index].ToString()))
                    backgroundBrush = LightPinkBrush;
                else
                    backgroundBrush = BackgroundBrush2;
                g.FillRectangle(backgroundBrush, e.Bounds);

                //text:
                SolidBrush foregroundBrush = (selected) ? ForegroundBrushSelected : ForegroundBrush;
                g.DrawString(text, e.Font, foregroundBrush, LOTlistBox.GetItemRectangle(index).Location);
            }
            e.DrawFocusRectangle();
        }

        private void AutoProcButton_Click(object sender, EventArgs e)
        {
            //GreenLotList.Add("D805858B"); //for testing
            if (GreenLotList.Count > 0)
            {
                if(webBrowser1.Url.ToString().Contains("?"))
                    PWDispoSummaryTab = webBrowser1.Url.ToString() + "cs=Dispo Summary";
                else
                    PWDispoSummaryTab = webBrowser1.Url.ToString() + "?cs=Dispo Summary";

                if (checkBox6751.Checked)
                {
                    using (AutoProcForm BringUpAutoProcForm = new AutoProcForm())
                    {
                        BringUpAutoProcForm.MesDatabase = DatabasetextBox.Text;
                        BringUpAutoProcForm.GreenLotListFromTEDC = GreenLotList;
                        BringUpAutoProcForm.DispoSummaryUrl = PWDispoSummaryTab;
                        var result = BringUpAutoProcForm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                        }
                        else
                            webBrowser1.Navigate(PWURL+ "?cs=Lot Summary");
                    }
                }
                else if (checkBox7634.Checked)
                {
                    using (AutoProcForm_7634 BringUpAutoProcForm = new AutoProcForm_7634())
                    {
                        BringUpAutoProcForm.MesDatabase = DatabasetextBox.Text;
                        BringUpAutoProcForm.GreenLotListFromTEDC = GreenLotList;
                        BringUpAutoProcForm.DispoSummaryUrl = PWDispoSummaryTab;
                        var result = BringUpAutoProcForm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                        }
                        else
                            webBrowser1.Navigate(PWURL + "?cs=Lot Summary");
                    }
                }
                else
                {
                    using (AutoProcForm_6051 BringUp6051AutoProcForm = new AutoProcForm_6051())
                    {
                        BringUp6051AutoProcForm.MesDatabase = DatabasetextBox.Text;
                        BringUp6051AutoProcForm.GreenLotListFromTEDC = GreenLotList;
                        BringUp6051AutoProcForm.DispoSummaryUrl = PWDispoSummaryTab;
                        var result = BringUp6051AutoProcForm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                        }
                        else
                            webBrowser1.Navigate(PWURL + "?cs=Lot Summary");
                    }
                }
            }
        }

        private void ExportToJMPbutton_Click(object sender, EventArgs e)
        {
            if (!(Directory.Exists(@"C:\TEDC\ExportedTable")))
                Directory.CreateDirectory(@"C:\TEDC\ExportedTable");
            StreamWriter wr = new StreamWriter(@"C:\TEDC\ExportedTable\Datagrid.csv");
            int cols2 = dataGridView2.Columns.Count;
            for (int i = 0; i < cols2; i++)
            {
                wr.Write("SOD: " + dataGridView2.Columns[i].Name.ToString().ToUpper() + ",");
            }
            int cols1 = dataGridView1.Columns.Count;
            for (int i = 0; i < cols1; i++)
            {
                wr.Write(dataGridView1.Columns[i].Name.ToString().ToUpper() + ",");
            }
            wr.WriteLine();

            for (int i = 0; i < (dataGridView2.Rows.Count - 1); i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    if (dataGridView2.Rows[i].Cells[j].Value != null)
                    {
                        wr.Write(dataGridView2.Rows[i].Cells[j].Value + ",");
                    }
                    else
                    {
                        wr.Write(",");
                    }
                }
                for (int j = 0; j < cols1; j++)
                {
                    try
                    {
                        if (i < (dataGridView1.Rows.Count - 1))
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value != null)
                            {
                                wr.Write(dataGridView1.Rows[i].Cells[j].Value + ",");
                            }
                            else
                            {
                                wr.Write(",");
                            }
                        }
                    }
                    catch { }
                }
                wr.WriteLine();
            }
            wr.Close();
            //myJMP.Application myJMP;
            //myJMP = new myJMP.Application();
            //myJMP.Document docOpn;
            //myJMP.Visible = true;
            //docOpn = myJMP.OpenDocument(@"C:\TEDC\ExportedTable\Datagrid.csv");

            if (File.Exists(myJMPExportedTable1))
                executeWindowsCommand(myJMPExportedTable1, Path.Combine(myJMPExportedTable1, "Datagrid_" + ".csv"));
            else if (File.Exists(myJMPExportedTable2))
                executeWindowsCommand(myJMPExportedTable2, @Path.Combine(myJMPExportedTable, "Datagrid_" + ".csv"));
            else if (File.Exists(myJMPExportedTable3))
                executeWindowsCommand(myJMPExportedTable3, Path.Combine(myJMPExportedTable, "Datagrid_" + ".csv"));
        }
        public const string myJMPExportedTable = @"C:\TEDC\ExportedTable";
        public const string myJMPExportedTable1 = @"C:\Program Files\SAS\JMPPRO\14\jmp.exe";
        public const string myJMPExportedTable2 = @"C:\Program Files\SAS\JMPPRO\12\jmp.exe";
        public const string myJMPExportedTable3 = @"C:\Program Files\SAS\JMPPRO\11\jmp.exe";
        public static Boolean executeWindowsCommand(string exe, string arguments)
        {
            if (!File.Exists(exe))
            {
                //MessageBox.Show("EXE file '" + exe + "' does not Exist.  Verify settings under 'Tools->Options' Menu");
                return false;
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = exe;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            p.Start();

            return true;
        }

        private void ExportToExcelbutton_Click(object sender, EventArgs e)
        {
            if (!(Directory.Exists(@"C:\TEDC\ExportedTable")))
                Directory.CreateDirectory(@"C:\TEDC\ExportedTable");
            StreamWriter wr = new StreamWriter(@"C:\TEDC\ExportedTable\Datagrid.csv");
            int cols2 = dataGridView2.Columns.Count;
            for (int i = 0; i < cols2; i++)
            {
                wr.Write("SOD: " + dataGridView2.Columns[i].Name.ToString().ToUpper() + ",");
            }
            int cols1 = dataGridView1.Columns.Count;
            for (int i = 0; i < cols1; i++)
            {
                wr.Write(dataGridView1.Columns[i].Name.ToString().ToUpper() + ",");
            }
            wr.WriteLine();

            for (int i = 0; i < (dataGridView2.Rows.Count - 1); i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    if (dataGridView2.Rows[i].Cells[j].Value != null)
                    {
                        wr.Write(dataGridView2.Rows[i].Cells[j].Value + ",");
                    }
                    else
                    {
                        wr.Write(",");
                    }
                }
                for (int j = 0; j < cols1; j++)
                {
                    try
                    {
                        if (i < (dataGridView1.Rows.Count - 1))
                        {
                            if (dataGridView1.Rows[i].Cells[j].Value != null)
                            {
                                wr.Write(dataGridView1.Rows[i].Cells[j].Value + ",");
                            }
                            else
                            {
                                wr.Write(",");
                            }
                        }
                    }
                    catch { }
                }
                wr.WriteLine();
            }
            wr.Close();
            Process.Start(@"C:\TEDC\ExportedTable\Datagrid.csv");
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            HelpForm Helpform = new HelpForm();
            //Helpform.CurrentSelectedSite = CurrentSite;
            Helpform.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*
            Properties.Settings.Default["UserCurrentSite"] = "";
            Properties.Settings.Default["OlaRecipePath"] = "";
            Properties.Settings.Default["StartingDirOfRecipes"] = "";
            Properties.Settings.Default["MarsDB"] = "";
            Properties.Settings.Default["DatabasetextBox"] = "";
            Properties.Settings.Default["PWURL"] = "";
            Properties.Settings.Default["PWDispoSummaryTab"] = "";
            Properties.Settings.Default.Save();
            UserCurrentSite = Properties.Settings.Default["UserCurrentSite"].ToString();
            */
            //testing

            if (string.IsNullOrEmpty(UserCurrentSite))
            {
                FindUNCPaths();
                if (UserIdrive.Contains("ra2pznas01"))//D1C
                {
                    UserCurrentSite = "D1C";
                    OlaRecipePath = @"\\ra2pznas01.rf2prod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                    StartingDirOfRecipes = @"\\rf2pzwfs002.rf2prod.mfg.intel.com\STTD\Sort\Recipe\CMT\Production\";
                    MarsDB = "D1D_PROD_MARS";
                    DatabasetextBox.Text = "D1D_PROD_XEUS";
                    PWURL = "http://d1-sortweb.rf3prod.mfg.intel.com/Preon/LotSummary/LotSummary/MENU_Lot_Sort%20Dispo";
                    PWDispoSummaryTab = "http://d1-sortweb.rf3prod.mfg.intel.com/Preon/DispoSummary/DispoSummary/MENU_Lot_Sort%20Dispo";
                    //PWURL = "http://rf3pvwb515n2.rf3prod.mfg.intel.com/PW/sortDispo.asp";
                    //PWDispoSummaryTab = "http://rf3pvwb515n2.rf3prod.mfg.intel.com/PW/sortDispo.asp?cs=Dispo Summary";
                }
                if (UserIdrive.Contains("f24xcnas4"))//F24
                {
                    UserCurrentSite = "F24";
                    OlaRecipePath = @"\\f24xcnas4.f24prod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                    StartingDirOfRecipes = @"\\f24p-nas-sortrms.f24prod.mfg.intel.com\sttd\Sort\Recipe\CMT\Production\";
                    MarsDB = "F24_PROD_MARS";
                    DatabasetextBox.Text = "F24_PROD_XEUS";
                    PWURL = "http://f24-sort.f24prod.mfg.intel.com/pw/sortDispo.asp";
                    PWDispoSummaryTab = "http://f24-sort.f24prod.mfg.intel.com/PW/sortDispo.asp?cs=Dispo Summary";
                }
                if (UserIdrive.Contains("afsp8xnfs2"))//AFO
                {
                    UserCurrentSite = "AFO";
                    OlaRecipePath = @"\\afsp2xnfs1.afoprod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                    StartingDirOfRecipes = @"\\afsp8xnfs2.afoprod.mfg.intel.com\sort_recipe\Sort\Recipe\CMT\Production\";
                    MarsDB = "D1D_PROD_MARS";
                    DatabasetextBox.Text = "D1D_PROD_XEUS";
                    PWURL = "http://rf2-sort.rf2prod.mfg.intel.com/pw/sortDispo.asp";
                    PWDispoSummaryTab = "http://rf2-sort.rf2prod.mfg.intel.com/PW/sortDispo.asp?cs=Dispo Summary";
                }
                if (UserIdrive.Contains("s21netap2"))//F11X
                {
                    UserCurrentSite = "F11X";
                    OlaRecipePath = @"\\s21netap2.f21prod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                    StartingDirOfRecipes = @"\\f21pucnasn2\sortrms\Sort\Recipe\CMT\Production\";
                    MarsDB = "F21_PROD_MARS";
                    DatabasetextBox.Text = "F21_PROD_XEUS";
                    PWURL = "http://f21-sort.f21prod.mfg.intel.com/pw/sortDispo.asp";
                    PWDispoSummaryTab = "http://f21-sort.f21prod.mfg.intel.com/PW/sortDispo.asp?cs=Dispo Summary";
                }
                if (UserIdrive.Contains("f28pnas01n1"))//S28
                {
                    UserCurrentSite = "S28";
                    OlaRecipePath = @"\\f28pnas01n1.f28prod.mfg.intel.com\OlaProduction\" + "OlaDllRecipe.xml";
                    StartingDirOfRecipes = @"\\f28pnas01n1.f28prod.mfg.intel.com\SortRMS\Sort\Recipe\CMT\Production\";
                    MarsDB = "F28_PROD_MARS";
                    DatabasetextBox.Text = "F28_PROD_XEUS";
                    PWURL = "http://f28-sort.f28prod.mfg.intel.com/pw/sortDispo.asp";
                    PWDispoSummaryTab = "http://f28-sort.f28prod.mfg.intel.com/PW/sortDispo.asp?cs=Dispo Summary";
                }
                Properties.Settings.Default["UserCurrentSite"] = UserCurrentSite;
                Properties.Settings.Default["OlaRecipePath"] = OlaRecipePath;
                Properties.Settings.Default["StartingDirOfRecipes"] = StartingDirOfRecipes;
                Properties.Settings.Default["MarsDB"] = MarsDB;
                Properties.Settings.Default["DatabasetextBox"] = DatabasetextBox.Text;
                Properties.Settings.Default["PWURL"] = PWURL;
                Properties.Settings.Default["PWDispoSummaryTab"] = PWDispoSummaryTab;
                Properties.Settings.Default.Save();
            }
            if (string.IsNullOrEmpty(UserCurrentSite))
            {
                HelpForm Helpform = new HelpForm();
                //Helpform.CurrentSelectedSite = CurrentSite;
                Helpform.ShowDialog();
            }
            else
            {
                OlaRecipePath = Properties.Settings.Default["OlaRecipePath"].ToString();
                StartingDirOfRecipes = Properties.Settings.Default["StartingDirOfRecipes"].ToString();
                MarsDB = Properties.Settings.Default["MarsDB"].ToString();
                DatabasetextBox.Text = Properties.Settings.Default["DatabasetextBox"].ToString();
                PWURL = Properties.Settings.Default["PWURL"].ToString();
                PWDispoSummaryTab = Properties.Settings.Default["PWDispoSummaryTab"].ToString();
            }
            this.Text = "TestEndDateChecker(Preon) Site:" + UserCurrentSite;
            webBrowser1.Navigate(PWURL);
        }
        public void FindUNCPaths() // not needed in settings enabled version
        {
            // gets information of all drives in your computer and saves it to array list of directories
            DriveInfo[] listofdirectories = DriveInfo.GetDrives();
            foreach (DriveInfo di in listofdirectories)
            {
                //checks the mapped drives and what drive letter they are assigned to and assigns it to mapped_path
                DirectoryInfo dir = di.RootDirectory;
                string network_path = GetUNCPath(dir.FullName.Substring(0, 2));
                string Mapped_path = Convert.ToString(di);

                Users_mapped_drives.Add(new KeyValuePair<string, string>(Mapped_path, network_path));
            }
            foreach (KeyValuePair<string, string> pair in Users_mapped_drives)
            {
                if (pair.Key.Contains("I"))
                {
                    UserIdrive = pair.Value;
                    break;
                }
            }
        }
        public string GetUNCPath(string path)
        {
            // if (path.Contains(@"\\")) return path;

            ManagementObject mo = new ManagementObject();
            mo.Path = new ManagementPath(string.Format("Win32_LogicalDisk='{0}'", path)); // regular standard expression

            //DriveType 4 = Network Drive in windows
            //check to see if the drive it mapped (network drive) and if it is, it will give you the drive name back.
            if (Convert.ToUInt32(mo["DriveType"]) == 4) return Convert.ToString(mo["ProviderName"]); //give the path of provider
            else return path;
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true;
            //}
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; //enter was pressed
                if (!(LOTlistBox.Items.Contains(toolStripTextBox1.Text.ToUpper())))
                    LOTlistBox.Items.Add(toolStripTextBox1.Text.ToUpper());
            }
        }

        private void PORTapeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void currentLocationOfLotsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> Lots = new List<string>();
            foreach (string lot in LOTlistBox.Items)
            {
                Lots.Add(lot);
            }
            CheckLotStatusForm(Lots);
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
            lotstat.MesDatabase = DatabasetextBox.Text;
            lotstat.LotsThatWereAutoproced = lotsthatweretriggered;
            lotstat.OperationToCheck = "6053";
            lotstat.Show();
        }

        private void DatabasetextBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (webBrowser1.Visible)
                    webBrowser1.Visible = false;
                else
                    webBrowser1.Visible = true;
            }
        }

        
    }
}
