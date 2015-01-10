using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Configuration;


public partial class WebForms_Mark_Tc : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;string studentID; OdbcDataReader reader;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                chktc.Checked = true;
                var SQL = "call spClassMaster()";
                var _dtAdapter = new OdbcDataAdapter(); var _dtblClasses = new DataTable();
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblClasses);
                ViewState["_dtblClasses"] = _dtblClasses;
                ddlSelectClass.DataSource = _dtblClasses; ddlSelectClass.DataTextField = "CLS"; ddlSelectClass.DataValueField = "CLASS_CODE"; ddlSelectClass.DataBind(); ddlSelectClass.Items.Insert(0, new ListItem("Select Class", ""));

                var _STUDENT_REGISTRATION_NBR = Convert.ToString(Session["_STUDENT_REGISTRATION_NBR"]);
                if (_STUDENT_REGISTRATION_NBR.Length > 0)
                {
                    if (Convert.ToString(Request.UrlReferrer).Contains("searchStudentByName.aspx") || Convert.ToString(Request.UrlReferrer).Contains("searchStudentByNameClassWise.aspx"))
                    {
                        
                        getStudentDetails1(_STUDENT_REGISTRATION_NBR);
                    }
                }


               
            }
        }
        else { Response.Redirect("Logout.aspx"); }
    }

    
    private void getStudentDetails1(string _STUDENT_REGISTRATION_NBR)
    {
        var SQL = "select * from ign_student_master where student_registration_nbr='" + Convert.ToString(txtAdmNoSrch.Text) + "';";
        //Response.Write(SQL);
        //Response.End();
        _Command.CommandText = SQL;

        var _dtblStudentData = new DataTable();
        var _dtAdapter = new OdbcDataAdapter();
        _dtAdapter.SelectCommand = _Command;
        _dtAdapter.Fill(_dtblStudentData);
        
        foreach (DataRow _row in _dtblStudentData.Rows)
        {
           // txtLeftOnDate.Text = Convert.ToDateTime(_row["LEFT_ON_DATE"]).ToString("dd-MMMM-yyyy");

            txtFirstName.Text = Convert.ToString(_row["FIRST_NAME"]);
            txtMiddleName.Text = Convert.ToString(_row["MIDDLE_NAME"]);
            txtLastName.Text = Convert.ToString(_row["LAST_NAME"]);
            txtAdmissionNo.Text = Convert.ToString(_row["STUDENT_REGISTRATION_NBR"]);
            //ddlSelectClass.SelectedValue = Convert.ToString(_row["CLASS_CODE"]);
            ddlSelectClass.SelectedIndex = ddlSelectClass.Items.IndexOf(ddlSelectClass.Items.FindByValue(Convert.ToString(_row["CLASS_CODE"])));
            txtRollNo.Text = Convert.ToString(_row["STUDENT_ROLL_NBR"]);
            //if (Convert.ToString(_row["DATE_OF_ADMISSION"]).Length > 0 && _row["DATE_OF_ADMISSION"] != DBNull.Value)
            //{
            //    txtAdmissionDate.Text = Convert.ToDateTime(_row["DATE_OF_ADMISSION"]).ToString("dd-MMMM-yyyy");
            //}
            if (Convert.ToString(_row["BIRTH_DATE"]).Length > 0 && _row["BIRTH_DATE"] != DBNull.Value)
            {
                txtBirthDate.Text = Convert.ToDateTime(_row["BIRTH_DATE"]).ToString("dd-MMMM-yyyy");
            }
            //ddlSelectGender.SelectedValue = Convert.ToString(_row["GENDER"]);
            ddlSelectGender.SelectedIndex = ddlSelectGender.Items.IndexOf(ddlSelectGender.Items.FindByText(Convert.ToString(_row["GENDER"])));
            txtCommunicationNo.Text = Convert.ToString(_row["NO_OF_COMMUNICATION"]);
            //ddlSelectReligion.SelectedValue = Convert.ToString(_row["RELIGION"]);
            ddlSelectReligion.SelectedIndex = ddlSelectReligion.Items.IndexOf(ddlSelectReligion.Items.FindByText(Convert.ToString(_row["RELIGION"])));
            txtCaste.Text = Convert.ToString(_row["CASTE"]);
            //ddlSelectCategory.SelectedValue = Convert.ToString(_row["CATEGORY"]);
            ddlSelectCategory.SelectedIndex = ddlSelectCategory.Items.IndexOf(ddlSelectCategory.Items.FindByText(Convert.ToString(_row["CATEGORY"])));

            //Response.Write(Convert.ToString(_row["CATEGORY"]));
            //Response.End();

            txtMotherTounge.Text = Convert.ToString(_row["MOTHER_TOUNGE"]);
            txtCity.Text = Convert.ToString(_row["CITY"]);
            txtAddress.Text = Convert.ToString(_row["ADDRESS_LINE1"]);
            txtFatherName.Text = Convert.ToString(_row["FATHER_NAME"]);
            txtFatherOccupation.Text = Convert.ToString(_row["FATHER_OCCUPATION"]);
            txtFatherEmail.Text = Convert.ToString(_row["FATHER_EMAIL_ID"]);
            txtFatherMobile.Text = Convert.ToString(_row["FATHER_MOBILE_NO"]);
            txtFatherOrganization.Text = Convert.ToString(_row["FATHER_ORGANIZATION"]);
            txtFatherOfficeNo.Text = Convert.ToString(_row["FATHER_OFFICE_NO"]);
            txtMotherName.Text = Convert.ToString(_row["MOTHER_NAME"]);
            txtMotherOccupation.Text = Convert.ToString(_row["MOTHER_OCCUPATION"]);
            txtMotherEmail.Text = Convert.ToString(_row["MOTHER_EMAIL_ID"]);
            txtMotherMobile.Text = Convert.ToString(_row["MOTHER_MOBILE_NO"]);
            txtMotheOrganization.Text = Convert.ToString(_row["MOTHER_ORGANIZATION"]);
            txtMotherOfficeNo.Text = Convert.ToString(_row["MOTHER_OFFICE_NO"]);
            txtEmergencyName.Text = Convert.ToString(_row["EMER_CONTACT_NAME"]);
            txtEmergencyContactNo.Text = Convert.ToString(_row["EMER_CONTACT_PHONE"]);
        }
    }


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        _Command.CommandText="select Student_id from ign_student_master where student_registration_nbr='"+txtAdmNoSrch.Text+"'";
        reader= _Command.ExecuteReader();
        studentID="";
        if(reader.Read())
        {
            studentID=Convert.ToString(reader["student_id"]);
        }
      reader.Close();

        using (var ObjConnection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString))
        {
            ObjConnection.Open();
            _Command.Connection = ObjConnection;


            _Command.Parameters.AddWithValue("FIRST_NAME", Convert.ToString(txtFirstName.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("MIDDLE_NAME", Convert.ToString(txtMiddleName.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("LAST_NAME", Convert.ToString(txtLastName.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("STUDENT_REGISTRATION_NBR", Convert.ToString(txtAdmissionNo.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("CLASS_CODE", Convert.ToString(ddlSelectClass.SelectedValue));

            _Command.Parameters.AddWithValue("STUDENT_ROLL_NBR", Convert.ToString(txtRollNo.Text.Trim().ToUpper()));

            //_Command.Parameters.AddWithValue("DATE_OF_ADMISSION", Convert.ToDateTime(txtAdmissionDate.Text.Trim()).ToString("yyyy-MM-dd"));
            _Command.Parameters.AddWithValue("BIRTH_DATE", Convert.ToDateTime(txtBirthDate.Text.Trim()).ToString("yyyy-MM-dd"));

            _Command.Parameters.AddWithValue("LEFT_ON_DATE", Convert.ToDateTime(txtLeftOnDate.Text.Trim()).ToString("yyyy-MM-dd"));


            _Command.Parameters.AddWithValue("GENDER", Convert.ToString(ddlSelectGender.SelectedValue));
            _Command.Parameters.AddWithValue("NO_OF_COMMUNICATION", Convert.ToString(txtCommunicationNo.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("RELIGION", Convert.ToString(ddlSelectReligion.SelectedValue));
            _Command.Parameters.AddWithValue("CASTE", Convert.ToString(txtCaste.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("CATEGORY", Convert.ToString(ddlSelectCategory.SelectedValue));
            _Command.Parameters.AddWithValue("MOTHER_TOUNGE", Convert.ToString(txtMotherTounge.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("CITY", Convert.ToString(txtCity.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("ADDRESS_LINE1", Convert.ToString(txtAddress.Text.Trim().ToUpper()));

            _Command.Parameters.AddWithValue("FATHER_NAME", Convert.ToString(txtFatherName.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("FATHER_OCCUPATION", Convert.ToString(txtFatherOccupation.Text.Trim()));
            _Command.Parameters.AddWithValue("FATHER_EMAIL_ID", Convert.ToString(txtFatherEmail.Text.Trim()));
            _Command.Parameters.AddWithValue("FATHER_MOBILE_NO", Convert.ToString(txtFatherMobile.Text.Trim()));
            _Command.Parameters.AddWithValue("FATHER_ORGANIZATION", Convert.ToString(txtFatherOrganization.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("FATHER_OFFICE_NO", Convert.ToString(txtFatherOfficeNo.Text.Trim().ToUpper()));

            _Command.Parameters.AddWithValue("MOTHER_NAME", Convert.ToString(txtMotherName.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("MOTHER_OCCUPATION", Convert.ToString(txtMotherOccupation.Text.Trim()));
            _Command.Parameters.AddWithValue("MOTHER_EMAIL_ID", Convert.ToString(txtMotherEmail.Text.Trim()));
            _Command.Parameters.AddWithValue("MOTHER_MOBILE_NO", Convert.ToString(txtMotherMobile.Text.Trim()));
            _Command.Parameters.AddWithValue("MOTHER_ORGANIZATION", Convert.ToString(txtMotheOrganization.Text.Trim()));
            _Command.Parameters.AddWithValue("MOTHER_OFFICE_NO", Convert.ToString(txtMotherOfficeNo.Text.Trim()));

            _Command.Parameters.AddWithValue("EMER_CONTACT_NAME", Convert.ToString(txtEmergencyName.Text.Trim().ToUpper()));
            _Command.Parameters.AddWithValue("EMER_CONTACT_PHONE", Convert.ToString(txtEmergencyContactNo.Text.Trim().ToUpper()));

            //var SQL = "update ign_student_master set FIRST_NAME=?,MIDDLE_NAME=?,LAST_NAME=?,STUDENT_REGISTRATION_NBR=?,class_code = ?,DATE_OF_ADMISSION=?,BIRTH_DATE=?,GENDER=?,NO_OF_COMMUNICATION=?,RELIGION=?,CASTE=?,CATEGORY=?,MOTHER_TOUNGE=?,CITY=?,ADDRESS_LINE1=?,FATHER_NAME=?,FATHER_OCCUPATION=?,FATHER_EMAIL_ID=?,FATHER_MOBILE_NO=?,FATHER_ORGANIZATION=?,FATHER_OFFICE_NO=?,MOTHER_NAME=?,MOTHER_OCCUPATION=?,MOTHER_EMAIL_ID=?,MOTHER_MOBILE_NO=?,MOTHER_ORGANIZATION=?,MOTHER_OFFICE_NO=?,EMER_CONTACT_NAME=?,EMER_CONTACT_PHONE=?,UPDATE_DATE=now(),UPDATE_TIME=now(),UPDATE_BY='FEE' where STUDENT_ID='" + Convert.ToString(Session["_STUDENT_ID"]) + "';";
            int i =0;
            if (chktc.Checked == true)
            {
                //var SQL = "insert into ign_tc_student_master(FIRST_NAME,MIDDLE_NAME,LAST_NAME,STUDENT_REGISTRATION_NBR,CLASS_CODE,STUDENT_ROLL_NBR,DATE_OF_ADMISSION,BIRTH_DATE,GENDER,NO_OF_COMMUNICATION,RELIGION,CASTE,CATEGORY,MOTHER_TOUNGE,CITY,ADDRESS_LINE1,FATHER_NAME,FATHER_OCCUPATION,FATHER_EMAIL_ID,FATHER_MOBILE_NO,FATHER_ORGANIZATION,FATHER_OFFICE_NO,MOTHER_NAME,MOTHER_OCCUPATION,MOTHER_EMAIL_ID,MOTHER_MOBILE_NO,MOTHER_ORGANIZATION,MOTHER_OFFICE_NO,EMER_CONTACT_NAME,EMER_CONTACT_PHONE,CREATE_BY,CREATE_DATE,CREATE_TIME,student_id) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,'" + Convert.ToString(Session["_User"]) + "',now(),now(),'"+studentID+"')";
                var SQL = "insert into ign_tc_student_master(FIRST_NAME,MIDDLE_NAME,LAST_NAME,STUDENT_REGISTRATION_NBR,CLASS_CODE,STUDENT_ROLL_NBR,BIRTH_DATE,LEFT_ON_DATE,GENDER,NO_OF_COMMUNICATION,RELIGION,CASTE,CATEGORY,MOTHER_TOUNGE,CITY,ADDRESS_LINE1,FATHER_NAME,FATHER_OCCUPATION,FATHER_EMAIL_ID,FATHER_MOBILE_NO,FATHER_ORGANIZATION,FATHER_OFFICE_NO,MOTHER_NAME,MOTHER_OCCUPATION,MOTHER_EMAIL_ID,MOTHER_MOBILE_NO,MOTHER_ORGANIZATION,MOTHER_OFFICE_NO,EMER_CONTACT_NAME,EMER_CONTACT_PHONE,CREATE_BY,CREATE_DATE,CREATE_TIME,student_id) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,'" + Convert.ToString(Session["_User"]) + "',now(),now(),'" + studentID + "')";

                //var SQL = "insert into ign_tc_student_master select * from ign_student_master where student_id='" + studentID + "' ";
                _Command.CommandText = SQL; i= _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
            }
            if (i > 0)
            {
                _Command.CommandText = "delete from ign_student_master where STUDENT_ID='" + studentID + "'";
                _Command.ExecuteNonQuery();
            }
        }
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('TC Submited !!!.'); window.location.href='Mark-Tc.aspx';", true);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        getStudentDetails1(txtAdmNoSrch.Text);
    }



    public string _STUDENT_REGISTRATION_NBR { get; set; }

    
}