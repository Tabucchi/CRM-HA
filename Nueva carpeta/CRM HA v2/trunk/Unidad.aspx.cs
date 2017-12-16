using DLL;
using DLL.Negocio;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class Unidad : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    #region Combo
                    cbFiltroEstado.DataSource = cCampoGenerico.GetListaEstadoUnidad();
                    cbFiltroEstado.DataValueField = "id";
                    cbFiltroEstado.DataTextField = "descripcion";
                    cbFiltroEstado.DataBind();
                    ListItem es = new ListItem("Todos", "0");
                    cbFiltroEstado.Items.Insert(0, es);
                    ListItem dr = new ListItem("Disponible / Reservado", "4");
                    cbFiltroEstado.Items.Insert(4, dr);

                    cbFiltroUnidad.DataSource = cCampoGenerico.GetListaTipoUnidad();
                    cbFiltroUnidad.DataValueField = "id";
                    cbFiltroUnidad.DataTextField = "descripcion";
                    cbFiltroUnidad.DataBind();
                    ListItem tu = new ListItem("Todas", "0");
                    cbFiltroUnidad.Items.Insert(0, tu);

                    cbFiltroAmbiente.DataSource = cUnidad.CargarComboAmbiente();
                    cbFiltroAmbiente.DataBind();
                    #endregion

                    lbProyecto.Text = cProyecto.Load(Request["idProyecto"].ToString()).Descripcion;

                    //Aviso de actualización de precios
                    List<cActualizarPrecio> ap = cActualizarPrecio.GetActualizacionByIdProyecto(Request["idProyecto"].ToString());
                    if (ap.Count > 0)
                        pnlMensajePrecios.Visible = true;

                    #region Última actualización
                    cHistorial historial = cHistorial.LoadByIdProyecto(Request["idProyecto"].ToString());
                    if (historial != null)
                    {
                        lbHistorialFecha.Text = String.Format("{0:f}", historial.Fecha);
                        lbHistorialMotivo.Text = historial.Motivo;
                    }
                    else
                    {
                        lbHistorialFecha.Text = "-";
                        lbHistorialMotivo.Text = "-";
                    }
                    #endregion

                    ListarTodos();
                    CalcularTotales();
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - Page_Load" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        #region ListView
        protected void lvUnidades_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "Editar":
                        {
                            cUnidad unidad = cUnidad.Load(e.CommandArgument.ToString());
                            hfId.Value = unidad.Id;
                            txtEditCodUF.Text = unidad.CodigoUF;

                            #region Combo
                            ddlEditUnidadFuncional.DataSource = cCampoGenerico.GetListaTipoUnidad();
                            ddlEditUnidadFuncional.DataValueField = "id";
                            ddlEditUnidadFuncional.DataTextField = "descripcion";
                            ddlEditUnidadFuncional.DataBind();
                            ListItem tu1 = new ListItem("Todas", "0");
                            ddlEditUnidadFuncional.Items.Insert(0, tu1);

                            if (unidad.IdEstado == Convert.ToString((Int16)estadoUnidad.Vendido_sin_boleto))
                            {
                                pnlEditEstadoCombo.Visible = true;
                                pnlEditEstado.Visible = false;

                                cbEditEstado.DataSource = cCampoGenerico.GetListaEstadoUnidad();
                                cbEditEstado.DataValueField = "id";
                                cbEditEstado.DataTextField = "descripcion";
                                cbEditEstado.DataBind();

                                cbEditEstado.SelectedValue = unidad.IdEstado;
                            }
                            else
                            {
                                pnlEditEstado.Visible = true;
                                pnlEditEstadoCombo.Visible = false;
                            }
                            #endregion

                            ddlEditUnidadFuncional.SelectedValue = unidad.GetUnidadFuncional.ToString();

                            txtEditNroUnidad.Text = unidad.NroUnidad;
                            txtEditNivel.Text = unidad.Nivel;
                            txtEditAmbiente.Text = unidad.Ambiente;
                            lbEditSupCubierta.Text = unidad.SupCubierta;
                            lbEditSupDescubierta.Text = unidad.SupDescubierta;
                            lbEditSupSemiDescubierta.Text = unidad.SupSemiDescubierta;
                            lbEditSupTotal.Text = unidad.SupTotal.ToString();
                            txtEditPrecioBase.Text = unidad.GetPrecioBase;
                            lbEstado.Text = unidad.GetEstado;
                            txtEditMoneda.Text = unidad.GetMoneda;
                            lbVendedor.Text = unidad.GetIdUsuario;

                            pnlValor.Visible = true;
                            pnlEditValor.Visible = false;
                            pnlEditCliente.Visible = false;

                            if (unidad.IdEstado == Convert.ToString((Int16)estadoUnidad.Vendido))
                            {
                                if (unidad.PrecioBase == 0) //Si esta vendido y con precio, solo se muestra el valor. Si esta vendido pero sin precio se habilita el textbox para cargarlo
                                {
                                    pnlValor.Visible = false;
                                    pnlEditValor.Visible = true;
                                }

                                pnlEditCliente.Visible = true; //Se habilita el listado de clientes para las unidades que se cargaron y estaban vendidas.
                                //Cliente
                                cbEditCliente.DataSource = cEmpresa.GetListaEmpresas();
                                cbEditCliente.DataValueField = "id";
                                cbEditCliente.DataTextField = "nombre";
                                cbEditCliente.DataBind();
                                //Agrego item TODAS
                                ListItem it = new ListItem("Seleccione un cliente...", "0");
                                cbEditCliente.Items.Insert(0, it);
                            }

                            modalEdit.Show();
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - lvUnidades_ItemCommand" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void lvUnidades_LayoutCreated(object sender, EventArgs e)
        {
            try
            {
                List<cUnidad> unidades = cUnidad.GetUnidadesByIdProyecto(Request["idProyecto"].ToString());
                if (unidades.Count > 0)
                {
                    if (Convert.ToDecimal(unidades[0].Moneda.ToString()) == (Int16)tipoMoneda.Dolar)
                        (lvUnidades.FindControl("lbMoneda") as Label).Text = "Dolar";
                    else
                        (lvUnidades.FindControl("lbMoneda") as Label).Text = "Peso";
                }
                /*List<cUnidad> unidades = cUnidad.GetUnidadesByIdProyecto(Request["idProyecto"].ToString());
                if (unidades != null && unidades.Count != 0)
                    CalcularTotales(unidades);*/
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - lvUnidades_LayoutCreated" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        
        protected void lvUnidades_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria != (Int32)eCategoria.Administración)
                    {
                        LinkButton edit = e.Item.FindControl("btnEditar") as LinkButton;
                        edit.Attributes.CssStyle.Add("visibility", "hidden");

                        pnlAlta.Visible = false;
                        pnlPrecio.Visible = false;
                        pnlEdit.Visible = false;
                        pnlDelete.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - lvUnidades_ItemDataBound" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Botones
        protected void btnNuevaUnidad_Click(object sender, EventArgs e)
        {
            if (hfSupTotalProyecto.Value != "100")
                Response.Redirect("CargaUnidad.aspx?idProyecto=" + Request["idProyecto"].ToString());
            else
            {
                pnlMensaje.Visible = true;
            }
        }

        protected void btnGuardarEdit_Click(object sender, EventArgs e)
        {
            try
            {
                cUnidad u = cUnidad.GetUnidadIguales(Request["idProyecto"].ToString(), txtEditNivel.Text, txtEditNroUnidad.Text, txtEditCodUF.Text);

                if (u == null)
                {
                    cUnidad unidad = cUnidad.Load(hfId.Value);
                    unidad.CodigoUF = txtEditCodUF.Text;
                    unidad.UnidadFuncional = cCampoGenerico.Load(ddlEditUnidadFuncional.SelectedValue, Tablas.tTipoUnidad).Descripcion;
                    unidad.NroUnidad = txtEditNroUnidad.Text;
                    unidad.Nivel = txtEditNivel.Text;
                    unidad.Ambiente = txtEditAmbiente.Text;

                    if (pnlEditValor.Visible == true)
                        unidad.PrecioBase = Convert.ToDecimal(txtEditPrecioBaseVendido.Text.Replace(".", ","));

                    if (pnlEditEstadoCombo.Visible == true)
                    {
                        unidad.IdEstado = cbEditEstado.SelectedValue;

                        if (cbEditEstado.SelectedValue == Convert.ToString((Int16)estadoUnidad.Disponible))
                        {
                            cEmpresaUnidad eu = cEmpresaUnidad.GetUnidad(unidad.CodigoUF, unidad.IdProyecto);

                            if (eu != null)
                            {
                                eu.Papelera = (Int16)papelera.Eliminado;
                                eu.Save();
                            }
                        }
                    }

                    unidad.Porcentaje = (Convert.ToDecimal(unidad.SupTotal) * 100) / cProyecto.Load(unidad.IdProyecto).SupTotal;

                    if (pnlEditCliente.Visible == true && cbEditCliente.SelectedValue != "0")
                    {
                        //Relacion Empresa-Unidad - CAMBIAR
                        cEmpresaUnidad empresaUnidad = new cEmpresaUnidad(cbEditCliente.SelectedValue, unidad.Id, unidad.CodigoUF, unidad.IdProyecto);
                        empresaUnidad.IdOv = "-1";
                        empresaUnidad.Save();
                        ListarTodos();
                    }

                    unidad.Save();
                    lbEditMensaje.Text = "";

                    modalEdit.Hide();

                    List<cUnidad> unidades = cUnidad.GetUnidadesByIdProyectoSinUnidadesModificadas(Request["idProyecto"].ToString());
                    lvUnidades.DataSource = unidades;
                    lvUnidades.DataBind();
                }
                else
                {
                    pnlMensajeError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - btnGuardarEdit_Click" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                cUnidad unidad = cUnidad.Load(hfId.Value);
                lbDeleteUnidad.Text = unidad.CodigoUF;
                modalDelete.Show();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - btnDelete_Click" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                cUnidad unidad = cUnidad.Load(hfId.Value);
                unidad.Papelera = (Int16)papelera.Eliminado;
                unidad.Save();

                Response.Redirect("Unidad.aspx?idProyecto=" + Request["idProyecto"].ToString());
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - btnEliminar_Click" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                var argument = ((Button)sender).CommandArgument;

                switch (argument)
                {
                    case "Editar":
                        modalEdit.Hide();
                        break;
                    case "Eliminar":
                        modalDelete.Hide();
                        break;
                }
                modalEdit.Hide();
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - btnCerrar_Click" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnActualizarPrecio_Click(object sender, EventArgs e)
        {
            Response.Redirect("ActualizarPrecio.aspx?idProyecto=" + Request["idProyecto"].ToString());
        }

        protected void lkbCargaModificacion_Click(object sender, EventArgs e)
        {
            Response.Redirect("CargaUnidad.aspx?idProyecto=" + Request["idProyecto"].ToString());
        }
        #endregion

        #region Filtro
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<cUnidad> unidades = cUnidad.Search(cbFiltro.SelectedValue, Request["idProyecto"].ToString(), cbFiltroEstado.SelectedValue, cbFiltroUnidad.SelectedValue, cbFiltroAmbiente.SelectedValue, cbFiltroSup.SelectedValue, txtMinM2.Text, txtMaxM2.Text, txtMinPrecio.Text, txtMaxPrecio.Text);
                lvUnidades.DataSource = unidades;
                lvUnidades.DataBind();

                lbCantUnidades.Text = unidades.Count().ToString() + " unidades";

                CalcularTotales();

                pnlMensaje.Visible = false;
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - btnBuscar_Click" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            IniciarFiltro();
            ListarTodos();
        }

        protected void cbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<cUnidad> unidades = cUnidad.Search(cbFiltro.SelectedValue, Request["idProyecto"].ToString(), cbFiltroEstado.SelectedValue, cbFiltroUnidad.SelectedValue, cbFiltroAmbiente.SelectedValue, cbFiltroSup.SelectedValue, txtMinM2.Text, txtMaxM2.Text, txtMinPrecio.Text, txtMaxPrecio.Text);
                lvUnidades.DataSource = unidades;
                lvUnidades.DataBind();

                lbCantUnidades.Text = unidades.Count + " unidades";
                pnlMensaje.Visible = false;
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - cbFiltro_SelectedIndexChanged" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Auxiliares
        public void ListarTodos()
        {
            try
            {
                List<cUnidad> unidades = cUnidad.GetUnidadesByIdProyectoSinUnidadesModificadas(Request["idProyecto"].ToString());
                lvUnidades.DataSource = unidades;
                lvUnidades.DataBind();

                if (unidades.Count > 0)
                {
                    if (Convert.ToDecimal(unidades[0].Moneda.ToString()) == (Int16)tipoMoneda.Dolar)
                        lbMoneda.Text = "Dolar";
                    else
                        lbMoneda.Text = "Peso";
                }
                else
                {
                    btnActualizarPrecio.Enabled = false;
                }

                pnlMensaje.Visible = false;
                lbCantUnidades.Text = unidades.Count + " unidades";

                decimal porcentaje = cUnidad.GetTotalPorcentaje(unidades);
                Label lblPorcentaje = (Label)lvUnidades.FindControl("lbPorcentaje");
                if (lblPorcentaje != null)
                {
                    lblPorcentaje.Text = porcentaje.ToString();
                    hfSupTotalProyecto.Value = porcentaje.ToString();
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - ListarTodos" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        public void IniciarFiltro()
        {
            cbFiltroEstado.SelectedValue = "0";
            cbFiltroUnidad.SelectedValue = "0";
            cbFiltroSup.SelectedValue = "supTotal";
            cbFiltroAmbiente.SelectedValue = "Todos";
            txtMinM2.Text = "";
            txtMaxM2.Text = "";
            txtMinPrecio.Text = "";
            txtMaxPrecio.Text = "";
        }

        //public void CalcularTotales(List<cUnidad> unidades)
        public void CalcularTotales()
        {
            try
            {
                /*if (unidades.Count > 0)
                {*/
                    /*Label lblSupCubierta = (Label)lvUnidades.FindControl("lbSupCubierta");
                    lblSupCubierta.Text = cUnidad.GetTotalSupCubierta(unidades);

                    Label lblSupSemiDescubierta = (Label)lvUnidades.FindControl("lbSupBalcon");
                    lblSupSemiDescubierta.Text = cUnidad.GetTotalSupSemiDescubierta(unidades);

                    Label lblSupDescubierta = (Label)lvUnidades.FindControl("lbSupTerraza");
                    lblSupDescubierta.Text = cUnidad.GetTotalSupDescubierta(unidades);

                    Label lblSupTotal = (Label)lvUnidades.FindControl("lbSupTotal");
                    lblSupTotal.Text = cUnidad.GetTotalSupTotal(unidades);

                    decimal porcentaje = cUnidad.GetTotalPorcentaje(unidades);
                    Label lblPorcentaje = (Label)lvUnidades.FindControl("lbPorcentaje");
                    lblPorcentaje.Text = porcentaje.ToString();
                    hfSupTotalProyecto.Value = porcentaje.ToString();
                    
                    Label lblPrecioBase = (Label)lvUnidades.FindControl("lbPrecioBase");
                    lblPrecioBase.Text = cUnidad.GetTotalPrecioBase(unidades);*/







                    decimal _totalSupCubierta = 0;
                    decimal _totalSupSemiDescubierta = 0;
                    decimal _totalSupDescubierta = 0;
                    decimal _totalSupTotal = 0;
                    decimal _porcentaje = 0;
                    decimal _total = 0;

                    foreach (ListViewItem item in lvUnidades.Items)
                    {
                        Label lblSupCubierta = item.FindControl("lbSupCubierta") as Label;
                        if (!string.IsNullOrEmpty(lblSupCubierta.Text))
                            _totalSupCubierta += Convert.ToDecimal(lblSupCubierta.Text);

                        Label lblSupSemiDescubierta = item.FindControl("lbSupSemiDescubierta") as Label;
                        if (!string.IsNullOrEmpty(lblSupSemiDescubierta.Text))
                            _totalSupSemiDescubierta += Convert.ToDecimal(lblSupSemiDescubierta.Text);

                        Label lblSupDescubierta = item.FindControl("lbSupDescubierta") as Label;
                        if (!string.IsNullOrEmpty(lblSupDescubierta.Text))
                            _totalSupDescubierta += Convert.ToDecimal(lblSupDescubierta.Text);

                        Label lblSupTotal = item.FindControl("lbSupTotal") as Label;
                        if (!string.IsNullOrEmpty(lblSupTotal.Text))
                            _totalSupTotal += Convert.ToDecimal(lblSupTotal.Text);

                        Label lblPorcentaje = item.FindControl("lbPorcentaje") as Label;
                        if (!string.IsNullOrEmpty(lblPorcentaje.Text))
                            _porcentaje += Convert.ToDecimal(lblPorcentaje.Text);

                        Label lbPrecio = item.FindControl("lbPrecioBase") as Label;
                        if (!string.IsNullOrEmpty(lbPrecio.Text))
                        {
                            if (cAuxiliar.IsNumeric(lbPrecio.Text))
                                _total += Convert.ToDecimal(lbPrecio.Text);
                        }
                    }

                    Label lblTotalSupCubierta = (Label)lvUnidades.FindControl("lbTotalSupCubierta");
                    if (lblTotalSupCubierta != null)
                        lblTotalSupCubierta.Text = String.Format("{0:#,#0.00}", _totalSupCubierta);

                    Label lblTotalSupSemiDescubierta = (Label)lvUnidades.FindControl("lbTotalSupBalcon");
                    if (lblTotalSupSemiDescubierta != null)
                        lblTotalSupSemiDescubierta.Text = String.Format("{0:#,#0.00}", _totalSupSemiDescubierta);

                    Label lblTotalSupDescubierta = (Label)lvUnidades.FindControl("lbTotalSupTerraza");
                    if (lblTotalSupDescubierta != null)
                        lblTotalSupDescubierta.Text = String.Format("{0:#,#0.00}", _totalSupDescubierta);

                    Label lblTotalSupTotal = (Label)lvUnidades.FindControl("lbTotalSupTotal");
                    if (lblTotalSupTotal != null)    
                        lblTotalSupTotal.Text = String.Format("{0:#,#0.00}", _totalSupTotal);

                    Label lblTotalPorcentaje = (Label)lvUnidades.FindControl("lbTotalPorcentaje");
                    if (lblTotalPorcentaje != null)
                    {
                        lblTotalPorcentaje.Text = String.Format("{0:#,#0.00}", _porcentaje);
                        hfSupTotalProyecto.Value = _porcentaje.ToString();
                    }

                    Label lblPrecioBase = (Label)lvUnidades.FindControl("lbTotalPrecioBase");
                    if (lblPrecioBase != null)
                        lblPrecioBase.Text = String.Format("{0:#,#0.00}", _total);
                //}
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("Unidad - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Reportes
        private DataSet CrearDataSet()
        {
            List<cUnidad> unidades = cUnidad.Search(cbFiltro.SelectedValue, Request["idProyecto"].ToString(), cbFiltroEstado.SelectedValue, cbFiltroUnidad.SelectedValue, cbFiltroAmbiente.SelectedValue, cbFiltroSup.SelectedValue, txtMinM2.Text, txtMaxM2.Text, txtMinPrecio.Text, txtMaxPrecio.Text);

            ArrayList seleccionados = new ArrayList();

            foreach (cUnidad p in unidades)
            {
                cUnidad pe = cUnidad.Load(p.Id);
                seleccionados.Add(pe);
            }

            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("codUF"));
            dt.Columns.Add(new DataColumn("unidadFuncional"));
            dt.Columns.Add(new DataColumn("nivel"));
            dt.Columns.Add(new DataColumn("nroUnidad"));
            dt.Columns.Add(new DataColumn("ambiente"));
            dt.Columns.Add(new DataColumn("supCubierta"));
            dt.Columns.Add(new DataColumn("supSemiDescubierta"));
            dt.Columns.Add(new DataColumn("supDescubierta"));
            dt.Columns.Add(new DataColumn("supTotal"));
            dt.Columns.Add(new DataColumn("porcentaje"));
            dt.Columns.Add(new DataColumn("GetPrecioBase"));
            dt.Columns.Add(new DataColumn("valorM2"));
            dt.Columns.Add(new DataColumn("Cliente"));

            foreach (cUnidad p in unidades)
            {
                dr = dt.NewRow();
                dr["codUF"] = p.CodigoUF;
                dr["unidadFuncional"] = p.UnidadFuncional;
                dr["nivel"] = p.Nivel;
                dr["nroUnidad"] = p.NroUnidad;
                dr["ambiente"] = p.Ambiente;
                dr["supCubierta"] = p.SupCubierta;
                dr["supSemiDescubierta"] = p.SupSemiDescubierta;
                dr["supDescubierta"] = p.SupDescubierta;
                dr["supTotal"] = p.SupTotal;
                dr["porcentaje"] = p.Porcentaje;
                if (p.IdEstado != Convert.ToString((Int16)estadoUnidad.Vendido) && p.IdEstado != Convert.ToString((Int16)estadoUnidad.Vendido_sin_boleto))
                    dr["GetPrecioBase"] = p.GetPrecioBase;
                else
                    dr["GetPrecioBase"] = "-";
                if (p.IdEstado == Convert.ToString((Int16)estadoUnidad.Vendido) || p.IdEstado == Convert.ToString((Int16)estadoUnidad.Vendido_sin_boleto))
                    dr["valorM2"] = "-";
                else
                    dr["valorM2"] = p.ValorM2;

                if (p.IdEstado != Convert.ToString((Int16)estadoUnidad.Vendido_sin_boleto))
                    dr["Cliente"] = p.GetEstado;                    
                else
                    dr["Cliente"] = estadoUnidad.Vendido;

                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tUnidad";

            ds.Tables[0].DefaultView.Sort = "nivel";

            return ds;
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath;
            string filename = "Unidades de " + lbProyecto.Text + ".pdf";

            //Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataSet(), false, System.Data.MissingSchemaAction.Ignore);
            CrystalReportSource.ReportDocument.SetDataSource(ds);

            CrystalReportSource.ReportDocument.SetParameterValue("fecha", String.Format("{0:MMMM yyyy}", DateTime.Today));
            CrystalReportSource.ReportDocument.SetParameterValue("fechaImpresion", String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));
            CrystalReportSource.ReportDocument.SetParameterValue("moneda", lbMoneda.Text);
            cHistorial historial = cHistorial.LoadByIdProyecto(Request["idProyecto"].ToString());
            if(historial != null)
                CrystalReportSource.ReportDocument.SetParameterValue("fechaUltimaActualizacion", String.Format("{0:dd/MM/yyyy}", historial.Fecha));
            else
                CrystalReportSource.ReportDocument.SetParameterValue("fechaUltimaActualizacion", String.Format("{0:dd/MM/yyyy}", DateTime.Now));
            CrystalReportSource.ReportDocument.SetParameterValue("obra", lbProyecto.Text);

            CrystalReportSource.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);

            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
        #endregion
    }
}