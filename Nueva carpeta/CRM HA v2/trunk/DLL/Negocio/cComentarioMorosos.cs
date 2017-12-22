using DLL.Base_de_Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Negocio
{
    public class cComentarioMorosos
    {
        private string id;
        private string idCuentaCorriente;
        private string comentario;
        private DateTime fecha;
        private string idUsuario;

        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string IdCuentaCorriente
        {
            get { return idCuentaCorriente; }
            set { idCuentaCorriente = value; }
        }
        public string Comentario
        {
            get { return comentario; }
            set { comentario = value; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public string IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }
        public string GetUsuario
        {
            get { return cUsuario.Load(idUsuario).Nombre; }
        }
        #endregion

        public cComentarioMorosos(string _idCuentaCorriente, string _comentario, DateTime _fecha, string _idUsuario)
        {
            idCuentaCorriente = _idCuentaCorriente;
            comentario = _comentario;
            fecha = DateTime.Now;
            idUsuario = _idUsuario;
            this.Save();
        }

        public cComentarioMorosos()
        { }

        #region Acceso a Datos
        public int Save()
        {
            cComentarioMorososDAO DAO = new cComentarioMorososDAO();
            return DAO.Save(this);
        }

        public static cComentarioMorosos Load(string id)
        {
            cComentarioMorososDAO DAO = new cComentarioMorososDAO();
            return DAO.Load(id);
        }

        public static List<cComentarioMorosos> GetComentariosByIdCCC(string _idCC)
        {
            cComentarioMorososDAO DAO = new cComentarioMorososDAO();
            return DAO.GetComentariosByIdCCC(_idCC);
        }
        #endregion
    }
}
