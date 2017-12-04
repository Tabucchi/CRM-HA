using Common.Logging;
using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class ListaOperacionVenta : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Administración)
                    pnlNuevoOV.Visible = true;
                    
                Cargar();
                CalcularTotales();
            }
        }

        #region Carga
        public void Cargar()
        {
            try
            {
                #region Combos
                cbEmpresa.DataSource = cEmpresa.GetDataTable();
                cbEmpresa.DataValueField = "id";
                cbEmpresa.DataTextField = "nombre";
                cbEmpresa.DataBind();
                ListItem ce = new ListItem("Seleccione un cliente...", "0");
                cbEmpresa.Items.Insert(0, ce);

                cbProyectos.DataSource = cProyecto.GetDataTable();
                cbProyectos.DataValueField = "id";
                cbProyectos.DataTextField = "descripcion";
                cbProyectos.DataBind();
                ListItem io = new ListItem("Seleccione una obra...", "0");
                cbProyectos.Items.Insert(0, io);
                cbProyectos.SelectedIndex = 0;

                cbEstado.DataSource = cOperacionVenta.CargarComboEstadoOV();
                cbEstado.DataBind();
                #endregion

                ListarTodos();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ListaOperacionVenta - " + DateTime.Now + "- " + ex.Message + " - Cargar");
                Response.Redirect("MensajeError.aspx");
            }
        }

        public void ListarTodos()
        {
            lvOperacionVenta.DataSource = cOperacionVenta.GetOperacionesVenta();
            lvOperacionVenta.DataBind();
        }
        #endregion

        #region Filtro
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<cOperacionVenta> ovs = cOperacionVenta.Search(cbEmpresa.SelectedValue, cbProyectos.SelectedValue, (cbEstado.SelectedIndex - 1).ToString());
                lvOperacionVenta.DataSource = ovs;
                lvOperacionVenta.DataBind();
                CalcularTotales();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ListaOperacionVenta - " + DateTime.Now + "- " + ex.Message + " - btnBuscar_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            try
            {
                IniciarFiltro();
                ListarTodos();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ListaOperacionVenta - " + DateTime.Now + "- " + ex.Message + " - btnVerTodos_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }

        public void IniciarFiltro()
        {
            cbEmpresa.SelectedValue = "0";
            cbProyectos.SelectedValue = "0";
        }
        #endregion

        #region Botones
        protected void btnNuevoOV_Click(object sender, EventArgs e)
        {
            Response.Redirect("OperacionVenta.aspx");
        }
        #endregion

        #region Listview
        protected void lvOperacionVenta_LayoutCreated(object sender, EventArgs e)
        {
            try
            {
                string estado = (cbEstado.SelectedIndex - 1).ToString();
                if(estado == "-1")
                    estado = Convert.ToString((Int16)estadoOperacionVenta.Activo);

                /*List<cOperacionVenta> ovs = cOperacionVenta.Search(cbEmpresa.SelectedValue, cbProyectos.SelectedValue, estado);
                if (ovs != null && ovs.Count != 0)
                    CalcularTotales(ovs);*/
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ListaOperacionVenta - " + DateTime.Now + "- " + ex.Message + " - lvOperacionVenta_LayoutCreated" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Métodos Auxiliares
        private void CalcularTotales()
        {
            try
            {
                //if (ovs.Count > 0)
                //{
                    //Label lblPrecioBase = (Label)lvOperacionVenta.FindControl("lbPrecioBase");
                    //lblPrecioBase.Text = cOperacionVenta.GetTotalPrecioBase(ovs);

                    //Label lblPrecioAcordado = (Label)lvOperacionVenta.FindControl("lbPrecioAcordado");
                    //lblPrecioAcordado.Text = cOperacionVenta.GetTotalPrecioAcordado(ovs);

                    //Label lblPrecioAcordadoPesos = (Label)lvOperacionVenta.FindControl("lbPrecioAcordadoPesos");
                    //lblPrecioAcordadoPesos.Text = cOperacionVenta.GetTotalPrecioAcordado(ovs);


                    decimal _totalPrecioAcordado = 0;
                    decimal _totalPrecioAcordadoPesos = 0;

                    foreach (ListViewItem item in lvOperacionVenta.Items)
                    {
                        //Label lbPrecioAcordado = item.FindControl("lbPrecioAcordado") as Label;
                        Label lbPrecioAcordadoPesos = item.FindControl("lbPrecioAcordadoPesos") as Label;

                        //_totalPrecioAcordado += Convert.ToDecimal(lbPrecioAcordado.Text);
                        _totalPrecioAcordadoPesos += Convert.ToDecimal(lbPrecioAcordadoPesos.Text);
                    }

                    //Label lblTotalPrecioAcordado = (Label)lvOperacionVenta.FindControl("lbTotalPrecioAcordado");
                    Label lblTotalPrecioAcordadoPesos = (Label)lvOperacionVenta.FindControl("lbTotalPrecioAcordadoPesos");

                    //lblTotalPrecioAcordado.Text = String.Format("{0:#,#0.00}", _totalPrecioAcordado);
                    lblTotalPrecioAcordadoPesos.Text = String.Format("{0:#,#0.00}", _totalPrecioAcordadoPesos);
                //}
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("ListaOperacionVenta - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion
    }
}