using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public cArchivo Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cArchivo arch = new cArchivo();
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
    }
}
