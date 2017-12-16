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
using System.Collections.Generic;
using log4net;

public partial class PedidosAcobrar : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cbEmpresa.DataSource = cEmpresa.GetListaEmpresas();
            cbEmpresa.DataValueField = "id";
            cbEmpresa.DataTextField = "nombre";
            cbEmpresa.DataBind();
            //Agrego item TODAS
            ListItem it = new ListItem("Todas", "0");
            cbEmpresa.Items.Insert(0, it);

            lbCantRegistros.Text = Convert.ToString(lvTickets.Items.Count());
        }
    }

    protected void lvTickets_PreRender(object sender, EventArgs e)
    {
        try
        {
            List<cPedido> pedidos = cPedido.Search(cbEmpresa.SelectedValue,
                                            string.IsNullOrEmpty(txtFechaDesde.Text) ? null : txtFechaDesde.Text,
                                            string.IsNullOrEmpty(txtFechaHasta.Text) ? null : txtFechaHasta.Text,
                                            "4", 0, (Int16)Categoria.Soporte, (Int16)Categoria.Desarollo, 0);
            lvTickets.DataSource = pedidos;
            lvTickets.DataBind();
            lbCantRegistros.Text = Convert.ToString(pedidos.Count());
        }
        catch (Exception ex) {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("PedidosAcobrar - " + DateTime.Now + "- " + ex.Message + " - lvlTickets_PreRender");
            return;
        }
    }

    protected void lnkDetalles_Click(object sender, EventArgs e)
    {
        LinkButton boton = (LinkButton)sender;
        Response.Redirect("DetallePedido.aspx?id=" + boton.CommandArgument.ToString(), false);
    }

    protected void btnVerTodas_Click(object sender, EventArgs e)
    {
        string url = "DetallePedido.aspx?idEmpresa=" + cbEmpresa.SelectedValue;
        url += "&fechaDesde=" + txtFechaDesde.Text;
        url += "&fechaHasta=" + txtFechaHasta.Text;
        url += "&idEstado=" + "4";
        url += "&page=0";

        Response.Redirect(url, false);
    }
}
