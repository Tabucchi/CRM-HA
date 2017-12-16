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
using System.Text;

public enum DatoAGraficar { idCliente, idEmpresa, idModoResolucion, idPrioridad };

public class cPedidoDAO
{
    public string GetTable
    { get { return "tPedido"; } }

    public string GetOrderBy
    { get { return "id DESC"; } }

    public List<cAtributo> AttributesClass(cPedido pedido)
    {
        List<cAtributo> lista = new List<cAtributo>();
        lista.Add(new cAtributo("idEmpresa", pedido.IdEmpresa));
        lista.Add(new cAtributo("idCliente", pedido.IdCliente));
        lista.Add(new cAtributo("idUsuario", pedido.IdUsuario));
        lista.Add(new cAtributo("Titulo", pedido.Titulo));
        lista.Add(new cAtributo("Descripcion", pedido.Descripcion));
        lista.Add(new cAtributo("Fecha", pedido.Fecha));

        if (pedido.FechaRealizacion != null)
            lista.Add(new cAtributo("FechaARealizar", pedido.FechaRealizacion));

        lista.Add(new cAtributo("Estado", pedido.IdEstado));
        lista.Add(new cAtributo("idCategoria", pedido.IdCategoria));
        lista.Add(new cAtributo("idPrioridad", pedido.IdPrioridad));
        lista.Add(new cAtributo("idAsignacionResponsable", pedido.IdResponsable));
        lista.Add(new cAtributo("idModoResolucion", pedido.IdModoResolucion));
        lista.Add(new cAtributo("idProyecto", pedido.IdProyecto));
        return lista;
    }

