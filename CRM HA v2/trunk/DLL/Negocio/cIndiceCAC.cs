using DLL.Base_de_Datos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;

namespace DLL.Negocio
{
    public class cIndiceCAC
    {
        private string id;
        private DateTime fecha;
        private decimal valor;

        public cIndiceCAC() { }

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }
        #endregion

        #region Acceso a Datos
        public int Save()
        {
            cIndiceCACDAO DAO = new cIndiceCACDAO();
            return DAO.Save(this);
        }
        public static cIndiceCAC Load(string id)
        {
            cIndiceCACDAO DAO = new cIndiceCACDAO();
            return DAO.Load(id);
        }
        public static ArrayList LoadTable()
        {
            cIndiceCACDAO DAO = new cIndiceCACDAO();
            return DAO.LoadTable();
        }
        public static List<cIndiceCAC> GetIndiceCAC()
        {
            cIndiceCACDAO DAO = new cIndiceCACDAO();
            return DAO.GetIndiceCAC();
        }
        public static string GetLastIndiceMonth()
        {
            cIndiceCACDAO DAO = new cIndiceCACDAO();
            return DAO.GetLastIndiceMonth();
        }
        public static decimal GetLastIndice()
        {
            cIndiceCACDAO DAO = new cIndiceCACDAO();
            return DAO.GetLastIndice();
        }

        public static decimal GetLastValueIndice()
        {
            cIndiceCACDAO DAO = new cIndiceCACDAO();
            return DAO.GetLastValueIndice();
        }

        public static decimal GetIndiceByFecha(DateTime fecha)
        {
            cIndiceCACDAO DAO = new cIndiceCACDAO();
            return DAO.GetIndiceByFecha(fecha);
        }

        #endregion

