using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_sear___chstudentanything : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    OdbcConnection _Connection = null; OdbcCommand _Command = null; int scrollno;
    String newselect; OdbcDataReader reader;
    protected void Page_Load(object sender, EventArgs e)
    {
        _Connection = (OdbcConnection)Session["_Connection"];
        _Command = new OdbcCommand();
        _Command.Connection = _Connection;

        if (Page.IsPostBack == false)
        {
            grid();
        }
    }


    public void grid()
    {
       
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select * from ign_student_master",_Connection));
       
        odbc.Fill(dt);
       
        //GridView1.DataSource = dt;
        //GridView1.DataBind();
       
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBox1.Text.Trim() == "")
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Please Enter Text To Search');", true);
        else
        {

            dt = new DataTable();
            DataTable dt1 = new DataTable();
            //OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select a.STUDENT_REGISTRATION_NBR as Adm_No,   concat(a.FIRST_NAME,' ',a.MIDDLE_NAME,' ',a.LAST_NAME) as Name, a.FATHER_NAME,a.MOTHER_NAME, date_format( a.BIRTH_DATE,'%e-%M-%Y') as Birth_Date  from ign_student_master a", _Connection));
            OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select a.STUDENT_REGISTRATION_NBR as Adm_No,concat(a.FIRST_NAME,' ',a.MIDDLE_NAME,' ',a.LAST_NAME) as Name,concat(b.CLASS_NAME,'-',b.CLASS_SECTION) as class, a.FATHER_NAME,a.MOTHER_NAME, date_format( a.BIRTH_DATE,'%e-%M-%Y') as Birth_Date from ign_student_master a,ign_class_master b where a.CLASS_CODE=b.CLASS_CODE", _Connection));

            odbc.Fill(dt);
       
            dt1 = myclass.searchDataTable(TextBox1.Text, dt);
            GridView1.DataSource = dt1;
            GridView1.DataBind();
            if (dt1.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('No Record Found');", true);
                GridView1.DataSource = dt1;
                GridView1.DataBind();
            }
        }
    }

    


    
}