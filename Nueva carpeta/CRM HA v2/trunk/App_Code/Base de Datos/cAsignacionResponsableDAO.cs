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

public class cAsignacionResponsableDAO
{
    public string GetTable
    { get { return "tAsignacionResponsable"; } }

    public string GetOrderBy
    { get { return "id ASC"; } }

    public List<cAtributo> AttributesClass(cAsignacionResponsable asignacion)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idResponsable", asignacion.IdResponsable));
        lista.Add(new cAtributo("idAsigno", asignacion.IdAsigno));
        lista.Add(new cAtributo("idPedido", asignacion.IdPedido));
        lista.Add(new cAtributo("Fecha", asignacion.Fecha));
        lista.Add(new cAtributo("Comentario", asignacion.Comentario));
        return lista;
    }

    public int Insert(cAsignacionResponsable asignacion)
    {
        return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(asignacion));
    }

    public cAsignacionResponsable Load(int id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cAsignacionResponsable asignacion = new cAsignacionResponsable();
        asignacion.Id = id;
        asignacion.IdResponsable = Convert.ToInt32(atributos["idResponsable"]);
        asignacion.IdAsigno = Convert.ToInt32(atributos["idAsigno"]);
        asignacion.IdPedido = Convert.ToInt32(atributos["idPedido"]);
        asignacion.Fecha = Convert.ToDateTime(atributos["Fecha"]);
        asignacion.Comentario = Convert.ToString(atributos["Comentario"]);
        return asignacion;
    }
}
