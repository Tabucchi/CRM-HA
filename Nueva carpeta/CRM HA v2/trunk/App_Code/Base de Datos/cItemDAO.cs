using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public class cItemDAO
{
    public string GetTable
    { get { return "tItem"; } }

    public string GetOrderBy
    { get { return "idPedido ASC"; } }

    public List<cAtributo> AttributesClass(cItem item)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idPedido", item.IdPedido));
        lista.Add(new cAtributo("Cantidad", item.Cantidad));
        lista.Add(new cAtributo("Nombre", item.Nombre));
        lista.Add(new cAtributo("Descripcion", item.Descripcion));
        lista.Add(new cAtributo("Costo", item.Costo));
        lista.Add(new cAtributo("Precio", item.Precio));
        return lista;
    }

    public int Insert(cItem item)
    {
        return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(item));
    }

    public bool Delete(int id)
    {
        return cDataBase.GetInstance().DeleteObject(Convert.ToString(id), GetTable);
    }

    public cItem Load(int id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cItem item = new cItem();
        item.Id = id;
        item.IdPedido = Convert.ToInt32(atributos["idPedido"]);
        item.Cantidad = Convert.ToInt32(atributos["Cantidad"]);
        item.Nombre = Convert.ToString(atributos["Nombre"]);
        item.Descripcion = Convert.ToString(atributos["Descripcion"]);
        item.Costo = Convert.ToString(atributos["Costo"]);
        item.Precio = Convert.ToString(atributos["Precio"]);
        return item;
    }

    public ArrayList SearchByIdPedido(int id)
    {
        ArrayList items = new ArrayList();
        string query = "SELECT id FROM " + GetTable + " WHERE idPedido = " + id + " ORDER BY " + GetOrderBy;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            items.Add(Load(Convert.ToInt32(idList[i])));
        }
        return items;
    }

    public ArrayList GetItems(DateTime fechaD, DateTime fechaH, string idEmpresa, string order)
    {
        ArrayList items = new ArrayList();
        SqlCommand com = new SqlCommand();
        
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        query.Append("SELECT i.id, p.Fecha, e.Nombre FROM tItem i INNER JOIN tPedido p ON i.idPedido = p.id");
        query.Append(" INNER JOIN tEmpresa e ON p.idEmpresa = e.id");
        query.Append(" WHERE (p.Fecha > @fechaD) AND (p.Fecha < @fechaH)");
        if (idEmpresa != "1") // 1 = 'todas'
            query.Append(" AND (p.idEmpresa = '"+ idEmpresa +"')");
        switch(order) { 
            case "Fecha":
                query.Append(" ORDER BY p.Fecha");
                break;
            case "Empresa":
                query.Append(" ORDER BY e.Nombre");
                break;
        }

        com.Parameters.Add("@fechaD", SqlDbType.DateTime);
        com.Parameters["@fechaD"].Value = fechaD;
        com.Parameters.Add("@fechaH", SqlDbType.DateTime);
        com.Parameters["@fechaH"].Value = fechaH;

        com.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            items.Add(Load(Convert.ToInt32(idList[i])));
        }
        return items;                     
    }
}
