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
using DLL.Negocio;

public enum Estado { Nuevo = 1, Finalizado = 2, A_cobrar = 4 };
public enum Prioridad { SinUrgencia = 0, Inmediato = 1, _24hs = 2, _48hs = 3, ProximaVisita = 4 };

public class cPedido
{
    private string id;
    private string idEmpresa;
    private string idCliente;
    private string idUsuario;
    private string titulo;
    private string descripcion;
    private DateTime fecha;
    private DateTime? fechaARealizar;
    private Int16 idEstado;
    private Int16 idCategoria;
    private Int16 idPrioridad;
    private string idResponsable;
    private Int16 idModoResolucion;
    private string idProyecto;

    public cPedido(string _idUsuario, string _idCliente, string _titulo, string _descripcion, Int16 _idCategoria, string _idPrioridad, string _fechaR, int? _idResponsable, string _mensaje, string _idProyecto)
    {
        idUsuario = _idUsuario;

        idCliente = _idCliente;
        titulo = _titulo;
        descripcion = _descripcion;
        idCategoria = _idCategoria;

        idProyecto = _idProyecto;

        idPrioridad = string.IsNullOrEmpty(_idPrioridad) ? Convert.ToInt16(0) : Convert.ToInt16(_idPrioridad);

        if (string.IsNullOrEmpty(_fechaR))
        {
            fechaARealizar = null;
        }
        else
        {
            DateTime dt = DateTime.ParseExact(_fechaR, "dd/MM/yyyy", null);
            fechaARealizar = dt;            
        }

        fecha = DateTime.Now;
        idEstado = (Int16)Estado.Nuevo;
        idEmpresa = cCliente.Load(idCliente).IdEmpresa;

        if (_idResponsable > 0)
        {
            cAsignacionResponsable asignacion = new cAsignacionResponsable(_idResponsable.ToString(), _idUsuario, _mensaje);
            idResponsable = Convert.ToString(asignacion.Save());           
        }
        else
        {
            idResponsable = "-1";
        }
        idModoResolucion = 0; //Ninguno.
    }

    public cPedido() { }

    public void Comentar(string _idUsuario, string tipo, string comentario, bool envio, bool visibilidadCliente)
    {
        //this.idEstado = (Int16)Estado.Pendiente;
        cComentario c = new cComentario(_idUsuario,
                                        tipo,
                                        id,
                                        comentario,
                                        visibilidadCliente);
        c.Save();
        this.Save();

        // Envio correo a involucrados
        if (envio)
        {
            cSendMail mail = new cSendMail();
            mail.EnviarComentario(this, GetInvolucrados(), c.Descripcion, c.GetNombreAutor(), visibilidadCliente);
        }
    }

    public void Finalizar(string _idUsuario, string tabla, string comentario, Int16 idModoResolucion)
    {
        this.idEstado = (Int16)Estado.Finalizado;
        this.idModoResolucion = idModoResolucion;
        cComentario c = new cComentario(_idUsuario, tabla, id, comentario, false);
        c.Save();
        this.Save();

        // Envio correo de finalizacion
        cSendMail mail = new cSendMail();
        mail.CrearFinalizarTicket(this);
    }

    public void Reasignar(string _idResponsable, string _idAsigno, string _comentario, string _idPedido)
    {
        cAsignacionResponsable a = new cAsignacionResponsable();
        a.IdResponsable = _idResponsable;
        a.IdAsigno = _idAsigno;
        a.Comentario = _comentario;
        a.IdPedido = _idPedido;
        a.Fecha = DateTime.Now;
        int idAsig = a.Save();
        cPedido pd = cPedido.Load(_idPedido);
        pd.IdResponsable = idAsig.ToString();
        pd.Save();
        
        // Envio correo de finalizacion
        cSendMail mail = new cSendMail();
        mail.AsignarPedido(pd);
    }

    public static List<cPedido> GetPedidos(string idEmpresa)
    {
        cPedidoDAO pedidoDao = new cPedidoDAO();
        return pedidoDao.GetPedidos(idEmpresa);
    }

    public static DataTable ObtenerReporteXLS(string idEmpresa)
    {
        cPedidoDAO pedidoDao = new cPedidoDAO();
        return pedidoDao.ObtenerReporteXLS(idEmpresa);
    }

