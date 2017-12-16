using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

public class cAsignacionResponsable
{
    private string id;
    private string idResponsable;
    private string idAsigno;
    private string comentario;
    private DateTime fecha;
    private string idPedido;

    public cAsignacionResponsable(string _idResponsable, string _idAsigno, string _comentario)
    {
        idResponsable = _idResponsable;
        idAsigno = _idAsigno;                        
        comentario = _comentario;
        idPedido = "-1";
        fecha = DateTime.Now;
    }

    public cAsignacionResponsable() { }

    public int Insert()
    {
        cAsignacionResponsableDAO DAO = new cAsignacionResponsableDAO();
        return DAO.Insert(this);
    }

    public int Save()
    {
        cAsignacionResponsableDAO DAO = new cAsignacionResponsableDAO();
        return DAO.Save(this);
    }

    public static cAsignacionResponsable Load(string id)
    {
        cAsignacionResponsableDAO DAO = new cAsignacionResponsableDAO();
        return DAO.Load(id);
    }

    public static Dictionary<string, int> GetAsignacionesPendientesPorUsuario()
    {
        cAsignacionResponsableDAO DAO = new cAsignacionResponsableDAO();
        return DAO.GetAsignacionesPendientesPorUsuario();
    }

    public static cAsignacionResponsable GetResponsablePorPedido(string id)
    {
        cAsignacionResponsableDAO DAO = new cAsignacionResponsableDAO();
        return DAO.GetResponsablePorPedido(id);
    }

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string IdResponsable
    {
        get { return idResponsable; }
        set { idResponsable = value; }
    }

    public string IdAsigno
    {
        get { return idAsigno; }
        set { idAsigno = value; }
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

    public string IdPedido
    {
        get { return idPedido; }
        set { idPedido = value; }
    }

    public cUsuario GetResponsable()
    {
        return cUsuario.Load(this.idResponsable) == null ? null : cUsuario.Load(this.idResponsable);
    }

    public cUsuario GetAsigno()
    {
        return cUsuario.Load(this.idAsigno) == null ? null : cUsuario.Load(this.idAsigno);
    }
    #endregion
}

