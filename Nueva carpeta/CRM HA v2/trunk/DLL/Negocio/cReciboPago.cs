using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cReciboPago
    {
        private string id;        
        private decimal gastoAdtvo;        
        private decimal total;        
        private decimal importePagado;        
        private decimal diferencia;        
        private decimal punitorio;
        
        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public decimal GastoAdtvo
        {
            get { return gastoAdtvo; }
            set { gastoAdtvo = value; }
        }
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }
        public decimal ImportePagado
        {
            get { return importePagado; }
            set { importePagado = value; }
        }
        public decimal Diferencia
        {
            get { return diferencia; }
            set { diferencia = value; }
        }
        public decimal Punitorio
        {
            get { return punitorio; }
            set { punitorio = value; }
        }
        #endregion

        #region Acceso a Datos
        public int Save()
        {
            cReciboPagoDAO DAO = new cReciboPagoDAO();
            return DAO.Save(this);
        }

        public static cRegistroPago Load(string id)
        {
            cRegistroPagoDAO DAO = new cRegistroPagoDAO();
            return DAO.Load(id);
        }
        #endregion
    }
}
