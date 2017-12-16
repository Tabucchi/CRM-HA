using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class cItem2
{
    private string id;
    private string idPedido;
    private string nombre;
    private string descripcion;
    private Int16 cantidad;
    private string costo;
    private string precioCliente;
    private DateTime? fecha;
    private string idUsuario;
    private string idEstado;
    private string idAprobo;

	public cItem2()
	{
    }

    public int Save()
    {
        cItemDAO2 itemDao = new cItemDAO2();
        return itemDao.Save(this);
    }

    public static List<cItem2> GetItems()
    {
        cItemDAO2 itemDao = new cItemDAO2();
        return itemDao.GetItems();
    }

    public static List<cItem2> GetItemsPedido(string _idPedido)
    {
        cItemDAO2 itemDao = new cItemDAO2();
        return itemDao.GetItemsPedido(_idPedido);
    }
       
    public static List<cItem2> Search(string idEmpresa, string fechaDesde, string fechaHasta)
    {
        cItemDAO2 DAO = new cItemDAO2();
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

        return DAO.Search(idEmpresa, _fechaDesde, _fechaHasta);
        
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

    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    public Int16 Cantidad
    {
        get { return cantidad; }
        set { cantidad = value; }
    }

    public string Costo
    {
        get { return costo; }
        set { costo = value; }
    }

    public string PrecioCliente
    {
        get { return precioCliente; }
        set { precioCliente = value; }
    }

    public DateTime? Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public string IdUsuario
    {
        get { return idUsuario; }
        set { idUsuario = value; }
    }

    public string GetUsuario
    {
        get
        {
            try { return cUsuario.Load(idUsuario).Nombre; }
            catch
            {
                if (idUsuario == null)
                    return "";
                else
                    return "-";
            }
        }
    }

    public string IdEstado
    {
        get { return idEstado; }
        set { idEstado = value; }
    }

    public string IdAprobo
    {
        get { return idAprobo; }
        set { idAprobo = value; }
    }

    public string GetEmpresaNombre
    {
        get
        {
            return cPedido.Load(idPedido).GetEmpresa;
        }
    }
    #endregion
}
