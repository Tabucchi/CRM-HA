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

public class cCliente
{
    private int id;
    private int idEmpresa;
    private string nombre;
    private string interno;
    private string mail;
    private string ip;
    private string password;
    private string usuarioRed;
    private string passwordRed;

    public cCliente(int _idEmpresa, string _nombre, string _interno, string _mail)
    {
        idEmpresa = _idEmpresa;
        nombre = _nombre;
        interno = _interno;
        mail = _mail;
    }

    public cCliente() { }

    public int Insert()
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.Insert(this);
    }

    public bool Update()
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.Update(this);
    }

    public bool Delete()
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.Delete(this.Id.ToString());
    }

    public static cCliente Load(int id)
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.LoadTable();
    }

    public static ArrayList SearchById(int id)
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.SearchByIdEmpresa(id);
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

    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }

    public string Interno
    {
        get { return interno; }
        set { interno = value; }
    }

    public string Mail
    {
        get { return mail; }
        set { mail = value; }
    }

    public string Ip
    {
        get { return ip; }
        set { ip = value; }
    }

    public string Password
    {
        get { return password; }
        set { password = value; }
    }

    public string UsuarioRed
    {
        get { return usuarioRed; }
        set { usuarioRed = value; }
    }

    public string PasswordRed
    {
        get { return passwordRed; }
        set { passwordRed = value; }
    }

    public string GetFullPhone()
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        string fullPhone = clienteDAO.GetFullPhone(idEmpresa);
        if (interno == "")
            return fullPhone;
        return fullPhone + " int. " + interno;
    }
    #endregion
}