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
    public class cIndiceCACDAO
    {
        public string GetTable
        { get { return "tCAC"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cIndiceCAC indice)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("fecha", indice.Fecha));
            lista.Add(new cAtributo("valor", indice.Valor));
            return lista;
        }

        public int Save(cIndiceCAC cliente)
        {
            if (string.IsNullOrEmpty(cliente.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(cliente));
            else
                return cDataBase.GetInstance().UpdateObject(cliente.Id, GetTable, AttributesClass(cliente));
        }

        public cIndiceCAC Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cIndiceCAC indice = new cIndiceCAC();
            indice.Id = Convert.ToString(atributos["id"]);
            indice.Fecha = Convert.ToDateTime(atributos["fecha"]);
            indice.Valor = Convert.ToDecimal(atributos["valor"]);
            return indice;
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

        public List<cIndiceCAC> GetIndiceCAC()
        {
            List<cIndiceCAC> indices = new List<cIndiceCAC>();
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
            DateTime min = new DateTime(DateTime.Today.Month == 3 ? DateTime.Today.Year - 1 : DateTime.Today.Year, DateTime.Today.AddMonths(-3).Month, 15);
            DateTime max = new DateTime(DateTime.Today.Month == 1 ? DateTime.Today.Year - 1 : DateTime.Today.Year, DateTime.Today.AddMonths(-2).Month, 14);

            string query = "SELECT id FROM " + GetTable + " WHERE fecha BETWEEN @fechaDesde AND @fechaHasta ORDER BY id DESC";
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
            
            string query = "SELECT id FROM " + GetTable + " WHERE fecha BETWEEN @fechaDesde AND @fechaHasta ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = min;

            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = max;
            com.CommandText = query.ToString();
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public decimal GetLastIndice()
        {
            //DateTime min = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 15);
            //DateTime max = new DateTime(DateTime.Today.Month == 12 ? DateTime.Today.Year + 1 : DateTime.Today.Year, DateTime.Today.Month == 12 ? 1 : DateTime.Today.Month + 1, 14);

            //string query = "SELECT TOP (1) id FROM " + GetTable + " WHERE fecha BETWEEN @fechaDesde AND @fechaHasta ORDER BY id DESC";
            string query = "SELECT TOP (1) id FROM " + GetTable + " ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            /*com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            com.Parameters["@fechaDesde"].Value = min;

            com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            com.Parameters["@fechaHasta"].Value = max;*/
            com.CommandText = query.ToString();
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public decimal GetLastValueIndice()
        {
            string query = "SELECT TOP (1) valor FROM " + GetTable + " ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
        }

        public decimal GetIndiceByFecha(DateTime fecha)
        {
            DateTime min;
            DateTime max;
            /*if (fecha.Day < 15)
            {
                min = new DateTime(fecha.Month == 1 ? fecha.Year - 1 : fecha.Year, fecha.Month == 1 ? 12 : fecha.Month - 1, 15);
                max = new DateTime(fecha.Year, fecha.Month, 14);                
            }
            else
            {
                min = new DateTime(fecha.Year, fecha.Month, 15);
                max = new DateTime(fecha.Month == 12 ? fecha.Year + 1 : fecha.Year, fecha.Month == 12 ? 1 : fecha.Month + 1, 14);
            }*/

            int _month = 0;
            int _year = 0;
            if (fecha.Month == 1)
            {
                _month = 11;
                _year = fecha.Year - 1;
            }
            else
            {
                _month = fecha.Month - 2;
                _year = fecha.Year;
            }

            if (fecha.Month == 2)
            {
                _month = 12;
                _year = fecha.Year - 1;
            }
            
            min = new DateTime(_year, _month, 15);
            max = new DateTime(fecha.Month == 1 ? fecha.Year - 1 : fecha.Year, fecha.Month == 1 ? 12 : fecha.Month - 1, 14);   

            string query = "SELECT TOP (1) id FROM " + GetTable + " WHERE fecha BETWEEN @fechaDesde AND @fechaHasta ORDER BY id DESC";
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
