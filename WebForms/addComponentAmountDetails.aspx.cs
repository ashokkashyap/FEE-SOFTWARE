using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_addComponentAmountDetails : System.Web.UI.Page
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
                var SQL = "call spComponentMaster";
                var _dtblComponents = new DataTable();
                var _dtAdapter = new OdbcDataAdapter(SQL, _Connection);
                _dtAdapter.Fill(_dtblComponents);
                ViewState["_dtblComponents"] = _dtblComponents;
                ddlSelectComponent.DataSource = _dtblComponents; ddlSelectComponent.DataTextField = "COMPONENT_NAME"; ddlSelectComponent.DataValueField = "COMPONENT_ID"; ddlSelectComponent.DataBind(); ddlSelectComponent.Items.Insert(0, new ListItem("Select Component", ""));
            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }
    protected void ddlSelectComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSelectComponent.SelectedIndex > 0)
        {
            getComponentAmounts(Convert.ToString(ddlSelectComponent.SelectedValue));

        }
        else { lvAmountDetails.DataSource = null; lvAmountDetails.DataBind(); }
    }

    private void getComponentAmounts(string ComponentID)
    {
        var SQL = "call spComponentDetailMaster('" + ComponentID + "')";
        var _dtblComponentdetails = new DataTable();
        var _dtAdapter = new OdbcDataAdapter(SQL, _Connection);
        _dtAdapter.Fill(_dtblComponentdetails);
        lvAmountDetails.DataSource = _dtblComponentdetails; lvAmountDetails.DataBind(); txtAmount.Text = "";
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string SQL = "insert into component_detail(COMPONENT_ID,COMPONENT_AMOUNT,SCHOOL_SESSION_ID) values(?,?,?)";
            _Command.Parameters.AddWithValue("@COMPONENT_ID", Convert.ToString(ddlSelectComponent.SelectedValue));
            _Command.Parameters.AddWithValue("@COMPONENT_AMOUNT", Convert.ToString(txtAmount.Text).Trim());
            _Command.Parameters.AddWithValue("@SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
            _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Amount Saved');", true);
        }
        catch (OdbcException)
        { Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Amount already exists for this Component !!!');", true); }
        finally { getComponentAmounts(Convert.ToString(ddlSelectComponent.SelectedValue)); }
    }
}