using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_MarkChequeBounce : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; DateTime dte;
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
        var SQL = "CALL `spAllChequeDetailsFromChequeNo`('" + Convert.ToString(txtChequeNo.Text).Trim() + "')";
        _Command.CommandText = SQL;
        var _dtAdapter = new OdbcDataAdapter(); _dtAdapter.SelectCommand = _Command;
        var _dtblRecords=new DataTable();_dtAdapter.Fill(_dtblRecords);
        rpChequeDetails.DataSource = _dtblRecords; rpChequeDetails.DataBind();
        if (_dtblRecords.Rows.Count > 0)
        {
            foreach (RepeaterItem _item in rpChequeDetails.Items)
            {
                HiddenField hfBounceStatus = (HiddenField)_item.FindControl("hfBounceStatus");
                CheckBox cbMarkBounce = (CheckBox)_item.FindControl("cbMarkBounce");
               
                if (Convert.ToString(hfBounceStatus.Value).ToUpper().Trim().Equals("Y"))
                {
                    cbMarkBounce.Checked = true; cbMarkBounce.Enabled = false;
                }
            } btnSubmit.Visible = true;
        }
        else
        {
            btnSubmit.Visible = false;
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('No Record found !!!');", true);
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string varStudentID = "", varRandomNumbr = "";
        foreach (RepeaterItem _item in rpChequeDetails.Items)
        {
            CheckBox cbMarkBounce = (CheckBox)_item.FindControl("cbMarkBounce");
            if (cbMarkBounce.Enabled)
            {
                if (cbMarkBounce.Checked)
                {
                    _Command.CommandText = "select STUDENT_ID,RAND_NUM from collect_component_detail where ID=?";
                    _Command.Parameters.AddWithValue("ID", Convert.ToString(((HiddenField)_item.FindControl("hfID")).Value));
                    OdbcDataReader _dtReader = _Command.ExecuteReader();
                    while (_dtReader.Read())
                    {
                        varStudentID = Convert.ToString(_dtReader["STUDENT_ID"]);
                        varRandomNumbr = Convert.ToString(_dtReader["RAND_NUM"]);
                    } _dtReader.Close();
                    _Command.Parameters.Clear();

                    _Command.CommandText = "select MAPPED_DATE from collect_component_master where STUDENT_ID=? and RAND_NUM=?";
                    _Command.Parameters.AddWithValue("STUDENT_ID", varStudentID);
                    _Command.Parameters.AddWithValue("RAND_NUM", varRandomNumbr);
                    DateTime varMappedDate = Convert.ToDateTime(_Command.ExecuteScalar());
                    _Command.Parameters.Clear();

                    Label date = (Label)_item.FindControl("lblChequeDate");
                    dte = Convert.ToDateTime(date.Text.Trim());


                    _Command.CommandText = "update collect_component_master set AMOUNT_PAID='0', PAID_DATE=null, RAND_NUM=null, FEE_CREATE_DATE=null, FEE_CREATE_TIME=null where STUDENT_ID='"+varStudentID+"' and paid_date='"+dte.ToString("yyyy-MM-dd")+"'";
                    _Command.Parameters.AddWithValue("STUDENT_ID", varStudentID);
                    _Command.Parameters.AddWithValue("RAND_NUM", varRandomNumbr);
                    _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
                    
                    _Command.CommandText = "update collect_component_detail set amount_paid='0', BOUNCE_STATUS='Y', BOUNCE_DATE=now(), FINE='0' where student_id='"+varStudentID+"' ";
                    _Command.Parameters.AddWithValue("MAPPED_DATE", varMappedDate.ToString("yyyy-MM-dd"));
                    _Command.Parameters.AddWithValue("ID", Convert.ToString(((HiddenField)_item.FindControl("hfID")).Value));
                    _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
                }
            }
        }
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Processing completed !!!'); window.location.href='MarkChequeBounce.aspx';", true);
    }
}