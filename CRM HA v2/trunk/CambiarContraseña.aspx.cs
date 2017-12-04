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

public partial class CambiarContraseña : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cUsuario usuario = cUsuario.Load(HttpContext.Current.User.Identity.Name);
            lbUsuario.Text = usuario.Nombre;
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        cUsuario usuario = cUsuario.Load(HttpContext.Current.User.Identity.Name);

        if (txtPassword.Text != txtConfirmarContraseña.Text || txtPassword.Text == "" || txtConfirmarContraseña.Text == "")
        {
            lbMensaje.Visible = true;
            lbMensajeError.Visible = false;
            txtPassword.Text = "";
            txtConfirmarContraseña.Text = "";
        }
        else
        {
            int j = 0; //se usa como flag.
            int l = 0;
            for (int i = 0; i < txtPassword.Text.Length; i++)
            {
                if (Char.IsNumber(txtPassword.Text, i)) //Compruebo que haya números en el código
                    j = 1;

                if (Char.IsLetter(txtPassword.Text, i)) //Compruebo que haya letras en el código
                    l = 1;
            }

            if (txtPassword.Text.Length < 5 || j == 0 || l == 0)
            {
                lbMensaje.Visible = false;
                lbMensajeError.Visible = true;
                return;
            }
            else
            {
                usuario.Clave = cUsuario.Codify(txtPassword.Text);
                usuario.Save();
                lbGuardar.Visible = true;
                lbMensaje.Visible = false;
                lbMensajeError.Visible = false;
            }
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }

    protected void ckPass_CheckedChanged(object sender, EventArgs e)
    {
        string pass = txtPassword.Text;
        string pass2 = txtConfirmarContraseña.Text;
        if (ckPass.Checked)
        {
            txtPassword.TextMode = TextBoxMode.SingleLine;
            txtConfirmarContraseña.TextMode = TextBoxMode.SingleLine;
        }
        else
        {

            txtPassword.TextMode = TextBoxMode.Password;
            txtConfirmarContraseña.TextMode = TextBoxMode.Password;
        }
    }
}
