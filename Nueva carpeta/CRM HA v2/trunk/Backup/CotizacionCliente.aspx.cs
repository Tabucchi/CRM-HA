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

public partial class CotizacionCliente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string id = cCompra.GetCodigoCompraById(Request["codigo"].ToString());
        cCompra compra = cCompra.Load(id, null); 
        
        lbNroCompra.Text = compra.Id;
        lbEmpresa.Text = compra.IdEmpresa;
        lbCliente.Text = compra.IdCliente;
        if (compra.IdPedido != "0") //si tiene un pedido asociado muestra el nro del Pedido.
        {
            lbTituloTicket.Visible = true;
            lbTicket.Visible = true;
        }

        lvNuevaCompra.DataSource = cItem.GetItems(compra.Id);
        lvNuevaCompra.DataBind();
                
        if (compra.Iva == 0)
            lbmasIva.Visible = true;
        else
            lbIvaIncluido.Visible = true;

        lbTotal.Text = compra.TotalCliente;
        lbFecha.Text = String.Format("{0:d/M/yyyy}", compra.Fecha);
    }

    #region Botones
    protected void btnAprobado_Click(object sender, EventArgs e)
    {
        string id = cCompra.GetCodigoCompraById(Request["codigo"].ToString());
        cCompra compra = cCompra.Load(id, null);

        compra.IdEstado = Convert.ToInt16(EstadoCompraNombre.Aprobado).ToString();
        btnAprobado.Enabled = false;
        btnRechazar.Enabled = false;
        lbMensajeAprobado.Visible = true;

        double totalP = Convert.ToDouble(compra.TotalProveedor);
        double totalC = Convert.ToDouble(compra.TotalCliente);

        compra.TotalProveedor = totalP.ToString();
        compra.TotalCliente = totalC.ToString();
                
        compra.Save();

        // Envio correo
        cSendMail mail = new cSendMail();
        mail.RespuestaCotizacion(compra, "0"); //si el segundo parametro es 0, en el mail aparece que la compra fue aprobada
    }

    protected void btnRechazar_Click(object sender, EventArgs e)
    {
        string id = cCompra.GetCodigoCompraById(Request["codigo"].ToString());
        cCompra compra = cCompra.Load(id, null);

        compra.IdEstado = Convert.ToInt16(EstadoCompraNombre.Rechazado).ToString();
        btnAprobado.Enabled = false;
        btnRechazar.Enabled = false;
        lbMensajeRechazada.Visible = true;

        double totalP = Convert.ToDouble(compra.TotalProveedor);
        double totalC = Convert.ToDouble(compra.TotalCliente);

        compra.TotalProveedor = totalP.ToString();
        compra.TotalCliente = totalC.ToString();

        compra.Save();

        // Envio correo
        cSendMail mail = new cSendMail();
        mail.RespuestaCotizacion(compra, "1"); //si el segundo parametro es distinto a 0, en el mail aparece que la compra fue aprobada
    }
    #endregion
    
}
