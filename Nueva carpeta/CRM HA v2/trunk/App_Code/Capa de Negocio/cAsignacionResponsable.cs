using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class cAsignacionResponsable
{
    private int id;
    private int idResponsable;
    private int idAsigno;
    private int idPedido;
    private string comentario;
    private DateTime fecha;

    public cAsignacionResponsable() { }

    public int Insert()
    {
        cAsignacionResponsableDAO DAO = new cAsignacionResponsableDAO();
        return DAO.Insert(this);
    }

    public static cAsignacionResponsable Load(int id)
    {
        cAsignacionResponsableDAO DAO = new cAsignacionResponsableDAO();
        return DAO.Load(id);
    }

    #region Propiedades
    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public int IdResponsable
    {
        get { return idResponsable; }
        set { idResponsable = value; }
    }

    public int IdAsigno
    {
        get { return idAsigno; }
        set { idAsigno = value; }
    }

    public int IdPedido
    {
        get { return idPedido; }
        set { idPedido = value; }
    }

    public string Comentario
    {
        get { return comentario; }
        set { comentario = value; }
    }

    public DateTime Fecha
    {
        get { return fecha; }
        set { fecha = value; }
    }
    #endregion
}

