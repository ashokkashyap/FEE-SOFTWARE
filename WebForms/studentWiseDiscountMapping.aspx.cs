using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_studentWiseDiscountMapping : System.Web.UI.Page
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
            { }
        }
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        

        string SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";

        _Command.CommandText = SQL;
        string admno = Convert.ToString(_Command.ExecuteScalar());

        if (admno == "")
        {
            ScriptManager.RegisterStartupScript(btnGetDetails, this.GetType(), "dd", "alert('Not Valid Admission No')", true);


        }
        else
        {
           
            OdbcDataReader _dtReader = _Command.ExecuteReader();


            while (_dtReader.Read())
            {
                lblStudentID.Text = Convert.ToString(_dtReader["STUDENT_ID"]);
                lblName.Text = Convert.ToString(_dtReader["NAME"]);
                lblClass.Text = Convert.ToString(_dtReader["CLASS"]);
                lblFatherName.Text = Convert.ToString(_dtReader["FATHER_NAME"]);
                lblMotherName.Text = Convert.ToString(_dtReader["MOTHER_NAME"]);
                lblAdmissionNo.Text = Convert.ToString(_dtReader["STUDENT_REGISTRATION_NBR"]);
                lblAddress.Text = Convert.ToString(_dtReader["ADDRESS_LINE1"]);
            } _dtReader.Close(); _dtReader.Dispose();

            DataTable _dtblRecords = new DataTable(); DataTable _dtblDiscounts = new DataTable();
            DataTable _dtblComponents = new DataTable(); DataTable _dtblStudentDiscountMapping = new DataTable();
            DataTable _dtblDiscounters = new DataTable();
            Dictionary<int, double> dicComponentIDAndMaxAmount = new Dictionary<int, double>();

            SQL = "CALL `spGetMaxAmountPayableAndComponentIDFromStudentIDAndSessionID`('" + lblStudentID.Text.Trim() + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
            _Command.CommandText = SQL;
            _dtReader = _Command.ExecuteReader();
            while (_dtReader.Read())
            {
                dicComponentIDAndMaxAmount.Add(Convert.ToInt32(_dtReader["COMPONENT_ID"]), Convert.ToDouble(_dtReader["AMOUNT_PAYBLE"]));
            } _dtReader.Close(); _dtReader.Dispose();

            SQL = "CALL `spComponentMaster`";
            _Command.CommandText = SQL;
            _dtReader = _Command.ExecuteReader();
            _dtblComponents.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();

            SQL = "CALL `spDiscountMaster`()";
            _Command.CommandText = SQL; _dtReader = _Command.ExecuteReader();
            _dtblDiscounts.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();
            ViewState["_dtblDiscounts"] = _dtblDiscounts;

            SQL = "CALL `spStudentDiscountMappingFromStudentIdAndSessionId`('" + lblStudentID.Text.Trim() + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
            _Command.CommandText = SQL; //lblAddress.Text = SQL;
            _dtReader = _Command.ExecuteReader();
            _dtblStudentDiscountMapping.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();

            _Command.CommandText = "CALL `spDiscounterMaster`()";
            _dtReader = _Command.ExecuteReader();
            _dtblDiscounters.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();

            _dtblRecords.Columns.Add("COMPONENT_ID");
            _dtblRecords.Columns.Add("COMPONENT_FREQUENCY");
            _dtblRecords.Columns.Add("COMPONENT_NAME");
            _dtblRecords.Columns.Add("AMOUNT_PAYBLE");
            _dtblRecords.Columns.Add("ID");

            foreach (KeyValuePair<int, double> pair in dicComponentIDAndMaxAmount)
            {
                var MappedComponents = from record in _dtblComponents.AsEnumerable() where Convert.ToInt32(record["COMPONENT_ID"]).Equals(pair.Key) select record;
                if (MappedComponents.Any())
                {
                    foreach (var MComponent in MappedComponents)
                    {
                        DataRow _row = _dtblRecords.NewRow();
                        _row["COMPONENT_ID"] = Convert.ToString(MComponent["COMPONENT_ID"]);
                        _row["COMPONENT_FREQUENCY"] = Convert.ToString(MComponent["COMPONENT_FREQUENCY"]);
                        _row["COMPONENT_NAME"] = Convert.ToString(MComponent["COMPONENT_NAME"]);
                        _row["AMOUNT_PAYBLE"] = Convert.ToString(pair.Value);
                        _row["ID"] = Convert.ToString("");
                        _dtblRecords.Rows.Add(_row);
                    }
                }
            }

            gvComponents.DataSource = _dtblRecords; gvComponents.DataBind();

            foreach (GridViewRow _row in gvComponents.Rows)
            {
                DateTime varSessionStartDate = Convert.ToDateTime(Session["_SessionStartDate"]);
                DateTime varSessionEndDate = Convert.ToDateTime(Session["_SessionEndDate"]);

                HiddenField hfComponentID = (HiddenField)_row.FindControl("hfCOMPONENT_ID");
                HiddenField hfID = (HiddenField)_row.FindControl("hfID");
                int varComponentFrequency = Convert.ToInt32(((HiddenField)_row.FindControl("hfCOMPONENT_FREQUENCY")).Value);
                DropDownList ddlStatus = (DropDownList)_row.FindControl("ddlStatus");
                DropDownList ddlApplicableDate = (DropDownList)_row.FindControl("ddlApplicableDate");
                DropDownList ddlSelectDiscounter = (DropDownList)_row.FindControl("ddlSelectDiscounter");
                Label lblPayableAmount = (Label)_row.FindControl("lblPayableAmount");

                ddlStatus.DataSource = _dtblDiscounts;
                ddlStatus.DataTextField = "DISCOUNT_NAME";
                ddlStatus.DataValueField = "DISCOUNT_ID";
                ddlStatus.DataBind(); ddlStatus.Items.Insert(0, new ListItem("N/A", "-1"));
                ddlSelectDiscounter.DataSource = _dtblDiscounters;
                ddlSelectDiscounter.DataTextField = "NAME";
                ddlSelectDiscounter.DataValueField = "DISCOUNTER_ID";
                ddlSelectDiscounter.DataBind();
                ddlSelectDiscounter.Items.Insert(0, new ListItem("N/A", "0"));

                while (varSessionStartDate <= varSessionEndDate)
                {
                    ddlApplicableDate.Items.Add(new ListItem(varSessionStartDate.ToString("dd-MMM-yyyy"), varSessionStartDate.ToString("dd-MMM-yyyy")));
                    varSessionStartDate = varSessionStartDate.AddMonths(varComponentFrequency);
                };

                var _DiscountDetails = from x in _dtblStudentDiscountMapping.AsEnumerable() where Convert.ToInt32(x["COMPONENT_ID"]).Equals(Convert.ToInt32(hfComponentID.Value)) select x;
                if (_DiscountDetails.Any())
                {
                    foreach (var _Discount in _DiscountDetails)
                    {
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(_Discount["DISCOUNT_ID"].ToString()));
                        ddlSelectDiscounter.SelectedIndex = ddlSelectDiscounter.Items.IndexOf(ddlSelectDiscounter.Items.FindByValue(_Discount["DISCOUNTER_ID"].ToString()));
                        hfID.Value = Convert.ToString(_Discount["ID"]);
                        double DiscountValue, ParentAmount = 0.0;
                        ParentAmount = Convert.ToDouble(_row.Cells[2].Text);
                        var _DiscountType = from y in _dtblDiscounts.AsEnumerable() where Convert.ToInt32(y["DISCOUNT_ID"]).Equals(Convert.ToInt32(_Discount["DISCOUNT_ID"])) select y;
                        foreach (var DiscountTypec in _DiscountType)
                        {
                            if (Convert.ToString(DiscountTypec["DISCOUNT_TYPE"]).Equals("FIX"))
                            {
                                DiscountValue = Convert.ToDouble(DiscountTypec["DISCOUNT_VALUE"]);
                                ParentAmount -= DiscountValue;
                            }
                            else
                            {
                                DiscountValue = Convert.ToDouble(DiscountTypec["DISCOUNT_VALUE"]);
                                ParentAmount -= ((ParentAmount * DiscountValue) / 100);
                            }
                            lblPayableAmount.Text = Convert.ToString(ParentAmount);
                        }
                    }
                    SQL = "CALL `spGetMinDiscountApplicableDateFromStudentSessionAndComponentID`('" + Convert.ToString(lblStudentID.Text) + "', '" + Convert.ToString(Session["_SessionID"]) + "', '" + Convert.ToString(hfComponentID.Value) + "')";
                    _Command.CommandText = SQL;
                    _dtReader = _Command.ExecuteReader();

                    if (_dtReader.HasRows)
                    {
                        while (_dtReader.Read())
                        {
                            DateTime dtApplicableDate = Convert.ToDateTime(_dtReader[0]);
                            ddlApplicableDate.SelectedValue = dtApplicableDate.ToString("dd-MMM-yyyy");
                        }
                    }
                    _dtReader.Close(); _dtReader.Dispose();
                }
                else
                {
                    ddlStatus.SelectedIndex = 0; ddlSelectDiscounter.SelectedIndex = 0;
                    hfID.Value = "-1";
                    lblPayableAmount.Text = Convert.ToString(_row.Cells[2].Text);
                }

                if (_row.Cells[2].Text.Equals("0"))
                {
                    ddlStatus.Enabled = false; ddlApplicableDate.Enabled = false;
                }
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DataTable _dtblDiscounts = new DataTable(); OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
        string SQL = "CALL `spDiscountMaster`()";
        _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command; _dtAdapter.Fill(_dtblDiscounts);
        foreach (GridViewRow _row in gvComponents.Rows)
        {
            SQL = "";
            DateTime varSessionStartDate = Convert.ToDateTime(Session["_SessionStartDate"]);
            DateTime varSessionEndDate = Convert.ToDateTime(Session["_SessionEndDate"]);
            HiddenField hfComponentID = (HiddenField)_row.FindControl("hfCOMPONENT_ID");
            HiddenField hfID = (HiddenField)_row.FindControl("hfID");
            int varComponentFrequency = Convert.ToInt32(((HiddenField)_row.FindControl("hfCOMPONENT_FREQUENCY")).Value);
            DropDownList ddlStatus = (DropDownList)_row.FindControl("ddlStatus");
            DropDownList ddlApplicableDate = (DropDownList)_row.FindControl("ddlApplicableDate");
            DropDownList ddlSelectDiscounter = (DropDownList)_row.FindControl("ddlSelectDiscounter");

            _Command.Parameters.Clear();
            SQL = "select count(*) from student_discount_mapping where STUDENT_ID=? and COMPONENT_ID=? and SCHOOL_SESSION_ID=? and MAPPED_DATE>=?";
            _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
            _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(hfComponentID.Value));
            _Command.Parameters.AddWithValue("SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
            _Command.Parameters.AddWithValue("MAPPED_DATE", Convert.ToDateTime(ddlApplicableDate.SelectedValue).ToString("yyyy-MM-dd"));
            _Command.CommandText = SQL;
            if (Convert.ToInt32(_Command.ExecuteScalar()) > 0)
            {
                _Command.Parameters.Clear();
                if (ddlStatus.SelectedIndex > 0)
                {
                    SQL = "update student_discount_mapping set DISCOUNT_ID=?,DISCOUNTER_ID=? where  STUDENT_ID=? and COMPONENT_ID=? and SCHOOL_SESSION_ID=? and MAPPED_DATE >=?;";
                    _Command.Parameters.AddWithValue("DISCOUNT_ID", Convert.ToString(ddlStatus.SelectedValue));
                    _Command.Parameters.AddWithValue("DISCOUNTER_ID", Convert.ToString(ddlSelectDiscounter.SelectedValue));
                    _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                    _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(hfComponentID.Value));
                    _Command.Parameters.AddWithValue("SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
                    _Command.Parameters.AddWithValue("MAPPED_DATE", Convert.ToDateTime(ddlApplicableDate.SelectedValue).ToString("yyyy-MM-dd"));
                    _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
                }
                else
                {
                    int startIndex = ddlApplicableDate.SelectedIndex;
                    while (startIndex < ddlApplicableDate.Items.Count)
                    {
                        SQL = "delete from student_discount_mapping where STUDENT_ID=? and COMPONENT_ID=? and MAPPED_DATE>=? and SCHOOL_SESSION_ID=?";
                        _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                        _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(hfComponentID.Value));
                        _Command.Parameters.AddWithValue("MAPPED_DATE", Convert.ToDateTime(ddlApplicableDate.Items[startIndex].Value).ToString("yyyy-MM-dd"));
                        _Command.Parameters.AddWithValue("SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
                        _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
                        startIndex++;
                    }
                }
            }
            else
            {
                if (ddlStatus.SelectedIndex > 0)
                {
                    _Command.Parameters.Clear();
                    int startIndex = ddlApplicableDate.SelectedIndex;
                    while (startIndex < ddlApplicableDate.Items.Count)
                    {
                        SQL = "insert into student_discount_mapping(STUDENT_ID,COMPONENT_ID,DISCOUNT_ID,DISCOUNTER_ID,MAPPED_DATE,SCHOOL_SESSION_ID) values(?,?,?,?,?,?)";
                        _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                        _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(hfComponentID.Value));
                        _Command.Parameters.AddWithValue("DISCOUNT_ID", Convert.ToString(ddlStatus.SelectedValue));
                        _Command.Parameters.AddWithValue("DISCOUNTER_ID", Convert.ToString(ddlSelectDiscounter.SelectedValue));
                        _Command.Parameters.AddWithValue("MAPPED_DATE", Convert.ToDateTime(ddlApplicableDate.Items[startIndex].Value).ToString("yyyy-MM-dd"));
                        //Response.Write(Convert.ToDateTime(ddlApplicableDate.Items[startIndex].Value).ToString("yyyy-MM-dd")+"<br/>"); 
                        _Command.Parameters.AddWithValue("SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
                        _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
                        startIndex++;
                    } //Response.End();
                }
                else
                {
                    _Command.Parameters.Clear();
                    int startIndex = ddlApplicableDate.SelectedIndex;
                    while (startIndex < ddlApplicableDate.Items.Count)
                    {
                        SQL = "delete from student_discount_mapping where STUDENT_ID=? and COMPONENT_ID=? and MAPPED_DATE>=? and SCHOOL_SESSION_ID=?";
                        _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                        _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(hfComponentID.Value));
                        _Command.Parameters.AddWithValue("MAPPED_DATE", Convert.ToDateTime(ddlApplicableDate.Items[startIndex].Value).ToString("yyyy-MM-dd"));
                        _Command.Parameters.AddWithValue("SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
                        _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
                        startIndex++;
                    }
                }
            }

            string AmountPayable = Convert.ToString(_row.Cells[2].Text);
            if (!(AmountPayable.Equals("0")))
            {
                int Discount = 0;
                if (ddlStatus.SelectedIndex > 0)
                {
                    var DiscountDetails = from x in _dtblDiscounts.AsEnumerable() where Convert.ToInt32(x["DISCOUNT_ID"]).Equals(Convert.ToInt32(ddlStatus.SelectedValue)) select x;
                    foreach (var _DiscountDetails in DiscountDetails)
                    {
                        if (Convert.ToString(_DiscountDetails["DISCOUNT_TYPE"]).Equals("FIX"))
                        {
                            Discount = Convert.ToInt32(_DiscountDetails["DISCOUNT_VALUE"]);
                        }
                        else
                        {
                            Discount = Convert.ToInt32((Convert.ToInt32(AmountPayable) * Convert.ToInt32(_DiscountDetails["DISCOUNT_VALUE"])) / 100);
                        }
                    }
                }
                _Command.Parameters.Clear();
                SQL = "update collect_component_master set DISCOUNT=? where STUDENT_ID=? and COMPONENT_ID=? and AMOUNT_PAYBLE=? and MAPPED_DATE>=? and SCHOOL_SESSION_ID=?";
                _Command.Parameters.AddWithValue("DISCOUNT", Convert.ToString(Discount));
                _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(hfComponentID.Value));
                _Command.Parameters.AddWithValue("AMOUNT_PAYBLE", Convert.ToString(AmountPayable));
                _Command.Parameters.AddWithValue("MAPPED_DATE", Convert.ToDateTime(ddlApplicableDate.SelectedValue).ToString("yyyy-MM-dd"));
                _Command.Parameters.AddWithValue("SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
                _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
            }
        }
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Mapping updated !!!.'); window.location.href='studentWiseDiscountMapping.aspx';", true);
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlStatus = (DropDownList)sender;
        GridViewRow _row = (GridViewRow)ddlStatus.NamingContainer;
        Label lblPayableAmount = (Label)_row.FindControl("lblPayableAmount");
        if (ddlStatus.SelectedIndex > 0)
        {
            DataTable _dtblDiscounts = (DataTable)ViewState["_dtblDiscounts"];
            double DiscountValue, ParentAmount = 0.0;
            ParentAmount = Convert.ToDouble(_row.Cells[2].Text);
            var _DiscountType = from y in _dtblDiscounts.AsEnumerable() where Convert.ToInt32(y["DISCOUNT_ID"]).Equals(Convert.ToInt32(ddlStatus.SelectedValue)) select y;
            foreach (var DiscountTypec in _DiscountType)
            {
                if (Convert.ToString(DiscountTypec["DISCOUNT_TYPE"]).Equals("FIX"))
                {
                    DiscountValue = Convert.ToDouble(DiscountTypec["DISCOUNT_VALUE"]);
                    ParentAmount -= DiscountValue;
                }
                else
                {
                    DiscountValue = Convert.ToDouble(DiscountTypec["DISCOUNT_VALUE"]);
                    ParentAmount -= ((ParentAmount * DiscountValue) / 100);
                }
                lblPayableAmount.Text = Convert.ToString(ParentAmount);
            }
        }
        else
        {
            lblPayableAmount.Text = Convert.ToString(_row.Cells[2].Text);
        }
    }
   
}