using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cNotaDebito
    {
        private string id;
        private string idCuota;
        private string idItemCCU;
        private Int64 nro;
        private DateTime fecha;
        private decimal monto;
        private Int16 _papelera;

        public cNotaDebito() { }

        public cNotaDebito(string _idCuota, string _idItemCCU, Int64 _nro, DateTime _fecha)
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
                    return "Válida";
                else
                    return "Anulada";
            }
        }

        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        public string GetMonto
        {
            get
            {
                if ((Int16)papelera.Activo == _papelera)
                    return String.Format("{0:#,#0.00}", Monto);
                else
                    return String.Format("{0:#,#0.00}", Monto * -1);
            }
        }

        public string GetEmpresa
        {
            get
            {
                if (IdItemCCU != "-1")
                {
                    cEmpresa empresa = cEmpresa.GetClientesNotaDebitoByItemCCU(IdItemCCU);
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
                    if (IdItemCCU != "-1")
                        return cCuota.Load(IdCuota).GetEmpresa;
                    else
                        return "-";
                }
            }
        }
        #endregion

        #region Acceso a Datos
        public int Save()
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.Save(this);
        }

        public static cNotaDebito Load(string id)
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.Load(id);
        }

        public static Int64 GetLastNro()
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.GetLastNro();
        }
        public static string GetNotaDebitoByIdCuota(string _idCuota)
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.GetReciboByIdCuota(_idCuota);
        }

        public static string GetNotaDebitoByIdItemCCU(string _idItemCCU)
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.GetNotaDebitoByIdItemCCU(_idItemCCU);
        }
        #endregion

        public static cNotaDebito GetNDByIdItemCCU(string _idItemCCU)
        { 
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.GetNDByIdItemCCU(_idItemCCU);
        }


        public static cNotaDebito CrearNotaDebito(cCuota cuota)
        {
            Int64 nro = cNotaDebito.GetLastNro() + 1;
            cNotaDebito recibo = new cNotaDebito(cuota.Id, "-1", nro, DateTime.Now);
            recibo._papelera = 1;
            recibo.Save();
            return recibo;
        }

        /*public static cNotaDebito CrearNotaDebito(string _idItemCC)
        {
            Int64 nro = cNotaDebito.GetLastNro() + 1;
            cNotaDebito recibo = new cNotaDebito("-1", _idItemCC, nro, DateTime.Now);
            recibo.papelera = 1;
            recibo.Save();
            return recibo;
        }*/

        public static cNotaDebito CrearNotaDebito(string _idCuota, string _idItemCC, decimal _monto)
        {
            Int64 nro = cNotaDebito.GetLastNro() + 1;
            cNotaDebito recibo = new cNotaDebito(_idCuota, _idItemCC, nro, DateTime.Now);
            recibo.Monto = _monto;
            recibo._papelera = 1;
            recibo.Save();
            return recibo;
        }

        public static cNotaDebito GetNotaDebitoByNro(string _nro)
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.GetNotaDebitoByNro(_nro);
        }

        public static cNotaDebito GetObjectNotaDebitoByIdItemCCU(string _idItemCCU)
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.GetObjectNotaDebitoByIdItemCCU(_idItemCCU);
        }

        public static List<cNotaDebito> GetAllNotasDebito()
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.GetAllNotasDebito();
        }

        public static List<cNotaDebito> GetNotasDebito(string _desde, string _hasta, string _idEmpresa, string _nro, string _estado)
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.GetNotasDebito(_desde, _hasta, _idEmpresa, _nro, _estado);
        }

        public static List<cNotaDebito> GetNotasDebitoToday(string _idCCU)
        {
            cNotaDebitoDAO DAO = new cNotaDebitoDAO();
            return DAO.GetNotasDebitoToday(_idCCU);
        }
    }
}

