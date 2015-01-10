using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Web.UI.HtmlControls;
using System.IO;


public partial class WebForms_Dailyrepoet_componentWise_all : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
    double totalamt = 0.0; OdbcDataReader Reader = null;
    double tamtwidfine = 0.0; int fine;
    DataTable _students = new DataTable(); string mode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;


            txtStartDate.Attributes.Add("ReadOnly", "true");


            _students = new DataTable();
            _Command.CommandText = "select a.STUDENT_REGISTRATION_NBR,a.STUDENT_ID, concat(a.FIRST_NAME,' ',a.MIDDLE_NAME,' ',a.LAST_NAME) as name from ign_student_master a";
            Reader = _Command.ExecuteReader();
            _students.Load(Reader);
            Reader.Close(); Reader.Dispose();



        }
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        btnDownloadExcel.Visible = true;
        pnlDetails.Controls.Add(FuncGetColectionPriodWisecash());
    }
    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        HtmlTable _htmlTable = FuncGetColectionPriodWisecash();
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        _htmlTable.RenderControl(hw);

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=CollectionReportAsOn" + DateTime.Now.ToString("dd-MM-yyyy").Trim().Replace(" ", "_") + ".xls");
        Response.ContentType = "application/vnd.ms-excel";
        this.EnableViewState = false;
        Response.Write(sw.ToString());
        Response.End();
    }



    public class CollectionOtherDetails
    {
        public int ScrollNo { get; set; }
        public int StudentID { get; set; }
        public DateTime MappedDate { get; set; }
    }
    public class StudentBasicDetails
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string AdmissionNumber { get; set; }
    }
    
    
 


    private HtmlTable FuncGetColectionPriodWisecash()
    {
        OdbcDataReader _dtReader = null;
        Dictionary<int, string> objDictionaryComponentList = new Dictionary<int, string>();
        _Command.CommandText = "CALL `spComponentMaster`()"; _Command.CommandType = CommandType.StoredProcedure;
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            objDictionaryComponentList.Add(Convert.ToInt32(_dtReader["component_id"]), _dtReader["component_name"].ToString());
        }
        _dtReader.Close();
        _dtReader.Dispose();

        List<Int32> lsPaidDates = new List<Int32>();
        // _Command.CommandText = "select distinct(paid_date) from collect_component_master where paid_date between '" + Convert.ToDateTime(txtStartDate2.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate2.Text).ToString("yyyy-MM-dd") + "' order by paid_date;";
        _Command.CommandText = "select distinct(student_id) from collect_component_detail where paid_date = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' order by paid_date;";

        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            lsPaidDates.Add(Convert.ToInt32(_dtReader[0]));
        } _dtReader.Close(); _dtReader.Dispose();
        _Command.CommandText = "select min(a.Rno) as min ,max(a.Rno) as max from collect_component_detail a  where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "'";
        _dtReader = _Command.ExecuteReader();

        var minrecno = "";
        var maxrecno = "";
        if (_dtReader.Read())
        {
            minrecno = Convert.ToString(_dtReader["min"]);
            maxrecno = Convert.ToString(_dtReader["max"]);
        } _dtReader.Close();


        HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1; objHtmlTable.BorderColor = "#000000"; objHtmlTable.Attributes.Add("style", "font-family:Arial");
        HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;


        objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.ColSpan = 18; objHtmlTableCell.Align = "center"; objHtmlTableCell.Attributes.Add("style", "color:Blue; font-weight:bold; font-size:14px;"); objHtmlTableCell.InnerText = "Happy Home Public School"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.ColSpan = 18; objHtmlTableCell.Align = "center"; objHtmlTableCell.Attributes.Add("style", "color:Blue; font-weight:bold; font-size:14px;"); objHtmlTableCell.InnerText = "For The Period         " + Convert.ToDateTime(txtStartDate.Text).ToString("dd-MMMM-yyyy") + " TO " + Convert.ToDateTime(txtStartDate.Text).ToString("dd-MMMM-yyyy") + "                RNO (" + Convert.ToString(minrecno) + " TO " + Convert.ToString(maxrecno) + " )"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);

        #region HeaderRow
        objHtmlTableRow = new HtmlTableRow();
        objHtmlTableCell = new HtmlTableCell();
        objHtmlTableCell.Attributes.Add("border-bottom", "solid red 1px");


        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Sno."; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "RNO."; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Name"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Admno"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
        {

            if (kvp.Value == "Annual Charges")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ANN FEE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Reg Charges")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "REG FEE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Dev. Charges")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "DEV FEE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }

            else if (kvp.Value == "Educom,SMS,Medical")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "EDU&SMS"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }

            else if (kvp.Value == "Conveyance")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CONY"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }

            else if (kvp.Value == "Security(Refundable)")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "SECT"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Physical Education")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "PHY-EDU"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "COMPUTER FEE")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "COMP"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }

            else if (kvp.Value == "ADMISSION FEE")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ADM FEE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Tuition Fee")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "TUTION FEE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Activity")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ACTIVITY"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "CLASS PRE-PRIMARY")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "P-PRIMARY"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Admission Charges")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ADM FEE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Orientation")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ORI FEE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }



            else
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = kvp.Value.ToUpper(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }

        }

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Fine"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Amount"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);


        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Mode"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTable.Rows.Add(objHtmlTableRow);
        #endregion
        int i = 1; int FFine = 0;
        foreach (Int32 _PaidDate in lsPaidDates)
        {
            objHtmlTableRow = new HtmlTableRow();
            objHtmlTableCell = new HtmlTableCell();
            DataTable _dtblCollectionRecord = new DataTable();
            string SQL = "select sum(a.AMOUNT_PAID) as sum ,a.COMPONENT_ID,a.PAID_DATE from collect_component_master a where a.PAID_DATE='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.student_id='" + _PaidDate + "' group by a.COMPONENT_ID";
            _Command.CommandText = SQL;
            _dtReader = _Command.ExecuteReader();
            _dtblCollectionRecord.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();
            double VarAmountPaid = 0.0;
            if (i > 0)
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = i.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            }
            else
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; border-bottom, solid red 1px; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }

            _Command.CommandText = " select a.rno from collect_component_detail a where a.student_id='" + _PaidDate + "' and a.paid_date='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "'";
            string r_no = _Command.ExecuteScalar().ToString();
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; border-bottom, solid red 1px; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(r_no); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);


            var name = from a in _students.AsEnumerable() where Convert.ToInt32(a["student_id"]).Equals(Convert.ToInt32(_PaidDate)) select a;
            string admno = ""; string sname = "";
            foreach (var stduntname in name)
            {
                admno = stduntname["STUDENT_REGISTRATION_NBR"].ToString();
                sname = stduntname["name"].ToString();
            }

            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(sname); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(admno); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
            {


                var Amount = from a in _dtblCollectionRecord.AsEnumerable() where Convert.ToInt32(a["COMPONENT_ID"]).Equals(Convert.ToInt32(kvp.Key)) select a["sum"];
                if (Amount.Any())
                {

                    foreach (var _Amount in Amount)
                    {

                        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_Amount); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

                    }
                }
                else
                {
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; border-bottom, solid red 1px; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(""); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                }

                _dtReader.Close(); _dtReader.Dispose();

            }
            _Command.CommandText = "select ifnull(sum(a.FINE),0) as fine ,a.PAID_DATE from collect_component_detail a where a.paid_date='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.student_id='"+_PaidDate+"'";
            _dtReader = _Command.ExecuteReader();

            if (_dtReader.Read())
            {
                fine = Convert.ToInt32(_dtReader["fine"]);
                FFine = FFine + fine;
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = fine.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else
            {

                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            _dtReader.Close(); _dtReader.Dispose();



            _Command.CommandText = "select ifnull(sum(a.AMOUNT_PAID),0) as amount,a.PAID_DATE from collect_component_master a where a.PAID_DATE='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.student_id ='" + _PaidDate + "'";
            _dtReader = _Command.ExecuteReader();


            if (_dtReader.Read())
            {
                string gtotal = Convert.ToString(_dtReader["amount"]);
                int Gtotal = Convert.ToInt32(gtotal) + Convert.ToInt32(fine);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(Gtotal); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else
            {

                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            _dtReader.Close(); _dtReader.Dispose();



            objHtmlTable.Rows.Add(objHtmlTableRow);
            i++;

            fine += fine;

            _Command.CommandText = " select case a.MODE when 'cash' then  a.MODE else a.cheque_number end as modee,a.rno from collect_component_detail a where a.student_id='" + _PaidDate + "' and a.paid_date='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "'";
            _dtReader = _Command.ExecuteReader();
            if (_dtReader.Read())
            {

                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["modee"]); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);


            }
            _dtReader.Close();
           
        }
        DataTable _dtblCollectionRecord1 = new DataTable();
        objHtmlTableRow = new HtmlTableRow();
        objHtmlTableCell = new HtmlTableCell();
        _Command.CommandText = "select  sum(a.AMOUNT_PAID) as amount,a.COMPONENT_ID from collect_component_master a  where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' group by a.COMPONENT_ID order by  cast(a.COMPONENT_ID as unsigned)";
        _dtReader = _Command.ExecuteReader();
        _dtblCollectionRecord1.Load(_dtReader);
        _dtReader.Close(); _dtReader.Dispose();
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = " G total"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
        {
            var Amount = from a in _dtblCollectionRecord1.AsEnumerable() where Convert.ToInt32(a["COMPONENT_ID"]).Equals(Convert.ToInt32(kvp.Key)) select a["amount"];

            if (Amount.Any())
            {
                foreach (var sum in Amount)
                {

                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString(sum); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

                }
            }
            else
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
        }
       
        _dtReader.Close();
        _Command.CommandText = "select  ifnull(sum(a.AMOUNT_PAID),0) as amount,a.COMPONENT_ID from collect_component_master a  where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "'";
        _dtReader = _Command.ExecuteReader();
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = FFine.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        while (_dtReader.Read())
        {
            var gtot = Convert.ToString(_dtReader["amount"]);
            int Gtot = Convert.ToInt32(gtot) + Convert.ToInt32(FFine);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString(Gtot); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        } _dtReader.Close();
       




        objHtmlTable.Rows.Add(objHtmlTableRow);


        return objHtmlTable;
    }
}