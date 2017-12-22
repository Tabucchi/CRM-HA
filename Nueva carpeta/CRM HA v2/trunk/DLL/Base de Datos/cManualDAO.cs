using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

public class cManualDAO
{
    public string GetTable
    { get { return "tManual"; } }

    public string GetOrderBy
    { get { return "Nombre ASC"; } }

    public List<cAtributo> AttributesClass(cManual manual)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("Titulo", manual.Titulo));
        lista.Add(new cAtributo("Descripcion", manual.Descripcion));
        lista.Add(new cAtributo("idUsuario", manual.IdUsuario));
        lista.Add(new cAtributo("fecha", manual.Fecha));
        lista.Add(new cAtributo("idEmpresa", manual.IdEmpresa));
        lista.Add(new cAtributo("Papelera", manual.Papelera));
        return lista;
    }

    public int Save(cManual manual)
    {
        if (string.IsNullOrEmpty(manual.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(manual));
        else
            return cDataBase.GetInstance().UpdateObject(manual.Id, GetTable, AttributesClass(manual));
    }

    public cManual Load(string id)
    {
        try
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cManual manual = new cManual();
            manual.Id = Convert.ToString(atributos["id"]);
            manual.Titulo = Convert.ToString(atributos["titulo"]);
            manual.Descripcion = Convert.ToString(atributos["descripcion"]);
            manual.IdUsuario = Convert.ToString(atributos["idUsuario"]);
            manual.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
            manual.Fecha = Convert.ToDateTime(atributos["fecha"]);
            manual.Papelera = Convert.ToInt16(atributos["Papelera"]);
            return manual;
        }catch
        {
            return null;
        }
    }

    public ArrayList LoadTable()
    {
        ArrayList manuales=new ArrayList();
        SqlCommand cmd=new SqlCommand("SELECT id FROM " + GetTable);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);
        if (idList == null) 
            return null;
        for(int i=0; idList.Count>i;i++)
        {
            manuales.Add(Load(Convert.ToString(idList[i])));
        }
        return manuales;
    }

    public List<cManual> GetManualesTop5()
    {
        List<cManual> manuales = new List<cManual>();
        string query = "SELECT TOP(5) id FROM " + GetTable + " WHERE Papelera = 1 Order by Fecha DESC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;

        for (int i = 0; idList.Count > i; i++)
        {
            manuales.Add(Load(Convert.ToString(idList[i])));
        }
        return manuales;
    }

    public List<cManual> GetManuales()
    {
        List<cManual> manuales=new List<cManual>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera = 1 Order by Fecha DESC";
        SqlCommand com=new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;

        for(int i=0;idList.Count>i; i++)
        {
            manuales.Add(Load(Convert.ToString(idList[i])));
        }
        return manuales;
    }

    public List<cManual> SearchByEmpresa(string idEmpresa)
    {
        List<cManual> manuales = new List<cManual>();
        SqlCommand cmd = new SqlCommand();

        string query = "SELECT id FROM " + GetTable + " WHERE idEmpresa";
        query += idEmpresa == "0" ? " <> @idEmpresa" : " = @idEmpresa";
        query += " AND Papelera = 1 Order by Fecha DESC";

        cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
        cmd.Parameters["@idEmpresa"].Value = idEmpresa;

        cmd.CommandText = query;
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);
        if (idList == null)
            return null;

        for (int i = 0; idList.Count > i; i++)
        {
            manuales.Add(Load(Convert.ToString(idList[i])));
        }
        return manuales;
    }

    public List<cManual> Search(string id)
    {
        List<cManual> manuales = new List<cManual>();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();

        query.Append("SELECT * FROM " + GetTable + " WHERE ");

        if (Convert.ToInt32(id) > 0)
            query.Append(" id = @id");
        else
            query.Append(" id <> @id");

        cmd.Parameters.Add("@id", SqlDbType.Int);
        cmd.Parameters["@id"].Value = id;

        query.Append(" AND Papelera = 1 ORDER BY Fecha DESC");

        cmd.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            manuales.Add(Load(Convert.ToString(idList[i])));
        }
        return manuales;
    }

    public string GetIdByNombre(string manual)
    {
        string query = "SELECT id FROM " + GetTable + " WHERE (titulo like '%" + manual + "%')";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public List<cManual> GetManualesPosibles(string texto)
    {
        List<cManual> manuales = new List<cManual>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 1" + " Order by titulo ASC";

        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            manuales.Add(Load(Convert.ToString(idList[i])));
        }
        return manuales;
    }
}
