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

public class cComentarioDAO
{
    public string GetTable
    { get { return "tComentario"; } }

    public string GetOrderBy
    { get { return "id ASC"; } }

    public List<cAtributo> AttributesClass(cComentario comentario)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idUsuario", comentario.IdUsuario));
        lista.Add(new cAtributo("idPedido", comentario.IdPedido));
        lista.Add(new cAtributo("Descripcion", comentario.Descripcion));
        lista.Add(new cAtributo("Fecha", comentario.Fecha));
        return lista;
    }

    public int Insert(cComentario comentario)
    {
        return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(comentario));
    }

    public cComentario Load(int id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cComentario comentario = new cComentario();
        comentario.Id = id;
        comentario.IdUsuario = Convert.ToInt32(atributos["idUsuario"]);
        comentario.IdPedido = Convert.ToInt32(atributos["idPedido"]);
        comentario.Descripcion = Convert.ToString(atributos["Descripcion"]);
        comentario.Fecha = Convert.ToDateTime(atributos["Fecha"]);
        return comentario;
    }

    public ArrayList LoadTable()
    {
        ArrayList comentarios = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            comentarios.Add(Load(Convert.ToInt32(idList[i])));
        }
        return comentarios;
    }

    public ArrayList SearchByIdPedido(int id)
    {
        ArrayList comentarios = new ArrayList();
        string query = "SELECT id FROM " + GetTable + " WHERE idPedido = " + id + " ORDER BY " + GetOrderBy;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            comentarios.Add(Load(Convert.ToInt32(idList[i])));
        }
        return comentarios;
    }
}
