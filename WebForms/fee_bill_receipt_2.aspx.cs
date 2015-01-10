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

public partial class WebForms_fee_bill_receipt_2 : System.Web.UI.Page
{
    OdbcDataAdapter objDtAdapter = null;
    DataSet objdtst = new DataSet();
    DataSet objstudent = new DataSet();

    double duefee = 0.0;
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
            feeBillId();
        }
        else
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

    public void feeBillId()
    {

        ArrayList objArrayListStudentId = new ArrayList();
        string varClassCode = Convert.ToString(Request.QueryString["class_id"]);
        string varSid = Convert.ToString(Request.QueryString["student_id"]);


        varSchoolSession = Convert.ToString(Session["_Session"]);
        objConnection = (OdbcConnection)Session["_Connection"];
        objCommand = new OdbcCommand();
        objCommand.Connection = objConnection;
        if (!IsPostBack)
        {
            #region
            HtmlTable objHtmlTable = new HtmlTable();
            //objHtmlTable.Align = "center";
            objHtmlTable.Border = 0; objHtmlTable.Width = "500px";
            HtmlTableRow objHtmlTableRow = null;
            HtmlTableCell objHtmlTableCell = null;

            HtmlTable objHtmlTable1 = null;
            HtmlTableRow objHtmlTableRow1 = null;
            HtmlTableCell objHtmlTableCell1 = null;

            HtmlTable objHtmlTable2 = null;
            HtmlTableRow objHtmlTableRow2 = null;
            HtmlTableCell objHtmlTableCell2 = null;
            #endregion

            objCommand.CommandText = "select student_id from ign_student_master where class_code = '" + varClassCode + "'";
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
                objHtmlTable1 = new HtmlTable(); objHtmlTable1.Border = 0; objHtmlTable1.Width = "70%"; objHtmlTable1.Attributes.Add("style", "font-size:14px;");
                objHtmlTable2 = new HtmlTable(); objHtmlTable2.Border = 0; objHtmlTable2.Width = "70%"; objHtmlTable2.Attributes.Add("style", "font-size:14px;");

                #region mapped Date
                objCommand.CommandText = "select max(MAPPED_DATE) from fee_collect_component_master where student_id = '" + varStudentId + "'";
                DateTime varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                #endregion

                #region Student Details
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + varStudentId + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {


                }
                objDtReader.Close();
                #endregion

                #region Fee Details

                int varTotal = 0; int Sno = 1;
               objCommand.CommandText = "select B.COMPONENT_NAME,(A.AMOUNT_PAYBLE-A.DISCOUNT) as AMOUNT_PAYBLE,MAPPED_DATE from fee_collect_component_master A, fee_component_master B where student_id = '" + varStudentId + "' and MAPPED_DATE = '" + varMappedDate.ToString("yyyy-MM-dd") + "' and A.COMPONENT_ID = B.COMPONENT_ID";
                //Response.Write(objCommand.CommandText);
                //Response.End();

                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToString(Sno) + ". " + Convert.ToString(objDtReader["COMPONENT_NAME"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(objDtReader["AMOUNT_PAYBLE"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //--------------------------------------------------------------------------//                    

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = Convert.ToString(Sno) + ". " + Convert.ToString(objDtReader["COMPONENT_NAME"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Rs " + Convert.ToString(objDtReader["AMOUNT_PAYBLE"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);
                    varTotal = varTotal + Convert.ToInt32(objDtReader["AMOUNT_PAYBLE"]);
                    Sno += 1;
                } objDtReader.Close();
                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>Total                             ";
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>Rs " + Convert.ToString(varTotal);
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.InnerHtml = "Received Rupees....................................<br/>By Cash/Cheque No..............................<br/>Date....................  Drawn on...................<br/>Branch...................................................<br/><br/>Cashier.............................";
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);
                //--------------------------------------------------------------------------//
                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>Total";
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>Rs " + Convert.ToString(varTotal);
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                objHtmlTableCell2.InnerHtml = "Received Rupees....................................<br/>By Cash/Cheque No..............................<br/>Date....................  Drawn on...................<br/>Branch...................................................<br/><br/>Cashier.............................";
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);
                #endregion

                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "left";
            
                objHtmlTableCell.Controls.Add(objHtmlTable1);
                objHtmlTableRow.Cells.Add(objHtmlTableCell);

                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "left";
                objHtmlTableCell.Controls.Add(objHtmlTable2);
                objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);

                objHtmlTableRow = new HtmlTableRow();
                objHtmlTableCell = new HtmlTableCell(); objHtmlTableCell.ColSpan = 2;
                objHtmlTableCell.InnerHtml = "<br/>";
                objHtmlTableRow.Cells.Add(objHtmlTableCell);
                objHtmlTable.Rows.Add(objHtmlTableRow);
                i++;
                if (i % 2 == 0)
                {
                    Panel1.Attributes.Add("style", "PAGE-BREAK-AFTER: always");
                }
                Panel1.Controls.Add(objHtmlTable);

            }
            //Panel1.Controls.Add(objHtmlTable);
        }
    }
    public void feeBillName()

    {
       


        ArrayList objArrayListStudentId = new ArrayList();
        string varClassCode = Convert.ToString(Request.QueryString["class_id"]);
        int varSId = Convert.ToInt32(Request.QueryString["student_id"]);
        varSchoolSession = Convert.ToString(Session["_Session"]);
        objConnection = (OdbcConnection)Session["_Connection"];
        objCommand = new OdbcCommand();
        objCommand.Connection = objConnection;


       
        DataTable dt = new DataTable();
        dt = (DataTable)(Session["Feecomponent"]);
        var TotalDiscount = Convert.ToString(Session["TotalDiscount"]);
        var Totalamountpayable = Convert.ToString(Session["Totalamountpayable"]);
        var totaldue = Convert.ToString(Session["totaldue"]);
        var A_TotalDiscount = Convert.ToString(Session["A_TotalDiscount"]);
        var Totalpayment = Convert.ToString(Session["Totalpayment"]);
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
        var calulationdate = Convert.ToString(Session["feecalculatedate"]);




        //objCommand.CommandText = "select student_id from ign_student_master where student_registration_nbr='" + admitionNO + "' ";
        //string STUID = Convert.ToString(objCommand.ExecuteScalar());

        int STUID = Convert.ToInt32(Session["student_id"]);

        ////////////   DATASET FOR LING/////////////
        objDtAdapter = new OdbcDataAdapter(new OdbcCommand("Select * from collect_component_master where student_id='" + STUID + "' ", objConnection));
        objDtAdapter.Fill(objdtst);

        var EWSSTUDENT = (from s in objdtst.Tables[0].AsEnumerable() where s["student_id"].ToString().Trim().Equals(STUID) && s["component_id"].ToString().Trim().Equals(Convert.ToString("11")) select s);


        ///////////END OF DATASET OF LING///////////////////////////////

        //objCommand.CommandText = "select distinct component_id from collect_component_master where component_id='11' and student_id='" + STUID + "' ";
        //string eswstudnt = Convert.ToString(objCommand.ExecuteScalar());



        objCommand.CommandText = "select distinct b.DISCOUNT_VALUE from student_discount_mapping a,discount_master b where a.student_id='" + STUID + "' and a.DISCOUNT_ID=b.DISCOUNT_ID";
        string discount_value = Convert.ToString(objCommand.ExecuteScalar());
        if (EWSSTUDENT.Any())
        {
            concession = "EWS";
        }
           
        
        else
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






        objCommand.CommandText = "select max(a.Rno) as rno from collect_component_detail a,ign_student_master b where a.STUDENT_ID=b.STUDENT_ID and b.STUDENT_REGISTRATION_NBR='" + admitionNO + "'";
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

            //objCommand.CommandText = "select student_id from ign_student_master where student_registration_nbr='" + admitionNO + "' ";
            //objDtReader = objCommand.ExecuteReader();
            //while (objDtReader.Read())
            //{
            //    objArrayListStudentId.Add(Convert.ToString(objDtReader["student_id"]));

            //}
            //objDtReader.Close();

            objArrayListStudentId.Add(Convert.ToString(STUID));

            int i = 0;
            foreach (string varStudentId in objArrayListStudentId)
            {
                objHtmlTableRow = new HtmlTableRow();
                objHtmlTable1 = new HtmlTable(); objHtmlTable1.Border = 0;  objHtmlTable1.Attributes.Add("style", "font-size:13px;");
                
                objHtmlTable2 = new HtmlTable(); objHtmlTable2.Border = 0;  objHtmlTable2.Attributes.Add("style", "font-size:13px;");
              
                #region mapped Date


                var varmappesdate = (from s in objdtst.Tables[0].AsEnumerable() where s["student_id"].ToString().Trim().Equals(STUID) orderby Convert.ToDateTime(s["mapped_date"]) descending select s["mapped_date"]).Take(1);



                //objCommand.CommandText = "select max(MAPPED_DATE) from collect_component_master where student_id = '" + varStudentId + "'";

                //if (!Convert.ToString(objCommand.ExecuteScalar()).Equals(""))
                //{
                //    varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                //}

                if (varmappesdate.Any())
                {
                    varMappedDate = Convert.ToDateTime(varmappesdate);
                }
                #endregion

                #region Student Details

                objstudent = (DataSet)Session["smaster"];

                var studentdetail = (from s in objstudent.Tables[0].AsEnumerable() where s["STUDENT_REGISTRATION_NBR"].ToString().Trim().ToUpper().Equals(Convert.ToString(admitionNO).Trim().ToUpper()) select s);


                //objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,A.STUDENT_ROLL_NBR,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_registration_nbr = '" + admitionNO + "'";
                //objDtReader = objCommand.ExecuteReader();
                //while (objDtReader.Read())
                //{

                if (studentdetail.Any())
                {

                    foreach (DataRow _row in studentdetail)
                    {
                        objHtmlTableRow1 = new HtmlTableRow();
                        objHtmlTableCell1 = new HtmlTableCell();

                        objHtmlTableCell1.Align = "center";
                        objHtmlTableCell1.InnerHtml = "HAPPY HOME PUBLIC SCHOOL<br/>";
                        objHtmlTableCell1.Style.Add("font-size", "14pt; border-radius: .2em;");
                        objHtmlTableCell1.ColSpan = 8;
                        objHtmlTableCell1.BgColor = ("gray");
                        objHtmlTableCell1.Style.Add("border", "solid 1px black");
                       // objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                        objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                        objHtmlTable1.Rows.Add(objHtmlTableRow1);


                        objHtmlTableRow1 = new HtmlTableRow();
                        objHtmlTableCell1 = new HtmlTableCell();
                        objHtmlTableCell1.InnerText = "B-4 SECTOR 11 ROHINI DELHI 110085";
                        objHtmlTableCell1.Style.Add("font-size", "13pt");
                        objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                        objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                        objHtmlTableCell1.BgColor=("lightgray");
                        objHtmlTableCell1.Align = "center";

                        objHtmlTableCell1.ColSpan = 8;
                        objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                        objHtmlTable1.Rows.Add(objHtmlTableRow1);


                        objHtmlTableRow1 = new HtmlTableRow();
                        objHtmlTableCell1 = new HtmlTableCell();
                        objHtmlTableCell1.InnerText = "SCHOOL COPY";
                        objHtmlTableCell1.Style.Add("font-size", "10pt");
                        objHtmlTableCell1.Align = "left";
                        objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                        objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                        objHtmlTableCell1.ColSpan = 8;
                        objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                        objHtmlTable1.Rows.Add(objHtmlTableRow1);

                        objHtmlTableRow1 = new HtmlTableRow();
                        objHtmlTableCell1 = new HtmlTableCell();
                        objHtmlTableCell1.InnerHtml = "Receipt No : HHPS " + reciptNO;
                        objHtmlTableCell1.ColSpan = 6;
                      //  objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                        objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                        objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                        objHtmlTable1.Rows.Add(objHtmlTableRow1);


                        objHtmlTableCell1 = new HtmlTableCell();
                        objHtmlTableCell1.ColSpan = 2;
                        objHtmlTableCell1.InnerHtml = "Pay DATE : " + calulationdate;
                      //  objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                        objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                        objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                        objHtmlTable1.Rows.Add(objHtmlTableRow1);



                        objHtmlTableRow1 = new HtmlTableRow();
                        objHtmlTableCell1 = new HtmlTableCell();

                        objHtmlTableCell1.InnerHtml = "Student's Name : " +Convert.ToString( _row["name"])+ "<br/><br/>";
                        objHtmlTableCell1.ColSpan = 6;
                        objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                        objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                        objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                        objHtmlTableCell1 = new HtmlTableCell();
                        objHtmlTableCell1.InnerHtml = "Adm No : " + Convert.ToString(_row["student_registration_nbr"]) + "<br/><br/>";
                        objHtmlTableCell1.ColSpan = 2;
                        objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                        objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                        objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                        objHtmlTable1.Rows.Add(objHtmlTableRow1);

                        objHtmlTableRow1 = new HtmlTableRow();
                        objHtmlTableCell1 = new HtmlTableCell();
                        objHtmlTableCell1.InnerHtml = "Father's Name : " + Convert.ToString(_row["FATHER_NAME"]) + "<br/><br/>";
                        objHtmlTableCell1.ColSpan = 6;
                        objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                        objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                        objHtmlTableCell1 = new HtmlTableCell();
                        objHtmlTableCell1.InnerHtml = "Class : " + Convert.ToString(_row["CLASS"]) + "<br/><br/>";
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
                      // objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
                        objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                        objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                        objHtmlTableCell1 = new HtmlTableCell();
                        objHtmlTableCell1.InnerText = "Roll No : " + Convert.ToString(_row["STUDENT_REGISTRATION_NBR"]);
                        objHtmlTableCell1.ColSpan = 2;
                       // objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black");
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
                        objHtmlTableCell2.Style.Add("font-size", "14pt;border-radius: .2em;");
                        objHtmlTableCell2.ColSpan = 8;
                        objHtmlTableCell2.Style.Add("border", "solid 1px black");
                      
                        objHtmlTableCell2.BgColor = ("gray");
                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTable2.Rows.Add(objHtmlTableRow2);



                        objHtmlTableRow2 = new HtmlTableRow();
                        objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "center";
                        objHtmlTableCell2.InnerHtml = "B-4 SECTOR 11 ROHINI DELHI 110085";
                        objHtmlTableCell2.Style.Add("font-size", "13pt");
                        objHtmlTableCell2.ColSpan = 8;
                        objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                        objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                        objHtmlTableCell2.BgColor = ("lightgray");
                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTable2.Rows.Add(objHtmlTableRow2);

                        objHtmlTableRow2 = new HtmlTableRow();
                        objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "LEFT";
                        objHtmlTableCell2.InnerHtml = "STUDENT COPY";
                        objHtmlTableCell2.Style.Add("font-size", "10pt");
                        objHtmlTableCell2.ColSpan = 8;
                        objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                        objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTable2.Rows.Add(objHtmlTableRow2);

                        objHtmlTableRow2 = new HtmlTableRow();
                        objHtmlTableCell2 = new HtmlTableCell();
                        objHtmlTableCell2.InnerHtml = "Receipt No : HHPS " + reciptNO;
                        objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                       
                        objHtmlTableCell2.ColSpan = 6;

                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTable2.Rows.Add(objHtmlTableRow2);

                        objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                        objHtmlTableCell2.InnerHtml = "Pay DATE : " + calulationdate;
                        objHtmlTableCell2.ColSpan = 2;
                     
                        objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTable2.Rows.Add(objHtmlTableRow2);




                        //objHtmlTableRow2 = new HtmlTableRow();
                        //objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                        ////objHtmlTableCell2.InnerHtml = "Note: Fees Once paid will not be refunded in any case. Deposit the fee before 15th of each otherwise Fine Rs 10 per day will be charged.<br/><br/>";
                        //objHtmlTableCell2.ColSpan = 8;
                        //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        //objHtmlTable2.Rows.Add(objHtmlTableRow2);


                        objHtmlTableRow2 = new HtmlTableRow();
                        objHtmlTableCell2 = new HtmlTableCell();
                        objHtmlTableCell2.InnerHtml = "Student's Name : " + Convert.ToString(_row["name"]) + "<br/><br/>";
                        objHtmlTableCell2.ColSpan = 6;
                        objHtmlTableCell2.Style.Add("border-top", "solid 1px black");
                        objHtmlTableCell2.Style.Add("border-left", "solid 1px black");


                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTableCell2 = new HtmlTableCell();
                        objHtmlTableCell2.InnerHtml = "Adm No : " + Convert.ToString(_row["student_registration_nbr"]) + "<br/><br/>";
                        objHtmlTableCell2.ColSpan = 2;
                        objHtmlTableCell2.Style.Add("border-top", "solid 1px black");
                        objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTable2.Rows.Add(objHtmlTableRow2);

                        objHtmlTableRow2 = new HtmlTableRow();
                        objHtmlTableCell2 = new HtmlTableCell();
                        objHtmlTableCell2.InnerHtml = "Father's Name : " + Convert.ToString(_row["father_name"]) + "<br/><br/>";
                        objHtmlTableCell2.ColSpan = 6;

                        objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                        objHtmlTableCell2 = new HtmlTableCell();
                        objHtmlTableCell2.InnerHtml = "Class : " + Convert.ToString(_row["CLASS"]) + "<br/><br/>";
                        objHtmlTableCell2.ColSpan = 2;
                        objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTable2.Rows.Add(objHtmlTableRow2);



                        objHtmlTableRow2 = new HtmlTableRow();
                        objHtmlTableCell2 = new HtmlTableCell();

                        if (concession == "N/A" || concession == "EWS")
                        {
                            objHtmlTableCell2.InnerHtml = "Concession :" + concession;

                        }
                        else
                        {
                            objHtmlTableCell2.InnerHtml = "Concession :" + concession + "%";

                        }



                        objHtmlTableCell2.ColSpan = 6;
                       // objHtmlTableCell2.Style.Add("border-bottom", "solid 1px black");
                        objHtmlTableCell2.Style.Add("border-left", "solid 1px black");


                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTableCell2 = new HtmlTableCell();
                        objHtmlTableCell2.InnerHtml = "Roll No : " + Convert.ToString(_row["student_registration_nbr"]);
                        objHtmlTableCell2.ColSpan = 2;
                        //objHtmlTableCell2.Style.Add("border-Bottom", "solid 1px black");
                        objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                        objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                        objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    }
                }

                objDtReader.Close();
                #endregion

                objHtmlTableRow1 = new HtmlTableRow();
                 
               
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Component" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.Style.Add("border", "solid 1px black; border-radius: .2em;");
                objHtmlTableCell1.BgColor = ("lightgray");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Duration" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.Style.Add("border", "solid 1px black;border-radius: .2em;");
                objHtmlTableCell1.BgColor = ("lightgray");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Amount" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt;border-radius: .2em;");
                objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.Style.Add("border", "solid 1px black");
                objHtmlTableCell1.BgColor = ("lightgray");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                

                 

                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>" + "Payment" + "<br/>";
                objHtmlTableCell1.Style.Add("font-size", "13pt; border-radius: .2em;");
                objHtmlTableCell1.ColSpan = 1;
                objHtmlTableCell1.Style.Add("border", "solid 1px black");
                objHtmlTableCell1.BgColor = ("lightgray");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                //-------------------------------------------------------


                objHtmlTableRow2 = new HtmlTableRow();
                 


                
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>" + "Component";
                objHtmlTableCell2.Style.Add("font-size", "13pt");
                objHtmlTableCell2.ColSpan = 2;
                objHtmlTableCell2.Style.Add("border", "solid 1px black;border-radius: .2em;");
                objHtmlTableCell2.BgColor = ("lightgray");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>" + "Duration";
                objHtmlTableCell2.Style.Add("font-size", "13pt;border-radius: .2em;");
                objHtmlTableCell2.ColSpan = 2;
                objHtmlTableCell2.Style.Add("border", "solid 1px black");
                objHtmlTableCell2.BgColor = ("lightgray");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>" + "Amount";
                objHtmlTableCell2.Style.Add("font-size", "13pt;border-radius: .2em;");
                objHtmlTableCell2.ColSpan = 2;
                objHtmlTableCell2.Style.Add("border", "solid 1px black");
                objHtmlTableCell2.BgColor = ("lightgray");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                
                

                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "<br/>" + "Payment";
                objHtmlTableCell2.Style.Add("font-size", "13pt;border-radius: .2em;");
                objHtmlTableCell2.ColSpan = 1;
                objHtmlTableCell2.Style.Add("border", "solid 1px black");
                objHtmlTableCell2.BgColor = ("lightgray");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                #region Fee Details

                DataTableReader dtr;
                dtr = dt.CreateDataReader();
              

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
                    objHtmlTableCell1.Style.Add("border", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    //-----------------------------------------------------------------------------------------

                    /////////kkkkkkkkkkkkkk////////////////////

                    objCommand.CommandText = "SELECT COMPONENT_ID FROM COMPONENT_MASTER WHERE COMPONENT_NAME='" + Convert.ToString(dtr["Component"]) + "'";
                    var component_id1 = objCommand.ExecuteScalar();

                    ////////////fee claculation 

                    var  feecalulatationpaid1 = (from s in objdtst.Tables[0].AsEnumerable() where s["student_id"].ToString().Trim().Equals(Convert.ToString(varStudentId)) && s["component_id"].ToString().Trim().Equals(Convert.ToString(component_id1)) && Convert.ToDateTime(s["mapped_date"]) >= Convert.ToDateTime("2014-04-01") && Convert.ToDateTime(s["mapped_date"]) <= Convert.ToDateTime((firstdate)) select Convert.ToDouble(s["AMOUNT_PAID"])).Sum();


                    var feecalulatationpayble2 = (from s in objdtst.Tables[0].AsEnumerable() where s["student_id"].ToString().Trim().Equals(Convert.ToString(varStudentId)) && s["component_id"].ToString().Trim().Equals(Convert.ToString(component_id1)) && Convert.ToDateTime(s["mapped_date"]) >= Convert.ToDateTime("2014-04-01") && Convert.ToDateTime(s["mapped_date"]) <= Convert.ToDateTime((firstdate)) select Convert.ToDouble(s["AMOUNT_PAYBLE"])).Sum();

                    //var feecalulatationpaybleforminus = (from s in objdtst.Tables[0].AsEnumerable() where s["student_id"].ToString().Trim().Equals(Convert.ToString(varStudentId)) && s["component_id"].ToString().Trim().Equals(Convert.ToString(component_id1)) && Convert.ToDateTime(s["mapped_date"]) < Convert.ToDateTime((ddlStartMonth.SelectedValue)) select Convert.ToDouble(s["AMOUNT_PAYBLE"])).Sum();

                    //var feecalulatatiopayable2 = (from s in objdtst.Tables[0].AsEnumerable() where s["student_id"].ToString().Trim().Equals(Convert.ToString(varStudentId)) && s["component_id"].ToString().Trim().Equals(Convert.ToString(component_id1)) && Convert.ToDateTime(s["mapped_date"]) >= Convert.ToDateTime((ddlStartMonth.SelectedValue)) && Convert.ToDateTime(s["MAPPED_DATE"]) <= Convert.ToDateTime((ddlEndMonth.SelectedValue)) select Convert.ToDouble(s["amount_payble"])).Sum();

                    //var feecalulatatiodiscount3 = (from s in objdtst.Tables[0].AsEnumerable() where s["student_id"].ToString().Trim().Equals(Convert.ToString(varStudentId)) && s["component_id"].ToString().Trim().Equals(Convert.ToString(component_id1)) && Convert.ToDateTime(s["mapped_date"]) >= Convert.ToDateTime((ddlStartMonth.SelectedValue)) && Convert.ToDateTime(s["MAPPED_DATE"]) <= Convert.ToDateTime((ddlEndMonth.SelectedValue)) select Convert.ToDouble(s["discount"])).Sum();

                    //var feecalulatatiopaidtotal4 = (from s in objdtst.Tables[0].AsEnumerable() where Convert.ToString(s["STUDENT_ID"]).Equals(Convert.ToString(varStudentId)) && Convert.ToString(s["COMPONENT_ID"]).Equals(Convert.ToString(component_id1)) select Convert.ToInt32(s["AMOUNT_PAID"])).Sum();

                    //var feecalulatationpayabletoatl5 = (from s in objdtst.Tables[0].AsEnumerable() where s["student_id"].ToString().Trim().Equals(Convert.ToString(varStudentId)) && s["component_id"].ToString().Trim().Equals(Convert.ToString(component_id1)) && Convert.ToDateTime(s["mapped_date"]) <= Convert.ToDateTime((ddlEndMonth.SelectedValue)) select Convert.ToDouble(s["AMOUNT_PAYBLE"])).Sum();

                   //////////fee calculation///////////////////

                    duefee = Convert.ToDouble(feecalulatationpayble2) - Convert.ToDouble(feecalulatationpaid1);
                    ///////////  nouse////////////
                    objCommand.CommandText = "select sum( a.AMOUNT_PAYBLE) - sum( a.amount_paid)  from collect_component_master a where a.COMPONENT_ID='" + component_id1 + "' and a.MAPPED_DATE between '2014-04-01' and '" + Convert.ToDateTime(firstdate).ToString("yyyy-MM-dd") + "' and student_id='" + varStudentId + "'";
                    string d1 = objCommand.ExecuteScalar().ToString();
                    ///////////nouse////////////


                    DateTime finaldate1;
                    string mapdatewitoutZeroAmount1 = "";
                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1,1";

                     mapdatewitoutZeroAmount1 = Convert.ToString(objCommand.ExecuteScalar());





                    //objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1";

                    //string Nothingpaid_date = Convert.ToString(objCommand.ExecuteScalar());
                     var Nothingpaid_date = "";
                     var nopaid_date = (from s in objdtst.Tables[0].AsEnumerable() where s["STUDENT_ID"].ToString().Trim().Equals(Convert.ToString(varStudentId)) && s["component_id"].ToString().Trim().Equals(Convert.ToString(component_id1)) && s["AMOUNT_PAID"].ToString() != (Convert.ToString("0")) orderby Convert.ToDateTime(s["MAPPED_DATE"]) descending select s["MAPPED_DATE"]).Take(1);

                     foreach (DateTime date in nopaid_date)
                     {
                           Nothingpaid_date = date.ToString();
                     }

                    //objCommand.CommandText = "select distinct a.paid_date from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1";
                    //string calu = Convert.ToString(objCommand.ExecuteScalar());
                     var calu = "";
                    var calulllll = (from s in objdtst.Tables[0].AsEnumerable() where s["STUDENT_ID"].ToString().Trim().Equals(Convert.ToString(varStudentId)) && s["component_id"].ToString().Trim().Equals(Convert.ToString(component_id1)) && s["AMOUNT_PAID"].ToString() != (Convert.ToString("0")) orderby Convert.ToDateTime(s["MAPPED_DATE"]) descending select s["paid_date"]).Take(1);
                    foreach (DateTime date in calulllll)
                    {
                          calu = date.ToString();
                    }

                    DateTime calu2 = Convert.ToDateTime(calulationdate);

                    if (mapdatewitoutZeroAmount1 == "" & Convert.ToString(Nothingpaid_date) == "")
                    {
                        string dateapr="2014-04-01";
                        finaldate1 = Convert.ToDateTime(dateapr);
                    }

                    else if (mapdatewitoutZeroAmount1 == "" & Convert.ToString(Nothingpaid_date).Length > 0)
                    {
                        if (calu2.ToString() == calu.ToString())
                        {
                            string dateapr = "2014-04-01";
                            finaldate1 = Convert.ToDateTime(dateapr);
                        }

                        else
                        {
                            DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                            finaldate1 = Convert.ToDateTime(dateforonepay);
                        }
                       
                    }
                    else if (mapdatewitoutZeroAmount1.Length > 0 & Convert.ToString(Nothingpaid_date).Length > 0)
                    {
                        if (calu2.ToString() == calu.ToString())
                        {
                            string dateapr = "2014-04-01";
                            finaldate1 = Convert.ToDateTime(firstdate);
                        }

                        else
                        {
                            DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                            finaldate1 = Convert.ToDateTime(dateforonepay);
                        }
                    }
                    else
                    {
                        finaldate1 = Convert.ToDateTime(mapdatewitoutZeroAmount1).AddMonths(1);
                    }

                    string finalmonthtoPrint1 = Convert.ToDateTime(finaldate1).ToString("MMM");

                     

                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = finalmonthtoPrint1 + " - " + Convert.ToDateTime(lastdate).ToString("MMM");
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);


                    //------------------------------------kkk------------------------------------------------------


                    objHtmlTableCell1 = new HtmlTableCell();
                    if (Convert.ToString(dtr["Discount"]).Equals(0))
                    {
                        objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(dtr["Amount"]); 
                    }
                    else
                    {
                        objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(dtr["Payment"]); 
                    }


                    
                    objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.Style.Add("border", "solid 1px black");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);

                    
                   
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToString(dtr["Payment"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1.ColSpan = 1;
                    objHtmlTableCell1.Style.Add("border", "solid 1px black");
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
                    objHtmlTableCell2.Style.Add("border", "solid 1px black");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                    //---------------------------------------reciptduemonth--------------------------------------------------

                    objCommand.CommandText = "SELECT COMPONENT_ID FROM COMPONENT_MASTER WHERE COMPONENT_NAME='"+Convert.ToString(dtr["Component"])+"'";
                    var component_id = objCommand.ExecuteScalar();
                    objCommand.CommandText = "select sum( a.AMOUNT_PAYBLE) - sum( a.amount_paid)  from collect_component_master a where a.COMPONENT_ID='" + component_id + "' and a.MAPPED_DATE between '2014-04-01' and '" + Convert.ToDateTime(firstdate).ToString("yyyy-MM-dd") + "' and student_id='" + varStudentId + "'";
                    string d = objCommand.ExecuteScalar().ToString();
 




                    DateTime finaldate;
                    objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1,1";

                    string mapdatewitoutZeroAmount = Convert.ToString(objCommand.ExecuteScalar());

                   // objCommand.CommandText = "select distinct a.MAPPED_DATE from collect_component_master a  where a.AMOUNT_PAID !='0' and a.COMPONENT_ID='" + component_id1 + "'   and a.STUDENT_ID='" + varStudentId + "' order by a.MAPPED_DATE desc limit 1";

                   // string Nothingpaid_date1 = Convert.ToString(objCommand.ExecuteScalar());

                    var Nothingpaid_date1 = "";
                    var nopaid_date1 = (from s in objdtst.Tables[0].AsEnumerable() where s["STUDENT_ID"].ToString().Trim().Equals(Convert.ToString(varStudentId)) && s["component_id"].ToString().Trim().Equals(Convert.ToString(component_id1)) && s["AMOUNT_PAID"].ToString() != (Convert.ToString("0")) orderby Convert.ToDateTime(s["MAPPED_DATE"]) descending select s["MAPPED_DATE"]).Take(1);

                    foreach (DateTime date in nopaid_date1)
                    {
                        Nothingpaid_date1 = date.ToString();
                    }

                    if (mapdatewitoutZeroAmount == "" & Nothingpaid_date1 == "")
                    {
                        
                        string dateapr = "2014-04-01";
                        finaldate = Convert.ToDateTime(dateapr);
                    }

                    else if (mapdatewitoutZeroAmount == "" & Nothingpaid_date1.Length > 0)
                    {
                        if (calu.ToString() == calu2.ToString())
                        {

                            string dateapr = "2014-04-01";
                            finaldate = Convert.ToDateTime(dateapr);
                        }

                        else
                        {
                            DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                            finaldate = Convert.ToDateTime(firstdate);
                        }
                    }
                    else if (mapdatewitoutZeroAmount.Length > 0 & Nothingpaid_date1.Length > 0)
                    {
                        if (calu.ToString() == calu2.ToString())
                        {

                            string dateapr = "2014-04-01";
                            finaldate = Convert.ToDateTime(firstdate);
                        }

                        else
                        {
                            DateTime dateforonepay = Convert.ToDateTime(Nothingpaid_date).AddMonths(1);
                            finaldate = Convert.ToDateTime(firstdate);
                        }
                    }

                    else
                    {
                        finaldate = Convert.ToDateTime(mapdatewitoutZeroAmount).AddMonths(1);
                    }
                    string finalmonthtoPrint = Convert.ToDateTime(finaldate).ToString("MMM");
 
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = finalmonthtoPrint + " - " + Convert.ToDateTime(lastdate).ToString("MMM");
                    objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.Style.Add("border", "solid 1px black");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

 
                    objHtmlTableCell2 = new HtmlTableCell();
                    if (Convert.ToString(dtr["Discount"]).Equals(0))
                    { 
                        objHtmlTableCell2.InnerHtml = "Rs " + Convert.ToString(dtr["Amount"]); 
                    }
                    else
                    {
                        objHtmlTableCell2.InnerHtml = "Rs " + Convert.ToString(dtr["Payment"]); 
                    }

                    // objHtmlTableCell2.InnerHtml = "Rs " + Convert.ToString(dtr["Amount"]);
                    objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.Style.Add("border", "solid 1px black");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    varTotal = varTotal + Convert.ToInt32(dtr["Amount"]);

                    

                    

                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = Convert.ToString(dtr["Payment"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2.Style.Add("border", "solid 1px black");
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    ;

                } objDtReader.Close();


              
               


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Fine";
                objHtmlTableCell1.Style.Add("border", "solid 1px black");
                objHtmlTableCell1.ColSpan = 6;

                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = fine;
                objHtmlTableCell1.ColSpan = 1;
                objHtmlTableCell1.Style.Add("border", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);



               
 
                int inputTotalPayble = Convert.ToUInt16(varTotal);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerText = "IN WORDS : " + Converter.NumberToWords(inputTotalPayble) + " " + "Only";
                objHtmlTableCell1.ColSpan = 6;
                objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                int p;

                Int32.TryParse(Convert.ToString(Session["Totalpayment"]), out p);

                int f;
                Int32.TryParse(Convert.ToString(Session["fine"]), out f);


                int totalpayablewithfine = p + f;

                Session["payablwithfine"] = totalpayablewithfine;




                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Amt. Payable : " + "Rs " + Convert.ToString(totalpayablewithfine);
                objHtmlTableCell1.ColSpan = 2;
                //objHtmlTableCell1.Style.Add("border-top", "solid 1px black");
                //objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableCell1.Style.Add("border", "solid 1px black");
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


                int totalwithoutfine = 0;

                Int32.TryParse(Convert.ToString(Session["Totalpayment"]), out totalwithoutfine);

                int totalfine = 0;

                Int32.TryParse(fine, out totalfine);

                int totalwithfine = totalwithoutfine + totalfine;


                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "Amt. Paid :" + " Rs " + totalwithfine;
                objHtmlTableCell1.ColSpan = 2;
                //objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableCell1.Style.Add("border", "solid 1px black");
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
                objHtmlTableCell1.InnerHtml = "Dues : " +"Rs "+Convert.ToString(Session["totaldue"]);
                objHtmlTableCell1.ColSpan = 2;
                //objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                //objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableCell1.Style.Add("border", "solid 1px black");
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
                objHtmlTableCell1.Style.Add("border-bottom", "solid 1px black;");
                objHtmlTableCell1.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell1.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

               




                //-----------------------------------------------------------------------------------------


                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;

                //----------------------------------------------------------------------------------------------------
                objHtmlTableCell1.InnerHtml = "         " + "<br/>" +  " <br/>" +  "<br/>" + " <br/>------------------------------------------------------------------------------------------------------------------------- <br/>" +"<br/><br/>";
                //---------------------------------------------------------------------------------------------------------


                objHtmlTableCell1.ColSpan = 8;
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);
               
                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "Fine";
                objHtmlTableCell2.Style.Add("border", "solid 1px black");
                objHtmlTableCell2.ColSpan = 6;

                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = fine;
                objHtmlTableCell2.Style.Add("border", "solid 1px black");
                objHtmlTableCell2.ColSpan = 1;
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);


              

 
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
                objHtmlTableCell2.InnerHtml = "Amt. Payable : " + "Rs " + Convert.ToString(totalpayablewithfine);
                objHtmlTableCell2.ColSpan = 2;
                //objHtmlTableCell2.Style.Add("border-top", "solid 1px black");
                //objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                objHtmlTableCell2.Style.Add("border", "solid 1px black");

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
                    objHtmlTableCell2.InnerText = "PAY MODE : " + Convert.ToString(Session["mode"] + "  " + Convert.ToString(Session["checkno"]) + "  " + Convert.ToDateTime(Session["checkdate"]).ToString("dd-MMM-yyyy") + "  " + Convert.ToString(Session["bankdetail"]));
                }
                objHtmlTableCell2.ColSpan = 6;
               
                objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerHtml = "Amt. Paid :" + " Rs "+ totalwithfine;
                objHtmlTableCell2.ColSpan = 2;
                objHtmlTableCell2.Style.Add("border", "solid 1px black");
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
                objHtmlTableCell2.InnerHtml = "Dues : " + "Rs " + Convert.ToString(Session["totaldue"]);
                objHtmlTableCell2.ColSpan = 2;
                
                objHtmlTableCell2.Style.Add("border", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);


                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerText = "1. Fee,Charges,Funds Once paid are not refundable. ";
                objHtmlTableCell2.ColSpan = 8;
                objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);




                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell();
                objHtmlTableCell2.InnerText = "2. Cheque subject to encashment ";
                objHtmlTableCell2.ColSpan = 8;
                objHtmlTableCell2.Style.Add("border-bottom", "solid 1px black;");
                objHtmlTableCell2.Style.Add("border-left", "solid 1px black");
                objHtmlTableCell2.Style.Add("border-right", "solid 1px black");
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                //-----------------------------------------------------------------------------------
                objHtmlTableRow2 = new HtmlTableRow();
                objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
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
                objHtmlTable2.CellSpacing = 0;
                objHtmlTable1.CellSpacing = 0;
               
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

                if (Convert.ToString(Session["radio"]) == "PRINT")
                {

                    Response.Write("<script language='javascript' type='text/javascript' > window.print(); </script>");
                }
            }
        }
    }

    public void getduedetailo()
    {
        

    }
}