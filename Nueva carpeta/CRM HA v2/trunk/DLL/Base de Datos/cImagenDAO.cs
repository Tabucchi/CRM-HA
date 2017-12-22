using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Data.SqlClient;

public class cImagenDAO
{
    public string GetTable
    { get { return "tImagen"; } }

    public List<cAtributo> AttributesClass(cImagen imagen)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("descripcion", imagen.Descripcion));
        lista.Add(new cAtributo("imagen", imagen.Imagen));
        return lista;
    }

    public cImagen Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cImagen accion = new cImagen();
        accion.Id = Convert.ToString(atributos["id"]);
        accion.Descripcion = Convert.ToString(atributos["descripcion"]);
        accion.Imagen = (byte[])atributos["imagen"];
        return accion;
    }

    public int Save(cImagen imagen)
    {
        if (string.IsNullOrEmpty(imagen.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(imagen));
        else
            return cDataBase.GetInstance().UpdateObject(imagen.Id, GetTable, AttributesClass(imagen));
    }

    public string Existe(string descripcion)
    {
        string query = "SELECT id FROM " + GetTable + " WHERE descripcion='" + descripcion + "'";

        SqlCommand com = new SqlCommand();
        com.CommandText = query.ToString();

        string existe = null;
        if (!string.IsNullOrEmpty(cDataBase.GetInstance().ExecuteScalar(com)))
            existe = cDataBase.GetInstance().ExecuteScalar(com);
        else
            existe = null;

        return existe;
    }
    public bool Delete(string id)
    {
        return cDataBase.GetInstance().DeleteObject(id, GetTable.ToString());
    }
}

