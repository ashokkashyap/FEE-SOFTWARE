using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;


public partial class WebForms_strength_Report : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataReader _dtReader = null; OdbcDataAdapter objDtAdapter = null;
    string varReportType = ""; DataTable dtblReportContent = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        if ((Session["_Connection"]) != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                btnReport.Visible = false; ViewState["varReportType"] = null; ViewState["dtblReportContent"] = null;
            }


        }
    }



    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlReportType.SelectedIndex > 0)
        {
            btnGenerateReport.Visible = true; 
        }
        else
        {
            btnGenerateReport.Visible = false; 
        }

        if (ddlReportType.SelectedItem.Text == "ClassWiseStrengthReport")
        {
            _Command = new OdbcCommand("select count(distinct STUDENT_ID) as totalStd from ign_student_master", _Connection);
            lblTotalStd.Text = "Total Student: " +Convert.ToString(_Command.ExecuteScalar());
        }

    }

    protected void btnGenerateReport_Click(object sender, ImageClickEventArgs e)
    {
        string ReportType = ddlReportType.SelectedValue.ToString();
        //Response.Write(ReportType);
        switch (ReportType.Trim())
        {
            case "ClassWiseStrengthReport": { ClassWiseStrengthReport(); break; };
            case "AdmNoWiseReport": { AdmNoWiseReport(); break; };
            //case "ClassGenderWiseStrengthReport": { ClassGenderWiseStrengthReport(); break; };
            //case "AgeWiseReport": { AgeWiseReport(); break; };
            //case "AgeGenderWiseStrengthReport": { AgeGenderWiseStrengthReport(); break; };
            default: { flushData(); break; };
        }


    }

    private void flushData()
    {
        gvRecords.DataSource = null; gvRecords.DataBind(); btnReport.Visible = false;
    }

    private void ClassWiseStrengthReport()
    {
        flushData(); btnReport.Visible = true; varReportType = "Class wise strength report as on : " + DateTime.Now.ToString("dd-MMM-yyyy");
        ViewState["varReportType"] = varReportType;
        dtblReportContent = new DataTable();
        dtblReportContent.Columns.Add("Class");
        dtblReportContent.Columns.Add("TotalStudents");
        //dtblReportContent.Rows.Add("Class", "TotalStudents");
        objDtAdapter = new OdbcDataAdapter("SELECT Concat(CLASS_NAME,' ',CLASS_SECTION) AS Class,(SELECT COUNT(*) FROM ign_student_master WHERE CLASS_CODE = A.CLASS_CODE) AS TotalStudents FROM ign_CLASS_MASTER A ORDER BY CLASS_PRIORITY,CLASS_SECTION", _Connection);
        objDtAdapter.Fill(dtblReportContent); ViewState["dtblReportContent"] = dtblReportContent;
        gvRecords.DataSource = dtblReportContent; gvRecords.DataBind();
    }
    private void AdmNoWiseReport()
    {
        flushData();
        btnReport.Visible = true; varReportType = "Admission # wise report as on : " + DateTime.Now.ToString("dd-MMM-yyyy");
        ViewState["varReportType"] = varReportType;
        dtblReportContent = new DataTable();
        dtblReportContent.Columns.Add("Sr_No");
        dtblReportContent.Columns.Add("ADMISSION NO");
        dtblReportContent.Columns.Add("NAME");
        dtblReportContent.Columns.Add("DATE OF BIRTH");
        dtblReportContent.Columns.Add("FATHER NAME");
        dtblReportContent.Columns.Add("CLASS");
        //dtblReportContent.Columns.Add("GENDER");
        //objDtAdapter = new OdbcDataAdapter("SELECT sm.STUDENT_REGISTRATION_NBR AS 'ADMISSION NO', sm.FIRST_NAME AS NAME, date_format(sm.BIRTH_DATE,'%e-%b-%Y') as 'DATE OF BIRTH', sm.FATHER_NAME AS 'FATHER NAME', concat(cm.CLASS_NAME,' ',cm.CLASS_SECTION) AS CLASS, sm.GENDER FROM ign_student_master sm, ign_class_master cm where cm.CLASS_CODE=sm.CLASS_CODE ORDER BY CAST(sm.STUDENT_REGISTRATION_NBR AS unsigned)", _Connection);
        objDtAdapter = new OdbcDataAdapter("SELECT @s:=@s+1 Sr_No, sm.STUDENT_REGISTRATION_NBR AS 'ADMISSION NO', sm.FIRST_NAME AS NAME, date_format(sm.BIRTH_DATE,'%e-%b-%Y') as 'DATE OF BIRTH', sm.FATHER_NAME AS 'FATHER NAME', concat(cm.CLASS_NAME,' ',cm.CLASS_SECTION) AS CLASS FROM (select @s:=0) as s, ign_student_master sm, ign_class_master cm where cm.CLASS_CODE=sm.CLASS_CODE ORDER BY CAST(sm.STUDENT_REGISTRATION_NBR AS unsigned)", _Connection);

        objDtAdapter.Fill(dtblReportContent); ViewState["dtblReportContent"] = dtblReportContent;
        gvRecords.DataSource = dtblReportContent; gvRecords.DataBind();
    }

    private void generateReport()
    {   
        var varSchoolName = ""; var varSchoolAddress = "";
        _Command.CommandText = "select SCHOOL_NAME, SCHOOL_ADDRESS from ign_school_master where SENDER_ID='Y';";
        _dtReader = _Command.ExecuteReader();
        while (_dtReader.Read())
        {
            varSchoolName = Convert.ToString(_dtReader["SCHOOL_NAME"]);
            varSchoolAddress = Convert.ToString(_dtReader["SCHOOL_ADDRESS"]);
        } _dtReader.Close();
        var FilePath = ddlReportType.SelectedItem.ToString().Trim().Replace(" ", "_") + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + "_" + DateTime.Now.Millisecond + "_" + ".pdf";
        var Filename = Server.MapPath("~/Reports/" + FilePath);

        using (Document objDocument = new Document(PageSize.A4, 2, 2, 2, 2))
        {
            PdfWriter objDocumentWriter = PdfWriter.GetInstance(objDocument, new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            objDocument.Open();

            PdfPTable objTable = new PdfPTable(1); objTable.WidthPercentage = 95f;
            PdfPCell objCell = null;

            objCell = new PdfPCell(new Phrase(varSchoolName, FontFactory.GetFont("Times-Bold", 18f))); objCell.HorizontalAlignment = Element.ALIGN_CENTER; objCell.BorderWidth = 0; objTable.AddCell(objCell);
            objCell = new PdfPCell(new Phrase(varSchoolAddress, FontFactory.GetFont("Times-Bold", 15f))); objCell.HorizontalAlignment = Element.ALIGN_CENTER; objCell.BorderWidth = 0; objTable.AddCell(objCell);
            objCell = new PdfPCell(new Phrase(Convert.ToString(ViewState["varReportType"]), FontFactory.GetFont("Times-Bold", 12f))); objCell.HorizontalAlignment = Element.ALIGN_CENTER; objCell.BorderWidth = 0; objTable.AddCell(objCell);

            dtblReportContent = (DataTable)ViewState["dtblReportContent"];
            PdfPTable objDataTable = new PdfPTable(dtblReportContent.Columns.Count + 1); objDataTable.WidthPercentage = 98f;

            objCell = new PdfPCell(new Phrase("SNo.", FontFactory.GetFont("Times-Bold", 10f))); objCell.HorizontalAlignment = Element.ALIGN_CENTER; objDataTable.AddCell(objCell);
            foreach (DataColumn dc in dtblReportContent.Columns)
            {
                objCell = new PdfPCell(new Phrase(dc.ColumnName.ToString(), FontFactory.GetFont("Times-Bold", 10f))); objCell.HorizontalAlignment = Element.ALIGN_CENTER; objDataTable.AddCell(objCell);
            }

            var Sno = 1;
            foreach (DataRow dr in dtblReportContent.Rows)
            {
                objCell = new PdfPCell(new Phrase(Convert.ToString(Sno), FontFactory.GetFont("Times", 8f))); objCell.HorizontalAlignment = Element.ALIGN_CENTER; objDataTable.AddCell(objCell);
                foreach (DataColumn dc in dtblReportContent.Columns)
                {
                    objCell = new PdfPCell(new Phrase(dr[dc].ToString(), FontFactory.GetFont("Times", 8f))); objCell.HorizontalAlignment = Element.ALIGN_LEFT; objDataTable.AddCell(objCell);
                }
                Sno++;
            }

            objDocument.Add(new Paragraph(" ", FontFactory.GetFont("Times-Bold", 10f)));
            objDocument.Add(objTable);
            objDocument.Add(new Paragraph(" ", FontFactory.GetFont("Times-Bold", 15f)));
            objDocument.Add(objDataTable);
        }
        Response.Redirect("~/Reports/" + FilePath);

    }
    protected void btnReport_Click(object sender, ImageClickEventArgs e)
    {
        //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "window.open('strength_Report.aspx?class_id=" + ddlReportType.SelectedValue + "');", true);

        generateReport();
    }
}