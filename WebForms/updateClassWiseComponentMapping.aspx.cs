using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_updateClassWiseComponentMapping : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                string SQL = "call spComponentMaster()";
                DataTable _dtblComponents = new DataTable();
                OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblComponents);
                ViewState["_dtblComponents"] = _dtblComponents;
                ddlSelectComponent.DataSource = _dtblComponents; ddlSelectComponent.DataTextField = "COMPONENT_NAME"; ddlSelectComponent.DataValueField = "COMPONENT_ID"; ddlSelectComponent.DataBind(); ddlSelectComponent.Items.Insert(0, new ListItem("Select Component", ""));
                _dtAdapter.Dispose();
            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }
    protected void ddlSelectComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSelectAmount.Items.Clear(); ddlSelectClass.Items.Clear(); ddlApplicableDate.Items.Clear(); gvMappedStudents.DataSource = null; gvMappedStudents.DataBind();
        if (ddlSelectComponent.SelectedIndex > 0)
        {
            string SQL = "call spComponentDetailMaster('" + ddlSelectComponent.SelectedValue + "')";
            DataTable _dtblComponentdetails = new DataTable();
            OdbcDataAdapter _dtAdapter = new OdbcDataAdapter(SQL, _Connection);
            _dtAdapter.Fill(_dtblComponentdetails);
            ViewState["_dtblComponentdetails"] = _dtblComponentdetails;
            ddlSelectAmount.DataSource = _dtblComponentdetails; ddlSelectAmount.DataTextField = "COMPONENT_AMOUNT"; ddlSelectAmount.DataValueField = "COMPONENT_DETAIL_ID"; ddlSelectAmount.DataBind(); ddlSelectAmount.Items.Insert(0, new ListItem("Select", ""));
        }
    }
    protected void ddlSelectAmount_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSelectClass.Items.Clear(); gvMappedStudents.DataSource = null; gvMappedStudents.DataBind();
        if (ddlSelectAmount.SelectedIndex > 0)
        {
            var SQL = "call spClassMaster()"; var _dtAdapter = new OdbcDataAdapter(); var _dtblClasses = new DataTable();
            _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
            _dtAdapter.Fill(_dtblClasses);
            ViewState["_dtblClasses"] = _dtblClasses;
            ddlSelectClass.DataSource = _dtblClasses; ddlSelectClass.DataTextField = "CLS"; ddlSelectClass.DataValueField = "CLASS_CODE"; ddlSelectClass.DataBind(); ddlSelectClass.Items.Insert(0, new ListItem("Select Class", ""));
        }
    }
    protected void ddlSelectClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlApplicableDate.Items.Clear();
        if (ddlSelectClass.SelectedIndex > 0)
        {
            getStudents(Convert.ToInt32(ddlSelectClass.SelectedValue));
            DataTable _dtblComponents = (DataTable)ViewState["_dtblComponents"];
            var ComponentFrequency = from ComponentDetails in _dtblComponents.AsEnumerable() where Convert.ToInt32(ComponentDetails["COMPONENT_ID"]).Equals(Convert.ToInt32(ddlSelectComponent.SelectedValue)) select ComponentDetails["COMPONENT_FREQUENCY"];
            int Frequency = 0;
            foreach (var _ComponentFrequency in ComponentFrequency) { Frequency = Convert.ToInt32(_ComponentFrequency); }
            if (Frequency > 0)
            {
                if (Convert.ToString(Session["_SessionStartDate"]) != "" && Session["_SessionStartDate"] != null)
                {
                    DateTime varSessionStartDate = Convert.ToDateTime(Session["_SessionStartDate"]);
                    DateTime varSessionEndDate = Convert.ToDateTime(Session["_SessionEndDate"]);
                    while (varSessionStartDate <= varSessionEndDate)
                    {
                        ddlApplicableDate.Items.Add(new ListItem(varSessionStartDate.ToString("dd-MMM-yyyy"), varSessionStartDate.ToString("dd-MMM-yyyy")));
                        varSessionStartDate = varSessionStartDate.AddMonths(Frequency);
                    }
                }
            }

            _dtblComponents.Dispose();
        }
        else
        {
            gvMappedStudents.DataSource = null; gvMappedStudents.DataBind();
        }
    }
    private void getStudents(int ClassCode)
    {
        //string SQL = "CALL `spMappedStudentDetailsFromComponentIDAndClassCode`('" + Convert.ToString(ddlSelectComponent.SelectedValue) + "','" + ClassCode + "')";
        //_Command.CommandText = SQL;
        //DataTable _dtblMappedStudentdetails = new DataTable(); DataTable _dtblUnMappedStudentdetails = new DataTable();
        //OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
        //_dtAdapter.SelectCommand = _Command;
        //_dtAdapter.Fill(_dtblMappedStudentdetails);
        //gvMappedStudents.DataSource = _dtblMappedStudentdetails; gvMappedStudents.DataBind();
        //_dtAdapter.Dispose();
        string SQL = ""; 
        Dictionary<int, DateTime> dicMappingDetails = new Dictionary<int, DateTime>();
        DataTable _dtblMappedStudentdetails = new DataTable();

        _dtblMappedStudentdetails.Columns.Add("STUDENT_ID"); _dtblMappedStudentdetails.Columns.Add("SNAME"); _dtblMappedStudentdetails.Columns.Add("STUDENT_REGISTRATION_NBR"); _dtblMappedStudentdetails.Columns.Add("FATHER_NAME"); _dtblMappedStudentdetails.Columns.Add("AMOUNT_PAYBLE");
        SQL = "CALL `spGetMaxMappedDateFromCollectionMasterClassCodeComponentIDSsID`('" + ClassCode + "', '" + Convert.ToString(ddlSelectComponent.SelectedValue) + "','" + Convert.ToString(Session["_SessionID"]) + "')";
        _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure;
        OdbcDataReader _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            dicMappingDetails.Add(Convert.ToInt32(_dtReader["STUDENT_ID"]), Convert.ToDateTime(_dtReader["MD"]));
        } _dtReader.Close(); _dtReader.Dispose();

        //a.Text = Convert.ToString(dicMappingDetails.Count);
        foreach (KeyValuePair<int, DateTime> pair in dicMappingDetails)
        {
            SQL = "CALL `spStudentDetailsAndAmountPayableFromStudentIDMappedDateAndCID`('" + Convert.ToString(pair.Key) + "','" + Convert.ToDateTime(pair.Value).ToString("yyyy-MM-dd") + "','" + Convert.ToString(ddlSelectComponent.SelectedValue) + "')";
            _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure; _dtReader = _Command.ExecuteReader();
            while (_dtReader.Read())
            {
                DataRow _row = _dtblMappedStudentdetails.NewRow();
                _row["STUDENT_ID"] = Convert.ToString(pair.Key);
                _row["SNAME"] = Convert.ToString(_dtReader["SNAME"]);
                _row["STUDENT_REGISTRATION_NBR"] = Convert.ToString(_dtReader["STUDENT_REGISTRATION_NBR"]);
                _row["FATHER_NAME"] = Convert.ToString(_dtReader["FATHER_NAME"]);
                _row["AMOUNT_PAYBLE"] = Convert.ToString(_dtReader["AMOUNT_PAYBLE"]);
                _dtblMappedStudentdetails.Rows.Add(_row);
            } _dtReader.Close(); _dtReader.Dispose();
        }
        gvMappedStudents.DataSource = _dtblMappedStudentdetails; gvMappedStudents.DataBind();
    }
    protected void cbHeader_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbHeader = (CheckBox)sender; bool checkState = cbHeader.Checked;
        foreach (GridViewRow _row in gvMappedStudents.Rows)
        {
            ((CheckBox)_row.FindControl("cbRow")).Checked = checkState;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string SQL_Update_CollectComponentMaster = "", SQL_StudentComponentMapping = "";
        SQL_Update_CollectComponentMaster = "update collect_component_master set AMOUNT_PAYBLE=?,MAPPED_UPDATE_DATE=now(),MAPPED_UPDATE_TIME=now(),UPDATE_BY=? where STUDENT_ID=? and COMPONENT_ID=? and MAPPED_DATE>=?";
        SQL_StudentComponentMapping = "insert into student_component_mapping(STUDENT_ID,COMPONENT_DETAIL_ID,SCHOOL_SESSION_ID,APPLICABLE_DATE) values ";
        int Counter = 0;
        _Command.CommandText = SQL_Update_CollectComponentMaster;
        foreach (GridViewRow _row in gvMappedStudents.Rows)
        {
            HiddenField StudentID = ((HiddenField)_row.FindControl("hfStudentID"));
            if (((CheckBox)_row.FindControl("cbRow")).Checked)
            {
                _Command.Parameters.AddWithValue("AMOUNT_PAYBLE", Convert.ToString(ddlSelectAmount.SelectedItem));
                _Command.Parameters.AddWithValue("UPDATE_BY", Convert.ToString(Session["_User"]));
                _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(StudentID.Value));
                _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(ddlSelectComponent.SelectedValue));
                _Command.Parameters.AddWithValue("MAPPED_DATE", Convert.ToDateTime(ddlApplicableDate.SelectedValue).ToString("yyyy-MM-dd"));

                _Command.CommandText = SQL_Update_CollectComponentMaster; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();

                int StartDateIndex = ddlApplicableDate.SelectedIndex;
                while (StartDateIndex < ddlApplicableDate.Items.Count)
                {
                    SQL_StudentComponentMapping += "('" + Convert.ToString(StudentID.Value) + "','" + Convert.ToString(ddlSelectAmount.SelectedValue) + "','" + Convert.ToString(Session["_SessionID"]) + "','" + Convert.ToDateTime(ddlApplicableDate.Items[StartDateIndex].Value).ToString("yyyy-MM-dd") + "'),";
                    StartDateIndex++;
                }
                Counter += 1;
            }
        }

        if (Counter > 0)
        {
            SQL_StudentComponentMapping = SQL_StudentComponentMapping.Substring(0, SQL_StudentComponentMapping.Length - 1); SQL_StudentComponentMapping += ";";
            _Command.CommandText = SQL_StudentComponentMapping; _Command.ExecuteNonQuery();
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Mapping Updated !!!'); window.location.href='classWiseComponentMapping.aspx';", true);
        }
    }
}