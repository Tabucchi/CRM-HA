using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "HA CRM";

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

                Check_LoggedUser();
                if (userData == "Usuario")
                {
                    if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int32)eCategoria.Administración)
                    {
                        pnlAdmin.Visible = true;
                        pnlReservas.Visible = true;                        
                    }

                    if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int32)eCategoria.Vendedor)
                    {                        
                        pnlDatosAnalisis.Visible = false;
                    }
                }
            }
        }

        public void Check_LoggedUser()
        {
            try
            {
                if (System.Convert.ToInt32(HttpContext.Current.User.Identity.Name) <= 0)
                    Response.Redirect("Login.aspx");

            }
            catch
            {
                Response.Redirect("Login.aspx");
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