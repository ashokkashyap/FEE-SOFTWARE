using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //using (OdbcConnection _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBCONNECT"].ConnectionString))
        //{
        //    _Connection.Open();
        //    using (OdbcCommand _Command = new OdbcCommand())
        //    {
        //        _Command.Connection = _Connection;
        //        _Command.CommandText = "select * from employeeMaster";
        //        var _DtReader = _Command.ExecuteReader();
        //        while (_DtReader.Read())
        //        {
        //            Response.Write(_DtReader[1]); Response.Write(_DtReader[2]); Response.Write(_DtReader[3]); Response.Write(_DtReader[4]);
        //        } _DtReader.Close();
        //    }
        //}
        Response.Redirect("WebForms/Login.aspx");
    }
}