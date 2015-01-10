using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for serviceA
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
 [System.Web.Script.Services.ScriptService]
public class serviceA : System.Web.Services.WebService {

    public serviceA () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return Convert.ToString((new Random()).Next(123456, 987654));
        //return "This method writes this string. !!!";
    }

    [WebMethod]
    public void MarkAllDue(DateTime _DueDate)
    {
        OdbcConnection _Connection; OdbcCommand _Command; OdbcDataReader _dtReader;
        string varSchoolSession = "";
        varSchoolSession = Convert.ToString(Session["_SessionID"]);
        _Connection = (OdbcConnection)Session["_Connection"];
        _Command = new OdbcCommand();
        _Command.Connection = _Connection;

        ArrayList objArrayListStudentId = new ArrayList();
        _Command.CommandText = "select distinct(student_id) from ign_student_master where student_id in (SELECT student_id FROM student_component_mapping where SCHOOL_SESSION_ID = " + varSchoolSession + ")";
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            objArrayListStudentId.Add(Convert.ToString(_dtReader["student_id"]));
        }
        _dtReader.Close();

        foreach (string varStudentId in objArrayListStudentId)
        {
            List<MappedComponentDetails> objListMappedComponent = new List<MappedComponentDetails>();
            _Command.CommandText = "select A.COMPONENT_ID,A.COMPONENT_NAME,A.COMPONENT_FREQUENCY,A.START_MONTH,A.START_YEAR,B.COMPONENT_DETAIL_ID,B.COMPONENT_AMOUNT from component_master A, component_detail B where A.COMPONENT_ID = B.COMPONENT_ID AND A.SCHOOL_SESSION_ID = " + varSchoolSession + " AND B.COMPONENT_DETAIL_ID IN (SELECT COMPONENT_DETAIL_ID FROM student_component_mapping WHERE STUDENT_ID = '" + varStudentId + "' and SCHOOL_SESSION_ID = " + varSchoolSession + ")";
            _dtReader = _Command.ExecuteReader();
            while (_dtReader.Read())
            {
                MappedComponentDetails objMappedComponent = new MappedComponentDetails();
                objMappedComponent.varComponentID = Convert.ToString(_dtReader["COMPONENT_ID"]);
                objMappedComponent.varComponentName = Convert.ToString(_dtReader["COMPONENT_NAME"]);
                objMappedComponent.varComponentFrequency = Convert.ToString(_dtReader["COMPONENT_FREQUENCY"]);
                objMappedComponent.varStartMonth = Convert.ToString(_dtReader["START_MONTH"]);
                objMappedComponent.varStartYear = Convert.ToString(_dtReader["START_YEAR"]);
                objMappedComponent.varComponentDetailID = Convert.ToString(_dtReader["COMPONENT_DETAIL_ID"]);
                objMappedComponent.varComponentAmount = Convert.ToString(_dtReader["COMPONENT_AMOUNT"]);
                objListMappedComponent.Add(objMappedComponent);
            }
            _dtReader.Close();

            foreach (MappedComponentDetails objMappedComponent in objListMappedComponent)
            {
                int varDiscount = 0;
                _Command.CommandText = "select DISCOUNT_TYPE,DISCOUNT_VALUE from student_discount_mapping A,discount_master B where STUDENT_ID = '" + varStudentId + "' and COMPONENT_ID = '" + objMappedComponent.varComponentID + "' and A.DISCOUNT_ID = B.DISCOUNT_ID";
                _dtReader = _Command.ExecuteReader();
                while (_dtReader.Read())
                {
                    if (Convert.ToString(_dtReader["DISCOUNT_TYPE"]) == "FIX")
                    {
                        varDiscount = Convert.ToInt32(_dtReader["DISCOUNT_VALUE"]);
                    }
                    else
                    {
                        varDiscount = (Convert.ToInt32(_dtReader["DISCOUNT_VALUE"]) * Convert.ToInt32(objMappedComponent.varComponentAmount)) / 100;
                    }
                }
                _dtReader.Close();

                DateTime varComponentStartDt = Convert.ToDateTime(objMappedComponent.varStartMonth + "/1/" + objMappedComponent.varStartYear);
                DateTime varComponentEndDt = varComponentStartDt.AddMonths(Convert.ToInt32(objMappedComponent.varComponentFrequency)).AddDays(-1);
                DateTime varComponentStartDtLoop = varComponentStartDt;
                DateTime varComponentEndDtLoop = varComponentEndDt;

                int x = 0;
                while (true)
                {
                    if (Convert.ToDateTime(_DueDate) >= varComponentStartDtLoop && Convert.ToDateTime(_DueDate) <= varComponentEndDtLoop)
                    {
                        _Command.CommandText = "select count(*) from collect_component_master where MAPPED_DATE >= '" + varComponentStartDtLoop.ToString("yyyy-MM-dd") + "' and MAPPED_DATE <= '" + varComponentEndDtLoop.ToString("yyyy-MM-dd") + "' and component_id= '" + objMappedComponent.varComponentID + "' and student_id ='" + varStudentId + "' and SCHOOL_SESSION_ID = '" + varSchoolSession + "'";
                        if (Convert.ToInt32(_Command.ExecuteScalar()) == 0)
                        {
                            _Command.Parameters.AddWithValue("STUDENT_ID", varStudentId);
                            _Command.Parameters.AddWithValue("COMPONENT_ID", objMappedComponent.varComponentID);
                            _Command.Parameters.AddWithValue("AMOUNT_PAYBLE", objMappedComponent.varComponentAmount);
                            _Command.Parameters.AddWithValue("AMOUNT_PAID", "0");
                            _Command.Parameters.AddWithValue("DISCOUNT", varDiscount.ToString());
                            _Command.Parameters.AddWithValue("MAPPED_DATE", varComponentStartDtLoop.ToString("yyyy-MM-dd"));
                            _Command.Parameters.AddWithValue("SCHOOL_SESSION_ID", varSchoolSession);
                            _Command.CommandText = "insert into collect_component_master(STUDENT_ID,COMPONENT_ID,AMOUNT_PAYBLE,AMOUNT_PAID,DISCOUNT,MAPPED_DATE,SCHOOL_SESSION_ID,MAPPED_CREATE_DATE,MAPPED_CREATE_TIME,RAND_NUM) values(?,?,?,?,?,?,?,now(),now(),HelloWorld())";
                            _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
                        }
                        break;
                    }
                    varComponentStartDtLoop = varComponentStartDtLoop.AddMonths(Convert.ToInt32(objMappedComponent.varComponentFrequency));
                    varComponentEndDtLoop = varComponentStartDtLoop.AddMonths(Convert.ToInt32(objMappedComponent.varComponentFrequency)).AddDays(-1);
                    x++;
                    if (x > 200)
                        break;
                }
            }
        }
    }

    [WebMethod]
    public List<string> getStudentsName(char fnameChar)
    {
        DataTable dtblList = new System.Data.DataTable();
        List<string> lsFnames = new List<string>();
        using (OdbcConnection _connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString))
        {
            _connection.Open();
            using (OdbcCommand _cmd = new OdbcCommand())
            {
                _cmd.Connection = _connection;
                _cmd.CommandText = "CALL `spTester`('" + fnameChar + "')";
                using (OdbcDataReader _dtReader = _cmd.ExecuteReader())
                {
                    dtblList.Load(_dtReader);
                }
            }
            _connection.Close();
        }
        if (dtblList.Rows.Count > 0)
        {
            foreach (DataRow item in dtblList.Rows)
            {
                lsFnames.Add(item[0].ToString());
            }
        }
        return lsFnames;
    }
}
