using DLL;
using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public static void ActualizarIndiceCACCuotas(string _idCC, string _idFp, Int16 _nro, Int16 _nro2)
        {
            try
            {
                string aux = null;
                //CAMBIAR
                List<cCuota> cuotas = cCuota.GetCuotasActivas(_idCC, _idFp);

                foreach (cCuota c in cuotas)
                {
                    //if (aux != c.IdCuentaCorriente)
                    //{
                        cCuentaCorriente cc = cCuentaCorriente.Load(c.IdCuentaCorriente);

                        if (c.Nro >= _nro)
                        {
                            string monedaCuota = null;
                            if (c.IdFormaPagoOV == "-1")
                                monedaCuota = cc.GetMoneda;
                            else
                                monedaCuota = cFormaPagoOV.Load(c.IdFormaPagoOV).GetMoneda;

                            if (monedaCuota == tipoMoneda.Pesos.ToString())
                            {
                                string indiceBase = cc.IdIndiceCAC;

                                //MODIFICAR
                                //decimal vCAC = cCuota.CalcularVariacionMensualCAC("10120", "10121", true);
                                //decimal vCAC = c.VariacionUVA;
                                /*c.VariacionCAC = Convert.ToDecimal("0");*/
                                decimal vCAC = Convert.ToDecimal("0");

                                //CAMBIAR
                                switch (c.Nro)
                                {
                                    case 2:
                                        //c.VariacionCAC = vCAC;
                                        c.VariacionUVA = Convert.ToDecimal("1,81200");
                                        vCAC = Convert.ToDecimal("1,81200");
                                        break;
                                }
                                

                                decimal _saldo = cc.Saldo;

                                if (c.Nro == 1)
                                    _saldo = cCuota.CalcularSaldoByIndice(Convert.ToDecimal("645460,40"), vCAC);
                                else
                                {
                                    int _saldoAnterior = c.Nro - 1;
                                    _saldo = cCuota.CalcularSaldoByIndice(cCuota.GetCuotaByNro(cc.Id, _saldoAnterior, c.IdFormaPagoOV).Saldo, vCAC);
                                }

                                decimal interes = 0;
                                cFormaPagoOV _fp = cFormaPagoOV.Load(c.IdFormaPagoOV);
                                if (_fp.InteresAnual != 0)
                                {
                                    interes = Convert.ToDecimal(_fp.InteresAnual) / 12;
                                    interes = interes + 100;
                                    _saldo = (_saldo * interes) / 100;
                                }

                                decimal valorCuota = 0;
                                cOperacionVenta ov = cOperacionVenta.Load(cc.IdOperacionVenta);
                                List<cFormaPagoOV> fps = cFormaPagoOV.GetFormaPagoOVByIdOV(ov.Id);
                                foreach (cFormaPagoOV f in fps)
                                {
                                    //CAMBIAR
                                    if (f.Id == _idFp)
                                    {
                                        if (c.IdFormaPagoOV == f.Id)
                                        {
                                            if (f.GetMoneda == tipoMoneda.Pesos.ToString())
                                            {
                                                cFormaPagoOV fp = cFormaPagoOV.Load(f.Id);

                                                int _cantAnticipo = cCuota.GetCuotasAnticipos(cc.Id, fp.Id).Count;
                                                //int _cantCuota = ((fp.CantCuotas - c.Nro) + 1) - _cantAnticipo;
                                                int _cantCuota = (fp.CantCuotas - c.Nro) + 1;
                                                _cantCuota = _cantCuota - _cantAnticipo;

                                                valorCuota = c.Nro != 1 ? cCuota.CalcularCuota(_cantCuota, _saldo) : cCuota.CalcularCuota(fp.CantCuotas, _saldo);

                                               



                                                actualizarCuotas(cc.Id, c.Nro, fp.CantCuotas, _cantAnticipo, tipoMoneda.Pesos.ToString(), _saldo, ov);
                                            }
                                        }
                                    }
                                }
                                
                                c.Monto = valorCuota;
                                c.MontoAjustado = _saldo;
                                c.Saldo = _saldo - valorCuota;

                                decimal _vencimiento1 = valorCuota + cCuota.CalcularComisionIva(valorCuota, c.Comision, cc.Iva);
                                c.TotalComision = cCuota.CalcularComisionIva(valorCuota, c.Comision, cc.Iva);
                                c.Vencimiento1 = _vencimiento1;
                                c.Vencimiento2 = cCuota.Calcular2Venc(_vencimiento1);
                                c.Save();
                            }

                            aux = c.IdCuentaCorriente;
                        }
                    
                       

                   // }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void actualizarCuotas(string _idCC, int _nroCuota, int _cantCuota, int _cantAnticipo, string _moneda, decimal _saldo1, cOperacionVenta _idOV)
        {
            decimal _totalComision = 0;
            decimal _vencimiento1 = 0;
            decimal _montoAjustado = 0;
            Int16 _nro = 0;

            Int16 _cuotasPagas = cCuota.GetCantCuotasPagas(_idCC);
            //int _cuotasRestantes = _cantCuota - _cuotasPagas;
            //int _cuotasRestantes = _cantCuota;
            int _cuotasRestantes = ((_cantCuota - _nroCuota) + 1) - _cantAnticipo;
            
            #region Actualizo el resto de las cuotas
            foreach (cCuota c in cCuota.GetCuotasByNro(_idCC, _nroCuota, _cantCuota))
            {
                if (_nro != c.Nro)
                {
                    if (c.Estado != (Int16)estadoCuenta_Cuota.Anticipo)
                    {
                        if (_moneda == tipoMoneda.Pesos.ToString())
                        {
                            decimal valorCuota = 0;

                            if (_cuotasRestantes != 0)
                                valorCuota = cCuota.CalcularCuota(_cuotasRestantes, _saldo1);
                            else
                                valorCuota = cCuota.CalcularCuota(1, _saldo1);

                            _totalComision = cCuota.CalcularComisionIva(valorCuota, c.Comision, _idOV.Iva);

                            _vencimiento1 = valorCuota + _totalComision;

                            if (c.VariacionCAC != 0)
                                _montoAjustado = cCuota.CalcularSaldoByIndice(_saldo1, c.VariacionCAC);
                            else
                                _montoAjustado = _saldo1;
                               
                            c.MontoAjustado = _montoAjustado;
                            c.TotalComision = _totalComision;
                            c.Vencimiento1 = _vencimiento1;
                            _montoAjustado = _montoAjustado - valorCuota;
                            c.Saldo = _montoAjustado;
                            _saldo1 = _montoAjustado;

                            //Redondea la última cuota
                            c.Monto = valorCuota;
                            c.MontoPago = 0;

                            //Al 2° vencimiento se le suma un 2%
                            c.Vencimiento2 = cCuota.Calcular2Venc(_vencimiento1);

                            c.Estado = Convert.ToInt16(estadoCuenta_Cuota.Activa);
                            c.IdRegistroPago = "-1"; //Se guarda con menos -1, hasta que se asocie un pago
                            c.Save();
                            _nroCuota++;
                            _cuotasRestantes--;
                        }

                        _nro = c.Nro;
                    }
                }
            }
            #endregion
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ActualizarIndiceCACCuotas("10581", "370", 2, 21);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            GetUVA();

            /*string valoresUva = WebRequestDolar();
            string[] valores1 = valoresUva.Split('[');
            string[] valores = valores1[1].Split(',');
            int count = valores.Count() - 1;

            string[] uva = valores[count].Split(':');

            string[] v = uva[1].Split('}');
            string v1 = v[0]; */
        }

        public void GetUVA()
        {
            string valoresUva = WebRequest();
            if (!string.IsNullOrEmpty(valoresUva))
            {
                string[] valores1 = valoresUva.Split('[');
                string[] valores = valores1[1].Split(',');
                int count = valores.Count() - 1;

                string[] uva = valores[count].Split(':');

                string[] v = uva[1].Split('}');

                if (!string.IsNullOrEmpty(v[0]))
                    new cUVA(Convert.ToDecimal(v[0].Replace(".", ",")), papelera.Activo).Save();

                string asd = v[0].Replace(".", ",");
                decimal asdad = Convert.ToDecimal(v[0].Replace(".", ","));
            }
        }

        private static string WebRequest()
        {
            const string WEBSERVICE_URL = "http://api.estadisticasbcra.com/uva";
            string jsonResponse = null;
            try
            {
                var webRequest = System.Net.WebRequest.Create(WEBSERVICE_URL);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/json";
                    webRequest.Headers.Add("Authorization", "BEARER eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE1NDAzMDUzMDEsInR5cGUiOiJleHRlcm5hbCIsInVzZXIiOiJudGFidWNjaGlAbmFleC5jb20uYXIifQ.kdTC4rkeAsZNM_Q7uWGIM96ZXr0Oi9ynyUt_QXSYKWAUAYiV5OQmmVfTkZbGAB6XiIjjyriDXZCvoFUuw5Z8JA");

                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            jsonResponse = sr.ReadToEnd();
                        }
                    }
                }
                return jsonResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string WebRequestDolar()
        {
            const string WEBSERVICE_URL = "http://api.estadisticasbcra.com/usd";
            string jsonResponse = null;
            try
            {
                var webRequest = System.Net.WebRequest.Create(WEBSERVICE_URL);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/json";
                    webRequest.Headers.Add("Authorization", "BEARER eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE1NDAzMDUzMDEsInR5cGUiOiJleHRlcm5hbCIsInVzZXIiOiJudGFidWNjaGlAbmFleC5jb20uYXIifQ.kdTC4rkeAsZNM_Q7uWGIM96ZXr0Oi9ynyUt_QXSYKWAUAYiV5OQmmVfTkZbGAB6XiIjjyriDXZCvoFUuw5Z8JA");

                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            jsonResponse = sr.ReadToEnd();
                        }
                    }
                }
                return jsonResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /*************************************************************************************/

        public void GetUVA1()
        {
            string valoresUva = WebRequest();
            if (!string.IsNullOrEmpty(valoresUva))
            {
                string[] valores1 = valoresUva.Split('[');
                string[] valores = valores1[1].Split(',');
                int count = valores.Count() - 1;

                string[] uva = valores[count].Split(':');

                string[] v = uva[1].Split('}');

                if (!string.IsNullOrEmpty(v[0]))
                    new cUVA(Convert.ToDecimal(v[0].Replace(".",",")), papelera.Activo).Save();

                string asd = v[0].Replace(".", ",");
                decimal asdad = Convert.ToDecimal(v[0].Replace(".", ","));
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            GetUVA1();

           /* DateTime day = Convert.ToDateTime("2017" + "-" + "12" + "-" + "24");
            DateTime day1 = Convert.ToDateTime(day.Year + "-" + "10" + "-" + 25);

            cUVA indice = cUVA.Load(cUVA.GetLastIdIndice().ToString());
            string indice_anterior = cUVA.GetIdIndiceByFecha(day1);
            
            cUVA.ActualizarIndiceUVACuotas(indice.Id, day, indice_anterior);

            ActualizarCuentasCorrientesUVA();

            Response.Redirect("Configuracion.aspx");   */       
        }
        

        #region Actualizar cuentas corrientes UVA
        public void ActualizarCuentasCorrientesUVA()
        {
            List<cCuentaCorrienteUsuario> cuentas = cCuentaCorrienteUsuario.GetCuentaCorriente();
            cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());

            foreach (cCuentaCorrienteUsuario c in cuentas)
            {
                DateTime day = Convert.ToDateTime("2017" + "-" + "10" + "-" + "25");


                CargarCuotasUVA(day, c.Id, c.IdEmpresa);
                actualizarEstadoCuotas(c.Id);

                if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
                    CargarCuotasUVA(DateTime.Now.AddMonths(1), c.Id, c.IdEmpresa);
            }
        }

        public void CargarCuotasUVA(DateTime _fecha, string _idCCU, string _idEmpresa)
        {
            string _idCC = null;
            string _idFormaPagoOv = null;

            List<cCuota> cuotas2 = cCuota.GetCuotasPendientesSoloCuotasActivas(_idEmpresa, _fecha, (Int16)eIndice.UVA);
            decimal _saldo = 0;
            decimal valorCuotaVencimiento1 = 0;
            decimal valorCuotaVencimiento2 = 0;

            #region Carga de cuotas del mes correspondiente
            if (cuotas2 != null && cuotas2.Count != 0)
            {
                foreach (cCuota cuota in cuotas2)
                {

                    if (cFormaPagoOV.Load(cuota.IdFormaPagoOV).Moneda == Convert.ToString((Int16)tipoMoneda.Pesos) && cuota.VariacionUVA != 0)
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
        #endregion

        #region Auxiliares
        public void actualizarEstadoCuotas(string _idCCU)
        {
            decimal newSaldo = 0;
            int contCuotasDolar = 0;
            List<cItemCCU> cuotasPendientes = cItemCCU.GetItemByCuotasPendientes(_idCCU);
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
        #endregion
        
    }
}