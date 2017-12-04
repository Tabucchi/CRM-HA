using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cCuentaCorrienteUsuarioDAO
    {
        public string GetTable
        { get { return "tCuentaCorrienteUsuario"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cCuentaCorrienteUsuario cuentacorriente)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idEmpresa", cuentacorriente.IdEmpresa));
            lista.Add(new cAtributo("papelera", cuentacorriente.Papelera));
            return lista;
        }

        public cCuentaCorrienteUsuario Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cCuentaCorrienteUsuario ccu = new cCuentaCorrienteUsuario();
            ccu.Id = Convert.ToString(atributos["id"]);
            ccu.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
            ccu.Papelera = Convert.ToInt16(atributos["papelera"]);
            return ccu;
        }

        public int Save(cCuentaCorrienteUsuario ccu)
        {
            if (string.IsNullOrEmpty(ccu.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(ccu));
            else
                return cDataBase.GetInstance().UpdateObject(ccu.Id, GetTable, AttributesClass(ccu));
        }

        public List<cCuentaCorrienteUsuario> GetCuentaCorriente()
        {
            List<cCuentaCorrienteUsuario> cc = new List<cCuentaCorrienteUsuario>();
            string query = "SELECT cc.id FROM " + GetTable + " cc INNER JOIN tEmpresa eu ON cc.idEmpresa = eu.id WHERE cc.papelera = '" + (Int16)papelera.Activo + "' Order by eu.Apellido, eu.Nombre ASC";

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

        public List<cCuentaCorrienteUsuario> GetCuentaCorrienteConSaldo()
        {
            List<cCuentaCorrienteUsuario> cc = new List<cCuentaCorrienteUsuario>();
            string query = "SELECT cc.id,eu.Apellido, eu.Nombre FROM " + GetTable + " cc INNER JOIN tEmpresa eu ON cc.idEmpresa = eu.id ";
            query += " INNER JOIN tItemCCU i ON cc.id = i.idCuentaCorrienteUsuario WHERE cc.papelera = '" + (Int16)papelera.Activo + "' AND i.saldo <> 0 ";
            query += " GROUP BY cc.id,eu.Apellido, eu.Nombre Order by eu.Apellido, eu.Nombre ASC";            

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

        public List<cCuentaCorrienteUsuario> GetCuentaCorrienteConOperacionVenta()
        {
            List<cCuentaCorrienteUsuario> cc = new List<cCuentaCorrienteUsuario>();
            string query = "SELECT cc.id FROM " + GetTable + " cc INNER JOIN tEmpresa e ON cc.idEmpresa = e.id INNER JOIN tEmpresaUnidad eu ON e.id = eu.idEmpresa ";
            query += " INNER JOIN tOperacionVenta op ON op.id=eu.idOv WHERE cc.papelera = '1' GROUP BY cc.id";

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

        public List<cCuentaCorrienteUsuario> GetListCuentaCorrienteByIdEmpresa(string _idEmpresa)
        {
            List<cCuentaCorrienteUsuario> cc = new List<cCuentaCorrienteUsuario>();
            string query = "SELECT cc.id FROM " + GetTable + " cc INNER JOIN tEmpresa eu ON cc.idEmpresa = eu.id WHERE eu.id = '" + _idEmpresa + "' AND cc.papelera = '" + (Int16)papelera.Activo + "' Order by id ASC";

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

        public List<cCuentaCorrienteUsuario> GetListCuentaCorrienteByEmpresa(string nombre, string apellido)
        {
            List<cCuentaCorrienteUsuario> cc = new List<cCuentaCorrienteUsuario>();
            string query = "SELECT cc.id FROM " + GetTable + " cc INNER JOIN tEmpresa eu ON cc.idEmpresa = eu.id ";
            query += " WHERE eu.nombre like '%" + nombre + "%' AND eu.apellido like '%" + apellido + "%' AND cc.papelera = '" + (Int16)papelera.Activo + "' Order by id ASC";

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

        public string GetCuentaCorrienteByIdEmpresa(string _idEmpresa)
        {
            string query = "SELECT id FROM " + GetTable + " WHERE Papelera = '" + (Int16)papelera.Activo + "' AND idEmpresa= '" + _idEmpresa + "'";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public Dictionary<string, string> GetSaldoProyecto()
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            SqlCommand cmd = new SqlCommand();

            string query = "SELECT p.id, Sum(ccu.Saldo) as Saldo FROM tCuentaCorrienteUsuario ccu INNER JOIN tCuentaCorriente cc ON ccu.idCC = cc.id INNER JOIN tEmpresaUnidad eu ON cc.idEmpresaUnidad = eu.id INNER JOIN tProyecto p ON eu.idProyecto = p.id GROUP BY p.id";

            cmd.CommandText = query.ToString();
            DataSet dataSet = cDataBase.GetInstance().GetDataSet(cmd, GetTable);
            if (dataSet == null) return null;
            DataTable dt = dataSet.Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                d.Add(Convert.ToString(row["id"]), Convert.ToString(row["Saldo"]));
            }
            return d;
        }
    }
}

