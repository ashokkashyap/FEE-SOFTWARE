using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_multipleClassComponentMapping : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader _dtReader = null;
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
                _Command.CommandText = SQL;
                DataTable _dtblComponents = new DataTable();
                _dtReader = _Command.ExecuteReader();
                _dtblComponents.Load(_dtReader);
                _dtReader.Close(); _dtReader.Dispose();
                ViewState["_dtblComponents"] = _dtblComponents;
                ddlSelectComponent.DataSource = _dtblComponents; ddlSelectComponent.DataTextField = "COMPONENT_NAME"; ddlSelectComponent.DataValueField = "COMPONENT_ID"; ddlSelectComponent.DataBind(); ddlSelectComponent.Items.Insert(0, new ListItem("Select Component", ""));
            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }
    protected void ddlSelectComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSelectAmount.Items.Clear(); ddlApplicableDate.Items.Clear(); lvClassList.DataSource = null; lvClassList.DataBind();
        if (ddlSelectComponent.SelectedIndex > 0)
        {
            string SQL = "call spComponentDetailMaster('" + ddlSelectComponent.SelectedValue + "')";
            _Command.CommandText = SQL;
            DataTable _dtblComponentdetails = new DataTable();
            _dtReader = _Command.ExecuteReader();
            _dtblComponentdetails.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();
            ViewState["_dtblComponentdetails"] = _dtblComponentdetails;
            ddlSelectAmount.DataSource = _dtblComponentdetails; ddlSelectAmount.DataTextField = "COMPONENT_AMOUNT"; ddlSelectAmount.DataValueField = "COMPONENT_DETAIL_ID"; ddlSelectAmount.DataBind(); ddlSelectAmount.Items.Insert(0, new ListItem("Select", ""));
            _dtblComponentdetails.Dispose();

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
            } _dtblComponents.Dispose();

            SQL = "CALL `spGetClassDetailsNotInCollectionMasterFromComponentIDAndSessID`('" + ddlSelectComponent.SelectedValue + "', '" + Convert.ToString(Session["_SessionID"]) + "')";
            _Command.CommandText = SQL;
            DataTable _dtblClasses = new DataTable();
            _dtReader = _Command.ExecuteReader();
            _dtblClasses.Load(_dtReader);
            _dtReader.Close(); _dtReader.Dispose();
            ViewState["_dtblClasses"] = _dtblClasses;
            lvClassList.DataSource = _dtblClasses; lvClassList.DataBind();
        }
    }
    protected void ddlSelectAmount_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
    {
        List<int> lsStudentIds = new List<int>();
        string CustomFields = "", SQl = "", SQL_Insert_CollectComponentMaster = "", SQL_StudentComponentMapping = "";
        SQL_Insert_CollectComponentMaster = "insert into collect_component_master(STUDENT_ID,COMPONENT_ID,AMOUNT_PAYBLE,AMOUNT_PAID,DISCOUNT,MAPPED_DATE,MAPPED_CREATE_DATE,MAPPED_CREATE_TIME,CREATE_BY,SCHOOL_SESSION_ID) values ";
        SQL_StudentComponentMapping = "insert into student_component_mapping(STUDENT_ID,COMPONENT_DETAIL_ID,SCHOOL_SESSION_ID,APPLICABLE_DATE) values ";
        int Counter = 0;
        foreach (ListViewItem _item in lvClassList.Items)
        {
            CheckBox cbField = (CheckBox)_item.FindControl("cbField");
            HiddenField hfClassCode = (HiddenField)_item.FindControl("hfClassCode");
            if (cbField.Checked)
            {
                CustomFields += Convert.ToString(hfClassCode.Value) + ",";
            }
        }
        if (CustomFields.Length > 0)
        {
            CustomFields = CustomFields.Substring(0, CustomFields.Length - 1);

            //Response.Write(CustomFields); Response.End();
            SQl = "CALL `spGetStudentIdsFromClassCodes`('" + CustomFields + "')";
            _Command.CommandText = SQl;
            _dtReader = _Command.ExecuteReader();
            while (_dtReader.Read())
            {
                lsStudentIds.Add(Convert.ToInt32(_dtReader[0]));
            } _dtReader.Close(); _dtReader.Dispose();
            foreach (int item in lsStudentIds)
            {
                int StartDateIndex = ddlApplicableDate.SelectedIndex;
                while (StartDateIndex < ddlApplicableDate.Items.Count)
                {
                    SQL_Insert_CollectComponentMaster += "('" + Convert.ToString(item) + "','" + Convert.ToString(ddlSelectComponent.SelectedValue) + "','" + Convert.ToString(ddlSelectAmount.SelectedItem) + "','0','0','" + Convert.ToDateTime(ddlApplicableDate.Items[StartDateIndex].Value).ToString("yyyy-MM-dd") + "',now(),now(),'" + Convert.ToString(Session["_User"]) + "','" + Convert.ToString(Session["_SessionID"]) + "'),";
                    SQL_StudentComponentMapping += "('" + Convert.ToString(item) + "','" + Convert.ToString(ddlSelectAmount.SelectedValue) + "','" + Convert.ToString(Session["_SessionID"]) + "','" + Convert.ToDateTime(ddlApplicableDate.Items[StartDateIndex].Value).ToString("yyyy-MM-dd") + "'),";
                    StartDateIndex++;
                }
                Counter += 1;
            }
            if (Counter > 0)
            {
                SQL_Insert_CollectComponentMaster = SQL_Insert_CollectComponentMaster.Substring(0, SQL_Insert_CollectComponentMaster.Length - 1); SQL_Insert_CollectComponentMaster += ";";
                SQL_StudentComponentMapping = SQL_StudentComponentMapping.Substring(0, SQL_StudentComponentMapping.Length - 1); SQL_StudentComponentMapping += ";";
                _Command.CommandText = SQL_StudentComponentMapping; _Command.ExecuteNonQuery();
                _Command.CommandText = SQL_Insert_CollectComponentMaster; _Command.ExecuteNonQuery();
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Mapping Saved.'); window.location.href='multipleClassComponentMapping.aspx';", true);
            }
        }
    }
}