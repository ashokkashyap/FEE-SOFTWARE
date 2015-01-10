using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.IO;

public partial class WebForms_download_student_master_transport : System.Web.UI.Page
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
                var SQL = "call spClassMaster()";
                var _dtAdapter = new OdbcDataAdapter(); var _dtblClasses = new DataTable();
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblClasses);
                ViewState["_dtblClasses"] = _dtblClasses;
                ddlSelectClass.DataSource = _dtblClasses; ddlSelectClass.DataTextField = "CLS"; ddlSelectClass.DataValueField = "CLASS_CODE"; ddlSelectClass.DataBind(); ddlSelectClass.Items.Insert(0, new ListItem("Select Class", ""));


            }
        }
    }
    protected void ddlclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        OdbcDataAdapter objAdapter = new OdbcDataAdapter("select concat(a.FIRST_NAME,' ',a.MIDDLE_NAME,' ',a.LAST_NAME) as name, concat(d.CLASS_NAME ,'-', d.CLASS_SECTION) as class ,a.STUDENT_REGISTRATION_NBR , a.FATHER_NAME, a.ADDRESS_LINE1, c.ROUTE_NAME, c.REMARKS from ign_student_master a , ign_bus_route_student_mapping b, ign_bus_route_master c , ign_class_master d where b.STUDENT_ID = a.STUDENT_ID and b.BUS_ROUTE_ID = c.BUS_ROUTE_ID and a.CLASS_CODE = d.CLASS_CODE  and A.CLASS_CODE= '" + ddlSelectClass.SelectedValue + "' ORDER BY A.FIRST_NAME", _Connection);
        DataSet objDataSet = new DataSet();
        objAdapter.Fill(objDataSet);

        Response.Clear();

        HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
        HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

        #region Row1
        objHtmlTableRow = new HtmlTableRow();
        foreach (DataColumn objDataColumn in objDataSet.Tables[0].Columns)
        {
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Align = "center";
            objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
            objHtmlTableCell.InnerText = objDataColumn.ColumnName;
            objHtmlTableRow.Controls.Add(objHtmlTableCell);
            objHtmlTable.Controls.Add(objHtmlTableRow);
        }
        #endregion
        #region StudentRows
        foreach (DataRow objDataRow in objDataSet.Tables[0].Rows)
        {
            int i = 0;
            objHtmlTableRow = new HtmlTableRow();
            foreach (DataColumn objDataColumn in objDataSet.Tables[0].Columns)
            {
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "center";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                if (objDataColumn.ColumnName.Equals("BIRTH_DATE") || objDataColumn.ColumnName.Equals("DATE_OF_ADMISSION") || objDataColumn.ColumnName.Equals("CREATE_DATE"))
                {
                    if (Convert.ToString(objDataRow[i]).Length > 0)
                    {
                        objHtmlTableCell.InnerText = Convert.ToDateTime(objDataRow[i]).ToString("dd-MMM-yyyy");
                    }
                }
                else if (objDataColumn.ColumnName.Equals("CLASS_NAME"))
                {
                    objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);
                }
                else
                {
                    objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);
                }
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                i++;
            }
        }
        #endregion
        Response.AddHeader("content-disposition", "attachment;filename=StudentMaster.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        System.IO.StringWriter StringWriter = new System.IO.StringWriter();
        HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);
        objHtmlTable.RenderControl(HtmlTextWriter);
        Response.Write(StringWriter.ToString());
        Response.End();
    }
}