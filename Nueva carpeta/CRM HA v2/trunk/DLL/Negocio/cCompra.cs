using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data.SqlClient;

public enum EstadoCompraNombre { Nuevo = 0, Cotizado = 1, Aprobado = 2, En_stock = 3, Entregado = 4, Rechazado = 5 };

public class cCompra
{
    private string id;
    private string idPedido;
    private string idUsuario;
    private string idEmpresa;
    private string idCliente;
    private string idEstado;
    private DateTime fecha;
    private string totalProveedor;
    private string totalCliente;
    private Int16 iva;
    private string codigo;

    public cCompra(string _idPedido, string _idUsuario, string _idEmpresa, string _idCliente, string _idEstado, string _fecha, string _totalProveedor, string _totalCliente, string _codigo)
    {
        idPedido = _idPedido;
        idUsuario = _idUsuario;
        idEmpresa = _idEmpresa;
        idCliente = _idCliente;
        idEstado = _idEstado;
        fecha = Convert.ToDateTime(_fecha);
        totalProveedor = _totalProveedor;
        totalCliente = _totalCliente;
        codigo = _codigo;
    }

    public cCompra() { }

    public static cCompra Load(string id, string idPedido)
    {
        cCompraDAO compraDAO = new cCompraDAO();
        return compraDAO.Load(id, null);
    }

    public static ArrayList LoadTable()
    {
        cCompraDAO compraDAO = new cCompraDAO();
        return compraDAO.LoadTable();
    }

    public int Save()
    {
        cCompraDAO compraDAO = new cCompraDAO();
        return compraDAO.Save(this);
    }

    public static List<cCompra> GetCompras(string id)
    {
        cCompraDAO compraDao = new cCompraDAO();
        return compraDao.GetCompraNro(id);
    }

    public static List<cCompra> GetCompra(string id)
    {
        cCompraDAO compraDao = new cCompraDAO();
        return compraDao.GetCompra(id);
    }

    public static List<cCompra> GetCompraByPedido(string idPedido)
    {
        cCompraDAO compraDao = new cCompraDAO();
        return compraDao.GetCompraByPedido(idPedido);
    }

    public static cCompra GetCompraNro(string id, string idPedido)
    {
        cCompraDAO compraDao = new cCompraDAO();
        return compraDao.GetCompraNro(id, idPedido);
    }

    public static string GetCodigoCompraById(string codigo)
    {
        cCompraDAO compraDao = new cCompraDAO();
        return compraDao.GetCodigoCompraById(codigo);
    }

    public string LastCompra()
    {
        cCompraDAO compraDao = new cCompraDAO();
        return compraDao.LastCompra();
    }

    public static string UpdateCompra(string idPedido, int estado)
    {
        cCompraDAO compraDao = new cCompraDAO();
        return compraDao.UpdateCompra(idPedido, estado);
    }

    public static List<cCompra> SearchCompra(string fechaDesde, string fechaHasta, string id, string idEmpresa, string idCliente, Int16 idEstado, string idUsuario)
    {
        cCompraDAO DAO = new cCompraDAO();
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;

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

        return DAO.SearchCompra(_fechaDesde, _fechaHasta, id, idEmpresa, idCliente, idEstado, idUsuario);
    }

    public List<iAutorComentario> GetInvolucrados()
    {
        cCompraDAO DAO = new cCompraDAO();
        return DAO.GetInvolucrados(this);
    }

    public void Comentar(string _idUsuario, string tipo, string comentario, bool envio)
    {
        cComentarioCompra c = new cComentarioCompra(_idUsuario, tipo, id, comentario);
        c.Save();

        //// Envio correo a involucrados
        //if (envio)
        //{
        //    cSendMail mail = new cSendMail();
        //    mail.EnviarComentario(this, GetInvolucrados(), c.Descripcion, c.GetNombreAutor());
        //}
    }

    public void Finalizar(string _idUsuario, string tipo, string comentario, string id)
    {
        cComentarioCompra c = new cComentarioCompra(_idUsuario, tipo, id, comentario);
        c.Save();

        //// Envio correo de finalizacion
        //cSendMail mail = new cSendMail();
        //mail.CrearFinalizarTicket(this);
    }

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string IdPedido
    {
        get { return idPedido; }
        set { idPedido = value; }
    }

    public string GetPedido
    {
        get { return cPedido.Load(IdPedido).Id; }
    }

    public string IdUsuario
    {
        get { return idUsuario; }
        set { idUsuario = value; }
    }

    public string GetUsuario
    {
        get { return cUsuario.Load(idUsuario).Nombre; }
    }

    public string IdEmpresa
    {
        get { return idEmpresa; }
        set { idEmpresa = value; }
    }

    public string GetEmpresa
    {
        get {
            if (idEmpresa == "-1")
                return "";
            else
                return cEmpresa.Load(idEmpresa).Nombre;
            }
    }

    public string IdCliente
    {
        get { return idCliente; }
        set { idCliente = value; }
    }

    public string GetCliente
    {
        get
        {
            if (idCliente == "-1")
                return "";
            else
                return cCliente.Load(idCliente).Nombre;
        }
    }

    public string IdEstado
    {
        get { return idEstado; }
        set { idEstado = value; }
    }

    public string GetEstado
    {
        get
        {
            string estado = null;
            switch (idEstado)
            {
                case "0":
                    estado = EstadoCompraNombre.Nuevo.ToString();
                    break;
                case "1":
                    estado = EstadoCompraNombre.Cotizado.ToString();
                    break;
                case "2":
                    estado = EstadoCompraNombre.Aprobado.ToString();
                    break;
                case "3":
                    estado = EstadoCompraNombre.En_stock.ToString().Replace("_", " ");
                    break;
                case "4":
                    estado = EstadoCompraNombre.Entregado.ToString();
                    break;
                case "5":
                    estado = EstadoCompraNombre.Rechazado.ToString();
                    break;
            }
            return estado;
        }
    }

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public string TotalProveedor
    {
        get { return totalProveedor; }
        set { totalProveedor = value; }
    }

    public string TotalCliente
    {
        get { return totalCliente; }
        set { totalCliente = value; }
    }

    public Int16 Iva
    {
        get { return iva; }
        set { iva = value; }
    }

    public string Codigo
    {
        get { return codigo; }
        set { codigo = value; }
    }
    #endregion

}
