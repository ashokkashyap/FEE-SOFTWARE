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

public partial class WebForms_chequeBounceReport : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; DataTable _dtblRecords = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            _dtblRecords = new DataTable();
            if (!IsPostBack)
            {
                var SQL = "CALL `spChequeBounceReport`()";
                _Command.CommandText = SQL;
                var _dtAdapter = new OdbcDataAdapter(); _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblRecords);
                rpChequeDetails.DataSource = _dtblRecords; rpChequeDetails.DataBind();
                if (_dtblRecords.Rows.Count > 0)
                {
                    btnDownloadExcel.Visible = true;
                    ViewState["_dtblRecords"] = _dtblRecords;
                }
                else
                {
                    btnDownloadExcel.Visible = false;
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('No Record found !!!');", true);
                }

            }
        }
    }
    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        _dtblRecords = (DataTable)ViewState["_dtblRecords"];
        HtmlTable _HtmlTable = new HtmlTable(); _HtmlTable.Border = 1; _HtmlTable.BorderColor = "#FFAB60";
        HtmlTableRow _TableRow = null;
        HtmlTableCell _TableCell = null;

        _TableRow = new HtmlTableRow();
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("SNO"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:14px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("STUDENT NAME"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:14px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("CLASS"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:14px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("CHEQUE DATE"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:14px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("BANK DETAILS"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:14px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _HtmlTable.Rows.Add(_TableRow);

        foreach (DataRow _row in _dtblRecords.Rows)
        {
            _TableRow = new HtmlTableRow();
            foreach (DataColumn _column in _dtblRecords.Columns)
            {
                if (_column.ColumnName != "ID" && _column.ColumnName != "BOUNCE_STATUS")
                {
                    _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row[_column]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                }
            } _HtmlTable.Rows.Add(_TableRow);
        }

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        _HtmlTable.RenderControl(hw);

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=ChequeBounceReportAsOn" + DateTime.Now.ToString("dd-MM-yyyy").Trim().Replace(" ", "_") + ".xls");
        Response.ContentType = "application/vnd.ms-excel";
        this.EnableViewState = false;
        Response.Write(sw.ToString());
        Response.End();
    }
}