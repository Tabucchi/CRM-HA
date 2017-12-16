using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

public class cProveedorDAO
{
    public string GetTable
    { get { return "tProveedor"; } }

    public string GetOrderBy
    { get { return "Nombre DESC"; } }

    public List<cAtributo> AttributesClass(cProveedor proveedor)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("Nombre", proveedor.Nombre));
        lista.Add(new cAtributo("Direccion", proveedor.Direccion));
        lista.Add(new cAtributo("Telefono", proveedor.Telefono));
        lista.Add(new cAtributo("Mail", proveedor.Mail));
        lista.Add(new cAtributo("Papelera", proveedor.Papelera));
        return lista;
    }

    public cProveedor Load(string id)
    {
        try
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cProveedor proveedor = new cProveedor();
            proveedor.Id = Convert.ToString(atributos["id"]);
            proveedor.Nombre = Convert.ToString(atributos["Nombre"]);
            proveedor.Direccion = Convert.ToString(atributos["Direccion"]);
            proveedor.Telefono = Convert.ToString(atributos["Telefono"]);
            proveedor.Mail = Convert.ToString(atributos["Mail"]);
            proveedor.Papelera = Convert.ToInt16(atributos["Papelera"]);
            return proveedor;
        }
        catch
        {
            return null;
        }
    }

    public ArrayList LoadTable()
    {
        ArrayList proveedores = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        for (int i = 0; idList.Count > i; i++)
        {
            proveedores.Add(Load(Convert.ToString(idList[i])));
        }
        return proveedores;
    }

    public int Save(cProveedor proveedor)
    {
        if (string.IsNullOrEmpty(proveedor.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(proveedor));
        else
            return cDataBase.GetInstance().UpdateObject(proveedor.Id, GetTable, AttributesClass(proveedor));
    }

    public List<cProveedor> GetProveedores()
    {
        List<cProveedor> proveedores = new List<cProveedor>();
        string query = "SELECT id FROM " + GetTable + " WHERE Papelera= 0 AND id<>0 Order by Nombre ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            proveedores.Add(Load(Convert.ToString(idList[i])));
        }
        return proveedores;
    }

    public List<cProveedor> Search(string idProveedor)
    {
        List<cProveedor> proveedores = new List<cProveedor>();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();

        query.Append("SELECT * FROM " + GetTable + " WHERE ");

        if (Convert.ToInt32(idProveedor) > 0)
            query.Append(" id = @id");
        else
            query.Append(" id <> @id");

        cmd.Parameters.Add("@id", SqlDbType.Int);
        cmd.Parameters["@id"].Value = idProveedor;

        query.Append(" ORDER  BY id DESC");

        cmd.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            proveedores.Add(Load(Convert.ToString(idList[i])));
        }
        return proveedores;
    }
}
