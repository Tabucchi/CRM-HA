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

public class cPedido
{
    private int id;
    private int idEmpresa;
    private int idCliente;
    private int idUsuario;
    private string titulo;
    private string descripcion;
    private DateTime fecha;
    private string fechaARealizar = "";
    private int estado;
    private ArrayList comentarios = new ArrayList();
    private int idCategoria;
    private int idAsignacionResponsable;

    public cPedido(int _idEmpresa, int _idCliente, int _idUsuario, string _titulo, string _descripcion, string _fechaARelizar, int _idCategoria)
    {
        idEmpresa = _idEmpresa;
        idCliente = _idCliente;
        idUsuario = _idUsuario;
        titulo = _titulo;
        descripcion = _descripcion;
        fecha = DateTime.Now;
        fechaARealizar = _fechaARelizar;
        estado = 0;
        idCategoria = _idCategoria;
        idAsignacionResponsable = -1;
    }

    public cPedido() { }

    public int Insert()
    {
        cPedidoDAO pedidoDAO = new cPedidoDAO();
        return pedidoDAO.Insert(this);
    }

    public bool Update()
    {
        cPedidoDAO pedidoDAO = new cPedidoDAO();
        return pedidoDAO.Update(this);
    }

    public static cPedido Load(int id)
    {
        cPedidoDAO pedidoDAO = new cPedidoDAO();
        return pedidoDAO.Load(id);
    }

    public static int GetNextId()
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetNextId();
    }

    public DateTime GetLastComment()
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetLastCommentDate(this.Id);
    }

    public ArrayList GetInvolucrados() // Devuelve a todos los usuarios involucrados en el pedido.
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetInvolucrados(this.id);
    }

    public DataSet GetDataSet()
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetReportFinishOrders();
    }

    public static ArrayList Filter(int? idEmpresa, int? idCliente, int? idUsuario, DateTime? fechaDesde, DateTime? fechaHasta, DateTime? fechaRealizacion, int? Estado, int? idCategoria)
    {
        cPedidoDAO pedidoDAO = new cPedidoDAO();
        return pedidoDAO.Filter(idEmpresa, idCliente, idUsuario, fechaDesde, fechaHasta, fechaRealizacion, Estado, idCategoria);
    }
    public static ArrayList GetNewsOrders(int idResponsable)
    {
        cPedidoDAO DAO = new cPedidoDAO();
        return DAO.GetNewsOrders(idResponsable);
    }

    #region Propiedades
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public int IdEmpresa
    {
        get { return idEmpresa; }
        set { idEmpresa = value; }
    }

    public int IdCliente
    {
        get { return idCliente; }
        set { idCliente = value; }
    }

    public int IdUsuario
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

    public string FechaRealizacion
    {
        get { return fechaARealizar; }
        set { fechaARealizar = value; }
    }

    public int Estado
    {
        get { return estado; }
        set { estado = value; }
    }

    public ArrayList Comentarios
    {
        get { return comentarios; }
        set { comentarios = value; }
    }

    public int IdCategoria
    {
        get { return idCategoria; }
        set { idCategoria = value; }
    }

    public int IdAsignacionResponsable
    {
        get { return idAsignacionResponsable; }
        set { idAsignacionResponsable = value; }
    }
    #endregion
}