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

public class cPedidoDAO
{
    public string GetTable
    { get { return "tPedido"; } }

    public string GetOrderBy
    { get { return "id DESC"; } }

    public string GetView
    { get { return "vPedido"; } }

    public List<cAtributo> AttributesClass(cPedido pedido)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idEmpresa", pedido.IdEmpresa));
        lista.Add(new cAtributo("idCliente", pedido.IdCliente));
        lista.Add(new cAtributo("idUsuario", pedido.IdUsuario));
        lista.Add(new cAtributo("Titulo", pedido.Titulo));
        lista.Add(new cAtributo("Descripcion", pedido.Descripcion));
        lista.Add(new cAtributo("Fecha", pedido.Fecha));
        if (pedido.FechaRealizacion != "")
            lista.Add(new cAtributo("FechaARealizar", Convert.ToDateTime(pedido.FechaRealizacion)));
        lista.Add(new cAtributo("Estado", pedido.Estado));
        lista.Add(new cAtributo("idCategoria", pedido.IdCategoria));
        lista.Add(new cAtributo("idAsignacionResponsable", pedido.IdAsignacionResponsable));
        return lista;
    }

    public int Insert(cPedido pedido)
    {
        return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(pedido));
    }

    public bool Update(cPedido pedido)
    {
        return cDataBase.GetInstance().UpdateObject(pedido.Id.ToString(), GetTable, AttributesClass(pedido));
    }

    public cPedido Load(int id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cPedido pedido = new cPedido();
        pedido.Id = id;
        pedido.IdEmpresa = Convert.ToInt32(atributos["idEmpresa"]);
        pedido.IdCliente = Convert.ToInt32(atributos["idCliente"]);
        pedido.IdUsuario = Convert.ToInt32(atributos["idUsuario"]);
        pedido.Titulo = Convert.ToString(atributos["Titulo"]);
        pedido.Descripcion = Convert.ToString(atributos["Descripcion"]);
        pedido.Fecha = Convert.ToDateTime(atributos["Fecha"]);
        if (Convert.ToString(atributos["FechaARealizar"]) == "")
            pedido.FechaRealizacion = Convert.ToString(atributos["FechaARealizar"]);
        else
            pedido.FechaRealizacion = Convert.ToDateTime(atributos["FechaARealizar"]).ToShortDateString();
        pedido.Estado = Convert.ToInt16(atributos["Estado"]);
        pedido.IdCategoria = Convert.ToInt32(atributos["idCategoria"]);
        pedido.IdAsignacionResponsable = Convert.ToInt32(atributos["idAsignacionResponsable"]);
        return pedido;
    }


    public ArrayList LoadTable()
    {
        ArrayList pedidos = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        for (int i = 0; idList.Count > i; i++)
        {
            pedidos.Add(Load(Convert.ToInt32(idList[i])));
        }
        return pedidos;
    }

    public ArrayList Filter(int? idEmpresa, int? idCliente, int? idUsuario, DateTime? fechaDesde, DateTime? fechaHasta, DateTime? fechaRealizacion, int? estado, int? idCategoria)
    {
        ArrayList pedidos = new ArrayList();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        query.Append("SELECT * FROM " + GetTable + " WHERE ");
        if (idEmpresa != null)
        {
            query.Append(" idEmpresa = @idEmpresa");
            cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
            cmd.Parameters["@idEmpresa"].Value = idEmpresa;
        }
        else { query.Append(" idEmpresa <> -1"); }
        if (idCliente != null)
        {
            query.Append(" AND idCliente = @idCliente");
            cmd.Parameters.Add("@idCliente", SqlDbType.Int);
            cmd.Parameters["@idCliente"].Value = idCliente;
        }
        if (idUsuario != null)
        {
            query.Append(" AND idUsuario = @idUsuario");
            cmd.Parameters.Add("@idUsuario", SqlDbType.Int);
            cmd.Parameters["@idUsuario"].Value = idUsuario;
        }
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
        if (fechaRealizacion != null)
        {
            query.Append(" AND FechaARealizar = @FechaRealizar");
            cmd.Parameters.Add("@FechaRealizar", SqlDbType.DateTime);
            cmd.Parameters["@FechaRealizar"].Value = fechaRealizacion;
        }
        if (estado != null)
        {
            if ((estado == 0) || (estado == 1))
                query.Append(" AND Estado <> @Estado");
            else
                query.Append(" AND Estado = @Estado");
            cmd.Parameters.Add("@Estado", SqlDbType.Int);
            cmd.Parameters["@Estado"].Value = 2;
        }
        if (idCategoria != null)
        {
            query.Append(" AND idCategoria = @idCategoria");
            cmd.Parameters.Add("@idCategoria", SqlDbType.Int);
            cmd.Parameters["@idCategoria"].Value = idCategoria;
        }

        query.Append(" ORDER BY id DESC");

        cmd.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            pedidos.Add(Load(Convert.ToInt32(idList[i])));
        }
        return pedidos;
    }

    public ArrayList GetNewsOrders(int idResponsable)
    {
        ArrayList pedidos = new ArrayList();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();
        query.Append("SELECT p.id FROM " + GetTable + " p INNER JOIN tAsignacionResponsable r ON p.id = r.idPedido WHERE ");
        query.Append("r.idResponsable = @idResponsable");
        cmd.Parameters.Add("@idResponsable", SqlDbType.Int);
        cmd.Parameters["@idResponsable"].Value = idResponsable;
        query.Append(" AND p.Estado <> 2");

        cmd.CommandText = query.ToString();
        
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++) {
            pedidos.Add(Load(Convert.ToInt32(idList[i])));
        }
        return pedidos;
    }

    public DateTime GetLastCommentDate(int idPedido)
    {
        string query = "SELECT Fecha FROM tComentario WHERE (id = ";
        query += "(SELECT MAX(c.id) FROM tPedido p INNER JOIN tComentario c ";
        query += "ON p.id = c.idPedido WHERE (p.id = @idPedido)))";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.Add("@idPedido", SqlDbType.Int);
        cmd.Parameters["@idPedido"].Value = idPedido;
        return Convert.ToDateTime(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public ArrayList GetInvolucrados(int idPedido)
    {
        ArrayList usuarios = new ArrayList();
        // Busco si hay responsable;
        cPedido pedido = cPedido.Load(idPedido);
        if (pedido.IdAsignacionResponsable != -1) {
            cAsignacionResponsable asignacion = cAsignacionResponsable.Load(pedido.IdAsignacionResponsable);
            cUsuario usuario = cUsuario.Load(asignacion.IdResponsable);
            usuarios.Add(usuario);
        }
        // Busco los usuario que comentaron el pedido
        ArrayList comentarios = cComentario.SearchByIdPedido(idPedido);
        for (int i = 0; comentarios.Count > i; i++) {
            cComentario comentario = (cComentario) comentarios[i];
            cUsuario usuario = cUsuario.Load(comentario.IdUsuario);
            usuarios.Add(usuario);
        }

        //elimino los repetidos
        ArrayList _usuarios = new ArrayList();
        _usuarios.Add(usuarios[0]);
        foreach (cUsuario usuario in usuarios) {
            bool hit = false;
            for (int i = 0; _usuarios.Count > i; i++) {
                cUsuario _usuario = (cUsuario)_usuarios[i];
                if (usuario.Nombre == _usuario.Nombre)
                    hit = true;
            }
            if (!hit)
                _usuarios.Add(usuario);
        }        
        return _usuarios;
    }

    public int GetNextId()
    {
        return cDataBase.GetInstance().GetIdentity(GetTable) + 1;
    }

    public DataSet GetReportFinishOrders()
    {
        ArrayList querys = new ArrayList();
        ArrayList nombreTablas = new ArrayList();
        string query = "SELECT * FROM tPedido WHERE Estado = 2";
        querys.Add(query);
        nombreTablas.Add("tPedido");

        query = "SELECT * FROM tCliente";
        querys.Add(query);
        nombreTablas.Add("tCliente");

        query = "SELECT * FROM tEmpresa";
        querys.Add(query);
        nombreTablas.Add("tEmpresa");

        query = "SELECT * FROM tComentario";
        querys.Add(query);
        nombreTablas.Add("tCliente");

        return cDataBase.GetInstance().GetDataSet(querys, nombreTablas);
    }
}
