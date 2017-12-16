using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

public partial class _Default : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Chequeo si se logueo un cliente sino un usuario de NAEX.
            if (System.Convert.ToInt32(Session["IdCliente"]) > 0)
                Response.Redirect("Clientes.aspx");

            //Mis Pedidos
            lvMisPedidos.DataSource = cPedido.GetMisPedidos(Convert.ToString(Session["IdUsuario"]));
            lvMisPedidos.DataBind();

            lbCantRegistros.Text = Convert.ToString(lvMisPedidos.Items.Count());

            //Vencimientos
            lvVencimientos.DataSource = cPedido.GetPedidosVencidos();
            lvVencimientos.DataBind();

            //Compras
            if (Session["IdUsuario"] != "" && Session["IdUsuario"] != null)
            {
                if (cUsuario.Load(Convert.ToString(Session["IdUsuario"])).IdCategoria == 1)
                {
                    lvCompras.DataSource = cCompra.SearchCompra(null, null, null, null, null, 0, null); //si es administrador ve todas las compras
                    lvCompras.DataBind();
                }
                else
                {
                    lvCompras.DataSource = cCompra.SearchCompra(null, null, null, null, null, 0, Convert.ToString(Session["IdUsuario"])); //si no es administrador ve solamente sus compras
                    lvCompras.DataBind();
                }
            }
        }
    }

    protected void lnkDetalles_Click(object sender, EventArgs e)
    {
        LinkButton boton = (LinkButton)sender;
        Response.Redirect("DetallePedido.aspx?id=" + boton.CommandArgument.ToString(), false);
    }
        
}
