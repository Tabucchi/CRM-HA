using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cRegistroPago
    {
        private string id;
        private DateTime fechaPago;
        private decimal monto;
        private string sucursal;
        private string transaccion;
        private string idEmpresa;
        private Int32 idImagen;
        private Int16 idEstado;
        private Int32 idCC;
        private Int16 nro;
        private Int16 formaPago;

        public cRegistroPago() {}

        public cRegistroPago(DateTime _fechaPago, decimal _monto, string _sucursal, string _transaccion, string _idEmpresa, Int32 _idImagen, Int16 _idEstado, Int32 _idCC, Int16 _nro, Int16 _formaPago)
        {
            fechaPago = _fechaPago;
            monto = _monto;
            sucursal = _sucursal;
            transaccion = _transaccion;
            idEmpresa = _idEmpresa;
            idImagen = _idImagen;
            idEstado = _idEstado;
            idCC = _idCC;
            nro = _nro;
            formaPago = _formaPago;
            this.Save();
        }

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public DateTime FechaPago
        {
            get { return fechaPago; }
            set { fechaPago = value; }
        }
        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }
        public string Sucursal
        {
            get { return sucursal; }
            set { sucursal = value; }
        }
        public string Transaccion
        {
            get { return transaccion; }
            set { transaccion = value; }
        }
        public string IdEmpresa
        {
            get { return idEmpresa; }
            set { idEmpresa = value; }
        }
        public Int32 IdImagen
        {
            get { return idImagen; }
            set { idImagen = value; }
        }
        public Int16 IdEstado
        {
            get { return idEstado; }
            set { idEstado = value; }
        }

        public string GetEmpresa
        {
            get { return cEmpresa.Load(IdEmpresa).Nombre; }
        }
        public Int32 IdCC
        {
            get { return idCC; }
            set { idCC = value; }
        }
        
        public Int16 Nro
        {
            get { return nro; }
            set { nro = value; }
        }
        public Int16 FormaPago
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
                    case 0:
                        _formaPago = formaDePago.UnPago.ToString().Replace("UnPago", "Pago");
                        break;
                    case 1:
                        _formaPago = formaDePago.Cuotas.ToString().Replace("Cuotas", "Adelanto de cuotas");;
                        break;
                }
                return _formaPago;
            }
        }
        #endregion

        #region Acceso a Datos
        public int Save()
        {
            cRegistroPagoDAO DAO = new cRegistroPagoDAO();
            return DAO.Save(this);
        }

        public static cRegistroPago Load(string id)
        {
            cRegistroPagoDAO DAO = new cRegistroPagoDAO();
            return DAO.Load(id);
        }

        public static string GetCancelarRegistro(string _id)
        {
            cRegistroPagoDAO dao = new cRegistroPagoDAO();
            return dao.GetCancelarRegistro(_id);
        }

        public static List<cRegistroPago> GetAllRegistrosByCC(string _idCC)
        {
            cRegistroPagoDAO dao = new cRegistroPagoDAO();
            return dao.GetAllRegistrosByCC(_idCC);
        }

        public static List<cRegistroPago> GetRegistros()
        {
            cRegistroPagoDAO dao = new cRegistroPagoDAO();
            return dao.GetRegistros();
        }

        public static List<cRegistroPago> GetRegistrosByIdCC(string _idCC)
        {
            cRegistroPagoDAO dao = new cRegistroPagoDAO();
            return dao.GetRegistrosByIdCC(_idCC);
        }

        public static List<cRegistroPago> GetRegistrosByIds(string _idCC, string ids)
        {
            cRegistroPagoDAO dao = new cRegistroPagoDAO();
            return dao.GetRegistrosByIds(_idCC, ids);
        }
        #endregion
    }
}
