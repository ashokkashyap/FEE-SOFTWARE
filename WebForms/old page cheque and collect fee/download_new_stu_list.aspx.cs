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
public partial class WebForms_download_new_stu_list : System.Web.UI.Page
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
                // btn_dwn_std.DataSource = _dtblClasses; btn_dwn_std.DataTextField = "CLS"; btn_dwn_std.DataValueField = "CLASS_CODE"; ddlSelectClass.DataBind(); ddlSelectClass.Items.Insert(0, new ListItem("Select Class", ""));

            }
        }

    }

    protected void btn_dwn_std_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime varStartDate = Convert.ToDateTime(txtStartDate.Text);
            var SQL = "CALL `sp_dwn_new_stuList1`('" + Convert.ToString(varStartDate.ToString("yyyy-MM-dd")) + "')";
            //Response.Write(SQL);
            //Response.End();
            //OdbcDataAdapter objAdapter = new OdbcDataAdapter("SELECT A.STUDENT_ID,A.STUDENT_REGISTRATION_NBR,A.FIRST_NAME,A.MIDDLE_NAME,A.LAST_NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.NO_OF_COMMUNICATION, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B WHERE A.CLASS_CODE = B.CLASS_CODE and A.CLASS_CODE= '"+ddlSelectClass.SelectedValue+"' ORDER BY B.CLASS_PRIORITY,B.CLASS_SECTION,A.FIRST_NAME", _Connection);
            //OdbcDataAdapter objAdapter = new OdbcDataAdapter("SELECT A.*,B.* FROM ign_student_master A,ign_class_master B WHERE A.CLASS_CODE = B.CLASS_CODE ORDER BY B.CLASS_PRIORITY,B.CLASS_SECTION,A.FIRST_NAME", _Connection);
            //OdbcDataAdapter objAdapter = new OdbcDataAdapter("select a.STUDENT_ID,a.STUDENT_REGISTRATION_NBR as Admission_No,concat(a.FIRST_NAME,' ',a.MIDDLE_NAME,' ',a.LAST_NAME) as Name,concat(b.CLASS_NAME,'-',b.CLASS_SECTION) as Class,a.STUDENT_ROLL_NBR as Roll_no,a.FATHER_NAME,a.MOTHER_NAME,a.DATE_OF_ADMISSION as Admission_Date,a.BIRTH_DATE as DOB, a.GENDER,a.BLOOD_GROUP,a.CATEGORY,a.ADDRESS_LINE1,a.NO_OF_COMMUNICATIONfrom ign_student_master a,ign_class_master b where a.CLASS_CODE=b.CLASS_CODE and a.CREATE_DATE>=2014-04-16 order by b.CLASS_PRIORITY,b.CLASS_SECTION", _Connection);
            _Command.CommandText = SQL; _Command.CommandType = CommandType.StoredProcedure;
            OdbcDataAdapter _dtAdapter = new OdbcDataAdapter(); _dtAdapter.SelectCommand = _Command;
            DataSet objDataSet = new DataSet();
            _dtAdapter.Fill(objDataSet);

            Response.Clear();

            HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
            HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

            #region Row1
            objHtmlTableRow = new HtmlTableRow();
            foreach (DataColumn objDataColumn in objDataSet.Tables[0].Columns)
            {
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "left";
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
                    objHtmlTableCell.Align = "left";
                    objHtmlTableCell.Attributes.Add("STYLE", "font-weight:1;");
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
        catch (Exception ex)
        {
            //Response.Write(ex);
            //Response.Clear();
        }
    }
}
