using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_searchStudentByAdmissionNo : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader _dtReader = null;
    OdbcDataAdapter _dtAdapter = null; DataTable _dtblClasses = new DataTable(); DataTable _dtblStudentDetails = new DataTable();
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
        //string SQL = "CALL `spStudentMasterAllColumnsList`()";
        //_Command.CommandText = SQL;
        //List<string> _lsColumnsList = new List<string>();
        //_dtReader = _Command.ExecuteReader();
        //while (_dtReader.Read())
        //{
        //    _lsColumnsList.Add(Convert.ToString(_dtReader[0]));
        //} _dtReader.Close(); _dtReader.Dispose();

        //SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";
        //_Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure; _dtAdapter = new OdbcDataAdapter();
        //_dtAdapter.SelectCommand = _Command;
        //_dtAdapter.Fill(_dtblStudentDetails);

        //if (_dtblStudentDetails.Rows.Count > 0)
        //{
        //    HtmlTable _htmlTable = new HtmlTable(); _htmlTable.Width = "100%"; HtmlTableRow _Row = null; HtmlTableCell _cell = null;

        //    int _cellCount = 1;
        //    _Row = new HtmlTableRow();
        //    foreach (string Column in _lsColumnsList)
        //    {
        //        string Val = "";
        //        _cell = new HtmlTableCell(); _cell.InnerText = Convert.ToString(Column).Replace("_", " "); _cell.Attributes.Add("style", "background-color: #FFFF99; font-weight: bold; font-family: Calibri; font-size: 12px; color: Black;"); _Row.Cells.Add(_cell); _htmlTable.Rows.Add(_Row);
        //        if (Column.Equals("BIRTH_DATE"))
        //        {
        //            Val = (from x in _dtblStudentDetails.AsEnumerable() select x[Convert.ToString("DOB")].ToString()).First();

        //        }
        //        else if (Column.Equals("DATE_OF_ADMISSION"))
        //        {
        //            Val = (from x in _dtblStudentDetails.AsEnumerable() select x[Convert.ToString("DOA")].ToString()).First();

        //        }
        //        else
        //        {
        //            Val = (from x in _dtblStudentDetails.AsEnumerable() select x[Convert.ToString(Column)].ToString()).First();
        //        }
        //        _cell = new HtmlTableCell(); _cell.InnerText = Val; _cell.Attributes.Add("style", "background-color: #FFFF99; font-weight: bold; font-family: Calibri; font-size: 12px; color: Blue;"); _Row.Cells.Add(_cell); _htmlTable.Rows.Add(_Row);
        //        _cellCount += 2;
        //        if (_cellCount > 4)
        //        {
        //            _cellCount = 1;
        //            _htmlTable.Rows.Add(_Row); _Row = new HtmlTableRow();
        //        }
        //    } _htmlTable.Rows.Add(_Row);
        //    pnlStudentDetails.Controls.Add(_htmlTable);
        //}
    }
    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {
        string SQL = "CALL `spStudentMasterAllColumnsList`()";
        _Command.CommandText = SQL;
        List<string> _lsColumnsList = new List<string>();
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            _lsColumnsList.Add(Convert.ToString(_dtReader[0]));
        } _dtReader.Close(); _dtReader.Dispose();

        SQL = "select concat(a.FIRST_NAME,' ',a.MIDDLE_NAME,' ',a.LAST_NAME) as Name , Concat(b.CLASS_NAME,'-',b.CLASS_SECTION) as class , a.STUDENT_REGISTRATION_NBR  as Registration_Nbr, a.STUDENT_ROLL_NBR,date_format(a.DATE_OF_ADMISSION, '%d-%M-%y') as Admission_date, date_format(a.BIRTH_DATE,'%d-%M-%y') as Birth_Date ,a.FATHER_NAME as father ,a.MOTHER_NAME as Mother,a.NO_OF_COMMUNICATION,a.ADDRESS_LINE1   from ign_student_master a ,  ign_class_master b where a.CLASS_CODE = b.CLASS_CODE and a.STUDENT_REGISTRATION_NBR =  '" + txtAdmissionNo.Text.Trim() + "'";
        
        _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure; _dtAdapter = new OdbcDataAdapter();
        _dtAdapter.SelectCommand = _Command;
        DataSet obj_dataset = new DataSet();
        _dtAdapter.Fill(obj_dataset);
        DetailsView1.DataSource = obj_dataset;
        DetailsView1.DataBind();
    }
}
        //if (_dtblStudentDetails.Rows.Count > 0)
        //{
        //    HtmlTable _htmlTable = new HtmlTable(); _htmlTable.Width = "100%"; HtmlTableRow _Row = null; HtmlTableCell _cell = null;

        //    int _cellCount = 1;
        //    _Row = new HtmlTableRow();
        //    foreach (string Column in _lsColumnsList)
        //    {
        //        string Val = "";
        //        _cell = new HtmlTableCell(); _cell.InnerText = Convert.ToString(Column).Replace("_", " "); _cell.Attributes.Add("style", "background-color: #FFFF99; font-weight: bold; font-family: Calibri; font-size: 12px; color: Black;"); _Row.Cells.Add(_cell); _htmlTable.Rows.Add(_Row);
        //        if (Column.Equals("BIRTH_DATE"))
        //        {
        //            Val = (from x in _dtblStudentDetails.AsEnumerable() select x[Convert.ToString("DOB")].ToString()).First();

        //        }
        //        else if (Column.Equals("DATE_OF_ADMISSION"))
        //        {
        //            Val = (from x in _dtblStudentDetails.AsEnumerable() select x[Convert.ToString("DOA")].ToString()).First();

        //        }
        //        else
        //        {
        //            Val = (from x in _dtblStudentDetails.AsEnumerable() select x[Convert.ToString(Column)].ToString()).First();
        //        }
        //        _cell = new HtmlTableCell(); _cell.InnerText = Val; _cell.Attributes.Add("style", "background-color: #FFFF99; font-weight: bold; font-family: Calibri; font-size: 12px; color: Blue;"); _Row.Cells.Add(_cell); _htmlTable.Rows.Add(_Row);
        //        _cellCount += 2;
        //        if (_cellCount > 4)
        //        {
        //            _cellCount = 1;
        //            _htmlTable.Rows.Add(_Row); _Row = new HtmlTableRow();
        //        }
        //    } _htmlTable.Rows.Add(_Row);
        //    pnlStudentDetails.Controls.Add(_htmlTable);
        