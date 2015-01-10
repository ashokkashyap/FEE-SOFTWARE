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

public partial class fee_bill_class_print : System.Web.UI.Page
{
    OdbcConnection objConnection; OdbcCommand objCommand; OdbcDataReader objDtReader;
    string varSchoolSession;
    protected void Page_Load(object sender, EventArgs e)
    {
        string varStatus = Session["varstat"].ToString();
        string varClassId = Request.QueryString["class_id"];
        int varSId = Convert.ToInt32(Request.QueryString["student_id"]);
        
        if (varStatus.ToUpper().Equals("Y"))
            {
            if(varSId==-1)
            {

                //feeBillId();
            }
        else
        {
            feeBillNameall();
           
        }
        }
    
            else if (varStatus.Equals(""))
            {
            if (varSId == -1)
            {
               // feeBillId();
            }
            else
            {
                feeBillNameall();
               // feeBillName();
            }
        }

    }
  
    public void feeBillIdFull()
    {
        ArrayList objArrayListStudentId = new ArrayList();
        string varClassCode = Convert.ToString(Request.QueryString["class_id"]);
        string varSid = Convert.ToString(Request.QueryString["student_id"]);


        varSchoolSession = Convert.ToString(Session["_Session"]);
        objConnection = (OdbcConnection)Session["_Connection"];
        objCommand = new OdbcCommand();
        objCommand.Connection = objConnection;
        DateTime month = Convert.ToDateTime(Session["month"]);
        if (!IsPostBack)
        {
            #region
            HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 0; objHtmlTable.Width = "800px";
            HtmlTableRow objHtmlTableRow = null;
            HtmlTableCell objHtmlTableCell = null;

            HtmlTable objHtmlTable1 = null;
            HtmlTableRow objHtmlTableRow1 = null;
            HtmlTableCell objHtmlTableCell1 = null;

            HtmlTable objHtmlTable2 = null;
            HtmlTableRow objHtmlTableRow2 = null;
            HtmlTableCell objHtmlTableCell2 = null;

            HtmlTable objHtmlTable3 = null;
            HtmlTableRow objHtmlTableRow3 = null;
            HtmlTableCell objHtmlTableCell3 = null;
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
                objHtmlTable1 = new HtmlTable(); objHtmlTable1.Border = 0; objHtmlTable1.Width = "70%"; objHtmlTable1.Attributes.Add("style", "font-size:12px;");
                objHtmlTable2 = new HtmlTable(); objHtmlTable2.Border = 0; objHtmlTable2.Width = "70%"; objHtmlTable2.Attributes.Add("style", "font-size:12px;");

                #region mapped Date
                objCommand.CommandText = "select max(MAPPED_DATE) from collect_component_master where student_id = '" + varStudentId + "'";
                DateTime varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                #endregion
                ///// ashok//////////////
                # region Student All Detail
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,a.STUDENT_ROLL_NBR,a.DATE_OF_ADMISSION,a.GENDER,a.ADDRESS_LINE1,a.CITY,a.STATE,a.FATHER_NAME,a.FATHER_OCCUPATION,a.MOTHER_NAME,a.MOTHER_OCCUPATION,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + varStudentId + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Fee Bill";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "School Copy";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2; objHtmlTableCell1.Align = "center";
                    objHtmlTableCell1.InnerHtml = "Guru Nanak Public School<br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "Note: Fees Once paid will not be refunded in any case. Deposit the fee before 15th of each otherwise Fine Rs 10 per day will be charged.<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //new
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);
                    //new ends


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = month + " To " + month.AddMonths(2).ToString("MMMM yyyy");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    ////ashok//////
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Father's Occupation : " + Convert.ToString(objDtReader["father_OCCUPATION"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Mother's Name: " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "mother's Occupation : " + Convert.ToString(objDtReader["mother_OCCUPATION"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Mother's Name: " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Addresh : " + Convert.ToString(objDtReader["ADDRESS_LINE1"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "City: " + Convert.ToString(objDtReader["city"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "State : " + Convert.ToString(objDtReader["state"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);


                    /////end ashok/////////
                    //--------------------------------------------------------------------------//
                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Fee Bill";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Student Copy";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "center";
                    objHtmlTableCell2.InnerHtml = "Guru Nanak Public School<br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = "Note: Fees Once paid will not be refunded in any case. Deposit the fee before 15th of each otherwise Fine Rs 10 per day will be charged.<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    //new
                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);
                    //new ends

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = month.ToString("MMMM yyyy") + " To " + month.AddMonths(2).ToString("MMMM yyyy");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);


                    ////ashok//////
                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Father's Occupation : " + Convert.ToString(objDtReader["father_OCCUPATION"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Mother's Name: " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "mother's Occupation : " + Convert.ToString(objDtReader["mother_OCCUPATION"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Mother's Name: " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Addresh : " + Convert.ToString(objDtReader["ADDRESS_LINE1"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "City: " + Convert.ToString(objDtReader["city"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);


                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "State : " + Convert.ToString(objDtReader["state"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);

                     /////end ashok/////////




                }
                objDtReader.Close();
                # endregion
                ///////////////
                #region Student Details
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + varStudentId + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Fee Bill";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "School Copy";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2; objHtmlTableCell1.Align = "center";
                    objHtmlTableCell1.InnerHtml = "Guru Nanak Public School<br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "Note: Fees Once paid will not be refunded in any case. Deposit the fee before 15th of each otherwise Fine Rs 10 per day will be charged.<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //new
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);
                    //new ends


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = varMappedDate.ToString("MMMM yyyy") + " To " + varMappedDate.AddMonths(2).ToString("MMMM yyyy");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);
                    //--------------------------------------------------------------------------//
                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Fee Bill";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Student Copy";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "center";
                    objHtmlTableCell2.InnerHtml = "Guru Nanak Public School<br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = "Note: Fees Once paid will not be refunded in any case. Deposit the fee before 15th of each otherwise Fine Rs 10 per day will be charged.<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    //new
                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);
                    //new ends

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = varMappedDate.ToString("MMMM yyyy") + " To " + varMappedDate.AddMonths(2).ToString("MMMM yyyy");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);
                }
                objDtReader.Close();
                #endregion

                #region Fee Details
                int varTotal = 0; int Sno = 1;
                objCommand.CommandText = "select B.COMPONENT_NAME,(A.AMOUNT_PAYBLE-A.DISCOUNT) as AMOUNT_PAYBLE,MAPPED_DATE from collect_component_master A, component_master B where student_id = '" + varStudentId + "' and MAPPED_DATE = '" + varMappedDate.ToString("yyyy-MM-dd") + "' and A.COMPONENT_ID = B.COMPONENT_ID";
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
                objHtmlTableCell1.InnerHtml = "<br/>Total";
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
        if (!IsPostBack)
        {
            #region
            HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 0; objHtmlTable.Width = "1000px";
            HtmlTableRow objHtmlTableRow = null;
            HtmlTableCell objHtmlTableCell = null;

            HtmlTable objHtmlTable1 = null;
            HtmlTableRow objHtmlTableRow1 = null;
            HtmlTableCell objHtmlTableCell1 = null;

            HtmlTable objHtmlTable2 = null;
            HtmlTableRow objHtmlTableRow2 = null;
            HtmlTableCell objHtmlTableCell2 = null;

            HtmlTable objHtmlTable3 = null;
            HtmlTableRow objHtmlTableRow3 = null;
            HtmlTableCell objHtmlTableCell3 = null;
            #endregion

            objCommand.CommandText = "select student_id from ign_student_master where class_code = '" + varClassCode + "' and student_id='" + varSId + "'";
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
                objHtmlTable1 = new HtmlTable(); objHtmlTable1.Border = 0; objHtmlTable1.Width = "80%"; objHtmlTable1.Attributes.Add("style", "font-size:12px;");
                objHtmlTable2 = new HtmlTable(); objHtmlTable2.Border = 0; objHtmlTable2.Width = "80%"; objHtmlTable2.Attributes.Add("style", "font-size:12px;");
                objHtmlTable3 = new HtmlTable(); objHtmlTable3.Border = 0; objHtmlTable3.Width = "80%"; objHtmlTable3.Attributes.Add("style", "font-size:12px;");

                #region mapped Date
                objCommand.CommandText = "select max(MAPPED_DATE) from collect_component_master where student_id = '" + varStudentId + "'";
                DateTime varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                #endregion

                #region Student Details
              //  objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + varStudentId + "'";
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,a.STUDENT_ROLL_NBR,a.DATE_OF_ADMISSION,a.GENDER,a.ADDRESS_LINE1,a.CITY,a.STATE,a.FATHER_NAME,a.FATHER_OCCUPATION,a.MOTHER_NAME,a.MOTHER_OCCUPATION,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + varStudentId + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "Fee Bill";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "School Copy";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2; objHtmlTableCell1.Align = "center";
                    //objHtmlTableCell1.InnerHtml = "Guru Nanak Public School<br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "Note: Fees Once paid will not be refunded in any case. Deposit the fee before 15th of each otherwise Fine Rs 10 per day will be charged.<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //new
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);
                    //new ends

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = Session["sdate"] + "-2014  " + " To " + Session["edate"] + "-2014";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);



                    //////ashok//////
                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "Father's Occupation : " + Convert.ToString(objDtReader["father_OCCUPATION"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "Mother's Name: " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "mother's Occupation : " + Convert.ToString(objDtReader["mother_OCCUPATION"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "Mother's Name: " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "Addresh : " + Convert.ToString(objDtReader["ADDRESS_LINE1"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "City: " + Convert.ToString(objDtReader["city"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "State : " + Convert.ToString(objDtReader["state"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //--------------------------------------------------------------------------//
                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Fee Bill";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Student Copy";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "center";
                    objHtmlTableCell2.InnerHtml = "Guru Nanak Public School<br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = "Note: Fees Once paid will not be refunded in any case. Deposit the fee before 15th of each otherwise Fine Rs 10 per day will be charged.<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    //new
                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);
                    //new ends

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = Session["sdate"] + "-2014  " + " To " + Session["edate"] + "-2014";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    ////////==============================


                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Fee Bill";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Bank Copy";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2; objHtmlTableCell3.Align = "center";
                    objHtmlTableCell3.InnerHtml = "Guru Nanak Public School<br/>";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2;
                    objHtmlTableCell3.InnerHtml = "Note: Fees Once paid will not be refunded in any case. Deposit the fee before 15th of each otherwise Fine Rs 10 per day will be charged.<br/><br/>";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2;
                    objHtmlTableCell3.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2;
                    objHtmlTableCell3.InnerHtml = Session["sdate"] + "-2014  " + " To " + Session["edate"] + "-2014";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    //objHtmlTableRow3 = new HtmlTableRow();
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "mother's Occupation : " + Convert.ToString(objDtReader["mother_OCCUPATION"]) + "<br/><br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "Mother's Name: " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    //objHtmlTableRow3 = new HtmlTableRow();
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "Addresh : " + Convert.ToString(objDtReader["ADDRESS_LINE1"]) + "<br/><br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "City: " + Convert.ToString(objDtReader["city"]) + "<br/><br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTable3.Rows.Add(objHtmlTableRow3);


                    //objHtmlTableRow3 = new HtmlTableRow();
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "State : " + Convert.ToString(objDtReader["state"]) + "<br/><br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);

                }
                objDtReader.Close();
                #endregion

                #region Fee Details
                int varTotal = 0;
                objCommand.CommandText = "select B.COMPONENT_NAME,(A.AMOUNT_PAYBLE-A.DISCOUNT) as AMOUNT_PAYBLE,MAPPED_DATE from collect_component_master A, component_master B where student_id = '" + varStudentId + "' and MAPPED_DATE = '" + varMappedDate.ToString("yyyy-MM-dd") + "' and A.COMPONENT_ID = B.COMPONENT_ID";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToString(objDtReader["COMPONENT_NAME"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(objDtReader["AMOUNT_PAYBLE"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //--------------------------------------------------------------------------//                    

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = Convert.ToString(objDtReader["COMPONENT_NAME"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Rs " + Convert.ToString(objDtReader["AMOUNT_PAYBLE"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);
                    varTotal = varTotal + Convert.ToInt32(objDtReader["AMOUNT_PAYBLE"]);

                    ///////////////////

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = Convert.ToString(objDtReader["COMPONENT_NAME"]);
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Rs " + Convert.ToString(objDtReader["AMOUNT_PAYBLE"]);
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);
                    varTotal = varTotal + Convert.ToInt32(objDtReader["AMOUNT_PAYBLE"]);

                } objDtReader.Close();
                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>Total";
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

                ////////////////
                objHtmlTableRow3 = new HtmlTableRow();
                objHtmlTableCell3 = new HtmlTableCell();
                objHtmlTableCell3.InnerHtml = "<br/>Total";
                objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                objHtmlTableCell3 = new HtmlTableCell();
                objHtmlTableCell3.InnerHtml = "<br/>Rs " + Convert.ToString(varTotal);
                objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                objHtmlTable3.Rows.Add(objHtmlTableRow3);

                objHtmlTableRow3 = new HtmlTableRow();
                objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2;
                objHtmlTableCell3.InnerHtml = "Received Rupees....................................<br/>By Cash/Cheque No..............................<br/>Date....................  Drawn on...................<br/>Branch...................................................<br/><br/>Cashier.............................";
                objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                objHtmlTable3.Rows.Add(objHtmlTableRow3);
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

                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "left";
                objHtmlTableCell.Controls.Add(objHtmlTable3);
                objHtmlTableRow.Cells.Add(objHtmlTableCell);

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

    public void feeBillNameall()
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
            #region
            HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 0; objHtmlTable.Width = "1000px";
            HtmlTableRow objHtmlTableRow = null;
            HtmlTableCell objHtmlTableCell = null;

            HtmlTable objHtmlTable1 = null;
            HtmlTableRow objHtmlTableRow1 = null;
            HtmlTableCell objHtmlTableCell1 = null;

            HtmlTable objHtmlTable2 = null;
            HtmlTableRow objHtmlTableRow2 = null;
            HtmlTableCell objHtmlTableCell2 = null;

            HtmlTable objHtmlTable3 = null;
            HtmlTableRow objHtmlTableRow3 = null;
            HtmlTableCell objHtmlTableCell3 = null;
            #endregion

            objCommand.CommandText = "select student_id from ign_student_master where class_code = '" + varClassCode + "' and student_id='" + varSId + "'";
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
                objHtmlTable1 = new HtmlTable(); objHtmlTable1.Border = 0; objHtmlTable1.Width = "70%"; objHtmlTable1.Attributes.Add("style", "font-size:12px;");
                objHtmlTable2 = new HtmlTable(); objHtmlTable2.Border = 0; objHtmlTable2.Width = "70%"; objHtmlTable2.Attributes.Add("style", "font-size:12px;");
                objHtmlTable3 = new HtmlTable(); objHtmlTable3.Border = 0; objHtmlTable3.Width = "70%"; objHtmlTable3.Attributes.Add("style", "font-size:12px;");
               

                #region mapped Date
                objCommand.CommandText = "select max(MAPPED_DATE) from collect_component_master where student_id = '" + varStudentId + "'";
                if (!Convert.ToString(objCommand.ExecuteScalar()).Equals("")) 
                     varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                
                
                #endregion

                #region Student Details
                //DateTime MNTHNAME = Convert.ToDateTime(Session["MNTHNAME"]);
                

                 objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + varStudentId + "'";
                objCommand.CommandText = "select A.first_name,A.middle_name,A.last_name,A.father_name,A.student_registration_nbr,a.STUDENT_ROLL_NBR,a.DATE_OF_ADMISSION,a.GENDER,a.ADDRESS_LINE1,a.CITY,a.STATE,a.FATHER_NAME,a.FATHER_OCCUPATION,a.MOTHER_NAME,a.MOTHER_OCCUPATION,B.class_name,B.class_section from ign_student_master A, ign_class_master B where A.class_code = B.class_code and A.student_id = '" + varStudentId + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "Fee Bill";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "School Copy";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2; objHtmlTableCell1.Align = "center";
                    //objHtmlTableCell1.InnerHtml = "Guru Nanak Public School<br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "Note: LAST DATE OF FEE DEPOSITE IS 10 OF EVERY 1ST MONTH.AFTER WHICH RS.100 WILL BE CHARGED AS FINE.<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //new
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);
                    //new ends

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                    objHtmlTableCell1.InnerHtml = Session["sdate"] + "-2014  " + " To " + Session["edate"] + "-2014";
                    //////////aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
                   
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                     ////ashok//////
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Father's Occupation : " + Convert.ToString(objDtReader["father_OCCUPATION"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    

                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "mother's Name : " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "Mother's Occupation: " + Convert.ToString(objDtReader["mother_OCCUPATION"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //objHtmlTableRow1 = new HtmlTableRow();
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "Addresh : " + Convert.ToString(objDtReader["ADDRESS_LINE1"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTableCell1 = new HtmlTableCell();
                    //objHtmlTableCell1.InnerHtml = "City: " + Convert.ToString(objDtReader["city"]) + "<br/><br/>";
                    //objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //objHtmlTable1.Rows.Add(objHtmlTableRow1);


                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "State : " + Convert.ToString(objDtReader["state"]) + "<br/><br/>";
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    //--------------------------------------------------------------------------//
                //objHtmlTableRow2 = new HtmlTableRow();
                //objHtmlTableCell2 = new HtmlTableCell();
                //objHtmlTableCell2.InnerHtml = "Fee Bill";
                //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                //objHtmlTableCell2 = new HtmlTableCell();
                //objHtmlTableCell2.InnerHtml = "Student Copy";
                //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                //objHtmlTable2.Rows.Add(objHtmlTableRow2);

                //objHtmlTableRow2 = new HtmlTableRow();
                //objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "center";
                //objHtmlTableCell2.InnerHtml = "Guru Nanak Public School<br/>";
                //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                //objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2= new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2; objHtmlTableCell2.Align = "center";
                    objHtmlTableCell2.InnerHtml = "<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = "Note: LAST DATE OF FEE DEPOSITE IS 10 OF EVERY 1ST MONTH.AFTER WHICH RS.100 WILL BE CHARGED AS FINE..<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    //new
                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);
                    //new ends

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell(); objHtmlTableCell2.ColSpan = 2;
                    objHtmlTableCell2.InnerHtml = Session["sdate"] + "-2014  " + " To " + Session["edate"] + "-2014";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Father's Name: " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Adm No: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Father's Occupation : " + Convert.ToString(objDtReader["father_OCCUPATION"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);


                    //objHtmlTableRow2 = new HtmlTableRow();
                    //objHtmlTableCell2 = new HtmlTableCell();
                    //objHtmlTableCell2.InnerHtml = "mother's Name : " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    //objHtmlTableCell2 = new HtmlTableCell();
                    //objHtmlTableCell2.InnerHtml = "Mother's Occupation: " + Convert.ToString(objDtReader["mother_OCCUPATION"]) + "<br/><br/>";
                    //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    //objHtmlTable2.Rows.Add(objHtmlTableRow2);

                    //objHtmlTableRow2 = new HtmlTableRow();
                    //objHtmlTableCell2 = new HtmlTableCell();
                    //objHtmlTableCell2.InnerHtml = "Addresh : " + Convert.ToString(objDtReader["ADDRESS_LINE1"]) + "<br/><br/>";
                    //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    //objHtmlTableCell2 = new HtmlTableCell();
                    //objHtmlTableCell2.InnerHtml = "City: " + Convert.ToString(objDtReader["city"]) + "<br/><br/>";
                    //objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    //objHtmlTable2.Rows.Add(objHtmlTableRow2);
                    
                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "State : " + Convert.ToString(objDtReader["state"]) + "<br/><br/>";
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    //////////////////////////////////////////////////////

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);

                    //objHtmlTableRow3 = new HtmlTableRow();
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "Fee Bill";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "Bank Copy";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    //objHtmlTableRow3 = new HtmlTableRow();
                    //objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2; objHtmlTableCell3.Align = "center";
                    //objHtmlTableCell3.InnerHtml = "Guru Nanak Public School<br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTable3.Rows.Add(objHtmlTableRow3);
                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2; objHtmlTableCell3.Align = "center";
                    objHtmlTableCell3.InnerHtml = "";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2;
                    objHtmlTableCell3.InnerHtml = "Note: LAST DATE OF FEE DEPOSITE IS 10 OF EVERY 1ST MONTH.AFTER WHICH RS.100 WILL BE CHARGED AS FINE..<br/><br/>";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2;
                    objHtmlTableCell3.InnerHtml = "DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2;
                    objHtmlTableCell3.InnerHtml = Session["sdate"] + "-2014  " + " To " + Session["edate"] + "-2014";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Name: " + Convert.ToString(objDtReader["first_name"]) + " " + Convert.ToString(objDtReader["last_name"]);
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Class: " + Convert.ToString(objDtReader["class_name"]) + " " + Convert.ToString(objDtReader["class_section"]);
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Father's Name: : " + Convert.ToString(objDtReader["father_name"]) + "<br/><br/>";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Adm No.: " + Convert.ToString(objDtReader["student_registration_nbr"]) + "<br/><br/>";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    //objHtmlTableRow3 = new HtmlTableRow();
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "mother's Name : " + Convert.ToString(objDtReader["mother_name"]) + "<br/><br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "Mother's Occupation : " + Convert.ToString(objDtReader["mother_OCCUPATION"]) + "<br/><br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTable3.Rows.Add(objHtmlTableRow3);

                    //objHtmlTableRow3 = new HtmlTableRow();
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "Addresh : " + Convert.ToString(objDtReader["ADDRESS_LINE1"]) + "<br/><br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTableCell3 = new HtmlTableCell();
                    //objHtmlTableCell3.InnerHtml = "City: " + Convert.ToString(objDtReader["city"]) + "<br/><br/>";
                    //objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    //objHtmlTable3.Rows.Add(objHtmlTableRow3);


                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "State : " + Convert.ToString(objDtReader["state"]) + "<br/><br/>";
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);


                }
                objDtReader.Close();
                #endregion

                #region Fee Details
                int varTotal = 0;
                objCommand.CommandText = "select B.COMPONENT_NAME,(A.AMOUNT_PAYBLE-A.DISCOUNT) as AMOUNT_PAYBLE,MAPPED_DATE from collect_component_master A, component_master B where student_id = '" + varStudentId + "' and MAPPED_DATE = '" + varMappedDate.ToString("yyyy-MM-dd") + "' and A.COMPONENT_ID = B.COMPONENT_ID";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    objHtmlTableRow1 = new HtmlTableRow();
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = Convert.ToString(objDtReader["COMPONENT_NAME"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTableCell1 = new HtmlTableCell();
                    objHtmlTableCell1.InnerHtml = "Rs " + Convert.ToString(objDtReader["AMOUNT_PAYBLE"]);
                    objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                    objHtmlTable1.Rows.Add(objHtmlTableRow1);

                    //--------------------------------------------------------------------------//                    

                    objHtmlTableRow2 = new HtmlTableRow();
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = Convert.ToString(objDtReader["COMPONENT_NAME"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTableCell2 = new HtmlTableCell();
                    objHtmlTableCell2.InnerHtml = "Rs " + Convert.ToString(objDtReader["AMOUNT_PAYBLE"]);
                    objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                    objHtmlTable2.Rows.Add(objHtmlTableRow2);
                    varTotal = varTotal + Convert.ToInt32(objDtReader["AMOUNT_PAYBLE"]);

                    ////////////////////

                    objHtmlTableRow3 = new HtmlTableRow();
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = Convert.ToString(objDtReader["COMPONENT_NAME"]);
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTableCell3 = new HtmlTableCell();
                    objHtmlTableCell3.InnerHtml = "Rs " + Convert.ToString(objDtReader["AMOUNT_PAYBLE"]);
                    objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                    objHtmlTable3.Rows.Add(objHtmlTableRow3);
                    varTotal = varTotal + Convert.ToInt32(objDtReader["AMOUNT_PAYBLE"]);

                } objDtReader.Close();
                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>Total";
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTableCell1 = new HtmlTableCell();
                objHtmlTableCell1.InnerHtml = "<br/>Rs " + Convert.ToString(varTotal);
                objHtmlTableRow1.Cells.Add(objHtmlTableCell1);
                objHtmlTable1.Rows.Add(objHtmlTableRow1);

                objHtmlTableRow1 = new HtmlTableRow();
                objHtmlTableCell1 = new HtmlTableCell(); objHtmlTableCell1.ColSpan = 2;
                objHtmlTableCell1.InnerHtml = "Received Rupees....................................<br/>By Cash/Cheque No..............................<br/>Date....................  Drawn on...................<br/>Branch...................................................";
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
                objHtmlTableCell2.InnerHtml = "Received Rupees....................................<br/>By Cash/Cheque No..............................<br/>Date....................  Drawn on...................<br/>Branch...................................................";
                objHtmlTableRow2.Cells.Add(objHtmlTableCell2);
                objHtmlTable2.Rows.Add(objHtmlTableRow2);

                objHtmlTableRow3 = new HtmlTableRow();
                objHtmlTableCell3 = new HtmlTableCell();
                objHtmlTableCell3.InnerHtml = "<br/>Total";
                objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                objHtmlTableCell3 = new HtmlTableCell();
                objHtmlTableCell3.InnerHtml = "<br/>Rs " + Convert.ToString(varTotal);
                objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                objHtmlTable3.Rows.Add(objHtmlTableRow3);

                objHtmlTableRow3 = new HtmlTableRow();
                objHtmlTableCell3 = new HtmlTableCell(); objHtmlTableCell3.ColSpan = 2;
                objHtmlTableCell3.InnerHtml = "Received Rupees....................................<br/>By Cash/Cheque No..............................<br/>Date....................  Drawn on...................<br/>Branch..................................................." ;
                objHtmlTableRow3.Cells.Add(objHtmlTableCell3);
                objHtmlTable3.Rows.Add(objHtmlTableRow3);
                #endregion

                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "left";
                objHtmlTableCell.Controls.Add(objHtmlTable1);
                objHtmlTableRow.Cells.Add(objHtmlTableCell);

                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "left";
                objHtmlTableCell.Controls.Add(objHtmlTable2);
                objHtmlTableRow.Cells.Add(objHtmlTableCell);

                objHtmlTableCell = new HtmlTableCell();
                objHtmlTableCell.Align = "left";
                objHtmlTableCell.Controls.Add(objHtmlTable3);
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
}
