using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cAutorizacionUVA
    {
        private string id;
        private DateTime fechaVencimiento;
        private string header;
        private Int16 _papelera;

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public DateTime FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }
        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        public Int16 Papelera
        {
            get { return _papelera; }
            set { _papelera = value; }
        }
        #endregion

        public cAutorizacionUVA() { }

        public cAutorizacionUVA(DateTime _fechaVenc, string _header)
        {
            fechaVencimiento = _fechaVenc;
            header = _header;
            _papelera = (Int16)papelera.Activo;
            this.Save();
        }

        #region Acceso a Datos
        public int Save()
        {
            cAutorizacionUVADAO DAO = new cAutorizacionUVADAO();
            return DAO.Save(this);
        }

        public static cAutorizacionUVA Load(string id)
        {
            cAutorizacionUVADAO DAO = new cAutorizacionUVADAO();
            return DAO.Load(id);
        }

        public static cAutorizacionUVA GetLast()
        {
            cAutorizacionUVADAO DAO = new cAutorizacionUVADAO();
            return DAO.GetLast();
        }

        public static DateTime GetAutorizacionByFecha()
        {
            cAutorizacionUVADAO DAO = new cAutorizacionUVADAO();
            return DAO.GetAutorizacionByFecha();
        }
        #endregion
    }
}
