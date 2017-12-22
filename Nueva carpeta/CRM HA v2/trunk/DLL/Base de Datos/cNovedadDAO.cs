using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Data.SqlClient;

public class cNovedadDAO
{
    public string GetTable
    { get { return "tNovedad"; } }

    public string GetOrderBy
    { get { return "id DESC"; } }

    public List<cAtributo> AttributesClass(cNovedad novedad)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idUsuario", novedad.IdUsuario));
        lista.Add(new cAtributo("descripcion", novedad.Descripcion));
        lista.Add(new cAtributo("fecha", novedad.Fecha));
        return lista;
    }

    public int Save(cNovedad novedad)
    {
        if (string.IsNullOrEmpty(novedad.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(novedad));
        else
            return cDataBase.GetInstance().UpdateObject(novedad.Id, GetTable, AttributesClass(novedad));
    }

    public cNovedad Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cNovedad novedad = new cNovedad();
        novedad.Id = id;
        novedad.IdUsuario = Convert.ToString(atributos["idUsuario"]);
        novedad.Descripcion = Convert.ToString(atributos["descripcion"]);
        novedad.Fecha = Convert.ToDateTime(atributos["fecha"]);
        return novedad;
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

    public ArrayList SearchByIdUsuario(string idUsuario)
    {
        ArrayList comentarios = new ArrayList();
        string query = "SELECT id FROM " + GetTable + " WHERE idUsuario = '" + idUsuario + "' ORDER BY " + GetOrderBy;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            comentarios.Add(Load(Convert.ToString(idList[i])));
        }
        return comentarios;
    }

    public ArrayList SearchLastFive()
    {
        ArrayList comentarios = new ArrayList();
        string query = "SELECT TOP(2) id FROM " + GetTable + " ORDER BY " + GetOrderBy;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            comentarios.Add(Load(Convert.ToString(idList[i])));
        }
        return comentarios;
    }

    public List<cNovedad> GetNovedades()
    {
        List<cNovedad> novedades = new List<cNovedad>();
        string query = "SELECT id FROM " + GetTable;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;

        for (int i = 0; idList.Count > i; i++)
        {
            novedades.Add(Load(Convert.ToString(idList[i])));
            string query2 = "SELECT nombre FROM tUsuario c WHERE " + novedades[i].IdUsuario + "=c.id";
            SqlCommand cmd = new SqlCommand(query2);
            string usuario = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
            novedades[i].Usuario = usuario;

            //DateTime fecha = Convert.ToDateTime(novedades[i].Fecha);
            //novedades[i].Fecha = fecha;

            string desc= novedades[i].Descripcion.Replace("<b>", "").Replace("</b>","");
            novedades[i].Descripcion = desc;
        }
        return novedades;
    }

    public string DeleteNovedad(string id)
    {
        string query = "DELETE FROM tNovedad WHERE id= '" + id + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }
}
