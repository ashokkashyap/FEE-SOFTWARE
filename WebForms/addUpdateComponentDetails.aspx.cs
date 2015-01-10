using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_addUpdateComponentDetails : System.Web.UI.Page
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
                var FRange = Enumerable.Range(1, 12);
                var YearRange = Enumerable.Range(DateTime.Now.Year - 1, 3);
                ddlAFrequency.DataSource = FRange; ddlAFrequency.DataBind();
                ddlAStartYear.DataSource = YearRange; ddlAStartYear.DataBind();
                ddlUFrequency.DataSource = FRange; ddlUFrequency.DataBind();
                ddlUStartYear.DataSource = YearRange; ddlUStartYear.DataBind();
                var PresentYear = DateTime.Now.Year;
                ddlAStartYear.SelectedValue = Convert.ToString(PresentYear);
                ddlUStartYear.SelectedValue = Convert.ToString(PresentYear);

                var SQL = "call spComponentMaster";
                var _dtblComponents = new DataTable();
                var _dtAdapter = new OdbcDataAdapter(SQL, _Connection);
                _dtAdapter.Fill(_dtblComponents);
                ViewState["_dtblComponents"] = _dtblComponents;
                ddlSelectComponent.DataSource = _dtblComponents; ddlSelectComponent.DataTextField = "COMPONENT_NAME"; ddlSelectComponent.DataValueField = "COMPONENT_ID"; ddlSelectComponent.DataBind(); ddlSelectComponent.Items.Insert(0, new ListItem("Select Component", ""));
                for (int x = 1; x < 13; x++)
                {
                    var Month = Convert.ToString(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x));
                    ddlAStartMonth.Items.Insert(x - 1, new ListItem(Month, Month));
                    ddlUStartMonth.Items.Insert(x - 1, new ListItem(Month, Month));
                }
                ddlAStartMonth.SelectedValue = "April";
            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }
    protected void ddlSelectComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSelectComponent.SelectedIndex > 0)
        {
            var _dtblComponents = (DataTable)ViewState["_dtblComponents"];
            var Data=from x in _dtblComponents.AsEnumerable() where Convert.ToInt32(x["COMPONENT_ID"]).Equals(Convert.ToInt32(ddlSelectComponent.SelectedValue)) select x;
            foreach (var record in Data)
            {
                txtUComponentName.Text = Convert.ToString(record["COMPONENT_NAME"]);
                ddlUFrequency.SelectedValue = Convert.ToString(record["COMPONENT_FREQUENCY"]);
                ddlUStartMonth.SelectedValue = Convert.ToString(record["START_MONTH"]);
                ddlUStartYear.SelectedValue = Convert.ToString(record["START_YEAR"]);
            }
        }
        else
        {
            txtUComponentName.Text = ""; ddlUFrequency.SelectedIndex = 0; ddlUStartMonth.SelectedIndex = 0; 
            ddlUStartYear.SelectedIndex = 0;
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        var SQL = "select ifnull(max(PRIORITY),0) from component_master";
        _Command.CommandText = SQL;
        int varComponentPriority = Convert.ToInt32(_Command.ExecuteScalar()) + 1;
        SQL = "insert into component_master(COMPONENT_NAME,COMPONENT_FREQUENCY,START_MONTH,START_YEAR,PRIORITY,CREATE_DATE,CREATE_TIME,CREATE_BY,SCHOOL_SESSION_ID) values(?,?,?,?,?,now(),now(),?,?)";
        _Command.Parameters.AddWithValue("@COMPONENT_NAME", Convert.ToString(txtAComponentName.Text).Trim());
        _Command.Parameters.AddWithValue("@COMPONENT_FREQUENCY", Convert.ToString(ddlAFrequency.SelectedValue));
        _Command.Parameters.AddWithValue("@START_MONTH", Convert.ToString(ddlAStartMonth.SelectedValue));
        _Command.Parameters.AddWithValue("@START_YEAR", Convert.ToString(ddlAStartYear.SelectedValue));
        _Command.Parameters.AddWithValue("@PRIORITY", Convert.ToString(varComponentPriority));
        _Command.Parameters.AddWithValue("@CREATE_BY", Convert.ToString(Session["_User"]));
        _Command.Parameters.AddWithValue("@SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
        _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Component Saved.'); window.location.href='addUpdateComponentDetails.aspx';", true);
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        var SQL = "update component_master set COMPONENT_NAME=?,COMPONENT_FREQUENCY=?,START_MONTH=?,START_YEAR=?,UPDATE_DATE=now(),UPDATE_TIME=now(),UPDATE_BY=?,SCHOOL_SESSION_ID=? where COMPONENT_ID=?";
        _Command.Parameters.AddWithValue("@COMPONENT_NAME", Convert.ToString(txtUComponentName.Text).Trim());
        _Command.Parameters.AddWithValue("@COMPONENT_FREQUENCY", Convert.ToString(ddlUFrequency.SelectedValue));
        _Command.Parameters.AddWithValue("@START_MONTH", Convert.ToString(ddlUStartMonth.SelectedValue));
        _Command.Parameters.AddWithValue("@START_YEAR", Convert.ToString(ddlUStartYear.SelectedValue));
        _Command.Parameters.AddWithValue("@UPDATE_BY", Convert.ToString(Session["_User"]));
        _Command.Parameters.AddWithValue("@SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
        _Command.Parameters.AddWithValue("@COMPONENT_ID", Convert.ToString(ddlSelectComponent.SelectedValue));
        _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Component Updated.'); window.location.href='addUpdateComponentDetails.aspx';", true);
    }
}