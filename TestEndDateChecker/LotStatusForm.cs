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
    public partial class LotStatusForm : Form
    {
        int countdowntimer = 60;
        int countdowntimersetting = 60;
        bool countdowntimerstarted = false;
        DateTime timeupdated;
        string timeleftoncounter = "";
        System.Windows.Forms.Timer timerforcountdown;
        Thread autoref;
        int totalCycles = 0;
        Color CurrentColor = Color.Yellow;
        List<string> LotlistLeftToCheck = new List<string>();
        public LotStatusForm()
        {
            InitializeComponent();
            //timerforcountdown = new System.Windows.Forms.Timer();
            //timerforcountdown.Tick += new EventHandler(timerforcountdown_Tick);
            //timerforcountdown.Interval = 1000; // 1 second
            //timerforcountdown.Start();
        }
        public List<string> LotsThatWereAutoproced { get; set; }
        public string MesDatabase { get; set; }

        public string OperationToCheck { get; set; }
        private void LotStatusForm_Load(object sender, EventArgs e)
        {
            //LotlistLeftToCheck = LotsThatWereAutoproced; //this is bad, it linked the lists... wtf
            LotlistLeftToCheck.AddRange(LotsThatWereAutoproced);
            ManualRefreshButton.PerformClick();
        }

        private void ManualRefreshButton_Click(object sender, EventArgs e)
        {
            countdowntimerstarted = true;
            countdowntimerstart();
            countdowntimer = 0;
            RefreshStatusProgressBar.Value = 0;
            timer1.Start();
        }
        private void countdowntimerstart()
        {
            countdowntimer = countdowntimersetting;
            timeupdated = DateTime.Now;
            timeleftoncounter = countdowntimer.ToString();
        }
        private void timerforcountdown_Tick(object sender, EventArgs e)
        {
            double calculate = (double)countdowntimersetting / (double)100;
            calculate = 1 / calculate;
            int howmuchtonicrement = Convert.ToInt32(calculate);
            if (countdowntimerstarted)
            {
                if (!(countdowntimer == 0))
                {
                    countdowntimer--;
                }
                else
                {
                    autoref = new Thread(new ThreadStart(autorefresh));
                    autoref.IsBackground = true;
                    autoref.Start();
                    RefreshStatusProgressBar.Value = 0;

                }
                timeleftoncounter = countdowntimer.ToString();
                RefreshStatusProgressBar.Increment(howmuchtonicrement);
            }
        }
        public void autorefresh()
        {
            try
            {
                if (countdowntimer < 1)
                {
                    countdowntimer = countdowntimersetting;
                    // do stuff here
                    UBER lotsstatus = new UBER();
                    DataTable DT = lotsstatus.LotStatustable(LotsThatWereAutoproced, MesDatabase);
                    updatedatethedatagridview(DT);
                }
            }
            catch
            {
                countdowntimer = countdowntimersetting;
            }
        }
        private void manualRefresh()
        {
            UBER lotsstatus = new UBER();
            DataTable DT = lotsstatus.LotStatustable(LotsThatWereAutoproced, MesDatabase);
            updatedatethedatagridview(DT);
        }
        private void updatedatethedatagridview(DataTable DTM)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { updatedatethedatagridview(DTM); });
                return;
            }
            LotStatusDataGridView.DataSource = DTM;
            try
            {
                LotStatusDataGridView.Columns[0].Width = 60;
                LotStatusDataGridView.Columns[1].Width = 90;
                LotStatusDataGridView.Columns[2].Width = 200;
            }
            catch { }
            foreach (DataGridViewRow datarow in LotStatusDataGridView.Rows)
            {
                try
                {
                    if (datarow.Cells[1].Value.ToString().Trim() == OperationToCheck) //operation check
                    {
                        datarow.DefaultCellStyle.BackColor = CurrentColor;
                    }
                    else // good, its on its way
                    {
                        LotlistLeftToCheck.Remove(datarow.Cells[0].Value.ToString().Trim());
                        datarow.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                }
                catch { }//empty row likely
            }
            totalCycles++;

            if (LotlistLeftToCheck.Any()) //list is not empty
            {
                if (totalCycles > 10)//more than 10 minutes have passed, ping user
                {
                    CurrentColor = Color.LightPink;
                    FlashWindow.Flash(this, 5);
                    totalCycles = 0;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double calculate = (double)countdowntimersetting / (double)100;
            calculate = 1 / calculate;
            int howmuchtonicrement = Convert.ToInt32(calculate);
            if (countdowntimerstarted)
            {
                if (!(countdowntimer == 0))
                {
                    countdowntimer--;
                }
                else
                {
                    autoref = new Thread(new ThreadStart(autorefresh));
                    autoref.IsBackground = true;
                    autoref.Start();
                    RefreshStatusProgressBar.Value = 0;

                }
                timeleftoncounter = countdowntimer.ToString();
                RefreshStatusProgressBar.Increment(howmuchtonicrement);
            }
        }
    }
}
