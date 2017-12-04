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


public class cEmpresa
{
    private int id;
    private string nombre;

    public string Nombre1
    {
        get { return nombre; }
        set { nombre = value; }
    }
    private string direccion;
    private string telefono;
    private string datos;

    public cEmpresa(string _nombre, string _direccion, string _telefono)
    {
        nombre = _nombre;
        direccion = _direccion;
        telefono = _telefono;
        datos = "";
    }

    public cEmpresa()
    { }

    public int Insert()
    {
        cEmpresaDAO empresaMapper = new cEmpresaDAO();
        return empresaMapper.Insert(this);
    }

    public bool Update()
    {
        cEmpresaDAO empresaDAO = new cEmpresaDAO();
        return empresaDAO.Update(this);
    }

    public bool Delete()
    {
        cEmpresaDAO empresaDAO = new cEmpresaDAO();
        return empresaDAO.Delete(this.Id.ToString());
    }

    public static cEmpresa Load(int id)
    {
        cEmpresaDAO empresaDAO = new cEmpresaDAO();
        return empresaDAO.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cEmpresaDAO empresaDAO = new cEmpresaDAO();
        return empresaDAO.LoadTable();
    }

    #region Propiedades
    public int Id
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

    public string Datos
    {
        get { return datos; }
        set { datos = value; }
    }

    public override string ToString()
    {
        return nombre.ToString();
    }
    #endregion
}
