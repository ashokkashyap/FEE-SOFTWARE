using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_studentFeeBill : System.Web.UI.Page
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
            {
                radiobtnNEW_old.SelectedValue = "NEW";
                var _dtblComponents = new DataTable(); var _dtblClasses = new DataTable();
                var _dtAdapter = new OdbcDataAdapter();

                var SQL = "call spClassMaster()";
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblClasses);
                ViewState["_dtblClasses"] = _dtblClasses;
                ddlSelectClass.DataSource = _dtblClasses; ddlSelectClass.DataTextField = "CLS"; ddlSelectClass.DataValueField = "CLASS_CODE"; ddlSelectClass.DataBind(); ddlSelectClass.Items.Insert(0, new ListItem("Select Class", ""));

            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }
    protected void ddlSelectClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSelectClass.SelectedIndex > 0)
        {
            var SQL = "CALL `spStudentDetailsFromClassCode`('" + ddlSelectClass.SelectedValue + "')";
            var _dtblStudentdetails = new DataTable();
            var _dtAdapter = new OdbcDataAdapter(SQL, _Connection);
            _dtAdapter.Fill(_dtblStudentdetails);
            ViewState["_dtblStudentdetails"] = _dtblStudentdetails;
            ddlSelectStudent.DataSource = _dtblStudentdetails; ddlSelectStudent.DataTextField = "SNAME"; ddlSelectStudent.DataValueField = "STUDENT_ID"; ddlSelectStudent.DataBind(); ddlSelectStudent.Items.Insert(0, new ListItem("Select Student", "-1"));

            //if (!IsPostBack)
            //{
            //for (int i = 1; i <= 12; i++)
            //{
            //    DateTime date = new DateTime(1900, i, 1);
            //    ddlMnth.Items.Add(new ListItem(date.ToString("MMMM"), i.ToString()));
            //}
            //ddlMnth.SelectedValue = DateTime.Today.Month.ToString();
            DateTime varSessionStartDate = Convert.ToDateTime(Session["_SessionStartDate"]);
            DateTime varSessionEndDate = Convert.ToDateTime(Session["_SessionEndDate"]);


            //while (varSessionStartDate <= varSessionEndDate)
            //{
            //  ddlMnth.Items.Add(new ListItem(varSessionStartDate.ToString("MMMM - yyyy"), varSessionStartDate.ToString("yyyy-MM-01")));
            //}
            //}
        }
        //else
        //{
        //    ddlSelectStudent.Items.Clear();

        //}
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Session["new"] = radiobtnNEW_old.SelectedValue;

        if (ddlSelectClass.SelectedIndex <= 0)
        {
            ScriptManager.RegisterStartupScript(btnSubmit, this.GetType(), "alert", "alert('Please Select Class')", true);
        }
        if (txtmnth.Text == "")
        {
            ScriptManager.RegisterStartupScript(btnSubmit, this.GetType(), "alert", "alert('Please Select Month')", true);
        }
        else
        {
            Session["datemoth"] = txtmnth.Text.ToString();
            string MNTHNAME = Convert.ToDateTime(txtmnth.Text).ToString("MMM");
            Session["sdate"] = MNTHNAME;

            DateTime sdate = Convert.ToDateTime(txtmnth.Text).AddMonths(2);
            string ENTHNAME = Convert.ToDateTime(sdate).ToString("MMM");
            Session["edate"] = ENTHNAME;

            Session["class"] = Convert.ToString(ddlSelectClass.SelectedValue);

            Session["ddlMnth"] = MNTHNAME;
            //  string month = Convert.ToString(ddlMnth.SelectedItem);

            // month = Convert.ToString(Session["month"]);

            Session["varstat"] = "y";
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "window.open('fill_bill_class_print_pdf.aspx?class_id=" + ddlSelectClass.SelectedValue + "&student_id=" + ddlSelectStudent.SelectedValue + "');", true);

        }
    }



    protected void txtmnth_TextChanged(object sender, EventArgs e)
    {

    }
}