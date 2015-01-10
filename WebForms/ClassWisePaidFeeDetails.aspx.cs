using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Drawing;

public partial class WebForms_ClassWisePaidFeeDetails : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader _dtReader = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["_Connection"]) != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                var SQL = "call spClassMaster";
                var _dtAdapter = new OdbcDataAdapter(); var _dtblClasses = new DataTable();
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblClasses);
                ViewState["_dtblClasses"] = _dtblClasses;
                ddlClassList.DataSource = _dtblClasses; ddlClassList.DataTextField = "CLS"; ddlClassList.DataValueField = "CLASS_CODE"; ddlClassList.DataBind(); ddlClassList.Items.Insert(0, new ListItem("SELECT CLASS", ""));
            }


        }

    }

    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            if (txtEndDate.Text.Equals(""))
            {
                txtEndDate.Text = DateTime.Now.ToShortDateString();

            }
            else
            {
                var sQL = "call spClassWisePaidFeeDetailsFromSessionIdAndBetweenDate('" + ddlClassList.Text + "','" + Convert.ToString(Session["_SessionID"]) + "','" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "')";
                //Response.Write(sQL);
                //Response.End();
                _Command.CommandText = sQL; _dtReader = _Command.ExecuteReader();
                DataTable _dtblRecords = new DataTable();
                _dtblRecords.Load(_dtReader);
                gvRecords.DataSource = _dtblRecords; gvRecords.DataBind();

            }
            dwnExlFile.Visible = true;
        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }

   
    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void dwnExlFile_Click(object sender, ImageClickEventArgs e)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ClassWisePaidRecord.xls"));
        Response.ContentType = "application/ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        gvRecords.AllowPaging = false;
        gvRecords.HeaderRow.Style.Add("background-color", "#FFFFFF");
        for (int i = 0; i < gvRecords.HeaderRow.Cells.Count; i++)
        {
            gvRecords.HeaderRow.Cells[i].Style.Add("background-color", "#507cd1");
        }
        int j = 1;
        foreach (GridViewRow _row in gvRecords.Rows)
        {
            _row.BackColor = Color.White;
            if (j <= gvRecords.Rows.Count)
            {
                if (j % 2 != 0)
                {
                    for (int k = 0; k < _row.Cells.Count; k++)
                    {
                        _row.Cells[k].Style.Add("background-color", "#EFF3FB");
                    }
                }
            }
            j++;
        }
        gvRecords.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }
}