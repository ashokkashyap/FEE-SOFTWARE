using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_addDiscountDetails : System.Web.UI.Page
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
                getDiscounts();
            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }

    private void getDiscounts()
    {
        var SQL = "call spDiscountMaster";
        var _dtblDiscounts = new DataTable();
        var _dtAdapter = new OdbcDataAdapter(SQL, _Connection);
        _dtAdapter.Fill(_dtblDiscounts);
        ViewState["_dtblDiscounts"] = _dtblDiscounts;
        gvDiscounts.DataSource = _dtblDiscounts; gvDiscounts.DataBind();
        txtDiscountName.Text = ""; txtDiscountValue.Text = ""; rblDiscountType.SelectedIndex = 0;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var SQL = "insert into discount_master(DISCOUNT_NAME,DISCOUNT_TYPE,DISCOUNT_VALUE,SCHOOL_SESSION_ID) values(?,?,?,?)";
        _Command.Parameters.AddWithValue("@DISCOUNT_NAME", Convert.ToString(txtDiscountName.Text).Trim());
        _Command.Parameters.AddWithValue("@DISCOUNT_TYPE", Convert.ToString(rblDiscountType.SelectedValue));
        _Command.Parameters.AddWithValue("@DISCOUNT_VALUE", Convert.ToInt32(txtDiscountValue.Text));
        _Command.Parameters.AddWithValue("@SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
        _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Discount Saved');", true);
        getDiscounts();
    }
}