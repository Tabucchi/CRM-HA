using DLL.Auxiliares;
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
    public class cUVADAO
    {
        public string GetTable { get { return "tUVA"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cUVA valor)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("valor", valor.Valor));
            lista.Add(new cAtributo("registerDate", valor.Fecha));
            lista.Add(new cAtributo("papelera", valor.Papelera));
            return lista;
        }

        public int Save(cUVA valor)
        {
            if (string.IsNullOrEmpty(valor.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(valor));
            else
                return cDataBase.GetInstance().UpdateObject(valor.Id, GetTable, AttributesClass(valor));
        }

        public cUVA Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cUVA uva = new cUVA();
            uva.Id = Convert.ToString(atributos["id"]);
            uva.Valor = Convert.ToDecimal(atributos["valor"]);
            uva.Fecha = Convert.ToDateTime(atributos["registerDate"]);
            uva.Papelera = Convert.ToInt16(atributos["papelera"]);
            return uva;
        }

        public ArrayList LoadTable()
        {
            ArrayList valores = new ArrayList();
            ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
            for (int i = 0; idList.Count > i; i++)
            {
                valores.Add(Load(Convert.ToString(idList[i])));
            }
            return valores;
        }

        public decimal GetLastIdIndice()
        {
            string query = "SELECT TOP (1) id FROM " + GetTable + " ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public decimal GetLastValorIndice()
        {
            string query = "SELECT TOP (1) valor FROM " + GetTable + " ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public List<cUVA> GetIndiceUVA()
        {
            List<cUVA> indices = new List<cUVA>();
            string query = "SELECT id FROM " + GetTable + " Order by id DESC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                indices.Add(Load(Convert.ToString(idList[i])));
            }
            return indices;
        }

        public string GetPreviousIndice()
        {
            DateTime min = new DateTime(DateTime.Today.Month == 2 ? DateTime.Today.Year - 1 : DateTime.Today.Year, DateTime.Today.AddMonths(-2).Month, 1);
            DateTime max = new DateTime(DateTime.Today.Month == 2 ? DateTime.Today.Year - 1 : DateTime.Today.Year, DateTime.Today.AddMonths(-2).Month, 28);

            string query = "SELECT id FROM " + GetTable + " WHERE registerDate BETWEEN @fechaDesde AND @fechaHasta ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = min;

            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = max;
            com.CommandText = query.ToString();
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public string GetLastIndiceMonth()
        {
            DateTime min = new DateTime(DateTime.Today.Month == 2 ? DateTime.Today.Year - 1 : DateTime.Today.Year, DateTime.Today.AddMonths(-2).Month, 15);
            //DateTime max = new DateTime(DateTime.Today.Month == 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year, DateTime.Today.Month == 12 ? 1 : DateTime.Today.Month + 1, 14);
            DateTime max = new DateTime(DateTime.Today.Month == 1 ? DateTime.Today.Year - 1 : DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 14);

            string query = "SELECT id FROM " + GetTable + " WHERE registerDate BETWEEN @fechaDesde AND @fechaHasta ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = min;

            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = max;
            com.CommandText = query.ToString();
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public string GetIdIndiceByFecha(DateTime fecha)
        {
            string query = "SELECT TOP (1) id FROM " + GetTable + " WHERE registerDate > @fechaDesde ORDER BY id ASC";

            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = fecha;

            com.CommandText = query.ToString();
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public decimal GetIndiceByFecha(DateTime fecha)
        {
            DateTime min = new DateTime(fecha.Month == 1 ? 12 : fecha.Year, fecha.Month == 1 ? 12 : fecha.AddMonths(-1).Month, 25);
            DateTime max = new DateTime(fecha.Year, fecha.Month, 24);

            string query = "SELECT TOP (1) id FROM " + GetTable + " WHERE registerDate BETWEEN @fechaDesde AND @fechaHasta ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = min;

            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = max;
            com.CommandText = query.ToString();
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
        }

        
    }
}
