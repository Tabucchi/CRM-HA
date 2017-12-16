using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

public class cHistorialDAO
{
    public string GetTable
    { get { return "tHistorial"; } }

    public string GetOrderBy
    { get { return "id ASC"; } }

    public List<cAtributo> AttributesClass(cHistorial precio)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("fecha", precio.Fecha));
        lista.Add(new cAtributo("motivo", precio.Motivo));
        lista.Add(new cAtributo("valorViejo", precio.ValorViejo));
        lista.Add(new cAtributo("valorNuevo", precio.ValorNuevo));
        lista.Add(new cAtributo("idUsuario", precio.IdUsuario));
        lista.Add(new cAtributo("idProyecto", precio.IdProyecto));
        lista.Add(new cAtributo("codUF", precio.CodUF));
        lista.Add(new cAtributo("nroUnidad", precio.NroUnidad));
        lista.Add(new cAtributo("idEstadoViejo", precio.IdEstadoViejo));
        lista.Add(new cAtributo("idEstadoNuevo", precio.IdEstadoNuevo));
        return lista;
    }

    public int Save(cHistorial precio)
    {
        if (string.IsNullOrEmpty(precio.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(precio));
        else
            return cDataBase.GetInstance().UpdateObject(precio.Id, GetTable, AttributesClass(precio));
    }

    public cHistorial Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cHistorial precio = new cHistorial();
        precio.Id = Convert.ToString(atributos["id"]);
        precio.Fecha = Convert.ToDateTime(atributos["fecha"]);
        precio.Motivo = Convert.ToString(atributos["motivo"]);
        precio.ValorViejo = Convert.ToDecimal(atributos["valorViejo"]);
        precio.ValorNuevo = Convert.ToDecimal(atributos["valorNuevo"]);
        precio.IdUsuario = Convert.ToString(atributos["idUsuario"]);
        precio.IdProyecto = Convert.ToString(atributos["idProyecto"]);
        precio.CodUF = Convert.ToString(atributos["codUF"]);
        precio.NroUnidad = Convert.ToString(atributos["nroUnidad"]);
        precio.IdEstadoViejo = Convert.ToString(atributos["idEstadoViejo"]);
        precio.IdEstadoNuevo = Convert.ToString(atributos["idEstadoNuevo"]);
        return precio;
    }

    public List<cHistorial> GetHistorial(string _idProyecto, string _motivo, bool todos)
    {
        List<cHistorial> _historial = new List<cHistorial>();
        string query = "SELECT id FROM " + GetTable + " WHERE id<>'0'";

        if (_idProyecto != "0")
            query += " AND idProyecto = " + _idProyecto;

        if (_motivo != "0")
        {
            switch (_motivo)
            {
                case "1":
                    query += " AND motivo = '" + historial.Evolución_de_precios.ToString().Replace("_", " ") + "'";
                    if(todos == false)
                        query += " AND CONVERT(VARCHAR(19),fecha, 105) = (Select TOP(1) CONVERT(VARCHAR(19),fecha, 105) from " + GetTable + " ORDER BY fecha DESC)";
                    break;
                case "2":
                    query += " AND motivo = '" + historial.Unidad_modificada.ToString().Replace("_", " ") + "'";
                    if (todos == false)
                        query += " AND CONVERT(VARCHAR(19),fecha, 105) = (Select TOP(1) CONVERT(VARCHAR(19),fecha, 105) from " + GetTable + " ORDER BY fecha DESC)";
                    break;
            }
        }

        query += " Order by fecha ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            _historial.Add(Load(Convert.ToString(idList[i])));
        }
        return _historial;
    }

    public List<cHistorial> GetHistorialByUnidad(string _idProyecto, string _nroUnidad, string _nivel, string _motivo)
    {
        List<cHistorial> _historial = new List<cHistorial>();
        string query = "SELECT h.id FROM tHistorial h, tUnidad u WHERE h.id<>'0'";

        query += " AND h.idProyecto = " + _idProyecto + " AND u.idProyecto = " + _idProyecto + " AND h.motivo = '" + _motivo + "'";

        if (_nroUnidad != "Seleccione una unidad..." && _nroUnidad != "")
            query += " AND h.nroUnidad = '" + _nroUnidad;

        if (_nivel != "0" && _nivel != "")
            query += "' AND u.nivel = '" + _nivel + "'";

        query += " GROUP BY h.id";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            _historial.Add(Load(Convert.ToString(idList[i])));
        }
        return _historial;
    }

    public List<cHistorial> GetHistorialByCodUF_NroUnidad(string _idProyecto, string _motivo, string _codUf)
    {
        List<cHistorial> _historial = new List<cHistorial>();
        string query = "SELECT id FROM " + GetTable + " WHERE id<>'0'";

        if (_idProyecto != "0")
            query += " AND idProyecto = " + _idProyecto;

        if (_motivo != "0")
        {
            if (Convert.ToInt16(_motivo) != (Int16)historial.Evolución_de_precios)
            {
                switch (Convert.ToInt16(_motivo))
                {
                    case (Int16)historial.Unidad_modificada:
                        query += " AND motivo = '" + historial.Unidad_modificada.ToString().Replace("_", " ") + "'";
                        break;
                }
            }
            else
                query += " AND motivo = '" + historial.Evolución_de_precios.ToString().Replace("_", " ") + "''";
        }

        query += " AND codUF = " + _codUf + " Order by fecha ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            _historial.Add(Load(Convert.ToString(idList[i])));
        }
        return _historial;
    }

    public cHistorial LoadByIdProyecto(string _idProyecto)
    {
        cHistorial historial = new cHistorial();
        string query = "SELECT TOP(1) id FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " ORDER BY id DESC";
        SqlCommand com = new SqlCommand(query);
        string idList = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
        if (idList == null) return null;
        historial = Load(idList);
        return historial;
    }
}

