using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Intel.FabAuto.ESFW.DS.UBER;
using Intel.FabAuto.ESFW.DS.UBER.Interfaces;
using Intel.FabAuto.ESFW.DS.UBER.Uniqe.Core;
using Intel.FabAuto.ESFW.DS.UBER.Uniqe.QEClient;

namespace TestEndDateChecker
{
    class UBER
    {
        public DataTable ReturnLastTEDFromLot(string LotID, string Database, string[] operations, bool showCbResortData)
        {
            string operation = string.Join(",", operations);
            string showlatest = "";
            if (showCbResortData)
                showlatest = "--";
            DataTable DT = new DataTable();
            try
            {
                //string TestEndDate = "";
                string SQL = @"select
w.lot as LOT_ID,
w.operation,
w.wafer_id as Wafer,
w.program_name as TP,
w.test_end_date_time as CB_TED,
w.test_start_date_time,
w.ituff_facility as Site,
w.operation,
w.devrevstep as Prod,
w.testing_session_sequence as Sort,
w.tester_id as CMT,
w.probe_card_id as SIU,
w.total_good as TG,
w.total_bad as TB,
max(decode(r.bin_counter_id, '1', r.rollup_value, 0)) as IB1,
max(decode(r.bin_counter_id, '2', r.rollup_value, 0)) as IB2,
max(decode(r.bin_counter_id, '3', r.rollup_value, 0)) as IB3,
max(decode(r.bin_counter_id, '4', r.rollup_value, 0)) as IB4,
max(decode(r.bin_counter_id, '5', r.rollup_value, 0)) as IB5,
max(decode(r.bin_counter_id, '6', r.rollup_value, 0)) as IB6,
max(decode(r.bin_counter_id, '7', r.rollup_value, 0)) as IB7,
max(decode(r.bin_counter_id, '8', r.rollup_value, 0)) as IB8,
max(decode(r.bin_counter_id, '9', r.rollup_value, 0)) as IB9,
max(decode(r.bin_counter_id, '10', r.rollup_value, 0)) as IB10,
max(decode(r.bin_counter_id, '11', r.rollup_value, 0)) as IB11,
max(decode(r.bin_counter_id, '12', r.rollup_value, 0)) as IB12,
max(decode(r.bin_counter_id, '13', r.rollup_value, 0)) as IB13,
max(decode(r.bin_counter_id, '14', r.rollup_value, 0)) as IB14,
max(decode(r.bin_counter_id, '15', r.rollup_value, 0)) as IB15,
max(decode(r.bin_counter_id, '16', r.rollup_value, 0)) as IB16,
max(decode(r.bin_counter_id, '17', r.rollup_value, 0)) as IB17,
max(decode(r.bin_counter_id, '18', r.rollup_value, 0)) as IB18,
max(decode(r.bin_counter_id, '19', r.rollup_value, 0)) as IB19,
max(decode(r.bin_counter_id, '20', r.rollup_value, 0)) as IB20,
max(decode(r.bin_counter_id, '21', r.rollup_value, 0)) as IB21,
max(decode(r.bin_counter_id, '22', r.rollup_value, 0)) as IB22,
max(decode(r.bin_counter_id, '23', r.rollup_value, 0)) as IB23,
max(decode(r.bin_counter_id, '24', r.rollup_value, 0)) as IB24,
max(decode(r.bin_counter_id, '25', r.rollup_value, 0)) as IB25,
max(decode(r.bin_counter_id, '26', r.rollup_value, 0)) as IB26,
max(decode(r.bin_counter_id, '27', r.rollup_value, 0)) as IB27,
max(decode(r.bin_counter_id, '28', r.rollup_value, 0)) as IB28,
max(decode(r.bin_counter_id, '29', r.rollup_value, 0)) as IB29,
max(decode(r.bin_counter_id, '30', r.rollup_value, 0)) as IB30,
max(decode(r.bin_counter_id, '31', r.rollup_value, 0)) as IB31,
max(decode(r.bin_counter_id, '32', r.rollup_value, 0)) as IB32,
max(decode(r.bin_counter_id, '33', r.rollup_value, 0)) as IB33,
max(decode(r.bin_counter_id, '34', r.rollup_value, 0)) as IB34,
max(decode(r.bin_counter_id, '35', r.rollup_value, 0)) as IB35,
max(decode(r.bin_counter_id, '36', r.rollup_value, 0)) as IB36,
max(decode(r.bin_counter_id, '37', r.rollup_value, 0)) as IB37,
max(decode(r.bin_counter_id, '38', r.rollup_value, 0)) as IB38,
max(decode(r.bin_counter_id, '39', r.rollup_value, 0)) as IB39,
max(decode(r.bin_counter_id, '40', r.rollup_value, 0)) as IB40,
max(decode(r.bin_counter_id, '41', r.rollup_value, 0)) as IB41,
max(decode(r.bin_counter_id, '42', r.rollup_value, 0)) as IB42,
max(decode(r.bin_counter_id, '43', r.rollup_value, 0)) as IB43,
max(decode(r.bin_counter_id, '44', r.rollup_value, 0)) as IB44,
max(decode(r.bin_counter_id, '45', r.rollup_value, 0)) as IB45,
max(decode(r.bin_counter_id, '46', r.rollup_value, 0)) as IB46,
max(decode(r.bin_counter_id, '47', r.rollup_value, 0)) as IB47,
max(decode(r.bin_counter_id, '48', r.rollup_value, 0)) as IB48,
max(decode(r.bin_counter_id, '49', r.rollup_value, 0)) as IB49,
max(decode(r.bin_counter_id, '50', r.rollup_value, 0)) as IB50,
max(decode(r.bin_counter_id, '51', r.rollup_value, 0)) as IB51,
max(decode(r.bin_counter_id, '52', r.rollup_value, 0)) as IB52,
max(decode(r.bin_counter_id, '53', r.rollup_value, 0)) as IB53,
max(decode(r.bin_counter_id, '54', r.rollup_value, 0)) as IB54,
max(decode(r.bin_counter_id, '55', r.rollup_value, 0)) as IB55,
max(decode(r.bin_counter_id, '56', r.rollup_value, 0)) as IB56,
max(decode(r.bin_counter_id, '57', r.rollup_value, 0)) as IB57,
max(decode(r.bin_counter_id, '58', r.rollup_value, 0)) as IB58,
max(decode(r.bin_counter_id, '59', r.rollup_value, 0)) as IB59,
max(decode(r.bin_counter_id, '60', r.rollup_value, 0)) as IB60,
max(decode(r.bin_counter_id, '61', r.rollup_value, 0)) as IB61,
max(decode(r.bin_counter_id, '62', r.rollup_value, 0)) as IB62,
max(decode(r.bin_counter_id, '63', r.rollup_value, 0)) as IB63,
max(decode(r.bin_counter_id, '64', r.rollup_value, 0)) as IB64,
max(decode(r.bin_counter_id, '65', r.rollup_value, 0)) as IB65,
max(decode(r.bin_counter_id, '66', r.rollup_value, 0)) as IB66,
max(decode(r.bin_counter_id, '67', r.rollup_value, 0)) as IB67,
max(decode(r.bin_counter_id, '68', r.rollup_value, 0)) as IB68,
max(decode(r.bin_counter_id, '69', r.rollup_value, 0)) as IB69,
max(decode(r.bin_counter_id, '70', r.rollup_value, 0)) as IB70,
max(decode(r.bin_counter_id, '71', r.rollup_value, 0)) as IB71,
max(decode(r.bin_counter_id, '72', r.rollup_value, 0)) as IB72,
max(decode(r.bin_counter_id, '73', r.rollup_value, 0)) as IB73,
max(decode(r.bin_counter_id, '74', r.rollup_value, 0)) as IB74,
max(decode(r.bin_counter_id, '75', r.rollup_value, 0)) as IB75,
max(decode(r.bin_counter_id, '76', r.rollup_value, 0)) as IB76,
max(decode(r.bin_counter_id, '77', r.rollup_value, 0)) as IB77,
max(decode(r.bin_counter_id, '78', r.rollup_value, 0)) as IB78,
max(decode(r.bin_counter_id, '79', r.rollup_value, 0)) as IB79,
max(decode(r.bin_counter_id, '80', r.rollup_value, 0)) as IB80,
max(decode(r.bin_counter_id, '81', r.rollup_value, 0)) as IB81,
max(decode(r.bin_counter_id, '82', r.rollup_value, 0)) as IB82,
max(decode(r.bin_counter_id, '83', r.rollup_value, 0)) as IB83,
max(decode(r.bin_counter_id, '84', r.rollup_value, 0)) as IB84,
max(decode(r.bin_counter_id, '85', r.rollup_value, 0)) as IB85,
max(decode(r.bin_counter_id, '86', r.rollup_value, 0)) as IB86,
max(decode(r.bin_counter_id, '87', r.rollup_value, 0)) as IB87,
max(decode(r.bin_counter_id, '88', r.rollup_value, 0)) as IB88,
max(decode(r.bin_counter_id, '89', r.rollup_value, 0)) as IB89,
max(decode(r.bin_counter_id, '90', r.rollup_value, 0)) as IB90,
max(decode(r.bin_counter_id, '91', r.rollup_value, 0)) as IB91,
max(decode(r.bin_counter_id, '92', r.rollup_value, 0)) as IB92,
max(decode(r.bin_counter_id, '93', r.rollup_value, 0)) as IB93,
max(decode(r.bin_counter_id, '94', r.rollup_value, 0)) as IB94,
max(decode(r.bin_counter_id, '95', r.rollup_value, 0)) as IB95,
max(decode(r.bin_counter_id, '96', r.rollup_value, 0)) as IB96,
max(decode(r.bin_counter_id, '97', r.rollup_value, 0)) as IB97,
max(decode(r.bin_counter_id, '98', r.rollup_value, 0)) as IB98,
max(decode(r.bin_counter_id, '99', r.rollup_value, 0)) as IB99
from a_testing_session w, a_testing_session_rollup r
where w.lao_start_ww = r.lao_start_ww
and w.ts_id = r.ts_id
and w.lot like '" + LotID + @"'
and w.operation in (" + operation + @")
" + showlatest + @"and w.latest_flag in 'Y'
and w.data_domain like 'SORT'
--and w.valid_flag like 'Y'
and r.bc_type_or_name in ('IB','FB')
group by
w.lao_start_ww,
w.test_start_date_time,
w.test_end_date_time,
w.ituff_facility,
w.operation,
w.devrevstep,
w.program_name,
w.lot,
w.wafer_id,
w.testing_session_sequence,
w.latest_flag,
w.tester_id,
w.probe_card_id,
w.total_good,
w.total_bad,
w.partial_session_flag
order by  case w.operation
when 6051 then 1
when 7087 then 2
when 6751 then 3
when 7286 then 4
when 7634 then 3
when 7466 then 4
end, w.wafer_id";

                if(operation.Contains("7180")) //F21 stuff
                {
                    SQL = SQL.Replace(@" case w.operation
when 6051 then 1
when 7087 then 2
when 6751 then 3
when 7286 then 4
when 7634 then 3
when 7466 then 4
end,", 
@" case w.operation
when 7180 then 1
when 8361 then 2
end,");
                }

                    /*OLD SQL = @"select 
w.lot as LOT_ID,
w.operation,
w.wafer_id as Wafer,
w.program_name as TP,
w.test_end_date_time as CB_TED

from a_testing_session w
where w.lot = '" + LotID + @"'
and w.operation in (" + operation + @")
and w.data_domain like 'SORT'
and w.valid_flag like 'Y'
and w.latest_flag in 'Y'
order by  case w.operation 
when 6051 then 1
when 7087 then 2
when 6751 then 3
when 7286 then 4
end"; //w.operation, w.wafer_id, w.test_end_date_time */
                List<Query> queries = new List<Query>();
                Query query = new Query(SQL, Database);
                query.AddParameter("Oper", "TEST");
                queries.Add(query);

                List<DataTable> tables = new UniqeClientHelper
                {
                    Authentication = AuthMode.IWA,
                    UserId = null,
                    Site = null,
                    TimeOutInSeconds = 30
                }.GetDataTables(queries);
                //DT.Load(tables[0]);

                DT = tables[0];
                return DT;
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem getting your SQL:" + Environment.NewLine + e.ToString());
                return DT;
            }
        }
        public string returnProductIDfromLOT(string lotID, string DB)
        {
            string productreadfromlot = "";
            string sql;
            if (!(DB.Contains("XEUS")))
            {
                MessageBox.Show("make sure your mes database is a xues database.");
            }
            sql =(@"SELECT 
                    f0.lot AS lot,
                    f0.product AS product, 
                    f0.LOT_TYPE AS owner, 
                    f0.VIRTUAL_LINE AS virtual_line, 
                    f0.operation As Operation,
                    w1.QTY,
                    w1.waferlist
                    FROM 
                    F_LOT_RUN_CARD f0,
                    (Select
                      count(w0.wafer) as qty,
                      LISTAGG (SUBSTR(w0.wafer,6,3), ';') within group (order by w0.wafer ASC) as waferlist
                      from F_WAFER w0 
                      where w0.Current_lot like '" + lotID + @"'
                      group by w0.current_lot) w1
                    WHERE
					          f0.lot = '" + lotID + @"'
                    AND      f0.Operation in (
                      Select Distinct Operation From F_OPERATION o
                              Where o.latest_version = 'Y'
                               and o.src_erase_date is Null 
                                and (Upper(o.Area) like '%SORT%' OR Upper(o.Area) like '%ETEST%') 
                                and o.State = 'Active'
                                and o.Units = 'Wafers')
                    order by PREV_OPER_OUT_DATE DESC");
            
            try
            {
                List<Query> queries = new List<Query>();
                Query query = new Query(sql, DB);
                query.AddParameter("Oper", "TEST");
                queries.Add(query);
                List<DataTable> tables = new UniqeClientHelper
                {
                    Authentication = AuthMode.IWA,
                    UserId = null,
                    Site = null,
                    TimeOutInSeconds = 30
                }.GetDataTables(queries);
                if (DB.Contains("XEUS"))
                    tables.ForEach(table => productreadfromlot = table.Rows[0][1].ToString().Replace(" ", "") + "," + table.Rows[0][2].ToString().Replace(" ", "") + "," + table.Rows[0][3].ToString().Replace(" ", "") + "," + table.Rows[0][4].ToString().Replace(" ", "") + "," + table.Rows[0][5].ToString().Replace(" ", "") + "," + table.Rows[0][6].ToString().Replace(" ", ""));
                else
                    tables.ForEach(table => productreadfromlot = table.Rows[0][1].ToString().Replace(" ", "") + "," + table.Rows[0][2].ToString().Replace(" ", ""));
                /*  output = dataGridView1.Rows[0].Cells[1].Value.ToString();
                            ProductTextBox.Text = output.Replace(" ", "");
                    */
                return productreadfromlot;

            }
            catch //(Exception ex)
            {
                //MessageBox.Show(ex.Message + "Exception, couldnt get the product id from the lot number, lot number may be bad or your database name is wrong.");
                return "Error";
            }
        }

        public string returnModifiedProductCode (string product)
        {
            try
            {
                if (product.Length == 7) //A0 level stepping
                {
                    string firsthalf = product.Substring(0, 6);
                    string secondhalf = product.Substring(6);
                    product = firsthalf + "   " + secondhalf + "*";
                }
                else // A1 level stepping
                {
                    string firsthalf = product.Substring(0, 6);
                    string stepping = product.Substring(6, 1);
                    string secondhalf = product.Substring(7);
                    product = firsthalf + " " + stepping + " " + secondhalf + "*";
                }
            }
            catch { }
            return product;
        }
        public List<string> returnAMTableFromDatabase(List<string> productIDs, string Site, string StartingDirOfRecipes, string OlaRecipePath)
        {
            productIDs = productIDs.Select(s => returnModifiedProductCode(s)).ToList();
            string productID = "";
            productID = string.Join("','", productIDs);
            string sql = "";
            string DB = "";
            #region OldCode
            /*  OLD SQL: Select distinct amrec.recipe_name,amjob.am_ldr_path,amjob.am_ldr_modelname,amjob.am_ldr_process,amjob.Row_Order,amjob.entity, amjob.route, amct.ctfile, amjob.operation, amjob.product, amrec.recipe_tp,
amjob.joblist, amjob.lotattr, amjob.tool_type, amjob.probe_card, amjob.alt_trigger, amjob.englot_list,amct.set_probecard
From ms_1_f_am_f3 amjob
join ms_1_f_am_f3 amrec
on amjob.joblist = amrec.jobid
left outer join ms_1_f_am_f3 amct
on replace(amct.product,' ','') = replace(amjob.product,' ','')
and amjob.am_ldr_path = amct.am_ldr_path
and replace(amjob.product,'^',null)=amjob.product
and amct.am_ldr_objectname = 'RECIPE_CTFILE'
Where amjob.am_ldr_path like  '%CMT'
and amjob.am_ldr_process like '%GP%'
and amjob.am_ldr_modelname = 'SORT_CMT_SCNET'
and amjob.lotattr = 'EMPTY'
and amjob.am_ldr_process = amrec.am_ldr_process
and amjob.am_ldr_process = amct.am_ldr_process
and amrec.recipe_name like '%'
Order by amjob.Row_Order
                 
            if (Site == "D1C")
            {
                MarsDB = "D1D_PROD_MARS";
                sql = @"Select Distinct amrec.recipe_name,amjob.am_ldr_process,amjob.Row_Order,amjob.entity, amjob.route, amct.ctfile, amjob.operation, amjob.product, amrec.recipe_tp,
amjob.joblist, amjob.lotattr, amjob.tool_type, amjob.probe_card, amjob.alt_trigger, amjob.englot_list,amct.set_probecard
From ms_1_f_am_f3 amjob
join ms_1_f_am_f3 amrec
  on amjob.joblist = amrec.jobid
  and amjob.am_ldr_process = amrec.am_ldr_process
  and amjob.am_ldr_process like '%GP%'
  and amjob.am_ldr_objectname = 'VAR_JOBLIST'
  and amrec.am_ldr_objectname = 'VAR_RECIPENAME'
  and amjob.am_ldr_path like  '%CMT'
  and amjob.am_ldr_modelname = amrec.am_ldr_modelname
  and amjob.am_ldr_modelname like '%D1C%' -- only need to remove 'not' if you want D1C ..Leave otherwise
left outer join ms_1_f_am_f3 amct
  on replace(amct.product,' ','') = replace(amjob.product,' ','')
  and amjob.am_ldr_path = amct.am_ldr_path
  and amjob.am_ldr_process = amct.am_ldr_process
  and replace(amjob.product,'^',null)=amjob.product --only needed for NPI Checker
  and amct.am_ldr_objectname = 'RECIPE_CTFILE'
  and amjob.am_ldr_modelname = amct.am_ldr_modelname
  Where amjob.lotattr = 'EMPTY'
--Where  amrec.recipe_name like '8PCHCVAD%' --filter by product / recipe name (use amjob.product or amrec.recipe_name)
Order by amjob.Row_Order";
            }
            #endregion
            #region AFO
            if (Site == "AFO")
            {
                MarsDB = "D1D_PROD_MARS";
                sql = @"Select Distinct amrec.recipe_name,amjob.am_ldr_process,amjob.Row_Order,amjob.entity, amjob.route, amct.ctfile, amjob.operation, amjob.product, amrec.recipe_tp,
amjob.joblist, amjob.lotattr, amjob.tool_type, amjob.probe_card, amjob.alt_trigger, amjob.englot_list,amct.set_probecard
From ms_1_f_am_f3 amjob
join ms_1_f_am_f3 amrec
  on amjob.joblist = amrec.jobid
  and amjob.am_ldr_process = amrec.am_ldr_process
  and amjob.am_ldr_process like '%GP%'
  and amjob.am_ldr_objectname = 'VAR_JOBLIST'
  and amrec.am_ldr_objectname = 'VAR_RECIPENAME'
  and amjob.am_ldr_path like  '%CMT'
  and amjob.am_ldr_modelname = amrec.am_ldr_modelname
  and amjob.am_ldr_modelname not like '%D1C%' -- only need to remove 'not' if you want D1C ..Leave otherwise
left outer join ms_1_f_am_f3 amct
  on replace(amct.product,' ','') = replace(amjob.product,' ','')
  and amjob.am_ldr_path = amct.am_ldr_path
  and amjob.am_ldr_process = amct.am_ldr_process
  and replace(amjob.product,'^',null)=amjob.product --only needed for NPI Checker
  and amct.am_ldr_objectname = 'RECIPE_CTFILE'
  and amjob.am_ldr_modelname = amct.am_ldr_modelname
  Where amjob.lotattr = 'EMPTY'
Order by amjob.Row_Order";
            }
            #endregion
            #region F24
            if (Site == "F24")
            {
                MarsDB = "F24_PROD_MARS";
                sql = @"Select Distinct amrec.recipe_name,amjob.am_ldr_process,amjob.Row_Order,amjob.entity, amjob.route, amct.ctfile, amjob.operation, amjob.product, amrec.recipe_tp,
amjob.joblist, amjob.lotattr, amjob.tool_type, amjob.probe_card, amjob.alt_trigger, amjob.englot_list,amct.set_probecard
From ms_1_f_am_f3 amjob
join ms_1_f_am_f3 amrec
  on amjob.joblist = amrec.jobid
  and amjob.am_ldr_process = amrec.am_ldr_process
  and amjob.am_ldr_process like '%GP%'
  and amjob.am_ldr_objectname = 'VAR_JOBLIST'
  and amrec.am_ldr_objectname = 'VAR_RECIPENAME'
  and amjob.am_ldr_path like  '%CMT'
  and amjob.am_ldr_modelname = amrec.am_ldr_modelname
left outer join ms_1_f_am_f3 amct
  on replace(amct.product,' ','') = replace(amjob.product,' ','')
  and amjob.am_ldr_path = amct.am_ldr_path
  and amjob.am_ldr_process = amct.am_ldr_process
  and replace(amjob.product,'^',null)=amjob.product --only needed for NPI Checker
  and amct.am_ldr_objectname = 'RECIPE_CTFILE'
  and amjob.am_ldr_modelname = amct.am_ldr_modelname
  Where amjob.lotattr = 'EMPTY'
Order by amjob.Row_Order";
            }
            #endregion
            #region F11X they dont use AM yet...
            if (Site == "F11X")
            {
                MarsDB = "F21_PROD_MARS";
                sql = @"Select Distinct amrec.recipe_name,amjob.am_ldr_process,amjob.Row_Order,amjob.entity, amjob.route, amct.ctfile, amjob.operation, amjob.product, amrec.recipe_tp,
amjob.joblist, amjob.lotattr, amjob.tool_type, amjob.probe_card, amjob.alt_trigger, amjob.englot_list,amct.set_probecard
From ms_1_f_am_f3 amjob
join ms_1_f_am_f3 amrec
  on amjob.joblist = amrec.jobid
  and amjob.am_ldr_process = amrec.am_ldr_process
  and amjob.am_ldr_process like '%GP%'
  and amjob.am_ldr_objectname = 'VAR_JOBLIST'
  and amrec.am_ldr_objectname = 'VAR_RECIPENAME'
  and amjob.am_ldr_path like  '%CMT'
  and amjob.am_ldr_modelname = amrec.am_ldr_modelname
left outer join ms_1_f_am_f3 amct
  on replace(amct.product,' ','') = replace(amjob.product,' ','')
  and amjob.am_ldr_path = amct.am_ldr_path
  and amjob.am_ldr_process = amct.am_ldr_process
  and replace(amjob.product,'^',null)=amjob.product --only needed for NPI Checker
  and amct.am_ldr_objectname = 'RECIPE_CTFILE'
  and amjob.am_ldr_modelname = amct.am_ldr_modelname
  Where amjob.lotattr = 'EMPTY'
Order by amjob.Row_Order";
            }
            #endregion
            #region S28
            if (Site == "S28")
            {
                MarsDB = "F28_PROD_MARS";
                sql = @"Select Distinct amrec.recipe_name,amjob.am_ldr_process,amjob.Row_Order,amjob.entity, amjob.route, amct.ctfile, amjob.operation, amjob.product, amrec.recipe_tp,
amjob.joblist, amjob.lotattr, amjob.tool_type, amjob.probe_card, amjob.alt_trigger, amjob.englot_list,amct.set_probecard
From ms_1_f_am_f3 amjob
join ms_1_f_am_f3 amrec
  on amjob.joblist = amrec.jobid
  and amjob.am_ldr_process = amrec.am_ldr_process
  and amjob.am_ldr_process like '%GP%'
  and amjob.am_ldr_objectname = 'VAR_JOBLIST'
  and amrec.am_ldr_objectname = 'VAR_RECIPENAME'
  and amjob.am_ldr_path like  '%CMT'
  and amjob.am_ldr_modelname = amrec.am_ldr_modelname
left outer join ms_1_f_am_f3 amct
  on replace(amct.product,' ','') = replace(amjob.product,' ','')
  and amjob.am_ldr_path = amct.am_ldr_path
  and amjob.am_ldr_process = amct.am_ldr_process
  and replace(amjob.product,'^',null)=amjob.product --only needed for NPI Checker
  and amct.am_ldr_objectname = 'RECIPE_CTFILE'
  and amjob.am_ldr_modelname = amct.am_ldr_modelname
  Where amjob.lotattr = 'EMPTY'
Order by amjob.Row_Order";
            }
            #endregion
            */
            #endregion
            if (Site == "D1C")
            {
                DB = "D1D_PROD_XEUS";
                sql = @"select 
AM_RECIPE.RECIPE_NAME,
AM_JOBLIST.product

from F_AM_F3 AM_JOBLIST
join F_AM_F3 AM_RECIPE -- join on the Recipe / TP Name Table
    on AM_RECIPE.AM_LDR_OBJECTNAME = 'VAR_RECIPENAME'
    and AM_RECIPE.am_ldr_modelname = AM_JOBLIST.am_ldr_modelname
    and replace(REGEXP_SUBSTR(AM_JOBLIST.PARAMETER_LIST,'JOBLIST=[^;]+'),'JOBLIST=','') = replace(REGEXP_SUBSTR(AM_RECIPE.PARAMETER_LIST,'JOBID=[^;]+'),'JOBID=','')
where 1=1
and AM_JOBLIST.AM_LDR_OBJECTNAME = 'VAR_JOBLIST'
and AM_JOBLIST.AM_LDR_PROCESS = 'GP'
and AM_JOBLIST.am_ldr_modelname = 'SORT_CMT_SCNET'
--and AM_RECIPE.RECIPE_NAME like '%8%'
and AM_JOBLIST.product in ('" + productID + @"')
and replace(REGEXP_SUBSTR(AM_JOBLIST.PARAMETER_LIST,'LOTATTR=[^;]+'),'LOTATTR=','') ='EMPTY'";
            }
            if (Site == "AFO")
            {
                DB = "D1D_PROD_XEUS";
                sql = @"select 
AM_RECIPE.RECIPE_NAME,
AM_JOBLIST.product

from F_AM_F3 AM_JOBLIST
join F_AM_F3 AM_RECIPE -- join on the Recipe / TP Name Table
    on AM_RECIPE.AM_LDR_OBJECTNAME = 'VAR_RECIPENAME'
    and AM_RECIPE.am_ldr_modelname = AM_JOBLIST.am_ldr_modelname
    and replace(REGEXP_SUBSTR(AM_JOBLIST.PARAMETER_LIST,'JOBLIST=[^;]+'),'JOBLIST=','') = replace(REGEXP_SUBSTR(AM_RECIPE.PARAMETER_LIST,'JOBID=[^;]+'),'JOBID=','')
where 1=1
and AM_JOBLIST.AM_LDR_OBJECTNAME = 'VAR_JOBLIST'
and AM_JOBLIST.AM_LDR_PROCESS = 'GP'
and AM_JOBLIST.am_ldr_modelname = 'AFO_SORT_CMT_SCNET'
--and AM_RECIPE.RECIPE_NAME like '%8%'
and replace(REGEXP_SUBSTR(AM_JOBLIST.PARAMETER_LIST,'LOTATTR=[^;]+'),'LOTATTR=','') ='EMPTY'";
            }
            if (Site == "F24")
            {
                DB = "F24_PROD_XEUS";
                sql = @"select 
AM_RECIPE.RECIPE_NAME,
AM_JOBLIST.product

from F_AM_F3 AM_JOBLIST
join F_AM_F3 AM_RECIPE -- join on the Recipe / TP Name Table
    on AM_RECIPE.AM_LDR_OBJECTNAME = 'VAR_RECIPENAME'
    and AM_RECIPE.am_ldr_modelname = AM_JOBLIST.am_ldr_modelname
    and replace(REGEXP_SUBSTR(AM_JOBLIST.PARAMETER_LIST,'JOBLIST=[^;]+'),'JOBLIST=','') = replace(REGEXP_SUBSTR(AM_RECIPE.PARAMETER_LIST,'JOBID=[^;]+'),'JOBID=','')
where 1=1
and AM_JOBLIST.AM_LDR_OBJECTNAME = 'VAR_JOBLIST'
and AM_JOBLIST.AM_LDR_PROCESS = 'GP'
and AM_JOBLIST.am_ldr_modelname = 'SORT_CMT_SCNET'
--and AM_RECIPE.RECIPE_NAME like '%8%'
and replace(REGEXP_SUBSTR(AM_JOBLIST.PARAMETER_LIST,'LOTATTR=[^;]+'),'LOTATTR=','') ='EMPTY'";
            }
            if (Site == "S28")
            {
                DB = "F28_PROD_XEUS";
                sql = @"select 
AM_RECIPE.RECIPE_NAME,
AM_JOBLIST.product

from F_AM_F3 AM_JOBLIST
join F_AM_F3 AM_RECIPE -- join on the Recipe / TP Name Table
    on AM_RECIPE.AM_LDR_OBJECTNAME = 'VAR_RECIPENAME'
    and AM_RECIPE.am_ldr_modelname = AM_JOBLIST.am_ldr_modelname
    and replace(REGEXP_SUBSTR(AM_JOBLIST.PARAMETER_LIST,'JOBLIST=[^;]+'),'JOBLIST=','') = replace(REGEXP_SUBSTR(AM_RECIPE.PARAMETER_LIST,'JOBID=[^;]+'),'JOBID=','')
where 1=1
and AM_JOBLIST.AM_LDR_OBJECTNAME = 'VAR_JOBLIST'
and AM_JOBLIST.AM_LDR_PROCESS = 'GP'
and AM_JOBLIST.am_ldr_modelname = 'SORT_CMT_SCNET'
--and AM_RECIPE.RECIPE_NAME like '%8%'
and replace(REGEXP_SUBSTR(AM_JOBLIST.PARAMETER_LIST,'LOTATTR=[^;]+'),'LOTATTR=','') ='EMPTY'";
            }
            try
            {
                List<Query> queries = new List<Query>();
                Query query = new Query(sql, DB);
                query.AddParameter("Oper", "TEST");
                queries.Add(query);

                List<DataTable> tables = new UniqeClientHelper
                {
                    Authentication = AuthMode.IWA,
                    UserId = null,
                    Site = null,
                    TimeOutInSeconds = 40
                }.GetDataTables(queries);

                DataTable AMDT = tables[0]; // your whole AM table
                List<string> recipeslist = new List<string>();
                foreach (DataRow row in AMDT.Rows)
                {
                    foreach(string product in productIDs)
                    {
                        if (row[1].ToString() == product)
                        {
                            recipeslist.Add(row[0].ToString());  //Find your product in the AM table and then add its corrisponding recipe to a list
                        }
                    }
                }
                List<string> PORTPList = new List<string>();
                CheckNodes Checknodesofrecipe = new CheckNodes();
                foreach (string recipe in recipeslist)
                {
                    try
                    { 
                        string TPtoAdd = Checknodesofrecipe.ReturnNodevalue(StartingDirOfRecipes + recipe + ".xml", "Path"); //Find the TP name from the recipe and add it
                        TPtoAdd = TPtoAdd.Substring(TPtoAdd.LastIndexOf("\\") + 1, 16).ToUpper();
                        if (!(PORTPList.Contains(TPtoAdd)))
                            PORTPList.Add(TPtoAdd);

                        try
                        {
                            if (File.Exists(OlaRecipePath))
                            {
                                string OLAtptoAdd = Checknodesofrecipe.ReturnMatchingOlaTapeName(TPtoAdd, OlaRecipePath); //find the ola recipes and add them
                                if (!(string.IsNullOrEmpty(OLAtptoAdd)))
                                    if (!(PORTPList.Contains(OLAtptoAdd)))
                                        PORTPList.Add(OLAtptoAdd);
                            }
                        }
                        catch (Exception EXO)//couldnt access OLA
                        {
                            MessageBox.Show("Could not access your OLA dir to verify your OLA tape: " + EXO.ToString());
                        }
                    }
                    catch (Exception EXC)
                    {
                        MessageBox.Show("Issues getting your POR tp: "+" Starting DIR: "+StartingDirOfRecipes + Environment.NewLine + " recipe: " + recipe + Environment.NewLine + EXC.ToString());
                    }
                }
                if (!(File.Exists(OlaRecipePath)))
                {
                    //MessageBox.Show("Seems you dont have access to the " + OlaRecipePath + ", so the OLA tapes may appear red.");
                }
                return PORTPList;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Exception, couldnt get the AM Table or couldnt find out the por tapes from it.");
                List<string> PORTPList = new List<string>();
                PORTPList.Add("Error");
                return PORTPList;
            }
        }
        public DataTable GetOperationFromLots (List<string>Lotlist, string MesDatabase)
        {
            DataTable LotlistWithOperation;
            List<string> updatedlotlist = new List<string>();
            foreach(string lot in Lotlist)
            {
                updatedlotlist.Add("'" + lot + "'");
            }
            string ConvertedLotList = string.Join(",", updatedlotlist);
            string sql = @"
select distinct L.lot, L.operation,  L.onhold, LF.Instruction_Flag
from F_Lot l
LEFT join F_EXPT_CONTEXT LF on 
L.Lot = LF.lot and 
L.operation = LF.operation and
LF.exec_flag = 'C' and
LF.INstruction_flag = 'Y'
where L.lot in (" + ConvertedLotList + @")";

            try
            {
                List<Query> queries = new List<Query>();
                Query query = new Query(sql, MesDatabase);
                query.AddParameter("Oper", "TEST");
                queries.Add(query);

                List<DataTable> tables = new UniqeClientHelper
                {
                    Authentication = AuthMode.IWA,
                    UserId = null,
                    Site = null,
                    TimeOutInSeconds = 30
                }.GetDataTables(queries);

                LotlistWithOperation = tables[0]; 
            }
            catch
            {
                LotlistWithOperation = null;
            }
            return LotlistWithOperation;
        }
        public DataTable LotStatustable(List<string> Lots, string Database)
        {
            string lotsformatted = "";
            List<string> lotsafterformatting = new List<string>();
            foreach(string lot in Lots)
            {
                string lotbrackets = "'" + lot + "'";
                lotsafterformatting.Add(lotbrackets);
            }
            lotsformatted = "(" + string.Join(",", lotsafterformatting) + ")";

            string SQL = @"
select L.lot, L.operation, L.last_action_date
from F_LOT L
where L.lot in " + lotsformatted;

            DataTable DT = new DataTable();
            try
            {
                List<Query> queries = new List<Query>();
                Query query = new Query(SQL, Database);
                query.AddParameter("Oper", "TEST");
                queries.Add(query);
                List<DataTable> tables = new UniqeClientHelper
                {
                    Authentication = AuthMode.IWA,
                    UserId = null,
                    Site = null,
                    TimeOutInSeconds = 30
                }.GetDataTables(queries);

                DT = tables[0];
                return DT;
            }
            catch (Exception Ex)
            {
                DataColumn column;
                DataRow row;
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "Exception";
                column.AutoIncrement = false;
                column.Caption = "Exception";
                column.ReadOnly = false;
                column.Unique = false;
                DT.Columns.Add(column);
                row = DT.NewRow();
                row["Exception"] = "Exception: " + Ex.ToString();
                DT.Rows.Add(row);
                //DT.Rows[0][0] = Ex.ToString();
                //MessageBox.Show("" + Ex.ToString());
                return DT;
            }
        }
    }
}
