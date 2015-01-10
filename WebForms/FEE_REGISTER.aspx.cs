using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;


public partial class WebForms_FEE_REGISTER : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcCommand _Command1 = null; OdbcDataReader _dtReader = null;
    DateTime StartDate = DateTime.Now;
    DateTime EndDate = DateTime.Now;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;

            _Command1 = new OdbcCommand();
            _Command1.Connection = _Connection;
            
            if (!IsPostBack)
            {

                
                bindddlclass();
                
            }
        }
    }

    protected void gvRecords_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);





            TableCell HeaderCell = new TableCell();

          



            HeaderCell.Text = "FEE REGISTER - " + Convert.ToDateTime(DateTime.Now).ToString("dd-MMMM-yyyy");


            HeaderCell.ColumnSpan = 11;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvRecords.Controls[0].Controls.AddAt(0, HeaderGridRow);

            HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            HeaderCell = new TableCell();
            HeaderCell = new TableCell();
            HeaderCell.Text = "HAPPY HOME PUBLIC SCHOOL";
            HeaderCell.ColumnSpan = 11;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvRecords.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }


    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {
        DataTable _dtblFeeRecord = new DataTable();
        _dtblFeeRecord.Columns.Add("ADM NO");
        _dtblFeeRecord.Columns.Add("STUDENT NAME");
        _dtblFeeRecord.Columns.Add("FATHER NAME");
        _dtblFeeRecord.Columns.Add("MONTH NAME");

        _Command.CommandText = "SELECT  b.STUDENT_ID, b.STUDENT_REGISTRATION_NBR as ADM_NO, concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as STUDENT_NAME,b.FATHER_NAME ,b.CLASS_CODE  FROM    ign_student_master b where   b.CLASS_CODE='" + ddlclass.SelectedValue + "' ORDER BY STUDENT_NAME ";
        _dtReader = _Command.ExecuteReader();
       
        while (_dtReader.Read())
        {

            string monthname = "";

            DataRow _row = _dtblFeeRecord.NewRow();

            _Command1.CommandText = "SELECT COUNT(*) FROM collect_component_master WHERE STUDENT_ID='" + Convert.ToString(_dtReader["STUDENT_ID"]) + "' AND COMPONENT_ID='11'";
            var EWS = Convert.ToString(_Command1.ExecuteScalar());

            if (Convert.ToString(EWS) == "0")
            {
                _Command1.CommandText = "SELECT monthname(max( a.MAPPED_DATE)) FROM collect_component_master a where  a.PAID_DATE is not null and a.STUDENT_ID='" + Convert.ToString(_dtReader["STUDENT_ID"]) + "'";
                var month = Convert.ToString(_Command1.ExecuteScalar());


                if (Convert.ToString(month) == "")
                {
                    monthname = "NOTHING PAID";
                }
                else
                {
                    monthname = Convert.ToString(month);
                }
            }

            else
            {
                monthname = "EWS";
            }
            _row["ADM NO"] = Convert.ToString(_dtReader["ADM_NO"]);
            _row["STUDENT NAME"] = Convert.ToString(_dtReader["STUDENT_NAME"]);
            _row["FATHER NAME"] = Convert.ToString(_dtReader["FATHER_NAME"]);
            _row["MONTH NAME"] = Convert.ToString(monthname);

            _dtblFeeRecord.Rows.Add(_row);
            dwnExlFile.Visible = true;
        }

        gvRecords.DataSource = _dtblFeeRecord;
        gvRecords.DataBind();


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
            // Response.Write(ex);
            throw ex;
            //Response.Redirect("~/Logout.aspx");
        }
        //Response.Clear();

    }
    protected void gvRecords_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public void bindddlclass()
    {
        var SQL = "call spClassMaster()"; var _dtAdapter = new OdbcDataAdapter(); var _dtblClasses = new DataTable();
        _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
        _dtAdapter.Fill(_dtblClasses);
        ViewState["_dtblClasses"] = _dtblClasses;
        ddlclass.DataSource = _dtblClasses; ddlclass.DataTextField = "CLS"; ddlclass.DataValueField = "CLASS_CODE"; ddlclass.DataBind(); ddlclass.Items.Insert(0, new ListItem("Select Class", ""));
    }



}


