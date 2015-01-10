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

public partial class WebForms_Duplicate_feeRecipt_print_single : System.Web.UI.Page
{
    OdbcConnection objConnection; OdbcCommand objCommand; OdbcDataReader objDtReader;
    string varSchoolSession, discount, receipt_no, total_paid, concession;
    DateTime varMappedDate; DateTime lastmonth; string finalmonth = ""; DateTime minusdate;

    protected void Page_Load(object sender, EventArgs e)
    {

        getduedetailo();
        Session["ttttt"] = "mayank";
        string varClassId = Request.QueryString["class_id"];
        int varSId = Convert.ToInt32(Request.QueryString["student_id"]);
        if (varSId == -1)
        {

        }
        else
        {
            // feeBillName();
        }

        string recipttype = "";
        recipttype = Convert.ToString(Session["print"]);
        if (recipttype == "school")
        {
            SCHOOL();
        }
        else if (recipttype == "student")
        {
            student();
        }
        else if (recipttype == "both")
        {
            feeBillName();
        }

    }


    //-------------------------------------------------------------------------------------------------------------


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



    //-------------------------------------------------------------------------------------------------------------------------------


    public void feeBillName()
    {
        ArrayList objArrayListStudentId = new ArrayList();
        string varClassCode = Convert.ToString(Request.QueryString["class_id"]);
        int varSId = Convert.ToInt32(Request.QueryString["student_id"]);
        varSchoolSession = Convert.ToString(Session["_Session"]);
        objConnection = (OdbcConnection)Session["_Connection"];
        objCommand = new OdbcCommand();
        objCommand.Connection = objConnection;



        int student_id = Convert.ToInt32(Session["_STUDENT_ID"]);
        var detailid = Session["detailid"];

        objCommand.CommandText = "select distinct component_id from collect_component_master where component_id='11' and student_id='" + student_id + "' ";
        string eswstudnt = Convert.ToString(objCommand.ExecuteScalar());



        objCommand.CommandText = "select distinct b.DISCOUNT_VALUE from student_discount_mapping a,discount_master b where a.student_id='" + student_id + "' and a.DISCOUNT_ID=b.DISCOUNT_ID";
        string discount_value = Convert.ToString(objCommand.ExecuteScalar());
        if (eswstudnt == "")
        {

            if (discount_value == "")
            {
                concession = "N/A";
            }
            else
            {
                concession = discount_value;
            }
        }
        else
        {
            concession = "EWS";
        }



        //////////////nnnnnnnnnnnnn/////////////////////////
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select sum(b.AMOUNT_PAYBLE) as AMOUNT_PAYBLE,sum(b.AMOUNT_PAID) as AMOUNT_PAID ,sum(b.AMOUNT_PAYBLE - b.AMOUNT_PAID - b.DISCOUNT) as due,c.COMPONENT_NAME as component,b.discount  from collect_component_detail a ,collect_component_master b,component_master c where b.COMPONENT_ID=c.COMPONENT_ID and b.STUDENT_ID='" + student_id + "' and b.PAID_DATE=a.PAID_DATE and b.DETAIL_ID=a.ID and b.DETAIL_ID='" + detailid + "'  group by b.COMPONENT_ID", objConnection));
        DataTable dt = new DataTable();
        odbc.Fill(dt);
        var Totalamountpayable = "";
        var Totalpayment = "";
        var TotalDiscount = Convert.ToString(Session["TotalDiscount"]);
        objCommand.CommandText = "select sum(a.AMOUNT_PAYBLE) as payable from collect_component_master a where a.DETAIL_ID='" + detailid + "'";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            Totalamountpayable = Convert.ToString(objDtReader["payable"]);
        }
        objDtReader.Close(); objDtReader.Dispose();
        //var Totalamountpayable = Convert.ToString(Session["Totalamountpayable"]);


        var totaldue = Convert.ToString(Session["totaldue"]);
        var A_TotalDiscount = Convert.ToString(Session["A_TotalDiscount"]);

