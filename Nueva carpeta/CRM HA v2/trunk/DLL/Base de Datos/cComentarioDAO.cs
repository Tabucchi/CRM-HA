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
    { get { return "id DESC"; } }

    public List<cAtributo> AttributesClass(cComentario comentario)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idUsuario", comentario.IdUsuario));
        lista.Add(new cAtributo("idPedido", comentario.IdPedido));
        lista.Add(new cAtributo("Descripcion", comentario.Descripcion));
        lista.Add(new cAtributo("Fecha", comentario.Fecha));
        lista.Add(new cAtributo("VisibilidadCliente", comentario.VisibilidadCliente));
        lista.Add(new cAtributo("Tipo", comentario.Tipo));
        return lista;
    }

    public int Save(cComentario comentario)
    {
        if (string.IsNullOrEmpty(comentario.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(comentario));
        else
            return cDataBase.GetInstance().UpdateObject(comentario.Id, GetTable, AttributesClass(comentario));
    }

    public cComentario Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cComentario comentario = new cComentario();
        comentario.Id = id;
        comentario.IdUsuario = Convert.ToString(atributos["idUsuario"]);
        comentario.IdPedido = Convert.ToString(atributos["idPedido"]);
        comentario.Descripcion = Convert.ToString(atributos["Descripcion"]);
        comentario.Fecha = Convert.ToDateTime(atributos["Fecha"]);
        comentario.VisibilidadCliente = Convert.ToBoolean(atributos["VisibilidadCliente"]);
        comentario.Tipo = Convert.ToString(atributos["Tipo"]);
        return comentario;
    }

    public ArrayList LoadTable()
    {
        ArrayList comentarios = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            comentarios.Add(Load(Convert.ToString(idList[i])));
        }
        return comentarios;
    }

    public ArrayList SearchByIdPedido(string id, bool visibleParaCliente)
    {
        ArrayList comentarios = new ArrayList();
        string query = "SELECT id FROM " + GetTable + " WHERE idPedido = '" + id + "'";
        if (visibleParaCliente)
            query += " AND visibilidadCliente = 'True'";
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

    public cComentario GetLastComentario(string idPedido)
    {
        try
        {
            string query = "SELECT id FROM tComentario WHERE (id = ";
            query += "(SELECT MAX(c.id) FROM tPedido p INNER JOIN tComentario c ";
            query += "ON p.id = c.idPedido WHERE (p.id = @idPedido)))";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.Add("@idPedido", SqlDbType.Int);
            cmd.Parameters["@idPedido"].Value = idPedido;
            string res = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
            return Load(res);
        }
        catch
        {
            return null;
        }
    }

    public List<cComentario> GetComentarios(string idPedido)
    {
        List<cComentario> comentarios = new List<cComentario>();
        string query = "SELECT id FROM " + GetTable + " WHERE idPedido=" + idPedido + " Order by Fecha ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            comentarios.Add(Load(Convert.ToString(idList[i])));
        }
        return comentarios;
    }
}
