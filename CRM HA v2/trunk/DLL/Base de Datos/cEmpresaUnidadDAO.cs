using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

public class cEmpresaUnidadDAO
{
    public string GetTable
    { get { return "tEmpresaUnidad"; } }

    public string GetOrderBy
    { get { return "id ASC"; } }

    public List<cAtributo> AttributesClass(cEmpresaUnidad unidad)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idEmpresa", unidad.IdEmpresa));
        lista.Add(new cAtributo("idUnidad", unidad.IdUnidad));
        lista.Add(new cAtributo("codUF", unidad.CodUF));
        lista.Add(new cAtributo("idProyecto", unidad.IdProyecto));
        lista.Add(new cAtributo("precioAcordado", unidad.PrecioAcordado));
        lista.Add(new cAtributo("idOv", unidad.IdOv));
        lista.Add(new cAtributo("papelera", unidad.Papelera));
        return lista;
    }

    public int Save(cEmpresaUnidad unidad)
    {
        if (string.IsNullOrEmpty(unidad.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(unidad));
        else
            return cDataBase.GetInstance().UpdateObject(unidad.Id, GetTable, AttributesClass(unidad));
    }

    public cEmpresaUnidad Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cEmpresaUnidad unidad = new cEmpresaUnidad();
        unidad.Id = Convert.ToString(atributos["id"]);
        unidad.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
        unidad.IdUnidad = Convert.ToString(atributos["idUnidad"]);
        unidad.CodUF = Convert.ToString(atributos["codUF"]);
        unidad.IdProyecto = Convert.ToString(atributos["idProyecto"]);
        unidad.PrecioAcordado = Convert.ToDecimal(atributos["precioAcordado"]);
        unidad.IdOv = Convert.ToString(atributos["idOv"]);
        unidad.Papelera = Convert.ToInt16(atributos["papelera"]);
        return unidad;
    }

    public string GetUnidadByIdProyecto(string _idProyecto)
    {
        string query = "SELECT id FROM " + GetTable + " WHERE idProyecto = '" + _idProyecto + "' AND papelera = " + (Int16)papelera.Activo;
        SqlCommand com = new SqlCommand(query);

        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
    }
    public string GetUnidadByIdUnidad(string _idUnidad)
    {
        string query = "SELECT id FROM tEmpresaUnidad where idUnidad='" + _idUnidad + "' AND papelera='" + (Int16)papelera.Activo + "'";
        SqlCommand com = new SqlCommand(query);

        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
    }

    public string GetIdOperacionVentaByIdUnidad(string _idUnidad)
    {
        string query = "SELECT idOv FROM tEmpresaUnidad where idUnidad='" + _idUnidad + "' AND papelera='" + (Int16)papelera.Activo + "'";
        SqlCommand com = new SqlCommand(query);
        string result = cDataBase.GetInstance().ExecuteScalar(com);

        if (result != null)
            return Convert.ToString(result);
        else
            return null;
    }

    public cEmpresaUnidad GetUnidad(string _codUF, string _idProyecto)
    {
        string query = "SELECT id FROM " + GetTable + " WHERE codUF =" + _codUF  + " AND idProyecto = '" + _idProyecto + "' AND papelera = " + (Int16)papelera.Activo;

        SqlCommand com = new SqlCommand(query);
        string id = cDataBase.GetInstance().ExecuteScalar(com);

        if (id != null)
            return Load(Convert.ToString(id));
        else
            return null;
    }

    public cEmpresa GetEmpresaByUnidad(string _codUF, string _idProyecto)
    {
        string query = "SELECT idEmpresa FROM " + GetTable + " WHERE codUF =" + _codUF + " AND idProyecto = '" + _idProyecto + "' AND papelera = " + (Int16)papelera.Activo;
        SqlCommand com = new SqlCommand(query);
        string idEmpresa = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));

        if (idEmpresa != null)
            return cEmpresa.Load(idEmpresa);
        else
            return null;
    }

    public List<cEmpresaUnidad> GetEmpresaUnidadOV(string _idOperacionVenta)
    {
        List<cEmpresaUnidad> cc = new List<cEmpresaUnidad>();
        string query = "SELECT eu.id FROM tEmpresaUnidad eu WHERE eu.idOv = '" + _idOperacionVenta + "' GROUP BY eu.id";

        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            cc.Add(Load(Convert.ToString(idList[i])));
        }
        return cc;
    }

}