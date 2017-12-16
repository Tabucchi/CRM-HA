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

public class cComentario
{
    private string id;
    private string idUsuario;
    private string tipo;
    private string idPedido;
    private string descripcion;
    private DateTime fecha;
    private bool visibilidadCliente;

    public cComentario(string _idUsuario, string _tipo, string _idPedido, string _descripcion, bool _visibilidad)
    {
        idUsuario = _idUsuario;
        tipo = _tipo;
        idPedido = _idPedido;
        descripcion = _descripcion;
        fecha = DateTime.Now;
        visibilidadCliente = _visibilidad;
    }

    public cComentario()
    { }

    #region Acceso a Datos
    public int Save()
    {
        if (Validar())
        {
            cComentarioDAO DAO = new cComentarioDAO();
            return DAO.Save(this);
        }
        return -1;
    }

    public static ArrayList LoadTable()
    {
        cComentarioDAO DAO = new cComentarioDAO();
        return DAO.LoadTable();
    }

    public static ArrayList SearchByIdPedido(string idPedido, bool visibleParaCliente)
    {
        cComentarioDAO DAO = new cComentarioDAO();
        return DAO.SearchByIdPedido(idPedido, visibleParaCliente);
    }

    public static cComentario GetLastComentario(string idPedido)
    {
        cComentarioDAO DAO = new cComentarioDAO();
        return DAO.GetLastComentario(idPedido);
    }

    public static List<cComentario> GetComentarios(string idPedido)
    {
        cComentarioDAO comentarioDAO = new cComentarioDAO();
        return comentarioDAO.GetComentarios(idPedido);
    }
    #endregion

    #region Validaciones
    public bool Validar()
    {
        try
        {
            bool flag = false;
            flag = Convert.ToInt32(this.idUsuario) <= 0 ? false : true;
            flag = string.IsNullOrEmpty(this.idUsuario) ? false : true;

            flag = Convert.ToInt32(this.idPedido) <= 0 ? false : true;
            flag = string.IsNullOrEmpty(this.idPedido) ? false : true;
            return flag;
        }
        catch
        {
            return false;
        }
    }
    #endregion

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string IdUsuario
    {
        get { return idUsuario; }
        set { idUsuario = value; }
    }

    public string Tipo
    {
        get { return tipo; }
        set { tipo = value; }
    }

    public string IdPedido
    {
        get { return idPedido; }
        set { idPedido = value; }
    }

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    public bool VisibilidadCliente
    {
        get { return visibilidadCliente; }
        set { visibilidadCliente = value; }
    }

    public cUsuario GetUsuario()
    {
        return cUsuario.Load(idUsuario) == null ? null : cUsuario.Load(idUsuario);
    }

    public iAutorComentario GetAutorComentario()
    {
        return tipo == "cCliente" ? (cCliente.Load(idUsuario) == null ? null : (iAutorComentario)cCliente.Load(idUsuario)) : (iAutorComentario)cUsuario.Load(idUsuario);
    }

    public string GetNombre_Autor
    {
        get { return tipo == "cCliente" ? (cCliente.Load(idUsuario) == null ? null : cCliente.Load(idUsuario).Nombre) : cUsuario.Load(idUsuario).Nombre;}
    }

    public string GetNombreAutor()
    {
        return tipo == "cCliente" ? (cCliente.Load(idUsuario) == null ? null : cCliente.Load(idUsuario).Nombre) : cUsuario.Load(idUsuario).Nombre;
    }
    #endregion

    public static List<cComentario> GetList(string idPedido, bool visibleParaCliente)
    {
        List<cComentario> comentarios = new List<cComentario>();
        ArrayList res = SearchByIdPedido(idPedido, visibleParaCliente);

        if (res != null)
        {
            foreach (cComentario c in res)
            {
                comentarios.Add(c);
            }
        }
        return comentarios;
    }

    
}