using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLL.Base_de_Datos;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace DLL.Negocio
{
    public class cCuota
    {
        private string id;
        private string idCuentaCorriente;
        private Int16 nro;
        private decimal monto;
        private decimal montoAjustado;
        private decimal vencimiento1;
        private decimal vencimiento2;
        private decimal variacionCAC;
        private decimal variacionUVA;
        private decimal comision;
        private int estado;
        private DateTime fecha;
        private DateTime fechaVencimiento1;
        private DateTime fechaVencimiento2;
        private decimal saldo;
        private decimal montoPago; //es el monto que se pago
        private decimal totalComision;
        private string idRegistroPago;
        private string idFormaPagoOV;
        private bool ajusteCAC;

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string IdCuentaCorriente
        {
            get { return idCuentaCorriente; }
            set { idCuentaCorriente = value; }
        }

        public string GetIdEmpresa
        {
            get { return cCuentaCorriente.Load(IdCuentaCorriente).IdEmpresa; }
        }
        public string GetEmpresa
        {
            get { return cCuentaCorriente.Load(IdCuentaCorriente).GetEmpresa; }
        }
        public string GetMail
        {
            get { return cCuentaCorriente.Load(IdCuentaCorriente).GetEmpresaMail; }
        }

        public Int16 Nro
        {
            get { return nro; }
            set { nro = value; }
        }
        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }
        public string GetMonto1
        {
            get
            { return String.Format("{0:#,#0.00}", Monto); }
        }

        public decimal MontoAjustado
        {
            get { return montoAjustado; }
            set { montoAjustado = value; }
        }
        public string GetMontoAjustado
        {
            get
            { return String.Format("{0:#,#0.00}", MontoAjustado); }
        }
        public decimal Vencimiento1
        {
            get { return vencimiento1; }
            set { vencimiento1 = value; }
        }
        public string GetVencimiento1
        {
            get
            { return String.Format("{0:#,#0.00}", Vencimiento1); }
        }
        public decimal VariacionUVA
        {
            get { return variacionUVA; }
            set { variacionUVA = value; }
        }
        public string GetVariacionUVA
        {
            get
            { return String.Format("{0:#,#0.00}", variacionUVA); }
        }

        public decimal Vencimiento2
        {
            get { return vencimiento2; }
            set { vencimiento2 = value; }
        }
        public string GetVencimiento2
        {
            get
            { return String.Format("{0:#,#0.00}", Vencimiento2); }
        }
        public decimal VariacionCAC
        {
            get { return variacionCAC; }
            set { variacionCAC = value; }
        }

        public string GetVariacionCAC
        {
            get
            { return String.Format("{0:#,#0.00}", variacionCAC); }
        }

        public string GetTipoIndice
        {
            get
            {
                cOperacionVenta ov = cOperacionVenta.GetOperacionByFormaPago(IdFormaPagoOV);
                string indice = null;

                if (ov.Cac == true)
                    indice = eIndice.CAC.ToString();

                if (ov.Uva == true)
                    indice = eIndice.UVA.ToString();

                if (ov.Cac == false && ov.Uva == false)
                    indice = eIndice.CAC.ToString();

                return indice;
            }
        }

        public string GetIndice
        {
            get{
                cOperacionVenta ov = cOperacionVenta.GetOperacionByFormaPago(IdFormaPagoOV);
                string indice = "0";

                if (ov.Cac == true)
                    indice = String.Format("{0:#,#0.00}", VariacionCAC);

                if (ov.Uva == true)
                    indice = String.Format("{0:#,#0.00}", variacionUVA);
                
                return indice;
            }
        }

        public decimal Comision
        {
            get { return comision; }
            set { comision = value; }
        }

        /*public decimal GetTotalComision
        {
            get {
                decimal aux1 = (monto * comision) / 100;
                decimal aux = aux1 * Convert.ToDecimal(1.21);
                return Math.Round(aux, 2);         
            }
        }*/
        public decimal TotalComision
        {
            get { return totalComision; }
            set { totalComision = value; }
        }
        public string GetTotalComision
        {
            get
            { return String.Format("{0:#,#0.00}", TotalComision); }
        }

        public int Estado
        {
            get { return estado; }
            set { estado = value; }
        }
        public string GetEstado
        {
            get
            {
                string estado = null;
                switch (Estado)
                {
                    case 0:
                        estado = estadoCuenta_Cuota.Pagado.ToString();
                        break;
                    case 1:
                        estado = estadoCuenta_Cuota.Activa.ToString();
                        break;
                    case 4:
                        estado = estadoCuenta_Cuota.Anticipo.ToString();
                        break;
                    case 5:
                        estado = estadoCuenta_Cuota.Pendiente.ToString();
                        break;
                }
                return estado;
            }
        }

        public string GetMoneda
        {
            get
            {
                return cFormaPagoOV.Load(IdFormaPagoOV).GetMoneda;
            }
        }

        public string GetSaldoPendiente
        {
            get
            {
                cCuentaCorriente cc = cCuentaCorriente.Load(IdCuentaCorriente);

                decimal _saldo = cc.Saldo;
                foreach (cCuota cuota in cCuota.GetCuotas(cc.Id))
                {
                    if (Nro > cuota.Nro)
                        _saldo = _saldo - cuota.Monto;
                }

                return _saldo.ToString();
            }
        }

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public DateTime FechaVencimiento1
        {
            get { return fechaVencimiento1; }
            set { fechaVencimiento1 = value; }
        }
        public string GetFechaVencimiento1
        {
            get
            { return FechaVencimiento1.ToString("dd/MM/yyyy"); }
        }

        public DateTime FechaVencimiento2
        {
            get { return fechaVencimiento2; }
            set { fechaVencimiento2 = value; }
        }
        public string GetFechaVencimiento2
        {
            get
            { return FechaVencimiento2.ToString("dd/MM/yyyy"); }
        }
        public string GetRecibo
        {
            get
            {
                string nro = cReciboCuota.GetReciboByIdCuota(Id);
                return nro == null ? "-" : cAuxiliar.AgregarCeroRecibo(nro);
            }
        }
        public string GetReciboSinCero
        {
            get
            {
                string nro = cReciboCuota.GetReciboByIdCuota(Id);
                return nro == null ? "-" : nro;
            }
        }
        public decimal Saldo
        {
            get { return saldo; }
            set { saldo = value; }
        }
        public string GetSaldo
        {
            get
            { return String.Format("{0:#,#0.00}", Saldo); }
        }

        public string GetSaldoResumen
        {
            get
            { 
                if(Saldo != 0)
                    return String.Format("{0:#,#0.00}", Saldo); 
                else
                    return String.Format("{0:#,#0.00}", MontoAjustado); 
            }
        }


        //public string SaldoAnteriorByIndiceCAC
        //{

        //}

        public string ValidarIndice
        {
            get
            {
                decimal variacion = 0;

                if (Nro - 1 != 0)
                {
                    decimal _saldo = cCuota.GetCuotaByNro(IdCuentaCorriente, Nro - 1, IdFormaPagoOV).Saldo;
                    if (_saldo != 0)
                        variacion = (MontoAjustado * 100) / _saldo;
                }
                else
                {
                    decimal _saldo = cFormaPagoOV.Load(IdFormaPagoOV).Monto;
                    if (_saldo != 0)
                        variacion = (MontoAjustado * 100) / _saldo;
                }

                return String.Format("{0:#,#0.00}", variacion - 100);
            }
        }

        public string DiferenciaSaldosCAC
        {
            get
            {
                return String.Format("{0:#,#0.00}", Convert.ToDecimal(GetSaldoResumen) - Convert.ToDecimal(SaldoAnteriorByIndiceCAC));
            }
        }

        public string DiferenciaSaldosUVA
        {
            get{
                return String.Format("{0:#,#0.00}", Convert.ToDecimal(GetSaldoResumen) - Convert.ToDecimal(SaldoAnteriorByIndiceUVA));
            }
        }

        public string SaldoAnteriorByIndiceCAC
        {
            get{
                decimal montoAjustado = 0;

                if (Saldo != 0)
                {
                    if (VariacionCAC != 0)
                        montoAjustado = (Saldo * 100) / (Math.Round(VariacionCAC, 2) + 100);
                    else
                        montoAjustado = Saldo;

                    montoAjustado = Math.Round(montoAjustado, 5);
                }
                else
                {
                    if (VariacionCAC != 0)
                        montoAjustado = (MontoAjustado * 100) / (Math.Round(VariacionCAC, 2) + 100);
                    else
                        montoAjustado = MontoAjustado;

                    montoAjustado = Math.Round(montoAjustado, 5);
                }
                
                return String.Format("{0:#,#0.00}", montoAjustado);
            }
        }

        public string SaldoAnteriorByIndiceUVA
        {
            get
            {
                decimal montoAjustado = 0;

                if (Saldo != 0)
                {
                    if (VariacionUVA != 0)
                        montoAjustado = (Saldo * 100) / (Math.Round(VariacionUVA, 2) + 100);
                    else
                        montoAjustado = Saldo;

                    montoAjustado = Math.Round(montoAjustado, 5);
                }
                else
                {
                    if (VariacionUVA != 0)
                        montoAjustado = (MontoAjustado * 100) / (Math.Round(VariacionCAC, 2) + 100);
                    else
                        montoAjustado = MontoAjustado;

                    montoAjustado = Math.Round(montoAjustado, 5);
                }

                return String.Format("{0:#,#0.00}", montoAjustado);
            }
        }

        public decimal MontoPago
        {
            get { return montoPago; }
            set { montoPago = value; }
        }

        public string IdRegistroPago
        {
            get { return idRegistroPago; }
            set { idRegistroPago = value; }
        }
        public string IdFormaPagoOV
        {
            get { return idFormaPagoOV; }
            set { idFormaPagoOV = value; }
        }

        public string GetFechaPago
        {
            get
            {
                if (IdRegistroPago != "-1")
                    return String.Format("{0:dd/MM/yyyy}", cRegistroPago.Load(IdRegistroPago).FechaPago);
                else
                    return "-";
            }
        }

        public string GetMonto
        {
            get
            {
                if (IdRegistroPago != "-1")
                    return String.Format("{0:#,#0.00}", cRegistroPago.Load(IdRegistroPago).Monto);
                else
                    return "-";
            }
        }
        public string GetNroTransaccion
        {
            get
            {
                if (IdRegistroPago != "-1")
                    return cRegistroPago.Load(IdRegistroPago).Transaccion.ToString();
                else
                    return "-";
            }
        }

        public string GetSucursal
        {
            get
            {
                if (IdRegistroPago != "-1")
                    return cRegistroPago.Load(IdRegistroPago).Sucursal.ToString();
                else
                    return "-";
            }
        }
        public bool AjusteCAC
        {
            get { return ajusteCAC; }
            set { ajusteCAC = value; }
        }

        public List<cReciboCuota> GetRecibos
        {
            get
            {
                return cReciboCuota.GetRecibosByNroCuota(IdCuentaCorriente, Nro.ToString(), idFormaPagoOV);
            }
        }
        #endregion

        #region Acceso a Datos
        public int Save()
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.Save(this);
        }

        public static cCuota Load(string id)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.Load(id);
        }

        public static List<cCuota> GetCuotas(string _idCuentaCorriente)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotas(_idCuentaCorriente);
        }

        public static List<cCuota> GetCuotasPendientes(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasPendientes(_idCC, _idFormaPagoOV);
        }

        public static List<cCuota> GetCuotasPendientes2(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasPendientes2(_idCC, _idFormaPagoOV);
        }

        public static List<cCuota> GetCuotasPendientesByIdCC(string _idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasPendientesByIdCC(_idCC);
        }

        public static List<cCuota> GetCuotasPendientesByIdCC(string _idCuentaCorriente, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasPendientesByIdCC(_idCuentaCorriente, _idFormaPagoOV);

        }
        //public static List<cCuota> GetCuotasPendientes(string _idOV, DateTime fechaHoy, bool _existCuota)
        public static List<cCuota> GetCuotasPendientes(string _idEmpresa, DateTime fechaHoy)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasPendientes(_idEmpresa, fechaHoy);
            //return DAO.GetCuotasPendientes(_idOV, fechaHoy, _existCuota);
        }
        
        public static List<cCuota> GetCuotasPendientes(string _idEmpresa, DateTime fechaHoy, Int16 indice)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasPendientes(_idEmpresa, fechaHoy, indice);
        }

        public static List<cCuota> GetCuotasPendientesSoloCuotasActivas(string _idEmpresa, DateTime fechaHoy, Int16 indice)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasPendientesSoloCuotasActivas(_idEmpresa, fechaHoy, indice);
        }

        public static List<cCuota> GetCuotasActivas(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivas(_idCC, _idFormaPagoOV);
        }

        public static List<cCuota> GetCuotasAnticipos(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasAnticipos(_idCC, _idFormaPagoOV);
        }

        public static List<cCuota> GetCuotasActivasByIdCC(string _idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivasByIdCC(_idCC);
        }

        public static List<cCuota> GetCuotasActivasAndPendientesByIdCC(string _idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivasAndPendientesByIdCC(_idCC);
        }

        public static List<cCuota> GetCuotasActivasDESC(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivasDESC(_idCC, _idFormaPagoOV);
        }

        public static List<cCuota> GetCuotasByIdFormaPagoOV(string _idCuentaCorriente, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasByIdFormaPagoOV(_idCuentaCorriente, _idFormaPagoOV);
        }

        public static List<cCuota> GetCuotasActivasByIdFormaPagoOV(string _idCuentaCorriente, string _idFormaPagoOV, string cantCuotasAdelantadas)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivasByIdFormaPagoOV(_idCuentaCorriente, _idFormaPagoOV, cantCuotasAdelantadas);
        }

        public static List<cCuota> GetCuotasPagoByIdFormaPagoOV(string _idCuentaCorriente, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasPagoByIdFormaPagoOV(_idCuentaCorriente, _idFormaPagoOV);
        }

        public static List<cCuota> GetAllCuotasActiva()
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetAllCuotasActiva();
        }
        public static List<cCuota> GetAllCuotasPendienteByActiva(string idEmpresa)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetAllCuotasActivaByActiva(idEmpresa);
        }
        public static List<cCuota> GetCuotasMes(Int16 estado, DateTime fecha)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasMes(estado, fecha);
        }
        public static cCuota GetCuotasByFecha(string _idCC, DateTime fecha)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasByFecha(_idCC, fecha);
        }

        public static cCuota GetCuotasLastMonth(string _idCC, DateTime fecha)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasLastMonth(_idCC, fecha);
        }

        public static List<cCuota> GetCuotasActivaByFecha(DateTime fecha)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivaByFecha(fecha);
        }

        public static List<cCuota> GetCuotasActivaCAC()
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivaCAC();
        }

        public static List<cCuota> GetCuotasActivaUVA()
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivaUVA();
        }

        public static List<cCuota> GetCuotasActivaByFechaConCAC(DateTime fecha)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivaByFechaConCAC(fecha);
        }

        public static List<cCuota> GetCuotasActivaByFechaConUVA(DateTime fecha)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivaByFechaConUVA(fecha);
        }
        #endregion

        public static ArrayList LoadTable(string _IdCuentaCorriente)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.LoadTable(_IdCuentaCorriente);
        }

        public static DataTable GetDataTable(string _idCC)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("id", typeof(string)));
            tbl.Columns.Add(new DataColumn("nro", typeof(string)));
            //ArrayList valores = LoadTable(_idCC);
            ArrayList valores = LoadTable(_idCC);
            foreach (cCuota cg in valores)
                tbl.Rows.Add(cg.Id, cg.Nro);
            return tbl;
        }

        public static decimal CalcularCuota(int cantCuotas, decimal total)
        {
            //CantCuotas: el total se divide por la cantidad de cuotas pendientes
            return Math.Round(total / cantCuotas, 5);
        }

        public static decimal CalcularVariacionMensualCAC(string _indiceBase, string _indiceMes, bool cac)
        {
            decimal aux = 0;
            if (cac == true)
            {
                if (_indiceBase != "0")
                {
                    if (_indiceMes != "0")//Si el índice CAC no esta actualizado
                    {
                        decimal indiceBase = cIndiceCAC.Load(_indiceBase).Valor;
                        decimal indiceMes = cIndiceCAC.Load(_indiceMes).Valor;

                        decimal variacion = Math.Round((indiceMes / indiceBase), 5) * 100;
                        //decimal variacion = (indiceMes / indiceBase) * 100;

                        aux = variacion - 100;
                    }
                    else
                        aux = 0;
                }
                else
                    aux = 0;
            }
            else
            {
                aux = 0;
            }

            return aux;
        }

        public static decimal CalcularVariacionMensualUVA(string _indiceBase, string _indiceMes, decimal monto)
        {
            decimal aux = 0;
            if (monto == 0)
            {
                if (_indiceBase != "0")
                {
                    if (_indiceMes != "0")//Si el índice CAC no esta actualizado
                    {
                        decimal indiceBase = cUVA.Load(_indiceBase).Valor;
                        decimal indiceMes = cUVA.Load(_indiceMes).Valor;

                        decimal variacion = Math.Round((indiceMes / indiceBase), 5) * 100;
                        //decimal variacion = (indiceMes / indiceBase) * 100;

                        aux = variacion - 100;
                    }
                    else
                        aux = 0;
                }
                else
                    aux = 0;
            }
            else //Cuando se calcula la primera vez, se toma el valor que se establece en la operación de venta.
            {
                if (_indiceBase != "0")
                {
                    if (_indiceMes != "0")//Si el índice CAC no esta actualizado
                    {
                        decimal indiceBase = monto;
                        decimal indiceMes = cUVA.Load(_indiceMes).Valor;

                        decimal variacion = Math.Round((indiceMes / indiceBase), 5) * 100;

                        aux = variacion - 100;
                    }
                    else
                        aux = 0;
                }
                else
                    aux = 0;
            }

            return aux;
        }

        public static decimal CalcularSaldoByIndice(decimal saldo, decimal variacionCAC_UVA)
        {
            decimal total = 0;

            if (variacionCAC_UVA != 0)
            {
                total = ((variacionCAC_UVA + 100) * saldo) / 100;

                //Calculo la comisión
                total = Math.Round(total, 5);

            }
            else
                total = saldo;

            return total;
        }

        public static decimal CalcularSaldoAnteriorByIndice(decimal saldo, decimal variacionCAC_UVA)
        {
            decimal total1 = 0;

            if(variacionCAC_UVA!=0)
                total1 = (saldo * 100) / (variacionCAC_UVA + 100);
            else
                total1 = saldo;

            total1 = Math.Round(total1, 5);

            return total1;
        }

        public static decimal CalcularComisionIva(decimal monto, decimal comision, bool _iva)
        {
            decimal aux1 = (monto * comision) / 100;
            decimal aux;
            if (_iva == true)
                aux = aux1 * Convert.ToDecimal(1.21); //Agrego IVA
            else
                aux = aux1;

            return aux;
        }

        public static decimal CalcularCuotaAjustada(decimal montoCuota, decimal comision)
        {
            /*
            decimal total = 0;
            
            //Calculo la comisión
            total = (montoCuota * (Convert.ToDecimal(comision) + 100)) / 100;
            total = Math.Round(total, 2);
            
            //Sumo el IVA
            total = (total * 121) / 100; 
            */

            return montoCuota + comision;
        }

        public static decimal CalcularPunitorio(decimal valorCuota)
        {
            decimal total = 0;

            total = (valorCuota * 103) / 100;

            return total;
        }

        public static decimal Calcular2Venc(decimal montoCuota)
        {
            /*
            decimal total = 0;
            decimal _comision = Convert.ToDecimal(comision);

            //Calculo la comisión
            //total = (montoCuota * _comision) + (((montoCuota * _comision) * _comision) * Convert.ToDecimal(1.21));
            total = 
            total = Math.Round(total, 2);
            */

            //return Math.Round((montoCuota * 102)/100, 2);
            return Convert.ToDecimal((montoCuota * 102) / 100); //Al total de la cuota se agrega un 2%
        }

        public static string GetTotalACobrar()
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetTotalACobrar();
        }

        public static decimal GetTotalMontoCuotas(string _idCC, int _nroDesde, int _nroA)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetTotalMontoCuotas(_idCC, _nroDesde, _nroA);
        }

        public static cCuota GetCuotaByNro(string _idCC, int _nro, string _idFp)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotaByNro(_idCC, _nro, _idFp);
        }

        public static cCuota GetCuotaByNro(string _idCC, string __idFormaPago, int _nro)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotaByNro(_idCC, __idFormaPago, _nro);
        }

        public static List<cCuota> GetCuotasByNro(string _idCC, int _nroDesde, int _nroA, string _idFormaPago)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasByNro(_idCC, _nroDesde, _nroA, _idFormaPago);
        }

        public static cCuota GetFirst(string idCC, string _idFormaPago)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetFirst(idCC, _idFormaPago);
        }

        public static cCuota GetFirstActiva(string idCC, string _idFormaPago)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetFirstActiva(idCC, _idFormaPago);
        }

        public static cCuota GetFirstPendiente(string idCC, string _idFormaPago)
        { 
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetFirstPendiente(idCC, _idFormaPago);
        }

        public static cCuota GetFirstNextPendiente(string idCC, string _idFormaPago)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetFirstNextPendiente(idCC, _idFormaPago);
        }

        public static cCuota GetFirstPagada(string idCC, string _idFormaPago)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetFirstPagada(idCC, _idFormaPago);
        }

        public static cCuota GetFirstActivaOrPendiente(string idCC, string _idFormaPago)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetFirstActivaOrPendiente(idCC, _idFormaPago);
        }

        public static cCuota GetFirst(string idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetFirst1(idCC);
        }

        public static cCuota GetLast(string idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetLast(idCC);
        }

        public static cCuota GetLastOrderDesc(string _idCC, string _idFp)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetLastOrderDesc(_idCC, _idFp);
        }

        public static cCuota GetLastByEstado(string idCC, Int16 _idEstado)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetLastByEstado(idCC, _idEstado);
        }

        public static cCuota GetLastPay(string idCC, string _idFp)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetLastPay(idCC, _idFp);
        }

        public static cCuota GetLastPayByIdCCU(string _idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetLastPayByIdCCU(_idCC);
        }

        public static Int16 GetCantCuotasPagas(string idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCantCuotasPagas(idCC);
        }
        
        public static Int16 GetCantCuotasPagasAnticipos(string _idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCantCuotasPagasAnticipos(_idCC);
        }

        public static List<cCuota> GetCuotasPagas(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasPagas(_idCC, _idFormaPagoOV);
        }

        public static Int32 GetCantCuotasPagasAdelantos(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCantCuotasPagasAdelantos(_idCC, _idFormaPagoOV);
        }

        public static Int16 GetCantCuotasAdelantadas(string _idCC, string _idFormaPagoOV)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCantCuotasAdelantadas(_idCC, _idFormaPagoOV);
        }

        public static ArrayList GetCuotaByIdCC(string _idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotaByIdCC(_idCC);
        }

        public static ArrayList GetCuotasOrderByFormaPagoOV(string _idCC)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasOrderByFormaPagoOV(_idCC);
        }

        public static List<cCuota> GetCuotasVencidas(string _idCuentaCorriente)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasVencidas(_idCuentaCorriente);
        }
        
        public static List<cCuota> GetCuotasActivasByRangoNro(string _idCuentaCorriente, string _nro)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivasByRangoNro(_idCuentaCorriente, _nro);
        }

        public static List<cCuota> GetCuotasSinceNro(string _idCuentaCorriente, string _nro)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasSinceNro(_idCuentaCorriente, _nro);
        }

        public static List<cCuota> GetCuotasByNroRecibo(string _idNroRecibo)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasByNroRecibo(_idNroRecibo);
        }

        public static decimal GetSaldoCuota(string _idCC, string _idFormaPago, Int16 _estado, string _moneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetSaldoCuota(_idCC, _idFormaPago, _estado, _moneda);
        }

        public static decimal GetSaldoCuotaLastMonth(string _idCC, string _idFormaPago, Int16 _estado, string _moneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetSaldoCuotaLastMonth(_idCC, _idFormaPago, _estado, _moneda);
        }

        public static decimal SaldoCC(string _idCC, string _idFormaPago, string _moneda)
        {
            decimal _saldo = GetSaldoCuota(_idCC, _idFormaPago, (Int16)estadoCuenta_Cuota.Pagado, _moneda);
            return _saldo;
            /*if (_saldo != 0 && _saldo != null)
            {
                int cuotasPendientes = cCuota.GetCuotasPendientes2(_idCC, _idFormaPago).Count;
                if (cuotasPendientes > 0)
                {
                    //int cantCuotas = cFormaPagoOV.Load(_idFormaPago).CantCuotas - cCuota.GetCuotasAnticipos(_idCC, _idFormaPago).Count - cuotasPendientes - cCuota.GetCuotasPagas(_idCC, _idFormaPago).Count;
                    int cantCuotas = cuotasPendientes - cCuota.GetCuotasAnticipos(_idCC, _idFormaPago).Count;

                    decimal _total = 0;
                    decimal _monto = 0;
                    List<cCuota> cuotas = cCuota.GetCuotasActivasDESC(_idCC, _idFormaPago);
                    foreach (cCuota c in cuotas)
                    {
                        _total += c.Monto;
                        _monto = c.Monto;
                    }

                    _total = _total + (cantCuotas * _monto);

                    return _total;


                    //cCuota cuota = cCuota.GetLastByEstado(_idCC, (Int16)estadoCuenta_Cuota.Activa);
                    //if(cuota == null)
                    //    cuota = cCuota.GetLastByEstado(_idCC, (Int16)estadoCuenta_Cuota.Pendiente);

                    //return cantCuotas * cuota.Monto;
                }
                else
                {
                    //decimal _total = 0;
                    List<cCuota> cuotas = cCuota.GetCuotasActivasDESC(_idCC, _idFormaPago);
                    DateTime dateDesde = Convert.ToDateTime(DateTime.Today.Year + "-" + DateTime.Today.Month + " - " + 1);

                    cCuota cuota = cCuota.GetCuotasLastMonth(_idCC, dateDesde);
                    if (cuota != null)
                    {
                        if (cuota.Estado == (Int16)estadoCuenta_Cuota.Activa)
                            _saldo = cuota.saldo;
                    }

                    //return _total;
                    return _saldo;
                };
            }
            else
            {
                return 0;
                //if (cCuentaCorriente.Load(_idCC).IdEstado == ((Int16)estadoCuenta_Cuota.Pagado))
                //    return 0;
                //else
                //    return GetSaldoCuota(_idCC, _idFormaPago, (Int16)estadoCuenta_Cuota.Pagado, _moneda);
            }*/
        }

        public static decimal DeudaCC(string _idCC, string _idFormaPago, string _moneda)
        {
            int cuotasPendientes = cCuota.GetCuotasPendientes2(_idCC, _idFormaPago).Count;
            if (cuotasPendientes > 0)
            {
                int cantCuotas = cuotasPendientes - cCuota.GetCuotasAnticipos(_idCC, _idFormaPago).Count;

                decimal _total = 0;
                decimal _monto = 0;
                List<cCuota> cuotas = cCuota.GetCuotasActivas(_idCC, _idFormaPago);
                if (cuotas.Count == 0)
                {
                    cuotas = cCuota.GetCuotasPendientes2(_idCC, _idFormaPago);
                    _monto = cuotas[0].Monto;
                }
                else
                {
                    _monto = cuotas[0].Monto;
                }

                /*foreach (cCuota c in cuotas)
                {
                    _total += c.Monto;
                    _monto = c.Monto;
                }*/

                _total = cantCuotas * _monto;

                return _total;

                //cCuota cuota = cCuota.GetLastByEstado(_idCC, (Int16)estadoCuenta_Cuota.Activa);
                //if (cuota == null)
                //    cuota = cCuota.GetLastByEstado(_idCC, (Int16)estadoCuenta_Cuota.Pendiente);

                //return cuotasPendientes * cuota.Monto;
            }
            else
            {
                return 0;
            };
        }

        public static string GetNroSaldoCuota(string _idCC, string _idFormaPago, Int16 _estado, string _moneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetNroSaldoCuota(_idCC, _idFormaPago, _estado, _moneda);
        }

        public static string NroSaldoCC(string _idCC, string _idFormaPago, string _moneda)
        {
            decimal _saldo = GetSaldoCuota(_idCC, _idFormaPago, (Int16)estadoCuenta_Cuota.Activa, _moneda);
            if (_saldo != 0 && _saldo != null)
                return GetNroSaldoCuota(_idCC, _idFormaPago, (Int16)estadoCuenta_Cuota.Activa, _moneda);
            else
                return GetNroSaldoCuota(_idCC, _idFormaPago, (Int16)estadoCuenta_Cuota.Pagado, _moneda);
        }

        public static ArrayList GetSaldos()
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetSaldos();
        }

        public static decimal GetCuotasMes(string _idEmpresa, DateTime dateDesde, DateTime dateHasta, string _idFp)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasMes(_idEmpresa, dateDesde, dateHasta, _idFp);
        }

        public static decimal GetCuotasMesMonto(string _idEmpresa, DateTime dateDesde, DateTime dateHasta, string _idFp)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasMesMonto(_idEmpresa, dateDesde, dateHasta, _idFp);
        }

        public static List<cCuota> GetCantCuotasNextYear(string _idEmpresa, string _idFormaPago, DateTime dateDesde, DateTime dateHasta)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCantCuotasNextYear(_idEmpresa, _idFormaPago, dateDesde, dateHasta);
        }

        public static List<cCuota> GetCuotasMesProyecto(string _idProyecto, DateTime dateDesde, DateTime dateHasta, Int16 _tipoMoneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasMesProyecto(_idProyecto, dateDesde, dateHasta, _tipoMoneda);
        }

        public static List<cCuota> GetCuotasRepetidas(DateTime dateDesde, DateTime dateHasta, Int16 _tipoMoneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasRepetidas(dateDesde, dateHasta, _tipoMoneda);
        }

        public static List<cCuota> GetCuotasMesProyectoPendientes(string _idProyecto, Int16 _tipoMoneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasMesProyectoPendientes(_idProyecto, _tipoMoneda);
        }

        public static List<cCuota> GetCuotasByProyecto(string _idProyecto)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasByProyecto(_idProyecto);
        }

        public static List<cCuota> GetCuotasProyectoRestantes(string _idProyecto, DateTime _dateDesde, Int16 _tipoMoneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasProyectoRestantes(_idProyecto, _dateDesde, _tipoMoneda);
        }

        public static List<cCuota> GetCuotasProyectoRestantesRepetidos(DateTime _dateDesde, Int16 _tipoMoneda)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasProyectoRestantesRepetidos(_dateDesde, _tipoMoneda);
        }

        public static List<cCuota> GetCuotasActivasByEmpresa(string _idEmpresa)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasActivasByEmpresa(_idEmpresa);
        }

        public static List<cCuota> GetItemByCuotasPendientes(string _idCCU)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetItemByCuotasPendientes(_idCCU);
        }

        public static decimal GetSaldoByProyecto()
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetSaldoByProyecto();
        }

        public static DataTable GetCuotasByFecha(DateTime dateDesde, DateTime dateHasta)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasByFecha(dateDesde, dateHasta);
        }

        public static DataTable GetCuotasByFecharRestantes(DateTime dateDesde)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasByFecharRestantes(dateDesde);
        }

        public static DataTable GetCuotasObraByFecha(string idObra, DateTime dateDesde, DateTime dateHasta)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasObraByFecha(idObra, dateDesde, dateHasta);
        }

        public static DataTable GetCuotasConMasProyectoByFecha(DateTime dateDesde, DateTime dateHasta)
        { 
             cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasConMasProyectoByFecha(dateDesde, dateHasta);
        }

        public static DataTable GetCuotasObraByFechaRestante(string idObra, DateTime dateDesde)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasObraByFechaRestante(idObra, dateDesde);
        }

        public static ArrayList GetEmpresas()
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetEmpresas();
        }

        public static ArrayList GetEmpresasByNombreApellido(string nombre, string apellido)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetEmpresasByNombreApellido(nombre, apellido);
        }

        public static ArrayList GetEmpresasByProyecto(string _idProyecto)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetEmpresasByProyecto(_idProyecto);
        }

        public static DataTable GetCuotasMesMontoByEmpresa(string _idEmpresa, DateTime dateDesde, DateTime dateHasta)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasMesMontoByEmpresa(_idEmpresa, dateDesde, dateHasta);
        }

        public static DataTable GetCuotasMesMontoByEmpresaAndProyecto(string _idEmpresa, string _idProyecto, DateTime dateDesde, DateTime dateHasta)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasMesMontoByEmpresaAndProyecto(_idEmpresa, _idProyecto, dateDesde, dateHasta);
        }

        public static DataTable GetCuotasMesRestantesMontoByEmpresa(string _idEmpresa, DateTime dateDesde)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasMesRestantesMontoByEmpresa(_idEmpresa, dateDesde);
        }

        public static DataTable GetCuotasMesRestantesMontoByEmpresaAndProyecto(string _idEmpresa, string _idProyecto, DateTime dateDesde)
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetCuotasMesRestantesMontoByEmpresaAndProyecto(_idEmpresa, _idProyecto, dateDesde);
        }

        public static DataTable GetMorosos()
        {
            cCuotaDAO DAO = new cCuotaDAO();
            return DAO.GetMorosos();
        }
    }
}
