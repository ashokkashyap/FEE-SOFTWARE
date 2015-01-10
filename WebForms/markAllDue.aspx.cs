using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Collections;

public partial class WebForms_markAllDue : System.Web.UI.Page
{
    OdbcConnection _Connection; OdbcCommand _Command; OdbcDataReader _dtReader;
    string varSchoolSession = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        varSchoolSession = Convert.ToString(Session["_SessionID"]);
        _Connection = (OdbcConnection)Session["_Connection"];
        _Command = new OdbcCommand();
        _Command.Connection = _Connection;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        (new serviceA()).MarkAllDue(Convert.ToDateTime(txtDueDate.Text));
    }
}