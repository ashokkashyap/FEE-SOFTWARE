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

public partial class WebForms_Cashreport : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader _dtReader = null;
    DateTime StartDate = DateTime.Now;
    DateTime EndDate = DateTime.Now;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            txtStartDate.Attributes.Add("ReadOnly", "true");

            if (!IsPostBack)
            {
                txtStartDate.Text = DateTime.Now.ToString("dd-MMMM-yyyy");


            }
        }
    }
    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            OdbcDataAdapter objAdapter = new OdbcDataAdapter("SELECT concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as name ,b.FATHER_NAME ,  concat(c.CLASS_NAME,'-',c.CLASS_SECTION) as class ,b.STUDENT_REGISTRATION_NBR,a.AMOUNT_PAID FROM collect_component_detail a , ign_student_master b , ign_class_master c WHERE a.CREATE_DATE ='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CASH' and a.STUDENT_ID =b.STUDENT_ID and b.CLASS_CODE = c.CLASS_CODE", _Connection);
            DataSet objDataSet = new DataSet();
            objAdapter.Fill(objDataSet);

            Response.Clear();

            HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
            HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

            #region Row1
            objHtmlTableRow = new HtmlTableRow();
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.ColSpan = 5; objHtmlTableCell.Align = "center";
            objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
            objHtmlTableCell.InnerText = "HIMALAYA PUBLIC SR. SEC. SCHOOL";
            objHtmlTableRow.Controls.Add(objHtmlTableCell);
            objHtmlTable.Controls.Add(objHtmlTableRow);
            #endregion
            #region Row2
            objHtmlTableRow = new HtmlTableRow();
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.ColSpan = 5; objHtmlTableCell.Align = "center";
            objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
            objHtmlTableCell.InnerText = "Cash Received on: " + txtStartDate.Text;
            objHtmlTableRow.Controls.Add(objHtmlTableCell);
            objHtmlTable.Controls.Add(objHtmlTableRow);
            #endregion

            #region Row1
            objHtmlTableRow = new HtmlTableRow();
            foreach (DataColumn objDataColumn in objDataSet.Tables[0].Columns)
            {
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "right";
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
                    if (objDataColumn.ColumnName.Equals("CHEQUE_DATE"))
                    {
                        if (Convert.ToString(objDataRow[i]).Length > 0)
                        {
                            objHtmlTableCell.InnerText = Convert.ToDateTime(objDataRow[i]).ToString("dd-MMM-yyyy");
                        }
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

            _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CASH'";
            int iCount = Convert.ToInt32(_Command.ExecuteScalar());

            #region Row2
            objHtmlTableRow = new HtmlTableRow();
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.ColSpan =5; objHtmlTableCell.Align = "right";
            objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
            objHtmlTableCell.InnerText = "Total: " + iCount;
            objHtmlTableRow.Controls.Add(objHtmlTableCell);
            objHtmlTable.Controls.Add(objHtmlTableRow);
            #endregion


            #endregion
            Response.AddHeader("content-disposition", "attachment;filename=cashMaster.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            System.IO.StringWriter StringWriter = new System.IO.StringWriter();
            HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);
            objHtmlTable.RenderControl(HtmlTextWriter);
            Response.Write(StringWriter.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
            Response.Write(ex.Message);
        }
    }
}