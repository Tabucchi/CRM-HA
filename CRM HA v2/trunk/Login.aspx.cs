using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Login : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            TipoUsuario tipoUsuario;

            int id = cUsuario.LoginUser(nombreUsuario.Value, cUsuario.Codify(pass.Value));
            if (id > 0)
                tipoUsuario = TipoUsuario.Usuario;
            else
            {
                id = cEmpresa.LoginEmpresaCliente(nombreUsuario.Value, cCliente.Codify(pass.Value));
                tipoUsuario = TipoUsuario.Cliente;
            }

            if (id > 0)
            {
                // Se crea el ticket de autenticación
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1, // Versión del ticket
                        Convert.ToString(id), //ID de usuario asociado al ticket
                        DateTime.Now, //Fecha de creación del ticket
                        DateTime.Now.AddMinutes(55), //Fecha y Hora de las expiración de la cookie
                        false, //Si es TRUE la cookie no expira.
                        tipoUsuario.ToString(), //Almacena datos del usuario, en este caso los roles
                        FormsAuthentication.FormsCookiePath); // El path de la cookie especificado en el Web.Config

                // Se encripta la cookie para añadir más seguridad
                string hashCookies = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies); //Cookie encriptada
                Response.Cookies.Add(cookie);

                // Registro Ingreso
                cUsuario.RegisterAccess(id.ToString(), HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

                // Redirecciono al Usuario - Importante!! no usar el RedirectFromLoginPage para que se puedan usar las Cookies de los HttpModules 
                //Response.Redirect(FormsAuthentication.GetRedirectUrl(Convert.ToString(id), false));

                Response.Redirect("Default.aspx");

            }
            else
            {
                mensajeError.Visible = true;
            }
        }
        catch
        {
            mensajeError.Visible = true;
        }
    }
}
