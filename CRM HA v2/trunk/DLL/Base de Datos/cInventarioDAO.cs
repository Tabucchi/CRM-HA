using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;

public class cInventarioDAO
{
    public string GetTable
    { get { return "tInventario"; } }

    public string GetOrderBy
    { get { return "id DESC"; } }

    public List<cAtributo> AttributesClass(cInventario inventario)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idImagen", inventario.IdImagen));
        lista.Add(new cAtributo("idCategoria", inventario.IdCategoria));
        lista.Add(new cAtributo("descripcion", inventario.Descripcion));
        lista.Add(new cAtributo("empresa", inventario.Empresa));
        lista.Add(new cAtributo("nro", inventario.Numero));
        lista.Add(new cAtributo("valor", inventario.Valor));
        lista.Add(new cAtributo("cantUnidades", inventario.CantUnidades));
        lista.Add(new cAtributo("idResponsable", inventario.IdResponsable));
        return lista;
    }

    public int Save(cInventario inventario)
    {
        if (string.IsNullOrEmpty(inventario.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(inventario));
        else
            return cDataBase.GetInstance().UpdateObject(inventario.Id, GetTable, AttributesClass(inventario));
    }

    public cInventario Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cInventario inventario = new cInventario();
        inventario.Id = Convert.ToString(atributos["id"]);
        inventario.IdImagen = Convert.ToString(atributos["idImagen"]);
        inventario.IdCategoria = Convert.ToString(atributos["idCategoria"]);
        inventario.Descripcion = Convert.ToString(atributos["descripcion"]);
        inventario.Empresa = Convert.ToString(atributos["empresa"]);
        inventario.Numero = Convert.ToString(atributos["nro"]);
        inventario.Valor = Convert.ToDecimal(atributos["valor"]);
        inventario.CantUnidades = Convert.ToInt16(atributos["cantUnidades"]);
        inventario.IdResponsable = Convert.ToInt16(atributos["idResponsable"]);
        return inventario;
    }

    public List<cInventario> GetInventarios(string empresa)
    {
        List<cInventario> empresas = new List<cInventario>();
        string query = "SELECT id FROM " + GetTable;

        if(!string.IsNullOrEmpty(empresa))
            query += " WHERE empresa=" + empresa;
        
        query += " Order by Descripcion ASC";

        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            empresas.Add(Load(Convert.ToString(idList[i])));
        }
        return empresas;
    }

    public Decimal GetValor(string idEmpresa)
    {
        string query = "SELECT SUM(cantUnidades * valor) AS valor FROM " + GetTable;

        if (!string.IsNullOrEmpty(idEmpresa))
            query += " WHERE empresa=" + idEmpresa;

        SqlCommand com = new SqlCommand();
        com.CommandText = query.ToString();

        decimal valor = 0;
        if (!string.IsNullOrEmpty(cDataBase.GetInstance().ExecuteScalar(com)))
            valor = Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(com));
        else
            valor = 0;

        return valor;
    }
    public bool Delete(string id)
    {
        return cDataBase.GetInstance().DeleteObject(id, GetTable.ToString());
    }
}

