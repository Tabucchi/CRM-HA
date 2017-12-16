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
    private int id;
    private int idUsuario;
    private int idPedido;
    private string descripcion;
    private DateTime fecha;

    public cComentario(int _idUsuario, int _idPedido, string _descripcion, DateTime _fecha)
    {
        idUsuario = _idUsuario;
        idPedido = _idPedido;
        descripcion = _descripcion;
        fecha = _fecha;
    }

    public cComentario()
    { }

    public int Insert()
    {
        cComentarioDAO comentarioDAO = new cComentarioDAO();
        return comentarioDAO.Insert(this);
    }

    public static ArrayList LoadTable()
    {
        cComentarioDAO comentarioDAO = new cComentarioDAO();
        return comentarioDAO.LoadTable();
    }

    public static ArrayList SearchByIdPedido(int id)
    {
        cComentarioDAO comentarioDAO = new cComentarioDAO();
        return comentarioDAO.SearchByIdPedido(id);
    }

    #region Propiedades
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public int IdUsuario
    {
        get { return idUsuario; }
        set { idUsuario = value; }
    }

    public int IdPedido
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
    #endregion
}