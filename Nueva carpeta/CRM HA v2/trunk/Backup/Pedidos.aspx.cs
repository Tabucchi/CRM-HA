using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using log4net;
using System.Xml;

public partial class Pedidos : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                #region Combos
                cbEmpresa.DataSource = cEmpresa.GetListaEmpresas();
                cbEmpresa.DataValueField = "id";
                cbEmpresa.DataTextField = "nombre";
                cbEmpresa.DataBind();
                //Agrego item TODAS
                ListItem it = new ListItem("Todas", "0");
                cbEmpresa.Items.Insert(0, it);

                cbEstado.DataSource = cCampoGenerico.GetDataTable(Tablas.tEstado);
                cbEstado.DataValueField = "id";
                cbEstado.DataTextField = "descripcion";
                cbEstado.DataBind();

                //Agrego item TODOS y selecciono NUEVOS
                it = new ListItem("Todos", "-1");
                cbEstado.Items.Insert(0, it);
                cbEstado.SelectedIndex = 1;
                #endregion

                cbSoporte.Checked = false;
                cbDesarrollo.Checked = false;
                cbTelecomunicaciones.Checked = false;

                cUsuario usuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"]));
                Int16 admin = 0, soporte = 0, desarrollo = 0, telco = 0;

                switch (usuario.IdCategoria)
                {
                    case (Int16)Categoria.Administración:
                        admin = (Int16)Categoria.Administración;
                        break;
                    case (Int16)Categoria.Soporte:
                        cbSoporte.Checked = true;
                        break;
                    case (Int16)Categoria.Desarollo:
                        cbDesarrollo.Checked = true;
                        break;
                    case (Int16)Categoria.Telecomunicaciones:
                        cbTelecomunicaciones.Checked = true;
                        break;
                }

                if (admin == 0)
                {
                    if (cbSoporte.Checked != true) soporte = (Int16)Categoria.Soporte;
                    if (cbDesarrollo.Checked != true) desarrollo = (Int16)Categoria.Desarollo;
                    if (cbTelecomunicaciones.Checked != true) telco = (Int16)Categoria.Telecomunicaciones;
                }
                else
                {
                    cbSoporte.Checked = true;
                    cbDesarrollo.Checked = true;
                    cbTelecomunicaciones.Checked = true;

                }

                lvTickets.DataSource = cPedido.GetPedidosPendientes(admin, soporte, desarrollo, telco);
                lvTickets.DataBind();

                lbCantRegistros.Text = Convert.ToString(lvTickets.Items.Count);

               /* if (diffDiasFecha(VerFechaMail()) > 15)
                {
                    lbMensajeMail.CssClass = "tituloMensaje";
                    lbMensajeMail.Text = "Último mail: " + VerFechaMail();
                }else
                    lbMensajeMail.Text = "Último mail: " + VerFechaMail();*/
            }
        }
        catch (Exception ex) {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Pedidos - " + DateTime.Now + "- " + ex.Message + " - Page_Load" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            Int16 soporte = 0, desarrollo = 0, telco = 0;
            //Explicación clase: En la clase cPedidoDAO, metodo GetPedidosPendientes.
            if (cbSoporte.Checked != true) soporte = (Int16)Categoria.Soporte;
            if (cbDesarrollo.Checked != true) desarrollo = (Int16)Categoria.Desarollo;
            if (cbTelecomunicaciones.Checked != true) telco = (Int16)Categoria.Telecomunicaciones;

            List<cPedido> pedidos = cPedido.Search(cbEmpresa.SelectedValue,
                                            string.IsNullOrEmpty(txtFechaDesde.Text) ? null : txtFechaDesde.Text,
                                            string.IsNullOrEmpty(txtFechaHasta.Text) ? null : txtFechaHasta.Text,
                                            cbEstado.SelectedValue,
                                            (Int16)Categoria.Administración, soporte, desarrollo, telco);
            lvTickets.DataSource = pedidos;
            lvTickets.DataBind();
            lbCantRegistros.Text = Convert.ToString(pedidos.Count());
        }
        catch (Exception ex) {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("Pedidos - " + DateTime.Now + "- " + ex.Message + " - btnBuscar_Click" + " - " + cUsuario.Load(Session["IdUsuario"].ToString()).Nombre);
            Response.Redirect("MensajeError.aspx");
        }
     }

    protected void lnkDetalles_Click(object sender, EventArgs e)
    {
        LinkButton boton = (LinkButton)sender;
        Response.Redirect("DetallePedido.aspx?id=" + boton.CommandArgument.ToString(), false);
    }

    protected void btnVerTodas_Click(object sender, EventArgs e)
    {
        Response.Redirect("Pedidos.aspx");
    }

    protected void lvTickets_PreRender(object sender, EventArgs e)
    {
        //List<cPedido> pedidos = cPedido.Search(cbEmpresa.SelectedValue,
        //                                string.IsNullOrEmpty(txtFechaDesde.Text) ? null : txtFechaDesde.Text,
        //                                string.IsNullOrEmpty(txtFechaHasta.Text) ? null : txtFechaHasta.Text,
        //                                cbEstado.SelectedValue);
        //lvTickets.DataSource = pedidos;
        //lvTickets.DataBind();
        //lbCantRegistros.Text = Convert.ToString(pedidos.Count());
    }

    #region CheckBox
    protected void CheckBox()
    {
        Int16 admin = 0, soporte = 0 , desarrollo = 0, telco = 0;

        //Explicación clase: En la clase cPedidoDAO, metodo GetPedidosPendientes.
        if (cbSoporte.Checked != true) soporte = (Int16)Categoria.Soporte;
        if (cbDesarrollo.Checked != true) desarrollo = (Int16)Categoria.Desarollo;
        if (cbTelecomunicaciones.Checked != true) telco = (Int16)Categoria.Telecomunicaciones;
        if (cbSoporte.Checked != true && cbDesarrollo.Checked != true && cbTelecomunicaciones.Checked != true) admin = 1;

        lvTickets.DataSource = cPedido.GetPedidosPendientes(admin, soporte, desarrollo, telco);
        lvTickets.DataBind();
        lbCantRegistros.Text = Convert.ToString(cPedido.GetPedidosPendientes(admin, soporte, desarrollo, telco).Count);
    }

    protected void cbSoporte_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox();
    }

    protected void cbDesarrollo_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox();
    }

    protected void cbTelecomunicaciones_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox();        
    }
    #endregion

    public DateTime VerFechaMail(){
        //string pathXML = "C:\\PUBLICACIONES\\CRM NAEX v5\\xml\\fechaMail.xml";
        string pathXML = "C:\\Users\\ntabucchi\\Documents\\Proyectos\\NAEX\\trunk\\xml\\fechaMail.xml";

        XmlTextReader reader = new XmlTextReader(pathXML);
        string name = "";
        DateTime xmlFecha = new DateTime();
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element: // The node is an element.
                    name = reader.Name;
                    break;
                case XmlNodeType.Text: //Display the text in each element.                    
                    if (name == "fecha")
                    {
                        xmlFecha = Convert.ToDateTime(reader.Value);
                    }
                    break;
            }
        }
        reader.Close();

        return xmlFecha; 
    }

    private int diffDiasFecha(DateTime fechaPasada)
    {
        //DateTime fActual                                     - fechaPedido
        TimeSpan diffFechas = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day) - fechaPasada;
        return diffFechas.Days;
    }
}
