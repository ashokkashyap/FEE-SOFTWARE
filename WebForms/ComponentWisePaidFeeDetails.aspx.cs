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

public partial class WebForms_ComponentWisePaidFeeDetails : System.Web.UI.Page
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
                var SQL = "call spComponentMaster";
                var _dtblComponents = new DataTable();
                var _dtAdapter = new OdbcDataAdapter(SQL, _Connection);
                _dtAdapter.Fill(_dtblComponents);
                ViewState["_dtblComponents"] = _dtblComponents;
                ddlComponenetList.DataSource = _dtblComponents; ddlComponenetList.DataTextField = "COMPONENT_NAME"; ddlComponenetList.DataValueField = "COMPONENT_ID"; ddlComponenetList.DataBind(); ddlComponenetList.Items.Insert(0, new ListItem("Select Component", ""));
            }


        }
    }
    protected void ddlComponenetList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvRecords_RowCreated(object sender, GridViewRowEventArgs e)
    {
       // var sQL = "select * from collect_component_master";
       //     _Command.CommandText = sQL;
       // _Connection = (OdbcConnection)Session["_Connection"];

       // _dtReader = _Command.ExecuteReader();

        if (e.Row.RowType == DataControlRowType.Header)
        {

            GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell2 = new TableCell();
            //HeaderCell2.Text = "NAME";
            //HeaderCell2.ColumnSpan = 1;
            //HeaderRow.Cells.Add(HeaderCell2);

            //if (Convert.ToString(_dtReader["COMPONENT_NAME"]).Contains("TUTION"))
            //{
            //    HeaderCell2 = new TableCell();
            //    HeaderCell2.Text = Convert.ToString(ddlComponenetList.SelectedItem.Text);
            //    HeaderCell2.ColumnSpan = 5;
            //    HeaderRow.Cells.Add(HeaderCell2);
            //}
            //else if (Convert.ToString(_dtReader["COMPONENT_NAME"]).Contains("DEVELOPMENT"))
            //{
            //    HeaderCell2 = new TableCell();
            //    HeaderCell2.Text = Convert.ToString(ddlComponenetList.SelectedItem.Text);
            //    HeaderCell2.ColumnSpan = 5;
            //    HeaderRow.Cells.Add(HeaderCell2);
            //}
            //else
            {
                HeaderCell2 = new TableCell();
                HeaderCell2.Text = Convert.ToString(ddlComponenetList.SelectedItem.Text);
                HeaderCell2.ColumnSpan = 1;
                HeaderRow.Cells.Add(HeaderCell2);
            }
            gvRecords.Controls[0].Controls.AddAt(0, HeaderRow);

        }

        //if (e.Row.RowType == DataControlRowType.Footer)
        //{
        //    GridViewRow footerRow = new GridViewRow(1, 0, DataControlRowType.Footer, DataControlRowState.Insert);
        //    TableCell footercell1 = new TableCell();
        //    footercell1.Text = "total collection";
        //    footercell1.ColumnSpan = 2;
        //    footerRow.Cells.Add(footercell1);
        //    gvRecords.Controls[0].Controls.AddAt(0, footerRow);
        //  //  footercell1 = new TableCell();
        //}
    }


    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {
       // try
        {

            if (txtEndDate.Text.Equals(""))
            {
                txtEndDate.Text = DateTime.Now.ToShortDateString();

            }
            else
            {
                var sQL = "call spComponentWisePaidFeeDetailsBetweenDate('" + ddlComponenetList.Text + "','" + Convert.ToString(Session["_SessionID"]) + "','" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "')";
                
                _Command.CommandText = sQL; _dtReader = _Command.ExecuteReader();
                DataTable _dtblRecords = new DataTable();
                _dtblRecords.Load(_dtReader);
                gvRecords.DataSource = _dtblRecords; gvRecords.DataBind();
                _dtReader.Close();

                Label lbl = gvRecords.FooterRow.FindControl("lbltotal") as Label;
                sQL = "select sum(amount_paid) as paid from collect_component_master where component_id='" + ddlComponenetList.SelectedValue + "' and paid_date between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToString(Session["_SessionID"]) + "' order by paid_date";
                
                //Response.Write(sQL);
                //Response.End();
                _Command.CommandText = sQL;
               _dtReader= _Command.ExecuteReader();
                
                while (_dtReader.Read())
                {
                    lbl.Text = Convert.ToString(_dtReader["paid"]);

                }


            }
            dwnExlFile.Visible = true;
        }
       // catch (Exception ex)
        {
           // Response.Write(ex);
        }
    }
    protected void dwnExlFile_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ComponentWisePaidRecord.xls"));
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
        catch (Exception ex)
        {
            Response.Write(ex);
            throw ex;
            //Response.Redirect("~/Logout.aspx");
        }
        //Response.Clear();
    }
}