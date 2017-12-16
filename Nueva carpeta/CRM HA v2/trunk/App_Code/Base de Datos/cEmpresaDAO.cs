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


public class cEmpresaDAO
{
    public string GetTable
    { get { return "tEmpresa"; } }

    public string GetOrderBy
    { get { return "Nombre ASC"; } }

    public List<cAtributo> AttributesClass(cEmpresa empresa)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("Nombre", empresa.Nombre));
        lista.Add(new cAtributo("Direccion", empresa.Direccion));
        lista.Add(new cAtributo("Telefono", empresa.Telefono));
        lista.Add(new cAtributo("Datos", empresa.Datos));
        return lista;
    }

    public int Insert(cEmpresa empresa)
    {
        return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(empresa));
    }

    public bool Update(cEmpresa empresa)
    {
        return cDataBase.GetInstance().UpdateObject(empresa.Id.ToString(), GetTable, AttributesClass(empresa));
    }

    public bool Delete(string id)
    {
        return cDataBase.GetInstance().DeleteObject(id, GetTable);
    }

    public cEmpresa Load(int id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cEmpresa empresa = new cEmpresa();
        empresa.Id = Convert.ToInt32(atributos["id"]);
        empresa.Nombre = Convert.ToString(atributos["Nombre"]);
        empresa.Direccion = Convert.ToString(atributos["Direccion"]);
        empresa.Telefono = Convert.ToString(atributos["Telefono"]);
        empresa.Datos = Convert.ToString(atributos["Datos"]);
        return empresa;
    }

    public ArrayList LoadTable()
    {
        ArrayList empresas = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToInt32(idList[i])));
        }
        return empresas;
    }

    public string GetFullPhone(int id)
    {
        string query = "SELECT Telefono FROM " + GetTable + " WHERE id = @idEmpresa";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
        cmd.Parameters["@idEmpresa"].Value = id;
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }


}

