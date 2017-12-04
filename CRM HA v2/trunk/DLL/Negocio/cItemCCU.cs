using DLL.Base_de_Datos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cItemCCU
    {
        private string id;
        private string idCuentaCorrienteUsuario;
        private DateTime fecha;
        private string concepto;
        private decimal debito;
        private decimal credito;
        private decimal saldo;
        private string idCuota;
        private Int16 idEstado;
        private Int16 tipoOperacion;

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string IdCuentaCorrienteUsuario
        {
            get { return idCuentaCorrienteUsuario; }
            set { idCuentaCorrienteUsuario = value; }
        }

        public string GetEmpresa
        {
            get { return cCuentaCorrienteUsuario.Load(IdCuentaCorrienteUsuario).GetEmpresa; }
        }

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public string GetFecha
        {
            get { return String.Format("{0:dd/MM/yyyy}", Fecha); }
        }
        public string Concepto
        {
            get { return concepto; }
            set { concepto = value; }
        }
        public decimal Debito
        {
            get { return debito; }
            set { debito = value; }
        }
        public string GetDebito
        {
            get
            { return String.Format("{0:#,#0.00}", Debito); }
        }
        public decimal Credito
        {
            get { return credito; }
            set { credito = value; }
        }
        public string GetCredito
        {
            get
            { return String.Format("{0:#,#0.00}", Credito); }
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
        public string IdCuota
        {
            get { return idCuota; }
            set { idCuota = value; }
        }
        public string GetNroRecibo
        {
            get
            {
                if (!string.IsNullOrEmpty(idCuota))
                    return cCuota.Load(IdCuota).GetRecibo;
                else
                    return "-";
            }
        }
        public Int16 IdEstado
        {
            get { return idEstado; }
            set { idEstado = value; }
        }
        public Int16 TipoOperacion
        {
            get { return tipoOperacion; }
            set { tipoOperacion = value; }
        }

        public string GetTipoOperacion
        {
            get
            {
                string comprobante = null;
                switch (tipoOperacion)
                {
                    case (Int16)eTipoOperacion.PagoCuota:
                        comprobante = "Recibo";
                        break;
                    case (Int16)eTipoOperacion.NotaCredito:
                        comprobante = "Nota de Crédito";
                        break;
                    case (Int16)eTipoOperacion.NotaDebito:
                        comprobante = "Nota de Débito";
                        break;
                    case (Int16)eTipoOperacion.OtrosPago:
                        comprobante = "Recibo";
                        break;
                    case (Int16)eTipoOperacion.Adelanto:
                        comprobante = "Recibo";
                        break;
                    case (Int16)eTipoOperacion.Saldo:
                        comprobante = "Recibo";
                        break;
                    case (Int16)eTipoOperacion.Anular:
                        comprobante = "Recibo";
                        break;
                }
                return comprobante;
            }
        }
        #endregion

        public cItemCCU() { }

        public cItemCCU(string _idCuentaCorrienteUsuario, DateTime _fecha, string _concepto, decimal _debito, decimal _credito, decimal _saldo, string _idCuota)
        {
            idCuentaCorrienteUsuario = _idCuentaCorrienteUsuario;
            fecha = _fecha;
            concepto = _concepto;
            debito = _debito;
            credito = _credito;
            saldo = _saldo;
            idCuota = _idCuota;
            idEstado = (Int16)estadoCuenta_Cuota.Activa;
            this.Save();
        }

        public cItemCCU(string _idCuentaCorrienteUsuario, DateTime _fecha, string _concepto, decimal _debito, decimal _credito, decimal _saldo, string _idCuota, Int16 _estado)
        {
            idCuentaCorrienteUsuario = _idCuentaCorrienteUsuario;
            fecha = _fecha;
            concepto = _concepto;
            debito = _debito;
            credito = _credito;
            saldo = _saldo;
            idCuota = _idCuota;
            idEstado = _estado;
            this.Save();
        }

        #region Acceso a Datos

        public static cItemCCU Load(string id)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.Load(id);
        }
        public int Save()
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.Save(this);
        }

        public static List<cItemCCU> GetCuentaCorriente(string _idCC)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetCuentaCorriente(_idCC);
        }

        public static cItemCCU GetCuentaCorrienteByIdCuota(string _idCuota)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetCuentaCorrienteByIdCuota(_idCuota);
        }

        public static string GetLastSaldoByIdCCU(string _idCCU)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetLastSaldoByIdCCU(_idCCU);
        }

        public static cItemCCU GetLastItemById(string _id)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetLastItemById(_id);
        }

        public static string GetCCByIdCuota(string _idCuota)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetCCByIdCuota(_idCuota);
        }

        public static cItemCCU GetItemCCUByIdCuota(string _idCuota)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetItemCCUByIdCuota(_idCuota);
        }

        public static cItemCCU GetFirtsItemCCUByIdCuota(string _idCuota)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetFirtsItemCCUByIdCuota(_idCuota);
        }

        public static cItemCCU GetLastReciboByIdCCU(string _idCCU)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetLastReciboByIdCCU(_idCCU);
        }

        public static cItemCCU GetLastNotaCreditoByIdCCU(string _idCCU)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetLastNotaCreditoByIdCCU(_idCCU);
        }

        public static cItemCCU GetLastNotaDebitoByIdCCU(string _idCCU)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetLastNotaDebitoByIdCCU(_idCCU);
        }

        public static int GetCantCuotasById(string _idCuota)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetCantCuotasById(_idCuota);
        }
        public static string GetTopNroCuota(string _idCuentaCorriente)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetTopNroCuota(_idCuentaCorriente);
        }
        public static List<cItemCCU> GetItems(Int16 _idEstado)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetItems(_idEstado);
        }

        public static List<cItemCCU> GetItemsByCuotas(string _idCuota)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetItemsByCuotas(_idCuota);
        }

        public static decimal GetTotalCuotasByEstado(string _idCuota, Int16 _idEstado)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetTotalCuotasByEstado(_idCuota, _idEstado);
        }

        public static List<cItemCCU> GetItemsByCuotas(string _idCuotaDesde, string _idCuotaHasta)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetItemsByCuotas(_idCuotaDesde, _idCuotaHasta);
        }

        public static List<cItemCCU> GetItemsByCuotas_EstadoPagado(string _idCuota)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetItemsByCuotas_EstadoPagado(_idCuota);
        }

        public string GetComprobante
        {
            get
            {
                string comprobante = null;
                switch (tipoOperacion)
                {
                    case (Int16)eTipoOperacion.PagoCuota:
                        comprobante = "R" + GetRecibo;
                        break;
                    case (Int16)eTipoOperacion.NotaCredito:
                        comprobante = "NC" + GetNotaCredito;
                        break;
                    case (Int16)eTipoOperacion.NotaDebito:
                        comprobante = "ND" + GetNotaDebito;
                        break;
                    case (Int16)eTipoOperacion.OtrosPago:
                        comprobante = "R" + GetRecibo;
                        break;
                    case (Int16)eTipoOperacion.Adelanto:
                        comprobante = "R" + GetRecibo;
                        break;
                    case (Int16)eTipoOperacion.Saldo:
                        comprobante = "R" + GetRecibo;
                        break;
                    case (Int16)eTipoOperacion.Reserva:
                        comprobante = "R" + GetRecibo;
                        break;
                    case (Int16)eTipoOperacion.Condonacion:
                        comprobante = "C" + GetCondonacion;
                        break;
                }
                return comprobante;
            }
        }
        
        public static string GetLastComprobante(string _idCCU)
        {
            ArrayList items = new ArrayList();
            cItemCCU item = new cItemCCU();
            item = GetLastReciboByIdCCU(_idCCU);

            if(item != null)
                items.Add(item.Id);

            item = GetLastNotaCreditoByIdCCU(_idCCU);
            if (item != null)
                items.Add(item.id);

            item = GetLastNotaDebitoByIdCCU(_idCCU);
            if (item != null)
                items.Add(item.id);

            Int16 _itemAux = 0;
            foreach(var _item in items){
                if(_itemAux < Convert.ToInt16(_item))
                    _itemAux = Convert.ToInt16(_item.ToString());
            }

            return _itemAux.ToString();
        }

        public string GetRecibo
        {
            get
            {
                //string nro = cReciboCuota.GetNroReciboByIdItemCCU(Id);
                //return nro == null ? " - " : cAuxiliar.AgregarCeroRecibo(nro);

                cReciboCuota r = cReciboCuota.GetReciboByIdItemCCU(Id);
                if (r != null)
                {
                    if (r._Papelera == (Int16)papelera.Activo)
                        return cAuxiliar.AgregarCeroRecibo(r.Nro.ToString());
                    else
                        return cAuxiliar.AgregarCeroRecibo(r.Nro.ToString()) + " - Anulado";
                }
                else
                    return " - ";
            }
        }

        public string GetCondonacion
        {
            get
            {
                //string nro = cCondonacion.GetNroCondonacionByIdItemCCU(Id);
                //return nro == null ? " - " : cAuxiliar.AgregarCeroRecibo(nro);

                cCondonacion c = cCondonacion.GetCondonacionByIdItemCCU(Id);
                if (c != null)
                {
                    if (c._Papelera == (Int16)papelera.Activo)
                        return cAuxiliar.AgregarCeroRecibo(c.Nro.ToString());
                    else
                        return cAuxiliar.AgregarCeroRecibo(c.Nro.ToString()) + " - Anulado";
                }
                else
                    return " - ";
            }
        }

        public string GetNotaCredito
        {
            get
            {
                //string nro = cNotaCredito.GetNotaCreditoByIdItemCCU(Id);
                //return nro == null ? " - " : cAuxiliar.AgregarCeroRecibo(nro);

                cNotaCredito nc = cNotaCredito.GetNCByIdItemCCU(Id);
                if (nc != null)
                {
                    if (nc._Papelera == (Int16)papelera.Activo)
                        return cAuxiliar.AgregarCeroRecibo(nc.Nro.ToString());
                    else
                        return cAuxiliar.AgregarCeroRecibo(nc.Nro.ToString()) + " - Anulado";
                }
                else
                    return " - ";
            }
        }

        public string GetNotaDebito
        {
            get
            {
                //string nro = cNotaDebito.GetNotaDebitoByIdItemCCU(Id);
                //return nro == null ? " - " : cAuxiliar.AgregarCeroRecibo(nro);

                cNotaDebito nd = cNotaDebito.GetNDByIdItemCCU(Id);
                if (nd != null)
                {
                    if (nd._Papelera == (Int16)papelera.Activo)
                        return cAuxiliar.AgregarCeroRecibo(nd.Nro.ToString());
                    else
                        return cAuxiliar.AgregarCeroRecibo(nd.Nro.ToString()) + " - Anulado";
                }
                else
                    return " - ";
            }
        }

        public static List<cItemCCU> GetCuentaCorrienteLast10(string _idCC)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetCuentaCorrienteLast10(_idCC);
        }

        public static List<cItemCCU> GetCuentaCorrienteLast10Desc(string _idCC)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetCuentaCorrienteLast10Desc(_idCC);
        }

        public static List<cItemCCU> Search(string _idCC, string _fechaDesde, string _fechaHasta)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.Search(_idCC, _fechaDesde, _fechaHasta);
        }

        public static List<cItemCCU> GetItemByCuotasPendientes(string _idCC)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetItemByCuotasPendientes(_idCC);
        }

        public static List<cItemCCU> GetItemByCuotasPendientesByIdCC(string _idCC)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetItemByCuotasPendientesByIdCC(_idCC);
        }

        public static List<cItemCCU> GetItemsFromId(string _id, string _idCCU)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetItemsFromId(_id, _idCCU);
        }

        public static List<cItemCCU> GetItemByCuotasAdelanto(string _idCC, Int16 _idEstado)
        {
            cItemCCUDAO DAO = new cItemCCUDAO();
            return DAO.GetItemByCuotasAdelanto(_idCC, _idEstado);
        }

        #endregion
    }
}
