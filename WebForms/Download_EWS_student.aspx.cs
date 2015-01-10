using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Web.UI.HtmlControls;

public partial class WebForms_Download_EWS_student : System.Web.UI.Page
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
                ddlclass.DataSource = _dtblClasses; ddlclass.DataTextField = "CLS"; ddlclass.DataValueField = "CLASS_CODE"; ddlclass.DataBind(); ddlclass.Items.Insert(0, new ListItem("ALL CLASS", ""));

                DetailsList();

            }
        }
    }



    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        
        OdbcDataAdapter objAdapter = new OdbcDataAdapter();
        DataSet objDataSet = new DataSet();

        if (ddlclass.SelectedItem.Text == "ALL CLASS")
        {
            objAdapter = new OdbcDataAdapter("SELECT distinct A.STUDENT_REGISTRATION_NBR as ADM_No,concat(A.FIRST_NAME,' ',A.MIDDLE_NAME,' ',A.LAST_NAME) AS STUDENT_NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.MOTHER_NAME,A.NO_OF_COMMUNICATION as Contact_No, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE and c.component_id='27' and a.student_id=c.student_id  ORDER BY B.CLASS_PRIORITY,B.CLASS_SECTION,A.FIRST_NAME", _Connection);

        }

        else
        {
            objAdapter = new OdbcDataAdapter("SELECT distinct A.STUDENT_REGISTRATION_NBR as ADM_No,concat(A.FIRST_NAME,' ',A.MIDDLE_NAME,' ',A.LAST_NAME) AS STUDENT_NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.MOTHER_NAME,A.NO_OF_COMMUNICATION as Contact_No, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE and c.component_id='27' and a.student_id=c.student_id and a.class_code='" + ddlclass.SelectedValue + "' ORDER BY B.CLASS_PRIORITY,B.CLASS_SECTION,A.FIRST_NAME", _Connection);

        }
       
        objAdapter.Fill(objDataSet);

        Response.Clear();

        HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
        HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

        #region Row1
        objHtmlTableRow = new HtmlTableRow();
        foreach (DataColumn objDataColumn in objDataSet.Tables[0].Columns)
        {
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Align = "left";
            objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;font-color:blue;");
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

    public void DetailsList()
    {
        if (ddlclass.SelectedItem.Text == "ALL CLASS")
        {
            _Command = new OdbcCommand("SELECT  count(distinct c.STUDENT_ID) as totalStdTransportMap FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE and c.component_id='27' and a.student_id=c.student_id ", _Connection);
        }
        else
        {
            _Command = new OdbcCommand("SELECT  count(distinct c.STUDENT_ID) as totalStdTransportMap FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE and c.component_id='27' and a.student_id=c.student_id and a.class_code='" + ddlclass.SelectedValue + "' ", _Connection);


        }
        lblTotalStudentMap.Text = ": " + Convert.ToString(_Command.ExecuteScalar());

    }
    protected void ddlclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        DetailsList();
    }
}