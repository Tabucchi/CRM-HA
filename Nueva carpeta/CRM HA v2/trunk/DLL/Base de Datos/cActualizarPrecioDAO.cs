using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

public class cActualizarPrecioDAO
{
    public string GetTable
    { get { return "tActualizarPrecio"; } }

    public string GetOrderBy
    { get { return "id ASC"; } }

    public List<cAtributo> AttributesClass(cActualizarPrecio precio)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("codUF", precio.CodigoUF));
        lista.Add(new cAtributo("idProyecto", precio.IdProyecto));
        lista.Add(new cAtributo("valorViejo", precio.ValorViejo));
        lista.Add(new cAtributo("valorNuevo", precio.ValorNuevo));
        lista.Add(new cAtributo("tipoActualizacion", precio.TipoActualizacion));
        lista.Add(new cAtributo("valorActualizacion", precio.ValorActualizacion));
        lista.Add(new cAtributo("estado", precio.Estado));
        return lista;
    }

    public int Save(cActualizarPrecio precio)
    {
        if (string.IsNullOrEmpty(precio.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(precio));
        else
            return cDataBase.GetInstance().UpdateObject(precio.Id, GetTable, AttributesClass(precio));
    }

    public cActualizarPrecio Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cActualizarPrecio precio = new cActualizarPrecio();
        precio.Id = Convert.ToString(atributos["id"]);
        precio.CodigoUF = Convert.ToString(atributos["codUF"]);
        precio.IdProyecto = Convert.ToString(atributos["idProyecto"]);
        precio.ValorViejo = Convert.ToDecimal(atributos["valorViejo"]);
        precio.ValorNuevo = Convert.ToDecimal(atributos["valorNuevo"]);
        precio.TipoActualizacion = Convert.ToString(atributos["tipoActualizacion"]);
        precio.ValorActualizacion = Convert.ToDecimal(atributos["valorActualizacion"]);
        precio.Estado = Convert.ToInt16(atributos["estado"]);
        return precio;
    }

    public List<cActualizarPrecio> GetPrecios()
    {
        List<cActualizarPrecio> precios = new List<cActualizarPrecio>();
        string query = "SELECT id FROM " + GetTable + " WHERE estado = '" + (Int16)estadoActualizarPrecio.A_confirmar + "' ORDER BY id ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
            precios.Add(Load(Convert.ToString(idList[i])));

        return precios;
    }

    public cActualizarPrecio GetActualizacionByProyectoAndUF(string _codUF, int _idProyecto)
    {
        string query = "SELECT id FROM tActualizarPrecio WHERE idProyecto = " + _idProyecto + " AND codUF = '" + _codUF + "' AND estado = 0";
        cActualizarPrecio ap = new cActualizarPrecio();
        SqlCommand com = new SqlCommand(query);
        string idList = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        if (idList == null) return null;
        ap = Load(idList);
        return ap;
    }

    public List<cActualizarPrecio> GetActualizacionByIdProyecto(string _idProyecto)
    {
        List<cActualizarPrecio> precios = new List<cActualizarPrecio>();
        string query = "SELECT id FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND estado = '" + (Int16)estadoActualizarPrecio.A_confirmar + "' ORDER BY id ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
            precios.Add(Load(Convert.ToString(idList[i])));
        return precios;
    }
}
