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

public class cComentarioCC
{
    private string id;
    private string idUsuario;
    private string idCuentaCorriente;
    private string idCuota;
    private string descripcion;
    private DateTime fecha;

    public cComentarioCC(string _idUsuario, string _idCuentaCorriente, string _idCuota, string _descripcion)
    {
        idUsuario = _idUsuario;
        idCuentaCorriente = _idCuentaCorriente;
        idCuota = _idCuota;
        descripcion = _descripcion;
        fecha = DateTime.Now;
    }

    public cComentarioCC()
    { }

    #region Acceso a Datos
    public int Save()
    {
        if (Validar())
        {
            cComentarioCCDAO DAO = new cComentarioCCDAO();
            return DAO.Save(this);
        }
        return -1;
    }

    public static ArrayList LoadTable()
    {
        cComentarioDAO DAO = new cComentarioDAO();
        return DAO.LoadTable();
    }

   /* public static ArrayList SearchByIdPedido(string idPedido, bool visibleParaCliente)
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
    }*/
    #endregion

    #region Validaciones
    public bool Validar()
    {
        try
        {
            bool flag = false;
            flag = Convert.ToInt32(this.idUsuario) <= 0 ? false : true;
            flag = string.IsNullOrEmpty(this.idUsuario) ? false : true;
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
    public string IdCuentaCorriente
    {
        get { return idCuentaCorriente; }
        set { idCuentaCorriente = value; }
    }
    public string IdCuota
    {
        get { return idCuota; }
        set { idCuota = value; }
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
    public cUsuario GetUsuario()
    {
        return cUsuario.Load(idUsuario) == null ? null : cUsuario.Load(idUsuario);
    }
    #endregion

    public static List<cComentarioCC> GetComentarios(string _idCuentaCorriente)
    {
        cComentarioCCDAO DAO = new cComentarioCCDAO();
        return DAO.GetComentarios(_idCuentaCorriente);
    }

    public static List<cComentarioCC> GetComentariosByIdCuota(string _idCuota)
    {
        cComentarioCCDAO DAO = new cComentarioCCDAO();
        return DAO.GetComentariosByIdCuota(_idCuota);
    }
}