using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_collectFeeSimple : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            txtCalculationDate.Attributes.Add("ReadOnly", "true");
            if (!IsPostBack)
            {
                if (Convert.ToString(Session["_SessionStartDate"]) != "" && Session["_SessionStartDate"] != null)
                {
                    DateTime varSessionStartDate = Convert.ToDateTime(Session["_SessionStartDate"]);
                    DateTime varSessionEndDate = Convert.ToDateTime(Session["_SessionEndDate"]);
                    while (varSessionStartDate <= varSessionEndDate)
                    {
                        ddlStartMonth.Items.Add(new ListItem(varSessionStartDate.ToString("MMMM - yyyy"), varSessionStartDate.ToString("yyyy-MM-01")));
                        ddlEndMonth.Items.Add(new ListItem(varSessionStartDate.ToString("MMMM - yyyy"), varSessionStartDate.ToString("yyyy-MM-01")));
                        varSessionStartDate = varSessionStartDate.AddMonths(1);
                    }
                }
            }
        }
    }
    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {
        string SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";
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
            }
        }
        if (Convert.ToString(lblStudentID.Text).Trim().Length > 0)
        {
            CalculationManager(Convert.ToInt32(lblStudentID.Text));
        }
    }
    public class ComponentDetails
    {
        public int varComponentID = 0;
        public string varComponentName = "";
        public int varComponentFrequency = 0;
    }
    public class FeeCollection
    {
        public int varStudentID = 0;
        public int varComponentID = 0;
        public string varComponentName = "";
        public int varComponentFrequency = 0;
        public double varAmountPayable = 0.0;
        public double varPreviousPayment = 0.0;
        public double varAmountPaid = 0.0;
        public double varDiscount = 0.0;
        public double varToBePaid = 0.0;
    }
    private void CalculationManager(int _StudentID)
    {
        string SQL = ""; DataTable _dtblFeeRecord = new DataTable(); OdbcDataReader _dtReader = null;
        List<FeeCollection> _LsFeeCollectionDue = new List<FeeCollection>(); List<FeeCollection> _LsFeeCollection = new List<FeeCollection>(); List<FeeCollection> _LsFeeRecord = new List<FeeCollection>(); FeeCollection _FeeCollection = null;
        ComponentDetails _ComponentDetails = null; List<ComponentDetails> _LsComponentDetails = new List<ComponentDetails>();

        _dtblFeeRecord.Columns.Add("COMPONENT_ID"); _dtblFeeRecord.Columns.Add("COMPONENT_NAME"); _dtblFeeRecord.Columns.Add("FREQ"); _dtblFeeRecord.Columns.Add("AMOUNT_PAYBLE"); _dtblFeeRecord.Columns.Add("PREVIOUS"); _dtblFeeRecord.Columns.Add("Discount"); _dtblFeeRecord.Columns.Add("ADISCOUNT"); _dtblFeeRecord.Columns.Add("ToBePaid");


        SQL = "CALL `spComponentDetailsFromStudentIDAndSessionId`('" + _StudentID + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
        _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure;

        _dtReader = _Command.ExecuteReader();
        if (_dtReader.HasRows)
        {
            while (_dtReader.Read())
            {
                _ComponentDetails = new ComponentDetails();
                _ComponentDetails.varComponentID = Convert.ToInt32(_dtReader["COMPONENT_ID"]);
                _ComponentDetails.varComponentName = Convert.ToString(_dtReader["COMPONENT_NAME"]);
                _ComponentDetails.varComponentFrequency = Convert.ToInt32(_dtReader["COMPONENT_FREQUENCY"]);
                _LsComponentDetails.Add(_ComponentDetails);
            } _dtReader.Close(); _dtReader.Dispose();
        }
        else { _dtReader.Close(); _dtReader.Dispose(); }

        if (_LsComponentDetails.Count > 0)
        {
            double varAMOUNT_PAYBLE, varPreviousDue, varDISCOUNT, varADISCOUNT = 0.0;
            foreach (ComponentDetails objComponentDetails in _LsComponentDetails)
            {
                varAMOUNT_PAYBLE = 0.0; varPreviousDue = 0.0; varDISCOUNT = 0.0;
                DataRow _row = _dtblFeeRecord.NewRow();
                _row["COMPONENT_ID"] = Convert.ToString(objComponentDetails.varComponentID);
                _row["COMPONENT_NAME"] = Convert.ToString(objComponentDetails.varComponentName);
                _row["FREQ"] = Convert.ToString(objComponentDetails.varComponentFrequency);
                SQL = "CALL `spCalculateFeeFromStudentIDComponentIDMappedDateRangeSessionID`('" + _StudentID + "','" + objComponentDetails.varComponentID + "', '" + ddlStartMonth.SelectedValue + "', '" + ddlEndMonth.SelectedValue + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
                _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure;
                _dtReader = _Command.ExecuteReader();
                if (_dtReader.HasRows)
                {
                    while (_dtReader.Read())
                    {
                        varAMOUNT_PAYBLE = Convert.ToDouble(_dtReader["AMOUNT_PAYBLE"]);
                        varPreviousDue = Convert.ToDouble(_dtReader["Previousdue"]);
                        varDISCOUNT = Convert.ToDouble(_dtReader["Discount"]); 
                        varADISCOUNT = Convert.ToDouble(_dtReader["ADISCOUNT"]);
                    } _dtReader.Close(); _dtReader.Dispose();
                }
                else { _dtReader.Close(); _dtReader.Dispose(); varAMOUNT_PAYBLE = 0.0; varPreviousDue = 0.0; varDISCOUNT = 0.0; }

                _row["AMOUNT_PAYBLE"] = Convert.ToString(varAMOUNT_PAYBLE);
                _row["PREVIOUS"] = Convert.ToString(varPreviousDue);
                _row["Discount"] = Convert.ToString(varDISCOUNT);
                _row["ADISCOUNT"] = Convert.ToString(varADISCOUNT);
                _row["ToBePaid"] = Convert.ToString((varAMOUNT_PAYBLE - (varDISCOUNT + varADISCOUNT)) + (varPreviousDue));
                _dtblFeeRecord.Rows.Add(_row);
            }

            for (int i = _dtblFeeRecord.Rows.Count - 1; i >= 0; i--)
            {
                DataRow _row = _dtblFeeRecord.Rows[i];
                if (Convert.ToInt32(_row["AMOUNT_PAYBLE"]) == 0 && Convert.ToInt32(_row["PREVIOUS"]) == 0 && Convert.ToInt32(_row["Discount"]) == 0)
                {
                    _row.Delete();
                }
            }
            ViewState["_dtblFeeRecord"] = _dtblFeeRecord;
            gvFeeAmountDetails.DataSource = _dtblFeeRecord; gvFeeAmountDetails.DataBind();
            btnVerify.Visible = true; btnReset.Visible = true;

            DateTime varFineStartingDate = Convert.ToDateTime(txtCalculationDate.Text);
            DateTime varFeesCalculationDate = Convert.ToDateTime(txtCalculationDate.Text);
            DateTime varFineStartDate = Convert.ToDateTime(varFeesCalculationDate.Month + "-11-" + varFeesCalculationDate.Year);

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

            varFineStartingDate = varFineStartingDate.AddDays(9);

            double FineAmount = 0.0;
            FineAmount = CalculatefineAmount(varFineStartingDate, varFeesCalculationDate, 0.0);
            txtFineAmount.Text = Convert.ToString(FineAmount);
            PrintCalculation();
        }
    }
    private double CalculatefineAmount(DateTime MappedDate, DateTime CalculationDate, double Fine)
    {
        double DateDiff = (CalculationDate - MappedDate).TotalDays;
        if (Convert.ToInt32(DateDiff) > 0)
        {
            if (DateDiff < 10)
            {
                Fine += DateDiff * 5;
            }
            else if (DateDiff > 10 && DateDiff < 15)
            {
                Fine += 50 + ((DateDiff - 10) * 10);
            }
            else
            {
                Fine += 250;
                Fine = CalculatefineAmount(MappedDate.AddMonths(1), CalculationDate, Fine);
            }
        }
        return Fine;
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

        if (TotalPayable == 0) { btnSubmit.Enabled = false; } else { btnSubmit.Enabled = true; }

        lblTotalAmount.Text = "<b>Total Payable: </b>" + Convert.ToString(varAmountPayable);
        lblTotalDue.Text = "<b>Total Previous Due: </b>" + Convert.ToString(varPreviousDue);
        lblTotalDiscount.Text = "<b>Total Discount: </b>" + Convert.ToString(varDiscount);
        lblTotalADiscount.Text = "<b>Total one time Discount: </b>" + Convert.ToString(varADiscount);
        lblTotalToBePaid.Text = "<b>Total Payable amount: </b>" + Convert.ToString(TotalPayable);
        lblTotalFine.Text = "<b>Total Fine: </b>" + Convert.ToString(Convert.ToInt32(txtFineAmount.Text));
        lblTotalPaid.Text = "<b>Total Payment: </b>" + Convert.ToString(ToBePaid + Convert.ToDouble(txtFineAmount.Text));
        hfTotalAmountPaid.Value = Convert.ToString(ToBePaid + Convert.ToDouble(txtFineAmount.Text));
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        PrintCalculation(); btnSubmit.Visible = true;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "window.location.href='collectSimple.aspx';", true);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int varStudentID = Convert.ToInt32(lblStudentID.Text);
        int ComponentID, ComponentFrequency, ComponentDetailID = 0;
        string sQL = "";
        OdbcDataReader _dtReader = null;
        double varAmountPayable, varAmountPaid, varDiscount, varDueAmount, varActualAmountPaid = 0.0;
        DateTime varMappedDate = DateTime.Now;
        DateTime varFeeStartDate = Convert.ToDateTime(ddlStartMonth.SelectedValue);
        DateTime varFeeEndDate = Convert.ToDateTime(ddlEndMonth.SelectedValue);

        if (Convert.ToString(txtChequeDate.Text).Trim().Length > 0)
        {
            sQL = "insert into collect_component_detail(STUDENT_ID,AMOUNT_PAID,MODE,CHEQUE_NUMBER,CHEQUE_DATE,BANK_NAME,PAID_DATE,FINE,SCHOOL_SESSION_ID,CREATE_DATE,CREATE_TIME)";
            sQL += "values('" + varStudentID + "','" + hfTotalAmountPaid.Value + "','" + ddlSelectPaymentMode.SelectedValue + "','" + txtChequeNo.Text.Trim() + "','" + Convert.ToDateTime(txtChequeDate.Text).ToString("yyyy-MM-dd") + "','" + txtBankDetails.Text + "','" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + txtFineAmount.Text + "', '" + Convert.ToString(Session["_SessionID"]) + "',now(),now())";
        }
        else
        {
            sQL = "insert into collect_component_detail(STUDENT_ID,AMOUNT_PAID,MODE,BANK_NAME,PAID_DATE,FINE,SCHOOL_SESSION_ID,CREATE_DATE,CREATE_TIME)";
            sQL += "values('" + varStudentID + "','" + hfTotalAmountPaid.Value + "','" + ddlSelectPaymentMode.SelectedValue + "','" + txtBankDetails.Text + "','" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + txtFineAmount.Text + "', '" + Convert.ToString(Session["_SessionID"]) + "',now(),now())";
        } _Command.CommandText = sQL; _Command.ExecuteNonQuery();
        sQL = "select max(ID) from collect_component_detail";
        _Command.CommandText = sQL; ComponentDetailID = Convert.ToInt32(_Command.ExecuteScalar());

        int i = 0;
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            i++;
            TextBox txtADiscount = (TextBox)_row.FindControl("txtADiscount");
            TextBox txtPayment = (TextBox)_row.FindControl("txtPayment");
            Label lblDiscount = (Label)_row.FindControl("lblDiscount");
            ComponentID = Convert.ToInt32(((HiddenField)_row.FindControl("hfCOMPONENT_ID")).Value);
            ComponentFrequency = Convert.ToInt32(((HiddenField)_row.FindControl("hfFREQ")).Value);
            varAmountPayable = 0.0; varAmountPaid = 0.0; varDiscount = 0.0; varDueAmount = 0.0;

            varActualAmountPaid = Convert.ToDouble(txtPayment.Text);
            if (Convert.ToInt32(_row.Cells[3].Text) > 0)
            {
                sQL = "CALL `spCollectionDetailsFromStudentIDComponentIDAndSessionID`('" + varStudentID + "', '" + ComponentID + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
                _Command.CommandText = sQL;
                _dtReader = _Command.ExecuteReader();
                if (_dtReader.HasRows)
                {
                    while (_dtReader.Read())
                    {
                        varAmountPayable = Convert.ToDouble(_dtReader["AMOUNT_PAYBLE"]);
                        varAmountPaid = Convert.ToDouble(_dtReader["AMOUNT_PAID"]);
                        varDiscount = Convert.ToDouble(_dtReader["DISCOUNT"]);
                        varMappedDate = Convert.ToDateTime(_dtReader["MAPPED_DATE"]);
                        varDueAmount = varAmountPayable - (varAmountPaid + varDiscount) + varDueAmount;
                        if (varDueAmount > 0) { break; } else { continue; }
                    } _dtReader.Close(); _dtReader.Dispose();
                }
                else { _dtReader.Close(); _dtReader.Dispose(); }

                while (varMappedDate < varFeeStartDate)
                {
                    varDueAmount = 0.0;
                    sQL = "CALL `spDueAmountMonthWiseFromStudentComponentSessionIDAndMappedDate`('" + varStudentID + "', '" + ComponentID + "','" + varMappedDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
                    _Command.CommandText = sQL;
                    varDueAmount = Convert.ToDouble(_Command.ExecuteScalar());

                    if (varActualAmountPaid > 0)
                    {
                        if (varActualAmountPaid >= varDueAmount)
                        {
                            //sQL = "update collect_component_master set amount_paid=amount_paid+" + varDueAmount + ",PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' where amount_payble>0 and Student_id='" + varStudentID + "' and component_id='" + ComponentID + "' and mapped_date='" + varMappedDate.ToString("yyyy-MM-dd") + "';";
                            sQL = "insert into collect_component_master(Student_id,component_id,AMOUNT_PAYBLE,AMOUNT_PAID,DISCOUNT,PAID_DATE,MAPPED_DATE,FEE_UPDATE_DATE,FEE_UPDATE_TIME,DETAIL_ID,SCHOOL_SESSION_ID) values('" + varStudentID + "', '" + ComponentID + "',0," + varDueAmount + ",0,'" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + varMappedDate.ToString("yyyy-MM-dd") + "',now(),now(),'" + ComponentDetailID + "','" + Convert.ToString(Session["_SessionID"]) + "')";
                            _Command.CommandText = sQL; _Command.ExecuteScalar();
                            varActualAmountPaid -= varDueAmount;
                        }
                        else
                        {
                            //sQL = "update collect_component_master set amount_paid=amount_paid+" + varActualAmountPaid + ",PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' where amount_payble>0 and Student_id='" + varStudentID + "' and component_id='" + ComponentID + "' and mapped_date='" + varMappedDate.ToString("yyyy-MM-dd") + "';";
                            sQL = "insert into collect_component_master(Student_id,component_id,AMOUNT_PAYBLE,AMOUNT_PAID,DISCOUNT,PAID_DATE,MAPPED_DATE,FEE_UPDATE_DATE,FEE_UPDATE_TIME,DETAIL_ID,SCHOOL_SESSION_ID) values('" + varStudentID + "', '" + ComponentID + "',0," + varActualAmountPaid + ",0,'" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + varMappedDate.ToString("yyyy-MM-dd") + "',now(),now(),'" + ComponentDetailID + "','" + Convert.ToString(Session["_SessionID"]) + "')";
                            _Command.CommandText = sQL; _Command.ExecuteScalar();
                            varActualAmountPaid -= varDueAmount;

                        }
                    }
                    varMappedDate = varMappedDate.AddMonths(ComponentFrequency);
                }

            }

            varFeeStartDate = Convert.ToDateTime(ddlStartMonth.SelectedValue);
            varFeeEndDate = Convert.ToDateTime(ddlEndMonth.SelectedValue);

            sQL = "delete from collect_component_master where amount_payble=0 and Student_id='" + varStudentID + "' and component_id='" + ComponentID + "' and mapped_date='" + varFeeEndDate.ToString("yyyy-MM-dd") + "';";
            _Command.CommandText = sQL; _Command.ExecuteScalar();

            if (Convert.ToDouble(txtADiscount.Text) > 0)
            {
                sQL = "insert into collect_component_master(Student_id,component_id,amount_payble,amount_paid,discount,paid_date,mapped_date,school_session_id) values('" + varStudentID + "','" + ComponentID + "',0,0," + Convert.ToDouble(txtADiscount.Text) + ",'" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + varFeeEndDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(Session["_SessionID"]) + "');";
                _Command.CommandText = sQL; _Command.ExecuteScalar();
            }

            if (varActualAmountPaid > 0)
            {
                while (varFeeStartDate <= varFeeEndDate)
                {
                    varDueAmount = 0.0;
                    sQL = "CALL `spDueAmountMonthWiseFromStudentComponentSessionIDAndMappedDate`('" + varStudentID + "', '" + ComponentID + "','" + varFeeStartDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
                    _Command.CommandText = sQL;
                    varDueAmount = Convert.ToDouble(_Command.ExecuteScalar());

                    if (varActualAmountPaid > 0)
                    {
                        if (varActualAmountPaid >= varDueAmount)
                        {
                            sQL = "select amount_paid from collect_component_master where Student_id='" + varStudentID + "' and component_id='" + ComponentID + "' and mapped_date='" + varFeeStartDate.ToString("yyyy-MM-dd") + "';";
                            _Command.CommandText = sQL; double amtPAid = Convert.ToDouble(_Command.ExecuteScalar());
                            if (amtPAid > 0)
                            {
                                sQL = "insert into collect_component_master(Student_id,component_id,AMOUNT_PAYBLE,AMOUNT_PAID,DISCOUNT,PAID_DATE,MAPPED_DATE,FEE_CREATE_DATE,FEE_CREATE_TIME,SCHOOL_SESSION_ID) values('" + varStudentID + "', '" + ComponentID + "',0," + varDueAmount + ",0,'" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + varMappedDate.ToString("yyyy-MM-dd") + "',now(),now(),'" + Convert.ToString(Session["_SessionID"]) + "')";
                            }
                            else
                            {
                                sQL = "update collect_component_master set amount_paid=amount_paid+" + varDueAmount + ",PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' where amount_payble>0 and Student_id='" + varStudentID + "' and component_id='" + ComponentID + "' and mapped_date='" + varFeeStartDate.ToString("yyyy-MM-dd") + "';";
                            }
                            _Command.CommandText = sQL; _Command.ExecuteScalar();
                            varActualAmountPaid -= varDueAmount;
                        }
                        else
                        {
                            sQL = "select amount_paid from collect_component_master where Student_id='" + varStudentID + "' and component_id='" + ComponentID + "' and mapped_date='" + varFeeStartDate.ToString("yyyy-MM-dd") + "';";
                            _Command.CommandText = sQL; double amtPAid = Convert.ToDouble(_Command.ExecuteScalar());
                            if (amtPAid > 0)
                            {
                                sQL = "insert into collect_component_master(Student_id,component_id,AMOUNT_PAYBLE,AMOUNT_PAID,DISCOUNT,PAID_DATE,MAPPED_DATE,FEE_CREATE_DATE,FEE_CREATE_TIME,SCHOOL_SESSION_ID) values('" + varStudentID + "', '" + ComponentID + "',0," + varActualAmountPaid + ",0,'" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + varMappedDate.ToString("yyyy-MM-dd") + "',now(),now(),'" + Convert.ToString(Session["_SessionID"]) + "')";
                            }
                            else
                            {
                                sQL = "update collect_component_master set amount_paid=amount_paid+" + varActualAmountPaid + ",PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' where amount_payble>0 and Student_id='" + varStudentID + "' and component_id='" + ComponentID + "' and mapped_date='" + varFeeStartDate.ToString("yyyy-MM-dd") + "';";
                            }
                            _Command.CommandText = sQL; _Command.ExecuteScalar();
                            varActualAmountPaid -= varDueAmount;
                        }
                    }
                    varFeeStartDate = varFeeStartDate.AddMonths(1);
                }

            }

        }

        varFeeStartDate = Convert.ToDateTime(ddlStartMonth.SelectedValue);
        varFeeEndDate = Convert.ToDateTime(ddlEndMonth.SelectedValue);

        while (varFeeStartDate <= varFeeEndDate)
        {
            sQL = "update collect_component_master set DETAIL_ID=" + ComponentDetailID + " where STUDENT_ID='" + varStudentID + "' and MAPPED_DATE='" + varFeeStartDate.ToString("yyyy-MM-dd") + "' and PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' and SCHOOL_SESSION_ID = '" + Convert.ToString(Session["_SessionID"]) + "';";
            _Command.CommandText = sQL; _Command.ExecuteNonQuery();
            varFeeStartDate = varFeeStartDate.AddMonths(1);
        }

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Fee Collected !!!'); window.location.href='collectFeeSimple.aspx';", true);

    }
    protected void txtADiscount_TextChanged(object sender, EventArgs e)
    {
        TextBox txtADiscount = (TextBox)sender;
        GridViewRow _row = (GridViewRow)txtADiscount.NamingContainer;
        TextBox txtPayment = (TextBox)_row.FindControl("txtPayment");
        Label lblDiscount = (Label)_row.FindControl("lblDiscount");
        double varAmountPayable, varPreviousDue, varDiscount, varADiscount = 0.0; double ToBePaid = 0.0;

        varAmountPayable = Convert.ToDouble(_row.Cells[2].Text);
        varPreviousDue = Convert.ToDouble(_row.Cells[3].Text);
        varDiscount = Convert.ToDouble(lblDiscount.Text);
        if (txtADiscount.Text.Trim().Length > 0)
        {
            varADiscount = Convert.ToDouble(txtADiscount.Text);
        }
        else { varADiscount = 0.0; }
        ToBePaid = varAmountPayable + varPreviousDue - varDiscount - varADiscount;
        txtPayment.Text = Convert.ToString(ToBePaid);
        PrintCalculation();
    }
}