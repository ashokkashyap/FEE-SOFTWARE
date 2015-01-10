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


public partial class WebForms_Download_EWS_STUDENTS : System.Web.UI.Page
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
                bindddlclass();
                var SQL = "call spClassMaster()";
                var _dtAdapter = new OdbcDataAdapter(); var _dtblClasses = new DataTable();
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblClasses);
                ViewState["_dtblClasses"] = _dtblClasses;
                ddlclass.DataSource = _dtblClasses; ddlclass.DataTextField = "CLS"; ddlclass.DataValueField = "CLASS_CODE"; ddlclass.DataBind(); ddlclass.Items.Insert(0, new ListItem("Select Class", ""));

            }
        }
    }



    protected void btnsubmit_Click(object sender, EventArgs e)
    {


        DataSet objDataSet = new DataSet();
        DataTable DT = new DataTable();
        OdbcDataAdapter objAdapter;
        if (ddlcatogery.SelectedItem.Text == "TUTION FEE(50%)")
        {
            if (ddlclasswithoutsection.SelectedIndex > 0)
            {
                  objAdapter = new OdbcDataAdapter("SELECT distinct A.STUDENT_REGISTRATION_NBR,CONCAT(A.FIRST_NAME,' ',A.MIDDLE_NAME,' ',A.LAST_NAME) AS NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.MOTHER_NAME,A.GENDER,ifnull(DATE_FORMAT(A.DATE_OF_ADMISSION,'%d-%b-%Y'),'N') as adm_date,ifnull(DATE_FORMAT(A.BIRTH_DATE,'%d-%b-%Y'),'NOT UPDATED') as BRTH_date,A.NO_OF_COMMUNICATION, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B WHERE A.CLASS_CODE = B.CLASS_CODE  and  B.class_name='" + ddlclasswithoutsection.SelectedItem.Text + "' and  A.STUDENT_ID  in (select distinct Student_id  from student_discount_mapping where discount_id='39') ORDER BY CAST(A.STUDENT_REGISTRATION_NBR AS UNSIGNED)", _Connection);

            }
            else
            {
                  objAdapter = new OdbcDataAdapter("SELECT distinct A.STUDENT_REGISTRATION_NBR,CONCAT(A.FIRST_NAME,' ',A.MIDDLE_NAME,' ',A.LAST_NAME) AS NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.MOTHER_NAME,A.GENDER,ifnull(DATE_FORMAT(A.DATE_OF_ADMISSION,'%d-%b-%Y'),'N') as adm_date,ifnull(DATE_FORMAT(A.BIRTH_DATE,'%d-%b-%Y'),'NOT UPDATED') as BRTH_date,A.NO_OF_COMMUNICATION, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B WHERE A.CLASS_CODE = B.CLASS_CODE  and  B.class_code='" + ddlclass.SelectedValue + "' and  A.STUDENT_ID  in (select distinct Student_id  from student_discount_mapping where discount_id='39') ORDER BY CAST(A.STUDENT_REGISTRATION_NBR AS UNSIGNED)", _Connection);

            }
            objDataSet = new DataSet();
            objAdapter.Fill(DT);
        }
        else if (ddlcatogery.SelectedItem.Text == "EWS")
        {
            if (ddlclasswithoutsection.SelectedIndex > 0)
            {
                objAdapter = new OdbcDataAdapter("SELECT distinct A.STUDENT_REGISTRATION_NBR,CONCAT(A.FIRST_NAME,' ',A.MIDDLE_NAME,' ',A.LAST_NAME) AS NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.MOTHER_NAME,A.GENDER,ifnull(DATE_FORMAT(A.DATE_OF_ADMISSION,'%d-%b-%Y'),'N') as adm_date,ifnull(DATE_FORMAT(A.BIRTH_DATE,'%d-%b-%Y'),'NOT UPDATED') as BRTH_date,A.NO_OF_COMMUNICATION, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE  and a.student_id=c.student_id and c.COMPONENT_ID='11'and B.class_name='" + ddlclasswithoutsection.SelectedItem.Text + "'  ORDER BY cast(A.STUDENT_REGISTRATION_NBR as unsigned)", _Connection);
            }
            else
            {
                objAdapter = new OdbcDataAdapter("SELECT distinct A.STUDENT_REGISTRATION_NBR,CONCAT(A.FIRST_NAME,' ',A.MIDDLE_NAME,' ',A.LAST_NAME) AS NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.MOTHER_NAME,A.GENDER,ifnull(DATE_FORMAT(A.DATE_OF_ADMISSION,'%d-%b-%Y'),'N') as adm_date,ifnull(DATE_FORMAT(A.BIRTH_DATE,'%d-%b-%Y'),'NOT UPDATED') as BRTH_date,A.NO_OF_COMMUNICATION, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE  and a.student_id=c.student_id and c.COMPONENT_ID='11'and B.class_code='" + ddlclass.SelectedValue + "'  ORDER BY cast(A.STUDENT_REGISTRATION_NBR as unsigned)", _Connection);

            }
            objDataSet = new DataSet();
            objAdapter.Fill(DT);
        }
        else
        {
            if (ddlclasswithoutsection.SelectedIndex > 0)
            {
                objAdapter = new OdbcDataAdapter("SELECT distinct A.STUDENT_REGISTRATION_NBR,CONCAT(A.FIRST_NAME,' ',A.MIDDLE_NAME,' ',A.LAST_NAME) AS NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.MOTHER_NAME,A.GENDER,ifnull(DATE_FORMAT(A.DATE_OF_ADMISSION,'%d-%b-%Y'),'N') as adm_date,ifnull(DATE_FORMAT(A.BIRTH_DATE,'%d-%b-%Y'),'NOT UPDATED') as BRTH_date,A.NO_OF_COMMUNICATION, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE and B.class_name='" + ddlclasswithoutsection.SelectedItem.Text + "' and a.student_id=c.student_id  ORDER BY cast(A.STUDENT_REGISTRATION_NBR as unsigned)", _Connection);
            }
            else
            {
                objAdapter = new OdbcDataAdapter("SELECT distinct A.STUDENT_REGISTRATION_NBR,CONCAT(A.FIRST_NAME,' ',A.MIDDLE_NAME,' ',A.LAST_NAME) AS NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.MOTHER_NAME,A.GENDER,ifnull(DATE_FORMAT(A.DATE_OF_ADMISSION,'%d-%b-%Y'),'N') as adm_date,ifnull(DATE_FORMAT(A.BIRTH_DATE,'%d-%b-%Y'),'NOT UPDATED') as BRTH_date,A.NO_OF_COMMUNICATION, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE and B.class_code='" + ddlclass.SelectedValue+ "' and a.student_id=c.student_id  ORDER BY cast(A.STUDENT_REGISTRATION_NBR as unsigned)", _Connection);

            }
            objDataSet = new DataSet();
            objAdapter.Fill(DT);
        }
        Response.Clear();

        gvRecords.DataSource = DT;
        gvRecords.DataBind();


        //DataSet objDataSet = new DataSet();
        //if (ddlclass.SelectedIndex > 0)
        //{
        //    OdbcDataAdapter objAdapter = new OdbcDataAdapter("SELECT A.STUDENT_ID,A.STUDENT_REGISTRATION_NBR,A.FIRST_NAME,A.MIDDLE_NAME,A.LAST_NAME,A.FATHER_OCCUPATION,A.MOTHER_NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.NO_OF_COMMUNICATION, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE and c.component_id='11' and a.student_id=c.student_id and a.class_code='" + ddlclass.SelectedValue + "' ORDER BY cast(A.STUDENT_REGISTRATION_NBR as unsigned)", _Connection);
        //      objDataSet = new DataSet();
        //    objAdapter.Fill(objDataSet);
        //}
        //else
        //{
        //    OdbcDataAdapter objAdapter = new OdbcDataAdapter("SELECT distinct A.STUDENT_ID,A.STUDENT_REGISTRATION_NBR,A.FIRST_NAME,A.MIDDLE_NAME,A.LAST_NAME,A.FATHER_OCCUPATION,A.MOTHER_NAME,CONCAT(B.CLASS_NAME,'-',IFNULL(B.CLASS_SECTION,'')) as CLASS_NAME,A.FATHER_NAME,A.NO_OF_COMMUNICATION, A.ADDRESS_LINE1 FROM ign_student_master A,ign_class_master B,collect_component_master c WHERE A.CLASS_CODE = B.CLASS_CODE and c.component_id='11' and a.student_id=c.student_id  ORDER BY cast(A.STUDENT_REGISTRATION_NBR as unsigned)", _Connection);
        //     objDataSet = new DataSet();
        //    objAdapter.Fill(objDataSet);
        //}

        //Response.Clear();

        //HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
        //HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

        //#region Row1
        //objHtmlTableRow = new HtmlTableRow();
        //foreach (DataColumn objDataColumn in objDataSet.Tables[0].Columns)
        //{
        //    objHtmlTableCell = new HtmlTableCell();
        //    objHtmlTableCell.Align = "center";
        //    objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;font-color:blue;");
        //    objHtmlTableCell.InnerText = objDataColumn.ColumnName;
        //    objHtmlTableRow.Controls.Add(objHtmlTableCell);
        //    objHtmlTable.Controls.Add(objHtmlTableRow);
        //}
        //#endregion
        //#region StudentRows
        //foreach (DataRow objDataRow in objDataSet.Tables[0].Rows)
        //{
        //    int i = 0;
        //    objHtmlTableRow = new HtmlTableRow();
        //    foreach (DataColumn objDataColumn in objDataSet.Tables[0].Columns)
        //    {
        //        objHtmlTableCell = new HtmlTableCell();
        //        objHtmlTableCell.Align = "center";
        //        objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
        //        if (objDataColumn.ColumnName.Equals("BIRTH_DATE") || objDataColumn.ColumnName.Equals("DATE_OF_ADMISSION") || objDataColumn.ColumnName.Equals("CREATE_DATE"))
        //        {
        //            if (Convert.ToString(objDataRow[i]).Length > 0)
        //            {
        //                objHtmlTableCell.InnerText = Convert.ToDateTime(objDataRow[i]).ToString("dd-MMM-yyyy");
        //            }
        //        }
        //        else if (objDataColumn.ColumnName.Equals("CLASS_NAME"))
        //        {
        //            objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);
        //        }
        //        else
        //        {
        //            objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);
        //        }
        //        objHtmlTableRow.Controls.Add(objHtmlTableCell);
        //        objHtmlTable.Controls.Add(objHtmlTableRow);
        //        i++;
        //    }
        //}
        //#endregion
        //Response.AddHeader("content-disposition", "attachment;filename=EWSStudentMaster.xls");
        //Response.Charset = "";
        //Response.ContentType = "application/vnd.xls";
        //System.IO.StringWriter StringWriter = new System.IO.StringWriter();
        //HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);
        //objHtmlTable.RenderControl(HtmlTextWriter);
        //Response.Write(StringWriter.ToString());
        //Response.End();
    }
    protected void ddlcatogery_SelectedIndexChanged(object sender, EventArgs e)
    {

        //HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
        //HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

        //#region Row1
        //objHtmlTableRow = new HtmlTableRow();
        //foreach (DataColumn objDataColumn in objDataSet.Tables[0].Columns)
        //{
        //    objHtmlTableCell = new HtmlTableCell();
        //    objHtmlTableCell.Align = "center";
        //    objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;font-color:blue;");
        //    objHtmlTableCell.InnerText = objDataColumn.ColumnName;
        //    objHtmlTableRow.Controls.Add(objHtmlTableCell);
        //    objHtmlTable.Controls.Add(objHtmlTableRow);
        //}
        //#endregion
        //#region StudentRows
        //foreach (DataRow objDataRow in objDataSet.Tables[0].Rows)
        //{
        //    int i = 0;
        //    objHtmlTableRow = new HtmlTableRow();
        //    foreach (DataColumn objDataColumn in objDataSet.Tables[0].Columns)
        //    {
        //        objHtmlTableCell = new HtmlTableCell();
        //        objHtmlTableCell.Align = "center";
        //        objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
        //        if (objDataColumn.ColumnName.Equals("BIRTH_DATE") || objDataColumn.ColumnName.Equals("DATE_OF_ADMISSION") || objDataColumn.ColumnName.Equals("CREATE_DATE"))
        //        {
        //            if (Convert.ToString(objDataRow[i]).Length > 0)
        //            {
        //                objHtmlTableCell.InnerText = Convert.ToDateTime(objDataRow[i]).ToString("dd-MMM-yyyy");
        //            }
        //        }
        //        else if (objDataColumn.ColumnName.Equals("CLASS_NAME"))
        //        {
        //            objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);
        //        }
        //        else
        //        {
        //            objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);
        //        }
        //        objHtmlTableRow.Controls.Add(objHtmlTableCell);
        //        objHtmlTable.Controls.Add(objHtmlTableRow);
        //        i++;
        //    }
        //}
        //#endregion
        //Response.AddHeader("content-disposition", "attachment;filename=EWSStudentMaster.xls");
        //Response.Charset = "";
        //Response.ContentType = "application/vnd.xls";
        //System.IO.StringWriter StringWriter = new System.IO.StringWriter();
        //HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);
        //objHtmlTable.RenderControl(HtmlTextWriter);
        //Response.Write(StringWriter.ToString());
        //Response.End();
    }
    protected void gvRecords_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);





            TableCell HeaderCell = new TableCell();
 


            HeaderCell.Text = ddlcatogery.SelectedItem.Text +" STUDENTS";


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

    public void bindddlclass()
    {

        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select cm.CLASS_NAME,cm.CLASS_PRIORITY from ign_class_master cm group by cm.CLASS_NAME order by cm.CLASS_PRIORITY,cm.CLASS_NAME,cm.CLASS_SECTION", _Connection));
      DataTable dt = new DataTable();
      odbc.Fill(dt);
      ddlclasswithoutsection.DataSource = dt; ddlclasswithoutsection.DataTextField = "CLASS_NAME"; ddlclasswithoutsection.DataValueField = "CLASS_PRIORITY"; ddlclasswithoutsection.DataBind(); ddlclasswithoutsection.Items.Insert(0, new ListItem("Select Class", ""));
       
    }


    protected void ddlclasswithoutsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlclass.SelectedIndex = 0;
    }
    protected void ddlclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlclasswithoutsection.SelectedIndex = 0;
    }
}