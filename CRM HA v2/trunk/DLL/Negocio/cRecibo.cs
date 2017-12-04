using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cReciboCuota
    {
        private string id;
        private string idCuota;
        private string idItemCCU;
        private Int64 nro;
        private DateTime fecha;
        private decimal monto;
        private Int16 _papelera;

        public cReciboCuota() { }

        public cReciboCuota(string _idCuota, string _idItemCCU, Int64 _nro, DateTime _fecha)
        {
            idCuota = _idCuota;
            IdItemCCU = _idItemCCU;
            nro = _nro;
            fecha = _fecha;
        }

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string IdCuota
        {
            get { return idCuota; }
            set { idCuota = value; }
        }
        public string IdItemCCU
        {
            get { return idItemCCU; }
            set { idItemCCU = value; }
        }
        
        public Int16 _Papelera
        {
            get { return _papelera; }
            set { _papelera = value; }
        }

        public string GetEstado
        {
            get
            {
                if ((Int16)papelera.Activo == _papelera)
                    return "Emitido";
                else
                    return "Anulado";
            }
        }

        public string tipoComprobante
        {
            get { return GetTipoComprobanteByIdItemCCU(IdItemCCU); }
        }

        public Int64 Nro
        {
            get { return nro; }
            set { nro = value; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public string GetEmpresa
        {
            get
            {
                if (IdItemCCU != "-1")
                {
                    cEmpresa empresa = cEmpresa.GetClientesReciboByItemCCU(IdItemCCU);
                    if (empresa != null)
                        return empresa.GetNombreCompleto;
                    else
                    {
                        if (IdCuota != "-1")
                            return cCuota.Load(IdCuota).GetEmpresa;
                        else
                            return "-";
                    }
                }
                else
                {
                    if (IdCuota != "-1")
                        return cCuota.Load(IdCuota).GetEmpresa;
                    else
                        return "-";
                }
            }
        }
        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        public string GetMonto
        {
            get { 
                if((Int16)papelera.Activo == _papelera)
                    return String.Format("{0:#,#0.00}", Monto); 
                else
                    return String.Format("{0:#,#0.00}", Monto * -1); 
            }
        }

        public string GetRecibo
        {
            get
            {
                return nro == null ? "-" : cAuxiliar.AgregarCeroRecibo(Nro.ToString());
            }
        }   
        #endregion

        #region Acceso a Datos
        public int Save()
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.Save(this);
        }

        public static cReciboCuota Load(string id)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.Load(id);
        }

        public static Int64 GetLastNro()
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetLastNro();
        }
        public static string GetReciboByIdCuota(string _idCuota)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetReciboByIdCuota(_idCuota);
        }

        public static string GetNroReciboByIdItemCCU(string _idItemCCU)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetNroReciboByIdItemCCU(_idItemCCU);
        }

        public static string GetTipoComprobanteByIdItemCCU(string _idItemCCU)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetTipoComprobanteByIdItemCCU(_idItemCCU);
        }
        #endregion

        public static cReciboCuota CrearRecibo(cCuota cuota)
        {
            Int64 nro = cReciboCuota.GetLastNro() + 1;
            cReciboCuota recibo = new cReciboCuota(cuota.Id, "-1", nro, DateTime.Now);
            recibo._papelera = 1;
            recibo.Save();
            return recibo;
        }

        public static cReciboCuota CrearRecibo(string _idCuota, string _idItemCC, decimal _monto)
        {
            Int64 nro = cReciboCuota.GetLastNro() + 1;
            cReciboCuota recibo = new cReciboCuota(_idCuota, _idItemCC, nro, DateTime.Now);
            recibo.Monto = _monto;
            recibo._papelera = 1;
            recibo.Save();
            return recibo;
        }

        public static cReciboCuota GetLastReciboByCCU(string _idCCU)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetLastReciboByCCU(_idCCU);
        }

        public static cReciboCuota GetReciboByIdItemCCU(string _idItemCCU)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetReciboByIdItemCCU(_idItemCCU);
        }

        /*public decimal GetMonto(string _idItemCCU)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetMonto(_idItemCCU);
        }*/

        public static cReciboCuota GetReciboByNro(string _nro)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetReciboByNro(_nro);
        }

        public static List<cReciboCuota> GetAllRecibos()
        {
             cReciboDAO DAO = new cReciboDAO();
             return DAO.GetAllRecibos();
        }

        public static List<cReciboCuota> GetRecibos(string _desde, string _hasta, string _idEmpresa, string _nro, string _estado)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetRecibos(_desde, _hasta, _idEmpresa, _nro, _estado);
        }

        public static List<cReciboCuota> GetRecibosByNroCuota(string _idCC, string _nro, string _idFormaPago)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetRecibosByNroCuota(_idCC, _nro, _idFormaPago);
        }

        public static List<cReciboCuota> GetRecibosByNroFromItemCCU(string _nro)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetRecibosByNroFromItemCCU(_nro);
        }

        public static List<cReciboCuota> GetRecibosToday(string _idCCU)
        {
            cReciboDAO DAO = new cReciboDAO();
            return DAO.GetRecibosToday(_idCCU);
        }
    }
}

