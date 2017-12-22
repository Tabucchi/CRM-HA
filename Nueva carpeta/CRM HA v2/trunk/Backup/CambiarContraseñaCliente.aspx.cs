using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class CambiarContraseñaCliente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cCliente cliente = cCliente.Load(Session["IdCliente"].ToString());
                lbCliente.Text = cliente.Nombre;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cCliente cliente = cCliente.Load(Session["IdCliente"].ToString());

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
                    cliente.ClaveSistema = cUsuario.Codify(txtPassword.Text);
                    cliente.Save();
                    lbGuardar.Visible = true;
                    lbMensaje.Visible = false;
                    lbMensajeError.Visible = false;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Clientes.aspx");
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
}