using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using log4net;

public partial class Login : System.Web.UI.Page 
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        string nombreUsuario = "fayerwayer";
        cTwitter tw = new cTwitter();
        List<cTwitter> mensajes = tw.Mensajes(nombreUsuario, 8);
        if (mensajes.Count > 0)
        {
            //     rptTwitterMensajes.DataSource = mensajes;
            //     rptTwitterMensajes.DataBind();            
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            int t = cUsuario.LoginUser(nombreUsuario.Value, cUsuario.Codify(pass.Value));
            // Seteo las variables que voy a usar          
            Session.Add("IdPedido", Convert.ToInt16(-1));

            if (t > 0)
            {
                // Oculto el mensaje de error.
                mensajeError.Visible = false;
                // Identificamos que usuario esta logeado en el sistema
                Session.Add("IdUsuario", Convert.ToInt16(t));
                Session.Add("IdCliente", Convert.ToInt16(-1));

                string ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                cUsuario.RegisterAccess(t.ToString(), ip);
                FormsAuthentication.RedirectFromLoginPage("Main.aspx", false);
            }
            else
            {
                int c = cCliente.LoginCliente(nombreUsuario.Value, cCliente.Codify(pass.Value));

                if (c > 0)
                {
                    mensajeError.Visible = false;
                    Session.Add("IdCliente", Convert.ToInt16(c));
                    Session.Add("IdUsuario", Convert.ToInt16(-1));

                    FormsAuthentication.RedirectFromLoginPage("Clientes.aspx", false);
                }
                else
                {
                    // Muestro el mensaje de error.
                    mensajeError.Visible = true;
                    Session.Add("Error", "Please try again.");
                }
            }
        }
        catch (Exception ex)
        {
            mensajeError.Visible = true;
            Session.Add("Error", ex.ToString());

            log4net.Config.XmlConfigurator.Configure();
            log.Error("Login - " + DateTime.Now + "- " + ex.Message + " - btnLogin_Click" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            return;
        }
    }
}
