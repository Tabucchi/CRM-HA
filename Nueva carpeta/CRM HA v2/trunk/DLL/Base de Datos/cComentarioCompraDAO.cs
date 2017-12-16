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

public class cComentarioCompraDAO
{
    public string GetTable
    { get { return "tComentarioCompra"; } }

    public string GetOrderBy
    { get { return "id DESC"; } }

    public List<cAtributo> AttributesClass(cComentarioCompra comentario)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idUsuario", comentario.IdUsuario));
        lista.Add(new cAtributo("idCompra", comentario.IdCompra));
        lista.Add(new cAtributo("Descripcion", comentario.Descripcion));
        lista.Add(new cAtributo("Fecha", comentario.Fecha));
        lista.Add(new cAtributo("Tipo", comentario.Tipo));
        return lista;
    }

    public cComentarioCompra Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cComentarioCompra comentario = new cComentarioCompra();
        comentario.Id = id;
        comentario.IdUsuario = Convert.ToString(atributos["idUsuario"]);
        comentario.IdCompra = Convert.ToString(atributos["idCompra"]);
        comentario.Descripcion = Convert.ToString(atributos["Descripcion"]);
        comentario.Fecha = Convert.ToDateTime(atributos["Fecha"]);
        comentario.Tipo = Convert.ToString(atributos["Tipo"]);
        return comentario;
    }

    public int Save(cComentarioCompra comentario)
    {
        if (string.IsNullOrEmpty(comentario.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(comentario));
        else
            return cDataBase.GetInstance().UpdateObject(comentario.Id, GetTable, AttributesClass(comentario));
    }

    public ArrayList SearchByIdPedido(string id)
    {
        ArrayList comentarios = new ArrayList();
        string query = "SELECT id FROM " + GetTable + " WHERE idCompra = '" + id + "'";
        query += " ORDER BY " + GetOrderBy;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            comentarios.Add(Load(Convert.ToString(idList[i])));
        }
        return comentarios;
    }
}
