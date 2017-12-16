using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "NAEX CRM 4.0.0.";
        Check_LoggedUser();

        if(!IsPostBack){
            if(cUsuario.Load(Session["IdUsuario"].ToString()).IdCategoria == (Int16)Categoria.Administración)
                pnlAdmin.Visible = true;
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Session["IdUsuario"] = 0;
        Response.Redirect("Login.aspx", false);
    }

    public void Check_LoggedUser()
    {
        if (System.Convert.ToInt32(Session["IdUsuario"]) <= 0)
            Response.Redirect("Login.aspx");
    }
}
