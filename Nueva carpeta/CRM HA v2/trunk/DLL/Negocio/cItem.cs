using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

public class cItem
{
    private string id;
    private string cantidad;
    private string descripcion;
    private string importeProveedor;
    private string importeCliente;
    private string idProveedor;
    private string idCompra;
    private string nroPedidoProveedor;

    public cItem(string _cantidad, string _descripcion, string _Importe, string _idProveedor, string _idCompra, string _nroPedidoProveedor)
    {
        cantidad = _cantidad;
        descripcion = _descripcion;
        importeProveedor = _Importe;
        idProveedor = _idProveedor;
        idCompra = _idCompra;
        nroPedidoProveedor = _nroPedidoProveedor;
    }

    public cItem() { }

    public static cItem Load(string id)
    {
        cItemDAO itemDAO = new cItemDAO();
        return itemDAO.Load(id);
    }

    public int Save()
    {
        cItemDAO itemDAO = new cItemDAO();
        return itemDAO.Save(this);
    }

    public bool Delete(string id)
    {
        cItemDAO itemDAO = new cItemDAO();
        return itemDAO.Delete(id);
    }

    public static List<cItem> GetItems(string idCompra)
    {
        cItemDAO itemDAO = new cItemDAO();
        return itemDAO.GetItems(idCompra);
    }

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Cantidad
    {
        get { return cantidad; }
        set { cantidad = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    public string ImporteProveedor
    {
        get { return importeProveedor; }
        set { importeProveedor = value; }
    }

    public string ImporteCliente
    {
        get { return importeCliente; }
        set { importeCliente = value; }
    }

    public string IdProveedor
    {
        get { return idProveedor; }
        set { idProveedor = value; }
    }

    public string GetProveedor
    {
        get { return cProveedor.Load(idProveedor).Nombre; }
    }

    public string IdCompra
    {
        get { return idCompra; }
        set { idCompra = value; }
    }

    public string NroPedidoProveedor
    {
        get { return nroPedidoProveedor; }
        set { nroPedidoProveedor = value; }
    }

    public string GetNroPedidoProveedor
    {
        get
        {
            if (nroPedidoProveedor == "-1")
                return "";
            else
                return nroPedidoProveedor;
        }
    }
    #endregion
}
