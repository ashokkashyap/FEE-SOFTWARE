using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Odbc;

public partial class Site : System.Web.UI.MasterPage
{
    OdbcConnection _Connection = null; OdbcCommand _Command = null; OdbcDataAdapter _DtAdapter = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();
        var _User = Convert.ToString(Session["_User"]);
        if (Session["_Connection"] != null && Convert.ToString(Session["_Connection"]) != "")
        {
            _Connection = (OdbcConnection)Session["_Connection"];
            _Command = new OdbcCommand();
            _Command.Connection = _Connection;
            lblLoggedUser.Text = _User;
            if (!IsPostBack)
            {
                //getMenus();
                getMenuItems(); Session["SelectedNode"] = null;
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
    private void getMenus()
    {
        var SQL = "select * from menu_master";
        _DtAdapter = new OdbcDataAdapter(SQL, _Connection);
        var _dtblMenuEntries = new DataTable();
        _DtAdapter.Fill(_dtblMenuEntries);
        ViewState["_dtblMenuEntries"] = _dtblMenuEntries;

        var MenuContent = "<div id='main'><ul id='browser' class='filetree treeview-famfamfam'>";
        var _ParentMenus = from m in _dtblMenuEntries.AsEnumerable() where Convert.ToString(m["PARENT_MENU_ID"]).Equals("") orderby m["MENU_PRIORITY"] ascending select m;
        if (_ParentMenus.Any())
        {
            foreach (var ParentMenu in _ParentMenus)
            {
                MenuContent += "<li class='closed'><span class='folder' style='font-weight:bold;'>" + Convert.ToString(ParentMenu["MENU_NAME"]) + "</span>";
                //MenuContent += "<ul><li><span class='file'>" + Convert.ToString(ParentMenu["MENU_NAME"]) + "</span><li></ul>";
                MenuContent = AddChildNode(MenuContent, Convert.ToInt32(ParentMenu["MENU_ID"]));
                MenuContent += "</li>";
            }
        } MenuContent += "</ul></div>";
        //Response.Write(MenuContent); Response.End();
        pnlMenu.Controls.Add(new LiteralControl(MenuContent));
    }
    private void getMenuItems()
    {
        string varRole = Convert.ToString(Session["_Role"]);
        var SQL = "select * from menu_master where ROLE='" + varRole.ToUpper() +"';";
        _DtAdapter = new OdbcDataAdapter(SQL, _Connection);
        var _dtblMenuEntries = new DataTable();
        _DtAdapter.Fill(_dtblMenuEntries);
        ViewState["_dtblMenuEntries"] = _dtblMenuEntries;

        TreeNode objParentNode = null; 
        var _ParentMenus = from m in _dtblMenuEntries.AsEnumerable() where Convert.ToString(m["PARENT_MENU_ID"]).Equals("") orderby m["MENU_PRIORITY"] ascending select m;
        if (_ParentMenus.Any())
        {
            foreach (var ParentMenu in _ParentMenus)
            {
                objParentNode = new TreeNode();
                objParentNode.Text = Convert.ToString(ParentMenu["MENU_NAME"]);
                objParentNode.SelectAction = TreeNodeSelectAction.SelectExpand;
                if (Convert.ToString(ParentMenu["URL"]).Length > 0)
                { objParentNode.NavigateUrl = Convert.ToString("WebForms/" + Convert.ToString(ParentMenu["URL"])); }
                else { objParentNode.NavigateUrl = ""; }
                ChildNodeIterator(_dtblMenuEntries, Convert.ToInt32(ParentMenu["MENU_ID"]), objParentNode);
                tvMenus.Nodes.Add(objParentNode);
            }
        }
        tvMenus.CollapseAll();
    }
    private void ChildNodeIterator(DataTable _dtblMenuEntries, int parentMenuID, TreeNode ParentNode)
    {
        var _SubMenu = from sm in _dtblMenuEntries.AsEnumerable() where Convert.ToString(sm["PARENT_MENU_ID"]).Equals(Convert.ToString(parentMenuID)) orderby sm["MENU_PRIORITY"] ascending select sm;
        if (_SubMenu.Any())
        {
            foreach (var ChildMenu in _SubMenu)
            {
                var objChildNode = new TreeNode();
                objChildNode.SelectAction = TreeNodeSelectAction.Select;
                objChildNode.Text = Convert.ToString(ChildMenu["MENU_NAME"]);
                if (Convert.ToString(ChildMenu["URL"]).Length > 0)
                { objChildNode.NavigateUrl = Convert.ToString(Convert.ToString(ChildMenu["URL"])); }
                else { objChildNode.NavigateUrl = ""; }
                ChildNodeIterator(_dtblMenuEntries, Convert.ToInt32(ChildMenu["MENU_ID"]), objChildNode);
                ParentNode.ChildNodes.Add(objChildNode);
            }
        }
    }
    private string AddChildNode(string menuCode,int ParentMenuID)
    {
        var _dtblMenuEntries = (DataTable)ViewState["_dtblMenuEntries"];
        var _SubMenu = from sm in _dtblMenuEntries.AsEnumerable() where Convert.ToString(sm["PARENT_MENU_ID"]).Equals(Convert.ToString(ParentMenuID)) orderby sm["MENU_PRIORITY"] ascending select sm;
        if (_SubMenu.Any())
        {
            foreach (var ChildMenu in _SubMenu)
            {
                var Child = from ch in _dtblMenuEntries.AsEnumerable() where Convert.ToString(ch["PARENT_MENU_ID"]).Equals(Convert.ToString(ChildMenu["MENU_ID"])) orderby ch["MENU_PRIORITY"] ascending select ch;
                if (Child.Any())
                {
                    menuCode += "<ul><li class='closed'><span class='folder' style='font-weight:bold;'>" + Convert.ToString(ChildMenu["MENU_NAME"]) + "</span>";
                    menuCode = AddChildNode(menuCode, Convert.ToInt32(ChildMenu["MENU_ID"]));
                    menuCode += "</li></ul>";
                }
                else
                {
                    menuCode += "<ul><li><span class='file'><a href='" + Convert.ToString(ChildMenu["URL"]) + "'>" + Convert.ToString(ChildMenu["MENU_NAME"]) + "</a></span></li>";
                    menuCode = AddChildNode(menuCode, Convert.ToInt32(ChildMenu["MENU_ID"]));
                    menuCode += "</ul>";
                }
            }
        }
        return menuCode;
    }
    private void expandNode(TreeNode selectedNode)
    {
        if (tvMenus.SelectedNode.Parent != null)
        {
            int _parentIndex = tvMenus.Nodes.IndexOf(tvMenus.SelectedNode.Parent);
            tvMenus.Nodes[_parentIndex].Expand(); tvMenus.SelectedNode.Expand();
        }
        else
        {
            tvMenus.SelectedNode.Expand();
        }
    }
    protected void tvMenus_SelectedNodeChanged(object sender, EventArgs e)
    {
        //int i = tvMenus.Nodes.IndexOf(tvMenus.SelectedNode);
        //Response.Write(i); Response.End();

        //tvMenus.CollapseAll();
        //var _ParentNode = tvMenus.Nodes.IndexOf(tvMenus.SelectedNode.Parent);
        //var SelectedNode = tvMenus.SelectedNode.ValuePath;
        //Response.Write(SelectedNode); Response.End();
        //tvMenus.FindNode(SelectedNode).Expanded = true;
        //Response.Write(SelectedNodeIndex);
        Session["SelectedNode"] = tvMenus.SelectedNode;
        expandNode(tvMenus.SelectedNode);
        //Response.Write(SelectedNodeIndex); Response.End();
        //tvMenus.Nodes[SelectedNodeIndex].Expand();
        //if (_ParentNode.Equals(-1))
        //{
        //    tvMenus.Nodes[Convert.ToInt32(SelectedNodeIndex)].Expand();
        //}
        //else
        //{
        //    var SelectedNode = tvMenus.SelectedNode.Value;
        //    Response.Write(_ParentNode); Response.Write(" fdsfdsf " + SelectedNode); Response.End();
        //    //tvMenus.Nodes[Convert.ToInt32(_ParentNode)].ChildNodes[Convert.ToInt32(SelectedNodeIndex)].Expand();
        //}

        //var _Name = tvMenus.SelectedNode.Text.ToString();
        //var _Index = tvMenus.Nodes.IndexOf(tvMenus.FindNode(_Name));
        //Response.Write(_Name + " " + _Index);
    }
    protected void lb_Click(object sender, EventArgs e)
    {
        Page.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Page.Response.Cache.SetNoStore();
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
        Page.Response.Redirect("Logout.aspx");
    }
}
