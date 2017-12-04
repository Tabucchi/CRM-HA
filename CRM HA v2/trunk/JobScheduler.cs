using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Globalization;
using System.Xml;
using System.IO;
using OpenPop.Pop3;
using System.Data;
using OpenPop.Mime;
using OpenPop.Pop3.Exceptions;
using System.Collections;
using log4net;
using DLL.Negocio;
using System.Threading;

public class JobScheduler : IJob
{
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    
    public virtual void Execute(JobExecutionContext context)
    {
        try
        {
            #region Dólar
                // Actualizo Valor Dolar
                //string valorActualDolar = Services.ObtenerValorDolar();
                string valorActualDolar = ObtenerValorDolar();

                if (valorActualDolar.Length == 3) valorActualDolar = valorActualDolar + "0"; //Agrego 0 en caso que sea numero redondo.

                if (valorActualDolar != "-1")
                {
                    new cValorDolar(valorActualDolar, papelera.Activo).Save();
                    //return;
                }   
            #endregion

            /*#region índice UVA
            GetUVA();
            #endregion*/

            //Genera Back up de la base de datos
            //cAuxiliar.ExecuteBackUp();

            //Actualiza las cuentas corrientes
            //Thread thread = new Thread(new ThreadStart(ActualizarCuentasCorrientes));
            //thread.Start();

            ActualizarCuentasCorrientes();

            //Actualiza las cuotas con UVA
            if (DateTime.Now.Day == 25)
                ActualizarCuotasUVA();

        }
        catch (Exception ex)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Error("JobScheduler - " + DateTime.Now + "- " + ex.Message + " - Execute");
            return;
        }
    }

    #region Dolar
    public string ObtenerValorDolar()
    {
        string valoresUva = WebRequestDolar();
        string[] valores1 = valoresUva.Split('[');
        string[] valores = valores1[1].Split(',');
        int count = valores.Count() - 1;

        string[] uva = valores[count].Split(':');

        string[] v = uva[1].Split('}');
        return v[0];
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

                string header = cAutorizacionUVA.GetLast().Header;

                webRequest.Headers.Add("Authorization", header);

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

    public void CargarCuotas(DateTime _fecha, string _idCCU, string _idEmpresa)
    {
        string _idCC = null;
        string _idFormaPagoOv = null;

        List<cCuota> cuotas2 = cCuota.GetCuotasPendientes(_idEmpresa, _fecha);
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

    #region UVA
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

            /*if (!string.IsNullOrEmpty(v[0]))
                new cUVA(Convert.ToDecimal(v[0].Replace(".",",")), papelera.Activo).Save();*/

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

                string header = cAutorizacionUVA.GetLast().Header;

                webRequest.Headers.Add("Authorization", header);

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

    public void ActualizarCuotasUVA()
    {
        GetUVA(); 

        DateTime day = Convert.ToDateTime(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + "25");
        DateTime day1 = Convert.ToDateTime(day.Year + "-" + day.AddMonths(-1).Month + "-" + 25);

        cUVA indice = cUVA.Load(cUVA.GetLastIdIndice().ToString());
        string indice_anterior = cUVA.GetIdIndiceByFecha(day1);

        cUVA.ActualizarIndiceUVACuotas(indice.Id, day, indice_anterior);

        ActualizarCuentasCorrientesUVA();
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
    #endregion
} 