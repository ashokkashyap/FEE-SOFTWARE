using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebForms_Logout : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        Page.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Page.Response.Cache.SetNoStore();
        Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.Security.FormsAuthentication.SignOut();
        Session.Clear(); Session.Abandon();
        Page.Response.Buffer = true;
        Page.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
        Page.Response.Expires = -1500;
        Page.Response.CacheControl = "no-cache";
        Page.Response.Redirect("../Default.aspx");
    }
}