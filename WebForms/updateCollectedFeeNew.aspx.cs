using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;


public partial class WebForms_updateCollectedFeeNew : System.Web.UI.Page
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
                    lblNoOfCommunication.Text = Convert.ToString(_dtReader["NO_OF_COMMUNICATION"]);
                } _dtReader.Close();
            }
            else
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Check details Please !!!'); window.location.href='updateCollectedFeeNew.aspx';", true);
            
            }
        }
        //Response.Write(lblStudentID.Text); Response.End();

        ddlSelectDate.Items.Clear();
        SQL = "CALL `spDistinctCollectionDatesFromStudentID`('" + Convert.ToString(lblStudentID.Text) + "')"; _Command.CommandText = SQL;
        ddlSelectDate.Items.Add(new ListItem("select", ""));
        
        using (var _dtReader = _Command.ExecuteReader())
        {
            while (_dtReader.Read())
            {
                if ( Convert.ToString(_dtReader["PAID_DATE"]).Trim().Length.Equals(0))
                {
                    //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Fee Record Not Found !!!'); window.location.href='updateCollectedFeeNew.aspx';", true);

                }
                else
                {
                ddlSelectDate.Items.Add(new ListItem(Convert.ToDateTime(_dtReader["PAID_DATE"]).ToString("dd-MMMM-yyyy"), Convert.ToDateTime(_dtReader["PAID_DATE"]).ToString("dd-MMMM-yyyy")));
                }
                } _dtReader.Close();
        }

        SQL = "CALL `spComponentMaster`()"; _Command.CommandText = SQL;
        OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
        _dtAdapter.SelectCommand = _Command;
        DataTable _dtblComponents = new DataTable();
        _dtAdapter.Fill(_dtblComponents);
        gvFeeAmountDetails.DataSource = _dtblComponents; gvFeeAmountDetails.DataBind(); gvFeeAmountDetails.Visible = false;
    }
    protected void ddlSelectDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSelectDate.SelectedIndex > 0)
        {

            
            var SQL = "call `spCollectedFeeAllDetailsFromStudentIDAndCollectionDate`('" + Convert.ToString(lblStudentID.Text) + "','" + Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd") + "')";
            _Command.CommandText = SQL;
            OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
            _dtAdapter.SelectCommand = _Command;
            DataTable _dtblCollectedFee = new DataTable();
            _dtAdapter.Fill(_dtblCollectedFee);

            foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
            {
                TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
                string varCOMPONENT_ID = ((HiddenField)_row.FindControl("hfCOMPONENT_ID")).Value;
                var Amount = from amt in _dtblCollectedFee.AsEnumerable() where Convert.ToInt32(amt["COMPONENT_ID"]).Equals(Convert.ToInt32(varCOMPONENT_ID)) select amt["AMOUNT_PAID"];
                if (Amount.Any())
                {
                    foreach (var _Amount in Amount)
                    {
                        txtAmount.Text = Convert.ToString(_Amount);
                    }
                }
                else
                {
                    txtAmount.Text = "0";
                }
            }            

            gvFeeAmountDetails.Visible = true;
            btnVerify.Visible = true;
            btnReset.Visible = false;
            btnSubmit.Visible = true;
        }
        else
        {
            gvFeeAmountDetails.Visible = false;
            btnVerify.Visible = false;
            btnReset.Visible = false;
            btnSubmit.Visible = false;
            foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
            {
                TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
                txtAmount.Text = ""; lblTotalAmount.Text = "";
            }
        }
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
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
            string varCOMPONENT_ID = ((HiddenField)_row.FindControl("hfCOMPONENT_ID")).Value;
            if (Convert.ToString(txtAmount.Text).Trim().Length > 0)
            {
                var sQL = "update collect_component_master set AMOUNT_PAID=?,fee_UPDATE_DATE=now(),fee_UPDATE_TIME=now(),UPDATE_BY=? where STUDENT_ID=? and COMPONENT_ID=? and PAID_DATE=?;";
                _Command.CommandText = sQL;
                _Command.Parameters.AddWithValue("AMOUNT_PAID", Convert.ToString(txtAmount.Text));
                _Command.Parameters.AddWithValue("UPDATE_BY", Convert.ToString(Session["_User"]));
                _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(varCOMPONENT_ID));
                _Command.Parameters.AddWithValue("PAID_DATE", Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd"));
                _Command.ExecuteNonQuery(); _Command.Parameters.Clear();

                var sql = "update collect_component_detail  set AMOUNT_PAID = '" + lblTotalAmount.Text + "' where where STUDENT_ID= '"+Convert.ToString(lblStudentID.Text)+"' and PAID_DATE='"+Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd")+"'";
                _Command.CommandText = sQL; _Command.ExecuteNonQuery();
            }
        }
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Fee Record Updated !!!'); window.location.href='updateCollectedFeeNew.aspx';", true);

    }
}