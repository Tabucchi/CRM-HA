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

public enum Autorizaciones { No_Autorizado=0, Autorizado=1};

public class cCliente : iAutorComentario
{
    private string id;
    private string idEmpresa;
    private string nombre;
    private string interno;
    private string mail;
    private string ip;
    private string password;
    private string usuarioRed;
    private string passwordRed;
    private string claveSistema;
    private Int16 autorizacion;
    private Int16 papelera;

    public cCliente(string _idEmpresa, string _nombre, string _interno, string _mail)
    {
        idEmpresa = _idEmpresa;
        nombre = _nombre;
        interno = _interno;
        mail = _mail;
    }

    public cCliente() { }

    #region Acceso a Datos
    public int Insert()
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.Save(this);
    }

    public static cCliente Load(string id)
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.LoadTable();
    }

    public static DataTable LoadTableForCombo()
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.LoadTableForCombo();
    }

    public static ArrayList SearchById(int id)
    {
        cClienteDAO clienteDAO = new cClienteDAO();
        return clienteDAO.SearchByIdEmpresa(id);
    }

    
    /// <summary>
    /// Devuelve una lista con nombre de cliente + nombre empresa.
    /// </summary>
    public static DataTable GetListaClientes()
    {
        DataTable tbl = new DataTable();
       /* tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("nombre", typeof(string)));
        ArrayList clientes = LoadTableForCombo();
        foreach (cCliente c in clientes)
        {
            tbl.Rows.Add(c.id, c.nombre);
        }*/
        return tbl;
    }

    public static cCliente SearchByNombreAndEmpresa(string nombreCliente, string nombreEmpresa)
    {
        cClienteDAO DAO = new cClienteDAO();
        return DAO.SearchByNombreAndEmpresa(nombreCliente, nombreEmpresa);
    }

    public static cCliente SearchByMail(string mail, bool activo)
    {
        cClienteDAO DAO = new cClienteDAO();
        return DAO.SearchByMail(mail, activo);
    }

    public static string[] SearchByText(string texto)
    {
        int i = 0;
        string[] items;
        cClienteDAO DAO = new cClienteDAO();
        List<cCliente> c = DAO.SearchByText(texto);
        if (c != null) { 
            items = new string[c.Count];
            foreach (cCliente x in c)
            {
                items.SetValue(x.nombre + " (" + x.GetEmpresa() + ")", i);
                i++;
            }
        }
        else
        {
            items = new string[0];
        }
        return items;
    }

    public int Save()
    {
        cClienteDAO DAO = new cClienteDAO();
        return DAO.Save(this);
    }

    public static int LoginCliente(string nameCliente, string password)
    {
        cClienteDAO clienteDao = new cClienteDAO();
        return clienteDao.LoginCliente(nameCliente, password);
    }

    public static List<cCliente> GetClientes()
    {
        cClienteDAO clienteDao = new cClienteDAO();
        return clienteDao.GetClientes();
    }
    
    public static List<cCliente> GetClientesByIdEmpresa(string idEmpresa)
    {
        cClienteDAO clienteDao = new cClienteDAO();
        return clienteDao.GetClientesByIdEmpresa(idEmpresa);
    }

    public static List<cCliente> GetClientesAutorizados(string idEmpresa)
    {
        cClienteDAO clienteDao = new cClienteDAO();
        return clienteDao.GetClientesAutorizados(idEmpresa);
    }
    #endregion

    public static Dictionary<string, string> GetClienteEmpresa()
    {
        cClienteDAO clienteDao = new cClienteDAO();
        return clienteDao.GetClienteEmpresa();
    }

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
    
    public Int16 Autorizacion
    {
        get { return autorizacion; }
        set { autorizacion = value; }
    }

    public Int16 Papelera
    {
        get { return papelera; }
        set { papelera = value; }
    }

    public string ClaveSistema
    {
        get { return claveSistema; }
        set { claveSistema = value; }
    }

    public string ClaveSistemaDecode
    {
        get { return Decode(claveSistema); }
    }

    public string GetEmpresa()
    {
       return cEmpresa.Load(this.idEmpresa) == null ? "" : cEmpresa.Load(this.idEmpresa).Nombre;
    }

    public string Empresa
    {
        get{return cEmpresa.Load(this.idEmpresa) == null ? "" : cEmpresa.Load(this.idEmpresa).Nombre;}
    }

    public bool GetAutorizacionBool {
        get{return Autorizacion == (Int16)Autorizaciones.No_Autorizado ? false : true; }
    }

    public string GetAutorizacion
    {
        get { return Autorizacion == (Int16)Autorizaciones.No_Autorizado ? Autorizaciones.No_Autorizado.ToString().Replace("_", " ") : Autorizaciones.Autorizado.ToString(); }
    }

    public override string ToString()
    {
        return Nombre;
    }
    #endregion

    public static string Codify(string _pass)
    {
        string newPass = "";
        char character;
        for (int i = 0; i < _pass.Length; i++)
        {
            character = _pass[i];
            Convert.ToInt16(character);
            newPass += Convert.ToString(character + 210);
        }
        return newPass;
    }

    public static string Decode(string _pass)
    {
        string newPass = "";
        char character;

        int i = 0;

        if (i < _pass.Length && _pass.Length != 3) //si el tamaño de la clave es mayor a 3 
        {
            while (i < _pass.Length)
            {
                //se toman de a tres valores
                string _aux = Convert.ToString(_pass[i]);
                _aux += Convert.ToString(_pass[i + 1]);
                _aux += Convert.ToString(_pass[i + 2]);

                //se obtiene el caracter
                int valorCaracter = (Convert.ToInt16(_aux) - 210);
                character = Convert.ToChar(valorCaracter);

                Convert.ToString(character);
                newPass += character;
                i = i + 3;
            }
        }
        else
        {
            /* int aux = (Convert.ToInt16(_pass) - 210);
             character = Convert.ToChar(aux);
             Convert.ToString(character);
             newPass += character;*/
        }

        return newPass;
    }
}