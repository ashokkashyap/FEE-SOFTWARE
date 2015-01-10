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


public partial class WebForms_busroute_student_mapping : System.Web.UI.Page
{
    #region variables
    //OdbcConnection objConnection;
    //OdbcCommand objCommand;
    OdbcConnection _Connection = null; OdbcCommand objCommand = null;
    OdbcDataReader objDtReader;
    string varSessionUserName, varSessionSchoolSession, varSessionSchoolid, varUsername;
    //SendSmsClass objSendSmsClass;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) !="")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            objCommand = new OdbcCommand();
            objCommand.Connection = _Connection;

            //try
            {
                #region SessionVariable Assignment
                varSessionUserName = Convert.ToString(Session["SessionUserName"]);
                varSessionSchoolSession = Convert.ToString(Session["SessionSchoolSession"]);
                varSessionSchoolid = Convert.ToString(Session["_SessionID"]);
                varUsername = Convert.ToString(Session["_User"]);
                #endregion
                //objConnection = new OdbcConnection();
                //objCommand = new OdbcCommand();
                //objConnection = (OdbcConnection)Session["sess_connection"];
                //objCommand.Connection = (OdbcConnection)Session["sess_connection"];
                if (!IsPostBack)
                {
                    funcLoadBusRouteDetails();
                    funcLoadClass();
                }
            }
            // catch (Exception ex)
            {
                //Response.Redirect(@"../logout.aspx");
                // Response.Redirect("Logout.aspx");
            }
        }
    }


    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (RadioButtonList1.SelectedValue == "1")
            {
                Panel1.Visible = true; ddlRouteName.Enabled = true; Panel2.Visible = false;
            }
            else if (RadioButtonList1.SelectedValue == "2")
            {
                Panel1.Visible = false; ddlRouteName.SelectedIndex = 0; ddlClassName.Enabled = true; ddlRouteName.Enabled = false; Panel2.Visible = true;
            }


        }
        catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
           // Response.Redirect("Logout.aspx");
        }
    }


    protected void btnStudentList_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadioButtonList1.SelectedValue == "1")
            {
                objCommand.Parameters.AddWithValue("@class_code", ddlClassName.SelectedValue);
                objCommand.CommandText = "select student_id,upper(concat(ifnull(first_name,''),' ',ifnull(middle_name,''),' ',ifnull(last_name,''))) as student_name, upper(ifnull(father_name,'')) as fname,STUDENT_REGISTRATION_NBR from ign_student_master where class_code='" + ddlClassName.SelectedValue + "' order by 2";
                //OdbcDataAdapter obj_adapter = new OdbcDataAdapter(objCommand.CommandText, objConnection);
                OdbcDataAdapter obj_adapter = new OdbcDataAdapter(objCommand.CommandText, _Connection);
                DataSet obj_dataset = new DataSet();
                obj_adapter.Fill(obj_dataset);
                grdStudentlist.DataSource = obj_dataset;
                grdStudentlist.DataBind();
                if (grdStudentlist.Rows.Count > 0)
                {
                    btnSubmit1.Visible = true;
                }
                foreach (GridViewRow gridrow in grdStudentlist.Rows)
                {
                    HiddenField HiddenField1 = (HiddenField)gridrow.FindControl("HiddenField1");
                    objCommand.CommandText = "select count(*) from ign_bus_route_student_mapping where BUS_ROUTE_ID='" + ddlRouteName.SelectedValue + "' and STUDENT_ID = '" + HiddenField1.Value + "'";
                    if (Convert.ToInt32(objCommand.ExecuteScalar()) > 0)
                    {
                        CheckBox CheckBox1 = (CheckBox)gridrow.FindControl("CheckBox1");
                        CheckBox1.Checked = true;
                    }
                }
            }
            else if (RadioButtonList1.SelectedValue == "2")
            {
                objCommand.CommandText = "select student_id,upper(concat(ifnull(first_name,''),' ',ifnull(middle_name,''),' ',ifnull(last_name,''))) as student_name, upper(ifnull(father_name,'')) as fname , STUDENT_REGISTRATION_NBR from ign_student_master where class_code='" + ddlClassName.SelectedValue + "' order by 2";
                //OdbcDataAdapter obj_adapter = new OdbcDataAdapter(objCommand.CommandText, objConnection);
                OdbcDataAdapter obj_adapter = new OdbcDataAdapter(objCommand.CommandText, _Connection);
                DataSet obj_dataset = new DataSet();
                obj_adapter.Fill(obj_dataset);
                grdStudentlist1.DataSource = obj_dataset;
                grdStudentlist1.DataBind();
                if (grdStudentlist1.Rows.Count > 0)
                {
                    btnSubmit2.Visible = true;
                }
                foreach (GridViewRow gridrow in grdStudentlist1.Rows)
                {
                    DropDownList ddl = (DropDownList)gridrow.FindControl("DropDownList1");
                    ddl.Items.Add(new ListItem("-SELECT-", "-1"));
                    objCommand.CommandText = "select BUS_ROUTE_ID,ROUTE_NAME,DRIVER_NAME,HELPER_NAME,INCHARGE_ID,REMARKS from ign_bus_route_master";
                    objDtReader = objCommand.ExecuteReader();
                    while (objDtReader.Read())
                    {
                        ddl.Items.Add(new ListItem(Convert.ToString(objDtReader["ROUTE_NAME"]).ToUpper(), Convert.ToString(objDtReader["BUS_ROUTE_ID"]).ToUpper()));
                    }
                    objDtReader.Close();
                    HiddenField HiddenField1 = (HiddenField)gridrow.FindControl("HiddenField1");
                    objCommand.CommandText = "select BUS_ROUTE_ID from ign_bus_route_student_mapping where STUDENT_ID = '" + HiddenField1.Value + "'";
                    ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(Convert.ToString(objCommand.ExecuteScalar())));

                }

            }
        }
        catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
            //Response.Redirect("Logout.aspx");
        }
    }

    #region-------------------load functions-----------------------

    protected void funcLoadBusRouteDetails()
    {
        ddlRouteName.Items.Add(new ListItem("-SELECT-", "-1"));
        objCommand.CommandText = "select BUS_ROUTE_ID,ROUTE_NAME,DRIVER_NAME,HELPER_NAME,INCHARGE_ID,REMARKS from ign_bus_route_master";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            ddlRouteName.Items.Add(new ListItem(Convert.ToString(objDtReader["ROUTE_NAME"]).ToUpper(), Convert.ToString(objDtReader["BUS_ROUTE_ID"]).ToUpper()));
        }
        objDtReader.Close();
    }
    protected void funcLoadClass()
    {
        ddlClassName.Items.Clear();
        ddlClassName.Items.Add(new ListItem("-SELECT-", "-1"));
        objCommand.CommandText = "call sp_class_master";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            ddlClassName.Items.Add(new ListItem(objDtReader["class1"].ToString(), objDtReader["class_code"].ToString()));
        }
        objDtReader.Close();
    }
    #endregion



    protected void btnSubmit1_Click(object sender, EventArgs e)
    {
        // try
        {
            if (Panel1.Visible == true)
            {
                string varStudentId = "";
               


                foreach (GridViewRow grdRow in grdStudentlist.Rows)
                {
                    HiddenField hdn = (HiddenField)grdRow.FindControl("HiddenField1");
                    varStudentId = hdn.Value;
                    CheckBox chk = (CheckBox)grdRow.FindControl("CheckBox1");
                    if (chk.Checked == true)
                    {


                        objCommand.CommandText = "select count(*) from ign_bus_route_student_mapping where student_id='" + varStudentId + "' ";
                        int count_status = Convert.ToInt32(objCommand.ExecuteScalar());
                        if (count_status.Equals(0))
                        {


                           // objCommand.CommandText = "insert into ign_bus_route_student_mapping(STUDENT_ID,BUS_ROUTE_ID,SCHOOL_SESSION_ID,CREATE_BY,CREATE_DATE,CREATE_TIME) values('" + varStudentId + "','" + ddlRouteName.SelectedValue + "','" + varSessionSchoolSession + "','" + varSessionUserName + "',now(),now())";
                            objCommand.CommandText = "insert into ign_bus_route_student_mapping(STUDENT_ID,BUS_ROUTE_ID,SCHOOL_SESSION_ID,CREATE_BY,CREATE_DATE,CREATE_TIME) values('" + varStudentId + "','" + ddlRouteName.SelectedValue + "','" + varSessionSchoolid + "','" + varUsername + "',now(),now())";
                            //Response.Write(objCommand.CommandText);
                            //Response.End();
                            objCommand.ExecuteNonQuery();

                        }
                        else
                        {

                            objCommand.CommandText = "update ign_bus_route_student_mapping set BUS_ROUTE_ID='" + ddlRouteName.SelectedValue + "' where student_id='" + varStudentId + "'";
                            objCommand.ExecuteNonQuery();

                        }
                    }
                    else
                    {

                        {
                            objCommand.CommandText = "delete from ign_bus_route_student_mapping where STUDENT_ID = '" + varStudentId + "'   and BUS_ROUTE_ID = '" + ddlRouteName.SelectedValue + "'";
                            objCommand.ExecuteNonQuery();
                        }
                    }
                }
                string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Updated'); window.location.href = 'busroute_student_mapping.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
                Response.Write(varSubmitMessage);
            }
        }
        //  catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
            //Response.Redirect("Logout.aspx");
        }
    }


    protected void btnSubmit2_Click(object sender, EventArgs e)
    {
        //  try
        {
            if (Panel2.Visible == true)
            {
                string varStudentId = "";

                foreach (GridViewRow grdRow in grdStudentlist1.Rows)
                {
                    HiddenField hdn = (HiddenField)grdRow.FindControl("HiddenField1");
                    varStudentId = hdn.Value;
                    DropDownList ddl = (DropDownList)grdRow.FindControl("DropDownList1");


                    if (ddl.SelectedIndex != 0)
                    {
                        objCommand.CommandText = "select count(*) from ign_bus_route_student_mapping where student_id='" + varStudentId + "' ";
                        int count_status = Convert.ToInt32(objCommand.ExecuteScalar());
                        
                        if (count_status.Equals(0))
                        {

                           // objCommand.Parameters.AddWithValue("@SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
                            objCommand.Parameters.AddWithValue("@CREATE_BY", Convert.ToString(Session["_user"]));
                            //objCommand.CommandText = "insert into ign_bus_route_student_mapping(STUDENT_ID,BUS_ROUTE_ID,SCHOOL_SESSION_ID,CREATE_BY,CREATE_DATE,CREATE_TIME) values('" + varStudentId + "','" + ddl.SelectedValue + "','" + varSessionSchoolSession + "','" + varSessionUserName + "',now(),now())";
                            objCommand.CommandText = "insert into ign_bus_route_student_mapping(STUDENT_ID,BUS_ROUTE_ID,SCHOOL_SESSION_ID,CREATE_BY,CREATE_DATE,CREATE_TIME) values('" + varStudentId + "','" + ddl.SelectedValue + "','" + varSessionSchoolid + "','" + varUsername + "',now(),now())";
                            
                            //Response.Write(objCommand.CommandText);
                            //Response.End();
                            objCommand.ExecuteNonQuery();
                            
                        }
                        else
                        {

                            objCommand.CommandText = "update ign_bus_route_student_mapping set BUS_ROUTE_ID='" + ddl.SelectedValue + "' where student_id='" + varStudentId + "' ";
                            objCommand.ExecuteNonQuery();
                        }

                    }
                    else
                    {
                        objCommand.CommandText = "delete from ign_bus_route_student_mapping where STUDENT_ID = '" + varStudentId + "'";
                        objCommand.ExecuteNonQuery();

                    }
                    //}
                }
                string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Updated'); window.location.href = 'busroute_student_mapping.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
                Response.Write(varSubmitMessage);
            }
        }
        //  catch (Exception ex)
        {
            //   Response.Redirect(@"../logout.aspx");
        }
    }
    protected void ddlRouteName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRouteName.SelectedIndex != 0)
        {
            ddlClassName.Enabled = true;
            ddlClassName.SelectedIndex = 0;

        }
        else
        {
            ddlClassName.Enabled = false;
            ddlClassName.SelectedIndex = 0;
        }

    }

}