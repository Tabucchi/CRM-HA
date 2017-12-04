using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;

public class cCampoGenericoDAO
{
    public string GetOrderBy
    { get { return "id ASC"; } }

    public List<cAtributo> AttributesClass(cCampoGenerico campo)
    {
        List<cAtributo> lista = new List<cAtributo>();
        //lista.Add(new cAtributo("id", campo.Id));
        lista.Add(new cAtributo("descripcion", campo.Descripcion));
        return lista;
    }

    public cCampoGenerico Load(string id, string tabla)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id.ToString(), tabla);
        cCampoGenerico campo = new cCampoGenerico();
        if (atributos != null) {         
            campo.Id = Convert.ToString(id);
            campo.Descripcion = Convert.ToString(atributos["descripcion"]);
        }
        return campo;
    }

    public cCampoGenerico LoadDescripcion(string id, string tabla)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id.ToString(), tabla);
        cCampoGenerico campo = new cCampoGenerico();
        if (atributos != null)
        {
            campo.Id = Convert.ToString(id);
            campo.Descripcion = Convert.ToString(atributos["descripcion"]);
        }
        return campo;
    }

    public ArrayList LoadTable(string tabla)
    {
        ArrayList valores = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(tabla, GetOrderBy);
        for (int i = 0; idList.Count > i; i++)
        {
            valores.Add(Load(Convert.ToString(idList[i]), tabla));
        }
        return valores;
    }

    public int Save(cCampoGenerico campo, string tabla)
    {
        if (string.IsNullOrEmpty(campo.Id))
            return cDataBase.GetInstance().InsertObject(tabla, AttributesClass(campo));
        else
            return cDataBase.GetInstance().UpdateObject(campo.Id, tabla, AttributesClass(campo));
    }

    public List<cCampoGenerico> GetResponsableInventario(string responsable)
    {
        List<cCampoGenerico> estados = new List<cCampoGenerico>();
        string query = "SELECT id FROM tResponsableInventario WHERE idEmpresaInventario=" + responsable + " Order by id ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            estados.Add(Load(Convert.ToString(idList[i]), Convert.ToString(Tablas.tResponsableInventario)));
        }
        return estados;
    }

    public string GetFechaMail() //Obtengo la fecha del ultimo mail
    {
        string query = "SELECT fecha FROM tFechaMail";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public string UpdateFecha(string fecha) //Cambia estado del campo papelera
    {
        string query = "UPDATE tFechaMail SET fecha= '" + fecha + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public string GetPassDatosTecnicos()
    {
        string query = "SELECT tipo FROM tDatosTecnicos ORDER BY id";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }
}
