using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cImagenCuota
    {
        private string id;
        private Int32 idCC;        
        private string idCuota;
        private string descripcion;        
        private byte[] imagen;
        private Int16 papelera;
        
        public cImagenCuota()
        {
        }

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public Int32 IdCC
        {
            get { return idCC; }
            set { idCC = value; }
        }
        public string IdCuota
        {
            get { return idCuota; }
            set { idCuota = value; }
        }
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        public byte[] Imagen
        {
            get { return imagen; }
            set { imagen = value; }
        }
        public Int16 Papelera
        {
            get { return papelera; }
            set { papelera = value; }
        }
        #endregion

        #region Acceso a Datos
        public static cImagenCuota Load(string id)
        {
            cImagenCuotaDAO DAO = new cImagenCuotaDAO();
            return DAO.Load(id);
        }

        public int Save()
        {
            cImagenCuotaDAO DAO = new cImagenCuotaDAO();
            return DAO.Save(this);
        }
        public static string Existe(string _idCuota)
        {
            cImagenCuotaDAO DAO = new cImagenCuotaDAO();
            return DAO.Existe(_idCuota);
        }

        public static string GetCancelarImagen(string _id)
        {
            cImagenCuotaDAO dao = new cImagenCuotaDAO();
            return dao.GetCancelarImagen(_id);
        }
        public static cImagenCuota GetImagenById(string _id)
        {
            cImagenCuotaDAO DAO = new cImagenCuotaDAO();
            return DAO.GetImagenById(_id);
        }

        public static cImagenCuota GetImagenByCuota(string _idCuota)
        {
            cImagenCuotaDAO DAO = new cImagenCuotaDAO();
            return DAO.GetImagenByCuota(_idCuota);
        }

        public static cImagenCuota GetImagenByRegistroPago(string _idRegistro)
        {
            cImagenCuotaDAO DAO = new cImagenCuotaDAO();
            return DAO.GetImagenByRegistroPago(_idRegistro);
        }
        #endregion

    }
}
