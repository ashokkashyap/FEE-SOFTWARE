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

public partial class WebForms_StudentcollectionReportTotal : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; string Student_id = "";
    double totalamt = 0.0;
    double tamtwidfine = 0.0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
          
            if (!IsPostBack)
            { }
        }
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
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

        _Command.CommandText = "call spStudentDetailsfromAdmissionNo('" + TxtHGMnbr.Text.Trim() + "')"; _Command.CommandType = CommandType.StoredProcedure;
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        { 
                Student_id = Convert.ToString(_dtReader["STUDENT_ID"]);
        }
         _dtReader.Close(); _dtReader.Dispose();



        List<DateTime> lsPaidDates = new List<DateTime>();
        _Command.CommandText = "select distinct(MAPPED_DATE) from collect_component_master  order by MAPPED_DATE;";
        //Response.Write(_Command.CommandText); Response.End();
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            lsPaidDates.Add(Convert.ToDateTime(_dtReader[0]));
        } _dtReader.Close(); _dtReader.Dispose();

        HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1; objHtmlTable.BorderColor = "#FFAB60"; objHtmlTable.Attributes.Add("style", "font-family:Arial");
        HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

        //Dictionary<int, int> dicScrollNumberAndStudentID = new Dictionary<int, int>();
        List<StudentBasicDetails> lsStudentDetails = new List<StudentBasicDetails>();
        List<CollectionOtherDetails> lsCollectionOtherDetails = new List<CollectionOtherDetails>();
        //foreach (DateTime _PaidDate in lsPaidDates)
        //{
        //    Response.Write(_PaidDate.ToString());
        //} Response.End();
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
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "FINE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "RE.ADM CHARGE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CHEQUE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CASH"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "GRAND TOTAL"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            objHtmlTable.Rows.Add(objHtmlTableRow);
            #endregion
            string sQL = "";


            sQL = "CALL `spAllCollectionDetailsForStudentPaidDateAndSessionID`('" + _PaidDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(Session["_SessionID"]) + "','" + Convert.ToInt32(Student_id) + "')";
          
            _Command.CommandText = sQL; _Command.CommandType = CommandType.StoredProcedure;
            _dtReader = _Command.ExecuteReader();
            _dtblCollectionRecord.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();
            int fcharge = 0;
          //  foreach (CollectionOtherDetails objCollectionOtherDetails in lsCollectionOtherDetails)
            //{
                objHtmlTableRow = new HtmlTableRow();
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(objCollectionOtherDetails.ScrollNo); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                sQL = "CALL `spStudentBasicDetailsFromStudentID`('" + Student_id + "')";
                _Command.CommandText = sQL; _Command.CommandType = CommandType.StoredProcedure;
                _dtReader = _Command.ExecuteReader(); lsStudentDetails.Clear();
                while (_dtReader.Read())
                {
                    StudentBasicDetails objStudentBasicDetails = new StudentBasicDetails();
                    objStudentBasicDetails.StudentID = Convert.ToInt32(Student_id);
                    objStudentBasicDetails.Name = Convert.ToString(_dtReader["NAME"]);
                    objStudentBasicDetails.Class = Convert.ToString(_dtReader["CLASS"]);
                    objStudentBasicDetails.AdmissionNumber = Convert.ToString(_dtReader["Admission_No"]);
                    lsStudentDetails.Add(objStudentBasicDetails);
                } _dtReader.Close(); _dtReader.Dispose();

                foreach (StudentBasicDetails _StudentBasicDetails in lsStudentDetails)
                {
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px;"); objHtmlTableCell.NoWrap = true; objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.Name); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.Class); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString((new System.Globalization.DateTimeFormatInfo()).GetMonthName(_PaidDate.Month)); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.AdmissionNumber); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                }

                double VarAmountPaid = 0.0;
                foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
                {
                    var Amount = from a in _dtblCollectionRecord.AsEnumerable() where Convert.ToInt32(a["STUDENT_ID"]).Equals(Convert.ToInt32(Student_id)) && Convert.ToInt32(a["COMPONENT_ID"]).Equals(Convert.ToInt32(kvp.Key)) && Convert.ToDateTime(a["MAPPED_DATE"]).Equals(Convert.ToDateTime(_PaidDate)) select a["AMOUNT_PAYBLE"] ;
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
                totalamt = 0.0;

                //var DetailID = (from a in _dtblCollectionRecord.AsEnumerable() where Convert.ToInt32(a["STUDENT_ID"]).Equals(Convert.ToInt32(objCollectionOtherDetails.StudentID)) && Convert.ToDateTime(a["MAPPED_DATE"]).Equals(Convert.ToDateTime(objCollectionOtherDetails.MappedDate)) && Convert.ToInt32(a["SCROLL_NO"]).Equals(Convert.ToInt32(objCollectionOtherDetails.ScrollNo)) select a["DETAIL_ID"]).Take(1);
                //foreach (var _DetailID in DetailID)
                {
                    //_Command.CommandText = "select * from collect_component_detail where ID=" + Convert.ToInt32(_DetailID);
                    //_dtReader = _Command.ExecuteReader();
                    //while (_dtReader.Read())
                    //{
                    //    //  Response.Write(fcharge);
                    //    if (fcharge == Convert.ToInt32(_DetailID))
                    //    {
                    //        fcharge = Convert.ToInt32(_DetailID);
                    //        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //        if (Convert.ToString(_dtReader["MODE"]).Equals("CASH"))
                    //        {
                    //            tamtwidfine = VarAmountPaid;
                    //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(" "); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(Convert.ToInt32(tamtwidfine)); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //        }
                    //        else
                    //        {
                    //            tamtwidfine = VarAmountPaid;
                    //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(Convert.ToInt32(tamtwidfine)); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(" "); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        fcharge = Convert.ToInt32(_DetailID);

                    //        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["FINE"]); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["RE_ADM_CHARGES"]); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //        if (Convert.ToString(_dtReader["MODE"]).Equals("CASH"))
                    //        {
                    //            tamtwidfine = VarAmountPaid + Convert.ToInt32(_dtReader["FINE"]) + Convert.ToInt32(_dtReader["RE_ADM_CHARGES"]);
                    //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(" "); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(Convert.ToInt32(tamtwidfine)); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //        }
                    //        else
                    //        {
                    //            tamtwidfine = VarAmountPaid + Convert.ToInt32(_dtReader["FINE"]) + Convert.ToInt32(_dtReader["RE_ADM_CHARGES"]);
                    //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(Convert.ToInt32(tamtwidfine)); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(" "); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                    //        }
                    //    }

                    //    totalamt += Convert.ToDouble(tamtwidfine);
                    //    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(""); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

                    //} _dtReader.Close(); _dtReader.Dispose();

                }

                objHtmlTable.Rows.Add(objHtmlTableRow);

           // }
            // Response.End();
            // objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.ColSpan = objDictionaryComponentList.Count + 10; objHtmlTableCell.Attributes.Add("style", "color:Blue; font-weight:bold; font-size:12px;"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableCell.Height = "10px"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);

            //objHtmlTableRow = new HtmlTableRow();
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            //foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
            //{

            //    _Command.CommandText = "select ifnull(sum(amount_paid),0)  from collect_component_master where   paid_date =  '" + _PaidDate.ToString("yyyy-MM-dd") + "'  and component_id= '" + Convert.ToInt32(kvp.Key) + "'";
            //    int amt = Convert.ToInt32(_Command.ExecuteScalar());
            //    if (amt.Equals(0))
            //    {
            //        {
            //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(amt); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //        }
            //    }
            //    else
            //    {
            //        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(amt); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //    }
            //}


            //_Command.CommandText = "select sum(a.FINE) as sfine,sum(a.RE_ADM_CHARGES) as srdm, sum(a.AMOUNT_PAID) as samtpaid, a.MODE from collect_component_detail a where paid_date =  '" + _PaidDate.ToString("yyyy-MM-dd") + "'";
            //_dtReader = _Command.ExecuteReader();
            //while (_dtReader.Read())
            //{
            //    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["sfine"]); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["srdm"]); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            //} _dtReader.Close(); _dtReader.Dispose();
            //_Command.CommandText = "select  ifnull(sum(a.amount_paid),0) as samtpaid from collect_component_detail a where   a.MODE= 'CHEQUE' and  paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + _PaidDate.ToString("yyyy-MM-dd") + "' ";
            //int tamtCHEQUE = Convert.ToInt32(_Command.ExecuteScalar());
            //{
            //    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(tamtCHEQUE); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //}
            //_Command.CommandText = "select ifnull(sum(a.amount_paid),0)  as samtpaid from collect_component_detail a where   a.MODE= 'CASH' and  paid_date =  '" + _PaidDate.ToString("yyyy-MM-dd") + "' ";
            //int tamtcash = Convert.ToInt32(_Command.ExecuteScalar());

            //{
            //    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(tamtcash); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //}


            //_Command.CommandText = "select ifnull(sum(a.amount_paid),0)   from collect_component_detail a where paid_date =  '" + _PaidDate.ToString("yyyy-MM-dd") + "'";

            //int tamt = Convert.ToInt32(_Command.ExecuteScalar());
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(tamt); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTable.Rows.Add(objHtmlTableRow);




            //objHtmlTableRow = new HtmlTableRow();
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            //foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
            //{

            //    _Command.CommandText = "select ifnull(sum(amount_paid),0)  from collect_component_master where   paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + _PaidDate.ToString("yyyy-MM-dd") + "'  and component_id= '" + Convert.ToInt32(kvp.Key) + "'";
            //    int amt = Convert.ToInt32(_Command.ExecuteScalar());
            //    if (amt.Equals(0))
            //    {
            //        {
            //            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(amt); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //        }
            //    }
            //    else
            //    {
            //        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(amt); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //    }
            //}


            //_Command.CommandText = "select sum(a.FINE) as sfine,sum(a.RE_ADM_CHARGES) as srdm, sum(a.AMOUNT_PAID) as samtpaid, a.MODE from collect_component_detail a where paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + _PaidDate.ToString("yyyy-MM-dd") + "'";
            //_dtReader = _Command.ExecuteReader();
            //while (_dtReader.Read())
            //{
            //    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["sfine"]); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            //    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["srdm"]); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            //} _dtReader.Close(); _dtReader.Dispose();


            //_Command.CommandText = "select  ifnull(sum(a.amount_paid),0)  as samtpaid, a.MODE from collect_component_detail a where   a.MODE= 'CHEQUE' and  paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + _PaidDate.ToString("yyyy-MM-dd") + "' ";
            //int tamtCHEQUE1 = Convert.ToInt32(_Command.ExecuteScalar());

            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(tamtCHEQUE1); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            //_Command.CommandText = "select ifnull(sum(a.amount_paid),0)  from collect_component_detail a where   a.MODE= 'CASH' and  paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + _PaidDate.ToString("yyyy-MM-dd") + "' ";
            //int tamtcash1 = Convert.ToInt32(_Command.ExecuteScalar());
            // Response.Write(tamtcash1);
            //Response.Write(_Command.CommandText);
            //Response.End();
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px");
            //objHtmlTableCell.InnerText = Convert.ToString(tamtcash1);
            objHtmlTableCell.Align = "center";
            objHtmlTableRow.Cells.Add(objHtmlTableCell);





            //_Command.CommandText = "select ifnull(sum(a.amount_paid),0)   from collect_component_detail a where paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + _PaidDate.ToString("yyyy-MM-dd") + "'";
            //int tamt1 = Convert.ToInt32(_Command.ExecuteScalar());

            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(tamt1); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);


            objHtmlTable.Rows.Add(objHtmlTableRow);

        }
        //Response.End();
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