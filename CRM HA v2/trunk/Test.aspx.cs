﻿using DLL;
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
                                case 20:
                                    //c.VariacionCAC = vCAC;
                                    c.VariacionCAC = Convert.ToDecimal("0");
                                    vCAC = Convert.ToDecimal("0");
                                    //1,01000
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
            ActualizarIndiceCACCuotas("10278", "213", 1, 21);
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
                    new cUVA(Convert.ToDecimal(v[0].Replace(".", ",")), papelera.Activo).Save();

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

        protected void Button4_Click(object sender, EventArgs e)
        {
            btnDescargar_Click(null, null);
        }

        #region Descargar
        public decimal CalcularSaldo(string _idProyecto, DateTime dateDesde, DateTime dateHasta)
        {
            decimal _saldoTotal = 0;
            DataTable dtSaldo1 = cCuota.GetCuotasObraByFecha(_idProyecto, dateDesde, dateHasta);

            if (dtSaldo1 != null)
            {
                foreach (DataRow dr in dtSaldo1.Rows)
                {
                    //Si tiene más de una obra diferente
                    ArrayList cantProyectos = cUnidad.GetCantProyectosByOV(dr[5].ToString());

                    if (cantProyectos.Count > 1)
                    {
                        List<cUnidad> unidades = cUnidad.GetUnidadByOV(dr[5].ToString());

                        if (unidades.Count > 1)
                        {
                            cOperacionVenta op = cOperacionVenta.Load(dr[5].ToString());

                            DataTable dt = new DataTable();
                            DataRow dr1;
                            DataSet ds = new DataSet();
                            decimal valorApeso = 0;
                            decimal valorBoletoApeso = 0;
                            decimal cuota = 0;

                            dt.Columns.Add(new DataColumn("idUnidad"));
                            dt.Columns.Add(new DataColumn("PorcentajeUnidad"));
                            dt.Columns.Add(new DataColumn("PorcentajeMonto"));
                            dt.Columns.Add(new DataColumn("idProyecto"));

                            foreach (cUnidad u in unidades)
                            {
                                dr1 = dt.NewRow();

                                cEmpresaUnidad eu = cEmpresaUnidad.GetUnidad(u.CodigoUF, u.IdProyecto);

                                #region Pesificar
                                if (op.GetMoneda == tipoMoneda.Dolar.ToString())
                                {
                                    //Pesificar precio acordado de la unidad
                                    if (u.Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                        valorApeso = eu.PrecioAcordado * cValorDolar.LoadActualValue();
                                    else
                                        valorApeso = eu.PrecioAcordado;
                                }
                                else
                                {
                                    valorApeso = eu.PrecioAcordado;
                                }

                                //Pesificar precio acordado del boleto
                                if (op.MonedaAcordada == Convert.ToString((Int16)tipoMoneda.Dolar))
                                    valorBoletoApeso = op.PrecioAcordado * cValorDolar.LoadActualValue();
                                else
                                    valorBoletoApeso = op.PrecioAcordado;

                                //Pesificar precio de la cuota
                                if (dr[3].ToString() == "0")
                                    cuota = Convert.ToDecimal(dr[2].ToString()) * cValorDolar.LoadActualValue();
                                else
                                    cuota = Convert.ToDecimal(dr[2].ToString());
                                #endregion

                                decimal porcentajeUnidad = (valorApeso * 100) / valorBoletoApeso;
                                decimal porcentajeCuota = Math.Round((porcentajeUnidad * cuota) / 100, 2);

                                dr1["idUnidad"] = u.Id;
                                dr1["PorcentajeUnidad"] = porcentajeUnidad;
                                dr1["PorcentajeMonto"] = porcentajeCuota;
                                dr1["idProyecto"] = u.IdProyecto;
                                dt.Rows.Add(dr1);
                            }

                            foreach (DataRow row in dt.Rows)
                            {
                                if (row[3].ToString() == _idProyecto)
                                {
                                    _saldoTotal += Convert.ToDecimal(row[2].ToString());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dr[1].ToString() == _idProyecto)
                        {
                            if (dr[3].ToString() == "0")
                                _saldoTotal += Convert.ToDecimal(dr[2].ToString()) * cValorDolar.LoadActualValue();
                            else
                                _saldoTotal += Convert.ToDecimal(dr[2].ToString());
                        }
                    }
                }
            }
            return _saldoTotal;
        }

        public decimal CalcularSaldoMesesRestantes(string _idProyecto, DateTime date, int _cantColumnasMes)
        {
            DateTime hoy = Convert.ToDateTime(date.Year + " -  " + date.Month + " -  " + 1);
            DateTime dateRestante = Convert.ToDateTime(hoy.AddMonths(_cantColumnasMes));

            decimal _totalRestante = 0;
            DataTable dtSaldoRestante = cCuota.GetCuotasObraByFechaRestante(_idProyecto, dateRestante);

            foreach (DataRow dr in dtSaldoRestante.Rows)
            {
                //Si tiene más de una obra diferente
                ArrayList cantProyectos = cUnidad.GetCantProyectosByOV(dr[5].ToString());

                if (cantProyectos.Count > 1)
                {
                    List<cUnidad> unidades = cUnidad.GetUnidadByOV(dr[5].ToString());

                    if (unidades.Count > 1)
                    {
                        cOperacionVenta op = cOperacionVenta.Load(dr[5].ToString());

                        DataTable dt = new DataTable();
                        DataRow dr1;
                        DataSet ds = new DataSet();
                        decimal valorApeso = 0;
                        decimal valorBoletoApeso = 0;
                        decimal cuota = 0;

                        dt.Columns.Add(new DataColumn("idUnidad"));
                        dt.Columns.Add(new DataColumn("PorcentajeUnidad"));
                        dt.Columns.Add(new DataColumn("PorcentajeMonto"));
                        dt.Columns.Add(new DataColumn("idProyecto"));

                        foreach (cUnidad u in unidades)
                        {
                            dr1 = dt.NewRow();

                            cEmpresaUnidad eu = cEmpresaUnidad.GetUnidad(u.CodigoUF, u.IdProyecto);

                            #region Pesificar
                            if (op.GetMoneda == tipoMoneda.Dolar.ToString())
                            {
                                //Pesificar precio acordado de la unidad
                                if (u.Moneda == Convert.ToString((Int16)tipoMoneda.Dolar))
                                    valorApeso = eu.PrecioAcordado * cValorDolar.LoadActualValue();
                                else
                                    valorApeso = eu.PrecioAcordado;
                            }
                            else
                            {
                                valorApeso = eu.PrecioAcordado;
                            }

                            //Pesificar precio acordado del boleto
                            if (op.MonedaAcordada == Convert.ToString((Int16)tipoMoneda.Dolar))
                                valorBoletoApeso = op.PrecioAcordado * cValorDolar.LoadActualValue();
                            else
                                valorBoletoApeso = op.PrecioAcordado;

                            //Pesificar precio de la cuota
                            if (dr[3].ToString() == "0")
                                cuota = Convert.ToDecimal(dr[2].ToString()) * cValorDolar.LoadActualValue();
                            else
                                cuota = Convert.ToDecimal(dr[2].ToString());
                            #endregion

                            decimal porcentajeUnidad = (valorApeso * 100) / valorBoletoApeso;
                            decimal porcentajeCuota = Math.Round((porcentajeUnidad * cuota) / 100, 2);

                            dr1["idUnidad"] = u.Id;
                            dr1["PorcentajeUnidad"] = porcentajeUnidad;
                            dr1["PorcentajeMonto"] = porcentajeCuota;
                            dr1["idProyecto"] = u.IdProyecto;
                            dt.Rows.Add(dr1);
                        }

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[3].ToString() == _idProyecto)
                            {
                                _totalRestante += Convert.ToDecimal(row[2].ToString());
                            }
                        }
                    }
                }
                else
                {
                    if (dr[1].ToString() == _idProyecto)
                    {
                        if (dr[3].ToString() == "0")
                            _totalRestante += Convert.ToDecimal(dr[2].ToString()) * cValorDolar.LoadActualValue();
                        else
                            _totalRestante += Convert.ToDecimal(dr[2].ToString());
                    }
                }
            }

            return _totalRestante;
        }

        public DateTime GetFecha()
        {
            DateTime date = new DateTime();

            cIndiceCAC lastIndice = cIndiceCAC.Load(cIndiceCAC.GetLastIndice().ToString());
            if (lastIndice.Fecha.Month == DateTime.Now.AddMonths(-1).Month)
                date = DateTime.Now.AddMonths(1);
            else
                date = DateTime.Now;

            return date;
        }

        decimal totalCtaCte = 0;
        decimal totalMes1 = 0;
        decimal totalMes2 = 0;
        decimal totalMes3 = 0;
        decimal totalMes4 = 0;
        decimal totalMesesRestantes = 0;
        decimal totalDeuda = 0;
        decimal total = 0;

        public List<cCuotasObra> listSaldos()
        {
            List<cProyecto> list4 = cProyecto.GetProyectos();
            List<cCuotasObra> saldos = new List<cCuotasObra>();
            List<cCuotasObra> saldosCtaCte = new List<cCuotasObra>();
            int cantColumnasMes = 5;

            foreach (var item in list4)
            {
                cCuotasObra saldo = new cCuotasObra();
                decimal _total = 0;

                DateTime date = GetFecha();
                DateTime dateDesde = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "1");
                DateTime dateHasta = Convert.ToDateTime(date.Year.ToString() + " - " + date.Month.ToString() + " - " + "29");

                saldo.proyecto = item.Descripcion;
                saldo.idProyecto = item.Id;

                #region 4 meses
                //Saldo 1
                decimal _saldo1Total = CalcularSaldo(item.Id, dateDesde.AddMonths(1), dateHasta.AddMonths(1));
                saldo.saldo1 = String.Format("{0:#,#0.00}", _saldo1Total);
                totalMes1 += _saldo1Total;

                //Saldo 2
                decimal _saldo2Total = CalcularSaldo(item.Id, dateDesde.AddMonths(2), dateHasta.AddMonths(2));
                saldo.saldo2 = String.Format("{0:#,#0.00}", _saldo2Total);
                totalMes2 += _saldo2Total;

                //Saldo 3
                decimal _saldo3Total = CalcularSaldo(item.Id, dateDesde.AddMonths(3), dateHasta.AddMonths(3));
                saldo.saldo3 = String.Format("{0:#,#0.00}", _saldo3Total);
                totalMes3 += _saldo3Total;

                //Saldo 4
                decimal _saldo4Total = CalcularSaldo(item.Id, dateDesde.AddMonths(4), dateHasta.AddMonths(4));
                saldo.saldo4 = String.Format("{0:#,#0.00}", _saldo4Total);
                totalMes4 += _saldo4Total;
                #endregion

                #region Meses restantes
                decimal _totalRestante = CalcularSaldoMesesRestantes(item.Id, date, cantColumnasMes);
                saldo.mesesRestantes = String.Format("{0:#,#0.00}", _totalRestante);
                totalMesesRestantes += _totalRestante;
                #endregion

                _total = _saldo1Total + _saldo2Total + _saldo3Total + _saldo4Total + _totalRestante;
                saldo.total = String.Format("{0:#,#0.00}", _total);
                totalDeuda += _total;

                saldos.Add(saldo);
            }

            return saldos;
        }
                
        private DataSet CrearDataSet()
        {
            List<cCuotasObra> saldos = listSaldos();

            DataTable dt = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();

            dt.Columns.Add(new DataColumn("obra"));
            dt.Columns.Add(new DataColumn("mes1"));
            dt.Columns.Add(new DataColumn("mes2"));
            dt.Columns.Add(new DataColumn("mes3"));
            dt.Columns.Add(new DataColumn("mes4"));
            dt.Columns.Add(new DataColumn("mesesRestantes"));
            dt.Columns.Add(new DataColumn("total"));

            foreach (cCuotasObra p in saldos)
            {
                dr = dt.NewRow();
                dr["obra"] = p.proyecto;
                dr["mes1"] = p.saldo1;
                dr["mes2"] = p.saldo2;
                dr["mes3"] = p.saldo3;
                dr["mes4"] = p.saldo4;
                dr["mesesRestantes"] = p.mesesRestantes;
                dr["total"] = p.total;
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            ds.Tables[0].TableName = "tCuotasObra";

            ds.Tables[0].DefaultView.Sort = "obra";

            return ds;
        }
        
        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string rutaURL = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Cuotas.pdf";
            //string filename = "Cuotas a cobrar por obra.pdf";

            CrystalDecisions.Web.CrystalReportSource s = new CrystalDecisions.Web.CrystalReportSource();
            s.Report.FileName = HttpContext.Current.Request.PhysicalApplicationPath + "Reportes\\CuotasObra.rpt";

            // Planilla
            DataSetUnidades ds = new DataSetUnidades();
            ds.Merge(CrearDataSet(), false, System.Data.MissingSchemaAction.Ignore);
            s.ReportDocument.SetDataSource(ds);

            #region Encabezado

            DateTime date = GetFecha();

            s.ReportDocument.SetParameterValue("titleMes1", String.Format("{0:MMM-yy}", date.AddMonths(1)));
            s.ReportDocument.SetParameterValue("titleMes2", String.Format("{0:MMM-yy}", date.AddMonths(2)));
            s.ReportDocument.SetParameterValue("titleMes3", String.Format("{0:MMM-yy}", date.AddMonths(3)));
            s.ReportDocument.SetParameterValue("titleMes4", String.Format("{0:MMM-yy}", date.AddMonths(4)));

            #endregion

            s.ReportDocument.SetParameterValue("ctaCte", String.Format("{0:#,#0.00}", (cCuentaCorrienteUsuario.GetTotalCtaCte() * -1)));
            s.ReportDocument.SetParameterValue("totalMes1", String.Format("{0:#,#0.00}", totalMes1));
            s.ReportDocument.SetParameterValue("totalMes2", String.Format("{0:#,#0.00}", totalMes2));
            s.ReportDocument.SetParameterValue("totalMes3", String.Format("{0:#,#0.00}", totalMes3));
            s.ReportDocument.SetParameterValue("totalMes4", String.Format("{0:#,#0.00}", totalMes4));
            s.ReportDocument.SetParameterValue("totalMesesRestantes", String.Format("{0:#,#0.00}", totalMesesRestantes));
            s.ReportDocument.SetParameterValue("totalDeuda", String.Format("{0:#,#0.00}", totalDeuda));
            s.ReportDocument.SetParameterValue("total", String.Format("{0:#,#0.00}", totalDeuda + (cCuentaCorrienteUsuario.GetTotalCtaCte() * -1)));

            //s.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL + filename);
            s.ReportDocument.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaURL);
            
            //cArchivoCuotasObra archivo = new cArchivoCuotasObra()

            FileStream stream = new FileStream(rutaURL, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            cArchivoCuotasObra arch = new cArchivoCuotasObra(reader.ReadBytes((int)stream.Length));
            arch.Save();
            
            stream.Close();
        }

        #endregion
    }
}