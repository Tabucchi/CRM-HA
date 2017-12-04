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
        lista.Add(new cAtributo("ClaveSistema", cliente.ClaveSistema));
        lista.Add(new cAtributo("Autorizado", cliente.Autorizacion));
        lista.Add(new cAtributo("Papelera", cliente.Papelera));
        return lista;
    }

    public int Save(cCliente cliente)
    {
        if (string.IsNullOrEmpty(cliente.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(cliente));
        else
            return cDataBase.GetInstance().UpdateObject(cliente.Id, GetTable, AttributesClass(cliente));
    }

    public cCliente Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cCliente cliente = new cCliente();
        cliente.Id = Convert.ToString(atributos["id"]);
        cliente.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
        cliente.Nombre = Convert.ToString(atributos["Nombre"]);
        cliente.Interno = Convert.ToString(atributos["Interno"]);
        cliente.Mail = Convert.ToString(atributos["Mail"]);
        cliente.Ip = Convert.ToString(atributos["Ip"]);
        cliente.Password = Convert.ToString(atributos["Password"]);
        cliente.UsuarioRed = Convert.ToString(atributos["UsuarioRed"]);
        cliente.PasswordRed = Convert.ToString(atributos["PasswordRed"]);
        cliente.ClaveSistema = Convert.ToString(atributos["ClaveSistema"]);
        cliente.Autorizacion = Convert.ToInt16(atributos["Autorizado"]);
        cliente.Papelera = Convert.ToInt16(atributos["Papelera"]);
        return cliente;
    }

    public ArrayList LoadTable()
    {
        ArrayList clientes = new ArrayList();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera = 1 ORDER BY " + GetOrderBy;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        for (int i = 0; idList.Count > i; i++)
        {
            clientes.Add(Load(Convert.ToString(idList[i])));
        }
        return clientes;
    }

    public DataTable LoadTableForCombo()
    {
        string query = "SELECT c.id, c.nombre, e.Nombre AS Empresa FROM " + GetTable + " AS c INNER JOIN tEmpresa AS e ON c.idEmpresa = e.id WHERE c.Papelera = 1 ORDER BY c.Nombre";
        SqlCommand com = new SqlCommand(query);

        return cDataBase.GetInstance().GetDataReader(com);
    }
    
    public ArrayList SearchByIdEmpresa(int id)
    {
        ArrayList clientes = new ArrayList();
        string query = "SELECT id FROM " + GetTable + " WHERE idEmpresa = " + id + " ORDER BY " + GetOrderBy;
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        for (int i = 0; idList.Count > i; i++)
        {
            clientes.Add(Load(Convert.ToString(idList[i])));
        }
        return clientes;
    }

    /// <summary>
    /// Devuelve una lista de clientes que contengan en parte de su nombre o en el nombre de su empresa el texto pasado por parametro.
    /// </summary>
    public List<cCliente> SearchByText(string texto)
    {
        List<cCliente> clientes = new List<cCliente>();
        string query = "SELECT c.id FROM tCliente AS c INNER JOIN tEmpresa AS e ON c.idEmpresa = e.id WHERE (c.Nombre LIKE '%" + texto + "%') OR (e.Nombre LIKE '%" + texto + "%') AND (c.Papelera = 1) ORDER BY c.Nombre";

        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            clientes.Add(Load(Convert.ToString(idList[i])));
        }
        return clientes;
    }

    public cCliente SearchByNombreAndEmpresa(string nombreCliente, string nombreEmpresa)
    {
        string query = "SELECT c.id FROM tCliente AS c INNER JOIN tEmpresa AS e ON c.idEmpresa = e.id WHERE (c.Nombre = '" + nombreCliente + "') AND (e.Nombre like '%" + nombreEmpresa + "%') AND (e.Papelera = '1')";

        SqlCommand com = new SqlCommand(query);
        string id = cDataBase.GetInstance().ExecuteScalar(com);
        return Load(Convert.ToString(id));
    }

    public cCliente SearchByMail(string mail, bool flag)
    {
        string query = "SELECT id FROM tCliente WHERE mail Like '%" + mail + "%'";

        if (flag == true)
        {
            query+=" AND Papelera=" + (Int16)papelera.Activo;
        }

        SqlCommand com = new SqlCommand(query);
        string id = cDataBase.GetInstance().ExecuteScalar(com);
        if (id == null)
            return null;
        return Load(Convert.ToString(id));
    }

    public string GetFullPhone(string idEmpresa)
    {
        cEmpresaDAO empresaDAO = new cEmpresaDAO();
        return empresaDAO.GetFullPhone(idEmpresa);
    }

    public List<cCliente> GetClientes()
    {
        List<cCliente> clientes = new List<cCliente>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 1" + " Order by Nombre ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            clientes.Add(Load(Convert.ToString(idList[i])));
        }
        return clientes;
    }
    
    public List<cCliente> GetClientesByIdEmpresa(string idEmpresa)
    {
        List<cCliente> clientes = new List<cCliente>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 1 AND IdEmpresa=" + idEmpresa + " Order by Nombre ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            clientes.Add(Load(Convert.ToString(idList[i])));
        }
        return clientes;
    }

    public List<cCliente> GetClientesAutorizados(string idEmpresa)
    {
        List<cCliente> clientes = new List<cCliente>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 1 AND IdEmpresa=" + idEmpresa + " AND Autorizado=" + (Int16)Autorizaciones.Autorizado + " Order by Nombre ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            clientes.Add(Load(Convert.ToString(idList[i])));
        }
        return clientes;
    }

    public int LoginCliente(string nameUser, string password)
    {
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        // Si la clave y el usuario son correctos devolverá 1, sino 0.
        query.Append("SELECT id FROM " + this.GetTable);
        query.Append(" WHERE Mail = @Mail AND ClaveSistema = @ClaveSistema AND Papelera = @Papelera");
        SqlCommand cmd = new SqlCommand(query.ToString());
        // Creamos los parámetros.
        cmd.Parameters.Add("@Mail", SqlDbType.NVarChar, 63);
        cmd.Parameters.Add("@ClaveSistema", SqlDbType.NVarChar, 63);
        cmd.Parameters.Add("@Papelera", SqlDbType.SmallInt);
        // Asignamos los valores recibidos como parámetro.
        cmd.Parameters["@Mail"].Value = nameUser;
        cmd.Parameters["@ClaveSistema"].Value = password;
        cmd.Parameters["@Papelera"].Value = 1; // Usuario activo.
        // Ejecutamos la consulta
        return Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
    }
    
    public Dictionary<string, string> GetClienteEmpresa()
    {
        Dictionary<string, string> d = new Dictionary<string, string>();
        SqlCommand cmd = new SqlCommand();

        string query = "SELECT c.Nombre as cliente, e.Nombre as empresa FROM tCliente AS c INNER JOIN tEmpresa AS e ON c.idEmpresa = e.id WHERE c.Papelera='0'";

        cmd.CommandText = query.ToString();
        DataSet dataSet = cDataBase.GetInstance().GetDataSet(cmd, GetTable);
        if (dataSet == null) return null;
        DataTable dt = dataSet.Tables[0];

        foreach (DataRow row in dt.Rows)
        {
            d.Add(Convert.ToString(row["cliente"]), Convert.ToString(row["empresa"]));
        }
        return d;
    }
}