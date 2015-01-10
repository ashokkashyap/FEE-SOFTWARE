using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_StudentWise_componentMapping_single : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; DataTable _dtblRecords;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                bndtptgrid();
                if (Convert.ToString(Session["admno"]) == "")
                {
                    
                }
                else
                {
                    txtAdmissionNo.Text = Convert.ToString(Session["admno"]);
                    btnGetDetails_Click(btnGetDetails, null);
                }
            
            }
        }
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["_SessionStartDate"]) != "" && Session["_SessionStartDate"] != null)
        {
            string SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";
            _Command.CommandText = SQL;
            OdbcDataReader _dtReader = _Command.ExecuteReader();
            while (_dtReader.Read())
            {
                lblStudentID.Text = Convert.ToString(_dtReader["STUDENT_ID"]);
                lblName.Text = Convert.ToString(_dtReader["NAME"]);
                lblClass.Text = Convert.ToString(_dtReader["CLASS"]);
                lblFatherName.Text = Convert.ToString(_dtReader["FATHER_NAME"]);
                lblMotherName.Text = Convert.ToString(_dtReader["MOTHER_NAME"]);
                lblAdmissionNo.Text = Convert.ToString(_dtReader["STUDENT_REGISTRATION_NBR"]);
                lblAddress.Text = Convert.ToString(_dtReader["ADDRESS_LINE1"]);
            } _dtReader.Close(); _dtReader.Dispose();


            _dtblRecords = new DataTable();
            Dictionary<int, double> dicComponentIDAndMaxAmount = new Dictionary<int, double>();

            SQL = "CALL `spComponentMaster`";
            _Command.CommandText = SQL;
            _dtReader = _Command.ExecuteReader();
            _dtblRecords.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();
            OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
            gvComponents.DataSource = _dtblRecords; gvComponents.DataBind();

            SQL = "CALL `spGetMaxAmountPayableAndComponentIDFromStudentIDAndSessionID`('" + lblStudentID.Text.Trim() + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
            _Command.CommandText = SQL;
            _dtReader = _Command.ExecuteReader();
            while (_dtReader.Read())
            {
                dicComponentIDAndMaxAmount.Add(Convert.ToInt32(_dtReader["COMPONENT_ID"]), Convert.ToDouble(_dtReader["AMOUNT_PAYBLE"]));
            } _dtReader.Close(); _dtReader.Dispose();


            foreach (GridViewRow _row in gvComponents.Rows)
            {
                DateTime varSessionStartDate = Convert.ToDateTime(Session["_SessionStartDate"]);
                DateTime varSessionEndDate = Convert.ToDateTime(Session["_SessionEndDate"]);

                HiddenField hfComponentID = (HiddenField)_row.FindControl("hfCOMPONENT_ID");
                HiddenField hfID = (HiddenField)_row.FindControl("hfID");
                DropDownList ddlStatus = (DropDownList)_row.FindControl("ddlStatus");
                DropDownList ddlApplicableDate = (DropDownList)_row.FindControl("ddlApplicableDate");

                ddlStatus.Items.Add(new ListItem("N/A", "-1"));
                SQL = "CALL `spComponentDetailMaster`('" + hfComponentID.Value + "')";
                _Command.CommandText = SQL;
                _dtReader = _Command.ExecuteReader();
                while (_dtReader.Read())
                {
                    ddlStatus.Items.Add(new ListItem(Convert.ToString(_dtReader["COMPONENT_AMOUNT"]), Convert.ToString(_dtReader["COMPONENT_DETAIL_ID"])));
                } _dtReader.Close(); _dtReader.Dispose();

                var ComponentFrequency = from ComponentDetails in _dtblRecords.AsEnumerable() where Convert.ToInt32(ComponentDetails["COMPONENT_ID"]).Equals(Convert.ToInt32(hfComponentID.Value)) select ComponentDetails["COMPONENT_FREQUENCY"];
                int Frequency = 0;
                foreach (var _ComponentFrequency in ComponentFrequency) { Frequency = Convert.ToInt32(_ComponentFrequency); }
                if (Frequency > 0)
                {
                    while (varSessionStartDate <= varSessionEndDate)
                    {
                        ddlApplicableDate.Items.Add(new ListItem(varSessionStartDate.ToString("dd-MMM-yyyy"), varSessionStartDate.ToString("dd-MMM-yyyy")));
                        varSessionStartDate = varSessionStartDate.AddMonths(Frequency);
                    }
                }

                var MaxAmount = from x in dicComponentIDAndMaxAmount.AsEnumerable() where x.Key.Equals(Convert.ToInt32(hfComponentID.Value)) select x.Value;
                if (MaxAmount.Any())
                {
                    foreach (var _MaxAmount in MaxAmount)
                    {
                        SQL = "CALL `spGetMinMapDateFromStudentIDPaybleAmtCompIDAndSessionID`('" + lblStudentID.Text.Trim() + "', '" + _MaxAmount.ToString() + "', '" + hfComponentID.Value + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
                        _Command.CommandText = SQL;
                        string MapDate = Convert.ToDateTime(_Command.ExecuteScalar()).ToString("dd-MMM-yyyy");
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(_MaxAmount.ToString()));
                        ddlApplicableDate.SelectedIndex = ddlApplicableDate.Items.IndexOf(ddlApplicableDate.Items.FindByValue(MapDate));
                    }
                }
                else { ddlStatus.SelectedIndex = 0; ddlApplicableDate.SelectedIndex = 0; }
            }
 
        }
    }

    public void bndtptgrid()
    {
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select * from rute_master", _Connection));
        DataTable dt = new DataTable();
        odbc.Fill(dt);
        gvtransport.DataSource = dt;
        gvtransport.DataBind();

        
    }
 
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow _row in gvComponents.Rows)
        {
            var COMPONENT_ID = ((HiddenField)_row.FindControl("hfCOMPONENT_ID")).Value;
            DropDownList ddlStatus = (DropDownList)_row.FindControl("ddlStatus");
            DropDownList ddlApplicableDate = (DropDownList)_row.FindControl("ddlApplicableDate");
            string SQL = "";
            if (ddlStatus.SelectedIndex == 0)
            {
                SQL = "select count(*) from collect_component_master where STUDENT_ID=? and COMPONENT_ID=? and SCHOOL_SESSION_ID=?";
                _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(COMPONENT_ID));
                _Command.Parameters.AddWithValue("SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
                _Command.CommandText = SQL;
                if (Convert.ToInt32(_Command.ExecuteScalar()) > 0)
                {
                    _Command.Parameters.Clear();
                    SQL = "update collect_component_master set AMOUNT_PAYBLE=?, MAPPED_UPDATE_DATE=now(), MAPPED_UPDATE_TIME=now(), UPDATE_BY=? where STUDENT_ID=? and COMPONENT_ID=? and MAPPED_DATE>=?";
                    _Command.CommandText = SQL;
                    int startIndex = ddlApplicableDate.SelectedIndex;
                    while (startIndex < ddlApplicableDate.Items.Count)
                    {
                        _Command.Parameters.AddWithValue("AMOUNT_PAYBLE", Convert.ToString(0));
                        _Command.Parameters.AddWithValue("UPDATE_BY", Convert.ToString(Session["_User"]));
                        _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                        _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(COMPONENT_ID));
                        _Command.Parameters.AddWithValue("MAPPED_DATE", Convert.ToDateTime(ddlApplicableDate.Items[startIndex].Value).ToString("yyyy-MM-dd"));
                        _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
                        startIndex++;
                    }
                }
                else
                {
                    _Command.Parameters.Clear();
                }

            }
            else
            {

                SQL = "select count(*) from collect_component_master where STUDENT_ID=? and COMPONENT_ID=? and SCHOOL_SESSION_ID=?";
                _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(COMPONENT_ID));
                _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(Session["_SessionID"]));
                _Command.CommandText = SQL;
                if (Convert.ToInt32(_Command.ExecuteScalar()) > 0)
                {
                    _Command.Parameters.Clear();
                    SQL = "update collect_component_master set AMOUNT_PAYBLE=?, MAPPED_UPDATE_DATE=now(), MAPPED_UPDATE_TIME=now(), UPDATE_BY=? where STUDENT_ID=? and COMPONENT_ID=? and MAPPED_DATE>=?";
                    _Command.CommandText = SQL;
                    int startIndex = ddlApplicableDate.SelectedIndex;
                    while (startIndex < ddlApplicableDate.Items.Count)
                    {
                        _Command.Parameters.AddWithValue("AMOUNT_PAYBLE", Convert.ToString(ddlStatus.SelectedItem));
                        _Command.Parameters.AddWithValue("UPDATE_BY", Convert.ToString(Session["_User"]));
                        _Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                        _Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(COMPONENT_ID));
                        _Command.Parameters.AddWithValue("MAPPED_DATE", Convert.ToDateTime(ddlApplicableDate.Items[startIndex].Value).ToString("yyyy-MM-dd"));
                        _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
                        startIndex++;
                    }
                }
                else
                {
                    _Command.Parameters.Clear();

                    string SQL_Insert_CollectComponentMaster = "", SQL_StudentComponentMapping = "";
                    SQL_Insert_CollectComponentMaster = "insert into collect_component_master(STUDENT_ID,COMPONENT_ID,AMOUNT_PAYBLE,AMOUNT_PAID,DISCOUNT,MAPPED_DATE,MAPPED_CREATE_DATE,MAPPED_CREATE_TIME,CREATE_BY,SCHOOL_SESSION_ID) values ";
                    SQL_StudentComponentMapping = "insert into student_component_mapping(STUDENT_ID,COMPONENT_DETAIL_ID,SCHOOL_SESSION_ID,APPLICABLE_DATE) values ";
                    int StartDateIndex = ddlApplicableDate.SelectedIndex;
                    int Counter = 0;
                    while (StartDateIndex < ddlApplicableDate.Items.Count)
                    {
                        SQL_Insert_CollectComponentMaster += "('" + Convert.ToString(lblStudentID.Text) + "','" + Convert.ToString(COMPONENT_ID) + "','" + Convert.ToString(ddlStatus.SelectedItem) + "','0','0','" + Convert.ToDateTime(ddlApplicableDate.Items[StartDateIndex].Value).ToString("yyyy-MM-dd") + "',now(),now(),'" + Convert.ToString(Session["_User"]) + "','" + Convert.ToString(Session["_SessionID"]) + "'),";
                        SQL_StudentComponentMapping += "('" + Convert.ToString(lblStudentID.Text) + "','" + Convert.ToString(ddlStatus.SelectedValue) + "','" + Convert.ToString(Session["_SessionID"]) + "','" + Convert.ToDateTime(ddlApplicableDate.Items[StartDateIndex].Value).ToString("yyyy-MM-dd") + "'),";
                        StartDateIndex++; Counter += 1;
                    }
                    if (Counter > 0)
                    {
                        SQL_Insert_CollectComponentMaster = SQL_Insert_CollectComponentMaster.Substring(0, SQL_Insert_CollectComponentMaster.Length - 1); SQL_Insert_CollectComponentMaster += ";";
                        SQL_StudentComponentMapping = SQL_StudentComponentMapping.Substring(0, SQL_StudentComponentMapping.Length - 1); SQL_StudentComponentMapping += ";";
                        _Command.CommandText = SQL_StudentComponentMapping; _Command.ExecuteNonQuery();
                        _Command.CommandText = SQL_Insert_CollectComponentMaster; _Command.ExecuteNonQuery();
                    }
                }
            }
            
        }

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Mapping updated !!!.'); window.location.href='StudentWise_componentMapping_single.aspx';", true);
    }

        protected void  Button1_Click(object sender, EventArgs e)
        {
           mpe.Show();
 
        }
}