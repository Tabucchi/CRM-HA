using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;
using System.Data;

public class cCompraDAO
{
    public string GetTable
    {get{return "tCompra";}}

    public string GetOrderBy
    { get { return "id ASC"; }}

    public List<cAtributo> AttributesClass(cCompra compra)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("IdPedido", compra.IdPedido));
        lista.Add(new cAtributo("IdEmpresa", compra.IdEmpresa));
        lista.Add(new cAtributo("IdCliente", compra.IdCliente));
        lista.Add(new cAtributo("IdUsuario", compra.IdUsuario));
        lista.Add(new cAtributo("IdEstado", compra.IdEstado));
        lista.Add(new cAtributo("Fecha", compra.Fecha));
        lista.Add(new cAtributo("TotalProveedor", compra.TotalProveedor));
        lista.Add(new cAtributo("TotalCliente", compra.TotalCliente));
        lista.Add(new cAtributo("Iva", compra.Iva));
        lista.Add(new cAtributo("Codigo", compra.Codigo));
        return lista;
    }

    public int Save(cCompra compra)
    {
        if (string.IsNullOrEmpty(compra.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(compra));
        else
            return cDataBase.GetInstance().UpdateObject(compra.Id, GetTable, AttributesClass(compra));
    }

    public cCompra Load(string id, string idPedido)
    {
        Hashtable atributos = new Hashtable();
        if (!string.IsNullOrEmpty(id))
            atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        else
        {
            id = cCompraDAO.GetIdCompra(idPedido);
            atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        }
        cCompra compra = new cCompra();
        compra.Id = Convert.ToString(atributos["id"]);
        compra.IdPedido = Convert.ToString(atributos["idPedido"]);
        compra.IdUsuario = Convert.ToString(atributos["idUsuario"]);
        compra.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
        compra.IdCliente = Convert.ToString(atributos["idCliente"]);
        compra.IdEstado = Convert.ToString(atributos["idEstado"]);
        compra.Fecha = Convert.ToDateTime(atributos["Fecha"]);
        compra.TotalProveedor = Convert.ToString(atributos["totalProveedor"]);
        compra.TotalCliente = Convert.ToString(atributos["totalCliente"]);
        compra.Iva = Convert.ToInt16(atributos["iva"]);
        compra.Codigo = Convert.ToString(atributos["codigo"]);
        return compra;
    }

    public ArrayList LoadTable()
    {
        ArrayList compras = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        for (int i = 0; idList.Count > i; i++)
        {
            compras.Add(Load(Convert.ToString(idList[i]), null));
        }
        return compras;
    }

    public static string GetIdCompra(string idPedido)
    {
        string query = "SELECT id FROM tCompra WHERE idPedido= '" + idPedido + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public List<cCompra> GetCompraNro(string id)
    {
        List<cCompra> compras = new List<cCompra>();
        string query;
        if (!string.IsNullOrEmpty(id))
            query = "SELECT * FROM tCompra WHERE (id = " + id + ") Order by id ASC";
        else
            query = "SELECT * FROM tCompra WHERE (id = '-1') Order by id ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            compras.Add(Load(Convert.ToString(idList[i]), null));
        }
        return compras;
    }

    public List<cCompra> GetCompra(string id)
    {
        List<cCompra> compras = new List<cCompra>();
        string query = "SELECT * FROM " + GetTable + " WHERE id= " + id + " Order by id ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            compras.Add(Load(Convert.ToString(idList[i]), null));
        }
        return compras;
    }

    public string GetCodigoCompraById(string codigo)
    {
        string query = "SELECT id FROM tCompra WHERE codigo= '" + codigo + "'";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public cCompra GetCompraNro(string id, string idPedido)
    {
        string query = null;
        string _id = null;

        if (!string.IsNullOrEmpty(id))
            query = "SELECT * FROM tCompra WHERE id= '" + id + "' Order by id ASC";

        if (!string.IsNullOrEmpty(idPedido))
            query = "SELECT * FROM tCompra WHERE idPedido= '" + idPedido + "' Order by id ASC";

        SqlCommand com = new SqlCommand(query);
        if(string.IsNullOrEmpty(idPedido))
            _id = cDataBase.GetInstance().ExecuteScalar(com);
                
        if (!string.IsNullOrEmpty(_id))
            return Load(id, null);
        else
            return Load(null, idPedido);
    }

    public List<cCompra> GetCompraByPedido(string idPedido)
    {
        List<cCompra> compras = new List<cCompra>();
        string query = "SELECT * FROM " + GetTable + " WHERE idPedido= " + idPedido + " Order by id ASC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            compras.Add(Load(Convert.ToString(idList[i]), null));
        }
        return compras;
    }

    public string LastCompra()
    {
        string query = "SELECT TOP (1) id FROM " + GetTable + " ORDER BY id DESC";
        SqlCommand com = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
    }

    public static string GetIdLastCompra()
    {
        string query = "SELECT TOP (1) id FROM tCompra ORDER BY id DESC";
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public string UpdateCompra(string id, int estado)
    {
        string query = "UPDATE tCompra SET idEstado=" + estado + " WHERE id=" + id;
        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public List<cCompra> SearchCompra(DateTime? fechaDesde, DateTime? fechaHasta, string id, string idEmpresa, string idCliente, Int16 idEstado, string idUsuario)
    {
        List<cCompra> compras = new List <cCompra>();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();

        query.Append("SELECT  * FROM tCompra WHERE ");

        if (!string.IsNullOrEmpty(id))
        {
            if (Convert.ToInt32(id) > 0)
            {
                query.Append(" id = @id");
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;
            }
        }
        else
            query.Append(" id <> '-1'");

        if (!string.IsNullOrEmpty(idEmpresa) && idEmpresa != "0")
            query.Append(" AND idEmpresa = " + idEmpresa);

        if (!string.IsNullOrEmpty(idCliente) && idCliente != "0")
            query.Append(" AND idCliente = " + idCliente);

        if (idEstado != 0) //si no se selecciona un estado, el listado se llena con las compras que no estan finalizadas
            query.Append(" AND idEstado = " + idEstado);
        else
            query.Append(" AND idEstado <> " + Convert.ToInt16(EstadoCompraNombre.Entregado) + " AND idEstado <> " + Convert.ToInt16(EstadoCompraNombre.Rechazado));

        if (fechaDesde != null)
        {
            query.Append(" AND Fecha >= @FechaDesde");
            cmd.Parameters.Add("@FechaDesde", SqlDbType.DateTime);
            cmd.Parameters["@FechaDesde"].Value = fechaDesde;
        }

        if (fechaHasta != null)
        {
            query.Append(" AND Fecha <= @FechaHasta");
            cmd.Parameters.Add("@FechaHasta", SqlDbType.DateTime);
            cmd.Parameters["@FechaHasta"].Value = fechaHasta;
        }

        if (idUsuario != null)
            query.Append(" AND idUsuario = " + idUsuario);
          
        query.Append(" ORDER  BY id DESC");

        cmd.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            compras.Add(Load(Convert.ToString(idList[i]), null));
        }
        return compras;
    }

    public List<iAutorComentario> GetInvolucrados(cCompra compra)
    {
        List<iAutorComentario> involucrados = new List<iAutorComentario>();

        // Agrego usuario que cargo el ticket
        involucrados.Add((iAutorComentario)cUsuario.Load(compra.IdUsuario));

        // Busco los usuario que comentaron el pedido
        List<cComentarioCompra> comentarios = cComentarioCompra.GetList(compra.Id);
        foreach (cComentarioCompra c in comentarios)
        {
            iAutorComentario involucrado = c.GetAutorComentario();
            if (!involucrados.Contains(involucrado))
                involucrados.Add(involucrado);
        }
        return involucrados;
    }
}
