using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPageCliente : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "NAEX CRM 4.0.0.";
        if (System.Convert.ToInt32(Session["IdCliente"]) <= 0)
    {
           Response.Redirect("Login.aspx");
        }
        else
        {
            Session["idEmpresa"] = cCliente.Load(Convert.ToString(Session["IdCliente"])).IdEmpresa;
        }
        
        //Deshabilitación de controles según permisos de usuario.
        //liNovedades.Attributes.CssStyle.Add("Display", "none");
        //liUsuarios.Attributes.CssStyle.Add("Display", "none");
        //liCompras.Attributes.CssStyle.Add("Display", "none");
        //liManuales.Attributes.CssStyle.Add("Display", "none");
        //liEstadisticas.Attributes.CssStyle.Add("Display", "none");
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["IdCliente"] = 0;
        Response.Redirect("Login.aspx", false);
    }
}
