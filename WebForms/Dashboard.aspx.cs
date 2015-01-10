using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_Dashboard : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            _Command.CommandText = "CALL `spGetSchoolDetails`()"; _Command.CommandType = CommandType.StoredProcedure;
            OdbcDataReader _dtReader = _Command.ExecuteReader();
            while (_dtReader.Read())
            {
                lblSchoolName.Text = Convert.ToString(_dtReader["SCHOOL_NAME"]);
            } _dtReader.Close(); _dtReader.Dispose();
            if(! IsPostBack)
            {
                //_Command.CommandText="delete  from collect_component_master  where  date_format(MAPPED_DATE,'%d') >01 and AMOUNT_PAYBLE >0";
                //_Command.ExecuteNonQuery();

            }
        }
    }
}