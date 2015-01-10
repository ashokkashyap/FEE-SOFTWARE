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

public partial class WebForms_collectionReportDateWisecumPeriodWiseSimple : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
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
            { }
        }
    }
    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {
        btnDownloadExcel.Visible = true;
        pnlDetails.Controls.Add(FuncGetColectionPriodWise1());
    }
    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        HtmlTable _htmlTable = FuncGetColectionPriodWise1();
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
    private HtmlTable FuncGetColectionPriodWise1()
    {
        OdbcDataReader _dtReader = null; DataTable _dtblCollectionRecord = new DataTable();
        Dictionary<int, string> objDictionaryComponentList = new Dictionary<int, string>();
        _Command.CommandText = "CALL `spComponentMaster`()"; _Command.CommandType = CommandType.StoredProcedure;
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            objDictionaryComponentList.Add(Convert.ToInt32(_dtReader["component_id"]), _dtReader["component_name"].ToString());
        } _dtReader.Close(); _dtReader.Dispose();

        List<DateTime> lsPaidDates = new List<DateTime>();
        _Command.CommandText = "select distinct(paid_date) from collect_component_master where paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "' order by paid_date;";
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            lsPaidDates.Add(Convert.ToDateTime(_dtReader[0]));
        } _dtReader.Close(); _dtReader.Dispose();

        HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1; objHtmlTable.BorderColor = "#FFAB60"; objHtmlTable.Attributes.Add("style", "font-family:Arial");
        HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

        List<StudentBasicDetails> lsStudentDetails = new List<StudentBasicDetails>();
        List<CollectionOtherDetails> lsCollectionOtherDetails = new List<CollectionOtherDetails>();
        foreach (DateTime _PaidDate in lsPaidDates)
        {
            objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.ColSpan = objDictionaryComponentList.Count + 10; objHtmlTableCell.Align = "left"; objHtmlTableCell.Attributes.Add("style", "color:Blue; font-weight:bold; font-size:12px;"); objHtmlTableCell.InnerText = _PaidDate.ToString("dd-MMM-yyyy"); objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
            #region HeaderRow
            objHtmlTableRow = new HtmlTableRow();
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "SNO"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "NAME"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Class & Sec"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "MONTH"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ADM NO"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = kvp.Value.ToUpper(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            }
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "FINE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "RE.ADM CHARGE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CHEQUE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CASH"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "GRAND TOTAL"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            objHtmlTable.Rows.Add(objHtmlTableRow);
            #endregion

            string sQL = "CALL `spGetDistinctScrollNoAndStudentIDFromPaidDateAndSessionID`('" + _PaidDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
            _Command.CommandText = sQL; _Command.CommandType = CommandType.StoredProcedure;

            lsStudentDetails.Clear(); lsCollectionOtherDetails.Clear();
            _dtReader = _Command.ExecuteReader();
            while (_dtReader.Read())
            {
                CollectionOtherDetails objCollectionOtherDetails = new CollectionOtherDetails();
                objCollectionOtherDetails.ScrollNo = Convert.ToInt32(_dtReader["SCROLL_NO"]);
                objCollectionOtherDetails.StudentID = Convert.ToInt32(_dtReader["STUDENT_ID"]);
                objCollectionOtherDetails.MappedDate = Convert.ToDateTime(_dtReader["MAPPED_DATE"]);
                lsCollectionOtherDetails.Add(objCollectionOtherDetails);
                //dicScrollNumberAndStudentID.Add(Convert.ToInt32(_dtReader["SCROLL_NO"]), Convert.ToInt32(_dtReader["STUDENT_ID"]));
            } _dtReader.Close(); _dtReader.Dispose();

            _dtblCollectionRecord.Clear();
            sQL = "CALL `spAllCollectionDetailsFromPaidDateAndSessionID`('" + _PaidDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
            _Command.CommandText = sQL; _Command.CommandType = CommandType.StoredProcedure;
            _dtReader = _Command.ExecuteReader();
            _dtblCollectionRecord.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();

            foreach (CollectionOtherDetails objCollectionOtherDetails in lsCollectionOtherDetails)
            {
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(objCollectionOtherDetails.ScrollNo); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                sQL = "CALL `spStudentBasicDetailsFromStudentID`('" + Convert.ToString(objCollectionOtherDetails.StudentID) + "')";
                _Command.CommandText = sQL; _Command.CommandType = CommandType.StoredProcedure;
                _dtReader = _Command.ExecuteReader(); lsStudentDetails.Clear();
                while (_dtReader.Read())
                {
                    StudentBasicDetails objStudentBasicDetails = new StudentBasicDetails();
                    objStudentBasicDetails.StudentID = Convert.ToInt32(objCollectionOtherDetails.StudentID);
                    objStudentBasicDetails.Name = Convert.ToString(_dtReader["NAME"]);
                    objStudentBasicDetails.Class = Convert.ToString(_dtReader["CLASS"]);
                    objStudentBasicDetails.AdmissionNumber = Convert.ToString(_dtReader["Admission_No"]);
                    lsStudentDetails.Add(objStudentBasicDetails);
                } _dtReader.Close(); _dtReader.Dispose();

                foreach (StudentBasicDetails _StudentBasicDetails in lsStudentDetails)
                {
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px;"); objHtmlTableCell.NoWrap = true; objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.Name); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.Class); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString((new System.Globalization.DateTimeFormatInfo()).GetMonthName(objCollectionOtherDetails.MappedDate.Month)); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.AdmissionNumber); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                }

                double VarAmountPaid = 0.0;
                foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
                {
                    var Amount = from a in _dtblCollectionRecord.AsEnumerable() where Convert.ToInt32(a["STUDENT_ID"]).Equals(Convert.ToInt32(objCollectionOtherDetails.StudentID)) && Convert.ToInt32(a["COMPONENT_ID"]).Equals(Convert.ToInt32(kvp.Key)) && Convert.ToDateTime(a["MAPPED_DATE"]).Equals(Convert.ToDateTime(objCollectionOtherDetails.MappedDate)) && Convert.ToInt32(a["SCROLL_NO"]).Equals(Convert.ToInt32(objCollectionOtherDetails.ScrollNo)) select a["AMOUNT_PAID"];
                    if (Amount.Any())
                    {
                        foreach (var _Amount in Amount)
                        {
                            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_Amount); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                            VarAmountPaid += Convert.ToDouble(_Amount);
                        }
                    }
                    else
                    { objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(""); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); }
                }

                var DetailID = (from a in _dtblCollectionRecord.AsEnumerable() where Convert.ToInt32(a["STUDENT_ID"]).Equals(Convert.ToInt32(objCollectionOtherDetails.StudentID)) && Convert.ToDateTime(a["MAPPED_DATE"]).Equals(Convert.ToDateTime(objCollectionOtherDetails.MappedDate)) && Convert.ToInt32(a["SCROLL_NO"]).Equals(Convert.ToInt32(objCollectionOtherDetails.ScrollNo)) select a["DETAIL_ID"]).Take(1);
                foreach (var _DetailID in DetailID)
                {
                    _Command.CommandText = "select * from collect_component_detail where ID=" + Convert.ToInt32(_DetailID);
                    _dtReader = _Command.ExecuteReader();
                    while (_dtReader.Read())
                    {
                        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["FINE"]); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["RE_ADM_CHARGES"]); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                        if (Convert.ToString(_dtReader["MODE"]).Equals("CASH"))
                        {
                            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(" "); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(Convert.ToInt32(VarAmountPaid)); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                        }
                        else
                        {
                            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(Convert.ToInt32(VarAmountPaid)); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(" "); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                        }
                    } _dtReader.Close(); _dtReader.Dispose();

                }

                objHtmlTable.Rows.Add(objHtmlTableRow);
            }
            objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.ColSpan = objDictionaryComponentList.Count + 10; objHtmlTableCell.Attributes.Add("style", "color:Blue; font-weight:bold; font-size:12px;"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableCell.Height = "10px"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        }
        return objHtmlTable;
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
}