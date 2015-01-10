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
        string varClassId = Request.QueryString["class_id"];
        int varSId = Convert.ToInt32(Request.QueryString["student_id"]);
        if(varSId==-1)
        {            
            feeBillId();
        }
        else
        {
            feeBillName();
        }
    }
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
            HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 0; objHtmlTable.Width = "800px";
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
                objHtmlTable1 = new HtmlTable(); objHtmlTable1.Border = 0; objHtmlTable1.Width = "70%"; objHtmlTable1.Attributes.Add("style", "font-size:12px;");
                objHtmlTable2 = new HtmlTable(); objHtmlTable2.Border = 0; objHtmlTable2.Width = "70%"; objHtmlTable2.Attributes.Add("style", "font-size:12px;");

                #region mapped Date
                objCommand.CommandText = "select max(MAPPED_DATE) from fee_collect_component_master where student_id = '" + varStudentId + "'";
                DateTime varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                #endregion

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
                    objHtmlTableCell1.InnerHtml = "Himalaya Public Sr. Sec. School<br/>";
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
                    objHtmlTableCell1.InnerHtml ="DATE :" + DateTime.Now.ToString("dd/MM/yyyy");
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
                    objHtmlTableCell2.InnerHtml = "Himalaya Public Sr. Sec. School<br/>";
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
                objCommand.CommandText = "select B.COMPONENT_NAME,(A.AMOUNT_PAYBLE-A.DISCOUNT) as AMOUNT_PAYBLE,MAPPED_DATE from fee_collect_component_master A, fee_component_master B where student_id = '" + varStudentId + "' and MAPPED_DATE = '" + varMappedDate.ToString("yyyy-MM-dd") + "' and A.COMPONENT_ID = B.COMPONENT_ID";
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
            HtmlTable objHtmlTable = new HtmlTable(); objHtmlTable.Border = 0; objHtmlTable.Width = "800px";
            HtmlTableRow objHtmlTableRow = null;
            HtmlTableCell objHtmlTableCell = null;

            HtmlTable objHtmlTable1 = null;
            HtmlTableRow objHtmlTableRow1 = null;
            HtmlTableCell objHtmlTableCell1 = null;

            HtmlTable objHtmlTable2 = null;
            HtmlTableRow objHtmlTableRow2 = null;
            HtmlTableCell objHtmlTableCell2 = null;
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

                #region mapped Date
                objCommand.CommandText = "select max(MAPPED_DATE) from collect_component_master where student_id = '" + varStudentId + "'";
                DateTime varMappedDate = Convert.ToDateTime(objCommand.ExecuteScalar());
                #endregion

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
                    objHtmlTableCell1.InnerHtml = "Guru Harkrishan Public School<br/>";
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
                    objHtmlTableCell2.InnerHtml = "Guru Harkrishan Public School<br/>";
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
}