        public static DataTable GetDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("id", typeof(string)));
            tbl.Columns.Add(new DataColumn("descripcion", typeof(string)));
            ArrayList valores = LoadTable();
            valores.Reverse();
            int count = 0;
            foreach (cIndiceCAC cg in valores)
            {
                if (count != 20)
                {
                    string descripcion = String.Format("{0:MMMM yyyy}", cg.Fecha) + " - " + cg.Valor;
                    tbl.Rows.Add(cg.Id, descripcion);
                    count++;
                }
            }
            return tbl;
        }

        public static void ActualizarIndiceCACCuotas(string _nuevoIndice, DateTime fecha, string indice_anterior)
        {
            try
            {
                string aux = null;
                List<cCuota> cuotas = cCuota.GetCuotasActivaByFecha(fecha);

                foreach (cCuota c in cuotas)
                {
                    if (aux != c.IdCuentaCorriente)
                    {
                        cCuentaCorriente cc = cCuentaCorriente.Load(c.IdCuentaCorriente);
                        decimal interes = 0;

                        if (cc.Id != "10278" && cc.Id != "10542")
                        {
                            string monedaCuota = null;
                            cFormaPagoOV formaPago = cFormaPagoOV.Load(c.IdFormaPagoOV);
                            if (c.IdFormaPagoOV == "-1")
                                monedaCuota = cc.GetMoneda;
                            else
                                monedaCuota = formaPago.GetMoneda;

                            if (monedaCuota == tipoMoneda.Pesos.ToString())
                            {
                                string indiceBase = cc.IdIndiceCAC;
                                
                                //MODIFICAR
                                //decimal vCAC = cCuota.CalcularVariacionMensualCAC(cCuentaCorriente.Load(c.IdCuentaCorriente).IdIndiceCAC, _nuevoIndice.ToString());
                                decimal vCAC = cCuota.CalcularVariacionMensualCAC(indice_anterior.ToString(), _nuevoIndice, true);
                                
                                //Si el indice CAC es menor respecto al mes anterior, el resultado es 0.
                                if (cIndiceCAC.Load(indice_anterior).Valor > cIndiceCAC.Load(_nuevoIndice).Valor)
                                {
                                    c.VariacionCAC = 0;
                                    vCAC = 0;
                                }
                                else
                                    c.VariacionCAC = vCAC;

                                decimal _saldo = cc.Saldo;

                                if (c.Nro == 1)
                                    _saldo = cCuota.CalcularSaldoByIndice(formaPago.Valor, vCAC);
                                else
                                {
                                    int _saldoAnterior = c.Nro - 1;
                                    _saldo = cCuota.CalcularSaldoByIndice(cCuota.GetCuotaByNro(cc.Id, _saldoAnterior, c.IdFormaPagoOV).Saldo, vCAC);
                                }
                                                                
                                if (formaPago.InteresAnual != 0)
                                {
                                    interes = Convert.ToDecimal(formaPago.InteresAnual) / 12;
                                    interes = interes + 100;
                                    _saldo = (_saldo * interes) / 100;
                                }

                                decimal valorCuota = 0;
                                /*if (cc.CantCuotas != 0)
                                    valorCuota = c.Nro != 1 ? cCuota.CalcularCuota(cc.CantCuotas - c.Nro + 1, _saldo) : cCuota.CalcularCuota(fp.CantCuotas, _saldo);
                                else
                                {*/
                                cOperacionVenta ov = cOperacionVenta.Load(cc.IdOperacionVenta);

                                List<cFormaPagoOV> fps = cFormaPagoOV.GetFormaPagoOVByIdOV(ov.Id);
                                foreach (cFormaPagoOV f in fps)
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
                                //}

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
                            Thread.Sleep(500);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void ActualizarCuotasDeCC(string _idCC, string _idFp, Int16 _nro)
        {
            try
            {
                string aux = null;
                List<cCuota> cuotas = cCuota.GetCuotasActivas(_idCC, _idFp);

                foreach (cCuota c in cuotas)
                {
                    cCuentaCorriente cc = cCuentaCorriente.Load(c.IdCuentaCorriente);
                    cFormaPagoOV fp = cFormaPagoOV.Load(c.IdFormaPagoOV);

                    if (c.Nro >= _nro)
                    {
                        string monedaCuota = null;
                        if (c.IdFormaPagoOV == "-1")
                            monedaCuota = cc.GetMoneda;
                        else
                            monedaCuota = fp.GetMoneda;

                        if (monedaCuota == tipoMoneda.Pesos.ToString())
                        {
                            string indiceBase = cc.IdIndiceCAC;

                            decimal _saldo = cc.Saldo;

                            if (c.Nro == 1)
                                _saldo = cCuota.CalcularSaldoByIndice(fp.Valor, c.VariacionCAC);
                            else
                            {
                                int _saldoAnterior = c.Nro - 1;
                                _saldo = cCuota.CalcularSaldoByIndice(cCuota.GetCuotaByNro(cc.Id, _saldoAnterior, c.IdFormaPagoOV).Saldo, c.VariacionCAC);
                            }

                            decimal interes = 0;
                            if (fp.InteresAnual != 0)
                            {
                                interes = Convert.ToDecimal(fp.InteresAnual) / 12;
                                interes = interes + 100;
                                _saldo = (_saldo * interes) / 100;
                            }

                            decimal valorCuota = 0;
                            cOperacionVenta ov = cOperacionVenta.Load(cc.IdOperacionVenta);
                            List<cFormaPagoOV> fps = cFormaPagoOV.GetFormaPagoOVByIdOV(ov.Id);
                            foreach (cFormaPagoOV f in fps)
                            {
                                if (f.Id == _idFp)
                                {
                                    if (c.IdFormaPagoOV == f.Id)
                                    {
                                        if (f.GetMoneda == tipoMoneda.Pesos.ToString())
                                        {
                                            cFormaPagoOV fpov = cFormaPagoOV.Load(f.Id);

                                            int _cantAnticipo = cCuota.GetCuotasAnticipos(cc.Id, fpov.Id).Count;
                                            int _cantCuota = (fpov.CantCuotas - c.Nro) + 1;
                                            _cantCuota = _cantCuota - _cantAnticipo;

                                            valorCuota = c.Nro != 1 ? cCuota.CalcularCuota(_cantCuota, _saldo) : cCuota.CalcularCuota(fpov.CantCuotas, _saldo);

                                            actualizarCuotas(cc.Id, c.Nro, fpov.CantCuotas, _cantAnticipo, tipoMoneda.Pesos.ToString(), _saldo, ov);
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
                        else
                        {
                            decimal _saldo = 0;

                            if (c.Nro == 1)
                                _saldo = cCuota.CalcularSaldoByIndice(fp.Valor, c.VariacionCAC);
                            else
                                _saldo = cCuota.GetCuotaByNro(cc.Id, c.Nro - 1, c.IdFormaPagoOV).Saldo;
                            
                            decimal interes = 0;
                            if (fp.InteresAnual != 0)
                            {
                                interes = Convert.ToDecimal(fp.InteresAnual) / 12;
                                interes = interes + 100;
                                _saldo = (_saldo * interes) / 100;
                            }

                            decimal valorCuota = 0;
                            cOperacionVenta ov = cOperacionVenta.Load(cc.IdOperacionVenta);
                            List<cFormaPagoOV> fps = cFormaPagoOV.GetFormaPagoOVByIdOV(ov.Id);
                            foreach (cFormaPagoOV f in fps)
                            {
                                if (f.Id == _idFp)
                                {
                                    if (c.IdFormaPagoOV == f.Id)
                                    {
                                        if (f.GetMoneda == tipoMoneda.Pesos.ToString())
                                        {
                                            cFormaPagoOV fpov = cFormaPagoOV.Load(f.Id);

                                            int _cantAnticipo = cCuota.GetCuotasAnticipos(cc.Id, fpov.Id).Count;
                                            int _cantCuota = (fpov.CantCuotas - c.Nro) + 1;
                                            _cantCuota = _cantCuota - _cantAnticipo;

                                            valorCuota = c.Nro != 1 ? cCuota.CalcularCuota(_cantCuota, _saldo) : cCuota.CalcularCuota(fpov.CantCuotas, _saldo);

                                            actualizarCuotas(cc.Id, c.Nro, fpov.CantCuotas, _cantAnticipo, tipoMoneda.Pesos.ToString(), _saldo, ov);
                                        }
                                        else
                                        {
                                            cFormaPagoOV fpov = cFormaPagoOV.Load(f.Id);

                                            int _cantAnticipo = cCuota.GetCuotasAnticipos(cc.Id, fpov.Id).Count;
                                            int _cantCuota = (fpov.CantCuotas - c.Nro) + 1;
                                            _cantCuota = _cantCuota - _cantAnticipo;

                                            valorCuota = c.Nro != 1 ? cCuota.CalcularCuota(_cantCuota, _saldo) : cCuota.CalcularCuota(fpov.CantCuotas, _saldo);

                                            actualizarCuotas(cc.Id, c.Nro, fpov.CantCuotas, _cantAnticipo, tipoMoneda.Dolar.ToString(), _saldo, ov);
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
                        else
                        {
                            decimal valorCuota = 0;

                            if (_cuotasRestantes != 0)
                                valorCuota = cCuota.CalcularCuota(_cuotasRestantes, _saldo1);
                            else
                                valorCuota = cCuota.CalcularCuota(1, _saldo1);

                            _totalComision = cCuota.CalcularComisionIva(valorCuota, c.Comision, _idOV.Iva);

                            _vencimiento1 = valorCuota + _totalComision;

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
    }
}

