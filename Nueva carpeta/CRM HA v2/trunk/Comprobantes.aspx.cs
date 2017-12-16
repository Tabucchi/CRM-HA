using Common.Logging;
using DLL.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class Comprobantes : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarComboCliente();
                
                DateTime date = DateTime.Now;
                DateTime desde = new DateTime(date.Year, date.Month, 1);
                DateTime hasta = new DateTime(date.Month == 12 ? date.Year + 1 : date.Year, date.Month == 12 ? 1 : date.Month + 1, 1);

                txtFechaDesde.Text = String.Format("{0:dd/MM/yyyy}", desde);
                txtFechaHasta.Text = String.Format("{0:dd/MM/yyyy}", hasta);

                lvComprobanteRecibos.DataSource = cReciboCuota.GetRecibos(txtFechaDesde.Text, txtFechaHasta.Text, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, "1");
                lvComprobanteRecibos.DataBind();

                lvComprobanteNotaCredito.DataSource = cNotaCredito.GetNotasCredito(txtFechaDesde.Text, txtFechaHasta.Text, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, "1");
                lvComprobanteNotaCredito.DataBind();

                lvComprobanteNotaDebito.DataSource = cNotaDebito.GetNotasDebito(txtFechaDesde.Text, txtFechaHasta.Text, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, "1");
                lvComprobanteNotaDebito.DataBind();

                lvComprobanteCondonacion.DataSource = cCondonacion.GetCondonaciones(txtFechaDesde.Text, txtFechaHasta.Text, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, "1");
                lvComprobanteCondonacion.DataBind();

                CalcularTotales();
            }
        }
        
        #region Botones / Combo
        protected void cbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            Listar();
        }

        public void CargarComboCliente()
        {
            cbFiltroCliente.DataSource = cEmpresa.GetDataTable();
            cbFiltroCliente.DataValueField = "id";
            cbFiltroCliente.DataTextField = "nombre";
            cbFiltroCliente.DataBind();
            //Agrego item TODAS
            ListItem it = new ListItem("Todas", "0");
            cbFiltroCliente.Items.Insert(0, it);

            cbEstado.SelectedValue = "1";
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Listar();
        }
        #endregion 
        
        #region Recibos
        private void CalcularTotalesRecibos(ListView list)
        {
            try
            {
                decimal _total = 0;

                foreach (ListViewItem item in list.Items)
                {
                    Label lbTotal = item.FindControl("lbMonto") as Label;

                    _total += Convert.ToDecimal(lbTotal.Text);
                }

                if (_total != 0)
                {
                    Label lblTotalDeudaVencida = (Label)list.FindControl("lbTotal");
                    lblTotalDeudaVencida.Text = String.Format("{0:#,#0.00}", _total);
                    hfTotalR.Value = String.Format("{0:#,#0.00}", _total);
                }
                else
                {
                    hfTotalR.Value = "0";
                }
            }
            catch (Exception ex)
            {

                log4net.Config.XmlConfigurator.Configure();
                log.Error("PlanillaRecibo - " + DateTime.Now + "- " + ex.Message + " - CalcularTotales" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void lvRecibos_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            cItemCCU itemCCU = cItemCCU.Load(e.CommandArgument.ToString());
            string _idEmpresa = cCuentaCorrienteUsuario.Load(itemCCU.IdCuentaCorrienteUsuario).IdEmpresa;

            ImprimirRecibo(itemCCU, _idEmpresa);
        }

        protected void ImprimirRecibo(cItemCCU _itemCCU, string _idEmpresa)
        {
            if (_itemCCU.Credito != 0)
                CrearPdfRecibo(_itemCCU, _idEmpresa);

            if (_itemCCU.Debito != 0 && _itemCCU.TipoOperacion == (Int16)eTipoOperacion.Reserva)
                CrearPdfRecibo(_itemCCU, _idEmpresa);
        }

        protected void CrearPdfRecibo(cItemCCU _itemCCU, string _idEmpresa)
        {
            cReciboCuota recibo;
            if (string.IsNullOrEmpty(cReciboCuota.GetNroReciboByIdItemCCU(_itemCCU.Id)))
                recibo = cReciboCuota.CrearRecibo("-1", _itemCCU.Id, _itemCCU.Credito + _itemCCU.Debito);
            else
                recibo = cReciboCuota.GetReciboByNro(_itemCCU.GetRecibo);

            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Comprobantes\\Recibos\\";
            string filename = "Recibo_" + recibo.Nro + ".pdf";

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("fecha", String.Format("{0:dd/MM/yyyy}", recibo.Fecha));
            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("recibo", _itemCCU.GetRecibo);
            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("fechaImpresion", String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("cliente", cEmpresa.Load(_idEmpresa).GetNombreCompleto);

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("monto", String.Format("{0:#,#0.00}", recibo.Monto) + ".-");
            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("montoLetras", cAuxiliar.enLetras(recibo.Monto.ToString()) + ".-");

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("concepto", _itemCCU.Concepto);
            CrystalReportSourceRecibo.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);
            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
        #endregion

        #region Nota de débito
        private void CalcularTotalesNotasDebito(ListView list)
        {
            try
            {
                decimal _total = 0;

                foreach (ListViewItem item in list.Items)
                {
                    Label lbTotal = item.FindControl("lbMonto") as Label;

                    _total += Convert.ToDecimal(lbTotal.Text);
                }

                if (_total != 0)
                {
                    Label lblTotal = (Label)list.FindControl("lbTotalND");
                    lblTotal.Text = String.Format("{0:#,#0.00}", _total);
                    hfTotalND.Value = String.Format("{0:#,#0.00}", _total);
                }
                else
                {
                    hfTotalND.Value = "0";
                }
            }
            catch (Exception ex)
            {

                log4net.Config.XmlConfigurator.Configure();
                log.Error("PlanillaRecibo - " + DateTime.Now + "- " + ex.Message + " - CalcularTotalesNotasDebito" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void lvNotaDebito_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            cItemCCU itemCCU = cItemCCU.Load(e.CommandArgument.ToString());
            string _idEmpresa = cCuentaCorrienteUsuario.Load(itemCCU.IdCuentaCorrienteUsuario).IdEmpresa;

            ImprimirNotaDebito(itemCCU, _idEmpresa);
        }

        protected void CrearPdfNotaDebito(cItemCCU _itemCCU, string _idEmpresa)
        {
            cNotaDebito nd = cNotaDebito.GetNotaDebitoByNro(_itemCCU.GetNotaDebito);

            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Comprobantes\\Nota de debito\\";
            string filename = "Nota de debito " + nd.Nro + ".pdf";

            decimal _monto = nd.Monto * -1;

            CrystalReportSourceNotaDebito.ReportDocument.SetParameterValue("fecha", String.Format("{0:dd/MM/yyyy}", nd.Fecha));
            CrystalReportSourceNotaDebito.ReportDocument.SetParameterValue("recibo", _itemCCU.GetNotaDebito);
            CrystalReportSourceNotaDebito.ReportDocument.SetParameterValue("fechaImpresion", String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));
            CrystalReportSourceNotaDebito.ReportDocument.SetParameterValue("cliente", cEmpresa.Load(_idEmpresa).GetNombreCompleto);
            CrystalReportSourceNotaDebito.ReportDocument.SetParameterValue("montoletras", cAuxiliar.enLetras(_monto.ToString()) + ".-");
            CrystalReportSourceNotaDebito.ReportDocument.SetParameterValue("monto", String.Format("{0:#,#0.00}", _monto) + ".-");
            CrystalReportSourceNotaDebito.ReportDocument.SetParameterValue("concepto", _itemCCU.Concepto);

            CrystalReportSourceNotaDebito.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);
            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }

        protected void ImprimirNotaDebito(cItemCCU _itemCCU, string _idEmpresa)
        {
            CrearPdfNotaDebito(_itemCCU, _idEmpresa);
        }
        #endregion

        #region Nota de crédito
        private void CalcularTotalesNotasCredito(ListView list)
        {
            try
            {
                decimal _total = 0;

                foreach (ListViewItem item in list.Items)
                {
                    Label lbTotal = item.FindControl("lbMonto") as Label;

                    _total += Convert.ToDecimal(lbTotal.Text);
                }

                if (_total != 0)
                {
                    Label lblTotal = (Label)list.FindControl("lbTotalNC");
                    lblTotal.Text = String.Format("{0:#,#0.00}", _total);
                    hfTotalNC.Value = String.Format("{0:#,#0.00}", _total);
                }
                else
                {
                    hfTotalNC.Value = "0";
                }
            }
            catch (Exception ex)
            {

                log4net.Config.XmlConfigurator.Configure();
                log.Error("PlanillaRecibo - " + DateTime.Now + "- " + ex.Message + " - CalcularTotalesNotasDebito" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void lvNotaCredito_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            cItemCCU itemCCU = cItemCCU.Load(e.CommandArgument.ToString());
            string _idEmpresa = cCuentaCorrienteUsuario.Load(itemCCU.IdCuentaCorrienteUsuario).IdEmpresa;

            ImprimirNotaCredito(itemCCU, _idEmpresa);
        }

        protected void CrearPdfNotaCredito(cItemCCU _itemCCU, string _idEmpresa)
        {
            cNotaCredito notaCredito = cNotaCredito.GetNotaCreditoByNro(_itemCCU.GetNotaCredito);

            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Comprobantes\\Nota de credito\\";
            string filename = "Nota de credito " + notaCredito.Nro + ".pdf";

            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("fecha", String.Format("{0:dd/MM/yyyy}", notaCredito.Fecha));
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("recibo", _itemCCU.GetNotaCredito);
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("fechaImpresion", String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("cliente", cEmpresa.Load(_idEmpresa).GetNombreCompleto);
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("monto", String.Format("{0:#,#0.00}", notaCredito.Monto) + ".-");
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("montoletras", cAuxiliar.enLetras(notaCredito.Monto.ToString()) + ".-");

            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("concepto", _itemCCU.Concepto);

            CrystalReportSourceNotaCredito.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);
            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }

        protected void ImprimirNotaCredito(cItemCCU _itemCCU, string _idEmpresa)
        {
            CrearPdfNotaCredito(_itemCCU, _idEmpresa);
        }
        #endregion

        #region Condonaciones
        private void CalcularTotalesCondonaciones(ListView list)
        {
            try
            {
                decimal _total = 0;

                foreach (ListViewItem item in list.Items)
                {
                    Label lbTotal = item.FindControl("lbMonto") as Label;

                    _total += Convert.ToDecimal(lbTotal.Text);
                }

                if (_total != 0)
                {
                    Label lblTotal = (Label)list.FindControl("lbTotal");
                    lblTotal.Text = String.Format("{0:#,#0.00}", _total);
                    hfTotalC.Value = String.Format("{0:#,#0.00}", _total);
                }
                else
                {
                    hfTotalC.Value = "0";
                }
            }
            catch (Exception ex)
            {

                log4net.Config.XmlConfigurator.Configure();
                log.Error("PlanillaRecibo - " + DateTime.Now + "- " + ex.Message + " - CalcularTotalesCondonaciones" + " - " + cUsuario.Load(HttpContext.Current.User.Identity.Name).Nombre);
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void lvCondonacion_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            cItemCCU itemCCU = cItemCCU.Load(e.CommandArgument.ToString());
            string _idEmpresa = cCuentaCorrienteUsuario.Load(itemCCU.IdCuentaCorrienteUsuario).IdEmpresa;

            ImprimirCondonacion(itemCCU, _idEmpresa);
        }

        protected void ImprimirCondonacion(cItemCCU _itemCCU, string _idEmpresa)
        {
            if (_itemCCU.Credito != 0)
                CrearPdfCondonacion(_itemCCU, _idEmpresa);

            if (_itemCCU.Debito != 0 && _itemCCU.TipoOperacion == (Int16)eTipoOperacion.Reserva)
                CrearPdfCondonacion(_itemCCU, _idEmpresa);
        }

        protected void CrearPdfCondonacion(cItemCCU _itemCCU, string _idEmpresa)
        {
            cCondonacion recibo;
            if (string.IsNullOrEmpty(cCondonacion.GetNroCondonacionByIdItemCCU(_itemCCU.Id)))
                recibo = cCondonacion.CrearCondonacion("-1", _itemCCU.Id, _itemCCU.Credito + _itemCCU.Debito);
            else
                recibo = cCondonacion.GetCondonacionByNro(_itemCCU.GetRecibo);

            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Comprobantes\\Recibos\\";
            string filename = "Recibo_" + recibo.Nro + ".pdf";

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("fecha", String.Format("{0:dd/MM/yyyy}", recibo.Fecha));
            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("recibo", _itemCCU.GetRecibo);
            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("cliente", cEmpresa.Load(_idEmpresa).GetNombreCompleto);

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("monto", String.Format("{0:#,#0.00}", recibo.Monto) + ".-");
            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("montoLetras", cAuxiliar.enLetras(recibo.Monto.ToString()) + ".-");

            CrystalReportSourceRecibo.ReportDocument.SetParameterValue("concepto", _itemCCU.Concepto);
            CrystalReportSourceRecibo.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);
            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
        #endregion

        #region Auxiliares
        public void Listar()
        {
            string desde = null;
            string hasta = null;

            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
                desde = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(txtFechaDesde.Text));

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
                hasta = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(txtFechaHasta.Text));
            
            switch (cbComprobante.SelectedValue)
            {
                case "Todos":
                    VisiblePanel(true, false, false, false, false);

                    lvComprobanteRecibos.DataSource = cReciboCuota.GetRecibos(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    lvComprobanteRecibos.DataBind();

                    lvComprobanteNotaDebito.DataSource = cNotaDebito.GetNotasDebito(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    lvComprobanteNotaDebito.DataBind();

                    lvComprobanteNotaCredito.DataSource = cNotaCredito.GetNotasCredito(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    lvComprobanteNotaCredito.DataBind();

                    lvComprobanteCondonacion.DataSource = cCondonacion.GetCondonaciones(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    lvComprobanteCondonacion.DataBind();

                    CalcularTotales();
                    break;
                case "Recibo":
                    VisiblePanel(false, true, false, false, false);

                    lvRecibos.DataSource = cReciboCuota.GetRecibos(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    lvRecibos.DataBind();

                    CalcularTotalesRecibos(lvRecibos);
                    break;
                case "NotaDebito":
                    VisiblePanel(false, false, true, false, false);

                    lvNotaDebito.DataSource = cNotaDebito.GetNotasDebito(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    lvNotaDebito.DataBind();

                    CalcularTotalesNotasDebito(lvNotaDebito);
                    break;
                case "NotaCredito":
                    VisiblePanel(false, false, false, true, false);

                    lvNotaCredito.DataSource = cNotaCredito.GetNotasCredito(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    lvNotaCredito.DataBind();

                    CalcularTotalesNotasCredito(lvNotaCredito);
                    break;
                case "Condonacion":
                    VisiblePanel(false, false, false, false, true);

                    lvCondonacion.DataSource = cCondonacion.GetCondonaciones(txtFechaDesde.Text, txtFechaHasta.Text, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    lvCondonacion.DataBind();

                    CalcularTotalesCondonaciones(lvCondonacion);
                    break;
            }
        }

        public void CalcularTotales()
        {
            CalcularTotalesRecibos(lvComprobanteRecibos);
            CalcularTotalesNotasDebito(lvComprobanteNotaDebito);
            CalcularTotalesNotasCredito(lvComprobanteNotaCredito);
            CalcularTotalesCondonaciones(lvComprobanteCondonacion);

            decimal _total = Convert.ToDecimal(hfTotalR.Value) + Convert.ToDecimal(hfTotalND.Value) + Convert.ToDecimal(hfTotalNC.Value) + Convert.ToDecimal(hfTotalC.Value);
            lbTotalComprobantes.Text = String.Format("{0:#,#0.00}", _total);             
        }

        public void VisiblePanel(bool _todos, bool _recibo, bool _notaDebito, bool _notaCredito, bool _condonacion)
        {
            pnlRecibos.Visible = _recibo;
            pnlNotaDebito.Visible = _notaDebito;
            pnlNotaCredito.Visible = _notaCredito;
            pnlCondonacion.Visible = _condonacion;
            pnlComprobantes.Visible = _todos;
        }
        #endregion

        protected void btnImprimirCC_Click(object sender, EventArgs e)
        {
            string desde = null;
            string hasta = null;

            if (!string.IsNullOrEmpty(txtFechaDesde.Text))
                desde = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(txtFechaDesde.Text));

            if (!string.IsNullOrEmpty(txtFechaHasta.Text))
                hasta = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(txtFechaHasta.Text));
                        
            switch (cbComprobante.SelectedValue)
            {
                case "Todos":
                    List<cReciboCuota> _recibos = cReciboCuota.GetRecibos(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    DataTable _tablaR = cExcel.ToDataTable(_recibos);
                    List<DataTable> _listR = new List<DataTable>();
                    _listR.Add(_tablaR);

                    List<cNotaCredito> _notasC = cNotaCredito.GetNotasCredito(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    DataTable _tablaNC = cExcel.ToDataTable(_notasC);
                    List<DataTable> _listNC = new List<DataTable>();
                    _listNC.Add(_tablaNC);

                    List<cNotaDebito> _notasD = cNotaDebito.GetNotasDebito(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    DataTable _tablaND = cExcel.ToDataTable(_notasD);
                    List<DataTable> _listND = new List<DataTable>();
                    _listND.Add(_tablaND);

                    List<cCondonacion> _condonaciones = cCondonacion.GetCondonaciones(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    DataTable _tablaCondonacion = cExcel.ToDataTable(_condonaciones);
                    List<DataTable> _listC = new List<DataTable>();
                    _listC.Add(_tablaCondonacion);
                    
                    string filename = "Comprobantes - " + DateTime.Now.ToShortDateString();
                    cExcel.DataTableToExcelAllComprobantes(_listR, _listNC, _listND, _listC, filename, String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalR.Value)), String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalNC.Value)), String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalND.Value)), String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalC.Value)));
                    break;
                case "Recibo":
                    List<cReciboCuota> recibos = cReciboCuota.GetRecibos(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    DataTable tablaR = cExcel.ToDataTable(recibos);
                    List<DataTable> listR = new List<DataTable>();
                    listR.Add(tablaR);
                    string filenameR = "Recibos - " + DateTime.Now.ToShortDateString();
                    cExcel.DataTableToExcelComprobantes(listR, filenameR, String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalR.Value)));
                    break;
                case "NotaDebito":
                    List<cNotaDebito> notasD = cNotaDebito.GetNotasDebito(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    DataTable tablaND = cExcel.ToDataTable(notasD);
                    List<DataTable> listND = new List<DataTable>();
                    listND.Add(tablaND);
                    string filenameND = "Notas de debito - " + DateTime.Now.ToShortDateString();
                    cExcel.DataTableToExcelComprobantes(listND, filenameND, String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalND.Value)));
                    break;
                case "NotaCredito":
                    List<cNotaCredito> notasC = cNotaCredito.GetNotasCredito(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    DataTable tablaNC = cExcel.ToDataTable(notasC);
                    List<DataTable> listNC = new List<DataTable>();
                    listNC.Add(tablaNC);
                    string filenameNC = "Notas de creditos - " + DateTime.Now.ToShortDateString();
                    cExcel.DataTableToExcelComprobantes(listNC, filenameNC, String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalNC.Value)));                    
                    break;
                case "Condonacion":
                    List<cCondonacion> condonaciones = cCondonacion.GetCondonaciones(desde, hasta, cbFiltroCliente.SelectedValue, txtFiltroNro.Text, cbEstado.SelectedValue);
                    DataTable tablaCondonaciones = cExcel.ToDataTable(condonaciones);
                    List<DataTable> listCondonaciones = new List<DataTable>();
                    listCondonaciones.Add(tablaCondonaciones);
                    string filenameCondonaciones = "Condonaciones - " + DateTime.Now.ToShortDateString();
                    cExcel.DataTableToExcelComprobantes(listCondonaciones, filenameCondonaciones, String.Format("{0:#,#0.00}", Convert.ToDecimal(hfTotalC.Value)));                    
                    break;
            }
        }

        protected void btnTodos_Click(object sender, EventArgs e)
        {
            lvComprobanteRecibos.DataSource = cReciboCuota.GetAllRecibos();
            lvComprobanteRecibos.DataBind();

            lvComprobanteNotaCredito.DataSource = cNotaCredito.GetAllNotasCredito();
            lvComprobanteNotaCredito.DataBind();

            lvComprobanteNotaDebito.DataSource = cNotaDebito.GetAllNotasDebito();
            lvComprobanteNotaDebito.DataBind();

            lvComprobanteCondonacion.DataSource = cCondonacion.GetAllCondonaciones();
            lvComprobanteCondonacion.DataBind();

            CalcularTotales();
        }
    }
}