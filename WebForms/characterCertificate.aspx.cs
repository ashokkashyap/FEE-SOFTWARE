using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class WebForms_characterCertificate : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
    OdbcDataReader dr;
    string varstudent_name, varstudent_name2, varstudent_name3, varstudent_registration_nbr, varfathername, vardob, vargender, varmothername;int varTuitionFee;
    string vardistinction = "";
    string varAdmNo, varAddress, varClass;
    ArrayList var_arry = new ArrayList();
    Document objDocument;
    PdfWriter objWriter;
    static readonly string[] ones = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
    static readonly string[] teens = new string[] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
    static readonly string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
    static readonly string[] thousandsGroups = { "", " Thousand", " Million", " Billion" };
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                var _dtblComponents = new DataTable(); var _dtblClasses = new DataTable();
                var _dtAdapter = new OdbcDataAdapter();

                var SQL = "call spClassMaster()";
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblClasses);
                ViewState["_dtblClasses"] = _dtblClasses;
                ddlSelectClass.DataSource = _dtblClasses; ddlSelectClass.DataTextField = "CLS"; ddlSelectClass.DataValueField = "CLASS_CODE"; ddlSelectClass.DataBind(); ddlSelectClass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Class", ""));

            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }
    protected void ddlSelectClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSelectClass.SelectedIndex > 0)
        {
            var SQL = "CALL `spStudentDetailsFromClassCode`('" + ddlSelectClass.SelectedValue + "')";
            var _dtblStudentdetails = new DataTable();
            var _dtAdapter = new OdbcDataAdapter(SQL, _Connection);
            _dtAdapter.Fill(_dtblStudentdetails);
            ViewState["_dtblStudentdetails"] = _dtblStudentdetails;
            ddlSelectStudent.DataSource = _dtblStudentdetails; ddlSelectStudent.DataTextField = "SNAME"; ddlSelectStudent.DataValueField = "STUDENT_ID"; ddlSelectStudent.DataBind(); ddlSelectStudent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Student", ""));

        }
        else
        {
            ddlSelectStudent.Items.Clear();
        }

        string cmd = "select c.COMPONENT_ID,c.COMPONENT_NAME from component_master c where c.COMPONENT_ID between '9' and '13'";
        var _dtblComponents = new DataTable();
        var _da = new OdbcDataAdapter(cmd, _Connection);
        _da.Fill(_dtblComponents);
        ViewState["_dtblComponents"] = _dtblComponents;
        ddlComponent.DataSource = _dtblComponents; ddlComponent.DataTextField = "COMPONENT_NAME"; ddlComponent.DataValueField = "COMPONENT_ID"; ddlComponent.DataBind(); ddlComponent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", ""));

    }
    protected void ddlSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSelectStudent.SelectedIndex > 0)
        {
            btnSubmit.Visible = true;
        }
        else
        {
            btnSubmit.Visible = false;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string pdfDocumentPath = ddlSelectStudent.SelectedItem.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + ".pdf";
        using (Document _document = new Document(PageSize.A4, 15, 15, 50, 50))
        {
            PdfWriter _writer = PdfWriter.GetInstance(_document, new FileStream(Server.MapPath("~/Reports/" + pdfDocumentPath), FileMode.OpenOrCreate, FileAccess.ReadWrite));
            _document.Open();
            Font _times20B = FontFactory.GetFont("Times-Bold", 20f);
            _document.Add(new Paragraph("Test Document", _times20B));
        }
        ifCharacterCertificate.Attributes["src"] = @"../Reports/" + pdfDocumentPath;

        if (ddlSelectClass.SelectedIndex >0 && ddlSelectStudent.SelectedIndex >0)
        {
            _Command.CommandText = "Select * from ign_student_master where student_id = '" + ddlSelectStudent.SelectedValue + "'";
            dr = _Command.ExecuteReader();
            while (dr.Read())
            {
                var_arry.Add(Convert.ToString(dr["STUDENT_ID"]));
            } dr.Close();

            string ReportName = ddlSelectClass.SelectedItem + DateTime.Now.ToString("ddMMMyyyy") + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond + "test.pdf";
            using (objDocument = new Document(PageSize.A4, 20, 20, 00, 00))
            {
                PdfWriter objWriter = PdfWriter.GetInstance(objDocument, new FileStream(Server.MapPath("~/Reports/" + ReportName), FileMode.OpenOrCreate, FileAccess.ReadWrite));
                objDocument.Open();
                foreach (string s in var_arry)
                {
                    objDocument.Add(new Paragraph(" ", FontFactory.GetFont("Times-Bold", 1f)));
                    genratecertificate(s);
                    objDocument.Add(new Paragraph(" ", FontFactory.GetFont("Times-Bold", 1f)));
                    objDocument.NewPage();
                }
                objDocument.Close();
            }
            Response.Redirect("~/Reports/" + ReportName);
        }

    }
    public void genratecertificate(string sid)
    {
        _Connection = (OdbcConnection)Session["_Connection"];
        _Command = new OdbcCommand();
        _Command.Connection = _Connection;

        OdbcDataReader objDtReader;
        string varcertifcateid = "";
       
        _Command.CommandText = "Select * from ign_student_master where student_id= '" + sid + "'";
        objDtReader = _Command.ExecuteReader();

        while (objDtReader.Read())
        {

            varstudent_name = Convert.ToString(objDtReader["FIRST_NAME"]);
            varstudent_name2 = Convert.ToString(objDtReader["middle_NAME"]);
            varstudent_name3 = Convert.ToString(objDtReader["LAST_NAME"]);
            varfathername = Convert.ToString(objDtReader["FATHER_NAME"]);
            varmothername = Convert.ToString(objDtReader["MOTHER_NAME"]);
            vargender = Convert.ToString(objDtReader["GENDER"]);
            varstudent_registration_nbr = Convert.ToString(objDtReader["STUDENT_REGISTRATION_NBR"]);
            if (!Convert.ToString(objDtReader["BIRTH_DATE"]).Equals(""))
            {
                vardob = Convert.ToDateTime(objDtReader["BIRTH_DATE"]).ToString("dd-MMM-yyyy");
            }
            varAddress = Convert.ToString(objDtReader["ADDRESS_LINE1"]);

        } objDtReader.Close(); objDtReader.Dispose();

        _Command.CommandText = "Select concat(ifnull(class_name,' '),'-',ifnull(class_section,' ')) as class from ign_student_master s,ign_class_master c  where s.student_id= '" + sid + "' and s.class_code=c.class_code";
        objDtReader = _Command.ExecuteReader();

        while (objDtReader.Read())
        {

            varClass = Convert.ToString(objDtReader["class"]);


        } objDtReader.Close(); objDtReader.Dispose();

        if (ddlComponent.SelectedIndex > 0)
        {
            _Command.CommandText = "select sum(c.AMOUNT_PAYBLE) from collect_component_master c where c.STUDENT_ID='"+Convert.ToString(ddlSelectStudent.SelectedValue)+"' and c.COMPONENT_ID='"+Convert.ToString(ddlComponent.SelectedValue)+"'";
            if (!Convert.ToString(_Command.ExecuteScalar()).Equals(""))
            {
                varTuitionFee = Convert.ToInt32(_Command.ExecuteScalar());
            }
            else
            {
                varTuitionFee = 0;
            }
        }
        Response.Write(varstudent_name);
       
        {
            //    PdfWriter objWriter = PdfWriter.GetInstance(objDocument, new FileStream(_FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite));
            //    objDocument.Open();
            FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
            iTextSharp.text.Font Algerian30B = FontFactory.GetFont("Algerian", BaseFont.CP1252, BaseFont.EMBEDDED, 24f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            //  Font Algerian30B = FontFactory.GetFont("Algerian", BaseFont.CP1252, BaseFont.CACHED, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            Font times20BU = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 20f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            Font times25B = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.CACHED, 25f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font times30B = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.CACHED, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font times16B = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font times12B = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font times12N = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font times14N = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 14f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font times20B = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 20f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial130B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial120B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 20f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial15B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 15f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial15N = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 15f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font Arial10B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial10N = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font Arial14N = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 14f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font Arial12B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial12N = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 12f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font Arial3B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 3f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial12U = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 12f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            Font Arial12UB = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 12f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            Font Arial8N = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 8f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

            Font Ftimes20 = FontFactory.GetFont("Times-Bold", 20f);
            Font Ftimes16 = FontFactory.GetFont("Times-Bold", 16f);
            Font Ftimes14 = FontFactory.GetFont("Times-Bold", 14f);
            Font Ftimes12 = FontFactory.GetFont("Times-Bold", 12f);
            Font Ftimes12N = FontFactory.GetFont("Times", 12f);
            Font Ftimes10 = FontFactory.GetFont("Times-Bold", 10f);
            Font Ftimes10N = FontFactory.GetFont("Times", 10f);
            Font Ftimes8N = FontFactory.GetFont("Times", 8f);

            PdfPTable outerTable6 = new PdfPTable(1);
            outerTable6.WidthPercentage = 95f;
            PdfPTable innerTable6 = new PdfPTable(1);
            innerTable6.SetWidths(new float[1] { 10f });


            PdfPCell cell1 = null;

            cell1 = new PdfPCell(new Phrase("", Arial12B));
            cell1.MinimumHeight = 40f;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.BorderWidthBottom = 0;
            cell1.BorderWidthLeft = 0;
            cell1.BorderWidthRight = 0;
            cell1.BorderWidthTop = 0;
            innerTable6.AddCell(cell1);


            cell1 = new PdfPCell(new Phrase("HIMALAYA PUBLIC SR. SEC. SCHOOL", Arial12N));
            cell1.MinimumHeight = 30f;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.BorderWidthBottom = 0;
            cell1.BorderWidthLeft = 0;
            cell1.BorderWidthRight = 0;
            cell1.BorderWidthTop = 0;
            innerTable6.AddCell(cell1);

            cell1 = new PdfPCell(new Phrase(DateTime.Now.ToString("MMMM dd,yyyy"), Arial12N));
            cell1.MinimumHeight = 20f;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell1.BorderWidthBottom = 0;
            cell1.BorderWidthLeft = 0;
            cell1.BorderWidthRight = 0;
            cell1.BorderWidthTop = 0;
            innerTable6.AddCell(cell1);

            cell1 = new PdfPCell(new Phrase("", Arial12B));
            cell1.MinimumHeight = 20f;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.BorderWidthBottom = 0;
            cell1.BorderWidthLeft = 0;
            cell1.BorderWidthRight = 0;
            cell1.BorderWidthTop = 0;
            innerTable6.AddCell(cell1);

            cell1 = new PdfPCell();
            cell1.BorderWidth = 0;
            cell1.AddElement(innerTable6);
            outerTable6.AddCell(cell1);
            objDocument.Add(outerTable6);


            PdfPTable outerTable8 = new PdfPTable(1);
            outerTable8.WidthPercentage = 40f;
            PdfPTable innerTable8 = new PdfPTable(1);
            innerTable8.SetWidths(new float[1] { 10f });

            cell1 = new PdfPCell(new Phrase("\nTO WHOM IT MAY CONCERN", Arial12N));
            cell1.MinimumHeight = 20f;
            cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.BorderWidthBottom = 1;
            cell1.BorderWidthLeft = 0;
            cell1.BorderWidthRight = 0;
            cell1.BorderWidthTop = 0;

            innerTable8.AddCell(cell1);

            cell1 = new PdfPCell();
            cell1.BorderWidth = 0;
            cell1.AddElement(innerTable8);
            outerTable8.AddCell(cell1);
            objDocument.Add(outerTable8);

            PdfPTable outerTable7 = new PdfPTable(1);
            outerTable7.WidthPercentage = 95f;
            PdfPTable innerTable7 = new PdfPTable(1);
            innerTable7.SetWidths(new float[1] { 10f });


            PdfPCell cell = null;
            cell = new PdfPCell(new Phrase("", Arial12B));
            cell.MinimumHeight = 10f;
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.BorderWidthBottom = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;

            innerTable7.AddCell(cell);

            cell = new PdfPCell(new Phrase("This is to certify that Master / Miss " + varstudent_name + " " + varstudent_name2 + " " + varstudent_name3 + " S / D / o SH. " + varfathername + " Mother Name SMT. " + varmothername + " R/O " + varAddress + " is / was a bonafide student of " + Convert.ToString(ddlSelectClass.SelectedItem.Text) + " vide Admission NO. " + varstudent_registration_nbr + " of this School has/had paid a sum of Rs. " + varTuitionFee + " ( " + FeeToWritten(varTuitionFee) + " ) upto MAR - 2015 as TUITION FEE for the academic session 2014-2015.", Arial10N));
            cell.MinimumHeight = 20f;
            cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.BorderWidthBottom = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;
            innerTable7.AddCell(cell);

            cell = new PdfPCell(new Phrase("", times12N));
            cell.MinimumHeight = 20f;
            cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.BorderWidthBottom = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;
            innerTable7.AddCell(cell);

            cell = new PdfPCell(new Phrase("Further certified that our school is recognized by Directorate of Education ( Government of National Capital Territory of Delhi ) vide letter No. dated / / and affiliated to C.B.S.E. . Our Affiliation number is", Arial10N));
            cell.MinimumHeight = 40f;
            cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.BorderWidthBottom = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;

            innerTable7.AddCell(cell);

            cell = new PdfPCell(new Phrase("For HIMALAYA PUBLIC SR. SEC. SCHOOL", Arial10N));
            cell.MinimumHeight = 40f;
            cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderWidthBottom = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;

            innerTable7.AddCell(cell);

            cell = new PdfPCell(new Phrase("PRINCIPAL", Arial10N));
            cell.MinimumHeight = 20f;
            cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.BorderWidthBottom = 0;
            cell.BorderWidthLeft = 0;
            cell.BorderWidthRight = 0;
            cell.BorderWidthTop = 0;



            innerTable7.AddCell(cell);

            //cell = new PdfPCell(new Phrase("\n, times12N));
            //cell.MinimumHeight = 20f;
            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
            //cell.BorderWidthBottom = 0;
            //cell.BorderWidthLeft = 0;
            //cell.BorderWidthRight = 0;
            //cell.BorderWidthTop = 0;

            //innerTable7.AddCell(cell);


            //cell = new PdfPCell(new Phrase("\n", times12N));
            //cell.MinimumHeight = 20f;
            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
            //cell.BorderWidthBottom = 0;
            //cell.BorderWidthLeft = 0;
            //cell.BorderWidthRight = 0;
            //cell.BorderWidthTop = 0;

            //innerTable7.AddCell(cell);

            //cell = new PdfPCell(new Phrase("\nHis Date of Birth is " + vardob + "(" + DateToWritten(Convert.ToDateTime(vardob)) + ")", times12N));
            //cell.MinimumHeight = 20f;
            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
            //cell.BorderWidthBottom = 0;
            //cell.BorderWidthLeft = 0;
            //cell.BorderWidthRight = 0;
            //cell.BorderWidthTop = 0;

            //innerTable7.AddCell(cell);

            //cell = new PdfPCell(new Phrase("\naccording to school records. Bears good Moral Character.", times12N));
            //cell.MinimumHeight = 20f;
            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderWidthBottom = 0;
            //cell.BorderWidthLeft = 0;
            //cell.BorderWidthRight = 0;
            //cell.BorderWidthTop = 0;

            //innerTable7.AddCell(cell);

            //cell = new PdfPCell(new Phrase("", Arial12N));
            //cell.MinimumHeight = 20f;
            //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
            //cell.BorderWidthBottom = 0;
            //cell.BorderWidthLeft = 0;
            //cell.BorderWidthRight = 0;
            //cell.BorderWidthTop = 0;

            //innerTable7.AddCell(cell);

            //cell = new PdfPCell(new Phrase("\n\nPrincipal", times14N));
            //cell.MinimumHeight = 20f;
            //cell.VerticalAlignment = Element.ALIGN_BOTTOM;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderWidthBottom = 0;
            //cell.BorderWidthLeft = 0;
            //cell.BorderWidthRight = 0;
            //cell.BorderWidthTop = 0;

            //innerTable7.AddCell(cell);

            innerTable7.WidthPercentage = 98f;
            cell = new PdfPCell();
            cell.BorderWidth = 0;
            cell.AddElement(innerTable7);
            outerTable7.AddCell(cell);


            var Para = new Paragraph("\n\n", times16B);
            Para.Alignment = Element.ALIGN_CENTER;
            objDocument.Add(Para);


            objDocument.Add(outerTable7);

        }
    }

 
    public static string FeeToWritten(int fees)
    {
        return string.Format("{0}", IntegerToWritten(fees));
    }

    public static string IntegerToWritten(int n)
    {
        if (n == 0)
            return "Zero";
        else if (n < 0)
            return "Negative " + IntegerToWritten(-n);

        return FriendlyInteger(n, "", 0);
    }
    private static string FriendlyInteger(int n, string leftDigits, int thousands)
    {
        if (n == 0)
            return leftDigits;

        string friendlyInt = leftDigits;
        if (friendlyInt.Length > 0)
            friendlyInt += " ";

        if (n < 10)
            friendlyInt += ones[n];
        else if (n < 20)
            friendlyInt += teens[n - 10];
        else if (n < 100)
            friendlyInt += FriendlyInteger(n % 10, tens[n / 10 - 2], 0);
        else if (n < 1000)
            friendlyInt += FriendlyInteger(n % 100, (ones[n / 100] + " Hundred"), 0);
        else
            friendlyInt += FriendlyInteger(n % 1000, FriendlyInteger(n / 1000, "", thousands + 1), 0);

        return friendlyInt + thousandsGroups[thousands];
    }
  
}