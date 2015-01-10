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
                using (objDocument = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20))
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
        //string varClassCode = Convert.ToString(Request.QueryString["class_id"]);
        objCommand.CommandText = "select student_id from ign_student_master where class_code = '" + cid + "'";
        //objCommand.CommandText = "select student_id from ign_student_master where create_date >='2014-01-25'";
        objDtReader1 = objCommand.ExecuteReader();
        while (objDtReader1.Read())
        {
            var_arry.Add(Convert.ToString(objDtReader1["STUDENT_ID"]));
          
            
        }
        objDtReader1.Close();
        string ReportName =  DateTime.Now.ToString("ddMMMyyyy") + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Millisecond + "test.pdf";
        using (objDocument = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20))
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



            PdfPTable outerTable = new PdfPTable(3);
            outerTable.SetWidths(new float[3] { 30f,10f,30f });
            outerTable.WidthPercentage = 80f;

            PdfPCell cell = null;


            PdfPTable innerTable1 = new PdfPTable(1);
            innerTable1.SetWidths(new float[1] { 10f });
            innerTable1.WidthPercentage = 100f;

            PdfPTable innerTable2 = new PdfPTable(1);
            innerTable2.SetWidths(new float[1] { 10f });
            innerTable2.WidthPercentage = 100f;

            PdfPTable innerTable3 = new PdfPTable(1);
            innerTable3.SetWidths(new float[1] { 10f });
            innerTable3.WidthPercentage = 100f;

            #endregion

            

            string radiobtn = Convert.ToString(Session["new"]);
            if (radiobtn == "NEW")
            {
                objCommand.CommandText = "select student_id from ign_student_master where student_id = '" + sid + "'";
                objDtReader = objCommand.ExecuteReader();
            }
            if (radiobtn == "OLD")
            {
                objCommand.CommandText = "select student_id from ign_student_master_old where student_id = '" + sid + "'";
                objDtReader = objCommand.ExecuteReader();
            }

           // objCommand.CommandText = "select student_id from ign_student_master where   student_id='" + sid + "'";
            
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
                datemonth1 = Convert.ToDateTime(Session["datemoth"]).AddMonths(2).ToString("yyyy-MM-01");

                #region mapped Date
                 
                #endregion

                #region Student Details
                //DateTime MNTHNAME = Convert.ToDateTime(Session["MNTHNAME"]);


                //objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + varStudentId + "'";
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,a.STUDENT_ROLL_NBR,a.DATE_OF_ADMISSION,a.GENDER,a.ADDRESS_LINE1,a.CITY,a.STATE,a.FATHER_NAME,a.FATHER_OCCUPATION,a.MOTHER_NAME,a.MOTHER_OCCUPATION,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + sid + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {

                    cell = new PdfPCell(new Phrase("", Ftimes14));
                    cell.MinimumHeight = 50f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Himalaya Public Sr. Sec. School            School Copy", Ftimes14));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);



                    cell = new PdfPCell(new Phrase("Note: LAST DATE OF FEE DEPOSITE IS 10 OF EVERY 1ST MONTH.AFTER WHICH RS.100 WILL BE CHARGED AS FINE.", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("DATE :" + DateTime.Now.ToString("dd/MM/yyyy"), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Session["sdate"] + "-2014  " + " To " + Session["edate"] + "-2014", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);


                    cell = new PdfPCell(new Phrase("Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable1.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
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





                    //cell = new PdfPCell(new Phrase("Himalaya Public Sr. Sec. School                     Student Copy", Ftimes12));
                    cell = new PdfPCell(new Phrase("Himalaya Public Sr. Sec. School                     Student Copy", Ftimes14));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);


                    

                    cell = new PdfPCell(new Phrase("Note: LAST DATE OF FEE DEPOSITE IS 10 OF EVERY 1ST MONTH.AFTER WHICH RS.100 WILL BE CHARGED AS FINE..", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    cell = new PdfPCell(new Phrase("DATE :" + DateTime.Now.ToString("dd/MM/yyyy"), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Session["sdate"] + "-2014  " + " To " + Session["edate"] + "-2014", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Father's Name: " + Convert.ToString(objDtReader["father_name"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable2.AddCell(cell);




                    //cell = new PdfPCell(new Phrase("Himalaya Public School", Ftimes12));
                    cell = new PdfPCell(new Phrase("Himalaya Public School", Ftimes12));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);


                    cell = new PdfPCell(new Phrase("Note: LAST DATE OF FEE DEPOSITE IS 10 OF EVERY 1ST MONTH.AFTER WHICH RS.100 WILL BE CHARGED AS FINE", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("DATE :" + DateTime.Now.ToString("dd/MM/yyyy"), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Session["sdate"] + "-2014  " + " To " + Session["edate"] + "-2014", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Father's Name: : " + Convert.ToString(objDtReader["father_name"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Adm No.: " + Convert.ToString(objDtReader["student_registration_nbr"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable3.AddCell(cell);

                
                   

                }
                objDtReader.Close();
                #endregion

                #region Fee Details
                int varTotal = 0;
                objCommand.CommandText = "select B.COMPONENT_NAME,sum(A.AMOUNT_PAYBLE-A.DISCOUNT) as AMOUNT_PAYBLE,MAPPED_DATE from collect_component_master A, component_master B where student_id = '" + varStudentId + "' and MAPPED_DATE between '" + datemonth + "' and '" + datemonth1 + "'   and A.COMPONENT_ID = B.COMPONENT_ID group by B.COMPONENT_ID ";
                //Response.Write(objCommand.CommandText);
                //Response.End();

                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                   

                    

                    PdfPTable innerTable4 = new PdfPTable(3);
                    innerTable4.SetWidths(new float[3] { 4f,3f,3f });
                    innerTable4.WidthPercentage = 100f;



                    if (Convert.ToString(objDtReader["COMPONENT_NAME"]).Contains("TUTION"))
                    {
                        cell = new PdfPCell(new Phrase("TUTION FEE", Arial8N));
                    
                        
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
                innerTable4.AddCell(cell);

                cell = new PdfPCell(new Phrase("", Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable4.AddCell(cell);

                cell = new PdfPCell(new Phrase( Convert.ToString(objDtReader["AMOUNT_PAYBLE"]), Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                innerTable4.AddCell(cell);

                cell = new PdfPCell();
                cell.BorderWidth = 0;
                cell.AddElement(innerTable4);
                innerTable1.AddCell(cell);

                

                    

                  

                    PdfPTable innerTable5 = new PdfPTable(3);
                    innerTable5.SetWidths(new float[3] { 4f, 3f, 3f });
                    innerTable5.WidthPercentage = 100f;




                    if (Convert.ToString(objDtReader["COMPONENT_NAME"]).Contains("TUTION"))
                    {
                        cell = new PdfPCell(new Phrase("TUTION FEE", Arial8N));


                    }
                    else if (Convert.ToString(objDtReader["COMPONENT_NAME"]).Contains("DEVELOPMENT"))
                    {
                        cell = new PdfPCell(new Phrase("DEVELOPMENT FEE", Arial8N));

                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["COMPONENT_NAME"]), Arial8N));

                    }
                   // cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["COMPONENT_NAME"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable5.AddCell(cell);

                    cell = new PdfPCell(new Phrase("", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable5.AddCell(cell);

                    cell = new PdfPCell(new Phrase( Convert.ToString(objDtReader["AMOUNT_PAYBLE"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = 0;
                    innerTable5.AddCell(cell);

                    cell = new PdfPCell();
                    cell.BorderWidth = 0;
                    cell.AddElement(innerTable5);
                    innerTable2.AddCell(cell);

                   

                    

                    PdfPTable innerTable6 = new PdfPTable(3);
                    innerTable6.SetWidths(new float[3] { 3f, 3f, 3f });
                    innerTable6.WidthPercentage = 100f;



                   

                    cell = new PdfPCell(new Phrase(Convert.ToString(objDtReader["COMPONENT_NAME"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable6.AddCell(cell);

                    cell = new PdfPCell(new Phrase("", Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    innerTable6.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Rs " + Convert.ToString(objDtReader["AMOUNT_PAYBLE"]), Arial8N));
                    cell.MinimumHeight = 10f;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = 0;
                    innerTable6.AddCell(cell);

                    cell = new PdfPCell();
                    cell.BorderWidth = 0;
                    cell.AddElement(innerTable6);
                    innerTable3.AddCell(cell);


                  


                    varTotal = varTotal + Convert.ToInt32(objDtReader["AMOUNT_PAYBLE"]);

                } objDtReader.Close();

                Chunk c1 = null; Paragraph pr = null;
                pr = new Paragraph();
                c1 = new Chunk("Total", Ftimes10);
                pr.Add(c1);
                c1 = new Chunk("                                                        ", Arial8N);
                pr.Add(c1);
                c1 = new Chunk("Rs " + Convert.ToString(varTotal), Ftimes10);
                pr.Add(c1);
                cell = new PdfPCell();
                cell.AddElement(pr);
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable1.AddCell(cell);

                

                cell = new PdfPCell(new Phrase("Received Rupees......By Cash/Cheque No......Date....................  Drawn on......Branch......", Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable1.AddCell(cell);

                cell = new PdfPCell(new Phrase("", Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable1.AddCell(cell);

                
                


                Chunk c2 = null; Paragraph pr1 = null;
                pr1 = new Paragraph();
                c2 = new Chunk("Total", Ftimes10);
                pr1.Add(c2);
                c2 = new Chunk("                                                        ", Arial8N);
                pr1.Add(c2);
                c2 = new Chunk("Rs " + Convert.ToString(varTotal), Ftimes10);
                pr1.Add(c2);
                cell = new PdfPCell();
                cell.AddElement(pr);
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable2.AddCell(cell);

                cell = new PdfPCell(new Phrase("Received Rupees......By Cash/Cheque No......Date....................  Drawn on......Branch......", Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable2.AddCell(cell);

                cell = new PdfPCell(new Phrase("", Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable2.AddCell(cell);


                 

                

                

                Chunk c3 = null; Paragraph pr2 = null;
                pr2 = new Paragraph();
                c3 = new Chunk("Total", Arial8N);
                pr2.Add(c3);
                c3 = new Chunk("                                                        ", Arial8N);
                pr2.Add(c3);
                c3 = new Chunk("Rs " + Convert.ToString(varTotal), Arial8N);
                pr2.Add(c3);
                cell = new PdfPCell();
                cell.AddElement(pr);
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable3.AddCell(cell);

                cell = new PdfPCell(new Phrase("Received Rupees......By Cash/Cheque No......Date....................  Drawn on......Branch......", Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable3.AddCell(cell);

                cell = new PdfPCell(new Phrase("", Arial8N));
                cell.MinimumHeight = 10f;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                innerTable3.AddCell(cell);

               

                 
                #endregion

                

                cell = new PdfPCell();
                cell.BorderWidth = 0;
                cell.AddElement(innerTable1);
                outerTable.AddCell(cell);

                

                cell = new PdfPCell(new Phrase(""));
                cell.BorderWidth = 0;
               // cell.AddElement(innerTable3);
                outerTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(""));
                cell.BorderWidth = 0;
                cell.AddElement(innerTable2);
                outerTable.AddCell(cell);


                objDocument.Add(outerTable);

               
                i++;
                


            }
            
        }
    }
}