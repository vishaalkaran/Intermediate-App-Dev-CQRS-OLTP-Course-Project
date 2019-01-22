using AppSecurity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eTools_2018_E01_Team_F_WebSite
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Request.IsAuthenticated) //are you logged on
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    if (!User.IsInRole(SecurityRoles.WebsiteAdmins)) //put a comma between each of the roles to allow multiple roles
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }
            }
        }
    }
}