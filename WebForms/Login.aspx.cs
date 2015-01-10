using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Configuration;
using System.Data;

public partial class WebForms_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        GLOBAL_TABLES gb = new GLOBAL_TABLES();



        DataSet ds = new DataSet();
        ds = gb.TABLES();
        Session["smaster"] = ds;
        OdbcConnection _Connection = null;
        if (ddl_lkg_ukg_senior.SelectedValue.Equals("SR"))
        {

          _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString);
        
        }
        else
        {
            _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnectJR"].ConnectionString);
        
        }

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

               
                Response.Redirect("Dashboard.aspx");
            }
            else
            {
                txtUserName.Text = ""; txtPassword.Text = "";
            }
        }
    }
}