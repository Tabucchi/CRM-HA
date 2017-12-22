using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BuscarCompra : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            #region Combos
            cbEmpresa.DataSource = cEmpresa.GetEmpresas();
            cbEmpresa.DataValueField = "id";
            cbEmpresa.DataTextField = "nombre";
            cbEmpresa.DataBind();
            ListItem cl = new ListItem("Seleccione una empresa", "0");
            cbEmpresa.Items.Insert(0, cl);

            cbEstado.DataSource = cCampoGenerico.GetEstadoCompra();
            cbEstado.DataValueField = "id";
            cbEstado.DataTextField = "descripcion";
            cbEstado.DataBind();
            ListItem es = new ListItem("Seleccione un estado", "0");
            cbEstado.Items.Insert(0, es);
            #endregion
        }
    }
    
    #region Botones
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        Int16 categoria = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).IdCategoria;
        string idUsuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id;

        lvCompra.DataSource = cCompra.SearchCompra(null, null, null, cbEmpresa.SelectedValue, null, Convert.ToInt16(cbEstado.SelectedValue), null);
        lvCompra.DataBind();
    }

    protected void btnNuevaCompra_Click(object sender, EventArgs e)
    {
        Response.Redirect("Compra.aspx?id=0");
    }

    protected void btnProveedor_Click(object sender, EventArgs e)
    {
        Response.Redirect("Proveedor.aspx");
    }
    #endregion

    protected void lvCompra_PreRender(object sender, EventArgs e)
    {
        Int16 categoria = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).IdCategoria;
        string idUsuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id;
        double totalProveedor = 0;
        double totalCliente = 0;

        lvCompra.DataSource = cCompra.SearchCompra(null, null, null, cbEmpresa.SelectedValue, null, Convert.ToInt16(cbEstado.SelectedValue), null);
        lvCompra.DataBind();

        List<cCompra> compras = cCompra.SearchCompra(null, null, null, cbEmpresa.SelectedValue, null, Convert.ToInt16(cbEstado.SelectedValue), null);

        for (int i = 0; i < compras.Count; i++)
        {
            totalProveedor = totalProveedor + Convert.ToDouble(compras[i].TotalProveedor);
            totalCliente = totalCliente + Convert.ToDouble(compras[i].TotalCliente);
        }

        if (categoria != 1)
            pnlTotales.Visible = false;

        lbTotalCliente.Text = totalCliente.ToString();
        lbTotalProveedor.Text = totalProveedor.ToString();
        lbTotalDiferencia.Text = Convert.ToString(totalCliente - totalProveedor);
    }
}
