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

public class cUsuario
{
    private int id;
    private string nombre;
    private string usuario;
    private string clave;
    private string mail;
    private int idCategoria;

    public bool Update()
    {
        cUsuarioDAO usuarioDAO = new cUsuarioDAO();
        return usuarioDAO.Update(this);
    }

    public static cUsuario Load(int id)
    {
        cUsuarioDAO userDAO = new cUsuarioDAO();
        return userDAO.Load(id);
    }

    public static ArrayList LoadTable()
    {
        cUsuarioDAO userDAO = new cUsuarioDAO();
        return userDAO.LoadTable();
    }

    // Si el usuario y clave son correctos devuelve el ID, sino -1
    public int LoginUser(string nameUser, string password)
    {
        cUsuarioDAO userDAO = new cUsuarioDAO();
        return userDAO.LoginUser(nameUser, password);
    }

    public string GetName(int id)
    {
        cUsuarioDAO userDAO = new cUsuarioDAO();
        return userDAO.GetName(id);
    }

    public static void RegisterAccess(string idUsuario)
    {
        cUsuarioDAO usuarioDAO = new cUsuarioDAO();
        usuarioDAO.RegisterAccess(idUsuario);
    }

    public static cUsuario GetId_By_IdAsignacionResponsable(int idAsignacion)
    {
        cUsuarioDAO DAO = new cUsuarioDAO();
        return cUsuario.Load(DAO.GetId_By_IdAsignacionResponsable(idAsignacion));
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

    public int IdCategoria
    {
        get { return idCategoria; }
        set { idCategoria = value; }
    }

    public override string ToString()
    {
        return nombre.ToString();
    }
    #endregion

}
