using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using log4net;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace crm
{
    public partial class DetallePedido : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int page;

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = "";

            if (!IsPostBack)
            {
                #region Combos
                ((MasterPage)this.Master).Check_LoggedUser();
                cbResponsable.DataSource = cUsuario.GetDataTable();
                cbResponsable.DataValueField = "id";
                cbResponsable.DataTextField = "nombre";
                cbResponsable.DataBind();
                cbResponsable.Items[0].Selected = true;

                //Carga de dropdownlist de Usuario del ModalPopup Finalizacion del ticket
                ddlUsuario.DataSource = cUsuario.GetUsuarios();
                ddlUsuario.DataValueField = "id";
                ddlUsuario.DataTextField = "nombre";
                ddlUsuario.DataBind();
                ddlUsuario.SelectedValue = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id;

                //Carga de dropdownlist de Modo Resolucion del ModalPopup Finalizacion del ticket
                ddlModoResolucion.DataSource = cCampoGenerico.GetDataTable(Tablas.tModoResolucion);
                ddlModoResolucion.DataValueField = "id";
                ddlModoResolucion.DataTextField = "descripcion";
                ddlModoResolucion.DataBind();
                ddlModoResolucion.Items[0].Selected = true;

                //Carga de dropdownlist de Cambio de Estado
                ddlEstados.DataSource = cCampoGenerico.GetDataTable(Tablas.tEstado);
                ddlEstados.DataValueField = "id";
                ddlEstados.DataTextField = "descripcion";
                ddlEstados.DataBind();
                ListItem it = new ListItem("Seleccione un estado...", "0");
                ddlEstados.Items.Insert(0, it);

                rptComentarios.DataSource = cComentario.GetComentarios(Request["id"].ToString());
                rptComentarios.DataBind();
                #endregion

                #region Compras
                //Carga de listview de items
                //lvItems.DataSource = cItem.GetItemsPedido(Request["id"]);
                //lvItems.DataBind();

                //Muestra mensaje de cantidad de items en una compra
                //List<cItem> items = cItem.GetItemsPedido(Request["id"]);
                //List<cCompra> compras = cCompra.GetCompraNro(Request["id"].ToString());
                ////if (items != null)
                //if (compras != null)
                //{
                //    if (compras.Count == 0)
                //    {
                //        lbMensaje1.Visible = true;
                //        btnAgregar.Visible = true;
                //    }
                //    else
                //    {
                //        lbMensaje2.Text = "Hay " + compras.Count + " items registrados";
                //        lbMensaje2.Visible = true;
                //        btnVer.Visible = true;
                //    }
                //}
                //else
                //{
                //    lbMensaje1.Visible = true;
                //    btnAgregar.Visible = true;
                //}

                //Completa el txtEditDescripcion del ModalEditDescripcion
                //cPedido pedido = cPedido.Load(Request["id"]);
                //txtEditDescripcion.Text = pedido.Descripcion;
                #endregion

                #region Navegación
                Int16 lastId = cPedido.GetFirstTicket();
                if (Convert.ToInt16(Request["id"].ToString()) == lastId)
                    btnPrevious.Enabled = false;

                Int16 nextId = cPedido.GetLastTicket();
                if (Convert.ToInt16(Request["id"].ToString()) == nextId)
                    btnNext.Enabled = false;
                #endregion
            }

            try
            {
                if (Request["id"] != null)
                {
                    id = Request["id"].ToString();
                }
                else
                {
                    List<cPedido> list = cPedido.Search(Convert.ToString(Request["idEmpresa"]),
                                                       Convert.ToString(Request["fechaDesde"]),
                                                       Convert.ToString(Request["fechaHasta"]),
                                                       Convert.ToString(Request["idEstado"]),
                                                       0, 0, 0, 0);

                    page = Convert.ToInt32(Request["page"]);

                    //Ajusto el numero de Pagina.
                    if (page >= list.Count) page = 0;
                    if (page < 0) page = list.Count - 1;

                    id = list[page].Id;
                }

                /* Validar si el id es valido y mostrar un mensaje si no existe */
                cPedido pedido = cPedido.Load(id);
                lblID.Text = "ID " + pedido.Id;

                #region Compras
                List<cCompra> compras = cCompra.GetCompraByPedido(pedido.Id);
                //if (items != null)
                if (compras != null)
                {
                    if (compras.Count == 0)
                    {
                       // lbMensaje1.Visible = true;
                      //  btnAgregar.Visible = true;
                    }
                    else
                    {
                      //  lbMensaje1.Visible = false;
                       // btnAgregar.Visible = false;
                      //  lbMensaje2.Text = "Hay una compra asociada";
                      // lbMensaje2.Visible = true;
                       // btnVer.Visible = true;
                    }
                }
                else
                {
                  //  lbMensaje1.Visible = true;
                  //  btnAgregar.Visible = true;
                }
                #endregion

                #region Pedido
                //Lo guardo en el campo Hidden
                txtId.Value = pedido.Id;

                lblEstado.Text = pedido.GetEstado;
                lblCargadoPor.Text = pedido.GetUsuario;

                lblFechaCarga.Text = pedido.Fecha.ToLongDateString() + " a las " + pedido.Fecha.ToShortTimeString() + "·";

                lblCliente.Text = pedido.GetCliente().Nombre + " de " + pedido.GetCliente().GetEmpresa().ToUpper();
                lblTitulo.Text = pedido.Titulo;
                lblDescripcion.Text = pedido.Descripcion;
                lblCategoria.Text = pedido.GetCategoria;
                lblPrioridad.Text = pedido.GetPrioridad;
                lblModoResolucion.Text = pedido.GetModoResolucion;

                lblFechaRealizacion.Text = pedido.GetFechaLimite;
                lblResponsable.Text = pedido.GetResponsable() == null ? "-" : cUsuario.Load(pedido.GetResponsable().IdResponsable).Nombre;

                // Estados
                switch (pedido.GetEstado)
                {
                    case "Nuevo":
                        //imgEstado.Src = "Imagenes/circle_red.png";
                        break;
                    case "Pendiente":
                        //imgEstado.Src = "Imagenes/circle_blue.png";
                        break;
                    case "Finalizado":
                        //imgEstado.Src = "Imagenes/circle_green.png";
                        lblFechaFinalizacion.Text = pedido.GetComentarioLast() == null ? "-" : pedido.GetLastComentarioInfo;
                        break;
                    default:
                        lblFechaFinalizacion.Text = "-";
                        break;
                }
                #endregion

                #region Comentarios
                // Comentarios
                List<cComentario> comentarios = cComentario.GetList(id, false);
                if (comentarios.Count > 0)
                {
                    rptComentarios.DataSource = comentarios;
                    rptComentarios.DataBind();
                }
                #endregion

                #region Agenda
                //Agenda
                txtDireccionCliente.Text = cEmpresa.Load(pedido.GetCliente().IdEmpresa).Direccion;
                txtTelefonoCliente.Text = cEmpresa.Load(pedido.GetCliente().IdEmpresa).Telefono + " (int " + pedido.GetCliente().Interno + ")";
                txtMailCliente.Text = pedido.GetCliente().Mail;
                #endregion

                //Completa el txtEditDescripcion del ModalEditDescripcion la primera vez que entra.
                if (!IsPostBack)
                    txtEditDescripcion.Text = pedido.Descripcion;
            }
            catch (Exception ex)
            {
                lblID.Text = "ID ERRONEO";
                lblEstado.Text = "-";
                lblFechaCarga.Text = "-";

                lblCliente.Text = "-";
                lblTitulo.Text = "-";
                lblDescripcion.Text = "-";
                lblCategoria.Text = "-";
                lblPrioridad.Text = "-";

                lblFechaRealizacion.Text = "-";
                lblResponsable.Text = "-";
                lblFechaFinalizacion.Text = "-";

                txtDireccionCliente.Text = "-";
                txtTelefonoCliente.Text = "-";
                txtMailCliente.Text = "-";
            }

            #region Ticket Finalizado
            // Deshabilitación de funciones si el estado es FINALIZADO.
            if (lblEstado.Text == Estado.Finalizado.ToString())
            {
                btnReasignar.Enabled = false;
                btnFinalizar.Enabled = false;
                btnEliminar.Enabled = false;
                btnAgregarComentario.Enabled = false;
                cbVisibilidad.Enabled = false;
            }
            else
            {
                btnReasignar.Enabled = true;
                btnFinalizar.Enabled = true;
                btnAgregarComentario.Enabled = true;
                cbVisibilidad.Enabled = true;
            }
            #endregion

            if (lblEstado.Text == Estado.A_cobrar.ToString().Replace("_", " "))
            {
                btnAcobrar.Enabled = false;
            }

            SetPermisosUsuario();
        }
        
        #region Setear Permisos
        public void SetPermisosUsuario()
        {
            switch (cUsuario.Load(Convert.ToString(Session["IdUsuario"])).IdCategoria)
            {
                case (Int16)Categoria.Administración:
                    filaQuienCerroTicket.Style.Add("Display", "");
                    btnReasignar.Text = "Reasignar";
                    cbVisibilidad.Visible = true;
                    btnAcobrar.Visible = true;
                    btnEliminar.Visible = true;
                    break;
                case (Int16)Categoria.Telecomunicaciones:
                    filaQuienCerroTicket.Style.Add("Display", "none");
                    btnReasignar.Text = "";
                    cbVisibilidad.Visible = false;
                    btnAcobrar.Visible = true;
                    btnEliminar.Visible = false;
                    break;
                default:
                    filaQuienCerroTicket.Style.Add("Display", "none");
                    btnReasignar.Text = "";
                    cbVisibilidad.Visible = false;
                    break;
            }
        }
        #endregion

        #region Botones
        // Boton Atrás
        protected void btnLast_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt16(Request["id"].ToString()) - 1;
            bool flag = false;

            while (flag != true)
            {
                try
                {
                    cPedido pedido = cPedido.Load(id.ToString());
                    flag = true;
                }
                catch
                {
                    id--;
                    flag = false;
                }
            }
            
            string url = "DetallePedido.aspx?id=" + id;
            Response.Redirect(url, false);
        }

        // Boton Siguiente
        protected void btnNext_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt16(Request["id"].ToString()) + 1;
            bool flag = false;

            while (flag != true)
            {
                try
                {
                    cPedido pedido = cPedido.Load(id.ToString());
                    flag = true;
                }
                catch
                {
                    id++;
                    flag = false;
                }
            }

            string url = "DetallePedido.aspx?id=" + id;
            Response.Redirect(url, false);
        }

        // Finalizar Ticket
        protected void btnFinalizarTicket_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtComentarioFin.Text))
                return;

            try
            {
                cPedido pedido = cPedido.Load(txtId.Value);

                if (ddlUsuario.SelectedValue != cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id)
                    pedido.Comentar(Session["IdUsuario"].ToString(), "cUsuario", txtComentario.Text, false, false);

                pedido.Finalizar(ddlUsuario.SelectedValue, "cUsuario", txtComentarioFin.Text, Convert.ToInt16(ddlModoResolucion.SelectedValue));
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("DetallePedido - " + DateTime.Now + "- " + ex.Message + " - btnFinalizarTicket_Click");
                Response.Redirect("MensajeError.aspx");
            }

            try
            {
                if (Request["id"] != null)
                    Response.Redirect("DetallePedido.aspx?id=" + Request["id"].ToString(), false);
                else
                {
                    string url = "DetallePedido.aspx?idEmpresa=" + Convert.ToString(Request["idEmpresa"]);
                    url += "&fechaDesde=" + Convert.ToString(Request["fechaDesde"]);
                    url += "&fechaHasta=" + Convert.ToString(Request["fechaHasta"]);
                    url += "&idEstado=" + Convert.ToString(Request["idEstado"]);

                    url += "&page=" + Convert.ToString(page);

                    Response.Redirect(url, false);
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("DetallePedido - " + DateTime.Now + "- " + ex.Message + " - btnFinalizarTicket_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }

        // Reasignar Responsable
        protected void btnAsignar_Click(object sender, EventArgs e)
        {
            if (cbResponsable.SelectedIndex < 0)
                return;

            try
            {
                cPedido pedido = cPedido.Load(txtId.Value);
                cAsignacionResponsable responsable = cAsignacionResponsable.GetResponsablePorPedido(pedido.Id);
                pedido.Reasignar(responsable, cbResponsable.SelectedValue, Session["IdUsuario"].ToString(), txtMensajeResponsable.Text, pedido.Id);

            }
            catch { }

            if (Request["id"] != null)
                Response.Redirect("DetallePedido.aspx?id=" + Request["id"].ToString(), false);
            else
            {
                string url = "DetallePedido.aspx?idEmpresa=" + Convert.ToString(Request["idEmpresa"]);
                url += "&fechaDesde=" + Convert.ToString(Request["fechaDesde"]);
                url += "&fechaHasta=" + Convert.ToString(Request["fechaHasta"]);
                url += "&idEstado=" + Convert.ToString(Request["idEstado"]);

                url += "&page=" + Convert.ToString(page);

                Response.Redirect(url, false);
            }
        }

        // Agregar comentarios
        protected void btnAgregarComentario_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtComentario.Text))
                return;

            try
            {
                cPedido pedido = cPedido.Load(txtId.Value);
                pedido.Comentar(Session["IdUsuario"].ToString(), "cUsuario", txtComentario.Text, true, cbVisibilidad.Checked);
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("DetallePedido - " + DateTime.Now + "- " + ex.Message + " - btnAgregarComentario_Click");
                Response.Redirect("MensajeError.aspx");
            }
            try
            {
                if (Request["id"] != null)
                    Response.Redirect("DetallePedido.aspx?id=" + Request["id"].ToString(), false);
                else
                {
                    string url = "DetallePedido.aspx?idEmpresa=" + Convert.ToString(Request["idEmpresa"]);
                    url += "&fechaDesde=" + Convert.ToString(Request["fechaDesde"]);
                    url += "&fechaHasta=" + Convert.ToString(Request["fechaHasta"]);
                    url += "&idEstado=" + Convert.ToString(Request["idEstado"]);
                    url += "&page=" + Convert.ToString(page);

                    Response.Redirect(url, false);
                }
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("DetallePedido - " + DateTime.Now + "- " + ex.Message + " - btnAgregarComentario_Click");
                Response.Redirect("MensajeError.aspx");
            }

        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                cPedido pedido = cPedido.Load(Request["id"]);

                //CrystalReportSource1.ReportDocument.SetParameterValue("fecha", Convert.ToDateTime(DateTime.Today).ToShortDateString());
                //CrystalReportSource1.ReportDocument.SetParameterValue("id", pedido.Id);
                //CrystalReportSource1.ReportDocument.SetParameterValue("empresa", pedido.GetCliente().GetEmpresa());
                //CrystalReportSource1.ReportDocument.SetParameterValue("direccion", pedido.GetDireccion());
                //CrystalReportSource1.ReportDocument.SetParameterValue("cliente", pedido.GetCliente().Nombre);
                //CrystalReportSource1.ReportDocument.SetParameterValue("categoria", pedido.GetCategoria);
                //CrystalReportSource1.ReportDocument.SetParameterValue("responsable", lblResponsable.Text);
                //CrystalReportSource1.ReportDocument.SetParameterValue("titulo", pedido.Titulo);
                //CrystalReportSource1.ReportDocument.SetParameterValue("descripcion", pedido.Descripcion);

                //CrystalReportSource1.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "C:\\CRM INFO\\Ordenes de trabajo\\" + pedido.Id + ".pdf");

                string filename = pedido.Id + ".pdf";
                Response.ContentType = "APPLICATION/OCTET-STREAM";
                Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);

                FileInfo fileToDownload = new System.IO.FileInfo("C:\\CRM INFO\\Ordenes de trabajo\\" + pedido.Id + ".pdf");
                Response.Flush();
                Response.WriteFile(fileToDownload.FullName);
                Response.End();
            }
            catch
            {
            }
        }

        protected void tempo_Click(object sender, EventArgs e)
        {
            return;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Compra.aspx?id=" + lblID.Text.Replace("ID ", ""));
        }

        protected void btnVer_Click(object sender, EventArgs e)
        {
            string id = Request["id"].ToString();
            Response.Redirect("Compra.aspx?id=" + Request["id"].ToString());
        }

        protected void btnAcobrar_Click(object sender, EventArgs e)
        {
            cPedido pedido = cPedido.Load(txtId.Value);
            pedido.IdEstado = (Int16)Estado.A_cobrar;
            pedido.Save();
            btnAcobrar.Enabled = false;
        }

        protected void btnAceptarEstado_Click(object sender, EventArgs e)
        {
            cPedido pedido = cPedido.Load(txtId.Value);
            pedido.IdEstado = Convert.ToInt16(ddlEstados.SelectedValue);
            pedido.Save();
            Response.Redirect("DetallePedido.aspx?id=" + txtId.Value);
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                cPedido pedido = cPedido.Load(txtId.Value);
                pedido.IdEstado = (Int16)Estado.Finalizado;
                pedido.Save();
                Response.Redirect("DetallePedido.aspx?id=" + Request["id"].ToString());
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("DetallePedido - " + DateTime.Now + "- " + ex.Message + " - btnEliminar_Click");
                return;
            }
        }
        #endregion

        #region Editar Descripción Pedido
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEditDescripcion.Text))
                return;

            try
            {
                cPedido pedido = cPedido.Load(txtId.Value);
                pedido.Descripcion = txtEditDescripcion.Text;
                pedido.Save();
            }
            catch { }

            if (Request["id"] != null)
                Response.Redirect("DetallePedido.aspx?id=" + Request["id"].ToString(), false);
            else
            {
                string url = "DetallePedido.aspx?idEmpresa=" + Convert.ToString(Request["idEmpresa"]);
                url += "&fechaDesde=" + Convert.ToString(Request["fechaDesde"]);
                url += "&fechaHasta=" + Convert.ToString(Request["fechaHasta"]);
                url += "&idEstado=" + Convert.ToString(Request["idEstado"]);
                url += "&page=" + Convert.ToString(page);

                Response.Redirect(url, false);
            }
        }

        protected void bntCancelar_Click(object sender, EventArgs e)
        {
            return;
        }
        #endregion

        //#region Insertar
        //protected void lvItems_ItemInserting(object sender, ListViewInsertEventArgs e)
        //{
        //    cItem item= new cItem();

        //    TextBox txt = (e.Item.FindControl("txtNombre")) as TextBox;
        //    if (txt != null)
        //        item.Nombre = txt.Text;

        //    txt = (e.Item.FindControl("txtDescripcion")) as TextBox;
        //    if (txt != null)
        //        item.Descripcion = txt.Text;

        //    txt = (e.Item.FindControl("txtCantidad")) as TextBox;
        //    if (txt != null)
        //        item.Cantidad = Convert.ToInt16(txt.Text);

        //    txt = (e.Item.FindControl("txtCosto")) as TextBox;
        //    if(txt!=null)
        //        item.Costo = txt.Text;

        //    txt = (e.Item.FindControl("txtPrecioCliente")) as TextBox;
        //    if (txt != null)
        //        item.PrecioCliente = txt.Text;

        //    item.IdPedido = Convert.ToString(Request["id"]);
        //    item.Fecha=DateTime.Today;
        //    item.IdUsuario = cUsuario.Load(Convert.ToString(Session["IdUsuario"])).Id;
        //    item.IdEstado = "0";
        //    item.IdAprobo = "0";

        //    item.Save();

        //    lvItems.EditIndex = -1;
        //    lvItems.DataSource = cItem.GetItemsPedido(Request["id"]);
        //    lvItems.DataBind();
        //}
        //#endregion

        public string GetTipoRespuesta(bool visibilidad, string tipo)
        {
            return visibilidad ? (tipo == "cUsuario" ? "(Respuesta al Cliente)" : "") : "";
        }
    }
}