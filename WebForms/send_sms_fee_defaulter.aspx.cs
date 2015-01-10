using System;
using System.Collections;
using System.Collections.Generic;
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

public partial class iguardian_facilitator_send_sms_fee_defaulter : System.Web.UI.Page
{
    #region variables
    OdbcConnection objConnection;
    OdbcCommand objCommand;
    OdbcDataReader objDtReader;
    string varSessionUserName, varSessionSchoolSession, varSessionSchoolName ,varTxt_no_comunication;
    SendSmsClass objSendSmsClass;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        #region SessionVariable Assignment
        varSessionSchoolName = Convert.ToString(Session["SessionSchoolName"]);

        varSessionUserName = Convert.ToString(Session["SessionUserName"]);
        varSessionSchoolSession = Convert.ToString(Session["SessionSchoolSession"]);
        #endregion
        objConnection = new OdbcConnection();
        objCommand = new OdbcCommand();
        objSendSmsClass = new SendSmsClass();
        objConnection = (OdbcConnection)Session["_Connection"];
        objCommand.Connection = (OdbcConnection)Session["_Connection"]; 

      
        if (!IsPostBack)
        {
            ddlClassName.Items.Add(new ListItem("Select", "-1"));
            objCommand.CommandText = "call sp_class_master";
            objDtReader = objCommand.ExecuteReader();
            while (objDtReader.Read())
            {
                ddlClassName.Items.Add(new ListItem(Convert.ToString(objDtReader["class1"]).ToUpper(), Convert.ToString(objDtReader["class_code"]).ToUpper()));
            }
            objDtReader.Close();

            ddlStaffName.Items.Add(new ListItem("Select", "-1"));
            objCommand.CommandText = "call sp_staff_master";
            objDtReader = objCommand.ExecuteReader();
            while (objDtReader.Read())
            {
                ddlStaffName.Items.Add(new ListItem(Convert.ToString(objDtReader["name1"]).ToUpper(), Convert.ToString(objDtReader["employee_id"]).ToUpper()));
            }
            objDtReader.Close();
        }        

    }

    protected void ddlClassName_SelectedIndexChanged(object sender, EventArgs e)
    {
        grd_student_list.DataSource = null;
        grd_student_list.DataBind();
    }
    protected void btnStudentList_Click(object sender, EventArgs e)
    {
        
      
        if (ddlClassName.SelectedIndex != 0)
        {
            DateTime varMappedDate = Convert.ToDateTime(txtDate.Text);

            string sQL = "CALL `spStudentDetailsClassFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "','" + ddlClassName.SelectedValue + "')";

            //string sQL = "CALL `spStudentDetailsFromCollectComponentMasterFromSessionID`('" + Convert.ToString(varMappedDate.ToString("yyyy-MM-dd")) + "')";
            objCommand.CommandText = sQL; objCommand.CommandType = CommandType.StoredProcedure;
            objDtReader = objCommand.ExecuteReader();


            DataTable _dtblStudentRecords = new DataTable();

            _dtblStudentRecords.Columns.Add("STUDENT_ID");
            _dtblStudentRecords.Columns.Add("Name");
            _dtblStudentRecords.Columns.Add("Father_name");
            _dtblStudentRecords.Columns.Add("Comm_Nbr");
            ///_dtblStudentRecords.Rows.Add(_row);


            int i = 1;
            while (objDtReader.Read())
            { 
             int payable = Convert.ToInt32(objDtReader["Payble"]);
             if (payable > 0)
             {
                 DataRow _row = null;
                 _row = _dtblStudentRecords.NewRow();
                 _row["STUDENT_ID"] = Convert.ToString(objDtReader["STUDENT_ID"]);
                 _row["Name"] = Convert.ToString(objDtReader["SNAME"]);
                 _row["Father_name"] = Convert.ToString(objDtReader["FATHER_NAME"]); ;
                 _row["Comm_Nbr"] = Convert.ToString(objDtReader["NO_OF_COMMUNICATION"]);
                 _dtblStudentRecords.Rows.Add(_row);
             
             }
             else
             { 
             
             
             }
             i++;
             
            }
            grd_student_list.DataSource = _dtblStudentRecords;
            grd_student_list.DataBind();
           


            //OdbcDataAdapter obj_adapter = new OdbcDataAdapter("call sp_student_master('" + ddlClassName.SelectedValue + "')", objConnection);
            //DataSet obj_dataset = new DataSet();
            //obj_adapter.Fill(obj_dataset);
            //grd_student_list.DataSource = obj_dataset;
            //grd_student_list.DataBind();
        }
        else
        {
            grd_student_list.DataSource = null;
            grd_student_list.DataBind();
        }
    }

   

    protected void btn_submit_Click(object sender, EventArgs e)
    {
        #region SchoolSMSDetails

        string varSenderIdSMS = ""; string varschoolwebsite = ""; string varUserNameSMS = ""; string varschoolCODE = ""; string varPasswordSMS = ""; int varSendToCount = 0;
        objCommand.CommandText = "select * from ign_school_master";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            varSenderIdSMS = Convert.ToString(objDtReader["SENDER_ID"]);
            varUserNameSMS = Convert.ToString(objDtReader["SMS_USER_NAME"]);
            varPasswordSMS = Convert.ToString(objDtReader["SMS_PASSWORD"]);
            varschoolwebsite = Convert.ToString(objDtReader["SCHOOL_WEBSITE"]);
            varschoolCODE = Convert.ToString(objDtReader["SCHOOL_CODE"]);
        } objDtReader.Close();


        varPasswordSMS = "test";
        #endregion
        
        List<StudentDetailsClass> objListStudentDetailsClass = new List<StudentDetailsClass>();
        if (grd_student_list.Rows.Count > 0)
        {
            foreach (GridViewRow grdrow in grd_student_list.Rows)
            {
                //CheckBox chkbox = (CheckBox)grdrow.FindControl("chk_box");
                TextBox txt_due_amnt = (TextBox)grdrow.FindControl("txt_due_amnt");
                TextBox txt_fine_amnt = (TextBox)grdrow.FindControl("txt_fine_amnt");

                if (!(txt_due_amnt.Text.Equals("")) )
                {

                    HiddenField HiddenField1 = (HiddenField)grdrow.FindControl("HiddenField1");
                    Session["student_id"] = HiddenField1.Value;
                    objCommand.CommandText = "SELECT A.STUDENT_ID,A.FIRST_NAME,A.MIDDLE_NAME,A.LAST_NAME,A.STUDENT_REGISTRATION_NBR,A.CLASS_CODE,A.FATHER_NAME,A.FATHER_EMAIL_ID,A.MOTHER_EMAIL_ID,A.NO_OF_COMMUNICATION,B.CLASS_NAME,B.CLASS_SECTION FROM ign_student_master A, ign_class_master B WHERE A.CLASS_CODE = B.CLASS_CODE AND STUDENT_ID = '" + HiddenField1.Value + "'";
                    objDtReader = objCommand.ExecuteReader();
                   
                    while (objDtReader.Read())
                    {
                        StudentDetailsClass objDetails = new StudentDetailsClass();
                        objDetails.varStudentId = Convert.ToString(objDtReader["STUDENT_ID"]);
                        objDetails.varFirstName = Convert.ToString(objDtReader["FIRST_NAME"]);
                        objDetails.varMiddleName = Convert.ToString(objDtReader["MIDDLE_NAME"]);
                        objDetails.varLastName = Convert.ToString(objDtReader["LAST_NAME"]);
                        objDetails.varRegistrationNo = Convert.ToString(objDtReader["STUDENT_REGISTRATION_NBR"]);
                        objDetails.varClassCode = Convert.ToString(objDtReader["CLASS_CODE"]);
                        objDetails.varFatherName = Convert.ToString(objDtReader["FATHER_NAME"]);
                        objDetails.varNoOfCommunication = Convert.ToString(objDtReader["NO_OF_COMMUNICATION"]);
                        objDetails.varClassName = Convert.ToString(objDtReader["CLASS_NAME"]);
                        objDetails.varClassSection = Convert.ToString(objDtReader["CLASS_SECTION"]);
                        objDetails.var_father_mail_id = Convert.ToString(objDtReader["FATHER_EMAIL_ID"]);
                        objDetails.var_mother_mail_id = Convert.ToString(objDtReader["MOTHER_EMAIL_ID"]);

                        objDetails.varFeeDueAmnt = Convert.ToString(txt_due_amnt.Text.Trim());
                        objDetails.varFeeFineAmnt = Convert.ToString(txt_fine_amnt.Text.Trim());
                        objListStudentDetailsClass.Add(objDetails);
                    }
                    objDtReader.Close();
                }
            }

                if (objListStudentDetailsClass.Count > 0)
                {
                    string varMessageSubject = ""; string varMessage = ""; string varMessageById = ""; string varMessageByRole = ""; string varMessageType = "";
                    string varSendDetailText = ""; string varSchoolSessionId = ""; string varCreateBy = ""; string varMessageForId = "";
                    string varMessageForRole = ""; string varReadFlag = ""; string varClassName = ""; string varNoOfCommunication = ""; string varMessageCommType = " ";

                    varMessageSubject = "FEE";
                    varMessageType = "FEE_DEFAULTER";
                    varMessageById = ddlStaffName.SelectedValue;
                    varMessageByRole = "STAFF";
                    //if (objListStudentDetailsClass.Count == 1)
                    //{
                        varSendDetailText = null;
                    //}
                    //else if (objListStudentDetailsClass.Count > 1)
                    //{
                    //    varSendDetailText = "MESSAGE TO MULTIPLE STUDENT";
                    //}
                    varSchoolSessionId = Convert.ToString(Session["SessionSchoolSession"]);
                    varCreateBy = Convert.ToString(Session["SessionUserName"]);
                    varMessageForRole = "PARENT";
                    varReadFlag = "N";

                    foreach (StudentDetailsClass objDetails in objListStudentDetailsClass)
                    {
                        //varMessageForId = Convert.ToString(varMessageForId) + objDetails.varStudentId + ",";
                        varClassName = objDetails.varClassName + objDetails.varClassSection;
                        varNoOfCommunication = objDetails.varNoOfCommunication;
                    }
                  //  varMessageForId = varMessageForId.Substring(0, varMessageForId.Length - 1);
                    varMessageCommType = "SMS";

                  
                    foreach (StudentDetailsClass objDetails in objListStudentDetailsClass)
                    {
                        varMessageForId = objDetails.varStudentId;

                        if (objDetails.varFeeFineAmnt.Equals(" "))
                        {

                            varMessage = objDetails.varFirstName + ": D/P, Fees due is Rs." + objDetails.varFeeDueAmnt + ". Due date was " + Convert.ToDateTime(txtDate.Text).ToString("dd-MMM-yyyy") + ". Kindly pay fees at the earliest. Please ignore if already paid. Regards, " + varschoolCODE + "";
                            
                            // varMessage = "Dear Parent, Fees due for your ward " + objDetails.varFirstName + " is Rs." + objDetails.varFeeDueAmnt + ". The due date was " + Convert.ToDateTime(txtDate.Text).ToString("dd-MMM-yyyy") + ". Kindly pay the fees at the earliest. Please ignore if fees already paid. Regards, " + varschoolCODE + "";

                            string EmailText1 = "<h3>Mail From: iguardian :</h3> <br><h4>Student Name :</h4>" + objDetails.varFirstName + "<br> <h4>Class Name :</h4>" + objDetails.varClassName + "-" + objDetails.varClassSection + " <br><h4> Subject:</h4> " + varMessageSubject + " <br><br> <h4> Message : </h4><br> " + varMessage + " <br> ";

                            objSendSmsClass.emailSend(objDetails.var_father_mail_id, Server.UrlEncode(EmailText1), Server.UrlEncode(varschoolwebsite));
                            
                            objSendSmsClass.emailSend(objDetails.var_mother_mail_id, Server.UrlEncode(EmailText1), Server.UrlEncode(varschoolwebsite));
                        }

                        else
                        {
                            varMessage = objDetails.varFirstName + ": D/P, Fees due is Rs." + objDetails.varFeeDueAmnt + ". Fine is Rs. " + objDetails.varFeeFineAmnt + ". Due date was " + Convert.ToDateTime(txtDate.Text).ToString("dd-MMM-yyyy") + ". Kindly pay fees at the earliest. Please ignore if already paid. Regards, " + varschoolCODE + "";

                            string EmailText1 = "<h3>Mail From: iguardian :</h3> <br><h4>Student Name :</h4>" + objDetails.varFirstName + "<br> <h4>Class Name :</h4>" + objDetails.varClassName + "-" + objDetails.varClassSection + " <br><h4> Subject:</h4> " + varMessageSubject + " <br><br> <h4> Message : </h4><br> " + varMessage + " <br> ";

                            objSendSmsClass.emailSend(objDetails.var_father_mail_id, Server.UrlEncode(EmailText1), Server.UrlEncode(varschoolwebsite));
                            
                            objSendSmsClass.emailSend(objDetails.var_mother_mail_id, Server.UrlEncode(EmailText1), Server.UrlEncode(varschoolwebsite));
                        }
                       // Response.Write(varMessage);
                      
                        varSendToCount++;
                        string msgtext = varMessage;
                        objSendSmsClass.SMSSend(objDetails.varNoOfCommunication, Server.UrlEncode(msgtext), varUserNameSMS, varPasswordSMS, varSenderIdSMS);
                      // HiddenField HiddenField1 = (HiddenField)grdrow.FindControl("HiddenField1");

                        string varstudentid = Convert.ToString(Session["student_id"]);

                        objCommand.CommandText = "insert into defaulter_sms (Student_id,mobile_no,sms,sshool_session_id) values('" + varstudentid + "','" + varNoOfCommunication + "','" + varMessage + "','" + varSchoolSessionId + "')";
                       objCommand.ExecuteNonQuery();




                    //    InsertMessageBoardMaster objInsertMessageBoardMaster = new InsertMessageBoardMaster();
                    //    objInsertMessageBoardMaster.FuncInsertMessageBoardMasterParent(varMessageSubject, varMessage, varMessageById, varMessageByRole, varMessageType, varMessageCommType, varSendDetailText, varSessionSchoolSession, varSessionUserName, varMessageForId, varMessageForRole, varReadFlag, varClassName, (OdbcConnection)Session["sess_connection"]);
                    }

                    

                    string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Submitted'); window.location.href = 'send_sms_fee_defaulter.aspx';</script>";
                   Response.Write(varSubmitMessage);
                }
                else
                {
                   string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Please Select Student');</script>";
                   Response.Write(varSubmitMessage);
                }
              
            }
        }
   
}
