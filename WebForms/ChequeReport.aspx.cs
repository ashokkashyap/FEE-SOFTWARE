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

public partial class WebForms_ChequeReport : System.Web.UI.Page
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
            OdbcDataAdapter objAdapter = new OdbcDataAdapter();
            DataSet objDataSet = new DataSet();
            // OdbcDataAdapter objAdapter = new OdbcDataAdapter("select a.CHEQUE_NUMBER, a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from collect_component_detail a where a.CREATE_DATE = '"+ Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd")+"' and a.MODE='CHEQUE'", _Connection);
            if (chklessdetails.Checked == false)
            {
                if (ddlcts.SelectedItem.Text == "ALL")
                {
                    //objAdapter = new OdbcDataAdapter("select a.CHEQUE_NUMBER,concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME)as name,b.FATHER_NAME,concat(c.CLASS_NAME,'-',c.CLASS_SECTION)as class,b.STUDENT_REGISTRATION_NBR,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE", _Connection);
                    objAdapter = new OdbcDataAdapter("select @s:=@s+1 Sr_No,b.STUDENT_REGISTRATION_NBR as ADM_NO, concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME)as STUDENT_NAME,b.FATHER_NAME,concat(c.CLASS_NAME,'-',c.CLASS_SECTION)as CLASS,a.CHEQUE_NUMBER,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from (SELECT @s:=0) as s,collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE", _Connection);

                }
                else
                {
                   // objAdapter = new OdbcDataAdapter("select a.CHEQUE_NUMBER,concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME)as name,b.FATHER_NAME,concat(c.CLASS_NAME,'-',c.CLASS_SECTION)as class,b.STUDENT_REGISTRATION_NBR,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and a.cts='" + ddlcts.SelectedItem.Text + "'", _Connection);
                    objAdapter = new OdbcDataAdapter("select @s:=@s+1 Sr_No,b.STUDENT_REGISTRATION_NBR as ADM_NO, concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME)as STUDENT_NAME,b.FATHER_NAME,concat(c.CLASS_NAME,'-',c.CLASS_SECTION)as CLASS,a.CHEQUE_NUMBER,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from (SELECT @s:=0) as s,collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and a.cts='" + ddlcts.SelectedItem.Text + "'", _Connection);
                }
                objAdapter.Fill(objDataSet);


                Response.Clear();

                HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
                HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

                #region Row1
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 10; objHtmlTableCell.Align = "center";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Happy Home Public School";
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion
                #region Row2
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 9; objHtmlTableCell.Align = "center";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Cheque Received on: " + txtStartDate.Text;
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion

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
                if (ddlcts.SelectedIndex > 0)
                {
                    if (ddlcts.SelectedItem.Text == "ALL")
                    {

                       // _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE'";
                        _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE'";
                    }
                    if (ddlcts.SelectedItem.Text == "CTS")
                    {
                        //_Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE' and Cts='CTS'";
                        _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE' and Cts='CTS'";

                    }

                    if (ddlcts.SelectedItem.Text == "NON CTS")
                    {
                        //_Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE' and Cts='NON CTS'";
                        _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE' and Cts='NON CTS'";

                    }
                    //else
                    //{
                    //    _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE'";

                    //}
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "alert('Please Select Cts Type First !!!')", true);

                }
                int iCount = Convert.ToInt32(_Command.ExecuteScalar());

                #region Row2
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 10; objHtmlTableCell.Align = "right";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Total: " + iCount;
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion


                #endregion
                Response.AddHeader("content-disposition", "attachment;filename=chequeMaster.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.xls";
                System.IO.StringWriter StringWriter = new System.IO.StringWriter();
                HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);
                objHtmlTable.RenderControl(HtmlTextWriter);
                Response.Write(StringWriter.ToString());
                Response.End();
            }
            
            if (chklessdetails.Checked == true) 
            {
                if (ddlcts.SelectedItem.Text == "ALL")
                {
                   // objAdapter = new OdbcDataAdapter("select a.CHEQUE_NUMBER,b.STUDENT_REGISTRATION_NBR,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from collect_component_detail a,ign_student_master b,ign_class_master c where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE", _Connection);
                   // objAdapter = new OdbcDataAdapter("select a.CHEQUE_NUMBER,b.STUDENT_REGISTRATION_NBR,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE", _Connection);
                    objAdapter = new OdbcDataAdapter("select @s:=@s+1 Sr_No, b.STUDENT_REGISTRATION_NBR as ADM_NO,a.CHEQUE_NUMBER as CHEQUE_NO,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from (SELECT @s:=0) as s, collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE", _Connection);
                      
                }
                else
                {

                    //objAdapter = new OdbcDataAdapter("select a.CHEQUE_NUMBER,b.STUDENT_REGISTRATION_NBR,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from collect_component_detail a,ign_student_master b,ign_class_master c where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and a.cts='"+ddlcts.SelectedItem.Text+"'", _Connection);
                    //objAdapter = new OdbcDataAdapter("select a.CHEQUE_NUMBER,b.STUDENT_REGISTRATION_NBR,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and a.cts='" + ddlcts.SelectedItem.Text + "'", _Connection);
                    objAdapter = new OdbcDataAdapter("select @s:=@s+1 Sr_No, b.STUDENT_REGISTRATION_NBR as ADM_NO,a.CHEQUE_NUMBER as CHEQUE_NO ,a.CHEQUE_DATE,a.BANK_NAME, a.AMOUNT_PAID from (SELECT @s:=0) as s, collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.MODE='CHEQUE' and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and a.cts='" + ddlcts.SelectedItem.Text + "'", _Connection);

                }
                objAdapter.Fill(objDataSet);

                Response.Clear();

                HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
                HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

                #region Row1
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 6; objHtmlTableCell.Align = "center";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Happy Home Public School";
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion
                #region Row2
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 6; objHtmlTableCell.Align = "center";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Cheque Received on: " + txtStartDate.Text;
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion

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
                if (ddlcts.SelectedIndex > 0)
                {
                    if (ddlcts.SelectedItem.Text == "ALL")
                    {

                        //_Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE'";
                        _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE'";
                    }
                    if (ddlcts.SelectedItem.Text == "CTS")
                    {
                        //_Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE' and Cts='CTS'";
                        _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE' and Cts='CTS'";

                    }

                    if (ddlcts.SelectedItem.Text == "NON CTS")
                    {
                        //_Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE' and Cts='NON CTS'";
                        _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.PAID_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE' and Cts='NON CTS'";

                    }
                    //else
                    //{
                    //    _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CHEQUE'";

                    //}
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "alert('Please Select Cts Type First !!!')", true);

                }
                int iCount = Convert.ToInt32(_Command.ExecuteScalar());

                #region Row2
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 6; objHtmlTableCell.Align = "right";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Total: " + iCount;
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion


                #endregion
                Response.AddHeader("content-disposition", "attachment;filename=chequeMaster.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.xls";
                System.IO.StringWriter StringWriter = new System.IO.StringWriter();
                HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);
                objHtmlTable.RenderControl(HtmlTextWriter);
                Response.Write(StringWriter.ToString());
                Response.End();
            }
        }
       
            
           
        catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
            Response.Write(ex.Message);
        }
    }
}