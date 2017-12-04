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
        lista.Add(new cAtributo("Fecha", asignacion.Fecha));
        lista.Add(new cAtributo("Comentario", asignacion.Comentario));
        lista.Add(new cAtributo("idPedido", asignacion.IdPedido));
        return lista;
    }

    public int Insert(cAsignacionResponsable asignacion)
    {
        return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(asignacion));
    }
    
    public int Save(cAsignacionResponsable asignacion)
    {
        if (string.IsNullOrEmpty(asignacion.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(asignacion));
        else
            return cDataBase.GetInstance().UpdateObject(asignacion.Id, GetTable, AttributesClass(asignacion));
    }

    public cAsignacionResponsable Load(string id)
    {
        try {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cAsignacionResponsable asignacion = new cAsignacionResponsable();
            asignacion.Id = id;
            asignacion.IdResponsable = Convert.ToString(atributos["idResponsable"]);
            asignacion.IdAsigno = Convert.ToString(atributos["idAsigno"]);
            asignacion.Fecha = Convert.ToDateTime(atributos["Fecha"]);
            asignacion.Comentario = Convert.ToString(atributos["Comentario"]);
            return asignacion;
        }
        catch { 
            return null; 
        }
    }

    public Dictionary<string, int> GetAsignacionesPendientesPorUsuario()
    {
        Dictionary<string, int> d = new Dictionary<string, int>();
        SqlCommand cmd = new SqlCommand();

        string query = "SELECT COUNT(a.id) AS cant, a.idResponsable AS idResponsable FROM tAsignacionResponsable AS a";
        query += " INNER JOIN tPedido AS p ON p.idAsignacionResponsable = a.id WHERE (p.Estado <> 2) GROUP BY a.idResponsable ORDER BY cant DESC";

        cmd.CommandText = query.ToString();
        DataSet dataSet = cDataBase.GetInstance().GetDataSet(cmd, GetTable);

        if (dataSet == null) return null;
        DataTable dt = dataSet.Tables[0];

        foreach (DataRow row in dt.Rows)
        {
            d.Add(Convert.ToString(row["idResponsable"]), Convert.ToInt32(row["cant"]));
        }
        return d;
    }

    public cAsignacionResponsable GetResponsablePorPedido(string idPedido)
    {
        string query = "SELECT a.id FROM tAsignacionResponsable AS a WHERE (a.id = '" + idPedido + "')";

        SqlCommand com = new SqlCommand(query);
        string id = cDataBase.GetInstance().ExecuteScalar(com);
        return Load(Convert.ToString(id));
    }
}
