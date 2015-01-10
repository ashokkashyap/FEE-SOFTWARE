using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_collectFeeNew : System.Web.UI.Page
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
            { txtDate.Text = DateTime.Now.ToString("dd-MMMM-yyyy"); }
        }
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        var SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";
        _Command.CommandText = SQL;
        using (var _dtReader = _Command.ExecuteReader())
        {
            while (_dtReader.Read())
            {
                lblStudentID.Text = Convert.ToString(_dtReader["STUDENT_ID"]);
                lblName.Text = Convert.ToString(_dtReader["NAME"]);
                lblClass.Text = Convert.ToString(_dtReader["CLASS"]);
                lblFatherName.Text = Convert.ToString(_dtReader["FATHER_NAME"]);
                lblMotherName.Text = Convert.ToString(_dtReader["MOTHER_NAME"]);
                lblAdmissionNo.Text = Convert.ToString(_dtReader["STUDENT_REGISTRATION_NBR"]);
                lblAddress.Text = Convert.ToString(_dtReader["ADDRESS_LINE1"]);
                lblNoOfCommunication.Text = Convert.ToString(_dtReader["NO_OF_COMMUNICATION"]);
            } _dtReader.Close();
        }
        SQL = "CALL `spComponentMaster`()"; _Command.CommandText = SQL;
        OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
        _dtAdapter.SelectCommand = _Command;
        DataTable _dtblComponents = new DataTable();
        _dtAdapter.Fill(_dtblComponents);
        gvFeeAmountDetails.DataSource = _dtblComponents; gvFeeAmountDetails.DataBind();
        btnVerify.Visible = true;
        btnReset.Visible = true;
        btnSubmit.Visible = true;
        //txtAdmissionNo.Enabled = false;
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        int VarTotal = 0;
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
            if (Convert.ToString(txtAmount.Text).Trim().Length > 0)
            {
                try
                {
                    VarTotal += Convert.ToInt32(txtAmount.Text);
                }
                catch { continue; }
            }
            else { VarTotal += 0; }
        }
        lblTotalAmount.Text = "Total Amount: " + VarTotal.ToString();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
            txtAmount.Text = ""; lblTotalAmount.Text = "";
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string sQL = "CALL `spGetMaxUniqueNoFromComponentMasterNew`()";
        _Command.CommandText = sQL; _Command.CommandType = CommandType.StoredProcedure;
        int varUniqueNO = Convert.ToInt32(_Command.ExecuteScalar()) + 1;

        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
            string varCOMPONENT_ID = ((HiddenField)_row.FindControl("hfCOMPONENT_ID")).Value;

            sQL = "insert into collect_component_master_new(STUDENT_ID,COMPONENT_ID,AMOUNT,COLLECTION_DATE,UNIQUE_NO,CREATE_DATE,CREATE_TIME,CREATE_BY) values(?,?,?,?,?,now(),now(),?)";
            _Command.CommandText = sQL;
            _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
            _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(varCOMPONENT_ID));
            if (Convert.ToString(txtAmount.Text).Trim().Length > 0)
            {
                _Command.Parameters.AddWithValue("AMOUNT", Convert.ToString(txtAmount.Text));
            }
            else
            { _Command.Parameters.AddWithValue("AMOUNT", Convert.ToString("0")); }
            _Command.Parameters.AddWithValue("COLLECTION_DATE", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
            _Command.Parameters.AddWithValue("UNIQUE_NO", Convert.ToString(varUniqueNO));
            _Command.Parameters.AddWithValue("CREATE_BY", Convert.ToString(Session["_User"]));
            _Command.ExecuteNonQuery(); _Command.Parameters.Clear();

        }
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page),"Script","alert('Fee Record Saved !!!'); window.location.href='collectFeeNew.aspx';",true);
    }
}