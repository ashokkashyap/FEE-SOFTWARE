using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_updateCollectedFeeAdmissionNo : System.Web.UI.Page
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
            {  }
        }
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        var _dtblRecords = new DataTable();
        var SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";
        _Command.CommandText = SQL;
        using (var _dtReader = _Command.ExecuteReader())
        {
            if (_dtReader.HasRows)
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
                }
            }
            else
            {
                lblStudentID.Text = Convert.ToString("");
                lblName.Text = Convert.ToString("");
                lblClass.Text = Convert.ToString("");
                lblFatherName.Text = Convert.ToString("");
                lblMotherName.Text = Convert.ToString("");
                lblAdmissionNo.Text = Convert.ToString("");
                lblAddress.Text = Convert.ToString("");
            }
        }
        _Command.CommandText = "CALL `spPaymentDatesFromStudentID`('" + Convert.ToString(lblStudentID.Text) + "')";
        _Command.CommandType = CommandType.StoredProcedure;
        ddlSelectPaymentDate.Items.Clear(); ddlSelectPaymentDate.Items.Add(new ListItem("select", "select"));
        using (var _dtReader = _Command.ExecuteReader())
        {
            if (_dtReader.HasRows)
            {
                while (_dtReader.Read())
                {
                    ddlSelectPaymentDate.Items.Add(new ListItem(Convert.ToDateTime(_dtReader[0]).ToString("dd-MMM-yyyy"), Convert.ToDateTime(_dtReader[0]).ToString("dd-MMM-yyyy")));
                }
            }
            else
            {
                ddlSelectPaymentDate.Items.Clear(); ddlSelectPaymentDate.Items.Add(new ListItem("select", "select"));
            }
        }
        gvFeeAmountDetails.DataSource = null; gvFeeAmountDetails.DataBind(); btnSubmit.Visible = false;
    }
    protected void ddlSelectPaymentDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        if (ddlSelectPaymentDate.SelectedIndex > 0)
        {
            string varRandomNumber = "";
            DataTable _dtblFeeDetails = new DataTable();
            _dtblFeeDetails.Columns.Add("COMPONENT_NAME");
            _dtblFeeDetails.Columns.Add("ID");
            _dtblFeeDetails.Columns.Add("COMPONENT_DETAIL_ID");
            _dtblFeeDetails.Columns.Add("DISCOUNT");
            _dtblFeeDetails.Columns.Add("AMOUNT_PAID");
            _dtblFeeDetails.Columns.Add("COMPONENT_AMOUNT");
            
            _Command.CommandText = "CALL `spPaymentAllDetailsFromStudentIDAndPayDate`('" + Convert.ToString(lblStudentID.Text) + "', '" + Convert.ToDateTime(ddlSelectPaymentDate.SelectedValue).ToString("yyyy-MM-dd") + "')";
            _Command.CommandType = CommandType.StoredProcedure;
            using (var _dtReader = _Command.ExecuteReader())
            {
                while (_dtReader.Read())
                {
                    DataRow _row = _dtblFeeDetails.NewRow();
                    varRandomNumber = Convert.ToString(_dtReader["RAND_NUM"]);
                    _row["COMPONENT_AMOUNT"] = Convert.ToString(_dtReader["AMOUNT_PAYBLE"]);
                    _row["AMOUNT_PAID"] = Convert.ToString(_dtReader["AMOUNT_PAID"]);
                    _row["DISCOUNT"] = Convert.ToString(_dtReader["DISCOUNT"]);
                    _row["COMPONENT_NAME"] = Convert.ToString(_dtReader["COMPONENT_NAME"]);
                    _row["ID"] = Convert.ToString(_dtReader["ID"]);
                    _dtblFeeDetails.Rows.Add(_row);
                }
            }

            _Command.CommandText = "CALL `spCollectComponentDetailFromStudentIDAndRandomNo`('" + Convert.ToString(lblStudentID.Text) + "', '" + Convert.ToString(varRandomNumber) + "')";
            _Command.CommandType = CommandType.StoredProcedure;
            using (var _dtReader = _Command.ExecuteReader())
            {
                if (_dtReader.HasRows)
                {
                    while (_dtReader.Read())
                    {
                        ViewState["vwDetailID"] = Convert.ToString(_dtReader["ID"]);
                        txtFineAmount.Text = Convert.ToString(_dtReader["FINE"]);
                        txtFineDetails.Text = Convert.ToString(_dtReader["FINE_DETAIL"]);
                        txtDiscountAmount.Text = Convert.ToString(_dtReader["DISCOUNT"]);
                        txtDiscountDetails.Text = Convert.ToString(_dtReader["DISCOUNT_DETAIL"]);
                        txtChequeNo.Text = Convert.ToString(_dtReader["CHECK_NUMBER"]);
                        if (Convert.ToString(_dtReader["CHECK_DATE"]).Trim().Length > 0)
                        {
                            txtChequeDate.Text = Convert.ToDateTime(_dtReader["CHECK_DATE"]).ToString("dd-MMM-yyyy");
                        }
                        txtBankDetails.Text = Convert.ToString(_dtReader["BANK_NAME"]);
                        ddlSelectPaymentMode.SelectedValue = Convert.ToString(_dtReader["TYPE"]);
                    }
                }
                else
                {
                    ViewState["vwDetailID"] = "-1";
                    txtFineAmount.Text = Convert.ToString("");
                    txtFineDetails.Text = Convert.ToString("");
                    txtDiscountAmount.Text = Convert.ToString("");
                    txtDiscountDetails.Text = Convert.ToString("");
                    txtChequeNo.Text = Convert.ToString("");
                    txtChequeDate.Text = Convert.ToString("");
                    txtBankDetails.Text = Convert.ToString("");
                    ddlSelectPaymentMode.SelectedIndex = 0;
                }
            }
            gvFeeAmountDetails.DataSource = _dtblFeeDetails; gvFeeAmountDetails.DataBind(); btnSubmit.Visible = true; lblMessage.Visible = false;
        }
        else
        {
            gvFeeAmountDetails.DataSource = null; gvFeeAmountDetails.DataBind(); btnSubmit.Visible = false;
            txtFineAmount.Text = Convert.ToString("");
            txtFineDetails.Text = Convert.ToString("");
            txtDiscountAmount.Text = Convert.ToString("");
            txtDiscountDetails.Text = Convert.ToString("");
            txtChequeNo.Text = Convert.ToString("");
            txtChequeDate.Text = Convert.ToString("");
            txtBankDetails.Text = Convert.ToString("");
            ddlSelectPaymentMode.SelectedIndex = 0; lblMessage.Visible = false;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            HiddenField hfID = (HiddenField)_row.FindControl("hfID");
            TextBox txtPayment = (TextBox)_row.FindControl("txtPayment");

            string varAmountPaid = Convert.ToString(txtPayment.Text);
            _Command.Parameters.AddWithValue("AMOUNT_PAID", varAmountPaid);
            _Command.Parameters.AddWithValue("ID", Convert.ToString(hfID.Value));
            _Command.CommandText = "update collect_component_master set AMOUNT_PAID=? where ID=?";
            _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
        }

        string VarPaymentMode = Convert.ToString(ddlSelectPaymentMode.SelectedValue);

        _Command.CommandText = "update collect_component_detail set TYPE=?,CHECK_NUMBER=?,CHECK_DATE=?,BANK_NAME=?,FINE=?,FINE_DETAIL=?,DISCOUNT=?,DISCOUNT_DETAIL=? where ID=?";
        _Command.Parameters.AddWithValue("TYPE", VarPaymentMode);
        _Command.Parameters.AddWithValue("CHECK_NUMBER", Convert.ToString(txtChequeNo.Text));
        if (Convert.ToString(txtChequeDate.Text).Trim().Length > 0)
        {
            _Command.Parameters.AddWithValue("CHECK_DATE", Convert.ToDateTime(txtChequeDate.Text).ToString("yyyy-MM-dd"));
        }
        else { _Command.Parameters.AddWithValue("CHECK_DATE", Convert.ToString(DBNull.Value)); }
        _Command.Parameters.AddWithValue("BANK_NAME", Convert.ToString(txtBankDetails.Text));
        _Command.Parameters.AddWithValue("FINE", Convert.ToString(txtFineAmount.Text));
        _Command.Parameters.AddWithValue("FINE_DETAIL", Convert.ToString(txtFineDetails.Text));
        _Command.Parameters.AddWithValue("DISCOUNT", Convert.ToString(txtDiscountAmount.Text));
        _Command.Parameters.AddWithValue("DISCOUNT_DETAIL", Convert.ToString(txtDiscountDetails.Text));
        _Command.Parameters.AddWithValue("ID", Convert.ToString(ViewState["vwDetailID"]));
        _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
        lblMessage.Visible = true;
        //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Record Updated !!!'); window.location.href='UpdateCollectedFeeAdmissionNo.aspx';", true);
    }
}