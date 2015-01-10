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

public partial class WebForms_Duplicate_feeRecipt_single : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader objdreader = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            


            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                txtName.Text = Convert.ToString(Session["admno"]);
                btnGetDetails_Click(btnGetDetails, null);
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListName(string prefixText, int count, string contextKey)
    {
        OdbcDataAdapter odap = new OdbcDataAdapter("select distinct a.STUDENT_REGISTRATION_NBR from ign_student_master a,collect_component_master b where a.student_id=b.student_id and a.STUDENT_REGISTRATION_NBR like '" + prefixText + "%'", ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString);
        DataSet ds = new DataSet();
        odap.Fill(ds);
        ArrayList arr = new ArrayList();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arr.Add(dr[0]);
        }
        string[] Items = arr.ToArray(typeof(string)) as string[];
        var s = from k in Items where k.StartsWith(prefixText.ToUpper()) select k;
        return s.ToArray();
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        var SQL = "select STUDENT_REGISTRATION_NBR from ign_student_master where CONCAT(IFNULL(FIRST_NAME,''),' ',IFNULL(MIDDLE_NAME,''),' ',IFNULL(LAST_NAME,'')) = '" + txtName.Text.Trim() + "'";
        _Command.CommandText = SQL;
        var RegNo = txtName.Text;
        //SQL = "call spStudentDetailsfromAdmissionNo('" + RegNo + "')";

        SQL = "select a.student_id, concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as name,b.FATHER_NAME,concat(c.CLASS_NAME,' ',c.CLASS_SECTION) as CLASS,b.STUDENT_REGISTRATION_NBR , a.AMOUNT_PAID,date_format(a.PAID_DATE,'%e-%M-%Y') as pay_date,a.MODE,a.id,a.Rno,concat( min(date_format(d.MAPPED_DATE,'%b')),'-',max(date_format(d.MAPPED_DATE,'%b'))) as months  from collect_component_detail a,collect_component_master d  ,ign_student_master b , ign_class_master c where a.STUDENT_ID=b.STUDENT_ID and c.class_code=b.class_code and b.STUDENT_REGISTRATION_NBR='" + RegNo + "' and a.ID= d.DETAIL_ID group by d.DETAIL_ID order by a.STUDENT_ID";
        var _dtAdapter = new OdbcDataAdapter();
        _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
        var _dtblRecords = new DataTable();
        _dtAdapter.Fill(_dtblRecords);
        gvStudentDetails.DataSource = _dtblRecords; gvStudentDetails.DataBind();
    }
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string recipt_print = "";
        var _btnEdit = (ImageButton)sender;
        var _row = (GridViewRow)_btnEdit.NamingContainer;
        var hfdetailid = (HiddenField)_row.FindControl("hfdetailid");
        Session["detailid"] = Convert.ToString(hfdetailid.Value);
        //var chkstudent = (CheckBox)_row.FindControl("chkstudent");
        //var chkschool = (CheckBox)_row.FindControl("chkschool");
        //var chkboth = (CheckBox)_row.FindControl("chkboth");
        var chllstrecpt = (RadioButtonList)_row.FindControl("chklstRecpt");

        if (chllstrecpt.SelectedValue == "Student")
        {
            recipt_print = "student";
        }
        else if (chllstrecpt.SelectedValue == "School")
        {
            recipt_print = "school";
        }

        else if (chllstrecpt.SelectedValue == "Both")
        {
            recipt_print = "both";
        }

        Session["print"] = Convert.ToString(recipt_print);

        Session["admitionNO"] = txtName.Text;
        var hfStudentID = (HiddenField)_row.FindControl("hfStudentID");

        //Response.Write(hfStudentID.Value); Response.End();
        Session["_STUDENT_ID"] = Convert.ToString(hfStudentID.Value);
        _Command.CommandText = "select a.mode,a.CHEQUE_NUMBER,a.BANK_NAME,a.FINE,a.RE_ADM_CHARGES,date_format(a.CHEQUE_DATE,'%e-%M-%Y') as CHEQUE_DATE,date_format(a.paid_date,'%e-%M-%Y') as paid_date  from collect_component_detail a where a.ID='" + Convert.ToString(hfdetailid.Value) + "'";
        objdreader = _Command.ExecuteReader();
        if (objdreader.Read())
        {

            Session["mode"] = Convert.ToString(objdreader["mode"]);
            Session["checkno"] = Convert.ToString(objdreader["CHEQUE_NUMBER"]);
            Session["checkdate"] = Convert.ToString(objdreader["CHEQUE_DATE"]);
            Session["bankdetail"] = Convert.ToString(objdreader["BANK_NAME"]);
            Session["fine"] = Convert.ToString(objdreader["FINE"]);
            Session["readmitionFine"] = Convert.ToString(objdreader["RE_ADM_CHARGES"]);
            Session["paiddate"] = Convert.ToString(objdreader["paid_date"]);
        }
        objdreader.Close();
        _Command.CommandText = "select min(date_format( a.MAPPED_DATE,'%M-%Y')) as date1 from collect_component_master  a where a.DETAIL_ID='" + Convert.ToString(hfdetailid.Value) + "'";
        objdreader = _Command.ExecuteReader();
        if (objdreader.Read())
        {
            Session["firstdate"] = Convert.ToString(objdreader["date1"]);


        }
        objdreader.Close();
        _Command.CommandText = "select max(date_format( a.MAPPED_DATE,'%M-%Y')) as date2 from collect_component_master  a where a.DETAIL_ID='" + Convert.ToString(hfdetailid.Value) + "'";
        objdreader = _Command.ExecuteReader();
        if (objdreader.Read())
        {
            Session["lastdate"] = Convert.ToString(objdreader["date2"]);
            

        }
        objdreader.Close();

        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('Duplicate_feeRecipt_print_single.aspx','_newtab');", true);

        // Response.Redirect("Student_Duplicate_FeeReceipt.aspx");
    }
    protected void gvStudentDetails_RowCreated(object sender, GridViewRowEventArgs e)
    {
        
        
    }
}