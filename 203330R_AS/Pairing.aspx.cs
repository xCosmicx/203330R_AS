using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _203330R_AS
{
    public partial class Pairing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Name = Request.QueryString["Name"];
            System.Diagnostics.Debug.WriteLine(Name);
            var appName = "203330R_AS";
            var userName = Name;
            System.Diagnostics.Debug.WriteLine(userName);
            var secretCode = "53cr3t1v3";
            var api = new GoogleAuthenticatorAPI();
            var setup = api.Pair(appName, userName, secretCode);
            Response.Write(setup.Html);
            
        }

    }
}