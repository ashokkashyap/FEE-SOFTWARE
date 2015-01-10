using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Configuration;

public partial class WebForms_Student_ledger : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (txtregsNo.Text == "")
        {
            ScriptManager.RegisterStartupScript(btnsubmit, this.GetType(), "alert", "alert('please fill registration no.')", true);
        }
        else
        {

            Session["Stdid"] = Convert.ToString(txtregsNo.Text);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "window.open('StudentLedgerPrint.aspx?class_id=" + ddlSelectClass.SelectedValue + "&student_id=" + txtregsNo.Text + "');", true);
        }
    }
}