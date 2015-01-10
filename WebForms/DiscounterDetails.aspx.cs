using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_DiscounterDetails : System.Web.UI.Page
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
            { getDicounters(); }
        }
    }
    protected void btnInsert_Click(object sender, ImageClickEventArgs e)
    {
        _Command.CommandText = "select count(*) from discounter_master where NAME='" + txtAddDiscounterName.Text.Trim() + "';";
        if (Convert.ToInt32(_Command.ExecuteScalar()) == 0)
        {
            _Command.CommandText = "insert into discounter_master(NAME,DESCRIPTION) values('" + txtAddDiscounterName.Text.Trim() + "','" + txtAddDescription.Text.Trim() + "')";
            _Command.ExecuteNonQuery();
            txtAddDiscounterName.Text = "";
            txtAddDescription.Text = "";
            getDicounters();
        }
    }
    protected void btnUpdateDetails_Click(object sender, ImageClickEventArgs e)
    {
        //_Command.CommandText = "select count(*) from discounter_master where NAME='" + txtUDiscounterName.Text.Trim() + "' and DISCOUNTER_ID = '" + ddlSelectDiscounter.SelectedValue.ToString() + "';";
        _Command.CommandText = "update discounter_master set NAME='" + txtUDiscounterName.Text.Trim() + "',DESCRIPTION='" + txtUDiscounterDescription.Text.Trim() + "' where DISCOUNTER_ID = '" + ddlSelectDiscounter.SelectedValue.ToString() + "';";
        _Command.ExecuteNonQuery();
        txtUDiscounterName.Text = "";
        txtUDiscounterDescription.Text = "";
        getDicounters();
    }
    private void getDicounters()
    {
        _Command.CommandText = "CALL `spDiscounterMaster`()";
        OdbcDataReader _dtReader = _Command.ExecuteReader();
        DataTable _dtblDiscounters = new DataTable();
        _dtblDiscounters.Load(_dtReader);
        _dtReader.Close(); _dtReader.Dispose();
        ddlSelectDiscounter.DataSource = _dtblDiscounters; ddlSelectDiscounter.DataTextField = "NAME"; ddlSelectDiscounter.DataValueField = "DISCOUNTER_ID"; ddlSelectDiscounter.DataBind(); ddlSelectDiscounter.Items.Insert(0, new ListItem("select", ""));
        repDiscounterList.DataSource = _dtblDiscounters; repDiscounterList.DataBind();
        ViewState["_dtblDiscounters"] = _dtblDiscounters;
    }
    protected void ddlSelectDiscounter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSelectDiscounter.SelectedIndex > 0)
        {
            DataTable _dtblDiscounters = (DataTable)ViewState["_dtblDiscounters"];
            var DiscounterDetails = from dd in _dtblDiscounters.AsEnumerable() where Convert.ToInt32(dd["DISCOUNTER_ID"]).Equals(Convert.ToInt32(ddlSelectDiscounter.SelectedValue)) select dd;
            foreach (var _DiscounterDetails in DiscounterDetails)
            {
                txtUDiscounterName.Text = Convert.ToString(_DiscounterDetails["NAME"]);
                txtUDiscounterDescription.Text = Convert.ToString(_DiscounterDetails["DESCRIPTION"]);
            }
            //txtUDiscounterName.Text = Convert.ToString(ddlSelectDiscounter.SelectedItem);
            //txtUDiscounterDescription.Text = Convert.ToString(ddlSelectDiscounter.SelectedValue);
        }
        else
        {
            txtUDiscounterName.Text = "";
            txtUDiscounterDescription.Text = "";
        }
    }
}