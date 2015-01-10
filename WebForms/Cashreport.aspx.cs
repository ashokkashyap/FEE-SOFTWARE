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
        //try
        {
            if (chkdetails.Checked == false)
            {

               // Response.Write("SELECT concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as name ,b.FATHER_NAME ,  concat(c.CLASS_NAME,'-',c.CLASS_SECTION) as class ,b.STUDENT_REGISTRATION_NBR,a.AMOUNT_PAID FROM collect_component_detail a , ign_student_master b , ign_class_master c WHERE a.PAID_DATE ='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CASH' and a.STUDENT_ID =b.STUDENT_ID and b.CLASS_CODE = c.CLASS_CODE");
               // Response.End();
                //OdbcDataAdapter objAdapter = new OdbcDataAdapter("SELECT concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as name ,b.FATHER_NAME ,  concat(c.CLASS_NAME,'-',c.CLASS_SECTION) as class ,b.STUDENT_REGISTRATION_NBR,a.AMOUNT_PAID FROM collect_component_detail a , ign_student_master b , ign_class_master c WHERE a.PAID_DATE ='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CASH' and a.STUDENT_ID =b.STUDENT_ID and b.CLASS_CODE = c.CLASS_CODE", _Connection);
                OdbcDataAdapter objAdapter = new OdbcDataAdapter("SELECT @s:=@s+1 SN, b.STUDENT_REGISTRATION_NBR as AD_NO,a.rno as Rpt_No,concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as STUDENT_NAME  ,  concat(c.CLASS_NAME,'-',c.CLASS_SECTION) as CLASS ,concat( date_format(min(d.MAPPED_DATE),'%b'),'- ',date_format(max(d.MAPPED_DATE),'%b')) as FEE_FOR,a.AMOUNT_PAID as Paid,case a.mode when 'CASH' then a.MODE else ifnull( a.CHEQUE_NUMBER,0)  end as Mode FROM (SELECT @s:=0) as s, collect_component_detail a , ign_student_master b , ign_class_master c,collect_component_master d WHERE a.PAID_DATE ='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.STUDENT_ID =b.STUDENT_ID and b.CLASS_CODE = c.CLASS_CODE and a.student_id=d.student_id and a.paid_date=d.paid_date group by a.student_id,a.id order by SN", _Connection);

                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);

                Response.Clear();

                HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
                HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

                #region Row1
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 8; objHtmlTableCell.Align = "center";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Happy Home Public School";
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion
                #region Row2
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 8; objHtmlTableCell.Align = "center";
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
                        string classname = Convert.ToString(objDataRow[i]);

                        if (objDataColumn.ColumnName.Equals("CHEQUE_DATE"))
                        {
                            if (Convert.ToString(objDataRow[i]).Length > 0)
                            {
                                objHtmlTableCell.InnerText = Convert.ToDateTime(objDataRow[i]).ToString("dd-MMM-yyyy");
                            }
                        }

                        else if (classname == "PRE PRIMARY-A")
                        {
                           // objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);

                            objHtmlTableCell.InnerText = "PP-A";
                        }

                        else if (classname == "PRE PRIMARY-B")
                        {
                            // objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);

                            objHtmlTableCell.InnerText = "PP-B";
                        }

                        else if (classname == "PRE PRIMARY-C")
                        {
                            // objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);

                            objHtmlTableCell.InnerText = "PP-C";
                        }

                        else if (classname == "PRE PRIMARY-D")
                        {
                            // objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);

                            objHtmlTableCell.InnerText = "PP-D";
                        }

                        else if (classname == "PRE SCHOOL-A")
                        {
                            // objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);

                            objHtmlTableCell.InnerText = "PS-A";
                        }
                        else if (classname == "PRE SCHOOL-B")
                        {
                            // objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);

                            objHtmlTableCell.InnerText = "PS-B";
                        }

                        else if (classname == "PRE SCHOOL-C")
                        {
                            // objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);

                            objHtmlTableCell.InnerText = "PS-C";
                        }
                        else if (classname == "PLAY GROUP-A")
                        {
                            // objHtmlTableCell.InnerText = Convert.ToString(objDataRow[i]);

                            objHtmlTableCell.InnerText = "PG-A";
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

                _Command.CommandText = "select ifnull(sum(a.AMOUNT_PAID),0) from collect_component_detail a where a.paid_date = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "'";
                int iCount = Convert.ToInt32(_Command.ExecuteScalar());

                #region Row2
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 8; objHtmlTableCell.Align = "right";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Grand Total:                                       " + iCount;
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion


                _Command.CommandText = "SELECT  COUNT(A.STUDENT_ID)  FROM collect_component_detail A  WHERE A.PAID_DATE='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "'";
                string totalfeecount = Convert.ToString(_Command.ExecuteScalar());

                #region Row3
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 4; objHtmlTableCell.Align = "right";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Total Fee Count: " + totalfeecount;
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion

                _Command.CommandText = "SELECT COUNT(DISTINCT( A.STUDENT_ID))  FROM collect_component_master A WHERE  A.PAID_DATE='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND A.COMPONENT_ID='6'";
                string totaltptcount = Convert.ToString(_Command.ExecuteScalar());

                #region Row4
                
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 4; objHtmlTableCell.Align = "right";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Total Transport Count Count: " + totaltptcount;
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion
                _Command.CommandText = "select ifnull(sum(a.AMOUNT_PAID ),0) from collect_component_detail a where a.paid_date = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.mode='Cash'";
                int totalcash = Convert.ToInt32(_Command.ExecuteScalar());

                #region Row5
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 4; objHtmlTableCell.Align = "right";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Total Cash Recieved: " + totalcash;
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion
                _Command.CommandText = "select ifnull( sum(a.AMOUNT_PAID ),0) from collect_component_detail a where a.paid_date = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and a.mode='Cheque'";
                int totalcheque = Convert.ToInt32(_Command.ExecuteScalar());

                #region Row6
              
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 4; objHtmlTableCell.Align = "right";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Total Bank Recived: " + totalcheque;
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);

                GridView1.DataSource = objHtmlTable;
                GridView1.DataBind();
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
            if (chkdetails.Checked == true)
            {
                //OdbcDataAdapter objAdapter = new OdbcDataAdapter("SELECT b.STUDENT_REGISTRATION_NBR,a.AMOUNT_PAID FROM collect_component_detail a , ign_student_master b , ign_class_master c WHERE a.PAID_DATE ='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CASH' and a.STUDENT_ID =b.STUDENT_ID and b.CLASS_CODE = c.CLASS_CODE", _Connection);
                OdbcDataAdapter objAdapter = new OdbcDataAdapter("SELECT @s:=@s+1 Sr_No, b.STUDENT_REGISTRATION_NBR as ADM_NO,a.AMOUNT_PAID FROM (SELECT @s:=0) as s, collect_component_detail a , ign_student_master b , ign_class_master c WHERE a.PAID_DATE ='" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CASH' and a.STUDENT_ID =b.STUDENT_ID and b.CLASS_CODE = c.CLASS_CODE", _Connection);

                DataSet objDataSet = new DataSet();
                objAdapter.Fill(objDataSet);

                Response.Clear();

                HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 1;
                HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

                #region Row1
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "center";
                objHtmlTableCell.Attributes.Add("STYLE", "font-weight:bold;");
                objHtmlTableCell.InnerText = "Happy Home Public School";
                objHtmlTableRow.Controls.Add(objHtmlTableCell);
                objHtmlTable.Controls.Add(objHtmlTableRow);
                #endregion
                #region Row2
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "center";
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

                _Command.CommandText = "select sum(a.AMOUNT_PAID ) from collect_component_detail a where a.CREATE_DATE = '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' AND a.MODE='CASH'";
                int iCount = Convert.ToInt32(_Command.ExecuteScalar());

                #region Row2
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.ColSpan = 3; objHtmlTableCell.Align = "right";
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
        }
       // catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
            //Response.Write(ex.Message);
        }
    }
}