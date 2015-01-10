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

public partial class WebForms_defaulterReport : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcCommand _Command1 = null; OdbcDataReader dtreader = null;
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
                var SQL = "call spClassMaster()";
                var _dtAdapter = new OdbcDataAdapter(); var _dtblClasses = new DataTable();
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblClasses);
                ViewState["_dtblClasses"] = _dtblClasses;
                ddlCLassName.DataSource = _dtblClasses; ddlCLassName.DataTextField = "CLS"; ddlCLassName.DataValueField = "CLASS_CODE"; ddlCLassName.DataBind(); ddlCLassName.Items.Insert(0, new ListItem("Select Class", ""));

                
            }
        }
    }
    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        if (chkcount.Checked == true)
        {
            HtmlTable _htmlTable = count();
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            _htmlTable.RenderControl(hw);

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=DefaulterReportAsOn" + Convert.ToDateTime(txtEndDate.Text).ToString("dd-MM-yyyy").Trim().Replace(" ", "_") + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            this.EnableViewState = false;
            Response.Write(sw.ToString());
            Response.End();
        }
        else
        {
            HtmlTable _htmlTable = GenerateRecordSet();
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            _htmlTable.RenderControl(hw);

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=DefaulterReportAsOn" + Convert.ToDateTime(txtEndDate.Text).ToString("dd-MM-yyyy").Trim().Replace(" ", "_") + ".xls");
            Response.ContentType = "application/vnd.ms-excel";
            this.EnableViewState = false;
            Response.Write(sw.ToString());
            Response.End();
        }
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        if (chkcount.Checked == true)
        {
            pnlDetails.Controls.Add(count());
        }
        else
        {
            pnlDetails.Controls.Add(GenerateRecordSet());
        }

    }

    private HtmlTable count()
    {
           int total_balace = 0;
        HtmlTable _HtmlTable = new HtmlTable(); _HtmlTable.Border = 1; _HtmlTable.BorderColor = "#FFAB60";
        HtmlTableRow _TableRow = null;
        HtmlTableCell _TableCell = null;

        _TableRow = new HtmlTableRow();
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("CLASS "); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("Total Student "); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
       
        
        _HtmlTable.Rows.Add(_TableRow);

        //try
        {

            DateTime varMappedDate = Convert.ToDateTime(txtEndDate.Text);
            var sQL="";
            if(ddlCLassName.SelectedIndex == 0)
            {
                
                    sQL  = "CALL `spStudentDetailsFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
            
            }
            else
            {

                    sQL = "CALL `spStudentDetailsClassFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "','"+ddlCLassName.SelectedValue+"')";
            
            
            }

                
                //var sQL = "CALL `spStudentDetailsFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
            _Command.CommandText = sQL; _Command.CommandType = CommandType.StoredProcedure;
            OdbcDataAdapter _dtAdapter = new OdbcDataAdapter(); _dtAdapter.SelectCommand = _Command;
            DataTable _dtblStudentRecords = new DataTable();
            _dtAdapter.Fill(_dtblStudentRecords);

            sQL = "select a.CLASS_CODE as code, CONCAT(IFNULL(a.CLASS_NAME,' '),'-', IFNULL(a.CLASS_SECTION,' ')) as class  from ign_class_master a order by a.CLASS_PRIORITY,a.CLASS_SECTION";
            _Command.CommandText = sQL;
            OdbcDataAdapter odbc = new OdbcDataAdapter(); odbc.SelectCommand = _Command;
            DataTable dt = new DataTable();
            odbc.Fill(dt);



            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    int count= 0;
                    string clas = Convert.ToString(row["class"]);



                    if (_dtblStudentRecords.Rows.Count > 0)
                    {

                        foreach (DataRow _row in _dtblStudentRecords.Rows)
                        {
                            if (!Convert.ToString(_row["STUDENT_ID"]).Equals(0))
                            {
                                int payable = Convert.ToInt32((_row["Payble"]));

                                string clas2 = Convert.ToString((_row["CLASS"]));

                                if (clas == clas2)
                                {
                                    if (payable > 0)
                                    {
                                        count++;
                                    }
                                }
                            }



                        }

                            _TableRow = new HtmlTableRow();

                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(clas); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(count); _TableRow.Cells.Add(_TableCell);



                            _HtmlTable.Rows.Add(_TableRow);
                       
                    }
                }
            }

           
            _TableRow = new HtmlTableRow();
           
            _HtmlTable.Rows.Add(_TableRow);

            btnDownloadExcel.Visible = true;
            return _HtmlTable;
        }

    }
    


    private HtmlTable GenerateRecordSet()
    {
       
        HtmlTable _HtmlTable = new HtmlTable(); _HtmlTable.Border = 1; _HtmlTable.BorderColor = "#FFAB60";
        HtmlTableRow _TableRow = null;
        HtmlTableCell _TableCell = null;
        if (ddlclasswithoutsection.SelectedIndex > 0)
        {
            _Command1.CommandText = "select a.CLASS_CODE from ign_class_master a  where a.CLASS_NAME='" + ddlclasswithoutsection.SelectedItem.Text + "' ";
        }
        else
        {
            _Command1.CommandText = "select a.CLASS_CODE from ign_class_master a  where a.CLASS_code='" + ddlCLassName.SelectedValue + "' ";

        }
        dtreader = _Command1.ExecuteReader();

        while (dtreader.Read())
        {
            int total_balace = 0;

            int class_code = Convert.ToInt32(dtreader["class_code"]);
            _Command.CommandText = "select concat(a.CLASS_NAME,' ',a.CLASS_SECTION) as class from ign_class_master a  where a.CLASS_CODE='"+class_code+"'  ";
            var CLASSNAME = Convert.ToString(_Command.ExecuteScalar());

            _TableRow = new HtmlTableRow();
            _TableCell = new HtmlTableCell(); _TableCell.ColSpan = 10; _TableCell.Align = "center"; _TableCell.InnerText = Convert.ToString("HAPPY HOME PUBLIC SCHOOL"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _HtmlTable.Rows.Add(_TableRow);
            _TableRow = new HtmlTableRow();
            _TableCell = new HtmlTableCell(); _TableCell.ColSpan = 10; _TableCell.Align = "center"; _TableCell.InnerText = Convert.ToString("CLASS " + CLASSNAME); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _HtmlTable.Rows.Add(_TableRow);
            _TableRow = new HtmlTableRow();
            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("STUDENT ID"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("STUDENT NAME"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("ADMISSION #"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("CLASS"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("FATHER NAME"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("ADDRESS"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);

            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("# OF COMMUNICATION"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("AMOUNT PAYABLE"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("AMOUNT PAID"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("AMOUNT DUE"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
            // _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("PENDING MONTHS "); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);


            _HtmlTable.Rows.Add(_TableRow);

            //try
            {
                DateTime varMappedDate = Convert.ToDateTime(txtEndDate.Text);
                var sQL = "";
                if (ddlCLassName.SelectedIndex == 0 && ddlclasswithoutsection.SelectedIndex==0)
                {

                    sQL = "CALL `spStudentDetailsFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";

                }
                else
                {
                    sQL = "CALL `spStudentDetailsClassFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "','" +class_code + "')";


                }


                //var sQL = "CALL `spStudentDetailsFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
                _Command.CommandText = sQL; _Command.CommandType = CommandType.StoredProcedure;
                OdbcDataAdapter _dtAdapter = new OdbcDataAdapter(); _dtAdapter.SelectCommand = _Command;
                DataTable _dtblStudentRecords = new DataTable();
                _dtAdapter.Fill(_dtblStudentRecords);

                if (_dtblStudentRecords.Rows.Count > 0)
                {

                    foreach (DataRow _row in _dtblStudentRecords.Rows)
                    {
                        if (!Convert.ToString(_row["STUDENT_ID"]).Equals(0))
                        {
                            int payable = Convert.ToInt32((_row["Payble"]));
                            if (payable > 0)
                            {
                                _TableRow = new HtmlTableRow();
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["STUDENT_ID"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["SNAME"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["STUDENT_REGISTRATION_NBR"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["CLASS"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["FATHER_NAME"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["ADDRESS_LINE1"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["NO_OF_COMMUNICATION"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["AMOUNT_PAYBLE"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["AMOUNT_PAID"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                                total_balace = total_balace + Convert.ToInt32(_row["Payble"]);
                                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["Payble"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);



                                _HtmlTable.Rows.Add(_TableRow);
                            }



                        }

                    }


                }

                int i = total_balace;
                _TableRow = new HtmlTableRow();
                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("Total Due"); _TableCell.ColSpan = 9; _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                _TableCell = new HtmlTableCell(); _TableCell.InnerText = total_balace.ToString(); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                _HtmlTable.Rows.Add(_TableRow);
                _TableRow = new HtmlTableRow();
                _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------"); _TableCell.ColSpan = 10; _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:9px;color:Black;"); _TableRow.Cells.Add(_TableCell);

                _HtmlTable.Rows.Add(_TableRow);
                
            }
           
            btnDownloadExcel.Visible = true;
            
        }
        return _HtmlTable;

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
        ddlCLassName.SelectedIndex = 0;
    }
    protected void ddlCLassName_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlclasswithoutsection.SelectedIndex = 0;
    }
}
                   
                   