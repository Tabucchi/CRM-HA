using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

public class cValorDolarDAO
    {
        public string GetTable { get { return "tValorDolar"; } }

        public List<cAtributo> AttributesClass(cValorDolar valor)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("valor", valor.ValorDolar));
            lista.Add(new cAtributo("registerDate", valor.RegisterDate));
            lista.Add(new cAtributo("papelera", valor.Papelera));
            return lista;
        }
    
        public int Save(cValorDolar valor)
        {
            if (string.IsNullOrEmpty(valor.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(valor));
            else
                return cDataBase.GetInstance().UpdateObject(valor.Id, GetTable, AttributesClass(valor));
        }

        public cValorDolar Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cValorDolar unidad = new cValorDolar();
            unidad.Id = Convert.ToString(atributos["id"]);
            unidad.ValorDolar = Convert.ToString(atributos["valor"]);
            unidad.RegisterDate = Convert.ToDateTime(atributos["registerDate"]);
            unidad.Papelera = Convert.ToInt16(atributos["papelera"]);
            return unidad;
        }

        public List<cValorDolar> GetValoresDolar()
        {
            List<cValorDolar> valores = new List<cValorDolar>();
            string query = "SELECT id FROM " + GetTable + " WHERE Papelera= " + (Int16)papelera.Activo + " Order by id DESC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                valores.Add(Load(Convert.ToString(idList[i])));
            }
            return valores;
        }

        public decimal LoadActualValue()
        {
            string query = "SELECT TOP 1 valor FROM tValorDolar WHERE papelera = '" + Convert.ToInt16(papelera.Activo).ToString() + "' ORDER BY id DESC";
            SqlCommand com = new SqlCommand(query);
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
        }
    }

