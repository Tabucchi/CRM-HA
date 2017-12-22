using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class MasterPageCliente : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userData = null;
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = identity.Ticket;
                        userData = ticket.UserData;
                    }
                }

                if (userData == "Usuario")
                {
                    if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int32)eCategoria.Administración || cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int32)eCategoria.Gerencia)
                    {
                    }
                }
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Response.Redirect("Login.aspx", false);
        }
    }
}