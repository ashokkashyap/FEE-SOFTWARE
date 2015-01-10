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

public partial class WebForms_bus_route_details : System.Web.UI.Page
{
    #region variables
    OdbcConnection objConnection;
    OdbcCommand objCommand;
    OdbcDataReader objDtReader;
    string varSessionUserName, varSessionSchoolSession, varSessionSchoolid;
    //SendSmsClass objSendSmsClass;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            #region SessionVariable Assignment
            varSessionUserName = Convert.ToString(Session["SessionUserName"]);
            varSessionSchoolSession = Convert.ToString(Session["SessionSchoolSession"]);
            varSessionSchoolid = Convert.ToString(Session["_SessionID"]);
            #endregion
            //objConnection = new OdbcConnection();
            // by gajendra
            string con = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
            OdbcConnection objConnection = new OdbcConnection(con);
            objConnection.Open();

            objCommand = new OdbcCommand();
            objCommand.Connection = objConnection;
            //
            
            //objConnection = (OdbcConnection)Session["sess_connection"];
            //objCommand.Connection = (OdbcConnection)Session["sess_connection"];
            if (!IsPostBack)
            {
                funcLoadStaffDetails();
                funcLoadBusRouteDetails();
            }
        }
        catch (Exception ex)
        {
           // Response.Redirect(@"../logout.aspx");
            Response.Redirect("Logout.aspx");
        }
    }


    #region-------------------load functions-----------------------
    protected void funcLoadStaffDetails()
    {
        ddlStaffNameTab2.Items.Clear();
        ddlStaffNameTab2.Items.Add(new ListItem("-SELECT-", "-1"));
        ddlStaffNameTab1.Items.Clear();
        ddlStaffNameTab1.Items.Add(new ListItem("-SELECT-", "-1"));
        objCommand.CommandText = "select employee_id,concat(ifnull(FIRST_NAME,''),' ',ifnull(middle_name,''),' ',ifnull(last_name,'')) as name1 from ign_staff_master order by first_name";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            ddlStaffNameTab2.Items.Add(new ListItem(Convert.ToString(objDtReader["name1"]).ToUpper(), Convert.ToString(objDtReader["employee_id"]).ToUpper()));
            ddlStaffNameTab1.Items.Add(new ListItem(Convert.ToString(objDtReader["name1"]).ToUpper(), Convert.ToString(objDtReader["employee_id"]).ToUpper()));
        }
        objDtReader.Close();
    }
    protected void funcLoadBusRouteDetails()
    {
        ddlRouteNameTab2.Items.Add(new ListItem("-SELECT-", "-1"));
        objCommand.CommandText = "select BUS_ROUTE_ID,ROUTE_NAME,DRIVER_NAME,HELPER_NAME,INCHARGE_ID,REMARKS from ign_bus_route_master";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            ddlRouteNameTab2.Items.Add(new ListItem(Convert.ToString(objDtReader["ROUTE_NAME"]).ToUpper(), Convert.ToString(objDtReader["BUS_ROUTE_ID"]).ToUpper()));
        }
        objDtReader.Close();
    }

    #endregion

    protected void ddlRouteNameTab2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlRouteNameTab2.SelectedIndex != 0)
            {
                objCommand.CommandText = "select ROUTE_NAME,DRIVER_NAME,HELPER_NAME,INCHARGE_ID,REMARKS from ign_bus_route_master where BUS_ROUTE_ID = '" + ddlRouteNameTab2.SelectedValue + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    txtRouteNameTab2.Text = Convert.ToString(objDtReader["ROUTE_NAME"]);
                    txtDriverNameTab2.Text = Convert.ToString(objDtReader["DRIVER_NAME"]);
                    txtHelperNameTab2.Text = Convert.ToString(objDtReader["HELPER_NAME"]);
                    txtRouteDetailsTab2.Text = Convert.ToString(objDtReader["REMARKS"]);
                    ddlStaffNameTab2.SelectedIndex = ddlStaffNameTab2.Items.IndexOf(ddlStaffNameTab2.Items.FindByValue(Convert.ToString(objDtReader["INCHARGE_ID"])));
                }
                objDtReader.Close();
            }
        }
        catch (Exception ex)
        {
            Response.Redirect(@"../logout.aspx");
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlRouteNameTab2.SelectedIndex != 0)
            {
                objCommand.Parameters.AddWithValue("@ROUTE_NAME", txtRouteNameTab2.Text.ToUpper());
                objCommand.Parameters.AddWithValue("@DRIVER_NAME", txtDriverNameTab2.Text.ToUpper());
                objCommand.Parameters.AddWithValue("@HELPER_NAME", txtHelperNameTab2.Text.ToUpper());
                objCommand.Parameters.AddWithValue("@INCHARGE_ID", ddlStaffNameTab2.SelectedValue);
                objCommand.Parameters.AddWithValue("@REMARKS", txtRouteDetailsTab2.Text.ToUpper());
                objCommand.CommandText = "update ign_bus_route_master set ROUTE_NAME=?,DRIVER_NAME=?,HELPER_NAME=?,INCHARGE_ID=?,REMARKS=? where BUS_ROUTE_ID = '" + ddlRouteNameTab2.SelectedValue + "'";
                objCommand.ExecuteNonQuery();
                string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Updated'); window.location.href = 'bus_route_details.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
                Response.Write(varSubmitMessage);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect(@"../logout.aspx");
        }
    }
    protected void Delete_Click(object sender, EventArgs e)
    {
        if (ddlRouteNameTab2.SelectedIndex != 0)
        {
            objCommand.CommandText = "delete from ign_bus_route_master where BUS_ROUTE_ID = '" + ddlRouteNameTab2.SelectedValue + "'";
            objCommand.ExecuteNonQuery();

            objCommand.CommandText = "delete from ign_bus_route_student_mapping where BUS_ROUTE_ID = '" + ddlRouteNameTab2.SelectedValue + "'";
            objCommand.ExecuteNonQuery();
            objCommand.CommandText = "delete from ign_bus_stop_master where BUS_ROUTE_ID = '" + ddlRouteNameTab2.SelectedValue + "'";
            objCommand.ExecuteNonQuery();
        }
        string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Deleted'); window.location.href = 'bus_route_details.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
        Response.Write(varSubmitMessage);
    
    }

    protected void btnAddRoute_Click(object sender, EventArgs e)
    {
       // try
        {
            objCommand.Parameters.AddWithValue("@ROUTE_NAME", txtRouteNameTab1.Text.ToUpper());
            objCommand.CommandText = "select count(*) from ign_bus_route_master where ROUTE_NAME = ?";
            if (Convert.ToInt32(objCommand.ExecuteScalar()) == 0)
            {
                
                objCommand.Parameters.Clear();
                objCommand.Parameters.AddWithValue("@ROUTE_NAME", txtRouteNameTab1.Text.ToUpper());
                objCommand.Parameters.AddWithValue("@DRIVER_NAME", txtDriverNameTab1.Text.ToUpper());
                objCommand.Parameters.AddWithValue("@HELPER_NAME", txtHelperNameTab1.Text.ToUpper());
                objCommand.Parameters.AddWithValue("@INCHARGE_ID", ddlStaffNameTab1.SelectedValue);
                objCommand.Parameters.AddWithValue("@REMARKS", txtRouteDetailsTab1.Text.ToUpper());
                //objCommand.Parameters.AddWithValue("@SCHOOL_SESSION_ID", varSessionSchoolSession);
                objCommand.Parameters.AddWithValue("@SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
               //objCommand.Parameters.AddWithValue("@CREATE_BY", varSessionUserName);
                objCommand.Parameters.AddWithValue("@CREATE_BY", Convert.ToString(Session["_user"]));
               
               // objCommand.CommandText = "insert into ign_bus_route_master (ROUTE_NAME,DRIVER_NAME,HELPER_NAME,INCHARGE_ID,REMARKS,SCHOOL_SESSION_ID,CREATE_BY,CREATE_DATE,CREATE_TIME) values(?,?,?,?,?,?,?,now(),now())";
                objCommand.CommandText = "insert into ign_bus_route_master (ROUTE_NAME,DRIVER_NAME,HELPER_NAME,INCHARGE_ID,REMARKS,SCHOOL_SESSION_ID,CREATE_BY,CREATE_DATE,CREATE_TIME) values(?,?,?,?,?,?,?,now(),now())";
                
                objCommand.ExecuteNonQuery();
                //Response.Write("sfdfs");
                //Response.End();
                string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Entered'); window.location.href = 'bus_route_details.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
                Response.Write(varSubmitMessage);

            }
        }
       // catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
        }
    }
}