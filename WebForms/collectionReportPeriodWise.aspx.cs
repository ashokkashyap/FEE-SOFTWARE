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

public partial class WebForms_collectionReportPeriodWise : System.Web.UI.Page
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
            { }
        }
    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        btnDownloadExcel.Visible = true;
        pnlDetails.Controls.Add(FuncGetColectionPriodWise1());
    }
    protected void btnDownloadExcel_Click(object sender, EventArgs e)
    {
        HtmlTable _htmlTable = FuncGetColectionPriodWise1();
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        _htmlTable.RenderControl(hw);

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=CollectionReportAsOn" + DateTime.Now.ToString("dd-MM-yyyy").Trim().Replace(" ", "_") + ".xls");
        Response.ContentType = "application/vnd.ms-excel";
        this.EnableViewState = false;
        Response.Write(sw.ToString());
        Response.End();
    }
    private HtmlTable FuncGetColectionPriodWise1()
    {
        OdbcCommand objCommand = (OdbcCommand)_Command;
        OdbcDataReader objDtReader = null;
        Dictionary<string, string> objDictionaryComponentList = new Dictionary<string, string>();
        objCommand.CommandText = "select component_id,component_name from component_master where school_session_id in (select SCHOOL_SESSION_ID from ign_school_session_master where ACTIVE_STATUS = 'Y')";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            objDictionaryComponentList.Add(objDtReader["component_id"].ToString(), objDtReader["component_name"].ToString());
        }
        objDtReader.Close();

        Dictionary<string, string> objDictionaryClassList = new Dictionary<string, string>();
        objCommand.CommandText = "select * from ign_class_master";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            objDictionaryClassList.Add(objDtReader["class_code"].ToString(), objDtReader["Class_name"].ToString() + " " + Convert.ToString(objDtReader["class_section"]));
        }
        objDtReader.Close();
        //Response.End();
        objCommand.CommandText = "select student_id,first_name,middle_name,last_name,student_registration_nbr,class_code,father_name from ign_student_master where student_id in(select distinct student_id from collect_component_master)";
        OdbcDataAdapter objAdapter = new OdbcDataAdapter(); objAdapter.SelectCommand = objCommand;
        DataSet objDataSetStudentDetails = new DataSet();
        objAdapter.Fill(objDataSetStudentDetails);

        HtmlTable objHtmlTable = new HtmlTable();
        objHtmlTable.Border = 1; objHtmlTable.BorderColor = "#FFAB60"; //objHtmlTable.BgColor = "#FFFF99";
        objHtmlTable.Attributes.Add("style", "font-family:Arial");
        //objHtmlTable.Width = "800px";
        HtmlTableRow objHtmlTableRow = null;
        HtmlTableCell objHtmlTableCell = null;

        #region HeaderRow
        objHtmlTableRow = new HtmlTableRow();
        objHtmlTableCell = new HtmlTableCell();
        objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px");
        objHtmlTableCell.InnerText = "SNO";
        objHtmlTableCell.Align = "center";
        objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell();
        objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px");
        objHtmlTableCell.InnerText = "Name";
        objHtmlTableCell.Align = "center";
        objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell();
        objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px");
        objHtmlTableCell.InnerText = "Adm No";
        objHtmlTableCell.Align = "center";
        objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell();
        objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px");
        objHtmlTableCell.InnerText = "Class";
        objHtmlTableCell.Align = "center";
        objHtmlTableRow.Cells.Add(objHtmlTableCell);
        objHtmlTableCell = new HtmlTableCell();
        objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px");
        objHtmlTableCell.InnerText = "Father Name";
        objHtmlTableCell.Align = "center";
        objHtmlTableRow.Cells.Add(objHtmlTableCell);
        foreach (KeyValuePair<string, string> kvp in objDictionaryComponentList)
        {
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px");
            objHtmlTableCell.InnerText = kvp.Value;
            objHtmlTableCell.Align = "center";
            objHtmlTableRow.Cells.Add(objHtmlTableCell);
        }
        objHtmlTable.Rows.Add(objHtmlTableRow);
        #endregion
        int i = 1;
        foreach (DataRow objDataRow in objDataSetStudentDetails.Tables[0].Rows)
        {
            objHtmlTableRow = new HtmlTableRow();
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Attributes.Add("style", "font-weight:bold; font-size:12px");
            objHtmlTableCell.InnerText = i.ToString();
            objHtmlTableCell.Align = "center";
            objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Attributes.Add("style", " font-weight:bold; font-size:12px");
            objHtmlTableCell.InnerText = Convert.ToString(objDataRow["first_name"]) + " " + Convert.ToString(objDataRow["middle_name"]) + " " + Convert.ToString(objDataRow["last_name"]);
            objHtmlTableCell.Align = "center";
            objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Attributes.Add("style", "font-weight:bold; font-size:12px");
            objHtmlTableCell.InnerText = Convert.ToString(objDataRow["student_registration_nbr"]);
            objHtmlTableCell.Align = "center";
            objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Attributes.Add("style", "font-weight:bold; font-size:12px");
            objHtmlTableCell.InnerText = objDictionaryClassList[Convert.ToString(objDataRow["class_code"])];
            objHtmlTableCell.Align = "center";
            objHtmlTableRow.Cells.Add(objHtmlTableCell);
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Attributes.Add("style", "font-weight:bold; font-size:12px");
            objHtmlTableCell.InnerText = Convert.ToString(objDataRow["father_name"]);
            objHtmlTableCell.Align = "center";
            objHtmlTableRow.Cells.Add(objHtmlTableCell);
            foreach (KeyValuePair<string, string> kvp in objDictionaryComponentList)
            {
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Attributes.Add("style", "font-weight:bold; font-size:12px");
                objCommand.CommandText = "select sum(AMOUNT_PAID) as AMOUNT_PAID from collect_component_master where student_id = '" + Convert.ToString(objDataRow["student_id"]) + "' and COMPONENT_ID = '" + kvp.Key + "' and PAID_DATE >= '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and PAID_DATE <= '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
                objDtReader = objCommand.ExecuteReader();
                if (objDtReader.HasRows)
                {
                    objHtmlTableCell.InnerText = Convert.ToString(objDtReader["AMOUNT_PAID"]);
                }
                else
                {
                    objHtmlTableCell.InnerText = "-";
                }
                objDtReader.Close();

                objHtmlTableCell.Align = "center";
                objHtmlTableRow.Cells.Add(objHtmlTableCell);
            }
            objHtmlTable.Rows.Add(objHtmlTableRow);
            i++;
        }

        objHtmlTableRow = new HtmlTableRow();
        objHtmlTableCell = new HtmlTableCell();
        objHtmlTableCell.ColSpan = 5; objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px");
        objHtmlTableCell.InnerText = "TOTAL";
        objHtmlTableCell.Align = "center";
        objHtmlTableRow.Cells.Add(objHtmlTableCell);
        foreach (KeyValuePair<string, string> kvp in objDictionaryComponentList)
        {
            objHtmlTableCell = new HtmlTableCell();
            objHtmlTableCell.Attributes.Add("style", "color:green; font-weight:bold; font-size:12px");
            objCommand.CommandText = "select sum(AMOUNT_PAID) as AMOUNT_PAID from collect_component_masteR where COMPONENT_ID = '" + kvp.Key + "' and PAID_DATE >= '" + Convert.ToDateTime(txtStartDate.Text).ToString("yyyy-MM-dd") + "' and PAID_DATE <= '" + Convert.ToDateTime(txtEndDate.Text).ToString("yyyy-MM-dd") + "'";
            objDtReader = objCommand.ExecuteReader();
            if (objDtReader.HasRows)
            {
                objHtmlTableCell.InnerText = Convert.ToString(objDtReader["AMOUNT_PAID"]);
            }
            else
            {
                objHtmlTableCell.InnerText = "-";
            }
            objDtReader.Close();

            objHtmlTableCell.Align = "center";
            objHtmlTableRow.Cells.Add(objHtmlTableCell);
        }
        objHtmlTable.Rows.Add(objHtmlTableRow);

        return objHtmlTable;
    }
}