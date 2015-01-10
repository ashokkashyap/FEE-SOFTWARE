using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Configuration;

public partial class WebForms_searchStudentByName : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListName(string prefixText, int count, string contextKey)
    {
        OdbcDataAdapter odap = new OdbcDataAdapter("select CONCAT(IFNULL(first_name,''),' ',IFNULL(MIDDLE_NAME,''),' ',IFNULL(LAST_NAME,'')) from ign_student_master WHERE FIRST_NAME LIKE '" + prefixText.ToUpper() + "%' OR MIDDLE_NAME LIKE '" + prefixText.ToUpper() + "%' OR LAST_NAME LIKE '" + prefixText.ToUpper() + "%'", ConfigurationManager.ConnectionStrings["DBCONNECT"].ConnectionString);
        DataSet ds = new DataSet();
        odap.Fill(ds);
        ArrayList arr = new ArrayList();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arr.Add(dr[0]);
        }
        string[] Items = arr.ToArray(typeof(string)) as string[];
        var s = from k in Items where k.StartsWith(prefixText.ToUpper()) select k;
        return s.ToArray();
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        var SQL = "select STUDENT_REGISTRATION_NBR from ign_student_master where CONCAT(IFNULL(FIRST_NAME,''),' ',IFNULL(MIDDLE_NAME,''),' ',IFNULL(LAST_NAME,'')) = '" + txtName.Text.Trim() + "'";
        _Command.CommandText = SQL;
        var RegNo = Convert.ToString(_Command.ExecuteScalar());
        Session["student_reg"] = RegNo;

        SQL = "call spStudentDetailsfromAdmissionNo('" + RegNo + "')";
        var _dtAdapter = new OdbcDataAdapter();
        _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
        var _dtblRecords = new DataTable();
        _dtAdapter.Fill(_dtblRecords);
        gvStudentDetails.DataSource = _dtblRecords; gvStudentDetails.DataBind();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        var _btnEdit = (ImageButton)sender;
        var _row = (GridViewRow)_btnEdit.NamingContainer;
        var hfStudentID = (HiddenField)_row.FindControl("hfStudentID");
        //Response.Write(hfStudentID.Value); Response.End();
        Session["_STUDENT_ID"] = Convert.ToString(hfStudentID.Value);
        Response.Redirect("updateStudentDetails.aspx");
    }
}