        objCommand.CommandText = "select a.amount_paid as paid  from collect_component_detail a where a.ID='" + detailid + "'";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            Totalpayment = Convert.ToString(objDtReader["paid"]);
        }
        objDtReader.Close(); objDtReader.Dispose();
        // var Totalpayment = Convert.ToString(Session["Totalpayment"]);


        var admitionNO = Convert.ToString(Session["admitionNO"]);
        var mode = Convert.ToString(Session["mode"]);
        var checkno = Convert.ToString(Session["checkno"]);
        var checkdate = Convert.ToString(Session["checkdate"]);
        var bankdetail = Convert.ToString(Session["bankdetail"]);
        var reciptNO = "";


        var firstdate = Convert.ToString(Session["firstdate"]);

        var lastdate = Convert.ToString(Session["lastdate"]);

        var fine = Convert.ToString(Session["fine"]);
        var readmitionFine = Convert.ToString(Session["readmitionFine"]);
        objCommand.CommandText = "select a.Rno as rno from collect_component_detail a where a.id='" + detailid + "' ";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            reciptNO = Convert.ToString(objDtReader["rno"]);
        }
        objDtReader.Close();


        if (!IsPostBack)
        {
            #region
            HtmlTable objHtmlTable = new HtmlTable();
            objHtmlTable.Border = 0;
            objHtmlTable.Width = "800px";
            HtmlTableRow objHtmlTableRow = null;
            HtmlTableCell objHtmlTableCell = null;

            HtmlTable objHtmlTable1 = null;
            HtmlTableRow objHtmlTableRow1 = null;
            HtmlTableCell objHtmlTableCell1 = null;

            HtmlTable objHtmlTable2 = null;
            HtmlTableRow objHtmlTableRow2 = null;
            HtmlTableCell objHtmlTableCell2 = null;


            #endregion

            objCommand.CommandText = "select student_id from ign_student_master where student_registration_nbr='" + admitionNO + "' ";
            objDtReader = objCommand.ExecuteReader();
            while (objDtReader.Read())
            {
                objArrayListStudentId.Add(Convert.ToString(objDtReader["student_id"]));

            }
            objDtReader.Close();
            int i = 0;
            foreach (string varStudentId in objArrayListStudentId)
            {
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTable1 = new HtmlTable(); objHtmlTable1.Border = 0; objHtmlTable1.Attributes.Add("style", "font-size:13px;");

                objHtmlTable2 = new HtmlTable(); objHtmlTable2.Border = 0; objHtmlTable2.Attributes.Add("style", "font-size:13px;");

                #region mapped Date
                objCommand.CommandText = "select max(MAPPED_DATE) from collect_component_master where student_id = '" + varStudentId + "'";

                if (!Convert.ToString(objCommand.ExecuteScalar()).Equals(""))
                {
                    varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                }
                #endregion

                #region Student Details
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,A.STUDENT_ROLL_NBR,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_registration_nbr = '" + admitionNO + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    objHtmlTableCell1.Align = "center";
                    objHtmlTableCell1.InnerHtml = "HAPPY HOME PUBLIC SCHOOL<br/>";
                    objHtmlTableCell1.Style.Add("font-size", "16pt");
                    objHtmlTableCell1.ColSpan = 8;
                    
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "B-4 SECTOR 11 ROHINI DELHI 110085";
                    objHtmlTableCell1.Style.Add("font-size", "13pt");
                    objHtmlTableCell1.Align = "center";

                    objHtmlTableCell1.ColSpan = 8;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "  ";
                    objHtmlTableCell1.Height = "20px";
                    objHtmlTableCell1.Style.Add("font-size", "26pt");
                    objHtmlTableCell1.Align = "center";
                    objHtmlTableCell1.ColSpan = 8;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "SCHOOL COPY";
                    objHtmlTableCell1.Style.Add("font-size", "14pt");
                    objHtmlTableCell1.Align = "left";
                    objHtmlTableCell1.ColSpan = 8;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Receipt No : HHPS " + reciptNO;
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "Pay DATE : " + Session["paiddate"];
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);



                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    objHtmlTableCell1.InnerHtml = "Student's Name : " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["MIDDLE_NAME"]) + " " + Convert.ToString(objDtReader["last_name"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Adm No : " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Father's Name : " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Class : " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                   

                    if (concession == "N/A" || concession == "EWS")
                    {
                        objHtmlTableCell1.InnerText = "Concession :" + concession;

                    }
                    else
                    {
                        objHtmlTableCell1.InnerText = "Concession :" + concession + "%";

                    }

                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "Roll No : " + Convert.ToString(objDtReader["STUDENT_ROLL_NBR"]);
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);



                    //--------------------------------------------------------------------------//

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "";
                    objHtmlTableCell2.ColSpan = 2;

                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);




                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "center";
                    objHtmlTableCell2.InnerHtml = "HAPPY HOME PUBLIC SCHOOL<br/>";
                    objHtmlTableCell2.Style.Add("font-size", "16pt");
                    objHtmlTableCell2.ColSpan = 8;
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);



                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "center";
                    objHtmlTableCell2.InnerHtml = "B-4 SECTOR 11 ROHINI DELHI 110085";
                    objHtmlTableCell2.Style.Add("font-size", "13pt");
                    objHtmlTableCell2.ColSpan = 8;
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "LEFT";
                    objHtmlTableCell2.InnerHtml = "STUDENT COPY";
                    objHtmlTableCell2.Style.Add("font-size", "14pt");
                    objHtmlTableCell2.ColSpan = 8;
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Receipt No : HHPS " + reciptNO;
                    objHtmlTableCell2.ColSpan = 6;
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = "Pay DATE : " + Session["paiddate"];
                    objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);




                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    //objHtmlTableCell2.InnerHtml = "Note: Fees Once paid will not be refunded in any case. Deposit the fee before 15th of each otherwise Fine Rs 10 per day will be charged.<br/><br/>";
                    objHtmlTableCell2.ColSpan = 8;
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);


                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Student's Name : " + Convert.ToString(objDtReader["first_name"]) + " " + " " + Convert.ToString(objDtReader["MIDDLE_NAME"]) + " " + Convert.ToString(objDtReader["last_name"]) + "<br/><br/>";
                    objHtmlTableCell2.ColSpan = 6;
                    objHtmlTableCell2.Style.Add("border-top", "solid 1px black");
                    objHtmlTableCell2.Style.Add("border-left", "solid 1px black");


                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Adm No : " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.Style.Add("border-top", "solid 1px black");
                    objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Father's Name : " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableCell2.ColSpan = 6;

                    objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Class : " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]) + "<br/><br/>";
                    objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();

                    if (concession == "N/A" || concession == "EWS")
                    {
                        objHtmlTableCell2.InnerText = "Concession :" + concession;

                    }
                    else
                    {
                        objHtmlTableCell2.InnerText = "Concession :" + concession + "%";

                    }



                   
                    objHtmlTableCell2.ColSpan = 6;
                    objHtmlTableCell2.Style.Add("border-bottom", "solid 1px black");
                    objHtmlTableCell2.Style.Add("border-left", "solid 1px black");


                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Roll No : " + Convert.ToString(objDtReader["STUDENT_ROLL_NBR"]);
                    objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.Style.Add("border-Bottom", "solid 1px black");
                    objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                }

                objDtReader.Close();
                #endregion

                objHtmlTableRow1 = new HtmlTableRow();
                //objHtmlTableCell1 = new HtmlTableCell();
                //objHtmlTableCell1.InnerHtml = "<br/>" + "S.NO." + "<br/>";
                //objHtmlTableCell1.Style.Add("font-size", "11pt");
                //objHtmlTableCell1.ColSpan = 2;
                //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);


                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Component" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Duration" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Amount" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);





                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Payment" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 1;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                //-------------------------------------------------------


                objHtmlTableRow2 = new HtmlTableRow();
                //objHtmlTableCell2 = new HtmlTableCell();
                //objHtmlTableCell2.InnerHtml = "<br/>" + "S.NO.";
                //objHtmlTableCell2.Style.Add("font-size", "11pt");
                //objHtmlTableCell2.ColSpan = 2;
                //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);




                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>" + "Component";
                objHtmlTableCell2.Style.Add("font-size", "13pt");
                objHtmlTableCell2.ColSpan = 2;
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>" + "Duration";
                objHtmlTableCell2.Style.Add("font-size", "13pt");
                objHtmlTableCell2.ColSpan = 2;
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>" + "Amount";
                objHtmlTableCell2.Style.Add("font-size", "13pt");
                objHtmlTableCell2.ColSpan = 2;
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);




                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>" + "Payment";
                objHtmlTableCell2.Style.Add("font-size", "13pt");
                objHtmlTableCell2.ColSpan = 1;
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                #region Fee Details

                DataTableReader dtr;
                dtr = dt.CreateDataReader();
                //while (dtr.Read())
                //{
                //     varDueAmountPayble = varDueAmountPayble + Convert.ToInt32(dtr["Amount"]);

                //}

                //--------------------------------------------------------------------------------------------------------------------------------------
                int varTotal = 0;


                //--------------------------------------------------------------------------------------------------------------------------------------


                int sno = 0;

                while (dtr.Read())
                {


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    if (Convert.ToString(dtr["Component"]).Contains("TUTION"))
                    {
                        objHtmlTableCell1.InnerHtml = "TUITION FEE";
                    }
                    else if (Convert.ToString(dtr["Component"]).Contains("DEVELOPMENT"))
                    {

                        objHtmlTableCell1.InnerHtml = "DEVELOPMENT FEE";
                    }
                    else
                    {
                        objHtmlTableCell1.InnerHtml = Convert.ToString(dtr["Component"]);
                    }

                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    //-----------------------------------------------------------------------------------------

                    /////////kkkkkkkkkkkkkk////////////////////

                    objCommand.CommandText = "SELECT COMPONENT_ID FROM COMPONENT_MASTER WHERE COMPONENT_NAME='" + Convert.ToString(dtr["Component"]) + "'";
                    var component_id1 = objCommand.ExecuteScalar();
                    objCommand.CommandText = "select sum( a.AMOUNT_PAYBLE) - sum( a.amount_paid)  from collect_component_master a where a.COMPONENT_ID='" + component_id1 + "' and a.MAPPED_DATE between '2014-04-01' and '" + Convert.ToDateTime(firstdate).ToString("yyyy-MM-dd") + "' and student_id='" + varStudentId + "'";
                    string d1 = objCommand.ExecuteScalar().ToString();

                    DateTime finaldate1;
                    string mapdatewitoutZeroAmount1 = "";
                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1,1";

                    mapdatewitoutZeroAmount1 = Convert.ToString(objCommand.ExecuteScalar());

                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1";

                    string Nothingpaid_date = Convert.ToString(objCommand.ExecuteScalar());

                    if (mapdatewitoutZeroAmount1 == "" & Nothingpaid_date == "")
                    {
                        string dateapr = "2014-04-01";
                        finaldate1 = Convert.ToDateTime(dateapr);
                    }

                    else if (mapdatewitoutZeroAmount1 == "" & Nothingpaid_date.Length > 0)
                    {

                        DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                        finaldate1 = Convert.ToDateTime(firstdate);
                    }

                    else
                    {
                        finaldate1 = Convert.ToDateTime(mapdatewitoutZeroAmount1).AddMonths(1);
                    }

                    string finalmonthtoPrint1 = Convert.ToDateTime(finaldate1).ToString("MMM");



                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToDateTime(firstdate).ToString("MMM") + " - " + Convert.ToDateTime(lastdate).ToString("MMM");
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);


                    //------------------------------------kkk------------------------------------------------------


                    objHtmlTableCell1 = new HtmlTableCell();
                    if (Convert.ToString(dtr["Discount"]).Equals(0))
                    {
                        objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(dtr["Amount"]);
                    }
                    else
                    {
                        objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(dtr["AMOUNT_PAID"]);
                    }



                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);



                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToString(dtr["AMOUNT_PAID"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1.ColSpan = 1;
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    //--------------------------------------------------------------------------//                    

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();

                    if (Convert.ToString(dtr["Component"]).Contains("TUTION"))
                    {
                        objHtmlTableCell2.InnerHtml = "TUITION FEE";
                    }
                    else if (Convert.ToString(dtr["Component"]).Contains("DEVELOPMENT"))
                    {

                        objHtmlTableCell2.InnerHtml = "DEVELOPMENT FEE";
                    }
                    else
                    {
                        objHtmlTableCell2.InnerHtml = Convert.ToString(dtr["Component"]);
                    }


                    // objHtmlTableCell2.InnerHtml = Convert.ToString(dtr["Component"]);
                    objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                    //---------------------------------------reciptduemonth--------------------------------------------------

                    objCommand.CommandText = "SELECT COMPONENT_ID FROM COMPONENT_MASTER WHERE COMPONENT_NAME='" + Convert.ToString(dtr["Component"]) + "'";
                    var component_id = objCommand.ExecuteScalar();
                    objCommand.CommandText = "select sum( a.AMOUNT_PAYBLE) - sum( a.amount_paid)  from collect_component_master a where a.COMPONENT_ID='" + component_id + "' and a.MAPPED_DATE between '2014-04-01' and '" + Convert.ToDateTime(firstdate).ToString("yyyy-MM-dd") + "' and student_id='" + varStudentId + "'";
                    string d = objCommand.ExecuteScalar().ToString();





                    DateTime finaldate;
                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1,1";

                    string mapdatewitoutZeroAmount = Convert.ToString(objCommand.ExecuteScalar());

                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1";

                    string Nothingpaid_date1 = Convert.ToString(objCommand.ExecuteScalar());

                    if (mapdatewitoutZeroAmount == "" & Nothingpaid_date1 == "")
                    {

                        string dateapr = "2014-04-01";
                        finaldate = Convert.ToDateTime(dateapr);
                    }

                    else if (mapdatewitoutZeroAmount == "" & Nothingpaid_date1.Length > 0)
                    {

                        DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                        finaldate = Convert.ToDateTime(firstdate);
                    }

                    else
                    {
                        finaldate = Convert.ToDateTime(mapdatewitoutZeroAmount).AddMonths(1);
                    }
                    string finalmonthtoPrint = Convert.ToDateTime(finaldate).ToString("MMM");

                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = Convert.ToDateTime(firstdate).ToString("MMM") + " - " + Convert.ToDateTime(lastdate).ToString("MMM");
                    objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);


                    objHtmlTableCell2 = new HtmlTableCell();
                    if (Convert.ToString(dtr["Discount"]).Equals(0))
                    {
                        objHtmlTableCell2.InnerHtml = "Rs " + Convert.ToString(dtr["AMOUNT_PAID"]);
                    }
                    else
                    {
                        objHtmlTableCell2.InnerHtml = "Rs " + Convert.ToString(dtr["AMOUNT_PAID"]);
                    }

                    // objHtmlTableCell2.InnerHtml = "Rs " + Convert.ToString(dtr["Amount"]);
                    objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);
                    varTotal = varTotal + Convert.ToInt32(dtr["AMOUNT_PAID"]);





                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = Convert.ToString(dtr["AMOUNT_PAID"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    ;

                } objDtReader.Close();






                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Fine";
                objHtmlTableCell1.ColSpan = 6;

                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = fine;
                objHtmlTableCell1.ColSpan = 1;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);





                //objHtmlTableRow1 = new HtmlTableRow();
                //objHtmlTableCell1 = new HtmlTableCell();
                //objHtmlTableCell1.InnerHtml = "<br/>Total";
                //objHtmlTableCell1.ColSpan = 4;

                //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                //objHtmlTableCell1 = new HtmlTableCell();
                //objHtmlTableCell1.InnerHtml = "<br/>Rs " + Convert.ToString(Totalamountpayable);
                //objHtmlTableCell1.ColSpan = 4;
                //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                //-----------------------------------------------------------------------------------------

                int inputTotalPayble = Convert.ToUInt16(varTotal);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = "IN WORDS : " + Converter.NumberToWords(inputTotalPayble) + " " + "Only";
                objHtmlTableCell1.ColSpan = 6;
                objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Amt. Payable : " + "Rs " + Totalamountpayable;
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();

                if (Convert.ToString(Session["mode"]) == "CASH")
                {
                    objHtmlTableCell1.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]);

                }
                else
                {
                    objHtmlTableCell1.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]) + "  " + Convert.ToString(Session["checkno"]) + "  " + Convert.ToDateTime(Session["checkdate"]).ToString("dd-MMM-yyyy") + "  " + Convert.ToString(Session["bankdetail"]);


                }


                objHtmlTableCell1.ColSpan = 6;
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Amt. Paid :" + " Rs " + Totalpayment;
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = "NOTE :";
                objHtmlTableCell1.ColSpan = 6;

                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Dues : " + "Rs " + totaldue;
                objHtmlTableCell1.ColSpan = 2;
                //objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = " 1. Fee,Charges,Funds Once paid are not refundable. ";
                objHtmlTableCell1.ColSpan = 8;
                //objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = " 2. Cheque subject to encashment";
                objHtmlTableCell1.ColSpan = 8;
                objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);






                //-----------------------------------------------------------------------------------------


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;

                //----------------------------------------------------------------------------------------------------
                objHtmlTableCell1.InnerHtml = "         " + "<br/>" + " <br/>" + "<br/>" + " <br/>------------------------------------------------------------------------------------------------------------------------- <br/>" + "<br/><br/>";
                //---------------------------------------------------------------------------------------------------------


                //objHtmlTableCell1.InnerHtml = "Received Rupees........" + Totalpayment + "<br/> Mode of Payment........" + mode + " <br/>Cheque No...................." + checkno + "<br/>Date................................" + checkdate + " <br/> Drawn on......................<br/>Branch.........................." + bankdetail + "<br/><br/>Cashier.............................";
                objHtmlTableCell1.ColSpan = 8;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "Fine";
                objHtmlTableCell2.ColSpan = 6;

                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = fine;
                objHtmlTableCell2.ColSpan = 1;
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);





                //objHtmlTableRow2 = new HtmlTableRow();
                //objHtmlTableCell2 = new HtmlTableCell();
                //objHtmlTableCell2.InnerHtml = "<br/>Total";
                //objHtmlTableCell2.ColSpan = 4;
                //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                //objHtmlTableCell2 = new HtmlTableCell();
                //objHtmlTableCell2.InnerHtml = "<br/>Rs " + Convert.ToString(varTotal);
                //objHtmlTableCell2.ColSpan = 4;
                //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                //objHtmlTable2.Rows.Add(objHtmlTableRow2);

                //-----------------------------------------------------------------------------------

                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerText = "IN WORDS : " + Converter.NumberToWords(inputTotalPayble) + " " + "Only";
                objHtmlTableCell2.ColSpan = 6;
                objHtmlTableCell2.Style.Add("border-top", "solid 1px black");
                objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                string gtot = Convert.ToString(Session["tt"]);

                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "Amt. Payable : " + "Rs " + Totalamountpayable;
                objHtmlTableCell2.ColSpan = 2;
                objHtmlTableCell2.Style.Add("border-top", "solid 1px black");
                objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();

                if (Convert.ToString(Session["mode"]) == "CASH")
                {
                    objHtmlTableCell2.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]);

                }
                else
                {
                    objHtmlTableCell2.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]) + "  " + Convert.ToString(Session["checkno"]) + "  " + Convert.ToDateTime(Session["checkdate"]).ToString("dd-MMM-yyyy") + "  " + Convert.ToString(Session["bankdetail"]);


                }



                objHtmlTableCell2.ColSpan = 6;
                objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "Amt. Paid :" + " Rs " + Totalpayment;
                objHtmlTableCell2.ColSpan = 2;
                objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerText = "NOTE :";
                objHtmlTableCell2.ColSpan = 6;

                objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "Dues : " + "Rs " + totaldue;
                objHtmlTableCell2.ColSpan = 2;

                objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);



                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerText = "1. Fee,Charges,Funds Once paid are not refundable. ";
                objHtmlTableCell2.ColSpan = 8;
                //objHtmlTableCell2.Style.Add("border-bottom", "solid 1px black");
                objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerText = "2. Cheque subject to encashment ";
                objHtmlTableCell2.ColSpan = 8;
                objHtmlTableCell2.Style.Add("border-bottom", "solid 1px black");
                objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                //-----------------------------------------------------------------------------------
                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                //objHtmlTableCell2.InnerHtml = "Received Rupees........" + Totalpayment + "<br/> Mode of Payment........" + mode + " <br/>Cheque No...................." + checkno + "<br/>Date................................" + checkdate + " <br/> Drawn on......................<br/>Branch.........................." + bankdetail + "<br/><br/>Cashier.............................";
                objHtmlTableCell2.ColSpan = 7;
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);
                #endregion

                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "center";
                objHtmlTableCell.Controls.Add(objHtmlTable1);
                objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);




                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "center";
                objHtmlTableCell.Controls.Add(objHtmlTable2);
                objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);

                objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);



                i++;
                if (i % 2 == 0)
                {
                    Panel1.Attributes.Add("style", "PAGE-BREAK-AFTER: always");
                }
                Panel1.Controls.Add(objHtmlTable);

            }
        }
    }

    public void getduedetailo()
    {


    }
    public void SCHOOL()
    {
        ArrayList objArrayListStudentId = new ArrayList();
        string varClassCode = Convert.ToString(Request.QueryString["class_id"]);
        int varSId = Convert.ToInt32(Request.QueryString["student_id"]);
        varSchoolSession = Convert.ToString(Session["_Session"]);
        objConnection = (OdbcConnection)Session["_Connection"];
        objCommand = new OdbcCommand();
        objCommand.Connection = objConnection;



        int student_id = Convert.ToInt32(Session["_STUDENT_ID"]);
        var detailid = Session["detailid"];


        objCommand.CommandText = "select distinct component_id from collect_component_master where component_id='11' and student_id='" + student_id + "' ";
        string eswstudnt = Convert.ToString(objCommand.ExecuteScalar());



        objCommand.CommandText = "select distinct b.DISCOUNT_VALUE from student_discount_mapping a,discount_master b where a.student_id='" + student_id + "' and a.DISCOUNT_ID=b.DISCOUNT_ID";
        string discount_value = Convert.ToString(objCommand.ExecuteScalar());
        if (eswstudnt == "")
        {

            if (discount_value == "")
            {
                concession = "N/A";
            }
            else
            {
                concession = discount_value;
            }
        }
        else
        {
            concession = "EWS";
        }




        //////////////nnnnnnnnnnnnn/////////////////////////
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select sum(b.AMOUNT_PAYBLE) as AMOUNT_PAYBLE,sum(b.AMOUNT_PAID) as AMOUNT_PAID ,sum(b.AMOUNT_PAYBLE - b.AMOUNT_PAID - b.DISCOUNT) as due,c.COMPONENT_NAME as component,b.discount  from collect_component_detail a ,collect_component_master b,component_master c where b.COMPONENT_ID=c.COMPONENT_ID and b.STUDENT_ID='" + student_id + "' and b.PAID_DATE=a.PAID_DATE and b.DETAIL_ID=a.ID and b.DETAIL_ID='" + detailid + "'  group by b.COMPONENT_ID", objConnection));
        DataTable dt = new DataTable();
        odbc.Fill(dt);
        var Totalamountpayable = "";
        var Totalpayment = "";
        var TotalDiscount = Convert.ToString(Session["TotalDiscount"]);
        objCommand.CommandText = "select sum(a.AMOUNT_PAYBLE) as payable from collect_component_master a where a.DETAIL_ID='" + detailid + "'";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            Totalamountpayable = Convert.ToString(objDtReader["payable"]);
        }
        objDtReader.Close(); objDtReader.Dispose();
        //var Totalamountpayable = Convert.ToString(Session["Totalamountpayable"]);


        var totaldue = Convert.ToString(Session["totaldue"]);
        var A_TotalDiscount = Convert.ToString(Session["A_TotalDiscount"]);

        objCommand.CommandText = "select a.amount_paid as paid  from collect_component_detail a where a.ID='" + detailid + "'";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            Totalpayment = Convert.ToString(objDtReader["paid"]);
        }
        objDtReader.Close(); objDtReader.Dispose();
        // var Totalpayment = Convert.ToString(Session["Totalpayment"]);


        var admitionNO = Convert.ToString(Session["admitionNO"]);
        var mode = Convert.ToString(Session["mode"]);
        var checkno = Convert.ToString(Session["checkno"]);
        var checkdate = Convert.ToString(Session["checkdate"]);
        var bankdetail = Convert.ToString(Session["bankdetail"]);
        var reciptNO = "";


        var firstdate = Convert.ToString(Session["firstdate"]);

        var lastdate = Convert.ToString(Session["lastdate"]);

        var fine = Convert.ToString(Session["fine"]);
        var readmitionFine = Convert.ToString(Session["readmitionFine"]);
        objCommand.CommandText = "select a.Rno as rno from collect_component_detail a where a.id='" + detailid + "' ";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            reciptNO = Convert.ToString(objDtReader["rno"]);
        }
        objDtReader.Close();


        if (!IsPostBack)
        {
            #region
            HtmlTable objHtmlTable = new HtmlTable();
            objHtmlTable.Border = 0;
            objHtmlTable.Width = "800px";
            HtmlTableRow objHtmlTableRow = null;
            HtmlTableCell objHtmlTableCell = null;

            HtmlTable objHtmlTable1 = null;
            HtmlTableRow objHtmlTableRow1 = null;
            HtmlTableCell objHtmlTableCell1 = null;

            HtmlTable objHtmlTable2 = null;
            HtmlTableRow objHtmlTableRow2 = null;
            HtmlTableCell objHtmlTableCell2 = null;


            #endregion

            objCommand.CommandText = "select student_id from ign_student_master where student_registration_nbr='" + admitionNO + "' ";
            objDtReader = objCommand.ExecuteReader();
            while (objDtReader.Read())
            {
                objArrayListStudentId.Add(Convert.ToString(objDtReader["student_id"]));

            }
            objDtReader.Close();
            int i = 0;
            foreach (string varStudentId in objArrayListStudentId)
            {
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTable1 = new HtmlTable(); objHtmlTable1.Border = 0; objHtmlTable1.Attributes.Add("style", "font-size:13px;");

                objHtmlTable2 = new HtmlTable(); objHtmlTable2.Border = 0; objHtmlTable2.Attributes.Add("style", "font-size:13px;");

                #region mapped Date
                objCommand.CommandText = "select max(MAPPED_DATE) from collect_component_master where student_id = '" + varStudentId + "'";

                if (!Convert.ToString(objCommand.ExecuteScalar()).Equals(""))
                {
                    varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                }
                #endregion

                #region Student Details
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,A.STUDENT_ROLL_NBR,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_registration_nbr = '" + admitionNO + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    objHtmlTableCell1.Align = "center";
                    objHtmlTableCell1.InnerHtml = "HAPPY HOME PUBLIC SCHOOL<br/>";
                    objHtmlTableCell1.Style.Add("font-size", "14pt");
                    objHtmlTableCell1.ColSpan = 8;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "B-4 SECTOR 11 ROHINI DELHI 110085";
                    objHtmlTableCell1.Style.Add("font-size", "11pt");
                    objHtmlTableCell1.Align = "center";

                    objHtmlTableCell1.ColSpan = 8;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "SCHOOL COPY";
                    objHtmlTableCell1.Style.Add("font-size", "10pt");
                    objHtmlTableCell1.Align = "left";
                    objHtmlTableCell1.ColSpan = 8;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Receipt No : HHPS " + reciptNO;
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "Pay DATE : " + Session["paiddate"];
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);



                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    objHtmlTableCell1.InnerHtml = "Student's Name : " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["MIDDLE_NAME"]) + " " + Convert.ToString(objDtReader["last_name"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Adm No : " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Father's Name : " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Class : " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    if (concession == "N/A" || concession == "EWS")
                    {
                        objHtmlTableCell1.InnerText = "Concession :" + concession;

                    }
                    else
                    {
                        objHtmlTableCell1.InnerText = "Concession :" + concession + "%";

                    }
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "Roll No : " + Convert.ToString(objDtReader["STUDENT_ROLL_NBR"]);
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);



                    //--------------------------------------------------------------------------//



                }

                objDtReader.Close();
                #endregion

                objHtmlTableRow1 = new HtmlTableRow();
                //objHtmlTableCell1 = new HtmlTableCell();
                //objHtmlTableCell1.InnerHtml = "<br/>" + "S.NO." + "<br/>";
                //objHtmlTableCell1.Style.Add("font-size", "11pt");
                //objHtmlTableCell1.ColSpan = 2;
                //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);


                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Component" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "11pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Duration" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "11pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Amount" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "11pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);





                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Payment" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "11pt");
                objHtmlTableCell1.ColSpan = 1;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                //-------------------------------------------------------




                #region Fee Details

                DataTableReader dtr;
                dtr = dt.CreateDataReader();
                //while (dtr.Read())
                //{
                //     varDueAmountPayble = varDueAmountPayble + Convert.ToInt32(dtr["Amount"]);

                //}

                //--------------------------------------------------------------------------------------------------------------------------------------
                int varTotal = 0;


                //--------------------------------------------------------------------------------------------------------------------------------------


                int sno = 0;

                while (dtr.Read())
                {


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    if (Convert.ToString(dtr["Component"]).Contains("TUTION"))
                    {
                        objHtmlTableCell1.InnerHtml = "TUITION FEE";
                    }
                    else if (Convert.ToString(dtr["Component"]).Contains("DEVELOPMENT"))
                    {

                        objHtmlTableCell1.InnerHtml = "DEVELOPMENT FEE";
                    }
                    else
                    {
                        objHtmlTableCell1.InnerHtml = Convert.ToString(dtr["Component"]);
                    }

                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    //-----------------------------------------------------------------------------------------

                    /////////kkkkkkkkkkkkkk////////////////////

                    objCommand.CommandText = "SELECT COMPONENT_ID FROM COMPONENT_MASTER WHERE COMPONENT_NAME='" + Convert.ToString(dtr["Component"]) + "'";
                    var component_id1 = objCommand.ExecuteScalar();
                    objCommand.CommandText = "select sum( a.AMOUNT_PAYBLE) - sum( a.amount_paid)  from collect_component_master a where a.COMPONENT_ID='" + component_id1 + "' and a.MAPPED_DATE between '2014-04-01' and '" + Convert.ToDateTime(firstdate).ToString("yyyy-MM-dd") + "' and student_id='" + varStudentId + "'";
                    string d1 = objCommand.ExecuteScalar().ToString();

                    DateTime finaldate1;
                    string mapdatewitoutZeroAmount1 = "";
                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1,1";

                    mapdatewitoutZeroAmount1 = Convert.ToString(objCommand.ExecuteScalar());

                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1";

                    string Nothingpaid_date = Convert.ToString(objCommand.ExecuteScalar());

                    if (mapdatewitoutZeroAmount1 == "" & Nothingpaid_date == "")
                    {
                        string dateapr = "2014-04-01";
                        finaldate1 = Convert.ToDateTime(dateapr);
                    }

                    else if (mapdatewitoutZeroAmount1 == "" & Nothingpaid_date.Length > 0)
                    {

                        DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                        finaldate1 = Convert.ToDateTime(firstdate);
                    }

                    else
                    {
                        finaldate1 = Convert.ToDateTime(mapdatewitoutZeroAmount1).AddMonths(1);
                    }

                    string finalmonthtoPrint1 = Convert.ToDateTime(finaldate1).ToString("MMM");



                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToDateTime(firstdate).ToString("MMM") + " - " + Convert.ToDateTime(lastdate).ToString("MMM");
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);


                    //------------------------------------kkk------------------------------------------------------


                    objHtmlTableCell1 = new HtmlTableCell();
                    if (Convert.ToString(dtr["Discount"]).Equals(0))
                    {
                        objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(dtr["Amount"]);
                    }
                    else
                    {
                        objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(dtr["AMOUNT_PAID"]);
                    }



                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);



                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToString(dtr["AMOUNT_PAID"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1.ColSpan = 1;
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    //--------------------------------------------------------------------------//                    



                    //---------------------------------------reciptduemonth--------------------------------------------------

                    objCommand.CommandText = "SELECT COMPONENT_ID FROM COMPONENT_MASTER WHERE COMPONENT_NAME='" + Convert.ToString(dtr["Component"]) + "'";
                    var component_id = objCommand.ExecuteScalar();
                    objCommand.CommandText = "select sum( a.AMOUNT_PAYBLE) - sum( a.amount_paid)  from collect_component_master a where a.COMPONENT_ID='" + component_id + "' and a.MAPPED_DATE between '2014-04-01' and '" + Convert.ToDateTime(firstdate).ToString("yyyy-MM-dd") + "' and student_id='" + varStudentId + "'";
                    string d = objCommand.ExecuteScalar().ToString();





                    DateTime finaldate;
                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1,1";

                    string mapdatewitoutZeroAmount = Convert.ToString(objCommand.ExecuteScalar());

                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1";

                    string Nothingpaid_date1 = Convert.ToString(objCommand.ExecuteScalar());

                    if (mapdatewitoutZeroAmount == "" & Nothingpaid_date1 == "")
                    {

                        string dateapr = "2014-04-01";
                        finaldate = Convert.ToDateTime(dateapr);
                    }

                    else if (mapdatewitoutZeroAmount == "" & Nothingpaid_date1.Length > 0)
                    {

                        DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                        finaldate = Convert.ToDateTime(firstdate);
                    }

                    else
                    {
                        finaldate = Convert.ToDateTime(mapdatewitoutZeroAmount).AddMonths(1);
                    }
                    string finalmonthtoPrint = Convert.ToDateTime(finaldate).ToString("MMM");





                    varTotal = varTotal + Convert.ToInt32(dtr["AMOUNT_PAID"]);






                    ;

                } objDtReader.Close();






                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Fine";
                objHtmlTableCell1.ColSpan = 6;

                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = fine;
                objHtmlTableCell1.ColSpan = 1;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);





                //objHtmlTableRow1 = new HtmlTableRow();
                //objHtmlTableCell1 = new HtmlTableCell();
                //objHtmlTableCell1.InnerHtml = "<br/>Total";
                //objHtmlTableCell1.ColSpan = 4;

                //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                //objHtmlTableCell1 = new HtmlTableCell();
                //objHtmlTableCell1.InnerHtml = "<br/>Rs " + Convert.ToString(Totalamountpayable);
                //objHtmlTableCell1.ColSpan = 4;
                //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                //-----------------------------------------------------------------------------------------

                int inputTotalPayble = Convert.ToUInt16(varTotal);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = "IN WORDS : " + Converter.NumberToWords(inputTotalPayble) + " " + "Only";
                objHtmlTableCell1.ColSpan = 6;
                objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Amt. Payable : " + "Rs " + Totalamountpayable;
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();

                if (Convert.ToString(Session["mode"]) == "CASH")
                {
                    objHtmlTableCell1.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]);

                }
                else
                {
                    objHtmlTableCell1.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]) + "  " + Convert.ToString(Session["checkno"]) + "  " + Convert.ToDateTime(Session["checkdate"]).ToString("dd-MMM-yyyy") + "  " + Convert.ToString(Session["bankdetail"]);


                }

              //  objHtmlTableCell1.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]) + "  " + Convert.ToString(Session["checkno"]) + "  " + Convert.ToString(Session["checkdate"]) + "  " + Convert.ToString(Session["bankdetail"]);
                objHtmlTableCell1.ColSpan = 6;
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Amt. Paid :" + " Rs " + Totalpayment;
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = "NOTE :";
                objHtmlTableCell1.ColSpan = 6;

                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Dues : " + "Rs " + totaldue;
                objHtmlTableCell1.ColSpan = 2;
                //objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = " 1. Fee,Charges,Funds Once paid are not refundable. ";
                objHtmlTableCell1.ColSpan = 8;
                //objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = " 2. Cheque subject to encashment";
                objHtmlTableCell1.ColSpan = 8;
                objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);





                //-----------------------------------------------------------------------------------------


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;

                //----------------------------------------------------------------------------------------------------
                objHtmlTableCell1.InnerHtml = "         " + "<br/>" + " <br/>" + "<br/>" + " <br/>------------------------------------------------------------------------------------------------------------------------- <br/>" + "<br/><br/>";
                //---------------------------------------------------------------------------------------------------------


                //objHtmlTableCell1.InnerHtml = "Received Rupees........" + Totalpayment + "<br/> Mode of Payment........" + mode + " <br/>Cheque No...................." + checkno + "<br/>Date................................" + checkdate + " <br/> Drawn on......................<br/>Branch.........................." + bankdetail + "<br/><br/>Cashier.............................";
                objHtmlTableCell1.ColSpan = 8;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);










                string gtot = Convert.ToString(Session["tt"]);







                #endregion

                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "center";
                objHtmlTableCell.Controls.Add(objHtmlTable1);
                objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);






                objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);



                i++;
                if (i % 2 == 0)
                {
                    Panel1.Attributes.Add("style", "PAGE-BREAK-AFTER: always");
                }
                Panel1.Controls.Add(objHtmlTable);

            }
        }
    }

    public void student()
    {
        ArrayList objArrayListStudentId = new ArrayList();
        string varClassCode = Convert.ToString(Request.QueryString["class_id"]);
        int varSId = Convert.ToInt32(Request.QueryString["student_id"]);
        varSchoolSession = Convert.ToString(Session["_Session"]);
        objConnection = (OdbcConnection)Session["_Connection"];
        objCommand = new OdbcCommand();
        objCommand.Connection = objConnection;



        int student_id = Convert.ToInt32(Session["_STUDENT_ID"]);
        var detailid = Session["detailid"];

        objCommand.CommandText = "select distinct component_id from collect_component_master where component_id='11' and student_id='" + student_id + "' ";
        string eswstudnt = Convert.ToString(objCommand.ExecuteScalar());



        objCommand.CommandText = "select distinct b.DISCOUNT_VALUE from student_discount_mapping a,discount_master b where a.student_id='" + student_id + "' and a.DISCOUNT_ID=b.DISCOUNT_ID";
        string discount_value = Convert.ToString(objCommand.ExecuteScalar());
        if (eswstudnt == "")
        {

            if (discount_value == "")
            {
                concession = "N/A";
            }
            else
            {
                concession = discount_value;
            }
        }
        else
        {
            concession = "EWS";
        }


        //////////////nnnnnnnnnnnnn/////////////////////////
        OdbcDataAdapter odbc = new OdbcDataAdapter(new OdbcCommand("select sum(b.AMOUNT_PAYBLE) as AMOUNT_PAYBLE,sum(b.AMOUNT_PAID) as AMOUNT_PAID ,sum(b.AMOUNT_PAYBLE - b.AMOUNT_PAID - b.DISCOUNT) as due,c.COMPONENT_NAME as component,b.discount  from collect_component_detail a ,collect_component_master b,component_master c where b.COMPONENT_ID=c.COMPONENT_ID and b.STUDENT_ID='" + student_id + "' and b.PAID_DATE=a.PAID_DATE and b.DETAIL_ID=a.ID and b.DETAIL_ID='" + detailid + "'  group by b.COMPONENT_ID", objConnection));
        DataTable dt = new DataTable();
        odbc.Fill(dt);
        var Totalamountpayable = "";
        var Totalpayment = "";
        var TotalDiscount = Convert.ToString(Session["TotalDiscount"]);
        objCommand.CommandText = "select sum(a.AMOUNT_PAYBLE) as payable from collect_component_master a where a.DETAIL_ID='" + detailid + "'";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            Totalamountpayable = Convert.ToString(objDtReader["payable"]);
        }
        objDtReader.Close(); objDtReader.Dispose();
        //var Totalamountpayable = Convert.ToString(Session["Totalamountpayable"]);


        var totaldue = Convert.ToString(Session["totaldue"]);
        var A_TotalDiscount = Convert.ToString(Session["A_TotalDiscount"]);

        objCommand.CommandText = "select a.amount_paid as paid  from collect_component_detail a where a.ID='" + detailid + "'";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            Totalpayment = Convert.ToString(objDtReader["paid"]);
        }
        objDtReader.Close(); objDtReader.Dispose();
        // var Totalpayment = Convert.ToString(Session["Totalpayment"]);


        var admitionNO = Convert.ToString(Session["admitionNO"]);
        var mode = Convert.ToString(Session["mode"]);
        var checkno = Convert.ToString(Session["checkno"]);
        var checkdate = Convert.ToString(Session["checkdate"]);
        var bankdetail = Convert.ToString(Session["bankdetail"]);
        var reciptNO = "";


        var firstdate = Convert.ToString(Session["firstdate"]);

        var lastdate = Convert.ToString(Session["lastdate"]);

        var fine = Convert.ToString(Session["fine"]);
        var readmitionFine = Convert.ToString(Session["readmitionFine"]);
        objCommand.CommandText = "select a.Rno as rno from collect_component_detail a where a.id='" + detailid + "' ";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            reciptNO = Convert.ToString(objDtReader["rno"]);
        }
        objDtReader.Close();


        if (!IsPostBack)
        {
            #region
            HtmlTable objHtmlTable = new HtmlTable();
            objHtmlTable.Border = 0;
            objHtmlTable.Width = "800px";
            HtmlTableRow objHtmlTableRow = null;
            HtmlTableCell objHtmlTableCell = null;

            HtmlTable objHtmlTable1 = null;
            HtmlTableRow objHtmlTableRow1 = null;
            HtmlTableCell objHtmlTableCell1 = null;

            HtmlTable objHtmlTable2 = null;
            HtmlTableRow objHtmlTableRow2 = null;
            HtmlTableCell objHtmlTableCell2 = null;


            #endregion

            objCommand.CommandText = "select student_id from ign_student_master where student_registration_nbr='" + admitionNO + "' ";
            objDtReader = objCommand.ExecuteReader();
            while (objDtReader.Read())
            {
                objArrayListStudentId.Add(Convert.ToString(objDtReader["student_id"]));

            }
            objDtReader.Close();
            int i = 0;
            foreach (string varStudentId in objArrayListStudentId)
            {
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTable1 = new HtmlTable(); objHtmlTable1.Border = 0; objHtmlTable1.Attributes.Add("style", "font-size:13px;");

                objHtmlTable2 = new HtmlTable(); objHtmlTable2.Border = 0; objHtmlTable2.Attributes.Add("style", "font-size:13px;");

                #region mapped Date
                objCommand.CommandText = "select max(MAPPED_DATE) from collect_component_master where student_id = '" + varStudentId + "'";

                if (!Convert.ToString(objCommand.ExecuteScalar()).Equals(""))
                {
                    varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                }
                #endregion

                #region Student Details
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,A.STUDENT_ROLL_NBR,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_registration_nbr = '" + admitionNO + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    objHtmlTableCell1.Align = "center";
                    objHtmlTableCell1.InnerHtml = "HAPPY HOME PUBLIC SCHOOL<br/>";
                    objHtmlTableCell1.Style.Add("font-size", "16pt");
                    objHtmlTableCell1.ColSpan = 8;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "B-4 SECTOR 11 ROHINI DELHI 110085";
                    objHtmlTableCell1.Style.Add("font-size", "13pt");
                    objHtmlTableCell1.Align = "center";

                    objHtmlTableCell1.ColSpan = 8;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "STUDENT COPY";
                    objHtmlTableCell1.Style.Add("font-size", "12pt");
                    objHtmlTableCell1.Align = "left";
                    objHtmlTableCell1.ColSpan = 8;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Receipt No : HHPS " + reciptNO;
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "Pay DATE : " + Session["paiddate"];
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);



                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    objHtmlTableCell1.InnerHtml = "Student's Name : " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["MIDDLE_NAME"]) + " " + Convert.ToString(objDtReader["last_name"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Adm No : " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Father's Name : " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Class : " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]) + "<br/><br/>";
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    if (concession == "N/A" || concession == "EWS")
                    {
                        objHtmlTableCell1.InnerText = "Concession :" + concession;

                    }
                    else
                    {
                        objHtmlTableCell1.InnerText = "Concession :" + concession + "%";

                    }





                  
                    objHtmlTableCell1.ColSpan = 6;
                    objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerText = "Roll No : " + Convert.ToString(objDtReader["STUDENT_ROLL_NBR"]);
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                    objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);



                    //--------------------------------------------------------------------------//



                }

                objDtReader.Close();
                #endregion

                objHtmlTableRow1 = new HtmlTableRow();
                //objHtmlTableCell1 = new HtmlTableCell();
                //objHtmlTableCell1.InnerHtml = "<br/>" + "S.NO." + "<br/>";
                //objHtmlTableCell1.Style.Add("font-size", "11pt");
                //objHtmlTableCell1.ColSpan = 2;
                //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);


                objHtmlTableCell1 = new HtmlTableCell();
                
                objHtmlTableCell1.InnerHtml = "<br/>" + "Component" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Duration" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Amount" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);





                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Payment" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 1;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                //-------------------------------------------------------




                #region Fee Details

                DataTableReader dtr;
                dtr = dt.CreateDataReader();
                //while (dtr.Read())
                //{
                //     varDueAmountPayble = varDueAmountPayble + Convert.ToInt32(dtr["Amount"]);

                //}

                //--------------------------------------------------------------------------------------------------------------------------------------
                int varTotal = 0;


                //--------------------------------------------------------------------------------------------------------------------------------------


                int sno = 0;

                while (dtr.Read())
                {


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();

                    if (Convert.ToString(dtr["Component"]).Contains("TUTION"))
                    {
                        objHtmlTableCell1.InnerHtml = "TUITION FEE";
                    }
                    else if (Convert.ToString(dtr["Component"]).Contains("DEVELOPMENT"))
                    {

                        objHtmlTableCell1.InnerHtml = "DEVELOPMENT FEE";
                    }
                    else
                    {
                        objHtmlTableCell1.InnerHtml = Convert.ToString(dtr["Component"]);
                    }

                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    //-----------------------------------------------------------------------------------------

                    /////////kkkkkkkkkkkkkk////////////////////

                    objCommand.CommandText = "SELECT COMPONENT_ID FROM COMPONENT_MASTER WHERE COMPONENT_NAME='" + Convert.ToString(dtr["Component"]) + "'";
                    var component_id1 = objCommand.ExecuteScalar();
                    objCommand.CommandText = "select sum( a.AMOUNT_PAYBLE) - sum( a.amount_paid)  from collect_component_master a where a.COMPONENT_ID='" + component_id1 + "' and a.MAPPED_DATE between '2014-04-01' and '" + Convert.ToDateTime(firstdate).ToString("yyyy-MM-dd") + "' and student_id='" + varStudentId + "'";
                    string d1 = objCommand.ExecuteScalar().ToString();

                    DateTime finaldate1;
                    string mapdatewitoutZeroAmount1 = "";
                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1,1";

                    mapdatewitoutZeroAmount1 = Convert.ToString(objCommand.ExecuteScalar());

                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1";

                    string Nothingpaid_date = Convert.ToString(objCommand.ExecuteScalar());

                    if (mapdatewitoutZeroAmount1 == "" & Nothingpaid_date == "")
                    {
                        string dateapr = "2014-04-01";
                        finaldate1 = Convert.ToDateTime(dateapr);
                    }

                    else if (mapdatewitoutZeroAmount1 == "" & Nothingpaid_date.Length > 0)
                    {

                        DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                        finaldate1 = Convert.ToDateTime(firstdate);
                    }

                    else
                    {
                        finaldate1 = Convert.ToDateTime(mapdatewitoutZeroAmount1).AddMonths(1);
                    }

                    string finalmonthtoPrint1 = Convert.ToDateTime(finaldate1).ToString("MMM");



                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToDateTime(firstdate).ToString("MMM") + " - " + Convert.ToDateTime(lastdate).ToString("MMM");
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);


                    //------------------------------------kkk------------------------------------------------------


                    objHtmlTableCell1 = new HtmlTableCell();
                    if (Convert.ToString(dtr["Discount"]).Equals(0))
                    {
                        objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(dtr["Amount"]);
                    }
                    else
                    {
                        objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(dtr["AMOUNT_PAID"]);
                    }



                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);



                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToString(dtr["AMOUNT_PAID"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1.ColSpan = 1;
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    //--------------------------------------------------------------------------//                    



                    //---------------------------------------reciptduemonth--------------------------------------------------

                    objCommand.CommandText = "SELECT COMPONENT_ID FROM COMPONENT_MASTER WHERE COMPONENT_NAME='" + Convert.ToString(dtr["Component"]) + "'";
                    var component_id = objCommand.ExecuteScalar();
                    objCommand.CommandText = "select sum( a.AMOUNT_PAYBLE) - sum( a.amount_paid)  from collect_component_master a where a.COMPONENT_ID='" + component_id + "' and a.MAPPED_DATE between '2014-04-01' and '" + Convert.ToDateTime(firstdate).ToString("yyyy-MM-dd") + "' and student_id='" + varStudentId + "'";
                    string d = objCommand.ExecuteScalar().ToString();





                    DateTime finaldate;
                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1,1";

                    string mapdatewitoutZeroAmount = Convert.ToString(objCommand.ExecuteScalar());

                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1";

                    string Nothingpaid_date1 = Convert.ToString(objCommand.ExecuteScalar());

                    if (mapdatewitoutZeroAmount == "" & Nothingpaid_date1 == "")
                    {

                        string dateapr = "2014-04-01";
                        finaldate = Convert.ToDateTime(dateapr);
                    }

                    else if (mapdatewitoutZeroAmount == "" & Nothingpaid_date1.Length > 0)
                    {

                        DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                        finaldate = Convert.ToDateTime(firstdate);
                    }

                    else
                    {
                        finaldate = Convert.ToDateTime(mapdatewitoutZeroAmount).AddMonths(1);
                    }
                    string finalmonthtoPrint = Convert.ToDateTime(finaldate).ToString("MMM");





                    varTotal = varTotal + Convert.ToInt32(dtr["AMOUNT_PAID"]);






                    ;

                } objDtReader.Close();






                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Fine";
                objHtmlTableCell1.ColSpan = 6;

                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = fine;
                objHtmlTableCell1.ColSpan = 1;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);





                //objHtmlTableRow1 = new HtmlTableRow();
                //objHtmlTableCell1 = new HtmlTableCell();
                //objHtmlTableCell1.InnerHtml = "<br/>Total";
                //objHtmlTableCell1.ColSpan = 4;

                //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                //objHtmlTableCell1 = new HtmlTableCell();
                //objHtmlTableCell1.InnerHtml = "<br/>Rs " + Convert.ToString(Totalamountpayable);
                //objHtmlTableCell1.ColSpan = 4;
                //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                //-----------------------------------------------------------------------------------------

                int inputTotalPayble = Convert.ToUInt16(varTotal);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = "IN WORDS : " + Converter.NumberToWords(inputTotalPayble) + " " + "Only";
                objHtmlTableCell1.ColSpan = 6;
                objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Amt. Payable : " + "Rs " + Totalamountpayable;
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                if (Convert.ToString(Session["mode"]) == "CASH")
                {
                    objHtmlTableCell1.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]);

                }
                else
                {
                    objHtmlTableCell1.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]) + "  " + Convert.ToString(Session["checkno"]) + "  " + Convert.ToDateTime(Session["checkdate"]).ToString("dd-MMM-yyyy") + "  " + Convert.ToString(Session["bankdetail"]);


                }

              //  objHtmlTableCell1.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"]) + "  " + Convert.ToString(Session["checkno"]) + "  " + Convert.ToString(Session["checkdate"]) + "  " + Convert.ToString(Session["bankdetail"]);
                objHtmlTableCell1.ColSpan = 6;
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Amt. Paid :" + " Rs " + Totalpayment;
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = "NOTE :";
                objHtmlTableCell1.ColSpan = 6;

                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Dues : " + "Rs " + totaldue;
                objHtmlTableCell1.ColSpan = 2;
                //objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = " 1. Fee,Charges,Funds Once paid are not refundable. ";
                objHtmlTableCell1.ColSpan = 8;
                //objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = " 2. Cheque subject to encashment";
                objHtmlTableCell1.ColSpan = 8;
                objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);






                //-----------------------------------------------------------------------------------------


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;

                //----------------------------------------------------------------------------------------------------
                objHtmlTableCell1.InnerHtml = "         " + "<br/>" + " <br/>" + "<br/>" + " <br/>------------------------------------------------------------------------------------------------------------------------- <br/>" + "<br/><br/>";
                //---------------------------------------------------------------------------------------------------------


                //objHtmlTableCell1.InnerHtml = "Received Rupees........" + Totalpayment + "<br/> Mode of Payment........" + mode + " <br/>Cheque No...................." + checkno + "<br/>Date................................" + checkdate + " <br/> Drawn on......................<br/>Branch.........................." + bankdetail + "<br/><br/>Cashier.............................";
                objHtmlTableCell1.ColSpan = 8;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                string gtot = Convert.ToString(Session["tt"]);

                #endregion

                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "center";
                objHtmlTableCell.Controls.Add(objHtmlTable1);
                objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);

                objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);
                i++;
                if (i % 2 == 0)
                {
                    Panel1.Attributes.Add("style", "PAGE-BREAK-AFTER: always");
                }
                
                Panel1.Controls.Add(objHtmlTable);

            }
        }
    }
}