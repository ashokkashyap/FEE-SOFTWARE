using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;

public partial class admin_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        OdbcConnection _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString);
        _Connection.Open(); Session["_Connection"] = _Connection;

        string varUsername = Convert.ToString(txtUserName.Text).Trim();
        string varPassword = Convert.ToString(txtPassword.Text).Trim();
        string varRole = "", varSchoolSessionID = ""; DateTime varSessionStartDate = DateTime.Now; DateTime varSessionEndDate = DateTime.Now;
        string SQL = "call `spVerifyCredentials`('" + Convert.ToString(varUsername) + "','" + Convert.ToString(varPassword) + "');";
        using (OdbcCommand _Command = new OdbcCommand())
        {
            _Command.Connection = _Connection;
            _Command.CommandText = SQL;
            OdbcDataReader _dtReader = _Command.ExecuteReader();
            if (_dtReader.HasRows)
            {
                while (_dtReader.Read())
                {
                    varRole = Convert.ToString(_dtReader["ROLE"]);
                } _dtReader.Close(); Session["_User"] = Convert.ToString(varUsername); Session["_Role"] = Convert.ToString(varRole);

                SQL = "CALL `spGetSessionDetails`()";
                _Command.CommandText = SQL;
                _dtReader = _Command.ExecuteReader();
                while (_dtReader.Read())
                {
                    varSchoolSessionID = Convert.ToString(_dtReader["SCHOOL_SESSION_ID"]);
                    varSessionStartDate = Convert.ToDateTime(_dtReader["START_DATE"]);
                    varSessionEndDate = Convert.ToDateTime(_dtReader["END_DATE"]);
                } _dtReader.Close(); _dtReader.Dispose(); Session["_SessionID"] = Convert.ToString(varSchoolSessionID); Session["_SessionStartDate"] = Convert.ToString(varSessionStartDate); Session["_SessionEndDate"] = Convert.ToString(varSessionEndDate);

                Response.Redirect("dashboard.aspx");

                //                Response.Redirect("ADMIN/INDEX.aspx");  
            }
            else
            {
                txtUserName.Text = ""; txtPassword.Text = "";
            }
        }
    }
}