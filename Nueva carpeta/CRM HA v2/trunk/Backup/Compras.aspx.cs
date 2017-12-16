using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Compras : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            cbEmpresa.DataSource = cEmpresa.GetEmpresas();
            cbEmpresa.DataValueField = "id";
            cbEmpresa.DataTextField = "nombre";
            cbEmpresa.DataBind();
            ListItem i = new ListItem("Todas", "0");
            cbEmpresa.Items.Insert(0, i);
            
            lvCompras.DataSource = cItem2.GetItems();
            lvCompras.DataBind();
        }
    }

   protected void btnBuscar_Click(object sender, EventArgs e)
    {
        List<cItem2> items = cItem2.Search(cbEmpresa.SelectedValue,
                                             string.IsNullOrEmpty(txtFechaDesde.Text) ? null : txtFechaDesde.Text,
                                             string.IsNullOrEmpty(txtFechaHasta.Text) ? null : txtFechaHasta.Text);
        lvCompras.DataSource = items;
        lvCompras.DataBind();
    }
}
