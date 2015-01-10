using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;
using System.Configuration;

public partial class WebForms_addStudentDetails : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            if (!IsPostBack)
            {
                var SQL = "call spClassMaster()";
                var _dtAdapter = new OdbcDataAdapter(); var _dtblClasses = new DataTable();
                _Command.CommandText = SQL; _dtAdapter.SelectCommand = _Command;
                _dtAdapter.Fill(_dtblClasses);
                ViewState["_dtblClasses"] = _dtblClasses;
                ddlSelectClass.DataSource = _dtblClasses; ddlSelectClass.DataTextField = "CLS"; ddlSelectClass.DataValueField = "CLASS_CODE"; ddlSelectClass.DataBind(); ddlSelectClass.Items.Insert(0, new ListItem("Select Class", ""));
            }
            int admnbr = 0;
           // _Command.CommandText = "Select Max(STUDENT_REGISTRATION_NBR) from ign_student_master";
            //admnbr = Convert.ToInt32(_Command.ExecuteScalar()) + 1;

            //txtAdmissionNo.Text = admnbr.ToString();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
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
            _Command.Parameters.AddWithValue("DATE_OF_ADMISSION", Convert.ToDateTime(txtAdmissionDate.Text.Trim()).ToString("yyyy-MM-dd"));
            _Command.Parameters.AddWithValue("BIRTH_DATE", Convert.ToDateTime(txtBirthDate.Text.Trim()).ToString("yyyy-MM-dd"));
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

            var SQL = "insert into ign_student_master(FIRST_NAME,MIDDLE_NAME,LAST_NAME,STUDENT_REGISTRATION_NBR,CLASS_CODE,STUDENT_ROLL_NBR,DATE_OF_ADMISSION,BIRTH_DATE,GENDER,NO_OF_COMMUNICATION,RELIGION,CASTE,CATEGORY,MOTHER_TOUNGE,CITY,ADDRESS_LINE1,FATHER_NAME,FATHER_OCCUPATION,FATHER_EMAIL_ID,FATHER_MOBILE_NO,FATHER_ORGANIZATION,FATHER_OFFICE_NO,MOTHER_NAME,MOTHER_OCCUPATION,MOTHER_EMAIL_ID,MOTHER_MOBILE_NO,MOTHER_ORGANIZATION,MOTHER_OFFICE_NO,EMER_CONTACT_NAME,EMER_CONTACT_PHONE,CREATE_DATE,CREATE_TIME,CREATE_BY) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,now(),now(),'FEE')";
            _Command.CommandText = SQL; _Command.ExecuteNonQuery(); _Command.Parameters.Clear();
        }
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page),"Script","alert('Record Saved !!!.'); window.location.href='addStudentDetails.aspx';",true);
    }
}