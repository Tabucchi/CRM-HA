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
    public class cUVA
    {
        private string id;
        private decimal valor;
        private DateTime fecha;
        private Int16 _papelera;

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public string GetRegisterDate
        {
            get { return String.Format("{0:dd/MM/yyyy}", Fecha); }
        }

        public Int16 Papelera
        {
            get { return _papelera; }
            set { _papelera = value; }
        }
        #endregion

        public cUVA() { }

        public cUVA(decimal _valor, papelera papeleraId)
        {
            valor = _valor;
            fecha = DateTime.Now;
            _papelera = Convert.ToInt16(papeleraId);
        }

        #region Acceso a Datos
        public int Save()
        {
            cUVADAO DAO = new cUVADAO();
            return DAO.Save(this);
        }

        public static cUVA Load(string id)
        {
            cUVADAO DAO = new cUVADAO();
            return DAO.Load(id);
        }

        public static ArrayList LoadTable()
        {
            cUVADAO DAO = new cUVADAO();
            return DAO.LoadTable();
        }
        #endregion

        public static DataTable GetDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("id", typeof(string)));
            tbl.Columns.Add(new DataColumn("valor", typeof(string)));
            ArrayList valores = LoadTable();
            valores.Reverse();
            int count = 0;
            foreach (cUVA u in valores)
            {
                if (count != 20)
                {
                    string descripcion = String.Format("{0:MMMM yyyy}", u.Fecha) + " - " + u.Valor;
                    tbl.Rows.Add(u.Id, descripcion);
                    count++;
                }
            }
            return tbl;
        }

        public static decimal GetLastIdIndice()
        {
            cUVADAO DAO = new cUVADAO();
            return DAO.GetLastIdIndice();
        }

        public static decimal GetLastValorIndice()
        {
            cUVADAO DAO = new cUVADAO();
            return DAO.GetLastValorIndice();
        }

        public static List<cUVA> GetIndiceUVA()
        {
            cUVADAO DAO = new cUVADAO();
            return DAO.GetIndiceUVA();
        }

        public static void ActualizarIndiceUVACuotas(string _nuevoIndice, DateTime fecha, string indice_anterior)
        {
            try
            {
                decimal valorUvaBoleto = 0;
                //List<cCuota> cuotas = cCuota.GetCuotasActivaByFechaConUVA(fecha);
                List<cCuota> cuotas = cCuota.GetCuotasActivaUVA();

                foreach (cCuota c in cuotas)
                {
                    cCuentaCorriente cc = cCuentaCorriente.Load(c.IdCuentaCorriente);
                    decimal interes = 0;
                        
                    if (c.Nro == 1)
                    {
                        cOperacionVenta op = cOperacionVenta.GetOperacionByFormaPago(c.IdFormaPagoOV);
                        valorUvaBoleto = op.ValorBaseUVA;
                    }

                    string monedaCuota = null;
                    cFormaPagoOV formaPago = cFormaPagoOV.Load(c.IdFormaPagoOV);
                    if (c.IdFormaPagoOV == "-1")
                        monedaCuota = cc.GetMoneda;
                    else
                        monedaCuota = formaPago.GetMoneda;

                    if (monedaCuota == tipoMoneda.Pesos.ToString())
                    {
                        decimal vUVA = 0;
                                
                        if (c.Nro == 1)
                            vUVA = cCuota.CalcularVariacionMensualUVA(indice_anterior.ToString(), _nuevoIndice, valorUvaBoleto);
                        else
                            vUVA = cCuota.CalcularVariacionMensualUVA(indice_anterior.ToString(), _nuevoIndice, 0);

                        //Si el indice CAC es menor respecto al mes anterior, el resultado es 0.
                        if (cUVA.Load(indice_anterior).Valor > cUVA.Load(_nuevoIndice).Valor)
                        {
                            c.VariacionUVA = 0;
                            vUVA = 0;
                        }
                        else
                            c.VariacionUVA = vUVA;

                        decimal _saldo = cc.Saldo;

                        if (c.Nro == 1)
                            _saldo = cCuota.CalcularSaldoByIndice(formaPago.Saldo, vUVA);
                        else
                        {
                            int _saldoAnterior = c.Nro - 1;
                            _saldo = cCuota.CalcularSaldoByIndice(cCuota.GetCuotaByNro(cc.Id, _saldoAnterior, c.IdFormaPagoOV).Saldo, vUVA);
                        }

                        if (formaPago.InteresAnual != 0)
                        {
                            interes = Convert.ToDecimal(formaPago.InteresAnual) / 12;
                            interes = interes + 100;
                            _saldo = (_saldo * interes) / 100;
                        }

                        decimal valorCuota = 0;
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
                                    int _cantCuota = (fp.CantCuotas - c.Nro) + 1;
                                    _cantCuota = _cantCuota - _cantAnticipo;

                                    valorCuota = c.Nro != 1 ? cCuota.CalcularCuota(_cantCuota, _saldo) : cCuota.CalcularCuota(fp.CantCuotas, _saldo);

                                    actualizarCuotas(cc.Id, c.Nro, fp.CantCuotas, _cantAnticipo, tipoMoneda.Pesos.ToString(), _saldo, ov);
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

                    Thread.Sleep(500);                       
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
                            decimal _saldo = cc.Saldo;

                            if (c.Nro == 1)
                                _saldo = cCuota.CalcularSaldoByIndice(fp.Valor, c.VariacionUVA);
                            else
                            {
                                int _saldoAnterior = c.Nro - 1;
                                _saldo = cCuota.CalcularSaldoByIndice(cCuota.GetCuotaByNro(cc.Id, _saldoAnterior, c.IdFormaPagoOV).Saldo, c.VariacionUVA);
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
                                _saldo = cCuota.CalcularSaldoByIndice(fp.Valor, c.VariacionUVA);
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

        public static string GetIdIndiceByFecha(DateTime fecha)
        {
            cUVADAO DAO = new cUVADAO();
            return DAO.GetIdIndiceByFecha(fecha);
        }

        public static decimal GetIndiceByFecha(DateTime fecha)
        {
            cUVADAO DAO = new cUVADAO();
            return DAO.GetIndiceByFecha(fecha);
        }
    }
}
