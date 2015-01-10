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

public partial class WebForms_DailyReport : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader _dtReader = null;  
    DateTime StartDate = DateTime.Now;
    DateTime EndDate = DateTime.Now;
    string minrecno = "";
    string maxrecno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            txtStrtDate.Attributes.Add("ReadOnly", "true");

            if (!IsPostBack)
            {

                txtStrtDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
                bindddlcomponent();
                txtEndDate.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");

               
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

            if (txtStrtDate.Text == "")
            {
                txtStrtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            }

            
               string varss =  "select min(a.Rno) as min ,max(a.Rno) as max from collect_component_detail a  where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
               _Command = new OdbcCommand();
               _Connection = (OdbcConnection)Session["_Connection"];
               _Command.Connection = _Connection;
               _dtReader = null;  
               _Command.CommandText = varss;
                
                //_Command.CommandText = "select min(a.Rno) as min ,max(a.Rno) as max from collect_component_detail a  where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";

            
            _dtReader = _Command.ExecuteReader();

            if (_dtReader.Read())
            {
                minrecno = Convert.ToString(_dtReader["min"]);
                maxrecno = Convert.ToString(_dtReader["max"]);
            } _dtReader.Close();
          
            HeaderCell.Text = "DAILY REPORT - "+ Convert.ToDateTime(txtStrtDate.Text).ToString("dd-MMMM-yyyy")+"                RNO ("+Convert.ToString( minrecno) +" TO " +Convert.ToString( maxrecno)+" )";

           
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
       
       
        try
        {
            if (ddlcomponent.SelectedIndex > 0)
            {

                if (txtEndDate.Text.Equals(""))
                {
                    txtEndDate.Text = DateTime.Now.ToShortDateString();

                }
                else
                {

                    var sQL = "select b.STUDENT_REGISTRATION_NBR as ADM_NO, concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as STUDENT_NAME,b.FATHER_NAME,concat(c.CLASS_NAME,'-',c.CLASS_SECTION)as CLASS,date_format(a.PAID_DATE,'%e-%b-%y')as PAID_DATE, a.MODE,a.CHEQUE_NUMBER,concat(date_format(a.CHEQUE_DATE,'%e-%b'),'</br>',a.BANK_NAME) as CHEQUE_Details, a.BANK_NAME,sum(d.AMOUNT_PAID) as AMOUNT_PAID,a.Rno,case a.MODE when 'cash' then  a.MODE else a.cheque_number end as modee,concat( SUBSTRING(monthname(min((d.MAPPED_DATE))),1,3),' ',SUBSTRING(monthname(max(d.MAPPED_DATE)),1,3)) as months from collect_component_detail a,ign_student_master b,ign_class_master c, collect_component_master d where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and d.STUDENT_ID=a.STUDENT_ID and a.ID=d.DETAIL_ID and d.component_id='" + Convert.ToString(ddlcomponent.SelectedValue) + "' group by a.Rno,d.DETAIL_ID ORDER BY a.paid_date,c.class_code";

                  //var sQL = "select b.STUDENT_REGISTRATION_NBR as ADM_NO, concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as STUDENT_NAME,b.FATHER_NAME,concat(c.CLASS_NAME,'-',c.CLASS_SECTION)as CLASS,date_format(a.PAID_DATE,'%e-%b-%y')as PAID_DATE, a.MODE,a.CHEQUE_NUMBER,concat(date_format(a.CHEQUE_DATE,'%e-%b'),'</br>',a.bank_name) as CHEQUE_Details, a.BANK_NAME,a.AMOUNT_PAID as AMOUNT_PAID,a.Rno,case a.MODE when 'cash' then  a.MODE else a.cheque_number end as modee,concat( SUBSTRING(monthname(min((d.MAPPED_DATE))),1,3),'-',SUBSTRING(monthname(max(d.MAPPED_DATE)),1,3)) as months from collect_component_detail a,ign_student_master b,ign_class_master c, collect_component_master d where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and d.STUDENT_ID=a.STUDENT_ID and a.ID=d.DETAIL_ID group by a.Rno,d.DETAIL_ID ORDER BY a.paid_date,c.class_code";

                    
                    _Command.CommandText = sQL; _dtReader = _Command.ExecuteReader();
                    DataTable _dtblRecords = new DataTable();
                    _dtblRecords.Load(_dtReader);
                    gvRecords.DataSource = _dtblRecords; gvRecords.DataBind();
                    _dtReader.Close();




                    Label lbl = gvRecords.FooterRow.FindControl("lbltotal") as Label;
                  //  sQL = "select sum(a.AMOUNT_PAID) + sum(a.fine) as paid from collect_component_detail a,ign_student_master b,ign_class_master c,collect_component_master d  where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and a.STUDENT_ID=d.STUDENT_ID  and d.COMPONENT_ID='"+Convert.ToString(ddlcomponent.SelectedValue)+"' and a.PAID_DATE=d.PAID_DATE";
                    sQL = "select  sum(a.AMOUNT_PAID) as PAID from collect_component_master a where a.COMPONENT_ID='"+Convert.ToString(ddlcomponent.SelectedValue)+"' and a.PAID_DATE BETWEEN '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
                    
 

                    _Command.CommandText = sQL;
                    _dtReader = _Command.ExecuteReader();

                    while (_dtReader.Read())
                    {
                        lbl.Text = Convert.ToString(_dtReader["paid"]);

                    } _dtReader.Close(); _dtReader.Dispose();

                    sQL = "select  sum(a.AMOUNT_PAID) from collect_component_master a  where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  and a.COMPONENT_ID = '"+Convert.ToString(ddlcomponent.SelectedValue)+"' and a.STUDENT_ID in(select student_id from collect_component_detail where mode='CASH' and PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  )";
                    _Command.CommandText = sQL;
                    int cash = Convert.ToInt32(_Command.ExecuteScalar());

                    Label lblcash = gvRecords.FooterRow.FindControl("lblcash") as Label;

                    lblcash.Text = Convert.ToString(cash);

                    sQL = "select ifnull(sum(a.AMOUNT_PAID),0)  from collect_component_master a  where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  and a.COMPONENT_ID = '" + Convert.ToString(ddlcomponent.SelectedValue) + "' and a.STUDENT_ID in(select student_id from collect_component_detail where mode='CHEQUE' and PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  )";
                    _Command.CommandText = sQL;
                    int cheque = Convert.ToInt32(_Command.ExecuteScalar());
                    Label lblcheque = gvRecords.FooterRow.FindControl("lblcheque") as Label;
                    lblcheque.Text = Convert.ToString(cheque);
                }
            }

            else
            {

                if (txtEndDate.Text.Equals(""))
                {
                    txtEndDate.Text = DateTime.Now.ToShortDateString();

                }
                else
                {

                    // var sQL = "select b.STUDENT_REGISTRATION_NBR as ADM_NO, concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as STUDENT_NAME,b.FATHER_NAME,concat(c.CLASS_NAME,'-',c.CLASS_SECTION)as CLASS,date_format(PAID_DATE,'%e-%M-%Y')as PAID_DATE, a.MODE,a.CHEQUE_NUMBER,a.CHEQUE_DATE, a.BANK_NAME, a.AMOUNT_PAID from collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate).ToString("yyyy-MM-dd") + "'  and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE";
                    var sQL = "select b.STUDENT_REGISTRATION_NBR as ADM_NO, concat(b.FIRST_NAME,' ',b.MIDDLE_NAME,' ',b.LAST_NAME) as STUDENT_NAME,b.FATHER_NAME,concat(c.CLASS_NAME,'-',c.CLASS_SECTION)as CLASS,date_format(a.PAID_DATE,'%e-%b-%y')as PAID_DATE, a.MODE,a.CHEQUE_NUMBER,concat(date_format(a.CHEQUE_DATE,'%e-%b'),'</br>',a.bank_name) as CHEQUE_Details, a.BANK_NAME,a.AMOUNT_PAID as AMOUNT_PAID,a.Rno,case a.MODE when 'cash' then  a.MODE else a.cheque_number end as modee,concat( SUBSTRING(monthname(min((d.MAPPED_DATE))),1,3),'-',SUBSTRING(monthname(max(d.MAPPED_DATE)),1,3)) as months from collect_component_detail a,ign_student_master b,ign_class_master c, collect_component_master d where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and d.STUDENT_ID=a.STUDENT_ID and a.ID=d.DETAIL_ID group by a.Rno,d.DETAIL_ID ORDER BY a.Rno, a.paid_date,c.class_code";

                    // var sQL = "SELECT b.STUDENT_REGISTRATION_NBR AS ADM_NO, CONCAT(b.FIRST_NAME,' ',b.MIDDLE_NAME,' '  ,b.LAST_NAME) AS STUDENT_NAME,b.FATHER_NAME, CONCAT(c.CLASS_NAME,'-',c.CLASS_SECTION)   AS CLASS, DATE_FORMAT(a.PAID_DATE,'%e-%M-%Y')   AS PAID_DATE, a.MODE,a.CHEQUE_NUMBER, DATE_FORMAT(a.CHEQUE_DATE,'%e-%M-%Y') AS   CHEQUE_DATE, a.BANK_NAME,a.AMOUNT_PAID + a.FINE AS AMOUNT_PAID,a.Rno,  concat( min(date_format(d.MAPPED_DATE,'%b')),'-',max(date_format(d.MAPPED_DATE,'%b')))   as months  FROM collect_component_detail a,ign_student_master b,ign_class_master c,collect_component_master d  WHERE a.PAID_DATE BETWEEN '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "' AND a.STUDENT_ID=b.STUDENT_ID   AND b.CLASS_CODE=c.CLASS_CODE  and a.ID=d.DETAIL_ID group by a.student_id ";
                    // var sQL="call 

                    //Response.Write(sQL);
                    //Response.End();
                    _Command.CommandText = sQL; _dtReader = _Command.ExecuteReader();
                    DataTable _dtblRecords = new DataTable();
                    _dtblRecords.Load(_dtReader);
                    gvRecords.DataSource = _dtblRecords; gvRecords.DataBind();
                    _dtReader.Close();




                    Label lbl = gvRecords.FooterRow.FindControl("lbltotal") as Label;
                    //sQL = "select sum(amount_paid) as paid from collect_component_master where paid_date between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToString(Session["_SessionID"]) + "' order by paid_date";
                    sQL = "select sum(a.AMOUNT_PAID)  as paid from collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE";

                    //Response.Write(sQL);
                    //Response.End();
                    _Command.CommandText = sQL;
                    _dtReader = _Command.ExecuteReader();

                    while (_dtReader.Read())
                    {
                        lbl.Text = Convert.ToString(_dtReader["paid"]);

                    } _dtReader.Close(); _dtReader.Dispose();

                    sQL = "select sum(a.AMOUNT_PAID) as paid from collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and a.mode='cash'";
                    _Command.CommandText = sQL;
                    int cash = Convert.ToInt32(_Command.ExecuteScalar());

                    Label lblcash = gvRecords.FooterRow.FindControl("lblcash") as Label;

                    lblcash.Text = Convert.ToString(cash);

                    sQL = "select ifnull( sum(a.AMOUNT_PAID),0) from collect_component_detail a,ign_student_master b,ign_class_master c where a.PAID_DATE between '" + Convert.ToDateTime(txtStrtDate.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'  and a.STUDENT_ID=b.STUDENT_ID and b.CLASS_CODE=c.CLASS_CODE and a.mode='cheque'";
                    _Command.CommandText = sQL;
                    int cheque = Convert.ToInt32(_Command.ExecuteScalar());
                    Label lblcheque = gvRecords.FooterRow.FindControl("lblcheque") as Label;
                    lblcheque.Text = Convert.ToString(cheque);
                }
            }
            dwnExlFile.Visible = true;
        }
        catch (Exception ex)
        {
            Response.Write(ex);
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
           // Response.Write(ex);
            throw ex;
            //Response.Redirect("~/Logout.aspx");
        }
        //Response.Clear();
    
    }
        protected void gvRecords_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void bindddlcomponent()
        {
            var SQL = "select * from component_master order by COMPONENT_FREQUENCY";
            var _dtAdapter = new OdbcDataAdapter(); var _dtblClasses = new DataTable();
            _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
            _dtAdapter.Fill(_dtblClasses);
            ViewState["_dtblClasses"] = _dtblClasses;
            ddlcomponent.DataSource = _dtblClasses; ddlcomponent.DataTextField = "COMPONENT_NAME"; ddlcomponent.DataValueField = "COMPONENT_ID"; ddlcomponent.DataBind(); ddlcomponent.Items.Insert(0, new ListItem("Select Component", ""));
  

        }
   


}


   