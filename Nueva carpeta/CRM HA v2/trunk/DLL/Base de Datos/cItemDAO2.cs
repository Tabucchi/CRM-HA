using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

public class cItemDAO2
{
    public string GetTable
    { get { return "tItem"; } }

    public string GetOrderBy
    { get { return "id DESC"; } }

	public List<cAtributo> AttributesClass(cItem2 item)
	{
	    List<cAtributo> lista=new List<cAtributo>();
        lista.Add(new cAtributo("idPedido", item.IdPedido));
        lista.Add(new cAtributo("Nombre", item.Nombre));
        lista.Add(new cAtributo("Descripcion", item.Descripcion));
        lista.Add(new cAtributo("Cantidad", item.Cantidad));
        lista.Add(new cAtributo("Costo", item.Costo));
        lista.Add(new cAtributo("Precio", item.PrecioCliente));
        lista.Add(new cAtributo("Fecha", item.Fecha));
        lista.Add(new cAtributo("idUsuario", item.IdUsuario));
        lista.Add(new cAtributo("idEstado", item.IdEstado));
        lista.Add(new cAtributo("idAprobo", item.IdAprobo));
	    return lista;
	}

    public cItem2 Load(string id)
    {
        try
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cItem2 item = new cItem2();
           // item.Id = Convert.ToString(atributos["id"]);
            item.Id = id;
            item.IdPedido = Convert.ToString(atributos["idPedido"]);
            item.Nombre = Convert.ToString(atributos["Nombre"]);
            item.Descripcion = Convert.ToString(atributos["Descripcion"]);
            item.Cantidad = Convert.ToInt16(atributos["Cantidad"]);
            item.Costo = Convert.ToString(atributos["Costo"]);
            item.PrecioCliente = Convert.ToString(atributos["Precio"]);
            
            if (atributos["Fecha"] == DBNull.Value)
                item.Fecha = null;
            else
                item.Fecha = Convert.ToDateTime(atributos["Fecha"]);  
              
            item.IdUsuario = Convert.ToString(atributos["idUsuario"]);
            item.IdAprobo = Convert.ToString(atributos["IdAprobo"]);
            return item;
        }
        catch
        {
            return null;
        }
    }

    public ArrayList LoadTable()
    {
        ArrayList items=new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        for(int i=0;idList.Count>i;i++)
        {
            items.Add(Load(Convert.ToString(idList[i])));
        }
        return items;
    }

    public int Save(cItem2 item)
    {
        if (string.IsNullOrEmpty(item.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(item));
        else
            return cDataBase.GetInstance().UpdateObject(item.Id, GetTable, AttributesClass(item));
    }

    public List<cItem2> GetItems()
    {
        List<cItem2> items = new List<cItem2>();
        string query = "SELECT id FROM " + GetTable + " order by idPedido desc";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            items.Add(Load(Convert.ToString(idList[i])));
        }
        return items;
    }

    public List<cItem2> GetItemsPedido(string idPedido)
    {
        List<cItem2> items = new List<cItem2>();
        string query = "SELECT id FROM " + GetTable + " WHERE IdPedido= " + idPedido;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for(int i = 0; idList.Count > i; i++)
        {
            items.Add(Load(Convert.ToString(idList[i])));
        }
        return items;
    }

    public List<cItem2> Search(string idEmpresa, DateTime? fechaDesde, DateTime? fechaHasta)
    {
        List<cItem2> items = new List<cItem2>();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();

        query.Append("SELECT i.id,i.idPedido, i.Nombre, i.Descripcion, i.Cantidad, i.Costo, i.Precio, i.Fecha, i.idUsuario, i.idEstado, i.idAprobo FROM tItem i INNER JOIN tPedido ON i.idPedido = tPedido.id");

        if (idEmpresa != "0")
            query.Append(" WHERE tPedido.idEmpresa=" + idEmpresa);
        else
            query.Append(" WHERE ");

        if (fechaDesde != null)
        {
            if (idEmpresa != "0")
                query.Append(" AND i.Fecha >= @FechaDesde");
            else
                query.Append(" i.Fecha >= @FechaDesde");
            cmd.Parameters.Add("@FechaDesde", SqlDbType.DateTime);
            cmd.Parameters["@FechaDesde"].Value = fechaDesde;
        }

        if (fechaHasta != null)
        {
            query.Append(" AND i.Fecha <= @FechaHasta");
            cmd.Parameters.Add("@FechaHasta", SqlDbType.DateTime);
            cmd.Parameters["@FechaHasta"].Value = fechaHasta;
        }

        cmd.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            items.Add(Load(Convert.ToString(idList[i])));
        }
        return items;
    }
}