using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_UpdateFeemode : System.Web.UI.Page
{
    int id, stid;
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader _dtReader = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                pnlupdate.Visible = false;
                txtchkno.Enabled = false;
                txtchequedate.Enabled = false;
                txtbank.Enabled = false;
                txtchkno.Enabled = false;

            }
        }
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
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
        } _dtReader.Close(); _dtReader.Dispose();

        SQL = "CALL `spStudentPaidFeeDetailsFromStudentIDAndSessionID`('" + lblStudentID.Text.Trim() + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
        //txtAdmissionNo.Text = SQL;
        //Response.Write(SQL);
        //Response.End();
        _Command.CommandText = SQL; _dtReader = _Command.ExecuteReader();
        _dtblRecords.Load(_dtReader);
        gvRecords.DataSource = _dtblRecords; gvRecords.DataBind();

    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int st = Convert.ToInt32(Session["studentid"]);
        int idd = Convert.ToInt32(Session["detailid"]);



        if (ddlmode.SelectedItem.Text == "CASH")
        {
            _Command.CommandText = "update collect_component_detail set mode='" + ddlmode.SelectedValue + "' where id='" + idd + "' and student_id='" + st + "'";
            _Command.ExecuteNonQuery();
        }
        else
        {
            _Command.CommandText = "update collect_component_detail set mode='" + ddlmode.SelectedValue + "',CHEQUE_DATE='" + txtchequedate.Text + "',BANK_NAME='" + txtbank.Text + "',CHEQUE_NUMBER='" + txtchkno.Text + "' where id='" + idd + "' and student_id='" + st + "'";
            _Command.ExecuteNonQuery();
        }

    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {

        pnlupdate.Visible = true;
        var updte = (ImageButton)sender;
        var _row = (GridViewRow)updte.NamingContainer;
        var hfStudentID = (HiddenField)_row.FindControl("hfSTUDENT_ID");
        var hfid = (HiddenField)_row.FindControl("hfID");
        stid = Convert.ToInt32(hfStudentID.Value);
        id = Convert.ToInt32(hfid.Value);
        Session["detailid"] = id;
        Session["studentid"] = stid;

    }


    protected void ddlmode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlmode.SelectedItem.Text == "CHEQUE")
        {
            txtchkno.Enabled = true;
            txtchequedate.Enabled = true;
            txtbank.Enabled = true;
            txtchkno.Enabled = true;
        }

        else
        {
            txtchkno.Enabled = false;
            txtchequedate.Enabled = false;
            txtbank.Enabled = false;
            txtchkno.Enabled = false;
        }
    }


}
   
     
    
    
 