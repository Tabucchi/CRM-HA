using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cProyectoDAO
    {
        public string GetTable
        { get { return "tProyecto"; } }

        public string GetOrderBy
        { get { return "Descripcion ASC"; } }

        public List<cAtributo> AttributesClass(cProyecto proyecto)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("descripcion", proyecto.Descripcion));
            lista.Add(new cAtributo("supTotal", proyecto.SupTotal));
            lista.Add(new cAtributo("papelera", proyecto.Papelera));
            return lista;
        }

        public int Save(cProyecto proyecto)
        {
            if (string.IsNullOrEmpty(proyecto.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(proyecto));
            else
                return cDataBase.GetInstance().UpdateObject(proyecto.Id, GetTable, AttributesClass(proyecto));
        }

        public cProyecto Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cProyecto proyecto = new cProyecto();
            proyecto.Id = Convert.ToString(atributos["id"]);
            proyecto.Descripcion = Convert.ToString(atributos["descripcion"]);
            proyecto.SupTotal = Convert.ToDecimal(atributos["supTotal"]);
            proyecto.Papelera = Convert.ToInt16(atributos["papelera"]);
            return proyecto;
        }

        public ArrayList LoadTable()
        {
            ArrayList clientes = new ArrayList();
            string query = "SELECT id FROM " + GetTable + " WHERE Papelera = 1 ORDER BY " + GetOrderBy;
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                clientes.Add(Load(Convert.ToString(idList[i])));
            }
            return clientes;
        }

        public List<cProyecto> GetProyectos()
        {
            List<cProyecto> proyectos = new List<cProyecto>();
            string query = "SELECT id FROM " + GetTable + " WHERE Papelera= " + (Int16)papelera.Activo + " Order by Descripcion ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                proyectos.Add(Load(Convert.ToString(idList[i])));
            }
            return proyectos;
        }
        
        public string GetProyecto()
        {
            string query = "SELECT p.descripcion FROM tCuentaCorriente cc INNER JOIN tUnidad u ON cc.idUnidad = u.id INNER JOIN tProyecto p ON u.idProyecto = p.id;";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public List<cProyecto> GetProyectosSaldos()
        {
            List<cProyecto> empresas = new List<cProyecto>();
            string query = "SELECT p.id, e.Apellido, e.nombre, p.descripcion  FROM tEmpresa e INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa INNER JOIN tOperacionVenta op ON op.id=eu.idOv  INNER JOIN tFormaPagoOV fp ON op.id=fp.idOperacionVenta ";
            query += " INNER JOIN tCuota c ON c.idFormaPagoOV = fp.id INNER JOIN tProyecto p ON p.id = eu.idProyecto WHERE eu.papelera = '1' AND eu.idOv <> '-1' GROUP BY p.id, e.Apellido, e.nombre, p.descripcion ORDER BY e.Apellido, e.nombre, p.descripcion";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                empresas.Add(Load(Convert.ToString(idList[i])));
            }
            return empresas;
        }      
        
        public ArrayList GetProyectoByIdOperacionVenta(string _idOperacionVenta)
        {
            ArrayList unidades = new ArrayList();
            string query = null;
            if (_idOperacionVenta != "-1")
                query = "SELECT p.descripcion FROM tEmpresaUnidad eu INNER JOIN tProyecto p ON eu.idProyecto = p.id WHERE eu.idOv='" + _idOperacionVenta + "'";
            else
                query = "SELECT p.descripcion FROM tCuentaCorriente cc INNER JOIN tUnidad u ON cc.idUnidad = u.id INNER JOIN tProyecto p ON u.idProyecto = p.id;";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Convert.ToString(idList[i]));
            }
            return unidades;
        }

        public List<cProyecto> GetUnidadesGroupByIdProyecto(string _idOpv)
        {
            List<cProyecto> cc = new List<cProyecto>();
            string query = "SELECT p.id FROM tEmpresaUnidad eu INNER JOIN tProyecto p ON eu.idProyecto = p.id WHERE eu.idOv = '" + _idOpv + "' GROUP BY p.id";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }
    }
}
