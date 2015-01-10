using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Odbc;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Configuration;  

public partial class WebForms_fill_bill_class_print_pdf : System.Web.UI.Page
{
    OdbcConnection objConnection; OdbcCommand objCommand; OdbcDataReader objDtReader, objDtReader1;
    string varSchoolSession;

    ArrayList var_arry = new ArrayList();
    Document objDocument;
    PdfWriter objWriter;
    static readonly string[] ones = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
    static readonly string[] teens = new string[] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
    static readonly string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
    static readonly string[] thousandsGroups = { "", " Thousand", " Million", " Billion" };

    protected void Page_Load(object sender, EventArgs e)
    {
        varSchoolSession = Convert.ToString(Session["_Session"]);
        objConnection = (OdbcConnection)Session["_Connection"];
        objCommand = new OdbcCommand();
        objCommand.Connection = objConnection;
        string varStatus = Session["varstat"].ToString();
        string varClassId = Request.QueryString["class_id"];
        string varSId = Convert.ToString(Request.QueryString["student_id"]);
        // if (varStatus.ToUpper().Equals("Y"))

        {
            if (varSId.Equals("-1"))
            {

                classdetail(varClassId);
            }
            else
            {
                string ReportName = DateTime.Now.ToString("ddMMMyyyy") + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond + "test.pdf";
                using (objDocument = new Document(PageSize.A4, 20, 20, 20, 20))
                {
                    PdfWriter objWriter = PdfWriter.GetInstance(objDocument, new FileStream(Server.MapPath("~/Reports/" + ReportName), FileMode.OpenOrCreate, FileAccess.ReadWrite));
                    objDocument.Open();
                    //foreach (string s in var_arry)
                    //{
                    objDocument.Add(new Paragraph(" ", FontFactory.GetFont("Times-Bold", 1f)));
                    feeBillNameall(varSId);
                    objDocument.Add(new Paragraph(" ", FontFactory.GetFont("Times-Bold", 1f)));
                    objDocument.NewPage();
                    //}
                    objDocument.Close();
                }
                Response.Redirect("~/Reports/" + ReportName);
               

            }

        }
    }
     

    public void classdetail(string cid)
    {

        objCommand.CommandText = "select student_id from ign_student_master where class_code = '" + cid + "'  and student_id not in (select student_id from ign_tc_student_master a where a.CLASS_CODE='" + cid + "'";
      
        objDtReader1 = objCommand.ExecuteReader();
        while (objDtReader1.Read())
        {
            var_arry.Add(Convert.ToString(objDtReader1["STUDENT_ID"]));
          
            
        }
        objDtReader1.Close();
        string ReportName =  DateTime.Now.ToString("ddMMMyyyy") + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond + "test.pdf";
        using (objDocument = new Document(PageSize.A4, 20, 20, 20, 20))
        {
            PdfWriter objWriter = PdfWriter.GetInstance(objDocument, new FileStream(Server.MapPath("~/Reports/" + ReportName), FileMode.OpenOrCreate, FileAccess.ReadWrite));
            objDocument.Open();
            foreach (string s in var_arry)
            {
                objDocument.Add(new Paragraph(" ", FontFactory.GetFont("Times-Bold", 1f)));
                feeBillNameall(s);
                objDocument.Add(new Paragraph(" ", FontFactory.GetFont("Times-Bold", 1f)));
                objDocument.NewPage();
            }
            objDocument.Close();
        }
        Response.Redirect("~/Reports/" + ReportName);

    }

    public class Converter
    {
        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "And ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }

