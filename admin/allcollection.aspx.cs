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
public partial class admin_allcollection : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null, _Command1 = null, _Command2 = null, _Command3 = null, _Command4 = null;
    
    double totalamt = 0.0;
    double tamtwidfine = 0.0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            txtStartDate.Attributes.Add("ReadOnly", "true");
            txtEndDate.Attributes.Add("ReadOnly", "true");
            if (!IsPostBack)
            {

            }
        }
    }
    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {
        OdbcConnection _Connection1 = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect1"].ConnectionString);
        _Connection1.Open();
        _Command1 = new OdbcCommand();
        _Command1.Connection = _Connection1;


        OdbcConnection _Connection2 = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect2"].ConnectionString);
        _Connection2.Open();
        _Command2 = new OdbcCommand();
        _Command2.Connection = _Connection2;

        OdbcConnection _Connection3 = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect3"].ConnectionString);
        _Connection3.Open();
        _Command3 = new OdbcCommand();
        _Command3.Connection = _Connection3;


        OdbcConnection _Connection4 = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect4"].ConnectionString);
        _Connection4.Open();
        _Command4 = new OdbcCommand();
        _Command4.Connection = _Connection4;



        _Command1.CommandText = "select sum(a.AMOUNT_PAID + a.FINE  + a.RE_ADM_CHARGES)  from collect_component_detail a where a.PAID_DATE between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
        
        int i = Convert.ToInt32(_Command1.ExecuteScalar());
        lblspsmhlCollection.Text = i.ToString();

        _Command2.CommandText = "select sum(a.AMOUNT_PAID + a.FINE  + a.RE_ADM_CHARGES)  from collect_component_detail a where a.PAID_DATE between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
        int j = Convert.ToInt32(_Command2.ExecuteScalar());
        lblspsptlCollection.Text = j.ToString();

        _Command3.CommandText = "select sum(a.AMOUNT_PAID + a.FINE  + a.RE_ADM_CHARGES) from collect_component_detail a where a.PAID_DATE between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
        int k = Convert.ToInt32(_Command3.ExecuteScalar());
        lblspschdCollection.Text = k.ToString();

        _Command3.CommandText = "select sum(a.AMOUNT_PAID + a.FINE  + a.RE_ADM_CHARGES)  from collect_component_detail a where a.PAID_DATE between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
        int l = Convert.ToInt32(_Command3.ExecuteScalar());
        lblspsnsrCollection.Text = l.ToString();






        _Command1.CommandText = "select sum(a.DISCOUNT) from collect_component_master a where a.PAID_DATE  between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";

        int m = Convert.ToInt32(_Command1.ExecuteScalar());
        lblspsmhlDiscount.Text = m.ToString();

        _Command2.CommandText = "select sum(a.DISCOUNT) from collect_component_master a where a.PAID_DATE  between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
        int n = Convert.ToInt32(_Command2.ExecuteScalar());
        lblspsptlDiscount.Text = n.ToString();

        _Command3.CommandText = "select sum(a.DISCOUNT) from collect_component_master a where a.PAID_DATE  between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
        int o = Convert.ToInt32(_Command3.ExecuteScalar());
        lblspschdDiscount.Text = o.ToString();

        _Command3.CommandText = "select sum(a.DISCOUNT) from collect_component_master a where a.PAID_DATE  between '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
        int p = Convert.ToInt32(_Command3.ExecuteScalar());
        lblspsnsrDiscount.Text = p.ToString();


        DateTime varMappedDate = Convert.ToDateTime(txtEndDate.Text);
        
        
        var sQL = "";
        sQL = "CALL `spStudentDetailsFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
       _Command1.CommandText = sQL; _Command1.CommandType = CommandType.StoredProcedure;
            OdbcDataAdapter _dtAdapter = new OdbcDataAdapter(); _dtAdapter.SelectCommand = _Command1;
            DataTable _dtblStudentRecords = new DataTable();
            _dtAdapter.Fill(_dtblStudentRecords);
        int q=0;
        if (_dtblStudentRecords.Rows.Count > 0)
        {

            foreach (DataRow _row in _dtblStudentRecords.Rows)
            {
                if (!Convert.ToString(_row["STUDENT_ID"]).Equals(0))
                {
                    if (!Convert.ToString(_row["Payble"]).Equals("0"))
                    {
                        q = q + 1;
                    }
                }

            }
        }
        lblspsmhlDefaulter.Text = q.ToString();






        sQL = "CALL `spStudentDetailsFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
        _Command2.CommandText = sQL; _Command2.CommandType = CommandType.StoredProcedure;
        OdbcDataAdapter _dtAdapter2 = new OdbcDataAdapter(); _dtAdapter2.SelectCommand = _Command2;
        DataTable _dtblStudentRecords2 = new DataTable();
        _dtAdapter2.Fill(_dtblStudentRecords2);
        int r = 0;
        if (_dtblStudentRecords2.Rows.Count > 0)
        {

            foreach (DataRow _row in _dtblStudentRecords2.Rows)
            {
                if (!Convert.ToString(_row["STUDENT_ID"]).Equals(0))
                {
                    if (!Convert.ToString(_row["Payble"]).Equals("0"))
                    {
                        r = r + 1;
                    }
                }

            }
        }
        lblspsptlDefaulter.Text = r.ToString();






        sQL = "CALL `spStudentDetailsFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
        _Command3.CommandText = sQL; _Command3.CommandType = CommandType.StoredProcedure;
        OdbcDataAdapter _dtAdapter3 = new OdbcDataAdapter(); _dtAdapter3.SelectCommand = _Command3;
        DataTable _dtblStudentRecords3 = new DataTable();
        _dtAdapter3.Fill(_dtblStudentRecords3);
        int s = 0;
        if (_dtblStudentRecords3.Rows.Count > 0)
        {

            foreach (DataRow _row in _dtblStudentRecords3.Rows)
            {
                if (!Convert.ToString(_row["STUDENT_ID"]).Equals(0))
                {
                    if (!Convert.ToString(_row["Payble"]).Equals("0"))
                    {
                        s = s + 1;
                    }
                }

            }
        }
        lblspschdDefaulter.Text = s.ToString();




        sQL = "CALL `spStudentDetailsFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
        _Command4.CommandText = sQL; _Command4.CommandType = CommandType.StoredProcedure;
        OdbcDataAdapter _dtAdapter4 = new OdbcDataAdapter(); _dtAdapter4.SelectCommand = _Command4;
        DataTable _dtblStudentRecords4 = new DataTable();
        _dtAdapter4.Fill(_dtblStudentRecords4);
        int t = 0;
        if (_dtblStudentRecords4.Rows.Count > 0)
        {

            foreach (DataRow _row in _dtblStudentRecords4.Rows)
            {
                if (!Convert.ToString(_row["STUDENT_ID"]).Equals(0))
                {
                    if (!Convert.ToString(_row["Payble"]).Equals("0"))
                    {
                        t = t + 1;
                    }
                }

            }
        }
        lblspsnsrDefaulter.Text = t.ToString();
    }
}