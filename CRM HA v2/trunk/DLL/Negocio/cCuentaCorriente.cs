using DLL.Base_de_Datos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace DLL.Negocio
{
    public enum estadoCuenta_Cuota { Pagado = 0, Activa = 1, Anulado = 2, Validar = 3, Anticipo = 4, Pendiente = 5 }
    public enum formaDePago { UnPago = 0, Cuotas = 1 };

    public class cCuentaCorriente
    {
        private string id;
        private string idEmpresa;
        private int cantCuotas;
        private decimal total;
        private decimal saldo;
        private decimal saldoPeso;
        private string formaPago;
        private string idIndiceCAC;
        private string idUnidad;
        private string unidadFuncional;
        private int idEstado;
        private decimal anticipo;
        private bool iva;
        private string idEmpresaUnidad;
        private string idOperacionVenta;
        private string monedaAcordada;
        private string textoAnulado;

        public cCuentaCorriente() { }

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string IdEmpresa
        {
            get { return idEmpresa; }
            set { idEmpresa = value; }
        }
        public string GetEmpresa
        {
            get { return cEmpresa.Load(IdEmpresa).GetNombreCompleto; }
        }
        public string GetEmpresaMail
        {
            get { return cEmpresa.Load(IdEmpresa).Mail; }
        }

        public int CantCuotas
        {
            get { return cantCuotas; }
            set { cantCuotas = value; }
        }
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }
        public string GetTotal
        {
            get
            { //return total.ToString("N2").Replace(".", ""); 
                return String.Format("{0:#,#0.00}", Total);
            }
        }
        public string GetTotalPesos
        {
            get
            {
                string total = null;
                if (monedaAcordada == Convert.ToString((Int16)tipoMoneda.Pesos))
                    total = String.Format("{0:#,#0.00}", Total);
                else
                {
                    if (IdOperacionVenta != "-1")
                    {
                        cOperacionVenta ov = cOperacionVenta.Load(IdOperacionVenta);
                        total = String.Format("{0:#,#0.00}", Total * ov.ValorDolar);
                    }
                    else
                    {
                        total = String.Format("{0:#,#0.00}", Total);
                    }
                }
                return total;
            }
        }
        public decimal Saldo
        {
            get { return saldo; }
            set { saldo = value; }
        }
        public string GetSaldo
        {
            get { return String.Format("{0:#,#0.00}", Saldo); }
        }

        public string GetSaldoPesos
        {
            get
            {
                if (IdOperacionVenta != "-1")
                {
                    /*cOperacionVenta ov = cOperacionVenta.Load(IdOperacionVenta);
                    string saldo = cFormaPagoOV.GetTotalSaldo(ov.Id, ov.MonedaAcordada, ov.ValorDolar);

                    if (ov.GetMoneda == tipoMoneda.Dolar.ToString())
                        saldo = String.Format("{0:#,#0.00}", Convert.ToDecimal(saldo) * ov.ValorDolar);

                    return saldo;*/

                    cOperacionVenta op = cOperacionVenta.Load(IdOperacionVenta);
                    List<cFormaPagoOV> saldos = cFormaPagoOV.GetFormaPagoOVByIdOV(op.Id);
                    decimal _saldoPesos = 0;
                    
                    foreach (cFormaPagoOV fp in saldos)
                    {
                        if (fp.GetMoneda == tipoMoneda.Pesos.ToString())
                        {
                            cCuota cuota_pendiente = cCuota.GetFirstPendiente(id, fp.Id);
                            if (cuota_pendiente != null)
                            {
                                _saldoPesos += cuota_pendiente.MontoAjustado;
                            }
                            else
                            {
                                cCuota cuota_activa = cCuota.GetFirstActiva(id, fp.Id);
                                if (cuota_activa != null)
                                {
                                    _saldoPesos += cuota_activa.MontoAjustado;
                                }
                                else
                                {
                                    cCuota cuota_pagada = cCuota.GetFirstPagada(id, fp.Id);
                                    if (cuota_pagada != null)
                                    {
                                        if (fp.CantCuotas > cuota_pagada.Nro)
                                        {
                                            _saldoPesos += cuota_pagada.MontoAjustado;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            cCuota cuota_pendiente = cCuota.GetFirstPendiente(id, fp.Id);
                            if (cuota_pendiente != null)
                            {
                                _saldoPesos += cValorDolar.ConvertToPeso(cuota_pendiente.MontoAjustado, cValorDolar.LoadActualValue());
                            }
                            else
                            {
                                cCuota cuota_activa = cCuota.GetFirstActiva(id, fp.Id);
                                if (cuota_activa != null)
                                {
                                    _saldoPesos += cValorDolar.ConvertToPeso(cuota_activa.MontoAjustado, cValorDolar.LoadActualValue());
                                }
                                else
                                {
                                    cCuota cuota_pagada = cCuota.GetFirstPagada(id, fp.Id);
                                    if (cuota_pagada != null)
                                    {
                                        if (fp.CantCuotas > cuota_pagada.Nro)
                                        {
                                            _saldoPesos += cValorDolar.ConvertToPeso(cuota_pagada.MontoAjustado, cValorDolar.LoadActualValue());
                                        }
                                    }
                                }
                            }
                        }


                        /*decimal saldoCuota = cCuota.SaldoCC(id, fp.Id, fp.Moneda);

                        //_saldoPesos = saldoCuota;
                        if (fp.GetMoneda == tipoMoneda.Pesos.ToString())
                        {
                            cCuota c = cCuota.GetFirst(id, fp.Id);

                            if (c != null)
                            {

                                if (fp.CantCuotas >= c.Nro + 1)
                                {
                                    cCuota c1 = cCuota.GetCuotaByNro(id, c.Nro + 1, fp.Id);

                                    if (c1 != null)
                                    {
                                        if (c1.Estado == (Int16)estadoCuenta_Cuota.Activa || c1.Estado == (Int16)estadoCuenta_Cuota.Pendiente)
                                            _saldoPesos += c1.MontoAjustado;
                                        else
                                            _saldoPesos = saldoCuota;
                                    }
                                }
                            }


                            //if (c != null)
                            //{
                            //    if (c.Estado == (Int16)estadoCuenta_Cuota.Activa)
                            //        _saldoPesos += saldoCuota;
                            //    else
                            //        _saldoPesos = saldoCuota;
                            //}
                        }
                        else
                        {
                            cCuota c = cCuota.GetFirst(id, fp.Id);
                            if (c != null)
                            {
                                if (c.Estado == (Int16)estadoCuenta_Cuota.Activa)
                                    _saldoPesos += cValorDolar.ConvertToPeso(saldoCuota, op.ValorDolar);
                                else
                                    _saldoPesos = cValorDolar.ConvertToPeso(saldoCuota, op.ValorDolar);
                            }
                        }

                        if (saldoCuota == 0)
                        {
                            cCuota c = cCuota.GetFirst(id, fp.Id);
                            if (c != null)
                            {
                                if (c.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                                {
                                    if (fp.GetMoneda == tipoMoneda.Dolar.ToString())
                                    {
                                        _saldoPesos += cValorDolar.ConvertToPeso(c.MontoAjustado, op.ValorDolar);
                                    }
                                    else
                                    {
                                        _saldoPesos += c.MontoAjustado;
                                    }
                                }
                            }
                        }*/
                    }

                    return String.Format("{0:#,#0.00}", _saldoPesos);
                }
                else
                    return "0";
            }
        }
        
        public decimal GetSaldoByFormaPago(string _idFp)
        {
            //if (IdOperacionVenta != "-1")
            //{
                //cOperacionVenta op = cOperacionVenta.Load(IdOperacionVenta);
                //cFormaPagoOV saldos = cFormaPagoOV.GetFormaPagoOVByIdOV(op.Id);
                decimal _totalSaldo = 0;

                cFormaPagoOV fp = cFormaPagoOV.Load(_idFp);
                //foreach (cFormaPagoOV fp in saldos)
                //{
                    decimal saldoCuota = cCuota.SaldoCC(id, fp.Id, fp.Moneda);
                    _totalSaldo += saldoCuota;

                    if (saldoCuota == 0)
                    {
                        cCuota c = cCuota.GetFirst(id, fp.Id);
                        if (c.Estado != (Int16)estadoCuenta_Cuota.Pagado)
                        {
                            _totalSaldo += c.MontoAjustado;                           
                        }
                    }
                //}

                return _totalSaldo;
            //}
            //else
            //    return "0";
            
        }

        public string GetDeudaPesos
        {
            get
            {
                if (IdOperacionVenta != "-1")
                {
                    cOperacionVenta op = cOperacionVenta.Load(IdOperacionVenta);
                    List<cFormaPagoOV> saldos = cFormaPagoOV.GetFormaPagoOVByIdOV(op.Id);
                    decimal _deudaPesos = 0;

                    foreach (cFormaPagoOV fp in saldos)
                    {
                        decimal saldoCuota = cCuota.DeudaCC(id, fp.Id, fp.Moneda);
                        if (fp.GetMoneda == tipoMoneda.Pesos.ToString())
                            _deudaPesos += saldoCuota;
                        else
                            _deudaPesos += cValorDolar.ConvertToPeso(saldoCuota, op.ValorDolar);
                    }

                    return String.Format("{0:#,#0.00}", _deudaPesos);
                }
                else
                    return "0";
            }
        }

        public decimal SaldoPeso
        {
            get { return saldoPeso; }
            set { saldoPeso = value; }
        }
        public string GetSaldoPeso
        {
            get { return String.Format("{0:#,#0.00}", SaldoPeso); }
        }

        public string FormaPago
        {
            get { return formaPago; }
            set { formaPago = value; }
        }
        public string GetFormaPago
        {
            get
            {
                string _formaPago = null;
                switch (FormaPago)
                {
                    case "0":
                        _formaPago = formaDePago.UnPago.ToString().Replace("UnPago", "1 Pago");
                        break;
                    case "1":
                        _formaPago = formaDePago.Cuotas.ToString();
                        break;
                }
                return _formaPago;
            }
        }
        public string IdIndiceCAC
        {
            get { return idIndiceCAC; }
            set { idIndiceCAC = value; }
        }
        public string IdUnidad
        {
            get { return idUnidad; }
            set { idUnidad = value; }
        }
        public string GetUnidad
        {
            get { return cUnidad.Load(IdUnidad).UnidadFuncional; }
        }

        public string GetProyecto
        {
            get
            {
                cEmpresaUnidad proyecto = cEmpresaUnidad.Load(IdEmpresaUnidad);
                return cProyecto.Load(proyecto.IdProyecto).Descripcion;
                /*string p = "";

                if (!string.IsNullOrEmpty(IdOperacionVenta))
                {                
                    int count = 1;
                    ArrayList proyectos = cProyecto.GetProyectoByIdOperacionVenta(IdOperacionVenta);

                    foreach (string proyecto in proyectos)
                    {
                        if (proyectos.Count == 1)
                            p = proyecto;
                        else
                        {
                            if (proyectos.Count != count)
                                p += proyecto + " <br/> ";
                            else
                                p += proyecto;

                            count++;
                        }
                    }
                }
                else
                {
                    cEmpresaUnidad proyecto = cEmpresaUnidad.Load(IdEmpresaUnidad);
                    p = cProyecto.Load(proyecto.IdProyecto).Descripcion; 
                }

                return p; */
            }
        }

        public string UnidadFuncional
        {
            get { return unidadFuncional; }
            set { unidadFuncional = value; }
        }
        public int IdEstado
        {
            get { return idEstado; }
            set { idEstado = value; }
        }
        public string GetEstado
        {
            get
            {
                string _estado = null;
                switch (IdEstado)
                {
                    case 0:
                        _estado = estadoCuenta_Cuota.Pagado.ToString();
                        break;
                    case 1:
                        _estado = estadoCuenta_Cuota.Activa.ToString();
                        break;
                    case 2:
                        _estado = estadoCuenta_Cuota.Anulado.ToString();
                        break;
                }
                return _estado;
            }
        }
        public decimal Anticipo
        {
            get { return anticipo; }
            set { anticipo = value; }
        }
        public string GetAnticipo
        {
            get { return String.Format("{0:#,#0.00}", Anticipo); }
        }

        public bool Iva
        {
            get { return iva; }
            set { iva = value; }
        }
        public string IdEmpresaUnidad
        {
            get { return idEmpresaUnidad; }
            set { idEmpresaUnidad = value; }
        }
        public string IdOperacionVenta
        {
            get { return idOperacionVenta; }
            set { idOperacionVenta = value; }
        }
        public string MonedaAcordada
        {
            get { return monedaAcordada; }
            set { monedaAcordada = value; }
        }
        public string GetMoneda
        {
            get
            {
                string moneda = null;
                switch (MonedaAcordada)
                {
                    case "0":
                        moneda = tipoMoneda.Dolar.ToString();
                        break;
                    case "1":
                        moneda = tipoMoneda.Pesos.ToString();
                        break;
                }
                return moneda;
            }
        }

        public string TextoAnulado
        {
            get { return textoAnulado; }
            set { textoAnulado = value; }
        }
        #endregion

        #region Acceso a Datos
        public static cCuentaCorriente Load(string id)
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.Load(id);
        }
        public int Save()
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.Save(this);
        }

        public static List<cCuentaCorriente> GetCuentaCorriente(string _idEmpresa, Int16 _estado, string _obra, Int16 _moneda)
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.GetCuentaCorriente(_idEmpresa, _estado, _obra, _moneda);
        }

        public static cCuentaCorriente GetCuentaCorrienteById(string _idEmpresa)
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.GetCuentaCorrienteById(_idEmpresa);
        }

        public static cCuentaCorriente GetCuentaCorrienteByIdEmpresaUnidad(string _idEmpresaUnidad)
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.GetCuentaCorrienteByIdEmpresaUnidad(_idEmpresaUnidad);
        }

        public static List<cCuentaCorriente> GetCuentaCorrienteByIdCliente(string _idEmpresa, Int16 _estado)
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.GetCuentaCorrienteByIdCliente(_idEmpresa, _estado);
        }

        public static cCuentaCorriente GetCuentaCorrienteByIdOv(string _idOv)
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.GetCuentaCorrienteByIdOv(_idOv);
        }
        #endregion

        #region Combos
        public static ListItemCollection CargarEstadoCuenta_Cuota()
        {
            ListItemCollection collection = new ListItemCollection();
            string[] unidades = Enum.GetNames(typeof(estadoCuenta_Cuota));

            foreach (string item in unidades)
            {
                int value = (int)Enum.Parse(typeof(estadoCuenta_Cuota), item);

                if (value != (Int16)estadoCuenta_Cuota.Anulado && value != (Int16)estadoCuenta_Cuota.Validar)
                {
                    if (value != (Int16)estadoCuenta_Cuota.Anticipo && value != (Int16)estadoCuenta_Cuota.Pendiente)
                        collection.Add(new ListItem(item.Replace("_", " "), value.ToString()));
                }
            }

            return collection;
        }
        #endregion

        public static DataTable GetTotalACobrarPorProyecto()
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.GetTotalACobrarPorProyecto();
        }

        public static DataTable GetTotalACobrarPorCliente()
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.GetTotalACobrarPorCliente();
        }

        public static string GetTotalMontoAcordado(List<cCuentaCorriente> cc)
        {
            decimal sum = 0;
            if (cc != null)
            {
                foreach (cCuentaCorriente o in cc)
                {
                    decimal total = Convert.ToDecimal(o.Total);
                    if (cAuxiliar.IsNumeric(total.ToString()) && !string.IsNullOrEmpty(total.ToString()))
                    {
                        if (o.IdOperacionVenta != "-1")
                        {
                            cOperacionVenta ov = cOperacionVenta.Load(o.IdOperacionVenta);
                            sum += Convert.ToDecimal(o.Total * ov.ValorDolar);
                        }
                        else
                        {
                            sum += Convert.ToDecimal(o.Total);
                        }
                    }
                }
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public static string GetTotalSaldoDolar(List<cCuentaCorriente> cc)
        {
            decimal sum = 0;
            if (cc != null)
            {
                foreach (cCuentaCorriente o in cc)
                {
                    decimal saldo = Convert.ToDecimal(o.Saldo);
                    if (cAuxiliar.IsNumeric(saldo.ToString()) && !string.IsNullOrEmpty(saldo.ToString()))
                    {
                        if (o.IdOperacionVenta != "-1")
                        {
                            cOperacionVenta ov = cOperacionVenta.Load(o.IdOperacionVenta);
                            sum += Convert.ToDecimal(o.Saldo * ov.ValorDolar);
                        }
                        else
                        {
                            sum += Convert.ToDecimal(o.Saldo);
                        }
                    }
                }
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public static string GetTotalSaldoPesos(List<cCuentaCorriente> cc)
        {
            decimal sum = 0;
            if (cc != null)
            {
                foreach (cCuentaCorriente o in cc)
                {
                    decimal saldo = Convert.ToDecimal(o.SaldoPeso);
                    if (cAuxiliar.IsNumeric(saldo.ToString()) && !string.IsNullOrEmpty(saldo.ToString()))
                        sum += Convert.ToDecimal(o.SaldoPeso);
                }
            }
            return String.Format("{0:#,#0.00}", sum);
        }

        public static cCuentaCorriente GetCCByIdUnidad(string _idUnidad, string _idEmpresaUnidad)
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.GetCCByIdUnidad(_idUnidad, _idEmpresaUnidad);
        }

        public static List<cCuentaCorriente> GetCuotasMesProyectoPendientes(string _idProyecto, Int16 _tipoMoneda)
        {
            cCuentaCorrienteDAO DAO = new cCuentaCorrienteDAO();
            return DAO.GetCuotasMesProyectoPendientes(_idProyecto, _tipoMoneda);
        }
    }
}
