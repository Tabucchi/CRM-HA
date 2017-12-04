using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;

public class cCategoriaInventarioDAO
{
    public string GetTable
    { get { return "tCategoriaInventario"; } }

    public string GetOrderBy
    { get { return "id DESC"; } }

    public List<cAtributo> AttributesClass(cCategoriaInventario inventario)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("descripcion", inventario.Descripcion));
        lista.Add(new cAtributo("numero", inventario.Numero));
        lista.Add(new cAtributo("contador", inventario.Contador));
        return lista;
    }

    public int Save(cCategoriaInventario inventario)
    {
        if (string.IsNullOrEmpty(inventario.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(inventario));
        else
            return cDataBase.GetInstance().UpdateObject(inventario.Id, GetTable, AttributesClass(inventario));
    }

    public cCategoriaInventario Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cCategoriaInventario inventario = new cCategoriaInventario();
        inventario.Id = Convert.ToString(id);
        inventario.Descripcion = Convert.ToString(atributos["descripcion"]);
        inventario.Numero = Convert.ToString(atributos["numero"]);
        inventario.Contador = Convert.ToString(atributos["contador"]);
        return inventario;
    }

    public List<cCategoriaInventario> GetCategorias()
    {
        List<cCategoriaInventario> estados = new List<cCategoriaInventario>();
        string query = "SELECT id FROM " + GetTable + " Order by id ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            estados.Add(Load(Convert.ToString(idList[i])));
        }
        return estados;
    }
}
