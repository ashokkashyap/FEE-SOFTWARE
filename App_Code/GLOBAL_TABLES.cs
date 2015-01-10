using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
/// <summary>
/// Summary description for GLOBAL_TABLES
/// </summary>
public class GLOBAL_TABLES
{
    DataSet globalobjdtst = new DataSet();

    public DataSet TABLES()
    {
        OdbcConnection _Connection = null; OdbcCommand _Command = null; int scrollno; OdbcDataReader _dtReader = null;
        

        
        _Command = new OdbcCommand();
        _Command.Connection = _Connection;
        _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString);
        _Command.Connection = _Connection;
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("SELECT a.student_id,a.FIRST_NAME,a.LAST_NAME, CONCAT(a.FIRST_NAME,' ',a.middle_name,' ',a.LAST_NAME) AS NAME,a.FATHER_NAME,a.MOTHER_NAME,a.STUDENT_REGISTRATION_NBR,a.ADDRESS_LINE1,    CONCAT(b.CLASS_NAME,'-',b.CLASS_SECTION) AS CLASS FROM ign_student_master a,ign_class_master b  WHERE a.CLASS_CODE=b.CLASS_CODE", _Connection));
        odbc.Fill(globalobjdtst);
        _Connection.Close();
        _Connection.Dispose();
        return globalobjdtst;




    }
}