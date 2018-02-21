using DLL.Negocio;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class Configuracion : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lvValorDolar.DataSource = cValorDolar.GetValoresDolar();
                    lvValorDolar.DataBind();

                    lvCAC.DataSource = cIndiceCAC.GetIndiceCAC();
                    lvCAC.DataBind();

                    lvUVA.DataSource = cUVA.GetIndiceUVA();
                    lvUVA.DataBind();

                    DateTime fecha = DateTime.Now.AddMonths(-1);
                    
                    decimal idLastIndice = cIndiceCAC.GetLastIndice();
                    DateTime dt = cIndiceCAC.Load(idLastIndice.ToString()).Fecha;
                    if (DateTime.Today.AddMonths(-1).Month == dt.Month)
                        pnlbtnModificar.Visible = false;
                    else
                        pnlbtnModificar.Visible = true;


                    decimal idLastIndiceUVA = cUVA.GetLastIdIndice();
                    DateTime dtUVA = cUVA.Load(idLastIndiceUVA.ToString()).Fecha;
                    if (DateTime.Today.Month == dtUVA.Month)
                        pnlbtnModificarUVA.Visible = false;
                    else
                        pnlbtnModificarUVA.Visible = true;
                }
            }
            catch
            {
                Response.Redirect("MensajeError.aspx");
            }
        }

        #region Índice CAC
        protected void ListPager_PreRender(object sender, EventArgs e)
        {
            try
            {
                lvCAC.DataSource = cIndiceCAC.GetIndiceCAC();
                lvCAC.DataBind();
            }
            catch
            {
                Response.Redirect("MensajeError.aspx");
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Now.AddMonths(-1);

            lbFechaIndice.Text = String.Format("{0:MMMM yyyy}", fecha);
            mpeIndice.Show();
        }

        protected void btnModificarCAC_Click(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Now.AddMonths(-2);

            lbFechaIndiceM.Text = String.Format("{0:MMMM yyyy}", fecha);
            mpeIndiceM.Show();
        }

        protected void btnFinalizarIndice_Click(object sender, EventArgs e)
        {
            decimal indice1 = Convert.ToDecimal(txtIndiceCAC.Text);
            decimal indice2 = Convert.ToDecimal(txtConfirmIndiceCAC.Text);

            if (indice1 != indice2)
            {
                pnlMensajeIndiceCAC.Visible = true;
                lbMensaje.Text = "Los índices ingresados no coinciden.";
            }
            else
            {
                cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
                DateTime fecha = Convert.ToDateTime("15" + lbFechaIndice.Text);
                //DateTime fecha = Convert.ToDateTime("15" + " diciembre 2017");

                if (lastIndice.Fecha < fecha)
                {
                    cIndiceCAC indice = new cIndiceCAC();
                    string indice_anterior = lastIndice.Id;

                    if (!string.IsNullOrEmpty(txtIndiceCAC.Text))
                    {
                        decimal _indice = Convert.ToDecimal(txtIndiceCAC.Text);
                        if (_indice != 0)
                        {
                            indice.Valor = _indice;

                            indice.Fecha = fecha;

                            Int32 id = indice.Save();

                            cIndiceCAC.ActualizarIndiceCACCuotas(id.ToString(), fecha.AddMonths(1), indice_anterior);

                            lvCAC.EditIndex = -1;
                            lvCAC.DataSource = cIndiceCAC.GetIndiceCAC();
                            lvCAC.DataBind();

                            mpeIndice.Hide();

                            pnlMensajeIndiceCAC.Visible = false;

                            ActualizarCuentasCorrientes();

                            Response.Redirect("Configuracion.aspx");
                        }
                        else
                        {
                            pnlMensajeIndiceCAC.Visible = true;
                            lbMensaje.Text = "El índice debe ser superior a 0.";
                        }
                    }
                }
                else
                {
                    pnlMensajeIndiceCAC.Visible = true;
                    lbMensaje.Text = "El índice para este mes ya fue ingresado.";
                }
            }
        }

        protected void btnFinalizarIndiceM_Click(object sender, EventArgs e)
        {
            decimal indice1 = Convert.ToDecimal(txtIndiceCACM.Text);
            decimal indice2 = Convert.ToDecimal(txtConfirmIndiceCACM.Text);

            if (indice1 != indice2)
            {
                pnlMensajeIndiceCACM.Visible = true;
                lbMensajeM.Text = "Los índices ingresados no coinciden.";
            }
            else
            {
                cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
                DateTime fecha = Convert.ToDateTime("15" + lbFechaIndiceM.Text);
                //DateTime fecha = Convert.ToDateTime("15" + " diciembre 2017");

                if (lastIndice.Fecha < fecha)
                {
                    cIndiceCAC indice = new cIndiceCAC();
                    string indice_anterior = lastIndice.Id;

                    if (!string.IsNullOrEmpty(txtIndiceCACM.Text))
                    {
                        decimal _indice = Convert.ToDecimal(txtIndiceCACM.Text);
                        if (_indice != 0)
                        {
                            indice.Valor = _indice;

                            indice.Fecha = fecha;

                            Int32 id = indice.Save();

                            cIndiceCAC.ActualizarIndiceCACCuotas(id.ToString(), fecha.AddMonths(1), indice_anterior);

                            lvCAC.EditIndex = -1;
                            lvCAC.DataSource = cIndiceCAC.GetIndiceCAC();
                            lvCAC.DataBind();

                            mpeIndice.Hide();

                            pnlMensajeIndiceCAC.Visible = false;

                            ActualizarCuentasCorrientesRecargaCAC();

                            Response.Redirect("Configuracion.aspx");
                        }
                        else
                        {
                            pnlMensajeIndiceCACM.Visible = true;
                            lbMensajeM.Text = "El índice debe ser superior a 0.";
                        }
                    }
                }
                else
                {
                    pnlBotonesM.Visible = false;
                    pnlMensajeIndiceCACM.Visible = true;
                    lbMensajeM.Text = "El índice para este mes ya fue ingresado. ¿Desea volver a cargar el índice?";
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            mpeIndice.Hide();
        }

        protected void btnCancelarM_Click(object sender, EventArgs e)
        {
            mpeIndiceM.Hide();
        }

        protected void btnIndiceSiM_Click(object sender, EventArgs e)
        {
            decimal indice1 = Convert.ToDecimal(txtIndiceCACM.Text);
            decimal indice2 = Convert.ToDecimal(txtConfirmIndiceCACM.Text);

            if (indice1 != indice2)
            {
                pnlMensajeIndiceCACM.Visible = true;
                lbMensajeM.Text = "Los índices ingresados no coinciden.";
            }
            else
            {
                cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
                DateTime fecha = Convert.ToDateTime("15" + lbFechaIndiceM.Text);
                //DateTime fecha = Convert.ToDateTime("15" + " diciembre 2017");

                if (lastIndice.Fecha.Month == fecha.Month)
                    lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndiceMonth().ToString());

                cIndiceCAC indice = new cIndiceCAC();
                string indice_anterior = lastIndice.Id;

                if (!string.IsNullOrEmpty(txtIndiceCACM.Text))
                {
                    decimal _indice = Convert.ToDecimal(txtIndiceCACM.Text);
                    if (_indice != 0)
                    {
                        indice.Valor = _indice;

                        indice.Fecha = fecha;

                        Int32 id = indice.Save();

                        cIndiceCAC.ActualizarIndiceRecargaCACCuotas(id.ToString(), fecha.AddMonths(1), indice_anterior);

                        lvCAC.EditIndex = -1;
                        lvCAC.DataSource = cIndiceCAC.GetIndiceCAC();
                        lvCAC.DataBind();

                        mpeIndice.Hide();

                        pnlMensajeIndiceCACM.Visible = false;

                        ActualizarCuentasCorrientesRecargaCAC();

                        Response.Redirect("Configuracion.aspx");
                    }
                    else
                    {
                        pnlMensajeIndiceCACM.Visible = true;
                        lbMensajeM.Text = "El índice debe ser superior a 0.";
                    }
                }
            }
        }
        #endregion

        #region Actualizar cuentas corrientes
        public void ActualizarCuentasCorrientes()
        {
            List<cCuentaCorrienteUsuario> cuentas = cCuentaCorrienteUsuario.GetCuentaCorriente();
            cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());

            foreach (cCuentaCorrienteUsuario c in cuentas)
            {
                CargarCuotas(DateTime.Now, c.Id, c.IdEmpresa);
                actualizarEstadoCuotas(c.Id);

                if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
                    CargarCuotas(DateTime.Now.AddMonths(1), c.Id, c.IdEmpresa);
            }
        }

        public void ActualizarCuentasCorrientesRecargaCAC()
        {
            List<cCuentaCorrienteUsuario> cuentas = cCuentaCorrienteUsuario.GetCuentaCorriente();
            cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());

            foreach (cCuentaCorrienteUsuario c in cuentas)
            {
                CargarCuotasErrorCAC(DateTime.Now, c.Id, c.IdEmpresa);
                actualizarEstadoCuotas(c.Id);
            }
        }

        public void CargarCuotasErrorCAC(DateTime _fecha, string _idCCU, string _idEmpresa)
        {
            string _idCC = null;
            string _idFormaPagoOv = null;


            decimal idCac = cIndiceCAC.GetLastIndice();
            cIndiceCAC _indice = cIndiceCAC.Load(idCac.ToString());
            
            DateTime _lastCac = _indice.Fecha;

            List<cCuota> cuotas2 = cCuota.GetCuotas(_idEmpresa, _lastCac, (Int16)eIndice.CAC);
            decimal _saldo = 0;
            decimal valorCuotaVencimiento1 = 0;
            decimal valorCuotaVencimiento2 = 0;

            #region Carga de cuotas del mes correspondiente
            if (cuotas2 != null && cuotas2.Count != 0)
            {
                foreach (cCuota cuota in cuotas2)
                {
                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos) && (cuota.VariacionCAC != 0 || cuota.VariacionUVA != 0))
                    {
                        //if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        //{
                            decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(_idCCU));
                            cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                            string idCCU = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(cc.IdEmpresa);
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

                            cItemCCU existingItem = cItemCCU.GetItemCCUByIdCuota(cuota.Id);
                            if (existingItem != null)
                            {
                                existingItem.IdEstado = (Int16)eEstadoItem.Pagado;
                                existingItem.Save();

                                cItemCCU ccuNew1 = new cItemCCU();
                                ccuNew1.IdCuentaCorrienteUsuario = existingItem.IdCuentaCorrienteUsuario;
                                ccuNew1.Fecha = DateTime.Now;
                                ccuNew1.Concepto = "Anulación de item por CAC incorrecto";
                                ccuNew1.Debito = 0;
                                ccuNew1.Credito = (existingItem.Debito) * -1;

                                string _lastSaldo = cItemCCU.GetLastSaldoByIdCCU(existingItem.IdCuentaCorrienteUsuario);
                                decimal _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + ((existingItem.Debito) * -1);

                                ccuNew1.Saldo = _nuevoSaldo;
                                ccuNew1.IdCuota = cuota.Id;
                                ccuNew1.IdEstado = (Int16)eEstadoItem.Pagado;
                                ccuNew1.TipoOperacion = (Int16)eTipoOperacion.Cuota;
                                int _idItemCCU = ccuNew1.Save();

                                lastSaldo = _nuevoSaldo;
                            }

                            cItemCCU ccuNew = new cItemCCU();
                            ccuNew.IdCuentaCorrienteUsuario = _idCCU;
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
                                cItemCCU item = cItemCCU.GetLastItemById(_idCCU);
                                if (item != null && item.IdEstado == (Int16)eEstadoItem.Pagado)
                                {
                                    if (lastSaldo > valorCuotaVencimiento1)
                                    {
                                        decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                        cItemCCU ccuPago = new cItemCCU();
                                        ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                        ccuPago.Fecha = DateTime.Now;
                                        ccuPago.Concepto = "Pago cuota " + cuota.Nro + " por saldo a favor"; ;
                                        ccuPago.Debito = 0;
                                        ccuPago.Credito = 0;
                                        ccuPago.Saldo = diferencia;
                                        ccuPago.IdCuota = cuota.Id;
                                        ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                        ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                        int _idItemCCU = ccuPago.Save();

                                        //Genera el recibo del pago
                                        //cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), cuota.Vencimiento1);
                                        cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(idCCU);
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
                                        ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                        ccuPago.Fecha = DateTime.Now;
                                        ccuPago.Concepto = "Pago parcial cuota " + cuota.Nro + " por saldo a favor";
                                        ccuPago.Debito = 0;
                                        ccuPago.Credito = 0;
                                        ccuPago.Saldo = diferencia;
                                        ccuPago.IdCuota = cuota.Id;
                                        ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                        ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                        int _idItemCCU = ccuPago.Save();
                                        
                                        //Genera el recibo del pago
                                        cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(_idCCU);
                                        cReciboCuota recibo = new cReciboCuota(cuota.Id, _idItemCCU.ToString(), lastNroRecibo.Nro, DateTime.Now);
                                        recibo.Monto = lastNroRecibo.Monto;
                                        recibo._Papelera = 1;
                                        recibo.Save();
                                    }
                                }
                            }
                            #endregion
                        //}
                    }
                    else
                    {
                        if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        {
                            if (cItemCCU.GetCantCuotasById(cuota.Id) == 0)
                            {
                                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(_idCCU));
                                cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                                string idCCU = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(cc.IdEmpresa);
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
                                    ccuNew.IdCuentaCorrienteUsuario = _idCCU;
                                    ccuNew.Concepto = "Cuota nro " + cuota.Nro;
                                    ccuNew.Debito = valorCuotaVencimiento1 * -1;

                                    _saldo += lastSaldo;
                                    if (_saldo < 0)
                                        ccuNew.Saldo = (valorCuotaVencimiento1 * -1) + (_saldo);
                                    else
                                        ccuNew.Saldo = _saldo - valorCuotaVencimiento1;

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
                                        cItemCCU item = cItemCCU.GetLastItemById(_idCCU);
                                        if (item != null && item.IdEstado == (Int16)eEstadoItem.Pagado)
                                        {
                                            if (lastSaldo > valorCuotaVencimiento1)
                                            {
                                                decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                                cItemCCU ccuPago = new cItemCCU();
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago cuota " + cuota.Nro + " por saldo a favor";
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = cuota.Vencimiento1;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //Genera el recibo del pago
                                                //cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), cuota.Vencimiento1);
                                                cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(idCCU);
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
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago parcial cuota " + cuota.Nro + " por saldo a favor";
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = 0;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //Genera el recibo del pago
                                                cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(_idCCU);
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
                }
            }
            #endregion
        }

        public void ActualizarCuentasCorrientesUVA()
        {
            List<cCuentaCorrienteUsuario> cuentas = cCuentaCorrienteUsuario.GetCuentaCorriente();

            foreach (cCuentaCorrienteUsuario c in cuentas)
            {
                CargarCuotas(DateTime.Now, c.Id, c.IdEmpresa);
                actualizarEstadoCuotas(c.Id);
            }
        }

        public void CargarCuotas(DateTime _fecha, string _idCCU, string _idEmpresa)
        {
            string _idCC = null;
            string _idFormaPagoOv = null;

            List<cCuota> cuotas2 = cCuota.GetCuotasPendientes(_idEmpresa, _fecha, (Int16)eIndice.CAC);
            decimal _saldo = 0;
            decimal valorCuotaVencimiento1 = 0;
            decimal valorCuotaVencimiento2 = 0;

            #region Carga de cuotas del mes correspondiente
            if (cuotas2 != null && cuotas2.Count != 0)
            {
                foreach (cCuota cuota in cuotas2)
                {
                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos) && (cuota.VariacionCAC != 0 || cuota.VariacionUVA != 0))
                    {
                        if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        {
                            if (cItemCCU.GetCantCuotasById(cuota.Id) == 0)
                            {
                                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(_idCCU));
                                cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                                string idCCU = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(cc.IdEmpresa);
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

                                    cItemCCU existingItem = cItemCCU.GetItemCCUByIdCuota(cuota.Id);
                                    if(existingItem != null)
                                    {
                                        existingItem.IdEstado = (Int16)eEstadoItem.Pagado;
                                        cCondonacion recibo = cCondonacion.CrearCondonacion(cuota.Id, existingItem.Id, (existingItem.Debito) * -1);
                                    }

                                    cItemCCU ccuNew = new cItemCCU();
                                    ccuNew.IdCuentaCorrienteUsuario = _idCCU;
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
                                        cItemCCU item = cItemCCU.GetLastItemById(_idCCU);
                                        if (item != null && item.IdEstado == (Int16)eEstadoItem.Pagado)
                                        {
                                            if (lastSaldo > valorCuotaVencimiento1)
                                            {
                                                decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                                cItemCCU ccuPago = new cItemCCU();
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago cuota " + cuota.Nro + " por saldo a favor"; ;
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = 0;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //hfIdItemCC.Value = _idItemCCU.ToString();

                                                //Genera el recibo del pago
                                                //cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), cuota.Vencimiento1);
                                                cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(idCCU);
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
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago parcial cuota " + cuota.Nro + " por saldo a favor";
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = 0;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //hfIdItemCC.Value = _idItemCCU.ToString();

                                                //Genera el recibo del pago
                                                cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(_idCCU);
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
                    else
                    {
                        if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        {
                            if (cItemCCU.GetCantCuotasById(cuota.Id) == 0)
                            {
                                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(_idCCU));
                                cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                                string idCCU = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(cc.IdEmpresa);
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
                                    ccuNew.IdCuentaCorrienteUsuario = _idCCU;
                                    ccuNew.Concepto = "Cuota nro " + cuota.Nro;
                                    ccuNew.Debito = valorCuotaVencimiento1 * -1;

                                    _saldo += lastSaldo;
                                    if (_saldo < 0)
                                        ccuNew.Saldo = (valorCuotaVencimiento1 * -1) + (_saldo);
                                    else
                                        ccuNew.Saldo = _saldo - valorCuotaVencimiento1;

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
                                        cItemCCU item = cItemCCU.GetLastItemById(_idCCU);
                                        if (item != null && item.IdEstado == (Int16)eEstadoItem.Pagado)
                                        {
                                            if (lastSaldo > valorCuotaVencimiento1)
                                            {
                                                decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                                cItemCCU ccuPago = new cItemCCU();
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago cuota " + cuota.Nro + " por saldo a favor";
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = cuota.Vencimiento1;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //hfIdItemCC.Value = _idItemCCU.ToString();

                                                //Genera el recibo del pago
                                                //cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), cuota.Vencimiento1);
                                                cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(idCCU);
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
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago parcial cuota " + cuota.Nro + " por saldo a favor";
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = 0;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //hfIdItemCC.Value = _idItemCCU.ToString();

                                                //Genera el recibo del pago
                                                cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(_idCCU);
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
                }
            }
            #endregion
        }

        public void actualizarEstadoCuotas(string _idCCU)
        {
            decimal newSaldo = 0;
            int contCuotasDolar = 0;
            List<cItemCCU> cuotasPendientes = cItemCCU.GetItemByCuotasPendientes(_idCCU);
            if(cuotasPendientes != null)
            {
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
                                string lastSaldo = cItemCCU.GetLastSaldoByIdCCU(_idCCU);
                                if (cItemCCU.GetCantCuotasById(cuota.Id) < 2) //Para que solo una vez agregue la fila con el segundo vencimiento
                                {
                                    cuota.Estado = (Int16)estadoCuenta_Cuota.Pendiente;
                                    cuota.Save();

                                    decimal diferencia = (cuota.Vencimiento2 - cuota.Vencimiento1) * -1;
                                    decimal newSaldo1 = Convert.ToDecimal(lastSaldo) + diferencia;
                                    cItemCCU newItem = new cItemCCU(item.IdCuentaCorrienteUsuario, DateTime.Now, "Recargo por segundo vencimiento de la cuota " + cuota.Nro, diferencia, 0, newSaldo1, cuota.Id);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Índice UVA
        protected void btnIngresarUVA_Click(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Now;

            lbFechaIndiceUVA.Text = String.Format("{0:MMMM yyyy}", fecha);
            ModalPopupExtender1.Show();
        }

        protected void btnFinalizarIndiceUVA_Click(object sender, EventArgs e)
        {
            decimal indice1 = Convert.ToDecimal(txtIndiceUVA.Text);
            decimal indice2 = Convert.ToDecimal(txtConfirmIndiceUVA.Text);

            if (indice1 != indice2)
            {
                //pnlMensajeIndiceUVA.Visible = true;
                //lbMensajeUVA.Text = "Los índices ingresados no coinciden.";
            }
            else
            {
                cUVA lastIndice = cUVA.Load(cUVA.GetLastIdIndice().ToString());
                //cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
                DateTime fecha = Convert.ToDateTime("25" + lbFechaIndiceUVA.Text);

                //DateTime fecha = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + "25");
                //DateTime fecha = Convert.ToDateTime("15" + " mayo 2017");

                if (lastIndice.Fecha < fecha)
                {
                    cUVA indice = new cUVA();
                    string indice_anterior = lastIndice.Id;

                    if (!string.IsNullOrEmpty(txtIndiceUVA.Text))
                    {
                        decimal _indice = Convert.ToDecimal(txtIndiceUVA.Text);
                        if (_indice != 0)
                        {
                            indice.Valor = _indice;

                            indice.Fecha = fecha;

                            Int32 id = indice.Save();

                            //cUVA.ActualizarIndiceUVACuotas(id.ToString(), fecha.AddMonths(1), indice_anterior);
                            cUVA.ActualizarCuotasUVA(id.ToString());

                            lvUVA.EditIndex = -1;
                            lvUVA.DataSource = cUVA.GetIndiceUVA();
                            lvUVA.DataBind();

                            ModalPopupExtender1.Hide();

                            pnlMensajeIndiceUVA.Visible = false;

                            ActualizarCuentasCorrientesUVA();

                            Response.Redirect("Configuracion.aspx");
                        }
                        else
                        {
                            pnlMensajeIndiceUVA.Visible = true;
                            lbMensajeUVA.Text = "El índice debe ser superior a 0.";
                        }
                    }
                }
                else
                {
                    pnlMensajeIndiceUVA.Visible = true;
                    lbMensajeUVA.Text = "El índice para este mes ya fue ingresado";
                }
            }
        }

        protected void btnCancelarUVA_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
        }
        #endregion





        protected void btnModificarUVA_Click(object sender, EventArgs e)
        {
            DateTime fecha = DateTime.Now.AddMonths(-2);

            lbFechaIndiceM.Text = String.Format("{0:MMMM yyyy}", fecha);
            mpeIndiceM.Show();
        }

        protected void btnFinalizarIndiceUVAM_Click(object sender, EventArgs e)
        {
            decimal indice1 = Convert.ToDecimal(txtIndiceUVAM.Text);
            decimal indice2 = Convert.ToDecimal(txtConfirmIndiceUVAM.Text);

            if (indice1 != indice2)
            {
                pnlMensajeIndiceUVAM.Visible = true;
                lbMensajeUVAM.Text = "Los índices ingresados no coinciden.";
            }
            else
            {
                cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
                DateTime fecha = Convert.ToDateTime("15" + lbFechaIndiceUVAM.Text);
                //DateTime fecha = Convert.ToDateTime("15" + " diciembre 2017");

                if (lastIndice.Fecha < fecha)
                {
                    cIndiceCAC indice = new cIndiceCAC();
                    string indice_anterior = lastIndice.Id;

                    if (!string.IsNullOrEmpty(txtIndiceUVAM.Text))
                    {
                        decimal _indice = Convert.ToDecimal(txtIndiceUVAM.Text);
                        if (_indice != 0)
                        {
                            indice.Valor = _indice;

                            indice.Fecha = fecha;

                            Int32 id = indice.Save();

                            cUVA.ActualizarCuotasUVA(id.ToString());

                            lvCAC.EditIndex = -1;
                            lvCAC.DataSource = cIndiceCAC.GetIndiceCAC();
                            lvCAC.DataBind();

                            mpeIndiceUVAM.Hide();

                            pnlMensajeIndiceCAC.Visible = false;

                            ActualizarCuentasCorrientesRecargaUVA();

                            Response.Redirect("Configuracion.aspx");
                        }
                        else
                        {
                            pnlMensajeIndiceCACM.Visible = true;
                            lbMensajeM.Text = "El índice debe ser superior a 0.";
                        }
                    }
                }
                else
                {
                    pnlBotonesM.Visible = false;
                    pnlMensajeIndiceCACM.Visible = true;
                    lbMensajeM.Text = "El índice para este mes ya fue ingresado. ¿Desea volver a cargar el índice?";
                }
            }
        }

        public void ActualizarCuentasCorrientesRecargaUVA()
        {
            List<cCuentaCorrienteUsuario> cuentas = cCuentaCorrienteUsuario.GetCuentaCorriente();

            foreach (cCuentaCorrienteUsuario c in cuentas)
            {
                CargarCuotasErrorCAC(DateTime.Now, c.Id, c.IdEmpresa);
                actualizarEstadoCuotas(c.Id);
            }
        }

        public void CargarCuotasErrorUVA(DateTime _fecha, string _idCCU, string _idEmpresa)
        {
            string _idCC = null;
            string _idFormaPagoOv = null;

            decimal idUVA = cUVA.GetLastIdIndice();
            cUVA _indice = cUVA.Load(idUVA.ToString());

            DateTime _lastUVA = _indice.Fecha;

            List<cCuota> cuotas2 = cCuota.GetCuotas(_idEmpresa, _lastUVA, (Int16)eIndice.UVA);
            decimal _saldo = 0;
            decimal valorCuotaVencimiento1 = 0;
            decimal valorCuotaVencimiento2 = 0;

            #region Carga de cuotas del mes correspondiente
            if (cuotas2 != null && cuotas2.Count != 0)
            {
                foreach (cCuota cuota in cuotas2)
                {
                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos) && (cuota.VariacionCAC != 0 || cuota.VariacionUVA != 0))
                    {
                        //if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        //{
                        decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(_idCCU));
                        cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                        string idCCU = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(cc.IdEmpresa);
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

                        cItemCCU existingItem = cItemCCU.GetItemCCUByIdCuota(cuota.Id);
                        if (existingItem != null)
                        {
                            existingItem.IdEstado = (Int16)eEstadoItem.Pagado;
                            existingItem.Save();

                            cItemCCU ccuNew1 = new cItemCCU();
                            ccuNew1.IdCuentaCorrienteUsuario = existingItem.IdCuentaCorrienteUsuario;
                            ccuNew1.Fecha = DateTime.Now;
                            ccuNew1.Concepto = "Anulación de item por UVA incorrecto";
                            ccuNew1.Debito = 0;
                            ccuNew1.Credito = (existingItem.Debito) * -1;

                            string _lastSaldo = cItemCCU.GetLastSaldoByIdCCU(existingItem.IdCuentaCorrienteUsuario);
                            decimal _nuevoSaldo = Convert.ToDecimal(_lastSaldo) + ((existingItem.Debito) * -1);

                            ccuNew1.Saldo = _nuevoSaldo;
                            ccuNew1.IdCuota = cuota.Id;
                            ccuNew1.IdEstado = (Int16)eEstadoItem.Pagado;
                            ccuNew1.TipoOperacion = (Int16)eTipoOperacion.Cuota;
                            int _idItemCCU = ccuNew1.Save();

                            lastSaldo = _nuevoSaldo;
                        }

                        cItemCCU ccuNew = new cItemCCU();
                        ccuNew.IdCuentaCorrienteUsuario = _idCCU;
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
                            cItemCCU item = cItemCCU.GetLastItemById(_idCCU);
                            if (item != null && item.IdEstado == (Int16)eEstadoItem.Pagado)
                            {
                                if (lastSaldo > valorCuotaVencimiento1)
                                {
                                    decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                    cItemCCU ccuPago = new cItemCCU();
                                    ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                    ccuPago.Fecha = DateTime.Now;
                                    ccuPago.Concepto = "Pago cuota " + cuota.Nro + " por saldo a favor"; ;
                                    ccuPago.Debito = 0;
                                    ccuPago.Credito = 0;
                                    ccuPago.Saldo = diferencia;
                                    ccuPago.IdCuota = cuota.Id;
                                    ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                    int _idItemCCU = ccuPago.Save();

                                    //Genera el recibo del pago
                                    //cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), cuota.Vencimiento1);
                                    cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(idCCU);
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
                                    ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                    ccuPago.Fecha = DateTime.Now;
                                    ccuPago.Concepto = "Pago parcial cuota " + cuota.Nro + " por saldo a favor";
                                    ccuPago.Debito = 0;
                                    ccuPago.Credito = 0;
                                    ccuPago.Saldo = diferencia;
                                    ccuPago.IdCuota = cuota.Id;
                                    ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                    ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                    int _idItemCCU = ccuPago.Save();

                                    //Genera el recibo del pago
                                    cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(_idCCU);
                                    cReciboCuota recibo = new cReciboCuota(cuota.Id, _idItemCCU.ToString(), lastNroRecibo.Nro, DateTime.Now);
                                    recibo.Monto = lastNroRecibo.Monto;
                                    recibo._Papelera = 1;
                                    recibo.Save();
                                }
                            }
                        }
                        #endregion
                        //}
                    }
                    else
                    {
                        if (cuota.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        {
                            if (cItemCCU.GetCantCuotasById(cuota.Id) == 0)
                            {
                                decimal lastSaldo = Convert.ToDecimal(cItemCCU.GetLastSaldoByIdCCU(_idCCU));
                                cCuentaCorriente cc = cCuentaCorriente.Load(cuota.IdCuentaCorriente);
                                string idCCU = cCuentaCorrienteUsuario.GetCuentaCorrienteByIdEmpresa(cc.IdEmpresa);
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
                                    ccuNew.IdCuentaCorrienteUsuario = _idCCU;
                                    ccuNew.Concepto = "Cuota nro " + cuota.Nro;
                                    ccuNew.Debito = valorCuotaVencimiento1 * -1;

                                    _saldo += lastSaldo;
                                    if (_saldo < 0)
                                        ccuNew.Saldo = (valorCuotaVencimiento1 * -1) + (_saldo);
                                    else
                                        ccuNew.Saldo = _saldo - valorCuotaVencimiento1;

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
                                        cItemCCU item = cItemCCU.GetLastItemById(_idCCU);
                                        if (item != null && item.IdEstado == (Int16)eEstadoItem.Pagado)
                                        {
                                            if (lastSaldo > valorCuotaVencimiento1)
                                            {
                                                decimal diferencia = lastSaldo - valorCuotaVencimiento1;

                                                cItemCCU ccuPago = new cItemCCU();
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago cuota " + cuota.Nro + " por saldo a favor";
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = cuota.Vencimiento1;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //Genera el recibo del pago
                                                //cReciboCuota recibo = cReciboCuota.CrearRecibo(cuota.Id, _idItemCCU.ToString(), cuota.Vencimiento1);
                                                cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(idCCU);
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
                                                ccuPago.IdCuentaCorrienteUsuario = _idCCU;
                                                ccuPago.Fecha = DateTime.Now;
                                                ccuPago.Concepto = "Pago parcial cuota " + cuota.Nro + " por saldo a favor";
                                                ccuPago.Debito = 0;
                                                ccuPago.Credito = 0;
                                                ccuPago.Saldo = diferencia;
                                                ccuPago.IdCuota = cuota.Id;
                                                ccuPago.IdEstado = (Int16)eEstadoItem.Pagado;
                                                ccuPago.TipoOperacion = (Int16)eTipoOperacion.PagoCuota;

                                                int _idItemCCU = ccuPago.Save();

                                                //Genera el recibo del pago
                                                cReciboCuota lastNroRecibo = cReciboCuota.GetLastReciboByCCU(_idCCU);
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
                }
            }
            #endregion
        }

        protected void btnCancelarUVAM_Click(object sender, EventArgs e)
        {
            mpeIndiceUVAM.Hide();
        }

        protected void btnIndiceSiUVAM_Click(object sender, EventArgs e)
        {
            decimal indice1 = Convert.ToDecimal(txtIndiceUVAM.Text);
            decimal indice2 = Convert.ToDecimal(txtConfirmIndiceUVAM.Text);

            if (indice1 != indice2)
            {
                pnlMensajeIndiceUVAM.Visible = true;
                lbMensajeUVAM.Text = "Los índices ingresados no coinciden.";
            }
            else
            {
                cUVA lastIndice = cUVA.Load(cUVA.GetLastIdIndice().ToString());
                DateTime fecha = Convert.ToDateTime("15" + lbFechaIndiceM.Text);
                //DateTime fecha = Convert.ToDateTime("15" + " diciembre 2017");

                if (lastIndice.Fecha.Month == fecha.Month)
                    lastIndice = cUVA.Load(cUVA.GetLastIndiceMonth().ToString());

                cUVA indice = new cUVA();
                string indice_anterior = lastIndice.Id;

                if (!string.IsNullOrEmpty(txtIndiceUVAM.Text))
                {
                    decimal _indice = Convert.ToDecimal(txtIndiceUVAM.Text);
                    if (_indice != 0)
                    {
                        indice.Valor = _indice;

                        indice.Fecha = fecha;

                        Int32 id = indice.Save();

                        cIndiceCAC.ActualizarIndiceRecargaCACCuotas(id.ToString(), fecha.AddMonths(1), indice_anterior);

                        lvCAC.EditIndex = -1;
                        lvCAC.DataSource = cIndiceCAC.GetIndiceCAC();
                        lvCAC.DataBind();

                        mpeIndiceUVAM.Hide();

                        pnlMensajeIndiceCACM.Visible = false;

                        ActualizarCuentasCorrientesRecargaUVA();

                        Response.Redirect("Configuracion.aspx");
                    }
                    else
                    {
                        pnlMensajeIndiceUVAM.Visible = true;
                        lbMensajeUVAM.Text = "El índice debe ser superior a 0.";
                    }
                }
            }
        }


        #region Renovar servicio
        //protected void btnFinalizarServicio_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        cAutorizacionUVA uva = cAutorizacionUVA.GetLast();
        //        uva.Papelera = (Int16)papelera.Eliminado;
        //        uva.Save();

        //        new cAutorizacionUVA(Convert.ToDateTime(txtFechaVenc.Text), txtHeader.Text);

        //        pnlMensajeServicio.Visible = true;
        //        txtFechaVenc.Text = "";
        //        txtHeader.Text = "";

        //        mpeServicio.Hide();
        //    }
        //    catch (Exception ex)
        //    {
        //        log4net.Config.XmlConfigurator.Configure();
        //        log.Error("Configuracion - " + DateTime.Now + "- " + ex.Message + " - btnFinalizarServicio_Click");
        //        Response.Redirect("MensajeError.aspx");
        //    }
        //}

        //protected void btnCancelarServicio_Click(object sender, EventArgs e)
        //{
        //    mpeServicio.Hide();
        //}
        #endregion
    }
}