using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cNotaCredito
    {
        private string id;
        private string idCuota;
        private string idItemCCU;
        private Int64 nro;
        private DateTime fecha;
        private decimal monto;
        private Int16 _papelera;

        public cNotaCredito() { }

        public cNotaCredito(string _idCuota, string _idItemCCU, Int64 _nro, DateTime _fecha)
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
                    cEmpresa empresa = cEmpresa.GetClientesNotaCreditoByItemCCU(IdItemCCU);
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
        #endregion

        #region Acceso a Datos
        public int Save()
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.Save(this);
        }

        public static cNotaCredito Load(string id)
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.Load(id);
        }

        public static Int64 GetLastNro()
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.GetLastNro();
        }
        public static string GetReciboByIdCuota(string _idCuota)
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.GetReciboByIdCuota(_idCuota);
        }

        public static string GetNotaCreditoByIdItemCCU(string _idItemCCU)
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.GetNotaCreditoByIdItemCCU(_idItemCCU);
        }
        #endregion

        public static cNotaCredito CrearRecibo(cCuota cuota)
        {
            Int64 nro = cNotaCredito.GetLastNro() + 1;
            cNotaCredito recibo = new cNotaCredito(cuota.Id, "-1", nro, DateTime.Now);
            recibo._papelera = 1;
            recibo.Save();
            return recibo;
        }

        /*public static cNotaCredito CrearNotaCredito(string _idItemCC)
        {
            Int64 nro = cNotaCredito.GetLastNro() + 1;
            cNotaCredito recibo = new cNotaCredito("-1", _idItemCC, nro, DateTime.Now);
            recibo.papelera = 1;
            recibo.Save();
            return recibo;
        }*/

        public static cNotaCredito CrearNotaCredito(string _idCuota, string _idItemCC, decimal _monto)
        {
            Int64 nro = cNotaCredito.GetLastNro() + 1;
            cNotaCredito recibo = new cNotaCredito(_idCuota, _idItemCC, nro, DateTime.Now);
            recibo.Monto = _monto;
            recibo._papelera = 1;
            recibo.Save();
            return recibo;
        }

        public static cNotaCredito GetNCByIdItemCCU(string _idItemCCU)
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.GetNCByIdItemCCU(_idItemCCU);
        }

        public static cNotaCredito GetNotaCreditoByNro(string _nro)
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.GetNotaCreditoByNro(_nro);
        }

        public static cNotaCredito GetObjectNotaCreditoByIdItemCCU(string _idItemCCU)
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.GetObjectNotaCreditoByIdItemCCU(_idItemCCU);
        }

        public static List<cNotaCredito> GetAllNotasCredito()
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.GetAllNotasCredito();
        }

        public static List<cNotaCredito> GetNotasCredito(string _desde, string _hasta, string _idEmpresa, string _nro, string _estado)
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.GetNotasCredito(_desde, _hasta, _idEmpresa, _nro, _estado);
        }

        public static List<cNotaCredito> GetNotasCreditoToday(string _idCCU)
        {
            cNotaCreditoDAO DAO = new cNotaCreditoDAO();
            return DAO.GetNotasCreditoToday(_idCCU);
        }
    }
}

