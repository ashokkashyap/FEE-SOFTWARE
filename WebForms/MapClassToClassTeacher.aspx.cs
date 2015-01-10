using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_MapClassToClassTeacher : System.Web.UI.Page
{
   OdbcConnection _Connection = null; OdbcCommand _Command = null;

    string school_id;
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
            bnddrpdwn();
            bnddlclass();
        }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        if (ddlteacher.SelectedIndex > 0 && ddlclass.SelectedIndex > 0)
        {
            string teacher = ddlteacher.SelectedItem.ToString();
            string techr_id = ddlteacher.SelectedValue.ToString();
            string clas = ddlclass.SelectedItem.ToString();
            string clas_id = ddlclass.SelectedValue.ToString();
           
            _Command.CommandText = "insert into ign_sub_class_staff_master(teacher_id,class_id,create_by,create_time,create_date) values('" + techr_id + "','" + clas_id + "','" + Convert.ToString(Session["_User"]) + "',now(),now())";
            int i = _Command.ExecuteNonQuery();

            if (i > 0)
            {
                ScriptManager.RegisterStartupScript(btnsubmit, this.GetType(), "Alert", "alert('Record Insert')", true);
            }
        }
    }

    public void bnddrpdwn()
    {

        DataTable dt = new DataTable();
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select  employee_id, first_name from  ign_staff_master", _Connection));
        odbc.Fill(dt);

        ddlteacher.DataSource = dt;
        ddlteacher.DataTextField = "first_Name";
        ddlteacher.DataValueField = "employee_id";
        ddlteacher.DataBind();
        ddlteacher.Items.Insert(0, "--SELECT--");



    }
    public void bnddlclass()
    {
        DataTable dt1 = new DataTable();
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select  class_code, concat(class_name,' ',class_section) as Class from  ign_class_master", _Connection));
        ddlclass.DataTextField = "Class";
        ddlclass.DataValueField = "class_code";
        odbc.Fill(dt1);
        ddlclass.DataSource = dt1;
        ddlclass.DataBind();
        ddlclass.Items.Insert(0, "--SELECT--");
    }
}