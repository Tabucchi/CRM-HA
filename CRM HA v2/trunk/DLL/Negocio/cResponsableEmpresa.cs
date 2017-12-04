using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DLL.Negocio
{
    public class cResponsableEmpresa
    {
        private string id;        
        private string idEmpresa;        
        private string idUsuario;
        
        #region Propiedades
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string IdEmpresa
        {
            get { return idEmpresa; }
            set { idEmpresa = value; }
        }

        public string IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        public string GetNombreEmpresa
        {
            get { return cEmpresa.Load(IdEmpresa).Nombre; }
        }

        public string GetNombreUsuario
        {
            get { return cUsuario.Load(IdUsuario).Nombre; }
        }
        #endregion

        public int Save()
        {
            cResponsableEmpresaDAO DAO = new cResponsableEmpresaDAO();
            return DAO.Save(this);
        }

        public static cResponsableEmpresa Load(string id)
        {
            cResponsableEmpresaDAO DAO = new cResponsableEmpresaDAO();
            return DAO.Load(id);
        }

        public string Eliminar(string id)
        {
            cResponsableEmpresaDAO DAO = new cResponsableEmpresaDAO();
            return DAO.Eliminar(id);
        }

        public static List<cResponsableEmpresa> Search(string idEmpresa, string idUsuario)
        {
            cResponsableEmpresaDAO DAO = new cResponsableEmpresaDAO();
            return DAO.Search(idEmpresa, idUsuario);
        }

        public static DataTable reporteXLS()
        {
            cResponsableEmpresaDAO DAO = new cResponsableEmpresaDAO();
            return DAO.reporteXLS();
        }

        public static string GetResponsable(string idEmpresa)
        {
            cResponsableEmpresaDAO DAO = new cResponsableEmpresaDAO();
            return DAO.GetResponsable(idEmpresa);
        }
    }
}
