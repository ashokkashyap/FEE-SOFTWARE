using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_paidFeeSimple : System.Web.UI.Page
{
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
        _Command.CommandText = SQL; _dtReader = _Command.ExecuteReader();
        _dtblRecords.Load(_dtReader);
        gvRecords.DataSource = _dtblRecords; gvRecords.DataBind();

        gvFeeAmountDetails.DataSource = null; gvFeeAmountDetails.DataBind();
        txtFineAmount.Text = Convert.ToString("0");
        txtFeesDate.Text = "";
        pnlExtraDetails.Visible = false; pnlMaindetails.Visible = false;
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (gvRecords.Rows.Count > 0)
        {
            DataTable _dtblRecords = new DataTable();
            int lastRowIndex = gvRecords.Rows.Count - 1;
            DateTime _PaidDate = Convert.ToDateTime(gvRecords.Rows[lastRowIndex].Cells[1].Text);
            string SQL = "CALL `spPaidFeeDetailsFromStudentIDPaidDateAndSessionID`('" + lblStudentID.Text.Trim() + "','" + _PaidDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
            _Command.CommandText = SQL; _dtReader = _Command.ExecuteReader();
            _dtblRecords.Load(_dtReader);
            _dtblRecords.Columns.Add("PREVIOUS");
            foreach (DataRow _row in _dtblRecords.Rows)
            {
                _row["PREVIOUS"] = Convert.ToString(Convert.ToInt32(_row["Paid"]) - Convert.ToInt32(_row["AMOUNT_PAYBLE"]));
            }
            gvFeeAmountDetails.DataSource = _dtblRecords; gvFeeAmountDetails.DataBind();
            txtFineAmount.Text = Convert.ToString(gvRecords.Rows[lastRowIndex].Cells[4].Text);
            txtFeesDate.Text = Convert.ToDateTime(gvRecords.Rows[lastRowIndex].Cells[1].Text).ToString("dd-MMMM-yyyy");
            pnlExtraDetails.Visible = true; pnlMaindetails.Visible = true;

            DateTime varFeesCollectionDate = Convert.ToDateTime(txtFeesDate.Text);
            DateTime varFineStartingDate = Convert.ToDateTime(txtFeesDate.Text);

            SQL = "CALL `spFineStartingMonthFromStudentIDAndSessionID`('" + Convert.ToString(lblStudentID.Text) + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
            _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure;
            _dtReader = _Command.ExecuteReader();
            while (_dtReader.Read())
            {
                if (Convert.ToInt32(_dtReader["AMOUNT_PAID"]) == 0)
                {
                    varFineStartingDate = Convert.ToDateTime(_dtReader["MAPPED_DATE"]);
                    break;
                }
            } _dtReader.Close(); _dtReader.Dispose();
            PrintCalculation();
        }
        else
        { gvFeeAmountDetails.DataSource = null; gvFeeAmountDetails.DataBind(); pnlExtraDetails.Visible = false; pnlMaindetails.Visible = false; }
    }
    public void PrintCalculation()
    {
        double varAmountPayable = 0.0; double varPreviousDue = 0.0; double varDiscount = 0.0; double varADiscount = 0.0; double ToBePaid = 0.0; double TotalPayable = 0.0;
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            TextBox txtADiscount = (TextBox)_row.FindControl("txtADiscount");
            TextBox txtPayment = (TextBox)_row.FindControl("txtPayment");
            Label lblDiscount = (Label)_row.FindControl("lblDiscount");

            varAmountPayable += Convert.ToDouble(_row.Cells[2].Text);
            varPreviousDue += Convert.ToDouble(_row.Cells[3].Text);
            varDiscount += Convert.ToDouble(lblDiscount.Text);
            varADiscount += Convert.ToDouble(txtADiscount.Text);
            if (Convert.ToDouble(txtPayment.Text) > 0)
            {
                ToBePaid += Convert.ToDouble(txtPayment.Text);
            }
        } TotalPayable = varAmountPayable + varPreviousDue - varDiscount - varADiscount;
        btnVerify.Visible = true; btnReset.Visible = true; btnSubmit.Visible = true;
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    { PrintCalculation(); btnSubmit.Visible = true; }
    protected void btnReset_Click(object sender, EventArgs e)
    { Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "window.location.href='PaidFeeSimple.aspx';", true); }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int lastRowIndex = gvRecords.Rows.Count - 1;
        DateTime _PaidDate = Convert.ToDateTime(gvRecords.Rows[lastRowIndex].Cells[1].Text);
        string sQL = "update collect_component_master set amount_paid=0,scroll_no=null,scroll_month=null,detail_id=null,paid_date=null where student_id='" + lblStudentID.Text + "' and paid_date='" + _PaidDate.ToString("yyyy-MM-dd") + "' and school_session_id='" + Convert.ToString(Session["_SessionID"]) + "';";
        _Command.CommandText = sQL; _Command.ExecuteNonQuery();
        sQL = "delete from collect_component_master where student_id='" + lblStudentID.Text + "' and amount_payble=0 and amount_paid=0 and discount=0 and school_session_id='" + Convert.ToString(Session["_SessionID"]) + "';";
        _Command.CommandText = sQL; _Command.ExecuteNonQuery();
        int toBeDeletedID = Convert.ToInt32(((HiddenField)gvRecords.Rows[lastRowIndex].FindControl("hfID")).Value);
        sQL = "delete from collect_component_detail where Id=" + toBeDeletedID;
        _Command.CommandText = sQL; _Command.ExecuteNonQuery();
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('U can collect fee now, its deleted.!!'); window.location.href='PaidFeeSimple.aspx';", true);
    }
}