using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace crm
{
    public partial class IngresarPedidoCliente : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["IdCliente"]) >= 0)
            {
                if (!IsPostBack)
                {
                    ListItem ic = new ListItem("Seleccione una categoría...", "0");
                    cbCategoria.DataSource = cCampoGenerico.GetDataTable(Tablas.tCategoria);
                    cbCategoria.DataValueField = "id";
                    cbCategoria.DataTextField = "descripcion";
                    cbCategoria.DataBind();
                    cbCategoria.Items.Insert(0, ic);
                    cbCategoria.SelectedIndex = 0;

                    cbPrioridad.DataSource = cCampoGenerico.GetDataTable(Tablas.tPrioridad);
                    cbPrioridad.DataValueField = "id";
                    cbPrioridad.DataTextField = "descripcion";
                    cbPrioridad.DataBind();
                }
            }
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                cSendMail send = new cSendMail();

                // Chequeo que el cliente tenga un mail registrado.
                if (string.IsNullOrEmpty(cCliente.Load(Convert.ToString(Session["IdCliente"])).Mail))
                    Response.Redirect("Mensaje.aspx?text=noMail");

                // Creo y guardo el ticket.
                cPedido pedido = new cPedido(cCliente.Load(Convert.ToString(Session["IdCliente"])).Id, // Id de usuario CRM.
                                             cCliente.Load(Convert.ToString(Session["IdCliente"])).Id,
                                             txtTitulo.Text,
                                             txtDescripcion.Text,
                                             Convert.ToInt16(cbCategoria.SelectedValue),
                                             cbPrioridad.SelectedValue,
                                             "",
                                             -1,
                                             "");

                pedido.Id = Convert.ToString(pedido.Save());

                //// Envio mail a Cliente + Soporte.
                send.CrearFinalizarTicket(pedido);

                Response.Redirect("Clientes.aspx");
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Agenda - " + DateTime.Now + "- " + ex.Message + " - ListPager_PreRender");
                return;
            }
        }
    }
}