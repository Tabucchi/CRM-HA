using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLL.Negocio;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

 public class cResponsableEmpresaDAO
{
    public string GetTable
     { get { return "tResponsableEmpresa"; } }

    public List<cAtributo> AttributesClass(cResponsableEmpresa re)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idEmpresa", re.IdEmpresa));
        lista.Add(new cAtributo("idUsuario", re.IdUsuario));
        return lista;
    }

    public int Save(cResponsableEmpresa re)
    {
        if (string.IsNullOrEmpty(re.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(re));
        else
            return cDataBase.GetInstance().UpdateObject(re.Id, GetTable, AttributesClass(re));
    }

    public cResponsableEmpresa Load(string id)
    {
        try
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cResponsableEmpresa re = new cResponsableEmpresa();
            re.Id = Convert.ToString(atributos["id"]);
            re.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
            re.IdUsuario = Convert.ToString(atributos["idUsuario"]);
            return re;
        }
        catch
        {
            return null;
        }
    }
     
    public string Eliminar(string id)
    {
        string query = "DELETE FROM tResponsableEmpresa WHERE id= '" + id + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public List<cResponsableEmpresa> Search(string idEmpresa, string idUsuario)
    {
        List<cResponsableEmpresa> empresas = new List<cResponsableEmpresa>();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();

        query.Append("SELECT r.id FROM tResponsableEmpresa AS r INNER JOIN tEmpresa AS e ON r.idEmpresa = e.id");

        if (!string.IsNullOrEmpty(idEmpresa) && !string.IsNullOrEmpty(idUsuario))
        {
            if (idEmpresa != "0" && idUsuario != "0")
                query.Append(" WHERE ");
        }

        if (!string.IsNullOrEmpty(idEmpresa))
        {
            if (idEmpresa != "0"){
                query.Append(" r.idEmpresa = @idEmpresa");
                cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
                cmd.Parameters["@idEmpresa"].Value = idEmpresa;
            }
        }

        if (!string.IsNullOrEmpty(idUsuario) && idUsuario != "0")
        {
            if (idUsuario != "0")
            {
                if (!string.IsNullOrEmpty(idEmpresa) && idEmpresa != "0") query.Append(" AND ");
                query.Append(" r.idUsuario = @idUsuario");
                cmd.Parameters.Add("@idUsuario", SqlDbType.Int);
                cmd.Parameters["@idUsuario"].Value = idUsuario;
            }
        }

        query.Append(" ORDER BY e.Nombre");

        cmd.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToString(idList[i])));
        }
        return empresas;
    }

    public DataTable reporteXLS()
    {
        SqlCommand com = new SqlCommand();
        string query = "SELECT e.Nombre AS Empresa, u.Nombre AS Responsable FROM tResponsableEmpresa AS r INNER JOIN tEmpresa AS e ON r.idEmpresa = e.id INNER JOIN tUsuario AS u ON r.idUsuario = u.id";
        com.CommandText = query.ToString();
        return cDataBase.GetInstance().GetDataReader(com);
    }

    public string GetResponsable(string idEmpresa)
    {
        string query = "SELECT idUsuario FROM " + GetTable + " WHERE idEmpresa = '" + idEmpresa + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }
 }
