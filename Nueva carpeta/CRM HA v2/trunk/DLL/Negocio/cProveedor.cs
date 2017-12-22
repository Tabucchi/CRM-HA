using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

public class cProveedor
{
    private string id;
    private string nombre;
    private string direccion;
    private string telefono;
    private string mail;
    private Int16 papelera;

	public cProveedor(string _nombre, string _direccion, string _telefono, string _mail)
	{
        _nombre = nombre;
        _direccion = direccion;
        _telefono = telefono;
        _mail = mail;
	}

    public cProveedor() { }

    public int Save()
    {
        cProveedorDAO DAO = new cProveedorDAO();
        return DAO.Save(this);
    }

    public static cProveedor Load(string id)
    {
        cProveedorDAO proveedorDAO = new cProveedorDAO();
        return proveedorDAO.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cProveedorDAO proveedorDAO = new cProveedorDAO();
        return proveedorDAO.LoadTable();
    }

    public static List<cProveedor> GetProveedores()
    {
        cProveedorDAO empresaDao = new cProveedorDAO();
        return empresaDao.GetProveedores();
    }

    public static List<cProveedor> Search(string idProveedor)
    {
        cProveedorDAO DAO = new cProveedorDAO();
        return DAO.Search(idProveedor);
    }

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }

    public string Direccion
    {
        get { return direccion; }
        set { direccion = value; }
    }

    public string Telefono
    {
        get { return telefono; }
        set { telefono = value; }
    }

    public string Mail
    {
        get { return mail; }
        set { mail = value; }
    }

    public Int16 Papelera
    {
        get { return papelera; }
        set { papelera = value; }
    }
    #endregion

    #region Combo Proveedor
    public static ArrayList LoadTableProveedor()
    {
        cProveedorDAO proveedorDAO = new cProveedorDAO();
        return proveedorDAO.LoadTable();
    }

    public static DataTable GetListaProveedor()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("Nombre", typeof(string)));
        ArrayList proveedor = LoadTableProveedor();
        foreach (cProveedor p in proveedor)
        {
            tbl.Rows.Add(p.id, p.Nombre);
        }
        return tbl;
    }
    #endregion

    #region Combo
    public static ArrayList LoadTableEmpresa()
    {
        cEmpresaDAO DAO = new cEmpresaDAO();
        return DAO.LoadTable();
    }

    public static DataTable GetListaEmpresas()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("nombre", typeof(string)));
        ArrayList empresa = LoadTableEmpresa();
        tbl.Rows.Add(0, "Seleccione una empresa...");
        foreach (cEmpresa c in empresa)
        {
            tbl.Rows.Add(c.Id, c.Nombre);
        }
        return tbl;
    }

    public static ArrayList LoadTableUsuario()
    {
        cUsuarioDAO DAO = new cUsuarioDAO();
        return DAO.LoadTable();
    }

    public static DataTable GetListaUsuarios()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("nombre", typeof(string)));
        ArrayList usuario = LoadTableUsuario();
        tbl.Rows.Add(0, "Seleccione un usuario...");
        foreach (cUsuario c in usuario)
        {
            tbl.Rows.Add(c.Id, c.Nombre);
        }
        return tbl;
    }
    #endregion
}
