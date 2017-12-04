using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cArchivoDAO
    {
        public string GetTable
        { get { return "tArchivo"; } }

        public string GetOrderBy
        { get { return "id ASC"; } }

        public List<cAtributo> AttributesClass(cArchivo arch)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("descripcion", arch.Descripcion));
            lista.Add(new cAtributo("extension", arch.Extension));
            lista.Add(new cAtributo("archivo", arch.Archivo));
            lista.Add(new cAtributo("fecha", arch.Fecha));
            lista.Add(new cAtributo("idProyecto", arch.IdProyecto));
            lista.Add(new cAtributo("papelera", arch.Papelera));
            return lista;
        }

        public int Save(cArchivo arch)
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
            arch.Descripcion = Convert.ToString(atributos["descripcion"]);
            arch.Extension = Convert.ToString(atributos["extension"]);
            arch.Archivo = (byte[])atributos["archivo"];
            arch.Fecha = Convert.ToDateTime(atributos["fecha"]);
            arch.IdProyecto = Convert.ToString(atributos["idProyecto"]);
            arch.Papelera = Convert.ToInt16(atributos["papelera"]);
            return arch;
        }

        public byte[] GetFile(string idProyecto)
        {
            //get
            //{
                string query = "SELECT archivo FROM " + GetTable + " WHERE idProyecto = " + idProyecto;
                SqlCommand cmd = new SqlCommand(query);
                return cDataBase.GetInstance().ExecuteScalarByte(cmd);
            //}
        }

        public byte[] GetFile2
        {
            get
            {
            string query = "SELECT archivo FROM " + GetTable + " WHERE idProyecto = 73";
            SqlCommand cmd = new SqlCommand(query);
            return cDataBase.GetInstance().ExecuteScalarByte(cmd);
            }
        }
    }
}
