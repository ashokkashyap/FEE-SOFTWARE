using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Configuration;

public partial class WebForms_searchStudentByNameClassWise : System.Web.UI.Page
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
                DataTable _dtblClasses = new DataTable();
                OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
                string SQL = "call spClassMaster()";
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblClasses);
                ViewState["_dtblClasses"] = _dtblClasses;
                ddlSelectClass.DataSource = _dtblClasses; ddlSelectClass.DataTextField = "CLS"; ddlSelectClass.DataValueField = "CLASS_CODE"; ddlSelectClass.DataBind(); ddlSelectClass.Items.Insert(0, new ListItem("Select Class", ""));
            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }
    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListName(string prefixText, int count, string contextKey)
    //{
    //    OdbcDataAdapter odap = new OdbcDataAdapter("select CONCAT(IFNULL(first_name,''),' ',IFNULL(MIDDLE_NAME,''),' ',IFNULL(LAST_NAME,'')) from ign_student_master WHERE CLASS_CODE='" + prefixText.ToUpper() + "' and FIRST_NAME LIKE '" + prefixText.ToUpper() + "%' OR MIDDLE_NAME LIKE '" + prefixText.ToUpper() + "%' OR LAST_NAME LIKE '" + prefixText.ToUpper() + "%'", ConfigurationManager.ConnectionStrings["DBCONNECT"].ConnectionString);
    //    DataSet ds = new DataSet();
    //    odap.Fill(ds);
    //    ArrayList arr = new ArrayList();
    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        arr.Add(dr[0]);
    //    }
    //    string[] Items = arr.ToArray(typeof(string)) as string[];
    //    var s = from k in Items where k.StartsWith(prefixText.ToUpper()) select k;
    //    return s.ToArray();
    //}
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        //ArrayList arlStudentID = new ArrayList(); 
        string StudentIDString = "";
        string SQL = "select STUDENT_ID from ign_student_master where CLASS_CODE ='" + Convert.ToString(ddlSelectClass.SelectedValue) + "';";
        _Command.CommandText = SQL;
        OdbcDataReader _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            //arlStudentID.Add(Convert.ToString(_dtReader["STUDENT_ID"])); 
            StudentIDString += Convert.ToString(_dtReader["STUDENT_ID"]) + ",";
        } _dtReader.Close(); _dtReader.Dispose(); StudentIDString = StudentIDString.Substring(0, StudentIDString.Length - 1);

        SQL = "select ism.*,icm.*,concat(ifnull(ism.FIRST_NAME,' '),' ',ifnull(ism.MIDDLE_NAME,' '),' ',ifnull(ism.LAST_NAME,' ')) as NAME,concat(ifnull(icm.CLASS_NAME,' '),' ',ifnull(icm.CLASS_SECTION,' ')) as CLASS,ism.NO_OF_COMMUNICATION from ign_student_master ism, ign_class_master icm where ism.CLASS_CODE=icm.CLASS_CODE and ism.STUDENT_ID in (" + StudentIDString + ") and ism.FIRST_NAME like '" + Convert.ToString(txtName.Text).Trim() + "%'";
        OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
        _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
        DataTable _dtblRecords = new DataTable();
        _dtAdapter.Fill(_dtblRecords);
        gvStudentDetails.DataSource = _dtblRecords; gvStudentDetails.DataBind();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ImageButton _btnEdit = (ImageButton)sender;
        GridViewRow _row = (GridViewRow)_btnEdit.NamingContainer;
        HiddenField hfStudentID = (HiddenField)_row.FindControl("hfStudentID");
        //Response.Write(hfStudentID.Value); Response.End();
        Session["_STUDENT_ID"] = Convert.ToString(hfStudentID.Value);
        Response.Redirect("updateStudentDetails.aspx");
    }
}