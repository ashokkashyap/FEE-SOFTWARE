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

public partial class WebForms_busstop_student_mapping : System.Web.UI.Page
{
    #region variables
    //OdbcConnection objConnection;
    //OdbcCommand objCommand;
    OdbcConnection _Connection = null; OdbcCommand objCommand = null;
    OdbcDataReader objDtReader;
    string varSessionUserName, varSessionSchoolSession, varSchoolSessionID;
    //SendSmsClass objSendSmsClass;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            objCommand = new OdbcCommand();
            objCommand.Connection = _Connection;
            //try
            {
                #region SessionVariable Assignment
                varSessionUserName = Convert.ToString(Session["SessionUserName"]);
                varSessionSchoolSession = Convert.ToString(Session["SessionSchoolSession"]);
                varSchoolSessionID = Convert.ToString(Session["_SessionID"]);
                #endregion
                //objConnection = new OdbcConnection();
                //objCommand = new OdbcCommand();
                //objConnection = (OdbcConnection)Session["sess_connection"];
                //objCommand.Connection = (OdbcConnection)Session["sess_connection"];
                if (!IsPostBack)
                {
                    funcLoadBusRouteDetails();
                }
            }
            //catch (Exception ex)
            {
                //Response.Redirect(@"../logout.aspx");
            }
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
    #endregion


    protected void btnStudentList_Click(object sender, EventArgs e)
    {
        //try
        {
            //objCommand.CommandText = "SELECT A.STUDENT_ID,upper(concat(ifnull(A.FIRST_NAME,''),ifnull(A.MIDDLE_NAME,''),ifnull(A.LAST_NAME,''))) as STUDENT_NAME,A.FATHER_NAME AS FNAME,CONCAT(C.CLASS_NAME,' ',IFNULL(C.CLASS_SECTION,'')) AS CLASS1,B.BUS_STOP_ID FROM ign_student_master A, ign_bus_route_student_mapping B, CLASS_MASTER C WHERE A.STUDENT_ID = B.STUDENT_ID AND A.CLASS_CODE = C.CLASS_CODE AND B.BUS_ROUTE_ID = '" + ddlRouteName.SelectedValue + "'";
            objCommand.CommandText = "SELECT A.STUDENT_ID,upper(concat(ifnull(A.FIRST_NAME,''),' ',ifnull(A.MIDDLE_NAME,''),' ',ifnull(A.LAST_NAME,''))) as STUDENT_NAME,A.FATHER_NAME AS FNAME,CONCAT(C.CLASS_NAME,' ',IFNULL(C.CLASS_SECTION,'')) AS CLASS1,B.BUS_STOP_ID FROM ign_student_master A, ign_bus_route_student_mapping B, ign_class_master C WHERE A.STUDENT_ID = B.STUDENT_ID AND A.CLASS_CODE = C.CLASS_CODE AND B.BUS_ROUTE_ID = '" + ddlRouteName.SelectedValue + "' order by C.CLASS_PRIORITY,C.CLASS_SECTION,A.FIRST_NAME";
            //OdbcDataAdapter obj_adapter = new OdbcDataAdapter(objCommand.CommandText, objConnection);
            OdbcDataAdapter obj_adapter = new OdbcDataAdapter(objCommand.CommandText, _Connection);
            DataSet obj_dataset = new DataSet();
            obj_adapter.Fill(obj_dataset);
            grdStudentlist.DataSource = obj_dataset;
            grdStudentlist.DataBind();
            if (grdStudentlist.Rows.Count > 0)
            {
                btnSubmit.Visible = true;
            }
            foreach (GridViewRow grdRow in grdStudentlist.Rows)
            {
                DropDownList ddl = (DropDownList)grdRow.FindControl("DropDownList1");
                ddl.Items.Add(new ListItem("-SELECT-", "-1"));
                objCommand.CommandText = "select BUS_STOP_ID,BUS_STOP_NAME from ign_bus_stop_master WHERE BUS_ROUTE_ID = '" + ddlRouteName.SelectedValue + "'";
                objDtReader = objCommand.ExecuteReader();
                while (objDtReader.Read())
                {
                    ddl.Items.Add(new ListItem(Convert.ToString(objDtReader["BUS_STOP_NAME"]).ToUpper(), Convert.ToString(objDtReader["BUS_STOP_ID"]).ToUpper()));
                }
                objDtReader.Close();
                HiddenField HiddenField1 = (HiddenField)grdRow.FindControl("HiddenField1");
                objCommand.CommandText = "select BUS_STOP_ID from ign_bus_route_student_mapping where STUDENT_ID = '" + HiddenField1.Value + "'";
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(Convert.ToString(objCommand.ExecuteScalar())));
            }
        }
       // catch (Exception ex)
        {
            //Response.Redirect(@"../logout.aspx");
            //Response.Redirect("Logout.aspx");

        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
       // try
        {
            if (Panel1.Visible == true && ddlRouteName.SelectedIndex != 0)
            {
                string varStudentId = "";

                foreach (GridViewRow grdRow in grdStudentlist.Rows)
                {
                    HiddenField hdn = (HiddenField)grdRow.FindControl("HiddenField1");
                    varStudentId = hdn.Value;
                    DropDownList ddl = (DropDownList)grdRow.FindControl("DropDownList1");
                    if (ddl.SelectedIndex != 0)
                    {
                        objCommand.CommandText = "update ign_bus_route_student_mapping set BUS_STOP_ID = '" + ddl.SelectedValue + "' where student_id = '" + varStudentId + "' and BUS_ROUTE_ID='" + ddlRouteName.SelectedValue + "'";
                        objCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        objCommand.CommandText = "update ign_bus_route_student_mapping set BUS_STOP_ID = null where student_id = '" + varStudentId + "' and BUS_ROUTE_ID='" + ddlRouteName.SelectedValue + "'";
                        objCommand.ExecuteNonQuery();
                    }
                }
                string varSubmitMessage = "<script language='javascript' type='text/javascript'>alert('Successfully Updated'); window.location.href = 'busstop_student_mapping.aspx?SMD=" + Convert.ToString(Request.QueryString["SMD"]) + "&MMD=" + Convert.ToString(Request.QueryString["MMD"]) + "';</script>";
                Response.Write(varSubmitMessage);
            }
        }
       // catch (Exception ex)
        {
           // Response.Redirect(@"../logout.aspx");
        }
    }
}