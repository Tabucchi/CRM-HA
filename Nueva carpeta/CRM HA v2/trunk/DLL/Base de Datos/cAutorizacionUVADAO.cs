using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cAutorizacionUVADAO
    {
        public string GetTable { get { return "tAutorizacionUVA"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cAutorizacionUVA valor)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("fechaVencimiento", valor.FechaVencimiento));
            lista.Add(new cAtributo("header", valor.Header));
            lista.Add(new cAtributo("papelera", valor.Papelera));
            return lista;
        }

        public int Save(cAutorizacionUVA valor)
        {
            if (string.IsNullOrEmpty(valor.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(valor));
            else
                return cDataBase.GetInstance().UpdateObject(valor.Id, GetTable, AttributesClass(valor));
        }

        public cAutorizacionUVA Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cAutorizacionUVA uva = new cAutorizacionUVA();
            uva.Id = Convert.ToString(atributos["id"]);
            uva.FechaVencimiento = Convert.ToDateTime(atributos["fechaVencimiento"]);
            uva.Header = Convert.ToString(atributos["header"]);
            uva.Papelera = Convert.ToInt16(atributos["papelera"]);
            return uva;
        }

        public cAutorizacionUVA GetLast()
        {
            string query = "SELECT TOP (1) id FROM " + GetTable + " WHERE papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";

            SqlCommand com = new SqlCommand(query);
            string id = cDataBase.GetInstance().ExecuteScalar(com);
            if (id == null)
                return null;
            else
                return Load(Convert.ToString(id));
        }

        public DateTime GetAutorizacionByFecha()
        {
            string query = "SELECT TOP (1) fechaVencimiento FROM " + GetTable + " WHERE papelera='" + (Int16)papelera.Activo + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            return Convert.ToDateTime(cDataBase.GetInstance().ExecuteScalar(com));
        }
    }
}
