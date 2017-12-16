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

public class cComentarioCCDAO
{
    public string GetTable
    { get { return "tComentarioCC"; } }

    public string GetOrderBy
    { get { return "id DESC"; } }

    public List<cAtributo> AttributesClass(cComentarioCC comentario)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idUsuario", comentario.IdUsuario));
        lista.Add(new cAtributo("idCuentaCorriente", comentario.IdCuentaCorriente));
        lista.Add(new cAtributo("idCuota", comentario.IdCuota));
        lista.Add(new cAtributo("Descripcion", comentario.Descripcion));
        lista.Add(new cAtributo("Fecha", comentario.Fecha));
        return lista;
    }

    public int Save(cComentarioCC comentario)
    {
        if (string.IsNullOrEmpty(comentario.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(comentario));
        else
            return cDataBase.GetInstance().UpdateObject(comentario.Id, GetTable, AttributesClass(comentario));
    }

    public cComentarioCC Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cComentarioCC comentario = new cComentarioCC();
        comentario.Id = id;
        comentario.IdUsuario = Convert.ToString(atributos["idUsuario"]);
        comentario.IdCuentaCorriente = Convert.ToString(atributos["idCuentaCorriente"]);
        comentario.IdCuota = Convert.ToString(atributos["idCuota"]);
        comentario.Descripcion = Convert.ToString(atributos["descripcion"]);
        comentario.Fecha = Convert.ToDateTime(atributos["fecha"]);
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
    
    public List<cComentarioCC> GetComentarios(string _idCuentaCorriente)
    {
        List<cComentarioCC> comentarios = new List<cComentarioCC>();
        string query = "SELECT id FROM " + GetTable + " WHERE idCuentaCorriente=" + _idCuentaCorriente + " Order by Fecha ASC";
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

    public List<cComentarioCC> GetComentariosByIdCuota(string _idCuota)
    {
        List<cComentarioCC> comentarios = new List<cComentarioCC>();
        string query = "SELECT id FROM " + GetTable + " WHERE idCuota=" + _idCuota + " Order by Fecha ASC";
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
