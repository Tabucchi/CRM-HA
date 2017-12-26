using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cArchivoCuotasObra
    {
        private string id;
        private DateTime fecha; 
        private byte[] archivo;
        private Int16 _papelera;
        
        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public string GetFecha
        {
            get { return Fecha.ToString("dd/MM/yyyy"); }
        }
        public byte[] Archivo
        {
            get { return archivo; }
            set { archivo = value; }
        }

        public Int16 Papelera
        {
            get { return _papelera; }
            set { _papelera = value; }
        }
        #endregion

        public int Save()
        {
            cArchivoCuotasObraDAO DAO = new cArchivoCuotasObraDAO();
            return DAO.Save(this);
        }

        public static cArchivoCuotasObra Load(string id)
        {
            cArchivoCuotasObraDAO DAO = new cArchivoCuotasObraDAO();
            return DAO.Load(id);
        }

        public cArchivoCuotasObra()
        {
        }

        public cArchivoCuotasObra(byte[] _archivo)
        {
            fecha = DateTime.Now;
            archivo = _archivo;
            _papelera = (Int16)papelera.Activo;
            this.Save();
        }
        
        public static byte[] GetFile(string idProyecto)
        {
            cArchivoCuotasObraDAO DAO = new cArchivoCuotasObraDAO();
            return DAO.GetFile(idProyecto);
        }

        public static List<cArchivoCuotasObra> Search(string _desde, string _hasta)
        {
            cArchivoCuotasObraDAO DAO = new cArchivoCuotasObraDAO();
            return DAO.Search(_desde, _hasta);
        }
    }
}
