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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class DetalleCC1 : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (cUsuario.Load(HttpContext.Current.User.Identity.Name).IdCategoria == (Int16)eCategoria.Administración)
                    pnlBotones.Visible = true;

                cCuentaCorrienteUsuario ccu = cCuentaCorrienteUsuario.Load(Request["idCC"].ToString());
                hfIdEmpresa.Value = ccu.IdEmpresa;

                lbNroCC.Text = ccu.IdEmpresa;
                lblCliente.Text = cEmpresa.Load(hfIdEmpresa.Value).GetNombreCompleto;

                lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
                lvCC.DataBind();


                TextBoxFecha.Text = DateTime.Now.ToString();
            }
        }

        public void CargarCuotas(DateTime _fecha)
        {
            string _idCC = null;
            string _idFormaPagoOv = null;

            List<cCuota> cuotas2 = cCuota.GetCuotasPendientes(hfIdEmpresa.Value, _fecha);
            decimal _saldo = 0;
            decimal valorCuotaVencimiento1 = 0;
            decimal valorCuotaVencimiento2 = 0;

            #region Carga de cuotas del mes correspondiente
            if (cuotas2 != null && cuotas2.Count != 0)
            {
                foreach (cCuota cuota in cuotas2)
                {
                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos) && cuota.VariacionCAC != 0)
                    {
                        if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        {
                            if (cItemCCU.GetCantCuotasById(cuota.Id) == 0)
                            {
                                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));
                                cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                                cFormaPagoOV fpov = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                _idCC = cc.Id;
                                _idFormaPagoOv = fpov.Id;

                                if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                {
                                    valorCuotaVencimiento1 = cuota.Vencimiento1 * cValorDolar.LoadActualValue();
                                    valorCuotaVencimiento2 = cuota.Vencimiento2 * cValorDolar.LoadActualValue();
                                }
                                else
                                {
                                    valorCuotaVencimiento1 = cuota.Vencimiento1;
                                    valorCuotaVencimiento2 = cuota.Vencimiento2;
                                }

                                cItemCCU ccuNew = new cItemCCU();
                                ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                ccuNew.Concepto = "Cuota nro " + cuota.Nro + " de la obra " + cc.GetProyecto + " - Cod. U.F.: " + cUnidad.LoadByIdEmpresaUnidad(cc.IdEmpresaUnidad).CodigoUF;
                                ccuNew.Debito = valorCuotaVencimiento1 * -1;

                                _saldo += lastSaldo;
                                ccuNew.Saldo = _saldo + (valorCuotaVencimiento1 * -1);

                                ccuNew.Credito = 0;
                                ccuNew.Fecha = DateTime.Now;
                                ccuNew.IdCuota = cuota.Id;
                                ccuNew.IdEstado = (Int16)eEstadoItem.Cuota;
                                ccuNew.Save();

                                #region En caso que haya pagos a cuenta
                                if (lastSaldo != 0)
                                {
                                    cItemCCU item = cItemCCU.GetLastItemById(Request["idCC"].ToString());
                                    if (item != null && item.IdEstado == (Int16)eEstadoItem.Pagado)
                                    {
                                        if (lastSaldo > valorCuotaVencimiento1)
                                        {
                                            decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                            cItemCCU ccuPago = new cItemCCU();
                                            ccuPago.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                            ccuPago.Fecha = DateTime.Now;
                                            ccuPago.Concepto = "Pago cuota " + cuota.Nro;
                                            ccuPago.Debito = 0;
                                            ccuPago.Credito = 0;
                                            ccuPago.Saldo = diferencia;
                                            ccuPago.IdCuota = cuota.Id;
                                            ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                            ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                            int _idItemCCU = ccuPago.Save();

                                            hfIdItemCC.Value = _idItemCCU.ToString();

                                            //Genera el recibo del pago
                                            cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(Request["idCC"].ToString());
                                            cReciboCuota recibo = new cReciboCuota(cuota.Id, _idItemCCU.ToString(), lastNroRecibo.Nro, DateTime.Now);
                                            recibo.Monto = lastNroRecibo.Monto;

                                            recibo._Papelera = 1;
                                            recibo.Save();

                                            cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            cuota.Save();

                                            item.IdEstado = (Int16)eEstadoItem.Pagado;
                                            item.Save();
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    else
                    {
                        if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        {
                            decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));
                            cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                            cFormaPagoOV fpov = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                            _idCC = cc.Id;
                            _idFormaPagoOv = fpov.Id;

                            if (string.IsNullOrEmpty(cItemCCU.GetCCByIdCuota(cuota.Id)))
                            {
                                if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                {
                                    valorCuotaVencimiento1 = cuota.Vencimiento1 * cValorDolar.LoadActualValue();
                                    valorCuotaVencimiento2 = cuota.Vencimiento2 * cValorDolar.LoadActualValue();
                                }
                                else
                                {
                                    valorCuotaVencimiento1 = cuota.Vencimiento1;
                                    valorCuotaVencimiento2 = cuota.Vencimiento2;
                                }

                                cItemCCU ccuNew = new cItemCCU();
                                ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                ccuNew.Concepto = "Cuota nro " + cuota.Nro;
                                ccuNew.Debito = valorCuotaVencimiento1 * -1;

                                _saldo += lastSaldo;
                                if (_saldo < 0)
                                    ccuNew.Saldo = (valorCuotaVencimiento1 * -1) + (_saldo);
                                else
                                    ccuNew.Saldo = (_saldo - valorCuotaVencimiento1);

                                if (_saldo == 0)
                                    ccuNew.Saldo = _saldo - valorCuotaVencimiento1;

                                ccuNew.Credito = 0;
                                ccuNew.Fecha = DateTime.Now;
                                ccuNew.IdCuota = cuota.Id;
                                ccuNew.IdEstado = (Int16)eEstadoItem.Cuota;
                                ccuNew.Save();

                                #region En caso que haya pagos a cuenta
                                if (lastSaldo != 0)
                                {
                                    cItemCCU item = cItemCCU.GetLastItemById(Request["idCC"].ToString());
                                    if (item != null)
                                    {
                                        if (lastSaldo > valorCuotaVencimiento1)
                                        {
                                            decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                            cItemCCU ccuPago = new cItemCCU();
                                            ccuPago.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                            ccuPago.Fecha = DateTime.Now;
                                            ccuPago.Concepto = "Pago cuota " + cuota.Nro + " por saldo a favor";
                                            ccuPago.Debito = 0;
                                            ccuPago.Credito = 0;
                                            ccuPago.Saldo = diferencia;
                                            ccuPago.IdCuota = cuota.Id;
                                            ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                            ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                            int _idItemCCU = ccuPago.Save();

                                            hfIdItemCC.Value = _idItemCCU.ToString();

                                            //Genera el recibo del pago
                                            cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(Request["idCC"].ToString());
                                            cReciboCuota recibo = new cReciboCuota(cuota.Id, _idItemCCU.ToString(), lastNroRecibo.Nro, DateTime.Now);
                                            recibo.Monto = lastNroRecibo.Monto;
                                            recibo._Papelera = 1;
                                            recibo.Save();

                                            cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            cuota.Save();

                                            item.IdEstado = (Int16)eEstadoItem.Pagado;
                                            item.Save();
                                        }

                                        if (lastSaldo < valorCuotaVencimiento1)
                                        {
                                            decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                            cItemCCU ccuPago = new cItemCCU();
                                            ccuPago.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                            ccuPago.Fecha = DateTime.Now;
                                            ccuPago.Concepto = "Pago parcial cuota " + cuota.Nro + " por saldo a favor";
                                            ccuPago.Debito = 0;
                                            ccuPago.Credito = 0;
                                            ccuPago.Saldo = diferencia;
                                            ccuPago.IdCuota = cuota.Id;
                                            ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                            ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                            int _idItemCCU = ccuPago.Save();

                                            hfIdItemCC.Value = _idItemCCU.ToString();

                                            //Genera el recibo del pago
                                            cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(Request["idCC"].ToString());
                                            cReciboCuota recibo = new cReciboCuota(cuota.Id, _idItemCCU.ToString(), lastNroRecibo.Nro, DateTime.Now);
                                            recibo.Monto = lastNroRecibo.Monto;
                                            recibo._Papelera = 1;
                                            recibo.Save();
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }

                if (_idCC != null)
                {
                    usrCtrl.IdCC = _idCC;
                    usrCtrl.IdFormaPagoOV = _idFormaPagoOv;
                    usrCtrl.Attributes.Add(usrCtrl.IdCC, _idCC);
                    usrCtrl.Attributes.Add(usrCtrl.IdFormaPagoOV, _idFormaPagoOv);

                    hfIdCC.Value = _idCC;
                    hfIdFormaPagoOV.Value = _idFormaPagoOv;
                }
            }
            #endregion
        }

        #region Botones de pago
        protected void lkbRecibo_Click(object sender, EventArgs e)
        {
            cItemCCU itemCCU = cItemCCU.Load(hfIdItemCC.Value.ToString());
            ImprimirRecibo(itemCCU, hfIdEmpresa.Value);
        }

        #region Pagos
        protected void rblPago_TextChanged(object sender, EventArgs e)
        {
            if (rblPago.SelectedValue == "PagoCuota")
            {
                #region Limpiar
                CloseCondonacion();
                CloseOtrosPagos();
                CloseAdelantoCuotas();
                CloseCancelarSaldo();
                CloseAnular();
                CloseAnularReserva();
                CloseMessage();
                #endregion

                List<cCuota> _cuotas = cCuota.GetItemByCuotasPendientes(Request["idCC"].ToString());

                if (_cuotas.Count != 0)
                {
                    lvCuotas.DataSource = _cuotas;
                    lvCuotas.DataBind();
                    pnlPagoCuota.Visible = true;
                }
                else
                {
                    pnlMensajePago.Visible = true;
                    lbMensajePago.Text = "No se encontraron pagos.";
                }
            }

            if (rblPago.SelectedValue == "Condonacion")
            {
                #region Limpiar
                CloseOtrosPagos();
                CloseAdelantoCuotas();
                CloseCancelarSaldo();
                CloseAnular();
                CloseAnularReserva();
                CloseMessage();
                #endregion

                List<cCuota> _cuotas = cCuota.GetItemByCuotasPendientes(Request["idCC"].ToString());

                if (_cuotas.Count != 0)
                {
                    lvCondonacion.DataSource = _cuotas;
                    lvCondonacion.DataBind();
                    pnlCondonacion.Visible = true;
                }
                else
                {
                    pnlMensajePago.Visible = true;
                    lbMensajePago.Text = "No se encontraron pagos.";
                }
            }

            if (rblPago.SelectedValue == "OtrosPago")
            {
                #region Limpiar
                ClosePagoCuota();
                CloseCondonacion();
                CloseAdelantoCuotas();
                CloseCancelarSaldo();
                CloseAnular();
                CloseAnularReserva();
                CloseMessage();
                #endregion

                pnlOtrosPago.Visible = true;
            }

            if (rblPago.SelectedValue == "AdelantoCuota")
            {
                #region Limpiar
                ClosePagoCuota();
                CloseCondonacion();
                CloseOtrosPagos();
                CloseCancelarSaldo();
                CloseAnular();
                CloseAnularReserva();
                CloseMessage();
                #endregion

                List<cItemCCU> pendientes = cItemCCU.GetItemByCuotasPendientes(Request["idCC"].ToString());

                if (pendientes.Count == 0)
                {
                    pnlAdelantoCuota.Visible = true;
                    cCuentaCorrienteUsuario ccu = cCuentaCorrienteUsuario.Load(Request["idCC"].ToString());

                    cbProyectos.DataSource = cUnidad.GetDataTableProyectoByIdEmpresa(ccu.IdEmpresa);
                    cbProyectos.DataValueField = "id";
                    cbProyectos.DataTextField = "descripcion";
                    cbProyectos.DataBind();
                    ListItem io = new ListItem("Seleccione una unidad...", "0");
                    cbProyectos.Items.Insert(0, io);
                    cbProyectos.SelectedIndex = 0;

                    ListItem fp = new ListItem("Seleccione una forma de pago...", "0");
                    cbFormaPago.Items.Insert(0, fp);
                    cbFormaPago.SelectedIndex = 0;
                }
                else
                {
                    pnlAdelanto.Visible = true;
                }
            }

            if (rblPago.SelectedValue == "CancelarSaldo")
            {
                #region Limpiar
                ClosePagoCuota();
                CloseCondonacion();
                CloseOtrosPagos();
                CloseAdelantoCuotas();
                CloseAnular();
                CloseAnularReserva();
                CloseMessage();
                #endregion

                pnlCancelarSaldo.Visible = true;
                cCuentaCorrienteUsuario ccu = cCuentaCorrienteUsuario.Load(Request["idCC"].ToString());

                cbProyectosCancelarSaldo.DataSource = cUnidad.GetDataTableProyectoByIdEmpresa(ccu.IdEmpresa);
                cbProyectosCancelarSaldo.DataValueField = "id";
                cbProyectosCancelarSaldo.DataTextField = "descripcion";
                cbProyectosCancelarSaldo.DataBind();
                ListItem io = new ListItem("Seleccione una unidad...", "0");
                cbProyectosCancelarSaldo.Items.Insert(0, io);
                cbProyectosCancelarSaldo.SelectedIndex = 0;

                ListItem fp = new ListItem("Seleccione una forma de pago...", "0");
            }

            if (rblPago.SelectedValue == "Anular")
            {
                #region Limpiar
                ClosePagoCuota();
                CloseCondonacion();
                CloseOtrosPagos();
                CloseAdelantoCuotas();
                CloseCancelarSaldo();
                CloseAnularReserva();
                CloseMessage();
                #endregion

                List<cComprobante> comprobantes = new List<cComprobante>();

                List<cReciboCuota> recibos = cReciboCuota.GetRecibosToday(Request["idCC"].ToString());
                string auxNro = null;
                foreach (cReciboCuota r in recibos)
                {
                    if (auxNro != r.Nro.ToString())
                    {
                        cComprobante c = new cComprobante();
                        c.id = r.Id;
                        c.tipo = (Int16)eComprobante.Recibo;
                        c.nro = r.Nro.ToString();
                        comprobantes.Add(c);
                        auxNro = r.Nro.ToString();
                    }
                    else
                    {
                        auxNro = r.Nro.ToString();
                    }
                }
                
                List<cCondonacion> condonaciones = cCondonacion.GetCondonacionToday(Request["idCC"].ToString());
                foreach (cCondonacion co in condonaciones)
                {
                    cComprobante c = new cComprobante();
                    c.id = co.Id;
                    c.tipo = (Int16)eComprobante.Condonacion;
                    c.nro = co.Nro.ToString();
                    comprobantes.Add(c);
                }

                if (comprobantes.Count != 0)
                {
                    pnlAnular.Visible = true;
                    pnlMensajeAnular.Visible = false;

                    lvAnular.DataSource = comprobantes;
                    lvAnular.DataBind();
                }
                else
                {
                    pnlMensajeAnular.Visible = true;
                }
            }

            if (rblPago.SelectedValue == "AnularReserva")
            {
                #region Limpiar
                ClosePagoCuota();
                CloseCondonacion();
                CloseOtrosPagos();
                CloseAdelantoCuotas();
                CloseCancelarSaldo();
                CloseAnular();
                CloseMessage();
                #endregion

                pnlAnularReserva.Visible = true;
                List<cReserva> reservas = cReserva.GetReservasByIdCCU(Request["idCC"].ToString());
                if (reservas.Count != 0)
                {
                    if (reservas.Count < 2)
                    {
                        pnlOneReserva.Visible = true;
                        hfIdUnidad.Value = reservas[0].Id;
                        lbCodUF.Text = reservas[0].GetCodigoUF;
                        lbNivelReserva.Text = reservas[0].GetNivel;
                        lbUnidadReserva.Text = reservas[0].GetNroUnidad;
                        hfIdItemCCU.Value = reservas[0].IdItemCCU;
                        pnlConceptoAnularReserva.Visible = true;
                    }
                    else
                    {
                        pnlReservas.Visible = true;
                        lvReservas.DataSource = reservas;
                        lvReservas.DataBind();
                    }
                }
                else
                {
                    pnlAnularReserva.Visible = false;
                    pnlMensajeReserva.Visible = true;
                    lbMensajeReserva.Text = "No se encontraron reservas.";
                }
            }
        }

        #region Pago Cuota
        protected void rblMonedaPago_TextChanged(object sender, EventArgs e)
        {
            if (rblMonedaPago.SelectedValue == tipoMoneda.Pesos.ToString() || rblMonedaPago.SelectedValue == tipoMoneda.Dolar.ToString())
                txtImportePago.Enabled = true;
        }

        protected void btnPago_Click(object sender, EventArgs e)
        {
            #region Variables
            decimal _nuevoSaldo = 0;
            bool flag = false;
            decimal _signoSaldo = 0;
            int index = 0;
            string _idCuota = null;
            #endregion

            #region Convertir a Dolar
            Decimal _txtMonto = Convert.ToDecimal(txtImportePago.Text);
            if (rblMonedaPago.SelectedValue == tipoMoneda.Dolar.ToString())
                _txtMonto = Convert.ToDecimal(txtImportePago.Text) * cValorDolar.LoadActualValue();
            #endregion

            #region Se busca la cuota seleccionada de la lista
            ArrayList idCuotas = new ArrayList();

            foreach (ListViewItem item in lvCuotas.Items)
            {
                RadioButton checkConfirm = item.FindControl("rdbUser") as RadioButton;

                if (checkConfirm.Checked)
                {
                    Label id = item.FindControl("lbIdConfirm") as Label;
                    _idCuota = id.Text;
                }
            }
            #endregion

            #region Se realiza el pago de la cuota seleccionada
            List<cItemCCU> items = cItemCCU.GetItemsByCuotas(_idCuota);

            foreach (cItemCCU item in items)
            {
                string _idCuentaCorrienteUsuario = Request["idCC"].ToString();
                cCuota _cuota = cCuota.Load(_idCuota);
                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));

                if (index < 1)
                {
                    #region Pago de cuota
                    if (item.IdEstado == (Int16)eEstadoItem.Cuota)
                    {
                        cCuota cuota = cCuota.Load(item.IdCuota);

                        List<cCuota> cuotasPendientes = cCuota.GetCuotasPendientes(hfIdEmpresa.Value, DateTime.Now);

                        if (cuotasPendientes.Count > 1)
                        {
                            if (flag == false)
                            {
                                if (lastSaldo < 0)
                                    _signoSaldo = lastSaldo * -1;
                                else
                                    _signoSaldo = lastSaldo;

                                #region if (_txtMonto == _signoSaldo)
                                if (_txtMonto == _signoSaldo)
                                {
                                    foreach (cCuota c in cuotasPendientes)
                                    {
                                        c.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                        c.Save();
                                        Pago(c.Id, c.Vencimiento1);

                                        //Actualizo saldo
                                        cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                                        fp.Saldo = fp.Saldo - fp.Monto;
                                        fp.Save();
                                    }
                                    foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                    {
                                        iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                        iccu.Save();
                                    }

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.Concepto = txtConceptoPago.Text;
                                    ccuNew.Debito = 0;
                                    ccuNew.Credito = _txtMonto;
                                    ccuNew.Saldo = _nuevoSaldo;
                                    ccuNew.IdCuota = cuota.Id;
                                    ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuNew.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;
                                    int _idItemCCU = ccuNew.Save();

                                    hfIdItemCC.Value = _idItemCCU.ToString();
                                }
                                #endregion

                                #region if (_txtMonto > _signoSaldo)
                                if (_txtMonto > _signoSaldo)
                                {
                                    decimal diferencia = _txtMonto - item.Debito;
                                    decimal resto = diferencia - (item.Saldo - item.Debito);
                                    _nuevoSaldo = resto;

                                    foreach (cCuota c in cuotasPendientes)
                                    {
                                        c.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                        c.Save();
                                        Pago(c.Id, c.Vencimiento1);

                                        //Actualizo saldo
                                        cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                                        fp.Saldo = fp.Saldo - fp.Monto;
                                        fp.Save();
                                    }
                                    item.IdEstado = (Int16)eEstadoItem.Pagado;
                                    item.Save();

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.Concepto = txtConceptoPago.Text;
                                    ccuNew.Debito = 0;
                                    ccuNew.Credito = _txtMonto;
                                    ccuNew.Saldo = _nuevoSaldo;
                                    ccuNew.IdCuota = cuota.Id;
                                    ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuNew.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;
                                    int _idItemCCU = ccuNew.Save();

                                    hfIdItemCC.Value = _idItemCCU.ToString();

                                    //Genera el recibo del pago
                                    cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), _txtMonto);

                                    cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente); //Historial de pagos
                                    cRegistroPago registro = new cRegistroPago(DateTime.Now, _txtMonto, "", "", hfIdEmpresa.Value, -1, (Int16)estadoCuenta_Cuota.Pagado, Convert.ToInt16(cc.Id), 1, 0);
                                }
                                #endregion

                                #region if (_txtMonto < _signoSaldo)
                                if (_txtMonto < _signoSaldo)
                                {
                                    string _lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
                                    _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + _txtMonto;

                                    decimal suma = 0;
                                    foreach (cItemCCU i in cItemCCU.GetItemsByCuotas(cuota.Id))
                                    {
                                        suma += i.Credito;
                                    }

                                    suma = suma + _txtMonto;

                                    if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                    {
                                        string venc = null;
                                        if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                            venc = String.Format("{0:#,#.00}", cuota.Vencimiento1);
                                        else
                                            venc = String.Format("{0:#,#.00}", cuota.Vencimiento1 * cValorDolar.LoadActualValue());

                                        if (Convert.ToDecimal(String.Format("{0:#,#.00}", suma)) == Convert.ToDecimal(venc))
                                        {
                                            cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            cuota.Save();
                                            Pago(cuota.Id, cuota.Vencimiento1);

                                            //Actualizo saldo
                                            cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                            fp.Saldo = fp.Saldo - fp.Monto;
                                            fp.Save();

                                            item.IdEstado = (Int16)eEstadoItem.Pagado;
                                            item.Save();
                                        }
                                    }

                                    if (cuota.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                    {
                                        string venc = null;
                                        if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                            venc = String.Format("{0:#,#0.00}", cuota.Vencimiento2);
                                        else
                                            venc = String.Format("{0:#,#0.00}", cuota.Vencimiento2 * cValorDolar.LoadActualValue());

                                        if (suma == Convert.ToDecimal(venc))
                                        {
                                            cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            cuota.Save();
                                            Pago(cuota.Id, cuota.Vencimiento2);

                                            //Actualizo saldo
                                            cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                            fp.Saldo = fp.Saldo - fp.Monto;
                                            fp.Save();

                                            foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                            {
                                                iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                                iccu.Save();
                                            }
                                        }
                                    }

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.Concepto = txtConceptoPago.Text;
                                    ccuNew.Debito = 0;
                                    ccuNew.Credito = _txtMonto;
                                    ccuNew.Saldo = _nuevoSaldo;
                                    ccuNew.IdCuota = cuota.Id;
                                    ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuNew.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;
                                    int _idItemCCU = ccuNew.Save();

                                    hfIdItemCC.Value = _idItemCCU.ToString();

                                    //Genera el recibo del pago
                                    cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), _txtMonto);

                                    cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente); //Historial de pagos
                                    cRegistroPago registro = new cRegistroPago(DateTime.Now, _txtMonto, "", "", hfIdEmpresa.Value, -1, (Int16)estadoCuenta_Cuota.Pagado, Convert.ToInt16(cc.Id), 1, 0);
                                }
                                #endregion

                                flag = true;
                            }
                        }
                        else
                        {
                            if (flag == false)
                            {
                                string _lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
                                _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + _txtMonto;

                                #region if ((ccu.Saldo * -1) <= _txtMonto)
                                if ((item.Saldo * -1) <= _txtMonto)
                                {
                                    item.IdEstado = (Int16)eEstadoItem.Pagado;
                                    item.Save();
                                }
                                else
                                {
                                    decimal sum = 0;
                                    foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                    {
                                        if (Convert.ToInt64(iccu.Id) < Convert.ToInt64(item.Id))
                                        {
                                            sum = sum + (iccu.Debito * -1);
                                        }
                                    }

                                    if ((item.Saldo * -1) <= (sum + _txtMonto))
                                    {
                                        item.IdEstado = (Int16)eEstadoItem.Pagado;
                                        item.Save();
                                    }
                                }
                                #endregion

                                decimal suma = 0;
                                decimal _monto = 0;
                                foreach (cItemCCU i in cItemCCU.GetItemsByCuotas(cuota.Id))//Para el caso que se hacen pagos parciales
                                {
                                    suma += i.Credito;
                                }

                                suma = suma + _txtMonto;

                                #region if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                {
                                    string venc = null;
                                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                        venc = String.Format("{0:#,#.00}", cuota.Vencimiento1);
                                    else
                                        venc = String.Format("{0:#,#.00}", cuota.Vencimiento1 * cValorDolar.LoadActualValue());

                                    //La segunda parte del IF, es para los casos que tenian saldo a favor
                                    if (Convert.ToDecimal(String.Format("{0:#,#.00}", suma)) >= Convert.ToDecimal(venc) || Convert.ToDecimal(String.Format("{0:#,#.00}", _lastSaldo)) * -1 <= Convert.ToDecimal(String.Format("{0:#,#.00}", suma)))
                                    {
                                        cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                        cuota.Save();
                                        Pago(cuota.Id, cuota.Vencimiento1);

                                        //Actualizo saldo
                                        cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                        fp.Saldo = fp.Saldo - fp.Monto;
                                        fp.Save();

                                        foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                        {
                                            iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                            iccu.Save();
                                        }
                                    }

                                    _monto = cuota.Vencimiento1;
                                }
                                #endregion

                                #region if (cuota.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                if (cuota.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                {
                                    string venc = null;
                                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                        venc = String.Format("{0:#,#.00}", cuota.Vencimiento2);
                                    else
                                        venc = String.Format("{0:#,#.00}", cuota.Vencimiento2 * cValorDolar.LoadActualValue());

                                    if (Convert.ToDecimal(String.Format("{0:#,#.00}", suma)) == Convert.ToDecimal(venc))
                                    {
                                        cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                        cuota.Save();
                                        Pago(cuota.Id, cuota.Vencimiento2);

                                        //Actualizo saldo
                                        cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                        fp.Saldo = fp.Saldo - fp.Monto;
                                        fp.Save();

                                        foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                        {
                                            iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                            iccu.Save();
                                        }
                                    }

                                    _monto = cuota.Vencimiento2;
                                }
                                #endregion

                                cItemCCU ccuNew = new cItemCCU();
                                ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                ccuNew.Fecha = DateTime.Now;
                                ccuNew.Concepto = txtConceptoPago.Text;
                                ccuNew.Debito = 0;
                                ccuNew.Credito = _txtMonto;
                                ccuNew.Saldo = _nuevoSaldo;
                                ccuNew.IdCuota = cuota.Id;
                                ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                ccuNew.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;
                                int _idItemCCU = ccuNew.Save();

                                hfIdItemCC.Value = _idItemCCU.ToString();

                                //Genera el recibo del pago
                                cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), _txtMonto);
                                cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente); //Historial de pagos

                                string _idRegistro = CrearRegistroPago(_monto, hfIdEmpresa.Value, cc.Id);
                                cuota.IdRegistroPago = _idRegistro;
                                cuota.Save();
                            }
                        }
                        index++;
                    }
                    #endregion
                }
            }

            lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
            lvCC.DataBind();
            #endregion

            #region Limpiar
            ClosePagoCuota();

            pnlPago.Visible = false;
            pnlCredito.Visible = false;
            pnlNDebito.Visible = false;
            #endregion
        }

        protected void btnContinuarPagoCuota_Click(object sender, EventArgs e)
        {
            bool flag = false;

            foreach (ListViewItem item in lvCuotas.Items)
            {
                RadioButton checkConfirm = item.FindControl("rdbUser") as RadioButton;

                if (checkConfirm.Checked)
                {
                    Label id = item.FindControl("lbIdConfirm") as Label;
                    flag = true;
                }
            }

            if (flag == false)
            {
                pnlMensajePago.Visible = true;
                lbMensajePago.Text = "Seleccione un pago de la lista.";
            }
            else
            {
                pnlMensajePago.Visible = false;
                PagoCuotaMonto.Visible = true;
            }
        }

        protected void btnCancelarPagoCuota_Click(object sender, EventArgs e)
        {
            pnlPago.Visible = false;
            rblPago.ClearSelection();
            pnlPagoCuota.Visible = false;
        }

        protected void btnPagoCancelar_Click(object sender, EventArgs e)
        {
            pnlPago.Visible = false;
            rblPago.ClearSelection();
            pnlPagoCuota.Visible = false;
            txtImportePago.Text = "";
            txtImportePago.Enabled = false;
            rblMonedaPago.ClearSelection();
            PagoCuotaMonto.Visible = false;
        }
        #endregion

        #region Otros Pagos
        protected void rblMonedaOtrosPago_TextChanged(object sender, EventArgs e)
        {
            if (rblMonedaOtrosPago.SelectedValue == tipoMoneda.Pesos.ToString() || rblMonedaOtrosPago.SelectedValue == tipoMoneda.Dolar.ToString())
                txtImporteOtrosPago.Enabled = true;
        }

        protected void btnOtrosPago_Click(object sender, EventArgs e)
        {
            decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));

            Decimal _txtMonto = Convert.ToDecimal(txtImporteOtrosPago.Text);
            if (rblMonedaOtrosPago.SelectedValue == tipoMoneda.Dolar.ToString())
                _txtMonto = Convert.ToDecimal(txtImporteOtrosPago.Text) * cValorDolar.LoadActualValue();

            cItemCCU ccuNew = new cItemCCU();
            ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
            ccuNew.Fecha = DateTime.Now;
            ccuNew.Concepto = txtConceptoOtrosPago.Text;
            ccuNew.Debito = 0;
            ccuNew.Credito = _txtMonto;
            ccuNew.Saldo = lastSaldo + _txtMonto;
            ccuNew.IdCuota = "-1";
            ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
            ccuNew.TipoOperacion = (Int16)eTipoOperacion.OtrosPago;
            int _idItemCCU = ccuNew.Save();

            //Genera el recibo del pago
            cReciboCuota recibo = cReciboCuota.CrearRecibo("-1", _idItemCCU.ToString(), _txtMonto);

            lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
            lvCC.DataBind();

            pnlOtrosPago.Visible = false;
            txtImporteOtrosPago.Text = "";
            txtImporteOtrosPago.Enabled = false;
            txtConceptoOtrosPago.Text = "";
            pnlOk.Visible = true;
            pnlOkNotaDebito.Visible = false;
        }
        #endregion

        #region Adelanto de cuotas
        protected void cbProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            cCuentaCorrienteUsuario ccu = cCuentaCorrienteUsuario.Load(Request["idCC"].ToString());

            cbFormaPago.Enabled = true;
            cbFormaPago.DataSource = cFormaPagoOV.GetDataTableFormaPago(ccu.IdEmpresa, cbProyectos.SelectedValue);
            cbFormaPago.DataValueField = "id";
            cbFormaPago.DataTextField = "descripcion";
            cbFormaPago.DataBind();
            ListItem fp = new ListItem("Seleccione una forma de pago...", "0");
            cbFormaPago.Items.Insert(0, fp);
            cbFormaPago.SelectedIndex = 0;
        }

        protected void cbFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            cUnidad unidad = cUnidad.Load(cbProyectos.SelectedValue);
            string eu = cEmpresaUnidad.GetUnidadByIdUnidad(unidad.Id);
            cCuentaCorriente cc = cCuentaCorriente.GetCCByIdUnidad(unidad.Id, eu);
            hfIdCC.Value = cc.Id;

            List<cCuota> pendientes = cCuota.GetCuotasPendientesByIdCC(cc.Id, cbFormaPago.SelectedValue);

            if (pendientes.Count == 0)
            {
                if (cbFormaPago.SelectedIndex != 0)
                    pnlCantCuotas.Visible = true;
            }
            else
            {
                pnlAdelanto.Visible = true;
                hfIdFormaPagoOV.Value = pendientes[0].IdFormaPagoOV;
            }
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            foreach (cCuota cuota in cCuota.GetCuotasActivasByIdCC(hfIdCC.Value))
            {
                cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);

                if (fp.Id == cbFormaPago.SelectedValue)
                {
                    hfIdFormaPagoOV.Value = cuota.IdFormaPagoOV;
                    hfIdCuota.Value = cuota.Id;
                }
            }

            usrCtrl.CargarListView(hfIdCC.Value, hfIdFormaPagoOV.Value, txtCantCuotas.Text);
            pnlListadoCuotas.Visible = true;
            pnlConceptoAdelanto.Visible = true;
        }

        protected void btnAdelantoCuota_Click(object sender, EventArgs e)
        {
            decimal _nuevoSaldo = 0;
            string lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
            Decimal _saldo = Convert.ToDecimal(lastSaldo);

            decimal montoTotal = usrCtrl.sumMontoCuota();

            //Débito del adelanto
            _nuevoSaldo = _saldo + (montoTotal * -1);

            cItemCCU ccuDebitoNew = new cItemCCU();
            ccuDebitoNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
            ccuDebitoNew.Fecha = DateTime.Now;
            ccuDebitoNew.Concepto = "Importe del adelanto de cuotas";
            ccuDebitoNew.Debito = montoTotal * -1;
            ccuDebitoNew.Credito = 0;
            ccuDebitoNew.Saldo = _nuevoSaldo;
            ccuDebitoNew.IdCuota = hfIdCuota.Value;
            ccuDebitoNew.IdEstado = (Int16)eEstadoItem.Pagado;
            ccuDebitoNew.TipoOperacion = (Int16)eTipoOperacion.Cuota;
            ccuDebitoNew.Save();

            //Crédito del adelanto
            _nuevoSaldo = _nuevoSaldo + montoTotal;

            cItemCCU ccuCreditoNew = new cItemCCU();
            ccuCreditoNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
            ccuCreditoNew.Fecha = DateTime.Now;
            ccuCreditoNew.Concepto = txtConceptoAdelantoCuota.Text;
            ccuCreditoNew.Debito = 0;
            ccuCreditoNew.Credito = montoTotal;
            ccuCreditoNew.Saldo = _nuevoSaldo;
            ccuCreditoNew.IdCuota = hfIdCuota.Value;
            ccuCreditoNew.IdEstado = (Int16)eEstadoItem.Pagado;
            ccuCreditoNew.TipoOperacion = (Int16)eTipoOperacion.Adelanto;
            int _idItemCCU = ccuCreditoNew.Save();

            //Genera el recibo del pago
            ArrayList _idCuotas = usrCtrl.GetIdCuotas();
            cReciboCuota recibo = null;
            Int64 _nro = 0;
            foreach (var _id in _idCuotas)
            {
                cCuota cuota = cCuota.Load(_id.ToString());

                if (_nro <= 0)
                {
                    recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), montoTotal);
                    _nro = recibo.Nro;
                }
                else
                {
                    recibo = new cReciboCuota(cuota.Id, _idItemCCU.ToString(), _nro, DateTime.Now);
                    recibo.Monto = montoTotal;
                    recibo._Papelera = 1;
                    recibo.Save();
                }
            }

            usrCtrl.btnFinalizarAdelanto_Click(hfIdCC.Value, montoTotal);

            lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
            lvCC.DataBind();

            hfIdItemCC.Value = _idItemCCU.ToString();

            rblPago.ClearSelection();
            pnlCantCuotas.Visible = false;
            txtCantCuotas.Text = "";
            pnlListadoCuotas.Visible = false;
            pnlConceptoAdelanto.Visible = false;
            txtConceptoAdelantoCuota.Text = "";

            pnlAdelantoCuota.Visible = false;
            pnlOk.Visible = true;
            pnlOkNotaDebito.Visible = false;

            pnlPago.Visible = false;
            pnlCredito.Visible = false;
            pnlNDebito.Visible = false;
        }
        #endregion

        #region Cancelar Saldo
        protected void cbProyectosCancelarSaldo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cCuentaCorrienteUsuario ccu = cCuentaCorrienteUsuario.Load(Request["idCC"].ToString());

            decimal _saldoPesos = 0;

            cUnidad unidad = cUnidad.Load(cbProyectosCancelarSaldo.SelectedValue);
            string eu = cEmpresaUnidad.GetUnidadByIdUnidad(unidad.Id);

            cCuentaCorriente cc = cCuentaCorriente.GetCCByIdUnidad(unidad.Id, eu);
            hfIdCC.Value = cc.Id;

            List<cFormaPagoOV> fps = cFormaPagoOV.GetFormaPagoOVByIdOV(cc.IdOperacionVenta);
            decimal saldoCuota = 0;
            foreach (cFormaPagoOV f in fps)
            {
                saldoCuota = cCuota.SaldoCC(cc.Id, f.Id, f.Moneda);

                if (f.GetMoneda == tipoMoneda.Pesos.ToString())
                    _saldoPesos += saldoCuota;
                else
                    _saldoPesos += cValorDolar.ConvertToPeso(saldoCuota, cValorDolar.LoadActualValue());
            }

            lbCancelarSaldo.Text = String.Format("{0:#,#0.00}", _saldoPesos);

            if (_saldoPesos != 0)
                pnlConceptoCancelarSaldo.Visible = true;
        }

        protected void cbFormaPagoCancelarSaldo_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlConceptoCancelarSaldo.Visible = true;

            decimal _saldoPesos = 0;

            cUnidad unidad = cUnidad.Load(cbProyectosCancelarSaldo.SelectedValue);
            string eu = cEmpresaUnidad.GetUnidadByIdUnidad(unidad.Id);

            cCuentaCorriente cc = cCuentaCorriente.GetCCByIdUnidad(unidad.Id, eu);
            hfIdCC.Value = cc.Id;

            List<cFormaPagoOV> fps = cFormaPagoOV.GetFormaPagoOVByIdOV(cc.IdOperacionVenta);
            decimal saldoCuota = 0;
            foreach (cFormaPagoOV f in fps)
            {
                saldoCuota = cCuota.SaldoCC(cc.Id, f.Id, f.Moneda);

                if (f.GetMoneda == tipoMoneda.Pesos.ToString())
                    _saldoPesos += saldoCuota;
                else
                    _saldoPesos += cValorDolar.ConvertToPeso(saldoCuota, cValorDolar.LoadActualValue());
            }

            lbCancelarSaldo.Text = String.Format("{0:#,#0.00}", _saldoPesos);
        }

        protected void btnCancelarSaldo_Click(object sender, EventArgs e)
        {
            decimal _nuevoSaldo = 0;
            string lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
            Decimal _saldo = Convert.ToDecimal(lastSaldo);
            decimal _montoSaldo = Convert.ToDecimal(lbCancelarSaldo.Text);

            //Se actualiza la cuenta corriente como pagada
            cCuentaCorriente cc = cCuentaCorriente.Load(hfIdCC.Value);
            cc.IdEstado = (Int16)estadoCuenta_Cuota.Pagado;
            cc.Save();

            List<cCuota> cuotas = cCuota.GetCuotasActivasAndPendientesByIdCC(hfIdCC.Value);

            //El resto de las cuotas las dejo en 0
            foreach (cCuota c in cuotas)
            {
                c.MontoAjustado = 0;
                c.Monto = 0;
                c.TotalComision = 0;
                c.Vencimiento1 = 0;
                c.Vencimiento2 = 0;
                c.Saldo = 0;
                c.Estado = (Int16)estadoCuenta_Cuota.Anticipo;
                c.Save();
            }

            //Actualizo la cuota del saldo
            cCuota _cuotaActual = cCuota.GetCuotaByNro(hfIdCC.Value, cuotas[0].Nro, null);
            _cuotaActual.Monto = 0;
            _cuotaActual.TotalComision = 0;
            _cuotaActual.Vencimiento1 = 0;
            _cuotaActual.Vencimiento2 = 0;
            _cuotaActual.Saldo = _montoSaldo;
            _cuotaActual.Estado = (Int16)estadoCuenta_Cuota.Anticipo;
            _cuotaActual.Save();

            decimal _montoDebito = _montoSaldo * -1;
            _nuevoSaldo = _saldo + _montoDebito;

            //Genero el item débito de la cuenta corriente
            cItemCCU ccuNewDebito = new cItemCCU();
            ccuNewDebito.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
            ccuNewDebito.Fecha = DateTime.Now;
            ccuNewDebito.Concepto = "Débito por cancelación de saldo";
            ccuNewDebito.Debito = _montoDebito;
            ccuNewDebito.Credito = 0;
            ccuNewDebito.Saldo = _nuevoSaldo;
            ccuNewDebito.IdCuota = "-1";
            ccuNewDebito.IdEstado = (Int16)eEstadoItem.Pagado;
            ccuNewDebito.TipoOperacion = (Int16)eTipoOperacion.Cuota;
            ccuNewDebito.Save();

            _nuevoSaldo = _nuevoSaldo + _montoSaldo;

            //Genero el item crédito de la cuenta corriente
            cItemCCU ccuNewCredito = new cItemCCU();
            ccuNewCredito.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
            ccuNewCredito.Fecha = DateTime.Now;
            ccuNewCredito.Concepto = txtConceptoCancelarSaldo.Text;
            ccuNewCredito.Debito = 0;
            ccuNewCredito.Credito = _montoSaldo;
            ccuNewCredito.Saldo = _nuevoSaldo;
            ccuNewCredito.IdCuota = "-1";
            ccuNewCredito.IdEstado = (Int16)eEstadoItem.Pagado;
            ccuNewCredito.TipoOperacion = (Int16)eTipoOperacion.Saldo;
            int _idItemCCU = ccuNewCredito.Save();

            cReciboCuota recibo = null;
            Int64 _nro = 0;
            foreach (cCuota c in cuotas)
            {
                if (_nro <= 0)
                {
                    recibo = cReciboCuota.CrearRecibo(c.Id, _idItemCCU.ToString(), _montoDebito * -1);
                    _nro = recibo.Nro;
                }
                else
                {
                    recibo = new cReciboCuota(c.Id, _idItemCCU.ToString(), _nro, DateTime.Now);
                    recibo.Monto = _montoDebito * -1;
                    recibo._Papelera = 1;
                    recibo.Save();
                }
            }

            lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
            lvCC.DataBind();

            hfIdItemCC.Value = _idItemCCU.ToString();

            rblPago.ClearSelection();
            pnlCancelarSaldo.Visible = false;
            lbCancelarSaldo.Text = "-";
            txtConceptoCancelarSaldo.Text = "";
            pnlConceptoCancelarSaldo.Visible = false;
            pnlOk.Visible = true;
            pnlOkNotaDebito.Visible = false;

            pnlPago.Visible = false;
            pnlCredito.Visible = false;
            pnlNDebito.Visible = false;
        }
        #endregion

        #region Anular
        protected void btnSiguienteAnular_Click(object sender, EventArgs e)
        {
            pnlConceptoAnular.Visible = true;
            Label nro = null;
            Label tipoDescripcion = null;

            foreach (ListViewItem item in lvAnular.Items)
            {
                RadioButton checkConfirm = item.FindControl("rdbUser") as RadioButton;
                
                if (checkConfirm.Checked)
                {
                    Label id = item.FindControl("lbId") as Label;
                    hfIdComprobanteAnular.Value = id.Text;
                    Label tipo = item.FindControl("lbTipo") as Label;
                    hfTipoComprobanteAnular.Value = tipo.Text;
                    tipoDescripcion = item.FindControl("lbDescripcion") as Label;                    

                    nro = item.FindControl("lbCAC") as Label;
                }
            }

            txtConceptoAnular.Text = "Anulación " + tipoDescripcion.Text + " nro. " + nro.Text;
        }

        protected void btnCancelarAnular_Click(object sender, EventArgs e)
        {
            rblPago.ClearSelection();
            pnlAnular.Visible = false;
            pnlConceptoAnular.Visible = false;
            pnlPago.Visible = false;
        }

        protected void btnAnularCuota_Click(object sender, EventArgs e)
        {
            string _lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
            decimal _monto = 0;
            decimal _nuevoSaldo = 0;

            DateTime date = Convert.ToDateTime(DateTime.Today.Year + " - " + DateTime.Today.Month + " - " + 10);

            #region Recibos
            if (Convert.ToInt16(hfTipoComprobanteAnular.Value) == (Int16)eComprobante.Recibo)
            {
                cReciboCuota recibo = cReciboCuota.Load(hfIdComprobanteAnular.Value);
                cItemCCU item = cItemCCU.Load(recibo.IdItemCCU);

                if (item.TipoOperacion == (Int16)eTipoOperacion.PagoCuota)
                {
                    cCuota cuota = cCuota.Load(recibo.IdCuota);
                    if (cuota.FechaVencimiento1 > date)
                        cuota.Estado = (Int16)estadoCuenta_Cuota.Activa;
                    else
                        cuota.Estado = (Int16)estadoCuenta_Cuota.Pendiente;
                    cuota.Save();

                    cItemCCU itemCuota = cItemCCU.GetFirtsItemCCUByIdCuota(recibo.IdCuota);
                    itemCuota.IdEstado = (Int16)eEstadoItem.Cuota;
                    itemCuota.Save();
                }

                if (item.TipoOperacion == (Int16)eTipoOperacion.Adelanto)
                {
                    //Actualizo los recibos y paso activas las cuotas adelantadas                    
                    List<cCuota> cuotas = cCuota.GetCuotasByNroRecibo(recibo.Nro.ToString());
                    foreach (cCuota c in cuotas)
                    {
                        c.Estado = (Int16)estadoCuenta_Cuota.Activa;
                        c.Save();
                    }

                    List<cReciboCuota> recibos = cReciboCuota.GetRecibosByNroFromItemCCU(recibo.Nro.ToString());
                    foreach (cReciboCuota r in recibos)
                    {
                        r._Papelera = (Int16)papelera.Eliminado;
                        r.Monto = 0;
                        r.Save();
                    }

                    //Actualizo las cuotas
                    cCuota cuota = cCuota.Load(recibo.IdCuota);
                    if(cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                        cIndiceCAC.ActualizarCuotasDeCC(cuota.IdCuentaCorriente, cuota.IdFormaPagoOV, cuota.Nro);
                    else
                        cIndiceCAC.ActualizarCuotasDeCC(cuota.IdCuentaCorriente, cuota.IdFormaPagoOV, Convert.ToInt16(cuota.Nro + 1));
                }

                if (item.TipoOperacion == (Int16)eTipoOperacion.Saldo)
                {
                    //Actualizo los recibos y paso activas las cuotas adelantadas 
                    List<cCuota> cuotas = cCuota.GetCuotasByNroRecibo(recibo.Nro.ToString());

                    cCuentaCorriente cc = cCuentaCorriente.Load(cuotas[0].IdCuentaCorriente);
                    cc.IdEstado = (Int16)estadoCuenta_Cuota.Activa;
                    cc.Save();

                    foreach (cCuota c in cuotas)
                    {
                        c.Estado = (Int16)estadoCuenta_Cuota.Activa;
                        c.Save();
                    }

                    List<cReciboCuota> recibos = cReciboCuota.GetRecibosByNroFromItemCCU(recibo.Nro.ToString());
                    foreach (cReciboCuota r in recibos)
                    {
                        r._Papelera = (Int16)papelera.Eliminado;
                        r.Monto = 0;
                        r.Save();
                    }

                    //Actualizo las cuotas                    
                    List<cFormaPagoOV> fps = cFormaPagoOV.GetFormaPagoOVByIdOV(cc.IdOperacionVenta);
                    foreach (cFormaPagoOV f in fps)
                    {
                        cCuota cuota = cCuota.GetLastPay(cc.Id, f.Id);
                        if (cuota != null)
                            cIndiceCAC.ActualizarCuotasDeCC(cuota.IdCuentaCorriente, cuota.IdFormaPagoOV, Convert.ToInt16(cuota.Nro + 1));
                        else
                            cIndiceCAC.ActualizarCuotasDeCC(cc.Id, f.Id, 1);
                    }
                }

                //Item con el débito
                _monto = recibo.Monto * -1;
                _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + _monto;

                cItemCCU ccuNew = new cItemCCU();
                ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                ccuNew.Fecha = DateTime.Now;
                ccuNew.Concepto = txtConceptoAnular.Text;
                ccuNew.Debito = _monto;
                ccuNew.Credito = 0;
                ccuNew.Saldo = _nuevoSaldo;
                ccuNew.IdCuota = recibo.IdCuota;
                ccuNew.IdEstado = (Int16)eEstadoItem.Anular;
                ccuNew.TipoOperacion = (Int16)eTipoOperacion.Anular;
                ccuNew.Save();
                                
                recibo._Papelera = (Int16)papelera.Eliminado;
                recibo.Monto = 0;
                recibo.Save();
            }
            #endregion

            #region Condonación
            if (Convert.ToInt16(hfTipoComprobanteAnular.Value) == (Int16)eComprobante.Condonacion)
            {
                cCondonacion recibo = cCondonacion.Load(hfIdComprobanteAnular.Value);
                cItemCCU item = cItemCCU.Load(recibo.IdItemCCU);

                if (item.TipoOperacion == (Int16)eTipoOperacion.Condonacion)
                {
                    cCuota cuota = cCuota.Load(recibo.IdCuota);
                    if (cuota.FechaVencimiento1 >= date)
                        cuota.Estado = (Int16)estadoCuenta_Cuota.Activa;
                    else
                        cuota.Estado = (Int16)estadoCuenta_Cuota.Pendiente;
                    cuota.Save();

                    cItemCCU itemCuota = cItemCCU.GetFirtsItemCCUByIdCuota(recibo.IdCuota);
                    itemCuota.IdEstado = (Int16)eEstadoItem.Cuota;
                    itemCuota.Save();
                }

                //Item con el débito
                _monto = recibo.Monto * -1;
                _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + _monto;

                cItemCCU ccuNew = new cItemCCU();
                ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                ccuNew.Fecha = DateTime.Now;
                ccuNew.Concepto = txtConceptoAnular.Text;
                ccuNew.Debito = _monto;
                ccuNew.Credito = 0;
                ccuNew.Saldo = _nuevoSaldo;
                ccuNew.IdCuota = recibo.IdCuota;
                ccuNew.IdEstado = (Int16)eEstadoItem.Anular;
                ccuNew.TipoOperacion = (Int16)eTipoOperacion.Anular;
                ccuNew.Save();

                recibo._Papelera = (Int16)papelera.Eliminado;
                recibo.Monto = 0;
                recibo.Save();
            }
            #endregion

            lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
            lvCC.DataBind();

            pnlPago.Visible = false;
            CloseAnular();

            pnlOk.Visible = false;
            pnlOkNotaDebito.Visible = false;
        }
        #endregion

        #region Anular reserva
        protected void btnSiguienteReservas_Click(object sender, EventArgs e)
        {
            int aux = 0;
            foreach (ListViewItem item in lvReservas.Items)
            {
                CheckBox checkConfirm = item.FindControl("chBoxConfirm") as CheckBox;

                if (checkConfirm.Checked)
                    aux++;

                if (aux <= lvReservas.Items.Count)
                {
                    pnlConceptoAnularReserva.Visible = true;
                    pnlMensajeReserva.Visible = false;
                    Label id = item.FindControl("lbIdReserva") as Label;
                    hfIdItemCCU.Value += cReserva.Load(id.Text).IdItemCCU + ",";
                }
                else
                {
                    pnlMensajeReserva.Visible = true;
                }
            }
        }

        protected void btnCancelarReservas_Click(object sender, EventArgs e)
        {
            pnlReservas.Visible = false;
            pnlAnularReserva.Visible = false;
        }

        protected void btnAnularReserva_Click(object sender, EventArgs e)
        {
            try
            {
                if (pnlReservas.Visible == true)
                {
                    hfIdItemCCU.Value = "";
                    foreach (ListViewItem item in lvReservas.Items)
                    {
                        CheckBox check = item.FindControl("chBoxConfirm") as CheckBox;

                        if (check.Checked)
                        {
                            Label id = item.FindControl("lbIdItemCCU") as Label;
                            hfIdItemCCU.Value += id.Text + ",";
                        }
                    }
                }

                Char delimiter = ',';
                String[] _items = hfIdItemCCU.Value.Split(delimiter);

                foreach (var it in _items)
                {
                    if (!string.IsNullOrEmpty(it.ToString()))
                    {
                        cItemCCU item = cItemCCU.Load(it.ToString());
                        item.IdEstado = (Int16)eEstadoItem.Pagado;
                        item.Save();

                        cReserva reserva = cReserva.GetReservaByIdItemCCU(item.Id);

                        decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));
                        decimal _monto = item.Credito * -1;
                        decimal _nuevoSaldo = lastSaldo + _monto;

                        cItemCCU ccuNew = new cItemCCU();
                        ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                        ccuNew.Fecha = DateTime.Now;
                        ccuNew.Concepto = txtConceptoAnularReserva.Text;
                        ccuNew.Debito = _monto;
                        ccuNew.Credito = 0;
                        ccuNew.Saldo = _nuevoSaldo;
                        ccuNew.IdCuota = "-1";
                        ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                        ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                        int _idItemCCU = ccuNew.Save();

                        //Genera el recibo del pago
                        cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCU.ToString(), _monto);

                        cUnidad unidad = cUnidad.Load(reserva.IdUnidad);
                        unidad.IdEstado = Convert.ToString((Int16)estadoUnidad.Disponible);
                        unidad.Save();

                        string _idEu = cEmpresaUnidad.GetUnidadByIdUnidad(reserva.IdUnidad);
                        cEmpresaUnidad eu = cEmpresaUnidad.Load(_idEu);
                        eu.Papelera = (Int16)papelera.Eliminado;
                        eu.Save();

                        reserva.Papelera = (Int16)papelera.Eliminado;
                        reserva.Save();
                    }
                }

                lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
                lvCC.DataBind();

                hfIdItemCCU.Value = "";
                rblPago.ClearSelection();
                pnlAnularReserva.Visible = false;
                lbNivelReserva.Text = "-";
                lbUnidadReserva.Text = "-";
                pnlOneReserva.Visible = false;
                pnlReservas.Visible = false;
                pnlConceptoAnularReserva.Visible = false;
                txtConceptoAnularReserva.Text = "-";

                pnlOk.Visible = true;
                pnlOkNotaDebito.Visible = false;
                pnlMensajeReserva.Visible = false;

                pnlPago.Visible = false;
                pnlCredito.Visible = false;
                pnlNDebito.Visible = false;
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("DetalleCC - " + DateTime.Now + "- " + ex.Message + " - btnAnularReserva_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }
        #endregion

        #region Condonación
        protected void rblMonedaCondonacion_TextChanged(object sender, EventArgs e)
        {
            if (rblMonedaCondonacion.SelectedValue == tipoMoneda.Pesos.ToString() || rblMonedaCondonacion.SelectedValue == tipoMoneda.Dolar.ToString())
                txtImporteCondonacion.Enabled = true;
        }

        protected void btnCondonacion_Click(object sender, EventArgs e)
        {
            #region Variables
            decimal _nuevoSaldo = 0;
            bool flag = false;
            decimal _signoSaldo = 0;
            int index = 0;
            string _idCuota = null;
            #endregion

            #region Convertir a Dolar
            Decimal _txtMonto = Convert.ToDecimal(txtImporteCondonacion.Text);
            if (rblMonedaCondonacion.SelectedValue == tipoMoneda.Dolar.ToString())
                _txtMonto = Convert.ToDecimal(txtImporteCondonacion.Text) * cValorDolar.LoadActualValue();
            #endregion
            
            #region Se busca la cuota seleccionada de la lista
            ArrayList idCuotas = new ArrayList();

            foreach (ListViewItem item in lvCondonacion.Items)
            {
                RadioButton checkConfirm = item.FindControl("rdbCondonacion") as RadioButton;

                if (checkConfirm.Checked)
                {
                    Label id = item.FindControl("lbIdConfirm") as Label;
                    _idCuota = id.Text;
                }
            }
            #endregion

            #region Se realiza el pago de la cuota seleccionada
            List<cItemCCU> items = cItemCCU.GetItemsByCuotas(_idCuota);

            foreach (cItemCCU item in items)
            {
                string _idCuentaCorrienteUsuario = Request["idCC"].ToString();
                cCuota _cuota = cCuota.Load(_idCuota);
                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));

                if (index < 1)
                {
                    #region Pago de cuota
                    if (item.IdEstado == (Int16)eEstadoItem.Cuota)
                    {
                        cCuota cuota = cCuota.Load(item.IdCuota);

                        List<cCuota> cuotasPendientes = cCuota.GetCuotasPendientes(hfIdEmpresa.Value, DateTime.Now);

                        if (cuotasPendientes.Count > 1)
                        {
                            if (flag == false)
                            {
                                if (lastSaldo < 0)
                                    _signoSaldo = lastSaldo * -1;
                                else
                                    _signoSaldo = lastSaldo;

                                #region if (_txtMonto == _signoSaldo)
                                if (_txtMonto == _signoSaldo)
                                {
                                    foreach (cCuota c in cuotasPendientes)
                                    {
                                        c.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                        c.Save();
                                        Pago(c.Id, c.Vencimiento1);

                                        //Actualizo saldo
                                        cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                                        fp.Saldo = fp.Saldo - fp.Monto;
                                        fp.Save();
                                    }
                                    foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                    {
                                        iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                        iccu.Save();
                                    }

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.Concepto = txtConceptoCondonacion.Text;
                                    ccuNew.Debito = 0;
                                    ccuNew.Credito = _txtMonto;
                                    ccuNew.Saldo = _nuevoSaldo;
                                    ccuNew.IdCuota = cuota.Id;
                                    ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuNew.TipoOperacion = (Int16)eTipoOperacion.Condonacion;
                                    int _idItemCCU = ccuNew.Save();

                                    hfIdItemCC.Value = _idItemCCU.ToString();
                                }
                                #endregion

                                #region if (_txtMonto > _signoSaldo)
                                if (_txtMonto > _signoSaldo)
                                {
                                    decimal diferencia = _txtMonto - item.Debito;
                                    decimal resto = diferencia - (item.Saldo - item.Debito);
                                    _nuevoSaldo = resto;

                                    foreach (cCuota c in cuotasPendientes)
                                    {
                                        c.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                        c.Save();
                                        Pago(c.Id, c.Vencimiento1);

                                        //Actualizo saldo
                                        cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                                        fp.Saldo = fp.Saldo - fp.Monto;
                                        fp.Save();
                                    }
                                    item.IdEstado = (Int16)eEstadoItem.Pagado;
                                    item.Save();

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.Concepto = txtConceptoCondonacion.Text;
                                    ccuNew.Debito = 0;
                                    ccuNew.Credito = _txtMonto;
                                    ccuNew.Saldo = _nuevoSaldo;
                                    ccuNew.IdCuota = cuota.Id;
                                    ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuNew.TipoOperacion = (Int16)eTipoOperacion.Condonacion;
                                    int _idItemCCU = ccuNew.Save();

                                    hfIdItemCC.Value = _idItemCCU.ToString();

                                    //Genera el recibo del pago
                                    cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), _txtMonto);

                                    cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente); //Historial de pagos
                                    cRegistroPago registro = new cRegistroPago(DateTime.Now, _txtMonto, "", "", hfIdEmpresa.Value, -1, (Int16)estadoCuenta_Cuota.Pagado, Convert.ToInt16(cc.Id), 1, 0);
                                }
                                #endregion

                                #region if (_txtMonto < _signoSaldo)
                                if (_txtMonto < _signoSaldo)
                                {
                                    string _lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
                                    _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + _txtMonto;

                                    decimal suma = 0;
                                    foreach (cItemCCU i in cItemCCU.GetItemsByCuotas(cuota.Id))
                                    {
                                        suma += i.Credito;
                                    }

                                    suma = suma + _txtMonto;

                                    if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                    {
                                        string venc = null;
                                        if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                            venc = String.Format("{0:#,#.00}", cuota.Vencimiento1);
                                        else
                                            venc = String.Format("{0:#,#.00}", cuota.Vencimiento1 * cValorDolar.LoadActualValue());

                                        if (Convert.ToDecimal(String.Format("{0:#,#.00}", suma)) == Convert.ToDecimal(venc))
                                        {
                                            cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            cuota.Save();
                                            Pago(cuota.Id, cuota.Vencimiento1);

                                            //Actualizo saldo
                                            cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                            fp.Saldo = fp.Saldo - fp.Monto;
                                            fp.Save();

                                            item.IdEstado = (Int16)eEstadoItem.Pagado;
                                            item.Save();
                                        }
                                    }

                                    if (cuota.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                    {
                                        string venc = null;
                                        if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                            venc = String.Format("{0:#,#0.00}", cuota.Vencimiento2);
                                        else
                                            venc = String.Format("{0:#,#0.00}", cuota.Vencimiento2 * cValorDolar.LoadActualValue());

                                        if (suma == Convert.ToDecimal(venc))
                                        {
                                            cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            cuota.Save();
                                            Pago(cuota.Id, cuota.Vencimiento2);

                                            //Actualizo saldo
                                            cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                            fp.Saldo = fp.Saldo - fp.Monto;
                                            fp.Save();

                                            foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                            {
                                                iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                                iccu.Save();
                                            }
                                        }
                                    }

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.Concepto = txtConceptoCondonacion.Text;
                                    ccuNew.Debito = 0;
                                    ccuNew.Credito = _txtMonto;
                                    ccuNew.Saldo = _nuevoSaldo;
                                    ccuNew.IdCuota = cuota.Id;
                                    ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuNew.TipoOperacion = (Int16)eTipoOperacion.Condonacion;
                                    int _idItemCCU = ccuNew.Save();

                                    hfIdItemCC.Value = _idItemCCU.ToString();

                                    //Genera el recibo del pago
                                    cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), _txtMonto);

                                    cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente); //Historial de pagos
                                    cRegistroPago registro = new cRegistroPago(DateTime.Now, _txtMonto, "", "", hfIdEmpresa.Value, -1, (Int16)estadoCuenta_Cuota.Pagado, Convert.ToInt16(cc.Id), 1, 0);
                                }
                                #endregion

                                flag = true;
                            }
                        }
                        else
                        {
                            if (flag == false)
                            {
                                string _lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
                                _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + _txtMonto;

                                #region if ((ccu.Saldo * -1) <= _txtMonto)
                                if ((item.Saldo * -1) <= _txtMonto)
                                {
                                    item.IdEstado = (Int16)eEstadoItem.Pagado;
                                    item.Save();
                                }
                                else
                                {
                                    decimal sum = 0;
                                    foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                    {
                                        if (Convert.ToInt64(iccu.Id) < Convert.ToInt64(item.Id))
                                        {
                                            sum = sum + (iccu.Debito * -1);
                                        }
                                    }

                                    if ((item.Saldo * -1) <= (sum + _txtMonto))
                                    {
                                        item.IdEstado = (Int16)eEstadoItem.Pagado;
                                        item.Save();
                                    }
                                }
                                #endregion

                                decimal suma = 0;
                                decimal _monto = 0;
                                foreach (cItemCCU i in cItemCCU.GetItemsByCuotas(cuota.Id))//Para el caso que se hacen pagos parciales
                                {
                                    suma += i.Credito;
                                }

                                suma = suma + _txtMonto;

                                #region if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                {
                                    string venc = null;
                                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                        venc = String.Format("{0:#,#.00}", cuota.Vencimiento1);
                                    else
                                        venc = String.Format("{0:#,#.00}", cuota.Vencimiento1 * cValorDolar.LoadActualValue());

                                    //La segunda parte del IF, es para los casos que tenian saldo a favor
                                    if (Convert.ToDecimal(String.Format("{0:#,#.00}", suma)) >= Convert.ToDecimal(venc) || Convert.ToDecimal(String.Format("{0:#,#.00}", _lastSaldo)) * -1 <= Convert.ToDecimal(String.Format("{0:#,#.00}", suma)))
                                    {
                                        cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                        cuota.Save();
                                        Pago(cuota.Id, cuota.Vencimiento1);

                                        //Actualizo saldo
                                        cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                        fp.Saldo = fp.Saldo - fp.Monto;
                                        fp.Save();

                                        foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                        {
                                            iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                            iccu.Save();
                                        }
                                    }

                                    _monto = cuota.Vencimiento1;
                                }
                                #endregion

                                #region if (cuota.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                if (cuota.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                {
                                    string venc = null;
                                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                        venc = String.Format("{0:#,#.00}", cuota.Vencimiento2);
                                    else
                                        venc = String.Format("{0:#,#.00}", cuota.Vencimiento2 * cValorDolar.LoadActualValue());

                                    if (Convert.ToDecimal(String.Format("{0:#,#.00}", suma)) == Convert.ToDecimal(venc))
                                    {
                                        cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                        cuota.Save();
                                        Pago(cuota.Id, cuota.Vencimiento2);

                                        //Actualizo saldo
                                        cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                        fp.Saldo = fp.Saldo - fp.Monto;
                                        fp.Save();

                                        foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(item.IdCuota))
                                        {
                                            iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                            iccu.Save();
                                        }
                                    }

                                    _monto = cuota.Vencimiento2;
                                }
                                #endregion

                                cItemCCU ccuNew = new cItemCCU();
                                ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                ccuNew.Fecha = DateTime.Now;
                                ccuNew.Concepto = txtConceptoCondonacion.Text;
                                ccuNew.Debito = 0;
                                ccuNew.Credito = _txtMonto;
                                ccuNew.Saldo = _nuevoSaldo;
                                ccuNew.IdCuota = cuota.Id;
                                ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                ccuNew.TipoOperacion = (Int16)eTipoOperacion.Condonacion;
                                int _idItemCCU = ccuNew.Save();

                                hfIdItemCC.Value = _idItemCCU.ToString();

                                //Genera el recibo del pago
                                cCondonacion condonacion = cCondonacion.CrearCondonacion(cuota.Id, _idItemCCU.ToString(), _txtMonto);

                                cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente); //Historial de pagos

                                string _idRegistro = CrearRegistroPago(_monto, hfIdEmpresa.Value, cc.Id);
                                cuota.IdRegistroPago = _idRegistro;
                                cuota.Save();
                            }
                        }
                        index++;
                    }
                    #endregion
                }
            }

            lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
            lvCC.DataBind();
            #endregion

            #region Limpiar
            CloseCondonacion();

            pnlPago.Visible = false;
            pnlCredito.Visible = false;
            pnlNDebito.Visible = false;
            #endregion
        }

        protected void btnContinuarCondonacion_Click(object sender, EventArgs e)
        {
            bool flag = false;

            foreach (ListViewItem item in lvCondonacion.Items)
            {
                RadioButton checkConfirm = item.FindControl("rdbCondonacion") as RadioButton;

                if (checkConfirm.Checked)
                {
                    Label id = item.FindControl("lbIdConfirm") as Label;
                    flag = true;
                }
            }

            if (flag == false)
            {
                pnlMensajePago.Visible = true;
                lbMensajePago.Text = "Seleccione un pago de la lista.";
            }
            else
            {
                pnlMensajePago.Visible = false;
                CondonacionCuotaMonto.Visible = true;
            }
        }

        protected void btnCancelarCondonacion_Click(object sender, EventArgs e)
        {
            CloseCondonacion();

            pnlPago.Visible = false;
            pnlCredito.Visible = false;
            pnlNDebito.Visible = false;
        }
        #endregion
        #endregion

        #region Nota de crédito
        protected void rblMonedaCredito_TextChanged(object sender, EventArgs e)
        {
            if (rblMonedaCredito.SelectedValue == tipoMoneda.Pesos.ToString() || rblMonedaCredito.SelectedValue == tipoMoneda.Dolar.ToString())
                txtImporteCredito.Enabled = true;
        }

        protected void btnCredito_Click(object sender, EventArgs e)
        {
            #region Variables
            decimal _nuevoSaldo = 0;
            bool flag = false;
            decimal _signoSaldo = 0;
            #endregion

            Decimal _txtMonto = Convert.ToDecimal(txtImporteCredito.Text);
            if (rblMonedaCredito.SelectedValue == tipoMoneda.Dolar.ToString())
                _txtMonto = Convert.ToDecimal(txtImporteCredito.Text) * cValorDolar.LoadActualValue();

            int index = 0;

            foreach (var item in lvCC.Items)
            {
                Label _idCuentaCorrienteUsuario = item.FindControl("lbIdCuentaCorrienteUsuario") as Label;
                Label _debito = item.FindControl("lbDebito") as Label;
                Label _saldo = item.FindControl("lbSaldo") as Label;
                cItemCCU ccu = cItemCCU.Load(_idCuentaCorrienteUsuario.Text);
                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));

                if (ccu.IdCuota != hfIdCuota.Value)
                {
                    if (index < 1)
                    {
                        #region Pago de cuota
                        if (ccu.IdEstado == (Int16)eEstadoItem.Cuota)
                        {
                            cCuota cuota = cCuota.Load(ccu.IdCuota);

                            List<cCuota> cuotasPendientes = cCuota.GetCuotasPendientes(hfIdEmpresa.Value, DateTime.Now);

                            if (cuotasPendientes.Count > 1)
                            {
                                if (flag == false)
                                {
                                    if (lastSaldo < 0)
                                        _signoSaldo = lastSaldo * -1;
                                    else
                                        _signoSaldo = lastSaldo;

                                    #region if (_txtMonto == _signoSaldo)
                                    if (_txtMonto == _signoSaldo)
                                    {
                                        foreach (cCuota c in cuotasPendientes)
                                        {
                                            c.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            c.Save();
                                            Pago(c.Id, c.Vencimiento1);

                                            //Actualizo saldo
                                            cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                                            fp.Saldo = fp.Saldo - fp.Monto;
                                            fp.Save();
                                        }
                                        foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(ccu.IdCuota))
                                        {
                                            iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                            iccu.Save();
                                        }

                                        cItemCCU ccuNew = new cItemCCU();
                                        ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                        ccuNew.Fecha = DateTime.Now;
                                        ccuNew.Concepto = txtConceptoCredito.Text;
                                        ccuNew.Debito = 0;
                                        ccuNew.Credito = _txtMonto;
                                        ccuNew.Saldo = _nuevoSaldo;
                                        ccuNew.IdCuota = cuota.Id;
                                        ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                        ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                                        int _idItemCCU = ccuNew.Save();

                                        hfIdItemCC.Value = _idItemCCU.ToString();
                                    }
                                    #endregion

                                    #region if (_txtMonto > _signoSaldo)
                                    if (_txtMonto > _signoSaldo)
                                    {
                                        decimal diferencia = _txtMonto - Convert.ToDecimal(_debito.Text);
                                        decimal resto = diferencia - (Convert.ToDecimal(_saldo.Text) - Convert.ToDecimal(_debito.Text));
                                        _nuevoSaldo = resto;

                                        foreach (cCuota c in cuotasPendientes)
                                        {
                                            c.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            c.Save();
                                            Pago(c.Id, c.Vencimiento1);

                                            //Actualizo saldo
                                            cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                                            fp.Saldo = fp.Saldo - fp.Monto;
                                            fp.Save();
                                        }
                                        ccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                        ccu.Save();

                                        cItemCCU ccuNew = new cItemCCU();
                                        ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                        ccuNew.Fecha = DateTime.Now;
                                        ccuNew.Concepto = txtConceptoCredito.Text;
                                        ccuNew.Debito = 0;
                                        ccuNew.Credito = _txtMonto;
                                        ccuNew.Saldo = _nuevoSaldo;
                                        ccuNew.IdCuota = cuota.Id;
                                        ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                        ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                                        int _idItemCCU = ccuNew.Save();

                                        hfIdItemCC.Value = _idItemCCU.ToString();

                                        //Genera el recibo del pago
                                        cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCU.ToString(), _txtMonto);

                                        cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente); //Historial de pagos
                                        cRegistroPago registro = new cRegistroPago(DateTime.Now, _txtMonto, "", "", hfIdEmpresa.Value, -1, (Int16)estadoCuenta_Cuota.Pagado, Convert.ToInt16(cc.Id), 1, 0);
                                    }
                                    #endregion

                                    #region if (_txtMonto < _signoSaldo)
                                    if (_txtMonto < _signoSaldo)
                                    {
                                        string _lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
                                        _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + _txtMonto;

                                        decimal suma = 0;
                                        foreach (cItemCCU i in cItemCCU.GetItemsByCuotas(cuota.Id))
                                        {
                                            suma += i.Credito;
                                        }

                                        suma = suma + _txtMonto;

                                        if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                        {
                                            string venc = null;
                                            if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                                venc = String.Format("{0:#,#.00}", cuota.Vencimiento1);
                                            else
                                                venc = String.Format("{0:#,#.00}", cuota.Vencimiento1 * cValorDolar.LoadActualValue());

                                            if (Convert.ToDecimal(String.Format("{0:#,#.00}", suma)) == Convert.ToDecimal(venc))
                                            {
                                                cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                                cuota.Save();
                                                Pago(cuota.Id, cuota.Vencimiento1);

                                                //Actualizo saldo
                                                cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                                fp.Saldo = fp.Saldo - fp.Monto;
                                                fp.Save();

                                                ccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccu.Save();
                                            }
                                        }

                                        if (cuota.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                        {
                                            string venc = null;
                                            if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                                venc = String.Format("{0:#,#0.00}", cuota.Vencimiento2);
                                            else
                                                venc = String.Format("{0:#,#0.00}", cuota.Vencimiento1 * cValorDolar.LoadActualValue());

                                            if (suma == Convert.ToDecimal(venc))
                                            {
                                                cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                                cuota.Save();
                                                Pago(cuota.Id, cuota.Vencimiento2);

                                                //Actualizo saldo
                                                cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                                fp.Saldo = fp.Saldo - fp.Monto;
                                                fp.Save();

                                                foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(ccu.IdCuota))
                                                {
                                                    iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                                    iccu.Save();
                                                }
                                            }
                                        }

                                        cItemCCU ccuNew = new cItemCCU();
                                        ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                        ccuNew.Fecha = DateTime.Now;
                                        ccuNew.Concepto = txtConceptoCredito.Text;
                                        ccuNew.Debito = 0;
                                        ccuNew.Credito = _txtMonto;
                                        ccuNew.Saldo = _nuevoSaldo;
                                        ccuNew.IdCuota = cuota.Id;
                                        ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                        ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                                        int _idItemCCU = ccuNew.Save();

                                        hfIdItemCC.Value = _idItemCCU.ToString();

                                        //Genera el recibo del pago
                                        cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCU.ToString(), _txtMonto);

                                        cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente); //Historial de pagos
                                        cRegistroPago registro = new cRegistroPago(DateTime.Now, _txtMonto, "", "", hfIdEmpresa.Value, -1, (Int16)estadoCuenta_Cuota.Pagado, Convert.ToInt16(cc.Id), 1, 0);
                                    }
                                    #endregion

                                    flag = true;
                                }
                            }
                            else
                            {
                                if (flag == false)
                                {
                                    string _lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
                                    _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + _txtMonto;

                                    #region if ((ccu.Saldo * -1) <= _txtMonto)
                                    if ((ccu.Saldo * -1) <= _txtMonto)
                                    {
                                        ccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                        ccu.Save();
                                    }
                                    else
                                    {
                                        decimal sum = 0;
                                        foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(ccu.IdCuota))
                                        {
                                            if (Convert.ToInt64(iccu.Id) < Convert.ToInt64(ccu.Id))
                                            {
                                                sum = sum + (iccu.Debito * -1);
                                            }
                                        }

                                        if ((ccu.Saldo * -1) <= (sum + _txtMonto))
                                        {
                                            ccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                            ccu.Save();
                                        }
                                    }
                                    #endregion

                                    decimal suma = 0;
                                    decimal _monto = 0;
                                    foreach (cItemCCU i in cItemCCU.GetItemsByCuotas(cuota.Id))
                                    {
                                        suma += i.Credito;
                                    }

                                    suma = suma + _txtMonto;

                                    #region if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                    if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                    {
                                        string venc = null;
                                        if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                            venc = String.Format("{0:#,#.00}", cuota.Vencimiento1);
                                        else
                                            venc = String.Format("{0:#,#.00}", cuota.Vencimiento1 * cValorDolar.LoadActualValue());

                                        if (Convert.ToDecimal(String.Format("{0:#,#.00}", suma)) == Convert.ToDecimal(venc))
                                        {
                                            cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            cuota.Save();
                                            Pago(cuota.Id, cuota.Vencimiento1);

                                            //Actualizo saldo
                                            cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                            fp.Saldo = fp.Saldo - fp.Monto;
                                            fp.Save();

                                            foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(ccu.IdCuota))
                                            {
                                                iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                                iccu.Save();
                                            }
                                        }
                                        else
                                        {
                                            if ((Convert.ToDecimal(_lastSaldo) * -1) <= _txtMonto)
                                            {
                                                cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                                cuota.Save();
                                                Pago(cuota.Id, cuota.Vencimiento1);

                                                //Actualizo saldo
                                                cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                                fp.Saldo = fp.Saldo - fp.Monto;
                                                fp.Save();

                                                foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(ccu.IdCuota))
                                                {
                                                    iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                                    iccu.Save();
                                                }
                                            }
                                        }

                                        _monto = cuota.Vencimiento1;
                                    }
                                    #endregion

                                    #region if (cuota.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                    if (cuota.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                    {
                                        string venc = null;
                                        if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos))
                                            venc = String.Format("{0:#,#.00}", cuota.Vencimiento2);
                                        else
                                            venc = String.Format("{0:#,#.00}", cuota.Vencimiento2 * cValorDolar.LoadActualValue());

                                        if (Convert.ToDecimal(String.Format("{0:#,#.00}", suma)) == Convert.ToDecimal(venc))
                                        {
                                            cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
                                            cuota.Save();
                                            Pago(cuota.Id, cuota.Vencimiento2);

                                            //Actualizo saldo
                                            cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
                                            fp.Saldo = fp.Saldo - fp.Monto;
                                            fp.Save();

                                            foreach (cItemCCU iccu in cItemCCU.GetItemsByCuotas(ccu.IdCuota))
                                            {
                                                iccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                                iccu.Save();
                                            }
                                        }

                                        _monto = cuota.Vencimiento2;
                                    }
                                    #endregion

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.Concepto = txtConceptoCredito.Text;
                                    ccuNew.Debito = 0;
                                    ccuNew.Credito = _txtMonto;
                                    ccuNew.Saldo = _nuevoSaldo;
                                    ccuNew.IdCuota = cuota.Id;
                                    ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                                    int _idItemCCU = ccuNew.Save();

                                    hfIdItemCC.Value = _idItemCCU.ToString();

                                    //Genera el recibo del pago
                                    cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCU.ToString(), _txtMonto);

                                    cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente); //Historial de pagos

                                    string _idRegistro = CrearRegistroPago(_monto, hfIdEmpresa.Value, cc.Id);
                                    cuota.IdRegistroPago = _idRegistro;
                                    cuota.Save();
                                }
                            }
                            index++;
                        }
                        #endregion

                        #region Pago de cuotas por adelanto
                        if (ccu.IdEstado == (Int16)eEstadoItem.Adelanto)
                        {
                            #region Total del adelanto
                            decimal _debitoAdelanto = 0;
                            List<cItemCCU> cuotasAdelanto = cItemCCU.GetItemByCuotasAdelanto(Request["idCC"].ToString(), (Int16)eEstadoItem.Adelanto);
                            foreach (cItemCCU i in cuotasAdelanto)
                            {
                                _debitoAdelanto = _debitoAdelanto + i.Debito;
                            }
                            _debitoAdelanto = _debitoAdelanto * -1;
                            #endregion

                            #region Variables
                            decimal totalCuotas = 0;
                            int cantCuotasAdelanto = 0;
                            #endregion

                            #region Actualiza estado de las cuotas
                            List<cCuota> cuotas = cCuota.GetCuotasActivasDESC(hfIdCC.Value, hfIdFormaPagoOV.Value);
                            foreach (cCuota c in cuotas)
                            {
                                if (totalCuotas == 0)
                                    totalCuotas = c.Monto;

                                if (totalCuotas >= _debitoAdelanto)
                                {
                                    cCuota _cuotaAdelantada = cCuota.Load(c.Id);
                                    _cuotaAdelantada.MontoAjustado = 0;
                                    _cuotaAdelantada.Saldo = 0;
                                    _cuotaAdelantada.Estado = (Int16)estadoCuenta_Cuota.Anticipo;
                                    _cuotaAdelantada.Save();

                                    PagoAdelanto(c.Id, c.Monto);

                                    //Actualizo saldo
                                    cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                                    fp.Saldo = fp.Saldo - fp.Monto;
                                    fp.Save();

                                    totalCuotas = totalCuotas + c.Monto;
                                    cantCuotasAdelanto++;
                                }
                            }
                            #endregion

                            _nuevoSaldo = Convert.ToDecimal(lastSaldo) + _txtMonto;

                            #region Carga del item
                            if (_debitoAdelanto == _txtMonto)
                            {
                                cItemCCU ccuNew = new cItemCCU();
                                ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                ccuNew.Fecha = DateTime.Now;
                                ccuNew.Concepto = txtConceptoCredito.Text;
                                ccuNew.Debito = 0;
                                ccuNew.Credito = _txtMonto;
                                ccuNew.Saldo = _nuevoSaldo;
                                ccuNew.IdCuota = "-1";
                                ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                                ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                                int _idItemCCU = ccuNew.Save();

                                //Actualizo itemCCU de la nota de débito
                                ccu.IdEstado = (Int16)eEstadoItem.Pagado;
                                ccu.Save();

                                //Genera el recibo del pago
                                cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCU.ToString(), _txtMonto);
                            }
                            else
                            {
                                decimal sum = 0;
                                List<cItemCCU> cuotasPagoParcialAdelanto = cItemCCU.GetItemByCuotasAdelanto(Request["idCC"].ToString(), (Int16)eEstadoItem.PagoParcialAdelanto);
                                foreach (cItemCCU c in cuotasPagoParcialAdelanto)
                                {
                                    sum = sum + c.Credito;
                                }

                                if (_debitoAdelanto == sum)
                                {
                                    foreach (cItemCCU c in cuotasPagoParcialAdelanto)
                                    {
                                        c.IdEstado = (Int16)eEstadoItem.Pagado;
                                        c.Save();
                                    }
                                }
                                else
                                {
                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                    ccuNew.Fecha = DateTime.Now;
                                    ccuNew.Concepto = txtConceptoCredito.Text;
                                    ccuNew.Debito = 0;
                                    ccuNew.Credito = _txtMonto;
                                    ccuNew.Saldo = _nuevoSaldo;
                                    ccuNew.IdCuota = "-1";
                                    ccuNew.IdEstado = (Int16)eEstadoItem.PagoParcialAdelanto;
                                    ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                                    int _idItemCCU = ccuNew.Save();

                                    //Genera el recibo del pago
                                    cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCU.ToString(), _txtMonto);
                                }
                            }
                            #endregion

                            #region Total pagos parciales del adelanto
                            List<cItemCCU> cuotasPagoParcialAdelanto1 = cItemCCU.GetItemByCuotasAdelanto(Request["idCC"].ToString(), (Int16)eEstadoItem.PagoParcialAdelanto);

                            decimal sumaCuotas = 0;
                            if (cuotasPagoParcialAdelanto1.Count != 0)
                            {
                                foreach (cItemCCU i in cuotasPagoParcialAdelanto1)
                                {
                                    sumaCuotas = sumaCuotas + i.Credito;
                                }
                            }
                            else
                                sumaCuotas = _txtMonto;
                            #endregion

                            #region Si queda resto lo agrego como crédito
                            if (sumaCuotas != _debitoAdelanto)
                            {
                                if (totalCuotas < _txtMonto)
                                {
                                    decimal diferencia = totalCuotas - _txtMonto;
                                    cItemCCU ccuResto = new cItemCCU();
                                    ccuResto.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                                    ccuResto.Fecha = DateTime.Now;
                                    ccuResto.Concepto = "Diferencia";
                                    ccuResto.Debito = 0;
                                    ccuResto.Credito = totalCuotas;
                                    ccuResto.Saldo = Convert.ToDecimal(lastSaldo) - diferencia;
                                    ccuResto.IdCuota = "-1";
                                    ccuResto.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuResto.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                                    int _idItemCCUResto = ccuResto.Save();

                                    //Genera el recibo del pago
                                    cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCUResto.ToString(), totalCuotas);
                                }
                            }
                            #endregion

                            #region Actualiza el item con el adelanto
                            if (sumaCuotas == _debitoAdelanto)
                            {
                                foreach (cItemCCU i in cuotasAdelanto)
                                {
                                    i.IdEstado = (Int16)eEstadoItem.Pagado;
                                    i.Save();
                                }

                                foreach (cItemCCU it in cuotasPagoParcialAdelanto1)
                                {
                                    it.IdEstado = (Int16)eEstadoItem.Pagado;
                                    it.Save();
                                }

                                decimal montoCuota = 0;
                                foreach (cCuota c in cuotas)
                                {
                                    if (montoCuota != _debitoAdelanto)
                                    {
                                        c.Estado = (Int16)estadoCuenta_Cuota.Anticipo;
                                        c.Save();
                                        montoCuota = montoCuota + c.Monto;
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion

                        if (ccu.IdEstado == (Int16)eEstadoItem.Pagado)
                        {
                            hfIdCuota.Value = "";
                        }
                        else
                            hfIdCuota.Value = ccu.IdCuota;
                    }
                }
            }

            bool flagAux = false;
            if (lvCC.Items.Count == 0 && hfIdCuota.Value == "")
            {
                if (!string.IsNullOrEmpty(txtImporteCredito.Text))
                {
                    decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));

                    cItemCCU ccuNew = new cItemCCU();
                    ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                    ccuNew.Fecha = DateTime.Now;
                    ccuNew.Concepto = txtConceptoCredito.Text;
                    ccuNew.Debito = 0;
                    ccuNew.Credito = _txtMonto;
                    if (lastSaldo > _txtMonto)
                        ccuNew.Saldo = lastSaldo + _txtMonto;
                    else
                        ccuNew.Saldo = _txtMonto + lastSaldo;
                    ccuNew.IdCuota = "-1";
                    ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                    ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                    int _idItemCCU = ccuNew.Save();

                    hfIdItemCC.Value = _idItemCCU.ToString();

                    //Genera el recibo del pago
                    cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCU.ToString(), _txtMonto);
                }

                flagAux = true;
            }

            if (flagAux == false)
            {
                if (hfIdCuota.Value == "")
                {
                    if (!string.IsNullOrEmpty(txtImporteCredito.Text))
                    {
                        decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));

                        cItemCCU ccuNew = new cItemCCU();
                        ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                        ccuNew.Fecha = DateTime.Now;
                        ccuNew.Concepto = txtConceptoCredito.Text;
                        ccuNew.Debito = 0;
                        ccuNew.Credito = _txtMonto;
                        if (lastSaldo > _txtMonto)
                            ccuNew.Saldo = lastSaldo + _txtMonto;
                        else
                            ccuNew.Saldo = _txtMonto + lastSaldo;
                        ccuNew.IdCuota = "-1";
                        ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                        ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                        int _idItemCCU = ccuNew.Save();

                        hfIdItemCC.Value = _idItemCCU.ToString();

                        //Genera el recibo del pago
                        cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCU.ToString(), _txtMonto);
                    }
                }

                if (lvCC.Items.Count == 0)
                {
                    if (!string.IsNullOrEmpty(txtImporteCredito.Text))
                    {
                        decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString()));

                        cItemCCU ccuNew = new cItemCCU();
                        ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                        ccuNew.Fecha = DateTime.Now;
                        ccuNew.Concepto = txtConceptoCredito.Text;
                        ccuNew.Debito = 0;
                        ccuNew.Credito = _txtMonto;
                        if (lastSaldo > _txtMonto)
                            ccuNew.Saldo = lastSaldo + _txtMonto;
                        else
                            ccuNew.Saldo = _txtMonto + lastSaldo;
                        ccuNew.IdCuota = "-1";
                        ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                        ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaCredito;
                        int _idItemCCU = ccuNew.Save();

                        hfIdItemCC.Value = _idItemCCU.ToString();

                        //Genera el recibo del pago
                        cNotaCredito nc = cNotaCredito.CrearNotaCredito("-1", _idItemCCU.ToString(), _txtMonto);
                    }
                }
            }

            lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
            lvCC.DataBind();

            hfIdCuota.Value = "";
            rblMonedaCredito.ClearSelection();
            txtImporteCredito.Text = "";
            txtImporteCredito.Enabled = false;
            txtConceptoCredito.Text = "";
            pnlOk.Visible = false;
            pnlCreditoOk.Visible = true;
            pnlOkNotaDebito.Visible = false;

            pnlPago.Visible = false;
            pnlCredito.Visible = false;
            pnlNDebito.Visible = false;
        }

        protected void lkbCredito_Click(object sender, EventArgs e)
        {
            cItemCCU itemCCU = cItemCCU.Load(hfIdItemCC.Value.ToString());
            ImprimirNotaCredito(itemCCU, hfIdEmpresa.Value);
        }
        #endregion

        #region Nota de débito
        protected void rblMonedaNotaDebito2_TextChanged(object sender, EventArgs e)
        {
            if (rblMonedaNotaDebito2.SelectedValue == tipoMoneda.Pesos.ToString() || rblMonedaNotaDebito2.SelectedValue == tipoMoneda.Dolar.ToString())
                txtImporteNotaDebito2.Enabled = true;
        }

        protected void btnNotaDebito_Click(object sender, EventArgs e)
        {
            try
            {
                decimal _nuevoSaldo = 0;
                string lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
                Decimal _saldo = Convert.ToDecimal(lastSaldo);

                Decimal _txtMonto = Convert.ToDecimal(txtImporteNotaDebito2.Text);
                if (rblMonedaNotaDebito2.SelectedValue == tipoMoneda.Dolar.ToString())
                    _txtMonto = Convert.ToDecimal(txtImporteNotaDebito2.Text) * cValorDolar.LoadActualValue() * -1;
                else
                    _txtMonto = _txtMonto * -1;

                _nuevoSaldo = _saldo + _txtMonto;

                cItemCCU ccuNew = new cItemCCU();
                ccuNew.IdCuentaCorrienteUsuario = Request["idCC"].ToString();
                ccuNew.Fecha = DateTime.Now;
                ccuNew.Concepto = txtConceptoNotaDebito2.Text;
                ccuNew.Debito = _txtMonto;
                ccuNew.Credito = 0;
                ccuNew.Saldo = _nuevoSaldo;
                ccuNew.IdCuota = "-1";
                ccuNew.IdEstado = (Int16)eEstadoItem.Pagado;
                ccuNew.TipoOperacion = (Int16)eTipoOperacion.NotaDebito;
                int _idItemCCU = ccuNew.Save();

                //Genera la nota de débito
                cNotaDebito nc = cNotaDebito.CrearNotaDebito("-1", _idItemCCU.ToString(), _txtMonto);

                lvCC.DataSource = cItemCCU.GetCuentaCorrienteLast10(Request["idCC"].ToString());
                lvCC.DataBind();

                hfIdItemCC.Value = _idItemCCU.ToString();

                rblMonedaNotaDebito2.ClearSelection();
                txtImporteNotaDebito2.Text = "";
                txtImporteNotaDebito2.Enabled = false;
                txtConceptoNotaDebito2.Text = "";
                pnlOk.Visible = false;
                pnlOkNotaDebito.Visible = true;

                pnlPago.Visible = false;
                pnlCredito.Visible = false;
                pnlNDebito.Visible = false;
            }
            catch (Exception ex)
            {
                log4net.Config.XmlConfigurator.Configure();
                log.Error("DetalleCC - " + DateTime.Now + "- " + ex.Message + " - btnNotaDebito_Click");
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void lkbNotaDebito_Click(object sender, EventArgs e)
        {
            cItemCCU itemCCU = cItemCCU.Load(hfIdItemCC.Value.ToString());
            ImprimirNotaDebito(itemCCU, hfIdEmpresa.Value);
        }
        #endregion
        #endregion

        #region Botones-Paneles
        protected void btnPagoCC_Click(object sender, EventArgs e)
        {
            pnlPago.Visible = true;
            CloseNotaCredito();
            CloseNotaDebito();

            CloseMessage();
        }

        protected void btnCreditoCC_Click(object sender, EventArgs e)
        {
            pnlPago.Visible = false;
            ClosePagoCuota();
            CloseOtrosPagos();
            CloseAdelantoCuotas();
            CloseCancelarSaldo();
            CloseAnular();
            CloseAnularReserva();
            CloseMessage();
            pnlCredito.Visible = true;
            CloseNotaDebito();

            CloseMessage();
        }

        protected void btnNotaDebitoCC_Click(object sender, EventArgs e)
        {
            pnlPago.Visible = false;
            ClosePagoCuota();
            CloseOtrosPagos();
            CloseAdelantoCuotas();
            CloseCancelarSaldo();
            CloseAnular();
            CloseAnularReserva();
            CloseMessage();
            CloseNotaCredito();
            pnlNDebito.Visible = true;

            CloseMessage();
        }
        #endregion

        #region ListView
        protected void lvCC_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string id = e.CommandArgument.ToString();
            cItemCCU itemCCU = cItemCCU.Load(e.CommandArgument.ToString());

            switch (e.CommandName)
            {
                case "Imprimir":
                    {
                        switch (itemCCU.TipoOperacion)
                        {
                            case (Int16)eTipoOperacion.PagoCuota:
                                if (itemCCU.GetRecibo != "-")
                                    ImprimirRecibo(itemCCU, hfIdEmpresa.Value);
                                break;
                            case (Int16)eTipoOperacion.NotaCredito:
                                if (itemCCU.GetNotaCredito != "-")
                                {
                                    ImprimirNotaCredito(itemCCU, hfIdEmpresa.Value);
                                }
                                break;
                            case (Int16)eTipoOperacion.NotaDebito:
                                if (itemCCU.GetNotaDebito != "-")
                                {
                                    ImprimirNotaDebito(itemCCU, hfIdEmpresa.Value);
                                }
                                break;
                            case (Int16)eTipoOperacion.OtrosPago:
                                if (itemCCU.GetRecibo != "-")
                                {
                                    ImprimirRecibo(itemCCU, hfIdEmpresa.Value);
                                }
                                break;
                            case (Int16)eTipoOperacion.Adelanto:
                                if (itemCCU.GetRecibo != "-")
                                {
                                    ImprimirRecibo(itemCCU, hfIdEmpresa.Value);
                                }
                                break;
                            case (Int16)eTipoOperacion.Saldo:
                                if (itemCCU.GetRecibo != "-")
                                {
                                    ImprimirRecibo(itemCCU, hfIdEmpresa.Value);
                                }
                                break;
                            case (Int16)eTipoOperacion.Anular:
                                if (itemCCU.GetRecibo != "-")
                                {
                                    ImprimirRecibo(itemCCU, hfIdEmpresa.Value);
                                }
                                break;
                            case (Int16)eTipoOperacion.Reserva:
                                if (itemCCU.GetRecibo != "-")
                                {
                                    ImprimirRecibo(itemCCU, hfIdEmpresa.Value);
                                }
                                break;
                            case (Int16)eTipoOperacion.Condonacion:
                                if (itemCCU.GetCondonacion != "-")
                                {
                                    ImprimirCondonacion(itemCCU, hfIdEmpresa.Value);
                                }
                                break;
                        }
                    } break;
            }
        }
        #endregion

        #region Auxiliares
        public void actualizarEstadoCuotas()
        {
            decimal newSaldo = 0;
            int contCuotasDolar = 0;
            List<cItemCCU> cuotasPendientes = cItemCCU.GetItemByCuotasPendientes(Request["idCC"].ToString());
            foreach (cItemCCU item in cuotasPendientes)
            {
                if (item.Fecha != DateTime.Now)
                {
                    cCuota cuota = cCuota.Load(item.IdCuota);
                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                    {
                        #region Actualizo el valor de la fila que contine la cuota pendiente
                        //Actualizo el saldo
                        //Saldo -: (-debito) || Saldo +: (-debito)
                        if (item.Saldo < 0)
                            item.Saldo = item.Saldo - item.Debito;
                        else
                            item.Saldo = item.Saldo + item.Debito;

                        if (contCuotasDolar == 0)
                            newSaldo += item.Saldo + (cuota.Vencimiento1 * cValorDolar.LoadActualValue() * -1);
                        else
                            newSaldo += item.Debito;

                        if (item.Debito != 0)
                            item.Debito = cuota.Vencimiento1 * cValorDolar.LoadActualValue() * -1;

                        contCuotasDolar++;
                        #endregion
                    }
                    else
                    {
                        DateTime now = Convert.ToDateTime(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day);
                        DateTime fechaVenc = cuota.FechaVencimiento1.AddDays(2);
                        fechaVenc = Convert.ToDateTime(fechaVenc.Year + "/" + fechaVenc.Month + "/" + fechaVenc.Day);
                        
                        if (fechaVenc < now)
                        {
                            int ads = cItemCCU.GetCantCuotasById(cuota.Id);
                            string lastSaldo = cItemCCU.GetLastSaldoByIdCCU(Request["idCC"].ToString());
                            if (cItemCCU.GetCantCuotasById(cuota.Id) < 2) //Para que solo una vez agregue la fila con el segundo vencimiento
                            {
                                decimal diferencia = (cuota.Vencimiento2 - cuota.Vencimiento1) * -1;
                                decimal newSaldo1 = Convert.ToDecimal(lastSaldo) + diferencia;
                                cItemCCU newItem = new cItemCCU(item.IdCuentaCorrienteUsuario, DateTime.Now, "Recargo por segundo vencimiento de la cuota " + cuota.Nro, diferencia, 0, newSaldo1, cuota.Id);
                            }
                        }
                    }
                }
            }
        }

        public void CloseMessage()
        {
            pnlOk.Visible = false;
            pnlCreditoOk.Visible = false;
            pnlOkNotaDebito.Visible = false;
            pnlAdelanto.Visible = false;
            pnlMensajePago.Visible = false;
            pnlMensajeReserva.Visible = false;
        }

        public void CloseNotaCredito()
        {
            pnlCredito.Visible = false;
            rblMonedaCredito.ClearSelection();
            txtImporteCredito.Text = "";
            txtImporteCredito.Enabled = false;
            txtConceptoCredito.Text = "";
        }

        public void CloseNotaDebito()
        {
            pnlNDebito.Visible = false;
            rblMonedaNotaDebito2.ClearSelection();
            txtImporteNotaDebito2.Text = "";
            txtImporteNotaDebito2.Enabled = false;
            txtConceptoNotaDebito2.Text = "";
        }

        public void ClosePagoCuota()
        {
            pnlPagoCuota.Visible = false;
            txtImportePago.Text = "";
            txtImportePago.Enabled = false;
            rblPago.ClearSelection();
            rblMonedaPago.ClearSelection();
            PagoCuotaMonto.Visible = false;
            txtConceptoPago.Text = "";
        }

        public void CloseOtrosPagos()
        {
            pnlOtrosPago.Visible = false;
            rblPago.ClearSelection();
            rblMonedaOtrosPago.ClearSelection();
            txtImporteOtrosPago.Text = "";
            txtImporteOtrosPago.Enabled = false;
            txtConceptoOtrosPago.Text = "";
        }

        public void CloseAdelantoCuotas()
        {
            pnlAdelantoCuota.Visible = false;
            cbProyectos.SelectedIndex = -1;
            cbFormaPago.SelectedIndex = -1;
            cbFormaPago.Enabled = false;
            pnlCantCuotas.Visible = false;
            txtCantCuotas.Text = "";
            pnlListadoCuotas.Visible = false;
            pnlListViewCuotas.Visible = false;
            pnlConceptoAdelanto.Visible = false;
            txtConceptoAdelantoCuota.Text = "";
            rblPago.ClearSelection();
        }

        public void CloseCancelarSaldo()
        {
            pnlCancelarSaldo.Visible = false;
            cbProyectosCancelarSaldo.SelectedIndex = -1;
            lbCancelarSaldo.Text = "-";
            pnlConceptoCancelarSaldo.Visible = false;
            txtConceptoCancelarSaldo.Text = "";
            rblPago.ClearSelection();
        }

        public void CloseAnular()
        {
            pnlAnular.Visible = false;
            pnlConceptoAnular.Visible = false;
            txtConceptoAnular.Text = "-";
            rblPago.ClearSelection();
        }

        public void CloseAnularReserva()
        {
            pnlAnularReserva.Visible = false;
            hfIdItemCCU.Value = "";
            pnlOneReserva.Visible = false;
            hfIdUnidad.Value = "";
            lbCodUF.Text = "-";
            lbNivelReserva.Text = "-";
            lbUnidadReserva.Text = "-";
            pnlReservas.Visible = false;
            pnlConceptoAnularReserva.Visible = false;
            txtConceptoAnularReserva.Text = "-";
            rblPago.ClearSelection();
        }

        public void CloseCondonacion()
        {
            pnlCondonacion.Visible = false;
            txtImporteCondonacion.Text = "";
            txtImporteCondonacion.Enabled = false;
            rblPago.ClearSelection();
            rblMonedaCondonacion.ClearSelection();
            CondonacionCuotaMonto.Visible = false;
            txtImporteCondonacion.Text = "";
        }

        protected void Pago(string _idCuota, decimal _txtMonto)
        {
            cCuota cuota = cCuota.Load(_idCuota);
            cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
            cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);

            #region Variables
            decimal _saldo = 0;
            int _cantCuotas = 0;
            #endregion

            _saldo = cuota.MontoAjustado - cuota.Monto;
            cuota.Saldo = _saldo;
            cuota.Save();
            _cantCuotas = cc.CantCuotas - cCuota.GetCantCuotasPagas(cc.Id);

            string _idRegistro = CrearRegistroPago(_txtMonto, hfIdEmpresa.Value, cc.Id);
            cuota.IdRegistroPago = _idRegistro;
            cuota.Save();

            //cSendMail send = new cSendMail();
            //send.EnviarAvisoPago(cEmpresa.Load(cc.IdEmpresa), cuota);
        }

        protected void PagoAdelanto(string _idCuota, decimal _txtMonto)
        {
            cCuota cuota = cCuota.Load(_idCuota);
            cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);

            string _idRegistro = CrearRegistroPago(_txtMonto, hfIdEmpresa.Value, cc.Id);
            cuota.IdRegistroPago = _idRegistro;
            cuota.Save();

            //cSendMail send = new cSendMail();
            //send.EnviarAvisoPago(cEmpresa.Load(cc.IdEmpresa), cuota);
        }

        public string CrearRegistroPago(decimal _txtMonto, string _idEmpresa, string _idCC)
        {
            cRegistroPago registro = new cRegistroPago();
            registro.FechaPago = DateTime.Now;
            registro.Monto = _txtMonto;
            registro.Sucursal = "";
            registro.Transaccion = "";
            registro.IdEmpresa = hfIdEmpresa.Value;
            registro.IdImagen = -1;
            registro.IdEstado = (Int16)estadoCuenta_Cuota.Pagado;
            registro.IdCC = Convert.ToInt16(_idCC);
            registro.Nro = 1;
            registro.FormaPago = 0;
            int _idRegistro = registro.Save();
            return _idRegistro.ToString();
        }

        #region Imprimir
        #region Recibo
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

        protected void ImprimirRecibo(cItemCCU _itemCCU, string _idEmpresa)
        {
            CrearPdfRecibo(_itemCCU, _idEmpresa);
        }
        #endregion

        #region Nota de crédito
        protected void CrearPdfNotaCredito(cItemCCU _itemCCU, string _idEmpresa)
        {
            cNotaCredito notaCredito;
            if (string.IsNullOrEmpty(cNotaCredito.GetNotaCreditoByIdItemCCU(_itemCCU.Id)))
                notaCredito = cNotaCredito.CrearNotaCredito("-1", _itemCCU.Id, _itemCCU.Credito);
            else
                notaCredito = cNotaCredito.GetNotaCreditoByNro(_itemCCU.GetNotaCredito);

            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Comprobantes\\Nota de credito\\";
            string filename = "Nota de credito " + notaCredito.Nro + ".pdf";

            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("fecha", String.Format("{0:dd/MM/yyyy}", notaCredito.Fecha));
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("recibo", _itemCCU.GetNotaCredito);
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("fechaImpresion", String.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("cliente", cEmpresa.Load(_idEmpresa).GetNombreCompleto);
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("monto", String.Format("{0:#,#0.00}", _itemCCU.Credito) + ".-");
            CrystalReportSourceNotaCredito.ReportDocument.SetParameterValue("montoletras", cAuxiliar.enLetras(_itemCCU.Credito.ToString()) + ".-");

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
            if (_itemCCU.Credito != 0)
                CrearPdfNotaCredito(_itemCCU, _idEmpresa);
        }
        #endregion

        #region Nota de débito
        protected void CrearPdfNotaDebito(cItemCCU _itemCCU, string _idEmpresa)
        {
            cNotaDebito notaDebito;
            if (string.IsNullOrEmpty(cReciboCuota.GetNroReciboByIdItemCCU(_itemCCU.Id)))
                notaDebito = cNotaDebito.CrearNotaDebito("-1", _itemCCU.Id, _itemCCU.Credito + _itemCCU.Debito);
            else
                notaDebito = cNotaDebito.GetNotaDebitoByNro(_itemCCU.GetRecibo);

            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Comprobantes\\Nota de debito\\";
            string filename = "Nota de debito " + notaDebito.Nro + ".pdf";

            decimal _monto = _itemCCU.Debito * -1;

            CrystalReportSourceNotaDebito.ReportDocument.SetParameterValue("fecha", String.Format("{0:dd/MM/yyyy}", notaDebito.Fecha));
            CrystalReportSourceNotaDebito.ReportDocument.SetParameterValue("recibo", _itemCCU.GetRecibo);
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
            if (_itemCCU.Debito != 0)
                CrearPdfNotaDebito(_itemCCU, _idEmpresa);
        }
        #endregion

        #region Condonación
        protected void CrearPdfCondonacion(cItemCCU _itemCCU, string _idEmpresa)
        {
            cCondonacion condonacion;
            if (string.IsNullOrEmpty(cCondonacion.GetNroCondonacionByIdItemCCU(_itemCCU.Id)))
                condonacion = cCondonacion.CrearCondonacion("-1", _itemCCU.Id, _itemCCU.Credito + _itemCCU.Debito);
            else
                condonacion = cCondonacion.GetCondonacionByNro(_itemCCU.GetCondonacion);

            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Comprobantes\\Recibos\\";
            string filename = "Condonacion_" + condonacion.Nro + ".pdf";

            CrystalReportSourceCondonacion.ReportDocument.SetParameterValue("fecha", String.Format("{0:dd/MM/yyyy}", condonacion.Fecha));
            CrystalReportSourceCondonacion.ReportDocument.SetParameterValue("recibo", _itemCCU.GetRecibo);
            CrystalReportSourceCondonacion.ReportDocument.SetParameterValue("cliente", cEmpresa.Load(_idEmpresa).GetNombreCompleto);

            CrystalReportSourceCondonacion.ReportDocument.SetParameterValue("monto", String.Format("{0:#,#0.00}", condonacion.Monto) + ".-");
            CrystalReportSourceCondonacion.ReportDocument.SetParameterValue("montoLetras", cAuxiliar.enLetras(condonacion.Monto.ToString()) + ".-");

            CrystalReportSourceCondonacion.ReportDocument.SetParameterValue("concepto", _itemCCU.Concepto);
            CrystalReportSourceCondonacion.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);
            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }

        protected void ImprimirCondonacion(cItemCCU _itemCCU, string _idEmpresa)
        {
            if (_itemCCU.Credito != 0)
                CrearPdfCondonacion(_itemCCU, _idEmpresa);

            if (_itemCCU.Debito != 0 && _itemCCU.TipoOperacion == (Int16)eTipoOperacion.Reserva)
                CrearPdfCondonacion(_itemCCU, _idEmpresa);
        }
        #endregion
        #endregion

        #region Filtro
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lvCC.DataSource = cItemCCU.Search(Request["idCC"].ToString(), txtFechaDesde.Text, txtFechaHasta.Text);
            lvCC.DataBind();
        }

        protected void btnVerTodos_Click(object sender, EventArgs e)
        {
            lvCC.DataSource = cItemCCU.Search(Request["idCC"].ToString(), null, null);
            lvCC.DataBind();
        }
        #endregion

        #region Imprimir
        private DataSet CrearDataSet()
        {
            List<cItemCCU> items = cItemCCU.GetCuentaCorriente(Request["idCC"].ToString());

            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("GetFecha"));
            dt.Columns.Add(new DataColumn("concepto"));
            dt.Columns.Add(new DataColumn("debito"));
            dt.Columns.Add(new DataColumn("credito"));
            dt.Columns.Add(new DataColumn("saldo"));
            dt.Columns.Add(new DataColumn("GetRecibo"));

            foreach (cItemCCU p in items)
            {
                dr = dt.NewRow();
                dr["GetFecha"] = p.GetFecha;
                dr["concepto"] = p.Concepto;
                dr["debito"] = p.Debito;
                dr["credito"] = p.Credito;
                dr["saldo"] = p.Saldo;
                dr["GetRecibo"] = p.GetRecibo;
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tItemCCU";

            return ds;
        }

        protected void btnImprimirCC_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Cuentas Corrientes\\";
            string filename = "Cuenta Corriente - " + Request["idCC"].ToString() + ".pdf";

            //Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataSet(), false, System.Data.MissingSchemaAction.Ignore);
            CrystalReportSource.ReportDocument.SetDataSource(ds);

            CrystalReportSource.ReportDocument.SetParameterValue("fecha", String.Format("{0:dd/MM/yyyy}", DateTime.Today));
            CrystalReportSource.ReportDocument.SetParameterValue("cliente", lblCliente.Text);

            CrystalReportSource.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);

            Response.ContentType = "APPLICATION/OCTET-STREAM";
            Response.AddHeader("Content-Disposition", "Attachment; Filename=" + filename);

            FileInfo fileToDownload = new System.IO.FileInfo(rutaURL + filename);
            Response.Flush();
            Response.WriteFile(fileToDownload.FullName);
            Response.End();
        }
        #endregion
        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            CargarCuotas(Convert.ToDateTime(TextBoxFecha.Text));
        }
    }
}