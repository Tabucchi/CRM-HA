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
    public class cArchivoCuotasObraDAO
    {
        public string GetTable
        { get { return "tArchivoCuotasObra"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cArchivoCuotasObra arch)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("fecha", arch.Fecha));
            lista.Add(new cAtributo("archivo", arch.Archivo));
            lista.Add(new cAtributo("papelera", arch.Papelera));
            return lista;
        }

        public int Save(cArchivoCuotasObra arch)
        {
            if (string.IsNullOrEmpty(arch.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(arch));
            else
                return cDataBase.GetInstance().UpdateObject(arch.Id, GetTable, AttributesClass(arch));
        }

        public cArchivoCuotasObra Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cArchivoCuotasObra arch = new cArchivoCuotasObra();
            arch.Id = Convert.ToString(atributos["id"]);
            arch.Fecha = Convert.ToDateTime(atributos["fecha"]);
            arch.Archivo = (byte[])atributos["archivo"];
            arch.Papelera = Convert.ToInt16(atributos["papelera"]);
            return arch;
        }

        public byte[] GetFile(string _id)
        {
            string query = "SELECT archivo FROM " + GetTable + " WHERE id = " + _id;
            SqlCommand cmd = new SqlCommand(query);
            return cDataBase.GetInstance().ExecuteScalarByte(cmd);
        }

        public List<cArchivoCuotasObra> Search(string _desde, string _hasta)
        {
            List<cArchivoCuotasObra> ovs = new List<cArchivoCuotasObra>();
            string blanco = " ";
            string query = "SELECT id FROM " + GetTable + " WHERE papelera=" + (Int16)papelera.Activo;
            
            if (_desde != null && _hasta != null)
                query += " AND fecha BETWEEN @fechaDesde AND @fechaHasta";

            query += blanco + "ORDER BY fecha ASC";

            SqlCommand com = new SqlCommand(query);

            if (_desde != null && _hasta != null)
            {
                com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
                com.Parameters["@fechaDesde"].Value = _desde;

                com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
                com.Parameters["@fechaHasta"].Value = _hasta;
            }

            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                ovs.Add(Load(Convert.ToString(idList[i])));
            }
            return ovs;
        }
    }
}
