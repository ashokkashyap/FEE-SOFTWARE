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
using System.Collections;

public partial class WebForms_StudentLedgerExl : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; string Student_id = ""; OdbcCommand Command = null;
    double totalamt = 0.0; string cash;
    double tamtwidfine = 0.0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            Command = new OdbcCommand();
            Command.Connection = _Connection;
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



        OdbcDataReader _dtReader = null; OdbcDataReader dtReader = null; DataTable _dtblCollectionRecord = new DataTable();
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
        _Command.CommandText = "select Distinct(a.MAPPED_DATE)  from collect_component_master a where a.AMOUNT_PAYBLE > 0 and a.AMOUNT_PAID>0   order by MAPPED_DATE;";
        //Response.Write(_Command.CommandText); Response.End();
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            lsPaidDates.Add(Convert.ToDateTime(_dtReader[0]));
        } _dtReader.Close(); _dtReader.Dispose();

        HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1; objHtmlTable.BorderColor = "#FFAB60"; objHtmlTable.Attributes.Add("style", "font-family:Arial");
        HtmlTableRow objHtmlTableRow = null;
        HtmlTableRow objHtmlTableRow2 = null;
        HtmlTableRow objHtmlTableRow3 = null;
        HtmlTableRow objHtmlTableRow4 = null;
        HtmlTableRow objHtmlTableRow5 = null;
        HtmlTableRow objHtmlTableRow6 = null;
        HtmlTableRow objHtmlTableRow7 = null;
        HtmlTableRow objHtmlTableRow71 = null;

        HtmlTableRow objHtmlTableRow8 = null;
        HtmlTableRow objHtmlTableRow9 = null;
        HtmlTableRow objHtmlTableRow10 = null;

        //HtmlTableRow objHtmlTableRow7 = null; HtmlTableRow objHtmlTableRow8 = null;

        HtmlTableCell objHtmlTableCell = null;
        

        //Dictionary<int, int> dicScrollNumberAndStudentID = new Dictionary<int, int>();
        List<StudentBasicDetails> lsStudentDetails = new List<StudentBasicDetails>();
        List<CollectionOtherDetails> lsCollectionOtherDetails = new List<CollectionOtherDetails>();
        //foreach (DateTime _PaidDate in lsPaidDates)
        //{
        //    Response.Write(_PaidDate.ToString());
        //} Response.End();
        int flag = 1;
        int srno = 1;
        foreach (DateTime _PaidDate in lsPaidDates)
        {
           // objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.ColSpan = objDictionaryComponentList.Count + 10; objHtmlTableCell.Align = "left"; objHtmlTableCell.Attributes.Add("style", "color:Blue; font-weight:bold; font-size:12px;"); objHtmlTableCell.InnerText = _PaidDate.ToString("dd-MMM-yyyy"); objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
           
            if (flag == 1)
            {


                #region HeaderRow

                
               

                objHtmlTableRow7 = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "HAPPY HOME PUBLIC SCHOOL"; objHtmlTableCell.ColSpan = 17; objHtmlTableCell.Align = "center"; objHtmlTableRow7.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow7);

                objHtmlTableRow8 = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "B4 sector 11 Rohini"; objHtmlTableCell.ColSpan = 17; objHtmlTableCell.Align = "center"; objHtmlTableRow8.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow8);


                objHtmlTableRow9 = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "STUDENT LEDGER"; objHtmlTableCell.ColSpan = 17; objHtmlTableCell.Align = "center"; objHtmlTableRow9.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow9);

                string sQL1 = "";
                sQL1 = "CALL `spStudentBasicDetailsFromStudentID`('" + Student_id + "')";
                _Command.CommandText = sQL1; _Command.CommandType = CommandType.StoredProcedure;
                _dtReader = _Command.ExecuteReader(); lsStudentDetails.Clear();
                while (_dtReader.Read())
                {


                    string objStudentBasicDetailsname = Convert.ToString(_dtReader["NAME"]);

                    string objStudentBasicDetailsCLASS = Convert.ToString(_dtReader["CLASS"]);

                    string objStudentBasicDetailsAdmission_No = Convert.ToString(_dtReader["Admission_No"]);

                    string objStudentBasicDetailsBIRTH_DATE = Convert.ToString(_dtReader["BIRTH_DATE"]);
                    string phone = Convert.ToString(_dtReader["NO_OF_COMMUNICATION"]);
                    string state = Convert.ToString(_dtReader["STATE"]);
                    string CITY = Convert.ToString(_dtReader["CITY"]);
                    string COUNTRY = Convert.ToString(_dtReader["COUNTRY"]);
                    string ADDRESS_LINE1 = Convert.ToString(_dtReader["ADDRESS_LINE1"]);
                    string MOTHER_NAME = Convert.ToString(_dtReader["MOTHER_NAME"]);
                    string FATHER_NAME = Convert.ToString(_dtReader["FATHER_NAME"]);
                     



                    objHtmlTableRow6 = new HtmlTableRow();
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "NAME"; objHtmlTableCell.ColSpan = 2; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow6.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString(objStudentBasicDetailsname); objHtmlTableCell.ColSpan = 4; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow6.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CLASS/SEC"; objHtmlTableCell.ColSpan = 2; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow6.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = objStudentBasicDetailsCLASS; objHtmlTableCell.ColSpan = 2; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow6.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ADDMISSION NO. "; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow6.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = objStudentBasicDetailsAdmission_No; objHtmlTableCell.ColSpan = 4; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow6.Cells.Add(objHtmlTableCell);
                   
                    objHtmlTable.Rows.Add(objHtmlTableRow6);

                    objHtmlTableRow5 = new HtmlTableRow();
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "FATHER NAME"; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow5.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = FATHER_NAME; objHtmlTableCell.ColSpan = 6; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow5.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "MOTHER NAME"; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow5.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = MOTHER_NAME; objHtmlTableCell.ColSpan = 5; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow5.Cells.Add(objHtmlTableCell);

                    objHtmlTable.Rows.Add(objHtmlTableRow5);


                    objHtmlTableRow4 = new HtmlTableRow();
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "DATE OF BIRTH"; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow4.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = objStudentBasicDetailsBIRTH_DATE; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow4.Cells.Add(objHtmlTableCell);

                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ADDRESH"; objHtmlTableCell.ColSpan = 2; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow4.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = ADDRESS_LINE1; objHtmlTableCell.ColSpan = 9; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow4.Cells.Add(objHtmlTableCell);

                    objHtmlTable.Rows.Add(objHtmlTableRow4);












                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "RATE & MONTHLY DUES."; objHtmlTableCell.ColSpan = 4; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow3.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.ColSpan = 4; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow3.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "TELEPHONE NO."; objHtmlTableCell.ColSpan = 4; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow3.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = phone; objHtmlTableCell.ColSpan = 5; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow3.Cells.Add(objHtmlTableCell);

                    objHtmlTable.Rows.Add(objHtmlTableRow3);








                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Bus No."; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow2.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow2.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Bus Stop"; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow2.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow2.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Fee"; objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow2.Cells.Add(objHtmlTableCell);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.ColSpan = 2; objHtmlTableCell.Align = "LEFT"; objHtmlTableRow2.Cells.Add(objHtmlTableCell);



                    objHtmlTable.Rows.Add(objHtmlTableRow2);
                }
                _dtReader.Close(); _dtReader.Dispose();


                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "SNO"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Date"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "NAME"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Class & Sec"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "MONTH"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "ADM NO"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Date of Birth"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

                foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
                {
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = kvp.Value.ToUpper(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                }
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Cash"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Cheque"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Total"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CHEQUE"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "CASH"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "GRAND TOTAL"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Cheque"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = "Cash"; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);
                #endregion
            }
            flag = flag + 1;
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
                objStudentBasicDetails.dateofbirth = Convert.ToString(_dtReader["BIRTH_DATE"]);
                lsStudentDetails.Add(objStudentBasicDetails);
            } _dtReader.Close(); _dtReader.Dispose();
           

    
            foreach (StudentBasicDetails _StudentBasicDetails in lsStudentDetails)
            {
               
              
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px;"); objHtmlTableCell.NoWrap = true; objHtmlTableCell.InnerText = srno.ToString(); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px;"); objHtmlTableCell.NoWrap = true; objHtmlTableCell.InnerText = _PaidDate.ToString("dd-MMM-yyyy"); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
               

                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px;"); objHtmlTableCell.NoWrap = true; objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.Name); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.Class); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString((new System.Globalization.DateTimeFormatInfo()).GetMonthName(_PaidDate.Month)); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.AdmissionNumber); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_StudentBasicDetails.dateofbirth); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                
                srno = srno + 1;
                  _dtReader.Close();
            }

            double VarAmountPaid = 0.0;
            foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
            {
                var Amount = from a in _dtblCollectionRecord.AsEnumerable() where Convert.ToInt32(a["STUDENT_ID"]).Equals(Convert.ToInt32(Student_id)) && Convert.ToInt32(a["COMPONENT_ID"]).Equals(Convert.ToInt32(kvp.Key)) && Convert.ToDateTime(a["MAPPED_DATE"]).Equals(Convert.ToDateTime(_PaidDate)) select a["AMOUNT_PAYBLE"];
                if (Amount.Any())
                {
                    foreach (var _Amount in Amount)
                    {
                        if (Convert.ToInt32(_Amount) >0 )
                        {
                            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(_Amount); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
                            VarAmountPaid += Convert.ToDouble(_Amount);
                        }
                        else
                        { objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(""); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); }


                    }
                    
                }
                else
                { objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(""); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); }
            }
           

            
           

            string dt = Convert.ToString((new System.Globalization.DateTimeFormatInfo()).GetMonthName(_PaidDate.Month));
            sQL = "select sum(amount_paid) as amount_paid  from collect_component_detail where student_id='" + Student_id + "' and date_format(paid_date,'%M')='" + dt + "' and mode='CASH'";
            Command.CommandText = sQL;
            dtReader = Command.ExecuteReader();
            if (dtReader.HasRows)
            {
                while (dtReader.Read())
                {
                    cash = Convert.ToString(dtReader["amount_paid"]);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = cash; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

                }
            }
            else
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

            }
           
            
            
            dtReader.Close();

            

            sQL = "select amount_paid from collect_component_detail where student_id='" + Student_id + "' and date_format(paid_date,'%M')='" + dt + "' and mode='CHEQUE'";
            Command.CommandText = sQL;
            dtReader = Command.ExecuteReader();
            if (dtReader.HasRows)
            {
                while (dtReader.Read())
                {
                    cash = Convert.ToString(dtReader["amount_paid"]);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = cash; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

                }
            }
             else
                { 
                
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(""); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            }
           

            dtReader.Close();



            //sQL = "select sum(a.AMOUNT_PAYBLE) as amount ,a.MAPPED_DATE,a.STUDENT_ID  from collect_component_master a where a.STUDENT_ID='"+Student_id+"'  group by a.STUDENT_ID,a.MAPPED_DATE;";
            sQL = "select sum(c.AMOUNT_PAYBLE) as amount from collect_component_master c where c.STUDENT_ID='" + Student_id + "' and c.MAPPED_DATE='" + _PaidDate.ToString("yyyy-MM-dd")+"'";
            Command.CommandText = sQL;
            dtReader = Command.ExecuteReader();
            if (dtReader.HasRows)
            {
                while (dtReader.Read())
                {

                    cash = Convert.ToString(dtReader["amount"]);
                    objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = cash; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);

                    //objHtmlTable.Rows.Add(objHtmlTableRow);

                }

            }
            else
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
            }

           

            objHtmlTable.Rows.Add(objHtmlTableRow);
            dtReader.Close();







            //_Command.CommandText = "select ifnull(sum(a.amount_paid),0)   from collect_component_detail a where paid_date between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + _PaidDate.ToString("yyyy-MM-dd") + "'";
            //int tamt1 = Convert.ToInt32(_Command.ExecuteScalar());

            //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:10px"); objHtmlTableCell.InnerText = Convert.ToString(tamt1); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell);


          

        }
        //Response.End();
         objHtmlTableRow71 = new HtmlTableRow();
         objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow71.Cells.Add(objHtmlTableCell);
         objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = ""; objHtmlTableCell.Align = "center"; objHtmlTableRow71.Cells.Add(objHtmlTableCell);
         objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText ="G Total"; objHtmlTableCell.Align = "center"; objHtmlTableRow71.Cells.Add(objHtmlTableCell);

        foreach (KeyValuePair<int, string> kvp in objDictionaryComponentList)
        {
            string sql = "";
            sql = "select sum(a.AMOUNT_PAYBLE) as sum from collect_component_master a where a.STUDENT_ID='" + Student_id + "' and component_id='"+Convert.ToString(kvp.Key)+"'";
            _Command.CommandText = sql;
            _dtReader = _Command.ExecuteReader();
            if (_dtReader.Read())
            {
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["sum"]); objHtmlTableCell.Align = "center"; objHtmlTableRow71.Cells.Add(objHtmlTableCell);
            objHtmlTable.Rows.Add(objHtmlTableRow71);
            }
            _dtReader.Close();
            
           
        }
        string ssql = "";
        ssql = "select sum(amount_paid) as paid from collect_component_detail where student_id='" + Student_id + "' and mode='cash'";
        _Command.CommandText = ssql;
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["paid"]); objHtmlTableCell.Align = "center"; objHtmlTableRow71.Cells.Add(objHtmlTableCell);

        }
        _dtReader.Close();

        ssql = "select sum(amount_paid) as paid from collect_component_detail where student_id='" + Student_id + "' and mode='cheque'";
        _Command.CommandText = ssql;
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["paid"]); objHtmlTableCell.Align = "center"; objHtmlTableRow71.Cells.Add(objHtmlTableCell);

        }
        _dtReader.Close();
       
       
        ssql = "select sum(amount_paid) as paid from collect_component_detail where student_id='" + Student_id + "'";
        _Command.CommandText = ssql;
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString(_dtReader["paid"]); objHtmlTableCell.Align = "center"; objHtmlTableRow71.Cells.Add(objHtmlTableCell);

        }
        _dtReader.Close();
       
           
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
        public string dateofbirth { get; set;  }
    }
    
}