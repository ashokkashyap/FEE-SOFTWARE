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

public partial class admin_AdminMasterPage : System.Web.UI.MasterPage
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataAdapter _DtAdapter = null;
    OdbcDataReader _dtReader = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();
        var _User = Convert.ToString(Session["_User"]);
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
           
            if (!IsPostBack)
            {


                ddlSChoolList.Items.Clear();
                ddlSChoolList.Items.Add(new ListItem(Convert.ToString("-select-"), Convert.ToString("-1")));
                ddlSChoolList.Items.Add(new ListItem(Convert.ToString("SHIVALIK MOHALI"), Convert.ToString("DBConnect1")));
                ddlSChoolList.Items.Add(new ListItem(Convert.ToString("SHIVALIK PATIALA"), Convert.ToString("DBConnect2")));
                ddlSChoolList.Items.Add(new ListItem(Convert.ToString("SHIVALIK CHANDIGARH"), Convert.ToString("DBConnect3")));
                ddlSChoolList.Items.Add(new ListItem(Convert.ToString("SHIVALIK NAWSHARHR"), Convert.ToString("DBConnect4")));

            }

        }
        else { Response.Redirect("Logout.aspx"); }
        //if (Session["SelectedNode"] != null && Convert.ToString(Session["SelectedNode"]).Length > 0)
        //{
        //    //var SelectedNode = Convert.ToInt32(Session["SelectedNode"]);
        //    //tvMenus.CollapseAll();
        //    //tvMenus.Nodes[Convert.ToInt32(SelectedNode)].Expand();
        //    TreeNode _node = (TreeNode)Session["SelectedNode"];
        //    expandNode(_node);
        //}
    }
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeView1.CollapseAll();
        Session["TreeNodeIndex"] = Convert.ToString(TreeView1.Nodes.IndexOf(TreeView1.SelectedNode));
        TreeView1.Nodes[Convert.ToInt32(Session["TreeNodeIndex"])].Expand();
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

        //       

        Session["_Connection"] = "";
        pnlMenu.Controls.Clear();
        if (ddlSChoolList.SelectedIndex.Equals(1))
        {

            OdbcConnection _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect1"].ConnectionString);
            _Connection.Open(); Session["_Connection"] = _Connection;
           
        }
        else if (ddlSChoolList.SelectedIndex.Equals(2))
        {
            OdbcConnection _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect2"].ConnectionString);
            _Connection.Open(); Session["_Connection"] = _Connection;
            
        }

        else if (ddlSChoolList.SelectedIndex.Equals(3))
        {
            OdbcConnection _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect3"].ConnectionString);
            _Connection.Open(); Session["_Connection"] = _Connection;
            
        }

        else if (ddlSChoolList.SelectedIndex.Equals(4))
        {
            OdbcConnection _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect4"].ConnectionString);
            _Connection.Open(); Session["_Connection"] = _Connection;
           
        }
        else
        {
            OdbcConnection _Connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString);
            _Connection.Open(); Session["_Connection"] = _Connection;
           
        }




    }
}