    public int Save(cPedido pedido)
    {
        if (string.IsNullOrEmpty(pedido.Id))
            return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(pedido));
        else
            return cDataBase.GetInstance().UpdateObject(pedido.Id, GetTable, AttributesClass(pedido));
    }

    public cPedido Load(string id)
    {
        Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
        cPedido pedido = new cPedido();
        pedido.Id = id;
        pedido.IdEmpresa = Convert.ToString(atributos["idEmpresa"]);
        pedido.IdCliente = Convert.ToString(atributos["idCliente"]);
        pedido.IdUsuario = Convert.ToString(atributos["idUsuario"]);
        pedido.Titulo = Convert.ToString(atributos["Titulo"]);
        pedido.Descripcion = Convert.ToString(atributos["Descripcion"]);
        pedido.Fecha = Convert.ToDateTime(atributos["Fecha"]);

        if (atributos["FechaARealizar"] == DBNull.Value)
            pedido.FechaRealizacion = null;
        else
            pedido.FechaRealizacion = Convert.ToDateTime(atributos["FechaARealizar"]);

        pedido.IdEstado = Convert.ToInt16(atributos["Estado"]);
        pedido.IdCategoria = Convert.ToInt16(atributos["idCategoria"]);
        pedido.IdPrioridad = Convert.ToInt16(atributos["idPrioridad"]);
        pedido.IdResponsable = Convert.ToString(atributos["idAsignacionResponsable"]);
        pedido.IdModoResolucion = Convert.ToInt16(atributos["idModoResolucion"]);
        pedido.IdProyecto = Convert.ToString(atributos["idProyecto"]);
        return pedido;
    }

    public ArrayList LoadTable()
    {
        ArrayList pedidos = new ArrayList();
        ArrayList idList = cDataBase.GetInstance().LoadTable(GetTable, GetOrderBy);
        for (int i = 0; idList.Count > i; i++)
        {
            pedidos.Add(Load(Convert.ToString(idList[i])));
        }
        return pedidos;
    }

    public List<iAutorComentario> GetInvolucrados(cPedido pedido)
    {
        List<iAutorComentario> involucrados = new List<iAutorComentario>();

        // Agrego usuario que cargo el ticket
        involucrados.Add((iAutorComentario)cUsuario.Load(pedido.IdUsuario));

        //Agrego cliente del ticket
        involucrados.Add((iAutorComentario)cCliente.Load(pedido.IdCliente));

        // Busco si hay responsable;
        if (Convert.ToInt32(pedido.IdResponsable) > 0)
            involucrados.Add((iAutorComentario)pedido.GetResponsable().GetResponsable());


        // Busco los usuario que comentaron el pedido
        List<cComentario> comentarios = cComentario.GetList(pedido.Id, false);
        foreach (cComentario c in comentarios)
        {
            iAutorComentario involucrado = c.GetAutorComentario();
            if (!involucrados.Contains(involucrado))
                involucrados.Add(involucrado);
        }
        
        return involucrados;
    }

    public List<cPedido> SearchByEstado(Estado estado)
    {
        List<cPedido> pedidos = new List<cPedido>();
        string query = "SELECT id FROM " + GetTable + " WHERE Estado = '" + Convert.ToInt16(estado) + "' ORDER BY id DESC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            pedidos.Add(Load(Convert.ToString(idList[i])));
        }
        return pedidos;
    }

    public List<cPedido> GetAllPedidosPendientes()
    {
        List<cPedido> pedidos = new List<cPedido>();
        string query = "SELECT id FROM " + GetTable + " WHERE Estado = '" + Convert.ToInt16(Estado.Nuevo) + "'";
        
        query += " ORDER BY id DESC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            if (!PedidoVencido(Load(Convert.ToString(idList[i]))))
                pedidos.Add(Load(Convert.ToString(idList[i])));
        }
        return pedidos;
    }

    public List<cPedido> GetPedidosPendientes()
    {
        List<cPedido> pedidos = new List<cPedido>();
        string query = "SELECT id FROM " + GetTable + " WHERE Estado = '" + Convert.ToInt16(Estado.Nuevo) + "'";

        query += " ORDER BY id DESC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            pedidos.Add(Load(Convert.ToString(idList[i])));
        }
        return pedidos;
    }

    public List<cPedido> Search(string idEmpresa, DateTime? fechaDesde, DateTime? fechaHasta, string idProyecto, Estado? estado, Int16 admin, Int16 soporte, Int16 desarrollo, Int16 telco)
    {
        List<cPedido> pedidos = new List<cPedido>();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();

        query.Append("SELECT * FROM " + GetTable + " WHERE ");

        if (Convert.ToInt32(idEmpresa) > 0)
            query.Append(" idEmpresa = @idEmpresa");
        else
            query.Append(" idEmpresa <> @idEmpresa");

        cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
        cmd.Parameters["@idEmpresa"].Value = idEmpresa;

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

        if (estado != null)
        {
            if (estado == Estado.Nuevo)
                query.Append(" AND Estado = @Estado");
            else
                query.Append(" AND Estado = @Estado");
            cmd.Parameters.Add("@Estado", SqlDbType.Int);
            cmd.Parameters["@Estado"].Value = estado;
        }

        if (idProyecto != "0") { query.Append(" AND idProyecto = '" + idProyecto + "'"); }

        //Como en la sentencia no permite buscar en la misma columna por dos valores diferentes (ej: (idCategoria = '2') AND (idCategoria = '3'))
        //lo que hago es buscar por los otros valores que son diferentes a los que busco (ej: (idCategoria <> '1') AND (idCategoria <> '4'))
        //if (admin != 0) { query.Append(" AND idCategoria<> '" + admin + "'"); }
        //if (soporte != 0) { query.Append(" AND idCategoria<> '" + soporte + "'"); }
        //if (desarrollo != 0) { query.Append(" AND idCategoria<> '" + desarrollo + "'"); }
        //if (telco != 0) { query.Append(" AND idCategoria<> '" + telco + "'"); }

        query.Append(" ORDER  BY id DESC");

        cmd.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            pedidos.Add(Load(Convert.ToString(idList[i])));
        }
        return pedidos;
    }

    public List<cPedido> SearchCliente(string idEmpresa, string prioridad, string cliente, string estado, DateTime? fechaDesde, DateTime? fechaHasta)
    {
        List<cPedido> pedidos = new List<cPedido>();
        SqlCommand cmd = new SqlCommand();
        System.Text.StringBuilder query = new System.Text.StringBuilder();

        query.Append("SELECT * FROM " + GetTable + " WHERE ");

        if (Convert.ToInt32(idEmpresa) > 0)
            query.Append(" idEmpresa = @idEmpresa");
        else
            query.Append(" idEmpresa <> @idEmpresa");

        cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
        cmd.Parameters["@idEmpresa"].Value = idEmpresa;

        if (prioridad != null)
        {
            if (Convert.ToInt16(prioridad) >= 0)
            {
                query.Append(" AND idPrioridad = @idPrioridad");
                cmd.Parameters.Add("@idPrioridad", SqlDbType.Int);
                cmd.Parameters["@idPrioridad"].Value = prioridad;
            }
        }

        if (cliente != null)
        {
            if (Convert.ToInt16(cliente) >= 0)
            {
                query.Append(" AND idCliente = @Cliente");
                cmd.Parameters.Add("@Cliente", SqlDbType.Int);
                cmd.Parameters["@Cliente"].Value = cliente;
            }
        }

        if (estado != null)
        {
            if (Convert.ToInt16(estado) >= 0)
            {
                query.Append(" AND Estado = @Estado");
                cmd.Parameters.Add("@Estado", SqlDbType.Int);
                cmd.Parameters["@Estado"].Value = estado;
            }
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

        query.Append(" ORDER  BY id DESC");

        cmd.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(cmd);

        if (idList == null) return null;

        for (int i = 0; idList.Count > i; i++)
        {
            pedidos.Add(Load(Convert.ToString(idList[i])));
        }
        return pedidos;
    }

    #region Metodos Default
    public List<cPedido> GetMisPedidos(string idUsuario)
    {
        List<cPedido> pedidos = new List<cPedido>();
        string query = "SELECT p.id FROM tPedido AS p INNER JOIN tAsignacionResponsable AS a ON p.idAsignacionResponsable = a.id";
        query += " WHERE (p.Estado <> '" + Convert.ToInt16(Estado.Finalizado) + "') AND (a.idResponsable = ' " + idUsuario + " ') ORDER BY p.id DESC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            if (!PedidoVencido(Load(Convert.ToString(idList[i]))))
                pedidos.Add(Load(Convert.ToString(idList[i])));
        }
        return pedidos;
    }

    public List<cPedido> GetPedidosVencidos()
    {
        SqlCommand com = new SqlCommand();
        List<cPedido> pedidos = new List<cPedido>();
        
        DateTime max = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        DateTime min = max.AddDays(-7);
        DateTime? venc;
        TimeSpan ts;

       // string query = "SELECT id FROM " + GetTable + " WHERE (FechaARealizar BETWEEN @fechaDesde AND @fechaHasta AND Estado <> '" + Convert.ToInt16(Estado.Finalizado) + "')";

        string query = "SELECT id FROM " + GetTable + " WHERE (FechaARealizar < @fechaHasta AND Estado <> '" + Convert.ToInt16(Estado.Finalizado) + "')";

        com.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
        com.Parameters["@fechaDesde"].Value = min;

        com.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
        com.Parameters["@fechaHasta"].Value = max;

        com.CommandText = query.ToString();
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            switch(Load(Convert.ToString(idList[i])).IdPrioridad){
                case (Int16)Prioridad.SinUrgencia:
                    venc = Load(Convert.ToString(idList[i])).FechaRealizacion;
                
                    if(venc!=null){
                        ts = max - Convert.ToDateTime(venc).AddDays(-2);

                        if(ts.Days > 5){ //5 dias
                            pedidos.Add(Load(Convert.ToString(idList[i])));
                        }
                    }
                break;
                case (Int16)Prioridad.Inmediato:
                    venc = Load(Convert.ToString(idList[i])).FechaRealizacion;
                
                    if(venc!=null){
                        ts = max - Convert.ToDateTime(venc).AddDays(-1);

                        if(ts.Days >= 1){
                            pedidos.Add(Load(Convert.ToString(idList[i])));
                        }
                    }
                break;
                case (Int16)Prioridad._24hs:
                    venc = Load(Convert.ToString(idList[i])).FechaRealizacion;

                    if (venc != null)
                    {
                        ts = max - Convert.ToDateTime(venc);

                        if (ts.Days >= 1)
                        {
                            pedidos.Add(Load(Convert.ToString(idList[i])));
                        }
                    }
                break;
                case (Int16)Prioridad._48hs:
                    venc = Load(Convert.ToString(idList[i])).FechaRealizacion;

                    if (venc != null)
                    {
                        ts = max - Convert.ToDateTime(venc).AddDays(-1);

                        if (ts.Days > 2)
                        {
                            pedidos.Add(Load(Convert.ToString(idList[i])));
                        }
                    }
                break;

            }            
        }
        return pedidos;

        /*
        List<cPedido> pedidos = new List<cPedido>();
        string query = "SELECT id FROM " + GetTable + " WHERE (Estado <> '" + Convert.ToInt16(Estado.Finalizado) + "') AND FechaARealizar IS NOT NULL ORDER BY FechaARealizar";
        
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null) return null;
        for (int i = 0; idList.Count > i; i++)
        {
            pedidos.Add(Load(Convert.ToString(idList[i])));
        }
        return pedidos;*/
    }

    public bool PedidoVencido(cPedido pedido)
    {
        DateTime max = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        DateTime? venc;
        TimeSpan ts;
        bool flag = false;
        venc = pedido.FechaRealizacion;

        switch (pedido.IdPrioridad)
        {
            case (Int16)Prioridad.SinUrgencia:

                if (venc != null)
                {
                    ts = max - Convert.ToDateTime(venc).AddDays(-2);

                    if (ts.Days >= 5)
                    {   //5 dias
                        flag = true;
                    }
                }
                break;
            case (Int16)Prioridad.Inmediato:

                if (venc != null)
                {
                    ts = max - Convert.ToDateTime(venc).AddDays(-1);

                    if (ts.Days >= 1)
                    {
                        flag = true;
                    }
                }
                break;
            case (Int16)Prioridad._24hs:

                if (venc != null)
                {
                    ts = max - Convert.ToDateTime(venc);

                    if (ts.Days >= 1)
                    {
                        flag = true;
                    }
                }
                break;
            case (Int16)Prioridad._48hs:

                if (venc != null)
                {
                    ts = max - Convert.ToDateTime(venc).AddDays(-1);

                    if (ts.Days > 2)
                    {
                        flag = true;
                    }
                }
                break;
        }
        return flag;
    }

    public List<cPedido> GetPedidos(string idEmpresa)
    {
        List<cPedido> pedidos = new List<cPedido>();
        string query = "SELECT id FROM " + GetTable + " WHERE idEmpresa = " + idEmpresa + "ORDER BY Fecha DESC";
        SqlCommand com = new SqlCommand(query);
        ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
        if (idList == null)
            return null;
        for (int i = 0; idList.Count > i; i++)
        {
            pedidos.Add(Load(Convert.ToString(idList[i])));
        }
        return pedidos;
    }
    #endregion

    #region Estadisticas Nuevas
    /* OK */
    public DataTable GetRanking(string idEmpresa, DatoAGraficar dato, int cantResultados, int cantMeses)
    {
        SqlCommand cmd = new SqlCommand();

        DateTime max = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day).AddDays(1);  //Sumo un dia para que tome los tickets de hoy.
        DateTime min = max.AddMonths(-cantMeses);

        string query = "SELECT TOP (" + cantResultados + ") COUNT(id) as cant, ";
        query += dato;
        query += " FROM " + GetTable;
        query += " WHERE (Fecha BETWEEN @fechaDesde AND @fechaHasta)";

        if (!string.IsNullOrEmpty((idEmpresa)))
            query += " AND idEmpresa = " + idEmpresa;

        query += " GROUP BY " + dato + " ORDER BY cant DESC";

        cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
        cmd.Parameters["@fechaDesde"].Value = min;

        cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
        cmd.Parameters["@fechaHasta"].Value = max;

        cmd.CommandText = query.ToString();

        DataSet dataSet = cDataBase.GetInstance().GetDataSet(cmd, GetTable);

        if (dataSet == null) return null;

        return dataSet.Tables[0];
    }

    public int GetCantidadPedidosPorMes(DateTime date, string idEmpresa, string idCliente)
    {
        SqlCommand cmd = new SqlCommand();
        DateTime min = new DateTime(date.Year, date.Month, 1);
        DateTime max = new DateTime(date.Month == 12 ? date.Year + 1 : date.Year, date.Month == 12 ? 1 : date.Month + 1, 1);

        string query = "SELECT COUNT(id) FROM " + GetTable + " WHERE Fecha BETWEEN @fechaDesde AND @fechaHasta";

        if (!string.IsNullOrEmpty(idCliente))
        {
            if (idCliente != "-1")
            {
                query += " AND idCliente = @idCliente";
                cmd.Parameters.Add("@idCliente", SqlDbType.Int);
                cmd.Parameters["@idCliente"].Value = idCliente;
            }
        }

        if (!string.IsNullOrEmpty(idEmpresa) && (idEmpresa != "-1") && (idEmpresa != "0"))
        {
            query += " AND idEmpresa = @idEmpresa";
            cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
            cmd.Parameters["@idEmpresa"].Value = idEmpresa;
        }

        cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
        cmd.Parameters["@fechaDesde"].Value = min;

        cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
        cmd.Parameters["@fechaHasta"].Value = max;

        cmd.CommandText = query.ToString();
        return Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    //Trae pedidos por estado, para graficar con distintos colores
    public int[] GetPedidosPorEstado(DateTime date, string _idEmpresa)
    {
        SqlCommand cmd;
        DateTime min = new DateTime(date.Year, date.Month, 1);
        DateTime max = new DateTime(date.Month == 12 ? date.Year + 1 : date.Year, date.Month == 12 ? 1 : date.Month + 1, 1);

        ArrayList estados = cCampoGenerico.LoadTable(Tablas.tEstado);
        ArrayList prioridades = cCampoGenerico.LoadTable(Tablas.tPrioridad);
        int[] cantidad = new int[estados.Count];
        int[] cant_prioridad = new int[prioridades.Count];

        foreach (cCampoGenerico e in estados)
        {
            cmd = new SqlCommand();
            string query = "SELECT COUNT(id) FROM " + GetTable + " WHERE Fecha BETWEEN @fechaDesde AND @fechaHasta";

            if (_idEmpresa != "0")
                query += " AND idEmpresa = " + _idEmpresa;
            query += " AND Estado = " + e.Id;
            query += " GROUP BY Estado";

            if (_idEmpresa != null)
            {
                cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
                cmd.Parameters["@idEmpresa"].Value = _idEmpresa;
            }

            cmd.Parameters.Add("@Estado", SqlDbType.Int);
            cmd.Parameters["@Estado"].Value = e.Id;

            cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            cmd.Parameters["@fechaDesde"].Value = min;

            cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            cmd.Parameters["@fechaHasta"].Value = max;

            cmd.CommandText = query.ToString();
            var res = new ArrayList();
            res.Add(cDataBase.GetInstance().ExecuteReader(cmd));
            var aux = Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));

            switch (e.Id)
            {
                case "0":
                    cantidad[0] = Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
                    break;
                case "2":
                    cantidad[Convert.ToInt32(e.Id) - 1] = Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
                    break;
                case "4":
                    cantidad[Convert.ToInt32(e.Id) - 2] = Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
                    break;
            }
        }
        return cantidad;
    }

    //Trae pedidos por prioridad, para graficar con distintos colores
    public int[] GetPedidosPorPrioridad(DateTime date, string _idEmpresa)
    {
        SqlCommand cmd;
        DateTime min = new DateTime(date.Year, date.Month, 1);
        DateTime max = new DateTime(date.Month == 12 ? date.Year + 1 : date.Year, date.Month == 12 ? 1 : date.Month + 1, 1);

        ArrayList prioridades = cCampoGenerico.LoadTable(Tablas.tPrioridad);
        int[] cant_prioridad = new int[prioridades.Count];

        foreach (cCampoGenerico e in prioridades)
        {
            cmd = new SqlCommand();
            string query = "SELECT COUNT(id) FROM " + GetTable + " WHERE Fecha BETWEEN @fechaDesde AND @fechaHasta";

            if (_idEmpresa != "0")
                query += " AND idEmpresa = " + _idEmpresa;

            query += " AND idPrioridad = " + e.Id;
            query += " GROUP BY idPrioridad";

            if (_idEmpresa != "0")
            {
                cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
                cmd.Parameters["@idEmpresa"].Value = _idEmpresa;
            }

            cmd.Parameters.Add("@idPrioridad", SqlDbType.Int);
            cmd.Parameters["@idPrioridad"].Value = e.Id;

            cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
            cmd.Parameters["@fechaDesde"].Value = min;

            cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
            cmd.Parameters["@fechaHasta"].Value = max;

            cmd.CommandText = query.ToString();
            var res = new ArrayList();
            res.Add(cDataBase.GetInstance().ExecuteReader(cmd));
            var aux = Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));

            cant_prioridad[Convert.ToInt32(e.Id)] = Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
        }
        return cant_prioridad;
    }

    //Grafico de Tortas Pedidos por Estado el último mes (para Estadistica Cliente)
    public Dictionary<string, int> GetEstadoUltimoMes(string idEmpresa)
    {
        Dictionary<string, int> d = new Dictionary<string, int>();
        SqlCommand cmd = new SqlCommand();
        DateTime hoy = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day + 1); //Ajusto la fecha sumando un día.
        DateTime antes = hoy.AddMonths(-1);

        string query = "select COUNT(estado) as cantidad, es.Tipo as tipo FROM " + GetTable + " inner join tEstado es on es.id = tPedido.Estado";
        query += " WHERE (Fecha BETWEEN @fechaDesde AND @fechaHasta)";

        if (!string.IsNullOrEmpty(idEmpresa))
        {
            query += " AND idEmpresa = @idEmpresa ";

            cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
            cmd.Parameters["@idEmpresa"].Value = idEmpresa;
        }

        query += "GROUP BY es.tipo";

        cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
        cmd.Parameters["@fechaDesde"].Value = antes;

        cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
        cmd.Parameters["@fechaHasta"].Value = hoy;

        cmd.CommandText = query.ToString();
        DataSet dataSet = cDataBase.GetInstance().GetDataSet(cmd, GetTable);
        if (dataSet == null) return null;
        DataTable dt = dataSet.Tables[0];

        foreach (DataRow row in dt.Rows)
        {
            d.Add(Convert.ToString(row["tipo"]), Convert.ToInt32(row["cantidad"]));
        }
        return d;
    }

    //Total de pedidos por Cliente entre dos fechas dadas.
    public int GetTotalPedidosCliente(DateTime date, string idCliente, string idEmpresa)
    {
        SqlCommand cmd = new SqlCommand();
        /*nuevo*/
        DateTime min = new DateTime(date.Year, date.Month, 1);
        DateTime max = new DateTime(date.Month == 12 ? date.Year + 1 : date.Year, date.Month == 12 ? 1 : date.Month + 1, 1);

        string query = "SELECT COUNT(tPedido.id), cl.Nombre FROM " + GetTable + " INNER JOIN tCliente cl on cl.id = tPedido.idCliente WHERE Fecha BETWEEN @fechaDesde AND @fechaHasta ";

        if (!string.IsNullOrEmpty(idEmpresa))
        {
            query += " AND tPedido.idEmpresa = @idEmpresa";

            cmd.Parameters.Add("@idEmpresa", SqlDbType.Int);
            cmd.Parameters["@idEmpresa"].Value = idEmpresa;
        }

        query += " AND cl.id=" + idCliente + " GROUP BY cl.Nombre";

        /*nuevo*/
        cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
        cmd.Parameters["@fechaDesde"].Value = min;

        cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
        cmd.Parameters["@fechaHasta"].Value = max;

        cmd.CommandText = query.ToString();
        return Convert.ToInt32(cDataBase.GetInstance().ExecuteScalar(cmd));
    }
    #endregion

    public DataTable ObtenerReporteXLS(string idEmpresa)
    {
        string query = "SELECT    p.id, c.nombre, p.Fecha, p.Titulo, cat.Tipo as Categoria, es.Tipo as Estado , pr.Tipo as Prioridad, com.Fecha as FechaCierre";
        query += " FROM tPedido p INNER JOIN tCliente c ON p.idCliente = c.id";
        query += " INNER JOIN tCategoria cat ON p.idCategoria = cat.id";
        query += " INNER JOIN tEstado es ON es.id = p.Estado";
        query += " INNER JOIN tPrioridad pr ON pr.id = p.idPrioridad";
        query += " LEFT JOIN (select MAX (Fecha) as Fecha, idPedido from tComentario group by idPedido) com ON com.idPedido = p.id";
        query += " WHERE p.idEmpresa = @idEmpresa";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.Add("@idEmpresa", SqlDbType.BigInt);
        cmd.Parameters["@idEmpresa"].Value = Int64.Parse(idEmpresa);
        return cDataBase.GetInstance().GetDataReader(cmd);
    }

    public DateTime GetFechaPrimerPedido(string idEmpresa)
    {
        string query = "SELECT TOP (1) Fecha FROM " + GetTable;

        if (idEmpresa != "0")
        {
            query += " WHERE (idEmpresa = " + idEmpresa + ")";
        }

        query += " ORDER BY Fecha";

        SqlCommand cmd = new SqlCommand(query);
        return Convert.ToDateTime(cDataBase.GetInstance().ExecuteScalar(cmd));
    }

    public Int64 GetFirstTicket()
    {
        //string query = "SELECT TOP (1) id FROM " + GetTable + "WHERE Estado = ' " + (Int16)Estado.Nuevo + "'";
        string query = "SELECT TOP (1) id FROM " + GetTable;
        SqlCommand com = new SqlCommand();
        com.CommandText = query.ToString();
        return Convert.ToInt64(cDataBase.GetInstance().ExecuteScalar(com));
    }

    public Int64 GetLastTicket()
    {
        //string query = "SELECT TOP (1) id FROM " + GetTable + "WHERE Estado = ' " + (Int16)Estado.Nuevo + "' ORDER BY id DESC";
        string query = "SELECT TOP (1) id FROM " + GetTable + " ORDER BY id DESC";
        SqlCommand com = new SqlCommand();
        com.CommandText = query.ToString();
        return Convert.ToInt64(cDataBase.GetInstance().ExecuteScalar(com));
    }

    //Grafico de Tortas Tickets Solucionados por Usuario el último mes
    public Dictionary<string, int> GetTicketsSolucionadoPorUsuarioUltimoMes()
    {
        Dictionary<string, int> d = new Dictionary<string, int>();
        SqlCommand cmd = new SqlCommand();
        DateTime date = DateTime.Today;
        DateTime min = new DateTime(date.Year, date.Month, 1);
        DateTime max = new DateTime(date.Month == 12 ? date.Year + 1 : date.Year, date.Month == 12 ? 1 : date.Month + 1, 1);

        string query = "SELECT COUNT(u.id) AS cantidad, u.Nombre ";
        query += " FROM tPedido AS p INNER JOIN tAsignacionResponsable AS a ON p.id = a.idPedido INNER JOIN tUsuario AS u ON u.id = p.idUsuario "; //CAMBIAR p.idUsuario x a.idResponsable
        query += " WHERE (p.Estado = '0') GROUP BY u.Nombre";

       /* string query = "SELECT COUNT(u.id) AS cantidad, u.Nombre FROM tComentario AS c INNER JOIN tUsuario AS u ON c.idUsuario = u.id";
        query += " WHERE(c.id IN(SELECT MAX(id) AS ID FROM tComentario GROUP BY idPedido HAVING(idPedido IN(SELECT DISTINCT c.idPedido";
        query += " FROM tComentario AS c INNER JOIN tPedido AS p ON c.idPedido = p.id WHERE (p.Estado = 0)))))";
        query += " GROUP BY u.Nombre";

        cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime);
        cmd.Parameters["@fechaDesde"].Value = min;

        cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime);
        cmd.Parameters["@fechaHasta"].Value = max;*/

        cmd.CommandText = query.ToString();
        DataSet dataSet = cDataBase.GetInstance().GetDataSet(cmd, GetTable);
        if (dataSet == null) return null;
        DataTable dt = dataSet.Tables[0];

        foreach (DataRow row in dt.Rows)
        {
            d.Add(Convert.ToString(row["Nombre"]), Convert.ToInt32(row["cantidad"]));
        }

        return d;
    }

    public DataTable ObtenerReporteSemanalXLS()
    {
        SqlCommand com = new SqlCommand();

        string query = "SELECT e.Nombre AS Empresa, e.Direccion AS Direccion, e.Telefono AS Telefono, c.Nombre AS Cliente, c.Interno AS Interno, c.Mail AS Mail ";
        query += "FROM tCliente AS c INNER JOIN tEmpresa AS e ON c.idEmpresa = e.id ";
        query += " Order by e.Nombre";

        com.CommandText = query.ToString();
        return cDataBase.GetInstance().GetDataReader(com);
    }

}



