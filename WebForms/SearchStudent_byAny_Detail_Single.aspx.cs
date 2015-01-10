using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using System.Configuration;
using System.Collections.Generic;


public partial class WebForms_SearchStudent_byAny_Detail_Single : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    DataTable _dtblStudentDetails = new DataTable();
    OdbcConnection _Connection = null; OdbcCommand _Command = null; int scrollno;
    String newselect; OdbcDataReader reader;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        _Connection = (OdbcConnection)Session["_Connection"];
        _Command = new OdbcCommand();
        _Command.Connection = _Connection;

        if (Page.IsPostBack == false)
        {
            grid();
            if (Convert.ToString(Session["admno"]) == "")
            {

            }
            else
            {
                TextBox1.Text = Convert.ToString(Session["admno"]);
                Button1_Click(Button1, null);

            }
        }
    }


    public void grid()
    {

        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select * from ign_student_master", _Connection));

        odbc.Fill(dt);

        //GridView1.DataSource = dt;
        //GridView1.DataBind();

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        var _btnview = (ImageButton)sender;
        var _row = (GridViewRow)_btnview.NamingContainer;
        var hfview = (HiddenField)_row.FindControl("hfview");
        var id = hfview.Value;

        string SQL = "CALL `spStudentMasterAllColumnsList`()";
        _Command.CommandText = SQL;
        List<string> _lsColumnsList = new List<string>();
        reader = _Command.ExecuteReader();
        while (reader.Read())
        {
            _lsColumnsList.Add(Convert.ToString(reader[0]));
        } reader.Close(); reader.Dispose();

        SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + id + "')";
        _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure; OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
        _dtAdapter.SelectCommand = _Command;
        _dtAdapter.Fill(_dtblStudentDetails);

        if (_dtblStudentDetails.Rows.Count > 0)
        {
            HtmlTable _htmlTable = new HtmlTable(); _htmlTable.Width = "100%"; HtmlTableRow _Row = null; HtmlTableCell _cell = null;

            int _cellCount = 1;
            _Row = new HtmlTableRow();
            foreach (string Column in _lsColumnsList)
            {
                string Val = "";
                _cell = new HtmlTableCell(); _cell.InnerText = Convert.ToString(Column).Replace("_", " "); _cell.Attributes.Add("style", "background-color: #ececec; font-weight: bold; font-family: Calibri; font-size: 14px; color: Black;"); _Row.Cells.Add(_cell); _htmlTable.Rows.Add(_Row);
                if (Column.Equals("BIRTH_DATE"))
                {
                    Val = (from x in _dtblStudentDetails.AsEnumerable() select x[Convert.ToString("DOB")].ToString()).First();

                }
                else if (Column.Equals("DATE_OF_ADMISSION"))
                {
                    Val = (from x in _dtblStudentDetails.AsEnumerable() select x[Convert.ToString("DOA")].ToString()).First();

                }
                else
                {
                    Val = (from x in _dtblStudentDetails.AsEnumerable() select x[Convert.ToString(Column)].ToString()).First();
                }
                _cell = new HtmlTableCell(); _cell.InnerText = Val; _cell.Attributes.Add("style", "background-color: #ececec; font-weight: bold; font-family: Calibri; font-size: 14px; color: Blue;"); _Row.Cells.Add(_cell); _htmlTable.Rows.Add(_Row);
                _cellCount += 2;
                if (_cellCount > 4)
                {
                    _cellCount = 1;
                    _htmlTable.Rows.Add(_Row); _Row = new HtmlTableRow();
                }
            } _htmlTable.Rows.Add(_Row);
            
            pnldetail.Controls.Add(_htmlTable);
            mpe.Show();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBox1.Text.Trim() == "")
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Enter Text To Search');", true);
        else
        {

            dt = new DataTable();
            DataTable dt1 = new DataTable();
            OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select a.STUDENT_REGISTRATION_NBR as AdmNo ,a.student_id,  concat(b.CLASS_NAME,' ',b.CLASS_SECTION) as CLASS,   concat(a.FIRST_NAME,' ',a.MIDDLE_NAME,' ',a.LAST_NAME)  as Name, a.FATHER_NAME,a.MOTHER_NAME, date_format( a.BIRTH_DATE,'%e-%M-%Y') as Birth_Date  from ign_student_master a,ign_class_master b where a.CLASS_CODE=b.CLASS_CODE", _Connection));

            odbc.Fill(dt);

            dt1 = myclass.searchDataTable(TextBox1.Text, dt);
            gvStudentDetails.DataSource = dt1;
            gvStudentDetails.DataBind();
            if (dt1.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('No Record Found');", true);
                gvStudentDetails.DataSource = dt1;
                gvStudentDetails.DataBind();
            }
        }
    }





}