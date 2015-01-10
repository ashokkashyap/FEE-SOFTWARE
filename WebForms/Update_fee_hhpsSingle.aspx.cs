using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class WebForms_Update_fee_hhpsSingle : System.Web.UI.Page
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null;
    int totalamount = 0;
    int VarTotal = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;

            string varstudent_id = Convert.ToString(Request.QueryString["SID"]);
            if (!IsPostBack)
            {
                
                funstudentdetails(varstudent_id);

            }
        }
    }

    public void funstudentdetails(string varstudent_id)
    {

        //Response.Write(varstudent_id);
        //  Response.End();
        var SQL = " select ism.STUDENT_ID,	ism.FIRST_NAME,	ism.MIDDLE_NAME,	ism.LAST_NAME,	ism.STUDENT_REGISTRATION_NBR,	concat(icm.CLASS_NAME,'-',icm.CLASS_SECTION) as class_code,	ism.STUDENT_ROLL_NBR,	ism.DATE_OF_ADMISSION,	ism.BIRTH_DATE,	ism.HOUSE,	ism.GENDER,	ism.NATIONALITY,	ism.BLOOD_GROUP,	ism.RELIGION,	ism.CASTE,	ism.CATEGORY,	ism.MOTHER_TOUNGE,	ism.ADDRESS_LINE1,	ism.CITY,	ism.STATE,	ism.ZIP_CODE,	ism.COUNTRY,	ism.USER_NAME,	ism.USER_NAME_PARENT,	ism.PHONE_HOME1,	ism.FATHER_NAME,	ism.FATHER_OCCUPATION,	ism.FATHER_EMAIL_ID,	ism.FATHER_MOBILE_NO,	ism.FATHER_ORGANIZATION,	ism.FATHER_OFFICE_NO,	ism.FATHER_OFFICE_ADD,	ism.MOTHER_NAME,	ism.MOTHER_OCCUPATION,	ism.MOTHER_EMAIL_ID,	ism.MOTHER_MOBILE_NO,	ism.MOTHER_ORGANIZATION,	ism.MOTHER_OFFICE_NO,	ism.MOTHER_OFFICE_ADD,	ism.NO_OF_COMMUNICATION,	ism.EMER_CONTACT_NAME,	ism.EMER_CONTACT_PHONE,concat(ifnull(ism.FIRST_NAME,' '),' ',ifnull(ism.MIDDLE_NAME,' '),' ',ifnull(ism.LAST_NAME,' ')) as NAME,concat(ifnull(icm.CLASS_NAME,' '),' ',ifnull(icm.CLASS_SECTION,' ')) as CLASS,ism.NO_OF_COMMUNICATION,date_format(ism.DATE_OF_ADMISSION,'%e-%M-%Y') as DOA,date_format(ism.BIRTH_DATE,'%e-%M-%Y') as DOB,ism.ADDRESS_LINE1,ism.MOTHER_NAME,ism.FATHER_NAME,ism.NO_OF_COMMUNICATION,ism.COUNTRY,ism.STATE,ism.CITY  from ign_student_master ism, ign_class_master icm where  icm.CLASS_CODE=ism.CLASS_CODE and ism.student_id = " + varstudent_id + "";


        _Command.CommandText = SQL;
        using (var _dtReader = _Command.ExecuteReader())
        {
            if (_dtReader.HasRows)
            {
                while (_dtReader.Read())
                {

                    lblStudentID.Text = Convert.ToString(_dtReader["STUDENT_ID"]);
                    lblName.Text = Convert.ToString(_dtReader["NAME"]);
                    lblClass.Text = Convert.ToString(_dtReader["CLASS"]);
                    lblFatherName.Text = Convert.ToString(_dtReader["FATHER_NAME"]);
                    lblMotherName.Text = Convert.ToString(_dtReader["MOTHER_NAME"]);
                    lblAdmissionNo.Text = Convert.ToString(_dtReader["STUDENT_REGISTRATION_NBR"]);
                    txtAdmissionNo.Text = Convert.ToString(_dtReader["STUDENT_REGISTRATION_NBR"]);
                    lblAddress.Text = Convert.ToString(_dtReader["ADDRESS_LINE1"]);
                    lblNoOfCommunication.Text = Convert.ToString(_dtReader["NO_OF_COMMUNICATION"]);
                }
                _dtReader.Close();
            }
            else
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Check details Please !!!'); window.location.href='updateCollectedFeeNew.aspx';", true);

            }

        }


        ddlSelectDate.Items.Clear();
        SQL = "CALL `spDistinctCollectionDatesFromStudentID`('" + Convert.ToString(lblStudentID.Text) + "')"; _Command.CommandText = SQL;
        ddlSelectDate.Items.Add(new ListItem(Request.QueryString["pdate"].ToString(), Request.QueryString["pdate"].ToString()));



        //  SQL = "CALL `spComponentMaster`()"; 

        SQL = "SELECT distinct cm.COMPONENT_ID,cm.COMPONENT_NAME, cm.COMPONENT_FREQUENCY,cm.START_MONTH,cm.START_YEAR,cm.PRIORITY FROM component_master cm, collect_component_master b where b.STUDENT_ID='" + lblStudentID.Text + "' and b.COMPONENT_ID=cm.COMPONENT_ID   ORDER BY cm.PRIORITY";
        _Command.CommandText = SQL;
        OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
        _dtAdapter.SelectCommand = _Command;
        DataTable _dtblComponents = new DataTable();
        _dtAdapter.Fill(_dtblComponents);
        gvFeeAmountDetails.DataSource = _dtblComponents; gvFeeAmountDetails.DataBind(); gvFeeAmountDetails.Visible = false;
        dateseleceted();

    }
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        var SQL = "CALL `spStudentDetailsfromAdmissionNo`('" + txtAdmissionNo.Text.Trim() + "')";
        _Command.CommandText = SQL;
        using (var _dtReader = _Command.ExecuteReader())
        {
            if (_dtReader.HasRows)
            {
                while (_dtReader.Read())
                {
                    lblStudentID.Text = Convert.ToString(_dtReader["STUDENT_ID"]);
                    lblName.Text = Convert.ToString(_dtReader["NAME"]);
                    lblClass.Text = Convert.ToString(_dtReader["CLASS"]);
                    lblFatherName.Text = Convert.ToString(_dtReader["FATHER_NAME"]);
                    lblMotherName.Text = Convert.ToString(_dtReader["MOTHER_NAME"]);
                    lblAdmissionNo.Text = Convert.ToString(_dtReader["STUDENT_REGISTRATION_NBR"]);
                    lblAddress.Text = Convert.ToString(_dtReader["ADDRESS_LINE1"]);
                    lblNoOfCommunication.Text = Convert.ToString(_dtReader["NO_OF_COMMUNICATION"]);
                } _dtReader.Close();
            }
            else
            {

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Check details Please !!!'); window.location.href='updateCollectedFeeNew.aspx';", true);

            }
        }
        //Response.Write(lblStudentID.Text); Response.End();

        ddlSelectDate.Items.Clear();
        SQL = "CALL `spDistinctCollectionDatesFromStudentID`('" + Convert.ToString(lblStudentID.Text) + "')"; _Command.CommandText = SQL;
        ddlSelectDate.Items.Add(new ListItem("select", ""));


        SQL = "CALL `spComponentMaster`()"; _Command.CommandText = SQL;
        OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
        _dtAdapter.SelectCommand = _Command;
        DataTable _dtblComponents = new DataTable();
        _dtAdapter.Fill(_dtblComponents);
        gvFeeAmountDetails.DataSource = _dtblComponents; gvFeeAmountDetails.DataBind(); gvFeeAmountDetails.Visible = false;

        dateseleceted();
    }


    public void dateseleceted()
    {



        {

            txtDate.Text = Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("dd-MMMM-yyyy");
            var SQL = "call `spCollectedFeeAllDetailsFromStudentIDAndCollectionDate`('" + Convert.ToString(lblStudentID.Text) + "','" + Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd") + "')";
            _Command.CommandText = SQL;
            OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
            _dtAdapter.SelectCommand = _Command;
            DataTable _dtblCollectedFee = new DataTable();
            _dtAdapter.Fill(_dtblCollectedFee);

            foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
            {
                TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
                string varCOMPONENT_ID = ((HiddenField)_row.FindControl("hfCOMPONENT_ID")).Value;
                var Amount = from amt in _dtblCollectedFee.AsEnumerable() where Convert.ToInt32(amt["COMPONENT_ID"]).Equals(Convert.ToInt32(varCOMPONENT_ID)) select amt["AMOUNT_PAID"];
                if (Amount.Any())
                {
                    foreach (var _Amount in Amount)
                    {
                        txtAmount.Text = Convert.ToString(_Amount);
                    }
                }
                else
                {
                    txtAmount.Text = "0";
                }
            }

            _Command.CommandText = "select mode,CHEQUE_NUMBER,CHEQUE_DATE,BANK_NAME from collect_component_detail where STUDENT_ID='" + Convert.ToString(lblStudentID.Text) + "' and PAID_DATE='" + Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd") + "' ";

            var _dtreader = _Command.ExecuteReader();

            if (_dtreader.Read())
            {
               // ddlmode.Items.Add(new ListItem(_dtreader["mode"].ToString(), _dtreader["mode"].ToString()));

                ddlmode.SelectedIndex = ddlmode.Items.IndexOf(ddlmode.Items.FindByText(_dtreader["mode"].ToString()));
                txtchkno.Text = _dtreader["CHEQUE_NUMBER"].ToString();
                txtchkdte.Text = _dtreader["CHEQUE_DATE"].ToString();
                txtbankdetail.Text = _dtreader["BANK_NAME"].ToString();
            }
            _dtreader.Close();
            _Command.CommandText = "select fine from collect_component_detail where id='"+Convert.ToString(Session["detail_id_recpt"])+"'";
            string fine = Convert.ToString(_Command.ExecuteScalar());

             
            txtfine.Text = fine;
            gvFeeAmountDetails.Visible = true;
            btnVerify.Visible = true;
            btnReset.Visible = false;
           
        }


    }


    protected void ddlSelectDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSelectDate.SelectedIndex > 0)
        {

            txtDate.Text = Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("dd-MMMM-yyyy");
            var SQL = "call `spCollectedFeeAllDetailsFromStudentIDAndCollectionDate`('" + Convert.ToString(lblStudentID.Text) + "','" + Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd") + "')";
            _Command.CommandText = SQL;
            OdbcDataAdapter _dtAdapter = new OdbcDataAdapter();
            _dtAdapter.SelectCommand = _Command;
            DataTable _dtblCollectedFee = new DataTable();
            _dtAdapter.Fill(_dtblCollectedFee);

            foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
            {
                TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
                string varCOMPONENT_ID = ((HiddenField)_row.FindControl("hfCOMPONENT_ID")).Value;
                var Amount = from amt in _dtblCollectedFee.AsEnumerable() where Convert.ToInt32(amt["COMPONENT_ID"]).Equals(Convert.ToInt32(varCOMPONENT_ID)) select amt["AMOUNT_PAID"];
                if (Amount.Any())
                {
                    foreach (var _Amount in Amount)
                    {
                        txtAmount.Text = Convert.ToString(_Amount);
                    }
                }
                else
                {
                    txtAmount.Text = "0";
                }
            }

            gvFeeAmountDetails.Visible = true;
            btnVerify.Visible = true;
            btnReset.Visible = false;
 
        }
        else
        {
            gvFeeAmountDetails.Visible = false;
            btnVerify.Visible = false;
            btnReset.Visible = false;
           
            foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
            {
                TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
                txtAmount.Text = ""; lblTotalAmount.Text = "";
            }
        }
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        btnSubmit.Visible = true;
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
            if (Convert.ToString(txtAmount.Text).Trim().Length > 0)
            {
                try
                {
                    VarTotal += Convert.ToInt32(txtAmount.Text);
                }
                catch { continue; }
            }
            else { VarTotal += 0; }
           

        }
        VarTotal = VarTotal + Convert.ToInt32(txtfine.Text);
        lblTotalAmount.Text = "Total Amount: " + VarTotal.ToString();


        {
            Session["totalamount"] = VarTotal;
        }
        /////llllllllllllll/////////////
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
            txtAmount.Text = ""; lblTotalAmount.Text = "";
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //Response.Write(Session["totalamount"].ToString());
        //Response.End();
        int flag_mul = 0;
        foreach (GridViewRow _row in gvFeeAmountDetails.Rows)
        {
            if (flag_mul == 0)
            {
                TextBox txtAmount = (TextBox)_row.FindControl("txtAmount");
                string varCOMPONENT_ID = ((HiddenField)_row.FindControl("hfCOMPONENT_ID")).Value;
                if (Convert.ToString(txtAmount.Text).Trim().Length > 0)
                {
                    var sql_multiple = "select cast(count(*) as unsigned) as count_comp from collect_component_master where detail_id = '" + Convert.ToString(Session["detail_id_recpt"]) + "' and component_id ='" + Convert.ToString(varCOMPONENT_ID) + "'";
                    _Command.CommandText = sql_multiple;

                    //_Command.ExecuteNonQuery(); 
                    string check_multiple = _Command.ExecuteScalar().ToString();
                    if (check_multiple == "1" || check_multiple == "0")
                    {
                        var sQL = "update collect_component_master set AMOUNT_PAID='" + Convert.ToString(txtAmount.Text) + "',fee_UPDATE_DATE=now(),fee_UPDATE_TIME=now(),UPDATE_BY='" + Convert.ToString(Session["_User"]) + "' , paid_date ='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' where STUDENT_ID='" + Convert.ToString(lblStudentID.Text) + "' and COMPONENT_ID='" + Convert.ToString(varCOMPONENT_ID) + "' and  PAID_DATE='" + Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd") + "' and detail_id='" + Convert.ToString(Session["detail_id_recpt"]) + "'";
                        _Command.CommandText = sQL;
                        //_Command.Parameters.AddWithValue("AMOUNT_PAID", Convert.ToString(txtAmount.Text));
                        //_Command.Parameters.AddWithValue("UPDATE_BY", Convert.ToString(Session["_User"]));
                        //_Command.Parameters.AddWithValue("STUDENT_ID", Convert.ToString(lblStudentID.Text));
                        //_Command.Parameters.AddWithValue("COMPONENT_ID", Convert.ToString(varCOMPONENT_ID));

                        //_Command.Parameters.AddWithValue("PAID_DATE", Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd"));
                        _Command.ExecuteNonQuery();  /// _Command.Parameters.Clear();
                    }
                    else
                    {
                        var sQL = "update collect_component_master set fee_UPDATE_DATE=now(),fee_UPDATE_TIME=now(),UPDATE_BY='" + Convert.ToString(Session["_User"]) + "' , paid_date ='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' where STUDENT_ID='" + Convert.ToString(lblStudentID.Text) + "' and  PAID_DATE='" + Convert.ToDateTime(ddlSelectDate.Text).ToString("yyyy-MM-dd") + "' and detail_id='" + Convert.ToString(Session["detail_id_recpt"]) + "'";
                        _Command.CommandText = sQL;
                        _Command.ExecuteNonQuery();

                        if (ddlmode.SelectedItem.Text == "CASH")
                        {

                            _Command.CommandText = "update collect_component_detail  set AMOUNT_PAID='" + Convert.ToString(Session["totalamount"]) + "', fine='" + Convert.ToString(txtfine.Text) + "', MODE='CASH',CHEQUE_NUMBER='',CHEQUE_DATE=null,BANK_NAME='', paid_date ='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' where STUDENT_ID= '" + Convert.ToString(lblStudentID.Text) + "' and PAID_DATE='" + Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd") + "' and id='" + Convert.ToString(Session["detail_id_recpt"]) + "'";
                                _Command.ExecuteNonQuery();
                            

                        }

                        else
                        {

                            _Command.CommandText = "update collect_component_detail  set AMOUNT_PAID='" + Convert.ToString(Session["totalamount"]) + "', fine='" + Convert.ToString(txtfine.Text) + "', MODE='" + Convert.ToString(ddlmode.SelectedValue) + "',CHEQUE_NUMBER='" + Convert.ToString(txtchkno.Text) + "',CHEQUE_DATE='" + Convert.ToDateTime(txtchkdte.Text).ToString("yyyy-MM-dd") + "',BANK_NAME='" + Convert.ToString(txtbankdetail.Text) + "', paid_date ='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' where STUDENT_ID= '" + Convert.ToString(lblStudentID.Text) + "' and PAID_DATE='" + Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd") + "' and id='" + Convert.ToString(Session["detail_id_recpt"]) + "' ";
                                _Command.ExecuteNonQuery();
                             
                        }

                        ScriptManager.RegisterStartupScript(btnSubmit, this.GetType(), "Multiple", "alert('Mode Or  date are updated but Amount R not Updated'); window.location.href='fee_detail_single.aspx';", true);
                        flag_mul++;
                    }

                }
            }
        }

        if (ddlmode.SelectedItem.Text == "CASH")
        {
            if (flag_mul == 0)
            {
                _Command.CommandText = "update collect_component_detail  set AMOUNT_PAID = '" + Session["totalamount"].ToString() + "', fine='" + Convert.ToString(txtfine.Text) + "' ,MODE='CASH',CHEQUE_NUMBER='',CHEQUE_DATE=null,BANK_NAME='', paid_date ='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' where STUDENT_ID= '" + Convert.ToString(lblStudentID.Text) + "' and PAID_DATE='" + Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd") + "' and id='" + Convert.ToString(Session["detail_id_recpt"]) + "'";
                _Command.ExecuteNonQuery();
            }

        }

        else
        {
            if (flag_mul == 0)
            {
                _Command.CommandText = "update collect_component_detail  set AMOUNT_PAID = '" + Session["totalamount"].ToString() + "', fine='" + Convert.ToString(txtfine.Text) + "' , MODE='" + Convert.ToString(ddlmode.SelectedValue) + "',CHEQUE_NUMBER='" + Convert.ToString(txtchkno.Text) + "',CHEQUE_DATE='" + Convert.ToDateTime(txtchkdte.Text).ToString("yyyy-MM-dd") + "',BANK_NAME='" + Convert.ToString(txtbankdetail.Text) + "', paid_date ='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' where STUDENT_ID= '" + Convert.ToString(lblStudentID.Text) + "' and PAID_DATE='" + Convert.ToDateTime(ddlSelectDate.SelectedValue).ToString("yyyy-MM-dd") + "' and id='" + Convert.ToString(Session["detail_id_recpt"]) + "' ";
                _Command.ExecuteNonQuery();
            }
        }
      
        //Response.Write(_Command.CommandText);
        //Response.End();

        if (flag_mul == 0)
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Script", "alert('Fee Record Updated !!!'); window.location.href='fee_detail_single.aspx';", true);
        }
    }
}