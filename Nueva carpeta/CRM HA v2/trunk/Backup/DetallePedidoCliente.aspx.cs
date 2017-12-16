using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using log4net;

public partial class DetallePedidoCliente : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = "";
               
        try {
            if (Request["id"] != null) {
                id = Request["id"].ToString();
            }            
            
            // Validar si el id es valido y mostrar un mensaje si no existe
            cPedido pedido = cPedido.Load(id);
            lblID.Text = "ID " + pedido.Id;

            //Lo guardo en el campo Hidden
            txtId.Value = pedido.Id;

            lblEstado.Text = pedido.GetEstado;

            //Si el estado es Nuevo, se habilita la opción de Modificar Pedido.
            //Si el estado es Finalizado, se deshabilita el comentario
            if (pedido.GetEstado == Convert.ToString(Estado.Finalizado))
            {
                pnlComentario.Enabled = false;
                txtWater.WatermarkText = " ";
            }
            
            lblFechaCarga.Text = pedido.Fecha.ToLongDateString() + " a las " + pedido.Fecha.ToShortTimeString() + "·";

            lblCliente.Text = pedido.GetCliente().Nombre;
            lblTitulo.Text = pedido.Titulo;
            lblCategoria.Text = pedido.GetCategoria;
            lblPrioridad.Text = pedido.GetPrioridad;
            
            // Estados
            switch (pedido.GetEstado)
            {
                case "Nuevo" :
                    lblFechaFinalizacion.Text = "-";
                    break;
                case "Pendiente" :
                    lblFechaFinalizacion.Text = "-";
                    break;
                case "Finalizado" :
                    //lblFechaFinalizacion.Text = pedido.GetComentarioLast().Fecha.ToLongDateString() + " a las " + pedido.GetComentarioLast().Fecha.ToShortTimeString() + "·";
                    break;
                default :
                    lblFechaFinalizacion.Text = "-";
                    break;
            }
            
            // Comentarios
            List<cComentario> comentarios = cComentario.GetList(id, true);
            
            if (comentarios.Count > 0)
            {
                rptComentarios.DataSource = comentarios;
                rptComentarios.DataBind();
            }
        }
        catch(Exception ex)
        {
            lblID.Text = "ID ERRONEO";
            lblEstado.Text = "-";
            lblFechaCarga.Text = "-";

            lblCliente.Text = "-";
            lblTitulo.Text = "-";
            lblCategoria.Text = "-";
            lblPrioridad.Text = "-";

            lblFechaFinalizacion.Text = "-";

            log4net.Config.XmlConfigurator.Configure();
            log.Error("DetallePedidoCliente - " + DateTime.Now + "- " + ex.Message + " - Page_Load");
            return;
        }

        try
        {
            // Deshabilitación de funciones si el estado es FINALIZADO.
            if (lblEstado.Text == Estado.Finalizado.ToString())
                btnAgregarComentario.Enabled = false;
            else
                btnAgregarComentario.Enabled = true;
        }
        catch (Exception ex) {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("DetallePedidoCliente - " + DateTime.Now + "- " + ex.Message + " - Page_Load");
            return;
        }
    }

    protected void btnAgregarComentario_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtComentario.Text))
            return;

        try
        {
            cPedido pedido = cPedido.Load(txtId.Value);
            pedido.Comentar(Session["IdCliente"].ToString(), "cCliente", txtComentario.Text, true, true);
        }
        catch(Exception ex) {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("DetallePedidoCliente - " + DateTime.Now + "- " + ex.Message + " - btnAgregarComentario_Click");
            return;
        }

        if (Request["id"] != null)
            Response.Redirect("DetallePedidoCliente.aspx?id=" + Request["id"].ToString(), false);
    }

    protected void tempo_Click(object sender, EventArgs e)
    {
        return;
    }

    protected void btnNuevoCliente_Click(object sender, EventArgs e)
    {
        Response.Redirect("Clientes.aspx");
    }
}