    public void feeBillNameall(string sid)
    {
        DateTime varMappedDate = new DateTime();
        ArrayList objArrayListStudentId = new ArrayList();
        string varClassCode = Convert.ToString(Request.QueryString["class_id"]);
        int varSId = Convert.ToInt32(Request.QueryString["student_id"]);
        varSchoolSession = Convert.ToString(Session["_Session"]);
        objConnection = (OdbcConnection)Session["_Connection"];
        objCommand = new OdbcCommand();
        objCommand.Connection = objConnection;

        if (!IsPostBack)
        {
            #region PDF_TABLES
            FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
            iTextSharp.text.Font Algerian30B = FontFactory.GetFont("Algerian", BaseFont.CP1252, BaseFont.EMBEDDED, 24f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            //  Font Algerian30B = FontFactory.GetFont("Algerian", BaseFont.CP1252, BaseFont.CACHED, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            Font times20BU = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 20f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            Font times25B = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.CACHED, 25f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font times30B = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.CACHED, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font times16B = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

            Font times20B = FontFactory.GetFont("Times-Bold", BaseFont.CP1252, BaseFont.EMBEDDED, 20f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial130B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial120B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 20f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial15B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 15f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial15N = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 15f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font Arial10B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial10N = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font Arial14N = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 14f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font Arial12B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 12f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial3B = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 3f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
            Font Arial12U = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 12f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            Font Arial12UB = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 12f, iTextSharp.text.Font.UNDERLINE, iTextSharp.text.BaseColor.BLACK);
            Font Arial8N = FontFactory.GetFont("Arial", BaseFont.CP1252, BaseFont.EMBEDDED, 10f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font Vladimir48N = FontFactory.GetFont("Vladimir Script", BaseFont.CP1252, BaseFont.EMBEDDED, 34f, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
            Font Ftimes20 = FontFactory.GetFont("Times-Bold", 20f);
            Font Ftimes16 = FontFactory.GetFont("Times-Bold", 16f);
            Font Ftimes14 = FontFactory.GetFont("Times-Bold", 14f);
            Font Ftimes12 = FontFactory.GetFont("Times-Bold", 12f);
            Font Ftimes12N = FontFactory.GetFont("Times", 12f);
            Font Ftimes10 = FontFactory.GetFont("Times-Bold", 10f);
            Font Ftimes10N = FontFactory.GetFont("Times", 10f);
            Font Ftimes8N = FontFactory.GetFont("Times", 8f);



            PdfPTable outerTable = new PdfPTable(1);
            outerTable.SetWidths(new float[1] { 30f});
            outerTable.WidthPercentage = 100f;

            PdfPTable outerTable1 = new PdfPTable(1);
            outerTable1.SetWidths(new float[1] { 30f });
            outerTable1.WidthPercentage = 100f;

            PdfPCell cell = null;



            PdfPTable innerTable1 = new PdfPTable(1);
            innerTable1.SetWidths(new float[1] { 10f });
            innerTable1.WidthPercentage = 100f;

            PdfPTable innerTable2 = new PdfPTable(1);
            innerTable2.SetWidths(new float[1] { 10f });
            innerTable2.WidthPercentage = 100f;

            

            #endregion
 
            {
                objCommand.CommandText = "select student_id from ign_student_master where student_id = '" + sid + "'";
                objDtReader = objCommand.ExecuteReader();
            }
            
            
            while (objDtReader.Read())
            {
                objArrayListStudentId.Add(Convert.ToString(objDtReader["student_id"]));

            }
            objDtReader.Close();
            int i = 0;
            foreach (string varStudentId in objArrayListStudentId)
            {
                string datemonth = "", datemonth1 = "";
                datemonth = Convert.ToDateTime(Session["datemoth"]).ToString("yyyy-MM-01");
                datemonth1 = Convert.ToDateTime(Session["datemoth"]).AddMonths(0).ToString("yyyy-MM-01");

               

                #region Student Details
                
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,a.STUDENT_ROLL_NBR,a.DATE_OF_ADMISSION,a.GENDER,a.ADDRESS_LINE1,a.CITY,a.STATE,a.FATHER_NAME,a.FATHER_OCCUPATION,a.MOTHER_NAME,a.MOTHER_OCCUPATION,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + sid + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
 

                    cell = new PdfPCell(new Phrase("HAPPY HOME PUBLIC SCHOOL ", Ftimes14));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("B-5 SECTOR 11 ROHINI DELHI 110085", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);

                    

                    cell = new PdfPCell(new Phrase("School Copy", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);


                    PdfPTable innerTable3 = new PdfPTable(2);
                    innerTable3.SetWidths(new float[2] { 10f, 10f });
                    innerTable3.WidthPercentage = 100f;



                    cell = new PdfPCell(new Phrase("Bill No : HHPS-", Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("DATE : " + DateTime.Now.ToString("dd/MM/yyyy") +" ", Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Name : " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["middle_name"]) + " " + Convert.ToString(objDtReader["last_name"]), Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 1;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthBottom = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Adm No : " + Convert.ToString(objDtReader["student_registration_nbr"]) + "", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthRight = 1;
                    cell.BorderWidthLeft = 0;
                    innerTable3.AddCell(cell);
                   
                    cell = new PdfPCell(new Phrase("Father's Name : " + Convert.ToString(objDtReader["father_name"]) + "", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 1;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Class : " + Convert.ToString(objDtReader["class_name"]) + "-" + Convert.ToString(objDtReader["class_section"])+ " ", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthRight = 1;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Concession : ", Arial8N));
                    cell.MinimumHeight = 12f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 1;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Roll No : "+Convert.ToString(objDtReader["STUDENT_ROLL_NBR"]) , Arial8N));
                    cell.MinimumHeight = 12f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthRight = 1;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell();
                    cell.BorderWidth = 0;
                    cell.AddElement(innerTable3);
                    innerTable1.AddCell(cell);


                    cell = new PdfPCell(new Phrase("", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;

                    innerTable1.AddCell(cell);



                    cell = new PdfPCell(new Phrase("", Ftimes12));
                    cell.MinimumHeight = 50f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    cell = new PdfPCell(new Phrase("HAPPY HOME PUBLIC SCHOOL", Ftimes14));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    cell=new PdfPCell(new Phrase("B-5 SECTOR 11 ROHINI DELHI 110085",Arial8N));
                    cell.MinimumHeight=10f;
                    cell.VerticalAlignment=Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment=Element.ALIGN_CENTER;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Student Copy ", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    PdfPTable innerTable4 = new PdfPTable(2);
                    innerTable4.SetWidths(new float[2] { 10f, 10f });
                    innerTable4.WidthPercentage = 100f;

                    
                    cell = new PdfPCell(new Phrase("Bill No : HHPS-", Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable4.AddCell(cell);

                    cell = new PdfPCell(new Phrase("DATE : " + DateTime.Now.ToString("dd/MM/yyyy"), Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = 0;
                    innerTable4.AddCell(cell);

                    
                    cell = new PdfPCell(new Phrase("Name : " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["middle_name"]) + " " + Convert.ToString(objDtReader["last_name"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 1;
                    innerTable4.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Adm. No : " + Convert.ToString(objDtReader["student_registration_nbr"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthRight = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 0;
                    innerTable4.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Father's Name : " + Convert.ToString(objDtReader["father_name"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 1;
                    innerTable4.AddCell(cell);


                    cell = new PdfPCell(new Phrase("Class : " + Convert.ToString(objDtReader["class_name"]) + "-" + Convert.ToString(objDtReader["class_section"]) + " ", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthRight = 1;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    innerTable4.AddCell(cell);


                    cell = new PdfPCell(new Phrase("Concession : ", Arial8N));
                    cell.MinimumHeight = 12f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 1;
                    innerTable4.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Roll No : " + Convert.ToString(objDtReader["STUDENT_ROLL_NBR"]), Arial8N));
                    cell.MinimumHeight = 12f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthRight = 1;
                    cell.BorderWidthTop = 0;
                    cell.BorderWidthLeft = 0;
                    innerTable4.AddCell(cell);

                    
                    cell = new PdfPCell();
                    cell.BorderWidth = 0;
                    cell.AddElement(innerTable4);
                    innerTable2.AddCell(cell);


                    cell = new PdfPCell(new Phrase("", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                }

                objDtReader.Close();

                #endregion

                #region Fee Details

                //------------------

                PdfPTable innerTableHead = new PdfPTable(5);
                innerTableHead.SetWidths(new float[5] { 1f, 4f, 3f, 3f, 3f });
                innerTableHead.WidthPercentage = 100f;


                cell = new PdfPCell(new Phrase("S.No", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead.AddCell(cell);

                
                cell = new PdfPCell(new Phrase("FEE PARTICULARS", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead.AddCell(cell);

                cell = new PdfPCell(new Phrase("DURATION", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead.AddCell(cell);

                cell = new PdfPCell(new Phrase("PAYABLE", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead.AddCell(cell);

                cell = new PdfPCell(new Phrase("PAID", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead.AddCell(cell);

                cell = new PdfPCell();
                cell.BorderWidth = 0;
                cell.AddElement(innerTableHead);
                innerTable1.AddCell(cell);



                    
                //------------------------------


                PdfPTable innerTableHead2 = new PdfPTable(5);
                innerTableHead2.SetWidths(new float[5] { 1f, 4f, 3f, 3f, 3f });
                innerTableHead2.WidthPercentage = 100f;

                cell = new PdfPCell(new Phrase("S.No", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead2.AddCell(cell);


                cell = new PdfPCell(new Phrase("FEE PARTICULARS", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead2.AddCell(cell);

                cell = new PdfPCell(new Phrase("DURATION", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead2.AddCell(cell);

                cell = new PdfPCell(new Phrase("PAYABLE", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead2.AddCell(cell);

                cell = new PdfPCell(new Phrase("PAID", Arial10B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTableHead2.AddCell(cell);

                cell = new PdfPCell();
                cell.BorderWidth = 0;
                cell.AddElement(innerTableHead2);
                innerTable2.AddCell(cell);

//------------------------------------------------

               
                int varTotal = 0;
                int varPaidTotal = 0;

                objCommand.CommandText = "select sum(a.AMOUNT_PAYBLE)-sum(a.AMOUNT_PAID)-sum(a.DISCOUNT) as dues from collect_component_master a  where a.STUDENT_ID='"+varStudentId+"' and a.MAPPED_DATE between '2014-04-01' and '" + datemonth + "'";
                var previous_due = Convert.ToString(objCommand.ExecuteScalar());
                 
                objCommand.CommandText = "select @s:=@s+1 S_No, B.COMPONENT_NAME ,date_format(A.PAID_DATE,'%M') as Duration,sum(A.AMOUNT_PAYBLE-A.DISCOUNT) as AMOUNT_PAYBLE,A.AMOUNT_PAID,MAPPED_DATE from (select @s:=0) as s, collect_component_master A, component_master B where student_id = '" + varStudentId + "' and MAPPED_DATE between '" + datemonth + "' and '" + datemonth1 + "'   and A.COMPONENT_ID = B.COMPONENT_ID group by B.COMPONENT_ID ";
                

                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    PdfPTable innerTable4 = new PdfPTable(5);
                    innerTable4.SetWidths(new float[5] { 1f,4f,3f,3f,3f });
                    innerTable4.WidthPercentage = 100f;

                    cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["S_No"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 1;
                    cell.BorderWidthRight = 0;
                    innerTable4.AddCell(cell);


                    if (Convert.ToString(objDtReader["COMPONENT_NAME"]).Contains("TUTION"))
                    {
                        cell = new PdfPCell(new Phrase("TUITION FEE", Arial8N));
                    
                        
                    }
                    else if (Convert.ToString(objDtReader["COMPONENT_NAME"]).Contains("DEVELOPMENT"))
                    {
                        cell = new PdfPCell(new Phrase("DEVELOPMENT FEE", Arial8N));

                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["COMPONENT_NAME"]), Arial8N));

                    }

                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTable4.AddCell(cell);


                cell = new PdfPCell(new Phrase(Convert.ToString(Session["sdate"]) + " - " + Convert.ToString(Session["sdate"]), Arial8N));
                //cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["Duration"]), Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                innerTable4.AddCell(cell);

                cell = new PdfPCell(new Phrase( Convert.ToString(objDtReader["AMOUNT_PAYBLE"]), Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 0;
                innerTable4.AddCell(cell);


                cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["AMOUNT_PAID"]), Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                innerTable4.AddCell(cell);

                cell = new PdfPCell();
                cell.BorderWidth = 0;
                cell.AddElement(innerTable4);
                innerTable1.AddCell(cell);


    //------------------------------------------------------------------------



                    PdfPTable innerTable5 = new PdfPTable(5);
                    innerTable5.SetWidths(new float[5] { 1f,4f, 3f, 3f,3f });
                    innerTable5.WidthPercentage = 100f;

                    cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["S_No"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 1;
                    cell.BorderWidthRight = 0;
                    innerTable5.AddCell(cell);

                    if (Convert.ToString(objDtReader["COMPONENT_NAME"]).Contains("TUTION"))
                    {
                        cell = new PdfPCell(new Phrase("TUITION FEE", Arial8N));


                    }
                    else if (Convert.ToString(objDtReader["COMPONENT_NAME"]).Contains("DEVELOPMENT"))
                    {
                        cell = new PdfPCell(new Phrase("DEVELOPMENT FEE", Arial8N));

                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["COMPONENT_NAME"]), Arial8N));

                    }
                  
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 1;
                    cell.BorderWidthRight = 1;
                    innerTable5.AddCell(cell);


                    cell = new PdfPCell(new Phrase(Convert.ToString(Session["sdate"]) + " - " + Convert.ToString(Session["sdate"]), Arial8N));
                    //cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["Duration"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0;
                    innerTable5.AddCell(cell);

                    cell = new PdfPCell(new Phrase( Convert.ToString(objDtReader["AMOUNT_PAYBLE"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 1;
                    cell.BorderWidthRight = 0;
                    innerTable5.AddCell(cell);


                    cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["AMOUNT_PAID"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 1;
                    cell.BorderWidthRight = 1;
                    innerTable5.AddCell(cell);


                    

                    cell = new PdfPCell();
                    cell.BorderWidth = 0;
                    cell.AddElement(innerTable5);
                    innerTable2.AddCell(cell);

                    varTotal = varTotal + Convert.ToInt32(objDtReader["AMOUNT_PAYBLE"]);
                    varPaidTotal = varPaidTotal + Convert.ToInt32(objDtReader["AMOUNT_PAID"]);


                } 
                
                objDtReader.Close();
                //-------------------------------

                PdfPTable footerTable = new PdfPTable(3);
                footerTable.SetWidths(new float[3] { 6.5f, 2.5f, 2.5f });
                footerTable.WidthPercentage = 100f;


                int inputTotalPayble = Convert.ToUInt16(varTotal);

                //cell = new PdfPCell(new Phrase(Converter.NumberToWords(inputTotalPayble), Arial12B));
                cell = new PdfPCell(new Phrase("IN WORDS : " +Converter.NumberToWords(inputTotalPayble) +" "+ "Only", Arial12B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable.AddCell(cell);


                cell = new PdfPCell(new Phrase("Amt.Payable", Arial8N));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                footerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(varTotal.ToString(), Arial12B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("Pay Mode :", Arial8N));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable.AddCell(cell);


                cell = new PdfPCell(new Phrase("Amount Paid :", Arial8N));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                footerTable.AddCell(cell);
                

                cell = new PdfPCell(new Phrase(varPaidTotal.ToString(), Arial12B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase("NOTE :1. Fee,Charges,Fund,Once paid are not refundable.", Arial8N));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable.AddCell(cell);

                if (Convert.ToInt32(previous_due) >= 0)
                {
                    cell = new PdfPCell(new Phrase("Previous Due :", Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1;
                    footerTable.AddCell(cell);
                }

                else
                {
                    cell = new PdfPCell(new Phrase("Previous Excess :", Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1;
                    footerTable.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase(previous_due, Arial12B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1;
                footerTable.AddCell(cell);




                cell = new PdfPCell();
                cell.BorderWidth = 0;
                cell.AddElement(footerTable);
                innerTable1.AddCell(cell);


                PdfPTable footerTable1 = new PdfPTable(3);
                footerTable1.SetWidths(new float[3] { 6.5f, 2.5f, 2.5f });
                footerTable1.WidthPercentage = 100f;

                

                //cell = new PdfPCell(new Phrase("IN WORDS :", Arial8N));
                cell = new PdfPCell(new Phrase("IN WORDS : " + Converter.NumberToWords(inputTotalPayble) + " " + "Only", Arial12B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable1.AddCell(cell);

              
                    cell = new PdfPCell(new Phrase("Amt.Payable", Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 0;
                    footerTable1.AddCell(cell);
                

                cell = new PdfPCell(new Phrase(varTotal.ToString(), Arial12B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable1.AddCell(cell);

                cell = new PdfPCell(new Phrase("Pay Mode :", Arial8N));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable1.AddCell(cell);


                cell = new PdfPCell(new Phrase("Amount Paid :", Arial8N));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 0;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 0;
                footerTable1.AddCell(cell);


                cell = new PdfPCell(new Phrase(varPaidTotal.ToString(), Arial12B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable1.AddCell(cell);

                cell = new PdfPCell(new Phrase("NOTE :1. Fee,Charges,Fund,Once paid are not refundable.", Arial8N));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthRight = 1;
                footerTable1.AddCell(cell);

                if (Convert.ToInt32(previous_due) >= 0)
                {
                    cell = new PdfPCell(new Phrase("Previous Dues :", Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1;
                    footerTable1.AddCell(cell);
                }

                else
                {
                    cell = new PdfPCell(new Phrase("Previous Excess :", Arial8N));
                    cell.MinimumHeight = 20f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthLeft = 0;
                    cell.BorderWidthRight = 1;
                    footerTable1.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase(previous_due, Arial12B));
                cell.MinimumHeight = 20f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 0;
                cell.BorderWidthLeft = 0;
                cell.BorderWidthRight = 1;
                footerTable1.AddCell(cell);

                cell = new PdfPCell();
                cell.BorderWidth = 0;
                cell.AddElement(footerTable1);
                innerTable2.AddCell(cell);



                //-------------------------

                
//--------- remove
                //Chunk c1 = null; Paragraph pr = null;
                //pr = new Paragraph();
                //c1 = new Chunk("Total", Ftimes14);
                //pr.Add(c1);
                //c1 = new Chunk("                                                         ", Ftimes12);
                //pr.Add(c1);
                //c1 = new Chunk("Rs " + Convert.ToString(varTotal), Ftimes14);
                //pr.Add(c1);
                //cell = new PdfPCell();
                //cell.AddElement(pr);
                //cell.MinimumHeight = 10f;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = 0;
                //innerTable1.AddCell(cell);

                

                //cell = new PdfPCell(new Phrase("Received Rupees......By Cash/Cheque No......Date....................  Drawn on......Branch......", Arial8N));
                //cell.MinimumHeight = 10f;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = 0;
                //innerTable1.AddCell(cell);

                //cell = new PdfPCell(new Phrase("", Arial8N));
                //cell.MinimumHeight = 10f;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = 0;
                //innerTable1.AddCell(cell);

                
                


                //Chunk c2 = null; Paragraph pr1 = null;
                //pr1 = new Paragraph();
                //c2 = new Chunk("Total", Ftimes10);
                //pr1.Add(c2);
                //c2 = new Chunk("                                                        ", Arial8N);
                //pr1.Add(c2);
                //c2 = new Chunk("Rs " + Convert.ToString(varTotal), Ftimes10);
                //pr1.Add(c2);
                //cell = new PdfPCell();
                //cell.AddElement(pr);
                //cell.MinimumHeight = 10f;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = 0;
                //innerTable2.AddCell(cell);

                //cell = new PdfPCell(new Phrase("Received Rupees......By Cash/Cheque No......Date....................  Drawn on......Branch......", Arial8N));
                //cell.MinimumHeight = 10f;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = 0;
                //innerTable2.AddCell(cell);

                //cell = new PdfPCell(new Phrase("", Arial8N));
                //cell.MinimumHeight = 10f;
                //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                //cell.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell.Border = 0;
                //innerTable2.AddCell(cell);
 
                #endregion
                //----------------------------
                

                cell = new PdfPCell();
                cell.BorderWidth = 0;
                cell.AddElement(innerTable1);
                outerTable.AddCell(cell);

                

                cell = new PdfPCell(new Phrase(""));
                cell.BorderWidth = 0;
            
                outerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.BorderWidth = 0;
                cell.AddElement(innerTable2);
                outerTable1.AddCell(cell);


                objDocument.Add(outerTable);
                objDocument.Add(outerTable1);

               
                i++;
            
            }
            
        }
    }
}