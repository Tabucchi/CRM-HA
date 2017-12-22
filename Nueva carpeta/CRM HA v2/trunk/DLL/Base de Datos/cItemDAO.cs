using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;

public class cItemDAO
{
    public string GetTable
    { get { return "tItem"; } }

    public string GetOrderBy
    { get { return "id ASC"; } }

    public List<cAtributo> AttributesClass(cItem item)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("Cantidad", item.Cantidad));
        lista.Add(new cAtributo("Descripcion", item.Descripcion));
        lista.Add(new cAtributo("ImporteProveedor", item.ImporteProveedor));
        lista.Add(new cAtributo("ImporteCliente", item.ImporteCliente));
        lista.Add(new cAtributo("IdProveedor", item.IdProveedor));
        lista.Add(new cAtributo("IdCompra", item.IdCompra));
        lista.Add(new cAtributo("NroPedidoProveedor", item.NroPedidoProveedor));
        return lista;
    }

    public cItem Load(string id)
    {
        try
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cItem item = new cItem();
            item.Id = Convert.ToString(atributos["id"]);
            item.Cantidad = Convert.ToString(atributos["cantidad"]);
            item.Descripcion = Convert.ToString(atributos["descripcion"]);
            item.ImporteProveedor = Convert.ToString(atributos["importeProveedor"]);
            item.ImporteCliente = Convert.ToString(atributos["importeCliente"]);
            item.IdProveedor = Convert.ToString(atributos["idProveedor"]);
            item.IdCompra = Convert.ToString(atributos["idCompra"]);
            item.NroPedidoProveedor = Convert.ToString(atributos["NroPedidoProveedor"]);
            return item;
        }
        catch
        {
            return null;
        }
    }

    public ArrayList LoadTable()
    {
        ArrayList items = new ArrayList();
        SqlCommand cmd = new SqlCommand("SELECT id FROM " + GetTable);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            items.Add(Load(Convert.ToString(idList[i])));
        }
        return items;
    }

    public int Save(cItem item)
    {
        if (string.IsNullOrEmpty(item.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(item));
        else
            return cDataBase.GetInstance().UpdateObject(item.Id, GetTable, AttributesClass(item));
    }

    public bool Delete(string id)
    {
        return cDataBase.GetInstance().DeleteObject(id, GetTable.ToString());
    }

    public List<cItem> GetItems(string idCompra)
    {
        List<cItem> compras = new List<cItem>();
        string query = "SELECT id FROM " + GetTable + " WHERE idCompra= " + idCompra + " Order by id ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            compras.Add(Load(Convert.ToString(idList[i])));
        }
        return compras;
    }
}
