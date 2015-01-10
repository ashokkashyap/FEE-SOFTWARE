using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_classWiseComponentMapping : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
    OdbcDataReader _objDataReader; DateTime varSessionStartDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
          
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                var SQL = "call spComponentMaster()";
                var _dtblComponents = new DataTable(); 
                var _dtAdapter = new OdbcDataAdapter();
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblComponents);
                ViewState["_dtblComponents"] = _dtblComponents;
                ddlSelectComponent.DataSource = _dtblComponents; ddlSelectComponent.DataTextField = "COMPONENT_NAME"; ddlSelectComponent.DataValueField = "COMPONENT_ID"; ddlSelectComponent.DataBind(); ddlSelectComponent.Items.Insert(0, new ListItem("Select Component", ""));

                //ddlApplicableDate.Items.Insert(0, new ListItem("select", "select"));
                //SQL = "select distinct MAPPED_DATE from collect_component_master where MAPPED_DATE is not null and SCHOOL_SESSION_ID='" + Convert.ToString(Session["_SessionID"]) + "' order by MAPPED_DATE";
                //_Command.CommandText = SQL;
                //using (var _dtReader = _Command.ExecuteReader())
                //{
                //    while (_dtReader.Read())
                //    {
                //        ddlApplicableDate.Items.Add(new ListItem(Convert.ToDateTime(_dtReader["MAPPED_DATE"]).ToString("dd-MMM-yyyy"), Convert.ToDateTime(_dtReader["MAPPED_DATE"]).ToString("dd-MMM-yyyy")));
                //    }
                //};
            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }
    protected void ddlSelectComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSelectAmount.Items.Clear(); ddlSelectClass.Items.Clear(); ddlApplicableDate.Items.Clear(); gvUnMappedStudentList.DataSource = null; gvUnMappedStudentList.DataBind(); gvMappedStudents.DataSource = null; gvMappedStudents.DataBind();
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
        ddlSelectClass.Items.Clear(); gvMappedStudents.DataSource = null; gvMappedStudents.DataBind(); gvUnMappedStudentList.DataSource = null; gvUnMappedStudentList.DataBind();
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
            
            foreach (var _ComponentFrequency in ComponentFrequency) 
            {
                Frequency = Convert.ToInt32(_ComponentFrequency);
            }
            if (Frequency > 0)
            {
                if (Convert.ToString(Session["_SessionStartDate"]) != "" && Session["_SessionStartDate"] != null)
                {
                    _Command.CommandText = "SELECT CONCAT(1,'-',a.START_MONTH,'-',a.START_YEAR) as d1 FROM component_master a WHERE a.COMPONENT_ID='" + ddlSelectComponent.SelectedValue + "'";
                    _objDataReader = _Command.ExecuteReader();
                    while (_objDataReader.Read())
                    {
                         varSessionStartDate = Convert.ToDateTime(_objDataReader["d1"]);

                    } _objDataReader.Close();

                  
                   // DateTime varSessionStartDate = Convert.ToDateTime(Session["_SessionStartDate"]);
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
            gvUnMappedStudentList.DataSource = null; gvUnMappedStudentList.DataBind();
            gvMappedStudents.DataSource = null; gvMappedStudents.DataBind();
        }
    }
    protected void cbHeader_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cbHeader = (CheckBox)sender;
        bool Status = cbHeader.Checked;
        foreach (GridViewRow _row in gvUnMappedStudentList.Rows)
        {
            CheckBox cbRow = (CheckBox)_row.FindControl("cbRow"); cbRow.Checked = Status;
        }
    }
    private void getStudents(int ClassCode)
    {
        gvUnMappedStudentList.DataSource = null; gvUnMappedStudentList.DataBind();
        gvMappedStudents.DataSource = null; gvMappedStudents.DataBind();
        string SQL = ""; Dictionary<int, DateTime> dicMappingDetails = new Dictionary<int, DateTime>(); string StudentIDs = "";
        DataTable _dtblMappedStudentdetails = new DataTable(); DataTable _dtblUnMappedStudentdetails = new DataTable();

        _dtblMappedStudentdetails.Columns.Add("STUDENT_ID"); _dtblMappedStudentdetails.Columns.Add("SNAME"); _dtblMappedStudentdetails.Columns.Add("STUDENT_REGISTRATION_NBR"); _dtblMappedStudentdetails.Columns.Add("FATHER_NAME"); _dtblMappedStudentdetails.Columns.Add("AMOUNT_PAYBLE");

        //SQL = "CALL `spMappedStudentDetailsFromComponentIDAndClassCode`('" + Convert.ToString(ddlSelectComponent.SelectedValue) + "','" + ClassCode + "')";
        SQL = "CALL `spGetMaxMappedDateFromCollectionMasterClassCodeComponentIDSsID`('" + ClassCode + "', '" + Convert.ToString(ddlSelectComponent.SelectedValue) + "','" + Convert.ToString(Session["_SessionID"]) + "')";
        _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure;
        OdbcDataReader _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            dicMappingDetails.Add(Convert.ToInt32(_dtReader["STUDENT_ID"]), Convert.ToDateTime(_dtReader["MD"]));
            StudentIDs += Convert.ToString(_dtReader["STUDENT_ID"]) + ",";
        } _dtReader.Close(); _dtReader.Dispose();

        if (StudentIDs.Length > 0)
        {
            StudentIDs = StudentIDs.Substring(0, StudentIDs.Length - 1);
        }

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
        if (StudentIDs.Length > 0)
        {
            SQL = "CALL `spStudentDetailsFromClassCodeAndSkippingStudentIDs`('" + ClassCode + "', '" + StudentIDs + "')";
        }
        else { SQL = "CALL `spStudentDetailsFromClassCodeAndSkippingStudentIDs`('" + ClassCode + "', '0000000000000000')"; }
        //Response.Write(SQL); Response.End();
        _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure; _dtReader = _Command.ExecuteReader();
        _dtblUnMappedStudentdetails.Load(_dtReader); _dtReader.Close(); _dtReader.Dispose();

        _dtblUnMappedStudentdetails.Columns.Add("SNAME");
        foreach (DataRow _row in _dtblUnMappedStudentdetails.Rows)
        {
            _row["SNAME"] = Convert.ToString(_row["FIRST_NAME"]) + " " + Convert.ToString(_row["MIDDLE_NAME"]) + " " + Convert.ToString(_row["LAST_NAME"]);
        }

        //lblInfo.Text = SQL;
        // 
        //OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();

        //_dtAdapter.SelectCommand = _Command;
        //_dtAdapter.Fill(_dtblMappedStudentdetails);
        ////_dtblMappedStudentdetails.Load(_dtReader);
        ////_dtReader.Close(); _dtReader.Dispose();

        //SQL = "CALL `spUnMappedStudentDetailsFromComponentIDAndClassCode`('" + Convert.ToString(ddlSelectComponent.SelectedValue) + "','" + ClassCode + "')";
        //_Command.CommandText = SQL;
        //_dtAdapter.SelectCommand = _Command;
        //_dtAdapter.Fill(_dtblUnMappedStudentdetails);

        gvUnMappedStudentList.DataSource = _dtblUnMappedStudentdetails; gvUnMappedStudentList.DataBind();
        gvMappedStudents.DataSource = _dtblMappedStudentdetails; gvMappedStudents.DataBind();
        //_dtAdapter.Dispose();
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string SQL_Insert_CollectComponentMaster = "", SQL_StudentComponentMapping = "";
        SQL_Insert_CollectComponentMaster = "insert into collect_component_master(STUDENT_ID,COMPONENT_ID,AMOUNT_PAYBLE,AMOUNT_PAID,DISCOUNT,MAPPED_DATE,MAPPED_CREATE_DATE,MAPPED_CREATE_TIME,CREATE_BY,SCHOOL_SESSION_ID) values ";
        SQL_StudentComponentMapping = "insert into student_component_mapping(STUDENT_ID,COMPONENT_DETAIL_ID,SCHOOL_SESSION_ID,APPLICABLE_DATE) values ";
        int Counter = 0;
        foreach (GridViewRow _row in gvUnMappedStudentList.Rows)
        {
            HiddenField StudentID = ((HiddenField)_row.FindControl("hfStudentID"));
            if (((CheckBox)_row.FindControl("cbRow")).Checked)
            {
                int StartDateIndex = ddlApplicableDate.SelectedIndex;
                while (StartDateIndex < ddlApplicableDate.Items.Count)
                {
                    SQL_Insert_CollectComponentMaster += "('" + Convert.ToString(StudentID.Value) + "','" + Convert.ToString(ddlSelectComponent.SelectedValue) + "','" + Convert.ToString(ddlSelectAmount.SelectedItem) + "','0','0','" + Convert.ToDateTime(ddlApplicableDate.Items[StartDateIndex].Value).ToString("yyyy-MM-dd") + "',now(),now(),'" + Convert.ToString(Session["_User"]) + "','" + Convert.ToString(Session["_SessionID"]) + "'),";
                    SQL_StudentComponentMapping += "('" + Convert.ToString(StudentID.Value) + "','" + Convert.ToString(ddlSelectAmount.SelectedValue) + "','" + Convert.ToString(Session["_SessionID"]) + "','" + Convert.ToDateTime(ddlApplicableDate.Items[StartDateIndex].Value).ToString("yyyy-MM-dd") + "'),";
                    StartDateIndex++;
                }
                Counter += 1;
            }
        }

        if (Counter > 0)
        {
            SQL_Insert_CollectComponentMaster = SQL_Insert_CollectComponentMaster.Substring(0, SQL_Insert_CollectComponentMaster.Length - 1); SQL_Insert_CollectComponentMaster += ";";
            SQL_StudentComponentMapping = SQL_StudentComponentMapping.Substring(0, SQL_StudentComponentMapping.Length - 1); SQL_StudentComponentMapping += ";";
            _Command.CommandText = SQL_StudentComponentMapping; _Command.ExecuteNonQuery();
            _Command.CommandText = SQL_Insert_CollectComponentMaster; _Command.ExecuteNonQuery();
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Mapping Saved.'); window.location.href='classWiseComponentMapping.aspx';", true);
        }
        //Response.Write(SQL_Insert_CollectComponentMaster + "<br/><br/><br/>");
        //Response.Write(SQL_StudentComponentMapping); Response.End();



        //foreach (GridViewRow _row in gvStudentList.Rows)
        //{
        //    if (((CheckBox)_row.FindControl("chkThis")).Checked)
        //    {
        //        HiddenField StudentID = ((HiddenField)_row.FindControl("hfStudentID"));
        //         SQL = "select count(*) from student_component_mapping where STUDENT_ID = '" + StudentID.Value + "' and  school_session_id = " + Convert.ToString(Session["_SessionID"]) + " and COMPONENT_DETAIL_ID in (select COMPONENT_DETAIL_ID from component_detail where COMPONENT_ID = '" + ddlSelectComponent.SelectedValue + "')";
        //        _Command.CommandText = SQL;
        //        if (Convert.ToInt32(_Command.ExecuteScalar()) == 0)
        //        {
        //            SQL = "insert into student_component_mapping(STUDENT_ID,COMPONENT_DETAIL_ID,SCHOOL_SESSION_ID) values('" + StudentID.Value + "','" + ddlSelectAmount.SelectedValue + "'," + Convert.ToString(Session["_SessionID"]) + ")";
        //            _Command.CommandText = SQL; _Command.ExecuteNonQuery();
        //        }
        //        else
        //        {
        //            SQL = "update student_component_mapping set COMPONENT_DETAIL_ID = '" + ddlSelectAmount.SelectedValue + "' where STUDENT_ID = '" + StudentID.Value + "' and  school_session_id = " + Convert.ToString(Session["_SessionID"]) + " and COMPONENT_DETAIL_ID in (select COMPONENT_DETAIL_ID from component_detail where COMPONENT_ID = '" + ddlSelectComponent.SelectedValue + "')";
        //            _Command.CommandText = SQL; _Command.ExecuteNonQuery();
        //        }
        //
                #region Insert/Update Mapping
        //        List<MappedComponentDetails> ls_objMappedComponentsDetails = new List<MappedComponentDetails>(); MappedComponentDetails objMappedComponentsDetails = null;
        //        SQL = "select cm.COMPONENT_ID,cm.COMPONENT_NAME,cm.COMPONENT_FREQUENCY,cm.START_MONTH,cm.START_YEAR,cd.COMPONENT_DETAIL_ID,cd.COMPONENT_AMOUNT from component_master cm, component_detail cd where cm.COMPONENT_ID=cd.COMPONENT_ID and cd.COMPONENT_DETAIL_ID='" + ddlSelectAmount.SelectedValue + "' and cd.SCHOOL_SESSION_ID=" + Convert.ToString(Session["_SessionID"]) + ";";
        //        _Command.CommandText = SQL;
        //        using (var _dtReader = _Command.ExecuteReader())
        //        {
        //            while (_dtReader.Read())
        //            {
        //                objMappedComponentsDetails = new MappedComponentDetails();
        //                objMappedComponentsDetails.varComponentID = Convert.ToString(_dtReader["COMPONENT_ID"]);
        //                objMappedComponentsDetails.varComponentName = Convert.ToString(_dtReader["COMPONENT_NAME"]);
        //                objMappedComponentsDetails.varComponentFrequency = Convert.ToString(_dtReader["COMPONENT_FREQUENCY"]);
        //                objMappedComponentsDetails.varStartMonth = Convert.ToString(_dtReader["START_MONTH"]);
        //                objMappedComponentsDetails.varStartYear = Convert.ToString(_dtReader["START_YEAR"]);
        //                objMappedComponentsDetails.varComponentDetailID = Convert.ToString(_dtReader["COMPONENT_DETAIL_ID"]);
        //                objMappedComponentsDetails.varComponentAmount = Convert.ToString(_dtReader["COMPONENT_AMOUNT"]);
        //                ls_objMappedComponentsDetails.Add(objMappedComponentsDetails);
        //            }
        //        }

        //        if (ddlApplicableDate.SelectedIndex > 0)
        //        {
        //            foreach (MappedComponentDetails _MappedComponentsDetails in ls_objMappedComponentsDetails)
        //            {
        //                DateTime varComponentStartDate = Convert.ToDateTime(_MappedComponentsDetails.varStartMonth + "/1/" + _MappedComponentsDetails.varStartYear);
        //                DateTime varComponentEndDate = varComponentStartDate.AddMonths(Convert.ToInt32(_MappedComponentsDetails.varComponentFrequency)).AddDays(-1);
        //                DateTime LoopvarComponentStartDate = varComponentStartDate;
        //                DateTime LoopvarComponentEndDate = varComponentEndDate;
        //                OdbcDataReader _dtReader = null;
        //                //int x = 1;
        //                //while (x < 20)
        //                //{
        //                    if (LoopvarComponentStartDate >= Convert.ToDateTime(ddlApplicableDate.SelectedValue))
        //                    {
        //                        SQL = "select * from collect_component_master where STUDENT_ID='" + StudentID.Value + "' and COMPONENT_ID='" + _MappedComponentsDetails.varComponentID + "' and MAPPED_DATE='" + LoopvarComponentStartDate.ToString("yyyy-MM-dd") + "'";
        //                        _Command.CommandText = SQL; _dtReader = _Command.ExecuteReader();
        //                        if (_dtReader.HasRows)
        //                        {
        //                            _dtReader.Close();
        //                            SQL = "update collect_component_master set AMOUNT_PAYBLE='" + _MappedComponentsDetails.varComponentAmount + "' where STUDENT_ID='" + StudentID + "' and COMPONENT_ID='" + ddlSelectComponent.SelectedValue + "' and MAPPED_DATE='" + LoopvarComponentStartDate.ToString("yyyy-MM-dd") + "' and AMOUNT_PAYBLE != '0'";
        //                            _Command.CommandText = SQL; _Command.ExecuteNonQuery();
        //                        }
        //                        else
        //                        {
        //                            _dtReader.Close();
        //                            SQL = "select * from collect_component_master where STUDENT_ID='" + StudentID.Value + "' and MAPPED_DATE='" + LoopvarComponentStartDate.ToString("yyyy-MM-dd") + "'";
        //                            _Command.CommandText = SQL; _dtReader = _Command.ExecuteReader();
        //                            if (_dtReader.HasRows)
        //                            {
        //                                _Command.Parameters.AddWithValue("STUDENT_ID", StudentID.Value);
        //                                _Command.Parameters.AddWithValue("COMPONENT_ID", _MappedComponentsDetails.varComponentID);
        //                                _Command.Parameters.AddWithValue("AMOUNT_PAYBLE", _MappedComponentsDetails.varComponentAmount);
        //                                _Command.Parameters.AddWithValue("AMOUNT_PAID", "0");
        //                                _Command.Parameters.AddWithValue("DISCOUNT", "0");
        //                                _Command.Parameters.AddWithValue("MAPPED_DATE", LoopvarComponentStartDate.ToString("yyyy-MM-dd"));
        //                                _Command.Parameters.AddWithValue("SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
        //                                SQL = "insert into collect_component_master(STUDENT_ID,COMPONENT_ID,AMOUNT_PAYBLE,AMOUNT_PAID,DISCOUNT,MAPPED_DATE,SCHOOL_SESSION_ID,MAPPED_CREATE_DATE,MAPPED_CREATE_TIME) values(?,?,?,?,?,?,?,now(),now())";
        //                                _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
        //                            } _dtReader.Close();
        //                        }
        //                    }
        //                    LoopvarComponentStartDate = LoopvarComponentStartDate.AddMonths(Convert.ToInt32(_MappedComponentsDetails.varComponentFrequency));
        //                    LoopvarComponentEndDate = LoopvarComponentStartDate.AddMonths(Convert.ToInt32(_MappedComponentsDetails.varComponentFrequency)).AddDays(-1);
        //                //    x++;
        //                //}
        //            }
        //        }
                #endregion
        //    }
        //}
    }
}    