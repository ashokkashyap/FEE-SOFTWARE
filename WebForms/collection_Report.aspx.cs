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

public partial class WebForms_collection_Report : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader _dtReader = null;
    DateTime  StartDate = DateTime.Now; 
    DateTime EndDate = DateTime.Now;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            txtStartDate.Attributes.Add("ReadOnly", "true");
            txtEndDate.Attributes.Add("ReadOnly", "true");
            if (!IsPostBack)
            { }
        }

    }
    protected void btnGetDetails_Click(object sender, ImageClickEventArgs e)
    {
       
        btnDownloadExcel.Visible = true;
        pnlDetails.Controls.Add(FuncGenerateReoncileStatement());
    }

    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        HtmlTable _htmlTable = (FuncGenerateReoncileStatement());
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        _htmlTable.RenderControl(hw);

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=feeReport" + DateTime.Now.ToString("dd-MM-yyyy").Trim().Replace(" ", "_") + ".xls");
        Response.ContentType = "application/vnd.ms-excel";
        this.EnableViewState = false;
        Response.Write(sw.ToString());
        Response.End();
    }
    //public void getMonthDetails(int month, out DateTime StartDate, out DateTime EndDate)
    //{
    //    StartDate = DateTime.Now; EndDate = DateTime.Now;
    //    int year = Convert.ToInt32(Convert.ToString(ddlSelectSession.SelectedItem).Substring(0, 4));
    //    int days = DateTime.DaysInMonth(year, month);
    //    StartDate = new DateTime(year, month, 1);
    //    EndDate = new DateTime(year, month, days);
    //    //Response.Write(startDate + "<br/>" + endDate); Response.End();
    //}

    private HtmlTable FuncGenerateReoncileStatement()
    {
        //DataTable _dtblCollectionRecord = new DataTable();
        //Dictionary<int, string> objDictionaryComponentList = new Dictionary<int, string>();
        //Dictionary<int, double> objDictionaryComponentAmountList = new Dictionary<int, double>();
        //DateTime startDate, endDate;
        //getMonthDetails(Convert.ToInt32(ddlSelectMonth.SelectedValue), out startDate, out endDate);

        //_Command.CommandText = "CALL `spComponentMaster`()";
        //_dtReader = _Command.ExecuteReader();
        //while (_dtReader.Read())
        //{
        //    objDictionaryComponentList.Add(Convert.ToInt32(_dtReader["COMPONENT_ID"]), Convert.ToString(_dtReader["COMPONENT_NAME"]));
        //} _dtReader.Close(); _dtReader.Dispose();

        //_Command.CommandText = "CALL `spComponentWiseCollectionDetailsFromPaidDateRangeAndSessionID`('" + startDate.ToString("yyyy-MM-dd") + "', '" + endDate.ToString("yyyy-MM-dd") + "', '" + Convert.ToString(ddlSelectSession.SelectedValue) + "')";
        //_dtReader = _Command.ExecuteReader();
        //while (_dtReader.Read())
        //{
        //    objDictionaryComponentAmountList.Add(Convert.ToInt32(_dtReader["COMPONENT_ID"]), Convert.ToDouble(_dtReader["AMOUNT_PAID"]));
        //} _dtReader.Close(); _dtReader.Dispose();



        HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 0; objHtmlTable.BorderColor = "#FFAB60"; objHtmlTable.Attributes.Add("style", "font-family:Tahoma");
        HtmlTableRow objHtmlTableRow = null; HtmlTableCell objHtmlTableCell = null;

        //objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:16px"); objHtmlTableCell.InnerText = Convert.ToString("GURU HARKRISHAN PUBLIC SCHOOL"); objHtmlTableCell.Align = "center"; objHtmlTableCell.ColSpan = 6; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        //objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:14px"); objHtmlTableCell.InnerText = Convert.ToString("HARGOBIND ENCLAVE, DELHI-92"); objHtmlTableCell.Align = "center"; objHtmlTableCell.ColSpan = 6; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        //objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:14px"); objHtmlTableCell.InnerText = Convert.ToString(" "); objHtmlTableCell.Align = "center"; objHtmlTableCell.ColSpan = 6; objHtmlTableCell.Height = "10px"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        //objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:14px"); objHtmlTableCell.InnerText = Convert.ToString("FEE RECONCILE STATEMENT FOR THE MONTH OF " + ddlSelectMonth.SelectedItem + " " + Convert.ToString(ddlSelectSession.SelectedItem).Substring(0, 4)); objHtmlTableCell.Align = "left"; objHtmlTableCell.ColSpan = 6; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        //objHtmlTableRow = new HtmlTableRow(); objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:14px"); objHtmlTableCell.InnerText = Convert.ToString(" "); objHtmlTableCell.Align = "center"; objHtmlTableCell.ColSpan = 6; objHtmlTableCell.Height = "10px"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);

        objHtmlTableRow = new HtmlTableRow();
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString("DATE"); objHtmlTableCell.Align = "left"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString("COLLECTION"); objHtmlTableCell.Align = "left"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString("CASH"); objHtmlTableCell.Align = "left"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString("CHEQUE"); objHtmlTableCell.Align = "left"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString("FORMS"); objHtmlTableCell.Align = "left"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px"); objHtmlTableCell.InnerText = Convert.ToString("AMOUNT"); objHtmlTableCell.Align = "left"; objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTable.Rows.Add(objHtmlTableRow);

        
        int counter = 1;
        double varMonthTotalCollection = 0.0; double varMonthTotalCashCollection = 0.0; double varMonthTotalChequeCollection = 0.0; double varMonthTotalComponentCollection = 0.0;
        StartDate = Convert.ToDateTime(txtStartDate.Text);
        EndDate = Convert.ToDateTime(txtEndDate.Text);
        while (StartDate <= EndDate)
        {
            //if (Convert.ToString(startDate.DayOfWeek) != "" && Convert.ToString(startDate.DayOfWeek) != "")
            {
                string ComponentName = "", SQL = ""; double varCashCollection, varChequeCollection, varTotalCollection = 0.0; double ComponentAmount = 0.0;
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(StartDate.ToString("dd-MMM-yyyy")); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);

                SQL = "CALL `spDayCollectionFromModePaidDateAndSessionID`('CASH', '" + StartDate.ToString("yyyy-MM-dd") + "', 1)";
                _Command.CommandText = SQL;
                varCashCollection = Convert.ToDouble(_Command.ExecuteScalar());
                SQL = "CALL `spDayCollectionFromModePaidDateAndSessionID`('CHEQUE', '" + StartDate.ToString("yyyy-MM-dd") + "', 1)";
                _Command.CommandText = SQL;
                varChequeCollection = Convert.ToDouble(_Command.ExecuteScalar());
                varTotalCollection = varCashCollection + varChequeCollection;

                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(varTotalCollection); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(varCashCollection); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(varChequeCollection); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);


                //if (counter <= objDictionaryComponentList.Count)
                //{
                //    ComponentName = Convert.ToString(objDictionaryComponentList[counter]);
                //    if (objDictionaryComponentAmountList.ContainsKey(counter))
                //    {
                //        ComponentAmount = Convert.ToDouble(objDictionaryComponentAmountList[counter]);
                //    }
                //    else { ComponentAmount = Convert.ToDouble(0); }
                //}
                //else if (counter == objDictionaryComponentList.Count + 1)
                //{
                //    ComponentName = "RE.ADM CHARGE";
                //    _Command.CommandText = "select ifnull(sum(RE_ADM_CHARGES),0) from collect_component_detail where paid_date between '" + startDate.ToString("yyyy-MM-dd") + "' and '" + endDate.ToString("yyyy-MM-dd") + "' and school_session_id='" + Convert.ToString(ddlSelectSession.SelectedValue) + "';";
                //    ComponentAmount = Convert.ToDouble(_Command.ExecuteScalar());
                //}
                //else if (counter == objDictionaryComponentList.Count + 2)
                //{
                //    ComponentName = "FINE";
                //    _Command.CommandText = "select ifnull(sum(FINE),0) from collect_component_detail where paid_date between '" + startDate.ToString("yyyy-MM-dd") + "' and '" + endDate.ToString("yyyy-MM-dd") + "' and school_session_id='" + Convert.ToString(ddlSelectSession.SelectedValue) + "';";
                //    ComponentAmount = Convert.ToDouble(_Command.ExecuteScalar());
                //}
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(ComponentName); objHtmlTableCell.Align = "left"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
                //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(ComponentAmount); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
                objHtmlTable.Rows.Add(objHtmlTableRow); counter += 1;

                varMonthTotalCollection += varTotalCollection;
                varMonthTotalCashCollection += varCashCollection;
                varMonthTotalChequeCollection += varChequeCollection;
                varMonthTotalComponentCollection += Convert.ToDouble(ComponentAmount);
            }
            StartDate = StartDate.AddDays(1);
        }
        objHtmlTableRow = new HtmlTableRow();
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString("TOTAL"); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(varMonthTotalCollection); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(varMonthTotalCashCollection); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(varMonthTotalChequeCollection); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString("TOTAL"); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:normal; font-size:12px; border:solid 1px black;"); objHtmlTableCell.InnerText = Convert.ToString(varMonthTotalComponentCollection); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        objHtmlTable.Rows.Add(objHtmlTableRow);

        objHtmlTableRow = new HtmlTableRow();
        objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px; height:50px;"); objHtmlTableCell.InnerText = Convert.ToString(" "); objHtmlTableCell.Align = "right"; objHtmlTableCell.ColSpan = 6; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        objHtmlTable.Rows.Add(objHtmlTableRow);

        //objHtmlTableRow = new HtmlTableRow();
        //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px;"); objHtmlTableCell.InnerText = Convert.ToString("PREPARED BY"); objHtmlTableCell.Align = "right"; objHtmlTableCell.ColSpan = 2; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px;"); objHtmlTableCell.InnerText = Convert.ToString("CHECKED BY"); objHtmlTableCell.Align = "center"; objHtmlTableCell.ColSpan = 3; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        //objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.Attributes.Add("style", "color:Black; font-weight:bold; font-size:12px;"); objHtmlTableCell.InnerText = Convert.ToString("O.S."); objHtmlTableCell.Align = "center"; objHtmlTableRow.Cells.Add(objHtmlTableCell); objHtmlTable.Rows.Add(objHtmlTableRow);
        //objHtmlTable.Rows.Add(objHtmlTableRow);
        return objHtmlTable;
    }
}