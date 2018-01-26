using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace crm.Controles
{
    public partial class CuotasAnticipo : System.Web.UI.UserControl
    {
        private string idCC;
        private string idFormaPagoOV;

        #region Propiedades
        public string IdCC
        {
            get { return idCC; }
            set { idCC = value; }
        }
        public string IdFormaPagoOV
        {
            get { return idFormaPagoOV; }
            set { idFormaPagoOV = value; }
        }

        public ListView listView
        {
            get
            {
                return lvCuotas;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hfCC.Value = IdCC;
                hfFormaPagoOv.Value = idFormaPagoOV;
            }
        }

        public void CargarListView(string idCC, string idFormaPagoOV, string cantCuotasAdelanto)
        {
            List<cCuota> listCuotas = cCuota.GetCuotasActivasByIdFormaPagoOV(idCC, idFormaPagoOV, cantCuotasAdelanto);
            hfCC.Value = IdCC;
            hfFormaPagoOv.Value = idFormaPagoOV;
            lvCuotas.DataSource = listCuotas;
            lvCuotas.DataBind();
        }

        public void CargarListViewCuota(cCuota cuota)
        {
            List<cCuota> listCuotas = new List<cCuota>();
            listCuotas.Add(cuota);
            lvCuotas.DataSource = listCuotas;
            lvCuotas.DataBind();
        }

        public void CargarListViewPago(string idCC, string idFormaPagoOV, string cantCuotasAdelanto)
        {
            List<cCuota> listCuotas = cCuota.GetCuotasPagoByIdFormaPagoOV(idCC, idFormaPagoOV);
            hfCC.Value = IdCC;
            hfFormaPagoOv.Value = idFormaPagoOV;
            lvCuotas.DataSource = listCuotas;
            lvCuotas.DataBind();
        }

        public void btnFinalizar_Click(string cuotaPagar)
        {
            #region Variables
            ArrayList cuotasAdelantadas = new ArrayList();
            ArrayList montosCuota = new ArrayList();
            decimal sumMontoCuota = 0;

            string idCuota = null;
            decimal _saldoAjustado = 0;
            decimal _saldo = 0;
            decimal valorCuota = 0;
            decimal _cuota = 0;
            decimal _saldoAux = 0;
            #endregion

            cCuentaCorriente cc = cCuentaCorriente.Load(hfCC.Value);

            foreach (ListViewItem item in lvCuotas.Items)
            {
                Label id = item.FindControl("lbId") as Label;
                cuotasAdelantadas.Add(id.Text);

                Label monto = item.FindControl("lbMonto") as Label;
                montosCuota.Add(monto.Text);
            }

            #region Se actualizan los datos de la cuota que se paga
            cCuota cuota = cCuota.GetCuotaByNro(hfCC.Value, hfFormaPagoOv.Value, Convert.ToInt16(cuotaPagar));
            idCuota = cuota.Id;

            foreach (string monto in montosCuota)
                sumMontoCuota += Convert.ToDecimal(monto);

            decimal montoCuota = cuota.Monto + sumMontoCuota;
            decimal _vencimiento1 = cuota.Vencimiento1 + sumMontoCuota; //Se suma el valor de la cuota porque no se agregan los gastos adtvo.

            cuota.Monto = montoCuota;
            cuota.Vencimiento1 = _vencimiento1;
            cuota.Vencimiento2 = cCuota.Calcular2Venc(_vencimiento1);

            _saldo = cuota.MontoAjustado - montoCuota;

            cuota.Saldo = cuota.MontoAjustado - montoCuota;
            cuota.MontoPago = montoCuota;
            cuota.Estado = (Int16)estadoCuenta_Cuota.Pagado;
            cuota.Save();
            #endregion

            #region Se cambio el estado de las cuotas que se adelantaron
            foreach (string _idCuota in cuotasAdelantadas)
            {
                cCuota _cuotaAdelantada = cCuota.Load(_idCuota);
                _cuotaAdelantada.MontoAjustado = 0;
                _cuotaAdelantada.Saldo = 0;
                _cuotaAdelantada.Estado = (Int16)estadoCuenta_Cuota.Anticipo;
                _cuotaAdelantada.Save();
            }
            #endregion

            #region Recalcula las cuotas
            cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);
            int _cantCuotas = fp.CantCuotas - cCuota.GetCantCuotasPagasAdelantos(cc.Id, fp.Id);

            foreach (cCuota c in cCuota.GetCuotasActivas(hfCC.Value, hfFormaPagoOv.Value))
            {
                if (c.Estado == (Int16)estadoCuenta_Cuota.Activa)
                {
                    decimal variacionCAC = c.VariacionCAC;

                    _saldoAjustado = cCuota.CalcularSaldoByIndice(_saldo, variacionCAC);
                    _cuota = _saldoAjustado / _cantCuotas;

                    if (variacionCAC != 0)
                    {
                        valorCuota = cCuota.CalcularCuota(_cantCuotas, _saldoAjustado);
                        c.MontoAjustado = _saldoAjustado;
                        _vencimiento1 = valorCuota + cCuota.CalcularComisionIva(valorCuota, c.Comision, cc.Iva);
                        c.Vencimiento1 = _vencimiento1;
                        c.Saldo = _saldoAjustado - valorCuota;
                    }
                    else
                    {
                        valorCuota = _cuota;
                        _saldoAux = _saldo;

                        c.MontoAjustado = _saldoAux;
                        _vencimiento1 = valorCuota + cCuota.CalcularComisionIva(valorCuota, c.Comision, cc.Iva);
                        c.Vencimiento1 = _vencimiento1;
                        c.Saldo = _saldoAux - valorCuota;
                    }

                    c.Monto = valorCuota;
                    c.Vencimiento2 = cCuota.Calcular2Venc(_vencimiento1);
                    c.Save();
                }
            }
            #endregion

            #region Actualización del saldo de la CC
            cOperacionVenta op = cOperacionVenta.Load(fp.IdOperacionVenta);
            if (fp.GetMoneda == tipoMoneda.Dolar.ToString())
            {
                cc.Saldo = cc.Saldo - cuota.Monto;
                cc.SaldoPeso = cc.SaldoPeso - cValorDolar.ConvertToPeso(cuota.Monto, Convert.ToDecimal(op.ValorDolar));
            }
            else
            {
                cc.Saldo = cc.Saldo - cValorDolar.ConvertToDolar(cuota.Monto, Convert.ToDecimal(op.ValorDolar));
                cc.SaldoPeso = cc.SaldoPeso - cuota.Monto;
            }
            cc.Save();
            #endregion

            Response.Redirect("DetalleCuota2.aspx?idCC=" + hfCC.Value);
        }

        public decimal sumMontoCuota()
        {
            decimal sumMontoCuota = 0;
            foreach (ListViewItem item in lvCuotas.Items)
            {
                Label monto = item.FindControl("lbMonto") as Label;
                sumMontoCuota += Convert.ToDecimal(monto.Text);
            }
            return sumMontoCuota;
        }

        public void btnFinalizarCC_Click(string _idCC)
        {
            #region Variables
            ArrayList cuotasAdelantadas = new ArrayList();
            ArrayList montosCuota = new ArrayList();
            decimal sumMontoCuota = 0;

            decimal _saldoAjustado = 0;
            decimal _saldo = 0;
            decimal valorCuota = 0;
            decimal _cuota = 0;
            decimal _saldoAux = 0;
            decimal _vencimiento1 = 0;
            #endregion

            cCuentaCorriente cc = cCuentaCorriente.Load(_idCC);

            foreach (ListViewItem item in lvCuotas.Items)
            {
                Label id = item.FindControl("lbId") as Label;
                cuotasAdelantadas.Add(id.Text);

                Label monto = item.FindControl("lbMonto") as Label;
                montosCuota.Add(monto.Text);
            }

            #region Se cambio el estado de las cuotas que se adelantaron
            foreach (string _idCuota in cuotasAdelantadas)
            {
                cCuota _cuotaAdelantada = cCuota.Load(_idCuota);
                _cuotaAdelantada.MontoAjustado = 0;
                _cuotaAdelantada.Saldo = 0;
                _cuotaAdelantada.Estado = (Int16)estadoCuenta_Cuota.Anticipo;
                _cuotaAdelantada.Save();
            }
            #endregion

            #region Recalcula las cuotas
            string _idFp = cCuota.GetLast(cc.Id).IdFormaPagoOV;
            cFormaPagoOV fp = cFormaPagoOV.Load(_idFp);
            int _cantCuotas = fp.CantCuotas - cCuota.GetCantCuotasPagasAdelantos(cc.Id, fp.Id);

            foreach (cCuota c in cCuota.GetCuotasActivas(_idCC, hfFormaPagoOv.Value))
            {
                if (c.Estado == (Int16)estadoCuenta_Cuota.Activa)
                {
                    decimal variacionCAC = c.VariacionCAC;

                    _saldoAjustado = cCuota.CalcularSaldoByIndice(_saldo, variacionCAC);
                    _cuota = _saldoAjustado / _cantCuotas;

                    if (variacionCAC != 0)
                    {
                        valorCuota = cCuota.CalcularCuota(_cantCuotas, _saldoAjustado);
                        c.MontoAjustado = _saldoAjustado;
                        _vencimiento1 = valorCuota + cCuota.CalcularComisionIva(valorCuota, c.Comision, cc.Iva);
                        c.Vencimiento1 = _vencimiento1;
                        c.Saldo = _saldoAjustado - valorCuota;
                    }
                    else
                    {
                        valorCuota = _cuota;
                        _saldoAux = _saldo;

                        c.MontoAjustado = _saldoAux;
                        _vencimiento1 = valorCuota + cCuota.CalcularComisionIva(valorCuota, c.Comision, cc.Iva);
                        c.Vencimiento1 = _vencimiento1;
                        c.Saldo = _saldoAux - valorCuota;
                    }

                    c.Monto = valorCuota;
                    c.Vencimiento2 = cCuota.Calcular2Venc(_vencimiento1);
                    c.Save();
                }
            }
            #endregion

            #region Actualización del saldo de la CC
            foreach (string monto in montosCuota)
                sumMontoCuota += Convert.ToDecimal(monto);

            cOperacionVenta op = cOperacionVenta.Load(fp.IdOperacionVenta);
            if (fp.GetMoneda == tipoMoneda.Dolar.ToString())
            {
                cc.Saldo = cc.Saldo - sumMontoCuota;
                cc.SaldoPeso = cc.SaldoPeso - cValorDolar.ConvertToPeso(sumMontoCuota, Convert.ToDecimal(op.ValorDolar));
            }
            else
            {
                cc.Saldo = cc.Saldo - cValorDolar.ConvertToDolar(sumMontoCuota, Convert.ToDecimal(op.ValorDolar));
                cc.SaldoPeso = cc.SaldoPeso - sumMontoCuota;
            }
            cc.Save();
            #endregion
        }

        public void btnFinalizarAdelanto_Click(string _idCC, decimal _montoAdelanto)
        {
            #region Variables
            ArrayList cuotasAdelantadas = new ArrayList();
            ArrayList montosCuota = new ArrayList();
            decimal sumMontoCuota = 0;
            decimal _saldo = 0;
            #endregion

            cCuentaCorriente cc = cCuentaCorriente.Load(_idCC);

            foreach (ListViewItem item in lvCuotas.Items)
            {
                Label id = item.FindControl("lbId") as Label;
                cuotasAdelantadas.Add(id.Text);

                Label monto = item.FindControl("lbMonto") as Label;
                montosCuota.Add(monto.Text);
            }

            #region Se cambio el estado de las cuotas que se adelantaron
            foreach (string _idCuota in cuotasAdelantadas)
            {
                cCuota _cuotaAdelantada = cCuota.Load(_idCuota);
                _cuotaAdelantada.MontoAjustado = 0;
                _cuotaAdelantada.Saldo = 0;
                _cuotaAdelantada.Estado = (Int16)estadoCuenta_Cuota.Anticipo;
                _cuotaAdelantada.Save();
            }
            #endregion

            #region Recalcula las cuotas
            foreach (string monto in montosCuota)
                sumMontoCuota += Convert.ToDecimal(monto);

            cCuota cuota = cCuota.GetFirstActiva(_idCC, hfFormaPagoOv.Value);
            //_saldo = cuota.MontoAjustado - cuota.Monto - _montoAdelanto;
            _saldo = cuota.MontoAjustado - _montoAdelanto;
            //decimal _monto = cuota.Monto + _montoAdelanto;
            //cuota.Monto = _monto;
            cuota.Saldo = _saldo;
            cuota.Save();

            //Actualizo el saldo de la cuota anterior
            if (cuota.Nro != 1)
            {
                cCuota c = cCuota.GetCuotaByNro(_idCC, cuota.Nro - 1, hfFormaPagoOv.Value);
                c.Saldo = _saldo;
                c.Save();
            }


            /*cCuota cuota = cCuota.GetFirstActiva(_idCC, hfFormaPagoOv.Value);
            _saldo = cuota.MontoAjustado - _montoAdelanto;*/

            cFormaPagoOV fp = cFormaPagoOV.Load(cuota.IdFormaPagoOV);

            //int _cantCuotas = fp.CantCuotas - cuotasAdelantadas.Count - (cuota.Nro - 1); //La última resta serían las cuotas pagas

            cCuota _lastCuotaAdelanto = cCuota.GetLastOrderDesc(_idCC, fp.Id);

            //int _cantCuotas = fp.CantCuotas - _lastCuotaAdelanto.Nro;

            

            if (cuota.Nro != 1)
                actualizarCuotas(_idCC, cuota.Nro, _lastCuotaAdelanto.Nro, fp.GetMoneda, _saldo, _montoAdelanto, cOperacionVenta.Load(fp.IdOperacionVenta), fp.IdOperacionVenta);
            else
                actualizarCuotas(_idCC, 0, _lastCuotaAdelanto.Nro, fp.GetMoneda, _saldo, _montoAdelanto, cOperacionVenta.Load(fp.IdOperacionVenta), fp.IdOperacionVenta);
            #endregion
        }

        public string GetIdCuota()
        {
            string _idCuota = null;
            foreach (ListViewItem item in lvCuotas.Items)
            {
                Label id = item.FindControl("lbId") as Label;
                _idCuota = id.Text;
            }

            return _idCuota;
        }

        public ArrayList GetIdCuotas()
        {
            ArrayList _idCuota = new ArrayList();
            foreach (ListViewItem item in lvCuotas.Items)
            {
                Label id = item.FindControl("lbId") as Label;
                _idCuota.Add(id.Text);
            }

            return _idCuota;
        }

        private void actualizarCuotas(string _idCC, int _nroCuota, int _cantCuota, string _moneda, decimal _saldo1, decimal _montoAdelanto, cOperacionVenta _idOV, string _idFormaPago)
        {
            decimal _totalComision = 0;
            decimal _vencimiento1 = 0;
            decimal _montoAjustado = 0;
            int _cuotasRestantes = (_cantCuota - _nroCuota) + 1;

            int index = 0; //La primer cuota solo se modifica el valor de la cuota que es igual al monto del adelanto

            #region Actualizo el resto de las cuotas
            foreach (cCuota c in cCuota.GetCuotasByNro(_idCC, _nroCuota, _cantCuota, _idFormaPago))
            {
                if (c.Estado != (Int16)estadoCuenta_Cuota.Anticipo)
                {
                    if (_moneda == tipoMoneda.Pesos.ToString())
                    {
                        decimal valorCuota = 0;

                        if (c.VariacionCAC != 0)
                            _montoAjustado = cCuota.CalcularSaldoByIndice(_saldo1, c.VariacionCAC);
                        else
                            _montoAjustado = _saldo1;

                        if (_cuotasRestantes != 0)
                            valorCuota = cCuota.CalcularCuota(_cuotasRestantes, _montoAjustado);
                        else
                            valorCuota = cCuota.CalcularCuota(1, _montoAjustado);

                        c.MontoAjustado = _montoAjustado;
                        _montoAjustado = _montoAjustado - valorCuota;
                        c.Saldo = _montoAjustado;


                        _totalComision = cCuota.CalcularComisionIva(valorCuota, c.Comision, _idOV.Iva);
                        _vencimiento1 = valorCuota + _totalComision;

                        c.TotalComision = _totalComision;
                        c.Vencimiento1 = _vencimiento1;

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
                        index++;
                    }
                }
            }
            #endregion
        }
    }
}