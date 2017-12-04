using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using log4net;

public partial class IngresoPedido : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            cbClientes.DataSource = cCliente.GetListaClientes();
            cbClientes.DataValueField = "id";
            cbClientes.DataTextField = "nombre";
            cbClientes.DataBind();

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

            ListItem i = new ListItem("Seleccione un Responsable...", "0");
            cbResponsable.DataSource = cUsuario.GetDataTable();
            cbResponsable.DataValueField = "id";
            cbResponsable.DataTextField = "nombre";
            cbResponsable.DataBind();
            cbResponsable.Items.Insert(0, i);
            cbResponsable.SelectedIndex = 0;

            cbEmpresa.DataSource = cEmpresa.GetEmpresas();
            cbEmpresa.DataValueField = "id";
            cbEmpresa.DataTextField = "nombre";
            cbEmpresa.DataBind();
        }
        
        SetPermisosUsuario();
    }

    protected void btnCargar_Click(object sender, EventArgs e)
    {
        cSendMail send = new cSendMail();
        cCliente cliente;

        //Parseo el string de cliente para conseguir Nombre de Cliente y Nombre de Empresa por separado.
        try {
           string[] txt = txtClientes.Text.Split('(');
           string nombreCliente = txt[0].Trim();
           string nombreEmpresa = txt[1].Trim(')');
           cliente = cCliente.SearchByNombreAndEmpresa(nombreCliente, nombreEmpresa);                   
       
        // Chequeo que el cliente tenga un mail registrado.
        if (string.IsNullOrEmpty(cliente.Mail))
            Response.Redirect("Mensaje.aspx?text=noMail");       

        // Creo y guardo el ticket.
        cPedido pedido = new cPedido(Session["IdUsuario"].ToString(),
                                     cliente.Id,
                                     txtTitulo.Text,
                                     txtDescripcion.Text,
                                     Convert.ToInt16(cbCategoria.SelectedValue),
                                     cbPrioridad.SelectedValue,
                                     txtFecha.Text,
                                     Convert.ToInt16(cbResponsable.SelectedValue),
                                     txtMensajeResponsable.Text);

        pedido.Id = Convert.ToString(pedido.Save());

        #region Asignacion de Ticket
        if (cbResponsable.SelectedValue != "0")
        {
            cAsignacionResponsable a = new cAsignacionResponsable();
            a.IdResponsable = cUsuario.Load(cbResponsable.SelectedValue).Id;
            a.IdAsigno = cUsuario.Load(cbResponsable.SelectedValue).Id;
            a.Comentario = "";
            a.IdPedido = pedido.Id;
            a.Fecha = DateTime.Now;
            a.Save();
        }
        #endregion

        // Envio mail a Cliente + Soporte.
        send.CrearFinalizarTicket(pedido);

        // Envio mail a Responsable.
        if (cbResponsable.SelectedIndex > 0)
            send.AsignarPedido(pedido);

        Response.Redirect("Mensaje.aspx?text=" + pedido.Id);
        }
        catch (Exception ex)
        {
            txtClientes.Text = "";

            log4net.Config.XmlConfigurator.Configure();
            log.Error("IngresoPedido - " + DateTime.Now + "- " + ex.Message + " - btnCargar_Click");
        }
    }

    public void SetPermisosUsuario()
    {
        if(cUsuario.Load(Convert.ToString(Session["IdUsuario"])).IdCategoria!=1)
        {
            txtMensajeResponsable.Visible = false;
            lbInfoAdicionalResponsable.Visible = false;
            lbInfoAdicionalMensaje.Visible = false;
        }
    }

    protected void btnAceptarCliente_Click(object sender, EventArgs e)
    {
        cCliente cliente = new cCliente();
        cliente.Nombre = txtNombreCliente.Text;
        cliente.IdEmpresa = cbEmpresa.SelectedValue;
        cliente.Interno = txtInternoCliente.Text;
        cliente.Mail = txtMailCliente.Text;
        cliente.Password = "";
        cliente.Autorizacion = (Int16)Autorizaciones.No_Autorizado;

        cliente.Ip = "";
        cliente.UsuarioRed = "";
        cliente.PasswordRed = "";
        cliente.ClaveSistema = "";
        cliente.Papelera = (Int16)papelera.Activo;
        
        cliente.Save();
    }
 
}