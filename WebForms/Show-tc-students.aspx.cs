using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_Show_tc_students : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
        }
        if (!IsPostBack)
        {
            bnddlclass();
        }
    }

    public void bnddlclass()
    {
        DataTable dt1 = new DataTable();
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select  class_code, concat(class_name,' ',class_section) as Class from  ign_class_master", _Connection));
        ddlcls.DataTextField = "Class";
        ddlcls.DataValueField = "class_code";
        odbc.Fill(dt1);
        ddlcls.DataSource = dt1;
        ddlcls.DataBind();
        ddlcls.Items.Insert(0, "--SELECT--");
    }

    public void bndgrid()
    {
        DataTable dt1 = new DataTable();
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select a.student_id as Id, a.student_registration_nbr as AdmissionNo, concat(a.FIRST_NAME,' ',a.MIDDLE_NAME,' ',a.LAST_NAME) as Name,concat(c.CLASS_NAME,' ',c.CLASS_SECTION) as Class, a.FATHER_NAME,a.MOTHER_NAME,date_format(a.LEFT_ON_DATE,'%e-%M-%y') LeftDate  from ign_tc_student_master a, ign_class_master c   where a.Student_id=a.STUDENT_ID  and a.CLASS_CODE='" + ddlcls.SelectedValue + "' and a.CLASS_CODE=c.CLASS_CODE", _Connection));

        odbc.Fill(dt1);
        grddetail.DataSource = dt1;
        grddetail.DataBind();

    }
    protected void ddlcls_SelectedIndexChanged(object sender, EventArgs e)
    {
        bndgrid();
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string varStudentId = "";
            foreach (GridViewRow gridrow in grddetail.Rows)
            {
                CheckBox CheckBox1 = (CheckBox)gridrow.FindControl("CheckBox1");
                if (CheckBox1.Checked == true)
                {
                    varStudentId = gridrow.Cells[1].Text.Trim();
                    _Command.CommandText = "insert into ign_student_master select * from ign_tc_student_master where student_id = '" + varStudentId + "'";
                    _Command.ExecuteNonQuery();

                    //objCommand.CommandText = "update ign_student_master set update_by = '"+varSessionUserName+"', update_date = now(), update_time= now()  where student_id = '" + varStudentId + "'";
                    //objCommand.ExecuteNonQuery();


                    _Command.CommandText = "delete from ign_tc_student_master where student_id = '" + varStudentId + "'"; ;
                    _Command.ExecuteNonQuery();
                }
            }
            string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Restored'); window.location.href = 'Show-tc-students.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
            Response.Write(varSubmitMessage);
        }
        catch (Exception ex)
        {
            Response.Redirect(@"../logout.aspx");
        }
    }
}
