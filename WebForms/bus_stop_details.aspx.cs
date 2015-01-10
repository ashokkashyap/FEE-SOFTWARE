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

public partial class WebForms_bus_stop_details : System.Web.UI.Page
{
    #region variables
    //OdbcConnection objConnection;
    OdbcConnection _Connection = null; OdbcCommand objCommand = null;
   // OdbcCommand objCommand;
    OdbcDataReader objDtReader;
    string varSessionUserName, varSessionSchoolSession, varSessionSchoolid;
    //SendSmsClass objSendSmsClass;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            objCommand = new OdbcCommand();
            objCommand.Connection = _Connection;
            // try
            {
                #region SessionVariable Assignment
                varSessionUserName = Convert.ToString(Session["SessionUserName"]);
                varSessionSchoolSession = Convert.ToString(Session["SessionSchoolSession"]);
                varSessionSchoolid = Convert.ToString(Session["_SessionID"]);
                #endregion
                // objConnection = new OdbcConnection();
                //objCommand = new OdbcCommand();
                //objConnection = (OdbcConnection)Session["sess_connection"];
                //objCommand.Connection = (OdbcConnection)Session["sess_connection"];
                if (!IsPostBack)
                {
                    funcLoadBusRouteDetails();
                }
            }
            //  catch (Exception ex)
            {
                //Response.Redirect(@"../logout.aspx");
                //Response.Redirect("Logout.aspx");
            }
        }
    }

    #region-------------------load functions-----------------------
    protected void funcLoadBusRouteDetails()
    {
        ddlRouteNameTab1.Items.Add(new ListItem("-SELECT-", "-1"));
        ddlRouteNameTab2.Items.Add(new ListItem("-SELECT-", "-1"));
        objCommand.CommandText = "select BUS_ROUTE_ID,ROUTE_NAME,DRIVER_NAME,HELPER_NAME,INCHARGE_ID,REMARKS from ign_bus_route_master";
        objDtReader = objCommand.ExecuteReader();
        while (objDtReader.Read())
        {
            ddlRouteNameTab1.Items.Add(new ListItem(Convert.ToString(objDtReader["ROUTE_NAME"]).ToUpper(), Convert.ToString(objDtReader["BUS_ROUTE_ID"]).ToUpper()));
            ddlRouteNameTab2.Items.Add(new ListItem(Convert.ToString(objDtReader["ROUTE_NAME"]).ToUpper(), Convert.ToString(objDtReader["BUS_ROUTE_ID"]).ToUpper()));
        }
        objDtReader.Close();
    }

    #endregion
    protected void btnAddStop_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlRouteNameTab1.SelectedIndex != 0 && txtStopNameTab1.Text.Trim() != "")
            {
                objCommand.Parameters.AddWithValue("@BUS_ROUTE_ID", ddlRouteNameTab1.SelectedValue);
                objCommand.Parameters.AddWithValue("@BUS_STOP_NAME", txtStopNameTab1.Text.ToUpper());
                objCommand.Parameters.AddWithValue("@BUS_STOP_DETAIL", txtStopDetailsTab1.Text.ToUpper());
                //objCommand.Parameters.AddWithValue("@SCHOOL_SESSION_ID", varSessionSchoolSession);
                objCommand.Parameters.AddWithValue("@SCHOOL_SESSION_ID", Convert.ToString(Session["_SessionID"]));
                //objCommand.Parameters.AddWithValue("@CREATE_BY", varSessionUserName);
                objCommand.Parameters.AddWithValue("@CREATE_BY",Convert.ToString(Session["_user"]));
                objCommand.CommandText = "insert into ign_bus_stop_master(BUS_ROUTE_ID,BUS_STOP_NAME,BUS_STOP_DETAIL,SCHOOL_SESSION_ID,CREATE_BY,CREATE_DATE,CREATE_TIME) values(?,?,?,?,?,now(),now())";
                objCommand.ExecuteNonQuery();
                string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Entered'); window.location.href = 'bus_stop_details.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
                Response.Write(varSubmitMessage);
            }
        }
        catch (Exception ex)
        {
           // Response.Redirect(@"../logout.aspx");
            Response.Redirect("Logout.aspx");
        }
    }


    protected void ddlRouteNameTab2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlRouteNameTab2.SelectedIndex != 0)
            {
                ddlStopNameTab2.Items.Clear();
                ddlStopNameTab2.Items.Add(new ListItem("-SELECT-", "-1"));
                objCommand.CommandText = "select BUS_STOP_ID,BUS_STOP_NAME from ign_bus_stop_master where BUS_ROUTE_ID = '" + ddlRouteNameTab2.SelectedValue + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    ddlStopNameTab2.Items.Add(new ListItem(Convert.ToString(objDtReader["BUS_STOP_NAME"]).ToUpper(), Convert.ToString(objDtReader["BUS_STOP_ID"]).ToUpper()));
                }
                objDtReader.Close();
            }
            else
            {
                ddlStopNameTab2.Items.Clear();
            }
        }
        catch (Exception ex)
        {
           // Response.Redirect(@"../logout.aspx");
            Response.Redirect("Logout.aspx");
        }
    }


    protected void ddlStopNameTab2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlStopNameTab2.SelectedIndex != 0)
            {
                objCommand.CommandText = "select * from ign_bus_stop_master where BUS_STOP_ID = '" + ddlStopNameTab2.SelectedValue + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    txtStopNameTab2.Text = Convert.ToString(objDtReader["BUS_STOP_NAME"]);
                    txtStopDetailsTab2.Text = Convert.ToString(objDtReader["BUS_STOP_DETAIL"]);
                }
                objDtReader.Close();
            }
            else
            {
                txtStopNameTab2.Text = ""; txtStopDetailsTab2.Text = "";
            }
        }
        catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
            Response.Redirect("Logout.aspx");
        }
    }
    protected void btnUpdateStop_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlRouteNameTab2.SelectedIndex != 0 && ddlStopNameTab2.SelectedIndex != 0 && txtStopNameTab2.Text.Trim() != "")
            {
                objCommand.Parameters.AddWithValue("@BUS_STOP_NAME", txtStopNameTab2.Text.ToUpper());
                objCommand.Parameters.AddWithValue("@BUS_STOP_DETAIL", txtStopDetailsTab2.Text.ToUpper());
                objCommand.CommandText = "update ign_bus_stop_master set BUS_STOP_NAME = ?,BUS_STOP_DETAIL = ? where BUS_STOP_ID = '" + ddlStopNameTab2.SelectedValue + "'";
                objCommand.ExecuteNonQuery();
                string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Updated'); window.location.href = 'bus_stop_details.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
                Response.Write(varSubmitMessage);
            }
        }
        catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
            Response.Redirect("Logout.aspx");
        }
    }


    protected void btnDeleteStop_Click(object sender, EventArgs e)
    {
        try
        {
            objCommand.CommandText = "select count(*) from ign_bus_route_student_mapping where BUS_STOP_ID = '" + ddlStopNameTab2.SelectedValue + "'";
            if (Convert.ToInt32(objCommand.ExecuteScalar()) == 0)
            {
                objCommand.CommandText = "delete from ign_bus_stop_master where BUS_STOP_ID = '" + ddlStopNameTab2.SelectedValue + "'";
                objCommand.ExecuteScalar();
                string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Deleted'); window.location.href = 'bus_stop_details.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
                Response.Write(varSubmitMessage);
            }
        }
        catch (Exception ex)
        {
           // Response.Redirect(@"../logout.aspx");
            Response.Redirect("Logout.aspx");
        }
    }
}