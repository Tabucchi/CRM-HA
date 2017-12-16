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

public class cClienteDAO
{
    public string GetTable
    { get { return "tCliente"; } }

    public string GetOrderBy
    { get { return "Nombre ASC"; } }

    public List<cAtributo> AttributesClass(cCliente cliente)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idEmpresa", cliente.IdEmpresa));
        lista.Add(new cAtributo("Nombre", cliente.Nombre));
        lista.Add(new cAtributo("Interno", cliente.Interno));
        lista.Add(new cAtributo("Mail", cliente.Mail));
        lista.Add(new cAtributo("Ip", cliente.Ip));
        lista.Add(new cAtributo("Password", cliente.Password));
        lista.Add(new cAtributo("UsuarioRed", cliente.UsuarioRed));
        lista.Add(new cAtributo("PasswordRed", cliente.PasswordRed));
        return lista;
    }

    public int Insert(cCliente cliente)
    {
        return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(cliente));
    }

    public bool Update(cCliente cliente)
    {
        return cDataBase.GetInstance().UpdateObject(cliente.Id.ToString(), GetTable, AttributesClass(cliente));
    }

    public bool Delete(string id)
    {
        return cDataBase.GetInstance().DeleteObject(id, GetTable);
    }

    public cCliente Load(int id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cCliente cliente = new cCliente();
        cliente.Id = Convert.ToInt32(atributos["id"]);
        cliente.IdEmpresa = Convert.ToInt32(atributos["idEmpresa"]);
        cliente.Nombre = Convert.ToString(atributos["Nombre"]);
        cliente.Interno = Convert.ToString(atributos["Interno"]);
        cliente.Mail = Convert.ToString(atributos["Mail"]);
        cliente.Ip = Convert.ToString(atributos["Ip"]);
        cliente.Password = Convert.ToString(atributos["Password"]);
        cliente.UsuarioRed = Convert.ToString(atributos["UsuarioRed"]);
        cliente.PasswordRed = Convert.ToString(atributos["PasswordRed"]);
        return cliente;
    }

    public ArrayList LoadTable()
    {
        ArrayList clientes = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        for (int i = 0; idList.Count > i; i++)
        {
            clientes.Add(Load(Convert.ToInt32(idList[i])));
        }
        return clientes;
    }

    public ArrayList SearchByIdEmpresa(int id)
    {
        ArrayList clientes = new ArrayList();
        string query = "SELECT id FROM " + GetTable + " WHERE idEmpresa = " + id + " ORDER BY " + GetOrderBy;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        for (int i = 0; idList.Count > i; i++)
        {
            clientes.Add(Load(Convert.ToInt32(idList[i])));
        }
        return clientes;
    }

    public string GetFullPhone(int idEmpresa)
    {
        cEmpresaDAO empresaDAO = new cEmpresaDAO();
        return empresaDAO.GetFullPhone(idEmpresa);
    }
}