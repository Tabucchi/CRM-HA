using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cCondonacion
    {
        private string id;
        private string idCuota;
        private string idItemCCU;
        private Int64 nro;
        private DateTime fecha;
        private decimal monto;
        private Int16 _papelera;

        public cCondonacion() { }

        public cCondonacion(string _idCuota, string _idItemCCU, Int64 _nro, DateTime _fecha)
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
                    return cEmpresa.GetClientesCondonacionByItemCCU(IdItemCCU).GetNombreCompleto;
                else{
                    if (IdItemCCU != "-1")
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
        #endregion

        #region Acceso a Datos
        public int Save()
        {
            cCondonacionDAO DAO = new cCondonacionDAO();
            return DAO.Save(this);
        }

        public static cCondonacion Load(string id)
        {
            cCondonacionDAO DAO = new cCondonacionDAO();
            return DAO.Load(id);
        }

        public static Int64 GetLastNro()
        {
            cCondonacionDAO DAO = new cCondonacionDAO();
            return DAO.GetLastNro();
        }

        //public static string GetReciboByIdCuota(string _idCuota)
        //{
        //    cReciboDAO DAO = new cReciboDAO();
        //    return DAO.GetReciboByIdCuota(_idCuota);
        //}

        public static string GetNroCondonacionByIdItemCCU(string _idItemCCU)
        {
            cCondonacionDAO DAO = new cCondonacionDAO();
            return DAO.GetNroCondonacionByIdItemCCU(_idItemCCU);
        }
        #endregion

        //public static cReciboCuota CrearRecibo(cCuota cuota)
        //{
        //    Int64 nro = cReciboCuota.GetLastNro() + 1;
        //    cReciboCuota recibo = new cReciboCuota(cuota.Id, "-1", nro, DateTime.Now);
        //    recibo._papelera = 1;
        //    recibo.Save();
        //    return recibo;
        //}

        public static cCondonacion CrearCondonacion(string _idCuota, string _idItemCC, decimal _monto)
        {
            Int64 nro = cCondonacion.GetLastNro() + 1;
            cCondonacion condonacion = new cCondonacion(_idCuota, _idItemCC, nro, DateTime.Now);
            condonacion.Monto = _monto;
            condonacion._papelera = 1;
            condonacion.Save();
            return condonacion;
        }

        //public static cReciboCuota GetReciboByIdItemCCU(string _idItemCCU)
        //{
        //    cReciboDAO DAO = new cReciboDAO();
        //    return DAO.GetReciboByIdItemCCU(_idItemCCU);
        //}

        public static cCondonacion GetCondonacionByIdItemCCU(string _idItemCCU)
        {
            cCondonacionDAO DAO = new cCondonacionDAO();
            return DAO.GetReciboByIdItemCCU(_idItemCCU);
        }

        public static cCondonacion GetCondonacionByNro(string _nro)
        {
            cCondonacionDAO DAO = new cCondonacionDAO();
            return DAO.GetCondonacionByNro(_nro);
        }

        public static List<cCondonacion> GetAllCondonaciones()
        {
            cCondonacionDAO DAO = new cCondonacionDAO();
            return DAO.GetAllCondonaciones();
        }

        public static List<cCondonacion> GetCondonaciones(string _desde, string _hasta, string _idEmpresa, string _nro, string _estado)
        {
            cCondonacionDAO DAO = new cCondonacionDAO();
            return DAO.GetCondonaciones(_desde, _hasta, _idEmpresa, _nro, _estado);
        }

        public static List<cCondonacion> GetCondonacionToday(string _idCCU)
        {
            cCondonacionDAO DAO = new cCondonacionDAO();
            return DAO.GetCondonacionToday(_idCCU);
        }
    }
}
