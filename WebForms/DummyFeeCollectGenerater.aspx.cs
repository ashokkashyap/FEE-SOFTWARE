using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_DummyFeeCollectGenerater : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; int scrollno;
    String newselect; int flagi = 0, Rno;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                checkCts.Checked = true;
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
                txtCalculationDate.Text = DateTime.Now.ToString("dd-MMMM-yyyy");
            }
            // i = 0;

            int ScrollNumbr = 0;
            _Command.CommandText = "select max(scroll_no) from  collect_component_master";
            int scroll_i = Convert.ToInt32(_Command.ExecuteScalar());

            if (scroll_i.Equals(0))
            {
                scroll_i = 1;
            }
            else
            {
                scrollno = scroll_i + 1;
            }
            txtScrollingNumber.Text = scrollno.ToString();
        }
    }
    
    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {

        String radionew_old = radiobtnNEW_old.SelectedValue;
        String admitionno_new = txtAdmissionNo.Text;
        getmax();

        if (radiobtnNEW_old.SelectedValue == "NEW")
        {
            newselect = "y";
            _Command.CommandText = "Select count(*) from ign_student_master where STUDENT_REGISTRATION_NBR = '" + admitionno_new + "' ";
        }
        else
        {
            newselect = "N";
            _Command.CommandText = "Select count(*) from ign_student_master_old where STUDENT_REGISTRATION_NBR = '" + admitionno_new + "' ";
        }
        /////////////end ashok/////////////
        // _Command.CommandText = "Select count(*) from ign_student_master where STUDENT_REGISTRATION_NBR = '" + txtAdmissionNo.Text.Trim() + "' ";
        int cnti = Convert.ToInt32(_Command.ExecuteScalar());
        if (cnti.Equals(1))
        {
            if (newselect == "y")
            {
                string SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";
                _Command.CommandText = SQL;
            }

            else if (newselect == "N")
            {
                string SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";
                _Command.CommandText = SQL;
            }
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

            CalculationManager(Convert.ToInt32(lblStudentID.Text));
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Admission Nbr Not Valid !!!');", true);

        }
    }
    private void CalculationManager(int _StudentID)
    {
        string SQL = ""; OdbcDataAdapter _dtAdapter = new OdbcDataAdapter(); DataTable _dtblFeeRecord = new DataTable(); OdbcDataReader _dtReader = null;
        List<FeeCollection> _LsFeeCollectionDue = new List<FeeCollection>(); List<FeeCollection> _LsFeeCollection = new List<FeeCollection>(); List<FeeCollection> _LsFeeRecord = new List<FeeCollection>(); FeeCollection _FeeCollection = null;
        ComponentDetails _ComponentDetails = null; List<ComponentDetails> _LsComponentDetails = new List<ComponentDetails>();



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

        _dtblFeeRecord.Columns.Add("COMPONENT_ID");
        _dtblFeeRecord.Columns.Add("COMPONENT_NAME");
        _dtblFeeRecord.Columns.Add("FREQ");
        _dtblFeeRecord.Columns.Add("AMOUNT_PAYBLE");
        _dtblFeeRecord.Columns.Add("PREVIOUS");
        _dtblFeeRecord.Columns.Add("Discount");
        _dtblFeeRecord.Columns.Add("ADISCOUNT");
        _dtblFeeRecord.Columns.Add("ToBePaid");

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

        DateTime varFeesCollectionDate = Convert.ToDateTime(txtCalculationDate.Text);
        DateTime varFineStartingDate = Convert.ToDateTime(txtCalculationDate.Text);

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

        int varCalculatedFineAmount = 0;
        //if (varFineStartingDate <= varFeesCollectionDate)
        //{
        //    DateTime varMinimumDateToAvoidFine = Convert.ToDateTime(varFineStartingDate.AddDays(14));

        //    varCalculatedFineAmount = 10 * ((varFeesCollectionDate - varMinimumDateToAvoidFine).Days);
        //}
        //if (varCalculatedFineAmount < 0) { varCalculatedFineAmount = 0; }
        txtFineAmount.Text = Convert.ToString(varCalculatedFineAmount);

        if (varFineStartingDate < varFeesCollectionDate)
        {
            if (varFineStartingDate < varFeesCollectionDate)
            {
                TimeSpan t = varFeesCollectionDate - varFineStartingDate;
                if (t.TotalDays > 30)
                {
                    txtReAdmissionCharges.Text = "0";
                }
                else
                { txtReAdmissionCharges.Text = "0"; }
            }
            else { txtReAdmissionCharges.Text = "0"; }
        }
        else { txtReAdmissionCharges.Text = "0"; }
        PrintCalculation();
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
        }
        ////////////////sssssssssssssssssssssss///////////////////




        //////////ssssssssssssssssssss/////////////ssssss

        TotalPayable = varAmountPayable + varPreviousDue - varDiscount - varADiscount;

        int ScrollNumbr = 0;
        if (txtScrollingNumber.Text.Trim().Length > 0)
        {
            ScrollNumbr = Convert.ToInt32(txtScrollingNumber.Text);
        }
        int varSelectedStartingMonthIndex = Convert.ToInt32(ddlStartMonth.SelectedIndex);
        int varSelectedEndingMonthIndex = Convert.ToInt32(ddlEndMonth.SelectedIndex);
        string NewScrollingNumbers = "";

        if (Convert.ToDateTime(ddlStartMonth.SelectedValue).Month.Equals(2) && Convert.ToDateTime(ddlEndMonth.SelectedValue).Month.Equals(3))
        {
            NewScrollingNumbers += Convert.ToString(ScrollNumbr);

        }
        else if (Convert.ToDateTime(ddlStartMonth.SelectedValue).Month.Equals(3) && Convert.ToDateTime(ddlEndMonth.SelectedValue).Month.Equals(3))
        {
            NewScrollingNumbers += Convert.ToString(ScrollNumbr);
        }
        else
        {
            if (Convert.ToDateTime(ddlEndMonth.SelectedValue).Month.Equals(3))
            {
                while (varSelectedStartingMonthIndex <= varSelectedEndingMonthIndex - 1)
                {
                    NewScrollingNumbers += Convert.ToString(ScrollNumbr) + ","; varSelectedStartingMonthIndex += 1; ScrollNumbr += 1;
                } NewScrollingNumbers = NewScrollingNumbers.Substring(0, NewScrollingNumbers.Length - 1);

            }
            else
            {
                while (varSelectedStartingMonthIndex <= varSelectedEndingMonthIndex)
                {
                    NewScrollingNumbers += Convert.ToString(ScrollNumbr) + ","; varSelectedStartingMonthIndex += 1; ScrollNumbr += 1;
                } NewScrollingNumbers = NewScrollingNumbers.Substring(0, NewScrollingNumbers.Length - 1);

            }

        }
        lblScrollingNumbers.Text = NewScrollingNumbers;
        int ScrollingMonth = Convert.ToDateTime(txtCalculationDate.Text).Month;
        string SQL = "CALL `spScrollNoAvailabilityFromScrollNoMonthNumbrAndSessionID`('" + Convert.ToString(NewScrollingNumbers) + "','" + Convert.ToString(ScrollingMonth) + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
        _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure;
        lblInfo.Text = ""; btnSubmit.Enabled = true;




        lblTotalAmount.Text = "<b>Total Payable: </b>" + Convert.ToString(varAmountPayable);
        lblTotalDue.Text = "<b>Total Previous Due: </b>" + Convert.ToString(varPreviousDue);
        lblTotalDiscount.Text = "<b>Total Discount: </b>" + Convert.ToString(varDiscount);
        lblTotalADiscount.Text = "<b>Total one time Discount: </b>" + Convert.ToString(varADiscount);
        lblTotalToBePaid.Text = "<b>Total Payable amount: </b>" + Convert.ToString(TotalPayable);
        lblTotalFine.Text = "<b>Total Fine: </b>" + Convert.ToString(Convert.ToInt32(txtFineAmount.Text) + Convert.ToInt32(txtReAdmissionCharges.Text));
        lblTotalPaid.Text = "<b>Total Payment: </b>" + Convert.ToString(ToBePaid + Convert.ToDouble(txtFineAmount.Text) + Convert.ToInt32(txtReAdmissionCharges.Text));
        hfTotalAmountPaid.Value = Convert.ToString(ToBePaid + Convert.ToDouble(txtFineAmount.Text) + Convert.ToInt32(txtReAdmissionCharges.Text));
    }
    protected void ddlSelectPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void txtADiscount_TextChanged(object sender, EventArgs e)
    {

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
            int varComponentID = Convert.ToInt32(((HiddenField)_row.FindControl("hfCOMPONENT_ID")).Value);

            ToBePaid = varAmountPayable + varPreviousDue - varDiscount - varADiscount;
            txtPayment.Text = Convert.ToString(ToBePaid);
            PrintCalculation();
        }
    }

    public class ComponentDetails
    {
        public int varComponentID = 0;
        public string varComponentName = "";
        public int varComponentFrequency = 0;
    }
    private void CalculateFee(int _StudentID)
    {
        string SQL = ""; OdbcDataAdapter _dtAdapter = new OdbcDataAdapter(); DataTable _dtblFeeRecord = new DataTable(); OdbcDataReader _dtReader = null;
        List<FeeCollection> _LsFeeCollectionDue = new List<FeeCollection>(); List<FeeCollection> _LsFeeCollection = new List<FeeCollection>(); List<FeeCollection> _LsFeeRecord = new List<FeeCollection>(); FeeCollection _FeeCollection = null;

        SQL = "CALL `spCalculatedDueAmountFromStudentIDBeforeDateAndSessionID`('" + _StudentID + "', '" + ddlStartMonth.SelectedValue + "','" + Convert.ToString(Session["_SessionID"]) + "');";
        _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure;
        _dtReader = _Command.ExecuteReader();
        if (_dtReader.HasRows)
        {
            while (_dtReader.Read())
            {
                _FeeCollection = new FeeCollection();
                _FeeCollection.varStudentID = Convert.ToInt32(_dtReader["STUDENT_ID"]);
                _FeeCollection.varComponentID = Convert.ToInt32(_dtReader["COMPONENT_ID"]);
                _FeeCollection.varComponentName = Convert.ToString(_dtReader["COMPONENT_NAME"]);
                _FeeCollection.varComponentFrequency = Convert.ToInt32(_dtReader["COMPONENT_FREQUENCY"]);
                _FeeCollection.varAmountPayable = Convert.ToDouble(0);
                _FeeCollection.varAmountPaid = Convert.ToDouble(0);
                _FeeCollection.varDiscount = Convert.ToDouble(0);
                _FeeCollection.varPreviousPayment = Convert.ToDouble(_dtReader["PreviousPayment"]);
                _LsFeeCollectionDue.Add(_FeeCollection);
            } _dtReader.Close(); _dtReader.Dispose();
        }
        else { _dtReader.Close(); _dtReader.Dispose(); }

        SQL = "CALL `spCalculateFeeFromStudentIDAndMappedDateRange`('" + _StudentID + "', '" + ddlStartMonth.SelectedValue + "', '" + ddlEndMonth.SelectedValue + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
        _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure;
        _dtReader = _Command.ExecuteReader();
        if (_dtReader.HasRows)
        {
            while (_dtReader.Read())
            {
                int varComponentID = Convert.ToInt32(_dtReader["COMPONENT_ID"]);

                var ExistingRecord = from record in _LsFeeCollectionDue.AsEnumerable() where record.varComponentID.Equals(varComponentID) select record;
                if (ExistingRecord.Any())
                {
                    foreach (var Record in ExistingRecord)
                    {
                        _FeeCollection = new FeeCollection();
                        _FeeCollection.varStudentID = Convert.ToInt32(_dtReader["STUDENT_ID"]);
                        _FeeCollection.varComponentID = Convert.ToInt32(_dtReader["COMPONENT_ID"]);
                        _FeeCollection.varComponentName = Convert.ToString(_dtReader["COMPONENT_NAME"]);
                        _FeeCollection.varComponentFrequency = Convert.ToInt32(_dtReader["COMPONENT_FREQUENCY"]);
                        _FeeCollection.varAmountPayable = Record.varAmountPayable + Convert.ToDouble(_dtReader["AMOUNT_PAYBLE"]);
                        _FeeCollection.varAmountPaid = Record.varAmountPaid + Convert.ToDouble(_dtReader["AMOUNT_PAID"]);
                        _FeeCollection.varDiscount = Record.varDiscount + Convert.ToDouble(_dtReader["DISCOUNT"]);
                        _FeeCollection.varToBePaid = Record.varToBePaid + Convert.ToDouble(_dtReader["ToBePaid"]);
                        _LsFeeCollection.Add(_FeeCollection);
                    }
                }
                else
                {
                    _FeeCollection = new FeeCollection();
                    _FeeCollection.varStudentID = Convert.ToInt32(_dtReader["STUDENT_ID"]);
                    _FeeCollection.varComponentID = Convert.ToInt32(_dtReader["COMPONENT_ID"]);
                    _FeeCollection.varComponentName = Convert.ToString(_dtReader["COMPONENT_NAME"]);
                    _FeeCollection.varComponentFrequency = Convert.ToInt32(_dtReader["COMPONENT_FREQUENCY"]);
                    _FeeCollection.varAmountPayable = Convert.ToDouble(_dtReader["AMOUNT_PAYBLE"]);
                    _FeeCollection.varAmountPaid = Convert.ToDouble(_dtReader["AMOUNT_PAID"]);
                    _FeeCollection.varDiscount = Convert.ToDouble(_dtReader["DISCOUNT"]);
                    _FeeCollection.varToBePaid = Convert.ToDouble(_dtReader["ToBePaid"]);
                    _LsFeeCollection.Add(_FeeCollection);
                }
            } _dtReader.Close(); _dtReader.Dispose();
        }
        else { _dtReader.Close(); _dtReader.Dispose(); }

        if (_LsFeeCollectionDue.Count > 0)
        {
            foreach (FeeCollection _objFeeCollection in _LsFeeCollectionDue)
            {
                int varComponentID = Convert.ToInt32(_objFeeCollection.varComponentID);
                var ExistingRecord = from record in _LsFeeCollection.AsEnumerable() where record.varComponentID.Equals(varComponentID) select record;
                if (ExistingRecord.Any())
                {
                    foreach (var Record in ExistingRecord)
                    {
                        _FeeCollection = new FeeCollection();
                        _FeeCollection.varStudentID = Convert.ToInt32(Record.varStudentID);
                        _FeeCollection.varComponentID = Convert.ToInt32(Record.varComponentID);
                        _FeeCollection.varComponentName = Convert.ToString(Record.varComponentName);
                        _FeeCollection.varComponentFrequency = Convert.ToInt32(Record.varComponentFrequency);
                        _FeeCollection.varAmountPayable = Convert.ToDouble(Record.varAmountPayable);
                        _FeeCollection.varAmountPaid = Convert.ToDouble(Record.varAmountPaid);
                        _FeeCollection.varDiscount = Convert.ToDouble(Record.varDiscount);
                        _FeeCollection.varToBePaid = Convert.ToDouble(Record.varToBePaid);
                        _LsFeeRecord.Add(_FeeCollection);
                    }
                }
                else
                {
                    _FeeCollection = new FeeCollection();
                    _FeeCollection.varStudentID = Convert.ToInt32(_objFeeCollection.varStudentID);
                    _FeeCollection.varComponentID = Convert.ToInt32(_objFeeCollection.varComponentID);
                    _FeeCollection.varComponentName = Convert.ToString(_objFeeCollection.varComponentName);
                    _FeeCollection.varComponentFrequency = Convert.ToInt32(_objFeeCollection.varComponentFrequency);
                    _FeeCollection.varAmountPayable = Convert.ToDouble(_objFeeCollection.varAmountPayable);
                    _FeeCollection.varAmountPaid = Convert.ToDouble(_objFeeCollection.varAmountPaid);
                    _FeeCollection.varDiscount = Convert.ToDouble(_objFeeCollection.varDiscount);
                    _FeeCollection.varToBePaid = Convert.ToDouble(_objFeeCollection.varToBePaid);
                    _LsFeeRecord.Add(_FeeCollection);
                }
            }
        }
        else { _LsFeeRecord = (List<FeeCollection>)_LsFeeCollection; }

        var FieldName = typeof(FeeCollection).GetFields().ToList();
        foreach (var _FieldName in FieldName)
        {
            _dtblFeeRecord.Columns.Add(Convert.ToString(_FieldName.Name.ToString()));
        }

        foreach (FeeCollection _objFeeCollection in _LsFeeRecord)
        {
            DataRow _row = _dtblFeeRecord.NewRow();
            _row["varStudentID"] = Convert.ToString(_objFeeCollection.varStudentID);
            _row["varComponentID"] = Convert.ToString(_objFeeCollection.varComponentID);
            _row["varComponentName"] = Convert.ToString(_objFeeCollection.varComponentName);
            _row["varComponentFrequency"] = Convert.ToString(_objFeeCollection.varComponentFrequency);
            _row["varAmountPayable"] = Convert.ToString(_objFeeCollection.varAmountPayable);
            _row["varAmountPaid"] = Convert.ToString(_objFeeCollection.varAmountPaid);
            _row["varDiscount"] = Convert.ToString(_objFeeCollection.varDiscount);
            _row["varToBePaid"] = Convert.ToString(_objFeeCollection.varToBePaid);
            _dtblFeeRecord.Rows.Add(_row);
        }
        gvFeeAmountDetails.DataSource = _dtblFeeRecord; gvFeeAmountDetails.DataBind();


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
    public void getmax()
    {
        int ScrollNumbr = 0;
        _Command.CommandText = "select max(scroll_no) from  collect_component_master";
        int scroll_i = Convert.ToInt32(_Command.ExecuteScalar());

        if (scroll_i.Equals(0))
        {
            scroll_i = 1;
        }
        else
        {
            scroll_i = scroll_i + 1;
        }
        txtScrollingNumber.Text = scroll_i.ToString();
    }
    private void FeeCalculatorNew(string _studentID)
    {
        string SQL = "CALL `spCalculateFeeFromStudentIDAndMappedDateRange`('" + _studentID + "', '" + ddlStartMonth.SelectedValue + "', '" + ddlEndMonth.SelectedValue + "')";
        Response.Write(SQL); Response.End();
        _Command.CommandText = SQL;
        OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
        _dtAdapter.SelectCommand = _Command;
        DataTable _dtblFeeRecord = new DataTable();
        _dtAdapter.Fill(_dtblFeeRecord);

        _dtblFeeRecord.Columns.Add("PREVIOUS");


        SQL = "CALL `spGetTotalDueAmountFromStudentIDAndDate`('" + _studentID + "','" + ddlStartMonth.SelectedValue + "')";
        _Command.CommandText = SQL;
        OdbcDataReader _dtReader = _Command.ExecuteReader();
        if (_dtReader.HasRows)
        {
            while (_dtReader.Read())
            {
                foreach (DataRow _row in _dtblFeeRecord.Rows)
                {
                    if (Convert.ToInt32(_row["COMPONENT_ID"]).Equals(Convert.ToInt32(_dtReader["COMPONENT_ID"])))
                    {
                        _row["PREVIOUS"] = Convert.ToString(_dtReader["TOTAL_DUE"]);
                    }
                }
            } _dtReader.Read();
        }
        else
        {
            _dtReader.Read();
            foreach (DataRow _row in _dtblFeeRecord.Rows)
            {
                _row["PREVIOUS"] = Convert.ToString("0");
            }
        }

        gvFeeAmountDetails.DataSource = _dtblFeeRecord; gvFeeAmountDetails.DataBind(); gvFeeAmountDetails.Visible = true;

        int varTotalAmount = 0, varTotalPaid = 0, varTotalDiscount = 0, varTotalToBePaid = 0;
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            Label lblDiscount = (Label)_row.FindControl("lblDiscount");
            TextBox txtPayment = (TextBox)_row.FindControl("txtPayment");
            TextBox txtADiscount = (TextBox)_row.FindControl("txtADiscount"); txtADiscount.Text = "0";
            int varAmt = 0;
            if (Convert.ToString(_row.Cells[3].Text).Trim().Length > 0 && _row.Cells[3].Text != null)
            {
                varAmt = (Convert.ToInt32(_row.Cells[2].Text) - Convert.ToInt32(_row.Cells[3].Text) - Convert.ToInt32(lblDiscount.Text));
            }
            else
            {
                varAmt = (Convert.ToInt32(_row.Cells[2].Text) - Convert.ToInt32(lblDiscount.Text));
            }
            if (varAmt <= 0) { txtPayment.Text = "0"; } else { txtPayment.Text = varAmt.ToString(); }
            varTotalAmount = varTotalAmount + Convert.ToInt32(_row.Cells[2].Text);
            varTotalPaid = varTotalPaid + Convert.ToInt32(_row.Cells[3].Text);
            varTotalDiscount = varTotalDiscount + Convert.ToInt32(lblDiscount.Text);
            varTotalToBePaid = varTotalToBePaid + Convert.ToInt32(txtPayment.Text);
        }
        lblTotalAmount.Text = "<b>Total Amount: </b>" + Convert.ToString(varTotalAmount);
        lblTotalDue.Text = "<b>Total Paid: </b>" + Convert.ToString(varTotalPaid);
        lblTotalDiscount.Text = "<b>Total Discount: </b>" + Convert.ToString(varTotalDiscount);
        lblTotalToBePaid.Text = "<b>Total to be Paid: </b>" + Convert.ToString(varTotalToBePaid);
        lblTotalPaid.Text = "<b>Total Payment: </b>" + Convert.ToString((varTotalAmount - varTotalPaid) - varTotalDiscount);
        btnVerify.Visible = true; btnReset.Visible = true; btnSubmit.Visible = true;


    }
    private void feeCalculator(string _studentID)
    {

        #region Verification
        gvFeeAmountDetails.DataSource = null; gvFeeAmountDetails.DataBind();
        _Command.CommandText = "SELECT * FROM ign_school_session_master WHERE ACTIVE_STATUS = 'Y'";
        var _DtReader = _Command.ExecuteReader();
        while (_DtReader.Read())
        {
            if (Convert.ToDateTime(txtCalculationDate.Text) < Convert.ToDateTime(_DtReader["START_DATE"]))
            {
                return;
            }
            if (Convert.ToDateTime(txtCalculationDate.Text) > Convert.ToDateTime(_DtReader["END_DATE"]))
            {
                return;
            }
        }
        _DtReader.Close();

        _Command.CommandText = "select count(*) from collect_component_master where student_id = '" + _studentID + "' and paid_date > '" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "'";
        if (Convert.ToInt32(_Command.ExecuteScalar()) > 0)
        {
            return;
        }
        #endregion
        #region StudentComponentDetails
        var SQL = "CALL `spMappedComponentsFromAdmissionNo`('" + Convert.ToString(Session["_SessionID"]) + "','" + txtAdmissionNo.Text.Trim() + "')";
        _Command.CommandText = SQL;
        var _dtblMappedComponents = new DataTable();
        var _dtAdapter = new OdbcDataAdapter();
        _dtAdapter.SelectCommand = _Command;
        _dtAdapter.Fill(_dtblMappedComponents);
        #endregion

        var _dtblFeeDescription = new DataTable(); _dtblFeeDescription.Columns.Add("COMPONENT_ID"); _dtblFeeDescription.Columns.Add("COMPONENT_NAME"); _dtblFeeDescription.Columns.Add("ID"); _dtblFeeDescription.Columns.Add("DISCOUNT"); _dtblFeeDescription.Columns.Add("DUE_AMOUNT"); _dtblFeeDescription.Columns.Add("COMPONENT_DETAIL_ID"); _dtblFeeDescription.Columns.Add("COMPONENT_AMOUNT");
        ArrayList objArrayListNotMappedId = new ArrayList();
        if (_dtblMappedComponents.Rows.Count > 0)
        {
            foreach (DataRow _row in _dtblMappedComponents.Rows)
            {
                var _dtblFeeDescriptionRow = _dtblFeeDescription.NewRow();
                int varAmountPayble = 0, varDueAmountPayble = 0, varDueAmountPaid = 0, varDiscount = 0;
                long varID = 0;
                string varFeesDuration = "";

                DateTime varComponentStartDt = Convert.ToDateTime(_row["START_MONTH"].ToString() + "/1/" + _row["START_YEAR"].ToString());
                DateTime varComponentEndDt = varComponentStartDt.AddMonths(Convert.ToInt32(_row["COMPONENT_FREQUENCY"].ToString())).AddDays(-1);
                DateTime varComponentStartDtLoop = varComponentStartDt;
                DateTime varComponentEndDtLoop = varComponentEndDt;

                int x = 0;
                while (true)
                {
                    if (Convert.ToDateTime(txtCalculationDate.Text) >= varComponentStartDtLoop && Convert.ToDateTime(txtCalculationDate.Text) <= varComponentEndDtLoop)
                    {
                        varFeesDuration = varComponentStartDtLoop.ToString("MMM") + "-" + varComponentEndDtLoop.ToString("MMM");
                        break;
                    }
                    varComponentStartDtLoop = varComponentStartDtLoop.AddMonths(Convert.ToInt32(_row["COMPONENT_FREQUENCY"].ToString()));
                    varComponentEndDtLoop = varComponentStartDtLoop.AddMonths(Convert.ToInt32(_row["COMPONENT_FREQUENCY"].ToString())).AddDays(-1);

                    x++;
                    if (x > 200)
                        break;
                }

                _Command.CommandText = "select id,sum(amount_payble) as amount_payble,sum(amount_paid) as amount_paid,sum(discount) as discount,paid_date from collect_component_master where student_id = '" + _studentID + "' and component_id = '" + _row["COMPONENT_ID"].ToString() + "' and MAPPED_DATE = '" + varComponentStartDtLoop.ToString("yyyy-MM-dd") + "'";

                _DtReader = _Command.ExecuteReader();
                if (_DtReader.HasRows)
                {
                    while (_DtReader.Read())
                    {
                        if (_DtReader["id"] != DBNull.Value)
                        {
                            varID = Convert.ToInt64(_DtReader["id"]);
                            if (Convert.ToString(_DtReader["paid_date"]).Length > 0)
                            {
                                varDueAmountPayble = varDueAmountPayble + Convert.ToInt32(_DtReader["amount_payble"]);
                                varDueAmountPaid = varDueAmountPaid + Convert.ToInt32(_DtReader["amount_paid"]) + Convert.ToInt32(_DtReader["discount"]);
                            }
                            else
                            {
                                varAmountPayble = Convert.ToInt32(_DtReader["amount_payble"]);
                                varDiscount = Convert.ToInt32(_DtReader["discount"]);
                            }
                        }

                    }
                }
                else
                { return; } _DtReader.Close();
                _Command.CommandText = "select id,sum(amount_payble) as amount_payble,sum(amount_paid) as amount_paid,sum(discount) as discount from collect_component_master where student_id = '" + _studentID + "' and component_id = '" + _row["COMPONENT_ID"].ToString() + "' and MAPPED_DATE < '" + varComponentStartDtLoop.ToString("yyyy-MM-dd") + "'";
                _DtReader = _Command.ExecuteReader();
                if (_DtReader.HasRows)
                {
                    while (_DtReader.Read())
                    {
                        if (_DtReader["amount_payble"] != DBNull.Value)
                        {
                            varDueAmountPayble = varDueAmountPayble + Convert.ToInt32(_DtReader["amount_payble"]);
                            varDueAmountPaid = varDueAmountPaid + Convert.ToInt32(_DtReader["amount_paid"]) + Convert.ToInt32(_DtReader["discount"]);
                        }
                    }
                }
                _DtReader.Close();

                _dtblFeeDescriptionRow["COMPONENT_AMOUNT"] = varAmountPayble;
                _dtblFeeDescriptionRow["DUE_AMOUNT"] = varDueAmountPayble - varDueAmountPaid;
                _dtblFeeDescriptionRow["COMPONENT_ID"] = _row["COMPONENT_ID"].ToString();
                _dtblFeeDescriptionRow["COMPONENT_NAME"] = _row["COMPONENT_NAME"].ToString() + " (" + varFeesDuration + ")";
                _dtblFeeDescriptionRow["COMPONENT_DETAIL_ID"] = _row["COMPONENT_DETAIL_ID"].ToString();
                _dtblFeeDescriptionRow["ID"] = varID;
                _dtblFeeDescriptionRow["DISCOUNT"] = varDiscount;
                _dtblFeeDescription.Rows.Add(_dtblFeeDescriptionRow);
            }
            _Command.CommandText = "select distinct component_id from collect_component_master where student_id = '" + _studentID + "' and component_id not in (select distinct component_id from component_detail where component_detail_id in(select component_detail_id from student_component_mapping where student_id = '" + _studentID + "') ) group by component_id having (sum(amount_payble)- sum(amount_paid))<>0";
            _DtReader = _Command.ExecuteReader();
            while (_DtReader.Read())
            {
                objArrayListNotMappedId.Add(_DtReader["component_id"]);
            }
            _DtReader.Close();
            foreach (string s in objArrayListNotMappedId)
            {
                DataRow _dtblFeeDescriptionRow = _dtblFeeDescription.NewRow();
                _Command.CommandText = "select COMPONENT_NAME from component_master where component_id = '" + s + "'";
                _DtReader = _Command.ExecuteReader();
                while (_DtReader.Read())
                {
                    _dtblFeeDescriptionRow["COMPONENT_ID"] = s;
                    _dtblFeeDescriptionRow["COMPONENT_NAME"] = Convert.ToString(_DtReader["COMPONENT_NAME"]);
                    _dtblFeeDescriptionRow["COMPONENT_AMOUNT"] = "0";
                }
                _DtReader.Close();

                _Command.CommandText = "select sum(amount_payble) as amount_payble,sum(amount_paid) as amount_paid,id from collect_component_master where component_id = '" + s + "' and student_id = '" + _studentID + "'";
                _DtReader = _Command.ExecuteReader();
                while (_DtReader.Read())
                {
                    _dtblFeeDescriptionRow["DUE_AMOUNT"] = Convert.ToInt32(_DtReader["amount_payble"]) - Convert.ToInt32(_DtReader["amount_paid"]);
                    _dtblFeeDescriptionRow["ID"] = Convert.ToString(_DtReader["id"]);
                    _dtblFeeDescriptionRow["COMPONENT_DETAIL_ID"] = "";
                }
                _DtReader.Close();
                _dtblFeeDescription.Rows.Add(_dtblFeeDescriptionRow);
            }
            gvFeeAmountDetails.DataSource = _dtblFeeDescription; gvFeeAmountDetails.DataBind();

            int varTotalAmount = 0, varTotalDue = 0, varTotalDiscount = 0, varTotalPayment = 0;
            foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
            {
                Label lblDiscount = (Label)_row.FindControl("lblDiscount");
                TextBox txtPayment = (TextBox)_row.FindControl("txtPayment");
                int varAmt = (Convert.ToInt32(_row.Cells[2].Text) + Convert.ToInt32(_row.Cells[3].Text) - Convert.ToInt32(lblDiscount.Text));
                if (varAmt <= 0) { txtPayment.Text = "0"; } else { txtPayment.Text = varAmt.ToString(); }
                varTotalAmount = varTotalAmount + Convert.ToInt32(_row.Cells[2].Text);
                varTotalDue = varTotalDue + Convert.ToInt32(_row.Cells[3].Text);
                varTotalDiscount = varTotalDiscount + Convert.ToInt32(lblDiscount.Text);
                varTotalPayment = varTotalPayment + Convert.ToInt32(txtPayment.Text);
            }
            lblTotalAmount.Text = "<b>Total Amount: </b>" + Convert.ToString(varTotalAmount);
            lblTotalDue.Text = "<b>Total Due: </b>" + Convert.ToString(varTotalDue);
            lblTotalDiscount.Text = "<b>Total Discount: </b>" + Convert.ToString(varTotalDiscount);
            lblTotalToBePaid.Text = "<b>Total to be Paid: </b>" + Convert.ToString(varTotalPayment);
            lblTotalPaid.Text = "<b>Total Paid: </b>" + Convert.ToString((varTotalAmount + varTotalDue) - varTotalDiscount);
            btnVerify.Visible = true; btnReset.Visible = true; btnSubmit.Visible = true;
        }
        else
        {
            lblTotalAmount.Text = "";
            lblTotalDue.Text = "";
            lblTotalDiscount.Text = "";
            lblTotalToBePaid.Text = "";
            lblTotalPaid.Text = "";
            btnVerify.Visible = false; btnReset.Visible = false; btnSubmit.Visible = false;
        }
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    

        {
            PrintCalculation(); btnSubmit.Visible = true;

            var Totalamountpayable = 0.0; var totaldue = 0.0; var TotalDiscount = 0.0;
            int i = 0; var A_TotalDiscount = 0.0; var Totalpayment = 0.0;
            var admition_no = txtAdmissionNo.Text;
            var mode = ddlSelectPaymentMode.SelectedItem;
            var checkno = txtChequeNo.Text;
            var checkdate = txtChequeDate.Text;
            var bankdetail = txtBankDetails.Text;
            var reciptNO = txtScrollingNumber.Text;
            var firstdate = ddlStartMonth.SelectedItem;
            var lastdate = ddlEndMonth.SelectedItem;
            var fine = txtFineAmount.Text;
            var readmitionFine = txtReAdmissionCharges.Text;

            DataTable dt = new DataTable();
            dt.Columns.Add("Component");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Due");
            dt.Columns.Add("Discount");
            dt.Columns.Add("A.Discount");
            dt.Columns.Add("Payment");
            foreach (GridViewRow row in gvFeeAmountDetails.Rows)
            {
                var tablerow = dt.NewRow();
                Label lblDiscount = (Label)row.FindControl("lblDiscount");
                TextBox txtPayment = (TextBox)row.FindControl("txtPayment");
                TextBox txtADiscount = (TextBox)row.FindControl("txtADiscount");
                var component = gvFeeAmountDetails.Rows[i].Cells[1].Text;
                var amount = gvFeeAmountDetails.Rows[i].Cells[2].Text;
                var due = gvFeeAmountDetails.Rows[i].Cells[3].Text;
                var Discount = lblDiscount.Text;  /////////pannding to show the values
                var A_discount = Convert.ToDouble(txtADiscount.Text);
                var payment = Convert.ToDouble(txtPayment.Text);
                Totalamountpayable += Convert.ToDouble(row.Cells[2].Text);
                totaldue += Convert.ToDouble(row.Cells[3].Text);
                TotalDiscount += Convert.ToDouble(lblDiscount.Text);
                A_TotalDiscount += Convert.ToDouble(txtADiscount.Text);
                Totalpayment += Convert.ToDouble(txtPayment.Text);
                tablerow["Component"] = Convert.ToString(component);
                tablerow["Amount"] = Convert.ToString(amount);
                tablerow["Due"] = Convert.ToString(due);
                tablerow["Discount"] = Convert.ToString(Discount);
                tablerow["A.Discount"] = Convert.ToString(A_discount);
                tablerow["Payment"] = Convert.ToString(payment);
                dt.Rows.Add(tablerow);
                dt.AcceptChanges();
                i++;

            }
            ViewState["feecomponentWise"] = dt;
            Session["TotalDiscount"] = TotalDiscount;
            Session["Totalamountpayable"] = Totalamountpayable;
            Session["totaldue"] = totaldue;
            Session["A_TotalDiscount"] = A_TotalDiscount;
            Session["Totalpayment"] = Totalpayment;
            Session["admitionNO"] = admition_no;
            Session["mode"] = mode;
            Session["checkno"] = checkno;
            Session["checkdate"] = checkdate;
            Session["bankdetail"] = bankdetail;
            Session["Feecomponent"] = ViewState["feecomponentWise"];
            Session["reciptNO"] = reciptNO;
            Session["firstdate"] = firstdate;
            Session["lastdate"] = lastdate;
            Session["fine"] = fine;
            Session["readmitionFine"] = readmitionFine;

        }

    protected void btnReset_Click(object sender, EventArgs e)
    {

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "window.location.href='collectFeeAdmissionNo.aspx';", true);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {



        if (flagi == 0)
        {
            if (hfTotalAmountPaid.Value.Equals("0"))
            {

                // Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow" "alert('Fee Collected !!!');", true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Fee Already Collected !!!'); window.open='StudentFeeReceipt.aspx';", true);


            }
            else
            {
                //btnSubmit.Visible = false;
                int varStudentID = Convert.ToInt32(lblStudentID.Text);
                int ComponentID, ComponentFrequency, ComponentDetailID = 0;
                string sQL = "";
                OdbcDataReader _dtReader = null;
                double varAmountPayable, varAmountPaid, varDiscount, varDueAmount, varActualAmountPaid = 0.0;
                DateTime varMappedDate = DateTime.Now;
                DateTime varFeeStartDate = Convert.ToDateTime(ddlStartMonth.SelectedValue);
                DateTime varFeeEndDate = Convert.ToDateTime(ddlEndMonth.SelectedValue);
                _Command.CommandText = "select max(Rno) as Rno from collect_component_detail";
                _dtReader = _Command.ExecuteReader();
                while (_dtReader.Read())
                {
                    Rno = Convert.ToInt32(_dtReader["Rno"]);

                }
                Rno = Rno + 1;
                _dtReader.Close();

                if (Convert.ToString(txtChequeDate.Text).Trim().Length > 0)
                {
                    if (checkCts.Checked == true)
                    {
                        var cts = "CTS";
                        sQL = "insert into collect_component_detail(STUDENT_ID,AMOUNT_PAID,MODE,CHEQUE_NUMBER,CHEQUE_DATE,BANK_NAME,PAID_DATE,FINE,RE_ADM_CHARGES,SCHOOL_SESSION_ID,CREATE_DATE,CREATE_TIME,Cts,Rno)";
                        sQL += "values('" + varStudentID + "','" + hfTotalAmountPaid.Value + "','" + ddlSelectPaymentMode.SelectedValue + "','" + txtChequeNo.Text.Trim() + "','" + Convert.ToDateTime(txtChequeDate.Text).ToString("yyyy-MM-dd") + "','" + txtBankDetails.Text + "','" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + txtFineAmount.Text + "','" + txtReAdmissionCharges.Text + "', '" + Convert.ToString(Session["_SessionID"]) + "',now(),now(),'" + cts + "','" + Rno + "')";
                        _Command.CommandText = sQL; _Command.ExecuteNonQuery();
                    }
                    if (checkCts.Checked == false)
                    {
                        var cts = "NON CTS";
                        sQL = "insert into collect_component_detail(STUDENT_ID,AMOUNT_PAID,MODE,CHEQUE_NUMBER,CHEQUE_DATE,BANK_NAME,PAID_DATE,FINE,RE_ADM_CHARGES,SCHOOL_SESSION_ID,CREATE_DATE,CREATE_TIME,Cts,Rno)";
                        sQL += "values('" + varStudentID + "','" + hfTotalAmountPaid.Value + "','" + ddlSelectPaymentMode.SelectedValue + "','" + txtChequeNo.Text.Trim() + "','" + Convert.ToDateTime(txtChequeDate.Text).ToString("yyyy-MM-dd") + "','" + txtBankDetails.Text + "','" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + txtFineAmount.Text + "','" + txtReAdmissionCharges.Text + "', '" + Convert.ToString(Session["_SessionID"]) + "',now(),now(),'" + cts + "','" + Rno + "')";
                        _Command.CommandText = sQL; _Command.ExecuteNonQuery();

                    }
                }
                else
                {
                    //sQL = "insert into collect_component_detail(STUDENT_ID,AMOUNT_PAID,MODE,BANK_NAME,PAID_DATE,FINE,RE_ADM_CHARGES,SCHOOL_SESSION_ID,CREATE_DATE,CREATE_TIME,Rno)";
                    //sQL += "values('" + varStudentID + "','" + hfTotalAmountPaid.Value + "','" + ddlSelectPaymentMode.SelectedValue + "','" + txtBankDetails.Text + "','" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + txtFineAmount.Text + "','" + txtReAdmissionCharges.Text + "', '" + Convert.ToString(Session["_SessionID"]) + "',now(),now(),'" + Rno + "')";
                    //_Command.CommandText = sQL; _Command.ExecuteNonQuery();

                }
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

                    int ScrollingNumber = Convert.ToInt32(txtScrollingNumber.Text);
                    int ScrollingMonth = Convert.ToDateTime(txtCalculationDate.Text).Month;

                    varActualAmountPaid = Convert.ToDouble(txtPayment.Text);

                    varFeeStartDate = Convert.ToDateTime(ddlStartMonth.SelectedValue);
                    varFeeEndDate = Convert.ToDateTime(ddlEndMonth.SelectedValue);

                    //sQL = "delete from collect_component_master where amount_payble=0 and Student_id='" + varStudentID + "' and component_id='" + ComponentID + "' and mapped_date='" + varFeeEndDate.ToString("yyyy-MM-dd") + "';";
                    //_Command.CommandText = sQL; _Command.ExecuteScalar();

                    if (Convert.ToDouble(txtADiscount.Text) > 0)
                    {
                        //sQL = "insert into collect_component_master(Student_id,component_id,amount_payble,amount_paid,discount,paid_date,mapped_date,school_session_id) values('" + varStudentID + "','" + ComponentID + "',0,0," + Convert.ToDouble(txtADiscount.Text) + ",'" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "','" + varFeeEndDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(Session["_SessionID"]) + "');";
                        //_Command.CommandText = sQL; _Command.ExecuteScalar();
                    }

                    if (varActualAmountPaid > 0)
                    {
                        while (varFeeStartDate <= varFeeEndDate)
                        {
                            varDueAmount = 0.0;
                            sQL = "CALL `spDueAmountMonthWiseFromStudentComponentSessionIDAndMappedDate`('" + varStudentID + "', '" + ComponentID + "','" + varFeeStartDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
                            _Command.CommandText = sQL; //if (i == 2) { Response.Write(sQL); Response.End(); }
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

                        //---------------------------

                    }

                }

                varFeeStartDate = Convert.ToDateTime(ddlStartMonth.SelectedValue);
                varFeeEndDate = Convert.ToDateTime(ddlEndMonth.SelectedValue);

                int varSelectedStartingMonthIndex = Convert.ToInt32(ddlStartMonth.SelectedIndex);
                int varSelectedEndingMonthIndex = Convert.ToInt32(ddlEndMonth.SelectedIndex);

                int ScrollingNumberr = Convert.ToInt32(txtScrollingNumber.Text);
                int ScrollingMonthh = Convert.ToDateTime(txtCalculationDate.Text).Month;

                string NewScrollingNumbers = "";

                if (Convert.ToDateTime(varFeeStartDate).Month.Equals(2) && Convert.ToDateTime(varFeeEndDate).Month.Equals(3))
                {
                    ScrollingNumberr = Convert.ToInt32(txtScrollingNumber.Text);
                    sQL = "update collect_component_master set SCROLL_NO=" + ScrollingNumberr + ", SCROLL_MONTH=2, DETAIL_ID=" + ComponentDetailID + " where STUDENT_ID='" + varStudentID + "' and MAPPED_DATE='" + varFeeStartDate.ToString("yyyy-MM-dd") + "' and PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' and SCHOOL_SESSION_ID = '" + Convert.ToString(Session["_SessionID"]) + "';";
                    _Command.CommandText = sQL; _Command.ExecuteNonQuery();
                    sQL = "update collect_component_master set SCROLL_NO=" + ScrollingNumberr + ", SCROLL_MONTH=2, DETAIL_ID=" + ComponentDetailID + " where STUDENT_ID='" + varStudentID + "' and MAPPED_DATE='" + varFeeStartDate.AddMonths(1).ToString("yyyy-MM-dd") + "' and PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' and SCHOOL_SESSION_ID = '" + Convert.ToString(Session["_SessionID"]) + "';";
                    _Command.CommandText = sQL; _Command.ExecuteNonQuery();

                }
                else if (Convert.ToDateTime(varFeeStartDate).Month.Equals(3) && Convert.ToDateTime(varFeeEndDate).Month.Equals(3))
                {
                    ScrollingNumberr = Convert.ToInt32(txtScrollingNumber.Text);
                    sQL = "update collect_component_master set SCROLL_NO=" + ScrollingNumberr + ", SCROLL_MONTH=3, DETAIL_ID=" + ComponentDetailID + " where STUDENT_ID='" + varStudentID + "' and MAPPED_DATE='" + varFeeStartDate.AddMonths(1).ToString("yyyy-MM-dd") + "' and PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' and SCHOOL_SESSION_ID = '" + Convert.ToString(Session["_SessionID"]) + "';";
                    _Command.CommandText = sQL; _Command.ExecuteNonQuery();
                }
                else
                {
                    if (Convert.ToDateTime(varFeeEndDate).Month.Equals(3))
                    {
                        ScrollingNumberr = Convert.ToInt32(txtScrollingNumber.Text);
                        while (varSelectedStartingMonthIndex <= varSelectedEndingMonthIndex - 1)
                        {
                            sQL = "update collect_component_master set SCROLL_NO=" + ScrollingNumberr + ", SCROLL_MONTH=" + ScrollingMonthh + ", DETAIL_ID=" + ComponentDetailID + " where STUDENT_ID='" + varStudentID + "' and MAPPED_DATE='" + Convert.ToDateTime(ddlStartMonth.Items[varSelectedStartingMonthIndex].Value).ToString("yyyy-MM-dd") + "' and PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' and SCHOOL_SESSION_ID = '" + Convert.ToString(Session["_SessionID"]) + "';";
                            _Command.CommandText = sQL; _Command.ExecuteNonQuery();
                            varSelectedStartingMonthIndex += 1; ScrollingNumberr += 1; ScrollingMonthh += 1;
                        } NewScrollingNumbers = NewScrollingNumbers.Substring(0, NewScrollingNumbers.Length - 1);

                    }
                    else
                    {
                        while (varSelectedStartingMonthIndex <= varSelectedEndingMonthIndex)
                        {
                            sQL = "update collect_component_master set SCROLL_NO=" + ScrollingNumberr + ", SCROLL_MONTH=" + ScrollingMonthh + ", DETAIL_ID=" + ComponentDetailID + " where STUDENT_ID='" + varStudentID + "' and MAPPED_DATE='" + Convert.ToDateTime(ddlStartMonth.Items[varSelectedStartingMonthIndex].Value).ToString("yyyy-MM-dd") + "' and PAID_DATE ='" + Convert.ToDateTime(txtCalculationDate.Text).ToString("yyyy-MM-dd") + "' and SCHOOL_SESSION_ID = '" + Convert.ToString(Session["_SessionID"]) + "';";
                            _Command.CommandText = sQL; _Command.ExecuteNonQuery();
                            varSelectedStartingMonthIndex += 1; ScrollingNumberr += 1; ScrollingMonthh += 1;
                        }

                    }

                }
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "alert('Fee Collected !!!');window.open('StudentFeeReceipt.aspx','_newtab');", true);

            }
            flagi = 1;
        }
        else

        { }
        //  Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Fee Collected !!!'); window.open='StudentFeeReceipt.aspx';", true);


    }
}