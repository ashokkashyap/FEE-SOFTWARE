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

public partial class admin_defaulterReport : System.Web.UI.Page
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
                ddlCLassName.DataSource = _dtblClasses; ddlCLassName.DataTextField = "CLS"; ddlCLassName.DataValueField = "CLASS_CODE"; ddlCLassName.DataBind(); ddlCLassName.Items.Insert(0, new ListItem("Select Class", ""));
                
            
            }
        }
    }
    protected void btnDownloadExcel_Click(object sender, EventArgs e)
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
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        pnlDetails.Controls.Add(GenerateRecordSet());
    }

    private HtmlTable GenerateRecordSet()
    {
        HtmlTable _HtmlTable = new HtmlTable(); _HtmlTable.Border = 1; _HtmlTable.BorderColor = "#FFAB60";
        HtmlTableRow _TableRow = null;
        HtmlTableCell _TableCell = null;

        _TableRow = new HtmlTableRow();
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("STUDENT ID"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("STUDENT NAME"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("ADMISSION #"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("CLASS"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("FATHER NAME"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("ADDRESS"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("# OF COMMUNICATION"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("AMOUNT PAYABLE"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("AMOUNT PAID"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
        _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString("AMOUNT DUE"); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;background-color:#FFFF99;"); _TableRow.Cells.Add(_TableCell);
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
            if (_dtblStudentRecords.Rows.Count > 0)
            {

                foreach (DataRow _row in _dtblStudentRecords.Rows)
                {
                    if (!Convert.ToString(_row["STUDENT_ID"]).Equals(0))
                    {
                        if (!Convert.ToString(_row["Payble"]).Equals("0"))
                        {
                            _TableRow = new HtmlTableRow();
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["STUDENT_ID"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["SNAME"]); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["STUDENT_REGISTRATION_NBR"]); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["CLASS"]); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["FATHER_NAME"]); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["ADDRESS_LINE1"]); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["NO_OF_COMMUNICATION"]); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["AMOUNT_PAYBLE"]); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["AMOUNT_PAID"]); _TableRow.Cells.Add(_TableCell);
                            _TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["Payble"]); _TableRow.Cells.Add(_TableCell);
                            _HtmlTable.Rows.Add(_TableRow);
                        }
                    }
                }
            }
         
            btnDownloadExcel.Visible = true;
            return _HtmlTable;
        }

    }
}
                   
                    //OdbcDataReader _dtReader = null;
                    //int varAmountPayable = 0, varAmountPaid = 0, varComponentDiscount = 0, varExtraDiscount = 0, varFine = 0;
                    //sQL = "CALL `spCollectedDetailsFromStudentIDAndMappedDate`('" + Convert.ToString(_row["STUDENT_ID"]) + "', '" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
                    //_Command.CommandText = sQL;
                    //_Command.CommandType = CommandType.StoredProcedure;
                    //_dtReader = _Command.ExecuteReader();
                    //while (_dtReader.Read())
                    //{
                    //    varAmountPayable = Convert.ToInt32(_dtReader["AMOUNT_PAYBLE"]);
                    //    varAmountPaid = Convert.ToInt32(_dtReader["AMOUNT_PAID"]);
                    //    varComponentDiscount = Convert.ToInt32(_dtReader["DISCOUNT"]);
                    //} _dtReader.Close();


                   // Response.Write(Convert.ToString(_row["STUDENT_ID"]));
                   // Response.Write(Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")));
                   //// Response.End();
                   // sQL = "CALL `spCollectComponentDetailFromStudentIDAndMappedDate`('" + Convert.ToString(_row["STUDENT_ID"]) + "', '" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
                   // _Command.CommandText = sQL; _Command.CommandType = CommandType.StoredProcedure;
                   // _dtReader = _Command.ExecuteReader();
                   // while (_dtReader.Read())
                   // {
                   //     varExtraDiscount = Convert.ToInt32(_dtReader["FINE"]);
                   //     varFine = Convert.ToInt32(_dtReader["DISCOUNT"]);
                   // } _dtReader.Close();
                  
                    
                    
                    
                    //int CalculatedValue = varAmountPayable + varFine - varAmountPaid - varComponentDiscount - varExtraDiscount;
                    //if (CalculatedValue > 0)
                    //{
                        
                        // Response.Write("/table start if");
                        // _TableRow = new HtmlTableRow();
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["STUDENT_ID"]); _TableCell.Attributes.Add("style", "font-family:Calibri;font-size:12px;color:Black;"); _TableRow.Cells.Add(_TableCell);
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["SNAME"]); _TableRow.Cells.Add(_TableCell);
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["STUDENT_REGISTRATION_NBR"]); _TableRow.Cells.Add(_TableCell);
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["CLASS"]); _TableRow.Cells.Add(_TableCell);
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["FATHER_NAME"]); _TableRow.Cells.Add(_TableCell);
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["ADDRESS_LINE1"]); _TableRow.Cells.Add(_TableCell);
                       
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(_row["NO_OF_COMMUNICATION"]); _TableRow.Cells.Add(_TableCell);
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(varAmountPayable); _TableRow.Cells.Add(_TableCell);
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(varAmountPaid); _TableRow.Cells.Add(_TableCell);
                        //_TableCell = new HtmlTableCell(); _TableCell.InnerText = Convert.ToString(CalculatedValue); _TableRow.Cells.Add(_TableCell);
                   // }
                    
        //        }
        //        //Response.End();
        //    }
         
        //    btnDownloadExcel.Visible = true;
        //    return _HtmlTable;
        //}
        ////catch
        ////{
        ////    btnDownloadExcel.Visible = true;
        ////    return _HtmlTable;
        ////}
   