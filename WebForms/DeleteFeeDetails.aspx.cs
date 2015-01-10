using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_DeleteFeeDetails : System.Web.UI.Page
{

    // OdbcConnection _Connection = null; OdbcCommand _Command = null;
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader _dtReader = null;
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

    protected void Btnsubmit_Click(object sender, EventArgs e)
    {

        DataTable _dtblRecords = new DataTable();
        string SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";
        _Command.CommandText = SQL; _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            lblStudentID.Text = Convert.ToString(_dtReader["STUDENT_ID"]);
            lblName.Text = Convert.ToString(_dtReader["NAME"]);
            lblClass.Text = Convert.ToString(_dtReader["CLASS"]);
            lblFatherName.Text = Convert.ToString(_dtReader["FATHER_NAME"]);
            lblMotherName.Text = Convert.ToString(_dtReader["MOTHER_NAME"]);
            //lblAdmissionNo.Text = Convert.ToString(_dtReader["STUDENT_REGISTRATION_NBR"]);
            //lblAddress.Text = Convert.ToString(_dtReader["ADDRESS_LINE1"]);
        } _dtReader.Close(); _dtReader.Dispose();

        SQL = "select a.student_id, a.SCROLL_NO, monthname(a.MAPPED_DATE) as mname, sum(a.AMOUNT_PAID) AS AMT, a.PAID_DATE from collect_component_master a where a.STUDENT_ID= '" + lblStudentID.Text + "' and a.AMOUNT_PAID <> 0 group by a.SCROLL_NO";
        //txtAdmissionNo.Text = SQL;
        _Command.CommandText = SQL; _dtReader = _Command.ExecuteReader();
        _dtblRecords.Load(_dtReader);
        gvRecords.DataSource = _dtblRecords; gvRecords.DataBind();



    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        // if (chkbox.Checked)
        {

            _Command.CommandText = "delete from collect_component_master where student_id = '" + lblStudentID.Text + "'";
            _Command.ExecuteNonQuery();

            _Command.CommandText = "delete from collect_component_detail where student_id = '" + lblStudentID.Text + "'";
            _Command.ExecuteNonQuery();
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Fee Record Deleted !!!'); window.location.href='DeleteFeeDetails.aspx';", true);
        }
    }
}