    public static Dictionary<string, int> GetEstadoUltimoMes(string idEmpresa)
    {
        cPedidoDAO dao = new cPedidoDAO();
        return dao.GetEstadoUltimoMes(idEmpresa);
    }

    #region Acceso a Datos
    public int Save()
    {
        if (Validar())
        {
            cPedidoDAO pedidoDAO = new cPedidoDAO();
            return pedidoDAO.Save(this);
        }
        return -1;
    }

    public static cPedido Load(string id)
    {
        cPedidoDAO pedidoDAO = new cPedidoDAO();
        return pedidoDAO.Load(id);
    }

    public List<iAutorComentario> GetInvolucrados()
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetInvolucrados(this);
    }

    public static List<cPedido> GetAllPedidosPendientes()
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetAllPedidosPendientes();
    }

    public static List<cPedido> GetPedidosPendientes()
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetPedidosPendientes();
    }

    public static List<cPedido> SearchByEstado(Estado estado)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.SearchByEstado(estado);
    }

    public static List<cPedido> Search(string idEmpresa, string fechaDesde, string fechaHasta, string idProyecto, string estado, Int16 admin, Int16 soporte, Int16 desarrollo, Int16 telco)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        Estado? _estado;

        //Casteo Fecha Desde
        if (string.IsNullOrEmpty(fechaDesde))
            _fechaDesde = null;
        else
            _fechaDesde = Convert.ToDateTime(fechaDesde);

        //Casteo Fecha Hasta
        if (string.IsNullOrEmpty(fechaHasta))
            _fechaHasta = null;
        else
            _fechaHasta = Convert.ToDateTime(fechaHasta).AddDays(1);

        //Casteo Estado
        if (Convert.ToInt16(estado) >= 0)
            _estado = (Estado)Convert.ToInt16(estado);
        else
            _estado = null;

        return DAO.Search(idEmpresa, _fechaDesde, _fechaHasta, idProyecto, _estado, admin, soporte, desarrollo, telco);
    }

    public static List<cPedido> SearchCliente(string idEmpresa, string prioridad, string cliente, string estado, string fechaDesde, string fechaHasta)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        string _prioridad;
        string _cliente;
        string _estado;
        DateTime? _fechaHasta;
        DateTime? _fechaDesde;

        if (string.IsNullOrEmpty(prioridad))
            _prioridad = null;
        else
            _prioridad = prioridad;

        if (string.IsNullOrEmpty(cliente))
            _cliente = null;
        else
            _cliente = cliente;

        if (Convert.ToInt16(estado) >= 0)
            _estado = estado;
        else
            _estado = null;

        if (string.IsNullOrEmpty(fechaDesde))
            _fechaDesde = null;
        else
            _fechaDesde = Convert.ToDateTime(fechaDesde);

        //Casteo Fecha Hasta
        if (string.IsNullOrEmpty(fechaHasta))
            _fechaHasta = null;
        else
            _fechaHasta = Convert.ToDateTime(fechaHasta);

        return DAO.SearchCliente(idEmpresa, _prioridad, _cliente, _estado, _fechaDesde, _fechaHasta);
    }

    public static List<cPedido> GetMisPedidos(string idUsuario)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetMisPedidos(idUsuario);
    }

    public static List<cPedido> GetPedidosVencidos()
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetPedidosVencidos();
    }

    public static DateTime GetFechaPrimerPedido(string idEmpresa)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetFechaPrimerPedido(idEmpresa);
    }

    public static Int64 GetFirstTicket()
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetFirstTicket();
    }

    public static Int64 GetLastTicket()
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetLastTicket();
    }
    #endregion

    #region Validaciones
    public bool Validar()
    {
        try
        {
            bool flag = false;
            flag = Convert.ToInt16(this.idUsuario) <= 0 ? false : true;
            flag = string.IsNullOrEmpty(this.idUsuario) ? false : true;

            flag = Convert.ToInt16(this.idCliente) <= 0 ? false : true;
            flag = string.IsNullOrEmpty(this.idCliente) ? false : true;
            return flag;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string IdEmpresa
    {
        get { return idEmpresa; }
        set { idEmpresa = value; }
    }

    public string IdCliente
    {
        get { return idCliente; }
        set { idCliente = value; }
    }

    public string IdUsuario
    {
        get { return idUsuario; }
        set { idUsuario = value; }
    }

    public string Titulo
    {
        get { return titulo; }
        set { titulo = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public DateTime? FechaRealizacion
    {
        get { return fechaARealizar; }
        set { fechaARealizar = value; }
    }

    public string GetFechaLimite
    {
        get { return fechaARealizar == null ? "-" : String.Format("{0:dd/MM/yyyy}", fechaARealizar); }
    }

    public Int16 IdEstado
    {
        get { return idEstado; }
        set { idEstado = value; }
    }

    public string GetEstado
    {
        get { return Convert.ToString(Enum.ToObject(typeof(Estado), idEstado)).Replace("_", " "); }
    }

    public Int16 IdCategoria
    {
        get { return idCategoria; }
        set { idCategoria = value; }
    }

    public string GetCategoria
    {
        get { return cCampoGenerico.Load(Convert.ToString(idCategoria), Tablas.tCategoria).Descripcion; }
    }

    public Int16 IdPrioridad
    {
        get { return idPrioridad; }
        set { idPrioridad = value; }
    }

    public string GetPrioridad
    {
        get { return cCampoGenerico.Load(Convert.ToString(idPrioridad), Tablas.tPrioridad).Descripcion; }
    }
    public string IdProyecto
    {
        get { return idProyecto; }
        set { idProyecto = value; }
    }
    public string GetProyecto
    {
        get {
            string dd = "";

            if (IdProyecto != "0")
                dd = cProyecto.Load(IdProyecto).Descripcion;
            else
                dd = "";

            return dd;
        
        }
    }
    public string IdResponsable
    {
        get { return idResponsable; }
        set { idResponsable = value; }
    }

    public cAsignacionResponsable GetResponsable()
    {
        return cAsignacionResponsable.GetResponsablePorPedido(IdResponsable) == null ? null : cAsignacionResponsable.GetResponsablePorPedido(IdResponsable);
    }
    
    public string GetResponsableNombre
    {
        get
        {
            string responsable = null;
            if (this.idResponsable != "-1")
            {
                responsable = cUsuario.Load(cAsignacionResponsable.Load(this.idResponsable).IdResponsable) == null ? null : cUsuario.Load(cAsignacionResponsable.Load(this.idResponsable).IdResponsable).Nombre;
            }else
                responsable=null;

            return responsable; 
        }
    }

    public string GetEmpresa
    {
        get
        {
            try { return cCliente.Load(idCliente).GetEmpresa(); }
            catch
            {
                try { return cEmpresa.Load(idEmpresa).Nombre; }
                catch { return "DATO PERDIDO"; }
            }
        }
    }

    public string GetDireccion()
    {
        return cEmpresa.Load(idEmpresa) == null ? null : cEmpresa.Load(idEmpresa).Direccion;
    }

    public string GetTelefono
    {
        get
        {
            if (idEmpresa != "87")
                return cEmpresa.Load(idEmpresa).Telefono == null ? null : cEmpresa.Load(idEmpresa).Telefono;
            else
                return "";
        }
    }

    public cCliente GetCliente()
    {
        return cCliente.Load(idCliente) == null ? null : cCliente.Load(idCliente);
    }

    public string GetClienteNombre
    {
        get
        {
            try { return cCliente.Load(idCliente).Nombre; }
            catch { return "DATO PERDIDO"; }
        }
    }

    public string GetUsuario
    {
        get
        {
            try { return cUsuario.Load(idUsuario).Nombre; }
            catch { return "DATO PERDIDO"; }
        }
    }

    public string GetLastComentarioInfo
    {
        get
        {
            //if (this.idEstado != (Int16)Estado.Finalizado) return "-"; //Si no esta Finalizado devuelvo nada.

            try { return "<b>" + cComentario.GetLastComentario(id).GetNombreAutor() + ":<b/> <i>' " + cComentario.GetLastComentario(id).Descripcion + "'</i> el <b>" + cComentario.GetLastComentario(id).Fecha.ToLongDateString() + " a las " + cComentario.GetLastComentario(id).Fecha.ToShortTimeString() + "·"; }
            catch { return "-"; }
        }
    }

    public cComentario GetComentarioLast()
    {
        return cComentario.GetLastComentario(id) == null ? null : cComentario.GetLastComentario(id);
    }

    public string GetDiasVencidos
    {
        get
        {
            TimeSpan fecha = DateTime.Today - Convert.ToDateTime(fechaARealizar);
            string dias = Convert.ToString(fecha.Days);

            if (dias == "0") return "Hoy";
            if (dias == "-1") return "Vence Manaña";
            if (dias == "1") return "Venció Ayer";
            return dias.Contains("-") ? "Vence en <b>" + dias.Replace("-", "") + "</b> días." : "Venció hace <b>" + dias + "</b> días.";
        }
    }

    public string GetPedidosDias
    {
        get
        {
            TimeSpan intervalo = DateTime.Today - new DateTime(fecha.Year, fecha.Month, fecha.Day);
            string dias = Convert.ToString(intervalo.Days);

            if (dias == "0") return "Hoy";
            if (dias == "1") return "Ayer";
            return fecha.ToString("dd/MM/yyyy");
        }
    }

    public string FechaCierre
    {
        get
        {
            if (this.idEstado == (Int16)Estado.Finalizado)

                return GetComentarioLast().Fecha.ToString("dd/MM/yyyy");

            else
                return "-";
        }
    }

    public Int16 IdModoResolucion
    {
        get { return idModoResolucion; }
        set { idModoResolucion = value; }
    }

    public string GetModoResolucion
    {
        get
        {
            try { return cCampoGenerico.Load(Convert.ToString(idModoResolucion), Tablas.tModoResolucion).Descripcion; }
            catch { return "-"; }
        }
    }
    #endregion

    #region Graficos Nuevos
    public static DataTable GetRanking(string idEmpresa, DatoAGraficar dato, int cantResultados, int cantMeses)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetRanking(idEmpresa, dato, cantResultados, cantMeses);
    }

    public static Dictionary<string, int> GetCantidadPedidosPorMes(int mesesParaGraficar, string idEmpresa, string idCliente)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        Dictionary<string, int> d = new Dictionary<string, int>();

        DateTime date = DateTime.Today.AddMonths(-mesesParaGraficar).AddMonths(mesesParaGraficar == 1 ? 0 : 1);

        for (int i = 0; i < mesesParaGraficar; i++)
        {
            d.Add(String.Format("{0:MMMM-yy}", date).ToString(),
                  DAO.GetCantidadPedidosPorMes(date, idEmpresa, idCliente));

            date = date.AddMonths(+1); //Sumo un mes.
        }
        return d;
    }

    public static Dictionary<string, int> GetTotalPedidosCliente(int mesesParaGraficar, string idEmpresa)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        Dictionary<string, int> d = new Dictionary<string, int>();

        DateTime date = DateTime.Today.AddMonths(-mesesParaGraficar).AddMonths(mesesParaGraficar == 1 ? 0 : 1);
        List<cCliente> clientes = new List<cCliente>();

        if (idEmpresa != null)
            clientes = cCliente.GetClientesByIdEmpresa(idEmpresa);
        else
            clientes = cCliente.GetClientes();

        int value = 0;
        foreach (var cliente in clientes)
        {
            for (int i = 0; i < mesesParaGraficar; i++)
            {
                value = value + DAO.GetTotalPedidosCliente(date, cliente.Id, idEmpresa); //se suma los pedidos de cada cliente desde la fecha minima a la maxima.
                date = date.AddMonths(+1); //Sumo un mes.
            }

            d.Add(cliente.Nombre, value); //se agrega al diccionario el cliente y la cantidad de pedidos.
            date = date.AddMonths(-mesesParaGraficar); //reinicio la fecha
            value = 0; //reinicio el value, para cargar la cantidad de pedidos del próximo cliente
        }
        return d;
    }

    //public static Dictionary<string, int> GetTotalPedidosCerradosPorUsuario(int mesesParaGraficar)
    //{
    //    cPedidoDAO DAO = new cPedidoDAO();
    //    Dictionary<string, int> d = new Dictionary<string, int>();

    //    DateTime date = DateTime.Today.AddMonths(-mesesParaGraficar).AddMonths(mesesParaGraficar == 1 ? 0 : 1);

    //    List<cCliente> clientes = cCliente.GetClientesByIdEmpresa(idEmpresa);

    //    int value = 0;
    //    foreach (var cliente in clientes)
    //    {
    //        for (int i = 0; i < mesesParaGraficar; i++)
    //        {
    //            value = value + DAO.GetTotalPedidosCliente(date, cliente.Id, idEmpresa); //se suma los pedidos de cada cliente desde la fecha minima a la maxima.
    //            date = date.AddMonths(+1); //Sumo un mes.
    //        }

    //        d.Add(cliente.Nombre, value); //se agrega al diccionario el cliente y la cantidad de pedidos.
    //        date = date.AddMonths(-mesesParaGraficar); //reinicio la fecha
    //        value = 0; //reinicio el value, para cargar la cantidad de pedidos del próximo cliente
    //    }
    //    return d;
    //}

    public static Dictionary<string, int[]> GetPedidosPorEstado(int mesesParaGraficar, string _idEmpresa)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        Dictionary<string, int[]> d = new Dictionary<string, int[]>();

        DateTime date = DateTime.Today.AddMonths(-mesesParaGraficar).AddMonths(mesesParaGraficar == 1 ? 0 : 1);

        for (int i = 0; i < mesesParaGraficar; i++)
        {
            d.Add(String.Format("{0:MMMM-yy}", date).ToString(),
                  DAO.GetPedidosPorEstado(date, _idEmpresa));

            date = date.AddMonths(+1); //Resto un mes.
        }
        return d;
    }

    public static Dictionary<string, int[]> GetPedidosPorPrioridad(int mesesParaGraficar, string _idEmpresa)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        Dictionary<string, int[]> d = new Dictionary<string, int[]>();

        DateTime date = DateTime.Today.AddMonths(-mesesParaGraficar).AddMonths(mesesParaGraficar == 1 ? 0 : 1);

        for (int i = 0; i < mesesParaGraficar; i++)
        {
            d.Add(String.Format("{0:MMMM-yy}", date).ToString(),
                  DAO.GetPedidosPorPrioridad(date, _idEmpresa));

            date = date.AddMonths(+1); //Resto un mes.
        }
        return d;
    }

    public static Dictionary<string, int> GetTicketsSolucionadoPorUsuarioUltimoMes()
    {
        cPedidoDAO dao = new cPedidoDAO();
        return dao.GetTicketsSolucionadoPorUsuarioUltimoMes();
    }
    #endregion

    public static DataTable ObtenerReporteSemanalXLS()
    {
        cPedidoDAO pedidoDao = new cPedidoDAO();
        return pedidoDao.ObtenerReporteSemanalXLS();
    }
    
    #region Dias
    public static DateTime diasHabiles_SinUrgencia(DateTime FechaInicial)
    {
        //DateTime FechaInicial = DateTime.Now;
        int i = 0;

        while (i != 6)
        {
            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday)
                FechaInicial = FechaInicial.AddDays(+1);

            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday)
                FechaInicial = FechaInicial.AddDays(+1);

            FechaInicial = FechaInicial.AddDays(+1);
            i++;
        }

        return FechaInicial;
    }

    public static DateTime diasHabiles_24hs(DateTime FechaInicial)
    {
        //DateTime FechaInicial = DateTime.Now;
        int i = 0;

        while (i != 1)
        {
            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday)
                FechaInicial = FechaInicial.AddDays(+1);

            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday)
                FechaInicial = FechaInicial.AddDays(+1);

            FechaInicial = FechaInicial.AddDays(+1);
            i++;
        }

        return FechaInicial;
    }

    public static DateTime diasHabiles_48hs(DateTime FechaInicial)
    {
        //DateTime FechaInicial = DateTime.Now;
        int i = 0;

        while (i != 3)
        {
            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday)
                FechaInicial = FechaInicial.AddDays(+1);

            if (FechaInicial.DayOfWeek == DayOfWeek.Saturday)
                FechaInicial = FechaInicial.AddDays(+1);

            FechaInicial = FechaInicial.AddDays(+1);
            i++;
        }

        return FechaInicial;
    }
    #endregion
} 