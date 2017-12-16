using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class cItem
{
    private int id;
    private int idPedido;
    private int cantidad;
    private string nombre;
    private string descripcion;
    private string costo;
    private string precio;

    public cItem(int _idPedido, int _cantidad, string _nombre, string _descripcion, string _costo, string _precio)
    {
        idPedido = _idPedido;
        cantidad = _cantidad;
        nombre = _nombre;
        descripcion = _descripcion;
        costo = _costo;
        precio = _precio;
    }

    public cItem()
    {
    }

    public int Insert()
    {
        cItemDAO itemDAO = new cItemDAO();
        return itemDAO.Insert(this);
    }

    public bool Delete()
    {
        cItemDAO itemDAO = new cItemDAO();
        return itemDAO.Delete(this.Id);
    }

    public static ArrayList SearchByIdPedido(int id)
    {
        cItemDAO itemDAO = new cItemDAO();
        return itemDAO.SearchByIdPedido(id);
    }

    public static ArrayList GetItems(DateTime fechaD, DateTime fechaH, string idPedido,string order)
    {
        cItemDAO DAO = new cItemDAO();
        return DAO.GetItems(fechaD, fechaH, idPedido, order);
    }

    #region Propiedades
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public int IdPedido
    {
        get { return idPedido; }
        set { idPedido = value; }
    }

    public int Cantidad
    {
        get { return cantidad; }
        set { cantidad = value; }
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

    public string Costo
    {
        get { return costo; }
        set { costo = value; }
    }

    public string Precio
    {
        get { return precio; }
        set { precio = value; }
    }
    #endregion
}

