using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using System.Data;

public enum eCategoria { Administración = 1, Gerencia = 2, Usuario = 3, Vendedor = 4 };
public enum TipoUsuario { Usuario = 1, Cliente = 2 };

public class cUsuario : iAutorComentario
{
    private string id;
    private string nombre;
    private string usuario;
    private string clave;
    private string mail;
    private Int16 idCategoria;
    private Int16 papelera;
    private string tipo;

    public cUsuario(string _nombre, string _usuario, string _clave, string _mail, Int16 _idCategoria, Int16 _papelera, string _tipo)
    {
        nombre = _nombre;
        usuario = _usuario;
        clave = _clave;
        mail = _mail;
        idCategoria = _idCategoria;
        papelera = _papelera;
        tipo = _tipo;
    }

    public cUsuario() { }

    public int Save()
    {
        cUsuarioDAO usuarioDAO = new cUsuarioDAO();
        return usuarioDAO.Save(this);
    }

    public static cUsuario Load(string id)
    {
        cUsuarioDAO userDAO = new cUsuarioDAO();
        return userDAO.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cUsuarioDAO userDAO = new cUsuarioDAO();
        return userDAO.LoadTable();
    }

    public static DataTable GetDataTable()
    {
        DataTable tbl = new DataTable();
        tbl.Columns.Add(new DataColumn("id", typeof(string)));
        tbl.Columns.Add(new DataColumn("nombre", typeof(string)));
        ArrayList valores = LoadTable();
        foreach (cUsuario u in valores)
        {
            tbl.Rows.Add(u.id, u.nombre);
        }
        return tbl;
    }

    // Si el usuario y clave son correctos devuelve el ID, sino -1
    public static int LoginUser(string nameUser, string password)
    {
        cUsuarioDAO userDAO = new cUsuarioDAO();
        return userDAO.LoginUser(nameUser, password);
    }

    public static void RegisterAccess(string idUsuario, string ip)
    {
        cUsuarioDAO usuarioDAO = new cUsuarioDAO();
        usuarioDAO.RegisterAccess(idUsuario, ip);
    }

    public static void InformationAccess(string idUsuario, string empresa, string ip)
    {
        cUsuarioDAO usuarioDAO = new cUsuarioDAO();
        usuarioDAO.InformationAccess(idUsuario, empresa, ip);
    }

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

    public static List<cUsuario> GetUsuarios()
    {
        cUsuarioDAO usuarioDao = new cUsuarioDAO();
        return usuarioDao.GetUsuarios();
    }

    public static string GetUsuarioByName(string nombre)
    {
        cUsuarioDAO usuarioDao = new cUsuarioDAO();
        return usuarioDao.GetUsuarioByName(nombre);
    }

    public static string GetLastRegistry(DateTime fecha)
    {
        cUsuarioDAO usuarioDao = new cUsuarioDAO();
        return usuarioDao.GetLastRegistry(fecha);
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

    public string Usuario
    {
        get { return usuario; }
        set { usuario = value; }
    }

    public string Clave
    {
        get { return clave; }
        set { clave = value; }
    }

    public string Mail
    {
        get { return mail; }
        set { mail = value; }
    }

    public Int16 IdCategoria
    {
        get { return idCategoria; }
        set { idCategoria = value; }
    }

    public Int16 Papelera
    {
        get { return papelera; }
        set { papelera = value; }
    }

    public string GetCategoria
    {
        get { return cCampoGenerico.Load(IdCategoria.ToString(), Tablas.tCategoria).Descripcion; }
        set { tipo = value; }
    }

    public override string ToString()
    {
        return nombre.ToString();
    }
    #endregion
}
