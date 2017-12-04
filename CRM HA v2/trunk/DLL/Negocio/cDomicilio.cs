using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cDomicilio
    {
        private string id;
        private string calle;
        private string direccion;
        private string codPostal;
        private string idProvincia;
        private string ciudad;
        
        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Calle
        {
            get { return calle; }
            set { calle = value; }
        }
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }
        public string CodPostal
        {
            get { return codPostal; }
            set { codPostal = value; }
        }
        public string IdProvincia
        {
            get { return idProvincia; }
            set { idProvincia = value; }
        }
        public string Ciudad
        {
            get { return ciudad; }
            set { ciudad = value; }
        }
        #endregion

        public cDomicilio()
        {
        }

        public cDomicilio(string _calle, string _direccion, string _codPostal, string _idProvincia, string _ciudad)
        {
            calle = _calle;
            direccion = _direccion;
            codPostal = _codPostal;
            idProvincia = _idProvincia;
            ciudad = _ciudad;
        }

        public int Save()
        {
            cDomicilioDAO DAO = new cDomicilioDAO();
            return DAO.Save(this);
        }

        public static cDomicilio Load(string id)
        {
            cDomicilioDAO DAO = new cDomicilioDAO();
            return DAO.Load(id);
        }
    }
}
