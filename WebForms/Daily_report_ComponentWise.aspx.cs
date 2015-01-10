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


public partial class WebForms_Daily_report_ComponentWise : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;

    string CASHTOTAL = "";
    double totalamt = 0.0;
    string CHEQUETOTAL = ""; int totalfeestudent = 0, totaltransportstudent = 0;
    double tamtwidfine = 0.0; int fine;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            txtStartDate.Attributes.Add("ReadOnly", "true");
            txtEndDate.Attributes.Add("ReadOnly", "true");
            if (!IsPostBack)
            {

            }
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

        List<DateTime> lsPaidDates = new List<DateTime>();
        // _Command.CommandText = "select distinct(paid_date) from collect_component_master where paid_date between '" + Convert.ToDateTime(txtStartDate2.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate2.Text).ToString("yyyy-MM-dd") + "' order by paid_date;";
        _Command.CommandText = "select distinct(paid_date) from collect_component_detail where paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'and mode='CASH' order by paid_date;";

        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            lsPaidDates.Add(Convert.ToDateTime(_dtReader[0]));
        } _dtReader.Close(); _dtReader.Dispose();

        HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1; objHtmlTable.BorderColor = "#FFAB60"; objHtmlTable.Attributes.Add("style", "font-family:Arial");
        HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;


        objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.ColSpan = 19; objHtmlTableCell.Align = "center"; objHtmlTableCell.Attributes.Add("style", "color:Blue; font-weight:bold; font-size:14px;"); objHtmlTableCell.InnerText = "Happy Home Public School"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.ColSpan = 19; objHtmlTableCell.Align = "center"; objHtmlTableCell.Attributes.Add("style", "color:Blue; font-weight:bold; font-size:14px;"); objHtmlTableCell.InnerText = "For The Period         " + Convert.ToDateTime(txtStartDate.Text).ToString("MMMM-yyyy") + " TO " + Convert.ToDateTime(txtEndDate.Text).ToString("MMMM-yyyy"); objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);

        #region HeaderRow
        objHtmlTableRow = new HtmlTableRow();
        objHtmlTableCell = new HtmlTableCell();


        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Sno."; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Date"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
        {

            if (kvp.Value == "Annual Charges")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ANN"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Sci,H.Sci,Physical")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Sc&Ho.Sc"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Development Charges")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "DEV"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

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
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ADM"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Tution Fee")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "TUTION"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "Activity Fee")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ACTIVITY"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else if (kvp.Value == "CLASS PRE-PRIMARY")
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "P-PRIMARY"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }

            else
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = kvp.Value.ToUpper(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }

        }

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Fine"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);


        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "FEE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "TPT"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);



        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CASH"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CHEQUE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:red; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "TOTAL"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);



        objHtmlTable.Rows.Add(objHtmlTableRow);

        #endregion

        int i = 1; int FFine = 0;

        foreach (DateTime _PaidDate in lsPaidDates)
        {
            objHtmlTableRow = new HtmlTableRow();
            objHtmlTableCell = new HtmlTableCell();
            DataTable _dtblCollectionRecord = new DataTable();
           // string SQL = "select sum(a.AMOUNT_PAID) as sum ,a.COMPONENT_ID,a.PAID_DATE from collect_component_master a where a.PAID_DATE='" + _PaidDate.ToString("yyyy-MM-dd") + "' and a.student_id in(select student_id from collect_component_detail where mode='CASH' and paid_date='" + _PaidDate.ToString("yyyy-MM-dd") + "' ) group by a.COMPONENT_ID";

            string SQL = "select sum(a.AMOUNT_PAID) as sum ,a.COMPONENT_ID,a.PAID_DATE from collect_component_master a where a.PAID_DATE='" + _PaidDate.ToString("yyyy-MM-dd") + "' and a.student_id in(select student_id from collect_component_detail where paid_date='" + _PaidDate.ToString("yyyy-MM-dd") + "' ) group by a.COMPONENT_ID";

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
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }

            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = _PaidDate.ToString("dd-MM-yyyy"); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

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
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(""); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                }

                _dtReader.Close(); _dtReader.Dispose();

            }

            _Command.CommandText = "select sum(a.FINE) as fine ,a.PAID_DATE from collect_component_detail a where a.paid_date='" + _PaidDate.ToString("yyyy-MM-dd") + "'";
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


            _Command.CommandText = "SELECT  COUNT(A.STUDENT_ID)  FROM collect_component_detail A  WHERE A.PAID_DATE='" + _PaidDate.ToString("yyyy-MM-dd") + "'";
            string totalfeecount = Convert.ToString(_Command.ExecuteScalar());

            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = totalfeecount.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            int x=0;

            Int32.TryParse(totalfeecount, out x);

            totalfeestudent=totalfeestudent+x;

            _Command.CommandText = "SELECT COUNT(DISTINCT( A.STUDENT_ID))  FROM collect_component_master A WHERE  A.PAID_DATE='" + _PaidDate.ToString("yyyy-MM-dd") + "' AND A.COMPONENT_ID='6'";
            string totaltptcount = Convert.ToString(_Command.ExecuteScalar());

            int y = 0;

            Int32.TryParse(totaltptcount, out y);

            totaltransportstudent = totaltransportstudent + y;

            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = totaltptcount.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);


           


            _Command.CommandText = "select sum(a.AMOUNT_PAID) as amount,a.PAID_DATE from collect_component_master a where a.PAID_DATE='" + _PaidDate.ToString("yyyy-MM-dd") + "' and a.student_id in(select student_id from collect_component_detail where mode='CASH' and paid_date='" + _PaidDate.ToString("yyyy-MM-dd") + "' )  ";
            _dtReader = _Command.ExecuteReader();


            if (_dtReader.Read())
            {
                string gtotal = Convert.ToString(_dtReader["amount"]);
                int Gtotal = Convert.ToInt32(gtotal) + Convert.ToInt32(fine);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(gtotal); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else
            {

                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            _dtReader.Close(); _dtReader.Dispose();


            _Command.CommandText = "select ifnull(sum(a.AMOUNT_PAID),0) as amount,a.PAID_DATE from collect_component_master a where a.PAID_DATE='" + _PaidDate.ToString("yyyy-MM-dd") + "' and a.student_id in(select student_id from collect_component_detail where mode='CHEQUE' and paid_date='" + _PaidDate.ToString("yyyy-MM-dd") + "' )  ";
            _dtReader = _Command.ExecuteReader();


            if (_dtReader.Read())
            {
                string gtotal = Convert.ToString(_dtReader["amount"]);
                int Gtotal = Convert.ToInt32(gtotal) + Convert.ToInt32(fine);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(gtotal); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            else
            {

                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
            _dtReader.Close(); _dtReader.Dispose();


            _Command.CommandText = "select sum(a.AMOUNT_PAID) as amount from collect_component_master a where a.PAID_DATE='" + _PaidDate.ToString("yyyy-MM-dd") + "' and a.student_id in(select student_id from collect_component_detail where paid_date='" + _PaidDate.ToString("yyyy-MM-dd") + "' )  ";
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
        }
        DataTable _dtblCollectionRecord1 = new DataTable();
        objHtmlTableRow = new HtmlTableRow();
        objHtmlTableCell = new HtmlTableCell();
        _Command.CommandText = "select  sum(a.AMOUNT_PAID) as amount,a.COMPONENT_ID from collect_component_master a  where a.PAID_DATE between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "' and a.student_id in(select student_id from collect_component_detail where paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "' ) group by a.COMPONENT_ID order by  cast(a.COMPONENT_ID as unsigned)";
        _dtReader = _Command.ExecuteReader();
        _dtblCollectionRecord1.Load(_dtReader);
        _dtReader.Close(); _dtReader.Dispose();
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

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = FFine.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = totalfeestudent.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = totaltransportstudent.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);




       

        _Command.CommandText = "select  sum(a.AMOUNT_PAID) as amount,a.COMPONENT_ID from collect_component_master a  where a.PAID_DATE between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'and a.student_id in(select student_id from collect_component_detail where MODE='CASH' AND paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "') ";
         CASHTOTAL = Convert.ToString(_Command.ExecuteScalar());
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = CASHTOTAL.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        _Command.CommandText = "select  sum(a.AMOUNT_PAID) as amount,a.COMPONENT_ID from collect_component_master a  where a.PAID_DATE between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'and a.student_id in(select student_id from collect_component_detail where MODE='CHEQUE' AND paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "') ";
         CHEQUETOTAL = Convert.ToString(_Command.ExecuteScalar());
         objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = CHEQUETOTAL.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);





        _Command.CommandText = "select  sum(a.AMOUNT_PAID)  as amount,a.COMPONENT_ID from collect_component_master a  where a.PAID_DATE between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'and a.student_id in(select student_id from collect_component_detail where paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "') ";
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            var gtot = Convert.ToString(_dtReader["amount"]);
            int Gtot = Convert.ToInt32(gtot) + Convert.ToInt32(FFine);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString(Gtot); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

        }



        objHtmlTable.Rows.Add(objHtmlTableRow);


        return objHtmlTable;
    }
}